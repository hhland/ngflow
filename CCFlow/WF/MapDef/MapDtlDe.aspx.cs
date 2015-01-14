using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BP.Sys;
using BP.En;
using BP.Web.Controls;
using BP.DA;
using BP.Web;
namespace CCFlow.WF.MapDef
{
    public partial class Comm_MapDef_MapDtlDe : BP.Web.WebPage
    {
        #region  Property 
        public new string MyPK
        {
            get
            {
                return this.Request.QueryString["FK_MapDtl"];
            }
        }
        public new string DoType
        {
            get
            {
                return this.Request.QueryString["DoType"];
            }
        }
        public string FK_MapExt
        {
            get
            {
                return this.Request.QueryString["FK_MapExt"];
            }
        }
        public new string Key
        {
            get
            {
                return this.Request.QueryString["Key"];
            }
        }
        public string FK_MapData
        {
            get
            {
                return this.Request.QueryString["FK_MapData"];
            }
        }
        public string FK_MapDtl
        {
            get
            {
                return this.Request.QueryString["FK_MapDtl"];
            }
        }
        #endregion  Property 

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = " From Table Design ";

            MapData.IsEditDtlModel = true;
            MapData md = new MapData(this.FK_MapData);
            MapDtl dtl = new MapDtl(this.FK_MapDtl);
            if (dtl.IsView == false)
                return;

            MapAttrs attrs = new MapAttrs(this.MyPK);
            MapAttrs attrs2 = new MapAttrs();
            MapExts mes = new MapExts(this.MyPK);
            string LinkFields = ",";
            if (mes.Count != 0)
            {
                foreach (MapExt me in mes)
                {
                    switch (me.ExtType)
                    {
                        case MapExtXmlList.Link:
                            LinkFields += me.AttrOfOper + ",";
                            break;
                        default:
                            break;
                    }
                }

                this.Page.RegisterClientScriptBlock("s8",
              "<script language='JavaScript' src='../Scripts/jquery-1.4.1.min.js' ></script>");

                this.Page.RegisterClientScriptBlock("b8",
             "<script language='JavaScript' src='../CCForm/MapExt.js' ></script>");

                this.Page.RegisterClientScriptBlock("dCd",
       "<script language='JavaScript' src='/DataUser/JSLibData/" + this.FK_MapDtl + ".js' ></script>");

                this.Pub1.Add("<div id='divinfo' style='width: 155px; position: absolute; color: Lime; display: none;cursor: pointer;align:left'></div>");
            }

            string t = DateTime.Now.ToString("MM-dd-hh:mm:ss");
            if (attrs.Count == 0)
                dtl.IntMapAttrs();
            

            this.Title = md.Name + " -  Design Details ";
            this.Pub1.AddTable("class='Table' border='0' ID='Tab' cellspacing='0' cellpadding='0' ");
            //     this.Pub1.AddCaptionLeftTX("<a href='MapDef.aspx?MyPK=" + md.No + "' ><img src='../Img/Btn/Back.gif' border=0/>" + this.ToE("Back"," Return ") + ":" + md.Name + "</a> - <img src='../Img/Btn/Table.gif' border=0/>" + dtl.Name + " - <a href=\"javascript:AddF('" + this.MyPK + "');\" ><img src='../Img/Btn/New.gif' border=0/>" + " New Field " + "</a> ");
            this.Pub1.Add(dtl.MTR);
            
            #region  Output Title .
            this.Pub1.AddTR();
            if (dtl.IsShowIdx)
                this.Pub1.AddTDTitle("");

            foreach (MapAttr attr in attrs)
            {
                if (attr.UIVisible == false)
                    continue;

                this.Pub1.Add("<TH style='width:" + attr.UIWidthInt + "px'>");
                this.Pub1.Add("<a href=\"javascript:Up('" + this.MyPK + "','" + attr.MyPK + "','" + t + "');\" ><img src='../Img/Btn/Left.gif' class=Arrow alt=' Move left ' border=0/></a>");
                if (attr.HisEditType == EditType.UnDel || attr.HisEditType == EditType.Edit)
                {
                    switch (attr.LGType)
                    {
                        case FieldTypeS.Normal:
                            this.Pub1.Add("<a href=\"javascript:Edit('" + this.MyPK + "','" + attr.MyPK + "','" + attr.MyDataType + "');\"  alt='" + attr.KeyOfEn + "'>" + attr.Name + "</a>");
                            break;
                        case FieldTypeS.Enum:
                            this.Pub1.Add("<a href=\"javascript:EditEnum('" + this.MyPK + "','" + attr.MyPK + "');\" alt='" + attr.KeyOfEn + "' >" + attr.Name + "</a>");
                            break;
                        case FieldTypeS.FK:
                            this.Pub1.Add("<a href=\"javascript:EditTable('" + this.MyPK + "','" + attr.MyPK + "','" + attr.MyDataTypeS + "');\"  alt='" + attr.KeyOfEn + "'>" + attr.Name + "</a>");
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    this.Pub1.Add(attr.Name);
                }
                //  this.Pub1.Add("[<a href=\"javascript:Insert('" + this.MyPK + "','" + attr.IDX + "');\" ><img src='../Img/Btn/Insert.gif' border=0/> Insert </a>]");
                this.Pub1.Add("<a href=\"javascript:Down('" + this.MyPK + "','" + attr.MyPK + "','" + t + "');\" ><img src='../Img/Btn/Right.gif' class=Arrow alt=' Move right ' border=0/></a>");
                this.Pub1.Add("</TH>");
            }

            if (dtl.IsEnableAthM)
                this.Pub1.AddTDTitle("<a href=\"javascript:Attachment('" + dtl.No + "');\"><img src='./../Img/set.gif' border=0 width='16px' /></a>");

            if (dtl.IsEnableM2M)
                this.Pub1.AddTDTitle("<a href=\"javascript:MapM2M('" + dtl.No + "');\"><img src='./../Img/set.gif' border=0 width='16px' /></a>");

            if (dtl.IsEnableM2MM)
                this.Pub1.AddTDTitle("<a href=\"javascript:window.showModalDialog('MapM2MM.aspx?NoOfObj=M2MM&FK_MapData=" + this.FK_MapDtl + "','m2m','dialogHeight: 500px; dialogWidth: 600px;center: yes; help: no')\"><img src='./../Img/set.gif' border=0 width='16px' /></a>");

            if (dtl.IsEnableLink)
                this.Pub1.AddTDTitle(dtl.LinkLabel);

            //Pub1.AddTDTitle("&nbsp;");

            this.Pub1.AddTREnd();
            #endregion  Output Title .

            #region  Output lines .
            for (int i = 1; i <= dtl.RowsOfList; i++)
            {
                this.Pub1.AddTR();
                if (dtl.IsShowIdx)
                    this.Pub1.AddTDIdx(i);
                foreach (MapAttr attr in attrs)
                {
                    if (attr.UIVisible == false)
                        continue;

                    #region  Whether output hyperlinks .
                    if (attr.UIIsEnable == false && LinkFields.Contains("," + attr.KeyOfEn + ","))
                    {
                        MapExt meLink = mes.GetEntityByKey(MapExtAttr.ExtType, MapExtXmlList.Link) as MapExt;
                        string url = meLink.Tag;
                        if (url.Contains("?") == false)
                            url = url + "?a3=2";
                        url = url + "&WebUserNo=" + WebUser.No + "&SID=" + WebUser.SID + "&EnName=" + this.FK_MapDtl;
                        if (url.Contains("@AppPath"))
                            url = url.Replace("@AppPath", "http://" + this.Request.Url.Host + this.Request.ApplicationPath);
                        if (url.Contains("@"))
                        {
                            if (attrs2.Count == 0)
                                attrs2 = new MapAttrs(this.FK_MapDtl);
                            foreach (MapAttr item in attrs2)
                            {
                                url = url.Replace("@" + item.KeyOfEn, item.DefVal);
                                if (url.Contains("@") == false)
                                    break;
                            }
                        }
                        this.Pub1.AddTD("<a href='" + url + "' target='" + meLink.Tag1 + "' >" + attr.DefVal + "</a>");
                        continue;
                    }
                    #endregion  Whether output hyperlinks .

                    #region  Output Fields .
                    switch (attr.LGType)
                    {
                        case FieldTypeS.Normal:
                            if (attr.MyDataType == BP.DA.DataType.AppBoolean)
                            {
                                CheckBox cb = new CheckBox();
                                cb.Checked = attr.DefValOfBool;
                                cb.Enabled = attr.UIIsEnable;
                                cb.Text = attr.Name;
                                this.Pub1.AddTD(cb);
                                break;
                            }
                            TextBox tb = new TextBox();
                            tb.ID = "TB_" + attr.KeyOfEn + "_" + i;
                            tb.Text = attr.DefVal;
                            tb.ReadOnly = !attr.UIIsEnable;
                            this.Pub1.AddTD(tb);
                            switch (attr.MyDataType)
                            {
                                case BP.DA.DataType.AppString:
                                    tb.Attributes["style"] = "width:" + attr.UIWidth + "px;border: none;";
                                    if (attr.UIHeight > 25)
                                    {
                                        tb.TextMode = TextBoxMode.MultiLine;
                                        tb.Attributes["Height"] = attr.UIHeight + "px";
                                        tb.Rows = attr.UIHeightInt / 25;
                                    }
                                    break;
                                case BP.DA.DataType.AppDateTime:
                                    tb.Attributes["style"] = "width:" + attr.UIWidth + "px;border: none;";
                                    if (attr.UIIsEnable)
                                    {
                                        tb.Attributes["onfocus"] = "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'});";
                                        //tb.Attributes["class"] = "TBcalendar";
                                    }
                                    break;
                                case BP.DA.DataType.AppDate:
                                    tb.Attributes["style"] = "width:" + attr.UIWidth + "px;border: none;";
                                    if (attr.UIIsEnable)
                                    {
                                        tb.Attributes["onfocus"] = "WdatePicker();";
                                        //  tb.Attributes["class"] = "TBcalendar";
                                    }
                                    break;
                                default:
                                    tb.Attributes["style"] = "width:" + attr.UIWidth + "px;border: none;";
                                    if (tb.ReadOnly == false)
                                    {
                                        // OnKeyPress="javascript:return VirtyNum(this);"
                                        //tb.Attributes["OnKeyDown"] = "javascript:return VirtyNum(this);";

                                        if (attr.MyDataType == DataType.AppInt)
                                            tb.Attributes["OnKeyDown"] = "javascript:return VirtyInt(this);";
                                        else
                                            tb.Attributes["OnKeyDown"] = "javascript:return VirtyNum(this);";

                                        tb.Attributes["onkeyup"] += "javascript:C" + i + "();C" + attr.KeyOfEn + "();";
                                        tb.Attributes["class"] = "TBNum";
                                    }
                                    else
                                    {
                                        // tb.Attributes["onpropertychange"] += "C" + attr.KeyOfEn + "();";
                                        tb.Attributes["class"] = "TBNumReadonly";
                                    }
                                    break;
                            }
                            break;
                        case FieldTypeS.Enum:
                            DDL ddl = new DDL();
                            ddl.ID = "DDL_" + attr.KeyOfEn + "_" + i;
                            try
                            {
                                ddl.BindSysEnum(attr.KeyOfEn);
                                ddl.SetSelectItem(attr.DefVal);
                            }
                            catch (Exception ex)
                            {
                                BP.Sys.PubClass.Alert(ex.Message);
                            }
                            ddl.Enabled = attr.UIIsEnable;
                            this.Pub1.AddTDCenter(ddl);
                            break;
                        case FieldTypeS.FK:
                            DDL ddl1 = new DDL();
                            ddl1.ID = "DDL_" + attr.KeyOfEn + "_" + i;
                            try
                            {
                                EntitiesNoName ens = attr.HisEntitiesNoName;
                                ens.RetrieveAll();
                                ddl1.BindEntities(ens);
                                if (ddl1.SetSelectItem(attr.DefVal) == false)
                                {
                                    ddl1.Items.Insert(0, new ListItem(" Please select ", attr.DefVal));
                                    ddl1.SelectedIndex = 0;
                                }
                            }
                            catch
                            {
                            }
                            ddl1.Enabled = attr.UIIsEnable;
                            this.Pub1.AddTDCenter(ddl1);
                            break;
                        default:
                            break;
                    }
                    #endregion s Output Fields .
                }

                #region  Output Accessories ,m2m
                if (dtl.IsEnableAthM)
                    this.Pub1.AddTD("<a href=\"javascript:EnableAthM('" + this.FK_MapDtl + "');\" ><img src='../Img/AttachmentM.png' border=0 width='16px' /></a>");

                if (dtl.IsEnableM2M)
                    this.Pub1.AddTD("<a href=\"javascript:window.showModalDialog('../CCForm/M2M.aspx?NoOfObj=M2M&IsTest=1&OID=0&FK_MapData=" + this.FK_MapDtl + "','m2m','dialogHeight: 500px; dialogWidth: 600px;center: yes; help: no')\"><img src='./../Img/M2M.png' border=0 width='16px' /></a>");

                if (dtl.IsEnableM2MM)
                    this.Pub1.AddTD("<a href=\"javascript:window.showModalDialog('../CCForm/M2MM.aspx?NoOfObj=M2MM&IsTest=1&OID=0&FK_MapData=" + this.FK_MapDtl + "','m2m','dialogHeight: 500px; dialogWidth: 600px;center: yes; help: no')\"><img src='./../Img/M2MM.png' border=0 width='16px' /></a>");

                if (dtl.IsEnableLink)
                    this.Pub1.AddTD("<a href='" + dtl.LinkUrl + "' target='" + dtl.LinkTarget + "' >" + dtl.LinkLabel + "</a>");
                #endregion  Output Accessories ,m2m
                
                //Pub1.AddTD("&nbsp;");
                this.Pub1.AddTREnd();
            }
            #endregion  Output lines .

            #region  Total .
            if (dtl.IsShowSum)
            {
                this.Pub1.AddTRSum();
                if (dtl.IsShowIdx)
                    this.Pub1.AddTD(" Total ");

                foreach (MapAttr attr in attrs)
                {
                    if (attr.UIVisible == false)
                        continue;
                    if (attr.IsNum && attr.LGType == FieldTypeS.Normal)
                    {
                        TB tb = new TB();
                        tb.ID = "TB_" + attr.KeyOfEn;
                        tb.Text = attr.DefVal;
                        tb.ShowType = attr.HisTBType;
                        tb.ReadOnly = true;
                        tb.Font.Bold = true;
                        tb.BackColor = System.Drawing.Color.FromName("#FFFFFF");
                        tb.Attributes["class"] = "TBNumReadonly";
                        this.Pub1.AddTD(tb);
                    }
                    else
                    {
                        this.Pub1.AddTD();
                    }
                }
                if (dtl.IsEnableAthM)
                    this.Pub1.AddTD();

                if (dtl.IsEnableM2M)
                    this.Pub1.AddTD();

                if (dtl.IsEnableM2MM)
                    this.Pub1.AddTD();

                if (dtl.IsEnableLink)
                    this.Pub1.AddTD();
                
            //    Pub1.AddTD("&nbsp;");
                this.Pub1.AddTREnd();
            }
            this.Pub1.AddTableEnd();
            #endregion  Total .

            #region  Autofill process design from the table .
            if (this.Key != null)
            {
                MapExt me = new MapExt(this.FK_MapExt);
                string[] strs = me.Tag1.Split('$');
                foreach (string str in strs)
                {
                    if (str.Contains(this.FK_MapDtl) == false)
                        continue;

                    string[] ss = str.Split(':');

                    string sql = ss[1];
                    sql = sql.Replace("@Key", this.Key);
                    sql = sql.Replace("@key", this.Key);
                    sql = sql.Replace("@val", this.Key);
                    sql = sql.Replace("@Val", this.Key);

                    DataTable dt = DBAccess.RunSQLReturnTable(sql);
                    int idx = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        idx++;
                        foreach (DataColumn dc in dt.Columns)
                        {
                            string val = dr[dc.ColumnName].ToString();
                            try
                            {
                                this.Pub1.GetTextBoxByID("TB_" + dc.ColumnName + "_" + idx).Text = val;
                            }
                            catch
                            {
                            }

                            try
                            {
                                this.Pub1.GetDDLByID("DDL_" + dc.ColumnName + "_" + idx).SetSelectItem(val);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            #endregion  Autofill process design from the table .

            #region  Processing expansion properties .
            for (int i = 1; i <= dtl.RowsOfList; i++)
            {
                foreach (MapExt me in mes)
                {
                    switch (me.ExtType)
                    {
                        case MapExtXmlList.DDLFullCtrl: //  Automatic filling .
                            DDL ddlOper = this.Pub1.GetDDLByID("DDL_" + me.AttrOfOper);
                            if (ddlOper == null)
                                continue;
                            ddlOper.Attributes["onchange"] = "DDLFullCtrl(this.value,\'" + ddlOper.ClientID + "\', \'" + me.MyPK + "\')";
                            break;
                        case MapExtXmlList.ActiveDDL:
                            DDL ddlPerant = this.Pub1.GetDDLByID("DDL_" + me.AttrOfOper + "_" + i);
                            if (ddlPerant == null)
                            {
                                me.Delete();
                                continue;
                            }

                            DDL ddlChild = this.Pub1.GetDDLByID("DDL_" + me.AttrsOfActive + "_" + i);
                            if (ddlChild == null)
                            {
                                me.Delete();
                                continue;
                            }

                            ddlPerant.Attributes["onchange"] = "DDLAnsc(this.value,\'" + ddlChild.ClientID + "\', \'" + me.MyPK + "\')";
                            if (ddlPerant.Items.Count == 0)
                                continue;

                            string val = ddlPerant.SelectedItemStringVal;

                            string valC1 = ddlChild.SelectedItemStringVal;


                            DataTable dt = DBAccess.RunSQLReturnTable(me.Doc.Replace("@Key", val));

                            ddlChild.Items.Clear();
                            foreach (DataRow dr in dt.Rows)
                            {
                                ddlChild.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                            }
                            ddlChild.SetSelectItem(valC1);
                            break;
                        case MapExtXmlList.AutoFullDLL: // Automatically populate drop-down box range .
                            DDL ddlFull = this.Pub1.GetDDLByID("DDL_" + me.AttrOfOper + "_" + i);
                            if (ddlFull == null)
                            {
                                me.Delete();
                                continue;
                            }

                            string valOld = ddlFull.SelectedItemStringVal;
                            ddlFull.Items.Clear();
                            string fullSQL = me.Doc.Replace("@WebUser.No", WebUser.No);
                            fullSQL = fullSQL.Replace("@WebUser.FK_Dept", WebUser.FK_Dept);
                            fullSQL = fullSQL.Replace("@WebUser.Name", WebUser.Name);

                            if (fullSQL.Contains("@"))
                            {
                                //Attrs attrsFull = mydtl.EnMap.Attrs;
                                //foreach (Attr attr in attrsFull)
                                //{
                                //    if (fullSQL.Contains("@") == false)
                                //        break;
                                //    fullSQL = fullSQL.Replace("@" + attr.Key, mydtl.GetValStrByKey(attr.Key));
                                //}
                            }
                            ddlFull.Bind(DBAccess.RunSQLReturnTable(fullSQL), "No", "Name");
                            ddlFull.SetSelectItem(valOld);
                            break;
                        case MapExtXmlList.TBFullCtrl: //  Automatic filling .
                            TextBox tbAuto = this.Pub1.GetTextBoxByID("TB_" + me.AttrOfOper + "_" + i);
                            if (tbAuto == null)
                            {
                                me.Delete();
                                continue;
                            }
                            tbAuto.Attributes["onkeyup"] = "DoAnscToFillDiv(this,this.value,\'" + tbAuto.ClientID + "\', \'" + me.MyPK + "\');";
                            tbAuto.Attributes["AUTOCOMPLETE"] = "OFF";
                            if (me.Tag != "")
                            {
                                /*  Handle drop-down box to select the range of issues  */
                                string[] strs = me.Tag.Split('$');
                                foreach (string str in strs)
                                {
                                    string[] myCtl = str.Split(':');
                                    string ctlID = myCtl[0];
                                    DDL ddlC = this.Pub1.GetDDLByID("DDL_" + ctlID + "_" + i);
                                    if (ddlC == null)
                                    {
                                        continue;
                                    }

                                    string sql = myCtl[1].Replace("~", "'");
                                    sql = sql.Replace("@WebUser.No", WebUser.No);
                                    sql = sql.Replace("@WebUser.Name", WebUser.Name);
                                    sql = sql.Replace("@WebUser.FK_Dept", WebUser.FK_Dept);
                                    sql = sql.Replace("@Key", tbAuto.Text.Trim());
                                    dt = DBAccess.RunSQLReturnTable(sql);
                                    string valC = ddlC.SelectedItemStringVal;
                                    ddlC.Items.Clear();
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        ddlC.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                                    }
                                    ddlC.SetSelectItem(valC);
                                }
                            }
                            // tbAuto.Attributes["onkeyup"] = "DoAnscToFillDiv(this,this.value);";
                            // tbAuto.Attributes["onkeyup"] = "DoAnscToFillDiv(this,this.value,\'" + tbAuto.ClientID + "\', \'" + me.MyPK + "\');";
                            break;
                        case MapExtXmlList.InputCheck:
                            TextBox tbCheck = this.Pub1.GetTextBoxByID("TB_" + me.AttrOfOper + "_" + i);
                            if (tbCheck != null)
                            {
                                tbCheck.Attributes[me.Tag2] += " rowPK=" + i + ";" + me.Tag1 + "(this);";
                            }
                            else
                            {
                                me.Delete();
                            }
                            break;
                        case MapExtXmlList.PopVal: // Pop-up window .
                            TB tb = this.Pub1.GetTBByID("TB_" + me.AttrOfOper + me.AttrOfOper + "_" + i);
                            if (tb == null)
                            {
                                //me.Delete();
                                continue;
                            }
                            tb.Attributes["ondblclick"] = "ReturnVal(this,'" + me.Doc + "','sd');";
                            break;
                        default:
                            break;
                    }
                }
            }
            #endregion  Processing expansion properties .

            #region  Automatic calculation formula output 
            this.Pub1.Add("\n<script language='JavaScript'>");
            MapExts exts = new MapExts(dtl.No);
            foreach (MapExt ext in exts)
            {
                if (ext.ExtType != MapExtXmlList.AutoFull)
                    continue;

                for (int i = 1; i <= dtl.RowsOfList; i++)
                {
                    string top = "\n function C" + i + "() { \n ";
                    string script = "";
                    foreach (MapAttr attr in attrs)
                    {
                        if (attr.UIVisible == false)
                            continue;
                        if (attr.IsNum == false)
                            continue;

                        if (attr.LGType != FieldTypeS.Normal)
                            continue;

                        if (ext.Tag == "1" && ext.Doc != "")
                        {
                            script += this.GenerAutoFull(i.ToString(), attrs, ext);
                        }
                    }
                    string end = " \n  } ";
                    this.Pub1.Add(top + script + end);
                }
            }
            this.Pub1.Add("\n</script>");

            //  Total output calculation formula 
            foreach (MapAttr attr in attrs)
            {
                if (attr.UIVisible == false)
                    continue;

                if (attr.LGType != FieldTypeS.Normal)
                    continue;

                if (attr.IsNum == false)
                    continue;

                if (attr.MyDataType == DataType.AppBoolean)
                    continue;

                string top = "\n<script language='JavaScript'> function C" + attr.KeyOfEn + "() { \n ";
                string end = "\n } </script>";
                this.Pub1.Add(top + this.GenerSum(attr, dtl) + " ; \t\n" + end);
            }
            #endregion  Automatic calculation formula output 

        }
        /// <summary>
        ///  Generating a computed column 
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="attrs"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        public string GenerAutoFull(string pk, MapAttrs attrs, MapExt ext)
        {
            try
            {
                string left = "\n  document.forms[0]." + this.Pub1.GetTextBoxByID("TB_" + ext.AttrOfOper + "_" + pk).ClientID + ".value = ";
                string right = ext.Doc;
                foreach (MapAttr mattr in attrs)
                {
                    string tbID = "TB_" + mattr.KeyOfEn + "_" + pk;
                    TextBox tb = this.Pub1.GetTextBoxByID(tbID);
                    if (tb == null)
                        continue;
                    right = right.Replace("@" + mattr.Name, " parseFloat( document.forms[0]." + this.Pub1.GetTextBoxByID(tbID).ClientID + ".value.replace( ',' ,  '' ) ) ");
                    right = right.Replace("@" + mattr.KeyOfEn, " parseFloat( document.forms[0]." + this.Pub1.GetTextBoxByID(tbID).ClientID + ".value.replace( ',' ,  '' ) ) ");
                }
                string s = left + right;
                s += "\t\n  document.forms[0]." + this.Pub1.GetTextBoxByID("TB_" + ext.AttrOfOper + "_" + pk).ClientID + ".value= VirtyMoney(document.forms[0]." + this.Pub1.GetTextBoxByID("TB_" + ext.AttrOfOper + "_" + pk).ClientID + ".value ) ;";
                return s += " C" + ext.AttrOfOper + "();";
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
                return "";
            }
        }
        public string GenerSum(MapAttr mattr, MapDtl dtl)
        {
            if (dtl.IsShowSum == false)
                return "";

            if (mattr.MyDataType == DataType.AppBoolean)
                return "";

            string left = "\n  document.forms[0]." + this.Pub1.GetTextBoxByID("TB_" + mattr.KeyOfEn).ClientID + ".value = ";
            string right = "";
            for (int i = 1; i <= dtl.RowsOfList; i++)
            {
                string tbID = "TB_" + mattr.KeyOfEn + "_" + i;
                TextBox tb = this.Pub1.GetTextBoxByID(tbID);
                if (tb == null)
                    continue;

                if (i == 0)
                    right += " parseFloat( document.forms[0]." + tb.ClientID + ".value.replace( ',' ,  '' ) )  ";
                else
                    right += " +parseFloat( document.forms[0]." + tb.ClientID + ".value.replace( ',' ,  '' ) )  ";
            }

            string s = left + right + " ;";
            switch (mattr.MyDataType)
            {
                case BP.DA.DataType.AppMoney:
                case BP.DA.DataType.AppRate:
                    return s += "\t\n  document.forms[0]." + this.Pub1.GetTextBoxByID("TB_" + mattr.KeyOfEn).ClientID + ".value= VirtyMoney(document.forms[0]." + this.Pub1.GetTextBoxByID("TB_" + mattr.KeyOfEn).ClientID + ".value ) ;";
                default:
                    return s;
            }
        }
    }

}