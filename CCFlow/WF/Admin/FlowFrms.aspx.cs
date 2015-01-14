using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Web.Controls;
using BP.WF;
using BP.WF.Template;
using BP.En;
using BP.DA;
using BP.Sys;
using BP;
namespace CCFlow.WF.Admin
{
    public partial class WF_Admin_FlowFrms : BP.Web.WebPage
    {
        #region  Property .
        /// <summary>
        ///  Display Type 
        /// </summary>
        public string ShowType
        {
            get
            {
                string s = this.Request.QueryString["ShowType"];
                if (s == null)
                    return "FrmLib";
                return s;
            }
        }
        public string FK_MapData
        {
            get
            {
                return this.Request.QueryString["FK_MapData"];
            }
        }
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        /// <summary>
        ///  Node 
        /// </summary>
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
                     return int.Parse(this.Request.QueryString["FK_Flow"]);
                }
            }
        }
        #endregion  Property .

        protected void Page_Load(object sender, EventArgs e)
        {
            #region  Executive Function .
            if (this.IsPostBack==false)
            {
                switch (this.DoType)
                {
                    case "Up":
                        FrmNode fnU = new FrmNode(this.MyPK);
                        fnU.DoUp();
                        break;
                    case "Down":
                        FrmNode fnD = new FrmNode(this.MyPK);
                        fnD.DoDown();
                        break;
                    case "DelFrm":
                        FrmNodes fnsR = new FrmNodes();
                        if (fnsR.Retrieve(FrmNodeAttr.FK_Frm, this.FK_MapData) != 0)
                        {
                            this.Alert(" This form has been more than one process node (" + fnsR.Count + ") Binding , So you can not delete it .");
                        }
                        else
                        {
                            MapData md = new MapData();
                            md.No = this.FK_MapData;
                            md.Delete();
                        }
                        break;
                    case "Del":
                        FrmNodes fns = new FrmNodes(this.FK_Flow, this.FK_Node);
                        foreach (FrmNode fn in fns)
                        {
                            if (fn.FK_Frm == this.FK_MapData)
                            {
                                fn.Delete();
                                break;
                            }
                        }
                        break;
                    case "Add":
                        FrmNode fnN = new FrmNode();
                        fnN.FK_Frm = this.FK_MapData;
                        fnN.FK_Node = this.FK_Node;
                        fnN.FK_Flow = this.FK_Flow;
                        fnN.Save();
                        break;
                    default:
                        break;
                }
            }
            #endregion  Executive Function .

            switch (this.ShowType)
            {
                case "Frm":
                    this.BindFrm();
                    this.Title = " Form ";
                    break;
                case "FrmLib":
                case "FrmLab":
                    this.BindFrmLib();
                    this.Title = " Form Library ";
                    break;
                case "FlowFrms":
                    this.BindFlowFrms();
                    this.Title = " Process Form ";
                    break;
                case "FrmSorts":
                    this.BindFrmSorts();
                    this.Title = " Process Category ";
                    break;
                case "EditPowerOrder": // Edit Permissions and order .
                    this.BindEditPowerOrder();
                    break;
                default:
                    break;
            }

            this.BindLeft();
        }
        /// <summary>
        ///  Edit Permissions and order 
        /// </summary>
        public void BindEditPowerOrder()
        {
            this.Pub1.AddH2(" Permissions and display order form ");
            this.Pub1.AddHR();
            this.Pub1.AddTable("align=left");
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("IDX");
            this.Pub1.AddTDTitle(" Serial number ");
            this.Pub1.AddTDTitle(" Name ");
            this.Pub1.AddTDTitle(" Display mode ");
            this.Pub1.AddTDTitle(" Can edit ?");
            this.Pub1.AddTDTitle(" Can print ");
            this.Pub1.AddTDTitle(" Access control scheme ");
            this.Pub1.AddTDTitle(" Custom ");
            this.Pub1.AddTDTitle(" Who is the primary key ?");
            this.Pub1.AddTDTitle(" Order ");
            this.Pub1.AddTDTitle("");
            this.Pub1.AddTDTitle("");
            this.Pub1.AddTREnd();

            FrmNodes fns = new FrmNodes(this.FK_Flow, this.FK_Node);
            int idx = 1;
            foreach (FrmNode fn in fns)
            {
                this.Pub1.AddTR();
                this.Pub1.AddTDIdx(idx++);
                this.Pub1.AddTD(fn.FK_Frm);

                MapData md = new MapData(fn.FK_Frm);
                md = new MapData(fn.FK_Frm);
                // this.Pub1.AddTD(md.Name);
                this.Pub1.AddTDA("FlowFrms.aspx?ShowType=Frm&FK_MapData=" + md.No + "&FK_Node=" + this.FK_Node, md.Name);

                DDL ddl = new DDL();
                ddl.ID = "DDL_FrmType_" + fn.FK_Frm;
                ddl.BindSysEnum("FrmType", (int)fn.HisFrmType);
                this.Pub1.AddTD(ddl);

                CheckBox cb = new CheckBox();
                cb.ID = "CB_IsEdit_" + md.No;
                cb.Text = " Can edit ?";
                cb.Checked = fn.IsEdit;
                this.Pub1.AddTD(cb);

                cb = new CheckBox();
                cb.ID = "CB_IsPrint_" + md.No;
                cb.Text = " Can print ";
                cb.Checked = fn.IsPrint;
                this.Pub1.AddTD(cb);


                ddl = new DDL();
                ddl.ID = "DDL_Sln_" + md.No;
                //   ddl.BindAtParas(md.Slns);
                ddl.Items.Add(new ListItem(" The default program ", "0"));
                ddl.Items.Add(new ListItem(" Custom ", this.FK_Node.ToString()));
                ddl.SetSelectItem(fn.FrmSln); // Set access control scheme .
                this.Pub1.AddTD(ddl);

                this.Pub1.AddTD("<a href=\"javascript:WinField('" + md.No + "','" + this.FK_Node + "','" + this.FK_Flow + "')\" > Field </a>|<a href=\"javascript:WinFJ('" + md.No + "','" + this.FK_Node + "','" + this.FK_Flow + "')\" > Accessory </a>");

                ddl = new DDL();
                ddl.ID = "DDL_WhoIsPK_" + md.No;
                ddl.BindSysEnum("WhoIsPK");
                ddl.SetSelectItem( (int)fn.WhoIsPK ); // Who is the primary key ?.
                this.Pub1.AddTD(ddl);

                TextBox tb = new TextBox();
                tb.ID = "TB_Idx_" + md.No;
                tb.Text = fn.Idx.ToString();
                tb.Columns = 5;
                this.Pub1.AddTD(tb);

                this.Pub1.AddTDA("FlowFrms.aspx?ShowType=EditPowerOrder&FK_Node=" + this.FK_Node + "&FK_Flow="+this.FK_Flow+"&MyPK=" + fn.MyPK + "&DoType=Up", " Move ");
                this.Pub1.AddTDA("FlowFrms.aspx?ShowType=EditPowerOrder&FK_Node=" + this.FK_Node + "&FK_Flow=" + this.FK_Flow + "&MyPK=" + fn.MyPK + "&DoType=Down", " Down ");

                this.Pub1.AddTREnd();
            }

            this.Pub1.AddTR();
            Button btn = new Button();
            btn.ID = "Save";
            btn.Text = "  Save  ";
            btn.CssClass = "Btn";
            btn.Click += new EventHandler(btn_SavePowerOrders_Click);
            this.Pub1.AddTD("colspan=12", btn);
            this.Pub1.AddTREnd();
            this.Pub1.AddTableEnd();
        }

        void btn_SavePowerOrders_Click(object sender, EventArgs e)
        {
            FrmNodes fns = new FrmNodes(this.FK_Flow, this.FK_Node);
            foreach (FrmNode fn in fns)
            {
                fn.IsEdit = this.Pub1.GetCBByID("CB_IsEdit_" + fn.FK_Frm).Checked;
                fn.IsPrint = this.Pub1.GetCBByID("CB_IsPrint_" + fn.FK_Frm).Checked;
                fn.Idx = int.Parse(this.Pub1.GetTextBoxByID("TB_Idx_" + fn.FK_Frm).Text);
                fn.HisFrmType = (BP.Sys.FrmType)this.Pub1.GetDDLByID("DDL_FrmType_" + fn.FK_Frm).SelectedItemIntVal;

                // Access control scheme .
                fn.FrmSln = this.Pub1.GetDDLByID("DDL_Sln_" + fn.FK_Frm).SelectedItemIntVal;
                fn.WhoIsPK = (WhoIsPK)this.Pub1.GetDDLByID("DDL_WhoIsPK_" + fn.FK_Frm).SelectedItemIntVal;

                fn.FK_Flow = this.FK_Flow;
                fn.FK_Node = this.FK_Node;
                //fn.FK_Frm = 

                fn.MyPK = fn.FK_Frm + "_" + fn.FK_Node + "_" + fn.FK_Flow;

                fn.Update();
            }
            this.Response.Redirect("FlowFrms.aspx?ShowType=EditPowerOrder&FK_Node=" + this.FK_Node+"&FK_Flow="+this.FK_Flow, true);
        }
        public void BindFrmSorts()
        {
            SysFormTrees fss = new SysFormTrees();
            fss.RetrieveAll();
            this.Pub1.AddH2(" Form Category Maintenance ");
            this.Pub1.AddHR();

            this.Pub1.AddTable("align=left");
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle(" No. ");
            this.Pub1.AddTDTitle(" Category Number ");
            this.Pub1.AddTDTitle(" Category Name ");
            this.Pub1.AddTREnd();

            for (int i = 1; i <= 15; i++)
            {
                this.Pub1.AddTR();
                this.Pub1.AddTDIdx(i);


                TextBox tb = new TextBox();
                tb.Text = i.ToString().PadLeft(2, '0');
                SysFormTree fs = fss.GetEntityByKey(SysFormTreeAttr.No, tb.Text) as SysFormTree;

                tb.ID = "TB_No_" + i;
                tb.Columns = 5;
                tb.ReadOnly = true;
                this.Pub1.AddTD(tb.Text);

                tb = new TextBox();
                tb.ID = "TB_Name_" + i;
                tb.Columns = 40;
                if (fs != null)
                    tb.Text = fs.Name;

                this.Pub1.AddTD(tb);
                this.Pub1.AddTREnd();
            }

            Button btn = new Button();
            btn.Text = "Save";
            btn.CssClass = "Btn";
            btn.Click += new EventHandler(btn_SaveFrmSort_Click);
            this.Pub1.Add(btn);

            this.Pub1.AddTR();
            this.Pub1.AddTD("colspan=2", btn);
            this.Pub1.AddTD(" To delete a category , Please clear the saved data to a text box .");
            this.Pub1.AddTREnd();

            this.Pub1.AddTableEndWithHR();

        }

        void btn_SaveFrmSort_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= 15; i++)
            {
                TextBox tbName = this.Pub1.GetTextBoxByID("TB_Name_" + i);
                SysFormTree fs = new SysFormTree();
                fs.No = i.ToString().PadLeft(2, '0');
                fs.Name = tbName.Text.ToString();
                if (fs.Name.Length > 1)
                    fs.Save();
                else
                    fs.Delete();
            }
            this.Alert(" Saved successfully ");
        }
        public void BindFlowFrms()
        {
            FrmNodes fns = new FrmNodes(this.FK_Flow, this.FK_Node);
            this.Pub1.AddH2(" Binding Process Form ");
            this.Pub1.AddTable("align=left");
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("Idx");
            this.Pub1.AddTDTitle(" Form Number ");
            this.Pub1.AddTDTitle(" Name ");
            this.Pub1.AddTDTitle(" Physical table ");
             this.Pub1.AddTDTitle(" Competence ");
            this.Pub1.AddTREnd();

            BP.WF.Node nd = new BP.WF.Node(this.FK_Node);

            MapDatas mds = new MapDatas();
            QueryObject obj_mds = new QueryObject(mds);
            obj_mds.AddWhere(MapDataAttr.AppType, (int)AppType.Application);
            obj_mds.addOrderBy(MapDataAttr.Name);
            obj_mds.DoQuery();
            //FrmSorts fss = new FrmSorts();
            //fss.RetrieveAll();
            SysFormTrees formTrees = new SysFormTrees();
            QueryObject objInfo = new QueryObject(formTrees);
            objInfo.AddWhere(SysFormTreeAttr.ParentNo,"0");
            objInfo.addOrderBy(SysFormTreeAttr.Name);
            objInfo.DoQuery();

            int idx = 0;
            foreach (SysFormTree fs in formTrees)
            {
                idx++;
                this.Pub1.AddTR();
                this.Pub1.AddTDIdx(idx);
                this.Pub1.AddTDB("colspan=4", fs.Name);
                this.Pub1.AddTREnd();
                foreach (MapData md in mds)
                {
                    if (md.FK_FormTree != fs.No)
                        continue;
                    idx++;
                    this.Pub1.AddTR();
                    this.Pub1.AddTDIdx(idx);

                    CheckBox cb = new CheckBox();
                    cb.ID = "CB_" + md.No;
                    cb.Text = md.No;
                    cb.Checked = fns.Contains(FrmNodeAttr.FK_Frm, md.No);

                    this.Pub1.AddTD(cb);
                    this.Pub1.AddTD("<a href='../MapDef/CCForm/Frm.aspx?FK_MapData=" + md.No + "&FK_Flow=" + this.FK_Flow + "'  target=_blank>" + md.Name + "</a>");
                    this.Pub1.AddTD(md.PTable);

                    if (cb.Checked)
                        this.Pub1.AddTD("<a href=\"javascript:WinField('" + md.No + "','" + this.FK_Node + "','" + this.FK_Flow + "')\"> Field </a>|<a href=\"javascript:WinFJ('" + md.No + "','" + this.FK_Node + "','" + this.FK_Flow + "')\"> Accessory </a>");
                    else
                        this.Pub1.AddTD();
                    //this.Pub1.AddTD(md.Designer);
                    //this.Pub1.AddTD(md.DesignerUnit);
                    //this.Pub1.AddTD(md.DesignerContact);
                    this.Pub1.AddTREnd();
                }
                AddChildNode(fs.No, mds, fns);
            }
            Button btn = new Button();
            btn.ID = "Btn_Save";
            btn.Text = "Save";
            btn.CssClass = "Btn";
            btn.Click += new EventHandler(btn_SaveFlowFrms_Click);
            this.Pub1.AddTR();
            this.Pub1.AddTD("colspan=5", btn);
            this.Pub1.AddTREnd();
            this.Pub1.AddTableEnd();
        }

        private void AddChildNode(string parentNo, MapDatas mds, FrmNodes fns)
        {
            SysFormTrees formTrees = new SysFormTrees();
            QueryObject objInfo = new QueryObject(formTrees);
            objInfo.AddWhere(SysFormTreeAttr.ParentNo, parentNo);
            objInfo.addOrderBy(SysFormTreeAttr.Name);
            objInfo.DoQuery();

            int idx = 0;
            foreach (SysFormTree fs in formTrees)
            {
                idx++;
                foreach (MapData md in mds)
                {
                    if (md.FK_FormTree != fs.No)
                        continue;
                    idx++;
                    this.Pub1.AddTR();
                    this.Pub1.AddTDIdx(idx);

                    CheckBox cb = new CheckBox();
                    cb.ID = "CB_" + md.No;
                    cb.Text = md.No;
                    cb.Checked = fns.Contains(FrmNodeAttr.FK_Frm, md.No);

                    this.Pub1.AddTD(cb);
                    this.Pub1.AddTD(md.Name);
                    this.Pub1.AddTD(md.PTable);
                    this.Pub1.AddTREnd();
                }
                AddChildNode(fs.No, mds, fns);
            }
        }
        void btn_SaveFlowFrms_Click(object sender, EventArgs e)
        {
            FrmNodes fns = new FrmNodes(this.FK_Flow, this.FK_Node);
            MapDatas mds = new MapDatas();
            mds.Retrieve(MapDataAttr.AppType, (int)AppType.Application);

            //BP.WF.Node nd = new BP.WF.Node(this.FK_Node);

            string ids = ",";
            foreach (MapData md in mds)
            {
                CheckBox cb = this.Pub1.GetCBByID("CB_" + md.No);
                if (cb == null || cb.Checked == false)
                    continue;
                ids += md.No + ",";
            }

            // Delete deleted .
            foreach (FrmNode fn in fns)
            {
                if (ids.Contains("," + fn.FK_Frm + ",") == false)
                {
                    fn.Delete();
                    continue;
                }
            }

            //  No increase in the collection of .
            string[] strs = ids.Split(',');
            foreach (string s in strs)
            {
                if (string.IsNullOrEmpty(s))
                    continue;
                if (fns.Contains(FrmNodeAttr.FK_Frm, s))
                    continue;

                FrmNode fn = new FrmNode();
                fn.FK_Frm = s;
                fn.FK_Flow = this.FK_Flow;
                fn.FK_Node = this.FK_Node;
                fn.Save();
            }
            this.Response.Redirect("FlowFrms.aspx?ShowType=EditPowerOrder&FK_Node=" + this.FK_Node+"&FK_Flow="+this.FK_Flow, true);
        }
        public void BindFrmLib()
        {
            this.Pub1.AddH2(" Form Library ");

            this.Pub1.AddTable("width=100% align=left");
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("Idx");
            this.Pub1.AddTDTitle(" Form Number ");
            this.Pub1.AddTDTitle(" Name ");
            this.Pub1.AddTDTitle(" Physical table ");

            //this.Pub1.AddTDTitle(" Designers ");
            //this.Pub1.AddTDTitle(" Design units ");
            //this.Pub1.AddTDTitle(" Contact ");
            this.Pub1.AddTREnd();

            MapDatas mds = new MapDatas();
            mds.Retrieve(MapDataAttr.AppType, (int)AppType.Application);

            SysFormTrees fss = new SysFormTrees();
            fss.RetrieveAll();
            int idx = 0;
            foreach (SysFormTree fs in fss)
            {
                idx++;
                this.Pub1.AddTR();
                this.Pub1.AddTDIdx(idx);
                this.Pub1.AddTD("colspan=6", "<b>" + fs.Name + "</b>");
                this.Pub1.AddTREnd();
                foreach (MapData md in mds)
                {
                    if (md.FK_FrmSort != fs.No)
                        continue;
                    idx++;
                    this.Pub1.AddTR();
                    this.Pub1.AddTDIdx(idx);
                    this.Pub1.AddTD(md.No);
                    this.Pub1.AddTDA("FlowFrms.aspx?ShowType=Frm&FK_MapData=" + md.No + "&FK_Node=" + this.FK_Node+"&FK_Flow="+this.FK_Flow, md.Name);
                    this.Pub1.AddTD(md.PTable);
                    //this.Pub1.AddTD(md.Designer);
                    //this.Pub1.AddTD(md.DesignerUnit);
                    //this.Pub1.AddTD(md.DesignerContact);
                    this.Pub1.AddTREnd();

                    //this.Pub1.AddTR();
                    //this.Pub1.AddTD();
                    //this.Pub1.AddTD();
                    //this.Pub1.AddTDBegin("colspan=5");
                    //this.Pub1.AddTDEnd();
                    //this.Pub1.AddTREnd();
                }
            }
            this.Pub1.AddTableEnd();
        }
        public void BindFrm()
        {
            MapData md = new MapData();
            if (string.IsNullOrEmpty(this.FK_MapData) == false)
            {
                md.No = this.FK_MapData;
                md.RetrieveFromDBSources();
                this.Pub1.AddH2(" Form Properties " + md.Name);
                this.Pub1.AddHR();
            }
            else
            {
                this.Pub1.AddH2(" New Form ");
                this.Pub1.AddHR();
            }

            this.Pub1.AddTable("align=left");
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle(" Property ");
            this.Pub1.AddTDTitle(" Collection ");
            this.Pub1.AddTDTitle(" Description ");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Form Name ");
            TextBox tb = new TextBox();
            tb.ID = "TB_Name";
            tb.Text = md.Name;
            this.Pub1.AddTD(tb);
            this.Pub1.AddTD(" Description ");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Form Number ");
            tb = new TextBox();
            tb.ID = "TB_No";
            tb.Text = md.No;
            if (string.IsNullOrEmpty(md.No) == false)
                tb.Attributes["readonly"] = "true";

            this.Pub1.AddTD(tb);
            this.Pub1.AddTD(" Also form ID.");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Form type ");
            DDL ddl = new DDL();
            ddl.ID = "DDL_FrmType";
            ddl.BindSysEnum(MapDataAttr.FrmType, (int)md.HisFrmType);
            this.Pub1.AddTD(ddl);
            this.Pub1.AddTD("");
            this.Pub1.AddTREnd();


            this.Pub1.AddTR();
            this.Pub1.AddTD(" Physical table / View ");
            tb = new TextBox();
            tb.ID = "TB_PTable";
            tb.Text = md.No;
            this.Pub1.AddTD(tb);
            this.Pub1.AddTD(" Multiple forms can correspond to the same table or view <br> If the table does not exist ,ccflow Will automatically create .");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Category ");
            ddl = new DDL();
            ddl.ID = "DDL_FK_FrmSort";
            SysFormTrees fss = new SysFormTrees();
            fss.RetrieveAll();

            ddl.Bind(fss, md.FK_FrmSort);

            this.Pub1.AddTD(ddl);
            this.Pub1.AddTD("");
            this.Pub1.AddTREnd();

            //this.Pub1.AddTR();
            //this.Pub1.AddTD(" Design units ");
            //tb = new TextBox();
            //tb.ID = "TB_" + MapDataAttr.DesignerUnit;
            //tb.Text = md.DesignerUnit;
            //if (string.IsNullOrEmpty(tb.Text))
            //    tb.Text = BP.Sys.SystemConfig.DeveloperName;

            //tb.Columns = 60;
            //this.Pub1.AddTD("colspan=2", tb);
            //this.Pub1.AddTREnd();

            //this.Pub1.AddTR();
            //this.Pub1.AddTD(" Contact ");
            //tb = new TextBox();
            //tb.ID = "TB_" + MapDataAttr.DesignerContact;
            //tb.Text = md.DesignerContact;
            //if (string.IsNullOrEmpty(tb.Text))
            //    tb.Text = BP.Sys.SystemConfig.ServiceTel + "," + BP.Sys.SystemConfig.ServiceMail;
            //tb.Columns = 60;
            //this.Pub1.AddTD("colspan=2", tb);
            //this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD("");
            this.Pub1.AddTDBegin();
            Button btn = new Button();
            btn.ID = "Btn_Save";
            btn.Text = "Save";
            btn.CssClass = "Btn";
            btn.Click += new EventHandler(btn_SaveFrm_Click);
            this.Pub1.Add(btn);

            if (string.IsNullOrEmpty(md.No) == false)
            {
                btn = new Button();
                btn.ID = "Btn_Delete";
                btn.Text = "Delete";
                btn.CssClass = "Btn";
                btn.Attributes["onclick"] = "return window.confirm(' Are you sure you want to delete it ?')";
                btn.Click += new EventHandler(btn_SaveFrm_Click);
                this.Pub1.Add(btn);
            }

            this.Pub1.AddTDEnd();
            this.Pub1.AddTD("");
            this.Pub1.AddTREnd();

            if (string.IsNullOrEmpty(md.No) == false)
            {
                this.Pub1.AddTR();
                this.Pub1.AddTDBegin("colspan=3");
                //// this.Pub1.Add("<a href='FlowFrms.aspx?ShowType=FrmLib&DoType=DelFrm&FK_Node=" + FK_Node + "&FK_MapData=" + md.No + "'  ><img src='./Img/Btn/Delete.gif' border=0 /> Delete </a>");
                //this.Pub1.Add("<a href='../MapDef/ViewFrm.aspx?DoType=Column4Frm&FK_MapData=" + md.No + "&FK_Flow=" + this.FK_Flow + "' target=_blank  ><img src='../Img/Btn/View.gif' border=0 /> Fool Form Preview </a>");
                //this.Pub1.Add("<a href='../CCForm/Frm.aspx?FK_MapData=" + md.No + "&FK_Flow=" + this.FK_Flow + "&IsTest=1&WorkID=0' target=_blank  ><img src='../Img/Btn/View.gif' border=0 /> Freedom Form Preview </a>");
                //this.Pub1.Add("<a href='../MapDef/ViewFrm.aspx?DoType=dd&FK_MapData=" + md.No + "&FK_Flow=" + this.FK_Flow + "' target=_blank  ><img src='../Img/Btn/View.gif' border=0 /> Phone Form Preview </a>");
                //this.Pub1.Add("<a href='../CCForm/Frm.aspx?FK_MapData=" + md.No + "&FK_Flow=" + this.FK_Flow + "' target=_blank  ><img src='../Img/Btn/View.gif' border=0 /> Start freedom Form Designer </a>");
                //this.Pub1.Add("<a href='../MapDef/MapDef.aspx?PK=" + md.No + "&FK_Flow=" + this.FK_Flow + "' target=_blank  ><img src='../Img/Btn/View.gif' border=0 /> Start fool Form Designer </a>");
                
                this.Pub1.Add("<a href='../CCForm/Frm.aspx?FK_MapData=" + md.No + "&FK_Flow=" + this.FK_Flow + "' target=_blank  ><img src='../Img/Btn/View.gif' border=0 /> Start freedom Form Designer </a>");

                this.Pub1.AddTDEnd();
                this.Pub1.AddTREnd();
            }
            this.Pub1.AddTableEnd();
        }

        void btn_SaveFrm_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.ID == "Btn_Delete")
            {
                //MapData mdDel = new MapData();
                //mdDel.No = this.FK_MapData;
                //mdDel.Delete();
                this.Response.Redirect("FlowFrms.aspx?ShowType=Frm&DoType=DelFrm&FK_Node=" + this.FK_Node + "&FK_MapData=" + this.FK_MapData + "&FK_Flow=" + this.FK_Flow, true);
                return;
            }
            MapData md = new MapData();
            if (string.IsNullOrEmpty(this.FK_MapData) == false)
            {
                md.No = this.FK_MapData;
                md.RetrieveFromDBSources();
            }
            md = (MapData)this.Pub1.Copy(md);

            md.HisFrmTypeInt = this.Pub1.GetDDLByID("DDL_" + MapDataAttr.FrmType).SelectedItemIntVal;
            md.FK_FrmSort = this.Pub1.GetDDLByID("DDL_" + MapDataAttr.FK_FrmSort).SelectedItemStringVal;
            md.HisAppType = AppType.Application;
            if (string.IsNullOrEmpty(this.FK_MapData) == true)
            {
                if (md.IsExits == true)
                {
                    this.Alert(" Form Number (" + md.No + ") Already exists ");
                    return;
                }
                else
                {
                    md.Insert();
                    this.Response.Redirect("FlowFrms.aspx?ShowType=Frm&FK_Node=" + this.FK_Node + "&FK_MapData=" + md.No + "&FK_Flow=" + this.FK_Flow, true);
                }
            }
            else
            {
                md.Update();
                this.Alert(" Update successful .");
            }
        }
        public void BindLeft()
        {
            this.Left.Add("<a href='http://ccflow.org' target=_blank ><img src='../../DataUser/ICON/" + SystemConfig.CompanyID + "/LogBiger.png' border=0/></a>");
            this.Left.AddHR();

            this.Left.AddUL();
            ////if (this.FK_Node == 0)
            ////{
            //    this.Left.AddLi("<a href=\"FlowFrms.aspx?ShowType=FrmLib&FK_Node=" + this.FK_Node + "&FK_MapData=" + this.FK_MapData + "&FK_Flow=" + this.FK_Flow + "\"><b> Form Library </b></a>");
            //    this.Left.Add(" Check out , Modification , Design , Form .<br><br>");

            //    this.Left.AddLi("<a href=\"FlowFrms.aspx?ShowType=FrmSorts&FK_Node=" + this.FK_Node + "&FK_MapData=" + this.FK_MapData + "&FK_Flow=" + this.FK_Flow + "\"><b> Category Maintenance </b></a>");
            //    this.Left.Add(" Maintenance Forms category .<br><br>");

            //    this.Left.AddLi("<a href=\"FlowFrms.aspx?ShowType=Frm&FK_Node=" + this.FK_Node + "&FK_Flow=" + this.FK_Flow + "\"><b> New Form </b></a>");
            //    this.Left.Add(" New Form .<br><br>");
            ////}
            //else
            //{
            this.Left.AddLi("<a href=\"FlowFrms.aspx?ShowType=FrmLib&FK_Node=" + this.FK_Node + "&FK_MapData=" + this.FK_MapData + "&FK_Flow=" + this.FK_Flow + "\"><b> Form Library </b></a>-<a href=\"FlowFrms.aspx?ShowType=FrmSorts&FK_Node=" + this.FK_Node + "&FK_MapData=" + this.FK_MapData + "\"><b> Category Maintenance </b></a>-<a href=\"FlowFrms.aspx?ShowType=Frm&FK_Node=" + this.FK_Node + "\"><b> New Form </b></a>");
            this.Left.Add(" Forms Library Maintenance , Category Maintenance , New Form <br><br>");

            this.Left.AddLi("<a href=\"FlowFrms.aspx?ShowType=FlowFrms&FK_Node=" + this.FK_Node + "&FK_MapData=" + this.FK_MapData + "&FK_Flow=" + this.FK_Flow + "\"><b> Increase removed form the binding process </b></a>");
            this.Left.Add(" Add or remove columns in a query result set contents .<br><br>");

            this.Left.AddLi("<a href=\"FlowFrms.aspx?ShowType=EditPowerOrder&FK_Node=" + this.FK_Node + "&FK_MapData=" + this.FK_MapData + "&FK_Flow=" + this.FK_Flow + "\"><b> Permissions and display order form </b></a>");
            this.Left.Add(" Form in the permissions control the display order of the node .<br><br>");
            //}
            this.Left.AddULEnd();
        }
    }
}