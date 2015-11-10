using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BP.Sys;
using BP.DA;
using BP.En;
using BP.Web.Controls;

namespace CCFlow.Web.Comm.UI
{
	/// <summary>
	/// SystemClass  The summary .
	/// </summary>
    public partial class DBIO : BP.Web.WebPageAdmin
	{
		protected BP.Web.Controls.Btn Btn1;
		protected BP.Web.Controls.ToolbarTB TB_EnsName
		{
			get
			{
				return this.BPToolBar1.GetTBByID("TB_EnsName");
			}
		}
		protected BP.Web.Controls.ToolbarDDL DDL_DBType
		{
			get
			{
				return this.BPToolBar1.GetDDLByKey("DDL_DBType");
			}
		}
		/// <summary>
		///  Access Control page .
		/// </summary>
		/// <returns></returns>
		protected override string WhoCanUseIt()
		{ 
			return  ",admin,8888,";
		}
		public void CheckConnStr()
		{
			 
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.CheckConnStr();
			this.BPToolBar1.ButtonClick += new System.EventHandler(this.BPToolBar1_ButtonClick);
			if (this.IsPostBack==false)
			{
				this.BPToolBar1.AddLab("sss"," Database migration ");
				//this.BPToolBar1.AddBtn(NamesOfBtn.Statistic," Registration ");
				//this.BPToolBar1.AddLab("sss"," Select an entity ");
				this.BPToolBar1.AddBtn(NamesOfBtn.Edit);
				this.BPToolBar1.AddLab("ss2"," The basic class name ");
				this.BPToolBar1.AddTB("TB_EnsName");
				this.BPToolBar1.AddBtn(NamesOfBtn.SelectNone);
				this.BPToolBar1.AddBtn(NamesOfBtn.SelectAll);
				this.BPToolBar1.AddDDL("DDL_DBType",false);
				this.DDL_DBType.BindSysEnum("SysDBType",false,AddAllLocation.None);
				this.BPToolBar1.AddBtn(NamesOfBtn.Export," Move ");
				//this.BPToolBar1.AddBtn(NamesOfBtn.FileManager," Generate SQL");
				this.TB_EnsName.Text="BP.En.Entity";
				this.CBL1.Items.Clear();
				ArrayList als =BP.En.ClassFactory.GetObjects("BP.En.Entities");	
				int i =0;
                foreach (Entities al in als)
                {
                    i++;

                    try
                    {
                        Entity myen = al.GetNewEntity;
                        if (myen.EnMap.EnType == EnType.View)
                            continue;
                        if (al.ToString() == "" || al.ToString() == null)
                            continue;

                        this.CBL1.Items.Add(new ListItem(i.ToString() + " " + myen.EnDesc + myen.EnMap.PhysicsTable, al.ToString()));
                    }
                    catch
                    {
                    }
                }
			}
		}
		public DBType CurrSelectedDBType
		{
			get
			{
				int val = this.BPToolBar1.GetDDLByKey("DDL_DBType").SelectedItemIntVal;
				return (DBType)val;
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


		/// <summary>
		///  Began to migrate 
		/// </summary>
		public void doExport()
		{
			string msg="";
			ArrayList al = new ArrayList();
			foreach(ListItem li in this.CBL1.Items)
			{
				if (li.Selected==false)
					continue;

				Entities ens= ClassFactory.GetEns( li.Value);
				al.Add(ens);
			}
			BP.Sys.PubClass.DBIO( this.CurrSelectedDBType ,al,false);
		}
		 

		private void BPToolBar1_ButtonClick(object sender, System.EventArgs e)
		{
			try
			{
				ToolbarBtn btn = (ToolbarBtn)sender;
				switch(btn.ID)
				{
					case NamesOfBtn.Export:
						this.doExport();
						return;
					case NamesOfBtn.SelectNone:
						this.CBL1.SelectNone();
						break;
					case NamesOfBtn.SelectAll:
						this.CBL1.SelectAll();
						break;
					case NamesOfBtn.FileManager:
						this.ResponseWriteBlueMsg(this.GenerCreateTableSQL(this.TB_EnsName.Text));
						//this.gener(this.TB_EnsName.Text);
						return;
					default:
						throw new Exception("error");
				}
			}
			catch(Exception ex)
			{
				this.ResponseWriteRedMsg(ex);
			}
		}
	}
}
