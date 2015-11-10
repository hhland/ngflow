using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.GPM;

namespace GMP2.GPM
{
    public partial class AppMenu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // If you do not BPM Data , His menu is initialized .
            BP.GPM.App app = new App();
            app.No = "CCFlowBPM";
            if (app.RetrieveFromDBSources() == 0)
            {
                BP.GPM.App.InitBPMMenu();
                app.Retrieve();
            }
        }
    }
}