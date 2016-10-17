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
    /// Interaction logic for CustomTile.xaml
    /// </summary>
    public partial class CustomTile : UserControl
    {
        public CustomTile(NetworkScanner.Scan.StructDataOfPC PC, int number)
        {
            InitializeComponent();
            TileHeader.Text = PC.NameOfPC;
            if(PC.TypeOfPC.ToUpper() == "PUBLIC")
            {
                var SizeinKb = PC.SizeOfSharedFolders;
                double size;
                if (SizeinKb >= Math.Pow(1024, 3))
                {
                    size = SizeinKb / Math.Pow(1024, 3);
                    PcSize.Text = size.ToString("F2");
                    SizeUnit.Text = " GB";
                }
                else if (SizeinKb >= Math.Pow(1024, 2))
                {
                    size = SizeinKb / Math.Pow(1024, 2);
                    PcSize.Text = size.ToString("F2");
                    SizeUnit.Text = " MB";
                }
                else if (SizeinKb >= 1024)
                {
                    size = SizeinKb / 1024;
                    PcSize.Text = size.ToString("F2");
                    SizeUnit.Text = " KB";
                }
                else
                {
                    PcSize.Text = SizeinKb.ToString();
                    SizeUnit.Text = " B";
                }
            }
            else
            {
                PcSize.Text = "Protected";
                PcSize.FontSize = 30;
                SizeUnit.Text = "";
            }
        }
    }
}
