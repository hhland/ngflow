using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCFlow.SDKFlowDemo.SDK
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack == false)
            {
                string userNo = this.Request.QueryString["UserNo"];
                if (string.IsNullOrEmpty(userNo))
                    this.TB_No.Text = userNo;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            #region  This section may be omitted .
            BP.WF.Port.Emp emp = new BP.WF.Port.Emp();
            emp.No = this.TB_No.Text;
            if (emp.IsExits == false)
            {
                this.ToMsg(" Username ("+emp.No+") Does not exist ....");
                return;
            }
            #endregion  This section may be omitted .

            // Take to the user ID .
            string userNo = this.TB_No.Text;
            //  Call the login interface , Write login .
            BP.WF.Dev2Interface.Port_Login(userNo);

            // Go Upcoming .
            this.Response.Redirect("ToDoList.aspx", true);
        }

        #region  Public Methods 
        public void ToMsg(string msg)
        {
            System.Web.HttpContext.Current.Session["info"] = msg;
            System.Web.HttpContext.Current.Application["info" + BP.Web.WebUser.No] = msg;
            BP.WF.Glo.SessionMsg = msg;
            System.Web.HttpContext.Current.Response.Redirect(
                "/SDKFlowDemo/SDK/Info.aspx?FK_Flow=2&FK_Type=2&FK_Node=2&WorkID=22" + DateTime.Now.ToString(), false);
        }
        public void ToErrorPage(string msg)
        {
            System.Web.HttpContext.Current.Session["info"] = msg;
            System.Web.HttpContext.Current.Application["info" + BP.Web.WebUser.No] = msg;
            BP.WF.Glo.SessionMsg = msg;
            System.Web.HttpContext.Current.Response.Redirect("/SDKFlowDemo/SDK/ErrorPage.aspx", false);
        }
        #endregion  Public Methods 
    }
}