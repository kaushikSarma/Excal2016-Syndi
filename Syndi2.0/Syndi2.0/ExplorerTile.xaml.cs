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
    /// Interaction logic for ExplorerTile.xaml
    /// </summary>
    public partial class ExplorerTile : UserControl
    {
        public ExplorerTile(string path, string searchString)
        {
            InitializeComponent();
            Path.Tag = path;
            HighlightText(path, searchString, Path);
            HighlightText(path.Substring(path.LastIndexOf('\\')+1), searchString, File);
        }
        private void HighlightText(string path, string search, TextBlock t)
        {
            List<string> brokenPath = new List<string>();
            string s = path,s2, substr;
            int i = 0;
            search = search.ToLower();
            do
            {
                i = s.ToLower().IndexOf(search);
                if (i >= 0)
                {
                    substr = s.Substring(i, search.Length);
                    s2 = s.Substring(0, i);
                    if (s2.Length > 0) brokenPath.Add(s2);
                    brokenPath.Add(substr);
                    s = s.Substring(i + search.Length);
                }
                else
                {
                    s2 = s;
                    s = "";
                    brokenPath.Add(s2);
                }
            } while (s.Length > 0);
            var highlight = new Run();
            highlight.Background = new SolidColorBrush(Color.FromArgb(200,200,150,255));
            foreach(string cs in brokenPath)
            {
                if(cs.ToLower() == search.ToLower())
                {
                    highlight.Text = cs;
                    t.Inlines.Add(highlight);
                }
                else
                {
                    t.Inlines.Add(new Run(cs));
                }
            }

        }
    }
}
