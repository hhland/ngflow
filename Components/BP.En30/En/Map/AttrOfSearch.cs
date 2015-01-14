using System;
using System.Collections;
using BP.En;

namespace BP.En
{
	public enum OperatorSymbol
	{
		/// <summary>
		///  Greater than 
		/// </summary>
		DaYu,
		/// <summary>
		///  Equal 
		/// </summary>
		DengYu,
		/// <summary>
		///  Less than 
		/// </summary>
		XiaoYu,
		/// <summary>
		///  Similar 
		/// </summary>
		Like,
	}
	/// <summary>
	///  Association Properties 
	/// </summary>
	public class AARef
	{
		/// <summary>
		///  Directory attributes 
		/// </summary>
		public string CataAttr=null;
		/// <summary>
		///  Associate key
		/// </summary>
		public string RefKey=null;
		/// <summary>
		///  Sub-properties 
		/// </summary>
		public string SubAttr=null;
		/// <summary>
		///  Association Properties 
		/// </summary>
		/// <param name="CataAttr"> Property </param>
		/// <param name="RefKey"></param>
		/// <param name="SubAttr"></param>
		public AARef(string cataAttr,string subAttr,string refKey)
		{
			this.CataAttr=cataAttr;
			this.SubAttr=subAttr;
			this.RefKey=refKey;

		}
	}
	public class AARefs : System.Collections.CollectionBase
	{
		#region  Structure 
		public AARefs()
		{
		}
		public AARefs this[int index]
		{
			get
			{
				return (AARefs)this.InnerList[index];
			}
		}
		#endregion
		 
		#region  Add a query attribute .
		/// <summary>
		///  Add a query attribute 
		/// </summary>
		/// <param name="lab"> Label </param>
		/// <param name="refKey"> Attributes of the entity </param>
		/// <param name="defaultvalue"> Defaults </param>
		public void Add(string lab,string key, string refKey,string defaultSymbol, string defaultvalue, int tbWidth)
		{
			AttrOfSearch aos= new AttrOfSearch(key,lab,refKey,defaultSymbol,defaultvalue,tbWidth,false);
			this.InnerList.Add(aos);
		}
		#endregion
	}

	/// <summary>
	/// SearchKey  The summary .
	///  To deal with a record store , Issue .
	/// </summary>
    public class AttrOfSearch
    {
        #region  Basic properties 
        /// <summary>
        ///  Whether to hide 
        /// </summary>
        private bool _IsHidden = false;
        /// <summary>
        ///  Whether to hide 
        /// </summary>
        public bool IsHidden
        {
            get
            {
                return _IsHidden;
            }
            set
            {
                _IsHidden = value;
            }
        }
        /// <summary>
        ///  Operation is available 
        /// </summary>
        private bool _SymbolEnable = true;
        /// <summary>
        ///  Operation is available 
        /// </summary>
        public bool SymbolEnable
        {
            get
            {
                return _SymbolEnable;
            }
            set
            {
                _SymbolEnable = value;
            }
        }

        /// <summary>
        ///  Label 
        /// </summary>
        private string _Lab = "";
        /// <summary>
        ///  Label 
        /// </summary>
        public string Lab
        {
            get
            {
                return _Lab;
            }
            set
            {
                _Lab = value;
            }
        }
        /// <summary>
        ///  Query defaults 
        /// </summary>
        private string _DefaultVal = "";
        /// <summary>
        /// OperatorKey
        /// </summary>
        public string DefaultVal
        {
            get
            {
                return _DefaultVal;
            }
            set
            {
                _DefaultVal = value;
            }
        }
        /// <summary>
        ///  Defaults 
        /// </summary>
        public string DefaultValRun
        {
            get
            {
                if (_DefaultVal == null)
                    return null;

                if (_DefaultVal.Contains("@"))
                {
                    if (_DefaultVal.Contains("@WebUser.No"))
                        return _DefaultVal.Replace("@WebUser.No", Web.WebUser.No);

                    if (_DefaultVal.Contains("@WebUser.Name"))
                        return _DefaultVal.Replace("@WebUser.Name", Web.WebUser.Name);

                    if (_DefaultVal.Contains("@WebUser.FK_Dept"))
                        return _DefaultVal.Replace("@WebUser.FK_Dept", Web.WebUser.FK_Dept);

                    if (_DefaultVal.Contains("@WebUser.FK_DeptName"))
                        return _DefaultVal.Replace("@WebUser.FK_DeptName", Web.WebUser.FK_DeptName);

                    if (_DefaultVal.Contains("@WebUser.FK_DeptNameOfFull"))
                        return _DefaultVal.Replace("@WebUser.FK_DeptNameOfFull", Web.WebUser.FK_DeptNameOfFull);

                    //if (_DefaultVal.Contains("@WebUser.FK_Unit"))
                    //    return _DefaultVal.Replace("@WebUser.FK_Unit", Web.WebUser.FK_Unit);

                }
                return _DefaultVal;
            }
        }
        /// <summary>
        ///  The default action symbol .
        /// </summary>
        private string _defaultSymbol = "=";
        /// <summary>
        ///  Operation Symbol 
        /// </summary>
        public string DefaultSymbol
        {
            get
            {
                return _defaultSymbol;
            }
            set
            {
                _defaultSymbol = value;
            }
        }
        /// <summary>
        ///  Corresponding attribute 
        /// </summary>
        private string _RefAttr = "";
        /// <summary>
        ///  Corresponding attribute 
        /// </summary>
        public string RefAttrKey
        {
            get
            {
                return _RefAttr;
            }
            set
            {
                _RefAttr = value;
            }
        }
        /// <summary>
        /// Key
        /// </summary>
        private string _Key = "";
        /// <summary>
        /// Key
        /// </summary>
        public string Key
        {
            get
            {
                return _Key;
            }
            set
            {
                _Key = value;
            }
        }
        /// <summary>
        /// TB  Width 
        /// </summary>
        private int _TBWidth = 10;
        /// <summary>
        /// TBWidth 
        /// </summary>
        public int TBWidth
        {
            get
            {
                return _TBWidth;
            }
            set
            {
                _TBWidth = value;
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Construct a general inquiry property 
        /// </summary>
        public AttrOfSearch(string key, string lab, string refAttr, string DefaultSymbol, string defaultValue, int tbwidth, bool isHidden)
        {
            this.Key = key;
            this.Lab = lab;
            this.RefAttrKey = refAttr;
            this.DefaultSymbol = DefaultSymbol;
            this.DefaultVal = defaultValue;
            this.TBWidth = tbwidth;
            this.IsHidden = isHidden;
        }
        #endregion
    }
	/// <summary>
	/// SearchKey  Set 
	/// </summary>
	public class AttrsOfSearch : System.Collections.CollectionBase
	{
		#region  Structure 
		public AttrsOfSearch()
		{
		}
		public AttrsOfSearch this[int index]
		{
			get
			{
				return (AttrsOfSearch)this.InnerList[index];
			}
		}
		#endregion
		 
		#region  Add a query attribute .
		/// <summary>
		///  Add a hidden attribute query 
		/// </summary>
		/// <param name="refKey"> Associate key</param>
		/// <param name="symbol"> Operation Symbol </param>
		/// <param name="val"> Operations val.</param>
		public void AddHidden(string refKey,string symbol, string val)
		{
			AttrOfSearch aos= new AttrOfSearch( "K"+this.InnerList.Count,refKey,refKey,symbol,val,0,true);
			this.InnerList.Add(aos);
		}
		/// <summary>
		///  Add a query attribute 
		/// </summary>
		/// <param name="lab"> Label </param>
		/// <param name="refKey"> Attributes of the entity </param>
		/// <param name="defaultvalue"> Defaults </param>
		public void Add(string lab, string refKey,string defaultSymbol, string defaultvalue, int tbWidth)
		{
			AttrOfSearch aos= new AttrOfSearch( "K"+this.InnerList.Count,lab,refKey,defaultSymbol,defaultvalue,tbWidth,false);
			this.InnerList.Add(aos);
		}
		public void Add( AttrOfSearch en)
		{
			this.InnerList.Add(en);
		}

		/// <summary>
		///  Increase 2 Attributes .
		/// </summary>
		/// <param name="lab"> Title </param>
		/// <param name="refKey"> Associated Key</param>
		/// <param name="defaultvalueOfFrom"> Defaults from </param>
		/// <param name="defaultvalueOfTo"> Defaults from </param>
		/// <param name="tbWidth"> Width </param>
		public void AddFromTo(string lab,string refKey,string defaultvalueOfFrom, string defaultvalueOfTo, int tbWidth)
		{
			AttrOfSearch aos= new AttrOfSearch( "Form_"+refKey,lab+"´Ó",refKey,">=", defaultvalueOfFrom,tbWidth,false);
			aos.SymbolEnable=false;
			this.InnerList.Add(aos);

			AttrOfSearch aos1= new AttrOfSearch( "To_"+refKey,"µ½",refKey,  "<=" , defaultvalueOfTo,tbWidth,false);
			aos1.SymbolEnable=false;
			this.InnerList.Add(aos1);

		}
		#endregion
	}
}
