using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProfitMiner.Core.Interfaces;

namespace ProfitMiner.Core.Markets
{
    public class Market : IMarket
    {
        readonly IMarket[] _markets;

        public Market()
        {
            _markets = new IMarket[] { new CoinMarketCap(), new Coinexchange() };
        }

        public async Task<MarketInfo[]> GetMarketSummaries()
        {
            List<MarketInfo> marketInfos = new List<MarketInfo>();

            foreach (var market in _markets)
            {
                var mi = await market.GetMarketSummaries();
                marketInfos.AddRange(mi);
            }

            FillMarketVolumes(marketInfos);

            return marketInfos
                .GroupBy(t => t.CoinCode)
                .Select(t =>
                {
                    var mis = t.ToArray();

                    return new MarketInfo
                    {
                        CoinCode = mis[0].CoinCode,
                        CoinName = mis[0].CoinName,
                        VolumeUsd24H = mis.Sum(x => x.VolumeUsd24H),
                        VolumeBtc24H = mis.Sum(x => x.VolumeBtc24H),
                        TradeCount24H = mis.Sum(x => x.TradeCount24H)
                    };
                })
                .ToArray();
        }

        private void FillMarketVolumes(List<MarketInfo> marketInfos)
        {
            var btc = marketInfos
                .Where(t => t.CoinCode.Equals("BTC", StringComparison.OrdinalIgnoreCase) && t.VolumeBtc24H > 0d && t.VolumeUsd24H > 0d)
                .OrderByDescending(t => t.VolumeUsd24H / t.VolumeBtc24H)
                .FirstOrDefault();


            var btc2 = marketInfos
                .Where(t => t.CoinCode.Equals("BTC", StringComparison.OrdinalIgnoreCase)).ToArray();

            if (btc == null)
                return;

            var btc_usd = btc.VolumeUsd24H / btc.VolumeBtc24H;

            foreach (var marketInfo in marketInfos)
            {
                if (marketInfo.VolumeUsd24H == 0d && marketInfo.VolumeBtc24H > 0d)
                {
                    marketInfo.VolumeUsd24H = marketInfo.VolumeBtc24H * btc_usd;
                }
                else if (marketInfo.VolumeBtc24H == 0d && marketInfo.VolumeUsd24H > 0d)
                {
                    marketInfo.VolumeBtc24H = marketInfo.VolumeUsd24H / btc_usd;
                }
            }
        }
    }
}