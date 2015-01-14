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
using BP;
using WF.WS;

namespace OrganizationalStructure
{
    public partial class FrmDeptEmp : ChildWindow
    {
        /// <summary>
        ///  The current department number 
        /// </summary>
        public string FK_Dept = "";
        public string doType = "Edit";
        public string EmpNo = null;
        // Disclaimer commission 
        public delegate void ReFreshParent();
        // Disclaimer event 
        public event ReFreshParent ReFreshParentEve; 
        /// <summary>
        ///  Department personnel information 
        /// </summary>
        public FrmDeptEmp()
        {
            InitializeComponent();
        }
        /// <summary>
        ///  Binding personnel information 
        /// </summary>
        /// <param name="doType"> Execution Type </param>
        /// <param name="fk_dept"> Department number </param>
        /// <param name="empNo"> Personnel Number </param>
        public void InitEmp(string doType, string fk_dept, string empNo)
        {
            this.doType = doType;
            this.FK_Dept = fk_dept;
            this.EmpNo = empNo;

            // Added time , Delete unavailable 
            if (this.doType == "New")
            {
                Btn_Del.IsEnabled = false;
                Btn_ResetPass.IsEnabled = false;
            }

            string sql = "SELECT * FROM Port_Emp WHERE No='" + this.EmpNo + "'"; //  Personnel information .
            sql += "@ SELECT FK_Dept,FK_Emp,Leader,FK_Duty,DutyLevel FROM Port_DeptEmp WHERE FK_Dept='" + this.FK_Dept + "' AND FK_Emp='" + this.EmpNo + "'"; //  Staff in the information department of the current .
            sql += "@ SELECT No,Name FROM Port_Station WHERE No IN (SELECT FK_Station FROM Port_DeptEmpStation WHERE FK_Dept='" + this.FK_Dept + "' AND FK_Emp='" + this.EmpNo + "')"; //  The current staff positions in this sector collection .

            sql += "@ SELECT No,Name FROM Port_Duty WHERE No IN (SELECT FK_Duty FROM Port_DeptDuty WHERE FK_Dept='" + this.FK_Dept + "')"; //  The current division of duties set .
            sql += "@ SELECT No,Name FROM Port_Station WHERE No IN (SELECT FK_Station FROM Port_DeptStation WHERE FK_Dept='" + this.FK_Dept + "')"; //  The current set of job sectors .

            WSDesignerSoapClient da = Glo.GetDesignerServiceInstance();
           
            
            da.RunSQLReturnTableSAsync(sql);
            da.RunSQLReturnTableSCompleted += new EventHandler<RunSQLReturnTableSCompletedEventArgs>(InitDeptInfoEvent);
        }

        void InitDeptInfoEvent(object sender, RunSQLReturnTableSCompletedEventArgs e)
        {
            DataSet ds = new DataSet();
            ds.FromXml(e.Result);

            DataTable Port_Emp = ds.Tables[0]; // Sector Information .
            DataTable Port_DeptEmp = ds.Tables[1]; // Information about the current staff and departments .

            DataTable StationsOfEmp = ds.Tables[2]; // Current job sector .

            DataTable DutysOfDept = ds.Tables[3]; // The current division of duties 
            DataTable StationsOfDept = ds.Tables[4]; // Current job sector .

            if (DutysOfDept.Rows.Count == 0 || StationsOfDept.Rows.Count == 0)
            {
                if (MessageBox.Show(" There is no correspondence between the posts and positions in this sector , So you can not increase the staff .\t\n  You now want to maintain the correspondence between this sector jobs and positions it ?",
                     " Error ", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    FrmDept fd = new FrmDept();
                    fd.InitDeptInfo("EditDept", this.FK_Dept);
                    fd.Show();
                    this.DialogResult = false;
                }
                return;
            }

            #region  Binding basic information .
            if (Port_DeptEmp.Rows.Count == 1)
            {
                this.TB_No.Text = Port_Emp.Rows[0]["No"];
                this.TB_Name.Text = Port_Emp.Rows[0]["Name"];
                this.TB_EmpNo.Text = Port_Emp.Rows[0]["EmpNo"] == null ? "" : Port_Emp.Rows[0]["EmpNo"];
                this.TB_Tel.Text = Port_Emp.Rows[0]["Tel"] == null ? "" : Port_Emp.Rows[0]["Tel"];
                this.TB_Email.Text = Port_Emp.Rows[0]["Email"] == null ? "" : Port_Emp.Rows[0]["Email"];

                // Bind its duties .
                Glo.Ctrl_DDL_BindDataTable(this.DDL_Duty, DutysOfDept, Port_DeptEmp.Rows[0]["FK_Duty"]);

                //  Its leadership ,
                this.TB_Leader.Text = Port_DeptEmp.Rows[0]["Leader"] == null ? "" : Port_DeptEmp.Rows[0]["Leader"];
                // Position Level 
                this.TB_Level.Text = Port_DeptEmp.Rows[0]["DutyLevel"] == null ? "" : Port_DeptEmp.Rows[0]["DutyLevel"];
            }
            else
            {
                //  Binding posts collection .
                Glo.Ctrl_DDL_BindDataTable(this.DDL_Duty, DutysOfDept, null);
            }
            #endregion  Binding basic information .

            #region  Binding corresponding departments and job information .
            this.LB_Station.Items.Clear();
            foreach (DataRow dr in StationsOfDept.Rows)
            {
                CheckBox lb = new CheckBox();
                lb.Tag = dr["No"].ToString();
                lb.Name = dr["No"].ToString();
                lb.Content = dr["Name"].ToString();
                foreach (DataRow drIt in StationsOfEmp.Rows)
                {
                    if (drIt["No"].ToString() == dr["No"].ToString())
                    {
                        lb.IsChecked = true;
                        break;
                    }
                }
                this.LB_Station.Items.Add(lb);
            }
            #endregion  Binding corresponding departments and job information .
        }
        /// <summary>
        ///  Button event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            string err = "";
            if (this.TB_No.Text.Length == 0)
                err += " Staff numbers can not be empty .";

            if (this.TB_Name.Text.Length == 0)
                err += " Person name can not be empty .";

            if (BP.Glo.IsNum(this.TB_Level.Text) == false)
                err += " Duty level must be numeric type .";

            if (string.IsNullOrEmpty(err) == false)
                throw new Exception(err);

            string attrs = "^Name=" + this.TB_Name.Text + "^FK_Duty=" + BP.Glo.GetDDLValOfString(this.DDL_Duty);
            attrs += "^FK_Dept=" + this.FK_Dept;
            attrs += "^DutyLevel=" + this.TB_Level.Text;
            attrs += "^No=" + this.TB_No.Text;
            attrs += "^EmpNo=" + this.TB_EmpNo.Text;
            attrs += "^Leader=" + this.TB_Leader.Text;
            attrs += "^Tel=" + this.TB_Tel.Text;
            attrs += "^Email=" + this.TB_Email.Text;

            //  Post collection .
            string stations = "";
            foreach (CheckBox li in this.LB_Station.Items)
            {
                if (li.IsChecked == false)
                    continue;
                stations += li.Tag.ToString() + ",";
            }
            WSDesignerSoapClient da = Glo.GetDesignerServiceInstance();
           
            
          
            if (this.doType == "Edit")
            {
                da.Emp_EditAsync(this.TB_No.Text, this.FK_Dept, attrs, stations);
                da.Emp_EditCompleted +=new EventHandler<Emp_EditCompletedEventArgs>(da_Emp_EditCompleted);
            }

            if (this.doType == "New")
            {
                da.Emp_NewAsync(this.TB_No.Text, this.FK_Dept, attrs, stations);
                da.Emp_NewCompleted +=new EventHandler<Emp_NewCompletedEventArgs>(da_Emp_NewCompleted);
            }
        }
        void da_Emp_NewCompleted(object sender, Emp_NewCompletedEventArgs e)
        {
         //    MessageBox.Show(e.Result, " New Success ", MessageBoxButton.OK);
            // Refresh the parent form 
            if (ReFreshParentEve != null) ReFreshParentEve();
            this.DialogResult = true;
        }
        void da_Emp_EditCompleted(object sender, Emp_EditCompletedEventArgs e)
        {
          //  MessageBox.Show(e.Result, " Edited successfully ", MessageBoxButton.OK);
            // Refresh the parent form 
            if (ReFreshParentEve != null) ReFreshParentEve();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        /// <summary>
        ///  Delete 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Del_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(" Are you sure you want to delete it ?", " Prompt ", MessageBoxButton.OKCancel)
                == MessageBoxResult.Cancel)
                return;
            WSDesignerSoapClient da = Glo.GetDesignerServiceInstance();
           
            
            da.Emp_DeleteAsync(this.TB_No.Text, this.FK_Dept);
            da.Emp_DeleteCompleted +=new EventHandler<Emp_DeleteCompletedEventArgs>(da_Emp_DeleteCompleted);
        }

        void da_Emp_DeleteCompleted(object sender, Emp_DeleteCompletedEventArgs e)
        {
            if (e.Result.Contains("error") == true)
            {
                MessageBox.Show(e.Result, " Delete failed ", MessageBoxButton.OK);
                return;
            }
            else
            {
                MessageBox.Show(e.Result, " Success ", MessageBoxButton.OK);
                // Refresh the parent form 
                if (ReFreshParentEve != null) ReFreshParentEve();
                this.DialogResult = true;
            }
        }

        private void Btn_Copy_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(" This feature does not currently support ", " Information ", MessageBoxButton.OK);
        }
        // Name to Pinyin 
        private void TB_Name_LostFocus(object sender, RoutedEventArgs e)
        {
            WSDesignerSoapClient da = Glo.GetDesignerServiceInstance();
           
            da.ParseStringToPinyinAsync( this.TB_Name.Text);
            da.ParseStringToPinyinCompleted += new EventHandler<ParseStringToPinyinCompletedEventArgs>(da_ParseStringToPinyinCompleted);
        }

        void da_ParseStringToPinyinCompleted(object sender, ParseStringToPinyinCompletedEventArgs e)
        {
            if (e.Result != null)
                this.TB_No.Text = e.Result.ToLower();
        }

        private void Btn_Move_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(" This feature does not currently support ", " Information ", MessageBoxButton.OK);
        }
        /// <summary>
        ///  Reset Password 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_ResetPass_Click(object sender, RoutedEventArgs e)
        {
            WSDesignerSoapClient da = Glo.GetDesignerServiceInstance();
           
            da.DoAsync("ResetPassword", this.EmpNo, true);
            da.DoCompleted += new EventHandler<DoCompletedEventArgs>(da_DoCompleted);
        }

        void da_DoCompleted(object sender, DoCompletedEventArgs e)
        {
            if (e.Result != null && e.Result.Contains("err"))
            {
                MessageBox.Show(e.Result, " Execution error ", MessageBoxButton.OK);
                return;
            }
            MessageBox.Show(" Current users can 123 Log in .", " Password Reset Success ", MessageBoxButton.OK);
        }
        private void Btn_SaveClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.OKButton_Click(sender, e);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, " Failed to save ", MessageBoxButton.OK);
                return;
            }

            //// Refresh the parent form 
            //if (ReFreshParentEve != null)
            //    ReFreshParentEve();

            this.DialogResult = true;
        }
    }
}

