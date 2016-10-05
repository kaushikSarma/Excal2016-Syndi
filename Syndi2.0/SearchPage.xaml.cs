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
            SearchFolder();
        }
        public void SearchFolder()
        {
            List<string> pcList = new List<string>();
            pcList.Add("THOUGHT-PLANE-0");
            List<string> SearchList = Search.GetSearchList(pcList,"motivate");
            Console.WriteLine("_____________________Starting Search____________________");
            foreach (var item in SearchList)
            {
                Console.WriteLine("Item" + item);
                SearchContainer.Children.Add(new ExplorerTile(item));
            }
            Console.WriteLine("_____________________End Search_______________________");

        }
    }
}
