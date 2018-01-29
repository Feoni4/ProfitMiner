using System.Collections.Generic;

namespace ProfitMiner.Core.Interfaces
{
    public class Miner
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string ExtractPath { get; set; }
        public string ExePath { get; set; }

        public bool IsDual { get; set; }
        public MiningAlgo[] Algos { get; set; }
        public Dictionary<string, string> Arguments { get; set; }

        public Miner()
        {
            IsDual = false;
        }

        public object GetRunVariants()
        {
            return null;
        }
    }
}