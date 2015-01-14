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
    public class ActiveAttrAttr
    {
        /// <summary>
        /// AttrKey
        /// </summary>
        public const string AttrKey = "AttrKey";
        /// <summary>
        ///  Expression 
        /// </summary>
        public const string AttrName = "AttrName";
        /// <summary>
        ///  Description 
        /// </summary>
        public const string Exp = "Exp";
        /// <summary>
        /// ExpApp
        /// </summary>
        public const string ExpApp = "ExpApp";
        /// <summary>
        /// for
        /// </summary>
        public const string For = "For";
        /// <summary>
        ///  Condition 
        /// </summary>
        public const string Condition = "Condition";
    }
	/// <summary>
	/// ActiveAttr  The summary .
	///  Examination of the data elements wrongdoing 
	/// 1, It is  ActiveAttr  One detail .
	/// 2, It represents a data element .
	/// </summary>
    public class ActiveAttr : XmlEn
    {
        #region  Property 
        /// <summary>
        ///  Select the time required for this attribute condition 
        /// </summary>
        public string Condition
        {
            get
            {
                return this.GetValStringByKey(ActiveAttrAttr.Condition);
            }
        }
        public string AttrKey
        {
            get
            {
                return this.GetValStringByKey(ActiveAttrAttr.AttrKey);
            }
        }
        public string AttrName
        {
            get
            {
                return this.GetValStringByKey(ActiveAttrAttr.AttrName);
            }
        }
        public string Exp
        {
            get
            {
                return this.GetValStringByKey(ActiveAttrAttr.Exp);
            }
        }
        public string ExpApp
        {
            get
            {
                return this.GetValStringByKey(ActiveAttrAttr.ExpApp);
            }
        }
        public string For
        {
            get
            {
                return this.GetValStringByKey(ActiveAttrAttr.For);
            }
        }
        #endregion

        #region  Structure 
        public ActiveAttr()
        {
        }
        /// <summary>
        ///  Get an instance of 
        /// </summary>
        public override XmlEns GetNewEntities
        {
            get
            {
                return new ActiveAttrs();
            }
        }
        #endregion
    }
	/// <summary>
	/// 
	/// </summary>
	public class ActiveAttrs:XmlEns
	{
		#region  Structure 
		/// <summary>
		///  Examination of the data elements wrongdoing 
		/// </summary>
		public ActiveAttrs(){}
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new ActiveAttr();
			}
		}
		public override string File
		{
			get
			{
				return SystemConfig.PathOfXML+"\\Ens\\ActiveAttr.xml";
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
