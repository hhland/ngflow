using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.Sys;
using BP.Port;
using BP.Web.Controls;
using BP.DA;
using BP.En;
using BP.Web;

namespace CCFlow.WF.CCForm
{
    public partial class DtlOpSelectItems : BP.Web.WebPage
    {

        public string getUTF8ToString(string param)
        {
            return Server.UrlDecode(Request[param]);
        }

        #region  Property .
        public string SKey
        {
            get
            {
                return getUTF8ToString("SKey");
            }
        }
        public Int64 WorkID
        {
            get
            {
                return Int64.Parse( this.Request.QueryString["WorkID"]);
            }
        }
        public string FK_MapDtl
        {
            get
            {
                return this.Request.QueryString["FK_MapDtl"];
            }
        }
        #endregion  Property .

        protected void Page_Load(object sender, EventArgs e)
        {

            if (this.DoType != null)
                return;

            MapDtl dtl = new BP.Sys.MapDtl(this.FK_MapDtl);
            this.Title = dtl.Name;
            this.Label1.Text = dtl.Name;

            this.Pub1.Add("&nbsp;&nbsp; Please enter a keyword :");
            TextBox tb = new TextBox();
            tb.ID = "TB_Key";
            tb.Text = this.SKey;
            this.Pub1.Add(tb);

            Button btn = new Button();
            btn.ID = "Btn1";
            btn.Text = " Inquiry ";
            btn.Click += new EventHandler(btn_Click);
            this.Pub1.Add(btn);

            #region  Display data .
            string key = this.Pub1.GetTextBoxByID("TB_Key").Text.Trim();
            string sql = "";
            if (this.SKey == null)
            {
                sql = dtl.ImpSQLInit.Clone() as string;
                sql = sql.Replace("~", "'");
            }
            else
            {
                sql = dtl.ImpSQLSearch.Clone() as string;
                sql = sql.Replace("@Key", key);
                sql = sql.Replace("~", "'");
            }

            sql = sql.Replace("@WebUser.No", WebUser.No);
            sql = sql.Replace("@WebUser.Name", WebUser.Name);
            sql = sql.Replace("@WebUser.FK_Dept", WebUser.FK_Dept);
            sql = sql.Replace("@WebUser.FK_DeptName", WebUser.FK_DeptName);

            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);

            this.BindTableMulti(dt);
            // Increase the boot process .
            Button button = new Button();
            button.ID = "Btn_Pub1";
            button.Text = " Add ";
            button.Click += new EventHandler(btn_Pub_Click);
            this.Pub1.Add(button);

            button = new Button();
            button.ID = "Btn_Pub2";
            button.Text = " Add and Close ";
            button.Click += new EventHandler(btn_Pub_Click);
            this.Pub1.Add(button);

            #endregion  Display data .
        }
        /// <summary>
        ///  Execute the query .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btn_Click(object sender, EventArgs e)
        {
            string key = this.Pub1.GetTextBoxByID("TB_Key").Text.Trim();
            key = Server.UrlEncode(key);
            this.Response.Redirect("DtlOpSelectItems.aspx?SKey=" + key + "&WorkID=" + this.WorkID + "&FK_MapDtl=" + this.FK_MapDtl, false);
        }
        /// <summary>
        ///  Initialization data 
        /// </summary>
        public void BindTableMulti(DataTable dt)
        {
            string pksVal = "no";
            string pksLab = "name";

            this.Pub2.AddTable("width='100%'");
            this.Pub2.AddTR();
            this.Pub2.AddTDTitle("IDX");
            this.Pub2.AddTDTitle(" Select all ");
            foreach (DataColumn dc in dt.Columns)
            {
                switch (dc.ColumnName.ToLower())
                {
                    case "ctitle":
                    case "cworkID":
                        pksVal = "CWorkID";
                        pksLab = "CTitle";
                        continue;
                    case "no":
                    case "name":
                        pksVal = "no";
                        pksLab = "name";
                        continue;
                    default:
                        break;
                }
                this.Pub2.AddTDTitle(dc.ColumnName);
            }
            this.Pub2.AddTREnd();

            //  Output Data .
            int idx = 0;
            foreach (DataRow dr in dt.Rows)
            {
                idx++;
                this.Pub2.AddTR();
                this.Pub2.AddTDIdx(idx);

                // Join Select .
                CheckBox cb = new CheckBox();
                cb.ID = "CB_" + dr[pksVal].ToString();
                cb.Text = dr[pksLab].ToString();
                this.Pub2.AddTD(cb);

                foreach (DataColumn dc in dt.Columns)
                {
                    switch (dc.ColumnName.ToLower())
                    {
                        case "ctitle":
                        case "ctorkID":
                        case "no":
                        case "name":
                            continue;
                        default:
                            break;
                    }

                    string val = dr[dc.ColumnName].ToString();
                    if (val == null)
                        val = "";
                    this.Pub2.AddTD(val);
                }
                this.Pub2.AddTREnd();
            }
            this.Pub2.AddTableEnd();
        }

        void btn_Pub_Click(object sender, EventArgs e)
        {
            #region  Get selected ID
            string ids = "";
            foreach (Control ctl in this.Pub2.Controls)
            {
                if (ctl == null || ctl.ID == null || ctl.ID.Contains("CB_") == false)
                    continue;

                CheckBox cb = ctl as CheckBox;
                if (cb == null)
                    continue;

                if (cb.Checked == false)
                    continue;

                ids += ctl.ID.Replace("CB_", "") + ",";
            }
            if (string.IsNullOrEmpty(ids) == true)
            {
                BP.Sys.PubClass.Alert(" You do not have to select items .");
                return;
            }
            this.Alert(" Successfully :" + ids + " Into a data table ...");
            #endregion  Get selected ID

            #region  Get the data .
            MapDtl dtl = new BP.Sys.MapDtl(this.FK_MapDtl);
            string sql = "";
            DataTable dt = null;
            try
            {
                sql = dtl.ImpSQLFull.Clone() as string;
                ids = ids.Replace(",", "','");
                sql = sql.Replace("@Keys", ids.Substring(0, ids.Length - 3));
                dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
                //当ID是int Type is likely to throw an exception .
            }
            catch
            {
                sql = dtl.ImpSQLFull.Clone() as string;
                sql = sql.Replace("@Keys", ids.Substring(0, ids.Length - 2));
                dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            }
            #endregion  Get the data .

            #region  The data in the schedule .
            GEDtl gedtl = new BP.Sys.GEDtl(this.FK_MapDtl);
            foreach (DataRow dr in dt.Rows)
            {
                gedtl.RefPK = this.WorkID.ToString();
                foreach (DataColumn dc in dt.Columns)
                {
                    // Assignment .
                    gedtl.SetValByKey(dc.ColumnName, dr[dc.ColumnName]);
                }
                gedtl.InsertAsOID(BP.DA.DBAccess.GenerOID());
            }
            #endregion  The data in the schedule .
             
            Button btn = sender as Button;
            if (btn.ID == "Btn_Pub1")
            {
                this.Alert(" Into success ");
            }
            else
            {
                this.WinCloseWithMsg(" Into success ");
            }
             
        }
    }
}