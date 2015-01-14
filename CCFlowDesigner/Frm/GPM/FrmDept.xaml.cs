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
using WF.WS;
using BP;

namespace OrganizationalStructure
{
    public partial class FrmDept : ChildWindow
    {
        /// <summary>
        ///  Execution Type 
        /// </summary>
        public string doType = "EditDept";
        /// <summary>
        ///  The current department 
        /// </summary>
        public string currDeptNo = "";
        // Disclaimer commission 
        public delegate void ReFreshParent();
        public event ReFreshParent ReFreshParentEve; // Disclaimer event 
        public FrmDept()
        {
            InitializeComponent();
        }
        /// <summary>
        ///  Initialization department information 
        /// </summary>
        /// <param name="doType"> Execution Type </param>
        /// <param name="deptNo"> Department number </param>
        public void InitDeptInfo(string doType, string deptNo)
        {
            this.doType = doType;
            this.currDeptNo = deptNo;

            if (this.doType == "EditDept")
                this.TB_No.IsReadOnly = true;
            else
                this.TB_No.IsReadOnly = false;

            string sql = "SELECT No,Name,FK_DeptType,Leader FROM Port_Dept WHERE No='" + this.currDeptNo + "'"; //  Sector Information .
            sql += "@ SELECT FK_Dept,FK_Duty FROM Port_DeptDuty WHERE FK_Dept='" + this.currDeptNo + "'"; //  The current division of duties .
            sql += "@ SELECT FK_Dept,FK_Station FROM Port_DeptStation WHERE FK_Dept='" + this.currDeptNo + "'"; //  Current job sector .
            sql += "@ SELECT No,Name FROM Port_DeptType "; //  Sector Type .
            sql += "@ SELECT No,Name FROM Port_Duty "; // Position .
            sql += "@ SELECT No,Name FROM Port_Station "; // Post 
            sql += "@ SELECT No,Name FROM Port_StationType "; //  Post Type .

            WSDesignerSoapClient da = Glo.GetDesignerServiceInstance();
           
            da.RunSQLReturnTableSAsync(sql);
            da.RunSQLReturnTableSCompleted += InitDeptInfoEvent;
        }
        public DataTable Port_Dept = null;
        public DataTable Port_DeptDuty = null;
        public DataTable Port_DeptStation = null;
        public DataTable Port_DeptType = null;
        public DataTable Port_Duty = null;
        public DataTable Port_Station = null;
        public DataTable Port_StationType = null;

        void InitDeptInfoEvent(object sender, RunSQLReturnTableSCompletedEventArgs e)
        {
            DataSet ds = new DataSet();
            ds.FromXml(e.Result);
            Port_Dept = ds.Tables[0]; // Sector Information .
            Port_DeptDuty = ds.Tables[1]; // The current division of duties .

            Port_DeptStation = ds.Tables[2]; // Current job sector .
            Port_DeptType = ds.Tables[3]; // Sector Type .

            Port_Duty = ds.Tables[4]; // Position .
            Port_Station = ds.Tables[5]; // Post .
            Port_StationType = ds.Tables[6]; // Post Type .

            #region  Binding basic information .
            if (this.doType == "EditDept")
            {
                if (Port_Dept.Rows.Count == 1)
                {
                    this.TB_No.Text = Port_Dept.Rows[0]["No"];
                    this.TB_Name.Text = Port_Dept.Rows[0]["Name"];
                    this.TB_Leader.Text = Port_Dept.Rows[0]["Leader"] == null ? "" : Port_Dept.Rows[0]["Leader"];
                }

                // Type binding sector .
                if (Port_Dept.Rows.Count == 1)
                    BP.Glo.Ctrl_DDL_BindDataTable(this.DDL_DeptType, Port_DeptType, Port_Dept.Rows[0]["FK_DeptType"]);
                else
                    BP.Glo.Ctrl_DDL_BindDataTable(this.DDL_DeptType, Port_DeptType, null);
            }
            else
            {
                this.Btn_Delete.IsEnabled = false;
                this.TB_No.Text = "";
                this.TB_Name.Text = "";
                this.TB_Leader.Text = "";

                BP.Glo.Ctrl_DDL_BindDataTable(this.DDL_DeptType, Port_DeptType, null);
            }
            #endregion  Binding basic information .

            #region  Binding departments and positions corresponding information .
            this.LB_Duty.Items.Clear();
            foreach (DataRow dr in Port_Duty.Rows)
            {
                CheckBox lb = new CheckBox();
                lb.Tag = dr["No"].ToString();
                lb.Name = dr["No"].ToString();
                lb.Content = dr["Name"].ToString();
                foreach (DataRow drIt in Port_DeptDuty.Rows)
                {
                    if (string.IsNullOrEmpty(drIt["FK_Duty"]))
                        continue;

                    if (drIt["FK_Duty"].ToString() == dr["No"].ToString())
                    {
                        lb.IsChecked = true;
                        break;
                    }
                }
                this.LB_Duty.Items.Add(lb);
            }
            #endregion  Binding departments and positions corresponding information .

            #region  Binding corresponding departments and job information .
            this.LB_Station.Items.Clear();
            foreach (DataRow dr in Port_Station.Rows)
            {
                CheckBox lb = new CheckBox();
                lb.Tag = dr["No"].ToString();
                lb.Name = "ST" + dr["No"].ToString();
                lb.Content = dr["Name"].ToString();
                foreach (DataRow drIt in Port_DeptStation.Rows)
                {
                    if (drIt["FK_Station"].ToString() == dr["No"].ToString())
                    {
                        lb.IsChecked = true;
                        break;
                    }
                }

                try
                {
                    this.LB_Station.Items.Add(lb);
                }
                catch
                {
                }
            }
            #endregion  Binding corresponding departments and job information .
        }
        /// <summary>
        ///  Save 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.TB_Name.Text))
            {
                MessageBox.Show(" Department name can not be empty ", " Prompted ", MessageBoxButton.OK);
                return;
            }
            // Property sector .
            string attrs = "^Name=" + this.TB_Name.Text + "^FK_DeptType=" + BP.Glo.GetDDLValOfString(this.DDL_DeptType);
            attrs += "^Leader=" + this.TB_Leader.Text;
            //  Post collection .
            string stations = "";
            foreach (CheckBox li in this.LB_Station.Items)
            {
                if (li.IsChecked == false)
                    continue;
                stations += li.Tag.ToString() + ",";
            }

            //  Duty collection .
            string dutys = "";
            foreach (CheckBox li in this.LB_Duty.Items)
            {
                if (li.IsChecked == false)
                    continue;
                dutys += li.Tag.ToString() + ",";
            }
            WSDesignerSoapClient da = Glo.GetDesignerServiceInstance();
           
            if (this.doType == "EditDept")
            {
                /* If you are editing */
                da.Dept_EditAsync(this.TB_No.Text, attrs, stations, dutys, (bool)CB_AddStation.IsChecked);
                da.Dept_EditCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(da_Dept_EditCompleted);
            }

            if (this.doType == "CrateSameLevel")
            {
                /* If the sector is to establish the same level */
                da.Dept_CreateSameLevelAsync(this.currDeptNo, attrs, stations, dutys);
                da.Dept_CreateSameLevelCompleted +=new EventHandler<Dept_CreateSameLevelCompletedEventArgs>(da_Dept_CreateSameLevelCompleted);
            }

            if (this.doType == "CrateSubLevel")
            {
                /* If it is established subordinate departments */
                da.Dept_CreateSubLevelAsync(this.currDeptNo, attrs, stations, dutys);
                da.Dept_CreateSubLevelCompleted += new EventHandler<Dept_CreateSubLevelCompletedEventArgs>(da_Dept_CreateSubLevelCompleted);
            }
        }

        void da_Dept_CreateSubLevelCompleted(object sender, Dept_CreateSubLevelCompletedEventArgs e)
        {
            if (e.Result.Contains("err"))
            {
                MessageBox.Show(e.Result, " Prompted ", MessageBoxButton.OK);
                return;
            }
            // Refresh the parent form 
            if (ReFreshParentEve != null) ReFreshParentEve();
            this.DialogResult = true;
        }
        void da_Dept_CreateSameLevelCompleted(object sender, Dept_CreateSameLevelCompletedEventArgs e)
        {
            if (e.Result.Contains("err"))
            {
                MessageBox.Show(e.Result, " Prompted ", MessageBoxButton.OK);
                return;
            }
            // Refresh the parent form 
            if (ReFreshParentEve != null) ReFreshParentEve();
            this.DialogResult = true;
        }
        void da_Dept_EditCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            // Refresh the parent form 
            if (ReFreshParentEve != null) ReFreshParentEve();
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(" Are you sure you want to delete it ?", " Prompt ", MessageBoxButton.OKCancel)
                == MessageBoxResult.Cancel)
                return;
            WSDesignerSoapClient da = Glo.GetDesignerServiceInstance();
           
            da.Dept_DeleteAsync(this.TB_No.Text, false);
            da.Dept_DeleteCompleted += new EventHandler<Dept_DeleteCompletedEventArgs>(da_Dept_DeleteCompleted);
        }

        void da_Dept_DeleteCompleted(object sender, Dept_DeleteCompletedEventArgs e)
        {
            if (e.Result.Contains("err"))
            {
                if (MessageBox.Show(e.Result + ", Are you sure you want to delete it mandatory ?", " Prompt ", MessageBoxButton.OKCancel)
                == MessageBoxResult.OK)
                {
                    WSDesignerSoapClient da = Glo.GetDesignerServiceInstance();
           
                    da.Dept_DeleteAsync(this.TB_No.Text, true);
                    da.Dept_DeleteCompleted += new EventHandler<Dept_DeleteCompletedEventArgs>(da_Dept_DeleteCompleted);
                }
            }
            else
            {
                MessageBox.Show(e.Result, " Deleted successfully ", MessageBoxButton.OK);
                // Refresh the parent form 
                if (ReFreshParentEve != null) ReFreshParentEve();
                this.DialogResult = true;
            }
        }
        // Check post 
        private void BT_SearchStation_Click(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT FK_Dept,FK_Station FROM Port_DeptStation WHERE FK_Dept='" + this.currDeptNo + "'"; //  Current job sector .
            sql += "@ SELECT No,Name FROM Port_Station WHERE Name like '%" + TB_StationName.Text + "%'"; // Post 
            WSDesignerSoapClient da = Glo.GetDesignerServiceInstance();
           
            da.RunSQLReturnTableSAsync(sql);
            da.RunSQLReturnTableSCompleted += new EventHandler<RunSQLReturnTableSCompletedEventArgs>(InitDeptStationInfoEvent);
        }

        void InitDeptStationInfoEvent(object sender, RunSQLReturnTableSCompletedEventArgs e)
        {
            DataSet ds = new DataSet();
            ds.FromXml(e.Result);
            Port_DeptStation = ds.Tables[0]; // Current job sector .
            Port_Station = ds.Tables[1]; // Post .

            #region  Binding corresponding departments and job information .
            this.LB_Station.Items.Clear();
            foreach (DataRow dr in Port_Station.Rows)
            {
                CheckBox lb = new CheckBox();
                lb.Tag = dr["No"].ToString();
                lb.Name = "ST" + dr["No"].ToString();
                lb.Content = dr["Name"].ToString();
                foreach (DataRow drIt in Port_DeptStation.Rows)
                {
                    if (drIt["FK_Station"].ToString() == dr["No"].ToString())
                    {
                        lb.IsChecked = true;
                        break;
                    }
                }

                try
                {
                    this.LB_Station.Items.Add(lb);
                }
                catch
                {
                }
            }
            #endregion  Binding corresponding departments and job information .
        }
    }
}

