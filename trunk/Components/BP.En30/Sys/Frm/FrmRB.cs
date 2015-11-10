using System;
using System.Collections;
using BP.DA;
using BP.En;
namespace BP.Sys
{
    /// <summary>
    ///  Single box 
    /// </summary>
    public class FrmRBAttr : EntityMyPKAttr
    {
        /// <summary>
        ///  Main table 
        /// </summary>
        public const string FK_MapData = "FK_MapData";
        /// <summary>
        /// X
        /// </summary>
        public const string X = "X";
        /// <summary>
        /// Y
        /// </summary>
        public const string Y = "Y";
        /// <summary>
        /// KeyOfEn
        /// </summary>
        public const string KeyOfEn = "KeyOfEn";
        /// <summary>
        /// IntKey
        /// </summary>
        public const string IntKey = "IntKey";
        /// <summary>
        /// EnumKey
        /// </summary>
        public const string EnumKey = "EnumKey";
        /// <summary>
        ///  Label 
        /// </summary>
        public const string Lab = "Lab";
        /// <summary>
        /// GUID
        /// </summary>
        public const string GUID = "GUID";
    }
    /// <summary>
    ///  Single box 
    /// </summary>
    public class FrmRB : EntityMyPK
    {
        #region  Property 
        public string Lab
        {
            get
            {
                return this.GetValStringByKey(FrmRBAttr.Lab);
            }
            set
            {
                this.SetValByKey(FrmRBAttr.Lab, value);
            }
        }
        public string KeyOfEn
        {
            get
            {
                return this.GetValStringByKey(FrmRBAttr.KeyOfEn);
            }
            set
            {
                this.SetValByKey(FrmRBAttr.KeyOfEn, value);
            }
        }
        public int IntKey
        {
            get
            {
                return this.GetValIntByKey(FrmRBAttr.IntKey);
            }
            set
            {
                this.SetValByKey(FrmRBAttr.IntKey, value);
            }
        }
        /// <summary>
        ///  Y
        /// </summary>
        public float Y
        {
            get
            {
                return this.GetValFloatByKey(FrmRBAttr.Y);
            }
            set
            {
                this.SetValByKey(FrmRBAttr.Y, value);
            }
        }
        public float X
        {
            get
            {
                return this.GetValFloatByKey(FrmRBAttr.X);
            }
            set
            {
                this.SetValByKey(FrmRBAttr.X, value);
            }
        }
        public string FK_MapData
        {
            get
            {
                return this.GetValStrByKey(FrmRBAttr.FK_MapData);
            }
            set
            {
                this.SetValByKey(FrmRBAttr.FK_MapData, value);
            }
        }
        public string EnumKey
        {
            get
            {
                return this.GetValStrByKey(FrmRBAttr.EnumKey);
            }
            set
            {
                this.SetValByKey(FrmRBAttr.EnumKey, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Single box 
        /// </summary>
        public FrmRB()
        {
        }
        public FrmRB(string mypk)
        {
            this.MyPK = mypk;
            this.Retrieve();
        }
        /// <summary>
        /// EnMap
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;
                Map map = new Map("Sys_FrmRB");
                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.EnDesc = " Single box ";
                map.EnType = EnType.Sys;

                map.AddMyPK();
                map.AddTBString(FrmRBAttr.FK_MapData, null, "FK_MapData", true, false, 0, 30, 20);
                map.AddTBString(FrmRBAttr.KeyOfEn, null, "KeyOfEn", true, false, 0, 30, 20);
                map.AddTBString(FrmRBAttr.EnumKey, null, "EnumKey", true, false, 0, 30, 20);
                map.AddTBString(FrmRBAttr.Lab, null, "Lab", true, false, 0, 90, 20);
                map.AddTBInt(FrmRBAttr.IntKey, 0, "IntKey", true, false);

                map.AddTBFloat(FrmRBAttr.X, 5, "X", true, false);
                map.AddTBFloat(FrmRBAttr.Y, 5, "Y", false, false);
                map.AddTBString(FrmBtnAttr.GUID, null, "GUID", true, false, 0, 128, 20);


                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        protected override bool beforeInsert()
        {
            this.MyPK = this.FK_MapData + "_" + this.KeyOfEn + "_" + this.IntKey;
            return base.beforeInsert();
        }

        protected override bool beforeUpdateInsertAction()
        {
            this.MyPK = this.FK_MapData + "_" + this.KeyOfEn + "_" + this.IntKey;
            return base.beforeUpdateInsertAction();
        }
    }
    /// <summary>
    ///  Single box s
    /// </summary>
    public class FrmRBs : EntitiesNoName
    {
        #region  Structure 
        /// <summary>
        ///  Single box s
        /// </summary>
        public FrmRBs()
        {
        }
        /// <summary>
        ///  Single box s
        /// </summary>
        /// <param name="fk_mapdata">s</param>
        public FrmRBs(string fk_mapdata)
        {
            if (SystemConfig.IsDebug)
                this.Retrieve(FrmLineAttr.FK_MapData, fk_mapdata);
            else
                this.RetrieveFromCash(FrmLineAttr.FK_MapData, (object)fk_mapdata);
        }
        /// <summary>
        ///  Get it  Entity
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new FrmRB();
            }
        }
        #endregion
    }
}
