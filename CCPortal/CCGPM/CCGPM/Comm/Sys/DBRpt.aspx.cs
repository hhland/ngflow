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
using BP.Web.Controls;
using BP.Sys;

namespace BP.Web.Comm
{
	/// <summary>
	/// DBRpt 的摘要说明。
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
				this.ToErrorPage("@您没有权限查看数据日志．");
				return;
			}
			this.BPToolBar1.ButtonClick+=new EventHandler(BPToolBar1_ButtonClick);


			//this.BPToolBar1.CheckChange+=new EventHandler(BPToolBar1_CheckChange);
			if (this.IsPostBack==false)
			{
				
				this.BPToolBar1.AddLab("lb","数据库完整性检查");

				//this.BPToolBar1.AddDDL("DDL_Level", new System.EventHandler( this.BPToolBar1_CheckChange ),true);

				this.BPToolBar1.AddDDL("DDL_Level",true);


				this.DDL_Level.Items.Add(new ListItem("安全级别低","1"));
				this.DDL_Level.Items.Add(new ListItem("安全级别中","2"));
				this.DDL_Level.Items.Add(new ListItem("安全级别高","3"));
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
				str="安全级别低：";
				str+="<BR>在此安全级别下操作会得出如下结果：";
				str+="<BR>1、一个表的外键值不能为空，或者null。得出数据报告。";
				str+="<BR>2、一个表的外键值不能为空，或者null。得出数据报告。";
				str+="<BR>提示：如果您想仅仅得到数据报告，请按照此级别执行，他是很安全的．不会更改数据．";
				this.Label1.ForeColor=Color.Green;

			}
			else if (this.DDL_Level.SelectedItemStringVal=="2")
			{
				str="安全级别中：";
				str+="<BR>在此安全级别下操作会得出如下结果：";
				str+="<BR>1、一个表的外键值不能为空，或者null。得出数据报告。";
				str+="<BR>2、一个表的外键值不能为空，或者null。得出数据报告。";
				str+="<BR> 提示：如果您想得到数据报告，去掉外键左右空格，请按照此级别执行。";
				this.Label1.ForeColor=Color.Black;
			}
			else
			{
				str="安全级别高：";
				str+="<BR>在此安全级别下操作会得出如下结果：";
				str+="<BR>1、一个表的外键值不能为空，或者null。得出数据报告。";
				str+="<BR>2、一个表的外键值不能为空，或者null。得出数据报告。";
				str+="<BR>提示：如果您想得到数据报告，去掉外键左右空格，并且删除实体外键盘对应不上的纪录请按照此级别执行，注意，他有可能删除一些您有用的数据。";
				this.Label1.ForeColor=Color.Red;
			}
			this.Label1.Text=str;

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

		private void BPToolBar1_ButtonClick(object sender, EventArgs e)
		{
			ToolbarBtn btn = (ToolbarBtn)sender;

			if (btn.ID==NamesOfBtn.Confirm)
			{
				DBCheckLevel level=(DBCheckLevel)this.DDL_Level.SelectedItemIntVal;
				string rpt= PubClass.DBRpt(level);
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
