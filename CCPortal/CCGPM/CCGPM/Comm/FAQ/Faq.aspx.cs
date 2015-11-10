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
using BP.Sys;
using BP.Web.Controls;

namespace BP.Web.Comm.Port
{
	/// <summary>
	/// FAQ 的摘要说明。
	/// </summary>
	public partial class UIFAQ : WebPage
	{
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (this.RefOID == 0)
            {
                this.Label1.Text = this.GenerCaption("系统XP=><a href='Ask.aspx' >提出问题</a>");
                //	this.GenerLabel(this.Label1,"");
                this.Bind();
            }
            else
            {
                this.Label1.Text = this.GenerCaption("系统XP=><a href='FAQ.aspx' >返回列表</a>=><a href='Ask.aspx' >提出问题</a>");
                //this.GenerLabel(this.Label1,"");
                this.BindFAQ();
                this.UCSys1.GetBtnByID("Btn_Submint").Click += new EventHandler(btn_Click);
            }
        }
		public void BindFAQ()
		{
            //FAQ en = new FAQ(this.RefOID) ;
            //if (this.IsPostBack==false)
            //    en.Update(FAQAttr.ReadNum, en.ReadNum+1) ;

            //this.UCSys1.Controls.Clear();
            //this.UCSys1.AddTable("width='100%'");
            //this.UCSys1.Add("<TR>");
            //this.UCSys1.AddTDBar("<b>提出人:</b>"+en.AskerText +"&nbsp;&nbsp;&nbsp;&nbsp;<b>时间:</b>"+en.RDT +"&nbsp;&nbsp;&nbsp;&nbsp;<b>阅读人次:</b>"+en.ReadNum ) ;
            //this.UCSys1.Add("</TR>");

            //this.UCSys1.Add("<TR>");
            //this.UCSys1.AddTDBar("<b>标题:</b>"+en.Title ) ;
            //this.UCSys1.Add("</TR>");

            //this.UCSys1.Add("<TR>");
            //this.UCSys1.AddTD( en.DocsHtml+"<br><br><br>" ) ;
            //this.UCSys1.Add("</TR>");

            //FAQDtls dtls = new FAQDtls(this.RefOID);
            //int i =0;
            //foreach(FAQDtl dtl in dtls)
            //{
            //    i++;
            //    this.UCSys1.Add("<TR>");

            //    if (dtl.Answer==WebUser.No)
            //        this.UCSys1.AddTDBar("第"+i+"楼, &nbsp;&nbsp;" +dtl.AnswerText +"&nbsp;&nbsp;" +dtl.RDT+" &nbsp;<a  href=\"javascript:DeleteDtl('"+dtl.OID+"')\" ><img src='../../Images/Btn/Delete.gif' border=0 >删除</a><a href=\"javascript:Edit( '"+dtl.Docs+"',  '"+dtl.OID+"')\" ><img src='../../Images/Btn/Edit.gif' border=0 >修改</a>") ;
            //    else
            //        this.UCSys1.AddTDBar("第"+i+"楼, &nbsp;&nbsp;" +dtl.AnswerText +"&nbsp;&nbsp;" +dtl.RDT+" &nbsp;<a  href=\"javascript:Replay('"+dtl.Docs+"')\"   ><img src='../../Images/Btn/Replay.gif' border=0 >回复</a>") ;

            //    this.UCSys1.Add("</TR>");

            //    this.UCSys1.Add("<TR>");
            //    this.UCSys1.AddTD( dtl.DocsHtml+"<br><br><br>" );
            //    this.UCSys1.Add("</TR>");
            //}

            //this.UCSys1.Add("<TR>");
            //this.UCSys1.AddTDTitle("回复");
            //this.UCSys1.Add("</TR>");

            //this.UCSys1.Add("<TR>");
            //this.UCSys1.Add("<TD Height='200px' >");
			
            //TB tb = new TB();
            //tb.ID="TB_Docs";

            //tb.Text="\n\n\n\n\n\n\n--------\n "+WebUser.Name;
            //tb.TextMode=TextBoxMode.MultiLine;
            //tb.Columns=0;
            //tb.Rows=0;
            //tb.Attributes["Width"]="100%";
            //tb.Attributes["Height"]="100%"; 
            //tb.Attributes["style"]="height:100%;width:100%;";
            //this.UCSys1.Add(tb) ;

            //this.UCSys1.Add("</TD>");
            //this.UCSys1.Add("</TR>");

            //this.UCSys1.Add("<TR>");
            //this.UCSys1.Add("<TD>&nbsp;&nbsp;&nbsp;&nbsp;");
            //Btn btn = new Btn();
            //btn.ID="Btn_Submint";
            //btn.Text="提交";
            ////btn.Click+=new EventHandler(btn_Click);
            //this.UCSys1.Add( btn) ;
            //this.UCSys1.Add("</TD>");
            //this.UCSys1.Add("</TR>");
            //this.UCSys1.Add("</Table>");
		}
		public void Bind()
		{
            //this.UCSys1.AddTable();
            //FAQs ens = new FAQs();
            //ens.RetrieveAll(FAQAttr.OID);
            //int i = 0 ;
            //this.UCSys1.AddTable();
            //this.UCSys1.Add("<TR>");
            //this.UCSys1.AddTDTitle( "ID" );
            //this.UCSys1.AddTDTitle( "提出人" );
            //this.UCSys1.AddTDTitle( "提出日期" );
            //this.UCSys1.AddTDTitle( "标题" );
            //this.UCSys1.AddTDTitle( "阅读/回复数" );
            //this.UCSys1.Add("</TR>");
            //foreach(FAQ en in ens)
            //{
            //    i++;
            //    this.UCSys1.Add("<TR title='"+en.Docs+"' onmouseover='TROver(this)' onmouseout='TROut(this)' >");
            //    this.UCSys1.AddTD( i  );
            //    this.UCSys1.AddTD( en.AskerText  );
            //    this.UCSys1.AddTD( en.RDT  );
            //    this.UCSys1.Add( "<TD class='TD' width='50%' ><a href='FAQ.aspx?RefOID="+en.OID+"' >"+en.Title + "</a></td>");
            //    this.UCSys1.AddTD(  en.ReadNum+"/"+en.DtlNum   );
            //    this.UCSys1.Add("</TR>");
            //}
            //this.UCSys1.Add("</Table>");
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

		private void btn_Click(object sender, EventArgs e)
		{
            //FAQDtl dtl = new FAQDtl();
            //dtl.FK_FAQ=this.RefOID;
            //dtl.Answer=WebUser.No;
            //dtl.Docs=this.UCSys1.GetTBByID("TB_Docs").Text;
            //dtl.Insert();

            //FAQ en = new FAQ(this.RefOID);
            //en.OID=this.RefOID;
            //en.Update(FAQAttr.DtlNum, en.DtlNum+1);

            //this.BindFAQ();
		}
	}
}
