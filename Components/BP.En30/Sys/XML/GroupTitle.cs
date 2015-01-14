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
    public class GroupTitleAttr
    {
        public const string For = "For";
        public const string Key = "Key";
        public const string Title = "Title";
    }
	/// <summary>
	/// GroupTitle  The summary .
	///  Examination of the data elements wrongdoing 
	/// 1, It is  GroupTitle  One detail .
	/// 2, It represents a data element .
	/// </summary>
    public class GroupTitle : XmlEn
    {
        #region  Property 
        /// <summary>
        ///  Select the time required for this attribute condition 
        /// </summary>
        public string Title
        {
            get
            {
                return this.GetValStringByKey(GroupTitleAttr.Title);
            }
        }
        public string For
        {
            get
            {
                return this.GetValStringByKey(GroupTitleAttr.For);
            }
        }
        public string Key
        {
            get
            {
                return this.GetValStringByKey(GroupTitleAttr.Key);
            }
        }
        #endregion

        #region  Structure 
        public GroupTitle()
        {
        }
        /// <summary>
        ///  Get an instance of 
        /// </summary>
        public override XmlEns GetNewEntities
        {
            get
            {
                return new GroupTitles();
            }
        }
        #endregion
    }
	/// <summary>
	/// 
	/// </summary>
	public class GroupTitles:XmlEns
	{
		#region  Structure 
		/// <summary>
		///  Examination of the data elements wrongdoing 
		/// </summary>
		public GroupTitles(){}
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new GroupTitle();
			}
		}
		public override string File
		{
			get
			{
				return SystemConfig.PathOfXML+"\\Ens\\GroupTitle.xml";
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
