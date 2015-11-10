using System;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.En
{
	/// <summary>
	///  Property 
	/// </summary>
	public class SimpleNoNameFixAttr : EntityNoNameAttr
	{}
	
	abstract public class SimpleNoNameFix : EntityNoName
	{		 
		#region  Structure 
		public override UAC HisUAC
		{
			get
			{
				UAC uac = new UAC();
				uac.OpenForSysAdmin();
				return uac;
			}
		}

		public SimpleNoNameFix()
		{
		}
		public SimpleNoNameFix(string _No) : base(_No){}
		public override Map EnMap
		{
			get
			{
				if (this._enMap!=null) return this._enMap;
				Map map = new Map(this.PhysicsTable);
				map.EnDesc=this.Desc;
				map.CodeStruct ="2" ;

				map.IsAutoGenerNo=true;

				map.DepositaryOfEntity=Depositary.Application;
				map.DepositaryOfMap=Depositary.Application;
				map.EnType=EnType.App;

				map.CodeStruct="3";
				map.IsAutoGenerNo=true;
				
 				map.AddTBStringPK(SimpleNoNameFixAttr.No,null,null,true,true,1,30,3);
                map.AddTBString(SimpleNoNameFixAttr.Name,null,null,true,false,1,60,500);
				this._enMap=map;
				return this._enMap;
			}
		}		 
		#endregion 		

		#region  Method requires subclasses override 
		/// <summary>
		///  The specified table 
		/// </summary>
		public abstract string PhysicsTable{get;}
		/// <summary>
		///  Description 
		/// </summary>
		public abstract string Desc{get;}
		#endregion 

		#region   Override the base class methods .
		#endregion
	}
	/// <summary>
	/// SimpleNoNameFixs
	/// </summary>
	abstract public class SimpleNoNameFixs : EntitiesNoName
	{
		/// <summary>
		/// SimpleNoNameFixs
		/// </summary>
		public SimpleNoNameFixs()
		{
		}


	}
}
