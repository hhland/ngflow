using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCFlow.SDKFlowDemo.BPFramework.Output
{
    public partial class DemoUserList : BP.Web.WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region  Executive Function .
            switch (this.DoType)
            {
                case "Del":
                    BP.Port.Emp en = new BP.Port.Emp();
                    en.No = this.RefNo;
                    //en.Delete();
                    break;
                default:
                    break;
            }
            #endregion  Executive Function .

            this.Pub1.AddTable("width=100%");
            this.Pub1.AddCaption(" Operator List ");
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("No.");
            this.Pub1.AddTDTitle(" Serial number ");
            this.Pub1.AddTDTitle(" Name ");
            this.Pub1.AddTDTitle(" Department ");
            this.Pub1.AddTDTitle(" Operating ");
            this.Pub1.AddTREnd();

            BP.Port.Emps ens = new BP.Port.Emps();
            ens.RetrieveAllFromDBSource();
            int idx = 0;
            foreach (BP.Port.Emp en in ens)
            {
                idx++;
                this.Pub1.AddTR();
                this.Pub1.AddTDIdx(idx);
                this.Pub1.AddTD(en.No);
                this.Pub1.AddTD(en.Name);
                this.Pub1.AddTD(en.FK_DeptText);
                this.Pub1.AddTD("<a href=\"javascript:Del('" + en.No + "')\" ><img src='/WF/Img/Btn/Delete.gif' border=0/> Operating </a>");
                this.Pub1.AddTREnd();
            }
            this.Pub1.AddTableEnd();


        }

    }
}