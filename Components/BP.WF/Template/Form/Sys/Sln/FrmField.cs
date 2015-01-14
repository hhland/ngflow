using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.WF.Port;
using BP.WF;

namespace BP.Sys
{
	/// <summary>
	/// Frm Property 
	/// </summary>
    public class FrmFieldAttr : EntityNoNameAttr
    {
        /// <summary>
        ///  Field 
        /// </summary>
        public const string KeyOfEn = "KeyOfEn";
        /// <summary>
        /// FK_Node
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        ///  Process ID 
        /// </summary>
        public const string FK_Flow = "FK_Flow";
        /// <summary>
        /// FK_MapData
        /// </summary>
        public const string FK_MapData = "FK_MapData";
        /// <summary>
        ///  Are Required 
        /// </summary>
        public const string IsNotNull = "IsNotNull";
        /// <summary>
        ///  Regex 
        /// </summary>
        public const string RegularExp = "RegularExp";
        /// <summary>
        ///  Type 
        /// </summary>
        public const string EleType = "EleType";
        /// <summary>
        ///  Whether to write flow chart ?
        /// </summary>
        public const string IsWriteToFlowTable = "IsWriteToFlowTable";
    }
	/// <summary>
	///  Form field program 
	/// </summary>
    public class FrmField : EntityMyPK
    {
        #region  Basic properties 
        /// <summary>
        ///  Element Type .
        /// </summary>
        public string EleType
        {
            get
            {
                return this.GetValStringByKey(FrmFieldAttr.EleType);
            }
            set
            {
                this.SetValByKey(FrmFieldAttr.EleType, value);
            }
        }
        /// <summary>
        ///  Regex 
        /// </summary>
        public string RegularExp
        {
            get
            {
                return this.GetValStringByKey(FrmFieldAttr.RegularExp);
            }
            set
            {
                this.SetValByKey(FrmFieldAttr.RegularExp, value);
            }
        }
        public string Name
        {
            get
            {
                return this.GetValStringByKey(FrmFieldAttr.Name);
            }
            set
            {
                this.SetValByKey(FrmFieldAttr.Name, value);
            }
        }
        /// <summary>
        ///  Is empty 
        /// </summary>
        public bool IsNotNull
        {
            get
            {
                return this.GetValBooleanByKey(FrmFieldAttr.IsNotNull);
            }
            set
            {
                this.SetValByKey(FrmFieldAttr.IsNotNull, value);
            }
        }
        /// <summary>
        ///  Whether the write process data table 
        /// </summary>
        public bool IsWriteToFlowTable
        {
            get
            {
                return this.GetValBooleanByKey(FrmFieldAttr.IsWriteToFlowTable);
            }
            set
            {
                this.SetValByKey(FrmFieldAttr.IsWriteToFlowTable, value);
            }
        }
        
        /// <summary>
        ///  Form ID
        /// </summary>
        public string FK_MapData
        {
            get
            {
                return this.GetValStringByKey(FrmFieldAttr.FK_MapData);
            }
            set
            {
                this.SetValByKey(FrmFieldAttr.FK_MapData, value);
            }
        }
        /// <summary>
        ///  Field 
        /// </summary>
        public string KeyOfEn
        {
            get
            {
                return this.GetValStringByKey(FrmFieldAttr.KeyOfEn);
            }
            set
            {
                this.SetValByKey(FrmFieldAttr.KeyOfEn, value);
            }
        }
        /// <summary>
        ///  Process ID 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.GetValStringByKey(FrmFieldAttr.FK_Flow);
            }
            set
            {
                this.SetValByKey(FrmFieldAttr.FK_Flow, value);
            }
        }
        /// <summary>
        ///  Solutions 
        /// </summary>
        public int FK_Node
        {
            get
            {
                return this.GetValIntByKey(FrmFieldAttr.FK_Node);
            }
            set
            {
                this.SetValByKey(FrmFieldAttr.FK_Node, value);
            }
        }
        /// <summary>
        ///  Is visible 
        /// </summary>
        public bool UIVisible
        {
            get
            {
                return this.GetValBooleanByKey(MapAttrAttr.UIVisible);
            }
            set
            {
                this.SetValByKey(MapAttrAttr.UIVisible, value);
            }
        }
        /// <summary>
        ///  Is available 
        /// </summary>
        public bool UIIsEnable
        {
            get
            {
                return this.GetValBooleanByKey(MapAttrAttr.UIIsEnable);
            }
            set
            {
                this.SetValByKey(MapAttrAttr.UIIsEnable, value);
            }
        }
        public string DefVal
        {
            get
            {
                return this.GetValStringByKey(MapAttrAttr.DefVal);
            }
            set
            {
                this.SetValByKey(MapAttrAttr.DefVal, value);
            }
        }
        /// <summary>
        ///  Whether it is a digital signature ?
        /// </summary>
        public bool IsSigan
        {
            get
            {
                return this.GetValBooleanByKey(MapAttrAttr.IsSigan);
            }
            set
            {
                this.SetValByKey(MapAttrAttr.IsSigan, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Form field program 
        /// </summary>
        public FrmField()
        {
        }
        /// <summary>
        ///  Form field program 
        /// </summary>
        /// <param name="no"></param>
        public FrmField(string mypk)
            : base(mypk)
        {
        }
        /// <summary>
        ///  Override the base class methods 
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("Sys_FrmSln");

                map.EnDesc = " Form field program ";
                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.CodeStruct = "4";
                map.IsAutoGenerNo = false;

                map.AddMyPK();

                // This form corresponds to the form ID
                map.AddTBString(FrmFieldAttr.FK_Flow, null, " Process ID ", true, false, 0, 4, 4);
                map.AddTBInt(FrmFieldAttr.FK_Node, 0, " Node ", true, false);

                map.AddTBString(FrmFieldAttr.FK_MapData, null, " Form ID", true, false, 0, 100, 10);
                map.AddTBString(FrmFieldAttr.KeyOfEn, null, " Field ", true, false, 0, 200, 20);
                map.AddTBString(FrmFieldAttr.Name, null, " Field name ", true, false, 0, 500, 20);
                map.AddTBString(FrmFieldAttr.EleType, null, " Type ", true, false, 0, 20, 20);

                // Control content .
                map.AddBoolean(MapAttrAttr.UIIsEnable, true, " Is available ", true, true);
                map.AddBoolean(MapAttrAttr.UIVisible, true, " Is visible ", true, true);
                map.AddBoolean(MapAttrAttr.IsSigan, false, " Are Signed ", true, true);

                // Add 2013-12-26.
                map.AddTBInt(FrmFieldAttr.IsNotNull, 0, " Is empty ", true, false);
                map.AddTBString(FrmFieldAttr.RegularExp, null, " Regex ", true, false, 0, 500, 20);

                //  Whether to write flow chart ? 2014-01-26, In the case of , Is first written to the node data table , Then copy To process data table 
                //  When a node Send ccflow Automatically written , The purpose is to write 
                map.AddTBInt(FrmFieldAttr.IsWriteToFlowTable, 0, " Whether to write flow chart ", true, false);


                map.AddBoolean(MapAttrAttr.IsSigan, false, " Are Signed ", true, true);

                map.AddTBString(MapAttrAttr.DefVal, null, " Defaults ", true, false, 0, 200, 20);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        protected override bool beforeInsert()
        {
            if (string.IsNullOrEmpty(this.EleType))
                this.EleType = FrmEleType.Field;

            this.MyPK = this.FK_MapData + "_" + this.FK_Flow + "_" + this.FK_Node + "_" + this.KeyOfEn + "_" + this.EleType;
            return base.beforeInsert();
        }
    }
	/// <summary>
    ///  Form field program s
	/// </summary>
    public class FrmFields : EntitiesMyPK
    {
        public FrmFields()
        {
        }
        /// <summary>
        ///  Inquiry 
        /// </summary>
        public FrmFields(string fk_mapdata, int nodeID)
        {
            this.Retrieve(FrmFieldAttr.FK_MapData, fk_mapdata, 
                FrmFieldAttr.FK_Node, nodeID,FrmFieldAttr.EleType,  FrmEleType.Field);
        }
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new FrmField();
            }
        }
    }
}
