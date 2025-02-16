using BitfinexConnector.Infrastructure.Interfaces;
using BitfinexConnector.Test.WebApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace BitfinexConnector.Test.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BitfinexController(IConnectorWebApi connectorWebApi) 
        : ControllerBase
    {
        private readonly IConnectorWebApi _connectorWebApi = connectorWebApi 
            ?? throw new ArgumentNullException(nameof(connectorWebApi));

        [HttpGet]
        public async Task<IActionResult> GetTickerByTradingPairs(TradingPairsDTO tradingPairsDTO)
        {
            try
            {
                return Ok(await _connectorWebApi.GetTickerByTradingPairs(tradingPairsDTO.TradingPairs));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
