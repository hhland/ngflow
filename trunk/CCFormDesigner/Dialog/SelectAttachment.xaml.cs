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
    public partial class SelectAttachment : ChildWindow
    {
        public SelectAttachment()
        {
            InitializeComponent();
        }
        public BPAttachment HisBPAttachment = null;
        public void BindIt(BPAttachment ment)
        {
            this.HisBPAttachment = ment;
            this.TB_No.Text = ment.Name;
            this.TB_Name.Text = ment.Label;
            this.TB_Exts.Text = ment.Exts;
            this.TB_SaveTo.Text = ment.SaveTo;
            this.CB_IsDelete.IsChecked = ment.IsDelete;
            this.CB_IsDownload.IsChecked = ment.IsDownload;
            this.CB_IsUpload.IsChecked = ment.IsUpload;
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            #region  Data Check .
            if (string.IsNullOrEmpty(this.TB_Name.Text)
               || string.IsNullOrEmpty(this.TB_No.Text))
            {
                MessageBox.Show(" You need to enter the field, the English name ", "Note", MessageBoxButton.OK);
                return;
            }
            if (this.TB_No.Text.Length >= 50)
            {
                MessageBox.Show(" English name is too long , Not more than 50 Characters , And it must be underlined or alphabetical .", "Note",
                    MessageBoxButton.OK);
                return;
            }
            #endregion  Data Check .


            #region  Property .
            string mypk = this.TB_No.Text.Trim();
            string vals = "@EnName=BP.Sys.FrmAttachment@MyPK=" +Glo.FK_MapData +"_" + mypk + "@FK_MapData=" + Glo.FK_MapData + "@Name=" + this.TB_Name.Text + "@Exts=" + this.TB_Exts.Text + "@NoOfObj=" + mypk;
            vals += "@UploadType=0";
            vals += "@SaveTo=" + this.TB_SaveTo.Text.Trim();
            vals += "@X=" +this.HisBPAttachment.X;
            vals += "@Y=" + this.HisBPAttachment.Y;

            if (this.CB_IsDelete.IsChecked == true)
                vals += "@IsDelete=1";
            else
                vals += "@IsDelete=0";

            if (this.CB_IsDownload.IsChecked == true)
                vals += "@IsDownload=1";
            else
                vals += "@IsDownload=0";

            if (this.CB_IsUpload.IsChecked == true)
                vals += "@IsUpload=1";
            else
                vals += "@IsUpload=0";

            #endregion  Property .

            FF.CCFormSoapClient daSaveFile = Glo.GetCCFormSoapClientServiceInstance();
            daSaveFile.SaveEnAsync(vals);
            daSaveFile.SaveEnCompleted += new EventHandler<FF.SaveEnCompletedEventArgs>(daSaveFile_SaveEnCompleted);
        }
        void daSaveFile_SaveEnCompleted(object sender, FF.SaveEnCompletedEventArgs e)
        {
            if (e.Result.Contains("Err"))
            {
                MessageBox.Show(e.Result, "Error", MessageBoxButton.OK);
                return;
            }

            if (this.HisBPAttachment == null)
                this.HisBPAttachment = new BPAttachment();

            this.HisBPAttachment.Label = this.TB_Name.Text;
            this.HisBPAttachment.Exts = this.TB_Exts.Text;
            this.HisBPAttachment.IsDelete = (bool)this.CB_IsDelete.IsChecked;
            this.HisBPAttachment.IsDownload = (bool)this.CB_IsDownload.IsChecked;
            this.HisBPAttachment.IsUpload = (bool)this.CB_IsUpload.IsChecked;
            this.HisBPAttachment.SaveTo = this.TB_SaveTo.Text;
            MessageBox.Show(" Saved successfully .", "Save OK", MessageBoxButton.OK);
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
       
        private void TB_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void TB_No_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void CB_IsDownload_Checked(object sender, RoutedEventArgs e)
        {
        }
    }
}

