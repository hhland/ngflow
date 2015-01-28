using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BP.Web;
using BP.En;
using BP.DA;
using BP.WF;
using BP.Sys;
using BP.Port;
using BP;

namespace CCFlow.WF.UC
{
    public partial class Login : BP.Web.UC.UCBase3
    {
        public string Lang
        {
            get
            {
                string s = this.Request.QueryString["Lang"];
                if (s == null)
                    return WebUser.SysLang;
                return s;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string userNo = this.Request.QueryString["UserNo"];
            if (userNo != null && userNo.Length > 1)
            {
                string sid = this.Request.QueryString["SID"];
                if (WebUser.CheckSID(userNo,sid) == true)
                {
                    Emp emp = new Emp(userNo);
                    BP.Web.WebUser.SignInOfGener(emp);
                    BP.Web.WebUser.SID = sid;
                    Response.Redirect(this.ToWhere, false);
                    return;
                }
            }

            if (this.Request.Browser.Cookies == false)
            {
                //this.Alert(" Your browser does not support cookies Function , Unable to use the system .");
                //return;
            }

            // if (this.DoType == "Logout")
            if (this.DoType != null)
            {
                BP.Web.WebUser.Exit();
                //this.Response.Redirect(this.PageID + ".aspx?DoType=del", true);
                //return;
            }

            WebUser.SysLang = this.Lang;
            Response.AddHeader("P3P", "CP=CAO PSA OUR");
            int colspan = 1;

            this.AddTable("border=0");

            this.AddTR();
            this.Add("<TD class=C align=left colspan=" + colspan + "><img src='/WF/Img/Login.gif' > <b> System Login </b></TD>");
            this.AddTREnd();
            this.AddTR();
            this.Add("<TD align=center >");

            this.AddTable("border=0px align=center ");
            this.AddTR();
            this.AddTD(  " Username :");

            TextBox tb = new TextBox();
            tb.ID = "TB_User";
            tb.Text = BP.Web.WebUser.No;
            tb.Columns = 20;

            this.AddTD(tb);
            this.AddTREnd();

            this.AddTR();
            this.AddTD( "Password:" );
            tb = new TextBox();
            tb.ID = "TB_Pass";
            tb.TextMode = TextBoxMode.Password;
            tb.Columns = 22;
            this.AddTD(tb);
            this.AddTREnd();

            this.AddTRSum();
            this.AddTDBegin("colspan=3 align=center");
            Button btn = new Button();
            btn.CssClass = "Btn";
            btn.Text = "Login";

            btn.Click += new EventHandler(btn_Click);
            this.Add(btn);
            if (WebUser.No != null)
            {
                string home = "";
                if (WebUser.IsWap)
                    home = "-<a href='Home.aspx'>Home</a>";

                if (WebUser.IsAuthorize)
                    this.Add(" - <a href=\"javascript:ExitAuth('" + WebUser.Auth + "')\" > Exit licensing model [" + WebUser.Auth + "]</a>" + home);
                else
                    this.Add(" - <a href='Tools.aspx?RefNo=AutoLog' > License landing </a>" + home);

                this.Add(" - <a href='" + this.PageID + ".aspx?DoType=Logout' ><font color=green><b> Exit </b></a>");
            }
            this.AddTDEnd();
            this.AddTREnd();
            this.AddTableEnd();

            this.AddBR();
            this.AddBR();

            this.AddTDEnd();
            this.AddTREnd();
            this.AddTableEnd();
        }
        public string ToWhere
        {
            get
            {
                if (this.Request.QueryString["ToWhere"] == null)
                {
                    if (this.Request.RawUrl.ToLower().Contains("small"))
                        return "EmpWorks.aspx";
                    else
                        return "EmpWorks.aspx";
                }
                else
                {
                    return this.Request.QueryString["ToWhere"];
                }
            }
        }
        void btn_Click(object sender, EventArgs e)
        {
            string user = this.GetTextBoxByID("TB_User").Text.Trim();
            string pass = this.GetTextBoxByID("TB_Pass").Text;
            try
            {

                Emp em = new Emp();
                em.No = user;
                if (em.RetrieveFromDBSources() == 0)
                {
                    this.Alert(" User name or password is incorrect , Note that both are case sensitive , Check to see if pressed CapsLock.");
                    return;
                }
                if (em.CheckPass(pass))
                {
                    //  Execution landing .
                    WebUser.SignInOfGenerLang(em, this.Lang);
                    if (this.Request.RawUrl.ToLower().Contains("wap"))
                        WebUser.IsWap = true;
                    else
                        WebUser.IsWap = false;

                    WebUser.Token = this.Session.SessionID;
                    if (WebUser.IsWap)
                    {
                        Response.Redirect("Home.aspx", true);
                        return;
                    }
                    Response.Redirect(this.ToWhere, false);
                    return;
                }
                this.Alert(" User name or password is incorrect , Note that both are case sensitive , Check to see if pressed CapsLock.");
            }
            catch (System.Exception ex)
            {
                this.Response.Write("<font color=red ><b>@ Username Password error !@ Check if pressed CapsLock.@ More information :" + ex.Message + "</b></font>");
            }
        }
    }
}