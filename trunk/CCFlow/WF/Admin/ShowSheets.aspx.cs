using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BP.WF;
using BP.En;
using BP.Port;
using BP.Web.Controls;
using BP.Web;
using BP.Sys;

namespace CCFlow.WF.Admin
{
    public partial class WF_Admin_WorkEndSheet : BP.Web.WebPage
    {
        #region  Property 
        /// <summary>
        ///  Process ID 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        /// <summary>
        ///  Node 
        /// </summary>
        public int FK_Node
        {
            get
            {
                return int.Parse(this.Request.QueryString["FK_Node"]);
            }
        }
        #endregion  Property 

        protected void Page_Load(object sender, EventArgs e)
        {
            BP.WF.Node mynd = new BP.WF.Node(this.FK_Node);
            if (mynd.IsStartNode)
            {
                this.Pub1.AddMsgOfWarning(" Prompt :", " This property is set at the beginning of the node does not make sense .");
                return;
            }

            Nodes nds = new Nodes(this.FK_Flow);
            this.Title = " Please select the node (" + mynd.Name + ") Displayed at the bottom of the form ";

            this.Pub1.AddTable("width='100%'");
            this.Pub1.AddCaptionLeft(" Please settings are displayed in this form of decentralized form ");
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle(" Step ");
            this.Pub1.AddTDTitle(" Node Name ( Note Select this node after node is invalid )");
            this.Pub1.AddTREnd();

            foreach (BP.WF.Node nd in nds)
            {
                this.Pub1.AddTR();
                this.Pub1.AddTD(nd.Step);
                CheckBox cb = new CheckBox();
                cb.ID = "CB_" + nd.NodeID;
                cb.Text = nd.Name;

                if (mynd.ShowSheets.Contains("@" + nd.NodeID) == true)
                    cb.Checked = true;

                this.Pub1.AddTD(cb);
                this.Pub1.AddTREnd();
            }

            this.Pub1.AddTRSum();
            this.Pub1.AddTD();
            Button btn = new Button();
            btn.CssClass = "Btn";
            btn.ID = "Btn_Save";
            btn.Text = " Save ";
            btn.Click += new EventHandler(btn_Click);
            this.Pub1.AddTD(btn);
            this.Pub1.AddTREnd();
            this.Pub1.AddTableEnd();
        }

        void btn_Click(object sender, EventArgs e)
        {
            Nodes nds = new Nodes(this.FK_Flow);
            BP.WF.Node mynd = new BP.WF.Node(this.FK_Node);
            string strs = "@";
            foreach (BP.WF.Node nd in nds)
            {
                CheckBox cb = this.Pub1.GetCBByID("CB_" + nd.NodeID);
                if (cb.Checked == false)
                    continue;

                strs += nd.NodeID + "@";
            }


            if (mynd.ShowSheets == strs)
                return;

            mynd.ShowSheets = strs;
            mynd.Update();
            //   Alert(strs);
            // Alert(" Saved successfully .");
        }
        public void BindCond()
        {

        }
    }
}
