using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.Sys;
using BP.XML;

namespace BP.WF.XML
{
    /// <summary>
    ///  Mode 
    /// </summary>
	public class ModeXml:XmlEnNoName
    {
        #region  Property 
        /// <summary>
        ///  Setting Description 
        /// </summary>
        public string SetDesc
        {
            get
            {
                return this.GetValStringByKey("SetDesc");
            }
        }
        /// <summary>
        ///  Category 
        /// </summary>
        public string FK_ModeSort
        {
            get
            {
                return this.GetValStringByKey("FK_ModeSort");
            }
        }
        /// <summary>
        ///  Category 
        /// </summary>
        public string Note
        {
            get
            {
                return this.GetValStringByKey("Note");
            }
        }
        public string ParaType
        {
            get
            {
                return this.GetValStringByKey("ParaType");
            }
        }
        #endregion
      
		#region  Structure 
		/// <summary>
		///  Mode 
		/// </summary>
        public ModeXml()
		{
		}
        /// <summary>
        ///  Mode 
        /// </summary>
        public ModeXml(string no):base(no)
        {
        }
		/// <summary>
		///  Get an instance of 
		/// </summary>
		public override XmlEns GetNewEntities
		{
			get
			{
                return new ModeXmls();
			}
		}
		#endregion
	}
	/// <summary>
    ///  Mode s
	/// </summary>
	public class ModeXmls:XmlEns
	{
		#region  Structure 
		/// <summary>
        ///  Skin s
		/// </summary>
        public ModeXmls() { }
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new ModeXml();
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
                return "Model";
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
