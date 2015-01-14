using System;
using System.IO;
using System.Data;
using System.Web.UI;
using System.Collections;
using System.Web.UI.WebControls;
using BP.Web;
//using BP.Rpt ; 
using BP.DA;
using BP.En;
using System.Reflection;
using System.Text.RegularExpressions;
using Excel;
using BP.Sys;
using BP.Sys;

namespace BP.Web
{
    /// <summary>
    /// PortalPage  The summary .
    /// </summary>
    public class MasterPage : System.Web.UI.MasterPage
    {
        #region  Property 
        /// <summary>
        /// key.
        /// </summary>
        public string Key
        {
            get
            {
                return this.Request.QueryString["Key"];
            }
        }
        /// <summary>
        /// _HisEns
        /// </summary>
        public Entities _HisEns = null;
        /// <summary>
        ///  His related functions 
        /// </summary>
        public Entities HisEns
        {
            get
            {
                if (this.EnsName != null)
                {
                    if (this._HisEns == null)
                        _HisEns = BP.En.ClassFactory.GetEns(this.EnsName);
                }
                return _HisEns;
            }
        }
        private Entity _HisEn = null;
        /// <summary>
        ///  His related functions 
        /// </summary>
        public Entity HisEn
        {
            get
            {
                if (_HisEn == null)
                    _HisEn = this.HisEns.GetNewEntity;
                return _HisEn;
            }
        }
        #endregion 

        public void LoadPop_del()
        {
            this.Page.RegisterClientScriptBlock("sdds",
  "<link href='/WF/Comm/JS/jquery-easyui/themes/default/easyui.css' rel='stylesheet' type='text/css' />");

            this.Page.RegisterClientScriptBlock("db7",
         "<script language='JavaScript' src='" + this.Request.ApplicationPath + "Comm/JS/jquery-easyui/jquery-1.4.4.min.js'></script>");

            this.Page.RegisterClientScriptBlock("db8",
        "<script language='JavaScript' src='" + this.Request.ApplicationPath + "Comm/JS/jquery-easyui/query.easyui.min.js'></script>");

            //   this.Page.RegisterClientScriptBlock("sds",
            // "<link href='/WF/Comm/JS/pop/skin/qq/ymPrompt.css' rel='stylesheet' type='text/css' />");
            //   this.Page.RegisterClientScriptBlock("db7",
            //"<script language='JavaScript' src='" + this.Request.ApplicationPath + "Comm/JS/pop/ymPrompt.js'></script>");
        }
        public string ShowType
        {
            get
            {
                string s = this.Request.QueryString["ShowType"];
                if (s == null)
                    s = "1";
                return s;
            }
        }
        public string PageID
        {
            get
            {
                return this.CurrPage;
            }
        }
        public string CurrPage
        {
            get
            {
                string url = System.Web.HttpContext.Current.Request.RawUrl;
                int i = url.LastIndexOf("/") + 1;
                int i2 = url.IndexOf(".aspx") - 6;
                try
                {
                    url = url.Substring(i);
                    url = url.Substring(0, url.IndexOf(".aspx"));
                    return url;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + url + " i=" + i + " i2=" + i2);
                }
            }
        }
        /// <summary>
        ///  Execution Type 
        /// </summary>
        public string DoType
        {
            get
            {
                return this.Request.QueryString["DoType"];
            }
        }
        /// <summary>
        /// EnsName
        /// </summary>
        public string EnsName
        {
            get
            {
                return   this.Request.QueryString["EnsName"];
            }
        }
        /// <summary>
        /// EnName
        /// </summary>
        public string EnName
        {
            get
            {
                return this.Request.QueryString["EnName"];
            }
        }
        public string RefPK
        {
            get
            {
                string s = this.Request.QueryString["RefPK"];
                if (s == null)
                    s = this.Request.QueryString["PK"];

                return s;
            }
        }
        /// <summary>
        ///  Page Index.
        /// </summary>
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

        public new void OpenFile(string fileFullName)
        {
            // string path = Server.MapPath(fileFullName);
            System.IO.FileInfo file = new System.IO.FileInfo(fileFullName);
            this.Response.Clear();
            this.Response.Charset = "GB2312";
            this.Response.ContentEncoding = System.Text.Encoding.UTF8;
            //  Adding header information ,为" File Download / Save as " Dialog box to specify the default file name  
            this.Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(file.Name));

            //  Adding header information , Specify the file size , Let the browser can display the download progress  
            this.Response.AddHeader("Content-Length", file.Length.ToString());

            //  Specify returns a client can not be read by the stream , Must be downloaded  
            this.Response.ContentType = "application/ms-excel";
            //  The file stream is sent to the client  
            this.Response.WriteFile(file.FullName);
            //  Stop execution of the page  
            this.Response.End();
        }
        public void GenerExcel(System.Data.DataTable dt, string fileName)
        {
            try
            {
                GenerExcel_pri(dt, fileName);
            }
            catch (Exception ex)
            {
                Log.DebugWriteError(ex.Message);
                try
                {
                    GenerExcel_pri(dt, fileName);
                }
                catch (Exception ex1)
                {
                    Log.DebugWriteError(ex1.Message);
                    string docs = SystemConfig.PathOfWebApp + "Comm/Helper/ExcelErr.txt";

                    try
                    {
                        docs = DataType.ReadTextFile(docs);
                    }
                    catch
                    {
                    }
                    throw new Exception("@ Help file location for this error :" + docs + "@ Generate  excel File Error , May be the system administrator has not installed correctly excel  Exception Information :" + ex1.Message + " <hr> Help :<hr>" + docs);
                    // GenerExcel_pri_Text(dt, fileName);
                }
            }
        }
        protected void GenerExcel_pri_Text(System.Data.DataTable dt, string fileName)
        {
            string myFileName = fileName.Clone().ToString();
            string path = this.Request.PhysicalApplicationPath + "\\Temp\\";
            if (System.IO.Directory.Exists(path) == false)
                System.IO.Directory.CreateDirectory(path);

            fileName = path + fileName;

            #region  Parameters and variable settings 

            FileStream objFileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter objStreamWriter = new StreamWriter(objFileStream, System.Text.Encoding.Unicode);
            #endregion

            #region  Generate an export file 
            try
            {
                string strLine = "";
                foreach (DataColumn dr in dt.Columns)
                {
                    strLine += dr.ColumnName + Convert.ToChar(9);
                }
                objStreamWriter.WriteLine(strLine);
                strLine = "";
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        strLine += dr[dc.ColumnName].ToString() + Convert.ToChar(9);
                    }
                    objStreamWriter.WriteLine(strLine);
                    strLine = "";
                }
            }
            catch
            {

            }

            finally
            {
                objStreamWriter.Close();
                objFileStream.Close();
            }
            #endregion


            this.OpenFile(fileName);

            //this.Response.Redirect("/Front/Tmp/" + myFileName);
            //this.Response.Redirect("/Front/Tmp/" + myFileName);
            //this.WinOpen("/Front/Tmp/" + myFileName);
            //  PubClass.WinOpen(   "./../Tmp/" + myFileName);
            //PubClass.WinOpen();
        }

        private void GenerExcel_pri(System.Data.DataTable dt, string fileName)
        {
            if (dt == null)
                return;

            if (fileName.IndexOf(".xl") == -1)
                fileName = fileName + ".xls";

            string fullName = BP.Sys.SystemConfig.PathOfTemp + fileName;
            DataView dv = new DataView(dt);
            GC.Collect();
            Application excel;
            //int rowIndex = 4;
            //int colIndex = 1;

            int rowIndex = 1;
            int colIndex = 0;

            _Workbook xBk;
            _Worksheet xSt;

            excel = new ApplicationClass();
            excel.DefaultFilePath = BP.Sys.SystemConfig.PathOfTemp;
            xBk = excel.Workbooks.Add(true);

            xSt = (_Worksheet)xBk.ActiveSheet;

            // 
            //  Obtain title  
            // 
            foreach (DataColumn col in dv.Table.Columns)
            {
                colIndex++;
                excel.Cells[1, colIndex] = col.ColumnName;
                xSt.get_Range(excel.Cells[1, colIndex], excel.Cells[1, colIndex]).HorizontalAlignment = XlVAlign.xlVAlignCenter;

                // Align set the title format .
                xSt.get_Range(excel.Cells[1, colIndex], excel.Cells[1, colIndex]).Font.Bold = true;
                // xSt.get_Range(excel.Cells[1, colIndex], excel.Cells[1, colIndex]).ClearFormats();// = true;
                // xSt.get_Range(excel.Cells[1, colIndex], excel.Cells[1, colIndex])..Bold = true; 
            }

            // 
            // Data acquiring table  
            // 
            foreach (DataRowView row in dv)
            {
                rowIndex++;
                colIndex = 1;
                foreach (DataColumn col in dv.Table.Columns)
                {
                    if (col.DataType == System.Type.GetType("System.DateTime"))
                    {
                        excel.Cells[rowIndex, colIndex] = (Convert.ToDateTime(row[col.ColumnName].ToString())).ToString("yyyy-MM-dd");
                        // xSt.get_Range(excel.Cells[rowIndex, colIndex], excel.Cells[rowIndex, colIndex]).HorizontalAlignment = XlVAlign.xlVAlignBottom;// Set the date type field format is centered  
                    }
                    else
                    {
                        if (col.DataType == System.Type.GetType("System.String"))
                        {
                            excel.Cells[rowIndex, colIndex] = "'" + row[col.ColumnName].ToString();
                            // xSt.get_Range(excel.Cells[rowIndex, colIndex], excel.Cells[rowIndex, colIndex]).HorizontalAlignment = XlVAlign.xlVAlignBottom;// Character Set for Align field format  
                        }
                        else
                        {
                            excel.Cells[rowIndex, colIndex] = row[col.ColumnName].ToString();
                        }
                    }
                    colIndex++;
                }
            }

            excel.Visible = true;

            xBk.SaveCopyAs(fileName);

            // ds = null;
            xBk.Close(false, null, null);
            excel.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xBk);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xSt);
            xBk = null;
            excel = null;
            xSt = null;
            GC.Collect();

            // string path = Server.MapPath(BP.Sys.SystemConfig.PathOfTemp + fileName + ".xls");
            // string path = Server.MapPath( xBk.FullName );
            //  string path = Server.MapPath(fileName + ".xls");

            this.OpenFile(fullName);

        }

        public static string GenerExcelFile(System.Data.DataTable dt)
        {
            return null;

            /*
            Excel.Application excelApp = new Excel.ApplicationClass();  // Creates a new Excel Application
            excelApp.Visible = false;  // Makes Excel visible to the user.

            // The following line adds a new workBill
            Excel.WorkBill newWorkBill = excelApp.WorkBills.Add(Excel.XlWBATemplate.xlWBATWorksheet);
   

            string fileName=PubClass.GenerTempFileName("xls");
            string file = SystemConfig.PathOfTemp+fileName;

            // The following code opens an existing workBill
            //string workBillPath = "c:/SomeWorkBill.xls";  // Add your own path here

            Excel.WorkBill excelWorkBill = excelApp.WorkBills.Open(file, 0,
                false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, 
                false,  0, true, false, false);
   
            // The following gets the Worksheets collection
            Excel.Sheets excelSheets = excelWorkBill.Worksheets;

            // The following gets Sheet1 for editing
            string currentSheet = "Sheet1";
            Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelSheets.get_Item(currentSheet);

            //  Generate column .
            int i =0;
            foreach(DataColumn dc in dt.Columns)
            {
                excelWorksheet.Cells[0,i]= dc.ColumnName;
                i++;
            }
            excelWorksheet.Visible=Excel.XlSheetVisibility.xlSheetVisible;

            i = 1;
            foreach(DataRow dr in dt.Rows)
            {
                int rowIdx=0;
                foreach(DataColumn dc in dt.Columns)
                {
                    excelWorksheet.Cells[i,rowIdx]=  dr[dc.ColumnName];
                    rowIdx++;
                }
                i++;
            }

//			excelWorksheet.SaveAs((file,
//　　　　　　　Excel.Missing.Value,Excel.Missing.Value,Missing.Value,Missing.Value,Missing.Value, 
//　　　　　　　Excel.XlSaveAsAccessMode.xlNoChange,Missing.Value,Missing.Value,Missing.Value, 
//　　　　　　　Missing.Value,Missing.Value);

            excelWorkBill.Save();

            //excelWorksheet.SaveAs( file,Missing.Value,Missing.Value,Missing.Value,Missing.Value,Missing.Value,Missing.Value,Missing.Value,Missing.Value,Missing.Value); 

//			excelWorksheet.SaveAs((file,
//　　　　　　　Excel.Missing.Value,Excel.Missing.Value,Missing.Value,Missing.Value,Missing.Value, 
//　　　　　　　Excel.XlSaveAsAccessMode.xlNoChange,Missing.Value,Missing.Value,Missing.Value, 
//　　　　　　　Missing.Value,Missing.Value);


             xexcelWorksheet=null; 
      excelWorkBill=null; 
      excelApp.Quit(); // This one is very important , Otherwise Excel Objects can not exit from memory  
      excelApp=null; 

            string url = "/"+System.Web.HttpContext.Current.Application+"/Temp/"+fileName;
            PubClass.WinOpen(url);
            return  file;
            */
        }

        public void ToFunc(string funcNo)
        {
        }


        #region  Operating system and related entities 

        public void InvokeDataIO(string ensName, bool isOpenWindow)
        {
            string url = this.Request.ApplicationPath + "Comm/Pub/DataIO.aspx?EnsName=" + ensName;
            if (isOpenWindow)
            {
                this.WinOpen(url);
            }
            else
            {
                this.Response.Redirect(url, true);
            }
        }
        /// <summary>
        ///  Calling a single entity manager 
        /// </summary>
        /// <param name="className"> The class name </param>
        /// <param name="pk"> Primary key </param>
        /// <param name="isOpenWindow"> From time to time to open a new window </param>
        public void InvokeEnManager(string enName, string pk, bool isOpenWindow)
        {
            string url = this.Request.ApplicationPath + "Comm/RefFunc/UIEn.aspx?EnsName=" + enName + "&PK=" + pk;
            if (isOpenWindow)
            {
                this.WinOpen(url, "", "card", 800, 400, 50, 50, false, false);
            }
            else
            {
                this.Response.Redirect(url, true);
            }
        }
        /// <summary>
        ///  Calling a single entity manager 
        /// </summary>
        /// <param name="className"> The class name </param>		 
        /// <param name="isOpenWindow"> From time to time to open a new window </param>
        public void InvokeEnManager(string className, bool isOpenWindow)
        {
            string url = this.Request.ApplicationPath + "Comm/RefFunc/UIEn.aspx?EnsName=" + className;
            if (isOpenWindow)
            {
                this.WinOpenShowModalDialog(url, " Card ", "card", 300, 400, 100, 100);
            }
            else
            {
                this.Response.Redirect(url, true);
            }
        }
        #endregion
        /// <summary>
        /// add html.
        /// </summary>
        /// <param name="html"></param>
        public void Add(string html)
        {
            this.Controls.Add(this.ParseControl(html));
        }
        ///// <summary>
        ///// UCSys1
        ///// </summary>
        //public BP.Web.Comm.UC.Migrated_UCSys UCSys1
        //{
        //    get
        //    {
        //        return (BP.Web.Comm.UC.Migrated_UCSys)this.FindControl("UCSys1");
        //    }
        //}
        //public BP.Web.Comm.UC.Migrated_UCSys UCSys2
        //{
        //    get
        //    {
        //        return (BP.Web.Comm.UC.Migrated_UCSys)this.FindControl("UCSys2");
        //    }
        //}
        //public BP.Web.Comm.UC.Migrated_UCSys UCSys3
        //{
        //    get
        //    {
        //        return (BP.Web.Comm.UC.Migrated_UCSys)this.FindControl("UCSys3");
        //    }
        //}
        //public BP.Web.Comm.UC.Migrated_UCSys UCSys4
        //{
        //    get
        //    {
        //        return (BP.Web.Comm.UC.Migrated_UCSys)this.FindControl("UCSys4");
        //    }
        //}
        //public BP.Web.Comm.UC.Migrated_UCEn UCEn1
        //{
        //    get
        //    {
        //        return (BP.Web.Comm.UC.Migrated_UCEn)this.FindControl("UCEn1");
        //    }
        //}
        public string RefNo
        {
            get
            {
                string s = this.Request.QueryString["RefNo"];
                if (s == null || s=="" || s==string.Empty)
                    s = this.Request.QueryString["No"];
                return s;
            }
        }
        /// <summary>
        ///  The parameters of the current page ．
        /// </summary> 
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

        #region  Import and export files 
        protected void ExportSHDGToReport_del(string file)
        {
            this.Response.WriteFile(file);
        }
        protected void InvokeDataCheck(En.Entities ens)
        {
            this.WinOpen(this.Request.ApplicationPath + "Comm/DataCheck.aspx?EnsName=" + ens.ToString(), " Data Check ", "FileManager", 800, 600, 50, 50);
        }
        /// <summary>
        ///  Call the file manager 
        /// </summary>
        /// <param name="en"></param>		 
        protected void InvokeFileManager(En.Entity en)
        {
            this.WinOpenShowModalDialog(this.Request.ApplicationPath + "Comm/FileManager.aspx?EnsName=" + en.ToString() + "&PK=" + en.PKVal, " File administrator ", "FileManager", 800, 600, 50, 50);
        }

        protected void ExportDGToExcel(System.Data.DataTable dt)
        {
            //this.Session["MyDT"] = dt;
            //this.WinOpen(this.Request.ApplicationPath + "Comm/ToExcel.aspx");
            //return;

            string fileNameS = "Ep" + DBAccess.GenerOID() + ".xls";
            string filename = this.Request.PhysicalApplicationPath + "\\Temp\\" + fileNameS;
            if (System.IO.File.Exists(filename))
                System.IO.File.Delete(filename);



            FileStream objFileStream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter objStreamWriter = new StreamWriter(objFileStream, System.Text.Encoding.Unicode);

            try
            {
                string strLine = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    strLine += dc.ColumnName + Convert.ToChar(9);
                }
                objStreamWriter.WriteLine(strLine);

                foreach (DataRow dr in dt.Rows)
                {
                    strLine = "";
                    foreach (DataColumn dc in dt.Columns)
                    {
                        string s = dr[dc.ColumnName].ToString();
                        if (s.Length > 14)
                        {
                            try
                            {
                                bool isValid = (new Regex(@"^-?(0|\d+)(\.\d+)?$")).IsMatch(s);
                                if (isValid)
                                    s = s + "z1z";
                            }
                            catch
                            {
                            }
                        }
                        strLine = strLine + s + Convert.ToChar(9);
                    }
                    objStreamWriter.WriteLine(strLine);
                }
                objStreamWriter.Close();
                objFileStream.Close();
            }
            catch
            {
            }
            finally
            {
                objStreamWriter.Close();
                objFileStream.Close();
            }
            this.WinOpen(this.Request.ApplicationPath + "/Temp/" + fileNameS);
        }
        protected string ExportDGToExcel(System.Data.DataTable dt, Map map, string title)
        {
            string filename = "Ep" + this.Session.SessionID + ".xls";
            string file = filename;
            bool flag = true;
            string filepath = this.Request.PhysicalApplicationPath + "\\Temp\\";

            #region  Parameters and variable settings 
            //			// Parameter calibration 
            //			if (dg == null || dg.Items.Count <=0 || filename == null || filename == "" || filepath == null || filepath == "")
            //				return null;

            // If the export directory is not established , Is established .
            if (Directory.Exists(filepath) == false)
                Directory.CreateDirectory(filepath);

            filename = filepath + filename;

            FileStream objFileStream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter objStreamWriter = new StreamWriter(objFileStream, System.Text.Encoding.Unicode);
            #endregion

            #region  Generate an export file 
            try
            {
                objStreamWriter.WriteLine();
                objStreamWriter.WriteLine(Convert.ToChar(9) + title + Convert.ToChar(9));
                objStreamWriter.WriteLine();
                string strLine = "";
                // Makefile title 
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    //strLine = strLine + dt.Columns[i].HeaderText + Convert.ToChar(9);

                    foreach (Attr attr in map.Attrs)
                    {
                        if (attr.Key == dt.Columns[i].ColumnName)
                        {
                            strLine = strLine + attr.Desc + Convert.ToChar(9);
                        }
                    }
                }

                objStreamWriter.WriteLine(strLine);
                strLine = "";
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (Attr attr in map.Attrs)
                    {
                        strLine = strLine + dr[attr.Key] + Convert.ToChar(9);
                    }
                    objStreamWriter.WriteLine(strLine);
                    strLine = "";
                }


                objStreamWriter.WriteLine();
                objStreamWriter.WriteLine(Convert.ToChar(9) + "  Lister :" + Convert.ToChar(9) + WebUser.Name + Convert.ToChar(9) + " Date :" + Convert.ToChar(9) + DateTime.Now.ToShortDateString());

            }
            catch
            {
                flag = false;
            }
            finally
            {
                objStreamWriter.Close();
                objFileStream.Close();
            }
            #endregion

            #region  Delete the old file 
            //DelExportedTempFile(filepath);
            #endregion

            if (flag)
            {
                this.WinOpen(this.Request.ApplicationPath + "/Temp/" + file);
                //this.Write_Javascript(" window.open('"+ Request.ApplicationPath + @"/Report/Exported/" + filename +"'); " );
                //this.Write_Javascript(" window.open('"+Request.ApplicationPath+"/Temp/" + file +"'); " );
            }

            return file;
        }
        protected void ExportPageToExcel(string filename)
        {
            //　一, Document type definition , Character Encoding 
            this.Response.Clear();
            this.Response.Buffer = true;
            this.Response.Charset = "utf-8";
            // This line is very important , attachment  Parameter represents as an attachment to download , You can change  online  Online Open 
            //filename=FileFlow.xls  Specify the name of the output file , Note that the extension and specify the file type matches , For :.doc 　　 .xls 　　 .txt 　　.htm　

            this.Response.AppendHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");
            this.Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");

            //Response.ContentType Specify the file type   For application/ms-excel 　　 application/ms-word  application/ms-txt 　　 application/ms-html
            // Or other browsers can be directly supporting documentation 
            Response.ContentType = "application/ms-excel";
            this.EnableViewState = false;

            //　二, Define an input stream 
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);

            //三, Bind target data to the input stream output 
            this.RenderControl(oHtmlTextWriter);

            //this  This page shows the output , You can also bind datagrid, Or other support obj.RenderControl() Control properties 
            this.Response.Write(oStringWriter.ToString());
            this.Response.End();

            //  To sum up : This routine Microsoft Visual Studio .NET 2003 Under the platform tested , Apply C#和VB, When using the VB When the  this  Keyword changed  me . 
        }

        protected void ExportDGToExcelV2(Entities ens, string outFileName)
        {
            System.Web.HttpResponse resp;
            resp = Page.Response;
            resp.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            resp.AppendHeader("Content-Disposition", "attachment;filename=" + outFileName);
            resp.ContentType = "application/ms-excel";

            string colHeaders = "", ls_item = "";

            // Object and object definition table row , At the same time use DataSet Its value is initialized  
            System.Data.DataTable dt = ens.ToDataTableDesc();
            DataRow[] myRow = dt.Select();// May be similar dt.Select("id>10") The purpose of the form to achieve the data filtering 
            int i = 0;
            int cl = dt.Columns.Count;

            // Obtain data tables for each column header , In between each title t Segmentation , Finally, add a carriage return after a column heading  
            for (i = 0; i < cl; i++)
            {
                if (i == (cl - 1))// Finally, a ,加n
                    colHeaders += dt.Columns[i].Caption.ToString() + "\n";
                else
                    colHeaders += dt.Columns[i].Caption.ToString() + "\t";
            }
            resp.Write(colHeaders);

            ////向HTTP Written to the output stream acquired data information  

            //// Progressive processing data    
            foreach (DataRow row in myRow)
            {
                // Data is written to the current row HTTP Output stream , And blank ls_item For downstream data      
                for (i = 0; i < cl; i++)
                {
                    if (i == (cl - 1))// Finally, a ,加n
                        ls_item += row[i].ToString() + "\n";
                    else
                        ls_item += row[i].ToString() + "\t";
                }
                resp.Write(ls_item);
                ls_item = "";
            }
            resp.End();
            return;
        }
        /// <summary>
        /// ExportDGToExcel
        /// </summary>
        /// <param name="ens"></param>
        /// <returns></returns>
        protected string ExportDGToExcel(Entities ens)
        {
            //System.Data.DataTable dt = ens.ToEmptyTableDesc();
            //return MasterPage.GenerExcelFile(dt);

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
            foreach (Entity en in ens)
            {
                /* Generate the file contents */
                strLine = "";
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

                    string str = en.GetValStringByKey(attr.Key);
                    if (str == "" || str == null)
                        str = " ";
                    strLine = strLine + str + Convert.ToChar(9);
                }
                objStreamWriter.WriteLine(strLine.Replace(Convert.ToChar(9).ToString() + Convert.ToChar(9).ToString(), Convert.ToChar(9).ToString()));
            }
            objStreamWriter.Close();
            objFileStream.Close();
            #endregion

            PubClass.WinOpen(this.Request.ApplicationPath + "/Temp/" + file);
            return file;
        }
        public string MyPK
        {
            get
            {
                return this.Request.QueryString["MyPK"];
            }
        }
        public int RefOID
        {
            get
            {
                string s = this.Request.QueryString["RefOID"];
                if (s == null)
                    s = this.Request.QueryString["OID"];
                if (s == null)
                    return 0;
                return int.Parse(s);
            }
        }
        public string GenerTableStr(System.Data.DataTable dt)
        {
            string str = "<Table id='tb' border=1 >";
            //  Title 
            str += "<TR>";
            foreach (DataColumn dc in dt.Columns)
            {
                str += "<TD class='DGCellOfHeader" + BP.Web.WebUser.Style + "' >" + dc.ColumnName + "</TD>";
            }
            str += "</TR>";

            // Content 
            foreach (DataRow dr in dt.Rows)
            {
                str += "<TR>";

                foreach (DataColumn dc in dt.Columns)
                {
                    str += "<TD >" + dr[dc.ColumnName] + "</TD>";
                }
                str += "</TR>";
            }
            str += "</Table>";
            return str;
        }
        public string GenerTablePage(System.Data.DataTable dt, string title)
        {
            return PubClass.GenerTablePage(dt, title);
        }
        public string GenerLabelStr(string title)
        {
            return PubClass.GenerLabelStr(title);
            //return str;
        }
        public Control GenerLabel(string title)
        {
            string path = this.Request.ApplicationPath;
            string str = "";
            str += "<TABLE style='font-size:14px' cellpadding='0' cellspacing='0' background='/WF/Img/DG_bgright.gif'>";
            str += "<TR>";
            str += "<TD>";
            str += "<IMG src='/WF/Img/DG_Title_Left.gif' border='0' width='30' height='25'></TD>";

            str += "<TD  valign=bottom noWrap background='/WF/Img/DG_Title_BG.gif'   height='25' border=0>&nbsp;";
            str += " &nbsp;<b>" + title + "</b>&nbsp;&nbsp;";
            str += "</TD>";
            str += "<TD >";
            str += "<IMG src='/WF/Img/DG_Title_Right.gif' border='0' width='25' height='25'></TD>";
            str += "</TR>";
            str += "</TABLE>";
            return this.ParseControl(str);
        }
        public string GenerCaption(string title)
        {
            if (BP.Web.WebUser.Style == "2")
                return "<div class=Table_Title ><span>" + title + "</span></div>";
            return title;
        }
        /// <summary>
        ///  Refresh 
        /// </summary>
        /// <param name="i"></param>
        public void RefreshHeader_del(int i)
        {
            //this.Response.Write("hello");
            this.Response.Write("<script language='JavaScript'> window.parent.frames(" + i.ToString() + ").location.href='../WF/Port/Head.aspx'</script>");
        }

       
  
        protected override void OnLoad(EventArgs e)
        {
            //if (Web.WebUser.No == null)
            //    this.ToSignInPage();
            base.OnLoad(e);
        }
        /// <summary>
        ///  Export to a excel, File for , Data Import .
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        protected void ExportEnToExcelModel_OpenWin(Attrs attrs, string sheetName)
        {
            string filename = sheetName + ".xls";
            string file = filename;
            //SystemConfig.PathOfTemp
            string filepath = Request.PhysicalApplicationPath + "\\Temp\\";

            #region  Parameters and variable settings 
            // If the export directory is not established , Is established .
            if (Directory.Exists(filepath) == false)
                Directory.CreateDirectory(filepath);

            filename = filepath + filename;
            FileStream objFileStream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter objStreamWriter = new StreamWriter(objFileStream, System.Text.Encoding.Unicode);
            #endregion

            #region  Generate an export file 
            string strLine = "";
            foreach (Attr attr in attrs)
            {
                strLine += attr.Desc + Convert.ToChar(9);
            }

            objStreamWriter.WriteLine(strLine);
            objStreamWriter.Close();
            objFileStream.Close();
            #endregion

            //this.WinOpen(Request.ApplicationPath+"/Temp/" + file,"sss", 500,800);

            this.Write_Javascript(" window.open('" + Request.ApplicationPath + "/Temp/" + file + "'); ");
        }

        #region  User access rights 
        /// <summary>
        ///  Who can use this page , He is a string consisting of numbers .
        /// such as ,admin,jww,002, 
        /// if return value is null, It's mean all emps can visit it . 
        /// </summary>
        protected virtual string WhoCanUseIt()
        {
            return null;
        }
        #endregion

        private void RP(string msg)
        {
            this.Response.Write(msg);
        }
        private void RPBR(string msg)
        {
            this.Response.Write(msg + "<br>");
        }
     
        public string GenerCreateTableSQL(string className)
        {
            ArrayList als = ClassFactory.GetObjects(className);
            int u = 0;
            string sql = "";
            foreach (Object obj in als)
            {
                u++;
                try
                {
                    Entity en = (Entity)obj;
                    switch (en.EnMap.EnDBUrl.DBType)
                    {
                        case DBType.Oracle:
                            sql += SqlBuilder.GenerCreateTableSQLOfOra_OK(en) + " \n GO \n";
                            break;
                        case DBType.Informix:
                            sql += SqlBuilder.GenerCreateTableSQLOfInfoMix(en) + " \n GO \n";
                            break;
                        default:
                            sql += SqlBuilder.GenerCreateTableSQLOfMS(en) + "\n GO \n";
                            break;
                    }
                }
                catch
                {
                    continue;
                }
                //Map map=en.EnMap;
                //objStreamWriter.WriteLine(Convert.ToChar(9)+"No:"+u.ToString()+Convert.ToChar(9) +map.EnDesc +Convert.ToChar(9) +map.PhysicsTable+Convert.ToChar(9) +map.EnType);
            }
            Log.DefaultLogWriteLineInfo(sql);
            return sql;
        }

        public void ExportEntityToExcel(string classbaseName)
        {
            #region  File 
            string filename = "DatabaseDesign.xls";
            string file = filename;
            //bool flag = true;
            string filepath = Request.PhysicalApplicationPath + "\\Temp\\";

            // If the export directory is not established , Is established .
            if (Directory.Exists(filepath) == false)
                Directory.CreateDirectory(filepath);

            filename = filepath + filename;
            FileStream objFileStream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter objStreamWriter = new StreamWriter(objFileStream, System.Text.Encoding.Unicode);
            #endregion

            //string str="";
            ArrayList als = ClassFactory.GetObjects(classbaseName);
            int i = 0;
            objStreamWriter.WriteLine();
            objStreamWriter.WriteLine(Convert.ToChar(9) + " System entities [" + classbaseName + "]" + Convert.ToChar(9));
            objStreamWriter.WriteLine();
            //objStreamWriter.WriteLine(Convert.ToChar(9)+" Thank you for using automatic generator system entity structure "+Convert.ToChar(9)+" Call date "+Convert.ToChar(9)+DateTime.Now.ToString("yyyy年MM月dd日"));
            objStreamWriter.WriteLine(Convert.ToChar(9) + "从" + classbaseName + " Inherited entities [" + als.Count + "]个");


            #region  Processing directory 
            objStreamWriter.WriteLine(Convert.ToChar(9) + " " + Convert.ToChar(9) + " System entities directory ");
            objStreamWriter.WriteLine(Convert.ToChar(9) + " No. " + Convert.ToChar(9) + " Entity Name " + Convert.ToChar(9) + " Physical table / View " + Convert.ToChar(9) + " Type ");
            int u = 0;
            foreach (Object obj in als)
            {
                try
                {
                    u++;
                    Entity en = (Entity)obj;
                    Map map = en.EnMap;
                    objStreamWriter.WriteLine(Convert.ToChar(9) + "No:" + u.ToString() + Convert.ToChar(9) + map.EnDesc + Convert.ToChar(9) + map.PhysicsTable + Convert.ToChar(9) + map.EnType);
                }
                catch
                {
                }
            }
            objStreamWriter.WriteLine();
            #endregion

            foreach (Object obj in als)
            {
                try
                {

                    i++;
                    Entity en = (Entity)obj;
                    Map map = en.EnMap;

                    #region  Generate an export file 
                    objStreamWriter.WriteLine(" No. " + i);
                    objStreamWriter.WriteLine(Convert.ToChar(9) + " Entity Name " + Convert.ToChar(9) + map.EnDesc + Convert.ToChar(9) + " Physical table / View " + Convert.ToChar(9) + map.PhysicsTable + Convert.ToChar(9) + " Entity Type " + Convert.ToChar(9) + map.EnType);
                    if (map.CodeStruct == null)
                    {
                        objStreamWriter.WriteLine(Convert.ToChar(9) + " Coding structure information :无");
                    }
                    else
                    {
                        objStreamWriter.WriteLine(Convert.ToChar(9) + " Coding structure " + Convert.ToChar(9) + map.CodeStruct +  " Check that the length of the number of " + Convert.ToChar(9) + map.IsCheckNoLength );
                    }
                    //objStreamWriter.WriteLine(Convert.ToChar(9)+" Physical storage location "+map.EnDBUrl+Convert.ToChar(9)+" Physical memory storage location "+Convert.ToChar(9)+map.DepositaryOfEntity+Convert.ToChar(9)+"Map  Memory storage location "+Convert.ToChar(9)+map.DepositaryOfMap);
                    objStreamWriter.WriteLine(Convert.ToChar(9) + " Physical storage location " + map.EnDBUrl + Convert.ToChar(9) + "Map  Memory storage location " + Convert.ToChar(9) + map.DepositaryOfMap);
                    objStreamWriter.WriteLine(Convert.ToChar(9) + " Access " + Convert.ToChar(9) + " Check to see " + en.HisUAC.IsView + Convert.ToChar(9) + " Are New " + en.HisUAC.IsInsert + Convert.ToChar(9) + " Delete " + en.HisUAC.IsDelete + " Whether the update " + en.HisUAC.IsUpdate + Convert.ToChar(9) + " Whether Accessories " + en.HisUAC.IsAdjunct);
                    if (map.Dtls.Count > 0)
                    {
                        /* output dtls */
                        EnDtls dtls = map.Dtls;
                        objStreamWriter.WriteLine(Convert.ToChar(9) + " Details / Information from the table : The number of " + dtls.Count);
                        int ii = 0;
                        foreach (EnDtl dtl in dtls)
                        {
                            ii++;
                            objStreamWriter.WriteLine(Convert.ToChar(9) + " " + Convert.ToChar(9) + " Serial number :" + ii + " Description :" + dtl.Desc + " Relation to the entity classes " + dtl.EnsName + " Foreign key " + dtl.RefKey);
                            objStreamWriter.WriteLine(Convert.ToChar(9) + " " + Convert.ToChar(9) + " Physical table :" + dtl.Ens.GetNewEntity.EnMap.PhysicsTable);
                            objStreamWriter.WriteLine(Convert.ToChar(9) + " " + Convert.ToChar(9) + " Remark : With respect to " + dtl.Desc + " More information , Please refer to " + dtl.EnsName);
                        }
                    }
                    else
                    {
                        objStreamWriter.WriteLine(Convert.ToChar(9) + " Details / Information from the table :无");
                    }

                    if (map.AttrsOfOneVSM.Count > 0)
                    {
                        /* output dtls */
                        AttrsOfOneVSM dtls = map.AttrsOfOneVSM;
                        objStreamWriter.WriteLine(Convert.ToChar(9) + " Many relationship : The number of " + dtls.Count);
                        int ii = 0;
                        foreach (AttrOfOneVSM dtl in dtls)
                        {
                            ii++;
                            objStreamWriter.WriteLine(Convert.ToChar(9) + " " + Convert.ToChar(9) + " Serial number :" + ii + " Description :" + dtl.Desc);
                            objStreamWriter.WriteLine(Convert.ToChar(9) + " " + Convert.ToChar(9) + " Many-entity class " + dtl.EnsOfMM.ToString() + " Foreign key " + dtl.AttrOfOneInMM);
                            objStreamWriter.WriteLine(Convert.ToChar(9) + " " + Convert.ToChar(9) + " Associated with this entity to the foreign key " + dtl.AttrOfOneInMM);
                            objStreamWriter.WriteLine(Convert.ToChar(9) + " " + Convert.ToChar(9) + " Multi-entity class " + dtl.EnsOfMM.ToString() + " Foreign key " + dtl.AttrOfMValue);
                        }
                    }
                    else
                    {
                        objStreamWriter.WriteLine(Convert.ToChar(9) + " Many relationship :无");
                    }

                    objStreamWriter.WriteLine(Convert.ToChar(9) + "表/ View structure ");
                    int iii = 0;
                    objStreamWriter.WriteLine(Convert.ToChar(9) + " " + Convert.ToChar(9) + " Properties No. " + Convert.ToChar(9) + " Property Description " + Convert.ToChar(9) + " Property " + Convert.ToChar(9) + " Physical field " + Convert.ToChar(9) + " Data Types " + Convert.ToChar(9) + " Defaults " + Convert.ToChar(9) + " Relationship Type " + Convert.ToChar(9) + " Remark ");

                    foreach (Attr attr in map.Attrs)
                    {
                        iii++;
                        if (attr.MyFieldType == FieldType.Enum)
                        {
                            objStreamWriter.WriteLine(Convert.ToChar(9) + " " + Convert.ToChar(9) + iii + Convert.ToChar(9) + attr.Desc + Convert.ToChar(9) + attr.Key + Convert.ToChar(9) + attr.Field + Convert.ToChar(9) + attr.MyDataTypeStr + Convert.ToChar(9) + attr.DefaultVal + Convert.ToChar(9) + " Enumerate " + Convert.ToChar(9) + " Enumerate Key" + attr.UIBindKey + "  About Enumeration go Sys_Enum Table to find more detailed information .");
                            continue;
                        }
                        if (attr.MyFieldType == FieldType.PKEnum)
                        {
                            objStreamWriter.WriteLine(Convert.ToChar(9) + " " + Convert.ToChar(9) + iii + Convert.ToChar(9) + attr.Desc + Convert.ToChar(9) + attr.Key + Convert.ToChar(9) + attr.Field + Convert.ToChar(9) + attr.MyDataTypeStr + Convert.ToChar(9) + attr.DefaultVal + Convert.ToChar(9) + " Primary key enumeration " + Convert.ToChar(9) + " Enumerate Key" + attr.UIBindKey + "  About Enumeration go Sys_Enum Table to find more detailed information .");

                            //objStreamWriter.WriteLine(Convert.ToChar(9)+" "+Convert.ToChar(9)+"No:"+iii+Convert.ToChar(9)+" Description "+Convert.ToChar(9)+attr.Desc+Convert.ToChar(9)+" Property "+Convert.ToChar(9)+attr.Key+Convert.ToChar(9)+" Property Default "+Convert.ToChar(9)+attr.DefaultVal+Convert.ToChar(9)+" Physical field "+Convert.ToChar(9)+attr.Field+Convert.ToChar(9)+" Field Relationship "+Convert.ToChar(9)+" Enumeration of the primary key "+Convert.ToChar(9)+" Field data types  "+Convert.ToChar(9)+attr.MyDataTypeStr+"");
                            continue;
                        }
                        if (attr.MyFieldType == FieldType.FK)
                        {
                            Entity tmp = attr.HisFKEn;
                            objStreamWriter.WriteLine(Convert.ToChar(9) + " " + Convert.ToChar(9) + iii + Convert.ToChar(9) + attr.Desc + Convert.ToChar(9) + attr.Key + Convert.ToChar(9) + attr.Field + Convert.ToChar(9) + attr.MyDataTypeStr + Convert.ToChar(9) + attr.DefaultVal + Convert.ToChar(9) + " Foreign key " + Convert.ToChar(9) + " Related entities :" + tmp.EnDesc + " Physical table :" + tmp.EnMap.PhysicsTable + "  With respect to " + tmp.EnDesc + " Please look for this entity inside information .");

                            //objStreamWriter.WriteLine(Convert.ToChar(9)+" "+Convert.ToChar(9)+"No:"+iii+Convert.ToChar(9)+" Description "+Convert.ToChar(9)+attr.Desc+Convert.ToChar(9)+" Property "+Convert.ToChar(9)+attr.Key+Convert.ToChar(9)+" Property Default "+Convert.ToChar(9)+attr.DefaultVal+Convert.ToChar(9)+" Physical field "+Convert.ToChar(9)+attr.Field+Convert.ToChar(9)+" Field Relationship "+Convert.ToChar(9)+" Foreign key "+Convert.ToChar(9)+" Field data types  "+Convert.ToChar(9)+attr.MyDataTypeStr+""+" Relation to the entity name "+Convert.ToChar(9)+tmp.EnDesc+" Physical table "+Convert.ToChar(9)+tmp.EnMap.PhysicsTable+Convert.ToChar(9)+" More detailed information please refer to "+Convert.ToChar(9));
                            continue;
                        }
                        if (attr.MyFieldType == FieldType.PKFK)
                        {
                            Entity tmp = attr.HisFKEn;
                            objStreamWriter.WriteLine(Convert.ToChar(9) + " " + Convert.ToChar(9) + iii + Convert.ToChar(9) + attr.Desc + Convert.ToChar(9) + attr.Key + Convert.ToChar(9) + attr.Field + Convert.ToChar(9) + attr.MyDataTypeStr + Convert.ToChar(9) + attr.DefaultVal + Convert.ToChar(9) + " Outside the primary key " + Convert.ToChar(9) + " Related entities :" + tmp.EnDesc + " Physical table :" + tmp.EnMap.PhysicsTable + "  With respect to " + tmp.EnDesc + " Please look for this entity inside information .");
                            continue;
                        }

                        // In other cases .
                        if (attr.MyFieldType == FieldType.Normal || attr.MyFieldType == FieldType.PK)
                        {
                            objStreamWriter.WriteLine(Convert.ToChar(9) + " " + Convert.ToChar(9) + iii + Convert.ToChar(9) + attr.Desc + Convert.ToChar(9) + attr.Key + Convert.ToChar(9) + attr.Field + Convert.ToChar(9) + attr.MyDataTypeStr + Convert.ToChar(9) + attr.DefaultVal + Convert.ToChar(9) + " General " + Convert.ToChar(9) + attr.EnterDesc);
                            //objStreamWriter.WriteLine(Convert.ToChar(9)+" "+Convert.ToChar(9)+"No:"+iii+Convert.ToChar(9)+" Description "+Convert.ToChar(9)+attr.Desc+Convert.ToChar(9)+" Property "+Convert.ToChar(9)+attr.Key+Convert.ToChar(9)+" Property Default "+Convert.ToChar(9)+attr.DefaultVal+Convert.ToChar(9)+" Physical field "+Convert.ToChar(9)+attr.Field+Convert.ToChar(9)+" Field Relationship "+Convert.ToChar(9)+" Character "+Convert.ToChar(9)+" Field data types "+Convert.ToChar(9)+attr.MyDataTypeStr+""+Convert.ToChar(9)+" Input Requirements "+Convert.ToChar(9)+attr.EnterDesc);
                            continue;
                        }
                        //objStreamWriter.WriteLine(" Properties No. :"+iii+Convert.ToChar(9)+" Description "+Convert.ToChar(9)+attr.Desc+Convert.ToChar(9)+" Property "+Convert.ToChar(9)+attr.Key+Convert.ToChar(9)+" Property Default "+Convert.ToChar(9)+attr.DefaultVal+Convert.ToChar(9)+" Physical field "+Convert.ToChar(9)+attr.Field+" Field Relationship "+Convert.ToChar(9)+" Character "+Convert.ToChar(9)+" Field data types "+Convert.ToChar(9)+attr.MyDataTypeStr+Convert.ToChar(9)+""+" Input Requirements "+Convert.ToChar(9)+attr.EnterDesc+Convert.ToChar(9));
                    }
                }
                catch
                {
                }
            }
            objStreamWriter.WriteLine();
            objStreamWriter.WriteLine(Convert.ToChar(9) + Convert.ToChar(9) + " " + Convert.ToChar(9) + Convert.ToChar(9) + "  Lister :" + Convert.ToChar(9) + WebUser.Name + Convert.ToChar(9) + " Date :" + Convert.ToChar(9) + DateTime.Now.ToShortDateString());

            objStreamWriter.Close();
            objFileStream.Close();

            this.Write_Javascript(" window.open('" + this.Request.ApplicationPath + "Temp/" + file + "'); ");


                    #endregion



        }
        public void Helper(string htmlFile)
        {
            this.WinOpen(htmlFile);
        }

        public void Helper()
        {
            this.WinOpen(this.Request.ApplicationPath + "/" + SystemConfig.AppSettings["PageOfHelper"]);
        }
        /// <summary>
        ///  Acquire property by key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetRequestStrByKey(string key)
        {
            return this.Request.QueryString[key];
        }

        #region  Method of operation 
        /// <summary>
        /// showmodaldialog 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="title"></param>
        /// <param name="Height"></param>
        /// <param name="Width"></param>
        protected void ShowModalDialog(string url, string title, int Height, int Width)
        {
            string script = "<script language='JavaScript'>window.showModalDialog('" + url + "','','dialogHeight: " + Height.ToString() + "px; dialogWidth: " + Width.ToString() + "px; dialogTop: 100px; dialogLeft: 100px; center: no; help: no'); </script> ";

            //this.RegisterStartupScript("key1s",script); // old .
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "K1", script); // new 

            //this.Response.Write( script );
            //this.RegisterClientScriptBlock("Dia",script);
        }
        /// <summary>
        ///  Close 
        /// </summary>
        protected void WinClose()
        {
            this.Response.Write("<script language='JavaScript'> window.close();</script>");
        }
        protected void WinClose(string val)
        {
            string clientscript = "<script language='javascript'> window.returnValue = '" + val + "'; window.close(); </script>";
            this.Page.Response.Write(clientscript);
        }
        /// <summary>
        ///  Open a new window 
        /// </summary>
        /// <param name="msg"></param>
        protected void WinOpen(string url)
        {
            this.WinOpen(url, "", "msg", 900, 500);
        }
        protected string dealUrl(string url)
        {
            if (url.IndexOf("?") == -1)
            {
                //url=url.Substring(0,url.IndexOf("",""));
                return url;
            }
            else
            {
                return url;
            }
        }
        protected void WinOpen(string url, string title, string winName, int width, int height)
        {
            this.WinOpen(url, title, winName, width, height, 0, 0);
        }
        protected void WinOpen(string url, string title, int width, int height)
        {
            this.WinOpen(url, title, "ActivePage", width, height, 0, 0);
        }
        protected void WinOpen(string url, string title, string winName, int width, int height, int top, int left)
        {
            WinOpen(url, title, winName, width, height, top, left, false, false);
        }
        protected void WinOpen(string url, string title, string winName, int width, int height, int top, int left, bool _isShowToolBar, bool _isShowAddress)
        {
            url = url.Replace("<", "[");
            url = url.Replace(">", "]");
            url = url.Trim();
            title = title.Replace("<", "[");
            title = title.Replace(">", "]");
            title = title.Replace("\"", "[");
            string isShowAddress = "no", isShowToolBar = "no";
            if (_isShowAddress)
                isShowAddress = "yes";
            if (_isShowToolBar)
                isShowToolBar = "yes";

            this.Response.Write("<script language='JavaScript'> var newWindow =window.open('" + url + "','" + winName + "','width=" + width + ",top=" + top + ",left=" + left + ",height=" + height + ",scrollbars=yes,resizable=yes,toolbar=" + isShowToolBar + ",location=" + isShowAddress + "'); newWindow.focus(); </script> ");
        }
        //private int MsgFontSize=1;
        /// <summary>
        ///  Output to the red warning page .
        /// </summary>
        /// <param name="msg"> News </param>
        protected void ResponseWriteRedMsg(string msg)
        {
            msg = msg.Replace("@", "<BR>@");
            System.Web.HttpContext.Current.Session["info"] = msg;
            System.Web.HttpContext.Current.Application["info" + WebUser.No] = msg;
            string url = "/WF/Comm/Port/ErrorPage.aspx";
            this.WinOpen(url, " Caveat ", "errmsg", 500, 400, 150, 270);
        }
        protected void ResponseWriteShowModalDialogRedMsg(string msg)
        {
            msg = msg.Replace("@", "<BR>@");
            System.Web.HttpContext.Current.Session["info"] = msg;
            //			string url="/WF/Comm/Port/ErrorPage.aspx";
            string url = "/WF/Comm/Port/ErrorPage.aspx?d=" + DateTime.Now.ToString();
            this.WinOpenShowModalDialog(url, " Caveat ", "msg", 500, 400, 120, 270);
        }
        protected void ResponseWriteShowModalDialogBlueMsg(string msg)
        {
            msg = msg.Replace("@", "<BR>@");
            System.Web.HttpContext.Current.Session["info"] = msg;
            string url = "/WF/Comm/Port/InfoPage.aspx?d=" + DateTime.Now.ToString();
            this.WinOpenShowModalDialog(url, " Prompt ", "msg", 500, 400, 120, 270);
        }

        protected void WinOpenShowModalDialog(string url, string title, string key, int width, int height, int top, int left)
        {
            //url=this.Request.ApplicationPath+"Comm/ShowModalDialog.htm?"+url;
            //this.RegisterStartupScript(key,"<script language='JavaScript'>window.showModalDialog('"+url+"','"+key+"' ,'dialogHeight: 500px; dialogWidth:"+width+"px; dialogTop: "+top+"px; dialogLeft: "+left+"px; center: yes; help: no' ) ;  </script> ");

            this.Page.ClientScript.RegisterStartupScript(this.GetType(), key, "<script language='JavaScript'>window.showModalDialog('" + url + "','" + key + "' ,'dialogHeight: 500px; dialogWidth:" + width + "px; dialogTop: " + top + "px; dialogLeft: " + left + "px; center: yes; help: no' ) ;  </script> ");

        }
        protected void WinOpenShowModalDialogResponse(string url, string title, string key, int width, int height, int top, int left)
        {
            url = this.Request.ApplicationPath + "Comm/ShowModalDialog.htm?" + url;
            this.Response.Write("<script language='JavaScript'>window.showModalDialog('" + url + "','" + key + "' ,'dialogHeight: 500px; dialogWidth:" + width + "px; dialogTop: " + top + "px; dialogLeft: " + left + "px; center: yes; help: no' ) ;  </script> ");
        }

        protected void ResponseWriteRedMsg(Exception ex)
        {
            this.ResponseWriteRedMsg(ex.Message);
        }
        /// <summary>
        ///  Output to the information on the page blue .
        /// </summary>
        /// <param name="msg"> News </param>
        protected void ResponseWriteBlueMsg(string msg)
        {
            msg = msg.Replace("@", "<br>@");
            System.Web.HttpContext.Current.Session["info"] = msg;
            System.Web.HttpContext.Current.Application["info" + WebUser.No] = msg;
            string url = "/WF/Comm/Port/InfoPage.aspx?d=" + DateTime.Now.ToString();
            //   string url = "/WF/Comm/Port/InfoPage.aspx?d=" + DateTime.Now.ToString();
            this.WinOpen(url, " Information ", "d" + this.Session.SessionID, 500, 400, 150, 270);
        }

        protected void AlertHtmlMsg(string msg)
        {
            if (string.IsNullOrEmpty(msg))
                return;

            msg = msg.Replace("@", "<br>@");
            System.Web.HttpContext.Current.Session["info"] = msg;
            string url = "MsgPage.aspx?d=" + DateTime.Now.ToString();
            this.WinOpen(url, " Information ", this.Session.SessionID, 500, 400, 150, 270);
        }
        /// <summary>
        ///  Saved successfully 
        /// </summary>
        protected void ResponseWriteBlueMsg_SaveOK()
        {
            Alert(" Saved successfully !", false);
        }
        /// <summary>
        ///  Saved successfully 
        /// </summary>
        /// <param name="num"> Record Number .</param>
        protected void ResponseWriteBlueMsg_SaveOK(int num)
        {
            Alert(" Total [" + num + "] Records saved successfully !", false);
        }
        /// <summary>
        /// ResponseWriteBlueMsg_DeleteOK
        /// </summary>
        protected void ResponseWriteBlueMsg_DeleteOK()
        {

            this.Alert(" Deleted successfully !", false);
            //
            // Update successful 
            //			//this.Alert(" Deleted successfully !");
            //			ResponseWriteBlueMsg(" Deleted successfully !");
        }
        /// <summary>
        /// " Total ["+delNum+"] Records deleted successfully !"
        /// </summary>
        /// <param name="delNum">delNum</param>
        protected void ResponseWriteBlueMsg_DeleteOK(int delNum)
        {
            //this.Alert(" Deleted successfully !");
            this.Alert(" Total [" + delNum + "] Records deleted successfully !", false);

        }
        /// <summary>
        /// ResponseWriteBlueMsg_UpdataOK
        /// </summary>
        protected void ResponseWriteBlueMsg_UpdataOK()
        {
            //this.ResponseWriteBlueMsg(" Update successful ",false);
            this.Alert(" Update successful !");
            // ResponseWriteBlueMsg(" Update successful !");
        }
        protected void ToSignInPage()
        {
            System.Web.HttpContext.Current.Response.Redirect(SystemConfig.PageOfLostSession);
        }
        protected void ToWelPage()
        {
            System.Web.HttpContext.Current.Response.Redirect(System.Web.HttpContext.Current.Request.ApplicationPath + "Wel.aspx");
        }
        protected void ToErrorPage(Exception mess)
        {
            this.ToErrorPage(mess.Message);
        }
        /// <summary>
        ///  Information can also switch to the surface .
        /// </summary>
        /// <param name="mess"></param>
        protected void ToErrorPage(string mess)
        {
            System.Web.HttpContext.Current.Session["info"] = mess;
            System.Web.HttpContext.Current.Response.Redirect("/WF/Comm/Port/ToErrorPage.aspx?d=" + DateTime.Now.ToString(), false);
        }
        /// <summary>
        ///  Information can also switch to the surface .
        /// </summary>
        /// <param name="mess"></param>
        protected void ToMsgPage(string mess)
        {
            mess = mess.Replace("@", "<BR>@");
            System.Web.HttpContext.Current.Session["info"] = mess;
            if (SystemConfig.AppSettings["PageMsg"] == null)
                System.Web.HttpContext.Current.Response.Redirect("/WF/Comm/Port/InfoPage.aspx?d=" + DateTime.Now.ToString(), false);
            else
                System.Web.HttpContext.Current.Response.Redirect(SystemConfig.AppSettings["PageMsg"] + "?d=" + DateTime.Now.ToString(), false);
        }
        protected void ToMsgPage_Do(string mess)
        {
            System.Web.HttpContext.Current.Session["info"] = mess;
            System.Web.HttpContext.Current.Response.Redirect("./Port/InfoPage.aspx?d=" + DateTime.Now.ToString(), false);
        }
        #endregion

        /// <summary>
        /// Go to a page on . '_top'
        /// </summary>
        /// <param name="mess"></param>
        /// <param name="target">'_top'</param>
        protected void ToErrorPage(string mess, string target)
        {
            System.Web.HttpContext.Current.Session["info"] = mess;
            System.Web.HttpContext.Current.Response.Redirect("/WF/Comm/Port/InfoPage.aspx target='_top'");
        }

        /// <summary>
        ///  Window OnInit Event , Automatically added to the page about the record of the current line Hidden
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            //ShowRuning();
            base.OnInit(e);

            if (this.WhoCanUseIt() != null)
            {
                if (this.WhoCanUseIt() == WebUser.No)
                    return;
                if (this.WhoCanUseIt().IndexOf("," + WebUser.No + ",") == -1)
                    this.ToErrorPage(" You do not have permission to access this page .");
            }




        }

        #region  Associated with the operation of the control 
        public void ShowDataTable(System.Data.DataTable dt)
        {
            this.Response.Write(this.DataTable2Html(dt, true));
        }
        /// <summary>
        ///  Show DataTable.
        /// </summary>
        public string DataTable2Html(System.Data.DataTable dt, bool isShowTitle)
        {
            string str = "";
            if (isShowTitle)
            {
                str = dt.TableName + "  Total :" + dt.Rows.Count + " Record .";
            }
            str += "<Table>";
            str += "<TR>";
            foreach (DataColumn dc in dt.Columns)
            {
                str += "  <TD warp=false >";
                str += dc.ColumnName;
                str += "  </TD>";
            }
            str += "</TR>";



            foreach (DataRow dr in dt.Rows)
            {
                str += "<TR>";

                foreach (DataColumn dc in dt.Columns)
                {
                    str += "  <TD>";
                    str += dr[dc.ColumnName];
                    str += "  </TD>";
                }
                str += "</TR>";
            }

            str += "</Table>";
            return str;

            //this.ResponseWriteBlueMsg(str);


        }
        /// <summary>
        ///  Show run 
        /// </summary>
        public void ShowRuning()
        {
            //if (this.IsPostBack==false)
            //	return ;		


            string str = "<script language=javascript><!-- function showRuning() {	sending.style.visibility='visible' } --> </script>";


            // if (!this.IsClientScriptBlockRegistered("ClientProxyScript"))
            //   this.RegisterClientScriptBlock("ClientProxyScript", str);

            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ClientProxyScript"))
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "ClientProxyScript", str);

            if (this.IsPostBack == false)
            {
                str = "<div id='sending' style='position: absolute; top: 126; left: -25; z-index: 10; visibility: hidden; width: 903; height: 74'><TABLE WIDTH=100% BORDER=0 CELLSPACING=0 CELLPADDING=0><TR><td width=30%></td><TD bgcolor=#ff9900><TABLE WIDTH=100% height=70 BORDER=0 CELLSPACING=2 CELLPADDING=0><TR><td bgcolor=#eeeeee align=center> The system is appropriate for your request ,  Please wait ...</td></tr></table></td><td width=30%></td></tr></table></div> ";
                this.Response.Write(str);
            }
        }

        #endregion

        #region  Image properties 

        /// <summary>
        ///  To check whether the function 
        /// </summary>
        protected bool IsCheckFunc
        {
            get
            {
                //if (this.SubPageMessage==null || this.SubPageTitle==null) 
                //return false;

                if (ViewState["IsCheckFunc"] != null)
                    return (bool)ViewState["IsCheckFunc"];
                else
                    return true;

            }
            set { ViewState["IsCheckFunc"] = value; }
        }


        #endregion

        #region  With respect to session  Operating .

        public static object GetSessionObjByKey(string key)
        {
            object val = System.Web.HttpContext.Current.Session[key];
            return val;
        }
        public static string GetSessionByKey(string key)
        {
            return (string)GetSessionObjByKey(key);
        }
        /// <summary>
        ///  Taken out of the string  Key1:val1;Key2:val2;  值. 
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <returns></returns>
        public static string GetSessionByKey(string key1, string key2)
        {
            string str = GetSessionByKey(key1);
            if (str == null)
                throw new Exception(" Does not take into " + key1 + " Value .");

            string[] strs = str.Split(';');
            foreach (string s in strs)
            {
                string[] ss = s.Split(':');
                if (ss[0] == key2)
                    return ss[1];
            }
            return null;
        }
        public static void SetSessionByKey(string key, object obj)
        {
            System.Web.HttpContext.Current.Session[key] = obj;
        }
        public static void SetSessionByKey(string key1, string key2, object obj)
        {
            string str = GetSessionByKey(key1);
            string KV = key2 + ":" + obj.ToString() + ";";
            if (str == null)
            {
                SetSessionByKey(key1, KV);
                return;
            }



            string[] strs = str.Split(';');
            foreach (string s in strs)
            {
                string[] ss = s.Split(':');
                if (ss[0] == key2)
                {
                    SetSessionByKey(key1, str.Replace(s + ";", KV));
                    return;
                }
            }

            SetSessionByKey(key1, str + KV);
        }
        #endregion

        #region  For  ViewState  Operations .
        /// <summary>
        ///  Set up  ViewState Value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="DefaultVal"></param>
        public void SetValueByKey_del(string key, object val, object DefaultVal)
        {
            if (val == null)
                ViewState[key] = DefaultVal;
            else
                ViewState[key] = val;
        }
        /// <summary>
        ///  Take out Val
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValueByKey_del(string key)
        {
            try
            {
                return ViewState[key].ToString();
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// ss
        /// </summary>
        /// <param name="key">ss</param>
        /// <param name="DefaultVal">ss</param>
        /// <returns></returns>
        public string GetValueByKey_del(string key, string DefaultVal)
        {
            try
            {
                return ViewState[key].ToString();
            }
            catch
            {
                return DefaultVal;
            }
        }
        /// <summary>
        ///  According to key  Taken out ,bool  The plant . 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public bool GetBoolValusByKey_del(string key, bool DefaultValue)
        {
            try
            {
                return bool.Parse(this.GetValueByKey_del(key));
            }
            catch
            {
                return DefaultValue;
            }
        }
        /// <summary>
        ///  Take out int valus ,  If you do not return  DefaultValue ;
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetIntValueByKey_del(string key, int DefaultValue)
        {
            try
            {
                return int.Parse(ViewState[key].ToString());
            }
            catch
            {
                return DefaultValue;
            }
        }
        #endregion



        /// <summary>
        ///  This one table  Is used on the handle of the page DataGride. 
        /// </summary>
        protected System.Data.DataTable Table
        {
            get
            {
                //DataTable dt = (System.Data.DataTable)ViewState["Table"];
                System.Data.DataTable dt = (System.Data.DataTable)ViewState["Table"];
                if (dt == null)
                    dt = new System.Data.DataTable();
                return dt;
            }
            set
            {
                ViewState["Table"] = value;
            }
        }
        protected System.Data.DataTable Table_bak
        {
            get
            {
                //DataTable dt = (System.Data.DataTable)ViewState["Table"];
                System.Data.DataTable dt = this.Session["Table"] as System.Data.DataTable;
                if (dt == null)
                    dt = new System.Data.DataTable();
                return dt;
            }
            set
            {
                this.Session["Table"] = value;
            }
        }
        protected System.Data.DataTable Table1
        {
            get
            {

                System.Data.DataTable dt = (System.Data.DataTable)ViewState["Table1"];
                if (dt == null)
                    dt = new System.Data.DataTable();
                return dt;
            }
            set
            {
                ViewState["Table1"] = value;
            }
        }
        /// <summary>
        ///  Application primary key 
        /// </summary>
        protected string PK
        {
            get
            {
                try
                {
                    return ViewState["PK"].ToString();
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                ViewState["PK"] = value;
            }
        }
        /// <summary>
        ///  To save state .
        /// </summary>
        protected bool IsNew_del
        {
            get
            {
                try
                {
                    return (bool)ViewState["IsNew"];
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                ViewState["IsNew"] = value;
            }
        }
        /// <summary>
        /// PKOID if is null return 0 
        /// </summary>
        protected int PKint
        {
            get
            {
                try
                {
                    return int.Parse(ViewState["PKint"].ToString());
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState["PKint"] = value;
            }
        }
        //		protected void ShowMessage(string msg)
        //		{
        //			PubClass.ShowMessage(msg);
        //		}		
        //		protected void ShowMessage_SaveOK()
        //		{
        //			PubClass.ShowMessageMSG_SaveOK();
        //		}
        protected void ShowMessage_SaveUnsuccessful()
        {
            //PubClass.ShowMessage(msg);
        }

        //		protected void ShowMessage_UpdateSuccessful()
        //		{
        //			PubClass.ShowMessage(" Update successful !");
        //		}
        protected void ShowMessage_UpdateUnsuccessful()
        {
            //PubClass.ShowMessage(msg);
        }
        protected void Write_Javascript(string script)
        {
            script = script.Replace("<", "[");
            script = script.Replace(">", "]");
            Response.Write("<script language=javascript> " + script + " </script>");
        }
        protected void ShowMessageWin(string url)
        {
            this.Response.Write("<script language='JavaScript'> window.open('" + url + "')</script>");
        }
        protected void Alert(string mess)
        {
            if (string.IsNullOrEmpty(mess))
                return;

            this.Alert(mess, false);
        }
        /// <summary>
        ///  Need not page  Parameters ,show message
        /// </summary>
        /// <param name="mess"></param>
        protected void Alert(string mess, bool isClent)
        {
            if (string.IsNullOrEmpty(mess))
                return;

            //this.ResponseWriteRedMsg(mess);
            //return;
            mess = mess.Replace("'", "＇");

            mess = mess.Replace("\"", "＂");

            mess = mess.Replace(";", ";");
            mess = mess.Replace(")", "）");
            mess = mess.Replace("(", "（");

            mess = mess.Replace(",", ",");
            mess = mess.Replace(":", ":");


            mess = mess.Replace("<", "［");
            mess = mess.Replace(">", "］");

            mess = mess.Replace("[", "［");
            mess = mess.Replace("]", "］");


            mess = mess.Replace("@", "\\n@");
            string script = "<script language=JavaScript>alert('" + mess + "');</script>";
            if (isClent)
                System.Web.HttpContext.Current.Response.Write(script);
            else
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "kesy", script);
            //this.RegisterStartupScript("key1", script);
        }

        protected void Alert(Exception ex)
        {
            this.Alert(ex.Message, false);
        }
        #region  Public methods 

        #region  Report export problems 
        /// <summary>
        ///  According to DataTable Export content to Excel中  
        /// </summary>
        /// <param name="dt"> To export content DataTable</param>
        /// <param name="filepath"> File path to be generated </param>
        /// <param name="filename"> Document to be produced </param>
        /// <returns></returns>
        protected bool ExportDataTableToExcel_OpenWin_del(System.Data.DataTable dt, string title)
        {
            string filename = "Ep" + this.Session.SessionID + ".xls";
            string file = filename;
            bool flag = true;
            string filepath = SystemConfig.PathOfTemp;

            #region  Deal with  datatable
            foreach (DataColumn dc in dt.Columns)
            {
                switch (dc.ColumnName)
                {
                    case "No":
                        dc.Caption = " Serial number ";
                        break;
                    case "Name":
                        dc.Caption = " Name ";
                        break;
                    case "Total":
                        dc.Caption = " Total ";
                        break;
                    case "FK_Dept":
                        dc.Caption = " Department number ";
                        break;
                    case "ZSJGName":
                        dc.Caption = " Department name ";
                        break;
                    case "IncNo":
                        dc.Caption = " Taxpayer ID ";
                        break;
                    case "IncName":
                        dc.Caption = " Taxpayer Name ";
                        break;
                    case "TaxpayerNo":
                        dc.Caption = " Taxpayer ID ";
                        break;
                    case "TaxpayerName":
                        dc.Caption = " Taxpayer Name ";
                        break;
                    case "byrk":
                        dc.Caption = " This month storage ";
                        break;
                    case "ljrk":
                        dc.Caption = " Cumulative storage ";
                        break;
                    case "qntq":
                        dc.Caption = " Last year ";
                        break;
                    case "jtqzje":
                        dc.Caption = " Decrease compared with last year amount ";
                        break;
                    case "jtqzjl":
                        dc.Caption = " Rate of change compared with last year ";
                        break;
                    case "BenYueYiJiao":
                        dc.Caption = " Paid this month ";
                        break;
                    case "BenYueYingJiao":
                        dc.Caption = " This month payable ";
                        break;
                    case "BenYueWeiJiao":
                        dc.Caption = " This month unpaid ";
                        break;
                    case "LeiJiWeiJiao":
                        dc.Caption = " Accumulated unpaid ";
                        break;
                    case "QuNianTongQiLeiJiYiJiao":
                        dc.Caption = " Over the same period last year unpaid ";
                        break;

                    case "QianNianTongQiLeiJiYiJiao":
                        dc.Caption = " Accumulated paid the same period the year before ";
                        break;
                    case "QianNianTongQiLeiJiYingJiao":
                        dc.Caption = " Accumulated over the same period the year before due ";
                        break;

                    case "JiaoQuNianTongQiZhengJian":
                        dc.Caption = " Decrease over last year ";
                        break;
                    case "JiaoQuNianTongQiZhengJianLv":
                        dc.Caption = " Rate of change compared with same period last year ";
                        break;

                    case "JiaoQianNianTongQiZhengJian":
                        dc.Caption = " Decrease over last year ";
                        break;
                    case "JiaoQianNianTongQiZhengJianLv":
                        dc.Caption = " Compared with the same period the previous year rate of change ";
                        break;
                    case "LeiJiYiJiao":
                        dc.Caption = " Accumulated paid ";
                        break;
                    case "LeiJiYingJiao":
                        dc.Caption = " Cumulative payable ";
                        break;
                    case "QuNianBenYueYiJiao":
                        dc.Caption = " Last month, paid ";
                        break;
                    case "QuNianBenYueYingJiao":
                        dc.Caption = " Last month, payable ";
                        break;
                    case "QuNianLeiJiYiJiao":
                        dc.Caption = " Last year, the cumulative paid ";
                        break;
                    case "QuNianLeiJiYingJiao":
                        dc.Caption = " Last year, the cumulative payable ";
                        break;
                    case "QianNianBenYueYiJiao":
                        dc.Caption = " Two years ago this month, paid ";
                        break;
                    case "QianNianBenYueYingJiao":
                        dc.Caption = " Two years ago this month, payable ";
                        break;
                    case "QianNianLeiJiYiJiao":
                        dc.Caption = " Accumulated paid the same period the year before ";
                        break;
                    case "QianNianLeiJiYingJiao":
                        dc.Caption = " Accumulated over the same period the year before due ";
                        break;
                    case "JiaoQuNianZhengJian":
                        dc.Caption = " Decrease over last year ";
                        break;
                    case "JiaoQuNianZhengJianLv":
                        dc.Caption = " Rate of change compared with same period last year ";
                        break;
                    case "JiaoQianNianZhengJian":
                        dc.Caption = " Decrease over the same period the previous year ";
                        break;
                    case "JiaoQianNianZhengJianLv":
                        dc.Caption = " Compared with the same period the previous year rate of change ";
                        break;
                    case "level":
                        dc.Caption = " Class times ";
                        break;
                }
            }
            #endregion

            #region  Parameters and variable settings 
            // Parameter calibration 
            if (dt == null || dt.Rows.Count <= 0 || filename == null || filename == "" || filepath == null || filepath == "")
                return false;

            // If the export directory is not established , Is established 
            if (Directory.Exists(filepath) == false) Directory.CreateDirectory(filepath);

            filename = filepath + filename;

            FileStream objFileStream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter objStreamWriter = new StreamWriter(objFileStream, System.Text.Encoding.Unicode);
            #endregion

            #region  Generate an export file 
            try
            {
                objStreamWriter.WriteLine();
                objStreamWriter.WriteLine(Convert.ToChar(9) + title + Convert.ToChar(9));
                objStreamWriter.WriteLine();
                string strLine = "";
                // Makefile title 
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    strLine = strLine + dt.Columns[i].Caption + Convert.ToChar(9);
                }

                objStreamWriter.WriteLine(strLine);

                strLine = "";

                // Generate the file contents 
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    for (int col = 0; col < dt.Columns.Count; col++)
                    {
                        strLine = strLine + dt.Rows[row][col] + Convert.ToChar(9);
                    }
                    objStreamWriter.WriteLine(strLine);
                    strLine = "";
                }
                objStreamWriter.WriteLine();
                objStreamWriter.WriteLine(Convert.ToChar(9) + " Lister :" + Convert.ToChar(9) + WebUser.Name + Convert.ToChar(9) + " Date :" + Convert.ToChar(9) + DateTime.Now.ToShortDateString());

            }
            catch
            {
                flag = false;
            }
            finally
            {
                objStreamWriter.Close();
                objFileStream.Close();
            }
            #endregion

            #region  Delete the old file 
            DelExportedTempFile(filepath);
            #endregion


            if (flag)
            {
                this.WinOpen("../Temp/" + file);
                //this.Write_Javascript(" window.open( ); " );
            }

            return flag;
        }
        /// <summary>
        ///  Delete temporary files generated when exporting  2002.11.09 create by bluesky 
        /// </summary>
        /// <param name="filepath"> Temporary file path </param>
        /// <returns></returns>
        public bool DelExportedTempFile(string filepath)
        {
            bool flag = true;
            try
            {
                string[] files = Directory.GetFiles(filepath);

                for (int i = 0; i < files.Length; i++)
                {
                    DateTime lastTime = File.GetLastWriteTime(files[i]);
                    TimeSpan span = DateTime.Now - lastTime;

                    if (span.Hours >= 1)
                        File.Delete(files[i]);
                }
            }
            catch
            {
                flag = false;
            }

            return flag;
        }

        #endregion  Export Report 


        #endregion

    }
}

