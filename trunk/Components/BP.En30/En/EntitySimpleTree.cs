using System;
using System.Collections;
using BP.DA;

namespace BP.En
{
	/// <summary>
	///  Property list 
	/// </summary>
    public class EntitySimpleTreeAttr
    {
        /// <summary>
        ///  Tree numbers 
        /// </summary>
        public const string No = "No";
        /// <summary>
        ///  Name 
        /// </summary>
        public const string Name = "Name";
        /// <summary>
        ///  Parent section number 
        /// </summary>
        public const string ParentNo = "ParentNo";
    }
	/// <summary>
	///  Tree entity 
	/// </summary>
    abstract public class EntitySimpleTree : Entity
    {
        #region  Property 
        public bool IsRoot
        {
            get
            {
                if (this.ParentNo == "-1" || this.ParentNo == "0")
                    return true;

                if (this.ParentNo == this.No)
                    return true;

                return false;
            }
        }
        /// <summary>
        ///  The only mark 
        /// </summary>
        public string No
        {
            get
            {
                return this.GetValStringByKey(EntitySimpleTreeAttr.No);
            }
            set
            {
                this.SetValByKey(EntitySimpleTreeAttr.No, value);
            }
        }
        /// <summary>
        ///  Name 
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetValStringByKey(EntitySimpleTreeAttr.Name);
            }
            set
            {
                this.SetValByKey(EntitySimpleTreeAttr.Name, value);
            }
        }
        /// <summary>
        ///  Parent node number 
        /// </summary>
        public string ParentNo
        {
            get
            {
                return this.GetValStringByKey(EntitySimpleTreeAttr.ParentNo);
            }
            set
            {
                this.SetValByKey(EntitySimpleTreeAttr.ParentNo, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Primary key 
        /// </summary>
        public override string PK
        {
            get
            {
                return EntitySimpleTreeAttr.No;
            }
        }
        /// <summary>
        ///  Tree numbers 
        /// </summary>
        public EntitySimpleTree()
        {
        }
        /// <summary>
        ///  Tree numbers 
        /// </summary>
        /// <param name="no"> Serial number </param>
        public EntitySimpleTree(string no)
        {
            if (string.IsNullOrEmpty(no))
                throw new Exception(this.EnDesc + "@ On the table [" + this.EnDesc + "] You must specify the number before query .");

            this.No = no;
            if (this.Retrieve() == 0)
                throw new Exception("@ No " + this._enMap.PhysicsTable + ", No = " + this.No + " Records .");
        }
        #endregion

    }
	/// <summary>
    ///  Tree entity s
	/// </summary>
    abstract public class EntitiesSimpleTree : Entities
    {
        /// <summary>
        ///  Check his child nodes 
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public int RetrieveHisChinren(EntitySimpleTree en)
        {
            int i=this.Retrieve(EntitySimpleTreeAttr.ParentNo, en.No);
            this.AddEntity(en);
            return i + 1;
        }

        /// <summary>
        ///  Get its child nodes 
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public EntitiesTree GenerHisChinren(EntitySimpleTree en)
        {
            EntitiesTree ens = this.CreateInstance() as EntitiesTree;
            foreach (EntitySimpleTree item in ens)
            {
                if (en.ParentNo == en.No)
                    ens.AddEntity(item);
            }
            return ens;
        }
        /// <summary>
        ///  Access to data based on location 
        /// </summary>
        public new EntitySimpleTree this[int index]
        {
            get
            {
                return (EntitySimpleTree)this.InnerList[index];
            }
        }
        /// <summary>
        ///  Structure 
        /// </summary>
        public EntitiesSimpleTree()
        {
        }
        /// <summary>
        ///  Check all 
        /// </summary>
        /// <returns></returns>
        public override int RetrieveAll()
        {
            return base.RetrieveAll("No");
        }
    }
}
