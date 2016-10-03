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
using System.IO;
using System.Windows.Forms;
using ShareLibrary;
using System.Text.RegularExpressions;
using System.Management;
using SegLibrary;
namespace Syndi2._0
{
    /// <summary>
    /// Interaction logic for SharePage.xaml
    /// </summary>
    public partial class SharePage : Page
    {
        public SharePage()
        {
            InitializeComponent();
            DisplaySharedFolder();
        }
        public void DisplaySharedFolder()
        {
            List<ManagementBaseObject> sharedList = new List<ManagementBaseObject>();
            sharedList = ShareWin.GetSharedFiles();
            foreach (var objShare in sharedList)
            {
                string name = objShare.Properties["Name"].Value.ToString();
                string path = objShare.Properties["Path"].Value.ToString();
                List<string> ImageList = new List<string>();
                List<string> VideoList = new List<string>();
                List<string> AudioList = new List<string>();
                List<string> TextList = new List<string>();
                ImageList = Seperate.GetImages(path);
                AudioList = Seperate.GetAudios(path);
                VideoList = Seperate.GetVideos(path);
                TextList = Seperate.GetDocs(path);
                var size = DirSize(new DirectoryInfo(path));
                FolderContainer.Children.Add(new FolderTile(name,path,"Video: " + VideoList.Count, "Audio: " + AudioList.Count, "Text: " + TextList.Count, "Images: " + ImageList.Count,"Size: " + size));
            }
        }

        public static long DirSize(DirectoryInfo d)
        {
            if (d.ToString() == @"C:\Users")
                return 0;
            long size = 0;
            try
            {
                FileInfo[] fis = d.GetFiles();
                foreach (FileInfo fi in fis)
                {
                    size += fi.Length;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Not accessible");
            }
            try
            {
                DirectoryInfo[] dis = d.GetDirectories();
                foreach (DirectoryInfo di in dis)
                {
                    size += DirSize(di);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Not accessible");
            }
            return size;
        }
        private void openBrowser_Click(object sender, RoutedEventArgs e)
        {
            var dlg1 = new Ionic.Utils.FolderBrowserDialogEx();
            dlg1.Description = "Select a file or folder";
            dlg1.ShowNewFolderButton = true;
            dlg1.ShowEditBox = true;
            dlg1.ShowBothFilesAndFolders = true;
            dlg1.ShowFullPathInEditBox = true;
            dlg1.RootFolder = System.Environment.SpecialFolder.MyComputer;

            // Show the FolderBrowserDialog.
            DialogResult result = dlg1.ShowDialog();
            if (result == DialogResult.OK)
            {
                var path = dlg1.SelectedPath;
                System.Windows.Forms.MessageBox.Show(path);
                var match = Regex.Match(path, @".*\\(.*)$");
                var name = match.Groups[1].Value;
                var myPath = "\\\\" + Environment.MachineName + "\\" + name;
                if (Directory.Exists(dlg1.SelectedPath))
                {
                    var check = ShareWin.CreateSharedFolder(path,name,name);
                    switch (check)
                    {
                        case 0:
                            Console.WriteLine("Folder successfuly shared.");
                            break;
                        case 1:
                            Console.WriteLine("Exception Thrown");
                            break;
                        case 2:
                            Console.WriteLine("Access Denied");
                            break;
                        case 8:
                            Console.WriteLine("Unknown Failure");
                            break;
                        case 9:
                            Console.WriteLine("Invalid Name");
                            break;
                        case 10:
                            Console.WriteLine("Invalid Level");
                            break;
                        case 21:
                            Console.WriteLine("Invalid Parameter");
                            break;
                        case 22:
                            Console.WriteLine("Duplicate Share");
                            break;
                        case 23:
                            Console.WriteLine("Redirected Path");
                            break;
                        case 24:
                            Console.WriteLine("Unknown Device or Directory");
                            break;
                        case 25:
                            Console.WriteLine("Net Name Not Found");
                            break;
                        default:
                            Console.WriteLine("Folder cannot be shared.");
                            break;
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Not allowed in Windows Environment");
                }
            }
        }
    }
}