using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.Sys;
using BP.XML;

namespace BP.WF.XML
{
    /// <summary>
    ///  Event List 
    /// </summary>
    public class EventListDtlList
    {
        /// <summary>
        ///  Save ago 
        /// </summary>
        public const string DtlSaveBefore = "DtlSaveBefore";
        /// <summary>
        ///  Saved 
        /// </summary>
        public const string DtlSaveEnd = "DtlSaveEnd";
        /// <summary>
        ///  Former record-keeping 
        /// </summary>
        public const string DtlItemSaveBefore = "DtlItemSaveBefore";
        /// <summary>
        ///  After the record-keeping 
        /// </summary>
        public const string DtlItemSaveAfter = "DtlItemSaveAfter";
        /// <summary>
        ///  Before the record is deleted 
        /// </summary>
        public const string DtlItemDelBefore = "DtlItemDelBefore";
        /// <summary>
        ///  After the record is deleted 
        /// </summary>
        public const string DtlItemDelAfter = "DtlItemDelAfter";
    }
    /// <summary>
    ///  From table event 
    /// </summary>
    public class EventListDtl : XmlEn
    {
        #region  Property 
        /// <summary>
        ///  Serial number 
        /// </summary>
        public string No
        {
            get
            {
                return this.GetValStringByKey("No");
            }
        }
        /// <summary>
        ///  Name 
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetValStringByKey(BP.Web.WebUser.SysLang);
            }
        }
        /// <summary>
        ///  Description 
        /// </summary>
        public string EventDesc
        {
            get
            {
                return this.GetValStringByKey("EventDesc");
            }
        }
        #endregion

        #region  Structure 
        /// <summary>
        ///  From table event 
        /// </summary>
        public EventListDtl()
        {
        }
        /// <summary>
        ///  Get an instance of 
        /// </summary>
        public override XmlEns GetNewEntities
        {
            get
            {
                return new EventListDtls();
            }
        }
        #endregion
    }
    /// <summary>
    ///  From table event s
    /// </summary>
    public class EventListDtls : XmlEns
    {
        #region  Structure 
        /// <summary>
        ///  From table event s
        /// </summary>
        public EventListDtls() { }
        #endregion

        #region  Override the base class property or method .
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override XmlEn GetNewEntity
        {
            get
            {
                return new EventListDtl();
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
                return "ItemDtl";
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
