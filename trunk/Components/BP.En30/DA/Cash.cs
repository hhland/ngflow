using System;
using System.Collections;
using System.IO;
using System.Text;
using BP.En;
using BP.Pub;
using BP.Sys;

namespace BP.DA
{	 
	/// <summary>
	/// Cash  The summary .
	/// </summary>
    public class Cash
    {
        static Cash()
        {
            if (SystemConfig.IsBSsystem==false)
            {
                CS_Cash = new Hashtable();
            }
        }
        public static readonly Hashtable CS_Cash;

        #region Bill_Cash  Document templates cash.
        private static Hashtable _Bill_Cash;
        public static Hashtable Bill_Cash
        {
            get
            {
                if (_Bill_Cash == null)
                    _Bill_Cash = new Hashtable();
                return _Bill_Cash;
            }
        }
        public static string GetBillStr(string cfile, bool isCheckCash)
        {
            string val = Bill_Cash[cfile] as string;
            if (isCheckCash == true)
                val = null;

            if (val == null)
            {
                string file = null;
                if (cfile.Contains(":"))
                    file = cfile;
                else
                    file = SystemConfig.PathOfDataUser + "\\CyclostyleFile\\" + cfile;


                if (SystemConfig.IsDebug)
                {
                    BP.Pub.RepBill.RepairBill(file);
                }
                try
                {
                    StreamReader read = new StreamReader(file, System.Text.Encoding.ASCII); //  File stream .
                    val = read.ReadToEnd();  // Reading is completed .
                    read.Close(); //  Shut down .
                }
                catch (Exception ex)
                {
                    throw new Exception("@ An error occurred while reading the document template .cfile=" + cfile + " @Ex=" + ex.Message);
                }
                _Bill_Cash[cfile] = val;
            }
            return val.Substring(0);
        }
        public static string[] GetBillParas(string cfile, string ensStrs, Entities ens)
        {
            string[] paras = Bill_Cash[cfile + "Para"] as string[];
            if (paras != null)
                return paras;

            Attrs attrs = new Attrs();
            foreach (Entity en in ens)
            {
                string perKey = en.ToString();

                Attrs enAttrs = en.EnMap.Attrs;
                foreach (Attr attr in enAttrs)
                {
                    Attr attrN = new Attr();
                    attrN.Key = perKey + "." + attr.Key;

                    //attrN.Key = attrN.Key.Replace("\\f2","");
                    //attrN.Key = attrN.Key.Replace("\\f3", "");

                    if (attr.IsRefAttr)
                    {
                        attrN.Field = perKey + "." + attr.Key + "Text";
                    }
                    attrN.MyDataType = attr.MyDataType;
                    attrN.MyFieldType = attr.MyFieldType;
                    attrN.UIBindKey = attr.UIBindKey;
                    attrN.Field = attr.Field;
                    attrs.Add(attrN);
                }
            }

            paras = Cash.GetBillParas_Gener(cfile, attrs);
            _Bill_Cash[cfile + "Para"] = paras;
            return paras;
        }
        public static string[] GetBillParas(string cfile, string ensStrs, Entity en)
        {
            string[] paras = Bill_Cash[cfile + "Para"] as string[];
            if (paras != null)
                return paras;

            paras = Cash.GetBillParas_Gener(cfile, en.EnMap.Attrs);
            _Bill_Cash[cfile + "Para"] = paras;
            return paras;
        }
        public static string[] GetBillParas_Gener(string cfile, Attrs attrs)
        {
            //  Attrs attrs = en.EnMap.Attrs;
            string[] paras = new string[300];
            string Billstr = Cash.GetBillStr(cfile, true);
            char[] chars = Billstr.ToCharArray();
            string para = "";
            int i = 0;
            bool haveError = false;
            string msg = "";
            foreach (char c in chars)
            {
                if (c == '>')
                {
                    #region  First, solve the problem of space .
                    string real = para.Clone().ToString();
                    if (attrs != null && real.Contains(" "))
                    {
                        real = real.Replace(" ", "");
                        Billstr = Billstr.Replace(para, real);
                        para = real;
                        haveError = true;
                    }
                    #endregion  First, solve the problem of space .

                    #region  Addressing the special symbol 
                    if (attrs != null && real.Contains("\\") && real.Contains("ND") == false)
                    {
                        haveError = true;
                        string findKey = null;
                        int keyLen = 0;
                        foreach (Attr attr in attrs)
                        {
                            if (real.Contains(attr.Key))
                            {
                                if (keyLen <= attr.Key.Length)
                                {
                                    keyLen = attr.Key.Length;
                                    findKey = attr.Key;
                                }
                            }
                        }

                        if (findKey == null)
                        {
                            msg += "@ Parameters :<font color=red><b>[" + real + "]</b></font> Possible spelling errors .";
                            continue;
                        }

                        if (real.Contains(findKey + ".NYR") == true)
                        {
                            real = findKey + ".NYR";
                        }
                        else if (real.Contains(findKey + ".RMB") == true)
                        {
                            real = findKey + ".RMB";
                        }
                        else if (real.Contains(findKey + ".RMBDX") == true)
                        {
                            real = findKey + ".RMBDX";
                        }
                        else
                        {
                            real = findKey;
                        }

                        Billstr = Billstr.Replace(para, real);
                        // msg += "@ Parameters :<font color=red><b>[" + para + "]</b></font> Does not meet specifications .";
                        //continue;
                    }
                    #endregion  First, solve the problem of space .

                    paras.SetValue(para, i);
                    i++;
                }

                if (c == '<')
                {
                    para = ""; //  If you encounter  '<'  Start recording 
                }
                else
                {
                    if (c.ToString() == "")
                        continue;
                    para += c.ToString();
                }
            }

            if (haveError)
            {
                string myfile = SystemConfig.PathOfDataUser + "\\CyclostyleFile\\" + cfile;
                if (System.IO.File.Exists(myfile) == false)
                    myfile = cfile;

                // throw new Exception("@ No files :"+myfile);

                StreamWriter wr = new StreamWriter(myfile,
                    false, Encoding.ASCII);
                wr.Write(Billstr);
                wr.Close();
            }

            if (msg != "")
            {
                string s = "@ Help : Open it with notepad template , Find red font .  Illegal character went inside angle brackets , Such as :¡¶|f0|fs20RDT.NYR|lang1033|kerning2¡·, Examples of modified :¡¶RDT.NYR¡·.@ Note the double quotes instead of single quotes , Vertical instead of a backslash .";
                //throw new Exception("@ Document templates £¨"+cfile+"£© The following labels error , The system can not fix it , You need to manually remove the tag or use Notepad to open repair find that they write tag .@" + msg + s);
            }
            return paras;
        }
        #endregion

        #region Conn cash
        private static Hashtable _Conn_Cash;
        public static Hashtable Conn_Cash
        {
            get
            {
                if (_Conn_Cash == null)
                    _Conn_Cash = new Hashtable();
                return _Conn_Cash;
            }
        }
        public static object GetConn(string fk_emp)
        {
            return Conn_Cash[fk_emp];
        }
        public static void SetConn(string fk_emp, object csh)
        {
            if (fk_emp == null )
                throw new Exception("fk_emp.  csh  Parameter has a null .");
            Conn_Cash[fk_emp] = csh;
        }
        #endregion

        #region BS_Cash
        private static Hashtable _BS_Cash;
        public static Hashtable BS_Cash
        {
            get
            {
                if (_BS_Cash == null)
                    _BS_Cash = new Hashtable();
                return _BS_Cash;
            }
        }
        #endregion

        #region SQL cash
        private static Hashtable _SQL_Cash;
        public static Hashtable SQL_Cash
        {
            get
            {
                if (_SQL_Cash == null)
                    _SQL_Cash = new Hashtable();
                return _SQL_Cash;
            }
        }
        public static BP.En.SQLCash GetSQL(string clName)
        {
            return SQL_Cash[clName] as BP.En.SQLCash;
        }
        public static void SetSQL(string clName, BP.En.SQLCash csh)
        {
            if (clName == null || csh == null)
                throw new Exception("clName.  csh  Parameter has a null .");
            SQL_Cash[clName] = csh;
        }
        public static void InitSQL()
        {
            ArrayList al = ClassFactory.GetObjects("BP.En.Entity");
            foreach (BP.En.Entity en in al)
            {
                string sql = BP.En.SqlBuilder.Retrieve(en);
            }
        }
        #endregion

        #region EnsData cash
        private static Hashtable _EnsData_Cash;
        public static Hashtable EnsData_Cash
        {
            get
            {
                if (_EnsData_Cash == null)
                    _EnsData_Cash = new Hashtable();
                return _EnsData_Cash;
            }
        }
        public static BP.En.Entities GetEnsData(string clName)
        {
            Entities ens = EnsData_Cash[clName] as BP.En.Entities;
            if (ens == null)
                return null;

            if (ens.Count == 0)
                return null;
            //throw new Exception(clName + " The number of 0");
            return ens;
        }
        public static void EnsDataSet(string clName, BP.En.Entities obj)
        {
            if (obj.Count == 0)
            {
                ///obj.RetrieveAll();
#warning  Set the number of 

                //  throw new Exception(clName + " Set the number of 0 ,  Make sure that the entity cache , Whether there is data ?sq=select * from " + obj.GetNewEntity.EnMap.PhysicsTable);
            }

            EnsData_Cash[clName] = obj;
        }
        public static void Remove(string clName)
        {
            EnsData_Cash.Remove(clName);
        }
        #endregion

        #region EnsData cash  Expand   Occasional cash  File .
        private static Hashtable _EnsData_Cash_Ext;
        public static Hashtable EnsData_Cash_Ext
        {
            get
            {
                if (_EnsData_Cash_Ext == null)
                    _EnsData_Cash_Ext = new Hashtable();
                return _EnsData_Cash_Ext;
            }
        }
        /// <summary>
        ///  As part of the data buffering to do 
        /// </summary>
        /// <param name="clName"></param>
        /// <returns></returns>
        public static BP.En.Entities GetEnsDataExt(string clName)
        {
            //  Determine whether the failure of the .
            if (SystemConfig.IsTempCashFail)
            {
                EnsData_Cash_Ext.Clear();
                return null;
            }

            try
            {
                BP.En.Entities ens;
                ens = EnsData_Cash_Ext[clName] as BP.En.Entities;
                return ens;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        ///  As part of the data buffering to do 
        /// </summary>
        /// <param name="clName"></param>
        /// <param name="obj"></param>
        public static void SetEnsDataExt(string clName, BP.En.Entities obj)
        {
            if (clName == null || obj == null)
                throw new Exception("clName.  obj  Parameter has a null .");
            EnsData_Cash_Ext[clName] = obj;
        }
        #endregion

        #region map cash
        private static Hashtable _Map_Cash;
        public static Hashtable Map_Cash
        {
            get
            {
                if (_Map_Cash == null)
                    _Map_Cash = new Hashtable();
                return _Map_Cash;
            }
        }
        public static BP.En.Map GetMap(string clName)
        {
            try
            {
                return Map_Cash[clName] as BP.En.Map;
            }
            catch
            {
                return null;
            }
        }
        public static void SetMap(string clName, BP.En.Map map)
        {
            if (clName == null)
                return;
            //    throw new Exception("clName. Can not be empty .");
            if (map == null)
            {
                Map_Cash.Remove(clName);
                return;
            }
            Map_Cash[clName] = map;
        }
        #endregion

        #region  Remove Objects 
        /// <summary>
        /// ´Ó Cash  Remove the object inside .
        /// </summary>
        public static object GetObj(string key, Depositary where)
        {

#if DEBUG
            if (where == Depositary.None)
                throw new Exception(" You do not have to [" + key + "] Put session or application  Which can not find them .");
#endif

            if (SystemConfig.IsBSsystem)
            {
                if (where == Depositary.Application)
                    // return  System.Web.HttpContext.Current.Cache[key];
                    return BS_Cash[key]; //  System.Web.HttpContext.Current.Cache[key];
                else
                    return System.Web.HttpContext.Current.Session[key];
            }
            else
            {
                return CS_Cash[key];
            }
        }
        public static object GetObj(string key)
        {
            if (SystemConfig.IsBSsystem)
            {
                object obj = BS_Cash[key]; // Cash.GetObjFormApplication(key, null);
                if (obj == null)
                    obj = Cash.GetObjFormSession(key);
                return obj;
            }
            else
            {
                return CS_Cash[key];
            }
        }
        /// <summary>
        ///  Delete  like  Cache object names .
        /// </summary>
        /// <param name="likeKey"></param>
        /// <returns></returns>
        public static int DelObjFormApplication(string likeKey)
        {
            int i = 0;
            if (SystemConfig.IsBSsystem)
            {
                string willDelKeys = "";
                foreach (string key  in BS_Cash.Keys)
                {
                    if (key.Contains(likeKey) == false)
                        continue;
                    willDelKeys += "@" + key;
                }

                string[] strs = willDelKeys.Split('@');
                foreach (string s in strs)
                {
                    if (s == null || s == "")
                        continue;
                    BS_Cash.Remove(s);
                    i++;
                }
            }
            else
            {
                string willDelKeys = "";
                foreach (string key in CS_Cash.Keys)
                {
                    if (key.Contains(likeKey) == false)
                        continue;
                    willDelKeys += "@" + key;
                }

                string[] strs = willDelKeys.Split('@');
                foreach (string s in strs)
                {
                    if (s == null || s == "")
                        continue;
                    CS_Cash.Remove(s);
                    i++;
                }
            }

            return i;
        }
        public static object GetObjFormApplication(string key, object isNullAsVal)
        {
            if (SystemConfig.IsBSsystem)
            {
                object obj = BS_Cash[key]; // System.Web.HttpContext.Current.Cache[key];
                if (obj == null)
                    return isNullAsVal;
                else
                    return obj;
            }
            else
            {
                object obj = CS_Cash[key];
                if (obj == null)
                    return isNullAsVal;
                else
                    return obj;
            }
        }
        public static object GetObjFormSession(string key)
        {
            if (SystemConfig.IsBSsystem)
            {
                try
                {
                    return System.Web.HttpContext.Current.Session[key];
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return CS_Cash[key];
            }
        }
        #endregion

        #region Remove Obj
        /// <summary>
        /// RemoveObj
        /// </summary>
        /// <param name="key"></param>
        /// <param name="where"></param>
        public static void RemoveObj(string key, Depositary where)
        {
            if (Cash.IsExits(key, where) == false)
                return;

            if (SystemConfig.IsBSsystem)
            {
                if (where == Depositary.Application)
                    System.Web.HttpContext.Current.Cache.Remove(key);
                else
                    System.Web.HttpContext.Current.Session.Remove(key);
            }
            else
            {
                CS_Cash.Remove(key);
            }
        }
        #endregion

        #region  Into the object 
        public static void RemoveObj(string key)
        {
            BS_Cash.Remove(key);
        }
        public static void AddObj(string key, Depositary where, object obj)
        {
            if (key == null)
                throw new Exception(" You need to obj=" + obj.ToString() + ", Set the main key .key");

            if (obj == null)
                throw new Exception(" You need to obj=null   Set the main key .key=" + key);

#if DEBUG
            if (where == Depositary.None)
                throw new Exception(" You do not have to [" + key + "] Put  session or application  Set inside them .");
#endif
            //if (Cash.IsExits(key, where))
            //    return;

            if (SystemConfig.IsBSsystem)
            {
                if (where == Depositary.Application)
                {
                    BS_Cash[key] = obj;
                }
                else
                {
                    System.Web.HttpContext.Current.Session[key] = obj;
                }
            }
            else
            {
                if (CS_Cash.ContainsKey(key))
                    CS_Cash[key] = obj;
                else
                    CS_Cash.Add(key, obj);
            }
        }
        #endregion

        #region  The object is not there to judge 
        /// <summary>
        ///  The object is not there to judge 
        /// </summary>
        public static bool IsExits(string key, Depositary where)
        {
            if (SystemConfig.IsBSsystem)
            {
                if (where == Depositary.Application)
                {
                    if (System.Web.HttpContext.Current.Cache[key] == null)
                        return false;
                    else
                        return true;
                }
                else
                {
                    if (System.Web.HttpContext.Current.Session[key] == null)
                        return false;
                    else
                        return true;
                }
            }
            else
            {
                return CS_Cash.ContainsKey(key);
            }
        }
        #endregion

        /// <summary>
        ///  Clear cache execution 
        /// </summary>
        public void ClearCash()
        {

        }
    }

    public class CashEntity
    {
        #region Hashtable  Property 
        private static Hashtable _Cash;
        public static Hashtable DCash
        {
            get
            {
                if (_Cash == null)
                    _Cash = new Hashtable();
                return _Cash;
            }
        }
        #endregion

        /// <summary>
        ///  The entity into the cache 
        /// </summary>
        /// <param name="enName"></param>
        /// <param name="ens"></param>
        /// <param name="enPK"></param>
        public static void PubEns(string enName, Entities ens, string enPK)
        {
            Hashtable ht = CashEntity.DCash[enName] as Hashtable;
            if (ht == null)
                ht = new Hashtable();

            ht.Clear();
            foreach (Entity en in ens)
                ht.Add(en.GetValStrByKey(enPK), en);

            //  The entity set into .
            CashEntity.DCash[enName + "Ens"] = ens;
        }
        public static Entities GetEns(string enName)
        {
            Entities ens = CashEntity.DCash[enName + "Ens"] as Entities;
            return ens;
        }
        /// <summary>
        ///  Update the object 
        /// </summary>
        /// <param name="enName"></param>
        /// <param name="key"></param>
        /// <param name="en"></param>
        public static void Update(string enName, string key, Entity en)
        {
            Hashtable ht = CashEntity.DCash[enName] as Hashtable;
            if (ht == null)
            {
                ht = new Hashtable();
                CashEntity.DCash[enName] = ht;
            }
            ht[key] = en;

            // Clear collection .
            CashEntity.DCash.Remove(enName + "Ens");
        }
        /// <summary>
        ///  Get an entity 
        /// </summary>
        /// <param name="enName"> Entity Name</param>
        /// <param name="pkVal"> Primary key value </param>
        /// <returns> Return to this entity </returns>
        public static Entity Select(string enName, string pkVal)
        {
            Hashtable ht = CashEntity.DCash[enName] as Hashtable;
            if (ht == null)
                return null;

            return ht[pkVal] as Entity;
        }
        /// <summary>
        ///  Delete 
        /// </summary>
        /// <param name="enName"></param>
        /// <param name="pkVal"></param>
        public static void Delete(string enName, string pkVal)
        {
            Hashtable ht = CashEntity.DCash[enName] as Hashtable;
            if (ht == null)
                return;

            ht.Remove(pkVal);
            // Clear collection .
            CashEntity.DCash.Remove(enName + "Ens");
        }
        /// <summary>
        ///  Insert 
        /// </summary>
        /// <param name="enName"></param>
        /// <param name="en"></param>
        /// <param name="pkVal"></param>
        public static void Insert(string enName, string pkVal, Entity en)
        {
            Hashtable ht = CashEntity.DCash[enName] as Hashtable;
            if (ht == null)
                return;

            //edited by liuxc,2014-8-21 17:21
            if (ht.ContainsKey(pkVal))
                ht[pkVal] = en;
            else
                ht.Add(pkVal, en);

            // Clear collection .
            CashEntity.DCash.Remove(enName + "Ens");
        }
    }
}
