using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;

namespace CCFlow.WF.App.Comm
{
    public partial class FrmCheck : System.Web.UI.UserControl
    {
        #region  Property 
        private bool isHidden = false;
        /// <summary>
        ///  Audit input box is hidden 
        /// </summary>
        public bool IsHidden
        {
            get
            {
                string _isHidden = this.Request.QueryString["IsHidden"];
                if (string.IsNullOrEmpty(_isHidden))
                    return false;
                else
                    return bool.Parse(_isHidden);
            }
        }
        /// <summary>
        ///  Node number 
        /// </summary>
        public int NodeID
        {
            get
            {
                try
                {
                    return int.Parse(this.Request.QueryString["FK_Node"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        ///  The work ID
        /// </summary>
        public Int64 WorkID
        {
            get
            {
                string workid = this.Request.QueryString["OID"];
                if (workid == null)
                    workid = this.Request.QueryString["WorkID"];
                return Int64.Parse(workid);
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
        /// <summary>
        ///  Perform operations .
        /// </summary>
        public string DoType
        {
            get
            {
                return this.Request.QueryString["DoType"];
            }
        }
        #endregion  Property 

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }

}