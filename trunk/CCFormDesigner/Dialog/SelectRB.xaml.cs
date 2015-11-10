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
using Silverlight;
using CCForm.FF;

namespace CCForm
{
    public partial class SelectRB : ChildWindow
    {
        public SelectRB()
        {
            InitializeComponent();
            //this.listBox1.SelectionMode = SelectionMode.Single;
            //this.listBox2.SelectionMode = SelectionMode.Single;
        }
        protected override void OnOpened()
        {
            if (this.listBox1.Items.Count == 0)
            {
                this.BindData();
                this.tabItem1.IsSelected = true;
                this.tabItem2.Header = " New ";
            }
            base.OnOpened();
        }
        public void BindData()
        {
            this.listBox1.Items.Clear();
            this.listBox2.Items.Clear();

            string sql = "SELECT No,Name,CfgVal FROM Sys_EnumMain order by Name";
            CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
            da.RunSQLReturnTableAsync(sql);
            da.RunSQLReturnTableCompleted += new EventHandler<FF.RunSQLReturnTableCompletedEventArgs>(da_RunSQLReturnTableCompleted);
            this.listBox1.SelectionChanged += new SelectionChangedEventHandler(listBox1_SelectionChanged);
        }

        void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListBoxItem li = e.AddedItems[0] as ListBoxItem;
                string[] itemName = li.Content.ToString().Split(':');
                this.TB_KeyOfEn.Text = itemName[0];
                this.TB_KeyOfName.Text = itemName[1];

                string s = li.Tag as string;
                string[] strs = s.Split('@');
                this.listBox2.Items.Clear();
                foreach (string str in strs)
                {
                    if (string.IsNullOrEmpty(str))
                        continue;
                    ListBoxItem dd = new ListBoxItem();
                    dd.Content = str;
                    this.listBox2.Items.Add(dd);
                }
                this.EditItem(li);
            }
            catch
            {
            }
        }
        /// <summary>
        /// da_RunSQLReturnTableCompleted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void da_RunSQLReturnTableCompleted(object sender, FF.RunSQLReturnTableCompletedEventArgs e)
        {
            DataSet ds = new DataSet();
            ds.FromXml(e.Result);
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                ListBoxItem item = new ListBoxItem();
                item.Tag = dr["CfgVal"].ToString();
                item.Content = dr["No"] + ":" + dr["Name"];
                this.listBox1.Items.Add(item);
            }
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
             



             ListBoxItem lbi = this.listBox1.SelectedItem as ListBoxItem;
                string enumKey = lbi.Content.ToString();
                enumKey = enumKey.Substring(0, enumKey.IndexOf(':'));


            CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
            da.SaveEnumFieldAsync(Glo.FK_MapData, this.TB_KeyOfEn.Text, this.TB_KeyOfName.Text, enumKey,Glo.X, Glo.Y);
            da.SaveEnumFieldCompleted += new EventHandler<SaveEnumFieldCompletedEventArgs>(da_SaveEnumFieldCompleted);

        }

        void da_SaveEnumFieldCompleted(object sender, SaveEnumFieldCompletedEventArgs e)
        {
            if (e.Result == "OK")
            {
                this.DialogResult = true;
                return;
            }

            MessageBox.Show(e.Result);
        }

        //void da_NewFieldsCompleted(object sender, NewFieldsCompletedEventArgs e)
        //{
        //    if (e.Result == null)
        //    {
        //        this.DialogResult = true;
        //    }
        //    else
        //    {
        //        MessageBox.Show( e.Result, "Note", MessageBoxButton.OK);
        //        return;
        //    }
        //}

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Btn_New_Click(object sender, RoutedEventArgs e)
        {
            this.TB_No.Text = "";
            this.TB_Name.Text = "";
            this.tabItem2.IsSelected = true;
            this.tabItem2.Header = " New ";

            this.listBox3.Items.Clear();
            int idx = 0;
            for (int i = idx; i < 20; i++)
            {
                TextBox tb = new TextBox();
                tb.Name = "TB_" + i;
                tb.Text = "";
                tb.Width = this.listBox3.Width;

                ListBoxItem lb = new ListBoxItem();
                lb.Name = "LB_" + i;
                lb.Content = tb;
                this.listBox3.Items.Add(lb);
            }
        }
        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem li = this.listBox1.SelectedItem as ListBoxItem;
            this.EditItem(li);
            this.tabItem2.IsSelected = true;

        }
        public void EditItem(ListBoxItem li)
        {
            string myStr = li.Content as string;
            this.TB_No.Text = myStr.Substring(0, myStr.IndexOf(':'));
            this.TB_Name.Text = myStr.Substring(myStr.IndexOf(':') + 1);
            this.tabItem2.Header = " Editor :" + this.TB_Name.Text;

            this.listBox3.Items.Clear();
            string s = li.Tag as string;
            string[] strs = s.Split('@');
            this.listBox3.Items.Clear();
            int idx = 0;
            foreach (string str in strs)
            {
                if (string.IsNullOrEmpty(str))
                    continue;

                string[] mystrs = str.Split('=');

                TextBox tb = new TextBox();
                tb.Name = "TB_" + idx;
                tb.Text = mystrs[1];
                tb.Width = this.listBox3.Width;

                ListBoxItem lb = new ListBoxItem();
                lb.Name = "LB_" + idx;
                lb.Content = tb;
                this.listBox3.Items.Add(lb);
                idx++;
            }

            idx++;
            for (int i = idx; i < 20; i++)
            {
                TextBox tb = new TextBox();
                tb.Name = "TB_" + i;
                tb.Text = "";
                tb.Width = this.listBox3.Width;

                ListBoxItem lb = new ListBoxItem();
                lb.Name = "LB_" + i;
                lb.Content = tb;
                this.listBox3.Items.Add(lb);
            }
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            string keys = "";
            int idx = 0;
            foreach (ListBoxItem li in this.listBox3.Items)
            {
                TextBox tb = li.Content as TextBox;
                if (string.IsNullOrEmpty(tb.Text))
                    continue;

                keys += "@" + idx + "=" + tb.Text;
                idx++;
            }
            FF.CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
            da.SaveEnumAsync(this.TB_No.Text, this.TB_Name.Text, keys);
            da.SaveEnumCompleted += new EventHandler<SaveEnumCompletedEventArgs>(da_SaveEnumCompleted);
        }

        void da_SaveEnumCompleted(object sender, SaveEnumCompletedEventArgs e)
        {
            this.BindData();

            //this.listBox1.SelectedIndex = 0;

            MessageBox.Show(" Saved successfully ");
            // this.tabItem1.IsSelected = true;
            //this.listBox1.see
        }

        private void Btn_Del_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("are you sure?", "Note",
                MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;

            FF.CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
            da.DoTypeAsync("DelEnum", this.TB_No.Text.Trim(), null, null, null, null);
            da.DoTypeCompleted += new EventHandler<DoTypeCompletedEventArgs>(da_DoTypeCompleted);
        }

        void da_DoTypeCompleted(object sender, DoTypeCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                MessageBox.Show(e.Result, " Error ", MessageBoxButton.OK);
                return;
            }
        
            MessageBox.Show(" Deleted successfully ", " Prompt ", MessageBoxButton.OK);
            this.tabItem1.IsSelected = true;
            this.TB_No.Text = "";
            this.TB_Name.Text = "";
            this.listBox3.Items.Clear();
            this.listBox1.Items.Clear();
            this.BindData();
        }

        private void TB_Name_LostFocus(object sender, RoutedEventArgs e)
        {
            Glo.GetKeyOfEn(this.TB_Name.Text, true, this.TB_No);
        }
      
    }
}

