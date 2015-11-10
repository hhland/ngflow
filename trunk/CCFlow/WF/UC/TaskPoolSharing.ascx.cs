using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.Port;
using BP.Sys;
using BP.Web.Controls;
using BP.DA;
using BP.En;
using BP.Web;

namespace CCFlow.WF.UC
{
    public partial class TaskPoolSharingUC : BP.Web.UC.UCBase3
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

            string appPath = this.Request.ApplicationPath;

            bool isPRI = BP.WF.Glo.IsEnablePRI;
            string groupVals = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (groupVals.Contains("@" + dr[this.GroupBy].ToString() + ","))
                    continue;
                groupVals += "@" + dr[this.GroupBy].ToString() + ",";
            }

            int colspan = 10;
           
            this.AddTable("width='100%' align=left");
            this.AddCaption(" Task pool -<a href='TaskPoolApply.aspx' > Application down to work </a>");

            string extStr = "";
            if (this.IsHungUp)
                extStr = "&IsHungUp=1";

            this.AddTR();
            this.AddTDTitle("ID");
            this.AddTDTitle("width=40%", " Title ");

            if (this.GroupBy != "FlowName")
                this.AddTDTitle("<a href='" + this.PageID + ".aspx?GroupBy=FlowName" + extStr + "&T=" + this.timeKey + "' > Process </a>");

            if (this.GroupBy != "NodeName")
                this.AddTDTitle("<a href='" + this.PageID + ".aspx?GroupBy=NodeName" + extStr + "&T=" + this.timeKey + "' > Node </a>");

            if (this.GroupBy != "StarterName")
                this.AddTDTitle("<a href='" + this.PageID + ".aspx?GroupBy=StarterName" + extStr + "&T=" + this.timeKey + "' > Sponsor </a>");

            if (isPRI)
                this.AddTDTitle("<a href='" + this.PageID + ".aspx?GroupBy=PRI" + extStr + "&T=" + this.timeKey + "' > Priority </a>");

            this.AddTDTitle(" Launch date ");
            this.AddTDTitle(" Accepted ");
            this.AddTDTitle(" The term ");
            this.AddTDTitle(" Status ");
            this.AddTDTitle(" Remark ");
            this.AddTREnd();

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

                this.AddTR();
                if (this.GroupBy == "Rec")
                    this.AddTD("colspan=" + colspan + " class=TRSum onclick=\"GroupBarClick('" + appPath + "','" + gIdx + "')\" ", "<div style='text-align:left; float:left' ><img src='/WF/Style/Min.gif' alert='Min' id='Img" + gIdx + "'   border=0 />&nbsp;<b>" + g.Replace(",", "") + "</b>");
                else
                    this.AddTD("colspan=" + colspan + " class=TRSum onclick=\"GroupBarClick('" + appPath + "','" + gIdx + "')\" ", "<div style='text-align:left; float:left' ><img src='/WF/Style/Min.gif' alert='Min' id='Img" + gIdx + "'   border=0 />&nbsp;<b>" + g.Replace(",", "") + "</b>");
                this.AddTREnd();

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[this.GroupBy].ToString() + "," != g)
                        continue;
                    string sdt = dr["SDT"] as string;
                    this.AddTR("ID='" + gIdx + "_" + i + "'");
                    i++;

                    int isRead = int.Parse(dr["IsRead"].ToString());
                    this.AddTDIdx(i);
                    if (BP.WF.Glo.IsWinOpenEmpWorks == true || 1 == 1)
                    {
                        if (isRead == 0)
                            this.AddTD("Class=TTD onclick=\"SetImg('" + appPath + "','I" + gIdx + "_" + i + "')\"", "<a href=\"javascript:WinOpenIt('" + appPath + "WF/MyFlow.aspx?FK_Flow=" + dr["FK_Flow"] + "&FK_Node=" + dr["FK_Node"] + "&FID=" + dr["FID"] + "&WorkID=" + dr["WorkID"] + "&IsRead=0&T=" + this.timeKey + "','" + dr["WorkID"] + "');\" ><img class=Icon src='/WF/Img/Mail_UnRead.png' id='I" + gIdx + "_" + i + "' />" + dr["Title"].ToString() + "</a>");
                        else
                            this.AddTD("Class=TTD", "<a href=\"javascript:WinOpenIt('/WF/MyFlow.aspx?FK_Flow=" + dr["FK_Flow"] + "&FK_Node=" + dr["FK_Node"] + "&FID=" + dr["FID"] + "&WorkID=" + dr["WorkID"] + "','" + dr["WorkID"] + "');\"  ><img src='/WF/Img/Mail_Read.png' id='I" + gIdx + "_" + i + "' class=Icon />" + dr["Title"].ToString() + "</a>");
                    }
                    else
                    {
                        if (isRead == 0)
                            this.AddTD("Class=TTD onclick=\"SetImg('" + appPath + "','I" + gIdx + "_" + i + "')\" ", "<a href=\"MyFlow" + this.PageSmall + ".aspx?FK_Flow=" + dr["FK_Flow"] + "&FK_Node=" + dr["FK_Node"] + "&FID=" + dr["FID"] + "&WorkID=" + dr["WorkID"] + "&IsRead=0&T=" + this.timeKey + "\" ><img class=Icon src='/WF/Img/Mail_UnRead.png' id='I" + gIdx + "_" + i + "' />" + dr["Title"].ToString() + "</a>");
                        else
                            this.AddTD("Class=TTD ", "<a href=\"MyFlow" + this.PageSmall + ".aspx?FK_Flow=" + dr["FK_Flow"] + "&FK_Node=" + dr["FK_Node"] + "&FID=" + dr["FID"] + "&WorkID=" + dr["WorkID"] + "&T=" + this.timeKey + "\" ><img class=Icon src='/WF/Img/Mail_Read.png' id='I" + gIdx + "_" + i + "' />" + dr["Title"].ToString() + "</a>");
                    }

                    if (this.GroupBy != "FlowName")
                        this.AddTD(dr["FlowName"].ToString());

                    if (this.GroupBy != "NodeName")
                        this.AddTD(dr["NodeName"].ToString());

                    if (this.GroupBy != "StarterName")
                        this.AddTD(dr["Starter"].ToString() + " " + dr["StarterName"]);

                    if (isPRI)
                        this.AddTD("<img class=Icon src='/WF/Img/PRI/" + dr["PRI"].ToString() + ".png' class=Icon />");

                    this.AddTD(dr["RDT"].ToString().Substring(5));
                    this.AddTD(dr["ADT"].ToString().Substring(5));
                    this.AddTD(dr["SDT"].ToString().Substring(5));

                    DateTime mysdt = DataType.ParseSysDate2DateTime(sdt);
                    if (cdt >= mysdt)
                    {
                        if (cdt.ToString("yyyy-MM-dd") == mysdt.ToString("yyyy-MM-dd"))
                            this.AddTDCenter(" Normal ");
                        else
                            this.AddTDCenter("<font color=red> Overdue </font>");
                    }
                    else
                    {
                        this.AddTDCenter(" Normal ");
                    }
                    this.AddTD(dr["FlowNote"].ToString());
                    this.AddTREnd();
                }
            }
            this.AddTableEnd();
            return;
        }
        public void BindList_PRI()
        {
            string appPath = this.Request.ApplicationPath;
            int colspan = 12;

            string extStr = "";
            if (this.IsHungUp)
                extStr = "&IsHungUp=1";

            this.AddTable("width='100%' align=left");
            this.AddCaption(" Task pool -<a href='TaskPoolApply.aspx' > Application down to work </a>");
            this.AddTR();
            this.AddTDTitle("ID");
            this.AddTDTitle("width='38%'", " Title ");

            if (this.GroupBy != "FlowName")
                this.AddTDTitle("<a href='" + this.PageID + ".aspx?GroupBy=FlowName" + extStr + "&T=" + this.timeKey + "' > Process </a>");

            if (this.GroupBy != "NodeName")
                this.AddTDTitle("<a href='" + this.PageID + ".aspx?GroupBy=NodeName" + extStr + "&T=" + this.timeKey + "' > Node </a>");

            if (this.GroupBy != "StarterName")
                this.AddTDTitle("<a href='" + this.PageID + ".aspx?GroupBy=StarterName" + extStr + "&T=" + this.timeKey + "' > Sponsor </a>");

            //  this.AddTDTitle(" Priority ");
            this.AddTDTitle(" Launch date ");
            this.AddTDTitle(" Accepted ");
            this.AddTDTitle(" The term ");
            this.AddTDTitle(" Status ");
            this.AddTDTitle(" Remark ");
            this.AddTREnd();

            int i = 0;
            bool is1 = false;
            DateTime cdt = DateTime.Now;
            int gIdx = 0;
            SysEnums ses = new SysEnums("PRI");
            foreach (SysEnum se in ses)
            {
                gIdx++;
                this.AddTR();
                this.AddTD("colspan=" + colspan + " class=TRSum onclick=\"GroupBarClick('" + appPath + "','" + gIdx + "')\" ", "<div style='text-align:left; float:left' ><img src='/WF/Style/Min.gif' alert='Min' id='Img" + gIdx + "' />&nbsp;<img src='/WF/Img/PRI/" + se.IntKey + ".png'  class=Icon />");
                this.AddTREnd();

                string pri = se.IntKey.ToString();
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["PRI"].ToString() != pri)
                        continue;

                    string sdt = dr["SDT"] as string;
                    this.AddTR("ID='" + gIdx + "_" + i + "'");
                    i++;
                    this.AddTDIdx(i);
                    if (BP.WF.Glo.IsWinOpenEmpWorks == true || 1 == 1)
                        this.AddTD("Class=TTD width='38%'", "<a href=\"javascript:WinOpenIt('/WF/MyFlow.aspx?FK_Flow=" + dr["FK_Flow"] + "&FK_Node=" + dr["FK_Node"] + "&FID=" + dr["FID"] + "&WorkID=" + dr["WorkID"] + "&T=" + this.timeKey + "','" + dr["WorkID"] + "');\"  >" + dr["Title"].ToString() + "</a>");
                    else
                        this.AddTD("Class=TTD width='38%'", "<a href=\"MyFlow" + this.PageSmall + ".aspx?FK_Flow=" + dr["FK_Flow"] + "&FK_Node=" + dr["FK_Node"] + "&FID=" + dr["FID"] + "&WorkID=" + dr["WorkID"] + "&T=" + this.timeKey + "\" >" + dr["Title"].ToString() + "</a>");

                    if (this.GroupBy != "FlowName")
                        this.AddTD(dr["FlowName"].ToString());

                    if (this.GroupBy != "NodeName")
                        this.AddTD(dr["NodeName"].ToString());

                    if (this.GroupBy != "StarterName")
                        this.AddTD(dr["Starter"].ToString() + " " + dr["StarterName"]);

                    this.AddTD(dr["RDT"].ToString().Substring(5));
                    this.AddTD(dr["ADT"].ToString().Substring(5));
                    this.AddTD(dr["SDT"].ToString().Substring(5));

                    DateTime mysdt = DataType.ParseSysDate2DateTime(sdt);
                    if (cdt >= mysdt)
                    {
                        if (cdt.ToString("yyyy-MM-dd") == mysdt.ToString("yyyy-MM-dd"))
                            this.AddTDCenter(" Normal ");
                        else
                            this.AddTDCenter("<font color=red> Overdue </font>");
                    }
                    else
                    {
                        this.AddTDCenter(" Normal ");
                    }
                    this.AddTD(dr["FlowNote"].ToString());
                    this.AddTREnd();
                }
            }
           
            this.AddTableEnd();
            return;
        }
        public DataTable dt = null;
        string timeKey;
        protected void Page_Load(object sender, EventArgs e)
        {
            timeKey = DateTime.Now.ToString("yyyyMMddHHmmss");
            this.FK_Flow = this.Request.QueryString["FK_Flow"];
            dt = BP.WF.Dev2Interface.DB_TaskPool();

            this.BindList();
        }
    }
}