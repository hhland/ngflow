#region Copyright
//------------------------------------------------------------------------------
// <copyright file="ConfigReaders.cs" company="BP">
//     
//      Copyright (c) 2002 Microsoft Corporation.  All rights reserved.
//     
//      BP ZHZS Team
//      Purpose: config system: finds config files, loads config factories,
//               filters out relevant config file sections
//      Date: Oct 14, 2003
//      Author: peng zhou (pengzhoucn@hotmail.com) 
//      http://www.CCPortal.com.cn
//
// </copyright>                                                                
//------------------------------------------------------------------------------
#endregion

using System;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using System.Data;
using System.IO;

namespace CCPortal
{
    /// <summary>
    /// 系统配值
    /// </summary>
    public class SystemConfig
    {
        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="cfgFile"></param>
        public static void ReadConfigFile(string cfgFile)
        {
            try
            {
                CCPortal.DA.DBAccess.HisConnOfOLEs.Clear();
            }
            catch
            {
            }
            try
            {
                CCPortal.DA.DBAccess.HisConnOfOras.Clear();
            }
            catch
            {
            }
            try
            {
                CCPortal.DA.DBAccess.HisConnOfSQLs.Clear();
            }
            catch
            {
            }
            try
            {

               // CCPortal.DA.ClassFactory._BPAssemblies = null;
            }
            catch
            {
            }
            try
            {
             //   CCPortal.DA.ClassFactory.Htable_Ens.Clear();
            }
            catch
            {
            }

            try
            {
              //  CCPortal.DA.ClassFactory.Htable_XmlEn.Clear();
            }
            catch
            {
            }

            try
            {
               // CCPortal.DA.ClassFactory.Htable_XmlEns.Clear();
            }
            catch
            {
            }
            try
            {
                CCPortal.SystemConfig.CS_AppSettings.Clear();
            }
            catch
            {
            }

            #region 加载 Web.Config 文件配置

            if (File.Exists(cfgFile) == false)
                throw new Exception("文件不存在 [" + cfgFile + "]");


            string _RefConfigPath = cfgFile;
            StreamReader read = new StreamReader(cfgFile);
            string firstline = read.ReadLine();
            string cfg = read.ReadToEnd();
            read.Close();

            int start = cfg.ToLower().IndexOf("<appsettings>");
            int end = cfg.ToLower().IndexOf("</appsettings>");

            cfg = cfg.Substring(start, end - start + "</appsettings".Length + 1);

            string tempFile = "Web.config.xml";

            StreamWriter write = new StreamWriter(tempFile);
            write.WriteLine(firstline);
            write.Write(cfg);
            write.Flush();
            write.Close();

            DataSet dscfg = new DataSet("cfg");
            dscfg.ReadXml(tempFile);

            //    CCPortal.SystemConfig.CS_AppSettings = new System.Collections.Specialized.NameValueCollection();
            CCPortal.SystemConfig.CS_DBConnctionDic.Clear();
            foreach (DataRow row in dscfg.Tables["add"].Rows)
            {
                CCPortal.SystemConfig.CS_AppSettings.Add(row["key"].ToString().Trim(), row["value"].ToString().Trim());
            }
            #endregion
        }

        #region 关于开发商的信息
        public static string Ver
        {
            get
            {
                try
                {
                    return AppSettings["Ver"];
                }
                catch
                {
                    return "1.0.0";
                }
            }
        }
        public static string TouchWay
        {
            get
            {
                try
                {
                    return AppSettings["TouchWay"];
                }
                catch
                {
                    return SystemConfig.CustomerTel + " 地址:" + SystemConfig.CustomerAddr;
                }
            }
        }
        public static string CopyRight
        {
            get
            {
                try
                {
                    return AppSettings["CopyRight"];
                }
                catch
                {
                    return "版权所有@" + CustomerName;
                }
            }
        }
        public static string CompanyID
        {
            get
            {
                string s = AppSettings["CompanyID"];
                if (string.IsNullOrEmpty(s))
                    return "CCFlow";
                return s;
            }
        }
        /// <summary>
        /// 开发商全称		 
        /// </summary>
        public static string DeveloperName
        {
            get { return AppSettings["DeveloperName"]; }
        }
        /// <summary>
        /// 开发商简称
        /// </summary>
        public static string DeveloperShortName
        {
            get { return AppSettings["DeveloperShortName"]; }
        }
        /// <summary>		 
        /// 开发商电话。
        /// </summary>
        public static string DeveloperTel
        {
            get { return AppSettings["DeveloperTel"]; }
        }
        /// <summary>		
        /// 开发商的地址。
        /// </summary>
        public static string DeveloperAddr
        {
            get { return AppSettings["DeveloperAddr"]; }
        }
        #endregion

        #region 用户配置信息
        /// <summary>
        /// 系统语言（）
        /// 对多语言的系统有效。
        /// </summary>
        public static string SysLanguage
        {
            get
            {
                string s = AppSettings["SysLanguage"];
                if (s == null)
                    s = "CH";
                return s;
            }
        }
        #endregion

        #region 逻辑处理
        /// <summary>
        /// 封装了AppSettings
        /// </summary>		
        private static NameValueCollection _CS_AppSettings;
        public static NameValueCollection CS_AppSettings
        {
            get
            {
                if (_CS_AppSettings == null)
                    _CS_AppSettings = new NameValueCollection();
                return _CS_AppSettings;
            }
            set
            {
                _CS_AppSettings = value;
            }
        }
        /// <summary>
        /// 封装了AppSettings
        /// </summary>
        public static NameValueCollection AppSettings
        {
            get
            {
                if (SystemConfig.IsBSsystem)
                {
                    return System.Configuration.ConfigurationManager.AppSettings;

                }
                else
                {
                    return CS_AppSettings;
                }
            }
        }
        static SystemConfig()
        {
            CS_DBConnctionDic = new Hashtable();
        }
        /// <summary>
        /// 应用程序路径
        /// </summary>
        public static string PhysicalApplicationPath
        {
            get
            {
                if (SystemConfig.IsBSsystem && HttpContext.Current != null)
                    return HttpContext.Current.Request.PhysicalApplicationPath;
                else
                    return AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            }
        }
        /// <summary>
        /// 文件放置的路径
        /// </summary>
        public static string PathOfUsersFiles
        {
            get
            {
                return "/Data/Files/";
            }
        }
        /// <summary>
        /// 临时文件路径
        /// </summary>
        public static string PathOfTemp
        {
            get
            {
                return PathOfDataUser + "\\Temp\\";
            }
        }
        public static string PathOfWorkDir
        {
            get
            {
                if (CCPortal.SystemConfig.IsBSsystem)
                {
                    string path1 = HttpContext.Current.Request.PhysicalApplicationPath + "\\..\\";
                    System.IO.DirectoryInfo info1 = new DirectoryInfo(path1);
                    return info1.FullName;
                }
                else
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "\\..\\..\\..\\";
                    System.IO.DirectoryInfo info = new DirectoryInfo(path);
                    return info.FullName;
                }
            }
        }
        public static string PathOfFDB
        {
            get
            {
                string s = SystemConfig.AppSettings["FDB"];
                if (s == "" || s == null)
                    return PathOfWebApp + "\\DataUser\\FDB\\";
                return s;
            }
        }
        /// <summary>
        /// 数据文件
        /// </summary>
        public static string PathOfData
        {
            get
            {
                return PathOfWebApp + SystemConfig.AppSettings["DataDirPath"] + "\\Data\\";
            }
        }
        public static string PathOfDataUser
        {
            get
            {
                return PathOfWebApp + "\\DataUser\\";
            }
        }
        /// <summary>
        /// XmlFilePath
        /// </summary>
        public static string PathOfXML
        {
            get
            {
                return PathOfWebApp + SystemConfig.AppSettings["DataDirPath"] + "\\Data\\XML\\";
            }
        }
        public static string PathOfAppUpdate
        {
            get
            {
                return PathOfWebApp + SystemConfig.AppSettings["DataDirPath"] + "\\Data\\AppUpdate\\";
            }
        }
        public static string PathOfCyclostyleFile
        {
            get
            {
                return PathOfWebApp + "\\DataUser\\CyclostyleFile\\";
            }
        }
        /// <summary>
        /// 应用程序名称
        /// </summary>
        public static string AppName
        {
            get
            {
                return System.Web.HttpContext.Current.Request.ApplicationPath.Replace("/", "");
            }
        }
        /// <summary>
        /// WebApp Path.
        /// </summary>
        public static string PathOfWebApp
        {
            get
            {
                if (SystemConfig.IsBSsystem)
                {
                    return System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                }
                else
                {
                    if (SystemConfig.SysNo == "FTA")
                        return AppDomain.CurrentDomain.BaseDirectory;
                    else
                        return AppDomain.CurrentDomain.BaseDirectory + "..\\..\\";
                }
            }
        }
        #endregion

        #region 共同变量。
        public static bool IsBSsystem_Test = true;
        /// <summary>
        /// 是不是BS系统结构。
        /// </summary>
        private static bool _IsBSsystem = true;
        /// <summary>
        /// 是不是BS系统结构。
        /// </summary>
        public static bool IsBSsystem
        {
            get
            {
                // return true;
                return SystemConfig._IsBSsystem;
            }
            set
            {
                SystemConfig._IsBSsystem = value;
            }
        }
        public static bool IsCSsystem
        {
            get
            {
                return !SystemConfig._IsBSsystem;
            }
        }
        #endregion

        #region 系统配置信息
        /// <summary>
        /// 执行清空
        /// </summary>
        public static void DoClearCash_del()
        {
           // HttpRuntime.UnloadAppDomain();
            //CCPortal.DA.Cash.Map_Cash.Clear();
            //CCPortal.DA.Cash.SQL_Cash.Clear();
            //CCPortal.DA.Cash.EnsData_Cash.Clear();
            //CCPortal.DA.Cash.EnsData_Cash_Ext.Clear();
            //CCPortal.DA.Cash.BS_Cash.Clear();
            //CCPortal.DA.Cash.Bill_Cash.Clear();
            try
            {
             //   System.Web.HttpContext.Current.Session.Clear();
               // System.Web.HttpContext.Current.Application.Clear();
            }
            catch
            {
            }
        }
        /// <summary>
        /// 系统编号
        /// </summary>		 
        public static string SysNo
        {
            get { return AppSettings["SysNo"]; }
        }

        /// <summary>
        /// 系统名称
        /// </summary>
        public static string SysName
        {
            get
            {
                string s = AppSettings["SysName"];
                if (s == null)
                    s = "请在web.config中配置SysName名称。";
                return s;
            }
        }
        public static string OrderWay
        {
            get
            {
                return AppSettings["OrderWay"];
            }
        }
        public static int PageSize
        {
            get
            {
                try
                {
                    return int.Parse(AppSettings["PageSize"]);
                }
                catch
                {
                    return 99999;
                }
            }
        }
        public static int MaxDDLNum
        {
            get
            {
                try
                {
                    return int.Parse(AppSettings["MaxDDLNum"]);
                }
                catch
                {
                    return 50;
                }
            }
        }
        public static int PageSpan
        {
            get
            {
                try
                {
                    return int.Parse(AppSettings["PageSpan"]);
                }
                catch
                {
                    return 20;
                }
            }
        }
        /// <summary>
        ///  到的路径.PageOfAfterAuthorizeLogin
        /// </summary>
        public static string PageOfAfterAuthorizeLogin
        {
            get { return System.Web.HttpContext.Current.Request.ApplicationPath + "" + AppSettings["PageOfAfterAuthorizeLogin"]; }
        }
        /// <summary>
        /// 丢失session 到的路径.
        /// </summary>
        public static string PageOfLostSession
        {
            get { return System.Web.HttpContext.Current.Request.ApplicationPath + "" + AppSettings["PageOfLostSession"]; }
        }
        /// <summary>
        /// 日志路径
        /// </summary>
        public static string PathOfLog
        {
            get { return PathOfWebApp + "\\DataUser\\Log\\"; }
        }

        /// <summary>
        /// 系统名称
        /// </summary>
        public static int TopNum
        {
            get
            {
                try
                {
                    return int.Parse(AppSettings["TopNum"]);
                }
                catch
                {
                    return 99999;
                }
            }
        }
        /// <summary>
        /// 服务电话
        /// </summary>
        public static string ServiceTel
        {
            get { return AppSettings["ServiceTel"]; }
        }
        /// <summary>
        /// 服务E-mail
        /// </summary>
        public static string ServiceMail
        {
            get { return AppSettings["ServiceMail"]; }
        }
        /// <summary>
        /// 第3方软件
        /// </summary>
        public static string ThirdPartySoftWareKey
        {
            get
            {
                return AppSettings["ThirdPartySoftWareKey"];
            }
        }
        /// <summary>
        /// 是否 debug 状态
        /// </summary>
        public static bool IsDebug
        {
            get
            {
                if (AppSettings["IsDebug"] == "1")
                    return true;
                else
                    return false;
            }
        }
        public static bool IsUnit
        {
            get
            {
                if (AppSettings["IsUnit"] == "1")
                    return true;
                else
                    return false;
            }
        }

        public static bool IsOpenSQLCheck
        {
            get
            {
                if (AppSettings["IsOpenSQLCheck"] == "0")
                    return false;
                else
                    return true;
            }
        }
        /// <summary>
        /// 是不是多系统工作。
        /// </summary>
        public static bool IsMultiSys
        {
            get
            {
                if (AppSettings["IsMultiSys"] == "1")
                    return true;
                else
                    return false;
            }
        }
        /// <summary>
        /// 是不是多线程工作。
        /// </summary>
        public static bool IsMultiThread_del
        {
            get
            {
                if (AppSettings["IsMultiThread"] == "1")
                    return true;
                else
                    return false;
            }
        }
        /// <summary>
        /// 是不是多语言版本
        /// </summary>
        public static bool IsMultiLanguageSys
        {
            get
            {
                if (AppSettings["IsMultiLanguageSys"] == "1")
                    return true;
                else
                    return false;
            }
        }
        #endregion

        #region 处理临时缓存
        /// <summary>
        /// 放在 Temp 中的cash 多少时间失效。
        /// 0, 表示永久不失效。
        /// </summary>
        private static int CashFail
        {
            get
            {
                try
                {
                    return int.Parse(AppSettings["CashFail"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 当前的 TempCash 是否失效了
        /// </summary>
        public static bool IsTempCashFail
        {
            get
            {
                if (SystemConfig.CashFail == 0)
                    return false;

                if (_CashFailDateTime == null)
                {
                    _CashFailDateTime = DateTime.Now;
                    return true;
                }
                else
                {
                    TimeSpan ts = DateTime.Now - _CashFailDateTime;
                    if (ts.Minutes >= SystemConfig.CashFail)
                    {
                        _CashFailDateTime = DateTime.Now;
                        return true;
                    }
                    return false;
                }
            }
        }
        public static DateTime _CashFailDateTime;
        #endregion

        #region 客户配置信息
        /// <summary>
        /// 客户编号
        /// </summary>
        public static string CustomerNo
        {
            get
            {
                return AppSettings["CustomerNo"];
            }
        }
        /// <summary>
        /// 客户名称
        /// </summary>
        public static string CustomerName
        {
            get
            {
                return AppSettings["CustomerName"];
            }
        }
        public static string CustomerURL
        {
            get
            {
                return AppSettings["CustomerURL"];
            }
        }
        /// <summary>
        /// 客户简称
        /// </summary>
        public static string CustomerShortName
        {
            get
            {
                return AppSettings["CustomerShortName"];
            }
        }
        /// <summary>
        /// 客户地址
        /// </summary>
        public static string CustomerAddr
        {
            get
            {
                return AppSettings["CustomerAddr"];
            }
        }
        /// <summary>
        /// 客户电话
        /// </summary>
        public static string CustomerTel
        {
            get
            {
                return AppSettings["CustomerTel"];
            }
        }
        #endregion

        /// <summary>
        ///取得配置 NestedNamesSection 内的相应 key 的内容
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static NameValueCollection GetConfig(string key)
        {
            Hashtable ht = (Hashtable)System.Configuration.ConfigurationManager.GetSection("NestedNamesSection");
            return (NameValueCollection)ht[key];
        }
        public static string GetValByKey(string key, string isNullas)
        {
            string s = AppSettings[key];
            if (s == null)
                s = isNullas;
            return s;
        }
        public static bool GetValByKeyBoolen(string key, bool isNullas)
        {
            string s = AppSettings[key];
            if (s == null)
                return isNullas;

            if (s == "1")
                return true;
            else
                return false;
        }
        public static int GetValByKeyInt(string key, int isNullas)
        {
            string s = AppSettings[key];
            if (s == null)
                return isNullas;

            return int.Parse(s);
        }
        public static string GetConfigXmlKeyVal(string key)
        {
            try
            {
                DataSet ds = new DataSet("dss");
                ds.ReadXml(CCPortal.SystemConfig.PathOfXML + "\\KeyVal.xml");
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["Key"].ToString() == key)
                        return dr["Val"].ToString();
                }
                throw new Exception("在您利用GetXmlConfig 取值出现错误，没有找到key= " + key + ", 请检查 /data/Xml/KeyVal.xml. ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string GetConfigXmlNode(string fk_Breed, string enName, string key)
        {
            try
            {
                string file = CCPortal.SystemConfig.PathOfXML + "\\Node\\" + fk_Breed + ".xml";
                DataSet ds = new DataSet("dss");
                try
                {
                    ds.ReadXml(file);
                }
                catch
                {
                    return null;
                }
                DataTable dt = ds.Tables[0];
                if (dt.Columns.Contains(key) == false)
                    throw new Exception(file + "配置错误，您没有按照格式配置，它不包含标记 " + key);
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["NodeEnName"].ToString() == enName)
                    {
                        if (dr[key].Equals(DBNull.Value))
                            return null;
                        else
                            return dr[key].ToString();

                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
           
        #region dsn
        public static string AppCenterDSN
        {
            get
            {
                string str = AppSettings["CCPortal.DSN"];
                if (string.IsNullOrEmpty(str))
                    str = AppSettings["AppCenterDSN"];
                return str;
            }
        }
        #endregion

        /// <summary>
        /// 获取主应用程序的数据库类型．
        /// </summary>
        public static string AppNo
        {
            get
            {
                string appNo = AppSettings["CCPortal.AppNo"];
                if (string.IsNullOrEmpty(appNo))
                    appNo = AppSettings["SysNo"];
                return appNo;
            }
        }
        /// <summary>
        /// 获取主应用程序的数据库类型．
        /// </summary>
        public static CCPortal.DA.DBType AppCenterDBType
        {
            get
            {
                string dbtype=AppSettings["CCPortal.DBType"];
                if (string.IsNullOrEmpty(dbtype))
                    dbtype = AppSettings["AppCenterDBType"];

                switch (dbtype)
                {
                    case "MSMSSQL":
                    case "MSSQL":
                        return CCPortal.DA.DBType.MSSQL;
                    case "Oracle":
                        return CCPortal.DA.DBType.Oracle;
                    case "MySQL":
                        return CCPortal.DA.DBType.MySQL;
                    case "Access":
                        return CCPortal.DA.DBType.Access;
                    case "Informix":
                        return CCPortal.DA.DBType.Informix;
                    default:
                        return CCPortal.DA.DBType.Oracle;
                }
            }
        }
        /// <summary>
        /// 获取不同类型的数据库变量标记
        /// </summary>
        public static string AppCenterDBVarStr
        {
            get
            {
                switch (SystemConfig.AppCenterDBType)
                {
                    case CCPortal.DA.DBType.Oracle:
                        return ":";
                    case CCPortal.DA.DBType.Informix:
                        return "?";
                    default:
                        return "@";
                }
            }
        }

        public static string AppCenterDBLengthStr
        {
            get
            {
                switch (SystemConfig.AppCenterDBType)
                {
                    case CCPortal.DA.DBType.Oracle:
                        return "Length";
                    case CCPortal.DA.DBType.MSSQL:
                        return "LEN";
                    case CCPortal.DA.DBType.Informix:
                        return "Length";
                    case CCPortal.DA.DBType.Access:
                        return "Length";
                    default:
                        return "Length";
                }
            }
        }
        /// <summary>
        /// 获取不同类型的substring函数的书写
        /// </summary>
        public static string AppCenterDBSubstringStr
        {
            get
            {
                switch (SystemConfig.AppCenterDBType)
                {
                    case CCPortal.DA.DBType.Oracle:
                        return "substr";
                    case CCPortal.DA.DBType.MSSQL:
                        return "substring";
                    case CCPortal.DA.DBType.Informix:
                        return "MySubString";
                    case CCPortal.DA.DBType.Access:
                        return "Mid";
                    default:
                        return "substring";
                }
            }
        }
        public static string AppCenterDBAddStringStr
        {
            get
            {
                switch (SystemConfig.AppCenterDBType)
                {
                    case CCPortal.DA.DBType.Oracle:
                    case CCPortal.DA.DBType.MySQL:
                    case CCPortal.DA.DBType.Informix:
                        return "||";
                    default:
                        return "+";
                }
            }
        }
        public static readonly Hashtable CS_DBConnctionDic;
    }
}
