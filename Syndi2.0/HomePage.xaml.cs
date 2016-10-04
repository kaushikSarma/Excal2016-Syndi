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
using System.DirectoryServices;
using NetworkScanner;
    
namespace Syndi2._0
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        List<Scan.StructDataOfPC> PcList;
        public HomePage()
        {
            InitializeComponent();            
        }
        public void OnPageLoad(object sender, RoutedEventArgs e)
        {
            PopulatePcCmdList();
        }
        public async void PopulatePcCmdList()
        {
            string netBiosName = Environment.MachineName;
            PcName.Text = netBiosName;
            await Task.Delay(100);
            PcList = Scan.DetailsOfPC();
            int count =  0;
            PcListTileContainer.Children.Clear();
            foreach (Scan.StructDataOfPC PC in PcList)
            {
                count++;
                CustomTile t = new CustomTile(PC, count);
                if(PC.TypeOfPC.ToUpper() == "PUBLIC")
                    t.ContentTileButton.Click += (sender1, ex) => this.Display(t);
                PcListTileContainer.Children.Add(t);
                if(PC.NameOfPC == netBiosName)
                {
                    Display(new CustomTile(PC, 0));
                }
            }
            if(count > 99)
            {
                NumberOfConnections.Text = count.ToString();
            }
            else if (count > 9)
            {
                NumberOfConnections.Text = "0" + count.ToString();
            }
            else
            {
                NumberOfConnections.Text = "00" + count.ToString();
            }
            if (count == 0)
            {
                availableText.Text = "unavailable";
                availableText.Foreground = new SolidColorBrush(Color.FromArgb(255, 232, 55, 78));//#FFE8554E
            }
            else if (count > 0)
            {
                availableText.Text = "available";
                availableText.Foreground = new SolidColorBrush(Color.FromArgb(255, 106, 232, 78));//#FF6AE84E
            }
        }
        /*
        public void PopulatePcList()
        {
            string netBiosName = System.Environment.MachineName;
            PcName.Text = netBiosName;
            List<String> _ComputerNames = new List<String>();
            String _ComputerSchema = "Computer";
            int count = 0;
            PcListTileContainer.Children.Clear();
            System.DirectoryServices.DirectoryEntry _WinNTDirectoryEntries = new System.DirectoryServices.DirectoryEntry("WinNT:");
            foreach (System.DirectoryServices.DirectoryEntry _AvailDomains in _WinNTDirectoryEntries.Children)
            {
                Console.WriteLine("List " + _AvailDomains.Name);
                foreach (System.DirectoryServices.DirectoryEntry _PCNameEntry in _AvailDomains.Children)
                {
                    if (_PCNameEntry.SchemaClassName.ToLower().Contains(_ComputerSchema.ToLower()))
                    {
                        if(_PCNameEntry.Name.Length > 0)
                        {
                            count++;
                            CustomTile t = new CustomTile(_PCNameEntry.Name, count);
                            t.ContentTileButton.Click += (sender1, ex) => this.Display(t);
                            PcListTileContainer.Children.Add(t);
                        }
                    }
                }
            }
            NumberOfConnections.Text = ((count < 10) ? "0" : "") + count.ToString() ;
            if (count == 0)
            {
                availableText.Text = "unavailable";
                availableText.Foreground = new SolidColorBrush(Color.FromArgb(255, 232, 55, 78));//#FFE8554E
            }
            else if(count > 0)
            {
                availableText.Text = "available";
                availableText.Foreground = new SolidColorBrush(Color.FromArgb(255, 106, 232, 78));//#FF6AE84E
            }
            Display(new CustomTile(netBiosName, 0));
        }
        */
        public void Display(CustomTile sender)
        {
            CurrentViewPc.Text = sender.TileHeader.Text;
            try
            {
                PcDetailsContainer.Children.Clear();
                List<string> folders = Scan.IdentifyFolderNames(sender.TileHeader.Text);
                SegLibrary.Seperate.CurrSearch("\\" + sender.TileHeader.Text, new System.Text.RegularExpressions.Regex(".*"), folders);
                foreach (string file in folders)
                {
                    Console.WriteLine(file);
                    PcDetailsContainer.Children.Add(new FolderTiles(file));
                }
            }
            catch
            {
                PcDetailsContainer.Children.Add(new FolderTiles("No Access"));
            }
        }

        private async void BrowseLeftPc_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            while (PCScrollViewer.HorizontalOffset != 0 && i < 20)
            {
                await Task.Delay(1);
                i++;
                PCScrollViewer.ScrollToHorizontalOffset(PCScrollViewer.HorizontalOffset - 10);
            }
        }

        private async void BrowseRightPc_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            while (PCScrollViewer.HorizontalOffset != PCScrollViewer.ScrollableWidth && i < 20)
            {
                await Task.Delay(1);
                i++;
                PCScrollViewer.ScrollToHorizontalOffset(PCScrollViewer.HorizontalOffset + 10);
            }
        }
        private async void BrowseLeftFiles_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            while (PCFolderScroll.HorizontalOffset != 0 && i < 20)
            {
                await Task.Delay(1);
                i++;
                PCFolderScroll.ScrollToHorizontalOffset(PCFolderScroll.HorizontalOffset - 10);
            }
        }

        private async void BrowseRightFiles_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            while (PCFolderScroll.HorizontalOffset != PCFolderScroll.ScrollableWidth && i < 20)
            {
                await Task.Delay(1);
                i++;
                PCFolderScroll.ScrollToHorizontalOffset(PCFolderScroll.HorizontalOffset + 10);
            }
        }
    }
}
