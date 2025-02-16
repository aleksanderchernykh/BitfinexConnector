using BitfinexConnector.Infrastructure.Model;

namespace BitfinexConnector.Infrastructure.Interfaces
{
    /// <summary>
    /// Web Api коннектер.
    /// </summary>
    public interface IConnectorWebApi
    {
        /// <summary>
        /// Получение информации о валютных парах.
        /// </summary>
        /// <param name="tradingPairs">Валютные пары.</param>
        /// <returns>Информация о валютных парах. </returns>
        Task<IEnumerable<Ticker>> GetTickersByTradingPairs(string[] tradingPairs);

        Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount);
        Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to = null, long? count = 0);
    }
}
