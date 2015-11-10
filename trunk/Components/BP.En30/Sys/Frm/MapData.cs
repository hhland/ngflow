using System;
using System.Data;
using System.Collections;
using BP.DA;
using BP.En;
using System.Collections.Generic;

namespace BP.Sys
{
    /// <summary>
    ///  Query by date 
    /// </summary>
    public enum DTSearchWay
    {
        /// <summary>
        ///  Not set 
        /// </summary>
        None,
        /// <summary>
        ///  By date 
        /// </summary>
        ByDate,
        /// <summary>
        ///  By date and time 
        /// </summary>
        ByDateTime
    }
    /// <summary>
    ///  Application Type 
    /// </summary>
    public enum AppType
    {
        /// <summary>
        ///  Process Form 
        /// </summary>
        Application = 0,
        /// <summary>
        ///  Node Form 
        /// </summary>
        Node = 1
    }
    public enum FrmFrom
    {
        Flow,
        Node,
        Dtl
    }
    /// <summary>
    ///  Form type 
    /// </summary>
    public enum FrmType
    {
        /// <summary>
        ///  Freedom Form 
        /// </summary>
        AspxFrm = 0,
        /// <summary>
        ///  Fool form 
        /// </summary>
        Column4Frm = 1,
        /// <summary>
        /// silverlight
        /// </summary>
        SLFrm = 2,
        /// <summary>
        /// URL  Form ( Custom )
        /// </summary>
        Url = 3,
        /// <summary>
        /// Word Type form 
        /// </summary>
        WordFrm = 4,
        /// <summary>
        /// Excel Type form 
        /// </summary>
        ExcelFrm = 5
    }
    /// <summary>
    ///  Mapping foundation 
    /// </summary>
    public class MapDataAttr : EntityNoNameAttr
    {
        public const string PTable = "PTable";
        public const string Dtls = "Dtls";
        public const string EnPK = "EnPK";
        public const string FrmW = "FrmW";
        public const string FrmH = "FrmH";
        /// <summary>
        ///  Table columns ( Effective form for fools )
        /// </summary>
        public const string TableCol = "TableCol";
        /// <summary>
        ///  Table width 
        /// </summary>
        public const string TableWidth = "TableWidth";
        /// <summary>
        ///  Source 
        /// </summary>
        public const string FrmFrom = "FrmFrom";
        /// <summary>
        /// DBURL
        /// </summary>
        public const string DBURL = "DBURL";
        /// <summary>
        ///  Designers 
        /// </summary>
        public const string Designer = "Designer";
        /// <summary>
        ///  Designers unit 
        /// </summary>
        public const string DesignerUnit = "DesignerUnit";
        /// <summary>
        ///  Contact the designer 
        /// </summary>
        public const string DesignerContact = "DesignerContact";
        /// <summary>
        ///  Forms category 
        /// </summary>
        public const string FK_FrmSort = "FK_FrmSort";
        /// <summary>
        ///  Form tree category 
        /// </summary>
        public const string FK_FormTree = "FK_FormTree";
        /// <summary>
        ///  Application Type 
        /// </summary>
        public const string AppType = "AppType";
        /// <summary>
        ///  Form type 
        /// </summary>
        public const string FrmType = "FrmType";
        /// <summary>
        /// Url( For custom form is valid )
        /// </summary>
        public const string Url = "Url";
        /// <summary>
        /// Tag
        /// </summary>
        public const string Tag = "Tag";
        /// <summary>
        ///  Remark 
        /// </summary>
        public const string Note = "Note";
        /// <summary>
        /// Idx
        /// </summary>
        public const string Idx = "Idx";
        /// <summary>
        /// GUID
        /// </summary>
        public const string GUID = "GUID";
        /// <summary>
        ///  The version number 
        /// </summary>
        public const string Ver = "Ver";

        #region  Report Properties ( Mode parameter storage ).
        /// <summary>
        ///  Are Keyword Search 
        /// </summary>
        public const string RptIsSearchKey = "RptIsSearchKey";
        /// <summary>
        ///  Time query 
        /// </summary>
        public const string RptDTSearchWay = "RptDTSearchWay";
        /// <summary>
        ///  Time Field 
        /// </summary>
        public const string RptDTSearchKey = "RptDTSearchKey";
        /// <summary>
        ///  Check foreign key field enumeration 
        /// </summary>
        public const string RptSearchKeys = "RptSearchKeys";
        #endregion  Report Properties ( Mode parameter storage ).

        #region  Other computational properties , Parameter storage .
        /// <summary>
        ///  The leftmost value 
        /// </summary>
        public const string MaxLeft = "MaxLeft";
        /// <summary>
        ///  Rightmost value 
        /// </summary>
        public const string MaxRight = "MaxRight";
        /// <summary>
        ///  The value of the head 
        /// </summary>
        public const string MaxTop = "MaxTop";
        /// <summary>
        ///  Value bottommost 
        /// </summary>
        public const string MaxEnd = "MaxEnd";
        #endregion  Other computational properties , Parameter storage .


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
    }
    /// <summary>
    ///  Mapping foundation 
    /// </summary>
    public class MapData : EntityNoName
    {
        #region weboffice Document Properties ( Parameter Properties )

        /// <summary>
        ///  Whether to enable the locked row 
        /// </summary>
        public bool IsRowLock
        {
            get
            {
                return this.GetParaBoolen(FrmAttachmentAttr.IsRowLock, false);
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

        #region  Automatic calculation of property .
        public float MaxLeft
        {
            get
            {
                return this.GetParaFloat(MapDataAttr.MaxLeft);
            }
            set
            {
                this.SetPara(MapDataAttr.MaxLeft, value);
            }
        }
        public float MaxRight
        {
            get
            {
                return this.GetParaFloat(MapDataAttr.MaxRight);
            }
            set
            {
                this.SetPara(MapDataAttr.MaxRight, value);
            }
        }
        public float MaxTop
        {
            get
            {
                return this.GetParaFloat(MapDataAttr.MaxTop);
            }
            set
            {
                this.SetPara(MapDataAttr.MaxTop, value);
            }
        }
        public float MaxEnd
        {
            get
            {
                return this.GetParaFloat(MapDataAttr.MaxEnd);
            }
            set
            {
                this.SetPara(MapDataAttr.MaxEnd, value);
            }
        }
        #endregion  Automatic calculation of property .

        #region  Report Properties ( Parameters stored ).
        /// <summary>
        ///  Are Keyword Search 
        /// </summary>
        public bool RptIsSearchKey
        {
            get
            {
                return this.GetParaBoolen(MapDataAttr.RptIsSearchKey, true);
            }
            set
            {
                this.SetPara(MapDataAttr.RptIsSearchKey, value);
            }
        }
        /// <summary>
        ///  Time query 
        /// </summary>
        public DTSearchWay RptDTSearchWay
        {
            get
            {
                return (DTSearchWay)this.GetParaInt(MapDataAttr.RptDTSearchWay);
            }
            set
            {
                this.SetPara(MapDataAttr.RptDTSearchWay, (int)value);
            }
        }
        /// <summary>
        ///  Time Field 
        /// </summary>
        public string RptDTSearchKey
        {
            get
            {
                return this.GetParaString(MapDataAttr.RptDTSearchKey);
            }
            set
            {
                this.SetPara(MapDataAttr.RptDTSearchKey, value);
            }
        }
        /// <summary>
        ///  Check foreign key field enumeration 
        /// </summary>
        public string RptSearchKeys
        {
            get
            {
                return this.GetParaString(MapDataAttr.RptSearchKeys, "*");
            }
            set
            {
                this.SetPara(MapDataAttr.RptSearchKeys, value);
            }
        }
        #endregion  Report Properties ( Parameters stored ).

        #region  Foreign key attribute 
        public string Ver
        {
            get
            {
                return this.GetValStringByKey(MapDataAttr.Ver);
            }
            set
            {
                this.SetValByKey(MapDataAttr.Ver, value);
            }
        }
        /// <summary>
        ///  Sequence number 
        /// </summary>
        public int Idx
        {
            get
            {
                return this.GetValIntByKey(MapDataAttr.Idx);
            }
            set
            {
                this.SetValByKey(MapDataAttr.Idx, value);
            }
        }
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
        public GroupFields GroupFields
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
        ///  Report form 
        /// </summary>
        public FrmRpts FrmRpts
        {
            get
            {
                FrmRpts obj = this.GetRefObject("FrmRpts") as FrmRpts;
                if (obj == null)
                {
                    obj = new FrmRpts(this.No);
                    this.SetRefObject("FrmRpts", obj);
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

        public static Boolean IsEditDtlModel
        {
            get
            {
                string s = BP.Web.WebUser.GetSessionByKey("IsEditDtlModel", "0");
                if (s == "0")
                    return false;
                else
                    return true;
            }
            set
            {
                BP.Web.WebUser.SetSessionByKey("IsEditDtlModel", "1");
            }
        }


        #region  Property 
        /// <summary>
        ///  Physical table 
        /// </summary>
        public string PTable
        {
            get
            {
                string s = this.GetValStrByKey(MapDataAttr.PTable);
                if (s == "" || s == null)
                    return this.No;
                return s;
            }
            set
            {
                this.SetValByKey(MapDataAttr.PTable, value);
            }
        }
        /// <summary>
        /// URL
        /// </summary>
        public string Url
        {
            get
            {
                return this.GetValStrByKey(MapDataAttr.Url);
            }
            set
            {
                this.SetValByKey(MapDataAttr.Url, value);
            }
        }
        public DBUrlType HisDBUrl
        {
            get
            {
                return DBUrlType.AppCenterDSN;
                // return (DBUrlType)this.GetValIntByKey(MapDataAttr.DBURL);
            }
        }
        public int HisFrmTypeInt
        {
            get
            {
                return this.GetValIntByKey(MapDataAttr.FrmType);
            }
            set
            {
                this.SetValByKey(MapDataAttr.FrmType, value);
            }
        }
        public FrmType HisFrmType
        {
            get
            {
                return (FrmType)this.GetValIntByKey(MapDataAttr.FrmType);
            }
            set
            {
                this.SetValByKey(MapDataAttr.FrmType, (int)value);
            }
        }
        public AppType HisAppType
        {
            get
            {
                return (AppType)this.GetValIntByKey(MapDataAttr.AppType);
            }
            set
            {
                this.SetValByKey(MapDataAttr.AppType, (int)value);
            }
        }
        /// <summary>
        ///  Remark 
        /// </summary>
        public string Note
        {
            get
            {
                return this.GetValStrByKey(MapDataAttr.Note);
            }
            set
            {
                this.SetValByKey(MapDataAttr.Note, value);
            }
        }
        /// <summary>
        ///  Are there CA.
        /// </summary>
        public bool IsHaveCA
        {
            get
            {
                return this.GetParaBoolen("IsHaveCA", false);

            }
            set
            {
                this.SetPara("IsHaveCA", value);
            }
        }
        /// <summary>
        ///  Category , Can be empty .
        /// </summary>
        public string FK_FrmSort
        {
            get
            {
                return this.GetValStrByKey(MapDataAttr.FK_FrmSort);
            }
            set
            {
                this.SetValByKey(MapDataAttr.FK_FrmSort, value);
            }
        }
        /// <summary>
        ///  Category , Can be empty .
        /// </summary>
        public string FK_FormTree
        {
            get
            {
                return this.GetValStrByKey(MapDataAttr.FK_FormTree);
            }
            set
            {
                this.SetValByKey(MapDataAttr.FK_FormTree, value);
            }
        }
        /// <summary>
        ///  Collection from the table .
        /// </summary>
        public string Dtls
        {
            get
            {
                return this.GetValStrByKey(MapDataAttr.Dtls);
            }
            set
            {
                this.SetValByKey(MapDataAttr.Dtls, value);
            }
        }
        /// <summary>
        ///  Primary key 
        /// </summary>
        public string EnPK
        {
            get
            {
                string s = this.GetValStrByKey(MapDataAttr.EnPK);
                if (string.IsNullOrEmpty(s))
                    return "OID";
                return s;
            }
            set
            {
                this.SetValByKey(MapDataAttr.EnPK, value);
            }
        }
        public Entities _HisEns = null;
        public new Entities HisEns
        {
            get
            {
                if (_HisEns == null)
                {
                    _HisEns = BP.En.ClassFactory.GetEns(this.No);
                }
                return _HisEns;
            }
        }
        public Entity HisEn
        {
            get
            {
                return this.HisEns.GetNewEntity;
            }
        }
        public float FrmW
        {
            get
            {
                return this.GetValFloatByKey(MapDataAttr.FrmW);
            }
            set
            {
                this.SetValByKey(MapDataAttr.FrmW, value);
            }
        }
        ///// <summary>
        /////  Forms control scheme 
        ///// </summary>
        //public string Slns
        //{
        //    get
        //    {
        //        return this.GetValStringByKey(MapDataAttr.Slns);
        //    }
        //    set
        //    {
        //        this.SetValByKey(MapDataAttr.Slns, value);
        //    }
        //}
        public float FrmH
        {
            get
            {
                return this.GetValFloatByKey(MapDataAttr.FrmH);
            }
            set
            {
                this.SetValByKey(MapDataAttr.FrmH, value);
            }
        }
        /// <summary>
        ///  Column of the table shows 
        /// </summary>
        public int TableCol
        {
            get
            {
                int i = this.GetValIntByKey(MapDataAttr.TableCol);
                if (i == 0 || i == 1)
                    return 4;
                return i;
            }
            set
            {
                this.SetValByKey(MapDataAttr.TableCol, value);
            }
        }
        public string TableWidth
        {
            get
            {
                //switch (this.TableCol)
                //{
                //    case 2:
                //        return
                //        labCol = 25;
                //        ctrlCol = 75;
                //        break;
                //    case 4:
                //        labCol = 20;
                //        ctrlCol = 30;
                //        break;
                //    case 6:
                //        labCol = 15;
                //        ctrlCol = 30;
                //        break;
                //    case 8:
                //        labCol = 10;
                //        ctrlCol = 15;
                //        break;
                //    default:
                //        break;
                //}


                int i = this.GetValIntByKey(MapDataAttr.TableWidth);
                if (i <= 50)
                    return "100%";
                return i + "px";
            }
        }
        #endregion

        #region  Constructor 
        public Map GenerHisMap()
        {
            MapAttrs mapAttrs = this.MapAttrs;
            if (mapAttrs.Count == 0)
            {
                this.RepairMap();
                mapAttrs = this.MapAttrs;
            }

            Map map = new Map(this.PTable);
            DBUrl u = new DBUrl(this.HisDBUrl);
            map.EnDBUrl = u;
            map.EnDesc = this.Name;
            map.EnType = EnType.App;
            map.DepositaryOfEntity = Depositary.None;
            map.DepositaryOfMap = Depositary.Application;

            Attrs attrs = new Attrs();
            foreach (MapAttr mapAttr in mapAttrs)
                map.AddAttr(mapAttr.HisAttr);

            //  Produced from table .
            MapDtls dtls = this.MapDtls; // new MapDtls(this.No);
            foreach (MapDtl dtl in dtls)
            {
                GEDtls dtls1 = new GEDtls(dtl.No);
                map.AddDtl(dtls1, "RefPK");
            }

            #region  Query conditions .
            map.IsShowSearchKey = this.RptIsSearchKey; // Whether to enable keyword search .
            //  Query by date .
            map.DTSearchWay = this.RptDTSearchWay; // Date query .
            map.DTSearchKey = this.RptDTSearchKey; // Date field .

            // Adding foreign key query field .
            string[] keys = this.RptSearchKeys.Split('*');
            foreach (string key in keys)
            {
                if (string.IsNullOrEmpty(key))
                    continue;

                map.AddSearchAttr(key);
            }
            #endregion  Query conditions .

            return map;
        }
        private GEEntity _HisEn = null;
        public GEEntity HisGEEn
        {
            get
            {
                if (this._HisEn == null)
                    _HisEn = new GEEntity(this.No);
                return _HisEn;
            }
        }
        /// <summary>
        ///  Entity 
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public GEEntity GenerGEEntityByDataSet(DataSet ds)
        {
            // New  Its instances .
            GEEntity en = this.HisGEEn;

            //  Its table.
            DataTable dt = ds.Tables[this.No];

            // Loading data .
            en.Row.LoadDataTable(dt, dt.Rows[0]);

            // dtls.
            MapDtls dtls = this.MapDtls;
            foreach (MapDtl item in dtls)
            {
                DataTable dtDtls = ds.Tables[item.No];
                GEDtls dtlsEn = new GEDtls(item.No);
                foreach (DataRow dr in dtDtls.Rows)
                {
                    //  Produced it Entity data.
                    GEDtl dtl = (GEDtl)dtlsEn.GetNewEntity;
                    dtl.Row.LoadDataTable(dtDtls, dr);

                    // Join this collection .
                    dtlsEn.AddEntity(dtl);
                }

                // Added to his collection in .
                en.Dtls.Add(dtDtls);
            }
            return en;
        }
        /// <summary>
        ///  Generate map.
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public static Map GenerHisMap(string no)
        {
            if (SystemConfig.IsDebug)
            {
                MapData md = new MapData();
                md.No = no;
                md.Retrieve();
                return md.GenerHisMap();
            }
            else
            {
                Map map = BP.DA.Cash.GetMap(no);
                if (map == null)
                {
                    MapData md = new MapData();
                    md.No = no;
                    md.Retrieve();
                    map = md.GenerHisMap();
                    BP.DA.Cash.SetMap(no, map);
                }
                return map;
            }
        }
        /// <summary>
        ///  Mapping foundation 
        /// </summary>
        public MapData()
        {
        }
        /// <summary>
        ///  Mapping foundation 
        /// </summary>
        /// <param name="no"> Number Mapping </param>
        public MapData(string no)
            : base(no)
        {
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
                Map map = new Map("Sys_MapData");
                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.EnDesc = " Mapping foundation ";
                map.EnType = EnType.Sys;
                map.CodeStruct = "4";

                map.AddTBStringPK(MapDataAttr.No, null, " Serial number ", true, false, 1, 20, 20);
                map.AddTBString(MapDataAttr.Name, null, " Description ", true, false, 0, 500, 20);
                map.AddTBString(MapDataAttr.EnPK, null, " Entity primary key ", true, false, 0, 10, 20);
                map.AddTBString(MapDataAttr.PTable, null, " Physical table ", true, false, 0, 500, 20);
                map.AddTBString(MapDataAttr.Url, null, " Connection ( Valid for custom forms )", true, false, 0, 500, 20);
                map.AddTBString(MapDataAttr.Dtls, null, " From Table ", true, false, 0, 500, 20);

                // Format : @1= Program name 1@2= Program name 2@3= Program name 3
                // map.AddTBString(MapDataAttr.Slns, null, " Forms Control Solutions ", true, false, 0, 500, 20);

                map.AddTBInt(MapDataAttr.FrmW, 900, "FrmW", true, true);
                map.AddTBInt(MapDataAttr.FrmH, 1200, "FrmH", true, true);

                map.AddTBInt(MapDataAttr.TableCol, 4, " Fool column form displays ", true, true);
                map.AddTBInt(MapDataAttr.TableWidth, 600, " Table width ", true, true);

                // Data Sources .
                map.AddTBInt(MapDataAttr.DBURL, 0, "DBURL", true, false);

                //Tag
                map.AddTBString(MapDataAttr.Tag, null, "Tag", true, false, 0, 500, 20);

                //FrmType  @ Freedom Form ,@ Fool form ,@ Custom Form .
                map.AddTBInt(MapDataAttr.FrmType, 0, " Form type ", true, false);


                //  This field can be empty .
                map.AddTBString(MapDataAttr.FK_FrmSort, null, " Forms category ", true, false, 0, 500, 20);
                map.AddTBString(MapDataAttr.FK_FormTree, null, " Form tree category ", true, false, 0, 500, 20);

                // enumAppType
                map.AddTBInt(MapDataAttr.AppType, 1, " Application Type ", true, false);

                map.AddTBString(MapDataAttr.Note, null, " Remark ", true, false, 0, 500, 20);
                map.AddTBString(MapDataAttr.Designer, null, " Designers ", true, false, 0, 500, 20);
                map.AddTBString(MapDataAttr.DesignerUnit, null, " Unit ", true, false, 0, 500, 20);
                map.AddTBString(MapDataAttr.DesignerContact, null, " Contact ", true, false, 0, 500, 20);

                // Increase parameter field .
                map.AddTBAtParas(4000);

                map.AddTBInt(MapDataAttr.Idx, 100, " Sequence number ", true, true);
                map.AddTBString(MapDataAttr.GUID, null, "GUID", true, false, 0, 128, 20);
                map.AddTBString(MapDataAttr.Ver, null, " The version number ", true, false, 0, 30, 20);
                this._enMap = map;
                return this._enMap;
            }
        }

        /// <summary>
        ///  Move 
        /// </summary>
        public void DoUp()
        {
            this.DoOrderUp(MapDataAttr.FK_FormTree, this.FK_FormTree, MapDataAttr.Idx);
        }
        /// <summary>
        ///  Down 
        /// </summary>
        public void DoOrderDown()
        {
            this.DoOrderDown(MapDataAttr.FK_FormTree, this.FK_FormTree, MapDataAttr.Idx);
        }
        #endregion

        /// <summary>
        ///  Importing 
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static MapData ImpMapData(DataSet ds)
        {
            return ImpMapData(ds, true);
        }
        /// <summary>
        ///  Importing Data 
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="isSetReadony"></param>
        /// <returns></returns>
        public static MapData ImpMapData(DataSet ds, bool isSetReadony)
        {
            string errMsg = "";
            if (ds.Tables.Contains("WF_Flow") == true)
                errMsg += "@ This template file for the process template .";

            if (ds.Tables.Contains("Sys_MapAttr") == false)
                errMsg += "@ Missing table :Sys_MapAttr";

            if (ds.Tables.Contains("Sys_MapData") == false)
                errMsg += "@ Missing table :Sys_MapData";
            if (errMsg != "")
                throw new Exception(errMsg);

            DataTable dt = ds.Tables["Sys_MapData"];
            string fk_mapData = dt.Rows[0]["No"].ToString();
            MapData md = new MapData();
            md.No = fk_mapData;
            if (md.IsExits)
                throw new Exception(" Already exists (" + fk_mapData + ") Data .");

            // Importing .
            return ImpMapData(fk_mapData, ds, isSetReadony);
        }
        /// <summary>
        ///  Import form 
        /// </summary>
        /// <param name="fk_mapdata"> Form ID</param>
        /// <param name="ds"> Form data </param>
        /// <param name="isSetReadonly"> Whether to set read-only ?</param>
        /// <returns></returns>
        public static MapData ImpMapData(string fk_mapdata, DataSet ds, bool isSetReadonly)
        {
            try
            {
                #region  Check the imported data is complete .
                string errMsg = "";
                //if (ds.Tables[0].TableName != "Sys_MapData")
                //    errMsg += "@ Non-form template .";

                if (ds.Tables.Contains("WF_Flow") == true)
                    errMsg += "@ This template file for the process template .";

                if (ds.Tables.Contains("Sys_MapAttr") == false)
                    errMsg += "@ Missing table :Sys_MapAttr";

                if (ds.Tables.Contains("Sys_MapData") == false)
                    errMsg += "@ Missing table :Sys_MapData";

                DataTable dtCheck = ds.Tables["Sys_MapAttr"];
                bool isHave = false;
                foreach (DataRow dr in dtCheck.Rows)
                {
                    if (dr["KeyOfEn"].ToString() == "OID")
                    {
                        isHave = true;
                        break;
                    }
                }

                if (isHave == false)
                    errMsg += "@ Missing Column :OID";

                if (errMsg != "")
                    throw new Exception(" The following error can not be imported , Possible reasons for non-form template file :" + errMsg);
                #endregion

                //  Defined in the last executed sql.
                string endDoSQL = "";

                // Check OID Field .
                MapData mdOld = new MapData();
                mdOld.No = fk_mapdata;
                mdOld.RetrieveFromDBSources();
                mdOld.Delete();

                //  Obtained datasetµÄmap.
                string oldMapID = "";
                DataTable dtMap = ds.Tables["Sys_MapData"];
                foreach (DataRow dr in dtMap.Rows)
                {
                    if (dr["No"].ToString().Contains("Dtl"))
                        continue;
                    oldMapID = dr["No"].ToString();
                }

                string timeKey = DateTime.Now.ToString("MMddHHmmss");
                // string timeKey = fk_mapdata;
                foreach (DataTable dt in ds.Tables)
                {
                    int idx = 0;
                    switch (dt.TableName)
                    {
                        case "Sys_MapDtl":
                            foreach (DataRow dr in dt.Rows)
                            {
                                MapDtl dtl = new MapDtl();
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    object val = dr[dc.ColumnName] as object;
                                    if (val == null)
                                        continue;
                                    dtl.SetValByKey(dc.ColumnName, val.ToString().Replace(oldMapID, fk_mapdata));
                                }
                                if (isSetReadonly)
                                {
                                    //dtl.IsReadonly = true;

                                    dtl.IsInsert = false;
                                    dtl.IsUpdate = false;
                                    dtl.IsDelete = false;
                                }

                                dtl.Insert();
                            }
                            break;
                        case "Sys_MapData":
                            foreach (DataRow dr in dt.Rows)
                            {
                                MapData md = new MapData();
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    object val = dr[dc.ColumnName] as object;
                                    if (val == null)
                                        continue;
                                    md.SetValByKey(dc.ColumnName, val.ToString().Replace(oldMapID, fk_mapdata));
                                    //md.SetValByKey(dc.ColumnName, val);
                                }
                                if (string.IsNullOrEmpty(md.PTable.Trim()))
                                    md.PTable = md.No;

                                if (string.IsNullOrEmpty(mdOld.FK_FormTree) == false)
                                    md.FK_FormTree = mdOld.FK_FormTree;

                                if (string.IsNullOrEmpty(mdOld.FK_FrmSort) == false)
                                    md.FK_FrmSort = mdOld.FK_FrmSort;

                                if (string.IsNullOrEmpty(mdOld.PTable) == false)
                                    md.PTable = mdOld.PTable;

                                md.DirectInsert();
                            }
                            break;
                        case "Sys_FrmBtn":
                            foreach (DataRow dr in dt.Rows)
                            {
                                idx++;
                                FrmBtn en = new FrmBtn();
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    object val = dr[dc.ColumnName] as object;
                                    if (val == null)
                                        continue;
                                    en.SetValByKey(dc.ColumnName, val.ToString().Replace(oldMapID, fk_mapdata));
                                }
                                if (isSetReadonly == true)
                                    en.IsEnable = false;


                                en.MyPK = "Btn_" + idx + "_" + fk_mapdata;
                                en.Insert();
                            }
                            break;
                        case "Sys_FrmLine":
                            foreach (DataRow dr in dt.Rows)
                            {
                                idx++;
                                FrmLine en = new FrmLine();
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    object val = dr[dc.ColumnName] as object;
                                    if (val == null)
                                        continue;

                                    en.SetValByKey(dc.ColumnName, val.ToString().Replace(oldMapID, fk_mapdata));
                                }
                                en.MyPK = "LE_" + idx + "_" + fk_mapdata;
                                en.Insert();
                            }
                            break;
                        case "Sys_FrmLab":
                            foreach (DataRow dr in dt.Rows)
                            {
                                idx++;
                                FrmLab en = new FrmLab();
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    object val = dr[dc.ColumnName] as object;
                                    if (val == null)
                                        continue;
                                    en.SetValByKey(dc.ColumnName, val.ToString().Replace(oldMapID, fk_mapdata));
                                }
                                //  en.FK_MapData = fk_mapdata;  Remove this line from the table to solve lab The problem .
                                en.MyPK = "LB_" + idx + "_" + fk_mapdata;
                                en.Insert();
                            }
                            break;
                        case "Sys_FrmLink":
                            foreach (DataRow dr in dt.Rows)
                            {
                                idx++;
                                FrmLink en = new FrmLink();
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    object val = dr[dc.ColumnName] as object;
                                    if (val == null)
                                        continue;
                                    en.SetValByKey(dc.ColumnName, val.ToString().Replace(oldMapID, fk_mapdata));
                                }
                                en.MyPK = "LK_" + idx + "_" + fk_mapdata;
                                en.Insert();
                            }
                            break;
                        case "Sys_FrmEle":
                            foreach (DataRow dr in dt.Rows)
                            {
                                idx++;
                                FrmEle en = new FrmEle();
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    object val = dr[dc.ColumnName] as object;
                                    if (val == null)
                                        continue;
                                    en.SetValByKey(dc.ColumnName, val.ToString().Replace(oldMapID, fk_mapdata));
                                }
                                if (isSetReadonly == true)
                                    en.IsEnable = false;

                                en.Insert();
                            }
                            break;
                        case "Sys_FrmImg":
                            foreach (DataRow dr in dt.Rows)
                            {
                                idx++;
                                FrmImg en = new FrmImg();
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    object val = dr[dc.ColumnName] as object;
                                    if (val == null)
                                        continue;

                                    en.SetValByKey(dc.ColumnName, val.ToString().Replace(oldMapID, fk_mapdata));
                                }
                                en.MyPK = "Img_" + idx + "_" + fk_mapdata;
                                en.Insert();
                            }
                            break;
                        case "Sys_FrmImgAth":
                            foreach (DataRow dr in dt.Rows)
                            {
                                idx++;
                                FrmImgAth en = new FrmImgAth();
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    object val = dr[dc.ColumnName] as object;
                                    if (val == null)
                                        continue;
                                    en.SetValByKey(dc.ColumnName, val.ToString().Replace(oldMapID, fk_mapdata));
                                }

                                if (string.IsNullOrEmpty(en.CtrlID))
                                    en.CtrlID = "ath" + idx;

                                en.Insert();
                            }
                            break;
                        case "Sys_FrmRB":
                            foreach (DataRow dr in dt.Rows)
                            {
                                idx++;
                                FrmRB en = new FrmRB();
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    object val = dr[dc.ColumnName] as object;
                                    if (val == null)
                                        continue;
                                    en.SetValByKey(dc.ColumnName, val.ToString().Replace(oldMapID, fk_mapdata));
                                }


                                try
                                {
                                    en.Save();
                                }
                                catch
                                {
                                }
                            }
                            break;
                        case "Sys_FrmAttachment":
                            foreach (DataRow dr in dt.Rows)
                            {
                                idx++;
                                FrmAttachment en = new FrmAttachment();
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    object val = dr[dc.ColumnName] as object;
                                    if (val == null)
                                        continue;
                                    en.SetValByKey(dc.ColumnName, val.ToString().Replace(oldMapID, fk_mapdata));
                                }
                                en.MyPK = "Ath_" + idx + "_" + fk_mapdata;
                                if (isSetReadonly == true)
                                {
                                    en.IsDelete = false;
                                    en.IsUpload = false;
                                }

                                try
                                {
                                    en.Insert();
                                }
                                catch
                                {
                                }
                            }
                            break;
                        case "Sys_MapM2M":
                            foreach (DataRow dr in dt.Rows)
                            {
                                idx++;
                                MapM2M en = new MapM2M();
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    object val = dr[dc.ColumnName] as object;
                                    if (val == null)
                                        continue;
                                    en.SetValByKey(dc.ColumnName, val.ToString().Replace(oldMapID, fk_mapdata));
                                }
                                //   en.NoOfObj = "M2M_" + idx + "_" + fk_mapdata;
                                if (isSetReadonly == true)
                                {
                                    en.IsDelete = false;
                                    en.IsInsert = false;
                                }
                                en.Insert();
                            }
                            break;
                        case "Sys_MapFrame":
                            foreach (DataRow dr in dt.Rows)
                            {
                                idx++;
                                MapFrame en = new MapFrame();
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    object val = dr[dc.ColumnName] as object;
                                    if (val == null)
                                        continue;
                                    en.SetValByKey(dc.ColumnName, val.ToString().Replace(oldMapID, fk_mapdata));
                                }
                                en.NoOfObj = "Fra_" + idx + "_" + fk_mapdata;
                                en.Insert();
                            }
                            break;
                        case "Sys_MapExt":
                            foreach (DataRow dr in dt.Rows)
                            {
                                idx++;
                                MapExt en = new MapExt();
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    object val = dr[dc.ColumnName] as object;
                                    if (val == null)
                                        continue;
                                    en.SetValByKey(dc.ColumnName, val.ToString().Replace(oldMapID, fk_mapdata));
                                }
                                try
                                {
                                    en.Insert();
                                }
                                catch
                                {
                                    en.MyPK = "Ext_" + idx + "_" + fk_mapdata;
                                    en.Insert();
                                }
                            }
                            break;
                        case "Sys_MapAttr":
                            foreach (DataRow dr in dt.Rows)
                            {
                                MapAttr en = new MapAttr();
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    object val = dr[dc.ColumnName] as object;
                                    if (val == null)
                                        continue;
                                    en.SetValByKey(dc.ColumnName, val.ToString().Replace(oldMapID, fk_mapdata));
                                }

                                if (isSetReadonly == true)
                                {
                                    if (en.DefValReal != null
                                        && (en.DefValReal.Contains("@WebUser.")
                                        || en.DefValReal.Contains("@RDT")))
                                        en.DefValReal = "";

                                    switch (en.UIContralType)
                                    {
                                        case UIContralType.DDL:
                                            en.UIIsEnable = false;
                                            break;
                                        case UIContralType.TB:
                                            en.UIIsEnable = false;
                                            break;
                                        case UIContralType.RadioBtn:
                                            en.UIIsEnable = false;
                                            break;
                                        case UIContralType.CheckBok:
                                            en.UIIsEnable = false;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                en.MyPK = en.FK_MapData + "_" + en.KeyOfEn;
                                en.DirectInsert();
                            }
                            break;
                        case "Sys_GroupField":
                            foreach (DataRow dr in dt.Rows)
                            {
                                idx++;
                                GroupField en = new GroupField();
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    object val = dr[dc.ColumnName] as object;
                                    if (val == null)
                                        continue;
                                    en.SetValByKey(dc.ColumnName, val.ToString().Replace(oldMapID, fk_mapdata));
                                }
                                int beforeID = en.OID;
                                en.OID = 0;
                                en.Insert();
                                endDoSQL += "@UPDATE Sys_MapAttr SET GroupID=" + en.OID + " WHERE FK_MapData='" + fk_mapdata + "' AND GroupID=" + beforeID;
                            }
                            break;
                        case "Sys_Enum":
                            foreach (DataRow dr in dt.Rows)
                            {
                                Sys.SysEnum se = new Sys.SysEnum();
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    string val = dr[dc.ColumnName] as string;
                                    se.SetValByKey(dc.ColumnName, val);
                                }
                                se.MyPK = se.EnumKey + "_" + se.Lang + "_" + se.IntKey;
                                if (se.IsExits)
                                    continue;
                                se.Insert();
                            }
                            break;
                        case "Sys_EnumMain":
                            foreach (DataRow dr in dt.Rows)
                            {
                                Sys.SysEnumMain sem = new Sys.SysEnumMain();
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    string val = dr[dc.ColumnName] as string;
                                    if (val == null)
                                        continue;
                                    sem.SetValByKey(dc.ColumnName, val);
                                }
                                if (sem.IsExits)
                                    continue;
                                sem.Insert();
                            }
                            break;
                        case "WF_Node":
                            if (dt.Rows.Count > 0)
                            {
                                endDoSQL += "@UPDATE WF_Node SET FWCSta=2"
                                    + ",FWC_X=" + dt.Rows[0]["FWC_X"]
                                    + ",FWC_Y=" + dt.Rows[0]["FWC_Y"]
                                    + ",FWC_H=" + dt.Rows[0]["FWC_H"]
                                    + ",FWC_W=" + dt.Rows[0]["FWC_W"]
                                    + ",FWCType=" + dt.Rows[0]["FWCType"]
                                    + " WHERE NodeID=" + fk_mapdata.Replace("ND", "");
                            }
                            break;
                        default:
                            break;
                    }
                }
                // Implementation of the final end sql.
                DBAccess.RunSQLs(endDoSQL);

                MapData mdNew = new MapData(fk_mapdata);
                mdNew.RepairMap();
                //  mdNew.FK_FrmSort = fk_sort;
                mdNew.Update();
                return mdNew;
            }
            catch (Exception ex)
            {
            }

            return null;
        }
        public void RepairMap()
        {
            GroupFields gfs = new GroupFields(this.No);
            if (gfs.Count == 0)
            {
                GroupField gf = new GroupField();
                gf.EnName = this.No;
                gf.Lab = this.Name;
                gf.Insert();
                string sqls = "";
                sqls += "@UPDATE Sys_MapDtl SET GroupID=" + gf.OID + " WHERE FK_MapData='" + this.No + "'";
                sqls += "@UPDATE Sys_MapAttr SET GroupID=" + gf.OID + " WHERE FK_MapData='" + this.No + "'";
                sqls += "@UPDATE Sys_MapFrame SET GroupID=" + gf.OID + " WHERE FK_MapData='" + this.No + "'";
                sqls += "@UPDATE Sys_MapM2M SET GroupID=" + gf.OID + " WHERE FK_MapData='" + this.No + "'";
                sqls += "@UPDATE Sys_FrmAttachment SET GroupID=" + gf.OID + " WHERE FK_MapData='" + this.No + "'";
                DBAccess.RunSQLs(sqls);
            }
            else
            {
                GroupField gfFirst = gfs[0] as GroupField;
                string sqls = "";
                sqls += "@UPDATE Sys_MapDtl SET GroupID=" + gfFirst.OID + "        WHERE  No   IN (SELECT X.No FROM (SELECT No FROM Sys_MapDtl WHERE GroupID NOT IN (SELECT OID FROM Sys_GroupField WHERE EnName='" + this.No + "')) AS X ) AND FK_MapData='" + this.No + "'";
                sqls += "@UPDATE Sys_MapAttr SET GroupID=" + gfFirst.OID + "       WHERE  MyPK IN (SELECT X.MyPK FROM (SELECT MyPK FROM Sys_MapAttr       WHERE GroupID NOT IN (SELECT OID FROM Sys_GroupField WHERE EnName='" + this.No + "')) AS X) AND FK_MapData='" + this.No + "' ";
                sqls += "@UPDATE Sys_MapFrame SET GroupID=" + gfFirst.OID + "      WHERE  MyPK IN (SELECT X.MyPK FROM (SELECT MyPK FROM Sys_MapFrame      WHERE GroupID NOT IN (SELECT OID FROM Sys_GroupField WHERE EnName='" + this.No + "')) AS X) AND FK_MapData='" + this.No + "' ";
                sqls += "@UPDATE Sys_MapM2M SET GroupID=" + gfFirst.OID + "        WHERE  MyPK IN (SELECT X.MyPK FROM (SELECT MyPK FROM Sys_MapM2M        WHERE GroupID NOT IN (SELECT OID FROM Sys_GroupField WHERE EnName='" + this.No + "')) AS X) AND FK_MapData='" + this.No + "' ";
                sqls += "@UPDATE Sys_FrmAttachment SET GroupID=" + gfFirst.OID + " WHERE  MyPK IN (SELECT X.MyPK FROM (SELECT MyPK FROM Sys_FrmAttachment WHERE GroupID NOT IN (SELECT OID FROM Sys_GroupField WHERE EnName='" + this.No + "')) AS X) AND FK_MapData='" + this.No + "' ";

#warning  These ones sql  For Oracle  There is a problem , But does not affect the use of .
                try
                {
                    DBAccess.RunSQLs(sqls);
                }
                catch
                {

                }
            }

            BP.Sys.MapAttr attr = new BP.Sys.MapAttr();
            if (this.EnPK == "OID")
            {
                if (attr.IsExit(MapAttrAttr.KeyOfEn, "OID", MapAttrAttr.FK_MapData, this.No) == false)
                {
                    attr.FK_MapData = this.No;
                    attr.KeyOfEn = "OID";
                    attr.Name = "OID";
                    attr.MyDataType = BP.DA.DataType.AppInt;
                    attr.UIContralType = UIContralType.TB;
                    attr.LGType = FieldTypeS.Normal;
                    attr.UIVisible = false;
                    attr.UIIsEnable = false;
                    attr.DefVal = "0";
                    attr.HisEditType = BP.En.EditType.Readonly;
                    attr.Insert();
                }
            }
            if (this.EnPK == "No" || this.EnPK == "MyPK")
            {
                if (attr.IsExit(MapAttrAttr.KeyOfEn, this.EnPK, MapAttrAttr.FK_MapData, this.No) == false)
                {
                    attr.FK_MapData = this.No;
                    attr.KeyOfEn = this.EnPK;
                    attr.Name = this.EnPK;
                    attr.MyDataType = BP.DA.DataType.AppInt;
                    attr.UIContralType = UIContralType.TB;
                    attr.LGType = FieldTypeS.Normal;
                    attr.UIVisible = false;
                    attr.UIIsEnable = false;
                    attr.DefVal = "0";
                    attr.HisEditType = BP.En.EditType.Readonly;
                    attr.Insert();
                }
            }

            if (attr.IsExit(MapAttrAttr.KeyOfEn, "RDT", MapAttrAttr.FK_MapData, this.No) == false)
            {
                attr = new BP.Sys.MapAttr();
                attr.FK_MapData = this.No;
                attr.HisEditType = BP.En.EditType.UnDel;
                attr.KeyOfEn = "RDT";
                attr.Name = " Updated ";

                attr.MyDataType = BP.DA.DataType.AppDateTime;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = false;
                attr.UIIsEnable = false;
                attr.DefVal = "@RDT";
                attr.Tag = "1";
                attr.Insert();
            }
        }
        protected override bool beforeInsert()
        {
            this.PTable = PubClass.DealToFieldOrTableNames(this.PTable);
            return base.beforeInsert();
        }
        /// <summary>
        ///  Set up Para  Parameters .
        /// </summary>
        public void ResetMaxMinXY()
        {
            #region  Calculate the leftmost , And the rightmost value .
            //  Seeking leftmost .
            float i1 = DBAccess.RunSQLReturnValFloat("SELECT MIN(X1) FROM Sys_FrmLine WHERE FK_MapData='" + this.No + "'", 0);
            if (i1 == 0) /* No Line , Only under circumstances Pictures .*/
                i1 = DBAccess.RunSQLReturnValFloat("SELECT MIN(X) FROM Sys_FrmImg WHERE FK_MapData='" + this.No + "'", 0);

            float i2 = DBAccess.RunSQLReturnValFloat("SELECT MIN(X)  FROM Sys_FrmLab  WHERE FK_MapData='" + this.No + "'", 0);
            if (i1 > i2)
                this.MaxLeft = i2;
            else
                this.MaxLeft = i1;

            //  Seeking rightmost .
            i1 = DBAccess.RunSQLReturnValFloat("SELECT Max(X2) FROM Sys_FrmLine WHERE FK_MapData='" + this.No + "'", 0);
            if (i1 == 0)
            {
                /* No line of cases , Calculated in accordance with pictures .*/
                i1 = DBAccess.RunSQLReturnValFloat("SELECT Max(X+W) FROM Sys_FrmImg WHERE FK_MapData='" + this.No + "'", 0);
            }
            this.MaxRight = i1;

            //  For the most top.
            i1 = DBAccess.RunSQLReturnValFloat("SELECT MIN(Y1) FROM Sys_FrmLine WHERE FK_MapData='" + this.No + "'", 0);
            i2 = DBAccess.RunSQLReturnValFloat("SELECT MIN(Y)  FROM Sys_FrmLab  WHERE FK_MapData='" + this.No + "'", 0);

            if (i1 > i2)
                this.MaxTop = i2;
            else
                this.MaxTop = i1;

            //  For the most end.
            i1 = DBAccess.RunSQLReturnValFloat("SELECT Max(Y1) FROM Sys_FrmLine WHERE FK_MapData='" + this.No + "'", 0);
            /* Adding small Zhou Peng 2014/10/23-----------------------START*/
            if (i1 == 0) /* No Line , Only under circumstances Pictures .*/
                i1 = DBAccess.RunSQLReturnValFloat("SELECT Max(Y+H) FROM Sys_FrmImg WHERE FK_MapData='" + this.No + "'", 0);

            /* Adding small Zhou Peng 2014/10/23-----------------------END*/
            i2 = DBAccess.RunSQLReturnValFloat("SELECT Max(Y)  FROM Sys_FrmLab  WHERE FK_MapData='" + this.No + "'", 0);
            if (i2 == 0)
                i2 = DBAccess.RunSQLReturnValFloat("SELECT Max(Y+H) FROM Sys_FrmImg WHERE FK_MapData='" + this.No + "'", 0);
            // Obtained bottommost   Accessory 
            float endFrmAtt = DBAccess.RunSQLReturnValFloat("SELECT Max(Y+H)  FROM Sys_FrmAttachment  WHERE FK_MapData='" + this.No + "'", 0);
            // The bottom of the list obtained 
            float endFrmDtl = DBAccess.RunSQLReturnValFloat("SELECT Max(Y+H)  FROM Sys_MapDtl  WHERE FK_MapData='" + this.No + "'", 0);

            // Find the bottom of the expansion control 
            float endFrmEle = DBAccess.RunSQLReturnValFloat("SELECT Max(Y+H)  FROM Sys_FrmEle  WHERE FK_MapData='" + this.No + "'", 0);
            // Obtained bottommost textbox
            float endFrmAttr = DBAccess.RunSQLReturnValFloat("SELECT Max(Y+UIHeight)  FROM  Sys_MapAttr  WHERE FK_MapData='" + this.No + "' and UIVisible='1'", 0);

            if (i1 > i2)
                this.MaxEnd = i1;
            else
                this.MaxEnd = i2;

            this.MaxEnd = this.MaxEnd > endFrmAtt ? this.MaxEnd : endFrmAtt;
            this.MaxEnd = this.MaxEnd > endFrmDtl ? this.MaxEnd : endFrmDtl;
            this.MaxEnd = this.MaxEnd > endFrmEle ? this.MaxEnd : endFrmEle;
            this.MaxEnd = this.MaxEnd > endFrmAtt ? this.MaxEnd : endFrmAttr;

            #endregion

            this.DirectUpdate();
        }

        /// <summary>
        ///  Demand shift .
        /// </summary>
        /// <param name="md"></param>
        /// <param name="scrWidth"></param>
        /// <returns></returns>
        public static float GenerSpanWeiYi(MapData md, float scrWidth)
        {
            if (scrWidth == 0)
                scrWidth = 900;

            float left = md.MaxLeft;
            if (left == 0)
            {
                md.ResetMaxMinXY();
                md.RetrieveFromDBSources();
                md.Retrieve();

                left = md.MaxLeft;
            }

            float right = md.MaxRight;
            float withFrm = right - left;
            if (withFrm >= scrWidth)
            {
                /*  If the form is wider than the actual width of the screen  */
                return -left;
            }
            float space = (scrWidth - withFrm) / 2; // Blank place .

            return -(left - space);
        }
        /// <summary>
        ///  Seek the screen width 
        /// </summary>
        /// <param name="md"></param>
        /// <param name="scrWidth"></param>
        /// <returns></returns>
        public static float GenerSpanWidth(MapData md, float scrWidth)
        {
            if (scrWidth == 0)
                scrWidth = 900;
            float left = md.MaxLeft;
            if (left == 0)
            {
                md.ResetMaxMinXY();
                left = md.MaxLeft;
            }

            float right = md.MaxRight;
            float withFrm = right - left;
            if (withFrm >= scrWidth)
            {
                return withFrm;
            }
            return scrWidth;
        }
        /// <summary>
        ///  Seeking screen height 
        /// </summary>
        /// <param name="md"></param>
        /// <param name="scrWidth"></param>
        /// <returns></returns>
        public static float GenerSpanHeight(MapData md, float scrHeight)
        {
            if (scrHeight == 0)
                scrHeight = 1200;

            float end = md.MaxEnd;
            if (end > scrHeight)
                return end + 10;
            else
                return scrHeight;
        }
        protected override bool beforeUpdateInsertAction()
        {
            this.PTable = PubClass.DealToFieldOrTableNames(this.PTable);
            MapAttrs.Retrieve(MapAttrAttr.FK_MapData, PTable);

            // Updated version number .
            this.Ver = DataType.CurrentDataTimess;

            #region   Check ca Authentication Settings .
            bool isHaveCA = false;
            foreach (MapAttr item in this.MapAttrs)
            {
                if (item.SignType == SignType.CA)
                {
                    isHaveCA = true;
                    break;
                }
            }
            this.IsHaveCA = isHaveCA;
            if (IsHaveCA == true)
            {
                // Increases hidden field .
                //MapAttr attr = new BP.Sys.MapAttr();
                // attr.MyPK = this.No + "_SealData";
                // attr.FK_MapData = this.No;
                // attr.HisEditType = BP.En.EditType.UnDel;
                //attr.KeyOfEn = "SealData";
                // attr.Name = "SealData";
                // attr.MyDataType = BP.DA.DataType.AppString;
                // attr.UIContralType = UIContralType.TB;
                //  attr.LGType = FieldTypeS.Normal;
                // attr.UIVisible = false;
                // attr.UIIsEnable = false;
                // attr.MaxLen = 4000;
                // attr.MinLen = 0;
                // attr.Save();
            }
            #endregion   Check ca Authentication Settings .

            return base.beforeUpdateInsertAction();
        }
        /// <summary>
        ///  Updated version 
        /// </summary>
        public void UpdateVer()
        {
            string sql = "UPDATE Sys_MapData SET VER='" + BP.DA.DataType.CurrentDataTimess + "' WHERE No='" + this.No + "'";
            BP.DA.DBAccess.RunSQL(sql);
        }
        protected override bool beforeDelete()
        {
            string sql = "";
            sql = "SELECT * FROM Sys_MapDtl WHERE FK_MapData ='" + this.No + "'";
            DataTable Sys_MapDtl = DBAccess.RunSQLReturnTable(sql);
            string ids = "'" + this.No + "'";
            foreach (DataRow dr in Sys_MapDtl.Rows)
                ids += ",'" + dr["No"] + "'";

            string where = " FK_MapData IN (" + ids + ")";

            #region  Delete related data .
            sql += "@DELETE FROM Sys_MapDtl WHERE FK_MapData='" + this.No + "'";
            sql += "@DELETE FROM Sys_FrmLine WHERE " + where;
            sql += "@DELETE FROM Sys_FrmEle WHERE " + where;
            sql += "@DELETE FROM Sys_FrmEvent WHERE " + where;
            sql += "@DELETE FROM Sys_FrmBtn WHERE " + where;
            sql += "@DELETE FROM Sys_FrmLab WHERE " + where;
            sql += "@DELETE FROM Sys_FrmLink WHERE " + where;
            sql += "@DELETE FROM Sys_FrmImg WHERE " + where;
            sql += "@DELETE FROM Sys_FrmImgAth WHERE " + where;
            sql += "@DELETE FROM Sys_FrmRB WHERE " + where;
            sql += "@DELETE FROM Sys_FrmAttachment WHERE " + where;
            sql += "@DELETE FROM Sys_MapM2M WHERE " + where;
            sql += "@DELETE FROM Sys_MapFrame WHERE " + where;
            sql += "@DELETE FROM Sys_MapExt WHERE " + where;
            sql += "@DELETE FROM Sys_MapAttr WHERE " + where;
            sql += "@DELETE FROM Sys_GroupField WHERE EnName IN (" + ids + ")";
            sql += "@DELETE FROM Sys_MapData WHERE No IN (" + ids + ")";
            sql += "@DELETE FROM Sys_MapM2M WHERE " + where;
            sql += "@DELETE FROM Sys_M2M WHERE " + where;
            DBAccess.RunSQLs(sql);
            #endregion  Delete related data .

            #region  Delete the physical table .
            try
            {
                BP.DA.DBAccess.RunSQL("DROP TABLE " + this.PTable);
            }
            catch
            {
            }

            MapDtls dtls = new MapDtls(this.No);
            foreach (MapDtl dtl in dtls)
            {
                try
                {
                    DBAccess.RunSQL("DROP TABLE " + dtl.PTable);
                }
                catch
                {
                }
                dtl.Delete();
            }
            #endregion

            return base.beforeDelete();
        }

        public System.Data.DataSet GenerHisDataSet()
        {
            return GenerHisDataSet(this.No);
        }
        public static System.Data.DataSet GenerHisDataSet(string FK_MapData)
        {

            // Sys_MapDtl.
            string sql = "SELECT * FROM Sys_MapDtl WHERE FK_MapData ='{0}'";
            sql = string.Format(sql, FK_MapData);
            DataTable dtMapDtl = DBAccess.RunSQLReturnTable(sql);
            dtMapDtl.TableName = "Sys_MapDtl";

            string ids = string.Format("'{0}'", FK_MapData);
            foreach (DataRow dr in dtMapDtl.Rows)
            {
                ids += ",'" + dr["No"] + "'";
            }

            List<string> listNames = new List<string>();
            // Sys_GroupField.
            listNames.Add("Sys_GroupField");
            sql = "SELECT * FROM Sys_GroupField WHERE  EnName IN (" + ids + ")";
            string sqls = sql;

            // Sys_Enum
            listNames.Add("Sys_Enum");
            sql = "SELECT * FROM Sys_Enum WHERE EnumKey IN ( SELECT UIBindKey FROM Sys_MapAttr WHERE FK_MapData IN (" + ids + ") )";
            sqls += ";" + sql;

            //  Audit Components 
            string nodeIDstr = FK_MapData.Replace("ND", "");
            if (DataType.IsNumStr(nodeIDstr))
            {
                //  Audit Component Status :0  Disable ;1  Enable ;2  Read-only ;
                listNames.Add("WF_Node");
                sql = "SELECT * FROM WF_Node WHERE NodeID=" + nodeIDstr + " AND  FWCSta in(1,2)";
                sqls += ";" + sql;
            }

            string where = " FK_MapData IN (" + ids + ")";

            // Sys_MapData.
            listNames.Add("Sys_MapData");
            sql = "SELECT * FROM Sys_MapData WHERE No='" + FK_MapData + "'";
            sqls += ";" + sql;


            // Sys_MapAttr.
            listNames.Add("Sys_MapAttr");
            sql = "SELECT * FROM Sys_MapAttr WHERE " + where + " AND KeyOfEn NOT IN('WFState') ORDER BY FK_MapData,IDX ";
            sqls += ";" + sql;

            // Sys_MapM2M.
            listNames.Add("Sys_MapM2M");
            sql = "SELECT * FROM Sys_MapM2M WHERE " + where;
            sqls += ";" + sql;

            // Sys_MapExt.
            listNames.Add("Sys_MapExt");
            sql = "SELECT * FROM Sys_MapExt WHERE " + where;
            sqls += ";" + sql;

            // line.
            listNames.Add("Sys_FrmLine");
            sql = "SELECT * FROM Sys_FrmLine WHERE " + where;
            sqls += ";" + sql;

            // ele.
            listNames.Add("Sys_FrmEle");
            sql = "SELECT * FROM Sys_FrmEle WHERE " + where;
            sqls += ";" + sql;

            // link.
            listNames.Add("Sys_FrmLink");
            sql = "SELECT * FROM Sys_FrmLink WHERE " + where;
            sqls += ";" + sql;

            // btn.
            listNames.Add("Sys_FrmBtn");
            sql = "SELECT * FROM Sys_FrmBtn WHERE " + where;
            sqls += ";" + sql;

            // Sys_FrmImg.
            listNames.Add("Sys_FrmImg");
            sql = "SELECT * FROM Sys_FrmImg WHERE " + where;
            sqls += ";" + sql;

            // Sys_FrmLab.
            listNames.Add("Sys_FrmLab");
            sql = "SELECT * FROM Sys_FrmLab WHERE " + where;
            sqls += ";" + sql;

            // Sys_FrmRB.
            listNames.Add("Sys_FrmRB");
            sql = "SELECT * FROM Sys_FrmRB WHERE " + where;
            sqls += ";" + sql;


            // Sys_FrmAttachment. 
            listNames.Add("Sys_FrmAttachment");
            sql = "SELECT * FROM Sys_FrmAttachment WHERE " + where + " AND FK_Node=0";
            sqls += ";" + sql;

            // Sys_FrmImgAth.
            listNames.Add("Sys_FrmImgAth");
            sql = "SELECT * FROM Sys_FrmImgAth WHERE " + where;
            sqls += ";" + sql;

            DataSet ds = DA.DBAccess.RunSQLReturnDataSet(sqls);
            if (ds != null && ds.Tables.Count == listNames.Count)
                for (int i = 0; i < listNames.Count; i++)
                {
                    ds.Tables[i].TableName = listNames[i];
                }

            //string[] strs = sqls.Split(';');
            //DataSet ds = new DataSet();
            //for (int i = 0; i < strs.Length; i++)
            //{
            //    sql = strs[i];
            //    if (string.IsNullOrEmpty(sql))
            //        continue;

            //    DataTable dt = RunSQLReturnTable(sql);
            //    string tableName = "DT" + i;
            //    try
            //    {
            //        tableName = listNames[i];
            //        //int indexStart = sql.IndexOf("From ", StringComparison.OrdinalIgnoreCase) + 5;
            //        //int indexEnd = sql.IndexOf(" WHERE", StringComparison.OrdinalIgnoreCase) - indexStart;
            //        //tableName = sql.Substring(indexStart, indexEnd);
            //    }
            //    catch (Exception) { }
            //    dt.TableName = tableName;
            //    ds.Tables.Add(dt);
            //}

            foreach (DataTable item in ds.Tables)
            {
                if (item.TableName == "Sys_MapAttr" && item.Rows.Count == 0)
                {
                    BP.Sys.MapAttr attr = new BP.Sys.MapAttr();
                    attr.FK_MapData = FK_MapData;
                    attr.KeyOfEn = "OID";
                    attr.Name = "OID";
                    attr.MyDataType = BP.DA.DataType.AppInt;
                    attr.UIContralType = UIContralType.TB;
                    attr.LGType = FieldTypeS.Normal;
                    attr.UIVisible = false;
                    attr.UIIsEnable = false;
                    attr.DefVal = "0";
                    attr.HisEditType = BP.En.EditType.Readonly;
                    attr.Insert();
                }
            }

            ds.Tables.Add(dtMapDtl);
            return ds;
        }


        /// <summary>
        ///  Automatic generation £ê£ó Program .
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="attrs"></param>
        /// <param name="attr"></param>
        /// <param name="tbPer"></param>
        /// <returns></returns>
        public static string GenerAutoFull(string pk, MapAttrs attrs, MapExt me, string tbPer)
        {
            string left = "\n document.forms[0]." + tbPer + "_TB" + me.AttrOfOper + "_" + pk + ".value = ";
            string right = me.Doc;
            foreach (MapAttr mattr in attrs)
            {
                right = right.Replace("@" + mattr.KeyOfEn, " parseFloat( document.forms[0]." + tbPer + "_TB_" + mattr.KeyOfEn + "_" + pk + ".value) ");
            }
            return " alert( document.forms[0]." + tbPer + "_TB" + me.AttrOfOper + "_" + pk + ".value ) ; \t\n " + left + right;
        }
    }
    /// <summary>
    ///  Mapping foundation s
    /// </summary>
    public class MapDatas : EntitiesMyPK
    {
        #region  Structure 
        /// <summary>
        ///  Mapping foundation s
        /// </summary>
        public MapDatas()
        {
        }
        /// <summary>
        ///  Get it  Entity
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new MapData();
            }
        }
        #endregion
    }
}
