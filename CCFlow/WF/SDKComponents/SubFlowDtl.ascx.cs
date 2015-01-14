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
    public partial class SubFlowDtl : BP.Web.UC.UCBase3
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
            // Check out all sub-processes data .
            GenerWorkFlows gwfs = new GenerWorkFlows();
            gwfs.Retrieve(GenerWorkFlowAttr.PWorkID, this.WorkID);

            this.AddTable();
            this.AddTR();
            this.AddTDTitle("序");
            this.AddTDTitle(" Title ");
            this.AddTDTitle(" Stay node ");
            this.AddTDTitle(" Status ");
            this.AddTDTitle(" Processors ");
            this.AddTDTitle(" Processing time ");
            this.AddTDTitle(" Information ");
            //this.AddTDTitle(" Operating ");
            this.AddTREnd();

            int idx = 0;
            foreach (GenerWorkFlow item in gwfs)
            {
                idx++;
                this.AddTR();
                this.AddTDIdx(idx);
                this.AddTD("style='word-break:break-all;'", 
                    "<a href='"+Glo.CCFlowAppPath+"WF/WFRpt.aspx?WorkID="+item.WorkID+"&FK_Flow="+item.FK_Flow+"' target=_blank >"+item.Title+"</a>");

                this.AddTD(item.NodeName);

                if ( item.WFState== WFState.Complete)
                this.AddTD(" Completed ");
                else
                    this.AddTD(" Unfinished ");

                this.AddTD(item.TodoEmps);
                this.AddTD(item.RDT);
                this.AddTD(item.FlowNote);
                this.AddTREnd();
            }
            this.AddTableEnd();
        }
    }
}