using System;
using System.Collections;
using BP.DA;
using BP.Sys;
using BP.En;

namespace BP.XML
{
    /// <summary>
    ///  RegularExpressionDtl  Regular expression templates 
    /// </summary>
	public class RegularExpressionDtl:XmlEn
	{
		#region  Property 
        /// <summary>
        ///  Serial number 
        /// </summary>
        public string ItemNo
        {
            get
            {
                return this.GetValStringByKey("ItemNo");
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
        public string Exp
        {
            get
            {
                return this.GetValStringByKey("Exp");
            }
        }
        public string ForEvent
        {
            get
            {
                return this.GetValStringByKey("ForEvent");
            }
        }
        public string Msg
        {
            get
            {
                return this.GetValStringByKey("Msg");
            }
        }
		#endregion

		#region  Structure 
		/// <summary>
		///  Node extended information 
		/// </summary>
        public RegularExpressionDtl()
		{
		}
		/// <summary>
		///  Get an instance of 
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
				return new RegularExpressionDtls();
			}
		}
		#endregion
	}
	/// <summary>
	/// 
	/// </summary>
	public class RegularExpressionDtls:XmlEns
	{
		#region  Structure 
		/// <summary>
		///  Assessment rate data elements 
		/// </summary>
        public RegularExpressionDtls() { }
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new RegularExpressionDtl();
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
				return "Dtl";
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
