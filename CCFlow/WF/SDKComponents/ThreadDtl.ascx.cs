using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.Web;
using BP.DA;
using BP.En;

namespace CCFlow.WF.SDKComponents
{
    public partial class ThreadDtl : BP.Web.UC.UCBase3
    {
        #region  Property .
        public int FID
        {
            get
            {
                return int.Parse(this.Request.QueryString["FID"]);
            }
        }
        public int WorkID
        {
            get
            {
                try
                {
                    return int.Parse(this.Request.QueryString["WorkID"]);
                }
                catch
                {
                    return int.Parse(this.Request.QueryString["OID"]);
                }
            }
        }
        /// <summary>
        ///  Node number 
        /// </summary>
        public int FK_Node
        {
            get
            {
                try
                {
                    return int.Parse(this.Request.QueryString["FK_Node"]);
                }
                catch
                {
                    return DBAccess.RunSQLReturnValInt("SELECT FK_Node FROM WF_GenerWorkFlow WHERE WorkID=" + this.WorkID);
                }
            }
        }
        /// <summary>
        ///  Process ID 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        #endregion  Property .

        protected void Page_Load(object sender, EventArgs e)
        {
            
            Node nd = new Node(this.FK_Node);
            Work wk = nd.HisWork;
            wk.OID = this.WorkID;
            wk.Retrieve();
            if (nd.HisNodeWorkType == NodeWorkType.WorkHL || nd.HisNodeWorkType == NodeWorkType.WorkFHL)
            {
            }
            else
            {
                this.AddFieldSetRed("err", " The current node (" + nd.Name + ") Non confluence , You can not view the child thread .");
                return;
            }
            GenerWorkerLists wls = new GenerWorkerLists();
            QueryObject qo = new QueryObject(wls);
            qo.AddWhere(GenerWorkerListAttr.FID, wk.OID);
            qo.addAnd();
            qo.AddWhere(GenerWorkerListAttr.IsEnable, 1);

            int i = qo.DoQuery();
            if (i == 1)
            {
                wls.Clear();
                qo.clear();
                qo.AddWhere(GenerWorkerListAttr.FID, wk.OID);
                qo.addAnd();
                qo.AddWhere(GenerWorkerListAttr.IsEnable, 1);
                qo.DoQuery();
            }

            // If no child process will not let it show 
            if (wls.Count > 0)
            {
                this.AddFieldSet(" Streaming information ");
                this.AddTable("border=0"); // ("<table border=0 >");
                this.AddTR();
                this.AddTDTitle("IDX");
                this.AddTDTitle(" Node ");
                this.AddTDTitle(" Processors ");
                this.AddTDTitle(" Name ");
                this.AddTDTitle(" Department ");
                this.AddTDTitle(" Status ");
                this.AddTDTitle(" Should be completed by the date ");
                this.AddTDTitle(" Actual Completion Date ");
                this.AddTDTitle("");
                this.AddTREnd();

                bool is1 = false;
                int idx = 0;
                foreach (GenerWorkerList wl in wls)
                {
                    idx++;
                    is1 = this.AddTR(is1);

                    this.AddTDIdx(idx);
                    this.AddTD(wl.FK_NodeText);
                    this.AddTD(wl.FK_Emp);

                    this.AddTD(wl.FK_EmpText);
                    this.AddTD(wl.FK_DeptT);

                    if (wl.IsPass)
                    {
                        this.AddTD(" Completed ");
                        this.AddTD(wl.SDT);
                        this.AddTD(wl.RDT);
                    }
                    else
                    {
                        this.AddTD("<font color=red> Unfinished </font>");
                        this.AddTD(wl.SDT);
                        this.AddTD();
                    }

                    if (wl.IsPass == false)
                    {
                        if (nd.ThreadKillRole == ThreadKillRole.ByHand)
                            this.AddTD("<a href=\"javascript:DoDelSubFlow('" + wl.FK_Flow + "','" + wl.WorkID + "')\"><img src='" + BP.WF.Glo.CCFlowAppPath + "WF/Img/Btn/Delete.gif' border=0/> Termination </a>");
                        else
                            this.AddTD();
                    }
                    else
                    {
                        this.AddTD("<a href=\"javascript:WinOpen('" + BP.WF.Glo.CCFlowAppPath + "WF/FHLFlow.aspx?WorkID=" + wl.WorkID + "&FID=" + wl.FID + "&FK_Flow=" + nd.FK_Flow + "&FK_Node=" + this.FK_Node + "')\"> Turn on </a>");
                    }
                    this.AddTREnd();
                }
                this.AddTableEnd();
                this.AddFieldSetEnd(); //.AddFieldSet(" Streaming information ");
            }
        }
    }
}