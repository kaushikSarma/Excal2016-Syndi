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
using System.Text.RegularExpressions;
using System.IO;

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
            c.IsChecked = true;
            SelectPC.Items.Add(c);
            foreach (string PCName in PC[0])
            {
                c = new CheckBox();
                c.Content = PCName;
                c.Foreground = ItemsColor;
                c.IsChecked = true;
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
            SearchContainer.Children.Clear();
            int count = 0;
            foreach(var item in SearchList)
            {
                Console.WriteLine(item);
                count++;
                var tile = new ExplorerTile(item, query);
                tile.DownloadThis.Click += (sender, ex) => DownloadItem(@tile.Path.Tag.ToString());
                SearchContainer.Children.Add(tile);
            }
            if(count == 0)
            {
                TextBlock t = new TextBlock();
                t.Text = "No Results to Display";
                t.FontSize = 40;
                t.HorizontalAlignment = HorizontalAlignment.Center;
                t.Foreground = new SolidColorBrush(Color.FromArgb(100, 250, 250, 250));
                SearchContainer.Children.Add(t);
            }
        }

        public void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            SearchFolder(SearchField.SearchQuery.Text);
        }
        private async void DownloadItem(string path)
        {
            Console.WriteLine("Copying started");
            await Task.Delay(5);
            Console.WriteLine("Copying started after timer");
            Console.WriteLine(path);
            var destn = Properties.Settings.Default["Path"].ToString();
            if (Directory.Exists(destn))
            {
                System.Threading.Thread copyFile = new System.Threading.Thread(() => NetworkScanner.Scan.CopyFiles(path, destn));
                copyFile.Start();
                copyFile.Join();
                System.Windows.Forms.MessageBox.Show("Successfully downloaded files");
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Invalid path or path not set go to settings");
            }

        }
    }
}
