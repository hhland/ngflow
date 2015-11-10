using System;
using System.Collections.Generic;
using System.Web;
using BP.WF;
using BP.Sys;
using BP.DA;
using System.IO;
using System.Data;
using System.Drawing.Imaging;
using System.Drawing;
using BP.Tools;

namespace BP.Web
{
    public class Common
    {
        class TableSQL
        {
            public string PK;
            public string INSERTED;
            public string UPDATED ;
            //public string DELETED ;
        }
        static Dictionary<string, TableSQL> dicTableSql = new Dictionary<string, TableSQL>();
        static TableSQL getTableSql(string tableName, DataColumnCollection columns = null)
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

        #region Form
        public static  string SaveFrm_Pri(string fk_mapdata, string xml, string sqls)
        {
            string str = "";
            DataSet ds = new DataSet();
            //ds.ReadXml(sr);
            ds = FormatToJson.JsonToDataSet(xml);
           
        
            foreach (DataTable dt in ds.Tables)
            {
                try
                {
                    str += SaveDT(dt);
                    if (dt.TableName.ToLower() == "wf_node")
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

            #region  Handle database compatibility issues 
            if (BP.Sys.SystemConfig.AppCenterDBType == DBType.Oracle)
            {
                sqls = sqls.Replace("LEN(", "LENGTH(");
            }

            sqls += "@UPDATE Sys_MapAttr SET Name='' WHERE FK_MapData='" + fk_mapdata + "'  AND Name IS NULL ";
            sqls += "@UPDATE Sys_MapAttr SET UIVisible=1 WHERE FK_MapData='" + fk_mapdata + "' AND UIVisible is null";
            sqls += "@UPDATE Sys_MapAttr SET UIIsEnable=1 WHERE FK_MapData='" + fk_mapdata + "' AND UIIsEnable is null";
            sqls += "@UPDATE Sys_MapAttr SET UIIsLine=0 WHERE FK_MapData='" + fk_mapdata + "' AND UIIsLine is null";

            #endregion  Handle database compatibility issues 

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


            // Remove cache .
            MapData md = new MapData(fk_mapdata);
            md.DeleteFromCash();
            md.UpdateVer();//  Updated version number , Execution sql, It is not necessary Retrieve

            //  Backup Files 
            WriteToXmlMapData(fk_mapdata, false);
            
            if (string.IsNullOrEmpty(str))
                return null;
            return str;
        }

        /// <summary>
        ///  Back up the current process to the user xml File 
        ///  When you save each user calls 
        /// </summary>
        public static void WriteToXmlMapData(string FK_MapData,bool savePrtSC)
        {
            string path = string.Empty, xmlName = string.Empty, ext = ".xml";
            if (!string.IsNullOrEmpty(FK_MapData))
            {
                if (FK_MapData.StartsWith("ND"))
                {//  Node Form 
                    int nodeNo = int.Parse(FK_MapData.Substring(2, FK_MapData.Length - 2));

                    string nodeName = "", FlowNo = "",FlowName ="";
                    string sql = "SELECT n.Name NodeName,FK_Flow FlowNo,f.Name FlowName FROM WF_Node n,WF_Flow f where NodeID ='{0}' and n.FK_Flow = f.No";
                    sql = string.Format(sql, nodeNo);
                    DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
                    if (dt != null && dt.Rows.Count == 1)
                    {
                        nodeName = dt.Rows[0]["NodeName"].ToString();
                        FlowNo = dt.Rows[0]["FlowNo"].ToString();
                        FlowName = dt.Rows[0]["FlowName"].ToString();
                    }

                    path = BP.Sys.SystemConfig.PathOfDataUser + @"FlowDesc\"
                        + FlowNo + "." + FlowName + "\\";
                    xmlName = nodeNo + "." + nodeName;
                }
                else
                { //  Process Form 
                    string sql = "SELECT Name from sys_MapData WHERE No ='{0}'";
                    sql = string.Format(sql, FK_MapData);
                    xmlName = BP.DA.DBAccess.RunSQLReturnString(sql);

                    path = BP.Sys.SystemConfig.PathOfDataUser + @"FlowDesc\FlowForm\"+xmlName + "\\";//  Process Form 
                }
            }

            xmlName = BP.Tools.StringExpressionCalculate.ReplaceBadCharOfFileName(xmlName);

            if (!string.IsNullOrEmpty(path))
            {
                string file = path + xmlName + ext;
              
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                else if (System.IO.File.Exists(file))
                {
                    DateTime time = System.IO.File.GetLastWriteTime(file);
                    string xmlNameOld = path + xmlName + time.ToString("@yyyyMMddHHmmss") + ext;

                    // Rename the file .
                    if (File.Exists(xmlNameOld))
                        File.Delete(xmlNameOld);
                    System.IO.File.Move(file,xmlNameOld);
                   
                }

                if (savePrtSC)
                {
                    file = path + xmlName + ".png";

                    uploadPng(file);
                }
                else
                    try
                    {
                        DataSet ds = Sys.MapData.GenerHisDataSet(FK_MapData);
                        if (!string.IsNullOrEmpty(file) && null != ds)
                            ds.WriteXml(file);
                    }
                    catch (Exception e) { }
            }
           
           
        }

        public static DataSet TurnXmlDataSet2SLDataSet(DataSet ds)
        {
            DataSet myds = new DataSet();
            foreach (DataTable dtXml in ds.Tables)
            {
                DataTable dt = new DataTable(dtXml.TableName);
                foreach (DataColumn dc in dtXml.Columns)
                {
                    DataColumn mydc = new DataColumn(dc.ColumnName, typeof(string));
                    dt.Columns.Add(mydc);
                }
                foreach (DataRow dr in dtXml.Rows)
                {
                    DataRow drNew = dt.NewRow();
                    foreach (DataColumn dc in dtXml.Columns)
                    {
                        drNew[dc.ColumnName] = dr[dc.ColumnName];
                    }
                    dt.Rows.Add(drNew);
                }
                myds.Tables.Add(dt);
            }
            return myds;
        }
        public  static int RunSQLs(string sqls)
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

        public static string SaveDT(DataTable dt)
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
        #endregion

        #region Flow

        public const string FlowTemplate = "FlowTemplate";
        public static void ShareToFtp()
        {
            string flowNo = HttpContext.Current.Request.QueryString["FK_Flow"];
            string bbsNo = HttpContext.Current.Request.QueryString["BBS"];
            string sharTo = HttpContext.Current.Request.QueryString["ShareTo"];
            Flow flow = new Flow(flowNo);
            string ip = "online.ccflow.org";
            string userNo = "ccflowlover";
            string pass = "ccflowlover";
            try
            {
                FtpSupport.FtpConnection conn = new FtpSupport.FtpConnection(ip, userNo, pass);
                //conn.SetCurrentDirectory("/");
                conn.SetCurrentDirectory(FlowTemplate + "\\" + sharTo + "\\");

                string createDir = bbsNo + "." + flow.No + "." + flow.Name;
                if (!conn.DirectoryExist(createDir))
                {
                    conn.CreateDirectory(createDir);
                }
                conn.SetCurrentDirectory(createDir + "\\");

                HttpContext.Current.Response.ContentType = "text/plain";

                string dir = BP.Sys.SystemConfig.PathOfDataUser + @"\FlowDesc\" + flow.No + "." + flow.Name + "\\";
                if (System.IO.Directory.Exists(dir))
                {

                    string[] fls = System.IO.Directory.GetFiles(dir);
                    foreach (string fff in fls)
                    {
                        string fileName = fff.Substring(fff.LastIndexOf("\\") + 1);
                        if (fileName.Contains("@"))//  Historical data is not uploaded 
                            continue;

                        conn.PutFile(fff, fileName);
                        //conn.DeleteFile(fileName);
                    }

                    // Uploaded successfully 
                    HttpContext.Current.Response.Write(" Uploaded successfully ");
                }
                else
                {
                    HttpContext.Current.Response.Write(" This process temporarily no publishable document ");
                }
                conn.Close();
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write(" Publish failed process templates :" + e.Message);
            }
        }

        public static void upload()
        {
            string path = string.Empty, pngName = string.Empty;


            string FK_Flow = HttpContext.Current.Request["FK_Flow"];
            string FK_MapData = HttpContext.Current.Request["FK_MapData"];
            if (!string.IsNullOrEmpty(FK_Flow))
            {//  Shared processes 
                Flow flow = new Flow(FK_Flow);

                path = BP.Sys.SystemConfig.PathOfDataUser + @"\FlowDesc\" + flow.No + "." + flow.Name + "\\";
                pngName = path + "Flow.png";

                if (!string.IsNullOrEmpty(path) && !System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                if (!string.IsNullOrEmpty(pngName))
                    Common.uploadPng(pngName);
            }
            else if (!string.IsNullOrEmpty(FK_MapData))
            {//  Sharing Form 
                Common.WriteToXmlMapData(FK_MapData, true);
            }
        }
        
        /// <summary>
        ///  Image compression save 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool uploadPng(string fileName)
        {
            // Get the uploaded data stream 
            ImageCodecInfo ici;
            Bitmap bmp = null;
            Encoder ecd = null;
            EncoderParameter ept = null;
            EncoderParameters eptS = null;
            try
            {
                ici = getImageCoderInfo(@"image/png");
                ecd = System.Drawing.Imaging.Encoder.Quality;
                eptS = new EncoderParameters(1);
                ept = new EncoderParameter(ecd, 10L);
                eptS.Param[0] = ept;
                bmp = new Bitmap(HttpContext.Current.Request.InputStream);
                bmp.Save(fileName, ici, eptS);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {

            }
        }
        //  Process installed webservice Super time , Needs to be done here , Write a completed call 
        public static  void InstallFlowTemplete(string fk_flowSort, string path)
        {
            try
            {

                var result = Flow.DoLoadFlowTemplate(fk_flowSort, path, ImpFlowTempleteModel.AsNewFlow);

                // The implementation of some repair view.
                Flow.RepareV_FlowData_View();

                string tmp = string.Format("{0},{1},{2}", fk_flowSort, result.No, result.Name);
                // Uploaded successfully 
                HttpContext.Current.Response.ContentType = "text/plain";
                HttpContext.Current.Response.Write(result.Name + " Successful installation ");
            }
            catch (Exception ex)
            {
                // Uploaded successfully 
                HttpContext.Current.Response.ContentType = "text/plain";
                HttpContext.Current.Response.Write(" Process installation errors :" + ex.Message);
            }
        }

        #endregion
       
        static ImageCodecInfo getImageCoderInfo(string coderType)//  Get the picture coding type information 
        {
            ImageCodecInfo[] iciS = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo retIci = null;
            foreach (ImageCodecInfo ici in iciS)
            {
                if (ici.MimeType.Equals(coderType))
                    retIci = ici;
            }
            return retIci;
        }

    }
}