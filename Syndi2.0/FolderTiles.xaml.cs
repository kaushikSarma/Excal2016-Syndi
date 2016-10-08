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
using System.Text.RegularExpressions;
namespace Syndi2._0
{
    /// <summary>
    /// Interaction logic for FolderTiles.xaml
    /// </summary>
    public partial class FolderTiles : UserControl
    {
        public FolderTiles(string Fname)
        {
            InitializeComponent();
            FolderName.Text = Fname;
            CheckType();
        }
        public void CheckType()
        {
            string name = FolderName.Text;
            var Video = new Regex(@".*\.(mp4|avi|asf|mov|qt|flv|swf|avchd|kmp|mkv|TS)", RegexOptions.IgnoreCase);
            var Image = new Regex(@".*\.(ani|bmp|cal|fax|gif|img|jbg|jpg|jpe|jpeg|mac|pbm|pcd|pcx|pct|pgm|png|ppm|psd|ras|tga|tiff|wmf|jpg)", RegexOptions.IgnoreCase);
            var Audio = new Regex(@".*\.(pcm|wav|aiff|mp3|aac|ogg|wma|flac|alac|wma|m4a)", RegexOptions.IgnoreCase);
            var Other = new Regex(@".*\.(docx|xls|txt|pdf|doc|ppt|one|pub|xlsx|pptx|doc|vsdx|accdb|dot|maw)", RegexOptions.IgnoreCase);
            if (Video.IsMatch(name))
            {
                image.Source = new BitmapImage(new Uri(@"Resource/Icons/video-player.png", UriKind.Relative));
            } else if (Audio.IsMatch(name))
            {
                image.Source = new BitmapImage(new Uri("Resource/Icons/headphones.png", UriKind.Relative));
            } else if (Image.IsMatch(name))
            {
                image.Source = new BitmapImage(new Uri("Resource/Icons/picture.png", UriKind.Relative));
            }
            else
            {
                image.Source = new BitmapImage(new Uri("Resource/Icons/file.png", UriKind.Relative));
            }
        }
    }
}
