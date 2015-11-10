using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF.Data;
using BP.DA;
using BP;
using BP.WF;
using BP.En;
using BP.Web;
using BP.Web.Controls;
using BP.Sys;
using BP.WF.Rpt;
using BP.Sys.Xml;
using System.Web.UI.DataVisualization.Charting;

namespace CCFlow.WF.Rpt.UC
{
    public partial class Group : BP.Web.UC.UCBase3
    {
        #region  Property 
        public string FK_Flow
        {
            get
            {
                string s = this.Request.QueryString["FK_Flow"];
                if (s == null)
                {
                    // throw new Exception(" Lose FK_Flow Parameters .");
                    s = "068";
                }
                s = s.Replace("ND", "");
                s = s.Replace("Rpt", "");
                return s;
            }
        }
        public new string RptNo
        {
            get
            {
                string s = this.Request.QueryString["RptNo"];
                if (string.IsNullOrEmpty(s))
                    return "ND" + int.Parse(this.FK_Flow) + "MyRpt";
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
        /// <summary>
        /// key
        /// </summary>
        public new string Key
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
        public UserRegedit currUR = null;
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
                return this.ViewState["CfgVal"].ToString();
            }
            set
            {
                this.ViewState["CfgVal"] = value;
            }
        }
        public MapRpt currMapRpt = null;
        public Entity HisEn = null;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region  Processing queries Permissions - General section , Do not modify directly from  /uc/Search.ascx 中copy.
            //this.Page.RegisterClientScriptBlock("sss",
            //"<link href='/WF/Comm/Style/Table" + BP.Web.WebUser.Style + ".css' rel='stylesheet' type='text/css' />");

            this.currMapRpt = new MapRpt(this.RptNo, this.FK_Flow);
            this.HisEn = this.HisEns.GetNewEntity;
            Flow fl = new Flow(this.currMapRpt.FK_Flow);

            this.Page.Title = " Subgroup analysis  - " + fl.Name;

            // Initialization Query toolbar .
            this.ToolBar1.InitToolbarOfMapRpt(fl, currMapRpt, this.RptNo, this.HisEn, 1);
            this.ToolBar1.AddLinkBtn(BP.Web.Controls.NamesOfBtn.Export); // Increase export .

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
            ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged_Goto);

            this.ToolBar1.GetLinkBtnByID(NamesOfBtn.Search).Click += new System.EventHandler(this.ToolBar1_ButtonClick);
            this.ToolBar1.GetLinkBtnByID(NamesOfBtn.Export).Click += new System.EventHandler(this.ToolBar1_ButtonClick);
            #endregion  Processing queries Permissions 

            this.CB_IsShowPict.Text = " Display graphics ";
            this.currUR = new UserRegedit(WebUser.No, this.RptNo + "_Group");

            #region  Treatment selection state grouping field 
            if (this.IsPostBack == false)
            {
                string reAttrs = this.Request.QueryString["Attrs"];
                // string reAttrs = null; 
                if (string.IsNullOrEmpty(reAttrs))
                    reAttrs = this.currUR.Vals;

                this.CfgVal = this.currUR.Vals;
                this.CheckBoxList1.Items.Clear();
                foreach (Attr attr in this.HisEn.EnMap.Attrs)
                {
                    if (attr.UIContralType == UIContralType.DDL)
                    {
                        ListItem li = new ListItem(attr.Desc, attr.Key);
                        if (reAttrs != null)
                            li.Selected = reAttrs.Contains(attr.Key);

                        //  Depending on the state   Setting Information .
                        li.Selected = this.CfgVal.Contains(attr.Key);

                        // Join the list of items .
                        this.CheckBoxList1.Items.Add(li);
                    }
                }

                if (this.CheckBoxList1.Items.Count == 0)
                    throw new Exception(this.currMapRpt.Name + "@ No foreign key condition , Not suitable for grouping queries "); // No foreign key condition , Not suitable for grouping queries .

                if (this.CheckBoxList1.Items.Count == 1)
                    this.CheckBoxList1.Enabled = false;
            }
            #endregion   Treatment selection state grouping field 

            #region  Working with variables .
            if (this.OrderBy != null)
            {
                /* Check for sorting requirements . */

                if (this.OrderBy != null)
                    currUR.OrderBy = this.OrderBy;

                if (this.OrderWay == "Up")
                    currUR.OrderWay = "DESC";
                else
                    currUR.OrderWay = "";

                if (this.NumKey == null)
                    this.NumKey = currUR.NumKey;
            }

            this.OrderBy = currUR.OrderBy;
            this.OrderWay = currUR.OrderWay;
            this.CfgVal = currUR.Vals;

            // If you include the annual date .
            if (this.HisEn.EnMap.Attrs.Contains("FK_NY")
                && this.HisEn.EnMap.Attrs.Contains("FK_ND"))
                this.ViewState["IsContinueNDYF"] = "TRUE";
            else
                this.ViewState["IsContinueNDYF"] = "FALSE";
            #endregion   Working with variables .

            #region  Sort increase 
            if (this.IsPostBack == false)
                this.CB_IsShowPict.Checked = currUR.IsPic;
            #endregion

            this.BindNums();

            if (this.IsPostBack == false)
                this.BingDG();

            this.CB_IsShowPict.CheckedChanged += new EventHandler(State_Changed);
            this.CheckBoxList1.SelectedIndexChanged += new EventHandler(State_Changed);
        }
        void ddl_SelectedIndexChanged_Goto(object sender, EventArgs e)
        {
            DDL ddl = sender as DDL;
            string item = ddl.SelectedItemStringVal;
            string tKey = DateTime.Now.ToString("MMddhhmmss");
            this.Response.Redirect(item + ".aspx?RptNo=" + this.RptNo + "&FK_Flow=" + this.FK_Flow + "&T=" + tKey, true);
        }

        public void BindNums()
        {
            this.UCSys2.Clear();

            //  Check out the Events column set about it .
            ActiveAttrs aas = new ActiveAttrs();
            aas.RetrieveBy(ActiveAttrAttr.For, this.RptNo);

            Attrs attrs = this.HisEn.EnMap.Attrs;
            attrs.AddTBInt("MyNum", 1, " Process number ", true, true);

            this.UCSys2.AddTable("class='Table' cellspacing='0' cellpadding='0' border='0' style='width:100%'");

            foreach (Attr attr in attrs)
            {
                #region  Exclude unnecessary fields .
                if (attr.UIContralType != UIContralType.TB)
                    continue;

                //edited by liuxc,2014-12-01
                if (attr.UIVisible == false && attr.Key != "MyNum")
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
                #endregion  Exclude unnecessary fields .

                #region  Property Exclusion does not require configuration beyond calculation 
                bool isHave = false;
                //  There is not configured to offset its properties .
                foreach (ActiveAttr aa in aas)
                {
                    if (aa.AttrKey != attr.Key)
                        continue;

                    CheckBox cb1 = new CheckBox();
                    cb1.ID = "CB_" + attr.Key;
                    cb1.Text = attr.Desc;
                    cb1.AutoPostBack = true;

                    if (this.CfgVal.IndexOf("@" + attr.Key) == -1)
                        cb1.Checked = false; /*  If you do not include  key .*/
                    else
                        cb1.Checked = true;

                    cb1.CheckedChanged += new EventHandler(State_Changed);

                    this.UCSys2.AddTR();
                    this.UCSys2.Add("<TD >");
                    this.UCSys2.Add(cb1);
                    this.UCSys2.Add("</TD>");
                    this.UCSys2.AddTD();
                    this.UCSys2.AddTREnd();
                    isHave = true;
                }
                if (isHave)
                    continue;
                #endregion  Property Exclusion does not require configuration beyond calculation 

                #region  Began to numeric field types to the list .
                this.UCSys2.AddTR();
                CheckBox cb = new CheckBox();
                cb.ID = "CB_" + attr.Key;
                cb.Text = attr.Desc;
                cb.AutoPostBack = true;
                cb.CheckedChanged += new EventHandler(State_Changed);

                if (this.CfgVal.IndexOf("@" + attr.Key) == -1)
                    cb.Checked = false; /*  If you do not include  key .*/
                else
                    cb.Checked = true;

                this.UCSys2.Add("<TD style='font-size:12px;text-align:left'>");
                this.UCSys2.Add(cb);
                this.UCSys2.Add("</TD>");

                #region  Processing calculations .
                DDL ddl = new DDL();
                ddl.ID = "DDL_" + attr.Key;
                ddl.Items.Add(new ListItem(" Sum ", "SUM"));
                ddl.Items.Add(new ListItem(" Averaging ", "AVG"));
                if (this.IsContainsNDYF)
                    ddl.Items.Add(new ListItem(" Cumulative demand ", "AMOUNT"));

                #region  Check processing and analysis projects .
                if (this.CfgVal.IndexOf("@" + attr.Key + "=AVG") != -1)
                {
                    ddl.SelectedIndex = 1;
                }
                else if (this.CfgVal.IndexOf("@" + attr.Key + "=SUM") != -1)
                {
                    ddl.SelectedIndex = 0;
                }
                else if (this.CfgVal.IndexOf("@" + attr.Key + "=AMOUNT") != -1)
                {
                    ddl.SelectedIndex = 2;
                }
                else if (this.CfgVal.IndexOf("@" + attr.Key + "=MAX") != -1)
                {
                    ddl.SelectedIndex = 3;
                }
                else if (this.CfgVal.IndexOf("@" + attr.Key + "=MIN") != -1)
                {
                    ddl.SelectedIndex = 4;
                }
                else if (this.CfgVal.IndexOf("@" + attr.Key + "=BZC") != -1)
                {
                    ddl.SelectedIndex = 5;
                }
                else if (this.CfgVal.IndexOf("@" + attr.Key + "=LSXS") != -1)
                {
                    ddl.SelectedIndex = 6;
                }
                #endregion  Check processing and analysis projects .

                ddl.AutoPostBack = true;
                ddl.SelectedIndexChanged += new EventHandler(State_Changed);

                this.UCSys2.Add("<TD style='font-size:12px;text-align:left'>");
                this.UCSys2.Add(ddl);
                this.UCSys2.AddTDEnd();
                this.UCSys2.AddTREnd();
                #endregion  Processing calculations .

                if (string.IsNullOrEmpty(this.NumKey))
                {
                    this.NumKey = attr.Key;
                    this.UCSys2.GetCBByID("CB_" + attr.Key).Checked = true;
                }
                #endregion  Began to numeric field types to the list .
            }
            this.UCSys2.AddTableEnd();
        }
        /// <summary>
        ///  Nothing treatment choice , Are not allowed to choose the first item selection .
        /// </summary>
        public void DealChoseNone()
        {
            #region  Check whether the list of values is selected ?
            System.Web.UI.ControlCollection ctls = this.UCSys2.Controls;
            bool isCheck = false;
            foreach (Control ct in ctls)
            {
                if (ct.ID == null)
                    continue;

                if (ct.ID.IndexOf("CB_") == -1)
                    continue;

                string key = ct.ID.Substring("CB_".Length);
                CheckBox cb = this.UCSys2.GetCBByID("CB_" + key);
                if (cb.Checked == false)
                    continue;
                isCheck = true;
            }

            if (isCheck == false)
            {
                foreach (Control ct in ctls)
                {
                    if (ct.ID == null)
                        continue;

                    if (ct.ID.IndexOf("CB_") == -1)
                        continue;

                    string key = ct.ID.Substring("CB_".Length);
                    CheckBox cb = this.UCSys2.GetCBByID("CB_" + key);
                    cb.Checked = true;
                    break;
                }
            }
            #endregion  Check whether the list of values is selected ?

            #region  Check whether the group list is selected ?
            isCheck = false;
            foreach (ListItem li in this.CheckBoxList1.Items)
            {
                if (li.Selected)
                    isCheck = true;
            }

            if (isCheck == false)
            {
                foreach (ListItem li in this.CheckBoxList1.Items)
                {
                    li.Selected = true;
                    break;
                }
            }
            #endregion  Check whether the group list is selected ?
        }

        #region  Method 
        /// <summary>
        ///  Binding Data 
        /// </summary>
        /// <returns></returns>
        public DataTable BingDG()
        {
            /// Resolve any case data items are not selected .
            this.DealChoseNone();

            Entities ens = this.HisEns;
            Entity en = this.HisEn;

            //  Check out the Events column set about it .
            ActiveAttrs aas = new ActiveAttrs();
            aas.RetrieveBy(ActiveAttrAttr.For, this.RptNo);

            Paras myps = new Paras();
            Attrs attrs = this.HisEn.EnMap.Attrs;

            //  Found packet data . 
            string sqlOfGroupKey = "";
            Attrs attrsOfNum = new Attrs(); // Custom field attribute collection variable .
            System.Web.UI.ControlCollection ctls = this.UCSys2.Controls;
            string StateNumKey = "StateNumKey@"; //  To save operation state needs .
            string Condition = ""; // Problem specific field conditions .
            foreach (Control ct in ctls)
            {
                if (ct.ID == null)
                    continue;
                if (ct.ID.IndexOf("CB_") == -1)
                    continue;

                string key = ct.ID.Substring("CB_".Length);
                CheckBox cb = this.UCSys2.GetCBByID("CB_" + key);
                if (cb.Checked == false)
                    continue;

                attrsOfNum.Add(attrs.GetAttrByKey(key));

                #region  Something special processing configuration .
                DDL ddl = this.UCSys2.GetDDLByID("DDL_" + key);
                if (ddl == null)
                {
                    ActiveAttr aa = (ActiveAttr)aas.GetEnByKey(ActiveAttrAttr.AttrKey, key);
                    if (aa == null)
                        continue;

                    Condition += aa.Condition;
                    sqlOfGroupKey += " round (" + aa.Exp + ", 4) AS " + key + ",";
                    StateNumKey += key + "=Checked@"; //  Record status 
                    continue;
                }
                #endregion  Something special processing configuration .

                #region  Generate sqlOfGroupKey.
                switch (ddl.SelectedItemStringVal)
                {
                    case "SUM":
                        sqlOfGroupKey += " round ( SUM(" + key + "), 4) " + key + ",";
                        StateNumKey += key + "=SUM@"; //  Record status 
                        break;
                    case "AVG":
                        sqlOfGroupKey += " round (AVG(" + key + "), 4)  " + key + ",";
                        StateNumKey += key + "=AVG@"; //  Record status 
                        break;
                    case "AMOUNT":
                        sqlOfGroupKey += " round ( SUM(" + key + "), 4) " + key + ",";
                        StateNumKey += key + "=AMOUNT@"; //  Record status 
                        break;
                    default:
                        throw new Exception(" Without judgment .");
                }
                #endregion  Generate sqlOfGroupKey.

            }

            // Ask whether there is a total of field .
            bool isHaveLJ = false; //  Are there cumulative field .
            if (StateNumKey.IndexOf("AMOUNT@") != -1)
                isHaveLJ = true;

            if (sqlOfGroupKey == "")
            {
                this.UCSys1.AddMsgOfWarning(" Caveat ",
                    "<img src='/WF/Img/Pub/warning.gif' /><b><font color=red> You did not choose the data analyzed </font></b>"); // You did not choose the data analyzed .
                return null;
            }

            /*  If you include the accumulated data , That it certainly needs a month field . Business logic error .*/

            //  The last one went comma .
            sqlOfGroupKey = sqlOfGroupKey.Substring(0, sqlOfGroupKey.Length - 1);

            Paras ps = new Paras();
            //  Generate  sql.
            string selectSQL = "SELECT ";
            string groupBy = " GROUP BY ";
            Attrs AttrsOfGroup = new Attrs();
            string StatesqlOfGroupKey = "StatesqlOfGroupKey=@"; //  To save operation state needs .
            foreach (ListItem li in this.CheckBoxList1.Items)
            {
                if (li.Value == "FK_NY")
                {
                    /*  If years   Packet ,  And if the internal   Accumulated property , Forced to choose .*/
                    if (isHaveLJ)
                        li.Selected = true;
                }

                if (li.Selected)
                {
                    selectSQL += li.Value + ",";
                    groupBy += li.Value + ",";

                    //  Join the group inside .
                    AttrsOfGroup.Add(attrs.GetAttrByKey(li.Value), false, false);
                    StatesqlOfGroupKey += li.Value + "@";
                }
            }

            // Remove the last comma .
            groupBy = groupBy.Substring(0, groupBy.Length - 1);

            //[ Display graphics ] Availability , Only display only choose one , Available  added by liuxc,2014-11-20
            if (!(CB_IsShowPict.Enabled = AttrsOfGroup.Count == 1))
            {
                CB_IsShowPict.Checked = false;
                TB_H.Enabled = false;
                TB_W.Enabled = false;
                lbtnApply.Enabled = false;

            }
            else
            {
                TB_H.Enabled = true;
                TB_W.Enabled = true;
                lbtnApply.Enabled = true;
            }

            #region  Generate Where   Through this process generates two  where.
            //  Turn up  WHERE  Data .
            string where = " WHERE ";
            string whereOfLJ = " WHERE "; //  Cumulative  where.
            string url = "";
            foreach (Control item in this.ToolBar1.Controls)
            {
                #region  Shielded special circumstances .
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
                #endregion  Shielded special circumstances .

                #region  Judge multiple choice  case.
                if (val == "mvals")
                {
                    UserRegedit sUr = new UserRegedit();
                    sUr.MyPK = WebUser.No + this.RptNo + "_SearchAttrs";
                    sUr.RetrieveFromDBSources();

                    /*  If the value is multiple choice  */
                    string cfgVal = sUr.MVals;
                    AtPara ap = new AtPara(cfgVal);
                    string instr = ap.GetValStrByKey(key);
                    if (string.IsNullOrEmpty(instr))
                    {
                        if (key == "FK_Dept" || key == "FK_Unit")
                        {
                            if (key == "FK_Dept")
                            {
                                val = WebUser.FK_Dept;
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
                        instr = instr.Replace("..", ".");
                        instr = instr.Replace(".", "','");
                        instr = instr.Substring(2);
                        instr = instr.Substring(0, instr.Length - 2);
                        where += " " + key + " IN (" + instr + ")  AND ";
                        continue;
                    }
                }
                #endregion  Judge multiple choice  case.

                #region  Judge other fields .
                where += " " + key + " =" + SystemConfig.AppCenterDBVarStr + key + "   AND ";
                if (key != "FK_NY")
                    whereOfLJ += " " + key + " =" + SystemConfig.AppCenterDBVarStr + key + "   AND ";
                myps.Add(key, val);
                #endregion  Judge other fields 
            }
            #endregion

            #region  Plus  where like  Condition 
            if (en.EnMap.IsShowSearchKey == true
                && this.ToolBar1.GetTBByID("TB_Key").Text.Trim().Length > 1)
            {
                string key = this.ToolBar1.GetTBByID("TB_Key").Text.Trim();
                if (key.Length > 1)
                {
                    string whereLike = "";
                    bool isAddOr = false;
                    foreach (Attr likeKey in attrs)
                    {
                        if (likeKey.IsNum)
                            continue;
                        if (likeKey.IsRefAttr)
                            continue;

                        if (likeKey.UIContralType != UIContralType.TB)
                            continue;

                        switch (likeKey.MyDataType)
                        {
                            case DataType.AppDate:
                            case DataType.AppDateTime:
                            case DataType.AppBoolean:
                                continue;
                            default:
                                break;
                        }

                        switch (likeKey.Field)
                        {
                            case "MyFileExt":
                            case "MyFilePath":
                            case "WebPath":
                                continue;
                            default:
                                break;
                        }

                        if (isAddOr == false)
                        {
                            isAddOr = true;
                            whereLike += "      " + likeKey.Field + " LIKE '%" + key + "%' ";
                        }
                        else
                        {
                            whereLike += "   OR   " + likeKey.Field + " LIKE '%" + key + "%'";
                        }
                    }
                    whereLike += "          ";
                    where += "(" + whereLike + ")";
                }
            }
            #endregion

            #region  Plus date period .
            if (en.EnMap.DTSearchWay != DTSearchWay.None)
            {
                string dtFrom = this.ToolBar1.GetTBByID("TB_S_From").Text.Trim();
                string dtTo = this.ToolBar1.GetTBByID("TB_S_To").Text.Trim();
                string field = en.EnMap.DTSearchKey;
                string addAnd = "";
                if (en.EnMap.IsShowSearchKey && this.ToolBar1.GetTBByID("TB_Key").Text.Trim().Length > 0)
                    addAnd = " AND ";

                if (en.EnMap.DTSearchWay == DTSearchWay.ByDate)
                {
                    where += addAnd + "( " + field + ">='" + dtFrom + " 01:01' AND " + field + "<='" + dtTo + " 23:59')     ";
                }
                else
                {
                    where += addAnd + "(";
                    where += field + " >='" + dtFrom + "' AND " + field + "<='" + dtTo + "'";
                    where += ")";
                }
            }
            #endregion

            where = where.Replace("AND  AND", " AND ");

            if (where == " WHERE ")
            {
                where = "" + Condition.Replace("and", "");
                whereOfLJ = "" + Condition.Replace("and", "");
            }
            else
            {
                if (where.EndsWith(" AND "))
                    where = where.Substring(0, where.Length - " AND ".Length) + Condition;
                else
                    where = where + Condition;

                whereOfLJ = whereOfLJ.Substring(0, whereOfLJ.Length - " AND ".Length) + Condition;
            }

            string orderByReq = this.Request.QueryString["OrderBy"];
            string orderby = "";

            if (orderByReq != null && this.OrderBy != null
                && (selectSQL.Contains(orderByReq) || sqlOfGroupKey.Contains(orderByReq)))
            {
                orderby = " ORDER BY " + this.OrderBy;
                if (this.OrderWay != "Up")
                    orderby += " DESC ";
            }

            //  Assembled into the desired  sql 
            string sql = "";
            sql = selectSQL + sqlOfGroupKey + " FROM " + this.currMapRpt.PTable + where + groupBy + orderby;

            //  Physical table .
            // this.ResponseWriteBlueMsg(sql);
            myps.SQL = sql;
            DataTable dt2 = DBAccess.RunSQLReturnTable(myps);
            // this.Response.Write(sql);

            DataTable dt1 = dt2.Clone();
            dt1.Columns.Add("IDX", typeof(int));

            #region  Him for sub-page 
            int myIdx = 0;
            foreach (DataRow dr in dt2.Rows)
            {
                myIdx++;
                DataRow mydr = dt1.NewRow();
                mydr["IDX"] = myIdx;
                foreach (DataColumn dc in dt2.Columns)
                {
                    mydr[dc.ColumnName] = dr[dc.ColumnName];
                }
                dt1.Rows.Add(mydr);
            }
            #endregion

            #region  Deal with  Int  Type of grouping column .
            DataTable dt = dt1.Clone();
            dt.Rows.Clear();
            foreach (Attr attr in AttrsOfGroup)
            {
                dt.Columns[attr.Key].DataType = typeof(string);
            }
            foreach (DataRow dr in dt1.Rows)
            {
                dt.ImportRow(dr);
            }
            #endregion

            //  Deal with this physical table  ,  If the cumulative field ,  To expand its columns .
            if (isHaveLJ)
            {
                //  First expansion column .
                foreach (Attr attr in attrsOfNum)
                {
                    if (StateNumKey.IndexOf(attr.Key + "=AMOUNT") == -1)
                        continue;

                    switch (attr.MyDataType)
                    {
                        case DataType.AppInt:
                            dt.Columns.Add(attr.Key + "Amount", typeof(int));
                            break;
                        default:
                            dt.Columns.Add(attr.Key + "Amount", typeof(decimal));
                            break;
                    }
                }

                //  Add a cumulative summary data .
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (Attr attr in attrsOfNum)
                    {
                        if (StateNumKey.IndexOf(attr.Key + "=AMOUNT") == -1)
                            continue;

                        // Forming inquiry sql.
                        if (whereOfLJ.Length > 10)
                            sql = "SELECT SUM(" + attr.Key + ") FROM " + this.currMapRpt.PTable + whereOfLJ + " AND ";
                        else
                            sql = "SELECT SUM(" + attr.Key + ") FROM " + this.currMapRpt.PTable + " WHERE ";

                        foreach (Attr attr1 in AttrsOfGroup)
                        {
                            switch (attr1.Key)
                            {
                                case "FK_NY":
                                    sql += " FK_NY <= '" + dr["FK_NY"] + "' AND FK_ND='" + dr["FK_NY"].ToString().Substring(0, 4) + "' AND ";
                                    break;
                                case "FK_Dept":
                                    sql += attr1.Key + "='" + dr[attr1.Key] + "' AND ";
                                    break;
                                case "FK_SJ":
                                case "FK_XJ":
                                    sql += attr1.Key + " LIKE '" + dr[attr1.Key] + "%' AND ";
                                    break;
                                default:
                                    sql += attr1.Key + "='" + dr[attr1.Key] + "' AND ";
                                    break;
                            }
                        }

                        sql = sql.Substring(0, sql.Length - "AND ".Length);
                        if (attr.MyDataType == DataType.AppInt)
                            dr[attr.Key + "Amount"] = DBAccess.RunSQLReturnValInt(sql, 0);
                        else
                            dr[attr.Key + "Amount"] = DBAccess.RunSQLReturnValDecimal(sql, 0, 2);
                    }
                }
            }

            // Start output data table.
            this.UCSys1.Clear();
            this.UCSys1.AddTable("class='Table' cellspacing='0' cellpadding='0' border='0' style='width:100%'");

            #region  Export   Packet   Column header 
            if (StateNumKey.IndexOf("=AMOUNT") != -1)
            {
                /*  If you include the cumulative  */

                //  Increase the grouping condition .
                this.UCSys1.AddTR();  //  Beginning in the first column .
                this.UCSys1.Add("<td rowspan=2 class='Title'>ID</td>");
                foreach (Attr attr in AttrsOfGroup)
                {
                    this.UCSys1.Add("<td rowspan=2 class='GroupTitle'>" + attr.Desc + "</td>");
                }
                //  Increase data columns 
                foreach (Attr attr in attrsOfNum)
                {
                    if (StateNumKey.IndexOf(attr.Key + "=AMOUNT") != -1)
                    {
                        /*   If this data column   Contains cumulative  */
                        this.UCSys1.Add("<td  colspan=2 class='GroupTitle' >" + attr.Desc + "</td>");
                    }
                    else
                    {
                        this.UCSys1.Add("<td  rowspan=2 class='GroupTitle' >" + attr.Desc + "</td>");
                    }
                }
                this.UCSys1.AddTREnd();  // end  Beginning in the first column 

                this.UCSys1.AddTR();
                foreach (Attr attr in attrsOfNum)
                {
                    if (StateNumKey.IndexOf(attr.Key + "=AMOUNT") == -1)
                        continue;

                    this.UCSys1.Add("<td class='GroupTitle'> This month </td>"); // This month  this.ToE("OrderCondErr")
                    this.UCSys1.Add("<td class='GroupTitle'> Grand total </td>"); // Grand total 
                }
                this.UCSys1.AddTR();
            }
            else  /*  If you do not include the cumulative  */
            {
                this.UCSys1.AddTR();
                this.UCSys1.AddTDGroupTitle("style='text-align:center'", "No.");

                //  Grouping criteria 
                foreach (Attr attr in AttrsOfGroup)
                {
                    if (this.OrderBy == attr.Key)
                    {
                        switch (this.OrderWay)
                        {
                            case "Down":
                                this.UCSys1.AddTDGroupTitle("<a href='" + this.PageID + ".aspx?FK_Flow=" + this.FK_Flow + "&DoType=" + this.DoType + "&RptNo=" + this.RptNo + "&OrderBy=" + attr.Key + "&OrderWay=Up' >" + attr.Desc + "<img src='/WF/Img/ArrDown.gif' border=0/></a>");
                                break;
                            case "Up":
                            default:
                                this.UCSys1.AddTDGroupTitle("<a href='" + this.PageID + ".aspx?FK_Flow=" + this.FK_Flow + "&DoType=" + this.DoType + "&RptNo=" + this.RptNo + "&OrderBy=" + attr.Key + "&OrderWay=Down' >" + attr.Desc + "<img src='/WF/Img/ArrUp.gif' border=0/></a>");
                                break;
                        }
                    }
                    else
                    {
                        this.UCSys1.AddTDGroupTitle("<a href='" + this.PageID + ".aspx?FK_Flow=" + this.FK_Flow + "&DoType=" + this.DoType + "&RptNo=" + this.RptNo + "&OrderBy=" + attr.Key + "&OrderWay=Down' >" + attr.Desc + "</a>");
                    }
                }

                //  Packet data 
                foreach (Attr attr in attrsOfNum)
                {
                    string lab = "";
                    if (StateNumKey.Contains(attr.Key + "=SUM"))
                    {
                        lab = "( Total )" + attr.Desc;
                    }
                    else
                    {
                        lab = "( Average )" + attr.Desc;
                    }

                    if (this.OrderBy == attr.Key)
                    {
                        switch (this.OrderWay)
                        {
                            case "Down":
                                if (this.NumKey == attr.Key)
                                    this.UCSys1.AddTDGroupTitle(lab + "<a href='" + this.PageID + ".aspx?FK_Flow=" + this.FK_Flow + "&DoType=" + this.DoType + "&RptNo=" + this.RptNo + "&NumKey=" + attr.Key + "&OrderBy=" + attr.Key + "&OrderWay=Up'><img src='/WF/Img/ArrDown.gif' border=0/></a>");
                                else
                                    this.UCSys1.AddTDGroupTitle("<a href=\"" + this.PageID + ".aspx?FK_Flow=" + this.FK_Flow + "&DoType=" + this.DoType + "&RptNo=" + this.RptNo + "&NumKey=" + attr.Key + "\" >" + lab + "</a><a href='" + this.PageID + ".aspx?RptNo=" + this.RptNo + "&NumKey=" + attr.Key + "&OrderBy=" + attr.Key + "&OrderWay=Up&FK_Flow=" + this.FK_Flow + "'><img src='/WF/Img/ArrDown.gif' border=0/></a>");
                                break;
                            case "Up":
                            default:
                                if (this.NumKey == attr.Key)
                                    this.UCSys1.AddTDGroupTitle(lab + "<a href='" + this.PageID + ".aspx?FK_Flow=" + this.FK_Flow + "&DoType=" + this.DoType + "&RptNo=" + this.RptNo + "&OrderBy=" + attr.Key + "&NumKey=" + attr.Key + "&OrderWay=Down'><img src='/WF/Img/ArrUp.gif' border=0/></a>");
                                else
                                    this.UCSys1.AddTDGroupTitle("<a href=\"" + this.PageID + ".aspx?FK_Flow=" + this.FK_Flow + "&DoType=" + this.DoType + "&RptNo=" + this.RptNo + "&NumKey=" + attr.Key + "\" >" + lab + "</a><a href='" + this.PageID + ".aspx?RptNo=" + this.RptNo + "&OrderBy=" + attr.Key + "&NumKey=" + attr.Key + "&OrderWay=Down&FK_Flow=" + this.FK_Flow + "'><img src='/WF/Img/ArrUp.gif' border=0/></a>");
                                break;
                        }
                    }
                    else
                    {
                        if (this.NumKey == attr.Key)
                            this.UCSys1.AddTDGroupTitle(lab + "<a href='" + this.PageID + ".aspx?FK_Flow=" + this.FK_Flow + "&DoType=" + this.DoType + "&RptNo=" + this.RptNo + "&NumKey=" + attr.Key + "&OrderBy=" + attr.Key + "' ><img src='/WF/Img/ArrDownUp.gif' border=0/></a>");
                        else
                            this.UCSys1.AddTDGroupTitle("<a href=\"" + this.PageID + ".aspx?FK_Flow=" + this.FK_Flow + "&DoType=" + this.DoType + "&RptNo=" + this.RptNo + "&NumKey=" + attr.Key + "\" >" + lab + "</a><a href='" + this.PageID + ".aspx?RptNo=" + this.RptNo + "&NumKey=" + attr.Key + "&OrderBy=" + attr.Key + "&FK_Flow=" + this.FK_Flow + "' ><img src='/WF/Img/ArrDownUp.gif' border=0/></a>");
                    }
                }
                this.UCSys1.AddTDGroupTitle(" Excavation ");
                this.UCSys1.AddTREnd();
            }
            #endregion  Generate header 

            #region  To generate the query 
            string YSurl = "GroupDtl.aspx?RptNo=" + this.RptNo + "&FK_Flow=" + this.FK_Flow;    //edited by liuxc,2014-12-18
            string keys = "";
            if (this.ToolBar1.FindControl("TB_S_From") != null)
            {
                YSurl += "&DTFrom=" + this.ToolBar1.GetTBByID("TB_S_From").Text;
                YSurl += "&DTTo=" + this.ToolBar1.GetTBByID("TB_S_To").Text;
            }
            if (this.ToolBar1.FindControl("TB_Key") != null)
            {
                YSurl += "&Key=" + this.ToolBar1.GetTBByID("TB_Key").Text;
            }

            //  Packet information contains sector ?
            bool IsHaveFK_Dept = false;
            foreach (Attr attr in AttrsOfGroup)
            {
                if (attr.Key == "FK_Dept")
                {
                    IsHaveFK_Dept = true;
                    break;
                }
            }
            foreach (AttrSearch a23 in en.EnMap.SearchAttrs)
            {
                Attr attrS = a23.HisAttr;
                if (attrS.MyFieldType == FieldType.RefText)
                    continue;

                if (IsHaveFK_Dept && attrS.Key == "FK_Dept")
                    continue;

                DDL ddl = this.ToolBar1.GetDDLByKey("DDL_" + attrS.Key);
                if (ddl == null)
                {
                    throw new Exception(attrS.Key);
                }

                string val = this.ToolBar1.GetDDLByKey("DDL_" + attrS.Key).SelectedItemStringVal;
                if (val == "all")
                    continue;
                keys += "&" + attrS.Key + "=" + val;
            }
            YSurl = YSurl + keys;
            #endregion

            #region  Expansion table  Foreign keys , And the foreign key or enumeration Chinese name into the inside .
            //  To expand the foreign key table 
            foreach (Attr attr in AttrsOfGroup)
            {
                dt.Columns.Add(attr.Key + "T", typeof(string));
            }
            foreach (Attr attr in AttrsOfGroup)
            {
                if (attr.IsEnum)
                {
                    /*  That it is an enumeration type  */
                    SysEnums ses = new SysEnums(attr.UIBindKey);
                    foreach (DataRow dr in dt.Rows)
                    {
                        int val = 0;
                        try
                        {
                            val = int.Parse(dr[attr.Key].ToString());
                        }
                        catch
                        {
                            dr[attr.Key + "T"] = " ";
                            continue;
                        }

                        foreach (SysEnum se in ses)
                        {
                            if (se.IntKey == val)
                                dr[attr.Key + "T"] = se.Lab;
                        }
                    }
                    continue;
                }
                foreach (DataRow dr in dt.Rows)
                {
                    string val = dr[attr.Key].ToString();
                    if (attr.UIBindKey.Contains(".") == false)
                    {
                        try
                        {
                            dr[attr.Key + "T"] = DBAccess.RunSQLReturnStringIsNull("SELECT Name FROM " + attr.UIBindKey + " WHERE No='" + val + "'", val);
                        }
                        catch
                        {
                            dr[attr.Key + "T"] = val;
                        }
                        continue;
                    }

                    Entity myen = attr.HisFKEn;
                    myen.SetValByKey(attr.UIRefKeyValue, val);
                    try
                    {
                        myen.Retrieve();
                        dr[attr.Key + "T"] = myen.GetValStrByKey(attr.UIRefKeyText);
                    }
                    catch
                    {
                        if (val == null || val.Length <= 1)
                        {
                            dr[attr.Key + "T"] = val;
                        }
                        else
                        {
                            dr[attr.Key + "T"] = val;
                        }
                    }
                }
            }
            #endregion  Expansion table  Foreign keys , And the foreign key or enumeration Chinese name into the inside .

            #region  Generate datagrade Body 
            int i = 0;
            bool is1 = false;
            foreach (DataRow dr in dt.Rows)
            {
                i++;
                url = YSurl.Clone() as string;
                string keyActive = "";
                //  Produce url .
                foreach (Attr attr in AttrsOfGroup)
                    url += "&" + attr.Key + "=" + dr[attr.Key].ToString();

                is1 = this.UCSys1.AddTR(is1);
                this.UCSys1.AddTDIdx(int.Parse(dr["IDX"].ToString()));
                //  Grouping criteria 
                foreach (Attr attr in AttrsOfGroup)
                {
                    this.UCSys1.AddTD(dr[attr.Key + "T"].ToString());
                }

                //  Packet data 
                foreach (Attr attr in attrsOfNum)
                {
                    decimal obj = 0;
                    try
                    {
                        obj = decimal.Parse(dr[attr.Key].ToString());
                    }
                    catch
                    {
                        // throw new Exception(dr[attr.Key].ToString() +"@SQL="+ sql +"@"+ex.Message +"@Attr="+attr.Key );
                    }

                    switch (attr.MyDataType)
                    {
                        case DataType.AppMoney:
                        case DataType.AppRate:
                            if (StateNumKey.IndexOf(attr.Key + "=AMOUNT") != -1) /*   If this data column   Contains cumulative  */
                            {
                                this.UCSys1.AddTDJE(obj);
                                this.UCSys1.AddTDJE(decimal.Parse(dr[attr.Key + "Amount"].ToString()));
                            }
                            else
                            {
                                this.UCSys1.AddTDJE(obj);
                            }
                            break;
                        default:
                            if (StateNumKey.IndexOf(attr.Key + "=AMOUNT") != -1) /*   If this data column   Contains cumulative  */
                            {
                                this.UCSys1.AddTDNum(obj);
                                this.UCSys1.AddTDNum(decimal.Parse(dr[attr.Key + "Amount"].ToString()));
                            }
                            else
                            {
                                this.UCSys1.AddTDNum(obj);
                            }
                            break;
                    }
                }
                this.UCSys1.AddTD("<a href=\"javascript:OpenEasyUiDialog('" + url + "','eudlgframe',' Details ',700,432,'icon-table',false,null,null,document.getElementById('mainDiv'))\" class='easyui-linkbutton'> Detailed </a>");
                this.UCSys1.AddTREnd();
            }

            #region   Join aggregate information .
            this.UCSys1.AddTR("class='TRSum'");
            this.UCSys1.AddTD(" Gather ");
            foreach (Attr attr in AttrsOfGroup)
            {
                this.UCSys1.AddTD();
            }

            // Total column does not show .
            string NoShowSum = SystemConfig.GetConfigXmlEns("NoShowSum", this.RptNo);
            if (NoShowSum == null)
                NoShowSum = "";

            Attrs attrsOfNum1 = attrsOfNum.Clone();
            decimal d = 0;
            foreach (Attr attr in attrsOfNum)
            {
                if (NoShowSum.Contains("@" + attr.Key + "@"))
                {
                    bool isHave = false;
                    foreach (ActiveAttr aa in aas)
                    {
                        if (aa.AttrKey != attr.Key)
                            continue;

                        isHave = true;
                        /*  If it is a computed column  */
                        string exp = aa.ExpApp;
                        if (exp == null || exp == "")
                        {
                            this.UCSys1.AddTD();
                            break;
                        }
                        foreach (Attr myattr in attrsOfNum1)
                        {
                            if (exp.IndexOf("@" + myattr.Key + "@") != -1)
                            {
                                d = 0;
                                foreach (DataRow dr1 in dt.Rows)
                                {
                                    try
                                    {
                                        d += decimal.Parse(dr1[myattr.Key].ToString());
                                    }
                                    catch
                                    {
                                    }
                                }

                                exp = exp.Replace("@" + myattr.Key + "@", d.ToString());
                            }
                        }
                        this.UCSys1.AddTDNum(DataType.ParseExpToDecimal(exp));
                    }

                    if (isHave == false)
                        this.UCSys1.AddTD();
                    continue;
                }

                switch (attr.MyDataType)
                {
                    case DataType.AppMoney:
                    case DataType.AppRate:
                        if (StateNumKey.IndexOf(attr.Key + "=AMOUNT") != -1) /*   If this data column   Contains cumulative  */
                        {
                            d = 0;
                            foreach (DataRow dr1 in dt.Rows)
                                d += decimal.Parse(dr1[attr.Key].ToString());
                            this.UCSys1.AddTDJE(d);

                            d = 0;
                            foreach (DataRow dr1 in dt.Rows)
                                d += decimal.Parse(dr1[attr.Key + "Amount"].ToString());
                            this.UCSys1.AddTDJE(d);
                        }
                        else
                        {
                            d = 0;
                            foreach (DataRow dr1 in dt.Rows)
                            {
                                try
                                {
                                    d += decimal.Parse(dr1[attr.Key].ToString());
                                }
                                catch
                                {
                                }
                            }

                            if (StateNumKey.IndexOf(attr.Key + "=AVG") < 1)
                            {
                                this.UCSys1.AddTDJE(d);
                            }
                            else
                            {
                                if (dt.Rows.Count == 0)
                                    this.UCSys1.AddTD();
                                else
                                    this.UCSys1.AddTDJE(d / dt.Rows.Count);
                            }
                        }
                        break;
                    default:
                        if (StateNumKey.IndexOf(attr.Key + "=AMOUNT") != -1) /*   If this data column   Contains cumulative  */
                        {
                            d = 0;
                            foreach (DataRow dr1 in dt.Rows)
                                d += decimal.Parse(dr1[attr.Key].ToString());
                            this.UCSys1.AddTDNum(d);

                            d = 0;
                            foreach (DataRow dr1 in dt.Rows)
                                d += decimal.Parse(dr1[attr.Key + "Amount"].ToString());
                            this.UCSys1.AddTDNum(d);
                        }
                        else
                        {
                            d = 0;
                            foreach (DataRow dr1 in dt.Rows)
                            {
                                try
                                {
                                    d += decimal.Parse(dr1[attr.Key].ToString());
                                }
                                catch
                                {
                                }
                            }

                            if (StateNumKey.IndexOf(attr.Key + "=AVG") < 1)
                            {
                                this.UCSys1.AddTDNum(d);
                            }
                            else
                            {
                                if (dt.Rows.Count == 0)
                                    this.UCSys1.AddTD();
                                else
                                    this.UCSys1.AddTDJE(d / dt.Rows.Count);
                            }
                        }
                        break;
                }
            }
            this.UCSys1.AddTD();
            this.UCSys1.AddTREnd();
            #endregion

            this.UCSys1.AddTableEnd();
            #endregion  Generate Body 

            #region  Generate graphics 
            if (this.CB_IsShowPict.Checked)
            {
                // Use tab Page displays a chart , Filling tab Frame 
                UCSys3.Add("<div class='easyui-tabs' data-options=\"fit:true\">" + Environment.NewLine);
                UCSys3.Add("    <div title=' Packet data ' data-options=\"iconCls:'icon-table'\" style='padding:5px'>" + Environment.NewLine);
                UCSys4.Add("    </div>" + Environment.NewLine);

                /*  In the case of  1 纬 */
                string colOfGroupField = "";
                string colOfGroupName = "";
                string colOfNumField = "";
                string colOfNumName = "";
                string title = "";
                int chartHeight = int.Parse(this.TB_H.Text);
                int chartWidth = int.Parse(this.TB_W.Text);

                if (isHaveLJ)
                {
                    /*   If there is a total of ,  In accordance with the cumulative field analysis .*/
                    colOfGroupField = AttrsOfGroup[0].Key;
                    colOfGroupName = AttrsOfGroup[0].Desc;

                    colOfNumName = attrsOfNum[0].Desc;
                    if (dt.Columns.Contains(attrsOfNum[0].Key + "AMOUNT"))
                        colOfNumField = attrsOfNum[0].Key + "AMOUNT";
                    else
                        colOfNumField = attrsOfNum[0].Key;
                }
                else
                {
                    colOfGroupField = AttrsOfGroup[0].Key;
                    colOfGroupName = AttrsOfGroup[0].Desc;

                    if (NumKey == null)
                    {
                        colOfNumName = attrsOfNum[0].Desc;
                        colOfNumField = attrsOfNum[0].Key;
                    }
                    else
                    {
                        //  colOfNumField = attrsOfNum[0].Key;
                        colOfNumName = attrs.GetAttrByKey(NumKey).Desc; // this.UCSys1.get;
                        colOfNumField = NumKey;
                    }
                }

                string colOfNumName1 = "";
                if (StateNumKey.Contains(this.NumKey + "=SUM"))
                    colOfNumName1 = "( Total )" + colOfNumName;
                else
                    colOfNumName1 = "( Average )" + colOfNumName;

                if (dt.Columns.Contains(colOfNumField) == false)
                {
                    foreach (Attr item in attrsOfNum)
                    {
                        if (dt.Columns.Contains(item.Key))
                        {
                            colOfNumField = item.Key;
                            break;
                        }
                    }
                }
                try
                {
                    //Chart chart = new Chart();
                    //chart.Width = new Unit(chartWidth);
                    //chart.Height = new Unit(chartHeight);
                    //chart.Titles.Add(" Comparative analysis  -  Histogram ");
                    //var chartArea = chart.ChartAreas.Add("MainChartArea");
                    //var legend = chart.Legends.Add("MainLegend");
                    //legend.Docking = Docking.Bottom;
                    //var serie = chart.Series.Add("MainSerie");
                    ////serie.LegendText = colOfNumName1;
                    //serie.Legend = "MainLegend";
                    //serie.ChartArea = "MainChartArea";
                    //serie.ChartType = SeriesChartType.Line;
                    //chartArea.AxisX.Title = colOfGroupName;
                    //chartArea.AxisX.TitleAlignment = System.Drawing.StringAlignment.Far;
                    //chartArea.AxisY.Title = colOfNumName1;
                    //chartArea.AxisY.TitleAlignment = System.Drawing.StringAlignment.Far;

                    //chart.DataSource = dt;

                    //serie.XValueMember = colOfGroupField + "T";
                    //serie.YValueMembers = colOfNumField;

                    //chart.DataBind();

                    //UCSys3.Add(chart);

                    var yfields = new Dictionary<string, string>();

                    foreach (Attr attr in attrsOfNum)
                    {
                        yfields.Add(attr.Key, attr.Desc);
                    }

                    // Increase histogram 
                    UCSys4.Add("    <div title=' Histogram ' data-options=\"iconCls:'icon-columnchart'\" style='padding:5px;text-align:center'>" + Environment.NewLine);
                    UCSys4.GenerateColumnChart(dt, AttrsOfGroup[0].Key + "T", AttrsOfGroup[0].Desc, yfields, this.Page.Title.Substring(" Subgroup analysis  - ".Length), chartWidth, chartHeight);
                    UCSys4.Add("    </div>" + Environment.NewLine);

                    // Increase Pie 
                    UCSys4.Add("    <div title=' Pie Chart ' data-options=\"iconCls:'icon-piechart'\" style='padding:5px;text-align:center'>" + Environment.NewLine);
                    UCSys4.GeneratePieChart(dt, AttrsOfGroup[0].Key + "T", AttrsOfGroup[0].Desc, yfields, this.Page.Title.Substring(" Subgroup analysis  - ".Length), chartWidth, chartHeight);
                    UCSys4.Add("    </div>" + Environment.NewLine);

                    // Increase stitches map 
                    UCSys4.Add("    <div title=' Line chart ' data-options=\"iconCls:'icon-linechart'\" style='padding:5px;text-align:center'>" + Environment.NewLine);
                    UCSys4.GenerateLineChart(dt, AttrsOfGroup[0].Key + "T", AttrsOfGroup[0].Desc, yfields, this.Page.Title.Substring(" Subgroup analysis  - ".Length), chartWidth, chartHeight);
                    UCSys4.Add("    </div>" + Environment.NewLine);

                    UCSys4.Add("</div>" + Environment.NewLine);

                    //url = this.Request.ApplicationPath + "/Temp/" + CCFlow.WF.Comm.UC.UCSys.GenerChart(dt,
                    //  colOfGroupField + "T", colOfGroupName,
                    //  colOfNumField, colOfNumName1
                    //  , "", chartHeight, chartWidth, ChartType.Histogram);

                    //this.UCSys3.AddBR("<img src='" + url + "' />");

                    //url = this.Request.ApplicationPath + "/Temp/" + CCFlow.WF.Comm.UC.UCSys.GenerChart(dt,
                    //   colOfGroupField + "T", colOfGroupName,
                    //   colOfNumField, colOfNumName1
                    //   , "", chartHeight, chartWidth, ChartType.Pie);

                    //this.UCSys3.AddBR("<img src='" + url + "' />");

                    //url = this.Request.ApplicationPath + "/Temp/" + BP.Web.UC.UCGraphics.GenerChart(dt,
                    //   colOfGroupField + "T", colOfGroupName,
                    //   colOfNumField, colOfNumName1
                    //   , "", chartHeight, chartWidth, ChartType.Line);
                    //this.UCSys3.AddBR("<img src='" + url + "' />");
                }
                catch (Exception ex)
                {
                    this.ResponseWriteRedMsg("@ Image files generated an error :" + ex.Message);
                }
                //this.BPTabStrip1.Items[0].Text = this.ToE("TableGrade", " Form ");
                //// this.BPTabStrip1.Items[0].Text = this.ToE("TableGrade", " Form -<a href=\"javascript:WinOpen('./Rpt/Adv.aspx')\" > Advanced </a>");
                //this.BPTabStrip1.Items[2].Text = this.ToE("Histogram", colOfNumName + "- Histogram ");
                //this.BPTabStrip1.Items[4].Text = this.ToE("Pie", colOfNumName + "- Pie ");
                //this.BPTabStrip1.Items[6].Text = this.ToE("Line", colOfNumName + "- Line chart ");
            }
            #endregion

            #region  Save operation status 
            currUR.Vals = StatesqlOfGroupKey + StateNumKey;
            currUR.CfgKey = this.RptNo + "_Group";
            currUR.FK_Emp = WebUser.No;
            currUR.OrderBy = this.OrderBy;
            currUR.OrderWay = this.OrderWay;
            currUR.IsPic = this.CB_IsShowPict.Checked;
            currUR.GenerSQL = myps.SQL;
            currUR.NumKey = this.NumKey;
            currUR.Paras = "";
            foreach (Para para in myps)
            {
                currUR.Paras += "@" + para.ParaName + "=" + para.val;
            }
            currUR.Save();

            this.SetValueByKey("Vals", currUR.Vals);
            this.SetValueByKey("CfgKey", currUR.CfgKey);
            this.SetValueByKey("OrderBy", currUR.OrderBy);
            this.SetValueByKey("OrderWay", currUR.OrderWay);
            this.SetValueByKey("IsPic", currUR.IsPic);
            this.SetValueByKey("SQL", currUR.GenerSQL);
            this.SetValueByKey("NumKey", currUR.NumKey);
            this.CfgVal = currUR.Vals;
            #endregion

            return dt1;
        }

        public DataTable DealTable(DataTable dt)
        {
            DataTable dtCopy = new DataTable();

            #region  Converts them to  string  Type .
            foreach (DataColumn dc in dt.Columns)
                dtCopy.Columns.Add(dc.ColumnName, typeof(string));

            foreach (DataRow dr in dt.Rows)
                dtCopy.ImportRow(dr);
            #endregion

            Entity en = this.currMapRpt.HisEn;
            Map map = en.EnMap;
            MapAttrs attrs = this.currMapRpt.MapAttrs;
            foreach (DataColumn dc in dt.Columns)
            {
                bool isLJ = false;
                Attr attr = null;
                try
                {
                    attr = map.GetAttrByKey(dc.ColumnName);
                    isLJ = false;
                }
                catch
                {
                    try
                    {
                        attr = map.GetAttrByKey(dc.ColumnName + "AMOUNT");
                        isLJ = true;
                    }
                    catch
                    {
                    }
                }

                if (attr == null)
                    continue;

                if (attr.UIBindKey == null || attr.UIBindKey == "")
                {
                    if (isLJ)
                        dtCopy.Columns[attr.Key.ToUpper() + "AMOUNT"].ColumnName = " Grand total ";
                    else
                        dtCopy.Columns[attr.Key.ToUpper()].ColumnName = attr.Desc;
                    continue;
                }

                //  Settings tab  
                if (attr.UIBindKey.IndexOf(".") != -1)
                {
                    //  Entity en1 = BP.En.ClassFactory.GetEns(attr.UIBindKey).GetNewEntity;
                    Entity en1 = attr.HisFKEn;
                    string pk = en1.PK;
                    foreach (DataRow dr in dtCopy.Rows)
                    {
                        if (dr[attr.Key] == DBNull.Value)
                            continue;

                        string val = (string)dr[attr.Key];
                        if (val == null || val == "")
                            continue;

                        en1.SetValByKey(pk, dr[attr.Key]);
                        int i = en1.RetrieveFromDBSources();
                        if (i == 0)
                            continue;

                        dr[attr.Key] = en1.GetValStrByKey(attr.UIRefKeyValue) + en1.GetValStrByKey(attr.UIRefKeyText);
                    }
                }
                else if (attr.UIBindKey.Length >= 2)
                {
                    foreach (DataRow mydr in dtCopy.Rows)
                    {
                        if (mydr[attr.Key] == DBNull.Value)
                            continue;

                        int intVal = int.Parse(mydr[attr.Key].ToString());
                        SysEnum se = new SysEnum(attr.UIBindKey, intVal);
                        mydr[attr.Key] = se.Lab;
                    }
                }
                dtCopy.Columns[attr.Key.ToUpper()].ColumnName = attr.Desc;
            }

            try
            {
                dtCopy.Columns["MYNUM"].ColumnName = " The number of ";
            }
            catch
            {
            }
            return dtCopy;
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
        #endregion

        private void ToolBar1_ButtonClick(object sender, System.EventArgs e)
        {
            var btn = (LinkBtn)sender;
            switch (btn.ID)
            {
                case NamesOfBtn.Help:
                    break;
                case NamesOfBtn.Excel:
                    DataTable dt = this.BingDG();
                    this.ExportDGToExcel(this.DealTable(dt), this.HisEn.EnDesc);
                    return;
                default:
                    this.ToolBar1.SaveSearchState(this.RptNo, this.Key);
                    this.BingDG();
                    return;
            }
        }

        void State_Changed(object sender, EventArgs e)
        {
            this.BingDG();
        }

        void CheckBoxList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BingDG();
        }

        protected void lbtnApply_Click(object sender, EventArgs e)
        {
            this.BingDG();
        }
    }
}