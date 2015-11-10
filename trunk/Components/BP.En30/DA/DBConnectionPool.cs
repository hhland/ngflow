using BP.En30.En;
using BP.Sys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BP.En30.DA
{
    public class DBConnectionPool : ObjectPool
    {

        protected DBConnectionPool() { }

        public static readonly DBConnectionPool Instance =
            new DBConnectionPool();

        protected static string connectionString =SystemConfig.AppCenterDSN;

        public static string ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }

        protected override object Create()
        {
            DbConnection conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }

        protected override bool Validate(object o)
        {
            try
            {
                DbConnection conn = (DbConnection)o;
                return !conn.State.Equals(ConnectionState.Closed);
            }
            catch (SqlException)
            {
                return false;
            }
        }

        protected override void Expire(object o)
        {
            try
            {
                DbConnection conn = (DbConnection)o;
                conn.Close();
            }
            catch (SqlException) { }
        }

        public DbConnection BorrowDBConnection()
        {
            try
            {
                return (DbConnection)base.GetObjectFromPool();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ReturnDBConnection(DbConnection conn)
        {
            base.ReturnObjectToPool(conn);
        }

    }

    public class DBOraConnectionPool : DBConnectionPool {

        private DBOraConnectionPool() {
        }

        public static readonly DBOraConnectionPool Instance =
            new DBOraConnectionPool();

        protected override object Create()
        {
            OracleConnection conn= new OracleConnection(SystemConfig.AppCenterDSN);
            conn.Open();
            return conn;
        }

    }
}
