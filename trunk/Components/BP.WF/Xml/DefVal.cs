using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.XML;
using BP.Sys;

namespace BP.WF.XML
{
    /// <summary>
    ///  Defaults 
    /// </summary>
	public class DefVal:XmlEnNoName
	{
		#region  Property 
        public string Val
        {
            get
            {
                return this.GetValStringByKey("Val");
            }
        }
		#endregion

		#region  Structure 
		/// <summary>
		///  Node extended information 
		/// </summary>
		public DefVal()
		{
		}
		/// <summary>
		///  Get an instance of 
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
				return new DefVals();
			}
		}
		#endregion
	}
	/// <summary>
    ///  Defaults s
	/// </summary>
	public class DefVals:XmlEns
	{
		#region  Structure 
        
		/// <summary>
		///  Assessment rate data elements 
		/// </summary>
        public DefVals() { }
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new DefVal();
			}
		}
        public override string File
        {
            get
            {
                return SystemConfig.PathOfData + "\\Xml\\DefVal.xml";
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
				return null; //new BP.ZF1.AdminDefVals();
			}
		}
		#endregion
		 
	}
}
