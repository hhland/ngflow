using System;
using System.Collections;
using System.Data;
using BP.DA;
using BP.En;
using BP.XML;
namespace BP.HTTP.Xml
{
	/// <summary>
	///  Property 
	/// </summary>
    public class RunSQLAttr
    {
        public const string Url = "Url";
        public const string FK_Img = "FK_Img";
        public const string Para = "Para";
        public const string ValType = "ValType";
        public const string Val = "Val";
        public const string Encode = "Encode";
    }
	/// <summary>
	/// AD  The summary .
	///  Number of assessment data elements 
	/// 1, It is  AD  One detail .
	/// 2, It represents a data element .
	/// </summary>
	public class RunSQL:XmlEnNoName
	{
		#region  Structure 
		public RunSQL()
		{
		}
		/// <summary>
		///  Get an instance of 
		/// </summary>
        public override XmlEns GetNewEntities
        {
            get
            {
                return new RunSQLs();
            }
        }
		#endregion
	}
	/// <summary>
    /// RunSQL
	/// </summary>
    public class RunSQLs : XmlEns
    {
        #region  Structure 
        /// <summary>
        ///  Number of assessment data elements 
        /// </summary>
        public RunSQLs() { }
     
        #endregion

        #region  Override the base class property or method .
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override XmlEn GetNewEntity
        {
            get
            {
                return new RunSQL();
            }
        }
        /// <summary>
        ///  File 
        /// </summary>
        public override string File
        {
            get
            {
                return SystemConfig.PathOfWebApp + "\\RunSQL\\RunSQL.xml";
            }
        }
        /// <summary>
        ///  Physical table name 
        /// </summary>
        public override string TableName
        {
            get
            {
                return "RunSQL";
            }
        }
        
        #endregion
    }
}
