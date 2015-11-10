using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BP.WF;
using BP.Port;

namespace BP.Web.WF
{
	/// <summary> 
	/// StopWorkFlow  The summary .
	/// </summary>
	public partial class StopWorkFlow : PageBase
	{
		#region  Controls 
		#endregion

		#region  Variable 
		/// <summary>
		///  The work ID
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
		public string  FlowNo
		{
			get
			{
				return  this.Request.QueryString["FK_Flow"]; 
			}
		}
		#endregion

		#region contral 
		public BP.Web.Controls.ToolbarBtn Btn_StopWorkFlow
		{
			get
			{
				return this.BPToolBar1.GetBtnByKey("Btn_StopWorkFlow");
			}
		}
		public BP.Web.Controls.ToolbarBtn Btn_ComeBackFlow
		{
			get
			{
				return this.BPToolBar1.GetBtnByKey("Btn_ComeBackFlow");
			}
		}
		public BP.Web.Controls.ToolbarBtn Btn_DeleteFlowByFlag
		{
			get
			{
				return this.BPToolBar1.GetBtnByKey("Btn_DeleteFlowByFlag");
			}
		}
		public BP.Web.Controls.ToolbarBtn Btn_DeleteWFByRealReal
		{
			get
			{
				return this.BPToolBar1.GetBtnByKey("Btn_DeleteWFByRealReal");
			}
		}
		
		/// <summary>
		/// Btn_Cancel
		/// </summary>
		public BP.Web.Controls.ToolbarBtn Btn_Cancel
		{
			get
			{
				return this.BPToolBar1.GetBtnByKey("Btn_Cancel");
			}
		}
		#endregion 

        private void SetState()
        {
            try
            {
                this.Btn_ComeBackFlow.Enabled = false;
                this.Btn_DeleteFlowByFlag.Enabled = false;
                this.Btn_DeleteWFByRealReal.Enabled = false;
                this.Btn_StopWorkFlow.Enabled = false;
                Int64 workId = Int64.Parse(this.Request.QueryString["WorkID"]);
                //int nodeId=int.Parse(this.Request.QueryString["WorkID"]);
                string flowNo = this.Request.QueryString["FK_Flow"];
                if (workId == 0)
                {
                    this.Alert("@ You have no selection process , Invalid operation .", false);
                    this.WinClose();
                    return;
                }
                WorkFlow wf = new WorkFlow(new Flow(flowNo), workId);
                if (wf.IsComplete)
                {
                    this.Alert("@ The process has been completed , Invalid operation .", false);
                    this.WinClose();
                    return;
                }
                GenerWorkFlow gwf = new GenerWorkFlow();
                //workId,flowNo
                gwf.WorkID = workId;
                gwf.FK_Flow = flowNo;
                if (gwf.IsExits == false)
                {
                    wf.DoDeleteWorkFlowByReal(true);
                    throw new Exception(" System error , Please contact your administrator : Error reason is that the current process [" + flowNo + " id=" + workId + "], Not completed , However, this process does not exist table this information , This process has become ineffective process , Information may be tested , The system has to remove it .");
                }
                else
                {
                    gwf.Retrieve();
                }

                if (gwf.WFState == WFState.Complete )
                {
                    this.Alert("@ The process has been completed , This action can not be .", false);
                    this.WinClose();
                    return;
                }
                else if (gwf.WFState == 0)
                {
                    this.Btn_DeleteFlowByFlag.Enabled = true;
                    this.Btn_StopWorkFlow.Enabled = true;
                }
                else
                {
                    throw new Exception("error ");
                }
                this.Label1.Text = " The current state of the process :" + gwf.WFState;

                Flow fl = new Flow(gwf.FK_Flow);
                // Display log information 		
                StartWork sw = (StartWork)fl.HisStartNode.HisWork;
                sw.OID = workId;
                if (sw.IsExits == false)
                {
                    gwf.Delete();
                    throw new Exception("@ Start node is physically deleted . Process Error ,  This entry process has failed ,  Please close the window to return to the system , Refresh Record .");
                }
                sw.Retrieve();
                

                //  Determine the process can not be deleted permissions .

                /* 
                 *  In the case of   4  Tax approval process . 
                 *  Let collection department to handle ,
                 *  Otherwise let mdg  Deal with .
                 * */
                if (fl.FK_FlowSort == "4")
                {
                    /*  Law Section levy  */
                    if (WebUser.FK_Dept == "000003")
                    {
                        this.Btn_DeleteWFByRealReal.Enabled = true;
                    }
                }
                else
                {
                    if (WebUser.FK_Dept == "000001")
                    {
                        this.Btn_DeleteWFByRealReal.Enabled = true;
                    }
                }
                //this.UCWFRpt1.BindDataV2(wf);
            }
            catch (Exception ex)
            {
                this.Alert(ex);
                this.WinClose();
            }
        }
		/// <summary>
		/// ss
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.BPToolBar1.ButtonClick += new System.EventHandler(this.Btn_Click_Click);
			if (this.IsPostBack==false)
			{
				this.BPToolBar1.AddBtn("Btn_StopWorkFlow"," Forcibly aborted ");
				this.BPToolBar1.AddBtn("Btn_ComeBackFlow"," Reinstated ");
				this.BPToolBar1.AddBtn("Btn_DeleteFlowByFlag"," Tombstone ");
				this.BPToolBar1.AddBtn("Btn_DeleteWFByRealReal"," Physically removed ");
				this.BPToolBar1.AddBtn(BP.Web.Controls.NamesOfBtn.Close);
				this.SetState();
			}
		}

		#region Web  Form Designer generated code 
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN:  This call is  ASP.NET Web  Form Designer required .
			//
			InitializeComponent();
			base.OnInit(e);
		}		
		/// <summary>
		///  Required method for Designer support  -  Do not use the code editor to modify 
		///  Contents of this method .
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion
 		 
		private void Btn_Click_Click(object sender, System.EventArgs e)
		{
			string msg="" ; 
			try
			{
                Int64 workId = Int64.Parse(this.Request.QueryString["WorkID"]);
				GenerWorkFlow gwf = new GenerWorkFlow(workId);
				WorkFlow wf = new WorkFlow(new Flow(gwf.FK_Flow), workId);
				BP.Web.Controls.ToolbarBtn  btn = (BP.Web.Controls.ToolbarBtn)sender;
				string title, docs;
				switch(btn.ID)
				{
					case "Btn_StopWorkFlow":
						msg="@ Termination process error .:";
					//	wf.DoStopWorkFlow(this.TB1.Text);
						//  Send a message to the relevant personnel .
						title=" Forced termination ["+gwf.Title+"] Process notification ";
						docs=this.TB1.Text;
						//WFPubClass.SendMsg( new WorkNodes(gwf.HisFlow,workId),title,docs);

						this.ResponseWriteBlueMsg("@ Forced to terminate the process successfully . And send system messages to the relevant personnel on the process ."); 
						break;
					case "Btn_DeleteFlowByFlag":
						msg="@ Tombstone process error .:";
//						wf.DoDeleteWorkFlowByFlag(this.TB1.Text);
						//  Send a message to the relevant personnel .
						title=" Tombstone ["+gwf.Title+"] Process notification ";
						docs=this.TB1.Text;
					//	WFPubClass.SendMsg( new WorkNodes(gwf.HisFlow,workId),title,docs);
						this.ResponseWriteBlueMsg("@ Tombstone process successful , And send system messages to the relevant personnel on the process .");
						break;
					case "Btn_ComeBackFlow":
						msg="@ Use the error recovery process .:";
                        wf.DoComeBackWorkFlow(this.TB1.Text); 

						//  Send a message to the relevant personnel .
						title=" Reinstated process ["+gwf.Title+"] Process notification ";
						docs=this.TB1.Text;						
						//WFPubClass.SendMsg( new WorkNodes(gwf.HisFlow,workId),title,docs);
						this.ResponseWriteBlueMsg("@ Recovery process successfully use , And send system messages to the relevant personnel on the process ");
						break;
					case "Btn_DeleteWFByRealReal":
						msg="@ Physically remove the process error .:";
                        wf.DoDeleteWorkFlowByReal(true);
						//  Send a message to the relevant personnel .
						title=" Physically removed ["+gwf.Title+"] Process notification ";
						docs=this.TB1.Text;						
						 
						 
						this.ResponseWriteBlueMsg("@ Physical processes successfully deleted ...");
						this.WinClose();
						break;
					case "Btn_Close":
						this.WinClose();
						return;					 
					default:
						break;
				}
				this.SetState();				
			}
			catch(Exception ex)
			{
				BP.DA.Log.DefaultLogWriteLine(BP.DA.LogType.Error,msg+ex.Message) ; 
				this.ResponseWriteRedMsg(msg+ex.Message) ; 
			}		
		}
	}
}
