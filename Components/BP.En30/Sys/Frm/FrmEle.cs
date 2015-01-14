using System;
using System.Collections;
using BP.DA;
using BP.En;
namespace BP.Sys
{
    /// <summary>
    ///  Extended form elements 
    /// </summary>
    public class FrmEleAttr : EntityMyPKAttr
    {
        /// <summary>
        /// IsEnable
        /// </summary>
        public const string IsEnable = "IsEnable";
        /// <summary>
        /// EleType
        /// </summary>
        public const string EleType = "EleType";
        /// <summary>
        /// EleID
        /// </summary>
        public const string EleID = "EleID";
        /// <summary>
        /// EleName
        /// </summary>
        public const string EleName = "EleName";
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
        /// H
        /// </summary>
        public const string H = "H";
        /// <summary>
        /// W
        /// </summary>
        public const string W = "W";
        /// <summary>
        /// Tag1
        /// </summary>
        public const string Tag1 = "Tag1";
        /// <summary>
        /// Tag2
        /// </summary>
        public const string Tag2 = "Tag2";
        /// <summary>
        /// Tag3
        /// </summary>
        public const string Tag3 = "Tag3";
        /// <summary>
        /// Tag4
        /// </summary>
        public const string Tag4 = "Tag4";
        /// <summary>
        /// GUID
        /// </summary>
        public const string GUID = "GUID";
    }
    /// <summary>
    ///  Extended form elements 
    /// </summary>
    public class FrmEle : EntityMyPK
    {
        #region   Signature storage location .
        public string HandSigantureSavePath
        {
            get
            {
                return this.GetValStrByKey(FrmEleAttr.Tag1);
            }
        }
        public string HandSiganture_WinOpenH
        {
            get
            {
                return this.GetValStrByKey(FrmEleAttr.Tag2);
            }
        }
        public string HandSiganture_WinOpenW
        {
            get
            {
                return this.GetValStrByKey(FrmEleAttr.Tag3);
            }
        }
        public string HandSiganture_UrlPath
        {
            get
            {
                return this.GetValStrByKey(FrmEleAttr.Tag4);
            }
        }
        #endregion  HandSigantureSavePath

        #region  Type 
        /// <summary>
        ///  Hand Signed 
        /// </summary>
        public const string HandSiganture = "HandSiganture";
        /// <summary>
        ///  Electronic Signature 
        /// </summary>
        public const string EleSiganture = "EleSiganture";
        /// <summary>
        ///  Web framework 
        /// </summary>
        public const string iFrame = "iFrame";
        #endregion  Type 

        #region  Property 
        /// <summary>
        ///  Whether hired 
        /// </summary>
        public bool IsEnable
        {
            get
            {
                return this.GetValBooleanByKey(FrmEleAttr.IsEnable);
            }
            set
            {
                this.SetValByKey(FrmEleAttr.IsEnable, value);
            }
        }
        /// <summary>
        /// EleID
        /// </summary>
        public string EleID
        {
            get
            {
                return this.GetValStrByKey(FrmEleAttr.EleID);
            }
            set
            {
                this.SetValByKey(FrmEleAttr.EleID, value);
            }
        }
        /// <summary>
        /// EleName
        /// </summary>
        public string EleName
        {
            get
            {
                return this.GetValStringByKey(FrmEleAttr.EleName);
            }
            set
            {
                this.SetValByKey(FrmEleAttr.EleName, value);
            }
        }
        /// <summary>
        /// Tag1
        /// </summary>
        public string Tag1
        {
            get
            {
                return this.GetValStringByKey(FrmEleAttr.Tag1);
            }
            set
            {
                this.SetValByKey(FrmEleAttr.Tag1, value);
            }
        }
        /// <summary>
        /// Tag2
        /// </summary>
        public string Tag2
        {
            get
            {
                return this.GetValStringByKey(FrmEleAttr.Tag2);
            }
            set
            {
                this.SetValByKey(FrmEleAttr.Tag2, value);
            }
        }
        /// <summary>
        /// Tag3
        /// </summary>
        public string Tag3
        {
            get
            {
                return this.GetValStringByKey(FrmEleAttr.Tag3);
            }
            set
            {
                this.SetValByKey(FrmEleAttr.Tag3, value);
            }
        }
        public string Tag4
        {
            get
            {
                return this.GetValStringByKey(FrmEleAttr.Tag4);
            }
            set
            {
                this.SetValByKey(FrmEleAttr.Tag4, value);
            }
        }
      
        /// <summary>
        /// FK_MapData
        /// </summary>
        public string FK_MapData
        {
            get
            {
                return this.GetValStrByKey(FrmEleAttr.FK_MapData);
            }
            set
            {
                this.SetValByKey(FrmEleAttr.FK_MapData, value);
            }
        }
        /// <summary>
        /// EleType
        /// </summary>
        public string EleType
        {
            get
            {
                return this.GetValStrByKey(FrmEleAttr.EleType);
            }
            set
            {
                this.SetValByKey(FrmEleAttr.EleType, value);
            }
        }
        public float X
        {
            get
            {
                return this.GetValFloatByKey(FrmEleAttr.X);
            }
            set
            {
                this.SetValByKey(FrmEleAttr.X, value);
            }
        }
        public float Y
        {
            get
            {
                return this.GetValFloatByKey(FrmEleAttr.Y);
            }
            set
            {
                this.SetValByKey(FrmEleAttr.Y, value);
            }
        }
        public float H
        {
            get
            {
                return this.GetValFloatByKey(FrmEleAttr.H);
            }
            set
            {
                this.SetValByKey(FrmEleAttr.H, value);
            }
        }
        public float W
        {
            get
            {
                return this.GetValFloatByKey(FrmEleAttr.W);
            }
            set
            {
                this.SetValByKey(FrmEleAttr.W, value);
            }
        }

        public int HOfInt
        {
            get
            {
                return int.Parse(this.H.ToString("0"));
            }
        }
        public int WOfInt
        {
            get
            {
                return int.Parse(this.W.ToString("0"));
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Extended form elements 
        /// </summary>
        public FrmEle()
        {
        }
        /// <summary>
        ///  Extended form elements 
        /// </summary>
        /// <param name="mypk"></param>
        public FrmEle(string mypk)
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
                Map map = new Map("Sys_FrmEle");
                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.EnDesc = " Extended form elements ";
                map.EnType = EnType.Sys;

                map.AddMyPK();

                map.AddTBString(FrmEleAttr.FK_MapData, null, "FK_MapData", true, false, 1, 30, 20);
                map.AddTBString(FrmEleAttr.EleType, null, "EleType Scalable ", true, false, 0, 200, 20);
                map.AddTBString(FrmEleAttr.EleID, null, "EleID", true, false, 0, 200, 20);
                map.AddTBString(FrmEleAttr.EleName, null, " Name ", true, false, 0, 200, 20);

                map.AddTBFloat(FrmEleAttr.X, 5, "X", true, false);
                map.AddTBFloat(FrmEleAttr.Y, 5, "Y", false, false);

                map.AddTBFloat(FrmEleAttr.H, 20, "H", true, false);
                map.AddTBFloat(FrmEleAttr.W, 20, "W", false, false);

                map.AddTBInt(FrmEleAttr.IsEnable, 1, "IsEnable", false, false);

                map.AddTBString(FrmEleAttr.Tag1, null, "Tag1", true, false, 0, 200, 20);
                map.AddTBString(FrmEleAttr.Tag2, null, "Tag2", true, false, 0, 200, 20);
                map.AddTBString(FrmEleAttr.Tag3, null, "Tag3", true, false, 0, 200, 20);
                map.AddTBString(FrmEleAttr.Tag4, null, "Tag4", true, false, 0, 200, 20);

                map.AddTBString(FrmBtnAttr.GUID, null, "GUID", true, false, 0, 128, 20);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        protected override bool beforeUpdateInsertAction()
        {
            this.MyPK = this.FK_MapData + "_" + this.EleType + "_" + this.EleID;
            return base.beforeUpdateInsertAction();
        }
    }
    /// <summary>
    ///  Extended form elements s
    /// </summary>
    public class FrmEles : EntitiesMyPK
    {
        #region  Structure 
        /// <summary>
        ///  Extended form elements s
        /// </summary>
        public FrmEles()
        {
        }
        /// <summary>
        ///  Extended form elements s
        /// </summary>
        /// <param name="fk_mapdata">s</param>
        public FrmEles(string fk_mapdata)
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
                return new FrmEle();
            }
        }
        #endregion
    }
}
