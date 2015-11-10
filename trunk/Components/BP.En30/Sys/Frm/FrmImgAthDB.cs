using System;
using System.Collections;
using BP.DA;
using BP.En;
namespace BP.Sys
{
    /// <summary>
    ///  Cut the picture attachment data storage  -  Property 
    /// </summary>
    public class FrmImgAthDBAttr : EntityMyPKAttr
    {
        /// <summary>
        ///  Accessory 
        /// </summary>
        public const string FK_FrmImgAth = "FK_FrmImgAth";
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
    }
    /// <summary>
    ///  Cut the picture attachment data storage 
    /// </summary>
    public class FrmImgAthDB : EntityMyPK
    {
        #region  Property 
        /// <summary>
        ///  Category 
        /// </summary>
        public string Sort
        {
            get
            {
                return this.GetValStringByKey(FrmImgAthDBAttr.Sort);
            }
            set
            {
                this.SetValByKey(FrmImgAthDBAttr.Sort, value);
            }
        }
        /// <summary>
        ///  Record Date 
        /// </summary>
        public string RDT
        {
            get
            {
                return this.GetValStringByKey(FrmImgAthDBAttr.RDT);
            }
            set
            {
                this.SetValByKey(FrmImgAthDBAttr.RDT, value);
            }
        }
        /// <summary>
        ///  File 
        /// </summary>
        public string FileFullName
        {
            get
            {
                return this.GetValStringByKey(FrmImgAthDBAttr.FileFullName);
            }
            set
            {
                this.SetValByKey(FrmImgAthDBAttr.FileFullName, value);
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
                return this.GetValStringByKey(FrmImgAthDBAttr.FileName);
            }
            set
            {
                this.SetValByKey(FrmImgAthDBAttr.FileName, value);
            }
        }
        /// <summary>
        ///  Attachment Extension 
        /// </summary>
        public string FileExts
        {
            get
            {
                return this.GetValStringByKey(FrmImgAthDBAttr.FileExts);
            }
            set
            {
                this.SetValByKey(FrmImgAthDBAttr.FileExts, value.Replace(".",""));
            }
        }
        /// <summary>
        ///  Related Accessories 
        /// </summary>
        public string FK_FrmImgAth
        {
            get
            {
                return this.GetValStringByKey(FrmImgAthDBAttr.FK_FrmImgAth);
            }
            set
            {
                this.SetValByKey(FrmImgAthDBAttr.FK_FrmImgAth, value);
            }
        }
        /// <summary>
        ///  Primary key value 
        /// </summary>
        public string RefPKVal
        {
            get
            {
                return this.GetValStringByKey(FrmImgAthDBAttr.RefPKVal);
            }
            set
            {
                this.SetValByKey(FrmImgAthDBAttr.RefPKVal, value);
            }
        }
        /// <summary>
        /// MyNote
        /// </summary>
        public string MyNote
        {
            get
            {
                return this.GetValStringByKey(FrmImgAthDBAttr.MyNote);
            }
            set
            {
                this.SetValByKey(FrmImgAthDBAttr.MyNote, value);
            }
        }
        /// <summary>
        ///  Record people 
        /// </summary>
        public string Rec
        {
            get
            {
                return this.GetValStringByKey(FrmImgAthDBAttr.Rec);
            }
            set
            {
                this.SetValByKey(FrmImgAthDBAttr.Rec, value);
            }
        }
        /// <summary>
        ///  Name of recording 
        /// </summary>
        public string RecName
        {
            get
            {
                return this.GetValStringByKey(FrmImgAthDBAttr.RecName);
            }
            set
            {
                this.SetValByKey(FrmImgAthDBAttr.RecName, value);
            }
        }
        /// <summary>
        ///  Annex No. 
        /// </summary>
        public string FK_MapData
        {
            get
            {
                return this.GetValStringByKey(FrmImgAthDBAttr.FK_MapData);
            }
            set
            {
                this.SetValByKey(FrmImgAthDBAttr.FK_MapData, value);
            }
        }
        /// <summary>
        ///  File size 
        /// </summary>
        public float FileSize
        {
            get
            {
                return this.GetValFloatByKey(FrmImgAthDBAttr.FileSize);
            }
            set
            {
                this.SetValByKey(FrmImgAthDBAttr.FileSize, value/1024);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Cut the picture attachment data storage 
        /// </summary>
        public FrmImgAthDB()
        {
        }
        /// <summary>
        ///  Cut the picture attachment data storage 
        /// </summary>
        /// <param name="mypk"></param>
        public FrmImgAthDB(string mypk)
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

                Map map = new Map("Sys_FrmImgAthDB");

                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.EnDesc = " Cut the picture attachment data storage ";
                map.EnType = EnType.Sys;
                map.AddMyPK();

                //  The following three fields a primary key . FK_FrmImgAth+"_"+RefPKVal
                map.AddTBString(FrmImgAthDBAttr.FK_MapData, null, " Accessory ID", true, false, 1, 30, 20);
                map.AddTBString(FrmImgAthDBAttr.FK_FrmImgAth, null, " Image Attachment No. ", true, false, 1, 50, 20);
                map.AddTBString(FrmImgAthDBAttr.RefPKVal, null, " Entity primary key ", true, false, 1, 50, 20);

                map.AddTBString(FrmImgAthDBAttr.FileFullName, null, " Full file path ", true, false, 0, 700, 20);
                map.AddTBString(FrmImgAthDBAttr.FileName, null, " Name ", true, false, 0, 500, 20);
                map.AddTBString(FrmImgAthDBAttr.FileExts, null, " Extensions ", true, false, 0, 200, 20);
                map.AddTBFloat(FrmImgAthDBAttr.FileSize, 0, " File size ", true, false);

                map.AddTBDateTime(FrmImgAthDBAttr.RDT, null, " Record Date ", true, false);
                map.AddTBString(FrmImgAthDBAttr.Rec, null, " Record people ", true, false, 0, 200, 20);
                map.AddTBString(FrmImgAthDBAttr.RecName, null, " Record the names of people ", true, false, 0, 200, 20);
                map.AddTBStringDoc(FrmImgAthDBAttr.MyNote, null, " Remark ", true, false);

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
            this.MyPK = this.FK_FrmImgAth + "_" + this.RefPKVal;
            return base.beforeInsert();
        }
        /// <summary>
        ///  Rewrite 
        /// </summary>
        /// <returns></returns>
        protected override bool beforeUpdate()
        {
            this.MyPK = this.FK_FrmImgAth + "_" + this.RefPKVal;
            return base.beforeUpdate();
        }
        #endregion
    }
    /// <summary>
    ///  Cut the picture attachment data storage s
    /// </summary>
    public class FrmImgAthDBs : EntitiesMyPK
    {
        #region  Structure 
        /// <summary>
        ///  Cut the picture attachment data storage s
        /// </summary>
        public FrmImgAthDBs()
        {
        }
        /// <summary>
        ///  Cut the picture attachment data storage s
        /// </summary>
        /// <param name="fk_mapdata">s</param>
        public FrmImgAthDBs(string fk_mapdata,string pkval)
        {
            this.Retrieve(FrmImgAthDBAttr.FK_MapData, fk_mapdata, 
                FrmImgAthDBAttr.RefPKVal, pkval);
        }
        /// <summary>
        ///  Get it  Entity
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new FrmImgAthDB();
            }
        }
        #endregion
    }
}
