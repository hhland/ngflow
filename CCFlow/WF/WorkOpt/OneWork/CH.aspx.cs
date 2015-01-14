using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCFlow.WF.OneWork
{
public partial class WF_WorkOpt_OneWork_CH : System.Web.UI.Page
{
    public string FK_Flow
    {
        get
        {
            return this.Request.QueryString["FK_Flow"];
        }
    }
    public string WorkID
    {
        get
        {
            return this.Request.QueryString["WorkID"];
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        this.Pub1.AddTable("width=100%");
        //    this.Pub1.AddCaptionLeftTX(" Work assessment information ");
        this.Pub1.AddTR();
        this.Pub1.AddTDTitle("IDX");
        this.Pub1.AddTDTitle(" Node ");
        this.Pub1.AddTDTitle(" Processors ");
        this.Pub1.AddTDTitle(" Accept time ");
        this.Pub1.AddTDTitle(" Deadline (天)");
        this.Pub1.AddTDTitle(" Time should be completed ");
        this.Pub1.AddTDTitle(" The actual completion time ");
        this.Pub1.AddTDTitle(" Status ");
        this.Pub1.AddTREnd();

        string sql = "SELECT A.*, B.Name AS FK_NodeT,B.DeductDays, c.Name as EmpName  FROM V" + this.FK_Flow + " A, WF_Node B, WF_Emp C WHERE A.FK_Node=b.NodeID AND A.Rec=C.No AND A.OID=" + this.WorkID;

        DataTable dt = null;
        try
        {
            dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
        }
        catch
        {
            this.Pub1.AddTableEnd();
            this.Pub1.AddFieldSetRed(" Error ", 
                "V" + this.FK_Flow + " view Not generated ,  Please tell administrators , Process Designer executed execution process checks can solve this problem .");
            return;
        }

        int idx = 0;
        foreach (DataRow dr in dt.Rows)
        {
            idx++;
            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx);
            this.Pub1.AddTD(dr["FK_NodeT"].ToString());
            this.Pub1.AddTD(dr["EmpName"].ToString());
            this.Pub1.AddTD(dr["RDT"].ToString());

            int deductDays = int.Parse(dr["DeductDays"].ToString());
            this.Pub1.AddTD(deductDays);

            //  Time should be completed .
            DateTime cdt = BP.DA.DataType.ParseSysDateTime2DateTime(dr["CDT"].ToString());
            DateTime sdt = cdt.AddDays(int.Parse(dr["DeductDays"].ToString()));
            this.Pub1.AddTD(sdt.ToString(BP.DA.DataType.SysDatatimeFormatCN));
            this.Pub1.AddTD(dr["CDT"].ToString());

            if (sdt < cdt)
                this.Pub1.AddTD(" Overdue ");
            else
                this.Pub1.AddTD(" Normal ");

            this.Pub1.AddTREnd();
        }
        this.Pub1.AddTableEnd();
    }
}
}