using System;
using System.Collections.Generic;
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
                        wf3.DoComeBackWorkFlow("无");
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
            this.Pub2.AddEasyUiPanelInfoBegin(" Retrieve approval ");
            string sql = "SELECT NodeID FROM WF_Node WHERE CheckNodes LIKE '%" + gwf.FK_Node + "%'";
            int myNode = DBAccess.RunSQLReturnValInt(sql, 0);
            if (myNode != 0)
            {
                GetTask gt = new GetTask(myNode);
                if (gt.Can_I_Do_It() == true)
                {
                    this.Pub2.Add(" Function execution :<a href=\"javascript:Takeback('" + WorkID + "','" + FK_Flow + "','" + gwf.FK_Node + "','" + myNode + "')\" > Click to retrieve the approval process execution </a>.");
                    this.Pub2.AddBR(" Explanation : If successfully retrieved ,ccflow Will be sent to stay in the work of others working nodes to your to-do list .");
                }
            }
            else
            {
                this.Pub2.Add(" You do not have this permission .");
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
            this.Pub2.AddEasyUiPanelInfoBegin(" Delete Process ");
            if (WebUser.No == "admin")
            {
                this.Pub2.Add(" Function execution :<a href=\"javascript:DoFunc('" + FlowOpList.FlowOverByCoercion + "','" + WorkID + "','" + FK_Flow + "','" + FK_Node + "')\" > Click the delete process </a>.");
                this.Pub2.AddBR(" Explanation : If the execution process will be completely deleted .");
            }
            else
            {
                this.Pub2.Add(" Only admin You can delete process , You do not have this permission .");
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
            this.Pub2.AddEasyUiPanelInfoBegin(" Work reminders ");
            this.Pub2.Add(" You do not have this permission .");
            this.Pub2.AddEasyUiPanelInfoEnd();
            Pub2.AddBR();
        }
        /// <summary>
        ///  Send revocation 
        /// </summary>
        public void UnSend()
        {
            /* Send revocation */
            this.Pub2.AddEasyUiPanelInfoBegin(" Send revocation ");
            this.Pub2.Add(" You do not have this permission .");
            this.Pub2.AddEasyUiPanelInfoEnd();
            Pub2.AddBR();
        }
        /// <summary>
        ///  Enabling data recovery process to the end node 
        /// </summary>
        public void RollBack()
        {
            this.Pub2.AddEasyUiPanelInfoBegin(" Enabling data recovery process to the end node ");
            if (WebUser.No == "admin")
            {
                this.Pub2.Add(" Function execution :<a href=\"javascript:DoFunc('ComeBack','" + WorkID + "','" + FK_Flow + "','" + FK_Node + "')\" > Click to perform the recovery process </a>.");
                this.Pub2.AddBR(" Explanation : If successful recovery ,ccflow Upcoming work will be sent to the last one ending process staff .");
            }
            else
            {
                this.Pub2.Add(" You do not have permission .");
            }
            this.Pub2.AddEasyUiPanelInfoEnd();
            Pub2.AddBR();
        }
        /// <summary>
        ///  Unsuspend 
        /// </summary>
        public void AddUnHungUp()
        {
            this.Pub2.AddEasyUiPanelInfoBegin(" Unsuspend ");
            if (BP.WF.Dev2Interface.Flow_IsCanDoCurrentWork(FK_Flow, int.Parse(FK_Node), WorkID, WebUser.No))
            {
                this.Pub2.Add(" Function execution :<a href=\"javascript:DoFunc('UnHungUp','" + WorkID + "','" + FK_Flow + "','" + FK_Node + "')\" > Click to cancel the pending execution process </a>.");
                this.Pub2.AddBR(" Explanation : Lifting process pending state .");
            }
            else
            {
                this.Pub2.AddBR(" You do not have this permission , Or not currently suspended state .");
            }
            this.Pub2.AddEasyUiPanelInfoEnd();
            Pub2.AddBR();
        }
        /// <summary>
        ///  Pending 
        /// </summary>
        public void AddHungUp()
        {
            this.Pub2.AddEasyUiPanelInfoBegin(" Pending ");
            if (BP.WF.Dev2Interface.Flow_IsCanDoCurrentWork(FK_Flow, int.Parse(FK_Node), WorkID, WebUser.No))
            {
                this.Pub2.Add(" Function execution :<a href=\"javascript:DoFunc('" + FlowOpList.HungUp + "','" + WorkID + "','" + FK_Flow + "','" + FK_Node + "','')\" > Click pending execution process </a>.");
                this.Pub2.AddBR(" Explanation : Suspend the process execution , Can be lifted after suspending pending , Time does not count pending examination .");
            }
            else
            {
                this.Pub2.Add(" You do not have this permission .");
            }
            this.Pub2.AddEasyUiPanelInfoEnd();
            Pub2.AddBR();
        }
        /// <summary>
        ///  Transfer 
        /// </summary>
        public void AddShift()
        {
            this.Pub2.AddEasyUiPanelInfoBegin(" Transfer ");
            if (BP.WF.Dev2Interface.Flow_IsCanDoCurrentWork(FK_Flow, int.Parse(FK_Node), WorkID, WebUser.No))
            {
                this.Pub2.Add(" Function execution :<a href=\"javascript:DoFunc('" + FlowOpList.UnHungUp + "','" + WorkID + "','" + FK_Flow + "','" + FK_Node + "')\" > Click to cancel the pending execution process </a>.");
                this.Pub2.AddBR(" Explanation : Lifting process pending state .");
            }
            else
            {
                this.Pub2.AddBR(" You do not have this permission , Or not currently suspended state .");
            }
            this.Pub2.AddEasyUiPanelInfoEnd();
            Pub2.AddBR();
        }

        public void AddShiftByCoercion()
        {
            this.Pub2.AddEasyUiPanelInfoBegin(" Forced transfer ");
            if (WebUser.No == "admin")
            {
                this.Pub2.Add(" Function execution :<a href=\"javascript:DoFunc('" + FlowOpList.ShiftByCoercion + "','" + WorkID + "','" + FK_Flow + "','" + FK_Node + "')\" > Click to cancel the pending execution process </a>.");
                this.Pub2.AddBR(" Explanation : Lifting process pending state .");
            }
            else
            {
                this.Pub2.AddBR(" You do not have this permission .");
            }
            this.Pub2.AddEasyUiPanelInfoEnd();
            Pub2.AddBR();
        }
    }
}