using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.En;
using BP.DA;
using BP.Demo;
using System.Collections;

namespace CCFlow.SDKFlowDemo.TeleCom._2DiShi
{
    public partial class S2_WangLuoFenXi : System.Web.UI.Page
    {
        #region  Variable process engine came .
        /// <summary>
        ///  The work ID, In establishing the draft has produced .
        /// </summary>
        public Int64 WorkID
        {
            get
            {
                return Int64.Parse(this.Request.QueryString["WorkID"]);
            }
        }
        /// <summary>
        ///  Process ID
        /// </summary>
        public Int64 FID
        {
            get
            {
                return Int64.Parse(this.Request.QueryString["FID"]);
            }
        }
        /// <summary>
        ///   Process ID .
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        /// <summary>
        ///  The current node ID
        /// </summary>
        public int FK_Node
        {
            get
            {
                return int.Parse(this.Request.QueryString["FK_Node"]);
            }
        }
        #endregion  Variable process engine came 

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void Btn_Send_Click(object sender, EventArgs e)
        {
            string info = "";
            /*step1 :  To check before sending device does not start .*/
            string sql = "SELECT COUNT(*) FROM tab_wf_commonkpioptivalue WHERE ParentWorkID="+this.WorkID+" AND WORKID=0";
            int numUnSendSubFlow=DBAccess.RunSQLReturnValInt(sql);
            if (numUnSendSubFlow > 0)
            {
                info += "<br>有" + numUnSendSubFlow+" The device does not originate sub-processes , So you can not perform this procedure sent to the Council .";
            }

            /*step2 :  Check that the sub-sub-processes have been completed , If not completed , On an error .*/
            GenerWorkFlows gwfs = BP.WF.Dev2Interface.DB_SubFlows(this.WorkID);
            if (gwfs.Count > 0)
            {
                info += "<br> The following sub-process is not completed , You can not submit .";
                foreach (GenerWorkFlow item in gwfs)
                    info += "<br>" + item.Title;
            }
            if (string.IsNullOrEmpty(info) == false)
            {
                this.Response.Write(info);
                return;
            }

            /*step3 :  Execution is sent down .*/
            string msg= BP.WF.Dev2Interface.Node_SendWork(this.FK_Flow, this.WorkID).ToMsgOfHtml();

            // Tips to send a message .
            this.Session["info"] = msg;
            this.Response.Redirect("ShowMsg.aspx?ss" + BP.DA.DataType.CurrentDataTime, true);


        }
        protected void Btn_Save_Click(object sender, EventArgs e)
        {

        }
        protected void Btn_Track_Click(object sender, EventArgs e)
        {
            BP.WF.Dev2Interface.UI_Window_FlowChartTruck(this.FK_Flow, this.FK_Node, this.WorkID, this.FID);
        }
    }
}