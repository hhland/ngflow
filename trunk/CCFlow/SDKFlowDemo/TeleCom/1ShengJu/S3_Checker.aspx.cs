using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.DA;
using BP.Demo;

namespace CCFlow.SDKFlowDemo.TeleComDemo.ShengJu
{
    public partial class S3_Checker : System.Web.UI.Page
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
            string msg = BP.WF.Dev2Interface.Node_SendWork(this.FK_Flow, this.WorkID).ToMsgOfHtml();

            //  Here one should turn to interface to display information .
            this.Session["info"] = msg;
            this.Response.Redirect("ShowMsg.aspx?ss=" + DataType.CurrentDataTime, true);
        }
    }
}