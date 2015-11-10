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
using BP.DA;
using BP.En;
using BP.Web;
using BP.Web.Controls;
using BP.Sys;

namespace BP.Web.Comm
{
    /// <summary>
    /// 显示方式
    /// </summary>
    /// <summary>
    /// SimplyNoName 的摘要说明
    /// </summary>
    public partial class Search : WebPage
    {
        public int PageIdxOfSeach
        {
            get
            {
                if (ViewState["PageIdxOfSeach"] == null)
                    return this.PageIdx;
                else
                    return 1;
            }
            set
            {
                ViewState["PageIdxOfSeach"] = value;
            }
        }
        public new Entities HisEns
        {
            get
            {
                if (this.EnsName != null)
                {
                    if (this._HisEns == null)
                        _HisEns = ClassFactory.GetEns(this.EnsName);
                }
                return _HisEns;
            }
        }
        public new string Key
        {
            get
            {
                return this.Request.QueryString["Key"];
            }
        }
        public new string EnsName
        {
            get
            {
                string str = this.Request.QueryString["EnsName"];
                if (str == null)
                    str = this.Request.QueryString["EnsName"];
                if (str == null)
                    throw new Exception("类名无效。");
                return str;
            }
        }
        /// <summary>
        /// _HisEns
        /// </summary>
        public new  Entities _HisEns = null;
        private  Entity _HisEn = null;
        public new Entity HisEn
        {
            get
            {
                if (_HisEn == null)
                    _HisEn = this.HisEns.GetNewEntity;
                return _HisEn;
            }
        }

        public ShowWay ShowWay
        {
            get
            {
                if (Session["ShowWay"] == null)
                {
                    if (this.Request.QueryString["ShowWay"] == null)
                    {
                        Session["ShowWay"] = "2";
                    }
                    else
                    {
                        Session["ShowWay"] = this.Request.QueryString["ShowWay"];
                    }
                }
                return (ShowWay)int.Parse(Session["ShowWay"].ToString());
            }
            set
            {
                Session["ShowWay"] = (int)value;
            }
        }

        public bool IsReadonly
        {
            get
            {
                string i = this.Request.QueryString["IsReadonly"];
                if (i == "1")
                    return true;
                else
                    return false;
            }
        }

        public TB TB_Key
        {
            get
            {
                return this.ToolBar1.GetTBByID("TB_Key");
            }
        }

        /// <summary>
        /// 当前选择de En
        /// </summary>
        public Entity CurrentSelectEnDa
        {
            get
            {
                Entity en = this.HisEn;
                en.Retrieve();
                return en;
            }
        }
        public bool IsShowGroup
        {
            get
            {
                if (this.Request.QueryString["IsShowGroup"] == null)
                {
                    return true;
                }
                else
                {
                    if (this.Request.QueryString["IsShowGroup"] == "0")
                        return false;
                    else
                        return true;
                }
            }
        }
        public bool IsShowToolBar1
        {
            get
            {
                string str = this.Request.QueryString["IsShowToolBar1"];
                if (str == null || str == "1")
                    return true;
                return false;
            }
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.Page.RegisterClientScriptBlock("s",
           "<link href='./Style/Table" + BP.Web.WebUser.Style + ".css' rel='stylesheet' type='text/css' />");
            // this.BPToolBar1.Visible = this.IsShowToolBar1;
            UAC uac = this.HisEn.HisUAC;
            if (uac.IsView == false)
                throw new Exception("您没有查看[" + this.HisEn.EnDesc + "]数据的权限.");

            if (this.IsReadonly)
            {
                uac.IsDelete = false;
                uac.IsInsert = false;
                uac.IsUpdate = false;
            }

            if (this.Request.QueryString["PageIdx"] == null)
                this.PageIdx = 1;
            else
                this.PageIdx = int.Parse(this.Request.QueryString["PageIdx"]);


            Entity en = this.HisEn;
            Map map = en.EnMap;
            this.ShowWay = ShowWay.Dtl;

            if (uac.IsView == false)
                throw new Exception("@对不起，您没有查看的权限！");

            #region 设置toolbar2 的 contral  设置查寻功能.

            this.ToolBar1.InitByMapV2(map, 1);


            this.ToolBar1.AddSpt("spt2");
            this.ToolBar1.AddBtn(NamesOfBtn.Excel);

            bool isEdit = true;
            if (this.IsReadonly)
                isEdit = false;

            if (uac.IsInsert == false)
                isEdit = false;

            if (isEdit)
                this.ToolBar1.AddLab("inse",
      "<input type=button id='ToolBar1$Btn_New' class=Btn name='ToolBar1$Btn_New' onclick=\"javascript:ShowEn('UIEn.aspx?EnsName=" + this.EnsName + "','cd','" + BP.Sys.EnsAppCfgs.GetValInt(this.EnsName, "WinCardH") + "' , '" + BP.Sys.EnsAppCfgs.GetValInt(this.EnsName, "WinCardW") + "');\"  value='新建(N)'  />");

            if (WebUser.No=="admin")
                this.ToolBar1.AddLab("sw", "<input type=button class=Btn  id='ToolBar1$Btn_P' class=Btn name='ToolBar1$Btn_P'  onclick=\"javascript:OpenAttrs('" + this.EnsName + "');\"  value='设置(P)'  />");

            #endregion

            #region 设置选择的 默认值
            AttrSearchs searchs = map.SearchAttrs;
            foreach (AttrSearch attr in searchs)
            {
                string mykey = this.Request.QueryString[attr.Key];
                if (mykey == "" || mykey == null)
                    continue;
                else
                    this.ToolBar1.GetDDLByKey("DDL_" + attr.Key).SetSelectItem(mykey, attr.HisAttr);
            }

            if (this.Request.QueryString["Key"] != null)
            {
                this.ToolBar1.GetTBByID("TB_Key").Text = this.Request.QueryString["Key"];
            }
            #endregion

            this.SetDGData();

            this.ToolBar1.GetBtnByID("Btn_Search").Click += new System.EventHandler(this.ToolBar1_ButtonClick);
            this.ToolBar1.GetBtnByID("Btn_Excel").Click += new System.EventHandler(this.ToolBar1_ButtonClick);
            this.Label1.Text = this.GenerCaption(this.HisEn.EnMap.EnDesc + "" + this.HisEn.EnMap.TitleExt);
            //this.GenerLabel(this.Label1, this.HisEn);
            //this.ResponseWriteBlueMsg(this.DDL_GroupKey.AutoPostBack.ToString() +this.BPToolBar1.Enabled.ToString()+ this.BPToolBar1.AutoPostBack.ToString() );
        }

        #region 方法

        public Entities SetDGData()
        {
            return this.SetDGData(this.PageIdx);
        }
        public Entities SetDGData(int pageIdx)
        {
            Entities ens = this.HisEns;
            Entity en = ens.GetNewEntity;
            QueryObject qo = new QueryObject(ens);
            qo = this.ToolBar1.GetnQueryObject(ens, en);
            int maxPageNum = 0;
            try
            {
                this.UCSys2.Clear();
                  maxPageNum = this.UCSys2.BindPageIdx(qo.GetCount(), SystemConfig.PageSize, pageIdx, "Search.aspx?EnsName=" + this.EnsName);
                if (maxPageNum > 1)
                    this.UCSys2.Add("翻页键:← → PageUp PageDown");
            }
            catch(Exception ex)
            {
                try
                {
                    en.CheckPhysicsTable();
                }
                catch(Exception wx)
                {
                    BP.DA.Log.DefaultLogWriteLineError(wx.Message);
                }
                maxPageNum = this.UCSys2.BindPageIdx(qo.GetCount(), SystemConfig.PageSize, pageIdx, "Search.aspx?EnsName=" + this.EnsName);
            }

            qo.DoQuery(en.PK, SystemConfig.PageSize, pageIdx);

            if (en.EnMap.IsShowSearchKey)
            {
                string keyVal = this.ToolBar1.GetTBByID("TB_Key").Text.Trim();
                if (keyVal.Length >= 1)
                {
                    Attrs attrs = en.EnMap.Attrs;
                    foreach (Entity myen in ens)
                    {
                        foreach (Attr attr in attrs)
                        {
                            if (attr.IsFKorEnum)
                                continue;

                            if (attr.IsPK)
                                continue;

                            switch (attr.MyDataType)
                            {
                                case DataType.AppRate:
                                case DataType.AppMoney:
                                case DataType.AppInt:
                                case DataType.AppFloat:
                                case DataType.AppDouble:
                                case DataType.AppBoolean:
                                    continue;
                                default:
                                    break;
                            }
                            myen.SetValByKey(attr.Key, myen.GetValStrByKey(attr.Key).Replace(keyVal, "<font color=red>" + keyVal + "</font>"));
                        }
                    }
                }
            }

            // string groupkey = this.DDL_GroupKey.SelectedItemStringVal;
            // string groupkey2 = "None";

            this.UCSys1.DataPanelDtl(ens, null);

            int ToPageIdx = this.PageIdx + 1;
            int PPageIdx = this.PageIdx - 1;

            this.UCSys1.Add("<SCRIPT language=javascript>");
            this.UCSys1.Add("\t\n document.onkeydown = chang_page;");
            this.UCSys1.Add("\t\n function chang_page() { ");
            //  this.UCSys3.Add("\t\n  alert(event.keyCode); ");
            if (this.PageIdx == 1)
            {
                this.UCSys1.Add("\t\n if (event.keyCode == 37 || event.keyCode == 33) alert('已经是第一页');");
            }
            else
            {
                this.UCSys1.Add("\t\n if (event.keyCode == 37  || event.keyCode == 38 || event.keyCode == 33) ");
                this.UCSys1.Add("\t\n     location='Search.aspx?EnsName=" + this.EnsName + "&PageIdx=" + PPageIdx + "';");
            }

            if (this.PageIdx == maxPageNum)
            {
                this.UCSys1.Add("\t\n if (event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 34) alert('已经是最后一页');");
            }
            else
            {
                this.UCSys1.Add("\t\n if (event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 34) ");
                this.UCSys1.Add("\t\n     location='Search.aspx?EnsName=" + this.EnsName + "&PageIdx=" + ToPageIdx + "';");
            }

            this.UCSys1.Add("\t\n } ");
            this.UCSys1.Add("</SCRIPT>");
            return ens;
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

        }
        #endregion

        #endregion

        private void ToolBar1_Del_ButtonClick(object sender, EventArgs e)
        {
            try
            {
                if (sender.GetType() == typeof(Btn))
                {
                    Btn mybtn = (Btn)sender;
                    switch (mybtn.ID)
                    {
                        case NamesOfBtn.New:
                            this.WinOpen("UIEn.aspx?EnName=" + this.HisEn.ToString());
                            this.SetDGData();
                            break;
                        case NamesOfBtn.ChoseField:
                            this.WinOpen("UIEnsFieldChose.aspx?EnsName=" + this.EnsName + "&dt=" + DateTime.Now.ToString("mmhhss"), "fle", "field", 400, 500, 150, 160, false, false);
                            this.SetDGData();
                            return;
                        case NamesOfBtn.ChoseCols:
                            this.Response.Redirect("UIEnsCols.aspx?EnsName=" + this.HisEns.ToString(), true);
                            break;
                        case NamesOfBtn.DataGroup:
                            this.Response.Redirect("Group.aspx?EnsName=" + this.HisEns.ToString(), true);
                            break;
                        default:
                            break;
                    }
                    return;
                }

                ToolbarCheckBtn btn = (ToolbarCheckBtn)sender;
                switch (btn.ID)
                {
                    case "Btn_New": // 缩落图
                        this.InvokeEnManager(this.EnsName, null, true);
                        break;
                    case "Btn_Card": // 缩落图
                        this.ShowWay = ShowWay.Cards;
                        break;
                    case "Btn_Top40": // 列表
                        this.ShowWay = ShowWay.List;
                        break;
                    case "Btn_Dtl":  // 详细信息
                        this.ShowWay = ShowWay.Dtl;
                        break;
                    default:
                        break;
                }
            }
            catch
            {
                //this.WinOpen("UIEn.aspx?EnName=BP.TA.Task");
                this.ShowWay = ShowWay.Dtl;
            }
            this.SetDGData();
        }


        private void ToolBar1_ButtonClick(object sender, System.EventArgs e)
        {
            try
            {
                Btn btn = (Btn)sender;
                switch (btn.ID)
                {
                    case NamesOfBtn.Insert: //数据导出
                        this.Response.Redirect("UIEn.aspx?EnName=" + this.HisEn.ToString(), true);
                        // this.Response.Write("<script language='JavaScript' src='JScript.js'>WinOpen('UIEn.aspx?EnsName="+this.EnsName+"&PK="+this.PK+"', 'cd' )</script>");
                        return;
                    case NamesOfBtn.Excel: //数据导出
                        Entities ens = this.HisEns;
                        Entity en = ens.GetNewEntity;
                        QueryObject qo = new QueryObject(ens);
                        qo = this.ToolBar1.GetnQueryObject(ens, en);
                        qo.DoQuery();
                        string file = "";
                        try
                        {
                            //this.ExportDGToExcel(ens.ToDataTableDescField(), this.HisEn.EnDesc);
                            file = this.ExportDGToExcel(ens.ToDataTableDesc(), this.HisEn.EnDesc);
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                file = this.ExportDGToExcel(ens.ToDataTableDescField(), this.HisEn.EnDesc);
                            }
                            catch
                            {
                                throw new Exception("数据没有正确导出可能的原因之一是:系统管理员没正确的安装Excel组件，请通知他，参考安装说明书解决。@系统异常信息：" + ex.Message);
                            }
                        }
                        this.SetDGData();
                        return;
                    case NamesOfBtn.Excel_S: //数据导出.
                        Entities ens1 = this.SetDGData();
                        try
                        {
                            this.ExportDGToExcel(ens1.ToDataTableDesc(), this.HisEn.EnDesc);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("数据没有正确导出可能的原因之一是:系统管理员没正确的安装Excel组件，请通知他，参考安装说明书解决。@系统异常信息：" + ex.Message);
                        }
                        this.SetDGData();
                        return;
                    //case NamesOfBtn.Search: //数据导出
                    //    this.ToolBar1.SaveSearchState(this.EnsName);
                    //    this.Response.Redirect("Search.aspx?EnsName="+this.EnsName,true);
                    //    return;
                    case NamesOfBtn.Xml: //数据导出
                        //this.Xml(this.HisEns);
                        return;
                    case "Btn_Print":  //数据导出.
                        //this.ExportDGToExcel(this.DG1,this.HisEn.EnDesc);
                        return;
                    default:
                        //this.ToolBar1.SaveSearchState(this.EnsName);
                        //this.Response.Redirect("Search.aspx?EnsName=" + this.EnsName, true);
                        // return;
                        this.PageIdx = 1;
                        this.SetDGData(1);
                        this.ToolBar1.SaveSearchState(this.EnsName, null);
                        return;
                }
            }
            catch (Exception ex)
            {
                if (!(ex is System.Threading.ThreadAbortException))
                {
                    this.ResponseWriteRedMsg(ex);
                    //在这里显示错误
                }
            }
        }

        private bool Btn_New_ButtonClick(object sender, EventArgs e)
        {
            this.WinOpen("UIEn.aspx?EnName=" + this.HisEn.ToString());
            this.SetDGData();
            return false;
        }
    }
}
