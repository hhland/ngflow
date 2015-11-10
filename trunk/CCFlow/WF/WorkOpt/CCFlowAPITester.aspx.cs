using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCFlow.WF.WorkOpt
{
    public partial class CCFlowAPITester : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string userNo = "";
            string sid = "";
            string flowNo = "";
            int nodeID = 10;
            Int64 workid = 0;
            Int64 fid = 0;

            CCFlowAPI api = new CCFlowAPI();
            api.Node_SendWork_Json("", 101, 1000, 0, "zhoupeng", "sdssss", "xxxxx");
        }
    }
}