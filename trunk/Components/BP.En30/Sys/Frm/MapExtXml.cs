using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.XML;

namespace BP.Sys
{
    public class MapExtXmlList
    {
        /// <summary>
        ///  Get in touch with the external data set 
        /// </summary>
        public const string AutoFull = "AutoFull";
        /// <summary>
        ///  Events Menu 
        /// </summary>
        public const string ActiveDDL = "ActiveDDL";
        /// <summary>
        ///  Enter the verification 
        /// </summary>
        public const string InputCheck = "InputCheck";
        /// <summary>
        ///  Automatically populate the text box 
        /// </summary>
        public const string TBFullCtrl = "TBFullCtrl";
        /// <summary>
        /// Pop The return value 
        /// </summary>
        public const string PopVal = "PopVal";
        /// <summary>
        /// Func
        /// </summary>
        public const string Func = "Func";
        /// <summary>
        /// ( Dynamic ) Populate drop-down box 
        /// </summary>
        public const string AutoFullDLL = "AutoFullDLL";
        /// <summary>
        ///  Automatically populate drop-down box 
        /// </summary>
        public const string DDLFullCtrl = "DDLFullCtrl";
        /// <summary>
        ///  Form filling is loaded 
        /// </summary>
        public const string PageLoadFull = "PageLoadFull";
        /// <summary>
        ///  Initiate the process 
        /// </summary>
        public const string StartFlow = "StartFlow";
        /// <summary>
        ///  Hyperlinks .
        /// </summary>
        public const string Link = "Link";
        /// <summary>
        ///  Automatic generation number 
        /// </summary>
        public const string AotuGenerNo = "AotuGenerNo";
        /// <summary>
        ///  Regex 
        /// </summary>
        public const string RegularExpression = "RegularExpression";


        public const string WordFrm = "WordFrm";
        public const string ExcelFrm = "ExcelFrm";

    }
	public class MapExtXml:XmlEnNoName
	{
		#region  Property 
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
		#endregion

		#region  Structure 
		/// <summary>
		///  Node extended information 
		/// </summary>
		public MapExtXml()
		{
		}
		/// <summary>
		///  Get an instance of 
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
				return new MapExtXmls();
			}
		}
		#endregion
	}
	/// <summary>
	/// 
	/// </summary>
	public class MapExtXmls:XmlEns
	{
		#region  Structure 
		/// <summary>
		///  Assessment rate data elements 
		/// </summary>
        public MapExtXmls() { }
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new MapExtXml();
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
				return "FieldExt";
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
