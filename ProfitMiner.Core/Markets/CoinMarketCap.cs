using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProfitMiner.Core.Interfaces;

namespace ProfitMiner.Core
{
    internal class CoinMarketCap : IMarket
    {
        public async Task<MarketInfo[]> GetMarketSummaries()
        {
            var marketData = await GetMarketInfo();

            var result = marketData.Select(t => new MarketInfo
            {
                CoinCode = t.Symbol,
                CoinName = t.Name,

                VolumeUsd24H = t.VolumeUsd24H ?? 0d,
                VolumeBtc24H = (t.VolumeUsd24H ?? 0d) * (t.Symbol.Equals("BTC", StringComparison.OrdinalIgnoreCase) ? (1d / (t.PriceUsd ?? 0d)) : (t.PriceBtc ?? 0d))
            }).ToArray();

            return result;
        }

        private async Task<CoinMarketInfo[]> GetMarketInfo()
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync("https://api.coinmarketcap.com/v1/ticker/?limit=0&convert=USD");
                var json = await result.Content.ReadAsStringAsync();
                return json.FromJson<CoinMarketInfo[]>();
            }
        }

        private class CoinMarketInfo
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Symbol { get; set; }
            public int Rank { get; set; }
            [JsonProperty("price_usd")]
            public double? PriceUsd { get; set; }
            [JsonProperty("price_btc")]
            public double? PriceBtc { get; set; }
            [JsonProperty("24h_volume_usd")]
            public double? VolumeUsd24H { get; set; }
            [JsonProperty("market_cap_usd")]
            public double? MarketCapUsd { get; set; }
            [JsonProperty("available_supply")]
            public double? AvailableSupply { get; set; }
            [JsonProperty("total_supply")]
            public double? TotalSupply { get; set; }
            [JsonProperty("percent_change_1h")]
            public double? PercentChange1H { get; set; }
            [JsonProperty("percent_change_24h")]
            public double? PercentChange24H { get; set; }
        }
    }

   
}
