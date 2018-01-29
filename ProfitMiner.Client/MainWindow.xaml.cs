using System;
using System.Collections.Generic;
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
using ProfitMiner.Core;
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

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            Jobs.JobConfig.Configue();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var z = new ZPoolMiningPool();
            var xxx = await z.GetAlgos();

            var x = xxx.ToJson(true);//Zoe
            throw  new Exception("fatality");

        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            Console.Write("Eppp");
        }
    }
}
