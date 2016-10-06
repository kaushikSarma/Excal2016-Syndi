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
using SearchLibrary;

namespace Syndi2._0
{
    /// <summary>
    /// Interaction logic for SearchPage.xaml
    /// </summary>
    public partial class SearchPage : Page
    {
        public SearchPage()
        {
            InitializeComponent();
            SearchField.SearchQuery.KeyUp += (sender, ex) => this.CheckEnterQuery(sender, ex);
        }
        public void CheckEnterQuery(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Console.WriteLine(SearchField.SearchQuery.Text);
                SearchFolder(SearchField.SearchQuery.Text);
            }
        }

        public void SearchFolder(string query)
        {
            List<List<string>> PcList = NetworkScanner.Scan.RetrievePCNames();
            List<string> SearchList = Search.GetSearchList(PcList[0],query);
            SearchContainer.Children.Clear();
            foreach (var item in SearchList)
            {
                Console.WriteLine("Item" + item);
                SearchContainer.Children.Add(new ExplorerTile(item, query));
            }
        }
    }
}
