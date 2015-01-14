using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.XML;
using BP.Sys;

namespace BP.WF.XML
{
    /// <summary>
    ///  Process Monitoring menu 
    /// </summary>
    public class WatchDogXml : XmlEnNoName
    {
        public new string Name
        {
            get
            {
                return this.GetValStringByKey(BP.Web.WebUser.SysLang);
            }
        }

        #region  Structure 
        /// <summary>
        ///  Process Monitoring menu 
        /// </summary>
        public WatchDogXml()
        {
        }
        /// <summary>
        ///  Get an instance of 
        /// </summary>
        public override XmlEns GetNewEntities
        {
            get
            {
                return new WatchDogXmls();
            }
        }
        #endregion
    }
    /// <summary>
    ///  Process Monitoring menu s
    /// </summary>
    public class WatchDogXmls : XmlEns
    {
        #region  Structure 
        /// <summary>
        ///  Process Monitoring menu s
        /// </summary>
        public WatchDogXmls() { }
        #endregion

        #region  Override the base class property or method .
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override XmlEn GetNewEntity
        {
            get
            {
                return new WatchDogXml();
            }
        }
        public override string File
        {
            get
            {
                return SystemConfig.PathOfWebApp + "\\WF\\Admin\\Sys\\Sys.xml";
            }
        }
        /// <summary>
        /// ±í
        /// </summary>
        public override string TableName
        {
            get
            {
                return "WatchDog";
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
