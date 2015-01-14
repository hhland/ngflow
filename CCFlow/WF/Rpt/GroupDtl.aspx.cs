using System;
using System.Collections;
using System.Collections.Generic;
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
using BP.Sys;
using BP.Web;
using BP.Web.Controls;
using BP.Web.UC;
using BP.XML;
using BP.Sys.Xml;
using BP.Port;
using BP.Web.Comm;
using System.Collections.Specialized;
using BP.WF.Rpt;

namespace CCFlow.WF.Rpt
{
    /// <summary>
    /// GroupEnsDtl  The summary .
    /// </summary>
    public partial class UIContrastDtl : BP.Web.WebPage
    {
        #region  Property 
        public string RptNo
        {
            get
            {
                return this.Request.QueryString["RptNo"];
            }
        }
        public string DTFrom
        {
            get
            {
                return this.Request.QueryString["DTFrom"];
            }
        }
        public string DTTo
        {
            get
            {
                return this.Request.QueryString["DTTo"];
            }
        }
        public string TBKey
        {
            get
            {
                return this.Request.QueryString["Key"];
            }
        }
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }

        public string FK_Dept
        {
            get
            {
                return (string)ViewState["FK_Dept"];
            }
            set
            {
                string val = value;
                if (val == "all")
                    return;

                if (this.FK_Dept == null)
                {
                    ViewState["FK_Dept"] = value;
                    return;
                }

                if (this.FK_Dept.Length > val.Length)
                    return;

                ViewState["FK_Dept"] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region  Processing style 
            //this.Page.RegisterClientScriptBlock("sds",
            // "<link href='/WF/Comm/Style/Table" + BP.Web.WebUser.Style + ".css' rel='stylesheet' type='text/css' />");

            if (this.Request.QueryString["PageIdx"] == null)
                this.PageIdx = 1;
            else
                this.PageIdx = int.Parse(this.Request.QueryString["PageIdx"]);
            #endregion  Processing style 

            //edited by liuxc,2014-12-18

            //BP.WF.Rpt.MapRpt mr = new MapRpt();
            //this.FK_Flow = mr.ParentMapData;
            //this.FK_Flow = this.FK_Flow.Replace("ND", "");
            //this.FK_Flow = this.FK_Flow.Replace("Rpt", "");
            //this.FK_Flow = this.FK_Flow.Replace("My", "");
            //this.FK_Flow = this.FK_Flow.PadLeft(3, '0');

            this.BindData();
        }

        public void BindData()
        {
            string RptNo = this.Request.QueryString["RptNo"];
            if (RptNo == null)
                RptNo = this.Request.QueryString["RptNo"];

            Entities ens = BP.En.ClassFactory.GetEns(RptNo);
            Entity en = ens.GetNewEntity;

            QueryObject qo = new QueryObject(ens);
            string[] strs = this.Request.RawUrl.Split('&');
            string[] strs1 = this.Request.RawUrl.Split('&');
            int i = 0;
            foreach (string str in strs)
            {
                if (str.IndexOf("RptNo") != -1)
                    continue;

                string[] mykey = str.Split('=');
                string key = mykey[0];

                if (key == "OID" || key == "MyPK")
                    continue;

                if (key == "FK_Dept")
                {
                    this.FK_Dept = mykey[1];
                    continue;
                }

                if (en.EnMap.Attrs.Contains(key) == false)
                    continue;

                qo.AddWhere(mykey[0], mykey[1]);
                qo.addAnd();
            }


            if (this.FK_Dept != null && (this.Request.QueryString["FK_Emp"] == null
                || this.Request.QueryString["FK_Emp"] == "all"))
            {
                if (this.FK_Dept.Length == 2)
                {
                    qo.AddWhere("FK_Dept", " = ", "all");
                    qo.addAnd();
                }
                else
                {
                    if (this.FK_Dept.Length == 8)
                    {
                        //if (this.Request.QueryString["ByLike"] != "1")
                        qo.AddWhere("FK_Dept", " = ", this.FK_Dept);
                    }
                    else
                    {
                        qo.AddWhere("FK_Dept", " like ", this.FK_Dept + "%");
                    }
                    qo.addAnd();
                }
            }
            qo.AddHD();

            #region  Plus date period .
            if (en.EnMap.DTSearchWay != DTSearchWay.None)
            {
                string field = en.EnMap.DTSearchKey;
                qo.addAnd();
                qo.addLeftBracket();
                if (en.EnMap.DTSearchWay == DTSearchWay.ByDate)
                {
                    qo.AddWhere(field, " >= ", this.DTFrom + " 01:01");
                    qo.addAnd();
                    qo.AddWhere(field, " >= ", this.DTTo + " 23:59");
                }
                else
                {
                    qo.AddWhere(field, " >= ", this.DTFrom);
                    qo.addAnd();
                    qo.AddWhere(field, " >= ", this.DTTo);
                }
                qo.addRightBracket();
            }
            #endregion

            int num = qo.DoQuery();
            this.DataPanelDtl(ens, null);
        }

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
        #endregion

        public void DataPanelDtl(Entities ens, string ctrlId)
        {
            //   this.Controls.Clear();
            Entity myen = ens.GetNewEntity;
            string pk = myen.PK;
            string clName = myen.ToString();
            Attrs attrs = myen.EnMap.Attrs;
            var attrCount = 0;
            var visibleAttrs = new List<Attr>();

            foreach (Attr attrT in attrs)
            {
                if (attrT.UIVisible == false)
                    continue;

                if (attrT.Key == "Title" || attrT.Key == "MyNum")
                    continue;

                attrCount++;
                visibleAttrs.Add(attrT);
            }

            MapRpt md = new MapRpt(this.RptNo, this.FK_Flow);

            this.Pub1.AddTable("class='Table' cellSpacing='0' cellPadding='0'  border='0' style='width:100%'");
            this.Pub1.AddTR();
            this.Pub1.AddTDGroupTitle("colspan=" + (attrCount + 2), myen.EnMap.EnDesc + "  Record :" + ens.Count + "Ìõ");
            this.Pub1.AddTREnd();
            this.Pub1.AddTR();
            this.Pub1.AddTDGroupTitle("style='text-align:center'", "Ðò");
            this.Pub1.AddTDGroupTitle(" Title ");

            visibleAttrs.ForEach(attr => Pub1.AddTDGroupTitle(attr.Desc));

            bool isRefFunc = false;
            isRefFunc = true;
            int pageidx = this.PageIdx - 1;
            int idx = SystemConfig.PageSize * pageidx;
            this.Pub1.AddTREnd();
            string style = WebUser.Style;

            foreach (Entity en in ens)
            {
                this.Pub1.AddTR();
                idx++;
                this.Pub1.AddTDIdx(idx);
                //this.Pub1.Add("<TD class='TD'><a href=\"javascript:WinOpen('../WorkOpt/OneWork/Track.aspx?FK_Flow=" + this.FK_Flow + "&WorkID=" + en.GetValStrByKey("OID") + "');\" ><img src='../Img/Track.png' border=0 />"+en.GetValStrByKey("Title")+"</a></TD>");
                this.Pub1.Add("<TD class='TD'><a href=\"javascript:WinOpen('../WorkOpt/OneWork/Track.aspx?FK_Flow=" + this.FK_Flow + "&WorkID=" + en.GetValStrByKey("OID") + "');\" ><img src='../Img/Track.png' border=0 />" + en.GetValStrByKey("Title") + "</a></TD>");

                foreach (var attr in visibleAttrs)
                {
                    if (attr.UIContralType == UIContralType.DDL)
                    {
                        if (attr.UIBindKey == "BP.Pub.NYs")
                            this.Pub1.AddTD(en.GetValStrByKey(attr.Key));
                        else
                            this.Pub1.AddTD(en.GetValRefTextByKey(attr.Key));
                        continue;
                    }

                    if (attr.UIHeight != 0)
                    {
                        this.Pub1.AddTDDoc("...", "...");
                        continue;
                    }

                    string str = en.GetValStrByKey(attr.Key);
                    switch (attr.MyDataType)
                    {
                        case DataType.AppDate:
                        case DataType.AppDateTime:
                            if (str == "" || str == null)
                                str = "&nbsp;";
                            this.Pub1.AddTD(str);
                            break;
                        case DataType.AppString:
                            if (str == "" || str == null)
                                str = "&nbsp;";
                            if (attr.UIHeight != 0)
                                this.Pub1.AddTDDoc(str, str);
                            else
                                this.Pub1.AddTD(str);
                            break;
                        case DataType.AppBoolean:
                            if (str == "1")
                                this.Pub1.AddTD("ÊÇ");
                            else
                                this.Pub1.AddTD("·ñ");
                            break;
                        case DataType.AppFloat:
                        case DataType.AppInt:
                        case DataType.AppRate:
                        case DataType.AppDouble:
                            this.Pub1.AddTDNum(str);
                            break;
                        case DataType.AppMoney:
                            this.Pub1.AddTDNum(decimal.Parse(str).ToString("0.00"));
                            break;
                        default:
                            throw new Exception("no this case ...");
                    }
                }

                this.Pub1.AddTREnd();
            }

            #region   Total code required to write here .

            bool IsHJ = false;
            foreach (Attr attr in attrs)
            {
                if (attr.MyFieldType == FieldType.RefText || attr.UIContralType == UIContralType.DDL)
                    continue;

                if (attr.Key == "OID" || attr.Key == "FID"
                    || attr.Key == "MID" || attr.Key.ToUpper() == "WORKID"
                    || attr.Key == BP.WF.Data.NDXRptBaseAttr.FlowEndNode
                    || attr.Key == BP.WF.Data.NDXRptBaseAttr.PWorkID)
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

            if (IsHJ)
            {
                //  Identify the configuration is not displayed in the column total .
                this.Pub1.Add("<TR class='TRSum'>");
                this.Pub1.AddTD("class=Sum", " Total ");
                this.Pub1.AddTD("class=Sum", "");

                foreach (Attr attr in attrs)
                {
                    if (attr.MyFieldType == FieldType.RefText
                        || attr.UIVisible == false
                        || attr.Key == "MyNum")
                        continue;

                    if (attr.MyDataType == DataType.AppBoolean)
                    {
                        this.Pub1.AddTD("class=Sum", "");
                        continue;
                    }

                    if (attr.UIContralType == UIContralType.DDL)
                    {
                        this.Pub1.AddTD("class=Sum", "");
                        continue;
                    }

                    if (attr.Key == "OID" || attr.Key == "FID"
                        || attr.Key == "MID" || attr.Key.ToUpper() == "WORKID")
                    {
                        this.Pub1.AddTD("class=Sum", "");
                        continue;
                    }

                    switch (attr.MyDataType)
                    {
                        case DataType.AppDouble:
                            this.Pub1.AddTDNum(ens.GetSumDecimalByKey(attr.Key));
                            break;
                        case DataType.AppFloat:
                            this.Pub1.AddTDNum(ens.GetSumDecimalByKey(attr.Key));
                            break;
                        case DataType.AppInt:
                            this.Pub1.AddTDNum(ens.GetSumDecimalByKey(attr.Key));
                            break;
                        case DataType.AppMoney:
                            this.Pub1.AddTDJE(ens.GetSumDecimalByKey(attr.Key));
                            break;
                        default:
                            this.Pub1.AddTD("class=Sum", "");
                            break;
                    }
                }

                this.Pub1.AddTREnd();
            }
            #endregion

            this.Pub1.AddTableEnd();
        }
        private void DataPanelDtlAdd(Entity en, Attr attr, BP.Sys.Xml.Searchs cfgs, string url, string cardUrl, string focusField)
        {
            string cfgurl = "";
            if (attr.UIContralType == UIContralType.DDL)
            {
                this.Pub1.AddTD(en.GetValRefTextByKey(attr.Key));
                return;
            }

            if (attr.UIHeight != 0)
            {
                this.Pub1.AddTDDoc("...", "...");
                return;
            }

            string str = en.GetValStrByKey(attr.Key);

            if (focusField == attr.Key)
                str = "<a href=" + cardUrl + ">" + str + "</a>";

            switch (attr.MyDataType)
            {
                case DataType.AppDate:
                case DataType.AppDateTime:
                    if (str == "" || str == null)
                        str = "&nbsp;";
                    this.Pub1.AddTD(str);
                    break;
                case DataType.AppString:
                    if (str == "" || str == null)
                        str = "&nbsp;";

                    if (attr.UIHeight != 0)
                    {
                        this.Pub1.AddTDDoc(str, str);
                    }
                    else
                    {
                        if (attr.Key.IndexOf("ail") == -1)
                            this.Pub1.AddTD(str);
                        else
                            this.Pub1.AddTD("<a href=\"javascript:mailto:" + str + "\"' >" + str + "</a>");
                    }
                    break;
                case DataType.AppBoolean:
                    if (str == "1")
                        this.Pub1.AddTD("ÊÇ");
                    else
                        this.Pub1.AddTD("·ñ");
                    break;
                case DataType.AppFloat:
                case DataType.AppInt:
                case DataType.AppRate:
                case DataType.AppDouble:
                    foreach (BP.Sys.Xml.Search pe in cfgs)
                    {
                        if (pe.Attr == attr.Key)
                        {
                            cfgurl = pe.URL;
                            Attrs attrs = en.EnMap.Attrs;
                            foreach (Attr attr1 in attrs)
                                cfgurl = cfgurl.Replace("@" + attr1.Key, en.GetValStringByKey(attr1.Key));
                            break;
                        }
                    }
                    if (cfgurl == "")
                    {
                        this.Pub1.AddTDNum(str);
                    }
                    else
                    {
                        cfgurl = cfgurl.Replace("@Keys", url);
                        this.Pub1.AddTDNum("<a href=\"javascript:WinOpen('" + cfgurl + "','dtl1');\" >" + str + "</a>");
                    }
                    break;
                case DataType.AppMoney:
                    cfgurl = "";
                    foreach (BP.Sys.Xml.Search pe in cfgs)
                    {
                        if (pe.Attr == attr.Key)
                        {
                            cfgurl = pe.URL;
                            Attrs attrs = en.EnMap.Attrs;
                            foreach (Attr attr2 in attrs)
                                cfgurl = cfgurl.Replace("@" + attr2.Key, en.GetValStringByKey(attr2.Key));
                            break;
                        }
                    }
                    if (cfgurl == "")
                    {
                        this.Pub1.AddTDNum(decimal.Parse(str).ToString("0.00"));
                    }
                    else
                    {
                        cfgurl = cfgurl.Replace("@Keys", url);

                        this.Pub1.AddTDNum("<a href=\"javascript:WinOpen('" + cfgurl + "','dtl1');\" >" + decimal.Parse(str).ToString("0.00") + "</a>");
                    }
                    break;
                default:
                    throw new Exception("no this case ...");
            }
        }
    }
}
