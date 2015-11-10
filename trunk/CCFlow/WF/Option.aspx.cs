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
using BP.Web.Controls;
using BP.WF;
using BP.DA;

namespace BP.Web.WF.WF
{
	/// <summary>
	/// Option  The summary .
	/// </summary>
	public partial class Option : BP.Web.WebPage
	{
		public ToolbarCheckBtnGroup ToolbarCheckBtnGroup1
		{
			get
			{
				return (ToolbarCheckBtnGroup)this.BPToolBar1.GetToolbarCheckBtnGroupByKey("ToolbarCheckBtnGroup1") ;
			}
		}
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			
			if (this.IsPostBack==false)
			{
				this.BPToolBar1.AddLab("slb"," Selected to perform content ");
				this.BPToolBar1.AddToolbarCheckBtnGroup("ToolbarCheckBtnGroup1");

				this.ToolbarCheckBtnGroup1.Add("Btn_StopWorkFlow"," Pending ");
				this.ToolbarCheckBtnGroup1.Add("Btn_ComeBackFlow"," Reinstated ");
				this.ToolbarCheckBtnGroup1.Add("Btn_DeleteFlowByFlag"," Tombstone ");
				this.ToolbarCheckBtnGroup1.Add("Btn_DeleteWFByRealReal"," Physically removed ");

				//this.BPToolBar1.AddSpt("sd");
				//this.BPToolBar1.AddBtn(BP.Web.Controls.NamesOfBtn.Help);

				this.ToolbarCheckBtnGroup1.Items[0].Selected=true;
				this.ToolbarCheckBtnGroup1.AutoPostBack=true;
				this.SetState();
			}
			this.BPToolBar1.ButtonClick+=new EventHandler(BPToolBar1_ButtonClick);

		}
		public void SetState()
		{
			string id=this.ToolbarCheckBtnGroup1.SelectedCheckButton.ID;
			string help="";
			switch(id)
			{
				case "Btn_StopWorkFlow":
					help="&nbsp;&nbsp; Help : Suspend the process is the process in operation , Need to temporarily stop . Such as : Penalties for a taxpayer , Very long time to find people .";
					help+="<br>&nbsp;&nbsp; Steps :<br>1) Please enter a reason to hang .<br>2) Press the OK button .";
					break;
				case "Btn_ComeBackFlow":
					help="&nbsp;&nbsp; Help : Pending process for a return to normalcy .";
					help+="<br>&nbsp;&nbsp; Steps :<br>1) Please enter the need to restore the use of reason .<br>2) Press the OK button .";
					break;
				case "Btn_DeleteFlowByFlag":
					help="&nbsp;&nbsp; Help : Delete this process to make a mark , Process system data tombstone exist in the database after .";
					help+="<br>&nbsp;&nbsp; Steps :<br>1) Please enter a tombstone reasons .<br>2) Press the OK button .";
					break;
				case "Btn_DeleteWFByRealReal":
					help="&nbsp;&nbsp; Help : This process physically removed , Completely remove the flow of information .";
					help+="<br>&nbsp;&nbsp; Steps :<br>1) Please enter the delete process of reason .<br>2) Press the OK button .";
					break;
				default:
					break;
			}
			this.Label1.Text=help;
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

		private void BPToolBar1_ButtonClick(object sender, EventArgs e)
		{
			//switch(this.ToolbarCheckBtnGroup1.
			this.SetState();
		}

		protected void Button1_Click(object sender, System.EventArgs e)
		{
			string id=this.ToolbarCheckBtnGroup1.SelectedCheckButton.ID;
			string help="";
            Int64 WorkID = Int64.Parse(this.Request.QueryString["WorkID"]);
			string fk_flow=  this.Request.QueryString["FK_Flow"] ;
			Flow fl = new Flow(fk_flow);
			WorkFlow wf = new WorkFlow(fl, WorkID);
			//Node nd = new Node(t
			switch(id)
			{
                //case "Btn_StopWorkFlow":
                //    wf.DoStopWorkFlow(this.TextBox1.Text);
                //    break;
				case "Btn_ComeBackFlow":
                    wf.DoComeBackWorkFlow(this.TextBox1.Text);
					break;
                //case "Btn_DeleteFlowByFlag":
                //    wf.DoDeleteWorkFlowByFlag(this.TextBox1.Text);
                //    break;
                //case "Btn_DeleteWFByRealReal":
                //    wf.DoDeleteWorkFlowByReal(); 
                //    break;
				default:
					break;
			}
			//this.Label1.Text=help;
			this.ResponseWriteBlueMsg(" Successful implementation .");
			this.WinClose();
		}

		protected void Button2_Click(object sender, System.EventArgs e)
		{
			this.WinClose();
		}
	}
}
