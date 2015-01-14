using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF.Template;
using BP.WF;
using BP.Port;
using BP.Sys;
using BP.Web.Controls;
using BP.DA;
using BP.En;
using BP.Web;

namespace CCFlow.WF.UC
{
    public partial class GetTask : BP.Web.UC.UCBase3
    {
        public Int64 WorkID
        {
            get
            {
                return Int64.Parse(this.Request.QueryString["WorkID"]);
            }
        }
        public int FK_Node
        {
            get
            {
                return int.Parse(this.Request.QueryString["FK_Node"]);
            }
        }
        public int ToNode
        {
            get
            {
                return int.Parse(this.Request.QueryString["ToNode"]);
            }
        }
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        public void BindWorkList()
        {
            string pageid = this.Request.RawUrl.ToLower();
            if (pageid.Contains("small"))
            {
                if (pageid.Contains("single"))
                    pageid = "SmallSingle";
                else
                    pageid = "Small";
            }
            else
            {
                pageid = "";
            }
            int colspan = 10;
            this.AddTable("width='90%' align=left");
            this.AddCaption("<a href='GetTask" + pageid + ".aspx'><img src='/WF/Img/Start.gif' > Retrieve processing </a>");
            this.AddTR();
            this.AddTDTitle("序");
            this.AddTDTitle(" Title ");
            this.AddTDTitle(" Sponsor ");
            this.AddTDTitle(" Start Time ");
            this.AddTDTitle(" Stay node ");
            this.AddTDTitle(" Current treatment of people ");
            this.AddTDTitle(" Arrival Time ");
            this.AddTDTitle(" Time should be completed ");
            this.AddTDTitle(" Operating ");
            this.AddTREnd();

            //  According to the promoters permission to judge , Whether this person has permission to operate .
            GetTasks jcs = new GetTasks(this.FK_Flow);
            string canDealNodes = "";
            int idx = 1;
            foreach (BP.WF.GetTask jc in jcs)
            {
                /*  Judge whether I can handle the current point data ? */
                if (jc.Can_I_Do_It() == false)
                    continue;

                canDealNodes += "''";
                DataTable dt = DBAccess.RunSQLReturnTable("SELECT * FROM WF_EmpWorks WHERE FK_Node IN (" + jc.CheckNodes + ") AND FK_Flow='" + this.FK_Flow + "' AND FK_Dept LIKE '" + BP.Web.WebUser.FK_Dept + "%'");
                if (dt.Rows.Count == 0)
                {
                    if (BP.Web.WebUser.FK_Dept.Length >= 4)
                        dt = DBAccess.RunSQLReturnTable("SELECT * FROM WF_EmpWorks WHERE FK_Node IN (" + jc.CheckNodes + ") AND FK_Flow='" + this.FK_Flow + "' AND FK_Dept LIKE '" + BP.Web.WebUser.FK_Dept.Substring(0, 2) + "%'");
                    else
                        dt = DBAccess.RunSQLReturnTable("SELECT * FROM WF_EmpWorks WHERE FK_Node IN (" + jc.CheckNodes + ") AND FK_Flow='" + this.FK_Flow + "' AND FK_Dept LIKE '" + BP.Web.WebUser.FK_Dept + "%'");
                }

                this.AddTR();
                this.Add("<TD  class=TRSum colspan=" + colspan + " align=left>" + jc.Name + " ;  =》 Redirect Audit node :" + jc.CheckNodes + "</TD>");
                this.AddTREnd();
                foreach (DataRow dr in dt.Rows)
                {
                    this.AddTR();
                    this.AddTDIdx(idx++);
                    this.AddTD(dr["Title"].ToString());
                    this.AddTD(dr["Starter"].ToString());
                    this.AddTD(dr["RDT"].ToString());
                    this.AddTD(dr["NodeName"].ToString());
                    this.AddTD(dr["FK_EmpText"].ToString());
                    this.AddTD(dr["ADT"].ToString());
                    this.AddTD(dr["SDT"].ToString());
                    this.AddTD("<a href=\"javascript:WinOpen('WFRpt.aspx?WorkID=" + dr["WorkID"] + "&FK_Flow=" + this.FK_Flow + "&FID=" + dr["FID"] + "')\"> Report </a> - <a href=\"javascript:Tackback('" + this.FK_Flow + "','" + dr["FK_Node"] + "','" + jc.NodeID + "','" + dr["WorkID"] + "')\"> Retrieve </a>");
                    this.AddTREnd();
                }
            }
            this.AddTableEnd();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageid = this.Request.RawUrl.ToLower();
            if (pageid.Contains("small"))
            {
                if (pageid.Contains("single"))
                    pageid = "SmallSingle";
                else
                    pageid = "Small";
            }
            else
            {
                pageid = "";
            }

            if (this.DoType == "Tackback")
            {
                /* */
                try
                {
                    string s = BP.WF.Dev2Interface.Node_Tackback(this.FK_Node, this.WorkID, this.ToNode);
                    //  s=s.Replace(
                    this.AddTable("width='90%' align=left");
                    this.AddCaption("<a href='GetTask" + pageid + ".aspx'><img src='/WF/Img/Start.gif' > Retrieve processing </a>-<a href=GetTask" + pageid + ".aspx?FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.FK_Node + "> Return </a>");
                    this.AddTR();
                    this.AddTDBegin();
                    this.AddMsgGreen(" Retrieve success ", "<h3> Work has been placed in your to-do </h3><hr><a href='MyFlow" + pageid + ".aspx?FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.ToNode + "&WorkID=" + this.WorkID + "&FID=0' > Click here to proceed </a>.<br><br>");
                    this.AddTDEnd();

                    this.AddTR();
                    this.AddTD(s);
                    this.AddTDEnd();
                    this.AddTableEnd();
                }
                catch (Exception ex)
                {
                    this.AddMsgOfWarning(" Error ", ex.Message);
                }
                return;
            }

            if (this.FK_Flow != null)
            {
                this.BindWorkList();
                return;
            }

            Flows fls = new Flows();
            BP.En.QueryObject qo = new QueryObject(fls);
            qo.addOrderBy(FlowAttr.FK_FlowSort);
            qo.DoQuery();

            int colspan = 5;
            this.AddTable("width='90%' align=left");
            this.AddCaption("<a href='GetTask" + pageid + ".aspx'><img src='/WF/Img/Start.gif' > Retrieve processing </a>");
            this.AddTR();
            this.AddTDTitle("序");
            this.AddTDTitle(" Process Category ");
            this.AddTDTitle(" Name ");
            this.AddTDTitle(" Flow chart ");
            this.AddTDTitle(" Description ");
            this.AddTREnd();

            int i = 0;
            bool is1 = false;
            string fk_sort = null;
            foreach (Flow fl in fls)
            {
                if (fl.FlowAppType == FlowAppType.DocFlow)
                    continue;

                i++;
                is1 = this.AddTR(is1);
                this.AddTDIdx(i);
                if (fl.FK_FlowSort == fk_sort)
                    this.AddTD();
                else
                    this.AddTDB(fl.FK_FlowSortText);

                fk_sort = fl.FK_FlowSort;

                this.AddTD("<a href='GetTask" + pageid + ".aspx?FK_Flow=" + fl.No + "&FK_Node=" + int.Parse(fl.No) + "01' >" + fl.Name + "</a>");

                this.AddTD("<a href=\"javascript:WinOpen('Chart.aspx?FK_Flow=" + fl.No + "&DoType=Chart','sd');\"  > Turn on </a>");
                this.AddTD(fl.Note);
                this.AddTREnd();
            }
            this.AddTableEnd();
        }
    }

}