using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.En;
using BP.Web;
using BP.Port;

namespace CCFlow.SDKFlowDemo
{
    public partial class DemoTurnPage : System.Web.UI.Page
    {
        #region  Property .
        public int PageIdx
        {
            get
            {
                try
                {
                    return int.Parse(this.Request.QueryString["PageIdx"]);
                }
                catch 
                {
                    return 1;
                }
            }
        }
        #endregion  Property .

        protected void Page_Load(object sender, EventArgs e)
        {
            // Variable definitions :  The first few pages .
            int currPageIdx = this.PageIdx;
            int pageSize = 12; // Page number of records .

            // Entity Query .
            BP.WF.Nodes ens = new BP.WF.Nodes();
            BP.En.QueryObject qo = new QueryObject(ens);
            qo.AddWhere(BP.WF.Template.NodeAttr.NodePosType, "1"); //  Set the search criteria .
            

            // Put the code into the form the tail ,  Form  第1,2,3,4,5 页 ....... 
            this.Pub2.Clear();
            this.Pub2.BindPageIdx(qo.GetCount(),
                pageSize, currPageIdx, "DemoTurnPage.aspx?1=2&3=xx");

            // Each page has 15 DATA , Take the first 2 Data page .
            qo.DoQuery("NodeID", pageSize, currPageIdx);

            this.Pub1.AddTable();
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("No.");
            this.Pub1.AddTDTitle(" Node number ");
            this.Pub1.AddTDTitle(" Node Name ");
            this.Pub1.AddTDTitle(" Operating ");
            this.Pub1.AddTREnd();

            int idx = 0;
            foreach (BP.WF.Node en in ens)
            {
                idx++;
                this.Pub1.AddTR();
                this.Pub1.AddTDIdx(idx);
                this.Pub1.AddTD(en.NodeID);
                this.Pub1.AddTD(en.Name);
                this.Pub1.AddTD("<a href='http://ccflow.org/case.aspx?ID="+en.NodeID+"' > Turn on </a>");
                this.Pub1.AddTREnd();
            }
            this.Pub1.AddTableEnd();


        }
    }
}