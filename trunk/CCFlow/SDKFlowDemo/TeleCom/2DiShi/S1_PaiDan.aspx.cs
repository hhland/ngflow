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
    public partial class S1_PaiDan : System.Web.UI.Page
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
        /// <summary>
        ///  Send 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Send_Click(object sender, EventArgs e)
        {
            // Performing transmission ,  Send this , Ordinary nodes to send shunt point , And specify the recipient .
            SendReturnObjs sendObjs = BP.WF.Dev2Interface.Node_SendWork(this.FK_Flow, this.WorkID, null, null, 0, "guobaogeng");

            // Tips to send a message .
            this.Session["info"] = sendObjs.ToMsgOfHtml();
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