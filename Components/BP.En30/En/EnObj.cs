using System;
using System.Collections;
using BP.DA;
using System.Data;
using BP.Sys;
using BP.En;

namespace BP.En
{
    /// <summary>
    ///  Access Control 
    /// </summary>
    public class UAC
    {
        #region  Common method 
        public void Readonly()
        {
            this.IsUpdate = false;
            this.IsDelete = false;
            this.IsInsert = false;
            this.IsAdjunct = false;
            this.IsView = true;
        }
        /// <summary>
        ///  All Open 
        /// </summary>
        public void OpenAll()
        {
            this.IsUpdate = true;
            this.IsDelete = true;
            this.IsInsert = true;
            this.IsAdjunct = false;
            this.IsView = true;
        }
        /// <summary>
        ///  Set all permissions for a job 
        /// </summary>
        /// <param name="fk_station"></param>
        public void OpenAllForStation(string fk_station)
        {
            Paras ps = new Paras();
            ps.Add("user", Web.WebUser.No);
            ps.Add("st", fk_station);

            if (DBAccess.IsExits("SELECT FK_Emp FROM Port_EmpStation WHERE FK_Emp=" + SystemConfig.AppCenterDBVarStr + "user AND FK_Station=" + SystemConfig.AppCenterDBVarStr + "st", ps))
                this.OpenAll();
            else
                this.Readonly();
        }
        /// <summary>
        ///  Only to the administrator 
        /// </summary>
        public UAC OpenForSysAdmin()
        {
            if (SystemConfig.SysNo == "WebSite")
            {
                this.OpenAll();
                return this;
            }

            if (BP.Web.WebUser.No == "admin")
                this.OpenAll();
            return this;
        }
        public UAC OpenForAppAdmin()
        {
            if (BP.Web.WebUser.No != null && BP.Web.WebUser.No.Contains("admin") == true)
            {
                this.OpenAll();
            }
            return this;
        }
        #endregion

        #region  Control Properties 
        /// <summary>
        ///  Is inserted 
        /// </summary>
        public bool IsInsert = false;
        /// <summary>
        ///  Delete 
        /// </summary>
        public bool IsDelete = false;
        /// <summary>
        ///  Whether the update 
        /// </summary>
        public bool IsUpdate = false;
        /// <summary>
        ///  Check to see 
        /// </summary>
        public bool IsView = true;
        /// <summary>
        ///  Accessory 
        /// </summary>
        public bool IsAdjunct = false;
        #endregion

        #region  Structure 
        /// <summary>
        ///  User access 
        /// </summary>
        public UAC()
        {

        }
        #endregion
    }
    /// <summary>
    /// Entity  The summary .
    /// </summary>	
    [Serializable]
    abstract public class EnObj
    {
        #region  Access Control .
        private string _DBVarStr = null;
        public string HisDBVarStr
        {
            get
            {
                if (_DBVarStr != null)
                    return _DBVarStr;

                _DBVarStr = this.EnMap.EnDBUrl.DBVarStr;
                return _DBVarStr;
            }
        }
        /// <summary>
        ///  His access control .
        /// </summary>
        protected UAC _HisUAC = null;
        /// <summary>
        ///  Get  uac  Control .
        /// </summary>
        /// <returns></returns>
        public virtual UAC HisUAC
        {
            get
            {
                if (_HisUAC == null)
                {
                    _HisUAC = new UAC();
                    if (BP.Web.WebUser.No == "admin")
                    {
                        _HisUAC.IsAdjunct = false;
                        _HisUAC.IsDelete = true;
                        _HisUAC.IsInsert = true;
                        _HisUAC.IsUpdate = true;
                        _HisUAC.IsView = true;
                    }
                }
                return _HisUAC;
            }
        }
        #endregion

        #region  Remove the external configuration of the attribute information 
        /// <summary>
        ///  Take out Map  Extended Attributes .
        ///  For the first 3 Extended attributes the development side .
        /// </summary>
        /// <param name="key"> Property Key</param>
        /// <returns> Properties set </returns>
        public string GetMapExtAttrByKey(string key)
        {
            Paras ps = new Paras();
            ps.Add("enName", this.ToString());
            ps.Add("key", key);

            return (string)DBAccess.RunSQLReturnVal("select attrValue from Sys_ExtMap WHERE className=" + SystemConfig.AppCenterDBVarStr + "enName AND attrKey=" + SystemConfig.AppCenterDBVarStr + "key", ps);
        }
        #endregion

        #region CreateInstance
        /// <summary>
        ///  Create an instance 
        /// </summary>
        /// <returns> Instance of itself </returns>
        public Entity CreateInstance()
        {
            return this.GetType().Assembly.CreateInstance(this.ToString()) as Entity;
            //return ClassFactory.GetEn(this.ToString());
        }
        private Entities _GetNewEntities = null;
        #endregion

        #region  Method 
        /// <summary>
        ///  Reset default information .
        /// </summary>
        public void ResetDefaultVal()
        {
            Attrs attrs = this.EnMap.Attrs;
            foreach (Attr attr in attrs)
            {
                string v = attr.DefaultValOfReal as string;
                if (v == null)
                    continue;

                if (attr.DefaultValOfReal.Contains("@") == false)
                    continue;

                string myval = this.GetValStrByKey(attr.Key);

                //  Set Default .
                switch (v)
                {
                    case "@WebUser.No":
                        if (attr.UIIsReadonly == true)
                        {
                            this.SetValByKey(attr.Key, Web.WebUser.No);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(myval) || myval == v)
                                this.SetValByKey(attr.Key, Web.WebUser.No);
                        }
                        continue;
                    case "@WebUser.Name":
                        if (attr.UIIsReadonly == true)
                        {
                            this.SetValByKey(attr.Key, Web.WebUser.Name);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(myval) || myval == v)
                                this.SetValByKey(attr.Key, Web.WebUser.Name);
                        }
                        continue;
                    case "@WebUser.FK_Dept":
                        if (attr.UIIsReadonly == true)
                        {
                            this.SetValByKey(attr.Key, Web.WebUser.FK_Dept);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(myval) || myval == v)
                                this.SetValByKey(attr.Key, Web.WebUser.FK_Dept);
                        }
                        continue;
                    case "@WebUser.FK_DeptName":
                        if (attr.UIIsReadonly == true)
                        {
                            this.SetValByKey(attr.Key, Web.WebUser.FK_DeptName);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(myval) || myval == v)
                                this.SetValByKey(attr.Key, Web.WebUser.FK_DeptName);
                        }
                        continue;
                    case "@WebUser.FK_DeptNameOfFull":
                        if (attr.UIIsReadonly == true)
                        {
                            this.SetValByKey(attr.Key, Web.WebUser.FK_DeptNameOfFull);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(myval) || myval == v)
                                this.SetValByKey(attr.Key, Web.WebUser.FK_DeptNameOfFull);
                        }
                        continue;
                    case "@RDT":
                        if (attr.UIIsReadonly == true)
                        {
                            if (attr.MyDataType == DataType.AppDate || myval == v)
                                this.SetValByKey(attr.Key, DataType.CurrentData);

                            if (attr.MyDataType == DataType.AppDateTime || myval == v)
                                this.SetValByKey(attr.Key, DataType.CurrentDataTime);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(myval) || myval == v)
                            {
                                if (attr.MyDataType == DataType.AppDate)
                                    this.SetValByKey(attr.Key, DataType.CurrentData);
                                else
                                    this.SetValByKey(attr.Key, DataType.CurrentDataTime);
                            }
                        }
                        continue;
                    default:
                        continue;
                }
            }
        }
        /// <summary>
        ///  All the values are set to default values , But except for the primary key .
        /// </summary>
        public void ResetDefaultValAllAttr()
        {
            Attrs attrs = this.EnMap.Attrs;
            foreach (Attr attr in attrs)
            {
                if (attr.UIIsReadonly == false && attr.DefaultValOfReal != null)
                    continue;

                if (attr.IsPK)
                    continue;

                string v = attr.DefaultValOfReal as string;
                if (v == null)
                {
                    this.SetValByKey(attr.Key, "");
                    continue;
                }


                if (v.Contains("@") == false)
                {
                    this.SetValByKey(attr.Key, v);
                    continue;
                }



                //if (v.Contains("@") == false || v == "0" || v == "0.00")
                //{
                //    this.SetValByKey(attr.Key, v);
                //    continue;
                //}

                //  Set Default .
                switch (v)
                {
                    case "@WebUser.No":
                        this.SetValByKey(attr.Key, Web.WebUser.No);
                        continue;
                    case "@WebUser.Name":
                        this.SetValByKey(attr.Key, Web.WebUser.Name);
                        continue;
                    case "@WebUser.FK_Dept":
                        this.SetValByKey(attr.Key, Web.WebUser.FK_Dept);
                        continue;
                    case "@WebUser.FK_DeptName":
                        this.SetValByKey(attr.Key, Web.WebUser.FK_DeptName);
                        continue;
                    case "@WebUser.FK_DeptNameOfFull":
                        this.SetValByKey(attr.Key, Web.WebUser.FK_DeptNameOfFull);
                        continue;
                    case "@RDT":
                        if (attr.MyDataType == DataType.AppDate)
                            this.SetValByKey(attr.Key, DataType.CurrentData);
                        else
                            this.SetValByKey(attr.Key, DataType.CurrentDataTime);
                        continue;
                    default:
                        continue;
                }
            }
        }
        #endregion

        #region  Structure 
        /// <summary>
        ///  Entity 
        /// </summary>
        public EnObj()
        {
        }
        private Map _tmpEnMap = null;
        /// <summary>
        /// Map
        /// </summary>
        protected Map _enMap
        {
            get
            {
                if (_tmpEnMap != null)
                    return _tmpEnMap;

                Map obj = Cash.GetMap(this.ToString());
                if (obj == null)
                {
                    if (_tmpEnMap == null)
                        return null;
                    else
                        return _tmpEnMap;
                }
                else
                {
                    _tmpEnMap = obj;
                }
                return _tmpEnMap;
            }
            set
            {
                if (value == null)
                {
                    _tmpEnMap = null;
                    return;
                }

                Map mp = (Map)value;
                if (SystemConfig.IsDebug)
                {
                    //#region  Inspection map  Is reasonable .
                    //if (mp != null)
                    //{
                    //    int i = 0;
                    //    foreach (Attr attr in this.EnMap.Attrs)
                    //    {
                    //        if (attr.MyFieldType == FieldType.PK || attr.MyFieldType == FieldType.PKEnum || attr.MyFieldType == FieldType.PKFK)
                    //            i++;
                    //    }
                    //    if (i == 0)
                    //        throw new Exception("@ Did not give 【" + this.EnDesc + "】 Define a primary key .");

                    //    if (this.IsNoEntity)
                    //    {
                    //        if (mp.Attrs.Contains("No") == false)
                    //            throw new Exception("@EntityNo 类map No  No  Property .@类" + mp.EnDesc + " , " + this.ToString());

                    //        if (i != 1)
                    //            throw new Exception("@ Multiple primary keys  EntityNo  Class is not allowed . @类" + mp.EnDesc + " , " + this.ToString());
                    //    }
                    //    else if (this.IsOIDEntity)
                    //    {
                    //        if (mp.Attrs.Contains("OID") == false)
                    //            throw new Exception("@EntityOID 类map No  OID  Property .@类" + mp.EnDesc + " , " + this.ToString());
                    //        if (i != 1)
                    //            throw new Exception("@ Multiple primary keys  EntityOID  Class is not allowed . @类" + mp.EnDesc + " , " + this.ToString());
                    //    }
                    //    else
                    //    {
                    //        if (mp.Attrs.Contains("MyPK"))
                    //            if (i != 1)
                    //                throw new Exception("@ Multiple primary keys  EntityMyPK  Class is not allowed . @类" + mp.EnDesc + " , " + this.ToString());
                    //    }
                    //}
                    //#endregion  Inspection map  Is reasonable .
                }

                if (mp == null || mp.DepositaryOfMap == Depositary.None)
                {
                    _tmpEnMap = mp;
                    return;
                }

                Cash.SetMap(this.ToString(), mp);
                _tmpEnMap = mp;
            }
        }
        /// <summary>
        ///  Subclass needs to inherit 
        /// </summary>
        public abstract Map EnMap
        {
            get;
        }
        #endregion

        #region row  Store entity data 
        /// <summary>
        ///  Entity  map  Information .	
        /// </summary>		
        //public abstract void EnMap();		
        private Row _row = null;
        public Row Row
        {
            get
            {
                if (this._row == null)
                {
                    this._row = new Row();
                    this._row.LoadAttrs(this.EnMap.Attrs);
                }
                return this._row;
            }
            set
            {
                this._row = value;
            }
        }
        #endregion

        #region  Operating on the property .

        #region  Setting method 
        public void SetValByKeySuperLink(string attrKey, string val)
        {
            this.SetValByKey(attrKey, DataType.DealSuperLink(val));
        }

        /// <summary>
        ///  Set up object Value type 
        /// </summary>
        /// <param name="attrKey">attrKey</param>
        /// <param name="val">val</param>
        public void SetValByKey(string attrKey, string val)
        {
            switch (val)
            {
                case null:
                case "&nbsp;":
                    val = "";
                    break;
                case "RDT":
                    if (val.Length > 4)
                    {
                        this.SetValByKey("FK_NY", val.Substring(0, 7));
                        this.SetValByKey("FK_ND", val.Substring(0, 4));
                    }
                    break;
                default:
                    break;
            }
            this.Row.SetValByKey(attrKey, val);
        }
        public void SetValByKey(string attrKey, int val)
        {
            this.Row.SetValByKey(attrKey, val);
        }
        public void SetValByKey(string attrKey, Int64 val)
        {
            this.Row.SetValByKey(attrKey, val);
        }
        public void SetValByKey(string attrKey, float val)
        {
            this.Row.SetValByKey(attrKey, val);
        }
        public void SetValByKey(string attrKey, decimal val)
        {
            this.Row.SetValByKey(attrKey, val);
        }
        public void SetValByKey(string attrKey, object val)
        {
            this.Row.SetValByKey(attrKey, val);
        }

        public void SetValByDesc(string attrDesc, object val)
        {
            if (val == null)
                throw new Exception("@ You can not set property [" + attrDesc + "]null 值.");
            this.Row.SetValByKey(this.EnMap.GetAttrByDesc(attrDesc).Key, val);
        }

        /// <summary>
        ///  Set the value of the associated type 
        /// </summary>
        /// <param name="attrKey">attrKey</param>
        /// <param name="val">val</param>
        public void SetValRefTextByKey(string attrKey, object val)
        {
            this.SetValByKey(attrKey + "Text", val);
        }
        /// <summary>
        ///  Set up bool Value type 
        /// </summary>
        /// <param name="attrKey">attrKey</param>
        /// <param name="val">val</param>
        public void SetValByKey(string attrKey, bool val)
        {
            if (val)
                this.SetValByKey(attrKey, 1);
            else
                this.SetValByKey(attrKey, 0);
        }
        /// <summary>
        ///  Set Default 
        /// </summary>
        public void SetDefaultVals()
        {
            foreach (Attr attr in this.EnMap.Attrs)
            {
                this.SetValByKey(attr.Key, attr.DefaultVal);
            }
        }
        /// <summary>
        ///  Setting date type 
        /// </summary>
        /// <param name="attrKey">attrKey</param>
        /// <param name="val">val</param>
        public void SetDateValByKey(string attrKey, string val)
        {
            try
            {
                this.SetValByKey(attrKey, DataType.StringToDateStr(val));
            }
            catch (System.Exception ex)
            {
                throw new Exception("@ Illegal date data format :key=[" + attrKey + "],value=" + val + " " + ex.Message);
            }
        }
        #endregion

        #region  Value Method 
        /// <summary>
        ///  Obtain Object
        /// </summary>
        /// <param name="attrKey"></param>
        /// <returns></returns>
        public Object GetValByKey(string attrKey)
        {
            return this.Row.GetValByKey(attrKey);

            //try
            //{
            //    return this.Row.GetValByKey(attrKey);				
            //}
            //catch(Exception ex)
            //{
            //    throw new Exception(ex.Message+"  "+attrKey+" EnsName="+this.ToString() );
            //}
        }
        /// <summary>
        /// GetValDateTime
        /// </summary>
        /// <param name="attrKey"></param>
        /// <returns></returns>
        public DateTime GetValDateTime(string attrKey)
        {
            return DataType.ParseSysDateTime2DateTime(this.GetValStringByKey(attrKey));
        }
        /// <summary>
        ///  In determining the   attrKey  Presence  map  Under the circumstances in order to use it 
        /// </summary>
        /// <param name="attrKey"></param>
        /// <returns></returns>
        public string GetValStrByKey(string key)
        {
            return this.Row.GetValByKey(key).ToString();
        }
        public string GetValStrByKey(string key, string isNullAs)
        {
            try
            {
                return this.Row.GetValByKey(key).ToString();
            }
            catch
            {
                return isNullAs;
            }
        }
        /// <summary>
        ///  Obtain String
        /// </summary>
        /// <param name="attrKey"></param>
        /// <returns></returns>
        public string GetValStringByKey(string attrKey)
        {
            switch (attrKey)
            {
                case "Doc":
                    string s = this.Row.GetValByKey(attrKey).ToString();
                    if (s == "")
                        s = this.GetValDocText();
                    return s;
                default:
                    try
                    {
                        if (this.Row == null)
                            throw new Exception("@ Not initialized Row.");
                        return this.Row.GetValByKey(attrKey).ToString();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("@ Appear as abnormal values acquired during :" + ex.Message + "  " + attrKey + "  You do not have to add this attribute in class ,EnsName=" + this.ToString());
                    }
                    break;
            }
        }
        public string GetValStringByKey(string attrKey, string defVal)
        {
            string val = this.GetValStringByKey(attrKey);
            if (val == null || val == "")
                return defVal;
            else
                return val;
        }
        /// <summary>
        ///   Remove large pieces of text 
        /// </summary>
        /// <returns></returns>
        public string GetValDocText()
        {
            string s = this.GetValStrByKey("Doc");
            if (s.Trim().Length != 0)
                return s;

            s = SysDocFile.GetValTextV2(this.ToString(), this.PKVal.ToString());
            this.SetValByKey("Doc", s);
            return s;
        }
        public string GetValDocHtml()
        {
            string s = this.GetValHtmlStringByKey("Doc");
            if (s.Trim().Length != 0)
                return s;

            s = SysDocFile.GetValHtmlV2(this.ToString(), this.PKVal.ToString());
            this.SetValByKey("Doc", s);
            return s;
        }
        /// <summary>
        ///  Get to Html  Information .
        /// </summary>
        /// <param name="attrKey">attr</param>
        /// <returns>html.</returns>
        public string GetValHtmlStringByKey(string attrKey)
        {
            return DataType.ParseText2Html(this.GetValStringByKey(attrKey));
        }
        public string GetValHtmlStringByKey(string attrKey, string defval)
        {
            return DataType.ParseText2Html(this.GetValStringByKey(attrKey, defval));
        }
        /// <summary>
        ///  Enumerate or obtain foreign key labels 
        ///  If you get an enumeration enumeration tag .
        ///  If it is a foreign key to obtain the name of the foreign key .
        /// </summary>
        /// <param name="attrKey"></param>
        /// <returns></returns>
        public string GetValRefTextByKey(string attrKey)
        {
            string str = "";
            try
            {
                str = this.Row.GetValByKey(attrKey + "Text") as string;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + attrKey);
            }
            return str;
        }
        public Int64 GetValInt64ByKey(string key)
        {
            return Int64.Parse(this.GetValStringByKey(key));
        }
        public int GetValIntByKey(string key, int IsZeroAs)
        {
            int i = this.GetValIntByKey(key);
            if (i == 0)
                i = IsZeroAs;
            return i;
        }
        public int GetValIntByKey11(string key)
        {
            return int.Parse(this.GetValStrByKey(key));
        }
        /// <summary>
        ///  According to key  Get int val
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetValIntByKey(string key)
        {
            try
            {
                return int.Parse(this.GetValStrByKey(key));
            }
            catch (Exception ex)
            {
                //if (SystemConfig.IsDebug == false)
                //    throw new Exception("@[" + this.EnMap.GetAttrByKey(key).Desc + "] Please enter a number , You enter [" + this.GetValStrByKey(key) + "].");
                //else
                //    throw new Exception("@表[" + this.EnDesc + "] Obtaining property [" + key + "]值, Error , You can not [" + this.GetValStringByKey(key) + "] Converted to int Type . Error Messages :" + ex.Message + "@ Check to see if the store enumerated types , You are SetValbyKey There are no conversion . The correct approach is :this.SetValByKey( Key ,(int)value)  ");

                string v = this.GetValStrByKey(key).ToLower();
                if (v == "true")
                {
                    this.SetValByKey(key, 1);
                    return 1;
                }
                if (v == "false")
                {
                    this.SetValByKey(key, 0);
                    return 0;
                }

                if (key == "OID")
                {
                    this.SetValByKey("OID", 0);
                    return 0;
                }

                if (this.GetValStrByKey(key) == "")
                {
                    Attr attr = this.EnMap.GetAttrByKey(key);
                    if (attr.IsNull)
                        return 567567567;
                    else
                        return int.Parse(attr.DefaultVal.ToString());
                }

                //else
                //{
                //    return int.Parse(this.EnMap.GetAttrByKey(key).DefaultVal.ToString());
                //}

                if (SystemConfig.IsDebug == false)
                    throw new Exception("@[" + this.EnMap.GetAttrByKey(key).Desc + "] Please enter a number , You enter [" + this.GetValStrByKey(key) + "].");
                else
                    throw new Exception("@表[" + this.EnDesc + "] Obtaining property [" + key + "]值, Error , You can not [" + this.GetValStringByKey(key) + "] Converted to int Type . Error Messages :" + ex.Message + "@ Check to see if the store enumerated types , You are SetValbyKey There are no conversion . The correct approach is :this.SetValByKey( Key ,(int)value)  ");
            }
        }
        /// <summary>
        ///  According to key  Get  bool val
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool GetValBooleanByKey(string key)
        {
            string s = this.GetValStrByKey(key);
            if (string.IsNullOrEmpty(s))
                s = this.EnMap.GetAttrByKey(key).DefaultVal.ToString();

            if (s.ToUpper() == "FALSE")
                return false;
            if (s.ToUpper() == "TRUE")
                return true;

            if (int.Parse(s) == 0)
                return false;
            else
                return true;
        }

        public bool GetValBooleanByKey(string key, bool defval)
        {
            try
            {

                if (int.Parse(this.GetValStringByKey(key)) == 0)
                    return false;
                else
                    return true;
            }
            catch
            {
                return defval;
            }
        }
        public string GetValBoolStrByKey(string key)
        {
            if (int.Parse(this.GetValStringByKey(key)) == 0)
                return "否";
            else
                return "是";
        }
        /// <summary>
        ///  According to key  Get flaot val
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public float GetValFloatByKey(string key, int blNum)
        {
            return float.Parse(float.Parse(this.Row.GetValByKey(key).ToString()).ToString("0.00"));
        }
        /// <summary>
        ///  According to key  Get flaot val
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public float GetValFloatByKey(string key)
        {
            try
            {
                return float.Parse(float.Parse(this.Row.GetValByKey(key).ToString()).ToString("0.00"));
            }
            catch
            {
                if (this.GetValStringByKey(key) == "")
                {
                    Attr attr = this.EnMap.GetAttrByKey(key);
                    if (attr.IsNull)
                        return 567567567;
                    else
                        return float.Parse(attr.DefaultVal.ToString());
                }
                return 0;
            }
        }
        public decimal GetValMoneyByKey(string key)
        {
            try
            {
                return decimal.Parse(this.GetValDecimalByKey(key).ToString("0.00"));
            }
            catch
            {
                if (this.GetValStringByKey(key) == "")
                {
                    Attr attr = this.EnMap.GetAttrByKey(key);
                    if (attr.IsNull)
                        return 567567567;
                    else
                        return decimal.Parse(attr.DefaultVal.ToString());
                }
                return 0;
            }
        }
        /// <summary>
        ///  According to key  Get flaot val
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public decimal GetValDecimalByKey(string key)
        {
            return decimal.Round(decimal.Parse(this.GetValStrByKey(key)), 4);
        }
        public decimal GetValDecimalByKey(string key, string items)
        {
            if (items == "" || items == null)
                return 0;

            if (items.IndexOf("@" + key) == -1)
                return 0;

            string str = items.Substring(items.IndexOf("@" + key));

            return decimal.Round(decimal.Parse(this.GetValStringByKey(key)), 4);
        }
        public double GetValDoubleByKey(string key)
        {
            try
            {
                return double.Parse(this.GetValStrByKey(key));
            }
            catch (Exception ex)
            {
                throw new Exception("@表[" + this.EnDesc + "] Obtaining property [" + key + "]值, Error , You can not [" + this.GetValStringByKey(key) + "] Converted to double Type . Error Messages :" + ex.Message);
            }
        }
        public string GetValAppDateByKey(string key)
        {
            try
            {
                string str = this.GetValStringByKey(key);
                if (str == null || str == "")
                    return str;
                return DataType.StringToDateStr(str);
            }
            catch (System.Exception ex)
            {
                throw new Exception("@ Examples :[" + this.EnMap.EnDesc + "]   Property [" + key + "]值[" + this.GetValStringByKey(key).ToString() + "] Date format conversion error :" + ex.Message);
            }
            //return "2003-08-01";
        }
        #endregion

        #endregion

        #region  For configuration information 
        public string GetCfgValStr(string key)
        {
            return BP.Sys.EnsAppCfgs.GetValString(this.ToString() + "s", key);
        }

        public int GetCfgValInt(string key)
        {
            return BP.Sys.EnsAppCfgs.GetValInt(this.ToString() + "s", key);
        }

        public bool GetCfgValBoolen(string key)
        {
            return BP.Sys.EnsAppCfgs.GetValBoolen(this.ToString() + "s", key);
        }
        public void SetCfgVal(string key, object val)
        {
            BP.Sys.EnsAppCfg cfg = new EnsAppCfg();
            cfg.MyPK = this.ToString() + "s@" + key;
            cfg.CfgKey = key;
            cfg.CfgVal = val.ToString();
            cfg.EnsName = this.ToString() + "s";
            cfg.Save();
        }
        #endregion

        #region  Property 
        /// <summary>
        ///  File Managers 
        /// </summary>
        public SysFileManagers HisSysFileManagers
        {
            get
            {
                return new SysFileManagers(this.ToString(), this.PKVal.ToString());
            }
        }
        public bool IsBlank
        {
            get
            {
                if (this._row == null)
                    return true;

                Attrs attrs = this.EnMap.Attrs;
                foreach (Attr attr in attrs)
                {

                    if (attr.UIIsReadonly && attr.IsFKorEnum == false)
                        continue;

                    //if (attr.IsFK && string.IsNullOrEmpty(attr.DefaultVal.ToString()) ==true)
                    //    continue; /* If it is a foreign key , And the default value of the foreign key null.*/

                    string str = this.GetValStrByKey(attr.Key);
                    if (str == "" || str == attr.DefaultVal.ToString() || str == null)
                        continue;

                    if (attr.MyDataType == DataType.AppDate && attr.DefaultVal == null)
                    {
                        if (str == DataType.CurrentData)
                            continue;
                        else
                            return true;
                    }

                    if (str == attr.DefaultVal.ToString() && attr.IsFK == false)
                        continue;

                    if (attr.IsEnum)
                    {
                        if (attr.DefaultVal.ToString() == str)
                            continue;
                        else
                            return false;
                        continue;
                    }

                    if (attr.IsNum)
                    {
                        if (decimal.Parse(str) != decimal.Parse(attr.DefaultVal.ToString()))
                            return false;
                        else
                            continue;
                    }

                    if (attr.IsFKorEnum)
                    {
                        //if (attr.DefaultVal == null || attr.DefaultVal == "")
                        //    continue;

                        if (attr.DefaultVal.ToString() != str)
                            return false;
                        else
                            continue;
                    }

                    if (str != attr.DefaultVal.ToString())
                        return false;
                }
                return true;
            }
        }
        /// <summary>
        ///  Gets or sets 
        ///  Is not empty entity .
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                if (this._row == null)
                {
                    return true;
                }
                else
                {
                    if (this.PKVal == null || this.PKVal.ToString() == "0" || this.PKVal.ToString() == "")
                        return true;
                    return false;
                }
            }
            set
            {
                this._row = null;
            }
        }
        /// <summary>
        ///  Description of this entity 
        /// </summary>
        public String EnDesc
        {
            get
            {
                return this.EnMap.EnDesc;
            }
        }
        /// <summary>
        ///  Take the primary key value . If it not the only primary health , The first value is returned .
        ///  Gets or sets 
        /// </summary>
        public Object PKVal
        {
            get
            {
                return this.GetValByKey(this.PK);
            }
            set
            {
                this.SetValByKey(this.PK, value);
            }
        }
        /// <summary>
        ///  If there is only one primary key , Returns PK, If there is more to return the first one .PK
        /// </summary>
        public int PKCount
        {
            get
            {
                switch (this.PK)
                {
                    case "OID":
                    case "No":
                    case "MyPK":
                        return 1;
                    default:
                        break;
                }

                int i = 0;
                foreach (Attr attr in this.EnMap.Attrs)
                {
                    if (attr.MyFieldType == FieldType.PK || attr.MyFieldType == FieldType.PKEnum || attr.MyFieldType == FieldType.PKFK)
                        i++;
                }
                if (i == 0)
                    throw new Exception("@ Did not give 【" + this.EnDesc + "," + this.EnMap.PhysicsTable + "】 Define a primary key .");
                else
                    return i;
            }
        }
        /// <summary>
        ///  Is not it OIDEntity
        /// </summary>
        public bool IsOIDEntity
        {
            get
            {
                if (this.PK == "OID")
                    return true;
                return false;
            }
        }
        /// <summary>
        ///  Is not it OIDEntity
        /// </summary>
        public bool IsNoEntity
        {
            get
            {
                if (this.PK == "No")
                    return true;
                return false;
            }
        }
        /// <summary>
        ///  Is TreeEntity
        /// </summary>
        public bool IsTreeEntity
        {
            get
            {
                if (this.PK == "ID")
                    return true;
                return false;
            }
        }
        /// <summary>
        ///  Is not it IsMIDEntity
        /// </summary>
        public bool IsMIDEntity
        {
            get
            {
                if (this.PK == "MID")
                    return true;
                return false;
            }
        }
        /// <summary>
        ///  If there is only one primary key , Returns PK, If there is more to return the first one .PK
        /// </summary>
        public virtual string PK
        {
            get
            {
                string pks = "";
                foreach (Attr attr in this.EnMap.Attrs)
                {
                    if (attr.MyFieldType == FieldType.PK
                        || attr.MyFieldType == FieldType.PKEnum || attr.MyFieldType == FieldType.PKFK)
                        pks += attr.Key + ",";
                }
                if (pks == "")
                    throw new Exception("@ Did not give 【" + this.EnDesc + "," + this.EnMap.PhysicsTable + "】 Define a primary key .");
                pks = pks.Substring(0, pks.Length - 1);
                return pks;
            }
        }
        public virtual string PKField
        {
            get
            {
                foreach (Attr attr in this.EnMap.Attrs)
                {
                    if (attr.MyFieldType == FieldType.PK
                        || attr.MyFieldType == FieldType.PKEnum
                        || attr.MyFieldType == FieldType.PKFK)
                        return attr.Field;
                }
                throw new Exception("@ Did not give 【" + this.EnDesc + "】 Define a primary key .");
            }
        }
        /// <summary>
        ///  If there is only one primary key , Returns PK, If there is more to return the first one .PK
        /// </summary>
        public string[] PKs
        {
            get
            {
                string[] strs1 = new string[this.PKCount];
                int i = 0;
                foreach (Attr attr in this.EnMap.Attrs)
                {
                    if (attr.MyFieldType == FieldType.PK || attr.MyFieldType == FieldType.PKEnum || attr.MyFieldType == FieldType.PKFK)
                    {
                        strs1[i] = attr.Key;
                        i++;
                    }
                }
                return strs1;
            }
        }
        /// <summary>
        ///  Take the primary key value .
        /// </summary>
        public Hashtable PKVals
        {
            get
            {
                Hashtable ht = new Hashtable();
                string[] strs = this.PKs;
                foreach (string str in strs)
                {
                    ht.Add(str, this.GetValStringByKey(str));
                }
                return ht;
            }
        }
        #endregion

        public void domens()
        {
        }

    }

}
