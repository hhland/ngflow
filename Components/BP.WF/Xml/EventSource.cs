using System;
using System.Collections;
using BP.DA;
using BP.Sys;
using BP.En;
using BP.XML;

namespace BP.WF.XML
{
    /// <summary>
    ///  Event Source 
    /// </summary>
	public class EventSource:XmlEn
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
		#endregion

		#region  Structure 
		/// <summary>
        ///  Event Source 
		/// </summary>
		public EventSource()
		{
		}
		/// <summary>
		///  Get an instance of 
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
				return new EventSources();
			}
		}
		#endregion
	}
	/// <summary>
    ///  Event Source s
	/// </summary>
    public class EventSources : XmlEns
    {
        #region  Structure 
        /// <summary>
        ///  Event Source s
        /// </summary>
        public EventSources() { }
        #endregion

        #region  Override the base class property or method .
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override XmlEn GetNewEntity
        {
            get
            {
                return new EventSource();
            }
        }
        public override string File
        {
            get
            {
                return SystemConfig.PathOfXML + "\\EventList.xml";
            }
        }
        /// <summary>
        ///  Physical table name 
        /// </summary>
        public override string TableName
        {
            get
            {
                return "Source";
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
