using System;
using System.Collections;
using BP.DA;
using BP.Sys;
using BP.En;
using BP.WF.Port;
using BP.WF.Template;
using BP.WF;

namespace BP.Sys
{
	/// <summary>
	/// Frm Property 
	/// </summary>
    public class SysFormAttr : EntityNoNameAttr
    {
        /// <summary>
        ///  Run Type 
        /// </summary>
        public const string FormRunType = "FormRunType";
        /// <summary>
        ///  Form tree 
        /// </summary>
        public const string FK_FormTree = "FK_FormTree";
        /// <summary>
        /// URL
        /// </summary>
        public const string URL = "URL";
        /// <summary>
        /// PTable
        /// </summary>
        public const string PTable = "PTable";
        /// <summary>
        /// DBURL
        /// </summary>
        public const string DBURL = "DBURL";
    }
	/// <summary>
	///  System Form 
	/// </summary>
    public class SysForm : EntityNoName
    {
        #region  Basic properties 
        public string PTable
        {
            get
            {
                return this.GetValStringByKey(SysFormAttr.PTable);
            }
            set
            {
                this.SetValByKey(SysFormAttr.PTable, value);
            }
        }
        public string URL
        {
            get
            {
                return this.GetValStringByKey(SysFormAttr.URL);
            }
            set
            {
                this.SetValByKey(SysFormAttr.URL, value);
            }
        }
        public FormRunType HisFormRunType
        {
            get
            {
                return (FormRunType)this.GetValIntByKey(SysFormAttr.FormRunType);
            }
            set
            {
                this.SetValByKey(SysFormAttr.FormRunType, (int)value);
            }
        }
        public string FK_FormTree
        {
            get
            {
                return this.GetValStringByKey(SysFormAttr.FK_FormTree);
            }
            set
            {
                this.SetValByKey(SysFormAttr.FK_FormTree, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        /// Frm
        /// </summary>
        public SysForm()
        {
        }
        /// <summary>
        /// Frm
        /// </summary>
        /// <param name="no"></param>
        public SysForm(string no)
            : base(no)
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

                Map map = new Map("Sys_MapData");

                map.EnDesc = " System Form ";
                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.CodeStruct = "4";
                map.IsAutoGenerNo = false;

                map.AddTBStringPK(SysFormAttr.No, null, null, true, true, 1, 4, 4);
                map.AddTBString(SysFormAttr.Name, null, null, true, false, 0, 200, 10);

                // Type of operation form .
                map.AddDDLSysEnum(SysFormAttr.FormRunType, (int)BP.WF.FormRunType.FreeForm, " Run Type ",
                    true, false, SysFormAttr.FormRunType, "@0= Fool form @1= Freedom Form @2= Custom Form @4=Silverlight Form ");

                // The form the corresponding physical table 
                map.AddTBString(SysFormAttr.PTable, null, " Physical table ", true, false, 0, 200, 10);

                // FormRunType= Custom form ,  This field is valid . 
                map.AddTBString(SysFormAttr.URL, null, "Url", true, false, 0, 200, 10);

                // System Form Category .
                map.AddTBString(SysFormAttr.FK_FormTree, null, " Form tree ", true, false, 0, 10, 20);

                map.AddTBInt(Sys.MapDataAttr.FrmW, 900, " System Form Width ", true, false);
                map.AddTBInt(Sys.MapDataAttr.FrmH, 1200, " System Form height ", true, false);

                this._enMap = map;
                return this._enMap;
            }
        }
        public int FrmW
        {
            get
            {
                return this.GetValIntByKey(Sys.MapDataAttr.FrmW);
            }
        }
        public int FrmH
        {
            get
            {
                return this.GetValIntByKey(Sys.MapDataAttr.FrmH);
            }
        }
        
        #endregion
    }
	/// <summary>
    ///  System Form s
	/// </summary>
    public class SysForms : EntitiesNoName
    {
        /// <summary>
        /// Frm
        /// </summary>
        public SysForms()
        {

        }
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new SysForm();
            }
        }
    }
}
