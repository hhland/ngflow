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

namespace BP.Web.Comm.UI
{
	/// <summary>
	/// SystemClass 的摘要说明。
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
		/// 控制页面的访问权限。
		/// </summary>
		/// <returns></returns>
		protected override string WhoCanUseIt()
		{ 
			return  ",admin,8888,";
		}
		public void CheckConnStr()
		{
			string str = SystemConfig.AppCenterDSN;
			if (str == SystemConfig.DBAccessOfOracle)
				throw new Exception("AppCenterDSN 不能与 DBAccessOfOracle 一样" );
			if (str == SystemConfig.DBAccessOfOracle1)
				throw new Exception("AppCenterDSN 不能与 DBAccessOfOracle1 一样" );

			if (str == SystemConfig.DBAccessOfOLE)
				throw new Exception("AppCenterDSN 不能与 DBAccessOfOLE 一样" );
			if (str == SystemConfig.DBAccessOfODBC)
				throw new Exception("AppCenterDSN 不能与 DBAccessOfODBC 一样" );

			if (str == SystemConfig.DBAccessOfMSMSSQL)
				throw new Exception("AppCenterDSN 不能与 DBAccessOfMSMSSQL 一样" );

			if (str == SystemConfig.DBAccessOfMSMSSQL)
				throw new Exception("AppCenterDSN 不能与 DBAccessOfMSMSSQL 一样" );
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.CheckConnStr();
			this.BPToolBar1.ButtonClick += new System.EventHandler(this.BPToolBar1_ButtonClick);
			if (this.IsPostBack==false)
			{
				this.BPToolBar1.AddLab("sss","数据库迁徙");
				//this.BPToolBar1.AddBtn(NamesOfBtn.Statistic,"注册");
				//this.BPToolBar1.AddLab("sss","选择一个实体");
				this.BPToolBar1.AddBtn(NamesOfBtn.Edit);
				this.BPToolBar1.AddLab("ss2","基本类名称");
				this.BPToolBar1.AddTB("TB_EnsName");
				this.BPToolBar1.AddBtn(NamesOfBtn.SelectNone);
				this.BPToolBar1.AddBtn(NamesOfBtn.SelectAll);
				this.BPToolBar1.AddDDL("DDL_DBType",false);
				this.DDL_DBType.BindSysEnum("SysDBType",false,AddAllLocation.None);
				this.BPToolBar1.AddBtn(NamesOfBtn.Export,"迁徙");
				//this.BPToolBar1.AddBtn(NamesOfBtn.FileManager,"生成SQL");
				this.TB_EnsName.Text="BP.En.Entity";
				this.CBL1.Items.Clear();
				ArrayList als =ClassFactory.GetObjects("BP.En.Entities");	
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

		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion


		/// <summary>
		/// 开始迁徙
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
			PubClass.DBIO( this.CurrSelectedDBType ,al,false);
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
