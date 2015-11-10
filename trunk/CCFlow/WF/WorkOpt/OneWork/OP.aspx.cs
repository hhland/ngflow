using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.DA;
using BP.Web;
using BP.Port;
using BP.WF;
namespace CCFlow.WF.OneWork
{
    public partial class WF_WorkOpt_OneWork_OP : BP.Web.WebPage
    {
        #region  Property 

        public new string DoType
        {
            get
            {
                return this.Request.QueryString["DoType"];
            }
        }

        public string FK_Node
        {
            get
            {
                return this.Request.QueryString["FK_Node"];
            }
        }

        public string FK_Flow
        {
            get
            {
                string flow = this.Request.QueryString["FK_Flow"];
                if (flow == null)
                {
                    throw new Exception("@ Did not get its process ID .");
                }
                else
                {
                    return flow;
                }
            }
        }

        public Int64 WorkID
        {
            get
            {
                return Int64.Parse(this.Request.QueryString["WorkID"]);
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region  Function execution 
            try
            {
                switch (this.DoType)
                {
                    case "Del":
                        WorkFlow wf = new WorkFlow(FK_Flow, WorkID);
                        wf.DoDeleteWorkFlowByReal(true);
                        this.WinCloseWithMsg(" Process has been deleted .");
                        break;
                    case "HungUp":
                        WorkFlow wf1 = new WorkFlow(FK_Flow, WorkID);
                        //wf1.DoHungUp(HungUpWa;
                        this.WinCloseWithMsg(" The process has been suspended .");
                        break;
                    case "UnHungUp":
                        WorkFlow wf2 = new WorkFlow(FK_Flow, WorkID);
                        //  wf2.DoUnHungUp();
                        this.WinCloseWithMsg(" The process has been lifted pending .");
                        break;
                    case "ComeBack":
                        WorkFlow wf3 = new WorkFlow(FK_Flow, WorkID);
                        wf3.DoComeBackWorkFlow("None");
                        this.WinCloseWithMsg(" The process has been enabled reply .");
                        break;
                    case "Takeback": /* Retrieve approval .*/
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                this.Alert(" Executive Function :" + DoType + ", Error :" + ex.Message);
            }
            #endregion

            int wfState = BP.DA.DBAccess.RunSQLReturnValInt("SELECT WFState FROM WF_GenerWorkFlow WHERE WorkID=" + WorkID, 1);
            WFState wfstateEnum = (WFState)wfState;
            //this.Pub2.AddH3(" You can do <hr>");
            switch (wfstateEnum)
            {
                case WFState.Runing: /*  Runtime */
                    this.FlowOverByCoercion(); /* Delete Process .*/
                    this.TackBackCheck(); /* Retrieve approval */
                    this.Hurry(); /* Reminders */
                    this.UnSend(); /* Send revocation */
                    break;
                case WFState.Complete: //  Carry out .
                case WFState.Delete: //  Tombstone ..
                    this.RollBack(); /* Reinstated process */
                    break;
                case WFState.HungUp: //  Pending .
                    this.AddUnHungUp(); /* Pending revocation */
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        ///  Retrieve approval 
        /// </summary>
        public void TackBackCheck()
        {
            GenerWorkFlow gwf = new GenerWorkFlow(this.WorkID);
            /*  Determine whether to retrieve the approval authority .*/
            this.Pub2.AddEasyUiPanelInfoBegin(GetGlobalResourceTitle("RetrieveApproval.Text"));
            string sql = "SELECT NodeID FROM WF_Node WHERE CheckNodes LIKE '%" + gwf.FK_Node + "%'";
            int myNode = DBAccess.RunSQLReturnValInt(sql, 0);
            if (myNode != 0)
            {
                GetTask gt = new GetTask(myNode);
                if (gt.Can_I_Do_It() == true)
                {
                    this.Pub2.Add(GetGlobalResourceLabel("FunctionExecution.Text") +"  :<a href=\"javascript:Takeback('" + WorkID + "','" + FK_Flow + "','" + gwf.FK_Node + "','" + myNode + "')\" > "+GetGlobalResourceLink("ClickToRetrieveTheApprovalProcessExecution.Text")+" </a>.");
                    this.Pub2.AddBR(GetGlobalResourceLabel("Explanation.Text") + " : "+GetGlobalResourceMsg("SUCRETSENTWORK.Text"));
                }
            }
            else
            {
                this.Pub2.Add(GetGlobalResourceMsg("NOPER.Text"));
            }
            this.Pub2.AddEasyUiPanelInfoEnd();
            Pub2.AddBR();
        }
        /// <summary>
        ///  Forced to delete process 
        /// </summary>
        public void FlowOverByCoercion()
        {
            GenerWorkFlow gwf = new GenerWorkFlow(WorkID);
            this.Pub2.AddEasyUiPanelInfoBegin(GetGlobalResourceTitle("DeleteProcess.Text"));
            if (WebUser.No == "admin")
            {
                this.Pub2.Add(GetGlobalResourceLabel("FunctionExecution.Text") + "  :<a href=\"javascript:DoFunc('" + FlowOpList.FlowOverByCoercion + "','" + WorkID + "','" + FK_Flow + "','" + FK_Node + "')\" > "+GetGlobalResourceLink("ClickTheDeleteProcess.Text")+" </a>.");
                this.Pub2.AddBR(GetGlobalResourceLabel("Explanation.Text") + " : "+GetGlobalResourceMsg("EXPRPCDEL.Text"));
            }
            else
            {
                this.Pub2.Add(GetGlobalResourceMsg("OADPNOPER.Text"));
            }
            this.Pub2.AddEasyUiPanelInfoEnd();
            Pub2.AddBR();
        }
        /// <summary>
        ///  Reminders 
        /// </summary>
        public void Hurry()
        {
            /* Reminders */
            this.Pub2.AddEasyUiPanelInfoBegin(GetGlobalResourceTitle("WorkReminders.Text"));
            this.Pub2.Add(GetGlobalResourceMsg("NOPER.Text"));
            this.Pub2.AddEasyUiPanelInfoEnd();
            Pub2.AddBR();
        }
        /// <summary>
        ///  Send revocation 
        /// </summary>
        public void UnSend()
        {
            /* Send revocation */
            this.Pub2.AddEasyUiPanelInfoBegin(GetGlobalResourceTitle("SendRevocation.Text"));
            this.Pub2.Add(GetGlobalResourceMsg("NOPER.Text"));
            this.Pub2.AddEasyUiPanelInfoEnd();
            Pub2.AddBR();
        }
        /// <summary>
        ///  Enabling data recovery process to the end node 
        /// </summary>
        public void RollBack()
        {
            this.Pub2.AddEasyUiPanelInfoBegin(GetGlobalResourceTitle("EnablingDataRecoveryProcessToTheEndNode.Text"));
            if (WebUser.No == "admin")
            {
                this.Pub2.Add(GetGlobalResourceLabel("FunctionExecution.Text") + "  :<a href=\"javascript:DoFunc('ComeBack','" + WorkID + "','" + FK_Flow + "','" + FK_Node + "')\" > "+GetGlobalResourceMsg("ClickToPerformTheRecoveryProcess.Text")+" </a>.");
                this.Pub2.AddBR(GetGlobalResourceLabel("Explanation.Text") + " : " + GetGlobalResourceMsg("SUCRECSENTWORKTOLASTSTAFF.Text"));
            }
            else
            {
                this.Pub2.Add(GetGlobalResourceMsg("NOPER.Text"));
            }
            this.Pub2.AddEasyUiPanelInfoEnd();
            Pub2.AddBR();
        }
        /// <summary>
        ///  Unsuspend 
        /// </summary>
        public void AddUnHungUp()
        {
            this.Pub2.AddEasyUiPanelInfoBegin(GetGlobalResourceTitle("Unsuspend.Text"));
            if (BP.WF.Dev2Interface.Flow_IsCanDoCurrentWork(FK_Flow, int.Parse(FK_Node), WorkID, WebUser.No))
            {
                this.Pub2.Add(GetGlobalResourceLabel("FunctionExecution.Text") + "  :<a href=\"javascript:DoFunc('UnHungUp','" + WorkID + "','" + FK_Flow + "','" + FK_Node + "')\" > "+GetGlobalResourceMsg("ClickToCancelThePendingExecutionProcess.Text")+" </a>.");
                this.Pub2.AddBR(GetGlobalResourceLabel("Explanation.Text") + " : " + GetGlobalResourceMsg("LPOCPS.Text"));
            }
            else
            {
                this.Pub2.AddBR(GetGlobalResourceMsg("NOPERORNOSUST.Text"));
            }
            this.Pub2.AddEasyUiPanelInfoEnd();
            Pub2.AddBR();
        }
        /// <summary>
        ///  Pending 
        /// </summary>
        public void AddHungUp()
        {
            this.Pub2.AddEasyUiPanelInfoBegin(GetGlobalResourceTitle("Pending.Text"));
            if (BP.WF.Dev2Interface.Flow_IsCanDoCurrentWork(FK_Flow, int.Parse(FK_Node), WorkID, WebUser.No))
            {
                this.Pub2.Add(GetGlobalResourceLabel("FunctionExecution.Text") + "  :<a href=\"javascript:DoFunc('" + FlowOpList.HungUp + "','" + WorkID + "','" + FK_Flow + "','" + FK_Node + "','')\" > "+GetGlobalResourceLink("ClickPendingExecutionProcess.Text")+"</a>.");
                this.Pub2.AddBR(GetGlobalResourceLabel("Explanation.Text") + " : " + GetGlobalResourceMsg("SUPPOCEXETIMENOTPEND.Text"));
            }
            else
            {
                this.Pub2.Add(GetGlobalResourceMsg("NOPER.Text"));
            }
            this.Pub2.AddEasyUiPanelInfoEnd();
            Pub2.AddBR();
        }
        /// <summary>
        ///  Transfer 
        /// </summary>
        public void AddShift()
        {
            this.Pub2.AddEasyUiPanelInfoBegin(GetGlobalResourceTitle("Transfer.Text"));
            if (BP.WF.Dev2Interface.Flow_IsCanDoCurrentWork(FK_Flow, int.Parse(FK_Node), WorkID, WebUser.No))
            {
                this.Pub2.Add(GetGlobalResourceLabel("FunctionExecution.Text") + "  :<a href=\"javascript:DoFunc('" + FlowOpList.UnHungUp + "','" + WorkID + "','" + FK_Flow + "','" + FK_Node + "')\" > " + GetGlobalResourceLink("ClickToCancelThePendingExecutionProcess.Text") + " </a>.");
                this.Pub2.AddBR(GetGlobalResourceLabel("Explanation.Text") + "  : " + GetGlobalResourceMsg("LPOCPS.Text"));
            }
            else
            {
                this.Pub2.AddBR(GetGlobalResourceMsg("NOPERORNOSUST.Text"));
            }
            this.Pub2.AddEasyUiPanelInfoEnd();
            Pub2.AddBR();
        }

        public void AddShiftByCoercion()
        {
            this.Pub2.AddEasyUiPanelInfoBegin(GetGlobalResourceTitle("ForcedTransfer.Text"));
            if (WebUser.No == "admin")
            {
                this.Pub2.Add(GetGlobalResourceLabel("FunctionExecution.Text") + "  :<a href=\"javascript:DoFunc('" + FlowOpList.ShiftByCoercion + "','" + WorkID + "','" + FK_Flow + "','" + FK_Node + "')\" > "+GetGlobalResourceLink("ClickToCancelThePendingExecutionProcess.Text")+" </a>.");
                this.Pub2.AddBR(GetGlobalResourceLabel("Explanation.Text") + "  : " + GetGlobalResourceMsg("LPOCPS.Text"));
            }
            else
            {
                this.Pub2.AddBR(GetGlobalResourceMsg("NOPER.Text"));
            }
            this.Pub2.AddEasyUiPanelInfoEnd();
            Pub2.AddBR();
        }


        protected override void InitializeCulture()
        {
            base.InitializeCulture();
            object _culture = Session["culture"];
            string culture = _culture == null ? "en-us" : _culture.ToString();
            System.Threading.Thread.CurrentThread.CurrentCulture =
            System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(culture);
        }

        protected string GetGlobalResourceMenu(string key)
        {
            return GetGlobalResourceObject("Menu", key).ToString();
        }

        protected string GetGlobalResourceTitle(string key)
        {
            return GetGlobalResourceObject("Title", key).ToString();
        }

        protected string GetGlobalResourceButton(string key)
        {
            return GetGlobalResourceObject("Button", key).ToString();
        }

        protected string GetGlobalResourceLink(string key)
        {
            return GetGlobalResourceObject("Link", key).ToString();
        }

        protected string GetGlobalResourceLabel(string key)
        {
            return GetGlobalResourceObject("Label", key).ToString();
        }

        protected string GetGlobalResourceMsg(string key)
        {
            return GetGlobalResourceObject("Msg", key).ToString();
        }

    }
}