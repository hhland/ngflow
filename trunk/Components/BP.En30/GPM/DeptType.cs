using System;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.GPM
{
    /// <summary>
    ///  Sector Type 
    /// </summary>
    public class DeptTypeAttr : EntityNoNameAttr
    {
    }
	/// <summary>
    ///   Sector Type 
	/// </summary>
	public class DeptType :EntityNoName
    {
        #region  Property 
        #endregion
     
		#region  Constructor 
		/// <summary>
		///  Sector Type 
		/// </summary>
		public DeptType()
        {
        }
        /// <summary>
        ///  Sector Type 
        /// </summary>
        /// <param name="_No"></param>
        public DeptType(string _No) : base(_No) { }
		#endregion 

		/// <summary>
		///  Sector Type Map
		/// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;
                Map map = new Map("Port_DeptType");
                map.EnDesc = " Sector Type ";
                map.CodeStruct = "2";

                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;

                map.AddTBStringPK(DeptTypeAttr.No, null, " Serial number ", true, true, 2, 2, 2);
                map.AddTBString(DeptTypeAttr.Name, null, " Name ", true, false, 1, 50, 20);
                this._enMap = map;
                return this._enMap;
            }
        }
	}
	/// <summary>
    ///  Sector Type 
	/// </summary>
    public class DeptTypes : EntitiesNoName
	{
		/// <summary>
		///  Sector Type s
		/// </summary>
        public DeptTypes() { }
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
                return new DeptType();
			}
		}
	}
}
