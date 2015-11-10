using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.En;
using BP.Port;
using BP.Web.Controls;
using BP.Web;
using BP.Sys;
namespace CCFlow.WF.Admin
{

    public partial class WF_Admin_CanReturnNodes : BP.Web.WebPage
    {
        #region  Property 
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
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            BP.WF.Node mynd = new BP.WF.Node();
            mynd.NodeID = this.FK_Node;
            mynd.RetrieveFromDBSources();

            if (mynd.HisReturnRole != ReturnRole.ReturnSpecifiedNodes)
            {
                this.Pub1.AddFieldSet(" Operator error :", "<br><br> Rule of the current node is not returned back to the specified node , So you can not operate this function .<br><br> Please node properties => Rules set to return properties in , Return the specified node , This function is effective .<br><br><br><br>");
                return;
            }

            this.Pub1.AddTable("width='80%'");
            this.Pub1.AddCaptionLeft("[" + mynd.Name + "],  Set returnable node .");

            BP.WF.NodeReturns rnds = new NodeReturns();
            rnds.Retrieve(BP.WF.NodeReturnAttr.FK_Node, this.FK_Node);

            BP.WF.Nodes nds = new Nodes();
            nds.Retrieve(BP.WF.Template.NodeAttr.FK_Flow, this.FK_Flow);

            int idx = 0;
            foreach (BP.WF.Node nd in nds)
            {
                if (nd.NodeID == this.FK_Node)
                    continue;

                CheckBox cb = new CheckBox();
                cb.Text = nd.Name;
                cb.ID = "CB_" + nd.NodeID;
                cb.Checked = rnds.IsExits(NodeReturnAttr.ReturnTo, nd.NodeID);

                this.Pub1.AddTR();
                this.Pub1.AddTDIdx(idx++);
                this.Pub1.AddTD("Step" + nd.Step + "");
                this.Pub1.AddTD(cb);
                this.Pub1.AddTREnd();
            }

            this.Pub1.AddTRSum();
            this.Pub1.AddTD();
            Button btn = new Button();
            btn.Text = "Save";
            btn.CssClass = "Btn";
            btn.Click += new EventHandler(btn_Click);
            this.Pub1.AddTD(btn);
            this.Pub1.AddTREnd();
            this.Pub1.AddTableEndWithHR();
            this.Pub1.AddFieldSet(" Special Note :", "1, Only return rule node attribute is set to the node returned formulation , This function is effective .<br> 2, If the node is set to return the node is the current node to the next step , Set meaningless , System not checking , Return when doing checks .");
        }

        void btn_Click(object sender, EventArgs e)
        {

            BP.WF.NodeReturns rnds = new NodeReturns();
            rnds.Delete(BP.WF.NodeReturnAttr.FK_Node, this.FK_Node);

            BP.WF.Nodes nds = new Nodes();
            nds.Retrieve(BP.WF.Template.NodeAttr.FK_Flow, this.FK_Flow);

            int i = 0;
            foreach (BP.WF.Node nd in nds)
            {
                CheckBox cb = this.Pub1.GetCBByID("CB_" + nd.NodeID);
                if (cb == null)
                    continue;

                if (cb.Checked == false)
                    continue;

                NodeReturn nr = new NodeReturn();
                nr.FK_Node = this.FK_Node;
                nr.ReturnTo = nd.NodeID;
                nr.Insert();
                i++;
            }
            if (i == 0)
            {
                this.Alert(" Please select a node to be returned .");
                return;
            }
            this.WinCloseWithMsg(" Setting Success ");
        }
    }
}