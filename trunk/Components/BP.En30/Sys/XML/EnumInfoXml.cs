using System;
using System.Collections;
using System.Data;
using BP.DA;
using BP.En;
using BP.XML;


namespace BP.Sys.Xml
{
	/// <summary>
	///  Property 
	/// </summary>
    public class EnumInfoXmlAttr
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
        ///  Description 
        /// </summary>
        public const string Vals = "Vals";
    }
	/// <summary>
	/// EnumInfoXml  The summary , Configuration properties .
	/// </summary>
    public class EnumInfoXml : XmlEn
    {
        #region  Property 
        public string Key
        {
            get
            {
                return this.GetValStringByKey("Key");
            }
        }
        /// <summary>
        /// Vals
        /// </summary>
        public string Vals
        {
            get
            {
                return this.GetValStringByKey(BP.Web.WebUser.SysLang);
            }
        }
        #endregion

        #region  Structure 
        public EnumInfoXml()
        {
        }
        public EnumInfoXml(string key)
        {
            this.RetrieveByPK("Key", key);
        }
        
        /// <summary>
        ///  Get an instance of 
        /// </summary>
        public override XmlEns GetNewEntities
        {
            get
            {
                return new EnumInfoXmls();
            }
        }
        #endregion
    }
	/// <summary>
	///  Properties collection 
	/// </summary>
	public class EnumInfoXmls:XmlEns
	{
		#region  Structure 
		/// <summary>
		///  Examination of the data elements wrongdoing 
		/// </summary>
		public EnumInfoXmls()
		{
		}
	 
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new EnumInfoXml();
			}
		}
        public override string File
        {
            get
            {
                return SystemConfig.PathOfXML + "\\Enum\\";
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
