using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Web;
using BP.En;
using BP.DA;
using BP.WF;
using BP.Sys;
using BP.Port;
using BP;

public partial class App_control_Login123 : BP.Web.UC.UCBase3
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.RegisterStartupScript("event_handler", "<script>document.body.onkeypress = keyPressed;</script>");
        this.Page.RegisterClientScriptBlock("default_button", "<script> function keyPressed() { if(window.event.keyCode == 13) { document.getElementById(\""
            + this.lbtnSubmit.ClientID + "\").click(); } } </script>");
        if (WebUser.No != null)
        {
            txtUserName.Text = WebUser.No;
        }
    }
    protected void lbtnSubmit_Click(object sender, EventArgs e)
    {
       
    }
    public void Login(string strUser, string strPass)
    {
        txtUserName.Text = strUser;
        txtPassword.Text= strPass;
        Login();
    }
    private void Login()
    {
        string user = txtUserName.Text.Trim();
        string pass = txtPassword.Text.Trim();
        try
        {
            // Close logged-in users 
            if (WebUser.No != null) WebUser.Exit();

            Emp em = new Emp(user);
            if (em.CheckPass(pass))
            {
                WebUser.SignInOfGenerLang(em, WebUser.SysLang);

                if (this.Request.RawUrl.ToLower().Contains("wap"))
                    WebUser.IsWap = true;
                else
                    WebUser.IsWap = false;

                WebUser.Token = this.Session.SessionID;

                Response.Redirect("Default.aspx", false);
                return;
            }
            this.Alert(" Username Password error , Note that the password is case sensitive , Check to see if pressed CapsLock..");
        }
        catch (System.Exception ex)
        {
            this.Alert("@ Username Password error !@ Check if pressed CapsLock.@ More information :" + ex.Message);
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Login();
    }
}