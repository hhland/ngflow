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
using BP.Web;
using BP.Web.Pub ;
using BP.Port;

namespace CCFlow.Web.Comm.UI.WF
{
	/// <summary>
	/// ChangePass  The summary .
	/// </summary>
	public partial class ChangePass12 : BP.Web.WebPage
	{
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.Btn_Save.Click += new System.EventHandler(this.Btn_Save_Click);
            //  this.Btn_C.Click += new System.EventHandler(this.Btn_C_Click);

            this.Label1.Text = this.GenerCaption(" Prompt : For system security periodically change your password ");
            //  this.GenerLabel(this.Label1, );
            //  Put user code to initialize the page here 
        }

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//this.SubPageMessage=" Change Password ";
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

        private void Btn_Save_Click(object sender, System.EventArgs e)
        {
            try
            {
                Emp ep = new Emp(WebUser.No);
                if (!ep.Pass.Equals(this.TB1.Text))
                {
                    this.Alert(" Old password is incorrect !");
                    //  this.ResponseWriteRedMsg(" Old password is incorrect !");
                    return;
                }

                if (this.TB2.Text.Equals(this.TB3.Text))
                {
                    ep.Pass = this.TB2.Text;
                    ep.Update();
                    this.Alert(" Modified successfully , Remember your new password !");
                    // this.ResponseWriteBlueMsg("");
                    //this.Response.Redirect("../wel.aspx",true);
                    return;
                }
                else
                {
                    this.Alert(" The two passwords do not match !");
                    // this.ResponseWriteRedMsg();
                    return;
                }
            }
            catch (System.Exception ex)
            {
                this.ResponseWriteRedMsg(" Error : " + ex.Message);
            }
        }
	}
}
