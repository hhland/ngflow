using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.XML;

namespace BP.Sys.XML
{
    /// <summary>
    ///  Data Types 
    /// </summary>
	public class SysDataType:XmlEn
	{
		#region  Property 
        public string No
        {
            get
            {
                return this.GetValStringByKey("No");
            }
        }
        public string Name
        {
            get
            {
                return this.GetValStringByKey(BP.Web.WebUser.SysLang);
            }
        }
		/// <summary>
		///  Picture 
		/// </summary>
		public string Desc
		{
			get
			{
                return this.GetValStringByKey("Desc");
			}
		}
		#endregion

		#region  Structure 
		/// <summary>
		///  Node extended information 
		/// </summary>
		public SysDataType()
		{
		}
		/// <summary>
		///  Get an instance of 
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
				return new SysDataTypes();
			}
		}
		#endregion
	}
	/// <summary>
	/// 
	/// </summary>
	public class SysDataTypes:XmlEns
	{
		#region  Structure 
		/// <summary>
		///  Assessment rate data elements 
		/// </summary>
        public SysDataTypes() { }
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new SysDataType();
			}
		}
        public override string File
        {
            get
            {
              //  return SystemConfig.PathOfWebApp + "\\WF\\MapDef\\Style\\SysDataType.xml";
                return SystemConfig.PathOfData + "\\XML\\SysDataType.xml";
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
				return null; //new BP.ZF1.AdminTools();
			}
		}
		#endregion
		 
	}
}
