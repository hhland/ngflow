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
    public partial class HungUp : BP.Web.UC.UCBase3
    {
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
        public string FK_Flow
        {
            get
            {
                string s = this.Request.QueryString["FK_Flow"];
                if (s == null)
                    return this.ViewState["FK_Flow"] as string;
                return s;
            }
            set
            {
                this.ViewState["FK_Flow"] = value;
            }
        }
        public bool IsHungUp
        {
            get
            {
                string s = this.Request.QueryString["IsHungUp"];
                if (s == null)
                    return false;
                else
                    return true;
            }
        }
        public string GroupBy
        {
            get
            {
                string s = this.Request.QueryString["GroupBy"];
                if (s == null)
                {
                    if (this.DoType == "CC")
                        s = "Rec";
                    else
                        s = "FlowName";
                }
                return s;
            }
        }
        public void BindList()
        {

            bool isPRI = BP.WF.Glo.IsEnablePRI;
            string groupVals = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (groupVals.Contains("@" + dr[this.GroupBy].ToString() + ","))
                    continue;
                groupVals += "@" + dr[this.GroupBy].ToString() + ",";
            }

            int colspan = 9;
           
            this.Pub1.AddTable("width='100%' align=left");
            this.Pub1.AddCaption("<img src='/WF/Img/Runing.gif' ><a href='" + PageID + ".aspx?IsHungUp=1' > Suspend Work </a>");

            string extStr = "";
            if (this.IsHungUp)
                extStr = "&IsHungUp=1";

            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("ID");
            this.Pub1.AddTDTitle(" Title ");

            if (this.GroupBy != "FlowName")
                this.Pub1.AddTDTitle("<a href='" + this.PageID + ".aspx?GroupBy=FlowName" + extStr + "' > Process </a>");

            if (this.GroupBy != "NodeName")
                this.Pub1.AddTDTitle("<a href='" + this.PageID + ".aspx?GroupBy=NodeName" + extStr + "' > Node </a>");

            if (this.GroupBy != "StarterName")
                this.Pub1.AddTDTitle("<a href='" + this.PageID + ".aspx?GroupBy=StarterName" + extStr + "' > Sponsor </a>");

            if (isPRI)
                this.Pub1.AddTDTitle("<a href='" + this.PageID + ".aspx?GroupBy=PRI" + extStr + "' > Priority </a>");

            this.Pub1.AddTDTitle(" Launch date ");
            this.Pub1.AddTDTitle(" Accepted ");
            this.Pub1.AddTDTitle(" The term ");
            this.Pub1.AddTDTitle(" Status ");
            this.Pub1.AddTREnd();

            int i = 0;
            bool is1 = false;
            DateTime cdt = DateTime.Now;
            string[] gVals = groupVals.Split('@');
            int gIdx = 0;
            foreach (string g in gVals)
            {
                if (string.IsNullOrEmpty(g))
                    continue;

                gIdx++;

                this.Pub1.AddTR();
                if (this.GroupBy == "Rec")
                    this.Pub1.AddTD("colspan=" + colspan + " class=TRSum onclick=\"GroupBarClick('" + gIdx + "')\" ", "<div style='text-align:left; float:left' ><img src='./Style/Min.gif' alert='Min' id='Img" + gIdx + "'   border=0 />&nbsp;<b>" + g.Replace(",", "") + "</b>");
                else
                    this.Pub1.AddTD("colspan=" + colspan + " class=TRSum onclick=\"GroupBarClick('" + gIdx + "')\" ", "<div style='text-align:left; float:left' ><img src='./Style/Min.gif' alert='Min' id='Img" + gIdx + "'   border=0 />&nbsp;<b>" + g.Replace(",", "") + "</b>");
                this.Pub1.AddTREnd();

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[this.GroupBy].ToString() + "," != g)
                        continue;

                    string sdt = dr[GenerWorkFlowAttr.SDTOfFlow] as string;

                    this.Pub1.AddTR("ID='" + gIdx + "_" + i + "'");
                    i++;

                    this.Pub1.AddTDIdx(i);
                    if (BP.WF.Glo.IsWinOpenEmpWorks == true)
                    {
                        this.Pub1.AddTD("Class=TTD onclick=\"SetImg('I" + gIdx + "_" + i + "')\"", "<a href=\"javascript:WinOpenIt('MyFlow.aspx?FK_Flow=" + dr["FK_Flow"] + "&FK_Node=" + dr["FK_Node"] + "&FID=" + dr["FID"] + "&WorkID=" + dr["WorkID"] + "&IsRead=0');\" ><img src='/WF/Img/Mail_UnRead.png' id='I" + gIdx + "_" + i + "' border=0/>" + dr["Title"].ToString());
                    }
                    else
                    {
                        this.Pub1.AddTD("Class=TTD", "<a href=\"MyFlow" + this.PageSmall + ".aspx?FK_Flow=" + dr["FK_Flow"] + "&FK_Node=" + dr["FK_Node"] + "&FID=" + dr["FID"] + "&WorkID=" + dr["WorkID"] + "\" >" + dr["Title"].ToString());
                    }

                    if (this.GroupBy != "FlowName")
                        this.Pub1.AddTD(dr["FlowName"].ToString());

                    if (this.GroupBy != "NodeName")
                        this.Pub1.AddTD(dr["NodeName"].ToString());

                    if (this.GroupBy != "StarterName")
                        this.Pub1.AddTD(dr["Starter"].ToString() + " " + dr["StarterName"]);

                    if (isPRI)
                        this.Pub1.AddTD("<img src='/WF/Img/PRI/" + dr["PRI"].ToString() + ".png' class=ImgPRI />");

                    this.Pub1.AddTD(dr["RDT"].ToString().Substring(5));
                    this.Pub1.AddTD(dr["SDTOfFlow"].ToString().Substring(5));
                    this.Pub1.AddTD(dr["SDTOfNode"].ToString().Substring(5));

                    DateTime mysdt = DataType.ParseSysDate2DateTime(sdt);
                    if (cdt >= mysdt)
                    {
                        if (cdt.ToString("yyyy-MM-dd") == mysdt.ToString("yyyy-MM-dd"))
                            this.Pub1.AddTDCenter(" Normal ");
                        else
                            this.Pub1.AddTDCenter("<font color=red> Overdue </font>");
                    }
                    else
                    {
                        this.Pub1.AddTDCenter(" Normal ");
                    }
                    this.Pub1.AddTREnd();
                }
            }
            this.Pub1.AddTableEnd();
            return;
        }
        public DataTable dt = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            dt = BP.WF.Dev2Interface.DB_GenerHungUpList();
            this.BindList();
        }
    }
}