using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.En;
using BP.DA;
using BP.Demo;
using System.Collections;


namespace CCFlow.SDKFlowDemo.TeleCom._2DiShi
{
    public partial class CallSubFlow : System.Web.UI.Page
    {
        #region  Variable process engine came .
        /// <summary>
        ///  Parent process ID
        /// </summary>
        public Int64 ParentWorkID
        {
            get
            {
                return Int64.Parse(this.Request.QueryString["ParentWorkID"]);
            }
        }
        #endregion  Variable process engine came 

        protected void Page_Load(object sender, EventArgs e)
        {
            int shebeiID = int.Parse(this.Request.QueryString["SheBeiID"]);
            /* If it is being sent over .*/
            BP.Demo.tab_wf_commonkpioptivalue en = new BP.Demo.tab_wf_commonkpioptivalue();
            en.OID = shebeiID;
            en.Retrieve(); // According Equipment ID  Check out the information on the device .
            if (en.WorkID != 0)
                throw new Exception("@ This process has already started .");
        }
        protected void Btn_Send_Click(object sender, EventArgs e)
        {
            //step1:  Lift the sub-sub-processes .
            Int64 workid  = BP.WF.Dev2Interface.Node_CreateBlankWork("027", null, null,BP.Web.WebUser.No, 
                " Automatic call "+BP.Web.WebUser.Name,
                this.ParentWorkID,"026",0,null);

            //step2:  Send perform sub-sub-processes .
            Hashtable ht = new Hashtable();
            if (this.CB_IsShiShi.Checked)
                ht.Add("IsShiShi", 1);
            else
                ht.Add("IsShiShi", 0);
            ht.Add("OID", workid);

            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork("027", workid, ht, null,0, this.TB_FZR.Text);

            //step3:  Update Device WorkID. 
            int shebeiID = int.Parse(this.Request.QueryString["SheBeiID"]);
            BP.Demo.tab_wf_commonkpioptivalue en = new BP.Demo.tab_wf_commonkpioptivalue();
            en.OID = shebeiID;
            en.Retrieve(); // According Equipment ID  Check out the information on the device 
            en.WorkID = (int)objs.VarWorkID; 
            // Returns the object from inside , Get it sub-sub-processes WorkID, Thus marking up the device starts Workid, And take advantage of this workid State associated with this process .
            en.Update();

             //step4:  Tips to send a message .
            this.Session["info"] = objs.ToMsgOfHtml();
            this.Response.Redirect("ShowMsg.aspx?ss" + BP.DA.DataType.CurrentDataTime, true);
        }
    }
}