using System;
using System.Collections;
using System.Data;
using BP.DA;
using BP.En;
using BP.Sys;
using BP.XML;

namespace BP.GE
{
	/// <summary>
    ///  Main Menu Properties 
	/// </summary>
    public class MainMenuXmlAttr
    {
        /// <summary>
        ///  Serial number   
        /// </summary>    
        public const string No = "No";
        /// <summary>
        ///  Name 
        /// </summary>
        public const string Name = "Name";
    }
	/// <summary>
	///  Main Menu 
	/// </summary>
    public class MainMenuXml : XmlMenu
    {
        #region  Property 
        
        #endregion

        #region  Structure 
        /// <summary>
        ///  Main Menu 
        /// </summary>
        public MainMenuXml()
        {
        }
        /// <summary>
        ///  Main Menu 
        /// </summary>
        /// <param name="no"> Serial number </param>
        public MainMenuXml(string no)
        {
            this.RetrieveByPK(MainMenuXmlAttr.No, no);
        }
        /// <summary>
        ///  Get an instance of 
        /// </summary>
        public override XmlEns GetNewEntities
        {
            get
            {
                return new MainMenuXmls();
            }
        }
        #endregion
    }
	/// <summary>
    ///  Main Menu s
	/// </summary>
    public class MainMenuXmls : XmlMenus
    {
        #region  Structure 
        /// <summary>
        ///  Main Menu s
        /// </summary>
        public MainMenuXmls() { }
        #endregion

        #region  Override the base class property or method .
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override XmlEn GetNewEntity
        {
            get
            {
                return new MainMenuXml();
            }
        }
        public override string File
        {
            get
            {
               return SystemConfig.PathOfXML + "\\MainMenu.xml";
               // return SystemConfig.PathOfXML + "\\Language\\";
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
