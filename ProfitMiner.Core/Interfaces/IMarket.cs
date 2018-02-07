using System.Threading.Tasks;

namespace ProfitMiner.Core.Interfaces
{
    public interface IMarket
    {
        Task<MarketInfo[]> GetMarketSummaries();
    }

    public class MarketInfo
    {
        public string CoinCode { get; set; }
        public string CoinName { get; set; }

        public double VolumeBtc24H { get; set; }
        public double VolumeUsd24H { get; set; }
        public double TradeCount24H { get; set; }
    }
}