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

namespace CCForm
{
    public partial class SelectCheckGroup : ChildWindow
    {
        public SelectCheckGroup()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.TB_GroupKey.Text)
               || string.IsNullOrEmpty(this.TB_GroupName.Text))
            {
                MessageBox.Show(" You need to enter the field, the English name ", " Prompt ", MessageBoxButton.OK);
                return;
            }


            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

