using System;
using System.Collections;
using BP.DA;
using BP.Sys;
using BP.En;
using BP.XML;

namespace BP.WF.XML
{
    /// <summary>
    ///  Data Source Type 
    /// </summary>
    public class DBSrcAttr
    {
        /// <summary>
        ///  Serial number 
        /// </summary>
        public const string No = "No";
        /// <summary>
        ///  Name 
        /// </summary>
        public const string Name = "Name";
        /// <summary>
        ///  Data Source Type 
        /// </summary>
        public const string SrcType = "SrcType";
        /// <summary>
        ///  Data Sources url
        /// </summary>
        public const string Url = "Url";
    }
    /// <summary>
    ///  Data Source Type 
    /// </summary>
	public class DBSrc:XmlEnNoName
    {
        #region  Property 
        /// <summary>
        ///  Data Source Type 
        /// </summary>
        public string SrcType
        {
            get
            {
                return this.GetValStringByKey(DBSrcAttr.SrcType);
            }
        }
        /// <summary>
        ///  Data Source Type URL
        /// </summary>
        public string Url
        {
            get
            {
                return this.GetValStringByKey(DBSrcAttr.Url);
            }
        }
        #endregion

        #region  Structure 
        /// <summary>
        ///  Data Source Type 
		/// </summary>
		public DBSrc()
		{
		}
		/// <summary>
		///  Data Source Type s
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
				return new DBSrcs();
			}
		}
		#endregion
	}
	/// <summary>
    ///  Data Source Type s
	/// </summary>
	public class DBSrcs:XmlEns
	{
		#region  Structure 
		/// <summary>
		///  Assessment rate data elements 
		/// </summary>
        public DBSrcs() { }
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new DBSrc();
			}
		}
        /// <summary>
        /// XML File Locations .
        /// </summary>
		public override string File
		{
			get
			{
                return SystemConfig.PathOfWebApp + "\\DataUser\\XML\\DBSrc.xml";
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
