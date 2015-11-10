using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Port;

namespace BP.Demo
{
	/// <summary>
	///  Class   Property 
	/// </summary>
	public class BanJiAttr: EntityNoNameAttr
	{
        /// <summary>
        ///  Head teacher 
        /// </summary>
        public const string BZR = "BZR";
	}
	/// <summary>
    ///  Class 
	/// </summary>
	public class BanJi :EntityNoName
	{	
		#region  Basic properties 
        /// <summary>
        ///  Head teacher 
        /// </summary>
        public string BZR
        {
            get
            {
                return this.GetValStrByKey(BanJiAttr.BZR);
            }
            set
            {
                this.SetValByKey(BanJiAttr.BZR, value);
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
		///  Class 
		/// </summary>		
		public BanJi(){}
		public BanJi(string no):base(no)
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
				map.PhysicsTable="Demo_BanJi";   //表
				map.DepositaryOfEntity=Depositary.None;  // Entity village put position .
                map.IsAllowRepeatName = true;
				map.EnDesc=" Class ";
				map.EnType=EnType.App;
				map.CodeStruct="3"; // Let numbered 3位, 从001 到 999 .
				#endregion

				#region  Field  
                map.AddTBStringPK(BanJiAttr.No, null, " Serial number ", true, true, 3, 3, 3);
				map.AddTBString(BanJiAttr.Name,null," Name ",true,false,0,50,200);
                map.AddTBString(BanJiAttr.BZR, null, " Head teacher ", true, false, 0, 200, 200);

				#endregion

				this._enMap=map;
				return this._enMap;
			}
		}
        public override Entities GetNewEntities
        {
            get { return new BanJis(); }
        }
		#endregion
		 
	}
	/// <summary>
	///  Class s
	/// </summary>
	public class BanJis : EntitiesNoName
	{
		#region  Rewrite 
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new BanJi();
			}
		}	
		#endregion 

		#region  Constructor 
		/// <summary>
		///  Class s
		/// </summary>
		public BanJis(){}
		#endregion
	}
	
}
