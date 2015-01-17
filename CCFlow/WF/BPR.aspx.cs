using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.Port;
using BP.Sys;
using BP.DA;
using BP.En;
using BP.Web;

namespace CCFlow.WF
{
    public partial class WF_BPR : BP.Web.WebPage
    {
        #region  Property 
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        public string FK_Node
        {
            get
            {
                return this.Request.QueryString["FK_Node"];
            }
        }
        public string FK_NY
        {
            get
            {
                return this.Request.QueryString["FK_NY"];
            }
        }
        public string FK_Emp
        {
            get
            {
                return this.Request.QueryString["FK_Emp"];
            }
        }
        #endregion  Property 

        public string GetDoType
        {
            get
            {
                if (this.DoType != null)
                    return this.DoType;

                if (this.FK_NY == null && this.FK_Emp == null && this.FK_Node == null)
                    return "1.ShowFlowNodes";

                if (this.FK_Node != null)
                {
                    return "1.1ShowFlowNodeEmp";
                }

                if (this.FK_Node != null && this.FK_Emp != null)
                {
                    return "1.1.1ShowNodeEmp";
                }

                return "1.ShowFlowNodes";
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            Flow fl = new Flow(this.FK_Flow);
            this.Title = " Process cost analysis -" + fl.Name;
            switch (this.GetDoType)
            {
                case "NodeEmp":
                    this.BindNodeEmp(fl);
                    break;
                case "1.1ShowFlowNodeEmp":
                    this.Bind1_1_ShowFlowNodeEmp(fl);
                    break;
                case "1.ShowFlowNodes":
                default:
                    this.Bind1_ShowFlowNodes(fl);
                    break;
            }
        }
        public void Bind1_1_ShowFlowNodeEmp(Flow fl)
        {
            Node nd = new Node(int.Parse(this.FK_Node));

            this.Pub1.AddTable();
            this.Pub1.AddCaptionLeft("<a href='BPR.aspx?FK_Flow=" + this.FK_Flow + "'> Return </a> - " + fl.Name + "-" + nd.Name);
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("Idx");
            this.Pub1.AddTDTitle(" The operator ");
            this.Pub1.AddTDTitle(" The number of executions ");
            this.Pub1.AddTREnd();

            string sql = "SELECT DISTINCT Rec FROM ND" + nd.NodeID;
            BP.WF.Port.WFEmps emps = new BP.WF.Port.WFEmps();
            emps.RetrieveInSQL(sql);

            int idx = 0;
            bool is1 = false;
            float val = 0;
            foreach (BP.WF.Port.WFEmp emp in emps)
            {
                idx++;
                is1 = this.Pub1.AddTR(is1);
                this.Pub1.AddTDIdx(idx);
                this.Pub1.AddTD("<a href=\"javascript:WinOpen('BPR.aspx?FK_Flow=" + this.FK_Flow + "&FK_Emp=" + emp.No + "&FK_Node=" + this.FK_Node + "')\">" + emp.Name);
                val = DBAccess.RunSQLReturnValFloat("SELECT COUNT(*) FROM ND" + nd.NodeID + " WHERE Rec='" + emp.No + "'");
                this.Pub1.AddTDNum("<a href='BPR.aspx?FK_Flow=" + this.FK_Flow + "&FK_Emp=" + emp.No + "&FK_Node=" + this.FK_Node + "'>" + val + "</a>");
                this.Pub1.AddTREnd();
            }
            this.Pub1.AddTableEnd();
        }
        public void Bind1_ShowFlowNodes(Flow fl)
        {
            this.Pub1.AddTable();
            this.Pub1.AddCaptionLeft(fl.Name);
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("Idx");
            this.Pub1.AddTDTitle(" Step ");
            this.Pub1.AddTDTitle(" Node ");
            this.Pub1.AddTDTitle(" Hours of Work ");
            this.Pub1.AddTDTitle(" The average time (days)");
            this.Pub1.AddTDTitle(" Number of staff ");


            this.Pub1.AddTDTitle(" Warning period (0 Without warning )");
            this.Pub1.AddTDTitle(" Deadline (days)");


            this.Pub1.AddTDTitle(" Overdue trips ");
            this.Pub1.AddTDTitle(" Overdue Rate ");


            this.Pub1.AddTDTitle(" Post ");
            this.Pub1.AddTREnd();
            Nodes nds = fl.HisNodes;
            int idx = 0;
            bool is1 = false;
            float val = 0;
            foreach (BP.WF.Node nd in nds)
            {
                idx++;
                is1 = this.Pub1.AddTR(is1);
                this.Pub1.AddTDIdx(idx);
                this.Pub1.AddTD("第:" + nd.Step + "步");
                this.Pub1.AddTD(nd.Name);

                val = DBAccess.RunSQLReturnValFloat("SELECT COUNT(*) FROM ND" + nd.NodeID + " ");
                this.Pub1.AddTD(val);

                switch (BP.Sys.SystemConfig.AppCenterDBType)
                {
                    case DBType.Oracle:
                        val = DBAccess.RunSQLReturnValFloat("SELECT NVL(AVG(to_date(substr(RDT,1,10),'yyyy-mm-dd')- to_date(substr(CDT,1,10),'yyyy-mm-dd')),0) FROM ND" + nd.NodeID);
                        break;
                    default:
                        val = DBAccess.RunSQLReturnValFloat("SELECT IsNULL( Avg(dbo.GetSpdays(RDT,CDT)),0) FROM ND" + nd.NodeID + " ");
                        break;
                }

                this.Pub1.AddTD(val);

                val = DBAccess.RunSQLReturnValFloat("SELECT COUNT(DISTINCT Rec) FROM ND" + nd.NodeID + " ");
                this.Pub1.AddTDNum("<a href=\"javascript:WinOpen('BPR.aspx?FK_Flow=" + this.FK_Flow + "&FK_Node=" + nd.NodeID + "&DoType=NodeEmp')\" >" + val + "</a>");

                this.Pub1.AddTD(nd.WarningDays);
                this.Pub1.AddTD(nd.DeductDays);

                this.Pub1.AddTD(0);
                this.Pub1.AddTD(0);

                this.Pub1.AddTDDoc(nd.HisStationsStr, nd.HisStationsStr);
                this.Pub1.AddTREnd();
            }
            this.Pub1.AddTableEnd();
        }

        public void BindNodeEmp(Flow fl)
        {
            Node nd = new Node(int.Parse(this.FK_Node));
            this.Pub1.AddTable();
            this.Pub1.AddCaptionLeft("<a href='BPR.aspx?FK_Flow=" + this.FK_Flow + "'> Return </a> - " + fl.Name + " - " + nd.Name);
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("Idx");
            this.Pub1.AddTDTitle(" Step ");
            this.Pub1.AddTDTitle(" Node ");
            this.Pub1.AddTDTitle(" Hours of Work ");
            this.Pub1.AddTDTitle(" The average time (days)");
            this.Pub1.AddTDTitle(" Number of staff ");
            this.Pub1.AddTDTitle(" Post ");
            this.Pub1.AddTREnd();
            Nodes nds = fl.HisNodes;
            int idx = 0;
            bool is1 = false;
            float val = 0;
            //foreach (BP.WF.Node nd in nds)
            //{
            //    idx++;
            //    is1 = this.Pub1.AddTR(is1);
            //    this.Pub1.AddTDIdx(idx);
            //    this.Pub1.AddTD("第:" + nd.Step + "步");
            //    this.Pub1.AddTD(nd.Name);

            //    val = DBAccess.RunSQLReturnValFloat("SELECT COUNT(*) FROM ND" + nd.NodeID + " ");
            //    this.Pub1.AddTD(val);

            //    val = DBAccess.RunSQLReturnValFloat("SELECT IsNULL( Avg(dbo.GetSpdays(RDT,CDT)),0) FROM ND" + nd.NodeID + " ");
            //    this.Pub1.AddTD(val);

            //    val = DBAccess.RunSQLReturnValFloat("SELECT COUNT(DISTINCT Rec) FROM ND" + nd.NodeID + " ");
            //    this.Pub1.AddTDNum("<a href=\"javascript:WinOpen('BPR.aspx?FK_Flow=" + this.FK_Flow + "&FK_Node=" + nd.NodeID + "&DoType=NodeEmp')\" >" + val + "</a>");

            //    this.Pub1.AddTDDoc(nd.HisStationsStr, nd.HisStationsStr);
            //    this.Pub1.AddTREnd();
            //}
            this.Pub1.AddTableEnd();
        }
    }
}