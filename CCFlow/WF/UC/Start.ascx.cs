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
    public partial class Start : BP.Web.UC.UCBase3
    {
        public void BindWap(Flows fls)
        {
            this.AddFieldSet("<img src='/WF/Img/Home.gif' ><a href='Home.aspx' >Home</a>-<img src='/WF/Img/Start.gif' > Launch ");
            this.AddUL();
            int i = 0;
            string fk_sort = null;
            foreach (Flow fl in fls)
            {
                if (fl.FlowAppType == FlowAppType.DocFlow)
                    continue;
                i++;
                fk_sort = fl.FK_FlowSort;
                this.AddLi("<a href='MyFlow.aspx?FK_Flow=" + fl.No + "&FK_Node=" + int.Parse(fl.No) + "01&T=" + this.timeKey + "' >" + fl.Name + "</a>&nbsp;<font style=\"color:#77c;font-size=4px\" >" + fl.FK_FlowSortText + "</font>");
            }
            this.AddULEnd();
            this.AddFieldSetEnd();
            return;
        }
        string timeKey = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            this.timeKey = DateTime.Now.ToString("yyyyMMddHHmmss");
            this.Page.Title =  " Work initiated " ;

            //  Get initiated entity.
            Flows fls = BP.WF.Dev2Interface.DB_GenerCanStartFlowsOfEntities(WebUser.No);
            if (WebUser.IsWap)
            {
                BindWap(fls);
                return;
            }
            string appPath = this.Request.ApplicationPath;

            string pageid = this.Request.RawUrl.ToLower();
            if (pageid.Contains("small"))
            {
                if (pageid.Contains("single"))
                    pageid = "SmallSingle";
                else
                    pageid = "Small";
                this.AddBR();
            }
            else
            {
                pageid = "";
            }

            int colspan = 5;
            this.AddTable("width='960px' align=center border=0");
            this.AddTR();
            this.AddCaptionLeft("<img src='/WF/Img/Start.gif' > <b> Launch </b>");
            this.AddTREnd();

            this.AddTR();
            this.AddTDTitle("序");
            this.AddTDTitle("width=80%", " Name ");
            this.AddTDTitle(" Flow chart ");
            this.AddTDTitle(" Batch launched ");
            this.AddTDTitle(" Description ");
            this.AddTREnd();

            int i = 1;
            string fk_sort = null;
            int idx = 0;
            int gIdx = 0;
            foreach (Flow fl in fls)
            {
                if (fl.FlowAppType == FlowAppType.DocFlow)
                    continue;
                idx++;
                //2012.9.16by Li Jian 
                if (fl.FK_FlowSort != fk_sort)
                {
                    gIdx++;
                    this.AddTDB("colspan=" + colspan + " class=Sum onclick=\"GroupBarClick('" + appPath + "','" + gIdx + "')\" ", "<div style='text-align:left; float:left' ><img src='/WF/Style/Min.gif' alert='Min' id='Img" + gIdx + "'   border=0 />&nbsp;<b>" + fl.FK_FlowSortText + "</b>");
                    this.AddTREnd();
                    fk_sort = fl.FK_FlowSort;
                }

                if (fl.FK_FlowSort == fk_sort)
                {
                    this.AddTR("ID='" + gIdx + "_" + idx + "'");
                    this.AddTDIdx(i++);
                }

                fk_sort = fl.FK_FlowSort;

                if (BP.WF.Glo.IsWinOpenStartWork == 1)
                    this.AddTD("<a href=\"javascript:WinOpenIt('MyFlow.aspx?FK_Flow=" + fl.No + "&FK_Node=" + int.Parse(fl.No) + "01&T=" + this.timeKey + "');\" >" + fl.Name + "</a>");
                else if (BP.WF.Glo.IsWinOpenStartWork == 2)
                    this.AddTD("<a href=\"javascript:WinOpenIt('/WF/OneFlow/MyFlow.aspx?FK_Flow=" + fl.No + "&FK_Node=" + int.Parse(fl.No) + "01&T=" + this.timeKey + "');\" >" + fl.Name + "</a>");
                else
                    this.AddTD("<a href='MyFlow" + pageid + ".aspx?FK_Flow=" + fl.No + "&FK_Node=" + int.Parse(fl.No) + "01' >" + fl.Name + "</a>");

                //}
                //else
                //    this.AddTD("<a href=\"javascript:StartListUrl('" + appPath + "','" + fl.StartGuidePara + "?FK_Flow=" + fl.No + "&FK_Node=" + int.Parse(fl.No) + "01&T=" + this.timeKey + "','" + fl.No + "','" + pageid + "')\" >" + fl.Name + "</a>");

                // Flow chart .
                this.AddTD("<a href=\"javascript:WinOpen('" + appPath + "WF/Chart.aspx?FK_Flow=" + fl.No + "&DoType=Chart&T=" + this.timeKey + "','sd');\"  > Turn on </a>");

                // Batch launched .
                if (fl.IsBatchStart)
                    this.AddTD("<a href=\"javascript:WinOpen('" + appPath + "WF/BatchStart.aspx?FK_Flow=" + fl.No + "&DoType=Chart&T=" + this.timeKey + "','sd');\"  > Batch launched </a>");
                else
                    this.AddTD("");

                this.AddTD(fl.Note);
                this.AddTREnd();
            }
            this.AddTRSum();
            this.AddTD("colspan=" + colspan, " Prompt : I was able to initiate the process is more process .");
            this.AddTREnd();
            this.AddTableEnd();
        }
    }
}