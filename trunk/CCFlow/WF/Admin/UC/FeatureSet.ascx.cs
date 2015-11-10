using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF.XML;
using BP.WF.Template;
using BP.WF;
using BP.En;

namespace CCFlow.WF.Admin.UC
{
    public partial class WF_Admin_UC_FeatureSet : BP.Web.UC.UCBase3
    {
        #region  Property .
        public int FK_Node
        {
            get
            {
                return int.Parse(this.Request.QueryString["FK_Node"]);
            }
        }
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        #endregion  Property .

        public void BindLeft()
        {
            FeatureSets fss = new FeatureSets();
            fss.RetrieveAll();

            this.Pub1.AddFieldSet(" Process characteristics ");
            this.Pub1.AddUL();
            foreach (FeatureSet fs in fss)
            {
                if (this.DoType == fs.No)
                {
                    this.Pub1.AddLiB("FeatureSetUI.aspx?FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.FK_Node + "&DoType=" + fs.No, fs.Name);
                    this.Lab = fs.Name;
                }
                else
                    this.Pub1.AddLi("FeatureSetUI.aspx?FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.FK_Node + "&DoType=" + fs.No, fs.Name);
            }
            this.Pub1.AddULEnd();

            this.Pub1.AddFieldSetEnd();  
        }
        public string Lab = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title =   " Feature Set ";
            this.BindLeft();
            if (this.DoType == null)
            {
                this.Pub2.AddFieldSet(" Help ", "  Modify the feature set is the entire process node specific attributes batch .");
                return;
            }

            this.Pub2.AddFieldSet(" Editor :" + this.Lab);
            this.Pub2.AddTable("border=0");
            this.Pub2.AddTR();
            switch (this.DoType)
            {
                case "Base":
                    this.Pub2.AddTDTitle(" Step ");
                    this.Pub2.AddTDTitle(" Node Name ");
                    this.Pub2.AddTDTitle(" Can I return ");
                    this.Pub2.AddTDTitle(" Whether it can be deleted ");
                    this.Pub2.AddTDTitle(" Whether it can be forwarded ");
                    this.Pub2.AddTDTitle(" No work permit allocation ?");
                    this.Pub2.AddTDTitle(" Can I view Report ?");
                    this.Pub2.AddTDTitle(" Is secrecy step ?");
                    break;
                case "FormType":
                    this.Pub2.AddTDTitle(" Step ");
                    this.Pub2.AddTDTitle(" Node Name ");
                    this.Pub2.AddTDTitle(" Type ");
                    this.Pub2.AddTDTitle("URL");
                    break;
                default:
                    this.Pub2.AddTDTitle(" Step ");
                    this.Pub2.AddTDTitle(" Node Name ");
                    this.Pub2.AddTDTitle(this.Lab);
                    break;
            }
            this.Pub2.AddTREnd();

            BP.WF.Template.Ext.NodeSheets nds = new BP.WF.Template.Ext.NodeSheets();
            nds.Retrieve("FK_Flow", this.FK_Flow);
            BP.WF.Template.Ext.NodeSheet mynd = new BP.WF.Template.Ext.NodeSheet();
            Attr attr = null;
            try
            {
                attr = mynd.EnMap.GetAttrByKey(this.DoType);
            }
            catch
            {
            }

            foreach (BP.WF.Template.Ext.NodeSheet nd in nds)
            {
                if (this.FK_Node == nd.NodeID)
                    this.Pub2.AddTR1();
                else
                    this.Pub2.AddTR();

                switch (this.DoType)
                {
                    case "Base":
                        break;
                    case "FormType":
                        this.Pub2.AddTDIdx(nd.Step);
                        this.Pub2.AddTD(nd.Name);
                        BP.Web.Controls.DDL ddl = new BP.Web.Controls.DDL();
                        ddl.ID = "DDL_" + nd.NodeID;
                        ddl.BindSysEnum(NodeAttr.FormType, nd.GetValIntByKey(NodeAttr.FormType));
                        this.Pub2.AddTD(ddl);
                        TextBox mytbURL = new TextBox();
                        mytbURL.ID = "TB_" + nd.NodeID;
                        mytbURL.Text = nd.GetValStringByKey(NodeAttr.FormUrl);
                        mytbURL.Columns = 50;
                        this.Pub2.AddTD(mytbURL);
                        break;
                    default:
                        this.Pub2.AddTDIdx(nd.Step);
                        this.Pub2.AddTD(nd.Name);
                        switch (attr.UIContralType)
                        {
                            case UIContralType.TB:
                                TextBox mytb = new TextBox();
                                mytb.ID = "TB_" + nd.NodeID;
                                mytb.Text = nd.GetValStringByKey(this.DoType);
                                mytb.Columns = 50;
                                this.Pub2.AddTD(mytb);
                                break;
                            case UIContralType.CheckBok:
                                CheckBox mycb = new CheckBox();
                                mycb.ID = "CB_" + nd.NodeID;
                                mycb.Text = attr.Desc;
                                mycb.Checked = nd.GetValBooleanByKey(this.DoType);
                                this.Pub2.AddTD(mycb);
                                break;
                            case UIContralType.DDL:
                                BP.Web.Controls.DDL ddlm = new BP.Web.Controls.DDL();
                                ddlm.ID = "DDL_" + nd.NodeID;
                                ddlm.BindSysEnum(attr.UIBindKey, nd.GetValIntByKey(this.DoType));
                                this.Pub2.AddTD(ddlm);
                                break;
                            default:
                                break;
                        }
                        break;
                }
                this.Pub2.AddTREnd();
            }
            this.Pub2.AddTableEndWithHR();
            Button btn = new Button();
            btn.ID = "Btn_Save";
            btn.Text = " Save ";
            btn.CssClass = "Btn";
            btn.Click += new EventHandler(btn_Click);
            this.Pub2.Add(btn);
            this.Pub2.AddFieldSetEnd();
        }

        void btn_Click(object sender, EventArgs e)
        {
            BP.WF.Template.Ext.NodeSheets nds = new BP.WF.Template.Ext.NodeSheets();
            nds.Retrieve("FK_Flow", this.FK_Flow);
            BP.WF.Template.Ext.NodeSheet mynd = new BP.WF.Template.Ext.NodeSheet();
            Attr attr = null;
            try
            {
                attr = mynd.EnMap.GetAttrByKey(this.DoType);
            }
            catch
            {
            }

            foreach (BP.WF.Template.Ext.NodeSheet nd in nds)
            {
                switch (this.DoType)
                {
                    case "Base":
                        break;
                    case "FormType":
                        nd.SetValByKey(NodeAttr.FormType, this.Pub2.GetDDLByID("DDL_" + nd.NodeID).SelectedItemStringVal);
                        nd.SetValByKey(NodeAttr.FormUrl, this.Pub2.GetTextBoxByID("TB_" + nd.NodeID).Text);
                        nd.Update();
                        break;
                    default:
                        switch (attr.UIContralType)
                        {
                            case UIContralType.TB:
                                nd.SetValByKey(this.DoType, this.Pub2.GetTextBoxByID("TB_" + nd.NodeID).Text);
                                break;
                            case UIContralType.CheckBok:
                                nd.SetValByKey(this.DoType, this.Pub2.GetCBByID("CB_" + nd.NodeID).Checked);
                                break;
                            case UIContralType.DDL:
                                nd.SetValByKey(this.DoType, this.Pub2.GetDDLByID("DDL_" + nd.NodeID).SelectedItemStringVal);
                                break;
                            default:
                                break;
                        }
                        nd.Update();
                        break;
                }
            }
        }
    }

}