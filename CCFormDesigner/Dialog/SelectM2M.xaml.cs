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
    public partial class SelectM2M : ChildWindow
    {
        public double X = 0;
        public double Y = 0;

        public int IsM2M = 0;
        public SelectM2M()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            #region  Data Check .
            if (string.IsNullOrEmpty(this.TB_Name.Text)
               || string.IsNullOrEmpty(this.TB_No.Text))
            {
                MessageBox.Show(" You need to enter the field, the English name ", " Prompt ", MessageBoxButton.OK);
                return;
            }
            if (this.TB_No.Text.Length >= 50)
            {
                MessageBox.Show(" English name is too long , Not more than 50 Characters , And it must be underlined or alphabetical .", "Note",
                    MessageBoxButton.OK);
                return;
            }
            #endregion  Data Check .
             

            FF.CCFormSoapClient ff = Glo.GetCCFormSoapClientServiceInstance();
            ff.DoTypeAsync("NewM2M", Glo.FK_MapData, this.TB_No.Text, this.IsM2M.ToString(), this.IsM2M.ToString(), this.X.ToString(), this.Y.ToString());
            ff.DoTypeCompleted += new EventHandler<FF.DoTypeCompletedEventArgs>(ff_DoTypeCompleted);
        }

        void ff_DoTypeCompleted(object sender, FF.DoTypeCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                MessageBox.Show(e.Result," Prompt ", MessageBoxButton.OK);
                return;
            }

            Glo.TempVal = this.TB_No.Text;
            this.DialogResult = true;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void TB_Name_LostFocus(object sender, RoutedEventArgs e)
        {
            Glo.GetKeyOfEn(this.TB_Name.Text, true, this.TB_No);
        }
      
    }
}

