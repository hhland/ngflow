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
	/// ManagerTools  The summary .
	/// </summary>
	public partial class ManagerTools : BP.Web.WebPageAdmin
	{
		protected System.Web.UI.WebControls.Label Label1;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            //if (Web.WebUser.No.IndexOf("8888") > 1 || Web.WebUser.No.IndexOf("admin") != -1 || Web.WebUser.No.IndexOf("8888") == 0)
            //{
            //    //this.ToErrorPage(" You do not have permission to use this feature го");
            //}
            //else
            //{
            //    if (Web.WebUser.No == "288888")
            //    {
            //        //this.ToErrorPage(" You do not have permission to use this feature го");
            //    }
            //    else
            //    {
            //        this.ToErrorPage(" You do not have permission to use this feature го");
            //    }
            //}


            DataSet ds = new DataSet();
            ds.ReadXml(BP.Sys.SystemConfig.PathOfXML + "AdminTools.xml");

            DataTable mydt = ds.Tables[0];

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ICON", typeof(string)));
            dt.Columns.Add(new DataColumn(" Name ", typeof(string)));
            dt.Columns.Add(new DataColumn(" Provided ", typeof(string)));
            dt.Columns.Add(new DataColumn(" Description ", typeof(string)));

            //dt.Columns.Add( new DataColumn(" Description ", typeof(string)));

            DataRow dr1 = dt.NewRow();
            dr1["ICON"] = "<Img src='./../WF/Img/Btn/Do.gif' border=0/>";
            dr1[" Name "] = "<a href='EditWebconfig.aspx' > Global Configuration </A>";
            dr1[" Provided "] = "admin";
            dr1[" Description "] = " Site information is used to set global ";
            dt.Rows.Add(dr1);

            dr1 = dt.NewRow();
            dr1["ICON"] = "<Img src='./../Img/Btn/Do.gif' border=0/>";
            dr1[" Name "] = "<a href='./../Port/ChangePass.aspx' > Change Password </A>";
            dr1[" Provided "] = "admin";
            dr1[" Description "] = " Site information is used to set global ";
            dt.Rows.Add(dr1);


            foreach (DataRow mydr in mydt.Rows)
            {
                if (mydr["Enable"].ToString().Trim() == "0")
                    continue;

                DataRow dr = dt.NewRow();
                dr["ICON"] = "<Img src='" + mydr["ICON"] + "' />";
                dr[" Name "] = "<a href='" + mydr["URL"] + "' >" + mydr["Name"] + "</A>";
                dr[" Provided "] = mydr["DFor"];
                dr[" Description "] = mydr["DESC"];
                dt.Rows.Add(dr);
            }

            this.UCSys1.AddTable();
            this.UCSys1.AddCaptionLeft(this.GenerCaption(" Webmaster Tools "));
            this.UCSys1.AddTR();
            this.UCSys1.AddTDTitle("");
            this.UCSys1.AddTDTitle(" Name ");
            this.UCSys1.AddTDTitle(" Provided ");
            this.UCSys1.AddTDTitle(" Description ");
            this.UCSys1.AddTREnd();

            foreach (DataRow dr in dt.Rows)
            {
                this.UCSys1.AddTR();
                this.UCSys1.AddTD(dr["ICON"].ToString());
                this.UCSys1.AddTD(dr[" Name "].ToString());
                this.UCSys1.AddTD(dr[" Provided "].ToString());
                this.UCSys1.AddTD(dr[" Description "].ToString());
                this.UCSys1.AddTREnd();
            }
            this.UCSys1.AddTableEnd();
            //    this.Ucsys1.Add(this.GenerCaption(" Function execution "));
            // this.Ucsys1.AddHR();
            // this.Response.Write(this.GenerTablePage(dt, " Webmaster Tools "));
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
