using System;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.Pub
{
	/// <summary>
	///  Year 
	/// </summary>
	public class ND :SimpleNoNameFix
	{
		#region  Achieve the basic square method 
		/// <summary>
		///  Physical table 
		/// </summary>
		public override string  PhysicsTable
		{
			get
			{
				return "Pub_ND";
			}
		}
		/// <summary>
		///  Description 
		/// </summary>
		public override string  Desc
		{
			get
			{
                return " Year ";// " Year ";
			}
		}
		#endregion 

		#region  Constructor 
		public ND()
		{
		}
		public ND(string _No ):base(_No)
		{
		}
		#endregion 
	}
	/// <summary>
	/// NDs
	/// </summary>
	public class NDs :SimpleNoNameFixs
	{
        protected override void OnClear()
        {
            if (1 == 1)
            {

            }
            base.OnClear();
        }
		/// <summary>
		///  Annual collection 
		/// </summary>
		public NDs()
		{
		}
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new ND();
			}
		}
        public override int RetrieveAll()
        {
            return base.RetrieveAll();
        }
	}
}
