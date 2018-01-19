namespace ProfitMiner.ClientCore.Interfaces
{
    public class MiningPoolAlogo
    {
        public string Name { get; set; }
        public string Protocol { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public double Fee { get; set; }
        public string Coin { get; set; }
        public int MinersCount { get; set; }
    }
}