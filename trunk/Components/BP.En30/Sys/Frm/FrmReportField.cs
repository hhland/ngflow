using System;
using System.Collections.Generic;
using System.Text;
using BP.DA;
using BP.En;

namespace BP.Sys
{
    /// <summary>
    ///  Form Report Settings Properties 
    /// </summary>
    public class FrmReportFieldAttr : EntityMyPKAttr
    {
        /// <summary>
        ///  Form Number 
        /// </summary>
        public const string FK_MapData = "FK_MapData";
        /// <summary>
        ///  Field name 
        /// </summary>
        public const string KeyOfEn = "KeyOfEn";
        /// <summary>
        ///  Display Chinese name 
        /// </summary>
        public const string Name = "Name";
        /// <summary>
        ///  Column Width 
        /// </summary>
        public const string UIWidth = "UIWidth";
        /// <summary>
        ///  Whether to display 
        /// </summary>
        public const string UIVisible = "UIVisible";
        /// <summary>
        ///  Display Order 
        /// </summary>
        public const string Idx = "Idx";
    }

    /// <summary>
    ///  Report Settings form data storage table 
    /// </summary>
    public class FrmReportField : EntityMyPK
    {
        #region  Property 
        /// <summary>
        ///  Form Number 
        /// </summary>
        public string FK_MapData
        {
            get
            {
                return this.GetValStrByKey(FrmReportFieldAttr.FK_MapData);
            }
            set
            {
                this.SetValByKey(FrmReportFieldAttr.FK_MapData, value);
            }
        }
        /// <summary>
        ///  Field name 
        /// </summary>
        public string KeyOfEn
        {
            get
            {
                return this.GetValStrByKey(FrmReportFieldAttr.KeyOfEn);
            }
            set
            {
                this.SetValByKey(FrmReportFieldAttr.KeyOfEn, value);
            }
        }
        /// <summary>
        ///  Display Chinese name 
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetValStrByKey(FrmReportFieldAttr.Name);
            }
            set
            {
                this.SetValByKey(FrmReportFieldAttr.Name, value);
            }
        }
        /// <summary>
        ///  Column Width 
        /// </summary>
        public string UIWidth
        {
            get
            {
                return this.GetValStrByKey(FrmReportFieldAttr.UIWidth);
            }
            set
            {
                this.SetValByKey(FrmReportFieldAttr.UIWidth, value);
            }
        }
        /// <summary>
        ///  Whether to display 
        /// </summary>
        public bool UIVisible
        {
            get
            {
                return this.GetValBooleanByKey(FrmReportFieldAttr.UIVisible);
            }
            set
            {
                this.SetValByKey(FrmReportFieldAttr.UIVisible, value);
            }
        }
        /// <summary>
        ///  Display Order 
        /// </summary>
        public int Idx
        {
            get
            {
                return this.GetValIntByKey(FrmReportFieldAttr.Idx);
            }
            set
            {
                this.SetValByKey(FrmReportFieldAttr.Idx, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Reporting Form 
        /// </summary>
        public FrmReportField()
        {
        }
        /// <summary>
        ///  Reporting Form 
        /// </summary>
        /// <param name="mypk"></param>
        public FrmReportField(string mypk)
        {
            this.MyPK = mypk;
            this.Retrieve();
        }
        #endregion

        /// <summary>
        /// EnMap
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;
                Map map = new Map("Sys_FrmRePortField");
                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.EnDesc = " Reporting Form ";
                map.EnType = EnType.Sys;
                map.AddMyPK();
                map.AddTBString(FrmReportFieldAttr.FK_MapData, null, " Form Number ", true, false, 1, 30, 80);
                map.AddTBString(FrmReportFieldAttr.KeyOfEn, null, " Field name ", true, false,1,100,100);
                map.AddTBString(FrmReportFieldAttr.Name, null, " Display Chinese name ", true, false, 1, 200, 200);
                map.AddTBString(FrmReportFieldAttr.UIWidth, "0", " Width ", true, false, 1, 100, 100);
                map.AddBoolean(FrmReportFieldAttr.UIVisible, true, " Whether to display ", true, true);
                map.AddTBInt(FrmReportFieldAttr.Idx, 0, " Display Order ", true, false);
                this._enMap = map;
                return this._enMap;
            }
        }

        protected override bool beforeUpdateInsertAction()
        {
            this.MyPK = this.FK_MapData + "_" + this.KeyOfEn;
            return base.beforeUpdateInsertAction();
        }
    }

    /// <summary>
    ///  Report Settings form data storage table s
    /// </summary>
    public class FrmReportFields : EntitiesMyPK
    {
        #region  Structure 
        /// <summary>
        ///  Reporting Form s
        /// </summary>
        public FrmReportFields()
        {
        }
        /// <summary>
        ///  Reporting Form s
        /// </summary>
        /// <param name="fk_mapdata">s</param>
        public FrmReportFields(string fk_mapdata)
        {
            if (SystemConfig.IsDebug)
                this.Retrieve(FrmReportFieldAttr.FK_MapData, fk_mapdata);
            else
                this.RetrieveFromCash(FrmReportFieldAttr.FK_MapData, (object)fk_mapdata);
        }
        /// <summary>
        ///  Get it  Entity
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new FrmReportField();
            }
        }
        #endregion
    }
}
