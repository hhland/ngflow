using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCFlow.SDKFlowDemo.TeleComDemo.ShengJu
{
    public partial class Do : System.Web.UI.Page
    {
        public string DoType
        {
            get
            {
                return this.Request.QueryString["DoType"];
            }
        }

        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
       
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (this.DoType)
            {
                case "DelSubFlow": /*  In the monitoring sub-process node , Delete sub-processes .*/
                    Int64 workid = Int64.Parse(this.Request.QueryString["WorkID"]);
                    string msg= BP.WF.Dev2Interface.Flow_DoDeleteFlowByReal(this.FK_Flow,workid, true);
                    BP.Sys.PubClass.Alert(msg);
                    break;
                default:
                    break;
            }
        }
    }
}