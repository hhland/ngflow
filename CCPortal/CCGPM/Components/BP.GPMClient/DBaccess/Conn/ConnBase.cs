using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OracleClient ;
using System.Data;
using System.Data.Common;

namespace CCPortal.DA
{
    #region 连接基类
    public abstract class ConnBase
    {
        public void DoClearSQL()
        {
            this.SQLID = 0;
            this.SQLs = new string[1000];
        }
        public int SQLID = 0;
        public string[] SQLs = null;
        public void AddSQL(string sql)
        {

            return;

            if (SystemConfig.IsOpenSQLCheck == false)
                return;

            if (SQLs == null)
                SQLs = new string[1000];

            if (this.SQLID == 1000)
            {
                this.DoClearSQL();
            }

            this.SQLs.SetValue(sql, this.SQLID);
            this.SQLID++;

            sql = sql.Replace("'", "^");

            //try
            //{
            //    string insert = "insert into Sys_SQL (No) valueS ('" + sql + "')";
            //    CCPortal.DA.DBAccessOfOracle.RunSQL(insert);
            //}
            //catch
            //{
            //    try
            //    {
            //        string update = "UPDATE Sys_SQL SET UTime=UTime+1 WHERE NO='" + sql + "'";
            //        CCPortal.DA.DBAccessOfOracle.RunSQL(update);
            //    }
            //    catch
            //    {
            //    }
            //}
        }
        public bool IsUsing = false;
        public int IDX = 0;
        public int Times = 0;
        public virtual DbConnection Conn
        {
            get { return null; }
        }
        public ConnBase()
        {
        }
        public virtual void Close()
        {

        }
    }
    public abstract class ConnBases : System.Collections.CollectionBase
    {
        public ConnBases()
        {
        }
        public virtual ConnBase GetOne()
        {
            return null;
        }
        public virtual void PubPool(ConnBase conn)
        {
        }
        public void CheckIt()
        {
            
        }
    }
    #endregion ConnBase
}
