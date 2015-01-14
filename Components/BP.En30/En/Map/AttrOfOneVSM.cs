using System;
using System.Collections;
using BP.En;

namespace BP.En
{
	/// <summary>
	/// SearchKey  The summary .
	///  To deal with a record store , Issue .
	/// </summary>
	public class AttrOfOneVSM 
	{
		#region  Basic properties 
		/// <summary>
		///  #NAME? .
		/// </summary>
		private Entities _ensOfMM=null;
		/// <summary>
		///  Entity set-many 
		/// </summary>
		public Entities EnsOfMM
		{
			get
			{
				return _ensOfMM;
			}
			set
			{
				_ensOfMM=value;
			}
		}
		/// <summary>
		///  #NAME? 
		/// </summary>
		private Entities _ensOfM=null;
		/// <summary>
		///  Entity set-many 
		/// </summary>
		public Entities EnsOfM
		{
			get
			{
				return _ensOfM;
			}
			set
			{
				_ensOfM=value;
			}
		}
		/// <summary>
		/// M Physical properties of the entity in the many- 
		/// </summary>
		private string _Desc=null;
		/// <summary>
		///  Physical properties of the entity in the many- 
		/// </summary>
		public string Desc
		{
			get
			{
			    return _Desc;//edited by liuxc,2014-10-18 "<font color=blue ><u>" + _Desc + "</u></font>";
			}
			set
			{
				_Desc=value;
			}
		}
		/// <summary>
		///  An entity attributes in entity-many .
		/// </summary>
		private string _AttrOfOneInMM=null;
		/// <summary>
		///  An entity attributes in entity-many 
		/// </summary>
		public string AttrOfOneInMM
		{
			get
			{
				return _AttrOfOneInMM;
			}
			set
			{
				_AttrOfOneInMM=value;
			}
		}

		/// <summary>
		/// M Physical properties of the entity in the many- 
		/// </summary>
		private string _AttrOfMInMM=null;
		/// <summary>
		///  Physical properties of the entity in the many- 
		/// </summary>
		public string AttrOfMInMM
		{
			get
			{
				return _AttrOfMInMM;
			}
			set
			{
				_AttrOfMInMM=value;
			}
		}
		/// <summary>
		///  Label 
		/// </summary>
		private string _AttrOfMText=null;
		/// <summary>
		///  Label 
		/// </summary>
		public string AttrOfMText
		{
			get
			{
				return _AttrOfMText;
			}
			set
			{
				_AttrOfMText=value;
			}
		}
		/// <summary>
		/// Value
		/// </summary>
		private string _AttrOfMValue=null;
		/// <summary>
		/// Value
		/// </summary>
		public string AttrOfMValue
		{
			get
			{
				return _AttrOfMValue;
			}
			set
			{
				_AttrOfMValue=value;
			}
		}
		#endregion

		#region  Constructor 
		/// <summary>
		/// AttrOfOneVSM
		/// </summary>
		public AttrOfOneVSM()
		{}
		/// <summary>
		/// AttrOfOneVSM
		/// </summary>
		/// <param name="_ensOfMM"></param>
		/// <param name="_ensOfM"></param>
		/// <param name="AttrOfOneInMM"></param>
		/// <param name="AttrOfMInMM"></param>
		/// <param name="AttrOfMText"></param>
		/// <param name="AttrOfMValue"></param>
		public AttrOfOneVSM(Entities _ensOfMM, Entities _ensOfM, string AttrOfOneInMM, string AttrOfMInMM , string AttrOfMText, string AttrOfMValue, string desc)
		{
			this.EnsOfM=_ensOfM;
			this.EnsOfMM=_ensOfMM;
			this.AttrOfOneInMM=AttrOfOneInMM;
			this.AttrOfMInMM=AttrOfMInMM;
			this.AttrOfMText=AttrOfMText;
			this.AttrOfMValue=AttrOfMValue;
			this.Desc=desc;
		}
		#endregion

	}
	/// <summary>
	/// AttrsOfOneVSM  Set 
	/// </summary>
	public class AttrsOfOneVSM : System.Collections.CollectionBase
	{
		public AttrsOfOneVSM()
		{
		}
		public AttrOfOneVSM this[int index]
		{
			get
			{
				return (AttrOfOneVSM)this.InnerList[index];
			}
		}
		/// <summary>
		///  Adding a SearchKey .
		/// </summary>
		/// <param name="r">SearchKey</param>
		public void Add(AttrOfOneVSM attr)
		{
			if (this.IsExits(attr))
				return ;
			this.InnerList.Add(attr);
		}

		/// <summary>
		///  Is there a collection of inside 
		/// </summary>
		/// <param name="en"> To check the EnDtl</param>
		/// <returns>true/false</returns>
		public bool IsExits(AttrOfOneVSM en)
		{
			foreach (AttrOfOneVSM attr in this )
			{
				if (attr.EnsOfMM == en.EnsOfMM  )
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		///  Add a property 
		/// </summary>
		/// <param name="_ensOfMM"> #NAME? </param>
		/// <param name="_ensOfM"> Multibody </param>
		/// <param name="AttrOfOneInMM"> Point entity ,ÔÚMM Properties in </param>
		/// <param name="AttrOfMInMM"> Multi-entity primary key in MM Properties in </param>
		/// <param name="AttrOfMText"></param>
		/// <param name="AttrOfMValue"></param>
		/// <param name="desc"> Description </param>
		public void Add(Entities _ensOfMM, Entities _ensOfM, string AttrOfOneInMM, string AttrOfMInMM , string AttrOfMText, string AttrOfMValue, string desc)
		{
			AttrOfOneVSM en = new AttrOfOneVSM(_ensOfMM,_ensOfM,AttrOfOneInMM,AttrOfMInMM,AttrOfMText,AttrOfMValue,desc) ;
			 
			this.Add(en);				
		}
		 
	}
}
