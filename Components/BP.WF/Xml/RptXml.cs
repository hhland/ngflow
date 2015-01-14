using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.XML;
using BP.Sys;

namespace BP.WF.XML
{
    /// <summary>
    ///  Process a formula 
    /// </summary>
    public class RptXml : XmlEnNoName
    {
        public new string Name
        {
            get
            {
                return this.GetValStringByKey(BP.Web.WebUser.SysLang);
            }
        }
        public new string URL
        {
            get
            {
                return this.GetValStringByKey("URL");
            }
        }
        public new string ICON
        {
            get
            {
                return this.GetValStringByKey("ICON");
            }
        }

        #region  Structure 
        /// <summary>
        ///  Node extended information 
        /// </summary>
        public RptXml()
        {
        }
        /// <summary>
        ///  Get an instance of 
        /// </summary>
        public override XmlEns GetNewEntities
        {
            get
            {
                return new RptXmls();
            }
        }
        #endregion
    }
    /// <summary>
    ///  Process a formula s
    /// </summary>
    public class RptXmls : XmlEns
    {
        #region  Structure 
        /// <summary>
        ///  Process a formula s
        /// </summary>
        public RptXmls() { }
        #endregion

        #region  Override the base class property or method .
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override XmlEn GetNewEntity
        {
            get
            {
                return new RptXml();
            }
        }
        public override string File
        {
            get
            {
                return SystemConfig.PathOfData + "\\Xml\\WFAdmin.xml";
            }
        }
        /// <summary>
        ///  Physical table name 
        /// </summary>
        public override string TableName
        {
            get
            {
                return "RptFlow";
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
