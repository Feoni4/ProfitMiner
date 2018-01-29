using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ProfitMiner.Core;
using ProfitMiner.Core.Pools.ZPool;

namespace ProfitMiner.Client.Jobs
{
    public class WalletLogginJob
    {
        public static async Task Execute()
        {
            var p = new ZPoolMiningPool();

            var wallet = await p.GetWalletState("176rJXonVnHjNrtHpZGXbbvLqZcFM8xFsh");
            var item = wallet.FirstOrDefault();
            if (item == null)
                return;

            var state = new
            {
                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                item.Unsold,
                item.Balance,
                item.Unpaid,
                item.Total
            };

            File.AppendAllLines("176rJXonVnHjNrtHpZGXbbvLqZcFM8xFsh.log", new[] { state.ToJson() });
        }
    }
}