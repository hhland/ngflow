using System;
using System.Collections;
using BP.DA;

namespace BP.En
{

	/// <summary>
	///  Property list 
	/// </summary>
	public class EntityMIDAttr
	{
		public static string MID="MID";
	}
	/// <summary>
	/// MID Entity , Only an entity that entity has only one primary key attribute .
	/// </summary>
	abstract public class EntityMID : Entity
	{

		public override int Save()
		{
			
			if ( this.Update()==0 )
			{
				this.MID=BP.DA.DBAccess.GenerOID();
				this.Insert();
				this.Retrieve();
			}
			return this.MID;
		}


//		public override int Save()
//		{
//
//			if (this.IsExits)
//				return this.Update();
//			else
//			{
//				this.Insert();
//				return 1;
//			}
//
//			//			if (this.Update()==0)
//			//				this.Insert();
//			//
//			//			return base.Save ();
//		}


		/// <summary>
		///  Whether there 
		/// </summary>
		/// <returns></returns>
		public bool IsExitCheckByPKs()
		{
			return false;
		}


		#region  Property 
		/// <summary>
		/// MID,  If it is empty on return  0 . 
		/// </summary>
		public int MID
		{
			get
			{
				try
				{
					return this.GetValIntByKey(EntityMIDAttr.MID);
				}
				catch
				{
					return 0; 
				}
			}

			set
			{
				this.SetValByKey(EntityMIDAttr.MID,value);
			}
		}
		#endregion

		#region  Constructor 
		/// <summary>
		///  Constructs an empty instance 
		/// </summary>
		protected EntityMID()
		{
		}
		/// <summary>
		///  According to MID Constructive Solid 
		/// </summary>
		/// <param name="MID">MID</param>
		protected EntityMID(int mid)  
		{
			this.SetValByKey(EntityMIDAttr.MID,MID);
			QueryObject qo = new QueryObject(this);
			qo.AddWhere(EntityMIDAttr.MID,mid);
			if (qo.DoQuery()==0)
				throw new Exception(" No inquiry into MID="+mid+" Examples .");
			//this.Retrieve();
		}
		#endregion
	 
		#region override  Method 
	
		public override int Retrieve()
		{
			if (this.MID==0)
			{
				return base.Retrieve();
			}
			else
			{
				QueryObject qo = new QueryObject(this);
				qo.AddWhere("MID", this.MID);
				if (qo.DoQuery()==0)
					throw new Exception(" Without this record :MID="+this.MID);
				else
					return 1;
			}
		}

		/// <summary>
		///  Removed before operation .
		/// </summary>
		/// <returns></returns>
		protected override bool beforeDelete() 
		{
			if (base.beforeDelete()==false)
				return false;

			try 
			{				
				if (this.MID < 0 )
					throw new Exception("@ Entity ["+this.EnDesc+"] Has not been instantiated , Can not Delete().");
				return true;
				 
			} 
			catch (Exception ex) 
			{
				throw new Exception("@["+this.EnDesc+"].beforeDelete err:"+ex.Message);
			}
		}
		public override int DirectInsert()
		{
			this.MID=DBAccess.GenerOID();
			//EnDA.Insert(this);
			return this.RunSQL( SqlBuilder.Insert(this) );

		}
		#endregion

		#region public  Method 
		 
		#endregion

	}
	/// <summary>
	/// MID Entity set 
	/// </summary>
	abstract public class EntitiesMID : Entities
	{
		public EntitiesMID()
		{
		}		 
	}
}
