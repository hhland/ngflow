using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace CCFlow.WF
{
    public partial class WF_AllotTask : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Int64 workid = BP.WF.Dev2Interface.Node_CreateBlankWork("001", null, null, BP.Web.WebUser.No, null);
          //  this.Response.Redirect("/WF/MyFlow.aspx?FK_Flow=001", true);
        }


    }
}