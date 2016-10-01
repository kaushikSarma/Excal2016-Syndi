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
    /// Interaction logic for FolderTile.xaml
    /// </summary>
    public partial class FolderTile : UserControl
    {
        public FolderTile(string title,String path,String video,String audio,string text,string images,string size)
        {
            InitializeComponent();
            FolderName.Text = title;
            Path.Text = path;
            AudioFiles.Text = audio;
            VideoFiles.Text = video;
            ImageFiles.Text = images;
            TextFiles.Text = text;
            Size.Text = size;
        }
    }
}
