using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.XML;
using BP.Sys;

namespace BP.WF.XML
{
    /// <summary>
    ///  Process button 
    /// </summary>
    public class FlowBar : XmlEn
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
        public string Title
        {
            get
            {
                return this.GetValStringByKey("Title");
            }
        }
        public string Url
        {
            get
            {
                return this.GetValStringByKey("Url");
            }
        }
        #endregion

        #region  Structure 
        /// <summary>
        ///  Node extended information 
        /// </summary>
        public FlowBar()
        {
        }
        /// <summary>
        ///  Get an instance of 
        /// </summary>
        public override XmlEns GetNewEntities
        {
            get
            {
                return new FlowBars();
            }
        }
        #endregion
    }
    /// <summary>
    ///  Process button s
    /// </summary>
    public class FlowBars : XmlEns
    {
        #region  Structure 
        /// <summary>
        ///  Assessment rate data elements 
        /// </summary>
        public FlowBars() { }
        #endregion

        #region  Override the base class property or method .
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override XmlEn GetNewEntity
        {
            get
            {
                return new FlowBar();
            }
        }
        public override string File
        {
            get
            {
                return SystemConfig.PathOfWebApp + "\\DataUser\\XML\\BarOfTop.xml";
            }
        }
        /// <summary>
        ///  Physical table name 
        /// </summary>
        public override string TableName
        {
            get
            {
                return "FlowBar";
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
