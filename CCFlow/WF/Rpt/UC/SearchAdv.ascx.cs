using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
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
using BP.WF.Rpt;
using BP;
using BP.Web.Comm;

namespace CCFlow.WF.Rpt
{
    public partial class SearchAdv : BP.Web.UC.UCBase3
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
                {
                    return "ND1MyRpt";// "ND68MyRpt";
                }
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

        public MapRpt currMapRpt = null;

        /// <summary>
        ///  Current query 
        /// </summary>
        public string CurrentQuery { get; set; }

        /// <summary>
        /// 非Model Query URL
        /// </summary>
        public string NormalSearchUrl { get; set; }

        /// <summary>
        ///  Display of the current query PK
        /// </summary>
        public string CurrentUR_PK
        {
            get { return Request.QueryString["CurrentUR_PK"]; }
        }

        /// <summary>
        ///  Whether the identity of the current in the query interface 
        /// </summary>
        public bool IsSearch
        {
            get
            {
                var isSearch = false;
                bool.TryParse(Request.QueryString["IsSearch"], out isSearch);
                return isSearch;
            }
        }

        /// <summary>
        ///  Whether shrink results interface 
        /// </summary>
        public bool ResultCollapsed = true;

        #endregion  Property .

        protected void Page_Load(object sender, EventArgs e)
        {
            #region  Processing queries Permissions ,  Do not modify here ,以Search.ascx Prevail .

            currMapRpt = new MapRpt(this.RptNo, this.FK_Flow);
            Entity en = this.HisEns.GetNewEntity;
            Flow fl = new Flow(this.currMapRpt.FK_Flow);

            //DDL ddl = new DDL();
            //ddl.ID = "GoTo";
            //ddl.Items.Add(new ListItem(" Inquiry ", "Search"));
            //ddl.Items.Add(new ListItem(" Advanced Search ", "SearchAdv"));
            //ddl.Items.Add(new ListItem(" Subgroup analysis ", "Group"));
            //ddl.Items.Add(new ListItem(" Cross report ", "D3"));
            //ddl.Items.Add(new ListItem(" Comparative analysis ", "Contrast"));
            //ddl.SetSelectItem(this.PageID);

            #endregion  Processing queries Permissions 

            this.NormalSearchUrl = string.Format("SearchAdv.aspx?RptNo={0}&FK_Flow={1}", this.RptNo, this.FK_Flow);

            # region //  Binding query templates .
            UserRegedits urs = new UserRegedits();
            urs.Retrieve(UserRegeditAttr.FK_Emp, WebUser.No, UserRegeditAttr.CfgKey, this.RptNo + "_SearchAdv");

            //1. Get a list of all my query 
            //NumKey Save the name of the query 
            UserRegedit currUR = null;  // Display of the current query 
            var idx = 1;
            foreach (UserRegedit ur in urs)
            {
                if (ur.MyPK.Contains("SearchAdvModel"))
                {
                    Pub1.AddLi(
                        string.Format(
                            "<div><a href='SearchAdv.aspx?RptNo={0}&FK_Flow={1}&CurrentUR_PK={2}'><span class='nav' style='{5}'>{3}. {4}</span></a></div>",
                            this.RptNo, this.FK_Flow, ur.MyPK, idx++, ur.NumKey, ur.MyPK == this.CurrentUR_PK ? "font-weight:bold" : ""));

                    if (ur.MyPK == this.CurrentUR_PK)
                        currUR = ur;

                    continue;
                }

                // If the current saved query is not selected , Use _SearchAdv Saved Queries display , This query is the last time the user had not saved queries 
                if (currUR != null) continue;

                currUR = ur;
                ViewState["newur"] = ur.MyPK;
            }

            // If this is the first time you open the query , Then save a _SearchAdv Record 
            if (currUR == null)
            {
                currUR = new UserRegedit(WebUser.No, this.RptNo + "_SearchAdv");
                ViewState["newur"] = currUR.MyPK;
            }
            #endregion

            this.CurrentQuery = string.IsNullOrWhiteSpace(currUR.NumKey)
                                    ? " Query conditions "
                                    : string.Format("[ {0} ] -  Query conditions ", currUR.NumKey);

            #region // Binding query conditions 

            var paras = currUR.Paras;

            MapAttrs attrs = currMapRpt.MapAttrs;
            string[] conds = paras.Split("^".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var condIdx = 0;    // The serial number identifies the serial number of the query , So when you modify a query condition , Pass over the serial number , Conditions which can be identified , The original query with field conditions to identify inaccurate , Because a field can appear in multiple query conditions 

            foreach (string cond in conds)
            {
                if (string.IsNullOrEmpty(cond))
                    continue;

                // Parameters .
                AtPara ap = new AtPara(cond);
                string attrKey = ap.GetValStrByKey("AttrKey");

                // Acquire property .
                MapAttr attr = attrs.Filter(MapAttrAttr.KeyOfEn, attrKey) as MapAttr;
                if (attr == null)
                    continue;

                Pub2.AddTR();
                //1. Increase left parenthesis .
                var ddl = new DDL();
                ddl.ID = "DDL_LeftBreak_" + condIdx;
                ddl.Style.Add("width", "40px");
                ddl.Items.Add(new ListItem("", ""));
                ddl.Items.Add(new ListItem("(", "("));

                if (!string.IsNullOrWhiteSpace(ap.GetValStrByKey("LeftBreak")))
                    ddl.SelectedIndex = 1;

                Pub2.AddTD("style='width:45px'", ddl);

                //2. Increase the field ,  Here can not be modified , Display text box 
                var tb = new TB();
                tb.ID = "TB_Attr_" + condIdx;
                tb.Text = attr.Name;
                tb.ReadOnly = true;
                Pub2.AddTD("style='width:100px'", tb);

                //3. Determine the field type , Can be used to determine the operator . Added to DDL中, And check the conditions of the current query operators 
                ddl = new DDL();
                ddl.ID = "DDL_Exp_" + condIdx;

                switch (attr.LGType)
                {
                    case FieldTypeS.Normal: // Ordinary type , Contain 【 Text , Digital , Date , Whether 】
                        switch (attr.MyDataType)
                        {
                            case DataType.AppString:    // Text , Operators are available for : Equal , Does not equal , Contain , Does not contain 
                                ddl.Items.Add(new ListItem(" Equal ", "="));
                                ddl.Items.Add(new ListItem(" Does not equal ", "!="));
                                ddl.Items.Add(new ListItem(" Contain ", "LIKE"));
                                ddl.Items.Add(new ListItem(" Does not contain ", "NOT LIKE"));
                                break;
                            case DataType.AppInt:
                            case DataType.AppFloat:
                            case DataType.AppDouble:
                            case DataType.AppMoney:
                            case DataType.AppRate:      // Digital , Operators are available for : Equal , Does not equal , Greater than , Greater than or equal , Less than , Less than or equal , Contain 
                                ddl.Items.Add(new ListItem(" Equal ", "="));
                                ddl.Items.Add(new ListItem(" Does not equal ", "!="));
                                ddl.Items.Add(new ListItem(" Greater than ", ">"));
                                ddl.Items.Add(new ListItem(" Greater than or equal ", ">="));
                                ddl.Items.Add(new ListItem(" Less than ", "<"));
                                ddl.Items.Add(new ListItem(" Less than or equal ", "<="));
                                ddl.Items.Add(new ListItem(" Contain ", "IN"));
                                break;
                            case DataType.AppDate:
                            case DataType.AppDateTime:  // Date , Operators are available for : Equal , Does not equal , Greater than , Greater than or equal , Less than , Less than or equal 
                                ddl.Items.Add(new ListItem(" Equal ", "="));
                                ddl.Items.Add(new ListItem(" Does not equal ", "!="));
                                ddl.Items.Add(new ListItem(" Greater than ", ">"));
                                ddl.Items.Add(new ListItem(" Greater than or equal ", ">="));
                                ddl.Items.Add(new ListItem(" Less than ", "<"));
                                ddl.Items.Add(new ListItem(" Less than or equal ", "<="));
                                break;
                            case DataType.AppBoolean:   // Boolean , Operators are available for : Equal , Does not equal 
                                ddl.Items.Add(new ListItem(" Equal ", "="));
                                ddl.Items.Add(new ListItem(" Does not equal ", "!="));
                                break;
                            default:
                                break;
                        }
                        break;
                    case FieldTypeS.Enum: // Enumeration . 
                        ddl.Items.Add(new ListItem(" Equal ", "="));
                        ddl.Items.Add(new ListItem(" Does not equal ", "!="));
                        //ddl.Items.Add(new ListItem(" Contain ", "IN"));
                        //ddl.Items.Add(new ListItem(" Does not contain ", "NOT IN")); // Because behind using DDL, So now is not supported [ Contain ]与[ Does not contain ] Operation 
                        break;
                    case FieldTypeS.FK: //  Foreign key .
                        ddl.Items.Add(new ListItem(" Equal ", "="));
                        ddl.Items.Add(new ListItem(" Does not equal ", "!="));
                        //ddl.Items.Add(new ListItem(" Contain ", "IN"));
                        //ddl.Items.Add(new ListItem(" Does not contain ", "NOT IN"));
                        break;
                    default:
                        break;
                }

                var exp = ap.GetValStrByKey("Exp");

                if (!string.IsNullOrWhiteSpace(exp) && exp.Length > 0)
                    ddl.SelectedValue = exp;

                Pub2.AddTD("style='width:80px'", ddl);

                //4. Increase the value of the query 
                switch (attr.LGType)
                {
                    case FieldTypeS.Normal: // Ordinary type , Contain 【 Text , Digital , Date , Whether 】
                        switch (attr.MyDataType)
                        {
                            case DataType.AppString:
                            case DataType.AppInt:
                            case DataType.AppFloat:
                            case DataType.AppDouble:
                            case DataType.AppMoney:
                            case DataType.AppRate:      // Text / Figures with text boxes 
                                tb = new TB();
                                tb.ID = "TB_Val_" + condIdx;
                                tb.Text = ap.GetValStrByKey("Val");
                                Pub2.AddTD(tb);
                                break;
                            case DataType.AppDate:
                                tb = new TB();
                                tb.ID = "TB_Val_" + condIdx;
                                tb.ShowType = TBType.Date;
                                tb.Attributes["onfocus"] = "WdatePicker();";
                                tb.Text = ap.GetValStrByKey("Val");
                                Pub2.AddTD(tb);
                                break;
                            case DataType.AppDateTime:  // Date , Operators are available for : Equal , Does not equal , Greater than , Greater than or equal , Less than , Less than or equal 
                                tb = new TB();
                                tb.ID = "TB_Val_" + condIdx;
                                tb.ShowType = TBType.DateTime;
                                tb.Attributes["onfocus"] = "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'});";
                                tb.Text = ap.GetValStrByKey("Val");
                                Pub2.AddTD(tb);
                                break;
                            case DataType.AppBoolean:   // Boolean , Operators are available for : Equal , Does not equal 
                                ddl = new DDL();
                                ddl.ID = "DDL_Val_" + condIdx;
                                ddl.Items.Add(new ListItem("", ""));
                                ddl.Items.Add(new ListItem("是", "1"));
                                ddl.Items.Add(new ListItem("否", "0"));
                                ddl.SetSelectItem(ap.GetValStrByKey("Val"));
                                Pub2.AddTD(ddl);
                                break;
                            default:
                                break;
                        }
                        break;
                    case FieldTypeS.Enum: // Enumeration . 
                        ddl = new DDL();
                        ddl.ID = "DDL_Val_" + condIdx;
                        ddl.SelfBindSysEnum(attr.UIBindKey);
                        ddl.SetSelectItem(ap.GetValIntByKey("Val"));
                        Pub2.AddTD(ddl);
                        break;
                    case FieldTypeS.FK: //  Foreign key .
                        ddl = new DDL();
                        ddl.ID = "DDL_Val_" + condIdx;
                        ddl.BindEntities(attr.HisAttr.HisFKEns, attr.UIRefKey, attr.UIRefKeyText);
                        ddl.SetSelectItem(ap.GetValStrByKey("Val"));
                        Pub2.AddTD(ddl);
                        break;
                    default:
                        break;
                }

                //5. Increase right parenthesis 
                ddl = new DDL();
                ddl.ID = "DDL_RightBreak_" + condIdx;
                ddl.Style.Add("width", "40px");
                ddl.Items.Add(new ListItem("", ""));
                ddl.Items.Add(new ListItem(")", ")"));

                if (!string.IsNullOrWhiteSpace(ap.GetValStrByKey("RightBreak")))
                    ddl.SelectedIndex = 1;

                Pub2.AddTD("style='width:45px'", ddl);

                //6. Increase logical operators between conditions 
                ddl = new DDL();
                ddl.ID = "DDL_Union_" + condIdx;
                ddl.Items.Add(new ListItem(" And ", "And"));
                ddl.Items.Add(new ListItem(" Or ", "Or"));
                ddl.SetSelectItem(ap.GetValStrByKey("Union"));
                Pub2.AddTD("style='width:60px'", ddl);

                Pub2.AddTDBegin();

                Pub2.Add("<a href='javascript:void(0)' class='easyui-linkbutton' onclick=\"selectAttr('" + condIdx + "','" + attr.KeyOfEn + "')\" data-options=\"iconCls:'icon-edit'\" title=' Modification '></a>" + Environment.NewLine);

                var btn = new LinkBtn(false, "Btn_Delete_" + condIdx, "");
                btn.SetDataOption("iconCls", "'icon-delete'");
                btn.Attributes.Add("onclick", "return confirm(' Are you sure you want to delete the query criteria ?');");
                btn.Attributes.Add("title", " Delete ");
                btn.Click += new EventHandler(btn_Delete_Click);
                Pub2.Add(btn);
                Pub2.AddSpace(1);

                // The last condition ,有[ Increase ] Push button , Click the pop-up dialog box, select the query field 
                if (condIdx == conds.Length - 1)
                {
                    Pub2.Add(
                    "<a href='javascript:void(0)' class='easyui-linkbutton' onclick=\"selectAttr('','')\" data-options=\"iconCls:'icon-add'\" title=' Increase '></a>" + Environment.NewLine);
                }

                Pub2.AddTDEnd();
                Pub2.AddTREnd();
                condIdx++;
            }

            if (conds.Length == 0 || condIdx != conds.Length)
            {
                // Add a blank condition 
                Pub2.AddTR();

                var ddl1 = new DDL();
                ddl1.ID = "DDL_LeftBreak";
                ddl1.Style.Add("width", "55px");
                ddl1.Items.Add(new ListItem("", ""));
                ddl1.Items.Add(new ListItem("(", " ( "));
                Pub2.AddTD("style='width:60px'", ddl1);

                var tb1 = new TB();
                tb1.ID = "TB_Attr";
                tb1.Text = string.Empty;
                tb1.ReadOnly = true;
                Pub2.AddTD("style='width:140px'", tb1);
                Pub2.AddTD("style='width:80px'", "");
                Pub2.AddTD();
                Pub2.AddTD("style='width:60px'", "");
                Pub2.AddTD("style='width:60px'", "");
                Pub2.AddTDBegin("style='width:100px'");

                Pub2.Add(
                    "<a href='javascript:void(0)' class='easyui-linkbutton' onclick=\"selectAttr('','')\" data-options=\"iconCls:'icon-add'\"> Increase </a>&nbsp;");

                Pub2.AddTDEnd();
                Pub2.AddTREnd();
            }

            if (currUR.MyPK.Contains("SearchAdvModel"))
            {
                btnSaveSearchDirect.Visible = true;
                btnDeleteModel.Visible = true;
            }
            else if (conds.Length > 0)
            {
                btnSaveSearch.Visible = true;
                btnDeleteModel.Visible = false;
            }
            #endregion

            // Select the fields on the form 
            LB_Attrs.Items.Clear();

            foreach (MapAttr attr in attrs)
            {
                LB_Attrs.Items.Add(new ListItem(attr.Name, attr.KeyOfEn));
            }

            if (IsSearch)
                this.SetDGData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var currUR = GetCurrentUserRegedit();
                currUR.Paras = GenerateParas(currUR);
                currUR.GenerSQL = GenerateSQLByQueryObject(currUR).SQLWithOutPara;
                currUR.Save();
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message, true, true);
                return;
            }

            SetDGData(1);
        }

        /// <summary>
        ///  Save and generate SQL Query 
        /// </summary>
        private void SaveAndGenerateSQL(UserRegedit currUR)
        {
            var paras = currUR.Paras.Split("^".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            currMapRpt = new MapRpt(this.RptNo, this.FK_Flow);
            MapAttrs attrs = currMapRpt.MapAttrs;

            var newParas = string.Empty;
            var newSQL = string.Format("SELECT * FROM {0} WHERE ", HisEns.GetNewEntity.EnMap.PhysicsTable);
            AtPara ap = null;
            Control ctrl = null;
            MapAttr attr = null;

            for (var i = 0; i < paras.Length; i++)
            {
                // Organization paras
                ap = new AtPara(paras[i]);
                ap.SetVal("LeftBreak", (Pub2.FindControl("DDL_LeftBreak_" + i) as DDL).SelectedItemStringVal);
                ap.SetVal("Exp", (Pub2.FindControl("DDL_Exp_" + i) as DDL).SelectedItemStringVal);
                ap.SetVal("RightBreak", (Pub2.FindControl("DDL_RightBreak_" + i) as DDL).SelectedItemStringVal);
                ap.SetVal("Union", (Pub2.FindControl("DDL_Union_" + i) as DDL).SelectedItemStringVal);

                ctrl = Pub2.FindControl("TB_Val_" + i);

                if (ctrl != null)
                {
                    ap.SetVal("Val", (ctrl as TB).Text);
                }
                else
                {
                    ctrl = Pub2.FindControl("DDL_Val_" + i);

                    if (ctrl == null)
                        throw new Exception(" The query is not found ");

                    ap.SetVal("Val", (ctrl as DDL).SelectedItemStringVal);
                }

                newParas += ap.GenerAtParaStrs() + "^";

                // Organization SQL
                if (string.IsNullOrWhiteSpace(ap.GetValStrByKey("AttrKey")) || (string.IsNullOrWhiteSpace(ap.GetValStrByKey("Exp")) && string.IsNullOrWhiteSpace(ap.GetValStrByKey("Val")))) continue;  // To remove the last increase in invalid conditions 

                attr = attrs.Filter(MapAttrAttr.KeyOfEn, ap.GetValStrByKey("AttrKey")) as MapAttr;
                newSQL += ap.GetValStrByKey("LeftBreak");
                newSQL += ap.GetValStrByKey("AttrKey") + " ";
                newSQL += ap.GetValStrByKey("Exp") + " ";

                switch (attr.LGType)
                {
                    case FieldTypeS.Normal: // Ordinary type , Contain 【 Text , Digital , Date , Whether 】
                        switch (attr.MyDataType)
                        {
                            case DataType.AppString:    // Text , Operators are available for : Equal , Does not equal , Contain , Does not contain 
                                switch (ap.GetValStrByKey("Exp"))
                                {
                                    case "LIKE":
                                    case "NOT LIKE":
                                        newSQL += string.Format("'%{0}%' ", ap.GetValStrByKey("Val"));
                                        break;
                                    default:
                                        newSQL += string.Format("'{0}' ", ap.GetValStrByKey("Val"));
                                        break;
                                }

                                break;
                            case DataType.AppInt:
                            case DataType.AppFloat:
                            case DataType.AppDouble:
                            case DataType.AppMoney:
                            case DataType.AppRate:      // Digital , Operators are available for : Equal , Does not equal , Greater than , Greater than or equal , Less than , Less than or equal , Contain 
                                switch (ap.GetValStrByKey("Exp"))
                                {
                                    case "IN":
                                        newSQL += "(" + ap.GetValStrByKey("Val").Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Aggregate(string.Empty, (curr, next) => curr + next + ",").TrimEnd(',') + ") ";
                                        break;
                                    default:
                                        newSQL += ap.GetValStrByKey("Val") + " ";
                                        break;
                                }

                                break;
                            case DataType.AppDate:
                            case DataType.AppDateTime:  // Date , Operators are available for : Equal , Does not equal , Greater than , Greater than or equal , Less than , Less than or equal 
                                newSQL += string.Format("'{0}' ", ap.GetValStrByKey("Val"));
                                break;
                            case DataType.AppBoolean:   // Boolean , Operators are available for : Equal , Does not equal 
                                newSQL += ap.GetValStrByKey("Val") + " ";
                                break;
                            default:
                                break;
                        }
                        break;
                    case FieldTypeS.Enum: // Enumeration . 
                        newSQL += ap.GetValStrByKey("Val") + " ";
                        break;
                    case FieldTypeS.FK: //  Foreign key .
                        switch (attr.MyDataType)
                        {
                            case DataType.AppString:    // Text 
                                newSQL += string.Format("'{0}' ", ap.GetValStrByKey("Val"));
                                break;
                            case DataType.AppInt:
                            case DataType.AppFloat:
                            case DataType.AppDouble:
                            case DataType.AppMoney:
                            case DataType.AppRate:      // Digital 
                                newSQL += ap.GetValStrByKey("Val") + " ";
                                break;
                        }
                        break;
                    default:
                        break;
                }

                newSQL += ap.GetValStrByKey("RightBreak") + " ";
                newSQL += ap.GetValStrByKey("Union") + " ";
            }

            currUR.GenerSQL = newSQL.TrimEnd("And ".ToCharArray()).TrimEnd("Or ".ToCharArray());
            currUR.Save();
        }

        /// <summary>
        ///  Generate Paras
        /// </summary>
        private string GenerateParas(UserRegedit currUR)
        {
            var paras = currUR.Paras.Split("^".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var newParas = string.Empty;
            AtPara ap = null;
            Control ctrl = null;

            for (var i = 0; i < paras.Length; i++)
            {
                // Organization paras
                ap = new AtPara(paras[i]);
                ap.SetVal("LeftBreak", (Pub2.FindControl("DDL_LeftBreak_" + i) as DDL).SelectedItemStringVal);
                ap.SetVal("Exp", (Pub2.FindControl("DDL_Exp_" + i) as DDL).SelectedItemStringVal);
                ap.SetVal("RightBreak", (Pub2.FindControl("DDL_RightBreak_" + i) as DDL).SelectedItemStringVal);
                ap.SetVal("Union", (Pub2.FindControl("DDL_Union_" + i) as DDL).SelectedItemStringVal);

                ctrl = Pub2.FindControl("TB_Val_" + i);

                if (ctrl != null)
                {
                    ap.SetVal("Val", (ctrl as TB).Text);
                }
                else
                {
                    ctrl = Pub2.FindControl("DDL_Val_" + i);

                    if (ctrl == null)
                        throw new Exception(" The query is not found ");

                    ap.SetVal("Val", (ctrl as DDL).SelectedItemStringVal);
                }

                newParas += ap.GenerAtParaStrs() + "^";
            }

            return newParas;
        }

        /// <summary>
        ///  Generate SQL
        /// </summary>
        /// <returns></returns>
        private string GenerateSQL(UserRegedit currUR)
        {
            var paras = currUR.Paras.Split("^".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            currMapRpt = new MapRpt(this.RptNo, this.FK_Flow);
            MapAttrs attrs = currMapRpt.MapAttrs;

            var newSQL = string.Format("SELECT * FROM {0} WHERE ", HisEns.GetNewEntity.EnMap.PhysicsTable);
            AtPara ap = null;
            MapAttr attr = null;

            for (var i = 0; i < paras.Length; i++)
            {
                ap = new AtPara(paras[i]);

                if (string.IsNullOrWhiteSpace(ap.GetValStrByKey("AttrKey")) || (string.IsNullOrWhiteSpace(ap.GetValStrByKey("Exp")) && string.IsNullOrWhiteSpace(ap.GetValStrByKey("Val")))) continue;  // To remove the last increase in invalid conditions 

                // Organization SQL
                attr = attrs.Filter(MapAttrAttr.KeyOfEn, ap.GetValStrByKey("AttrKey")) as MapAttr;
                newSQL += ap.GetValStrByKey("LeftBreak");
                newSQL += ap.GetValStrByKey("AttrKey") + " ";
                newSQL += ap.GetValStrByKey("Exp") + " ";

                switch (attr.LGType)
                {
                    case FieldTypeS.Normal: // Ordinary type , Contain 【 Text , Digital , Date , Whether 】
                        switch (attr.MyDataType)
                        {
                            case DataType.AppString:    // Text , Operators are available for : Equal , Does not equal , Contain , Does not contain 
                                switch (ap.GetValStrByKey("Exp"))
                                {
                                    case "LIKE":
                                    case "NOT LIKE":
                                        newSQL += string.Format("'%{0}%' ", ap.GetValStrByKey("Val"));
                                        break;
                                    default:
                                        newSQL += string.Format("'{0}' ", ap.GetValStrByKey("Val"));
                                        break;
                                }

                                break;
                            case DataType.AppInt:
                            case DataType.AppFloat:
                            case DataType.AppDouble:
                            case DataType.AppMoney:
                            case DataType.AppRate:      // Digital , Operators are available for : Equal , Does not equal , Greater than , Greater than or equal , Less than , Less than or equal , Contain 
                                switch (ap.GetValStrByKey("Exp"))
                                {
                                    case "IN":
                                        newSQL += "(" + ap.GetValStrByKey("Val").Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Aggregate(string.Empty, (curr, next) => curr + next + ",").TrimEnd(',') + ") ";
                                        break;
                                    default:
                                        newSQL += ap.GetValStrByKey("Val") + " ";
                                        break;
                                }

                                break;
                            case DataType.AppDate:
                            case DataType.AppDateTime:  // Date , Operators are available for : Equal , Does not equal , Greater than , Greater than or equal , Less than , Less than or equal 
                                newSQL += string.Format("'{0}' ", ap.GetValStrByKey("Val"));
                                break;
                            case DataType.AppBoolean:   // Boolean , Operators are available for : Equal , Does not equal 
                                newSQL += ap.GetValStrByKey("Val") + " ";
                                break;
                            default:
                                break;
                        }
                        break;
                    case FieldTypeS.Enum: // Enumeration . 
                        newSQL += ap.GetValStrByKey("Val") + " ";
                        break;
                    case FieldTypeS.FK: //  Foreign key .
                        switch (attr.MyDataType)
                        {
                            case DataType.AppString:    // Text 
                                newSQL += string.Format("'{0}' ", ap.GetValStrByKey("Val"));
                                break;
                            case DataType.AppInt:
                            case DataType.AppFloat:
                            case DataType.AppDouble:
                            case DataType.AppMoney:
                            case DataType.AppRate:      // Digital 
                                newSQL += ap.GetValStrByKey("Val") + " ";
                                break;
                        }
                        break;
                    default:
                        break;
                }

                newSQL += ap.GetValStrByKey("RightBreak") + " ";
                newSQL += ap.GetValStrByKey("Union") + " ";
            }

            return newSQL.TrimEnd("And ".ToCharArray()).TrimEnd("Or ".ToCharArray());
        }

        /// <summary>
        ///  Generate SQL
        /// </summary>
        /// <returns></returns>
        private QueryObject GenerateSQLByQueryObject(UserRegedit currUR)
        {
            Entities ens = this.HisEns;
            QueryObject qo = new QueryObject(ens);

            var paras = currUR.Paras.Split("^".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            currMapRpt = new MapRpt(this.RptNo, this.FK_Flow);
            MapAttrs attrs = currMapRpt.MapAttrs;

            AtPara ap = null;
            MapAttr attr = null;
            var ps = new List<AtPara>();
            var breaks = new List<bool>();  // Identifies the number of columns in parentheses ,true Left parenthesis ,false For the right parenthesis 

            // Removing ineffective conditions 
            for (var i = 0; i < paras.Length; i++)
            {
                ap = new AtPara(paras[i]);

                if (string.IsNullOrWhiteSpace(ap.GetValStrByKey("AttrKey")) || (string.IsNullOrWhiteSpace(ap.GetValStrByKey("Exp")) && string.IsNullOrWhiteSpace(ap.GetValStrByKey("Val")))) continue;  // To remove the last increase in invalid conditions 
                if (ap.GetValStrByKey("LeftBreak") == "(")
                    breaks.Add(true);

                if (ap.GetValStrByKey("RightBreak") == ")")
                    breaks.Add(false);

                ps.Add(ap);
            }

            // Check the accuracy of the preliminary test conditions , The main testing whether paired brackets 
            var errorMsg = " The query error , Unpaired parentheses , Please check !";

            if (breaks.Count % 2 != 0)
                throw new Exception(errorMsg);

            while (breaks.Count > 0)
            {
                var isDo = false;   // It has been found to identify a pair of brackets 

                for (var i = 0; i < breaks.Count - 1; i++)
                {
                    if (breaks[i] && !breaks[i + 1])
                    {
                        breaks.RemoveRange(i, 2);
                        isDo = true;
                        break;
                    }
                }

                if (!isDo)
                    throw new Exception(errorMsg);
            }

            var dictParas = new Dictionary<string, int>();  // The parameters used in the query / The number of occurrences , In order to make the parameter name does not repeat 
            var paramName = string.Empty;

            for (var i = 0; i < ps.Count; i++)
            {
                // Organization SQL
                attr = attrs.Filter(MapAttrAttr.KeyOfEn, ps[i].GetValStrByKey("AttrKey")) as MapAttr;
                if (ps[i].GetValStrByKey("LeftBreak") == "(")
                    qo.addLeftBracket();

                paramName = attr.KeyOfEn;

                if (dictParas.ContainsKey(paramName))
                {
                    dictParas[paramName]++;
                    paramName += dictParas[paramName];
                }
                else
                {
                    dictParas.Add(attr.KeyOfEn, 1);
                    paramName += 1;
                }

                switch (attr.LGType)
                {
                    case FieldTypeS.Normal: // Ordinary type , Contain 【 Text , Digital , Date , Whether 】
                        switch (attr.MyDataType)
                        {
                            case DataType.AppString:    // Text , Operators are available for : Equal , Does not equal , Contain , Does not contain 
                                switch (ps[i].GetValStrByKey("Exp"))
                                {
                                    case "LIKE":
                                    case "NOT LIKE":
                                        qo.AddWhere(attr.KeyOfEn, ps[i].GetValStrByKey("Exp"), string.Format("%{0}% ", ps[i].GetValStrByKey("Val")), paramName);
                                        break;
                                    default:
                                        qo.AddWhere(attr.KeyOfEn, ps[i].GetValStrByKey("Exp"), ps[i].GetValStrByKey("Val"), paramName);
                                        break;
                                }

                                break;
                            case DataType.AppInt:
                            case DataType.AppFloat:
                            case DataType.AppDouble:
                            case DataType.AppMoney:
                            case DataType.AppRate:      // Digital , Operators are available for : Equal , Does not equal , Greater than , Greater than or equal , Less than , Less than or equal , Contain 
                                switch (ps[i].GetValStrByKey("Exp"))
                                {
                                    case "IN":
                                        qo.AddWhere(attr.KeyOfEn, "IN",
                                                      ps[i].GetValStrByKey("Val").Split(" ".ToCharArray(),
                                                                                     StringSplitOptions.
                                                                                         RemoveEmptyEntries).Aggregate(
                                                                                             string.Empty,
                                                                                             (curr, next) =>
                                                                                             curr + next + ",").TrimEnd(
                                                                                                 ','), paramName);
                                        break;
                                    default:
                                        qo.AddWhere(attr.KeyOfEn, ps[i].GetValStrByKey("Exp"), ps[i].GetValStrByKey("Val"), paramName);
                                        break;
                                }

                                break;
                            case DataType.AppDate:
                            case DataType.AppDateTime:  // Date , Operators are available for : Equal , Does not equal , Greater than , Greater than or equal , Less than , Less than or equal 
                                qo.AddWhere(attr.KeyOfEn, ps[i].GetValStrByKey("Exp"), ps[i].GetValStrByKey("Val"), paramName);
                                break;
                            case DataType.AppBoolean:   // Boolean , Operators are available for : Equal , Does not equal 
                                qo.AddWhere(attr.KeyOfEn, ps[i].GetValStrByKey("Exp"), ps[i].GetValStrByKey("Val"), paramName);
                                break;
                            default:
                                break;
                        }
                        break;
                    case FieldTypeS.Enum: // Enumeration . 
                        qo.AddWhere(attr.KeyOfEn, ps[i].GetValStrByKey("Exp"), ps[i].GetValStrByKey("Val"), paramName);
                        break;
                    case FieldTypeS.FK: //  Foreign key .
                        switch (attr.MyDataType)
                        {
                            case DataType.AppString:    // Text 
                                qo.AddWhere(attr.KeyOfEn, ps[i].GetValStrByKey("Exp"), ps[i].GetValStrByKey("Val"), paramName);
                                break;
                            case DataType.AppInt:
                            case DataType.AppFloat:
                            case DataType.AppDouble:
                            case DataType.AppMoney:
                            case DataType.AppRate:      // Digital 
                                qo.AddWhere(attr.KeyOfEn, ps[i].GetValStrByKey("Exp"), ps[i].GetValStrByKey("Val"), paramName);
                                break;
                        }
                        break;
                    default:
                        break;
                }

                if (ps[i].GetValStrByKey("RightBreak") == ")")
                    qo.addRightBracket();

                // Finally, a connector is not added to the list 
                if (i < ps.Count - 1)
                {
                    if (ps[i].GetValStrByKey("Union") == "And")
                        qo.addAnd();
                    else if (ps[i].GetValStrByKey("Union") == "Or")
                        qo.addOr();
                }
            }

            return qo;
        }

        protected void btnSaveSearch_Click(object sender, EventArgs e)
        {
            UserRegedit currUR = null;

            try
            {
                currUR = GetCurrentUserRegedit();
                currUR.Paras = GenerateParas(currUR);
                currUR.GenerSQL = GenerateSQLByQueryObject(currUR).SQLWithOutPara;
                currUR.Save();
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message, true, true);
                return;
            }

            var url = string.Format("SearchAdv.aspx?RptNo={0}&FK_Flow={1}", this.RptNo,
                                    this.FK_Flow);

            if (!currUR.MyPK.Contains("SearchAdvModel"))
            {
                var modelName = this.hiddenAttrKey.Value;
                var urModel = new UserRegedit();
                urModel.AutoMyPK = false;
                urModel.FK_Emp = WebUser.No;
                urModel.CfgKey = this.RptNo + "_SearchAdv";

                var lastModelName =
                    BP.DA.DBAccess.RunSQLReturnString(
                        string.Format(
                            "SELECT TOP 1 SUR.MyPK FROM Sys_UserRegedit sur WHERE sur.CfgKey = '{0}' AND sur.MyPK LIKE '{1}{0}Model%' ORDER BY sur.MyPK DESC",
                            urModel.CfgKey, urModel.FK_Emp));

                urModel.MyPK = string.Format("{0}{1}Model{2}", urModel.FK_Emp, urModel.CfgKey,
                                             lastModelName == null
                                                 ? "001"
                                                 : (int.Parse(
                                                     lastModelName.Substring(
                                                         string.Format("{0}{1}Model", urModel.FK_Emp, urModel.CfgKey)
                                                             .Length)) + 1).ToString("000"));
                urModel.NumKey = modelName;
                urModel.GenerSQL = currUR.GenerSQL;
                urModel.Paras = currUR.Paras;
                urModel.Insert();

                url += "&CurrentUR_PK=" + urModel.MyPK;
            }
            else
            {
                url += "&CurrentUR_PK=" + currUR.MyPK;
            }

            Response.Redirect(url, true);
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            MapData md = new MapData(this.RptNo);
            Entities ens = md.HisEns;
            Entity en = ens.GetNewEntity;
            var currUR = GetCurrentUserRegedit();
            var qo = GenerateSQLByQueryObject(currUR);

            DataTable dt = qo.DoQueryToTable();
            DataTable myDT = new DataTable();
            MapAttrs attrs = new MapAttrs(this.RptNo);

            foreach (MapAttr attr in attrs)
            {
                myDT.Columns.Add(new DataColumn(attr.Name, typeof(string)));
            }

            foreach (DataRow dr in dt.Rows)
            {
                DataRow myDR = myDT.NewRow();

                foreach (MapAttr attr in attrs)
                {
                    if (dt.Columns.Contains(attr.Field + "Text"))
                        myDR[attr.Name] = dr[attr.Field + "Text"];
                    else
                        myDR[attr.Name] = dr[attr.Field];
                }

                myDT.Rows.Add(myDR);
            }

            try
            {
                this.ExportDGToExcel(myDT, en.EnDesc);
            }
            catch (Exception ex)
            {
                this.Alert(ex);
            }

            this.SetDGData(this.PageIdx);
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            var hattr = hiddenAttrKey.Value.Split('^');
            if (hattr.Length < 2)
            {
                this.Alert(" Parameter error ", true, true);
                return;
            }

            var edittingCondIdx = hattr[0];
            var newAttr = hattr[1];
            var isNewCond = string.IsNullOrWhiteSpace(edittingCondIdx);
            var condIdx = -1;
            int.TryParse(edittingCondIdx, out condIdx);

            var currUR = GetCurrentUserRegedit();
            var isFirstCond = string.IsNullOrWhiteSpace(currUR.Paras);

            if (isFirstCond)
            {
                // Save the first query criteria used in the field 
                currUR.Paras = "@LeftBreak=" + (Pub2.FindControl("DDL_LeftBreak") as DDL).SelectedItemStringVal;
                currUR.Paras += "@AttrKey=" + newAttr;
                currUR.Paras += "@Exp=@Val=@RightBreak=@Union=";
                currUR.Save();
            }
            else
            {
                var paras = currUR.Paras.Split("^".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                AtPara ap = null;
                Control ctrl = null;

                currUR.Paras = string.Empty;

                for (var i = 0; i < paras.Length; i++)
                {
                    ap = new AtPara(paras[i]);
                    ap.SetVal("LeftBreak", (Pub2.FindControl("DDL_LeftBreak_" + i) as DDL).SelectedItemStringVal);

                    if (i == condIdx && !string.IsNullOrWhiteSpace(edittingCondIdx))
                    {
                        ap.SetVal("AttrKey", newAttr);
                        ap.SetVal("Exp", string.Empty);
                        ap.SetVal("Val", string.Empty);
                    }
                    else
                    {
                        ap.SetVal("Exp", (Pub2.FindControl("DDL_Exp_" + i) as DDL).SelectedItemStringVal);

                        ctrl = Pub2.FindControl("TB_Val_" + i);

                        if (ctrl != null)
                        {
                            ap.SetVal("Val", (ctrl as TB).Text);
                        }
                        else
                        {
                            ctrl = Pub2.FindControl("DDL_Val_" + i);

                            if (ctrl == null)
                                throw new Exception(" The query is not found ");

                            ap.SetVal("Val", (ctrl as DDL).SelectedItemStringVal);
                        }
                    }

                    ap.SetVal("RightBreak", (Pub2.FindControl("DDL_RightBreak_" + i) as DDL).SelectedItemStringVal);
                    ap.SetVal("Union", (Pub2.FindControl("DDL_Union_" + i) as DDL).SelectedItemStringVal);

                    currUR.Paras += ap.GenerAtParaStrs() + "^";
                }

                if (isNewCond)
                {
                    // Save the new query conditions plus 
                    currUR.Paras += "@LeftBreak=@AttrKey=" + newAttr;
                    currUR.Paras += "@Exp=@Val=@RightBreak=@Union=";
                }

                try
                {
                    // Generate SQL
                    currUR.GenerSQL = GenerateSQLByQueryObject(currUR).SQLWithOutPara;
                    currUR.Save();
                }
                catch (Exception ex)
                {
                    this.Alert(ex.Message, true, true);
                    return;
                }
            }

            Response.Redirect(string.Format("SearchAdv.aspx?RptNo={0}&FK_Flow={1}&CurrentUR_PK={2}", this.RptNo,
                                            this.FK_Flow, this.CurrentUR_PK));
        }

        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            var condIdx = int.Parse((sender as LinkBtn).ID.Replace("Btn_Delete_", ""));
            var currUR = GetCurrentUserRegedit();

            var paras = currUR.Paras.Split("^".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var newParas = string.Empty;

            for (var i = 0; i < paras.Length; i++)
            {
                if (i == condIdx) continue;

                newParas += paras[i] + "^";
            }

            currUR.Paras = newParas;

            try
            {
                currUR.GenerSQL = GenerateSQLByQueryObject(currUR).SQLWithOutPara;
            }
            catch (Exception ex)
            {
                this.Alert(ex, true);
                return;
            }

            if (currUR.MyPK.Contains("SearchAdvModel"))
                currUR.AutoMyPK = false;

            currUR.Save();

            Response.Redirect(string.Format("SearchAdv.aspx?RptNo={0}&FK_Flow={1}&CurrentUR_PK={2}", this.RptNo,
                                            this.FK_Flow, this.CurrentUR_PK));
        }

        protected void btnDeleteModel_Click(object sender, EventArgs e)
        {
            var currUR = new UserRegedit();
            currUR.MyPK = this.CurrentUR_PK;
            currUR.Delete();

            Response.Redirect(NormalSearchUrl);
        }
        /// <summary>
        ///  Obtain records of the current operation 
        /// </summary>
        /// <returns></returns>
        private UserRegedit GetCurrentUserRegedit()
        {
            var currUR = new UserRegedit();

            if (string.IsNullOrEmpty(this.CurrentUR_PK))
            {
                currUR.MyPK = ViewState["newur"].ToString();
            }
            else
            {
                currUR.MyPK = this.CurrentUR_PK;
            }

            currUR.Retrieve();

            if (currUR.MyPK.Contains("SearchAdvModel"))
                currUR.AutoMyPK = false;

            return currUR;
        }

        public Entities SetDGData()
        {
            return this.SetDGData(this.PageIdx);
        }

        public Entities SetDGData(int pageIdx)
        {
            this.ResultCollapsed = false;

            #region  Perform data paging query , And bind the paging controls .

            Entities ens = this.HisEns;
            Entity en = ens.GetNewEntity;
            var currUR = GetCurrentUserRegedit();
            QueryObject qo = null;

            try
            {
                qo = GenerateSQLByQueryObject(currUR);
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message, true, true);
                return null;
            }

            this.Pub3.Clear();
            this.Pub3.BindPageIdxEasyUi(qo.GetCount(),
                                        this.PageID + ".aspx?RptNo=" + this.RptNo + "&EnsName=" + this.RptNo +
                                        "&FK_Flow=" + this.FK_Flow + "&CurrentUR_PK=" + this.CurrentUR_PK + "&IsSearch=true", pageIdx,
                                        SystemConfig.PageSize);

            qo.DoQuery(en.PK, SystemConfig.PageSize, pageIdx);
            #endregion  Perform data paging query , And bind the paging controls .

            // Binding Data .
            this.BindEns(ens, null);

            return ens;
        }

        private string GenerEnUrl(Entity en, Attrs attrs)
        {
            string url = "";
            foreach (Attr attr in attrs)
            {
                switch (attr.UIContralType)
                {
                    case UIContralType.TB:
                        if (attr.IsPK)
                            url += "&" + attr.Key + "=" + en.GetValStringByKey(attr.Key);
                        break;
                    case UIContralType.DDL:
                        url += "&" + attr.Key + "=" + en.GetValStringByKey(attr.Key);
                        break;
                }
            }
            return url;
        }
        /// <summary>
        ///  Binding entity set .
        /// </summary>
        /// <param name="ens"></param>
        /// <param name="ctrlId"></param>
        public void BindEns(Entities ens, string ctrlId)
        {
            #region  Definition of variables .
            MapData md = new MapData(this.RptNo);
            if (this.Page.Title == "")
                this.Page.Title = md.Name;

            this.UCSys1.Controls.Clear();
            Entity myen = ens.GetNewEntity;
            string pk = myen.PK;
            string clName = myen.ToString();
            Attrs attrs = myen.EnMap.Attrs;
            #endregion  Definition of variables .

            #region   Generate a table title 
            this.UCSys1.AddTable("class='Table' cellspacing='0' cellpadding='0' border='0' style='width: 100%'");
            this.UCSys1.AddTR();
            this.UCSys1.AddTDTitleGroup("No.");
            this.UCSys1.AddTDTitleGroup(" Title ");
            foreach (Attr attr in attrs)
            {
                if (attr.IsRefAttr
                    || attr.Key == "Title"
                    || attr.Key == "MyNum")
                    continue;

                this.UCSys1.AddTDTitleGroup(attr.Desc);
            }
            this.UCSys1.AddTREnd();
            #endregion   Generate a table title 

            #region  User interface attribute set 
            bool isRefFunc = false;
            isRefFunc = true;
            int pageidx = this.PageIdx - 1;
            int idx = SystemConfig.PageSize * pageidx;
            bool is1 = false;

            string focusField = "Title";
            int WinCardH = 500;
            int WinCardW = 400;
            #endregion  User interface attribute set 

            #region  Data output .
            string urlExt = "";
            foreach (Entity en in ens)
            {
                #region  Deal with keys
                string style = WebUser.Style;
                string url = this.GenerEnUrl(en, attrs);
                #endregion

                #region  Output Fields .
                idx++;
                this.UCSys1.AddTR();
                this.UCSys1.AddTDIdx(idx);
                // this.UCSys1.AddTD("<a href=\"javascript:WinOpen('./../WFRpt.aspx?FK_Flow=" + this.currMapRpt.FK_Flow + "&WorkID=" + en.GetValStrByKey("OID") + "','tdr');\" ><img src='/WF/Img/Track.png' border=0 />" + en.GetValByKey("Title") + "</a>");
                this.UCSys1.AddTD("<a href=\"javascript:WinOpen('" + BP.WF.Glo.CCFlowAppPath + "WF/WFRpt.aspx?FK_Flow=" + this.currMapRpt.FK_Flow + "&WorkID=" + en.GetValStrByKey("OID") + "','tdr');\" >" + en.GetValByKey("Title") + "</a>");

                //this.UCSys1.AddTD("<img src='/WF/Img/Track.png' border=0 />" + en.GetValByKey("Title")  );

                string val = "";
                foreach (Attr attr in attrs)
                {
                    if (attr.IsRefAttr || attr.Key == "MyNum" || attr.Key == "Title")
                        continue;

                    if (attr.UIContralType == UIContralType.DDL)
                    {
                        string s = en.GetValRefTextByKey(attr.Key);
                        if (string.IsNullOrEmpty(s))
                        {
                            switch (attr.Key)
                            {
                                case "FK_NY": // 2012-01
                                    s = en.GetValStringByKey(attr.Key);
                                    break;
                                default: // In other cases , The encoded output out .
                                    s = en.GetValStringByKey(attr.Key);
                                    break;
                            }
                        }
                        this.UCSys1.AddTD(s);
                        continue;
                    }

                    //if (attr.UIHeight != 0)
                    //{
                    //    this.UCSys1.AddTDDoc("...", "<p alt=\"" + en.GetValStringByKey(attr.Key) + "\" >...</p>");
                    //    continue;
                    //}

                    string str = en.GetValStrByKey(attr.Key);
                    //if (focusField == attr.Key)
                    //{
                    //    str = "<b><font color='blue' >" + str + "</font></a>";
                    //}

                    switch (attr.MyDataType)
                    {
                        case DataType.AppDate:
                        case DataType.AppDateTime:
                            if (str == "" || str == null)
                                str = "&nbsp;";
                            this.UCSys1.AddTD(str);
                            break;
                        case DataType.AppString:
                            if (str == "" || str == null)
                                str = "&nbsp;";

                            if (attr.UIHeight != 0)
                                this.UCSys1.AddTDDoc(str, str);
                            else
                                this.UCSys1.AddTD(str);
                            break;
                        case DataType.AppBoolean:
                            if (str == "1")
                                this.UCSys1.AddTD("是");
                            else
                                this.UCSys1.AddTD("否");
                            break;
                        case DataType.AppFloat:
                        case DataType.AppInt:
                        case DataType.AppRate:
                        case DataType.AppDouble:
                            this.UCSys1.AddTDNum(str);
                            break;
                        case DataType.AppMoney:
                            this.UCSys1.AddTDNum(decimal.Parse(str).ToString("0.00"));
                            break;
                        default:
                            throw new Exception("no this case ...");
                    }
                }
                this.UCSys1.AddTREnd();
                #endregion  Output Fields .
            }
            #endregion  Data output .

            #region  Calculate whether you can find the total , Mainly determines whether there is data in this type Entities中.
            bool IsHJ = false;
            foreach (Attr attr in attrs)
            {
                if (attr.MyFieldType == FieldType.RefText
                    || attr.Key == "Title"
                    || attr.Key == "MyNum")
                    continue;

                if (attr.UIVisible == false)
                    continue;

                if (attr.UIContralType == UIContralType.DDL)
                    continue;

                if (attr.Key == "OID" ||
                    attr.Key == "MID"
                    || attr.Key == "FID"
                    || attr.Key == "PWorkID"
                    || attr.Key.ToUpper() == "WORKID")
                    continue;

                switch (attr.MyDataType)
                {
                    case DataType.AppDouble:
                    case DataType.AppFloat:
                    case DataType.AppInt:
                    case DataType.AppMoney:
                        IsHJ = true;
                        break;
                    default:
                        break;
                }
            }
            #endregion  Calculate whether you can find the total , Mainly determines whether there is data in this type Entities中.

            #region   Total output .
            if (IsHJ)
            {
                this.UCSys1.Add("<TR class='Sum' >");
                this.UCSys1.AddTD();
                this.UCSys1.AddTD(" Total ");
                foreach (Attr attr in attrs)
                {
                    if (attr.Key == "MyNum")
                        continue;

                    if (attr.UIContralType == UIContralType.DDL)
                    {
                        this.UCSys1.AddTD();
                        continue;
                    }

                    if (attr.UIVisible == false)
                        continue;

                    if (attr.Key == "OID" || attr.Key == "MID"
                        || attr.Key.ToUpper() == "WORKID"
                        || attr.Key == "FID")
                    {
                        this.UCSys1.AddTD();
                        continue;
                    }

                    switch (attr.MyDataType)
                    {
                        case DataType.AppDouble:
                            this.UCSys1.AddTDNum(ens.GetSumDecimalByKey(attr.Key));
                            break;
                        case DataType.AppFloat:
                            this.UCSys1.AddTDNum(ens.GetSumDecimalByKey(attr.Key));
                            break;
                        case DataType.AppInt:
                            this.UCSys1.AddTDNum(ens.GetSumDecimalByKey(attr.Key));
                            break;
                        case DataType.AppMoney:
                            this.UCSys1.AddTDJE(ens.GetSumDecimalByKey(attr.Key));
                            break;
                        default:
                            this.UCSys1.AddTD();
                            break;
                    }
                }
                /* End loop */
                this.UCSys1.AddTD();
                this.UCSys1.AddTREnd();
            }
            #endregion
            this.UCSys1.AddTableEnd();
        }
    }
}