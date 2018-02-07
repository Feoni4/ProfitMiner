using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProfitMiner.Core.Interfaces;

namespace ProfitMiner.Core.Markets
{
    internal class Coinexchange : IMarket
    {
        public async Task<MarketInfo[]> GetMarketSummaries()
        {
            var markets = await QueryCoinexchange<CoinexchangeMarket[]>("https://www.coinexchange.io/api/v1/getmarkets");
            var summaries = await QueryCoinexchange<CoinexchangeMarketInfo[]>("https://www.coinexchange.io/api/v1/getmarketsummaries");

            return summaries
                .Join(markets, l => l.Id, r => r.Id, (summary, market) => new {summary, market})
                .GroupBy(t => t.market.MarketAssetCode)
                .Select(t =>
                {
                    var info = t.ToArray();

                    return new MarketInfo
                    {
                        CoinCode = info[0].market.MarketAssetCode,
                        CoinName = info[0].market.MarketAssetName,

                        VolumeUsd24H = 0d,
                        VolumeBtc24H = info.Sum(x => x.summary.BtcVolume ?? 0),
                        TradeCount24H = info.Sum(x => x.summary.TradeCount)
                    };
                })
                .ToArray();
        }

        private async Task<T> QueryCoinexchange<T>(string uri)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();

                var result = json.FromJson<CoinexchangeResponse<T>>();

                return result.Result;
            }
        }

        private class CoinexchangeResponse<T>
        {
            public T Result { get; set; }
        }

        private class CoinexchangeMarket
        {
            [JsonProperty("MarketID")]
            public int Id { get; set; }
            public string MarketAssetCode { get; set; }
            public string MarketAssetName { get; set; }

            public bool Active { get; set; }
        }

        private class CoinexchangeMarketInfo
        {
            [JsonProperty("MarketID")]
            public int Id { get; set; }
            public double? BtcVolume { get; set; }
            public int TradeCount { get; set; }
        }
    }
}