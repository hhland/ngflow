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
    public partial class Link : BP.Web.UC.UCBase3
    {
        public void BindWap()
        {
            //Links ens = new Links();
            //ens.RetrieveAll();
            //this.AddFieldSet("<img src='/WF/Img/Home.gif' ><a href='Home.aspx' >Home</a>");
            //this.AddUL();
            //foreach (Link en in ens)
            //{
            //    this.AddLi(en.Url, "<b>" + en.Name + "</b>", en.Target);
            //    this.Add("<font color=green>" + en.Url + "</font><br>" + en.Note);

            //}
            //this.AddULEnd();
            //this.AddFieldSetEnd();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (WebUser.IsWap)
            {
                BindWap();
                return;
            }

            //Links ens = new Links();
            //ens.RetrieveAll();

            //this.DivInfoBlockBegin();

            //this.AddUL();
            //foreach (Link en in ens)
            //{
            //    this.AddLi(en.Url, "<b>" + en.Name + "</b>", en.Target);
            //    this.Add("<font color=green>" + en.Url + "</font><br>" + en.Note);
            //}
            //this.AddULEnd();
            //this.AddBR();
            //this.AddBR();
            //this.AddBR();
            //this.AddBR();
            //this.AddBR();
            //this.DivInfoBlockEnd();
        }
    }

}