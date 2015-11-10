using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Port;
using BP.Web;

namespace CCFlow.WF.UC
{
    public partial class ChangeDept : BP.Web.UC.UCBase3
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string toDept = this.Request.QueryString["FK_Dept"];
            if (string.IsNullOrEmpty(toDept)==false)
            {
                string updataSQL = BP.WF.Glo.UpdataMainDeptSQL.Clone() as string;
                updataSQL = updataSQL.Replace("@FK_Dept", toDept);
                updataSQL = updataSQL.Replace("@No", WebUser.No);
                BP.DA.DBAccess.RunSQL(updataSQL);

                this.AddFieldSet(" Prompt ");
                this.Add(" Successful handover !");
                this.AddFieldSetEnd();
                return;
            }

            if (WebUser.No == null)
            {
                this.AddFieldSet(" Prompt ");
                this.Add(" You are not logged in !!!!");
                this.AddFieldSetEnd();
                return;
            }

            string sql = "select No,Name FROM Port_Dept WHERE No IN (select fk_dept from port_EmpDept where fk_emp='" + WebUser.No + "')";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 1)
            {
                this.AddFieldSet(" Prompt :");
                this.Add(" You only have one department , So you can not switch Login .");
                this.AddFieldSetEnd();
                return;
            }

            if (dt.Rows.Count == 0)
            {
                this.AddFieldSet(" System error :");
                this.Add(" Can not find your department collection , Please inform the administrator .");
                this.AddFieldSetEnd();
                return;
            }


            this.AddFieldSet(" Hello :" + WebUser.No + "," + WebUser.Name + ";  The current department :" + WebUser.FK_DeptName);

            this.AddUL();
            foreach (DataRow dr in dt.Rows)
            {
                string fk_dept = dr["No"].ToString();
                if (fk_dept == WebUser.FK_Dept)
                    this.AddLi(WebUser.FK_DeptName);
                else
                    this.AddLi("<a href='ChangeDept.aspx?FK_Dept=" + dr["No"] + "'>" + dr["Name"] + "</a>");
            }
            this.AddULEnd();

            this.AddFieldSetEnd();
            return;
        }
    }
}