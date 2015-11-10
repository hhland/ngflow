using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.En;
using BP.DA;
using BP.Demo;
using BP.Web;
using BP.Port;

namespace CCFlow.SDKFlowDemo.TeleCom._3SheBei
{
    public partial class S1_WangLuoFenXi : System.Web.UI.Page
    {
        #region  Variable process engine came .
        /// <summary>
        ///  The work ID, In establishing the draft has produced .
        /// </summary>
        public Int64 WorkID
        {
            get
            {
                return Int64.Parse(this.Request.QueryString["WorkID"]);
            }
        }
        /// <summary>
        ///  Process ID
        /// </summary>
        public Int64 FID
        {
            get
            {
                return Int64.Parse(this.Request.QueryString["FID"]);
            }
        }
        /// <summary>
        ///   Process ID .
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        /// <summary>
        ///  The current node ID
        /// </summary>
        public int FK_Node
        {
            get
            {
                return int.Parse(this.Request.QueryString["FK_Node"]);
            }
        }
        #endregion  Variable process engine came 

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.ID)
            {
                case "Btn_Send":
                    this.Send();
                    break;
                case "Btn_Save":
                    this.Save();
                    break;
                case "Btn_Return":
                    BP.WF.Dev2Interface.UI_Window_Return(this.FK_Flow, this.FK_Node, this.WorkID, this.FID);
                    break;
                case "Btn_Track":
                 //   BP.WF.Dev2Interface.UI_Window_FlowChart(this.FK_Flow, this.FK_Node, this.WorkID, this.FID);
                    break;
                default:
                    throw new Exception("@ Business logic is not implemented " + btn.Text);
            }
        }
        /// <summary>
        ///  Send 
        /// </summary>
        public void Send()
        {

            Hashtable ht = new Hashtable();
            if (this.CB_IsShiShi.Checked)
                ht.Add("IsShiShi", 1);
            else
                ht.Add("IsShiShi", 0);

            // call  Sub-sub-processes , After performing transmission and returns an object gets sent .
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(this.FK_Flow,this.WorkID, ht, null, 0,this.TB_FZR.Text);

            //int shebeiID = int.Parse(this.Request.QueryString["SheBeiID"]);
            //BP.Demo.tab_wf_commonkpioptivalue en = new BP.Demo.tab_wf_commonkpioptivalue();
            //en.OID = shebeiID;
            //en.Retrieve(); // According Equipment ID  Check out the information on the device 
            //en.WorkID = (int)objs.VarWorkID; // Returns the object from inside , Get it sub-sub-processes WorkID.
            //en.Update();

            // Tips to send a message .
            this.Session["info"] = objs.ToMsgOfHtml();
            this.Response.Redirect("ShowMsg.aspx?ss" + BP.DA.DataType.CurrentDataTime, true);
        }
        /// <summary>
        ///  Save 
        /// </summary>
        public void Save()
        {
        }
    }
}