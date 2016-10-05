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
        public void Display(CustomTile sender)
        {
            CurrentViewPc.Text = sender.TileHeader.Text;
            try
            {
                PcDetailsContainer.Children.Clear();
                List<string> folders = Scan.IdentifyFolderNames(sender.TileHeader.Text);
                Console.WriteLine(folders.Count);
                
                //SegLibrary.Seperate.CurrSearch("\\" + sender.TileHeader.Text, new System.Text.RegularExpressions.Regex(".*"), folders);
                foreach (string file in folders)
                {
                    Console.WriteLine(file);
                    string name = file;
                    string path = "\\" + sender.TileHeader.Text + "\\" + file;
                    List<string> ImageList = new List<string>();
                    List<string> VideoList = new List<string>();
                    List<string> AudioList = new List<string>();
                    List<string> TextList = new List<string>();
                    ImageList = SegLibrary.Seperate.GetImages(path);
                    AudioList = SegLibrary.Seperate.GetAudios(path);
                    VideoList = SegLibrary.Seperate.GetVideos(path);
                    TextList = SegLibrary.Seperate.GetDocs(path);
                    var size = SharePage.DirSize(new System.IO.DirectoryInfo(@path));
                    PcDetailsContainer.Children.Add(new FolderTile(name, path, VideoList.Count.ToString(), AudioList.Count.ToString(), TextList.Count.ToString(), ImageList.Count.ToString(), size));
                }
            }
            catch
            {
                Console.WriteLine("Exception thrown ! No Access");
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
