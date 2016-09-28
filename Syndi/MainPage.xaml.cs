using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Networking;
using Windows.Networking.Connectivity;
using System.DirectoryServices;
using System.Security;



// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Syndi
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            var hostNames = NetworkInformation.GetHostNames();
            var hostName = hostNames.FirstOrDefault(name => name.Type == HostNameType.DomainName)?.DisplayName ?? "???";
            HomePageTitle.Text = hostName;
            List<string> cb = new List<string>();
            DirectoryEntry root = new DirectoryEntry("WinNT:");
            foreach (DirectoryEntry computers in root.Children)
            {
                foreach (DirectoryEntry computer in computers.Children)
                {
                    if (computer.Name != "Schema")
                    {
                        cb.Add(computer.Name);
                    }
                }
            }
            NumberOfConnections.Text = cb.Count.ToString();
        }

        public void homeButtonClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), null);
        }
        public void shareButtonClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SharePage), null);
        }
    }
}