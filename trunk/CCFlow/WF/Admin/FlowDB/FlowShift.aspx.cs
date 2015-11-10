using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCFlow.WF.Admin.WatchOneFlow
{
    public partial class FlowShift : System.Web.UI.Page
    {
        #region  Property 
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
                return Int64.Parse( this.Request.QueryString["WorkID"]);
            }
        }
        #endregion  Property 

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void Btn_OK_Click(object sender, EventArgs e)
        {
            string str = this.TB_Emp.Text.Trim();
            BP.WF.Port.Emp emp = new BP.WF.Port.Emp();
            emp.No = str;
            if (emp.RetrieveFromDBSources() == 0)
            {
                BP.Sys.PubClass.Alert(" Enter the number of personnel error :"+str);
                return;
            }

            string note = this.TB_Note.Text.Trim();

            // Performing handover .
            BP.WF.Dev2Interface.Node_Shift(this.FK_Flow,this.FK_Node, this.WorkID, this.FID, emp.No, note);

            //  Prompt .
            BP.Sys.PubClass.Alert(" Has been successfully transferred to the :" + str);
            BP.Sys.PubClass.WinClose("ss");
        }

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            BP.Sys.PubClass.WinClose();
        }
    }
}