using System;
using System.Data.SqlClient;

namespace BP.DA
{
	/// <summary>
	///бб Which library is connected to the го
	///   They kept in  web.config  The list of in го
	/// </summary>
	public enum DBUrlType
	{ 
		/// <summary>
		///  Main application 
		/// </summary>
		AppCenterDSN,
		/// <summary>
		/// DBAccessOfOracle
		/// </summary>
		DBAccessOfOracle,
		/// <summary>
		/// DBAccessOfOracle1
		/// </summary>
		DBAccessOfOracle1,
		/// <summary>
		/// DBAccessOfMSMSSQL
		/// </summary>
		DBAccessOfMSMSSQL,
		/// <summary>
		/// access Connection го
		/// </summary>
		DBAccessOfOLE,
		/// <summary>
		/// DBAccessOfODBC
		/// </summary>
		DBAccessOfODBC
	}
	/// <summary>
	/// DBUrl  The summary .
	/// </summary>
	public class DBUrl
	{
		/// <summary>
		///  Connection 
		/// </summary>
		public DBUrl()
		{
		} 
		/// <summary>
		///  Connection 
		/// </summary>
		/// <param name="type"> Connection type</param>
		public DBUrl(DBUrlType type)
		{
			this.DBUrlType=type;
		}
		/// <summary>
		///  Defaults 
		/// </summary>
		public DBUrlType  _DBUrlType=DBUrlType.AppCenterDSN;
		/// <summary>
		///  Library to connect to the .
		/// </summary>
		public DBUrlType DBUrlType
		{
			get
			{
				return _DBUrlType;
			}
			set
			{
				_DBUrlType=value;
			}
		}
        public string DBVarStr
        {
            get
            {
                switch (this.DBType)
                {
                    case DBType.Oracle:
                        return ":";
                    case DBType.MySQL:
                    case DBType.MSSQL:
                        return "@";
                    case DBType.Informix:
                        return "?";
                    default:
                        return "@";
                }
            }
        }
		/// <summary>
		///  Database Type 
		/// </summary>
		public DBType DBType
		{
			get
			{
				switch(this.DBUrlType)
				{
					case DBUrlType.AppCenterDSN:
						return DBAccess.AppCenterDBType ; 
					case DBUrlType.DBAccessOfMSMSSQL:
						return DBType.MSSQL;
					case DBUrlType.DBAccessOfOLE:
						return DBType.Access;
					case DBUrlType.DBAccessOfOracle1:
                    case DBUrlType.DBAccessOfOracle:
						return DBType.Oracle ;
					default:
						throw new Exception(" Ambiguous connection ");
				}
			}
		}
	}
	
}
