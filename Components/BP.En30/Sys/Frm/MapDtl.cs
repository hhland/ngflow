using System;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.Sys
{
    /// <summary>
    ///  From the table below shows the way 
    /// </summary>
    public enum DtlShowModel
    {
        /// <summary>
        ///  Tabular form 
        /// </summary>
        Table,
        /// <summary>
        ///  Card mode 
        /// </summary>
        Card
    }
    /// <summary>
    ///  Oak-line processing 
    /// </summary>
    public enum WhenOverSize
    {
        /// <summary>
        ///  Does not deal with 
        /// </summary>
        None,
        /// <summary>
        ///  Add a line 
        /// </summary>
        AddRow,
        /// <summary>
        ///  Page 
        /// </summary>
        TurnPage
    }
    public enum DtlOpenType
    {
        /// <summary>
        ///  Open to persons 
        /// </summary>
        ForEmp,
        /// <summary>
        ///  Open for work 
        /// </summary>
        ForWorkID,
        /// <summary>
        ///  Opening for Process 
        /// </summary>
        ForFID
    }
    /// <summary>
    ///  Work schedule 
    /// </summary>
    public enum DtlModel
    {
        /// <summary>
        ///  Ordinary 
        /// </summary>
        Ordinary,
        /// <summary>
        ///  Fixed column 
        /// </summary>
        FixRow
    }
    /// <summary>
    ///  Details 
    /// </summary>
    public class MapDtlAttr : EntityNoNameAttr
    {
        /// <summary>
        ///  Filling Data
        /// </summary>
        public const string ImpFixDataSql = "ImpFixDataSql";

        /// <summary>
        ///  Filling the tree Sql
        /// </summary>
        public const string ImpFixTreeSql = "ImpFixTreeSql";
        /// <summary>
        ///  Mode 
        /// </summary>
        public const string Model = "Model";
        /// <summary>
        ///  Main table 
        /// </summary>
        public const string FK_MapData = "FK_MapData";
        /// <summary>
        /// PTable
        /// </summary>
        public const string PTable = "PTable";
        /// <summary>
        /// DtlOpenType
        /// </summary>
        public const string DtlOpenType = "DtlOpenType";
        /// <summary>
        ///  The insertion position of the form 
        /// </summary>
        public const string RowIdx = "RowIdx";
        /// <summary>
        ///  The number of rows 
        /// </summary>
        public const string RowsOfList = "RowsOfList";
        /// <summary>
        ///  Whether to display the total 
        /// </summary>
        public const string IsShowSum = "IsShowSum";
        /// <summary>
        ///  Whether to display idx
        /// </summary>
        public const string IsShowIdx = "IsShowIdx";
        /// <summary>
        ///  Whether to allow copy Data 
        /// </summary>
        public const string IsCopyNDData = "IsCopyNDData";
        /// <summary>
        ///  Is read-only 
        /// </summary>
        public const string IsReadonly = "IsReadonly";
        /// <summary>
        /// WhenOverSize
        /// </summary>
        public const string WhenOverSize = "WhenOverSize";
        /// <summary>
        /// GroupID
        /// </summary>
        public const string GroupID = "GroupID";
        /// <summary>
        ///  Whether you can delete 
        /// </summary>
        public const string IsDelete = "IsDelete";
        /// <summary>
        ///  Whether it can be inserted 
        /// </summary>
        public const string IsInsert = "IsInsert";
        /// <summary>
        ///  Whether it can be updated 
        /// </summary>
        public const string IsUpdate = "IsUpdate";
        /// <summary>
        ///  Is enabled by 
        /// </summary>
        public const string IsEnablePass = "IsEnablePass";
      
        /// <summary>
        ///  Whether it is the confluence of aggregated data 
        /// </summary>
        public const string IsHLDtl = "IsHLDtl";
        /// <summary>
        ///  Whether to display the title 
        /// </summary>
        public const string IsShowTitle = "IsShowTitle";
        /// <summary>
        ///  Display Format 
        /// </summary>
        public const string DtlShowModel = "DtlShowModel";
        /// <summary>
        ///  Is visible 
        /// </summary>
        public const string IsView = "IsView";
        /// <summary>
        /// x
        /// </summary>
        public const string X = "X";
        /// <summary>
        /// Y
        /// </summary>
        public const string Y = "Y";
        /// <summary>
        /// H Height 
        /// </summary>
        public const string H = "H";
        /// <summary>
        /// w Width 
        /// </summary>
        public const string W = "W";
        /// <summary>
        /// 
        /// </summary>
        public const string FrmW = "FrmW";
        /// <summary>
        /// 
        /// </summary>
        public const string FrmH = "FrmH";
       
        /// <summary>
        ///  Whether to enable multiple attachments 
        /// </summary>
        public const string IsEnableAthM = "IsEnableAthM";
        /// <summary>
        ///  Whether enabled many 
        /// </summary>
        public const string IsEnableM2M = "IsEnableM2M";
        /// <summary>
        ///  Whether to enable many more 
        /// </summary>
        public const string IsEnableM2MM = "IsEnableM2MM";
        /// <summary>
        ///  Multi-header columns 
        /// </summary>
        public const string MTR = "MTR";
        /// <summary>
        /// GUID
        /// </summary>
        public const string GUID = "GUID";
        /// <summary>
        ///  Packet 
        /// </summary>
        public const string GroupField = "GroupField";
        /// <summary>
        ///  Whether to enable the grouping field 
        /// </summary>
        public const string IsEnableGroupField = "IsEnableGroupField";
        /// <summary>
        ///  Node ( Permissions for multiple forms of control )
        /// </summary>
        public const string FK_Node = "FK_Node";

        #region  Parameter Properties .
        public const string IsEnableLink = "IsEnableLink";
        public const string LinkLabel = "LinkLabel";
        public const string LinkUrl = "LinkUrl";
        public const string LinkTarget = "LinkTarget";
        #endregion  Parameter Properties .

        #region  Parameter Properties .
        /// <summary>
        ///  Lock is enabled 
        /// </summary>
        public const string IsRowLock = "IsRowLock";
        #endregion  Parameter Properties .

        #region  Import Export Properties .
        /// <summary>
        ///  Can I export 
        /// </summary>
        public const string IsExp = "IsExp";
        /// <summary>
        ///  Can I import Excel?
        /// </summary>
        public const string IsImp = "IsImp";
        /// <summary>
        ///  Whether to enable select Import ?
        /// </summary>
        public const string IsEnableSelectImp = "IsEnableSelectImp";
        /// <summary>
        ///  Inquiry sql
        /// </summary>
        public const string ImpSQLSearch = "ImpSQLSearch";
        /// <summary>
        ///  Choose sql
        /// </summary>
        public const string ImpSQLInit = "ImpSQLInit";
        /// <summary>
        ///  Fill data 
        /// </summary>
        public const string ImpSQLFull = "ImpSQLFull";
        #endregion
    }

    /// <summary>
    ///  Details 
    /// </summary>
    public class MapDtl : EntityNoName
    {
        #region  Import Export Properties .
        /// <summary>
        ///  Can I export 
        /// </summary>
        public bool IsExp
        {
            get
            {
                return this.GetValBooleanByKey(MapDtlAttr.IsExp);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.IsExp, value);
            }
        }
        /// <summary>
        ///  Can I import ?
        /// </summary>
        public bool IsImp
        {
            get
            {
                return this.GetValBooleanByKey(MapDtlAttr.IsImp);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.IsImp, value);
            }
        }
        /// <summary>
        ///  Select whether to enable the project to import data ?
        /// </summary>
        public bool IsEnableSelectImp
        {
            get
            {
                return this.GetValBooleanByKey(MapDtlAttr.IsEnableSelectImp);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.IsEnableSelectImp, value);
            }
        }
        /// <summary>
        ///  Inquiry sql
        /// </summary>
        public string ImpSQLInit
        {
            get
            {
                return this.GetValStringByKey(MapDtlAttr.ImpSQLInit).Replace("~", "'");
            }
            set
            {
                this.SetValByKey(MapDtlAttr.ImpSQLInit, value);
            }
        }
        /// <summary>
        ///  Search for sql
        /// </summary>
        public string ImpSQLSearch
        {
            get
            {
                return this.GetValStringByKey(MapDtlAttr.ImpSQLSearch).Replace("~", "'");
            }
            set
            {
                this.SetValByKey(MapDtlAttr.ImpSQLSearch, value);
            }
        }
        /// <summary>
        ///  Fill data 
        /// </summary>
        public string ImpSQLFull
        {
            get
            {
                return this.GetValStringByKey(MapDtlAttr.ImpSQLFull).Replace("~","'");
            }
            set
            {
                this.SetValByKey(MapDtlAttr.ImpSQLFull, value);
            }
        }
        #endregion

        #region  Basic Settings 

        public string ImpFixDataSql
        {
            get { return this.GetValStringByKey(MapDtlAttr.ImpFixDataSql); }
            set { this.SetValByKey(MapDtlAttr.ImpFixDataSql, value); }

        }

        /// <summary>
        ///  Fill properties sql
        /// </summary>
        public string ImpFixTreeSql
        {
            
            get { return this.GetValStringByKey(MapDtlAttr.ImpFixTreeSql); }
            set { this.SetValByKey(MapDtlAttr.ImpFixTreeSql, value); }
        }

        /// <summary>
        ///  Mode 
        /// </summary>
        public DtlModel DtlModel
        {
            get
            {
                return (DtlModel)this.GetValIntByKey(MapDtlAttr.Model);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.Model, (int)value);
            }
        }
        /// <summary>
        ///  Whether to enable the line lock .
        /// </summary>
        public bool IsRowLock
        {
            get
            {
                return this.GetParaBoolen(MapDtlAttr.IsRowLock, false);
            }
            set
            {
                this.SetPara(MapDtlAttr.IsRowLock, value);
            }
        }
        #endregion  Basic Settings  

        #region  Parameter Properties 
        public bool IsEnableLink
        {
            get
            {
                return this.GetParaBoolen(MapDtlAttr.IsEnableLink,false);
            }
            set
            {
                this.SetPara(MapDtlAttr.IsEnableLink, value);
            }
        }
        public string LinkLabel
        {
            get
            {
                string s= this.GetParaString(MapDtlAttr.LinkLabel);
                if (string.IsNullOrEmpty(s))
                    return " Detailed ";
                return s;
            }
            set
            {
                this.SetPara(MapDtlAttr.LinkLabel, value);
            }
        }
        public string LinkUrl
        {
            get
            {
                string s = this.GetParaString(MapDtlAttr.LinkUrl);
                if (string.IsNullOrEmpty(s))
                    return "http://ccport.org";

                s = s.Replace("*", "@");
                return s;
            }
            set
            {
                string val = value;
                val = val.Replace("@", "*");
                this.SetPara(MapDtlAttr.LinkUrl, val);
            }
        }
        public string LinkTarget
        {
            get
            {
                string s = this.GetParaString(MapDtlAttr.LinkTarget);
                if (string.IsNullOrEmpty(s))
                    return "_blank";
                return s;
            }
            set
            {
                this.SetPara(MapDtlAttr.LinkTarget, value);
            }
        }
        /// <summary>
        ///  Node ID
        /// </summary>
        public int FK_Node
        {
            get
            {
                return this.GetValIntByKey(MapDtlAttr.FK_Node);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.FK_Node, value);
            }
        }
        #endregion  Parameter Properties 

        #region  Foreign key attribute 
        /// <summary>
        ///  Frame 
        /// </summary>
        public MapFrames MapFrames
        {
            get
            {
                MapFrames obj = this.GetRefObject("MapFrames") as MapFrames;
                if (obj == null)
                {
                    obj = new MapFrames(this.No);
                    this.SetRefObject("MapFrames", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  Group field 
        /// </summary>
        public GroupFields GroupFields_del
        {
            get
            {
                GroupFields obj = this.GetRefObject("GroupFields") as GroupFields;
                if (obj == null)
                {
                    obj = new GroupFields(this.No);
                    this.SetRefObject("GroupFields", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  Logical extension 
        /// </summary>
        public MapExts MapExts
        {
            get
            {
                MapExts obj = this.GetRefObject("MapExts") as MapExts;
                if (obj == null)
                {
                    obj = new MapExts(this.No);
                    this.SetRefObject("MapExts", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  Event 
        /// </summary>
        public FrmEvents FrmEvents
        {
            get
            {
                FrmEvents obj = this.GetRefObject("FrmEvents") as FrmEvents;
                if (obj == null)
                {
                    obj = new FrmEvents(this.No);
                    this.SetRefObject("FrmEvents", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  Many 
        /// </summary>
        public MapM2Ms MapM2Ms
        {
            get
            {
                MapM2Ms obj = this.GetRefObject("MapM2Ms") as MapM2Ms;
                if (obj == null)
                {
                    obj = new MapM2Ms(this.No);
                    this.SetRefObject("MapM2Ms", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  From Table 
        /// </summary>
        public MapDtls MapDtls
        {
            get
            {
                MapDtls obj = this.GetRefObject("MapDtls") as MapDtls;
                if (obj == null)
                {
                    obj = new MapDtls(this.No);
                    this.SetRefObject("MapDtls", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  Hyperlinks 
        /// </summary>
        public FrmLinks FrmLinks
        {
            get
            {
                FrmLinks obj = this.GetRefObject("FrmLinks") as FrmLinks;
                if (obj == null)
                {
                    obj = new FrmLinks(this.No);
                    this.SetRefObject("FrmLinks", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  Push button 
        /// </summary>
        public FrmBtns FrmBtns
        {
            get
            {
                FrmBtns obj = this.GetRefObject("FrmLinks") as FrmBtns;
                if (obj == null)
                {
                    obj = new FrmBtns(this.No);
                    this.SetRefObject("FrmBtns", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  Element 
        /// </summary>
        public FrmEles FrmEles
        {
            get
            {
                FrmEles obj = this.GetRefObject("FrmEles") as FrmEles;
                if (obj == null)
                {
                    obj = new FrmEles(this.No);
                    this.SetRefObject("FrmEles", obj);
                }
                return obj;
            }
        }
        /// <summary>
        /// Ïß
        /// </summary>
        public FrmLines FrmLines
        {
            get
            {
                FrmLines obj = this.GetRefObject("FrmLines") as FrmLines;
                if (obj == null)
                {
                    obj = new FrmLines(this.No);
                    this.SetRefObject("FrmLines", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  Label 
        /// </summary>
        public FrmLabs FrmLabs
        {
            get
            {
                FrmLabs obj = this.GetRefObject("FrmLabs") as FrmLabs;
                if (obj == null)
                {
                    obj = new FrmLabs(this.No);
                    this.SetRefObject("FrmLabs", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  Picture 
        /// </summary>
        public FrmImgs FrmImgs
        {
            get
            {
                FrmImgs obj = this.GetRefObject("FrmLabs") as FrmImgs;
                if (obj == null)
                {
                    obj = new FrmImgs(this.No);
                    this.SetRefObject("FrmLabs", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  Accessory 
        /// </summary>
        public FrmAttachments FrmAttachments
        {
            get
            {
                FrmAttachments obj = this.GetRefObject("FrmAttachments") as FrmAttachments;
                if (obj == null)
                {
                    obj = new FrmAttachments(this.No);
                    this.SetRefObject("FrmAttachments", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  Image Attachment 
        /// </summary>
        public FrmImgAths FrmImgAths
        {
            get
            {
                FrmImgAths obj = this.GetRefObject("FrmImgAths") as FrmImgAths;
                if (obj == null)
                {
                    obj = new FrmImgAths(this.No);
                    this.SetRefObject("FrmImgAths", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  Radio buttons 
        /// </summary>
        public FrmRBs FrmRBs
        {
            get
            {
                FrmRBs obj = this.GetRefObject("FrmRBs") as FrmRBs;
                if (obj == null)
                {
                    obj = new FrmRBs(this.No);
                    this.SetRefObject("FrmRBs", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  Property 
        /// </summary>
        public MapAttrs MapAttrs
        {
            get
            {
                MapAttrs obj = this.GetRefObject("MapAttrs") as MapAttrs;
                if (obj == null)
                {
                    obj = new MapAttrs(this.No);
                    this.SetRefObject("MapAttrs", obj);
                }
                return obj;
            }
        }
        #endregion

        #region  Property 
        public GEDtls HisGEDtls_temp = null;
        public DtlShowModel HisDtlShowModel
        {
            get
            {
                return (DtlShowModel)this.GetValIntByKey(MapDtlAttr.DtlShowModel);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.DtlShowModel, (int)value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public WhenOverSize HisWhenOverSize
        {
            get
            {
                return (WhenOverSize)this.GetValIntByKey(MapDtlAttr.WhenOverSize);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.WhenOverSize, (int)value);
            }
        }
      
        public bool IsShowSum
        {
            get
            {
                return this.GetValBooleanByKey(MapDtlAttr.IsShowSum);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.IsShowSum, value);
            }
        }
        public bool IsShowIdx
        {
            get
            {
                return this.GetValBooleanByKey(MapDtlAttr.IsShowIdx);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.IsShowIdx, value);
            }
        }
        public bool IsReadonly_del
        {
            get
            {
                return this.GetValBooleanByKey(MapDtlAttr.IsReadonly);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.IsReadonly, value);
            }
        }
        public bool IsShowTitle
        {
            get
            {
                return this.GetValBooleanByKey(MapDtlAttr.IsShowTitle);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.IsShowTitle, value);
            }
        }
        /// <summary>
        ///  Whether it is the confluence of aggregated data 
        /// </summary>
        public bool IsHLDtl
        {
            get
            {
                return this.GetValBooleanByKey(MapDtlAttr.IsHLDtl);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.IsHLDtl, value);
            }
        }
        public int _IsReadonly = 2;
        public bool IsReadonly
        {
            get
            {
                if (_IsReadonly != 2)
                {
                    if (_IsReadonly == 1)
                        return true;
                    else
                        return false;
                }

                if (this.IsDelete || this.IsInsert || this.IsUpdate)
                {
                    _IsReadonly = 0;
                    return false;
                }
                _IsReadonly = 1;
                return true;
            }
        }
        public bool IsDelete
        {
            get
            {
                return this.GetValBooleanByKey(MapDtlAttr.IsDelete);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.IsDelete, value);
            }
        }
        public bool IsInsert
        {
            get
            {
                return this.GetValBooleanByKey(MapDtlAttr.IsInsert);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.IsInsert, value);
            }
        }
        /// <summary>
        ///  Is visible 
        /// </summary>
        public bool IsView
        {
            get
            {
                return this.GetValBooleanByKey(MapDtlAttr.IsView);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.IsView, value);
            }
        }
        public bool IsUpdate
        {
            get
            {
                return this.GetValBooleanByKey(MapDtlAttr.IsUpdate);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.IsUpdate, value);
            }
        }
        /// <summary>
        ///  Whether to enable multiple attachments 
        /// </summary>
        public bool IsEnableAthM
        {
            get
            {
                return this.GetValBooleanByKey(MapDtlAttr.IsEnableAthM);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.IsEnableAthM, value);
            }
        }
        /// <summary>
        ///  Whether to enable the grouping field 
        /// </summary>
        public bool IsEnableGroupField
        {
            get
            {
                return this.GetValBooleanByKey(MapDtlAttr.IsEnableGroupField);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.IsEnableGroupField, value);
            }
        }
        /// <summary>
        ///  Whether hired audit connection 
        /// </summary>
        public bool IsEnablePass
        {
            get
            {
                return this.GetValBooleanByKey(MapDtlAttr.IsEnablePass);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.IsEnablePass, value);
            }
        }
      
        /// <summary>
        ///  Whether copy Data ?
        /// </summary>
        public bool IsCopyNDData
        {
            get
            {
                return this.GetValBooleanByKey(MapDtlAttr.IsCopyNDData);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.IsCopyNDData, value);
            }
        }
        /// <summary>
        ///  Whether enabled many 
        /// </summary>
        public bool IsEnableM2M
        {
            get
            {
                return this.GetValBooleanByKey(MapDtlAttr.IsEnableM2M);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.IsEnableM2M, value);
            }
        }
        /// <summary>
        ///  Whether to enable many more 
        /// </summary>
        public bool IsEnableM2MM
        {
            get
            {
                return this.GetValBooleanByKey(MapDtlAttr.IsEnableM2MM);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.IsEnableM2MM, value);
            }
        }

        public bool IsUse = false;
        /// <summary>
        ///  Check that the permissions staff 
        /// </summary>
        public DtlOpenType DtlOpenType
        {
            get
            {
                return (DtlOpenType)this.GetValIntByKey(MapDtlAttr.DtlOpenType);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.DtlOpenType, (int)value);
            }
        }
        /// <summary>
        ///  Group field 
        /// </summary>
        public string GroupField
        {
            get
            {
                return this.GetValStrByKey(MapDtlAttr.GroupField);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.GroupField, value);
            }
        }
        public string FK_MapData
        {
            get
            {
                return this.GetValStrByKey(MapDtlAttr.FK_MapData);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.FK_MapData, value);
            }
        }
        public int RowsOfList
        {
            get
            {
                return this.GetValIntByKey(MapDtlAttr.RowsOfList);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.RowsOfList, value);
            }
        }
        public int RowIdx
        {
            get
            {
                return this.GetValIntByKey(MapDtlAttr.RowIdx);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.RowIdx, value);
            }
        }
        public int GroupID
        {
            get
            {
                return this.GetValIntByKey(MapDtlAttr.GroupID);
            }
            set
            {
                this.SetValByKey(MapDtlAttr.GroupID, value);
            }
        }
        public string PTable
        {
            get
            {
                string s = this.GetValStrByKey(MapDtlAttr.PTable);
                if (s == "" || s == null)
                {
                    s = this.No;
                    if (s.Substring(0, 1) == "0")
                    {
                        return "T" + this.No;
                    }
                    else
                        return s;
                }
                else
                {
                    if (s.Substring(0, 1) == "0")
                    {
                        return "T" + this.No;
                    }
                    else
                        return s;
                }
            }
            set
            {
                this.SetValByKey(MapDtlAttr.PTable, value);
            }
        }
        /// <summary>
        ///  Multi-header 
        /// </summary>
        public string MTR
        {
            get
            {
                string s= this.GetValStrByKey(MapDtlAttr.MTR);
                s = s.Replace("¡¶","<");
                s = s.Replace( "¡·",">");
                s = s.Replace("[","'");
                return s;
            }
            set
            {
                string s = value;
                s = s.Replace("<","¡¶");
                s = s.Replace(">", "¡·");
                s = s.Replace("'", "[");
                this.SetValByKey(MapDtlAttr.MTR, value);
            }
        }
        #endregion

        #region  Constructor 
        public Map GenerMap()
        {
            bool isdebug = SystemConfig.IsDebug;

            if (isdebug == false)
            {
                Map m = BP.DA.Cash.GetMap(this.No);
                if (m != null)
                    return m;
            }

            MapAttrs mapAttrs = this.MapAttrs;
            Map map = new Map(this.PTable);
            map.EnDesc = this.Name;
            map.EnType = EnType.App;
            map.DepositaryOfEntity = Depositary.None;
            map.DepositaryOfMap = Depositary.Application;

            Attrs attrs = new Attrs();
            foreach (MapAttr mapAttr in mapAttrs)
                map.AddAttr(mapAttr.HisAttr);

            BP.DA.Cash.SetMap(this.No, map);
            return map;
        }
        public GEDtl HisGEDtl
        {
            get
            {
                GEDtl dtl = new GEDtl(this.No);
                return dtl;
            }
        }
        public GEEntity GenerGEMainEntity(string mainPK)
        {
            GEEntity en = new GEEntity(this.FK_MapData, mainPK);
            return en;
        }
        /// <summary>
        ///  Details 
        /// </summary>
        public MapDtl()
        {
        }
        public MapDtl(string mypk)
        {
            this.No = mypk;
            this._IsReadonly = 2;
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
                Map map = new Map("Sys_MapDtl");
                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.EnDesc = " Details ";
                map.EnType = EnType.Sys;

                map.AddTBStringPK(MapDtlAttr.No, null, " Serial number ", true, false, 1, 20, 20);
                map.AddTBString(MapDtlAttr.Name, null, " Description ", true, false, 1, 50, 20);
                map.AddTBString(MapDtlAttr.FK_MapData, null, " Main table ", true, false, 0, 30, 20);
                map.AddTBString(MapDtlAttr.PTable, null, " Physical table ", true, false, 0, 30, 20);
                map.AddTBString(MapDtlAttr.GroupField, null, " Group field ", true, false, 0, 200, 20);

                //map.AddTBInt(MapDtlAttr.Model, 0, " Mode ", false, false);
                map.AddDDLSysEnum(MapDtlAttr.Model, 0, " Mode ", true, true,
                 MapDtlAttr.Model, "@0= General @1= Fixed line ");

                map.AddTBString(MapDtlAttr.ImpFixTreeSql, null, " Fixed column tree SQL", true, false, 0, 500, 20);
                map.AddTBString(MapDtlAttr.ImpFixDataSql, null, " Fixed column data SQL", true, false, 0, 500, 20);

                map.AddTBInt(MapDtlAttr.RowIdx, 99, " Location ", false, false);
                map.AddTBInt(MapDtlAttr.GroupID, 0, "GroupID", false, false);
                map.AddTBInt(MapDtlAttr.RowsOfList, 6, "Rows", false, false);

                map.AddBoolean(MapDtlAttr.IsEnableGroupField, false, " Whether to enable the grouping field ", false, false);

                map.AddBoolean(MapDtlAttr.IsShowSum, true, "IsShowSum", false, false);
                map.AddBoolean(MapDtlAttr.IsShowIdx, true, "IsShowIdx", false, false);
                map.AddBoolean(MapDtlAttr.IsCopyNDData, true, "IsCopyNDData", false, false);
                map.AddBoolean(MapDtlAttr.IsHLDtl, false, " Is confluence summary ", false, false);

                map.AddBoolean(MapDtlAttr.IsReadonly, false, "IsReadonly", false, false);
                map.AddBoolean(MapDtlAttr.IsShowTitle, true, "IsShowTitle", false, false);
                map.AddBoolean(MapDtlAttr.IsView, true, " Is visible ", false, false);

              

                map.AddBoolean(MapDtlAttr.IsInsert, true, "IsInsert", false, false);
                map.AddBoolean(MapDtlAttr.IsDelete, true, "IsDelete", false, false);
                map.AddBoolean(MapDtlAttr.IsUpdate, true, "IsUpdate", false, false);

                map.AddBoolean(MapDtlAttr.IsEnablePass, false, " Whether auditing is enabled by ?", false, false);
                map.AddBoolean(MapDtlAttr.IsEnableAthM, false, " Whether to enable multiple attachments ", false, false);

                map.AddBoolean(MapDtlAttr.IsEnableM2M, false, " Whether to enable M2M", false, false);
                map.AddBoolean(MapDtlAttr.IsEnableM2MM, false, " Whether to enable M2M", false, false);
                map.AddDDLSysEnum(MapDtlAttr.WhenOverSize, 0, "WhenOverSize", true, true,
                 MapDtlAttr.WhenOverSize, "@0= Does not deal with @1= Down along by line @2= Page Display ");

                map.AddDDLSysEnum(MapDtlAttr.DtlOpenType, 1, " Open Data Types ", true, true,
                    MapDtlAttr.DtlOpenType, "@0= The operator @1= The work ID@2= Process ID");

                map.AddDDLSysEnum(MapDtlAttr.DtlShowModel, 0, " Display Format ", true, true,
               MapDtlAttr.DtlShowModel, "@0= Form @1= Card ");

                map.AddTBFloat(MapDtlAttr.X, 5, "X", true, false);
                map.AddTBFloat(MapDtlAttr.Y, 5, "Y", false, false);

                map.AddTBFloat(MapDtlAttr.H, 150, "H", true, false);
                map.AddTBFloat(MapDtlAttr.W, 200, "W", false, false);

                map.AddTBFloat(MapDtlAttr.FrmW, 900, "FrmW", true, true);
                map.AddTBFloat(MapDtlAttr.FrmH, 1200, "FrmH", true, true);

                //MTR  Multi-header columns .
                map.AddTBString(MapDtlAttr.MTR, null, " Multi-header columns ", true, false, 0, 3000, 20);
                map.AddTBString(FrmBtnAttr.GUID, null, "GUID", true, false, 0, 128, 20);

                //add 2014-02-21.
                map.AddTBInt(MapDtlAttr.FK_Node, 0, " Node ( User access control process forms )", false, false);

                // Parameters .
                map.AddTBAtParas(300);

                #region  Import and export filling .
                // 2014-07-17 for xinchang bank.
                map.AddBoolean(MapDtlAttr.IsExp, true, "IsExp", false, false);
                map.AddBoolean(MapDtlAttr.IsImp, true, "IsImp", false, false);
                map.AddBoolean(MapDtlAttr.IsEnableSelectImp, false, " Select whether to enable data import ?", false, false);
                map.AddTBString(MapDtlAttr.ImpSQLSearch, null, " Inquiry SQL", true, false, 0, 500, 20);
                map.AddTBString(MapDtlAttr.ImpSQLInit, null, " Initialization SQL", true, false, 0, 500, 20);
                map.AddTBString(MapDtlAttr.ImpSQLFull, null, " Data padding SQL", true, false, 0, 500, 20);
                #endregion  Import and export filling .



                this._enMap = map;
                return this._enMap;
            }
        }

        #region  Basic properties .
        public float X
        {
            get
            {
                return this.GetValFloatByKey(FrmImgAttr.X);
            }
        }
        public float Y
        {
            get
            {
                return this.GetValFloatByKey(FrmImgAttr.Y);
            }
        }
        public float W
        {
            get
            {
                return this.GetValFloatByKey(FrmImgAttr.W);
            }
        }
        public float H
        {
            get
            {
                return this.GetValFloatByKey(FrmImgAttr.H);
            }
        }
        public float FrmW
        {
            get
            {
                return this.GetValFloatByKey(MapDtlAttr.FrmW);
            }
        }
        public float FrmH
        {
            get
            {
                return this.GetValFloatByKey(MapDtlAttr.FrmH);
            }
        }
        #endregion  Basic properties .

        /// <summary>
        ///  Get the number of 
        /// </summary>
        /// <param name="fk_val"></param>
        /// <returns></returns>
        public int GetCountByFK(int workID)
        {
            return BP.DA.DBAccess.RunSQLReturnValInt("select COUNT(OID) from " + this.PTable + " WHERE WorkID=" + workID);
        }
        public int GetCountByFK(string field, string val)
        {
            return BP.DA.DBAccess.RunSQLReturnValInt("select COUNT(OID) from " + this.PTable + " WHERE " + field + "='" + val + "'");
        }
        public int GetCountByFK(string field, Int64 val)
        {
            return BP.DA.DBAccess.RunSQLReturnValInt("select COUNT(OID) from " + this.PTable + " WHERE " + field + "=" + val);
        }
        public int GetCountByFK(string f1, Int64 val1, string f2, string val2)
        {
            return BP.DA.DBAccess.RunSQLReturnValInt("SELECT COUNT(OID) from " + this.PTable + " WHERE " + f1 + "=" + val1 + " AND " + f2 + "='" + val2 + "'");
        }
        #endregion

        public void IntMapAttrs()
        {
            BP.Sys.MapData md = new BP.Sys.MapData();
            md.No = this.No;
            if (md.RetrieveFromDBSources() == 0)
            {
                md.Name = this.Name;
                md.Insert();
            }

            MapAttrs attrs = new MapAttrs(this.No);
            BP.Sys.MapAttr attr = new BP.Sys.MapAttr();
            if (attrs.Contains(MapAttrAttr.KeyOfEn, "OID") == false)
            {
                attr = new BP.Sys.MapAttr();
                attr.FK_MapData = this.No;
                attr.HisEditType = EditType.Readonly;

                attr.KeyOfEn = "OID";
                attr.Name = " Primary key ";
                attr.MyDataType = BP.DA.DataType.AppInt;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = false;
                attr.UIIsEnable = false;
                attr.DefVal = "0";
                attr.Insert();
            }

            if (attrs.Contains(MapAttrAttr.KeyOfEn, "RefPK") == false)
            {
                attr = new BP.Sys.MapAttr();
                attr.FK_MapData = this.No;
                attr.HisEditType = EditType.Readonly;

                attr.KeyOfEn = "RefPK";
                attr.Name = " Associate ID";
                attr.MyDataType = BP.DA.DataType.AppString;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = false;
                attr.UIIsEnable = false;
                attr.DefVal = "0";
                attr.Insert();
            }

            if (attrs.Contains(MapAttrAttr.KeyOfEn, "FID") == false)
            {
                attr = new BP.Sys.MapAttr();
                attr.FK_MapData = this.No;
                attr.HisEditType = EditType.Readonly;

                attr.KeyOfEn = "FID";
                attr.Name = "FID";
                attr.MyDataType = BP.DA.DataType.AppInt;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = false;
                attr.UIIsEnable = false;
                attr.DefVal = "0";
                attr.Insert();
            }

            if (attrs.Contains(MapAttrAttr.KeyOfEn, "RDT") == false)
            {
                attr = new BP.Sys.MapAttr();
                attr.FK_MapData = this.No;
                attr.HisEditType = EditType.UnDel;

                attr.KeyOfEn = "RDT";
                attr.Name = " Record Time ";
                attr.MyDataType = BP.DA.DataType.AppDateTime;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = false;
                attr.UIIsEnable = false;
                attr.Tag = "1";
                attr.Insert();
            }

            if (attrs.Contains(MapAttrAttr.KeyOfEn, "Rec") == false)
            {
                attr = new BP.Sys.MapAttr();
                attr.FK_MapData = this.No;
                attr.HisEditType = EditType.Readonly;

                attr.KeyOfEn = "Rec";
                attr.Name = " Record people ";
                attr.MyDataType = BP.DA.DataType.AppString;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = false;
                attr.UIIsEnable = false;
                attr.MaxLen = 20;
                attr.MinLen = 0;
                attr.DefVal = "@WebUser.No";
                attr.Tag = "@WebUser.No";
                attr.Insert();
            }
        }
        private void InitExtMembers()
        {
            /*  If multiple attachments enabled */
            if (this.IsEnableAthM)
            {
                BP.Sys.FrmAttachment athDesc = new BP.Sys.FrmAttachment();
                athDesc.MyPK = this.No + "_AthM";
                if (athDesc.RetrieveFromDBSources() == 0)
                {
                    athDesc.FK_MapData = this.No;
                    athDesc.NoOfObj = "AthM";
                    athDesc.Name = this.Name;
                    athDesc.Insert();
                }
            }

            if (this.IsEnableM2M)
            {
                MapM2M m2m = new MapM2M();
                m2m.MyPK = this.No + "_M2M";
                m2m.Name = "M2M";
                m2m.NoOfObj = "M2M";
                m2m.FK_MapData = this.No;
                if (m2m.RetrieveFromDBSources() == 0)
                {
                    m2m.FK_MapData = this.No;
                    m2m.NoOfObj = "M2M";
                    m2m.Insert();
                }
            }

            if (this.IsEnableM2MM)
            {
                MapM2M m2m = new MapM2M();
                m2m.MyPK = this.No + "_M2MM";
                m2m.Name = "M2MM";
                m2m.NoOfObj = "M2MM";
                m2m.FK_MapData = this.No;
                if (m2m.RetrieveFromDBSources() == 0)
                {
                    m2m.FK_MapData = this.No;
                    m2m.NoOfObj = "M2MM";
                    m2m.Insert();
                }
            }
        }
        protected override bool beforeInsert()
        {
            this.InitExtMembers();
            return base.beforeInsert();
        }
        protected override bool beforeUpdateInsertAction()
        {
            BP.Sys.MapData md = new BP.Sys.MapData();
            md.No = this.No;
            if (md.RetrieveFromDBSources() == 0)
            {
                md.Name = this.Name;
                md.Insert();
            }

            if (this.IsRowLock == true)
            {
                /* Inspection */
                MapAttrs attrs = new MapAttrs(this.No);
                if (attrs.Contains(MapAttrAttr.KeyOfEn, "IsRowLock") == false)
                    throw new Exception(" You enabled from the form (" + this.Name + ") Line data lock function , However, this is not from the table IsRowLock Field , Please refer to the help documentation .");
            }

            if (this.IsEnablePass)
            {
                /* Determine whether there is IsPass  Field .*/
                MapAttrs attrs = new MapAttrs(this.No);
                if (attrs.Contains(MapAttrAttr.KeyOfEn, "IsPass") == false)
                    throw new Exception(" You enabled from the form (" + this.Name + ") Review of data options , However, this is not from the table IsPass Field , Please refer to the help documentation .");
            }
            return base.beforeUpdateInsertAction();
        }
        protected override bool beforeUpdate()
        {
            MapAttrs attrs = new MapAttrs(this.No);
            bool isHaveEnable = false;
            foreach (MapAttr attr in attrs)
            {
                if (attr.UIIsEnable && attr.UIContralType == UIContralType.TB)
                    isHaveEnable = true;
            }
            this.InitExtMembers();
            return base.beforeUpdate();
        }
        protected override bool beforeDelete()
        {
            string sql = "";
            sql += "@DELETE FROM Sys_FrmLine WHERE FK_MapData='" + this.No + "'";
            sql += "@DELETE FROM Sys_FrmLab WHERE FK_MapData='" + this.No + "'";
            sql += "@DELETE FROM Sys_FrmLink WHERE FK_MapData='" + this.No + "'";
            sql += "@DELETE FROM Sys_FrmImg WHERE FK_MapData='" + this.No + "'";
            sql += "@DELETE FROM Sys_FrmImgAth WHERE FK_MapData='" + this.No + "'";
            sql += "@DELETE FROM Sys_FrmRB WHERE FK_MapData='" + this.No + "'";
            sql += "@DELETE FROM Sys_FrmAttachment WHERE FK_MapData='" + this.No + "'";
            sql += "@DELETE FROM Sys_MapFrame WHERE FK_MapData='" + this.No + "'";
            sql += "@DELETE FROM Sys_MapExt WHERE FK_MapData='" + this.No + "'";
            sql += "@DELETE FROM Sys_MapAttr WHERE FK_MapData='" + this.No + "'";
            sql += "@DELETE FROM Sys_MapData WHERE No='" + this.No + "'";
            sql += "@DELETE FROM Sys_GroupField WHERE EnName='" + this.No + "'";
            sql += "@DELETE FROM Sys_MapM2M WHERE FK_MapData='" + this.No + "'";
            DBAccess.RunSQLs(sql);
            try
            {
                BP.DA.DBAccess.RunSQL("DROP TABLE " + this.PTable);
            }
            catch
            {
            }
            return base.beforeDelete();
        }
    }
    /// <summary>
    ///  Details s
    /// </summary>
    public class MapDtls : EntitiesNoName
    {
        #region  Structure 
        /// <summary>
        ///  Details s
        /// </summary>
        public MapDtls()
        {
        }
        /// <summary>
        ///  Details s
        /// </summary>
        /// <param name="fk_mapdata">s</param>
        public MapDtls(string fk_mapdata)
        {
            this.Retrieve(MapDtlAttr.FK_MapData, fk_mapdata, MapDtlAttr.FK_Node, 0, MapDtlAttr.No);
        }
        /// <summary>
        ///  Get it  Entity
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new MapDtl();
            }
        }
        #endregion
    }
}
