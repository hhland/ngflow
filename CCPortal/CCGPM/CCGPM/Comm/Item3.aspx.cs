using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BP.En;
using BP.DA;
using BP.Web;
using BP.Web.Controls;
using BP.Sys;
using BP.Sys.Xml;

namespace BP.Web.Comm
{
    /// <summary>
    /// Item3 的摘要说明。
    /// </summary>
    public partial class Item3 : System.Web.UI.Page
    {
        #region attrs
        public Control GenerLabel(string title)
        {
            return this.ParseControl(title);

            string path = this.Request.ApplicationPath;
            if (path == "/" || path == "")
                path = "";
            
            string str = "";
            str += "<TABLE   cellpadding='0' cellspacing='0' background='" + path + "/Images/DG_bgright.gif'>";
            str += "<TBODY>";
            str += "<TR>";
            str += "<TD>";
            str += "<IMG src='" + path + "/Images/DG_Title_Left.gif' border='0' width='30' height='26'></TD>";
            str += "<TD   vAlign='bottom' noWrap background='/Images/DG_Title_BG.gif'>&nbsp;";
            str += " &nbsp;" + title + "&nbsp;&nbsp;";
            str += "</TD>";
            str += "<TD>";
            str += "<IMG src='" + path + "/Images/DG_Title_Right.gif' border='0' width='25' height='26'></TD>";
            str += "</TR>";
            str += "</TBODY>";
            str += "</TABLE>";
            return this.ParseControl(str);
        }
        
        //protected System.Web.UI.WebControls.Label Label1;
        public Entities _GetEns = null;
        //protected System.Web.UI.WebControls.Label Label2;
        /// <summary>
        /// 当前的实体集合．
        /// </summary>
        public Entities GetEns
        {
            get
            {
                if (_GetEns == null)
                {
                    if (this.EnName != null)
                    {
                        Entity en = ClassFactory.GetEn(EnName);
                        if (en == null)
                            throw new Exception(EnName);

                        _GetEns = en.GetNewEntities;
                    }
                    else
                    {
                        _GetEns = ClassFactory.GetEns(EnsName);
                    }
                }
                return _GetEns;
            }
        }
        public string EnName
        {
            get
            {
                return this.Request.QueryString["EnName"];
            }
        }
        public string EnsName
        {
            get
            {
                return this.Request.QueryString["EnsName"];
            }
        }
        public string PK
        {
            get
            {
                return ViewState["PK"] as string;
            }
            set
            {
                ViewState["PK"] = value;
            }
        }
        /// <summary>
        /// 得到一个新的事例数据．
        /// </summary>
        public Entity GetEnDa
        {
            get
            {
                Entity en = this.GetEns.GetNewEntity;
                if (en.PKCount == 1)
                {
                    if (this.PK == null || this.PK == "")
                    {
                        return en;
                    }
                    else
                    {
                        try
                        {
                            en.PKVal = this.PK;
                            en.Retrieve();
                            return en;
                        }
                        catch(Exception ex)
                        {
                            en.CheckPhysicsTable();
                            throw ex;
                        }
                    }
                }
                else if (en.IsMIDEntity)
                {
                    string val = this.Request.QueryString["MID"];
                    if (val == null)
                        val = this.Request.QueryString["PK"];
                    if (val == null)
                    {
                        return en;
                    }
                    else
                    {
                        en.SetValByKey("MID", val);
                        en.Retrieve();
                        return en;
                    }
                }

                Attrs attrs = en.EnMap.Attrs;
                foreach (Attr attr in attrs)
                {
                    if (attr.IsPK)
                    {
                        string str = this.Request.QueryString[attr.Key];
                        if (str == null)
                        {
                            if (en.IsMIDEntity)
                            {
                                en.SetValByKey("MID", this.Request.QueryString["PK"]);
                                continue;
                            }
                            else
                            {
                                throw new Exception("@没有把主键值[" + attr.Key + "]传输过来.");
                            }
                        }

                        en.SetValByKey(attr.Key, this.Request.QueryString[attr.Key]);
                    }
                }
                if (en.IsExits == false)
                {
                    throw new Exception("@数据没有记录.");
                }
                else
                {
                    en.Retrieve();
                }
                return en;
            }
        }

        /// <summary>
        /// 是不是只读
        /// </summary>
        public bool IsReadonly
        {
            get
            {
                if (ViewState["IsReadonly"] == null)
                    return false;
                else
                    return (bool)ViewState["IsReadonly"];
            }
            set
            {
                ViewState["IsReadonly"] = value;
            }
        }
        public Entity _CurrEn = null;
        public Entity CurrEn
        {
            get
            {
                if (_CurrEn == null)
                {
                    _CurrEn = this.GetEnDa;
                }
                return _CurrEn;
            }
            set
            {
                _CurrEn = value;
            }
        }
        #endregion

        public Button Btn_Save
        {
            get
            {
                return this.UCEn1.GetButtonByID("Btn_Save");
            }
        }
        public Button Btn_Del
        {
            get
            {
                return this.UCEn1.GetButtonByID("Btn_Del");
            }
        }
        public Button Btn_DelFile
        {
            get
            {
                return this.UCEn1.GetButtonByID("Btn_DelFile");
            }
        }
        public Button Btn_New
        {
            get
            {
                return this.UCEn1.GetButtonByID("Btn_New");
            }
        }
        public bool IsShowReback
        {
            get
            {
                if (this.Request.QueryString["EnsName"] != null)
                    return true;
                else
                    return false;
            }
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (this.IsPostBack == false)
            {
                this.PK = this.Request.QueryString["PK"];

                if (this.PK == null || this.PK == "")
                    this.PK = this.Request.QueryString["OID"];

                if (this.PK == null || this.PK == "")
                    this.PK = this.Request.QueryString["No"];

                if (this.PK == null || this.PK == "")
                    this.PK = this.Request.QueryString["MyPK"];
            }

            this.Label1.Controls.Add(this.GenerLabel(this.CurrEn.EnDesc));

            /*
            if (this.IsShowReback)
            {
                this.Label1.Controls.Add("<a href='Item3Ens.aspx?EnsName="+this.Request.QueryString["EnsName"]+"'>"+this.GenerLabel(this.CurrEn.EnDesc));
            }
            else
            {
                this.Label1.Controls.Add(this.GenerLabel(this.CurrEn.EnDesc));
            }
            */
            try
            {
                this.Bind3Item(this.CurrEn, this.IsReadonly, false);
            }
            catch (Exception ex)
            {
                this.CurrEn.CheckPhysicsTable();
                throw ex;
            }

            if (this.IsReadonly == false && this.Btn_Save!=null )
                this.Btn_Save.Click += new EventHandler(Btn_Save_Click);


            if (this.Btn_Del != null)
                this.Btn_Del.Click += new EventHandler(Btn_Del_Click);

            if (this.Btn_New != null)
                this.Btn_New.Click += new EventHandler(Btn_New_Click);

            if (this.Btn_DelFile != null)
                this.Btn_DelFile.Click += new EventHandler(Btn_DelFile_Click);
        }

        public void Bind3Item(Entity en, bool isReadonly, bool isShowDtl)
        {
            AttrDescs ads = new AttrDescs(en.ToString());

            this.UCEn1.HisEn = en;
            this.UCEn1.IsReadonly = isReadonly;
            this.UCEn1.IsShowDtl = isShowDtl;
            this.UCEn1.Controls.Clear();
            this.UCEn1.Attributes["visibility"] = "hidden";
            this.UCEn1.Controls.Clear();

            this.UCEn1.Add("<table width='100%' id='AutoNumber1' border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse' bordercolor='#111111' >");
            bool isLeft = true;
            object val = null;
            Attrs attrs = en.EnMap.Attrs;
             int docTBCount = 0;
             foreach (Attr attr in attrs)
             {
                 if (attr.MyFieldType == FieldType.MultiValues)
                     docTBCount++;
             }

            foreach (Attr attr in attrs)
            {
                if (attr.Key == "MyNum")
                    continue;

                val = en.GetValByKey(attr.Key);
                if (attr.UIContralType == UIContralType.TB)
                {
                    if (attr.MyFieldType == FieldType.RefText)
                    {
                        continue;
                    }
                    else if (attr.MyFieldType == FieldType.MultiValues)
                    {
                        /* 如果是多值的.*/
                        LB lb = new LB(attr);
                        lb.Visible = true;
                        if (docTBCount == 1)
                            lb.Height = 128;
                        else
                            lb.Height = 300;

                        lb.SelectionMode = ListSelectionMode.Multiple;
                        Entities ens = ClassFactory.GetEns(attr.UIBindKey);
                        ens.RetrieveAll();
                        this.UCEn1.AddTR();
                        this.UCEn1.Controls.Add(lb);
                    }
                    else
                    {
                        if (attr.UIVisible == false)
                        {
                            this.UCEn1.SetValByKey(attr.Key, val.ToString());
                            continue;
                        }
                        else
                        {
                            if (attr.UIHeight != 0)
                            {
                                
                                /* doc 文本类型。　*/
                                //FredCK.FCKeditorV2.FCKeditor  area = new FredCK.FCKeditorV2.FCKeditor();
                                //area.ID = "TB_" + attr.Key;
                                //if (val.ToString() == "" && en.IsEmpty==false && attr.Key=="Doc" )
                                //    val = en.GetValDocText();
                                //area.Value = val.ToString();

                                this.UCEn1.AddTR();
                                this.UCEn1.Add("<TD colspan=3 nowarp=true class='FDesc' >" + attr.Desc + "</TD>");
                                this.UCEn1.AddTREnd();

                                this.UCEn1.AddTR();
                                this.UCEn1.Add("<TD colspan=3 class='DocCell' height='400' >");
                               // this.UCEn1.Add(area);
                                this.UCEn1.Add("</TD>");
                                this.UCEn1.AddTREnd();
                                continue;
                            }
                            else
                            {
                                TB tb = new TB();
                                tb.ID = "TB_" + attr.Key;
                                tb.IsHelpKey = false;


                                if (isReadonly || attr.UIIsReadonly)
                                    tb.Enabled = false;
                                switch (attr.MyDataType)
                                {
                                    case DataType.AppMoney:
                                        tb.Text = decimal.Parse(val.ToString()).ToString("0.00");
                                        break;
                                    default:
                                        tb.Text = val.ToString();
                                        break;
                                }
                                tb.Attributes["width"] = "100%";
                                this.UCEn1.AddTR();
                                this.UCEn1.AddContral(attr.Desc, tb);

                                /*
                                AttrDesc ad = ads.GetEnByKey(AttrDescAttr.Attr,  attr.Key ) as AttrDesc;
                                if (ad!=null)
                                    this.AddContral(attr.Desc,tb);
                                else
                                {
                                    //this.AddContral(attr.Desc,tb);

                                    tb.Attributes["width"]="";

                                    //this.AddTR();
                                    this.Add("<TD class='DGCellOfFieldInfo1' width='1%' >"+attr.Desc+"</TD>");
                                    this.Add("<TD class='DGCellOfEnterInfo1' colspan=2 >");
                                    this.Add(tb);
                                    this.Add("</TD>");
                                    this.AddTREnd();
                                    continue;
                                }
                                */

                            }
                        }
                    }
                }
                else if (attr.UIContralType == UIContralType.CheckBok)
                {
                    CheckBox cb = new CheckBox();
                    cb.Checked = en.GetValBooleanByKey(attr.Key);

                    if (isReadonly || !attr.UIIsReadonly)
                        cb.Enabled = false;
                    else
                        cb.Enabled = attr.UIVisible;

                    cb.ID = "CB_" + attr.Key;
                    this.UCEn1.AddTR();
                    this.UCEn1.AddContral(attr.Desc, cb);
                }
                else if (attr.UIContralType == UIContralType.DDL)
                {
                    if (isReadonly || !attr.UIIsReadonly)
                    {
                        /* 如果是 DDLIsEnable 的, 就要找到. */
                        if (attr.MyFieldType == FieldType.Enum)
                        {
                            /* 如果是 enum 类型 */
                            int enumKey = int.Parse(val.ToString());
                            BP.Sys.SysEnum enEnum = new BP.Sys.SysEnum(attr.UIBindKey, "CH", enumKey);

                            //DDL ddl = new DDL(attr,text,en.Lab,false);
                            DDL ddl = new DDL();
                            ddl.Items.Add(new ListItem(enEnum.Lab, val.ToString()));
                            ddl.Items[0].Selected = true;
                            ddl.Enabled = false;
                            ddl.ID = "DDL_" + attr.Key;

                            this.UCEn1.AddTR();
                            this.UCEn1.AddContral(attr.Desc, ddl, false);
                            //this.Controls.Add(ddl);
                        }
                        else
                        {
                            /* 如果是 ens 类型 */
                            Entities ens = ClassFactory.GetEns(attr.UIBindKey);
                            Entity en1 = ens.GetNewEntity;
                            en1.SetValByKey(attr.UIRefKeyValue, val.ToString());
                            string lab = "";
                            try
                            {
                                en1.Retrieve();
                                lab = en1.GetValStringByKey(attr.UIRefKeyText);
                            }
                            catch
                            {
                                if (SystemConfig.IsDebug == false)
                                {
                                    lab = "" + val.ToString();
                                }
                                else
                                {
                                    lab = "" + val.ToString();
                                    //lab="没有关联到值"+val.ToString()+"Class="+attr.UIBindKey+"EX="+ex.Message;
                                }
                            }

                            DDL ddl = new DDL(attr, val.ToString(), lab, false, this.Page.Request.ApplicationPath);
                            ddl.ID = "DDL_" + attr.Key;
                            this.UCEn1.AddTR();
                            this.UCEn1.AddContral(attr.Desc, ddl, false);
                            //this.Controls.Add(ddl);
                        }
                    }
                    else
                    {
                        /* 可以使用的情况. */
                        DDL ddl1 = new DDL(attr, val.ToString(), "enumLab", true, this.Page.Request.ApplicationPath);
                        ddl1.ID = "DDL_" + attr.Key;

                        this.UCEn1.AddTR();
                        this.UCEn1.AddContral(attr.Desc, ddl1, true);
                    }
                }
                else if (attr.UIContralType == UIContralType.RadioBtn)
                {

                }

                AttrDesc ad1 = ads.GetEnByKey(AttrDescAttr.Attr, attr.Key) as AttrDesc;
                if (ad1 == null)
                    this.UCEn1.AddTD("class='Note'", "&nbsp;");
                else
                    this.UCEn1.AddTD("class='Note'", ad1.Desc);

                this.UCEn1.AddTREnd();
            } //结束循环.

            #region 查看是否包含 MyFile字段如果有就认为是附件。
            if (en.EnMap.Attrs.Contains("MyFileName"))
            {
                /* 如果包含这二个字段。*/
                string fileName = en.GetValStringByKey("MyFileName");
                string filePath = en.GetValStringByKey("MyFilePath");
                string fileExt = en.GetValStringByKey("MyFileExt");

                string url = "";
                if (fileExt != "")
                {
                    BP.Sys.EnCfg cfg = new EnCfg(en.ToString());

                    // 系统物理路径。
                    string path = this.Request.PhysicalApplicationPath.ToLower();
                    string path1 = filePath.ToLower();
                    path1 = path1.Replace(path, "");
                    url = "&nbsp;&nbsp;<a href='" + cfg.FJWebPath + en.PKVal + "." + fileExt + "' target=_blank ><img src='../Images/FileType/" + fileExt + ".gif' border=0 />" + fileName + "</a>";
                }

                this.UCEn1.AddTR();
                AttrDesc ad1 = ads.GetEnByKey(AttrDescAttr.Attr, "MyFileName") as AttrDesc;
                if (ad1 == null)
                    this.UCEn1.AddTD(" nowrap=true class='FDesc' ", "附件或图片:");
                else
                    this.UCEn1.AddTD(" align=right nowrap=true class='FDesc' ", ad1.Desc);

                HtmlInputFile file = new HtmlInputFile();
                file.ID = "file";
                // file.Attributes.Add("class", "Btn1");
                file.Attributes.Add("style", "width:60%");

                this.UCEn1.Add("<TD colspan=2 class='FDesc'  >");
                this.UCEn1.Add(file);
                this.UCEn1.Add(url + "&nbsp;&nbsp;");
                if (fileExt != "")
                {
                    Button btn1 = new Button();
                    btn1.Text = "移除";
                    btn1.CssClass = "Btn";
                    btn1.Attributes.Add("class", "Btn1");
                    btn1.ID = "Btn_DelFile";
                    btn1.Attributes["onclick"] += " return confirm('此操作要执行移除附件或图片，是否继续？');";
                    this.UCEn1.Add(btn1);
                }
                if (en.IsEmpty == false)
                {
                    if (en.EnMap.HisAttrFiles.Count > 0 || en.EnMap.Attrs.Contains("MyFileNum"))
                        this.UCEn1.Add("<a href=\"javascript:WinOpen('FileManager.aspx?EnName=" + en.ToString() + "&PK=" + en.PKVal + "','mn')\" >上传附件</></TD>");
                }
                else
                {
                    this.UCEn1.AddTDEnd();
                }
                this.UCEn1.AddTREnd();
            }
            #endregion

            #region save button .
            this.UCEn1.AddTR();
            this.UCEn1.Add("<TD align=center colspan=3>");

            Button btn = new Button();
            if (en.HisUAC.IsInsert)
            {
                btn = new Button();
                btn.ID = "Btn_New";
                btn.Text = "  新 建  ";
                btn.CssClass = "Btn";
                btn.Attributes.Add("class", "Btn1");

                this.UCEn1.Add(btn);
                this.UCEn1.Add("&nbsp;");
            }

            if (en.HisUAC.IsUpdate)
            {
                btn = new Button();
                btn.ID = "Btn_Save";
                btn.Text = "  保 存  ";
                btn.CssClass = "Btn";

                btn.Attributes.Add("class", "Btn1");
                this.UCEn1.Add(btn);
                this.UCEn1.Add("&nbsp;");
            }


            if (en.HisUAC.IsDelete)
            {
                btn = new Button();
                btn.ID = "Btn_Del";
                btn.Text = "  删 除  ";
                btn.CssClass = "Btn";

                btn.Attributes.Add("class", "Btn1");

                btn.Attributes["onclick"] = " return confirm('您确定要执行删除吗？');";
                this.UCEn1.Add(btn);
                this.UCEn1.Add("&nbsp;");
            }

            this.UCEn1.Add("&nbsp;<input type=button class=Btn onclick='javascript:window.close()' value='  关  闭  ' />");

            this.UCEn1.Add("</TD>");
            this.UCEn1.AddTREnd();
            #endregion

            this.UCEn1.AddTableEnd();


            if (en.IsExit(en.PK, en.PKVal) == false)
                return;

            string refstrs = "";
            if (en.IsEmpty)
            {
                refstrs += "";
                return;
            }

            this.UCEn1.Add("<HR>");

            string keys = "&PK=" + en.PKVal.ToString();
            foreach (Attr attr in en.EnMap.Attrs)
            {
                if (attr.MyFieldType == FieldType.Enum ||
                    attr.MyFieldType == FieldType.FK ||
                    attr.MyFieldType == FieldType.PK ||
                    attr.MyFieldType == FieldType.PKEnum ||
                    attr.MyFieldType == FieldType.PKFK)
                    keys += "&" + attr.Key + "=" + en.GetValStringByKey(attr.Key);
            }
            Entities hisens = en.GetNewEntities;

            keys += "&r=" + System.DateTime.Now.ToString("ddhhmmss");
            refstrs = BP.Web.Comm.UC.UCEn.GetRefstrs(keys, en, en.GetNewEntities);
            if (refstrs != "")
                refstrs += "<hr>";
            this.UCEn1.Add(refstrs);
        }

        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        private void Btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                Entity en = this.UCEn1.GetEnData(this.GetEns.GetNewEntity);
                this.CurrEn = en;
                if (this.PK == null && en.IsExits == false)
                {
                    en.Insert();
                    this.PK = en.PKVal.ToString();
                }
                else
                    en.Update();

                #region 保存附件
                if (en.EnMap.Attrs.Contains("MyFileName"))
                {

                    HtmlInputFile file = this.UCEn1.FindControl("file") as HtmlInputFile;
                    if (file != null && file.Value.IndexOf(":") != -1)
                    {
                        /* 如果包含这二个字段。*/
                        string fileName = file.PostedFile.FileName;
                        fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                        //en.SetValByKey("MyFileName",fileName);

                        EnCfg cfg = new EnCfg(en.ToString());
                        string filePath = cfg.FJSavePath; // en.GetValStringByKey("MyFilePath");
                        en.SetValByKey("MyFilePath", filePath);

                        string ext = "";
                        if (fileName.IndexOf(".") != -1)
                            ext = fileName.Substring(fileName.LastIndexOf(".") + 1);

                        en.SetValByKey("MyFileExt", ext);

                        //						// 处理 myFileName
                        //						string myFileName = en.GetValStringByKey("MyFileName");
                        //						myFileName = myFileName.Substring(0,myFileName.LastIndexOf("."));
                        //						myFileName = myFileName+"."+ext;

                        en.SetValByKey("MyFileName", fileName);

                        string fullFile = filePath + "/" + en.PKVal + "." + ext;
                        file.PostedFile.SaveAs(fullFile);
                        en.Update();
                    }
                }
                #endregion


                this.Bind3Item(this.CurrEn, this.IsReadonly, false);
                this.Btn_Save.Click += new EventHandler(Btn_Save_Click);
                this.Label2.Text = "保存成功。" + DataType.CurrentTime;
                this.Label2.ForeColor = Color.Blue;
            }
            catch (Exception ex)
            {
                this.Label2.ForeColor = Color.Red;
                this.Label2.Text = ex.Message;
            }
        }

        private void Btn_Del_Click(object sender, EventArgs e)
        {
            try
            {
                Entity en = this.UCEn1.GetEnData(this.GetEns.GetNewEntity);
                this.CurrEn = en;
                en.Delete();
                this.Label2.ForeColor = Color.Blue;
                this.Label2.Text = "删除成功。";
                this.CurrEn.ResetDefaultVal();

                this.UCEn1.Clear();
                this.UCEn1.AddMsgOfInfo("提示:", "删除成功.");
                //this.WinClose();
            }
            catch (Exception ex)
            {
                this.Label2.ForeColor = Color.Red;
                this.Label2.Text = ex.Message;
            }
        }

        private void Btn_New_Click(object sender, EventArgs e)
        {
            this.Response.Redirect("Item3.aspx?EnName=" + this.EnName,true);
        }

        private void Btn_DelFile_Click(object sender, EventArgs e)
        {
            try
            {
                Entity en = this.UCEn1.GetEnData(this.GetEns.GetNewEntity);
                string file = en.GetValStringByKey("MyFilePath") + "//" + en.PKVal + "." + en.GetValStringByKey("MyFileExt");
                System.IO.File.Delete(file);
                en.SetValByKey("MyFileExt", "");
                en.SetValByKey("MyFileName", "");
                en.SetValByKey("MyFilePath", "");
                en.Update();

                this.CurrEn = en;
                this.UCEn1.Bind3Item(this.CurrEn, this.IsReadonly, false);

                this.Btn_Save.Click += new EventHandler(Btn_Save_Click);
                this.Label2.Text = "文件删除成功:" + DataType.CurrentTime;
                this.Label2.ForeColor = Color.Blue;
            }
            catch (Exception ex)
            {
                this.Label2.ForeColor = Color.Red;
                this.Label2.Text = ex.Message;
            }
        }
    }

}