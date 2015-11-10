using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.DA;
using System.Data;
using System.Web.UI.HtmlControls;

namespace CCFlow.WF
{
    public partial class MyFlow_ModifyOldDataSelect : System.Web.UI.Page
    {
        public string FK_Flow
        {
            get
            {
                string s = this.Request.QueryString["FK_Flow"];
                if (string.IsNullOrEmpty(s))
                    throw new Exception("@ Process ID parameter error ...");

                return BP.WF.Dev2Interface.TurnFlowMarkToFlowNo(s);
            }
        }

        public string FK_Node
        {
            get
            {
                string s = this.Request.QueryString["FK_Node"];
                if (string.IsNullOrEmpty(s))
                    throw new Exception("@ Process ID parameter error ...");

                return s;
            }
        }

        public int oidIndex = 0;

        string oldWorkid_SQL;
        protected void Page_Load(object sender, EventArgs e)
        {
            oldWorkid_SQL = DBAccess.RunSQLReturnString(string.Format("SELECT OldWorkID_SQL FROM WF_Flow where no={0}", this.FK_Flow));

            if (string.IsNullOrEmpty(oldWorkid_SQL))
            {
                string tsql = string.Format("select PTABLE from SYS_MAPDATA where NO='ND{0}'", FK_Node);
                string ptable = DBAccess.RunSQLReturnString(tsql);
                oldWorkid_SQL = string.Format("select * from {0}", ptable);
            }

            bindSearch();


            if (!IsPostBack)
            {
                bindGridView();
            }
        }

        protected void bindGridView()
        {
            string whereStr = getWhereStr();
            
            DataTable dtData = DBAccess.RunSQLReturnTable(string.Format("select * from ({0}) where rownum<10 {1}", oldWorkid_SQL, whereStr));

            if (!IsPostBack)
            {
                for (int i = 0; i < dtData.Columns.Count; i++)
                {
                    string colName = dtData.Columns[i].ColumnName.ToString();

                    BoundField bf = new BoundField();
                    bf.ShowHeader = true;
                    bf.HeaderText = colName;
                    bf.DataField = colName;
                    GVData.Columns.Add(bf);
                }
            }
            

            GVData.PageSize = dtData.Rows.Count + 1;
            GVData.DataSource = null;
            GVData.DataSource = dtData;
            GVData.DataBind();
        }

        protected void bindSearch()
        {
            DataTable dtData = DBAccess.RunSQLReturnTable(string.Format("select * from ({0}) where rownum<0", oldWorkid_SQL));


            for (int i = 0; i < dtData.Columns.Count; i++)
            {
                string colName = dtData.Columns[i].ColumnName.ToString();

                HtmlTableRow tr = new HtmlTableRow();
                tbSearch.Rows.Add(tr);

                HtmlTableCell td_Txt = new HtmlTableCell();
                td_Txt.InnerText = colName + ":";
                td_Txt.Attributes.Add("class", "tdLeft");
                tr.Cells.Add(td_Txt);

                HtmlTableCell tdCol_Txt = new HtmlTableCell();
                tdCol_Txt.Attributes.Add("class", "tdRight");
                TextBox txt = new TextBox();
                txt.ID = "txt" + i.ToString();
                tdCol_Txt.Controls.Add(txt);
                tr.Cells.Add(tdCol_Txt);

                if (colName.ToUpper().Trim() == "OID")
                {
                    oidIndex = i + 1;
                }
            }
        }

        protected void GVData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#339933'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c;");
            e.Row.Attributes.Add("onclick", "CheckSelect_Row(GVData,this);");
        }

        protected string getWhereStr()
        {
            string whereStr = "";
            for (int i = 0; i < tbSearch.Rows.Count; i++)
            {
                TextBox txt = (TextBox)tbSearch.Rows[i].Cells[1].Controls[0];
                if (txt.Text != "")
                {
                    whereStr += " and ";
                    whereStr += string.Format("\"{0}\" like '%{1}%'", tbSearch.Rows[i].Cells[0].InnerText.Substring(0,tbSearch.Rows[i].Cells[0].InnerText.Length-1), txt.Text.Replace("'", "''"));
                }
            }
            return whereStr;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            bindGridView();
        }
    }
}