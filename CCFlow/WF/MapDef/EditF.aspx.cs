using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using BP.Web.Controls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BP.Sys;
using BP.En;
using BP.Web;
using BP.Web.UC;
using BP.DA;
namespace CCFlow.WF.MapDef
{
    public partial class Comm_MapDef_EditF : BP.Web.WebPage
    {
        /// <summary>
        /// GroupField
        /// </summary>
        public int GroupField
        {
            get
            {
                string s = this.Request.QueryString["GroupField"];
                if (s == "" || s == null)
                    return 0;
                return int.Parse(s);
            }
        }
        /// <summary>
        ///  Execution Type 
        /// </summary>
        public new string DoType
        {
            get
            {
                return this.Request.QueryString["DoType"];
            }
        }
        public int FType
        {
            get
            {
                return int.Parse(this.Request.QueryString["FType"]);
            }
        }
        public string IDX
        {
            get
            {
                return this.Request.QueryString["IDX"];
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.MyPK == null)
                throw new Exception("Mypk==null");

            this.Title = " Edit field ";

            switch (this.DoType)
            {
                case "Add":
                    this.Add();
                    break;
                case "Edit":
                    MapAttr attr = new MapAttr();
                    attr.MyPK = this.RefNo;
                    attr.RetrieveFromDBSources();
                    attr.MyDataType = this.FType;
                    switch (attr.MyDataType)
                    {
                        case BP.DA.DataType.AppString:
                            this.EditString(attr);
                            break;
                        case BP.DA.DataType.AppDateTime:
                        case BP.DA.DataType.AppDate:
                        case BP.DA.DataType.AppInt:
                        case BP.DA.DataType.AppFloat:
                        case BP.DA.DataType.AppMoney:
                            this.EditInt(attr);
                            break;
                        case BP.DA.DataType.AppBoolean:
                            this.EditBool(attr);
                            break;
                        default:
                            throw new Exception(" The type considered " + this.FType);
                    }
                    break;
                default:
                    break;
            }
        }
        public void Add()
        {
            MapAttr attr = new MapAttr();
            attr.MyDataType = this.FType;
            attr.FK_MapData = this.MyPK;
            attr.UIIsEnable = true;
            switch (this.FType)
            {
                case DataType.AppString:
                    this.EditString(attr);
                    break;
                case DataType.AppInt:
                case DataType.AppDateTime:
                case DataType.AppDate:
                case DataType.AppFloat:
                case DataType.AppMoney:
                    this.EditInt(attr);
                    break;
                case DataType.AppBoolean:
                    this.EditBool(attr);
                    break;
                default:
                    break;
            }
        }
        int idx = 1;
        public void EditBeforeAdd(MapAttr mapAttr)
        {
            this.Pub1.AddTable();
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("ID");
            this.Pub1.AddTDTitle(" Project ");
            this.Pub1.AddTDTitle(" Collection ");
            this.Pub1.AddTDTitle(" Remark ");
            this.Pub1.AddTREnd();

            if (mapAttr.IsTableAttr)
            {
                /* if here is table attr, It's will let use can change data type. */
                this.Pub1.AddTR();
                this.Pub1.AddTDIdx(idx++);
                this.Pub1.AddTD(" Change the data type ");
                DDL ddlType = new DDL();
                ddlType.ID = "DDL_DTType";
                BP.Sys.XML.SysDataTypes xmls = new BP.Sys.XML.SysDataTypes();
                xmls.RetrieveAll();
                ddlType.Bind(xmls, "No", "Name");
                ddlType.SetSelectItem(mapAttr.MyDataTypeS);

                ddlType.AutoPostBack = true;
                ddlType.SelectedIndexChanged += new EventHandler(ddlType_SelectedIndexChanged);

                this.Pub1.AddTD(ddlType);
                this.Pub1.AddTD("");
                this.Pub1.AddTREnd();
            }

            this.Pub1.AddTR1();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Fields Chinese name ");
            TB tb = new TB();
            tb.ID = "TB_Name";
            tb.Text = mapAttr.Name;
            tb.Attributes["width"] = "100%";

            this.Pub1.AddTD(tb);
            this.Pub1.AddTD("");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" English name field ");
            tb = new TB();
            tb.ID = "TB_KeyOfEn";
            tb.Text = mapAttr.KeyOfEn;
            if (this.RefNo != null)
                tb.Enabled = false;

            tb.Attributes["onkeyup"] = "return IsDigit(this);";
            this.Pub1.AddTD(tb);

            if (string.IsNullOrEmpty(mapAttr.KeyOfEn))
                this.Pub1.AddTD(" Letter / Digital / Underline combination ");
            else
                this.Pub1.AddTD("<a href=\"javascript:clipboardData.setData('Text','" + mapAttr.KeyOfEn + "');alert(' Already copy To the paste version ');\" ><img src='../Img/Btn/Copy.gif' class='ICON' /> Copy the field names </a></TD>");

            this.Pub1.AddTREnd();


            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Defaults ");
            tb = new TB();
            tb.ID = "TB_DefVal";
            tb.Text = mapAttr.DefValReal;


            switch (this.FType)
            {
                case BP.DA.DataType.AppDouble:
                case BP.DA.DataType.AppInt:
                case BP.DA.DataType.AppFloat:
                    this.Pub1.AddTDNum(tb);
                    tb.ShowType = TBType.Num;
                    tb.Text = mapAttr.DefVal;
                    if (tb.Text == "")
                        tb.Text = "0";
                    break;
                case BP.DA.DataType.AppMoney:
                case BP.DA.DataType.AppRate:
                    this.Pub1.AddTDNum(tb);
                    tb.ShowType = TBType.Moneny;
                    break;
                default:
                    this.Pub1.AddTD(tb);
                    break;
            }

            tb.ShowType = mapAttr.HisTBType;
            switch (this.FType)
            {
                case DataType.AppDateTime:
                case DataType.AppDate:
                    CheckBox cb = new CheckBox();
                    cb.Text = " By default the current system date ";
                    cb.ID = "CB_DefVal";
                    if (mapAttr.DefValReal == "@RDT")
                        cb.Checked = true;
                    else
                        cb.Checked = false;
                    cb.AutoPostBack = true;
                    cb.CheckedChanged += new EventHandler(cb_CheckedChanged_rdt);
                    this.Pub1.AddTD(cb);
                    break;
                case DataType.AppString:
                    DDL ddl = new DDL();
                    ddl.AutoPostBack = true;

                    BP.WF.XML.DefVals vals = new BP.WF.XML.DefVals();
                    vals.Retrieve("Lang", WebUser.SysLang);
                    foreach (BP.WF.XML.DefVal def in vals)
                        ddl.Items.Add(new ListItem(def.Name, def.Val));

                    //ddl.Items.Add(new ListItem(" Select System agreed defaults ", ""));
                    //ddl.Items.Add(new ListItem(" Operator number ", "@WebUser.No"));
                    //ddl.Items.Add(new ListItem(" Operator Name ", "@WebUser.Name"));
                    //ddl.Items.Add(new ListItem(" Membership department number ", "@WebUser.FK_Dept"));
                    //ddl.Items.Add(new ListItem(" Membership department name ", "@WebUser.FK_DeptName"));

                    //ddl.Items.Add(new ListItem(" The current date -1", "@yyyy年mm月dd日"));
                    //ddl.Items.Add(new ListItem(" The current date -2", "@yy年mm月dd日"));

                    //ddl.Items.Add(new ListItem(" Current year ", "@FK_ND"));
                    //ddl.Items.Add(new ListItem(" Current month ", "@FK_YF"));

                    ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged_DefVal);
                    ddl.SetSelectItem(mapAttr.DefValReal);
                    ddl.ID = "DDL_SelectDefVal";
                    this.Pub1.AddTD(ddl);
                    break;
                default:
                    this.Pub1.AddTD("&nbsp;");
                    break;
            }
            this.Pub1.AddTREnd();

            #region  Can be empty .
            switch (this.FType)
            {
                case BP.DA.DataType.AppDouble:
                case BP.DA.DataType.AppInt:
                case BP.DA.DataType.AppFloat:
                case BP.DA.DataType.AppMoney:
                case BP.DA.DataType.AppRate:
                    idx++;
                    this.Pub1.AddTR();
                    this.Pub1.AddTDIdx(idx);
                    this.Pub1.AddTD(" Can be empty ");
                    DDL ddlIsNull = new DDL();
                    ddlIsNull.Items.Add(new ListItem(" Can not be empty , The default value is calculated in accordance with .", "0"));
                    ddlIsNull.Items.Add(new ListItem(" Can be empty , Regardless of the default value .", "1"));
                    ddlIsNull.ID = "DDL_IsNull";

                    if (mapAttr.MinLen == 0)
                        ddlIsNull.SetSelectItem(0);
                    else
                        ddlIsNull.SetSelectItem(1);

                    this.Pub1.AddTD("colspan=2", ddlIsNull);
                    this.Pub1.AddTREnd();
                    break;
                default:
                    break;
            }
            #endregion  Can be empty .


            RadioButton rb = new RadioButton();
            if (MapData.IsEditDtlModel == false)
            {
                //this.Pub1.AddTR();
                //this.Pub1.AddTD(" Is visible on the screen ");
                //this.Pub1.Add("<TD>");
                //rb = new RadioButton();
                //rb.ID = "RB_UIVisible_0";
                //rb.Text = "不 可 见";
                //rb.GroupName = "s1";
                //if (mapAttr.UIVisible)
                //    rb.Checked = false;
                //else
                //    rb.Checked = true;
                //this.Pub1.Add(rb);

                //rb = new RadioButton();
                //rb.ID = "RB_UIVisible_1";
                //rb.Text = " Visible  ";
                //rb.GroupName = "s1";

                //if (mapAttr.UIVisible)
                //    rb.Checked = true;
                //else
                //    rb.Checked = false;
                //this.Pub1.Add(rb);
                //this.Pub1.Add("</TD>");
                //this.Pub1.AddTD(" Controls whether displayed on the page ");
                //this.Pub1.AddTREnd();
            }

            this.Pub1.AddTR1();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Can edit ");
            this.Pub1.Add("<TD>");


            rb = new RadioButton();
            rb.ID = "RB_UIIsEnable_0";
            rb.Text = " Not editable ";
            rb.GroupName = "s";
            rb.Checked = !mapAttr.UIIsEnable;

            this.Pub1.Add(rb);
            rb = new RadioButton();
            rb.ID = "RB_UIIsEnable_1";
            rb.Text = " Editable ";
            rb.GroupName = "s";
            rb.Checked = mapAttr.UIIsEnable;

            this.Pub1.Add(rb);
            this.Pub1.Add("</TD>");
            this.Pub1.AddTD("");
            this.Pub1.AddTREnd();

            #region  Whether the interface is visible 
            this.Pub1.AddTR1();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Whether the interface is visible "); // Whether the interface is visible 
            this.Pub1.AddTDBegin();
            rb = new RadioButton();
            rb.ID = "RB_UIVisible_0";
            rb.Text = " Invisible "; //  Interface is not visible 
            rb.GroupName = "sa3";
            if (mapAttr.UIVisible)
                rb.Checked = false;
            else
                rb.Checked = true;

            this.Pub1.Add(rb);
            if (mapAttr.IsTableAttr)
                rb.Enabled = false;

            rb = new RadioButton();
            rb.ID = "RB_UIVisible_1";
            rb.Text = " Interface is visible "; //  Interface is visible ;
            rb.GroupName = "sa3";

            if (mapAttr.UIVisible)
                rb.Checked = true;
            else
                rb.Checked = false;

            if (mapAttr.IsTableAttr)
                rb.Enabled = false;

            this.Pub1.Add(rb);
            this.Pub1.AddTDEnd();

            this.Pub1.AddTD(" Was hidden field is not visible .");
            //   this.Pub1.AddTD(" It is visible in the control of the interface in the form ");
            this.Pub1.AddTREnd();
            #endregion  Whether the interface is visible 

        }

        void ddl_SelectedIndexChanged_DefVal(object sender, EventArgs e)
        {
            this.Pub1.GetTBByID("TB_DefVal").Text = this.Pub1.GetDDLByID("DDL_SelectDefVal").SelectedItemStringVal;
        }

        void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            MapAttr attr = new MapAttr(this.RefNo);
            attr.MyDataTypeS = this.Pub1.GetDDLByID("DDL_DTType").SelectedItemStringVal;
            attr.Update();
            this.Response.Redirect("EditF.aspx?DoType=" + this.DoType + "&MyPK=" + this.MyPK + "&RefNo=" + this.RefNo + "&FType=" + attr.MyDataType + "&GroupField=" + this.GroupField, true);
            // this.Response.Redirect(this.Request.RawUrl, true);
        }
        public void EditBeforeEnd(MapAttr mapAttr)
        {

            #region  The combined number of cells 
            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" The combined number of cells ");
            DDL ddl1 = new DDL();
            ddl1.ID = "DDL_ColSpan";
            for (int i = 1; i < 12; i++)
            {
                ddl1.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            ddl1.SetSelectItem(mapAttr.ColSpan.ToString());
            this.Pub1.AddTD(ddl1);

            this.Pub1.AddTD(" Effective form for fools ");
            this.Pub1.AddTREnd();
            #endregion  The combined number of cells 

            #region  Field Grouping 
            this.Pub1.AddTR1();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Field Grouping ");
            DDL ddlGroup = new DDL();
            ddlGroup.ID = "DDL_GroupID";
            GroupFields gfs = new GroupFields(mapAttr.FK_MapData);
            ddlGroup.Bind(gfs, GroupFieldAttr.OID, GroupFieldAttr.Lab);
            if (mapAttr.GroupID == 0)
                mapAttr.GroupID = this.GroupField;

            ddlGroup.SetSelectItem(mapAttr.GroupID);

            this.Pub1.AddTD("colspan=3", ddlGroup);
            this.Pub1.AddTREnd();
            #endregion  Field Grouping 

            #region  Whether it is a digital signature field 
            if (mapAttr.UIIsEnable == false && mapAttr.MyDataType == DataType.AppString && mapAttr.LGType == FieldTypeS.Normal)
            {
                this.Pub1.AddTR();
                this.Pub1.AddTDIdx(idx++);

                DDL ddl = new DDL();
                ddl.ID = "DDL_SignType";
                ddl.Items.Add(new ListItem("无", "0"));
                ddl.Items.Add(new ListItem(" Pictures signature ", "1"));
                ddl.Items.Add(new ListItem("CA Signature ", "2"));
                ddl.SetSelectItem((int)mapAttr.SignType);


                //CheckBox cb = new CheckBox();
                //cb.ID = "CB_IsSigan";
                //cb.Text = " Whether it is a digital signature field ";
                //cb.Checked = mapAttr.IsSigan;

                this.Pub1.AddTD("colspan=2", ddl);
                if (mapAttr.SignType == SignType.CA)
                {
                    TB sigan = new TB();
                    sigan.ID = "TB_SiganField";
                    sigan.Text = mapAttr.Para_SiganField;
                    this.Pub1.AddTD(sigan);
                }
                else if (mapAttr.SignType == SignType.Pic)
                {
                    DDL ddlPic = new DDL();
                    ddlPic.ID = "DDL_PicType";
                    ddlPic.Items.Add(new ListItem(" Automatic signature ", "0"));
                    ddlPic.Items.Add(new ListItem(" Manual signature ", "1"));
                    ddlPic.SetSelectItem((int)mapAttr.PicType);
                    this.Pub1.AddTD(ddlPic);
                }
                else
                    this.Pub1.AddTD();
                this.Pub1.AddTREnd();
            }
            #endregion  Field Grouping 

            this.Pub1.AddTRSum();
            this.Pub1.Add("<TD colspan=4 align=center>");
            Button btn = new Button();
            btn.ID = "Btn_Save";
            btn.CssClass = "Btn";
            btn.Text = "  Save  ";
            btn.Click += new EventHandler(btn_Save_Click);
            this.Pub1.Add(btn);

            btn = new Button();
            btn.ID = "Btn_SaveAndClose";
            btn.CssClass = "Btn";
            btn.Text = " Save and Close "; // " Save and Close ";
            btn.Click += new EventHandler(btn_Save_Click);
            this.Pub1.Add(btn);

            btn = new Button();
            btn.CssClass = "Btn";
            btn.ID = "Btn_SaveAndNew";
            btn.Text = " Save New "; // " Save New ";
            btn.Click += new EventHandler(btn_Save_Click);
            this.Pub1.Add(btn);

            if (this.RefNo != null)
            {
                //btn = new Button();
                //btn.ID = "Btn_AutoFull";
                //btn.Text =" Extended Settings ";
                //btn.Attributes["onclick"] = "javascript:WinOpen('AutoFull.aspx?RefNo=" + this.RefNo + "&FK_MapData=" + mapAttr.FK_MapData + "',''); return false;";

                //this.Pub1.Add("<input type=button class=Btn value=' Extended Settings ' onclick=\"javascript:WinOpen('AutoFull.aspx?RefNo=" + this.RefNo + "&FK_MapData=" + mapAttr.FK_MapData + "',''); return false;\" />");
                this.Pub1.Add("<input type=button class=Btn value=' Extended Settings ' onclick=\"javascript:EUIWinOpen('AutoFull.aspx?RefNo=" + this.RefNo + "&FK_MapData=" + mapAttr.FK_MapData + "','Extended Settings'); return false;\" />");

                if (mapAttr.HisEditType == EditType.Edit)
                {
                    btn = new Button();
                    btn.ID = "Btn_Del";
                    btn.CssClass = "Btn";
                    btn.Text = " Delete ";
                    btn.Click += new EventHandler(btn_Save_Click);
                    btn.Attributes["onclick"] = " return confirm(' You acknowledge that you ?');";
                    this.Pub1.Add(btn);
                }

                string myUrl = "EleBatch.aspx?KeyOfEn=" + mapAttr.KeyOfEn + "&FK_MapData=" + mapAttr.FK_MapData + "&EleType=MapAttr";
                this.Pub1.Add("<a href='" + myUrl + "' target='M" + mapAttr.KeyOfEn + "' ><img src='../Img/Btn/Apply.gif' border=0> Batch </a>");

            }

            string url = "Do.aspx?DoType=AddF&MyPK=" + mapAttr.FK_MapData + "&IDX=" + mapAttr.IDX;
            btn = new Button();
            btn.ID = "Btn_New";
            btn.CssClass = "Btn";
            btn.Text = " New ";
            btn.Click += new EventHandler(btn_Click);
            this.Pub1.Add(btn);

            btn = new Button();
            btn.ID = "Btn_Back";
            btn.CssClass = "Btn";
            btn.Text = " Return ";
            btn.Click += new EventHandler(btn_Click);
            this.Pub1.Add(btn);

            this.Pub1.AddTDEnd();
            this.Pub1.AddTREnd();
            this.Pub1.AddTableEndWithBR();
        }
        public void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.ID)
            {
                case "Btn_New":
                    MapAttr mapAttr = new MapAttr(this.RefNo);
                    string url = "Do.aspx?DoType=AddF&MyPK=" + mapAttr.FK_MapData + "&IDX=" + mapAttr.IDX + "&GroupField = " + this.GroupField;
                    this.Response.Redirect(url, true);
                    return;
                case "Btn_Back":
                    string url1 = "Do.aspx?DoType=AddF&MyPK=" + this.MyPK + "&GroupField = " + this.GroupField;
                    this.Response.Redirect(url1, true);
                    return;
                default:
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapAttr"></param>
        public void EditString(MapAttr mapAttr)
        {
            this.EditBeforeAdd(mapAttr);
            TB tb = new TB();
            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Minimum length ");
            tb = new TB();
            tb.ID = "TB_MinLen";
            tb.CssClass = "TBNum";
            tb.Text = mapAttr.MinLen.ToString();
            this.Pub1.AddTD(tb);
            this.Pub1.AddTD("");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR1();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" The maximum length ");
            tb = new TB();
            tb.ID = "TB_MaxLen";
            tb.CssClass = "TBNum";
            tb.Text = mapAttr.MaxLen.ToString();

            //DDL cb = new DDL();
            //cb.ID = "DDL_TBType";
            //cb.Items.Add(new ListItem(" Single-line text box ", "0"));
            //cb.Items.Add(new ListItem(" Multi-line text box ", "1"));
            //cb.Items.Add(new ListItem("Sina Edit box ", "2"));
            //cb.Items.Add(new ListItem("FCKEditer Edit box ", "3"));

            this.Pub1.AddTD(tb);

            DDL ddlBig = new DDL();
            ddlBig.ID = "DDL_TBModel";
            ddlBig.BindSysEnum("TBModel", mapAttr.TBModel);
            ddlBig.AutoPostBack = true;
            ddlBig.SelectedIndexChanged += new EventHandler(ddlBig_SelectedIndexChanged);
            this.Pub1.AddTD(ddlBig);

            //CheckBox cb = new CheckBox();
            //cb.CheckedChanged += new EventHandler(cb_CheckedChanged);
            //cb.ID = "CB_IsM";
            //cb.Text = this.ToE("IsBigDoc", " Whether the chunk of text ( Effective form for fools )");
            //cb.AutoPostBack = true;
            //if (mapAttr.MaxLen >= 3000)
            //{
            //    cb.Checked = true;
            //    tb.Enabled = false;
            //}
            //else
            //{
            //    cb.Checked = false;
            //    tb.Enabled = true;
            //}
            //this.Pub1.AddTD(cb);
            //if (mapAttr.IsTableAttr)
            //    cb.Enabled = false;

            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" The width of the text box ");
            tb = new TB();
            tb.ID = "TB_UIWidth";
            tb.CssClass = "TBNum";
            tb.Text = mapAttr.UIWidth.ToString();
            this.Pub1.AddTD(tb);
            this.Pub1.AddTDB(" Effective from the table ");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Height ");
            tb = new TB();
            tb.ID = "TB_UIHeight";
            tb.CssClass = "TBNum";
            tb.Text = mapAttr.UIHeight.ToString();
            this.Pub1.AddTD(tb);
            this.Pub1.AddTD("");
            this.Pub1.AddTREnd();
            this.EditBeforeEnd(mapAttr);
        }

        void ddlBig_SelectedIndexChanged(object sender, EventArgs e)
        {
            DDL ddl = this.Pub1.GetDDLByID("DDL_TBModel");
            if (ddl.SelectedItemIntVal != 0)
            {
                this.Pub1.GetTBByID("TB_MaxLen").Text = "4000";
                this.Pub1.GetTBByID("TB_UIHeight").Text = "390";
                this.Pub1.GetTBByID("TB_MaxLen").Enabled = false;
            }
            else
            {
                this.Pub1.GetTBByID("TB_MaxLen").Enabled = true;
                this.Pub1.GetTBByID("TB_MaxLen").Text = "400";
            }
        }
        void cb_CheckedChanged_rdt(object sender, EventArgs e)
        {
            CheckBox cb = this.Pub1.GetCBByID("CB_DefVal");
            if (cb.Checked)
            {
                this.Pub1.GetTBByID("TB_DefVal").Text = "@RDT";
            }
            else
            {
                this.Pub1.GetTBByID("TB_DefVal").Text = "";
            }
        }
        void cb_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = this.Pub1.GetCBByID("CB_IsM");
            if (cb.Checked)
            {
                this.Pub1.GetTBByID("TB_MaxLen").Enabled = false;
                this.Pub1.GetTBByID("TB_MaxLen").Text = "4000";
                this.Pub1.GetTBByID("TB_UIHeight").Text = "90";
            }
            else
            {
                this.Pub1.GetTBByID("TB_MaxLen").Enabled = true;
                this.Pub1.GetTBByID("TB_MaxLen").Text = "50";
            }
        }
        public void EditInt(MapAttr mapAttr)
        {
            this.EditBeforeAdd(mapAttr);

            TB tb = new TB();
            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" The width of the text box ");
            tb = new TB();
            tb.ID = "TB_UIWidth";
            tb.CssClass = "TBNum";
            tb.Text = mapAttr.UIWidth.ToString();
            this.Pub1.AddTD(tb);
            this.Pub1.AddTDB(" Effective from the table ");
            this.Pub1.AddTREnd();
            this.EditBeforeEnd(mapAttr);
        }
        public void EditBool(MapAttr mapAttr)
        {
            this.Pub1.AddTable();
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("ID"); //  Project 
            this.Pub1.AddTDTitle(" Project "); //  Project 
            this.Pub1.AddTDTitle(" Collection ");   // 值
            this.Pub1.AddTDTitle(" Description "); //  Description 
            this.Pub1.AddTREnd();

            if (mapAttr.IsTableAttr)
            {
                /* if here is table attr, It's will let use can change data type. */
                this.Pub1.AddTR();
                this.Pub1.AddTDIdx(idx++);
                this.Pub1.AddTD(" Change the data type ");
                DDL ddlType = new DDL();
                ddlType.ID = "DDL_DTType";
                BP.Sys.XML.SysDataTypes xmls = new BP.Sys.XML.SysDataTypes();
                xmls.RetrieveAll();
                ddlType.Bind(xmls, "No", "Name");
                ddlType.SetSelectItem(mapAttr.MyDataTypeS);

                ddlType.AutoPostBack = true;
                ddlType.SelectedIndexChanged += new EventHandler(ddlType_SelectedIndexChanged);

                this.Pub1.AddTD(ddlType);
                this.Pub1.AddTD("");
                this.Pub1.AddTREnd();
            }

            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Fields Chinese name ");
            TB tb = new TB();
            tb.ID = "TB_Name";
            tb.Text = mapAttr.Name;
            tb.Attributes["width"] = "100%";

            this.Pub1.AddTD(tb);
            this.Pub1.AddTD("");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" English name field ");
            tb = new TB();
            tb.ID = "TB_KeyOfEn";
            tb.Text = mapAttr.KeyOfEn;

            if (this.RefNo != null)
                tb.Enabled = false;

            this.Pub1.AddTD(tb);


            if (string.IsNullOrEmpty(mapAttr.KeyOfEn))
                this.Pub1.AddTD(" Letter / Digital / Underline combination ");
            else
                this.Pub1.AddTD("<a href=\"javascript:clipboardData.setData('Text','" + mapAttr.KeyOfEn + "');alert(' Already copy To the paste version ');\" ><img src='../Img/Btn/Copy.gif' class='ICON' /> Copy the field names </a></TD>");



            this.Pub1.AddTREnd();

            this.Pub1.AddTR1();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Defaults ");
            CheckBox cb = new CheckBox();
            cb.ID = "CB_DefVal";
            cb.Text = " Please select ";
            cb.Checked = mapAttr.DefValOfBool;

            this.Pub1.AddTD(cb);
            this.Pub1.AddTD("");
            this.Pub1.AddTREnd();



            this.Pub1.AddTR1();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Can edit ");
            this.Pub1.Add("<TD>");

            RadioButton rb = new RadioButton();
            rb.ID = "RB_UIIsEnable_0";
            rb.Text = " Not editable ";
            rb.GroupName = "s";
            rb.Checked = !mapAttr.UIIsEnable;
            this.Pub1.Add(rb);


            rb = new RadioButton();
            rb.ID = "RB_UIIsEnable_1";
            rb.Text = " Editable ";
            rb.GroupName = "s";
            rb.Checked = mapAttr.UIIsEnable;
            this.Pub1.Add(rb);

            this.Pub1.Add("</TD>");
            this.Pub1.AddTD();
            // this.Pub1.AddTD(this.ToE("IsReadonly", " Is read-only "));
            this.Pub1.AddTREnd();


            #region  Whether a separate line display 
            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Presentation "); // Presentation ;
            this.Pub1.AddTDBegin();
            rb = new RadioButton();
            rb.ID = "RB_UIIsLine_0";
            rb.Text = " Two display "; //  Two lines 
            rb.GroupName = "sa";
            if (mapAttr.UIIsLine)
                rb.Checked = false;
            else
                rb.Checked = true;

            this.Pub1.Add(rb);
            rb = new RadioButton();
            rb.ID = "RB_UIIsLine_1";
            rb.Text = " The entire line display "; // " Party ";
            rb.GroupName = "sa";

            if (mapAttr.UIIsLine)
                rb.Checked = true;
            else
                rb.Checked = false;

            this.Pub1.Add(rb);
            this.Pub1.AddTDEnd();

            this.Pub1.AddTD(" Effective form for fools ");

            //this.Pub1.AddTD(" It controls the display in the form of ");
            this.Pub1.AddTREnd();
            #endregion  Can edit 

            #region  Whether the interface is visible 
            this.Pub1.AddTR1();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Whether the interface is visible "); // Whether the interface is visible 
            this.Pub1.AddTDBegin();
            rb = new RadioButton();
            rb.ID = "RB_UIVisible_0";
            rb.Text = " Invisible "; //  Interface is not visible 
            rb.GroupName = "sa3";
            if (mapAttr.UIVisible)
                rb.Checked = false;
            else
                rb.Checked = true;

            this.Pub1.Add(rb);
            rb = new RadioButton();
            rb.ID = "RB_UIVisible_1";
            rb.Text = " Interface is visible "; //  Interface is visible ;
            rb.GroupName = "sa3";

            if (mapAttr.UIVisible)
                rb.Checked = true;
            else
                rb.Checked = false;

            this.Pub1.Add(rb);
            this.Pub1.AddTDEnd();

            this.Pub1.AddTD(" Was hidden field is not visible .");
            //this.Pub1.AddTD(" It is visible in the control of the interface in the form ");
            this.Pub1.AddTREnd();
            #endregion  Whether the interface is visible 

            this.EditBeforeEnd(mapAttr);
        }
        public void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                switch (btn.ID)
                {
                    case "Btn_Del":
                        this.Response.Redirect("Do.aspx?DoType=Del&MyPK=" + this.MyPK + "&RefNo=" + this.RefNo + "&GroupField = " + this.GroupField, true);
                        return;
                    default:
                        break;
                }

                MapAttr attr = new MapAttr();
                if (this.RefNo != null)
                {
                    attr.MyPK = this.RefNo;
                    try
                    {
                        attr.Retrieve();
                    }
                    catch
                    {
                        attr.CheckPhysicsTable();
                        attr.Retrieve();
                    }

                    attr = (MapAttr)this.Pub1.Copy(attr);
                    attr.GroupID = this.Pub1.GetDDLByID("DDL_GroupID").SelectedItemIntVal;
                    attr.ColSpan = this.Pub1.GetDDLByID("DDL_ColSpan").SelectedItemIntVal;
                    if (attr.UIIsEnable == false && attr.MyDataType == DataType.AppString)
                    {
                        try
                        {
                            attr.IsSigan = this.Pub1.GetCBByID("CB_IsSigan").Checked;
                        }
                        catch
                        {
                        }
                    }

                    switch (this.FType)
                    {
                        case DataType.AppBoolean:
                            attr.MyDataType = BP.DA.DataType.AppBoolean;
                            attr.DefValOfBool = this.Pub1.GetCBByID("CB_DefVal").Checked;
                            break;
                        case DataType.AppDateTime:
                        case DataType.AppDate:
                            attr.DefValReal = this.Pub1.GetTBByID("TB_DefVal").Text;
                            //if (this.Pub1.GetCBByID("CB_DefVal").Checked)
                            //    attr.DefValReal = "1";
                            //else
                            //    attr.DefValReal = "0";
                            break;
                        case DataType.AppString:
                            attr.UIBindKey = this.Pub1.GetDDLByID("DDL_TBModel").SelectedItemStringVal;
                            if (attr.TBModel == 2)
                            {
                                attr.MaxLen = 4000;
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    attr = (MapAttr)this.Pub1.Copy(attr);
                    attr.GroupID = this.Pub1.GetDDLByID("DDL_GroupID").SelectedItemIntVal;
                    attr.ColSpan = this.Pub1.GetDDLByID("DDL_ColSpan").SelectedItemIntVal;

                    MapAttrs attrS = new MapAttrs(this.MyPK);
                    int idx = 0;
                    foreach (MapAttr en in attrS)
                    {
                        idx++;
                        en.IDX = idx;
                        en.Update();
                        if (en.KeyOfEn == attr.KeyOfEn)
                            throw new Exception(" Field already exists  Key=" + attr.KeyOfEn);
                    }
                    if (this.IDX == null || this.IDX == "")
                        attr.IDX = 0;
                    else
                        attr.IDX = int.Parse(this.IDX) - 1;

                    attr.MyDataType = this.FType;
                    switch (this.FType)
                    {
                        case DataType.AppBoolean:
                            attr.MyDataType = BP.DA.DataType.AppBoolean;
                            attr.UIContralType = UIContralType.CheckBok;
                            attr.DefValOfBool = this.Pub1.GetCBByID("CB_DefVal").Checked;
                            break;
                        case DataType.AppString:
                            attr.UIBindKey = this.Pub1.GetDDLByID("DDL_TBModel").SelectedItemStringVal;
                            break;
                        default:
                            break;
                    }
                }

                //  Increase is empty ,  Valid for numeric fields .
                try
                {
                    attr.MinLen = this.Pub1.GetDDLByID("DDL_IsNull").SelectedItemIntVal;
                }
                catch
                {
                }

                // Digital Signatures .
                try
                {
                    // Signature Type .
                    attr.SignType = (SignType)this.Pub1.GetDDLByID("DDL_SignType").SelectedItemIntVal;

                    if (attr.SignType == SignType.Pic)
                        attr.PicType = (PicType)this.Pub1.GetDDLByID("DDL_PicType").SelectedItemIntVal;// Whether the automatic signature 
                    else if (attr.SignType == SignType.CA)
                        attr.Para_SiganField = this.Pub1.GetTBByID("TB_SiganField").Text;// Digital signature field .


                }
                catch
                {

                }


                // Preservation of digital signature .
                Response.Buffer = true;
                attr.FK_MapData = this.MyPK;
                attr.MyPK = this.RefNo;

                // Execution time update  Deal with mapdata Business logic calculations .
                MapData md = new MapData(attr.FK_MapData);
                md.Update();

                attr.Save();

                switch (btn.ID)
                {
                    case "Btn_SaveAndClose":
                        this.WinClose();
                        return;
                    case "Btn_SaveAndNew":
                        this.Response.Redirect("Do.aspx?DoType=AddF&MyPK=" + this.MyPK + "&IDX=" + this.IDX + "&GroupField=" + attr.GroupID, true);
                        return;
                    default:
                        break;
                }
                this.Response.Redirect("EditF.aspx?DoType=Edit&MyPK=" + this.MyPK + "&RefNo=" + attr.MyPK + "&FType=" + this.FType + "&GroupField=" + attr.GroupID, true);
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }
        public string GetCaption
        {

            get
            {
                if (this.DoType == "Add")
                    return " Add a new field guide  - <a href='Do.aspx?DoType=ChoseFType&GroupField=" + this.GroupField + "' >  Return type selection  </a> - " + " Editor ";
                else
                    return "<a href='Do.aspx?DoType=ChoseFType&MyPK=" + this.MyPK + "&RefNo=" + this.RefNo + "&GroupField=" + this.GroupField + "'> Return type selection </a> - " + " Editor ";
            }
        }
    }

}