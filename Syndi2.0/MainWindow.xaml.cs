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

namespace Syndi2._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HomePage hPage = new HomePage();
        private SharePage sPage = new SharePage();

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(hPage);
        }

        public void BackgroundLoaded(object sender, RoutedEventArgs e)
        {
            BitmapImage b = new BitmapImage();
            b.BeginInit();
            b.UriSource = new Uri("Assets/Icons/SpaceBg.png", UriKind.Relative);
            b.EndInit();
            // ... Get Image reference from sender.
            var image = sender as Image;
            // ... Assign Source.
            image.Source = b;
        }

        private void homeButtonClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(hPage);
        }

        private void shareButtonClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(sPage);
        }
        private List<string> exec_cmd(string arguments)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = arguments;
            process.StartInfo = startInfo;
            process.Start();
            List<string> output = new List<string>();
            output.Add(process.StandardOutput.ReadLine());
            process.Close();
            return output;
        }
    }
}
