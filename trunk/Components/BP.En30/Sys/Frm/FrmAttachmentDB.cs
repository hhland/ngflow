using System;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.Sys
{
    /// <summary>
    ///  Accessories Data Storage  -  Property 
    /// </summary>
    public class FrmAttachmentDBAttr : EntityMyPKAttr
    {
        /// <summary>
        ///  Accessory 
        /// </summary>
        public const string FK_FrmAttachment = "FK_FrmAttachment";
        /// <summary>
        ///  Main table 
        /// </summary>
        public const string FK_MapData = "FK_MapData";
        /// <summary>
        /// RefPKVal
        /// </summary>
        public const string RefPKVal = "RefPKVal";
        /// <summary>
        ///  File name 
        /// </summary>
        public const string FileName = "FileName";
        /// <summary>
        ///  File Extension 
        /// </summary>
        public const string FileExts = "FileExts";
        /// <summary>
        ///  File size 
        /// </summary>
        public const string FileSize = "FileSize";
        /// <summary>
        ///  Save to 
        /// </summary>
        public const string FileFullName = "FileFullName";
        /// <summary>
        ///  Record Date 
        /// </summary>
        public const string RDT = "RDT";
        /// <summary>
        ///  Record people 
        /// </summary>
        public const string Rec = "Rec";
        /// <summary>
        ///  Record the names of people 
        /// </summary>
        public const string RecName = "RecName";
        /// <summary>
        ///  Category 
        /// </summary>
        public const string Sort = "Sort";
        /// <summary>
        ///  Remark 
        /// </summary>
        public const string MyNote = "MyNote";
        /// <summary>
        ///  Node ID
        /// </summary>
        public const string NodeID = "NodeID";
        /// <summary>
        ///  Whether locked row 
        /// </summary>
        public const string IsRowLock = "IsRowLock";
        /// <summary>
        ///  Upload GUID
        /// </summary>
        public const string UploadGUID = "UploadGUID";

    }
    /// <summary>
    ///  Accessories Data Storage 
    /// </summary>
    public class FrmAttachmentDB : EntityMyPK
    {
        #region  Property 
        /// <summary>
        ///  Category 
        /// </summary>
        public string Sort
        {
            get
            {
                return this.GetValStringByKey(FrmAttachmentDBAttr.Sort);
            }
            set
            {
                this.SetValByKey(FrmAttachmentDBAttr.Sort, value);
            }
        }
        /// <summary>
        ///  Record Date 
        /// </summary>
        public string RDT
        {
            get
            {
                return this.GetValStringByKey(FrmAttachmentDBAttr.RDT);
            }
            set
            {
                this.SetValByKey(FrmAttachmentDBAttr.RDT, value);
            }
        }
        /// <summary>
        ///  File 
        /// </summary>
        public string FileFullName
        {
            get
            {
                return this.GetValStringByKey(FrmAttachmentDBAttr.FileFullName);
            }
            set
            {
                this.SetValByKey(FrmAttachmentDBAttr.FileFullName, value);
            }
        }
        /// <summary>
        ///  Upload GUID
        /// </summary>
        public string UploadGUID
        {
            get
            {
                return this.GetValStringByKey(FrmAttachmentDBAttr.UploadGUID);
            }
            set
            {
                this.SetValByKey(FrmAttachmentDBAttr.UploadGUID, value);
            }
        }
        /// <summary>
        ///  Accessories path 
        /// </summary>
        public string FilePathName
        {
            get
            {
                return this.FileFullName.Substring(this.FileFullName.LastIndexOf('\\') + 1);
            }
        }
        /// <summary>
        ///  Attachment Name 
        /// </summary>
        public string FileName
        {
            get
            {
                return this.GetValStringByKey(FrmAttachmentDBAttr.FileName);
            }
            set
            {
                this.SetValByKey(FrmAttachmentDBAttr.FileName, value);
            }
        }
        /// <summary>
        ///  Attachment Extension 
        /// </summary>
        public string FileExts
        {
            get
            {
                return this.GetValStringByKey(FrmAttachmentDBAttr.FileExts);
            }
            set
            {
                this.SetValByKey(FrmAttachmentDBAttr.FileExts, value.Replace(".",""));
            }
        }
        /// <summary>
        ///  Related Accessories 
        /// </summary>
        public string FK_FrmAttachment
        {
            get
            {
                return this.GetValStringByKey(FrmAttachmentDBAttr.FK_FrmAttachment);
            }
            set
            {
                this.SetValByKey(FrmAttachmentDBAttr.FK_FrmAttachment, value);
            }
        }
        /// <summary>
        ///  Primary key value 
        /// </summary>
        public string RefPKVal
        {
            get
            {
                return this.GetValStringByKey(FrmAttachmentDBAttr.RefPKVal);
            }
            set
            {
                this.SetValByKey(FrmAttachmentDBAttr.RefPKVal, value);
            }
        }
        /// <summary>
        /// MyNote
        /// </summary>
        public string MyNote
        {
            get
            {
                return this.GetValStringByKey(FrmAttachmentDBAttr.MyNote);
            }
            set
            {
                this.SetValByKey(FrmAttachmentDBAttr.MyNote, value);
            }
        }
        /// <summary>
        ///  Record people 
        /// </summary>
        public string Rec
        {
            get
            {
                return this.GetValStringByKey(FrmAttachmentDBAttr.Rec);
            }
            set
            {
                this.SetValByKey(FrmAttachmentDBAttr.Rec, value);
            }
        }
        /// <summary>
        ///  Name of recording 
        /// </summary>
        public string RecName
        {
            get
            {
                return this.GetValStringByKey(FrmAttachmentDBAttr.RecName);
            }
            set
            {
                this.SetValByKey(FrmAttachmentDBAttr.RecName, value);
            }
        }
        /// <summary>
        ///  Annex No. 
        /// </summary>
        public string FK_MapData
        {
            get
            {
                return this.GetValStringByKey(FrmAttachmentDBAttr.FK_MapData);
            }
            set
            {
                this.SetValByKey(FrmAttachmentDBAttr.FK_MapData, value);
            }
        }
        /// <summary>
        ///  File size 
        /// </summary>
        public float FileSize
        {
            get
            {
                return this.GetValFloatByKey(FrmAttachmentDBAttr.FileSize);
            }
            set
            {
                this.SetValByKey(FrmAttachmentDBAttr.FileSize, value/1024);
            }
        }
        /// <summary>
        ///  Whether locked row ?
        /// </summary>
        public bool IsRowLock
        {
            get
            {
                return this.GetValBooleanByKey(FrmAttachmentDBAttr.IsRowLock);
            }
            set
            {
                this.SetValByKey(FrmAttachmentDBAttr.IsRowLock, value);
            }
        }
        /// <summary>
        ///  Attachment Extension 
        /// </summary>
        public string NodeID
        {
            get
            {
                return this.GetValStringByKey(FrmAttachmentDBAttr.NodeID);
            }
            set
            {
                this.SetValByKey(FrmAttachmentDBAttr.NodeID, value);
            }
        }
        /// <summary>
        ///  Accessory Type 
        /// </summary>
        public AttachmentUploadType HisAttachmentUploadType
        {
            get
            {

                if (this.MyPK.Contains("_") && this.MyPK.Length < 32)
                    return AttachmentUploadType.Single;
                else
                    return AttachmentUploadType.Multi;
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Accessories Data Storage 
        /// </summary>
        public FrmAttachmentDB()
        {
        }
        /// <summary>
        ///  Accessories Data Storage 
        /// </summary>
        /// <param name="mypk"></param>
        public FrmAttachmentDB(string mypk)
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
                Map map = new Map("Sys_FrmAttachmentDB");

                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.EnDesc = " Accessories Data Storage ";
                map.EnType = EnType.Sys;
                map.AddMyPK();
                map.AddTBString(FrmAttachmentDBAttr.FK_MapData, null,"FK_MapData", true, false, 1, 30, 20);
                map.AddTBString(FrmAttachmentDBAttr.FK_FrmAttachment, null, " Annex No. ", true, false, 1, 500, 20);
                map.AddTBString(FrmAttachmentDBAttr.RefPKVal, null, " Entity primary key ", true, false, 0, 200, 20);

                map.AddTBString(FrmAttachmentDBAttr.Sort, null, " Category ", true, false, 0, 200, 20);
                map.AddTBString(FrmAttachmentDBAttr.FileFullName, null, "FileFullName", true, false, 0, 700, 20);
                map.AddTBString(FrmAttachmentDBAttr.FileName, null," Name ", true, false, 0, 500, 20);
                map.AddTBString(FrmAttachmentDBAttr.FileExts, null, " Expand ", true, false, 0, 200, 20);
                map.AddTBFloat(FrmAttachmentDBAttr.FileSize, 0, " File size ", true, false);

                map.AddTBDateTime(FrmAttachmentDBAttr.RDT, null, " Record Date ", true, false);
                map.AddTBString(FrmAttachmentDBAttr.Rec, null, " Record people ", true, false, 0, 200, 20);
                map.AddTBString(FrmAttachmentDBAttr.RecName, null, " Record the names of people ", true, false, 0, 200, 20);
                map.AddTBStringDoc(FrmAttachmentDBAttr.MyNote, null, " Remark ", true, false);
                map.AddTBString(FrmAttachmentDBAttr.NodeID, null, " Node ID", true, false, 0, 200, 20);

                map.AddTBInt(FrmAttachmentDBAttr.IsRowLock, 0, " Whether locked row ", true, false);

                // This value is generated when uploading .
                map.AddTBString(FrmAttachmentDBAttr.UploadGUID, null, " Upload GUID", true, false, 0, 200, 20);

                this._enMap = map;
                return this._enMap;
            }
        }
        /// <summary>
        ///  Rewrite 
        /// </summary>
        /// <returns></returns>
        protected override bool beforeInsert()
        {
            return base.beforeInsert();
        }
        #endregion
    }
    /// <summary>
    ///  Accessories Data Storage s
    /// </summary>
    public class FrmAttachmentDBs : EntitiesMyPK
    {
        #region  Structure 
        /// <summary>
        ///  Accessories Data Storage s
        /// </summary>
        public FrmAttachmentDBs()
        {
        }
        /// <summary>
        ///  Accessories Data Storage s
        /// </summary>
        /// <param name="fk_mapdata">s</param>
        public FrmAttachmentDBs(string fk_mapdata,string pkval)
        {
            this.Retrieve(FrmAttachmentDBAttr.FK_MapData, fk_mapdata, 
                FrmAttachmentDBAttr.RefPKVal, pkval);
        }
        public FrmAttachmentDBs(string fk_mapdata, Int64 pkval)
        {
            this.Retrieve(FrmAttachmentDBAttr.FK_MapData, fk_mapdata,
                FrmAttachmentDBAttr.RefPKVal, pkval);
        }
        /// <summary>
        ///  Get it  Entity
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new FrmAttachmentDB();
            }
        }
        #endregion
    }
}
