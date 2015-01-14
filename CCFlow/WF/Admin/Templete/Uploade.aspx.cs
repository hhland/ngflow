using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.DA;

namespace CCFlow.WF.Admin.Templete
{
    public partial class Uploade : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sql = "SELECT No,Name,ParentNo FROM WF_FlowSort";
            string sql1 = "SELECT No,Name, FK_Sort FROM WF_Flow";
        }
    }
}