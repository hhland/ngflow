using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Sys;
using BP.WF.Rpt;
namespace CCFlow.WF.MapDef.Rpt
{
    public partial class Do : BP.Web.WebPage
    {
        public string RptNo
        {
            get
            {
                return this.Request.QueryString["RptNo"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            switch (this.DoType)
            {
                case "Del":
                    MapRpt rpt = new MapRpt();
                    rpt.No = this.RptNo;
                    rpt.Delete();
                    this.WinClose("ok");
                    break;
                case "EnableKeySearch":
                    MapData rpt1 = new MapData(this.RptNo);
                    rpt1.RptIsSearchKey = !rpt1.RptIsSearchKey;
                    rpt1.Update();
                    this.WinClose("ok");
                    break;
                default:
                    break;
            }
        }
    }
}