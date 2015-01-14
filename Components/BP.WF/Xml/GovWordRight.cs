using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.Sys;
using BP.XML;

namespace BP.WF.XML
{
    /// <summary>
    ///  Documents on the right predicate 
    /// </summary>
    public class GovWordRightAttr
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
    ///  Documents on the right predicate 
    /// </summary>
	public class GovWordRight:XmlEnNoName
    {
        #region  Structure 
        /// <summary>
        ///  Documents on the right predicate 
		/// </summary>
		public GovWordRight()
		{
		}
		/// <summary>
		///  Documents on the right predicate s
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
				return new GovWordRights();
			}
		}
		#endregion
	}
	/// <summary>
    ///  Documents on the right predicate s
	/// </summary>
	public class GovWordRights:XmlEns
	{
		#region  Structure 
		/// <summary>
		///  Assessment rate data elements 
		/// </summary>
        public GovWordRights() { }
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new GovWordRight();
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
                return "GovWordRight";
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
