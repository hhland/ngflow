using System;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.GPM
{
    /// <summary>
    ///  Position 
    /// </summary>
    public class DutyAttr : EntityNoNameAttr
    {
    }
	/// <summary>
    ///   Position 
	/// </summary>
	public class Duty :EntityNoName
    {
        #region  Property 
        #endregion
     
		#region  Constructor 
		/// <summary>
		///  Position 
		/// </summary>
		public Duty()
        {
        }
        /// <summary>
        ///  Position 
        /// </summary>
        /// <param name="_No"></param>
        public Duty(string _No) : base(_No) { }
		#endregion 

		/// <summary>
		///  Position Map
		/// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;
                Map map = new Map("Port_Duty");
                map.EnDesc = " Position ";
                map.CodeStruct = "2";

                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;

                map.AddTBStringPK(DutyAttr.No, null, " Serial number ", true, true, 2, 2, 2);
                map.AddTBString(DutyAttr.Name, null, " Name ", true, false, 1, 50, 20);
                this._enMap = map;
                return this._enMap;
            }
        }
	}
	/// <summary>
    ///  Position 
	/// </summary>
    public class Dutys : EntitiesNoName
	{
		/// <summary>
		///  Position s
		/// </summary>
        public Dutys() { }
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
                return new Duty();
			}
		}
	}
}
