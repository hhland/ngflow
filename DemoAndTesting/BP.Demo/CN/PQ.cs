using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Port;

namespace BP.CN
{
	/// <summary>
	///  Area 
	/// </summary>
	public class PQAttr: EntityNoNameAttr
	{
		#region  Basic properties 
		public const string FK_SF="FK_SF";
		#endregion
	}
	/// <summary>
    ///  Area 
	/// </summary>
	public class PQ :EntityNoName
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
		///  Area 
		/// </summary>		
		public PQ(){}
		public PQ(string no):base(no)
		{
		}

		
		/// <summary>
		/// Map
		/// </summary>
		public override Map EnMap
		{
			get
			{
				if (this._enMap!=null) 
					return this._enMap;
				Map map = new Map();

				#region  Basic properties  
				map.EnDBUrl =new DBUrl(DBUrlType.AppCenterDSN) ; 
				map.PhysicsTable="CN_PQ";  
				map.AdjunctType = AdjunctType.AllType ;  
				map.DepositaryOfMap=Depositary.Application; 
				map.DepositaryOfEntity=Depositary.None; 
				map.IsCheckNoLength=false;
				map.EnDesc=" Area ";
				map.EnType=EnType.App;
				map.CodeStruct="4";
				#endregion

				#region  Field  
				map.AddTBStringPK(PQAttr.No,null," Serial number ",true,false,0,50,50);
				map.AddTBString(PQAttr.Name,null," Name ",true,false,0,50,200);
				#endregion

				this._enMap=map;
				return this._enMap;
			}
		}
        public override Entities GetNewEntities
        {
            get { return new PQs(); }
        }
		#endregion
		 
	}
	/// <summary>
	///  Area 
	/// </summary>
	public class PQs : EntitiesNoName
	{
		#region 
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new PQ();
			}
		}	
		#endregion 

		#region  Constructor 
		/// <summary>
		///  Area s
		/// </summary>
		public PQs(){}
		#endregion
	}
	
}
