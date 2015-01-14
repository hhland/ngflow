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
using BP.En;
using BP.DA;
using BP.Web;
using BP.Web.Comm;
using BP.Web.Controls;

namespace CCFlow.Web.Comm
{
	/// <summary>
	/// DBRpt  The summary .
	/// </summary>
    public partial class DBRpt : BP.Web.WebPageAdmin
	{
		public ToolbarDDL DDL_Level
		{
			get
			{
				return this.BPToolBar1.GetDDLByKey("DDL_Level");
			}
		}
		public ToolbarLab Lab_Msg
		{
			get
			{
				return this.BPToolBar1.GetLabByKey("Lab_Msg");
			}
		}
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (WebUser.No!="admin")
			{
				this.ToErrorPage("@ You do not have permission to view the data log го");
				return;
			}
			this.BPToolBar1.ButtonClick+=new EventHandler(BPToolBar1_ButtonClick);


			//this.BPToolBar1.CheckChange+=new EventHandler(BPToolBar1_CheckChange);
			if (this.IsPostBack==false)
			{
				
				this.BPToolBar1.AddLab("lb"," Database Integrity Check ");

				//this.BPToolBar1.AddDDL("DDL_Level", new System.EventHandler( this.BPToolBar1_CheckChange ),true);

				this.BPToolBar1.AddDDL("DDL_Level",true);


				this.DDL_Level.Items.Add(new ListItem(" Low security level ","1"));
				this.DDL_Level.Items.Add(new ListItem(" Security levels ","2"));
				this.DDL_Level.Items.Add(new ListItem(" High security level ","3"));
				this.BPToolBar1.AddBtn(NamesOfBtn.Confirm);
				this.BPToolBar1.AddSpt("spt1");
				this.BPToolBar1.AddBtn(NamesOfBtn.Help);
				this.SetText();
			}
			this.DDL_Level.SelectedIndexChanged +=new EventHandler(BPToolBar1_CheckChange);			

		}
		public void SetText()
		{
			string str="";
			if (this.DDL_Level.SelectedItemStringVal=="1")
			{
				str=" Low security level :";
				str+="<BR> Operating at this level of security will be the following results :";
				str+="<BR>1, A foreign key table can not be empty , Or null. Derived data reports .";
				str+="<BR>2, A foreign key table can not be empty , Or null. Derived data reports .";
				str+="<BR> Prompt : If you want to just get the data report , Perform in accordance with this level , He is a very safe го Does not change the data го";
				this.Label1.ForeColor=Color.Green;

			}
			else if (this.DDL_Level.SelectedItemStringVal=="2")
			{
				str=" Security levels :";
				str+="<BR> Operating at this level of security will be the following results :";
				str+="<BR>1, A foreign key table can not be empty , Or null. Derived data reports .";
				str+="<BR>2, A foreign key table can not be empty , Or null. Derived data reports .";
				str+="<BR>  Prompt : If you want to get data reports , Remove spaces around the foreign key , Perform in accordance with this level .";
				this.Label1.ForeColor=Color.Black;
			}
			else
			{
				str=" High security level :";
				str+="<BR> Operating at this level of security will be the following results :";
				str+="<BR>1, A foreign key table can not be empty , Or null. Derived data reports .";
				str+="<BR>2, A foreign key table can not be empty , Or null. Derived data reports .";
				str+="<BR> Prompt : If you want to get data reports , Remove spaces around the foreign key , And deletes the corresponding entity outside the keyboard does not perform according to the records on this level , Watch out , He may remove some of your useful data .";
				this.Label1.ForeColor=Color.Red;
			}
			this.Label1.Text=str;

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
			ToolbarBtn btn = (ToolbarBtn)sender;

			if (btn.ID==NamesOfBtn.Confirm)
			{
                BP.DA.DBCheckLevel level = (BP.DA.DBCheckLevel)this.DDL_Level.SelectedItemIntVal;
				string rpt= BP.Sys.PubClass.DBRpt(level);
				this.Label1.Text =rpt;
			}
			else
			{
				this.Helper("DBRpt.htm");
			}

		}
		private void BPToolBar1_CheckChange(object sender, EventArgs e)
		{
			this.SetText();
		}
	}
}
