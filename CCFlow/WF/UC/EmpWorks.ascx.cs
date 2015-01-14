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
    public partial class EmpWorks : BP.Web.UC.UCBase3
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
            if (this.GroupBy == "PRI")
            {
                this.BindList_PRI();
                return;
            }

            string appPath = BP.WF.Glo.CCFlowAppPath;//this.Request.ApplicationPath;

            bool isPRI = BP.WF.Glo.IsEnablePRI;
            string groupVals = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (groupVals.Contains("@" + dr[this.GroupBy].ToString() + ","))
                    continue;
                groupVals += "@" + dr[this.GroupBy].ToString() + ",";
            }

            int colspan = 10;
            this.Pub1.AddTable("width='100%'  cellspacing='0' cellpadding='0' align=left");
            this.Pub1.AddCaption(" Upcoming work ");
            string extStr = "";
            if (this.IsHungUp)
                extStr = "&IsHungUp=1";

            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("ID");
            this.Pub1.AddTDTitle("width=40%", " Title ");

            if (this.GroupBy != "FlowName")
                this.Pub1.AddTDTitle("<a href='EmpWorks.aspx?GroupBy=FlowName" + extStr + "&T=" + this.timeKey + "' > Process </a>");

            if (this.GroupBy != "NodeName")
                this.Pub1.AddTDTitle("<a href='EmpWorks.aspx?GroupBy=NodeName" + extStr + "&T=" + this.timeKey + "' > Node </a>");

            if (this.GroupBy != "StarterName")
                this.Pub1.AddTDTitle("<a href='EmpWorks.aspx?GroupBy=StarterName" + extStr + "&T=" + this.timeKey + "' > Sponsor </a>");

            if (isPRI)
                this.Pub1.AddTDTitle("<a href='EmpWorks.aspx?GroupBy=PRI" + extStr + "&T=" + this.timeKey + "' > Priority </a>");

            this.Pub1.AddTDTitle(" Launch date ");
            this.Pub1.AddTDTitle(" Accepted ");
            this.Pub1.AddTDTitle(" The term ");
            this.Pub1.AddTDTitle(" Status ");
            this.Pub1.AddTDTitle(" Remark ");
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
                    this.Pub1.AddTD("colspan=" + colspan + " class=TRSum onclick=\"GroupBarClick('" + appPath + "','" + gIdx + "')\" ", "<div style='text-align:left; float:left' ><img src='Style/Min.gif' alert='Min' id='Img" + gIdx + "'   border=0 />&nbsp;<b>" + g.Replace(",", "") + "</b>");
                else
                    this.Pub1.AddTD("colspan=" + colspan + " class=TRSum onclick=\"GroupBarClick('" + appPath + "','" + gIdx + "')\" ", "<div style='text-align:left; float:left' ><img src='Style/Min.gif' alert='Min' id='Img" + gIdx + "'   border=0 />&nbsp;<b>" + g.Replace(",", "") + "</b>");
                this.Pub1.AddTREnd();

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[this.GroupBy].ToString() + "," != g)
                        continue;
                    string sdt = dr["SDT"] as string;
                    string paras = dr["AtPara"] as string;

                    this.Pub1.AddTR("ID='" + gIdx + "_" + i + "'");
                    i++;

                    int isRead = int.Parse(dr["IsRead"].ToString());
                    this.Pub1.AddTDIdx(i);
                    if (BP.WF.Glo.IsWinOpenEmpWorks == true)
                    {
                        if (isRead == 0)
                            this.Pub1.AddTD("Class=TTD onclick=\"SetImg('" + appPath + "','I" + gIdx + "_" + i + "')\"", "<a href=\"javascript:WinOpenIt('MyFlow.aspx?FK_Flow=" + dr["FK_Flow"] + "&FK_Node=" + dr["FK_Node"] + "&FID=" + dr["FID"] + "&WorkID=" + dr["WorkID"] + "&IsRead=0&T=" + this.timeKey + "&Paras="+paras+"');\" ><img class=Icon src='Img/Mail_UnRead.png' id='I" + gIdx + "_" + i + "' />" + dr["Title"].ToString() + "</a>");
                        else
                            this.Pub1.AddTD("Class=TTD", "<a href=\"javascript:WinOpenIt('MyFlow.aspx?FK_Flow=" + dr["FK_Flow"] + "&FK_Node=" + dr["FK_Node"] + "&FID=" + dr["FID"] + "&WorkID=" + dr["WorkID"] + "&Paras="+paras+"&T="+this.timeKey+"');\"  ><img src='Img/Mail_Read.png' id='I" + gIdx + "_" + i + "' class=Icon />" + dr["Title"].ToString() + "</a>");
                    }
                    else
                    {
                        if (isRead == 0)
                            this.Pub1.AddTD("Class=TTD onclick=\"SetImg('" + appPath + "','I" + gIdx + "_" + i + "')\" ", "<a href=\"MyFlow" + this.PageSmall + ".aspx?FK_Flow=" + dr["FK_Flow"] + "&FK_Node=" + dr["FK_Node"] + "&FID=" + dr["FID"] + "&WorkID=" + dr["WorkID"] + "&IsRead=0&Paras="+paras+"&T=" + this.timeKey + "\" ><img class=Icon src='Img/Mail_UnRead.png' id='I" + gIdx + "_" + i + "' />" + dr["Title"].ToString() + "</a>");
                        else
                            this.Pub1.AddTD("Class=TTD ", "<a href=\"MyFlow" + this.PageSmall + ".aspx?FK_Flow=" + dr["FK_Flow"] + "&FK_Node=" + dr["FK_Node"] + "&FID=" + dr["FID"] + "&WorkID=" + dr["WorkID"] + "&Paras=" + paras + "&T=" + this.timeKey + "\" ><img class=Icon src='Img/Mail_Read.png' id='I" + gIdx + "_" + i + "' />" + dr["Title"].ToString() + "</a>");
                    }

                    if (this.GroupBy != "FlowName")
                        this.Pub1.AddTD(dr["FlowName"].ToString());

                    if (this.GroupBy != "NodeName")
                        this.Pub1.AddTD(dr["NodeName"].ToString());

                    if (this.GroupBy != "StarterName")
                        this.Pub1.AddTD(dr["Starter"].ToString() + " " + dr["StarterName"]);

                    if (isPRI)
                        this.Pub1.AddTD("<img class=Icon src='Img/PRI/" + dr["PRI"].ToString() + ".png' class=Icon />");

                    this.Pub1.AddTD(dr["RDT"].ToString().Substring(5));
                    this.Pub1.AddTD(dr["ADT"].ToString().Substring(5));
                    this.Pub1.AddTD(dr["SDT"].ToString().Substring(5));

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

                    this.Pub1.AddTD(dr["FlowNote"].ToString());
                    this.Pub1.AddTREnd();
                }
            }
            this.Pub1.AddTableEnd();
            return;
        }
        public void BindList_PRI()
        {
            string appPath = this.Request.ApplicationPath;
            int colspan = 11;

            string extStr = "";
            if (this.IsHungUp)
                extStr = "&IsHungUp=1";

            this.Pub1.AddTable("width='100%'");
            this.Pub1.AddCaption(" Upcoming work ");
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("ID");
            this.Pub1.AddTDTitle("width='38%'", " Title ");

            if (this.GroupBy != "FlowName")
                this.Pub1.AddTDTitle("<a href='EmpWorks.aspx?GroupBy=FlowName" + extStr + "&T=" + this.timeKey + "' > Process </a>");

            if (this.GroupBy != "NodeName")
                this.Pub1.AddTDTitle("<a href='EmpWorks.aspx?GroupBy=NodeName" + extStr + "&T=" + this.timeKey + "' > Node </a>");

            if (this.GroupBy != "StarterName")
                this.Pub1.AddTDTitle("<a href='EmpWorks.aspx?GroupBy=StarterName" + extStr + "&T=" + this.timeKey + "' > Sponsor </a>");

            this.Pub1.AddTDTitle(" Launch date ");
            this.Pub1.AddTDTitle(" Accepted ");
            this.Pub1.AddTDTitle(" The term ");
            this.Pub1.AddTDTitle(" Status ");
            this.Pub1.AddTDTitle(" Remark ");
            this.Pub1.AddTREnd();

            int i = 0;
            bool is1 = false;
            DateTime cdt = DateTime.Now;
            int gIdx = 0;
            SysEnums ses = new SysEnums("PRI");
            foreach (SysEnum se in ses)
            {

                gIdx++;
                this.Pub1.AddTR();
                this.Pub1.AddTD("colspan=" + colspan + " class=TRSum onclick=\"GroupBarClick('" + appPath + "','" + gIdx + "')\" ", "<div style='text-align:left; float:left' ><img src='Style/Min.gif' alert='Min' id='Img" + gIdx + "' />&nbsp;<img src='Img/PRI/" + se.IntKey + ".png'  class=Icon />");
                this.Pub1.AddTREnd();

                string pri = se.IntKey.ToString();
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["PRI"].ToString() != pri)
                        continue;

                    string sdt = dr["SDT"] as string;
                    string paras = dr["AtPara"] as string;

                    this.Pub1.AddTR("ID='" + gIdx + "_" + i + "'");
                    i++;
                    this.Pub1.AddTDIdx(i);
                    if (BP.WF.Glo.IsWinOpenEmpWorks == true)
                        this.Pub1.AddTD("Class=TTD width='38%'", "<a href=\"javascript:WinOpenIt('MyFlow.aspx?FK_Flow=" + dr["FK_Flow"] + "&FK_Node=" + dr["FK_Node"] + "&FID=" + dr["FID"] + "&WorkID=" + dr["WorkID"] + "&Paras=" + paras + "&T=" + this.timeKey + "');\"  >" + dr["Title"].ToString() + "</a>");
                    else
                        this.Pub1.AddTD("Class=TTD width='38%'", "<a href=\"MyFlow" + this.PageSmall + ".aspx?FK_Flow=" + dr["FK_Flow"] + "&FK_Node=" + dr["FK_Node"] + "&FID=" + dr["FID"] + "&WorkID=" + dr["WorkID"] + "&Paras=" + paras + "&T=" + this.timeKey + "\" >" + dr["Title"].ToString() + "</a>");

                    if (this.GroupBy != "FlowName")
                        this.Pub1.AddTD(dr["FlowName"].ToString());

                    if (this.GroupBy != "NodeName")
                        this.Pub1.AddTD(dr["NodeName"].ToString());

                    if (this.GroupBy != "StarterName")
                        this.Pub1.AddTD(dr["Starter"].ToString() + " " + dr["StarterName"]);

                    this.Pub1.AddTD(dr["RDT"].ToString().Substring(5));
                    this.Pub1.AddTD(dr["ADT"].ToString().Substring(5));
                    this.Pub1.AddTD(dr["SDT"].ToString().Substring(5));

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
                    this.Pub1.AddTD(dr["FlowNote"].ToString());
                    this.Pub1.AddTREnd();
                }
            }
            this.Pub1.AddTRSum();
            this.Pub1.AddTD("colspan=" + colspan, "&nbsp;");
            this.Pub1.AddTREnd();
            this.Pub1.AddTableEnd();
        }
        public DataTable dt = null;
        string timeKey;
        protected void Page_Load(object sender, EventArgs e)
        {
            timeKey = DateTime.Now.ToString("yyyyMMddHHmmss");
            this.FK_Flow = this.Request.QueryString["FK_Flow"];
            if (this.IsHungUp)
                dt = BP.WF.Dev2Interface.DB_GenerHungUpList();
            else
                dt = BP.WF.Dev2Interface.DB_GenerEmpWorksOfDataTable();
            this.BindList();
        }
    }
}