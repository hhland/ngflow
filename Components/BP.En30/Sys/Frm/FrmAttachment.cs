using System;
using System.Collections;
using BP.DA;
using BP.En;
namespace BP.Sys
{
    public enum AthCtrlWay
    {
        /// <summary>
        ///  Form the primary key 
        /// </summary>
        PK,
        /// <summary>
        /// FID
        /// </summary>
        FID,
        /// <summary>
        ///  Parent process ID
        /// </summary>
        PWorkID
    }
    /// <summary>
    ///  Accessories upload type 
    /// </summary>
    public enum AttachmentUploadType
    {
        /// <summary>
        ///  Single 
        /// </summary>
        Single,
        /// <summary>
        ///  Multiple 
        /// </summary>
        Multi,
        /// <summary>
        ///  Specified 
        /// </summary>
        Specifically
    }
    /// <summary>
    ///  Attachment uploads 
    /// </summary>
    public enum AthUploadWay
    {
        /// <summary>
        ///  Inheritance 
        /// </summary>
        Inherit,
        /// <summary>
        ///  Collaborative model 
        /// </summary>
        Interwork
    }
    /// <summary>
    ///  Files show the way 
    /// </summary>
    public enum FileShowWay
    {
        /// <summary>
        ///  Form 
        /// </summary>
        Table,
        /// <summary>
        ///  Picture 
        /// </summary>
        Pict,
        /// <summary>
        ///  Free Mode 
        /// </summary>
        Free
    }
    
    /// <summary>
    ///  Accessory 
    /// </summary>
    public class FrmAttachmentAttr : EntityMyPKAttr
    {
        /// <summary>
        /// Name
        /// </summary>
        public const string Name = "Name";
        /// <summary>
        ///  Main table 
        /// </summary>
        public const string FK_MapData = "FK_MapData";
        /// <summary>
        ///  Node ID
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        /// X
        /// </summary>
        public const string X = "X";
        /// <summary>
        /// Y
        /// </summary>
        public const string Y = "Y";
        /// <summary>
        ///  Width 
        /// </summary>
        public const string W = "W";
        /// <summary>
        ///  Height 
        /// </summary>
        public const string H = "H";
        /// <summary>
        ///  The format required to upload 
        /// </summary>
        public const string Exts = "Exts";
        /// <summary>
        ///  Annex No. 
        /// </summary>
        public const string NoOfObj = "NoOfObj";
        /// <summary>
        ///  Can upload 
        /// </summary>
        public const string IsUpload = "IsUpload";
        /// <summary>
        ///  Whether to increase 
        /// </summary>
        public const string IsNote = "IsNote";
        /// <summary>
        ///  Whether you can delete 
        /// </summary>
        public const string IsDelete = "IsDelete";
        /// <summary>
        ///  Whether to display the title bar 
        /// </summary>
        public const string IsShowTitle = "IsShowTitle";
        /// <summary>
        ///  Can I download 
        /// </summary>
        public const string IsDownload = "IsDownload";
        /// <summary>
        ///  Save to 
        /// </summary>
        public const string SaveTo = "SaveTo";
        /// <summary>
        ///  Category 
        /// </summary>
        public const string Sort = "Sort";
        /// <summary>
        ///  Upload type 
        /// </summary>
        public const string UploadType = "UploadType";
        /// <summary>
        /// RowIdx
        /// </summary>
        public const string RowIdx = "RowIdx";
        /// <summary>
        /// GroupID
        /// </summary>
        public const string GroupID = "GroupID";
        /// <summary>
        ///  Automatic control the size 
        /// </summary>
        public const string IsAutoSize = "IsAutoSize";
        /// <summary>
        /// GUID
        /// </summary>
        public const string GUID = "GUID";
        /// <summary>
        ///  Data control ( Father and son have the effect of process )
        /// </summary>
        public const string CtrlWay = "CtrlWay";
        /// <summary>
        ///  Uploads ( Father and son have the effect of process )
        /// </summary>
        public const string AthUploadWay = "AthUploadWay";
        /// <summary>
        ///  Files show the way 
        /// </summary>
        public const string FileShowWay = "FileShowWay";


        #region weboffice Property .
        /// <summary>
        ///  Whether to enable the locked row 
        /// </summary>
        public const string IsRowLock = "IsRowLock";
        /// <summary>
        ///  Whether to enable weboffice
        /// </summary>
        public const string IsWoEnableWF = "IsWoEnableWF";

        /// <summary>
        ///  Whether saving enabled 
        /// </summary>
        public const string IsWoEnableSave = "IsWoEnableSave";
        /// <summary>
        ///  Is read-only 
        /// </summary>
        public const string IsWoEnableReadonly = "IsWoEnableReadonly";
        /// <summary>
        ///  Whether to amend enabled 
        /// </summary>
        public const string IsWoEnableRevise = "IsWoEnableRevise";
        /// <summary>
        ///  View whether a user traces 
        /// </summary>
        public const string IsWoEnableViewKeepMark = "IsWoEnableViewKeepMark";
        /// <summary>
        ///  Whether to print 
        /// </summary>
        public const string IsWoEnablePrint = "IsWoEnablePrint";
        /// <summary>
        ///  Whether the signature is enabled 
        /// </summary>
        public const string IsWoEnableSeal = "IsWoEnableSeal";
        /// <summary>
        ///  Whether Taohong enabled 
        /// </summary>
        public const string IsWoEnableOver = "IsWoEnableOver";
        /// <summary>
        ///  Whether to enable document template 
        /// </summary>
        public const string IsWoEnableTemplete = "IsWoEnableTemplete";
        /// <summary>
        ///  Whether the information is automatically written to the audit 
        /// </summary>
        public const string IsWoEnableCheck = "IsWoEnableCheck";
        /// <summary>
        ///  Whether insertion process 
        /// </summary>
        public const string IsWoEnableInsertFlow = "IsWoEnableInsertFlow";
        /// <summary>
        ///  The insertion point whether the risk 
        /// </summary>
        public const string IsWoEnableInsertFengXian = "IsWoEnableInsertFengXian";
        /// <summary>
        ///  Whether the traces mode is enabled 
        /// </summary>
        public const string IsWoEnableMarks = "IsWoEnableMarks";
        /// <summary>
        ///  Whether to download enabled 
        /// </summary>
        public const string IsWoEnableDown = "IsWoEnableDown";



  
        #endregion weboffice Property .

        #region  Shortcuts .
        /// <summary>
        ///  Whether to enable shortcuts 
        /// </summary>
        public const string FastKeyIsEnable = "FastKeyIsEnable";
        /// <summary>
        ///  Shortcuts generation rules 
        /// </summary>
        public const string FastKeyGenerRole = "FastKeyGenerRole";
        #endregion
    }
    /// <summary>
    ///  Accessory 
    /// </summary>
    public class FrmAttachment : EntityMyPK
    {
        #region  Property 
        /// <summary>
        ///  Node number 
        /// </summary>
        public int FK_Node
        {
            get
            {
                return this.GetValIntByKey(FrmAttachmentAttr.FK_Node);
            }
            set
            {
                this.SetValByKey(FrmAttachmentAttr.FK_Node, value);
            }
        }
        /// <summary>
        ///  Upload type £¨ Single , Multiple , Specified £©
        /// </summary>
        public AttachmentUploadType UploadType
        {
            get
            {
                return (AttachmentUploadType)this.GetValIntByKey(FrmAttachmentAttr.UploadType);
            }
            set
            {
                this.SetValByKey(FrmAttachmentAttr.UploadType, (int)value);
            }
        }
        /// <summary>
        ///  Type Name 
        /// </summary>
        public string UploadTypeT
        {
            get
            {
                if (this.UploadType == AttachmentUploadType.Multi)
                    return " More Accessories ";
                if (this.UploadType == AttachmentUploadType.Single)
                    return " Single attachment ";
                if (this.UploadType == AttachmentUploadType.Specifically)
                    return " Specified ";
                return "XXXXX";
            }
        }
        /// <summary>
        ///  Can upload 
        /// </summary>
        public bool IsUpload
        {
            get
            {
                return this.GetValBooleanByKey(FrmAttachmentAttr.IsUpload);
            }
            set
            {
                this.SetValByKey(FrmAttachmentAttr.IsUpload, value);
            }
        }
        /// <summary>
        ///  Can I download 
        /// </summary>
        public bool IsDownload
        {
            get
            {
                return this.GetValBooleanByKey(FrmAttachmentAttr.IsDownload);
            }
            set
            {
                this.SetValByKey(FrmAttachmentAttr.IsDownload, value);
            }
        }
        /// <summary>
        ///  Whether you can delete 
        /// </summary>
        public bool IsDelete
        {
            get
            {
                return this.GetValBooleanByKey(FrmAttachmentAttr.IsDelete);
            }
            set
            {
                this.SetValByKey(FrmAttachmentAttr.IsDelete, value);
            }
        }
        /// <summary>
        ///  Automatic control the size 
        /// </summary>
        public bool IsAutoSize
        {
            get
            {
                return this.GetValBooleanByKey(FrmAttachmentAttr.IsAutoSize);
            }
            set
            {
                this.SetValByKey(FrmAttachmentAttr.IsAutoSize, value);
            }
        }
        /// <summary>
        /// IsShowTitle
        /// </summary>
        public bool IsShowTitle
        {
            get
            {
                return this.GetValBooleanByKey(FrmAttachmentAttr.IsShowTitle);
            }
            set
            {
                this.SetValByKey(FrmAttachmentAttr.IsShowTitle, value);
            }
        }
        /// <summary>
        ///  Remarks column 
        /// </summary>
        public bool IsNote
        {
            get
            {
                return this.GetValBooleanByKey(FrmAttachmentAttr.IsNote);
            }
            set
            {
                this.SetValByKey(FrmAttachmentAttr.IsNote, value);
            }
        }
        /// <summary>
        ///  Attachment Name 
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetValStringByKey(FrmAttachmentAttr.Name);
            }
            set
            {
                this.SetValByKey(FrmAttachmentAttr.Name, value);
            }
        }
        /// <summary>
        ///  Category 
        /// </summary>
        public string Sort
        {
            get
            {
                return this.GetValStringByKey(FrmAttachmentAttr.Sort);
            }
            set
            {
                this.SetValByKey(FrmAttachmentAttr.Sort, value);
            }
        }
        /// <summary>
        ///  Required format 
        /// </summary>
        public string Exts
        {
            get
            {
                return this.GetValStringByKey(FrmAttachmentAttr.Exts);
            }
            set
            {
                this.SetValByKey(FrmAttachmentAttr.Exts, value);
            }
        }
        public string SaveTo
        {
            get
            {
                string s = this.GetValStringByKey(FrmAttachmentAttr.SaveTo);
                if (s == "" || s == null)
                    s = SystemConfig.PathOfDataUser + @"\UploadFile\" + this.FK_MapData + "\\";
                return s;
            }
            set
            {
                this.SetValByKey(FrmAttachmentAttr.SaveTo, value);
            }
        }
        /// <summary>
        ///  Annex No. 
        /// </summary>
        public string NoOfObj
        {
            get
            {
                return this.GetValStringByKey(FrmAttachmentAttr.NoOfObj);
            }
            set
            {
                this.SetValByKey(FrmAttachmentAttr.NoOfObj, value);
            }
        }
        /// <summary>
        /// Y
        /// </summary>
        public float Y
        {
            get
            {
                return this.GetValFloatByKey(FrmAttachmentAttr.Y);
            }
            set
            {
                this.SetValByKey(FrmAttachmentAttr.Y, value);
            }
        }
        /// <summary>
        /// X
        /// </summary>
        public float X
        {
            get
            {
                return this.GetValFloatByKey(FrmAttachmentAttr.X);
            }
            set
            {
                this.SetValByKey(FrmAttachmentAttr.X, value);
            }
        }
        /// <summary>
        /// W
        /// </summary>
        public float W
        {
            get
            {
                return this.GetValFloatByKey(FrmAttachmentAttr.W);
            }
            set
            {
                this.SetValByKey(FrmAttachmentAttr.W, value);
            }
        }
        /// <summary>
        /// H
        /// </summary>
        public float H
        {
            get
            {
                return this.GetValFloatByKey(FrmAttachmentAttr.H);
            }
            set
            {
                this.SetValByKey(FrmAttachmentAttr.H, value);
            }
        }
        public int RowIdx
        {
            get
            {
                return this.GetValIntByKey(FrmAttachmentAttr.RowIdx);
            }
            set
            {
                this.SetValByKey(FrmAttachmentAttr.RowIdx, value);
            }
        }
        public int GroupID
        {
            get
            {
                return this.GetValIntByKey(FrmAttachmentAttr.GroupID);
            }
            set
            {
                this.SetValByKey(FrmAttachmentAttr.GroupID, value);
            }
        }
        /// <summary>
        ///  Data control 
        /// </summary>
        public AthCtrlWay HisCtrlWay
        {
            get
            {
                return (AthCtrlWay)this.GetValIntByKey(FrmAttachmentAttr.CtrlWay);
            }
            set
            {
                this.SetValByKey(FrmAttachmentAttr.CtrlWay, (int)value);
            }
        }
        /// <summary>
        ///  Files show the way 
        /// </summary>
        public FileShowWay FileShowWay
        {
            get
            {
                return (FileShowWay)this.GetParaInt(FrmAttachmentAttr.FileShowWay);
            }
            set
            {
                this.SetPara(FrmAttachmentAttr.FileShowWay, (int)value);
            }
        }
        /// <summary>
        ///  Uploads £¨ Effective processes for father and son £©
        /// </summary>
        public AthUploadWay AthUploadWay
        {
            get
            {
                return (AthUploadWay)this.GetValIntByKey(FrmAttachmentAttr.AthUploadWay);
            }
            set
            {
                this.SetValByKey(FrmAttachmentAttr.AthUploadWay, (int)value);
            }
        }
        /// <summary>
        /// FK_MapData
        /// </summary>
        public string FK_MapData
        {
            get
            {
                return this.GetValStrByKey(FrmAttachmentAttr.FK_MapData);
            }
            set
            {
                this.SetValByKey(FrmAttachmentAttr.FK_MapData, value);
            }
        }
        #endregion

        #region weboffice Document Properties ( Parameter Properties )
     
        /// <summary>
        ///  Whether to enable the locked row 
        /// </summary>
        public bool IsRowLock
        {
            get
            {
                return this.GetParaBoolen(FrmAttachmentAttr.IsRowLock,false);
            }
            set
            {
                this.SetPara(FrmAttachmentAttr.IsRowLock, value);
            }
        }
      
      /// <summary>
      ///  Whether printing is enabled 
      /// </summary>
        public bool IsWoEnablePrint
        {
            get
            {
                return this.GetParaBoolen(FrmAttachmentAttr.IsWoEnablePrint);
            }
            set
            {
                this.SetPara(FrmAttachmentAttr.IsWoEnablePrint, value);
            }
        }
        /// <summary>
        ///  Whether to enable read-only 
        /// </summary>
        public bool IsWoEnableReadonly
        {
            get
            {
                return this.GetParaBoolen(FrmAttachmentAttr.IsWoEnableReadonly);
            }
            set
            {
                this.SetPara(FrmAttachmentAttr.IsWoEnableReadonly, value);
            }
        }
        /// <summary>
        ///  Whether to amend enabled 
        /// </summary>
        public bool IsWoEnableRevise
        {
            get
            {
                return this.GetParaBoolen(FrmAttachmentAttr.IsWoEnableRevise);
            }
            set
            {
                this.SetPara(FrmAttachmentAttr.IsWoEnableRevise, value);
            }
        }
        /// <summary>
        ///  Whether saving enabled 
        /// </summary>
        public bool IsWoEnableSave
        {
            get
            {
                return this.GetParaBoolen(FrmAttachmentAttr.IsWoEnableSave);
            }
            set
            {
                this.SetPara(FrmAttachmentAttr.IsWoEnableSave, value);
            }
        }
        /// <summary>
        ///  View whether a user traces 
        /// </summary>
        public bool IsWoEnableViewKeepMark
        {
            get
            {
                return this.GetParaBoolen(FrmAttachmentAttr.IsWoEnableViewKeepMark);
            }
            set
            {
                this.SetPara(FrmAttachmentAttr.IsWoEnableViewKeepMark, value);
            }
        }
      /// <summary>
      ///  Whether to enable weboffice
      /// </summary>
        public bool IsWoEnableWF
        {
            get
            {
                return this.GetParaBoolen(FrmAttachmentAttr.IsWoEnableWF);
            }
            set
            {
                this.SetPara(FrmAttachmentAttr.IsWoEnableWF, value);
            }
        }

        /// <summary>
        ///  Whether Taohong enabled 
        /// </summary>
        public bool IsWoEnableOver
        {
            get
            {
                return this.GetParaBoolen(FrmAttachmentAttr.IsWoEnableOver);
            }
            set
            {
                this.SetPara(FrmAttachmentAttr.IsWoEnableOver, value);
            }
        }

        /// <summary>
        ///  Whether the signature is enabled 
        /// </summary>
        public bool IsWoEnableSeal
        {
            get
            {
                return this.GetParaBoolen(FrmAttachmentAttr.IsWoEnableSeal);
            }
            set
            {
                this.SetPara(FrmAttachmentAttr.IsWoEnableSeal, value);
            }
        }

        /// <summary>
        ///  Whether to enable document template 
        /// </summary>
        public bool IsWoEnableTemplete
        {
            get
            {
                return this.GetParaBoolen(FrmAttachmentAttr.IsWoEnableTemplete);
            }
            set
            {
                this.SetPara(FrmAttachmentAttr.IsWoEnableTemplete, value);
            }
        }

         /// <summary>
        ///  Whether the record node information 
        /// </summary>
        public bool IsWoEnableCheck
        {
            get
            {
                return this.GetParaBoolen(FrmAttachmentAttr.IsWoEnableCheck);
            }
            set
            {
                this.SetPara(FrmAttachmentAttr.IsWoEnableCheck, value);
            }
        }

        /// <summary>
        ///  Whether to insert a flow chart 
        /// </summary>
        public bool IsWoEnableInsertFlow
        {
            get
            {
                return this.GetParaBoolen(FrmAttachmentAttr.IsWoEnableInsertFlow);
            }
            set
            {
                this.SetPara(FrmAttachmentAttr.IsWoEnableInsertFlow, value);
            }
        }

        /// <summary>
        ///  The insertion point whether the risk 
        /// </summary>
        public bool IsWoEnableInsertFengXian
        {
            get
            {
                return this.GetParaBoolen(FrmAttachmentAttr.IsWoEnableInsertFengXian);
            }
            set
            {
                this.SetPara(FrmAttachmentAttr.IsWoEnableInsertFengXian, value);
            }
        }

        /// <summary>
        ///  Whether the traces mode is enabled 
        /// </summary>
        public bool IsWoEnableMarks
        {
            get
            {
                return this.GetParaBoolen(FrmAttachmentAttr.IsWoEnableMarks);
            }
            set
            {
                this.SetPara(FrmAttachmentAttr.IsWoEnableMarks, value);
            }
        }

        /// <summary>
        ///  The insertion point whether the risk 
        /// </summary>
        public bool IsWoEnableDown
        {
            get
            {
                return this.GetParaBoolen(FrmAttachmentAttr.IsWoEnableDown);
            }
            set
            {
                this.SetPara(FrmAttachmentAttr.IsWoEnableDown, value);
            }
        }
        
        #endregion weboffice Document Properties 


        #region  Shortcuts 
        /// <summary>
        ///  Whether to enable shortcuts 
        /// </summary>
        public bool FastKeyIsEnable
        {
            get
            {
                return this.GetParaBoolen(FrmAttachmentAttr.FastKeyIsEnable);
            }
            set
            {
                this.SetPara(FrmAttachmentAttr.FastKeyIsEnable, value);
            }
        }
        /// <summary>
        ///  Enable Rule 
        /// </summary>
        public string FastKeyGenerRole
        {
            get
            {
                return this.GetParaString(FrmAttachmentAttr.FastKeyGenerRole);
            }
            set
            {
                this.SetPara(FrmAttachmentAttr.FastKeyGenerRole, value);
            }
        }
        #endregion  Shortcuts 


        #region  Constructor 
        /// <summary>
        ///  Accessory 
        /// </summary>
        public FrmAttachment()
        {
        }
        /// <summary>
        ///  Accessory 
        /// </summary>
        /// <param name="mypk"></param>
        public FrmAttachment(string mypk)
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
                Map map = new Map("Sys_FrmAttachment");

                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.EnDesc = " Accessory ";
                map.EnType = EnType.Sys;
                map.AddMyPK();

                map.AddTBString(FrmAttachmentAttr.FK_MapData, null," Form ID", true, false, 1, 30, 20);
                map.AddTBString(FrmAttachmentAttr.NoOfObj, null, " Annex No. ", true, false, 0, 200, 20);
                map.AddTBInt(FrmAttachmentAttr.FK_Node, 0, " Node Control (¶Ôsln Effective )", false, false);

                map.AddTBString(FrmAttachmentAttr.Name, null," Name ", true, false, 0, 200, 20);
                map.AddTBString(FrmAttachmentAttr.Exts, null, " The format required to upload ", true, false, 0, 200, 20);
                map.AddTBString(FrmAttachmentAttr.SaveTo, null, " Save to ", true, false, 0, 150, 20);
                map.AddTBString(FrmAttachmentAttr.Sort, null, " Category ( May be empty )", true, false, 0, 500, 20);

                map.AddTBFloat(FrmAttachmentAttr.X, 5, "X", true, false);
                map.AddTBFloat(FrmAttachmentAttr.Y, 5, "Y", false, false);
                map.AddTBFloat(FrmAttachmentAttr.W, 40, "TBWidth", false, false);
                map.AddTBFloat(FrmAttachmentAttr.H, 150, "H", false, false);

                map.AddBoolean(FrmAttachmentAttr.IsUpload, true, " Can upload ", false, false);
                map.AddBoolean(FrmAttachmentAttr.IsDelete, true, " Whether you can delete ", false, false);
                map.AddBoolean(FrmAttachmentAttr.IsDownload, true, " Can I download ", false, false);

                map.AddBoolean(FrmAttachmentAttr.IsAutoSize, true, " Automatic control the size ", false, false);
                map.AddBoolean(FrmAttachmentAttr.IsNote, true, " Whether to increase Remark ", false, false);
                map.AddBoolean(FrmAttachmentAttr.IsShowTitle, true, " Whether to display the title bar ", false, false);
                map.AddTBInt(FrmAttachmentAttr.UploadType, 0, " Upload type 0 Single 1 Multiple 2 Designation ", false, false);

                // Effective processes for father and son .
                map.AddTBInt(FrmAttachmentAttr.CtrlWay, 0, " Control presentation control 0=PK,1=FID,2=ParentID", false, false);
                map.AddTBInt(FrmAttachmentAttr.AthUploadWay, 0, " Control upload control 0= Inheritance ,1= Collaborative model .", false, false);

                // Parameter Properties .
                map.AddTBAtParas(3000);

                map.AddTBInt(FrmAttachmentAttr.RowIdx, 0, "RowIdx", false, false);
                map.AddTBInt(FrmAttachmentAttr.GroupID, 0, "GroupID", false, false);
                map.AddTBString(FrmAttachmentAttr.GUID, null, "GUID", true, false, 0, 128, 20);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        public bool IsUse = false;
        protected override bool beforeUpdateInsertAction()
        {
            if (this.FK_Node == 0)
                this.MyPK = this.FK_MapData + "_" + this.NoOfObj;
            else
                this.MyPK = this.FK_MapData + "_" + this.NoOfObj + "_" + this.FK_Node;

            return base.beforeUpdateInsertAction();
        }
        protected override bool beforeInsert()
        {
            this.IsWoEnableWF = true;
     
            this.IsWoEnableSave = false;
            this.IsWoEnableReadonly = false;
            this.IsWoEnableRevise = false;
            this.IsWoEnableViewKeepMark = false;
            this.IsWoEnablePrint = false;
            this.IsWoEnableOver = false;
            this.IsWoEnableSeal = false;
            this.IsWoEnableTemplete = false;
            return base.beforeInsert();
        }
    }
    /// <summary>
    ///  Accessory s
    /// </summary>
    public class FrmAttachments : EntitiesMyPK
    {
        #region  Structure 
        /// <summary>
        ///  Accessory s
        /// </summary>
        public FrmAttachments()
        {
        }
        /// <summary>
        ///  Accessory s
        /// </summary>
        /// <param name="fk_mapdata">s</param>
        public FrmAttachments(string fk_mapdata)
        {
            this.Retrieve(FrmAttachmentAttr.FK_MapData, fk_mapdata,FrmAttachmentAttr.FK_Node,0);
        }
        /// <summary>
        ///  Get it  Entity
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new FrmAttachment();
            }
        }
        #endregion
    }
}
