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
    public class WebConfigDescAttr
    {
        /// <summary>
        ///  Wrongdoing 
        /// </summary>
        public const string No = "No";
        /// <summary>
        /// Name
        /// </summary>
        public const string Name = "Name";
        /// <summary>
        ///  Expression 
        /// </summary>
        public const string URL = "URL";
        /// <summary>
        ///  Description 
        /// </summary>
        public const string Note = "Note";
        /// <summary>
        ///  Type 
        /// </summary>
        public const string DBType = "DBType";
        /// <summary>
        /// IsEnable
        /// </summary>
        public const string IsEnable = "IsEnable";
        public const string IsShow = "IsShow";
        /// <summary>
        /// Vals
        /// </summary>
        public const string Vals = "Vals";
    }
    /// <summary>
    ///  Profile Information 
    /// </summary>
    public class WebConfigDesc : XmlEn
    {
        #region  Property 
        private string _No = "";
        public string No
        {
            get
            {
                if (_No == "")
                    return this.GetValStringByKey(WebConfigDescAttr.No);
                else
                    return _No;
            }
            set
            {
                _No = value;
            }
        }
        public string Name
        {
            get
            {
                return this.GetValStringByKey(WebConfigDescAttr.Name);
            }
        }
        public bool IsEnable
        {
            get
            {
                if (this.GetValStringByKey(WebConfigDescAttr.IsEnable) == "0")
                    return false;
                return true;
            }
        }
        public bool IsShow
        {
            get
            {
                if (this.GetValStringByKey(WebConfigDescAttr.IsShow) == "0")
                    return false;
                return true;
            }
        }
        /// <summary>
        ///  Description 
        /// </summary>
        public string Note
        {
            get
            {
                return this.GetValStringByKey(WebConfigDescAttr.Note);
            }
        }
        public string Vals
        {
            get
            {
                return this.GetValStringByKey(WebConfigDescAttr.Vals);
            }
        }
        /// <summary>
        ///  Type 
        /// </summary>
        public string DBType
        {
            get
            {
                return this.GetValStringByKey(WebConfigDescAttr.DBType);
            }
        }
        #endregion

        #region  Structure 
        public WebConfigDesc()
        {
        }
        /// <summary>
        ///  Get an instance of 
        /// </summary>
        public override XmlEns GetNewEntities
        {
            get
            {
                return new WebConfigDescs();
            }
        }
        #endregion
    }
    /// <summary>
    ///  Profile Information 
    /// </summary>
    public class WebConfigDescs : XmlEns
    {
        #region  Structure 
        /// <summary>
        ///  Profile Information 
        /// </summary>
        public WebConfigDescs() { }
        #endregion

        #region  Override the base class property or method .
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override XmlEn GetNewEntity
        {
            get
            {
                return new WebConfigDesc();
            }
        }
        /// <summary>
        ///  File 
        /// </summary>
        public override string File
        {
            get
            {
                return SystemConfig.PathOfXML + "\\WebConfigDesc.xml";
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
