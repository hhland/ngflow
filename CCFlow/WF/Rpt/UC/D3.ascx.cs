using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.DA;
using BP.Web.Controls;
using BP.En;
using BP.WF;
using BP.Web;
using BP.Sys;
using BP.Port;
using BP.WF.Data;
using BP;
using BP.Web.Comm;

namespace CCFlow.WF.Rpt.UC
{
    public partial class D3 : BP.Web.UC.UCBase3
    {
        #region  Property .
        /// <summary>
        ///  No. Name 
        /// </summary>
        public string RptNo
        {
            get
            {
                string s = this.Request.QueryString["RptNo"];
                if (string.IsNullOrEmpty(s))
                    return "ND68MyRpt";
                return s;
            }
        }
        public string FK_Flow
        {
            get
            {
                string s = this.Request.QueryString["FK_Flow"];
                if (string.IsNullOrEmpty(s))
                    return "068";
                return s;
            }
        }
        public Entities _HisEns = null;
        public Entities HisEns
        {
            get
            {
                if (_HisEns == null)
                {
                    if (this.RptNo != null)
                    {
                        if (this._HisEns == null)
                            _HisEns = BP.En.ClassFactory.GetEns(this.RptNo);
                    }
                }
                return _HisEns;
            }
        }
        public BP.WF.Rpt.MapRpt currMapRpt = null;
        #endregion  Property .

        #region  Property 
        /// <summary>
        /// key
        /// </summary>
        public   string Key
        {
            get
            {
                try
                {
                    return this.ToolBar1.GetTBByID("TB_Key").Text;
                }
                catch
                {
                    return null;
                }
            }
        }
        public UserRegedit ur = null;
        /// <summary>
        ///  Whether pagination 
        /// </summary>
        public bool IsFY
        {
            get
            {
                string str = this.Request.QueryString["IsFY"];
                if (str == null || str == "0")
                    return false;
                return true;
            }
        }
        public string NumKey
        {
            get
            {
                string str = this.Request.QueryString["NumKey"];
                if (str == null)
                    return ViewState["NumKey"] as string;
                else
                    return str;
            }
            set
            {
                ViewState["NumKey"] = value;
            }
        }
        public string OrderBy
        {
            get
            {
                string str = this.Request.QueryString["OrderBy"];
                if (str == null)
                    return ViewState["OrderBy"] as string;
                else
                    return str;
            }
            set
            {
                ViewState["OrderBy"] = value;
            }
        }
        public string OrderWay
        {
            get
            {
                string str = this.Request.QueryString["OrderWay"];
                if (str == null)
                    return ViewState["OrderWay"] as string;
                else
                    return str;
            }
            set
            {
                ViewState["OrderWay"] = value;
            }
        }
        public bool IsReadonly
        {
            get
            {
                string i = this.Request.QueryString["IsReadonly"];
                if (i == "1")
                    return true;
                else
                    return false;
            }
        }
        public bool IsShowSum
        {
            get
            {
                string i = this.Request.QueryString["IsShowSum"];
                if (i == "1")
                    return true;
                else
                    return false;
            }
        }
        public bool IsContainsNDYF
        {
            get
            {
                if (this.ViewState["IsContinueNDYF"].ToString().ToUpper() == "TRUE")
                    return true;
                else
                    return false;
            }
        }
        public string CfgVal
        {
            get
            {
                return "";
                //return this.ViewState["CfgVal"].ToString();
            }
            set
            {
                this.ViewState["CfgVal"] = value;
            }
        }
        #endregion

        #region  Controls 
        public CheckBox CB_IsImg
        {
            get
            {
                return this.Left.GetCBByID("CB_IsImg");
            }
        }
        /// <summary>
        ///  Filtering empty data .
        /// </summary>
        public CheckBox CB_IsNull
        {
            get
            {
                return this.Left.GetCBByID("CB_IsNull");
            }
        }

        public CheckBox CB_IsRate
        {
            get
            {
                return this.Left.GetCBByID("CB_IsRate");
            }
        }
        public DDL DDL_Num
        {
            get
            {
                return this.Left.GetDDLByID("DDL_Num");
            }
        }
        public DDL DDL_FXWay
        {
            get
            {
                return this.Left.GetDDLByID("DDL_FXWay");
            }
        }
        public DDL DDL_D1
        {
            get
            {
                return this.Left.GetDDLByID("DDL_D1");
            }
        }
        public DDL DDL_D1_Order
        {
            get
            {
                return this.Left.GetDDLByID("DDL_D1_Order");
            }
        }

        public DDL DDL_D2
        {
            get
            {
                return this.Left.GetDDLByID("DDL_D2");
            }
        }
        public DDL DDL_D2_Order
        {
            get
            {
                return this.Left.GetDDLByID("DDL_D2_Order");
            }
        }
        #endregion

        public MapAttrs currMapAttrs = null;
        public Entity currEn = null; 

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ur = new UserRegedit(WebUser.No, this.RptNo + "_D3");
            this.currMapAttrs = new MapAttrs(this.RptNo);
            this.currEn = this.HisEns.GetNewEntity;

            #region  Processing queries Permissions 
           // this.Page.RegisterClientScriptBlock("sss",
           //"<link href='/WF/Comm/Style/Table" + BP.Web.WebUser.Style + ".css' rel='stylesheet' type='text/css' />");

            currMapRpt = new BP.WF.Rpt.MapRpt(this.RptNo, this.FK_Flow);
            Entity en = this.HisEns.GetNewEntity;
            Flow fl = new Flow(this.currMapRpt.FK_Flow);

            // Initialization Query toolbar .
            this.ToolBar1.InitToolbarOfMapRpt(fl, currMapRpt, this.RptNo, en, 1);
            this.ToolBar1.AddLinkBtn(BP.Web.Controls.NamesOfBtn.Export);

            // Go increase .
            this.ToolBar1.Add("&nbsp;");
            DDL ddl = new DDL();
            ddl.ID = "GoTo";
            ddl.Items.Add(new ListItem(" Inquiry ", "Search"));
            ddl.Items.Add(new ListItem(" Advanced Search ", "SearchAdv"));
            ddl.Items.Add(new ListItem(" Subgroup analysis ", "Group"));
            ddl.Items.Add(new ListItem(" Cross report ", "D3"));
            ddl.Items.Add(new ListItem(" Comparative analysis ", "Contrast"));
            ddl.SetSelectItem(this.PageID);
            this.ToolBar1.AddDDL(ddl);
            ddl.AutoPostBack = true;
            ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged_GoTo);

            this.ToolBar1.GetLinkBtnByID(NamesOfBtn.Search).Click += new System.EventHandler(this.ToolBar1_ButtonClick);
            this.ToolBar1.GetLinkBtnByID(NamesOfBtn.Export).Click += new System.EventHandler(this.ToolBar1_ButtonClick);
            #endregion  Processing queries Permissions 

            // Deal with left.
            this.InitLeft();

            if (this.IsPostBack == false)
                this.BindDG();
        }
        void ddl_SelectedIndexChanged_GoTo(object sender, EventArgs e)
        {
            DDL ddl = sender as DDL;
            string item = ddl.SelectedItemStringVal;

            string tKey = DateTime.Now.ToString("MMddhhmmss");
            this.Response.Redirect(item + ".aspx?RptNo=" + this.RptNo + "&FK_Flow=" + this.FK_Flow + "&T=" + tKey, true);
        }
        /// <summary>
        ///  Initialization left.
        /// </summary>
        public void InitLeft()
        {
            this.Left.AddTable("class='Table' cellspacing='0' cellpadding='0' border='0' style='width:100%'");
            string paras = this.ur.Paras;

            #region  Options 
            this.Left.AddTR();
            this.Left.AddTDGroupTitle("colspan=2", " Options ");
            this.Left.AddTREnd();

            DDL ddl = new DDL();
            ddl.ID = "DDL_Num";
            ddl.AutoPostBack = true;
            ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged);


            ListItem li = null;
            this.Left.AddTR();
            this.Left.AddTD(" Analysis of data :");
            Attrs attrs = this.currEn.EnMap.Attrs; ;
            attrs.AddTBInt("MyNum", 1, " Process number ", true, true);
            foreach (Attr attr in attrs)
            {
                if (attr.UIContralType != UIContralType.TB)
                    continue;

                if (attr.UIVisible == false)
                    continue;

                if (attr.IsNum == false)
                    continue;

                // Remove the special field .
                switch (attr.Key)
                {
                    case NDXRptBaseAttr.OID:
                    case "MID":
                    case NDXRptBaseAttr.PWorkID:
                    case NDXRptBaseAttr.FID:
                    case NDXRptBaseAttr.FlowEndNode:
                    case "WorkID":
                        continue;
                    default:
                        break;
                }
                 

                li = new ListItem(attr.Desc, attr.Key);
                if (paras.Contains("@Num=" + attr.Key))
                    li.Selected = true;
                ddl.Items.Add(li);
            }
            this.Left.AddTD(ddl);
            this.Left.AddTREnd();

            this.Left.AddTR();
            this.Left.AddTD(" Analytical methods :");
            ddl = new DDL();
            ddl.ID = "DDL_FXWay";
            ddl.AutoPostBack = true;

            li = new ListItem(" Sum ", "SUM");
            if (paras.Contains("@FXWay=SUM"))
                li.Selected = true;
            ddl.Items.Add(li);

            li = new ListItem(" Averaging ", "AVG");
            if (paras.Contains("@FXWay=AVG"))
                li.Selected = true;
            ddl.Items.Add(li);
            ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged);
            this.Left.AddTD(ddl);
            this.Left.AddTREnd();


            this.Left.AddTR();
            CheckBox cb = new CheckBox();
            cb.AutoPostBack = true;
            cb.ID = "CB_IsRate";
            cb.Text = " Shows the percentage of ";
            if (paras.Contains("@IsRate=1"))
                cb.Checked = true;
            else
                cb.Checked = false;

            cb.CheckedChanged += new EventHandler(cb_CheckedChanged);
            this.Left.AddTD(cb);

            cb = new CheckBox();
            cb.ID = "CB_IsImg";
            cb.AutoPostBack = true;
            if (paras.Contains("@IsImg=1"))
                cb.Checked = true;
            else
                cb.Checked = false;

            cb.Text = " Display graphics ";
            cb.CheckedChanged += new EventHandler(cb_CheckedChanged);
            this.Left.AddTD(cb);
            this.Left.AddTREnd();


            AtPara ps = new AtPara(paras);
            this.Left.AddTR();
            this.Left.AddTDBegin();
            TextBox tb = new TextBox();
            tb.Width = 40;
            tb.ID = "TB_W";
            tb.BorderWidth = 1;
            //tb.BorderStyle = BorderStyle.Outset;
            tb.Text = ps.GetValStrByKey("W");
            if (string.IsNullOrEmpty(tb.Text))
                tb.Text = "500";

            this.Left.Add("宽:");
            this.Left.Add(tb);
            this.Left.AddTDEnd();

            this.Left.AddTDBegin();
            this.Left.Add("高:");
            tb = new TextBox();
            tb.Width = 40;
            tb.ID = "TB_H";
            tb.BorderWidth = 1;
            //tb.BorderStyle = BorderStyle.Outset;
            tb.Text = ps.GetValStrByKey("H");
            if (string.IsNullOrEmpty(tb.Text))
                tb.Text = "300";

            this.Left.Add(tb);
            this.Left.AddTDEnd();
            this.Left.AddTREnd();


            this.Left.AddTR();
            cb = new CheckBox();
            cb.AutoPostBack = true;
            cb.ID = "CB_IsNull";
            cb.Text = " Filtering for null Data values ";
            if (paras.Contains("@IsNull=1"))
                cb.Checked = true;
            else
                cb.Checked = false;
            cb.CheckedChanged += new EventHandler(cb_CheckedChanged);
            this.Left.AddTD("colspan=2", cb);
            this.Left.AddTREnd();
            #endregion  Options 

            #region  Latitude Cross 
            this.Left.AddTR();
            this.Left.AddTDGroupTitle("colspan='2'", " Latitude Cross  - <a href=# > Anyway swap </a>");
            this.Left.AddTREnd();

            ddl = new DDL();
            ddl.ID = "DDL_D1";
            ddl.AutoPostBack = true;
            ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged);
            this.Left.AddTR();
            this.Left.AddTD(" Latitude project :");
            foreach (Attr attr in attrs)
            {
                if (attr.UIContralType == UIContralType.DDL)
                {
                    li = new ListItem(attr.Desc, attr.Key);

                    if (paras.Contains("@D1=" + attr.Key))
                        li.Selected = true;

                    ddl.Items.Add(new ListItem(attr.Desc, attr.Key));
                }
            }
            this.Left.AddTD(ddl);
            this.Left.AddTREnd();

            ddl = new DDL();
            ddl.ID = "DDL_D1_Order";
            ddl.AutoPostBack = true;
            ddl.Items.Add(new ListItem(" Ascending ", "Up"));
            ddl.Items.Add(new ListItem(" Descending ", "Desc"));
            if (paras.Contains("@D1_Order=Up"))
                li.Selected = true;
            else
                li.Selected = false;

            this.Left.AddTR();
            this.Left.AddTD(" Sort by :");
            this.Left.AddTD(ddl);
            this.Left.AddTREnd();
            #endregion  Latitude Cross 

            #region  Longitudinal latitude 
            this.Left.AddTR();
            this.Left.AddTDGroupTitle("colspan='2'", " Longitudinal latitude ");
            this.Left.AddTREnd();

            this.Left.AddTR();
            this.Left.AddTD(" Data Project :");
            ddl = new DDL();
            ddl.ID = "DDL_D2";
            ddl.AutoPostBack = true;
            ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged);
            foreach (Attr attr in attrs)
            {
                if (attr.UIContralType == UIContralType.DDL)
                {
                    li = new ListItem(attr.Desc, attr.Key);
                    if (paras.Contains("@D2=" + attr.Key))
                        li.Selected = true;
                    ddl.Items.Add(new ListItem(attr.Desc, attr.Key));
                }
            }
            this.Left.AddTD(ddl);
            this.Left.AddTREnd();

            ddl = new DDL();
            ddl.ID = "DDL_D2_Order";
            ddl.AutoPostBack = true;
            ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged);
            ddl.Items.Add(new ListItem(" Ascending ", ""));
            ddl.Items.Add(new ListItem(" Descending ", "DESC"));
            if (paras.Contains("@D2_Order=Up"))
                li.Selected = true;
            else
                li.Selected = false;
            this.Left.AddTR();
            this.Left.AddTD(" Sort by :");
            this.Left.AddTD(ddl);
            this.Left.AddTREnd();
            #endregion  Longitudinal latitude 

            this.Left.AddTableEnd();

            //#region  Longitudinal latitude 1
            //this.Left.AddTR();
            //this.Left.AddTDTitle("colspan=2", " Longitudinal latitude 2");
            //this.Left.AddTREnd();

            //this.Left.AddTR();
            //this.Left.AddTD(" Data Project ");
            //ddl = new DDL();
            //ddl.ID = "DDL_D3";
            //foreach (MapAttr attr in this.HisMAs)
            //{
            //    if (attr.UIBindKey == "")
            //        continue;
            //    ddl.Items.Add(new ListItem(attr.Name, attr.KeyOfEn));
            //}
            //this.Left.AddTD(ddl);
            //this.Left.AddTREnd();

            //ddl = new DDL();
            //ddl.ID = "DDL_D3_Order";
            //ddl.Items.Add(new ListItem(" Ascending ", ""));
            //ddl.Items.Add(new ListItem(" Descending ", "DESC"));
            //this.Left.AddTR();
            //this.Left.AddTD(" Sort by ");
            //this.Left.AddTD(ddl);
            //this.Left.AddTREnd();
            //#endregion  Longitudinal latitude 2

        }
        public DataTable BindDG()
        {
            // The data source is correct .
            if (this.DDL_D1.Items.Count <= 1 || this.DDL_Num.Items.Count == 0)
            {
                this.Right.Clear();
                this.ToolBar1.Visible = false;
                this.Right.AddMsgGreen(" Prompt :", "<h2> There is not enough or no latitude data analysis projects .</h2>");
                return null;
            }

            // Select unanimously not let two dimensions .
            if (this.DDL_D1.SelectedItemStringVal == this.DDL_D2.SelectedItemStringVal)
            {
                if (this.DDL_D1.SelectedIndex == 0)
                    this.DDL_D2.SelectedIndex = 1;
                else
                    this.DDL_D2.SelectedIndex = 0;
            }

            Attrs attrs = this.currEn.EnMap.Attrs; 
            SysEnums sesD1 = null;
            Entities ensD1 = null;

            SysEnums sesD2 = null;
            Entities ensD2 = null;
            Map map = this.currEn.EnMap ;

            #region  Generate two latitude statements 
            Attr attrD1 = attrs.GetAttrByKey(this.DDL_D1.SelectedItemStringVal);
            if (attrD1.IsEnum)
                sesD1 = new SysEnums(attrD1.UIBindKey);
            else
            {
                ensD1 = attrD1.HisFKEns;
                if (ensD1.Count == 0)
                    ensD1.RetrieveAll();
            }

            Attr attrD2 = attrs.GetAttrByKey(this.DDL_D2.SelectedItemStringVal);
            if (attrD2.IsEnum)
                sesD2 = new SysEnums(attrD2.UIBindKey);
            else
            {
                ensD2 = attrD2.HisFKEns;
                if (ensD2.Count == 0)
                    ensD2.RetrieveAll();
            }
            #endregion


            #region  Generate original execution sql
            string Condition = ""; // Problem specific field conditions .
            Paras myps = new BP.DA.Paras();
            string sql = "SELECT " + attrD1.Key + "," + attrD2.Key + ", " + this.DDL_FXWay.SelectedItemStringVal + "(" + this.DDL_Num.SelectedItemStringVal + ") FROM " + map.PhysicsTable;
            //  Turn up  WHERE  Data .
            string where = " WHERE ";
            string whereOfLJ = " WHERE "; //  Cumulative where.
            string url = "";
            foreach (Control item in this.ToolBar1.Controls)
            {
                if (item.ID == null)
                    continue;
                if (item.ID.IndexOf("DDL_") == -1)
                    continue;
                if (item.ID.IndexOf("DDL_Form_") == 0 || item.ID.IndexOf("DDL_To_") == 0)
                    continue;

                string key = item.ID.Substring("DDL_".Length);
                DDL ddl = (DDL)item;
                if (ddl.SelectedItemStringVal == "all")
                    continue;

                string val = ddl.SelectedItemStringVal;
                if (val == null)
                    continue;

                if (val == "mvals")
                {
                    UserRegedit sUr = new UserRegedit();
                    sUr.MyPK = WebUser.No + this.RptNo + "_SearchAttrs";
                    sUr.RetrieveFromDBSources();

                    /*  If the value is multiple choice  */
                    string cfgVal = sUr.MVals;
                    AtPara ap = new AtPara(cfgVal);
                    string instr = ap.GetValStrByKey(key);
                    if (instr == null || instr == "")
                    {
                        if (key == "FK_Dept" || key == "FK_Unit")
                        {
                            if (key == "FK_Dept")
                            {
                                val = WebUser.FK_Dept;
                                ddl.SelectedIndex = 0;
                            }

                            if (key == "FK_Unit")
                            {
                                //  val = WebUser.FK_Unit;
                                ddl.SelectedIndex = 0;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        instr = instr.Replace("..", ".");
                        instr = instr.Replace(".", "','");
                        instr = instr.Substring(2);
                        instr = instr.Substring(0, instr.Length - 2);
                        where += " " + key + " IN (" + instr + ")  AND ";
                        continue;
                    }
                }

                if (key == "FK_Dept")
                {
                    if (val.Length == 8)
                    {
                        where += " FK_Dept =" + SystemConfig.AppCenterDBVarStr + "V_Dept    AND ";
                    }
                    else
                    {
                        switch (SystemConfig.AppCenterDBType)
                        {
                            case DBType.Oracle:
                            case DBType.Informix:
                                where += " FK_Dept LIKE '%'||:V_Dept||'%'   AND ";
                                break;
                            case DBType.MSSQL:
                            default:
                                where += " FK_Dept LIKE  " + SystemConfig.AppCenterDBVarStr + "V_Dept+'%'   AND ";
                                break;
                        }
                    }
                    myps.Add("V_Dept", val);
                }
                else
                {
                    where += " " + key + " =" + SystemConfig.AppCenterDBVarStr + key + "   AND ";
                    if (key != "FK_NY")
                        whereOfLJ += " " + key + " =" + SystemConfig.AppCenterDBVarStr + key + "   AND ";

                    myps.Add(key, val);
                }
            }
            #endregion

            #region  Plus  where like  Condition 
            try
            {
                string key = this.ToolBar1.GetTBByID("TB_Key").Text.Trim();
                if (key.Length > 1)
                {
                    string whereLike = "";

                    bool isAddAnd = false;
                    foreach (Attr likeKey in attrs)
                    {
                        if (likeKey.IsNum)
                            continue;
                        if (likeKey.IsRefAttr)
                            continue;

                        switch (likeKey.Field)
                        {
                            case "MyFileExt":
                            case "MyFilePath":
                            case "WebPath":
                                continue;
                            default:
                                break;
                        }


                        if (isAddAnd == false)
                        {
                            isAddAnd = true;
                            whereLike += "      " + likeKey.Field + " LIKE '%" + key + "%' ";
                        }
                        else
                        {
                            whereLike += "   AND   " + likeKey.Field + " LIKE '%" + key + "%'";
                        }
                    }
                    whereLike += "          ";
                    where += whereLike;
                }
            }
            catch
            {
            }
            #endregion

            #region  Plus date period .
            if (map.DTSearchWay != DTSearchWay.None)
            {
                string dtFrom = this.ToolBar1.GetTBByID("TB_S_From").Text.Trim();
                string dtTo = this.ToolBar1.GetTBByID("TB_S_To").Text.Trim();
                string field = map.DTSearchKey;
                if (map.DTSearchWay == DTSearchWay.ByDate)
                {
                    where += "( " + field + ">='" + dtFrom + " 01:01' AND " + field + "<='" + dtTo + " 23:59')     ";
                }
                else
                {
                    where += "(";
                    where += field + " >='" + dtFrom + "' AND " + field + "<='" + dtTo + "'";
                    where += ")";
                }
            }
            if (where == " WHERE ")
            {
                where = "" + Condition.Replace("and", "");
                whereOfLJ = "" + Condition.Replace("and", "");
            }
            else
            {
                where = where.Substring(0, where.Length - " AND ".Length) + Condition;
                whereOfLJ = whereOfLJ.Substring(0, whereOfLJ.Length - " AND ".Length) + Condition;
            }
            #endregion

            sql += where + " GROUP BY  " + attrD1.Key + "," + attrD2.Key;
            myps.SQL = sql;
            DataTable dt = DBAccess.RunSQLReturnTable(myps);

            string leftMsg = this.DDL_FXWay.SelectedItem.Text + ":" + this.DDL_Num.SelectedItem.Text;

            #region  Generate a table  -   Generation title 
            this.Right.AddTable("class='Table' cellspacing='0' cellpadding='0' border='0' style='width:100%'");
            this.Right.AddTR();
            this.Right.AddTDGroupTitle(leftMsg);
            if (sesD1 != null)
            {
                foreach (SysEnum se in sesD1)
                {
                    this.Right.AddTDGroupTitle(se.Lab);
                }
            }
            if (ensD1 != null)
            {
                if (ensD1.Count == 0)
                    ensD1.RetrieveAll();
                foreach (Entity en in ensD1)
                {
                    this.Right.AddTDGroupTitle(en.GetValStrByKey("Name"));
                }
            }
            this.Right.AddTREnd();
            #endregion . Generation title .

            #region  Generation cell 
            if (sesD2 != null)
            {
                foreach (SysEnum se in sesD2)
                {
                    this.Right.AddTR();
                    this.Right.AddTDGroupTitle(se.Lab);

                    if (sesD1 != null)
                    {
                        foreach (SysEnum seD1 in sesD1)
                        {
                            this.Right.AddTD("onclick='' ", this.GetIt(dt, seD1.IntKey.ToString(), se.IntKey.ToString()));
                        }
                    }

                    if (ensD1 != null)
                    {
                        foreach (Entity enD1 in ensD1)
                        {
                            this.Right.AddTD(this.GetIt(dt, enD1.GetValStrByKey("No"), se.IntKey.ToString()));
                        }
                    }
                    this.Right.AddTREnd();
                }
            }

            if (ensD2 != null)
            {
                foreach (Entity en in ensD2)
                {
                    this.Right.AddTR();
                    this.Right.AddTDGroupTitle(en.GetValStrByKey("Name"));

                    if (sesD1 != null)
                    {
                        foreach (SysEnum seD1 in sesD1)
                        {
                            this.Right.AddTD(this.GetIt(dt, seD1.IntKey.ToString(), en.GetValStrByKey("No")));
                        }
                    }
                    if (ensD1 != null)
                    {
                        foreach (Entity enD1 in ensD1)
                        {
                            this.Right.AddTD(this.GetIt(dt, enD1.GetValStrByKey("No"), en.GetValStrByKey("No")));
                        }
                    }
                    this.Right.AddTREnd();
                }
            }
            this.Right.AddTableEnd();
            #endregion . Generation cell .

            #region  Save state 
            string paras = "";
            if (this.CB_IsImg.Checked)
                paras = "@IsImg=1";
            else
                paras = "@IsImg=0";

            if (this.CB_IsRate.Checked)
                paras += "@IsRate=1";
            else
                paras += "@IsRate=0";

            if (this.CB_IsNull.Checked)
                paras += "@IsNull=1";
            else
                paras += "@IsNull=0";

            paras += "@Num=" + this.DDL_Num.SelectedItemStringVal;
            paras += "@FXWay=" + this.DDL_FXWay.SelectedItemStringVal;
            paras += "@D1=" + this.DDL_D1.SelectedItemStringVal;
            paras += "@D1_Order=" + this.DDL_D1_Order.SelectedItemStringVal;
            paras += "@D2=" + this.DDL_D2.SelectedItemStringVal;
            paras += "@D2_Order=" + this.DDL_D2_Order.SelectedItemStringVal;
            paras += "@W=" + this.Left.GetTextBoxByID("TB_W").Text;
            paras += "@H=" + this.Left.GetTextBoxByID("TB_H").Text;

            ur.CfgKey = this.RptNo + "_D3";
            ur.MyPK = WebUser.No + "_" + ur.CfgKey;
            ur.FK_Emp = WebUser.NoOfSessionID;
            ur.Paras = paras;
            ur.Save();
            #endregion

            return null;
        }
        public decimal GetIt(DataTable dt, string d1, string d2)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[0].ToString() == d1 && dr[1].ToString() == d2)
                    return decimal.Parse(dr[2].ToString());
            }
            return 0;
        }

        void cb_CheckedChanged(object sender, EventArgs e)
        {
            this.BindDG();
        }
        void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindDG();
        }
        void ToolBar1_ButtonClick(object sender, EventArgs e)
        {
            var btn = (LinkBtn)sender;

            switch (btn.ID)
            {
                case NamesOfBtn.Help:
                    break;
                case NamesOfBtn.Excel:
                    DataTable dt = this.BindDG();
                    //this.ExportDGToExcel(this.DealTable(dt), this.currEn.EnDesc);
                    return;
                default:
                    this.ToolBar1.SaveSearchState(this.RptNo, this.Key);
                    this.BindDG();
                    return;
            }
        }
    }
}