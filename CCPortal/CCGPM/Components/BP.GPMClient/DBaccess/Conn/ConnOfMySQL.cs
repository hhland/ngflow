using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient ;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace CCPortal.DA
{
    /// <summary>
    /// MySqlConnection
    /// </summary>
    public class ConnOfMySQL : ConnBase
    {
        public new MySqlConnection Conn = null;
        public ConnOfMySQL()
        {
        }
    }
    /// <summary>
    /// SqlConnections
    /// </summary>
    public class ConnOfMySQLs : System.Collections.CollectionBase
    {
        /// <summary>
        ///  MySqlConnections
        /// </summary>
        public ConnOfMySQLs()
        {
        }
        public void Add(ConnOfMySQL conn)
        {
            this.InnerList.Add(conn);
        }
        /// <summary>
        ///  初始化
        /// </summary>
        public void Init()
        {
            for (int i = 0; i <= 3; i++)
            {
                ConnOfMySQL conn = new ConnOfMySQL();
                conn.IDX = i;
                this.Add(conn);
            }
        }
        public bool isLock = false;
        public ConnOfMySQL GetOne()
        {
            while (isLock)
            {
                ;
            }
            isLock = true;
            foreach (ConnOfMySQL conn in this)
            {
                if (conn.IsUsing == false)
                {
                    if (conn.Conn == null)
                        conn.Conn = new MySqlConnection(SystemConfig.AppCenterDSN);
                    conn.Times++;
                    conn.IsUsing = true;
                    isLock = false;
                    return conn;
                }
            }

            //如果没有新的连接。
            ConnOfMySQL nconn = new ConnOfMySQL();
            nconn.IDX = this.Count;
            nconn.Conn = new MySqlConnection(SystemConfig.AppCenterDSN);
            nconn.IsUsing = true;
            this.InnerList.Add(nconn);
            isLock = false;
            return nconn;
        }
        /// <summary>
        /// PutPool
        /// </summary>
        /// <param name="conn"></param>
        public void PutPool(ConnBase conn)
        {
            conn.IsUsing = false;
            this.InnerList[conn.IDX] = conn;
        }
    }
}
