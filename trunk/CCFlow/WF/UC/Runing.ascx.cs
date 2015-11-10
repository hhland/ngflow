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
using BP.WF;
using BP.Port;
using BP.Sys;
using BP.Web.Controls;
using BP.DA;
using BP.En;
using BP.Web;

namespace CCFlow.WF.UC
{
    public partial class Runing : BP.Web.UC.UCBase3
    {
        #region  Property .
        public string _PageSamll = null;
        public string PageSmall
        {
            get
            {
                if (_PageSamll == null)
                {
                    if (this.PageID.ToLower().Contains("smallsingle"))
                        _PageSamll = "SmallSingle";
                    else if (this.PageID.ToLower().Contains("small"))
                        _PageSamll = "Small";
                    else
                        _PageSamll = "";
                }
                return _PageSamll;
            }
        }
        public string GroupBy
        {
            get
            {
                string s = this.Request.QueryString["GroupBy"];
                if (s == null)
                    s = "FlowName";
                return s;
            }
        }
        #endregion  Property .

        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = BP.WF.Dev2Interface.DB_GenerRuning();
            this.Page.Title = " Working in transit ";
            if (WebUser.IsWap)
            {
                this.BindWap();
                return;
            }

            string appPath = BP.WF.Glo.CCFlowAppPath;//this.Request.ApplicationPath;
            string fk_flow = this.Request.QueryString["FK_Flow"];

            int colspan = 6;
            this.Pub1.AddTable("border=1px align=center width='100%'");

            if (WebUser.IsWap)
                this.Pub1.AddCaption("<img src='Img/Home.gif' >&nbsp;<a href='Home.aspx' >Home</a>-<img src='Img/EmpWorks.gif' > Working in transit ");
            else
                this.Pub1.AddCaption(" Working in transit ");

            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("nowarp=true", "No.");
            this.Pub1.AddTDTitle("nowarp=true width='40%'", " Title ");

            if (this.GroupBy != "FlowName")
                this.Pub1.AddTDTitle("<a href='" + this.PageID + ".aspx?GroupBy=FlowName&FK_Flow=" + fk_flow + "' > Process </a>");

            if (this.GroupBy != "NodeName")
                this.Pub1.AddTDTitle("<a href='" + this.PageID + ".aspx?GroupBy=NodeName&FK_Flow=" + fk_flow + "' > The current node </a>");

            if (this.GroupBy != GenerWorkFlowAttr.StarterName)
                this.Pub1.AddTDTitle("<a href='" + this.PageID + ".aspx?GroupBy=StarterName&FK_Flow=" + fk_flow + "' > Sponsor </a>");

            this.Pub1.AddTDTitle("nowarp=true", " Launch date ");
            this.Pub1.AddTDTitle("nowarp=true", " Operating ");
            this.Pub1.AddTREnd();

            string groupVals = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (groupVals.Contains("@" + dr[this.GroupBy]))
                    continue;
                groupVals += "@" + dr[this.GroupBy];
            }

            int i = 0;
            bool is1 = false;
            string title = null;
            string workid = null;
            fk_flow = null;
            int gIdx = 0;
            string[] gVals = groupVals.Split('@');
            foreach (string g in gVals)
            {
                if (string.IsNullOrEmpty(g))
                    continue;

                gIdx++;
                this.Pub1.AddTR();
                this.Pub1.AddTD("colspan=" + colspan + " class=TRSum onclick=\"GroupBarClick('" + appPath + "','" + gIdx + "')\" ", "<div style='text-align:left; float:left' ><img src='Style/Min.gif' alert='Min' id='Img" + gIdx + "'   border=0 />&nbsp;<b>" + g + "</b>");
                this.Pub1.AddTREnd();

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[this.GroupBy].ToString() != g)
                        continue;
                    i++;
                    this.Pub1.AddTR("ID='" + gIdx + "_" + i + "'");
                    this.Pub1.AddTDIdx(i);

                    BP.WF.WFState wfstate = (WFState)int.Parse(dr["WFState"].ToString());
                    title = "<img src='Img/WFState/" + wfstate + ".png' class='Icon' />" + dr["Title"].ToString();

                    workid = dr["WorkID"].ToString();
                    fk_flow = dr["FK_Flow"].ToString();

                    this.Pub1.Add("<TD class=TTD><a href=\"javascript:WinOpen('WFRpt.aspx?WorkID=" + workid + "&FK_Flow=" + fk_flow + "&FID=" + dr["FID"] + "')\" >" + title + "</a></TD>");
                    //  this.Pub1.AddTDDoc(title, 50, title);
                    if (this.GroupBy != "FlowName")
                        this.Pub1.AddTD(dr["FlowName"].ToString());

                    if (this.GroupBy != "NodeName")
                        this.Pub1.AddTD(dr["NodeName"].ToString());

                    if (this.GroupBy != GenerWorkFlowAttr.StarterName)
                        this.Pub1.AddTD(dr[GenerWorkFlowAttr.StarterName].ToString());

                    this.Pub1.AddTD(dr["RDT"].ToString());
                    this.Pub1.AddTDBegin();
                    this.Pub1.Add("<a href=\"javascript:UnSend('" + appPath + "','" + this.PageSmall + "','" + dr["FID"] + "','" + workid + "','" + fk_flow + "');\" ><img src='Img/Action/UnSend.png' border=0 class=Icon /> Undo Send </a>");
                    this.Pub1.Add("<a href=\"javascript:Press('" + appPath + "','" + dr["FID"] + "','" + workid + "','" + fk_flow + "');\" ><img src='Img/Action/Press.png' border=0 class=Icon /> Reminders </a>");

                    this.Pub1.AddTDEnd();
                    this.Pub1.AddTREnd();
                }
            }
            this.Pub1.AddTRSum();
            this.Pub1.AddTD("colspan=" + colspan, "&nbsp;");
            this.Pub1.AddTREnd();
            this.Pub1.AddTableEnd();
        }
        public void BindWap()
        {
            this.Clear();
            this.AddFieldSet("<img src='Img/Home.gif' ><a href='Home.aspx' >Home</a>-<img src='Img/EmpWorks.gif' >" + " Working in transit ");
            string sql = " SELECT a.WorkID FROM WF_GenerWorkFlow A, WF_GenerWorkerlist B  WHERE A.WorkID=B.WorkID   AND B.FK_EMP='" + BP.Web.WebUser.No + "' AND B.IsEnable=1";
            GenerWorkFlows gwfs = new GenerWorkFlows();
            gwfs.RetrieveInSQL(GenerWorkFlowAttr.WorkID, "(" + sql + ")");
            int i = 0;
            bool is1 = true;
            this.AddUL();
            foreach (GenerWorkFlow gwf in gwfs)
            {
                i++;
                is1 = this.AddTR(is1);
                this.AddTDBegin("border=0");
                this.AddLi(gwf.Title + gwf.NodeName);
                this.Add("<a href=\"javascript:Do(' You acknowledge that you ?','MyFlowInfo" + BP.WF.Glo.FromPageType + ".aspx?DoType=UnSend&FID=" + gwf.FID + "&WorkID=" + gwf.WorkID + "&FK_Flow=" + gwf.FK_Flow + "');\" ><img src='Img/btn/delete.gif' border=0 /> Cancel </a>");
                this.Add("<a href=\"javascript:WinOpen('WFRpt.aspx?WorkID=" + gwf.WorkID + "&FK_Flow=" + gwf.FK_Flow + "&FID=0')\" ><img src='Img/btn/rpt.gif' border=0 /> Report </a>");
            }
            this.AddULEnd();
            this.AddFieldSetEnd();
        }
    }
}