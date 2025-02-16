using BitfinexConnector.Connector;
using BitfinexConnector.Infrastructure.Interfaces;
using BitfinexConnector.Infrastructure.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace BitfinexConnector.IntegrationTests
{
    public class ConnectorWebApiTests
    {
        private readonly IConnectorWebApi _connector;

        public ConnectorWebApiTests()
        {
            var options = Options.Create(new ConnectorOptions
            {
                BitfinexURL = "https://api-pub.bitfinex.com/v2/"
            });

            var loggerMock = new Mock<ILogger<ConnectorWebApi>>();
            _connector = new ConnectorWebApi(options, loggerMock.Object);
        }

        [Fact]
        public async Task GetTickersByTradingPairs_ShouldReturnValidData()
        {
            var tradingPairs = new[] 
            { 
                "BTCUSD", 
                "ETHUSD" 
            };

            var tickers = await _connector.GetTickersByTradingPairs(tradingPairs);

            Assert.NotEmpty(tickers);
            Assert.All(tickers, ticker =>
            {
                Assert.NotNull(ticker.SYMBOL);
                Assert.True(ticker.BID > 0);
            });
        }

        [Fact]
        public async Task GetTickersByTradingPairs_ShouldThrowException_WhenNullPair()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _connector.GetTickersByTradingPairs(null));
        }

        [Fact]
        public async Task GetTickersByTradingPairs_ShouldThrowException_WhenInvalidPair()
        {
            var tradingPairs = new[] 
            { 
                "INVALIDPAIR" 
            };

            var exception = await Assert.ThrowsAsync<Exception>(() => _connector.GetTickersByTradingPairs(tradingPairs));
        }
    }
}
