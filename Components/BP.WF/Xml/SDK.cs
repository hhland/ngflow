using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.XML;
using BP.Sys;


namespace BP.WF.XML
{
    /// <summary>
    /// sdk
    /// </summary>
	public class SDK:XmlEn
	{
		#region  Property 
        public string No
        {
            get
            {
                return this.GetValStringByKey("DoWhat");
            }
        }
        public string Name
        {
            get
            {
                return this.GetValStringByKey(BP.Web.WebUser.SysLang);
            }
        }
        public string Url
        {
            get
            {
                return this.GetValStringByKey("Url");
            }
        }
		#endregion

		#region  Structure 
		/// <summary>
		///  Node extended information 
		/// </summary>
		public SDK()
		{
		}
		/// <summary>
		///  Get an instance of 
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
				return new SDKs();
			}
		}
		#endregion
	}
	/// <summary>
	/// 
	/// </summary>
	public class SDKs:XmlEns
	{
		#region  Structure 
		/// <summary>
		///  Assessment rate data elements 
		/// </summary>
        public SDKs() { }
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new SDK();
			}
		}
		public override string File
		{
			get
			{
                return SystemConfig.PathOfData + "\\XML\\SDK.xml";
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
