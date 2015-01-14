using System;
using System.Collections;
using System.Data;
using BP.DA;
using BP.En;
using BP.XML;

namespace BP.Sys.Xml
{
	/// <summary>
	///  Property 
	/// </summary>
	public class SearchAttr
	{
		/// <summary>
		///  Wrongdoing 
		/// </summary>
		public const string Attr="Attr";
		/// <summary>
		///  Expression 
		/// </summary>
		public const string URL="URL";
		/// <summary>
		///  Description 
		/// </summary>
		public const string For="For";
	}
	/// <summary>
	/// Search  The summary .
	///  Examination of the data elements wrongdoing 
	/// 1, It is  Search  One detail .
	/// 2, It represents a data element .
	/// </summary>
	public class Search:XmlEn
	{

		#region  Property 
		public string Attr
		{
			get
			{
				return this.GetValStringByKey(SearchAttr.Attr);
			}
		}
		public string For
		{
			get
			{
				return this.GetValStringByKey(SearchAttr.For);
			}
		}
		public string URL
		{
			get
			{
				return this.GetValStringByKey(SearchAttr.URL);
			}
		}
		#endregion

		#region  Structure 
		public Search()
		{
		}
		/// <summary>
		///  Get an instance of 
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
				return new Searchs();
			}
		}
		#endregion
	}
	/// <summary>
	/// 
	/// </summary>
	public class Searchs:XmlEns
	{
		#region  Structure 
		/// <summary>
		///  Examination of the data elements wrongdoing 
		/// </summary>
		public Searchs(){}
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new Search();
			}
		}
		public override string File
		{
			get
			{
				return SystemConfig.PathOfXML+"\\Ens\\Search.xml";
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
