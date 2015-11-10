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
    public partial class WF_Admin_CanCancelNodes : BP.Web.WebPage
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

            if (mynd.HisCancelRole != CancelRole.SpecNodes)
            {
                this.Pub1.AddFieldSet(" Operator error :", "<br><br> Avoidance rules of the current node to the specified node is not revoked , So you can not operate this function .<br><br> Please node properties => Avoidance rules in setting property , Revoke the specified node , This function is effective .<br><br><br><br>");
                return;
            }

            this.Pub1.AddTable("width='100%'");
            this.Pub1.AddCaptionLeft("[" + mynd.Name + "],  Set revocable node .");

            BP.WF.NodeCancels rnds = new NodeCancels();
            rnds.Retrieve(BP.WF.NodeCancelAttr.FK_Node, this.FK_Node);

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
                cb.Checked = rnds.IsExits(NodeCancelAttr.CancelTo, nd.NodeID);

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
            this.Pub1.AddTD();
            this.Pub1.AddTREnd();
            this.Pub1.AddTableEnd();

            this.Pub1.AddFieldSet(" Special Note :", "1, Only avoidance rules node attribute is set to develop a node revocation , This function is effective .<br> 2, Set revoked node node if the current node is the next step , Set meaningless , System not checking , Revocation only do check .");
        }

        void btn_Click(object sender, EventArgs e)
        {

            BP.WF.NodeCancels rnds = new NodeCancels();
            rnds.Delete(BP.WF.NodeCancelAttr.FK_Node, this.FK_Node);

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

                NodeCancel nr = new NodeCancel();
                nr.FK_Node = this.FK_Node;
                nr.CancelTo = nd.NodeID;
                nr.Insert();
                i++;
            }
            if (i == 0)
            {
                this.Alert(" Please select a node to be revoked .");
                return;
            }
            this.WinCloseWithMsg(" Setting Success ");
        }
    }
}