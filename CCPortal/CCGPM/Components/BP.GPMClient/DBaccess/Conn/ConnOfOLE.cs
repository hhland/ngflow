using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb ;
using System.Data;
using System.Data.Common;

namespace CCPortal.DA
{
    public class ConnOfOLE : ConnBase
    {
        public new OleDbConnection Conn = null;
        public ConnOfOLE()
        {
        }
    }
    public class ConnOfOLEs : System.Collections.CollectionBase
    {
        public ConnOfOLEs()
        {
        }
        public void Add(ConnOfOLE conn)
        {
            this.InnerList.Add(conn);
        }
        /// <summary>
        ///  初始化
        /// </summary>
        public void Init()
        {
            for (int i = 0; i <= 1; i++)
            {
                ConnOfOLE conn = new ConnOfOLE();
                conn.IDX = i;
                this.Add(conn);
            }
        }
        public bool isLock = false;
        public ConnOfOLE GetOne()
        {
            //if (this.Count == 0)
            //    this.Init();
            while (isLock)
            {
                ;
            }

            isLock = true;
            foreach (ConnOfOLE conn in this)
            {
                if (conn.IsUsing == false)
                {
                    if (conn.Conn == null)
                        conn.Conn = new OleDbConnection(SystemConfig.AppCenterDSN);
                    conn.Times++;
                    conn.IsUsing = true;
                    isLock = false;
                    return conn;
                }
            }

            // 如果没有新的连接。
            ConnOfOLE nconn = new ConnOfOLE();
            nconn.IDX = this.Count;
            nconn.Conn = new OleDbConnection(SystemConfig.AppCenterDSN);
            this.InnerList.Add(nconn);
            isLock = false;
            return nconn;
            //throw new Exception("没有可用的连接了，请报告给管理员。");
        }
        /// <summary>
        /// PutPool
        /// </summary>
        /// <param name="conn"></param>
        public void PutPool(ConnBase conn)
        {
            conn.Close();
            conn.IsUsing = false;
            this.InnerList[conn.IDX] = conn;
        }
    }
}
