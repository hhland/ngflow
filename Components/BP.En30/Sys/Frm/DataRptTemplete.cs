using System;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.Sys
{
    /// <summary>
    ///  Report data is stored templates 
    /// </summary>
    public class DataRptAttr : EntityMyPKAttr
    {
        public const string RefOID = "RefOID";
        public const string ColCount = "ColCount";
        public const string RowCount = "RowCount";
        public const string Val = "Val";
    }
	/// <summary>
    ///   Report data is stored templates   
	/// </summary>
    public class DataRpt : EntityMyPK
    {
        #region  Constructor 
        /// <summary>
        ///  Report data is stored templates 
        /// </summary>
        public DataRpt()
        {
        }
        #endregion

        /// <summary>
        ///  Report data is stored templates 
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;
                Map map = new Map("Sys_DataRpt");
                map.EnDesc = " Report data is stored templates ";
                map.DepositaryOfMap = Depositary.Application;

                map.AddMyPK();
                map.AddTBString(DataRptAttr.ColCount, null, "ÁÐ", true, true, 0, 200, 20);
                map.AddTBString(DataRptAttr.RowCount, null, "ÐÐ", true, true, 0, 200, 20);
                map.AddTBDecimal(DataRptAttr.Val, 0, "Öµ", true, false);
                map.AddTBDecimal(DataRptAttr.RefOID, 0, " Value associated ", true, false);

                this._enMap = map;
                return this._enMap;
            }
        }
    }
	/// <summary>
    /// DataRpt Data Storage 
	/// </summary>
    public class DataRpts : SimpleNoNames
    {
        /// <summary>
        /// DataRpt Data Storage s
        /// </summary>
        public DataRpts() 
        {
        }
        /// <summary>
        /// DataRpt Data Storage  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new DataRpt();
            }
        }
    }
}
