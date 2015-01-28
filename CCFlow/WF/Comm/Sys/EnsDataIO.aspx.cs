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

public partial class CCFlow_Comm_Sys_EnsDataIO : BP.Web.WebPageAdmin
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
    public new string Step
    {
        get
        {
            return this.Request.QueryString["Step"];
        }
    }

    public void Bind2()
    {
        this.Pub1.DivInfoBlockBegin();

        this.Pub1.Add("<b>Step 2/3:</b> Setting field correspondence <hr>");

        string filePath = BP.Sys.SystemConfig.PathOfWebApp + "/Temp/" + WebUser.No + "DTS.xls";
        DataTable dt = BP.DA.DBLoad.GetTableByExt(filePath, null);

        this.Pub1.AddTable();
        this.Pub1.AddTR();
        this.Pub1.AddTDTitle(" Whether to import the field ?");
        this.Pub1.AddTDTitle(" Chinese Name ");
        this.Pub1.AddTDTitle(" Field manual matching ");
        this.Pub1.AddTREnd();

        Entity en = BP.En.ClassFactory.GetEns(this.EnsName).GetNewEntity;
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
        btn.Text = " Empty way to import ";
        btn.Attributes["onclick"] = "return window.confirm(' Are you sure you want to perform ?  If the implementation of the existing data will be cleared ,Excel The data import into .');";
        this.Pub1.Add(btn);
        btn.Click += new EventHandler(btn_DataIO_Click);



        this.Pub1.AddB("&nbsp;&nbsp;&nbsp;&nbsp;with:");
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
        btn.Text = " Primary key , Perform updates by importing .";
        btn.Attributes["onclick"] = "return window.confirm(' Are you sure you want to perform ?  If the implementation of the existing data will be updated as the primary key .');";
        this.Pub1.Add(btn);

        this.Pub1.Add(" - <a href='EnsDataIO.aspx?EnsName=" + this.EnsName + "&Step=1' > Returns data file upload </a> - <a href='EnsDataIO.aspx?EnsName=" + this.EnsName + "&DoType=OutHtml' target=_blank > Open an existing data source </a>");

        btn.Click += new EventHandler(btn_UpdateIO_Click);

        this.Pub1.DivInfoBlockEnd();
    }
    /// <summary>
    ///  Empty way to import 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void btn_DataIO_Click(object sender, EventArgs e)
    {
        Entities ens = BP.En.ClassFactory.GetEns(this.EnsName);
        ens.RetrieveAll();
        string msg = " Execute the following information :<hr>";
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
            //  Begin the import .

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
                    msg += "@ Line number :" + idx + " Successful implementation .";
                }
                catch (Exception ex)
                {
                    msg += "<font color=red>@ Line number :" + idx + " Execution failed ." + rowMsg + " @ Failure information :" + ex.Message + "</font>";
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
            this.ResponseWriteRedMsg(" Execution error : Data has been rolled back back . Error Messages :" + ex.Message +". MSG= "+msg);
        }
    }
    /// <summary>
    ///  Update mode 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void btn_UpdateIO_Click(object sender, EventArgs e)
    {
        Entities ens = BP.En.ClassFactory.GetEns(this.EnsName);
        ens.RetrieveAll();
        string msg =  " Execute the following information :<hr>";
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
                //  Check out the data , According to the updated master key .
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
                    msg += "@row:" + idx + " OK.";
                }
                catch (Exception ex)
                {
                    msg += "<font color=red>@Row:" + idx + "error." + rowMsg + " @error:" + ex.Message + "</font>";
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
            this.ResponseWriteRedMsg(  " Execution error : Data has been rolled back back . Error Messages :" + ex.Message + ". MSG= " + msg);
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
        this.Pub1.AddH1(  " Import and export data " );
        switch (this.DoType)
        {
            case "OutHtml":
                Entities ens2 = BP.En.ClassFactory.GetEns(this.EnsName);
                ens2.RetrieveAll();
                this.Pub1.Clear();
                this.Pub1.BindEns(ens2);
                return;
            case "OutAll":
                Entities ens = BP.En.ClassFactory.GetEns(this.EnsName);
                ens.RetrieveAll();
                this.ExportDGToExcel(ens);
                string file1 = this.ExportDGToExcel(ens);
                this.Response.Redirect(this.Request.ApplicationPath + "/Temp/" + file1, true);
                return;
            case "OutCurrent":
                Entities ens1 = BP.En.ClassFactory.GetEns(this.EnsName);
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

                this.Pub1.AddH3("<a href='EnsDataIO.aspx?EnsName=" + this.EnsName + "&DoType=OutAll' target=_self > Export all data to Excel.</a>");
                this.Pub1.Add("<font color=green> Export all data to Excel.</font>");



                this.Pub1.AddH3("<a href='EnsDataIO.aspx?EnsName=" + this.EnsName + "&Step=1' target=_self  > Perform data import </a>");
                this.Pub1.Add("<font color=green> According to a fixed format from Excel Import Data .</font>");

              
                //this.Pub1.AddH3("<a href='EnsDataIO.aspx?EnsName=" + this.EnsName + "&DoType=OutCurrent' target=_self  > Export to the current query Excel.</a>");
                //this.Pub1.Add("<font color=green> Export data in accordance with the current query conditions .</font>");
                this.Pub1.DivInfoBlockEnd();
                break;
        }
    }
    public void Bind1()
    {
        HtmlInputFile file = new HtmlInputFile();
        file.ID = "f";
        this.Pub1.DivInfoBlockBegin();
        this.Pub1.Add("<b>Step 1/3:</b> Upload Excel Data files <hr>");
        this.Pub1.Add(file);
        Button btn = new Button();
        btn.CssClass = "Btn";
        btn.ID = "Btn_Up";
        btn.Text = " Upload data files ";
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
                this.Alert(" Please upload xls File . \t\n" + file.Value);
            }


            string ext= ".xls";
            if ( file.PostedFile.FileName.Contains(".xlsx"))
                ext=".xlsx";

            string filePath = BP.Sys.SystemConfig.PathOfWebApp + "/Temp/" + WebUser.No + "DTS" + ext;
            file.PostedFile.SaveAs(filePath);
            DataTable dt = BP.DA.DBLoad.GetTableByExt(filePath, null);
            if (dt.Rows.Count == 0)
                throw new Exception("@ Failed to read file , There is no data in the file .");

            this.Response.Redirect("EnsDataIO.aspx?EnsName=" + this.EnsName + "&Step=2", true);
            return;
        }
        catch (Exception ex)
        {
            this.ResponseWriteRedMsg("@ Error reading file :" + ex.Message);
        }
    }
    public void OutExcel()
    {
        Entities ens = BP.En.ClassFactory.GetEns(this.EnsName);
        Map map = ens.GetNewEntity.EnMap;
        string strLine = "<table border=1 >";
        // Makefile title 
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
        this.Pub1.Add("Excel Table Style ( You can copy and copy to Excel The completion of data acquisition .)");
        this.Pub1.Add(strLine);
        this.Pub1.DivInfoBlockEnd();

    }

    public void OutExcel_bak()
    {
        Entities ens = BP.En.ClassFactory.GetEns(this.EnsName);
        Map map = ens.GetNewEntity.EnMap;
        string filename = WebUser.No + ".xls";
        string file = filename;
        // bool flag = true;
        string filepath = SystemConfig.PathOfWebApp + "\\Temp\\";

        #region  Parameters and variable settings 
        // If the export directory is not established , Is established .
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
        // Makefile title 
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
