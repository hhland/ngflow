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
	public partial class Info : BP.Web.WebPage
	{
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Response.AddHeader("P3P", "CP=CAO PSA OUR");
            this.UCSys1.AddMsgOfInfo(" Prompt ", this.Msg);

            this.Session["Info"] = null;
            this.Session["info"] = null;
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
	}
}
