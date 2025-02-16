using BitfinexConnector.Infrastructure.Interfaces;
using BitfinexConnector.Infrastructure.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BitfinexConnector.Connector
{
    /// <summary>
    /// Коннектор для работы с WebApi Bitfinex.
    /// </summary>
    public class ConnectorWebApi : Connector, IConnectorWebApi
    {
        /// <summary>
        /// Клиент Bitfinex.
        /// </summary>
        private HttpClient BitfinexClient = new();

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="connectorOptions">Параметры подключения к Bitfinex. </param>
        /// <param name="logger">Логгер. </param>
        public ConnectorWebApi(IOptions<ConnectorOptions> connectorOptions, ILogger<Connector> logger) 
            : base(connectorOptions, logger)
        {
            var bitfinexUrl = connectorOptions.Value.BitfinexURL;

            if (string.IsNullOrEmpty(bitfinexUrl))
                throw new ArgumentException("Bitfinex URL is not configured.");

            BitfinexClient.BaseAddress = new Uri(bitfinexUrl);
            BitfinexClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        /// <summary>
        /// Получение информации о валютных парах.
        /// </summary>
        /// <param name="tradingPairs">Валютные пары.</param>
        /// <returns>Информация о валютных парах. </returns>
        /// <exception cref="ArgumentNullException">Передан пустой массив с параметрами. </exception>
        /// <exception cref="ArgumentException">Ошибка в данных переданного массива.</exception>
        public virtual async Task<IEnumerable<Ticker>> GetTickersByTradingPairs(string[] tradingPairs)
        {
            if (tradingPairs is null || tradingPairs.Length < 1)
                throw new ArgumentNullException(nameof(tradingPairs), "An empty collection has been transferred");

            if (tradingPairs.Any(string.IsNullOrEmpty))
                throw new ArgumentException("Сollection has an empty row", nameof(tradingPairs));

            try
            {
                return await GetTickerByRequest(tradingPairs);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message);
                throw;
            }
        }

        public Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to = null, long? count = 0)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получение информации о валютных парах.
        /// </summary>
        /// <param name="tradingPairs">Валютные пары.</param>
        /// <returns>Информация о валютных парах. </returns>
        /// <exception cref="Exception">Ошибка при отправке запроса. </exception>
        private async Task<IEnumerable<Ticker>> GetTickerByRequest(string[] tradingPairs)
        {
            HttpResponseMessage response = await BitfinexClient.GetAsync($"tickers?symbols={string.Join(",", tradingPairs.Select(tradingPair => $"t{tradingPair}"))}");
            if(!response.IsSuccessStatusCode)
                throw new Exception($"HTTP request failed with status code: {response.StatusCode}");

            var responseContent = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(responseContent))
                throw new Exception("Empty response from the API");

            if (responseContent == "[]")
                throw new Exception("No data found for the given trading pairs");

            var tickers = JsonConvert.DeserializeObject<List<List<object>>>(responseContent);
            if (tickers is null || tickers.Count == 0)
                throw new Exception($"The collection tickers is empty");

            var resultTickers = new List<Ticker>();

            foreach (var tickerData in tickers)
            {
                var ticker = new Ticker
                {
                    SYMBOL = tickerData[0].ToString().Substring(1),
                    BID = Convert.ToSingle(tickerData[1]),
                };

                resultTickers.Add(ticker);
            }

            return resultTickers;
        }
    }
}
