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
using BP.Web;
using BP.Port;

namespace CCFlow.SDKFlowDemo.TeleCom._3SheBei
{

    public partial class Site : System.Web.UI.MasterPage
    {
        #region  Variable process engine came .
        /// <summary>
        ///  The work ID, In establishing the draft has produced .
        /// </summary>
        public Int64 WorkID
        {
            get
            {
                try
                {
                    return Int64.Parse(this.Request.QueryString["WorkID"]);
                }
                catch
                {
                    return 0;
                }
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
                BP.WF.Dev2Interface.Flow_IsCanDoCurrentWork(FK_Flow,this.FK_Node, this.WorkID, BP.Web.WebUser.No);
            }
            catch (Exception ex)
            {
                this.Response.Write("<font color=red>" + ex.Message + "</font>");
            }

            try
            {
                BP.WF.Node nd = new BP.WF.Node(this.FK_Node);
                this.Page.Title = " Process :" + nd.FlowName + ", Node :" + nd.Name + ",WorkID:" + this.WorkID + ",FK_Node:" + this.FK_Node;
            }
            catch
            {
            }
        }
    }
}