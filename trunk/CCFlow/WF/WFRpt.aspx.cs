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

namespace CCFlow.WF
{
    public partial class WF_WFRpt : BP.Web.WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Request.QueryString["DoType"] != null)
                return;

            string fk_flow = this.Request.QueryString["FK_Flow"];
            string fk_node = this.Request.QueryString["FK_Node"];
            string workid = this.Request.QueryString["WorkID"];

            //if (this.Request.QueryString["DoType"] =="CC")
            //{
            //    BP.WF.CCList cc = new BP.WF.CCList();
            //    cc.MyPK = this.Request.QueryString["CCID"];
            //    cc.Retrieve();
            //    if (cc.HisSta == BP.WF.CCSta.UnRead)
            //    {
            //        cc.HisSta = BP.WF.CCSta.Read;
            //        cc.Update();
            //    }
            //}

            if (this.Request.QueryString["ViewWork"] != null)
                return;

            this.Response.Redirect("./WorkOpt/OneWork/Track.aspx?FK_Flow=" + BP.WF.Dev2Interface.TurnFlowMarkToFlowNo(fk_flow) + "&FK_Node=" + fk_node + "&WorkID=" + workid, true);
            return;
        }
    }

}