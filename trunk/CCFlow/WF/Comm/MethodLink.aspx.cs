using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BP.DA;
using BP.Web;
using BP.En;

namespace CCFlow.WF.Comm
{
    public partial class Comm_MethodLink : BP.Web.WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Ucsys1.AddBR();
            this.Ucsys1.AddH3("&nbsp;&nbsp;&nbsp;&nbsp; Hello :"+BP.Web.WebUser.No+" , "+BP.Web.WebUser.Name +"  -  Function execution  ");

            this.Ucsys1.AddHR();
            this.Ucsys1.Add("<ul>");

            #region  bind it .
            ArrayList al = BP.En.ClassFactory.GetObjects("BP.En.Method");
            int i = 1;
            foreach (Method en in al)
            {
                if (en.IsCanDo == false
                    || en.IsVisable == false)
                    continue;

                this.Ucsys1.AddLi("<a href=\"javascript:ShowIt('" + en.ToString() + "');\"  >" + en.GetIcon(this.Request.ApplicationPath) + en.Title + "</a><br><font size=2 color=Green>" + en.Help + "</font><br><br>");
            }
            this.Ucsys1.Add("</ul>");
            #endregion
        }
    }
}
