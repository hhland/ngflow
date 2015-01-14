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

public partial class WF_Admin_WinOpen : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (BP.Web.WebUser.No != "admin")
            throw new Exception("@ Users must be made illegal admin To operate , Users are now logged :" + BP.Web.WebUser.No);
    }
}
