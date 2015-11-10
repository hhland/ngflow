using System;
using System.Data.SqlClient;

namespace CCPortal.DA
{
	/// <summary>
	///　连接到哪个库上．
	///  他们存放在 web.config 的列表内．
	/// </summary>
	public enum DBUrlType
	{ 
		/// <summary>
		/// 主要的应用程序
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
		/// access的连接．
		/// </summary>
		DBAccessOfOLE,
		/// <summary>
		/// DBAccessOfODBC
		/// </summary>
		DBAccessOfODBC
	}
	/// <summary>
	/// DBUrl 的摘要说明。
	/// </summary>
	public class DBUrl
	{
		/// <summary>
		/// 连接
		/// </summary>
		public DBUrl()
		{
		} 
		/// <summary>
		/// 连接
		/// </summary>
		/// <param name="type">连接type</param>
		public DBUrl(DBUrlType type)
		{
			this.DBUrlType=type;
		}
		/// <summary>
		/// 默认值
		/// </summary>
		public DBUrlType  _DBUrlType=DBUrlType.AppCenterDSN;
		/// <summary>
		/// 要连接的到的库。
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
		/// 数据库类型
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
						throw new Exception("不明确的连接");
				}
			}
		}
	}
	
}
