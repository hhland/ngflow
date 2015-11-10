using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;    
using System.Web.UI.WebControls;
using BP.GPM;
using BP.Web;

public partial class SSO_MasterPage : System.Web.UI.MasterPage
{
    public string usermsg = "当前为游客模式";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (WebUser.No != null)
        {
            usermsg = "帐号：" + BP.Web.WebUser.No + " 姓名： " + BP.Web.WebUser.Name + " 部门：" + BP.Web.WebUser.FK_DeptName;
            WebUser.SetSID(this.Session.SessionID);
        }
    }
}
