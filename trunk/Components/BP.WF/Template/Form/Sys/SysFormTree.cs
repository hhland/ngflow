using System;
using System.Data;
using System.Collections;
using BP.DA;
using BP.En;
using BP.Port;

namespace BP.Sys
{
    /// <summary>
    ///  Property 
    /// </summary>
    public class SysFormTreeAttr : EntityTreeAttr
    {
    }
    /// <summary>
    ///   Process tree form 
    /// </summary>
    public class SysFormTree : EntityTree
    {
        #region  Constructor 
        /// <summary>
        ///  Process tree form 
        /// </summary>
        public SysFormTree()
        {
        }
        /// <summary>
        ///  Process tree form 
        /// </summary>
        /// <param name="_No"></param>
        public SysFormTree(string _No) : base(_No) { }
        #endregion

        #region  Systems approach .
        /// <summary>
        ///  Process tree form Map
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("Sys_FormTree");
                map.EnDesc = " Form tree ";
                map.CodeStruct = "2";

                map.DepositaryOfEntity = Depositary.Application;
                map.DepositaryOfMap = Depositary.Application;

                map.AddTBStringPK(SysFormTreeAttr.No, null, " Serial number ", true, true, 1, 10, 20);
                map.AddTBString(SysFormTreeAttr.Name, null, " Name ", true, false, 0, 100, 30);
                map.AddTBString(SysFormTreeAttr.ParentNo, null, " Parent No", false, false, 0, 100, 30);
                map.AddTBString(SysFormTreeAttr.TreeNo, null, "TreeNo", false, false, 0, 100, 30);

                map.AddTBInt(SysFormTreeAttr.IsDir, 0, " Whether it is a directory ?", false, false);
                map.AddTBInt(SysFormTreeAttr.Idx, 0, "Idx", false, false);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion  Systems approach .

        protected override bool beforeDelete()
        {
            if (!string.IsNullOrEmpty(this.No))
                DeleteChild(this.No);
            return base.beforeDelete();
        }
        /// <summary>
        ///  Delete subkey 
        /// </summary>
        /// <param name="parentNo"></param>
        private void DeleteChild(string parentNo)
        {
            SysFormTrees formTrees = new SysFormTrees();
            formTrees.RetrieveByAttr(SysFormTreeAttr.ParentNo, parentNo);
            foreach (SysFormTree item in formTrees)
            {
                MapData md = new MapData();
                md.FK_FormTree = item.No;
                md.Delete();
                DeleteChild(item.No);
            }
        }
    }
    /// <summary>
    ///  Process tree form 
    /// </summary>
    public class SysFormTrees : EntitiesTree
    {
        /// <summary>
        ///  Process tree form s
        /// </summary>
        public SysFormTrees() { }
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new SysFormTree();
            }

        }
        public override int RetrieveAll()
        {
            int i = base.RetrieveAll();
            if (i == 0)
            {
                SysFormTree fs = new SysFormTree();
                fs.Name = " Document type ";
                fs.No = "01";
                fs.Insert();

                fs = new SysFormTree();
                fs.Name = " Office class ";
                fs.No = "02";
                fs.Insert();
                i = base.RetrieveAll();
            }
            return i;
        }
    }
}
