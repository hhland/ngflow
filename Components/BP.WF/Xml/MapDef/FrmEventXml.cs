using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.XML;
using BP.Sys;

namespace BP.WF.XML
{
    /// <summary>
    ///  Form events 
    /// </summary>
	public class FrmEventXml:XmlEn
	{
		#region  Property 
        public string No
        {
            get
            {
                return this.GetValStringByKey("No");
            }
        }
        public string Name
        {
            get
            {
                return this.GetValStringByKey(BP.Web.WebUser.SysLang);
            }
        }
		/// <summary>
		///  Picture 
		/// </summary>
		public string Img
		{
			get
			{
				return  this.GetValStringByKey("Img") ;
			}
		}
        public string Title
        {
            get
            {
                return this.GetValStringByKey("Title");
            }
        }
        public string Url
        {
            get
            {
                 string url=this.GetValStringByKey("Url");
                 if (url == "")
                     url = "javascript:" + this.GetValStringByKey("OnClick") ;
                 return url;
            }
        }
		#endregion

		#region  Structure 
		/// <summary>
		///  Form events 
		/// </summary>
		public FrmEventXml()
		{
		}
		/// <summary>
		///  Get an instance of 
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
				return new FrmEventXmls();
			}
		}
		#endregion
	}
	/// <summary>
    ///  Form events 
	/// </summary>
	public class FrmEventXmls:XmlEns
	{
		#region  Structure 
		/// <summary>
		///  Assessment rate data elements 
		/// </summary>
        public FrmEventXmls() { }
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new FrmEventXml();
			}
		}
		public override string File
		{
			get
			{
               // return SystemConfig.PathOfWebApp + "\\WF\\MapDef\\Style\\XmlDB.xml";

                return SystemConfig.PathOfData + "\\XML\\XmlDB.xml";

			}
		}
		/// <summary>
		///  Physical table name 
		/// </summary>
		public override string TableName
		{
			get
			{
                return "FrmEvent";
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
