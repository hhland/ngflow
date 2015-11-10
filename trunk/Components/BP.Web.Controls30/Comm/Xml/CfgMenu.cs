using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.XML;
using BP.Sys;


namespace BP.Web.Comm
{
    public class CfgMenuAttr
    {
        /// <summary>
        ///  Serial number 
        /// </summary>
        public const string No = "No";
        /// <summary>
        ///  Name 
        /// </summary>
        public const string Name = "Name";
        public const string HelpFile = "HelpFile";
        public const string Img = "Img";
    }
    /// <summary>
    ///  Configuration Menu 
    /// </summary>
    public class CfgMenu : XmlMenu
    {
        #region  Structure 
        public CfgMenu()
        {
        }
        /// <summary>
        ///  Serial number 
        /// </summary>
        /// <param name="no"></param>
        public CfgMenu(string no)
        {
        }
        /// <summary>
        ///  Get an instance of 
        /// </summary>
        public override XmlEns GetNewEntities
        {
            get
            {
                return new CfgMenus();
            }
        }
        #endregion
    }
    /// <summary>
    ///  Configuration Menu s
    /// </summary>
    public class CfgMenus : XmlMenus
    {
        #region  Structure 
        /// <summary>
        ///  Configuration Menu 
        /// </summary>
        public CfgMenus()
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
                return new CfgMenu();
            }
        }
        public override string File
        {
            get
            {
                return SystemConfig.PathOfWebApp + "\\Comm\\Sys\\CfgMenu.xml";
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
                return null; //new BP.ZF1.Helps();
            }
        }
        #endregion
    }
}
