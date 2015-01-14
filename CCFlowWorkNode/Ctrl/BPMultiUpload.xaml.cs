using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WorkNode
{
    public partial class BPMultiUpload : UserControl
    {
        public BPMultiUpload()
        {
            InitializeComponent();
        }

        ChildWindow cw = new ChildWindow();
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Page page = Page.Instance;
            cw.Content = page;
            cw.Closed += (Sender2, e2) =>
            {
                dataGrid1.ItemsSource = page._files;

            };
            cw.Show();
        }


    }
}
