using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SSO_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (BP.DA.DBAccess.IsExitsObject("GPM_Bar") == false)
            {
                this.Response.Redirect("Loginin.aspx", true);
                return;
            }

            if (BP.Web.WebUser.No == null && BP.DA.DBAccess.IsExitsObject("GPM_Bar"))
            {
                this.Response.Redirect("Loginin.aspx", true);
            }
        }
        catch
        {
            this.Response.Redirect("Loginin.aspx", true);
        }
        if (BP.Web.WebUser.No != null)
            this.Title = "您好:" + BP.Web.WebUser.No + "," + BP.Web.WebUser.Name;
    }
}