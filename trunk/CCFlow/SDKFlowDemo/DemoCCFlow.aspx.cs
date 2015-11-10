using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SDKFlowDemo_DemoCCFlow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string doType = this.Request.QueryString["DoType"];

        switch (doType)
        {
            case "TurnToStartNewFlow":
                break;
            default:
                break;
        }
    }
    public void DemoStartNewFlow()
    {
    }
}