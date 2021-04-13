using Binance.API.DTO;
using Binance.Net.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Binance.API.BinanceNet
{
    public class BinanceCoreService
    {
        private readonly IBinanceClient _binanceClient;
        //private readonly IBinanceDataProvider _dataProvider;
        public BinanceCoreService(IBinanceClient client)
        {
            _binanceClient = client;
            //_dataProvider = dataProvider;
            _binanceClient.SetApiCredentials("apiKey", "apiSecret");
        }
        public List<MyWalletSummaryDto> MyAccountInfo()
        {
            List<MyWalletSummaryDto> MyWalletSummaryDtos = new List<MyWalletSummaryDto>();
            foreach (var item in _binanceClient.General.GetAccountInfo().Data.Balances.Where(x => x.Total > 0).ToList())
            {
                MyWalletSummaryDto myWalletSummaryDto = new MyWalletSummaryDto
                {
                    Asset = item.Asset,
                    Free = item.Free,
                    Locked = item.Locked,
                    UnitValueUsdt = item.Asset == "USDT" ? 1 : _binanceClient.Spot.Market.GetBookPrice(item.Asset + "USDT").Data.BestBidPrice,
                    UsdtTotal = item.Asset == "USDT" ? ((item.Free + item.Locked)) : ((item.Free + item.Locked) * _binanceClient.Spot.Market.GetBookPrice(item.Asset + "USDT").Data.BestBidPrice)
                };
                MyWalletSummaryDtos.Add(myWalletSummaryDto);
            }          
            foreach (var item in MyWalletSummaryDtos)
            {
                item.PortfolioPercentage = (item.UsdtTotal / MyWalletSummaryDtos.Sum(x => x.UsdtTotal)) * 100;
            }

            MyWalletSummaryDto myWalletSummaryTotalDto = new MyWalletSummaryDto
            {
                Asset = "TOTAL",
                UsdtTotal = MyWalletSummaryDtos.Sum(x => x.UsdtTotal),
                Free= MyWalletSummaryDtos.Sum(x => x.UsdtTotal),
                Locked=0,
                PortfolioPercentage=100,
                UnitValueUsdt=1
            };
            MyWalletSummaryDtos.Add(myWalletSummaryTotalDto);
            return MyWalletSummaryDtos;
        }
        
        public List<MyTradeDto> MyTradeList()
        {
            //var assets = _binanceClient.General.GetUserCoins().Data.Select(x=>x.Coin).ToList<string>(); Tüm Coinler çok uzun sürüyor
            var assets = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("CoinList:MyCoinList").Get<List<string>>();
            List<MyTradeDto> myTrades = new List<MyTradeDto>();
            var currency = "";
            foreach (var coinName in assets)
            {
                currency = "USDT";
                var tradeList = _binanceClient.Spot.Order.GetMyTrades(coinName+ currency).Data;
                if (tradeList != null && tradeList.Count() == 0)
                {
                    currency = "TRY";
                    tradeList = _binanceClient.Spot.Order.GetMyTrades(coinName + currency).Data;
                }
                if (tradeList != null && tradeList.Count() == 0)
                {
                    currency = "BTC";
                    tradeList = _binanceClient.Spot.Order.GetMyTrades(coinName + currency).Data;
                }
                if (tradeList!=null &&tradeList.Count() != 0)
                {
                    var accountInfo = _binanceClient.General.GetAccountInfo().Data.Balances.Where(x => x.Total > 0).ToList();
                    MyTradeDto myTrade = new MyTradeDto
                    {
                        TotalBuyQuantity = tradeList.Where(x => x.IsBuyer == true).Sum(x => x.Quantity),
                        TotalBuyPrice = tradeList.Where(x => x.IsBuyer == true).Sum(x => x.Quantity * x.Price),
                        TotalSellQuantity = tradeList.Where(x => x.IsBuyer == false).Sum(x => x.Quantity),
                        TotalSellPrice = tradeList.Where(x => x.IsBuyer == false).Sum(x => x.Quantity * x.Price),
                        Asset = coinName,
                        Currency= currency
                    };
                    if (accountInfo.FirstOrDefault(x => x.Asset == coinName) != null)
                    {
                        myTrade.TotalAccountPrice = accountInfo.First(x => x.Asset == coinName).Total * _binanceClient.Spot.Market.GetBookPrice(coinName + currency).Data.BestBidPrice;
                        myTrade.TotalAccountQuantity = accountInfo.First(x => x.Asset == coinName).Free + accountInfo.First(x => x.Asset == coinName).Locked;
                    }
                    myTrade.Profit = (myTrade.TotalSellPrice - myTrade.TotalBuyPrice) + myTrade.TotalAccountPrice;
                    myTrades.Add(myTrade);
                }      
            }
            return myTrades;
        }
        public MyWalletSummaryDto MyAccountTotalInfo()
        {
            
            MyWalletSummaryDto myWalletSummaryTotalDto = new MyWalletSummaryDto
            {
                Asset = "TOTAL",
                UsdtTotal = MyAccountInfo().Sum(x => x.UsdtTotal)
            };
            return myWalletSummaryTotalDto;
        }
    }
}
