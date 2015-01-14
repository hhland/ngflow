using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.XML;
using BP.Sys;

namespace BP.WF.XML
{
    public class LangAttr
    {
        /// <summary>
        ///  Serial number 
        /// </summary>
        public const string No = "No";
        /// <summary>
        ///  Name 
        /// </summary>
        public const string Name = "Name";
    }
    /// <summary>
    ///  Language 
    /// </summary>
	public class Lang:XmlEnNoName
	{
		#region  Structure 
		/// <summary>
		///  Node extended information 
		/// </summary>
		public Lang()
		{
		}
		/// <summary>
		///  Get an instance of 
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
				return new Langs();
			}
		}
		#endregion
	}
	/// <summary>
	///  Language s
	/// </summary>
	public class Langs:XmlEns
	{
		#region  Structure 
		/// <summary>
		///  Assessment rate data elements 
		/// </summary>
        public Langs() { }
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new Lang();
			}
		}
        /// <summary>
        /// XML File Locations .
        /// </summary>
		public override string File
		{
			get
			{
                return SystemConfig.CCFlowAppPath + "WF\\Style\\Tools.xml";
			}
		}
		/// <summary>
		///  Physical table name 
		/// </summary>
		public override string TableName
		{
			get
			{
				return "Lang";
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
