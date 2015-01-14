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
    public partial class Comm_MapDef_MapDtl : BP.Web.WebPage
    {
        #region  Property 
        public new string DoType
        {
            get
            {
                string v = this.Request.QueryString["DoType"];
                if (v == null || v == "")
                    v = "New";
                return v;
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
            MapData md = new MapData(this.FK_MapData);
            this.Title = md.Name + " -  Design Details ";
            switch (this.DoType)
            {
                case "Edit":
                    MapDtl dtl = new MapDtl();
                    if (this.FK_MapDtl == null)
                    {
                        dtl.No = this.FK_MapData + "Dtl";
                    }
                    else
                    {
                        dtl.No = this.FK_MapDtl;
                        dtl.Retrieve();
                    }
                    BindEdit(md, dtl);
                    break;
                default:
                case "New":
                    int num = BP.DA.DBAccess.RunSQLReturnValInt("SELECT COUNT(*) FROM Sys_MapDtl WHERE FK_MapData='" + this.FK_MapData + "'") + 1;
                    MapDtl dtl1 = new MapDtl();
                    dtl1.Name = " From Table " + num;
                    dtl1.No = this.FK_MapData + "Dtl" + num;
                    dtl1.PTable = this.FK_MapData + "Dtl" + num;
                    BindEdit(md, dtl1);
                    break;
            }
        }
        void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            try
            {
                switch (this.DoType)
                {
                    case "New":
                    default:
                        MapDtl dtlN = new MapDtl();
                        dtlN = (MapDtl)this.Pub1.Copy(dtlN);
                        try
                        {
                            dtlN.GroupField = this.Pub1.GetDDLByID("DDL_GroupField").SelectedItemStringVal;
                        }
                        catch
                        {
                        }

                        if (this.DoType == "New")
                        {
                            if (dtlN.IsExits)
                            {
                                this.Alert(" Existing number :" + dtlN.No);
                                return;
                            }
                        }
                        dtlN.FK_MapData = this.FK_MapData;
                        dtlN.GroupID = 0;
                        dtlN.RowIdx = 0;
                        GroupFields gfs1 = new GroupFields(this.FK_MapData);
                        if (gfs1.Count == 1)
                        {
                            GroupField gf = (GroupField)gfs1[0];
                            dtlN.GroupID = gf.OID;
                        }
                        else
                        {
                            dtlN.GroupID = this.Pub1.GetDDLByID("DDL_GroupID").SelectedItemIntVal;
                        }
                        dtlN.Insert();
                        if (btn.ID.Contains("AndClose"))
                        {
                            this.WinClose();
                            return;
                        }
                        this.Response.Redirect("MapDtl.aspx?DoType=Edit&FK_MapDtl=" + dtlN.No + "&FK_MapData=" + this.FK_MapData, true);
                        break;
                    case "Edit":
                        MapDtl dtl = new MapDtl(this.FK_MapDtl);
                        dtl = (MapDtl)this.Pub1.Copy(dtl);

                        // Parameter save .
                        dtl.IsEnableLink = this.Pub1.GetCBByID("CB_" + MapDtlAttr.IsEnableLink).Checked;
                        dtl.LinkLabel = this.Pub1.GetTBByID("TB_" + MapDtlAttr.LinkLabel).Text;
                        dtl.LinkTarget = this.Pub1.GetTBByID("TB_" + MapDtlAttr.LinkTarget).Text;
                        dtl.LinkUrl = this.Pub1.GetTBByID("TB_" + MapDtlAttr.LinkUrl).Text;

                        // Locking .
                        dtl.IsRowLock = this.Pub1.GetCBByID("CB_" + MapDtlAttr.IsRowLock).Checked;

                        // Group field .
                        try
                        {
                            dtl.GroupField = this.Pub1.GetDDLByID("DDL_GroupField").SelectedItemStringVal;
                        }
                        catch
                        {
                        }

                        if (this.DoType == "New")
                        {
                            if (dtl.IsExits)
                            {
                                this.Alert(" Existing number :" + dtl.No);
                                return;
                            }
                        }

                        dtl.FK_MapData = this.FK_MapData;
                        GroupFields gfs = new GroupFields(dtl.FK_MapData);
                        if (gfs.Count > 1)
                            dtl.GroupID = this.Pub1.GetDDLByID("DDL_GroupID").SelectedItemIntVal;

                        if (this.DoType == "New")
                            dtl.Insert();
                        else
                            dtl.Update();

                        if (btn.ID.Contains("AndC"))
                        {
                            this.WinClose();
                            return;
                        }
                        this.Response.Redirect("MapDtl.aspx?DoType=Edit&FK_MapDtl=" + dtl.No + "&FK_MapData=" + this.FK_MapData, true);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }
        void btn_Del_Click(object sender, EventArgs e)
        {
            try
            {
                MapDtl dtl = new MapDtl();
                dtl.No = this.FK_MapDtl;
                dtl.Delete();
                this.WinClose();
                //this.Response.Redirect("MapDtl.aspx?DoType=DtlList&FK_MapData=" + this.FK_MapData, true);
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }

        void btn_MapAth_Click(object sender, EventArgs e)
        {
            FrmAttachment ath = new FrmAttachment();
            ath.MyPK = this.FK_MapDtl + "_AthM";
            if (ath.RetrieveFromDBSources() == 0)
            {
                ath.FK_MapData = this.FK_MapDtl;
                ath.NoOfObj = "AthM";
                ath.Name = " From the table in Annex I ";
                ath.UploadType = AttachmentUploadType.Multi;
                ath.Insert();
            }
            this.Response.Redirect("Attachment.aspx?DoType=Edit&FK_MapData=" + this.FK_MapDtl + "&UploadType=1&Ath=AthM", true);
        }
        void btn_MapExt_Click(object sender, EventArgs e)
        {
            this.Response.Redirect("MapExt.aspx?DoType=New&FK_MapData=" + this.FK_MapDtl, true);
        }
        void btn_New_Click(object sender, EventArgs e)
        {
            this.Response.Redirect("MapDtl.aspx?DoType=New&FK_MapData=" + this.FK_MapData, true);
        }
        void btn_Go_Click(object sender, EventArgs e)
        {
            MapDtl dtl = new MapDtl(this.FK_MapDtl);
            dtl.IntMapAttrs();
            this.Response.Redirect("MapDtlDe.aspx?DoType=Edit&FK_MapData=" + this.FK_MapData + "&FK_MapDtl=" + this.FK_MapDtl, true);
        }

        public void BindEdit(MapData md, MapDtl dtl)
        {
            this.Pub1.AddTable();
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("ID");
            this.Pub1.AddTDTitle("colspan=3", " Basic Settings ");
            this.Pub1.AddTREnd();

            int idx = 1;
            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Table English name ");
            TB tb = new TB();
            tb.ID = "TB_No";
            tb.Text = dtl.No;
            if (this.DoType == "Edit")
                tb.Enabled = false;
            this.Pub1.AddTD(tb);
            this.Pub1.AddTD();
            //this.Pub1.AddTD(" English name globally unique ");
            this.Pub1.AddTREnd();


            this.Pub1.AddTR1();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Table Chinese name ");
            tb = new TB();
            tb.ID = "TB_Name";
            tb.Text = dtl.Name;
            this.Pub1.AddTD(tb);
            this.Pub1.AddTD("XX  From Table ");
            this.Pub1.AddTREnd();


            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Physical table name ");
            tb = new TB();
            tb.ID = "TB_PTable";
            tb.Text = dtl.PTable;

            this.Pub1.AddTD(tb);
            this.Pub1.AddTD();
            //this.Pub1.AddTD(" Physical table name stored data ");
            //  this.Pub1.AddTD(" Physical table name stored data ");
            this.Pub1.AddTREnd();




            this.Pub1.AddTR1();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Operating authority ");
            DDL ddl = new DDL();
            ddl.BindSysEnum(MapDtlAttr.DtlOpenType, (int)dtl.DtlOpenType);
            ddl.ID = "DDL_DtlOpenType";
            this.Pub1.AddTD(ddl);
            this.Pub1.AddTD();
            // this.Pub1.AddTD(" Is used to control access to the table from ");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR1();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Mode ");
            DDL workModelDDl = new DDL();
            ////workModelDDl.Items.Add(new ListItem(" General ", "0"));
            ////workModelDDl.Items.Add(new ListItem(" Fixed line ", "1"));
            ////workModelDDl.SelectedItem.Value = ((int) dtl.DtlModel)+"";
            workModelDDl.BindSysEnum(MapDtlAttr.Model, (int)dtl.DtlModel);
            workModelDDl.ID = "DDL_Model";
            this.Pub1.AddTD(workModelDDl);
            this.Pub1.AddTD();
            this.Pub1.AddTREnd();


            if (dtl.DtlModel == DtlModel.FixRow)
            {
                tb = new TB();
                tb.ID = "TB_" + MapDtlAttr.ImpFixTreeSql;
                tb.Text = dtl.ImpFixTreeSql;
                tb.Columns = 80;

                this.Pub1.AddTR();
                this.Pub1.AddTDIdx(idx++);
                this.Pub1.AddTD(" Tree-structured data sources ");
                this.Pub1.AddTD("colspan=2", tb);
                this.Pub1.AddTREnd();

                tb = new TB();
                tb.ID = "TB_" + MapDtlAttr.ImpFixDataSql;
                tb.Text = dtl.ImpFixDataSql;
                tb.Columns = 80;

                this.Pub1.AddTR();
                this.Pub1.AddTDIdx(idx++);
                this.Pub1.AddTD(" Data source list ");
                this.Pub1.AddTD("colspan=2", tb);
                this.Pub1.AddTREnd();


            }

            CheckBox cb = new CheckBox();

            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            cb.ID = "CB_IsView";
            cb.Text = " Is visible ";
            cb.Checked = dtl.IsView;
            this.Pub1.AddTD(cb);

            cb = new CheckBox();
            cb.ID = "CB_IsUpdate";
            cb.Text = " Whether you can modify the line "; // " Whether you can modify the line ";
            cb.Checked = dtl.IsUpdate;
            this.Pub1.AddTD(cb);

            cb = new CheckBox();
            cb.ID = "CB_IsInsert";
            cb.Text = " Can the new row "; // " Can the new row ";
            cb.Checked = dtl.IsInsert;
            this.Pub1.AddTD(cb);
            this.Pub1.AddTREnd();


            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            cb = new CheckBox();
            cb.ID = "CB_IsDelete";
            cb.Text = " Can I delete a row "; // " Can I delete a row ";
            cb.Checked = dtl.IsDelete;
            this.Pub1.AddTD(cb);

            cb = new CheckBox();
            cb.ID = "CB_IsShowIdx";
            cb.Text = " Whether to display the number column "; //" Whether to display the number column ";
            cb.Checked = dtl.IsShowIdx;
            this.Pub1.AddTD(cb);

            cb = new CheckBox();
            cb.ID = "CB_IsShowSum";
            cb.Text = " Whether the total row ";// " Whether the total row ";
            cb.Checked = dtl.IsShowSum;
            this.Pub1.AddTD(cb);
            this.Pub1.AddTREnd();


            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            cb = new CheckBox();
            cb.ID = "CB_IsCopyNDData";
            cb.Text = " From one node is allowed Copy Data ";
            cb.Checked = dtl.IsCopyNDData;
            this.Pub1.AddTD(cb);

            cb = new CheckBox();
            cb.ID = "CB_IsHLDtl";
            cb.Text = " Whether it is from the confluence of the summary table ( The current node is a confluence node effective )";
            cb.Checked = dtl.IsHLDtl;
            this.Pub1.AddTD("colspan=2", cb);
            this.Pub1.AddTREnd();


            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            cb = new CheckBox();
            cb.ID = "CB_IsEnableAthM";
            cb.Text = " Whether to enable multiple attachments ";
            cb.Checked = dtl.IsEnableAthM;
            this.Pub1.AddTD(cb);

            cb = new CheckBox();
            cb.ID = "CB_" + MapDtlAttr.IsEnableM2M;
            cb.Text = " Whether enabled many ";
            cb.Checked = dtl.IsEnableM2M;
            this.Pub1.AddTD(cb);

            cb = new CheckBox();
            cb.ID = "CB_" + MapDtlAttr.IsEnableM2MM;
            cb.Text = " Whether to enable many more ";
            cb.Checked = dtl.IsEnableM2MM;
            this.Pub1.AddTD(cb);
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            cb = new CheckBox();
            cb.ID = "CB_IsEnablePass";
            cb.Text = " Whether hired auditing field ?";// " Whether the total row ";
            cb.Checked = dtl.IsEnablePass;
            this.Pub1.AddTD(cb);

            string sql = "SELECT KeyOfEn as No, Name FROM Sys_MapAttr WHERE FK_MapData='" + dtl.No + "' AND  ((MyDataType in (1,4,6) and UIVisible=1 ) or (UIContralType=1))";
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
            {
                this.Pub1.AddTD();
                this.Pub1.AddTD();
            }
            else
            {
                this.Pub1.AddTDBegin("colspan=2");
                cb = new CheckBox();
                cb.ID = "CB_IsEnableGroupField";
                cb.Text = " Whether hired group field ?";// " Whether the total row ";
                cb.Checked = dtl.IsEnableGroupField;
                this.Pub1.Add(cb);

                ddl = new DDL();
                ddl.ID = "DDL_GroupField";
                ddl.BindSQL(sql, "No", "Name", dtl.GroupField);
                this.Pub1.Add(ddl);
                this.Pub1.AddTDEnd();
            }
            this.Pub1.AddTREnd();


            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            cb = new CheckBox();
            cb.ID = "CB_" + MapDtlAttr.IsRowLock;
            cb.Text = " Whether to enable the locked row （ If you enable the need to increase IsRowLock A hidden column , The default value is 0.）?";// " Whether the total row ";
            cb.Checked = dtl.IsRowLock;
            this.Pub1.AddTD("colspan=3", cb);
            this.Pub1.AddTREnd();


            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            cb = new CheckBox();
            cb.ID = "CB_" + MapDtlAttr.IsShowTitle;
            cb.Text = " Whether to display the header ( If the head is more exemplar list , Do not show up on the table header )?"; ;
            cb.Checked = dtl.IsShowTitle;
            this.Pub1.AddTD("colspan=3", cb);
            this.Pub1.AddTREnd();


            //this.Pub1.AddTR();
            //this.Pub1.AddTDIdx(idx++);
            //cb = new CheckBox();
            //cb.ID = "CB_IsShowTitle";
            //cb.Text = " Whether to display the header ";// " Whether to display the header ";
            //cb.Checked = dtl.IsShowTitle;
            //this.Pub1.AddTD(cb);
            //this.Pub1.AddTREnd();


            //cb = new CheckBox();
            //cb.ID = "CB_IsEnableAth";
            //cb.Text = " Whether to enable single attachment "; 
            //cb.Checked = dtl.IsEnablePass;
            //this.Pub1.AddTD(cb);
            //this.Pub1.AddTREnd();


            this.Pub1.AddTR1();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Initializing the number of rows ");
            tb = new TB();
            tb.ID = "TB_RowsOfList";
            tb.Attributes["class"] = "TBNum";
            tb.TextExtInt = dtl.RowsOfList;
            this.Pub1.AddTD(tb);
            this.Pub1.AddTD("");
            this.Pub1.AddTREnd();


            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Display Format ");
            ddl = new DDL();
            ddl.ID = "DDL_DtlShowModel";
            ddl.BindSysEnum(MapDtlAttr.DtlShowModel, (int)dtl.HisDtlShowModel);
            this.Pub1.AddTD(ddl);
            this.Pub1.AddTD();
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Beyond treatment ");
            ddl = new DDL();
            ddl.ID = "DDL_WhenOverSize";
            ddl.BindSysEnum(MapDtlAttr.WhenOverSize, (int)dtl.HisWhenOverSize);
            this.Pub1.AddTD(ddl);
            this.Pub1.AddTD();
            this.Pub1.AddTREnd();


            #region  Data import and export schedule .
            this.Pub1.AddTRSum();
            this.Pub1.AddTDTitle("colspan=4", " Data import and export schedule ");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            cb = new CheckBox();
            cb.ID = "CB_IsExp";
            cb.Text = " Can I export ?";// " Can I export ";
            cb.Checked = dtl.IsExp;
            this.Pub1.AddTD(cb);

            cb = new CheckBox();
            cb.ID = "CB_IsImp";
            cb.Text = " Can I import ?";// " Can I export ";
            cb.Checked = dtl.IsImp;
            this.Pub1.AddTD(cb);
            this.Pub1.AddTD();
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            cb = new CheckBox();
            cb.ID = "CB_" + MapDtlAttr.IsEnableSelectImp;
            cb.Text = " Whether to enable selective import ( In case true Must configure the data source presents sql)?";
            cb.Checked = dtl.IsEnableSelectImp;
            this.Pub1.AddTD("colspan=3", cb);
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Initialization SQL");
            tb = new TB();
            tb.ID = "TB_" + MapDtlAttr.ImpSQLInit;
            tb.Text = dtl.ImpSQLInit;
            tb.Columns = 80;
            this.Pub1.AddTD("colspan=2", tb);
            this.Pub1.AddTREnd();


            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Inquiry SQL");
            tb = new TB();
            tb.ID = "TB_" + MapDtlAttr.ImpSQLSearch;
            tb.Text = dtl.ImpSQLSearch;
            tb.Columns = 80;
            this.Pub1.AddTD("colspan=2", tb);
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Filling SQL");
            tb = new TB();
            tb.ID = "TB_" + MapDtlAttr.ImpSQLFull;
            tb.Text = dtl.ImpSQLFull;
            tb.Columns = 80;
            this.Pub1.AddTD("colspan=2", tb);
            this.Pub1.AddTREnd();

            #endregion  Data import and export schedule .


            #region  Hyperlinks .
            this.Pub1.AddTRSum();
            this.Pub1.AddTDTitle("colspan=4", " Form on the right column of the super-connected configuration ");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            cb = new CheckBox();
            cb.ID = "CB_" + MapDtlAttr.IsEnableLink;
            cb.Text = " Whether to enable hyper-links ?";
            cb.Checked = dtl.IsEnableLink;
            this.Pub1.AddTD(cb);
            this.Pub1.AddTD(" Hyperlink tags ");

            tb = new TB();
            tb.ID = "TB_" + MapDtlAttr.LinkLabel;
            tb.Text = dtl.LinkLabel;
            this.Pub1.AddTD(tb);
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD("colspan=3", " Connection URL");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            tb = new TB();
            tb.ID = "TB_" + MapDtlAttr.LinkUrl;
            tb.Text = dtl.LinkUrl;
            tb.Columns = 90;
            this.Pub1.AddTD("colspan=3", tb);
            this.Pub1.AddTREnd();


            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Connection target ");
            tb = new TB();
            tb.ID = "TB_" + MapDtlAttr.LinkTarget;
            tb.Text = dtl.LinkTarget;
            this.Pub1.AddTD("colspan=2", tb);
            this.Pub1.AddTREnd();
            #endregion  Hyperlinks .


            GroupFields gfs = new GroupFields(md.No);
            if (gfs.Count > 1)
            {
                this.Pub1.AddTR1();
                this.Pub1.AddTDIdx(idx++);
                this.Pub1.AddTD(" Displayed in the group ");
                ddl = new DDL();
                ddl.ID = "DDL_GroupID";
                ddl.BindEntities(gfs, GroupFieldAttr.OID, GroupFieldAttr.Lab, false, AddAllLocation.None);
                ddl.SetSelectItem(dtl.GroupID);
                this.Pub1.AddTD("colspan=2", ddl);
                this.Pub1.AddTREnd();
            }
            if (gfs.Count > 1)
                this.Pub1.AddTR();
            else
                this.Pub1.AddTR1();

            this.Pub1.AddTRSum();
            this.Pub1.AddTD("");
            this.Pub1.AddTDBegin("colspan=3 align=center");

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

            if (this.FK_MapDtl != null)
            {
                //btn = new Button();
                //btn.ID = "Btn_D";
                //btn.Text = this.ToE("DesignSheet", " Design a Form "); // " Design a Form ";
                //btn.Click += new EventHandler(btn_Go_Click);
                //this.Pub1.Add(btn);

                btn = new Button();
                btn.ID = "Btn_Del";
                btn.CssClass = "Btn";
                btn.Text = " Delete "; // " Delete ";
                btn.Attributes["onclick"] = " return confirm(' You acknowledge that you ?');";
                btn.Click += new EventHandler(btn_Del_Click);
                this.Pub1.Add(btn);


                btn = new Button();
                btn.ID = "Btn_New";
                btn.CssClass = "Btn";
                btn.Text = " New "; // " Delete ";
                btn.Click += new EventHandler(btn_New_Click);
                this.Pub1.Add(btn);

                btn = new Button();
                btn.ID = "Btn_MapExt";
                btn.CssClass = "Btn";
                btn.Text = " Extended Settings "; // " Delete ";

                btn.Click += new EventHandler(btn_MapExt_Click);
                this.Pub1.Add(btn);

                if (dtl.IsEnableAthM)
                {
                    btn = new Button();
                    btn.CssClass = "Btn";
                    btn.ID = "Btn_IsEnableAthM";
                    btn.Text = " Attachment Properties "; // " Delete ";
                    btn.Click += new EventHandler(btn_MapAth_Click);
                    this.Pub1.Add(btn);
                }

                // btn = new Button();
                // btn.ID = "Btn_DtlTR";
                // btn.Text = " Multi-header ";
                // btn.Attributes["onclick"] = "javascript:WinOpen('')";
                //// btn.Click += new EventHandler(btn_DtlTR_Click);
                // this.Pub1.Add(btn);
            }
            this.Pub1.AddTDEnd();
            this.Pub1.AddTREnd();
            this.Pub1.AddTableEnd();
        }
    }

}