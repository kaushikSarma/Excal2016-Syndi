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
                var match = Regex.Match(path, @".*\\(.*)$");
                var name = match.Groups[1].Value;
                var myPath = "\\\\" + Environment.MachineName + "\\" + name;
                if (Directory.Exists(dlg1.SelectedPath))
                {
                    System.Windows.Forms.MessageBox.Show(myPath);
                    if(Directory.Exists(@myPath))
                    {
                        System.Windows.Forms.MessageBox.Show("File already shared");
                    }
                    else
                    {
                        if (ShareWin.ShareWithEveryone(dlg1.SelectedPath, name, name))
                        {
                            System.Windows.Forms.MessageBox.Show("Success");
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Failure");
                        }
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