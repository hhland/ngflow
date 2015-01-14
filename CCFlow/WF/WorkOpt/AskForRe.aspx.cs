using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BP.DA;
using BP.WF;
using BP.Sys;
using BP.Web;
namespace CCFlow.WF.WorkOpt
{
    public partial class AskForRe : System.Web.UI.Page
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

            TextBox tb = new TextBox();
            tb.ID = "TB_Doc";
            tb.Columns = 50;
            tb.Rows = 10;
            tb.TextMode = TextBoxMode.MultiLine;

            // Obtain endorsement views .
            GenerWorkFlow gwf = new GenerWorkFlow(this.WorkID);
            tb.Text = gwf.Paras_AskForReply; 

            this.Pub1.Add(tb);
            this.Pub1.AddBR();

            Button btn = new Button();
            btn.Text = " Comments submitted for endorsement ";
            btn.ID = "Bt";
            btn.Click += new EventHandler(btn_Click);
            this.Pub1.Add(btn);
        }

        void btn_Click(object sender, EventArgs e)
        {
            string replay= this.Pub1.GetTextBoxByID("TB_Doc").Text;

            string info = BP.WF.Dev2Interface.Node_AskforReply(this.FK_Flow, this.FK_Node,
                 this.WorkID, this.FID, replay);

            this.ToMsg(info, "Info");
        }

        public void ToMsg(string msg, string type)
        {
            this.Session["info"] = msg;
            this.Application["info" + WebUser.No] = msg;
            BP.WF.Glo.SessionMsg = msg;
            this.Response.Redirect("../MyFlowInfo" + BP.WF.Glo.FromPageType + ".aspx?FK_Flow=" + this.FK_Flow + "&FK_Type=" + type + "&FK_Node=" + this.FK_Node + "&WorkID=" + this.WorkID, false);
        }

    }
}