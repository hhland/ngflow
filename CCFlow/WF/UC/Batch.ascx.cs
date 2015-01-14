using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.WF.Data;
using BP.WF.Template;
using BP.Port;
using BP.Sys;
using BP.Web.Controls;
using BP.DA;
using BP.En;
using BP.Web;
namespace CCFlow.WF.UC
{
    public partial class Batch : BP.Web.UC.UCBase3
    {
        #region  Property .
        /// <summary>
        ///  Node ID
        /// </summary>
        public int FK_Node
        {
            get
            {
                return int.Parse(this.Request.QueryString["FK_Node"]);
            }
        }
        /// <summary>
        ///  Form ID
        /// </summary>
        public string FK_MapData
        {
            get
            {
                return "ND" + this.FK_Node;
            }
        }
        /// <summary>
        ///  Review the number of batch .
        /// </summary>
        public int ListNum
        {
            get
            {
                return 12;
            }
        }
        #endregion  Property .

        /// <summary>
        ///  Get node batch processing 
        /// </summary>
        public int BindNodeList()
        {

            string sql = "SELECT a.NodeID, a.Name,a.FlowName, COUNT(*) AS NUM  FROM WF_Node a, WF_EmpWorks b WHERE A.NodeID=b.FK_Node AND B.FK_Emp='" + WebUser.No + "' AND b.WFState NOT IN (7) AND a.BatchRole!=0 GROUP BY A.NodeID, a.Name,a.FlowName ";
            DataTable dt = DBAccess.RunSQLReturnTable(sql);

            this.AddTable("width=100%");
            this.AddCaption(" Batch ");
            this.AddTR();
            this.AddTDBegin();

            this.AddBR();
            this.AddUL();
            foreach (DataRow dr in dt.Rows)
            {
                this.Add("<Li style='list-style-type:square; color:#959595;'><a href='Batch.aspx?FK_Node=" + dr["NodeID"]
                    + "'  style=\"text-decoration:none; font-size:14px; font-weight:normal;\">" + dr["FlowName"].ToString() + " --> " + dr["Name"].ToString() + "(" + dr["Num"] + ")" + "</a></Li>");
                this.AddBR();
            }
            this.AddULEnd();


            this.AddTDEnd();
            this.AddTREnd();
            this.AddTableEnd();

            return dt.Rows.Count;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Request.QueryString["FK_Node"] == null)
            {
                //  If the node is not received ID Parameters , To bind the current staff can perform batch audit work to be done .
                int num = this.BindNodeList();
                return;
            }

            BP.WF.Node nd = new BP.WF.Node(this.FK_Node);
            Flow fl = nd.HisFlow;
            string sql = "SELECT a.*, b.Starter,b.ADT,b.WorkID FROM " + fl.PTable
                        + " a , WF_EmpWorks b WHERE a.OID=B.WorkID AND b.WFState Not IN (7) AND b.FK_Node=" + nd.NodeID
                        + " AND b.FK_Emp='" + WebUser.No + "'";
            // string sql = "SELECT Title,RDT,ADT,SDT,FID,WorkID,Starter FROM WF_EmpWorks WHERE FK_Emp='" + WebUser.No + "'";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            if (nd.HisBatchRole == BatchRole.None)
            {
                this.AddFieldSetRed(" Error ", " Node (" + nd.Name + ") Can not perform batch processing operations .");
                return;
            }

            string inSQL = "SELECT WorkID FROM WF_EmpWorks WHERE FK_Emp='" + WebUser.No + "' AND WFState!=7 ";
            Works wks = nd.HisWorks;
            wks.RetrieveInSQL(inSQL);

            BtnLab btnLab = new BtnLab(this.FK_Node);
            this.AddTable("width='100%'");

            // Move button position 
            if (nd.HisBatchRole == BatchRole.Group)
                this.AddCaption(" Batch : " + nd.FlowName + " --> " + nd.Name + "<a href='Batch.aspx'> Return </a>&nbsp;&nbsp;<input  ID=\"btnGroup\" type=\"button\" value=\" Reply combined volume \" CssClass=\"Btn\" onclick=\"BatchGroup()\" />");
            else
                this.AddCaption(" Batch : " + nd.FlowName + " --> " + nd.Name + "<a href='Batch.aspx'> Return </a>");

            #region  Generation title .
            this.AddTR();
            this.AddTDTitle(" No. ");
            string str1 = "<INPUT id='checkedAll' onclick='SelectAll()' value=' Choose ' type='checkbox' name='checkedAll'>";
            this.AddTDTitle("align='left'", str1);
            this.AddTDTitle(" Title ");
            this.AddTDTitle(" Sponsor ");
            this.AddTDTitle(" Accepted ");

            //  Fields displayed . BatchParas The rules for  @ Fields Chinese name =fieldName@ Fields Chinese name 1=fieldName1 
            MapAttrs attrs = new MapAttrs(this.FK_MapData);
            string[] strs = nd.BatchParas.Split(',');
            foreach (string str in strs)
            {
                if (string.IsNullOrEmpty(str) || str.Contains("@PFlowNo") == true)
                    continue;

                foreach (MapAttr attr in attrs)
                {
                    if (str != attr.KeyOfEn)
                        continue;
                    this.AddTDTitle(attr.Name);
                }
            }
            this.AddTREnd();
            #endregion  Generation title .

            GERpt rpt = nd.HisFlow.HisGERpt;
            bool is1 = false;
            int idx = -1;
            foreach (Work wk in wks)
            {
                idx++;
                if (idx == this.ListNum)
                    break;

                #region  Show the necessary columns .
                is1 = this.AddTR(is1);
                this.AddTDIdx(idx);
                CheckBox cb = new CheckBox();
                cb.ID = "CB_" + wk.OID.ToString();
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["WorkID"].ToString() != wk.OID.ToString())
                        continue;

                    cb.Text = "";
                    this.AddTD(cb);

                    //this.AddTD("<a href=\"javascript:WinOpen('MyFlow.aspx?WorkID=" + wk.OID + "&FK_Node=" + this.FK_Node + "&FK_Flow="+nd.FK_Flow+"','s')\" >" + dr["Title"].ToString() + "</a>");
                    this.AddTD("<a href=\"javascript:WinOpen('FlowFormTree/Default.aspx?WorkID=" + wk.OID + "&FK_Node=" + this.FK_Node + "&IsSend=0&FK_Flow=" + nd.FK_Flow + "','s')\" >" + dr["Title"].ToString() + "</a>");
                    this.AddTD(dr["Starter"].ToString());
                    this.AddTD(dr["ADT"].ToString());
                    break;
                }
                #endregion  Show the necessary columns .

                #region  Shows the field data from the definition of ..
                foreach (string str in strs)
                {
                    if (string.IsNullOrEmpty(str) || str.Contains("@PFlowNo") == true)
                        continue;
                    foreach (MapAttr attr in attrs)
                    {
                        if (str != attr.KeyOfEn)
                            continue;

                        TB tb = new TB();
                        tb.ID = "TB_" + attr.KeyOfEn + "_" + wk.OID;
                        switch (attr.LGType)
                        {
                            case FieldTypeS.Normal:
                                switch (attr.MyDataType)
                                {
                                    case BP.DA.DataType.AppString:
                                        if (attr.UIRows == 1)
                                        {
                                            tb.Text = wk.GetValStringByKey(attr.KeyOfEn);
                                            tb.Attributes["style"] = "width: " + attr.UIWidth + "px; text-align: left; height: 15px;padding: 0px;margin: 0px;";
                                            if (attr.UIIsEnable)
                                                tb.CssClass = "TB";
                                            else
                                                tb.CssClass = "TBReadonly";
                                            this.AddTD(tb);
                                        }
                                        else
                                        {
                                            tb.TextMode = TextBoxMode.MultiLine;
                                            tb.Text = wk.GetValStringByKey(attr.KeyOfEn);
                                            tb.Attributes["style"] = "width: " + attr.UIWidth + "px; text-align: left;padding: 0px;margin: 0px;";
                                            tb.Attributes["maxlength"] = attr.MaxLen.ToString();
                                            tb.Rows = attr.UIRows;
                                            if (attr.UIIsEnable)
                                                tb.CssClass = "TBDoc";
                                            else
                                                tb.CssClass = "TBReadonly";
                                            this.AddTD(tb);
                                        }
                                        break;
                                    case BP.DA.DataType.AppDate:
                                        tb.ShowType = TBType.Date;
                                        tb.Text = wk.GetValStrByKey(attr.KeyOfEn);

                                        if (attr.UIIsEnable)
                                            tb.Attributes["onfocus"] = "WdatePicker();";

                                        if (attr.UIIsEnable)
                                            tb.Attributes["class"] = "TB";
                                        else
                                            tb.Attributes["class"] = "TBReadonly";

                                        tb.Attributes["style"] = "width: " + attr.UIWidth + "px; text-align: left; height: 19px;";
                                        this.AddTD(tb);
                                        break;
                                    case BP.DA.DataType.AppDateTime:
                                        tb.ShowType = TBType.DateTime;
                                        tb.Text = wk.GetValStrByKey(attr.KeyOfEn); // en.GetValStrByKey(attr.KeyOfEn);

                                        if (attr.UIIsEnable)
                                            tb.Attributes["class"] = "TBcalendar";
                                        else
                                            tb.Attributes["class"] = "TBReadonly";

                                        if (attr.UIIsEnable)
                                            tb.Attributes["onfocus"] = "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'});";
                                        tb.Attributes["style"] = "width: " + attr.UIWidth + "px; text-align: left; height: 19px;";
                                        this.AddTD(tb);
                                        break;
                                    case BP.DA.DataType.AppBoolean:
                                        cb = new CheckBox();
                                        //cb.Width = 350;
                                        cb.Text = attr.Name;
                                        cb.ID = "CB_" + attr.KeyOfEn + "_" + wk.OID;
                                        cb.Checked = attr.DefValOfBool;
                                        cb.Enabled = attr.UIIsEnable;
                                        cb.Checked = wk.GetValBooleanByKey(attr.KeyOfEn);

                                        if (cb.Enabled == false)
                                            cb.Enabled = false;
                                        else
                                        {
                                            //add by dgq 2013-4-9, Event add content modified 
                                            // cb.Attributes["onmousedown"] = "Change('" + attr.FK_MapData + "')";
                                            cb.Enabled = true;
                                        }
                                        this.AddTD(cb);
                                        break;
                                    case BP.DA.DataType.AppDouble:
                                    case BP.DA.DataType.AppFloat:
                                        tb.Attributes["style"] = "width: " + attr.GetValStrByKey("UIWidth") + "px; text-align: right; height: 19px;word-break: keep-all;";
                                        tb.Text = attr.DefVal;

                                        if (attr.UIIsEnable)
                                        {
                                            // Increased verification 
                                            tb.Attributes.Add("onkeyup", @"Change('" + attr.FK_MapData + "');");
                                            tb.Attributes.Add("onblur", @"value=value.replace(/[^-?\d+\.*\d*$]/g,'');TB_ClickNum(this,0);");
                                            tb.Attributes.Add("onClick", "TB_ClickNum(this)");
                                            tb.Attributes["OnKeyPress"] += @"javascript:return  VirtyNum(this,'float');";
                                            tb.Attributes["class"] = "TBNum";
                                        }
                                        else
                                            tb.Attributes["class"] = "TBReadonly";

                                        this.AddTD(tb);
                                        break;
                                    case BP.DA.DataType.AppInt:
                                        tb.Attributes["style"] = "width: " + attr.GetValStrByKey("UIWidth") + "px; text-align: right; height: 19px;word-break: keep-all;";
                                        tb.Text = attr.DefVal;

                                        if (attr.UIIsEnable)
                                        {
                                            // Increased verification 
                                            tb.Attributes.Add("onkeyup", @"Change('" + attr.FK_MapData + "');");
                                            tb.Attributes.Add("onblur", @"value=value.replace(/[^-?\d]/g,'');TB_ClickNum(this,0);");
                                            tb.Attributes.Add("onClick", "TB_ClickNum(this)");
                                            tb.Attributes["OnKeyPress"] += @"javascript:return  VirtyNum(this,'int');";
                                            tb.Attributes["class"] = "TBNum";
                                        }
                                        else
                                            tb.Attributes["class"] = "TBReadonly";

                                        this.AddTD(tb);
                                        break;
                                    case BP.DA.DataType.AppMoney:
                                        if (attr.UIIsEnable)
                                        {
                                            // Increased verification 
                                            tb.Attributes.Add("onkeyup", @"Change('" + attr.FK_MapData + "');");
                                            tb.Attributes.Add("onblur", @"value=value.replace(/[^-?\d+\.*\d*$]/g,'');TB_ClickNum(this,'0.00');");
                                            tb.Attributes.Add("onClick", "TB_ClickNum(this)");
                                            tb.Attributes["OnKeyPress"] += @"javascript:return  VirtyNum(this,'float');";
                                            tb.Attributes["class"] = "TBNum";
                                        }
                                        else
                                            tb.Attributes["class"] = "TBReadonly";

                                        //  tb.ShowType = TBType.Moneny;
                                        tb.Text = wk.GetValIntByKey(attr.KeyOfEn).ToString("0.00");

                                        tb.Attributes["style"] = "width: " + attr.GetValStrByKey("UIWidth") + "px; text-align: right; height: 19px;";
                                        this.AddTD(tb);
                                        break;
                                    case BP.DA.DataType.AppRate:
                                        if (attr.UIIsEnable)
                                            tb.Attributes["class"] = "TBNum";
                                        else
                                            tb.Attributes["class"] = "TBReadonly";
                                        tb.ShowType = TBType.Moneny;
                                        tb.Text = wk.GetValMoneyByKey(attr.KeyOfEn).ToString("0.00");
                                        tb.Attributes["style"] = "width: " + attr.GetValStrByKey("UIWidth") + "px; text-align: right; height: 19px;";
                                        this.AddTD(tb);
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case FieldTypeS.Enum:
                                if (attr.UIContralType == UIContralType.DDL)
                                {
                                    DDL ddle = new DDL();
                                    ddle.ID = "DDL_" + attr.KeyOfEn + "_" + wk.OID;
                                    ddle.BindSysEnum(attr.UIBindKey);
                                    ddle.SetSelectItem(wk.GetValIntByKey(attr.KeyOfEn));
                                    ddle.Enabled = attr.UIIsEnable;
                                    ddle.Attributes["tabindex"] = attr.IDX.ToString();
                                    if (attr.UIIsEnable)
                                    {
                                        //add by dgq 2013-4-9, Event add content modified 
                                        ddle.Attributes["onchange"] = "Change('" + attr.FK_MapData + "')";
                                    }
                                    //    ddle.Enabled = false;
                                    this.AddTD(ddle);
                                }
                                else
                                {

                                }
                                break;
                            case FieldTypeS.FK:
                                DDL ddl1 = new DDL();
                                ddl1.ID = "DDL_" + attr.KeyOfEn + "_" + wk.OID;
                                ddl1.Attributes["tabindex"] = attr.IDX.ToString();
                                if (ddl1.Enabled)
                                {
                                    EntitiesNoName ens = attr.HisEntitiesNoName;
                                    ens.RetrieveAll();
                                    ddl1.BindEntities(ens);
                                    ddl1.SetSelectItem(wk.GetValStrByKey(attr.KeyOfEn));
                                }
                                else
                                {
                                    ddl1.Attributes["style"] = "width: " + attr.UIWidth + "px;height: 19px;";
                                    if (ddl1.Enabled == true)
                                        ddl1.Enabled = false;
                                    ddl1.Attributes["Width"] = attr.UIWidth.ToString();
                                    ddl1.Items.Add(new ListItem(wk.GetValRefTextByKey(attr.KeyOfEn), wk.GetValStrByKey(attr.KeyOfEn)));
                                }
                                ddl1.Enabled = attr.UIIsEnable;
                                this.AddTD(ddl1);
                                break;
                            default:
                                break;
                        }
                    }
                }
                #endregion  Shows the field data from the definition of ..

                this.AddTREnd();
            }
            this.AddTableEndWithHR();

            MapExts mes = new MapExts(this.FK_MapData);

            #region  Handle extended attributes .
            if (mes.Count != 0)
            {
                this.Page.RegisterClientScriptBlock("s81",
              "<script language='JavaScript' src='/WF/Scripts/jquery-1.4.1.min.js' ></script>");
                this.Page.RegisterClientScriptBlock("b81",
             "<script language='JavaScript' src='/WF/CCForm/MapExt.js' defer='defer' type='text/javascript' ></script>");
                this.Add("<div id='divinfo' style='width: 155px; position: absolute; color: Lime; display: none;cursor: pointer;align:left'></div>");
                this.Page.RegisterClientScriptBlock("dCd",
    "<script language='JavaScript' src='/DataUser/JSLibData/" + this.FK_MapData + ".js' ></script>");

                foreach (Work wk in wks)
                {
                    foreach (MapExt me in mes)
                    {
                        switch (me.ExtType)
                        {
                            case MapExtXmlList.DDLFullCtrl: //  Automatic filling .
                                DDL ddlOper = this.GetDDLByID("DDL_" + me.AttrOfOper + "_" + wk.OID);
                                if (ddlOper == null)
                                    continue;
                                ddlOper.Attributes["onchange"] = "DDLFullCtrl(this.value,\'" + ddlOper.ClientID + "\', \'" + me.MyPK + "\')";
                                break;
                            case MapExtXmlList.ActiveDDL:
                                DDL ddlPerant = this.GetDDLByID("DDL_" + me.AttrOfOper + "_" + wk.OID);
                                string val, valC;
                                if (ddlPerant == null || wk.OID < 100)
                                    continue;

#warning  Here you need to optimize 
                                string ddlC = "ContentPlaceHolder1_Batch1_DDL_" + me.AttrsOfActive + "_" + wk.OID;
                                //  ddlPerant.Attributes["onchange"] = " isChange=true; DDLAnsc(this.value, \'" + ddlC + "\', \'" + me.MyPK + "\')";
                                ddlPerant.Attributes["onchange"] = "DDLAnsc(this.value, \'" + ddlC + "\', \'" + me.MyPK + "\')";

                                DDL ddlChild = this.GetDDLByID("DDL_" + me.AttrsOfActive + "_" + wk.OID);
                                val = ddlPerant.SelectedItemStringVal;
                                if (ddlChild.Items.Count == 0)
                                    valC = wk.GetValStrByKey(me.AttrsOfActive);
                                else
                                    valC = ddlChild.SelectedItemStringVal;

                                string mysql = me.Doc.Replace("@Key", val);
                                if (mysql.Contains("@"))
                                {
                                    mysql = BP.WF.Glo.DealExp(mysql, wk, null);
                                }

                                ddlChild.Bind(DBAccess.RunSQLReturnTable(mysql), "No", "Name");
                                if (ddlChild.SetSelectItem(valC) == false)
                                {
                                    ddlChild.Items.Insert(0, new ListItem(" Please select " + valC, valC));
                                    ddlChild.SelectedIndex = 0;
                                }
                                break;
                            case MapExtXmlList.AutoFullDLL: // Automatically populate drop-down box range .
                                DDL ddlFull = this.GetDDLByID("DDL_" + me.AttrOfOper + "_" + wk.OID);
                                if (ddlFull == null)
                                    continue;

                                string valOld = wk.GetValStrByKey(me.AttrOfOper);

                                string fullSQL = me.Doc.Replace("@WebUser.No", WebUser.No);
                                fullSQL = fullSQL.Replace("@WebUser.Name", WebUser.Name);
                                fullSQL = fullSQL.Replace("@WebUser.FK_Dept", WebUser.FK_Dept);
                                fullSQL = fullSQL.Replace("@WebUser.FK_DeptName", WebUser.FK_DeptName);
                                fullSQL = fullSQL.Replace("@Key", this.Request.QueryString["Key"]);

                                if (fullSQL.Contains("@"))
                                {
                                    Attrs attrsFull = wk.EnMap.Attrs;
                                    foreach (Attr attr in attrsFull)
                                    {
                                        if (fullSQL.Contains("@") == false)
                                            break;
                                        fullSQL = fullSQL.Replace("@" + attr.Key, wk.GetValStrByKey(attr.Key));
                                    }
                                }

                                ddlFull.Items.Clear();
                                ddlFull.Bind(DBAccess.RunSQLReturnTable(fullSQL), "No", "Name");
                                if (ddlFull.SetSelectItem(valOld) == false)
                                {
                                    ddlFull.Items.Insert(0, new ListItem(" Please select " + valOld, valOld));
                                    ddlFull.SelectedIndex = 0;
                                }
                                // ddlFull.Attributes["onchange"] = " isChange=true;";
                                break;
                            case MapExtXmlList.TBFullCtrl: //  Automatic filling .
                                TextBox tbAuto = this.GetTextBoxByID("TB_" + me.AttrOfOper + "_" + wk.OID);
                                if (tbAuto == null)
                                    continue;
                                // tbAuto.Attributes["onkeyup"] = " isChange=true; DoAnscToFillDiv(this,this.value,\'" + tbAuto.ClientID + "\', \'" + me.MyPK + "\');";
                                tbAuto.Attributes["onkeyup"] = " DoAnscToFillDiv(this,this.value,\'" + tbAuto.ClientID + "\', \'" + me.MyPK + "\');";
                                tbAuto.Attributes["AUTOCOMPLETE"] = "OFF";
                                if (me.Tag != "")
                                {
                                    /*  Handle drop-down box to select the range of issues  */
                                    string[] strsTmp = me.Tag.Split('$');
                                    foreach (string str in strsTmp)
                                    {
                                        string[] myCtl = str.Split(':');
                                        string ctlID = myCtl[0];
                                        DDL ddlC1 = this.GetDDLByID("DDL_" + ctlID + "_" + wk.OID);
                                        if (ddlC1 == null)
                                        {
                                            //me.Tag = "";
                                            // me.Update();
                                            continue;
                                        }
                                        sql = myCtl[1].Replace("~", "'");
                                        sql = sql.Replace("@WebUser.No", WebUser.No);
                                        sql = sql.Replace("@WebUser.Name", WebUser.Name);
                                        sql = sql.Replace("@WebUser.FK_Dept", WebUser.FK_Dept);
                                        sql = sql.Replace("@Key", tbAuto.Text.Trim());
                                        dt = DBAccess.RunSQLReturnTable(sql);
                                        string valC1 = ddlC1.SelectedItemStringVal;
                                        ddlC1.Items.Clear();
                                        foreach (DataRow dr in dt.Rows)
                                            ddlC1.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                                        ddlC1.SetSelectItem(valC1);
                                    }
                                }
                                break;
                            case MapExtXmlList.InputCheck:
                                TextBox tbCheck = this.GetTextBoxByID("TB_" + me.AttrOfOper + "_" + wk.OID);
                                if (tbCheck != null)
                                    tbCheck.Attributes[me.Tag2] += " rowPK=" + wk.OID + "; " + me.Tag1 + "(this);";
                                break;
                            case MapExtXmlList.PopVal: // Pop-up window .
                                TB tb = this.GetTBByID("TB_" + me.AttrOfOper + "_" + wk.OID);
                                //  tb.Attributes["ondblclick"] = " isChange=true; ReturnVal(this,'" + me.Doc + "','sd');";
                                tb.Attributes["ondblclick"] = " ReturnVal(this,'" + me.Doc + "','sd');";
                                break;
                            case MapExtXmlList.Link: //  Hyperlinks .
                                //TB tb = this.Pub1.GetTBByID("TB_" + me.AttrOfOper + "_" + mydtl.OID);
                                //tb.Attributes["ondblclick"] = " isChange=true; ReturnVal(this,'" + me.Doc + "','sd');";
                                break;
                            case MapExtXmlList.RegularExpression:// Regex , Control data processing 
                                TextBox tbExp = this.GetTextBoxByID("TB_" + me.AttrOfOper + "_" + wk.OID);
                                if (tbExp == null || me.Tag == "onsubmit")
                                    continue;
                                // Regular validate input format 
                                string regFilter = me.Doc;
                                if (regFilter.LastIndexOf("/g") < 0 && regFilter.LastIndexOf('/') < 0)
                                    regFilter = "'" + regFilter + "'";
                                // Handling Events 
                                tbExp.Attributes.Add("" + me.Tag + "", "return txtTest_Onkeyup(this," + regFilter + ",'" + me.Tag1 + "')");//[me.Tag] += "this.value=this.value.replace(" + regFilter + ",'')";
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            #endregion  Expand property 

            Button btn = new Button();
            if (nd.HisBatchRole == BatchRole.Ordinary)
            {
                /* If ordinary batch .*/
                btn.CssClass = "Btn";
                btn.ID = "Btn_Send";
                btn.Text = " Batch Processing :" + btnLab.SendLab;
                btn.Click += new EventHandler(btn_Send_Click);
                btn.Attributes["onclick"] = " return confirm(' Are you sure you want to perform ?');";
                this.Add(btn);
                if (btnLab.ReturnEnable == false)
                {
                    btn = new Button();
                    btn.CssClass = "Btn";
                    btn.ID = "Btn_Return";
                    btn.Text = " Batch Processing :" + btnLab.ReturnEnable;
                    btn.Click += new EventHandler(btnDelete_Return_Click);
                    btn.Attributes["onclick"] = " return confirm(' Are you sure you want to perform ?');";
                    this.Add(btn);
                }
            }

            if (nd.HisBatchRole == BatchRole.Group)
            {
                /* If the packet Review ?*/
                btn = new Button();
                btn.CssClass = "Btn";
                btn.ID = "Btn_Group";
                //btn.Text = btnLab.SendLab;
                btn.Text = " Reply combined volume ";
                btn.Click += new EventHandler(btn_Group_Click);
                btn.Attributes["onclick"] = " return confirm(' Are you sure you want to perform ?');";
                this.Add(btn);
            }

            if (btnLab.ReturnEnable == false)
            {
                btn = new Button();
                btn.CssClass = "Btn";
                btn.ID = "Btn_Return";
                btn.Text = " Batch Processing :" + btnLab.ReturnEnable;
                btn.Click += new EventHandler(btnDelete_Return_Click);
                btn.Attributes["onclick"] = " return confirm(' Are you sure you want to perform ?');";
                this.Add(btn);
            }

            if (btnLab.DeleteEnable != 0)
            {
                btn = new Button();
                btn.CssClass = "Btn";
                btn.ID = "Btn_Del";
                btn.Text = " Batch Processing :" + btnLab.DeleteLab;
                btn.Click += new EventHandler(btnDelete_Click);
                btn.Attributes["onclick"] = " return confirm(' Are you sure you want to perform ?');";
                this.Add(btn);
            }
        }

        #region   Batch processes promoter group 
        void btn_Group_Click(object sender, EventArgs e)
        {
            string sql = "SELECT Title,RDT,ADT,SDT,FID,WorkID,Starter FROM WF_EmpWorks WHERE FK_Emp='" + WebUser.No + "'";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            BP.WF.Node nd = new BP.WF.Node(this.FK_Node);
            string[] strs = nd.BatchParas.Split(',');
            MapAttrs attrs = new MapAttrs(this.FK_MapData);

            string msg = "";
            string ids = "";
            int idx = -1;
            foreach (DataRow dr in dt.Rows)
            {
                Int64 workid = Int64.Parse(dr["WorkID"].ToString());
                CheckBox cb = this.GetCBByID("CB_" + workid);
                if (cb == null || cb.Checked == false)
                    continue;

                idx++;
                if (idx == this.ListNum)
                    break;

                ids += workid + ",";
                Hashtable ht = new Hashtable();

                #region  To property assignment .
                bool isChange = false;
                foreach (string str in strs)
                {
                    if (string.IsNullOrEmpty(str))
                        continue;
                    foreach (MapAttr attr in attrs)
                    {
                        if (str != attr.KeyOfEn)
                            continue;

                        if (attr.LGType == FieldTypeS.Normal)
                        {
                            TB tb = this.GetTBByID("TB_" + attr.KeyOfEn + "_" + workid);
                            if (tb != null)
                            {
                                if (tb.Text != attr.DefVal)
                                    isChange = true;

                                ht.Add(str, tb.Text);
                                continue;
                            }

                            cb = this.GetCBByID("CB_" + attr.KeyOfEn + "_" + workid);
                            if (cb != null)
                            {
                                if (cb.Checked != attr.DefValOfBool)
                                    isChange = true;

                                if (cb.Checked)
                                    ht.Add(str, 1);
                                else
                                    ht.Add(str, 0);
                                continue;
                            }
                        }
                        else
                        {
                            DDL ddl = this.GetDDLByID("DDL_" + attr.KeyOfEn + "_" + workid);
                            if (ddl != null)
                            {
                                if (ddl.SelectedItemStringVal != attr.DefVal)
                                    isChange = true;
                                if (attr.LGType == FieldTypeS.Enum)
                                    ht.Add(str, ddl.SelectedItemIntVal);
                                else
                                    ht.Add(str, ddl.SelectedItemStringVal);
                                continue;
                            }
                        }
                    }
                }
                #endregion  To property assignment .

                // The save .
                BP.WF.Dev2Interface.Node_SaveWork(nd.FK_Flow, this.FK_Node, workid, ht);
            }

            if (ids == "")
            {
                this.Alert(" You did not choose to work .");
            }
            else
            {
                string[] paras = nd.BatchParas.Split(',');
                string[] mystrs = paras[0].Split('=');
                string flowNo = mystrs[1];

                // BP.Sys.PubClass.WinOpen("MyFlow.aspx?FK_Flow=" + flowNo + "&FK_Node=" + flowNo + "01&DoFunc=SetParentFlow&CFlowNo=" + nd.FK_Flow + "&WorkIDs=" + ids, 1000, 900);
                //this.Response.Redirect("MyFlow.aspx?FK_Flow=" + flowNo + "&FK_Node=" + flowNo + "01&DoFunc=SetParentFlow&CFlowNo=" + nd.FK_Flow + "&WorkIDs=" + ids, true);
                string url = "MyFlow.aspx?FK_Flow=" + flowNo + "&FK_Node=" + flowNo + "01&DoFunc=SetParentFlow&CFlowNo=" + nd.FK_Flow + "&WorkIDs=" + ids;
                WinOpen(url);
            }
        }
        #endregion   Batch processes promoter group 


        #region   Batch Transmission 
        // Send 
        void btn_Send_Click(object sender, EventArgs e)
        {
            string sql = "SELECT Title,RDT,ADT,SDT,FID,WorkID,Starter FROM WF_EmpWorks WHERE FK_Emp='" + WebUser.No + "'";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            BP.WF.Node nd = new BP.WF.Node(this.FK_Node);
            string[] strs = nd.BatchParas.Split(',');
            MapAttrs attrs = new MapAttrs(this.FK_MapData);

            string msg = "";
            int idx = -1;
            foreach (DataRow dr in dt.Rows)
            {
                idx++;
                if (idx == this.ListNum)
                    break;

                Int64 workid = Int64.Parse(dr["WorkID"].ToString());
                CheckBox cb = this.GetCBByID("CB_" + workid);
                if (cb == null || cb.Checked == false)
                    continue;

                Hashtable ht = new Hashtable();

                #region  To property assignment .
                bool isChange = false;
                foreach (string str in strs)
                {
                    if (string.IsNullOrEmpty(str))
                        continue;
                    foreach (MapAttr attr in attrs)
                    {
                        if (str != attr.KeyOfEn)
                            continue;

                        if (attr.LGType == FieldTypeS.Normal)
                        {
                            TB tb = this.GetTBByID("TB_" + attr.KeyOfEn + "_" + workid);
                            if (tb != null)
                            {
                                if (tb.Text != attr.DefVal)
                                    isChange = true;

                                ht.Add(str, tb.Text);
                                continue;
                            }

                            cb = this.GetCBByID("CB_" + attr.KeyOfEn + "_" + workid);
                            if (cb != null)
                            {
                                if (cb.Checked != attr.DefValOfBool)
                                    isChange = true;

                                if (cb.Checked)
                                    ht.Add(str, 1);
                                else
                                    ht.Add(str, 0);
                                continue;
                            }
                        }
                        else
                        {
                            DDL ddl = this.GetDDLByID("DDL_" + attr.KeyOfEn + "_" + workid);
                            if (ddl != null)
                            {
                                if (ddl.SelectedItemStringVal != attr.DefVal)
                                    isChange = true;
                                if (attr.LGType == FieldTypeS.Enum)
                                    ht.Add(str, ddl.SelectedItemIntVal);
                                else
                                    ht.Add(str, ddl.SelectedItemStringVal);
                                continue;
                            }
                        }
                    }
                }
                #endregion  To property assignment .


                msg += "@ Work (" + dr["Title"] + ") Processing follows .<br>";
                BP.WF.SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(nd.FK_Flow, workid, ht);
                msg += objs.ToMsgOfHtml();
                msg += "<hr>";
            }

            if (msg == "")
            {
                this.Alert(" You did not choose to work .");
            }
            else
            {
                this.Clear();
                msg += "<a href='Batch.aspx'> Return ...</a>";

                this.AddMsgOfInfo(" Batch processing of information ", msg);
            }
        }
        #endregion

        #region  Bulk Delete 
        void btnDelete_Click(object sender, EventArgs e)
        {
            string sql = "SELECT Title,RDT,ADT,SDT,FID,WorkID,Starter FROM WF_EmpWorks WHERE FK_Emp='" + WebUser.No + "'";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);

            BP.WF.Node nd = new BP.WF.Node(this.FK_Node);

            string msg = "";
            foreach (DataRow dr in dt.Rows)
            {
                Int64 workid = Int64.Parse(dr["WorkID"].ToString());
                Int64 fid = Int64.Parse(dr["FID"].ToString());
                CheckBox cb = this.GetCBByID("CB_" + workid);
                if (cb == null || cb.Checked == false)
                    continue;

                msg += "@ Work (" + dr["Title"] + ") Processing follows .<br>";
                string mes = BP.WF.Dev2Interface.Flow_DoDeleteFlowByFlag(nd.FK_Flow, workid, " Batch return ", true);
                msg += mes;
                msg += "<hr>";
            }

            if (msg == "")
            {
                this.Alert(" You did not choose to work .");
            }
            else
            {
                this.Clear();
                msg += "<a href='Batch.aspx'> Return ...</a>";
                this.AddMsgOfInfo(" Batch processing of information ", msg);
            }
        }
        #endregion  Bulk Delete 


        // Batch return 
        void btnDelete_Return_Click(object sender, EventArgs e)
        {
            string sql = "SELECT Title,RDT,ADT,SDT,FID,WorkID,Starter FROM WF_EmpWorks WHERE FK_Emp='" + WebUser.No + "'";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);

            BP.WF.Node nd = new BP.WF.Node(this.FK_Node);

            string msg = "";
            foreach (DataRow dr in dt.Rows)
            {
                Int64 workid = Int64.Parse(dr["WorkID"].ToString());
                Int64 fid = Int64.Parse(dr["FID"].ToString());
                CheckBox cb = this.GetCBByID("CB_" + workid);
                if (cb == null || cb.Checked == false)
                    continue;

                msg += "@ Work (" + dr["Title"] + ") Processing follows .<br>";
                BP.WF.SendReturnObjs objs = null;// BP.WF.Dev2Interface.Node_ReturnWork(nd.FK_Flow, workid,fid,this.FK_Node," Batch return ");
                msg += objs.ToMsgOfHtml();
                msg += "<hr>";
            }

            if (msg == "")
            {
                this.Alert(" You did not choose to work .");
            }
            else
            {
                this.Clear();
                msg += "<a href='Batch.aspx'> Return ...</a>";
                this.AddMsgOfInfo(" Batch processing of information ", msg);
            }
        }

        protected void btnGroup_Click(object sender, EventArgs e)
        {
            btn_Group_Click(sender, e);
        }

    }
}