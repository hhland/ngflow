using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP;
namespace BP.Sys
{
	/// <summary>
	///   Configuration Information 
	/// </summary>
    public class EnCfgAttr : EntityNoAttr
    {
        /// <summary>
        ///  Packet Label 
        /// </summary>
        public const string GroupTitle = "GroupTitle";
        /// <summary>
        ///  Accessories path 
        /// </summary>
        public const string FJSavePath = "FJSavePath";
        /// <summary>
        ///  Accessories path 
        /// </summary>
        public const string FJWebPath = "FJWebPath";
        /// <summary>
        ///  Data analysis methods 
        /// </summary>
        public const string Datan = "Datan";
    }
	/// <summary>
	/// EnCfgs
	/// </summary>
    public class EnCfg : EntityNo
    {
        #region  Basic properties 
        /// <summary>
        ///  Data analysis methods 
        /// </summary>
        public string Datan
        {
            get
            {
                return this.GetValStringByKey(EnCfgAttr.Datan);
            }
            set
            {
                this.SetValByKey(EnCfgAttr.Datan, value);
            }
        }
        /// <summary>
        ///  Data Sources 
        /// </summary>
        public string GroupTitle
        {
            get
            {
                return this.GetValStringByKey(EnCfgAttr.GroupTitle);
            }
            set
            {
                this.SetValByKey(EnCfgAttr.GroupTitle, value);
            }
        }
        /// <summary>
        ///  Accessories path 
        /// </summary>
        public string FJSavePath
        {
            get
            {
                string str = this.GetValStringByKey(EnCfgAttr.FJSavePath);
                if (str == "" || str == null || str==string.Empty)
                    return BP.Sys.SystemConfig.PathOfDataUser + this.No + "\\";
                return str;
            }
            set
            {
                this.SetValByKey(EnCfgAttr.FJSavePath, value);
            }
        }
        public string FJWebPath
        {
            get
            {
                string str = this.GetValStringByKey(EnCfgAttr.FJWebPath);
                if (str == "" || str == null)
                    return BP.Sys.Glo.Request.ApplicationPath +"/DataUser/" + this.No + "/";
                return str;
            }
            set
            {
                this.SetValByKey(EnCfgAttr.FJWebPath, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  System entities 
        /// </summary>
        public EnCfg()
        {
        }
        /// <summary>
        ///  System entities 
        /// </summary>
        /// <param name="no"></param>
        public EnCfg(string enName)
        {
            this.No = enName;
            try
            {
                this.Retrieve();
            }
            catch (Exception ex)
            {
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
                Map map = new Map("Sys_EnCfg");
                map.DepositaryOfEntity = Depositary.Application;
                map.DepositaryOfMap = Depositary.Application;
                map.EnDesc = " Entity Configuration ";
                map.EnType = EnType.Sys;

                map.AddTBStringPK(EnCfgAttr.No, null, " Entity Name ", true, false, 1, 100, 60);
                map.AddTBString(EnCfgAttr.GroupTitle, null, " Packet Label ", true, false, 0, 2000, 60);
                map.AddTBString(EnCfgAttr.FJSavePath, null, " Save Path ", true, false, 0, 100, 60);
                map.AddTBString(EnCfgAttr.FJWebPath, null, " Accessory Web Path ", true, false, 0, 100, 60);
                map.AddTBString(EnCfgAttr.Datan, null, " Field data analysis methods ", true, false, 0, 200, 60);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
	/// <summary>
	///  Entity set 
	/// </summary>
    public class EnCfgs : EntitiesNoName
    {
        #region  Structure 
        /// <summary>
        ///  Configuration Information 
        /// </summary>
        public EnCfgs()
        {
        }
        /// <summary>
        ///  Get it  Entity
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new EnCfg();
            }
        }
        #endregion
    }
}
