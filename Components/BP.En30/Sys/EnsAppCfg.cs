using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.En;
using BP;
namespace BP.Sys
{
	/// <summary>
	///   Application Configuration 
	/// </summary>
    public class EnsAppCfgAttr : EntityNoAttr
    {
        /// <summary>
        ///  Packet Label 
        /// </summary>
        public const string EnsName = "EnsName";
        /// <summary>
        ///  Configuration key 
        /// </summary>
        public const string CfgKey = "CfgKey";
        /// <summary>
        /// 值
        /// </summary>
        public const string CfgVal = "CfgVal";
    }
	/// <summary>
    ///  Application Configuration 
	/// </summary>
    public class EnsAppCfg : EntityMyPK
    {
        #region  Basic properties 
        /// <summary>
        ///  Configuration Label 
        /// </summary>
        public string CfgVal
        {
            get
            {
                string val= this.GetValStrByKey(EnsAppCfgAttr.CfgVal);
                if (val == null || val == "")
                    return null;
                return val;
            }
            set
            {
                this.SetValByKey(EnsAppCfgAttr.CfgVal, value);
            }
        }
        /// <summary>
        /// Int 值
        /// </summary>
        public int CfgValOfInt
        {
            get
            {
                return this.GetValIntByKey(EnsAppCfgAttr.CfgVal);
            }
        }
        /// <summary>
        /// Boolen 值
        /// </summary>
        public bool CfgValOfBoolen
        {
            get
            {
                return this.GetValBooleanByKey(EnsAppCfgAttr.CfgVal);
            }
        }
        /// <summary>
        ///  Data Sources 
        /// </summary>
        public string EnsName
        {
            get
            {
                return this.GetValStringByKey(EnsAppCfgAttr.EnsName);
            }
            set
            {
                this.SetValByKey(EnsAppCfgAttr.EnsName, value);
            }
        }
        /// <summary>
        ///  Accessories path 
        /// </summary>
        public string CfgKey
        {
            get
            {
                return this.GetValStringByKey(EnsAppCfgAttr.CfgKey);
            }
            set
            {
                this.SetValByKey(EnsAppCfgAttr.CfgKey, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  System entities 
        /// </summary>
        public EnsAppCfg()
        {
        }
        /// <summary>
        ///  System entities 
        /// </summary>
        /// <param name="no"></param>
        public EnsAppCfg(string pk)
        {
            this.MyPK = pk;
            int i = this.RetrieveFromDBSources();
            if (i == 0)
            {
                //BP.Sys.Xml.EnsAppXml xml = new BP.Sys.Xml.EnsAppXml();
            }
        }
        public EnsAppCfg(string ensName, string cfgkey)
        {
            this.MyPK = ensName + "@" + cfgkey;
            try
            {
                this.Retrieve();
            }
            catch
            {
                BP.Sys.Xml.EnsAppXmls xmls = new BP.Sys.Xml.EnsAppXmls();
                int i = xmls.Retrieve(BP.Sys.Xml.EnsAppXmlEnsName.EnsName, ensName,
                      "No", cfgkey);
                if (i == 0)
                {
                    Attrs attrs = this.EnMap.HisCfgAttrs;
                    foreach (Attr attr in attrs)
                    {
                        if (attr.Key == cfgkey)
                        {
                            this.EnsName = ensName;
                            this.CfgKey = cfgkey;
                            if (attr.Key == "FocusField")
                            {
                                Entity en = BP.En.ClassFactory.GetEns(ensName).GetNewEntity;
                                if (en.EnMap.Attrs.Contains("Name"))
                                    this.CfgVal = "Name";
                                if (en.EnMap.Attrs.Contains("Title"))
                                    this.CfgVal = "Title";
                            }
                            else
                            {
                                this.CfgVal = attr.DefaultVal.ToString();
                            }
                            this.Insert();
                            return;
                        }
                    }
                }
                BP.Sys.Xml.EnsAppXml xml = xmls[0] as BP.Sys.Xml.EnsAppXml;
                this.EnsName = ensName;
                this.CfgKey = cfgkey;
                this.CfgVal = xml.DefVal;
                this.Insert();
            }
        }
        /// <summary>
        /// map
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;
                Map map = new Map("Sys_EnsAppCfg");
                map.DepositaryOfEntity = Depositary.Application;
                map.DepositaryOfMap = Depositary.Application;
                map.EnDesc = " Entity set configuration ";
                map.EnType = EnType.Sys;

                map.AddMyPK();
                map.AddTBString(EnsAppCfgAttr.EnsName, null, " Entity set ", true, false, 0, 100, 60);
                map.AddTBString(EnsAppCfgAttr.CfgKey, null, "键", true, false, 0, 100, 60);
                map.AddTBString(EnsAppCfgAttr.CfgVal, null, "值", true, false, 0, 200, 60);


                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
	/// <summary>
    ///  Application Configuration 
	/// </summary>
    public class EnsAppCfgs : Entities
    {

        #region  Get Data 
        public static string GetValString(string ensName, string cfgKey)
        {
            EnsAppCfg cfg = new EnsAppCfg(ensName,cfgKey);
            return  cfg.CfgVal;
        }
        public static int GetValInt(string ensName, string cfgKey)
        {
            try
            {
                EnsAppCfg cfg = new EnsAppCfg(ensName, cfgKey);
                return cfg.CfgValOfInt;
            }
            catch
            {
                return 400;
            }
        }
        public static bool GetValBoolen(string ensName, string cfgKey)
        {
            EnsAppCfg cfg = new EnsAppCfg(ensName, cfgKey);
            return cfg.CfgValOfBoolen;
        }
        #endregion  Get Data 

        #region  Structure 
        /// <summary>
        ///  Application Configuration 
        /// </summary>
        public EnsAppCfgs()
        {

        }
        /// <summary>
        ///  Get it  Entity
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new EnsAppCfg();
            }
        }
        #endregion
    }
}
