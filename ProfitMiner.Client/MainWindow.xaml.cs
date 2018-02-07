using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CsvHelper;
using ProfitMiner.Core;
using ProfitMiner.Core.Markets;
using ProfitMiner.Core.Pools.ZPool;

namespace ProfitMiner.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            Jobs.JobConfig.Configue();

            
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var xzzz = new Market();
            var coinMarketInfos = await xzzz.GetMarketSummaries();

            var z = new ZPoolMiningPool();
            var xxx = await z.GetAlgos();

            List<Tuple<string, string, double?>> coinCap =new List<Tuple<string, string, double?>>();

            foreach (var alogo in xxx)
            {
                foreach (var coin in alogo.Coin.Split(','))
                {
                    var marketInfo = coinMarketInfos.FirstOrDefault(t => coin.Equals(t.CoinCode, StringComparison.OrdinalIgnoreCase));
                    if (marketInfo == null && coin.Contains("-"))
                    {
                        var coin2 = coin.Split('-')[0];
                        marketInfo = coinMarketInfos.FirstOrDefault(t => coin2.Equals(t.CoinCode, StringComparison.OrdinalIgnoreCase));
                    }

                    var item = new Tuple<string, string, double?>(alogo.Name, coin, marketInfo?.VolumeUsd24H);
                    coinCap.Add(item);
                }
            }

            using (var fs = File.OpenWrite("D:\\coins.csv"))
            using(var fsw = new StreamWriter(fs))
            using (var csv = new CsvHelper.CsvWriter(fsw))
            {
                csv.Configuration.Delimiter = ";";
                //csv.WriteRecord(new[] {"Algo", "Coin", "VolumeUsd24H" });
                csv.WriteRecords(coinCap);
            }


            var x = xxx.ToJson(true);//Zoe
            throw  new Exception("fatality");

        }

        //private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        //{
        //    Console.Write("Eppp");
        //}
    }


}
