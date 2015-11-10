using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Comm_WinOpen : System.Web.UI.MasterPage
{  
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.RegisterClientScriptBlock("s",
        "<link href='/Comm/Style/Table" + BP.Web.WebUser.Style + ".css' rel='stylesheet' type='text/css' />");
    }
}
