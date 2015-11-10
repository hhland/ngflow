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
using BP.DA;
using BP.En;
using BP.Web;
using BP.Web.Controls;
using BP.Sys; 
using System.Collections.Specialized;


namespace BP.Web.WF.Comm
{
	/// <summary>
	/// GroupEnsDtl 的摘要说明。
	/// </summary>
	public partial class GroupEnsDtl : WebPage
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (this.IsPostBack == false)
            {
                this.Label1.Text = this.GenerCaption("详细");
                this.BindData();
            }
		}
		/// <summary>
		/// 部门
		/// </summary>
		public string FK_Dept
		{
			get
			{
				return (string)ViewState["FK_Dept"];
			}
			set
			{
				this.ViewState["FK_Dept"]=value;
			}
		}
        public void BindData()
        {
            string ensname = this.Request.QueryString["EnsName"];
            Entities ens = ClassFactory.GetEns(ensname);
            QueryObject qo = new QueryObject(ens);
            string url = this.Request.RawUrl;
            string[] strs = url.Split('&');
            int i = 0;
            int strsLen = strs.Length;
            //string[] mystrs= ssd
            foreach (string str in strs)
            {
                string[] mykey = str.Split('=');
                string key = mykey[0];
                string val = mykey[1];

                if (key == "FK_Dept")
                {
                    /* 取出最长的部门值。*/
                    if (this.FK_Dept == null)
                    {
                        this.FK_Dept = val;
                    }
                    else
                    {
                        if (this.FK_Dept.Length > val.Length)
                        {
                        }
                        else
                        {
                            this.FK_Dept = val;
                        }
                    }
                }
            }

            //			if (this.Request.RawUrl.IndexOf("FK_Dept=")!=-1)
            //				strsLen=strsLen-1;

            foreach (string str in strs)
            {
                //string mystr = str.Replace("DDL_");
                if (str.IndexOf("EnsName") != -1)
                    continue;

                string[] mykey = str.Split('=');
                string key = mykey[0];
                string val = mykey[1];

                if (key == "FK_Dept")
                {
                    val = "all";
                }
                qo.AddWhere(key, val);
                i++;
                if (i <= strsLen - 2)
                    qo.addAnd();
            }

            /* 如果到了最后 */
            if (this.FK_Dept != null)
            {
                qo.addAnd();
                qo.AddWhere("FK_Dept", " = ", this.FK_Dept);
            }
            qo.DoQuery();
            this.UCSys1.DataPanelDtl(ens,null);
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
	}
}
