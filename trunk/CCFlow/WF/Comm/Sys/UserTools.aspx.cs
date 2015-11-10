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

namespace CCFlow.Web.WF.Comm.Sys
{
	/// <summary>
	/// UserTools  The summary .
	/// </summary>
    public partial class UserTools : BP.Web.WebPageAdmin
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			DataSet ds = new DataSet();
			ds.ReadXml(BP.Sys.SystemConfig.PathOfXML+"UserTools.xml");
			DataTable mydt=ds.Tables[0];
			DataTable dt = new DataTable();
			dt.Columns.Add( new DataColumn("ICON", typeof(string)));
			dt.Columns.Add( new DataColumn(" Name ", typeof(string)));
			dt.Columns.Add( new DataColumn(" Description ", typeof(string)));

			foreach(DataRow mydr in mydt.Rows)
			{
				if ( mydr["Enable"].ToString().Trim() =="0")
					continue;

				DataRow dr =dt.NewRow();
				dr["ICON"] = "<Img src='"+mydr["ICON"]+"' />";
				dr[" Name "] = "<a href='"+mydr["URL"]+"' >"+mydr["Name"]+"</A>";
				dr[" Description "] = mydr["DESC"] ;
				dt.Rows.Add(dr);
			}
		 
			this.Response.Write(this.GenerTablePage( dt ," User Tools ")) ; 
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
