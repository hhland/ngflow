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
	public class Skin:XmlEnNoName
	{
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

		#region  Structure 
		/// <summary>
		///  Node extended information 
		/// </summary>
		public Skin()
		{
		}
		/// <summary>
		///  Get an instance of 
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
				return new Skins();
			}
		}
		#endregion
	}
	/// <summary>
    ///  Skin s
	/// </summary>
	public class Skins:XmlEns
	{
		#region  Structure 
		/// <summary>
        ///  Skin s
		/// </summary>
        public Skins() { }
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new Skin();
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
				return "Skin";
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
