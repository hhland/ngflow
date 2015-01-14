using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.XML;

namespace BP.Sys
{

    public class InputCheckXmlList
    {
        /// <summary>
        ///  Events Menu 
        /// </summary>
        public const string ActiveDDL = "ActiveDDL";
        /// <summary>
        ///  Enter the verification 
        /// </summary>
        public const string InputCheck = "InputCheck";
        /// <summary>
        ///  AutoComplete 
        /// </summary>
        public const string AutoFull = "AutoFull";
        /// <summary>
        /// Pop The return value 
        /// </summary>
        public const string PopVal = "PopVal";
    }
	public class InputCheckXml:XmlEnNoName
	{
		#region  Property 
        public new string Name
        {
            get
            {
                return this.GetValStringByKey(BP.Web.WebUser.SysLang);
            }
        }
		#endregion

		#region  Structure 
		/// <summary>
		///  Node extended information 
		/// </summary>
		public InputCheckXml()
		{
		}
        public InputCheckXml(string no)
        {
            
        }
		/// <summary>
		///  Get an instance of 
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
				return new InputCheckXmls();
			}
		}
		#endregion
	}
	/// <summary>
	/// 
	/// </summary>
	public class InputCheckXmls:XmlEns
	{
		#region  Structure 
		/// <summary>
		///  Assessment rate data elements 
		/// </summary>
        public InputCheckXmls() { }
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new InputCheckXml();
			}
		}
		public override string File
		{
			get
			{
                return SystemConfig.PathOfXML + "MapExt.xml";
			}
		}
		/// <summary>
		///  Physical table name 
		/// </summary>
		public override string TableName
		{
			get
			{
                return "InputCheck";
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
