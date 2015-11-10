using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GMP2.GPM
{
    public partial class Default : System.Web.UI.Page
    {
        public string usermsg = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (BP.DA.DBAccess.IsExitsObject("GPM_APP") == false)
            {
                this.Response.Redirect("DBInstall.aspx", true);
                return;
            }
            // Login log records 
            BP.GPM.SystemLoginLog loginLog = new BP.GPM.SystemLoginLog();
            loginLog.FK_Emp = BP.Web.WebUser.No;
            loginLog.FK_App = "GPM";
            loginLog.RContent = " Login System ";
            loginLog.LoginDateTime = DateTime.Now.ToString();
            loginLog.IP = System.Web.HttpContext.Current.Request.UserHostAddress;
            loginLog.Insert();
            usermsg = " Account number :" + BP.Web.WebUser.No + "  Full name : " + BP.Web.WebUser.Name + "  Department :" + BP.Web.WebUser.FK_DeptName;
        }
    }
}