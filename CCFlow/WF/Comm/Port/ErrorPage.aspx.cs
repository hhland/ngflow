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
using BP.Web.UC;
using BP.Web;


namespace CCFlow.Web.Comm
{
	/// <summary>
	/// ErrPage  The summary .
	/// </summary>
	public partial class ErrPage: BP.Web.WebPage
	{

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Response.AddHeader("P3P", "CP=CAO PSA OUR");
            this.UCSys1.Add(this.Msg);
        }

        private string Msg
        {
            get
            {
                string msg = this.Session["info"] as string;
                if (msg == null)
                    msg = this.Application["info" + WebUser.No] as string;
                if (msg == null)
                {
                    msg = "@ The message is lost ."; // "@ Information not found , Please find it in the way work .";
                }
                return msg;
            }
        }
		/// <summary>
		/// DealPage
		/// </summary>
		private void DealPage()
		{
//			string mess ; 
//			switch (this.ErrorId)
//			{
//				case "NoUserNoSession":
//					mess=" You can record the time is too long ";
//				case "ddd":
//				default :
//			}
//			this.LabMess.Text=mess;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//this.IsAuthenticate=false ;
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

		private void Btn1_Click(object sender, System.EventArgs e)
		{
			this.WinClose();
			 
		}

		private void Btn2_Click(object sender, System.EventArgs e)
		{
			this.Session["info"]=this.Msg;
			this.Response.Redirect("../FAQ/Ask.aspx",true);
		}

		private void Btn3_Click(object sender, System.EventArgs e)
		{
		
		}
	}
}
