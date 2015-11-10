using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;

namespace CCFlow.WF.Admin.WatchOneFlow
{
    public partial class FlowSkip : System.Web.UI.Page
    {
        #region  Property 
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
        #endregion  Property 

        protected void Page_Load(object sender, EventArgs e)
        {
            Nodes nds = new Nodes(this.FK_Flow);
            foreach (BP.WF.Node nd in nds)
            {
                this.DDL_SkipToNode.Items.Add(new ListItem(" Step :" + nd.Step + " Name :" + nd.Name,
                    nd.NodeID.ToString()));
            }
        }

        protected void Btn_OK_Click(object sender, EventArgs e)
        {
            string str = this.TB_SkipToEmp.Text.Trim();
            BP.WF.Port.Emp emp = new BP.WF.Port.Emp();
            emp.No = str;
            if (emp.RetrieveFromDBSources() == 0)
            {
                BP.Sys.PubClass.Alert(" Enter the number of personnel error :" + str);
                return;
            }

            string note = this.TB_Note.Text.Trim();
            BP.WF.Node nd=new Node( int.Parse( this.DDL_SkipToNode.SelectedValue));

            // Performing transmission .
            BP.WF.Dev2Interface.Node_SendWork(this.FK_Flow,this.WorkID,null,null,nd.NodeID,emp.No);

            //  Prompt .
            BP.Sys.PubClass.Alert(" Jump to have successfully :" + str+", Go to :"+nd.Name);
            BP.Sys.PubClass.WinClose("ss");
        }

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            BP.Sys.PubClass.WinClose();
        }
    }
}