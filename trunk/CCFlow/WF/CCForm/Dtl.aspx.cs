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
using BP.Web;
using BP.DA;
using BP.En;
using BP.WF.XML;
using BP.Sys;
using BP.Web.Controls;
namespace CCFlow.WF.CCForm
{
    public partial class Comm_Dtl : BP.Web.WebPage
    {
        #region  Property 
        public int FK_Node
        {
            get
            {
                if (string.IsNullOrEmpty(this.Request.QueryString["FK_Node"]))
                    return 0;

                return int.Parse(this.Request.QueryString["FK_Node"]);
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
        public new string EnsName
        {
            get
            {
                string str = this.Request.QueryString["EnsName"];
                if (str == null)
                    return "ND299Dtl";
                return str;
            }
        }
        /// <summary>
        ///  Main table FK_MapData
        /// </summary>
        public string MainEnsName
        {
            get
            {
                string str = this.Request.QueryString["MainEnsName"];
                if (str == null)
                    return "ND299";
                return str;
            }
        }
        public int BlankNum
        {
            get
            {
                try
                {
                    return int.Parse(ViewState["BlankNum"].ToString());
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState["BlankNum"] = value;
            }
        }
        public new string RefPK
        {
            get
            {
                string str = this.Request.QueryString["RefPK"];
                return str;
            }
        }
        public string RefPKVal
        {
            get
            {
                string str = this.Request.QueryString["RefPKVal"];
                if (str == null)
                    return "1";
                return str;
            }
        }
        public Int64 FID
        {
            get
            {
                string str = this.Request.QueryString["FID"];
                if (str == null)
                    return 0;
                return Int64.Parse(str);
            }
        }
        /// <summary>
        ///  Schedule number .
        /// </summary>
        public int DtlCount
        {
            get
            {
                return int.Parse(ViewState["DtlCount"].ToString());
            }
            set
            {
                ViewState["DtlCount"] = value;
            }
        }
        /// <summary>
        ///  Read-only 
        /// </summary>
        public int IsReadonly
        {
            get
            {
                try
                {
                    return int.Parse(this.Request.QueryString["IsReadonly"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        ///  Increase the number of columns .
        /// </summary>
        public int addRowNum
        {
            get
            {
                try
                {
                    int i = int.Parse(this.Request.QueryString["addRowNum"]);
                    if (this.Request.QueryString["IsCut"] == null)
                        return i;
                    else
                        return i;
                }
                catch
                {
                    return 0;
                }
            }
        }

        public int _allRowCount = 0;

        public int allRowCount
        {
            get
            {
                int i = 0;
                try
                {
                    i = int.Parse(this.Request.QueryString["rowCount"]);
                }
                catch
                {
                    return 0;
                }
                return i;
            }


        }





        public int IsWap
        {
            get
            {
                if (this.Request.QueryString["IsWap"] == "1")
                    return 1;
                return 0;
            }
        }
        public bool IsEnable_del
        {
            get
            {
                string s = this.ViewState["R"] as string;
                if (s == null || s == "0")
                    return false;
                return true;
            }
            set
            {
                if (value)
                    this.ViewState["R"] = "1";
                else
                    this.ViewState["R"] = "0";
            }
        }
        public GEEntity _MainEn = null;
        public GEEntity MainEn
        {
            get
            {
                if (_MainEn == null)
                    _MainEn = new GEEntity(this.FK_MapData, this.RefPKVal);
                return _MainEn;
            }
        }
        public MapAttrs _MainMapAttrs = null;
        public MapAttrs MainMapAttrs
        {
            get
            {
                if (_MainMapAttrs == null)
                    _MainMapAttrs = new MapAttrs(this.FK_MapData);
                return _MainMapAttrs;
            }
        }
        public string FK_MapData = null;
        #endregion  Property .

        protected void Page_Load(object sender, EventArgs e)
        {
            MapDtl mdtl = new MapDtl(this.EnsName);
            if (mdtl.DtlModel == DtlModel.FixRow)
            {
                this.Response.Redirect("DtlFixRow.aspx?1=2" + this.RequestParas, true);
                return;
            }
            if (mdtl.HisDtlShowModel == DtlShowModel.Card)
            {
                this.Response.Redirect("DtlCard.aspx?EnsName=" + this.EnsName + "&RefPKVal=" + this.RefPKVal + "&IsWap=" + this.IsWap + "&FK_Node=" + this.FK_Node + "&MainEnsName=" + this.MainEnsName, true);
                return;
            }

            //this.Page.RegisterClientScriptBlock("s",
            // "<link href='" + BP.WF.Glo.CCFlowAppPath + "WF/Comm/Style/Table" + BP.Web.WebUser.Style + ".css' rel='stylesheet' type='text/css' />");

            if (this.IsReadonly == 1)
            {
                mdtl._IsReadonly = 1;
                this.Button1.Enabled = false;
            }

            this.Bind(mdtl);
        }

        public void Bind(MapDtl mdtl)
        {
            if (this.Request.QueryString["IsTest"] != null)
                BP.DA.Cash.SetMap(this.EnsName, null);

            GEDtls dtls = new GEDtls(this.EnsName);
            this.FK_MapData = mdtl.FK_MapData;

            GEEntity mainEn = null;

            #region  Generation title 
            MapAttrs attrs = new MapAttrs(this.EnsName);
            MapAttrs attrs2 = new MapAttrs();
            int numOfCol = 0;

            float dtlWidth = mdtl.W - 20;
            this.Pub1.Add("<Table class='dtl' border=0  style='width:" + dtlWidth + "px'>");
            this.Pub1.Add(mdtl.MTR);
            if (mdtl.IsShowTitle)
            {
                this.Pub1.AddTR();
                if (this.IsWap == 1)
                {
                    BP.WF.Node nd = new BP.WF.Node(this.FK_Node);
                    string url = "../WAP/MyFlow.aspx?WorkID=" + this.RefPKVal + "&FK_Node=" + this.FK_Node + "&FK_Flow=" + nd.FK_Flow;
                    this.Pub1.AddTD("<img onclick=\"javascript:SaveDtlDataTo('" + url + "');\" src='../Wap/Img/Back.png' style='width:50px;height:16px' border=0/>");
                }
                else
                {
                    this.Pub1.Add("<TD class='Idx' ><img src='../Img/Btn/Table.gif' onclick=\"return DtlOpt('" + this.RefPKVal + "','" + this.EnsName + "');\" border=0/></TD>");
                    numOfCol++;
                }

                foreach (MapAttr attr in attrs)
                {
                    if (attr.UIVisible == false)
                        continue;

                    if (attr.IsPK)
                        continue;

                    // If you enable the grouping , And is currently the group field .
                    if (mdtl.IsEnableGroupField && mdtl.GroupField == attr.KeyOfEn)
                        continue;

                    //for lijian  Increased  @ Symbol is a newline . 
                    this.Pub1.AddTDTitleExt(attr.Name.Replace("@", "<br>"));// ("<TD class='FDesc' nowarp=true ><label>" + attr.Name + "</label></TD>");
                    numOfCol++;
                }

                if (mdtl.IsEnableAthM)
                {
                    this.Pub1.AddTDTitleExt("");
                    numOfCol++;
                }

                if (mdtl.IsEnableM2M)
                {
                    this.Pub1.AddTDTitleExt("");
                    numOfCol++;
                }

                if (mdtl.IsEnableM2MM)
                {
                    this.Pub1.AddTDTitleExt("");
                    numOfCol++;
                }

                if (mdtl.IsDelete && this.IsReadonly == 0)
                {
                    this.Pub1.Add("<TD class='TitleExt' nowarp=true ><img src='../Img/Btn/Save.gif' border=0 onclick='SaveDtlData();' ></TD>");
                    numOfCol++;
                }

                if (mdtl.IsEnableLink)
                {
                    this.Pub1.AddTDTitleExt("");
                    numOfCol++;
                }

                this.Pub1.AddTREnd();
            }
            #endregion  Generation title 

            QueryObject qo = null;
            try
            {
                qo = new QueryObject(dtls);
                switch (mdtl.DtlOpenType)
                {
                    case DtlOpenType.ForEmp:  //  By staff to control .
                        qo.AddWhere(GEDtlAttr.RefPK, this.RefPKVal);
                        qo.addAnd();
                        qo.AddWhere(GEDtlAttr.Rec, WebUser.No);
                        break;
                    case DtlOpenType.ForWorkID: //  By Job ID To control 
                        qo.AddWhere(GEDtlAttr.RefPK, this.RefPKVal);
                        break;
                    case DtlOpenType.ForFID: //  By the process ID To control .
                        qo.AddWhere(GEDtlAttr.FID, this.RefPKVal);
                        break;
                }
            }
            catch
            {
                dtls.GetNewEntity.CheckPhysicsTable();
            }

            #region  Generate page 
            if (mdtl.IsEnableGroupField == true || mdtl.HisWhenOverSize == WhenOverSize.None || mdtl.HisWhenOverSize == WhenOverSize.AddRow)
            {
                /* In case   Are grouped display mode  .*/
                try
                {
                    int num = qo.DoQuery();
                    if (allRowCount == 0)
                    {
                        if (mdtl.RowsOfList >= num)
                        {
                            mdtl.RowsOfList = mdtl.RowsOfList;
                            _allRowCount = mdtl.RowsOfList;
                        }
                        else
                        {
                            mdtl.RowsOfList = num;
                            _allRowCount = num;

                        }
                    }
                    else
                    {
                        mdtl.RowsOfList = allRowCount;
                        _allRowCount = allRowCount;
                    }


                    if (this.IsReadonly == 0)
                    {
                        int dtlCount = dtls.Count;
                        for (int i = 0; i < mdtl.RowsOfList - dtlCount; i++)
                        {
                            BP.Sys.GEDtl dt = new GEDtl(this.EnsName);
                            dt.ResetDefaultVal();
                            dt.SetValByKey(GEDtlAttr.RefPK, this.RefPKVal);
                            dt.OID = i;
                            dtls.AddEntity(dt);
                        }

                        //if (num == mdtl.RowsOfList)
                        //{
                        //    BP.Sys.GEDtl dt1 = new GEDtl(this.EnsName);
                        //    dt1.ResetDefaultVal();
                        //    dt1.SetValByKey(GEDtlAttr.RefPK, this.RefPKVal);
                        //    dt1.OID = mdtl.RowsOfList + 1;
                        //    dtls.AddEntity(dt1);
                        //}
                    }
                }
                catch
                {
                    dtls.GetNewEntity.CheckPhysicsTable();
                }
            }
            else
            {
                /* If not grouped display mode  .*/
                this.Pub2.Clear();
                try
                {


                    int count = qo.GetCount();
                    if (allRowCount == 0)
                    {
                        if (mdtl.RowsOfList >= count)
                        {
                            mdtl.RowsOfList = mdtl.RowsOfList;
                            _allRowCount = mdtl.RowsOfList;
                        }
                        else
                        {
                            mdtl.RowsOfList = count;
                            _allRowCount = count;

                        }
                    }
                    else
                    {
                        mdtl.RowsOfList = allRowCount;
                        _allRowCount = allRowCount;
                    }

                    this.DtlCount = count;
                    this.Pub2.Clear();
                    this.Pub2.BindPageIdx(count, mdtl.RowsOfList, this.PageIdx, "Dtl.aspx?EnsName=" + this.EnsName + "&RefPKVal=" + this.RefPKVal + "&IsWap=" + this.IsWap + "&IsReadonly=" + this.IsReadonly + "&MainEnsName=" + this.MainEnsName);
                    int num = qo.DoQuery("OID", mdtl.RowsOfList, this.PageIdx, false);

                    if (mdtl.IsInsert && this.IsReadonly == 0)
                    {
                        int dtlCount = dtls.Count;
                        for (int i = 0; i < mdtl.RowsOfList - dtlCount; i++)
                        {
                            BP.Sys.GEDtl dt = new GEDtl(this.EnsName);
                            dt.ResetDefaultVal();
                            dt.SetValByKey(GEDtlAttr.RefPK, this.RefPKVal);
                            dt.OID = i;
                            dtls.AddEntity(dt);
                        }

                        //if (num == mdtl.RowsOfList)
                        //{
                        //    BP.Sys.GEDtl dt1 = new GEDtl(this.EnsName);
                        //    dt1.ResetDefaultVal();
                        //    dt1.SetValByKey(GEDtlAttr.RefPK, this.RefPKVal);
                        //    dt1.OID = mdtl.RowsOfList + 1;
                        //    dtls.AddEntity(dt1);
                        //}
                    }
                }
                catch
                {
                    dtls.GetNewEntity.CheckPhysicsTable();
                }

            }
            #endregion  Generate page 

            DDL ddl = new DDL();
            CheckBox cb = new CheckBox();

            //  Line Lock .
            bool isRowLock = mdtl.IsRowLock;

            #region  Generate data 
            int idx = 1;
            string ids = ",";
            int dtlsNum = dtls.Count;
            MapExts mes = new MapExts(this.EnsName);

            //  Need to automatically populate a drop-down box IDs.  The drop-down box does not need to automatically populate data .
            string autoFullDataDDLIDs = ",";
            string LinkFields = ",";
            foreach (MapExt me in mes)
            {
                switch (me.ExtType)
                {
                    case MapExtXmlList.ActiveDDL:
                        autoFullDataDDLIDs += me.AttrsOfActive + ",";
                        break;
                    case MapExtXmlList.AutoFullDLL:
                        autoFullDataDDLIDs += me.AttrOfOper + ",";
                        break;
                    case MapExtXmlList.Link:
                        LinkFields += me.AttrOfOper + ",";
                        break;
                    default:
                        break;
                }
            }

            if (mdtl.IsEnableGroupField)
            {
                /* If a packet display mode ,  Special treatment is necessary to display .
                 1,  Determined set of packets .
                 */

                string gField = mdtl.GroupField;
                MapAttr attrG = attrs.GetEntityByKey(MapAttrAttr.KeyOfEn, gField) as MapAttr;
                if (attrG == null)
                {
                    this.Pub1.Clear();
                    this.Pub1.AddFieldSetRed("err",
                        " Schedule design errors , Grouping field does not exist schedule , Please contact the administrator to resolve this issue .");
                    return;
                }

                if (attrG.UIContralType == UIContralType.DDL)
                {
                    gField = gField + "Text";
                }

                // Determined set of packets .
                string tmp = "";
                foreach (BP.Sys.GEDtl dtl in dtls)
                {
                    if (tmp.Contains("," + dtl.GetValStrByKey(gField) + ",") == false)
                        tmp += "," + dtl.GetValStrByKey(gField);
                }
                string[] strs = tmp.Split(',');

                string groupStr = "";
                //  Traversal - Grouping set .
                foreach (string str in strs)
                {
                    if (string.IsNullOrEmpty(str))
                        continue;

                    #region  Increase grouping row .

                    this.Pub1.AddTR();
                    if (attrG.UIContralType == UIContralType.CheckBok)
                    {
                        if (str == "0")
                            this.Pub1.AddTD("colspan=" + numOfCol, attrG.Name + ":是");
                        else
                            this.Pub1.AddTD("colspan=" + numOfCol, attrG.Name + ":否");
                    }
                    else
                    {
                        if (!groupStr.Contains(str + ","))
                        {
                            this.Pub1.AddTD("colspan=" + numOfCol, str);
                            groupStr += str + ",";
                        }
                    }
                    this.Pub1.AddTREnd();

                    #endregion  Increase grouping row .

                    #region  Increasing the packet data .

                    foreach (BP.Sys.GEDtl dtl in dtls)
                    {
                        if (dtl.GetValStrByKey(gField) != str)
                            continue;

                        #region  Deal with  IDX AddTR

                        if (ids.Contains("," + dtl.OID + ","))
                            continue;
                        ids += dtl.OID + ",";
                        this.Pub1.AddTR(" class='dtlrow' oid='" + dtl.OID + "'");
                        if (mdtl.IsShowIdx)
                        {
                            this.Pub1.AddTDIdx(idx++);
                        }
                        else
                            this.Pub1.AddTD();

                        #endregion  Deal with 

                        #region  Increase rows

                        foreach (MapAttr attr in attrs)
                        {
                            if (attr.UIVisible == false
                                || attr.KeyOfEn == "OID"
                                || attr.KeyOfEn == attrG.KeyOfEn)
                                continue;

                            //// Handle its default value .
                            //if (attr.DefValReal.Contains("@") == true && attr.UIIsEnable == false)
                            //    dtl.SetValByKey(attr.KeyOfEn, attr.DefVal);

                            string val = dtl.GetValByKey(attr.KeyOfEn).ToString();
                            if (attr.UIIsEnable == false && dtl.OID >= 100 &&
                                LinkFields.Contains("," + attr.KeyOfEn + ","))
                            {
                                if (string.IsNullOrEmpty(val))
                                    val = "...";
                                MapExt meLink = mes.GetEntityByKey(MapExtAttr.ExtType, MapExtXmlList.Link,
                                    MapExtAttr.AttrOfOper, attr.KeyOfEn) as MapExt;

                                string url = meLink.Tag.Clone() as string;
                                if (url.Contains("?") == false)
                                    url = url + "?a3=2";

                                url = url + "&WebUserNo=" + WebUser.No + "&SID=" + WebUser.SID + "&EnName=" + mdtl.No +
                                      "&OID=" + dtl.OID;
                                if (url.Contains("@AppPath"))
                                    url = url.Replace("@AppPath",
                                        "http://" + this.Request.Url.Host + this.Request.ApplicationPath);

                                if (url.Contains("@"))
                                {
                                    if (attrs2.Count == 0)
                                        attrs2 = new MapAttrs(mdtl.No);
                                    foreach (MapAttr item in attrs2)
                                    {
                                        url = url.Replace("@" + item.KeyOfEn, dtl.GetValStrByKey(item.KeyOfEn));
                                        if (url.Contains("@") == false)
                                            break;
                                    }
                                    if (url.Contains("@"))
                                    {
                                        /* There also may be a main table parameters */
                                        if (mainEn == null)
                                            mainEn = this.MainEn;
                                        foreach (Attr attrM in mainEn.EnMap.Attrs)
                                        {
                                            url = url.Replace("@" + attrM.Key, mainEn.GetValStrByKey(attrM.Key));
                                            if (url.Contains("@") == false)
                                                break;
                                        }
                                    }
                                }
                                this.Pub1.AddTD("<a href='" + url + "' target='" + meLink.Tag1 + "' >" + val + "</a>");
                                continue;
                            }

                            switch (attr.UIContralType)
                            {
                                case UIContralType.TB:
                                    TextBox tb = new TextBox();
                                    tb.ID = "TB_" + attr.KeyOfEn + "_" + dtl.OID;
                                    tb.Enabled = attr.UIIsEnable;
                                    if (attr.UIIsEnable == false)
                                    {
                                        tb.Attributes.Add("readonly", "true");
                                        tb.CssClass = "TBReadonly";
                                    }
                                    else
                                    {
                                        tb.Attributes["onfocus"] = "isChange=true;";
                                    }
                                    switch (attr.MyDataType)
                                    {
                                        case DataType.AppString:
                                            tb.Attributes["style"] = "width:" + attr.UIWidth + "px;border-width:0px;";
                                            this.Pub1.AddTD("width='2px'", tb);
                                            tb.Text = val;
                                            if (attr.UIIsEnable == false)
                                            {
                                                tb.Attributes.Add("readonly", "true");
                                                tb.CssClass = "TBReadonly";
                                            }

                                            if (attr.UIHeight > 25)
                                            {
                                                tb.TextMode = TextBoxMode.MultiLine;
                                                tb.Attributes["Height"] = attr.UIHeight + "px";
                                                tb.Rows = attr.UIHeightInt / 25;
                                            }
                                            break;
                                        case DataType.AppDate:
                                            float dateWidth = attr.UIWidth;
                                            tb.Attributes["style"] = "width:" + dateWidth + "px;border-width:0px;";
                                            if (val != "0")
                                                tb.Text = val;

                                            if (attr.UIIsEnable)
                                            {
                                                tb.Attributes["onfocus"] = "WdatePicker();isChange=false;";
                                                tb.Attributes["onChange"] = "isChange=true;";
                                                tb.Attributes["class"] = "Wdate";
                                                //tb.CssClass = "easyui-datebox";
                                                //tb.Attributes["data-options"] = "editable:false";

                                            }
                                            else
                                                tb.ReadOnly = true;

                                            this.Pub1.AddTD("width='2px'", tb);
                                            break;
                                        case DataType.AppDateTime:

                                            float dateTimeWidth = attr.UIWidth;
                                            tb.Attributes["style"] = "width:" + dateTimeWidth + "px;border-width:0px;";
                                            if (val != "0")
                                                tb.Text = val;
                                            if (attr.UIIsEnable)
                                            {
                                                tb.Attributes["onfocus"] =
                                                    "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'});isChange=false;";
                                                //tb.CssClass = "easyui-datetimebox";
                                                //tb.Attributes["data-options"] = "editable:false";

                                                tb.Attributes["onChange"] = "isChange=true;";
                                                tb.Attributes["class"] = "Wdate";

                                            }
                                            else
                                            {
                                                tb.ReadOnly = true;
                                            }
                                            this.Pub1.AddTD("width='2px'", tb);
                                            break;
                                        case DataType.AppInt:
                                            tb.Attributes["style"] = "width:" + attr.UIWidth + "px;border-width:0px;";
                                            if (attr.UIIsEnable == false)
                                            {
                                                tb.Attributes["class"] = "TBNumReadonly";
                                                tb.ReadOnly = true;
                                            }
                                            try
                                            {
                                                tb.Text = val;
                                            }
                                            catch (Exception ex)
                                            {
                                                this.Alert(ex.Message + " val =" + val);
                                                tb.Text = "0";
                                            }
                                            this.Pub1.AddTD(tb);
                                            break;
                                        case DataType.AppMoney:
                                        case DataType.AppFloat:
                                        case DataType.AppRate:

                                            tb.Attributes["style"] = "width:" + attr.UIWidth + "px;border-width:0px;";
                                            if (attr.UIIsEnable == false)
                                            {
                                                tb.Attributes["class"] = "TBNumReadonly";
                                                tb.ReadOnly = true;
                                            }

                                            try
                                            {
                                                tb.Text = decimal.Parse(val).ToString("0.00");
                                            }
                                            catch (Exception ex)
                                            {
                                                this.Alert(ex.Message + " val =" + val);
                                                tb.Text = "0.00";
                                            }
                                            this.Pub1.AddTD(tb);
                                            break;
                                        default:
                                            tb.Attributes["style"] = "width:" + attr.UIWidth +
                                                                     "px;text-align:right;border-width:0px;";
                                            tb.Text = val;
                                            this.Pub1.AddTD(tb);
                                            break;
                                    }

                                    if (attr.IsNum && attr.LGType == FieldTypeS.Normal)
                                    {
                                        if (tb.Enabled)
                                        {
                                            // OnKeyPress="javascript:return VirtyNum(this);"
                                            if (attr.MyDataType == DataType.AppInt)
                                            {
                                                tb.Attributes["onkeyup"] += @"C" + dtl.OID + "();C" + attr.KeyOfEn +
                                                                            "(); ";
                                                tb.Attributes["OnKeyPress"] +=
                                                    @"javascript:return VirtyNum(this,'int');";

                                                tb.Attributes["onblur"] += @"value=value.replace(/[^-?\d]/g,'');C" +
                                                                           dtl.OID + "();C" + attr.KeyOfEn + "();";
                                            }
                                            else
                                            {
                                                tb.Attributes["onkeyup"] += @"C" + dtl.OID + "();C" + attr.KeyOfEn +
                                                                            "();";
                                                tb.Attributes["OnKeyPress"] +=
                                                    @"javascript:return VirtyNum(this,'float');";

                                                tb.Attributes["onblur"] +=
                                                    @"value=value.replace(/[^-?\d+\.*\d*$]/g,'');C" + dtl.OID + "();C" +
                                                    attr.KeyOfEn + "();";
                                            }

                                            tb.Attributes["style"] = "width:" + attr.UIWidth +
                                                                     "px;text-align:right;border-width:0px;";
                                        }
                                        else
                                        {
                                            tb.Attributes["onpropertychange"] += "C" + attr.KeyOfEn + "();";
                                            tb.Attributes["style"] = "width:" + attr.UIWidth +
                                                                     "px;text-align:right;border-width:0px;";
                                        }
                                    }
                                    break;
                                case UIContralType.DDL:
                                    switch (attr.LGType)
                                    {
                                        case FieldTypeS.Enum:
                                            DDL myddl = new DDL();
                                            myddl.ID = "DDL_" + attr.KeyOfEn + "_" + dtl.OID;
                                            myddl.Attributes["onchange"] = "isChange= true;";
                                            if (attr.UIIsEnable)
                                            {
                                                try
                                                {
                                                    myddl.BindSysEnum(attr.KeyOfEn);
                                                    myddl.SetSelectItem(val);
                                                }
                                                catch (Exception ex)
                                                {
                                                    BP.Sys.PubClass.Alert(ex.Message);
                                                }
                                            }
                                            else
                                            {
                                                myddl.Items.Add(new ListItem(dtl.GetValRefTextByKey(attr.KeyOfEn),
                                                    dtl.GetValStrByKey(attr.KeyOfEn)));
                                            }
                                            myddl.Enabled = attr.UIIsEnable;
                                            this.Pub1.AddTDCenter(myddl);
                                            break;
                                        case FieldTypeS.FK:
                                            DDL ddl1 = new DDL();
                                            ddl1.ID = "DDL_" + attr.KeyOfEn + "_" + dtl.OID;
                                            ddl1.Attributes["onchange"] = "isChange=true;";
                                            ddl1.Attributes["onfocus"] = "isChange=true;";
                                            if (attr.UIIsEnable)
                                            {
                                                //   ddl1.Attributes["onchange"] = "isChange=true;";
                                                EntitiesNoName ens = attr.HisEntitiesNoName;
                                                ens.RetrieveAll();
                                                ddl1.BindEntities(ens);
                                                if (ddl1.SetSelectItem(val) == false)
                                                    ddl1.Items.Insert(0, new ListItem(" Please select ", val));
                                            }
                                            else
                                            {
                                                ddl1.Items.Add(new ListItem(dtl.GetValRefTextByKey(attr.KeyOfEn),
                                                    dtl.GetValStrByKey(attr.KeyOfEn)));
                                            }
                                            ddl1.Enabled = attr.UIIsEnable;
                                            this.Pub1.AddTDCenter(ddl1);
                                            break;
                                        default:
                                            break;
                                    }
                                    break;
                                case UIContralType.CheckBok:
                                    cb = new CheckBox();
                                    cb.ID = "CB_" + attr.KeyOfEn + "_" + dtl.OID;
                                    cb.Text = attr.Name;
                                    if (val == "1")
                                        cb.Checked = true;
                                    else
                                        cb.Checked = false;
                                    //  cb.Attributes["onchecked"] = "alert('ss'); isChange= true; ";
                                    cb.Attributes["onclick"] = "isChange= true;";
                                    this.Pub1.AddTD(cb);
                                    break;
                                default:
                                    break;
                            }
                        }


                        if (mdtl.IsEnableAthM)
                        {
                            if (1==1||dtl.OID >= 100)
                                this.Pub1.AddTD(
                                    "<a href=\"javascript:window.showModalDialog('AttachmentUpload.aspx?IsBTitle=1&PKVal=" +
                                    dtl.OID + "&Ath=AthM&FK_MapData=" + mdtl.No + "&FK_FrmAttachment=" + mdtl.No +
                                    "_AthM','dialogHeight: 500px; dialogWidth: 600px;center: yes; help: no')\"><img src='../Img/AttachmentM.png' border=0 width='16px' /></a>");
                            else
                                this.Pub1.AddTD("");
                        }

                        if (mdtl.IsEnableM2M)
                        {
                            if (dtl.OID >= 100)
                                this.Pub1.AddTD(
                                    "<a href=\"javascript:window.showModalDialog('M2M.aspx?IsOpen=1&NoOfObj=M2M&OID=" +
                                    dtl.OID + "&FK_MapData=" + mdtl.No +
                                    "','m2m','dialogHeight: 500px; dialogWidth: 600px;center: yes; help: no')\"><img src='../Img/M2M.png' border=0 width='16px' /></a>");
                            else
                                this.Pub1.AddTD("");
                        }

                        if (mdtl.IsEnableM2MM)
                        {
                            if (dtl.OID >= 100)
                                this.Pub1.AddTD(
                                    "<a href=\"javascript:window.showModalDialog('M2MM.aspx?IsOpen=1&NoOfObj=M2MM&OID=" +
                                    dtl.OID + "&FK_MapData=" + mdtl.No +
                                    "','m2m','dialogHeight: 500px; dialogWidth: 600px;center: yes; help: no')\"><img src='../Img/M2M.png' border=0 width='16px' /></a>");
                            else
                                this.Pub1.AddTD("");
                        }

                        if (mdtl.IsDelete && this.IsReadonly == 0 && dtl.OID >= 100)
                        {
                            if (isRowLock == true && dtl.IsRowLock == true)
                                this.Pub1.AddTD("<img src='../Img/Btn/Lock.png' class=ICON />"); // If the current record is locked , And started locking settings .
                            else
                                this.Pub1.Add(
                                    "<TD border=0><img src='../Img/Btn/Delete.gif' onclick=\"javascript:Del('" + dtl.OID +
                                    "','" + this.EnsName + "','" + this.RefPKVal + "','" + this.PageIdx + "')\" /></TD>");
                        }
                        else if (mdtl.IsDelete)
                        {
                            if (this.IsReadonly == 0)
                                this.Pub1.Add("<TD class=TD border=0>&nbsp;</TD>");

                        }
                        this.Pub1.AddTREnd();

                        #endregion  Increase rows
                    }

                    #endregion  Increasing the packet data .
                }
            }
            else
            {
                foreach (BP.Sys.GEDtl dtl in dtls)
                {
                    #region  Deal with 

                    if (ids.Contains("," + dtl.OID + ","))
                        continue;

                    ids += dtl.OID + ",";
                    this.Pub1.AddTR(" class='dtlrow' oid='" + dtl.OID + "'");

                    //if (dtlsNum == idx && mdtl.IsShowIdx && mdtl.IsInsert && this.IsReadonly == 0)
                    //{
                    //    DDL myAdd = new DDL();
                    //    myAdd.AutoPostBack = true;
                    //    myAdd.Items.Add(new ListItem("+", "+"));
                    //    for (int i = 1; i < 10; i++)
                    //    {
                    //        myAdd.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    //    }
                    //    myAdd.SelectedIndexChanged += new EventHandler(myAdd_SelectedIndexChanged);
                    //    this.Pub1.AddTD(myAdd);
                    //}
                    //else
                    //{
                    if (mdtl.IsShowIdx)
                    {
                        this.Pub1.AddTDIdx(idx++);
                    }
                    else
                        this.Pub1.AddTD();
                    //}

                    #endregion  Deal with 

                    #region  Increase rows

                    foreach (MapAttr attr in attrs)
                    {
                        if (attr.UIVisible == false || attr.KeyOfEn == "OID")
                            continue;

                        //// Handle its default value .
                        //if (attr.DefValReal.Contains("@") == true && attr.UIIsEnable == false)
                        //    dtl.SetValByKey(attr.KeyOfEn, attr.DefVal);

                        string val = dtl.GetValByKey(attr.KeyOfEn).ToString();
                        if (attr.UIIsEnable == false && dtl.OID >= 100 && LinkFields.Contains("," + attr.KeyOfEn + ","))
                        {
                            if (string.IsNullOrEmpty(val))
                                val = "...";
                            MapExt meLink = mes.GetEntityByKey(MapExtAttr.ExtType, MapExtXmlList.Link,
                                MapExtAttr.AttrOfOper, attr.KeyOfEn) as MapExt;

                            string url = meLink.Tag.Clone() as string;
                            if (url.Contains("?") == false)
                                url = url + "?a3=2";

                            url = url + "&WebUserNo=" + WebUser.No + "&SID=" + WebUser.SID + "&EnName=" + mdtl.No +
                                  "&OID=" + dtl.OID;
                            if (url.Contains("@AppPath"))
                                url = url.Replace("@AppPath",
                                    "http://" + this.Request.Url.Host + this.Request.ApplicationPath);
                            if (url.Contains("@"))
                            {
                                if (attrs2.Count == 0)
                                    attrs2 = new MapAttrs(mdtl.No);
                                foreach (MapAttr item in attrs2)
                                {
                                    url = url.Replace("@" + item.KeyOfEn, dtl.GetValStrByKey(item.KeyOfEn));
                                    if (url.Contains("@") == false)
                                        break;
                                }
                                if (url.Contains("@"))
                                {
                                    /* There also may be a main table parameters */
                                    if (mainEn == null)
                                        mainEn = this.MainEn;
                                    foreach (Attr attrM in mainEn.EnMap.Attrs)
                                    {
                                        url = url.Replace("@" + attrM.Key, mainEn.GetValStrByKey(attrM.Key));
                                        if (url.Contains("@") == false)
                                            break;
                                    }
                                }
                            }
                            this.Pub1.AddTD("<a href='" + url + "' target='" + meLink.Tag1 + "' >" + val + "</a>");
                            continue;
                        }

                        switch (attr.UIContralType)
                        {
                            case UIContralType.TB:
                                TextBox tb = new TextBox();
                                tb.ID = "TB_" + attr.KeyOfEn + "_" + dtl.OID;
                                tb.Enabled = attr.UIIsEnable;
                                if (attr.UIIsEnable == false)
                                {
                                    tb.Attributes.Add("readonly", "true");
                                    tb.CssClass = "TBReadonly";
                                }
                                else
                                {
                                    tb.Attributes["onfocus"] = "isChange=true;";
                                }
                                switch (attr.MyDataType)
                                {
                                    case DataType.AppString:
                                        tb.Attributes["style"] = "width:" + attr.UIWidth + "px;border-width:0px;";
                                        this.Pub1.AddTD("width='2px'", tb);
                                        tb.Text = val;
                                        if (attr.UIIsEnable == false)
                                        {
                                            tb.Attributes.Add("readonly", "true");
                                            tb.CssClass = "TBReadonly";
                                        }

                                        if (attr.UIHeight > 25)
                                        {
                                            tb.TextMode = TextBoxMode.MultiLine;
                                            tb.Attributes["Height"] = attr.UIHeight + "px";
                                            tb.Rows = attr.UIHeightInt / 25;
                                        }
                                        break;
                                    case DataType.AppDate:
                                        float dateWidth = attr.UIWidth;

                                        tb.Attributes["style"] = "width:" + dateWidth + "px;border-width:0px;";
                                        if (val != "0")
                                            tb.Text = val;
                                        tb.Attributes["readonly"] = "readonly";
                                        if (attr.UIIsEnable)
                                        {
                                            tb.Attributes["onfocus"] = "WdatePicker();isChange=false;";
                                            tb.Attributes["onChange"] = "isChange=true;";
                                            tb.Attributes["class"] = "Wdate";

                                            //tb.CssClass = "easyui-datebox";
                                            //tb.Attributes["data-options"] = "editable:false";
                                        }
                                        else
                                        {
                                            tb.ReadOnly = true;
                                        }
                                        this.Pub1.AddTD("width='2px'", tb);
                                        break;
                                    case DataType.AppDateTime:
                                        float dateTimeWidth = attr.UIWidth;

                                        tb.Attributes["style"] = "width:" + dateTimeWidth + "px;border-width:0px;";
                                        if (val != "0")
                                            tb.Text = val;
                                        if (attr.UIIsEnable)
                                        {
                                            tb.Attributes["onfocus"] =
                                                "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'});isChange=false;";
                                            //tb.CssClass = "easyui-datetimebox";
                                            //tb.Attributes["data-options"] = "editable:false";
                                            tb.Attributes["onChange"] = "isChange=true;";
                                            tb.Attributes["class"] = "Wdate";




                                        }
                                        else
                                        {
                                            tb.ReadOnly = true;
                                        }
                                        this.Pub1.AddTD("width='2px'", tb);
                                        break;
                                    case DataType.AppInt:
                                        tb.Attributes["style"] = "width:" + attr.UIWidth + "px;border-width:0px;";
                                        if (attr.UIIsEnable == false)
                                        {
                                            tb.Attributes["class"] = "TBNumReadonly";
                                            tb.ReadOnly = true;
                                        }
                                        try
                                        {
                                            tb.Text = val;
                                        }
                                        catch (Exception ex)
                                        {
                                            this.Alert(ex.Message + " val =" + val);
                                            tb.Text = "0";
                                        }
                                        this.Pub1.AddTD(tb);
                                        break;
                                    case DataType.AppMoney:
                                    case DataType.AppFloat:
                                    case DataType.AppRate:
                                        tb.Attributes["style"] = "width:" + attr.UIWidth + "px;border-width:0px;";
                                        if (attr.UIIsEnable == false)
                                        {
                                            tb.Attributes["class"] = "TBNumReadonly";
                                            tb.ReadOnly = true;
                                        }

                                        try
                                        {
                                            tb.Text = decimal.Parse(val).ToString("0.00");
                                        }
                                        catch (Exception ex)
                                        {
                                            this.Alert(ex.Message + " val =" + val);
                                            tb.Text = "0.00";
                                        }
                                        this.Pub1.AddTD(tb);
                                        break;
                                    default:
                                        tb.Attributes["style"] = "width:" + attr.UIWidth +
                                                                 "px;text-align:right;border-width:0px;";
                                        tb.Text = val;
                                        this.Pub1.AddTD(tb);
                                        break;
                                }

                                if (attr.IsNum && attr.LGType == FieldTypeS.Normal)
                                {
                                    if (tb.Enabled)
                                    {
                                        // OnKeyPress="javascript:return VirtyNum(this);"
                                        if (attr.MyDataType == DataType.AppInt)
                                        {
                                            tb.Attributes["onkeyup"] += @"C" + dtl.OID + "();C" + attr.KeyOfEn + "(); ";
                                            tb.Attributes["OnKeyPress"] += @"javascript:return  VirtyNum(this,'int');";
                                            tb.Attributes["onblur"] += @"value=value.replace(/[^-?\d]/g,'');C" + dtl.OID +
                                                                       "();C" + attr.KeyOfEn + "();";
                                        }
                                        else
                                        {
                                            tb.Attributes["onkeyup"] += @"C" + dtl.OID + "();C" + attr.KeyOfEn + "();  ";
                                            tb.Attributes["OnKeyPress"] += @"javascript:return  VirtyNum(this,'float');";

                                            tb.Attributes["onblur"] += @"value=value.replace(/[^-?\d+\.*\d*$]/g,'');C" +
                                                                       dtl.OID + "();C" + attr.KeyOfEn + "();";
                                        }
                                        tb.Attributes["style"] = "width:" + attr.UIWidth +
                                                                 "px;text-align:right;border-width:0px;";
                                    }
                                    else
                                    {
                                        tb.Attributes["onpropertychange"] += "C" + attr.KeyOfEn + "();";
                                        tb.Attributes["style"] = "width:" + attr.UIWidth +
                                                                 "px;text-align:right;border-width:0px;";
                                    }
                                }
                                break;
                            case UIContralType.DDL:
                                switch (attr.LGType)
                                {
                                    case FieldTypeS.Enum:
                                        DDL myddl = new DDL();
                                        myddl.ID = "DDL_" + attr.KeyOfEn + "_" + dtl.OID;
                                        myddl.Attributes["onchange"] = "isChange= true;";
                                        if (attr.UIIsEnable)
                                        {
                                            try
                                            {
                                                myddl.BindSysEnum(attr.KeyOfEn);
                                                myddl.SetSelectItem(val);
                                            }
                                            catch (Exception ex)
                                            {
                                                BP.Sys.PubClass.Alert(ex.Message);
                                            }
                                        }
                                        else
                                        {
                                            myddl.Items.Add(new ListItem(dtl.GetValRefTextByKey(attr.KeyOfEn),
                                                dtl.GetValStrByKey(attr.KeyOfEn)));
                                        }
                                        myddl.Enabled = attr.UIIsEnable;
                                        this.Pub1.AddTDCenter(myddl);
                                        break;
                                    case FieldTypeS.FK:
                                        DDL ddl1 = new DDL();
                                        ddl1.ID = "DDL_" + attr.KeyOfEn + "_" + dtl.OID;
                                        ddl1.Attributes["onchange"] = "isChange=true;";
                                        ddl1.Attributes["onfocus"] = "isChange=true;";
                                        if (attr.UIIsEnable)
                                        {
                                            //   ddl1.Attributes["onchange"] = "isChange=true;";
                                            EntitiesNoName ens = attr.HisEntitiesNoName;
                                            ens.RetrieveAll();
                                            ddl1.BindEntities(ens);
                                            if (ddl1.SetSelectItem(val) == false)
                                                ddl1.Items.Insert(0, new ListItem(" Please select ", val));
                                        }
                                        else
                                        {
                                            ddl1.Items.Add(new ListItem(dtl.GetValRefTextByKey(attr.KeyOfEn),
                                                dtl.GetValStrByKey(attr.KeyOfEn)));
                                        }
                                        ddl1.Enabled = attr.UIIsEnable;
                                        this.Pub1.AddTDCenter(ddl1);
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case UIContralType.CheckBok:
                                cb = new CheckBox();
                                cb.ID = "CB_" + attr.KeyOfEn + "_" + dtl.OID;
                                cb.Text = attr.Name;
                                if (val == "1")
                                    cb.Checked = true;
                                else
                                    cb.Checked = false;
                                //  cb.Attributes["onchecked"] = "alert('ss'); isChange= true; ";
                                cb.Attributes["onclick"] = "isChange= true;";
                                this.Pub1.AddTD(cb);
                                break;
                            default:
                                break;
                        }
                    }


                    if (mdtl.IsEnableAthM)
                    {
                        if (1==1||dtl.OID >= 100)
                            this.Pub1.AddTD(
                                "<a href=\"javascript:window.showModalDialog('AttachmentUpload.aspx?IsBTitle=1&PKVal=" +
                                dtl.OID + "&Ath=AthM&FK_MapData=" + mdtl.No + "&FK_FrmAttachment=" + mdtl.No +
                                "_AthM','dialogHeight: 500px; dialogWidth: 600px;center: yes; help: no')\"><img src='../Img/AttachmentM.png' border=0 width='16px' /></a>");
                        else
                            this.Pub1.AddTD("");
                    }

                    if (mdtl.IsEnableM2M)
                    {
                        if (dtl.OID >= 100)
                            this.Pub1.AddTD(
                                "<a href=\"javascript:window.showModalDialog('M2M.aspx?IsOpen=1&NoOfObj=M2M&OID=" +
                                dtl.OID + "&FK_MapData=" + mdtl.No +
                                "','m2m','dialogHeight: 500px; dialogWidth: 600px;center: yes; help: no')\"><img src='../Img/M2M.png' border=0 width='16px' /></a>");
                        else
                            this.Pub1.AddTD("");
                    }

                    if (mdtl.IsEnableM2MM)
                    {
                        if (dtl.OID >= 100)
                            this.Pub1.AddTD(
                                "<a href=\"javascript:window.showModalDialog('M2MM.aspx?IsOpen=1&NoOfObj=M2MM&OID=" +
                                dtl.OID + "&FK_MapData=" + mdtl.No +
                                "','m2m','dialogHeight: 500px; dialogWidth: 600px;center: yes; help: no')\"><img src='../Img/M2M.png' border=0 width='16px' /></a>");
                        else
                            this.Pub1.AddTD("");
                    }

                    if (mdtl.IsEnableLink)
                    {
                        string url = mdtl.LinkUrl.Clone() as string;
                        if (url.Contains("?") == false)
                            url = url + "?a3=2";
                        url = url.Replace("*", "@");

                        if (url.Contains("OID="))
                            url = url + "&WebUserNo=" + WebUser.No + "&SID=" + WebUser.SID + "&EnName=" + mdtl.No;
                        else
                            url = url + "&WebUserNo=" + WebUser.No + "&SID=" + WebUser.SID + "&EnName=" + mdtl.No +
                                  "&OID=" + dtl.OID;

                        if (url.Contains("@AppPath"))
                            url = url.Replace("@AppPath",
                                "http://" + this.Request.Url.Host + this.Request.ApplicationPath);

                        url = BP.WF.Glo.DealExp(url, dtl, null);
                        url = url.Replace("@OID", dtl.OID.ToString());
                        url = url.Replace("@FK_Node", this.FK_Node.ToString());
                        url = url.Replace("'", "");

                        if (dtl.OID >= 100)
                            this.Pub1.AddTD("<a href=\"" + url + "\" target='" + mdtl.LinkTarget + "' >" +
                                            mdtl.LinkLabel + "</a>");
                        else
                            this.Pub1.AddTD("");
                    }

                    if (mdtl.IsDelete && this.IsReadonly == 0 && dtl.OID >= 100)
                    {
                        if (isRowLock == true && dtl.IsRowLock == true)
                            this.Pub1.AddTD("<img src='../Img/Btn/Lock.png' class=ICON />"); // If the current record is locked , And started locking settings .
                        else
                            this.Pub1.Add("<TD border=0><img src='../Img/Btn/Delete.gif' onclick=\"javascript:Del('" +
                                          dtl.OID + "','" + this.EnsName + "','" + this.RefPKVal + "','" + this.PageIdx +
                                          "')\" /></TD>");
                    }
                    else if (mdtl.IsDelete)
                    {
                        if (this.IsReadonly == 0)
                            this.Pub1.Add("<TD class=TD border=0>&nbsp;</TD>");
                    }
                    this.Pub1.AddTREnd();

                    #endregion  Increase rows
                }
                if (mdtl.IsInsert && this.IsReadonly == 0)
                {
                    this.Pub1.AddTR();
                    DDL myAdd = new DDL();
                    myAdd.AutoPostBack = true;
                    myAdd.Items.Add(new ListItem("+", "+"));
                    for (int i = 1; i < 10; i++)
                    {
                        myAdd.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    }
                    myAdd.SelectedIndexChanged += new EventHandler(myAdd_SelectedIndexChanged);
                    this.Pub1.AddTD(myAdd);
                    foreach (MapAttr attr in attrs)
                    {
                        if (attr.UIVisible == false || attr.KeyOfEn == "OID")
                            continue;

                        //// Handle its default value .
                        //if (attr.DefValReal.Contains("@") == true && attr.UIIsEnable == false)
                        //    dtl.SetValByKey(attr.KeyOfEn, attr.DefVal);
                        this.Pub1.AddTD("");
                    }

                    if (mdtl.IsDelete)
                    {
                        if (this.IsReadonly == 0)
                            this.Pub1.Add("<TD class=TD border=0>&nbsp;</TD>");
                    }
                    this.Pub1.AddTREnd();

                }
            }

            #region  Expand property 
            if (this.IsReadonly == 0 && mes.Count != 0)
            {
                this.Page.RegisterClientScriptBlock("s81",
              "<script language='JavaScript' src='../Scripts/jquery-1.4.1.min.js' ></script>");

                this.Page.RegisterClientScriptBlock("b81",
             "<script language='JavaScript' src='MapExt.js' defer='defer' type='text/javascript' ></script>");

                this.Pub1.Add("<div id='divinfo' style='width: 155px; position: absolute; color: Lime; display: none;cursor: pointer;align:left'></div>");

                this.Page.RegisterClientScriptBlock("dCd",
    "<script language='JavaScript' src='/DataUser/JSLibData/" + mdtl.No + ".js' ></script>");

                foreach (BP.Sys.GEDtl mydtl in dtls)
                {
                    //ddl.ID = "DDL_" + attr.KeyOfEn + "_" + dtl.OID;
                    foreach (MapExt me in mes)
                    {
                        switch (me.ExtType)
                        {
                            case MapExtXmlList.DDLFullCtrl: //  Automatic filling .
                                DDL ddlOper = this.Pub1.GetDDLByID("DDL_" + me.AttrOfOper + "_" + mydtl.OID);
                                if (ddlOper == null)
                                    continue;
                                ddlOper.Attributes["onchange"] = "DDLFullCtrl(this.value,\'" + ddlOper.ClientID + "\', \'" + me.MyPK + "\')";
                                break;
                            case MapExtXmlList.ActiveDDL:
                                DDL ddlPerant = this.Pub1.GetDDLByID("DDL_" + me.AttrOfOper + "_" + mydtl.OID);
                                string val, valC;
                                DataTable dt;
                                if (ddlPerant == null)
                                    continue;
#warning  Here you need to optimize 
                                string ddlC = "Pub1_DDL_" + me.AttrsOfActive + "_" + mydtl.OID;
                                ddlPerant.Attributes["onchange"] = " isChange=true; DDLAnsc(this.value, \'" + ddlC + "\', \'" + me.MyPK + "\')";
                                DDL ddlChild = this.Pub1.GetDDLByID("DDL_" + me.AttrsOfActive + "_" + mydtl.OID);
                                val = ddlPerant.SelectedItemStringVal;
                                if (ddlChild.Items.Count == 0)
                                    valC = mydtl.GetValStrByKey(me.AttrsOfActive);
                                else
                                    valC = ddlChild.SelectedItemStringVal;

                                string mysql = me.Doc.Replace("@Key", val);
                                if (mysql.Contains("@") && mydtl.OID >= 100)
                                {
                                    mysql = BP.WF.Glo.DealExp(mysql, mydtl, null);
                                }
                                else
                                {
                                    continue;
                                    mysql = mysql.Replace("@WebUser.No", WebUser.No);
                                    mysql = mysql.Replace("@WebUser.Name", WebUser.Name);
                                    mysql = mysql.Replace("@WebUser.FK_Dept", WebUser.FK_Dept);
                                }

                                dt = DBAccess.RunSQLReturnTable(mysql);

                                ddlChild.Bind(dt, "No", "Name");
                                if (ddlChild.SetSelectItem(valC) == false)
                                {
                                    ddlChild.Items.Insert(0, new ListItem(" Please select " + valC, valC));
                                    ddlChild.SelectedIndex = 0;
                                }
                                ddlChild.Attributes["onchange"] = " isChange=true;";
                                break;
                            case MapExtXmlList.AutoFullDLL: // Automatically populate drop-down box range .
                                DDL ddlFull = this.Pub1.GetDDLByID("DDL_" + me.AttrOfOper + "_" + mydtl.OID);
                                if (ddlFull == null)
                                    continue;

                                string valOld = mydtl.GetValStrByKey(me.AttrOfOper);
                                //string valOld =ddlFull.SelectedItemStringVal;

                                string fullSQL = me.Doc.Replace("@WebUser.No", WebUser.No);
                                fullSQL = fullSQL.Replace("@WebUser.Name", WebUser.Name);
                                fullSQL = fullSQL.Replace("@WebUser.FK_Dept", WebUser.FK_Dept);
                                fullSQL = fullSQL.Replace("@WebUser.FK_DeptName", WebUser.FK_DeptName);
                                fullSQL = fullSQL.Replace("@Key", this.Request.QueryString["Key"]);

                                if (fullSQL.Contains("@"))
                                {
                                    Attrs attrsFull = mydtl.EnMap.Attrs;
                                    foreach (Attr attr in attrsFull)
                                    {
                                        if (fullSQL.Contains("@") == false)
                                            break;
                                        fullSQL = fullSQL.Replace("@" + attr.Key, mydtl.GetValStrByKey(attr.Key));
                                    }
                                }

                                if (fullSQL.Contains("@"))
                                {
                                    /* Fetch data from the main table */
                                    Attrs attrsFull = this.MainEn.EnMap.Attrs;
                                    foreach (Attr attr in attrsFull)
                                    {
                                        if (fullSQL.Contains("@") == false)
                                            break;

                                        if (fullSQL.Contains("@" + attr.Key) == false)
                                            continue;

                                        fullSQL = fullSQL.Replace("@" + attr.Key, this.MainEn.GetValStrByKey(attr.Key));
                                    }
                                }

                                ddlFull.Items.Clear();
                                ddlFull.Bind(DBAccess.RunSQLReturnTable(fullSQL), "No", "Name");
                                if (ddlFull.SetSelectItem(valOld) == false)
                                {
                                    ddlFull.Items.Insert(0, new ListItem(" Please select " + valOld, valOld));
                                    ddlFull.SelectedIndex = 0;
                                }
                                ddlFull.Attributes["onchange"] = " isChange=true;";
                                break;
                            case MapExtXmlList.TBFullCtrl: //  Automatic filling .
                                TextBox tbAuto = this.Pub1.GetTextBoxByID("TB_" + me.AttrOfOper + "_" + mydtl.OID);
                                if (tbAuto == null)
                                    continue;
                                tbAuto.Attributes["onkeyup"] = " isChange=true; DoAnscToFillDiv(this,this.value,\'" + tbAuto.ClientID + "\', \'" + me.MyPK + "\');";
                                tbAuto.Attributes["AUTOCOMPLETE"] = "OFF";
                                if (me.Tag != "")
                                {
                                    /*  Handle drop-down box to select the range of issues  */
                                    string[] strs = me.Tag.Split('$');
                                    foreach (string str in strs)
                                    {
                                        string[] myCtl = str.Split(':');
                                        string ctlID = myCtl[0];
                                        DDL ddlC1 = this.Pub1.GetDDLByID("DDL_" + ctlID + "_" + mydtl.OID);
                                        if (ddlC1 == null)
                                        {
                                            //me.Tag = "";
                                            // me.Update();
                                            continue;
                                        }

                                        string sql = myCtl[1].Replace("~", "'");
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
                                TextBox tbCheck = this.Pub1.GetTextBoxByID("TB_" + me.AttrOfOper + "_" + mydtl.OID);
                                if (tbCheck != null)
                                    tbCheck.Attributes[me.Tag2] += " rowPK=" + mydtl.OID + "; " + me.Tag1 + "(this);";
                                break;
                            case MapExtXmlList.PopVal: // Pop-up window .
                                TB tb = this.Pub1.GetTBByID("TB_" + me.AttrOfOper + "_" + mydtl.OID);
                                tb.Attributes["ondblclick"] = " isChange=true; ReturnVal(this,'" + me.Doc + "','sd');";
                                break;
                            case MapExtXmlList.Link: //  Hyperlinks .

                                //TB tb = this.Pub1.GetTBByID("TB_" + me.AttrOfOper + "_" + mydtl.OID);
                                //tb.Attributes["ondblclick"] = " isChange=true; ReturnVal(this,'" + me.Doc + "','sd');";
                                break;
                            case MapExtXmlList.RegularExpression:// Regex , Control data processing 
                                TextBox tbExp = this.Pub1.GetTextBoxByID("TB_" + me.AttrOfOper + "_" + mydtl.OID);
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

            #region  Total generation 
            if (mdtl.IsShowSum && dtls.Count > 1)
            {
                this.Pub1.AddTRSum();
                this.Pub1.AddTD();
                foreach (MapAttr attr in attrs)
                {
                    //  Grouping fields or hidden fields are not displayed  [liold 140602]
                    if (attr.UIVisible == false)
                        continue;
                    if (attr.Field == mdtl.GroupField && mdtl.IsEnableGroupField)
                        continue;

                    if (attr.IsNum && attr.LGType == FieldTypeS.Normal)
                    {
                        TextBox tb = new TextBox();
                        tb.ID = "TB_" + attr.KeyOfEn;
                        tb.Text = attr.DefVal;
                        // tb.ShowType = attr.HisTBType;
                        tb.ReadOnly = true;
                        tb.Font.Bold = true;
                        tb.BackColor = System.Drawing.Color.FromName("infobackground");
                        switch (attr.MyDataType)
                        {
                            case DataType.AppRate:
                            case DataType.AppMoney:
                                tb.Text = dtls.GetSumDecimalByKey(attr.KeyOfEn).ToString("0.00");
                                tb.Attributes["style"] = "width:" + attr.UIWidth + "px;text-align:right;border:none";
                                break;
                            case DataType.AppInt:
                                tb.Text = dtls.GetSumIntByKey(attr.KeyOfEn).ToString();
                                tb.Attributes["style"] = "width:" + attr.UIWidth + "px;text-align:right;border:none";
                                break;
                            case DataType.AppFloat:
                                tb.Text = dtls.GetSumFloatByKey(attr.KeyOfEn).ToString("F");
                                tb.Attributes["style"] = "width:" + attr.UIWidth + "px;text-align:right;border:none";
                                break;
                            default:
                                break;
                        }
                        this.Pub1.AddTD("align=right", tb);
                    }
                    else
                    {
                        this.Pub1.AddTD();
                    }
                }
                if (mdtl.IsEnableAthM)
                    this.Pub1.AddTD();

                if (mdtl.IsEnableM2M)
                    this.Pub1.AddTD();

                if (mdtl.IsEnableM2MM)
                    this.Pub1.AddTD();

                if (mdtl.IsReadonly == false)
                    this.Pub1.AddTD();

                this.Pub1.AddTREnd();
            }
            #endregion  Total generation 

            #endregion  Generate data 

            this.Pub1.AddTableEnd();

            #region  Generate   Automatic calculation of line 
            if (this.IsReadonly == 0)
            {
                //  Automatic calculation formula output 
                this.Response.Write("\n<script language='JavaScript'>");
                MapExts exts = new MapExts(mdtl.No);
                foreach (GEDtl dtl in dtls)
                {
                    string top = "\n function C" + dtl.OID + "() { \n ";
                    string script = "";
                    string end = " \n  } ";
                    // Add Function 
                    if (exts == null || exts.Count == 0)
                    {
                        this.Response.Write(top + script + end);
                        continue;
                    }
                    foreach (MapExt ext in exts)
                    {
                        if (ext.ExtType != MapExtXmlList.AutoFull)
                        {
                            this.Response.Write(top + script + end);
                            continue;
                        }

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
                                script += this.GenerAutoFull(dtl.OID.ToString(), attrs, ext);
                            }
                        }
                        this.Response.Write(top + script + end);
                    }
                }
                this.Response.Write("\n</script>");

                //  Total output calculation formula 
                foreach (MapAttr attr in attrs)
                {
                    if (attr.UIVisible == false)
                        continue;

                    if (attr.LGType != FieldTypeS.Normal)
                        continue;

                    if (attr.IsNum == false)
                        continue;

                    string top = "\n<script language='JavaScript'> function C" + attr.KeyOfEn + "() { \n ";
                    string end = "\n  isChange =true ;  } </script>";
                    this.Response.Write(top + this.GenerSum(attr, dtls) + " ; \t\n" + end);
                }
            }
            #endregion
        }
        bool isAddDDLSelectIdxChange = false;
        void myAdd_SelectedIndexChanged(object sender, EventArgs e)
        {
            DDL ddl = sender as DDL;
            string val = ddl.SelectedItemStringVal;
            string url = "";
            isAddDDLSelectIdxChange = true;
            this.Save();
            try
            {
                int addRow = int.Parse(ddl.SelectedItemStringVal.Replace("+", "").Replace("-", ""));
                _allRowCount += addRow;
            }
            catch
            {

            }

            if (val.Contains("+"))
                url = "Dtl.aspx?EnsName=" + this.EnsName + "&RefPKVal=" + this.RefPKVal + "&PageIdx=" + this.PageIdx + "&rowCount=" + _allRowCount + "&AddRowNum=" + ddl.SelectedItemStringVal.Replace("+", "").Replace("-", "") + "&IsCut=0&IsWap=" + this.IsWap + "&FK_Node=" + this.FK_Node + "&Key=" + this.Request.QueryString["Key"];
            else
                url = "Dtl.aspx?EnsName=" + this.EnsName + "&RefPKVal=" + this.RefPKVal + "&PageIdx=" + this.PageIdx + "&rowCount=" + _allRowCount + "&AddRowNum=" + ddl.SelectedItemStringVal.Replace("+", "").Replace("-", "") + "&IsWap=" + this.IsWap + "&FK_Node=" + this.FK_Node + "&Key=" + this.Request.QueryString["Key"];

            this.Response.Redirect(url, true);
        }
        public void Save()
        {
            MapDtl mdtl = new MapDtl(this.EnsName);
            GEDtls dtls = new GEDtls(this.EnsName);
            FrmEvents fes = new FrmEvents(this.EnsName); // Get events .
            GEEntity mainEn = mdtl.GenerGEMainEntity(this.RefPKVal);

            #region  Handling Events from the table before saving .
            if (fes.Count > 0)
            {
                try
                {
                    string msg = fes.DoEventNode(EventListDtlList.DtlSaveEnd, mainEn);
                    if (msg != null)
                        this.Alert(msg);
                }
                catch (Exception ex)
                {
                    this.Alert(ex.Message);
                    return;
                }
            }
            #endregion  Handling Events from the table before saving .

            QueryObject qo = new QueryObject(dtls);
            switch (mdtl.DtlOpenType)
            {
                case DtlOpenType.ForEmp:
                    qo.AddWhere(GEDtlAttr.RefPK, this.RefPKVal);
                    qo.addAnd();
                    qo.AddWhere(GEDtlAttr.Rec, WebUser.No);
                    break;
                case DtlOpenType.ForWorkID:
                    qo.AddWhere(GEDtlAttr.RefPK, this.RefPKVal);
                    break;
                case DtlOpenType.ForFID:
                    qo.AddWhere(GEDtlAttr.FID, this.RefPKVal);
                    break;
            }

            int num = qo.DoQuery("OID", mdtl.RowsOfList, this.PageIdx, false);
            int dtlCount = dtls.Count;
            if (allRowCount == 0)
            {
                mdtl.RowsOfList = mdtl.RowsOfList + this.addRowNum;
            }
            else
            {
                mdtl.RowsOfList = allRowCount;
            }
            for (int i = 0; i < mdtl.RowsOfList - dtlCount; i++)
            {
                BP.Sys.GEDtl dt = new GEDtl(this.EnsName);
                dt.ResetDefaultVal();
                dt.OID = i;
                dtls.AddEntity(dt);
            }

            //if (num == mdtl.RowsOfList)
            //{
            //    BP.Sys.GEDtl dt1 = new GEDtl(this.EnsName);
            //    dt1.ResetDefaultVal();
            //    dt1.OID = mdtl.RowsOfList + 1;
            //    dtls.AddEntity(dt1);
            //}


            Map map = dtls.GetNewEntity.EnMap;
            bool isTurnPage = false;
            string err = "";
            int idx = 0;

            //  Determine whether there is an event .
            bool isHaveBefore = false;
            bool isHaveEnd = false;
            FrmEvent fe_Before = fes.GetEntityByKey(FrmEventAttr.FK_Event, EventListDtlList.DtlItemSaveBefore) as FrmEvent;
            if (fe_Before == null)
                isHaveBefore = false;
            else
                isHaveBefore = true;

            FrmEvent fe_End = fes.GetEntityByKey(FrmEventAttr.FK_Event, EventListDtlList.DtlItemSaveAfter) as FrmEvent;
            if (fe_End == null)
                isHaveEnd = false;
            else
                isHaveEnd = true;

            //...................................
            bool isRowLock = mdtl.IsRowLock;
            foreach (GEDtl dtl in dtls)
            {
                idx++;
                try
                {
                    this.Pub1.Copy(dtl, dtl.OID.ToString(), map);

                    // If it is OK to lock , Not executed .
                    if (isRowLock == true && dtl.IsRowLock)
                        continue;

                    if (dtl.OID < mdtl.RowsOfList + 2)
                    {
                        int myOID = dtl.OID;
                        dtl.OID = 0;
                        if (dtl.IsBlank)
                            continue;

                        dtl.OID = myOID;
                        if (dtl.OID == mdtl.RowsOfList + 1)
                            isTurnPage = true;

                        dtl.RefPK = this.RefPKVal;
                        if (this.FID != 0)
                            dtl.FID = this.FID;

                        if (isHaveBefore)
                        {
                            try
                            {
                                string r = fes.DoEventNode(EventListDtlList.DtlItemSaveBefore, dtl);
                                if (r == "false" || r == "0")
                                    continue;
                                err += r;
                            }
                            catch (Exception ex)
                            {
                                err += ex.Message;
                                continue;
                            }
                        }
                        dtl.InsertAsOID(DBAccess.GenerOID("Dtl"));
                    }
                    else
                    {
                        if (this.FID != 0)
                            dtl.FID = this.FID;
                        if (isHaveBefore)
                        {
                            try
                            {
                                err += fes.DoEventNode(EventListDtlList.DtlItemSaveBefore, dtl);
                            }
                            catch (Exception ex)
                            {
                                err += ex.Message;
                                continue;
                            }
                        }
                        dtl.Update();
                    }

                    if (isHaveEnd)
                    {
                        /*  If there is an event after storage .*/
                        try
                        {
                            fes.DoEventNode(EventListDtlList.DtlItemSaveAfter, dtl);
                        }
                        catch (Exception ex)
                        {
                            err += ex.Message;
                        }
                    }
                }
                catch (Exception ex)
                {
                    dtl.CheckPhysicsTable();
                    err += "Row: " + idx + " Error \r\n" + ex.Message;
                }
            }

            if (err != "")
            {
                BP.DA.Log.DefaultLogWriteLineInfo(err);
                this.Alert(err);
                return;
            }

            if (isAddDDLSelectIdxChange == true)
                return;

            #region  After saving process events from the table .
            if (fes.Count > 0)
            {
                try
                {
                    string msg = fes.DoEventNode(EventListDtlList.DtlSaveEnd, mainEn);
                    if (msg != null)
                        this.Alert(msg);
                }
                catch (Exception ex)
                {
                    this.Alert(ex.Message);
                    return;
                }
            }
            #endregion  Handling Events .

            if (isTurnPage)
            {
                int pageNum = 0;
                int count = this.DtlCount + 1;
                decimal pageCountD = decimal.Parse(count.ToString()) / decimal.Parse(mdtl.RowsOfList.ToString()); //  Number of pages .
                string[] strs = pageCountD.ToString("0.0000").Split('.');
                if (int.Parse(strs[1]) > 0)
                    pageNum = int.Parse(strs[0]) + 1;
                else
                    pageNum = int.Parse(strs[0]);
                this.Response.Redirect("Dtl.aspx?EnsName=" + this.EnsName + "&RefPKVal=" + this.RefPKVal + "&PageIdx=" + pageNum + "&IsWap=" + this.IsWap + "&FK_Node=" + this.FK_Node + "&FID=" + this.FID + "&Key=" + this.Key, true);
            }
            else
            {
                this.Response.Redirect("Dtl.aspx?EnsName=" + this.EnsName + "&RefPKVal=" + this.RefPKVal + "&PageIdx=" + this.PageIdx + "&IsWap=" + this.IsWap + "&FK_Node=" + this.FK_Node + "&FID=" + this.FID + "&Key=" + this.Key, true);
            }
        }
        public void ExpExcel()
        {
            BP.Sys.MapDtl mdtl = new MapDtl(this.EnsName);
            this.Title = mdtl.Name;
            GEDtls dtls = new GEDtls(this.EnsName);
            QueryObject qo = new QueryObject(dtls);
            switch (mdtl.DtlOpenType)
            {
                case DtlOpenType.ForEmp:
                    qo.AddWhere(GEDtlAttr.RefPK, this.RefPKVal);
                    //qo.addAnd();
                    //qo.AddWhere(GEDtlAttr.Rec, WebUser.No);
                    break;
                case DtlOpenType.ForWorkID:
                    qo.AddWhere(GEDtlAttr.RefPK, this.RefPKVal);
                    break;
                case DtlOpenType.ForFID:
                    qo.AddWhere(GEDtlAttr.FID, this.RefPKVal);
                    break;
            }
            qo.DoQuery();

            // this.ExportDGToExcelV2(dtls, this.Title + ".xls");
            //DataTable dt = dtls.ToDataTableDesc();
            // this.GenerExcel(dtls.ToDataTableDesc(), mdtl.Name + ".xls");

            this.GenerExcel_pri_Text(dtls.ToDataTableDesc(), mdtl.Name + "@" + WebUser.No + "@" + DataType.CurrentData + ".xls");

            //this.ExportDGToExcelV2(dtls, this.Title + ".xls");
            //dtls.GetNewEntity.CheckPhysicsTable();
            //this.Response.Redirect("Dtl.aspx?EnsName=" + this.EnsName + "&RefPKVal=" + this.RefPKVal, true);
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
                //if (pk == "0")
                //    return null;
                string left = "\n  document.forms[0]." + this.Pub1.GetTextBoxByID("TB_" + ext.AttrOfOper + "_" + pk).ClientID + ".value = ";
                string right = ext.Doc;
                foreach (MapAttr mattr in attrs)
                {
                    //if (mattr.IsNum == false)
                    //    continue;

                    //if (mattr.LGType != FieldTypeS.Normal)
                    //    continue;

                    string tbID = "TB_" + mattr.KeyOfEn + "_" + pk;
                    TextBox tb = this.Pub1.GetTextBoxByID(tbID);
                    if (tb == null)
                        continue;

                    //right = right.Replace("@" + mattr.Name, " parseFloat( document.forms[0]." + tb.ClientID + ".value.replace( ',' ,  '' ) ) ");
                    //right = right.Replace("@" + mattr.KeyOfEn, " parseFloat( document.forms[0]." + tb.ClientID + ".value.replace( ',' ,  '' ) ) ");

                    right = right.Replace("@" + mattr.Name, " parseFloat(replaceAll(document.forms[0]." + tb.ClientID + ".value,',' ,  '' ) ) ");
                    right = right.Replace("@" + mattr.KeyOfEn, " parseFloat( replaceAll(document.forms[0]." + tb.ClientID + ".value, ',' ,  '' ) ) ");
                }
                string s = left + right;
                s += "\t\n  document.forms[0]." + this.Pub1.GetTextBoxByID("TB_" + ext.AttrOfOper + "_" + pk).ClientID + ".value= VirtyMoney(document.forms[0]." + this.Pub1.GetTextBoxByID("TB_" + ext.AttrOfOper + "_" + pk).ClientID + ".value ) ;";
                return s += " C" + ext.AttrOfOper + "();";
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public string GenerSum(MapAttr mattr, GEDtls dtls)
        {
            if (dtls.Count <= 1)
                return "";

            string ClientID = "";
            try
            {
                ClientID = this.Pub1.GetTextBoxByID("TB_" + mattr.KeyOfEn).ClientID;
            }
            catch
            {
                return "";
            }

            string left = "\n  document.forms[0]." + ClientID + ".value = ";
            string right = "";
            int i = 0;
            foreach (GEDtl dtl in dtls)
            {
                string tbID = "TB_" + mattr.KeyOfEn + "_" + dtl.OID;
                TextBox tb = this.Pub1.GetTextBoxByID(tbID);
                if (tb == null)
                    continue;

                if (i == 0)
                    right += " parseVal2Float('" + tb.ClientID + "')";
                else
                    right += " +parseVal2Float('" + tb.ClientID + "')";
                i++;
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
        protected void Button1_Click(object sender, EventArgs e)
        {
            this.Save();
        }
    }

}