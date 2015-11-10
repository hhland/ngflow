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
    /// Item3Ens 的摘要说明。
    /// </summary>
    public partial class Item3Ens : System.Web.UI.Page
    {
        public int PageSize
        {
            get
            {
                return 15;
            }
        }
        public int PageIdx
        {
            get
            {
                string str = this.Request.QueryString["PageIdx"];
                if (str == null || str == "")
                    str = "1";
                return int.Parse(str);
            }
            set
            {
                ViewState["PageIdx"] = value;
            }
        }
        public string Paras
        {
            get
            {
                string str = "";
                foreach (string s in this.Request.QueryString)
                {
                    str += "&" + s + "=" + this.Request.QueryString[s];
                }
                return str;
            }
        }
        public string EnsName
        {
            get
            {
                string clsName = this.Request.QueryString["EnsName"];
                if (clsName == null)
                    clsName = this.Request.QueryString["EnsName"];
                return clsName;
            }
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            Entities ens = ClassFactory.GetEns(this.EnsName);
            Entity en = ens.GetNewEntity;

            QueryObject qo = new QueryObject(ens);
            int num = qo.GetCount();
            qo.DoQuery(en.PK, this.PageSize, this.PageIdx, true);


            UAC uac = en.HisUAC;
            if (uac.IsInsert)
                this.Label1.Text = PubClass.GenerLabelStr("<B>" + en.EnDesc + "</b>&nbsp;&nbsp;[<a href=\"javascript:Edit('Item3.aspx?EnName=" + en.ToString() + this.Paras + "') ;\" ><img src='/Images/Btn/Add.gif' border=0/>增加记录</a>][<a href='javascript:window.location.reload();'><img src='/Images/Btn/Refurbish.gif' border=0/>刷新</a>]");
            else
                this.Label1.Text = PubClass.GenerLabelStr("<B>" + en.EnDesc + "</b>&nbsp;&nbsp;[<a href='javascript:window.location.reload();'><img src='/Images/Btn/Refurbish.gif' border=0/>刷新</a>]");

            this.DataPanelDtl(ens);

            this.UCSys2.BindPageIdx(num, this.PageSize , this.PageIdx, "Item3Ens.aspx?EnsName=" + this.EnsName+"&PageSize="+this.PageSize );
        }
        public string GenerEnUrl(Entity en, Attrs attrs)
        {
            string url = "";
            foreach (Attr attr in attrs)
            {
                switch (attr.UIContralType)
                {
                    case UIContralType.TB:
                        if (attr.IsPK)
                            url += "&" + attr.Key + "=" + en.GetValStringByKey(attr.Key);
                        break;
                    case UIContralType.DDL:
                        url += "&" + attr.Key + "=" + en.GetValStringByKey(attr.Key);
                        break;
                }
            }
            return url;
        }
        private void AddAttr(Entity en, Attr attr, string url)
        {
            string cfgurl = "";
            if (attr.UIContralType == UIContralType.DDL)
            {
                this.UCSys1.AddTD(en.GetValRefTextByKey(attr.Key));
                return;
            }

            string str = en.GetValStringByKey(attr.Key);
            switch (attr.MyDataType)
            {
                case DataType.AppDate:
                case DataType.AppDateTime:
                    if (str == "" || str == null)
                        str = "&nbsp;";
                    this.UCSys1.AddTD(str);
                    break;
                case DataType.AppString:
                    if (str == "" || str == null)
                        str = "&nbsp;";

                    if (attr.UIHeight != 0)
                    {
                        this.UCSys1.AddTDDoc("大块文本信息", "");
                        //this.UCSys1.AddTDDoc(str, str);
                    }
                    else
                    {
                        if (attr.Key.IndexOf("ail") == -1)
                            this.UCSys1.AddTD(str);
                        else
                            this.UCSys1.AddTD("<a href=\"javascript:mailto:" + str + "\"' >" + str + "</a>");
                    }
                    break;
                case DataType.AppBoolean:
                    if (str == "1")
                        this.UCSys1.AddTD("是");
                    else
                        this.UCSys1.AddTD("否");
                    break;
                case DataType.AppFloat:
                case DataType.AppInt:
                case DataType.AppRate:
                case DataType.AppDouble:
                    if (cfgurl == "")
                        this.UCSys1.AddTDNum(str);
                    else
                    {
                        cfgurl = cfgurl.Replace("@Keys", url);
                        this.UCSys1.AddTDNum("<a href=\"javascript:WinOpen('" + cfgurl + "','dtl1');\" >" + str + "</a>");
                    }
                    break;
                case DataType.AppMoney:
                    cfgurl = "";
                    if (cfgurl == "")
                    {
                        this.UCSys1.AddTDNum(decimal.Parse(str).ToString("0.00"));
                    }
                    else
                    {
                        cfgurl = cfgurl.Replace("@Keys", url);
                        this.UCSys1.AddTDNum("<a href=\"javascript:WinOpen('" + cfgurl + "','dtl1');\" >" + decimal.Parse(str).ToString("0.00") + "</a>");
                    }
                    break;
                default:
                    throw new Exception("no this case ...");
            }
        }
        public void DataPanelDtl(Entities ens)
        {
            Entity myen = ens.GetNewEntity;
            string pk = myen.PK;
            string clName = myen.ToString();
            Attrs attrs = myen.EnMap.Attrs;
            Attrs selectedAttrs = myen.EnMap.Attrs; //.GetChoseAttrs( ens ) ;


            // 生成标题
            this.UCSys1.AddTable();
            this.UCSys1.AddTR();
            this.UCSys1.AddTDTitle("序");
            foreach (Attr attrT in selectedAttrs)
            {
                if (attrT.UIVisible == false)
                    continue;

                this.UCSys1.AddTDTitle(attrT.Desc);
            }
            this.UCSys1.AddTDTitle("操作");

            UAC uac = myen.HisUAC;

            string msg = "";
            if (uac.IsUpdate)
                msg += "编辑";


            this.UCSys1.AddTREnd();
            int idx = 0;
            foreach (Entity en in ens)
            {
                #region 处理keys
                string style = WebUser.Style;
                string url = this.GenerEnUrl(en, attrs);
                #endregion

                this.UCSys1.Add("<TR  onmouseover='TROver(this)' onmouseout='TROut(this)' ondblclick=\"Edit('Item3.aspx?EnsName=" + ens.ToString() + "&PK=" + en.GetValByKey(pk) + url + "')\" >");
                idx++;
                this.UCSys1.AddTDIdx(idx);
                string val = "";
                foreach (Attr attr in selectedAttrs)
                {
                    if (attr.UIVisible == false)
                        continue;

                    this.AddAttr(en, attr, url);
                }

                if (uac.IsDelete)
                    this.UCSys1.AddTD("[<a href=\"javascript:Edit('Item3.aspx?EnsName=" + ens.ToString() + "&PK=" + en.GetValByKey(pk) + url + "');\">" + msg + "</a>][<a href=\"javascript:Del('"+en.ToString()+"','" + en.PKVal + "')\" >删除</a>]");
                else
                    this.UCSys1.AddTD("[<a href=\"javascript:Edit('Item3.aspx?EnsName=" + ens.ToString() + "&PK=" + en.GetValByKey(pk) + url + "');\">" + msg + "</a>]");

                this.UCSys1.AddTREnd();
            }

            #region  求合计代码写在这里。
            string NoShowSum = SystemConfig.GetConfigXmlEns("NoShowSum", ens.ToString());
            if (NoShowSum == null)
                NoShowSum = "";

            bool IsHJ = false;
            foreach (Attr attr in selectedAttrs)
            {
                if (attr.UIVisible == false)
                    continue;
                if (attr.MyFieldType == FieldType.RefText)
                    continue;

                if (attr.UIContralType == UIContralType.DDL)
                    continue;

                if (NoShowSum.IndexOf("@" + attr.Key + "@") != -1)
                    continue;

                if (attr.Key == "OID" || attr.Key == "MID" || attr.Key.ToUpper() == "WORKID")
                    continue;

                switch (attr.MyDataType)
                {
                    case DataType.AppDouble:
                    case DataType.AppFloat:
                    case DataType.AppInt:
                    case DataType.AppMoney:
                        IsHJ = true;
                        break;
                    default:
                        break;
                }
            }

            if (ens.Count <= 1)
                IsHJ = false;


            if (IsHJ)
            {
                // 找出配置是不显示合计的列。

                if (NoShowSum == null)
                    NoShowSum = "";

                this.UCSys1.Add("<TR class='TRSum' >");
                this.UCSys1.AddTD("合计");
                foreach (Attr attr in selectedAttrs)
                {

                    if (attr.MyFieldType == FieldType.RefText)
                        continue;

                    if (attr.UIVisible == false)
                        continue;


                    if (attr.MyDataType == DataType.AppBoolean)
                    {
                        this.UCSys1.AddTD();
                        continue;
                    }

                    if (attr.UIContralType == UIContralType.DDL)
                    {
                        this.UCSys1.AddTD();
                        continue;
                    }
                    if (attr.Key == "OID" || attr.Key == "MID" || attr.Key.ToUpper() == "WORKID")
                    {
                        this.UCSys1.AddTD();
                        continue;
                    }


                    if (NoShowSum.IndexOf("@" + attr.Key + "@") != -1)
                    {
                        /*不需要显示它他们的合计。*/
                        this.UCSys1.AddTD();
                        continue;
                    }



                    switch (attr.MyDataType)
                    {
                        case DataType.AppDouble:
                            this.UCSys1.AddTDNum(ens.GetSumDecimalByKey(attr.Key));
                            break;
                        case DataType.AppFloat:
                            this.UCSys1.AddTDNum(ens.GetSumDecimalByKey(attr.Key));
                            break;
                        case DataType.AppInt:
                            this.UCSys1.AddTDNum(ens.GetSumDecimalByKey(attr.Key));
                            break;
                        case DataType.AppMoney:
                            this.UCSys1.AddTDJE(ens.GetSumDecimalByKey(attr.Key));
                            break;
                        default:
                            this.UCSys1.AddTD();
                            break;
                    }
                }
                this.UCSys1.AddTREnd();
            }
            #endregion

            this.UCSys1.AddTableEnd();
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
    }

}