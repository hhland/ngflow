using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.Sys;
using BP.XML;

namespace BP.WF.XML
{
    /// <summary>
    ///  Documents left predicate 
    /// </summary>
    public class GovWordLeftAttr
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
    ///  Documents left predicate 
    /// </summary>
	public class GovWordLeft:XmlEnNoName
    {

        #region  Structure 
        /// <summary>
        ///  Documents left predicate 
		/// </summary>
		public GovWordLeft()
		{
		}
		/// <summary>
		///  Documents left predicate s
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
				return new GovWordLefts();
			}
		}
		#endregion
	}
	/// <summary>
    ///  Documents left predicate s
	/// </summary>
	public class GovWordLefts:XmlEns
	{
		#region  Structure 
		/// <summary>
		///  Assessment rate data elements 
		/// </summary>
        public GovWordLefts() { }
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new GovWordLeft();
			}
		}
        /// <summary>
        /// XML File Locations .
        /// </summary>
		public override string File
		{
			get
			{
                return SystemConfig.PathOfWebApp + "\\WF\\Data\\XML\\XmlDB.xml";
			}
		}
		/// <summary>
		///  Physical table name 
		/// </summary>
		public override string TableName
		{
			get
			{
                return "GovWordLeft";
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
