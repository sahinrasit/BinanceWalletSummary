using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Binance.API.DTO
{
    public class MyTradeDto
    {
        public decimal TotalBuyQuantity { get; set; }
        public decimal TotalBuyPrice { get; set; }
        public decimal TotalSellQuantity { get; set; }
        public decimal TotalSellPrice { get; set; }
        public decimal TotalAccountQuantity { get; set; }
        public decimal TotalAccountPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal Profit { get; set; }
        public string Currency { get; set; }
        public string Asset { get; set; }
    }
}
