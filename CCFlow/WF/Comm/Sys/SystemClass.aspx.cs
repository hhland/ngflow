using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BP.Sys;
using BP.DA;
using BP.En;
using BP.Web.Controls;
using BP;

namespace CCFlow.Web.Comm.UI
{
	/// <summary>
	/// SystemClass  The summary .
	/// </summary>
    public partial class SystemClass : BP.Web.WebPageAdmin
	{
		protected BP.Web.Controls.Btn Btn1;
		protected BP.Web.Controls.ToolbarTB TB_EnsName
		{
			get
			{
				return this.BPToolBar1.GetTBByID("TB_EnsName");
			}
		}
		/// <summary>
		///  Access Control page .
		/// </summary>
		/// <returns></returns>
		protected override string WhoCanUseIt()
		{ 
			return  ",admin,8888,";
		}
		public new string EnName
		{
			get
			{
				return this.Request.QueryString["EnName"];
			}
		}
        public void BindEn(string enName)
        {
            Entity en =BP.En.ClassFactory.GetEn(enName);
            Map map = en.EnMap;

            this.UCSys1.AddTable();
            this.UCSys1.Add("<TR>");
            this.UCSys1.AddTDBar(" Basic properties ");
            this.UCSys1.Add("</TR>");

            this.UCSys1.Add("<TR>");
            this.UCSys1.AddTD(" Class name " + en.ToString() + " Entity Name :" + en.EnMap.EnDesc + " Physical table :" +
                en.EnMap.PhysicsTable + " Type :" + en.EnMap.EnType + "EnDBUrl:" + en.EnMap.EnDBUrl + "  Coding structure " + en.EnMap.CodeLength + " Length coding :" + en.EnMap.CodeLength + " Storage location :" + en.EnMap.DepositaryOfEntity + " Map Storage location :" + en.EnMap.DepositaryOfMap);
            this.UCSys1.Add("</TR>");

            this.UCSys1.Add("<TR>");
            this.UCSys1.AddTDBar(" Mapping Information ");
            this.UCSys1.Add("</TR>");

            this.UCSys1.Add("<TR>");

            this.UCSys1.AddTable();
            this.UCSys1.Add("<TR>");

            this.UCSys1.AddTDTitle("ID");
            this.UCSys1.AddTDTitle(" Property ");
            this.UCSys1.AddTDTitle(" Field ");
            this.UCSys1.AddTDTitle(" Data Types ");
            this.UCSys1.AddTDTitle(" Data Types ");
            this.UCSys1.Add("<TR>");

            this.UCSys1.Add("</TR>");
           
            this.UCSys1.Add("</Table>");
            this.UCSys1.Add("</TR>");
            this.UCSys1.Add("</Table>");
        }
        public void BindEns()
        { 

        }
		protected void Page_Load(object sender, System.EventArgs e)
		{

		//	this.GenerLabel(this.Label1," System entities ");
            if (this.EnName != null)
            {
                this.BindEn(this.EnName);
                return;
            }

			this.BPToolBar1.ButtonClick += new System.EventHandler(this.BPToolBar1_ButtonClick);
            if (this.IsPostBack == false)
            {
                //this.BPToolBar1.AddLab("sss"," Entity Management System ");
                //this.BPToolBar1.AddBtn(NamesOfBtn.Statistic," Registration ");
                //this.BPToolBar1.AddLab("sss"," Select an entity ");
                //this.BPToolBar1.AddBtn(NamesOfBtn.Edit);
                this.BPToolBar1.AddLab("ss2", " Base class ");
                this.BPToolBar1.AddTB("TB_EnsName");
                this.BPToolBar1.AddBtn(NamesOfBtn.Search);
                this.BPToolBar1.AddBtn(NamesOfBtn.Export);
                this.BPToolBar1.AddBtn(NamesOfBtn.FileManager, " Generate SQL");
                this.BPToolBar1.AddBtn(NamesOfBtn.Card, " Add notes ");
                this.TB_EnsName.Text = "BP.En.Entity";
            }

			this.Bind();
		}
        public void Bind()
        {
            this.UCSys1.Controls.Clear();
            ArrayList al = null;
            try
            {
                string info = this.TB_EnsName.Text;
                if (info.Length == 0)
                    info = "BP.En.Entity";

                this.TB_EnsName.Text = info;
                al = BP.En.ClassFactory.GetObjects(info);
            }
            catch (Exception ex)
            {
                this.ResponseWriteBlueMsg(ex.Message);
                return;
            }

            this.UCSys1.AddTable();
            this.UCSys1.Add("<TR>");
            this.UCSys1.AddTDTitle("ID");
            this.UCSys1.AddTDTitle(" Class name ");
            this.UCSys1.AddTDTitle(" Description ");
            this.UCSys1.AddTDTitle(" Physical table ");
            this.UCSys1.AddTDTitle(" Type ");
            this.UCSys1.AddTDTitle(" Operating 1");
            this.UCSys1.AddTDTitle(" Operating 2");
            this.UCSys1.AddTDTitle(" Operating 3");
           // this.UCSys1.AddTDTitle(" Operating 4");
        //   this.UCSys1.AddTDTitle(" Operating 5");
            this.UCSys1.Add("</TR>");

            int i = 0;
            foreach (Object obj in al)
            {
                i++;
                Entity en = null;
                try
                {
                    en = obj as Entity;
                    string s = en.EnDesc;
                    if (en == null)
                        continue;
                    this.UCSys1.AddTREnd();
                }
                catch
                {
                    continue;
                }

                this.UCSys1.Add("<TR window.location.href='SystemClass.aspx?EnName=" + en.ToString() + "' onmouseover='TROver(this)' onmouseout='TROut(this)' >");
                this.UCSys1.AddTDIdx(i);
                this.UCSys1.AddTD(en.ToString());
                this.UCSys1.AddTD(en.EnDesc);
                this.UCSys1.AddTD(en.EnMap.PhysicsTable);
                this.UCSys1.AddTD(en.EnMap.EnType.ToString());
                this.UCSys1.AddTD("<a href=\"javascript:WinOpen('SystemClassDtl.aspx?EnsName=" + en.ToString() + "','dtl') ; \" > Field </a>");
                this.UCSys1.AddTD("<a href=\"javascript:WinOpen('SystemClassDtl.aspx?EnsName=" + en.ToString() + "&Type=Check','dtl') ; \" > Examination </a>");

                this.UCSys1.AddTD("<a href=\"javascript:WinOpen('../RefFunc/UIEn.aspx?EnName=" + en.ToString() + "&Type=Check','dtl') ; \" > Interface </a>");
                //this.UCSys1.AddTD("<a href=\"javascript:WinOpen('EnsCfg.aspx?EnsName=" + en.ToString() + "&Type=Check','dtl') ; \" > Set up </a>");
            //    this.UCSys1.AddTD("<a href=\"javascript:WinOpen('EnsAppCfg.aspx?EnsName=" + en.ToString() + "s&Type=Check','dtl') ; \" > Property set </a>");
                this.UCSys1.AddTREnd();
            }
            this.UCSys1.AddTableEnd();
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

		private void BPToolBar1_ButtonClick(object sender, System.EventArgs e)
		{
			try
			{
				ToolbarBtn btn = (ToolbarBtn)sender;
				switch(btn.ID)
				{
					case NamesOfBtn.Search:
						this.Bind();
						return;
					case NamesOfBtn.Export:
						this.ExportEntityToExcel(this.TB_EnsName.Text);
						return;
					case NamesOfBtn.FileManager:
						this.ResponseWriteBlueMsg(this.GenerCreateTableSQL(this.TB_EnsName.Text));
						return;
                    case NamesOfBtn.Card:
                        return;
					default:
						throw new Exception("error");
				}
			}
			catch(Exception ex)
			{
				this.ResponseWriteRedMsg(ex);
			}
		}
        /// <summary>
        ///  Add comments for each field 
        /// </summary>
        public void AddNote()
        { 

        }
	}
}
