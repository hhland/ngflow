using System;
using System.IO;
using System.Collections;
using BP.DA;
using System.Data;
using BP.Sys;
using BP.En;

namespace BP.XML
{
    public class XmlMenuAttr
    {
        public const string No = "No";
        public const string Name = "Name";
        public const string Url = "Url";
        public const string Target = "Target";
        public const string Img = "Img";
    }
    abstract public class XmlMenu : XmlEnNoName
    {
        /// <summary>
        ///  Function No. 
        /// </summary>
        public string Img
        {
            get
            {
                return this.GetValStringByKey("Img");
            }
        }
        public string Name
        {
            get
            {
                return this.GetValStringByKey("Name");
            }
        }
        public string Url
        {
            get
            {
                return this.GetValStringByKey("Url");
            }
        }
        public string Target
        {
            get
            {
                return this.GetValStringByKey("Target");
            }
        }
        public XmlMenu()
        {
        }
        public XmlMenu(string no)
        {
            this.RetrieveByPK("No", no);
        }
    }
    /// <summary>
    /// XmlMenus  The summary .
    /// </summary>
    abstract public class XmlMenus : XmlEns
    {
        #region  Structure 
        /// <summary>
        ///  Structure 
        /// </summary>
        public XmlMenus()
        {
        }
        #endregion  Structure 
    }
}