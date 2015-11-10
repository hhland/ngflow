using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BP.Web;
using BP.Sys;
using BP.Port;
using BP.DA;
using BP.Sys.Xml;
using BP.Web.Controls;
using BP.En;
using BP;

public partial class Comm_Sys_EnsDataIO : BP.Web.WebPageAdmin
{
    public new string EnsName
    {
        get
        {
            string s = this.Request.QueryString["EnsName"];
            if (s == null)
                s = "BP.GE.Infos";
            return s;
        }
    }
    public string Step
    {
        get
        {
            return this.Request.QueryString["Step"];
        }
    }

    public void Bind2()
    {
        this.Pub1.DivInfoBlockBegin();

        this.Pub1.Add("<b>第2/3步：</b>设置字段对应关系<hr>");

        string filePath = BP.Sys.SystemConfig.PathOfWebApp + "/Temp/" + WebUser.No + "DTS.xls";
        DataTable dt = BP.DA.DBLoad.GetTableByExt(filePath, null);

        this.Pub1.AddTable();
        this.Pub1.AddTR();
        this.Pub1.AddTDTitle("是否导入该字段?");
        this.Pub1.AddTDTitle("中文名称");
        this.Pub1.AddTDTitle("字段手工匹配");
        this.Pub1.AddTREnd();

        Entity en = ClassFactory.GetEns(this.EnsName).GetNewEntity;
        Attrs attrs = en.EnMap.Attrs;
        foreach (Attr attr in attrs)
        {
            this.Pub1.AddTR();
            CheckBox cb = new CheckBox();
            cb.ID = "CB_" + attr.Key;
            cb.Text = attr.Key;
            cb.Checked = true;
            this.Pub1.AddTD(cb);
            this.Pub1.AddTD(attr.Desc);

            DDL ddl = new DDL();
            ddl.ID = "DDL_" + attr.Key;
            int i = -1;
            foreach (DataColumn dc in dt.Columns)
            {
                i++;
                ListItem li = new ListItem();
                li.Text = dc.ColumnName;
                li.Value = i.ToString();
                if (li.Text == attr.Desc)
                    li.Selected = true;
                ddl.Items.Add(li);
            }
            this.Pub1.AddTD(ddl);
            this.Pub1.AddTREnd();
        }
        this.Pub1.AddTableEndWithHR();

        Button btn = new Button();
        btn.ID = "Btn_Clear";
        btn.CssClass = "Btn";
        btn.Text = "清空方式导入";
        btn.Attributes["onclick"] = "return window.confirm('您确定要执行吗？ 如果执行现有的数据将会被清空，Excel中的数据导入进去。');";
        this.Pub1.Add(btn);
        btn.Click += new EventHandler(btn_DataIO_Click);

        this.Pub1.AddB("&nbsp;&nbsp;&nbsp;&nbsp;以:");
        DDL ddl1 = new DDL();
        ddl1.ID = "DDL_PK";
        foreach (Attr attr in attrs)
        {
            ddl1.Items.Add(new ListItem(attr.Desc, attr.Key));
        }
        this.Pub1.Add(ddl1);

        ddl1.SetSelectItem(en.PK);

        btn = new Button();
        btn.CssClass = "Btn";
        btn.ID = "Btn_Update";
        btn.Text = "为主键，执行更新方式导入。";
        btn.Attributes["onclick"] = "return window.confirm('您确定要执行吗？ 如果执行现有的数据将会按照主键更新。');";
        this.Pub1.Add(btn);

        this.Pub1.Add(" - <a href='EnsDataIO.aspx?EnsName=" + this.EnsName + "&Step=1' >返回数据文件上传</a> - <a href='EnsDataIO.aspx?EnsName=" + this.EnsName + "&DoType=OutHtml' target=_blank >打开现有的数据源</a>");
        
        btn.Click += new EventHandler(btn_UpdateIO_Click);

        this.Pub1.DivInfoBlockEnd();
    }
    /// <summary>
    /// 清空方式导入
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void btn_DataIO_Click(object sender, EventArgs e)
    {
        Entities ens = ClassFactory.GetEns(this.EnsName);
        ens.RetrieveAll();
        string msg = "执行信息如下：<hr>";
        try
        {
            string filePath = BP.Sys.SystemConfig.PathOfWebApp + "/Temp/" + WebUser.No + "DTS.xls";
            if (System.IO.File.Exists(filePath)==false)
                filePath = BP.Sys.SystemConfig.PathOfWebApp + "/Temp/" + WebUser.No + "DTS.xlsx";

            DataTable dt = BP.DA.DBLoad.GetTableByExt(filePath, null);
            Entity en = ens.GetNewEntity;
            Attrs attrs = en.EnMap.Attrs;

            //this.ResponseWriteRedMsg(dt.Rows.Count.ToString() );
            //return;
            // 开始执行导入。

            ens.ClearTable();

            int idx = 0;
            foreach (DataRow dr in dt.Rows)
            {
                idx++;
                Entity en1 = ens.GetNewEntity;
                string rowMsg = "";
                foreach (Attr attr in attrs)
                {
                    if (this.Pub1.GetCBByID("CB_" + attr.Key).Checked == false)
                        continue;
                    string item = this.Pub1.GetDDLByID("DDL_" + attr.Key).SelectedItem.Text;
                     
                    en1.SetValByKey(attr.Key, dr[item]);
                    rowMsg += attr.Key + " = " + dr[item] + " , ";
                }

                try
                {
                    en1.Insert();
                    msg += "@行号：" + idx + "执行成功。";
                }
                catch (Exception ex)
                {
                    msg += "<font color=red>@行号：" + idx + "执行失败。" + rowMsg + " @失败信息:" + ex.Message + "</font>";
                    msg += ex.Message;
                }
            }
            this.ResponseWriteBlueMsg(msg);
        }
        catch (Exception ex)
        {
            ens.ClearTable();
            foreach (Entity myen in ens)
            {
                if (myen.IsOIDEntity)
                {
                    EntityOID enOId = (EntityOID)myen;
                    enOId.InsertAsOID(enOId.OID);
                }
                else
                {
                    myen.Insert();
                }
            }
            this.ResponseWriteRedMsg("执行错误：数据已经回滚回来。错误信息：" + ex.Message +"。 MSG= "+msg);
        }
    }
    /// <summary>
    /// 更新方式
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void btn_UpdateIO_Click(object sender, EventArgs e)
    {
        Entities ens = ClassFactory.GetEns(this.EnsName);
        ens.RetrieveAll();
        string msg = "执行信息如下：<hr>";
        try
        {
            string filePath = BP.Sys.SystemConfig.PathOfWebApp + "/Temp/" + WebUser.No + "DTS.xls";
            if (System.IO.File.Exists(filePath) == false)
                filePath = BP.Sys.SystemConfig.PathOfWebApp + "/Temp/" + WebUser.No + "DTS.xlsx";

            DataTable dt = BP.DA.DBLoad.GetTableByExt(filePath, null);
            Entity en = ens.GetNewEntity;
            Attrs attrs = en.EnMap.Attrs;

            string updateKey = this.Pub1.GetDDLByID("DDL_PK").SelectedValue;
            int idx = 0;
            foreach (DataRow dr in dt.Rows)
            {
                idx++;
                Entity en1 = ens.GetNewEntity;
                // 查询出来数据,根据要更新的主键。
                foreach (Attr attr in attrs)
                {
                    if (updateKey == attr.Key)
                    {
                        this.Pub1.GetCBByID("CB_" + attr.Key).Checked = true;
                        string item = this.Pub1.GetDDLByID("DDL_" + attr.Key).SelectedItem.Text;
                        en1.SetValByKey(attr.Key, dr[item]);
                        en1.Retrieve(attr.Key, dr[item]);
                        break;
                    }
                }

                string rowMsg = "";
                foreach (Attr attr in attrs)
                {
                    if (this.Pub1.GetCBByID("CB_" + attr.Key).Checked == false)
                        continue;
                    string item = this.Pub1.GetDDLByID("DDL_" + attr.Key).SelectedItem.Text;

                    en1.SetValByKey(attr.Key, dr[item]);
                    rowMsg += attr.Key + " = " + dr[item] + " , ";
                }

                try
                {
                    en1.Save();
                    msg += "@row：" + idx + " OK。";
                }
                catch (Exception ex)
                {
                    msg += "<font color=red>@Row：" + idx + "error。" + rowMsg + " @error:" + ex.Message + "</font>";
                    msg += ex.Message;
                }
            }
            this.ResponseWriteBlueMsg(msg);
        }
        catch (Exception ex)
        {
            ens.ClearTable();
            foreach (Entity myen in ens)
            {
                if (myen.IsOIDEntity)
                {
                    EntityOID enOId = (EntityOID)myen;
                    enOId.InsertAsOID(enOId.OID);
                }
                else
                {
                    myen.Insert();
                }
            }
            this.ResponseWriteRedMsg(  "执行错误：数据已经回滚回来。错误信息：" + ex.Message + "。 MSG= " + msg);
        }
    }
    public void OutAll()
    {

    }
    public void OutCurrent()
    {
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Pub1.AddH1( "导入导出数据" );
        switch (this.DoType)
        {
            case "OutHtml":
                Entities ens2 = ClassFactory.GetEns(this.EnsName);
                ens2.RetrieveAll();
                this.Pub1.Clear();
                this.Pub1.BindEns(ens2);
                return;
            case "OutAll":
                Entities ens = ClassFactory.GetEns(this.EnsName);
                ens.RetrieveAll();
                this.ExportDGToExcel(ens);
                string file1 = this.ExportDGToExcel(ens);
                this.Response.Redirect(this.Request.ApplicationPath + "/Temp/" + file1, true);
                return;
            case "OutCurrent":
                Entities ens1 = ClassFactory.GetEns(this.EnsName);
                //QueryObject qo = BP.Web.Com
                ens1.RetrieveAll();
                string file = this.ExportDGToExcel(ens1);
                this.Response.Redirect(this.Request.ApplicationPath + "/Temp/" + file, true);
                return;
            default:
                break;
        }

        switch (this.Step)
        {
            case "3":
                break;
            case "2":
                this.Bind2();
                return;
            case "1":
                this.Bind1();
                return;
            default:
                this.Pub1.DivInfoBlockBegin();

                this.Pub1.AddH3("<a href='EnsDataIO.aspx?EnsName=" + this.EnsName + "&DoType=OutAll' target=_self >导出全部数据到Excel。</a>");
                this.Pub1.Add("<font color=green>导出全部数据到Excel。</font>");

                this.Pub1.AddH3("<a href='EnsDataIO.aspx?EnsName=" + this.EnsName + "&Step=1' target=_self  > 执行数据导入</a>");
                this.Pub1.Add("<font color=green>按照固定的格式从Excel中导入数据。</font>");
              
                //this.Pub1.AddH3("<a href='EnsDataIO.aspx?EnsName=" + this.EnsName + "&DoType=OutCurrent' target=_self  >导出当前的查询到Excel。</a>");
                //this.Pub1.Add("<font color=green>按照当前的查询条件导出数据。</font>");
                this.Pub1.DivInfoBlockEnd();
                break;
        }
    }
    public void Bind1()
    {
        HtmlInputFile file = new HtmlInputFile();
        file.ID = "f";
        this.Pub1.DivInfoBlockBegin();
        this.Pub1.Add("<b>第1/3步：</b>上传Excel数据文件<hr>");
        this.Pub1.Add(file);
        Button btn = new Button();
        btn.CssClass = "Btn";
        btn.ID = "Btn_Up";
        btn.Text = "上传数据文件";
        this.Pub1.Add(btn);
        btn.Click += new EventHandler(btn_Click);

        this.Pub1.DivInfoBlockEnd();
        this.Pub1.AddBR();
        this.OutExcel();
    }
    void btn_Click(object sender, EventArgs e)
    {
        try
        {
            if (File.Exists(BP.Sys.SystemConfig.PathOfWebApp + "/Temp/" + WebUser.No + "DTS.xls"))
                File.Delete(BP.Sys.SystemConfig.PathOfWebApp + "/Temp/" + WebUser.No + "DTS.xls");

            if (File.Exists(BP.Sys.SystemConfig.PathOfWebApp + "/Temp/" + WebUser.No + "DTS.xlsx"))
                File.Delete(BP.Sys.SystemConfig.PathOfWebApp + "/Temp/" + WebUser.No + "DTS.xlsx");

            HtmlInputFile file = this.Pub1.FindControl("f") as HtmlInputFile;
            if (file.Value.Contains(".xls"))
            {
                this.Alert("请上传xls文件。 \t\n" + file.Value);
            }

            string ext= ".xls";
            if ( file.PostedFile.FileName.Contains(".xlsx"))
                ext=".xlsx";

            string filePath = BP.Sys.SystemConfig.PathOfWebApp + "/Temp/" + WebUser.No + "DTS" + ext;
            file.PostedFile.SaveAs(filePath);
            DataTable dt = BP.DA.DBLoad.GetTableByExt(filePath, null);
            if (dt.Rows.Count == 0)
                throw new Exception("@读取文件失败，没有数据在文件里。");

            this.Response.Redirect("EnsDataIO.aspx?EnsName=" + this.EnsName + "&Step=2", true);
            return;
        }
        catch (Exception ex)
        {
            this.ResponseWriteRedMsg("@读取文件错误:" + ex.Message);
        }
    }
    public void OutExcel()
    {
        Entities ens = ClassFactory.GetEns(this.EnsName);
        Map map = ens.GetNewEntity.EnMap;
        string strLine = "<table border=1 >";
        //生成文件标题
        strLine += "<TR>";
        foreach (Attr attr in map.Attrs)
        {
            if (attr.Key.IndexOf("Text") == -1)
            {
                if (attr.UIVisible == false)
                    continue;
            }

            if (attr.MyFieldType == FieldType.Enum
                || attr.MyFieldType == FieldType.PKEnum
                || attr.MyFieldType == FieldType.PKFK
                || attr.MyFieldType == FieldType.FK)
                continue;

            strLine += "<TD>" + attr.Desc + "</TD>";
        }
        strLine += "</TR>";
        strLine += "</Table>";


        this.Pub1.DivInfoBlockBegin();
        this.Pub1.Add("Excel表格样式(您可以复制并copy到Excel中完成数据采集。)");
        this.Pub1.Add(strLine);
        this.Pub1.DivInfoBlockEnd();

    }

    public void OutExcel_bak()
    {
        Entities ens = ClassFactory.GetEns(this.EnsName);
        Map map = ens.GetNewEntity.EnMap;
        string filename = WebUser.No + ".xls";
        string file = filename;
        // bool flag = true;
        string filepath = SystemConfig.PathOfWebApp + "\\Temp\\";

        #region 参数及变量设置
        //如果导出目录没有建立，则建立.
        if (Directory.Exists(filepath) == false)
            Directory.CreateDirectory(filepath);

        filename = filepath + filename;
        if (File.Exists(filename))
        {
            File.Delete(filename);
        }
        FileStream objFileStream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
        StreamWriter objStreamWriter = new StreamWriter(objFileStream, System.Text.Encoding.UTF32);
        #endregion

        string strLine = "";
        //生成文件标题
        foreach (Attr attr in map.Attrs)
        {
            if (attr.Key.IndexOf("Text") == -1)
            {
                if (attr.UIVisible == false)
                    continue;
            }

            if (attr.MyFieldType == FieldType.Enum
                || attr.MyFieldType == FieldType.PKEnum
                || attr.MyFieldType == FieldType.PKFK
                || attr.MyFieldType == FieldType.FK)
                continue;

            strLine = strLine + attr.Desc + Convert.ToChar(9);
        }

        objStreamWriter.WriteLine(strLine);
        objStreamWriter.Close();
        objFileStream.Dispose();

        string url = this.Request.ApplicationPath + "/Temp/" + file;
        this.Response.Redirect(url, true);
    }
}
