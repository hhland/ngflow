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
namespace CCFlow.WF.MapDef
{
    public partial class Comm_MapDef_EditEnum : BP.Web.WebPage
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
        public string FType
        {
            get
            {
                return this.Request.QueryString["FType"];
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
            //this.Response.Write(this.Request.RawUrl);
            this.Title = " Editing enumerated type "; // " Editing enumerated type ";
            MapAttr attr = null;

            if (this.RefNo == null)
            {
                attr = new MapAttr();
                string enumKey = this.Request.QueryString["EnumKey"];
                if (enumKey != null)
                {
                    SysEnumMain se = new SysEnumMain(enumKey);
                    attr.KeyOfEn = enumKey;
                    attr.UIBindKey = enumKey;
                    attr.Name = se.Name;
                    attr.Name = se.Name;
                }
            }
            else
            {
                attr = new MapAttr(this.RefNo);
            }
            BindEnum(attr);
        }
        int idx = 1;
        public void BindEnum(MapAttr mapAttr)
        {
            this.Pub1.AddTable();
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("ID");
            this.Pub1.AddTDTitle(" Project ");
            this.Pub1.AddTDTitle(" Collection ");
            this.Pub1.AddTDTitle(" Explanation ");
            this.Pub1.AddTREnd();

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
            if (this.RefNo != null)
            {
                this.Pub1.AddTD(mapAttr.KeyOfEn);
            }
            else
            {
                tb = new TB();
                tb.ID = "TB_KeyOfEn";
                tb.Text = mapAttr.KeyOfEn;
                this.Pub1.AddTD(tb);
            }

            if (string.IsNullOrEmpty(mapAttr.KeyOfEn))
                this.Pub1.AddTD(" Letter / Digital / Underline combination ");
            else
                this.Pub1.AddTD("<a href=\"javascript:clipboardData.setData('Text','" + mapAttr.KeyOfEn + "');alert(' Already copy To the paste version ');\" ><img src='../Img/Btn/Copy.gif' class='ICON' /> Copy the field names </a></TD>");

            // this.Pub1.AddTDTitle("&nbsp;");
            //this.Pub1.AddTD(" Do not start with a number , Do Chinese .");
            this.Pub1.AddTREnd();

         

            this.Pub1.AddTR1();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Defaults ");

            DDL ddl = new DDL();
            ddl.ID = "DDL";
            ddl.BindSysEnum(mapAttr.UIBindKey);
            ddl.SetSelectItem(mapAttr.DefVal);
            this.Pub1.AddTD(ddl);
            this.Pub1.AddTD("<a href='SysEnum.aspx?RefNo=" + mapAttr.UIBindKey+ "'> Editor </a>");
            this.Pub1.AddTREnd();


            #region  Can edit 
            this.Pub1.AddTR1();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Can edit ");
            this.Pub1.AddTDBegin();
            RadioButton rb = new RadioButton();
            rb.ID = "RB_UIIsEnable_0";
            rb.Text = " Not editable ";
            rb.GroupName = "s";
            if (mapAttr.UIIsEnable)
                rb.Checked = false;
            else
                rb.Checked = true;

            this.Pub1.Add(rb);
            rb = new RadioButton();
            rb.ID = "RB_UIIsEnable_1";
            rb.Text = " Editable ";
            rb.GroupName = "s";

            if (mapAttr.UIIsEnable)
                rb.Checked = true;
            else
                rb.Checked = false;

            this.Pub1.Add(rb);
            this.Pub1.AddTDEnd();

            this.Pub1.AddTD("");
            this.Pub1.AddTREnd();
            #endregion  Can edit 


            #region  Show controls 
            this.Pub1.AddTR1();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Control Types ");
            this.Pub1.AddTDBegin();
             rb = new RadioButton();
             rb.ID = "RB_Ctrl_0";
            rb.Text = " Drop-down box ";
            rb.GroupName = "Ctrl";
            if (mapAttr.UIContralType == UIContralType.DDL)
                rb.Checked = true;
            else
                rb.Checked = false;

            this.Pub1.Add(rb);
            rb = new RadioButton();
            rb.ID = "RB_Ctrl_1";
            rb.Text = " Radio buttons ";
            rb.GroupName = "Ctrl";

            if (mapAttr.UIContralType == UIContralType.DDL)
                rb.Checked = false;
            else
                rb.Checked = true;
            this.Pub1.Add(rb);
            this.Pub1.AddTDEnd();

            this.Pub1.AddTD("");
            this.Pub1.AddTREnd();
            #endregion  Show controls 


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
            this.Pub1.AddTREnd();
            #endregion  Whether the interface is visible 


            #region  The combined number of cells 
            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" The combined number of cells ");
            ddl = new DDL();
            ddl.ID = "DDL_ColSpan";
            for (int i = 1; i < 12; i++)
            {
                ddl.Items.Add(new ListItem(i.ToString(),i.ToString()));
            }
            ddl.SetSelectItem(mapAttr.ColSpan.ToString());
            this.Pub1.AddTD(ddl);
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
            this.Pub1.AddTD(ddlGroup);
            this.Pub1.AddTD(" Modify group membership ");
            this.Pub1.AddTREnd();
            #endregion  Field Grouping 

            this.Pub1.AddTRSum();
            this.Pub1.Add("<TD colspan=4 >");
            Button btn = new Button();
            btn.ID = "Btn_Save";
            btn.CssClass = "Btn";
            btn.Text ="  Save  ";
            btn.Click += new EventHandler(btn_Save_Click);
            this.Pub1.Add(btn);

            btn = new Button();
            btn.ID = "Btn_SaveAndClose";
            btn.CssClass = "Btn";
            btn.Text = " Save and Close ";
            btn.Click += new EventHandler(btn_Save_Click);
            this.Pub1.Add(btn);

            btn = new Button();
            btn.ID = "Btn_SaveAndNew";
            btn.CssClass = "Btn";
            btn.Text = " Save and New ";
            btn.Click += new EventHandler(btn_Save_Click);
            this.Pub1.Add(btn);
            if (this.RefNo != null)
            {
                btn = new Button();
                btn.ID = "Btn_AutoFull";
                btn.CssClass = "Btn";
                btn.Text = " Extended Settings ";
                //  btn.Click += new EventHandler(btn_Save_Click);
                btn.Attributes["onclick"] = "javascript:WinOpen('AutoFull.aspx?RefNo=" + this.RefNo + "&FK_MapData=" + mapAttr.FK_MapData + "',''); return false;";
                this.Pub1.Add(btn);

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
                this.Pub1.Add("<a href='" + myUrl + "' target='M"+mapAttr.KeyOfEn+"' ><img src='../Img/Btn/Apply.gif' border=0> Batch </a>");
            }

            string url = "Do.aspx?DoType=AddF&MyPK=" + mapAttr.FK_MapData + "&IDX=" + mapAttr.IDX;
            this.Pub1.Add("<a href='" + url + "'><img src='../Img/Btn/New.gif' border=0> New </a></TD>");

          
            this.Pub1.AddTREnd();
            this.Pub1.AddTableEndWithBR();


        }
        void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                switch (btn.ID)
                {
                    case "Btn_Del":
                        this.Response.Redirect("Do.aspx?DoType=Del&MyPK=" + this.MyPK + "&RefNo=" + this.RefNo, true);
                        return;

                    default:
                        break;
                }

                MapAttr attr = new MapAttr();
                attr.MyPK = this.RefNo;
                if (this.RefNo != null)
                    attr.Retrieve();
                attr = (MapAttr)this.Pub1.Copy(attr);
                attr.FK_MapData = this.MyPK;
                attr.DefVal = this.Pub1.GetDDLByID("DDL").SelectedItemStringVal;
                attr.GroupID = this.Pub1.GetDDLByID("DDL_GroupID").SelectedItemIntVal;
                attr.ColSpan = this.Pub1.GetDDLByID("DDL_ColSpan").SelectedItemIntVal;

                if (this.Pub1.GetRadioButtonByID("RB_Ctrl_0").Checked)
                    attr.UIContralType = UIContralType.DDL;
                else
                    attr.UIContralType = UIContralType.RadioBtn;

                if (this.RefNo == null)
                {
                    attr.MyPK = this.MyPK + "_" + this.Pub1.GetTBByID("TB_KeyOfEn").Text;
                    string idx = this.Request.QueryString["IDX"];
                    if (idx == null || idx == "")
                    {
                    }
                    else
                    {
                        attr.IDX = int.Parse(this.Request.QueryString["IDX"]);
                    }

                    string enumKey = this.Request.QueryString["EnumKey"];
                    attr.UIBindKey = enumKey;
                    attr.MyDataType = BP.DA.DataType.AppInt;
                    attr.HisEditType = EditType.Edit;

                    attr.UIContralType = UIContralType.DDL;
                    attr.LGType = FieldTypeS.Enum;
                    attr.Insert();
                }
                else
                {
                    attr.Update();
                }

                switch (btn.ID)
                {
                    case "Btn_SaveAndClose":
                        this.WinClose();
                        return;
                    case "Btn_SaveAndNew":
                        this.Response.Redirect("Do.aspx?DoType=AddF&MyPK=" + this.MyPK + "&IDX=" + attr.IDX + "&GroupField=" + this.GroupField, true);
                        return;
                    default:
                        break;
                }
                if (this.RefNo == null)
                    this.Response.Redirect("EditEnum.aspx?DoType=Edit&MyPK=" + this.MyPK + "&RefNo=" + attr.MyPK + "&GroupField=" + this.GroupField, true);
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
                    return " Add a new field guide  - <a href='Do.aspx?DoType=ChoseFType&GroupField=" + this.GroupField + "'> Select Type </a>";
                else
                    return " <a href='Do.aspx?DoType=ChoseFType&MyPK=" + this.MyPK + "&RefNo=" + this.RefNo + "&GroupField=" + this.GroupField + "'> Editor </a>";
            }
        }
    }

}