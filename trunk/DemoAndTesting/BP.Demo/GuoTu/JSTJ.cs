using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Port;

namespace BP.Demo
{
	/// <summary>
	///  Product 
	/// </summary>
	public class JSTJAttr: EntityNoNameAttr
	{
		#region  Basic properties 
		public const  string FK_SF="FK_SF";
        public const string Addr = "Addr";

		#endregion
	}
	/// <summary>
    ///  Product 
	/// </summary>
    public class JSTJ : EntityNoName
    {
        #region  Basic properties 

        #endregion

        #region  Constructor 
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                uac.OpenForSysAdmin();
                return uac;
            }
        }
        /// <summary>
        ///  Product 
        /// </summary>		
        public JSTJ() { }
        /// <summary>
        ///  Product 
        /// </summary>
        /// <param name="no"></param>
        public JSTJ(string no)
            : base(no)
        {

        }
        /// <summary>
        /// Map
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;
                Map map = new Map();

                #region  Basic properties 
                map.EnDBUrl = new DBUrl(DBUrlType.AppCenterDSN);
                map.PhysicsTable = "Demo_JSTJ";
                map.AdjunctType = AdjunctType.AllType;
                map.DepositaryOfMap = Depositary.Application;
                map.DepositaryOfEntity = Depositary.None;
                map.IsCheckNoLength = false;
                map.EnDesc = " Product ";
                map.EnType = EnType.App;
                map.CodeStruct = "4";
                #endregion

                #region  Field 
                map.AddTBStringPK(JSTJAttr.No, null, " Serial number ", true, false, 0, 200, 50);
                map.AddTBString(JSTJAttr.Name, null, " Name ", true, false, 0, 200, 200);

                #endregion

                this._enMap = map;
                return this._enMap;
            }
        }
        public override Entities GetNewEntities
        {
            get { return new JSTJs(); }
        }
        #endregion
    }
	/// <summary>
	///  Product 
	/// </summary>
	public class JSTJs : EntitiesNoName
	{
		#region 
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new JSTJ();
			}
		}	
		#endregion 

		#region  Constructor 
		/// <summary>
		///  Product s
		/// </summary>
		public JSTJs(){}
		#endregion
	}
	
}
