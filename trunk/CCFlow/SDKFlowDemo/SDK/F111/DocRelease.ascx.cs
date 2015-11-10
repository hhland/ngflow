using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCFlow.SDKFlowDemo.SDK.F111
{
    public partial class DocRelease : BP.Web.UC.UCBase3
    {
        #region  Public Methods 
        public void ToMsg(string msg)
        {
            System.Web.HttpContext.Current.Session["info"] = msg;
            System.Web.HttpContext.Current.Application["info" + BP.Web.WebUser.No] = msg;
            BP.WF.Glo.SessionMsg = msg;
            System.Web.HttpContext.Current.Response.Redirect(
                "/SDKFlowDemo/SDK/Info.aspx?FK_Flow=2&FK_Type=2&FK_Node=2&WorkID=22" + DateTime.Now.ToString(), false);
        }
        public void ToErrorPage(string msg)
        {
            System.Web.HttpContext.Current.Session["info"] = msg;
            System.Web.HttpContext.Current.Application["info" + BP.Web.WebUser.No] = msg;
            BP.WF.Glo.SessionMsg = msg;
            System.Web.HttpContext.Current.Response.Redirect("/SDKFlowDemo/SDK/ErrorPage.aspx", false);
        }
        #endregion  Public Methods s

        #region  Accept 4 Large parameter .
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
                return Int64.Parse(this.Request.QueryString["FID"]);
            }
        }
        #endregion 4 Large parameter .

        protected void Page_Load(object sender, EventArgs e)
        {
            BP.WF.Node nd = new BP.WF.Node(this.FK_Node);
            this.Page.Title = " Hello :" + BP.Web.WebUser.No + " - " + BP.Web.WebUser.Name + ". The current node :" + nd.Name;

            #region  Handling access control .
            if (this.FK_Node == 11101)
            {
                /* If this is the start node , Would not be allowed to return .*/
                this.Btn_Return.Enabled = false;

                // Endorsement nor can .
                this.Btn_AskForHelp.Enabled = false;
            }

            if (BP.WF.Dev2Interface.Flow_IsCanDoCurrentWork(FK_Flow,this.FK_Node, this.WorkID, BP.Web.WebUser.No) == false)
            {
                /* If the current staff can not handle the current work , Put all the button  Disabling swap */
                foreach (Control ctl in this.Controls)
                {
                    Button btn = ctl as Button;
                    if (btn != null)
                        btn.Enabled = false;
                }
            }
            #endregion  Handling access control .

        }

        #region  Execution flow control button function .
        protected void Btn_Send_Click(object sender, EventArgs e)
        {
            try
            {
                BP.WF.SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(this.FK_Flow, this.WorkID);
                this.Response.Write(objs.ToMsgOfHtml());
                this.ToMsg(objs.ToMsgOfHtml());
            }
            catch (Exception ex)
            {
                this.ToMsg(ex.Message);
            }
        }
        protected void Btn_Return_Click(object sender, EventArgs e)
        {
            BP.WF.Dev2Interface.UI_Window_Return(this.FK_Flow, this.FK_Node, this.WorkID, this.FID);
        }
        protected void Btn_Track_Click(object sender, EventArgs e)
        {
            BP.WF.Dev2Interface.UI_Window_OneWork(this.FK_Flow, this.WorkID, this.FID);
        }
        protected void Btn_AskForHelp_Click(object sender, EventArgs e)
        {
            BP.WF.Dev2Interface.UI_Window_AskForHelp(this.FK_Flow, this.FK_Node, this.WorkID, this.FID);
        }
        protected void Btn_CC_Click(object sender, EventArgs e)
        {
            BP.WF.Dev2Interface.UI_Window_CC(this.FK_Flow,this.FK_Node, this.WorkID, this.FID);
        }
        #endregion  Execution flow control button function .

    }
}