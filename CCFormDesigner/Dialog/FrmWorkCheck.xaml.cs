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
    public partial class FrmWorkCheck : ChildWindow
    {
        public BPWorkCheck HisWorkCheck;
        public FrmWorkCheck()
        {
            InitializeComponent();
        }

        public void BindIt(BPWorkCheck fwc)
        {
            CB_FWCType.SelectedIndex = 0;
            RD_DisEnble.IsChecked = true;
            this.HisWorkCheck = fwc;
            if (this.HisWorkCheck.FWC_Sta == "1")
            {
                RD_Enble.IsChecked = true;
            }
            if (this.HisWorkCheck.FWC_Type == "1")
            {
                CB_FWCType.SelectedIndex = 1;
            }
            this.Show();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.HisWorkCheck.FWC_Sta = RD_DisEnble.IsChecked == false ? "1" : "2";
            this.HisWorkCheck.FWC_Type = CB_FWCType.SelectedIndex.ToString();
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

