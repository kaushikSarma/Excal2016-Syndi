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
        public FolderTile(string title,String path,String video,String audio,string text,string images,long size)
        {
            InitializeComponent();
            this.ToolTip = path;
            FolderName.Text = title;
            //Path.Text = path;
            AudioFiles.Text = audio;
            VideoFiles.Text = video;
            ImageFiles.Text = images;
            TextFiles.Text = text;
            double SizeInKB = size;
            if(size >= Math.Pow(1024,3))
            {
                SizeInKB = size / Math.Pow(1024, 3);
                Size.Text = SizeInKB.ToString("F2");
                SizeUnit.Text = " GB";
            } else if(size >= Math.Pow(1024, 2))
            {
                SizeInKB = size / Math.Pow(1024, 2);
                Size.Text = SizeInKB.ToString("F2");
                SizeUnit.Text = " MB";
            } else if(size >= 1024)
            {
                SizeInKB = size / 1024;
                Size.Text = SizeInKB.ToString("F2");
                SizeUnit.Text = " KB";
            }
            else
            {
                Size.Text = SizeInKB.ToString();
                SizeUnit.Text = " B";
            }
        }
    }
}
