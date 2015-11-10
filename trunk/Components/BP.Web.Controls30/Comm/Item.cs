using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.XML;
using BP.Sys;


namespace BP.Web.Port.Xml
{
    public class ItemAttr
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
        /// HelpFile
        /// </summary>
        public const string HelpFile = "HelpFile";
    }
	/// <summary>
	/// 
	/// </summary>
	public class Item:XmlEn
	{
		#region  Property 
        public string HelpFile
        {
            get
            {
                return this.GetValStringByKey(ItemAttr.HelpFile);
            }
        }
		/// <summary>
		///  Serial number 
		/// </summary>
		public string No
		{
			get
			{
				return this.GetValStringByKey(ItemAttr.No);
			}
		}
		/// <summary>
		///  Name 
		/// </summary>
		public string Name
		{
			get
			{
                return this.GetValStringByKey(BP.Web.WebUser.SysLang );
			}
		}
		#endregion

		#region  Structure 
		public Item()
		{
		}
		/// <summary>
		///  Serial number 
		/// </summary>
		/// <param name="no"></param>
		public Item(string no)
		{

		}
		/// <summary>
		///  Get an instance of 
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
				return new Items();
			}
		}
		#endregion

		#region   Public Methods 
		 
		#endregion
	}
	/// <summary>
	/// 
	/// </summary>
	public class Items:XmlEns
	{
		#region  Structure 
		/// <summary>
		///  Assessment rate data elements 
		/// </summary>
		public Items(){}
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new Item();
			}
		}
		public override string File
		{
			get
			{
				return  SystemConfig.PathOfXML+"\\Menu.xml";
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
				return null; //new BP.ZF1.Items();
			}
		}
		#endregion
		 
	}
}
