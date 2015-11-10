using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Port;



namespace BP.CN
{
	/// <summary>
	///  Island cities 
	/// </summary>
	public class ZDSAttr: EntityNoNameAttr
	{
		#region  Basic properties 
		public const  string FK_PQ="FK_PQ";
        public const string FK_ZDS = "FK_ZDS";
        public const string FK_SF = "FK_SF";
        public const string NameS = "NameS";


		#endregion
	}
	/// <summary>
    ///  Island cities 
	/// </summary>
	public class ZDS :EntityNoName
	{	
		#region  Basic properties 
        public string NameS
        {
            get
            {
                return this.GetValStrByKey(ZDSAttr.NameS);
            }
        }
		 
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
		///  Island cities 
		/// </summary>		
		public ZDS(){}
		public ZDS(string no):base(no)
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
                map.PhysicsTable = "CN_ZDS";
                map.AdjunctType = AdjunctType.AllType;
                map.DepositaryOfMap = Depositary.Application;
                map.DepositaryOfEntity = Depositary.None;
                map.IsCheckNoLength = false;
                map.EnDesc = " Island cities ";
                map.EnType = EnType.App;
                map.CodeStruct = "4";
                #endregion

                #region  Field 
                map.AddTBStringPK(ZDSAttr.No, null, " Serial number ", true, false, 0, 200, 50);
                map.AddTBString(ZDSAttr.Name, null, " Name ", true, false, 0, 200, 200);
                map.AddTBString(ZDSAttr.NameS, null, " Name s", true, false, 0, 200, 200);


                map.AddTBString(ZDSAttr.FK_SF, null, "FK_SF", true, false, 0, 200, 200);

                map.AddDDLEntities(ZDSAttr.FK_PQ, null, " Area ", new PQs(), true);
              //  map.AddDDLEntities(ZDSAttr.FK_ZDS, null, " Province ", new SFs(), true);
                #endregion

                this._enMap = map;
                return this._enMap;
            }
		}
		#endregion
		 
	}
	/// <summary>
	///  Island cities 
	/// </summary>
	public class ZDSs : EntitiesNoName
	{
		#region 
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new ZDS();
			}
		}	
		#endregion 

		#region  Constructor 
		/// <summary>
		///  Island cities s
		/// </summary>
		public ZDSs(){}

        public ZDSs(string sf)
        {
            this.Retrieve(ZDSAttr.FK_SF, sf);
        }

		#endregion
	}
	
}
