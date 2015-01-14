using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.Sys;


namespace BP.XML
{
    /// <summary>
    ///  RegularExpression  Regular expression templates 
    /// </summary>
	public class RegularExpression:XmlEn
	{
		#region  Property 
        /// <summary>
        ///  Serial number 
        /// </summary>
        public string No
        {
            get
            {
                return this.GetValStringByKey("No");
            }
        }
        /// <summary>
        ///  Name 
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetValStringByKey("Name");
            }
        }
        public string Note
        {
            get
            {
                return this.GetValStringByKey("Note");
            }
        }
        public string ForCtrl
        {
            get
            {
                return this.GetValStringByKey("ForCtrl");
            }
        }
		#endregion

		#region  Structure 
		/// <summary>
		///  Node extended information 
		/// </summary>
        public RegularExpression()
		{
		}
		/// <summary>
		///  Get an instance of 
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
				return new RegularExpressions();
			}
		}
		#endregion
	}
	/// <summary>
	/// 
	/// </summary>
	public class RegularExpressions:XmlEns
	{
		#region  Structure 
		/// <summary>
		///  Assessment rate data elements 
		/// </summary>
        public RegularExpressions() { }
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new RegularExpression();
			}
		}
		public override string File
		{
			get
			{
                return SystemConfig.PathOfData + "\\XML\\RegularExpression.xml";
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
