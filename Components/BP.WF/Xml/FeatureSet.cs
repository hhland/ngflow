using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.XML;
using BP.Sys;

namespace BP.WF.XML
{
	/// <summary>
	///  Personalize 
	/// </summary>
	public class FeatureSet:XmlEnNoName
	{
		#region  Property 
        public new string Name
        {
            get
            {
                return this.GetValStringByKey(BP.Web.WebUser.SysLang);
            }
        }
		#endregion

		#region  Structure 
		/// <summary>
		///  Node extended information 
		/// </summary>
		public FeatureSet()
		{
		}
        public FeatureSet(string no)
        {
            
        }
		/// <summary>
		///  Get an instance of 
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
				return new FeatureSets();
			}
		}
		#endregion
	}
	/// <summary>
    ///  Personalize s
	/// </summary>
	public class FeatureSets:XmlEns
	{
		#region  Structure 
		/// <summary>
		///  Assessment rate data elements 
		/// </summary>
        public FeatureSets() { }
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new FeatureSet();
			}
		}
		public override string File
		{
			get
			{
                return SystemConfig.PathOfXML + "FeatureSet.xml";
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
