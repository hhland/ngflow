/*
 Brief introduction : Responsible for data access class 
 Created :2002-10
 Last modification time :2002-10
*/
using System;
using System.Data;
using System.Diagnostics;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections; 
using System.Collections.Specialized;
using System.Web;
using BP.DA;
using BP.Pub;
using BP.Sys;


namespace BP.En
{
	public class EntityDBAccess
	{
		#region  The basic operation of the entity 
		/// <summary>
		///  Delete 
		/// </summary>
		/// <param name="en"></param>
		/// <returns></returns>
		public static int Delete(Entity en) 
		{
			if (en.EnMap.EnType==EnType.View)
				return 0;

			switch(en.EnMap.EnDBUrl.DBUrlType)
			{
				case DBUrlType.AppCenterDSN :
                    return DBAccess.RunSQL(en.SQLCash.Delete, SqlBuilder.GenerParasPK(en) );
				default :
					throw new Exception("@ There is no set type .");
			}
		}
        public static int Update(Entity en)
        {
            try
            {
                switch (en.EnMap.EnDBUrl.DBUrlType)
                {
                    case DBUrlType.AppCenterDSN:
                        switch (SystemConfig.AppCenterDBType)
                        {
                            case DBType.Oracle:
                                return DBAccess.RunSQL(en.SQLCash.Update, SqlBuilder.GenerParas(en,null) );
                            case DBType.Access:
                                return DBAccess.RunSQL(SqlBuilder.UpdateOfMSAccess(en, null));
                            default:
                                return DBAccess.RunSQL(SqlBuilder.Update(en, null));
                        }
                    case DBUrlType.DBAccessOfMSMSSQL:
                        return DBAccessOfMSMSSQL.RunSQL(SqlBuilder.Update(en, null));
                    case DBUrlType.DBAccessOfOracle:
                        return DBAccessOfOracle.RunSQL(SqlBuilder.Update(en, null));
                    default:
                        throw new Exception("@ There is no set type .");
                }
            }
            catch (Exception ex)
            {
                if (BP.Sys.SystemConfig.IsDebug)
                    en.CheckPhysicsTable();
                throw ex;
            }

        }
		/// <summary>
		///  Update 
		/// </summary>
		/// <param name="en"> To produce an updated statement </param>
		/// <param name="keys"> To update your property (null, Consider updating all )</param>
		/// <returns>sql</returns>
		public static int Update(Entity en, string[] keys)
		{
			if (en.EnMap.EnType==EnType.View)
				return 0;
            try
            {
                switch (en.EnMap.EnDBUrl.DBUrlType)
                {
                    case DBUrlType.AppCenterDSN:
                        switch (SystemConfig.AppCenterDBType)
                        {
                            case DBType.MSSQL:
                            case DBType.Oracle:
                            case DBType.MySQL:
                                return DBAccess.RunSQL(en.SQLCash.GetUpdateSQL(en, keys), SqlBuilder.GenerParas(en, keys));
                            case DBType.Informix:
                                return DBAccess.RunSQL(en.SQLCash.GetUpdateSQL(en, keys), SqlBuilder.GenerParas_Update_Informix(en, keys));
                            case DBType.Access:
                                return DBAccess.RunSQL(SqlBuilder.UpdateOfMSAccess(en, keys));
                            default:
                                //return DBAccess.RunSQL(en.SQLCash.GetUpdateSQL(en, keys),
                                //    SqlBuilder.GenerParas(en, keys));
                                if (keys != null)
                                {
                                    Paras ps = new Paras();
                                    Paras myps = SqlBuilder.GenerParas(en, keys);
                                    foreach (Para p in myps)
                                    {
                                        foreach (string s in keys)
                                        {
                                            if (s == p.ParaName)
                                            {
                                                ps.Add(p);
                                                break;
                                            }
                                        }
                                    }
                                    return DBAccess.RunSQL(en.SQLCash.GetUpdateSQL(en, keys), ps);
                                }
                                else
                                {
                                    return DBAccess.RunSQL(en.SQLCash.GetUpdateSQL(en, keys), 
                                        SqlBuilder.GenerParas(en, keys));
                                }
                                break;

                        }
                    case DBUrlType.DBAccessOfMSMSSQL:
                        return DBAccessOfMSMSSQL.RunSQL(SqlBuilder.Update(en, keys));
                    case DBUrlType.DBAccessOfOracle:

                        return DBAccessOfOracle.RunSQL(SqlBuilder.Update(en, keys));
                    default:
                        throw new Exception("@ There is no set type .");
                }
            }
            catch (Exception ex)
            {
                if (BP.Sys.SystemConfig.IsDebug)
                    en.CheckPhysicsTable();
                throw ex;
            }
		}
		/// <summary>
		///  Increase 
		/// </summary>
		/// <param name="en"></param>
		/// <returns></returns>
		public static int Insert_del(Entity en)
		{
			if (en.EnMap.EnType==EnType.Ext )
				throw new Exception("@ Entity ["+en.EnDesc+"] Is an extension of the type , Can not perform insert .");

			if (en.EnMap.EnType==EnType.View)
				throw new Exception("@ Entity ["+en.EnDesc+"] View type is , Can not perform insert .");

			try
			{
				switch(en.EnMap.EnDBUrl.DBUrlType)
				{
					case DBUrlType.AppCenterDSN :
						return DBAccess.RunSQL(SqlBuilder.Insert(en));
					case DBUrlType.DBAccessOfMSMSSQL :
						return DBAccessOfMSMSSQL.RunSQL(SqlBuilder.Insert(en));
					case DBUrlType.DBAccessOfOracle :
						return DBAccessOfOracle.RunSQL(SqlBuilder.Insert(en));
					default :
						throw new Exception("@ There is no set type .");
				}		 
			}
			catch(Exception ex)
			{
				en.CheckPhysicsTable(); //  Check the physical table .
				throw ex;
			}
		}
		#endregion 
	
		#region  The method generates a sequence number 
		 
		#endregion

        public static int RetrieveV2(Entity en, string sql,Paras paras)
        {
            try
            {
                DataTable dt = new DataTable();
                switch (en.EnMap.EnDBUrl.DBUrlType)
                {
                    case DBUrlType.AppCenterDSN:
                        dt = DBAccess.RunSQLReturnTable(sql, paras);
                        break;
                    case DBUrlType.DBAccessOfMSMSSQL:
                        dt = DBAccessOfMSMSSQL.RunSQLReturnTable(sql);
                        break;
                    case DBUrlType.DBAccessOfOracle:
                        dt = DBAccessOfOracle.RunSQLReturnTable(sql);
                        break;
                    default:
                        throw new Exception("@ Not Set DB Type .");
                }

                if (dt.Rows.Count == 0)
                    return 0;
                Attrs attrs = en.EnMap.Attrs;
                EntityDBAccess.fullDate(dt, en, attrs);
                int i = dt.Rows.Count;
                dt.Dispose();
                return i;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public static int Retrieve(Entity en, string sql, Paras paras)
        {

            DataTable dt ;
            switch (en.EnMap.EnDBUrl.DBUrlType)
            {
                case DBUrlType.AppCenterDSN:
                    dt = DBAccess.RunSQLReturnTable(sql, paras);
                    break;
                case DBUrlType.DBAccessOfMSMSSQL:
                    dt = DBAccessOfMSMSSQL.RunSQLReturnTable(sql);
                    break;
                case DBUrlType.DBAccessOfOracle:
                    dt = DBAccessOfOracle.RunSQLReturnTable(sql);
                    break;
                default:
                    throw new Exception("@ Not Set DB Type .");
            }

            if (dt.Rows.Count == 0)
                return 0;
            Attrs attrs = en.EnMap.Attrs;
            EntityDBAccess.fullDate(dt, en, attrs);
            int i = dt.Rows.Count;
            dt.Dispose();
            return i;
        }
		/// <summary>
		///  Inquiry 
		/// </summary>
		/// <param name="en"> Entity </param>
		/// <param name="sql"> Organization query </param>
		/// <returns></returns>
        public static int Retrieve(Entity en, string sql)
        {
            try
            {
                DataTable dt = new DataTable();
                switch (en.EnMap.EnDBUrl.DBUrlType)
                {
                    case DBUrlType.AppCenterDSN:
                        dt = DBAccess.RunSQLReturnTable(sql);
                        break;
                    case DBUrlType.DBAccessOfMSMSSQL:
                        dt = DBAccessOfMSMSSQL.RunSQLReturnTable(sql);
                        break;
                    case DBUrlType.DBAccessOfOracle:
                        dt = DBAccessOfOracle.RunSQLReturnTable(sql);
                        break;
                    default:
                        throw new Exception("@ Not Set DB Type .");
                }

                if (dt.Rows.Count == 0)
                    return 0;
                Attrs attrs = en.EnMap.Attrs;
                EntityDBAccess.fullDate(dt, en, attrs);
                int i = dt.Rows.Count;
                dt.Dispose();
                return i;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
		private static void fullDate(DataTable dt, Entity en, Attrs attrs )
		{
            foreach (Attr attr in attrs)
            {
                en.Row.SetValByKey(attr.Key, dt.Rows[0][attr.Key]);
            }
		}
        public static int Retrieve(Entities ens, string sql)
        {
            try
            {
                DataTable dt = new DataTable();
                switch (ens.GetNewEntity.EnMap.EnDBUrl.DBUrlType)
                {
                    case DBUrlType.AppCenterDSN:
                        dt = DBAccess.RunSQLReturnTable(sql);
                        break;
                    case DBUrlType.DBAccessOfMSMSSQL:
                        dt = DBAccessOfMSMSSQL.RunSQLReturnTable(sql);
                        break;
                    case DBUrlType.DBAccessOfOracle:
                        dt = DBAccessOfOracle.RunSQLReturnTable(sql);
                        break;
                    case DBUrlType.DBAccessOfOLE:
                        dt = DBAccessOfOLE.RunSQLReturnTable(sql);
                        break;
                    default:
                        throw new Exception("@ Not Set DB Type .");
                }

                if (dt.Rows.Count == 0)
                    return 0;

                Map enMap = ens.GetNewEntity.EnMap;
                Attrs attrs = enMap.Attrs;

                //Entity  en1 = ens.GetNewEntity;
                foreach (DataRow dr in dt.Rows)
                {
                    Entity en = ens.GetNewEntity;
                    //Entity  en = en1.CreateInstance();
                    foreach (Attr attr in attrs)
                    {
                        en.Row.SetValByKey(attr.Key, dr[attr.Key]);
                    }
                    ens.AddEntity(en);
                }
                int i = dt.Rows.Count;
                dt.Dispose();
                return i;
                //return dt.Rows.Count;
            }
            catch (System.Exception ex)
            {
                // ens.GetNewEntity.CheckPhysicsTable();
                throw new Exception("@дк[" + ens.GetNewEntity.EnDesc + "] An error occurred while inquiries :" + ex.Message);
            }
        }
        public static int Retrieve(Entities ens, string sql, Paras paras, string[] fullAttrs)
        {
            DataTable dt =null;
            switch (ens.GetNewEntity.EnMap.EnDBUrl.DBUrlType)
            {
                case DBUrlType.AppCenterDSN:
                    dt = DBAccess.RunSQLReturnTable(sql, paras);
                    break;
                case DBUrlType.DBAccessOfMSMSSQL:
                    dt = DBAccessOfMSMSSQL.RunSQLReturnTable(sql);
                    break;
                case DBUrlType.DBAccessOfOracle:
                    dt = DBAccessOfOracle.RunSQLReturnTable(sql);
                    break;
                case DBUrlType.DBAccessOfOLE:
                    dt = DBAccessOfOLE.RunSQLReturnTable(sql);
                    break;
                default:
                    throw new Exception("@ Not Set DB Type .");
            }

            if (dt.Rows.Count == 0)
                return 0;

            // Set query .
            QueryObject.InitEntitiesByDataTable(ens, dt, fullAttrs);

            int i = dt.Rows.Count;
            dt.Dispose();
            return i;
            //return dt.Rows.Count;
        }
	}
	
}
