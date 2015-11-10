using System;
using System.Collections;
using BP.DA;

namespace BP.En
{
	/// <summary>
	///  Property list 
	/// </summary>
    public class EntityMultiTreeAttr
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
        /// <summary>
        ///  Tree No. 
        /// </summary>
        public const string TreeNo = "TreeNo";
        /// <summary>
        ///  Sequence number 
        /// </summary>
        public const string Idx = "Idx";
        /// <summary>
        ///  Whether it is a directory 
        /// </summary>
        public const string IsDir = "IsDir";
    }
	/// <summary>
	///  Multiple trees entities 
	/// </summary>
    abstract public class EntityMultiTree : Entity
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
                return this.GetValStringByKey(EntityMultiTreeAttr.No);
            }
            set
            {
                this.SetValByKey(EntityMultiTreeAttr.No, value);
            }
        }
        /// <summary>
        ///  Tree numbers 
        /// </summary>
        public string TreeNo
        {
            get
            {
                return this.GetValStringByKey(EntityMultiTreeAttr.TreeNo);
            }
            set
            {
                this.SetValByKey(EntityMultiTreeAttr.TreeNo, value);
            }
        }
        /// <summary>
        ///  Name 
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetValStringByKey(EntityMultiTreeAttr.Name);
            }
            set
            {
                this.SetValByKey(EntityMultiTreeAttr.Name, value);
            }
        }
        /// <summary>
        ///  Parent node number 
        /// </summary>
        public string ParentNo
        {
            get
            {
                return this.GetValStringByKey(EntityMultiTreeAttr.ParentNo);
            }
            set
            {
                this.SetValByKey(EntityMultiTreeAttr.ParentNo, value);
            }
        }
        /// <summary>
        ///  Whether it is a directory 
        /// </summary>
        public bool IsDir
        {
            get
            {
                return this.GetValBooleanByKey(EntityMultiTreeAttr.IsDir);
            }
            set
            {
                this.SetValByKey(EntityMultiTreeAttr.IsDir, value);
            }
        }
        /// <summary>
        ///  Sequence number 
        /// </summary>
        public int Idx
        {
            get
            {
                return this.GetValIntByKey(EntityMultiTreeAttr.Idx);
            }
            set
            {
                this.SetValByKey(EntityMultiTreeAttr.Idx, value);
            }
        }
        /// <summary>
        ///  Level 
        /// </summary>
        public int Grade
        {
            get
            {
                return this.TreeNo.Length / 2;
            }
        }
        #endregion

        #region  Need to be rewritten .
        /// <summary>
        ///  Subject field associated .
        ///  For example, in the process tree form ,  Is the process ID field , Entity classes need to be rewritten .
        /// </summary>
        public abstract string RefObjField
        {
            get;
        }
        #endregion  Need to be rewritten .

        #region  Constructor 
        /// <summary>
        ///  Primary key 
        /// </summary>
        public override string PK
        {
            get
            {
                return EntityMultiTreeAttr.No;
            }
        }
        /// <summary>
        ///  Tree numbers 
        /// </summary>
        public EntityMultiTree()
        {
        }
        /// <summary>
        ///  Tree numbers 
        /// </summary>
        /// <param name="no"> Serial number </param>
        public EntityMultiTree(string no)
        {
            if (string.IsNullOrEmpty(no))
                throw new Exception(this.EnDesc + "@ On the table [" + this.EnDesc + "] You must specify the number before query .");

            this.No = no;
            if (this.Retrieve() == 0)
                throw new Exception("@ No " + this._enMap.PhysicsTable + ", No = " + this.No + " Records .");
        }
        #endregion

        #region  Business logic processing 
        /// <summary>
        ///  Reset treeNo
        /// </summary>
        public void ResetTreeNo()
        {
        }
        /// <summary>
        ///  Check the name issue .
        /// </summary>
        /// <returns></returns>
        protected override bool beforeInsert()
        {
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

        #region  Subclass allows calls 
        /// <summary>
        ///  New sibling nodes 
        /// </summary>
        /// <returns></returns>
        public EntityMultiTree DoCreateSameLevelNode()
        {
            EntityMultiTree en = this.CreateInstance() as EntityMultiTree;
            en.No = BP.DA.DBAccess.GenerOID(this.ToString()).ToString(); // en.GenerNewNoByKey(EntityMultiTreeAttr.No);
            en.Name = " New node " + en.No;
            en.ParentNo = this.ParentNo;
            en.IsDir = false;
            en.TreeNo = this.GenerNewNoByKey(EntityMultiTreeAttr.TreeNo, EntityMultiTreeAttr.ParentNo, this.ParentNo);

             // To the entity class assignment .
            en.SetValByKey(this.RefObjField, this.GetValStringByKey(this.RefObjField) ); 

            en.Insert();
            return en;
        }
        /// <summary>
        ///  New child nodes 
        /// </summary>
        /// <returns></returns>
        public EntityMultiTree DoCreateSubNode()
        {
            EntityMultiTree en = this.CreateInstance() as EntityMultiTree;
            en.No = BP.DA.DBAccess.GenerOID(this.ToString()).ToString(); // en.GenerNewNoByKey(EntityMultiTreeAttr.No);
            en.Name = " New node " + en.No;
            en.ParentNo = this.No;
            en.IsDir = false;

            // To the entity class assignment .
            en.SetValByKey(this.RefObjField, this.GetValStringByKey(this.RefObjField) ); 

            en.TreeNo = this.GenerNewNoByKey(EntityMultiTreeAttr.TreeNo, EntityMultiTreeAttr.ParentNo, this.No);
            if (en.TreeNo.Substring(en.TreeNo.Length - 2) == "01")
                en.TreeNo = this.TreeNo + "01";
            en.Insert();

            //  Set this node is a directory 
            if (this.IsDir == false)
            {
                this.IsDir = true;
                this.Update(EntityMultiTreeAttr.IsDir, true);
            }
            return en;
        }
        /// <summary>
        ///  Move 
        /// </summary>
        /// <returns></returns>
        public string DoUp()
        {
            this.DoOrderUp(EntityMultiTreeAttr.ParentNo, this.ParentNo,
                this.RefObjField, this.GetValStringByKey(RefObjField), EntityMultiTreeAttr.Idx);
            return null;
        }
        /// <summary>
        ///  Down 
        /// </summary>
        /// <returns></returns>
        public string DoDown()
        {
            this.DoOrderDown(EntityMultiTreeAttr.ParentNo, this.ParentNo,
                this.RefObjField, this.GetValStringByKey(RefObjField), EntityMultiTreeAttr.Idx);
            return null;
        }
        #endregion
    }
	/// <summary>
    ///  Multiple trees entities s
	/// </summary>
    abstract public class EntitiesMultiTree : Entities
    {
        /// <summary>
        ///  Check his child nodes 
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public int RetrieveHisChinren(EntityMultiTree en)
        {
            int i=this.Retrieve(EntityMultiTreeAttr.ParentNo, en.No);
            this.AddEntity(en);
            return i + 1;
        }
        /// <summary>
        ///  Get its child nodes 
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public EntitiesTree GenerHisChinren(EntityMultiTree en)
        {
            EntitiesTree ens = this.CreateInstance() as EntitiesTree;
            foreach (EntityMultiTree item in ens)
            {
                if (en.ParentNo == en.No)
                    ens.AddEntity(item);
            }
            return ens;
        }
        /// <summary>
        ///  Access to data based on location 
        /// </summary>
        public new EntityMultiTree this[int index]
        {
            get
            {
                return (EntityMultiTree)this.InnerList[index];
            }
        }
        /// <summary>
        ///  Structure 
        /// </summary>
        public EntitiesMultiTree()
        {
        }
    }
}
