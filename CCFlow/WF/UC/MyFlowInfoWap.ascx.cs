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
    public partial class MyFlowInfoWap : BP.Web.UC.UCBase3
    {
        public string FK_Flow
        {
            get
            {
                string s = this.Request.QueryString["FK_Flow"];
                if (s == null)
                    s = "012";
                return s;
            }
        }
        /// <summary>
        ///  Current work ID
        /// </summary>
        public Int64 WorkID
        {
            get
            {
                if (ViewState["WorkID"] == null)
                {
                    if (this.Request.QueryString["WorkID"] == null)
                        return 0;
                    else
                        return Int64.Parse(this.Request.QueryString["WorkID"]);
                }
                else
                    return Int64.Parse(ViewState["WorkID"].ToString());
            }
            set
            {
                ViewState["WorkID"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (this.DoType)
            {
                case "DeleteFlow":
                    string fk_flow = this.Request.QueryString["FK_Flow"];
                    Int64 workid = Int64.Parse(this.Request.QueryString["WorkID"]);
                    try
                    {
                        WorkFlow wf = new WorkFlow(new Flow(fk_flow), workid);
                        wf.DoDeleteWorkFlowByReal(true);
                    }
                    catch
                    {

                    }
                    this.Session["info"] = " Process deleted successfully ";
                    break;
                case "UnShift":
                    try
                    {
                        WorkFlow mwf = new WorkFlow(this.FK_Flow, this.WorkID);
                        string str = mwf.DoUnShift();
                        this.Session["info"] = str;
                    }
                    catch (Exception ex)
                    {
                        this.Session["info"] = "@ Undo failure .@ Failure information " + ex.Message;
                    }
                    break;
                case "UnSend":
                    try
                    {
                        string str = BP.WF.Dev2Interface.Flow_DoUnSend(this.FK_Flow, this.WorkID);
                        this.Session["info"] = str;
                    }
                    catch (Exception ex)
                    {
                        this.Session["info"] = "@ Undo failure .@ Failure information " + ex.Message;
                    }
                    break;
                default:
                    break;
            }

            string s = this.Session["info"] as string;
            this.Session["info"] = null;
            if (s == null)
                s = this.Application["info" + WebUser.No] as string;

            if (s == null)
                s = BP.WF.Glo.SessionMsg;

            if (s != null)
            {
                if (this.PageID.Contains("Small"))
                {
                    this.Add("<br/>");
                    this.Add("<br/>");
                }

                s = s.Replace("@@", "@");
                s = s.Replace("@", "</span><br/><br/><img src='Img/email_start.png' align='middle' /><span class='info'>");
                s ="<span>"+s+"</span>";
                this.Add("<div style='width:500px;text-align:left;font-size:14px'>");
                if (WebUser.IsWap)
                    this.AddFieldSet("<a href=Home.aspx ><img src='" + BP.WF.Glo.CCFlowAppPath + "/WF/Img/Home.gif' border=0/>Home</a> -  Operation Tips ", s);
                else
                    this.AddFieldSet(" Operation Tips ", s);

                this.Add("<br><br></div>");
                return;
            }

            //  string sql = "SELECT * FROM WF_EmpWorks WHERE FK_Emp='" + BP.Web.WebUser.No + "'  AND FK_Flow='" + this.FK_Flow + "' ORDER BY WorkID ";
            DataTable dt = BP.WF.Dev2Interface.DB_GenerEmpWorksOfDataTable();// BP.DA.DBAccess.RunSQLReturnTable(sql);

            int colspan = 9;
            this.AddTable(" style='border=1px;align=center;width=80%;height:450px;");
            this.AddCaption(" Upcoming work ");
            this.AddTR();
            this.AddTDTitle("ID");
            this.AddTDTitle(" Process ");
            this.AddTDTitle(" Node ");
            this.AddTDTitle(" Title ");
            this.AddTDTitle(" Launch ");
            this.AddTDTitle(" Launch date ");
            this.AddTDTitle(" Accepted ");
            this.AddTDTitle(" The term ");
            this.AddTDTitle(" Status ");
            this.AddTREnd();

            int i = 0;
            bool is1 = false;
            DateTime cdt = DateTime.Now;
            foreach (DataRow dr in dt.Rows)
            {
                string sdt = dr["SDT"] as string;
                is1 = this.AddTR(is1); // ("onmouseover='TROver(this)' onmouseout='TROut(this)' onclick=\"\" ");
                i++;
                this.AddTDIdx(i);
                this.AddTD(dr["FlowName"].ToString());
                this.AddTD(dr["NodeName"].ToString());
                this.AddTD("<a href=\"" + BP.WF.Glo.CCFlowAppPath + "/MyFlow.aspx?FK_Flow=" + dr["FK_Flow"] + "&FID=" + dr["FID"] + "&WorkID=" + dr["WorkID"] + "\" >" + dr["Title"].ToString());
                this.AddTD(dr["Starter"].ToString());
                this.AddTD(dr["RDT"].ToString());
                this.AddTD(dr["ADT"].ToString());
                this.AddTD(dr["SDT"].ToString());
                DateTime mysdt = DataType.ParseSysDate2DateTime(sdt);
                if (cdt >= mysdt)
                {
                    this.AddTD("<font color=red> Overdue </font>");
                }
                else
                {
                    this.AddTD(" Normal ");
                }
                this.AddTREnd();
            }
            this.AddTRSum();
            this.AddTD("colspan=" + colspan, "&nbsp;");
            this.AddTREnd();
            this.AddTableEnd();


        }
    }

}