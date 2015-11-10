using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.DA;
using BP.Web;
namespace CCFlow.WF
{

    public partial class WF_Hurry : BP.Web.WebPage
    {
        #region  Property .
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        public int FK_Node
        {
            get
            {
                return int.Parse(this.Request.QueryString["FK_Node"]);
            }
        }
        public Int64 WorkID
        {
            get
            {
                return Int64.Parse(this.Request.QueryString["WorkID"]);
            }
        }
        public Int64 FID
        {
            get
            {
                try
                {
                    return Int64.Parse(this.Request.QueryString["FID"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
        #endregion  Property .

        protected void Page_Load(object sender, EventArgs e)
        {
            HungUp hu = new HungUp();
            hu.MyPK = this.WorkID + "_" + this.FK_Node;
            int i = hu.RetrieveFromDBSources();

            GenerWorkFlow gwf = new GenerWorkFlow();
            gwf.WorkID = this.WorkID;
            if (gwf.RetrieveFromDBSources() == 0)
            {
                this.Pub1.AddFieldSet(" Error "," Is currently the starting node , Or work does not exist .");
                return;
            }

            this.Pub1.AddFieldSet(" Work <b>(" + gwf.Title + ")</b> Suspend Mode ");
            RadioButton rb = new RadioButton();
            rb.GroupName = "s";
            rb.Text = " Permanently suspend ";
            rb.ID = "RB_HungWay0";
            if (hu.HungUpWay == 0)
                rb.Checked = true;
            else
                rb.Checked = false;

            this.Pub1.Add(rb);
            this.Pub1.AddBR();

            rb = new RadioButton();
            rb.GroupName = "s";
            rb.Text = " At a specified date automatically released pending .";
            rb.ID = "RB_HungWay1";
            if (hu.HungUpWay == HungUpWay.SpecDataRel )
                rb.Checked = true;
            else
                rb.Checked = false;
            this.Pub1.Add(rb);
            this.Pub1.AddBR();

            this.Pub1.Add("&nbsp;&nbsp;&nbsp;&nbsp; Pending the date of the lifting process :");
            BP.Web.Controls.TB tb = new BP.Web.Controls.TB();
            tb.ShowType = BP.Web.Controls.TBType.DateTime;
            tb.ID = "TB_RelData";
            if (hu.DTOfUnHungUpPlan.Length == 0)
            {
                DateTime dt = DateTime.Now.AddDays(7);
                hu.DTOfUnHungUpPlan = dt.ToString(DataType.SysDataTimeFormat);
            }
            tb.Text = hu.DTOfUnHungUpPlan;
            this.Pub1.Add(tb);
            this.Pub1.AddFieldSetEnd();

            this.Pub1.AddFieldSet(" Pending reason :");
            tb = new BP.Web.Controls.TB();
            tb.ID = "TB_Note";
            tb.TextMode = TextBoxMode.MultiLine;
            tb.Columns = 70;
            tb.Height = 50;
            this.Pub1.Add(tb);
            this.Pub1.AddFieldSetEnd();

            this.Pub1.Add("&nbsp;&nbsp;&nbsp;&nbsp;");
            Button btn = new Button();
            btn.ID = "Btn_OK";

            if (gwf.WFState == WFState.HungUp)
                btn.Text = "  Unsuspend  ";
            else
                btn.Text = "  Pending  ";

            btn.Click += new EventHandler(btn_Click);
            this.Pub1.Add(btn);

            btn = new Button();
            btn.ID = "Btn_Cancel";
            btn.Text = "  Return  ";
            btn.Click += new EventHandler(btn_Click);
            this.Pub1.Add(btn);
        }

        void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.ID == "Btn_Cancel")
            {
                this.Response.Redirect("../MyFlow.aspx?FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.FK_Node + "&WorkID=" + this.WorkID + "&FID=" + this.FID, true);
                return;
            }

            try
            {

                HungUpWay way = HungUpWay.SpecDataRel;
                RadioButton rb = this.Pub1.GetRadioButtonByID("RB_HungWay0");
                if (rb.Checked)
                    way = HungUpWay.Forever;

                string reldata = this.Pub1.GetTBByID("TB_RelData").Text;
                string note = this.Pub1.GetTBByID("TB_Note").Text;
                GenerWorkFlow gwf = new GenerWorkFlow(this.WorkID);
                if (gwf.WFState == WFState.HungUp)
                    BP.WF.Dev2Interface.Node_UnHungUpWork(this.FK_Flow, this.WorkID, note);
                else
                    BP.WF.Dev2Interface.Node_HungUpWork(this.FK_Flow, this.WorkID, (int)way, reldata, note);

                this.WinClose();
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
                //this.Pub1.AddMsgOfWarning.Response.Write(ex);
            }
        }
    }
}