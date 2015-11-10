
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
using BP.Port;
using BP.Web;
using System.Xml;
using System.IO ;
using BP.Sys; 

namespace BP.Web
{
	/// <summary>
	/// SignIn 的摘要说明。
	/// </summary>
	public partial class SignInPG: Page
	{
		//protected BP.Web.Controls.BPCheckBox BPCheckBox1;

		public string RawUrl
		{
			get
			{
				return ViewState["RawUrl"] as string ; 
			}
			set
			{
				ViewState["RawUrl"]=value;
			}
		}
		public bool IsTurnTo
		{
			get
			{
				if (this.Request.QueryString["IsTurnTo"]==null)
					return false;
				else
					return true;
			}
		}
		public void Page_Load(object sender, System.EventArgs e)
		{
            string userNo = this.Request.QueryString["UserNo"];
            if (userNo != null && userNo.Length > 1)
            {
                string sid = this.Request.QueryString["SID"];
                if (WebUser.CheckSID(userNo,sid) == true)
                {
                    Response.Redirect("Home.aspx", false);
                    return;
                }
                else
                {
                    this.Response.Write("授权验证失败。");
                    this.TB_No.Text = userNo;
                }
            }

            if (DBAccess.IsExitsObject("Port_Emp") == false)
            {
                Response.Redirect("/GPM/DBInstall.aspx", false);
                return;
            }

			string script="<script language=javascript>function setFocus(ctl) {if (document.forms[0][ctl] !=null )  { document.forms[0][ctl].focus(); } } setFocus('"+this.TB_Pass.ClientID+"'); </script>";
			this.RegisterStartupScript("func", script);
			if (this.Request.QueryString["Token"] !=null )
			{
				HttpCookie hc1 = this.Request.Cookies["CCS"];
				if (hc1!=null)
				{
					if (this.Request.QueryString["Token"]==hc1.Values["Token"] )
					{
						Emp em = new Emp(this.Request.QueryString["No"]);
						WebUser.SignInOfGener(em,true);
						WebUser.Token = this.Request.QueryString["Token"];
						Response.Redirect("Home.aspx",false);
						return;
					}
				}
			}

			if (this.IsPostBack==false)
			{
				this.TB_No.Attributes["background-image"]="url('beer.gif')"; 
				HttpCookie hc = this.Request.Cookies["CCS"];
				if (hc!=null)
				{

              //      Response.Redirect("Home.aspx", false);
					this.TB_No.Text=hc.Values["UserNo"];
					//this.Lab1.Text=hc.Values["UserName"];
					//this.TB_Pass.Text=hc.Values["Pass"];
				}
			}
			if (this.Request.QueryString["IsChangeUser"]!=null)
			{
				this.RawUrl=this.Request.RawUrl;
			}			 
			if (this.Request.Browser.MajorVersion < 6 )	
			{
				this.Response.Write("对不起，系统检测到您当前使用的IE版本是["+this.Request.Browser.Version+"]，系统不能在当前的IE上正常工作。想正确的使用此系统，请升级到IE6.0，请点击<a href='../IE6.rar'>这里下载IE6.0</A>。下载后，解开压缩文件，运行 ie6setup.exe，如果有疑问请致电 ["+BP.Sys.SystemConfig.ServiceTel+"], 或者发 Mail ["+BP.Sys.SystemConfig.ServiceMail+"]。");
				this.Btn1.Enabled=false;
				this.TB_No.Enabled=false;
				this.TB_Pass.Enabled=false;
			}
		} 

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN：该调用是 ASP.NET Web 窗体设计器所必需的。
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

		public void Btn1_Click(object sender, System.EventArgs e)
		{
			try
			{
				Emp em = new Emp(this.TB_No.Text);
				if ( em.Pass.Trim().Equals(this.TB_Pass.Text.Trim() ) || this.TB_Pass.Text.ToLower().Trim()== BP.Sys.SystemConfig.AppSettings["GenerPass"] || SystemConfig.IsDebug ) 
				{
					//OnlineUserManager.AddUser(em,"ss",em.FK_DeptText);
                    if (this.Request.QueryString["IsChangeUser"] != null)
                    {
                        /* 如果是更改用户.*/
                        if (this.Session["OID"] != null)
                        {
                            string oid = WebUser.No;
                            this.Session.Clear();
                        }
                    }

					HttpCookie cookie=new HttpCookie("CCS");
					cookie.Expires = DateTime.Now.AddMonths(10);
					cookie.Values.Add( "UserNo",em.No);
					cookie.Values.Add( "UserName",em.Name);
					cookie.Values.Add( "Token",this.Page.Session.SessionID );
                    cookie.Values.Add("SID", this.Page.Session.SessionID);

					Response.AppendCookie( cookie );
					if (this.Session["UserNo"]!=null)
					{
						string oid= this.Session["UserNo"].ToString();
					}
					WebUser.SignInOfGener(em,"CH","",true,true);
                    //WebUser.SetSID(this.Session.SessionID);

                    Response.Redirect("Default.aspx", false);
					return;
				}
				else
				{
					throw new Exception("密码错误！检查是否按下了CapsLock ？");
				}
			}
			catch(System.Exception ex)
			{
				this.Response.Write("<font color=red ><b>@用户名密码错误!@检查是否按下了CapsLock.@更详细的信息:"+ex.Message+"</b></font>");
				 
			}
		}
		 
	}
}
