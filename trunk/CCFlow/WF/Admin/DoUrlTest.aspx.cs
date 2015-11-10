using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace CCFlow.WF.Admin
{
    public partial class WF_MapDef_DoUrlTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string doType = this.Request.QueryString["DoType"];
            if (string.IsNullOrEmpty(doType))
            {
                this.Response.Write("Error: Execution is empty ");
                return;
            }

            try
            {
                switch (doType)
                {
                    case "test":
                        break;
                    default:
                        this.Response.Write(" No judgment execution mark :" + doType);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.Response.Write("Error:" + ex.Message);
            }

        }
    }
}