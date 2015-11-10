using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient ;
using System.Data;
using System.Data.Common;

namespace CCPortal.DA
{
    /// <summary>
    /// SqlConnection
    /// </summary>
    public class ConnOfSQL : ConnBase
    {
        public new SqlConnection Conn = null;
        public ConnOfSQL()
        {
        }
    }
    /// <summary>
    /// SqlConnections
    /// </summary>
    public class ConnOfSQLs : System.Collections.CollectionBase
    {
        public ConnOfSQL GetByID(int id)
        {
            foreach (ConnOfSQL conn in this)
                if (conn.IDX == id)
                    return conn;
            return null;
        }
        /// <summary>
        /// SqlConnections
        /// </summary>
        public ConnOfSQLs()
        {
        }
        public void Add(ConnOfSQL conn)
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
                ConnOfSQL conn = new ConnOfSQL();
                conn.IDX = i;
                this.Add(conn);
            }
        }
        public bool isLock = false;
        public ConnOfSQL GetOne()
        {
            while (isLock)
            {
                ;
            }
            isLock = true;
            foreach (ConnOfSQL conn in this)
            {
                if (conn.IsUsing == false)
                {
                    if (conn.Conn == null)
                        conn.Conn = new SqlConnection(SystemConfig.AppCenterDSN);
                    conn.Times++;
                    conn.IsUsing = true;
                    isLock = false;
                    return conn;
                }
            }

            //如果没有新的连接。
            ConnOfSQL nconn = new ConnOfSQL();
            nconn.IDX = this.Count;
            nconn.Conn = new SqlConnection(SystemConfig.AppCenterDSN);
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
