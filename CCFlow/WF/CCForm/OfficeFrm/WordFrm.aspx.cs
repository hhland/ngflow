using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Web.Controls;
using BP.WF.Template;
using BP.WF;
using BP.En;
using BP.DA;
using BP.Sys;
using BP.Web;

namespace CCFlow.WF.CCForm.OfficeFrm
{
    public partial class WordFrm : BP.Web.WebPage
    {
        #region  Property 
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        public int FK_Node
        {
            get
            {
                try
                {
                    string nodeid = this.Request.QueryString["NodeID"];
                    if (nodeid == null)
                        nodeid = this.Request.QueryString["FK_Node"];
                    return int.Parse(nodeid);
                }
                catch
                {
                    if (string.IsNullOrEmpty(this.FK_Flow))
                        return 0;
                    else
                        return int.Parse(this.FK_Flow); // 0;  There may be a process to call the process a form .
                }
            }
        }
        public string WorkID
        {
            get
            {
                return this.Request.QueryString["WorkID"];
            }
        }
        public int OID
        {
            get
            {
                string cworkid = this.Request.QueryString["CWorkID"];
                if (string.IsNullOrEmpty(cworkid) == false && int.Parse(cworkid) != 0)
                    return int.Parse(cworkid);

                string oid = this.Request.QueryString["WorkID"];
                if (oid == null || oid == "")
                    oid = this.Request.QueryString["OID"];
                if (oid == null || oid == "")
                    oid = "0";
                return int.Parse(oid);
            }
        }
        /// <summary>
        ///  Continuation of the process ID
        /// </summary>
        public int CWorkID
        {
            get
            {
                try
                {
                    return int.Parse(this.Request.QueryString["CWorkID"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        ///  Parent process ID
        /// </summary>
        public int PWorkID
        {
            get
            {
                try
                {
                    return int.Parse(this.Request.QueryString["PWorkID"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        ///  Process ID
        /// </summary>
        public int FID
        {
            get
            {
                try
                {
                    return int.Parse(this.Request.QueryString["FID"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        public int OIDPKVal
        {
            get
            {

                if (ViewState["OIDPKVal"] == null)
                    return 0;
                return int.Parse(ViewState["OIDPKVal"].ToString());
            }
            set
            {
                ViewState["OIDPKVal"] = value;
            }
        }

        public string FK_MapData
        {
            get
            {
                string s = this.Request.QueryString["FK_MapData"];
                if (s == null)
                    return "ND101";
                return s;
            }
        }

        public bool IsEdit
        {
            get
            {
                if (this.Request.QueryString["IsEdit"] == "0")
                    return false;
                return true;
            }
        }

        public string SID
        {
            get
            {
                return this.Request.QueryString["PWorkID"];
            }
        }

        public bool IsPrint
        {
            get
            {
                if (this.Request.QueryString["IsPrint"] == "1")
                    return true;
                return false;
            }
        }

        public string NodeInfo
        {
            get
            {
                BP.WF.Node nodeInfo = new BP.WF.Node(FK_Node);

                return nodeInfo.Name + ": " + WebUser.Name + "     Time :" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        public bool IsMarks { get; set; }

        public bool ReadOnly { get; set; }

        public string UserName { get; set; }

        public bool IsCheck { get; set; }

        /// <summary>
        ///  Whether it is first opened Word Form 
        /// </summary>
        public bool IsFirst { get; set; }

        /// <summary>
        ///  Filled the main table JSON Data , Containing key,value Array 
        /// </summary>
        public string ReplaceParams { get; set; }

        /// <summary>
        ///  Filled the main table attribute names Organization JSON Data ,为String Array 
        /// </summary>
        public string ReplaceFields { get; set; }

        /// <summary>
        ///  Filling the schedule data JSON Data , For an array of objects 
        /// </summary>
        public string ReplaceDtls { get; set; }

        /// <summary>
        ///  Filling the schedule number JSON Data ,为String Array 
        /// </summary>
        public string ReplaceDtlNos { get; set; }
        #endregion  Property 

        protected void Page_Load(object sender, EventArgs e)
        {
            UserName = WebUser.Name;

            if (string.IsNullOrEmpty(FK_MapData))
            {
                divMenu.InnerHtml = "<h1 style='color:red'> Incoming parameter error !<h1>";
                return;
            }

            var md = new MapData(this.FK_MapData);

            string type = Request["action"];
            if (string.IsNullOrEmpty(type))
            {
                InitOffice(true, md);
            }
            else
            {
                if (type.Equals("LoadFile"))
                {
                    LoadFile();
                    return;
                }

                if (type.Equals("SaveFile"))
                {
                    SaveFile();
                    SaveFieldInfos();
                    return;
                }

                InitOffice(false, md);
            }

            // Open the data file .
            GEEntityWordFrm en = new GEEntityWordFrm(this.FK_MapData);
            bool isCreated = false;

            // Data integrity checking property .
            if (en.EnMap.Attrs.Contains(GEEntityWordFrmAttr.FilePath) == false)
            {
                var attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = BP.En.EditType.UnDel;
                attr.KeyOfEn = GEEntityWordFrmAttr.FilePath;
                attr.Name = " File Path ";
                attr.MyDataType = BP.DA.DataType.AppString;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = false;
                attr.UIIsEnable = false;
                attr.MaxLen = 500;
                attr.MinLen = 0;
                attr.Tag = "1";
                attr.Insert();

                en.EnMap.Attrs.Add(attr.HisAttr);
                isCreated = true;
            }

            if (en.EnMap.Attrs.Contains(GEEntityWordFrmAttr.RDT) == false)
            {
                var attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = BP.En.EditType.UnDel;
                attr.KeyOfEn = GEEntityWordFrmAttr.RDT;
                attr.Name = " Last modification time ";
                attr.MyDataType = BP.DA.DataType.AppDateTime;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = false;
                attr.UIIsEnable = false;
                attr.Tag = "1";
                attr.Insert();

                en.EnMap.Attrs.Add(attr.HisAttr);
                isCreated = true;
            }


            if (en.EnMap.Attrs.Contains(GEEntityWordFrmAttr.LastEditer) == false)
            {
                var attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = BP.En.EditType.UnDel;
                attr.KeyOfEn = GEEntityWordFrmAttr.LastEditer;
                attr.Name = " Last Modified people ";
                attr.MyDataType = BP.DA.DataType.AppString;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = false;
                attr.UIIsEnable = false;
                attr.MaxLen = 50;
                attr.MinLen = 0;
                attr.Tag = "1";
                attr.Insert();

                en.EnMap.Attrs.Add(attr.HisAttr);
                isCreated = true;
            }

            if (isCreated)
                en = new GEEntityWordFrm(this.FK_MapData);

            en.CheckPhysicsTable();
            en.OID = this.OID;

            var root = SystemConfig.PathOfDataUser + "\\FrmOfficeTemplate\\";
            var rootInfo = new DirectoryInfo(root);

            if (!rootInfo.Exists)
                rootInfo.Create();

            var files = rootInfo.GetFiles(en.FK_MapData + ".*");
            FileInfo tmpFile = null;
            FileInfo wordFile = null;

            var pathDir = SystemConfig.PathOfDataUser + @"\FrmOfficeFiles\" + this.FK_MapData;

            if (!Directory.Exists(pathDir))
                Directory.CreateDirectory(pathDir);

            //  Determine whether there is the data file .
            if (files.Length == 0)
            {
                Response.Write("<h3>Word Form template file does not exist , Please ensure you have uploaded Word Form Template </h3>");
                Response.End();
                return;
            }

            tmpFile = files[0];
            wordFile = new FileInfo(pathDir + "\\" + this.OID + tmpFile.Extension);

            if (wordFile.Exists == false)
            {
                IsFirst = true;
                File.Copy(tmpFile.FullName, wordFile.FullName);
            }
            else
            {
                IsFirst = false;
            }

            if (en.RetrieveFromDBSources() == 0)
            {
                en.FilePath = wordFile.FullName;
                en.RDT = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                en.LastEditer = WebUser.Name;
                en.Insert(); //  An insert .
            }

            en.ResetDefaultVal();

            // Accept an external parameter data .
            string[] paras = this.RequestParas.Split('&');
            foreach (string str in paras)
            {
                if (string.IsNullOrEmpty(str) || str.Contains("=") == false)
                    continue;

                string[] kvs = str.Split('=');
                en.SetValByKey(kvs[0], kvs[1]);
            }

            // Loading data .
            this.LoadFrmData(new MapAttrs(this.FK_MapData), en);

            // Replace  word  Inside the data .
            fileName.Text = string.Format(@"\{0}\{1}{2}", en.FK_MapData, this.OID, wordFile.Extension);
            fileType.Text = wordFile.Extension.TrimStart('.');
        }

        /// <summary>
        ///  Save from word Extracted data 
        /// </summary>
        private void SaveFieldInfos()
        {
            var mes = new MapExts(this.FK_MapData);
            if (mes.Count == 0) return;

            var item = mes.GetEntityByKey(MapExtAttr.ExtType, MapExtXmlList.PageLoadFull) as MapExt;
            if (item == null) return;
            
            var fieldCount = 0;
            foreach (var key in Request.Form.AllKeys)
            {
                var idx = 0;
                if (key.StartsWith("field") && key.Length > 5 && int.TryParse(key.Substring(5), out idx))
                {
                    fieldCount++;
                }
            }

            var fieldsJson = string.Empty;
            for (var i = 0; i < fieldCount; i++)
            {
                fieldsJson += Request["field" + i];
            }

            //var fieldsJson = Request["field"];
            var fields = LitJson.JsonMapper.ToObject<List<WordField>>(fieldsJson);

            // Update the master table data 
            var en = new GEEntityWordFrm(this.FK_MapData);
            en.OID = this.OID;

            if (en.RetrieveFromDBSources() == 0)
            {
                throw new Exception("OID=" + this.OID + " The data in " + this.FK_MapData + " Does not exist , Please check !");
            }

            // Because here weboffice In the upload interface , Only the return value to upload success and failure , No specific return information parameters , So without making exception handling 
            foreach (var field in fields)
            {
                en.SetValByKey(field.key, field.value);
            }

            en.LastEditer = WebUser.Name;
            en.RDT = DataType.CurrentDataTime;
            en.Update();

            //todo: Updated schedule data , Here the logic may also questionable 
            var mdtls = new MapDtls(this.FK_MapData);
            if (mdtls.Count == 0) return;

            var dtlsCount = 0;
            foreach(var key in Request.Form.AllKeys)
            {
                var idx = 0;
                if(key.StartsWith("dtls") && key.Length > 4 && int.TryParse(key.Substring(4),out idx))
                {
                    dtlsCount++;
                }
            }

            var dtlsJson = string.Empty;
            for(var i=0;i<dtlsCount;i++)
            {
                dtlsJson += Request["dtls" + i];
            }

            //var dtlsJson = Request["dtls"];
            var dtls = LitJson.JsonMapper.ToObject<List<WordDtlTable>>(dtlsJson);
            GEDtls gedtls = null;
            GEDtl gedtl = null;
            WordDtlTable wdtl = null;

            foreach (MapDtl mdtl in mdtls)
            {
                wdtl = dtls.FirstOrDefault(o => o.dtlno == mdtl.No);

                if (wdtl == null || wdtl.dtl.Count == 0) continue;

                // Update is not really on here , Because they do not know the schedule of the primary key , The original list of deleted data can only , And then re-insert the new data 
                gedtls = new GEDtls(mdtl.No);
                gedtls.Delete(GEDtlAttr.RefPK, en.PKVal);

                foreach (var d in wdtl.dtl)
                {
                    gedtl = gedtls.GetNewEntity as GEDtl;

                    foreach (var cell in d.cells)
                    {
                        gedtl.SetValByKey(cell.key, cell.value);
                    }

                    gedtl.RefPK = en.PKVal.ToString();
                    gedtl.RDT = DataType.CurrentDataTime;
                    gedtl.Rec = WebUser.No;
                    gedtl.Insert();
                }
            }
        }

        private void InitOffice(bool isMenu, MapData md)
        {
            bool isCompleate = false;

            if(WorkID == "0")
            {
                isCompleate = false;
            }
            else
            {
                isCompleate = new WorkFlow(FK_Flow, int.Parse(WorkID)).IsComplete;
            }

            if (!isCompleate)
            {
                if (md.IsWoEnableReadonly)
                {
                    ReadOnly = true;
                }

                if (md.IsWoEnableCheck)
                {
                    IsCheck = true;
                }

                IsMarks = md.IsWoEnableMarks;
            }
            else
            {
                ReadOnly = true;
            }

            if (isMenu && !isCompleate)
            {
                #region  Initialization button 
                if (md.IsWoEnableViewKeepMark)
                {
                    divMenu.InnerHtml = "<select id='marks' onchange='ShowUserName()' style='width: 100px'><option value='1'> Whole </option><select>";
                }

                if (md.IsWoEnableTemplete)
                {
                    AddBtn(NamesOfBtn.Open, " Open the template ", "OpenTempLate");
                }
                if (md.IsWoEnableSave)
                {
                    AddBtn(NamesOfBtn.Save, " Save ", "saveOffice");
                }
                if (md.IsWoEnableRevise)
                {
                    AddBtn(NamesOfBtn.Accept, " Accept Change ", "acceptOffice");
                    AddBtn(NamesOfBtn.Refuse, " Reject Changes ", "refuseOffice");
                }

                if (md.IsWoEnableOver)
                {
                    AddBtn("over", " Tao Hong file ", "overOffice");
                }

                if (md.IsWoEnablePrint)
                {
                    AddBtn(NamesOfBtn.Print, " Print ", "printOffice");
                }
                if (md.IsWoEnableSeal)
                {
                    AddBtn(NamesOfBtn.Seal, " Signature ", "sealOffice");
                }
                if (md.IsWoEnableInsertFlow)
                {
                    AddBtn(NamesOfBtn.FlowImage, " Insert flowchart ", "InsertFlow");
                }
                if (md.IsWoEnableInsertFengXian)
                {
                    AddBtn("fegnxian", " Risk insertion point ", "InsertFengXian");
                }
                if (md.IsWoEnableDown)
                {
                    AddBtn(NamesOfBtn.Download, " Download ", "DownLoad");
                }
                #endregion
            }
        }

        private void LoadFile()
        {
            string name = Request.QueryString["fileName"];
            var path = SystemConfig.PathOfDataUser + "\\FrmOfficeFiles\\" + name;

            var result = File.ReadAllBytes(path);

            Response.Clear();
            Response.BinaryWrite(result);
            Response.End();
        }

        public void LoadFrmData(MapAttrs mattrs, Entity en)
        {
            var mes = new MapExts(this.FK_MapData);
            var dictParams = new Dictionary<string, string>();
            var fields = new List<string>();

            dictParams.Add("No", WebUser.No);
            dictParams.Add("Name", WebUser.Name);
            dictParams.Add("FK_Dept", WebUser.FK_Dept);
            dictParams.Add("FK_DeptName", WebUser.FK_DeptName);

            if (mes.Count == 0)
            {
                ReplaceParams = GenerateParamsJsonString(dictParams);
                ReplaceFields = "[]";
                ReplaceDtlNos = "[]";
                ReplaceDtls = "[]";
                return;
            }

            MapExt item = mes.GetEntityByKey(MapExtAttr.ExtType, MapExtXmlList.PageLoadFull) as MapExt;
            if (item == null)
            {
                ReplaceParams = GenerateParamsJsonString(dictParams);
                ReplaceFields = "[]";
                ReplaceDtlNos = "[]";
                ReplaceDtls = "[]";
                return;
            }

            DataTable dt = null;
            string sql = item.Tag;
            if (string.IsNullOrEmpty(sql) == false)
            {
                /*  If you have filled the main table sql  */
                #region  Deal with sql Variable 
                string[] paras = this.RequestParas.Split('&');
                foreach (string str in paras)
                {
                    if (string.IsNullOrEmpty(str) || str.Contains("=") == false)
                        continue;
                    string[] kvs = str.Split('=');
                    sql = sql.Replace("@"+kvs[0], kvs[1]);
                }
                sql = sql.Replace("@WebUser.No", WebUser.No);
                sql = sql.Replace("@WebUser.Name", WebUser.Name);
                sql = sql.Replace("@WebUser.FK_Dept", WebUser.FK_Dept);
                sql = sql.Replace("@WebUser.FK_DeptName", WebUser.FK_DeptName);

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
                            dictParams.Add(dc.ColumnName, dr[dc.ColumnName].ToString());
                            fields.Add(dc.ColumnName);
                        }
                    }
                }
            }

            if (IsFirst)
                ReplaceParams = GenerateParamsJsonString(dictParams);

            ReplaceFields = GenerateFieldsJsonString(fields);

            if (string.IsNullOrEmpty(item.Tag1)
                || item.Tag1.Length < 15)
            {
                ReplaceDtls = "[]";
                ReplaceDtlNos = "[]";
                return;
            }

            ReplaceDtls = "[";
            ReplaceDtlNos = "[";
            MapDtls dtls = new MapDtls(this.FK_MapData);
            //  Filled from the table .
            foreach (MapDtl dtl in dtls)
            {
                ReplaceDtlNos += "\"" + dtl.No + "\",";

                if (!IsFirst)
                    continue;

                string[] sqls = item.Tag1.Split('*');
                foreach (string mysql in sqls)
                {
                    if (string.IsNullOrEmpty(mysql))
                        continue;

                    if (mysql.Contains(dtl.No + "=") == false)
                        continue;

                    #region  Deal with sql.
                    sql = mysql;
                    string[] paras = this.RequestParas.Split('&');
                    foreach (string str in paras)
                    {
                        if (string.IsNullOrEmpty(str) || str.Contains("=") == false)
                            continue;
                        string[] kvs = str.Split('=');
                        sql = sql.Replace("@" + kvs[0], kvs[1]);
                    }
                    sql = sql.Replace(dtl.No + "=", "");
                    sql = sql.Replace("@WebUser.No", WebUser.No);
                    sql = sql.Replace("@WebUser.Name", WebUser.Name);
                    sql = sql.Replace("@WebUser.FK_Dept", WebUser.FK_Dept);
                    sql = sql.Replace("@WebUser.FK_DeptName", WebUser.FK_DeptName);
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

                    try
                    {
                        gedtls.Delete(GEDtlAttr.RefPK, en.PKVal);
                    }
                    catch
                    {
                        gedtls.GetNewEntity.CheckPhysicsTable();
                    }

                    dt = DBAccess.RunSQLReturnTable(sql);
                    //dictDtls.Add(dtl.No, dt);
                    ReplaceDtls += "{\"dtlno\":\"" + dtl.No + "\",\"dtl\":[";
                    var idx = 1;
                    foreach (DataRow dr in dt.Rows)
                    {
                        ReplaceDtls += "{\"rowid\":" + (idx++) + ",\"cells\":[";
                        GEDtl gedtl = gedtls.GetNewEntity as GEDtl;

                        foreach (DataColumn dc in dt.Columns)
                        {
                            gedtl.SetValByKey(dc.ColumnName, dr[dc.ColumnName].ToString());
                            ReplaceDtls += "{\"key\":\"" + dc.ColumnName + "\",\"value\":\"" + dr[dc.ColumnName] +
                                           "\"},";
                        }

                        ReplaceDtls = ReplaceDtls.TrimEnd(',') + "]},";
                        gedtl.RefPK = en.PKVal.ToString();
                        gedtl.RDT = DataType.CurrentDataTime;
                        gedtl.Rec = WebUser.No;
                        gedtl.Insert();
                    }

                    ReplaceDtls = ReplaceDtls.TrimEnd(',') + "]}";
                }
            }

            ReplaceDtls = ReplaceDtls.TrimEnd(',') + "]";
            ReplaceDtlNos = ReplaceDtlNos.TrimEnd(',') + "]";
        }

        /// <summary>
        ///  Conversion set for key json String 
        /// </summary>
        /// <param name="dictParams"> Key set </param>
        /// <returns></returns>
        private string GenerateParamsJsonString(Dictionary<string, string> dictParams)
        {
            return "[" + dictParams.Aggregate(string.Empty, (curr, next) => curr + string.Format("{{\"key\":\"{0}\",\"value\":\"{1}\"}},", next.Key, (next.Value ?? "").Replace("\\", "\\\\").Replace("'", "\'"))).TrimEnd(',') + "]";
        }

        /// <summary>
        ///  Change String Set for json String 
        /// </summary>
        /// <param name="fields">String Set </param>
        /// <returns></returns>
        private string GenerateFieldsJsonString(List<string> fields)
        {
            return LitJson.JsonMapper.ToJson(fields);
            //return "[" + fields.Aggregate(string.Empty, (curr, next) => curr + "\"" + next + "\",").TrimEnd(',') + "]";
        }

        private void SaveFile()
        {
            try
            {
                HttpFileCollection files = HttpContext.Current.Request.Files;

                if (files.Count > 0)
                {
                    // Check the file name extension 
                    HttpPostedFile postedFile = files[0];
                    var fileName = Path.GetFileName(Request.QueryString["filename"]);
                    var path = SystemConfig.PathOfDataUser + @"\FrmOfficeFiles\" + this.FK_MapData;

                    if (fileName != "")
                    {
                        postedFile.SaveAs(path + "\\" + fileName);

                        var en = new GEEntityWordFrm(this.FK_MapData);
                        en.RetrieveFromDBSources();

                        en.LastEditer = WebUser.Name;
                        en.RDT = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        en.Update();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AddBtn(string id, string label, string clickEvent)
        {
            var btn = new LinkBtn(true);
            btn.ID = id;
            btn.Text = label;
            btn.Attributes["onclick"] = "return " + clickEvent + "()";
            btn.PostBackUrl = "javascript:void(0)";
            divMenu.Controls.Add(btn);
        }
    }

    #region  And front-end for JS Get on JSON Interaction defined auxiliary class 

    /// <summary>
    ///  Field 
    /// </summary>
    public class WordField
    {
        /// <summary>
        ///  Field Name 
        /// </summary>
        public string key { get; set; }
        /// <summary>
        ///  Field values 
        /// </summary>
        public string value { get; set; }
    }

    /// <summary>
    ///  List 
    /// </summary>
    public class WordDtlTable
    {
        /// <summary>
        ///  Schedule No. 
        /// </summary>
        public string dtlno { get; set; }
        /// <summary>
        ///  Schedule set of rows 
        /// </summary>
        public List<WordDtlRow> dtl { get; set; }
    }

    /// <summary>
    ///  Schedule Line 
    /// </summary>
    public class WordDtlRow
    {
        /// <summary>
        ///  Line number 
        /// </summary>
        public string rowid { get; set; }
        /// <summary>
        ///  Row cell data collection 
        /// </summary>
        public List<WordField> cells { get; set; }
    }
    #endregion
}