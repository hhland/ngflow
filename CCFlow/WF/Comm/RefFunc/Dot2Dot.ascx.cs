using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BP.Port;
using BP.Web.Controls;
using BP.Web;
using BP.DA;
using BP.En;
using BP.Sys;
using BP;

namespace CCFlow.WF.Comm.RefFunc
{
    public partial class Dot2Dot_UC : BP.Web.UC.UCBase3
    {
        #region  Property .
        public AttrOfOneVSM AttrOfOneVSM
        {
            get
            {
                Entity en = ClassFactory.GetEn(this.EnName);
                foreach (AttrOfOneVSM attr in en.EnMap.AttrsOfOneVSM)
                {
                    if (attr.EnsOfMM.ToString() == this.AttrKey)
                    {
                        return attr;
                    }
                }
                throw new Exception(" Error not found property ． ");
            }
        }
        /// <summary>
        ///  A working class 
        /// </summary>
        public new string EnsName
        {
            get
            {
                return this.Request.QueryString["EnsName"];
            }
        }
        public string AttrKey
        {
            get
            {
                return this.Request.QueryString["AttrKey"];
            }
        }
        public string PK
        {
            get
            {
                if (ViewState["PK"] == null)
                {
                    string pk = this.Request.QueryString["PK"];
                    if (pk == null)
                        pk = this.Request.QueryString["No"];

                    if (pk == null)
                        pk = this.Request.QueryString["RefNo"];

                    if (pk == null)
                        pk = this.Request.QueryString["OID"];

                    if (pk == null)
                        pk = this.Request.QueryString["MyPK"];


                    if (pk != null)
                    {
                        ViewState["PK"] = pk;
                    }
                    else
                    {
                        Entity mainEn = BP.En.ClassFactory.GetEn(this.EnName);
                        ViewState["PK"] = this.Request.QueryString[mainEn.PK];
                    }
                }

                return ViewState["PK"].ToString();
            }
        }
        public DropDownList DDL_Group
        {
            get
            {
                return this.ToolBar1.GetDropDownListByID("DDL_Group");
            }
        }

        public bool IsLine
        {
            get
            {
                try
                {
                    return (bool)ViewState["IsLine"];
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                ViewState["IsLine"] = value;
            }
        }
        public string MainEnName
        {
            get
            {
                return ViewState["MainEnName"] as string;
            }
            set
            {
                this.ViewState["MainEnName"] = value;
            }
        }
        public string MainEnPKVal
        {
            get
            {
                return ViewState["MainEnPKVal"] as string;
            }
            set
            {
                this.ViewState["MainEnPKVal"] = value;
            }
        }
        public bool IsTreeShowWay
        {
            get
            {
                if (this.Request.QueryString["IsTreeShowWay"] != null)
                    return true;
                return false;
            }
        }
        /// <summary>
        ///  Display mode 
        /// </summary>
        public string ShowWay
        {
            get
            {
                string str = this.Request.QueryString["ShowWay"];
                if (str == null)
                    str = this.DDL_Group.SelectedValue;
                return str;
            }
        }
        public string MainEnPK
        {
            get
            {
                return ViewState["MainEnPK"] as string;
            }
            set
            {
                this.ViewState["MainEnPK"] = value;
            }
        }
        private Entity _MainEn = null;
        public Entity MainEn
        {
            get
            {
                if (_MainEn == null)
                    _MainEn = ClassFactory.GetEn(this.Request.QueryString["EnsName"]);
                return _MainEn;
            }
            set
            {
                _MainEn = value;
            }
        }

        public int ErrMyNum = 0;
        public LinkBtn Btn_Save
        {
            get
            {
                return this.ToolBar1.GetLinkBtnByID("Btn_Save");
            }
        }
        public LinkBtn Btn_SaveAndClose
        {
            get
            {
                return this.ToolBar1.GetLinkBtnByID("Btn_SaveAndClose");
            }
        }
        #endregion  Property .

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region  Treatment may result from   Parent entity   Business logic .
                Entity enP = ClassFactory.GetEn(this.EnName);
                this.Page.Title = enP.EnDesc;
                this.MainEnName = enP.EnDesc;
                this.MainEnPKVal = this.PK;
                this.MainEnPK = enP.PK;
                if (enP.EnMap.EnType != EnType.View)
                {
                    try
                    {
                        enP.SetValByKey(enP.PK, this.PK);// =this.PK;
                        enP.Retrieve(); // Inquiry .
                        enP.Update(); //  Perform an update , Write deal   Parent entity   Business logic .
                    }
                    catch
                    {
                    }
                }
                MainEn = enP;
                #endregion
            }
            catch (Exception ex)
            {
                this.ToErrorPage(ex.Message);
                return;
            }

            AttrOfOneVSM ensattr = this.AttrOfOneVSM;
            this.ToolBar1.AddLab("lab_desc", " Packet :");
            DropDownList ddl = new DropDownList();
            ddl.ID = "DDL_Group";
            ddl.AutoPostBack = true;
            this.ToolBar1.Add(ddl);
            ddl.Items.Clear();
            ddl.SelectedIndexChanged += new EventHandler(DDL_Group_SelectedIndexChanged);
            Entity open = ensattr.EnsOfM.GetNewEntity;
            Map map = open.EnMap;
            int len = 19;

            //  If the longest   Title   〉 15  Length . On one line display .
            if (len > 20)
                this.IsLine = true;
            else
                this.IsLine = false;

            //  To join enum  Type .
            foreach (Attr attr in map.Attrs)
            {
                /* map */
                if (attr.IsFKorEnum == false)
                    continue;
                this.DDL_Group.Items.Add(new ListItem(attr.Desc, attr.Key));
            }

            this.DDL_Group.Items.Add(new ListItem("无", "None"));
            foreach (ListItem li in ddl.Items)
            {
                if (li.Value == this.ShowWay)
                    li.Selected = true;
            }
            this.ToolBar1.AddSpt("spt");

            CheckBox cb = new CheckBox();
            cb.ID = "checkedAll";
            cb.Attributes["onclick"] = "SelectAll(this);";
            cb.Text = " Select all " ;
            this.ToolBar1.Add(cb);
            cb.Dispose();
            this.DDL_Group.SelectedIndexChanged += new EventHandler(DDL_Group_SelectedIndexChanged);

            #region  Processing save permissions .
            UAC uac = ensattr.EnsOfMM.GetNewEntity.HisUAC;
            if (uac.IsInsert == true || uac.IsUpdate == true)
            {
                this.ToolBar1.AddSpace(4);
                this.ToolBar1.AddLinkBtn("Btn_Save", " Save ");
                //this.ToolBar1.AddBtn("Btn_Save", " Save ");
                //this.Btn_Save.UseSubmitBehavior = false;
                //this.Btn_Save.OnClientClick = "this.disabled=true;";
                try
                {
                    this.ToolBar1.GetLinkBtnByID("Btn_Save").Click += new EventHandler(BPToolBar1_ButtonClick);
                    //this.ToolBar1.GetLinkBtnByID("Btn_SaveAndClose").Click += new EventHandler(BPToolBar1_ButtonClick);
                }
                catch
                {
                }
            }
            #endregion  Processing save permissions .

            this.SetDataV2();
        }
        #endregion Page_Load

        #region  Method 
        public void SetDataV2()
        {
            this.UCSys1.ClearViewState();
            AttrOfOneVSM attrOM = this.AttrOfOneVSM;
            Entities ensOfM = attrOM.EnsOfM;
            //if (ensOfM.Count == 0)
                ensOfM.RetrieveAll();

            try
            {
                Entities ensOfMM = attrOM.EnsOfMM;
                QueryObject qo = new QueryObject(ensOfMM);
                qo.AddWhere(attrOM.AttrOfOneInMM, this.PK);
                qo.DoQuery();

                if (this.DDL_Group.SelectedValue == "None")
                {
                    if (this.IsLine)
                        this.UCSys1.UIEn1ToM_OneLine(ensOfM, attrOM.AttrOfMValue, attrOM.AttrOfMText, ensOfMM, attrOM.AttrOfMInMM);
                    else
                        this.UCSys1.UIEn1ToM(ensOfM, attrOM.AttrOfMValue, attrOM.AttrOfMText, ensOfMM, attrOM.AttrOfMInMM);
                }
                else
                {
                    if (this.IsLine)
                        this.UCSys1.UIEn1ToMGroupKey_Line(ensOfM, attrOM.AttrOfMValue, attrOM.AttrOfMText, ensOfMM, attrOM.AttrOfMInMM, this.DDL_Group.SelectedValue);
                    else
                        this.UCSys1.UIEn1ToMGroupKey(ensOfM, attrOM.AttrOfMValue, attrOM.AttrOfMText, ensOfMM, attrOM.AttrOfMInMM, this.DDL_Group.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    ensOfM.GetNewEntity.CheckPhysicsTable();
                }
                catch (Exception ex1)
                {
                    BP.DA.Log.DefaultLogWriteLineError(ex1.Message);
                }

                this.UCSys1.ClearViewState();
                ErrMyNum++;
                if (ErrMyNum > 3)
                {
                    this.UCSys1.AddMsgOfWarning("error", ex.Message);
                    return;
                }
                this.SetDataV2();
            }
        }
        private void BPToolBar1_ButtonClick(object sender, System.EventArgs e)
        {
            var btn = (LinkBtn) sender;
            switch (btn.ID)
            {
                case NamesOfBtn.SelectNone:
                    //this.CBL1.SelectNone();
                    break;
                case NamesOfBtn.SelectAll:
                    //this.CBL1.SelectAll();
                    break;
                case NamesOfBtn.Save:
                    if (this.IsTreeShowWay)
                        SaveTree();
                    else
                        Save();

                    string str = this.Request.RawUrl;
                    if (str.Contains("ShowWay="))
                        str = str.Replace("&ShowWay=", "&1=");
                    this.Response.Redirect(str + "&ShowWay=" + this.DDL_Group.SelectedItem.Value, true);
                    return;
                case "Btn_SaveAndClose":
                    if (this.IsTreeShowWay)
                        SaveTree();
                    else
                        Save();
                    this.WinClose();
                    break;
                case "Btn_Close":
                    this.WinClose();
                    break;
                case "Btn_EditMEns":
                    this.EditMEns();
                    break;
                default:
                    throw new Exception("@ Not found " + btn.ID);
            }
        }
        #endregion  Method 

        #region  Operating 
        public void EditMEns()
        {
            this.WinOpen(this.Request.ApplicationPath + "/Comm/UIEns.aspx?EnsName=" + this.AttrOfOneVSM.EnsOfM.ToString());
        }
        public void Save()
        {
            AttrOfOneVSM attr = this.AttrOfOneVSM;
            Entities ensOfMM = attr.EnsOfMM;
            ensOfMM.Delete(attr.AttrOfOneInMM, this.PK);

            // The save .
            foreach (System.Web.UI.Control ctl in this.UCSys1.Controls)
            {
                if (ctl == null || ctl.ID == null)
                    continue;

                if (ctl.ID.Contains("CB_") == false)
                    continue;

                CheckBox cb = (CheckBox)ctl;  
                if (cb == null)
                    continue;

                if (cb.Checked == false)
                    continue;

                Entity en1 = ensOfMM.GetNewEntity;
                en1.SetValByKey(attr.AttrOfOneInMM, this.PK);
                en1.SetValByKey(attr.AttrOfMInMM, ctl.ID.Replace("CB_",""));
                en1.Insert();
            }

            // Update entity , There appear to prevent the business logic .
            string msg = "";
            Entity enP = ClassFactory.GetEn(this.EnName);
            if (enP.EnMap.EnType != EnType.View)
            {
                enP.SetValByKey(enP.PK, this.PK);// =this.PK;
                enP.Retrieve(); // Inquiry .
                try
                {
                    enP.Update(); //  Perform an update , Write deal   Parent entity   Business logic .
                }
                catch (Exception ex)
                {
                    msg += " Perform the update error :" + enP.EnDesc + " " + ex.Message;
                }
            }
        }
        public void SaveTree()
        {
            AttrOfOneVSM attr = this.AttrOfOneVSM;
            Entities ensOfMM = attr.EnsOfMM;
            ensOfMM.Delete(attr.AttrOfOneInMM, this.PK); // Delete the saved data .

            AttrOfOneVSM attrOM = this.AttrOfOneVSM;
            Entities ensOfM = attrOM.EnsOfM;
            ensOfM.RetrieveAll();

            Entity enP = ClassFactory.GetEn(this.Request.QueryString["EnsName"]);
            if (enP.EnMap.EnType != EnType.View)
            {
                enP.SetValByKey(enP.PK, this.PK);// =this.PK;
                enP.Retrieve(); // Inquiry .
                enP.Update(); //  Perform an update , Write deal   Parent entity   Business logic .
            }
        }
        #endregion

        #region Web  Form Designer generated code 
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN:  This call is  ASP.NET Web  Form Designer required .
            //
            InitializeComponent();
            base.OnInit(e);
        }
        /// <summary>
        ///  Required method for Designer support  -  Do not use the code editor to modify 
        ///  Contents of this method .
        /// </summary>
        private void InitializeComponent()
        {

        }
        //public string RowUrl
        //{
        //    get
        //    {
        //        //string str = "EnsName="+this.EnsName+"&EnName="+this.EnName+"&AttrKey="+this.AttrKey+"&NodeID=101";
        //        return str;
        //    }
        //}
        private void DDL_Group_SelectedIndexChanged(object sender, EventArgs e)
        {

            string str = this.Request.RawUrl;
            if (str.Contains("ShowWay="))
                str = str.Replace("&ShowWay=", "&1=");
            this.Response.Redirect(str + "&ShowWay=" + this.DDL_Group.SelectedItem.Value, true);
            return;

            //// this.SetDataV2();
            //CheckBox mycb = this.ToolBar1.GetCBByID("RB_Tree");
            //if (mycb == null)
            //    this.SetDataV2();
            ////else
            ////    this.BindTree();
        }
        #endregion
    }


}