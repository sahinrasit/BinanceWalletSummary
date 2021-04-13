using Binance.API.BinanceNet;
using Binance.API.DTO;
using Binance.API.Extensions;
using Binance.Net.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Binance.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BinanceController : ControllerBase
    {
        private readonly IBinanceClient _binanceClient;
        //private readonly IBinanceDataProvider _dataProvider;
        private readonly BinanceCoreService _binanceCoreService;

        public BinanceController(IBinanceClient client)
        {
            _binanceClient = client;
            _binanceCoreService = new BinanceCoreService(_binanceClient);
        }
        [HttpGet]
        [Route("MyAccountInfo")]
        public IActionResult MyAccountInfo()
        {
            return Ok(_binanceCoreService.MyAccountInfo());
        }
        [HttpGet]
        [Route("MyTradeList")]
        public IActionResult MyTradeList()
        {          
            List<MyTradeDto> myTradeListSession;
            myTradeListSession = HttpContext.Session.Get<List<MyTradeDto>>("trade");
            if (myTradeListSession==null ||myTradeListSession.Count<1)
            {
                myTradeListSession = _binanceCoreService.MyTradeList();
                HttpContext.Session.Set<List<MyTradeDto>>("trade", myTradeListSession);
            }
            var smyTradeListSession = HttpContext.Session.Get<List<MyTradeDto>>("trade");
            return Ok(myTradeListSession);
        }

        [HttpGet]
        [Route("MyAccountTotalInfo")]
        public IActionResult MyAccountTotalInfo()
        {
            return Ok(_binanceCoreService.MyAccountTotalInfo());
        }
    }
}
