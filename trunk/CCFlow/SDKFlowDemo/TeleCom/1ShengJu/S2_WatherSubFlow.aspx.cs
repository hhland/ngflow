using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.En;
using BP.DA;
using BP.Demo;
using BP.Web;
using BP.Port;


namespace CCFlow.SDKFlowDemo.TeleComDemo.ShengJu
{
    public partial class S2_WatherSubFlow : System.Web.UI.Page
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
            try
            {
                BP.WF.Dev2Interface.Flow_IsCanDoCurrentWork(FK_Flow, this.FK_Node, this.WorkID, BP.Web.WebUser.No);
            }
            catch (Exception ex)
            {
                this.Response.Write(ex.Message);
            }
        }
        /// <summary>
        ///  Send 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Send_Click(object sender, EventArgs e)
        {
            string info = "";
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

            string msg = BP.WF.Dev2Interface.Node_SendWork(this.FK_Flow, this.WorkID).ToMsgOfHtml();
            this.Response.Write("<font color=red>" + msg + "</font>");
        }
    }
}