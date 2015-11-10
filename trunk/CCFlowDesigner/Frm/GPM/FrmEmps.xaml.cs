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
    public partial class FrmEmps : ChildWindow
    {
        // Disclaimer commission 
        public delegate void ReFreshParent();
        // Disclaimer event 
        public event ReFreshParent ReFreshParentEve;
        private string _FK_Dept = "";
        public FrmEmps()
        {
            InitializeComponent();
        }
        /// <summary>
        ///  Initialization personnel information 
        /// </summary>
        public void InitEmps(string FK_Dept)
        {
            _FK_Dept = FK_Dept;
            // Get all personnel 
            string sql = "SELECT Top 200 No,Name,EmpNo FROM Port_Emp";
            sql += "@ select FK_Emp from Port_DeptEmp where FK_Dept = '" + _FK_Dept + "'";
            WSDesignerSoapClient da = Glo.GetDesignerServiceInstance();
           
            da.RunSQLReturnTableSAsync(sql);
            da.RunSQLReturnTableSCompleted += new EventHandler<RunSQLReturnTableSCompletedEventArgs>(InitDeptInfoEvent);
        }

        void InitDeptInfoEvent(object sender, RunSQLReturnTableSCompletedEventArgs e)
        {
            DataSet ds = new DataSet();
            ds.FromXml(e.Result);
            string isHaveEmpNos = "";// Staff numbers have been included in the collection 
            DataTable Port_Emp = ds.Tables[0]; // Personnel information .
            DataTable dt_Emps = ds.Tables[1]; // Included in the department staff 
            List<CheckListBoxModel> emps = new List<CheckListBoxModel>();
            // Finishing The department has included staff 
            foreach (DataRow empNoRow in dt_Emps.Rows)
            {
                isHaveEmpNos += empNoRow["FK_Emp"] + ",";
            }
            // Add Item 
            foreach (DataRow row in Port_Emp.Rows)
            {
                CheckListBoxModel emp = new CheckListBoxModel();
                emp.ID = row["No"].ToString();
                emp.ModelName = row["No"].ToString() + "(" + row["EmpNo"] + ")-" + row["Name"];
                if (isHaveEmpNos.Contains(row["No"].ToString() + ","))
                {
                    emp.IsSelected = true;
                }
                emps.Add(emp);
            }
            CKB_Emps.ItemsSource = emps.ToList();
            CKB_Emps.UpdateLayout();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            string empNos = "";
            IEnumerable<BP.CheckListBoxModel> list = (IEnumerable<BP.CheckListBoxModel>)CKB_Emps.ItemsSource;
            IEnumerable<BP.CheckListBoxModel> selectedList = list.Where(a => a.IsSelected == true);

            // Gets a collection of personnel numbers 
            foreach (BP.CheckListBoxModel emp in selectedList)
            {
                if (empNos.Length == 0)
                    empNos += emp.ID;
                else
                    empNos += "^" + emp.ID;
            }
            if (empNos == "")
            {
                MessageBox.Show(" Please select staff ."," Prompted ", MessageBoxButton.OK);
                return;
            }
            WSDesignerSoapClient da = Glo.GetDesignerServiceInstance();
           
          
            da.Dept_Emp_RelatedAsync(empNos, _FK_Dept);
            da.Dept_Emp_RelatedCompleted += new EventHandler<Dept_Emp_RelatedCompletedEventArgs>(da_Dept_Emp_RelatedCompleted);
        }

        
        void da_Dept_Emp_RelatedCompleted(object sender, Dept_Emp_RelatedCompletedEventArgs e)
        {
            if (e.Result.Contains("error") == true)
            {
                MessageBox.Show(e.Result, " Associated with failure ", MessageBoxButton.OK);
                return;
            }
            ReFreshParentEve();
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        // Inquiry 
        private void Btn_Query_Click(object sender, RoutedEventArgs e)
        {
            string info = TB_Content.Text;
            // Get all personnel 
            string sql = "SELECT Top 200 No,Name,EmpNo FROM Port_Emp WHERE No like '%" + info + "%' OR Name like '%" + info + "%' OR EmpNo like '%" + info + "%'";
            sql += "@ select FK_Emp from Port_DeptEmp where FK_Dept = '" + _FK_Dept + "'";
            WSDesignerSoapClient da = Glo.GetDesignerServiceInstance();
           
            da.RunSQLReturnTableSAsync(sql);
            da.RunSQLReturnTableSCompleted += new EventHandler<RunSQLReturnTableSCompletedEventArgs>(InitDeptInfoEvent);
        }
    }
}

