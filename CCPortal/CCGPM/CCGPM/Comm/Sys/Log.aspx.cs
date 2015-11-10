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
using System.IO;


namespace BP.Web.Comm.Sys
{
	/// <summary>
	/// Log 的摘要说明。
	/// </summary>
    public partial class UILog : BP.Web.WebPageAdmin
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{

            this.Label1.Text = this.GenerCaption("日志管理");

			//this.GenerLabel(this.Label1,"日志管理");

			System.IO.DirectoryInfo di= new System.IO.DirectoryInfo(BP.Sys.SystemConfig.PathOfLog  );
			if (di.Exists==false)
			{
				di.Create();
			}
			this.UCSys1.AddTable();
			this.UCSys1.Add("<TR>");
			this.UCSys1.AddTDTitle("ID");
			this.UCSys1.AddTDTitle("文件名称");
			this.UCSys1.AddTDTitle("大小");
			//this.UCSys1.AddTDTitle("建立日期");
			this.UCSys1.Add("</TR>");

			FileInfo[] fis =  di.GetFiles("*.*");
			int idx=0;
			foreach(FileInfo fi in fis)
			{
				idx++;
				this.UCSys1.Add("<TR onmouseover='TROver(this)' onmouseout='TROut(this)'>");
				this.UCSys1.Add("<TD class='Idx' >"+idx.ToString()+"</TD>");
				this.UCSys1.AddTD("<a href='../../Data/Log/"+fi.Name+"'>"+fi.Name+"</a>");
				this.UCSys1.AddTDNum( fi.Length.ToString("#")  );
				this.UCSys1.Add("</TR>");
			}
			this.UCSys1.Add("</Table>");

			//onmouseover='TROver(this)' onmouseout='TROut(this)'
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
