using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.Port;
using BP.Web;
using BP.En;
using BP.WF.Template;

namespace CCFlow.Plug_in.CCFlow.WF.WorkOpt
{
    public partial class ToNodes : BP.Web.WebPage
    {
        #region  Go to the node .
        public string CFlowNo
        {
            get
            {
                return this.Request.QueryString["CFlowNo"];
            }
        }
        public string WorkIDs
        {
            get
            {
                return this.Request.QueryString["WorkIDs"];
            }
        }
        public string DoFunc
        {
            get
            {
                return this.Request.QueryString["DoFunc"];
            }
        }
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
                if (this.Request.QueryString["FID"] != null)
                    return Int64.Parse(this.Request.QueryString["FID"]);
                return 0;
            }
        }
        #endregion  Go to the node 

        protected void Page_Load(object sender, EventArgs e)
        {
            // Get the current node arrives .
            Nodes nds = BP.WF.Dev2Interface.WorkOpt_GetToNodes(this.FK_Flow, this.FK_Node, this.WorkID, this.FID);

            // Check for different forms .
            bool isSubYBD = false; // Different forms ?
            foreach (Node mynd in nds)
            {
                BP.Web.Controls.RadioBtn rb = new BP.Web.Controls.RadioBtn();
                if (mynd.NodeID == 0)
                {
                    rb = new BP.Web.Controls.RadioBtn();
                    rb.GroupName = "s";
                    rb.Text = "<b> You can distribute different forms start node </b>";
                    rb.ID = "RB_SameSheet";
                    rb.Attributes["onclick"] = "RBSameSheet(this);";
                    if (this.IsPostBack == false)
                        rb.Checked = true;

                    this.Pub1.Add(rb);
                    this.Pub1.AddBR();
                    isSubYBD = true;
                    continue;
                }

                if (isSubYBD == true)
                {
                    /* If it is different form .*/
                    CheckBox cb = new CheckBox();
                    cb.ID = "CB_" + mynd.NodeID;
                    cb.Text = mynd.Name;
                    this.Pub1.Add("&nbsp;&nbsp;&nbsp;&nbsp;");
                    this.Pub1.Add(cb);

                    if (mynd.HisDeliveryWay == DeliveryWay.BySelected)
                    {
                        /* Select from the previous step to send staff .*/
                        this.Pub1.Add(" - <a href=\"javascript:WinShowModalDialog_Accepter('Accepter.aspx?FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.FK_Node + "&ToNode=" + mynd.NodeID + "&WorkID=" + this.WorkID + "&FID=" + this.FID + "&type=1')\" > Choose to accept staff </a>");
                    }
                    this.Pub1.AddBR();
                    continue;
                }
                else
                {
                    rb = new BP.Web.Controls.RadioBtn();
                    rb.GroupName = "s";
                    rb.Text = mynd.Name;
                    rb.ID = "RB_" + mynd.NodeID;
                    rb.Attributes["onclick"] = "SetUnEable(this);";
                    this.Pub1.Add(rb);
                    if (mynd.HisDeliveryWay == DeliveryWay.BySelected)
                    {
                        /* Select from the previous step to send staff .*/
                        this.Pub1.Add(" - <a href=\"javascript:WinShowModalDialog_Accepter('Accepter.aspx?FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.FK_Node + "&ToNode=" + mynd.NodeID + "&WorkID=" + this.WorkID + "&FID=" + this.FID + "&type=1')\" > Select recipient </a>");
                    }
                    this.Pub1.AddBR();
                }
            }

            this.Pub1.AddHR();
            Button btn = new Button();
            btn.ID = "To";
            btn.Text = "  执 行  ";
            this.Pub1.Add(btn);
            btn.Click += new EventHandler(btn_Click);

            btn = new Button();
            btn.ID = "Btn_Cancel";
            btn.Text = " Cancel / Return ";
            this.Pub1.Add(btn);
            btn.Click += new EventHandler(btn_Click);
        }

        void btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.ID == "Btn_Cancel")
            {
                string url = "../MyFlow.aspx?FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.FK_Node + "&WorkID=" + this.WorkID + "&FID=" + this.FID;
                this.Response.Redirect(url, true);
                return;
            }

            #region  Calculated arrival node .
            Nodes nds = BP.WF.Dev2Interface.WorkOpt_GetToNodes(this.FK_Flow, this.FK_Node, this.WorkID, this.FID);
            string toNodes = "";
            foreach (Node mynd in nds)
            {
                if (mynd.HisRunModel == RunModel.SubThread
                    && mynd.HisSubThreadType == SubThreadType.UnSameSheet)
                    continue; // If the node is a child thread .

                if (mynd.NodeID == 0)
                    continue;

                BP.Web.Controls.RadioBtn rb = this.Pub1.GetRadioBtnByID("RB_" + mynd.NodeID);
                if (rb.Checked == false)
                    continue;

                toNodes = mynd.NodeID.ToString();
                break;
            }

            if (toNodes == "")
            {
                //  Check whether the child has a different thread forms .
                bool isHave = false;
                foreach (Node mynd in nds)
                {
                    if (mynd.NodeID == 0)
                        isHave = true;
                }

                if (isHave)
                {
                    /* Increase child thread different forms */
                    foreach (Node mynd in nds)
                    {
                        if (mynd.HisSubThreadType != SubThreadType.UnSameSheet)
                            continue;

                        CheckBox cb = this.Pub1.GetCBByID("CB_" + mynd.NodeID);
                        if (cb == null)
                            continue;

                        if (cb.Checked == true)
                            toNodes += "," + mynd.NodeID;
                    }
                }
            }
            #endregion  Calculated choice to node .

            if (toNodes == "")
            {
                this.Pub1.AddFieldSetRed(" Send an error ", " You do not have to reach the node selection .");
                return;
            }

            //  Performing transmission .
            string msg = "";
            Node nd = new Node(this.FK_Node);
            Work wk = nd.HisWork;
            wk.OID = this.WorkID;
            wk.Retrieve();

            try
            {
                string toNodeStr = int.Parse(FK_Flow) + "01";
                // If the starting node 
                if (toNodeStr == toNodes)
                {
                    // The parameter update to the database inside .
                    GenerWorkFlow gwf = new GenerWorkFlow();
                    gwf.WorkID = this.WorkID;
                    gwf.RetrieveFromDBSources();
                    gwf.Paras_ToNodes = toNodes;
                    gwf.Save();

                    WorkNode firstwn = new WorkNode(wk, nd);

                    Node toNode = new Node(toNodeStr);
                    msg = firstwn.NodeSend(toNode, gwf.Starter).ToMsgOfHtml();
                }
                else
                {
                    msg = BP.WF.Dev2Interface.WorkOpt_SendToNodes(this.FK_Flow,
                        this.FK_Node, this.WorkID, this.FID, toNodes).ToMsgOfHtml();
                }
            }
            catch (Exception ex)
            {
                this.Pub1.AddFieldSetRed(" Send an error ", ex.Message);
                return;
            }

            #region  Business logic methods to handle the general sent successfully after , This method may throw an exception .
            try
            {
                // Business logic methods to handle the general sent successfully after , This method may throw an exception .
                Glo.DealBuinessAfterSendWork(this.FK_Flow, this.WorkID, this.DoFunc, WorkIDs, this.CFlowNo, 0, null);
            }
            catch (Exception ex)
            {
                this.ToMsg(msg, ex.Message);
                return;
            }
            #endregion  Business logic methods to handle the general sent successfully after , This method may throw an exception .


            /* Processing steering problems .*/
            switch (nd.HisTurnToDeal)
            {
                case TurnToDeal.SpecUrl:
                    string myurl = nd.TurnToDealDoc.Clone().ToString();
                    if (myurl.Contains("&") == false)
                        myurl += "?1=1";
                    myurl = BP.WF.Glo.DealExp(myurl, wk, null);
                    myurl += "&FromFlow=" + this.FK_Flow + "&FromNode=" + this.FK_Node + "&PWorkID=" + this.WorkID + "&UserNo=" + WebUser.No + "&SID=" + WebUser.SID;
                    this.Response.Redirect(myurl, true);
                    return;
                case TurnToDeal.TurnToByCond:
                    TurnTos tts = new TurnTos(this.FK_Flow);
                    if (tts.Count == 0)
                        throw new Exception("@ You do not turn to the conditions set after the completion of the node .");
                    foreach (TurnTo tt in tts)
                    {
                        tt.HisWork = wk;
                        if (tt.IsPassed == true)
                        {
                            string url = tt.TurnToURL.Clone().ToString();
                            if (url.Contains("&") == false)
                                url += "?1=1";
                            url = BP.WF.Glo.DealExp(url, wk, null);
                            url += "&PFlowNo=" + this.FK_Flow + "&FromNode=" + this.FK_Node + "&PWorkID=" + this.WorkID + "&UserNo=" + WebUser.No + "&SID=" + WebUser.SID;
                            this.Response.Redirect(url, true);
                            return;
                        }
                    }
#warning  For Shanghai modify it if you can not find the path information prompted by the system .
                    this.ToMsg(msg, "info");
                    //throw new Exception(" You define the steering condition is not satisfied , No Exit .");
                    break;
                default:
                    this.ToMsg(msg, "info");
                    break;
            }
            return;
        }

        public void ToMsg(string msg, string type)
        {
            this.Session["info"] = msg;
            this.Application["info" + WebUser.No] = msg;

            Glo.SessionMsg = msg;
            this.Response.Redirect("./../MyFlowInfo" + Glo.FromPageType + ".aspx?FK_Flow=" + this.FK_Flow + "&FK_Type=" + type + "&FK_Node=" + this.FK_Node + "&WorkID=" + this.WorkID, false);
        }
    }
}