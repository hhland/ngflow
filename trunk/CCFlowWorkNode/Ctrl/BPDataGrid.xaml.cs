using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System;

namespace WorkNode
{
    public partial class BPDataGrid : DataGrid
    {
        public event RowEventHandler DeleteRow;
        public event RowEventHandler AddRow;
        public BPDataGrid()
        {
            InitializeComponent();
        }
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            if (img.Tag == null)
            {
                if (AddRow != null)
                {
                    AddRow.Invoke(this, sender);
                }
            }
            else
            {
                if (DeleteRow != null)
                {
                    DeleteRow.Invoke(this, sender);
                }
            }
        }
    }
    public delegate void RowEventHandler(object sender, object args);
}
