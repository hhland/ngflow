using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.Sys;
using BP.XML;

namespace BP.WF.XML
{
    /// <summary>
    ///  Mapping menu 
    /// </summary>
    public class MapMenu : XmlEn
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
        public string JS
        {
            get
            {
                return this.GetValStringByKey("JS");
            }
        }
        /// <summary>
        ///  Picture 
        /// </summary>
        public string Img
        {
            get
            {
                return this.GetValStringByKey("Img");
            }
        }
        /// <summary>
        ///  Explanation 
        /// </summary>
        public string Note
        {
            get
            {
                return this.GetValStringByKey("Note");
            }
        }
        #endregion

        #region  Structure 
        /// <summary>
        ///  Node extended information 
        /// </summary>
        public MapMenu()
        {
        }
        /// <summary>
        ///  Get an instance of s
        /// </summary>
        public override XmlEns GetNewEntities
        {
            get
            {
                return new MapMenus();
            }
        }
        #endregion
    }
    /// <summary>
    ///  Mapping menu s
    /// </summary>
    public class MapMenus : XmlEns
    {
        #region  Structure 
        /// <summary>
        ///  Mapping menu s
        /// </summary>
        public MapMenus() { }
        #endregion

        #region  Override the base class property or method .
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override XmlEn GetNewEntity
        {
            get
            {
                return new MapMenu();
            }
        }
        public override string File
        {
            get
            {
               // return SystemConfig.PathOfWebApp + "\\WF\\MapDef\\Style\\XmlDB.xml";
                return SystemConfig.PathOfData + "\\XML\\XmlDB.xml";

            }
        }
        /// <summary>
        ///  Physical table name 
        /// </summary>
        public override string TableName
        {
            get
            {
                return "MapMenu";
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
