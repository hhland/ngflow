using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.XML;
using BP.Sys;


namespace BP.Web.Port.Xml
{
	public class AdminToolAttr
	{
		/// <summary>
		///  Serial number 
		/// </summary>
		public const string ICON="ICON";
		/// <summary>
		///  Name 
		/// </summary>
		public const string Name="Name";
		/// <summary>
		/// Url
		/// </summary>
		public const string Url="Url";
		/// <summary>
		/// DESC
		/// </summary>
		public const string DESC="DESC";
		/// <summary>
		/// 
		/// </summary>
		public const string Enable="Enable";
	}
	public class AdminTool:XmlEn
	{
		#region  Property 
		public string Enable
		{
			get
			{
				return this.GetValStringByKey(AdminToolAttr.Enable);
			}
		}
		public string Url
		{
			get
			{
				return this.GetValStringByKey(AdminToolAttr.Url);
			}
		}
		public string DESC
		{
			get
			{
				return this.GetValStringByKey(AdminToolAttr.DESC);
			}
		}
		/// <summary>
		///  Serial number 
		/// </summary>
		public string ICON
		{
			get
			{
				return this.GetValStringByKey(AdminToolAttr.ICON);
			}
		}
		/// <summary>
		///  Name 
		/// </summary>
		public string Name
		{
			get
			{
				return this.GetValStringByKey(AdminToolAttr.Name);
			}
		}
		#endregion

		#region  Structure 
		public AdminTool()
		{
		}
		/// <summary>
		///  Serial number 
		/// </summary>
		/// <param name="no"></param>
		public AdminTool(string no)
		{

		}
		/// <summary>
		///  Get an instance of 
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
				return new AdminTools();
			}
		}
		#endregion

		#region   Public Methods 
		 
		#endregion
	}
	/// <summary>
	/// 
	/// </summary>
	public class AdminTools:XmlEns
	{
		#region  Structure 
		/// <summary>
		///  Assessment rate data elements 
		/// </summary>
		public AdminTools(){}
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new AdminTool();
			}
		}
		public override string File
		{
			get
			{
				return  SystemConfig.PathOfXML+"\\AdminTools.xml";
			}
		}
		/// <summary>
		///  Physical table name 
		/// </summary>
		public override string TableName
		{
			get
			{
				return "AdminTool";
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
