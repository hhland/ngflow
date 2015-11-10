using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.Web;
using BP.En;
using BP.WF.XML;

namespace CCFlow.WF.Rpt
{
    public partial class WF_Rpt_MasterPage : BP.Web.MasterPage
    {
        #region  Property .
        public string FK_MapRpt
        {
            get
            {
                string s = this.Request.QueryString["FK_MapRpt"];
                if (string.IsNullOrEmpty(s))
                    return "ND" + int.Parse(this.FK_Flow) + "MyRpt";
                return s;
            }
        }
        public string FK_Flow
        {
            get
            {
                string s= this.Request.QueryString["FK_Flow"];
                if (string.IsNullOrEmpty(s))
                    return "068";
                return s;
            }
        }
        #endregion  Property .

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.RegisterClientScriptBlock("sds",
            "<link href='/WF/Comm/Style/Table" + BP.Web.WebUser.Style + ".css' rel='stylesheet' type='text/css' />");

            RptXmls xmls = new RptXmls();
            xmls.RetrieveAll();

            BP.WF.Rpt.MapRpts rpts = new BP.WF.Rpt.MapRpts(this.FK_Flow);
            foreach (BP.WF.Rpt.MapRpt rpt in rpts)
            {
                this.Pub1.AddFieldSet(rpt.Name);
                this.Pub1.AddUL();
                this.Pub1.Add("<li><a href='Search.aspx?RptNo=" + rpt.No + "&FK_Flow=" + this.FK_Flow + "' > Inquiry </a></li>");
                this.Pub1.Add("<li><a href='Group.aspx?RptNo=" + rpt.No + "&FK_Flow=" + this.FK_Flow + "' > Subgroup analysis </a></li>");
                this.Pub1.Add("<li><a href='D3.aspx?RptNo=" + rpt.No + "&FK_Flow=" + this.FK_Flow + "' > Cross report </a></li>");
                this.Pub1.Add("<li><a href='Contrast.aspx?RptNo=" + rpt.No + "&FK_Flow=" + this.FK_Flow + "' > Comparative analysis </a></li>");
                this.Pub1.AddULEnd();
                this.Pub1.AddFieldSetEnd();
            }
        }
    }
}