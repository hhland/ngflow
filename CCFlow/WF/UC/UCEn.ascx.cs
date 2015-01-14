//===========================================================================
//  This file is used as  ASP.NET 2.0 Web  Conversion of some revision of the project .
//  Class names have been changed , And the class has been modified from a file [App_Code\Migrated\comm\uc\Stub_ucen_ascx_cs.cs] The abstract base class  
//  Inherit .
//  At runtime , This allows your  Web  Applications in other classes using the abstract base class bindings and access .
//  Code-behind page . 
//  Associated content page [comm\uc\ucen.ascx] Has also been modified , To reference the new class name .
//  For more information about this code pattern , Please refer to  http://go.microsoft.com/fwlink/?LinkId=46995
//===========================================================================
namespace CCFlow.WF.UC
{
    using System;
    using System.IO;
    using System.Data;
    using System.Drawing;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;
    using System.Web.UI;
    using BP;
    using BP.En;
    using BP.Sys;
    using BP.Sys.Xml;
    using BP.DA;
    using BP.Web;
    using BP.Web.Controls;
    using BP.WF;
    /// <summary>
    ///	UCEn  The summary .
    /// </summary>
    public partial class UCEn : BP.Web.UC.UCBase3
    {
        #region add 2010-07-24  Processing the second entity algorithm bound 

        #region add varable.
        public BP.Sys.GroupField currGF = new BP.Sys.GroupField();
        public MapDtls dtls;
        public MapFrames frames;
        public MapM2Ms m2ms;
        public FrmAttachments aths;
        private GroupFields gfs;
        public int rowIdx = 0;
        public bool isLeftNext = true;
        public int FK_Node = 0;
        public string CCFlowAppPath = BP.WF.Glo.CCFlowAppPath;


        #endregion add varable.

        public void BindColumn2(Entity en, string enName)
        {
            this.ctrlUseSta = "";
            this.EnName = enName;
            this.HisEn = en;
            this.mapData = new MapData(enName);
            currGF = new GroupField();
            MapAttrs mattrs = this.mapData.MapAttrs;
            gfs = this.mapData.GroupFields;
            dtls = this.mapData.MapDtls;
            frames = this.mapData.MapFrames;
            m2ms = this.mapData.MapM2Ms;
            aths = this.mapData.FrmAttachments;
            mes = this.mapData.MapExts;

            #region  Handling Events .
            fes = this.mapData.FrmEvents;
            if (this.IsPostBack == false)
            {
                try
                {
                    string msg = fes.DoEventNode(FrmEventList.FrmLoadBefore, en);
                    if (string.IsNullOrEmpty(msg) == false)
                        this.Alert(msg);
                }
                catch (Exception ex)
                {
                    this.Alert(ex.Message);
                    return;
                }
            }
            #endregion  Handling Events .

            // Processing defaults .
            this.DealDefVal(mattrs);
            // Processing load before filling .
            this.LoadData(mattrs, en);
            string appPath = BP.WF.Glo.CCFlowAppPath; //this.Page.Request.ApplicationPath;

            this.Add("<table width=100% >");
            foreach (BP.Sys.GroupField gf in gfs)
            {
                currGF = gf;
                this.AddTR();
                this.AddTD("colspan=2 class=GroupField valign='top' align=left ", "<div style='text-align:left; float:left'>&nbsp;" + gf.Lab + "</div><div style='text-align:right; float:right'></div>");
                this.AddTREnd();

                int idx = -1;
                isLeftNext = true;
                rowIdx = 0;

                #region  Increase the field .
                foreach (MapAttr attr in mattrs)
                {
                    #region  Exclude 
                    if (attr.GroupID != gf.OID)
                    {
                        if (gf.Idx == 0 && attr.GroupID == 0)
                        {
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (attr.HisAttr.IsRefAttr || attr.UIVisible == false)
                        continue;
                    #endregion  Exclude 

                    #region  Set up 
                    rowIdx++;
                    this.AddTR(" ID='" + currGF.Idx + "_" + rowIdx + "'");
                    if (attr.UIIsEnable == false)
                    {
                        if (this.LinkFields.Contains("," + attr.KeyOfEn + ","))
                        {
                            MapExt meLink = mes.GetEntityByKey(MapExtAttr.ExtType, MapExtXmlList.Link) as MapExt;
                            string url = meLink.Tag;
                            if (url.Contains("?") == false)
                                url = url + "?a3=2";
                            url = url + "&WebUserNo=" + WebUser.No + "&SID=" + WebUser.SID + "&EnName=" + enName;
                            if (url.Contains("@AppPath"))
                                url = url.Replace("@AppPath", "http://" + this.Request.Url.Host + CCFlowAppPath);
                            if (url.Contains("@"))
                            {
                                Attrs attrs = en.EnMap.Attrs;
                                foreach (Attr item in attrs)
                                {
                                    url = url.Replace("@" + attr.KeyOfEn, en.GetValStrByKey(attr.KeyOfEn));
                                    if (url.Contains("@") == false)
                                        break;
                                }
                            }
                            this.AddTD("<a href='" + url + "' target='" + meLink.Tag1 + "' >" + en.GetValByKey(attr.KeyOfEn) + "</a>");
                            this.AddTREnd();
                            continue;
                        }
                    }
                    #endregion  Set up 

                    #region  Join Field 
                    //  Sequence number is displayed .
                    idx++;
                    if (attr.IsBigDoc && attr.UIIsLine)
                    {
                        if (attr.UIIsEnable)
                            this.Add("<TD colspan=2 height='" + attr.UIHeight.ToString() + "px'    width='100%' valign=top align=left>" + attr.Name + "<br>");
                        else
                            this.Add("<TD colspan=2 height='" + attr.UIHeight.ToString() + "px'   width='100%' valign=top class=TBReadonly>" + attr.Name + "<br>");

                        TB mytbLine = new TB();
                        if (attr.IsBigDoc)
                        {
                            mytbLine.TextMode = TextBoxMode.MultiLine;
                            mytbLine.Attributes["class"] = "TBDoc";
                        }

                        mytbLine.ID = "TB_" + attr.KeyOfEn;
                        if (attr.IsBigDoc)
                        {
                            //  mytbLine = 5;
                            // mytbLine.Columns = 30;
                        }

                        mytbLine.Attributes["style"] = "width:98%;height:100%;padding: 0px;margin: 0px;";
                        mytbLine.Text = en.GetValStrByKey(attr.KeyOfEn);
                        mytbLine.Enabled = attr.UIIsEnable;

                        this.Add(mytbLine);
                        this.AddTDEnd();
                        this.AddTREnd();
                        rowIdx++;
                        continue;
                    }

                    TB tb = new TB();
                    tb.Attributes["width"] = "100%";
                    tb.Attributes["border"] = "1px";
                    tb.Columns = 40;
                    tb.ID = "TB_" + attr.KeyOfEn;
                    Control ctl = tb;

                    #region add contrals.
                    switch (attr.LGType)
                    {
                        case FieldTypeS.Normal:
                            tb.Enabled = attr.UIIsEnable;
                            switch (attr.MyDataType)
                            {
                                case BP.DA.DataType.AppString:
                                    tb.ShowType = TBType.TB;
                                    tb.Text = en.GetValStrByKey(attr.KeyOfEn);
                                    break;
                                case BP.DA.DataType.AppDate:
                                    tb.ShowType = TBType.Date;
                                    tb.Text = en.GetValStrByKey(attr.KeyOfEn);
                                    if (attr.UIIsEnable)
                                        tb.Attributes["onfocus"] = "WdatePicker();";
                                    break;
                                case BP.DA.DataType.AppDateTime:
                                    tb.ShowType = TBType.DateTime;
                                    tb.Text = en.GetValStrByKey(attr.KeyOfEn);
                                    if (attr.UIIsEnable)
                                        tb.Attributes["onfocus"] = "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'});";
                                    break;
                                case BP.DA.DataType.AppBoolean:
                                    CheckBox cb = new CheckBox();
                                    cb.Text = attr.Name;
                                    cb.ID = "CB_" + attr.KeyOfEn;
                                    cb.Checked = attr.DefValOfBool;
                                    cb.Enabled = attr.UIIsEnable;
                                    cb.Checked = en.GetValBooleanByKey(attr.KeyOfEn);
                                    this.AddTD("colspan=2", cb);
                                    continue;
                                case BP.DA.DataType.AppDouble:
                                case BP.DA.DataType.AppFloat:
                                    tb.ShowType = TBType.Num;
                                    tb.Text = en.GetValStrByKey(attr.KeyOfEn);
                                    // Increased verification 
                                    //tb.Attributes.Add("onkeyup", @"value=value.replace(/[^-?\d+\.*\d*$]/g,'')");
                                    tb.Attributes.Add("onblur", @"value=value.replace(/[^-?\d+\.*\d*$]/g,'')");
                                    break;
                                case BP.DA.DataType.AppInt:
                                    tb.ShowType = TBType.Num;
                                    tb.Text = en.GetValStrByKey(attr.KeyOfEn);
                                    // Increased verification 
                                    //tb.Attributes.Add("onkeyup", @"value=value.replace(/[^-?\d]/g,'')");
                                    tb.Attributes.Add("onblur", @"value=value.replace(/[^-?\d]/g,'')");
                                    break;
                                case BP.DA.DataType.AppMoney:
                                case BP.DA.DataType.AppRate:
                                    tb.ShowType = TBType.Moneny;
                                    tb.Text = decimal.Parse(en.GetValStrByKey(attr.KeyOfEn)).ToString("0.00");
                                    // Increased verification 
                                    //tb.Attributes.Add("onkeyup", @"value=value.replace(/[^-?\d+\.*\d*$]/g,'')");
                                    tb.Attributes.Add("onblur", @"value=value.replace(/[^-?\d+\.*\d*$]/g,'')");
                                    break;
                                default:
                                    break;
                            }
                            switch (attr.MyDataType)
                            {
                                case BP.DA.DataType.AppString:
                                case BP.DA.DataType.AppDateTime:
                                case BP.DA.DataType.AppDate:
                                    if (tb.Enabled)
                                        tb.Attributes["class"] = "TB";
                                    else
                                        tb.Attributes["class"] = "TBReadonly";
                                    break;
                                default:
                                    if (tb.Enabled)
                                        tb.Attributes["class"] = "TBNum";
                                    else
                                        tb.Attributes["class"] = "TBNumReadonly";
                                    break;
                            }
                            break;
                        case FieldTypeS.Enum:
                            DDL ddle = new DDL();
                            ddle.ID = "DDL_" + attr.KeyOfEn;
                            ddle.BindSysEnum(attr.UIBindKey);
                            ddle.SetSelectItem(en.GetValStrByKey(attr.KeyOfEn));
                            ddle.Enabled = attr.UIIsEnable;
                            ctl = ddle;
                            break;
                        case FieldTypeS.FK:
                            DDL ddl1 = new DDL();
                            ddl1.ID = "DDL_" + attr.KeyOfEn;
                            try
                            {
                                EntitiesNoName ens = attr.HisEntitiesNoName;
                                ens.RetrieveAll();
                                ddl1.BindEntities(ens);
                                ddl1.SetSelectItem(en.GetValStrByKey(attr.KeyOfEn));
                            }
                            catch
                            {

                            }
                            ddl1.Enabled = attr.UIIsEnable;
                            ctl = ddl1;
                            break;
                        default:
                            break;
                    }
                    #endregion add contrals.

                    string desc = attr.Name.Replace(":", "");
                    desc = desc.Replace(":", "");
                    desc = desc.Replace(" ", "");

                    if (desc.Length >= 5)
                    {
                        this.Add("<TD colspan=2 class=FDesc width='100%' ><div style='float:left'>" + desc + "</div><br>");
                        this.Add(ctl);
                        this.AddTREnd();
                    }
                    else
                    {
                        this.AddTDDesc(desc);
                        this.AddTD("width='100%' class=TBReadonly", ctl);
                        this.AddTREnd();
                    }
                    #endregion  Join Field 

                }
                #endregion  Increase the field .

                //  Insert col.
                string fid = "0";
                try
                {
                    fid = en.GetValStrByKey("FID");
                }
                catch
                {
                }
                this.InsertObjects2Col(true, en.PKVal.ToString(), fid);
            }
            this.AddTableEnd();


            #region  Deal with iFrom  Adaptive problem .
            string js = "\t\n<script type='text/javascript' >";
            foreach (MapDtl dtl in dtls)
            {
                if (dtl.IsView == false)
                    continue;

                js += "\t\n window.setInterval(\"ReinitIframe('F" + dtl.No + "','TD" + dtl.No + "')\", 200);";
            }
            foreach (MapFrame fr in frames)
            {
                //  if (fr.IsAutoSize)
                js += "\t\n window.setInterval(\"ReinitIframe('F" + fr.NoOfObj + "','TD" + fr.NoOfObj + "')\", 200);";
            }
            foreach (MapM2M m2m in m2ms)
            {
                //  if (m2m.ShowWay == FrmShowWay.FrmAutoSize)
                js += "\t\n window.setInterval(\"ReinitIframe('F" + m2m.NoOfObj + "','TD" + m2m.NoOfObj + "')\", 200);";
            }
            foreach (FrmAttachment ath in aths)
            {
                // if (ath.IsAutoSize)
                js += "\t\n window.setInterval(\"ReinitIframe('F" + ath.MyPK + "','TD" + ath.MyPK + "')\", 200);";
            }
            js += "\t\n</script>";
            this.Add(js);
            #endregion  Deal with iFrom  Adaptive problem .

            //  Processing extensions .
            this.AfterBindEn_DealMapExt(enName, mattrs, en);
            if (this.IsReadonly == false)
            {
                #region  Deal with iFrom SaveDtlData.
                js = "\t\n<script type='text/javascript' >";
                js += "\t\n function SaveDtl(dtl) { ";
                //    js += "\t\n    GenerPageKVs(); // Calls generated kvs ";
                js += "\t\n document.getElementById('F' + dtl ).contentWindow.SaveDtlData(); ";
                js += "\t\n } ";
                js += "\t\n</script>";
                this.Add(js);
                #endregion  Deal with iFrom SaveDtlData.

                #region  Deal with iFrom  SaveM2M Save
                js = "\t\n<script type='text/javascript' >";
                js += "\t\n function SaveM2M(dtl) { ";
                js += "\t\n document.getElementById('F' + dtl ).contentWindow.SaveM2M();";
                js += "\t\n } ";
                js += "\t\n</script>";
                this.Add(js);
                #endregion  Deal with iFrom  SaveM2M Save.
            }
        }
        public string _tempAddDtls = "";
        public void InsertObjects2Col(bool isJudgeRowIdx, string pk, string fid)
        {
            #region  From Table 
            foreach (MapDtl dtl in dtls)
            {
                if (dtl.IsView == false)
                    continue;

                if (_tempAddDtls.Contains(dtl.No))
                    continue;

                //if (dtl.IsUse)
                //    continue;

                //if (isJudgeRowIdx)
                //{
                //    if (dtl.RowIdx != rowIdx)
                //        continue;
                //}

                if (dtl.GroupID != currGF.OID)
                    continue;

                if (dtl.GroupID == 0 && rowIdx == 0)
                {
                    dtl.GroupID = currGF.OID;
                    dtl.RowIdx = 0;
                    dtl.Update();
                }

                dtl.IsUse = true;
                rowIdx++;

                this.AddTR(" ID='" + currGF.Idx + "_" + rowIdx + "' ");
                this.Add("<TD colspan=2 ID='TD" + dtl.No + "' height='100px' width='100%' style='align:left'>");
                string src = "";
                try
                {
                    src = CCFlowAppPath + "WF/CCForm/Dtl.aspx?EnsName=" + dtl.No + "&RefPKVal=" + this.HisEn.PKVal + "&FID=" + this.HisEn.GetValStringByKey("FID") + "&IsWap=0&FK_Node=" + dtl.FK_MapData.Replace("ND", "");
                }
                catch
                {
                    src = CCFlowAppPath + "WF/CCForm/Dtl.aspx?EnsName=" + dtl.No + "&RefPKVal=" + this.HisEn.PKVal + "&IsWap=0&FK_Node=" + dtl.FK_MapData.Replace("ND", "");
                }

                if (this.IsReadonly || dtl.IsReadonly)
                    this.Add("<iframe ID='F" + dtl.No + "'  src='" + src +
                             "&IsReadonly=1' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='100%' height='100px' />");
                else
                {
                    AddLoadFunction(dtl.No, "blur", "SaveDtl");
                    //this.Add("<iframe ID='F" + dtl.No + "'   Onblur=\"SaveDtl('" + dtl.No + "');\"  src='" + src + "' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='100%' height='100px' />");

                    this.Add("<iframe ID='F" + dtl.No + "'  onload='" + dtl.No + "load();'    src='" + src + "' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='100%' height='100px' />");

                }


                this.AddTDEnd();
                this.AddTREnd();
                _tempAddDtls += dtl.No;

                //  Use the following Link  The program .
                //// myidx++;
                //this.AddTR(" ID='" + currGF.Idx + "_" + rowIdx + "' ");
                //string src = "";
                //try
                //{
                //    src = "/WF/CCForm/Dtl.aspx?EnsName=" + dtl.No + "&RefPKVal=" + this.HisEn.PKVal + "&FID=" + this.HisEn.GetValStringByKey("FID") + "&IsWap=1&FK_Node=" + dtl.FK_MapData.Replace("ND", "");
                //}
                //catch
                //{
                //    src = "/WF/CCForm/Dtl.aspx?EnsName=" + dtl.No + "&RefPKVal=" + this.HisEn.PKVal + "&IsWap=1&FK_Node=" + dtl.FK_MapData.Replace("ND", "");
                //}
                //_tempAddDtls += dtl.No;
                //this.Add("<TD colspan=2 class=FDesc ID='TD" + dtl.No + "'><a href='" + src + "'>" + dtl.Name + "</a></TD>");
                //// this.Add("<iframe ID='F" + dtl.No + "' frameborder=0 Onblur=\"SaveDtl('" + dtl.No + "');\" style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' src='" + src + "' height='10px' scrolling=no  /></iframe>");
                ////this.AddTDEnd();
                //this.AddTREnd();
            }
            #endregion  From Table 

            #region  Frame Table 
            foreach (MapFrame fram in frames)
            {
                if (fram.IsUse)
                    continue;

                if (isJudgeRowIdx)
                {
                    if (fram.RowIdx != rowIdx)
                        continue;
                }

                if (fram.GroupID == 0 && rowIdx == 0)
                {
                    fram.GroupID = currGF.OID;
                    fram.RowIdx = 0;
                    fram.Update();
                }
                else if (fram.GroupID == currGF.OID)
                {

                }
                else
                {
                    continue;
                }
                fram.IsUse = true;
                rowIdx++;
                this.AddTR(" ID='" + currGF.Idx + "_" + rowIdx + "' ");
                string src = fram.URL;

                if (src.Contains("?"))
                    src += "&Table=" + fram.FK_MapData + "&WorkID=" + pk + "&FID=" + fid;
                else
                    src += "?Table=" + fram.FK_MapData + "&WorkID=" + pk + "&FID=" + fid;
                this.Add("<TD colspan=2 class=FDesc ID='TD" + fram.NoOfObj + "'><a href='" + src + "'>" + fram.Name + "</a></TD>");
                this.AddTREnd();
            }
            #endregion  From Table 

            #region  Accessory 
            foreach (FrmAttachment ath in aths)
            {
                if (ath.IsUse)
                    continue;
                if (isJudgeRowIdx)
                {
                    if (ath.RowIdx != rowIdx)
                        continue;
                }

                if (ath.GroupID == 0 && rowIdx == 0)
                {
                    ath.GroupID = currGF.OID;
                    ath.RowIdx = 0;
                    ath.Update();
                }
                else if (ath.GroupID == currGF.OID)
                {

                }
                else
                {
                    continue;
                }
                ath.IsUse = true;
                rowIdx++;

                string src = CCFlowAppPath + "WF/CCForm/AttachmentUpload.aspx?IsWap=1&PKVal=" + this.HisEn.PKVal + "&NoOfObj=" + ath.NoOfObj + "&FK_MapData=" + EnsName + "&FK_FrmAttachment=" + ath.MyPK + this.RequestParas;
                this.AddTR(" ID='" + currGF.Idx + "_" + rowIdx + "' ");
                this.Add("<TD colspan=2 class=FDesc ID='TD" + ath.NoOfObj + "'><a href='" + src + "'>" + ath.Name + "</a></TD>");
                this.AddTREnd();
            }
            #endregion  Accessory 

            #region  Many relationship 
            foreach (MapM2M m2m in m2ms)
            {
                if (m2m.IsUse)
                    continue;

                if (isJudgeRowIdx)
                {
                    if (m2m.RowIdx != rowIdx)
                        continue;
                }

                if (m2m.GroupID == 0 && rowIdx == 0)
                {
                    m2m.GroupID = currGF.OID;
                    m2m.RowIdx = 0;
                    m2m.Update();
                }
                else if (m2m.GroupID == currGF.OID)
                {

                }
                else
                {
                    continue;
                }
                m2m.IsUse = true;
                rowIdx++;
                this.AddTR(" ID='" + currGF.Idx + "_" + rowIdx + "' ");
                if (m2m.ShowWay == FrmShowWay.FrmAutoSize)
                    this.Add("<TD colspan=4 ID='TD" + m2m.NoOfObj + "' height='50px' width='100%'  >");
                else
                    this.Add("<TD colspan=4 ID='TD" + m2m.NoOfObj + "' height='" + m2m.H + "' width='" + m2m.W + "'  >");

                string src = "";
                if (m2m.HisM2MType == M2MType.M2M)
                    src = CCFlowAppPath + "WF/CCForm/M2M.aspx?NoOfObj=" + m2m.NoOfObj;
                else
                    src = CCFlowAppPath + "WF/CCForm/M2MM.aspx?NoOfObj=" + m2m.NoOfObj;

                string paras = this.RequestParas;

                if (paras.Contains("FID=") == false)
                    paras += "&FID=" + this.HisEn.GetValStrByKey("FID");

                if (paras.Contains("OID=") == false)
                    paras += "&OID=" + this.HisEn.GetValStrByKey("OID");

                src += "&r=q" + paras;

                if (src.Contains("FK_MapData") == false)
                    src += "&FK_MapData=" + m2m.FK_MapData;

                switch (m2m.ShowWay)
                {
                    case FrmShowWay.FrmAutoSize:
                        if (m2m.IsEdit)
                        {
                            AddLoadFunction(m2m.NoOfObj, "blur", "SaveM2M");
                            // this.Add("<iframe ID='F" + m2m.NoOfObj + "'   Onblur=\"SaveM2M('" + m2m.NoOfObj + "');\"  src='" + src + "' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='100%' height='10px' scrolling=no /></iframe>");

                            this.Add("<iframe ID='F" + m2m.NoOfObj + "' onload='" + m2m.NoOfObj + "load();'    src='" + src + "' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='100%' height='10px' scrolling=no /></iframe>");

                        }
                        else
                            this.Add("<iframe ID='F" + m2m.NoOfObj + "'   src='" + src + "' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='100%' height='10px' scrolling=no /></iframe>");
                        break;
                    case FrmShowWay.FrmSpecSize:
                        if (m2m.IsEdit)
                        {
                            // this.Add("<iframe ID='F" + m2m.NoOfObj + "'   Onblur=\"SaveM2M('" + m2m.NoOfObj + "');\"  src='" + src + "' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='" + m2m.W + "' height='" + m2m.H + "' scrolling=auto /></iframe>");
                            AddLoadFunction(m2m.NoOfObj, "blur", "SaveM2M");

                            this.Add("<iframe ID='F" + m2m.NoOfObj + "'   onload='" + m2m.NoOfObj + "load();' src='" + src + "' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='" + m2m.W + "' height='" + m2m.H + "' scrolling=auto /></iframe>");

                        }
                        else
                            this.Add("<iframe ID='F" + m2m.NoOfObj + "'    src='" + src + "' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='" + m2m.W + "' height='" + m2m.H + "' scrolling=auto /></iframe>");
                        break;
                    case FrmShowWay.Hidden:
                        break;
                    case FrmShowWay.WinOpen:
                        this.Add("<a href=\"javascript:WinOpen('" + src + "&IsOpen=1','" + m2m.W + "','" + m2m.H + "');\"  />" + m2m.Name + "</a>");
                        break;
                    default:
                        break;
                }
            }
            #endregion  Many relationship 
        }
        public MapExts mes = null;
        public bool IsLoadData = false;
        public void LoadData(MapAttrs mattrs, Entity en)
        {
            this.LinkFields = "";
            if (mes.Count == 0)
                return;
            foreach (MapExt myitem in mes)
            {
                if (myitem.ExtType == MapExtXmlList.Link)
                    this.LinkFields += "," + myitem.AttrOfOper + ",";
            }

            if (this.IsLoadData == false)
                return;

            MapExt item = mes.GetEntityByKey(MapExtAttr.ExtType, MapExtXmlList.PageLoadFull) as MapExt;
            if (item == null)
                return;

            DataTable dt = null;
            string sql = item.Tag;
            if (string.IsNullOrEmpty(sql) == false)
            {
                /*  If you have filled the main table sql  */
                #region  Deal with sql Variable 
                sql = sql.Replace("@WebUser.No", BP.Web.WebUser.No);
                sql = sql.Replace("@WebUser.Name", BP.Web.WebUser.Name);
                sql = sql.Replace("@WebUser.FK_Dept", BP.Web.WebUser.FK_Dept);
                sql = sql.Replace("@WebUser.FK_DeptName", BP.Web.WebUser.FK_DeptName);
                foreach (MapAttr attr in mattrs)
                {
                    if (sql.Contains("@"))
                        sql = sql.Replace("@" + attr.KeyOfEn, en.GetValStrByKey(attr.KeyOfEn));
                    else
                        break;
                }
                #endregion  Deal with sql Variable 

                if (string.IsNullOrEmpty(sql) == false)
                {
                    if (sql.Contains("@"))
                        throw new Exception(" Set sql There are errors that may have replaced the variable :" + sql);
                    dt = DBAccess.RunSQLReturnTable(sql);
                    if (dt.Rows.Count == 1)
                    {
                        DataRow dr = dt.Rows[0];
                        foreach (DataColumn dc in dt.Columns)
                        {
                            en.SetValByKey(dc.ColumnName, dr[dc.ColumnName].ToString());
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(item.Tag1)
                || item.Tag1.Length < 15)
                return;

            //  Filled from the table .
            foreach (MapDtl dtl in dtls)
            {
                string[] sqls = item.Tag1.Split('*');
                foreach (string mysql in sqls)
                {
                    if (string.IsNullOrEmpty(mysql))
                        continue;

                    if (mysql.Contains(dtl.No + "=") == false)
                        continue;

                    #region  Deal with sql.
                    sql = mysql;
                    sql = sql.Replace(dtl.No + "=", "");
                    sql = sql.Replace("@WebUser.No", BP.Web.WebUser.No);
                    sql = sql.Replace("@WebUser.Name", BP.Web.WebUser.Name);
                    sql = sql.Replace("@WebUser.FK_Dept", BP.Web.WebUser.FK_Dept);
                    sql = sql.Replace("@WebUser.FK_DeptName", BP.Web.WebUser.FK_DeptName);
                    foreach (MapAttr attr in mattrs)
                    {
                        if (sql.Contains("@"))
                            sql = sql.Replace("@" + attr.KeyOfEn, en.GetValStrByKey(attr.KeyOfEn));
                        else
                            break;
                    }
                    #endregion  Deal with sql.

                    if (string.IsNullOrEmpty(sql))
                        continue;

                    if (sql.Contains("@"))
                        throw new Exception(" Set sql There are errors that may have replaced the variable :" + sql);

                    GEDtls gedtls = new GEDtls(dtl.No);
                    gedtls.Delete(GEDtlAttr.RefPK, en.PKVal);

                    dt = DBAccess.RunSQLReturnTable(sql);
                    foreach (DataRow dr in dt.Rows)
                    {
                        GEDtl gedtl = gedtls.GetNewEntity as GEDtl;
                        foreach (DataColumn dc in dt.Columns)
                        {
                            gedtl.SetValByKey(dc.ColumnName, dr[dc.ColumnName].ToString());
                        }
                        gedtl.RefPK = en.PKVal.ToString();
                        gedtl.RDT = DataType.CurrentDataTime;
                        gedtl.Rec = WebUser.No;
                        gedtl.Insert();
                    }
                }
            }
        }
        public string ctrlUseSta = "";
        public int idx = 0;
        public void BindColumn4(Entity en, string enName)
        {
            this.ctrlUseSta = "";
            this.EnName = enName;
            this.HisEn = en;
            this.mapData = new MapData(enName);
            currGF = new GroupField();
            MapAttrs mattrs = this.mapData.MapAttrs;
            gfs = this.mapData.GroupFields;
            dtls = this.mapData.MapDtls;
            frames = this.mapData.MapFrames;
            m2ms = this.mapData.MapM2Ms;
            aths = this.mapData.FrmAttachments;
            mes = this.mapData.MapExts;

            #region  Handling Events .
            fes = this.mapData.FrmEvents;
            if (this.IsPostBack == false)
            {
                try
                {
                    string msg = fes.DoEventNode(FrmEventList.FrmLoadBefore, en);
                    if (string.IsNullOrEmpty(msg) == false)
                        this.Alert(msg);
                }
                catch (Exception ex)
                {
                    //string msg = ex.Message;
                    this.Alert(ex.Message);
                    return;
                }
            }
            #endregion  Handling Events .

            // Processing defaults .
            this.DealDefVal(mattrs);
            // Processing load before filling .
            this.LoadData(mattrs, en);
            string appPath = CCFlowAppPath; //this.Page.Request.ApplicationPath;

            #region  Calculated from the width of the columns .
            int labCol = 80;
            int ctrlCol = 260;
            int width = (labCol + ctrlCol) * mapData.TableCol / 2;
            #endregion  Calculated from the width of the columns .

            #region  Generate header .
            this.Add("\t\n<Table style='width:" + width + "px;' align=left>");

            this.AddTREnd();
            #endregion  Generate header .

            foreach (GroupField gf in gfs)
            {
                currGF = gf;
                this.AddTR();
                if (gfs.Count == 1)
                    this.AddTD("colspan=" + this.mapData.TableCol + " style='width:" + width + "px' class=GroupField valign='top' align=left ", "<div style='text-align:left; float:left'>&nbsp;" + gf.Lab + "</div><div style='text-align:right; float:right'></div>");
                else
                    this.AddTD("colspan=" + this.mapData.TableCol + " style='width:" + width + "px' class=GroupField valign='top' align=left  onclick=\"GroupBarClick('" + gf.Idx + "')\"  ", "<div style='text-align:left; float:left'>&nbsp;<img src='" + CCFlowAppPath + "WF/Style/Min.gif' alert='Min' id='Img" + gf.Idx + "' border=0 />&nbsp;" + gf.Lab + "</div><div style='text-align:right; float:right'></div>");
                this.AddTREnd();

                bool isHaveH = false;
                idx = -1;

                rowIdx = 0;
                int colSpan = this.mapData.TableCol;  //  Definition colspan Width .
                this.AddTR();
                for (int i = 0; i < mattrs.Count; i++)
                {
                    MapAttr attr = mattrs[i] as MapAttr;

                    #region  Filter fields are not displayed .
                    if (attr.GroupID != gf.OID)
                    {
                        if (gf.Idx == 0 && attr.GroupID == 0)
                        {
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (attr.HisAttr.IsRefAttr || attr.UIVisible == false)
                        continue;

                    if (colSpan == 0)
                        this.InsertObjects(true);
                    #endregion  Filter fields are not displayed .

                    #region  Add blank columns .
                    if (colSpan <= 0)
                    {
                        /* If the column has been used .*/
                        this.AddTREnd();
                        colSpan = this.mapData.TableCol; // Add columns .
                        rowIdx++;
                    }
                    #endregion  Add blank columns .

                    #region  Output processing large blocks of text .
                    //  Sequence number is displayed .
                    idx++;
                    if (attr.IsBigDoc && (attr.ColSpan == this.mapData.TableCol || attr.ColSpan == 0))
                    {
                        int h = attr.UIHeightInt + 20;
                        if (attr.UIIsEnable)
                            this.Add("<TD height='" + h.ToString() + "px'  colspan=" + this.mapData.TableCol + " width='100%' valign=top align=left>");
                        else
                            this.Add("<TD height='" + h.ToString() + "px'  colspan=" + this.mapData.TableCol + " width='100%' valign=top class=TBReadonly>");

                        this.Add("<div style='font-size:12px;color:black;' >");
                        Label lab = new Label();
                        lab.ID = "Lab" + attr.KeyOfEn;
                        lab.Text = attr.Name;
                        this.Add(lab);
                        this.Add("</div>");
                        if (attr.TBModel == 2)
                        {
                            // Rich text output .
                            this.AddRichTextBox(en, attr);
                        }
                        else
                        {
                            TB mytbLine = new TB();
                            mytbLine.TextMode = TextBoxMode.MultiLine;
                            mytbLine.ID = "TB_" + attr.KeyOfEn;
                            mytbLine.Text = en.GetValStrByKey(attr.KeyOfEn).Replace("\\n", "\n");

                            mytbLine.Enabled = attr.UIIsEnable;
                            if (mytbLine.Enabled == false)
                                mytbLine.Attributes.Add("readonly", "true");
                            else
                                mytbLine.Attributes["class"] = "TBDoc";

                            mytbLine.Attributes["style"] = "width:98%;height:" + attr.UIHeight + "px;padding: 0px;margin: 0px;";
                            this.Add(mytbLine);

                            if (mytbLine.Enabled)
                            {
                                string ctlID = mytbLine.ClientID;
                                Label mylab = this.GetLabelByID("Lab" + attr.KeyOfEn);
                                mylab.Text = "<a href=\"javascript:TBHelp('" + ctlID + "','" + appPath + "','" + enName + "','" + attr.KeyOfEn + "')\">" + attr.Name + "</a>";
                            }
                        }

                        this.AddTDEnd();
                        this.AddTREnd();
                        rowIdx++;
                        isLeftNext = true;
                        continue;
                    }

                    if (attr.IsBigDoc)
                    {
                        if (colSpan == this.mapData.TableCol)
                        {
                            /* Already filled up */
                            this.AddTR(" ID='" + currGF.Idx + "_" + rowIdx + "' ");
                            colSpan = colSpan - attr.ColSpan; //  Minus the already occupied col.
                        }

                        this.Add("<TD class=FDesc colspan=" + attr.ColSpan + " height='" + attr.UIHeight.ToString() + "px' >");
                        this.Add(attr.Name);
                        TB mytbLine = new TB();
                        mytbLine.ID = "TB_" + attr.KeyOfEn;
                        mytbLine.TextMode = TextBoxMode.MultiLine;
                        mytbLine.Attributes["class"] = "TBDoc";
                        mytbLine.Text = en.GetValStrByKey(attr.KeyOfEn);
                        if (mytbLine.Enabled == false)
                        {
                            mytbLine.Attributes["class"] = "TBReadonly";
                            mytbLine.Attributes.Add("readonly", "true");
                        }
                        mytbLine.Attributes["style"] = "width:98%;height:100%;padding: 0px;margin: 0px;";
                        this.Add(mytbLine);
                        this.AddTDEnd();
                        continue;
                    }
                    #endregion  Chunks of text output .

                    #region  Handle hyperlinks 
                    if (attr.UIIsEnable == false)
                    {
                        /*  Determine whether there is a hidden hyperlink field . */
                        if (this.LinkFields.Contains("," + attr.KeyOfEn + ","))
                        {
                            MapExt meLink = mes.GetEntityByKey(MapExtAttr.ExtType, MapExtXmlList.Link) as MapExt;
                            string url = meLink.Tag;
                            if (url.Contains("?") == false)
                                url = url + "?a3=2";
                            url = url + "&WebUserNo=" + WebUser.No + "&SID=" + WebUser.SID + "&EnName=" + enName;
                            if (url.Contains("@AppPath"))
                                url = url.Replace("@AppPath", "http://" + this.Request.Url.Host + CCFlowAppPath);
                            if (url.Contains("@"))
                            {
                                Attrs attrs = en.EnMap.Attrs;
                                foreach (Attr item in attrs)
                                {
                                    url = url.Replace("@" + attr.KeyOfEn, en.GetValStrByKey(attr.KeyOfEn));
                                    if (url.Contains("@") == false)
                                        break;
                                }
                            }
                            this.AddTD("colspan=" + colSpan, "<a href='" + url + "' target='" + meLink.Tag1 + "' >" + en.GetValByKey(attr.KeyOfEn) + "</a>");
                            continue;
                        }
                    }
                    #endregion  Handle hyperlinks 

                    #region   First determine whether the remaining cells of the current needs of the current control .
                    if (attr.ColSpan + 1 > mapData.TableCol)
                        attr.ColSpan = this.mapData.TableCol - 1; // If you set the 

                    if (colSpan < attr.ColSpan + 1 || colSpan == 1 || colSpan == 0)
                    {
                        /* If the remaining columns can not meet the current cell , To supplement its , Wrap it .*/
                        if (colSpan != 0)
                            this.AddTD("colspan=" + colSpan, "");
                        this.AddTREnd();

                        colSpan = mapData.TableCol;
                        this.AddTR();
                    }
                    #endregion   First determine whether the remaining cells of the current needs of the current control .

                    #region  The other is to add a column controls described in a column field .
                    TB tb = new TB();
                    tb.ID = "TB_" + attr.KeyOfEn;
                    tb.Enabled = attr.UIIsEnable;
                    colSpan = colSpan - 1 - attr.ColSpan; //  First subtract the current placeholder .
                    switch (attr.LGType)
                    {
                        case FieldTypeS.Normal:
                            switch (attr.MyDataType)
                            {
                                case BP.DA.DataType.AppString:
                                    this.AddTDDesc(attr.Name);
                                    if (attr.IsSigan)
                                    {
                                        string v = en.GetValStrByKey(attr.KeyOfEn);
                                        if (v.Length == 0)
                                            this.AddTD("colspan=" + attr.ColSpan, "<img src='" + CCFlowAppPath + "DataUser/Siganture/" + WebUser.No + ".jpg' border=0 onerror=\"this.src='" + CCFlowAppPath + "DataUser/Siganture/UnName.jpg'\"/>");
                                        else
                                            this.AddTD("colspan=" + attr.ColSpan, "<img src='" + CCFlowAppPath + "DataUser/Siganture/" + v + ".jpg' border=0 onerror=\"this.src='" + CCFlowAppPath + "DataUser/Siganture/UnName.jpg'\"/>");
                                    }
                                    else
                                    {
                                        tb.ShowType = TBType.TB;
                                        tb.Text = en.GetValStrByKey(attr.KeyOfEn);
                                        tb.Attributes["width"] = "100%";
                                        this.AddTD("colspan=" + attr.ColSpan, tb);
                                    }
                                    break;
                                case BP.DA.DataType.AppDate:
                                    this.AddTDDesc(attr.Name);
                                    tb.ShowType = TBType.Date;
                                    tb.Text = en.GetValStrByKey(attr.KeyOfEn);
                                    if (attr.UIIsEnable)
                                        tb.Attributes["onfocus"] = "WdatePicker();";

                                    this.AddTD("colspan=" + attr.ColSpan, tb);
                                    break;
                                case BP.DA.DataType.AppDateTime:
                                    this.AddTDDesc(attr.Name);
                                    tb.ShowType = TBType.DateTime;
                                    tb.Text = en.GetValStrByKey(attr.KeyOfEn);
                                    if (attr.UIIsEnable)
                                        tb.Attributes["onfocus"] = "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'});";

                                    this.AddTD("colspan=" + attr.ColSpan, tb);
                                    break;
                                case BP.DA.DataType.AppBoolean:
                                    this.AddTDDesc("");
                                    CheckBox cb = new CheckBox();
                                    cb.Text = attr.Name;
                                    cb.ID = "CB_" + attr.KeyOfEn;
                                    cb.Checked = attr.DefValOfBool;
                                    cb.Enabled = attr.UIIsEnable;
                                    cb.Checked = en.GetValBooleanByKey(attr.KeyOfEn);
                                    this.AddTD("colspan=" + attr.ColSpan, cb);
                                    break;
                                case BP.DA.DataType.AppDouble:
                                case BP.DA.DataType.AppFloat:
                                    // Increased verification 
                                    //tb.Attributes.Add("onkeyup", @"value=value.replace(/[^-?\d+\.*\d*$]/g,'')");
                                    tb.Attributes.Add("onblur", @"value=value.replace(/[^-?\d+\.*\d*$]/g,'')");
                                    this.AddTDDesc(attr.Name);
                                    tb.ShowType = TBType.Num;
                                    tb.Text = en.GetValStrByKey(attr.KeyOfEn);
                                    this.AddTD("colspan=" + attr.ColSpan, tb);
                                    break;
                                case BP.DA.DataType.AppInt:
                                    this.AddTDDesc(attr.Name);
                                    tb.ShowType = TBType.Num;
                                    // Increased verification 
                                    //tb.Attributes.Add("onkeyup", @"value=value.replace(/[^-?\d]/g,'')");
                                    tb.Attributes.Add("onblur", @"value=value.replace(/[^-?\d]/g,'')");
                                    tb.Text = en.GetValStrByKey(attr.KeyOfEn);
                                    this.AddTD("colspan=" + attr.ColSpan, tb);
                                    break;
                                case BP.DA.DataType.AppMoney:
                                    this.AddTDDesc(attr.Name);
                                    tb.ShowType = TBType.Moneny;

                                    if (SystemConfig.AppSettings["IsEnableNull"] == "1")
                                    {
                                        decimal v = en.GetValMoneyByKey(attr.KeyOfEn);
                                        if (v == 567567567)
                                            tb.Text = "";
                                        else
                                            tb.Text = v.ToString("0.00");
                                    }
                                    else
                                        tb.Text = en.GetValMoneyByKey(attr.KeyOfEn).ToString("0.00");

                                    //tb.Text = en.GetValMoneyByKey(attr.KeyOfEn).ToString("0.00");

                                    // Increased verification 
                                    //tb.Attributes.Add("onkeyup", @"value=value.replace(/[^-?\d+\.*\d*$]/g,'')");
                                    tb.Attributes.Add("onblur", @"value=value.replace(/[^-?\d+\.*\d*$]/g,'')");
                                    this.AddTD("colspan=" + attr.ColSpan, tb);
                                    break;
                                case BP.DA.DataType.AppRate:
                                    this.AddTDDesc(attr.Name);
                                    tb.ShowType = TBType.Moneny;
                                    tb.Text = en.GetValMoneyByKey(attr.KeyOfEn).ToString("0.00");
                                    // Increased verification 
                                    //tb.Attributes.Add("onkeyup", @"value=value.replace(/[^-?\d+\.*\d*$]/g,'')");
                                    tb.Attributes.Add("onblur", @"value=value.replace(/[^-?\d+\.*\d*$]/g,'')");
                                    this.AddTD("colspan=" + attr.ColSpan, tb);
                                    break;
                                default:
                                    break;
                            }
                            // tb.Attributes["width"] = "100%";
                            switch (attr.MyDataType)
                            {
                                case BP.DA.DataType.AppString:
                                case BP.DA.DataType.AppDateTime:
                                case BP.DA.DataType.AppDate:
                                    if (tb.Enabled)
                                    {
                                        tb.MaxLength = attr.MaxLen;
                                    }
                                    else
                                    {
                                        tb.Attributes["class"] = "TBReadonly";
                                    }
                                    break;
                                default:
                                    if (tb.Enabled)
                                        tb.Attributes["class"] = "TBNum";
                                    else
                                        tb.Attributes["class"] = "TBNumReadonly";
                                    break;
                            }
                            break;
                        case FieldTypeS.Enum:
                            if (attr.UIContralType == UIContralType.DDL)
                            {
                                this.AddTDDesc(attr.Name);
                                DDL ddle = new DDL();
                                ddle.ID = "DDL_" + attr.KeyOfEn;
                                ddle.BindSysEnum(attr.UIBindKey);
                                ddle.SetSelectItem(en.GetValStrByKey(attr.KeyOfEn));
                                ddle.Enabled = attr.UIIsEnable;
                                this.AddTD("colspan=" + attr.ColSpan, ddle);
                            }
                            else
                            {
                                this.AddTDDesc(attr.Name);
                                this.Add("<TD class=TD colspan='" + attr.ColSpan + "'>");
                                SysEnums ses = new SysEnums(attr.UIBindKey);
                                foreach (SysEnum item in ses)
                                {
                                    RadioButton rb = new RadioButton();
                                    rb.ID = "RB_" + attr.KeyOfEn + "_" + item.IntKey;
                                    rb.Text = item.Lab;
                                    if (item.IntKey == en.GetValIntByKey(attr.KeyOfEn))
                                        rb.Checked = true;
                                    else
                                        rb.Checked = false;
                                    rb.GroupName = attr.KeyOfEn;
                                    this.Add(rb);
                                }
                                this.AddTDEnd();
                            }
                            break;
                        case FieldTypeS.FK:
                            this.AddTDDesc(attr.Name);
                            DDL ddl1 = new DDL();
                            ddl1.ID = "DDL_" + attr.KeyOfEn;
                            try
                            {
                                EntitiesNoName ens = attr.HisEntitiesNoName;
                                ens.RetrieveAll();
                                ddl1.BindEntities(ens);
                                ddl1.SetSelectItem(en.GetValStrByKey(attr.KeyOfEn));
                            }
                            catch
                            {
                            }
                            ddl1.Enabled = attr.UIIsEnable;
                            this.AddTD("colspan=" + attr.ColSpan, ddl1);
                            break;
                        default:
                            break;
                    }
                    #endregion  The other is to add a column controls described in a column field 

                } //  End field collection cycle .

                //  After the packet processing it ,  First determine whether the remaining cells of the current needs of the current control .
                if (colSpan != this.mapData.TableCol)
                {
                    /*  If the remaining columns can not meet the current cell , To supplement its , Wrap it .*/
                    if (colSpan != 0)
                        this.AddTD("colspan=" + colSpan, "");

                    this.AddTREnd();
                    colSpan = mapData.TableCol;
                }
                this.InsertObjects(false);
            } //  End packet cycle .


            #region  Audit Components 
            FrmWorkCheck fwc = new FrmWorkCheck(enName);
            if (fwc.HisFrmWorkCheckSta != FrmWorkCheckSta.Disable)
            {
                rowIdx++;

                this.AddTR();
                this.AddTD("colspan=" + this.mapData.TableCol + " class=GroupField valign='top' align=left ", "<div style='text-align:left; float:left'>&nbsp; Audit information </div><div style='text-align:right; float:right'></div>");
                this.AddTREnd();

                // myidx++;
                this.AddTR(" ID='" + currGF.Idx + "_" + rowIdx + "' ");
                this.Add("<TD colspan=" + this.mapData.TableCol + " ID='TD" + enName + "' height='50px' width='100%' style='align:left'>");
                string src = CCFlowAppPath + "WF/WorkOpt/WorkCheck.aspx?s=2";
                string paras = this.RequestParas;
                try
                {
                    if (paras.Contains("FID=") == false)
                        paras += "&FID=" + en.GetValStrByKey("FID");
                }
                catch
                {
                }
                if (paras.Contains("OID=") == false)
                    paras += "&OID=" + en.GetValStrByKey("OID");
                src += "&r=q" + paras;
                this.Add("<iframe ID='F33" + fwc.No + "'  src='" + src + "' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0'  width='100%'  scrolling=auto/></iframe>");
                this.AddTDEnd();
                this.AddTREnd();
            }
            #endregion  Audit Components 


            this.AddTREnd();
            this.AddTableEnd();

            #region  Deal with iFrom  Adaptive problem .
            string js = "\t\n<script type='text/javascript' >";
            foreach (MapDtl dtl in dtls)
            {
                if (dtl.IsView == false)
                    continue;

                js += "\t\n window.setInterval(\"ReinitIframe('F" + dtl.No + "','TD" + dtl.No + "')\", 200);";
            }
            foreach (MapFrame fr in frames)
            {
                //  if (fr.IsAutoSize)
                js += "\t\n window.setInterval(\"ReinitIframe('F" + fr.NoOfObj + "','TD" + fr.NoOfObj + "')\", 200);";
            }
            foreach (MapM2M m2m in m2ms)
            {
                //  if (m2m.ShowWay == FrmShowWay.FrmAutoSize)
                js += "\t\n window.setInterval(\"ReinitIframe('F" + m2m.NoOfObj + "','TD" + m2m.NoOfObj + "')\", 200);";
            }
            foreach (FrmAttachment ath in aths)
            {
                // if (ath.IsAutoSize)
                js += "\t\n window.setInterval(\"ReinitIframe('F" + ath.MyPK + "','TD" + ath.MyPK + "')\", 200);";
            }
            js += "\t\n</script>";
            this.Add(js);
            #endregion  Deal with iFrom  Adaptive problem .

            //  Processing extensions .
            this.AfterBindEn_DealMapExt(enName, mattrs, en);
            if (this.IsReadonly == false)
            {
                #region  Deal with iFrom SaveDtlData.
                js = "\t\n<script type='text/javascript' >";
                js += "\t\n function SaveDtl(dtl) { ";
                //    js += "\t\n    GenerPageKVs(); // Calls generated kvs ";
                js += "\t\n document.getElementById('F' + dtl ).contentWindow.SaveDtlData(); ";
                js += "\t\n } ";
                js += "\t\n</script>";
                this.Add(js);
                #endregion  Deal with iFrom SaveDtlData.

                #region  Deal with iFrom  SaveM2M Save
                js = "\t\n<script type='text/javascript' >";
                js += "\t\n function SaveM2M(dtl) { ";
                js += "\t\n document.getElementById('F' + dtl ).contentWindow.SaveM2M();";
                js += "\t\n } ";
                js += "\t\n</script>";
                this.Add(js);
                #endregion  Deal with iFrom  SaveM2M Save.
            }
        }
        private void AfterBindEn_DealMapExt(string enName, MapAttrs mattrs, Entity en)
        {
            #region  Handling Events .
            if (dtls.Count >= 1)
            {
                string scriptSaveDtl = "";
                scriptSaveDtl = "\t\n<script type='text/javascript' >";
                scriptSaveDtl += "\t\n function SaveDtlAll(){ ";

                foreach (MapDtl dtl in dtls)
                {
                    if (dtl.IsUpdate == true || dtl.IsInsert == true)
                    {
                        scriptSaveDtl += "\t\n try{  ";

                        if (dtl.HisDtlShowModel == DtlShowModel.Table)
                            scriptSaveDtl += "\t\n  SaveDtl('" + dtl.No + "'); ";

                        scriptSaveDtl += "\t\n } catch(e) { ";
                        scriptSaveDtl += "\t\n  alert(e.name  + e.message);  return false;";
                        scriptSaveDtl += "\t\n } ";
                    }
                }

                scriptSaveDtl += "\t\n  return true; } ";
                scriptSaveDtl += "\t\n</script>";

                this.Add(scriptSaveDtl);
            }
            else
            {
                string scriptSaveDtl = "";
                scriptSaveDtl = "\t\n<script type='text/javascript' >";
                scriptSaveDtl += "\t\n function SaveDtlAll() { ";
                scriptSaveDtl += "\t\n return true; } ";
                scriptSaveDtl += "\t\n</script>";
                this.Add(scriptSaveDtl);
            }


            fes = this.mapData.FrmEvents;
            if (this.IsPostBack == false)
            {
                try
                {
                    string msg = fes.DoEventNode(FrmEventList.FrmLoadAfter, en);
                    if (msg != null)
                        this.Alert(msg);
                }
                catch (Exception ex)
                {
                    this.Alert(" Before loading error :" + ex.Message);
                    return;
                }
            }
            #endregion  Handling Events .

            #region  Processing Extension Set 
            if (mes.Count != 0)
            {
                #region load js.
                this.Page.RegisterClientScriptBlock("s4",
              "<script language='JavaScript' src='" + CCFlowAppPath + "WF/Scripts/jquery-1.4.1.min.js' ></script>");

                this.Page.RegisterClientScriptBlock("b7",
             "<script language='JavaScript' src='" + CCFlowAppPath + "WF/CCForm/MapExt.js' defer='defer' type='text/javascript' ></script>");

                this.Page.RegisterClientScriptBlock("y7",
            "<script language='JavaScript' src='" + CCFlowAppPath + "DataUser/JSLibData/" + enName + ".js' ></script>");

                this.Add("<div id='divinfo' style='width: 155px; position: absolute; color: Lime; display: none;cursor: pointer;align:left'></div>");
                #endregion load js.

                #region  Processed first automatic filling , Drop-down box data .
                foreach (MapExt me in mes)
                {
                    switch (me.ExtType)
                    {
                        case MapExtXmlList.AutoFullDLL: //  Automatically populate drop-down box .
                            DDL ddlFull = this.GetDDLByID("DDL_" + me.AttrOfOper);
                            if (ddlFull == null)
                            {
                                me.Delete();
                                continue;
                            }

                            string valOld = ddlFull.SelectedItemStringVal;
                            string fullSQL = me.Doc.Clone() as string;

                            fullSQL = BP.WF.Glo.DealExp(fullSQL, en, "");

                            ddlFull.Items.Clear();
                            ddlFull.Bind(DBAccess.RunSQLReturnTable(fullSQL), "No", "Name");
                            ddlFull.SetSelectItem(en.GetValStrByKey(me.AttrOfOper));
                            break;
                        case MapExtXmlList.AutoFull: //  Automatically populate drop-down box .
                            break;
                        default:
                            break;
                    }
                }
                #endregion  Processed first automatic filling , Drop-down box data .

                #region  In the process other .
                System.Data.DataTable dt = new DataTable();
                foreach (MapExt me in mes)
                {
                    switch (me.ExtType)
                    {
                        case MapExtXmlList.DDLFullCtrl: //  Automatically populate other controls ..
                            DDL ddlOper = this.GetDDLByID("DDL_" + me.AttrOfOper);
                            if (ddlOper == null)
                                continue;

                            ddlOper.Attributes["onchange"] = "DDLFullCtrl(this.value,\'" + ddlOper.ClientID + "\', \'" + me.MyPK + "\')";

                            if (me.Tag != "")
                            {
                                /*  Handle drop-down box to select the range of issues  */
                                string[] strs = me.Tag.Split('$');
                                foreach (string str in strs)
                                {
                                    string[] myCtl = str.Split(':');
                                    string ctlID = myCtl[0];
                                    DDL ddlC1 = this.GetDDLByID("DDL_" + ctlID);
                                    if (ddlC1 == null)
                                    {
                                        //me.Tag = "";
                                        //me.Update();
                                        continue;
                                    }

                                    string sql = myCtl[1].Replace("~", "'");
                                    sql = BP.WF.Glo.DealExp(sql, en, null);
                                    sql = sql.Replace("@Key", ddlOper.SelectedItemStringVal.Trim());

                                    //sql = sql.Replace("@WebUser.No", WebUser.No);
                                    //sql = sql.Replace("@WebUser.Name", WebUser.Name);
                                    //sql = sql.Replace("@WebUser.FK_Dept", WebUser.FK_Dept);
                                    //sql = sql.Replace("@WebUser.FK_DeptName", WebUser.FK_DeptName);
                                    //sql = sql.Replace("@Key", ddlOper.SelectedItemStringVal.Trim());
                                    //if (sql.Contains("@"))
                                    //{
                                    //    foreach (MapAttr attr in mattrs)
                                    //    {
                                    //        if (sql.Contains("@" + attr.KeyOfEn) == false)
                                    //            continue;
                                    //        sql = sql.Replace("@" + attr.KeyOfEn, en.GetValStrByKey(attr.KeyOfEn));
                                    //        if (sql.Contains("@") == false)
                                    //            break;
                                    //    }
                                    //}

                                    dt = DBAccess.RunSQLReturnTable(sql);
                                    string valC1 = ddlC1.SelectedItemStringVal;
                                    ddlC1.Items.Clear();
                                    foreach (DataRow dr in dt.Rows)
                                        ddlC1.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                                    ddlC1.SetSelectItem(valC1);
                                }
                            }

                            break;
                        case MapExtXmlList.ActiveDDL: /* Automatic initialization ddl The drop-down box data .*/
                            DDL ddlPerant = this.GetDDLByID("DDL_" + me.AttrOfOper);
                            DDL ddlChild = this.GetDDLByID("DDL_" + me.AttrsOfActive);
                            if (ddlPerant == null || ddlChild == null)
                                continue;
                            ddlPerant.Attributes["onchange"] = "DDLAnsc(this.value,\'" + ddlChild.ClientID + "\', \'" + me.MyPK + "\')";
                            //  Processing the default choice .
                            string val = ddlPerant.SelectedItemStringVal;
                            string valClient = en.GetValStrByKey(me.AttrsOfActive); // ddlChild.SelectedItemStringVal;

                            string fullSQL = me.Doc.Clone() as string;
                            fullSQL = fullSQL.Replace("~", ",");
                            fullSQL = fullSQL.Replace("@Key", val);
                            fullSQL = fullSQL.Replace("@WebUser.No", WebUser.No);
                            fullSQL = fullSQL.Replace("@WebUser.Name", WebUser.Name);
                            fullSQL = fullSQL.Replace("@WebUser.FK_Dept", WebUser.FK_Dept);
                            fullSQL = BP.WF.Glo.DealExp(fullSQL, en, null);

                            dt = DBAccess.RunSQLReturnTable(fullSQL);
                            // ddlChild.Items.Clear();
                            foreach (DataRow dr in dt.Rows)
                                ddlChild.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));

                            ddlChild.SetSelectItem(valClient);
                            break;
                        case MapExtXmlList.AutoFullDLL: //  Automatically populate drop-down box .
                            continue; // Has been processed .
                        case MapExtXmlList.TBFullCtrl: //  Automatic filling .
                            TextBox tbAuto = this.GetTextBoxByID("TB_" + me.AttrOfOper);
                            if (tbAuto == null)
                                continue;

                            // onpropertychange
                            // tbAuto.Attributes["onpropertychange"] = "DoAnscToFillDiv(this,this.value,\'" + tbAuto.ClientID + "\', \'" + me.MyPK + "\');";
                            // tbAuto.Attributes["onkeydown"] = "DoAnscToFillDiv(this,this.value,\'" + tbAuto.ClientID + "\', \'" + me.MyPK + "\');";
                            // tbAuto.Attributes["onkeyup"] = "DoAnscToFillDiv(this,this.value,\'" + tbAuto.ClientID + "\', \'" + me.MyPK + "\');";
                            // tbAuto.Attributes["ondblclick"] = "ReturnValTBFullCtrl(this,'" + me.MyPK + "','sd');";

                            tbAuto.Attributes["ondblclick"] = "ReturnValTBFullCtrl(this,'" + me.MyPK + "');";
                            tbAuto.Attributes["onkeyup"] = "DoAnscToFillDiv(this,this.value,\'" + tbAuto.ClientID + "\', \'" + me.MyPK + "\');";
                            tbAuto.Attributes["AUTOCOMPLETE"] = "OFF";
                            if (me.Tag != "")
                            {
                                /*  Handle drop-down box to select the range of issues  */
                                string[] strs = me.Tag.Split('$');
                                foreach (string str in strs)
                                {
                                    string[] myCtl = str.Split(':');
                                    string ctlID = myCtl[0];
                                    DDL ddlC1 = this.GetDDLByID("DDL_" + ctlID);
                                    if (ddlC1 == null)
                                    {
                                        //me.Tag = "";
                                        //me.Update();
                                        continue;
                                    }

                                    string sql = myCtl[1].Replace("~", "'");

                                    string txt = tbAuto.Text.Trim();

                                    //if (string.IsNullOrEmpty(txt))
                                    //    txt = "$";

                                    sql = sql.Replace("@Key", txt);
                                    sql = BP.WF.Glo.DealExp(sql, en, null);

                                    //sql = sql.Replace("@WebUser.No", WebUser.No);
                                    //sql = sql.Replace("@WebUser.Name", WebUser.Name);
                                    //sql = sql.Replace("@WebUser.FK_Dept", WebUser.FK_Dept);
                                    //sql = sql.Replace("@WebUser.FK_DeptName", WebUser.FK_DeptName);
                                    //if (sql.Contains("@"))
                                    //{
                                    //    foreach (MapAttr attr in mattrs)
                                    //    {
                                    //        if (sql.Contains("@" + attr.KeyOfEn) == false)
                                    //            continue;
                                    //        sql = sql.Replace("@" + attr.KeyOfEn, en.GetValStrByKey(attr.KeyOfEn));
                                    //        if (sql.Contains("@") == false)
                                    //            break;
                                    //    }
                                    //}

                                    try
                                    {
                                        dt = DBAccess.RunSQLReturnTable(sql);
                                    }
                                    catch (Exception ex)
                                    {
                                        this.Clear();

                                        this.AddFieldSet(" Configuration error ");
                                        this.Add(me.ToStringAtParas() + "<hr> Error Messages :<br>" + ex.Message);
                                        this.AddFieldSetEnd();
                                        return;
                                    }

                                    string valC1 = ddlC1.SelectedItemStringVal;
                                    ddlC1.Items.Clear();
                                    foreach (DataRow dr in dt.Rows)
                                        ddlC1.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                                    ddlC1.SetSelectItem(valC1);
                                }
                            }
                            break;
                        case MapExtXmlList.InputCheck:
                            TextBox tbJS = this.GetTextBoxByID("TB_" + me.AttrOfOper);
                            if (tbJS != null)
                            {
                                tbJS.Attributes[me.Tag2] = me.Tag1 + "(this);";
                            }
                            else
                            {
                                DDL ddl = this.GetDDLByID("DDL_" + me.AttrOfOper);
                                if (ddl != null)
                                    ddl.Attributes[me.Tag2] = me.Tag1 + "(this);";
                            }
                            break;
                        case MapExtXmlList.PopVal: //  Pop-up window .
                            TB tb = this.GetTBByID("TB_" + me.AttrOfOper);
                            if (tb == null)
                                continue;

                            // Remove vocabulary event 
                            if (tb.Rows > 1)
                                tb.Attributes.Remove("ondblclick");

                            if (tb.CssClass != "TBReadonly")
                            {
                                if (me.PopValWorkModel == 0)
                                    tb.Attributes["ondblclick"] = "ReturnVal(this,'" + BP.WF.Glo.DealExp(me.Doc, en, null) + "','sd');";
                                else
                                    tb.Attributes["ondblclick"] = "ReturnValCCFormPopVal(this,'" + me.MyPK + "','" + en.PKVal + "');";
                            }
                            break;
                        case MapExtXmlList.RegularExpression:// Regex , Control data processing 
                            WebControl tbExp = this.GetTBByID("TB_" + me.AttrOfOper);

                            if (tbExp == null)
                                tbExp = this.GetCBByID("CB_" + me.AttrOfOper);

                            if (tbExp == null)
                                tbExp = this.GetDDLByID("DDL_" + me.AttrOfOper);

                            if (tbExp == null || me.Tag == "onsubmit")
                                continue;

                            // Regular validate input format 
                            string regFilter = me.Doc;
                            if (regFilter.LastIndexOf("/g") < 0 && regFilter.LastIndexOf('/') < 0)
                                regFilter = "'" + regFilter + "'";
                            // Handling Events 
                            if (me.Tag == "onkeyup" || me.Tag == "onkeypress")
                            {
                                tbExp.Attributes.Add("" + me.Tag + "", "return txtTest_Onkeyup(this," + regFilter + ",'" + me.Tag1 + "')");//[me.Tag] += "this.value=this.value.replace(" + regFilter + ",'')";
                                //tbExp.Attributes[me.Tag] += "value=value.replace(" + regFilter + ",'')";
                            }
                            else if (me.Tag == "onclick")
                            {
                                tbExp.Attributes[me.Tag] += me.Doc;
                            }
                            else
                            {
                                tbExp.Attributes[me.Tag] += "EleInputCheck2(this," + regFilter + ",'" + me.Tag1 + "');";
                            }
                            break;
                        default:
                            break;
                    }
                }
                #endregion  In the process other .

            }

            #region  When you save a handle regular expression validation 
            string scriptCheckFrm = "";
            scriptCheckFrm = "\t\n<script type='text/javascript' >";
            scriptCheckFrm += "\t\n function SysCheckFrm(){ ";
            scriptCheckFrm += "\t\n var isPass = true;";
            scriptCheckFrm += "\t\n var alloweSave = true;";
            scriptCheckFrm += "\t\n var erroMsg = ' Message :';";
            foreach (MapExt me in mes)
            {
                if (me.ExtType == MapExtXmlList.RegularExpression && me.Tag == "onsubmit")
                {
                    TB tb = this.GetTBByID("TB_" + me.AttrOfOper);
                    if (tb == null)
                        continue;
                    scriptCheckFrm += "\t\n try{  ";
                    scriptCheckFrm += "\t\n var element = document.getElementById('" + tb.ClientID + "');";
                    // Regular validate input format 
                    string regFilter = me.Doc;
                    if (regFilter.LastIndexOf("/g") < 0 && regFilter.LastIndexOf('/') < 0)
                        regFilter = "'" + regFilter + "'";

                    scriptCheckFrm += "\t\n isPass = EleSubmitCheck(element," + regFilter + ",'" + me.Tag1 + "');";
                    //scriptCheckFrm += "\t\n var reg =new RegExp(" + regFilter + ");   isPass = reg.test(element.value); ";
                    scriptCheckFrm += "\t\n  if(isPass == false){";
                    scriptCheckFrm += "\t\n   //EleSubmitCheck(element," + regFilter + ",'" + me.Tag1 + "'); alloweSave = false;";
                    scriptCheckFrm += "\t\n   alloweSave = false;";
                    scriptCheckFrm += "\t\n    erroMsg += '" + me.Tag1 + ";';";
                    scriptCheckFrm += "\t\n  }";
                    scriptCheckFrm += "\t\n } catch(e) { ";
                    scriptCheckFrm += "\t\n  alert(e.name  + e.message);  return false;";
                    scriptCheckFrm += "\t\n } ";
                }
            }
            scriptCheckFrm += "\t\n if(alloweSave == false){";
            scriptCheckFrm += "\t\n     alert(erroMsg);";
            scriptCheckFrm += "\t\n  } ";
            scriptCheckFrm += "\t\n return alloweSave; } ";
            scriptCheckFrm += "\t\n</script>";
            this.Add(scriptCheckFrm);
            #endregion

            #endregion  Processing Extension Set 

            #region  Automatic calculation processing 
            string js = "\t\n <script type='text/javascript' >oid=" + en.PKVal + ";</script>";
            this.Add(js);
            foreach (MapExt ext in mes)
            {
                if (ext.Tag != "1")
                    continue;

                js = "\t\n <script type='text/javascript' >";
                TB tb = null;
                try
                {
                    tb = this.GetTBByID("TB_" + ext.AttrOfOper);
                    if (tb == null)
                        continue;
                }
                catch
                {
                    continue;
                }

                string left = "\n  document.forms[0]." + tb.ClientID + ".value = ";
                string right = ext.Doc;

                Paras ps = new Paras();
                ps.SQL = "SELECT KeyOfEn,Name FROM Sys_MapAttr WHERE FK_MapData=" + ps.DBStr + "FK_MapData AND LGType=0 AND (MyDataType=2 OR MyDataType=3 OR MyDataType=5 OR MyDataType=8 OR MyDataType=9) ORDER BY KeyOfEn DESC";
                ps.Add("FK_MapData", enName);

                DataTable dt = DBAccess.RunSQLReturnTable(ps);
                foreach (DataRow dr in dt.Rows)
                {
                    string keyofen = dr[0].ToString();
                    string name = dr[1].ToString();

                    if (ext.Doc.Contains("@" + keyofen)
                        || (ext.Doc.Contains("@" + name) && !string.IsNullOrEmpty(name)))
                    {
                    }
                    else
                    {
                        continue;
                    }

                    string tbID = "TB_" + keyofen;
                    TB mytb = this.GetTBByID(tbID);
                    this.GetTBByID(tbID).Attributes["onkeyup"] += "javascript:Auto" + ext.AttrOfOper + "();";

                    right = right.Replace("@" + keyofen, " parseFloat( document.forms[0]." + mytb.ClientID + ".value.replace( ',' ,  '' ) ) ");
                    if (!string.IsNullOrEmpty(name))
                        right = right.Replace("@" + name, " parseFloat( document.forms[0]." + mytb.ClientID + ".value.replace( ',' ,  '' ) ) ");
                }

                int myDataType = BP.DA.DataType.AppMoney;

                // Determine the type 
                foreach (MapAttr attr in mattrs)
                {
                    if (attr.KeyOfEn == ext.AttrOfOper)
                    {
                        myDataType = attr.MyDataType;
                    }
                }

                js += "\t\n function Auto" + ext.AttrOfOper + "() { ";
                js += left + right + ";";
                if (myDataType == BP.DA.DataType.AppFloat || myDataType == BP.DA.DataType.AppDouble)
                {
                    js += " \t\n  document.forms[0]." + tb.ClientID + ".value= document.forms[0]." + tb.ClientID + ".value;";
                }
                else
                {
                    js += " \t\n  document.forms[0]." + tb.ClientID + ".value= VirtyMoney(document.forms[0]." + tb.ClientID + ".value ) ;";
                }
                js += "\t\n } ";
                js += "\t\n</script>";
                this.Add(js); // Join Inside .
            }
            #endregion

        }
        public void InsertObjects(bool isJudgeRowIdx)
        {
            #region  From Table 
            foreach (MapDtl dtl in dtls)
            {
                if (dtl.IsView == false || this.ctrlUseSta.Contains(dtl.No))
                    continue;

                if (dtl.GroupID == 0)
                {
                    dtl.GroupID = currGF.OID;
                    dtl.RowIdx = 0;
                    dtl.Update();
                }

                if (isJudgeRowIdx)
                {
                    if (dtl.RowIdx != rowIdx)
                        continue;
                }

                if (dtl.GroupID == currGF.OID)
                {

                }
                else
                {
                    continue;
                }

                // dtl.IsUse = true;

                this.ctrlUseSta += dtl.No;

                rowIdx++;
                // myidx++;
                this.AddTR(" ID='" + currGF.Idx + "_" + rowIdx + "' ");
                this.Add("<TD colspan=" + this.mapData.TableCol + " ID='TD" + dtl.No + "' height='50px' width='100%' style='align:left'>");
                string src = "";
                try
                {
                    src = CCFlowAppPath + "WF/CCForm/Dtl.aspx?EnsName=" + dtl.No + "&RefPKVal=" + this.HisEn.PKVal + "&FID=" + this.HisEn.GetValStringByKey("FID") + "&IsWap=0&FK_Node=" + dtl.FK_MapData.Replace("ND", "");
                }
                catch
                {
                    src = CCFlowAppPath + "WF/CCForm/Dtl.aspx?EnsName=" + dtl.No + "&RefPKVal=" + this.HisEn.PKVal + "&IsWap=0&FK_Node=" + dtl.FK_MapData.Replace("ND", "");
                }

                if (this.IsReadonly || dtl.IsReadonly)
                    this.Add("<iframe ID='F" + dtl.No + "'  src='" + src +
                             "&IsReadonly=1' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='100%' height='30px' /></iframe>");
                else
                {
                    //this.Add("<iframe ID='F" + dtl.No + "'   Onblur=\"SaveDtl('" + dtl.No + "');\"  src='" + src + "' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='100%' height='10px' /></iframe>");

                    AddLoadFunction(dtl.No, "blur", "SaveDtl");

                    this.Add("<iframe ID='F" + dtl.No + "'   onload='" + dtl.No + "load();'  src='" + src + "' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='100%' height='10px' /></iframe>");

                }

                this.AddTDEnd();
                this.AddTREnd();
            }
            #endregion  From Table 

            #region  Many relationship 
            foreach (MapM2M m2m in m2ms)
            {
                if (this.ctrlUseSta.Contains("@" + m2m.MyPK))
                    continue;

                if (isJudgeRowIdx)
                {
                    if (m2m.RowIdx != rowIdx)
                        continue;
                }

                if (m2m.GroupID == 0 && rowIdx == 0)
                {
                    m2m.GroupID = currGF.OID;
                    m2m.RowIdx = 0;
                    m2m.Update();
                }
                else if (m2m.GroupID == currGF.OID)
                {

                }
                else
                {
                    continue;
                }

                this.ctrlUseSta += "@" + m2m.MyPK;


                rowIdx++;
                this.AddTR(" ID='" + currGF.Idx + "_" + rowIdx + "' ");

                string src = CCFlowAppPath + "WF/CCForm/M2M.aspx?NoOfObj=" + m2m.NoOfObj;
                string paras = this.RequestParas;
                if (paras.Contains("FID=") == false)
                    paras += "&FID=" + this.HisEn.GetValStrByKey("FID");

                if (paras.Contains("OID=") == false)
                    paras += "&OID=" + this.HisEn.GetValStrByKey("OID");

                src += "&r=q" + paras;
                if (src.Contains("FK_MapData") == false)
                    src += "&FK_MapData=" + m2m.FK_MapData;
                switch (m2m.ShowWay)
                {
                    case FrmShowWay.FrmAutoSize:
                        this.Add("<TD colspan=" + this.mapData.TableCol + " ID='TD" + m2m.NoOfObj + "' height='20px' width='100%'  >");
                        if (m2m.HisM2MType == M2MType.M2M)
                        {

                            AddLoadFunction(m2m.NoOfObj, "blur", "SaveM2M");


                            //  this.Add("<iframe ID='F" + m2m.NoOfObj + "'   Onblur=\"SaveM2M('" + m2m.NoOfObj + "');\"  src='" + src + "' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='100%' height='10px' scrolling=no /></iframe>");
                            this.Add("<iframe ID='F" + m2m.NoOfObj + "'  onload='" + m2m.NoOfObj + "load();'   src='" + src + "' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='100%' height='10px' scrolling=no /></iframe>");

                        }
                        else
                            this.Add("<iframe ID='F" + m2m.NoOfObj + "' src='" + src + "' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='100%' height='10px' scrolling=no /></iframe>");
                        break;
                    case FrmShowWay.FrmSpecSize:
                        this.Add("<TD colspan=" + this.mapData.TableCol + "  ID='TD" + m2m.NoOfObj + "' height='" + m2m.H + "' width='" + m2m.W + "'  >");
                        if (m2m.HisM2MType == M2MType.M2M)
                        {
                            AddLoadFunction(m2m.NoOfObj, "blur", "SaveM2M");

                            // this.Add("<iframe ID='F" + m2m.NoOfObj + "'   Onblur=\"SaveM2M('" + m2m.NoOfObj + "');\"  src='" + src + "' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='" + m2m.W + "' height='" + m2m.H + "' scrolling=auto /></iframe>");
                            this.Add("<iframe ID='F" + m2m.NoOfObj + "' onload='" + m2m.NoOfObj + "load();'   src='" + src + "' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='" + m2m.W + "' height='" + m2m.H + "' scrolling=auto /></iframe>");

                        }
                        else
                            this.Add("<iframe ID='F" + m2m.NoOfObj + "'    src='" + src + "' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='" + m2m.W + "' height='" + m2m.H + "' scrolling=auto /></iframe>");
                        break;
                    case FrmShowWay.Hidden:
                        break;
                    case FrmShowWay.WinOpen:
                        this.Add("<TD colspan=" + this.mapData.TableCol + " ID='TD" + m2m.NoOfObj + "' height='20px' width='100%'  >");
                        this.Add("<a href=\"javascript:WinOpen('" + src + "&IsOpen=1','" + m2m.W + "','" + m2m.H + "');\"  />" + m2m.Name + "</a>");
                        break;
                    default:
                        break;
                }
            }
            #endregion  Many relationship 

            #region  Frame 
            foreach (MapFrame fram in frames)
            {
                if (this.ctrlUseSta.Contains("@" + fram.MyPK))
                    continue;

                if (isJudgeRowIdx)
                {
                    if (fram.RowIdx != rowIdx)
                        continue;
                }

                if (fram.GroupID == 0 && rowIdx == 0)
                {
                    fram.GroupID = currGF.OID;
                    fram.RowIdx = 0;
                    fram.Update();
                }
                else if (fram.GroupID == currGF.OID)
                {
                }
                else
                {
                    continue;
                }

                this.ctrlUseSta += "@" + fram.MyPK;
                rowIdx++;
                // myidx++;
                this.AddTR(" ID='" + currGF.Idx + "_" + rowIdx + "' ");
                if (fram.IsAutoSize)
                    this.Add("<TD colspan=" + this.mapData.TableCol + " ID='TD" + fram.NoOfObj + "' height='50px' width='100%'  >");
                else
                    this.Add("<TD colspan=" + this.mapData.TableCol + " ID='TD" + fram.NoOfObj + "' height='" + fram.H + "' width='" + fram.W + "'  >");

                string paras = this.RequestParas;
                if (paras.Contains("FID=") == false)
                    paras += "&FID=" + this.HisEn.GetValStrByKey("FID");

                if (paras.Contains("WorkID=") == false)
                    paras += "&WorkID=" + this.HisEn.GetValStrByKey("OID");

                string src = fram.URL;
                if (src.Contains("?"))
                    src += "&r=q" + paras;
                else
                    src += "?r=q" + paras;

                if (fram.IsAutoSize)
                {
                    this.Add("<iframe ID='F" + fram.NoOfObj + "'   src='" + src + "' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='100%' height='10px' scrolling=auto /></iframe>");
                }
                else
                {
                    this.Add("<iframe ID='F" + fram.NoOfObj + "'   src='" + src + "' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='" + fram.W + "' height='" + fram.H + "' scrolling=auto /></iframe>");
                }

                this.AddTDEnd();
                this.AddTREnd();
            }
            #endregion  Frame 

            #region  Accessory 
            foreach (BP.Sys.FrmAttachment ath in aths)
            {
                if (this.ctrlUseSta.Contains("@" + ath.MyPK))
                    continue;
                if (isJudgeRowIdx)
                {
                    if (ath.RowIdx != rowIdx)
                        continue;
                }

                if (ath.GroupID == 0 && rowIdx == 0)
                {
                    ath.GroupID = currGF.OID;
                    ath.RowIdx = 0;
                    ath.Update();
                }
                else if (ath.GroupID == currGF.OID)
                {
                }
                else
                {
                    continue;
                }
                this.ctrlUseSta += "@" + ath.MyPK;
                rowIdx++;
                // myidx++;
                this.AddTR(" ID='" + currGF.Idx + "_" + rowIdx + "' ");
                this.Add("<TD colspan=" + this.mapData.TableCol + " ID='TD" + ath.MyPK + "' height='50px' width='100%' style='align:left'>");
                string src = "";
                if (this.IsReadonly)
                    src = CCFlowAppPath + "WF/CCForm/AttachmentUpload.aspx?PKVal=" + this.HisEn.PKVal + "&Ath=" + ath.NoOfObj + "&FK_MapData=" + EnName + "&FK_FrmAttachment=" + ath.MyPK + "&IsReadonly=1" + this.RequestParas;
                else
                    src = CCFlowAppPath + "WF/CCForm/AttachmentUpload.aspx?PKVal=" + this.HisEn.PKVal + "&Ath=" + ath.NoOfObj + "&FK_MapData=" + EnName + "&FK_FrmAttachment=" + ath.MyPK + this.RequestParas;

                if (ath.IsAutoSize)
                {
                    this.Add("<iframe ID='F" + ath.MyPK + "'   src='" + src + "' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='100%' height='10px' scrolling=auto /></iframe>");
                }
                else
                {
                    this.Add("<iframe ID='F" + ath.MyPK + "'   src='" + src + "' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='" + ath.W + "' height='" + ath.H + "' scrolling=auto /></iframe>");
                }
                this.AddTDEnd();
                this.AddTREnd();
            }
            #endregion  Accessory 
        }
        public void AddRichTextBox(Entity en, MapAttr attr)
        {
            /* Description This is a rich-text output */
            this.Page.RegisterClientScriptBlock("c51",
     "<script language='JavaScript' src='" + CCFlowAppPath + "WF/Comm/kindeditor/kindeditor-all.js'  charset='utf-8' ></script>");

            this.Page.RegisterClientScriptBlock("c2",
    "<script language='JavaScript' src='" + CCFlowAppPath + "WF/Comm/kindeditor/lang/zh_CN.js'  charset='utf-8' ></script>");

            this.Page.RegisterClientScriptBlock("c53",
  "<script language='JavaScript' src='" + CCFlowAppPath + "WF/Comm/kindeditor/plugins/code/prettify.js'  charset='utf-8' ></script>");


            this.Page.RegisterClientScriptBlock("s51",
    "<link href='" + CCFlowAppPath + "WF/Comm/kindeditor/plugins/code/prettify.css' rel='stylesheet' type='text/css' />");

            this.Page.RegisterClientScriptBlock("s52",
    "<link href='" + CCFlowAppPath + "WF/Comm/kindeditor/themes/default/default.css' rel='stylesheet' type='text/css' />");

            string strs = "\t\n <script>";
            strs += "\t\n  var editor1; ";
            strs += "\t\n KindEditor.ready(function(K) {";
            strs += "\t\n var fID='TB_" + attr.KeyOfEn + "'; ";
            strs += "\t\n var tbID='ContentPlaceHolder1_MyFlowUC1_MyFlow1_UCEn1_'+fID;";

            strs += "\t\n var ctrl =document.getElementById( tbID);";
            strs += "\t\n if (ctrl==null)";
            strs += "\t\n tbID = 'ContentPlaceHolder1_UCEn1_' + fID;";

            strs += "\t\n ctrl =document.getElementById( tbID);";
            strs += "\t\n if (ctrl == null) { ";
            strs += "\t\n tbID = 'ContentPlaceHolder1_WFRpt1_UCEn1_' + fID;";
            strs += "\t\n ctrl =document.getElementById( tbID);";
            strs += "\t\n  } ";

            strs += "\t\n if (ctrl == null) { ";
            strs += "\t\n tbID = 'ContentPlaceHolder1_FHLFlow1_UCEn1_' + fID;";
            strs += "\t\n ctrl =document.getElementById( tbID);";
            strs += "\t\n  } ";


            strs += "\t\n if (ctrl == null) { ";
            strs += "\t\n     alert(' To help set not found the controls '); ";
            strs += "\t\n  } ";

            strs += "\t\n   editor1 = K.create('#'+tbID, {";
            strs += "\t\n cssPath : '" + CCFlowAppPath + "WF/Comm/kindeditor/plugins/code/prettify.css',";
            strs += "\t\n uploadJson : '" + CCFlowAppPath + "WF/Comm/kindeditor/asp.net/upload_json.ashx',";
            strs += "\t\n fileManagerJson : '" + CCFlowAppPath + "WF/Comm/kindeditor/asp.net/file_manager_json.ashx',";
            strs += "\t\n allowFileManager : true,";

            strs += "\t\n width : '100%',";
            //strs += "\t\n width : '" + attr.UIWidth + "px',";

            strs += "\t\n height : '" + attr.UIHeight + "px'";

            strs += "\t\n });";
            strs += "\t\n });";

            //strs += "\t\n KindEditor.show(function(K) {";
            //strs += "\t\n KindEditor.ready(function(K) {";

            strs += "\t\n </script>";
            this.Add(strs);

            TB tbd = new TB();
            tbd.TextMode = TextBoxMode.MultiLine;
            tbd.ID = "TB_" + attr.KeyOfEn;
            tbd.Text = en.GetValStrByKey(attr.KeyOfEn);
            tbd.TextMode = TextBoxMode.MultiLine;
            tbd.Attributes["style"] = "width:" + attr.UIWidth + "px;height:" + attr.UIHeight + "px;visibility:hidden;";
            this.Add(tbd);

        }
        #endregion

        #region  Form a free-form output .
        public string FK_MapData = null;
        FrmEvents fes = null;
        public new string EnName = null;
        public string LinkFields = "";
        public MapData mapData = null;
        /// <summary>
        ///  Handle its default value .
        /// </summary>
        /// <param name="mattrs"></param>
        private void DealDefVal(MapAttrs mattrs)
        {
            if (this.IsReadonly)
                return;

            this.Page.RegisterClientScriptBlock("y7",
          "<script language='JavaScript' src='" + CCFlowAppPath + "DataUser/JSLibData/" + this.EnName + "_Self.js' ></script>");

            this.Page.RegisterClientScriptBlock("yfd7",
      "<script language='JavaScript' src='" + CCFlowAppPath + "DataUser/JSLibData/" + this.EnName + ".js' ></script>");

            foreach (MapAttr attr in mattrs)
            {
                if (attr.DefValReal.Contains("@") == false)
                    continue;

                this.HisEn.SetValByKey(attr.KeyOfEn, attr.DefVal);
            }
        }
        public void BindCCForm(Entity en, string enName, bool isReadonly,float srcWidth)
        {
            MapData md = new MapData(enName);
            BindCCForm(en, md, md.MapAttrs, enName, isReadonly, srcWidth);
        }
        public void BindCCForm(Entity en, MapData md, MapAttrs mattrs, string enName, bool isReadonly, float srcWidth)
        {
            this.ctrlUseSta = "";

            this.EnName = enName;
            this.mapData = md;
            string appPath = BP.WF.Glo.CCFlowAppPath; //this.Request.ApplicationPath;

            // Calculated according to the width of the fine-tuning .
            float wtX = 0;// MapData.GenerSpanWeiYi(md, srcWidth);
            float x=0;

            mes = this.mapData.MapExts;
            this.IsReadonly = isReadonly;
            this.FK_MapData = enName;
            this.HisEn = en;
            this.EnName = enName;
            this.m2ms = this.mapData.MapM2Ms;
            this.dtls = this.mapData.MapDtls;
            this.mes = this.mapData.MapExts;

            // Is loaded CA Signature  dll.
            bool IsAddCa = false;

            #region  Handling Events .
            fes = this.mapData.FrmEvents;
            if (this.IsPostBack == false)
            {
                try
                {
                    string msg = fes.DoEventNode(FrmEventList.FrmLoadBefore, en);
                    if (msg == "OK")
                    {
                        en.RetrieveFromDBSources();
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(msg) == false)
                        {
                            en.RetrieveFromDBSources();
                            this.Alert(msg);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Alert(ex.Message);
                    return;
                }
            }
            #endregion  Handling Events .

            //   MapAttrs mattrs = this.mapData.MapAttrs;
            this.DealDefVal(mattrs);

            // Processing load before filling .
            this.LoadData(mattrs, en);

            #region  Export Ele
            FrmEles eles = this.mapData.FrmEles;
            if (eles.Count >= 1)
            {
                string myjs = "\t\n<script type='text/javascript' >";
                myjs += "\t\n function BPPaint(ctrl,url,w,h,fk_FrmEle)";
                myjs += "\t\n {";
                myjs += "\t\n  var v= window.showModalDialog(url, 'ddf', 'dialogHeight: '+h+'px; dialogWidth: '+w+'px;center: yes; help: no'); ";
                myjs += "\t\n  if (v==null )  ";
                myjs += "\t\n     return ; ";

                ////  myjs += "\t\n     alert(document.getElementById('Ele'+fk_FrmEle ));";
                //  myjs += "\t\n  ctrl.src='dsdsd'; ";
                // myjs += "\t\n  alert(' Has been executed successfully , Thank you for using ')";
                //  myjs += "\t\n  ctrl.src=v; ";
                myjs += "\t\n  ctrl.src=v+'?temp='+new Date(); ";

                //   myjs += "\t\n  alert(ctrl.src)";
                //myjs += "\t\n  ctrl.setAttribute('src',v); ";
                //myjs += "\t\n  document.getElementById('Ele'+fk_FrmEle ).src=v; ";
                //myjs += "\t\n  document.getElementById('Ele'+fk_FrmEle ).setAttribute('src', v); ";
                myjs += "\t\n }";
                myjs += "\t\n</script>";
                this.Add(myjs);

                FrmEleDBs dbs = new FrmEleDBs(this.FK_MapData, en.PKVal.ToString());
                foreach (FrmEle ele in eles)
                {
                    float y = ele.Y;
                    x=ele.X+wtX;
                    this.Add("\t\n<DIV id=" + ele.MyPK + " style='position:absolute;left:" +  x + "px;top:" + y + "px;text-align:left;vertical-align:top' >");
                    switch (ele.EleType)
                    {
                        case FrmEle.HandSiganture:
                            FrmEleDB db = dbs.GetEntityByKey(FrmEleDBAttr.EleID, ele.EleID) as FrmEleDB;
                            string dbFile = appPath + "DataUser/BPPaint/Def.png";
                            if (db != null)
                                dbFile = db.Tag1;

                            if (this.IsReadonly || ele.IsEnable == false)
                            {
                                this.Add("\t\n<img src='" + dbFile + "' onerror=\"this.src='" + appPath + "DataUser/BPPaint/Def.png'\" style='padding: 0px;margin: 0px;border-width: 0px;width:" + ele.W + "px;height:" + ele.H + "px;' />");
                            }
                            else
                            {
                                string url = appPath + "WF/CCForm/BPPaint.aspx?W=" + ele.HandSiganture_WinOpenW + "&H=" + ele.HandSiganture_WinOpenH + "&MyPK=" + ele.PKVal + "&PKVal=" + en.PKVal;
                                myjs = "javascript:BPPaint(this,'" + url + "','" + ele.HandSiganture_WinOpenW + "','" + ele.HandSiganture_WinOpenH + "','" + ele.MyPK + "');";
                                //string myjs = "javascript:window.open('" + appPath + "WF/CCForm/BPPaint.aspx?PKVal=" + en.PKVal + "&MyPK=" + ele.MyPK + "&H=" + ele.HandSiganture_WinOpenH + "&W=" + ele.HandSiganture_WinOpenW + "', 'sdf', 'dialogHeight: " + ele.HandSiganture_WinOpenH + "px; dialogWidth: " + ele.HandSiganture_WinOpenW + "px;center: yes; help: no');";
                                this.Add("\t\n<img id='Ele" + ele.MyPK + "' onclick=\"" + myjs + "\" onerror=\"this.src='" + appPath + "DataUser/BPPaint/Def.png'\" src='" + dbFile + "' style='padding: 0px;margin: 0px;border-width: 0px;width:" + ele.W + "px;height:" + ele.H + "px;' />");
                            }
                            break;
                        case FrmEle.iFrame: // Output Framework .
                            string paras = this.RequestParas;
                            if (paras.Contains("FID=") == false)
                                paras += "&FID=" + this.HisEn.GetValStrByKey("FID");

                            if (paras.Contains("WorkID=") == false)
                                paras += "&WorkID=" + this.HisEn.GetValStrByKey("OID");

                            string src = ele.Tag1.Clone() as string; // url 
                            if (src.Contains("?"))
                                src += "&r=q" + paras;
                            else
                                src += "?r=q" + paras;

                            if (src.Contains("UserNo") == false)
                                src += "&UserNo=" + WebUser.No;
                            if (src.Contains("SID") == false)
                                src += "&SID=" + WebUser.SID;
                            if (src.Contains("@"))
                            {
                                foreach (Attr m in en.EnMap.Attrs)
                                {
                                    if (src.Contains("@") == false)
                                        break;
                                    src = src.Replace("@" + m.Key, en.GetValStrByKey(m.Key));
                                }
                            }
                            this.Add("<iframe ID='F" + ele.EleID + "'   src='" + src + "' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='" + ele.W + "' height='" + ele.H + "' scrolling=auto /></iframe>");
                            break;
                        case FrmEle.EleSiganture:
                        default:
                            this.Add(" Untreated ");
                            break;
                    }
                }
                this.Add("\t\n</DIV>");
            }
            #endregion  Export Ele

            #region  Output button 
            FrmBtns btns = this.mapData.FrmBtns;
            foreach (FrmBtn btn in btns)
            {
                x= btn.X + wtX;
                this.Add("\t\n<DIV id=u2 style='position:absolute;left:" +x + "px;top:" + btn.Y + "px;text-align:left;' >");
                this.Add("\t\n<span >");

                string doDoc = BP.WF.Glo.DealExp(btn.EventContext, en, null);
                switch (btn.HisBtnEventType)
                {
                    case BtnEventType.Disable:
                        this.Add("<input type=button class=Btn value='" + btn.Text.Replace("&nbsp;", " ") + "' disabled='disabled'/>");
                        break;
                    case BtnEventType.RunExe:
                    case BtnEventType.RunJS:
                        this.Add("<input type=button class=Btn value=\"" + btn.Text.Replace("&nbsp;", " ") + "\" enable=true onclick=\"" + doDoc + "\" />");
                        break;
                    default:
                        Button myBtn = new Button();
                        myBtn.Enabled = true;
                        myBtn.CssClass = "Btn";
                        myBtn.ID = btn.MyPK;
                        myBtn.Text = btn.Text.Replace("&nbsp;", " ");
                        myBtn.Click += new EventHandler(myBtn_Click);
                        this.Add(myBtn);
                        break;
                }
                this.Add("\t\n</span>");
                this.Add("\t\n</DIV>");
            }
            #endregion

            #region  Vertical and label output  &  Hyperlinks  Img.
            FrmLabs labs = this.mapData.FrmLabs;
            foreach (FrmLab lab in labs)
            {
                Color col = ColorTranslator.FromHtml(lab.FontColor);
                x= lab.X + wtX ;
                this.Add("\t\n<DIV id=u2 style='position:absolute;left:" +x+ "px;top:" + lab.Y + "px;text-align:left;' >");
                this.Add("\t\n<span style='color:" + lab.FontColorHtml + ";font-family: " + lab.FontName + ";font-size: " + lab.FontSize + "px;' >" + lab.TextHtml + "</span>");
                this.Add("\t\n</DIV>");
            }

            FrmLines lines = this.mapData.FrmLines;
            foreach (FrmLine line in lines)
            {
                if (line.X1 == line.X2)
                {
                    /*  A vertical line  */
                    float h = line.Y1 - line.Y2;
                    h = Math.Abs(h);
                    if (line.Y1 < line.Y2)
                    {
                        x = line.X1 + wtX;
                        this.Add("\t\n<img id='" + line.MyPK + "'  style=\"padding:0px;position:absolute; left:" + x + "px; top:" + line.Y1 + "px; width:" + line.BorderWidth + "px; height:" + h + "px;background-color:" + line.BorderColorHtml + "\" />");
                    }
                    else
                    {
                        x = line.X2 + wtX;
                        this.Add("\t\n<img id='" + line.MyPK + "'  style=\"padding:0px;position:absolute; left:" +x+ "px; top:" + line.Y2 + "px; width:" + line.BorderWidth + "px; height:" + h + "px;background-color:" + line.BorderColorHtml + "\" />");
                    }
                }
                else
                {
                    /*  A horizontal line  */
                    float w = line.X2 - line.X1;

                    if (line.X1 < line.X2)
                    {
                        x = line.X1 + wtX;
                        this.Add("\t\n<img id='" + line.MyPK + "'  style=\"padding:0px;position:absolute; left:" + x + "px; top:" + line.Y1 + "px; width:" + w + "px; height:" + line.BorderWidth + "px;background-color:" + line.BorderColorHtml + "\" />");
                    }
                    else
                    {
                        x = line.X2 + wtX;
                        this.Add("\t\n<img id='" + line.MyPK + "'  style=\"padding:0px;position:absolute; left:" + x + "px; top:" + line.Y2 + "px; width:" + w + "px; height:" + line.BorderWidth + "px;background-color:" + line.BorderColorHtml + "\" />");
                    }
                }
            }

            FrmLinks links = this.mapData.FrmLinks;
            foreach (FrmLink link in links)
            {
                string url = link.URL;
                if (url.Contains("@"))
                {
                    foreach (MapAttr attr in mattrs)
                    {
                        if (url.Contains("@") == false)
                            break;
                        url = url.Replace("@" + attr.KeyOfEn, en.GetValStrByKey(attr.KeyOfEn));
                    }
                }
                x = link.X + wtX;
                this.Add("\t\n<DIV id=u2 style='position:absolute;left:" + x+ "px;top:" + link.Y + "px;text-align:left;' >");
                this.Add("\t\n<span style='color:" + link.FontColorHtml + ";font-family: " + link.FontName + ";font-size: " + link.FontSize + "px;' > <a href=\"" + url + "\" target='" + link.Target + "'> " + link.Text + "</a></span>");
                this.Add("\t\n</DIV>");
            }

            FrmImgs imgs = this.mapData.FrmImgs;
            foreach (FrmImg img in imgs)
            {
                float y = img.Y;
                string imgSrc = "";
                //imgSrc = appPath + "DataUser/ICON/" + BP.Sys.SystemConfig.CompanyID + "/LogBiger.png";
                // Image Type 
                if (img.HisImgAppType == ImgAppType.Img)
                {
                    // Data sources for the local .
                    if (img.SrcType == 0)
                    {
                        if (img.ImgPath.Contains(";") == false)
                            imgSrc = img.ImgPath;
                    }

                    // Data sources for the specified path .
                    if (img.SrcType == 1)
                    {
                        // Pictures path is not the default value 
                        imgSrc = img.ImgURL;
                        if (imgSrc.Contains("@"))
                        {
                            /* If you have a variable */
                            imgSrc = BP.WF.Glo.DealExp(imgSrc, en, "");
                        }
                    }

                    x = img.X + wtX;
                    this.Add("\t\n<DIV id=" + img.MyPK + " style='position:absolute;left:" + x + "px;top:" + y + "px;text-align:left;vertical-align:top' >");
                    if (string.IsNullOrEmpty(img.LinkURL) == false)
                        this.Add("\t\n<a href='" + img.LinkURL + "' target=" + img.LinkTarget + " ><img src='" + imgSrc + "'  onerror=\"this.src='/DataUser/ICON/CCFlow/LogBig.png'\"  style='padding: 0px;margin: 0px;border-width: 0px;width:" + img.W + "px;height:" + img.H + "px;' /></a>");
                    else
                        this.Add("\t\n<img src='" + imgSrc + "'  onerror=\"this.src='/DataUser/ICON/CCFlow/LogBig.png'\"  style='padding: 0px;margin: 0px;border-width: 0px;width:" + img.W + "px;height:" + img.H + "px;' />");
                    this.Add("\t\n</DIV>");
                    continue;
                }

                #region  Electronic Signature 
                // Log people get jobs 
                string stationNo = "";
                // Signature counterparts 
                string fk_dept = WebUser.FK_Dept;
                // Sector source categories 
                string sealType = "0";
                // Signature corresponding positions 
                string fk_station = img.Tag0;
                // Form fields 
                string sealField = "";
                string sql = "";
                // If you set up a collection of departments and positions split 
                if (!string.IsNullOrEmpty(img.Tag0) && img.Tag0.Contains("^") && img.Tag0.Split('^').Length == 4)
                {
                    fk_dept = img.Tag0.Split('^')[0];
                    fk_station = img.Tag0.Split('^')[1];
                    sealType = img.Tag0.Split('^')[2];
                    sealField = img.Tag0.Split('^')[3];
                    // If the department is not set , To obtain sector sources 
                    ////if (fk_dept == "all")
                    //{
                    // The current default landing people 
                    fk_dept = WebUser.FK_Dept;
                    // Sponsor 
                    if (sealType == "1")
                    {
                        sql = "SELECT FK_Dept FROM WF_GenerWorkFlow WHERE WorkID=" + this.HisEn.GetValStrByKey("OID");
                        fk_dept = BP.DA.DBAccess.RunSQLReturnString(sql);
                    }
                    // Form fields 
                    if (sealType == "2" && !string.IsNullOrEmpty(sealField))
                    {
                        // Determine whether the field exists 
                        foreach (MapAttr attr in mattrs)
                        {
                            if (attr.KeyOfEn == sealField)
                            {
                                fk_dept = this.HisEn.GetValStrByKey(sealField);
                                break;
                            }
                        }
                    }
                    ////}
                }
                // Determine whether the person under this sector 
                //sql = "SELECT fk_station from port_deptEmpStation where fk_dept='" + fk_dept + "' and fk_emp='" + WebUser.No + "'";
                sql = string.Format(" select FK_Station from Port_DeptStation where FK_Dept ='{0}' and FK_Station in (select FK_Station from Port_EmpStation where FK_Emp='{1}')", fk_dept, WebUser.No);
                DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    if (fk_station.Contains(dr[0].ToString() + ","))
                    {
                        stationNo = dr[0].ToString();
                        break;
                    }
                }
                // Reload   There may be cached 
                img.Retrieve("MyPk", img.MyPK);
                //0. Can not be modified , Taken from the data table ,1 You can modify the , Using a combination of data acquisition and preservation 
                if (img.IsEdit == 1 && this.IsReadonly == false)
                {
                    imgSrc = CCFlowAppPath + "DataUser/Seal/" + fk_dept + "_" + stationNo + ".jpg";
                    // Set the primary key 
                    string myPK = string.IsNullOrEmpty(img.EnPK) ? "seal" : img.EnPK;
                    myPK = myPK + "_" + this.HisEn.GetValStrByKey("OID") + "_" + img.MyPK;

                    FrmEleDB imgDb = new FrmEleDB();
                    QueryObject queryInfo = new QueryObject(imgDb);
                    queryInfo.AddWhere(FrmEleAttr.MyPK, myPK);
                    queryInfo.DoQuery();
                    // Determine whether there 
                    if (imgDb != null && !string.IsNullOrEmpty(imgDb.FK_MapData))
                    {
                        imgDb.FK_MapData = string.IsNullOrEmpty(img.EnPK) ? "seal" : img.EnPK;
                        imgDb.EleID = this.HisEn.GetValStrByKey("OID");
                        imgDb.RefPKVal = img.MyPK;
                        imgDb.Tag1 = imgSrc;
                        imgDb.Update();
                    }
                    else
                    {
                        imgDb.FK_MapData = string.IsNullOrEmpty(img.EnPK) ? "seal" : img.EnPK;
                        imgDb.EleID = this.HisEn.GetValStrByKey("OID");
                        imgDb.RefPKVal = img.MyPK;
                        imgDb.Tag1 = imgSrc;
                        imgDb.Insert();
                    }
                    // Adding Controls 
                    x = img.X + wtX;
                    this.Add("\t\n<DIV id=" + img.MyPK + " style='position:absolute;left:" + x + "px;top:" + y + "px;text-align:left;vertical-align:top' >");
                    this.Add("\t\n<img src='" + imgSrc + "' onerror='javascript:this.src='" + appPath + "DataUser/ICON/" + BP.Sys.SystemConfig.CompanyID + "/LogBiger.png';' style='padding: 0px;margin: 0px;border-width: 0px;width:" + img.W + "px;height:" + img.H + "px;' />");
                    this.Add("\t\n</DIV>");
                }
                else
                {
                    FrmEleDB realDB = null;
                    FrmEleDB imgDb = new FrmEleDB();
                    QueryObject objQuery = new QueryObject(imgDb);
                    objQuery.AddWhere(FrmEleAttr.FK_MapData, img.EnPK);
                    objQuery.addAnd();
                    objQuery.AddWhere(FrmEleAttr.EleID, this.HisEn.GetValStrByKey("OID"));
                    objQuery.DoQuery();
                    if (objQuery.GetCount() == 0)
                    {
                        FrmEleDBs imgdbs = new FrmEleDBs();
                        QueryObject objQuerys = new QueryObject(imgdbs);
                        objQuerys.AddWhere(FrmEleAttr.EleID, this.HisEn.GetValStrByKey("OID"));
                        objQuerys.DoQuery();
                        foreach (FrmEleDB single in imgdbs)
                        {
                            if (single.FK_MapData.Substring(6, single.FK_MapData.Length - 6).Equals(img.EnPK.Substring(6, img.EnPK.Length - 6)))
                            {
                                single.FK_MapData = img.EnPK;
                                single.MyPK = img.EnPK + "_" + this.HisEn.GetValStrByKey("OID") + "_" + img.EnPK;
                                single.RefPKVal = img.EnPK;
                                single.DirectInsert();
                                realDB = single;
                                break;
                            }
                        }
                    }
                    else
                    {
                        realDB = imgDb;
                    }
                    imgSrc = realDB.Tag1;
                    // If there is no record found , Controls are not displayed . Did not take a step instructions stamped 
                    x = img.X + wtX;
                    this.Add("\t\n<DIV id=" + img.MyPK + " style='position:absolute;left:" + x+ "px;top:" + y + "px;text-align:left;vertical-align:top' >");
                    this.Add("\t\n<img src='" + imgSrc + "' onerror='javascript:this.src='" + appPath + "DataUser/ICON/" + BP.Sys.SystemConfig.CompanyID + "/LogBiger.png';' style='padding: 0px;margin: 0px;border-width: 0px;width:" + img.W + "px;height:" + img.H + "px;' />");
                    this.Add("\t\n</DIV>");
                }
                #endregion
            }
            #endregion  Vertical and label output 

            #region  Output data control .
            TB tb = new TB();
            //DDL ddl = new DDL();
            //CheckBox cb = new CheckBox();
            foreach (MapAttr attr in mattrs)
            {
                if (attr.UIVisible == false && attr.UIIsEnable)
                {
                    TB tbH = new TB();
                    //tbH.Visible = false;
                    tbH.Attributes["Style"] = "display:none;";
                    tbH.ID = "TB_" + attr.KeyOfEn;
                    tbH.Text = en.GetValStrByKey(attr.KeyOfEn);
                    this.Add(tbH);
                    continue;
                }

                if (attr.UIVisible == false)
                    continue;

                x = attr.X + wtX;
                if (attr.LGType == FieldTypeS.Enum || attr.LGType == FieldTypeS.FK)
                    this.Add("<DIV id='F" + attr.KeyOfEn + "' style='position:absolute; left:" + x + "px; top:" + attr.Y + "px;  height:16px;text-align: left;word-break: keep-all;' >");
                else
                    this.Add("<DIV id='F" + attr.KeyOfEn + "' style='position:absolute; left:" + x + "px; top:" + attr.Y + "px; width:" + attr.UIWidth + "px; height:16px;text-align: left;word-break: keep-all;' >");

                this.Add("<span>");

                #region add contrals.
                if (attr.UIIsEnable == false && this.LinkFields.Contains("," + attr.KeyOfEn + ","))
                {
                    MapExt meLink = mes.GetEntityByKey(MapExtAttr.ExtType, MapExtXmlList.Link) as MapExt;
                    string url = meLink.Tag;
                    if (url.Contains("?") == false)
                        url = url + "?a3=2";
                    url = url + "&WebUserNo=" + WebUser.No + "&SID=" + WebUser.SID + "&EnName=" + enName;
                    if (url.Contains("@AppPath"))
                        url = url.Replace("@AppPath", "http://" + this.Request.Url.Host + appPath);
                    if (url.Contains("@"))
                    {
                        Attrs attrs = en.EnMap.Attrs;
                        foreach (Attr item in attrs)
                        {
                            url = url.Replace("@" + attr.KeyOfEn, en.GetValStrByKey(attr.KeyOfEn));
                            if (url.Contains("@") == false)
                                break;
                        }
                    }
                    this.Add("<a href='" + url + "' target='" + meLink.Tag1 + "' >" + en.GetValByKey(attr.KeyOfEn) + "</a>");
                    this.Add("</span>");
                    this.Add("</DIV>");
                    continue;
                }

                #region  Digital Signatures 
                if (attr.IsSigan)
                {
                    #region  Pictures signature  (dai guoqiang)
                    if (attr.SignType == SignType.Pic)
                    {
                        bool isEdit = false;// Can Edit Signature 
                        string v = en.GetValStrByKey(attr.KeyOfEn);
                        // If it is empty , The default login using the current signature 
                        if (string.IsNullOrEmpty(v))
                        {
                            v = WebUser.No;
                            // If it is read-only and is empty , Displayed as unsigned 
                            if (this.IsReadonly)
                            {
                                v = "sigan-readonly";
                            }

                            if (attr.PicType == PicType.ShouDong)
                            {
                                isEdit = true;
                                v = "sigan-readonly";
                            }
                        }
                        if (this.FK_Node != 0 && this.IsReadonly == false)
                        {
                            // Gets the program , If editable , Then on the property is set to true
                            v = en.GetValStrByKey(attr.KeyOfEn);
                            long workId = Convert.ToInt64(this.HisEn.GetValStrByKey("OID"));
                            FrmField keyOfEn = new FrmField();
                            QueryObject info = new QueryObject(keyOfEn);
                            info.AddWhere(FrmFieldAttr.FK_Node, this.FK_Node);
                            info.addAnd();
                            info.AddWhere(FrmFieldAttr.FK_MapData, attr.FK_MapData);
                            info.addAnd();
                            info.AddWhere(FrmFieldAttr.KeyOfEn, attr.KeyOfEn);
                            info.addAnd();
                            info.AddWhere(MapAttrAttr.UIIsEnable, "1");
                            if (info.DoQuery() > 0)
                            {
                                isEdit = true;// Editable , If the value is empty editable picture 
                                if (string.IsNullOrEmpty(v)) v = "siganture";
                            }
                            else
                            {
                                // Not editable , If the value is an empty show non-editable picture 
                                if (string.IsNullOrEmpty(v)) v = "sigan-readonly";
                            }
                        }




                        // If editable , Modify the signature 
                        if (isEdit)
                        {
                            this.Add("<img src='" + appPath + "DataUser/Siganture/" + v + ".jpg' "
                            + "ondblclick=\"SigantureAct(this,'" + WebUser.No + "','" + attr.FK_MapData + "','" + attr.KeyOfEn
                            + "','" + this.HisEn.GetValStrByKey("OID") + "');\" border=\"0\" alt=\" Double-click to sign or cancel signature \" onerror=\"this.src='" + appPath + "DataUser/Siganture/UnName.jpg'\"/>");
                        }
                        else
                        {
                            this.Add("<img src='" + appPath + "DataUser/Siganture/" + v + ".jpg' border=0 onerror=\"this.src='" + appPath + "DataUser/Siganture/UnName.jpg'\"/>");
                        }


                    } // End Picture signature .
                    #endregion  End Picture signature 

                    #region CA Signature  (song honggang 2014-06-08)
                    if (attr.SignType == SignType.CA)
                    {
                        if (IsAddCa == false)
                        {
                            IsAddCa = true;
                            HtmlGenericControl loadWebSignJs = new HtmlGenericControl("script");
                            loadWebSignJs.Attributes["type"] = "text/javascript";
                            loadWebSignJs.Attributes["src"] = "/WF/Activex/Sign/Loadwebsign.js";
                            Page.Header.Controls.Add(loadWebSignJs);
                            ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "mainCA", "/WF/Activex/Sign/main.js");
                        }

                        if (!string.IsNullOrEmpty(attr.Para_SiganField))
                        {
                            //string signClient = this.GetTBByID("TB_" + attr.Para_SiganField).ClientID;
                            string signClient = "";
                            if (this.PageID == "Frm")
                                signClient = "ctl00$ContentPlaceHolder1$UCEn1$TB_" + attr.Para_SiganField;
                            else
                                signClient = "ctl00$ContentPlaceHolder1$MyFlowUC1$MyFlow1$UCEn1$TB_" + attr.Para_SiganField;

                            this.Add("<span id='" + signClient + "sealpostion' />");
                            this.Add("<img  src='" + appPath + "DataUser/Siganture/setting.JPG' ondblclick=\"addseal('" + signClient + "');\"  border=0 onerror=\"this.src='" + appPath + "DataUser/Siganture/UnName.jpg'\"/>");
                        }
                    }
                    #endregion  End CA Signature 

                    this.Add("</span>");
                    this.Add("</DIV>");
                    continue;

                }
                #endregion

                if (attr.MaxLen >= 3999 && attr.TBModel == 2)
                {
                    this.AddRichTextBox(en, attr);
                    this.Add("</span>");
                    this.Add("</DIV>");
                    continue;
                }

                if (attr.UIContralType == UIContralType.TB)
                {
                    tb = new TB();
                    tb.ID = "TB_" + attr.KeyOfEn;
                    if (attr.UIIsEnable == false || isReadonly == true)
                    {
                        tb.Attributes.Add("readonly", "true");
                        tb.CssClass = "TBReadonly";
                        tb.ReadOnly = true;
                    }
                    else
                    {
                        //add by dgq 2013-4-9  Adding modification event 
                        tb.Attributes["onchange"] += "Change('" + attr.FK_MapData + "');";
                    }
                    tb.Attributes["tabindex"] = attr.IDX.ToString();
                }


                switch (attr.LGType)
                {
                    case FieldTypeS.Normal:
                        switch (attr.MyDataType)
                        {
                            case BP.DA.DataType.AppString:
                                if (attr.UIRows == 1)
                                {
                                    tb.Text = en.GetValStringByKey(attr.KeyOfEn);
                                    tb.Attributes["style"] = "width: " + attr.UIWidth + "px; text-align: left; height: 15px;padding: 0px;margin: 0px;";
                                    if (attr.UIIsEnable && isReadonly == false)
                                        tb.CssClass = "TB";
                                    else
                                        tb.CssClass = "TBReadonly";
                                    this.Add(tb);
                                }
                                else
                                {
                                    tb.TextMode = TextBoxMode.MultiLine;

                                    tb.Text = en.GetValStringByKey(attr.KeyOfEn);

                                    tb.Attributes["style"] = "width: " + attr.UIWidth + "px; text-align: left;padding: 0px;margin: 0px;";
                                    tb.Attributes["maxlength"] = attr.MaxLen.ToString();
                                    tb.Rows = attr.UIRows;

                                    if (attr.UIIsEnable && isReadonly == false)
                                    {
                                        tb.CssClass = "TBDoc";
                                        tb.Attributes["ondblclick"] = "TBHelp('" + tb.ID + "','" + appPath + "','" + enName + "','" + attr.KeyOfEn + "');";
                                    }
                                    else
                                        tb.CssClass = "TBReadonly";

                                    this.Add(tb);
                                }
                                break;
                            case BP.DA.DataType.AppDate:
                                tb.ShowType = TBType.Date;
                                tb.Text = en.GetValStrByKey(attr.KeyOfEn);
                                if (attr.UIIsEnable && this.IsReadonly == false)
                                {
                                    tb.Attributes["onfocus"] = "WdatePicker();";
                                    tb.Attributes["class"] = "TB";
                                }
                                else
                                    tb.Attributes["class"] = "TBReadonly";

                                tb.Attributes["style"] = "width: " + attr.UIWidth + "px; text-align: left; height: 19px;";
                                this.Add(tb);
                                break;
                            case BP.DA.DataType.AppDateTime:
                                tb.ShowType = TBType.DateTime;
                                tb.Text = en.GetValStrByKey(attr.KeyOfEn);

                                if (attr.UIIsEnable && this.IsReadonly == false)
                                    tb.Attributes["class"] = "TBcalendar";
                                else
                                    tb.Attributes["class"] = "TBReadonly";

                                if (attr.UIIsEnable)
                                    tb.Attributes["onfocus"] = "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'});";
                                tb.Attributes["style"] = "width: " + attr.UIWidth + "px; text-align: left; height: 19px;";
                                this.Add(tb);
                                break;
                            case BP.DA.DataType.AppBoolean:
                                CheckBox cb = new CheckBox();
                                cb.Width = 350;
                                cb.Text = attr.Name;
                                cb.ID = "CB_" + attr.KeyOfEn;
                                cb.Checked = attr.DefValOfBool;
                                cb.Enabled = attr.UIIsEnable;
                                cb.Checked = en.GetValBooleanByKey(attr.KeyOfEn);
                                if (cb.Enabled == false || isReadonly == true)
                                    cb.Enabled = false;
                                else
                                {
                                    //add by dgq 2013-4-9, Event add content modified 
                                    cb.Attributes["onmousedown"] = "Change('" + attr.FK_MapData + "')";
                                    cb.Enabled = true;
                                }
                                this.Add(cb);
                                break;
                            case BP.DA.DataType.AppDouble:
                            case BP.DA.DataType.AppFloat:
                                tb.Attributes["style"] = "width: " + attr.GetValStrByKey("UIWidth") + "px; text-align: right; height: 19px;word-break: keep-all;";
                                tb.Text = en.GetValStrByKey(attr.KeyOfEn);

                                if (attr.UIIsEnable && isReadonly == false)
                                {
                                    // Increased verification 
                                    //tb.Attributes.Add("onkeyup", @"value=value.replace(/[^-?\d+\.*\d*$]/g,'');Change('" + attr.FK_MapData + "');");
                                    tb.Attributes.Add("onkeyup", @"Change('" + attr.FK_MapData + "');");
                                    tb.Attributes.Add("onblur", @"value=value.replace(/[^-?\d+\.*\d*$]/g,'');TB_ClickNum(this,0);");
                                    tb.Attributes.Add("onClick", "TB_ClickNum(this)");
                                    tb.Attributes.Add("onkeydown", @"VirtyNum(this)");
                                    tb.Attributes["OnKeyPress"] += @"javascript:return  VirtyNum(this,'float');";
                                    tb.Attributes["class"] = "TBNum";
                                }
                                else
                                    tb.Attributes["class"] = "TBReadonly";

                                this.Add(tb);
                                break;
                            case BP.DA.DataType.AppInt:
                                // tb.ShowType = TBType.Num;
                                tb.Attributes["style"] = "width: " + attr.GetValStrByKey("UIWidth") + "px; text-align: right; height: 19px;word-break: keep-all;";
                                tb.Text = en.GetValStrByKey(attr.KeyOfEn);
                                if (attr.UIIsEnable && isReadonly == false)
                                {
                                    // Increased verification 
                                    tb.Attributes.Add("onkeyup", @"Change('" + attr.FK_MapData + "');");
                                    tb.Attributes.Add("onblur", @"value=value.replace(/[^-?\d]/g,'');TB_ClickNum(this,0);");
                                    tb.Attributes.Add("onClick", "TB_ClickNum(this)");
                                    tb.Attributes.Add("onkeydown", "VirtyNum(this)");
                                    tb.Attributes["OnKeyPress"] += @"javascript:return  VirtyNum(this,'int');";
                                    tb.Attributes["class"] = "TBNum";
                                }
                                else
                                    tb.Attributes["class"] = "TBReadonly";

                                this.Add(tb);
                                break;
                            case BP.DA.DataType.AppMoney:
                                tb.Attributes["style"] = "width: " + attr.GetValStrByKey("UIWidth") + "px; text-align: right; height: 19px;";

                                if (attr.UIIsEnable && isReadonly == false)
                                {
                                    // Increased verification 
                                    tb.Attributes.Add("onkeyup", @"Change('" + attr.FK_MapData + "');");
                                    tb.Attributes.Add("onblur", @"value=value.replace(/[^-?\d+\.*\d*$]/g,'');TB_ClickNum(this,'0.00');");
                                    tb.Attributes.Add("onClick", "TB_ClickNum(this)");
                                    tb.Attributes.Add("onkeydown", "VirtyNum(this)");
                                    tb.Attributes["OnKeyPress"] += @"javascript:return  VirtyNum(this,'float');";
                                    tb.Attributes["class"] = "TBNum";
                                }
                                else
                                    tb.Attributes["class"] = "TBReadonly";

                                if (SystemConfig.AppSettings["IsEnableNull"] == "1")
                                {
                                    decimal v = en.GetValMoneyByKey(attr.KeyOfEn);
                                    if (v == 567567567)
                                        tb.Text = "";
                                    else
                                        tb.Text = v.ToString("0.00");
                                }
                                else
                                {
                                    tb.Text = en.GetValMoneyByKey(attr.KeyOfEn).ToString("0.00");
                                }

                                this.Add(tb);
                                break;
                            case BP.DA.DataType.AppRate:
                                if (attr.UIIsEnable && isReadonly == false)
                                    tb.Attributes["class"] = "TBNum";
                                else
                                    tb.Attributes["class"] = "TBReadonly";
                                tb.ShowType = TBType.Moneny;
                                tb.Text = en.GetValMoneyByKey(attr.KeyOfEn).ToString("0.00");
                                tb.Attributes["style"] = "width: " + attr.GetValStrByKey("UIWidth") + "px; text-align: right; height: 19px;";
                                // Increased verification 
                                //tb.Attributes.Add("onkeyup", @"value=value.replace(/[^-?\d+\.*\d*$]/g,'')");
                                tb.Attributes.Add("onblur", @"value=value.replace(/[^-?\d+\.*\d*$]/g,'')");
                                this.Add(tb);
                                break;
                            default:
                                break;
                        }
                        break;
                    case FieldTypeS.Enum:
                        if (attr.UIContralType == UIContralType.DDL)
                        {
                            DDL ddle = new DDL();
                            ddle.ID = "DDL_" + attr.KeyOfEn;
                            ddle.BindSysEnum(attr.UIBindKey);
                            ddle.SetSelectItem(en.GetValStrByKey(attr.KeyOfEn));
                            ddle.Enabled = attr.UIIsEnable;
                            ddle.Attributes["tabindex"] = attr.IDX.ToString();
                            if (attr.UIIsEnable)
                            {
                                //add by dgq 2013-4-9, Event add content modified 
                                ddle.Attributes["onchange"] = "Change('" + attr.FK_MapData + "')";
                            }
                            if (ddle.Enabled == true && isReadonly == true)
                                ddle.Enabled = false;
                            this.Add(ddle);
                        }
                        else
                        {
                            //BP.Sys.FrmRBs rbs = new FrmRBs();
                            //rbs.Retrieve(FrmRBAttr.FK_MapData, enName,
                            //    FrmRBAttr.KeyOfEn, attr.KeyOfEn);
                        }
                        break;
                    case FieldTypeS.FK:
                        DDL ddl1 = new DDL();
                        ddl1.ID = "DDL_" + attr.KeyOfEn;
                        ddl1.Attributes["tabindex"] = attr.IDX.ToString();
                        ddl1.Enabled = attr.UIIsEnable;
                        if (ddl1.Enabled)
                        {
                            EntitiesNoName ens = attr.HisEntitiesNoName;
                            ens.RetrieveAll();
                            ddl1.BindEntities(ens);

                            ddl1.Items.Add(new ListItem(" Please select ", ""));

                            string val = en.GetValStrByKey(attr.KeyOfEn);
                            if (string.IsNullOrEmpty(val) == true)
                            {
                                ddl1.SetSelectItem("");
                            }
                            else
                                ddl1.SetSelectItem(val);

                            //add by dgq 2013-4-9, Event add content modified 
                            ddl1.Attributes["onchange"] = "Change('" + attr.FK_MapData + "')";
                        }
                        else
                        {
                            // ddl1.Attributes["style"] = "width: " + attr.UIWidth + "px;height: 19px;";
                            if (ddl1.Enabled == true && isReadonly == true)
                                ddl1.Enabled = false;
                            ddl1.Attributes["Width"] = attr.UIWidth.ToString();
                            ddl1.Items.Add(new ListItem(en.GetValRefTextByKey(attr.KeyOfEn), en.GetValStrByKey(attr.KeyOfEn)));
                        }

                        if (attr.UIIsEnable == true && this.IsReadonly == true)
                            ddl1.Enabled = false;
                        this.Add(ddl1);
                        break;
                    default:
                        break;
                }
                #endregion add contrals.

                this.Add("</span>");
                this.Add("</DIV>");
            }

            #region   Export  rb.
            BP.Sys.FrmRBs myrbs = this.mapData.FrmRBs;
            MapAttr attrRB = new MapAttr();
            foreach (BP.Sys.FrmRB rb in myrbs)
            {
                x = rb.X + wtX;
                this.Add("<DIV id='F" + rb.MyPK + "' style='position:absolute; left:" + x + "px; top:" + rb.Y + "px; width:100%; height:16px;text-align: left;word-break: keep-all;' >");
                this.Add("<span style='word-break: keep-all;font-size:12px;'>");

                System.Web.UI.WebControls.RadioButton rbCtl = new RadioButton();
                rbCtl.ID = "RB_" + rb.KeyOfEn + "_" + rb.IntKey.ToString();
                rbCtl.GroupName = rb.KeyOfEn;
                rbCtl.Text = rb.Lab;
                this.Add(rbCtl);

                if (attrRB.KeyOfEn != rb.KeyOfEn)
                {
                    foreach (MapAttr ma in mattrs)
                    {
                        if (ma.KeyOfEn == rb.KeyOfEn)
                        {
                            attrRB = ma;
                            break;
                        }
                    }
                }
                if (isReadonly == true || attrRB.UIIsEnable == false)
                    rbCtl.Enabled = false;
                else
                {
                    //add by dgq 2013-4-9, Event add content modified 
                    rbCtl.Attributes["onmousedown"] = "Change('" + attrRB.FK_MapData + "')";
                }
                this.Add("</span>");
                this.Add("</DIV>");
            }

            foreach (MapAttr attr in mattrs)
            {
                if (attr.UIContralType == UIContralType.RadioBtn)
                {
                    string id = "RB_" + attr.KeyOfEn + "_" + en.GetValStrByKey(attr.KeyOfEn);
                    RadioButton rb = this.GetRBLByID(id);
                    if (rb != null)
                        rb.Checked = true;
                }
            }
            #endregion   Export  rb.

            #endregion  Output data control .

            #region  Output details .
            foreach (MapDtl dtl in dtls)
            {
                if (dtl.IsView == false)
                    continue;

                  x = dtl.X + wtX;
                float y = dtl.Y;

                this.Add("<DIV id='Fd" + dtl.No + "' style='position:absolute; left:" + x+ "px; top:" + y + "px; width:" + dtl.W + "px; height:" + dtl.H + "px;text-align: left;' >");
                this.Add("<span>");

                string src = "";
                if (dtl.HisDtlShowModel == DtlShowModel.Table)
                {
                    if (isReadonly == true)
                        src = appPath + "WF/CCForm/Dtl.aspx?EnsName=" + dtl.No + "&RefPKVal=" + en.PKVal + "&IsReadonly=1&FID=" + en.GetValStrByKey("FID", "0") + "&FK_Node=" + this.FK_Node;
                    else
                        src = appPath + "WF/CCForm/Dtl.aspx?EnsName=" + dtl.No + "&RefPKVal=" + en.PKVal + "&IsReadonly=0&FID=" + en.GetValStrByKey("FID", "0") + "&FK_Node=" + this.FK_Node;
                }
                else
                {
                    if (isReadonly == true)
                        src = appPath + "WF/CCForm/DtlCard.aspx?EnsName=" + dtl.No + "&RefPKVal=" + en.PKVal + "&IsReadonly=1&FID=" + en.GetValStrByKey("FID", "0");
                    else
                        src = appPath + "WF/CCForm/DtlCard.aspx?EnsName=" + dtl.No + "&RefPKVal=" + en.PKVal + "&IsReadonly=0&FID=" + en.GetValStrByKey("FID", "0");
                }

                if (this.IsReadonly == true || dtl.IsReadonly)
                    this.Add("<iframe ID='F" + dtl.No + "' src='" + src +
                             "' frameborder=0  style='position:absolute;width:" + dtl.W + "px; height:" + dtl.H +
                             "px;text-align: left;'  leftMargin='0'  topMargin='0' scrolling=auto /></iframe>");
                else
                {

                    AddLoadFunction(dtl.No, "blur", "SaveDtl");

                    //this.Add("<iframe ID='F" + dtl.No + "' Onblur=\"SaveDtl('" + dtl.No + "');\"  src='" + src + "' frameborder=0  style='position:absolute;width:" + dtl.W + "px; height:" + dtl.H + "px;text-align: left;'  leftMargin='0'  topMargin='0' scrolling=auto /></iframe>");
                    this.Add("<iframe ID='F" + dtl.No + "' onload= '" + dtl.No + "load();'  src='" + src + "' frameborder=0  style='position:absolute;width:" + dtl.W + "px; height:" + dtl.H + "px;text-align: left;'  leftMargin='0'  topMargin='0' scrolling=auto /></iframe>");
                }

                this.Add("</span>");
                this.Add("</DIV>");
            }

            string js = "";
            if (this.IsReadonly == false)
            {
                js = "\t\n<script type='text/javascript' >";
                js += "\t\n function SaveDtl(dtl) { ";
                js += "\t\n   GenerPageKVs(); // Calls generated kvs ";
                js += "\t\n   document.getElementById('F' + dtl ).contentWindow.SaveDtlData();";
                js += "\t\n } ";

                js += "\t\n function SaveM2M(dtl) { ";
                js += "\t\n   document.getElementById('F' + dtl ).contentWindow.SaveM2M();";
                js += "\t\n } ";

                js += "\t\n</script>";
                this.Add(js);
            }
            #endregion  Output details .

            #region  Output Reports .
            foreach (FrmRpt rpt in md.FrmRpts)
            {
                if (rpt.IsView == false)
                    continue;

                x = rpt.X + wtX;
                float y = rpt.Y;

                this.Add("<DIV id='Fd" + rpt.No + "' style='position:absolute; left:" + x + "px; top:" + y + "px; width:" + rpt.W + "px; height:" + rpt.H + "px;text-align: left;' >");
                this.Add("<span>");

                string src = "";
                if (rpt.HisDtlShowModel == DtlShowModel.Table)
                {
                    if (isReadonly == true)
                        src = appPath + "WF/CCForm/Dtl.aspx?EnsName=" + rpt.No + "&RefPKVal=" + en.PKVal + "&IsReadonly=1&FID=" + en.GetValStrByKey("FID", "0");
                    else
                        src = appPath + "WF/CCForm/Dtl.aspx?EnsName=" + rpt.No + "&RefPKVal=" + en.PKVal + "&IsReadonly=0&FID=" + en.GetValStrByKey("FID", "0");
                }
                else
                {
                    if (isReadonly == true)
                        src = appPath + "WF/CCForm/DtlCard.aspx?EnsName=" + rpt.No + "&RefPKVal=" + en.PKVal + "&IsReadonly=1&FID=" + en.GetValStrByKey("FID", "0");
                    else
                        src = appPath + "WF/CCForm/DtlCard.aspx?EnsName=" + rpt.No + "&RefPKVal=" + en.PKVal + "&IsReadonly=0&FID=" + en.GetValStrByKey("FID", "0");
                }

                if (this.IsReadonly == true || rpt.IsReadonly)
                    this.Add("<iframe ID='F" + rpt.No + "' src='" + src +
                             "' frameborder=0  style='position:absolute;width:" + rpt.W + "px; height:" + rpt.H +
                             "px;text-align: left;'  leftMargin='0'  topMargin='0' scrolling=auto /></iframe>");
                else
                {
                    AddLoadFunction(rpt.No, "blur", "SaveDtl");

                    //this.Add("<iframe ID='F" + rpt.No + "' Onblur=\"SaveDtl('" + rpt.No + "');\"  src='" + src + "' frameborder=0  style='position:absolute;width:" + rpt.W + "px; height:" + rpt.H + "px;text-align: left;'  leftMargin='0'  topMargin='0' scrolling=auto /></iframe>");
                    this.Add("<iframe ID='F" + rpt.No + "' onload='" + rpt.No + "load();'  src='" + src + "' frameborder=0  style='position:absolute;width:" + rpt.W + "px; height:" + rpt.H + "px;text-align: left;'  leftMargin='0'  topMargin='0' scrolling=auto /></iframe>");
                }

                this.Add("</span>");
                this.Add("</DIV>");
            }
            #endregion  Output Reports .

            #region  Audit Components 
            FrmWorkCheck fwc = new FrmWorkCheck(enName);
            if (fwc.HisFrmWorkCheckSta != FrmWorkCheckSta.Disable)
            {
                x = fwc.FWC_X + wtX;
                this.Add("<DIV id='FWC" + fwc.No + "' style='position:absolute; left:" + x + "px; top:" + fwc.FWC_Y + "px; width:" + fwc.FWC_W + "px; height:" + fwc.FWC_H + "px;text-align: left;' >");
                this.Add("<span>");
                string src = appPath + "WF/WorkOpt/WorkCheck.aspx?s=2";
                string paras = this.RequestParas;
                try
                {
                    if (paras.Contains("FID=") == false)
                        paras += "&FID=" + this.HisEn.GetValStrByKey("FID");
                }
                catch
                {
                }
                if (paras.Contains("OID=") == false)
                    paras += "&OID=" + this.HisEn.GetValStrByKey("OID");
                if (fwc.HisFrmWorkCheckSta == FrmWorkCheckSta.Readonly)
                    src += "&DoType=View";
                src += "&r=q" + paras;
                this.Add("<iframe ID='F33" + fwc.No + "'  src='" + src + "' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='" + fwc.FWC_W + "' height='" + fwc.FWC_H + "'   scrolling=auto/></iframe>");
                this.Add("</span>");
                this.Add("</DIV>");
            }
            #endregion  Audit Components 

            #region  Many relationship 
            foreach (MapM2M m2m in m2ms)
            {
                x = m2m.X + wtX;
                this.Add("<DIV id='Fd" + m2m.NoOfObj + "' style='position:absolute; left:" +x  + "px; top:" + m2m.Y + "px; width:" + m2m.W + "px; height:" + m2m.H + "px;text-align: left;' >");
                this.Add("<span>");

                string src = ".aspx?NoOfObj=" + m2m.NoOfObj;
                string paras = this.RequestParas;
                try
                {
                    if (paras.Contains("FID=") == false)
                        paras += "&FID=" + this.HisEn.GetValStrByKey("FID");
                }
                catch
                {
                }

                if (paras.Contains("OID=") == false)
                    paras += "&OID=" + this.HisEn.GetValStrByKey("OID");
                src += "&r=q" + paras;
                if (m2m.IsEdit)
                    src += "&IsEdit=1";
                else
                    src += "&IsEdit=0";

                if (src.Contains("FK_MapData") == false)
                    src += "&FK_MapData=" + enName;

                if (m2m.HisM2MType == M2MType.M2MM)
                    src = appPath + "WF/CCForm/M2MM" + src;
                else
                    src = appPath + "WF/CCForm/M2M" + src;

                switch (m2m.ShowWay)
                {
                    case FrmShowWay.FrmAutoSize:
                    case FrmShowWay.FrmSpecSize:
                        if (m2m.IsEdit)
                        {
                            AddLoadFunction(m2m.NoOfObj, "blur", "SaveM2M");

                            // this.Add("<iframe ID='F" + m2m.NoOfObj + "'   Onblur=\"SaveM2M('" + m2m.NoOfObj + "');\"  src='" + src + "' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='" + m2m.W + "' height='" + m2m.H + "'   scrolling=auto/></iframe>");
                            this.Add("<iframe ID='F" + m2m.NoOfObj + "'  onload='" + m2m.NoOfObj + "load();'  src='" + src + "' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='" + m2m.W + "' height='" + m2m.H + "'   scrolling=auto/></iframe>");

                        }
                        else
                            this.Add("<iframe ID='F" + m2m.NoOfObj + "'  src='" + src + "' frameborder=0 style='padding:0px;border:0px;'  leftMargin='0'  topMargin='0' width='" + m2m.W + "' height='" + m2m.H + "'   scrolling=auto/></iframe>");
                        break;
                    case FrmShowWay.Hidden:
                        break;
                    case FrmShowWay.WinOpen:
                        this.Add("<a href=\"javascript:WinOpen('" + src + "&IsOpen=1','" + m2m.W + "','" + m2m.H + "');\"  />" + m2m.Name + "</a>");
                        break;
                    default:
                        break;
                }
                this.Add("</span>");
                this.Add("</DIV>");
            }
            #endregion  Many relationship 

            #region  Output Accessories 
            FrmAttachments aths = this.mapData.FrmAttachments;
            FrmAttachmentDBs athDBs = null;
            if (aths.Count > 0)
                athDBs = new FrmAttachmentDBs(enName, en.PKVal.ToString());

            foreach (FrmAttachment ath in aths)
            {
                if (ath.UploadType == AttachmentUploadType.Single)
                {
                    /*  Single file  */
                    FrmAttachmentDB athDB = athDBs.GetEntityByKey(FrmAttachmentDBAttr.FK_FrmAttachment, ath.MyPK) as FrmAttachmentDB;
                      x = ath.X + wtX;
                    float y = ath.Y;
                    this.Add("<DIV id='Fa" + ath.MyPK + "' style='position:absolute; left:" + x + "px; top:" + y + "px; text-align: left;float:left' >");
                    //  this.Add("<span>");
                    this.Add("<DIV>");
                    Label lab = new Label();
                    lab.ID = "Lab" + ath.MyPK;
                    this.Add(lab);
                    if (athDB != null)
                    {
                        //  lab.Text = "<img src='" + appPath + "WF/Img/FileType/" + athDB.FileExts + ".gif' border=0/>" + athDB.FileName;
                        if (ath.IsWoEnableWF)
                            lab.Text = "<a  href=\"javascript:OpenOfiice('" + athDB.FK_FrmAttachment + "','" + this.HisEn.GetValStrByKey("OID") + "','" + athDB.MyPK + "','" + this.FK_MapData + "','" + ath.NoOfObj + "','" + this.HisEn.GetValStrByKey("FK_Node") + "')\"><img src='" + BP.WF.Glo.CCFlowAppPath + "WF/Img/FileType/" + athDB.FileExts + ".gif' border=0/>" + athDB.FileName + "</a>";
                        else
                            lab.Text = "<img src='" + BP.WF.Glo.CCFlowAppPath + "WF/Img/FileType/" + athDB.FileExts + ".gif' border=0/>" + athDB.FileName;
                        // lab.Text = "<a href='" + this.Request.ApplicationPath + "DataUser/UploadFile/" + athDB.FilePathName + "' target=_blank ><img src='/WF/Img/FileType/" + athDB.FileExts + ".gif' border=0/>" + athDB.FileName + "</a>";
                    }
                    this.Add("</DIV>");
                    this.Add("<DIV>");
                    Button mybtn = new Button();
                    mybtn.CssClass = "Btn";

                    if (ath.IsUpload && this.IsReadonly == false)
                    {
                        mybtn.ID = ath.MyPK;
                        mybtn.Text = " Upload ";
                        mybtn.CssClass = "bg";
                        mybtn.ID = "Btn_Upload_" + ath.MyPK + "_" + this.HisEn.PKVal;
                        mybtn.Attributes["style"] = "display:none;";
                        mybtn.Click += new EventHandler(btnUpload_Click);
                        this.Add(mybtn);
                        FileUpload fu = new FileUpload();
                        fu.ID = ath.MyPK;
                        fu.Attributes["Width"] = ath.W.ToString();
                        string uploadName = "";
                        if (this.PageID == "Frm")
                            uploadName = "ContentPlaceHolder1_UCEn1_" + mybtn.ID;
                        else
                            uploadName = "ContentPlaceHolder1_MyFlowUC1_MyFlow1_UCEn1_" + mybtn.ID;
                        fu.Attributes["onchange"] = "UploadChange('" + uploadName + "');";
                        this.Add(fu);
                    }
                    if (ath.IsDownload)
                    {
                        mybtn = new Button();
                        mybtn.Text = " Download ";
                        mybtn.CssClass = "Btn";

                        mybtn.ID = "Btn_Download_" + ath.MyPK + "_" + this.HisEn.PKVal;
                        mybtn.Click += new EventHandler(btnUpload_Click);
                        mybtn.CssClass = "bg";
                        if (athDB == null)
                            mybtn.Visible = false;
                        else
                            mybtn.Visible = true;
                        this.Add(mybtn);
                    }

                    if (this.IsReadonly == false)
                    {
                        if (ath.IsDelete)
                        {
                            mybtn = new Button();
                            mybtn.CssClass = "Btn";
                            mybtn.Text = " Delete ";
                            mybtn.Attributes["onclick"] = " return confirm(' Are you sure you want to delete it ?');";
                            mybtn.ID = "Btn_Delete_" + ath.MyPK + "_" + this.HisEn.PKVal;
                            mybtn.Click += new EventHandler(btnUpload_Click);
                            mybtn.CssClass = "bg";
                            if (athDB == null)
                                mybtn.Visible = false;
                            else
                                mybtn.Visible = true;
                            this.Add(mybtn);


                        }
                        if (ath.IsWoEnableWF)
                        {
                            mybtn = new Button();
                            mybtn.CssClass = "Btn";
                            mybtn.Text = " Editor ";
                            mybtn.ID = "Btn_Open_" + ath.MyPK + "_" + this.HisEn.PKVal;
                            mybtn.Click += new EventHandler(btnUpload_Click);
                            mybtn.CssClass = "bg";
                            if (athDB == null)
                                mybtn.Visible = false;
                            else
                                mybtn.Visible = true;
                            this.Add(mybtn);
                        }
                    }

                    this.Add("</DIV>");
                    this.Add("</DIV>");
                }

                if (ath.UploadType == AttachmentUploadType.Multi)
                {
                    x = ath.X + wtX;
                    this.Add("<DIV id='Fd" + ath.MyPK + "' style='position:absolute; left:" + x + "px; top:" + ath.Y + "px; width:" + ath.W + "px; height:" + ath.H + "px;text-align: left;' >");
                    this.Add("<span>");
                    string src = "";
                    if (this.IsReadonly)
                        src = appPath + "WF/CCForm/AttachmentUpload.aspx?PKVal=" + this.HisEn.PKVal.ToString() + "&Ath=" + ath.NoOfObj + "&FK_FrmAttachment=" + ath.MyPK + "&IsReadonly=1" + this.RequestParas;
                    else
                        src = appPath + "WF/CCForm/AttachmentUpload.aspx?PKVal=" + this.HisEn.PKVal.ToString() + "&Ath=" + ath.NoOfObj + "&FK_FrmAttachment=" + ath.MyPK + this.RequestParas;

                    this.Add("<iframe ID='F" + ath.MyPK + "'    src='" + src + "' frameborder=0  style='position:absolute;width:" + ath.W + "px; height:" + ath.H + "px;text-align: left;'  leftMargin='0'  topMargin='0' scrolling=auto /></iframe>");
                    this.Add("</span>");
                    this.Add("</DIV>");
                }
            }
            #endregion  Output Accessories .

            #region  Export  img  Accessory 
            FrmImgAths imgAths = this.mapData.FrmImgAths;
            if (imgAths.Count != 0 && this.IsReadonly == false)
            {
                js = "\t\n<script type='text/javascript' >";
                js += "\t\n function ImgAth(url,athMyPK)";
                js += "\t\n {";
                js += "\t\n  var v= window.showModalDialog(url, 'ddf', 'dialogHeight: 650px; dialogWidth: 950px;center: yes; help: no'); ";
                js += "\t\n  if (v==null )  ";
                js += "\t\n     return ;";
                js += "\t\n document.getElementById('Img'+athMyPK ).setAttribute('src', v); ";
                js += "\t\n }";
                js += "\t\n</script>";
                this.Add(js);
            }

            foreach (FrmImgAth ath in imgAths)
            {
                x = ath.X + wtX;
                this.Add("\t\n<DIV id=" + ath.MyPK + " style='position:absolute;left:" + x + "px;top:" + ath.Y + "px;text-align:left;vertical-align:top' >");
                string url = appPath + "WF/CCForm/ImgAth.aspx?W=" + ath.W + "&H=" + ath.H + "&FK_MapData=" + enName + "&MyPK=" + en.PKVal + "&ImgAth=" + ath.MyPK;
                if (isReadonly == false && ath.IsEdit == true)
                    this.AddFieldSet("<a href=\"javascript:ImgAth('" + url + "','" + ath.MyPK + "');\" > Editor </a>");

                FrmImgAthDB imgAthDb = new FrmImgAthDB();
                imgAthDb.MyPK = ath.MyPK + "_" + en.PKVal;
                imgAthDb.RetrieveFromDBSources();
                if (imgAthDb != null && !string.IsNullOrEmpty(imgAthDb.FileName))
                {
                    this.Add("\t\n<img src='" + appPath + "DataUser/ImgAth/Data/" + imgAthDb.FileName + ".png' onerror=\"this.src='" + appPath + "WF/Data/Img/LogH.PNG'\" name='Img" + ath.MyPK + "' id='Img" + ath.MyPK + "' style='padding: 0px;margin: 0px;border-width: 0px;' width=" + ath.W + " height=" + ath.H + " />");
                }
                else
                {
                    this.Add("\t\n<img src='" + appPath + "DataUser/ImgAth/Data/" + ath.MyPK + "_" + en.PKVal + ".png' onerror=\"this.src='" + appPath + "WF/Data/Img/LogH.PNG'\" name='Img" + ath.MyPK + "' id='Img" + ath.MyPK + "' style='padding: 0px;margin: 0px;border-width: 0px;' width=" + ath.W + " height=" + ath.H + " />");
                }
                if (isReadonly == false && ath.IsEdit == true)
                    this.AddFieldSetEnd();
                this.Add("\t\n</DIV>");
            }
            #endregion  Output Accessories .

            //  Processing extensions .
            if (isReadonly == false)
                this.AfterBindEn_DealMapExt(enName, mattrs, en);
            return;
        }

        void AddLoadFunction(string id, string eventName, string method)
        {
            string js = "";
            js = "\t\n<script type='text/javascript' >";
            js += "\t\n function " + id + "load() { ";
            js += "\t\n   if (document.all) {";
            js += "\t\n     document.getElementById('F" + id + "').attachEvent('on" + eventName + "',function(event){" + method + "('" + id + "');});";
            js += "\t\n } ";

            js += "\t\n else { ";
            js += "\t\n  document.getElementById('F" + id + "').contentWindow.addEventListener('" + eventName + "',function(event){" + method + "('" + id + "');}, false); ";
            js += "\t\n } }";

            js += "\t\n</script>";
            this.Add(js);

        }
        void btnUpload_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string[] ids = btn.ID.Split('_');
            //string athPK = ids[2] + "_" + ids[3] ;

            string doType = ids[1];

            string athPK = btn.ID.Replace("Btn_" + doType + "_", "");
            athPK = athPK.Substring(0, athPK.LastIndexOf('_'));

            string athDBPK = athPK + "_" + this.HisEn.PKVal.ToString();
            FrmAttachment frmAth = new FrmAttachment();
            frmAth.MyPK = athPK;
            frmAth.RetrieveFromDBSources();

            string pkVal = this.HisEn.PKVal.ToString();
            switch (doType)
            {
                case "Delete":
                    FrmAttachmentDB db = new FrmAttachmentDB();
                    db.MyPK = athDBPK;
                    int id = db.Delete();
                    if (id == 0)
                        throw new Exception("@ Success is not deleted .");
                    try
                    {
                        Button btnDel = this.GetButtonByID("Btn_Delete_" + athDBPK);
                        btnDel.Visible = false;

                        btnDel = this.GetButtonByID("Btn_Download_" + athDBPK);
                        btnDel.Visible = false;

                        btnDel = this.GetButtonByID("Btn_Open_" + athDBPK);
                        btnDel.Visible = false;
                    }
                    catch (Exception ex)
                    {

                    }
                    Label lab1 = this.GetLabelByID("Lab" + frmAth.MyPK);
                    lab1.Text = "";
                    break;
                case "Upload":
                    FileUpload fu = this.FindControl(athPK) as FileUpload;
                    if (fu.HasFile == false || fu.FileName.Length <= 2)
                    {
                        this.Alert(" Please select a file to upload .");
                        return;
                    }

                    // Check the format to meet the requirements .
                    if (frmAth.Exts == "" || frmAth.Exts == "*.*")
                    {
                        /* Any format can be uploaded .*/
                    }
                    else
                    {
                        string fileExt = fu.FileName.Substring(fu.FileName.LastIndexOf('.') + 1);
                        fileExt = fileExt.ToLower().Replace(".", "");
                        if (frmAth.Exts.ToLower().Contains(fileExt) == false)
                        {
                            this.Alert(" You upload a file format does not meet the requirements , Requirements format :" + frmAth.Exts);
                            return;
                        }
                    }

                    // Processing save path .
                    string saveTo = frmAth.SaveTo;



                    if (saveTo.Contains("*") || saveTo.Contains("@"))
                    {
                        /* If there is a path variable .*/
                        saveTo = saveTo.Replace("*", "@");
                        saveTo = BP.WF.Glo.DealExp(saveTo, this.HisEn, null);
                    }

                    try
                    {
                        saveTo = Server.MapPath("~/" + saveTo);


                    }
                    catch (Exception)
                    {

                        saveTo = saveTo;
                    }

                    if (System.IO.Directory.Exists(saveTo) == false)
                        System.IO.Directory.CreateDirectory(saveTo);

                    saveTo = saveTo + "\\" + athDBPK + "." + fu.FileName.Substring(fu.FileName.LastIndexOf('.') + 1);
                    fu.SaveAs(saveTo);


                    FileInfo info = new FileInfo(saveTo);
                    FrmAttachmentDB dbUpload = new FrmAttachmentDB();
                    dbUpload.MyPK = athDBPK;
                    dbUpload.FK_FrmAttachment = athPK;
                    dbUpload.RefPKVal = this.HisEn.PKVal.ToString();
                    if (this.EnName == null)
                        dbUpload.FK_MapData = this.HisEn.ToString();
                    else
                        dbUpload.FK_MapData = this.EnName;

                    dbUpload.FileExts = info.Extension;
                    dbUpload.FileFullName = saveTo;
                    dbUpload.FileName = fu.FileName;
                    dbUpload.FileSize = (float)info.Length;
                    dbUpload.Rec = WebUser.No;
                    dbUpload.RecName = WebUser.Name;
                    dbUpload.RDT = BP.DA.DataType.CurrentDataTime;

                    if (this.Request.QueryString["FK_Node"] != null)
                        dbUpload.NodeID = this.Request.QueryString["FK_Node"];

                    dbUpload.Save();

                    Button myBtnDel = this.GetButtonByID("Btn_Delete_" + athDBPK);
                    if (myBtnDel != null)
                    {
                        myBtnDel.Visible = true;
                        myBtnDel = this.GetButtonByID("Btn_Download_" + athDBPK);
                        myBtnDel.Visible = true;
                    }

                    Button myBtnOpen = this.GetButtonByID("Btn_Open_" + athDBPK);

                    if (myBtnOpen != null)
                    {
                        myBtnOpen.Visible = true;
                        myBtnOpen = this.GetButtonByID("Btn_Download_" + athDBPK);
                        myBtnOpen.Visible = true;
                    }

                    Label lab = this.GetLabelByID("Lab" + frmAth.MyPK);
                    if (lab != null)
                    {
                        if (frmAth.IsWoEnableWF)
                            lab.Text = "<a href=\"javascript:OpenOfiice('" + dbUpload.FK_FrmAttachment + "','" + this.HisEn.GetValStrByKey("OID") + "','" + dbUpload.MyPK + "','" + this.FK_MapData + "','" + frmAth.NoOfObj + "','" + this.FK_Node + "')\"><img src='" + BP.WF.Glo.CCFlowAppPath + "WF/Img/FileType/" + dbUpload.FileExts + ".gif' border=0/>" + dbUpload.FileName + "</a>";
                        else
                            lab.Text = "<img src='" + BP.WF.Glo.CCFlowAppPath + "WF/Img/FileType/" + dbUpload.FileExts + ".gif' border=0/>" + dbUpload.FileName;
                    }
                    return;
                case "Download":
                    FrmAttachmentDB dbDown = new FrmAttachmentDB();
                    dbDown.MyPK = athDBPK;
                    if (dbDown.RetrieveFromDBSources() == 0)
                    {
                        dbDown.Retrieve(FrmAttachmentDBAttr.FK_MapData, this.HisEn.ClassID,
                            FrmAttachmentDBAttr.RefPKVal, this.HisEn.PKVal.ToString(), FrmAttachmentDBAttr.FK_FrmAttachment, frmAth.FK_MapData + "_" + frmAth.NoOfObj);
                    }
                    string downPath = GetRealPath(dbDown.FileFullName);
                    PubClass.DownloadFile(downPath, dbDown.FileName);
                    break;
                case "Open":
                    var url = CCFlowAppPath + "WF/WebOffice/AttachOffice.aspx?DoType=EditOffice&DelPKVal=" + athDBPK + "&FK_FrmAttachment=" + frmAth.MyPK + "&PKVal=" + pkVal + "&FK_Node=" + this.HisEn.GetValStringByKey("FK_Node") + "&FK_MapData=" + frmAth.FK_MapData + "&NoOfObj=" + frmAth.NoOfObj;
                    PubClass.WinOpen(url, "WebOffice Editor ", 850, 600);
                    break;
                default:
                    break;
            }
        }

        private string GetRealPath(string fileFullName)
        {
            bool isFile = false;
            string downpath = "";
            try
            {
                // If a relative path may get less storage is the absolute path 
                FileInfo downInfo = new FileInfo(Server.MapPath("~/" + fileFullName));
                isFile = true;
                downpath = Server.MapPath("~/" + fileFullName);
            }
            catch (Exception)
            {
                FileInfo downInfo = new FileInfo(fileFullName);
                isFile = true;
                downpath = fileFullName;
            }
            if (!isFile)
            {
                throw new Exception(" Did not find the downloaded file path !");
            }

            return downpath;
        }

        bool CanEditor(string fileType)
        {
            try
            {
                string fileTypes = BP.Sys.SystemConfig.AppSettings["OpenTypes"];

                if (fileTypes.Contains(fileType))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }
        void myBtn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            FrmBtn mybtn = new FrmBtn(btn.ID);
            string doc = mybtn.EventContext.Replace("~", "'");

            Attrs attrs = this.HisEn.EnMap.Attrs;
            foreach (Attr attr in attrs)
            {
                doc = doc.Replace("@" + attr.Key, this.HisEn.GetValStrByKey(attr.Key));
            }
            doc = doc.Replace("@FK_Dept", WebUser.FK_Dept);
            doc = doc.Replace("@WebUser.FK_Dept", WebUser.FK_Dept);
            doc = doc.Replace("@WebUser.FK_DeptName", WebUser.FK_DeptName);
            doc = doc.Replace("@WebUser.No", WebUser.No);
            doc = doc.Replace("@WebUser.Name", WebUser.Name);
            doc = doc.Replace("@MyPK", this.HisEn.PKVal.ToString());

            #region  Handling two variables .
            string alertMsgErr = mybtn.MsgErr;
            string alertMsgOK = mybtn.MsgOK;
            if (alertMsgOK.Contains("@"))
            {
                foreach (Attr attr in attrs)
                    alertMsgOK = alertMsgOK.Replace("@" + attr.Key, this.HisEn.GetValStrByKey(attr.Key));
            }

            if (alertMsgErr.Contains("@"))
            {
                foreach (Attr attr in attrs)
                    alertMsgErr = alertMsgErr.Replace("@" + attr.Key, this.HisEn.GetValStrByKey(attr.Key));
            }
            #endregion  Handling two variables .

            try
            {
                switch (mybtn.HisBtnEventType)
                {
                    case BtnEventType.RunSQL:
                        DBAccess.RunSQL(doc);
                        this.Alert(alertMsgOK);
                        return;
                    case BtnEventType.RunSP:
                        DBAccess.RunSP(doc);
                        this.Alert(alertMsgOK);
                        return;
                    case BtnEventType.RunURL:
                        doc = doc.Replace("@AppPath", BP.WF.Glo.CCFlowAppPath);
                        string text = DataType.ReadURLContext(doc, 800, System.Text.Encoding.UTF8);
                        if (text != null && text.Substring(0, 7).Contains("Err"))
                            throw new Exception(text);
                        alertMsgOK += text;
                        this.Alert(alertMsgOK);
                        return;
                    default:
                        throw new Exception(" Execution type untreated :" + mybtn.HisBtnEventType);
                }
            }
            catch (Exception ex)
            {
                this.Alert(alertMsgErr + ex.Message);
            }

            #region  Handling button events .
            #endregion
        }
        #endregion

        public static string GetRefstrs(string keys, Entity en, Entities hisens)
        {
            string refstrs = "";
            string appPath = BP.WF.Glo.CCFlowAppPath;// System.Web.HttpContext.Current.Request.ApplicationPath;
            int i = 0;

            #region  Join many of the entity editor 
            AttrsOfOneVSM oneVsM = en.EnMap.AttrsOfOneVSM;
            if (oneVsM.Count > 0)
            {
                foreach (AttrOfOneVSM vsM in oneVsM)
                {
                    //  string url = path + "/Comm/UIEn1ToM.aspx?EnsName=" + en.ToString() + "&AttrKey=" + vsM.EnsOfMM.ToString() + keys;
                    string url = "UIEn1ToM.aspx?EnsName=" + en.ToString() + "&AttrKey=" + vsM.EnsOfMM.ToString() + keys;
                    try
                    {
                        try
                        {
                            i = DBAccess.RunSQLReturnValInt("SELECT COUNT(*)  as NUM FROM " + vsM.EnsOfMM.GetNewEntity.EnMap.PhysicsTable + " WHERE " + vsM.AttrOfOneInMM + "='" + en.PKVal + "'");
                        }
                        catch
                        {
                            i = DBAccess.RunSQLReturnValInt("SELECT COUNT(*)  as NUM FROM " + vsM.EnsOfMM.GetNewEntity.EnMap.PhysicsTable + " WHERE " + vsM.AttrOfOneInMM + "=" + en.PKVal);
                        }
                    }
                    catch (Exception ex)
                    {
                        vsM.EnsOfMM.GetNewEntity.CheckPhysicsTable();
                        throw ex;
                    }

                    if (i == 0)
                        refstrs += "[<a href=\"javascript:WinShowModalDialog('" + url + "','onVsM'); \"  >" + vsM.Desc + "</a>]";
                    else
                        refstrs += "[<a href=\"javascript:WinShowModalDialog('" + url + "','onVsM'); \"  >" + vsM.Desc + "-" + i + "</a>]";
                }
            }
            #endregion

            #region  Joined his door   Method 
            RefMethods myreffuncs = en.EnMap.HisRefMethods;
            if (myreffuncs.Count > 0)
            {
                foreach (RefMethod func in myreffuncs)
                {
                    if (func.Visable == false)
                        continue;

                    // string url = path + "/Comm/RefMethod.aspx?Index=" + func.Index + "&EnsName=" + hisens.ToString() + keys;
                    string url = appPath + "WF/Comm/RefMethod.aspx?Index=" + func.Index + "&EnsName=" + hisens.ToString() + keys;
                    if (func.Warning == null)
                    {
                        if (func.Target == null)
                            refstrs += "[" + func.GetIcon(appPath) + "<a href='" + url + "' ToolTip='" + func.ToolTip + "' >" + func.Title + "</a>]";
                        else
                            refstrs += "[" + func.GetIcon(appPath) + "<a href=\"javascript:WinOpen('" + url + "','" + func.Target + "')\" ToolTip='" + func.ToolTip + "' >" + func.Title + "</a>]";
                    }
                    else
                    {
                        if (func.Target == null)
                            refstrs += "[" + func.GetIcon(appPath) + "<a href=\"javascript: if ( confirm('" + func.Warning + "') ) { window.location.href='" + url + "' }\" ToolTip='" + func.ToolTip + "' >" + func.Title + "</a>]";
                        else
                            refstrs += "[" + func.GetIcon(appPath) + "<a href=\"javascript: if ( confirm('" + func.Warning + "') ) { WinOpen('" + url + "','" + func.Target + "') }\" ToolTip='" + func.ToolTip + "' >" + func.Title + "</a>]";
                    }
                }
            }
            #endregion

            #region  He added detail 
            EnDtls enDtls = en.EnMap.Dtls;
            //  string path = this.Request.ApplicationPath;
            if (enDtls.Count > 0)
            {
                foreach (EnDtl enDtl in enDtls)
                {
                    //string url = path + "/Comm/UIEnDtl.aspx?EnsName=" + enDtl.EnsName + "&Key=" + enDtl.RefKey + "&Val=" + en.PKVal.ToString() + "&MainEnsName=" + en.ToString() + keys;
                    string url = appPath + "Comm/UIEnDtl.aspx?EnsName=" + enDtl.EnsName + "&RefKey=" + enDtl.RefKey + "&RefVal=" + en.PKVal.ToString() + "&MainEnsName=" + en.ToString();
                    try
                    {
                        try
                        {
                            i = DBAccess.RunSQLReturnValInt("SELECT COUNT(*) FROM " + enDtl.Ens.GetNewEntity.EnMap.PhysicsTable + " WHERE " + enDtl.RefKey + "='" + en.PKVal + "'");
                        }
                        catch
                        {
                            i = DBAccess.RunSQLReturnValInt("SELECT COUNT(*) FROM " + enDtl.Ens.GetNewEntity.EnMap.PhysicsTable + " WHERE " + enDtl.RefKey + "=" + en.PKVal);
                        }
                    }
                    catch (Exception ex)
                    {
                        enDtl.Ens.GetNewEntity.CheckPhysicsTable();
                        throw ex;
                    }

                    if (i == 0)
                        refstrs += "[<a href=\"javascript:WinOpen('" + url + "', 'dtl" + enDtl.RefKey + "'); \" >" + enDtl.Desc + "</a>]";
                    else
                        refstrs += "[<a href=\"javascript:WinOpen('" + url + "', 'dtl" + enDtl.RefKey + "'); \"  >" + enDtl.Desc + "-" + i + "</a>]";
                }
            }
            #endregion

            return refstrs;
        }
        public UCEn()
        {
        }
        public void AddContral()
        {
            this.Controls.Add(new LiteralControl("<td class='FDesc' nowrap width=1% ></td>"));
            this.Controls.Add(new LiteralControl("<td></TD>"));
        }
        public void AddContral(string desc, CheckBox cb)
        {
            this.Controls.Add(new LiteralControl("<td class='FDesc' nowrap width=1% > " + desc + "</td>"));
            this.Controls.Add(new LiteralControl("<td>"));
            this.Controls.Add(cb);
            this.Controls.Add(new LiteralControl("</td>"));
        }
        public void AddContral(string desc, CheckBox cb, int colspan)
        {
            this.Controls.Add(new LiteralControl("<td class='FDesc' nowrap width=1% > " + desc + "</td>"));
            this.Controls.Add(new LiteralControl("<td  colspan='" + colspan + "'>"));
            this.Controls.Add(cb);
            this.Controls.Add(new LiteralControl("</td>"));
        }
        //		public void AddContral(string desc, string val)
        public void AddContral(string desc, string val)
        {
            this.Add("<TD class='FDesc' > " + desc + "</TD>");
            this.Add("<TD>" + val + "</TD>");
        }
        public void AddContral(string desc, TB tb, string helpScript)
        {
            if (tb.ReadOnly)
            {
                if (tb.Attributes["Class"] == "TBNum")
                    tb.Attributes["Class"] = "TBNumReadonly";
                else
                    tb.Attributes["Class"] = "TBReadonly";
            }

            tb.Attributes["style"] = "width=500px;height=100%";
            if (tb.TextMode == TextBoxMode.MultiLine)
            {
                AddContralDoc(desc, tb);
                return;
            }

            tb.Attributes["Width"] = "80%";

            this.Add("<td class='FDesc' nowrap width=1% >" + desc + "</td>");
            this.Add("<td >" + helpScript);
            this.Add(tb);
            this.AddTDEnd();
        }
        public void AddContral(string desc, TB tb, string helpScript, int colspan)
        {
            if (tb.ReadOnly)
            {
                if (tb.Attributes["Class"] == "TBNum")
                    tb.Attributes["Class"] = "TBNumReadonly";
                else
                    tb.Attributes["Class"] = "TBReadonly";
            }

            tb.Attributes["style"] = "width=100%;height=100%";
            if (tb.TextMode == TextBoxMode.MultiLine)
            {
                AddContralDoc(desc, tb);
                return;
            }
            this.Add("<td class='FDesc' nowrap width=1% >" + desc + "</td>");
            if (colspan < 3)
            {
                this.Add("<td  colspan=" + colspan + " width='30%' >" + helpScript);
            }
            else
            {
                this.Add("<td  colspan=" + colspan + " width='80%' >" + helpScript);
            }
            this.Add(tb);
            this.AddTDEnd(); // ("</td>");
        }
        public void AddContral(string desc, TB tb, int colSpanOfCtl)
        {
            if (tb.ReadOnly)
            {
                if (tb.Attributes["Class"] == "TBNum")
                    tb.Attributes["Class"] = "TBNumReadonly";
                else
                    tb.Attributes["Class"] = "TBReadonly";
            }

            tb.Attributes["style"] = "width=100%;height=100%";
            if (tb.TextMode == TextBoxMode.MultiLine)
            {
                AddContralDoc(desc, tb, colSpanOfCtl);
                return;
            }

            this.Add("<td class='FDesc' nowrap width=1% > " + desc + "</td>");

            if (colSpanOfCtl < 3)
                this.Add("<td  colspan=" + colSpanOfCtl + " width='30%' >");
            else
                this.Add("<td  colspan=" + colSpanOfCtl + " width='80%' >");

            this.Add(tb);
            this.AddTDEnd();
        }
        /// <summary>
        ///  Increase air pieces 
        /// </summary>
        /// <param name="desc"></param>
        /// <param name="tb"></param>
        public void AddContral(string desc, TB tb)
        {
            if (tb.ReadOnly)
            {
                if (tb.Attributes["Class"] == "TBNum")
                    tb.Attributes["Class"] = "TBNumReadonly";
                else
                    tb.Attributes["Class"] = "TBReadonly";
            }

            //if (tb.ReadOnly == false)
            //    desc += "<font color=red><b>*</b></font>";

            tb.Attributes["style"] = "width=100%";
            if (tb.TextMode == TextBoxMode.MultiLine)
            {
                AddContralDoc(desc, tb);
                return;
            }

            this.Add("<td class='FDesc' nowrap width=1% > " + desc + "</td>");

            this.Add("<td  width='30%'>");
            this.Add(tb);
            this.AddTDEnd(); // ("</td>");
        }
        //		public void AddContralDoc(string desc, TB tb)
        public void AddContralDoc(string desc, TB tb)
        {
            //if (desc.Length>
            this.Add("<td class='FDesc'  colspan='2' nowrap height='100px' width='50%' >" + desc + "<br>");
            if (tb.ReadOnly)
                tb.Attributes["Class"] = "TBReadonly";
            this.Add(tb);
            this.Add("</td>");
        }
        public void AddContralDoc(string desc, TB tb, int colspanOfctl)
        {
            //if (desc.Length>
            this.Add("<td class='FDesc'  colspan='" + colspanOfctl + "' nowrap height='100px' width='50%' >" + desc + "<br>");
            if (tb.ReadOnly)
                tb.Attributes["Class"] = "TBReadonly";
            this.Add(tb);
            this.Add("</td>");
        }
        public void AddContralDoc(string desc, int colspan, TB tb)
        {
            this.Add("<td class='FDesc'  colspan='" + colspan + "' nowrap width=1%  height='100px'  >" + desc + "<br>");
            if (tb.ReadOnly)
                tb.EnsName = "TBReadonly";
            this.Add(tb);
            this.Add("</td>");
        }

        #region  Method 
        public bool IsReadonly
        {
            get
            {
                string s = this.ViewState["IsReadonly"] as string;
                if (s == "1")
                    return true;
                return false;
            }
            set
            {
                if (value)
                    ViewState["IsReadonly"] = "1";
                else
                    ViewState["IsReadonly"] = "0";
            }
        }
        public bool IsShowDtl
        {
            get
            {
                return (bool)this.ViewState["IsShowDtl"];
            }
            set
            {
                ViewState["IsShowDtl"] = value;
            }
        }
        public void SetValByKey(string key, string val)
        {
            TB tb = new TB();
            tb.ID = "TB_" + key;
            tb.Text = val;
            tb.Visible = false;
            this.Controls.Add(tb);
        }
        public object GetValByKey(string key)
        {
            TB en = (TB)this.FindControl("TB_" + key);
            return en.Text;
        }

        public void BindReadonly(Entity en)
        {
            this.HisEn = en;
            //this.IsReadonly = isReadonly;
            //this.IsShowDtl = isShowDtl;
            this.Attributes["visibility"] = "hidden";
            this.Controls.Clear();
            this.AddTable(); //("<table   width='100%' id='AutoNumber1'  border='1' cellpadding='0' cellspacing='0' style='border-collapse: collapse' bordercolor='#111111' >");
            bool isLeft = true;
            object val = null;
            bool isAddTR = true;
            foreach (Attr attr in en.EnMap.Attrs)
            {
                if (isLeft && isAddTR)
                {
                    this.Add("<tr>");
                }
                isAddTR = true;
                val = en.GetValByKey(attr.Key);
                if (attr.UIContralType == UIContralType.TB)
                {
                    if (attr.MyFieldType == FieldType.RefText)
                    {
                        this.AddContral(attr.Desc, val.ToString().ToString());
                        isAddTR = false;
                        continue;
                    }
                    else if (attr.MyFieldType == FieldType.MultiValues)
                    {
                        /*  If it is more than the value of .*/
                        LB lb = new LB(attr);
                        lb.Visible = true;
                        lb.Height = 128;
                        lb.SelectionMode = ListSelectionMode.Multiple;
                        Entities ens = ClassFactory.GetEns(attr.UIBindKey);
                        ens.RetrieveAll();
                        this.Controls.Add(lb);
                    }
                    else
                    {
                        if (attr.UIVisible == false)
                        {
                            this.SetValByKey(attr.Key, val.ToString());
                            continue;
                        }
                        else
                        {

                            if (attr.UIHeight != 0)
                            {
                                this.AddContral(attr.Desc, val.ToString());
                            }
                            else
                            {

                                switch (attr.MyDataType)
                                {
                                    case DataType.AppMoney:
                                        //this.AddContral(attr.Desc, val.ToString().ToString("0.00")  );
                                        break;
                                    default:
                                        this.AddContral(attr.Desc, val.ToString());
                                        break;
                                }
                            }
                        }

                    }
                }
                else if (attr.UIContralType == UIContralType.CheckBok)
                {
                    if (en.GetValBooleanByKey(attr.Key))
                        this.AddContral(attr.Desc, "是");
                    else
                        this.AddContral(attr.Desc, "否");
                }
                else if (attr.UIContralType == UIContralType.DDL)
                {
                    this.AddContral(attr.Desc, val.ToString());
                }
                else if (attr.UIContralType == UIContralType.RadioBtn)
                {
                    //					Sys.SysEnums enums = new BP.Sys.SysEnums(attr.UIBindKey); 
                    //					foreach(SysEnum en in enums)
                    //					{
                    //						return ;
                    //					}
                }

                if (isLeft == false)
                    this.AddTREnd();

                isLeft = !isLeft;
            } //  End loop .

            this.Add("</TABLE>");



            if (en.IsExit(en.PK, en.PKVal) == false)
                return;

            string refstrs = "";
            if (en.IsEmpty)
            {
                refstrs += "";
                return;
            }

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
            refstrs = GetRefstrs(keys, en, en.GetNewEntities);
            if (refstrs != "")
                refstrs += "<hr>";
            this.Add(refstrs);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="en"></param>
        /// <param name="isReadonly"></param>
        /// <param name="isShowDtl"></param>
        private void btn_Click(object sender, EventArgs e)
        {
        }

        public Entity GetEnData(Entity en)
        {
            try
            {

                string s = null;
                foreach (Attr attr in en.EnMap.Attrs)
                {
                    if (attr.MyFieldType == FieldType.RefText)
                        continue;

                    if (attr.Key == "MyNum")
                    {
                        en.SetValByKey(attr.Key, 1);
                        continue;
                    }

                    switch (attr.UIContralType)
                    {
                        case UIContralType.TB:
                            if (attr.UIVisible)
                            {
                                if (attr.UIHeight == 0)
                                {
                                    //  Handle special characters .
                                    s = this.GetTBByID("TB_" + attr.Key).Text;
                                    en.SetValByKey(attr.Key, s);
                                    continue;
                                }
                                else
                                {
                                    if (this.IsExit("TB_" + attr.Key))
                                    {
                                        //  Handle special characters .
                                        s = this.GetTBByID("TB_" + attr.Key).Text;
                                        en.SetValByKey(attr.Key, s);
                                        continue;
                                    }

                                    if (this.IsExit("TBH_" + attr.Key))
                                    {
                                        HtmlInputHidden input = (HtmlInputHidden)this.FindControl("TBH_" + attr.Key);
                                        en.SetValByKey(attr.Key, input.Value);
                                        continue;
                                    }

                                    if (this.IsExit("TBF_" + attr.Key))
                                    {
                                        //FredCK.FCKeditorV2.FCKeditor fck = (FredCK.FCKeditorV2.FCKeditor)this.FindControl("TB_" + attr.Key);
                                        //en.SetValByKey(attr.Key, fck.Value);
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                en.SetValByKey(attr.Key, this.GetValByKey(attr.Key));
                            }
                            break;
                        case UIContralType.DDL:
                            en.SetValByKey(attr.Key, this.GetDDLByKey("DDL_" + attr.Key).SelectedItem.Value);
                            break;
                        case UIContralType.CheckBok:
                            en.SetValByKey(attr.Key, this.GetCBByKey("CB_" + attr.Key).Checked);
                            break;
                        case UIContralType.RadioBtn:
                            if (attr.IsEnum)
                            {
                                SysEnums ses = new SysEnums(attr.UIBindKey);
                                foreach (SysEnum se in ses)
                                {
                                    string id = "RB_" + attr.Key + "_" + se.IntKey;
                                    RadioButton rb = this.GetRBLByID(id);
                                    if (rb != null && rb.Checked)
                                    {
                                        en.SetValByKey(attr.Key, se.IntKey);
                                        break;
                                    }
                                }
                            }
                            if (attr.MyFieldType == FieldType.FK)
                            {
                                Entities ens = BP.En.ClassFactory.GetEns(attr.UIBindKey);
                                ens.RetrieveAll();
                                foreach (Entity enNoName in ens)
                                {
                                    RadioButton rb = this.GetRBLByID(attr.Key + "_" + enNoName.GetValStringByKey(attr.UIRefKeyValue));
                                    if (rb != null && rb.Checked)
                                    {
                                        en.SetValByKey(attr.Key, enNoName.GetValStrByKey(attr.UIRefKeyValue));
                                        break;
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetEnData error :" + ex.Message);
            }
            return en;
        }

        public DDL GetDDLByKey(string key)
        {
            return (DDL)this.FindControl(key);
        }
        //		public CheckBox GetCBByKey(string key)
        public CheckBox GetCBByKey(string key)
        {
            return (CheckBox)this.FindControl(key);
        }
        #endregion

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (this.IsPostBack)
            {
                //	this.Bind(this.HisEn,this.IsReadonly,this.IsShowDtl) ;
            }
        }
        public Entity HisEn = null;
        public static string GetRefstrs1(string keys, Entity en, Entities hisens)
        {
            string refstrs = "";

            #region  Join many of the entity editor 
            AttrsOfOneVSM oneVsM = en.EnMap.AttrsOfOneVSM;
            if (oneVsM.Count > 0)
            {
                foreach (AttrOfOneVSM vsM in oneVsM)
                {
                    string url = "UIEn1ToM.aspx?EnsName=" + en.ToString() + "&AttrKey=" + vsM.EnsOfMM.ToString() + keys;
                    int i = 0;
                    try
                    {
                        i = DBAccess.RunSQLReturnValInt("SELECT COUNT(*)  as NUM FROM " + vsM.EnsOfMM.GetNewEntity.EnMap.PhysicsTable + " WHERE " + vsM.AttrOfOneInMM + "='" + en.PKVal + "'");
                    }
                    catch
                    {
                        i = DBAccess.RunSQLReturnValInt("SELECT COUNT(*)  as NUM FROM " + vsM.EnsOfMM.GetNewEntity.EnMap.PhysicsTable + " WHERE " + vsM.AttrOfOneInMM + "=" + en.PKVal);
                    }

                    if (i == 0)
                        refstrs += "[<a href='" + url + "'  >" + vsM.Desc + "</a>]";
                    else
                        refstrs += "[<a href='" + url + "'  >" + vsM.Desc + "-" + i + "</a>]";

                }
            }
            #endregion

            #region  Joined his door-related functions 
            //			SysUIEnsRefFuncs reffuncs = en.GetNewEntities.HisSysUIEnsRefFuncs ;
            //			if ( reffuncs.Count > 0  )
            //			{
            //				foreach(SysUIEnsRefFunc en1 in reffuncs)
            //				{
            //					string url="RefFuncLink.aspx?RefFuncOID="+en1.OID.ToString()+"&MainEnsName="+hisens.ToString()+keys;
            //					refstrs+="[<a href='"+url+"' >"+en1.Name+"</a>]";
            //				}
            //			}
            #endregion

            #region  He added detail 
            EnDtls enDtls = en.EnMap.Dtls;
            if (enDtls.Count > 0)
            {
                foreach (EnDtl enDtl in enDtls)
                {
                    string url = "UIEnDtl.aspx?EnsName=" + enDtl.EnsName + "&RefKey=" + enDtl.RefKey + "&RefVal=" + en.PKVal.ToString() + "&MainEnsName=" + en.ToString() + keys;
                    int i = 0;
                    try
                    {
                        i = DBAccess.RunSQLReturnValInt("SELECT COUNT(*) FROM " + enDtl.Ens.GetNewEntity.EnMap.PhysicsTable + " WHERE " + enDtl.RefKey + "='" + en.PKVal + "'");
                    }
                    catch
                    {
                        i = DBAccess.RunSQLReturnValInt("SELECT COUNT(*) FROM " + enDtl.Ens.GetNewEntity.EnMap.PhysicsTable + " WHERE " + enDtl.RefKey + "=" + en.PKVal);
                    }

                    if (i == 0)
                        refstrs += "[<a href='" + url + "'  >" + enDtl.Desc + "</a>]";
                    else
                        refstrs += "[<a href='" + url + "'  >" + enDtl.Desc + "-" + i + "</a>]";
                }
            }
            #endregion

            return refstrs;
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
        ///		 Required method for Designer support  -  Do not use the code editor 
        ///		 Modify the contents of this method .
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion

    }
}
