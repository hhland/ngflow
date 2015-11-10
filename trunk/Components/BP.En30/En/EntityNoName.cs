using System;
using System.Collections;
using BP.DA;

namespace BP.En
{
	/// <summary>
	///  Property list 
	/// </summary>
	public class EntityNoNameAttr : EntityNoAttr
	{	
		/// <summary>
		///  Name 
		/// </summary>
		public const string Name="Name";
        /// <summary>
        ///  Name abbreviation 
        /// </summary>
        public const string NameOfS = "NameOfS";

	}
    public class EntityNoNameMyFileAttr : EntityNoMyFileAttr
    {
        /// <summary>
        ///  Name 
        /// </summary>
        public const string Name = "Name";
    }
	/// <summary>
	///  Has a base class name entity number 
	/// </summary>
    abstract public class EntityNoName : EntityNo
    {
        #region  Property 

        /// <summary>
        ///  Name 
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetValStringByKey(EntityNoNameAttr.Name);
            }
            set
            {
                this.SetValByKey(EntityNoNameAttr.Name, value);
            }
        }
        //public string NameE
        //{
        //    get
        //    {
        //        return this.GetValStringByKey("NameE");
        //    }
        //    set
        //    {
        //        this.SetValByKey("NameE", value);
        //    }
        //}
        #endregion

        #region  Constructor 
        /// <summary>
        /// 
        /// </summary>
        public EntityNoName()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_No"></param>
        protected EntityNoName(string _No) : base(_No) { }
        #endregion

        #region  Business logic processing 
        /// <summary>
        ///  Check the name issue .
        /// </summary>
        /// <returns></returns>
        protected override bool beforeInsert()
        {
            if (this.No.Trim().Length == 0)
            {
                if (this.EnMap.IsAutoGenerNo)
                    this.No = this.GenerNewNo;
                else
                    throw new Exception("@ Did not give [" + this.EnDesc + " , " + this.Name + "] Set the primary key .");
            }

            if (this.EnMap.IsAllowRepeatName == false)
            {
                if (this.PKCount == 1)
                {
                    if (this.ExitsValueNum("Name", this.Name) >= 1)
                        throw new Exception("@ Insert failed [" + this.EnMap.EnDesc + "]  Serial number [" + this.No + "] Name [" + Name + "] Repeat .");
                }
            }
            return base.beforeInsert();
        }
        protected override bool beforeUpdate()
        {
            if (this.EnMap.IsAllowRepeatName == false)
            {
                if (this.PKCount == 1)
                {
                    if (this.ExitsValueNum("Name", this.Name) >= 2)
                        throw new Exception("@ Update Failed [" + this.EnMap.EnDesc + "]  Serial number [" + this.No + "] Name [" + Name + "] Repeat .");
                }
            }
            return base.beforeUpdate();
        }
        #endregion
    }
	/// <summary>
    ///  Has a base class name entity number s
	/// </summary>
    abstract public class EntitiesNoName : EntitiesNo
    {
        /// <summary>
        ///  Add objects to the collection at the end of , If the object already exists , Not added .
        /// </summary>
        /// <param name="entity"> The object to add </param>
        /// <returns> Adding to return to the place </returns>
        public virtual int AddEntity(EntityNoName entity)
        {
            foreach (EntityNoName en in this)
            {
                if (en.No == entity.No)
                    return 0;
            }
            return this.InnerList.Add(entity);
        }
        /// <summary>
        ///  Add objects to the collection at the end of , If the object already exists , Not added 
        /// </summary>
        /// <param name="entity"> The object to add </param>
        /// <returns> Adding to return to the place </returns>
        public virtual void Insert(int index, EntityNoName entity)
        {
            foreach (EntityNoName en in this)
            {
                if (en.No == entity.No)
                    return;
            }

            this.InnerList.Insert(index, entity);
        }
        /// <summary>
        ///  Access to data based on location 
        /// </summary>
        public new EntityNoName this[int index]
        {
            get
            {
                return (EntityNoName)this.InnerList[index];
            }
        }
        /// <summary>
        ///  Structure 
        /// </summary>
        public EntitiesNoName()
        {
        }
        /// <summary> 
        ///  Fuzzy query by name 
        /// </summary>
        /// <param name="likeName">likeName</param>
        /// <returns> Return query Num</returns>
        public int RetrieveByLikeName(string likeName)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere("Name", "like", " %" + likeName + "% ");
            return qo.DoQuery();
        }
        public override int RetrieveAll()
        {
            return base.RetrieveAll("No");
        }
    }
}
