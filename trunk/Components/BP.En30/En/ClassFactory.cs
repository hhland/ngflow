
using System;
using System.Collections;
using System.ComponentModel;
using System.Data; 
using System.Web;
using System.Reflection;
using System.IO;
using BP.DA;
using BP.En;   
using BP.Sys;
using BP.Pub;

 
namespace BP.En
{ 
	/// <summary>
	/// ClassFactory  The summary .
	/// </summary>
    public class ClassFactory 
    {
        #region public moen
        /// <summary>
        ///  Load xml Profiles 
        /// </summary>
        /// <param name="path"> File Path </param>
        /// <param name="tableName"> Physical table name </param>
        /// <param name="key">key</param>
        /// <param name="val">值</param>
        /// <returns></returns>
        public static bool LoadConfigXml(string path, string tableName, string key, string val)
        {
            try
            {
                BP.Sys.SystemConfig.CS_AppSettings.Clear();
            }
            catch
            {
            }

            DataSet ds = new DataSet();
            ds.ReadXml(path);

            DataTable dt = ds.Tables[tableName];
          //  BP.Sys.SystemConfig.CS_AppSettings = new System.Collections.Specialized.NameValueCollection();
            BP.Sys.SystemConfig.CS_DBConnctionDic.Clear();
            foreach (DataRow row in dt.Rows)
            {
                BP.Sys.SystemConfig.CS_AppSettings.Add(row[key].ToString().Trim(), row[val].ToString().Trim());
            }
            ds.Dispose();
            BP.Sys.SystemConfig.IsBSsystem_Test = false;
            BP.Sys.SystemConfig.IsBSsystem = false;
            return true;
        }
        public static bool LoadConfig_del(string cfgFile)
        {
            try
            {
                BP.Sys.SystemConfig.CS_AppSettings.Clear();
            }
            catch
            {
            }
            #region  Load  Web.Config  File Configuration 

            if (!File.Exists(cfgFile))
                throw new Exception(" Can not find the configuration file [" + cfgFile + "]2");

            DataSet ds = new DataSet();
            ds.ReadXml(cfgFile);
            DataTable dt = ds.Tables["add"];
            foreach (DataRow dr in dt.Rows)
            {
                string key = dr["key"] as string;
                if (key == null || key == "")
                    continue;

                string value = dr["value"] as string;
                if (value == null || value == "")
                    continue;

                BP.Sys.SystemConfig.CS_AppSettings.Add(key, value);
            }
            //  Add special judge .
            SystemConfig.AppCenterDSN = SystemConfig.AppCenterDSN.Replace("VisualFlowDesigner", "VisualFlow");
            #endregion
            return true;
        }
        /// <summary>
        ///  Load web.config Profiles 
        /// </summary>
        /// <param name="cfgFile">web.config Configuration file path </param>
        /// <returns>true or false</returns>
        public static bool LoadConfig(string cfgFile)
        {
            try
            {
                BP.Sys.SystemConfig.CS_AppSettings.Clear();
            }
            catch
            {
            }

            SystemConfig.IsBSsystem = false;

            #region  Load  Web.Config  File Configuration 
            if (!File.Exists(cfgFile))
                throw new Exception(" Can not find the configuration file [" + cfgFile + "]2");

            DataSet ds = new DataSet();
            ds.ReadXml(cfgFile);

            StreamReader read = new StreamReader(cfgFile);
            string firstline = read.ReadLine();
            string cfg = read.ReadToEnd();
            read.Close();

            int start = cfg.ToLower().IndexOf("<appsettings>");
            int end = cfg.ToLower().IndexOf("</appsettings>");

            cfg = cfg.Substring(start, end - start + "</appsettings".Length + 1);

            cfgFile = "__$AppConfig.cfg";
            StreamWriter write = new StreamWriter(cfgFile);
            write.WriteLine(firstline);
            write.Write(cfg);
            write.Flush();
            write.Close();

            DataSet dscfg = new DataSet("cfg");
            try
            {
                dscfg.ReadXml(cfgFile);
            }
            catch (Exception ex)
            {
                throw new Exception(" Load Profile [" + cfgFile + "] Failure !\n" + ex.Message + " Failed to start !");
            }

            //   BP.Sys.SystemConfig.CS_AppSettings = new System.Collections.Specialized.NameValueCollection();

            BP.Sys.SystemConfig.CS_DBConnctionDic.Clear();
            DataTable dt = dscfg.Tables["add"];
            foreach (DataRow dr in dt.Rows)
            {
                string key = dr["key"] as string;
                if (key == null || key == "")
                    continue;

                string value = dr["value"] as string;
                if (value == null || value == "")
                    continue;

                BP.Sys.SystemConfig.CS_AppSettings.Add(key, value);
            }
            dscfg.Dispose();

            //  Add special judge .
            SystemConfig.AppCenterDSN = SystemConfig.AppCenterDSN.Replace("VisualFlowDesigner", "VisualFlow");
            #endregion
            return true;
        }
        #endregion

        #region  A relationship with the statements 

        /// <summary>
        ///  Set the value of the specified property on an object instance 
        /// </summary>
        /// <param name="obj"> Object instance </param>
        /// <param name="propertyName"> Property name , Property for non-static characteristics </param>
        /// <param name="val">值</param>
        public static void SetValue(object obj, string propertyName, object val)
        {
            Type tp = obj.GetType();
            PropertyInfo p = tp.GetProperty(propertyName);
            if (p == null)
                throw new Exception(" Failed to set property values ! Type [" + tp + "] No property [" + propertyName + "]");
            p.SetValue(obj, val, null);
        }
        /// <summary>
        ///  Gets the value of the specified property on the object instance 
        /// </summary>
        /// <param name="obj"> Object instance </param>
        /// <param name="propertyName"> Property name </param>
        /// <returns>值</returns>
        public static object GetValue(object obj, string propertyName)
        {
            Type tp = obj.GetType();
            PropertyInfo p = tp.GetProperty(propertyName);
            if (p == null)
                throw new Exception(" Failure to obtain the property value ! Type [" + tp + "] No property [" + propertyName + "]");
            object val = p.GetValue(obj, null);
            return val;
        }
        /// <summary>
        ///  Gets the value of the specified property on the object instance , Converted to string
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns>值</returns>
        public static string GetValueToStr(object obj, string propertyName)
        {
            object val = GetValue(obj, propertyName);
            if (val == null)
                return "";
            else
                return val.ToString();
        }
        #endregion

        #region  Constructor ,  Property 
        static ClassFactory()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            if (Directory.Exists(path + "bin\\"))
            {
                if (!string.IsNullOrEmpty(SystemConfig.AppSettings["CCFlowAppPath"]) && Directory.Exists(path + SystemConfig.AppSettings["CCFlowAppPath"] + "bin\\"))
                {
                    _BasePath = path + SystemConfig.AppSettings["CCFlowAppPath"] + "bin\\";
                }
                else
                {
                    _BasePath = path + "bin\\";
                }
            }
            else
            {
                _BasePath = path;
            }
        }
        private static string _BasePath = null;
        public static string BasePath
        {
            get
            {
                if (_BasePath == null)
                {
                    if (SystemConfig.AppSettings["InstallPath"] == null)
                        _BasePath = "D:\\";
                    else
                        _BasePath = SystemConfig.AppSettings["InstallPath"];
                }
                return _BasePath;
            }
        }
        #endregion  Property 

        #region  Assembly 
        public static Assembly[] _BPAssemblies = null;
        /// <summary>
        ///  Get taking assemblies [dll]
        /// </summary>
        /// <returns></returns>
        public static Assembly[] BPAssemblies
        {
            get
            {
                if (_BPAssemblies == null)
                {
                    string[] fs = System.IO.Directory.GetFiles(BasePath, "BP.*.dll");
                    string[] fs1 = System.IO.Directory.GetFiles(BasePath, "*.ssss");

                    string strs = "";
                    foreach (string str in fs)
                    {
                        strs += str + ";";
                    }

                    foreach (string str in fs1)
                    {
                        strs += str + ";";
                    }

                    fs = strs.Split(';');
                    //  How many   Does not contain  .Web. 的ddl .
                    int fsCount = 0;
                    foreach (string s in fs)
                    {
                        if (s.Length == 0)
                            continue;

                        //if (s.IndexOf(".Web.") != -1)
                        //    continue;

                        fsCount++;
                    }

                    // Add them to the  asss  Go inside .
                    Assembly[] asss = new Assembly[fsCount];
                    int idx = 0;
                    int fsIndex = -1;
                    foreach (string s in fs)
                    {
                        fsIndex++;
                        //if (s.IndexOf(".Web.") != -1)
                        //    continue;

                        if (s.Length == 0)
                            continue;

                        asss[idx] = Assembly.LoadFrom(fs[fsIndex]);
                        idx++;
                    }
                    _BPAssemblies = asss;
                }
                return _BPAssemblies;
            }
        }

        public static Assembly[] BPAssemblies_Bak
        {
            get
            {
                if (_BPAssemblies == null)
                {
                    string[] fs = System.IO.Directory.GetFiles(BasePath, "BP.*.dll");
                    string[] fs1 = System.IO.Directory.GetFiles(BasePath, "*.ssss");

                    string strs = "";
                    foreach (string str in fs)
                    {
                        strs += str + ";";
                    }

                    foreach (string str in fs1)
                    {
                        strs += str + ";";
                    }

                    fs = strs.Split(';');
                    //  How many   Does not contain  .Web. 的ddl .
                    int fsCount = 0;
                    foreach (string s in fs)
                    {
                        if (s.Length == 0)
                            continue;

                        if (s.IndexOf(".Web.") != -1)
                            continue;

                        fsCount++;
                    }

                    // Add them to the  asss  Go inside .
                    Assembly[] asss = new Assembly[fsCount];
                    int idx = 0;
                    int fsIndex = -1;
                    foreach (string s in fs)
                    {
                        fsIndex++;
                        if (s.IndexOf(".Web.") != -1)
                            continue;

                        if (s.Length == 0)
                            continue;

                        asss[idx] = Assembly.LoadFrom(fs[fsIndex]);
                        idx++;
                    }
                    _BPAssemblies = asss;
                }
                return _BPAssemblies;
            }
        }

        /// <summary>
        /// 把class  In memory to 
        /// </summary>
        public static void PutClassIntoCash()
        {
            Entity en = ClassFactory.GetEn("BP.Sys.FAQ");
            Entities ens = ClassFactory.GetEns("BP.Sys.FAQs");
        }
        #endregion  Assembly 

        #region  Type 
        public static Type GetBPType(string className)
        {
            Type typ = null;
            foreach (Assembly ass in BPAssemblies)
            {
                typ = ass.GetType(className);
                if (typ != null)
                    return typ;
            }
            return typ;
        }

        public static ArrayList GetBPTypes(string baseEnsName)
        {
            ArrayList arr = new ArrayList();
            Type baseClass = null;
            foreach (Assembly ass in BPAssemblies)
            {
                if (baseClass == null)
                    baseClass = ass.GetType(baseEnsName);
                Type[] tps = ass.GetTypes();
                for (int i = 0; i < tps.Length; i++)
                {
                    if (tps[i].IsAbstract
                        || tps[i].BaseType == null
                        || !tps[i].IsClass
                        || !tps[i].IsPublic
                        )
                        continue;
                    Type tmp = tps[i].BaseType;

                    if (tmp.Namespace == null)
                        throw new Exception(tmp.FullName);

                    while (tmp != null && tmp.Namespace.IndexOf("BP") != -1)
                    {
                        if (tmp.FullName == baseEnsName)
                            arr.Add(tps[i]);
                        tmp = tmp.BaseType;
                    }
                }
            }
            if (baseClass == null)
            {
                throw new Exception("@ Can not find the type of :" + baseEnsName + "!");
            }
            return arr;

        }

        public static bool IsFromType(string childTypeFullName, string parentTypeFullName)
        {
            foreach (Assembly ass in BPAssemblies)
            {
                Type childType = ass.GetType(childTypeFullName);
                while (childType != null && childType.BaseType != null)
                {
                    if (childType.BaseType.FullName == parentTypeFullName)
                        return true;
                    childType = childType.BaseType;
                }
            }
            return false;
        }
        #endregion  Type 

        #region  Object instance 

        /// <summary>
        ///  Get a object
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static object GetObject_del(string className)
        {
            if (className == "" || className == null)
                throw new Exception("@ Class name to be converted is empty ...");

            /*  There are no judgments memory .*/
            object obj = DA.Cash.GetObjFormApplication(className, null);
            if (obj == null)
            {
                /*  If it is empty , To determine what is transferred to the memory go . */
                if (ClassFactory._BPAssemblies == null)
                {
                    /*  In case _BPAssemblies Is empty , Implementation transferred to it . */
                    ClassFactory.PutClassIntoCash();
                    obj = DA.Cash.GetObjFormApplication(className, null);
                }
            }
            if (obj == null)
            {
                ClassFactory.PutClassIntoCash();
                throw new Exception(" To map the class [" + className + "] Does not exist .");
            }

            return obj;

            //if (Cash.IsExits(cashName, Depositary.Application))
            //    return ; 

            //Type ty = null;
            //object obj=null;
            //foreach (Assembly ass in BPAssemblies)
            //{
            //    ty = ass.GetType(className);
            //    if (ty == null)
            //        continue;

            //    obj = ass.CreateInstance(className);
            //    if (obj != null)
            //    {
            //        Cash.AddObj(cashName, Depositary.Application, obj);
            //        return obj;
            //    }
            //    else
            //        throw new Exception("@ Create an object instance  " + className + "  Failure !");

            //}
            //if(obj==null)
            //    throw new Exception("@ Create an object type  "+className+"  Failure !");
            //return obj ;

        }
        /// <summary>
        ///  Do not try this method to get the case 
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static object GetObject_OK(string className)
        {
            if (className == "" || className == null)
                throw new Exception("@ Class name to be converted is empty ...");

            // Assembly.

            Type ty = null;
            object obj = null;
            foreach (Assembly ass in BPAssemblies)
            {
                ty = ass.GetType(className);
                if (ty == null)
                    continue;

                obj = ass.CreateInstance(className);
                if (obj != null)
                    return obj;
                else
                    throw new Exception("@ Create an object instance  " + className + "  Failure !");

            }
            if (obj == null)
                throw new Exception("@ Create an object type  " + className + "  Failure , Make sure the spelling is wrong .");

            return obj;
        }
        /// <summary>
        ///  According to an abstract base class , Remove this system he inherited the collection from above subclasses .
        ///  Non-abstract classes .
        /// </summary>
        /// <param name="baseEnsName"> Abstract class name </param>
        /// <returns>ArrayList</returns>
        public static ArrayList GetObjects(string baseEnsName)
        {
            ArrayList arr = new ArrayList();
            Type baseClass = null;
            foreach (Assembly ass in BPAssemblies)
            {
                if (baseClass == null)
                    baseClass = ass.GetType(baseEnsName);

                Type[] tps = null;
                try
                {
                    tps = ass.GetTypes();
                }
                catch
                {
                    //throw new Exception(ass.FullName+ass.Evidence.ToString()+ ex.Message);
                    continue;
                }

                for (int i = 0; i < tps.Length; i++)
                {
                    if (tps[i].IsAbstract
                        || tps[i].BaseType == null
                        || !tps[i].IsClass
                        || !tps[i].IsPublic
                        )
                        continue;

                    Type tmp = tps[i].BaseType;
                    if (tmp.Namespace == null)
                        throw new Exception(tmp.FullName);

                    while (tmp != null && tmp.Namespace.IndexOf("BP") != -1)
                    {
                        if (tmp.FullName == baseEnsName)
                            arr.Add(ass.CreateInstance(tps[i].FullName));
                        tmp = tmp.BaseType;
                    }
                }
            }
            if (baseClass == null)
            {
                throw new Exception("@ Can not find the type of " + baseEnsName + "!");
            }
            return arr;
        }
        #endregion  Examples 

        #region  Other 
        /// <summary>
        ///  Generating System Reports 
        /// </summary>
        /// <returns></returns>
        public static string SysReport()
        {
            return null;
        }

        #region  Get en
        private static Hashtable Htable_En;
        /// <summary>
        ///  Get a Entity 
        /// </summary>
        /// <param name="className"> The class name </param>
        /// <returns>En</returns>
        public static Entity GetEn(string className)
        {
            if (Htable_En == null)
            {
                Htable_En = new Hashtable();
                string cl = "BP.En.EnObj";
                ArrayList al = ClassFactory.GetObjects(cl);
                foreach (Entity en in al)
                {
                    string key =  string.Empty ;
                    if( null == en || string.IsNullOrEmpty(key =en.ToString()))
                        continue;
                  
                    if (!Htable_En.ContainsKey(key))
                        Htable_En.Add(key, en);
                    else
                    {
                        continue;
                        //Htable_En[key] = en;
                    }
                }
            }
            object tmp = Htable_En[className];
            return (tmp as Entity);
        }
        #endregion

        #region  Get  GetDataIOEn
        private static Hashtable Htable_DataIOEn;
        /// <summary>
        ///  Get a Entity 
        /// </summary>
        /// <param name="className"> The class name </param>
        /// <returns>En</returns>
        public static BP.DTS.DataIOEn GetDataIOEn(string className)
        {
            if (Htable_DataIOEn == null)
            {
                Htable_DataIOEn = new Hashtable();
                string cl = "BP.DTS.DataIOEn";
                ArrayList al = ClassFactory.GetObjects(cl);
                foreach (BP.DTS.DataIOEn en in al)
                    Htable_DataIOEn.Add(en.ToString(), en);
            }
            object tmp = Htable_DataIOEn[className];
            return (tmp as BP.DTS.DataIOEn);
        }
        #endregion

        #region  Get  GetMethod
        private static Hashtable Htable_Method;
        /// <summary>
        ///  Get a Entity 
        /// </summary>
        /// <param name="className"> The class name </param>
        /// <returns>En</returns>
        public static BP.En.Method GetMethod(string className)
        {
            if (Htable_Method == null)
            {
                Htable_Method = new Hashtable();
                string cl = "BP.En.Method";
                ArrayList al = ClassFactory.GetObjects(cl);
                foreach (BP.En.Method en in al)
                    Htable_Method.Add(en.ToString(), en);
            }
            object tmp = Htable_Method[className];
            return (tmp as BP.En.Method);
        }
        #endregion

        #region  Get  ens
        public static Hashtable Htable_Ens;
        /// <summary>
        ///  Get a Entity 
        /// </summary>
        /// <param name="className"> The class name </param>
        /// <returns>En</returns>
        public static Entities GetEns(string className)
        {
            if (className.Contains(".") == false)
            {
                GEEntitys myens = new GEEntitys(className);
                return myens;
            }

            if (Htable_Ens == null || Htable_Ens.Count == 0)
            {
                Htable_Ens = new Hashtable();
                string cl = "BP.En.Entities";
                ArrayList al = ClassFactory.GetObjects(cl);

                Htable_Ens.Clear();
                foreach (Entities en in al)
                {
                    if (en.ToString() == null)
                        continue;

                    try
                    {
                        Htable_Ens.Add(en.ToString(), en);
                    }
                    catch
                    {
                    }
                }
            }
            Entities ens = Htable_Ens[className] as Entities;

#warning  Clears  cash  Data .
            return ens;
        }
        #endregion

        #region  Get  ens
        public static Hashtable Htable_Evbase;
        /// <summary>
        ///  Get a real event 
        /// </summary>
        /// <param name="className"> The class name </param>
        /// <returns>BP.Sys.EventBase</returns>
        public static BP.Sys.EventBase GetEventBase(string className)
        {
            if (Htable_Evbase == null || Htable_Evbase.Count == 0)
            {
                Htable_Evbase = new Hashtable();
                string cl = "BP.Sys.EventBase";
                ArrayList al = ClassFactory.GetObjects(cl);
                Htable_Evbase.Clear();
                foreach (EventBase en in al)
                {
                    if (en.ToString() == null)
                        continue;
                    try
                    {
                        Htable_Evbase.Add(en.ToString(), en);
                    }
                    catch
                    {
                    }
                }
            }
            BP.Sys.EventBase ens = Htable_Evbase[className] as EventBase;
            return ens;
        }
        #endregion

        #region  Get  ens
        public static Hashtable Htable_XmlEns;
        /// <summary>
        ///  Get a Entity 
        /// </summary>
        /// <param name="className"> The class name </param>
        /// <returns>En</returns>
        public static XML.XmlEns GetXmlEns(string className)
        {
            if (Htable_XmlEns == null)
            {
                Htable_XmlEns = new Hashtable();
                string cl = "BP.XML.XmlEns";
                ArrayList al = ClassFactory.GetObjects(cl);
                foreach (XML.XmlEns en in al)
                    Htable_XmlEns.Add(en.ToString(), en);
            }
            object tmp = Htable_XmlEns[className];
            return (tmp as XML.XmlEns);
        }
        #endregion

        #region  Get  en
        public static Hashtable Htable_XmlEn;
        /// <summary>
        ///  Get a Entity 
        /// </summary>
        /// <param name="className"> The class name </param>
        /// <returns>En</returns>
        public static XML.XmlEn GetXmlEn(string className)
        {
            if (Htable_XmlEn == null)
            {
                Htable_XmlEn = new Hashtable();
                string cl = "BP.XML.XmlEn";
                ArrayList al = ClassFactory.GetObjects(cl);
                foreach (XML.XmlEn en in al)
                    Htable_XmlEn.Add(en.ToString(), en);
            }
            object tmp = Htable_XmlEn[className];
            return (tmp as XML.XmlEn);
        }
        #endregion

        #endregion
    }
}
