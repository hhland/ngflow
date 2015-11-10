using System;
using System.Collections;
using System.Data;
using BP.DA;
using BP.En;
using BP.XML;


namespace BP.Sys.Xml
{
    /// <summary>
    ///  Property 
    /// </summary>
    public class EnsAppGroupXmlEnsName
    {
        /// <summary>
        ///  Wrongdoing 
        /// </summary>
        public const string EnsName = "EnsName";
        /// <summary>
        ///  Expression 
        /// </summary>
        public const string GroupKey = "GroupKey";
        /// <summary>
        ///  Data Types 
        /// </summary>
        public const string GroupName = "GroupName";
    }
    /// <summary>
    /// EnsAppGroupXml  The summary , Configuration properties .
    /// </summary>
    public class EnsAppGroupXml : XmlEnNoName
    {
        #region  Property 
        /// <summary>
        ///  Class name 
        /// </summary>
        public string EnsName
        {
            get
            {
                return this.GetValStringByKey(EnsAppGroupXmlEnsName.EnsName);
            }
        }
        /// <summary>
        ///  Data Types 
        /// </summary>
        public string GroupName
        {
            get
            {
                return this.GetValStringByKey(EnsAppGroupXmlEnsName.GroupName);
            }
        }
        /// <summary>
        ///  Description 
        /// </summary>
        public string GroupKey
        {
            get
            {
                return this.GetValStringByKey(EnsAppGroupXmlEnsName.GroupKey);
            }
        }
        #endregion

        #region  Structure 
        public EnsAppGroupXml()
        {
        }
        /// <summary>
        ///  Get an instance of 
        /// </summary>
        public override XmlEns GetNewEntities
        {
            get
            {
                return new EnsAppGroupXmls();
            }
        }
        #endregion
    }
    /// <summary>
    ///  Properties collection 
    /// </summary>
    public class EnsAppGroupXmls : XmlEns
    {
        #region  Structure 
        /// <summary>
        ///  Examination of the data elements wrongdoing 
        /// </summary>
        public EnsAppGroupXmls()
        {
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
                return new EnsAppGroupXml();
            }
        }
        public override string File
        {
            get
            {
                return SystemConfig.PathOfXML + "\\Ens\\EnsAppXml\\";
            }
        }
        /// <summary>
        ///  Physical table name 
        /// </summary>
        public override string TableName
        {
            get
            {
                return "Group";
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
