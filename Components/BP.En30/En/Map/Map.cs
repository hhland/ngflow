using System;
using System.Collections;
using System.Data;
using BP.DA;
using BP.Sys;
using BP.Web.Controls;

namespace BP.En
{
    /// <summary>
    ///  Editor Type 
    /// </summary>
    public enum EditerType
    {
        /// <summary>
        ///  No editor 
        /// </summary>
        None,
        /// <summary>
        /// Sina Editor 
        /// </summary>
        Sina,
        /// <summary>
        /// FKEditer
        /// </summary>
        FKEditer,
        /// <summary>
        /// KindEditor
        /// </summary>
        KindEditor,
        /// <summary>
        ///  Baidu UEditor
        /// </summary>
        UEditor
    }
    /// <summary>
    ///  Accessory Type 
    /// </summary>
    public enum AdjunctType
    {
        /// <summary>
        ///  Does not require attachment .
        /// </summary>
        None,
        /// <summary>
        ///  Picture 
        /// </summary>
        PhotoOnly,
        /// <summary>
        /// word  File .
        /// </summary>
        WordOnly,
        /// <summary>
        ///  All Types 
        /// </summary>
        ExcelOnly,
        /// <summary>
        ///  All Types .
        /// </summary>
        AllType
    }
    /// <summary>
    ///  Entity Type 
    /// </summary>
    public enum EnType
    {
        /// <summary>
        ///  System entities 
        /// </summary>
        Sys,
        /// <summary>
        ///  Administrators maintain the entity 
        /// </summary>
        Admin,
        /// <summary>
        ///  Application entity 
        /// </summary>
        App,
        /// <summary>
        ///  Third party entities （ You can update ）
        /// </summary>
        ThirdPartApp,
        /// <summary>
        ///  View ( Invalid update )
        /// </summary>
        View,
        /// <summary>
        ///  You can incorporate rights management 
        /// </summary>
        PowerAble,
        /// <summary>
        ///  Other 
        /// </summary>
        Etc,
        /// <summary>
        ///  Breakdown or a point .
        /// </summary>
        Dtl,
        /// <summary>
        ///  Peer to peer 
        /// </summary>
        Dot2Dot,
        /// <summary>
        /// XML　 Type 
        /// </summary>
        XML,
        /// <summary>
        ///  Extension type , Need it for the query .
        /// </summary>
        Ext
    }
    /// <summary>
    ///  Moved to the display mode 
    /// </summary>
    public enum MoveToShowWay
    {
        /// <summary>
        ///  Do not show 
        /// </summary>
        None,
        /// <summary>
        ///  Drop-down list 
        /// </summary>
        DDL,
        /// <summary>
        ///  Tile 
        /// </summary>
        Panel
    }
    /// <summary>
    /// EnMap  The summary .
    /// </summary>
    public class Map
    {
        #region  Help .
        /// <summary>
        ///  Help increase 
        /// </summary>
        /// <param name="key"> Field </param>
        /// <param name="url"></param>
        public void SetHelperUrl(string key, string url)
        {
            Attr attr = this.GetAttrByKey(key);
            attr.HelperUrl = url;
        }
        /// <summary>
        ///  Help increase 
        /// </summary>
        /// <param name="key"> Field </param>
        public void SetHelperBaidu(string key)
        {
            Attr attr = this.GetAttrByKey(key);
            attr.HelperUrl = "http://www.baidu.com/s?word=ccflow " + attr.Desc;
        }
        /// <summary>
        ///  Help increase 
        /// </summary>
        /// <param name="key"> Field </param>
        /// <param name="keyword"> Keyword </param>
        public void SetHelperBaidu(string key, string keyword)
        {
            Attr attr = this.GetAttrByKey(key);
            attr.HelperUrl = "http://www.baidu.com/s?word=" + keyword;
        }
        /// <summary>
        ///  Help increase 
        /// </summary>
        /// <param name="key"> Field </param>
        /// <param name="context"> Connection </param>
        public void SetHelperAlert(string key, string context)
        {
            Attr attr = this.GetAttrByKey(key);
            attr.HelperUrl = "javascript:alert('"+context+"')";
        }
        #endregion  Help .


        #region 与xml  File operations are related 
        /// <summary>
        /// xml  Location of the file 
        /// </summary>
        public string XmlFile = null;
        #endregion 与xml  File operations are related 

        private Boolean _IsAllowRepeatNo;
        public Boolean IsAllowRepeatNo 
        {
            get { return _IsAllowRepeatNo; }
            set { _IsAllowRepeatNo = value; }
        }

        #region chuli
        /// <summary>
        ///  Query ( To avoid excessive waste of resources , Disposable generate multiple use )
        /// </summary>
        public string SelectSQL = null;
        /// <summary>
        ///  Whether it is a simple set of attributes 
        ///  Here is the problem of handling the foreign key , Too many in the process of batch operation of the system will affect the efficiency of the foreign key .
        /// </summary>
        public bool IsSimpleAttrs = false;
        /// <summary>
        ///  Simple set 
        /// </summary>
        public Attrs SetToSimple()
        {
            Attrs attrs = new Attrs();
            foreach (Attr attr in this._attrs)
            {
                if (attr.MyFieldType == FieldType.PK ||
                    attr.MyFieldType == FieldType.PKEnum
                    ||
                    attr.MyFieldType == FieldType.PKFK)
                {
                    attrs.Add(new Attr(attr.Key, attr.Field, attr.DefaultVal, attr.MyDataType, true, attr.Desc));
                }
                else
                {
                    attrs.Add(new Attr(attr.Key, attr.Field, attr.DefaultVal, attr.MyDataType, false, attr.Desc));
                }
            }
            return attrs;
        }
        #endregion

        #region  About caching problem 
        public string  _FK_MapData = null;
        public string FK_MapData
        {
            get
            {
                if (_FK_MapData == null)
                    return this.PhysicsTable ;
                return _FK_MapData;
            }
            set
            {
                _FK_MapData = value;
            }
        }
        /// <summary>
        ///  Display mode 
        /// </summary>
        private FormShowType _FormShowType = FormShowType.NotSet;
        /// <summary>
        ///  Storage location OfEntity
        /// </summary>
        public FormShowType FormShowType
        {
            get
            {
                return _FormShowType;
            }
            set
            {
                _FormShowType = value;
            }
        }
        /// <summary>
        ///  Storage location 
        /// </summary>
        private Depositary _DepositaryOfEntity = Depositary.None;
        /// <summary>
        ///  Storage location OfEntity
        /// </summary>
        public Depositary DepositaryOfEntity
        {
            get
            {
                return _DepositaryOfEntity;
            }
            set
            {
                _DepositaryOfEntity = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>		
        private Depositary _DepositaryOfMap = Depositary.Application;
        /// <summary>
        ///  Storage location 
        /// </summary>
        public Depositary DepositaryOfMap
        {
            get
            {
                return _DepositaryOfMap;
            }
            set
            {
                _DepositaryOfMap = value;
            }
        }
        #endregion

        #region  Query property deal 

        #region  Non-enumerated values and foreign key criteria query 
        private AttrsOfSearch _attrsOfSearch = null;
        /// <summary>
        ///  Find a property 
        /// </summary>
        public AttrsOfSearch AttrsOfSearch
        {
            get
            {
                if (this._attrsOfSearch == null)
                    this._attrsOfSearch = new AttrsOfSearch();
                return this._attrsOfSearch;
            }
        }
        /// <summary>
        ///  Get all the Attrs
        /// </summary>
        /// <returns></returns>
        public Attrs GetChoseAttrs(Entity en)
        {
            return BP.Sys.CField.GetMyAttrs(en.GetNewEntities,en.EnMap);
        }
        public Attrs GetChoseAttrs(Entities ens)
        {
            return BP.Sys.CField.GetMyAttrs(ens, this);
        }
        #endregion

        #region  About enumeration value and foreign key search criteria 
        /// <summary>
        ///  Find attrs 
        /// </summary>
        private AttrSearchs _SearchAttrs = null;
        /// <summary>
        ///  Find attrs
        /// </summary>
        public AttrSearchs SearchAttrs
        {
            get
            {
                if (this._SearchAttrs == null)
                    this._SearchAttrs = new AttrSearchs();
                return this._SearchAttrs;
            }
        }
        public void AddHidden(string refKey, string symbol, string val)
        {
            AttrOfSearch aos = new AttrOfSearch("K" + this.AttrsOfSearch.Count, refKey, refKey, symbol, val, 0, true);
            this.AttrsOfSearch.Add(aos);
        }
        /// <summary>
        ///  Join Find Properties . Must be external keyboard / Enumerated type /boolen.
        /// </summary>
        /// <param name="key">key</param>
        public void AddSearchAttr(string key)
        {
            Attr attr = this.GetAttrByKey(key);
            if (attr.Key == "FK_Dept")
                this.SearchAttrs.Add(attr, false, null);
            else
                this.SearchAttrs.Add(attr, true, null);
        }
        /// <summary>
        ///  Join Find Properties . Must be external keyboard / Enumerated type /boolen.
        /// </summary>
        /// <param name="key"> Key </param>
        /// <param name="isShowSelectedAll"> Whether to show all </param>
        /// <param name="relationalDtlKey"> Cascading submenus field </param>
        public void AddSearchAttr(string key, bool isShowSelectedAll, string relationalDtlKey)
        {
            Attr attr = this.GetAttrByKey(key);
            this.SearchAttrs.Add(attr, isShowSelectedAll, relationalDtlKey);
        }
        /// <summary>
        ///  Join Find Properties .
        /// </summary>
        /// <param name="attr"> Property </param>
        public void AddSearchAttr_del(Attr attr)
        {
            //if (attr.MyFieldType == FieldType.Enum || attr.MyFieldType == FieldType.PKEnum
            //    || attr.MyFieldType == FieldType.FK || attr.MyFieldType == FieldType.PKFK
            //    || attr.MyDataType == DataType.AppBoolean
            //    || attr.MyDataType == DataType.AppDate
            //    || attr.MyDataType == DataType.AppDateTime)
            //{
            //    this.SearchAttrs.Add(attr, true, this.IsAddRefName);
            //}
            //else
            //{
            //    throw new Exception("@ Property [" + attr.Key + "," + attr.Desc + "] Find the collection can not be added to . Because he is not an enumeration type with foreign key .");
            //}
        }
        #endregion

        #endregion

        #region  Public Methods 
        /// <summary>
        ///  Made field 
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>field name </returns>
        public string GetFieldByKey(string key)
        {
            return GetAttrByKey(key).Field;
        }
        /// <summary>
        ///  Obtain description 
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>val</returns>
        public string GetDescByKey(String key)
        {
            return GetAttrByKey(key).Desc;
        }
        /// <summary>
        ///  Through a key  Get its property values .
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>attr</returns>
        public Attr GetAttrByKey(string key)
        {
            foreach (Attr attr in this.Attrs)
            {
                if (attr.Key.ToUpper() == key.ToUpper())
                {
                    return attr;
                }
            }

            if (key == null)
                throw new Exception("@[" + this.EnDesc + "]  Get property key  Value can not be null .");


            if (this.ToString().Contains("."))
                throw new Exception("@[" + this.EnDesc + "," + this.PhysicsTable + "]  Not found  key=[" + key + "] Properties , Please check Map File . One reason for this problem is wrong , Associated attributes in an entity set up systems of this entity , You did not follow the rules at the time of writing information to an entity set reftext, refvalue. Please verify .");
            else
            {
                throw new Exception("@[" + this.EnDesc + "," + this.PhysicsTable + "]  Not found  key=[" + key + "] Properties , Please check Sys_MapAttr Whether the data table ,用SQL Carried out : SELECT * FROM Sys_MapAttr WHERE FK_MapData='" + this.ToString() + "' AND KeyOfEn='" + key + "'  Can query the data , If there is no possibility of the field attribute is missing .");
            }
        }
        /// <summary>
        ///  Acquire property .
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Attr GetAttrByBindKey(string key)
        {
            foreach (Attr attr in this.Attrs)
            {
                if (attr.UIBindKey == key)
                {
                    return attr;
                }
            }
            if (key == null)
                throw new Exception("@[" + this.EnDesc + "]  Get property key  Value can not be null .");

            if (this.ToString().Contains("."))
                throw new Exception("@[" + this.EnDesc + "," + this.ToString() + "]  Not found  key=[" + key + "] Properties , Please check Map File . One reason for this problem is wrong , Associated attributes in an entity set up systems of this entity , You did not follow the rules at the time of writing information to an entity set reftext, refvalue. Please verify .");
            else
                throw new Exception("@[" + this.EnDesc + "," + this.ToString() + "]  Not found  key=[" + key + "] Properties , Please check Sys_MapAttr Whether the data table ,用SQL Carried out : SELECT * FROM Sys_MapAttr WHERE FK_MapData='"+this.ToString()+"' AND KeyOfEn='"+key+"'  Can query the data , If there is no possibility of the field attribute is missing .");
        }
        /// <summary>
        ///  Through a key  Get its property values .
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>attr</returns>
        public Attr GetAttrByDesc(string desc)
        {
            foreach (Attr attr in this.Attrs)
            {
                if (attr.Desc == desc)
                {
                    return attr;
                }
            }
            if (desc == null)
                throw new Exception("@[" + this.EnDesc + "]  Get property  desc   Value can not be null .");

            throw new Exception("@[" + this.EnDesc + "]  Not found  desc=[" + desc + "] Properties , Please check Map File . One reason for this problem is wrong , Associated attributes in an entity set up systems of this entity , You did not follow the rules at the time of writing information to an entity set reftext, refvalue. Please verify .");
        }
        #endregion

        #region  Calculated property 
        /// <summary>
        ///  Via maximum TB Width .
        /// </summary>
        private int _MaxTBLength = 0;
        /// <summary>
        ///  Top TB Width .
        /// </summary>
        public float MaxTBLength
        {
            get
            {
                if (_MaxTBLength == 0)
                {
                    foreach (Attr attr in this.Attrs)
                    {
                        if (attr.UIWidth > _MaxTBLength)
                        {
                            _MaxTBLength = (int)attr.UIWidth;
                        }
                    }
                }
                return _MaxTBLength;
            }
        }
        /// <summary>
        ///  Physical keyboard set 
        /// </summary>
        private Attrs _HisPhysicsAttrs = null;
        /// <summary>
        ///  Physical keyboard set 
        /// </summary>
        public Attrs HisPhysicsAttrs
        {
            get
            {
                if (_HisPhysicsAttrs == null)
                {
                    _HisPhysicsAttrs = new Attrs();
                    foreach (Attr attr in this.Attrs)
                    {
                        if (attr.MyFieldType == FieldType.NormalVirtual || attr.MyFieldType == FieldType.RefText)
                            continue;
                        _HisPhysicsAttrs.Add(attr, false, this.IsAddRefName);
                    }
                }
                return _HisPhysicsAttrs;
            }
        }
        /// <summary>
        ///  His foreign key set 
        /// </summary>
        private Attrs _HisFKAttrs = null;
        /// <summary>
        ///  His foreign key set 
        /// </summary>
        public Attrs HisFKAttrs
        {
            get
            {
                if (_HisFKAttrs == null)
                {
                    _HisFKAttrs = new Attrs();
                    foreach (Attr attr in this.Attrs)
                    {
                        if (attr.MyFieldType == FieldType.FK
                            || attr.MyFieldType == FieldType.PKFK)
                        {
                            _HisFKAttrs.Add(attr, false, false);
                        }
                    }
                }
                return _HisFKAttrs;
            }
        }
        private int _isFull = -1;
        /// <summary>
        ///  Is there an automatic calculation 
        /// </summary>
        public bool IsHaveAutoFull
        {
            get
            {
                if (_isFull == -1)
                {
                    foreach (Attr attr in _attrs)
                    {
                        if (attr.AutoFullDoc != null)
                            _isFull = 1;
                    }
                    if (_isFull == -1)
                        _isFull = 0;
                }
                if (_isFull == 0)
                    return false;
                return true;
            }
        }
        public bool IsHaveFJ=false;
        /// <summary>
        ///  Moved to the display mode 
        /// </summary>
        public string TitleExt = null;
        private int _isJs = -1;
        public bool IsHaveJS
        {
            get
            {
                if (_isJs == -1)
                {
                    foreach (Attr attr in _attrs)
                    {
                        if (attr.AutoFullDoc == null)
                            continue;
                        if (attr.AutoFullWay == AutoFullWay.Way1_JS)
                        {
                            _isJs = 1;
                            break;
                        }
                    }

                    if (_isJs == -1)
                        _isJs = 0;
                }

                if (_isJs == 0)
                    return false;
                return true;
            }
        }
        /// <summary>
        ///  Whether to join the name associated 
        /// AttrKey -  AttrKeyName 
        /// </summary>
        public bool IsAddRefName = false;
        /// <summary>
        ///  His foreign key Enum Set 
        /// </summary>
        private Attrs _HisEnumAttrs = null;
        /// <summary>
        ///  His foreign key Enum Set 
        /// </summary>
        public Attrs HisEnumAttrs
        {
            get
            {
                if (_HisEnumAttrs == null)
                {
                    _HisEnumAttrs = new Attrs();
                    foreach (Attr attr in this.Attrs)
                    {
                        if (attr.MyFieldType == FieldType.Enum || attr.MyFieldType == FieldType.PKEnum)
                        {
                            _HisEnumAttrs.Add(attr, true, false);
                        }
                    }
                }
                return _HisEnumAttrs;
            }
        }
        /// <summary>
        ///  His foreign key EnumandPk Set 
        /// </summary>
        private Attrs _HisFKEnumAttrs = null;
        /// <summary>
        ///  His foreign key EnumandPk Set 
        /// </summary>
        public Attrs HisFKEnumAttrs
        {
            get
            {
                if (_HisFKEnumAttrs == null)
                {
                    _HisFKEnumAttrs = new Attrs();
                    foreach (Attr attr in this.Attrs)
                    {
                        if (attr.MyFieldType == FieldType.Enum
                            || attr.MyFieldType == FieldType.PKEnum
                            || attr.MyFieldType == FieldType.FK
                            || attr.MyFieldType == FieldType.PKFK)
                        {
                            _HisFKEnumAttrs.Add(attr);
                        }
                    }
                }
                return _HisFKEnumAttrs;
            }
        }
        #endregion

        #region  His physical configuration information 
        private Attrs _HisCfgAttrs = null;
        public Attrs HisCfgAttrs
        {
            get
            {
                if (this._HisCfgAttrs == null)
                {
                    this._HisCfgAttrs = new Attrs();
                    if (Web.WebUser.No == "admin")
                    {

                        this._HisCfgAttrs.AddDDLSysEnum("UIRowStyleGlo", 2," Table row style ( Application Global )", true, false, "UIRowStyleGlo", 
                            "@0= No style @1= Alternate style @2= Mouse movement @3= Alternately and mouse movement ");

                        this._HisCfgAttrs.AddBoolen("IsEnableDouclickGlo", true,
                             " Whether to start double-click to open ( Application Global )");

                        this._HisCfgAttrs.AddBoolen("IsEnableFocusField", true, " Whether the focus is enabled field ");
                        this._HisCfgAttrs.AddTBString("FocusField", null, " Focus field ( Click to open the column is used to display ",
                            true, false, 0, 20, 20);
                        this._HisCfgAttrs.AddBoolen("IsEnableRefFunc", true, " Whether related functions enabled column ");
                        this._HisCfgAttrs.AddBoolen("IsEnableOpenICON", true, " Whether to enable open icon ");
                        this._HisCfgAttrs.AddDDLSysEnum("MoveToShowWay", 0," Moved to the display mode ", true, false,
                            "MoveToShowWay", "@0= Do not show @1= Drop-down list @2= Tile ");
                        this._HisCfgAttrs.AddTBString("MoveTo", null, " Move to the field ", true, false, 0, 20, 20);
                        this._HisCfgAttrs.AddTBInt("WinCardW", 820, " Pop-up window width ", true, false);
                        this._HisCfgAttrs.AddTBInt("WinCardH", 480, " Pop-up height ", true, false);
                        this._HisCfgAttrs.AddDDLSysEnum("EditerType", 0,  " Chunks of text editors ", 
                            true, false, "EditerType", "@0=无@1=sina Editor @2=FKCEditer@3=KindEditor@4=UEditor");

                      //  this._HisCfgAttrs.AddDDLSysEnum("UIRowStyleGlo", 2, " Table row style ( Application Global )", true, false, "UIRowStyleGlo", "@0= No style @1= Alternate style @2= Mouse movement @3= Alternately and mouse movement ");
                    }
                }
                return _HisCfgAttrs;
            }
        }
        #endregion

        #region  His related information .
        private Attrs _HisRefAttrs = null;
        public Attrs HisRefAttrs
        {
            get
            {
                if (this._HisRefAttrs == null)
                {
                    this._HisRefAttrs = new Attrs();

                    foreach (Attr attr in this.Attrs)
                    {
                        if (attr.MyFieldType == FieldType.FK || attr.MyFieldType == FieldType.PKFK)
                        {
                            _HisRefAttrs.Add(attr);
                        }
                    }
                }
                return _HisRefAttrs;
            }
        }
        #endregion

        #region  About related functions 
        /// <summary>
        ///  Add a related functions 
        /// </summary>
        /// <param name="title"> Title </param>
        /// <param name="classMethodName"> Connection </param>
        /// <param name="icon"> Icon </param>
        /// <param name="tooltip"> Message </param>
        /// <param name="target"> Connected to </param>
        /// <param name="width"> Width </param>
        /// <param name="height"> Height </param>
        public void AddRefMethod(string title, string classMethodName, Attrs attrs, string warning, string icon, string tooltip, string target, int width, int height)
        {
            RefMethod func = new RefMethod();
            func.Title = title;
            func.Warning = warning;
            func.ClassMethodName = classMethodName;
            func.Icon = icon;
            func.ToolTip = tooltip;
            func.Width = width;
            func.Height = height;
            func.HisAttrs = attrs;
            this.HisRefMethods.Add(func);
        }
        public void AddRefMethodOpen()
        {
            RefMethod func = new RefMethod();
            func.Title = " Turn on ";
            func.ClassMethodName = this.ToString() + ".DoOpenCard";
            func.Icon = "/WF/Img/Btn/Edit.gif";
            this.HisRefMethods.Add(func);
        }
        /// <summary>
        ///  Increase 
        /// </summary>
        /// <param name="func"></param>
        public void AddRefMethod(RefMethod rm)
        {
            this.HisRefMethods.Add(rm);
        }
        #endregion

        #region  His detailed information about 
        /// <summary>
        ///  Increase Details 
        /// </summary>
        /// <param name="ens"> Collection of information </param>
        /// <param name="refKey"> Property </param>
        public void AddDtl(Entities ens, string refKey)
        {
            EnDtl dtl = new EnDtl();
            dtl.Ens = ens;
            dtl.RefKey = refKey;
            this.Dtls.Add(dtl);
        }
        /// <summary>
        ///  Related functions s
        /// </summary> 
        private RefMethods _RefMethods = null;
        /// <summary>
        ///  Related functions 
        /// </summary>
        public RefMethods HisRefMethods
        {
            get
            {
                if (this._RefMethods == null)
                    _RefMethods = new RefMethods();

                return _RefMethods;
            }
        }
        /// <summary>
        ///  Details s
        /// </summary> 
        private EnDtls _Dtls = null;
        /// <summary>
        ///  His details 
        /// </summary>
        public EnDtls Dtls
        {
            get
            {
                if (this._Dtls == null)
                    _Dtls = new EnDtls();

                return _Dtls;
            }
        }
        /// <summary>
        ///  All the details 
        /// </summary> 
        private EnDtls _DtlsAll = null;
        /// <summary>
        ///  All the details 
        /// </summary>
        public EnDtls DtlsAll
        {
            get
            {
                if (this._DtlsAll == null)
                {
                    _DtlsAll = this.Dtls;

                    //  Joined his multiple choice .
                    foreach (AttrOfOneVSM en in this.AttrsOfOneVSM)
                    {
                        EnDtl dtl = new EnDtl();
                        dtl.Ens = en.EnsOfMM;
                        dtl.RefKey = en.AttrOfOneInMM;
                        //dtl.Desc =en.Desc;
                        //dtl.Desc = en.Desc ;
                        _DtlsAll.Add(dtl);
                    }

                }
                return _DtlsAll;
            }
        }
        #endregion

        #region  Number culvert construction 
        /// <summary>
        ///  Number culvert construction  
        /// </summary>
        /// <param name="dburl"> Database Connectivity </param>
        /// <param name="physicsTable"> Physical table.</param>
        public Map(DBUrl dburl, string physicsTable)
        {
            this.EnDBUrl = dburl;
            this.PhysicsTable = physicsTable;
        }
        /// <summary>
        ///  Number culvert construction 
        /// </summary>
        /// <param name="physicsTable"> Physical table</param>
        public Map(string physicsTable)
        {
            this.PhysicsTable = physicsTable;
        }
        /// <summary>
        ///  Number culvert construction 
        /// </summary>
        /// <param name="DBUrlKeyList"> Connected Key  You can use   DBUrlKeyList  Get </param>
        /// <param name="physicsTable"> Physical table </param>
        public Map(DBUrlType dburltype, string physicsTable)
        {
            this.EnDBUrl = new DBUrl(dburltype);
            this.PhysicsTable = physicsTable;
        }
        /// <summary>
        ///  Number culvert construction 
        /// </summary>
        public Map() { }
        #endregion

        #region  Property 
        /// <summary>
        ///  #NAME? 
        /// </summary>
        private AttrsOfOneVSM _AttrsOfOneVSM = new AttrsOfOneVSM();
        /// <summary>
        ///  Point-to-many association 
        /// </summary>
        public AttrsOfOneVSM AttrsOfOneVSM
        {
            get
            {
                if (this._AttrsOfOneVSM == null)
                    this._AttrsOfOneVSM = new AttrsOfOneVSM();
                return this._AttrsOfOneVSM;
            }
            set
            {
                this._AttrsOfOneVSM = value;
            }
        }
        /// <summary>
        ///  Remove his name through a multi-entity class OneVSM Property .
        /// </summary>
        /// <param name="ensOfMMclassName"></param>
        /// <returns></returns>
        public AttrOfOneVSM GetAttrOfOneVSM(string ensOfMMclassName)
        {
            foreach (AttrOfOneVSM attr in this.AttrsOfOneVSM)
            {
                if (attr.EnsOfMM.ToString() == ensOfMMclassName)
                {
                    return attr;
                }
            }
            throw new Exception("error param:  " + ensOfMMclassName);
        }
        /// <summary>
        ///  File Type 
        /// </summary>
        private AdjunctType _AdjunctType = AdjunctType.None;
        /// <summary>
        ///  File Type 
        /// </summary>
        public AdjunctType AdjunctType
        {
            get
            {
                return this._AdjunctType;
            }
            set
            {
                this._AdjunctType = value;
            }
        }
        public string MoveTo = null;
        /// <summary>
        ///  Entity Description 
        /// </summary>
        string _EnDesc = "";
        public string EnDesc
        {
            get
            {
                return this._EnDesc;
            }
            set
            {
                this._EnDesc = value;
            }
        }
        public bool IsShowSearchKey = true;
        public BP.Sys.DTSearchWay DTSearchWay= BP.Sys.DTSearchWay.None;
        public string  DTSearchKey = null;
        /// <summary>
        ///  Picture DefaultImageUrl
        /// </summary>
        public string Icon = "../Images/En/Default.gif";
        /// <summary>
        ///  Entity Type 
        /// </summary>
        EnType _EnType = EnType.App;
        /// <summary>
        ///  Entity Type   The default is 0( User application ).
        /// </summary>
        public EnType EnType
        {
            get
            {
                return this._EnType;
            }
            set
            {
                this._EnType = value;
            }
        }
        #region   According build properties xml.
        private string PKs = "";
        public void GenerMap(string xml)
        {
            DataSet ds = new DataSet("");
            ds.ReadXml(xml);
            foreach (DataTable dt in ds.Tables)
            {
                switch (dt.TableName)
                {
                    case "Base":
                        this.DealDT_Base(dt);
                        break;
                    case "Attr":
                        this.DealDT_Attr(dt);
                        break;
                    case "SearchAttr":
                        this.DealDT_SearchAttr(dt);
                        break;
                    case "Dtl":
                        this.DealDT_SearchAttr(dt);
                        break;
                    case "Dot2Dot":
                        this.DealDT_Dot2Dot(dt);
                        break;
                    default:
                        throw new Exception("XML  Configuration information is incorrect , No agreement marks :" + dt.TableName);
                }
            }
            //  Check the configuration of the integrity of the .

        }

        private void DealDT_Base(DataTable dt)
        {
            if (dt.Rows.Count != 1)
                throw new Exception(" Basic information configuration error , Not more or less than 1 Rows .");
            foreach (DataColumn dc in dt.Columns)
            {
                string val = dt.Rows[0][dc.ColumnName].ToString();
                if (val == null)
                    continue;
                if (dt.Rows[0][dc.ColumnName] == DBNull.Value)
                    continue;

                switch (dc.ColumnName)
                {
                    case "EnDesc":
                        this.EnDesc = val;
                        break;
                    case "Table":
                        this.PhysicsTable = val;
                        break;
                    case "DBUrl":
                        this.EnDBUrl = new DBUrl(DataType.GetDBUrlByString(val));
                        break;
                    case "ICON":
                        this.Icon = val;
                        break;
                    case "CodeStruct":
                        this.CodeStruct = val;
                        break;
                    case "AdjunctType":
                        //this.PhysicsTable=val;
                        break;
                    case "EnType":
                        switch (val)
                        {
                            case "Admin":
                                this.EnType = BP.En.EnType.Admin;
                                break;
                            case "App":
                                this.EnType = BP.En.EnType.App;
                                break;
                            case "Dot2Dot":
                                this.EnType = BP.En.EnType.Dot2Dot;
                                break;
                            case "Dtl":
                                this.EnType = BP.En.EnType.Dtl;
                                break;
                            case "Etc":
                                this.EnType = BP.En.EnType.Etc;
                                break;
                            case "PowerAble":
                                this.EnType = BP.En.EnType.PowerAble;
                                break;
                            case "Sys":
                                this.EnType = BP.En.EnType.Sys;
                                break;
                            case "View":
                                this.EnType = BP.En.EnType.View;
                                break;
                            case "XML":
                                this.EnType = BP.En.EnType.XML;
                                break;
                            default:
                                throw new Exception(" No agreement marks :EnType =  " + val);
                        }
                        break;
                    case "DepositaryOfEntity":
                        switch (val)
                        {
                            case "Application":
                                this.DepositaryOfEntity = Depositary.Application;
                                break;
                            case "None":
                                this.DepositaryOfEntity = Depositary.None;
                                break;
                            case "Session":
                                this.DepositaryOfEntity = Depositary.Application;
                                break;
                            default:
                                throw new Exception(" No agreement marks :DepositaryOfEntity=[" + val + "]  Should be selected for ,Application, None, Session ");
                        }
                        break;
                    case "DepositaryOfMap":
                        switch (val)
                        {
                            case "Application":
                            case "Session":
                                this.DepositaryOfMap = Depositary.Application;
                                break;
                            case "None":
                                this.DepositaryOfMap = Depositary.None;
                                break;
                            default:
                                throw new Exception(" No agreement marks :DepositaryOfMap=[" + val + "]  Should be selected for ,Application, None, Session ");
                        }
                        break;
                    case "PKs":
                        this.PKs = val;
                        break;
                    default:
                        throw new Exception(" Basic information, there is no agreement marks :" + val);
                }
            }
        }
        private void DealDT_Attr(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                Attr attr = new Attr();
                foreach (DataColumn dc in dt.Columns)
                {
                    string val = dr[dc.ColumnName].ToString();
                    switch (dc.ColumnName)
                    {
                        case "Key":
                            attr.Key = val;
                            break;
                        case "Field":
                            attr.Field = val;
                            break;
                        case "DefVal":
                            attr.DefaultVal = val;
                            break;
                        case "DT":
                            attr.MyDataType = DataType.GetDataTypeByString(val);
                            break;
                        case "UIBindKey":
                            attr.UIBindKey = val;
                            break;
                        case "UIIsReadonly":
                            if (val == "1" || val.ToUpper() == "TRUE")
                                attr.UIIsReadonly = true;
                            else
                                attr.UIIsReadonly = false;
                            break;
                        case "MinLen":
                            attr.MinLength = int.Parse(val);
                            break;
                        case "MaxLen":
                            attr.MaxLength = int.Parse(val);
                            break;
                        case "TBLen":
                            attr.UIWidth = int.Parse(val);
                            break;
                        default:
                            throw new Exception(" No agreement marks :" + val);
                    }
                }

                //  Determine property .
                if (attr.UIBindKey == null)
                {
                    /*  Description is not set foreign key or enumeration type .*/
                    //if (attr.MyDataType
                }
                else
                {
                    if (attr.UIBindKey.IndexOf(".") != -1)
                    {
                        /* That it is a class .*/
                        Entities ens = attr.HisFKEns;
                        EntitiesNoName ensNoName = ens as EntitiesNoName;
                        if (ensNoName == null)
                        {
                            /* Without a successful conversion of .*/
                        }
                        else
                        {
                            /* Has been converted successfully ,  Indicates that it is EntityNoName  Type . */
                            if (this.PKs.IndexOf(attr.Key) != -1)
                            {
                                /*  If it is a primary key   */
                                if (attr.Field == "")
                                    attr.Field = attr.Key;
                                this.AddDDLEntitiesPK(attr.Key, attr.Field, attr.DefaultVal.ToString(), attr.Desc, ensNoName, attr.UIIsReadonly);
                            }
                            else
                            {
                                this.AddDDLEntities(attr.Key, attr.Field, attr.DefaultVal.ToString(), attr.Desc, ensNoName, attr.UIIsReadonly);
                            }
                        }

                    }
                    else
                    {
                    }

                }


            }
        }
        private void DealDT_SearchAttr(DataTable dt)
        {
        }
        private void DealDT_Dtl(DataTable dt)
        {
        }
        private void DealDT_Dot2Dot(DataTable dt)
        {
        }
        #endregion

        #region  And Generation No String relevant 
        /// <summary>
        ///  The length of the string field is generated .
        /// </summary>
        int _GenerNoLength = 0;
        public int GenerNoLength
        {
            get
            {
                if (this._GenerNoLength == 0)
                    throw new Exception("@ Not specified field length of the string generated .");
                return this._GenerNoLength;
            }
            set
            {
                this._GenerNoLength = value;
            }
        }
        /// <summary>
        ///  Coding structure 
        ///  Such as : 0, 2322;
        /// </summary>
        string _CodeStruct = "2";
        /// <summary>
        ///  Coding structure 
        /// </summary>
        public string CodeStruct
        {
            get
            {
                return this._CodeStruct;
            }
            set
            {
                this._CodeStruct = value;
                this.IsAutoGenerNo = true;
            }
        }
        /// <summary>
        ///  The total length of the number of .
        /// </summary>
        public int CodeLength
        {
            get
            {
                int i = 0;
                if (CodeStruct.Length == 0)
                {
                    i = int.Parse(this.CodeStruct);
                }
                else
                {
                    char[] s = this.CodeStruct.ToCharArray();
                    foreach (char c in s)
                    {
                        i = i + int.Parse(c.ToString());
                    }
                }

                return i;
            }
        }
        /// <summary>
        ///  Whether to allow duplicate names ( The default is not allowed to repeat .)
        /// </summary>
        private bool _IsAllowRepeatName = true;
        /// <summary>
        ///  Whether to allow duplicate names .
        /// 在insert,update  Check before .
        /// </summary>
        public bool IsAllowRepeatName
        {
            get
            {
                return _IsAllowRepeatName;
            }
            set
            {
                _IsAllowRepeatName = value;
            }
        }
        /// <summary>
        ///  Whether automatic numbering 
        /// </summary>
        private bool _IsAutoGenerNo = false;
        /// <summary>
        ///  Whether automatic numbering .		 
        /// </summary>
        public bool IsAutoGenerNo
        {
            get
            {
                return _IsAutoGenerNo;
            }
            set
            {
                _IsAutoGenerNo = value;
            }
        }
        /// <summary>
        ///  Check that the number length .（ The default false）
        /// </summary>
        private bool _IsCheckNoLength = false;
        /// <summary>
        ///  Check that the number length .
        /// 在insert  Check before .
        /// </summary>
        public bool IsCheckNoLength
        {
            get
            {
                return _IsCheckNoLength;
            }
            set
            {
                _IsCheckNoLength = value;
            }
        }
        #endregion

        #region  Associated with the connection .

        DBUrl _EnDBUrl = null;
        /// <summary>
        ///  Database Connectivity 
        /// </summary>
        public DBUrl EnDBUrl
        {
            get
            {
                if (this._EnDBUrl == null)
                {
                    _EnDBUrl = new DBUrl();
                }
                return this._EnDBUrl;
            }
            set
            {
                this._EnDBUrl = value;
            }
        }
        private string _PhysicsTable = null;

        public bool IsView
        {
            get
            {
                string sql = "";
                switch (this.EnDBUrl.DBType)
                {
                    case DBType.Oracle:
                        sql = "SELECT TABTYPE  FROM TAB WHERE UPPER(TNAME)=:v";
                        DataTable oradt = DBAccess.RunSQLReturnTable(sql, "v", this.PhysicsTableExt.ToUpper());
                        if (oradt.Rows.Count == 0)
                            throw new Exception("@ Table does not exist [" + this.PhysicsTableExt + "]");
                        if (oradt.Rows[0][0].ToString().ToUpper().Trim() == "V".ToString())
                            return true;
                        else
                            return false;
                        break;
                    case DBType.Access:
                        sql = "select   Type   from   msysobjects   WHERE   UCASE(name)='" + this.PhysicsTableExt.ToUpper() + "'";
                        DataTable dtw = DBAccess.RunSQLReturnTable(sql);
                        if (dtw.Rows.Count == 0)
                            throw new Exception("@ Table does not exist [" + this.PhysicsTableExt + "]");
                        if (dtw.Rows[0][0].ToString().Trim() == "5")
                            return true;
                        else
                            return false;
                    case DBType.MSSQL:
                        sql = "select xtype from sysobjects WHERE name ="+SystemConfig.AppCenterDBVarStr+"v";
                        DataTable dt1 = DBAccess.RunSQLReturnTable(sql, "v", this.PhysicsTableExt);
                        if (dt1.Rows.Count == 0)
                            throw new Exception("@ Table does not exist [" + this.PhysicsTableExt + "]");

                        if (dt1.Rows[0][0].ToString().ToUpper().Trim() == "V".ToString() )
                            return true;
                        else
                            return false;
                    case DBType.Informix:
                        sql = "select tabtype from systables where tabname = '"+this.PhysicsTableExt.ToLower()+"'";
                        DataTable dtaa = DBAccess.RunSQLReturnTable(sql);
                        if (dtaa.Rows.Count == 0)
                            throw new Exception("@ Table does not exist [" + this.PhysicsTableExt + "]");

                        if (dtaa.Rows[0][0].ToString().ToUpper().Trim() == "V")
                            return true;
                        else
                            return false;
                    case DBType.MySQL:
                        sql = "SELECT Table_Type FROM information_schema.TABLES WHERE table_name=:v and table_schema='"+SystemConfig.AppCenterDBDatabase+"'";
                        DataTable dt2 = DBAccess.RunSQLReturnTable(sql, "v", this.PhysicsTableExt);
                        if (dt2.Rows.Count == 0)
                            throw new Exception("@ Table does not exist [" + this.PhysicsTableExt + "]");

                        if (dt2.Rows[0][0].ToString().ToUpper().Trim() == "VIEW")
                            return true;
                        else
                            return false;
                    default:
                        throw new Exception("@ The judge did not do .");
                }

                DataTable dt = DBAccess.RunSQLReturnTable(sql, "v", this.PhysicsTableExt.ToUpper());
                if (dt.Rows.Count == 0)
                    throw new Exception("@ Table does not exist [" + this.PhysicsTableExt + "]");

                if (dt.Rows[0][0].ToString() == "VIEW")
                    return true;
                else
                    return false;
            }
        }

        public string PhysicsTableExt
        {
            get
            {
                if (this.PhysicsTable.IndexOf(".") != -1)
                {
                    string[] str = this.PhysicsTable.Split('.');
                    return str[1];
                }
                else
                    return this.PhysicsTable;
            }
        }
        /// <summary>
        ///  Physical table name 
        /// </summary>
        /// <returns>Table name</returns>
        public string PhysicsTable
        {
            get
            {
                return this._PhysicsTable;
                /*
                if (DBAccess.AppCenterDBType==DBType.Oracle)
                {
                    return ""+this._PhysicsTable+"";
                }
                else
                {
                    return this._PhysicsTable;
                }
                */
            }
            set
            {
                //  Because the composition of the select  Statements into the memory , Time should also be amended to modify its data memory .
                //DA.Cash.AddObj(this.ToString()+"SQL",Depositary.Application,null);

                DA.Cash.RemoveObj(this.ToString() + "SQL", Depositary.Application);
                Cash.RemoveObj("MapOf" + this.ToString(), this.DepositaryOfMap); // RemoveObj

                //DA.Cash.setObj(en.ToString()+"SQL",en.EnMap.DepositaryOfMap) as string;
                this._PhysicsTable = value;
            }
        }
        #endregion

        private Attrs _attrs = null;
        public Attrs Attrs
        {
            get
            {
                if (this._attrs == null)
                    this._attrs = new Attrs();
                return this._attrs;
            }
            set
            {
                if (this._attrs == null)
                    this._attrs = new En.Attrs();

                Attrs myattrs = value;
                foreach (Attr item in myattrs)
                    this._attrs.Add(item);
            }
        }
        #endregion

        #region  In property-related operations 

        #region DDL

        #region  To help set   Fixed   Enum types are operating relationship .
        public void AddDDLFixEnum(string key, string field, int defaultVal, bool IsPK, string desc, DDLShowType showtype, bool isReadonly)
        {
            Attr attr = new Attr();
            attr.Key = key;
            attr.Field = field;
            attr.DefaultVal = defaultVal;
            attr.MyDataType = DataType.AppInt;

            if (IsPK)
                attr.MyFieldType = FieldType.PK;
            else
                attr.MyFieldType = FieldType.Normal;

            attr.Desc = desc;
            attr.UIContralType = UIContralType.DDL;
            attr.UIDDLShowType = showtype;
            attr.UIIsReadonly = isReadonly;
            this.Attrs.Add(attr);
        }
        public void AddDDLFixEnumPK(string key, int defaultVal, string desc, DDLShowType showtype, bool isReadonly)
        {
            this.AddDDLFixEnum(key, key, defaultVal, true, desc, showtype, isReadonly);
        }
        public void AddDDLFixEnumPK(string key, string field, int defaultVal, string desc, DDLShowType showtype, bool isReadonly)
        {
            this.AddDDLFixEnumPK(key, field, defaultVal, desc, showtype, isReadonly);
        }
        public void AddDDLFixEnum(string key, int defaultVal, string desc, DDLShowType showtype, bool isReadonly)
        {
            this.AddDDLFixEnum(key, key, defaultVal, false, desc, showtype, isReadonly);
        }
        public void AddBoolean_del(string key, int defaultVal, string desc, bool isReadonly)
        {
            this.AddDDLFixEnum(key, key, defaultVal, false, desc, DDLShowType.Boolean, isReadonly);
        }
        public void AddBoolean_del(string key, string field, int defaultVal, string desc, bool isReadonly)
        {
            this.AddDDLFixEnum(key, field, defaultVal, false, desc, DDLShowType.Boolean, isReadonly);
        }
        #endregion

        #region  与boolen  Related operations .
        /// <summary>
        ///  Increase boolen  Related operations .
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="field">field</param>
        /// <param name="defaultVal">defaultVal</param>
        /// <param name="desc">desc</param>
        /// <param name="isUIEnable">isUIEnable</param>
        /// <param name="isUIVisable">isUIVisable</param>
        public void AddBoolean(string key, string field, bool defaultVal, string desc, bool isUIVisable, bool isUIEnable, bool isLine)
        {
            Attr attr = new Attr();
            attr.Key = key;
            attr.Field = field;

            if (defaultVal)
                attr.DefaultVal = 1;
            else
                attr.DefaultVal = 0;

            attr.MyDataType = DataType.AppBoolean;
            attr.Desc = desc;
            attr.UIContralType = UIContralType.CheckBok;
            attr.UIIsReadonly = isUIEnable;
            attr.UIVisible = isUIVisable;
            attr.UIIsLine = isLine;
            this.Attrs.Add(attr);
        }
        /// <summary>
        ///  Increase boolen  Related operations .
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="field">field</param>
        /// <param name="defaultVal">defaultVal</param>
        /// <param name="desc">desc</param>
        /// <param name="isUIEnable">isUIEnable</param>
        /// <param name="isUIVisable">isUIVisable</param>
        public void AddBoolean(string key, bool defaultVal, string desc, bool isUIVisable, bool isUIEnable)
        {
            AddBoolean(key, key, defaultVal, desc, isUIVisable, isUIEnable,false);
        }

        /// <summary>
        ///  Increase boolen  Related operations .
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="field">field</param>
        /// <param name="defaultVal">defaultVal</param>
        /// <param name="desc">desc</param>
        /// <param name="isUIEnable">isUIEnable</param>
        /// <param name="isUIVisable">isUIVisable</param>
        public void AddBoolean(string key, bool defaultVal, string desc, bool isUIVisable, bool isUIEnable, bool isLine)
        {
            AddBoolean(key, key, defaultVal, desc, isUIVisable, isUIEnable, isLine);
        }


        #endregion

        #region  To help set custom , Enum types are operating relationship .
        public void AddDDLSysEnumPK(string key, string field, int defaultVal, string desc, bool isUIVisable, bool isUIEnable, string sysEnumKey)
        {
            Attr attr = new Attr();
            attr.Key = key;
            attr.Field = field;
            attr.DefaultVal = defaultVal;
            attr.MyDataType = DataType.AppInt;
            attr.MyFieldType = FieldType.PKEnum;
            attr.Desc = desc;
            attr.UIContralType = UIContralType.DDL;
            attr.UIDDLShowType = DDLShowType.SysEnum;
            attr.UIBindKey = sysEnumKey;
            attr.UIVisible = isUIVisable;
            attr.UIIsReadonly = isUIEnable;
            this.Attrs.Add(attr);
        }
        /// <summary>
        ///  Custom enumeration type 
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field"> Field </param>
        /// <param name="defaultVal"> Default </param>
        /// <param name="desc"> Description </param>
        /// <param name="sysEnumKey">Key</param>
        public void AddDDLSysEnum(string key, string field, int defaultVal, string desc, bool isUIVisable, bool isUIEnable, string sysEnumKey, string cfgVal, bool isLine)
        {
            Attr attr = new Attr();
            attr.Key = key;
            attr.Field = field;
            attr.DefaultVal = defaultVal;
            attr.MyDataType = DataType.AppInt;
            attr.MyFieldType = FieldType.Enum;
            attr.Desc = desc;
            attr.UIContralType = UIContralType.DDL;
            attr.UIDDLShowType = DDLShowType.SysEnum;
            attr.UIBindKey = sysEnumKey;
            attr.UITag = cfgVal;
            attr.UIVisible = isUIVisable;
            attr.UIIsReadonly = isUIEnable;
            attr.UIIsLine = isLine;
            this.Attrs.Add(attr);
        }
        /// <summary>
        ///  Custom enumeration type 
        /// </summary>
        /// <param name="key">键</param>		
        /// <param name="defaultVal"> Default </param>
        /// <param name="desc"> Description </param>
        /// <param name="sysEnumKey">Key</param>
        public void AddDDLSysEnum(string key, int defaultVal, string desc, bool isUIVisable, bool isUIEnable, string sysEnumKey)
        {
            AddDDLSysEnum(key, key, defaultVal, desc, isUIVisable, isUIEnable, sysEnumKey, null,false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultVal"></param>
        /// <param name="desc"></param>
        /// <param name="isUIVisable"></param>
        /// <param name="isUIEnable"></param>
        /// <param name="sysEnumKey"></param>
        /// <param name="cfgVal"></param>
        public void AddDDLSysEnum(string key, int defaultVal, string desc, bool isUIVisable, bool isUIEnable, string sysEnumKey, string cfgVal,bool isLine)
        {
            AddDDLSysEnum(key, key, defaultVal, desc, isUIVisable, isUIEnable, sysEnumKey, cfgVal, isLine);
        }
        public void AddDDLSysEnum(string key, int defaultVal, string desc, bool isUIVisable, bool isUIEnable, string sysEnumKey, string cfgVal)
        {
            AddDDLSysEnum(key, key, defaultVal, desc, isUIVisable, isUIEnable, sysEnumKey, cfgVal,false);
        }
        public void AddDDLSysEnum(string key, int defaultVal, string desc, bool isUIVisable, bool isUIEnable)
        {
            AddDDLSysEnum(key, key, defaultVal, desc, isUIVisable, isUIEnable, key, null, false);
        }
        #endregion


        #region  To help set custom , Enum types are operating relationship .
        /// <summary>
        ///  Custom enumeration type 
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field"> Field </param>
        /// <param name="defaultVal"> Default </param>
        /// <param name="desc"> Description </param>
        /// <param name="sysEnumKey">Key</param>
        public void AddRadioBtnSysEnum(string key, string field, int defaultVal, string desc, bool isUIVisable, bool isUIEnable, string sysEnumKey)
        {
            Attr attr = new Attr();
            attr.Key = key;
            attr.Field = field;
            attr.DefaultVal = defaultVal;
            attr.MyDataType = DataType.AppInt;
            attr.MyFieldType = FieldType.Enum;
            attr.Desc = desc;
            attr.UIContralType = UIContralType.RadioBtn;
            attr.UIDDLShowType = DDLShowType.Self;
            attr.UIBindKey = sysEnumKey;
            attr.UIVisible = isUIVisable;
            attr.UIIsReadonly = isUIEnable;
            this.Attrs.Add(attr);
        }
        /// <summary>
        ///  Custom enumeration type 
        /// </summary>
        /// <param name="key">键</param>		
        /// <param name="defaultVal"> Default </param>
        /// <param name="desc"> Description </param>
        /// <param name="sysEnumKey">Key</param>
        public void AddRadioBtnSysEnum(string key, int defaultVal, string desc, bool isUIVisable, bool isUIEnable, string sysEnumKey)
        {
            AddDDLSysEnum(key, key, defaultVal, desc, isUIVisable, isUIEnable, sysEnumKey, null,false);
        }
        #endregion



        #region  And operated by an entity relationship .

        #region entityNoName
        public void AddDDLEntities(string key, string defaultVal, string desc, EntitiesSimpleTree ens, bool uiIsEnable)
        {
            this.AddDDLEntities(key, key, defaultVal, DataType.AppString, desc, ens, "No", "Name", uiIsEnable);
        }
        public void AddDDLEntities(string key, string defaultVal, string desc, EntitiesTree ens, bool uiIsEnable)
        {
            this.AddDDLEntities(key, key, defaultVal, DataType.AppString, desc, ens, "No", "Name", uiIsEnable);
        }
        public void AddDDLEntities(string key, string defaultVal, string desc, EntitiesNoName ens, bool uiIsEnable)
        {
            this.AddDDLEntities(key, key, defaultVal, DataType.AppString, desc, ens, "No", "Name", uiIsEnable);
        }
        public void AddDDLEntities(string key, string field, string defaultVal, string desc, EntitiesNoName ens, bool uiIsEnable)
        {
            this.AddDDLEntities(key, field, defaultVal, DataType.AppString, desc, ens, "No", "Name", uiIsEnable);
        }
        #endregion

        #region EntitiesOIDName
        public void AddDDLEntities(string key, int defaultVal, string desc, EntitiesOIDName ens, bool uiIsEnable)
        {
            this.AddDDLEntities(key, key, defaultVal, DataType.AppInt, desc, ens, "OID", "Name", uiIsEnable);
        }
        public void AddDDLEntities(string key, string field, object defaultVal, string desc, EntitiesOIDName ens, bool uiIsEnable)
        {
            this.AddDDLEntities(key, field, defaultVal, DataType.AppInt, desc, ens, "OID", "Name", uiIsEnable);
        }
        #endregion

        /// <summary>
        ///  Entity related to the operation of .
        /// </summary>
        /// <param name="key"> Health value </param>
        /// <param name="field"> Field </param>
        /// <param name="defaultVal"> Defaults </param>
        /// <param name="dataType">DataType Type </param>
        /// <param name="desc"> Description </param>
        /// <param name="ens"> Entity set </param>
        /// <param name="refKey"> Construction association </param>
        /// <param name="refText"> Associated Text</param>
        private void AddDDLEntities(string key, string field, object defaultVal, int dataType, FieldType _fildType, string desc, Entities ens, string refKey, string refText, bool uiIsEnable)
        {
            Attr attr = new Attr();
            attr.Key = key;
            attr.Field = field;
            attr.DefaultVal = defaultVal;
            attr.MyDataType = dataType;
            attr.MyFieldType = _fildType;
            attr.MaxLength = 50;

            attr.Desc = desc;
            attr.UIContralType = UIContralType.DDL;
            attr.UIDDLShowType = DDLShowType.Ens;
            attr.UIBindKey = ens.ToString();
            // attr.UIBindKeyOfEn = ens.GetNewEntity.ToString();

            attr.HisFKEns = ens;


            attr.HisFKEns = ens;
            attr.UIRefKeyText = refText;
            attr.UIRefKeyValue = refKey;
            attr.UIIsReadonly = uiIsEnable;

            this.Attrs.Add(attr, true, this.IsAddRefName);
        }
        public void AddDDLEntities(string key, string field, object defaultVal, int dataType, string desc, Entities ens, string refKey, string refText, bool uiIsEnable)
        {
            AddDDLEntities(key, field, defaultVal, dataType, FieldType.FK, desc, ens, refKey, refText, uiIsEnable);
        }
        /// <summary>
        ///  Entity related to the operation of . Field with the same name attribute .
        /// </summary>
        /// <param name="key"> Health value </param>
        /// <param name="field"> Field </param>
        /// <param name="defaultVal"> Defaults </param>
        /// <param name="dataType">DataType Type </param>
        /// <param name="desc"> Description </param>
        /// <param name="ens"> Entity set </param>
        /// <param name="refKey"> Construction association </param>
        /// <param name="refText"> Associated Text</param>
        public void AddDDLEntities(string key, object defaultVal, int dataType, string desc, Entities ens, string refKey, string refText, bool uiIsEnable)
        {
            AddDDLEntities(key, key, defaultVal, dataType, desc, ens, refKey, refText, uiIsEnable);
        }
        public void AddDDLEntitiesPK(string key, object defaultVal, int dataType, string desc, EntitiesTree ens, bool uiIsEnable)
        {
            AddDDLEntities(key, key, defaultVal, dataType, FieldType.PKFK, desc, ens, "No", "Name", uiIsEnable);
        }
        public void AddDDLEntitiesPK(string key, object defaultVal, int dataType, string desc, Entities ens, string refKey, string refText, bool uiIsEnable)
        {
            AddDDLEntities(key, key, defaultVal, dataType, FieldType.PKFK, desc, ens, refKey, refText, uiIsEnable);
        }
        public void AddDDLEntitiesPK(string key, string field, object defaultVal, int dataType, string desc, Entities ens, string refKey, string refText, bool uiIsEnable)
        {
            AddDDLEntities(key, field, defaultVal, dataType, FieldType.PKFK, desc, ens, refKey, refText, uiIsEnable);
        }

        #region  With respect to EntitiesNoName  Related operations .
        /// <summary>
        ///  With respect to EntitiesNoName  Related operations 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="defaultVal"></param>
        /// <param name="desc"></param>
        /// <param name="ens"></param>
        /// <param name="uiIsEnable"></param>
        public void AddDDLEntitiesPK(string key, string field, string defaultVal, string desc, EntitiesTree ens, bool uiIsEnable)
        {
            AddDDLEntities(key, field, (object)defaultVal, DataType.AppString, FieldType.PKFK, desc, ens, "No", "Name", uiIsEnable);
        }
        public void AddDDLEntitiesPK(string key, string field, string defaultVal, string desc, EntitiesNoName ens, bool uiIsEnable)
        {
            AddDDLEntities(key, field, (object)defaultVal, DataType.AppString, FieldType.PKFK, desc, ens, "No", "Name", uiIsEnable);
        }
        public void AddDDLEntitiesPK(string key, string defaultVal, string desc, EntitiesNoName ens, bool uiIsEnable)
        {
            AddDDLEntitiesPK(key, key, defaultVal, desc, ens, uiIsEnable);
        }
        public void AddDDLEntitiesPK(string key, string defaultVal, string desc, EntitiesTree ens, bool uiIsEnable)
        {
            AddDDLEntitiesPK(key, key, defaultVal, desc, ens, uiIsEnable);
        }
        public void AddDDLEntitiesPK(string key, string defaultVal, string desc, EntitiesSimpleTree ens, bool uiIsEnable)
        {
            AddDDLEntitiesPK(key, key, defaultVal, DataType.AppString,  desc, ens, "No", "Name", uiIsEnable);
        }
        #endregion

        #endregion






        #endregion

        #region TB

        #region string  Related operations .

        #region  With respect to 
        protected void AddTBString(string key, string field, object defaultVal, FieldType _FieldType, TBType tbType, string desc, bool uiVisable, bool isReadonly, int minLength, int maxLength, int tbWith, bool isUILine)
        {
            Attr attr = new Attr();
            attr.Key = key;

            attr.Field = field;
            attr.DefaultVal = defaultVal;
            attr.MyDataType = DataType.AppString;
            attr.Desc = desc;
            attr.UITBShowType = tbType;
            attr.UIVisible = uiVisable;
            attr.UIWidth = tbWith;
            attr.UIIsReadonly = isReadonly;
            attr.MaxLength = maxLength;
            attr.MinLength = minLength;
            attr.MyFieldType = _FieldType;
            attr.UIIsLine = isUILine;
            this.Attrs.Add(attr);
        }
        #endregion

        #region  Communal .
        /// <summary>
        ///  Synchronize the two entities property .
        /// </summary>
        public void AddAttrsFromMapData()
        {
            if (string.IsNullOrEmpty(this.FK_MapData))
                throw new Exception("@ You do not have to map的 FK_MapData  Assignment .");

            MapData md = null;
            md = new MapData();
            md.No = this.FK_MapData;
            if (md.RetrieveFromDBSources() == 0)
            {
                md.Name = this.FK_MapData;
                md.PTable = this.PhysicsTable;
                md.EnPK = this.PKs;
                md.Insert();
                md.RepairMap();
            }
            md.Retrieve();
            BP.Sys.MapAttrs attrs = new BP.Sys.MapAttrs(this.FK_MapData);

            /*把  Hand-written attr  Add  mapattrs Go inside . */
            foreach (Attr attr in this.Attrs)
            {
                if (attrs.Contains(BP.Sys.MapAttrAttr.KeyOfEn, attr.Key))
                    continue;

                if (attr.IsRefAttr)
                    continue;

                // The properties file entity class to go into the relationship between the entity classes .
                BP.Sys.MapAttr mapattrN = attr.ToMapAttr;
                mapattrN.FK_MapData = this.FK_MapData;
                if (mapattrN.UIHeight == 0)
                    mapattrN.UIHeight = 23;
                mapattrN.Insert();
                attrs.AddEntity(mapattrN);
            }

            // The relationship between the entity class property entity class to go into the file .
            foreach (BP.Sys.MapAttr attr in attrs)
            {
                if (this.Attrs.Contains(attr.KeyOfEn) == true)
                    continue;
                this.AddAttr(attr.HisAttr);
            }
        }
        public void AddAttrs(Attrs attrs)
        {
            foreach (Attr attr in attrs)
            {
                if (attr.IsRefAttr)
                    continue;
                this.Attrs.Add(attr);
            }
        }
        public void AddAttr(Attr attr)
        {
            this.Attrs.Add(attr);
        }
        public void AddAttr(string key, object defaultVal, int dbtype, bool isPk, string desc)
        {
            if (isPk)
                AddTBStringPK(key, key, desc, true, false, 0, 1000, 100);
            else
                AddTBString(key, key, defaultVal.ToString(), FieldType.Normal, TBType.TB, desc, true, false, 0, 1000, 100,false);
        }
        /// <summary>
        ///  Adding a textbox  Attribute types .
        /// </summary>
        /// <param name="key"> Health value </param>
        /// <param name="field"> Field values </param>
        /// <param name="defaultVal"> Defaults </param>
        /// <param name="_FieldType"> Field Type </param>
        /// <param name="desc"> Description </param>
        /// <param name="uiVisable"> Is not visible </param>
        /// <param name="uiVisable"> Is not read-only </param>
        /// <param name="minLength"> Minimum length </param>
        /// <param name="maxLength"> The maximum length </param>
        /// <param name="tbWith"> Width </param> 
        public void AddTBString(string key, string defaultVal, string desc, bool uiVisable, bool isReadonly, int minLength, int maxLength, int tbWith)
        {
            AddTBString(key, key, defaultVal, FieldType.Normal, TBType.TB, desc, uiVisable, isReadonly, minLength, maxLength, tbWith,false);
        }
        public void AddTBString(string key, string field, object defaultVal, string desc, bool uiVisable, bool isReadonly, int minLength, int maxLength, int tbWith)
        {
            AddTBString(key, field, defaultVal, FieldType.Normal, TBType.TB, desc, uiVisable, isReadonly, minLength, maxLength, tbWith,false);
        }
        public void AddTBString(string key, string defaultVal, string desc, bool uiVisable, bool isReadonly, int minLength, int maxLength, int tbWith,bool isUILine)
        {
            AddTBString(key, key, defaultVal, FieldType.Normal, TBType.TB, desc, uiVisable, isReadonly, minLength, maxLength, tbWith, isUILine);
        }
      
        /// <summary>
        ///  Accessories collection 
        /// </summary>
        public void AddMyFileS()
        {
            this.AddTBInt(EntityNoMyFileAttr.MyFileNum, 0, " Accessory ", false, false);
            this.IsHaveFJ = true;
        }
        /// <summary>
        ///  Accessories collection 
        /// </summary>
        /// <param name="desc"></param>
        public void AddMyFileS(string desc)
        {
            this.AddTBInt(EntityNoMyFileAttr.MyFileNum, 0, desc, false, false);
            this.IsHaveFJ = true;
        }
        /// <summary>
        ///  Add an accessory 
        /// </summary>
        public void AddMyFile()
        {
            this.AddTBString(EntityNoMyFileAttr.MyFileName, null, " Attachments or pictures ", false, false, 0, 100, 200);
            this.AddTBString(EntityNoMyFileAttr.MyFilePath, null, "MyFilePath", false, false, 0, 100, 200);
            this.AddTBString(EntityNoMyFileAttr.MyFileExt, null, "MyFileExt", false, false, 0, 10, 10);
            this.AddTBString(EntityNoMyFileAttr.WebPath, null, "WebPath", false, false, 0, 200, 10);

            this.AddTBInt(EntityNoMyFileAttr.MyFileH, 0, "MyFileH", false, false);
            this.AddTBInt(EntityNoMyFileAttr.MyFileW, 0, "MyFileW", false, false);
            this.AddTBFloat("MyFileSize", 0, "MyFileSize", false, false);
            this.IsHaveFJ = true;
        }
        /// <summary>
        ///  Add an accessory 
        /// </summary>
        /// <param name="fileDesc"> Description </param>
        public void AddMyFile(string fileDesc)
        {
            this.AddTBString(EntityNoMyFileAttr.MyFileName, null, fileDesc, false, false, 0, 100, 200);
            this.AddTBString(EntityNoMyFileAttr.MyFilePath, null, "MyFilePath", false, false, 0, 100, 200);
            this.AddTBString(EntityNoMyFileAttr.MyFileExt, null, "MyFileExt", false, false, 0, 10, 10);
            this.AddTBString(EntityNoMyFileAttr.WebPath, null, "WebPath", false, false, 0, 200, 10);
            this.AddTBInt(EntityNoMyFileAttr.MyFileH, 0, "MyFileH", false, false);
            this.AddTBInt(EntityNoMyFileAttr.MyFileW, 0, "MyFileW", false, false);
            this.AddTBFloat("MyFileSize", 0, "MyFileSize", false, false);
            this.IsHaveFJ = true;
        }
        private AttrFiles _HisAttrFiles = null;
        public AttrFiles HisAttrFiles
        {
            get
            {
                if (_HisAttrFiles == null)
                    _HisAttrFiles = new AttrFiles();
                return _HisAttrFiles;
            }
        }
        /// <summary>
        ///  Add a specific accessory , You can use it to increase the number of ?
        ///  Such as : Increase Resume , Increase Papers .
        /// </summary>
        /// <param name="fileDesc"></param>
        /// <param name="fExt"></param>
        public void AddMyFile(string fileDesc, string fExt)
        {
            HisAttrFiles.Add(fExt, fileDesc);
            this.IsHaveFJ = true;
        }

        #region  Increasing the chunk of text input 
        public void AddTBStringDoc()
        {
            AddTBStringDoc("Doc", "Doc", null, " Content ", true, false, 0, 4000, 300, 300,true);
        }
        public void AddTBStringDoc(string key, string defaultVal, string desc, bool uiVisable, bool isReadonly,bool isUILine)
        {
            AddTBStringDoc(key, key, defaultVal, desc, uiVisable, isReadonly, 0, 4000, 300, 300, isUILine);
        }
        public void AddTBStringDoc(string key, string defaultVal, string desc, bool uiVisable, bool isReadonly)
        {
            AddTBStringDoc(key, key, defaultVal, desc, uiVisable, isReadonly, 0, 4000, 300, 300, false);
        }
        public void AddTBStringDoc(string key, string defaultVal, string desc, bool uiVisable, bool isReadonly, int minLength, int maxLength, int tbWith, int rows)
        {
            AddTBStringDoc(key, key, defaultVal, desc, uiVisable, isReadonly, minLength, maxLength, tbWith, rows,false);
        }
        public void AddTBStringDoc(string key, string field, string defaultVal, string desc, bool uiVisable, bool isReadonly, int minLength, int maxLength, int tbWith, int rows, bool isUILine)
        {
            Attr attr = new Attr();
            attr.Key = key;
            attr.Field = field;
            attr.DefaultVal = defaultVal;
            attr.MyDataType = DataType.AppString;
            attr.Desc = desc;
            attr.UITBShowType = TBType.TB;
            attr.UIVisible = uiVisable;
            attr.UIWidth = 300;
            attr.UIIsReadonly = isReadonly;
            attr.MaxLength = 4000;
            attr.MinLength = minLength;
            attr.MyFieldType = FieldType.Normal;
            attr.UIHeight = rows;
            attr.UIIsLine = isUILine;
            this.Attrs.Add(attr);
        }
        #endregion

        #region  PK
        public void AddTBStringPK(string key, string defaultVal, string desc, bool uiVisable, bool isReadonly, int minLength, int maxLength, int tbWith)
        {
            this.PKs = key;
            AddTBString(key, key, defaultVal, FieldType.PK, TBType.TB, desc, uiVisable, isReadonly, minLength, maxLength, tbWith,false);
        }
        public void AddTBStringPK(string key, string field, object defaultVal, string desc, bool uiVisable, bool isReadonly, int minLength, int maxLength, int tbWith)
        {
            this.PKs = key;
            AddTBString(key, field, defaultVal, FieldType.PK, TBType.TB, desc, uiVisable, isReadonly, minLength, maxLength, tbWith,false);
        }
        #endregion

        #region PKNo

        #endregion

        #region   Foreign key to  Ens  Related operations .
        /// <summary>
        ///  Foreign key to  Ens  Related operations .
        /// </summary>
        /// <param name="key"> Property </param>
        /// <param name="field"> Field </param>
        /// <param name="defaultVal"> Defaults </param>
        /// <param name="desc"> Description </param>
        /// <param name="ens"> Entity </param>		 
        /// <param name="uiVisable"> Is not visible </param>
        /// <param name="isReadonly"> Is not read-only </param>
        /// <param name="minLength"> Minimum length </param>
        /// <param name="maxLength"> The maximum length </param>
        /// <param name="tbWith"> Width </param>
        public void AddTBStringFKEns(string key, string field, string defaultVal, string desc, Entities ens, string refKey, string refText, bool uiVisable, bool isReadonly, int minLength, int maxLength, int tbWith)
        {
            Attr attr = new Attr();
            attr.Key = key;

            attr.Field = field;
            attr.DefaultVal = defaultVal;
            attr.MyDataType = DataType.AppString;
            attr.UIBindKey = ens.ToString();
            attr.HisFKEns = ens;
            // attr.UIBindKeyOfEn = ens.GetNewEntity.ToString();

            attr.Desc = desc;
            attr.UITBShowType = TBType.Ens;
            attr.UIVisible = uiVisable;
            attr.UIWidth = tbWith;
            attr.UIIsReadonly = isReadonly;
            attr.MaxLength = maxLength;
            attr.MinLength = minLength;
            attr.UIRefKeyValue = refKey;
            attr.UIRefKeyText = refText;
            attr.MyFieldType = FieldType.FK;
            this.Attrs.Add(attr);
        }
        /// <summary>
        ///  Foreign key to  Ens  Related operations .
        /// </summary>
        /// <param name="key"> Property </param>
        /// <param name="defaultVal"> Defaults </param>
        /// <param name="desc"> Description </param>
        /// <param name="ens"> Entity </param>		 
        /// <param name="uiVisable"> Is not visible </param>
        /// <param name="isReadonly"> Is not read-only </param>
        /// <param name="minLength"> Minimum length </param>
        /// <param name="maxLength"> The maximum length </param>
        /// <param name="tbWith"> Width </param>
        public void AddTBStringFKEns(string key, string defaultVal, string desc, Entities ens, string refKey, string refText, bool uiVisable, bool isReadonly, int minLength, int maxLength, int tbWith)
        {
            this.AddTBStringFKEns(key, key, defaultVal, desc, ens, refKey, refText, uiVisable, isReadonly, minLength, maxLength, tbWith);
        }
        #endregion

        #region  An operation in a multi-valued relationship 
        /// <summary>
        ///  An operation in a multi-valued relationship 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="defaultVal"></param>
        /// <param name="desc"></param>
        /// <param name="ens"></param>
        /// <param name="uiVisable"></param>
        /// <param name="isReadonly"></param>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <param name="tbWith"></param>
        public void AddTBMultiValues(string key, string field, object defaultVal, string desc, Entities ens, string refValue, string refText, bool uiVisable, bool isReadonly, int minLength, int maxLength, int tbWith)
        {
            Attr attr = new Attr();
            attr.Key = key;
            attr.Field = field;
            attr.DefaultVal = defaultVal;
            attr.MyDataType = DataType.AppString;
            attr.UIBindKey = ens.ToString();
            attr.HisFKEns = ens;

            // attr.UIBindKeyOfEn = ens.GetNewEntity.ToString();

            attr.Desc = desc;
            attr.UITBShowType = TBType.Ens;
            attr.UIVisible = uiVisable;
            attr.UIWidth = tbWith;
            attr.UIIsReadonly = isReadonly;
            attr.UIRefKeyText = refText;
            attr.UIRefKeyValue = refValue;
            attr.MaxLength = maxLength;
            attr.MinLength = minLength;
            attr.MyFieldType = FieldType.MultiValues;

            this.Attrs.Add(attr);
        }
        #endregion

        #region   Primary key in  Ens  Related operations .
        /// <summary>
        ///  Foreign key to  Ens  Related operations .
        ///  Primary key 
        /// </summary>
        /// <param name="key"> Property </param>
        /// <param name="field"> Field </param>
        /// <param name="defaultVal"> Defaults </param>
        /// <param name="desc"> Description </param>
        /// <param name="ens"> Entity </param>		 
        /// <param name="uiVisable"> Is not visible </param>
        /// <param name="isReadonly"> Is not read-only </param>
        /// <param name="minLength"> Minimum length </param>
        /// <param name="maxLength"> The maximum length </param>
        /// <param name="tbWith"> Width </param>
        public void AddTBStringPKEns(string key, string field, object defaultVal, string desc, Entities ens, string refVal, string refText, bool uiVisable, bool isReadonly, int minLength, int maxLength, int tbWith)
        {
            Attr attr = new Attr();
            attr.Key = key;
            attr.Field = field;
            attr.DefaultVal = defaultVal;
            attr.MyDataType = DataType.AppString;
            attr.UIBindKey = ens.ToString();
            attr.HisFKEns = attr.HisFKEns;
            //attr.UIBindKeyOfEn = ens.GetNewEntity.ToString();
            attr.Desc = desc;
            attr.UITBShowType = TBType.Ens;
            attr.UIVisible = uiVisable;
            attr.UIWidth = tbWith;
            attr.UIIsReadonly = isReadonly;

            attr.UIRefKeyText = refText;
            attr.UIRefKeyValue = refVal;

            attr.MaxLength = maxLength;
            attr.MinLength = minLength;
            attr.MyFieldType = FieldType.PKFK;
            this.Attrs.Add(attr);
        }
        /// <summary>
        ///  Foreign key to  Ens  Related operations .
        /// </summary>
        /// <param name="key"> Property </param>
        /// <param name="defaultVal"> Defaults </param>
        /// <param name="desc"> Description </param>
        /// <param name="ens"> Entity </param>		 
        /// <param name="uiVisable"> Is not visible </param>
        /// <param name="isReadonly"> Is not read-only </param>
        /// <param name="minLength"> Minimum length </param>
        /// <param name="maxLength"> The maximum length </param>
        /// <param name="tbWith"> Width </param>
        public void AddTBStringPKEns(string key, string defaultVal, string desc, Entities ens, string refKey, string refText, bool uiVisable, bool isReadonly, int minLength, int maxLength, int tbWith)
        {
            this.AddTBStringPKEns(key, key, defaultVal, desc, ens, refKey, refText, uiVisable, isReadonly, minLength, maxLength, tbWith);
        }
        #endregion

        #region   Primary key in  DataHelpKey  Related operations .
        /// <summary>
        ///  Foreign key to  DataHelpKey  Related operations ,  The right to use and customize the help system .
        /// </summary>
        /// <param name="key"> Property </param>
        /// <param name="field"> Field </param>
        /// <param name="defaultVal"> Defaults </param>
        /// <param name="desc"> Description </param>
        /// <param name="DataHelpKey"> 在TB  Defined in the right health help Key </param></param>		 
        /// <param name="uiVisable"> Is not visible </param>
        /// <param name="isReadonly"> Is not read-only </param>
        /// <param name="minLength"> Minimum length </param>
        /// <param name="maxLength"> The maximum length </param>
        /// <param name="tbWith"> Width </param>
        public void AddTBStringPKSelf(string key, string field, object defaultVal, string desc, string DataHelpKey, bool uiVisable, bool isReadonly, int minLength, int maxLength, int tbWith)
        {
            Attr attr = new Attr();
            attr.Key = key;
            attr.Field = field;
            attr.DefaultVal = defaultVal;
            attr.MyDataType = DataType.AppString;
            attr.UIBindKey = DataHelpKey;
            attr.Desc = desc;
            attr.UITBShowType = TBType.Self;
            attr.UIVisible = uiVisable;
            attr.UIWidth = tbWith;
            attr.UIIsReadonly = isReadonly;
            attr.MaxLength = maxLength;
            attr.MinLength = minLength;
            attr.MyFieldType = FieldType.PK;
            this.Attrs.Add(attr);
        }
        /// <summary>
        ///  Foreign key to  Ens  Related operations . The right to use and customize the help system .
        /// </summary>
        /// <param name="key"> Property </param>
        /// <param name="defaultVal"> Defaults </param>
        /// <param name="desc"> Description </param>
        /// <param name="DataHelpKey"> 在TB  Defined in the right health help Key </param></param>
        /// <param name="uiVisable"> Is not visible </param>
        /// <param name="isReadonly"> Is not read-only </param>
        /// <param name="minLength"> Minimum length </param>
        /// <param name="maxLength"> The maximum length </param>
        /// <param name="tbWith"> Width </param>
        public void AddTBStringPKSelf(string key, object defaultVal, string desc, string DataHelpKey, bool uiVisable, bool isReadonly, int minLength, int maxLength, int tbWith)
        {
            this.AddTBStringPKSelf(key, key, defaultVal, desc, DataHelpKey, uiVisable, isReadonly, minLength, maxLength, tbWith);
        }
        #endregion

        #region   Foreign key to  DataHelpKey  Related operations .
        /// <summary>
        ///  Foreign key to  DataHelpKey  Related operations . The right to use and customize the help system .
        /// </summary>
        /// <param name="key"> Property </param>
        /// <param name="field"> Field </param>
        /// <param name="defaultVal"> Defaults </param>
        /// <param name="desc"> Description </param>
        /// <param name="DataHelpKey"> 在TB  Defined in the right health help Key </param></param>		 
        /// <param name="uiVisable"> Is not visible </param>
        /// <param name="isReadonly"> Is not read-only </param>
        /// <param name="minLength"> Minimum length </param>
        /// <param name="maxLength"> The maximum length </param>
        /// <param name="tbWith"> Width </param>
        public void AddTBStringFKSelf(string key, string field, object defaultVal, string desc, string DataHelpKey, bool uiVisable, bool isReadonly, int minLength, int maxLength, int tbWith)
        {
            Attr attr = new Attr();
            attr.Key = key;
            attr.Field = field;
            attr.DefaultVal = defaultVal;
            attr.MyDataType = DataType.AppString;
            attr.UIBindKey = DataHelpKey;
            attr.Desc = desc;
            attr.UITBShowType = TBType.Self;
            attr.UIVisible = uiVisable;
            attr.UIWidth = tbWith;
            attr.UIIsReadonly = isReadonly;
            attr.MaxLength = maxLength;
            attr.MinLength = minLength;
            attr.MyFieldType = FieldType.Normal;
            this.Attrs.Add(attr);
        }
        /// <summary>
        ///  Foreign key to  Ens  Related operations . Use and  Ens  Right Help System .
        /// </summary>
        /// <param name="key"> Property </param>
        /// <param name="defaultVal"> Defaults </param>
        /// <param name="desc"> Description </param>
        /// <param name="DataHelpKey"> 在TB  Defined in the right health help Key </param></param>
        /// <param name="uiVisable"> Is not visible </param>
        /// <param name="isReadonly"> Is not read-only </param>
        /// <param name="minLength"> Minimum length </param>
        /// <param name="maxLength"> The maximum length </param>
        /// <param name="tbWith"> Width </param>
        public void AddTBStringFKSelf(string key, object defaultVal, string desc, string DataHelpKey, bool uiVisable, bool isReadonly, int minLength, int maxLength, int tbWith)
        {
            this.AddTBStringFKSelf(key, key, defaultVal, desc, DataHelpKey, uiVisable, isReadonly, minLength, maxLength, tbWith);
        }
        #endregion

        #region   Increased foreign key plant 
        public void AddTBStringFKValue(string refKey, string key, string desc, bool IsVisable, int with)
        {

        }

        #endregion

        #endregion

        #endregion

        #region  Date Type 
        public void AddTBDate(string key)
        {
            switch (key)
            {
                case "RDT":
                    AddTBDate("RDT", " Record Date ", true, true);
                    break;
                case "UDT":
                    AddTBDate("UDT", " Updated ", true, true);
                    break;
                default:
                    AddTBDate(key, key, true, true);
                    break;
            }
        }
        /// <summary>
        ///  Date Type of control healthy increase 
        /// </summary>
        /// <param name="key"> Health value </param>
        /// <param name="defaultVal"> Defaults </param>
        /// <param name="desc"> Description </param>
        /// <param name="uiVisable"> Is not visible </param>
        /// <param name="isReadonly"> Is not read-only </param>
        public void AddTBDate(string key, string field, string defaultVal, string desc, bool uiVisable, bool isReadonly)
        {
            Attr attr = new Attr();
            attr.Key = key;
            attr.Field = field;
            attr.DefaultVal = defaultVal;
            attr.MyDataType = DataType.AppDate;
            attr.Desc = desc;
            attr.UITBShowType = TBType.Date;
            attr.UIVisible = uiVisable;
            attr.UIIsReadonly = isReadonly;
            attr.MaxLength = 50;
            this.Attrs.Add(attr);
        }
        /// <summary>
        ///  Date Type of control healthy increase 
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="defaultVal">defaultVal/ If you want to use the day information , Please choose the latter method is added </param>
        /// <param name="desc">desc</param>
        /// <param name="uiVisable">uiVisable</param>
        /// <param name="isReadonly">isReadonly</param>
        public void AddTBDate(string key, string defaultVal, string desc, bool uiVisable, bool isReadonly)
        {
            AddTBDate(key, key, defaultVal, desc, uiVisable, isReadonly);
        }
        /// <summary>
        ///  Date Type of control healthy increase ( The default date is the current date )
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="desc">desc</param>
        /// <param name="uiVisable">uiVisable</param>
        /// <param name="isReadonly">isReadonly</param>
        public void AddTBDate(string key, string desc, bool uiVisable, bool isReadonly)
        {
            AddTBDate(key, key, DateTime.Now.ToString(DataType.SysDataFormat), desc, uiVisable, isReadonly);
        }
        #endregion

        #region  Datetime type .
        /// <summary>
        ///  Date Type of control healthy increase 
        /// </summary>
        /// <param name="key"> Health value </param>
        /// <param name="defaultVal"> Defaults </param>
        /// <param name="desc"> Description </param>
        /// <param name="uiVisable"> Is not visible </param>
        /// <param name="isReadonly"> Is not read-only </param>
        public void AddTBDateTime(string key, string field, string defaultVal, string desc, bool uiVisable, bool isReadonly)
        {
            Attr attr = new Attr();
            attr.Key = key;
            attr.Field = field;
            attr.DefaultVal = defaultVal;
            attr.MyDataType = DataType.AppDateTime;
            attr.Desc = desc;
            attr.UITBShowType = TBType.DateTime;
            attr.UIVisible = uiVisable;
            attr.UIIsReadonly = isReadonly;
            attr.MaxLength = 50;
            attr.UIWidth = 100;
            this.Attrs.Add(attr);
        }
        public void AddTBDateTime(string key, string defaultVal, string desc, bool uiVisable, bool isReadonly)
        {
            this.AddTBDateTime(key, key, defaultVal, desc, uiVisable, isReadonly);
        }
        public void AddTBDateTime(string key, string desc, bool uiVisable, bool isReadonly)
        {
            this.AddTBDateTime(key, key, DateTime.Now.ToString(DataType.SysDataTimeFormat), desc, uiVisable, isReadonly);
        }
        #endregion

        #region  Types of funds 
        public void AddTBMoney(string key, string field, float defaultVal, string desc, bool uiVisable, bool isReadonly)
        {
            Attr attr = new Attr();
            attr.Key = key;
            attr.Field = field;
            attr.DefaultVal = defaultVal;
            attr.MyDataType = DataType.AppMoney;
            attr.Desc = desc;
            attr.UITBShowType = TBType.Moneny;
            attr.UIVisible = uiVisable;
            attr.UIIsReadonly = isReadonly;
            this.Attrs.Add(attr);
        }
        public void AddTBMoney(string key, float defaultVal, string desc, bool uiVisable, bool isReadonly)
        {
            this.AddTBMoney(key, key, defaultVal, desc, uiVisable, isReadonly);
        }
        #endregion

        #region Int Type 
        /// <summary>
        ///  Adding a common type .
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="_Field"> Field </param>
        /// <param name="defaultVal"> Defaults </param>
        /// <param name="desc"> Description </param>
        /// <param name="uiVisable"> Is not visible </param>
        /// <param name="isReadonly"> Is not read-only </param>
        public void AddTBInt(string key, string _Field, int defaultVal, string desc, bool uiVisable, bool isReadonly)
        {
            Attr attr = new Attr();
            attr.Key = key;
            attr.Field = _Field;
            attr.DefaultVal = defaultVal;
            attr.MyDataType = DataType.AppInt;
            attr.MyFieldType = FieldType.Normal;
            attr.Desc = desc;
            attr.UITBShowType = TBType.Int;
            attr.UIVisible = uiVisable;
            attr.UIIsReadonly = isReadonly;
            this.Attrs.Add(attr);
        }
        /// <summary>
        ///  Adding a common type . Field values and attributes the same .
        /// </summary>
        /// <param name="key">键</param>		 
        /// <param name="defaultVal"> Defaults </param>
        /// <param name="desc"> Description </param>
        /// <param name="uiVisable"> Is not visible </param>
        /// <param name="isReadonly"> Is not read-only </param>
        public void AddTBInt(string key, int defaultVal, string desc, bool uiVisable, bool isReadonly)
        {
            this.AddTBInt(key, key, defaultVal, desc, uiVisable, isReadonly);
        }
        /// <summary>
        ///  Adding a PK Types .
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="_Field"> Field </param>
        /// <param name="defaultVal"> Defaults </param>
        /// <param name="desc"> Description </param>
        /// <param name="uiVisable"> Is not visible </param>
        /// <param name="isReadonly"> Is not read-only </param>
        public void AddTBIntPK(string key, string _Field, int defaultVal, string desc, bool uiVisable, bool isReadonly, bool identityKey)
        {
            this.PKs = key;
            Attr attr = new Attr();
            attr.Key = key;
            attr.Field = _Field;
            attr.DefaultVal = defaultVal;
            attr.MyDataType = DataType.AppInt;
            attr.MyFieldType = FieldType.PK;
            attr.Desc = desc;
            attr.UITBShowType = TBType.Int;
            attr.UIVisible = uiVisable;
            attr.UIIsReadonly = isReadonly;
            if (identityKey)
                attr.UIBindKey = "1"; // Mark this special value , Column since it can automatically generate growth .
            this.Attrs.Add(attr);
        }
        /// <summary>
        ///  Adding a PK Types . Field values and attributes the same .
        /// </summary>
        /// <param name="key">键</param>		 
        /// <param name="defaultVal"> Defaults </param>
        /// <param name="desc"> Description </param>
        /// <param name="uiVisable"> Is not visible </param>
        /// <param name="isReadonly"> Is not read-only </param>
        public void AddTBIntPKOID(string _field, string desc)
        {
            this.AddTBIntPK("OID", _field, 0, "OID", false, true,false);
        }
        /// <summary>
        ///  Adding a MID
        /// </summary>
        public void AddTBMID()
        {
            Attr attr = new Attr();
            attr.Key = "MID";
            attr.Field = "MID";
            attr.DefaultVal = 0;
            attr.MyDataType = DataType.AppInt;
            attr.MyFieldType = FieldType.Normal;
            attr.Desc = "MID";
            attr.UITBShowType = TBType.Int;
            attr.UIVisible = false;
            attr.UIIsReadonly = true;
            this.Attrs.Add(attr);
        }
        public void AddTBIntPKOID()
        {
            this.AddTBIntPKOID("OID", "OID");
        }
        public void AddTBMyNum(string desc)
        {
            this.AddTBInt("MyNum", 1, desc, true, true);
        }
        public void AddTBMyNum()
        {
            this.AddTBInt("MyNum", 1, " The number of ", true, true);
        }
        /// <summary>
        ///  Increase   AtParas Field .
        /// </summary>
        /// <param name="fieldLength"></param>
        public void AddTBAtParas(int fieldLength)
        {
            this.AddTBString("AtPara", null, "AtPara", false, true, 0, fieldLength, 10);
        }
        /// <summary>
        ///  Primary key 
        /// </summary>
        public void AddMyPK()
        {
            this.PKs = "MyPK";
            this.AddTBStringPK("MyPK", null, "MyPK", true, true, 1, 100, 10);

            //Attr attr = new Attr();
            //attr.Key = "MyPK";
            //attr.Field = "MyPK";
            //attr.DefaultVal = null;
            //attr.MyDataType = DataType.AppString;
            //attr.MyFieldType = FieldType.PK;
            //attr.Desc = "MyPK";
            //attr.UITBShowType = TBType.TB;
            //attr.UIVisible = false;
            //attr.UIIsReadonly = true;
            //attr.MinLength = 1;
            //attr.MaxLength = 100;
            //this.Attrs.Add(attr);
        }
        public void AddMyPKNoVisable()
        {
            this.AddTBStringPK("MyPK", null, "MyPK", false, false, 1, 100, 10);
        }
        /// <summary>
        ///  Automatically increase the growth column 
        /// </summary>
        public void AddAID()
        {
            Attr attr = new Attr();
            attr.Key = "AID";
            attr.Field = "AID";
            attr.DefaultVal = null;
            attr.MyDataType = DataType.AppInt;
            attr.MyFieldType = FieldType.PK;
            attr.Desc = "AID";
            attr.UITBShowType = TBType.TB;
            attr.UIVisible = false;
            attr.UIIsReadonly = true;
            this.Attrs.Add(attr);
        }
        /// <summary>
        ///  Adding a PK Types . Field values and attributes the same .
        /// </summary>
        /// <param name="key">键</param>		 
        /// <param name="defaultVal"> Defaults </param>
        /// <param name="desc"> Description </param>
        /// <param name="uiVisable"> Is not visible </param>
        /// <param name="isReadonly"> Is not read-only </param>
        public void AddTBIntPK(string key, int defaultVal, string desc, bool uiVisable, bool isReadonly)
        {
            this.AddTBIntPK(key, key, defaultVal, desc, uiVisable, isReadonly,false);
        }

        public void AddTBIntPK(string key, int defaultVal, string desc, bool uiVisable, bool isReadonly, bool identityKey)
        {
            this.AddTBIntPK(key, key, defaultVal, desc, uiVisable, isReadonly, identityKey);
        }
        public void AddTBIntMyNum()
        {
            this.AddTBInt("MyNum", "MyNum", 1, " The number of ", true, true);
        }
        #endregion

        #region Float Type 
        public void AddTBFloat(string key, string _Field, float defaultVal, string desc, bool uiVisable, bool isReadonly)
        {
            Attr attr = new Attr();
            attr.Key = key;
            attr.Field = _Field;
            attr.DefaultVal = defaultVal;
            attr.MyDataType = DataType.AppFloat;
            attr.Desc = desc;
            attr.UITBShowType = TBType.Num;
            attr.UIVisible = uiVisable;
            attr.UIIsReadonly = isReadonly;
            this.Attrs.Add(attr);
        }
        public void AddTBFloat(string key, float defaultVal, string desc, bool uiVisable, bool isReadonly)
        {
            this.AddTBFloat(key, key, defaultVal, desc, uiVisable, isReadonly);
        }
        #endregion

        #region Decimal Type 
        public void AddTBDecimal(string key, string _Field, decimal defaultVal, string desc, bool uiVisable, bool isReadonly)
        {
            Attr attr = new Attr();
            attr.Key = key;
            attr.Field = _Field;
            attr.DefaultVal = defaultVal;
            attr.MyDataType = DataType.AppDouble;
            attr.Desc = desc;
            attr.UITBShowType = TBType.Decimal;
            attr.UIVisible = uiVisable;
            attr.UIIsReadonly = isReadonly;
            this.Attrs.Add(attr);
        }
        public void AddTBDecimal(string key, decimal defaultVal, string desc, bool uiVisable, bool isReadonly)
        {
            this.AddTBDecimal(key, key, defaultVal, desc, uiVisable, isReadonly);
        }
        #endregion
        #endregion

        #endregion
    }
}
