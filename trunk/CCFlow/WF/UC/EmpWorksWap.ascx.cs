using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BP.WF;
using BP.Port;
using BP.Sys;
using BP.Web.Controls;
using BP.DA;
using BP.En;
using BP.Web;
namespace CCFlow.WF.UC
{
    public partial class EmpWorksWap : BP.Web.UC.UCBase3
    {
        public string FK_Flow
        {
            get
            {
                string s = this.Request.QueryString["FK_Flow"];
                if (s == null)
                    return this.ViewState["FK_Flow"] as string;
                return s;
            }
            set
            {
                this.ViewState["FK_Flow"] = value;
            }
        }
        public Int64 WorkID
        {
            get
            {
                if (this.Request.QueryString["WorkID"] == null)
                    return 0;

                return Int64.Parse(this.Request.QueryString["WorkID"]);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "Work on the way.";

            this.AddFieldSet("<img src='/WF/Img/Home.gif' ><a href='Home.aspx' >Home</a>-<img src='/WF/Img/EmpWorks.gif' > <b> Upcoming work </b>");
            this.AddUL();

            string sql = "SELECT * FROM WF_EmpWorks WHERE FK_Emp='" + BP.Web.WebUser.No + "' ORDER BY WorkID ";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            DateTime cdt = DateTime.Now;
            foreach (DataRow dr in dt.Rows)
            {
                string sdt = dr["SDT"] as string;
                DateTime mysdt = DataType.ParseSysDate2DateTime(sdt);
                if (cdt >= mysdt)
                {
                    this.AddLi("<font color=red ><a href=\"MyFlow.aspx?FK_Flow=" + dr["FK_Flow"] + "&WorkID=" + dr["WorkID"] + "\" >" + dr["Title"].ToString() + " - " + dr["NodeName"].ToString() + "</a></font>");
                }
                else
                {
                    this.AddLi("<a href=\"MyFlow.aspx?FK_Flow=" + dr["FK_Flow"] + "&WorkID=" + dr["WorkID"] + "\" >" + dr["Title"].ToString() + " - " + dr["NodeName"].ToString() + "</a>");
                }

                this.Add(dr["Starter"].ToString() + " - " + dr["RDT"].ToString());
            }
            this.AddULEnd();
            this.AddFieldSetEnd();
        }
    }
}