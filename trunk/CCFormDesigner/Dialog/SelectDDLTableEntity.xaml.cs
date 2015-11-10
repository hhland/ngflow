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
    public partial class SelectDDLTableEntity : ChildWindow
    {
        public SelectDDLTableEntity()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            FF.CCFormSoapClient ff = Glo.GetCCFormSoapClientServiceInstance();
            ff.DoTypeAsync("SaveSFTable", this.TB_CHName.Text, this.TB_EnName.Text, null, null, null);
            ff.DoTypeCompleted += new EventHandler<FF.DoTypeCompletedEventArgs>(ff_DoTypeCompleted);
        }

        void ff_DoTypeCompleted(object sender, FF.DoTypeCompletedEventArgs e)
        {
            if (e.Result == null)
            {
                this.DialogResult = true;
                return;
            }
            MessageBox.Show(e.Result);
        }
        void ff_RunSQLsCompleted(object sender, FF.RunSQLsCompletedEventArgs e)
        {
            /* Begin saving data .*/
            MessageBox.Show(" Table or view data storage function is not implemented , You can open the database directly modify the table or view  " + this.TB_EnName.Text + ".");
            this.DialogResult = true;
        }

        void fc_DoTypeCompleted(object sender, FF.DoTypeCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                MessageBox.Show(e.Result);
                return;
            }

            this.TB_EnName.IsEnabled = false;
            this.OKButton.Content = " Save ";
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

