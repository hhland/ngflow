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

public partial class AppDemo_Login1 : BP.Web.WebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strNo = "";
        string strPs = "";
        string isRemember = "";

        if (this.Request.QueryString["DoType"] == "Logout")
        {
            BP.Web.WebUser.Exit();
        }

        if (IsPostBack == false)
        {

            if (Request.QueryString["username"] != null)// Determine whether the jump from other pages 
            {
                strNo = Request.QueryString["username"].ToString();

                if (Request.QueryString["password"] != null)
                {
                    strPs = Request.QueryString["password"].ToString();
                    Login(strNo, strPs);
                }
            }
            else
            {
                if (this.Request.Browser.Cookies == true) // Get cookie
                {
                    if (Request.Cookies["CCS"] != null)
                    {
                        strNo = Convert.ToString(Request.Cookies["CCS"].Values["No"]);
                        if (strNo != "")
                        {
                            strPs = Convert.ToString(Request.Cookies["CCS"].Values["Pass"]);
                            isRemember = Request.Cookies["CCS"].Values["IsRememberMe"].ToString();
                            // Get cookie The user name and password , And judges whether consistent 
                            Emp em = new Emp(strNo);
                            em.No = strNo;
                            if (em.RetrieveFromDBSources() == 0)
                                return;

                            if (em.CheckPass(strPs))
                            {
                                WebUser.SignInOfGenerLang(em, WebUser.SysLang, isRemember == "0" ? false : true);
                                if (isRemember == "1")
                                {

                                    if (this.Request.RawUrl.ToLower().Contains("wap"))
                                        WebUser.IsWap = true;
                                    else
                                        WebUser.IsWap = false;

                                    WebUser.Token = this.Session.SessionID;

                                    Response.Redirect("Default.aspx", false);
                                    return;
                                }
                                else
                                {
                                    this.TB_No.Text = strNo;
                                }
                            }
                            else
                            {
                                this.TB_No.Text = strNo;
                            }
                        }

                    }
                }


            }
            this.Page.RegisterStartupScript("event_handler", "<script>document.body.onkeypress = keyPressed;</script>");
            this.Page.RegisterClientScriptBlock("default_button", "<script> function keyPressed() { if(window.event.keyCode == 13) { document.getElementById(\""
                + this.Btn1.ClientID + "\").click(); } } </script>");
            if (WebUser.No != null)
            {
                TB_No.Text = WebUser.No;
            }

        }
    }
    public void Login(string strUser, string strPass)
    {
        TB_No.Text = strUser;
        TB_Pass.Text = strPass;
        Login();
    }
    private void Login()
    {
        string user = TB_No.Text.Trim();
        string pass = TB_Pass.Text.Trim();
        try
        {
            // Close logged-in users 
            if (WebUser.No != null) WebUser.Exit();

            Emp em = new Emp(user);
            if (em.CheckPass(pass))
            {
                bool bl = this.IsRemember.Checked;
                WebUser.SignInOfGenerLang(em, WebUser.SysLang, bl);

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
    public void Btn1_Click(object sender, System.EventArgs e)
    {
        Login();
    }
}