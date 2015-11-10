using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.XML;
using BP.Sys;

namespace BP.WF.XML
{
    /// <summary>
    ///  Work Options 
    /// </summary>
	public class WorkOptXml:XmlEnNoName
    {
        #region  Property .
        public new string Name
        {
            get
            {
                return this.GetValStringByKey(BP.Web.WebUser.SysLang);
            }
        }
        public new string CSS
        {
            get
            {
                return this.GetValStringByKey("CSS");
            }
        }

        public string URL
        {
            get
            {
                return this.GetValStringByKey("URL");
            }
        }
        #endregion  Property .

        #region  Structure 
        /// <summary>
		///  Node extended information 
		/// </summary>
		public WorkOptXml()
		{
		}
		/// <summary>
		///  Get an instance of 
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
				return new WorkOptXmls();
			}
		}
		#endregion
	}
	/// <summary>
    ///  Work Options s
	/// </summary>
	public class WorkOptXmls:XmlEns
	{
		#region  Structure 
		/// <summary>
        ///  Work Options s
		/// </summary>
        public WorkOptXmls() { }
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new WorkOptXml();
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
				return "WorkOpt";
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
