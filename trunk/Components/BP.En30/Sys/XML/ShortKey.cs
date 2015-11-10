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
	public class ShortKeyAttr
	{
		/// <summary>
		///  Wrongdoing 
		/// </summary>
		public const string No="No";
		/// <summary>
		/// Name
		/// </summary>
		public const string Name="Name";
		/// <summary>
		///  Expression 
		/// </summary>
		public const string URL="URL";
		/// <summary>
		///  Description 
		/// </summary>
		public const string DFor="DFor";
		/// <summary>
		///  Picture 
		/// </summary>
		public const string Img="Img";
        /// <summary>
        /// Target
        /// </summary>
        public const string Target = "Target";
	}
	/// <summary>
	/// ShortKey  The summary .
	///  Examination of the data elements wrongdoing 
	/// 1, It is  ShortKey  One detail .
	/// 2, It represents a data element .
	/// </summary>
	public class ShortKey:XmlEn
	{
		#region  Property 
		public string No
		{
			get
			{
				return this.GetValStringByKey(ShortKeyAttr.No);
			}
		}
		/// <summary>
		///  Data 
		/// </summary>
		public string DFor
		{
			get
			{
				return this.GetValStringByKey(ShortKeyAttr.DFor);
			}
		}
		public string Name
		{
			get
			{
                return this.GetValStringByKey(BP.Web.WebUser.SysLang );
			}
		}
		/// <summary>
		/// URL
		/// </summary>
		public string URL
		{
			get
			{
				return this.GetValStringByKey(ShortKeyAttr.URL);
			}
		}
		/// <summary>
		///  Picture 
		/// </summary>
		public string Img
		{
			get
			{
				return this.GetValStringByKey(ShortKeyAttr.Img);
			}
		}
        public string Target
        {
            get
            {
                return this.GetValStringByKey(ShortKeyAttr.Target);
            }
        }
		#endregion

		#region  Structure 
		public ShortKey()
		{
		}
		/// <summary>
		///  Get an instance of 
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
				return new ShortKeys();
			}
		}
		#endregion
	}
	/// <summary>
	/// 
	/// </summary>
	public class ShortKeys:XmlEns
	{
		#region  Structure 
		/// <summary>
		///  Examination of the data elements wrongdoing 
		/// </summary>
		public ShortKeys(){}
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new ShortKey();
			}
		}
		public override string File
		{
			get
			{
				return SystemConfig.PathOfXML+"\\Menu.xml";
			}
		}
		/// <summary>
		///  Physical table name 
		/// </summary>
		public override string TableName
		{
			get
			{
				return "ShortKey";
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
