using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.XML;
using BP.Sys;

namespace BP.WF.XML
{
    /// <summary>
    ///  Event 
    /// </summary>
    public class EventList : XmlEn
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
        ///  Extension name 
        /// </summary>
        public string NameHtml
        {
            get
            {
                if (this.IsHaveMsg)
                    return "<img src='../Img/Message24.png' border=0 width='17px'/>" + this.GetValStringByKey(BP.Web.WebUser.SysLang);
                else
                    return  this.GetValStringByKey(BP.Web.WebUser.SysLang);
            }
        }
        /// <summary>
        ///  Enter a description 
        /// </summary>
        public string EventDesc
        {
            get
            {
                return this.GetValStringByKey("EventDesc");
            }
        }
        /// <summary>
        ///  Event Type 
        /// </summary>
        public string EventType
        {
            get
            {
                return this.GetValStringByKey("EventType");
            }
        }
        /// <summary>
        ///  Is there a message 
        /// </summary>
        public bool IsHaveMsg
        {
            get
            {
                return this.GetValBoolByKey("IsHaveMsg");
            }
        }
        #endregion

        #region  Structure 
        /// <summary>
        ///  Event 
        /// </summary>
        public EventList()
        {
        }
        /// <summary>
        ///  Get an instance of 
        /// </summary>
        public override XmlEns GetNewEntities
        {
            get
            {
                return new EventLists();
            }
        }
        #endregion
    }
    /// <summary>
    ///  Event s
    /// </summary>
    public class EventLists : XmlEns
    {
        #region  Structure 
        /// <summary>
        ///  Event s
        /// </summary>
        public EventLists() { }
        #endregion

        #region  Override the base class property or method .
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override XmlEn GetNewEntity
        {
            get
            {
                return new EventList();
            }
        }
        /// <summary>
        ///  Storage path 
        /// </summary>
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
