using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Sys;
using BP.En;
using BP.Web;
namespace CCFlow.WF.MapDef
{
    public partial class WF_MapDef_FrmAttachment : BP.Web.WebPage
    {
        #region  Property .
        /// <summary>
        ///  Type 
        /// </summary>
        public string UploadType
        {
            get
            {
                string s = this.Request.QueryString["UploadType"];
                if (s == null)
                    s = "1";
                return s;
            }
        }
        public string FK_MapData
        {
            get
            {
                string s = this.Request.QueryString["FK_MapData"];
                if (s == null)
                    s = "test";
                return s;
            }
        }
        public string Ath
        {
            get
            {
                return this.Request.QueryString["Ath"];
            }
        }
        public int FK_Node
        {
            get
            {
                try
                {
                    return int.Parse(this.Request.QueryString["FK_Node"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
        #endregion  Property .

        protected void Page_Load(object sender, EventArgs e)
        {
            FrmAttachment ath = new FrmAttachment();
            ath.FK_MapData = this.FK_MapData;
            ath.NoOfObj = this.Ath;
            ath.FK_Node = this.FK_Node;

            if (this.FK_Node==0)
                ath.MyPK = this.FK_MapData + "_" + this.Ath;
            else
                ath.MyPK = this.FK_MapData + "_" + this.Ath+"_"+this.FK_Node;

            int i = ath.RetrieveFromDBSources();
            if (i==0 && this.FK_Node != 0)
            {
                ath.FK_MapData = this.FK_MapData;
                ath.NoOfObj = this.Ath;
                ath.FK_Node = this.FK_Node;
                ath.MyPK = this.FK_MapData + "_" + this.Ath;
                ath.RetrieveFromDBSources();

                // Insert a new .
                ath.MyPK = this.FK_MapData + "_" + this.Ath+"_"+this.FK_Node;
                ath.FK_Node = this.FK_Node;
                ath.FK_MapData = this.FK_MapData;
                ath.NoOfObj = this.Ath;
                ath.DirectInsert();
            }

            this.Title = " Accessories attribute set ";
            this.Pub1.AddTable();
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle(" Project ");
            this.Pub1.AddTDTitle(" Collection ");
            this.Pub1.AddTDTitle(" Explanation ");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Serial number ");
            TextBox tb = new TextBox();
            tb.ID = "TB_" + FrmAttachmentAttr.NoOfObj;
            tb.Text = ath.NoOfObj;
            if (this.Ath != null)
                tb.Enabled = false;

            this.Pub1.AddTD(tb);
            this.Pub1.AddTD(" Only English alphanumeric identification number or to decline line .");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Name ");
            tb = new TextBox();
            tb.ID = "TB_" + FrmAttachmentAttr.Name;
            tb.Text = ath.Name;
            this.Pub1.AddTD(tb);
            this.Pub1.AddTD(" The Chinese name of the attachment .");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" File Format ");
            tb = new TextBox();
            tb.ID = "TB_" + FrmAttachmentAttr.Exts;
            tb.Text = ath.Exts;
            this.Pub1.AddTD(tb);
            this.Pub1.AddTD(" Examples :doc,docx,xls, Multiple formats separated by commas .");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Save to ");
            tb = new TextBox();
            tb.ID = "TB_" + FrmAttachmentAttr.SaveTo;
            tb.Text = ath.SaveTo;
            tb.Columns = 60;
            this.Pub1.AddTD("colspan=2", tb);
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Category ");
            tb = new TextBox();
            tb.ID = "TB_" + FrmAttachmentAttr.Sort;
            tb.Text = ath.Sort;
            tb.Columns = 60;
            this.Pub1.AddTD("colspan=2", tb);
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD("colspan=3", " Help : Category may be empty , Format set for : Category name 1, Category name 2, Category name 3");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Height ");
            BP.Web.Controls.TB mytb = new BP.Web.Controls.TB();
            mytb.ID = "TB_" + FrmAttachmentAttr.H;
            mytb.Text = ath.H.ToString();
            mytb.ShowType = BP.Web.Controls.TBType.Float;
            this.Pub1.AddTD("colspan=1", mytb);
            this.Pub1.AddTD(" Effective form for fools ");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Width ");
            mytb = new BP.Web.Controls.TB();
            mytb.ID = "TB_" + FrmAttachmentAttr.W;
            mytb.Text = ath.W.ToString();
            mytb.ShowType = BP.Web.Controls.TBType.Float;
            mytb.Columns = 60;
            this.Pub1.AddTD("colspan=1", mytb);
            this.Pub1.AddTD(" Effective form for fools ");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Automatic control ");
            CheckBox cb = new CheckBox();
            cb.ID = "CB_" + FrmAttachmentAttr.IsAutoSize;
            cb.Text = " Automatic control of the height and width ( Effective form for fools )";
            cb.Checked = ath.IsAutoSize;
            this.Pub1.AddTD("colspan=2", cb);
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            cb = new CheckBox();
            cb.ID = "CB_" + FrmAttachmentAttr.IsNote;
            cb.Text = " Whether to increase Remarks column ";
            cb.Checked = ath.IsNote;
            this.Pub1.AddTD(cb);

            cb = new CheckBox();
            cb.ID = "CB_" + FrmAttachmentAttr.IsShowTitle;
            cb.Text = " Whether to display the title bar ";
            cb.Checked = ath.IsShowTitle;
            this.Pub1.AddTD(cb);
            this.Pub1.AddTD("");
            this.Pub1.AddTREnd();


            GroupFields gfs = new GroupFields(ath.FK_MapData);

            this.Pub1.AddTR1();
            this.Pub1.AddTD(" Displayed in the group ");
            BP.Web.Controls.DDL ddl = new BP.Web.Controls.DDL();
            ddl.ID = "DDL_GroupField";
            ddl.BindEntities(gfs, GroupFieldAttr.OID, GroupFieldAttr.Lab, false, BP.Web.Controls.AddAllLocation.None);
            ddl.SetSelectItem(ath.GroupID);
            this.Pub1.AddTD("colspan=2", ddl);

            this.Pub1.AddTREnd();
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("colspan=3", " Access control ");
            this.Pub1.AddTREnd();


            this.Pub1.AddTR();
            cb = new CheckBox();
            cb.ID = "CB_" + FrmAttachmentAttr.IsDownload;
            cb.Text = " Can download ";
            cb.Checked = ath.IsDownload;
            this.Pub1.AddTD(cb);

            cb = new CheckBox();
            cb.ID = "CB_" + FrmAttachmentAttr.IsDelete;
            cb.Text = " Whether it can be deleted ";
            cb.Checked = ath.IsDelete;
            this.Pub1.AddTD(cb);

            cb = new CheckBox();
            cb.ID = "CB_" + FrmAttachmentAttr.IsUpload;
            cb.Text = " Can upload ";
            cb.Checked = ath.IsUpload;
            this.Pub1.AddTD(cb);
            this.Pub1.AddTREnd();

          
            this.Pub1.AddTR1();
            this.Pub1.AddTD(" Data display control mode ");
            ddl = new BP.Web.Controls.DDL();
            ddl.ID = "DDL_CtrlWay";
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem(" By primary key ","0"));
            ddl.Items.Add(new ListItem("FID", "1"));
            ddl.Items.Add(new ListItem("ParentWorkID", "2"));
            ddl.SetSelectItem( (int)ath.HisCtrlWay);
            this.Pub1.AddTD("colspan=2", ddl);
            this.Pub1.AddTREnd();

            this.Pub1.AddTR1();
            this.Pub1.AddTD(" Data upload control ");
            ddl = new BP.Web.Controls.DDL();
            ddl.ID = "DDL_AthUploadWay";
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem(" Inheritance ", "0"));
            ddl.Items.Add(new ListItem(" Collaborative model ", "1"));
            ddl.SetSelectItem((int)ath.AthUploadWay);
            this.Pub1.AddTD("colspan=2", ddl);
            this.Pub1.AddTREnd();

            this.Pub1.AddTR1();
            this.Pub1.AddTD(" Show the way ");
            ddl = new BP.Web.Controls.DDL();
            ddl.ID = "DDL_"+FrmAttachmentAttr.FileShowWay;
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("Table The way ", "0"));
            ddl.Items.Add(new ListItem(" Image Carousel way ", "1"));
            ddl.Items.Add(new ListItem(" Free Mode ", "2"));

            ddl.SetSelectItem((int)ath.FileShowWay);
            this.Pub1.AddTD("colspan=2", ddl);
            this.Pub1.AddTREnd();


            #region WebOffice Control mode .

            this.Pub1.AddTR1();
            this.Pub1.AddTDTitle("colspan=3", "WebOffice Control mode .");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            cb = new CheckBox();
            cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableWF;
            cb.Text = " Whether to enable weboffice?";
            cb.Checked = ath.IsWoEnableWF;
            this.Pub1.AddTD(cb);

            cb = new CheckBox();
            cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableSave;
            cb.Text = " Whether saving enabled ?";
            cb.Checked = ath.IsWoEnableSave;
            this.Pub1.AddTD(cb);

            cb = new CheckBox();
            cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableReadonly;
            cb.Text = " Is read-only ?";
            cb.Checked = ath.IsWoEnableReadonly;
            this.Pub1.AddTD(cb);
            this.Pub1.AddTREnd();
             


            this.Pub1.AddTR();
            cb = new CheckBox();
            cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableRevise;
            cb.Text = " Whether to amend enabled ?";
            cb.Checked = ath.IsWoEnableRevise;
            this.Pub1.AddTD(cb);

            cb = new CheckBox();
            cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableViewKeepMark;
            cb.Text = " View whether a user traces ?";
            cb.Checked = ath.IsWoEnableViewKeepMark;
            this.Pub1.AddTD(cb);

            cb = new CheckBox();
            cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnablePrint;
            cb.Text = " Whether to print ?";
            cb.Checked = ath.IsWoEnablePrint;
            this.Pub1.AddTD(cb);
            this.Pub1.AddTREnd();
             

            this.Pub1.AddTR();
            cb = new CheckBox();
            cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableOver;
            cb.Text = " Whether Taohong enabled ?";
            cb.Checked = ath.IsWoEnableOver;
            this.Pub1.AddTD(cb);

            cb = new CheckBox();
            cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableSeal;
            cb.Text = " Whether the signature is enabled ?";
            cb.Checked = ath.IsWoEnableSeal;
            this.Pub1.AddTD(cb);

            cb = new CheckBox();
            cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableTemplete;
            cb.Text = " Whether to enable the template file ?";
            cb.Checked = ath.IsWoEnableTemplete;
            this.Pub1.AddTD(cb);
          
            this.Pub1.AddTREnd();
            this.Pub1.AddTR();
            cb = new CheckBox();
            cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableCheck;
            cb.Text = " Whether the record node information ?";
            cb.Checked = ath.IsWoEnableCheck;
            this.Pub1.AddTD(cb);
            cb = new CheckBox();
            cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableInsertFlow;
            cb.Text = " Whether to enable the insertion process ?";
            cb.Checked = ath.IsWoEnableInsertFlow;
            this.Pub1.AddTD(cb);
            cb = new CheckBox();
            cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableInsertFengXian;
            cb.Text = " Whether the insertion point to enable risk ?";
            cb.Checked = ath.IsWoEnableInsertFengXian;
            this.Pub1.AddTD(cb);
            this.Pub1.AddTR();
            cb = new CheckBox();
            cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableMarks;
            cb.Text = " Whether to enter the traces mode ?";
            cb.Checked = ath.IsWoEnableMarks;
            this.Pub1.AddTD(cb);
            cb = new CheckBox();
            cb.ID = "CB_" + FrmAttachmentAttr.IsWoEnableDown;
            cb.Text = " Whether to download enabled ?";
            cb.Checked = ath.IsWoEnableDown;
            this.Pub1.AddTD(cb);
            this.Pub1.AddTD("");
            this.Pub1.AddTREnd();
      

            this.Pub1.AddTREnd();
            #endregion WebOffice Control mode .

            #region  Shortcuts generation rules .
            this.Pub1.AddTR1();
            this.Pub1.AddTDTitle("colspan=3", " Shortcuts generation rules .");
            this.Pub1.AddTREnd();


            this.Pub1.AddTR();
            cb = new CheckBox();
            cb.ID = "CB_" + FrmAttachmentAttr.FastKeyIsEnable;
            cb.Text = " Whether to generate shortcuts enabled ?( Enabling it will generate in the annex to the same directory in accordance with the rules )";
            cb.Checked = ath.FastKeyIsEnable; 
            this.Pub1.AddTD("colspan=3", cb);
            this.Pub1.AddTREnd();


            this.Pub1.AddTR();
            tb=new BP.Web.Controls.TB();
            tb.ID = "TB_" + FrmAttachmentAttr.FastKeyGenerRole;
            tb.Text = ath.FastKeyGenerRole;
            tb.Columns = 30;
            this.Pub1.AddTD("colspan=3", tb);
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD("colspan=3", " Format :*FiledName.*OID");
            this.Pub1.AddTREnd(); 
            #endregion  Shortcuts generation rules .

            #region  Save button .
            this.Pub1.AddTR();
            this.Pub1.AddTD("");
            Button btn = new Button();
            btn.ID = "Btn_Save";
            btn.Text = "  Save  ";
            btn.CssClass = "Btn";
            btn.Click += new EventHandler(btn_Click);
            this.Pub1.AddTD(btn);

            if (this.Ath != null)
            {
                btn = new Button();
                btn.ID = "Btn_Delete";
                btn.Text = "  Delete  ";
                btn.CssClass = "Btn";
                btn.Attributes["onclick"] = " return confirm(' You acknowledge that you ?');";
                btn.Click += new EventHandler(btn_Click);
                this.Pub1.AddTD(btn);
            }
            else
            {
                this.Pub1.AddTD();
            }
            this.Pub1.AddTREnd();
            #endregion  Save button .

            this.Pub1.AddTableEnd();
        }

        void btn_Click(object sender, EventArgs e)
        {
            FrmAttachment ath = new FrmAttachment();
            if (this.FK_Node == 0)
                ath.MyPK = this.FK_MapData + "_" + this.Ath;
            else
                ath.MyPK = this.FK_MapData + "_" + this.Ath + "_" + this.FK_Node;

            ath.RetrieveFromDBSources();

            Button btn = sender as Button;
            if (btn.ID == "Btn_Delete")
            {
                //ath.MyPK = this.FK_MapData + "_" + this.Ath;
                ath.Delete();
                this.WinClose(" Deleted successfully .");
                return;
            }

            ath.MyPK = this.FK_MapData + "_" + this.Ath;
            if (this.Ath != null)
                ath.RetrieveFromDBSources();
            ath = this.Pub1.Copy(ath) as FrmAttachment;
            ath.FK_MapData = this.FK_MapData;
            ath.FK_Node = this.FK_Node;
            if (string.IsNullOrEmpty(this.Ath)==false)
                ath.NoOfObj = this.Ath;

            if (this.FK_Node == 0)
                ath.MyPK = this.FK_MapData + "_" + ath.NoOfObj;
            else
                ath.MyPK = this.FK_MapData + "_" + ath.NoOfObj + "_" + this.FK_Node;

            GroupFields gfs1 = new GroupFields(this.FK_MapData);
            if (gfs1.Count == 1)
            {
                GroupField gf = (GroupField)gfs1[0];
                ath.GroupID = gf.OID;
            }
            else
            {
                ath.GroupID = this.Pub1.GetDDLByID("DDL_GroupField").SelectedItemIntVal;
            }

            // Special judge for the process .
            ath.HisCtrlWay = (AthCtrlWay)this.Pub1.GetDDLByID("DDL_CtrlWay").SelectedItemIntVal;
            ath.AthUploadWay = (AthUploadWay)this.Pub1.GetDDLByID("DDL_AthUploadWay").SelectedItemIntVal;
            ath.FileShowWay = (FileShowWay)this.Pub1.GetDDLByID("DDL_FileShowWay").SelectedItemIntVal; // Files show the way .
            

            ath.IsWoEnableWF = this.Pub1.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableWF).Checked;
            ath.IsWoEnableSave = this.Pub1.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableSave).Checked;
            ath.IsWoEnableReadonly = this.Pub1.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableReadonly).Checked;
            ath.IsWoEnableRevise = this.Pub1.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableRevise).Checked;
            ath.IsWoEnableViewKeepMark = this.Pub1.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableViewKeepMark).Checked;
            ath.IsWoEnablePrint = this.Pub1.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnablePrint).Checked;
            ath.IsWoEnableSeal = this.Pub1.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableSeal).Checked;
            ath.IsWoEnableOver = this.Pub1.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableOver).Checked;
            ath.IsWoEnableTemplete = this.Pub1.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableTemplete).Checked;
            ath.IsWoEnableCheck = this.Pub1.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableCheck).Checked;
            ath.IsWoEnableInsertFengXian = this.Pub1.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableInsertFengXian).Checked;
            ath.IsWoEnableInsertFlow = this.Pub1.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableInsertFlow).Checked;
            ath.IsWoEnableMarks = this.Pub1.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableMarks).Checked;
            ath.IsWoEnableDown = this.Pub1.GetCBByID("CB_" + FrmAttachmentAttr.IsWoEnableDown).Checked;


            ath.FastKeyIsEnable = this.Pub1.GetCBByID("CB_" + FrmAttachmentAttr.FastKeyIsEnable).Checked;
            ath.FastKeyGenerRole = this.Pub1.GetTBByID("TB_" + FrmAttachmentAttr.FastKeyGenerRole).Text;

            if (ath.FastKeyIsEnable == true)
                if (ath.FastKeyGenerRole.Contains("*OID") == false)
                    throw new Exception("@ Shortcuts generation rules must contain *OID, Doing so can cause duplicate filenames .");

            if (this.Ath == null)
            {
                ath.UploadType = (AttachmentUploadType)int.Parse(this.UploadType);

                if (this.FK_Node == 0)
                    ath.MyPK = this.FK_MapData + "_" + ath.NoOfObj;
                else
                    ath.MyPK = this.FK_MapData + "_" + ath.NoOfObj + "_" + this.FK_Node;

                if (ath.IsExits == true)
                {
                    this.Alert(" Annex No. (" + ath.NoOfObj + ") Already exists .");
                    return;
                }
                ath.Insert();
            }
            else
            {
                ath.NoOfObj = this.Ath;
                if (this.FK_Node == 0)
                    ath.MyPK = this.FK_MapData + "_" + this.Ath;
                else
                    ath.MyPK = this.FK_MapData + "_" + this.Ath + "_" + this.FK_Node;

                ath.Update();
            }
            this.WinCloseWithMsg(" Saved successfully ");
        }
    }
}