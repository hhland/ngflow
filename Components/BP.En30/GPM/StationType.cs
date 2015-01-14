using System;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.GPM
{
    /// <summary>
    ///  Post Type 
    /// </summary>
    public class StationTypeAttr : EntityNoNameAttr
    {
    }
	/// <summary>
    ///   Post Type 
	/// </summary>
	public class StationType :EntityNoName
    {
        #region  Property 
        public string FK_StationType
        {
            get
            {
                return this.GetValStrByKey(StationAttr.FK_StationType);
            }
            set
            {
                this.SetValByKey(StationAttr.FK_StationType, value);
            }
        }

        public string FK_StationTypeText
        {
            get
            {
                return this.GetValRefTextByKey(StationAttr.FK_StationType);
            }
        }

        #endregion
     
		#region  Constructor 
		/// <summary>
		///  Post Type 
		/// </summary>
		public StationType()
        {
        }
        /// <summary>
        ///  Post Type 
        /// </summary>
        /// <param name="_No"></param>
        public StationType(string _No) : base(_No) { }
		#endregion 

		/// <summary>
		///  Post Type Map
		/// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;
                Map map = new Map("Port_StationType");
                map.EnDesc = " Post Type ";
                map.CodeStruct = "2";

                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;

                map.AddTBStringPK(StationTypeAttr.No, null, " Serial number ", true, true, 2, 2, 2);
                map.AddTBString(StationTypeAttr.Name, null, " Name ", true, false, 1, 50, 20);
                this._enMap = map;
                return this._enMap;
            }
        }
	}
	/// <summary>
    ///  Post Type 
	/// </summary>
    public class StationTypes : EntitiesNoName
	{
		/// <summary>
		///  Post Type s
		/// </summary>
        public StationTypes() { }
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
                return new StationType();
			}
		}
	}
}
