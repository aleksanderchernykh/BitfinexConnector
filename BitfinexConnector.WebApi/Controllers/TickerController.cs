using BitfinexConnector.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BitfinexConnector.WebApi.Controllers
{
    [ApiController]
    [Route("ticker")]
    public class TickerController(IConnectorWebApi connectorWebApi)
        : ControllerBase
    {
        private readonly IConnectorWebApi _connectorWebApi = connectorWebApi
            ?? throw new ArgumentNullException(nameof(connectorWebApi));

        [HttpGet("get")]
        public async Task<IActionResult> GetTickersByTradingPairs([FromQuery] string[] tradingPairs)
        {
            try
            {
                return Ok(await _connectorWebApi.GetTickersByTradingPairs(tradingPairs));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
