using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Silverlight;

namespace CCForm
{
    public partial class FrmEvent : ChildWindow
    {
        public FrmEvent()
        {
            InitializeComponent();
            bindIt();
        }

        protected override void OnOpened()
        {
           
            base.OnOpened();
        }
        public void bindIt()
        {
            this.RB_FrmLoadBefore.IsChecked = true;
            this.RB_Checked(this.RB_FrmLoadBefore, null);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            string id = "";
            if ((bool)this.RB_FrmLoadAfter.IsChecked)
                id = this.RB_FrmLoadAfter.Name;

            if ((bool)this.RB_FrmLoadBefore.IsChecked)
                id = this.RB_FrmLoadBefore.Name;

            if ((bool)this.RB_SaveBefore.IsChecked)
                id = this.RB_SaveBefore.Name;

            if ((bool)this.RB_SaveAfter.IsChecked)
                id = this.RB_SaveAfter.Name;

            if (id == "")
            {
                MessageBox.Show(" Please select the type of event .", " Prompt ", MessageBoxButton.OK);
                return;
            }
            id = id.Replace("RB_", "");

            string info = "@EnName=BP.Sys.FrmEvent@MyPK=" + Glo.FK_MapData + "_" + id + "@FK_Event=" + id + "@FK_MapData=" + Glo.FK_MapData + "@DoType=" + this.DDL_EventType.SelectedIndex + "@DoDoc=" + this.TB_DoDoc.Text.Replace('@', '^') + "@MsgOK=" + this.TB_MsgOK.Text.Replace('@', '^') + "@MsgError=" + this.TB_MsgErr.Text.Replace('@', '^');
            FF.CCFormSoapClient daInfoSave = Glo.GetCCFormSoapClientServiceInstance();
            daInfoSave.SaveEnAsync(info);
            daInfoSave.SaveEnCompleted += new EventHandler<FF.SaveEnCompletedEventArgs>(daInfoSave_SaveEnCompleted);
        }
        void daInfoSave_SaveEnCompleted(object sender, FF.SaveEnCompletedEventArgs e)
        {
            if (e.Result.Contains("Err"))
            {
                MessageBox.Show(e.Result, " Save error ", MessageBoxButton.OK);
                return;
            }
            MessageBox.Show(" Single record saved successfully ", " Saving tips ", MessageBoxButton.OK);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void RB_Checked(object sender, RoutedEventArgs e)
        {
            //  Get id.
            RadioButton rb = sender as RadioButton;
            string id = rb.Name.Replace("RB_", "");

            FF.CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
            string sql = "SELECT * FROM Sys_FrmEvent WHERE FK_MapData='" + Glo.FK_MapData + "' AND FK_Event='" + id + "'";
            da.RunSQLReturnTableAsync(sql);
            da.RunSQLReturnTableCompleted += new EventHandler<FF.RunSQLReturnTableCompletedEventArgs>(da_RunSQLReturnTableCompleted);
        }
        void da_RunSQLReturnTableCompleted(object sender, FF.RunSQLReturnTableCompletedEventArgs e)
        {
            DataSet ds = new DataSet();
            ds.FromXml(e.Result);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
            {
                /*  No data  */
                this.TB_DoDoc.Text = "";
                this.TB_MsgErr.Text = "";
                this.TB_MsgOK.Text = "";
                this.DDL_EventType.SelectedIndex = 0;
            }
            else
            {
                this.TB_DoDoc.Text = dt.Rows[0]["DoDoc"].Replace("~", "'");
                try
                {
                    this.TB_MsgErr.Text = dt.Rows[0]["MsgError"].Replace("~", "'");
                }
                catch
                {
                }

                //try
                //{
                //    this.TB_MsgErr.Text = dt.Rows[0]["MsgErr"].Replace("~", "'");
                //}
                //catch
                //{
                //}
                this.TB_MsgOK.Text = dt.Rows[0]["MsgOK"].Replace("~", "'");
                this.DDL_EventType.SelectedIndex = int.Parse(dt.Rows[0]["DoType"]);
            }
        }
        private void OKBtnSaveAndClose_Click(object sender, RoutedEventArgs e)
        {
        }
        private void Btn_Help_Click(object sender, RoutedEventArgs e)
        {
            string msg = " Help ";
            msg += "\r\n1,  If you perform multiple  sql  You can use  ;  Separate .";
            msg += "\r\n2,  Content format support variables such conventions :@WebUser.No  The current operator number ....";
            msg += "\r\n3,  For more details, please refer to the use of  ccflow Operating Instructions Form Designer .";
            MessageBox.Show(msg);
        }
    }
}

