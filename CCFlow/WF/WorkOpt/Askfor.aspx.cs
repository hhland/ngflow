using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Web;
using BP.WF;
using BP.DA;

namespace CCFlow.WF.WorkOpt
{
    public partial class Askfor : BP.Web.WebPage
    {
        #region  Parameters 
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
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
                return Int64.Parse(this.Request.QueryString["FID"]);
            }
        }
        public int FK_Node
        {
            get
            {
                return int.Parse(this.Request.QueryString["FK_Node"]);
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Pub2.AddTable("style='width:100%'");

            this.Pub2.AddTR();
            this.Pub2.AddTDTitle("colspan=3", " Work endorsement ");
            this.Pub2.AddTREnd();

            this.Pub2.AddTR();

            this.Pub2.AddTD(" Plus sign people ");
            TextBox tb = new TextBox();
            tb.Text = "";
            tb.Columns = 50;
            tb.ID = "TB_Worker";

            Pub2.AddTDBegin();
            this.Pub2.Add(tb);

            HiddenField hidden = new HiddenField();
            hidden.ID = "HID_SelectedEmps";
            Pub2.Add(hidden);
            Pub2.AddTDEnd();

            Button mybtn = new Button();
            mybtn.CssClass = "Btn";
            mybtn.Text = " Select the plus sign people ";
            mybtn.OnClientClick += "javascript:ShowIt(" + tb.ClientID + "," + hidden.ClientID + ");";
            this.Pub2.AddTD(mybtn);
            //this.Pub2.AddTD(" Please enter a personnel number ");
            this.Pub2.AddTREnd();

            this.Pub2.AddTR();
            this.Pub2.AddTDBegin("colspan=3");
            this.Pub2.Add(" Explain the reasons for endorsement <br>");
            tb = new TextBox();
            tb.TextMode = TextBoxMode.MultiLine;
            tb.Text = " Hello :\t\n  Now you get the job referrals . \t\n " + BP.Web.WebUser.Name;
            tb.ID = "TB_Note";
            tb.Columns = 70;
            tb.Rows = 5;

            this.Pub2.Add(tb);
            this.Pub2.AddTDEnd();
            this.Pub2.AddTREnd();

            this.Pub2.AddTR();
            this.Pub2.AddTD(" Treatment ");
            this.Pub2.AddTDBegin("colspan=2");

            RadioButton rb = new RadioButton();
            rb.ID = "RB_0";
            rb.GroupName = "s";
            rb.Text = " Plus sign after the other , Sent directly to the next step .";
            rb.Checked = true;
            this.Pub2.Add(rb);

            rb = new RadioButton();
            rb.ID = "RB_1";
            rb.GroupName = "s";
            rb.Text = " The other party countersign forwarded to me , Send me to the next step .";
            this.Pub2.Add(rb);
            this.Pub2.AddTDEnd();
            this.Pub2.AddTREnd();


            this.Pub2.AddTR();
            this.Pub2.AddTD("");
            this.Pub2.AddTDBegin("colspan=2");
            Button btn = new Button();
            btn.Text = " Submit ";
            btn.ID = "Btn_Submit";
            btn.Click += new EventHandler(btn_Click);
            this.Pub2.Add(btn);

            btn = new Button();
            btn.Text = " Cancel ";
            btn.ID = "Btn_Cancel";
            btn.Click += new EventHandler(btn_Click);
            this.Pub2.Add(btn);
            this.Pub2.AddTDEnd();
            this.Pub2.AddTREnd();
            this.Pub2.AddTableEnd();

            Int64 workid = Int64.Parse(this.Request.QueryString["WorkID"]);
            string sql = "SELECT  * FROM ND" + int.Parse(this.FK_Flow) + "Track WHERE ActionType=24 AND WorkID=" + workid + " AND (EmpFrom='" + WebUser.No + "' OR EmpTo='" + WebUser.No + "')";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);

            if (dt.Rows.Count != 0)
            {
                this.Pub2.AddFieldSet(" Endorsement information ");
                foreach (DataRow dr in dt.Rows)
                {
                    this.Pub2.Add("<br> Node :" + dr[TrackAttr.NDFromT] + "<hr>");
                    this.Pub2.Add(" Information :" + DataType.ParseText2Html(dr[TrackAttr.Msg].ToString()) + "<br>");
                }
                this.Pub2.AddFieldSetEnd();
            }
        }

        public void ToMsg(string msg, string type)
        {
            this.Session["info"] = msg;
            this.Application["info" + WebUser.No] = msg;
            Glo.SessionMsg = msg;
            this.Response.Redirect("../MyFlowInfo" + Glo.FromPageType + ".aspx?FK_Flow=" + this.FK_Flow + "&FK_Type=" + type + "&FK_Node=" + this.FK_Node + "&WorkID=" + this.WorkID, false);
        }

        void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.ID == "Btn_Cancel")
            {
                this.Response.Redirect("../MyFlow.aspx?FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.FK_Node + "&WorkID=" + this.WorkID + "&FID=" + this.FID, true);
                return;
            }

            Int64 workid = Int64.Parse(this.Request.QueryString["WorkID"]);
            int fk_node = int.Parse(this.Request.QueryString["FK_Node"]);
            string askFor = (Pub2.FindControl("HID_SelectedEmps") as HiddenField).Value;// this.Pub2.GetTextBoxByID("TB_Worker").Text;
            string note = this.Pub2.GetTextBoxByID("TB_Note").Text;

            BP.Port.Emp emp = new BP.Port.Emp();
            emp.No = askFor;
            int i = emp.RetrieveFromDBSources();
            if (i != 1)
            {
                this.Alert(" People enter endorsement （" + askFor + "） Incorrect . Plus sign can only choose one ");
                return;
            }
            if (emp.No == WebUser.No)
            {
                this.Alert(" Can not let yourself plus sign .");
                return;
            }

            BP.WF.AskforHelpSta sta = BP.WF.AskforHelpSta.AfterDealSend;
            bool is1 = this.Pub2.GetRadioButtonByID("RB_0").Checked;
            if (is1)
                sta = BP.WF.AskforHelpSta.AfterDealSend;

            is1 = this.Pub2.GetRadioButtonByID("RB_1").Checked;
            if (is1)
                sta = BP.WF.AskforHelpSta.AfterDealSendByWorker;

            try
            {
                string info = BP.WF.Dev2Interface.Node_Askfor(workid, sta, askFor, note);
                this.ToMsg(info, "Info");
            }
            catch (Exception ex)
            {
                this.Pub2.AddMsgOfWarning("err", ex.Message);
            }
        }
    }
}