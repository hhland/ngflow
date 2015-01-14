using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.XML;

namespace BP.Sys
{
    public class ExpFucnXmlList
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
        /// <summary>
        /// Func
        /// </summary>
        public const string Func = "Func";
    }
	public class ExpFucnXml:XmlEnNoName
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
		public ExpFucnXml()
		{
		}
        public ExpFucnXml(string no)
        {
            
        }
		/// <summary>
		///  Get an instance of 
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
				return new ExpFucnXmls();
			}
		}
		#endregion
	}
	/// <summary>
	/// 
	/// </summary>
	public class ExpFucnXmls:XmlEns
	{
		#region  Structure 
		/// <summary>
		///  Assessment rate data elements 
		/// </summary>
        public ExpFucnXmls() { }
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new ExpFucnXml();
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
                return "ExpFunc";
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
