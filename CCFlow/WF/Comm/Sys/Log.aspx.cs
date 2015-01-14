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


namespace CCFlow.Web.Comm.Sys
{
	/// <summary>
	/// Log  The summary .
	/// </summary>
    public partial class UILog : BP.Web.WebPageAdmin
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{

            this.Label1.Text = this.GenerCaption(" Log Management ");

			//this.GenerLabel(this.Label1," Log Management ");

			System.IO.DirectoryInfo di= new System.IO.DirectoryInfo(BP.Sys.SystemConfig.PathOfLog  );
			if (di.Exists==false)
			{
				di.Create();
			}
			this.UCSys1.AddTable();
			this.UCSys1.Add("<TR>");
			this.UCSys1.AddTDTitle("ID");
			this.UCSys1.AddTDTitle(" File name ");
			this.UCSys1.AddTDTitle(" Size ");
			//this.UCSys1.AddTDTitle(" Creation Date ");
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

		#region Web  Form Designer generated code 
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN:  This call is  ASP.NET Web  Form Designer required .
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
	}
}
