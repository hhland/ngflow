using System;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.Pub
{
	/// <summary>
	///  Years 
	/// </summary>
	public class NY :SimpleNoNameFix
	{
		#region  Achieve the basic square method 
		/// <summary>
		///  Physical table 
		/// </summary>
		public override string  PhysicsTable
		{
			get
			{
				return "Pub_NY";
			}
		}
		/// <summary>
		///  Description 
		/// </summary>
		public override string  Desc
		{
			get
			{
                return  " Years ";// " Years ";
			}
		}
		#endregion 

		#region  Constructor 
		 
		public NY(){}
		 
		public NY(string _No ): base(_No){}
		
		#endregion 
	}
	/// <summary>
	/// NDs
	/// </summary>
	public class NYs :SimpleNoNameFixs
	{
		/// <summary>
		///  Date set 
		/// </summary>
		public NYs()
		{
		}
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new NY();
			}
		}
	}
}
