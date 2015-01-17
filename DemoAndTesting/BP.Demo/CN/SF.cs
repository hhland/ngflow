using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Port;

namespace BP.CN
{
    /// <summary>
    ///  Province 
    /// </summary>
    public class SFAttr : EntityNoNameAttr
    {
        public const string FK_PQ = "FK_PQ";
        public const string Names = "Names";
        public const string JC = "JC";
    }
    /// <summary>
    ///  Province 
    /// </summary>
    public class SF : EntityNoName
    {
        #region  Basic properties 
        /// <summary>
        ///  Area No. 
        /// </summary>
        public string FK_PQ
        {
            get
            {
                return this.GetValStrByKey(SFAttr.FK_PQ);
            }
            set
            {
                this.SetValByKey(SFAttr.FK_PQ, value);
            }
        }
        /// <summary>
        ///  Area Name 
        /// </summary>
        public string FK_PQT
        {
            get
            {
                return this.GetValRefTextByKey(SFAttr.FK_PQ);
            }
        }
        /// <summary>
        ///  Small name 
        /// </summary>
        public string Names
        {
            get
            {
                return this.GetValStrByKey(SFAttr.Names);
            }
            set
            {
                this.SetValByKey(SFAttr.Names, value);
            }
        }
        /// <summary>
        ///  Abbreviation 
        /// </summary>
        public string JC
        {
            get
            {
                return this.GetValStrByKey(SFAttr.JC);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Access .
        /// </summary>
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                uac.OpenForSysAdmin();
                return uac;
            }
        }
        /// <summary>
        ///  Province 
        /// </summary>		
        public SF() { }
        /// <summary>
        ///  Province 
        /// </summary>
        /// <param name="no"></param>
        public SF(string no)
            : base(no)
        {
        }
        /// <summary>
        /// Map
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;
                Map map = new Map();

                #region  Basic properties 
                map.EnDBUrl = new DBUrl(DBUrlType.AppCenterDSN);
                map.PhysicsTable = "CN_SF";
                map.AdjunctType = AdjunctType.AllType;
                map.DepositaryOfMap = Depositary.Application;
                map.DepositaryOfEntity = Depositary.None;
                map.IsCheckNoLength = false;
                map.EnDesc = " Province ";
                map.EnType = EnType.App;
                map.CodeStruct = "4";
                #endregion

                #region  Field 
                map.AddTBStringPK(SFAttr.No, null, " Serial number ", true, false, 2, 2, 2);
                map.AddTBString(SFAttr.Name, null, " Name ", true, false, 0, 200, 200);
                map.AddTBString(SFAttr.Names, null, " Small name ", true, false, 0, 200, 200);
                map.AddTBString(SFAttr.JC, null, " Abbreviation ", true, false, 0, 200, 200);
                map.AddDDLEntities(SFAttr.FK_PQ, null, " Area ", new PQs(), true);
                #endregion

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    ///  Province s
    /// </summary>
    public class SFs : EntitiesNoName
    {
        #region  Province .
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new SF();
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Province s
        /// </summary>
        public SFs() { }
        #endregion
    }
}
