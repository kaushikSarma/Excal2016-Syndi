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
            List<List<string>> PcList = InitializePcList();
            SearchField.SearchButton.Click += (sender, ex) => this.SearchButtonClick(sender, ex);
            SearchField.SearchQuery.KeyUp += (sender, ex) => this.CheckEnterQuery(sender, ex);
            InitializePcCombo(PcList);
        }
        public void InitializePcCombo(List<List<string>> PC)
        {
            CheckBox c = new CheckBox();
            SolidColorBrush ItemsColor = new SolidColorBrush(Color.FromArgb(80, 255, 255, 250));
            c.FontSize = 20;
            c.Foreground = ItemsColor;
            c.Content = "Select all" ;
            c.Click += (sender1, e) => this.SelectItem(sender1, e);
            SelectPC.Items.Add(c);
            foreach (string PCName in PC[0])
            {
                c = new CheckBox();
                c.Content = PCName;
                c.Foreground = ItemsColor;
                c.Click += (sender1, e) => this.SelectItem(sender1, e);
                SelectPC.Items.Add(c);
            }
        }
        public void SelectItem(object sender, RoutedEventArgs e)
        {
            CheckBox c = (CheckBox)sender;

            if((bool)c.IsChecked) {
                if(c.Content.ToString() == "Select all")
                {
                    foreach (CheckBox check in SelectPC.Items)
                    {
                        check.IsChecked = true;
                    }
                }
            }
            else
            {
                if (c.Content.ToString() == "Select all")
                {
                    foreach (CheckBox check in SelectPC.Items)
                    {
                        check.IsChecked = false;
                    }
                }
            }
        }
        public List<List<string>> InitializePcList()
        {
            return NetworkScanner.Scan.RetrievePCNames();
        }

        public void CheckEnterQuery(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchFolder(SearchField.SearchQuery.Text);
            }
        }

        public async void SearchFolder(string query)
        {
            await Task.Delay(10);
            List<string> PcList = new List<string>();
            SearchContainer.Children.Clear();
            foreach (var item in SelectPC.Items)
            {
                CheckBox ite= (CheckBox)item;
                if(ite.IsChecked == true && ite.Content.ToString() != "Select all")
                {
                    PcList.Add(ite.Content.ToString());
                }
            }
            List<string> SearchList = Search.GetSearchList(PcList, query);
            foreach(var item in SearchList)
            {
                SearchContainer.Children.Add(new ExplorerTile(item, query));
            }
        }

        public void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            SearchFolder(SearchField.SearchQuery.Text);
        }
    }
}
