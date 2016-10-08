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
using System.Runtime.InteropServices;

namespace Syndi2._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HomePage hPage = new HomePage();
        private SharePage sPage = new SharePage();
        private SearchPage searchPage = new SearchPage();
        private SettingsPage settingsPage = new SettingsPage();
        public MainWindow()
        {
            InitializeComponent();
        }
        public void OnWindowLoad(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(searchPage);
        }
        private void homeButtonClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(hPage);
        }

        private void shareButtonClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(sPage);
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(((MainFrame)this).CurrentSource.ToString());
        }

        private void searchButtonClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(searchPage);
        }
        private void settingsButtonClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(settingsPage);
        }
    }
}
