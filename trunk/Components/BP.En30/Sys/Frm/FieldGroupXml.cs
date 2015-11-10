using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.XML;


namespace BP.Sys
{
    /// <summary>
    ///  Packet content 
    /// </summary>
    public class FieldGroupXml : XmlEn
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
        public string Desc
        {
            get
            {
                return this.GetValStringByKey(BP.Web.WebUser.SysLang+"Desc");
            }
        }
        #endregion

        #region  Structure 
        /// <summary>
        ///  Node extended information 
        /// </summary>
        public FieldGroupXml()
        {
        }
        /// <summary>
        ///  Get an instance of s
        /// </summary>
        public override XmlEns GetNewEntities
        {
            get
            {
                return new FieldGroupXmls();
            }
        }
        #endregion
    }
    /// <summary>
    ///  Packet content s
    /// </summary>
    public class FieldGroupXmls : XmlEns
    {
        #region  Structure 
        /// <summary>
        ///  Packet content s
        /// </summary>
        public FieldGroupXmls() { }
        #endregion

        #region  Override the base class property or method .
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override XmlEn GetNewEntity
        {
            get
            {
                return new FieldGroupXml();
            }
        }
        public override string File
        {
            get
            {
               // return SystemConfig.PathOfWebApp + "\\WF\\MapDef\\Style\\XmlDB.xml";
                                return SystemConfig.PathOfData + "\\XML\\XmlDB.xml";
                //\MapDef\\Style\
            }
        }
        /// <summary>
        ///  Physical table name 
        /// </summary>
        public override string TableName
        {
            get
            {
                return "FieldGroup";
            }
        }
        public override Entities RefEns
        {
            get
            {
                return null; //new BP.ZF1.AdminTools();
            }
        }
        #endregion
    }
}
