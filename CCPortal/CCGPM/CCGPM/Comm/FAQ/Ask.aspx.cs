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

namespace BP.Web.WF.Portal
{
	/// <summary>
	/// SendSysErr 的摘要说明。
	/// </summary>
	public partial class UIAsk : WebPage
	{

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.GenerCaption("提交问题");
          //  this.GenerLabel(this.Label1, "提交问题");
            //this.BPToolBar1.ButtonClick += new System.EventHandler(this.BPToolBar1_ButtonClick);
            if (this.IsPostBack == false)
            {
                string msg = Session["info"] as string;
                if (msg != null)
                    msg = msg.Replace("<BR>", "\n");

                this.TB_Docs.Text = " 技术服务部:\n    工作期间发现下列问题，请解决．\n   " + msg + "\n" + this.TB_Docs.Text + "\n\n       " + Web.WebUser.Name;

                //this.TB_Sender.Text=WebUser.Name;
                this.TB_Title.Text = WebUser.Name + "发现的问题,请尽快解决！！！";
                //this.BPToolBar1.AddLab("ss","寻求帮助");
                this.BPToolBar1.AddBtn(BP.Web.Controls.NamesOfBtn.Send);
                this.BPToolBar1.AddBtn(BP.Web.Controls.NamesOfBtn.Cancel);
                this.BPToolBar1.AddSpt("sa");
                this.BPToolBar1.AddBtn(BP.Web.Controls.NamesOfBtn.Help);

                this.TB_Docs.Attributes["Width"] = "100%";
                this.TB_Docs.Attributes["Height"] = "100%";
                this.TB_Docs.Attributes["TextMode"] = "MultiLine";
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

		 

		protected void BPToolBar1_ButtonClick_1(object sender, System.EventArgs e)
		{
			try
			{
                BP.Web.Controls.ToolbarBtn btn = (BP.Web.Controls.ToolbarBtn)sender;
				switch(btn.ID)
				{
					case BP.Web.Controls.NamesOfBtn.Send:
                        //Sys.FAQ faq = new BP.Sys.FAQ(); // = new BP.Sys.Ask();
                        //faq.Title=this.TB_Title.Text ;
                        //faq.Asker =WebUser.No ;
                        //faq.Doc=this.TB_Docs.Text ;
                        //faq.RDT=DA.DataType.CurrentDataTime ;
                        //faq.Insert();
                        this.ToCommMsgPage("您的问题已经发送成功, 我们会尽快的处理它, 您可以到用户使用体验去找到您的问题, 感谢您反馈．");
						//string script= "<script language=JavaScript>alert('您的问题已经发送成功,我们会尽快的处理它，感谢您反馈．');</script>";
						//this.ToMsgPage( 
						//this.Response.Write(  script );
						//this.WinClose();
						break;
					case BP.Web.Controls.NamesOfBtn.Help:
						this.Helper();
						break;
					default:
						break;
				}
			}
			catch(Exception ex)
			{
				this.Alert(ex.Message);
			}
		}
	}
}
