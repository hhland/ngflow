using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.DA;
using BP;
using BP.WF.Data;
using BP.WF;
using BP.En;
using BP.Web;
using BP.Web.Controls;
using BP.Sys;
using BP.WF.Rpt;
using BP.Sys.Xml;

namespace CCFlow.WF.Rpt.UC
{
    public partial class Contrast : BP.Web.UC.UCBase3
    {
        #region  Property 
        public string FK_Flow
        {
            get
            {
                string s = this.Request.QueryString["FK_Flow"];
                if (s == null)
                    s = "068";
                return s;
            }
        }
        public string RptNo
        {
            get
            {
                string s = this.Request.QueryString["RptNo"];
                if (string.IsNullOrEmpty(s))
                    return "ND" + int.Parse(this.FK_Flow) + "MyRpt";
                return s;
            }
        }
        public Entity currEn = null;
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
        /// <summary>
        /// key
        /// </summary>
        public string Key
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
        public MapRpt currMapRpt = null;
        public MapAttrs HisMAs = null;
        #endregion

        #region  Controls 
        public DDL DDL_Num
        {
            get
            {
                return this.Left.GetDDLByID("DDL_Num");
            }
        }
        public DDL DDL_D
        {
            get
            {
                return this.Left.GetDDLByID("DDL_D");
            }
        }
        public DDL DDL_FXWay
        {
            get
            {
                return this.Left.GetDDLByID("DDL_FXWay");
            }
        }
        public DDL DDL_V1
        {
            get
            {
                return this.Left.GetDDLByID("DDL_V1");
            }
        }
        public DDL DDL_Group
        {
            get
            {
                return this.Left.GetDDLByID("DDL_Group");
            }
        }
        public DDL DDL_Order
        {
            get
            {
                return this.Left.GetDDLByID("DDL_Order");
            }
        }
        public DDL DDL_V2
        {
            get
            {
                return this.Left.GetDDLByID("DDL_V2");
            }
        }
        public MapAttrs currMapAttrs = null;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
           
            this.currMapAttrs = new MapAttrs(this.RptNo);
            this.currEn = this.HisEns.GetNewEntity;

            this.ur = new UserRegedit(WebUser.No, this.RptNo + "_Con");

            #region  Processing queries Permissions ,  Do not modify here ,以Search.ascx Prevail .
           // this.Page.RegisterClientScriptBlock("sss",
           //"<link href='/WF/Comm/Style/Table" + BP.Web.WebUser.Style + ".css' rel='stylesheet' type='text/css' />");

            currMapRpt = new MapRpt(this.RptNo, this.FK_Flow);
            Entity en = this.HisEns.GetNewEntity;
            Flow fl = new Flow(this.currMapRpt.FK_Flow);

            // Initialization Query toolbar .
            this.ToolBar1.InitToolbarOfMapRpt(fl, currMapRpt, this.RptNo, en, 1);
            this.ToolBar1.AddLinkBtn(NamesOfBtn.Export);

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
        ///  Displayed on the left tab .
        /// </summary>
        public void InitLeft()
        {
            this.Left.AddTable("class='Table' cellSpacing='0' cellPadding='0'  border='0' style='width:100%'");
            string paras = this.ur.Paras;

            #region  Options 
            this.Left.AddTR();
            this.Left.AddTDGroupTitle("colspan=2", " Analysis of data ");
            this.Left.AddTREnd();

            DDL ddl = new DDL();
            ddl.ID = "DDL_Num";
            ddl.AutoPostBack = true;
            ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged);

            ListItem li = null;
            this.Left.AddTR();
            this.Left.AddTD(" Data entry :");
            Attrs attrs = this.currEn.EnMap.Attrs;
            attrs.AddTBInt("MyNum", 1, " Process number ", true, true);
            foreach (Attr attr in attrs)
            {
                if (attr.UIContralType != UIContralType.TB)
                    continue;

                if (attr.UIVisible == false)
                    continue;

                if (attr.IsNum == false)
                    continue;

                switch (attr.Key)
                {
                    case NDXRptBaseAttr.OID:
                    case NDXRptBaseAttr.FID:
                    case "MID":
                    case NDXRptBaseAttr.PWorkID:
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

            ddl = new DDL();
            ddl.ID = "DDL_Order";
            ddl.AutoPostBack = true;
            ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged);

            li = new ListItem(" Ascending ", "UP");
            if (paras.Contains("@Order=UP"))
                li.Selected = true;
            ddl.Items.Add(li);

            li = new ListItem(" Descending ", "DESC");
            if (paras.Contains("@Order=DESC"))
                li.Selected = true;
            ddl.Items.Add(li);

            this.Left.AddTR();
            this.Left.AddTD(" Sort by :");
            this.Left.AddTD(ddl);
            this.Left.AddTREnd();
            #endregion  Options 

            #region  For comparison 
            this.Left.AddTR();
            this.Left.AddTDGroupTitle("colspan=2", " Compare contents ");
            this.Left.AddTREnd();

            ddl = new DDL();
            ddl.ID = "DDL_D";
            ddl.AutoPostBack = true;
            ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged);
            this.Left.AddTR();
            this.Left.AddTD(" Compare Options :");
            string dAttr = null;
            foreach (Attr attr in attrs)
            {
                if (attr.UIContralType == UIContralType.DDL)
                {
                    li = new ListItem(attr.Desc, attr.Key);

                    if (paras.Contains("@D=" + attr.Key))
                    {
                        dAttr = attr.Key;
                        li.Selected = true;
                    }
                    ddl.Items.Add(li);
                }
            }
            this.Left.AddTD(ddl);
            this.Left.AddTREnd();

            /* Get data analysis item .*/
            SysEnums sesD = null;
            Entities ensD = null;
            Map map = this.currMapRpt.HisEn.EnMap;
            Attr attrD1 = attrs.GetAttrByKey(this.DDL_D.SelectedItemStringVal);
            if (attrD1.IsEnum)
                sesD = new SysEnums(attrD1.UIBindKey);
            else
                ensD = attrD1.HisFKEns;

            this.Left.AddTR();
            this.Left.AddTD(" Benchmark project :");
            ddl = new DDL();
            ddl.ID = "DDL_V1";
            ddl.AutoPostBack = true;
            ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged);
            if (sesD != null)
            {
                if (sesD.Count == 0)
                    throw new Exception(attrD1.UIBindKey);

                foreach (SysEnum se in sesD)
                {
                    li = new ListItem(se.Lab, se.IntKey.ToString());
                    if (paras.Contains("@V1=" + se.IntKey))
                        li.Selected = true;
                    ddl.Items.Add(li);
                }
            }
            if (ensD != null)
            {
                ensD.RetrieveAll();
                if (ensD.Count == 0)
                    ensD.RetrieveAll();
                // throw new Exception(attrD1.UIBindKey);

                foreach (Entity en in ensD)
                {
                    li = new ListItem(en.GetValStrByKey("Name"), en.GetValStrByKey("No"));
                    if (paras.Contains("@V1=" + li.Value))
                        li.Selected = true;
                    ddl.Items.Add(li);
                }
            }
            this.Left.AddTD(ddl);
            this.Left.AddTREnd();

            this.Left.AddTR();
            this.Left.AddTD(" Comparison Project :");
            ddl = new DDL();
            ddl.ID = "DDL_V2";
            ddl.AutoPostBack = true;
            ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged);
            if (sesD != null)
            {
                foreach (SysEnum se in sesD)
                {
                    li = new ListItem(se.Lab, se.IntKey.ToString());
                    if (paras.Contains("@V2=" + se.IntKey))
                        li.Selected = true;
                    ddl.Items.Add(li);
                }
            }

            if (ensD != null)
            {
                foreach (Entity en in ensD)
                {
                    li = new ListItem(en.GetValStrByKey("Name"), en.GetValStrByKey("No"));
                    if (paras.Contains("@V2=" + li.Value))
                        li.Selected = true;
                    ddl.Items.Add(li);
                }
            }
            this.Left.AddTD(ddl);
            this.Left.AddTREnd();
            #endregion  Latitude Cross 

            ddl = new DDL();
            ddl.ID = "DDL_Group";
            ddl.AutoPostBack = true;
            ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged);
            this.Left.AddTR();
            this.Left.AddTD(" Group Project :");
            foreach (Attr attr in attrs)
            {
                if (attr.UIContralType == UIContralType.DDL)
                {
                    li = new ListItem(attr.Desc, attr.Key);
                    if (paras.Contains("@Group=" + attr.Key))
                        li.Selected = true;
                    ddl.Items.Add(li);
                }
            }
            this.Left.AddTD(ddl);
            this.Left.AddTREnd();
            this.Left.AddTableEnd();
        }
        public void SaveState()
        {
            #region  Save state 
            string paras = "";
            paras += "@Num=" + this.DDL_Num.SelectedItemStringVal;
            paras += "@FXWay=" + this.DDL_FXWay.SelectedItemStringVal;
            try
            {
                paras += "@V1=" + this.DDL_V1.SelectedItemStringVal;
                paras += "@V2=" + this.DDL_V2.SelectedItemStringVal;
            }
            catch
            {
            }

            paras += "@D=" + this.DDL_D.SelectedItemStringVal;
            paras += "@Order=" + this.DDL_Order.SelectedItemStringVal;
            paras += "@Group=" + this.DDL_Group.SelectedItemStringVal;

            ur.CfgKey = this.RptNo + "_Con";
            ur.MyPK = WebUser.No + "_" + ur.CfgKey;
            ur.FK_Emp = WebUser.NoOfSessionID;
            ur.Paras = paras;
            ur.Save();
            #endregion
        }
        public DataTable BindDG()
        {
            #region  Check to select items .
            try
            {
                if (this.DDL_V1.SelectedIndex == this.DDL_V2.SelectedIndex)
                {
                    if (this.DDL_V1.SelectedIndex == 0)
                        this.DDL_V2.SelectedIndex = 1;
                    else
                        this.DDL_V2.SelectedIndex = 0;
                }

                if (this.DDL_Group.SelectedIndex == this.DDL_D.SelectedIndex)
                {
                    if (this.DDL_Group.SelectedIndex == 0)
                        this.DDL_D.SelectedIndex = 1;
                    else
                        this.DDL_D.SelectedIndex = 0;
                }
                this.SaveState();
            }
            catch
            {

            }
            #endregion  Check to select items .
            
            Attrs attrs = this.currEn.EnMap.Attrs; 
            Attr d1 = attrs.GetAttrByKey(this.DDL_D.SelectedItemStringVal);
            Attr dNum = attrs.GetAttrByKey(this.DDL_Num.SelectedItemStringVal);

            string v1 = this.DDL_V1.SelectedItemStringVal;
            string v2 = this.DDL_V2.SelectedItemStringVal;
            string groupAttr = this.DDL_Group.SelectedItemStringVal;
            string fxField = this.DDL_FXWay.SelectedItemStringVal + "(" + dNum.Key + ")";

            string sql1 = "SELECT " + groupAttr + "," + d1.Key + "," + fxField + " FROM " + this.currMapRpt.PTable + " WHERE " + d1.Field + "='" + this.DDL_V1.SelectedItemStringVal + "'  GROUP BY " + groupAttr + "," + d1.Key;
            string sql2 = "SELECT " + groupAttr + "," + d1.Key + "," + fxField + " FROM " + this.currMapRpt.PTable + " WHERE " + d1.Field + "='" + this.DDL_V2.SelectedItemStringVal + "' GROUP BY " + groupAttr + "," + d1.Key;

            //   throw new Exception(sql1);

            DataTable dt1 = DBAccess.RunSQLReturnTable(sql1);
            DataTable dt2 = DBAccess.RunSQLReturnTable(sql2);
            this.Bind(dt1, dt2, "sss");
            return null;

            SysEnums sesD1 = null;
            Entities ensD1 = null;
            SysEnums sesD2 = null;
            Entities ensD2 = null;
            Map map = this.currMapRpt.HisEn.EnMap;

            #region  Generate two latitude statements 
            Attr attrD1 = attrs.GetAttrByKey(this.DDL_V1.SelectedItemStringVal);
            if (attrD1.IsEnum)
                sesD1 = new SysEnums(attrD1.UIBindKey);
            else
                ensD1 = attrD1.HisFKEns;

            Attr attrD2 = attrs.GetAttrByKey(this.DDL_V2.SelectedItemStringVal);
            if (attrD2.IsEnum)
                sesD2 = new SysEnums(attrD2.UIBindKey);
            else
                ensD2 = attrD2.HisFKEns;
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
                                //   val = WebUser.FK_Unit;
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


            return null;
        }
        public void Bind(DataTable dt1, DataTable dt2, string url)
        {
            // Increase benchmark project / Contrast determine whether the project has a value of , Prevent error 
            //added by liuxc,2014.11.29
            if (DDL_V1.SelectedItem == null)
            {
                Right.AddEasyUiPanelInfo(" Prompt ", " No data base project , Please complete data !");
                return;
            }

            if (DDL_V2.SelectedItem == null)
            {
                Right.AddEasyUiPanelInfo(" Prompt ", " No data comparing projects , Please complete data !");
                return;
            }

            string key = this.DDL_Group.SelectedItemStringVal;
            Attr attr = this.currMapRpt.HisEn.EnMap.GetAttrByKey(key);
            Entities ensOfGroup = attr.HisFKEns;
            ensOfGroup.RetrieveAll();

            string str = "";
            str += "<Table class='Table' cellspacing='0' cellpadding='0' border='0' style='width:100%'>";
            str += "<TR>";
            str += "  <TD warp=false class='GroupTitle' nowrap style='text-align:center' >序</TD>";
            str += "  <TD warp=false class='GroupTitle' nowrap >" + this.DDL_D.SelectedItem.Text + "</TD>";
            str += "  <TD warp=false class='GroupTitle' nowrap >" + this.DDL_V1.SelectedItem == null ? "" : this.DDL_V1.SelectedItem.Text + "</TD>";
            str += "  <TD warp=false class='GroupTitle' nowrap >" + this.DDL_V2.SelectedItem.Text + "</TD>";
            str += "  <TD warp=false class='GroupTitle' nowrap > Lower value </TD>";
            str += "  <TD warp=false class='GroupTitle' nowrap > Reduce the proportion of %</TD>";
            str += "</TR>";

            int idx = 0;
            foreach (Entity en in ensOfGroup)
            {
                bool isHave = false;
                foreach (DataRow dr in dt1.Rows)
                {
                    string kv = dr[0].ToString();
                    if (en.GetValStringByKey(attr.UIRefKeyValue) == kv)
                    {
                        isHave = true;
                        break;
                    }
                }
                if (isHave == false)
                    continue;  //  This value does not exist  就continue;

                idx++;
                str += "<TR >";
                str += "  <TD warp=false class='Idx' nowrap >" + idx.ToString() + "</TD>";
                str += "  <TD warp=false class='TD' nowrap >" + en.GetValStringByKey(attr.UIRefKeyText) + "</TD>";
                decimal num1 = 0;
                decimal num2 = 0;
                foreach (DataRow dr1 in dt1.Rows)
                {
                    string kv = dr1[0].ToString();  // Cycle to a value  .
                    if (en.GetValStringByKey(attr.UIRefKeyValue) != kv)
                        continue;

                    num1 = decimal.Parse(dr1[2].ToString());
                    num2 = 0;
                    str += "  <TD warp=false class='TDNum' nowrap ><a href=\"javascript:WinOpen('" + url + "&" + this.DDL_D.SelectedItemStringVal + "=" + dr1[0].ToString() + "&" + this.DDL_Num.SelectedItemStringVal + "=" + this.DDL_V1.SelectedItemStringVal + "')\"  >" + dr1[2].ToString() + "</a></TD>"; //  Period 1值.
                    isHave = false;
                    foreach (DataRow dr2 in dt2.Rows)
                    {
                        num2 = decimal.Parse(dr2[2].ToString());
                        if (dr2[0].ToString() == en.GetValStringByKey(attr.UIRefKeyValue))
                        {
                            isHave = true;
                            decimal cz = num1 - num2;
                            str += "  <TD warp=false class='TDNum' nowrap ><a href=\"javascript:WinOpen('" + url + "&" + this.DDL_D.SelectedItemStringVal + "=" + dr2[0].ToString() + "&" + this.DDL_Num.SelectedItemStringVal + "=" + this.DDL_V2.SelectedItemStringVal + "')\"  >" + num2.ToString() + "</a></TD>"; //  Period 1值.
                            str += "  <TD warp=false class='TDNum' nowrap >" + cz + "</TD>"; //  Reduce the number of .
                            break;
                        }
                    }
                    if (isHave == false)
                    {
                        num2 = 0;
                        str += "  <TD warp=false class='TDNum' nowrap >0</TD>"; //  Period 1值.
                        str += "  <TD warp=false class='TDNum' nowrap >" + num1 + "</TD>"; //  Reduce the number of .
                    }
                }
                if (num1 == 0)
                {
                    str += "  <TD warp=false class='TDNum' nowrap >0.00</TD>";
                }
                else
                {
                    decimal fz = decimal.Parse(num1.ToString()) - decimal.Parse(num2.ToString());
                    decimal fm = decimal.Parse(num1.ToString());
                    decimal rate = fz / fm * 100;
                    str += "  <TD warp=false class='TDNum' nowrap >" + rate.ToString("0.00") + "</TD>";
                }
                str += "</TR>";
            }
            str += "</Table>";
            this.Right.Add(str);
        }

        void cb_CheckedChanged(object sender, EventArgs e)
        {
            this.BindDG();
        }
        void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            DDL ddl = sender as DDL;
            if (ddl != null && ddl.ID == "DDL_D")
            {
                this.SaveState();
                this.Response.Redirect(this.PageID+".aspx?FK_Flow=" + this.FK_Flow + "&DoType=" + this.DoType, true);
                return;
            }
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
                    // this.ExportDGToExcel(this.DealTable(dt), this.HisEns.GetNewEntity.EnDesc);
                    return;
                default:
                    this.ToolBar1.SaveSearchState(this.RptNo, this.Key);
                    this.BindDG();
                    return;
            }
        }
    }
}