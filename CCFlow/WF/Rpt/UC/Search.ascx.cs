using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.DA;
using BP.Web.Controls;
using BP.En;
using BP.WF;
using BP.Sys;
using BP.WF.Rpt;
using BP;

namespace CCFlow.WF.Rpt
{
    public partial class Search : BP.Web.UC.UCBase3
    {
        #region  Property .
        /// <summary>
        ///  No. Name 
        /// </summary>
        public string RptNo
        {
            get
            {
                string s = this.Request.QueryString["RptNo"];
                if (string.IsNullOrEmpty(s))
                {
                    return "ND68MyRpt";
                }
                return s;
            }
        }
        public string FK_Flow
        {
            get
            {
                string s = this.Request.QueryString["FK_Flow"];
                if (string.IsNullOrEmpty(s))
                    return "068";
                return s;
            }
        }
        public Entities _HisEns = null;
        public Entities HisEns
        {
            get
            {
                if (_HisEns == null)
                {
                    if (this.RptNo != null)
                    {
                        if (this._HisEns == null)
                            _HisEns = BP.En.ClassFactory.GetEns(this.RptNo);
                    }
                }
                return _HisEns;
            }
        }
        public MapRpt currMapRpt = null;
        #endregion  Property .

        protected void Page_Load(object sender, EventArgs e)
        {
            #region  Processing queries Permissions ,  Do not modify here ,以Search.ascx Prevail .
           // this.Page.RegisterClientScriptBlock("sss",
           //"<link href='" + BP.WF.Glo.CCFlowAppPath + "WF/Comm/Style/Table" + BP.Web.WebUser.Style + ".css' rel='stylesheet' type='text/css' />");
            currMapRpt = new MapRpt(this.RptNo,this.FK_Flow);
            bool isoi = currMapRpt.IsCanSearchRpt;
            Entity en = this.HisEns.GetNewEntity;
            Flow fl = new Flow(this.currMapRpt.FK_Flow);

            // Initialization Query toolbar .
            this.ToolBar1.InitToolbarOfMapRpt(fl, currMapRpt, this.RptNo, en, 1);
            this.ToolBar1.AddLinkBtn(BP.Web.Controls.NamesOfBtn.Export);

            // Go increase .
            this.ToolBar1.Add("&nbsp;");
            DDL ddl = new DDL();
            ddl.ID = "GoTo";
            ddl.Items.Add(new ListItem(" Inquiry ", "Search"));
            ddl.Items.Add(new ListItem(" Advanced Search ", "SearchAdv"));
            ddl.Items.Add(new ListItem(" Subgroup analysis ", "Group"));
            ddl.Items.Add(new ListItem(" Cross report ", "D3"));
            ddl.Items.Add(new ListItem(" Comparative analysis ", "Contrast"));
            ddl.SetSelectItem(this.PageID);
            this.ToolBar1.AddDDL(ddl);
            ddl.AutoPostBack = true;
            ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged_GoTo);

            this.ToolBar1.GetLinkBtnByID(NamesOfBtn.Search).Click += new System.EventHandler(this.ToolBar1_ButtonClick);
            this.ToolBar1.GetLinkBtnByID(NamesOfBtn.Export).Click += new System.EventHandler(this.ToolBar1_ButtonClick);

            #endregion  Processing queries Permissions 

            // Process button .
            this.SetDGData();
        }

        void ddl_SelectedIndexChanged_GoTo(object sender, EventArgs e)
        {
            DDL ddl = sender as DDL;
            string item = ddl.SelectedItemStringVal;

            string tKey = DateTime.Now.ToString("MMddhhmmss");
            this.Response.Redirect(item + ".aspx?RptNo=" + this.RptNo + "&FK_Flow=" + this.FK_Flow+"&T="+tKey,true);
        }

        public Entities SetDGData()
        {
            return this.SetDGData(this.PageIdx);
        }

        public Entities SetDGData(int pageIdx)
        {
            #region  Perform data paging query , And bind the paging controls .

            Entities ens = this.HisEns;
            Entity en = ens.GetNewEntity;
            QueryObject qo = new QueryObject(ens);
            qo = this.ToolBar1.GetnQueryObject(ens, en);

            this.Pub2.Clear();
            this.Pub2.BindPageIdxEasyUi(qo.GetCount(),
                                        this.PageID + ".aspx?RptNo=" + this.RptNo + "&EnsName=" + this.RptNo +
                                        "&FK_Flow=" + this.FK_Flow,
                                        pageIdx,
                                        SystemConfig.PageSize);
            
            qo.DoQuery(en.PK, SystemConfig.PageSize, pageIdx);
            #endregion  Perform data paging query , And bind the paging controls .

            #region  Check that by Keyword , If it is to put the key marked in red .

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
            #endregion  Check that by Keyword , If it is to put the key marked in red .

            //  Deal with entity的GuestNo  Problem column .
          
          //  if (en.EnMap.Attrs.Contains(NDXRptBaseAttr.ex
            //foreach (Entity en in ens)
            //{
            //}

            // Binding Data .
            this.BindEns(ens, null);

            #region  Generation flip js, Temporarily use 
            //int ToPageIdx = this.PageIdx + 1;
            //int PPageIdx = this.PageIdx - 1;
            //this.UCSys1.Add("<SCRIPT language=javascript>");
            //this.UCSys1.Add("\t\n document.onkeydown = chang_page;");
            //this.UCSys1.Add("\t\n function chang_page() { ");
            //if (this.PageIdx == 1)
            //{
            //    this.UCSys1.Add("\t\n if (event.keyCode == 37 || event.keyCode == 33) alert(' The first page is already ');");
            //}
            //else
            //{
            //    this.UCSys1.Add("\t\n if (event.keyCode == 37  || event.keyCode == 38 || event.keyCode == 33) ");
            //    this.UCSys1.Add("\t\n     location='" + this.PageID + ".aspx?RptNo=" + this.RptNo + "&FK_Flow=" + this.currMapRpt.FK_Flow + "&PageIdx=" + PPageIdx + "';");
            //}

            //if (this.PageIdx == maxPageNum)
            //{
            //    this.UCSys1.Add("\t\n if (event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 34) alert(' This is the last one ');");
            //}
            //else
            //{
            //    this.UCSys1.Add("\t\n if (event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 34) ");
            //    this.UCSys1.Add("\t\n     location='" + this.PageID + ".aspx?RptNo=" + this.RptNo + "&FK_Flow=" + this.currMapRpt.FK_Flow + "&PageIdx=" + ToPageIdx + "';");
            //}

            //this.UCSys1.Add("\t\n } ");
            //this.UCSys1.Add("</SCRIPT>");
            #endregion  Generation flip js

            return ens;
        }
        private string GenerEnUrl(Entity en, Attrs attrs)
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
        /// <summary>
        ///  Binding entity set .
        /// </summary>
        /// <param name="ens"></param>
        /// <param name="ctrlId"></param>
        public void BindEns(Entities ens, string ctrlId)
        {
            #region  Definition of variables .
            MapData md = new MapData(this.RptNo);
            if (this.Page.Title == "")
                this.Page.Title = md.Name;

            this.UCSys1.Controls.Clear();
            Entity myen = ens.GetNewEntity;
            string pk = myen.PK;
            string clName = myen.ToString();
            Attrs attrs = myen.EnMap.Attrs;
            #endregion  Definition of variables .

            this.UCSys1.AddTable("class='Table' cellspacing='0' cellpadding='0' border='0' style='width:100%;line-height:22px'");

            #region   Generate a table title 
            this.UCSys1.AddTR();
            this.UCSys1.AddTDGroupTitle("style='text-align:center'","No.");
            this.UCSys1.AddTDGroupTitle(" Title ");

            foreach (Attr attr in attrs)
            {
                if (attr.IsRefAttr
                    || attr.Key == "Title" 
                    || attr.Key=="MyNum")
                    continue;

                this.UCSys1.AddTDGroupTitle(attr.Desc);
            }

            this.UCSys1.AddTREnd();
            #endregion   Generate a table title 

            #region  User interface attribute set 

            int pageidx = this.PageIdx - 1;
            int idx = SystemConfig.PageSize * pageidx;
            #endregion  User interface attribute set 

            #region  Data output .

            foreach (Entity en in ens)
            {
                #region  Output Fields .

                idx++;
                this.UCSys1.AddTR();
                this.UCSys1.AddTDIdx(idx);
                this.UCSys1.AddTD("<a href=\"javascript:WinOpen('" + BP.WF.Glo.CCFlowAppPath + "WF/WFRpt.aspx?FK_Flow=" + this.currMapRpt.FK_Flow + "&WorkID=" + en.GetValStrByKey("OID") +   "','tdr');\" >" + en.GetValByKey("Title") + "</a>");
                
                foreach (Attr attr in attrs)
                {
                    if (attr.IsRefAttr || attr.Key == "MyNum" || attr.Key == "Title")
                        continue;

                    if (attr.UIContralType == UIContralType.DDL)
                    {
                        string s = en.GetValRefTextByKey(attr.Key);
                        if (string.IsNullOrEmpty(s))
                        {
                            switch (attr.Key)
                            {
                                case "FK_NY": // 2012-01
                                    s = en.GetValStringByKey(attr.Key);
                                    break;
                                default: // In other cases , The encoded output out .
                                    s = en.GetValStringByKey(attr.Key);
                                    break;
                            }
                        }
                        this.UCSys1.AddTD(s);
                        continue;
                    }
                    
                    string str = en.GetValStrByKey(attr.Key);

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
                                this.UCSys1.AddTDDoc(str, str);
                            else
                                this.UCSys1.AddTD(str);
                            break;
                        case DataType.AppBoolean:
                            if (str == "1")
                                this.UCSys1.AddTD("Yes");
                            else
                                this.UCSys1.AddTD("No");
                            break;
                        case DataType.AppFloat:
                        case DataType.AppInt:
                        case DataType.AppRate:
                        case DataType.AppDouble:
                            this.UCSys1.AddTDNum(str);
                            break;
                        case DataType.AppMoney:
                            this.UCSys1.AddTDNum(decimal.Parse(str).ToString("0.00"));
                            break;
                        default:
                            throw new Exception("no this case ...");
                    }
                }

                this.UCSys1.AddTREnd();
                #endregion  Output Fields .
            }
            #endregion  Data output .

            #region  Calculate whether you can find the total , Mainly determines whether there is data in this type Entities中.

            bool IsHJ = false;
            foreach (Attr attr in attrs)
            {
                if (attr.MyFieldType == FieldType.RefText
                    || attr.Key == "Title"
                    || attr.Key == "MyNum")
                    continue;

                if (attr.UIVisible == false)
                    continue;

                if (attr.UIContralType == UIContralType.DDL)
                    continue;

                if (attr.Key == "OID" ||
                    attr.Key == "MID"
                    || attr.Key == "FID"
                    || attr.Key == "PWorkID"
                    || attr.Key.ToUpper() == "WORKID")
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
            #endregion  Calculate whether you can find the total , Mainly determines whether there is data in this type Entities中.

            #region   Total output .

            if (IsHJ)
            {
                this.UCSys1.Add("<TR class='Sum' >");
                this.UCSys1.AddTD();
                this.UCSys1.AddTD(" Total ");
                foreach (Attr attr in attrs)
                {
                    if (attr.Key == "MyNum")
                        continue;

                    if (attr.UIContralType == UIContralType.DDL)
                    {
                        this.UCSys1.AddTD();
                        continue;
                    }

                    if (attr.UIVisible == false)
                        continue;

                    if (attr.Key == "OID" || attr.Key == "MID" 
                        || attr.Key.ToUpper() == "WORKID" 
                        || attr.Key == "FID")
                    {
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
                /* End loop */
                this.UCSys1.AddTD();
                this.UCSys1.AddTREnd();
            }

            #endregion

            this.UCSys1.AddTableEnd();
        }

        private void ToolBar1_ButtonClick(object sender, System.EventArgs e)
        {
            try
            {
                var btn = (LinkBtn)sender;

                switch (btn.ID)
                {
                    case NamesOfBtn.Export: // Export Data .
                    case NamesOfBtn.Excel: // Export Data 
                        MapData md = new MapData(this.RptNo);
                        Entities ens = md.HisEns;
                        Entity en = ens.GetNewEntity;
                        QueryObject qo = new QueryObject(ens);
                        qo = this.ToolBar1.GetnQueryObject(ens, en);

                        DataTable dt = qo.DoQueryToTable();
                        DataTable myDT = new DataTable();
                        MapAttrs attrs = new MapAttrs(this.RptNo);

                        foreach (MapAttr attr in attrs)
                        {
                            myDT.Columns.Add(new DataColumn(attr.Name, typeof(string)));
                        }

                        foreach (DataRow dr in dt.Rows)
                        {
                            DataRow myDR = myDT.NewRow();
                            foreach (MapAttr attr in attrs)
                            {
                                myDR[attr.Name] = dr[attr.Field];
                            }
                            myDT.Rows.Add(myDR);
                        }

                        string file = "";
                        try
                        {
                            ExportDGToExcel(myDT, en.EnDesc);
                        }
                        catch (Exception ex)
                        {
                                throw new Exception(" One reason may be the data is not correctly exported : The system administrator is not properly installed Excel Package , Please inform him , Refer to the installation instructions to solve .@ System anomalies :" + ex.Message);
                        }

                        this.SetDGData();
                        return;
                    default:
                        this.PageIdx = 1;
                        this.SetDGData(1);
                        this.ToolBar1.SaveSearchState(this.RptNo, null);
                        return;
                }
            }
            catch (Exception ex)
            {
                if (!(ex is System.Threading.ThreadAbortException))
                {
                    this.ResponseWriteRedMsg(ex);
                    // Here an error 
                }
            }
        }
    }
}