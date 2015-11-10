using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCFlow.SDKFlowDemo
{
    public partial class DemoJZOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //  Demo matrix output .  Also called N Palace grid output .
            BP.Port.Emps ens = new BP.Port.Emps();
            ens.RetrieveAll();

            int cols = 3; // Define the number of columns displayed  从0 Begin .
            decimal widthCell = 100 / cols;
            this.Pub1.AddTable("width=100% border=0");
            this.Pub1.AddCaption(" Matrix output , Also called n Palace grid output , You can change the number of columns in the output .");
            int idx = -1;
            bool is1 = false;
            foreach (BP.Port.Emp en in ens)
            {
                idx++;
                if (idx == 0)
                    is1 = this.Pub1.AddTR(is1);
                this.Pub1.AddTDBegin("width='" + widthCell + "%' border=0 valign=top");

                #region  Output .

                this.Pub1.Add("<h2> Full name :" + en.Name+"</h2>");

                this.Pub1.Add(" Department :" + en.FK_DeptText);

                #endregion  Output .

                this.Pub1.AddTDEnd();
                if (idx == cols - 1)
                {
                    idx = -1;
                    this.Pub1.AddTREnd();
                }
            }

            while (idx != -1)
            {
                idx++;
                if (idx == cols - 1)
                {
                    idx = -1;
                    this.Pub1.AddTD();
                    this.Pub1.AddTREnd();
                }
                else
                {
                    this.Pub1.AddTD();
                }
            }
            this.Pub1.AddTableEnd();
        }
    }
}