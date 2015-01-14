using System;
using System.Collections;
using BP.DA;

namespace BP.En
{
	/// <summary>
	/// EntityOIDNameAttr
	/// </summary>
	public class EntityOIDNameAttr:EntityOIDAttr
	{
		/// <summary>
		///  Name 
		/// </summary>
		public const string Name="Name";
	}
	/// <summary>
	///  For  OID Name  Entity inherited property .	
	/// </summary>
    abstract public class EntityOIDName : EntityOID
    {
        #region  Structure 
        /// <summary>
        ///  Primary key value 
        /// </summary>
        public override string PK
        {
            get
            {
                return "OID";
            }
        }
        public override string PKField
        {
            get
            {
                return "OID";
            }
        }
        /// <summary>
        ///  Structure 
        /// </summary>
        protected EntityOIDName() { }
        /// <summary>
        ///  Structure 
        /// </summary>
        /// <param name="oid">OID</param>
        protected EntityOIDName(int oid) : base(oid) { }
        #endregion

        #region  Properties Methods 
        /// <summary>
        ///  Name 
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetValStringByKey(EntityOIDNameAttr.Name);
            }
            set
            {
                this.SetValByKey(EntityOIDNameAttr.Name, value);
            }
        }
        /// <summary>
        ///  Query by name .
        /// </summary>
        /// <returns> Check out the number of returns </returns>
        public int RetrieveByName()
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere("Name", this.Name);
            return qo.DoQuery();
        }
        protected int LoadDir(string dir)
        {


            return 1;
        }
        
        protected override bool beforeUpdate()
        {
            if (this.EnMap.IsAllowRepeatName == false)
            {
                if (this.PKCount == 1)
                {
                    if (this.ExitsValueNum("Name", this.Name) >= 2)
                        throw new Exception("@ Update Failed [" + this.EnMap.EnDesc + "] OID=[" + this.OID + "] Name [" + Name + "] Repeat .");
                }
            }
            return base.beforeUpdate();
        }
        #endregion
    }
	/// <summary>
	///  For OID Name  Entity inherited property 
	/// </summary>
	abstract public class EntitiesOIDName : EntitiesOID
	{
		#region  Structure 
		/// <summary>
		///  Structure 
		/// </summary>
		public EntitiesOIDName()
		{
			//
			// TODO:  Add constructor logic here 
			//
		}		
		#endregion
	}
}
