using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.En;
using BP;
using BP.Sys;

public partial class WF_MapDef_UC_MExt : BP.Web.UC.UCBase3
{
    #region  Property .
    public string FK_MapData
    {
        get
        {
            return this.Request.QueryString["FK_MapData"];
        }
    }

    public string OperAttrKey
    {
        get
        {
            return this.Request.QueryString["OperAttrKey"];
        }
    }
    public string ExtType
    {
        get
        {
            string s = this.Request.QueryString["ExtType"];
            if (s == "")
                s = null;
            return s;
        }
    }
    public string Lab = null;
    #endregion  Property .

    /// <summary>
    /// BindLeft
    /// </summary>
    public void BindLeft()
    {
        if (this.ExtType == MapExtXmlList.StartFlow)
            return;

        MapExtXmls fss = new MapExtXmls();
        fss.RetrieveAll();
        this.Left.Add("<a href='http://ccflow.org' target=_blank  ><img src='../../DataUser/ICON/" + SystemConfig.CompanyID + "/LogBiger.png' style='width:180px;' /></a><hr>");
        this.Left.AddUL();
        foreach (MapExtXml fs in fss)
        {
            if (this.ExtType == fs.No)
            {
                this.Lab = fs.Name;
                this.Left.AddLiB(fs.URL + "&FK_MapData=" + this.FK_MapData + "&ExtType=" + fs.No + "&RefNo=" + this.RefNo, "<span>" + fs.Name + "</span>");
            }
            else
                this.Left.AddLi(fs.URL + "&FK_MapData=" + this.FK_MapData + "&ExtType=" + fs.No + "&RefNo=" + this.RefNo, "<span>" + fs.Name + "</span>");
        }


        this.Left.AddLi("<a href='MapExt.aspx?FK_MapData=" + this.FK_MapData + "&RefNo=" + this.RefNo + "'><span> Help </span></a>");
        this.Left.AddULEnd();
    }
    public void BindLeftV1()
    {
        this.Left.Add("\t\n<div id='tabsJ'  align='center'>");
        MapExtXmls fss = new MapExtXmls();
        fss.RetrieveAll();

        this.Left.AddUL();
        foreach (MapExtXml fs in fss)
        {
            if (this.ExtType == fs.No)
            {
                this.Lab = fs.Name;
                this.Left.AddLiB(fs.URL + "&FK_MapData=" + this.FK_MapData + "&ExtType=" + fs.No + "&RefNo=" + this.RefNo, "<span>" + fs.Name + "</span>");
            }
            else
                this.Left.AddLi(fs.URL + "&FK_MapData=" + this.FK_MapData + "&ExtType=" + fs.No + "&RefNo=" + this.RefNo, "<span>" + fs.Name + "</span>");
        }
        this.Left.AddLi("<a href='MapExt.aspx?FK_MapData=" + this.FK_MapData + "&RefNo=" + this.RefNo + "'><span> Help </span></a>");
        this.Left.AddULEnd();
        this.Left.AddDivEnd();
    }
    /// <summary>
    ///  New auto-complete textbox 
    /// </summary>
    public void EditAutoFullM2M_TB()
    {
        MapExt myme = new MapExt(this.MyPK);
        MapM2Ms m2ms = new MapM2Ms(myme.FK_MapData);

        this.Pub2.AddH2(" Set automatically populated from the table . <a href='?ExtType=" + this.ExtType + "&MyPK=" + this.MyPK + "&FK_MapData=" + this.FK_MapData + "&RefNo=" + this.RefNo + "'> Return </a>");
        if (m2ms.Count == 0)
        {
            this.Pub2.Clear();
            this.Pub2.AddFieldSet(" Set automatically populated from the table . <a href='?ExtType=" + this.ExtType + "&MyPK=" + this.MyPK + "&FK_MapData=" + this.FK_MapData + "&RefNo=" + this.RefNo + "'> Return </a>");
            this.Pub2.Add(" Under this form does not from the table , So you can not set the auto-fill from the table .");
            this.Pub2.AddFieldSetEnd();
            return;
        }
        string[] strs = myme.Tag2.Split('$');
        bool is1 = false;
        bool isHaveM2M = false;
        bool isHaveM2MM = false;
        foreach (MapM2M m2m in m2ms)
        {
            if (m2m.HisM2MType == M2MType.M2M)
                isHaveM2M = true;
            if (m2m.HisM2MType == M2MType.M2MM)
                isHaveM2MM = true;

            TextBox tb = new TextBox();
            tb.ID = "TB_" + m2m.NoOfObj;
            tb.Columns = 70;
            tb.Rows = 5;
            tb.TextMode = TextBoxMode.MultiLine;
            foreach (string s in strs)
            {
                if (s == null)
                    continue;

                if (s.Contains(m2m.NoOfObj + ":") == false)
                    continue;

                string[] ss = s.Split(':');
                tb.Text = ss[1];
            }
            this.Pub2.AddFieldSet(" Serial number :" + m2m.NoOfObj + ", Name :" + m2m.Name);
            this.Pub2.Add(tb);
            this.Pub2.AddFieldSetEnd();
        }
        this.Pub2.AddHR();
        Button mybtn = new Button();
        mybtn.ID = "Btn_Save";
        mybtn.CssClass = "Btn";
        mybtn.Text = " Save ";
        mybtn.Click += new EventHandler(mybtn_SaveAutoFullM2M_Click);
        this.Pub2.Add(mybtn);

        mybtn = new Button();
        mybtn.CssClass = "Btn";
        mybtn.ID = "Btn_Cancel";
        mybtn.Text = " Cancel ";
        mybtn.Click += new EventHandler(mybtn_SaveAutoFullM2M_Click);
        this.Pub2.Add(mybtn);
        this.Pub2.AddFieldSetEnd();

        if (isHaveM2M)
        {
            this.Pub2.AddFieldSet(" Help : Many ");
            this.Pub2.Add(" After the master data table changes , Many data to be changed , Change format :");
            this.Pub2.AddBR(" Examples :SELECT No,Name FROM WF_Emp WHERE FK_Dept='@Key' ");
            this.Pub2.AddBR(" Values related content is changed from time to time automatically populate checkbox.");
            this.Pub2.AddBR(" Watch out :");
            this.Pub2.AddBR("1,@Key  Is passed over the main table field variables .");
            this.Pub2.AddBR("2, Must and only No,Name Two columns , Do not reverse the order .");
            this.Pub2.AddFieldSetEnd();
        }

        if (isHaveM2MM)
        {
            this.Pub2.AddFieldSet(" Help : Many more ");
            this.Pub2.Add(" After the master data table changes , Many more data to be changed , Change format :");
            this.Pub2.AddBR(" Examples :SELECT a.FK_Emp M1ID, a.FK_Station as M2ID, b.Name as M2Name FROM Port_EmpStation a, Port_Station b WHERE  A.FK_Station=B.No and a.FK_Emp='@Key'");
            this.Pub2.AddBR(" Values related content is changed from time to time automatically populate checkbox.");
            this.Pub2.AddBR(" Watch out :");
            this.Pub2.AddBR("1,@Key  Is passed over the main table field variables .");
            this.Pub2.AddBR("2, Must and only 3 Column  M1ID,M2ID,M2Name, Do not reverse the order .1 Column ID Corresponding list ID,2,3 Column corresponds to the list of data sources ID With name .");
            this.Pub2.AddFieldSetEnd();
        }
    }
    /// <summary>
    ///  New auto-complete textbox 
    /// </summary>
    public void EditAutoFullDtl_TB()
    {
        MapExt myme = new MapExt(this.MyPK);
        MapDtls dtls = new MapDtls(myme.FK_MapData);

        this.Pub2.AddH2(" Set automatically populated from the table . <a href='?ExtType=" + this.ExtType + "&MyPK=" + this.MyPK + "&FK_MapData=" + this.FK_MapData + "&RefNo=" + this.RefNo + "'> Return </a>");
        if (dtls.Count == 0)
        {
            this.Pub2.Clear();
            this.Pub2.AddFieldSet(" Set automatically populated from the table . <a href='?ExtType=" + this.ExtType + "&MyPK=" + this.MyPK + "&FK_MapData=" + this.FK_MapData + "&RefNo=" + this.RefNo + "'> Return </a>");
            this.Pub2.Add(" Under this form does not from the table , So you can not set the auto-fill from the table .");
            this.Pub2.AddFieldSetEnd();
            return;
        }

        string[] strs = myme.Tag1.Split('$');
        bool is1 = false;
        foreach (MapDtl dtl in dtls)
        {
            TextBox tb = new TextBox();
            tb.ID = "TB_" + dtl.No;
            tb.Columns = 70;
            tb.Rows = 5;
            tb.TextMode = TextBoxMode.MultiLine;
            foreach (string s in strs)
            {
                if (s == null)
                    continue;

                if (s.Contains(dtl.No + ":") == false)
                    continue;

                string[] ss = s.Split(':');
                tb.Text = ss[1];
            }
            this.Pub2.AddFieldSet(" Serial number :" + dtl.No + ", Name :" + dtl.Name);
            this.Pub2.Add(tb);

            string fs = " Fields can be filled :";
            MapAttrs attrs = new MapAttrs(dtl.No);
            foreach (MapAttr item in attrs)
            {
                if (item.KeyOfEn == "OID" || item.KeyOfEn == "RefPKVal")
                    continue;
                fs += item.KeyOfEn + ",";
            }
            this.Pub2.Add("<BR>" + fs.Substring(0, fs.Length - 1));
            this.Pub2.AddFieldSetEnd();
        }

        this.Pub2.AddHR();
        Button mybtn = new Button();
        mybtn.ID = "Btn_Save";
        mybtn.CssClass = "Btn";
        mybtn.Text = " Save ";
        mybtn.Click += new EventHandler(mybtn_SaveAutoFullDtl_Click);
        this.Pub2.Add(mybtn);

        mybtn = new Button();
        mybtn.ID = "Btn_Cancel";
        mybtn.Text = " Cancel ";
        mybtn.Click += new EventHandler(mybtn_SaveAutoFullDtl_Click);
        this.Pub2.Add(mybtn);
        this.Pub2.AddFieldSetEnd();

        this.Pub2.AddFieldSet(" Help :");
        this.Pub2.Add(" Here you need to set up a query ");
        this.Pub2.AddBR(" Such as :SELECT XLMC AS suozaixianlu, bustype as V_BusType FROM [V_XLVsBusType] WHERE jbxx_htid='@Key'");
        this.Pub2.AddBR(" To this query from the list correspond with the value of the text box can happen sometimes change automatically filled in .");
        this.Pub2.AddBR(" Watch out :");
        this.Pub2.AddBR("1,@Key  Is passed over the main table field variables .");
        this.Pub2.AddBR("2, Word from the table column field name , And filling sql Column fields Match case .");
        this.Pub2.AddFieldSetEnd();
    }
    /// <summary>
    ///  New auto-complete textbox 
    /// </summary>
    public void EditAutoFullDtl_DDL()
    {
        this.Pub2.AddFieldSet("<a href='?ExtType=" + this.ExtType + "&MyPK=" + this.MyPK + "&FK_MapData=" + this.FK_MapData + "&RefNo=" + this.RefNo + "'> Return </a> - Set automatically populated from the table ");
        MapExt myme = new MapExt(this.MyPK);
        MapDtls dtls = new MapDtls(myme.FK_MapData);
        string[] strs = myme.Tag1.Split('$');
        this.Pub2.AddTable("border=0  align=left ");
        bool is1 = false;
        foreach (MapDtl dtl in dtls)
        {
            is1 = this.AddTR(is1);
            TextBox tb = new TextBox();
            tb.ID = "TB_" + dtl.No;
            tb.Columns = 80;
            tb.Rows = 3;
            tb.TextMode = TextBoxMode.MultiLine;
            foreach (string s in strs)
            {
                if (s == null)
                    continue;

                if (s.Contains(dtl.No + ":") == false)
                    continue;
                string[] ss = s.Split(':');
                tb.Text = ss[1];
            }

            this.Pub2.AddTDBegin();
            this.Pub2.AddB("&nbsp;&nbsp;" + dtl.Name + "- From Table ");
            this.Pub2.AddBR();
            this.Pub2.Add(tb);
            this.Pub2.AddTDEnd();
            this.Pub2.AddTREnd();
        }
        this.Pub2.AddTableEndWithHR();
        Button mybtn = new Button();
        mybtn.ID = "Btn_Save";
        mybtn.CssClass = "Btn";
        mybtn.Text = " Save ";
        mybtn.Click += new EventHandler(mybtn_SaveAutoFullDtl_Click);
        this.Pub2.Add(mybtn);

        mybtn = new Button();
        mybtn.ID = "Btn_Cancel";
        mybtn.Text = " Cancel ";
        mybtn.Click += new EventHandler(mybtn_SaveAutoFullDtl_Click);
        this.Pub2.Add(mybtn);
        this.Pub2.AddFieldSetEnd();
    }

    public void EditAutoJL()
    {
        MapExt myme = new MapExt(this.MyPK);
        MapAttrs attrs = new MapAttrs(myme.FK_MapData);
        string[] strs = myme.Tag.Split('$');

        this.Pub2.AddTable("border=0 width='70%' align=left");
        this.Pub2.AddCaptionLeft("<a href='?ExtType=" + this.ExtType + "&MyPK=" + this.MyPK + "&FK_MapData=" + this.FK_MapData + "&RefNo=" + this.RefNo + "'> Return </a> - Set cascading menus ");
        bool is1 = false;
        foreach (MapAttr attr in attrs)
        {
            if (attr.LGType == FieldTypeS.Normal)
                continue;
            if (attr.UIIsEnable == false)
                continue;

            TextBox tb = new TextBox();
            tb.ID = "TB_" + attr.KeyOfEn;
            tb.Attributes["width"] = "100%";
            tb.Columns = 90;
            tb.Rows = 4;
            tb.TextMode = TextBoxMode.MultiLine;

            foreach (string s in strs)
            {
                if (s == null)
                    continue;

                if (s.Contains(attr.KeyOfEn + ":") == false)
                    continue;

                string[] ss = s.Split(':');
                tb.Text = ss[1];
            }

            this.Pub2.AddTR();
            this.Pub2.AddTD("" + attr.Name + "  " + attr.KeyOfEn + "  Field <br>");
            this.Pub2.AddTREnd();

            this.Pub2.AddTR();
            this.Pub2.AddTD(tb);
            this.Pub2.AddTREnd();
        }
        this.Pub2.AddTR();
        this.Pub2.AddTDBegin();

        Button mybtn = new Button();
        mybtn.ID = "Btn_Save";
        mybtn.CssClass = "Btn";
        mybtn.Text = " Save ";
        mybtn.Click += new EventHandler(mybtn_SaveAutoFullJilian_Click);
        this.Pub2.Add(mybtn);

        mybtn = new Button();
        mybtn.CssClass = "Btn";
        mybtn.ID = "Btn_Cancel";
        mybtn.Text = " Cancel ";
        mybtn.Click += new EventHandler(mybtn_SaveAutoFullJilian_Click);
        this.Pub2.Add(mybtn);

        this.Pub2.AddTDEnd();
        this.Pub2.AddTREnd();

        this.Pub2.AddTableEnd();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        this.BindLeft();
        this.Page.Title = " Forms Extension Settings ";
        switch (this.DoType)
        {
            case "Del":
                MapExt mm = new MapExt();
                mm.MyPK = this.MyPK;
                if (this.ExtType == MapExtXmlList.InputCheck)
                    mm.Retrieve();

                mm.Delete();
                this.Response.Redirect("MapExt.aspx?FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "&RefNo=" + this.RefNo, true);
                return;
            case "EditAutoJL":
                this.EditAutoJL();
                return;
            default:
                break;
        }

        if (this.ExtType == null)
        {
            this.Pub2.AddFieldSet("Help");
            this.Pub2.AddH3(" All technical data are finishing in ,《 Gallop workflow engine - Process development manual .doc》and《 Gallop workflow engine - Operating Instructions Form Designer .doc》 Two files .");
            this.Pub2.AddH3("<br> These two files are located :D:\\ccflow\\Documents Below .");
            this.Pub2.AddH3("<a href='http://ccflow.org/Help.aspx' target=_blank> Official website Help ..</a>");
            this.Pub2.AddFieldSetEnd();
            return;
        }

        MapExts mes = new MapExts();
        switch (this.ExtType)
        {
            case MapExtXmlList.WordFrm: //word Stencil .
                this.FrmWord();
                break;
            case MapExtXmlList.ExcelFrm: //ExcelFrm.
                this.FrmExcel();
                break;
            case MapExtXmlList.Link: // Field connection .
                if (this.MyPK != null || this.DoType == "New")
                {
                    this.BindLinkEdit();
                    return;
                }
                this.BindLinkList();
                break;
            case MapExtXmlList.RegularExpression: // Regex .
                if (this.DoType == "templete")// Select the template 
                {
                    this.BindReTemplete();
                    return;
                }
                if (this.MyPK != null || this.DoType == "New")
                {
                    this.BindRegularExpressionEdit();
                    return;
                }
                this.BindRegularExpressionList();
                break;
            case MapExtXmlList.PageLoadFull: // Form filling is loaded .
            case MapExtXmlList.StartFlow: // Form filling is loaded .
                this.BindPageLoadFull();
                break;
            case MapExtXmlList.AutoFullDLL: // Dynamically populate drop-down box .
                this.BindAutoFullDDL();
                break;
            case MapExtXmlList.ActiveDDL: // Linkage menu .
                if (this.MyPK != null || this.OperAttrKey != null || this.DoType == "New")
                {
                    Edit_ActiveDDL();
                    return;
                }
                mes.Retrieve(MapExtAttr.ExtType, this.ExtType,
                    MapExtAttr.FK_MapData, this.FK_MapData);
                this.MapExtList(mes);
                break;
            case MapExtXmlList.TBFullCtrl:  // AutoComplete .
                if (this.DoType == "EditAutoFullDtl")
                {
                    this.EditAutoFullDtl_TB();
                    return;
                }
                if (this.DoType == "EditAutoFullM2M")
                {
                    this.EditAutoFullM2M_TB();
                    return;
                }

                if (this.MyPK != null || this.DoType == "New")
                {
                    this.EditAutoFull_TB();
                    return;
                }
                mes.Retrieve(MapExtAttr.ExtType, this.ExtType,
                    MapExtAttr.FK_MapData, this.FK_MapData);
                this.MapExtList(mes);
                break;
            case MapExtXmlList.DDLFullCtrl:  //DDL AutoComplete .
                if (this.DoType == "EditAutoFullDtl")
                {
                    this.EditAutoFullDtl_DDL();
                    return;
                }
                if (this.MyPK != null || this.DoType == "New")
                {
                    this.EditAutoFull_DDL();
                    return;
                }
                mes.Retrieve(MapExtAttr.ExtType, this.ExtType,
                    MapExtAttr.FK_MapData, this.FK_MapData);
                this.MapExtList(mes);
                break;
            case MapExtXmlList.InputCheck: // Input inspection .
                if (this.MyPK != null || this.DoType == "New")
                {
                    Edit_InputCheck();
                    return;
                }
                mes.Retrieve(MapExtAttr.ExtType, this.ExtType,
                    MapExtAttr.FK_MapData, this.FK_MapData);
                this.MapJS(mes);
                break;
            case MapExtXmlList.PopVal: // Linkage menu .
                if (this.MyPK != null || this.DoType == "New")
                {
                    Edit_PopVal();
                    return;
                }
                mes.Retrieve(MapExtAttr.ExtType, this.ExtType,
                    MapExtAttr.FK_MapData, this.FK_MapData);
                this.MapExtList(mes);
                break;
            case MapExtXmlList.Func: // Linkage menu .
                this.BindExpFunc();
                break;
            default:
                break;
        }
    }

    #region  Regex .
    public void BindReTemplete()
    {
        this.Pub2.AddFieldSet(" Using the Event template :" + this.OperAttrKey);
        this.Pub2.AddTable("align=left width=100%");

        this.Pub2.AddTR();
        this.Pub2.AddTD("colspan=2", "<b color='blue'> Using the Event template , Can help you quickly define the form field events </b>");
        this.Pub2.AddTREnd();

        this.Pub2.AddTR();
        this.Pub2.AddTDTitle("colspan=2", " Event Templates - Click on the name of the choice it ");
        this.Pub2.AddTREnd();

        this.Pub2.AddTR();
        ListBox lb = new ListBox();
        lb.Style["width"] = "100%";
        lb.AutoPostBack = false;
        lb.ID = "LBReTemplete";
        
        BP.XML.RegularExpressions res = new BP.XML.RegularExpressions();
        res.RetrieveAll();
        foreach (BP.XML.RegularExpression item in res)
        {
            ListItem li = new ListItem(item.Name + "->" + item.Note, item.No);
            lb.Items.Add(li);
        }
        this.Pub2.AddTD("colspan=2", lb);
        this.Pub2.AddTREnd();

        this.Pub2.AddTRSum();
        Button btn = new Button();
        btn.ID = "BtnSave";
        btn.CssClass = "Btn";
        btn.Text = "Save";
        btn.Click += new EventHandler(btn_SaveReTemplete_Click);

        this.Pub2.AddTD("colspan=1 width='60'", btn);
        this.Pub2.AddTD("<a href='MapExt.aspx?FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "&OperAttrKey=" + this.OperAttrKey + "&DoType=New'> Return </a>");
        this.Pub2.AddTREnd();

        this.Pub2.AddTableEnd();

        this.Pub2.AddFieldSetEnd();
    }
    public void btn_SaveReTemplete_Click(object sender, EventArgs e)
    {
        ListBox lb = this.Pub2.FindControl("LBReTemplete") as ListBox;
        if (lb == null && lb.SelectedItem.Value == null) return;

        string newMyPk = "";
        BP.XML.RegularExpressionDtls reDtls = new BP.XML.RegularExpressionDtls();
        reDtls.RetrieveAll();

        // Delete the existing logic .
        BP.Sys.MapExts exts = new BP.Sys.MapExts();
        exts.Delete(MapExtAttr.AttrOfOper, this.OperAttrKey,
            MapExtAttr.ExtType, BP.Sys.MapExtXmlList.RegularExpression);

        //  Start loading .
        foreach (BP.XML.RegularExpressionDtl dtl in reDtls)
        {
            if (dtl.ItemNo != lb.SelectedItem.Value)
                continue;

            BP.Sys.MapExt ext = new BP.Sys.MapExt();
            ext.MyPK = this.FK_MapData + "_" + this.OperAttrKey + "_" + MapExtXmlList.RegularExpression + "_" + dtl.ForEvent;
            ext.FK_MapData = this.FK_MapData;
            ext.AttrOfOper = this.OperAttrKey;
            ext.Doc = dtl.Exp; // Expression formula .
            ext.Tag = dtl.ForEvent; // Time .
            ext.Tag1 = dtl.Msg;  // News 
            ext.ExtType = MapExtXmlList.RegularExpression; //  Expression formula  .
            ext.Insert();
            newMyPk = ext.MyPK;
        }
        this.Response.Redirect("MapExt.aspx?FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "&MyPK=" + newMyPk + "&OperAttrKey=" + this.OperAttrKey + "&DoType=New", true);
    }
    public void BindRegularExpressionList()
    {
        MapExts mes = new MapExts();
        mes.Retrieve(MapExtAttr.ExtType, this.ExtType,
                   MapExtAttr.FK_MapData, this.FK_MapData);
        this.Pub2.AddTable("align=left width=100%");
        this.Pub2.AddCaptionLeftTX(" Regex ");
        this.Pub2.AddTR();
        this.Pub2.AddTDTitle("IDX");
        this.Pub2.AddTDTitle(" Field ");
        //this.Pub2.AddTDTitle(" Expression formula ");
        //this.Pub2.AddTDTitle(" Message ");
        this.Pub2.AddTDTitle(" Operating ");
        this.Pub2.AddTREnd();

        string tKey = DateTime.Now.ToString("yyMMddhhmmss");

        MapAttrs attrs = new MapAttrs(this.FK_MapData);
        int idx = 0;
        foreach (MapAttr attr in attrs)
        {
            if (attr.UIVisible == false)
                continue;

            #region  songhonggang (2014-06-15)  Show all regular modification 
            //if (attr.UIIsEnable == false)
            //    continue;
            #endregion

            this.Pub2.AddTR();
            this.Pub2.AddTDIdx(idx++);
            this.Pub2.AddTD(attr.KeyOfEn + "-" + attr.Name);
            MapExt me = mes.GetEntityByKey(MapExtAttr.AttrOfOper, attr.KeyOfEn) as MapExt;
            if (me == null)
                this.Pub2.AddTD("<a href='MapExt.aspx?s=" + tKey + "&FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "&OperAttrKey=" + attr.KeyOfEn + "&DoType=New'> Set up </a>");
            else
                this.Pub2.AddTD("<a href='MapExt.aspx?s=" + tKey + "&FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "&MyPK=" + me.MyPK + "&OperAttrKey=" + attr.KeyOfEn + "'> Modification </a>");
            this.Pub2.AddTREnd();
        }
        this.Pub2.AddTableEnd();
    }
   
    public void BindRegularExpressionEdit()
    {
        this.Pub2.AddTable();
        this.Pub2.AddCaption(" Regex  -<a href=\"MapExt.aspx?s=3&FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "&OperAttrKey=" + this.OperAttrKey + "&DoType=templete\"> Load Template </a>- <a href='MapExt.aspx?s=3&FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "' > Return </a>");

        this.Pub2.AddTR();
        this.Pub2.AddTDTitle("No.");
        this.Pub2.AddTDTitle(" Event ");
        this.Pub2.AddTDTitle(" Event Content ");
        this.Pub2.AddTDTitle(" Message ");
        this.Pub2.AddTDTitle(" Action ");
        this.Pub2.AddTREnd();
           
        #region  Binding events 
        int idx = 1;
        idx = BindRegularExpressionEditExt(idx, "onblur");
        idx = BindRegularExpressionEditExt(idx, "onchange");

        idx = BindRegularExpressionEditExt(idx, "onclick");
        idx = BindRegularExpressionEditExt(idx, "ondblclick");

        idx = BindRegularExpressionEditExt(idx, "onkeypress");
        idx = BindRegularExpressionEditExt(idx, "onkeyup");
        idx = BindRegularExpressionEditExt(idx, "onsubmit");
        #endregion
        this.Pub2.AddTableEnd();

        Button btn = new Button();
        btn.ID = "Btn_Save";
        btn.Text = "Save";
        btn.Click += new EventHandler(BindRegularExpressionEdit_Click);
        this.Pub2.Add(btn);
       
    }
    public int BindRegularExpressionEditExt(int idx, string myEvent)
    {
        //  Inquiry .
        MapExt me = new MapExt();
        me.FK_MapData = this.FK_MapData;
        me.Tag = myEvent;
        me.AttrOfOper = this.OperAttrKey;
        me.MyPK = me.FK_MapData + "_" + this.OperAttrKey + "_" + this.ExtType + "_" + myEvent;
        me.RetrieveFromDBSources();

        this.Pub2.AddTR();
        this.Pub2.AddTDIdx(idx);
        this.Pub2.AddTD(myEvent);

        TextBox tb = new TextBox();
        tb.TextMode = TextBoxMode.MultiLine;
        string tbdocid = "TB_Doc_" + myEvent;
        tb.ID = tbdocid;
        tb.Text = me.Doc;
        tb.Columns = 50;
        tb.Rows = 3;
        this.Pub2.AddTD(tb);

        tb = new TextBox();
        tb.ID = "TB_Tag1_" + myEvent;
        tb.Text = me.Tag1;
        tb.Columns = 50;
        tb.Rows = 3;
        this.Pub2.AddTD(tb);

        HyperLink hl = new HyperLink();
        hl.ID = "HL_Tag1_" + myEvent;
        hl.Text = "edit";
        string FK_MapData = Request.Params["FK_MapData"],
            OperAttrKey = Request.Params["OperAttrKey"]
            ;
        string src = "/WF/MapDef/FrmSelfjs.aspx?FK_MapData=" + FK_MapData + "&event=" + myEvent + "&OperAttrKey=" + OperAttrKey+"&docid="+tbdocid
            , title = FK_MapData + ":" + OperAttrKey + ":" + myEvent;
        hl.NavigateUrl = "javascript:EUIWinOpen('"+src+"','"+title+"').window('maximize');";
        this.Pub2.AddTD(hl);

        this.Pub2.AddTREnd();
        idx = idx + 1;
        return idx;
    }
    public void BindRegularExpressionEdit_ClickSave(string myEvent)
    {
        MapExt me = new MapExt();
        me.FK_MapData = this.FK_MapData;
        me.ExtType = this.ExtType;
        me.Tag = myEvent;
        me.AttrOfOper = this.OperAttrKey;
        me.MyPK = this.FK_MapData + "_" + this.OperAttrKey + "_" + me.ExtType + "_" + me.Tag;
        me.Delete();

        me.Doc = this.Pub2.GetTextBoxByID("TB_Doc_" + myEvent).Text;
        me.Tag1 = this.Pub2.GetTextBoxByID("TB_Tag1_" + myEvent).Text;
        if (me.Doc.Trim().Length == 0)
            return;

        me.Insert();
    }
    void BindRegularExpressionEdit_Click(object sender, EventArgs e)
    {
        #region  Save 
        BindRegularExpressionEdit_ClickSave("onblur");
        BindRegularExpressionEdit_ClickSave("onchange");

        BindRegularExpressionEdit_ClickSave("onclick");

        BindRegularExpressionEdit_ClickSave("ondblclick");

        BindRegularExpressionEdit_ClickSave("onkeypress");
        BindRegularExpressionEdit_ClickSave("onkeyup");
        BindRegularExpressionEdit_ClickSave("onsubmit");
        #endregion

        this.Response.Redirect("MapExt.aspx?s=3&FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType, true);
    }
    #endregion RegularExpression


    #region link.
    public void BindLinkList()
    {
        MapExts mes = new MapExts();
        mes.Retrieve(MapExtAttr.ExtType, this.ExtType,
                   MapExtAttr.FK_MapData, this.FK_MapData);
        this.Pub2.AddTable("align=left width=100%");
        this.Pub2.AddCaptionLeft(" Hyperlink field ");
        this.Pub2.AddTR();
        this.Pub2.AddTDTitle("IDX");
        this.Pub2.AddTDTitle(" Field ");
        this.Pub2.AddTDTitle(" Connection ");
        this.Pub2.AddTDTitle(" Window ");
        this.Pub2.AddTDTitle(" Operating ");
        this.Pub2.AddTREnd();

        MapAttrs attrs = new MapAttrs(this.FK_MapData);
        int idx = 0;
        foreach (MapAttr attr in attrs)
        {
            if (attr.UIVisible == false)
                continue;

            if (attr.UIIsEnable == true)
                continue;

            this.Pub2.AddTR();
            this.Pub2.AddTDIdx(idx++);
            this.Pub2.AddTD(attr.KeyOfEn + "-" + attr.Name);
            MapExt me = mes.GetEntityByKey(MapExtAttr.AttrOfOper, attr.KeyOfEn) as MapExt;
            if (me == null)
            {
                this.Pub2.AddTD("-");
                this.Pub2.AddTD("-");
                this.Pub2.AddTD("<a href='MapExt.aspx?s=3&FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "&OperAttrKey=" + attr.KeyOfEn + "&DoType=New'> Set up </a>");
            }
            else
            {
                this.Pub2.AddTD(me.Tag);
                this.Pub2.AddTD(me.Tag1);
                this.Pub2.AddTD("<a href='MapExt.aspx?s=3&FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "&MyPK=" + me.MyPK + "&OperAttrKey=" + attr.KeyOfEn + "'> Modification </a>");
            }
            this.Pub2.AddTREnd();
        }
        this.Pub2.AddTableEnd();
    }
    public void BindLinkEdit()
    {
        MapExt me = new MapExt();
        if (this.MyPK != null)
        {
            me.MyPK = this.MyPK;
            me.RetrieveFromDBSources();
        }
        else
        {
            me.FK_MapData = this.FK_MapData;
            me.AttrOfOper = this.OperAttrKey;
            me.Tag = "http://ccflow.org";
            me.Tag1 = "_" + this.OperAttrKey;
        }

        this.Pub2.AddTable();
        this.Pub2.AddCaptionLeft(" Hyperlink field  - <a href='MapExt.aspx?s=3&FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "' > Return </a>");
        this.Pub2.AddTR();
        this.Pub2.AddTD(" English name field ");
        this.Pub2.AddTD(this.OperAttrKey);
        this.Pub2.AddTREnd();

        this.Pub2.AddTR();
        this.Pub2.AddTD(" Fields Chinese name ");
        MapAttr ma = new MapAttr(this.FK_MapData, this.OperAttrKey);
        this.Pub2.AddTD(ma.Name);
        this.Pub2.AddTREnd();
        TextBox tb = new TextBox();
        tb.ID = "TB_Tag";
        tb.Text = me.Tag;
        tb.Columns = 50;
        this.Pub2.AddTR();
        this.Pub2.AddTD("Url");
        this.Pub2.AddTD(tb);
        this.Pub2.AddTREnd();

        tb = new TextBox();
        tb.ID = "TB_Tag1";
        tb.Text = me.Tag1;
        tb.Columns = 50;
        this.Pub2.AddTR();
        this.Pub2.AddTD(" Window ");
        this.Pub2.AddTD(tb);
        this.Pub2.AddTREnd();

        Button btn = new Button();
        btn.ID = "Btn_Save";
        btn.CssClass = "Btn";
        btn.Text = "Save";
        btn.Click += new EventHandler(BindLinkEdit_Click);
        this.Pub2.AddTR();
        this.Pub2.AddTD(btn);
        if (this.MyPK != null)
        {
            btn = new Button();
            btn.ID = "Btn_Del";
            btn.CssClass = "Btn";
            btn.Text = "Delete";
            btn.Click += new EventHandler(BindLinkEdit_Click);
            btn.Attributes["onclick"] = "return window.confirm(' Are you sure you want to delete it ?');";
            this.Pub2.AddTD(btn);
        }
        else
        {
            this.Pub2.AddTD();
        }
        this.Pub2.AddTREnd();
        this.Pub2.AddTableEnd();
    }
    void BindLinkEdit_Click(object sender, EventArgs e)
    {
        MapExt me = new MapExt();
        Button btn = sender as Button;
        if (btn.ID == "Btn_Del")
        {
            me.MyPK = this.MyPK;
            me.Delete();
            this.Response.Redirect("MapExt.aspx?s=3&FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType, true);
            return;
        }

        me = (MapExt)this.Pub2.Copy(me);
        me.FK_MapData = this.FK_MapData;
        me.AttrOfOper = this.OperAttrKey;
        //me.Tag = this.Pub2.GetTextBoxByID("TB_Tag").Text;
        //me.Tag1 = this.Pub2.GetTextBoxByID("TB_Tag1").Text;
        me.ExtType = this.ExtType;
        if (this.MyPK == null)
            me.MyPK = me.FK_MapData + "_" + me.AttrOfOper + "_" + this.ExtType;
        else
            me.MyPK = this.MyPK;
        me.Save();

        this.Response.Redirect("MapExt.aspx?s=3&FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType, true);
    }
    #endregion
    /// <summary>
    /// BindPageLoadFull
    /// </summary>
    public void BindPageLoadFull()
    {
        MapExt me = new MapExt();
        me.MyPK = this.FK_MapData + "_" + this.ExtType;
        me.RetrieveFromDBSources();

        this.Pub2.AddTable("align=left");
        this.Pub2.AddCaptionLeft(" Filled the main table SQL");
        this.Pub2.AddTR();
        this.Pub2.AddTDTitle(" Main table settings ");
        this.Pub2.AddTREnd();

        TextBox tb = new TextBox();
        tb.ID = "TB_" + MapExtAttr.Tag;
        tb.Text = me.Tag;
        tb.TextMode = TextBoxMode.MultiLine;
        tb.Rows = 10;
        tb.Columns = 70;
        this.Pub2.AddTR();
        this.Pub2.AddTD(tb);
        this.Pub2.AddTREnd();

        this.Pub2.AddTR();
        this.Pub2.AddTD(" Explanation : Filled the main table sql, Expressions support @ Public Variables and agreed . <br> Such as : SELECT No,Name,Tel FROM Port_Emp WHERE No='@WebUser.No' ,  If the column name with the same name as the beginning of a form field , Will automatically give value .");
        this.Pub2.AddTREnd();

        MapDtls dtls = new MapDtls(this.FK_MapData);
        if (dtls.Count != 0)
        {
            this.Pub2.AddTR();
            this.Pub2.AddTDTitle(" From automatically populate the table .");
            this.Pub2.AddTREnd();
            string[] sqls = me.Tag1.Split('*');
            foreach (MapDtl dtl in dtls)
            {
                this.Pub2.AddTR();
                this.Pub2.AddTD(" From Table :(" + dtl.No + ")" + dtl.Name);
                this.Pub2.AddTREnd();
                tb = new TextBox();
                tb.ID = "TB_" + dtl.No;
                foreach (string sql in sqls)
                {
                    if (string.IsNullOrEmpty(sql))
                        continue;
                    string key = sql.Substring(0, sql.IndexOf('='));
                    if (key == dtl.No)
                    {
                        tb.Text = sql.Substring(sql.IndexOf('=') + 1);
                        break;
                    }
                }
                tb.TextMode = TextBoxMode.MultiLine;
                tb.Rows = 10;
                tb.Columns = 70;
                this.Pub2.AddTR();
                this.Pub2.AddTD(tb);
                this.Pub2.AddTREnd();
            }

            this.Pub2.AddTR();
            this.Pub2.AddTD(" Explanation : The result set from a table filled ");
            this.Pub2.AddTREnd();
        }

        Button btn = new Button();
        btn.CssClass = "Btn";
        btn.ID = "Btn_Save";
        btn.Text = " Save ";
        btn.Click += new EventHandler(btn_SavePageLoadFull_Click);
        this.Pub2.AddTR();
        this.Pub2.AddTD(btn);
        this.Pub2.AddTREnd();
        this.Pub2.AddTableEnd();
        return;
    }
    /// <summary>
    ///  Save it 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void btn_SavePageLoadFull_Click(object sender, EventArgs e)
    {
        MapExt me = new MapExt();
        me.MyPK = this.FK_MapData + "_" + this.ExtType;
        me.FK_MapData = this.FK_MapData;
        me.ExtType = this.ExtType;
        me.RetrieveFromDBSources();

        me.Tag = this.Pub2.GetTextBoxByID("TB_" + MapExtAttr.Tag).Text;
        string sql = "";
        MapDtls dtls = new MapDtls(this.FK_MapData);
        foreach (MapDtl dtl in dtls)
        {
            sql += "*" + dtl.No + "=" + this.Pub2.GetTextBoxByID("TB_" + dtl.No).Text;
        }
        me.Tag1 = sql;

        me.MyPK = this.FK_MapData + "_" + this.ExtType;

        string info = me.Tag1 + me.Tag;
        if (string.IsNullOrEmpty(info))
            me.Delete();
        else
            me.Save();
    }

    #region  Save word Template Properties .
    /// <summary>
    /// Word Property .
    /// </summary>
    public void FrmWord()
    {
        MapData ath = new MapData(this.FK_MapData);

        #region WebOffice Control mode .
        this.Pub2.AddTable();

        this.Pub2.AddTR1();
        this.Pub2.AddTDTitle("colspan=3", "WebOffice Control mode .");
        this.Pub2.AddTREnd();

        this.Pub2.AddTR();
        CheckBox cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableWF;
        cb.Text = " Whether to enable weboffice?";
        cb.Checked = ath.IsWoEnableWF;
        this.Pub2.AddTD(cb);

        cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableSave;
        cb.Text = " Whether saving enabled ?";
        cb.Checked = ath.IsWoEnableSave;
        this.Pub2.AddTD(cb);

        cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableReadonly;
        cb.Text = " Is read-only ?";
        cb.Checked = ath.IsWoEnableReadonly;
        this.Pub2.AddTD(cb);
        this.Pub2.AddTREnd();



        this.Pub2.AddTR();
        cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableRevise;
        cb.Text = " Whether to amend enabled ?";
        cb.Checked = ath.IsWoEnableRevise;
        this.Pub2.AddTD(cb);

        cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableViewKeepMark;
        cb.Text = " View whether a user traces ?";
        cb.Checked = ath.IsWoEnableViewKeepMark;
        this.Pub2.AddTD(cb);

        cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnablePrint;
        cb.Text = " Whether to print ?";
        cb.Checked = ath.IsWoEnablePrint;
        this.Pub2.AddTD(cb);
        this.Pub2.AddTREnd();


        this.Pub2.AddTR();
        cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableOver;
        cb.Text = " Whether Taohong enabled ?";
        cb.Checked = ath.IsWoEnableOver;
        this.Pub2.AddTD(cb);

        cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableSeal;
        cb.Text = " Whether the signature is enabled ?";
        cb.Checked = ath.IsWoEnableSeal;
        this.Pub2.AddTD(cb);

        cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableTemplete;
        cb.Text = " Whether to enable the template file ?";
        cb.Checked = ath.IsWoEnableTemplete;
        this.Pub2.AddTD(cb);

        this.Pub2.AddTREnd();
        this.Pub2.AddTR();
        cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableCheck;
        cb.Text = " Whether the record node information ?";
        cb.Checked = ath.IsWoEnableCheck;
        this.Pub2.AddTD(cb);
        cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableInsertFlow;
        cb.Text = " Whether to enable the insertion process ?";
        cb.Checked = ath.IsWoEnableInsertFlow;
        this.Pub2.AddTD(cb);
        cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableInsertFengXian;
        cb.Text = " Whether the insertion point to enable risk ?";
        cb.Checked = ath.IsWoEnableInsertFengXian;
        this.Pub2.AddTD(cb);
        this.Pub2.AddTR();
        cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableMarks;
        cb.Text = " Whether to enter the traces mode ?";
        cb.Checked = ath.IsWoEnableMarks;
        this.Pub2.AddTD(cb);
        cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableDown;
        cb.Text = " Whether to download enabled ?";
        cb.Checked = ath.IsWoEnableDown;
        this.Pub2.AddTD(cb);
        this.Pub2.AddTD("");
        this.Pub2.AddTREnd();
        this.Pub2.AddTREnd();
        this.Pub2.AddTableEnd();
        #endregion WebOffice Control mode .

        this.Pub2.Add(" Template file ( Must be *.doc File ):");
        FileUpload fu = new FileUpload();
        fu.ID = "FU";
        this.Pub2.Add(fu);
        this.Pub2.Add("<a href='/DataUser/FrmOfficeTemplate/"+this.FK_MapData+".doc' target=_blank> Download or open a template </a>");

        this.Pub2.AddBR(); 
        Button btn = new Button();
        btn.ID = "Save";
        btn.Text = " Save ";
        btn.Click += new EventHandler(btn_SaveWordFrm_Click);
        this.Pub2.Add(btn);
    }

    void btn_SaveWordFrm_Click(object sender, EventArgs e)
    {
        MapData ath = new MapData(this.FK_MapData);
        ath.IsWoEnableWF = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableWF).Checked;
        ath.IsWoEnableSave = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableSave).Checked;
        ath.IsWoEnableReadonly = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableReadonly).Checked;
        ath.IsWoEnableRevise = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableRevise).Checked;
        ath.IsWoEnableViewKeepMark = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableViewKeepMark).Checked;
        ath.IsWoEnablePrint = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnablePrint).Checked;
        ath.IsWoEnableSeal = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableSeal).Checked;
        ath.IsWoEnableOver = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableOver).Checked;
        ath.IsWoEnableTemplete = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableTemplete).Checked;
        ath.IsWoEnableCheck = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableCheck).Checked;
        ath.IsWoEnableInsertFengXian = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableInsertFengXian).Checked;
        ath.IsWoEnableInsertFlow = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableInsertFlow).Checked;
        ath.IsWoEnableMarks = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableMarks).Checked;
        ath.IsWoEnableDown = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableDown).Checked;
        ath.Update();

        FileUpload fu = this.Pub2.FindControl("FU") as FileUpload;
        if (fu.FileName != null)
            fu.SaveAs(SystemConfig.PathOfDataUser + "\\FrmOfficeTemplate\\" + this.FK_MapData + ".doc");
    }
    #endregion  Save word Template Properties .

    #region  Save Excel Template Properties .
    /// <summary>
    /// Word Property .
    /// </summary>
    public void FrmExcel()
    {
        MapData ath = new MapData(this.FK_MapData);

        #region Excel  Control mode .
        this.Pub2.AddTable();

        this.Pub2.AddTR1();
        this.Pub2.AddTDTitle("colspan=3", "Excel Control mode .");
        this.Pub2.AddTREnd();

        this.Pub2.AddTR();
        CheckBox cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableWF;
        cb.Text = " Whether to enable weboffice?";
        cb.Checked = ath.IsWoEnableWF;
        this.Pub2.AddTD(cb);

        cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableSave;
        cb.Text = " Whether saving enabled ?";
        cb.Checked = ath.IsWoEnableSave;
        this.Pub2.AddTD(cb);

        cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableReadonly;
        cb.Text = " Is read-only ?";
        cb.Checked = ath.IsWoEnableReadonly;
        this.Pub2.AddTD(cb);
        this.Pub2.AddTREnd();



        this.Pub2.AddTR();
        cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableRevise;
        cb.Text = " Whether to amend enabled ?";
        cb.Checked = ath.IsWoEnableRevise;
        this.Pub2.AddTD(cb);

        cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableViewKeepMark;
        cb.Text = " View whether a user traces ?";
        cb.Checked = ath.IsWoEnableViewKeepMark;
        this.Pub2.AddTD(cb);

        cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnablePrint;
        cb.Text = " Whether to print ?";
        cb.Checked = ath.IsWoEnablePrint;
        this.Pub2.AddTD(cb);
        this.Pub2.AddTREnd();


        this.Pub2.AddTR();
        cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableOver;
        cb.Text = " Whether Taohong enabled ?";
        cb.Checked = ath.IsWoEnableOver;
        this.Pub2.AddTD(cb);

        cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableSeal;
        cb.Text = " Whether the signature is enabled ?";
        cb.Checked = ath.IsWoEnableSeal;
        this.Pub2.AddTD(cb);

        cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableTemplete;
        cb.Text = " Whether to enable the template file ?";
        cb.Checked = ath.IsWoEnableTemplete;
        this.Pub2.AddTD(cb);

        this.Pub2.AddTREnd();
        this.Pub2.AddTR();
        cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableCheck;
        cb.Text = " Whether the record node information ?";
        cb.Checked = ath.IsWoEnableCheck;
        this.Pub2.AddTD(cb);
        cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableInsertFlow;
        cb.Text = " Whether to enable the insertion process ?";
        cb.Checked = ath.IsWoEnableInsertFlow;
        this.Pub2.AddTD(cb);
        cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableInsertFengXian;
        cb.Text = " Whether the insertion point to enable risk ?";
        cb.Checked = ath.IsWoEnableInsertFengXian;
        this.Pub2.AddTD(cb);
        this.Pub2.AddTR();
        cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableMarks;
        cb.Text = " Whether to enter the traces mode ?";
        cb.Checked = ath.IsWoEnableMarks;
        this.Pub2.AddTD(cb);
        cb = new CheckBox();
        cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableDown;
        cb.Text = " Whether to download enabled ?";
        cb.Checked = ath.IsWoEnableDown;
        this.Pub2.AddTD(cb);
        this.Pub2.AddTD("");
        this.Pub2.AddTREnd();
        this.Pub2.AddTREnd();
        this.Pub2.AddTableEnd();
        #endregion WebOffice Control mode .

        this.Pub2.Add(" Template file ( Must be *.xls File ):");
        FileUpload fu = new FileUpload();
        fu.ID = "FU";
        this.Pub2.Add(fu);
        this.Pub2.Add("<a href='/DataUser/FrmOfficeTemplate/" + this.FK_MapData + ".xls' target=_blank> Download or open a template </a>");

        this.Pub2.AddBR();
        Button btn = new Button();
        btn.ID = "Save";
        btn.Text = " Save ";
        btn.Click += new EventHandler(btn_SaveExcelFrm_Click);
        this.Pub2.Add(btn);
    }

    void btn_SaveExcelFrm_Click(object sender, EventArgs e)
    {
        MapData ath = new MapData(this.FK_MapData);
        ath.IsWoEnableWF = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableWF).Checked;
        ath.IsWoEnableSave = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableSave).Checked;
        ath.IsWoEnableReadonly = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableReadonly).Checked;
        ath.IsWoEnableRevise = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableRevise).Checked;
        ath.IsWoEnableViewKeepMark = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableViewKeepMark).Checked;
        ath.IsWoEnablePrint = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnablePrint).Checked;
        ath.IsWoEnableSeal = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableSeal).Checked;
        ath.IsWoEnableOver = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableOver).Checked;
        ath.IsWoEnableTemplete = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableTemplete).Checked;
        ath.IsWoEnableCheck = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableCheck).Checked;
        ath.IsWoEnableInsertFengXian = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableInsertFengXian).Checked;
        ath.IsWoEnableInsertFlow = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableInsertFlow).Checked;
        ath.IsWoEnableMarks = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableMarks).Checked;
        ath.IsWoEnableDown = this.Pub2.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableDown).Checked;
        ath.Update();

        FileUpload fu = this.Pub2.FindControl("FU") as FileUpload;
        if (fu.FileName != null)
            fu.SaveAs(SystemConfig.PathOfDataUser + "\\FrmOfficeTemplate\\" + this.FK_MapData + ".xls");
    }
    #endregion  Save word Template Properties .

    public void BindAutoFullDDL()
    {
        if (this.DoType == "Del")
        {
            BP.Sys.MapExt me = new MapExt();
            me.MyPK = this.Request.QueryString["FK_MapExt"];
            me.Delete();
        }

        MapAttrs attrs = new MapAttrs();
        attrs.Retrieve(MapAttrAttr.FK_MapData, this.FK_MapData,
            MapAttrAttr.UIContralType, (int)BP.En.UIContralType.DDL,
            MapAttrAttr.UIVisible, 1, MapAttrAttr.UIIsEnable, 1);

        if (attrs.Count == 0)
        {
            this.Pub2.AddMsgOfWarning(" Prompt ",
                " This form does not automatically fill the content that can be set .<br> Only to meet , Visible , Enabled , Is a foreign key field , It can be set .");
            return;
        }

        MapExts mes = new MapExts();
        mes.Retrieve(MapExtAttr.FK_MapData, this.FK_MapData, MapExtAttr.ExtType, MapExtXmlList.AutoFullDLL);
        this.Pub2.AddTable("align=left width='60%'");
        this.Pub2.AddCaptionLeft(this.Lab);
        this.Pub2.AddTR();
        this.Pub2.AddTDTitle(" No. ");
        this.Pub2.AddTDTitle(" Field ");
        this.Pub2.AddTDTitle(" Chinese name ");
        this.Pub2.AddTDTitle(" Binding source ");
        this.Pub2.AddTDTitle(" Operating ");
        this.Pub2.AddTREnd();
        string fk_attr = this.Request.QueryString["FK_Attr"];
        int idx = 0;
        MapAttr attrOper = null;
        foreach (MapAttr attr in attrs)
        {
            if (attr.KeyOfEn == fk_attr)
                attrOper = attr;

            this.Pub2.AddTR();
            this.Pub2.AddTDIdx(idx++);
            this.Pub2.AddTD(attr.KeyOfEn);
            this.Pub2.AddTD(attr.Name);
            this.Pub2.AddTD(attr.UIBindKey);
            MapExt me = mes.GetEntityByKey(MapExtAttr.AttrOfOper, attr.KeyOfEn) as MapExt;
            if (me == null)
                this.Pub2.AddTD("<a href='?FK_MapData=" + this.FK_MapData + "&FK_Attr=" + attr.KeyOfEn + "&ExtType=" + MapExtXmlList.AutoFullDLL + "' > Set up </a>");
            else
                this.Pub2.AddTD("<a href='?FK_MapData=" + this.FK_MapData + "&FK_Attr=" + attr.KeyOfEn + "&ExtType=" + MapExtXmlList.AutoFullDLL + "' > Editor </a> - <a href=\"javascript:DoDel('" + me.MyPK + "','" + this.FK_MapData + "','" + MapExtXmlList.AutoFullDLL + "')\" > Delete </a>");
            this.Pub2.AddTREnd();
        }

        if (fk_attr != null)
        {
            MapExt me = new MapExt();
            me.MyPK = MapExtXmlList.AutoFullDLL + "_" + this.FK_MapData + "_" + fk_attr;
            me.RetrieveFromDBSources();
            this.Pub2.AddTR();
            this.Pub2.AddTDBegin("colspan=5");
            this.Pub2.AddFieldSet(" Set up :(" + attrOper.KeyOfEn + " - " + attrOper.Name + ") Runtime automatically populate data ");
            TextBox tb = new TextBox();
            tb.TextMode = TextBoxMode.MultiLine;
            tb.Columns = 80;
            tb.ID = "TB_Doc";
            tb.Rows = 4;
            tb.Text = me.Doc.Replace("~", "'");
            this.Pub2.Add(tb);
            this.Pub2.AddBR();
            Button btn = new Button();
            btn.ID = "Btn_Save_AutoFullDLL";
            btn.CssClass = "Btn";
            btn.Text = "Save ";
            btn.Click += new EventHandler(btn_Save_AutoFullDLL_Click);
            this.Pub2.Add(btn);
            this.Pub2.Add("<br> Case :SELECT No,Name FROM Port_Emp WHERE FK_Dept LIKE '@WebUser.FK_Dept%' <br> You can use @ Symbols take the form of field variables , Or global variables , For more information, please refer to the instructions .");
            this.Pub2.Add("<br> Data source must have No,Name Two columns .");

            this.Pub2.AddFieldSetEnd();
            this.Pub2.AddTDEnd();
            this.Pub2.AddTREnd();
            this.Pub2.AddTableEnd();
        }
        else
        {
            this.Pub2.AddTableEnd();
        }
    }
    void btn_Save_AutoFullDLL_Click(object sender, EventArgs e)
    {
        string attr = this.Request.QueryString["FK_Attr"];
        MapExt me = new MapExt();
        me.MyPK = MapExtXmlList.AutoFullDLL + "_" + this.FK_MapData + "_" + attr;
        me.RetrieveFromDBSources();
        me.FK_MapData = this.FK_MapData;
        me.AttrOfOper = attr;
        me.ExtType = MapExtXmlList.AutoFullDLL;
        me.Doc = this.Pub2.GetTextBoxByID("TB_Doc").Text.Replace("'", "~");

        try
        {
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(me.Doc);
        }
        catch
        {
            this.Alert("SQL Can not be properly executed , Misspelled , Please check .");
            return;
        }

        me.Save();
        this.Response.Redirect("MapExt.aspx?FK_MapData=" + this.FK_MapData + "&ExtType=" + MapExtXmlList.AutoFullDLL, true);
    }
    /// <summary>
    ///  Function execution 
    /// </summary>
    public void BindExpFunc()
    {
        BP.Sys.ExpFucnXmls xmls = new ExpFucnXmls();
        xmls.RetrieveAll();

        this.Pub2.AddFieldSet(" Export ");
        this.Pub2.AddUL();
        foreach (ExpFucnXml item in xmls)
        {
            this.Pub2.AddLi("MapExt.aspx?FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "&MyPK=" + this.MyPK + "&DoType=" + item.No + "&RefNo=" + this.RefNo,
           item.Name);
        }
        this.Pub2.AddULEnd();
        this.Pub2.AddFieldSetEnd();
    }
    void mybtn_SaveAutoFullDtl_Click(object sender, EventArgs e)
    {
        Button btn = sender as Button;
        if (btn.ID.Contains("Cancel"))
        {
            this.Response.Redirect("MapExt.aspx?FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "&MyPK=" + this.MyPK + "&RefNo=" + this.RefNo, true);
            return;
        }

        MapExt myme = new MapExt(this.MyPK);
        MapDtls dtls = new MapDtls(myme.FK_MapData);
        string info = "";
        string error = "";
        foreach (MapDtl dtl in dtls)
        {
            TextBox tb = this.Pub2.GetTextBoxByID("TB_" + dtl.No);
            if (tb.Text.Trim() == "")
                continue;
            try
            {
                //DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(tb.Text);
                //MapAttrs attrs = new MapAttrs(dtl.No);
                //string err = "";
                //foreach (DataColumn dc in dt.Columns)
                //{
                //    if (attrs.IsExits(MapAttrAttr.KeyOfEn, dc.ColumnName) == false)
                //    {
                //        err += "<br>列" + dc.ColumnName + " From the table can not be   Attributes match .";
                //    }
                //}
                //if (err != "")
                //{
                //    error += " For ("+dtl.Name+") Inspection sql An error occurred while setting :"+err;
                //}
            }
            catch (Exception ex)
            {
                this.Alert("SQL ERROR: " + ex.Message);
                return;
            }
            info += "$" + dtl.No + ":" + tb.Text;
        }

        if (error != "")
        {
            this.Pub2.AddMsgOfWarning(" Setting error , Please correct :", error);
            return;
        }
        myme.Tag1 = info;
        myme.Update();
        this.Response.Redirect("MapExt.aspx?FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "&MyPK=" + this.MyPK + "&RefNo=" + this.RefNo, true);
    }
    void mybtn_SaveAutoFullM2M_Click(object sender, EventArgs e)
    {
        Button btn = sender as Button;
        if (btn.ID.Contains("Cancel"))
        {
            this.Response.Redirect("MapExt.aspx?FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "&MyPK=" + this.MyPK + "&RefNo=" + this.RefNo, true);
            return;
        }

        MapExt myme = new MapExt(this.MyPK);
        MapM2Ms m2ms = new MapM2Ms(myme.FK_MapData);
        string info = "";
        string error = "";
        foreach (MapM2M m2m in m2ms)
        {
            TextBox tb = this.Pub2.GetTextBoxByID("TB_" + m2m.NoOfObj);
            if (tb.Text.Trim() == "")
                continue;
            try
            {
                DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(tb.Text);
                string err = "";
                if (dt.Columns[0].ColumnName != "No")
                    err += "Column1 is not No.";
                if (dt.Columns[1].ColumnName != "Name")
                    err += "Column2 is not Name.";

                if (err != "")
                {
                    error += " For (" + m2m.Name + ") Inspection sql An error occurred while setting : Make sure that the correct order of the columns is the case whether the match ." + err;
                }
            }
            catch (Exception ex)
            {
                this.Alert("SQL ERROR: " + ex.Message);
                return;
            }
            info += "$" + m2m.NoOfObj + ":" + tb.Text;
        }

        if (error != "")
        {
            this.Pub2.AddMsgOfWarning(" Setting error , Please correct :", error);
            return;
        }
        myme.Tag2 = info;
        myme.Update();
        this.Response.Redirect("MapExt.aspx?FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "&MyPK=" + this.MyPK + "&RefNo=" + this.RefNo, true);
    }
    void mybtn_SaveAutoFullJilian_Click(object sender, EventArgs e)
    {
        Button btn = sender as Button;
        if (btn.ID.Contains("Cancel"))
        {
            this.Response.Redirect("MapExt.aspx?FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "&MyPK=" + this.MyPK + "&RefNo=" + this.RefNo, true);
            return;
        }

        MapExt myme = new MapExt(this.MyPK);
        MapAttrs attrs = new MapAttrs(myme.FK_MapData);
        string info = "";
        foreach (MapAttr attr in attrs)
        {
            if (attr.LGType == FieldTypeS.Normal)
                continue;

            if (attr.UIIsEnable == false)
                continue;

            TextBox tb = this.Pub2.GetTextBoxByID("TB_" + attr.KeyOfEn);
            if (tb.Text.Trim() == "")
                continue;

            try
            {
                DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(tb.Text);
                if (tb.Text.Contains("@Key") == false)
                    throw new Exception(" Lack @Key Parameters .");

                if (dt.Columns.Contains("No") == false || dt.Columns.Contains("Name") == false)
                    throw new Exception(" In your sql Form the formula must have No,Name Two columns , To bind the drop-down box .");
            }
            catch (Exception ex)
            {
                this.Alert("SQL ERROR: " + ex.Message);
                return;
            }
            info += "$" + attr.KeyOfEn + ":" + tb.Text;
        }
        myme.Tag = info;
        myme.Update();
        this.Alert(" Saved successfully .");
        //   this.Response.Redirect("MapExt.aspx?FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "&MyPK=" + this.MyPK + "&RefNo=" + this.RefNo, true);
    }
    public void Edit_PopVal()
    {
        this.Pub2.AddTable("border=0");
        MapExt me = null;
        if (this.MyPK == null)
        {
            me = new MapExt();
            this.Pub2.AddCaptionLeft(" New :" + this.Lab + "- Help Please see the Form Designer manual gallop ");
        }
        else
        {
            me = new MapExt(this.MyPK);
            this.Pub2.AddCaptionLeft(" Editor :" + this.Lab + "- Help Please see the Form Designer manual gallop ");
        }

        me.FK_MapData = this.FK_MapData;
        this.Pub2.AddTR();
        this.Pub2.AddTDTitle(" Project ");
        this.Pub2.AddTDTitle(" Collection ");
        this.Pub2.AddTDTitle(" Explanation ");
        this.Pub2.AddTREnd();

        this.Pub2.AddTR();
        this.Pub2.AddTD(" The role of field ");
        BP.Web.Controls.DDL ddl = new BP.Web.Controls.DDL();
        ddl.ID = "DDL_Oper";
        MapAttrs attrs = new MapAttrs(this.FK_MapData);
        foreach (MapAttr attr in attrs)
        {
            if (attr.UIVisible == false)
                continue;

            if (attr.UIIsEnable == false)
                continue;

            if (attr.UIContralType == UIContralType.TB)
            {
                ddl.Items.Add(new ListItem(attr.KeyOfEn + " - " + attr.Name, attr.KeyOfEn));
                continue;
            }
        }
        ddl.SetSelectItem(me.AttrOfOper);
        this.Pub2.AddTD(ddl);
        this.Pub2.AddTD(" Deal with pop Form fields .");
        this.Pub2.AddTREnd();

        this.Pub2.AddTR();
        this.Pub2.AddTD(" Setting Type ");
        this.Pub2.AddTDBegin();

        RadioButton rb = new RadioButton();
        rb.Text = " Custom URL";
        rb.ID = "RB_Tag_0";
        rb.GroupName = "sd";
        if (me.PopValWorkModel == 0)
            rb.Checked = true;
        else
            rb.Checked = false;
        this.Pub2.Add(rb);
        rb = new RadioButton();
        rb.ID = "RB_Tag_1";
        rb.Text = "ccform Internal ";
        rb.GroupName = "sd";
        if (me.PopValWorkModel == 1)
            rb.Checked = true;
        else
            rb.Checked = false;
        this.Pub2.Add(rb);
        this.Pub2.AddTDEnd();
        this.Pub2.AddTD(" If the custom URL, Only fill URL Field .");
        this.Pub2.AddTREnd();


        this.Pub2.AddTR();
        this.Pub2.AddTD("URL");
        TextBox tb = new TextBox();
        tb.ID = "TB_" + MapExtAttr.Doc;
        tb.Text = me.Doc;
        tb.Columns = 50;
        this.Pub2.AddTD("colspan=2", tb);
        this.Pub2.AddTREnd();

        this.Pub2.AddTR();
        this.Pub2.AddTD("colspan=3", "URL Fill Description : Please enter a pop-up window url, When the operator closes the return value will be set in the current control <br>Test URL:http://localhost/Flow/SDKFlowDemo/PopSelectVal.aspx.");
        this.Pub2.AddTREnd();


        this.Pub2.AddTR();
        this.Pub2.AddTD(" Packet SQL");
        tb = new TextBox();
        tb.ID = "TB_" + MapExtAttr.Tag1;
        tb.Text = me.Tag1;
        tb.Columns = 50;
        this.Pub2.AddTD("colspan=2", tb);
        this.Pub2.AddTREnd();

        this.Pub2.AddTR();
        this.Pub2.AddTD(" Data Sources SQL");
        tb = new TextBox();
        tb.ID = "TB_" + MapExtAttr.Tag2;
        tb.Text = me.Tag2;
        tb.Columns = 50;
        this.Pub2.AddTD("colspan=2", tb);
        this.Pub2.AddTREnd();
        this.Pub2.AddTREnd();

        #region  Select mode 
        this.Pub2.AddTR();
        this.Pub2.AddTD(" Select mode ");
        this.Pub2.AddTDBegin();

        rb = new RadioButton();
        rb.Text = " Multiple Choice ";
        rb.ID = "RB_Tag3_0";
        rb.GroupName = "dd";
        if (me.PopValSelectModel == 0)
            rb.Checked = true;
        else
            rb.Checked = false;
        this.Pub2.Add(rb);

        rb = new RadioButton();
        rb.ID = "RB_Tag3_1";
        rb.Text = " Multiple choice ";
        rb.GroupName = "dd";
        if (me.PopValSelectModel == 1)
            rb.Checked = true;
        else
            rb.Checked = false;
        this.Pub2.Add(rb);
        this.Pub2.AddTDEnd();
        this.Pub2.AddTD("");
        this.Pub2.AddTREnd();
        #endregion  Select mode 


        #region  Presentation 
        this.Pub2.AddTR();
        this.Pub2.AddTD(" Data source presentation ");
        this.Pub2.AddTDBegin();

        rb = new RadioButton();
        rb.Text = " Tabular form ";
        rb.ID = "RB_Tag4_0";
        rb.GroupName = "dsd";
        if (me.PopValShowModel == 0)
            rb.Checked = true;
        else
            rb.Checked = false;
        this.Pub2.Add(rb);

        rb = new RadioButton();
        rb.ID = "RB_Tag4_1";
        rb.Text = " Catalog way ";
        rb.GroupName = "dsd";
        if (me.PopValShowModel == 1)
            rb.Checked = true;
        else
            rb.Checked = false;
        this.Pub2.Add(rb);
        this.Pub2.AddTDEnd();
        this.Pub2.AddTD("");
        this.Pub2.AddTREnd();
        #endregion  Presentation 

        this.Pub2.AddTR();
        this.Pub2.AddTD(" The return value format ");
        ddl = new BP.Web.Controls.DDL();
        ddl.ID = "DDL_PopValFormat";
        ddl.BindSysEnum("PopValFormat");

        ddl.SetSelectItem(me.PopValFormat);

        this.Pub2.AddTD("colspan=2", ddl);
        this.Pub2.AddTREnd();
        this.Pub2.AddTREnd();

        this.Pub2.AddTRSum();
        Button btn = new Button();
        btn.ID = "BtnSave";
        btn.CssClass = "Btn";
        btn.Text = "Save";
        btn.Click += new EventHandler(btn_SavePopVal_Click);
        this.Pub2.AddTD("colspan=3", btn);
        this.Pub2.AddTREnd();
        this.Pub2.AddTableEnd();
    }
    public string EventName
    {
        get
        {
            string s = this.Request.QueryString["EventName"];
            return s;
        }
    }
    string temFile = "s@xa";
    public void Edit_InputCheck()
    {
        MapExt me = null;
        if (this.MyPK == null)
        {
            me = new MapExt();
            this.Pub2.AddFieldSet(" New :" + this.Lab);
        }
        else
        {
            me = new MapExt(this.MyPK);
            this.Pub2.AddFieldSet(" Editor :" + this.Lab);
        }
        me.FK_MapData = this.FK_MapData;
        temFile = me.Tag;

        this.Pub2.AddTable("border=0  width='70%' align=left ");
        MapAttr attr = new MapAttr(this.RefNo);
        this.Pub2.AddCaptionLeft(attr.KeyOfEn + " - " + attr.Name);
        this.Pub2.AddTR();
        this.Pub2.AddTD(" Library Source :");
        this.Pub2.AddTDBegin();

        System.Web.UI.WebControls.RadioButton rb = new System.Web.UI.WebControls.RadioButton();
        rb.Text = "ccflow System js Library .";
        rb.ID = "RB_0";
        rb.AutoPostBack = true;
        if (me.DoWay == 0)
            rb.Checked = true;
        else
            rb.Checked = false;
        rb.GroupName = "s";
        rb.CheckedChanged += new EventHandler(rb_CheckedChanged);
        this.Pub2.Add(rb);

        rb = new System.Web.UI.WebControls.RadioButton();
        rb.AutoPostBack = true;
        rb.Text = " I have a custom library .";
        rb.CheckedChanged += new EventHandler(rb_CheckedChanged);
        rb.GroupName = "s";
        rb.ID = "RB_1";
        rb.AutoPostBack = true;
        if (me.DoWay == 1)
            rb.Checked = true;
        else
            rb.Checked = false;
        this.Pub2.Add(rb);
        this.Pub2.AddTDEnd();
        this.Pub2.AddTREnd();

        this.Pub2.AddTR();
        this.Pub2.AddTDTitle("colspan=2", " Function List ");
        this.Pub2.AddTREnd();
        this.Pub2.AddTR();

        ListBox lb = new ListBox();
        lb.Attributes["width"] = "100%";
        lb.AutoPostBack = false;
        lb.ID = "LB1";
        this.Pub2.AddTD("colspan=2", lb);
        this.Pub2.AddTREnd();

        this.Pub2.AddTRSum();
        Button btn = new Button();
        btn.ID = "BtnSave";
        btn.CssClass = "Btn";
        btn.Text = "Save";
        btn.Click += new EventHandler(btn_SaveInputCheck_Click);

        this.Pub2.AddTD("colspan=1", btn);
        this.Pub2.AddTD("<a href='MapExt.aspx?FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "'> Return </a>");
        this.Pub2.AddTREnd();
        this.Pub2.AddTableEnd();
        this.Pub2.AddFieldSetEnd();
        rb_CheckedChanged(null, null);
    }
    void rb_CheckedChanged(object sender, EventArgs e)
    {
        string path = BP.Sys.SystemConfig.PathOfData + "\\JSLib\\";
        System.Web.UI.WebControls.RadioButton rb = this.Pub2.GetRadioButtonByID("RB_0"); // sender as System.Web.UI.WebControls.RadioButton;
        if (rb.Checked == false)
            path = BP.Sys.SystemConfig.PathOfDataUser + "\\JSLib\\";

        ListBox lb = this.Pub2.FindControl("LB1") as ListBox;
        lb.Items.Clear();
        lb.AutoPostBack = false;
        lb.SelectionMode = ListSelectionMode.Multiple;
        lb.Rows = 10;
        //lb.SelectedIndexChanged += new EventHandler(lb_SelectedIndexChanged);
        string file = temFile;
        if (string.IsNullOrEmpty(temFile) == false)
        {
            file = file.Substring(file.LastIndexOf('\\') + 4);
            file = file.Replace(".js", "");
        }
        else
        {
            file = "!!!";
        }

        MapExts mes = new MapExts();
        mes.Retrieve(MapExtAttr.FK_MapData, this.FK_MapData,
            MapExtAttr.AttrOfOper, this.OperAttrKey,
            MapExtAttr.ExtType, this.ExtType);

        string[] dirs = System.IO.Directory.GetDirectories(path);
        foreach (string dir in dirs)
        {
            string[] strs = Directory.GetFiles(dir);
            foreach (string s in strs)
            {
                if (s.Contains(".js") == false)
                    continue;

                ListItem li = new ListItem(s.Replace(path, "").Replace(".js", ""), s);
                if (s.Contains(file))
                    li.Selected = true;

                lb.Items.Add(li);
            }
        }
    }
    public void EditAutoFull_TB()
    {
        MapExt me = null;
        if (this.MyPK == null)
            me = new MapExt();
        else
            me = new MapExt(this.MyPK);

        me.FK_MapData = this.FK_MapData;

        this.Pub2.AddTable("border=0");
        this.Pub2.AddCaptionLeft(" New :" + this.Lab);
        this.Pub2.AddTR();
        this.Pub2.AddTDTitle(" Project ");
        this.Pub2.AddTDTitle(" Collection ");
        this.Pub2.AddTREnd();

        this.Pub2.AddTR();
        this.Pub2.AddTD(" Drop-down box ");
        BP.Web.Controls.DDL ddl = new BP.Web.Controls.DDL();
        ddl.ID = "DDL_Oper";
        MapAttrs attrs = new MapAttrs(this.FK_MapData);
        foreach (MapAttr attr in attrs)
        {
            if (attr.UIVisible == false)
                continue;

            if (attr.UIIsEnable == false)
                continue;

            if (attr.UIContralType == UIContralType.TB)
            {
                ddl.Items.Add(new ListItem(attr.KeyOfEn + " - " + attr.Name, attr.KeyOfEn));
                continue;
            }
        }
        ddl.SetSelectItem(me.AttrOfOper);
        this.Pub2.AddTD(ddl);
        this.Pub2.AddTREnd();

        this.Pub2.AddTR();
        this.Pub2.AddTDTitle("colspan=2", " Automatic filling SQL:");
        this.Pub2.AddTREnd();

        this.Pub2.AddTR();
        TextBox tb = new TextBox();
        tb.ID = "TB_Doc";
        tb.Text = me.Doc;
        tb.TextMode = TextBoxMode.MultiLine;
        tb.Rows = 5;
        tb.Columns = 80;
        this.Pub2.AddTD("colspan=2", tb);
        this.Pub2.AddTREnd();

        this.Pub2.AddTR();
        this.Pub2.AddTDTitle("colspan=2", " Keyword query SQL:");
        this.Pub2.AddTREnd();

        this.Pub2.AddTR();
        tb = new TextBox();
        tb.ID = "TB_Tag";
        tb.Text = me.Tag;
        tb.TextMode = TextBoxMode.MultiLine;
        tb.Rows = 5;
        tb.Columns = 80;
        this.Pub2.AddTD("colspan=2", tb);
        this.Pub2.AddTREnd();

        this.Pub2.AddTRSum();
        this.Pub2.AddTDBegin("colspan=2");

        Button btn = new Button();
        btn.CssClass = "Btn";
        btn.ID = "BtnSave";
        btn.Text = " Save ";
        btn.Click += new EventHandler(btn_SaveAutoFull_Click);
        this.Pub2.Add(btn);

        if (this.MyPK == null)
        {
        }
        else
        {
            this.Pub2.Add("<a href=\"MapExt.aspx?MyPK=" + this.MyPK + "&FK_MapData=" + this.FK_MapData + "&RefNo = " + this.RefNo + "&ExtType=" + this.ExtType + "&DoType=EditAutoJL\" > Cascading drop-down box </a>");
            this.Pub2.Add("-<a href=\"MapExt.aspx?MyPK=" + this.MyPK + "&FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "&RefNo=" + this.RefNo + "&DoType=EditAutoFullDtl\" > Filled from the table </a>");
            this.Pub2.Add("-<a href=\"MapExt.aspx?MyPK=" + this.MyPK + "&FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "&RefNo=" + this.RefNo + "&DoType=EditAutoFullM2M\" > Filled with many </a>");

        }
        this.Pub2.AddTDEnd();
        this.Pub2.AddTREnd();
        this.Pub2.AddTableEnd();
        #region  Output Case 

        this.Pub2.AddFieldSet(" Help ");
        this.Pub2.AddB("For oracle:");
        string sql = " Automatic filling SQL:<br>SELECT No as ~No~ , Name as ~Name~, Name as ~mingcheng~ FROM WF_Emp WHERE No LIKE '@Key%' AND ROWNUM<=15";
        sql += "<br> Keyword Search SQL:<br>SELECT No as ~No~ , Name as ~Name~, Name as ~mingcheng~ FROM WF_Emp WHERE No LIKE '@Key%'  ";
        this.Pub2.AddBR(sql.Replace("~", "\""));

        this.Pub2.AddB("<br>For sqlserver:");
        sql = " Automatic filling SQL:<br>SELECT TOP 15 No, Name , Name as mingcheng FROM WF_Emp WHERE No LIKE '@Key%'";
        sql += "<br> Keyword Search SQL:<br>SELECT  No, Name , Name as mingcheng FROM WF_Emp WHERE No LIKE '@Key%'";
        this.Pub2.AddBR(sql.Replace("~", "\""));

        this.Pub2.AddB("<br> Watch out :");
        this.Pub2.AddBR("1, Auto-complete textbox filled case :  Must have No,Name Two , It is used to display the following list of tips .");
        this.Pub2.AddBR("2, Set the appropriate number of records , To improve the efficiency of the system .");
        this.Pub2.AddBR("3,@Key  The system agreed keyword , That is, when the user enters a character after ccform This keyword is passed to the database query results returned to the user .");
        this.Pub2.AddBR("4, Other columns of the same form field names can be automatically populated , Pay attention to the case match .");
        this.Pub2.AddBR("5, Keyword Search sql Is used , Two-point text box that pops up when you query , If you press the auto-fill empty sql Calculate .");

        this.Pub2.AddFieldSetEnd();
        #endregion  Output Case 
    }
    public void EditAutoFull_DDL()
    {
        MapExt me = null;
        if (this.MyPK == null)
            me = new MapExt();
        else
            me = new MapExt(this.MyPK);

        me.FK_MapData = this.FK_MapData;

        this.Pub2.AddTable("align=left");
        this.Pub2.AddCaptionLeft(" New :" + this.Lab);
        this.Pub2.AddTR();
        this.Pub2.AddTDTitle(" Project ");
        this.Pub2.AddTDTitle(" Collection ");
        this.Pub2.AddTDTitle(" Explanation ");
        this.Pub2.AddTREnd();

        this.Pub2.AddTR();
        this.Pub2.AddTD(" Drop-down box ");
        BP.Web.Controls.DDL ddl = new BP.Web.Controls.DDL();
        ddl.ID = "DDL_Oper";
        MapAttrs attrs = new MapAttrs(this.FK_MapData);
        foreach (MapAttr attr in attrs)
        {
            if (attr.UIVisible == false)
                continue;

            if (attr.UIIsEnable == false)
                continue;

            if (attr.UIContralType == UIContralType.DDL)
            {
                ddl.Items.Add(new ListItem(attr.KeyOfEn + " - " + attr.Name, attr.KeyOfEn));
                continue;
            }
        }
        ddl.SetSelectItem(me.AttrOfOper);

        this.Pub2.AddTD(ddl);
        this.Pub2.AddTD(" Entry ");
        this.Pub2.AddTREnd();

        this.Pub2.AddTR();
        this.Pub2.AddTDTitle("colspan=3", " Automatic filling SQL:");
        this.Pub2.AddTREnd();

        this.Pub2.AddTR();
        TextBox tb = new TextBox();
        tb.ID = "TB_Doc";
        tb.Text = me.Doc;
        tb.TextMode = TextBoxMode.MultiLine;
        tb.Rows = 5;
        tb.Columns = 80;
        this.Pub2.AddTD("colspan=3", tb);
        this.Pub2.AddTREnd();

        this.Pub2.AddTRSum();
        Button btn = new Button();
        btn.CssClass = "Btn";
        btn.ID = "BtnSave";
        btn.Text = " Save ";
        btn.Click += new EventHandler(btn_SaveAutoFull_Click);
        this.Pub2.AddTD("colspan=2", btn);
        if (this.MyPK == null)
            this.Pub2.AddTD();
        else
            this.Pub2.AddTD("<a href=\"MapExt.aspx?MyPK=" + this.MyPK + "&FK_MapData=" + this.FK_MapData + "&RefNo = " + this.RefNo + "&ExtType=" + this.ExtType + "&DoType=EditAutoJL\" > Cascading drop-down box </a>-<a href=\"MapExt.aspx?MyPK=" + this.MyPK + "&FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "&RefNo=" + this.RefNo + "&DoType=EditAutoFullDtl\" > Filled from the table </a>");
        this.Pub2.AddTREnd();

        #region  Output Case 
        this.Pub2.AddTRSum();
        this.Pub2.Add("\n<TD class='BigDoc' valign=top colspan=3>");

        this.Pub2.AddFieldSet(" Filled with stories :");
        string sql = "SELECT dizhi as Addr, fuzeren as Manager FROM Prj_Main WHERE No = '@Key'";
        this.Pub2.Add(sql.Replace("~", "\""));
        this.Pub2.AddBR("<hr><b> Explanation :</b> According to the user current drop-down box to select instances （ Such as : Choose a project ） The other attributes of this instance on the controls ");
        this.Pub2.Add("（ Such as : Address Project , Person in charge .）");
        this.Pub2.AddBR("<b> Remark :</b><br>1. Only column names match the names of the fields in the form to automatically fill up .<br>2.sql Check out the line of data ,@Key  Is the value of the currently selected .");
        this.Pub2.AddFieldSetEnd();

        this.Pub2.AddTDEnd();
        this.Pub2.AddTREnd();
        this.Pub2.AddTableEnd();
        #endregion  Output Case 
    }
    public void Edit_ActiveDDL()
    {
        MapExt me = null;
        if (this.MyPK == null)
        {
            me = new MapExt();
            this.Pub2.AddFieldSet(" New :" + this.Lab);
        }
        else
        {
            me = new MapExt(this.MyPK);
            this.Pub2.AddFieldSet(" Editor :" + this.Lab);
        }
        me.FK_MapData = this.FK_MapData;

        this.Pub2.AddTable("border=0  width='300px' align=left ");
        this.Pub2.AddCaptionLeft(this.Lab);
        this.Pub2.AddTR();
        this.Pub2.AddTDTitle(" Project ");
        this.Pub2.AddTDTitle(" Collection ");
        this.Pub2.AddTDTitle(" Explanation ");
        this.Pub2.AddTREnd();

        this.Pub2.AddTR();
        this.Pub2.AddTD(" Main Menu ");
        BP.Web.Controls.DDL ddl = new BP.Web.Controls.DDL();
        ddl.ID = "DDL_Oper";
        MapAttrs attrs = new MapAttrs(this.FK_MapData);
        int num = 0;
        foreach (MapAttr attr in attrs)
        {
            if (attr.UIVisible == false)
                continue;

            if (attr.UIIsEnable == false)
                continue;

            if (attr.UIContralType == UIContralType.DDL)
            {
                num++;
                ddl.Items.Add(new ListItem(attr.KeyOfEn + " - " + attr.Name, attr.KeyOfEn));
                continue;
            }
        }
        ddl.SetSelectItem(me.AttrOfOper);

        this.Pub2.AddTD(ddl);
        this.Pub2.AddTD(" Entry ");
        this.Pub2.AddTREnd();

        this.Pub2.AddTR();
        this.Pub2.AddTD(" Reaction entry ");
        ddl = new BP.Web.Controls.DDL();
        ddl.ID = "DDL_Attr";
        foreach (MapAttr attr in attrs)
        {
            if (attr.UIVisible == false)
                continue;

            if (attr.UIIsEnable == false)
                continue;

            if (attr.UIContralType != UIContralType.DDL)
                continue;

            ddl.Items.Add(new ListItem(attr.KeyOfEn + " - " + attr.Name, attr.KeyOfEn));
        }
        ddl.SetSelectItem(me.AttrsOfActive);
        this.Pub2.AddTD(ddl);
        this.Pub2.AddTD(" To achieve the linkage effects menu ");
        this.Pub2.AddTREnd();

        this.Pub2.AddTR();
        this.Pub2.AddTDTitle("colspan=3", " Linkage way ");
        this.Pub2.AddTREnd();

        this.Pub2.AddTR();
        this.Pub2.Add("<TD class=BigDoc width='100%' colspan=3>");
        RadioButton rb = new RadioButton();
        rb.Text = " By sql Get linkage ";
        rb.GroupName = "sdr";
        rb.ID = "RB_0";
        if (me.DoWay == 0)
            rb.Checked = true;

        this.Pub2.AddFieldSet(rb);
        this.Pub2.Add(" In the text box below, enter a SQL, With the number , Label column , Slave used to bind the drop-down box .");
        this.Pub2.Add(" Such as :SELECT No, Name FROM CN_City WHERE FK_SF = '@Key' ");
        this.Pub2.AddBR();
        TextBox tb = new TextBox();
        tb.ID = "TB_Doc";
        tb.Text = me.Doc;
        tb.Columns = 80;
        tb.CssClass = "TH";
        tb.TextMode = TextBoxMode.MultiLine;
        tb.Rows = 7;
        this.Pub2.Add(tb);
        this.Pub2.Add(" Explanation :@Key is ccflow Conventions keywords , Is passed over the main drop-down box value ");
        this.Pub2.AddFieldSetEnd();

        rb = new RadioButton();
        rb.Text = " By coding to identify acquisition ";
        rb.GroupName = "sdr";
        rb.Enabled = false;
        rb.ID = "RB_1";
        if (me.DoWay == 1)
            rb.Checked = true;

        this.Pub2.AddFieldSet(rb);
        this.Pub2.Add(" Main menu is the number of the first few numbers driven menu , Do not have the linkage content .");
        this.Pub2.Add(" Such as :  Drop-down box is the main provinces , Urban linkage menu .");
        this.Pub2.AddFieldSetEnd();

        this.Pub2.Add("</TD>");
        this.Pub2.AddTREnd();



        this.Pub2.AddTRSum();
        Button btn = new Button();
        btn.CssClass = "Btn";
        btn.ID = "BtnSave";
        btn.Text = "Save";
        btn.Click += new EventHandler(btn_SaveJiLian_Click);
        this.Pub2.AddTD("colspan=3", btn);
        this.Pub2.AddTREnd();
        this.Pub2.AddTableEnd();

        this.Pub2.AddFieldSetEnd();
    }
    void btn_SaveJiLian_Click(object sender, EventArgs e)
    {
        MapExt me = new MapExt();
        me.MyPK = this.MyPK;
        if (me.MyPK.Length > 2)
            me.RetrieveFromDBSources();
        me = (MapExt)this.Pub2.Copy(me);
        me.ExtType = this.ExtType;
        me.Doc = this.Pub2.GetTextBoxByID("TB_Doc").Text;
        me.AttrOfOper = this.Pub2.GetDDLByID("DDL_Oper").SelectedItemStringVal;
        me.AttrsOfActive = this.Pub2.GetDDLByID("DDL_Attr").SelectedItemStringVal;
        if (me.AttrsOfActive == me.AttrOfOper)
        {
            this.Alert(" Not be the same two projects .");
            return;
        }
        if (this.Pub2.GetRadioButtonByID("RB_1").Checked)
            me.DoWay = 1;
        else
            me.DoWay = 0;

        me.FK_MapData = this.FK_MapData;
        try
        {
            me.MyPK = this.FK_MapData + "_" + me.ExtType + "_" + me.AttrOfOper + "_" + me.AttrsOfActive;

            if (me.Doc.Contains("No") == false || me.Doc.Contains("Name") == false)
                throw new Exception(" In your sql Expressions , Must have No,Name  Also two columns .");
            //DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(me.Doc);
            //if (dt.Columns.Contains("Name") == false || dt.Columns.Contains("No") == false)
            //    throw new Exception(" In your sql Expressions , Must have No,Name  Also two columns .");
            me.Save();
        }
        catch (Exception ex)
        {
            this.Alert(ex.Message);
            return;
        }
        this.Response.Redirect("MapExt.aspx?FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "&RefNo = " + this.RefNo, true);
    }
    void btn_SaveInputCheck_Click(object sender, EventArgs e)
    {
        ListBox lb = this.Pub2.FindControl("LB1") as ListBox;

        //  Check the path .  Did not create it .
        string pathDir = BP.Sys.SystemConfig.PathOfDataUser + "\\JSLibData\\";
        if (Directory.Exists(pathDir) == false)
            Directory.CreateDirectory(pathDir);

        //  Delete existing data .
        MapExt me = new MapExt();
        me.Retrieve(MapExtAttr.FK_MapData, this.FK_MapData,
            MapExtAttr.ExtType, this.ExtType,
            MapExtAttr.AttrOfOper, this.OperAttrKey);

        foreach (ListItem li in lb.Items)
        {
            if (li.Selected == false)
                continue;

            me = (MapExt)this.Pub2.Copy(me);
            me.ExtType = this.ExtType;

            //  Property operations .
            me.AttrOfOper = this.OperAttrKey;
            //this.Pub2.GetDDLByID("DDL_Oper").SelectedItemStringVal;

            int doWay = 0;
            if (this.Pub2.GetRadioButtonByID("RB_0").Checked == false)
                doWay = 1;

            me.DoWay = doWay;
            me.Doc = BP.DA.DataType.ReadTextFile(li.Value);
            FileInfo info = new FileInfo(li.Value);
            me.Tag2 = info.Directory.Name;

            // Get the name of the function .
            string func = me.Doc;
            func = me.Doc.Substring(func.IndexOf("function") + 8);
            func = func.Substring(0, func.IndexOf("("));
            me.Tag1 = func.Trim();

            //  Check the path , Did not create it .
            FileInfo fi = new FileInfo(li.Value);
            me.Tag = li.Value;
            me.FK_MapData = this.FK_MapData;
            me.ExtType = this.ExtType;
            me.MyPK = this.FK_MapData + "_" + me.ExtType + "_" + me.AttrOfOper + "_" + me.Tag1;
            try
            {
                me.Insert();
            }
            catch
            {
                me.Update();
            }
        }

        #region  All the js  Files on a file inside .
        MapExts mes = new MapExts();
        mes.Retrieve(MapExtAttr.FK_MapData, this.FK_MapData,
            MapExtAttr.ExtType, this.ExtType);

        string js = "";
        foreach (MapExt me1 in mes)
        {
            js += "\r\n" + BP.DA.DataType.ReadTextFile(me1.Tag);
        }

        if (File.Exists(pathDir + "\\" + this.FK_MapData + ".js"))
            File.Delete(pathDir + "\\" + this.FK_MapData + ".js");

        BP.DA.DataType.WriteFile(pathDir + "\\" + this.FK_MapData + ".js", js);
        #endregion  All the js  Files on a file inside .


        this.Response.Redirect("MapExt.aspx?FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "&RefNo = " + this.RefNo, true);
    }
    void btn_SavePopVal_Click(object sender, EventArgs e)
    {
        MapExt me = new MapExt();
        me.MyPK = this.MyPK;
        if (me.MyPK.Length > 2)
            me.RetrieveFromDBSources();
        me = (MapExt)this.Pub2.Copy(me);
        me.ExtType = this.ExtType;
        me.Doc = this.Pub2.GetTextBoxByID("TB_Doc").Text;
        me.AttrOfOper = this.Pub2.GetDDLByID("DDL_Oper").SelectedItemStringVal;
        me.SetPara("PopValFormat", this.Pub2.GetDDLByID("DDL_PopValFormat").SelectedItemStringVal);

        RadioButton rb = this.Pub2.GetRadioButtonByID("RB_Tag_0");
        if (rb.Checked)
            me.PopValWorkModel = 0;
        else
            me.PopValWorkModel = 1;

        rb = this.Pub2.GetRadioButtonByID("RB_Tag3_0");
        if (rb.Checked)
            me.PopValSelectModel = 0;
        else
            me.PopValSelectModel = 1;

           rb = this.Pub2.GetRadioButtonByID("RB_Tag4_0");
        if (rb.Checked)
            me.PopValShowModel = 0;
        else
            me.PopValShowModel = 1;
        

        me.FK_MapData = this.FK_MapData;
        me.MyPK = this.FK_MapData + "_" + me.ExtType + "_" + me.AttrOfOper;
        me.Save();
        this.Response.Redirect("MapExt.aspx?FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "&RefNo = " + this.RefNo, true);
    }
    void btn_SaveAutoFull_Click(object sender, EventArgs e)
    {
        MapExt me = new MapExt();
        me.MyPK = this.MyPK;
        if (me.MyPK.Length > 2)
            me.RetrieveFromDBSources();

        me = (MapExt)this.Pub2.Copy(me);
        me.ExtType = this.ExtType;
        me.Doc = this.Pub2.GetTextBoxByID("TB_Doc").Text;
        me.AttrOfOper = this.Pub2.GetDDLByID("DDL_Oper").SelectedItemStringVal;
        me.FK_MapData = this.FK_MapData;
        me.MyPK = this.FK_MapData + "_" + me.ExtType + "_" + me.AttrOfOper;

        try
        {
            //DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(me.Doc);
            //if (string.IsNullOrEmpty(me.Tag) == false)
            //{
            //    dt = BP.DA.DBAccess.RunSQLReturnTable(me.Tag);
            //    if (dt.Columns.Contains("Name") == false || dt.Columns.Contains("No") == false)
            //        throw new Exception(" In your sql Expressions , Must have No,Name  Also two columns .");
            //}

            //if (this.ExtType == MapExtXmlList.TBFullCtrl)
            //{
            //    if (dt.Columns.Contains("Name") == false || dt.Columns.Contains("No") == false)
            //        throw new Exception(" In your sql Expressions , Must have No,Name  Also two columns .");
            //}

            //MapAttrs attrs = new MapAttrs(this.FK_MapData);
            //foreach (DataColumn dc in dt.Columns)
            //{
            //    if (dc.ColumnName.ToLower() == "no" || dc.ColumnName.ToLower() == "name")
            //        continue;

            //    if (attrs.Contains(MapAttrAttr.KeyOfEn, dc.ColumnName) == false)
            //        throw new Exception("@ The system does not find what you want to match the column (" + dc.ColumnName + "), Watch out : You specify the column names are case sensitive .");
            //}
            me.Save();
        }
        catch (Exception ex)
        {
            //this.Alert(ex.Message);
            this.AlertMsg_Warning("SQL Error ", ex.Message);
            return;
        }
        this.Response.Redirect("MapExt.aspx?FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "&RefNo=" + this.RefNo, true);
    }
    public void MapExtList(MapExts ens)
    {
        this.Pub2.AddTable("border=0 width='80%' align=left");
        this.Pub2.AddCaptionLeft("<a href='MapExt.aspx?FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "&DoType=New&RefNo=" + this.RefNo + "' ><img src='/WF/Img/Btn/New.gif' border=0/> New :" + this.Lab + "</a>");

        this.Pub2.AddTR();
        this.Pub2.AddTDTitle(" Type ");
        this.Pub2.AddTDTitle(" Description ");
        this.Pub2.AddTDTitle(" Field ");
        this.Pub2.AddTDTitle(" Delete ");
        this.Pub2.AddTREnd();
        foreach (MapExt en in ens)
        {
            MapAttr ma = new MapAttr();
            ma.MyPK = this.FK_MapData + "_" + en.AttrOfOper;
            if (ma.RetrieveFromDBSources() == 0)
            {
                ma.Delete();
                continue;
            }

            this.Pub2.AddTR();
            this.Pub2.AddTD(en.ExtType);
            this.Pub2.AddTDA("MapExt.aspx?FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "&MyPK=" + en.MyPK + "&RefNo=" + this.RefNo, en.ExtDesc);

            this.Pub2.AddTD(en.AttrOfOper + " " + ma.Name);

            this.Pub2.AddTD("<a href=\"javascript:DoDel('" + en.MyPK + "','" + this.FK_MapData + "','" + this.ExtType + "');\" > Delete </a>");
            this.Pub2.AddTREnd();
        }
        this.Pub2.AddTableEndWithBR();
    }
    public void MapJS(MapExts ens)
    {
        this.Pub2.AddTable("border=0 width=90% align=left");
        this.Pub2.AddCaptionLeft(" Script validation ");
        this.Pub2.AddTR();
        this.Pub2.AddTDTitle(" Field ");
        this.Pub2.AddTDTitle(" Type ");
        this.Pub2.AddTDTitle(" Chinese name verification function ");
        this.Pub2.AddTDTitle(" Show ");
        this.Pub2.AddTDTitle(" Operating ");
        this.Pub2.AddTREnd();

        MapAttrs attrs = new MapAttrs(this.FK_MapData);
        foreach (MapAttr attr in attrs)
        {
            if (attr.UIVisible == false)
                continue;

            MapExt myEn = null;
            foreach (MapExt en in ens)
            {
                if (en.AttrOfOper == attr.KeyOfEn)
                {
                    myEn = en;
                    break;
                }
            }

            if (myEn == null)
            {
                this.Pub2.AddTRTX();
                this.Pub2.AddTD(attr.KeyOfEn + "-" + attr.Name);
                this.Pub2.AddTD("None");
                this.Pub2.AddTD("None");
                this.Pub2.AddTD("None");
                this.Pub2.AddTDA("MapExt.aspx?FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "&RefNo=" + attr.MyPK + "&OperAttrKey=" + attr.KeyOfEn + "&DoType=New", "<img src='/WF/Img/Btn/Edit.gif' border=0/> Editor ");
                this.Pub2.AddTREnd();
            }
            else
            {
                this.Pub2.AddTRTX();
                this.Pub2.AddTD(attr.KeyOfEn + "-" + attr.Name);

                if (myEn.DoWay == 0)
                    this.Pub2.AddTD(" System functions ");
                else
                    this.Pub2.AddTD(" Custom Functions ");

                string file = myEn.Tag;
                file = file.Substring(file.LastIndexOf('\\') + 4);
                file = file.Replace(".js", "");

                this.Pub2.AddTDA("MapExt.aspx?FK_MapData=" + this.FK_MapData + "&ExtType=" + this.ExtType + "&MyPK=" + myEn.MyPK + "&RefNo=" + attr.MyPK + "&OperAttrKey=" + attr.KeyOfEn, file);

                this.Pub2.AddTD(myEn.Tag2 + "=" + myEn.Tag1 + "(this);");

                this.Pub2.AddTD("<a href=\"javascript:DoDel('" + myEn.MyPK + "','" + this.FK_MapData + "','" + this.ExtType + "');\" ><img src='/WF/Img/Btn/Delete.gif' border=0/> Delete </a>");
                this.Pub2.AddTREnd();
            }
        }
        this.Pub2.AddTableEnd();
    }


}