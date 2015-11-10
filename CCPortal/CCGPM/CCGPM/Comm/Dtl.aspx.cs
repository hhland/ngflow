using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BP.Web;
using BP.DA;
using BP.En;
using BP.Sys;
using BP.Web.Controls;

public partial class Comm_Dtl : WebPage
{
    #region 属性
    public new string EnsName
    {
        get
        {
            string str = this.Request.QueryString["EnsName"];
            if (str == null)
                return "ND299Dtl";
            return str;
        }
    }
    
    public string RefPKVal
    {
        get
        {
            string str = this.Request.QueryString["RefPKVal"];
            if (str == null)
                return "1";
            return str;
        }
    }
    #endregion 属性

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.IsPostBack == false)
        {
            this.BPToolBar1.AddBtn(NamesOfBtn.Save);
          //  this.BPToolBar1.AddBtn(NamesOfBtn.SaveAndClose);
            //this.BPToolBar1.AddBtn(NamesOfBtn.Close);
            this.BPToolBar1.AddBtn(NamesOfBtn.Delete);
            //   this.BPToolBar1.AddBtn(NamesOfBtn.Excel_S);
            this.BPToolBar1.AddBtn(NamesOfBtn.Excel, "导出Excel");
            //   this.BPToolBar1.AddBtn(NamesOfBtn.Close);
        }
        this.BPToolBar1.ButtonClick += new EventHandler(BPToolBar1_ButtonClick);
        this.Bind();
    }
    public void Bind()
    {
        #region 生成标题
        MapAttrs attrs = new MapAttrs(this.EnsName);
        this.Ucsys1.AddTable();
        this.Ucsys1.AddTR();
        this.Ucsys1.AddTDTitle();
        CheckBox cb = new CheckBox();
        cb.ID = "";
        this.Ucsys1.AddTDTitle(cb);
        foreach (MapAttr attr in attrs)
        {
            if (attr.UIVisible == false)
                continue;

            if (attr.IsPK)
                continue;

            this.Ucsys1.AddTDTitle(attr.Name);
        }
        this.Ucsys1.AddTREnd();
        #endregion 生成标题


        BP.Sys.MapDtl mdtl = new MapDtl(this.EnsName);
        this.Title = mdtl.Name;
      this.Label1.Text=  this.GenerCaption( mdtl.Name);
        GEDtls dtls = new GEDtls(this.EnsName);
        QueryObject qo = null;
        try
        {
            qo = new QueryObject(dtls);
            switch (mdtl.DtlOpenType)
            {
                case DtlOpenType.ForEmp:
                    qo.AddWhere(GEDtlAttr.RefPK, this.RefPKVal);
                    qo.addAnd();
                    qo.AddWhere(GEDtlAttr.Rec, WebUser.No);
                    break;
                case DtlOpenType.ForWorkID:
                    qo.AddWhere(GEDtlAttr.RefPK, this.RefPKVal);
                    break;
                case DtlOpenType.ForFID:
                    qo.AddWhere(GEDtlAttr.FID, this.RefPKVal);
                    break;
                default:
                    break;
            }

        }
        catch (Exception ex)
        {
            dtls.GetNewEntity.CheckPhysicsTable();
            this.Response.Redirect("Dtl.aspx?EnsName=" + this.EnsName + "&RefPKVal=" + this.RefPKVal, true);
            return;
        }

        #region 生成翻页
        this.Ucsys2.Clear();
        try
        {
            this.Ucsys2.Clear();
            this.Ucsys2.BindPageIdx(qo.GetCount(), BP.Sys.SystemConfig.PageSize, this.PageIdx, "Dtl.aspx?EnsName=" + this.EnsName + "&RefPKVal=" + this.RefPKVal);
            qo.DoQuery("OID", BP.Sys.SystemConfig.PageSize, this.PageIdx, false);
        }
        catch (Exception ex)
        {
            dtls.GetNewEntity.CheckPhysicsTable();
            this.Response.Redirect("Dtl.aspx?EnsName=" + this.EnsName + "&RefPKVal=" + this.RefPKVal, true);
            return;
        }
        #endregion 生成翻页


        BP.Sys.GEDtl dt = new GEDtl(this.EnsName);
        dt.ResetDefaultVal();
        dt.OID = 0;
        dtls.AddEntity(dt);

        DDL ddl = new DDL();

        #region 生成数据
        int i = 0;
        foreach (BP.Sys.GEDtl dtl in dtls)
        {
            i++;
            if (dtl.OID == 0)
            {
                this.Ucsys1.AddTRSum();
                this.Ucsys1.AddTD("colspan=2", "新建");
            }
            else
            {
                this.Ucsys1.AddTR();
                this.Ucsys1.AddTDIdx(i);
                cb = new CheckBox();
                cb.ID = "CB_" + dtl.OID;
                this.Ucsys1.AddTD(cb);
            }


            foreach (MapAttr attr in attrs)
            {
                if (attr.UIVisible == false)
                    continue;

                if (attr.KeyOfEn == "OID")
                    continue;

                string val = dtl.GetValByKey(attr.KeyOfEn).ToString();
                switch (attr.UIContralType)
                {
                    case UIContralType.TB:
                        TB tb = new TB();
                        this.Ucsys1.AddTD(tb);
                        tb.LoadMapAttr(attr);
                        tb.ID = "TB_" + attr.KeyOfEn + "_" + dtl.OID;

                        switch (attr.MyDataType)
                        {
                            case DataType.AppMoney:
                            case DataType.AppRate:
                                tb.TextExtMoney = decimal.Parse(val);
                                break;
                            default:
                                tb.Text = val;
                                break;
                        }

                        //if (attr.UIIsEnable == false)
                        //{
                        //    if (attr.IsNum)
                        //        tb.ReadOnly = true;
                        //    else
                        //        tb.Enabled = false;
                        //}

                        if (attr.IsNum && attr.LGType == FieldTypeS.Normal)
                        {
                            if (tb.Enabled)
                            {
                                // OnKeyPress="javascript:return VirtyNum(this);"
                                if (attr.MyDataType == DataType.AppInt)
                                    tb.Attributes["OnKeyDown"] = "javascript:return VirtyInt(this);";
                                else
                                    tb.Attributes["OnKeyDown"] = "javascript:return VirtyNum(this);";


                                tb.Attributes["onkeyup"] += "javascript:C" + dtl.OID + "();C" + attr.KeyOfEn + "();";
                                tb.Attributes["class"] = "TBNum";
                            }
                            else
                            {
                                tb.Attributes["onpropertychange"] += "C" + attr.KeyOfEn + "();";
                                tb.Attributes["class"] = "TBNumReadonly";
                            }
                        }
                        break;
                    case UIContralType.DDL:
                        ddl = new DDL();
                        ddl.LoadMapAttr(attr);
                        ddl.ID = "DDL_" + attr.KeyOfEn + "_" + dtl.OID;
                        this.Ucsys1.AddTD(ddl);
                        ddl.SetSelectItem(val);
                        break;
                    case UIContralType.CheckBok:
                        cb = new CheckBox();
                        cb.ID = "CB_" + attr.KeyOfEn + "_" + dtl.OID;
                        cb.Text = attr.Name;
                        if (val == "1")
                            cb.Checked = true;
                        else
                            cb.Checked = false;
                        this.Ucsys1.AddTD(cb);
                        break;
                    default:
                        break;
                }
            }
            this.Ucsys1.AddTREnd();
        }

        #region 生成合计
        this.Ucsys1.AddTRSum();
        this.Ucsys1.AddTD("colspan=2", "合计");
        foreach (MapAttr attr in attrs)
        {
            if (attr.UIVisible == false)
                continue;

            if (attr.IsNum && attr.LGType == FieldTypeS.Normal)
            {
                TB tb = new TB();
                tb.ID = "TB_" + attr.KeyOfEn;
                tb.Text = attr.DefVal;
                tb.ShowType = attr.HisTBType;
                tb.ReadOnly = true;
                tb.Font.Bold = true;
                tb.BackColor = System.Drawing.Color.FromName("infobackground");

                switch (attr.MyDataType)
                {
                    case DataType.AppRate:
                    case DataType.AppMoney:
                        tb.TextExtMoney = dtls.GetSumDecimalByKey(attr.KeyOfEn);
                        break;
                    case DataType.AppInt:
                        tb.TextExtInt = dtls.GetSumIntByKey(attr.KeyOfEn);
                        break;
                    case DataType.AppFloat:
                        tb.TextExtFloat = dtls.GetSumFloatByKey(attr.KeyOfEn);
                        break;
                    default:
                        break;
                }
                this.Ucsys1.AddTD(tb);
            }
            else
            {
                this.Ucsys1.AddTD();
            }
        }
        this.Ucsys1.AddTREnd();
        #endregion 生成合计


        #endregion 生成数据
        this.Ucsys1.AddTableEnd();

        #region 生成 自动计算行
        // 输出自动计算公式
        this.Response.Write("\n<script language='JavaScript'>");
        foreach (GEDtl dtl in dtls)
        {
            string top = "\n function C" + dtl.OID + "() { \n ";
            string script = "";
            foreach (MapAttr attr in attrs)
            {
                if (attr.UIVisible == false)
                    continue;

                if (attr.IsNum == false)
                    continue;

                if (attr.LGType != FieldTypeS.Normal)
                    continue;

                //if (attr.AutoFullDoc != "")
                //{
                //    script += this.GenerAutoFull(dtl.OID.ToString(), attrs, attr);
                //}
            }
            string end = " \n  } ";
            this.Response.Write(top + script + end);
        }
        this.Response.Write("\n</script>");

        // 输出合计算计公式
        foreach (MapAttr attr in attrs)
        {
            if (attr.UIVisible == false)
                continue;

            if (attr.LGType != FieldTypeS.Normal)
                continue;

            if (attr.IsNum == false)
                continue;

            string top = "\n<script language='JavaScript'> function C" + attr.KeyOfEn + "() { \n ";
            string end = "\n } </script>";
            this.Response.Write(top + this.GenerSum(attr, dtls) + " ; \t\n" + end);
        }
        #endregion
    }
    public void Delete()
    {

    }
    public void Save()
    {
        BP.Sys.MapDtl mdtl = new MapDtl(this.EnsName);
        GEDtls dtls = new GEDtls(this.EnsName);
        QueryObject qo = null;
        qo = new QueryObject(dtls);
        switch (mdtl.DtlOpenType)
        {
            case DtlOpenType.ForEmp:
                qo.AddWhere(GEDtlAttr.RefPK, this.RefPKVal);
                qo.addAnd();
                qo.AddWhere(GEDtlAttr.Rec, WebUser.No);
                break;
            case DtlOpenType.ForWorkID:
                qo.AddWhere(GEDtlAttr.RefPK, this.RefPKVal);
                break;
            case DtlOpenType.ForFID:
                qo.AddWhere(GEDtlAttr.FID, this.RefPKVal);
                break;
        }


        qo.DoQuery("OID", BP.Sys.SystemConfig.PageSize, this.PageIdx, false);
        Map map = dtls.GetNewEntity.EnMap;
        int idx = 0;
        string msg="";
        foreach (GEDtl dtl in dtls)
        {
            try
            {
                idx++;
                this.Ucsys1.Copy(dtl, dtl.OID.ToString(), map);
                dtl.Update();
            }
            catch(Exception ex)
            {
                msg += "\r\n在保存("+idx+")行出现错误：\r\n"+ex.Message;
            }
        }


        BP.Sys.GEDtl en = new GEDtl(this.EnsName);
        en = (BP.Sys.GEDtl)this.Ucsys1.Copy(en, "0", map);
        if (en.IsBlank == false)
        {
            en.RefPK = this.RefPKVal;
            en.SetValByKey("FID", this.RefPKVal);
            try
            {
                en.Insert();
            }
            catch (Exception ex)
            {
                msg += "\r\n在插入新行时出现错误：\r\n" + ex.Message;
            }
            if (msg.Length > 2)
            {
                this.Alert(msg);
                return;
            }
        }

        this.Response.Redirect("Dtl.aspx?EnsName=" + this.EnsName + "&RefPKVal=" + this.RefPKVal + "&PageIdx=" + this.PageIdx, true);
    }
    public void ExpExcel()
    {
        BP.Sys.MapDtl mdtl = new MapDtl(this.EnsName);
        this.Label1.Text = this.GenerCaption(mdtl.Name);


        this.Title = mdtl.Name;

        GEDtls dtls = new GEDtls(this.EnsName);
        QueryObject qo = null;
        qo = new QueryObject(dtls);
        switch (mdtl.DtlOpenType)
        {
            case DtlOpenType.ForEmp:
                qo.AddWhere(GEDtlAttr.RefPK, this.RefPKVal);
                qo.addAnd();
                qo.AddWhere(GEDtlAttr.Rec, WebUser.No);
                break;
            case DtlOpenType.ForWorkID:
                qo.AddWhere(GEDtlAttr.RefPK, this.RefPKVal);
                break;
            case DtlOpenType.ForFID:
                qo.AddWhere(GEDtlAttr.FID, this.RefPKVal);
                break;
        }
        qo.DoQuery();

        // this.ExportDGToExcelV2(dtls, this.Title + ".xls");
        //DataTable dt = dtls.ToDataTableDesc();

       // this.GenerExcel(dtls.ToDataTableDesc(), mdtl.Name + ".xls");

        this.GenerExcel_pri_Text(dtls.ToDataTableDesc(), mdtl.Name+"@"+WebUser.No +"@"+DataType.CurrentData+ ".xls");


        //this.ExportDGToExcelV2(dtls, this.Title + ".xls");
        //dtls.GetNewEntity.CheckPhysicsTable();
        //this.Response.Redirect("Dtl.aspx?EnsName=" + this.EnsName + "&RefPKVal=" + this.RefPKVal, true);
    }
    void BPToolBar1_ButtonClick(object sender, EventArgs e)
    {
        ToolbarBtn btn = sender as ToolbarBtn;
        switch (btn.ID)
        {
            case NamesOfBtn.New:
            case NamesOfBtn.Save:
            case NamesOfBtn.SaveAndNew:
                this.Save();
                break;
            case NamesOfBtn.SaveAndClose:
                this.Save();
                this.WinClose();
                break;
            case NamesOfBtn.Delete:
                GEDtls dtls = new GEDtls(this.EnsName);
                QueryObject qo = new QueryObject(dtls);
                qo.DoQuery("OID", BP.Sys.SystemConfig.PageSize, this.PageIdx, false);
                foreach (GEDtl dtl in dtls)
                {
                    CheckBox cb = this.Ucsys1.GetCBByID("CB_" + dtl.PKVal);
                    if (cb == null)
                        continue;

                    if (cb.Checked)
                        dtl.Delete();
                }
                this.Ucsys1.Clear();
                this.Bind();
                break;
            case NamesOfBtn.Excel:
                this.ExpExcel();
                break;
            default:
                BP.Sys.PubClass.Alert("当前版本不支持此功能。");
                break;
        }
    }
    /// <summary>
    /// 生成列的计算
    /// </summary>
    /// <param name="pk"></param>
    /// <param name="attrs"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public string GenerAutoFull(string pk, MapAttrs attrs, MapExt me)
    {
        string left = "\n  document.forms[0]." + this.Ucsys1.GetTBByID("TB_" + me.AttrOfOper + "_" + pk).ClientID + ".value = ";
        string right = me.Doc;
        foreach (MapAttr mattr in attrs)
        {
            string tbID = "TB_" + mattr.KeyOfEn + "_" + pk;
            TB tb = this.Ucsys1.GetTBByID(tbID);
            if (tb == null)
                continue;
            right = right.Replace("@" + mattr.Name, " parseFloat( document.forms[0]." + this.Ucsys1.GetTBByID(tbID).ClientID + ".value.replace( ',' ,  '' ) ) ");
            right = right.Replace("@" + mattr.KeyOfEn, " parseFloat( document.forms[0]." + this.Ucsys1.GetTBByID(tbID).ClientID + ".value.replace( ',' ,  '' ) ) ");
        }

        string s = left + right;
     //   s += "\t\n  document.forms[0]." + this.Ucsys1.GetTBByID("TB_" + me.AttrOfOper + "_" + pk).ClientID + ".value= VirtyMoney(document.forms[0]." + this.Ucsys1.GetTBByID("TB_" + attr.KeyOfEn + "_" + pk).ClientID + ".value ) ;";
        return s += " C" + me.AttrOfOper + "();";
    }
    public string GenerSum(MapAttr mattr, GEDtls dtls)
    {
        string left = "\n  document.forms[0]." + this.Ucsys1.GetTBByID("TB_" + mattr.KeyOfEn).ClientID + ".value = ";
        string right = "";
        int i = 0;
        foreach (GEDtl dtl in dtls)
        {
            string tbID = "TB_" + mattr.KeyOfEn + "_" + dtl.OID;
            TB tb = this.Ucsys1.GetTBByID(tbID);
            if (i == 0)
                right += " parseFloat( document.forms[0]." + tb.ClientID + ".value.replace( ',' ,  '' ) )  ";
            else
                right += " +parseFloat( document.forms[0]." + tb.ClientID + ".value.replace( ',' ,  '' ) )  ";

            i++;
        }

        string s = left + right + " ;";
        switch (mattr.MyDataType)
        {
            case BP.DA.DataType.AppMoney:
            case BP.DA.DataType.AppRate:
                return s += "\t\n  document.forms[0]." + this.Ucsys1.GetTBByID("TB_" + mattr.KeyOfEn).ClientID + ".value= VirtyMoney(document.forms[0]." + this.Ucsys1.GetTBByID("TB_" + mattr.KeyOfEn).ClientID + ".value ) ;";
            default:
                return s;
        }
    }

}
