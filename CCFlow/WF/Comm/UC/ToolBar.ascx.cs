using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BP.En;
using BP.Sys;
using BP.DA;
using BP.Web;
using BP.Web.Controls;
using BP.Web.UC;
using BP.XML;
using BP.Sys.Xml;
using BP.Port;
using BP;

namespace CCFlow.WF.Comm.UC
{
    public partial class ToolBar : BP.Web.UC.UCBase3
    {
        #region  Method 
        public new string EnsName
        {
            get
            {
                return this.Request.QueryString["EnsName"];
            }
        }
        /// <summary>
        ///  Whether to add the Query button 
        /// </summary>
        public bool _AddSearchBtn = true;
        public void AddSpt(string id)
        {
            this.Add("&nbsp;");
        }

        private string GetTextById(string id)
        {
            string text = "";
            switch (id)
            {
                case NamesOfBtn.UnDo:
                    text = " Undo operation ";
                    break;
                case NamesOfBtn.Do:
                    text = " Carried out ";
                    break;
                case NamesOfBtn.ChoseField:
                    text = " Select the field ";
                    break;
                case NamesOfBtn.DataGroup:
                    text = " Grouping queries ";
                    break;
                case NamesOfBtn.Copy:
                    text = " Copy ";
                    break;
                case NamesOfBtn.Go:
                    text = " Go to ";
                    break;
                case NamesOfBtn.ExportToModel:
                    text = " Template ";
                    break;
                case NamesOfBtn.DataCheck:
                    text = " Data Check ";
                    break;
                case NamesOfBtn.DataIO:
                    text = " Data Import ";
                    break;
                case NamesOfBtn.Statistic:
                    text = " Statistics ";
                    break;
                case NamesOfBtn.Balance:
                    text = " Fair ";
                    break;
                case NamesOfBtn.Down:
                    text = " Drop ";
                    break;
                case NamesOfBtn.Up:
                    text = " Rise ";
                    break;
                case NamesOfBtn.Chart:
                    text = " Graph ";
                    break;
                case NamesOfBtn.Rpt:
                    text = " Report form ";
                    break;
                case NamesOfBtn.ChoseCols:
                    text = " Select the column of the query ";
                    break;
                case NamesOfBtn.Excel:
                    text = " Export all ";
                    break;
                case NamesOfBtn.Excel_S:
                    text = " Export Current ";
                    break;
                case NamesOfBtn.Xml:
                    text = " Export to Xml";
                    break;
                case NamesOfBtn.Send:
                    text = " Send ";
                    break;
                case NamesOfBtn.Reply:
                    text = " Reply ";
                    break;
                case NamesOfBtn.Forward:
                    text = " Forwarding ";
                    break;
                case NamesOfBtn.Next:
                    text = " Next ";
                    break;
                case NamesOfBtn.Previous:
                    text = " Previous ";
                    break;
                case NamesOfBtn.Selected:
                    text = " Choose ";
                    break;
                case NamesOfBtn.Add:
                    text = " Increase ";
                    break;
                case NamesOfBtn.Adjunct:
                    text = " Accessory ";
                    break;
                case NamesOfBtn.AllotTask:
                    text = " Batch task ";
                    break;
                case NamesOfBtn.Apply:
                    text = " Application ";
                    break;
                case NamesOfBtn.ApplyTask:
                    text = " Application task ";
                    break;
                case NamesOfBtn.Back:
                    text = " Retreat ";
                    break;
                case NamesOfBtn.Card:
                    text = " Card ";
                    break;
                case NamesOfBtn.Close:
                    text = " Shut down ";
                    break;
                case NamesOfBtn.Confirm:
                    text = " Determine ";
                    break;
                case NamesOfBtn.Delete:
                    text = " Delete ";
                    break;
                case NamesOfBtn.Edit:
                    text = " Editor ";
                    break;
                case NamesOfBtn.EnList:
                    text = " List ";
                    break;
                case NamesOfBtn.Cancel:
                    text = " Cancel ";
                    break;
                case NamesOfBtn.Export:
                    text = " Export ";
                    break;
                case NamesOfBtn.FileManager:
                    text = " File Management ";
                    break;
                case NamesOfBtn.Help:
                    text = " Help ";
                    break;
                case NamesOfBtn.Insert:
                    text = " Insert ";
                    break;
                case NamesOfBtn.LogOut:
                    text = "Log Out";
                    break;
                case NamesOfBtn.Messagers:
                    text = " News ";
                    break;
                case NamesOfBtn.New:
                    text = " New ";
                    //  text = " New ";
                    break;
                case NamesOfBtn.Print:
                    text = " Print ";
                    break;
                case NamesOfBtn.Refurbish:
                    text = " Refresh ";
                    break;
                case NamesOfBtn.Reomve:
                    text = " Remove ";
                    break;
                case NamesOfBtn.Save:
                    text = " Save ";
                    break;
                case NamesOfBtn.SaveAndClose:
                    text = " Save and Close ";
                    break;
                case NamesOfBtn.SaveAndNew:
                    text = " Save and New ";
                    break;
                case NamesOfBtn.SaveAsDraft:
                    text = " Save Draft ";
                    break;
                case NamesOfBtn.Search:
                    text = " Find (F)";
                    break;
                case NamesOfBtn.SelectAll:
                    text = " Select all ";
                    break;
                case NamesOfBtn.SelectNone:
                    text = " Uncheck ";
                    break;
                case NamesOfBtn.View:
                    text = " Check out ";
                    break;
                case NamesOfBtn.Update:
                    text = " Update ";
                    break;
                default:
                    throw new Exception("@ Not defined ToolBarBtn  Mark  " + id);
            }

            return text;
        }

        public void AddBtn(string id)
        {
            var btn = new Btn();
            btn.Attributes["class"] = "Btn";

            btn.ID = id;
            btn.Text = GetTextById(id);

            switch (id)
            {
                case NamesOfBtn.Delete:
                    btn.Attributes["onclick"] = "return confirm(' Are you sure you want to delete it ?');";
                    break;
                default:
                    break;
            }

            this.Add(btn);
        }

        public void AddLinkBtn(string id)
        {
            var btn = new LinkBtn(true, id, GetTextById(id));

            switch (id)
            {
                case NamesOfBtn.Delete:
                    btn.Attributes["onclick"] = "return confirm(' Are you sure you want to delete it ?');";
                    break;
                default:
                    break;
            }

            this.Add(btn);
        }

        public void AddLab(string id, string lab)
        {
            Label en = new Label();
            en.ID = id;
            en.Text = lab;
            this.Add(en);
        }
        public bool IsExitsContral(string ctrlID)
        {
            if (this.FindControl(ctrlID) == null)
                return false;
            return true;
        }
        public void AddBtn(string id, string text)
        {
            Btn en = new Btn();
            en.ID = id;
            en.Text = text;
            //en.Attributes["class"] = "Btn";            if (id == "Btn_Delete")
            //   en.Attributes["onclick"] = "return confirm(' Are you sure you want to delete it ?');";
            this.Add(en);
        }

        public void AddLinkBtn(string id, string text)
        {
            var en = new LinkBtn(true, id, text);

            if (id == "Btn_Delete")
                en.Attributes["onclick"] = "return confirm(' Are you sure you want to delete it ?');";

            this.Add(en);
        }

        public void Btn_Click(object sender, EventArgs e)
        {

        }
        public void AddTB(string id, string text)
        {
            TB en = new TB();
            en.ID = id;
            en.Text = text;
            this.Add(en);
        }
        public void AddTB(string id)
        {
            this.AddTB(id);
        }
        public void AddTB(TB tb)
        {
            this.Add(tb);
        }
        public void AddDDL(DDL ddl)
        {
            this.Add(ddl);
        }
        public void AddDDL(string ddl)
        {
            DDL d = new DDL();
            d.ID = ddl;
            this.Add(d);
        }

        public DDL GetDDLByKey(string key)
        {
            return this.FindControl(key) as DDL;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        public QueryObject GetnQueryObjectOracle(Entities ens, Entity en)
        {
            QueryObject qo = this.InitQueryObjectByEns(ens, en.EnMap.IsShowSearchKey, en.EnMap.DTSearchWay, en.EnMap.DTSearchKey,
                en.EnMap.Attrs, en.EnMap.AttrsOfSearch, en.EnMap.SearchAttrs);
            string pk = en.PK;
            if (en.EnMap.Attrs.Contains("No"))
                qo.addOrderBy("No");
            return qo;
        }
        public QueryObject InitQueryObjectByEns(Entities ens, bool IsShowSearchKey, DTSearchWay dw, string dtKey, Attrs attrs, AttrsOfSearch attrsOfSearch, AttrSearchs searchAttrs)
        {
            QueryObject qo = new QueryObject(ens);

            #region  Keyword 
            string keyVal = "";
            //Attrs attrs = en.EnMap.Attrs;
            if (IsShowSearchKey)
            {
                keyVal = this.GetTBByID("TB_Key").Text.Trim();
                this.Page.Session["SKey"] = keyVal;
            }

            if (keyVal.Length >= 1)
            {
                Attr attrPK = new Attr();
                foreach (Attr attr in attrs)
                {
                    if (attr.IsPK)
                    {
                        attrPK = attr;
                        break;
                    }
                }
                int i = 0;
                foreach (Attr attr in attrs)
                {

                    switch (attr.MyFieldType)
                    {
                        case FieldType.Enum:
                        case FieldType.FK:
                        case FieldType.PKFK:
                            continue;
                        default:
                            break;
                    }


                    if (attr.MyDataType != DataType.AppString)
                        continue;

                    if (attr.MyFieldType == FieldType.RefText)
                        continue;

                    if (attr.Key == "FK_Dept")
                        continue;

                    i++;
                    if (i == 1)
                    {
                        /*  The first came in . */
                        qo.addLeftBracket();
                        if (SystemConfig.AppCenterDBVarStr == "@")
                            qo.AddWhere(attr.Key, " LIKE ", " '%'+" + SystemConfig.AppCenterDBVarStr + "SKey+'%'");
                        else
                            qo.AddWhere(attr.Key, " LIKE ", " '%'||" + SystemConfig.AppCenterDBVarStr + "SKey||'%'");
                        continue;
                    }
                    qo.addOr();

                    if (SystemConfig.AppCenterDBVarStr == "@")
                        qo.AddWhere(attr.Key, " LIKE ", "'%'+" + SystemConfig.AppCenterDBVarStr + "SKey+'%'");
                    else
                        qo.AddWhere(attr.Key, " LIKE ", "'%'||" + SystemConfig.AppCenterDBVarStr + "SKey||'%'");

                }
                qo.MyParas.Add("SKey", keyVal);
                qo.addRightBracket();
            }
            else
            {
                qo.addLeftBracket();
                qo.AddWhere("abc", "all");
                qo.addRightBracket();
            }
            #endregion

            #region  Common attributes 
            string opkey = ""; //  Operation Symbol .
            foreach (AttrOfSearch attr in attrsOfSearch)
            {
                if (attr.IsHidden)
                {
                    qo.addAnd();
                    qo.addLeftBracket();
                    qo.AddWhere(attr.RefAttrKey, attr.DefaultSymbol, attr.DefaultValRun);
                    qo.addRightBracket();
                    continue;
                }

                if (attr.SymbolEnable == true)
                {
                    opkey = this.GetDDLByKey("DDL_" + attr.Key).SelectedItemStringVal;
                    if (opkey == "all")
                        continue;
                }
                else
                {
                    opkey = attr.DefaultSymbol;
                }

                qo.addAnd();
                qo.addLeftBracket();

                if (attr.DefaultVal.Length >= 8)
                {
                    string date = "2005-09-01";
                    try
                    {
                        /*  It could be date . */
                        string y = this.GetDDLByKey("DDL_" + attr.Key + "_Year").SelectedItemStringVal;
                        string m = this.GetDDLByKey("DDL_" + attr.Key + "_Month").SelectedItemStringVal;
                        string d = this.GetDDLByKey("DDL_" + attr.Key + "_Day").SelectedItemStringVal;
                        date = y + "-" + m + "-" + d;

                        if (opkey == "<=")
                        {
                            DateTime dt = DataType.ParseSysDate2DateTime(date).AddDays(1);
                            date = dt.ToString(DataType.SysDataFormat);
                        }
                    }
                    catch
                    {
                    }

                    qo.AddWhere(attr.RefAttrKey, opkey, date);
                }
                else
                {
                    qo.AddWhere(attr.RefAttrKey, opkey, this.GetTBByID("TB_" + attr.Key).Text);
                }
                qo.addRightBracket();
            }
            #endregion

            #region  Foreign key 
            foreach (AttrSearch attr1 in searchAttrs)
            {
                Attr attr = attr1.HisAttr;

                if (attr.MyFieldType == FieldType.RefText)
                    continue;


                DDL ddl = this.GetDDLByKey("DDL_" + attr.Key);
                if (ddl.Items.Count == 0)
                    continue;

                string selectVal = ddl.SelectedItemStringVal;
                if (selectVal == "all" || selectVal == "-1")
                    continue;

                if (selectVal == "mvals")
                {
                    UserRegedit sUr = new UserRegedit();
                    sUr.MyPK = WebUser.No + this.EnsName + "_SearchAttrs";
                    sUr.RetrieveFromDBSources();

                    /*  If the value is multiple choice  */
                    string cfgVal = sUr.MVals;
                    AtPara ap = new AtPara(cfgVal);
                    string instr = ap.GetValStrByKey(attr.Key);
                    if (instr == null || instr == "")
                    {
                        if (attr.Key == "FK_Dept" || attr.Key == "FK_Unit")
                        {
                            if (attr.Key == "FK_Dept")
                            {
                                selectVal = WebUser.FK_Dept;
                                ddl.SelectedIndex = 0;
                            }

                            //if (attr.Key == "FK_Unit")
                            //{
                            //    selectVal = WebUser.FK_Unit;
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

                        qo.addAnd();
                        qo.addLeftBracket();
                        qo.AddWhereIn(attr.Key, "(" + instr + ")");
                        qo.addRightBracket();
                        continue;
                    }
                }


                qo.addAnd();
                qo.addLeftBracket();

                if (attr.UIBindKey == "BP.Port.Depts" || attr.UIBindKey == "BP.Port.Units")  // Judgment special circumstances .
                    qo.AddWhere(attr.Key, " LIKE ", selectVal + "%");
                else
                    qo.AddWhere(attr.Key, selectVal);

                //qo.AddWhere(attr.Key,this.GetDDLByKey("DDL_"+attr.Key).SelectedItemStringVal ) ;
                qo.addRightBracket();
            }
            #endregion.

            if (dw != DTSearchWay.None)
            {
                string dtFrom = this.GetTBByID("TB_S_From").Text.Trim();
                string dtTo = this.GetTBByID("TB_S_To").Text.Trim();
                if (dw == DTSearchWay.ByDate)
                {
                    qo.addAnd();
                    qo.addLeftBracket();
                    qo.SQL = dtKey + " >= '" + dtFrom + " 01:01'";
                    qo.addAnd();
                    qo.SQL = dtKey + " <= '" + dtTo + " 23:59'";
                    qo.addRightBracket();

                    //qo.AddWhere(dtKey, ">=", dtFrom+" 01:01");
                    //qo.addAnd();
                    //qo.AddWhere(dtKey, "<=", dtTo + " 23:59");
                    //qo.addRightBracket();
                }

                if (dw == DTSearchWay.ByDateTime)
                {
                    qo.addAnd();
                    qo.addLeftBracket();
                    qo.SQL = dtKey + " >= '" + dtFrom + "'";
                    qo.addAnd();
                    qo.SQL = dtKey + " <= '" + dtTo + "'";
                    qo.addRightBracket();

                    //qo.addAnd();
                    //qo.addLeftBracket();
                    //qo.AddWhere(dtKey, ">=", dtFrom);
                    //qo.addAnd();
                    //qo.AddWhere(dtKey, "<=", dtTo);
                    //qo.addRightBracket();
                }
            }

            //  throw new Exception(qo.SQL);
            return qo;
        }
        public QueryObject GetnQueryObject(Entities ens, Entity en)
        {
            if (en.EnMap.EnDBUrl.DBType == DBType.Oracle)
                return this.GetnQueryObjectOracle(ens, en);

            QueryObject qo = this.InitQueryObjectByEns(ens, en.EnMap.IsShowSearchKey, en.EnMap.DTSearchWay, en.EnMap.DTSearchKey, en.EnMap.Attrs,
                en.EnMap.AttrsOfSearch, en.EnMap.SearchAttrs);

            switch (en.PK)
            {
                case "No":
                    qo.addOrderBy("No");
                    break;
                case "OID":
                    qo.addOrderBy("OID");
                    break;
                default:
                    break;
            }
            return qo;
        }

        public void SaveSearchState(string ensName, string key)
        {
            if (string.IsNullOrEmpty(ensName))
                throw new Exception("@EnsName  Null " + ensName);

            UserRegedit ur = new UserRegedit();
            ur.MyPK = WebUser.No + ensName + "_SearchAttrs";
            ur.RetrieveFromDBSources();
            ur.FK_Emp = WebUser.No;
            ur.CfgKey = "SearchAttrs";
            ur.SearchKey = key;

            if (key == "" || key == null)
            {
                try
                {
                    ur.SearchKey = this.GetTBByID("TB_Key").Text;
                }
                catch
                {
                }
            }

            // Query time .
            try
            {
                ur.DTFrom_Datatime = this.GetTBByID("TB_S_From").Text;
                ur.DTTo_Datatime = this.GetTBByID("TB_S_To").Text;
            }
            catch
            {
            }

            string str = "";
            foreach (Control ti in this.Controls)
            {
                if (ti.ID == null)
                    continue;

                if (ti.ID.IndexOf("DDL_") == -1)
                    continue;

                DDL ddl = (DDL)ti;
                if (ddl.Items.Count == 0)
                    continue;

                str += "@" + ti.ID + "=" + ddl.SelectedItemStringVal;
            }
            ur.FK_Emp = WebUser.No;
            ur.CfgKey = ensName + "_SearchAttrs";
            ur.Vals = str;
            try
            {
                ur.SearchKey = this.GetTBByID("TB_Key").Text;
            }
            catch
            {
            }
            ur.Save();
        }
        public void InitFuncEn(UAC uac, Entity en)
        {
            if (en.EnMap.EnType == EnType.View)
                uac.Readonly();


            if (uac.IsInsert)
            {
                if (en.EnMap.EnType != EnType.Dtl)
                {
                    this.AddLinkBtn(NamesOfBtn.New, " New (N)");
                    //this.GetLinkBtnByID(NamesOfBtn.New).OnClientClick = "this.disabled=true;"; 
                }
            }

            if (uac.IsUpdate)
            {
                this.AddLinkBtn(NamesOfBtn.Save, " Save (S)");
                this.AddLinkBtn(NamesOfBtn.SaveAndClose, " Save and Close ");
                //this.GetLinkBtnByID(NamesOfBtn.Save).OnClientClick = "this.disabled=true;"; 
            }

            if (uac.IsInsert && uac.IsUpdate)
            {
                if (en.EnMap.EnType != EnType.Dtl)
                {
                    this.AddLinkBtn(NamesOfBtn.SaveAndNew, " Save and New (R)");
                    //this.GetLinkBtnByID(NamesOfBtn.SaveAndNew).OnClientClick = "this.disabled=true;"; 
                }
            }

            string pkval = en.PKVal.ToString();

            if (uac.IsDelete && pkval != "0" && pkval.Length >= 1)
            {
                this.AddLinkBtn(NamesOfBtn.Delete, " Delete (D)");
                // this.GetLinkBtnByID(NamesOfBtn.Delete).OnClientClick = "this.disabled=true;"; 
            }

            if (uac.IsAdjunct)
            {
                this.AddLinkBtn(NamesOfBtn.Adjunct);
                if (en.IsEmpty == false)
                {
                    int i = DBAccess.RunSQLReturnValInt("select COUNT(*) from Sys_FileManager WHERE RefTable='" + en.ToString() + "' AND RefKey='" + en.PKVal + "'");
                    if (i != 0)
                    {
                        this.GetLinkBtnByID(NamesOfBtn.Adjunct).Text += "-" + i;
                    }
                }
            }
        }

        public void InitByMapV2(Map map, int page)
        {
            string str = this.Page.Request.QueryString["EnsName"];
            if (str == null)
                str = this.Page.Request.QueryString["EnsName"];

            if (str == null)
                return;

            InitByMapV2(map, page, str);
        }
        /// <summary>
        ///  Initialization map
        /// </summary>
        /// <param name="map">map</param>
        /// <param name="i"> Selected page </param>
        public void InitByMapV2(Map map, int page, string ensName)
        {
            UserRegedit ur = new UserRegedit(WebUser.No, ensName + "_SearchAttrs");
            string cfgKey = ur.Vals;
            this.InitByMapV2(map.IsShowSearchKey, map.DTSearchWay, map.AttrsOfSearch, map.SearchAttrs, null, page, ur);

            #region  Set Default 
            string[] keys = cfgKey.Split('@');
            foreach (Control ti in this.Controls)
            {
                if (ti.ID == null)
                    continue;

                if (ti.ID == "TB_Key")
                {
                    TB tb = (TB)ti;
                    tb.Text = ur.SearchKey;
                    continue;
                }

                if (ti.ID == "TB_S_From")
                {
                    TB tb = (TB)ti;
                    if (map.DTSearchWay == DTSearchWay.ByDate)
                    {
                        tb.Text = ur.DTFrom_Data;
                        tb.Attributes["onfocus"] = "WdatePicker();";
                    }
                    else
                        tb.Text = ur.DTFrom_Datatime;
                    continue;
                }

                if (ti.ID == "TB_S_To")
                {
                    TB tb = (TB)ti;
                    if (map.DTSearchWay == DTSearchWay.ByDate)
                    {
                        tb.Text = ur.DTTo_Data;
                        tb.Attributes["onfocus"] = "WdatePicker();";
                    }
                    else
                        tb.Text = ur.DTFrom_Datatime;
                    continue;
                }


                if (ti.ID.IndexOf("DDL_") == -1)
                    continue;

                if (cfgKey.IndexOf(ti.ID) == -1)
                    continue;

                foreach (string key in keys)
                {
                    if (key.Length < 3)
                        continue;

                    if (key.IndexOf(ti.ID) == -1)
                        continue;

                    string[] vals = key.Split('=');

                    DDL ddl = (DDL)ti;
                    bool isHave = ddl.SetSelectItem(vals[1]);
                    if (isHave == false)
                    {
                        /* Did not have to find the person you want to select */
                        try
                        {
                            Attr attr = map.GetAttrByKey(vals[0].Replace("DDL_", ""));
                            ddl.SetSelectItem(vals[1], attr);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            #endregion  Set Default 

            #region  Processing cascade relationship 
            bool IsCheckRelational = false;

            //  Get select collection categories .
            AttrSearchs bigAttrs = new AttrSearchs();
            foreach (AttrSearch attr in map.SearchAttrs)
            {
                if (attr.RelationalDtlKey == null)
                    continue;
                bigAttrs.Add(attr);
            }

            //  They traverse , Generate an event for them .
            foreach (AttrSearch attr in bigAttrs)
            {
                AttrSearch smallAttr = null;
                foreach (AttrSearch attr1 in map.SearchAttrs)
                {
                    if (attr1.Key == attr.RelationalDtlKey)
                        smallAttr = attr1;
                }

                if (smallAttr == null)
                    throw new Exception("@ Cascading submenu key that you set in the query set of attributes which does not exist .");

                Entities ens = smallAttr.HisAttr.HisFKEns;
                ens.RetrieveAll();
                Entity en = smallAttr.HisAttr.HisFKEn;

                //  Add an event .
                DDL ddl = this.GetDDLByID("DDL_" + attr.Key);
                // ddl.Attributes["onchange"] = "Redirect" + attr.Key + "(this.options.selectedIndex)";

                ddl.Attributes["onchange"] = "Redirect" + attr.Key + "()";
                DDL ddlSmil = this.GetDDLByID("DDL_" + attr.RelationalDtlKey);
                string script = "";
                //  Analyzing cascade , In accordance with the number still in accordance with the rules of the foreign key cascade cascade .
                if (en.EnMap.Attrs.Contains(attr.Key))
                {
                    /* According to the foreign key or enumeration type cascade  */
                }
                else
                {
                    /* According to the rules of cascading number .*/
                    script = "\t\n<script type='text/javascript'>";
                    script += "\t\n<!--";
                    string arrayStrs = "";
                    bool isfirst = true;
                    foreach (EntityNoName en1 in ens)
                    {
                        if (isfirst)
                        {
                            isfirst = false;
                            arrayStrs += "[\"" + en1.Name + "\",\"" + en1.No + "\"]";
                        }
                        else
                        {
                            arrayStrs += ",[\"" + en1.Name + "\",\"" + en1.No + "\"]";
                        }
                    }
                    script += "\t\n var data" + attr.Key + " = new Array(" + arrayStrs + "); ";
                    script += "\t\n Redirect" + attr.Key + "();";
                    // Data Linkage 
                    script += "\t\n function Redirect" + attr.Key + "(){";
                    // script += "\t\n alert('sss')";
                    script += "\t\n	var ddlBig" + attr.Key + " = document.getElementById(\"" + ddl.ClientID + "\");";
                    script += "\t\n	var ddlSmall" + attr.Key + " = document.getElementById(\"" + ddlSmil.ClientID + "\");";
                    script += "\t\n	var value_Big" + attr.Key + " = getSelectValue" + attr.Key + "( ddlBig" + attr.Key + " );";
                    script += "\t\n	var value_Big_length" + attr.Key + " = value_Big" + attr.Key + ".length;";
                    script += "\t\n	var index" + attr.Key + " = 0;";
                    script += "\t\n	ddlSmall" + attr.Key + ".options.length = 0;";
                    script += "\t\n	for(i=0; i<data" + attr.Key + ".length; i++){					";
                    script += "\t\n		if(data" + attr.Key + "[i][1].substr(0, value_Big_length" + attr.Key + ") == value_Big" + attr.Key + "){";
                    script += "\t\n			ddlSmall" + attr.Key + ".options[index" + attr.Key + "++] = new Option(data" + attr.Key + "[i][0],data" + attr.Key + "[i][1]);";
                    script += "\t\n		}";
                    script += "\t\n	}";
                    script += "\t\n	ddlSmall" + attr.Key + ".options[0].selected = true;";
                    script += "\t\n	}";
                    script += " // Gets the value of the specified drop-down list ";
                    script += "\t\n function getSelectValue" + attr.Key + "(oper) { ";
                    script += "\t\n	return oper.options[oper.options.selectedIndex].value;";
                    script += "\t\n	} ";
                    script += "\t\n	//-->";
                    script += "\t\n	</script> ";
                }
                //  Sign him 
                this.Page.RegisterClientScriptBlock(attr.Key, script);
            }
            #endregion
        }
        /// <summary>
        ///  Query 
        /// </summary>
        /// <param name="isShowKey"></param>
        /// <param name="sw"></param>
        /// <param name="dtSearchKey"></param>
        /// <param name="attrsOfSearch"></param>
        /// <param name="attrsOfFK"></param>
        /// <param name="attrD1"></param>
        /// <param name="page"></param>
        /// <param name="ur"></param>
        public void InitByMapV2(bool isShowKey, DTSearchWay sw, AttrsOfSearch attrsOfSearch, AttrSearchs attrsOfFK, Attrs attrD1, int page, UserRegedit ur)
        {
            int keysNum = 0;
            //  Keyword .
            if (isShowKey)
            {
                this.AddLab("Lab_Key", " Keyword :&nbsp;");
                TB tb = new TB();
                tb.ID = "TB_Key";
                tb.Columns = 13;
                this.AddTB(tb);
                keysNum++;
            }
            this.Add("&nbsp;");

            if (sw != DTSearchWay.None)
            {
                Label lab = new Label();
                lab.ID = "Lab_From";
                lab.Text = " Date from :";
                this.Add(lab);
                TB tbDT = new TB();
                tbDT.ID = "TB_S_From";
                if (sw == DTSearchWay.ByDate)
                    tbDT.ShowType = TBType.Date;
                if (sw == DTSearchWay.ByDateTime)
                    tbDT.ShowType = TBType.DateTime;
                this.Add(tbDT);

                lab = new Label();
                lab.ID = "Lab_To";
                lab.Text = "到:";
                this.Add(lab);

                tbDT = new TB();
                tbDT.ID = "TB_S_To";
                if (sw == DTSearchWay.ByDate)
                    tbDT.ShowType = TBType.Date;
                if (sw == DTSearchWay.ByDateTime)
                    tbDT.ShowType = TBType.DateTime;
                this.Add(tbDT);
            }

            //  Non-foreign key attribute .
            foreach (AttrOfSearch attr in attrsOfSearch)
            {
                if (attr.IsHidden)
                    continue;

                this.AddLab("Lab_" + attr.Key, attr.Lab);
                keysNum++;

                if (attr.SymbolEnable == true)
                {
                    DDL ddl = new DDL();
                    ddl.ID = "DDL_" + attr.Key;
                    ddl.SelfShowType = DDLShowType.Ens; //  attr.UIDDLShowType;		 
                    ddl.SelfBindKey = "BP.Sys.Operators";
                    ddl.SelfEnsRefKey = "No";
                    ddl.SelfEnsRefKeyText = "Name";
                    ddl.SelfDefaultVal = attr.DefaultSymbol;
                    ddl.SelfAddAllLocation = AddAllLocation.None;
                    ddl.SelfIsShowVal = false; /// Not to show ID 
                    //ddl.ID="DDL_"+attr.Key;
                    //ddl.SelfBind();
                    this.AddDDL(ddl);
                    this.GetDDLByKey("DDL_" + attr.Key).SelfBind();
                }

                if (attr.DefaultVal.Length >= 8)
                {
                    DateTime mydt = BP.DA.DataType.ParseSysDate2DateTime(attr.DefaultVal);

                    DDL ddl = new DDL();
                    ddl.ID = "DDL_" + attr.Key + "_Year";
                    ddl.SelfShowType = DDLShowType.Ens;
                    ddl.SelfBindKey = "BP.Pub.NDs";
                    ddl.SelfEnsRefKey = "No";
                    ddl.SelfEnsRefKeyText = "Name";
                    ddl.SelfDefaultVal = mydt.ToString("yyyy");
                    ddl.SelfAddAllLocation = AddAllLocation.None;
                    ddl.SelfIsShowVal = false; /// Not to show ID 
                    this.AddDDL(ddl);
                    ddl.SelfBind();
                    //ddl.SelfBind();

                    ddl = new DDL();
                    ddl.ID = "DDL_" + attr.Key + "_Month";
                    ddl.SelfShowType = DDLShowType.Ens;
                    ddl.SelfBindKey = "BP.Pub.YFs";
                    ddl.SelfEnsRefKey = "No";
                    ddl.SelfEnsRefKeyText = "Name";
                    ddl.SelfDefaultVal = mydt.ToString("MM");
                    ddl.SelfAddAllLocation = AddAllLocation.None;
                    ddl.SelfIsShowVal = false; /// Not to show ID 
                    //	ddl.SelfBind();
                    this.AddDDL(ddl);
                    ddl.SelfBind();

                    ddl = new DDL();
                    ddl.ID = "DDL_" + attr.Key + "_Day";
                    ddl.SelfShowType = DDLShowType.Ens;
                    ddl.SelfBindKey = "BP.Pub.Days";
                    ddl.SelfEnsRefKey = "No";
                    ddl.SelfEnsRefKeyText = "Name";
                    ddl.SelfDefaultVal = mydt.ToString("dd");
                    ddl.SelfAddAllLocation = AddAllLocation.None;
                    ddl.SelfIsShowVal = false; /// Not to show ID 
                    //ddl.SelfBind();
                    this.AddDDL(ddl);
                    this.GetDDLByKey(ddl.ID).SelfBind();

                }
                else
                {
                    TB tb = new TB();
                    tb.ID = "TB_" + attr.Key;
                    tb.Text = attr.DefaultVal;
                    tb.Columns = attr.TBWidth;
                    this.AddTB(tb);
                }
            }

            string ensName = this.Page.Request.QueryString["EnsName"];
            string cfgVal = "";
            cfgVal = ur.Vals;

            //  Foreign key attribute query .			 
            bool isfirst = true;
            foreach (AttrSearch attr1 in attrsOfFK)
            {
                Attr attr = attr1.HisAttr;

                if (attr.MyFieldType == FieldType.RefText)
                    continue;

                DDL ddl = new DDL();
                ddl.ID = "DDL_" + attr.Key;
                this.AddDDL(ddl);
                keysNum++;
                //if (keysNum == 3 || keysNum == 6 || keysNum == 9)
                //    this.AddBR("b_" + keysNum);
                if (attr.MyFieldType == FieldType.Enum)
                {
                    this.GetDDLByKey("DDL_" + attr.Key).BindSysEnum(attr.UIBindKey, false, AddAllLocation.TopAndEndWithMVal);
                    this.GetDDLByKey("DDL_" + attr.Key).Items[0].Text = ">>" + attr.Desc;

                    this.GetDDLByKey("DDL_" + attr.Key).Attributes["onclick"] = "DDL_mvals_OnChange(this,'" + ensName + "','" + attr.Key + "')";
                    // this.GetDDLByKey("DDL_" + attr.Key).Attributes["onchange"] = "DDL_mvals_OnChange(this,'" + ur.MyPK + "','" + attr.Key + "')";
                    // ddl.Attributes["onchange"] = "DDL_mvals_OnChange(this,'" + ur.MyPK + "','" + attr.Key + "')";
                }
                else
                {
                    ListItem liMvals = new ListItem("* Number of combinations ..", "mvals");
                    liMvals.Attributes.CssStyle.Add("style", "color:green");
                    liMvals.Attributes.Add("color", "green");
                    liMvals.Attributes.Add("style", "color:green");

                    // liMvals.Attributes.Add("onclick", "alert('sss')");

                    switch (attr.UIBindKey)
                    {
                        case "BP.Port.Depts":
                            ddl.Items.Clear();
                            BP.Port.Depts depts = new BP.Port.Depts();
                            depts.RetrieveAll();
                            foreach (BP.Port.Dept dept in depts)
                            {
                                string space = "";
                                //   space = space.PadLeft(dept.Grade - 1, '-');
                                ListItem li = new ListItem(space + dept.Name, dept.No);
                                this.GetDDLByKey("DDL_" + attr.Key).Items.Add(li);
                            }
                            if (depts.Count > SystemConfig.MaxDDLNum)
                                this.AddLab("lD", "<a href=\"javascript:onDDLSelectedMore('DDL_" + attr.Key + "', '" + this.EnsName + "', 'BP.Port.Depts', 'No','Name')\" >...</a>");

                            if (ddl.Items.Count >= 2)
                                ddl.Items.Add(liMvals);

                            ddl.Attributes["onchange"] = "DDL_mvals_OnChange(this,'" + ensName + "','" + attr.Key + "')";
                            break;
                        //case "BP.Port.Units":
                        //    ddl.Items.Clear();
                        //    BP.Port.Units units = new BP.Port.Units();
                        //    units.RetrieveAll();
                        //    foreach (BP.Port.Unit unit in units)
                        //    {
                        //        string space = "";
                        //        space = space.PadLeft(unit.No.Length / 2 - 1, '-');
                        //        ListItem li = new ListItem(space + unit.Name, unit.No);
                        //        this.GetDDLByKey("DDL_" + attr.Key).Items.Add(li);
                        //    }
                        //    if (units.Count > SystemConfig.MaxDDLNum)
                        //        this.AddLab("lD", "<a href=\"javascript:onDDLSelectedMore('DDL_" + attr.Key + "', '" + this.EnsName + "', 'BP.Port.Units', 'No','Name')\" >...</a>");

                        //    if (ddl.Items.Count >= 2)
                        //        ddl.Items.Add(liMvals);

                        //    ddl.Attributes["onchange"] = "DDL_mvals_OnChange(this,'" + ensName + "','" + attr.Key + "')";
                        // break;
                        default:
                            ddl.Items.Clear();
                            if (attr.MyDataType == DataType.AppBoolean)
                            {
                                ddl.Items.Add(new ListItem(">>" + attr.Desc, "all"));
                                ddl.Items.Add(new ListItem("是", "1"));
                                ddl.Items.Add(new ListItem("否", "0"));
                                break;
                            }
                            Entities ens = attr.HisFKEns;
                            ens.RetrieveAll();
                            ddl.Items.Add(new ListItem(">>" + attr.Desc, "all"));
                            foreach (Entity en in ens)
                                ddl.Items.Add(new ListItem(en.GetValStrByKey("Name"), en.GetValStrByKey("No")));

                            if (ddl.Items.Count >= 2)
                                ddl.Items.Add(liMvals);

                            ddl.Attributes["onchange"] = "DDL_mvals_OnChange(this,'" + ensName + "','" + attr.Key + "')";
                            break;
                    }
                }
                if (isfirst)
                    isfirst = false;
            }
            if (_AddSearchBtn)
                this.AddBtn("Btn_Search", " Inquiry (L)");
        }
    }

}