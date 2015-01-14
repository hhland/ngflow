using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.XML;

namespace BP.Sys.XML
{
    /// <summary>
    ///  Online Help 
    /// </summary>
	public class OnlineHelper:XmlEnNoName
	{
		#region  Structure 
		/// <summary>
        ///  Online Help 
		/// </summary>
		public OnlineHelper()
		{
		}
		/// <summary>
        ///  Online Help 
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
				return new OnlineHelpers();
			}
		}
		#endregion
	}
	/// <summary>
    ///  Online Help s
	/// </summary>
	public class OnlineHelpers:XmlEns
	{
		#region  Structure 
		/// <summary>
        ///  Online Help s
		/// </summary>
        public OnlineHelpers() { }
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new OnlineHelper();
			}
		}
		public override string File
		{
			get
			{
                return SystemConfig.PathOfWebApp + "\\WF\\OnlineHepler\\";
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
