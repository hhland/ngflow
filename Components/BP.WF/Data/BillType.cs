using System;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.WF.Data
{
	/// <summary>
    ///   Document Type 
	/// </summary>
    public class BillType : EntityNoName
    {
        #region  Property .
        /// <summary>
        ///  Process ID 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.GetValStrByKey("FK_Flow");
            }
            set
            {
                this.SetValByKey("FK_Flow", value);
            }
        }
        /// <summary>
        /// UI Access control interface 
        /// </summary>
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                uac.OpenForSysAdmin();
                return uac;
            }
        }
        #endregion  Property .

        #region  Constructor 
        /// <summary>
        ///  Document Type 
        /// </summary>
        public BillType()
        {
        }
        /// <summary>
        ///  Document Type 
        /// </summary>
        /// <param name="_No"></param>
        public BillType(string _No) : base(_No) { }
        /// <summary>
        ///  Document Type Map
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;
                Map map = new Map("WF_BillType");
                map.EnDesc = " Document Type ";
                map.CodeStruct = "2";
                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.IsAutoGenerNo = true;

                map.AddTBStringPK(SimpleNoNameAttr.No, null, " Serial number ", true, true, 2, 2, 2);
                map.AddTBString(SimpleNoNameAttr.Name, null, " Name ", true, false, 1, 50, 50);
                map.AddTBString("FK_Flow", null, " Process ", true, false, 1, 50, 50);

                map.AddTBInt("IDX", 0, "IDX", false, false);
                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
	/// <summary>
    ///  Document Type 
	/// </summary>
    public class BillTypes : SimpleNoNames
    {
        /// <summary>
        ///  Document Type s
        /// </summary>
        public BillTypes() { }
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new BillType();
            }
        }
    }
}
