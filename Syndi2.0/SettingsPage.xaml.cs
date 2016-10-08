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
using System.Windows.Forms;

namespace Syndi2._0
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            if (Properties.Settings.Default["Path"].ToString().Length > 0)
            {
                DefaultPath.Text = Properties.Settings.Default["Path"].ToString();
            }

        }

        private void SetDefaultPath(object sender, RoutedEventArgs e)
        {
            var dlg1 = new Ionic.Utils.FolderBrowserDialogEx();
            dlg1.Description = "Select a folder";
            dlg1.ShowNewFolderButton = true;
            dlg1.ShowEditBox = true;
            dlg1.ShowBothFilesAndFolders = false;

            dlg1.ShowFullPathInEditBox = true;
            dlg1.RootFolder = System.Environment.SpecialFolder.MyComputer;

            // Show the FolderBrowserDialog.
            DialogResult result = dlg1.ShowDialog();
            if (result == DialogResult.OK)
            {
                var path = dlg1.SelectedPath;
                Console.WriteLine(path);
                DefaultPath.Text = path;
                Properties.Settings.Default["Path"] = path;
                Properties.Settings.Default.Save();
            }
        }
            
    }
}
