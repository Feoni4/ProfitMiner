using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProfitMiner.ClientCore.Interfaces;
using ProfitMiner.Core.Pools.ZPool;

namespace ProfitMiner.ClientCore.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async void TestMethod1()
        {
            var x = new ZPoolMiningPool();

            var xxx = await x.GetAlgos();

            //List<MiningPoolAlogo> xxx;
            //while (true)
            //{
            //    xxx = x.GetAlgos().Result;
            //    if (xxx != null)
            //        break;

            //    Thread.Sleep(10000);
            //}

            var xre = string.Join("\t", xxx/*.Where(t => t.MinersCount > 100)*/.Select(t => t.Name + ":" + t.MinersCount).OrderBy(t => t));
        }

        [TestMethod]
        public void WalletState()
        {
            var x = new ZPoolMiningPool();
            var wallet = x.GetWalletState("176rJXonVnHjNrtHpZGXbbvLqZcFM8xFsh").Result;
        }
    }
}