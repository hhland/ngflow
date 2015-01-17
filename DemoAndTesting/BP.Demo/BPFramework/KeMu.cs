using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Port;

namespace BP.Demo
{
	/// <summary>
	///  Subject   Property 
	/// </summary>
	public class KeMuAttr: EntityNoNameAttr
	{
	}
	/// <summary>
    ///  Subject 
	/// </summary>
	public class KeMu :EntityNoName
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
		///  Subject 
		/// </summary>		
		public KeMu(){}
        /// <summary>
        ///  Subject 
        /// </summary>
        /// <param name="no"> Serial number </param>
		public KeMu(string no):base(no)
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
				map.PhysicsTable="Demo_KeMu";   //表
				map.DepositaryOfEntity=Depositary.None;  // Entity village put position .
                map.IsAllowRepeatName = true;
				map.IsCheckNoLength=false;
				map.EnDesc=" Subject ";
				map.EnType=EnType.App;
				map.CodeStruct="3"; // Let numbered 3位, 从001 到 999 .
				#endregion

				#region  Field  
                map.AddTBStringPK(KeMuAttr.No, null, " Serial number ", true, true, 3, 3, 3);
				map.AddTBString(KeMuAttr.Name,null," Name ",true,false,0,50,200);
				#endregion

				this._enMap=map;
				return this._enMap;
			}
		}
        public override Entities GetNewEntities
        {
            get { return new KeMus(); }
        }
		#endregion
		 
	}
	/// <summary>
	///  Subject 
	/// </summary>
	public class KeMus : EntitiesNoName
	{
		#region  Rewrite 
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new KeMu();
			}
		}	
		#endregion 

		#region  Constructor 
		/// <summary>
		///  Subject s
		/// </summary>
		public KeMus(){}
		#endregion
	}
	
}
