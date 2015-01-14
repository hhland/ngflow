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
using BP.WF;
using BP.En;
using BP.Port;
using BP.Web.Controls;
using BP.Web;
using BP.Sys;

namespace CCFlow.WF.Admin
{
    public partial class WF_Admin_DoType : BP.Web.WebPage
    {
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            switch (this.DoType)
            {
                case "FlowCheck":
                    BP.WF.Flow fl = new BP.WF.Flow(this.RefNo);
                    this.Ucsys1.AddFieldSet(fl.Name+" Check the information flow ");

                    this.Title = fl.Name+" Process inspection ";

                    string info = fl.DoCheck().Replace("@", "<BR>@");
                    info = info.Replace("@ Error ", "<font color=red><b>@ Error </b></font>");
                    info = info.Replace("@ Caveat ", "<font color=yellow><b>@ Caveat </b></font>");
                    info = info.Replace("@ Information ", "<font color=black><b>@ Information </b></font>");

                    this.Ucsys1.Add(info); //   Check the information flow 
                    this.Ucsys1.AddFieldSetEnd();
                    break;
                default:
                    this.Ucsys1.AddMsgOfInfo(" Error flag ", this.DoType);
                    break;
            }
        }
    }
}