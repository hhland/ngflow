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

namespace BP.Web.WF.Comm
{
	/// <summary>
	/// FilePreview 的摘要说明。
	/// </summary>
	public partial class FilePreview : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string mapfilepath = Session["PreviewFilePath"] as string;
			if( mapfilepath ==null || mapfilepath=="" )
			{
				this.Response.Write( "没有检索到资源！");
				return ;
			}

			string ext = System.IO.Path.GetExtension( mapfilepath ).ToLower();
			this.Response.Redirect( mapfilepath );
			/*
			switch( ext )
			{
				case ".gif":
				case ".jpg":
					this.Response.WriteFile( mapfilepath );
					break;
				case ".rtf":
				case ".doc":
				case ".txt":
					this.Response.Redirect( mapfilepath );
					break;
				default:
					this.Response.Write( "没有提供对此类文件的预览！");
					break;
			}
			*/

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
