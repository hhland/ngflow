using System;
using System.Collections;
using BP.DA;

namespace BP.En
{
	/// <summary>
	///  Property list 
	/// </summary>
    public class EntityTreeAttr
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
        /// <summary>
        ///  Control parameters 
        /// </summary>
        public const string CtrlWayPara = "CtrlWayPara";
        /// <summary>
        ///  Icon 
        /// </summary>
        public const string ICON = "ICON";
    }
	/// <summary>
	///  Tree entity 
	/// </summary>
    abstract public class EntityTree : Entity
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
                return this.GetValStringByKey(EntityTreeAttr.No);
            }
            set
            {
                this.SetValByKey(EntityTreeAttr.No, value);
            }
        }
        /// <summary>
        ///  Tree numbers 
        /// </summary>
        public string TreeNo
        {
            get
            {
                return this.GetValStringByKey(EntityTreeAttr.TreeNo);
            }
            set
            {
                this.SetValByKey(EntityTreeAttr.TreeNo, value);
            }
        }
        /// <summary>
        ///  Name 
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetValStringByKey(EntityTreeAttr.Name);
            }
            set
            {
                this.SetValByKey(EntityTreeAttr.Name, value);
            }
        }
        /// <summary>
        ///  Parent node number 
        /// </summary>
        public string ParentNo
        {
            get
            {
                return this.GetValStringByKey(EntityTreeAttr.ParentNo);
            }
            set
            {
                this.SetValByKey(EntityTreeAttr.ParentNo, value);
            }
        }
        /// <summary>
        ///  Icon 
        /// </summary>
        public string ICON
        {
            get
            {
                return this.GetValStringByKey(EntityTreeAttr.ICON);
            }
            set
            {
                this.SetValByKey(EntityTreeAttr.ICON, value);
            }
        }
        /// <summary>
        ///  Whether it is a directory 
        /// </summary>
        public bool IsDir
        {
            get
            {
                return this.GetValBooleanByKey(EntityTreeAttr.IsDir);
            }
            set
            {
                this.SetValByKey(EntityTreeAttr.IsDir, value);
            }
        }
        /// <summary>
        ///  Sequence number 
        /// </summary>
        public int Idx
        {
            get
            {
                return this.GetValIntByKey(EntityTreeAttr.Idx);
            }
            set
            {
                this.SetValByKey(EntityTreeAttr.Idx, value);
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

        #region  Constructor 
        /// <summary>
        ///  Primary key 
        /// </summary>
        public override string PK
        {
            get
            {
                return EntityTreeAttr.No;
            }
        }
        /// <summary>
        ///  Tree numbers 
        /// </summary>
        public EntityTree()
        {
        }
        /// <summary>
        ///  Tree numbers 
        /// </summary>
        /// <param name="no"> Serial number </param>
        public EntityTree(string no)
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

            if (string.IsNullOrEmpty(this.No))
                this.No = this.GenerNewNoByKey("No");
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
        public EntityTree DoCreateSameLevelNode()
        {
            EntityTree en = this.CreateInstance() as EntityTree;
            en.No = BP.DA.DBAccess.GenerOID(this.ToString()).ToString(); // en.GenerNewNoByKey(EntityTreeAttr.No);
            en.Name = " New node " + en.No;
            en.ParentNo = this.ParentNo;
            en.IsDir = false;
            // en.TreeNo=this.GenerNewNoByKey(EntityTreeAttr.TreeNo,EntityTreeAttr.ParentNo,this.ParentNo)
            en.TreeNo = this.GenerNewNoByKey(EntityTreeAttr.TreeNo, EntityTreeAttr.ParentNo, this.ParentNo);
            en.Insert();
            return en;
        }
        /// <summary>
        ///  New child nodes 
        /// </summary>
        /// <returns></returns>
        public EntityTree DoCreateSubNode()
        {
            EntityTree en = this.CreateInstance() as EntityTree;
            en.No = BP.DA.DBAccess.GenerOID(this.ToString()).ToString(); // en.GenerNewNoByKey(EntityTreeAttr.No);
            en.Name = " New node " + en.No;
            en.ParentNo = this.No;
            en.IsDir = false;
            en.TreeNo = this.GenerNewNoByKey(EntityTreeAttr.TreeNo, EntityTreeAttr.ParentNo, this.No);
            if (en.TreeNo.Substring(en.TreeNo.Length - 2) == "01")
                en.TreeNo = this.TreeNo + "01";
            en.Insert();

            //  Set this node is a directory 
            if (this.IsDir == false)
            {
                this.Retrieve();
                this.IsDir = true;
                this.Update();
            }
            return en;
        }
        /// <summary>
        ///  Move 
        /// </summary>
        /// <returns></returns>
        public string DoUp()
        {
            this.DoOrderUp(EntityTreeAttr.ParentNo, this.ParentNo, EntityTreeAttr.Idx);
            return null;
        }
        /// <summary>
        ///  Down 
        /// </summary>
        /// <returns></returns>
        public string DoDown()
        {
            this.DoOrderDown(EntityTreeAttr.ParentNo, this.ParentNo, EntityTreeAttr.Idx);
            return null;
        }
        #endregion
    }
	/// <summary>
    ///  Tree entity s
	/// </summary>
    abstract public class EntitiesTree : Entities
    {
        /// <summary>
        ///  Check his child nodes 
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public int RetrieveHisChinren(EntityTree en)
        {
            int i=this.Retrieve(EntityTreeAttr.ParentNo, en.No);
            this.AddEntity(en);
            return i + 1;
        }

        /// <summary>
        ///  Get its child nodes 
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public EntitiesTree GenerHisChinren(EntityTree en)
        {
            EntitiesTree ens = this.CreateInstance() as EntitiesTree;
            foreach (EntityTree item in ens)
            {
                if (en.ParentNo == en.No)
                    ens.AddEntity(item);
            }
            return ens;
        }
        /// <summary>
        ///  Access to data based on location 
        /// </summary>
        public new EntityTree this[int index]
        {
            get
            {
                return (EntityTree)this.InnerList[index];
            }
        }
        /// <summary>
        ///  Structure 
        /// </summary>
        public EntitiesTree()
        {
        }
        /// <summary>
        ///  Check all 
        /// </summary>
        /// <returns></returns>
        public override int RetrieveAll()
        {
            return base.RetrieveAll("TreeNo");
        }
    }
}
