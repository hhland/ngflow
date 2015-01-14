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
using BP.Web;

namespace BP.Web.Portal
{
	/// <summary>
	/// Exit  The summary .
	/// </summary>
	public partial class Exit :  WebPage
	{

		private string url
		{
			get
			{
				try
				{
					return (string)ViewState["url"];
				}
				catch
				{
				   return "Wel.aspx";
				}
			}
			set
			{
				ViewState["url"]=value;
			}
		}
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.Session["url"]=this.Request.RawUrl;
			try
			{
				WebUser.Exit();
				//this.WinClose();
                this.Response.Redirect("/SSO/Loginin.aspx?DoType=Logout");
            }
			catch(System.Exception ex)
			{
				this.ResponseWriteRedMsg(ex) ; 
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is  ASP.NET Web  Form Designer required .
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

		private void LinkButton1_Click(object sender, System.EventArgs e)
		{
			System.Web.HttpContext.Current.Response.Redirect(System.Web.HttpContext.Current.Request.ApplicationPath+"/SignIn.aspx?url="+this.url); //this.ToSignInPage();
		}

		private void Btn_O_Click(object sender, System.EventArgs e)
		{			

			try
			{
				WebUser.Exit();
				this.ResponseWriteBlueMsg(" Right now , You have security out of the system , Thank you for using , See you !");
			}
			catch(System.Exception ex)
			{
				this.ResponseWriteRedMsg(ex) ; 
			}
		}

		private void Btn1_Click(object sender, System.EventArgs e)
		{
			this.Response.Redirect("SignIn.aspx",true);
		}
	}
}
