using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProfitMiner.Core.Interfaces;

namespace ProfitMiner.Core.Pools.ZPool
{
    public class ZPoolMiningPool : IMiningPool
    {
        public bool Supports24HEstimation { get; }

        public ZPoolMiningPool()
        {
            Supports24HEstimation = true;
        }

        public async Task<List<MiningPoolWalletState>> GetWalletState(string wallet)
        {
            var walletState = await QueryApi<WalletState>("api/wallet?address=" + wallet);

            if (walletState == null)
                return new List<MiningPoolWalletState>(0);

            return new List<MiningPoolWalletState>
            {
                new MiningPoolWalletState
                {
                    Coin = walletState.currency,
                    Unsold = walletState.unsold,
                    Balance = walletState.balance,
                    Unpaid = walletState.unpaid,
                    Total = walletState.total
                }
            };
        }

        public async Task<List<MiningPoolAlogo>> GetAlgos()
        {
            var poolStatus = await QueryApi<Dictionary<string, PoolStatus>>("api/status");
            var coinsStatus = await QueryApi<Dictionary<string, Currency>>("api/currencies");

            if (poolStatus == null)
                return null;

            var coins = coinsStatus?.GroupBy(t => t.Value.algo).ToDictionary(t => t.Key, t => string.Join(",", t.Select(x => x.Key)), StringComparer.OrdinalIgnoreCase);

            if (coins == null)
                coins = new Dictionary<string, string>();

            return poolStatus
                .Select(t => new MiningPoolAlogo
                {
                    Name = t.Value.name,
                    Coin = coins.ContainsKey(t.Value.name) ? coins[t.Value.name] : "qty:" + t.Value.coins.ToString(),
                    Fee = t.Value.fees,
                    MinersCount = t.Value.workers,
                    Port = t.Value.port,
                    Protocol = "stratum",
                    Server = $"tcp://{t.Value.name}.mine.zpool.ca"
                })
                .ToList();
        }

        private async Task<T> QueryApi<T>(string requestUri)
        {
            using (var client = new HttpClient { BaseAddress = new Uri("http://www.zpool.ca"), Timeout = TimeSpan.FromSeconds(5) })
            {
                client.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");

                var result = await client.GetAsync(requestUri);
                result.EnsureSuccessStatusCode();

                var jsonStr = await result.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(jsonStr);
            }
        }

        private class WalletState
        {
            public string currency { get; set; }
            public double unsold { get; set; }
            public double balance { get; set; }
            public double unpaid { get; set; }
            public double paid24h { get; set; }
            public double total { get; set; }
        }

        private class PoolStatus
        {
            public string name { get; set; }
            public int port { get; set; }
            public int coins { get; set; }
            public double fees { get; set; }
            public double hashrate { get; set; }
            public int workers { get; set; }
            public double estimate_current { get; set; }
            public double estimate_last24h { get; set; }
            public double actual_last24h { get; set; }
            public double hashrate_last24h { get; set; }
        }

        public class Currency
        {
            public string algo { get; set; }
            public int port { get; set; }
            public string name { get; set; }
            public int height { get; set; }
            public int workers { get; set; }
            public int shares { get; set; }
            public double hashrate { get; set; }
            [JsonProperty(PropertyName = "24h_blocks")]
            public int blocks_24h { get; set; }
            [JsonProperty(PropertyName = "24h_btc")]
            public double btc_24h { get; set; }
            public int lastblock { get; set; }
            public int timesincelast { get; set; }
        }
    }
}