using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCFlow.WF
{
    public partial class _BasePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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