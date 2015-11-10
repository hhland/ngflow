using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCFlow.WF.WorkOpt
{
    public partial class WF_WorkOpt_CC : BP.Web.WebPage
    {
        #region  Variable .
        public Int64 FID
        {
            get
            {
                return Int64.Parse(this.Request.QueryString["FID"]);
            }
        }
        public Int64 WorkID
        {
            get
            {
                return Int64.Parse(this.Request.QueryString["WorkID"]);
            }
        }
        public int FK_Node
        {
            get
            {
                return int.Parse(this.Request.QueryString["FK_Node"]);
            }
        }
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        #endregion  Variable .

        protected void Page_Load(object sender, EventArgs e)
        {
          //  BP.Sys.MapData.GenerSpanHeight(null, 2);
            BP.WF.Flow fl = new BP.WF.Flow(this.FK_Flow);
            string sql = "SELECT Title FROM " + fl.PTable + " WHERE OID=" + this.WorkID;
            if (this.FID != 0)
                sql = "SELECT Title FROM " + fl.PTable + " WHERE OID=" + this.FID;
            string title = BP.DA.DBAccess.RunSQLReturnStringIsNull(sql, null);
            if (title == null)
                this.Pub1.AddFieldSet(" Error ", " System abnormalities , Please contact the administrator .");

            this.Title = " Work Cc ";
            this.Pub1.AddTable("width='100%' border=1");
            this.Pub1.AddCaptionLeft(" Please select or enter staff ( Separate multiple personnel by commas ), Then the Send button ...");
            this.Pub1.AddTR();
            //this.Pub1.AddTDTitle(" Recipient :");
            this.Pub1.Add("<TD style='text-align:center;' width='20%' valign='middle'><h5> Recipient :</h5></TD>");

            HiddenField hidden = new HiddenField();
            hidden.ID = "HID_SelectedEmps";

            TextBox tb = new TextBox();
            tb.ID = "TB_Accepter";
            tb.Width = 500;

            Pub1.AddTDBegin("width='530'");
            Pub1.Add(tb);
            Pub1.Add(hidden);
            Pub1.AddTDEnd();

            Button mybtn = new Button();
            mybtn.CssClass = "Btn";
            mybtn.Text = " Select recipient ";
            mybtn.OnClientClick += "javascript:ShowIt(" + tb.ClientID + "," + hidden.ClientID + ");";
            this.Pub1.AddTD(mybtn);
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            //this.Pub1.AddTDTitle(" Title :");
            this.Pub1.Add("<TD style='text-align:center;' valign='middle'><h5> Title :</h5></TD>");
            tb = new TextBox();
            tb.ID = "TB_Title";
            tb.Width = 500;
            tb.Text = title;
            this.Pub1.AddTD(" colspan=2", tb);
            //this.Pub1.AddTD();
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            //this.Pub1.AddTDTitle("消<BR>息<BR>内<br>容");
            this.Pub1.Add("<TD style='text-align:center;' valign='middle'><h5>Msg</h5></TD>");
            tb = new TextBox();
            tb.ID = "TB_Doc";
            tb.Width = 500;
            tb.TextMode = TextBoxMode.MultiLine;
            tb.Rows = 12;
            this.Pub1.AddTD(" colspan=2", tb);
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD("");
            Button btn = new Button();
            btn.CssClass = "Btn";
            btn.ID = "btn";
            btn.Click += new EventHandler(btn_Click);
            btn.Text = " Execution Cc ";
            this.Pub1.AddTD(" colspan=2", btn);
            //this.Pub1.AddTD("");
            this.Pub1.AddTREnd();
            this.Pub1.AddTableEnd();
        }

        void btn_Click(object sender, EventArgs e)
        {
            string accepters = (Pub1.FindControl("HID_SelectedEmps") as HiddenField).Value;// this.Pub1.GetTextBoxByID("TB_Accepter").Text;
            accepters = accepters.Trim();
            if (string.IsNullOrEmpty(accepters))
            {
                this.Alert(" The recipient can not be empty ");
                return;
            }
            string title = this.Pub1.GetTextBoxByID("TB_Title").Text;
            if (string.IsNullOrEmpty(title))
            {
                this.Alert(" The title can not be empty ");
                return;
            }
            string doc = this.Pub1.GetTextBoxByID("TB_Doc").Text;

            // Node .
            BP.WF.Node nd = new BP.WF.Node(this.FK_Node);

            /* Check whether there is a problem staff .*/
            string[] emps = accepters.Split(',');
            BP.Port.Emp myemp = new BP.Port.Emp();
            string errMsg = "";
            foreach (string emp in emps)
            {
                if (string.IsNullOrEmpty(emp))
                    continue;
                myemp.No = emp;
                if (myemp.IsExits == false)
                    errMsg += "@ Staff (" + emp + ") Spelling mistake .";
            }

            if (string.IsNullOrEmpty(errMsg) == false)
            {
                this.Alert(errMsg);
                return;
            }

            // CC Information .

            string msg = "";
            foreach (string emp in emps)
            {
                if (string.IsNullOrEmpty(emp))
                    continue;

                myemp.No = emp;
                myemp.Retrieve();

                msg += "(" + myemp.No + "," + myemp.Name + ")";

                //  Write data depending on the configuration node attributes .
                switch (nd.CCWriteTo)
                {
                    case BP.WF.CCWriteTo.All:
                        BP.WF.Dev2Interface.Node_CC_WriteTo_CClist(this.FK_Node, this.FK_Node, this.WorkID, emp, myemp.Name, title, doc);
                        BP.WF.Dev2Interface.Node_CC_WriteTo_Todolist(this.FK_Node, this.FK_Node, this.WorkID, emp, myemp.Name);
                        break;
                    case BP.WF.CCWriteTo.CCList:
                        BP.WF.Dev2Interface.Node_CC_WriteTo_CClist(this.FK_Node, this.FK_Node, this.WorkID, emp, myemp.Name, title, doc);
                        break;
                    case BP.WF.CCWriteTo.Todolist:
                        BP.WF.Dev2Interface.Node_CC_WriteTo_Todolist(this.FK_Node, this.FK_Node, this.WorkID, emp, myemp.Name);
                        break;
                    default:
                        break;
                }
            }

            // Written to the log .
            BP.WF.Dev2Interface.WriteTrack(nd.FK_Flow, nd.NodeID, this.WorkID, this.FID, " Copied to :" + msg, BP.WF.ActionType.CC,
                null, null, null);

            this.WinCloseWithMsg(" Cc success ...");
        }
    }
}