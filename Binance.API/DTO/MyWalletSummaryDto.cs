namespace Binance.API.DTO
{
    public class MyWalletSummaryDto
    {
        public string Asset { get; set; }
        public decimal Free { get; set; }
        public decimal Locked { get; set; }
        public decimal UnitValueUsdt { get; set; }
        public decimal UsdtTotal { get; set; }
        public decimal PortfolioPercentage { get; set; }
    }
}
