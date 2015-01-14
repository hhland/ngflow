using System;
using System.Collections.Generic;
using System.Text;

namespace BP.DA
{
    /// <summary>
    ///  Database Deployment Type 
    /// </summary>
    public enum DBModel
    {
        /// <summary>
        ///  Independent （ Centralized model ）
        /// </summary>
        Single=0,
        /// <summary>
        ///  Domain mode 
        /// </summary>
        Domain=1
    }
    /// <summary>
    ///  Database Type 
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
    ///  Storage location 
    /// </summary>
    public enum Depositary
    {
         /// <summary>
        ///  No custody 
        /// </summary>
        None,
        /// <summary>
        ///  All 
        /// </summary>
        Application        
    }
    /// <summary>
    ///  Chart Type 
    /// </summary>
    public enum ChartType
    {
        /// <summary>
        ///  Histogram 
        /// </summary>
        Histogram,
        /// <summary>
        ///  Propyl-like figure 
        /// </summary>
        Pie,
        /// <summary>
        ///  Line chart 
        /// </summary>
        Line
    }
    /// <summary>
    ///  Grouping 
    /// </summary>
    public enum GroupWay
    {
        /// <summary>
        ///  Seek cooperation 
        /// </summary>
        BySum,
        /// <summary>
        ///  Averaging 
        /// </summary>
        ByAvg
    }
    /// <summary>
    ///  Sort by 
    /// </summary>
    public enum OrderWay
    {
        /// <summary>
        ///  Ascending 
        /// </summary>
        OrderByUp,
        /// <summary>
        ///  Descending 
        /// </summary>
        OrderByDown
    }
    /// <summary>
    ///  Check the level of data 
    /// </summary>
    public enum DBCheckLevel
    {
        /// <summary>
        /// 低, Only the report , Not operate any data 
        /// </summary>
        Low = 1,
        /// <summary>
        /// 中, The inspection report , Delete the space around the foreign key .
        /// </summary>
        Middle = 2,
        /// <summary>
        /// 高, Do not delete the corresponding data on .
        /// </summary>
        High = 3,
    }
}
