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
    /// Interaction logic for FolderTiles.xaml
    /// </summary>
    public partial class FolderTiles : UserControl
    {
        public FolderTiles(string Fname)
        {
            InitializeComponent();
            FolderName.Text = Fname;
        }
    }
}
