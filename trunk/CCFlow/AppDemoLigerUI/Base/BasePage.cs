using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace CCFlow.AppDemoLigerUI
{
    public class BasePage : Page
    {
        protected override void OnInit(EventArgs e)
        {
            if (BP.Web.WebUser.No == null)
            {
                this.Response.Redirect("Login.aspx", true);
            }
            base.OnInit(e);
        }
        /// <summary>
        ///  Need not page  Parameters ,show message
        /// </summary>
        /// <param name="mess"></param>
        protected void Alert(string mess, bool isClent)
        {
            if (string.IsNullOrEmpty(mess))
                return;

            mess = mess.Replace("@@", "@");
            mess = mess.Replace("@@", "@");

            mess = mess.Replace("'", "＇");

            mess = mess.Replace("\"", "＇");

            mess = mess.Replace("\"", "＂");

            mess = mess.Replace(";", ";");
            mess = mess.Replace(")", "）");
            mess = mess.Replace("(", "（");

            mess = mess.Replace(",", ",");
            mess = mess.Replace(":", ":");

            mess = mess.Replace("<", "［");
            mess = mess.Replace(">", "］");

            mess = mess.Replace("[", "［");
            mess = mess.Replace("]", "］");

            mess = mess.Replace("@", "\\n@");
            string script = "<script language=JavaScript>alert('" + mess + "');</script>";
            if (isClent)
                System.Web.HttpContext.Current.Response.Write(script);
            else
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "kesy", script);
        }
        protected void Alert(Exception ex)
        {
            this.Alert(ex.Message, false);
        }


        protected override void InitializeCulture()
        {
            base.InitializeCulture();
            object _culture = Session["culture"];
            string culture = _culture == null ? "en-us" : _culture.ToString();
            System.Threading.Thread.CurrentThread.CurrentCulture =
            System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(culture);
        }

        protected string GetGlobalResourceMenu(string key)
        {
            return GetGlobalResourceObject("Menu", key).ToString();
        }

        protected string GetGlobalResourceTitle(string key)
        {
            return GetGlobalResourceObject("Title", key).ToString();
        }

        protected string GetGlobalResourceButton(string key)
        {
            return GetGlobalResourceObject("Button", key).ToString();
        }

        protected string GetGlobalResourceLink(string key)
        {
            return GetGlobalResourceObject("Link", key).ToString();
        }

        protected string GetGlobalResourceLabel(string key)
        {
            return GetGlobalResourceObject("Label", key).ToString();
        }

        protected string GetGlobalResourceMsg(string key)
        {
            return GetGlobalResourceObject("Msg", key).ToString();
        }
    }
}