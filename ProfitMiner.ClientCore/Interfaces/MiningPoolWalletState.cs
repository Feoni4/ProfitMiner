namespace ProfitMiner.ClientCore.Interfaces
{
    public class MiningPoolWalletState
    {
        public string Coin { get; set; }
        public double Balance { get; set; }
        public double Unsold { get; set; }
        public double Unpaid { get; set; }
        public double Total { get; set; }
    }
}