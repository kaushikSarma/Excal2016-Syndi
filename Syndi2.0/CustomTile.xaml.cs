﻿using System;
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
        public CustomTile(string Title,int number)
        {
            InitializeComponent();
            TileHeader.Text = Title;
            PcNumber.Text = ((number < 9) ? "0" : "") + number.ToString();
        }
    }
}
