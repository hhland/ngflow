using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.En;
using BP.PRJ;
using BP.Web;
public partial class ExpandingApplication_PRJ_EmpPrjStations : WebPage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        Prj prj = new Prj(this.RefNo);
        this.Pub1.AddTable("width='80%'");
        this.Pub1.AddCaptionLeft(prj.Name + " -  Members post setting ");
        this.Pub1.AddTR();
        this.Pub1.AddTDTitle("IDX");
        this.Pub1.AddTDTitle(" Staff ");
        this.Pub1.AddTDTitle(" Name ");
        this.Pub1.AddTDTitle(" Existing posts ");
        this.Pub1.AddTDTitle(" Edit Post ");
        this.Pub1.AddTDTitle(" Removed ");
        this.Pub1.AddTREnd();

        EmpPrjs emps = new EmpPrjs();
        emps.Retrieve(EmpPrjAttr.FK_Prj, this.RefNo);

        int idx = 1;
        foreach (EmpPrj emp in emps)
        {
            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);

            this.Pub1.AddTD(emp.FK_Emp);
            this.Pub1.AddTD(emp.FK_EmpT);

            EmpPrjStations stas = new EmpPrjStations();
            stas.Retrieve(EmpPrjStationAttr.FK_EmpPrj, this.RefNo + "_" + emp.FK_Emp);
            string str = "";
            foreach (EmpPrjStation sta in stas)
            {
                str += sta.FK_StationT + ";";
            }

            if (str == "")
                str = "无";
            this.Pub1.AddTD(str);
            this.Pub1.AddTD("<a href=''> Editor </a>");
            this.Pub1.AddTD(" Removed ");
            this.Pub1.AddTREnd();
        }
        this.Pub1.AddTableEnd();
    }
}