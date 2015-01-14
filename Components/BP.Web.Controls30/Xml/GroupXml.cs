using System;
using System.Collections;
using System.Data;
using BP.DA;
using BP.Sys;
using BP.En;
using BP.XML;

namespace BP.Web
{
	/// <summary>
    ///  Packet Menu Properties 
	/// </summary>
    public class GroupXmlAttr
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
	///  Packet menu 
	/// </summary>
    public class GroupXml : XmlEnNoName
    {
        #region  Property 
        #endregion

        #region  Structure 
        /// <summary>
        ///  Packet menu 
        /// </summary>
        public GroupXml()
        {

        }
        /// <summary>
        ///  Packet menu 
        /// </summary>
        /// <param name="no"> Serial number </param>
        public GroupXml(string no)
        {
            this.RetrieveByPK(GroupXmlAttr.No, no);
        }
        
        /// <summary>
        ///  Get an instance of 
        /// </summary>
        public override XmlEns GetNewEntities
        {
            get
            {
                return new GroupXmls();
            }
        }
        #endregion
    }
	/// <summary>
    ///  Packet menu s
	/// </summary>
    public class GroupXmls : XmlMenus
    {
        #region  Structure 
        /// <summary>
        ///  Packet menu s
        /// </summary>
        public GroupXmls()
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
                return new GroupXml();
            }
        }
        public override string File
        {
            get
            {
                return SystemConfig.PathOfXML + "\\Ens\\Group.xml";
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
