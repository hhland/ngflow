using System;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.Sys
{
    /// <summary>
    ///  Frame 
    /// </summary>
    public class MapFrameAttr : EntityMyPKAttr
    {
        /// <summary>
        ///  Main table 
        /// </summary>
        public const string FK_MapData = "FK_MapData";
        /// <summary>
        /// URL
        /// </summary>
        public const string URL = "URL";
        /// <summary>
        ///  The insertion position of the form 
        /// </summary>
        public const string RowIdx = "RowIdx";
        /// <summary>
        /// GroupID
        /// </summary>
        public const string GroupID = "GroupID";
        public const string H = "H";
        public const string W = "W";
        /// <summary>
        ///  Can adaptive size 
        /// </summary>
        public const string IsAutoSize = "IsAutoSize";
        /// <summary>
        ///  Internal Number 
        /// </summary>
        public const string NoOfObj = "NoOfObj";
        /// <summary>
        ///  Name 
        /// </summary>
        public const string Name = "Name";
        /// <summary>
        /// GUID
        /// </summary>
        public const string GUID = "GUID";
    }
    /// <summary>
    ///  Frame 
    /// </summary>
    public class MapFrame : EntityMyPK
    {
        #region  Property 
        /// <summary>
        ///  Whether adaptive size 
        /// </summary>
        public bool IsAutoSize
        {
            get
            {
                return this.GetValBooleanByKey(MapFrameAttr.IsAutoSize);
            }
            set
            {
                this.SetValByKey(MapFrameAttr.IsAutoSize, value);
            }
        }
        /// <summary>
        ///  Serial number 
        /// </summary>
        public string NoOfObj
        {
            get
            {
                return this.GetValStrByKey(MapFrameAttr.NoOfObj);
            }
            set
            {
                this.SetValByKey(MapFrameAttr.NoOfObj, value);
            }
        }
        /// <summary>
        ///  Name 
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetValStrByKey(MapFrameAttr.Name);
            }
            set
            {
                this.SetValByKey(MapFrameAttr.Name, value);
            }
        }
        /// <summary>
        ///  Connection 
        /// </summary>
        public string URL
        {
            get
            {
                string s= this.GetValStrByKey(MapFrameAttr.URL);
                if (string.IsNullOrEmpty(s))
                    return "http://ccflow.org";
                return s;
            }
            set
            {
                this.SetValByKey(MapFrameAttr.URL, value);
            }
        }
        /// <summary>
        ///  Height 
        /// </summary>
        public string H
        {
            get
            {
                return this.GetValStrByKey(MapFrameAttr.H);
            }
            set
            {
                this.SetValByKey(MapFrameAttr.H, value);
            }
        }
        /// <summary>
        ///  Width 
        /// </summary>
        public string W
        {
            get
            {
                return this.GetValStrByKey(MapFrameAttr.W);
            }
            set
            {
                this.SetValByKey(MapFrameAttr.W, value);
            }
        }
        public bool IsUse = false;
        public string FK_MapData
        {
            get
            {
                return this.GetValStrByKey(MapFrameAttr.FK_MapData);
            }
            set
            {
                this.SetValByKey(MapFrameAttr.FK_MapData, value);
            }
        }
        public int RowIdx
        {
            get
            {
                return this.GetValIntByKey(MapFrameAttr.RowIdx);
            }
            set
            {
                this.SetValByKey(MapFrameAttr.RowIdx, value);
            }
        }
        
        public int GroupID
        {
            get
            {
                return this.GetValIntByKey(MapFrameAttr.GroupID);
            }
            set
            {
                this.SetValByKey(MapFrameAttr.GroupID, value);
            }
        }
       
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Frame 
        /// </summary>
        public MapFrame()
        {
        }
        /// <summary>
        ///  Frame 
        /// </summary>
        /// <param name="no"></param>
        public MapFrame(string mypk)
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
                Map map = new Map("Sys_MapFrame");
                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.EnDesc = " Frame ";
                map.EnType = EnType.Sys;

                map.AddMyPK();
                map.AddTBString(MapFrameAttr.NoOfObj, null, " Serial number ", true, false, 1, 20, 20);
                map.AddTBString(MapFrameAttr.Name, null, " Name ", true, false, 1, 200, 20);

                map.AddTBString(MapFrameAttr.FK_MapData, null, " Main table ", true, false, 0, 30, 20);
                map.AddTBString(MapFrameAttr.URL, null, "URL", true, false, 0, 3000, 20);
                map.AddTBString(MapFrameAttr.W, null, "W", true, false, 0, 20, 20);
                map.AddTBString(MapFrameAttr.H, null, "H", true, false, 0, 20, 20);

                //map.AddTBInt(MapFrameAttr.H, 500, " Height ", false, false);
                //map.AddTBInt(MapFrameAttr.W, 400, " Width ", false, false);

                map.AddBoolean(MapFrameAttr.IsAutoSize, true, " Whether to automatically set the size ", false, false);
                map.AddTBInt(MapFrameAttr.RowIdx, 99, " Location ", false, false);
                map.AddTBInt(MapFrameAttr.GroupID, 0, "GroupID", false, false);

                map.AddTBString(FrmBtnAttr.GUID, null, "GUID", true, false, 0, 128, 20);

                
                this._enMap = map;
                return this._enMap;
            }
        }
      
        protected override bool beforeUpdateInsertAction()
        {
            this.MyPK = this.FK_MapData + "_" + this.NoOfObj;
            return base.beforeUpdateInsertAction();
        }
        #endregion
    }
    /// <summary>
    ///  Frame s
    /// </summary>
    public class MapFrames : EntitiesMyPK
    {
        #region  Structure 
        /// <summary>
        ///  Frame s
        /// </summary>
        public MapFrames()
        {
        }
        /// <summary>
        ///  Frame s
        /// </summary>
        /// <param name="fk_mapdata">s</param>
        public MapFrames(string fk_mapdata)
        {
            this.Retrieve(MapFrameAttr.FK_MapData, fk_mapdata, MapFrameAttr.GroupID);
        }
        /// <summary>
        ///  Get it  Entity
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new MapFrame();
            }
        }
        #endregion
    }
}
