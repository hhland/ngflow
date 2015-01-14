using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.XML;

namespace BP.Sys
{
    /// <summary>
    ///  Text Box Event Properties 
    /// </summary>
    public class TBEventXmlList
    {
        /// <summary>
        ///  Function 
        /// </summary>
        public const string Func = "Func";
        /// <summary>
        ///  Event Name 
        /// </summary>
        public const string EventName = "EventName";
        /// <summary>
        /// Ϊ
        /// </summary>
        public const string DFor = "DFor";
    }
    /// <summary>
    ///  Box Event 
    /// </summary>
	public class TBEventXml:XmlEn
	{
		#region  Property 
        /// <summary>
        ///  Event Name 
        /// </summary>
        public string EventName
        {
            get
            {
                return this.GetValStringByKey(TBEventXmlList.EventName);
            }
        }
        /// <summary>
        ///  Function 
        /// </summary>
        public string Func
        {
            get
            {
                return this.GetValStringByKey(TBEventXmlList.Func);
            }
        }
        /// <summary>
        ///  Data for 
        /// </summary>
        public string DFor
        {
            get
            {
                return this.GetValStringByKey(TBEventXmlList.DFor);
            }
        }
		#endregion

		#region  Structure 
		/// <summary>
		///  Box Event 
		/// </summary>
		public TBEventXml()
		{
		}
        public TBEventXml(string no)
        {
        }
		/// <summary>
		///  Get an instance of 
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
				return new TBEventXmls();
			}
		}
		#endregion
	}
	/// <summary>
    ///  Box Event s
	/// </summary>
	public class TBEventXmls:XmlEns
	{
		#region  Structure 
		/// <summary>
        ///  Box Event s
		/// </summary>
        public TBEventXmls() { }
        /// <summary>
        ///  Box Event s
        /// </summary>
        /// <param name="dFor"></param>
        public TBEventXmls(string dFor)
        {
            this.Retrieve(TBEventXmlList.DFor, dFor);
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
				return new TBEventXml();
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
                return "TBEvent";
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
