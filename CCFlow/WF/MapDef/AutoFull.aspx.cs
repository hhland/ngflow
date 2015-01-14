using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Web.Controls;
using BP.Sys;
using BP.En;
using BP.Web;
using BP.Web.UC;
using BP.DA;
namespace CCFlow.WF.MapDef
{
    public partial class WF_MapDef_AutoFull : BP.Web.WebPage
    {
        #region  Property 
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
        public string FK_MapData
        {
            get
            {
                return this.Request.QueryString["FK_MapData"];
            }
        }
        #endregion

        #region  Property 
        public void BindTop()
        {
            // this.Pub1.Add("\t\n<div id='tabsJ'  align='center'>");
            MapExtXmls fss = new MapExtXmls();
            fss.RetrieveAll();

            this.Left.Add("<a href='http://ccflow.org' target=_blank  ><img src='../../DataUser/ICON/" + BP.Sys.SystemConfig.CompanyID + "/LogBiger.png' style='width:180px;' /></a><hr>");

            //this.Left.Add("<a href='http://ccflow.org' target=_blank ><img src='../../DataUser/LogBiger.png' /></a><hr>");

            this.Left.AddUL();
            foreach (MapExtXml fs in fss)
            {
                if (this.PageID == fs.No)
                    this.Left.AddLiB(fs.URL + "&FK_MapData=" + this.FK_MapData + "&ExtType=" + fs.No + "&RefNo=" + this.RefNo, "<span>" + fs.Name + "</span>");
                else
                    this.Left.AddLi(fs.URL + "&FK_MapData=" + this.FK_MapData + "&ExtType=" + fs.No + "&RefNo=" + this.RefNo, "<span>" + fs.Name + "</span>");
            }
            this.Left.AddLi("<a href='MapExt.aspx?FK_MapData=" + this.FK_MapData + "&RefNo=" + this.RefNo + "'><span> Help </span></a>");
            this.Left.AddULEnd();
        }
        #endregion  Property 

        protected void Page_Load(object sender, EventArgs e)
        {
            this.BindTop();

            if (this.RefNo == null)
            {
                /* Please select the fields you want to set */
                MapAttrs mattrs = new MapAttrs();
                mattrs.Retrieve(MapAttrAttr.FK_MapData, this.FK_MapData);

                this.Pub1.AddFieldSet(" Please select the fields you want to set ");
                this.Pub1.AddUL("class=''");
                foreach (MapAttr en in mattrs)
                {

                    if (en.UIVisible == false && en.UIIsEnable == false)
                        continue;

                    this.Pub1.AddLi("?FK_MapData=" + this.FK_MapData + "&RefNo=" + en.MyPK + "&ExtType=AutoFull", en.KeyOfEn + " - " + en.Name);
                }
                this.Pub1.AddULEnd();
                this.Pub1.AddFieldSetEnd();
                return;
            }

            MapAttr mattr = new MapAttr(this.RefNo);
            Attr attr = mattr.HisAttr;
            this.Title = "为[" + mattr.KeyOfEn + "][" + mattr.Name + "] AutoComplete Settings "; // this.ToE("GuideNewField");

            switch (attr.MyDataType)
            {
                case BP.DA.DataType.AppRate:
                case BP.DA.DataType.AppMoney:
                case BP.DA.DataType.AppInt:
                case BP.DA.DataType.AppFloat:
                    BindNumType(mattr);
                    break;
                case BP.DA.DataType.AppString:
                    BindStringType(mattr);
                    break;
                default:
                    BindStringType(mattr);
                    break;
            }

        }
        public void BindStringType(MapAttr mattr)
        {
            BindNumType(mattr);
        }
        public void BindNumType(MapAttr mattr_del)
        {
            MapExt me = new MapExt();
            me.MyPK = this.RefNo + "_AutoFull";
            me.RetrieveFromDBSources();

            this.Pub1.AddTable("align=left");
            this.Pub1.AddCaptionLeft(" Data Acquisition  -  When a field value needs to be obtained from other tables , Set this feature .");
            this.Pub1.AddTR();
            this.Pub1.Add("<TD>");
            RadioBtn rb = new RadioBtn();
            rb.GroupName = "s";
            rb.Text = " The way 0: Do nothing .";
            rb.ID = "RB_Way_0";
            if (me.Tag == "0")
                rb.Checked = true;

            this.Pub1.AddFieldSet(rb);
            this.Pub1.Add(  " Do nothing .");
            this.Pub1.AddFieldSetEnd();

            this.Pub1.AddTDEnd();
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.Add("<TD>");

            rb = new RadioBtn();
            rb.GroupName = "s";
            rb.Text = " The way 1: The form data is calculated ."; //"";
            rb.ID = "RB_Way_1";
            if (me.Tag == "1")
                rb.Checked = true;
            this.Pub1.AddFieldSet(rb);
            this.Pub1.Add( " Such as :@ Unit price *@ Quantity ");
            this.Pub1.AddBR();

            TextBox tb = new TextBox();
            tb.ID = "TB_JS";
            tb.Width = 450;
            tb.TextMode = TextBoxMode.MultiLine;
            tb.Rows = 5;
            if (me.Tag == "1")
                tb.Text = me.Doc;

            this.Pub1.Add(tb);
            this.Pub1.AddFieldSetEnd();
            this.Pub1.AddTDEnd();
            this.Pub1.AddTREnd();

            //  The way 2  Use SQL Automatic filling 
            this.Pub1.AddTR();
            this.Pub1.Add("<TD>");

            rb = new RadioBtn();
            rb.GroupName = "s";
            rb.Text =  " The way 2: Use SQL Automatic filling ( This feature has been in ccflow5 Cancel , Need this feature, please node or the logic in the form of event where complete ).";
            rb.ID = "RB_Way_2";
            rb.Enabled = false;

            if (me.Tag == "2")
                rb.Checked = true;

           // if (mattr.HisAutoFull == AutoFullWay.Way2_SQL)

            this.Pub1.AddFieldSet(rb);
            this.Pub1.Add( " Such as :Select Addr From  Product table  WHERE No=@FK_Pro  FK_Pro Is this table any field name <BR>");

            tb = new TextBox();
            tb.ID = "TB_SQL";
            tb.Width = 450;
            tb.TextMode = TextBoxMode.MultiLine;
            tb.Rows = 5;
            if (me.Tag == "2")
                tb.Text = me.Doc;

            this.Pub1.Add(tb);

            this.Pub1.AddFieldSetEnd();
            this.Pub1.AddTDEnd();
            this.Pub1.AddTREnd();

            //  The way 3  This form foreign key column 
            this.Pub1.AddTR();
            this.Pub1.Add("<TD>");
            rb = new RadioBtn();
            rb.GroupName = "s";
            rb.Text = " The way 3: This form foreign key column ." ;
            // rb.Text = " The way 3: This form foreign key column </font></b>";
            rb.ID = "RB_Way_3";
            if (me.Tag == "3")
                rb.Checked = true;

            //if (mattr.HisAutoFull == AutoFullWay.Way3_FK)

            this.Pub1.AddFieldSet(rb);
            this.Pub1.Add(  " Such as : There are number of the column in the form of goods , Goods need to fill address , Supplier Phone .");
            this.Pub1.AddBR();


            //  It is equal to a value of the foreign key table .
            Attrs attrs = null;
            MapData md = new MapData();
            md.No = this.FK_MapData;
            if (md.RetrieveFromDBSources() == 0)
            {
                attrs = md.GenerHisMap().HisFKAttrs;
            }
            else
            {
                MapDtl mdtl = new MapDtl();
                mdtl.No = this.FK_MapData;
                attrs = mdtl.GenerMap().HisFKAttrs;
            }

            if (attrs.Count > 0)
            {
            }
            else
            {
                rb.Enabled = false;
                if (rb.Checked)
                    rb.Checked = false;
                this.Pub1.Add("@ The table does not have the foreign key field .");
            }

            foreach (Attr attr in attrs)
            {
                if (attr.IsRefAttr)
                    continue;

                rb = new RadioBtn();
                rb.Text = attr.Desc;
                rb.ID = "RB_FK_" + attr.Key;
                rb.GroupName = "sd";

                if (me.Doc.Contains(attr.Key))
                    rb.Checked = true;

                this.Pub1.Add(rb);
                DDL ddl = new DDL();
                ddl.ID = "DDL_" + attr.Key;

                string sql = "";
                switch (BP.Sys.SystemConfig.AppCenterDBType)
                {
                    case DBType.Oracle:
                    case DBType.Informix:
                        continue;
                        sql = "Select fname as 'No' ,fDesc as 'Name' FROM Sys_FieldDesc WHERE tableName='" + attr.HisFKEn.EnMap.PhysicsTable + "'";
                        break;
                    case DBType.MySQL:
                        sql = "Select COLUMN_NAME as No,COLUMN_NAME as Name from information_schema.COLUMNS WHERE TABLE_NAME='" + attr.HisFKEn.EnMap.PhysicsTable + "'";
                        break;
                    default:
                        sql = "Select name as 'No' ,Name as 'Name' from syscolumns WHERE ID=OBJECT_ID('" + attr.HisFKEn.EnMap.PhysicsTable + "')";
                        break;
                }

                //  string sql = "Select fname as 'No' ,fDesc as 'Name' FROM Sys_FieldDesc WHERE tableName='" + attr.HisFKEn.EnMap.PhysicsTable + "'";
                //string sql = "Select NO , NAME  FROM Port_Emp ";

                DataTable dt = DBAccess.RunSQLReturnTable(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    //  ddl.Items.Add(new ListItem(this.ToE("Field") + dr[0].ToString() + " " + this.ToE("Desc") + " " + dr[1].ToString(), dr[0].ToString()));
                    ListItem li = new ListItem(dr[0].ToString() + ";" + dr[1].ToString(), dr[0].ToString());
                    if (me.Doc.Contains(dr[0].ToString()))
                        li.Selected = true;

                    ddl.Items.Add(li);
                }

                this.Pub1.Add(ddl);
                this.Pub1.AddBR();
            }

            this.Pub1.AddFieldSetEnd();
            this.Pub1.AddTDEnd();
            this.Pub1.AddTREnd();

            //  The way 3  This form foreign key column 
            this.Pub1.AddTR();
            this.Pub1.Add("<TD>");
            rb = new RadioBtn();
            rb.GroupName = "s";
            rb.Text =  " The way 4: An evaluator from the list of .";
            rb.ID = "RB_Way_4";
            if (me.Tag == "4")
                rb.Checked = true;

            this.Pub1.AddFieldSet(rb);
            this.Pub1.Add( " Such as : Evaluation of the column from the table .");
            this.Pub1.AddBR();

            //  From the table it for a summation , Averaging , Seeking maximum , For the minimum .
            MapDtls dtls = new MapDtls(this.FK_MapData);
            if (dtls.Count > 0)
            {
            }
            else
            {
                rb.Enabled = false;
                if (rb.Checked)
                    rb.Checked = false;
                // this.Pub1.Add("@ No Table .");
            }
            foreach (MapDtl dtl in dtls)
            {
                DDL ddlF = new DDL();
                ddlF.ID = "DDL_" + dtl.No + "_F";
                MapAttrs mattrs1 = new MapAttrs(dtl.No);
                int count = 0;
                foreach (MapAttr mattr1 in mattrs1)
                {
                    if (mattr1.LGType != FieldTypeS.Normal)
                        continue;

                    if (mattr1.KeyOfEn == MapAttrAttr.MyPK)
                        continue;

                    if (mattr1.IsNum == false)
                        continue;
                    switch (mattr1.KeyOfEn)
                    {
                        case "OID":
                        case "RefOID":
                        case "FID":
                            continue;
                        default:
                            break;
                    }
                    count++;
                    ListItem li = new ListItem(mattr1.Name, mattr1.KeyOfEn);
                    if (me.Tag  == "4")
                        if (me.Doc.Contains("=" + mattr1.KeyOfEn))
                            li.Selected = true;
                    ddlF.Items.Add(li);
                }
                if (count == 0)
                    continue;

                rb = new RadioBtn();
                rb.Text = dtl.Name;
                rb.ID = "RB_" + dtl.No;
                rb.GroupName = "dtl";
                if (me.Doc.Contains(dtl.No))
                    rb.Checked = true;

                this.Pub1.Add(rb);

                DDL ddl = new DDL();
                ddl.ID = "DDL_" + dtl.No + "_Way";
                ddl.Items.Add(new ListItem(" Total demand ", "SUM"));
                ddl.Items.Add(new ListItem(" Averaging ", "AVG"));
                ddl.Items.Add(new ListItem(" Seeking maximum ", "MAX"));
                ddl.Items.Add(new ListItem(" Minimum requirements ", "MIN"));
                this.Pub1.Add(ddl);

                if (me.Tag  == "4")
                {
                    if (me.Doc.Contains("SUM"))
                        ddl.SetSelectItem("SUM");
                    if (me.Doc.Contains("AVG"))
                        ddl.SetSelectItem("AVG");
                    if (me.Doc.Contains("MAX"))
                        ddl.SetSelectItem("MAX");
                    if (me.Doc.Contains("MIN"))
                        ddl.SetSelectItem("MIN");
                }

                this.Pub1.Add(ddlF);
                this.Pub1.AddBR();
            }

            this.Pub1.AddFieldSetEnd();
            this.Pub1.AddTDEnd();
            this.Pub1.AddTREnd();


            #region  The way 5
            //this.Pub1.AddTD();
            //this.Pub1.AddTR();

            //this.Pub1.AddFieldSet(rb);
            //this.Pub1.Add(this.ToE("Way2D", " Embedded JS"));
            //tb = new TextBox();
            //tb.ID = "TB_JS";
            //tb.Width = 450;
            //tb.TextMode = TextBoxMode.MultiLine;
            //tb.Rows = 5;
            //if (mattr.HisAutoFull == AutoFullWay.Way5_JS)
            //    tb.Text = mattr.AutoFullDoc;
            //this.Pub1.Add(tb);
            //this.Pub1.AddFieldSetEnd();

            //this.Pub1.AddTDEnd();
            //this.Pub1.AddTREnd();
            #endregion  The way 5


            this.Pub1.AddTRSum();
            this.Pub1.AddTDBegin("aligen=center");
            Button btn = new Button();
            btn.ID = "Btn_Save";
            btn.CssClass = "Btn";
            btn.Text = "  Save  ";
            btn.Click += new EventHandler(btn_Click);
            this.Pub1.Add(btn);

            btn = new Button();
            btn.ID = "Btn_SaveAndClose";
            btn.CssClass = "Btn";
            btn.Text = "  Save and Close  ";
            btn.Click += new EventHandler(btn_Click);
            this.Pub1.Add(btn);
            this.Pub1.AddTREnd();
            this.Pub1.AddTableEnd();
            return;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btn_Click(object sender, EventArgs e)
        {
            MapAttr mattrNew = new MapAttr(this.RefNo);

            MapExt me = new MapExt();
            me.MyPK =   this.RefNo + "_AutoFull";
            me.RetrieveFromDBSources();
            me.FK_MapData = this.FK_MapData;
            me.AttrOfOper = mattrNew.KeyOfEn;
            me.ExtType = MapExtXmlList.AutoFull;
            if (this.Pub1.GetRadioButtonByID("RB_Way_0").Checked)
            {
                me.Tag = "0";
            }

            // JS  The way .
            if (this.Pub1.GetRadioButtonByID("RB_Way_1").Checked)
            {
                me.Tag = "1";
                me.Doc = this.Pub1.GetTextBoxByID("TB_JS").Text;

                /* Check that the field is filled in correctly .*/
                MapAttrs attrsofCheck = new MapAttrs(this.FK_MapData);
                string docC = me.Doc;
                foreach (MapAttr attrC in attrsofCheck)
                {
                    if (attrC.IsNum == false)
                        continue;
                    docC = docC.Replace("@" + attrC.KeyOfEn, "");
                    docC = docC.Replace("@" + attrC.Name, "");
                }

                if (docC.Contains("@"))
                {
                    this.Alert(" You fill in the formula expression is incorrect , Lead to some numeric type field is not the correct replacement ." + docC);
                    return;
                }
            }

            //  Foreign key way .
            if (this.Pub1.GetRadioButtonByID("RB_Way_2").Checked)
            {
                me.Tag = "2";
                me.Doc = this.Pub1.GetTextBoxByID("TB_SQL").Text;

                //mattr.HisAutoFull = AutoFullWay.Way2_SQL;
                //mattr.AutoFullDoc = this.Pub1.GetTextBoxByID("TB_SQL").Text;
            }

            //  This form foreign key column .
            string doc = "";
            if (this.Pub1.GetRadioButtonByID("RB_Way_3").Checked)
            {
                me.Tag = "3";

               // mattr.HisAutoFull = AutoFullWay.Way3_FK;
                MapData md = new MapData(this.FK_MapData);
                Attrs attrs = md.GenerHisMap().HisFKAttrs;
                foreach (Attr attr in attrs)
                {
                    if (attr.IsRefAttr)
                        continue;

                    if (this.Pub1.GetRadioButtonByID("RB_FK_" + attr.Key).Checked == false)
                        continue;
                    // doc = " SELECT " + this.Pub1.GetDDLByID("DDL_" + attr.Key).SelectedValue + " FROM " + attr.HisFKEn.EnMap.PhysicsTable + " WHERE NO=@" + attr.Key;
                    doc = "@AttrKey=" + attr.Key + "@Field=" + this.Pub1.GetDDLByID("DDL_" + attr.Key).SelectedValue + "@Table=" + attr.HisFKEn.EnMap.PhysicsTable;
                }
                me.Doc = doc;
            }

            //  This form columns from a table .
            if (this.Pub1.GetRadioButtonByID("RB_Way_4").Checked)
            {
                me.Tag = "4";

                MapDtls dtls = new MapDtls(this.FK_MapData);
             //   mattr.HisAutoFull = AutoFullWay.Way4_Dtl;
                foreach (MapDtl dtl in dtls)
                {
                    try
                    {
                        if (this.Pub1.GetRadioButtonByID("RB_" + dtl.No).Checked == false)
                            continue;
                    }
                    catch
                    {
                        continue;
                    }
                    //  doc = "SELECT " + this.Pub1.GetDDLByID( "DDL_"+dtl.No + "_Way").SelectedValue + "(" + this.Pub1.GetDDLByID("DDL_"+dtl.No+"_F").SelectedValue + ") FROM " + dtl.No + " WHERE REFOID=@OID";
                    doc = "@Table=" + dtl.No + "@Field=" + this.Pub1.GetDDLByID("DDL_" + dtl.No + "_F").SelectedValue + "@Way=" + this.Pub1.GetDDLByID("DDL_" + dtl.No + "_Way").SelectedValue;
                }
                me.Doc = doc;
            }

            try
            {
                me.Save();
            }
            catch (Exception ex)
            {
                this.ResponseWriteRedMsg(ex);
                return;
            }

            this.Alert(" Saved successfully ");
            this.Pub1.Clear();
            Button btn = sender as Button;
            if (btn.ID.Contains("Close"))
            {
                this.WinClose();
                return;
            }
            else
            {
                this.Response.Redirect(this.Request.RawUrl, true);
            }
        }
        public void BindStringType()
        {
        }
        public string GetCaption
        {
            get
            {
                if (this.DoType == "Add")
                    return " New Wizard " + " - <a href='Do.aspx?DoType=ChoseFType'> Select Type </a> - " + " Editor ";
                else
                    return "<a href='Do.aspx?DoType=ChoseFType&MyPK=" + this.MyPK + "&RefNo=" + this.RefNo + "'> Select Type </a> - " + " Editor ";
            }
        }
    }
}