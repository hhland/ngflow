using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;

namespace CCFlow.WF.UC
{
    public partial class UCFlows : BP.Web.UC.UCBase3
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BP.WF.Flows fls = new BP.WF.Flows();
            fls.RetrieveAll();

            FlowSorts ens = new FlowSorts();
            ens.RetrieveAll();

            DataTable dt = BP.WF.Dev2Interface.DB_GenerCanStartFlowsOfDataTable(BP.Web.WebUser.No);

            int cols = 3; // Define the number of columns displayed  From 0 Begin .
            decimal widthCell = 100 / cols;
            this.AddTable("width=100% border=0");
            this.AddCaption(" Initiate the process -( Explanation : The process is not the point you do not have permission to initiate .)");
            int idx = -1;
            bool is1 = false;

            string timeKey = "s" + this.Session.SessionID + DateTime.Now.ToString("yyMMddHHmmss");
            foreach (FlowSort en in ens)
            {
                idx++;
                if (idx == 0)
                    is1 = this.AddTR(is1);

                this.AddTDBegin("width='" + widthCell + "%' border=0 valign=top");
                // Output category .
                //this.AddFieldSet(en.Name);
                this.AddB(en.Name);
                this.AddUL();

                #region  Output Process .
                foreach (Flow fl in fls)
                {
                    if (fl.FlowAppType == FlowAppType.DocFlow)
                        continue;

                    if (fl.FK_FlowSort != en.No)
                        continue;

                    bool isHaveIt = false;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["No"].ToString() != fl.No)
                            continue;
                        isHaveIt = true;
                        break;
                    }

                    string extUrl = "";
                    if (fl.IsBatchStart)
                        extUrl = "<a href='/WF/BatchStart.aspx?FK_Flow="+fl.No+"' > Batch launched </a>|<a href='/WF/Rpt/Search.aspx?RptNo=ND" + int.Parse(fl.No) + "MyRpt&FK_Flow=" + fl.No + "'> Inquiry </a>|<a href=\"javascript:WinOpen('/WF/Chart.aspx?FK_Flow=" + fl.No + "&DoType=Chart&T=" + timeKey + "','sd');\"  >Chart</a>";
                    else
                        extUrl = "<a href='/WF/Rpt/Search.aspx?RptNo=ND" + int.Parse(fl.No) + "MyRpt&FK_Flow=" + fl.No + "'> Inquiry </a>|<a href=\"javascript:WinOpen('/WF/Chart.aspx?FK_Flow=" + fl.No + "&DoType=Chart&T=" + timeKey + "','sd');\"  >Chart</a>";

                    if (isHaveIt)
                    {
                            if (Glo.IsWinOpenStartWork == 1)
                                this.AddLi("<a href=\"javascript:WinOpenIt('MyFlow.aspx?FK_Flow=" + fl.No + "&FK_Node=" + int.Parse(fl.No) + "01&T=" + timeKey + "');\" >" + fl.Name + "</a> - " + extUrl);
                            else if (Glo.IsWinOpenStartWork == 2)
                                this.AddLi("<a href=\"javascript:WinOpenIt('/WF/OneFlow/MyFlow.aspx?FK_Flow=" + fl.No + "&FK_Node=" + int.Parse(fl.No) + "01&T=" + timeKey + "');\" >" + fl.Name + "</a> - " + extUrl);
                            else
                                this.AddLi("<a href='MyFlow.aspx?FK_Flow=" + fl.No + "&FK_Node=" + int.Parse(fl.No) + "01' >" + fl.Name + "</a> - " + extUrl);
                    }
                    else
                    {
                        this.AddLi(fl.Name + " - " + extUrl);
                    }
                }
                #endregion  Output Process .

                this.AddULEnd();
               // this.AddFieldSetEnd();


                this.AddTDEnd();
                if (idx == cols - 1)
                {
                    idx = -1;
                    this.AddTREnd();
                }
            }

            while (idx != -1)
            {
                idx++;
                if (idx == cols - 1)
                {
                    idx = -1;
                    this.AddTD();
                    this.AddTREnd();
                }
                else
                {
                    this.AddTD();
                }
            }
            this.AddTableEnd();

        }
    }
}