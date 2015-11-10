using System;
using System.Collections;
using System.Data;
using BP.DA;
using BP.En;
using BP.XML;


namespace BP.Sys.Xml
{
    public class GlobalKeyValList
    {
        /// <summary>
        ///  Series Properties s
        /// </summary>
        public const string Subjection = "Subjection";
    }
	/// <summary>
	///  Property 
	/// </summary>
	public class GlobalKeyValAttr
	{
		/// <summary>
		///  Height 
		/// </summary>
        public const string Key = "Key";
        /// <summary>
        ///  Whether to accept the file data 
        /// </summary>
        public const string Val = "Val";
	}
	/// <summary>
	/// GlobalKeyVal  The summary .
	///  Examination of the data elements wrongdoing 
	/// 1, It is  GlobalKeyVal  One detail .
	/// 2, It represents a data element .
	/// </summary>
	public class GlobalKeyVal:XmlEn
	{
		#region  Property 
        public string Key
		{
			get
			{
                return this.GetValStringByKey(GlobalKeyValAttr.Key);
			}
		}
        public string Val
		{
			get
			{
                return this.GetValStringByKey(GlobalKeyValAttr.Val);
			}
		}
		#endregion

		#region  Structure 
		public GlobalKeyVal()
		{
		}
		/// <summary>
		///  Get an instance of 
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
				return new GlobalKeyVals();
			}
		}
		#endregion
	}
	/// <summary>
	///  Global Key val  Variable types of settings 
	/// </summary>
	public class GlobalKeyVals:XmlEns
	{
		#region  Structure 
		/// <summary>
		///  Examination of the data elements wrongdoing 
		/// </summary>
		public GlobalKeyVals()
		{
		}
	 
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new GlobalKeyVal();
			}
		}
		public override string File
		{
			get
			{
				return SystemConfig.PathOfXML+"\\Ens\\GlobalKeyVal.xml";
			}
		}
		/// <summary>
		///  Physical table name 
		/// </summary>
		public override string TableName
		{
			get
			{
				return "Item";
			}
		}
		public override Entities RefEns
		{
			get
			{
				return null;
			}
		}
		#endregion
	}
}
