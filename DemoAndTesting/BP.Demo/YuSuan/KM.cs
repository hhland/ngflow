using System;
using System.Data;
using System.Collections;
using BP.DA;
using BP.En;
using BP.Port;

namespace BP.YS
{
    /// <summary>
    ///  Subject property 
    /// </summary>
    public class KMAttr : EntityTreeAttr
    {
    }
    /// <summary>
    ///   Subject 
    /// </summary>
    public class KM : EntityTree
    {
        #region  Constructor 
        /// <summary>
        ///  Subject 
        /// </summary>
        public KM()
        {
        }
        /// <summary>
        ///  Subject 
        /// </summary>
        /// <param name="_No"></param>
        public KM(string _No) : base(_No) { }
        #endregion

        /// <summary>
        ///  Subject Map
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("YS_KM");
                map.EnDesc = " Subject ";
                map.CodeStruct = "2";
                map.IsAllowRepeatName = false;

                map.DepositaryOfEntity = Depositary.Application;
                map.DepositaryOfMap = Depositary.Application;

                map.AddTBStringPK(KMAttr.No, null, " Serial number ", true, true, 1, 10, 20);
                map.AddTBString(KMAttr.Name, null, " Name ", true, false, 0, 100, 30);
                map.AddTBString(KMAttr.ParentNo, null, " Parent No", false, false, 0, 100, 30);
                map.AddTBString(KMAttr.TreeNo, null, "TreeNo", false, false, 0, 100, 30);

                map.AddTBInt(KMAttr.Idx, 0, "Idx", false, false);
                map.AddTBInt(KMAttr.IsDir, 0, "IsDir", false, false);

                this._enMap = map;
                return this._enMap;
            }
        }

        
    }
    /// <summary>
    ///  Subject 
    /// </summary>
    public class KMs : EntitiesTree
    {
        /// <summary>
        ///  Subject s
        /// </summary>
        public KMs() { }
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new KM();
            }

        }
        /// <summary>
        ///  Subject s
        /// </summary>
        /// <param name="no">ss</param>
        /// <param name="name">anme</param>
        public void AddByNoName(string no, string name)
        {
            KM en = new KM();
            en.No = no;
            en.Name = name;
            this.AddEntity(en);
        }
        public override int RetrieveAll()
        {
            int i = base.RetrieveAll();
            if (i == 0)
            {
                KM fs = new KM();
                fs.Name = " Document type ";
                fs.No = "01";
                fs.Insert();

                fs = new KM();
                fs.Name = " Office class ";
                fs.No = "02";
                fs.Insert();
                i = base.RetrieveAll();
            }

            return i;
        }
    }
}
