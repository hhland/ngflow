using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Web;
using BP.En;
using BP.DA;
using BP.WF;
using BP.Sys;
using BP.Port;
using BP;
namespace CCFlow.WF.UC
{
    public partial class KeySearch : BP.Web.UC.UCBase3
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = " Process data query ";
            //TextBox tb = new TextBox();
            //tb.ID = "TB_Key";
            //Button btn = new Button();
            //btn.ID = "Btn_Search";
            //btn.Click += new EventHandler(btn_Click);
        }

        void btn_Click(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string text = this.TextBox1.Text.Trim();
            if (string.IsNullOrEmpty(text))
                return;
            Button btn = sender as Button;
            string sql = "";
            switch (btn.ID)
            {
                case "Btn_ByWorkID":
                    int workid = 0;
                    try
                    {
                        workid = int.Parse(text);
                    }
                    catch
                    {
                        this.Alert(" You entered is not a WorkID" + text);
                        return;
                    }
                    /*zqp 2014 12 5 Modification , Query is incorrect */
                    if (this.CheckBox1.Checked)
                        sql = "SELECT A.*,B.Name as FlowName FROM V_FlowData a,WF_Flow b  WHERE A.FK_Flow=B.No AND A.OID=" + workid + " AND FlowEmps LIKE '@%" + WebUser.No + "%'";
                    else
                        sql = "SELECT A.*,B.Name as FlowName FROM V_FlowData a,WF_Flow b  WHERE A.FK_Flow=B.No AND A.OID=" + workid;
                    break;
                case "Btn_ByTitle":
                    if (this.CheckBox1.Checked)
                        sql = "SELECT A.*,B.Name as FlowName FROM V_FlowData a,WF_Flow b  WHERE A.FK_Flow=B.No AND a.Title LIKE '%" + text + "%' AND FlowEmps LIKE '@%" + WebUser.No + "%'";
                    else
                        sql = "SELECT A.*,B.Name as FlowName FROM V_FlowData a,WF_Flow b  WHERE A.FK_Flow=B.No AND a.Title LIKE '%" + text + "%'";
                    break;
                default:
                    break;
            }

            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
            {
                this.Pub1.Clear();
                this.Pub1.AddH3("&nbsp;&nbsp; Not even find out anything , I really do not Incredibles .");
                return;
            }

            this.Pub1.Clear();
            this.Pub1.AddTable();
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("Idx");
            this.Pub1.AddTDTitle(" Process ");
            this.Pub1.AddTDTitle(" Title ");
            this.Pub1.AddTDTitle(" Sponsor ");
            this.Pub1.AddTDTitle(" Launch date ");
            this.Pub1.AddTDTitle(" Status ");
            this.Pub1.AddTDTitle(" Participants ");
            this.Pub1.AddTREnd();
            int idx = 1;
            foreach (DataRow dr in dt.Rows)
            {
                this.Pub1.AddTR();
                this.Pub1.AddTDIdx(idx++);
                this.Pub1.AddTD(dr["FlowName"].ToString());
                this.Pub1.AddTD("<A href=\"javascript:OpenIt('" + dr["FK_Flow"] + "','" + dr["FlowEndNode"] + "','" + dr["OID"] + "');\" >" + dr["Title"].ToString() + "</a>");
                this.Pub1.AddTD(dr["FlowStarter"].ToString());
                this.Pub1.AddTD(dr["FlowStartRDT"].ToString());
                switch (int.Parse(dr["WFState"].ToString()))
                {
                    case 0:
                        this.Pub1.AddTD(" Unfinished ");
                        break;
                    case 1:
                        this.Pub1.AddTD(" Completed ");
                        break;
                    default:
                        this.Pub1.AddTD(" Unknown ");
                        break;
                }
                this.Pub1.AddTDBigDoc(dr["FlowEmps"].ToString());
                this.Pub1.AddTREnd();
            }
            this.Pub1.AddTableEnd();
        }
    }
}