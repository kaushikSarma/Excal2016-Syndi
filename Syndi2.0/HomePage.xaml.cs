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
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Syndi2._0
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        //Variables
        public List<Scan.StructDataOfPC> PcList = new List<Scan.StructDataOfPC>();
        public List<ListingPC.Testing.DataOfPC> Pc = new List<ListingPC.Testing.DataOfPC>();
        //Functions
        public HomePage()
        {
            InitializeComponent();            
        }
        public void OnPageLoad(object sender, RoutedEventArgs e)
        {
            PopulatePc();
        }

        public async void PopulatePc()
        {
            string netBiosName = Environment.MachineName;
            PcName.Text = netBiosName;
            await Task.Delay(5);
            Pc.Clear();
            Pc = ListingPC.ListNetwork.ScanNetwork();

            int count = 0;
            Scan.StructDataOfPC com = new Scan.StructDataOfPC();

            PcList.Clear();
            foreach (ListingPC.Testing.DataOfPC PC in Pc)
            {
                count++;
                Console.WriteLine(PC.ToString());
                com.NameOfPC = PC.Name;
                com.TypeOfPC = PC.Type;
                if (PC.Type == "Public")
                {
                    com.SizeOfSharedFolders = ListingPC.FolderSize.SizeOfSharedFiles(PC.Name);
                }
                else
                {
                    com.SizeOfSharedFolders = 0;
                }
                PcList.Add(com);
            }

            PcListTileContainer.Children.Clear();
            foreach (Scan.StructDataOfPC PC in PcList)
            {
                Console.WriteLine(PC.ToString());
                CustomTile t = new CustomTile(PC, count);
                if (PC.TypeOfPC.ToUpper() == "PUBLIC")
                    t.ContentTileButton.Click += (sender1, ex) => this.Display(t);
                else
                    t.ContentTileButton.Click += (sender1, ex) => this.GrantAccess(PC.NameOfPC);
                PcListTileContainer.Children.Add(t);
                if (PC.NameOfPC == netBiosName)
                {
                    Display(new CustomTile(PC, 0));
                }
            }
            if (count > 99)
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
        public async void PopulatePcList()
        {
            string netBiosName = Environment.MachineName;
            PcName.Text = netBiosName;
            await Task.Delay(5);
            PcList = Scan.DetailsOfPC();
            FolderPathPanel.Children.Clear();
            int count =  0;
            PcListTileContainer.Children.Clear();
            foreach (Scan.StructDataOfPC PC in PcList)
            {
                count++;
                Console.WriteLine(PC.ToString());
                CustomTile t = new CustomTile(PC, count);
                if(PC.TypeOfPC.ToUpper() == "PUBLIC")
                    t.ContentTileButton.Click += (sender1, ex) => this.Display(t);
                else
                    t.ContentTileButton.Click += (sender1, ex) => this.GrantAccess(PC.NameOfPC);
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
        public async void GrantAccess(string name)
        {
            await Task.Delay(5);
            List<string> accessToken = Prompt.ShowDialog();
            Console.WriteLine("____________________________GrantAccess_________________________________");
            Console.WriteLine(name);
            Console.WriteLine(accessToken.ToArray()[0]);
            Console.WriteLine(accessToken.ToArray()[1]);
            if(accessToken==null)
            {
                System.Windows.Forms.MessageBox.Show("Invalid Credentials");
                return;
            }
            if (accessToken.Count==2)
            {
                bool output = NetworkScanner.Scan.ShowFiles(name,accessToken.ToArray()[0], accessToken.ToArray()[1]);
                if (output)
                {
                    PopulatePc();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Invalid Credentials");
                }
            }

        }
        public async void Display(CustomTile sender)
        {
            await Task.Delay(5);
            PopulateFileList(sender.TileHeader.Text);
        }
        public void PopulateFileList(string path) { 
            CurrentViewPc.Text = path;
            try
            {
                PcDetailsContainer.Children.Clear();
                List<string> folders = Scan.IdentifyFolderNames(path);
                
                foreach (string folder in folders)
                {
                    string name = folder;
                    string Path = "\\\\" + path + "\\" + folder.TrimEnd();
                    List<string> ImageList = new List<string>();
                    List<string> VideoList = new List<string>();
                    List<string> AudioList = new List<string>();
                    List<string> TextList = new List<string>();
                    ImageList = SegLibrary.Seperate.GetImages(@Path);
                    AudioList = SegLibrary.Seperate.GetAudios(@Path);
                    VideoList = SegLibrary.Seperate.GetVideos(@Path);
                    TextList = SegLibrary.Seperate.GetDocs(@Path);
                    var size = SharePage.DirSize(new System.IO.DirectoryInfo(@Path));
                    FolderTile f = new FolderTile(name, path, VideoList.Count.ToString(), AudioList.Count.ToString(), TextList.Count.ToString(), ImageList.Count.ToString(), size);
                    f.DownloadThis.Tag = Path;
                    f.DownloadThis.Click += (sender1, ex) => this.DownloadItem(sender1, ex, @f.DownloadThis.Tag.ToString());
                    f.MouseDoubleClick += (sender1, ex) => this.ExploreItem(@f.DownloadThis.Tag.ToString());
                    PcDetailsContainer.Children.Add(f);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception thrown ! No Access");
                Console.WriteLine(e + path);
            }
        }
        private async void ExploreItem(string path)
        {
            await Task.Delay(5);
            int indexFrom, indexLength;
            List<string> Inner = new List<string>();
            string Path = path;
            SegLibrary.Seperate.CurrSearch(path,new System.Text.RegularExpressions.Regex(".*"),Inner);
            PcDetailsContainer.Children.Clear();
            NavigationButton t = new NavigationButton();
            try
            {
                int i = Path.LastIndexOf('\\') + 1;
                Console.WriteLine("Location : " + Path.LastIndexOf('\\') + " Path : " + Path.Substring(i));
                t.Display.Text = Path.Substring(i);
                t.PathInfo.Tag = Path;
                t.MouseDoubleClick += (sender, ex) => NavigationButtonClick(t);
                
            } catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
            FolderPathPanel.Children.Add(t);
            foreach(string name in Inner)
            {
                indexFrom = name.LastIndexOf('\\') + 1;
                indexLength = name.Length;
                Path = path + "\\" + name.Substring(indexFrom);
                if (Directory.Exists(@Path))
                {
                    List<string> ImageList = new List<string>();
                    List<string> VideoList = new List<string>();
                    List<string> AudioList = new List<string>();
                    List<string> TextList = new List<string>();
                    ImageList = SegLibrary.Seperate.GetImages(@Path);
                    AudioList = SegLibrary.Seperate.GetAudios(@Path);
                    VideoList = SegLibrary.Seperate.GetVideos(@Path);
                    TextList = SegLibrary.Seperate.GetDocs(@Path);
                    var size = SharePage.DirSize(new System.IO.DirectoryInfo(@Path));
                    FolderTile tile = new FolderTile(name.Substring(indexFrom, indexLength - indexFrom), Path, VideoList.Count.ToString(), AudioList.Count.ToString(), TextList.Count.ToString(), ImageList.Count.ToString(), size);
                    tile.DownloadThis.Tag = Path;
                    tile.DownloadThis.Click += (sender1, ex) => this.DownloadItem(sender1, ex, @tile.DownloadThis.Tag.ToString());
                    tile.MouseDoubleClick += (sender1, ex) => this.ExploreItem(@tile.DownloadThis.Tag.ToString());
                    PcDetailsContainer.Children.Add(tile);
                }
                else
                {
                    FolderTiles tile = new FolderTiles(name.Substring(indexFrom, indexLength - indexFrom));
                    tile.FolderName.Tag = Path;
                    string tempPath = Path.Substring(Path.LastIndexOf("\\")+1);
                    tile.MouseDoubleClick += (sender, ex) => RunFile(@tile.FolderName.Tag.ToString());
                    tile.DownloadThis.Click += (sender, ex) => this.DownloadItem(sender, ex, @tile.FolderName.Tag.ToString());
                    PcDetailsContainer.Children.Add(tile);
                }
                Console.WriteLine(@Path);
            }
        }
        public void NavigationButtonClick(NavigationButton b)
        {
            int indexOfb = FolderPathPanel.Children.IndexOf(b),
                count = FolderPathPanel.Children.Count;
            if(indexOfb <= count - 1)
            {
                FolderPathPanel.Children.RemoveRange(indexOfb + 1, count - indexOfb - 1);
            }
            ExploreItem(@b.PathInfo.Tag.ToString());
        }
        public void RunFile(string path)
        {
            // System.Windows.Forms.MessageBox.Show(path.Trim());
            Console.WriteLine("_____________Debug_____________");
            //Console.WriteLine(path.Trim());
            //path = Regex.Replace(path, @" +\\", @"\");
            //Console.WriteLine(path);
            System.Diagnostics.Process.Start(path.Trim());
        }
        private async void DownloadItem(object sender, RoutedEventArgs e, string path)
        {
            Console.WriteLine("Copying started");
            await Task.Delay(5);
            Console.WriteLine("Copying started after timer");
            try
            {
                Console.WriteLine(path);
                var destn = Properties.Settings.Default["Path"].ToString();
                if (Directory.Exists(path))
                {
                    var AppendedPath = destn + "\\" + path.Split('\\')[path.Split('\\').Length - 1];
                    foreach (string dirPath in Directory.GetDirectories(path, "*",
                    SearchOption.AllDirectories))
                        Directory.CreateDirectory(dirPath.Replace(path, AppendedPath));

                    //Copy all the files & Replaces any files with the same name
                    foreach (string newPath in Directory.GetFiles(path, "*.*",
                        SearchOption.AllDirectories))
                        File.Copy(newPath, newPath.Replace(path, AppendedPath), true);
                }
                else
                {
                    var destination = System.IO.Path.Combine(destn, System.IO.Path.GetFileName(path));
                    System.IO.File.Copy(path, destination, true);
                }
                System.Windows.Forms.MessageBox.Show("Successfully downloaded files");
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Access denied or Invalid Path");
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
    public static class Prompt
    {
        public static List<string> ShowDialog()
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 350,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen
            };
            System.Windows.Forms.Label textLabel1= new System.Windows.Forms.Label() { Left = 50, Top = 20, Text = "Username:" };
            System.Windows.Forms.TextBox textBox1 = new System.Windows.Forms.TextBox() { Left = 50, Top = 50, Width = 400 };
            System.Windows.Forms.Label textLabel2 = new System.Windows.Forms.Label() { Left = 50, Top = 80, Text = "Password" };
            System.Windows.Forms.TextBox textBox2 = new System.Windows.Forms.TextBox() { Left = 50, Top = 110, Width = 400 };
            System.Windows.Forms.Button confirmation = new System.Windows.Forms.Button() { Text = "Ok", Left = 350, Width = 100, Top = 130, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox1);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel1);
            prompt.Controls.Add(textLabel2);
            prompt.Controls.Add(textBox2);
            prompt.AcceptButton = confirmation;
            List<string> result = new List<string>();
            //Console.WriteLine(textBox1.Text);
            //Console.WriteLine(textBox2.Text);
            var res = prompt.ShowDialog() == DialogResult.OK ? result : null;
            result.Add(textBox1.Text);
            result.Add(textBox2.Text);
            if (res == null)
                return null;
            return result;
        }
    }
}
