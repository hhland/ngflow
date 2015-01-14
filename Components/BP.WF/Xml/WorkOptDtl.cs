using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.XML;
using BP.Sys;

namespace BP.WF.XML
{
    /// <summary>
    ///  Work Detail option 
    /// </summary>
	public class WorkOptDtlXml:XmlEnNoName
	{
        public new string Name
        {
            get
            {
                return this.GetValStringByKey(BP.Web.WebUser.SysLang);
            }
        }
        public string URL
        {
            get
            {
                return this.GetValStringByKey("URL");
            }
        }

		#region  Structure 
		/// <summary>
		///  Node extended information 
		/// </summary>
		public WorkOptDtlXml()
		{
		}
		/// <summary>
		///  Get an instance of 
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
				return new WorkOptDtlXmls();
			}
		}
		#endregion
	}
	/// <summary>
    ///  Work Detail option s
	/// </summary>
	public class WorkOptDtlXmls:XmlEns
	{
		#region  Structure 
		/// <summary>
        ///  Work Detail option s
		/// </summary>
        public WorkOptDtlXmls() { }
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new WorkOptDtlXml();
			}
		}
		public override string File
		{
			get
			{
                return SystemConfig.PathOfWebApp + "\\WF\\Style\\Tools.xml";
			}
		}
		/// <summary>
		///  Physical table name 
		/// </summary>
		public override string TableName
		{
			get
			{
				return "WorkOptDtl";
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
