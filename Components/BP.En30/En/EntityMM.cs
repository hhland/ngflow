using System;

namespace BP.En
{
	/// <summary>
	///  Collection-many .
	/// </summary>
	abstract public class EntityMM:Entity
	{
		/// <summary>
		///  Collection-many 
		/// </summary>
		public EntityMM()
		{
		}
	}
	/// <summary>
	///  Collection-many 
	/// </summary>
	abstract public class EntitiesMM : Entities
	{
		/// <summary>
		///  Collection-many 
		/// </summary>
		protected EntitiesMM() 
		{
		}
		/// <summary>
		///  Provided by an entity  val  Another query entity set .
		/// </summary>
		/// <param name="attr"> Property </param>
		/// <param name="val">ֲ</param>
		/// <param name="refEns"> Associated collection </param>
		/// <returns> Associated collection </returns>
		protected Entities throwOneKeyValGetRefEntities(string attr , int val, Entities refEns )
		{																									
			QueryObject qo = new QueryObject(refEns);
			qo.AddWhere(attr, val);
			return refEns;
		}
		/// <summary>
		///  Provided by an entity  val  Another query entity set .
		/// </summary>
		/// <param name="attr"> Property </param>
		/// <param name="val">ֲ</param>
		/// <param name="refEns"> Associated collection </param>
		/// <returns> Associated collection </returns>
		protected Entities throwOneKeyValGetRefEntities(string attr, string val, Entities  refEns) 	 
		{
			QueryObject qo = new QueryObject(refEns);
			qo.AddWhere(attr, val);
			return refEns;
		}
	}
}
