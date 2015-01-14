using System;
using System.Collections;
using System.Data;
using BP.DA;
using BP.En;
using BP.Sys;
using BP.XML;

namespace BP.Web.Controls
{
	/// <summary>
    ///  Property values 
	/// </summary>
    public class FrmPopValAttr
    {
        /// <summary>
        ///  Serial number   
        /// </summary>    
        public const string No = "No";
        /// <summary>
        ///  Name 
        /// </summary>
        public const string Name = "Name";
        /// <summary>
        ///  Label 1
        /// </summary>
        public const string Tag1 = "Tag1";
        /// <summary>
        ///  Label 2
        /// </summary>
        public const string Tag2 = "Tag2";
        /// <summary>
        ///  Parameters 
        /// </summary>
        public const string AtPara = "AtPara";
        public const string H = "H";
        public const string W = "W";
    }
	/// <summary>
	///  Value 
	/// </summary>
    public class FrmPopVal : XmlEnNoName
    {
        #region  Property 
        public string AtPara
        {
            get
            {
                return this.GetValStringByKey(FrmPopValAttr.AtPara);
            }
        }
        public string Tag1
        {
            get
            {
                return this.GetValStringByKey(FrmPopValAttr.Tag1);
            }
        }
        public string Tag2
        {
            get
            {
                return this.GetValStringByKey(FrmPopValAttr.Tag2);
            }
        }
        public string H
        {
            get
            {
                return this.GetValStringByKey(FrmPopValAttr.H);
            }
        }
        public string W
        {
            get
            {
                return this.GetValStringByKey(FrmPopValAttr.W);
            }
        }
        #endregion

        #region  Structure 
        /// <summary>
        ///  Value 
        /// </summary>
        public FrmPopVal()
        {

        }
        /// <summary>
        ///  Value 
        /// </summary>
        /// <param name="no"> Serial number </param>
        public FrmPopVal(string no)
        {
            this.RetrieveByPK(FrmPopValAttr.No, no);
        }
        
        /// <summary>
        ///  Get an instance of 
        /// </summary>
        public override XmlEns GetNewEntities
        {
            get
            {
                return new FrmPopVals();
            }
        }
        #endregion
    }
	/// <summary>
    ///  Value s
	/// </summary>
    public class FrmPopVals : XmlMenus
    {
        #region  Structure 
        /// <summary>
        ///  Value s
        /// </summary>
        public FrmPopVals()
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
                return new FrmPopVal();
            }
        }
        public override string File
        {
            get
            {
                return SystemConfig.PathOfDataUser + "\\Xml\\FrmPopVal.xml";
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
