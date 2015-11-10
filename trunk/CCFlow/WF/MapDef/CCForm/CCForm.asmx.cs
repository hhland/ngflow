using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Services;
using BP.DA;
using BP.En;
using BP.Sys;
using BP.Tools;
using BP.WF;
using BP.WF.Template;

namespace BP.Web
{
    /// <summary>
    /// DA  The summary 
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    //  To allow the use of  ASP.NET AJAX  Call this from a script  Web  Service , Please cancel the downlink comment .
    // [System.Web.Script.Services.ScriptService]
    public class CCForm : System.Web.Services.WebService
    {
        #region  Public Methods 

        /// <summary>
        ///  Gets the value of 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebMethod]
        public string CfgKey(string key)
        {
            return BP.Sys.SystemConfig.AppSettings[key];
        }
        
        /// <summary>
        ///  Upload multiple attachments 
        /// </summary>
        /// <param name="WorkID"> Business Number </param>
        /// <param name="FK_Node"> Node number </param>
        /// <param name="FK_FrmAttachment"> Controls ID</param>
        /// <param name="UserNo"> Upload person account </param>
        /// <param name="UserName"> Upload person Chinese name </param>
        /// <param name="FileByte"> File size </param>
        /// <param name="fileName"> File name , Can not be empty </param>
        /// <param name="MyNote"> Remark , Can be empty </param>
        /// <param name="sort"> Sequence , Can be empty </param>
        /// <returns> Processing messages </returns>
        [WebMethod]
        public string AttachmentFiles(string WorkID, string FK_Node, string FK_FrmAttachment, string UserNo, string UserName, byte[] FileByte, String fileName, string MyNote, string sort)
        {

            try
            {
                string guid = BP.DA.DBAccess.GenerGUID();
                string exts = fileName.Substring(fileName.IndexOf(".") + 1);
                FrmAttachment athDesc = new FrmAttachment(FK_FrmAttachment);
                // If you have to upload the type of restrictions , Judging format 
                if (athDesc.Exts == "*.*" || athDesc.Exts == "")
                {
                    /* Any format can be uploaded */
                }
                else
                {
                    if (athDesc.Exts.ToLower().Contains(exts) == false)
                    {
                        return "error: The file you uploaded , Does not meet the format requirements of the system , Required file format :" + athDesc.Exts + ", You can now upload file format :" + exts;
                    }
                }

                // Save the file 
                string realSaveTo = UploadFile(FileByte, fileName, athDesc.SaveTo);
                FileInfo info = new FileInfo(realSaveTo);

                FrmAttachmentDB dbUpload = new FrmAttachmentDB();

                dbUpload.MyPK = guid;
                dbUpload.NodeID = FK_Node.ToString();

                if (athDesc.AthUploadWay == AthUploadWay.Inherit)
                {
                    /* If it is inherited , Let him keep the local PK. */
                    dbUpload.RefPKVal = WorkID;
                }

                if (athDesc.AthUploadWay == AthUploadWay.Interwork)
                {
                    /* If it is synergistic , Let him be PWorkID. */
                    string pWorkID = BP.DA.DBAccess.RunSQLReturnValInt("SELECT PWorkID FROM WF_GenerWorkFlow WHERE WorkID=" + WorkID, 0).ToString();
                    if (pWorkID == null || pWorkID == "0")
                        pWorkID = WorkID;

                    dbUpload.RefPKVal = pWorkID;
                }

                dbUpload.FK_MapData = athDesc.FK_MapData;
                dbUpload.FK_FrmAttachment = FK_FrmAttachment;

                dbUpload.FileExts = info.Extension;
                dbUpload.FileFullName = realSaveTo;
                dbUpload.FileName = info.Name;
                dbUpload.FileSize = (float)info.Length;

                dbUpload.RDT = DataType.CurrentDataTimess;
                dbUpload.Rec = UserNo;
                dbUpload.RecName = UserName;
                if (athDesc.IsNote)
                    dbUpload.MyNote = MyNote;

                if (!string.IsNullOrEmpty(sort))
                    dbUpload.Sort = sort;

                dbUpload.UploadGUID = guid;
                dbUpload.Insert();
            }
            catch (Exception ex)
            {
                return "error:" + ex.Message;
            }
            return "success";
        }
        /// <summary>
        ///  Save the file 
        /// </summary>
        /// <param name="FileByte"> File size </param>
        /// <param name="fileName"> File name </param>
        /// <param name="saveTo"> Save Path </param>
        /// <returns></returns>
        public string UploadFile(byte[] FileByte, String fileName, string saveTo)
        {
            // Create a directory .
            string pathSave = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + saveTo;
            if (!System.IO.Directory.Exists(pathSave))
                System.IO.Directory.CreateDirectory(pathSave);

            string filePath = pathSave + fileName;
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            // As used herein, the absolute path to index 
            FileStream stream = new FileStream(filePath, FileMode.CreateNew);
            stream.Write(FileByte, 0, FileByte.Length);
            stream.Close();

            DataSet ds = new DataSet();
            ds.ReadXml(filePath);

            return Silverlight.DataSetConnector.Connector.ToXml(ds);
        }
        /// <summary>
        ///  Upload file .
        /// </summary>
        /// <param name="FileByte"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [WebMethod]
        public string UploadFile(byte[] FileByte, String fileName)
        {
            // Create a temporary directory .
            string pathTemp = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\Temp";
            if (!System.IO.Directory.Exists(pathTemp))
                System.IO.Directory.CreateDirectory(pathTemp);

            string path = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
            string filePath = path + "\\" + fileName;
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            // As used herein, the absolute path to index 
            FileStream stream = new FileStream(filePath, FileMode.CreateNew);
            stream.Write(FileByte, 0, FileByte.Length);
            stream.Close();

            DataSet ds = new DataSet();
            ds.ReadXml(filePath);

            string strs = string.Empty;
            strs = FormatToJson.ToJson(ds);
            //strs =  Silverlight.DataSetConnector.Connector.ToXml(ds);
            return strs;
        }
        /// <summary>
        ///  Carried out sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public int RunSQL(string sql)
        {
            return BP.DA.DBAccess.RunSQL(sql);
        }

        [WebMethod(EnableSession = true)]
        public int RunSQLs(string sqls)
        {
            if (string.IsNullOrEmpty(sqls))
                return 0;

            int i = 0;
            string[] strs = sqls.Split('@');
            foreach (string str in strs)
            {
                if (string.IsNullOrEmpty(str))
                    continue;
                i += BP.DA.DBAccess.RunSQL(str);
            }
            return i;
        }
        /// <summary>
        ///  Run sql Return table.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        [WebMethod]
        public string RunSQLReturnTable(string sql)
        {
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            DataSet ds = new DataSet();
            ds.Tables.Add(BP.DA.DBAccess.RunSQLReturnTable(sql));
            return Silverlight.DataSetConnector.Connector.ToXml(ds);
        }
        /// <summary>
        ///  Run sql Return table.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        [WebMethod]
        public string RunSQLReturnJson(string sql)
        {
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            return BP.DA.DataType.ToJson(ds.Tables[0]);
        }
        /// <summary>
        ///  Run sql Return table.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        [WebMethod]
        public DataTable RunSQLReturnTable2DataTable(string sql)
        {
            return BP.DA.DBAccess.RunSQLReturnTable(sql);
        }
        /// <summary>
        ///  Run sql Return String.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        [WebMethod]
        public string RunSQLReturnString(string sql)
        {
            return BP.DA.DBAccess.RunSQLReturnString(sql);
        }
        /// <summary>
        ///  Run sql Return String.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        [WebMethod]
        public int RunSQLReturnValInt(string sql)
        {
            return BP.DA.DBAccess.RunSQLReturnValInt(sql);
        }
        /// <summary>
        ///  Run sql Return float.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        [WebMethod]
        public float RunSQLReturnValFloat(string sql)
        {
            return BP.DA.DBAccess.RunSQLReturnValFloat(sql);
        }
        /// <summary>
        ///  Run sql Return table.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        [WebMethod]
        public string RunSQLReturnTableS(string sqls)
        {
            string[] strs = sqls.Split('@');
            DataSet ds = new DataSet();
            int i = 0;
            foreach (string sql in strs)
            {
                if (string.IsNullOrEmpty(sql))
                    continue;
                DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
                dt.TableName = "DT" + i;
                ds.Tables.Add(dt);
                i++;
            }
            return Silverlight.DataSetConnector.Connector.ToXml(ds);
        }
        /// <summary>
        ///  Will be transformed into Chinese Pinyin .
        /// </summary>
        /// <param name="name"></param>
        /// <param name="flag">true:  Spelling ,false :  Jianpin </param>
        /// <returns></returns>
        [WebMethod]
        public string ParseStringToPinyin(string name, bool flag)
        {
            string s = string.Empty; ;
            try
            {
                if (flag)
                {
                    s = BP.DA.DataType.ParseStringToPinyin(name);
                    if (s.Length > 15)
                        s = BP.DA.DataType.ParseStringToPinyinWordFirst(name);
                }
                else
                {
                    s = BP.DA.DataType.ParseStringToPinyinJianXie(name);
                }

                s = s.Trim().Replace(" ", "");
                s = s.Trim().Replace(" ", "");
                s = s.Replace(",", "");
                s = s.Replace(".", "");
                return s;
            }
            catch
            {
                return null;
            }
        }

        private string DealPK(string pk, string fromMapdata, string toMapdata)
        {
            if (pk.Contains("*" + fromMapdata))
                return pk.Replace("*" + toMapdata, "*" + toMapdata);
            else
                return pk + "*" + toMapdata;
        }
        public void LetAdminLogin()
        {
            BP.Port.Emp emp = new BP.Port.Emp("admin");
            BP.Web.WebUser.SignInOfGener(emp);
        }
        /// <summary>
        ///  Let him log in 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="sid"></param>
        public void LetUserLogin(string user, string sid)
        {
            BP.Port.Emp emp = new BP.Port.Emp(user);
            BP.Web.WebUser.SignInOfGener(emp);
        }

        /// <summary>
        ///  Save Enum.
        /// </summary>
        /// <param name="enumKey"></param>
        /// <param name="enumLab"></param>
        /// <param name="cfg"></param>
        /// <returns></returns>
        [WebMethod]
        public string SaveEnum(string enumKey, string enumLab, string cfg)
        {
            SysEnumMain sem = new SysEnumMain();
            sem.No = enumKey;
            if (sem.RetrieveFromDBSources() == 0)
            {
                sem.Name = enumLab;
                sem.CfgVal = cfg;
                sem.Lang = WebUser.SysLang;
                sem.Insert();
            }
            else
            {
                sem.Name = enumLab;
                sem.CfgVal = cfg;
                sem.Lang = WebUser.SysLang;
                sem.Update();
            }

            string[] strs = cfg.Split('@');
            foreach (string str in strs)
            {
                if (string.IsNullOrEmpty(str))
                    continue;
                string[] kvs = str.Split('=');
                SysEnum se = new SysEnum();
                se.EnumKey = enumKey;
                se.Lang = WebUser.SysLang;
                se.IntKey = int.Parse(kvs[0]);
                se.Lab = kvs[1];
                se.Insert();
            }
            return "save ok.";
        }

        /// <summary>
        ///  Save enumeration field 
        /// </summary>
        /// <param name="fk_mapData"> Form ID</param>
        /// <param name="fieldKey"> Field values </param>
        /// <param name="fieldName">名</param>
        /// <param name="enumKey"> Enum value </param>
        /// <returns> Whether successfully saved </returns>
        [WebMethod]
        public string SaveEnumField(string fk_mapData, string fieldKey, string fieldName, string enumKey, double x, double y)
        {
            try
            {
                MapAttr attr = new MapAttr();
                attr.MyPK = fk_mapData + "_" + fieldKey;
                if (attr.IsExits == true)
                    return " Field {" + fieldKey + "} Already exists .";

                attr.FK_MapData = fk_mapData;
                attr.KeyOfEn = fieldKey;
                attr.Name = fieldName;
                attr.MyDataType = DataType.AppInt;

                attr.X = float.Parse(x.ToString());
                attr.Y = float.Parse(y.ToString());

                attr.UIBindKey = enumKey;
                attr.UIContralType = UIContralType.DDL;
                attr.LGType = FieldTypeS.Enum;
                attr.Insert();
                return "OK";
            }
            catch (Exception ex)
            {
                return "@ Error Messages :" + ex.Message;
                // +" StackTrace:" + ex.StackTrace;
                //                return "@ Error Messages :" + ex.Message + " StackTrace:" + ex.StackTrace;
            }
        }
        /// <summary>
        ///  Save the foreign key field 
        /// </summary>
        /// <param name="fk_mapData"> Form ID</param>
        /// <param name="fieldKey"> Field values </param>
        /// <param name="fieldName">名</param>
        /// <param name="enName"> Enum value </param>
        /// <returns> Whether successfully saved </returns>
        [WebMethod]
        public string SaveFKField(string fk_mapData, string fieldKey, string fieldName, string enName, double x, double y)
        {
            try
            {
                MapAttr attr = new MapAttr();
                attr.MyPK = fk_mapData + "_" + fieldKey;
                if (attr.IsExits == true)
                    return " Field {" + fieldKey + "} Already exists .";

                attr.FK_MapData = fk_mapData;
                attr.KeyOfEn = fieldKey;
                attr.Name = fieldName;
                attr.MyDataType = DataType.AppString;

                attr.X = float.Parse(x.ToString());
                attr.Y = float.Parse(y.ToString());

                attr.UIBindKey = enName;
                attr.UIContralType = UIContralType.DDL;
                attr.LGType = FieldTypeS.FK;
                attr.Insert();
                return "OK";
            }
            catch (Exception ex)
            {
                return "@ Error Messages :" + ex.Message;
            }
        }
        #endregion


        [WebMethod]
        public string SaveImageAsFile(byte[] img, string pkval, string fk_Frm_Ele)
        {
            FrmEle fe = new FrmEle(fk_Frm_Ele);
            System.Drawing.Image newImage;
            using (MemoryStream ms = new MemoryStream(img, 0, img.Length))
            {
                ms.Write(img, 0, img.Length);
                newImage = Image.FromStream(ms, true);
                Bitmap bitmap = new Bitmap(newImage, new Size(fe.WOfInt, fe.HOfInt));

                if (System.IO.Directory.Exists(fe.HandSigantureSavePath + "\\" + fe.FK_MapData + "\\") == false)
                    System.IO.Directory.CreateDirectory(fe.HandSigantureSavePath + "\\" + fe.FK_MapData + "\\");

                string saveTo = fe.HandSigantureSavePath + "\\" + fe.FK_MapData + "\\" + pkval + ".jpg";
                bitmap.Save(saveTo, ImageFormat.Jpeg);

                string pathFile = System.Web.HttpContext.Current.Request.ApplicationPath + fe.HandSiganture_UrlPath + fe.FK_MapData + "/" + pkval + ".jpg";
                FrmEleDB ele = new FrmEleDB();
                ele.FK_MapData = fe.FK_MapData;
                ele.EleID = fe.EleID;
                ele.RefPKVal = pkval;
                ele.Tag1 = pathFile.Replace("\\\\", "\\");
                ele.Tag1 = pathFile.Replace("////", "//");

                ele.Tag2 = saveTo.Replace("\\\\", "\\");
                ele.Tag2 = saveTo.Replace("////", "//");

                ele.GenerPKVal();
                ele.Save();

                return pathFile;
                // return "../DataUser/" + realpath + strFileName + ".png";
            }
            //FrmEleDB db = new FrmEleDB();
            //db.MyPK= 
            //return "error";
        }

        /// <summary>
        ///  Loading a form template 
        /// </summary>
        /// <param name="fileByte"> Byte count </param>
        /// <param name="fk_mapData"></param>
        /// <param name="isClear"></param>
        /// <returns></returns>
        [WebMethod]
        public string LoadFrmTemplete(byte[] fileByte, string fk_mapData, bool isClear)
        {
            try
            {

                string file = "\\Temp\\" + fk_mapData + ".xml";
                UploadFile(fileByte, file);
                string path = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + file;
                this.LoadFrmTempleteFile(path, fk_mapData, isClear, true);
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        ///  Import form 
        /// </summary>
        /// <param name="file"> File </param>
        /// <param name="fk_mapData"> Form ID</param>
        /// <param name="isClear"> Whether to clear existing elements </param>
        /// <param name="isSetReadonly"> Read only if set ?</param>
        /// <returns> Import results </returns>
        [WebMethod]
        public string LoadFrmTempleteFile(string file, string fk_mapData, bool isClear, bool isSetReadonly)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(file);
                MapData.ImpMapData(fk_mapData, ds, isSetReadonly);
                if (fk_mapData.Contains("ND"))
                {
                    /*  It is determined whether the node form  */
                    int nodeID = 0;
                    try
                    {
                        nodeID = int.Parse(fk_mapData.Replace("ND", ""));
                    }
                    catch
                    {
                        return null;
                    }
                    Node nd = new Node(nodeID);
                    nd.RepareMap();
                }
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        ///  Get xml Data 
        /// </summary>
        /// <param name="xmlFileName"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetXmlData(string xmlFileName)
        {
            DataTable dt = new DataTable("o");
            dt.Columns.Add(new DataColumn("DFor"));
            dt.Columns.Add(new DataColumn("Tag1"));
            dt.Columns.Add(new DataColumn("Tag2"));
            dt.Columns.Add(new DataColumn("Tag3"));
            dt.Columns.Add(new DataColumn("Tag4"));

            DataRow dr = dt.NewRow();
            dr["DFor"] = "HandSiganture";
            dr["Tag1"] = "@Label= Storage path @FType=String@DefVal=D:\\ccflow\\VisualFlow\\DataUser\\BPPaint\\";
            dr["Tag2"] = "@Label= Window opening height @FType=Int@DefVal=500";
            dr["Tag3"] = "@Label= Open window width @FType=Int@DefVal=200";
            dr["Tag4"] = "@Label=UrlPath@FType=String@DefVal=/DataUser/BPPaint/";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["DFor"] = "EleSiganture";
            dr["Tag1"] = "@Label= Location @FType=String";
            dr["Tag2"] = "@Label= Height @FType=Int";
            dr["Tag3"] = "@Label= Width @FType=Int";
            dr["Tag4"] = "";
            dt.Rows.Add(dr);

            DataSet myds = new DataSet();
            myds.Tables.Add(dt);
            return Silverlight.DataSetConnector.Connector.ToXml(myds);
        }
        [WebMethod]
        public string DoType(string dotype, string v1, string v2, string v3, string v4, string v5)
        {
            string sql = "";
            try
            {
                switch (dotype)
                {
                    case "CreateCheckGroup":
                        string gKey = v1;
                        string gName = v2;
                        string enName1 = v3;

                        MapAttr attrN = new MapAttr();
                        int i = attrN.Retrieve(MapAttrAttr.FK_MapData, enName1, MapAttrAttr.KeyOfEn, gKey + "_Note");
                        i += attrN.Retrieve(MapAttrAttr.FK_MapData, enName1, MapAttrAttr.KeyOfEn, gKey + "_Checker");
                        i += attrN.Retrieve(MapAttrAttr.FK_MapData, enName1, MapAttrAttr.KeyOfEn, gKey + "_RDT");
                        if (i > 0)
                            return " Prefixes have been used :" + gKey + " ,  Please confirm that you add this audit group or , Please replace other prefixes .";

                        GroupField gf = new GroupField();
                        gf.Lab = gName;
                        gf.EnName = enName1;
                        gf.Insert();

                        attrN = new MapAttr();
                        attrN.FK_MapData = enName1;
                        attrN.KeyOfEn = gKey + "_Note";
                        attrN.Name = " Audit opinion ";
                        attrN.MyDataType = DataType.AppString;
                        attrN.UIContralType = UIContralType.TB;
                        attrN.UIIsEnable = true;
                        attrN.UIIsLine = true;
                        attrN.MaxLen = 4000;
                        attrN.GroupID = gf.OID;
                        attrN.UIHeight = 23 * 3;
                        attrN.IDX = 1;
                        attrN.Insert();

                        attrN = new MapAttr();
                        attrN.FK_MapData = enName1;
                        attrN.KeyOfEn = gKey + "_Checker";
                        attrN.Name = " Reviewer ";// " Reviewer ";
                        attrN.MyDataType = DataType.AppString;
                        attrN.UIContralType = UIContralType.TB;
                        attrN.MaxLen = 50;
                        attrN.MinLen = 0;
                        attrN.UIIsEnable = true;
                        attrN.UIIsLine = false;
                        attrN.DefVal = "@WebUser.Name";
                        attrN.UIIsEnable = false;
                        attrN.GroupID = gf.OID;
                        attrN.IsSigan = true;
                        attrN.IDX = 2;
                        attrN.Insert();

                        attrN = new MapAttr();
                        attrN.FK_MapData = enName1;
                        attrN.KeyOfEn = gKey + "_RDT";
                        attrN.Name = " Review Date "; // " Review Date ";
                        attrN.MyDataType = DataType.AppDateTime;
                        attrN.UIContralType = UIContralType.TB;
                        attrN.UIIsEnable = true;
                        attrN.UIIsLine = false;
                        attrN.DefVal = "@RDT";
                        attrN.UIIsEnable = false;
                        attrN.GroupID = gf.OID;
                        attrN.IDX = 3;
                        attrN.Insert();

                        /*
                         *  Is reviewed to determine whether the packet node set , If the focus is on the field for the node set .
                         */

                        string frmID = attrN.FK_MapData;
                        frmID = frmID.Replace("ND", "");
                        int nodeid = 0;
                        try
                        {
                            nodeid = int.Parse(frmID);
                        }
                        catch
                        {
                            // Transformation is not a node form fields unsuccessful .
                            return null;
                        }

                        Node nd = new Node();
                        nd.NodeID = nodeid;
                        if (nd.RetrieveFromDBSources()!=0)
                        {
                            if (string.IsNullOrEmpty(nd.FocusField) == false)
                                return null;

                            nd.FocusField = "@" + gKey + "_Note";
                            nd.Update();
                        }
                        return null;
                    case "NewDtl":
                        MapDtl dtlN = new MapDtl();
                        dtlN.No = v1;
                        if (dtlN.RetrieveFromDBSources() != 0)
                            return " From the table already exists ";
                        dtlN.Name = v1;
                        dtlN.FK_MapData = v2;
                        dtlN.PTable = v1;
                        dtlN.Insert();
                        dtlN.IntMapAttrs();
                        return null;
                    case "DelM2M":
                        MapM2M m2mDel = new MapM2M();
                        m2mDel.MyPK = v1;
                        m2mDel.Delete();
                        //M2M m2mData = new M2M();
                        //m2mData.Delete(M2MAttr.FK_MapData, v1);
                        return null;
                    case "NewAthM": //  New  NewAthM. 
                        string fk_mapdataAth = v1;
                        string athName = v2;

                        BP.Sys.FrmAttachment athM = new FrmAttachment();
                        athM.MyPK = athName;
                        if (athM.IsExits)
                            return " Multiple choice name :" + athName + ", Already exists .";

                        athM.X = float.Parse(v3);
                        athM.Y = float.Parse(v4);
                        athM.Name = " Multiple file uploads ";
                        athM.FK_MapData = fk_mapdataAth;
                        athM.Insert();
                        return null;
                    case "NewM2M":
                        string fk_mapdataM2M = v1;
                        string m2mName = v2;
                        MapM2M m2m = new MapM2M();
                        m2m.FK_MapData = v1;
                        m2m.NoOfObj = v2;
                        if (v3 == "0")
                        {
                            m2m.HisM2MType = M2MType.M2M;
                            m2m.Name = " New-to-many ";
                        }
                        else
                        {
                            m2m.HisM2MType = M2MType.M2MM;
                            m2m.Name = " New many more ";
                        }

                        m2m.X = float.Parse(v4);
                        m2m.Y = float.Parse(v5);
                        m2m.MyPK = m2m.FK_MapData + "_" + m2m.NoOfObj;
                        if (m2m.IsExits)
                            return " Multiple choice name :" + m2mName + ", Already exists .";
                        m2m.Insert();
                        return null;
                    case "DelEnum":

                        // Delete empty data .
                        BP.DA.DBAccess.RunSQL("DELETE FROM Sys_MapAttr WHERE FK_MapData Is null or FK_MapData='' ");

                        //  Check whether the physical table is used .
                        sql = "SELECT  * FROM Sys_MapAttr WHERE UIBindKey='" + v1 + "'";
                        DataTable dtEnum = DBAccess.RunSQLReturnTable(sql);
                        string msgDelEnum = "";
                        foreach (DataRow dr in dtEnum.Rows)
                        {
                            msgDelEnum += "\n  Form Number :" + dr["FK_MapData"] + " ,  Field :" + dr["KeyOfEn"] + ",  Name :" + dr["Name"];
                        }

                        if (msgDelEnum != "")
                            return " This enumeration has been cited in the following fields , You can not delete it ." + msgDelEnum;

                        sql = "DELETE FROM Sys_EnumMain WHERE No='" + v1 + "'";
                        sql += "@DELETE FROM Sys_Enum WHERE EnumKey='" + v1 + "' ";
                        DBAccess.RunSQLs(sql);
                        return null;
                    case "DelSFTable": /*  Delete the physical table custom . */
                        //  Check whether the physical table is used .
                        sql = "SELECT  * FROM Sys_MapAttr WHERE UIBindKey='" + v1 + "'";
                        DataTable dt = DBAccess.RunSQLReturnTable(sql);
                        string msgDel = "";
                        foreach (DataRow dr in dt.Rows)
                        {
                            msgDel += "\n  Form Number :" + dr["FK_MapData"] + " ,  Field :" + dr["KeyOfEn"] + ",  Name :" + dr["Name"];
                        }

                        if (msgDel != "")
                            return " The data table has been referenced by the following fields , You can not delete it ." + msgDel;

                        SFTable sfDel = new SFTable();
                        sfDel.No = v1;
                        sfDel.DirectDelete();
                        return null;
                    case "SaveSFTable":
                        string enName = v2;
                        string chName = v1;
                        if (string.IsNullOrEmpty(v1) || string.IsNullOrEmpty(v2))
                            return " View English name can not be empty .";

                        SFTable sf = new SFTable();
                        sf.No = enName;
                        sf.Name = chName;

                        sf.No = enName;
                        sf.Name = chName;

                        sf.FK_Val = enName;
                        sf.Save();
                        if (DBAccess.IsExitsObject(enName) == true)
                        {
                            /* This object already exists , Check whether there No,Name列.*/
                            sql = "SELECT No,Name FROM " + enName;
                            try
                            {
                                DBAccess.RunSQLReturnTable(sql);
                            }
                            catch (Exception ex)
                            {
                                return " You specify a table or view (" + enName + "), Does not contain No,Name Two , Does not comply ccflow Rules agreed . Technical Information :" + ex.Message;
                            }
                            return null;
                        }
                        else
                        {
                            /* Creating the table , And insert the underlying data .*/
                            try
                            {
                                //  If there is no table or view , We must create it .
                                sql = "CREATE TABLE " + enName + "(No varchar(30) NOT NULL,Name varchar(50) NULL)";
                                DBAccess.RunSQL(sql);
                                DBAccess.RunSQL("INSERT INTO " + enName + " (No,Name) VALUES('001','Item1')");
                                DBAccess.RunSQL("INSERT INTO " + enName + " (No,Name) VALUES('002','Item2')");
                                DBAccess.RunSQL("INSERT INTO " + enName + " (No,Name) VALUES('003','Item3')");
                            }
                            catch (Exception ex)
                            {
                                sf.DirectDelete();
                                return " Error occurred during the creation of a physical table , May be illegal physical table name . Technical Information :" + ex.Message;
                            }
                        }
                        return null; /* After creating a successful return null */
                    case "FrmTempleteExp":  // Export Form .
                        MapData mdfrmtem = new MapData();
                        mdfrmtem.No = v1;
                        if (mdfrmtem.RetrieveFromDBSources() == 0)
                        {
                            if (v1.Contains("ND"))
                            {
                                int nodeId = int.Parse(v1.Replace("ND", ""));
                                Node nd123 = new Node(nodeId);
                                mdfrmtem.Name = nd123.Name;
                                mdfrmtem.PTable = v1;
                                mdfrmtem.EnPK = "OID";
                                mdfrmtem.Insert();
                            }
                        }

                        DataSet ds = mdfrmtem.GenerHisDataSet();
                        string file = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\Temp\\" + v1 + ".xml";
                        if (System.IO.File.Exists(file))
                            System.IO.File.Delete(file);
                        ds.WriteXml(file);

                        // BP.Sys.PubClass.DownloadFile(file, mdfrmtem.Name + ".xml");
                        //this.DownLoadFile(System.Web.HttpContext.Current, file, mdfrmtem.Name);
                        return null;
                    case "FrmTempleteImp": // Import form .
                        DataSet dsImp = new DataSet();
                        string fileImp = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\Temp\\" + v1 + ".xml";
                        dsImp.ReadXml(fileImp); // Read the file .
                        MapData.ImpMapData(v1, dsImp, true);
                        return null;
                    case "NewHidF":
                        string fk_mapdataHid = v1;
                        string key = v2;
                        string name = v3;
                        int dataType = int.Parse(v4);
                        MapAttr mdHid = new MapAttr();
                        mdHid.MyPK = fk_mapdataHid + "_" + key;
                        mdHid.FK_MapData = fk_mapdataHid;
                        mdHid.KeyOfEn = key;
                        mdHid.Name = name;
                        mdHid.MyDataType = dataType;
                        mdHid.HisEditType = EditType.Edit;
                        mdHid.MaxLen = 100;
                        mdHid.MinLen = 0;
                        mdHid.LGType = FieldTypeS.Normal;
                        mdHid.UIVisible = false;
                        mdHid.UIIsEnable = false;
                        mdHid.Insert();
                        return null;
                    case "DelDtl":
                        MapDtl dtl = new MapDtl(v1);
                        dtl.Delete();
                        return null;
                    case "DelWorkCheck":

                        BP.Sys.FrmWorkCheck check = new FrmWorkCheck();
                        check.No = v1;
                        check.Delete();
                        return null;
                    case "DeleteFrm":
                        string delFK_Frm = v1;
                        MapData mdDel = new MapData(delFK_Frm);
                        mdDel.Delete();
                        sql = "@DELETE FROM Sys_MapData WHERE No='" + delFK_Frm + "'";
                        sql = "@DELETE FROM WF_FrmNode WHERE FK_Frm='" + delFK_Frm + "'";
                        DBAccess.RunSQLs(sql);
                        return null;
                    case "FrmUp":
                    case "FrmDown":
                        FrmNode myfn = new FrmNode();
                        myfn.Retrieve(FrmNodeAttr.FK_Node, v1, FrmNodeAttr.FK_Frm, v2);
                        if (dotype == "FrmUp")
                            myfn.DoUp();
                        else
                            myfn.DoDown();
                        return null;
                    case "SaveFlowFrm":
                        //  Transformation Parameters significance .
                        string vals = v1;
                        string fk_Node = v2;
                        string fk_flow = v3;
                        bool isPrint = false;
                        if (v5 == "1")
                            isPrint = true;

                        bool isReadonly = false;
                        if (v4 == "1")
                            isReadonly = true;

                        string msg = this.SaveEn(vals);
                        if (msg.Contains("Error"))
                            return msg;

                        string fk_frm = msg;
                        Frm fm = new Frm();
                        fm.No = fk_frm;
                        fm.Retrieve();

                        FrmNode fn = new FrmNode();
                        if (fn.Retrieve(FrmNodeAttr.FK_Frm, fk_frm,
                            FrmNodeAttr.FK_Node, fk_Node) == 1)
                        {
                            fn.IsEdit = !isReadonly;
                            fn.IsPrint = isPrint;
                            fn.FK_Flow = fk_flow;
                            fn.Update();
                            BP.DA.DBAccess.RunSQL("UPDATE Sys_MapData SET FK_FrmSort='01',AppType=1  WHERE No='" + fk_frm + "'");
                            return fk_frm;
                        }

                        fn.FK_Frm = fk_frm;
                        fn.FK_Flow = fk_flow;
                        fn.FK_Node = int.Parse(fk_Node);
                        fn.IsEdit = !isReadonly;
                        fn.IsPrint = isPrint;
                        fn.Idx = 100;
                        fn.FK_Flow = fk_flow;
                        fn.Insert();

                        MapData md = new MapData();
                        md.No = fm.No;
                        if (md.RetrieveFromDBSources() == 0)
                        {
                            md.Name = fm.Name;
                            md.EnPK = "OID";
                            md.Insert();

                        }

                        MapAttr attr = new MapAttr();
                        attr.FK_MapData = md.No;
                        attr.KeyOfEn = "OID";
                        attr.Name = "WorkID";
                        attr.MyDataType = BP.DA.DataType.AppInt;
                        attr.UIContralType = UIContralType.TB;
                        attr.LGType = FieldTypeS.Normal;
                        attr.UIVisible = false;
                        attr.UIIsEnable = false;
                        attr.DefVal = "0";
                        attr.HisEditType = BP.En.EditType.Readonly;
                        attr.Insert();

                        attr = new MapAttr();
                        attr.FK_MapData = md.No;
                        attr.KeyOfEn = "FID";
                        attr.Name = "FID";
                        attr.MyDataType = BP.DA.DataType.AppInt;
                        attr.UIContralType = UIContralType.TB;
                        attr.LGType = FieldTypeS.Normal;
                        attr.UIVisible = false;
                        attr.UIIsEnable = false;
                        attr.DefVal = "0";
                        attr.HisEditType = BP.En.EditType.Readonly;
                        attr.Insert();

                        attr = new MapAttr();
                        attr.FK_MapData = md.No;
                        attr.KeyOfEn = "RDT";
                        attr.Name = " Record Date ";
                        attr.MyDataType = BP.DA.DataType.AppDateTime;
                        attr.UIContralType = UIContralType.TB;
                        attr.LGType = FieldTypeS.Normal;
                        attr.UIVisible = false;
                        attr.UIIsEnable = false;
                        attr.DefVal = "@RDT";
                        attr.HisEditType = BP.En.EditType.Readonly;
                        attr.Insert();
                        return fk_frm;
                    default:
                        return "Error:" + dotype + " ,  This tag is not set .";
                }
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }
        /// <summary>
        ///  Save entity.
        ///  Case : @EnName=BP.Sys.FrmLabel@PKVal=Lin13b@X=100@Y=299@Text= My Tags 
        /// </summary>
        /// <param name="vals"></param>
        /// <returns></returns>
        [WebMethod]
        public string SaveEn(string vals)
        {
            Entity en = null;
            try
            {
                AtPara ap = new AtPara(vals);
                string enName = ap.GetValStrByKey("EnName");
                string pk = ap.GetValStrByKey("PKVal");
                en = ClassFactory.GetEn(enName);
                en.ResetDefaultVal();

                if (en == null)
                    throw new Exception(" Invalid class name :" + enName);

                if (string.IsNullOrEmpty(pk) == false)
                {
                    en.PKVal = pk;
                    en.RetrieveFromDBSources();
                }

                foreach (string key in ap.HisHT.Keys)
                {
                    if (key == "PKVal")
                        continue;
                    en.SetValByKey(key, ap.HisHT[key].ToString().Replace('^', '@'));
                }
                en.Save();
                return en.PKVal as string;
            }
            catch (Exception ex)
            {
                if (en != null)
                    en.CheckPhysicsTable();
                return "Error:" + ex.Message;
            }
        }

        /// <summary>
        ///  Get path 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [WebMethod]
        public string FtpMethod(string doType, string v1, string v2, string v3)
        {
            try
            {
                FtpSupport.FtpConnection conn = new FtpSupport.FtpConnection("192.168.1.138", "administrator", "jiaozi");
                switch (doType)
                {
                    case "ShareFrm": /* Shared Templates */
                        MapData md = new MapData();
                        DataSet ds = md.GenerHisDataSet();
                        string file = BP.Sys.SystemConfig.PathOfTemp + v1 + "_" + v2 + "_" + DateTime.Now.ToString("MM-dd hh-mm") + ".xml";
                        ds.WriteXml(file);
                        conn.SetCurrentDirectory("/");
                        conn.SetCurrentDirectory("/Upload.Form/");
                        conn.SetCurrentDirectory(v3);
                        conn.PutFile(file, md.Name + ".xml");
                        conn.Close();
                        return null;
                    case "GetDirs":
                        //   return "@01. Daily office @02. Human Resources @03. Other categories ";
                        conn.SetCurrentDirectory(v1);
                        FtpSupport.Win32FindData[] dirs = conn.FindFiles();
                        conn.Close();
                        string dirsStr = "";
                        foreach (FtpSupport.Win32FindData dir in dirs)
                        {
                            dirsStr += "@" + dir.FileName;
                        }
                        return dirsStr;
                    case "GetFls":
                        conn.SetCurrentDirectory(v1);
                        FtpSupport.Win32FindData[] fls = conn.FindFiles();
                        conn.Close();
                        string myfls = "";
                        foreach (FtpSupport.Win32FindData fl in fls)
                        {
                            myfls += "@" + fl.FileName;
                        }
                        return myfls;
                    case "LoadTempleteFile":
                        string fileFtpPath = v1;
                        conn.SetCurrentDirectory("/Form. Form templates /");
                        conn.SetCurrentDirectory(v3);

                        /* Download the file to the specified directory : */
                        string tempFile = BP.Sys.SystemConfig.PathOfTemp + "\\" + v2 + ".xml";
                        conn.GetFile(v1, tempFile, false, FileAttributes.Archive, FtpSupport.FtpTransferType.Ascii);
                        return this.LoadFrmTempleteFile(tempFile, v2, true, true);
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }
        /// <summary>
        ///  Get foreign key data 
        /// </summary>
        /// <param name="uiBindKey"> Foreign key </param>
        /// <returns> Foreign Key Data </returns>
        [WebMethod]
        public string RequestSFTableV1(string uiBindKey)
        {
            if (string.IsNullOrEmpty(uiBindKey))
                throw new Exception("@uiBindKey Can not be null .");

            DataSet ds = new DataSet();
            ds.Tables.Add(PubClass.GetDataTableByUIBineKeyForCCFormDesigner(uiBindKey));
            return Silverlight.DataSetConnector.Connector.ToXml(ds);
        }

        #region  Produce  page  Menu 
        public void InitFrm(string fk_mapdata)
        {
            try
            {
                BP.Sys.PubClass.InitFrm(fk_mapdata);
            }
            catch (Exception ex)
            {
                FrmLines lines = new FrmLines();
                lines.Delete(FrmLabAttr.FK_MapData, fk_mapdata);
                throw ex;
            }
        }
        private DataSet ds = null;
        /// <summary>
        ///  Get a Frm
        /// </summary>
        /// <param name="fk_mapdata">map</param>
        /// <param name="workID"> For 0</param>
        /// <returns>form Description </returns>
        [WebMethod]
        public string GenerFrm(string fk_mapdata, int workID)
        {
            try
            {
                this.ds = MapData.GenerHisDataSet(fk_mapdata);
                if (this.ds == null || this.ds.Tables.Count <= 0)
                {
                    MapData md = new MapData();
                    md.No = fk_mapdata;
                    if (md.RetrieveFromDBSources() == 0)
                    {
                        MapDtl dtl = new MapDtl();
                        dtl.No = fk_mapdata;
                        if (dtl.RetrieveFromDBSources() == 0)
                            throw new Exception(" Load error , The form ID=" + fk_mapdata + " Lose , Please reload the repair process once again .");
                        else
                        {
                            md.Copy(dtl);
                            md.DirectInsert();
                        }
                    }
                }
              
                string json = FormatToJson.ToJson(ds);
                //string xml =  Silverlight.DataSetConnector.Connector.ToXml(ds);
                return json;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion  Produce  frm

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromMapData"></param>
        /// <param name="fk_mapdata"></param>
        /// <param name="isClear"> Are Clear </param>
        /// <param name="isSetReadonly"> Is set to read-only ?</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public string CopyFrm(string fromMapData, string fk_mapdata, bool isClear, bool isSetReadonly)
        {
            isSetReadonly = true;
            this.LetAdminLogin();

            MapData md = new MapData(fromMapData);
            MapData.ImpMapData(fk_mapdata, md.GenerHisDataSet(), isSetReadonly);

            //  If a node form , Must perform a repair , So as not to miss the field should be some system .
            if (fk_mapdata.Contains("ND") == true)
            {
                try
                {
                    string fk_node = fk_mapdata.Replace("ND", "");
                    Node nd = new Node(int.Parse(fk_node));
                    nd.RepareMap();
                }
                catch
                {
                    //  Does not handle exceptions .
                }
            }
            return null;
        }

     
        [WebMethod]
        public string SaveFrm(string fk_mapdata, string xml, string sqls, string mapAttrKeyName)
        {
            DataSet ds = new DataSet();
            try
            {
                //ds.ReadXml(sr);
                ds = FormatToJson.JsonToDataSet(xml);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
       
            string str = "";
            foreach (DataTable dt in ds.Tables)
            {
                try
                {
                    str += SaveDT(dt);
                    if (dt.TableName.ToLower() == "wf_node" && dt.Columns.Count >0 && dt.Rows.Count ==1)
                    {
                        /*  Update audit component status . */
                        string nodeid = fk_mapdata.Replace("ND", "");
                        BP.DA.DBAccess.RunSQL("UPDATE WF_Node SET FWCType=1 WHERE FWCType =0 AND NodeID=" + nodeid);
                    }
                }
                catch (Exception ex)
                {
                    str += "@ Save " + dt.TableName + " Failure :" + ex.Message;
                }
            }

            #region  Compatible database processing 
            if (BP.Sys.SystemConfig.AppCenterDBType == DBType.Oracle)
            {
                sqls = sqls.Replace("LEN(", "LENGTH(");
            }

            sqls += "@UPDATE Sys_MapAttr SET Name='' WHERE FK_MapData='" + fk_mapdata + "'  AND Name IS NULL ";
            sqls += "@UPDATE Sys_MapAttr SET UIVisible=1 WHERE FK_MapData='" + fk_mapdata + "' AND UIVisible is null";
            sqls += "@UPDATE Sys_MapAttr SET UIIsEnable=1 WHERE FK_MapData='" + fk_mapdata + "' AND UIIsEnable is null";
            sqls += "@UPDATE Sys_MapAttr SET UIIsLine=0 WHERE FK_MapData='" + fk_mapdata + "' AND UIIsLine is null";

            #endregion  Handle database compatibility issues 

            //  Updated version number , Execution sql, It is not necessary Retrieve
            string sql = "@UPDATE Sys_MapData SET VER='" + BP.DA.DataType.CurrentDataTimess + "' WHERE No='" + fk_mapdata + "'";
            string fiximgsealsql = "@UPDATE SYS_FRMIMG set IMGAPPTYPE=1 where FK_MAPDATA='" + fk_mapdata+"' and MYPK like 'ImgSeal%' ";
            sqls += sql;
            sqls += fiximgsealsql;

            RunSQLs(sqls);

            #region  Save field names solve problems .
            //string  sql = "SELECT KeyOfEn FROM Sys_MapAttr WHERE Name='' AND FK_MapData='' ";
            // DataTable mydt = DBAccess.RunSQLReturnTable(sql);
            // if (mydt.Rows.Count != 0)
            // {
            //     string[] strs = mapAttrKeyName.Split('@');
            //     sqls = "";
            //     foreach (DataRow dr in mydt.Rows)
            //     {
            //         string key = dr[0].ToString();
            //         foreach (string mystr in strs)
            //         {
            //             if (mystr.Contains(key + "=") == false)
            //                 continue;

            //             string[] kv = mystr.Split('=');

            //             sqls += "@ UPDATE Sys_MapAttr SET Name='"+kv[2]+"' WHERE FK_MapData='' AND KeyOfEn=''";
            //         }
            //     }
            //     //  Carried out sql.
            //     DBAccess.RunSQLs(sqls);
            // }
            #endregion  Save field names solve problems .


            //// Remove cache .
            //MapData md = new MapData(fk_mapdata);
            //md.DeleteFromCash();

            //  Backup Files 
            CCFlow.WF.Admin.XAP.DoPort.WriteToXmlMapData(fk_mapdata, false);

            if (string.IsNullOrEmpty(str))
                return null;
            return str;
        }

        class TableSQL
        {
            public string PK;
            public string INSERTED;
            public string UPDATED;
        }
        static Dictionary<string, TableSQL> dicTableSql = new Dictionary<string, TableSQL>();
        TableSQL getTableSql(string tableName, DataColumnCollection columns = null)
        {
            TableSQL tableSql = null;
            if (dicTableSql.ContainsKey(tableName))
            {
                tableSql = dicTableSql[tableName];
            }
            else
            {
                if (columns != null && columns.Count > 0)
                {
                    string igF = "@RowIndex@RowState@";

                    #region gener sql.
                    // Generate sqlUpdate
                    string sqlUpdate = "UPDATE " + tableName + " SET ";
                    foreach (DataColumn dc in columns)
                    {
                        if (igF.Contains("@" + dc.ColumnName + "@"))
                            continue;

                        switch (dc.ColumnName)
                        {
                            case "MyPK":
                            case "OID":
                            case "No":
                                continue;
                            default:
                                break;
                        }

                        if (tableName == "Sys_MapAttr" && dc.ColumnName == "UIBindKey")
                            continue;
                        try
                        {
                            sqlUpdate += dc.ColumnName + "=" + BP.Sys.SystemConfig.AppCenterDBVarStr + dc.ColumnName.Trim() + ",";
                        }
                        catch
                        {
                        }
                    }
                    sqlUpdate = sqlUpdate.Substring(0, sqlUpdate.Length - 1);
                    string pk = "";
                    if (columns.Contains("NodeID"))
                        pk = "NodeID";
                    if (columns.Contains("MyPK"))
                        pk = "MyPK";
                    if (columns.Contains("OID"))
                        pk = "OID";
                    if (columns.Contains("No"))
                        pk = "No";
                    if (columns.Contains("NodeID"))
                        pk = "NodeID";

                    sqlUpdate += " WHERE " + pk + "=" + BP.Sys.SystemConfig.AppCenterDBVarStr + pk;

                    // Generate sqlInsert
                    string sqlInsert = "INSERT INTO " + tableName + " ( ";
                    foreach (DataColumn dc in columns)
                    {
                        if (igF.Contains("@" + dc.ColumnName.Trim() + "@"))
                            continue;

                        if (tableName == "Sys_MapAttr" && dc.ColumnName.Trim() == "UIBindKey")
                            continue;

                        sqlInsert += dc.ColumnName.Trim() + ",";
                    }
                    sqlInsert = sqlInsert.Substring(0, sqlInsert.Length - 1);
                    sqlInsert += ") VALUES (";
                    foreach (DataColumn dc in columns)
                    {
                        if (igF.Contains("@" + dc.ColumnName + "@"))
                            continue;

                        if (tableName == "Sys_MapAttr" && dc.ColumnName.Trim() == "UIBindKey")
                            continue;

                        sqlInsert += BP.Sys.SystemConfig.AppCenterDBVarStr + dc.ColumnName.Trim() + ",";
                    }
                    sqlInsert = sqlInsert.Substring(0, sqlInsert.Length - 1);
                    sqlInsert += ")";
                    #endregion gener sql.

                    tableSql = new TableSQL();
                    tableSql.UPDATED = sqlUpdate;
                    tableSql.INSERTED = sqlInsert;
                    tableSql.PK = pk;

                    dicTableSql.Add(tableName, tableSql);
                }
            }

            return tableSql;
        }
        public string SaveDT(DataTable dt)
        {
            if (dt.Rows.Count == 0)
                return "";

            string tableName = dt.TableName.Replace("CopyOf", "");
            if (tableName == "Sys_MapData")
            {
                int i = 0;
            }

            if (dt.Columns.Count <= 0)
                return null;

            TableSQL sqls = getTableSql(tableName, dt.Columns);
            string updataSQL = sqls.UPDATED;
            string pk = sqls.PK;
            string insertSQL = sqls.INSERTED;

            #region save data.
            foreach (DataRow dr in dt.Rows)
            {
                BP.DA.Paras ps = new BP.DA.Paras();
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.ColumnName == pk)
                        continue;

                    if (tableName == "Sys_MapAttr" && dc.ColumnName.Trim() == "UIBindKey")
                        continue;

                    if (updataSQL.Contains(BP.Sys.SystemConfig.AppCenterDBVarStr + dc.ColumnName.Trim()))
                        ps.Add(dc.ColumnName.Trim(), dr[dc.ColumnName.Trim()]);
                }

                ps.Add(pk, dr[pk]);
                ps.SQL = updataSQL;
                try
                {
                    if (BP.DA.DBAccess.RunSQL(ps) == 0)
                    {
                        ps.Clear();
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (tableName == "Sys_MapAttr" && dc.ColumnName == "UIBindKey")
                                continue;

                            if (updataSQL.Contains(BP.Sys.SystemConfig.AppCenterDBVarStr + dc.ColumnName.Trim()))
                                ps.Add(dc.ColumnName.Trim(), dr[dc.ColumnName.Trim()]);
                        }
                        ps.SQL = insertSQL;
                        BP.DA.DBAccess.RunSQL(ps);
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    string pastrs = "";
                    foreach (Para p in ps)
                    {
                        pastrs += "\t\n@" + p.ParaName + "=" + p.val;
                    }
                    throw new Exception("@ Carried out sql=" + ps.SQL + " Failure ." + ex.Message + "\t\n@paras=" + pastrs);
                }
            }
            #endregion
            return null;
        }

    }
}
