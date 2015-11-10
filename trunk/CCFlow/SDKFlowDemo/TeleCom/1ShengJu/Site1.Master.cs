using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCFlow.SDKFlowDemo.TelecomDemo.Parent
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                BP.WF.Node nd = new BP.WF.Node(this.FK_Node);
                this.Page.Title = " Process :" + nd.FlowName + ", Node :" + nd.Name + ",WorkID:" + this.WorkID + ",FK_Node:" + this.FK_Node;
            }
            catch
            {
            }
        }

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
    }
}