using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF.Template;
using BP.WF;
using BP.DA;
using BP.Port;
using BP.Web;

namespace CCFlow.WF
{
    public partial class StartGuide : BP.Web.WebPage
    {
        #region  Property .
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        public string SKey
        {
            get
            {
                return this.Request.QueryString["SKey"];
            }
        }
        #endregion  Property .

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.DoType != null)
                return;

            this.Pub1.Add("<b> Please enter a keyword :</b>");
            TextBox tb = new TextBox();
            tb.ID = "TB_Key";
            tb.Text = this.SKey;
            this.Pub1.Add(tb);
            this.Pub1.AddTD();

            ImageButton imgbtn = new ImageButton();
            imgbtn.ID = "imgbtn1";
            imgbtn.ImageUrl = "./Img/Search.gif";
            imgbtn.Click += new ImageClickEventHandler(btn_Click);
            this.Pub1.Add(imgbtn);
            //Button btn = new Button();
            //btn.ID = "Btn1";
            //btn.Text = " Inquiry ";
            //btn.Attributes.Add("CssClass", "Img");
            //btn.Click += new EventHandler(btn_Click);
            //this.Pub1.Add(btn);
            this.Pub1.AddTD();
            //Button button = new Button();
            //button.ID = "Btn_Sav2";
            //button.Text = " Start the process ";
            //button.Click += new EventHandler(btn_Start_Click);
            //this.Pub1.Add(button);
            ImageButton imgbtnsav = new ImageButton();
            imgbtnsav.ID = "imgbtn2";
            imgbtn.ImageUrl = "./Img/Start.gif";

            imgbtnsav.Click += new ImageClickEventHandler(btn_Start_Click);
            this.Pub1.Add(imgbtnsav);

            #region  Display data .
            string key = this.Pub1.GetTextBoxByID("TB_Key").Text.Trim();
            Flow fl = new Flow(this.FK_Flow);
            string sql = "";
            if (this.SKey == null)
            {
                sql = fl.StartGuidePara2.Clone() as string;
                sql = sql.Replace("~", "'");
            }
            else
            {
                sql = fl.StartGuidePara1.Clone() as string;
                sql = sql.Replace("@Key", key);
                sql = sql.Replace("~", "'");
            }

            sql = sql.Replace("@WebUser.No", WebUser.No);
            sql = sql.Replace("@WebUser.Name", WebUser.Name);
            sql = sql.Replace("@WebUser.FK_Dept", WebUser.FK_Dept);
            sql = sql.Replace("@WebUser.FK_DeptName", WebUser.FK_DeptName);

            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);

            switch (fl.StartGuideWay)
            {
                case StartGuideWay.BySystemUrlOne:
                case StartGuideWay.BySystemUrlOneEntity:
                      this.BindTableOne(dt);
                    break;
                case StartGuideWay.ByHistoryUrl: // Historical Data .
                    if (dt.Rows.Count == 0)
                    {
                        string url = "MyFlow.aspx?FK_Flow=" + this.FK_Flow + "&FK_Node=" + int.Parse(this.FK_Flow) + "01&WorkID=0&IsCheckGuide=1";
                        this.Response.Redirect(url, true);
                        //this.BindTableOne(dt);
                    }
                    else
                        this.BindTableOne(dt);
                    break;
                default:
                    this.BindTableMulti(dt);
                    break;
            }
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
            //if (string.IsNullOrEmpty(key))
            //{
            //    this.Alert(" Please enter a keyword ...");
            //    return;
            //}
            this.Response.Redirect("StartGuide.aspx?FK_Flow=" + this.FK_Flow + "&SKey=" + key, true);

            //Flow fl = new Flow(this.FK_Flow);
            //string sql = fl.StartGuidePara1.Clone() as string;
            //sql = sql.Replace("@Key", key);
            //sql = sql.Replace("~", "'");
            //DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            //if (fl.StartGuideWay == StartGuideWay.BySystemUrlOne)
            //    this.BindTableOne(dt);
            //else
            //    this.BindTableMulti(dt);
        }
        /// <summary>
        ///  Initialization data 
        /// </summary>
        public void BindTableOne(DataTable dt)
        {
            this.Pub2.AddTable("width='100%'");
            this.Pub2.AddTR();
            this.Pub2.AddTDTitle("IDX");
            foreach (DataColumn dc in dt.Columns)
            {
                switch (dc.ColumnName.ToLower())
                {
                    case "pflowno":
                    case "pworkID":
                    case "no":
                    case "name":
                        continue;
                    default:
                        break;
                }
                this.Pub2.AddTDTitle(dc.ColumnName);
            }
            this.Pub2.AddTREnd();

            Flow fl=new Flow(this.FK_Flow);

            string url = "MyFlow.aspx?FK_Flow=" + this.FK_Flow + "&FK_Node=" + int.Parse(this.FK_Flow) + "01&WorkID=0&IsCheckGuide=1";
            //  Output Data .
            int idx = 0;
            foreach (DataRow dr in dt.Rows)
            {
                idx++;
                this.Pub2.AddTR();
                this.Pub2.AddTDIdx(idx);

                string paras = url + "";
                foreach (DataColumn dc in dt.Columns)
                    paras += "&" + dc.ColumnName + "=" + dr[dc.ColumnName];

                int i = 0;
                foreach (DataColumn dc in dt.Columns)
                {
                    switch (dc.ColumnName.ToLower())
                    {
                        case "pflowno":
                        case "pworkID":
                        case "no":
                        case "name":
                            continue;
                        default:
                            break;
                    }

                    string val = dr[dc.ColumnName].ToString();
                    if (val == null)
                        val = "";

                    i++;
                    // Output Connections .
                    if (i == 1)
                    {
                        if (fl.StartGuideWay == StartGuideWay.ByHistoryUrl)
                            this.Pub2.AddTD("<a href='" + url + "&CopyFormWorkID=" + dr["OID"] + "&CopyFormNode=" + int.Parse(this.FK_Flow) + "01' >" + val + "</a>");
                        else
                            this.Pub2.AddTD("<a href='" + paras + "' >" + val + "</a>");
                    }
                    else
                    {
                        this.Pub2.AddTD(val);
                    }
                }
                this.Pub2.AddTREnd();
            }
            this.Pub2.AddTableEnd();
        }
        /// <summary>
        ///  Initialization data 
        /// </summary>
        public void BindTableMulti(DataTable dt)
        {
            //if (dt.Columns.Contains("CTitle") == false || dt.Columns.Contains("CWorkID") == false)
            //{
            //    this.Pub2.AddFieldSetRed(" Navigation parameter setting error ", " Lack CFlowNo,CWorkID列.");
            //    return;
            //}

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

        void btn_Start_Click(object sender, EventArgs e)
        {
            string cWorkID = "";
            foreach (Control ctl in this.Pub2.Controls)
            {
                if (ctl == null || ctl.ID == null || ctl.ID.Contains("CB_") == false)
                    continue;

                CheckBox cb = ctl as CheckBox;
                if (cb == null)
                    continue;

                if (cb.Checked == false)
                    continue;

                cWorkID += ctl.ID.Replace("CB_", "") + ",";
            }
            if (string.IsNullOrEmpty(cWorkID) == true)
            {
                BP.Sys.PubClass.Alert(" You do not have to select items .");
                return;
            }

            Flow fl = new Flow(this.FK_Flow);
            string url = "MyFlow.aspx?FK_Flow=" + this.FK_Flow + "&FK_Node=" + int.Parse(this.FK_Flow) + "01&WorkID=0&IsCheckGuide=1";

            // Agreed parameters necessary system .
            switch (fl.StartGuideWay)
            {
                case StartGuideWay.BySystemUrlMulti:
                case StartGuideWay.BySystemUrlOne:
                    url += "&DoFunc=SetParentFlow&WorkIDs=" + cWorkID + "&CFlowNo=" + fl.StartGuidePara3;
                    break;
                case StartGuideWay.BySystemUrlMultiEntity:
                case StartGuideWay.BySystemUrlOneEntity:
                    url += "&DoFunc=" + fl.StartGuideWay.ToString() + "&Nos=" + cWorkID + "&StartGuidePara3=" + fl.StartGuidePara3;
                    break;
                default:
                    break;
            }
            this.Response.Redirect(url, true);

        }
    }
}