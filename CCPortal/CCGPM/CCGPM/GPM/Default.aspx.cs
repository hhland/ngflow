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
            if (BP.DA.DBAccess.IsExitsObject("Port_Emp") == false)
            {
                this.Response.Redirect("DBInstall.aspx", true);
                return;
            }
        
            if (BP.Web.WebUser.No == null)
            {
                this.Response.Redirect("/SSO/Loginin.aspx", true);
                return;
            }
            //记录登录日志
            BP.GPM.SystemLoginLog loginLog = new BP.GPM.SystemLoginLog();
            loginLog.FK_Emp = BP.Web.WebUser.No;
            loginLog.FK_App = "GPM";
            loginLog.RContent = "登录系统";
            loginLog.LoginDateTime = DateTime.Now.ToString();
            loginLog.IP = System.Web.HttpContext.Current.Request.UserHostAddress;
            loginLog.Insert();
            usermsg = "帐号：" + BP.Web.WebUser.No + " 姓名： " + BP.Web.WebUser.Name + " 部门：" + BP.Web.WebUser.FK_DeptName;
        }
    }
}