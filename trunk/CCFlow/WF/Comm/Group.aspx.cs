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
using BP;
using BP.Sys.Xml;

namespace CCFlow.Web.Comm
{
    /// <summary>
    ///  Summary Description 
    /// </summary>
    public partial class GroupEnsNum : BP.Web.WebPage
    {
        public new string EnsName
        {
            get
            {
                if (this.Request.QueryString["EnsName"] == null)
                    return "BP.GE.Infos";

                return this.Request.QueryString["EnsName"];
            }
        }
        /// <summary>
        /// key
        /// </summary>
        public new string Key
        {
            get
            {
                return this.ToolBar1.GetTBByID("TB_Key").Text;
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
                return this.ViewState["CfgVal"].ToString();
            }
            set
            {
                this.ViewState["CfgVal"] = value;
            }
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.Page.RegisterClientScriptBlock("s",
       "<link href='./Style/Table" + BP.Web.WebUser.Style + ".css' rel='stylesheet' type='text/css' />");

            this.CB_IsShowPict.Text = " Display graphics ";
            this.BPTabStrip1.Items[2].Text = " Histogram ";
            this.BPTabStrip1.Items[4].Text =  " Pie ";
            this.BPTabStrip1.Items[6].Text =  " Line chart ";

            #region  Permissions issue 
            UAC uac = new UAC();
            try
            {
                uac = this.HisEn.HisUAC;
            }
            catch
            {
                uac.IsView = true;
            }

            if (uac.IsView == false)
                throw new Exception(" You do not have to view [" + this.HisEn.EnDesc + "] Authority data .");

            if (this.IsReadonly)
            {
                uac.IsDelete = false;
                uac.IsInsert = false;
                uac.IsUpdate = false;
            }
            #endregion  Permissions issue 

            this.ur = new UserRegedit(WebUser.NoOfSessionID, this.EnsName + "_Group");
            if (this.IsPostBack)
            {
                ur.Vals = this.GetValueByKey("Vals");
                ur.CfgKey = this.GetValueByKey("CfgKey");
                ur.OrderBy = this.GetValueByKey("OrderBy");
                ur.OrderWay = this.GetValueByKey("OrderWay");
                ur.IsPic = this.GetValueByKeyBool("IsPic");
                ur.GenerSQL = this.GetValueByKey("SQL");
                ur.NumKey = this.GetValueByKey("NumKey");
                ur.MVals = this.GetValueByKey("MVals");
                ur.Save();
            }

            #region  Set up tool bar 1 µÄcontral
            if (uac.IsView == false)
                throw new Exception("@ I am sorry , You do not have permission to view !");

            if (this.OrderBy != null)
            {
                if (this.OrderBy != null)
                    ur.OrderBy = this.OrderBy;

                if (this.OrderWay == "Up")
                    ur.OrderWay = "DESC";
                else
                    ur.OrderWay = "";

                if (this.NumKey == null)
                    this.NumKey = ur.NumKey;

                // ur.Save();
            }


            this.OrderBy = ur.OrderBy;
            this.OrderWay = ur.OrderWay;
            this.CfgVal = ur.Vals;

            Map map = this.HisEn.EnMap;
            if (map.Attrs.Contains("FK_NY") && map.Attrs.Contains("FK_ND"))
            {
                this.ViewState["IsContinueNDYF"] = "TRUE";
            }
            else
            {
                this.ViewState["IsContinueNDYF"] = "FALSE";
            }

            if (this.IsPostBack == false)
            {
                string reAttrs = this.Request.QueryString["Attrs"];
                this.CheckBoxList1.Items.Clear();
                foreach (Attr attr in map.Attrs)
                {
                    if (attr.UIContralType == UIContralType.DDL)
                    {
                        ListItem li = new ListItem(attr.Desc, attr.Key);
                        if (reAttrs != null)
                        {
                            if (reAttrs.IndexOf(attr.Key) != -1)
                            {
                                li.Selected = true;
                            }
                        }

                        //  Depending on the state   Setting Information .
                        if (this.CfgVal.IndexOf(attr.Key) != -1)
                            li.Selected = true;
                        this.CheckBoxList1.Items.Add(li);
                    }
                }

                if (this.CheckBoxList1.Items.Count == 0)
                    throw new Exception(map.EnDesc + "   No foreign key condition , Not suitable for grouping queries " ); // No foreign key condition , Not suitable for grouping queries .

                if (this.CheckBoxList1.Items.Count == 1)
                    this.CheckBoxList1.Enabled = false;
            }
            #endregion

            this.ToolBar1.InitByMapV2(this.HisEn.EnMap, 1);

            #region  Setting choice   Defaults 
            AttrSearchs searchs = map.SearchAttrs;
            foreach (AttrSearch attr in searchs)
            {
                string mykey = this.Request.QueryString[attr.HisAttr.Key];
                if (mykey == "" || mykey == null)
                    continue;
                else
                    this.ToolBar1.GetDDLByKey("DDL_" + attr.HisAttr.Key).SetSelectItem(mykey, attr.HisAttr);
            }
            #endregion

            //this.ToolBar1.InitByMapVGroup(this.HisEn.EnMap);
            this.ToolBar1.AddSpt("spt1");
            this.ToolBar1.AddBtn(NamesOfBtn.Excel);

            //this.ToolBar1.AddBtn(NamesOfBtn.Excel);
            //this.ToolBar1.AddBtn(NamesOfBtn.Help);

            #region  Sort increase 
            this.BPMultiPage1.AddPageView("Table");
            this.BPMultiPage1.AddPageView("Img");
            this.BPMultiPage1.AddPageView("Imgs");
            this.BPMultiPage1.AddPageView("Imgss");
            if (this.IsPostBack == false)
                this.CB_IsShowPict.Checked = ur.IsPic;
            // this.DDL_OrderBy.SelectedItem(ur.OrderBy);
            // this.DDL_OrderWay.SelectedItem(ur.OrderWay);
            #endregion

            this.BindNums();
            if (this.IsPostBack == false)
                this.BingDG();

            this.ToolBar1.GetBtnByID("Btn_Search").Click += new System.EventHandler(this.ToolBar1_ButtonClick);
            this.ToolBar1.GetBtnByID("Btn_Excel").Click += new System.EventHandler(this.ToolBar1_ButtonClick);

            this.CB_IsShowPict.CheckedChanged += new EventHandler(State_Changed);
            this.CheckBoxList1.SelectedIndexChanged += new EventHandler(State_Changed);

            // this.ToolBar1 += new System.EventHandler(this.ToolBar1_ButtonClick);
            //string lab = SystemConfig.GetConfigXmlEns("GroupEns", this.EnsName);
            //if (lab == null)
            // lab = this.HisEn.EnMap.EnDesc;

            this.Label1.Text = this.GenerCaption(this.HisEn.EnMap.EnDesc);


            //this.Title = this.HisEn.EnDesc;
            //this.la
            //this.GenerLabel(this.Label1, this.HisEn);
            //this.Label1.Controls.Add(this.GenerLabel("<img src='../Img/Btn/DataGroup.gif' border=0  />" + lab));
            //this.Label1.Controls.Add(this.GenerLabel("<img src='../Img/Btn/DataGroup.gif' border=0  />"+this.HisEn.EnDesc+"<img src='../Img/Btn/Table.gif' border=0  /><a href='UIEns.aspx?EnsName="+this.HisEns.ToString()+"&Readonly=1'> Inquiry </a><img src='../Img/Btn/Table.gif' border=0  /><a href='UIEnsCols.aspx?EnsName="+this.HisEns.ToString()+"&IsReadonly=1'> Select the column of the query </a>")); 
            //this.Label1.Controls.Add(this.GenerLabel("<img src='../Img/Btn/DataGroup.gif' border=0  />"+this.HisEn.EnDesc+"<img src='../Img/Btn/Table.gif' border=0  /><a href='UIEns.aspx?EnsName="+this.HisEns.ToString()+"&Readonly=1'> Inquiry </a><img src='../Img/Btn/Table.gif' border=0  /><a href='UIEnsCols.aspx?EnsName="+this.HisEns.ToString()+"&Readonly=1'> Select the column of the query </a>")); 
        }

        public void BindNums()
        {
            this.UCSys2.Clear();
            //  Check out the Events column set about it .
            ActiveAttrs aas = new ActiveAttrs();
            aas.RetrieveBy(ActiveAttrAttr.For, this.EnsName);

            Map map = this.HisEn.EnMap;
            this.UCSys2.Add("<table border=0 cellPadding=0 >");
            foreach (Attr attr in map.Attrs)
            {
                if (attr.IsPK || attr.IsNum == false)
                    continue;
                if (attr.UIContralType == UIContralType.TB == false)
                    continue;
                if (attr.UIVisible == false)
                    continue;
                if (attr.MyFieldType == FieldType.FK)
                    continue;
                if (attr.MyFieldType == FieldType.Enum)
                    continue;
                if (attr.Key == "OID" || attr.Key == "WorkID" || attr.Key == "MID")
                    continue;


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

                    this.UCSys2.Add("<TD style='font-size:12px;text-align:left'style='background:url(imags/TitleCaption1.gif)'>");
                    this.UCSys2.Add(cb1);
                    this.UCSys2.Add("</TD>");
                    isHave = true;
                }
                if (isHave)
                    continue;


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

                DDL ddl = new DDL();
                ddl.ID = "DDL_" + attr.Key;
                ddl.Items.Add(new ListItem(" Sum ", "SUM"));
                ddl.Items.Add(new ListItem(" Averaging ", "AVG"));
                if (this.IsContainsNDYF)
                    ddl.Items.Add(new ListItem(" Cumulative demand ", "AMOUNT"));

                //ddl.Items.Add(new ListItem(this.ToE("ForMax", " Seeking maximum "), "MAX"));
                //ddl.Items.Add(new ListItem(this.ToE("ForMin", " Minimum requirements "), "MIN"));
                //ddl.Items.Add(new ListItem(this.ToE("ForBZC", " Poor standard requirements "), "BZC"));
                //ddl.Items.Add(new ListItem(this.ToE("ForLSXS", " Seeking dispersion coefficient "), "LSXS"));

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

                ddl.AutoPostBack = true;
                ddl.SelectedIndexChanged  += new EventHandler(State_Changed);

                this.UCSys2.Add("<TD style='font-size:12px;text-align:left'>");
                this.UCSys2.Add(ddl);
                this.UCSys2.AddTDEnd();

                this.UCSys2.AddTREnd();

                if (this.NumKey == "" || this.NumKey == null)
                {
                    this.NumKey = attr.Key;
                    this.UCSys2.GetCBByID("CB_" + attr.Key).Checked = true;
                }
            }
            this.UCSys2.AddTableEnd();

            //			//this.DDL_GroupField.Items.Add(new ListItem(" The number of ","COUNT(*)"));
            //			this.DDL_GroupWay.Items.Add(new ListItem(" Sum ","0"));
            //			this.DDL_GroupWay.Items.Add(new ListItem(" Averaging ","1"));
            //
            //			this.DDL_Order.Items.Add(new ListItem(" Descending ","0"));
            //			this.DDL_Order.Items.Add(new ListItem(" Ascending ","1"));
            //
            //
            //			//this.DDL_GroupField.Items.Add(new ListItem(" The number of ","COUNT(*)"));
            //			this.DDL_GroupWay.Items.Add(new ListItem(" Sum ","0"));
            //			this.DDL_GroupWay.Items.Add(new ListItem(" Averaging ","1"));
            //
            //			this.DDL_Order.Items.Add(new ListItem(" Descending ","0"));
            //			this.DDL_Order.Items.Add(new ListItem(" Ascending ","1"));
        }
        #region  Method 
        /// <summary>
        ///  Nothing treatment choice .
        /// </summary>
        public void DealChoseNone()
        {
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
                }
            }

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
        }
        public DataTable BingDG()
        {
            this.DealChoseNone();

            Entities ens = this.HisEns;
            Entity en = ens.GetNewEntity;

            //  Check out the Events column set about it .
            ActiveAttrs aas = new ActiveAttrs();
            aas.RetrieveBy(ActiveAttrAttr.For, this.EnsName);

            Paras myps = new Paras();
            Map map = en.EnMap;
            //  Turn up   Packet data . 
            string groupKey = "";
            Attrs AttrsOfNum = new Attrs();
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

                AttrsOfNum.Add(map.GetAttrByKey(key));

                DDL ddl = this.UCSys2.GetDDLByID("DDL_" + key);
                if (ddl == null)
                {
                    ActiveAttr aa = (ActiveAttr)aas.GetEnByKey(ActiveAttrAttr.AttrKey, key);
                    if (aa == null)
                        continue;

                    Condition += aa.Condition;
                    groupKey += " round (" + aa.Exp + ", 4) AS " + key + ",";
                    StateNumKey += key + "=Checked@"; //  Record status 
                    //groupKey+=" round ( SUM("+key+"), 4) "+key+",";
                    //StateNumKey+=key+"=SUM@"; //  Record status 
                    continue;
                }

                switch (ddl.SelectedItemStringVal)
                {
                    case "SUM":
                        groupKey += " round ( SUM(" + key + "), 4) " + key + ",";
                        StateNumKey += key + "=SUM@"; //  Record status 
                        break;
                    case "AVG":
                        groupKey += " round (AVG(" + key + "), 4)  " + key + ",";
                        StateNumKey += key + "=AVG@"; //  Record status 
                        break;
                    case "AMOUNT":
                        groupKey += " round ( SUM(" + key + "), 4) " + key + ",";
                        StateNumKey += key + "=AMOUNT@"; //  Record status 
                        break;
                    default:
                        throw new Exception(" Without judgment .");
                }
            }

            bool isHaveLJ = false; //  Are there cumulative field .
            if (StateNumKey.IndexOf("AMOUNT@") != -1)
                isHaveLJ = true;

            if (groupKey == "")
            {
                this.UCSys1.AddMsgOfWarning(  " Warning ",
                    "<img src='../Img/Pub/warning.gif' /><b><font color=red>  You did not choose the data analyzed </font></b>"); // You did not choose the data analyzed .
                return null;
            }

            /*  If you include the accumulated data , That it certainly needs a month field . Business logic error .*/
            groupKey = groupKey.Substring(0, groupKey.Length - 1);
            BP.DA.Paras ps = new Paras();
            //  Generate  sql.
            string selectSQL = "SELECT ";
            string groupBy = " GROUP BY ";
            Attrs AttrsOfGroup = new Attrs();
            string StateGroupKey = "StateGroupKey=@"; //  To save operation state needs .
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
                    AttrsOfGroup.Add(map.GetAttrByKey(li.Value), false, false);
                    StateGroupKey += li.Value + "@";
                }
            }

            groupBy = groupBy.Substring(0, groupBy.Length - 1);

            #region  Generate Where  _OLD .    Through this process generates two  where.
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
                    sUr.MyPK = WebUser.No + this.EnsName + "_SearchAttrs";
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

                            //if (key == "FK_Unit")
                            //{
                            //    val = WebUser.FK_Unit;
                            //    ddl.SelectedIndex = 0;
                            //}
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
                                where += " FK_Dept LIKE '%'||:V_Dept||'%'   AND ";
                                break;
                            case DBType.Informix:
                                where += " FK_Dept LIKE '%'||:V_Dept||'%'   AND ";
                                break;
                            case DBType.MSSQL:
                            default:
                                where += " FK_Dept LIKE  "+SystemConfig.AppCenterDBVarStr+"V_Dept+'%'   AND ";
                                //  WHERE += " FK_Dept LIKE '@V_Dept%'   AND ";
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
                    Attrs attrs = en.EnMap.Attrs;
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

            string orderByReq=this.Request.QueryString["OrderBy"];
            string orderby = "";

            if (orderByReq != null)
            {
                //this.Alert(orderByReq + "  " + this.OrderWay);
                //this.ResponseWriteBlueMsg(selectSQL);
            }

            if (orderByReq != null && this.OrderBy != null && (selectSQL.Contains(orderByReq) || groupKey.Contains(orderByReq)))
            {
                orderby = " ORDER BY " + this.OrderBy;
                if (this.OrderWay != "Up")
                    orderby += " DESC ";
            }
             
            //  Assembled into the desired  sql 
            string sql = "";
            sql = selectSQL + groupKey + " FROM " + map.PhysicsTable + where + groupBy + orderby;

            //   Physical table .
            //  this.ResponseWriteBlueMsg(sql);
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
                foreach (Attr attr in AttrsOfNum)
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
                    foreach (Attr attr in AttrsOfNum)
                    {
                        if (StateNumKey.IndexOf(attr.Key + "=AMOUNT") == -1)
                            continue;

                        // Forming inquiry sql.
                        if (whereOfLJ.Length > 10)
                            sql = "SELECT SUM(" + attr.Key + ") FROM " + map.PhysicsTable + whereOfLJ + " AND ";
                        else
                            sql = "SELECT SUM(" + attr.Key + ") FROM " + map.PhysicsTable + " WHERE ";

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
            //  Generate header .
            this.UCSys1.AddTable("width='30%'");

            #region  Increase the grouping condition 
            if (StateNumKey.IndexOf("=AMOUNT") != -1)
            {
                /*  If you include the cumulative  */

                //  Increase the grouping condition .
                this.UCSys1.AddTR();  //  Beginning in the first column .
                this.UCSys1.Add("<td rowspan=2 class='GroupTitle'>ID</td>");
                foreach (Attr attr in AttrsOfGroup)
                {
                    this.UCSys1.Add("<td rowspan=2 class='GroupTitle'>" + attr.Desc + "</td>");
                }
                //  Increase data columns 
                foreach (Attr attr in AttrsOfNum)
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
                foreach (Attr attr in AttrsOfNum)
                {
                    if (StateNumKey.IndexOf(attr.Key + "=AMOUNT") == -1)
                        continue;

                    this.UCSys1.Add("<td class='GroupTitle'> This month </td>"); // This month  this.ToE("OrderCondErr")
                    this.UCSys1.Add("<td class='GroupTitle'> Grand total </td>"); // Grand total 
                }
                this.UCSys1.AddTR();
            }
            else  /*  No case of total  */
            {
                this.UCSys1.AddTR();
                this.UCSys1.AddTDGroupTitle("ID");

                //  Grouping criteria 
                foreach (Attr attr in AttrsOfGroup)
                {
                    if (this.OrderBy == attr.Key)
                    {
                        switch (this.OrderWay)
                        {
                            case "Down":
                                this.UCSys1.AddTDGroupTitle("<a href='Group.aspx?EnsName=" + this.EnsName + "&OrderBy=" + attr.Key + "&OrderWay=Up' >" + attr.Desc + "<img src='../Img/ArrDown.gif' border=0/></a>");
                                break;
                            case "Up":
                            default:
                                this.UCSys1.AddTDGroupTitle("<a href='Group.aspx?EnsName=" + this.EnsName + "&OrderBy=" + attr.Key + "&OrderWay=Down' >" + attr.Desc + "<img src='../Img/ArrUp.gif' border=0/></a>");
                                break;
                        }
                    }
                    else
                    {
                        this.UCSys1.AddTDGroupTitle("<a href='Group.aspx?EnsName=" + this.EnsName + "&OrderBy=" + attr.Key + "&OrderWay=Down' >" + attr.Desc + "</a>");
                    }
                }

                //  Packet data 
                foreach (Attr attr in AttrsOfNum)
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
                                    this.UCSys1.AddTDGroupTitle(lab + "<a href='Group.aspx?EnsName=" + this.EnsName + "&NumKey=" + attr.Key + "&OrderBy=" + attr.Key + "&OrderWay=Up'><img src='../Img/ArrDown.gif' border=0/></a>");
                                else
                                    this.UCSys1.AddTDGroupTitle("<a href=\"Group.aspx?EnsName=" + this.EnsName + "&NumKey=" + attr.Key + "\" >" + lab + "</a><a href='Group.aspx?EnsName=" + this.EnsName + "&NumKey=" + attr.Key + "&OrderBy=" + attr.Key + "&OrderWay=Up'><img src='../Img/ArrDown.gif' border=0/></a>");
                                break;
                            case "Up":
                            default:
                                if (this.NumKey == attr.Key)
                                    this.UCSys1.AddTDGroupTitle(lab + "<a href='Group.aspx?EnsName=" + this.EnsName + "&OrderBy=" + attr.Key + "&NumKey=" + attr.Key + "&OrderWay=Down'><img src='../Img/ArrUp.gif' border=0/></a>");
                                else
                                    this.UCSys1.AddTDGroupTitle("<a href=\"Group.aspx?EnsName=" + this.EnsName + "&NumKey=" + attr.Key + "\" >" + lab + "</a><a href='Group.aspx?EnsName=" + this.EnsName + "&OrderBy=" + attr.Key + "&NumKey=" + attr.Key + "&OrderWay=Down'><img src='../Img/ArrUp.gif' border=0/></a>");
                                break;
                        }
                    }
                    else
                    {
                        if (this.NumKey == attr.Key)
                            this.UCSys1.AddTDGroupTitle(lab + "<a href='Group.aspx?EnsName=" + this.EnsName + "&NumKey=" + attr.Key + "&OrderBy=" + attr.Key + "' ><img src='../Img/ArrDownUp.gif' border=0/></a>");
                        else
                            this.UCSys1.AddTDGroupTitle("<a href=\"Group.aspx?EnsName=" + this.EnsName + "&NumKey=" + attr.Key + "\" >" + lab + "</a><a href='Group.aspx?EnsName=" + this.EnsName + "&NumKey=" + attr.Key + "&OrderBy=" + attr.Key + "' ><img src='../Img/ArrDownUp.gif' border=0/></a>");

                    }
                }
                this.UCSys1.AddTDGroupTitle("");
                this.UCSys1.AddTREnd();
            }
            #endregion  Generate header 

            #region  To generate the query 
            string YSurl = "ContrastDtl.aspx?EnsName=" + this.EnsName;
            string keys = "";

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

            foreach (AttrSearch a23 in map.SearchAttrs)
            {
                Attr attrS = a23.HisAttr;

                if (attrS.MyFieldType == FieldType.RefText)
                    continue;

                if (IsHaveFK_Dept && attrS.Key == "FK_Dept")
                    continue;

                //ToolbarDDL ddl = (ToolbarDDL)ctl;
                string val = this.ToolBar1.GetDDLByKey("DDL_" + attrS.Key).SelectedItemStringVal;
                if (val == "all")
                    continue;
                keys += "&" + attrS.Key + "=" + val;
            }
            YSurl = YSurl + keys;
            #endregion

            //this.Table =dt;

            #region  Generate foreign keys 
            //  To expand the foreign key table 
            foreach (Attr attr in AttrsOfGroup)
            {
                dt.Columns.Add(attr.Key + "T", typeof(string));
            }
            foreach (Attr attr in AttrsOfGroup)
            {
                if (attr.UIBindKey.IndexOf(".") == -1)
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
                    Entity myen = attr.HisFKEn;
                    string val = dr[attr.Key].ToString();
                    myen.SetValByKey(attr.UIRefKeyValue, val);
                    try
                    {
                        myen.Retrieve();
                        //  dr[attr.Key + "T"] = val + myen.GetValStringByKey(attr.UIRefKeyText);
                        //   dr[attr.Key + "T"] = myen.GetValStrByKey(attr.UIRefKeyValue)+ myen.GetValStrByKey(attr.UIRefKeyText);
                        dr[attr.Key + "T"] = myen.GetValStrByKey(attr.UIRefKeyText);
                    }
                    catch 
                    {
                        if (val == null || val.Length <= 1)
                        {
                            dr[attr.Key + "T"] = val;
                        }
                        else if (val.Substring(0, 2) == "63")
                        {
                            try
                            {
                                BP.Port.Dept Dept = new BP.Port.Dept(val);
                                dr[attr.Key + "T"] = Dept.Name;
                            }
                            catch
                            {
                                dr[attr.Key + "T"] = val;
                            }
                        }
                        else
                        {
                            dr[attr.Key + "T"] = val;
                        }
                    }
                }
            }
            #endregion

            #region  Generate Body 
            int i = 0;
            bool is1 = false;
            foreach (DataRow dr in dt.Rows)
            {
                i++;

                url = YSurl.Clone() as string;
                string keyActive = "";
                //  Produce url .
                foreach (Attr attr in AttrsOfGroup)
                {
                    url += "&" + attr.Key + "=" + dr[attr.Key].ToString();
                    //keyActive+="&"+attr.Key+"="+dr[attr.Key].ToString() ; 
                }

                is1 = this.UCSys1.AddTR(is1);
                // this.UCSys1.AddTRTXHand("onclick=\"WinOpen('" + url + "','dtl');\" ");

                this.UCSys1.AddTDIdx(int.Parse(dr["IDX"].ToString()));
                //  Grouping criteria 
                foreach (Attr attr in AttrsOfGroup)
                {
                    this.UCSys1.AddTD(dr[attr.Key + "T"].ToString());
                }

                //  Packet data 
                foreach (Attr attr in AttrsOfNum)
                {
                    decimal obj = 0;
                    try
                    {
                        obj = decimal.Parse(dr[attr.Key].ToString());
                    }
                    catch (Exception ex)
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
                this.UCSys1.AddTD("<a href=\"javascript:WinOpen('" + url + "','s','900', '600')\" > Detailed </a>");
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
            string NoShowSum = SystemConfig.GetConfigXmlEns("NoShowSum", this.EnsName);
            if (NoShowSum == null)
                NoShowSum = "";

            Attrs AttrsOfNum1 = AttrsOfNum.Clone();
            decimal d = 0;
            foreach (Attr attr in AttrsOfNum)
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
                        foreach (Attr myattr in AttrsOfNum1)
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
                    else
                    {

                    }
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

            #region  Generate   Graph 
            this.BPTabStrip1.Visible = this.CB_IsShowPict.Checked;
            //if (AttrsOfGroup.Count==1)
            if (this.CB_IsShowPict.Checked)
            {
                /*  In the case of  1 Î³ */
                string colOfGroupField = "";
                string colOfGroupName = "";
                string colOfNumField = "";
                string colOfNumName = "";
                string title = "";
                int chartHeight = this.TB_H.TextExtInt;
                int chartWidth = this.TB_W.TextExtInt;


                if (isHaveLJ)
                {
                    /*   If there is a total of ,  In accordance with the cumulative field analysis .*/
                    colOfGroupField = AttrsOfGroup[0].Key;
                    colOfGroupName = AttrsOfGroup[0].Desc;

                    colOfNumName = AttrsOfNum[0].Desc;
                    if (dt.Columns.Contains(AttrsOfNum[0].Key + "AMOUNT"))
                        colOfNumField = AttrsOfNum[0].Key + "AMOUNT";
                    else
                        colOfNumField = AttrsOfNum[0].Key;
                }
                else
                {
                    colOfGroupField = AttrsOfGroup[0].Key;
                    colOfGroupName = AttrsOfGroup[0].Desc;

                    if (NumKey == null)
                    {
                        colOfNumName = AttrsOfNum[0].Desc;
                        colOfNumField = AttrsOfNum[0].Key;
                    }
                    else
                    {
                        //  colOfNumField = AttrsOfNum[0].Key;
                        colOfNumName = map.GetAttrByKey(NumKey).Desc; // this.UCSys1.get;
                        colOfNumField = NumKey;
                    }
                }


                string colOfNumName1 = "";
                if (StateNumKey.Contains(this.NumKey + "=SUM"))
                    colOfNumName1 = "( Total )" + colOfNumName;
                else
                    colOfNumName1 = "( Average )" + colOfNumName;


                //  DataTable dtChart = this.DealTable(dt);


                try
                {
                    this.Img1.ImageUrl = "../Temp/" + CCFlow.WF.Comm.UC.UCSys.GenerChart(dt,
                        colOfGroupField + "T", colOfGroupName,
                        colOfNumField, colOfNumName1
                        , "", chartHeight, chartWidth, ChartType.Histogram);

                    this.Img2.ImageUrl = "../Temp/" + BP.Web.UC.UCGraphics.GenerChart(dt,
                        colOfGroupField + "T", colOfGroupName,
                        colOfNumField, colOfNumName1
                        , "", chartHeight, chartWidth, ChartType.Pie);

                    this.Img3.ImageUrl = "../Temp/" + BP.Web.UC.UCGraphics.GenerChart(dt,
                        colOfGroupField + "T", colOfGroupName,
                        colOfNumField, colOfNumName1
                        , "", chartHeight, chartWidth, ChartType.Line);
                }
                catch (Exception ex)
                {
                    this.ResponseWriteRedMsg("@ Image files generated an error :" + ex.Message);
                    ///return;
                }

                this.BPTabStrip1.Items[0].Text =  " Form ";
                // this.BPTabStrip1.Items[0].Text = this.ToE("TableGrade", " Form -<a href=\"javascript:WinOpen('./Rpt/Adv.aspx')\" > Advanced </a>");

                this.BPTabStrip1.Items[2].Text =   colOfNumName + "- Histogram ";
                this.BPTabStrip1.Items[4].Text =  colOfNumName + "- Pie ";
                this.BPTabStrip1.Items[6].Text =   colOfNumName + "- Line chart ";
            }
            #endregion

            #region  Save state 
            //if (this.IsPostBack)
            //{
            //    this.ResponseWriteBlueMsg("hi");
            //  Save state .

            ur.Vals = StateGroupKey + StateNumKey;
            ur.CfgKey = this.EnsName + "_Group";
            ur.FK_Emp = WebUser.NoOfSessionID;
            ur.OrderBy = this.OrderBy;
            ur.OrderWay = this.OrderWay;
            ur.IsPic = this.CB_IsShowPict.Checked;
            ur.GenerSQL = myps.SQL;
            ur.NumKey = this.NumKey;
            ur.Paras = "";
            foreach (Para para in myps)
            {
                ur.Paras += "@" + para.ParaName + "=" + para.val;
            }
            ur.Save();

            this.SetValueByKey("Vals", ur.Vals);
            this.SetValueByKey("CfgKey", ur.CfgKey);
            this.SetValueByKey("OrderBy", ur.OrderBy);
            this.SetValueByKey("OrderWay", ur.OrderWay);
            this.SetValueByKey("IsPic", ur.IsPic);
            this.SetValueByKey("SQL", ur.GenerSQL);
            this.SetValueByKey("NumKey", ur.NumKey);

            //ur.Save();
            //  }
            this.CfgVal = ur.Vals;
            #endregion

            return dt1;
        }
         
        //public string Vals = null;
        //public string CfgKey = null;
        //public string OrderBy = null;
        //public string OrderWay = null;
        //public bool IsPic = false;
        //public bool NumKey = false;
        //public bool Paras = false;
        //public bool SQL = false;


        public DataTable DealTable(DataTable dt)
        {
            DataTable dtCopy = new DataTable();

            #region  Converts them to  string  Type .
            foreach (DataColumn dc in dt.Columns)
                dtCopy.Columns.Add(dc.ColumnName, typeof(string));

            foreach (DataRow dr in dt.Rows)
                dtCopy.ImportRow(dr);
            #endregion

            Entity en = this.HisEn;
            Map map = en.EnMap;
            Attrs attrs = en.EnMap.Attrs;
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
            //foreach (DataColumn dc in dtCopy.Columns)
            //{
            //   // Attrs
            //   // foreach (BP.En.Attr attr in attrs)
            //    //{
            //    //}
            //}
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
            Btn btn = (Btn)sender;
            switch (btn.ID)
            {
                case NamesOfBtn.Save:
                    GroupEnsTemplates rts = new GroupEnsTemplates();
                    GroupEnsTemplate rt = new GroupEnsTemplate();
                    rt.EnsName = this.EnsName;
                    //rt.Name=""
                    string name = "";
                    //string opercol="";
                    string attrs = "";
                    foreach (ListItem li in CheckBoxList1.Items)
                    {
                        if (li.Selected)
                        {
                            attrs += "@" + li.Value;
                            name += li.Text + "_";
                        }
                    }

                    name = this.HisEn.EnDesc + name.Substring(0, name.Length - 1);
                    if (rt.Search(WebUser.No, this.EnsName, attrs) >= 1)
                    {
                        this.InvokeEnManager(rts.ToString(), rt.OID.ToString(), true);
                        return;
                    }
                    rt.Name = name;
                    rt.Attrs = attrs;
                    //rt.OperateCol=this.DDL_GroupField.SelectedItemStringVal+"@"+this.DDL_GroupWay.SelectedItemStringVal;
                    rt.Rec = WebUser.No;
                    rt.EnName = this.EnsName;
                    rt.EnName = this.HisEn.EnMap.EnDesc;
                    rt.Save();
                    this.InvokeEnManager(rts.ToString(), rt.OID.ToString(), true);
                    //	this.ResponseWriteBlueMsg(" The current template has joined the queue custom reports , Click here <a href');\" Edit Custom Report </a>");
                    break;
                case NamesOfBtn.Help:
                    this.Helper();
                    break;
                case NamesOfBtn.Excel:
                    DataTable dt = this.BingDG();
                    this.ExportDGToExcel(this.DealTable(dt), this.HisEns.GetNewEntity.EnDesc);
                    return;
                default:
                    this.ToolBar1.SaveSearchState(this.EnsName, this.Key);
                    if (this.IsPostBack)
                    {
                        this.ur = new UserRegedit(WebUser.NoOfSessionID, this.EnsName + "_Group");
                        ur.Vals = this.GetValueByKey("Vals");
                        ur.CfgKey = this.GetValueByKey("CfgKey");
                        ur.OrderBy = this.GetValueByKey("OrderBy");
                        ur.OrderWay = this.GetValueByKey("OrderWay");
                        ur.IsPic = bool.Parse(this.GetValueByKey("IsPic"));
                        ur.GenerSQL = this.GetValueByKey("SQL");
                        ur.NumKey = this.GetValueByKey("NumKey");
                        ur.Save();
                    }
                    this.BingDG();
                    // this.Response.Redirect(this.Request.RawUrl, true);
                    return;
            }
            //this.SetDGDataV2();
        }

        void State_Changed(object sender, EventArgs e)
        {
            //this.ur = new UserRegedit(WebUser.No, this.EnsName + "_Group");
            //ur.IsPic = this.CB_IsShowPict.Checked;
            //ur.Save();
            this.BingDG();
        }
        void CheckBoxList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BingDG();
        }

    }
}
