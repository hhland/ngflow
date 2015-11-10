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
    public partial class FrmImgSeal : ChildWindow
    {
        public FrmImgSeal()
        {
            InitializeComponent();
        }

        public BPImgSeal HisImg = null;
        public string FileName { get; set; }
        public string FK_Dept { get; set; }
        public string FK_Station { get; set; }

        public void BindIt(BPImgSeal img)
        {
            HisImg = img;
            rdBtnDisable.IsChecked = true;
            rdBtnEnable.IsChecked = this.HisImg.IsEdit;
            TB_CN_Seal.Text = this.HisImg.TB_CN_Name;
            TB_En_Seal.Text = this.HisImg.TB_En_Name;
            // Determine whether to include department 
            if (this.HisImg.Tag0.Contains("^") && this.HisImg.Tag0.Split('^').Length == 4)
            {
                this.FK_Dept = this.HisImg.Tag0.Split('^')[0];
                this.FK_Station = this.HisImg.Tag0.Split('^')[1];
                if(this.FK_Dept == "all")
                {
                    this.DDL_SealType.SelectedIndex = Convert.ToInt32(string.IsNullOrEmpty(this.HisImg.Tag0.Split('^')[2]) ? "0":this.HisImg.Tag0.Split('^')[2]);
                    this.TB_SealField.Text = this.HisImg.Tag0.Split('^')[3];
                }
            }
            else
            {
                this.FK_Dept = "";
                this.FK_Station = this.HisImg.Tag0;
            }
            InitStationInfo();
            this.Show();
        }

        private void InitStationInfo()
        {
            string sql = "SELECT No,Name FROM Port_Dept ORDER BY Idx";
            FF.CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
            da.RunSQLReturnTableAsync(sql);
            da.RunSQLReturnTableCompleted += new EventHandler<FF.RunSQLReturnTableCompletedEventArgs>(da_RunSQLReturnDeptTableCompleted);
        }

        void da_RunSQLReturnDeptTableCompleted(object sender, FF.RunSQLReturnTableCompletedEventArgs e)
        {
            try
            {
                Silverlight.DataSet ds = new Silverlight.DataSet();
                ds.FromXml(e.Result);

                DDL_Dept.Items.Clear();
                ComboBoxItem allDept = new ComboBoxItem();
                allDept.Content = " Whole ";
                allDept.Tag = "all";
                DDL_Dept.Items.Add(allDept);

                foreach (Silverlight.DataRow dr in ds.Tables[0].Rows)
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = dr["Name"].ToString();
                    item.Tag = dr["No"].ToString();
                    if ((FK_Dept == dr["No"].ToString()))
                    {
                        item.IsSelected = true;
                    }
                    DDL_Dept.Items.Add(item);
                }
                // The first item is selected by default 
                if (string.IsNullOrEmpty(FK_Dept) || FK_Dept == "all")
                    DDL_Dept.SelectedIndex = 0;
                DDL_Dept.UpdateLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TB_En_Seal.Text.Trim()))
            {
                MessageBox.Show(" Signature English name can not be empty .");
                return;
            }
            // Available , And the field is empty judgment 
            if (this.TB_SealField.IsEnabled && this.DDL_SealType.IsEnabled && this.DDL_SealType.SelectedIndex == 2 && this.TB_SealField.Text == "")
            {
                MessageBox.Show(" Sector sources for form fields , Form field name can not be empty .");
                return;
            }
            //  Departments and positions set .
            string stations = "";
            ComboBoxItem lbi = (ComboBoxItem)DDL_Dept.SelectedItem;
            FK_Dept = lbi.Tag.ToString();
            // Add Sector 
            stations = FK_Dept + "^";
            foreach (CheckBox li in this.LB_Station.Items)
            {
                if (li.IsChecked == false)
                    continue;
                // Add post 
                stations += li.Tag.ToString() + ",";
            }
            // Adding sector sources 
            ComboBoxItem lbi_Dept = (ComboBoxItem)DDL_SealType.SelectedItem;
            stations += "^" + lbi_Dept.Tag.ToString();
            // Add field 
            stations += "^" + TB_SealField.Text;
            this.HisImg.Tag0 = stations;
            this.HisImg.IsEdit = (bool)rdBtnEnable.IsChecked;
            this.HisImg.TB_CN_Name = TB_CN_Seal.Text;
            this.HisImg.TB_En_Name = TB_En_Seal.Text;
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void TB_CN_Seal_LostFocus(object sender, RoutedEventArgs e)
        {
            Glo.GetKeyOfEn(this.TB_CN_Seal.Text, true, this.TB_En_Seal);
        }
       

        private void CB_Dept_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // The default is unavailable 
            this.DDL_SealType.IsEnabled = false;
            this.TB_SealField.IsEnabled = false;
            if (DDL_Dept.Items == null || DDL_Dept.Items.Count == 0)
            {
                LB_Station.Items.Clear();
                return;
            }

            ComboBoxItem lbi = (ComboBoxItem)DDL_Dept.SelectedItem;
            FK_Dept = lbi.Tag.ToString();
            string sql = "SELECT distinct b.No,b.Name FROM Port_DeptStation a,Port_Station b WHERE a.FK_Station = b.No AND a.FK_Dept='" + FK_Dept + "'";
            if (FK_Dept == "all")
            {
                // If it is set to all available 
                this.DDL_SealType.IsEnabled = true;
                this.TB_SealField.IsEnabled = true;
                sql = "SELECT distinct b.No,b.Name FROM Port_DeptStation a,Port_Station b WHERE a.FK_Station = b.No";
            }
            FF.CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
            da.RunSQLReturnTableAsync(sql);
            da.RunSQLReturnTableCompleted += new EventHandler<FF.RunSQLReturnTableCompletedEventArgs>(da_RunSQLReturnStationTableCompleted);
        }

        void da_RunSQLReturnStationTableCompleted(object sender, FF.RunSQLReturnTableCompletedEventArgs e)
        {
            try
            {
                Silverlight.DataSet ds = new Silverlight.DataSet();
                ds.FromXml(e.Result);

                this.LB_Station.Items.Clear();
                foreach (Silverlight.DataRow dr in ds.Tables[0].Rows)
                {
                    CheckBox lb = new CheckBox();
                    lb.Tag = dr["No"].ToString();
                    lb.Name = "ST" + dr["No"].ToString();
                    lb.Content = dr["Name"].ToString();
                    if (this.FK_Station.Contains(dr["No"].ToString()))
                    {
                        lb.IsChecked = true;
                    }
                    try
                    {
                        this.LB_Station.Items.Add(lb);
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

