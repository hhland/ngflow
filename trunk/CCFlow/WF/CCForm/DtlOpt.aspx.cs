﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.En;
using BP.WF.XML;
using BP.Web;
using BP.Sys;
using ClosedXML.Excel;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace CCFlow.WF.CCForm
{
    public partial class WF_DtlOpt : BP.Web.WebPage
    {
        public Int64 WorkID
        {
            get
            {
                return Int64.Parse(this.Request.QueryString["WorkID"]);
            }
        }
        public string FK_MapDtl
        {
            get
            {
                return this.Request.QueryString["FK_MapDtl"];
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = " Detail Options ";

            WorkOptDtlXmls xmls = new WorkOptDtlXmls();
            xmls.RetrieveAll();
            MapDtl dtl = new MapDtl(this.FK_MapDtl);

            this.Pub1.Add("\t\n<div id='tabsJ'  align='center'>");
            this.Pub1.Add("\t\n<ul>");
            foreach (WorkOptDtlXml item in xmls)
            {
                switch (item.No)
                {
                    case "UnPass":
                        if (dtl.IsEnablePass == false)
                            continue;
                        break;
                    case "SelectItems":
                        if (dtl.IsEnableSelectImp == false)
                            continue;
                        break;
                    default:
                        break;
                }
                string url = item.URL + "?DoType=" + item.No + "&WorkID=" + this.WorkID + "&FK_MapDtl=" + this.FK_MapDtl;
                this.Pub1.AddLi("<a href=\"" + url + "\" ><span>" + item.Name + "</span></a>");
            }
            this.Pub1.Add("\t\n</ul>");
            this.Pub1.Add("\t\n</div>");

            switch (this.DoType)
            {
                case "UnPass":
                    this.BindUnPass();
                    break;
                case "ExpImp":
                default:
                    this.BindExpImp();
                    break;
            }
        }
        private void BindExpImp()
        {
            MapDtl dtl = new MapDtl(this.FK_MapDtl);
            if (this.Request.QueryString["Flag"] == "ExpTemplete")
            {
                GEDtls dtls = new GEDtls(this.FK_MapDtl);
                dtls.Retrieve(GEDtlAttr.RefPK, this.WorkID);
                //this.ExportDGToExcelV2Header(dtls, dtl.Name + "-templete.xlsx");
                outputExcelTemplete(dtls, dtl.Name );
                //string file = this.Request.PhysicalApplicationPath + @"\DataUser\DtlTemplete\" + this.FK_MapDtl + ".xls";
                //if (System.IO.File.Exists(file) == false)
                //{
                //    this.WinCloseWithMsg(" Design errors : Process designers did not import into the template from the table " + file);
                //    return;
                //}
               // BP.Sys.PubClass.OpenExcel(file, dtl.Name + ".xls");
                this.WinClose();
            }

            //if (this.Request.QueryString["Flag"] == "ExpTemplete")
            //{
            //    string file = this.Request.PhysicalApplicationPath + @"\DataUser\DtlTemplete\" + this.FK_MapDtl + ".xls";
            //    if (System.IO.File.Exists(file) == false)
            //    {
            //        this.WinCloseWithMsg(" Design errors : Process designers did not import into the template from the table " + file);
            //        return;
            //    }
            //    BP.Sys.PubClass.OpenExcel(file, dtl.Name + ".xls");
            //    this.WinClose();
            //    return;
            //}

            if (this.Request.QueryString["Flag"] == "ExpData")
            {
                GEDtls dtls = new GEDtls(this.FK_MapDtl);
                dtls.Retrieve(GEDtlAttr.RefPK, this.WorkID);
                // this.ExportDGToExcelV2(dtls, dtl.Name + ".xlsx");
                this.outputExcelData(dtls,dtl.Name);
                this.WinClose();
                return;
            }

            if (dtl.IsExp)
            {
                this.Pub1.AddFieldSet(" Export Data ");
                this.Pub1.Add(" Export this point from the table below connections , You can increase or decrease the column as needed columns .");
                string urlExp = "DtlOpt.aspx?DoType=" + this.DoType + "&WorkID=" + this.WorkID + "&FK_MapDtl=" + this.FK_MapDtl + "&Flag=ExpData";
                this.Pub1.Add("<a href='" + urlExp + "' target=_blank ><img src='../Img/FileType/xls.gif' border=0 /><b> Export Data </b></a>");
                this.Pub1.AddFieldSetEnd();
            }

            if (dtl.IsImp)
            {
                this.Pub1.AddFieldSet(" By Excel Importing :" + dtl.Name);
                this.Pub1.Add(" Download data templates : Using data templates to export a data template , You can edit the data on the basis of , The edited information <br> The following functions are imported by coming , To improve efficiency .");
                string url = "DtlOpt.aspx?DoType=" + this.DoType + "&WorkID=" + this.WorkID + "&FK_MapDtl=" + this.FK_MapDtl + "&Flag=ExpTemplete";
                this.Pub1.Add("<a href='" + url + "' target=_blank ><img src='../Img/FileType/xls.gif' border=0 /> Data Template </a>");
                this.Pub1.Add("<br>");

                this.Pub1.Add(" Format data files :");
                System.Web.UI.WebControls.FileUpload fu = new System.Web.UI.WebControls.FileUpload();
                fu.ID = "fup";
                this.Pub1.Add(fu);

                BP.Web.Controls.DDL ddl = new BP.Web.Controls.DDL();
                ddl.Items.Add(new ListItem(" Select the import mode ", "all"));
                ddl.Items.Add(new ListItem(" Empty the way ", "0"));
                ddl.Items.Add(new ListItem(" Append mode ", "1"));
                ddl.ID = "DDL_ImpWay";
                this.Pub1.Add(ddl);

                Button btn = new Button();
                btn.Text = " Importing ";
                btn.CssClass = "Btn";
                btn.ID = "Btn_" + dtl.No;
                btn.Click += new EventHandler(btn_Click);
                this.Pub1.Add(btn);
                this.Pub1.AddFieldSetEnd();
            }

            if (dtl.IsEnableSelectImp)
            {
                this.Pub1.AddFieldSet(" Importing from a data source :" + dtl.Name);
                this.Pub1.Add(" Enter the list , Selecting one or more records , And then the OK button , The import .");
                string url = "DtlOpSelectItems.aspx?DoType=" + this.DoType + "&WorkID=" + this.WorkID + "&FK_MapDtl=" + this.FK_MapDtl + "&Flag=ExpTemplete";
                this.Pub1.Add("<a href='" + url + "' target=_self ><img src='../Img/Table.gif' border=0 /><b> Enter ....</b></a>");
                this.Pub1.AddFieldSetEnd();
            }
        }
        void btn_Exp_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string id = btn.ID.Replace("Btn_Exp", "");

            MapDtl dtl = new MapDtl(id);
            GEDtls dtls = new GEDtls(id);
            this.ExportDGToExcelV2(dtls, dtl.Name + ".xls");
            return;
        }
        void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            try
            {
                BP.Web.Controls.DDL DDL_ImpWay = (BP.Web.Controls.DDL)this.Pub1.FindControl("DDL_ImpWay");
                System.Web.UI.WebControls.FileUpload fuit = (System.Web.UI.WebControls.FileUpload)this.Pub1.FindControl("fup");
                if (DDL_ImpWay.SelectedIndex == 0)
                {
                    this.Alert(" Please select Import mode .");
                    return;
                }

                string tempPath =  "d:/Temp/";
                if (System.IO.Directory.Exists(tempPath) == false)
                    System.IO.Directory.CreateDirectory(tempPath);

                MapDtl dtl = new MapDtl(this.FK_MapDtl);
                MapExts mes = new MapExts(this.FK_MapDtl);
                // Obtain extensions .
                string fileName = fuit.FileName.ToLower();
                if (fileName.Contains(".xls") == false)
                {
                    this.Alert(" The file must be uploaded excel File .");
                    return;
                }
                string ext = ".xls";
                if (fileName.Contains(".xlsx"))
                    ext = ".xlsx";

                // Save temporary files .
                string file = tempPath + Session.SessionID + ext;
                fuit.SaveAs(file);
                
                GEDtls dtls = new GEDtls(this.FK_MapDtl);
                System.Data.DataTable dt = BP.DA.DBLoad.GetTableByExt(file);

                //file = this.Request.PhysicalApplicationPath + "\\DataUser\\DtlTemplete\\" + this.FK_MapDtl + ext;
                //if (System.IO.File.Exists(file) == false)
                //{
                //    if (ext == ".xlsx")
                //        file = this.Request.PhysicalApplicationPath + "\\DataUser\\DtlTemplete\\" + this.FK_MapDtl + ".xls";
                //    else
                //        file = this.Request.PhysicalApplicationPath + "\\DataUser\\DtlTemplete\\" + this.FK_MapDtl + ".xls";
                //}

                //string  tempfile = string.Format("http://{0}:{1}/WF/CCForm/DtlOpt.aspx?DoType=&WorkID={2}&FK_MapDtl={3}&Flag=ExpTemplete"
                //    ,Request.Url.Host
                //    ,Request.Url.Port
                //    ,WorkID
                //    ,FK_MapDtl
                //    );

                //System.Data.DataTable dtTemplete = BP.DA.DBLoad.GetTableByExt(tempfile);

                #region  Check that the two documents consistent .
                string[] columnCaptions = getColumnCaptions(dtls);
                foreach (string dcColumnName in columnCaptions)
                //foreach (DataColumn dc in dtTemplete.Columns)
                {
                    bool isHave = false;
                    foreach (DataColumn mydc in dt.Columns)
                    {
                        if (dcColumnName == mydc.ColumnName)
                        {
                            isHave = true;
                            break;
                        }
                    }
                    if (isHave == false)
                        throw new Exception("@ You import excel Format does not meet the system requirements , Please download the template file to re-fill .");
                }
                #endregion  Check that the two documents consistent .

                #region  Generate attribute to import .

                BP.En.Attrs attrs = dtls.GetNewEntity.EnMap.Attrs;
                BP.En.Attrs attrsExp = new BP.En.Attrs();
                foreach (string dcColumnName in columnCaptions)
                //foreach (DataColumn dc in dtTemplete.Columns)
                {
                    foreach (Attr attr in attrs)
                    {
                        if (attr.UIVisible == false)
                            continue;

                        if (attr.IsRefAttr)
                            continue;

                        if (attr.Desc == dcColumnName.Trim())
                        {
                            attrsExp.Add(attr);
                            break;
                        }
                    }
                }
                #endregion  Generate attribute to import .

                #region  The import data .
                if (DDL_ImpWay.SelectedIndex == 1)
                    BP.DA.DBAccess.RunSQL("DELETE FROM " + dtl.PTable + " WHERE RefPK='" + this.WorkID + "'");

                int i = 0;
                Int64 oid = BP.DA.DBAccess.GenerOID(this.FK_MapDtl, dt.Rows.Count);
                string rdt = BP.DA.DataType.CurrentData;

                string errMsg = "";
                foreach (DataRow dr in dt.Rows)
                {
                    GEDtl dtlEn = dtls.GetNewEntity as GEDtl;
                    dtlEn.ResetDefaultVal();

                    foreach (BP.En.Attr attr in attrsExp)
                    {
                        if (attr.UIVisible == false || dr[attr.Desc] == DBNull.Value)
                            continue;
                        string val = dr[attr.Desc].ToString();
                        if (string.IsNullOrWhiteSpace(val))
                            continue;
                        val = val.Trim();
                        switch (attr.MyFieldType)
                        {
                            case FieldType.Enum:
                            case FieldType.PKEnum:
                                SysEnums ses = new SysEnums(attr.UIBindKey);
                                bool isHavel = false;
                                foreach (SysEnum se in ses)
                                {
                                    if (val == se.Lab)
                                    {
                                        val = se.IntKey.ToString();
                                        isHavel = true;
                                        break;
                                    }
                                }
                                if (isHavel == false)
                                {
                                    errMsg += "@ Non-standard data formats ,第(" + i + ")行,列(" + attr.Desc + "), Data (" + val + ") Do not conform to the format , Do not change the value in the enumeration list .";
                                    val = attr.DefaultVal.ToString();
                                }
                                break;
                            case FieldType.FK:
                            case FieldType.PKFK:
                                Entities ens = null;
                                if (attr.UIBindKey.Contains("."))
                                    ens = BP.En.ClassFactory.GetEns(attr.UIBindKey);
                                else
                                    ens = new GENoNames(attr.UIBindKey, "desc");

                                ens.RetrieveAll();
                                bool isHavelIt = false;
                                foreach (Entity en in ens)
                                {
                                    if (val == en.GetValStrByKey("Name"))
                                    {
                                        val = en.GetValStrByKey("No");
                                        isHavelIt = true;
                                        break;
                                    }
                                }
                                if (isHavelIt == false)
                                    errMsg += "@ Non-standard data formats ,第(" + i + ")行,列(" + attr.Desc + "), Data (" + val + ") Do not conform to the format , Do not change the value of the foreign key data list .";
                                break;
                            default:
                                break;
                        }

                        if (attr.MyDataType == BP.DA.DataType.AppBoolean)
                        {
                            if (val.Trim() == "是" || val.Trim().ToLower() == "yes")
                                val = "1";

                            if (val.Trim() == "否" || val.Trim().ToLower() == "no")
                                val = "0";
                        }
                        
                       

                        dtlEn.SetValByKey(attr.Key, val);


                        foreach (MapExt me in mes)
                        {
                            switch (me.ExtType)
                            {

                                case MapExtXmlList.TBFullCtrl:
                                    {
                                        Dictionary<string, string> dict = getDllList(me.MyPK, val);
                                        if (dict != null)
                                        {
                                            foreach (string key in dict.Keys)
                                            {

                                                string dval = dict[key];
                                                dtlEn.SetValByKey(key, dval);
                                            }
                                        }
                                        break;
                                    }
                                default: break;
                            }
                        }

                    }

                  

                        dtlEn.RefPKInt = (int)this.WorkID;
                    dtlEn.SetValByKey("RDT", rdt);
                    dtlEn.SetValByKey("Rec", WebUser.No);
                    i++;

                    dtlEn.InsertAsOID(oid);
                    oid++;
                }
                #endregion  The import data .

                if (string.IsNullOrEmpty(errMsg) == true)
                    this.Alert(" Total (" + i + ") Successful import of data .");
                else
                    this.Alert(" Total (" + i + ") Successful import of data , But there are mistakes :" + errMsg);
            }
            catch (Exception ex)
            {
                string msg = ex.Message.Replace("'", "[");
                this.Alert(msg);
            }
        }
        private void BindUnPass()
        {
            MapDtl dtl = new MapDtl(this.FK_MapDtl);
            Node nd = new Node(dtl.FK_MapData);
            MapData md = new MapData(dtl.FK_MapData);

            string starter = "SELECT Rec FROM " + md.PTable + " WHERE OID=" + this.WorkID;
            starter = BP.DA.DBAccess.RunSQLReturnString(starter);

            GEDtls geDtls = new GEDtls(dtl.No);
            geDtls.Retrieve(GEDtlAttr.Rec, starter, "IsPass", "0");

            MapAttrs attrs = new MapAttrs(dtl.No);
            this.Pub1.AddTable();
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("IDX");

            if (geDtls.Count > 0)
            {
                string str1 = "<INPUT id='checkedAll' onclick='selectAll()' type='checkbox' name='checkedAll'>";
                this.Pub1.AddTDTitle(str1);
            }
            else
            {
                this.Pub1.AddTDTitle();
            }

            string spField = ",Checker,Check_RDT,Check_Note,";

            foreach (MapAttr attr in attrs)
            {
                if (attr.UIVisible == false
                    && spField.Contains("," + attr.KeyOfEn + ",") == false)
                    continue;

                this.Pub1.AddTDTitle(attr.Name);
            }
            this.Pub1.AddTREnd();
            int idx = 0;
            foreach (GEDtl item in geDtls)
            {
                idx++;
                this.Pub1.AddTR();
                this.Pub1.AddTDIdx(idx);
                CheckBox cb = new CheckBox();
                cb.ID = "CB_" + item.OID;
                this.Pub1.AddTD(cb);
                foreach (MapAttr attr in attrs)
                {
                    if (attr.UIVisible == false
                        && spField.Contains("," + attr.KeyOfEn + ",") == false)
                        continue;

                    if (attr.MyDataType == BP.DA.DataType.AppBoolean)
                    {
                        this.Pub1.AddTD(item.GetValBoolStrByKey(attr.KeyOfEn));
                        continue;
                    }

                    switch (attr.UIContralType)
                    {
                        case UIContralType.DDL:
                            this.Pub1.AddTD(item.GetValRefTextByKey(attr.KeyOfEn));
                            continue;
                        default:
                            this.Pub1.AddTD(item.GetValStrByKey(attr.KeyOfEn));
                            continue;
                    }
                }
                this.Pub1.AddTREnd();
            }
            this.Pub1.AddTableEndWithHR();

            if (geDtls.Count == 0)
                return;

            if (nd.IsStartNode == false)
                return;

            Button btn = new Button();
            btn.ID = "Btn_Delete";
            btn.CssClass = "Btn";
            btn.Text = " Bulk Delete ";
            btn.Attributes["onclick"] = " return confirm(' Are you sure you want to perform ?');";
            btn.Click += new EventHandler(btn_DelUnPass_Click);
            this.Pub1.Add(btn);

            btn = new Button();
            btn.ID = "Btn_Imp";
            btn.CssClass = "Btn";
            btn.Text = " Import and re-edit ( Append mode )";
            btn.Click += new EventHandler(btn_Imp_Click);
            this.Pub1.Add(btn);

            btn = new Button();
            btn.ID = "Btn_ImpClear";
            btn.CssClass = "Btn";
            btn.Text = " Import and re-edit ( Empty the way )";
            btn.Click += new EventHandler(btn_Imp_Click);
            this.Pub1.Add(btn);
        }

        void btn_DelUnPass_Click(object sender, EventArgs e)
        {
            MapDtl dtl = new MapDtl(this.FK_MapDtl);
            Node nd = new Node(dtl.FK_MapData);
            MapData md = new MapData(dtl.FK_MapData);

            string starter = "SELECT Rec FROM " + md.PTable + " WHERE OID=" + this.WorkID;
            starter = BP.DA.DBAccess.RunSQLReturnString(starter);
            GEDtls geDtls = new GEDtls(this.FK_MapDtl);
            geDtls.Retrieve(GEDtlAttr.Rec, starter, "IsPass", "0");
            foreach (GEDtl item in geDtls)
            {
                if (this.Pub1.GetCBByID("CB_" + item.OID).Checked == false)
                    continue;
                item.Delete();
            }
            this.Response.Redirect(this.Request.RawUrl, true);
        }
        void btn_Imp_Click(object sender, EventArgs e)
        {
            MapDtl dtl = new MapDtl(this.FK_MapDtl);
            Button btn = sender as Button;
            if (btn.ID.Contains("ImpClear"))
            {
                /* If it is clear the way to import .*/
                BP.DA.DBAccess.RunSQL("DELETE FROM " + dtl.PTable + " WHERE RefPK='" + this.WorkID + "'");
            }

            Node nd = new Node(dtl.FK_MapData);
            MapData md = new MapData(dtl.FK_MapData);

            string starter = "SELECT Rec FROM " + md.PTable + " WHERE OID=" + this.WorkID;
            starter = BP.DA.DBAccess.RunSQLReturnString(starter);
            GEDtls geDtls = new GEDtls(this.FK_MapDtl);
            geDtls.Retrieve(GEDtlAttr.Rec, starter, "IsPass", "0");

            string strs = "";
            foreach (GEDtl item in geDtls)
            {
                if (this.Pub1.GetCBByID("CB_" + item.OID).Checked == false)
                    continue;
                strs += ",'" + item.OID + "'";
            }
            if (strs == "")
            {
                this.Alert(" Please select the data you want to perform .");
                return;
            }
            strs = strs.Substring(1);
            BP.DA.DBAccess.RunSQL("UPDATE  " + dtl.PTable + " SET RefPK='" + this.WorkID + "',BatchID=0,Check_Note='',Check_RDT='" + BP.DA.DataType.CurrentDataTime + "', Checker='',IsPass=1  WHERE OID IN (" + strs + ")");
            this.WinClose();
        }

        protected void outputExcelTemplete(GEDtls dtls,string dtlName) {
            string tempFileName = dtlName + "-templete.xlsx"
                ,tempFilePath=Server.MapPath("/DataUser/Temp/"+tempFileName)
                ;
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");

            Response.AppendHeader("Content-Disposition", "attachment;filename=" +tempFileName);
            Response.ContentType = "application/ms-excel";
            XLWorkbook xlWorkBook = new XLWorkbook();
            //IntPtr t = new IntPtr(xlApp.Hwnd);
            //int k = 0;
            //User32Util.GetWindowThreadProcessId(t, out k);
            //System.Diagnostics.Process P = System.Diagnostics.Process.GetProcessById(k);
            // System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp.Workbooks);//释放
            //xlApp.SheetsInNewWorkbook = 1;
            IXLWorksheet workSheet = xlWorkBook.AddWorksheet(dtlName);

            int rowIndex = 1;
            int colIndex = 0;
            foreach (string caption in getColumnCaptions(dtls))//填充表头
            {
                IXLCell cell = workSheet.Cell(rowIndex, ++colIndex);
                //Excel.Range header = (Excel.Range)workSheet.Cells[rowIndex, ++colIndex];
                cell.SetValue(caption);
                //header.NumberFormatLocal = "@";
                //header.Value2 = ob[key];
            }
            xlWorkBook.SaveAs(tempFilePath);
            Response.WriteFile(tempFilePath);
        }


        protected void outputExcelData(GEDtls dtls, string dtlName)
        {
            string tempFileName = dtlName + ".xlsx"
                , tempFilePath = Server.MapPath("/DataUser/Temp/" + tempFileName)
                ;
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");

            Response.AppendHeader("Content-Disposition", "attachment;filename=" + tempFileName);
            Response.ContentType = "application/ms-excel";
            XLWorkbook xlWorkBook = new XLWorkbook();
            //IntPtr t = new IntPtr(xlApp.Hwnd);
            //int k = 0;
            //User32Util.GetWindowThreadProcessId(t, out k);
            //System.Diagnostics.Process P = System.Diagnostics.Process.GetProcessById(k);
            // System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp.Workbooks);//释放
            //xlApp.SheetsInNewWorkbook = 1;
            IXLWorksheet workSheet = xlWorkBook.AddWorksheet(dtlName);

            int rowIndex = 1;
            int colIndex = 0;
            foreach (string caption in getColumnCaptions(dtls))//填充表头
            {
                IXLCell cell = workSheet.Cell(rowIndex, ++colIndex);
                //Excel.Range header = (Excel.Range)workSheet.Cells[rowIndex, ++colIndex];
                cell.SetValue(caption);
                //header.NumberFormatLocal = "@";
                //header.Value2 = ob[key];
            }

            System.Data.DataTable dt = dtls.ToDataTableDesc();
            DataRow[] myRow = dt.Select();

            int i = 0;
            int cl = dt.Columns.Count;
            foreach (DataRow row in myRow)
            {
                rowIndex++;
                colIndex = 0;
                // Data is written to the current row HTTP Output stream , And blank ls_item For downstream data      
                for (i = 0; i < cl; i++)
                {
                    string val = row[colIndex].ToString();
                    IXLCell cell = workSheet.Cell(rowIndex, ++colIndex);
                    cell.SetValue(val);
                }
                
            }

            xlWorkBook.SaveAs(tempFilePath);
            Response.WriteFile(tempFilePath);
        }

        protected Dictionary<string,string> getDllList(string FK_MapExt,string key) {
            Dictionary<string, string> dict = null;
            try
            {
                string url = string.Format("http://localhost:{0}/WF/CCForm/HanderMapExt.ashx?Key={1}&FK_MapExt={2}&DoType=ReqCtrl&KVs="
                    , Request.Url.Port
                    , key
                    , FK_MapExt
                    );
                WebRequest webrequest = WebRequest.Create(url);
                Stream resStream = webrequest.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(resStream);
                string content = sr.ReadToEnd();
                JObject jsonroot = JObject.Parse(content);
                
                dict = new Dictionary<string, string>();
                foreach (JProperty jp in jsonroot["head"][0]) {
                    dict[jp.Name] = jp.Value.ToString();
                }
            }
            catch {
                dict = null;
            }
            return dict;
        }

    }
}