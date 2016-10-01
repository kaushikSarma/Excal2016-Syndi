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
    
namespace Syndi2._0
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            string netBiosName = System.Environment.MachineName;
            List < List < string >> PcList = NetworkScanner.Scan.RetrievePCNames();
            PcName.Text = netBiosName;
            NumberOfConnections.Text = ((PcList[0].Count + PcList[1].Count) < 10 ? "0" : "") + (PcList[0].Count + PcList[1].Count).ToString();
            string s = "";
            foreach (List<string> L in PcList)
            {
                foreach(string name in L)
                {
                    s += "\n" + name;
                }
            }
        }
    }
}
