using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.XML;
using BP.Sys;

namespace BP.WF.XML
{
    /// <summary>
    ///  Skin 
    /// </summary>
	public class CondTypeXml:XmlEnNoName
	{
        public new string Name
        {
            get
            {
                return this.GetValStringByKey(BP.Web.WebUser.SysLang);
            }
        }

		#region  Structure 
		/// <summary>
		///  Node extended information 
		/// </summary>
        public CondTypeXml()
		{
		}
		/// <summary>
		///  Get an instance of 
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
                return new CondTypeXmls();
			}
		}
		#endregion
	}
	/// <summary>
    ///  Skin s
	/// </summary>
	public class CondTypeXmls:XmlEns
	{
		#region  Structure 
		/// <summary>
        ///  Skin s
		/// </summary>
        public CondTypeXmls() { }
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new CondTypeXml();
			}
		}
        public override string File
        {
            get
            {
                return SystemConfig.PathOfData + "\\Xml\\WFAdmin.xml";
            }
        }
		/// <summary>
		///  Physical table name 
		/// </summary>
		public override string TableName
		{
			get
			{
				return "CondType";
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
