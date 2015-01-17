using System;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
using BP.En;
using Microsoft.Web.UI.WebControls;
using BP.Web;
using BP.Port;
using BP.DA;
using BP.Sys;
using System.Web.UI;

namespace BP.Web.Controls
{
    /// <summary>
    /// ToolbarCheckGroup
    /// </summary>
    public class ToolbarCG : Microsoft.Web.UI.WebControls.ToolbarCheckGroup
    {
        public ToolbarCG()
        {

        }
    }
    /// <summary>
    /// ToolbarCheckGroup
    /// </summary>
    public class ToolbarCB : Microsoft.Web.UI.WebControls.ToolbarCheckButton
    {
        public ToolbarCB()
        {
        }
    }
    /// <summary>
    /// ToolbarCheckBtn
    /// </summary>
    public class ToolbarCheckBtn : Microsoft.Web.UI.WebControls.ToolbarCheckButton
    {
        /// <summary>
        /// SetImageUrl
        /// </summary>
        private void SetImageUrl()
        {
            if (this.ID.IndexOf("Btn_") == -1)
                throw new Exception("@ Does not meet the system naming rules " + this.ID);
            this.ImageUrl = System.Web.HttpContext.Current.Request.ApplicationPath + "WF/Img/Btn/" + this.ID.Substring(4) + ".gif";

            if (this.ID == "Btn_Delete")
            {
                //				if (this.Hit==null)
                //					this.Attributes["onclick"] += " return confirm(' To perform this operation delete , Whether to continue ?');";
                //				else
                //					this.Attributes["onclick"] += " return confirm(' To perform this operation delete 　["+this.Hit+"], Whether to continue ?');";
            }
        }
        /// <summary>
        /// ToolbarCheckBtn
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        public ToolbarCheckBtn(string id, string text)
        {
            this.ID = id;
            this.Text = text;
            this.SetImageUrl();
        }
        /// <summary>
        /// ToolbarCheckBtn
        /// </summary>
        public ToolbarCheckBtn()
        {
        }
    }
    /// <summary>
    /// ToolbarBtn
    /// </summary>
    public class ToolbarBtn : Microsoft.Web.UI.WebControls.ToolbarButton
    {
        /// <summary>
        ///  Serial number 
        /// </summary>
        public string SelfNo
        {
            get
            {
                return (string)ViewState["SelfNo"];
            }
            set
            {
                ViewState["SelfNo"] = value;
            }
        }
        /// <summary>
        /// ToolbarBtn
        /// </summary>
        public ToolbarBtn()
        {
        }
        /// <summary>
        /// ToolbarBtn
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="text">text</param> 
        public ToolbarBtn(string id, string text)
        {
            this.ID = id;
            this.Text = text;
            this.SetImageUrl();
        }
        /// <summary>
        /// ToolbarBtn
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="text">text</param>
        /// <param name="selfNo">selfno</param>
        public ToolbarBtn(string id, string text, string selfNo)
        {
            this.ID = id;
            this.Text = text;
            this.SelfNo = selfNo;
            this.SetImageUrl();
        }
        /// <summary>
        /// set image of url
        /// </summary>
        private void SetImageUrl()
        {
            if (this.ID.IndexOf("Btn_") == -1)
                throw new Exception("@ Does not meet the system naming rules " + this.ID);
            this.ImageUrl = System.Web.HttpContext.Current.Request.ApplicationPath + "WF/Img/Btn/" + this.ID.Substring(4) + ".gif";

            if (this.ID == "Btn_Delete")
            {
                //				if (this.Hit==null)
                //					this.Attributes["onclick"] += " return confirm(' To perform this operation delete , Whether to continue ?');";
                //				else
                //					this.Attributes["onclick"] += " return confirm(' To perform this operation delete 　["+this.Hit+"], Whether to continue ?');";
            }
        }
        /// <summary>
        /// the hit
        /// </summary>
        public string Hit
        {
            get
            {
                return ViewState["Hit"].ToString();
            }
            set
            {
                ViewState["Hit"] = value;
            }
        }
    }
    /// <summary>
    /// from ToolbarLabel
    /// </summary>
    public class ToolbarLab : Microsoft.Web.UI.WebControls.ToolbarLabel
    {
        /// <summary>
        ///  Serial number 
        /// </summary>
        public string SelfNo
        {
            get
            {
                return (string)ViewState["SelfNo"];
            }
            set
            {
                ViewState["SelfNo"] = value;
            }
        }
        /// <summary>
        /// from ToolbarLabel
        /// </summary>
        public ToolbarLab()
        {
        }
        /// <summary>
        /// from ToolbarLabel
        /// </summary>
        /// <param name="id">id</param>
        public ToolbarLab(string id)
        {
            this.ID = id;
        }
        /// <summary>
        /// from ToolbarLabel
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="text">text</param>
        /// <param name="selfNo">selfno</param>
        public ToolbarLab(string id, string text, string selfNo)
        {
            this.Text = text;
            this.ID = id;
            this.SelfNo = selfNo;
        }
        /// <summary>
        /// set image of url
        /// </summary>
        public void SetImageUrl()
        {

            if (this.ID.IndexOf("Lab_") == -1)
                throw new Exception("@ Does not meet the system naming rules " + this.ID);
            this.ImageUrl = System.Web.HttpContext.Current.Request.ApplicationPath + "images/Lab/" + this.ID.Substring(4) + ".gif";
        }
    }
    /// <summary>
    /// from ToolbarSeparator
    /// </summary>
    public class ToolbarSpt : Microsoft.Web.UI.WebControls.ToolbarSeparator
    {
        /// <summary>
        ///  Serial number 
        /// </summary>
        public string SelfNo
        {
            get
            {
                return (string)ViewState["SelfNo"];
            }
            set
            {
                ViewState["SelfNo"] = value;
            }
        }
        /// <summary>
        /// from ToolbarSeparator
        /// </summary>
        public ToolbarSpt()
        {
        }
        /// <summary>
        /// from ToolbarSeparator
        /// </summary>
        /// <param name="id">id</param> 
        public ToolbarSpt(string id)
        {
            this.ID = id;
        }
        /// <summary>
        /// from ToolbarSpt
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="selfNo">selfno</param>
        public ToolbarSpt(string id, string selfNo)
        {
            this.ID = id;
            this.SelfNo = selfNo;
        }
        public ToolbarSpt(string id, string selfNo, bool isBr)
        {
            this.ID = id;
            this.SelfNo = selfNo;
            //this.Orientation=Orientation.Vertical;
        }
    }
    /// <summary>
    /// ToolbarCheckBtnCollection
    /// </summary>
    public class ToolbarCheckBtnCollection : Microsoft.Web.UI.WebControls.ToolbarCheckButtonCollection
    {
        /// <summary>
        /// ToolbarCheckBtnCollection
        /// </summary>
        public ToolbarCheckBtnCollection()
        {
        }
        /// <summary>
        /// ToolbarCheckBtnCollection
        /// </summary>
        /// <param name="id"></param>
        public ToolbarCheckBtnCollection(string id)
        {

        }
        /// <summary>
        ///  Increase 
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="text"> Text </param>
        public void Add(string id, string text)
        {
            this.Add(new ToolbarCheckBtn(id, text));
        }
    }
    /// <summary>
    /// ToolbarCheckBtnCollection
    /// </summary>
    public class ToolbarCheckBtnGroup : Microsoft.Web.UI.WebControls.ToolbarCheckGroup
    {
        /// <summary>
        /// ToolbarCheckBtnCollection
        /// </summary>
        public ToolbarCheckBtnGroup()
        {
        }
        /// <summary>
        /// ToolbarCheckGroup
        /// </summary>
        /// <param name="id"></param>
        public ToolbarCheckBtnGroup(string id)
        {
            this.ID = id;
        }
        /// <summary>
        ///  Increase 
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="text"> Text </param>
        public void Add(string id, string text)
        {
            this.Items.Add(new ToolbarCheckBtn(id, text));
        }
        public void Add(string id, string text, string img)
        {
            ToolbarCheckBtn tcb = new ToolbarCheckBtn(id, text);
            tcb.ImageUrl = img;
            this.Items.Add(tcb);
        }
    }
    /// <summary>
    /// BPListBox  The summary .
    /// </summary>
    public class BPToolBar : Microsoft.Web.UI.WebControls.Toolbar
    {
        #region init
        //public string jsOfOnselectMore = "alert('hello')";
        //protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        //{
        //    if (jsOfOnselectMore == null)
        //    {
        //        base.AddAttributesToRender(writer);
        //        return;
        //    }

        //  //  this.

        //    // Show the CompareValidator's error message as bold.   
        //    //writer.AddStyleAttribute(System.Web.UI.HtmlTextWriterStyle.FontWeight, "bold");

        //  //  writer.AddAttribute(HtmlTextWriterAttribute.Onclick, jsOfOnselectMore);

        //    //  writer.AddAttribute(HtmlTextWriterAttribute.Selected, jsOfOnselectMore);
        //    writer.AddAttribute("OnCheckChange", jsOfOnselectMore);


        // //   writer.AddAttribute("Selected", jsOfOnselectMore);
        ////    writer.AddAttribute(HtmlTextWriterAttribute.Selected, jsOfOnselectMore);

        //   // writer.AddAttribute("Onclick", jsOfOnselectMore);
        //    //writer.AddAttribute("Onchange", jsOfOnselectMore);



        //    // string js = "alert('hello ')";
        //    //writer.AddAttribute("onmouseover", js);
        //    // Call the Base's AddAttributesToRender method.   
        //    base.AddAttributesToRender(writer);
        //}


        public void InitFuncEn(UAC uac, Entity en)
        {
            if (en.EnMap.EnType == EnType.View)
                uac.Readonly();

            //this.AddLab("Lab_Key1"," Title :"+en.EnDesc);
            if (uac.IsInsert)
            {
                if (en.EnMap.EnType != EnType.Dtl)
                {
                    this.AddBtn(NamesOfBtn.New);
                   // this.AddBtn(NamesOfBtn.Copy);
                }
            }

            if (uac.IsUpdate)
            {
                this.AddBtn(NamesOfBtn.Save);
                // this.AddBtn(NamesOfBtn.SaveAndClose);
            }

            if (uac.IsInsert && uac.IsUpdate)
            {
                if (en.EnMap.EnType != EnType.Dtl)
                {
                    //this.AddBtn(NamesOfBtn.SaveAndNew );
                    //this.AddBtn(NamesOfBtn.SaveAndClose );
                }
            }

            //if (uac.IsDelete && en.PKVal.ToString().Length >= 1)
            //{
            //    this.AddBtn(NamesOfBtn.Delete);
            //}

            if (uac.IsAdjunct)
            {
                this.AddBtn(NamesOfBtn.Adjunct);
                if (en.IsEmpty == false)
                {
                    int i = DBAccess.RunSQLReturnValInt("select COUNT(*) from Sys_FileManager WHERE RefTable='" + en.ToString() + "' AND RefKey='" + en.PKVal + "'");
                    if (i != 0)
                    {
                        this.GetBtnByKey(NamesOfBtn.Adjunct).Text += "-" + i;
                    }
                }
            }

            //if (this.Controls.Count != 0)
            //    this.AddSpt("spt");

            
        }
        /// <summary>
        ///  Initialization function 
        /// </summary>
        public void InitFunc(UAC uac, Entity en, Entities ens, bool IsDataHelp)
        {
            if (uac.IsInsert)
                this.AddBtn(NamesOfBtn.New);

            if (uac.IsUpdate)
                this.AddBtn(NamesOfBtn.Save);

            //if (uac.IsInsert && uac.IsUpdate)
            //    this.AddBtn(NamesOfBtn.SaveAndNew);

            if (uac.IsDelete)
                this.AddBtn(NamesOfBtn.Delete);

            if (uac.IsAdjunct)
            {
                this.AddBtn(NamesOfBtn.Adjunct);
            }

            if (IsDataHelp)
                this.AddBtn(NamesOfBtn.Confirm);

            if (uac.IsInsert || uac.IsUpdate || uac.IsDelete)
            {
                //this.AddBtn(NamesOfBtn.ChoseField);
            }
            else
            {
                if (en.EnMap.Attrs.Count > 12)
                {
                    this.AddBtn(NamesOfBtn.ChoseField);
                }
            }

            foreach (Attr attr in en.EnMap.Attrs)
            {
                if (attr.MyDataType == DataType.AppString)
                    continue;

                if (attr.MyDataType == DataType.AppDate)
                    continue;

                if (attr.MyDataType == DataType.AppDateTime)
                    continue;

                if (attr.MyDataType == DataType.AppBoolean)
                    continue;

                if (attr.IsPK)
                    continue;
                if (attr.UIContralType == UIContralType.DDL)
                    continue;

                if (attr.UIVisible == false)
                    continue;


                if (attr.MyDataType == DataType.AppFloat
                    || attr.MyDataType == DataType.AppInt
                    || attr.MyDataType == DataType.AppMoney
                    || attr.MyDataType == DataType.AppDouble
                    || attr.MyDataType == DataType.AppRate)
                {
                    this.AddBtn(NamesOfBtn.DataGroup);
                    break;
                }
            }
            //this.AddBtn(NamesOfBtn.DataGroup);
            //if (IsDataHelp==false)
            if (1 == 2)
            {
                #region  He added detail 
                //				EnDtls enDtls= en.EnMap.Dtls;
                //				if ( enDtls.Count > 0 && IsDataHelp==false)
                //				{
                //					this.AddSpt(new ToolbarSpt("ref_s1s","s2s") );						
                //					foreach(EnDtl enDtl in enDtls)
                //					{
                //						ToolbarBtn btn = new ToolbarBtn("Btn_Ref"+enDtl.EnsName,enDtl.Desc,enDtl.EnsName );
                //						btn.ImageUrl=this.Page.Request.ApplicationPath+enDtl.Ens.GetNewEntity.EnMap.Icon; 
                //						this.AddBtn(btn);
                //					}
                //				}
                #endregion

                #region  Join many of the entity editor 
                //				AttrsOfOneVSM oneVsM= en.EnMap.AttrsOfOneVSM;
                //				if ( oneVsM.Count > 0 && IsDataHelp==false)
                //				{
                //					this.AddSpt("oneVsM");
                //					foreach(AttrOfOneVSM vsM in oneVsM)
                //					{
                //						ToolbarBtn btn = new ToolbarBtn();
                //						btn.ID="Btn_OneVsM"+vsM.EnsOfMM;
                //						btn.Text=vsM.Desc;
                //						btn.SelfNo=vsM.EnsOfMM.ToString();
                //						btn.ImageUrl=this.Page.Request.ApplicationPath+vsM.EnsOfMM.GetNewEntity.EnMap.Icon ; 
                //						this.AddBtn(btn);
                //					}
                //				}
                #endregion

                #region  Joined his door-related functions ( Has put the right , Remove .)
                /*
				SysUIEnsRefFuncs reffuncs = ens.HisSysUIEnsRefFuncs;
				if ( reffuncs.Count > 0  )
				{
					string webpath= this.Page.Request.ApplicationPath ; 
					AddSpt(new ToolbarSpt("ref_ss","ss") );
					//this.BPToolBar1.AddLab("Lab_RefFuncs"," Related functions ");
					foreach( BP.Sys.SysUIEnsRefFunc en1 in reffuncs)
					{
						ToolbarBtn btn = new ToolbarBtn("Btn_"+en1.OID.ToString(),en1.Name,en1.OID.ToString()) ;
						btn.ImageUrl=webpath+"/"+en1.Icon;
						this.AddBtn(btn);
					}
				}
				 
						 
				#endregion

				#region  Joined his related information .
				/*  In order to improve the efficiency of removing the .2004-10-28 
						Attrs refAttrs= this.HisEn.EnMap.HisRefAttrs;
						if ( refAttrs.Count > 0)
						{					
							this.BPToolBar1.AddSpt(new ToolbarSpt("ref_s1s","s2s") );						
							foreach(Attr attr in refAttrs)
							{
								if (attr.MyFieldType==FieldType.RefText)
									continue;
								if (attr.UIVisible==false)
									continue;

								Entity en= ClassFactory.GetEns(attr.UIBindKey).GetNewEntity;
								ToolbarBtn btn = new ToolbarBtn("Btn_Ref"+attr.Key,en.EnDesc,attr.Key);
								btn.ImageUrl=this.Request.ApplicationPath+en.EnMap.DefaultImageUrl;
								this.BPToolBar1.AddBtn(btn);
							}
						}			
						*/
                #endregion

            }

            this.AddSpt(new ToolbarSpt("ref_Help", "s2s1"));

            if (this.Items.Count > 1)
            {
                //this.AddBtn(NamesOfBtn.Help);
            }
            else
                this.Visible = false;
        }
        #endregion

        #region  With respect to  AddTCG  Operating 
        /// <summary>
        ///  Adding a Btn
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="text1"></param>
        /// <param name="selected1"></param>
        public void AddCB(string id1, string text1, bool selected1, bool aotuPostBack)
        {
            ToolbarCB cb = new ToolbarCB();
            cb.ID = id1;
            cb.Text = text1;
            cb.Selected = selected1;
            cb.AutoPostBack = aotuPostBack;
            cb.ImageUrl = System.Web.HttpContext.Current.Request.ApplicationPath + "Images/CB/" + id1.Substring(3) + ".gif";
            this.Items.Add(cb);
        }
        public ToolbarCB GetToolbarCBByKey(string key)
        {
            return (ToolbarCB)this.GetContralByKey(key);

        }
        #endregion

        #region  With respect to AddTCG  Operating 
        /// <summary>
        ///  Adding a Btn
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="text1"></param>
        /// <param name="selected1"></param>
        public void AddTCG(string id1, string text1, bool selected1)
        {
            ToolbarCG cg = new ToolbarCG();
            cg.ID = "TCG1";

            ToolbarCheckBtn btn1 = new ToolbarCheckBtn();
            btn1.ID = id1;
            btn1.Text = text1;
            btn1.Selected = selected1;

            cg.Items.Add(btn1);

            this.Items.Add(cg);
        }
        #endregion


        #region ToolbarCheckBtn

        /// <summary>
        ///  Adding a ToolbarCheckBtnGroup
        /// </summary>
        /// <param name="id"></param>
        public void AddToolbarCheckBtnGroup(string id)
        {
            this.Items.Add(new ToolbarCheckBtnGroup(id));
        }
        /// <summary>
        /// AddCheckBtn
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        public void AddCheckBtn(string id, string text)
        {
            ToolbarCheckBtn en = new ToolbarCheckBtn(id, text);
            this.Items.Add(en);
        }


        #endregion


        #region  Operating entity queries regarding details .
        public static DataTable GenerSearchTable(BPToolBar contral, Entities ens)
        {
            Map map = ens.GetNewEntity.EnMap;
            string keyVal = "%" + contral.GetTBByID("TB_Key").Text.Trim() + "%";
            QueryObject qo = new QueryObject(ens);
            //  Plus key operation .
            if (keyVal != "%%")
            {
                Attrs attrs = map.Attrs;
                qo.addLeftBracket();
                foreach (Attr attr in attrs)
                {
                    if (attr.MyDataType != DataType.AppString)
                        continue;

                    if (attr.Key == "OID" || attr.Key.ToUpper() == "WORKID" || attr.Key.ToUpper() == "NODEID")
                        continue;

                    if (attr.MyFieldType == FieldType.RefText)
                        continue;

                    if (attr.MyFieldType == FieldType.RefText)
                        continue;
                    if (attr.UIContralType == UIContralType.DDL || attr.UIContralType == UIContralType.CheckBok)
                        continue;
                    qo.addOr();
                    qo.AddWhere(attr.Key, " LIKE ", keyVal);
                }
                qo.addRightBracket();
            }

            //  User-specified attributes 
            if (map.AttrsOfSearch.Count >= 1)
            {
                qo.addLeftBracket();
                foreach (AttrOfSearch attr in map.AttrsOfSearch)
                {

                    qo.addAnd();
                    qo.AddWhere(attr.RefAttrKey, contral.GetDDLByKey("DDL_" + attr.Key).SelectedItemStringVal, contral.GetTBByID("TB_" + attr.Key).Text);
                }
                qo.addRightBracket();
            }
            //  Foreign key 
            //Attrs searchAttrs = map.SearchAttrs;
            //foreach (Attr attr in searchAttrs)
            //{
            //    if (attr.MyFieldType == FieldType.RefText)
            //        continue;
            //    qo.addAnd();
            //    qo.addLeftBracket();
            //    qo.AddWhere(attr.Key, contral.GetDDLByKey("DDL_" + attr.Key).SelectedItemStringVal);
            //    qo.addRightBracket();
            //}

            return qo.DoQueryToTable();

            //return ens.ToDataTable();
        }
        #endregion


        /// <summary>
        ///  Determine whether there is a control .
        /// </summary>
        /// <param name="id"> To determine the id</param>
        /// <returns> Whether there </returns>
        public bool IsExitsContral(string id)
        {
            foreach (Microsoft.Web.UI.WebControls.ToolbarItem en in this.Items)
            {
                if (en.ID == id)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// ToolbarItem
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ToolbarItem GetContralByKey(string id)
        {
            foreach (Microsoft.Web.UI.WebControls.ToolbarItem en in this.Items)
            {
                if (en.ID == id)
                    return en;
            }
            throw new Exception("@ Not found ID=" + id + "  Controls ");
        }
        public ToolbarBtn GetBtnByKey(string id)
        {
            return (ToolbarBtn)this.GetContralByKey(id);
        }
        public ToolbarCheckBtn GetCheckBtnByKey(string id)
        {
            return (ToolbarCheckBtn)this.GetContralByKey(id);
        }
        public ToolbarTB GetTBByID(string id)
        {
            return (ToolbarTB)this.GetContralByKey(id);
        }
        public ToolbarDDL GetDDLByKey(string id)
        {
            return (ToolbarDDL)this.GetContralByKey(id);
        }
        public ToolbarLab GetLabByKey(string id)
        {
            return (ToolbarLab)this.GetContralByKey(id);
        }
        /// <summary>
        /// ToolbarCheckBtn
        /// </summary>
        /// <param name="key"></param>
        public ToolbarCheckBtnGroup GetToolbarCheckBtnGroupByKey(string id)
        {
            return (ToolbarCheckBtnGroup)this.GetContralByKey(id);
        }

        #region  Constructor 
        public string InitTableSqlByEns(Entities ens, AttrsOfSearch attrsOfSearch, Attrs searchAttrs)
        {
            Entity en = ens.GetNewEntity;
            QueryObject qo = new QueryObject(ens);
            qo.addLeftBracket();
            qo.AddWhere("abc", "all");
            qo.addRightBracket();

            #region  Common attributes 
            string opkey = ""; //  Operation Symbol .
            foreach (AttrOfSearch attr in attrsOfSearch)
            {
                if (attr.SymbolEnable == true)
                {
                    opkey = this.GetDDLByKey("DDL_" + attr.Key).SelectedItemStringVal;
                    if (opkey == "all")
                        continue;
                }
                else
                {
                    opkey = attr.DefaultSymbol;
                }

                qo.addAnd();
                qo.addLeftBracket();
                if (attr.IsHidden)
                    qo.AddWhere(attr.RefAttrKey, attr.DefaultSymbol, attr.DefaultVal);
                else
                {
                    if (attr.DefaultVal.Length >= 8)
                    {
                        string date = "";
                        try
                        {
                            /*  It could be date . */
                            string y = this.GetDDLByKey("DDL_" + attr.Key + "_Year").SelectedItemStringVal;
                            string m = this.GetDDLByKey("DDL_" + attr.Key + "_Month").SelectedItemStringVal;
                            string d = this.GetDDLByKey("DDL_" + attr.Key + "_Day").SelectedItemStringVal;
                            date = y + "-" + m + "-" + d;
                        }
                        catch
                        {
                        }
                        qo.AddWhere(attr.RefAttrKey, opkey, date);
                    }
                    else
                    {
                        qo.AddWhere(attr.RefAttrKey, opkey, this.GetTBByID("TB_" + attr.Key).Text);
                    }
                }
                qo.addRightBracket();
            }
            #endregion

            #region  Foreign key 
            foreach (Attr attr in searchAttrs)
            {
                if (attr.MyFieldType == FieldType.RefText)
                    continue;
                qo.addAnd();
                qo.addLeftBracket();

                if (attr.UIBindKey == "BP.Port.Depts" || attr.UIBindKey == "BP.Port.Units" )  // Judgment special circumstances .
                    qo.AddWhere(attr.Key, " LIKE ", this.GetDDLByKey("DDL_" + attr.Key).SelectedItemStringVal + "%");
                else
                    qo.AddWhere(attr.Key, this.GetDDLByKey("DDL_" + attr.Key).SelectedItemStringVal);

                qo.addRightBracket();
            }
            #endregion.

            string sql = qo.SQL;

            return sql;

            //try
            //{
            //    sql = sql.Substring(sql.LastIndexOf("WHERE"));
            //}
            //catch
            //{ 
            //}

            //return sql;

        }
        public string InitTableSqlByEnsForOracle(Entities ens, AttrsOfSearch attrsOfSearch, Attrs searchAttrs)
        {
            Entity en = ens.GetNewEntity;
            QueryObject qo = new QueryObject(ens);

            qo.addLeftBracket();
            qo.AddWhere("abc", "all");
            qo.addRightBracket();

            #region  Common attributes 
            string opkey = ""; //  Operation Symbol .
            foreach (AttrOfSearch attr in attrsOfSearch)
            {
                if (attr.SymbolEnable == true)
                {
                    opkey = this.GetDDLByKey("DDL_" + attr.Key).SelectedItemStringVal;
                    if (opkey == "all")
                        continue;
                }
                else
                {
                    opkey = attr.DefaultSymbol;
                }

                qo.addAnd();
                qo.addLeftBracket();
                if (attr.IsHidden)
                    qo.AddWhere(attr.RefAttrKey, attr.DefaultSymbol, attr.DefaultVal);
                else
                {
                    if (attr.DefaultVal.Length >= 8)
                    {
                        string date = "";
                        try
                        {
                            /*  It could be date . */
                            string y = this.GetDDLByKey("DDL_" + attr.Key + "_Year").SelectedItemStringVal;
                            string m = this.GetDDLByKey("DDL_" + attr.Key + "_Month").SelectedItemStringVal;
                            string d = this.GetDDLByKey("DDL_" + attr.Key + "_Day").SelectedItemStringVal;
                            date = y + "-" + m + "-" + d;
                        }
                        catch
                        {

                        }
                        qo.AddWhere(attr.RefAttrKey, opkey, date);
                    }
                    else
                    {
                        qo.AddWhere(attr.RefAttrKey, opkey, this.GetTBByID("TB_" + attr.Key).Text);
                    }
                }
                qo.addRightBracket();
            }
            #endregion

            #region  Foreign key 
            foreach (Attr attr in searchAttrs)
            {
                if (attr.MyFieldType == FieldType.RefText)
                    continue;
                qo.addAnd();
                qo.addLeftBracket();

                if (attr.UIBindKey == "BP.Port.Depts" || attr.UIBindKey == "BP.Port.Units" )  // Judgment special circumstances .
                    qo.AddWhere(attr.Key, " LIKE ", this.GetDDLByKey("DDL_" + attr.Key).SelectedItemStringVal + "%");
                else
                    qo.AddWhere(attr.Key, this.GetDDLByKey("DDL_" + attr.Key).SelectedItemStringVal);

                qo.addRightBracket();
            }
            #endregion.

            if (en.EnMap.Attrs.Contains("No"))
            {
                qo.addOrderBy("No");
            }

            string sql = qo.SQL;

            try
            {
                sql = sql.Substring(sql.LastIndexOf("WHERE"));
            }
            catch
            {

            }

            return sql.Substring(5);

        }
        /// <summary>
        ///  Get a QueryObject
        /// </summary>
        /// <param name="ens"></param>
        /// <param name="en"></param>
        /// <returns></returns>
        public QueryObject InitQueryObjectByEns(Entities ens, bool IsShowSearchKey, Attrs attrs, AttrsOfSearch attrsOfSearch, Attrs searchAttrs)
        {
            QueryObject qo = new QueryObject(ens);

            #region  Keyword 
            string keyVal = "";
            //Attrs attrs = en.EnMap.Attrs;
            if (IsShowSearchKey)
            {
                keyVal = this.GetTBByID("TB_Key").Text.Trim();
                this.Page.Session["SKey"] = keyVal;
            }

            if (keyVal.Length >= 1)
            {
                //  this.Page.Session["KeyVal"+ens.GetNewEntity.ToString() ] = keyVal;

                Attr attrPK = new Attr();
                foreach (Attr attr in attrs)
                {
                    if (attr.IsPK)
                    {
                        attrPK = attr;
                        break;
                    }
                }

                int i = 0;
                foreach (Attr attr in attrs)
                {

                    switch (attr.MyFieldType)
                    {
                        case FieldType.Enum:
                        case FieldType.FK:
                        case FieldType.PKFK:
                            continue;
                        default:
                            break;
                    }


                    if (attr.MyDataType != DataType.AppString)
                        continue;

                    if (attr.MyFieldType == FieldType.RefText)
                        continue;

                    if (attr.Key == "FK_Dept")
                        continue;

                    i++;
                    if (i == 1)
                    {
                        /*  The first came in . */
                        qo.addLeftBracket();
                        if (SystemConfig.AppCenterDBVarStr == "@")
                            qo.AddWhere(attr.Key, " LIKE ", " '%'+" + SystemConfig.AppCenterDBVarStr + "SKey+'%'");
                        else
                            qo.AddWhere(attr.Key, " LIKE ", " '%'||" + SystemConfig.AppCenterDBVarStr + "SKey||'%'");
                        continue;
                    }
                    qo.addOr();

                    if (SystemConfig.AppCenterDBVarStr == "@")
                        qo.AddWhere(attr.Key, " LIKE ", "'%'+" + SystemConfig.AppCenterDBVarStr + "SKey+'%'");
                    else
                        qo.AddWhere(attr.Key, " LIKE ", "'%'||" + SystemConfig.AppCenterDBVarStr + "SKey||'%'");

                }
                qo.MyParas.Add("SKey", keyVal);
                qo.addRightBracket();
            }
            else
            {
                qo.addLeftBracket();
                qo.AddWhere("abc", "all");
                qo.addRightBracket();
            }
            #endregion

            #region  Common attributes 
            string opkey = ""; //  Operation Symbol .
            foreach (AttrOfSearch attr in attrsOfSearch)
            {
                if (attr.IsHidden)
                {
                    qo.addAnd();
                    qo.addLeftBracket();
                    qo.AddWhere(attr.RefAttrKey, attr.DefaultSymbol, attr.DefaultVal);
                    qo.addRightBracket();
                    continue;
                }

                if (attr.SymbolEnable == true)
                {
                    opkey = this.GetDDLByKey("DDL_" + attr.Key).SelectedItemStringVal;
                    if (opkey == "all")
                        continue;
                }
                else
                {
                    opkey = attr.DefaultSymbol;
                }

                qo.addAnd();
                qo.addLeftBracket();

                if (attr.DefaultVal.Length >= 8)
                {
                    string date = "2005-09-01";
                    try
                    {
                        /*  It could be date . */
                        string y = this.GetDDLByKey("DDL_" + attr.Key + "_Year").SelectedItemStringVal;
                        string m = this.GetDDLByKey("DDL_" + attr.Key + "_Month").SelectedItemStringVal;
                        string d = this.GetDDLByKey("DDL_" + attr.Key + "_Day").SelectedItemStringVal;
                        date = y + "-" + m + "-" + d;

                        if (opkey == "<=")
                        {
                            DateTime dt = DataType.ParseSysDate2DateTime(date).AddDays(1);
                            date = dt.ToString(DataType.SysDataFormat);
                        }
                    }
                    catch
                    {

                    }
                    qo.AddWhere(attr.RefAttrKey, opkey, date);
                }
                else
                {
                    qo.AddWhere(attr.RefAttrKey, opkey, this.GetTBByID("TB_" + attr.Key).Text);
                }
                qo.addRightBracket();
            }
            #endregion

            #region  Foreign key 
            foreach (Attr attr in searchAttrs)
            {
                if (attr.MyFieldType == FieldType.RefText)
                    continue;

                string selectVal = this.GetDDLByKey("DDL_" + attr.Key).SelectedItemStringVal;
                if (selectVal == "all")
                    continue;


                qo.addAnd();
                qo.addLeftBracket();

                if (attr.UIBindKey == "BP.Port.Depts" || attr.UIBindKey == "BP.Port.Units" )  // Judgment special circumstances .
                    qo.AddWhere(attr.Key, " LIKE ", selectVal + "%");
                else
                    qo.AddWhere(attr.Key, selectVal);

                //qo.AddWhere(attr.Key,this.GetDDLByKey("DDL_"+attr.Key).SelectedItemStringVal ) ;
                qo.addRightBracket();
            }
            #endregion.

            return qo;
        }
        /// <summary>
        ///  Initialization Table
        /// </summary>
        /// <param name="ens"></param>
        public DataTable InitTableByEns(Entities ens, Entity en)
        {
            return null;// this.InitQueryObjectByEns(ens, en.EnMap.IsShowSearchKey, en.EnMap.Attrs, en.EnMap.AttrsOfSearch, en.EnMap.SearchAttrs).DoQueryToTable();
        }
        /// <summary>
        ///  Temporary variables 
        /// </summary>
        public int recordConut = 0;


        public QueryObject InitTableByEnsV2(Entities ens, Entity en, int pageSize, int page)
        {
            switch (en.EnMap.EnDBUrl.DBType)
            {
                case DBType.Oracle:
                    return this.InitTableByEnsV2OfOrcale(ens, en, pageSize, page);
                default:
                    break;
            }

            QueryObject qo=new QueryObject();// this.InitQueryObjectByEns(ens, en.EnMap.IsShowSearchKey, en.EnMap.Attrs, en.EnMap.AttrsOfSearch, en.EnMap.SearchAttrs);
            this.recordConut = qo.GetCount();
            int pageCount = recordConut / pageSize;
            int myleftCount = recordConut - (pageCount * pageSize);
            pageCount++;
            int top = pageSize * (page - 1);

            try
            {
                DDL ddl = (DDL)this.Page.FindControl("DDL_Page");
                ddl.Items.Clear();

                for (int i = 1; i <= pageCount; i++)
                    ddl.Items.Add(new ListItem(i.ToString(), i.ToString()));

                if (pageCount == 0)
                    ddl.Items.Add(new ListItem("1", "1"));

                Label lab = (Label)this.Page.FindControl("Lab_End");
                lab.Text = "Total [" + ddl.Items.Count + "] pages,[" + recordConut + "] Records .";


                if (page == 1)
                {
                    qo.Top = pageSize;
                    return qo;
                }

                ddl.SetSelectItemByIndex(page - 1);
            }
            catch
            {
            }

            string pk = en.PK;
            if (en.EnMap.Attrs.Contains("No"))
            {
                qo.addOrderBy("No");
            }
            return qo;
        }

        public QueryObject InitTableByEnsV2_bak(Entities ens, Entity en, int pageSize, int page)
        {
            if (en.EnMap.EnDBUrl.DBType == DBType.Oracle)
                return this.InitTableByEnsV2OfOrcale(ens, en, pageSize, page);

            QueryObject qo = new QueryObject(); // this.InitQueryObjectByEns(ens, en.EnMap.IsShowSearchKey, en.EnMap.Attrs,
            // en.EnMap.AttrsOfSearch, en.EnMap.SearchAttrs);

            this.recordConut = qo.GetCount(); //  Get   Its number .
            int pageCount = recordConut / pageSize; //  Number of pages .
            int myleftCount = recordConut - (pageCount * pageSize); // 
            pageCount++;
            int top = pageSize * (page - 1);
            try
            {
                DDL ddl = (DDL)this.Page.FindControl("DDL_Page");
                ddl.Items.Clear();
                for (int i = 1; i <= pageCount; i++)
                    ddl.Items.Add(new ListItem(i.ToString(), i.ToString()));

                if (pageCount == 0)
                    ddl.Items.Add(new ListItem("1", "1"));

                Label lab = (Label)this.Page.FindControl("Lab_End");
                lab.Text = "Total [" + ddl.Items.Count + "] pages,[" + recordConut + "] Records .";

                if (page == 1 && en.EnMap.EnDBUrl.DBType == DBType.MSSQL)
                {
                    qo.Top = pageSize;
                    return qo;
                }
                ddl.SetSelectItemByIndex(page - 1);
            }
            catch
            {

            }

            string pk = en.PK;
            string wheresql = qo.SQL;
            string sql = "";
            switch (DBAccess.AppCenterDBType)
            {
                case DBType.Oracle:
                    //sql="SELECT "+en.PKField+" FROM "+en.EnMap.PhysicsTable+" "+wheresql.Substring(wheresql.IndexOf("WHERE (1=1)")) ;
                    //sql="SELECT "+en.EnMap.PhysicsTable+"."+en.PKField+"  "+wheresql.Substring(wheresql.IndexOf("FROM")) ;
                    break;
                default:
                    sql = "SELECT top " + top + " " + en.PKField + " FROM " + en.EnMap.PhysicsTable + " " + wheresql.Substring(wheresql.IndexOf("WHERE (1=1)"));
                    qo.addAnd();
                    qo.AddWhereNotInSQL(pk, sql);
                    qo.Top = pageSize;
                    break;
            }
            //Log.DefaultLogWriteLineInfo(qo.SQL);
            if (en.EnMap.Attrs.Contains("No"))
            {
                qo.addOrderBy("No");
            }
            return qo;
        }

        public QueryObject InitTableByEnsV2Oracle(Entities ens, Entity en, int pageSize, int page)
        {
            //  First, they are based on foreign key lookup .
            QueryObject qo = new QueryObject();// this.InitQueryObjectByEns(ens, en.EnMap.IsShowSearchKey, en.EnMap.Attrs, en.EnMap.AttrsOfSearch, en.EnMap.SearchAttrs);

            this.recordConut = qo.GetCount(); //  Calculated from the total number of .
            int pageCount = recordConut / pageSize; //  Calculated from the number of leaves .
            int myleftCount = recordConut - (pageCount * pageSize);
            pageCount++;
            int top = pageSize * (page - 1);

            int topnum = pageSize * page - pageSize;
            int downnum = pageSize * page;

            try
            {
                DDL ddl = (DDL)this.Page.FindControl("DDL_Page");
                ddl.Items.Clear();
                for (int i = 1; i <= pageCount; i++)
                    ddl.Items.Add(new ListItem(i.ToString(), i.ToString()));

                if (pageCount == 0)
                    ddl.Items.Add(new ListItem("1", "1"));

                Label lab = (Label)this.Page.FindControl("Lab_End");
                lab.Text = "Total [" + ddl.Items.Count + "] pages,[" + recordConut + "] Records .";

                if (page == 1)
                {
                    qo.addAnd();
                    qo.AddWhereField("RowNum", " < ", pageSize + 1);
                    return qo;
                }
                ddl.SetSelectItemByIndex(page - 1);
            }
            catch
            {
            }

            string pk = en.PK;
            string wheresql = qo.SQL;
            string sql = "SELECT " + en.EnMap.PhysicsTable + "." + pk + " " + wheresql.Substring(wheresql.IndexOf("FROM")) + " AND  ROWNUM < " + downnum.ToString();

            qo.addAnd();
            qo.AddWhereField("RowNum", " < ", topnum);
            qo.addAnd();
            qo.AddWhereInSQL(pk, sql);

            if (en.EnMap.Attrs.Contains("No"))
            {
                qo.addOrderBy("No");
            }
            return qo;
        }
        public QueryObject InitTableByEnsV2OfOrcale(Entities ens, Entity en, int pageSize, int page)
        {
            QueryObject qo = new QueryObject(); // this.InitQueryObjectByEns(ens, en.EnMap.IsShowSearchKey, en.EnMap.Attrs, en.EnMap.AttrsOfSearch, en.EnMap.SearchAttrs);
            this.recordConut = qo.GetCount();
            int pageCount = recordConut / pageSize;
            int myleftCount = recordConut - (pageCount * pageSize);
            pageCount++;
            int top = pageSize * (page - 1);

            try
            {
                DDL ddl = (DDL)this.Page.FindControl("DDL_Page");
                ddl.Items.Clear();

                for (int i = 1; i <= pageCount; i++)
                    ddl.Items.Add(new ListItem(i.ToString(), i.ToString()));

                if (pageCount == 0)
                    ddl.Items.Add(new ListItem("1", "1"));

                Label lab = (Label)this.Page.FindControl("Lab_End");
                lab.Text = "Total [" + ddl.Items.Count + "] pages,[" + recordConut + "] Records .";


                if (page == 1)
                {
                    qo.Top = pageSize;
                    return qo;
                }

                ddl.SetSelectItemByIndex(page - 1);
            }
            catch
            {
            }

            string pk = en.PK;

            if (en.EnMap.Attrs.Contains("No"))
            {
                qo.addOrderBy("No");
            }


            //			string wheresql=qo.SQL;
            //			string sql="";

            //int from=10;
            //,,int to=90;
            //			int index = wheresql.IndexOf( "WHERE (1=1)" ) ;
            //			if (index ==-1)
            //				index=0;

            //sql="SELECT "+en.PKField+" FROM "+en.EnMap.PhysicsTable+" "+wheresql.Substring(index)  ;
            //			qo.addAnd();
            //			qo.AddWhereNotInSQL(pk,sql);
            //qo.Top=pageSize;
            //Log.DefaultLogWriteLineInfo(qo.SQL);
            return qo;

        }
        public QueryObject GetnQueryObjectOracle(Entities ens, Entity en)
        {
            QueryObject qo = new QueryObject(); // this.InitQueryObjectByEns(ens, en.EnMap.IsShowSearchKey, en.EnMap.Attrs, en.EnMap.AttrsOfSearch, en.EnMap.SearchAttrs);
            string pk = en.PK;
            if (en.EnMap.Attrs.Contains("No"))
                qo.addOrderBy("No");

            return qo;
        }

        public QueryObject GetnQueryObject(Entities ens, Entity en)
        {
            if (en.EnMap.EnDBUrl.DBType == DBType.Oracle)
                return this.GetnQueryObjectOracle(ens, en);

            QueryObject qo = new QueryObject();// this.InitQueryObjectByEns(ens, en.EnMap.IsShowSearchKey, en.EnMap.Attrs,
           //     en.EnMap.AttrsOfSearch, en.EnMap.SearchAttrs);

            switch (en.PK)
            {
                case "No":
                    qo.addOrderBy("No");
                    break;
                case "OID":
                    qo.addOrderBy("OID");
                    break;
                default:
                    break;
            }
            return qo;
        }

        public QueryObject GetnQueryObjectBak20090810(Entities ens, Entity en)
        {
            if (en.EnMap.EnDBUrl.DBType == DBType.Oracle)
                return this.GetnQueryObjectOracle(ens, en);

            QueryObject qo = new QueryObject(); // this.InitQueryObjectByEns(ens, en.EnMap.IsShowSearchKey, en.EnMap.Attrs,
             //   en.EnMap.AttrsOfSearch, en.EnMap.SearchAttrs);

            switch (en.PK)
            {
                case "No":
                    qo.addOrderBy("No");
                    break;
                case "OID":
                    qo.addOrderBy("OID");
                    break;
                default:
                    break;
            }
            return qo;
        }

        #region  Set up 
        public void SaveSearchState(string className,string keyval)
        {
            UserRegedit ur = new UserRegedit();
            ur.SearchKey = keyval; // this.Page.Session["SKey"] as string;

            string str = "";
            foreach (Microsoft.Web.UI.WebControls.ToolbarItem ti in this.Items)
            {
                if (ti.ID == null)
                    continue;


                if (ti.ID.IndexOf("DDL_") == -1)
                    continue;

                ToolbarDDL ddl = (ToolbarDDL)ti;
                str += "@" + ti.ID + "=" + ddl.SelectedItemStringVal;
            }
            ur.FK_Emp = WebUser.No;
            ur.CfgKey = className + "_SearchAttrs";
            ur.Vals = str;
            ur.MyPK = WebUser.No + ur.CfgKey;
            ur.Save();
        }

        /// <summary>
        ///  Initialization map
        /// </summary>
        /// <param name="map">map</param>
        /// <param name="i"> Selected page </param>
        public void InitByMapV2(Map map, int page)
        {
            string str = this.Page.Request.QueryString["EnsName"];
            if (str == null)
                str = this.Page.Request.QueryString["EnsName"];
            if (str == null)
                return;

            UserRegedit ur = new UserRegedit(WebUser.No, str + "_SearchAttrs");
            string cfgKey = ur.Vals;

          //  InitByMapV2(map.IsShowSearchKey, map.AttrsOfSearch, map.SearchAttrs, null, page, ur);
            //  Set the query defaults , According to the previous configuration .

            if (cfgKey == "")
                return;

            string[] keys = cfgKey.Split('@');

            foreach (Microsoft.Web.UI.WebControls.ToolbarItem ti in this.Items)
            {
                if (ti.ID == null)
                    continue;

                if (ti.ID == "TB_Key")
                {
                    ToolbarTB tb = (ToolbarTB)ti;
                    tb.Text = ur.SearchKey;
                    continue;
                }

                if (ti.ID.IndexOf("DDL_") == -1)
                    continue;

                if (cfgKey.IndexOf(ti.ID) == -1)
                    continue;

                foreach (string key in keys)
                {
                    if (key.Length < 3)
                        continue;

                    if (key.IndexOf(ti.ID) == -1)
                        continue;

                    string[] vals = key.Split('=');

                    ToolbarDDL ddl = (ToolbarDDL)ti;
                    bool isHave = ddl.SetSelectItem(vals[1]);
                    if (isHave == false)
                    {
                        /* Did not have to find the person you want to select */
                        try
                        {
                            Attr attr = map.GetAttrByKey(vals[0].Replace("DDL_", ""));
                            ddl.SetSelectItem(vals[1], attr);
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }
        public string EnsName
        {
            get
            {
                string val = this.Page.Request.QueryString["EnsName"];
                if (val == null)
                    val = this.Page.Request.QueryString["EnsName"];

                return val;
            }
        }
        /// <summary>
        ///  According to Map,  Construct a ToolBar
        /// </summary>
        /// <param name="map"></param>
        public void InitByMapV2(bool isShowKey, AttrsOfSearch attrsOfSearch, Attrs attrsOfFK, Attrs attrD1, int page, UserRegedit ur)
        {
            int keysNum = 0;

            //  Keyword .
            if (isShowKey)
            {
                this.AddLab("Lab_Key",  " Keyword ");
                ToolbarTB tb = new ToolbarTB();
                tb.ID = "TB_Key";
                tb.Columns = 9;
                this.AddTB(tb);
                keysNum++;
            }

            //			BP.Sys.Operators ops = new BP.Sys.Operators();
            //			ops.RetrieveAll();

            //  Non-foreign key attribute .
            foreach (AttrOfSearch attr in attrsOfSearch)
            {
                if (attr.IsHidden)
                    continue;

                this.AddLab("Lab_" + attr.Key, attr.Lab);
                keysNum++;

                //if (keysNum==3 || keysNum==6 || keysNum==9)
                //    this.AddBR("b_"+keysNum);

                if (attr.SymbolEnable == true)
                {
                    ToolbarDDL ddl = new ToolbarDDL();
                    ddl.ID = "DDL_" + attr.Key;
                    ddl.SelfShowType = DDLShowType.Ens; //  attr.UIDDLShowType;		 
                    ddl.SelfBindKey = "BP.Sys.Operators";
                    ddl.SelfEnsRefKey = "No";
                    ddl.SelfEnsRefKeyText = "Name";
                    ddl.SelfDefaultVal = attr.DefaultSymbol;
                    ddl.SelfAddAllLocation = AddAllLocation.None;
                    ddl.SelfIsShowVal = false; /// Not to show ID 
                    //ddl.ID="DDL_"+attr.Key;
                    //ddl.SelfBind();
                    this.AddDDL(ddl, false);
                    this.GetDDLByKey("DDL_" + attr.Key).SelfBind();
                }

                if (attr.DefaultVal.Length >= 8)
                {
                    DateTime mydt = BP.DA.DataType.ParseSysDate2DateTime(attr.DefaultVal);

                    /* Datetime type may be .*/
                    //					Map map = new Map();
                    //					map.AddDDLEntities(attr.Key, mydt.ToString("yyyy"),"年",  new BP.Pub.NDs(),false);
                    //					map.AddDDLEntities(attr.Key, mydt.ToString("MM"),"月",  new BP.Pub.YFs(),false);
                    //					map.AddDDLEntities(attr.Key, mydt.ToString("dd"),"日",  new BP.Pub.YFs(),false);


                    ToolbarDDL ddl = new ToolbarDDL();
                    ddl.ID = "DDL_" + attr.Key + "_Year";
                    ddl.SelfShowType = DDLShowType.Ens;
                    ddl.SelfBindKey = "BP.Pub.NDs";
                    ddl.SelfEnsRefKey = "No";
                    ddl.SelfEnsRefKeyText = "Name";
                    ddl.SelfDefaultVal = mydt.ToString("yyyy");
                    ddl.SelfAddAllLocation = AddAllLocation.None;
                    ddl.SelfIsShowVal = false; /// Not to show ID 
                    this.AddDDL(ddl, false);
                    this.GetDDLByKey("DDL_" + attr.Key + "_Year").SelfBind();
                    //ddl.SelfBind();


                    ddl = new ToolbarDDL();
                    ddl.ID = "DDL_" + attr.Key + "_Month";
                    ddl.SelfShowType = DDLShowType.Ens;
                    ddl.SelfBindKey = "BP.Pub.YFs";
                    ddl.SelfEnsRefKey = "No";
                    ddl.SelfEnsRefKeyText = "Name";
                    ddl.SelfDefaultVal = mydt.ToString("MM");
                    ddl.SelfAddAllLocation = AddAllLocation.None;
                    ddl.SelfIsShowVal = false; /// Not to show ID 
                    //	ddl.SelfBind();
                    this.AddDDL(ddl, false);
                    this.GetDDLByKey(ddl.ID).SelfBind();

                    ddl = new ToolbarDDL();
                    ddl.ID = "DDL_" + attr.Key + "_Day";
                    ddl.SelfShowType = DDLShowType.Ens;
                    ddl.SelfBindKey = "BP.Pub.Days";
                    ddl.SelfEnsRefKey = "No";
                    ddl.SelfEnsRefKeyText = "Name";
                    ddl.SelfDefaultVal = mydt.ToString("dd");
                    ddl.SelfAddAllLocation = AddAllLocation.None;
                    ddl.SelfIsShowVal = false; /// Not to show ID 
                    //ddl.SelfBind();
                    this.AddDDL(ddl, false);
                    this.GetDDLByKey(ddl.ID).SelfBind();

                }
                else
                {
                    ToolbarTB tb = new ToolbarTB();
                    tb.ID = "TB_" + attr.Key;
                    tb.Text = attr.DefaultVal;
                    tb.Columns = attr.TBWidth;
                    this.AddTB(tb);
                }
            }

            string className = this.Page.Request.QueryString["EnsName"];
            string cfgVal = "";

            cfgVal = ur.Vals;

            //if (className != null)
            //{
            //    UserRegedit ur = new UserRegedit(WebUser.No, className + "_SearchAttrs");
            //    ur.FK_Emp = WebUser.No;
            //    cfgVal = ur.Vals;
            //}


            //  Foreign key attribute query .			 
            bool isfirst = true;
            foreach (Attr attr in attrsOfFK)
            {
                if (attr.MyFieldType == FieldType.RefText)
                    continue;

                this.AddDDL(new ToolbarDDL(attr, null), false);
                keysNum++;
                //if (keysNum == 3 || keysNum == 6 || keysNum == 9)
                //    this.AddBR("b_" + keysNum);

                if (attr.MyFieldType == FieldType.Enum)
                {
                    this.GetDDLByKey("DDL_" + attr.Key).BindSysEnum(attr.UIBindKey, false, AddAllLocation.TopAndEnd);
                    this.GetDDLByKey("DDL_" + attr.Key).Items[0].Text = ">>" + attr.Desc;
                }
                else
                {
                    switch (attr.UIBindKey)
                    {
                        case "BP.Port.Depts":
                            BP.Port.Depts Depts = new BP.Port.Depts();
                            Depts.RetrieveAll();
                            foreach (BP.Port.Dept zsjg in Depts)
                            {
                                string space = "";
                               // space = space.PadLeft(zsjg.Grade - 1, '-');
                                ListItem li = new ListItem(space + zsjg.Name, zsjg.No);
                                this.GetDDLByKey("DDL_" + attr.Key).Items.Add(li);
                            }
                            if (Depts.Count > SystemConfig.MaxDDLNum)
                                this.AddLab("<a href=\"javascript:onDDLSelectedMore('DDL_" + attr.Key + "', '" + this.EnsName + "', 'BP.Port.Depts', 'No','Name')\" >...</a>");
                            break;
                        //case "BP.Port.Units":
                        //    BP.Port.Units units = new BP.Port.Units();
                        //    units.RetrieveAll();
                        //    foreach (BP.Port.Unit zsjg in units)
                        //    {
                        //        string space = "";
                        //        space = space.PadLeft(zsjg.Grade - 1, '-');
                        //        ListItem li = new ListItem(space + zsjg.Name, zsjg.No);
                        //        this.GetDDLByKey("DDL_" + attr.Key).Items.Add(li);
                        //    }
                        //    if (units.Count > SystemConfig.MaxDDLNum)
                        //        this.AddLab("<a href=\"javascript:onDDLSelectedMore('DDL_" + attr.Key + "', '" + this.EnsName + "', 'BP.Port.Units', 'No','Name')\" >...</a>");
                        //    break;
                        default:
                            this.GetDDLByKey("DDL_" + attr.Key).SelfBind();
                            this.GetDDLByKey("DDL_" + attr.Key).Items[0].Text = ">>" + attr.Desc;
                            break;
                    }
                }

                if (isfirst)
                {
                    //this.GetDDLByKey("DDL_"+attr.Key).SetSelectItem(defSeleVal);
                    //this.GetDDLByKey("DDL_"+attr.Key).SetSelectItem(defSeleVal);
                    isfirst = false;
                }
            }

            this.AddBtn("Btn_Search",  " Inquiry (L)");
            if (this.Page.FindControl("BPToolBar1") == null || this.Page.FindControl("BPToolBar1").Visible == false)
            {
             //   this.AddBtn(NamesOfBtn.ChoseField);
            }
        }
        #endregion

        /// <summary>
        /// init by map
        /// </summary>
        /// <param name="map"></param>
        public void InitByMap(Map map)
        {
          //  InitByMap(true, map.AttrsOfSearch, map.SearchAttrs, null);
        }
        /// <summary>
        ///  According to Map,  Construct a ToolBar
        /// </summary>
        /// <param name="map"></param>
        public void InitByMap(bool isShowKey, AttrsOfSearch attrsOfSearch, Attrs attrsOfFK, Attrs attrD1)
        {
            //  Keyword .
            if (isShowKey)
            {
                this.AddLab("Lab_Key", " Keyword ");
                ToolbarTB tb = new ToolbarTB();
                tb.ID = "TB_Key";
                tb.Columns = 9;
                this.AddTB(tb);
            }

            //	BP.Sys.Operators ops = new BP.Sys.Operators();
            //	ops.RetrieveAll();
            //  Non outside property .
            foreach (AttrOfSearch attr in attrsOfSearch)
            {
                if (attr.IsHidden)
                    continue;

                this.AddLab("Lab_" + attr.Key, attr.Lab);
                if (attr.SymbolEnable == true)
                {
                    ToolbarDDL ddl = new ToolbarDDL();
                    ddl.ID = "DDL_" + attr.Key;
                    ddl.SelfShowType = DDLShowType.Ens; //  attr.UIDDLShowType;		 
                    ddl.SelfBindKey = "BP.Sys.Operators";
                    ddl.SelfEnsRefKey = "No";
                    ddl.SelfEnsRefKeyText = "Name";
                    ddl.SelfDefaultVal = attr.DefaultSymbol;
                    ddl.SelfAddAllLocation = AddAllLocation.None;
                    ddl.SelfIsShowVal = false; //  Not to show ID 
                    ddl.ID = "DDL_" + attr.Key;
                    //ddl.SelfBind();
                    this.AddDDL(ddl, false);
                    this.GetDDLByKey("DDL_" + attr.Key).SelfBind();
                }

                if (attr.DefaultVal.Length >= 7)
                {
                    DateTime mydt = BP.DA.DataType.ParseSysDate2DateTime(attr.DefaultVal);

                    /* Datetime type may be .*/
                    //					Map map = new Map();
                    //					map.AddDDLEntities(attr.Key, mydt.ToString("yyyy"),"年",  new BP.Pub.NDs(),false);
                    //					map.AddDDLEntities(attr.Key, mydt.ToString("MM"),"月",  new BP.Pub.YFs(),false);
                    //					map.AddDDLEntities(attr.Key, mydt.ToString("dd"),"日",  new BP.Pub.YFs(),false);

                    ToolbarDDL ddl = new ToolbarDDL();
                    ddl.ID = "DDL_" + attr.Key + "_Year";
                    ddl.SelfShowType = DDLShowType.Ens;
                    ddl.SelfBindKey = "BP.Pub.NDs";
                    ddl.SelfEnsRefKey = "No";
                    ddl.SelfEnsRefKeyText = "Name";
                    ddl.SelfDefaultVal = mydt.ToString("yyyy");
                    ddl.SelfAddAllLocation = AddAllLocation.None;
                    ddl.SelfIsShowVal = false; /// Not to show ID 
                    this.AddDDL(ddl, false);
                    this.GetDDLByKey("DDL_" + attr.Key + "_Year").SelfBind();
                    //ddl.SelfBind();

                    ddl = new ToolbarDDL();
                    ddl.ID = "DDL_" + attr.Key + "_Month";
                    ddl.SelfShowType = DDLShowType.Ens;
                    ddl.SelfBindKey = "BP.Pub.YFs";
                    ddl.SelfEnsRefKey = "No";
                    ddl.SelfEnsRefKeyText = "Name";
                    ddl.SelfDefaultVal = mydt.ToString("MM");
                    ddl.SelfAddAllLocation = AddAllLocation.None;
                    ddl.SelfIsShowVal = false; /// Not to show ID 
                    //	ddl.SelfBind();
                    this.AddDDL(ddl, false);
                    this.GetDDLByKey(ddl.ID).SelfBind();

                    ddl = new ToolbarDDL();
                    ddl.ID = "DDL_" + attr.Key + "_Day";
                    ddl.SelfShowType = DDLShowType.Ens;
                    ddl.SelfBindKey = "BP.Pub.Days";
                    ddl.SelfEnsRefKey = "No";
                    ddl.SelfEnsRefKeyText = "Name";
                    ddl.SelfDefaultVal = mydt.ToString("dd");
                    ddl.SelfAddAllLocation = AddAllLocation.None;
                    ddl.SelfIsShowVal = false; /// Not to show ID 
                    //ddl.SelfBind();
                    this.AddDDL(ddl, false);
                    this.GetDDLByKey(ddl.ID).SelfBind();

                }
                else
                {
                    ToolbarTB tb = new ToolbarTB();
                    tb.ID = "TB_" + attr.Key;
                    tb.Text = attr.DefaultVal;
                    tb.Columns = attr.TBWidth;
                    this.AddTB(tb);
                }


                //				ToolbarTB tb = new ToolbarTB();
                //				tb.ID = "TB_"+attr.Key;
                //				tb.Text = attr.DefaultVal;
                //				tb.Columns=attr.TBWidth ; 
                //this.AddTB(tb);
            }

            //  Foreign key attribute query .			 
            bool isfirst = true;
            foreach (Attr attr in attrsOfFK)
            {
                if (attr.MyFieldType == FieldType.RefText)
                    continue;

                this.AddDDL(new ToolbarDDL(attr, null), false);

                if (attr.MyFieldType == FieldType.Enum)
                {
                    this.GetDDLByKey("DDL_" + attr.Key).BindSysEnum(attr.UIBindKey, false, AddAllLocation.TopAndEnd);
                    this.GetDDLByKey("DDL_" + attr.Key).Items[0].Text = ">>" + attr.Desc;
                }
                else
                {
                    switch (attr.UIBindKey)
                    {
                        case "BP.Port.Depts":
                            BP.Port.Depts Depts = new BP.Port.Depts();
                            Depts.RetrieveAll();
                            foreach (BP.Port.Dept zsjg in Depts)
                            {
                                string space = "";
                               // space = space.PadLeft(zsjg.Grade - 1, '-');
                                ListItem li = new ListItem(space + zsjg.Name, zsjg.No);
                                this.GetDDLByKey("DDL_" + attr.Key).Items.Add(li);
                            }
                            if (Depts.Count > SystemConfig.MaxDDLNum)
                                this.AddLab("<a href=\"javascript:onDDLSelectedMore('DDL_" + attr.Key + "', '" + this.EnsName + "', 'BP.Port.Depts', 'No','Name')\" >...</a>");
                            break;
                        //case "BP.Port.Units":
                        //    BP.Port.Units units = new BP.Port.Units();
                        //    units.RetrieveAll();
                        //    foreach (BP.Port.Unit zsjg in units)
                        //    {
                        //        string space = "";
                        //        space = space.PadLeft(zsjg.Grade - 1, '-');
                        //        ListItem li = new ListItem(space + zsjg.Name, zsjg.No);
                        //        this.GetDDLByKey("DDL_" + attr.Key).Items.Add(li);
                        //    }
                        //    if (units.Count > SystemConfig.MaxDDLNum)
                        //        this.AddLab("<a href=\"javascript:onDDLSelectedMore('DDL_" + attr.Key + "', '" + this.EnsName + "', 'BP.Port.Units', 'No','Name')\" >...</a>");
                        //    break;
                        default:
                            this.GetDDLByKey("DDL_" + attr.Key).SelfBind();
                            this.GetDDLByKey("DDL_" + attr.Key).Items[0].Text = ">>" + attr.Desc;
                            break;
                    }

                    ////  All the information Bind.					
                    //if (attr.UIBindKey=="BP.Port.Depts" )
                    //{
                    //    BP.Port.Depts Depts = new BP.Port.Depts();
                    //    Depts.RetrieveAll();
                    //    foreach(BP.Port.Dept  zsjg in Depts)
                    //    {
                    //        string space="";
                    //        space=space.PadLeft(zsjg.Grade-1,'-' );
                    //        ListItem li = new ListItem( space+zsjg.Name, zsjg.No );
                    //        this.GetDDLByKey("DDL_"+attr.Key).Items.Add( li );
                    //    }
                    //    //this.GetDDLByKey("DDL_"+attr.Key).SetSelectItemByIndex(Depts.Count-1);		
                    //}
                    //else
                    //{
                    //    this.GetDDLByKey("DDL_"+attr.Key).SelfBind();
                    //    this.GetDDLByKey("DDL_"+attr.Key).Items[0].Text="=>"+attr.Desc;

                    //}
                }

                if (isfirst)
                {
                    //this.GetDDLByKey("DDL_"+attr.Key).SetSelectItem(defSeleVal);
                    //this.GetDDLByKey("DDL_"+attr.Key).SetSelectItem(defSeleVal);

                    isfirst = false;
                }
            }

            this.AddBtn("Btn_Search",  " Inquiry (F)");
        }
        public BPToolBar()
        {
            //this.CssClass="BPToolBar"+WebUser.Style;
            //this.Font.Size=FontUnit.XSmall;
            //this.PreRender +=new System.EventHandler(this.TBPreRender);
            //this.Init += new System.EventHandler(this.TBInit);
            //this.OnUnload += new System.EventHandler(this.TBInit);
            //this.OnInit += new System.EventHandler(this.myOnInit);

            //  en.HoverStyle.CssText = "font-size:14px";
            // en.SelectedStyle.CssText = "font-size:14px";
            // en.DefaultStyle.CssText = "font-size:14px";

            this.Style.Add("font-size", "14px");
        }

        public void BindHisSearchDDL(Attrs attrs)
        {
            foreach (Attr attr in attrs)
            {
                this.GetDDLByKey("DDL_" + attr.Key).SelfBind();
            }

        }



        #endregion

        #region  Increase control methods 

        public void AddHtml_del(string html)
        {

            //Panel1.Controls.Add(  );
            //System.Web.UI.Control cl = new System.Web.UI.Control();
            //	cl.


            //this.Controls.Add( this.ParseControl(  html ) );
            //this.Items.Add(en);
        }

        public void AddLab(ToolbarLab en)
        {
            this.Items.Add(en);
        }
        public void AddLab(string text)
        {
            ToolbarLab en = new ToolbarLab();
            en.Text = text;
            en.ID = "L" + this.Items.Count;
            this.AddLab(en);
        }
        public void AddLab(string id, string text, string icon)
        {
            ToolbarLab en = new ToolbarLab();
            en.Text = text;
            en.ID = id;
            en.ImageUrl = icon;
            this.AddLab(en);
        }
        public void AddLab(string id, string text)
        {
            ToolbarLab en = new ToolbarLab(id);
            en.Text = text;
            this.AddLab(en);
        }
        public void AddLabWithIcon(string id, string text)
        {
            ToolbarLab en = new ToolbarLab();
            en.Text = text;
            en.ID = id;
            en.SetImageUrl();
            this.AddLab(en);
        }
        /// <summary>
        ///  By a btn
        /// </summary>
        /// <param name="id">NamesOfBtns</param>
        public void AddBtn(string id)
        {
            if ((BP.Web.WebUser.SysLang != "CH"))
            {
                id = id.Replace("Btn_", "");
                string tx = id;
                ToolbarBtn en1 = new ToolbarBtn("Btn_" + id, tx);
                en1.HoverStyle.CssText = "font-size:14px";
                en1.SelectedStyle.CssText = "font-size:14px";
                en1.DefaultStyle.CssText = "font-size:14px";
                en1.AccessKey = "F";
                if (en1.ID == "Btn_Delete")
                {
                    //this.AddSpt("dl");
                    // this.Attributes["onclick"] += "alert( window.event.srcElement.tagName ); alert( window.event.srcElement );";
                }
                this.AddBtn(en1);
                return;
            }

            string text = "";
            switch (id)
            {
                case NamesOfBtn.UnDo:
                    text = " Undo operation ";
                    break;
                case NamesOfBtn.Do:
                    text = " Carried out ";
                    break;
                case NamesOfBtn.ChoseField:
                    text = " Select the field ";
                    break;
                case NamesOfBtn.DataGroup:
                    text = " Grouping queries ";
                    break;
                case NamesOfBtn.Copy:
                    text = " Copy ";
                    break;
                case NamesOfBtn.Go:
                    text = " Go to ";
                    break;
                case NamesOfBtn.ExportToModel:
                    text = " Template ";
                    break;
                case NamesOfBtn.DataCheck:
                    text = " Data Check ";
                    break;
                case NamesOfBtn.DataIO:
                    text = " Data Import ";
                    break;
                case NamesOfBtn.Statistic:
                    text = " Statistics ";
                    break;
                case NamesOfBtn.Balance:
                    text = " Fair ";
                    break;
                case NamesOfBtn.Down:
                    text = " Drop ";
                    break;
                case NamesOfBtn.Up:
                    text = " Rise ";
                    break;
                case NamesOfBtn.Chart:
                    text = " Graph ";
                    break;
                case NamesOfBtn.Rpt:
                    text = " Report form ";
                    break;
                case NamesOfBtn.ChoseCols:
                    text = " Select the column of the query ";
                    break;
                case NamesOfBtn.Excel:
                    text = " Export all ";
                    break;
                case NamesOfBtn.Excel_S:
                    text = " Export Current ";
                    break;
                case NamesOfBtn.Xml:
                    text = " Export to Xml";
                    break;
                case NamesOfBtn.Send:
                    text = " Send ";
                    break;
                case NamesOfBtn.Reply:
                    text = " Reply ";
                    break;
                case NamesOfBtn.Forward:
                    text = " Forwarding ";
                    break;
                case NamesOfBtn.Next:
                    text = " Next ";
                    break;
                case NamesOfBtn.Previous:
                    text = " Previous ";
                    break;
                case NamesOfBtn.Selected:
                    text = " Choose ";
                    break;
                case NamesOfBtn.Add:
                    text = " Increase ";
                    break;
                case NamesOfBtn.Adjunct:
                    text = " Accessory ";
                    break;
                case NamesOfBtn.AllotTask:
                    text = " Batch task ";
                    break;
                case NamesOfBtn.Apply:
                    text = " Application ";
                    break;
                case NamesOfBtn.ApplyTask:
                    text = " Application task ";
                    break;
                case NamesOfBtn.Back:
                    text = " Retreat ";
                    break;
                case NamesOfBtn.Card:
                    text = " Card ";
                    break;
                case NamesOfBtn.Close:
                    text = " Shut down ";
                    break;
                case NamesOfBtn.Confirm:
                    text = " Determine ";
                    break;
                case NamesOfBtn.Delete:
                    text = " Delete ";
                    break;
                case NamesOfBtn.Edit:
                    text = " Editor ";
                    break;
                case NamesOfBtn.EnList:
                    text = " List ";
                    break;
                case NamesOfBtn.Cancel:
                    text = " Cancel ";
                    break;
                case NamesOfBtn.Export:
                    text = " Export ";
                    break;
                case NamesOfBtn.FileManager:
                    text = " File Management ";
                    break;
                case NamesOfBtn.Help:
                    text = " Help ";
                    break;
                case NamesOfBtn.Insert:
                    text = " Insert ";
                    break;
                case NamesOfBtn.LogOut:
                    text = "Log Out";
                    break;
                case NamesOfBtn.Messagers:
                    text = " News ";
                    break;
                case NamesOfBtn.New:
                    text = " New ";
                    break;
                case NamesOfBtn.Print:
                    text = " Print ";
                    break;
                case NamesOfBtn.Refurbish:
                    text = " Refresh ";
                    break;
                case NamesOfBtn.Reomve:
                    text = " Remove ";
                    break;
                case NamesOfBtn.Save:
                    text = " Save ";
                    break;
                case NamesOfBtn.SaveAndClose:
                    text = " Save and Close ";
                    break;
                case NamesOfBtn.SaveAndNew:
                    text = " Save and New ";
                    break;
                case NamesOfBtn.SaveAsDraft:
                    text = " Save Draft ";
                    break;
                case NamesOfBtn.Search:
                    text = " Find ";
                    break;
                case NamesOfBtn.SelectAll:
                    text = " Select all ";
                    break;
                case NamesOfBtn.SelectNone:
                    text = " Uncheck ";
                    break;
                case NamesOfBtn.View:
                    text = " Check out ";
                    break;
                case NamesOfBtn.Update:
                    text = " Update ";
                    break;
                default:
                    throw new Exception("@ Not defined ToolBarBtn  Mark  " + id);
            }

            ToolbarBtn en = new ToolbarBtn(id, text);
            en.HoverStyle.CssText = "font-size:14px";
            en.SelectedStyle.CssText = "font-size:14px";
            en.DefaultStyle.CssText = "font-size:14px";
            en.AccessKey = "F";

            if (en.ID == "Btn_Delete")
            {
                this.AddSpt("dl");
                // this.Attributes["onclick"] += "alert( window.event.srcElement.tagName ); alert( window.event.srcElement );";
            }

            this.AddBtn(en);
        }
        public void AddBtn(string id, string text)
        {
            ToolbarBtn en = new ToolbarBtn(id, text);
            this.AddBtn(en);
        }
        public void AddBtn(string id, string text, string ToolTip)
        {
            ToolbarBtn en = new ToolbarBtn();
            en.ID = id;
            en.Text = text;
            en.ToolTip = ToolTip;
            this.AddBtn(en);
        }
        public void AddBtn(string id, string text, string ToolTip, string icon)
        {
            ToolbarBtn en = new ToolbarBtn();
            en.ID = id;
            en.Text = text;
            en.ToolTip = ToolTip;
            en.ImageUrl = icon;
            this.AddBtn(en);
        }
        public void AddBtn(ToolbarBtn en)
        {
            foreach (ToolbarItem item in this.Items)
            {
                if (item.ID == en.ID)
                {
                    ToolbarBtn btn = this.GetBtnByKey(en.ID);
                    btn = en;
                    return;
                }
            }
            this.Items.Add(en);
        }
        public void AddSpt(string id)
        {
            ToolbarSpt en = new ToolbarSpt();
            en.ID = id;
            this.AddSpt(en);
        }
        public void AddSpt(ToolbarSpt en)
        {
            //  en.DefaultStyle.CssText=""
            //   en.HoverStyle.CssText = "font-size:14px";
            // en.SelectedStyle.CssText = "font-size:14px";
            //  en.DefaultStyle.CssText = "font-size:14px";

            this.Items.Add(en);
        }
        public void AddBR(string id)
        {

            return;




            /*
           ToolbarBR en = new ToolbarBR();
           en.ID = id;	
           this.Items.Add(en);
           */

            //this.AddSpt(en);
            //this.Items.Add(en);	

        }


        public void AddDDL(ToolbarDDL en, bool AutoPostBack)
        {
            en.AutoPostBack = AutoPostBack;
            this.Items.Add(en);
        }
        public void AddDDL(string desc, string id, bool AutoPostBack)
        {
            this.AddLab(desc);
            ToolbarDDL en = new ToolbarDDL(id);
            en.AutoPostBack = AutoPostBack;
            this.Items.Add(en);
        }
        public void AddDDL(string id, bool AutoPostBack)
        {
            ToolbarDDL en = new ToolbarDDL(id);
            en.AutoPostBack = AutoPostBack;
            this.Items.Add(en);
        }
        public void AddTB(ToolbarTB en)
        {
            this.Items.Add(en);
        }
        /// <summary>
        /// AddTB
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Columns"></param>
        public void AddTB(string id, int Columns)
        {
            ToolbarTB en = new ToolbarTB(id);
            en.Columns = Columns;
            //en.Width=Unit.Pixel(50);
            this.Items.Add(en);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void AddTB(string id)
        {
            ToolbarTB en = new ToolbarTB(id);
            //en.Width=Unit.Pixel(50);
            this.Items.Add(en);
        }
        /// <summary>
        ///  Join Information 
        /// </summary>
        /// <param name="tbdesc"></param>
        /// <param name="id"></param>
        public void AddTB(string tbdesc, string id)
        {
            this.AddLab("Lab" + id, tbdesc);
            this.AddTB(id, 10);
        }
        public void AddTB(string tbdesc, string id, int cols)
        {
            this.AddLab("Lab" + id, tbdesc);
            this.AddTB(id, cols);
        }
        public void AddDDL(ToolbarDDL en, System.EventHandler selectedIndexChanged, bool AutoPostBack)
        {
            //en.AutoPostBack = AutoPostBack ;
            //en.SelectedIndexChanged +=selectedIndexChanged;
            this.Items.Add(en);
        }
        public void AddDDL(string id, System.EventHandler selectedIndexChanged, bool AutoPostBack)
        {
            ToolbarDDL en = new ToolbarDDL(id);
            //en.AutoPostBack = AutoPostBack ; 
            //en.SelectedIndexChanged +=selectedIndexChanged;
            this.Items.Add(en);
        }
        #endregion
    }
}
