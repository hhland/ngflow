using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCFlow.SDKFlowDemo.TeleCom._3SheBei
{
    public partial class ShowMsg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string info = this.Session["info"] as string;
            if (info != null)
                this.Label2.Text = info.Replace("@","<br>@");
        }
    }
}