using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.Port;
using BP.Sys;
using BP.Web.Controls;
using BP.DA;
using BP.En;
using BP.Web;

namespace CCFlow.WF.UC
{
    public partial class FHLFlow : BP.Web.UC.UCBase3
    {
        #region  Parameters .
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        public string FK_Node
        {
            get
            {
                return this.Request.QueryString["FK_Node"];
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
        public Button Btn_Return
        {
            get
            {
                return this.ToolBar1.GetBtnByID("Btn_Return");
            }
        }
        public Button Btn_Del
        {
            get
            {
                return this.ToolBar1.GetBtnByID("Btn_Del");
            }
        }
        public Button Btn_Close
        {
            get
            {
                return this.ToolBar1.GetBtnByID("Btn_Close");
            }
        }
        #endregion  Parameters .

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = " Working deal ";
            
            //  Return process .
            this.ToolBar1.Add("<input class=Btn type=button onclick=\"javascript:window.location.href='./WorkOpt/ReturnWork.aspx?WorkID=" + this.WorkID + "&FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.FK_Node + "&FID=" + this.FID + "'\" value=' Return ' >");
             
            //  Shut down .
            this.ToolBar1.AddBtn("Btn_Close", " Shut down ");
            this.Btn_Close.OnClientClick = "window.close();";

            //this.Btn_Del.OnClientClick = "return confirm('are you sure?')";
            //this.Btn_Del.Click += new EventHandler(Btn_Del_Click);

            GenerWorkFlow gwf = new GenerWorkFlow(this.FID);
            WorkFlow wf = new WorkFlow(this.FK_Flow, this.FID);
            WorkNode wn = new WorkNode(this.FID, gwf.FK_Node);
            WorkNode wnPri = wn.GetPreviousWorkNode_FHL(this.WorkID); //  He on one node .
            BP.WF.Node ndPri = wnPri.HisNode;
            try
            {
                // Show different forms depending on the form type .
                if (ndPri.HisFormType == NodeFormType.FixForm)
                {
                    this.UCEn1.BindColumn4(wnPri.HisWork, "ND" + wnPri.HisNode.NodeID);
                    this.UCEn1.Add(wnPri.HisWork.WorkEndInfo);
                }
                else if (ndPri.HisFormType == NodeFormType.FreeForm)
                {
                    this.UCEn1.BindCCForm(wnPri.HisWork, "ND" + wnPri.HisNode.NodeID, true, 0);
                }
                else if (ndPri.HisFormType == NodeFormType.SDKForm)
                {
                    string url = "&FK_Flow=" + this.FK_Flow + "&FK_Node=" + ndPri.NodeID + "&WorkID=" + this.WorkID + "&FID=" + this.FID;

                    string src = ndPri.FormUrl;
                    if (src.Contains("?"))
                        src = src + "&IsReadonly=1&FK" + url;
                    else
                        src = src + "?1=2&IsReadonly=1" + url;
                    this.Add("<iframe ID='Ff' src='" + src + "' frameborder=0  style='width:100%; height:900px;text-align: left;'  leftMargin='0'  topMargin='0' scrolling=auto />");
                    this.Add("</iframe>");
                }
            }
            catch
            {
                this.WinCloseWithMsg(" This work has been terminated or deleted .");
            }
        }
        void Btn_Del_Click(object sender, EventArgs e)
        {
            try
            {
                WorkFlow wf14 = new WorkFlow(this.FK_Flow, this.WorkID);
                wf14.DoDeleteWorkFlowByReal(true);
                this.Alert(" Successful implementation .");
                this.WinClose();
            }
            catch (Exception ex)
            {
                this.WinCloseWithMsg(ex.Message);
            }
        }
    }
}