using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.Sys;
using BP.XML;

namespace BP.WF.XML
{
    /// <summary>
    ///  Mobile Menu 
    /// </summary>
    public class Mobile : XmlEn
    {
        #region  Property 
        /// <summary>
        ///  Serial number 
        /// </summary>
        public string No
        {
            get
            {
                return this.GetValStringByKey("No");
            }
        }
        /// <summary>
        ///  Name 
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetValStringByKey("Name");
            }
        }
        #endregion

        #region  Structure 
        /// <summary>
        ///  Node extended information 
        /// </summary>
        public Mobile()
        {
        }
        /// <summary>
        ///  Get an instance of 
        /// </summary>
        public override XmlEns GetNewEntities
        {
            get
            {
                return new Mobiles();
            }
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class Mobiles : XmlEns
    {
        #region  Structure 
        /// <summary>
        ///  Assessment rate data elements 
        /// </summary>
        public Mobiles() { }
        #endregion

        #region  Override the base class property or method .
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override XmlEn GetNewEntity
        {
            get
            {
                return new Mobile();
            }
        }
        public override string File
        {
            get
            {
                return SystemConfig.CCFlowAppPath + "DataUser\\XML\\Mobiles.xml";
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
