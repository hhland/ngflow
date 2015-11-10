using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.XML;
using BP.Sys;

namespace BP.WF.XML
{
    /// <summary>
    ///  A type of work 
    /// </summary>
    public class OneWorkXml : XmlEnNoName
    {
        #region  Property .
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
                return this.GetValStringByKey("No");
            }
        }
        #endregion  Property .

        #region  Structure 
        /// <summary>
        ///  Node extended information 
        /// </summary>
        public OneWorkXml()
        {
        }
        /// <summary>
        ///  Get an instance of 
        /// </summary>
        public override XmlEns GetNewEntities
        {
            get
            {
                return new OneWorkXmls();
            }
        }
        #endregion
    }
    /// <summary>
    ///  A type of work s
    /// </summary>
    public class OneWorkXmls : XmlEns
    {
        #region  Structure 
        /// <summary>
        ///  A type of work s
        /// </summary>
        public OneWorkXmls() { }
        #endregion

        #region  Override the base class property or method .
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override XmlEn GetNewEntity
        {
            get
            {
                return new OneWorkXml();
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
                return "OneWork";
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
