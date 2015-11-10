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

namespace BP.Web.Comm
{
	/// <summary>
	/// ChangeSystem 的摘要说明。
	/// </summary>
	public partial class ChangeSystem : WebPage
	{

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.Label1.Text = this.GenerCaption("切换系统");
            //this.GenerLabel(this.Label1,"切换系统");
            this.UCSys1.BindSystems();
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
