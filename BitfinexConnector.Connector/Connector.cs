using BitfinexConnector.Infrastructure.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BitfinexConnector.Connector
{
    /// <summary>
    /// Базовый класс для коннектора.
    /// </summary>
    /// <param name="connectorOptions">Параметры подключения. </param>
    /// <param name="logger">Логгер. </param>
    public abstract class Connector(IOptions<ConnectorOptions> connectorOptions,
        ILogger<Connector> logger)
    {
        /// <summary>
        /// Настройки для подключения к бирже.
        /// </summary>
        protected readonly ConnectorOptions _connectorOptions = connectorOptions.Value;

        /// <summary>
        /// Логгер.
        /// </summary>
        protected readonly ILogger<Connector> _logger = logger;
    }
}
