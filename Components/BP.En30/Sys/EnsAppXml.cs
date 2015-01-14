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
    public class EnsAppXmlEnsName
    {
        /// <summary>
        ///  Wrongdoing 
        /// </summary>
        public const string EnsName = "EnsName";
        /// <summary>
        ///  Expression 
        /// </summary>
        public const string Desc = "Desc";
        /// <summary>
        ///  Data Types 
        /// </summary>
        public const string DBType = "DBType";
        /// <summary>
        ///  Defaults 
        /// </summary>
        public const string DefVal = "DefVal";
        /// <summary>
        /// ֵ
        /// </summary>
        public const string EnumKey = "EnumKey";
        /// <summary>
        /// ֵ
        /// </summary>
        public const string EnumVals = "EnumVals";
    }
    /// <summary>
    /// EnsAppXml  The summary , Configuration properties .
    /// </summary>
    public class EnsAppXml : XmlEnNoName
    {
        #region  Property 
        /// <summary>
        ///  Enum value 
        /// </summary>
        public string EnumKey
        {
            get
            {
                return this.GetValStringByKey(EnsAppXmlEnsName.EnumKey);
            }
        }
        public string EnumVals
        {
            get
            {
                return this.GetValStringByKey(EnsAppXmlEnsName.EnumVals);
            }
        }
        /// <summary>
        ///  Class name 
        /// </summary>
        public string EnsName
        {
            get
            {
                return this.GetValStringByKey(EnsAppXmlEnsName.EnsName);
            }
        }
        /// <summary>
        ///  Data Types 
        /// </summary>
        public string DBType
        {
            get
            {
                return this.GetValStringByKey(EnsAppXmlEnsName.DBType);
            }
        }
        /// <summary>
        ///  Description 
        /// </summary>
        public string Desc
        {
            get
            {
                return this.GetValStringByKey(EnsAppXmlEnsName.Desc);
            }
        }
        /// <summary>
        ///  Defaults 
        /// </summary>
        public string DefVal
        {
            get
            {
                return this.GetValStringByKey(EnsAppXmlEnsName.DefVal);
            }
        }
        public bool DefValBoolen
        {
            get
            {
                return this.GetValBoolByKey(EnsAppXmlEnsName.DefVal);
            }
        }
        public int DefValInt
        {
            get
            {
                return this.GetValIntByKey(EnsAppXmlEnsName.DefVal);
            }
        }
        #endregion

        #region  Structure 
        public EnsAppXml()
        {
        }
        /// <summary>
        ///  Get an instance of 
        /// </summary>
        public override XmlEns GetNewEntities
        {
            get
            {
                return new EnsAppXmls();
            }
        }
        #endregion
    }
    /// <summary>
    ///  Properties collection 
    /// </summary>
    public class EnsAppXmls : XmlEns
    {
        #region  Structure 
        /// <summary>
        ///  Examination of the data elements wrongdoing 
        /// </summary>
        public EnsAppXmls()
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
                return new EnsAppXml();
            }
        }
        public override string File
        {
            get
            {
                return SystemConfig.PathOfXML + "\\Ens\\EnsAppXml\\GE.xml";
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
