using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.XML;
using BP.Sys;
using BP.Sys;

namespace BP.WF.XML
{
    /// <summary>
    ///  Mode 
    /// </summary>
	public class ModeSortXml:XmlEnNoName
	{
		#region  Structure 
		/// <summary>
		///  Node extended information 
		/// </summary>
        public ModeSortXml()
		{
		}
		/// <summary>
		///  Get an instance of 
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
                return new ModeSortXmls();
			}
		}
		#endregion
	}
	/// <summary>
    ///  Mode s
	/// </summary>
	public class ModeSortXmls:XmlEns
	{
		#region  Structure 
		/// <summary>
        ///  Mode s
		/// </summary>
        public ModeSortXmls() { }
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new ModeSortXml();
			}
		}
        public override string File
        {
            get
            {
                return SystemConfig.PathOfWebApp + "\\WF\\Admin\\AccepterRole\\AccepterRole.xml";

            }
        }
		/// <summary>
		///  Physical table name 
		/// </summary>
		public override string TableName
		{
			get
			{
                return "ModelSort";
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
