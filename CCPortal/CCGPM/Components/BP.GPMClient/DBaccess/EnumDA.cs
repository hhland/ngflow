using System;
using System.Collections.Generic;
using System.Text;

namespace CCPortal.DA
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DBType
    {
        /// <summary>
        /// sqlserver
        /// </summary>
        MSSQL,
        /// <summary>
        /// oracle  
        /// </summary>
        Oracle,
        /// <summary>
        /// Access
        /// </summary>
        Access,
        /// <summary>
        /// Sybase
        /// </summary>
        Sybase,
        /// <summary>
        /// DB2
        /// </summary>
        DB2,
        /// <summary>
        /// MySQL
        /// </summary>
        MySQL,
        /// <summary>
        /// Informix
        /// </summary>
        Informix
    }
    /// <summary>
    /// 保管位置
    /// </summary>
    public enum Depositary
    {
         /// <summary>
        /// 不保管
        /// </summary>
        None,
        /// <summary>
        /// 全体
        /// </summary>
        Application        
    }
}
