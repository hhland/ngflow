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
using System.Security.Cryptography;
using System.Text;
using System.Globalization;

namespace CCFlow.AppDemoLigerUI
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string culture = Request.Params["culture"];
            if (!string.IsNullOrWhiteSpace(culture))
            {
                //HttpContext.Current.Profile.SetPropertyValue("culture", culture);
                Session["culture"] = culture;
                Response.Redirect(Request.Url.LocalPath);
            }

            if (this.Request.QueryString["DoType"] == "Logout")
                BP.Web.WebUser.Exit();

            if (this.Request.Browser.Cookies == false)
            {
                this.Response.Write(" Browser does not support cookies.");
                return;
            }

            string strNo = "";
            string strPs = "";
            string isRemember = "";
            if (IsPostBack == false)
            {
                if (this.Request.Browser.Cookies == true) // Get cookie
                {
                    if (Request.Cookies["CCS"] != null)
                    {
                        strNo = Convert.ToString(Request.Cookies["CCS"].Values["No"]);
                        if (strNo != "")
                        {
                            if (strNo == "Guest")
                                return;

                            strPs = Convert.ToString(Request.Cookies["CCS"].Values["Pass"]);
                            isRemember = Request.Cookies["CCS"].Values["IsRememberMe"].ToString();
                            // Get cookie The user name and password , And judges whether consistent 
                            Emp em = new Emp(strNo);
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

                                    Response.Redirect("/AppDemoLigerUI/Default.aspx", false);
                                    return;
                                }
                                else
                                {
                                    this.txtUserName.Text = strNo;
                                }
                            }
                            else
                            {
                                this.txtUserName.Text = strNo;
                            }
                        }

                    }
                }
            }

            this.Page.RegisterStartupScript("event_handler", "<script>document.body.onkeypress = keyPressed;</script>");
            this.Page.RegisterClientScriptBlock("default_button", "<script> function keyPressed() { if(window.event.keyCode == 13) { document.getElementById(\""
                + this.lbtnSubmit.ClientID + "\").click(); } } </script>");

            //if (WebUser.No != null)
            //    txtUserName.Text = WebUser.No;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string user = txtUserName.Text.Trim();
            string pass = txtPassword.Text.Trim();
            try
            {
                if (WebUser.No != null)
                    WebUser.Exit();

                BP.Port.Emp em = new BP.Port.Emp(user);
                if (em.CheckPass(Encrypt(pass)))
                {
                    bool bl = this.IsRemember.Checked;

                    WebUser.SignInOfGenerLang(em, WebUser.SysLang, bl);

                    if (this.Request.RawUrl.ToLower().Contains("wap"))
                        WebUser.IsWap = true;
                    else
                        WebUser.IsWap = false;

                    WebUser.Token = this.Session.SessionID;

                    string s = "";
                    s = BP.Web.WebUser.No;
                    if (string.IsNullOrEmpty(s))
                        s = BP.Web.WebUser.NoOfRel;
                    if (string.IsNullOrEmpty(s))
                        throw new Exception("@ Number is not written :" + s);

                    this.Response.Redirect("/AppDemoLigerUI/Default.aspx?ss=" + s + "&DDD=" + em.No, false);
                    return;
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "kesy", "<script language=JavaScript>alert(' Username Password error , Note that the password is case sensitive , Check to see if pressed CapsLock..');</script>");
                }
            }
            catch (System.Exception ex)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "kesy", "<script language=JavaScript>alert('@ Username Password error !@ Check if pressed CapsLock.@ More information :" + ex.Message + "');</script>");
            }
        }


        public string Encrypt(string Text)
        {
            string sKey = "zhangweilong";
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        protected override void InitializeCulture()
        {
            base.InitializeCulture();
            object _culture = Session["culture"];
            string culture = _culture == null ? "en-us" : _culture.ToString();
            System.Threading.Thread.CurrentThread.CurrentCulture =
            System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(culture);
        }
    }
}