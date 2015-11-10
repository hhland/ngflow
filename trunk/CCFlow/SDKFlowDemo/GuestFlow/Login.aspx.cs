using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCFlow.SDKFlowDemo.GuestFlow
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            //  Get Student Information .
            string stuNo = this.TB_No.Text.Trim();
            string pass = this.TB_Pass.Text.Trim();

            //  Should be based on student number , Check out the student name , Here is a direct definition of .
            string stuName = " Joe Smith ";

            //  Carried out Guest Landed 
            BP.WF.Dev2InterfaceGuest.Port_Login(stuNo, stuName);

            //  Transfer students to fill out the application form for leave .
            string url = "/WF/MyFlow.aspx?FK_Flow=055&FK_Node=05501&GuestNo=" + stuNo + "&GuestName=" + stuName;
            url += "SysSendEmps=yangyilei";

            // Note that there are two parameters URL Pass passed .
            this.Response.Redirect(url, true);
        }
    }
}