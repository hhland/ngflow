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
using BP.Port;
using BP.Sys;
using BP.DA;

namespace BP.Web.Comm
{
	/// <summary>
	/// FuncLink 的摘要说明。
	/// </summary>
	public partial class FuncLink : WebPage
	{
		public string Flag
		{
			get
			{
				return this.Request.QueryString["Flag"];
			}
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			//string msg="";
			try
			{
				switch(this.Flag)
				{
					case "To县局OfGS":
//						string station=WebUser.HisEmp.HisStation.No;
//						if (station=="000000" || station=="00000000" || station=="0000000000" || station=="000000000000"  )
//							this.Response.Redirect("UIEns.aspx?EnsName=BP.En.县局&IsReadonly=1",false);
//						else
//							this.ToErrorPage("您没有操作此功能的权限．");
						break;						 
					case "DeleteEn":
						Entity en =ClassFactory.GetEn(this.Request.QueryString["MainEnsName"]) ;
						//Entity en =ens.GetNewEntity;
						en.PKVal=this.Request.QueryString[en.PK];
						en.Delete();						
						//this.Response.Write("@删除成功.");
						//this.WinClose();
						//this.Alert("删除成功!!!");
						//this.WinClose();
 						this.ToErrorPage("删除成功!!!");
						//ToErrorPage(
						//this.Alert("删除成功,需要您刷新页面.");
						break;
					case "DeleteEns":
						break;
					case "DeleteEnssd":
						break;
					case "DeleteEsdsn":
						break;
					default:
						break;
				}
			}
			catch(Exception ex)
			{
				this.ToErrorPage(ex.Message);
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
	}
}
