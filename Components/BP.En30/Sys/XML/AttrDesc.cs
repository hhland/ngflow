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
    public class AttrDescAttr
    {
        /// <summary>
        ///  Wrongdoing 
        /// </summary>
        public const string Attr = "Attr";
        /// <summary>
        ///  Expression 
        /// </summary>
        public const string Desc = "Desc";
        /// <summary>
        ///  Description 
        /// </summary>
        public const string For = "For";
        /// <summary>
        ///  Height 
        /// </summary>
        public const string Height = "Height";
        /// <summary>
        ///  Whether to accept the file data 
        /// </summary>
        public const string IsAcceptFile = "IsAcceptFile";
    }
	/// <summary>
	/// AttrDesc  The summary , Configuration properties .
	/// </summary>
	public class AttrDesc:XmlEn
	{
		#region  Property 
		public string Attr
		{
			get
			{
				return this.GetValStringByKey(AttrDescAttr.Attr);
			}
		}
		public string For
		{
			get
			{
				return this.GetValStringByKey(AttrDescAttr.For);
			}
		}
        public string Desc
        {
            get
            {
                return this.GetValStringByKey(AttrDescAttr.Desc);
            }
        }
        public bool IsAcceptFile
        {
            get
            {
                string s = this.GetValStringByKey(AttrDescAttr.IsAcceptFile);
                if (s == null || s == "" || s=="0")
                    return false;

                return true;
            }
        }
		public int Height
		{
            get
            {
                string str = this.GetValStringByKey(AttrDescAttr.Height);
                if (str == null || str == "")
                    return 200;
                return int.Parse(str);
            }
		}
		#endregion

		#region  Structure 
		public AttrDesc()
		{
		}
		/// <summary>
		///  Get an instance of 
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
				return new AttrDescs();
			}
		}
		#endregion
	}
	/// <summary>
	///  Properties collection 
	/// </summary>
	public class AttrDescs:XmlEns
	{
		#region  Structure 
		/// <summary>
		///  Examination of the data elements wrongdoing 
		/// </summary>
		public AttrDescs()
		{
		}
		public AttrDescs(string enName)
		{
			this.RetrieveBy(AttrDescAttr.For, enName);
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
				return new AttrDesc();
			}
		}
		public override string File
		{
			get
			{
				return SystemConfig.PathOfXML+"\\Ens\\AttrDesc\\";
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
