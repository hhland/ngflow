using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Windows.Browser;
using System.Text;
using Microsoft.Expression.Interactivity;
using Microsoft.Expression.Interactivity.Layout;
using System.Windows.Media.Imaging;
using Silverlight;
using BP.En;
using BP.Sys;
using CCForm.FF;

namespace CCForm
{
    public partial class SelectDDLTable : ChildWindow
    {
        public SelectDDLTable()
        {
            InitializeComponent();
            this.tableEntity.Closed += new EventHandler(tableEntity_Closed);
        }
        void tableEntity_Closed(object sender, EventArgs e)
        {
            if (this.tableEntity.DialogResult == false)
                return;

            this.BindData();

            foreach (ListBoxItem item in this.listBox1.Items)
                item.IsSelected = false;

            foreach (ListBoxItem item in this.listBox1.Items)
            {
                if (item.Content.ToString().Contains(":" + this.TB_KeyOfName.Text))
                {
                    item.IsSelected = true;
                    break;
                }
            }
        }
        protected override void OnOpened()
        {
            if (this.listBox1.Items.Count == 0)
            {
                this.BindData();
                //this.tabItem1.IsSelected = true;
                //this.tabItem2.Header = " New ";
            }
            base.OnOpened();
        }
        public void BindData()
        {
            this.listBox1.Items.Clear();

            string sql = "SELECT No,Name,TableDesc,FK_Val FROM Sys_SFTable";
            FF.CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
            da.RunSQLReturnTableAsync(sql);
            da.RunSQLReturnTableCompleted += new EventHandler<FF.RunSQLReturnTableCompletedEventArgs>(da_RunSQLReturnTableCompleted);
            //  this.listBox1.SelectionChanged += new SelectionChangedEventHandler(listBox1_SelectionChanged);
        }
        void da_RunSQLReturnTableCompleted(object sender, FF.RunSQLReturnTableCompletedEventArgs e)
        {
            DataSet ds = new DataSet();
            ds.FromXml(e.Result);
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                ListBoxItem item = new ListBoxItem();
                item.Tag = dr["No"].ToString() + ":" + dr["FK_Val"];
                item.Content = dr["No"] + ":" + dr["Name"];
                this.listBox1.Items.Add(item);
            }

            this.listBox1.UpdateLayout();


        }
        public string SelectEnName = null;
        void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            ListBoxItem li = e.AddedItems[0] as ListBoxItem;
            string[] itemName = li.Content.ToString().Split(':');
            string[] noFK_Val = li.Tag.ToString().Split(':');

            this.TB_KeyOfEn.Text = noFK_Val[1];
            this.TB_KeyOfName.Text = itemName[1];

            // Selected entit.
            this.SelectEnName = noFK_Val[0];


            //string s = li.Tag as string;
            //string[] strs = s.Split('@');
            //this.listBox2.Items.Clear();
            //foreach (string str in strs)
            //{
            //    if (string.IsNullOrEmpty(str))
            //        continue;
            //    ListBoxItem dd = new ListBoxItem();
            //    dd.Content = str;
            //    this.listBox2.Items.Add(dd);
            //}
            //this.EditItem(li);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            #region  Data Check .
            if (string.IsNullOrEmpty(this.TB_KeyOfName.Text)
               || string.IsNullOrEmpty(this.TB_KeyOfEn.Text))
            {
                MessageBox.Show(" You need to enter the field, the English name ", "Note", MessageBoxButton.OK);
                return;
            }
            if (this.TB_KeyOfEn.Text.Length >= 50)
            {
                MessageBox.Show(" English name is too long , Not more than 50 Characters , And it must be underlined or alphabetical .", "Note",
                    MessageBoxButton.OK);
                return;
            }
            #endregion  Data Check .


            // Saved directly to the database .
            CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
            da.SaveFKFieldAsync(Glo.FK_MapData, this.TB_KeyOfEn.Text.Trim(), this.TB_KeyOfName.Text.Trim(),
                this.SelectEnName,Glo.X,Glo.Y);
            da.SaveFKFieldCompleted += new EventHandler<SaveFKFieldCompletedEventArgs>(da_SaveFKFieldCompleted);
        }
        void da_SaveFKFieldCompleted(object sender, SaveFKFieldCompletedEventArgs e)
        {
            if (e.Result != "OK")
            {
                MessageBox.Show(e.Result);
                return;
            }
            this.DialogResult = true;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Btn_Del_Click(object sender, RoutedEventArgs e)
        {
            if (this.listBox1.SelectedIndex == -1)
            {
                MessageBox.Show(" Please select the item you want to delete .");
                return;
            }

            if (MessageBox.Show(" Are you sure you want to delete it ?", " Delete Confirmation ", MessageBoxButton.OKCancel)
                == MessageBoxResult.Cancel)
                return;

            ListBoxItem item = this.listBox1.SelectedItem as ListBoxItem;
            string[] kv = item.Content.ToString().Split(':');

            FF.CCFormSoapClient ff = Glo.GetCCFormSoapClientServiceInstance();
            ff.DoTypeAsync("DelSFTable",kv[0],null,null,null,null,null);
            ff.DoTypeCompleted += new EventHandler<FF.DoTypeCompletedEventArgs>(ff_DoTypeCompleted);
        }

        void ff_DoTypeCompleted(object sender, FF.DoTypeCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                MessageBox.Show(e.Result);
                return;
            }
            MessageBox.Show(" Deleted successfully ");
            this.BindData();
        }
        public SelectDDLTableEntity tableEntity = new SelectDDLTableEntity();
        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (this.listBox1.SelectedIndex == -1)
            {
                MessageBox.Show(" Please choose a dictionary table , Then the Edit button .", " Prompt ", MessageBoxButton.OK);
                return;
            }

            ListBoxItem item = this.listBox1.SelectedItem as ListBoxItem;
            string[] kv = item.Content.ToString().Split(':');

         ////   this.tableEntity.tabItem2.IsEnabled = false;
         //   this.tableEntity.OKButton.Content = " Determine ";
         //   this.tableEntity.TB_EnName.Text = kv[0];
         //   this.tableEntity.TB_EnName.IsEnabled=false;
         //   this.tableEntity.TB_CHName.Text = kv[1];
         //   this.tableEntity.Show();

            // 被zhoupeng 在 2014-10-24 Write off .

            string url = Glo.BPMHost + "/WF/Comm/RefFunc/UIEn.aspx?DoType=New&EnsName=BP.Sys.SFTables&PK="+kv[0];
            HtmlPage.Window.Eval("window.showModalDialog('" + url + "',window,'dialogHeight:450px;dialogWidth:780px;center:Yes;help:No;scroll:auto;resizable:1;status:No;');");
            this.Close();
        }

        private void Btn_DBSrc_Click(object sender, RoutedEventArgs e)
        {
            string url = Glo.BPMHost + "/WF/Comm/Search.aspx?DoType=New&EnsName=BP.Sys.SFDBSrcs&PK=";
            HtmlPage.Window.Eval("window.showModalDialog('" + url + "',window,'dialogHeight:450px;dialogWidth:980px;center:Yes;help:No;scroll:auto;resizable:1;status:No;');");
        }

        private void Btn_Create_Click(object sender, RoutedEventArgs e)
        {
            // 被zhoupeng 在 2014-10-24 Write off .
            ////   this.tableEntity.tabItem2.IsEnabled = false;
            //this.tableEntity.TB_EnName.IsEnabled = true;
            //this.tableEntity.OKButton.Content = " Determine ";
            //this.tableEntity.TB_CHName.Text = "";
            //this.tableEntity.TB_EnName.Text = "";
            //this.tableEntity.Show();

            string url = Glo.BPMHost + "/WF/Comm/Sys/SFGuide.aspx?DoType=New&MyPK=" + Glo.FK_MapData;
            HtmlPage.Window.Eval("window.showModalDialog('" + url + "',window,'dialogHeight:450px;dialogWidth:680px;center:Yes;help:No;scroll:auto;resizable:1;status:No;');");
            this.Close();
        }
    }
}

