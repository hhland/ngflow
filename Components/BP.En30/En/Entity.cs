using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using BP.DA;
using System.Data;
using BP.Sys;
using BP.En;
using System.Reflection;

namespace BP.En
{
    /// <summary>
    /// Entity  The summary .
    /// </summary>	
    [Serializable]
    abstract public class Entity : EnObj
    {
        #region  And caching related operations 
        private Entities _GetNewEntities = null;
        public virtual Entities GetNewEntities
        {
            get
            {
                if (_GetNewEntities == null)
                {
                    string str = this.ToString();
                    ArrayList al = BP.En.ClassFactory.GetObjects("BP.En.Entities");
                    foreach (Object o in al)
                    {
                        Entities ens = o as Entities;

                        if (ens == null)
                            continue;
                        if (ens.GetNewEntity.ToString() == str)
                        {
                            _GetNewEntities = ens;
                            return _GetNewEntities;
                        }
                    }
                    throw new Exception("@no ens" + this.ToString());
                }
                return _GetNewEntities;
            }
        }
        protected virtual string CashKey_Del
        {
            get
            {
                return null;
            }
        }
        public virtual string ClassID
        {
            get
            {
                return this.ToString();
            }
        }
        #endregion

        #region 与sql Related to the operation 
        protected SQLCash _SQLCash = null;
        public virtual SQLCash SQLCash
        {
            get
            {
                if (_SQLCash == null)
                {
                    _SQLCash = BP.DA.Cash.GetSQL(this.ToString());
                    if (_SQLCash == null)
                    {
                        _SQLCash = new SQLCash(this);
                        BP.DA.Cash.SetSQL(this.ToString(), _SQLCash);
                    }
                }
                return _SQLCash;
            }
            set
            {
                _SQLCash = value;
            }
        }
        /// <summary>
        ///  Converted into 
        /// </summary>
        /// <returns></returns>
        public string ToStringAtParas()
        {
            string str = "";
            foreach (Attr attr in this.EnMap.Attrs)
            {
                str += "@" + attr.Key + "=" + this.GetValByKey(attr.Key);
            }
            return str;
        }
        public DataTable ToDataTableField(string tableName)
        {
            DataTable dt = this.GetNewEntities.ToEmptyTableField();
            dt.TableName = tableName;

            DataRow dr = dt.NewRow();
            foreach (Attr attr in this.EnMap.Attrs)
            {
                if (attr.MyDataType == DataType.AppBoolean)
                {
                    if (this.GetValIntByKey(attr.Key) == 1)
                        dr[attr.Key] = "1";
                    else
                        dr[attr.Key] = "0";
                    continue;
                }

                /* If it is a foreign key   We must remove the spaces around .
                 *  */
                if (attr.MyFieldType == FieldType.FK
                    || attr.MyFieldType == FieldType.PKFK)
                    dr[attr.Key] = this.GetValByKey(attr.Key).ToString().Trim();
                else
                    dr[attr.Key] = this.GetValByKey(attr.Key);
            }
            dt.Rows.Add(dr);
            return dt;
        }
        #endregion

        #region  With respect to database  Operating 
        public int RunSQL(string sql)
        {
            Paras ps = new Paras();
            ps.SQL = sql;
            return this.RunSQL(ps);
        }
        /// <summary>
        ///  In this entity is run sql  Return result sets 
        /// </summary>
        /// <param name="sql"> To run sql</param>
        /// <returns> Implementation of the results </returns>
        public int RunSQL(Paras ps)
        {
            switch (this.EnMap.EnDBUrl.DBUrlType)
            {
                case DBUrlType.AppCenterDSN:
                    return DBAccess.RunSQL(ps);
                case DBUrlType.DBAccessOfMSMSSQL:
                    return DBAccessOfMSMSSQL.RunSQL(ps.SQL);
                case DBUrlType.DBAccessOfOracle:
                    return DBAccessOfOracle.RunSQL(ps.SQL);
                default:
                    throw new Exception("@ There is no set type .");
            }
        }
        public int RunSQL(string sql, Paras paras)
        {
            switch (this.EnMap.EnDBUrl.DBUrlType)
            {
                case DBUrlType.AppCenterDSN:
                    return DBAccess.RunSQL(sql, paras);
                case DBUrlType.DBAccessOfMSMSSQL:
                    return DBAccessOfMSMSSQL.RunSQL(sql);
                case DBUrlType.DBAccessOfOracle:
                    return DBAccessOfOracle.RunSQL(sql);
                default:
                    throw new Exception("@ There is no set type .");
            }
        }
        /// <summary>
        ///  In this entity is run sql  Return result sets 
        /// </summary>
        /// <param name="sql"> To run  select sql</param>
        /// <returns> Execution of the query results </returns>
        public DataTable RunSQLReturnTable(string sql)
        {
            switch (this.EnMap.EnDBUrl.DBUrlType)
            {
                case DBUrlType.AppCenterDSN:
                    return DBAccess.RunSQLReturnTable(sql);
                case DBUrlType.DBAccessOfMSMSSQL:
                    return DBAccessOfMSMSSQL.RunSQLReturnTable(sql);
                case DBUrlType.DBAccessOfOracle:
                    return DBAccessOfOracle.RunSQLReturnTable(sql);
                default:
                    throw new Exception("@ There is no set type .");
            }
        }
        #endregion

        #region  Operating on the details of 
        public Entities GetEnsDaOfOneVSM(AttrOfOneVSM attr)
        {
            Entities ensOfMM = attr.EnsOfMM;
            Entities ensOfM = attr.EnsOfM;
            ensOfM.Clear();

            QueryObject qo = new QueryObject(ensOfMM);
            qo.AddWhere(attr.AttrOfOneInMM, this.PKVal.ToString());
            qo.DoQuery();

            foreach (Entity en in ensOfMM)
            {
                Entity enOfM = ensOfM.GetNewEntity;
                enOfM.PKVal = en.GetValStringByKey(attr.AttrOfMInMM);
                enOfM.Retrieve();
                ensOfM.AddEntity(enOfM);
            }
            return ensOfM;
        }
        /// <summary>
        ///  Collection of entities acquired entity set-many .
        /// </summary>
        /// <param name="ensOfMMclassName"> The class name of the entity sets </param>
        /// <returns> Data entities </returns>
        public Entities GetEnsDaOfOneVSM(string ensOfMMclassName)
        {
            AttrOfOneVSM attr = this.EnMap.GetAttrOfOneVSM(ensOfMMclassName);

            return GetEnsDaOfOneVSM(attr);
        }
        public Entities GetEnsDaOfOneVSMFirst()
        {
            AttrOfOneVSM attr = this.EnMap.AttrsOfOneVSM[0];
            //	throw new Exception("err "+attr.Desc); 
            return this.GetEnsDaOfOneVSM(attr);
        }
        #endregion

        #region  Operating on the details of 
        /// <summary>
        ///  Get his data entities 
        /// </summary>
        /// <param name="EnsName"> The class name </param>
        /// <returns></returns>
        public Entities GetDtlEnsDa(string EnsName)
        {
            Entities ens = ClassFactory.GetEns(EnsName);
            return GetDtlEnsDa(ens);
            /*
            EnDtls eds =this.EnMap.Dtls; 
            foreach(EnDtl ed in eds)
            {
                if (ed.EnsName==EnsName)
                {
                    Entities ens =ClassFactory.GetEns(EnsName) ; 
                    QueryObject qo = new QueryObject(ClassFactory.GetEns(EnsName));
                    qo.AddWhere(ed.RefKey,this.PKVal.ToString());
                    qo.DoQuery();
                    return ens;
                }
            }
            throw new Exception("@ Entity ["+this.EnDesc+"], Does not contain "+EnsName);	
            */
        }
        /// <summary>
        ///  Out his data entity 
        /// </summary>
        /// <param name="ens"> Set </param>
        /// <returns> Entity information after execution </returns>
        public Entities GetDtlEnsDa(Entities ens)
        {
            foreach (EnDtl dtl in this.EnMap.Dtls)
            {
                if (dtl.Ens.GetType() == ens.GetType())
                {
                    QueryObject qo = new QueryObject(dtl.Ens);
                    qo.AddWhere(dtl.RefKey, this.PKVal.ToString());
                    qo.DoQuery();
                    return dtl.Ens;
                }
            }
            throw new Exception("@ In taking [" + this.EnDesc + "] The details of an error .[" + ens.GetNewEntity.EnDesc + "], He not in the collection .");
        }

        public Entities GetDtlEnsDa(EnDtl dtl)
        {

            try
            {
                QueryObject qo = new QueryObject(dtl.Ens);
                qo.AddWhere(dtl.RefKey, this.PKVal.ToString());
                qo.DoQuery();
                return dtl.Ens;
            }
            catch (Exception)
            {
                throw new Exception("@ In taking [" + this.EnDesc + "] The details of an error .[" + dtl.Desc + "], He not in the collection .");
            }
        }

        //		/// <summary>
        //		///  Returns the first entity 
        //		/// </summary>
        //		/// <returns> Returns the first entity , If you did not throw an exception </returns>
        //		public Entities GetDtl()
        //		{
        //			 return this.GetDtls(0);
        //		}
        //		/// <summary>
        //		///  Returns the first entity 
        //		/// </summary>
        //		/// <returns> Returns the first entity </returns>
        //		public Entities GetDtl(int index)
        //		{
        //			try
        //			{
        //				return this.GetDtls(this.EnMap.Dtls[index].Ens);
        //			}
        //			catch( Exception ex)
        //			{
        //				throw new Exception("@ In obtaining the order to take ["+this.EnDesc+"] The details , Error :"+ex.Message);
        //			}			 
        //		}
        /// <summary>
        ///  Remove the details of his collection .
        /// </summary>
        /// <returns></returns>
        public System.Collections.ArrayList GetDtlsDatasOfArrayList()
        {
            ArrayList al = new ArrayList();
            foreach (EnDtl dtl in this.EnMap.Dtls)
            {
                al.Add(this.GetDtlEnsDa(dtl.Ens));
            }
            return al;
        }

        public List<Entities> GetDtlsDatasOfList()
        {
            List<Entities> al = new List<Entities>();
            foreach (EnDtl dtl in this.EnMap.Dtls)
            {
                al.Add(this.GetDtlEnsDa(dtl));
            }
            return al;
        }
        #endregion

        #region  Checks if a property value exists in the entity set 
        /// <summary>
        ///  Checks if a property value exists in the entity set 
        ///  This method is often used in beforeinsert中.
        /// </summary>
        /// <param name="key"> To check the key.</param>
        /// <param name="val"> To check the key. Corresponding val</param>
        /// <returns></returns>
        protected int ExitsValueNum(string key, string val)
        {
            string field = this.EnMap.GetFieldByKey(key);
            Paras ps = new Paras();
            ps.Add("p", val);

            string sql = "SELECT COUNT( " + key + " ) FROM " + this.EnMap.PhysicsTable + " WHERE " + key + "=" + this.HisDBVarStr + "p";
            return int.Parse(DBAccess.RunSQLReturnVal(sql, ps).ToString());
        }
        #endregion

        #region  Numbers are related to treatment .
        /// <summary>
        ///  This method is not hierarchical dictionary , Generate a number . According to the formulation of   Property .
        /// </summary>
        /// <param name="attrKey"> Property </param>
        /// <returns> Generated numbers </returns> 
        public string GenerNewNoByKey(string attrKey)
        {
            try
            {
                string sql = null;
                Attr attr = this.EnMap.GetAttrByKey(attrKey);
                if (attr.UIIsReadonly == false)
                    throw new Exception("@ Column needs to automatically generate numbers (" + attr.Key + ") Must be read only .");

                string field = this.EnMap.GetFieldByKey(attrKey);
                switch (this.EnMap.EnDBUrl.DBType)
                {
                    case DBType.MSSQL:
                        sql = "SELECT CONVERT(INT, MAX(CAST(" + field + " as int)) )+1 AS No FROM " + this._enMap.PhysicsTable;
                        break;
                    case DBType.Oracle:
                    case DBType.MySQL:
                        sql = "SELECT MAX(" + field + ") +1 AS No FROM " + this._enMap.PhysicsTable;
                        break;
                    case DBType.Informix:
                        sql = "SELECT MAX(" + field + ") +1 AS No FROM " + this._enMap.PhysicsTable;
                        break;
                    case DBType.Access:
                        sql = "SELECT MAX( [" + field + "]) +1 AS  No FROM " + this._enMap.PhysicsTable;
                        break;
                    default:
                        throw new Exception("error");
                }
                string str = DBAccess.RunSQLReturnValInt(sql, 1).ToString();
                if (str == "0" || str == "")
                    str = "1";
                return str.PadLeft(int.Parse(this._enMap.CodeStruct), '0');
            }
            catch (Exception ex)
            {
                this.CheckPhysicsTable();
                throw ex;
            }
        }
        /// <summary>
        ///  Produced in accordance with the order of a column of numbers .
        /// </summary>
        /// <param name="attrKey"> Columns to be generated </param>
        /// <param name="attrGroupKey"> Grouping column names </param>
        /// <param name="FKVal"> Primary key grouping </param>
        /// <returns></returns>		
        public string GenerNewNoByKey(int nolength, string attrKey, string attrGroupKey, string attrGroupVal)
        {
            if (attrGroupKey == null || attrGroupVal == null)
                throw new Exception("@ Group field attrGroupKey attrGroupVal  Can not be empty ");

            Paras ps = new Paras();
            ps.Add("groupKey", attrGroupKey);
            ps.Add("groupVal", attrGroupVal);

            string sql = "";
            string field = this.EnMap.GetFieldByKey(attrKey);
            ps.Add("f", attrKey);

            switch (this.EnMap.EnDBUrl.DBType)
            {
                case DBType.MSSQL:
                    sql = "SELECT CONVERT(bigint, MAX([" + field + "]))+1 AS Num FROM " + this.EnMap.PhysicsTable + " WHERE " + attrGroupKey + "='" + attrGroupVal + "'";
                    break;
                case DBType.Oracle:
                case DBType.Informix:
                    sql = "SELECT MAX( :f )+1 AS No FROM " + this.EnMap.PhysicsTable + " WHERE " + this.HisDBVarStr + "groupKey=" + this.HisDBVarStr + "groupVal ";
                    break;
                case DBType.MySQL:
                    sql = "SELECT MAX(" + field + ") +1 AS Num FROM " + this.EnMap.PhysicsTable + " WHERE " + attrGroupKey + "='" + attrGroupVal + "'";
                    break;
                case DBType.Access:
                    sql = "SELECT MAX([" + field + "]) +1 AS Num FROM " + this.EnMap.PhysicsTable + " WHERE " + attrGroupKey + "='" + attrGroupVal + "'";
                    break;
                default:
                    throw new Exception("error");
            }

            DataTable dt = DBAccess.RunSQLReturnTable(sql, ps);
            string str = "1";
            if (dt.Rows.Count != 0)
            {
                //System.DBNull n = new DBNull();
                if (dt.Rows[0][0] is DBNull)
                    str = "1";
                else
                    str = dt.Rows[0][0].ToString();
            }
            return str.PadLeft(nolength, '0');
        }
        public string GenerNewNoByKey(string attrKey, string attrGroupKey, string attrGroupVal)
        {
            return this.GenerNewNoByKey(int.Parse(this.EnMap.CodeStruct), attrKey, attrGroupKey, attrGroupVal);
        }
        /// <summary>
        ///  According to two students check sequence number .
        /// </summary>
        /// <param name="attrKey"></param>
        /// <param name="attrGroupKey1"></param>
        /// <param name="attrGroupKey2"></param>
        /// <param name="attrGroupVal1"></param>
        /// <param name="attrGroupVal2"></param>
        /// <returns></returns>
        public string GenerNewNoByKey(string attrKey, string attrGroupKey1, string attrGroupKey2, object attrGroupVal1, object attrGroupVal2)
        {
            string f = this.EnMap.GetFieldByKey(attrKey);
            Paras ps = new Paras();
            //   ps.Add("f", f);

            string sql = "";
            switch (this.EnMap.EnDBUrl.DBType)
            {
                case DBType.Oracle:
                case DBType.Informix:
                    sql = "SELECT   MAX(" + f + ") +1 AS No FROM " + this.EnMap.PhysicsTable;
                    break;
                case DBType.MSSQL:
                    sql = "SELECT CONVERT(INT, MAX(" + this.EnMap.GetFieldByKey(attrKey) + ") )+1 AS No FROM " + this.EnMap.PhysicsTable + " WHERE " + this.EnMap.GetFieldByKey(attrGroupKey1) + "='" + attrGroupVal1 + "' AND " + this.EnMap.GetFieldByKey(attrGroupKey2) + "='" + attrGroupVal2 + "'";
                    break;
                case DBType.Access:
                    sql = "SELECT CONVERT(INT, MAX(" + this.EnMap.GetFieldByKey(attrKey) + ") )+1 AS No FROM " + this.EnMap.PhysicsTable + " WHERE " + this.EnMap.GetFieldByKey(attrGroupKey1) + "='" + attrGroupVal1 + "' AND " + this.EnMap.GetFieldByKey(attrGroupKey2) + "='" + attrGroupVal2 + "'";
                    break;
                default:
                    break;
            }

            DataTable dt = DBAccess.RunSQLReturnTable(sql, ps);
            string str = "1";
            if (dt.Rows.Count != 0)
                str = dt.Rows[0][0].ToString();
            return str.PadLeft(int.Parse(this.EnMap.CodeStruct), '0');
        }
        #endregion

        #region  Constructor 
        public Entity()
        {
        }
        #endregion

        #region  Sorting operation 
        protected void DoOrderUp(string groupKeyAttr, string groupKeyVal, string idxAttr)
        {
            //  string pkval = this.PKVal as string;
            string pkval = this.PKVal.ToString();
            string pk = this.PK;
            string table = this.EnMap.PhysicsTable;

            string sql = "SELECT " + pk + "," + idxAttr + " FROM " + table + " WHERE " + groupKeyAttr + "='" + groupKeyVal + "' ORDER BY " + idxAttr;
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            int idx = 0;
            string beforeNo = "";
            string myNo = "";
            bool isMeet = false;

            foreach (DataRow dr in dt.Rows)
            {
                idx++;
                myNo = dr[pk].ToString();
                if (myNo == pkval)
                    isMeet = true;

                if (isMeet == false)
                    beforeNo = myNo;
                DBAccess.RunSQL("UPDATE " + table + " SET " + idxAttr + "=" + idx + " WHERE " + pk + "='" + myNo + "'");
            }
            DBAccess.RunSQL("UPDATE " + table + " SET " + idxAttr + "=" + idxAttr + "+1 WHERE " + pk + "='" + beforeNo + "'");
            DBAccess.RunSQL("UPDATE " + table + " SET " + idxAttr + "=" + idxAttr + "-1 WHERE " + pk + "='" + pkval + "'");
        }
        protected void DoOrderUp(string groupKeyAttr, string groupKeyVal, string gKey2, string gVal2, string idxAttr)
        {
            //  string pkval = this.PKVal as string;
            string pkval = this.PKVal.ToString();
            string pk = this.PK;
            string table = this.EnMap.PhysicsTable;

            string sql = "SELECT " + pk + "," + idxAttr + " FROM " + table + " WHERE (" + groupKeyAttr + "='" + groupKeyVal + "' AND " + gKey2 + "='" + gVal2 + "') ORDER BY " + idxAttr;
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            int idx = 0;
            string beforeNo = "";
            string myNo = "";
            bool isMeet = false;

            foreach (DataRow dr in dt.Rows)
            {
                idx++;
                myNo = dr[pk].ToString();
                if (myNo == pkval)
                    isMeet = true;

                if (isMeet == false)
                    beforeNo = myNo;
                DBAccess.RunSQL("UPDATE " + table + " SET " + idxAttr + "=" + idx + " WHERE " + pk + "='" + myNo + "'  AND  (" + groupKeyAttr + "='" + groupKeyVal + "' AND " + gKey2 + "='" + gVal2 + "') ");
            }
            DBAccess.RunSQL("UPDATE " + table + " SET " + idxAttr + "=" + idxAttr + "+1 WHERE " + pk + "='" + beforeNo + "'  AND  (" + groupKeyAttr + "='" + groupKeyVal + "' AND " + gKey2 + "='" + gVal2 + "')");
            DBAccess.RunSQL("UPDATE " + table + " SET " + idxAttr + "=" + idxAttr + "-1 WHERE " + pk + "='" + pkval + "'  AND   (" + groupKeyAttr + "='" + groupKeyVal + "' AND " + gKey2 + "='" + gVal2 + "')");
        }
        protected void DoOrderDown(string groupKeyAttr, string groupKeyVal, string idxAttr)
        {
            string pkval = this.PKVal.ToString();
            string pk = this.PK;
            string table = this.EnMap.PhysicsTable;

            string sql = "SELECT " + pk + " ," + idxAttr + " FROM " + table + " WHERE " + groupKeyAttr + "='" + groupKeyVal + "' order by " + idxAttr;
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            int idx = 0;
            string nextNo = "";
            string myNo = "";
            bool isMeet = false;
            foreach (DataRow dr in dt.Rows)
            {
                myNo = dr[pk].ToString();
                if (isMeet == true)
                {
                    nextNo = myNo;
                    isMeet = false;
                }
                idx++;

                if (myNo == pkval)
                    isMeet = true;
                DBAccess.RunSQL("UPDATE " + table + " SET " + idxAttr + "=" + idx + " WHERE " + pk + "='" + myNo + "'");
            }

            DBAccess.RunSQL("UPDATE  " + table + " SET " + idxAttr + "=" + idxAttr + "-1 WHERE " + pk + "='" + nextNo + "'");
            DBAccess.RunSQL("UPDATE  " + table + " SET " + idxAttr + "=" + idxAttr + "+1 WHERE " + pk + "='" + pkval + "'");
        }
        protected void DoOrderDown(string groupKeyAttr, string groupKeyVal, string gKeyAttr2, string gKeyVal2, string idxAttr)
        {
            string pkval = this.PKVal.ToString();
            string pk = this.PK;
            string table = this.EnMap.PhysicsTable;

            string sql = "SELECT " + pk + " ," + idxAttr + " FROM " + table + " WHERE (" + groupKeyAttr + "='" + groupKeyVal + "' AND " + gKeyAttr2 + "='" + gKeyVal2 + "' ) order by " + idxAttr;
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            int idx = 0;
            string nextNo = "";
            string myNo = "";
            bool isMeet = false;
            foreach (DataRow dr in dt.Rows)
            {
                myNo = dr[pk].ToString();
                if (isMeet == true)
                {
                    nextNo = myNo;
                    isMeet = false;
                }
                idx++;

                if (myNo == pkval)
                    isMeet = true;
                DBAccess.RunSQL("UPDATE " + table + " SET " + idxAttr + "=" + idx + " WHERE " + pk + "='" + myNo + "' AND  (" + groupKeyAttr + "='" + groupKeyVal + "' AND " + gKeyAttr2 + "='" + gKeyVal2 + "' ) ");
            }

            DBAccess.RunSQL("UPDATE  " + table + " SET " + idxAttr + "=" + idxAttr + "-1 WHERE " + pk + "='" + nextNo + "' AND (" + groupKeyAttr + "='" + groupKeyVal + "' AND " + gKeyAttr2 + "='" + gKeyVal2 + "' )");
            DBAccess.RunSQL("UPDATE  " + table + " SET " + idxAttr + "=" + idxAttr + "+1 WHERE " + pk + "='" + pkval + "' AND (" + groupKeyAttr + "='" + groupKeyVal + "' AND " + gKeyAttr2 + "='" + gKeyVal2 + "' )");
        }
        #endregion  Sorting operation 

        #region  Direct operation 
        /// <summary>
        ///  Direct update 
        /// </summary>
        public int DirectUpdate()
        {
            return EntityDBAccess.Update(this, null);
        }
        /// <summary>
        ///  Straightforward Insert
        /// </summary>
        public virtual int DirectInsert()
        {
            try
            {
                switch (SystemConfig.AppCenterDBType)
                {
                    case DBType.MSSQL:
                        return this.RunSQL(this.SQLCash.Insert, SqlBuilder.GenerParas(this, null));
                    case DBType.Access:
                        return this.RunSQL(this.SQLCash.Insert, SqlBuilder.GenerParas(this, null));
                        break;
                    case DBType.MySQL:
                    case DBType.Informix:
                    default:
                        return this.RunSQL(this.SQLCash.Insert.Replace("[", "").Replace("]", ""), SqlBuilder.GenerParas(this, null));
                }
            }
            catch (Exception ex)
            {
                this.roll();
                if (SystemConfig.IsDebug)
                {
                    try
                    {
                        this.CheckPhysicsTable();
                    }
                    catch (Exception ex1)
                    {
                        throw new Exception(ex.Message + " == " + ex1.Message);
                    }
                }
                throw ex;
            }

            //this.RunSQL(this.SQLCash.Insert, SqlBuilder.GenerParas(this, null));
        }
        /// <summary>
        ///  Straightforward Delete
        /// </summary>
        public void DirectDelete()
        {
            EntityDBAccess.Delete(this);
        }
        public void DirectSave()
        {
            if (this.IsExits)
                this.DirectUpdate();
            else
                this.DirectInsert();
        }
        #endregion

        #region Retrieve
        /// <summary>
        ///  According to attribute query 
        /// </summary>
        /// <param name="attr"> Property name </param>
        /// <param name="val">值</param>
        /// <returns> Whether the query to </returns>
        public bool RetrieveByAttrAnd(string attr1, object val1, string attr2, object val2)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(attr1, val1);
            qo.addAnd();
            qo.AddWhere(attr2, val2);

            if (qo.DoQuery() >= 1)
                return true;
            else
                return false;
        }
        /// <summary>
        ///  According to attribute query 
        /// </summary>
        /// <param name="attr"> Property name </param>
        /// <param name="val">值</param>
        /// <returns> Whether the query to </returns>
        public bool RetrieveByAttrOr(string attr1, object val1, string attr2, object val2)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(attr1, val1);
            qo.addOr();
            qo.AddWhere(attr2, val2);

            if (qo.DoQuery() == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        ///  According to attribute query 
        /// </summary>
        /// <param name="attr"> Property name </param>
        /// <param name="val">值</param>
        /// <returns> Whether the query to </returns>
        public bool RetrieveByAttr(string attr, object val)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(attr, val);
            if (qo.DoQuery() == 1)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 从DBSources Direct inquiries 
        /// </summary>
        /// <returns> The number of queries </returns>
        public virtual int RetrieveFromDBSources()
        {
            try
            {
                return EntityDBAccess.Retrieve(this, this.SQLCash.Select, SqlBuilder.GenerParasPK(this));
            }
            catch
            {
                this.CheckPhysicsTable();
                return EntityDBAccess.Retrieve(this, this.SQLCash.Select, SqlBuilder.GenerParasPK(this));
            }
        }
        /// <summary>
        ///  Inquiry 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public int Retrieve(string key, object val)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(key, val);
            return qo.DoQuery();
        }

        public int Retrieve(string key1, object val1, string key2, object val2)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(key1, val1);
            qo.addAnd();
            qo.AddWhere(key2, val2);
            return qo.DoQuery();
        }
        public int Retrieve(string key1, object val1, string key2, object val2, string key3, object val3)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(key1, val1);
            qo.addAnd();
            qo.AddWhere(key2, val2);
            qo.addAnd();
            qo.AddWhere(key3, val3);
            return qo.DoQuery();
        }
        /// <summary>
        ///  By primary key query , Check out the number of returns .
        ///  If you check out the multiple entities , That the first entity to value .	 
        /// </summary>
        /// <returns> Check out the number of </returns>
        public virtual int Retrieve()
        {
            #region  Obtained from the cache .
            if (this.EnMap.DepositaryOfEntity == Depositary.Application)
            {
                Entities ens;
                try
                {
                    ens = CashEntity.GetEns(this.ToString());
                }
                catch (Exception ex)
                {
                    throw new Exception("@ An error occurred during the memory check @" + ex.Message);
                }

                //  Empty inside to put it into .
                if (ens == null)
                {
                    ens = this.GetNewEntities;
                    ens.FlodInCash(); /* The whole entities  Go into the cache .*/
                }

                string pk = this.PK;
                string pkval = this.GetValStrByKey(pk);
                int count = ens.Count;
                for (int i = 0; i < count; i++)
                {
                    Entity en = ens[i];
                    if (en.PK == pk && en.GetValStrByKey(pk) == pkval)
                    {
                        this.Row = en.Row; /*  If there , Returns it .*/
                        return 1;
                    }
                }

                if (this.RetrieveFromDBSources() != 0)
                {
                    /*  Queries from the data table  */
                    ens.FlodInCash();
                    return 1;
                }

                Attr attr = this.EnMap.GetAttrByKey(pk);
                if (SystemConfig.IsDebug)
                    throw new Exception("@At [" + this.EnDesc + this.EnMap.PhysicsTable + "] Not found [" + attr.Field + attr.Desc + "]=[" + this.PKVal + "] Records .");
                else
                    throw new Exception("@At [" + this.EnDesc + "] Not found [" + attr.Desc + "]=[" + this.PKVal + "] Records .");
            }
            #endregion  Obtained from the cache .

            #region  Anything hold .
            if (this.EnMap.DepositaryOfEntity == Depositary.None)
            {
                /* If there is no entity placed in the cache .*/
                try
                {
                    if (EntityDBAccess.Retrieve(this, this.SQLCash.Select, SqlBuilder.GenerParasPK(this)) <= 0)
                    {
                        string msg = "";
                        switch (this.PK)
                        {
                            case "OID":
                                msg += "[  Primary key =OID 值=" + this.GetValStrByKey("OID") + " ]";
                                break;
                            case "No":
                                msg += "[  Primary key =No 值=" + this.GetValStrByKey("No") + " ]";
                                break;
                            case "MyPK":
                                msg += "[  Primary key =MyPK 值=" + this.GetValStrByKey("MyPK") + " ]";
                                break;
                            case "ID":
                                msg += "[  Primary key =ID 值=" + this.GetValStrByKey("ID") + " ]";
                                break;
                            default:
                                Hashtable ht = this.PKVals;
                                foreach (string key in ht.Keys)
                                    msg += "[  Primary key =" + key + " 值=" + ht[key] + " ]";
                                break;
                        }
                        Log.DefaultLogWriteLine(LogType.Error, "@ No [" + this.EnMap.EnDesc + "  " + this.EnMap.PhysicsTable + ", 类[" + this.ToString() + "],  Physical table [" + this.EnMap.PhysicsTable + "]  Examples .PK = " + this.GetValByKey(this.PK));
                        if (SystemConfig.IsDebug)
                            throw new Exception("@ No [" + this.EnMap.EnDesc + "  " + this.EnMap.PhysicsTable + ", 类[" + this.ToString() + "],  Physical table [" + this.EnMap.PhysicsTable + "]  Examples ." + msg);
                        else
                            throw new Exception("@ No Records Found [" + this.EnMap.EnDesc + "  " + this.EnMap.PhysicsTable + ", " + msg + " Record does not exist , Please contact your administrator ,  Or confirm input errors .");
                    }
                    return 1;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains(" Invalid "))
                        this.CheckPhysicsTable();
                    throw new Exception(ex.Message +"@At Entity(" + this.ToString() + ") An error occurred during query @" + ex.StackTrace);
                }
            }
            #endregion  Anything hold .

            throw new Exception("@ Does not determine the type of cache settings .");
        }
        /// <summary>
        ///  The method of judgment is not present .
        /// </summary>
        /// <returns></returns>
        public virtual bool IsExits
        {
            get
            {
                try
                {
                    if (this.PKField.Contains(","))
                    {
                        Attrs attrs = this.EnMap.Attrs;

                        /* Description multiple primary keys */
                        QueryObject qo = new QueryObject(this);
                        string[] pks = this.PKField.Split(',');

                        bool isNeedAddAnd = false;
                        foreach (string pk in pks)
                        {
                            if (string.IsNullOrEmpty(pk))
                                continue;

                            if (isNeedAddAnd == true)
                            {
                                qo.addAnd();
                            }
                            else
                            {
                                isNeedAddAnd = true;
                            }

                            Attr attr = attrs.GetAttrByKey(pk);
                            switch (attr.MyDataType)
                            {
                                case DataType.AppBoolean:
                                case DataType.AppInt:
                                    qo.AddWhere(pk, this.GetValIntByKey(attr.Key));
                                    break;
                                case DataType.AppDouble:
                                case DataType.AppMoney:
                                case DataType.AppRate:
                                    qo.AddWhere(pk, this.GetValDecimalByKey(attr.Key));
                                    break;
                                default:
                                    qo.AddWhere(pk, this.GetValStringByKey(attr.Key));
                                    break;
                            }

                        }

                        if (qo.DoQueryToTable().Rows.Count == 0)
                            return false;

                        return true;
                    }

                    object obj = this.PKVal;
                    if (obj == null || obj.ToString() == "")
                        return false;

                    if (this.IsOIDEntity)
                        if (obj.ToString() == "0")
                            return false;

                    //  Generate the database judge sentences .
                    string selectSQL = "SELECT " + this.PKField + " FROM " + this.EnMap.PhysicsTable + " WHERE ";
                    switch (this.EnMap.EnDBUrl.DBType)
                    {
                        case DBType.MSSQL:
                            selectSQL += SqlBuilder.GetKeyConditionOfMS(this);
                            break;
                        case DBType.Oracle:
                            selectSQL += SqlBuilder.GetKeyConditionOfOraForPara(this);
                            break;
                        case DBType.Informix:
                            selectSQL += SqlBuilder.GetKeyConditionOfInformixForPara(this);
                            break;
                        case DBType.MySQL:
                            selectSQL += SqlBuilder.GetKeyConditionOfMS(this);
                            break;
                        case DBType.Access:
                            selectSQL += SqlBuilder.GetKeyConditionOfOLE(this);
                            break;
                        default:
                            throw new Exception("@ Not designed to ." + this.EnMap.EnDBUrl.DBUrlType);
                    }

                    //  Query from the database inside , There is no judgment .
                    switch (this.EnMap.EnDBUrl.DBUrlType)
                    {
                        case DBUrlType.AppCenterDSN:
                            return DBAccess.IsExits(selectSQL, SqlBuilder.GenerParasPK(this));
                        case DBUrlType.DBAccessOfMSMSSQL:
                            return DBAccessOfMSMSSQL.IsExits(selectSQL);
                        case DBUrlType.DBAccessOfOLE:
                            return DBAccessOfOLE.IsExits(selectSQL);
                        case DBUrlType.DBAccessOfOracle:
                            return DBAccessOfOracle.IsExits(selectSQL);
                        default:
                            throw new Exception("@ Not designed to DBUrl." + this.EnMap.EnDBUrl.DBUrlType);
                    }

                }
                catch (Exception ex)
                {
                    this.CheckPhysicsTable();
                    throw ex;
                }
            }
        }
        /// <summary>
        ///  According to the primary key query , Check out the results are not assigned to the current entity .
        /// </summary>
        /// <returns> Check out the number of </returns>
        public DataTable RetrieveNotSetValues()
        {
            return this.RunSQLReturnTable(SqlBuilder.Retrieve(this));
        }
        /// <summary>
        ///  The existence of this table 
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool IsExit(string pk, object val)
        {
            if (pk == "OID")
            {
                if (int.Parse(val.ToString()) == 0)
                    return false;
                else
                    return true;
            }
            //else
            //{
            //    string sql = "SELECT " + pk + " FROM " + this.EnMap.PhysicsTable + " WHERE " + pk + " ='" + val + "'";
            //    return DBAccess.IsExits(sql);
            //}

            QueryObject qo = new QueryObject(this);
            qo.AddWhere(pk, val);
            if (qo.DoQuery() == 0)
                return false;
            else
                return true;
        }
        public bool IsExit(string pk1, object val1, string pk2, object val2)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(pk1, val1);
            qo.addAnd();
            qo.AddWhere(pk2, val2);

            if (qo.DoQuery() == 0)
                return false;
            else
                return true;
        }

        public bool IsExit(string pk1, object val1, string pk2, object val2, string pk3, object val3)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(pk1, val1);
            qo.addAnd();
            qo.AddWhere(pk2, val2);
            qo.addAnd();
            qo.AddWhere(pk3, val3);

            if (qo.DoQuery() == 0)
                return false;
            else
                return true;
        }
        #endregion

        #region delete
        private bool CheckDB()
        {

            #region  Check the data .
            //CheckDatas  ens=new CheckDatas(this.EnMap.PhysicsTable);
            //foreach(CheckData en in ens)
            //{
            //    string sql="DELETE  "+en.RefTBName+"   WHERE  "+en.RefTBFK+" ='"+this.GetValByKey(en.MainTBPK) +"' ";	
            //    DBAccess.RunSQL(sql);
            //}
            #endregion

            #region  Determine whether there is detailed 
            foreach (BP.En.EnDtl dtl in this.EnMap.Dtls)
            {
                string sql = "DELETE  FROM  " + dtl.Ens.GetNewEntity.EnMap.PhysicsTable + "   WHERE  " + dtl.RefKey + " ='" + this.PKVal.ToString() + "' ";
                //DBAccess.RunSQL(sql);
                /*
                //string sql="SELECT "+dtl.RefKey+" FROM  "+dtl.Ens.GetNewEntity.EnMap.PhysicsTable+"   WHERE  "+dtl.RefKey+" ='"+this.PKVal.ToString() +"' ";	
                DataTable dt= DBAccess.RunSQLReturnTable(sql); 
                if(dt.Rows.Count==0)
                    continue;
                else
                    throw new Exception("@["+this.EnDesc+"], An error occurred during deletion , It has ["+dt.Rows.Count+"] The existence of a breakdown , You can not delete !");
                    */
            }
            #endregion

            return true;
        }
        /// <summary>
        ///  Before deleting the work to be done 
        /// </summary>
        /// <returns></returns>
        protected virtual bool beforeDelete()
        {
            if (this.EnMap.Attrs.Contains("MyFileName"))
                this.DeleteHisFiles();

            this.CheckDB();
            return true;
        }
        /// <summary>
        ///  Delete its files 
        /// </summary>
        public void DeleteHisFiles()
        {
            //  BP.DA.DBAccess.RunSQL("SELECT * FROM sys_filemanager WHERE EnName='" + this.ToString() + "' AND RefVal='" + this.PKVal + "'");

            try
            {
                BP.DA.DBAccess.RunSQL("DELETE FROM sys_filemanager WHERE EnName='" + this.ToString() + "' AND RefVal='" + this.PKVal + "'");
            }
            catch
            {

            }
        }
        /// <summary>
        ///  It related entities delete ．
        /// </summary>
        public void DeleteHisRefEns()
        {
            #region  Check the data .
            //			CheckDatas  ens=new CheckDatas(this.EnMap.PhysicsTable);
            //			foreach(CheckData en in ens)
            //			{
            //				string sql="DELETE  FROM "+en.RefTBName+"   WHERE  "+en.RefTBFK+" ='"+this.GetValByKey(en.MainTBPK) +"' ";	
            //				DBAccess.RunSQL(sql); 
            //			}
            #endregion

            #region  Determine whether there is detailed 
            foreach (BP.En.EnDtl dtl in this.EnMap.Dtls)
            {
                string sql = "DELETE FROM " + dtl.Ens.GetNewEntity.EnMap.PhysicsTable + "   WHERE  " + dtl.RefKey + " ='" + this.PKVal.ToString() + "' ";
                DBAccess.RunSQL(sql);
            }
            #endregion

            #region  Determine whether there is a pair of relationship .
            foreach (BP.En.AttrOfOneVSM dtl in this.EnMap.AttrsOfOneVSM)
            {
                string sql = "DELETE  FROM " + dtl.EnsOfMM.GetNewEntity.EnMap.PhysicsTable + "   WHERE  " + dtl.AttrOfOneInMM + " ='" + this.PKVal.ToString() + "' ";
                DBAccess.RunSQL(sql);
            }
            #endregion
        }
        /// <summary>
        ///  Delete the cache 
        /// </summary>
        public void DeleteDataAndCash()
        {
            this.Delete();
            this.DeleteFromCash();
        }
        public void DeleteFromCash()
        {
            // Delete Cache .
            CashEntity.Delete(this.ToString(), this.PKVal.ToString());
            //  Deleting Data .
            this.Row.Clear();
        }
        public int Delete()
        {
            if (this.beforeDelete() == false)
                return 0;

            int i = 0;
            try
            {
                i = EntityDBAccess.Delete(this);
            }
            catch (Exception ex)
            {
                Log.DebugWriteInfo(ex.Message);
                throw ex;
            }

            //  Start update memory data .
            switch (this.EnMap.DepositaryOfEntity)
            {
                case Depositary.Application:

                    //  If you do this , Calling insert Abnormally .
                  //  this.DeleteFromCash(); //
                    break;
                case Depositary.None:
                    break;
            }

            this.afterDelete();
            return i;
        }
        /// <summary>
        ///  Delete the specified 
        /// </summary>
        /// <param name="pk"></param>
        public int Delete(object pk)
        {
            Paras ps = new Paras();
            ps.Add(this.PK, pk);
            switch (this.EnMap.EnDBUrl.DBType)
            {
                case DBType.Oracle:
                case DBType.MSSQL:
                case DBType.MySQL:
                    return DBAccess.RunSQL("DELETE FROM " + this.EnMap.PhysicsTable + " WHERE " + this.PK + " =" + this.HisDBVarStr + pk);
                default:
                    throw new Exception(" Types not covered .");
            }
        }
        /// <summary>
        ///  Delete the specified data 
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="val"></param>
        public int Delete(string attr, object val)
        {
            Paras ps = new Paras();
            ps.Add(attr, val);
            switch (this.EnMap.EnDBUrl.DBType)
            {
                case DBType.Oracle:
                case DBType.MSSQL:
                case DBType.Informix:
                case DBType.MySQL:
                    return DBAccess.RunSQL("DELETE FROM " + this.EnMap.PhysicsTable + " WHERE " + this.EnMap.GetAttrByKey(attr).Field + " =" + this.HisDBVarStr + attr, ps);
                case DBType.Access:
                    return DBAccess.RunSQL("DELETE FROM " + this.EnMap.PhysicsTable + " WHERE " + this.EnMap.GetAttrByKey(attr).Field + " =" + this.HisDBVarStr + attr, ps);
                default:
                    throw new Exception(" Types not covered .");
            }
        }
        public int Delete(string attr1, object val1, string attr2, object val2)
        {
            Paras ps = new Paras();
            ps.Add(attr1, val1);
            ps.Add(attr2, val2);
            switch (this.EnMap.EnDBUrl.DBType)
            {
                case DBType.Oracle:
                case DBType.MSSQL:
                case DBType.Informix:
                case DBType.Access:
                case DBType.MySQL:
                    return DBAccess.RunSQL("DELETE FROM " + this.EnMap.PhysicsTable + " WHERE " + this.EnMap.GetAttrByKey(attr1).Field + " =" + this.HisDBVarStr + attr1 + " AND " + this.EnMap.GetAttrByKey(attr2).Field + " =" + this.HisDBVarStr + attr2, ps);
                default:
                    throw new Exception(" Types not covered .");
            }
        }
        public int Delete(string attr1, object val1, string attr2, object val2, string attr3, object val3)
        {
            Paras ps = new Paras();
            ps.Add(attr1, val1);
            ps.Add(attr2, val2);
            ps.Add(attr3, val3);

            switch (this.EnMap.EnDBUrl.DBType)
            {
                case DBType.Oracle:
                case DBType.MSSQL:
                case DBType.Access:
                case DBType.MySQL:
                    return DBAccess.RunSQL("DELETE FROM " + this.EnMap.PhysicsTable + " WHERE " + this.EnMap.GetAttrByKey(attr1).Field + " =" + this.HisDBVarStr + attr1 + " AND " + this.EnMap.GetAttrByKey(attr2).Field + " =" + this.HisDBVarStr + attr2 + " AND " + this.EnMap.GetAttrByKey(attr3).Field + " =" + this.HisDBVarStr + attr3, ps);
                default:
                    throw new Exception(" Types not covered .");
            }
        }
        public int Delete(string attr1, object val1, string attr2, object val2, string attr3, object val3, string attr4, object val4)
        {
            Paras ps = new Paras();
            ps.Add(attr1, val1);
            ps.Add(attr2, val2);
            ps.Add(attr3, val3);
            ps.Add(attr4, val4);

            switch (this.EnMap.EnDBUrl.DBType)
            {
                case DBType.Oracle:
                case DBType.MSSQL:
                case DBType.Access:
                case DBType.MySQL:
                    return DBAccess.RunSQL("DELETE FROM " + this.EnMap.PhysicsTable + " WHERE " + this.EnMap.GetAttrByKey(attr1).Field + " =" + this.HisDBVarStr + attr1 + " AND " + this.EnMap.GetAttrByKey(attr2).Field + " =" + this.HisDBVarStr + attr2 + " AND " + this.EnMap.GetAttrByKey(attr3).Field + " =" + this.HisDBVarStr + attr3 + " AND " + this.EnMap.GetAttrByKey(attr4).Field + " =" + this.HisDBVarStr + attr4, ps);
                default:
                    throw new Exception(" Types not covered .");
            }
        }
        protected virtual void afterDelete()
        {
            if (this.EnMap.DepositaryOfEntity != Depositary.Application)
                return;


            /// Delete Cache .
            BP.DA.CashEntity.Delete(this.ToString(), this.PKVal.ToString());
            return;
        }
        #endregion

        #region  Parameter field 
        private AtPara atPara
        {
            get
            {
                AtPara at = this.Row.GetValByKey("_ATObj_") as AtPara;
                if (at != null)
                    return at;
                try
                {
                    string atParaStr = this.GetValStringByKey("AtPara");
                    if (string.IsNullOrEmpty(atParaStr))
                    {
                        /* Data not found , Executes initialization .*/
                        this.InitParaFields();

                        //  Reacquisition time .
                        atParaStr = this.GetValStringByKey("AtPara");
                        if (string.IsNullOrEmpty(atParaStr))
                            atParaStr = "@1=1@2=2";

                        at = new AtPara(atParaStr);
                        this.SetValByKey("_ATObj_", at);
                        return at;
                    }
                    at = new AtPara(atParaStr);
                    this.SetValByKey("_ATObj_", at);
                    return at;
                }
                catch (Exception ex)
                {
                    throw new Exception("@ Get parameters AtPara When abnormal " + ex.Message + ", You may not have agreed to join the parameter field AtPara. " + ex.Message);
                }
            }
        }
        /// <summary>
        ///  Initialization parameter field ( Need subclasses override )
        /// </summary>
        /// <returns></returns>
        protected virtual void InitParaFields()
        {
        }
        /// <summary>
        ///  Get parameters 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetParaString(string key)
        {
            return atPara.GetValStrByKey(key);
        }
        /// <summary>
        ///  Get parameters 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isNullAsVal"></param>
        /// <returns></returns>
        public string GetParaString(string key, string isNullAsVal)
        {
            string str = atPara.GetValStrByKey(key);
            if (string.IsNullOrEmpty(str))
                return isNullAsVal;
            return str;
        }
        /// <summary>
        ///  Get parameters Init值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetParaInt(string key)
        {
            return atPara.GetValIntByKey(key);
        }
        public float GetParaFloat(string key)
        {
            return atPara.GetValFloatByKey(key);
        }
        /// <summary>
        ///  Get parameters boolen值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool GetParaBoolen(string key)
        {
            return atPara.GetValBoolenByKey(key);
        }
        /// <summary>
        ///  Get parameters boolen值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="IsNullAsVal"></param>
        /// <returns></returns>
        public bool GetParaBoolen(string key, bool IsNullAsVal)
        {
            return atPara.GetValBoolenByKey(key, IsNullAsVal);
        }
        /// <summary>
        ///  Setting parameters 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void SetPara(string key, string obj)
        {
            if (atPara != null)
                this.Row.Remove("_ATObj_");


            string atParaStr = this.GetValStringByKey("AtPara");
            if (atParaStr.Contains("@" + key + "=") == false)
            {
                atParaStr += "@" + key + "=" + obj;
                this.SetValByKey("AtPara", atParaStr);
                return;
            }
            else
            {
                AtPara at = new AtPara(atParaStr);
                at.SetVal(key, obj);
                this.SetValByKey("AtPara", at.GenerAtParaStrs());
                return;
            }
        }
        public void SetPara(string key, int obj)
        {
            SetPara(key, obj.ToString());
        }
        public void SetPara(string key, float obj)
        {
            SetPara(key, obj.ToString());
        }
        public void SetPara(string key, bool obj)
        {
            if (obj == false)
                SetPara(key, "0");
            else
                SetPara(key, "1");
        }
        #endregion

        #region  General Method 
        /// <summary>
        ///  Get real 
        /// </summary>
        /// <param name="key"></param>
        public object GetRefObject(string key)
        {
            return this.Row["_" + key];
            //object obj = this.Row[key];
            //if (obj == null)
            //{
            //    if (this.Row.ContainsKey(key) == false)
            //        return null;
            //    obj = this.Row[key];
            //}
            //return obj;
        }
        /// <summary>
        ///  Set entity 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void SetRefObject(string key, object obj)
        {
            if (obj == null)
                return;

            this.Row.SetValByKey("_" + key, obj);
        }
        #endregion

        #region insert
        /// <summary>
        ///  Work to do before inserting .
        /// </summary>
        /// <returns></returns>
        protected virtual bool beforeInsert()
        {
            return true;
        }
        protected virtual bool roll()
        {
            return true;
        }
        public virtual void InsertWithOutPara()
        {
            this.RunSQL(SqlBuilder.Insert(this));
        }
        /// <summary>
        /// Insert .
        /// </summary>
        public virtual int Insert()
        {
            if (this.beforeInsert() == false)
                return 0;

            if (this.beforeUpdateInsertAction() == false)
                return 0;

            int i = 0;
            try
            {
                i = this.DirectInsert();
            }
            catch(Exception ex)
            {
                this.CheckPhysicsTable();
                throw ex;
            }

            //  Start update memory data .
            switch (this.EnMap.DepositaryOfEntity)
            {
                case Depositary.Application:
                    CashEntity.Insert(this.ToString(), this.PKVal.ToString(), this);
                    break;
                case Depositary.None:
                    break;
            }
           

            this.afterInsert();
            this.afterInsertUpdateAction();

            return i;
        }
        protected virtual void afterInsert()
        {
            return;
        }
        /// <summary>
        ///  After updating to be done with the insertion .
        /// </summary>
        protected virtual void afterInsertUpdateAction()
        {
            if (this.EnMap.HisFKEnumAttrs.Count > 0)
                this.RetrieveFromDBSources();

            if (this.EnMap.IsAddRefName)
            {
                this.ReSetNameAttrVal();
                this.DirectUpdate();
            }
            return;
        }
        /// <summary>
        ///  From a copy copy.
        ///  For two similar number of basic   Entity  copy. 
        /// </summary>
        /// <param name="fromEn"></param>
        public virtual void Copy(Entity fromEn)
        {
            foreach (Attr attr in this.EnMap.Attrs)
            {
                //if (attr.IsPK)
                //    continue;

                object obj = fromEn.GetValByKey(attr.Key);
                if (obj == null)
                    continue;

                this.SetValByKey(attr.Key, obj);
            }
        }
        /// <summary>
        ///  From a copy 
        /// </summary>
        /// <param name="fromRow"></param>
        public virtual void Copy(Row fromRow)
        {
            Attrs attrs = this.EnMap.Attrs;
            foreach (Attr attr in attrs)
            {
                try
                {
                    this.SetValByKey(attr.Key, fromRow.GetValByKey(attr.Key));
                }
                catch
                {
                }
            }
        }
        public virtual void Copy(XML.XmlEn xmlen)
        {
            Attrs attrs = this.EnMap.Attrs;
            foreach (Attr attr in attrs)
            {
                object obj = null;
                try
                {
                    obj = xmlen.GetValByKey(attr.Key);
                }
                catch
                {
                    continue;
                }

                if (obj == null || obj.ToString() == "")
                    continue;
                this.SetValByKey(attr.Key, xmlen.GetValByKey(attr.Key));
            }
        }
        /// <summary>
        ///  Copy  Hashtable
        /// </summary>
        /// <param name="ht"></param>
        public virtual void Copy(System.Collections.Hashtable ht)
        {
            foreach (string k in ht.Keys)
            {
                object obj = null;
                try
                {
                    obj = ht[k];
                }
                catch
                {
                    continue;
                }

                if (obj == null || obj.ToString() == "")
                    continue;
                this.SetValByKey(k, obj);
            }
        }
        public virtual void Copy(DataRow dr)
        {
            foreach (Attr attr in this.EnMap.Attrs)
            {
                try
                {
                    this.SetValByKey(attr.Key, dr[attr.Key]);
                }
                catch
                {
                }
            }
        }
        public string Copy(string refDoc)
        {
            foreach (Attr attr in this._enMap.Attrs)
            {
                refDoc = refDoc.Replace("@" + attr.Key, this.GetValStrByKey(attr.Key));
            }
            return refDoc;
        }


        public void Copy()
        {
            foreach (Attr attr in this.EnMap.Attrs)
            {
                if (attr.IsPK == false)
                    continue;

                if (attr.MyDataType == DataType.AppInt)
                    this.SetValByKey(attr.Key, 0);
                else
                    this.SetValByKey(attr.Key, "");
            }

            try
            {
                this.SetValByKey("No", "");
            }
            catch
            {
            }
        }
        #endregion

        #region verify
        /// <summary>
        ///  Check data 
        /// </summary>
        /// <returns></returns>
        public bool verifyData()
        {
            return true;

            string str = "";
            Attrs attrs = this.EnMap.Attrs;
            string s;
            foreach (Attr attr in attrs)
            {
                if (attr.MyFieldType == FieldType.RefText)
                    continue;

                if (attr.MyDataType == DataType.AppString && attr.MinLength > 0)
                {
                    if (attr.UIIsReadonly)
                        continue;

                    s = this.GetValStrByKey(attr.Key);
                    //  Handle special characters .
                    s = s.Replace("'", "~");
                    s = s.Replace("\"", "]");
                    s = s.Replace(">", "》");
                    s = s.Replace("<", "《");
                    this.SetValByKey(attr.Key, s);

                    if (s.Length < attr.MinLength || s.Length > attr.MaxLength)
                    {
                        if (attr.Key == "No" && attr.UIIsReadonly)
                        {
                            if (this.GetValStringByKey(attr.Key).Trim().Length == 0 || this.GetValStringByKey(attr.Key) == " Automatic generation ")
                                this.SetValByKey("No", this.GenerNewNoByKey("No"));
                        }
                        else
                        {
                            str += "@[" + attr.Key + "," + attr.Desc + "] Input errors , Please enter  " + attr.MinLength + " ～ " + attr.MaxLength + "  Characters range , Is currently empty .";
                        }
                    }
                }

                //else if (attr.MyDataType == DataType.AppDateTime)
                //{
                //    if (this.GetValStringByKey(attr.Key).Trim().Length != 16)
                //    {
                //        //str+="@["+ attr.Desc +"] Enter the date and time format error , Enter the field values ["+this.GetValStringByKey(attr.Key)+"] Does not meet the system format "+BP.DA.DataType.SysDataTimeFormat+" Claim .";
                //    }
                //}
                //else if (attr.MyDataType == DataType.AppDate)
                //{
                //    if (this.GetValStringByKey(attr.Key).Trim().Length != 10)
                //    {
                //        //str+="@["+ attr.Desc +"] Enter the date format error , Enter the field values ["+this.GetValStringByKey(attr.Key)+"] Does not meet the system format "+BP.DA.DataType.SysDataFormat+" Claim .";
                //    }
                //}
            }

            if (str == "")
                return true;



            // throw new Exception("@ In the Save [" + this.EnDesc + "],PK[" + this.PK + "=" + this.PKVal + "] When a message is not the whole entry errors :" + str);

            if (SystemConfig.IsDebug)
                throw new Exception("@ In the Save [" + this.EnDesc + "], Primary key [" + this.PK + "=" + this.PKVal + "] When a message is not the whole entry errors :" + str);
            else
                throw new Exception("@ In the Save [" + this.EnDesc + "][" + this.EnMap.GetAttrByKey(this.PK).Desc + "=" + this.PKVal + "] Error :" + str);
        }
        #endregion

        #region  Update , Insert previous work .
        protected virtual bool beforeUpdateInsertAction()
        {
            switch (this.EnMap.EnType)
            {
                case EnType.View:
                case EnType.XML:
                case EnType.Ext:
                    return false;
                default:
                    break;
            }

            this.verifyData();
            return true;
        }
        #endregion  Update , Insert previous work .

        #region  Update operation 
        /// <summary>
        ///  Update 
        /// </summary>
        /// <returns></returns>
        public virtual int Update()
        {
            return this.Update(null);
        }
        /// <summary>
        ///  Just update a property 
        /// </summary>
        /// <param name="key1">key1</param>
        /// <param name="val1">val1</param>
        /// <returns> The number of updates </returns>
        public int Update(string key1, object val1)
        {
            this.SetValByKey(key1, val1);
            return this.Update(key1.Split(','));
        }
        public int Update(string key1, object val1, string key2, object val2)
        {
            this.SetValByKey(key1, val1);
            this.SetValByKey(key2, val2);

            key1 = key1 + "," + key2;
            return this.Update(key1.Split(','));
        }
        public int Update(string key1, object val1, string key2, object val2, string key3, object val3)
        {
            this.SetValByKey(key1, val1);
            this.SetValByKey(key2, val2);
            this.SetValByKey(key3, val3);

            key1 = key1 + "," + key2 + "," + key3;
            return this.Update(key1.Split(','));
        }
        public int Update(string key1, object val1, string key2, object val2, string key3, object val3, string key4, object val4)
        {
            this.SetValByKey(key1, val1);
            this.SetValByKey(key2, val2);
            this.SetValByKey(key3, val3);
            this.SetValByKey(key4, val4);
            key1 = key1 + "," + key2 + "," + key3 + "," + key4;
            return this.Update(key1.Split(','));
        }
        public int Update(string key1, object val1, string key2, object val2, string key3, object val3, string key4, object val4, string key5, object val5)
        {
            this.SetValByKey(key1, val1);
            this.SetValByKey(key2, val2);
            this.SetValByKey(key3, val3);
            this.SetValByKey(key4, val4);
            this.SetValByKey(key5, val5);

            key1 = key1 + "," + key2 + "," + key3 + "," + key4 + "," + key5;
            return this.Update(key1.Split(','));
        }
        public int Update(string key1, object val1, string key2, object val2, string key3, object val3, string key4, object val4, string key5, object val5, string key6, object val6)
        {
            this.SetValByKey(key1, val1);
            this.SetValByKey(key2, val2);
            this.SetValByKey(key3, val3);
            this.SetValByKey(key4, val4);
            this.SetValByKey(key5, val5);
            this.SetValByKey(key6, val6);
            key1 = key1 + "," + key2 + "," + key3 + "," + key4 + "," + key5 + "," + key6;
            return this.Update(key1.Split(','));
        }
        protected virtual bool beforeUpdate()
        {
            return true;
        }
        /// <summary>
        ///  Update Entity 
        /// </summary>
        public int Update(string[] keys)
        {
            string str = "";
            try
            {
                str = "@ Before the update error  ";
                if (this.beforeUpdate() == false)
                    return 0;

                str = "@ Before updating insert error ";
                if (this.beforeUpdateInsertAction() == false)
                    return 0;

                str = "@ An error occurred while updating ";
                int i = EntityDBAccess.Update(this, keys);
                str = "@ Error after update ";

                //  Start update memory data .
                switch (this.EnMap.DepositaryOfEntity)
                {
                    case Depositary.Application:
                        //this.DeleteFromCash();
                        CashEntity.Update(this.ToString(),this.PKVal.ToString(), this);
                        break;
                    case Depositary.None:
                        break;
                }
                this.afterUpdate();
                str = "@ Error after update insert ";
                this.afterInsertUpdateAction();
                return i;
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains(" Truncates the string ") && ex.Message.Contains(" Lack "))
                {
                    /* Description string length in question .*/
                    this.CheckPhysicsTable();

                    /* Comparison of the field length parameter in question */
                    string errs = "";
                    foreach (Attr attr in this.EnMap.Attrs)
                    {
                        if (attr.MyDataType != BP.DA.DataType.AppString)
                            continue;

                        if (attr.MaxLength < this.GetValStrByKey(attr.Key).Length)
                        {
                            errs += "@ Mapping inside " + attr.Key + "," + attr.Desc + ",  With respect to the data entered. :{" + this.GetValStrByKey(attr.Key) + "},  Too .";
                        }
                    }

                    if (errs != "")
                        throw new Exception("@ Perform an update [" + this.ToString() + "] Error @ Error Field :" + errs + " <br> Once you submit clear ." + ex.Message);
                    else
                        throw ex;
                }


                Log.DefaultLogWriteLine(LogType.Error, ex.Message);
                if (SystemConfig.IsDebug)
                    throw new Exception("@[" + this.EnDesc + "] An error occurs during the update :" + str + ex.Message);
                else
                    throw ex;
            }
        }
        private int UpdateOfDebug(string[] keys)
        {
            string str = "";
            try
            {
                str = "@ Before the update error ";
                if (this.beforeUpdate() == false)
                {
                    return 0;
                }
                str = "@before Update InsertAction Error ";
                if (this.beforeUpdateInsertAction() == false)
                {
                    return 0;
                }
                int i = EntityDBAccess.Update(this, keys);
                str = "@after Update Error ";
                this.afterUpdate();
                str = "@after Insert UpdateAction Error ";
                this.afterInsertUpdateAction();
                //this.UpdateMemory();
                return i;
            }
            catch (System.Exception ex)
            {
                string msg = "@[" + this.EnDesc + "]UpdateOfDebug An error occurs during the update :" + str + ex.Message;
                Log.DefaultLogWriteLine(LogType.Error, msg);

                if (SystemConfig.IsDebug)
                    throw new Exception(msg);
                else
                    throw ex;
            }
        }
        protected virtual void afterUpdate()
        {
            return;
        }
        public virtual int Save()
        {
            switch (this.PK)
            {
                case "OID":
                    if (this.GetValIntByKey("OID") == 0)
                    {
                        //this.SetValByKey("OID",EnDA.GenerOID());
                        this.Insert();
                        return 1;
                    }
                    else
                    {
                        this.Update();
                        return 1;
                    }
                    break;
                case "MyPK":
                case "No":
                case "ID":
                    string pk = this.GetValStrByKey(this.PK);
                    if (pk == "" || pk == null)
                    {
                        this.Insert();
                        return 1;
                    }
                    else
                    {
                        int i = this.Update();
                        if (i == 0)
                        {
                            this.Insert();
                            i = 1;
                        }
                        return i;
                    }
                    break;
                default:
                    if (this.Update() == 0)
                        this.Insert();
                    return 1;
                    break;
            }
        }
        #endregion

        #region  Processing on the database 
        /// <summary>
        ///  The system date is converted to  Oracle  Date type can be stored .
        /// </summary>
        protected void TurnSysDataToOrData()
        {
            Map map = this.EnMap;
            string val = "";
            foreach (Attr attr in map.Attrs)
            {
                try
                {
                    val = this.GetValStringByKey(attr.Key);
                    switch (attr.MyDataType)
                    {
                        case DataType.AppDateTime:
                            if (val.ToUpper().IndexOf("_DATE") > 0)
                                continue;
                            this.SetValByKey(attr.Key, " TO_DATE('" + val + "', '" + DataType.SysDataTimeFormat + "') ");
                            break;
                        case DataType.AppDate:
                            if (val.ToUpper().IndexOf("_DATE") > 0)
                                continue;

                            if (val.Length > 10)
                                val = val.Substring(0, 10);
                            this.SetValByKey(attr.Key, " TO_DATE('" + val + "', '" + DataType.SysDataFormat + "'    )");
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(" An error occurs during execution date conversion :EnName=" + this.ToString() + " TurnSysDataToOrData@ Attr=" + attr.Key + " , Val=" + this.GetValStringByKey(attr.Key) + " Message=" + ex.Message);
                }
            }
        }
        /// <summary>
        ///  Check whether the date 
        /// </summary>
        protected void CheckDateAttr()
        {
            Attrs attrs = this.EnMap.Attrs;
            foreach (Attr attr in attrs)
            {
                if (attr.MyDataType == DataType.AppDate || attr.MyDataType == DataType.AppDateTime)
                {
                    DateTime dt = this.GetValDateTime(attr.Key);
                }
            }
        }
        /// <summary>
        ///  Establish physical table 
        /// </summary>
        protected void CreatePhysicsTable()
        {
            switch (DBAccess.AppCenterDBType)
            {
                case DBType.Oracle:
                    DBAccess.RunSQL(SqlBuilder.GenerCreateTableSQLOfOra_OK(this));
                    break;
                case DBType.Informix:
                    DBAccess.RunSQL(SqlBuilder.GenerCreateTableSQLOfInfoMix(this));
                    break;
                case DBType.MSSQL:
                    DBAccess.RunSQL(SqlBuilder.GenerCreateTableSQLOfMS(this));
                    break;
                case DBType.MySQL:
                    DBAccess.RunSQL(SqlBuilder.GenerCreateTableSQLOfMySQL(this));
                    break;
                case DBType.Access:
                    DBAccess.RunSQL(SqlBuilder.GenerCreateTableSQLOf_OLE(this));
                    break;
                default:
                    throw new Exception("@ Database type not judge .");
            }
            this.CreateIndexAndPK();
        }
        private void CreateIndexAndPK()
        {
            if (this.EnMap.EnDBUrl.DBType != DBType.Informix)
            {
                #region  Indexing 
                try
                {
                    int pkconut = this.PKCount;
                    if (pkconut == 1)
                    {
                        DBAccess.CreatIndex(this.EnMap.PhysicsTable, this.PKField);
                    }
                    else if (pkconut == 2)
                    {
                        string pk0 = this.PKs[0];
                        string pk1 = this.PKs[1];
                        DBAccess.CreatIndex(this.EnMap.PhysicsTable, pk0, pk1);
                    }
                    else if (pkconut == 3)
                    {
                        try
                        {
                            string pk0 = this.PKs[0];
                            string pk1 = this.PKs[1];
                            string pk2 = this.PKs[2];
                            DBAccess.CreatIndex(this.EnMap.PhysicsTable, pk0, pk1, pk2);
                        }
                        catch
                        {
                        }
                    }
                    else if (pkconut == 4)
                    {
                        try
                        {
                            string pk0 = this.PKs[0];
                            string pk1 = this.PKs[1];
                            string pk2 = this.PKs[2];
                            string pk3 = this.PKs[3];
                            DBAccess.CreatIndex(this.EnMap.PhysicsTable, pk0, pk1, pk2, pk3);
                        }
                        catch
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.DefaultLogWriteLineError(ex.Message);
                    throw ex;
                }
                #endregion
            }

            #region  Establish a primary key 
            if (DBAccess.IsExitsTabPK(this.EnMap.PhysicsTable) == false)
            {
                try
                {
                    int pkconut = this.PKCount;
                    if (pkconut == 1)
                    {
                        try
                        {
                            DBAccess.CreatePK(this.EnMap.PhysicsTable, this.PKField, this.EnMap.EnDBUrl.DBType);
                            DBAccess.CreatIndex(this.EnMap.PhysicsTable, this.PKField);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else if (pkconut == 2)
                    {
                        try
                        {
                            string pk0 = this.PKs[0];
                            string pk1 = this.PKs[1];
                            DBAccess.CreatePK(this.EnMap.PhysicsTable, pk0, pk1, this.EnMap.EnDBUrl.DBType);
                            DBAccess.CreatIndex(this.EnMap.PhysicsTable, pk0, pk1);
                        }
                        catch
                        {
                        }
                    }
                    else if (pkconut == 3)
                    {
                        try
                        {
                            string pk0 = this.PKs[0];
                            string pk1 = this.PKs[1];
                            string pk2 = this.PKs[2];
                            DBAccess.CreatePK(this.EnMap.PhysicsTable, pk0, pk1, pk2, this.EnMap.EnDBUrl.DBType);
                            DBAccess.CreatIndex(this.EnMap.PhysicsTable, pk0, pk1, pk2);
                        }
                        catch
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.DefaultLogWriteLineError(ex.Message);
                    throw ex;
                }
            }
            #endregion
        }
        /// <summary>
        ///  If a property is a foreign key , And it is also a field to store its name .
        ///  Setting this attribute foreign key name .
        /// </summary>
        protected void ReSetNameAttrVal()
        {
            Attrs attrs = this.EnMap.Attrs;
            foreach (Attr attr in attrs)
            {
                if (attr.IsFKorEnum == false)
                    continue;

                string s = this.GetValRefTextByKey(attr.Key);
                this.SetValByKey(attr.Key + "Name", s);
            }
        }
        private void CheckPhysicsTable_SQL()
        {
            string table = this._enMap.PhysicsTable;
            DBType dbtype = this._enMap.EnDBUrl.DBType;
            string sqlFields = "";
            string sqlYueShu = "";

            sqlFields = "SELECT column_name as FName,data_type as FType,CHARACTER_MAXIMUM_LENGTH as FLen from information_schema.columns where table_name='" + this.EnMap.PhysicsTable + "'";
            sqlYueShu = "SELECT b.name, a.name FName from sysobjects b join syscolumns a on b.id = a.cdefault where a.id = object_id('" + this.EnMap.PhysicsTable + "') ";

            DataTable dtAttr = DBAccess.RunSQLReturnTable(sqlFields);
            DataTable dtYueShu = DBAccess.RunSQLReturnTable(sqlYueShu);


            #region  Repair table fields .
            Attrs attrs = this._enMap.Attrs;
            foreach (Attr attr in attrs)
            {
                if (attr.IsRefAttr)
                    continue;

                string FType = "";
                string Flen = "";

                #region  Determine whether there .
                bool isHave = false;
                foreach (DataRow dr in dtAttr.Rows)
                {
                    if (dr["FName"].ToString().ToLower() == attr.Field.ToLower())
                    {
                        isHave = true;
                        FType = dr["FType"] as string;
                        Flen = dr["FLen"].ToString();
                        break;
                    }
                }
                if (isHave == false)
                {
                    /* This column does not exist  ,  This column is increased .*/
                    switch (attr.MyDataType)
                    {
                        case DataType.AppString:
                        case DataType.AppDate:
                        case DataType.AppDateTime:
                            int len = attr.MaxLength;
                            if (len == 0)
                                len = 200;
                            //throw new Exception(" The minimum length of the property can not be 0.");
                            if (dbtype == DBType.Access && len >= 254)
                                DBAccess.RunSQL("ALTER TABLE " + this.EnMap.PhysicsTable + " ADD " + attr.Field + "  Memo DEFAULT '" + attr.DefaultVal + "' NULL");
                            else
                                DBAccess.RunSQL("ALTER TABLE " + this.EnMap.PhysicsTable + " ADD " + attr.Field + " NVARCHAR(" + len + ") DEFAULT '" + attr.DefaultVal + "' NULL");
                            continue;
                        case DataType.AppInt:
                            DBAccess.RunSQL("ALTER TABLE " + this.EnMap.PhysicsTable + " ADD " + attr.Field + " INT DEFAULT '" + attr.DefaultVal + "' NOT NULL");
                            continue;
                        case DataType.AppBoolean:
                            DBAccess.RunSQL("ALTER TABLE " + this.EnMap.PhysicsTable + " ADD " + attr.Field + " INT DEFAULT '" + attr.DefaultVal + "' NOT NULL");
                            continue;
                        case DataType.AppFloat:
                        case DataType.AppMoney:
                        case DataType.AppRate:
                        case DataType.AppDouble:
                            DBAccess.RunSQL("ALTER TABLE " + this.EnMap.PhysicsTable + " ADD " + attr.Field + " FLOAT DEFAULT '" + attr.DefaultVal + "' NULL");
                            continue;
                        default:
                            throw new Exception("error MyFieldType= " + attr.MyFieldType + " key=" + attr.Key);
                    }
                }
                #endregion

                #region  Check the type of match .
                switch (attr.MyDataType)
                {
                    case DataType.AppString:
                    case DataType.AppDate:
                    case DataType.AppDateTime:
                        if (FType.ToLower().Contains("char"))
                        {
                            /* Type is correct , Check the length of the */
                            if (Flen == null)
                                throw new Exception("" + attr.Key + " -" + sqlFields);
                            int len = int.Parse(Flen);
                            if (len < attr.MaxLength)
                            {
                                try
                                {
                                    if (attr.MaxLength >= 4000)
                                        DBAccess.RunSQL("alter table " + this.EnMap.PhysicsTable + " alter column " + attr.Key + " NVARCHAR(4000)");
                                    else
                                        DBAccess.RunSQL("alter table " + this.EnMap.PhysicsTable + " alter column " + attr.Key + " NVARCHAR(" + attr.MaxLength + ")");
                                }
                                catch
                                {
                                    /* If the type does not match , Delete it to re-build ,  Remove constraints , In the Delete Column , Reconstruction .*/
                                    foreach (DataRow dr in dtYueShu.Rows)
                                    {
                                        if (dr["FName"].ToString().ToLower() == attr.Key.ToLower())
                                            DBAccess.RunSQL("alter table " + table + " drop constraint " + dr[0].ToString());
                                    }

                                    //  In execution again .
                                    if (attr.MaxLength >= 4000)
                                        DBAccess.RunSQL("alter table " + this.EnMap.PhysicsTable + " alter column " + attr.Key + " NVARCHAR(4000)");
                                    else
                                        DBAccess.RunSQL("alter table " + this.EnMap.PhysicsTable + " alter column " + attr.Key + " NVARCHAR(" + attr.MaxLength + ")");
                                }
                            }
                        }
                        else
                        {
                            /* If the type does not match , Delete it to re-build ,  Remove constraints , In the Delete Column , Reconstruction .*/
                            foreach (DataRow dr in dtYueShu.Rows)
                            {
                                if (dr["FName"].ToString().ToLower() == attr.Key.ToLower())
                                    DBAccess.RunSQL("alter table " + table + " drop constraint " + dr[0].ToString());
                            }

                            DBAccess.RunSQL("ALTER TABLE " + this.EnMap.PhysicsTable + " drop column " + attr.Field);
                            DBAccess.RunSQL("ALTER TABLE " + this.EnMap.PhysicsTable + " ADD " + attr.Field + " NVARCHAR(" + attr.MaxLength + ") DEFAULT '" + attr.DefaultVal + "' NULL");
                            continue;
                        }
                        break;
                    case DataType.AppInt:
                    case DataType.AppBoolean:
                        if (FType != "int")
                        {
                            /* If the type does not match , Delete it to re-build ,  Remove constraints , In the Delete Column , Reconstruction .*/
                            foreach (DataRow dr in dtYueShu.Rows)
                            {
                                if (dr["FName"].ToString().ToLower() == attr.Key.ToLower())
                                    DBAccess.RunSQL("alter table " + table + " drop constraint " + dr[0].ToString());
                            }
                            DBAccess.RunSQL("ALTER TABLE " + this.EnMap.PhysicsTable + " drop column " + attr.Field);
                            DBAccess.RunSQL("ALTER TABLE " + this.EnMap.PhysicsTable + " ADD " + attr.Field + " INT DEFAULT '" + attr.DefaultVal + "' NULL");
                            continue;
                        }
                        break;
                    case DataType.AppFloat:
                    case DataType.AppMoney:
                    case DataType.AppRate:
                    case DataType.AppDouble:
                        if (FType != "float")
                        {
                            /* If the type does not match , Delete it to re-build ,  Remove constraints , In the Delete Column , Reconstruction .*/
                            foreach (DataRow dr in dtYueShu.Rows)
                            {
                                if (dr["FName"].ToString().ToLower() == attr.Key.ToLower())
                                    DBAccess.RunSQL("alter table " + table + " drop constraint " + dr[0].ToString());
                            }
                            DBAccess.RunSQL("ALTER TABLE " + this.EnMap.PhysicsTable + " drop column " + attr.Field);
                            DBAccess.RunSQL("ALTER TABLE " + this.EnMap.PhysicsTable + " ADD " + attr.Field + " FLOAT DEFAULT '" + attr.DefaultVal + "' NULL");
                            continue;
                        }
                        break;
                    default:
                        throw new Exception("error MyFieldType= " + attr.MyFieldType + " key=" + attr.Key);
                        break;
                }
                #endregion
            }
            #endregion  Repair table fields .

            #region  Check whether there is an enumeration type .
            attrs = this._enMap.HisEnumAttrs;
            foreach (Attr attr in attrs)
            {
                if (attr.MyDataType != DataType.AppInt)
                    continue;

                if (attr.UITag == null)
                    continue;

                try
                {
                    SysEnums ses = new SysEnums(attr.UIBindKey, attr.UITag);
                    continue;
                }
                catch
                {
                }

                try
                {
                    string[] strs = attr.UITag.Split('@');
                    SysEnums ens = new SysEnums();
                    ens.Delete(SysEnumAttr.EnumKey, attr.UIBindKey);
                    foreach (string s in strs)
                    {
                        if (s == "" || s == null)
                            continue;

                        string[] vk = s.Split('=');
                        SysEnum se = new SysEnum();
                        se.IntKey = int.Parse(vk[0]);
                        se.Lab = vk[1];
                        se.EnumKey = attr.UIBindKey;
                        se.Insert();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("@ An error occurred while automatically increase enumeration , Make sure your form is correct ." + ex.Message + "attr.UIBindKey=" + attr.UIBindKey);
                }

            }
            #endregion

            #region  Indexing 
            try
            {
                int pkconut = this.PKCount;
                if (pkconut == 1)
                {
                    DBAccess.CreatIndex(this._enMap.PhysicsTable, this.PKField);
                }
                else if (pkconut == 2)
                {
                    string pk0 = this.PKs[0];
                    string pk1 = this.PKs[1];
                    DBAccess.CreatIndex(this._enMap.PhysicsTable, pk0, pk1);
                }
                else if (pkconut == 3)
                {
                    try
                    {
                        string pk0 = this.PKs[0];
                        string pk1 = this.PKs[1];
                        string pk2 = this.PKs[2];
                        DBAccess.CreatIndex(this._enMap.PhysicsTable, pk0, pk1, pk2);
                    }
                    catch
                    {
                    }
                }
                else if (pkconut == 4)
                {
                    try
                    {
                        string pk0 = this.PKs[0];
                        string pk1 = this.PKs[1];
                        string pk2 = this.PKs[2];
                        string pk3 = this.PKs[3];
                        DBAccess.CreatIndex(this._enMap.PhysicsTable, pk0, pk1, pk2, pk3);
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                Log.DefaultLogWriteLineError(ex.Message);
                throw ex;
                //throw new Exception("create pk error :"+ex.Message );
            }
            #endregion

            #region  Establish a primary key 
            if (DBAccess.IsExitsTabPK(this._enMap.PhysicsTable) == false)
            {
                try
                {
                    int pkconut = this.PKCount;
                    if (pkconut == 1)
                    {
                        try
                        {
                            DBAccess.CreatePK(this._enMap.PhysicsTable, this.PKField, this._enMap.EnDBUrl.DBType);
                            DBAccess.CreatIndex(this._enMap.PhysicsTable, this.PKField);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else if (pkconut == 2)
                    {
                        try
                        {
                            string pk0 = this.PKs[0];
                            string pk1 = this.PKs[1];
                            DBAccess.CreatePK(this._enMap.PhysicsTable, pk0, pk1, this._enMap.EnDBUrl.DBType);
                            DBAccess.CreatIndex(this._enMap.PhysicsTable, pk0, pk1);
                        }
                        catch
                        {
                        }
                    }
                    else if (pkconut == 3)
                    {
                        try
                        {
                            string pk0 = this.PKs[0];
                            string pk1 = this.PKs[1];
                            string pk2 = this.PKs[2];
                            DBAccess.CreatePK(this._enMap.PhysicsTable, pk0, pk1, pk2, this._enMap.EnDBUrl.DBType);
                            DBAccess.CreatIndex(this._enMap.PhysicsTable, pk0, pk1, pk2);
                        }
                        catch
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.DefaultLogWriteLineError(ex.Message);
                    throw ex;
                }
            }
            #endregion
        }
        /// <summary>
        ///  Check the physical table 
        /// </summary>
        public void CheckPhysicsTable()
        {

            this._enMap = this.EnMap;

            //  string msg = "";
            if (this._enMap.EnType == EnType.View
                || this._enMap.EnType == EnType.XML
                || this._enMap.EnType == EnType.ThirdPartApp
                || this._enMap.EnType == EnType.Ext)
                return;

            if (DBAccess.IsExitsObject(this._enMap.PhysicsTable) == false)
            {
                /*  If the physical table does not exist on the establishment of a new physical form .*/
                this.CreatePhysicsTable();
                return;
            }

            DBType dbtype = this._enMap.EnDBUrl.DBType;
            if (this._enMap.IsView)
                return;

            //  If it is not the main application database would not allow the implementation of inspection .  Consider the safety of third-party systems .
            if (this._enMap.EnDBUrl.DBUrlType
                != DBUrlType.AppCenterDSN)
                return;
            switch (SystemConfig.AppCenterDBType)
            {
                case DBType.MSSQL:
                    this.CheckPhysicsTable_SQL();
                    break;
                case DBType.Oracle:
                    this.CheckPhysicsTable_Ora();
                    break;
                case DBType.MySQL:
                    this.CheckPhysicsTable_MySQL();
                    break;
                case DBType.Informix:
                    this.CheckPhysicsTable_Informix();
                    break;
                default:
                    break;
            }

        }
        private void CheckPhysicsTable_Informix()
        {
            #region  Check the field if there is 
            string sql = "SELECT *  FROM " + this.EnMap.PhysicsTable + " WHERE 1=2";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);

            // If there is no .
            foreach (Attr attr in this.EnMap.Attrs)
            {
                if (attr.MyFieldType == FieldType.RefText)
                    continue;

                if (attr.IsPK)
                    continue;

                if (dt.Columns.Contains(attr.Key) == true)
                    continue;

                if (attr.Key == "AID")
                {
                    /*  Automatic growth column  */
                    DBAccess.RunSQL("ALTER TABLE " + this.EnMap.PhysicsTable + " ADD " + attr.Field + " INT  Identity(1,1)");
                    continue;
                }

                /* This column does not exist  ,  This column is increased .*/
                switch (attr.MyDataType)
                {
                    case DataType.AppString:
                    case DataType.AppDate:
                    case DataType.AppDateTime:
                        int len = attr.MaxLength;
                        if (len == 0)
                            len = 200;

                        if (len >= 254)
                            DBAccess.RunSQL("alter table " + this.EnMap.PhysicsTable + " add " + attr.Field + " lvarchar(" + len + ") default '" + attr.DefaultVal + "'");
                        else
                            DBAccess.RunSQL("alter table " + this.EnMap.PhysicsTable + " add " + attr.Field + " varchar(" + len + ") default '" + attr.DefaultVal + "'");
                        break;
                    case DataType.AppInt:
                    case DataType.AppBoolean:
                        DBAccess.RunSQL("ALTER TABLE " + this.EnMap.PhysicsTable + " ADD " + attr.Field + " INT8 DEFAULT " + attr.DefaultVal + " ");
                        break;
                    case DataType.AppFloat:
                    case DataType.AppMoney:
                    case DataType.AppRate:
                    case DataType.AppDouble:
                        DBAccess.RunSQL("ALTER TABLE " + this.EnMap.PhysicsTable + " ADD " + attr.Field + " FLOAT DEFAULT  " + attr.DefaultVal + " ");
                        break;
                    default:
                        throw new Exception("error MyFieldType= " + attr.MyFieldType + " key=" + attr.Key);
                }
            }
            #endregion

            #region  Check the length of the field meets the minimum requirements 
            foreach (Attr attr in this.EnMap.Attrs)
            {
                if (attr.MyFieldType == FieldType.RefText)
                    continue;
                if (attr.MyDataType == DataType.AppDouble
                    || attr.MyDataType == DataType.AppFloat
                    || attr.MyDataType == DataType.AppInt
                    || attr.MyDataType == DataType.AppMoney
                    || attr.MyDataType == DataType.AppBoolean
                    || attr.MyDataType == DataType.AppRate)
                    continue;

                int maxLen = attr.MaxLength;
                dt = new DataTable();
                sql = "select c.*  from syscolumns c inner join systables t on c.tabid = t.tabid where t.tabname = lower('" + this.EnMap.PhysicsTable.ToLower() + "') and c.colname = lower('" + attr.Key + "') and c.collength < " + attr.MaxLength;
                dt = this.RunSQLReturnTable(sql);
                if (dt.Rows.Count == 0)
                    continue;
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        if (attr.MaxLength >= 255)
                            this.RunSQL("alter table " + dr["owner"] + "." + this.EnMap.PhysicsTableExt + " modify " + attr.Field + " lvarchar(" + attr.MaxLength + ")");
                        else
                            this.RunSQL("alter table " + dr["owner"] + "." + this.EnMap.PhysicsTableExt + " modify " + attr.Field + " varchar(" + attr.MaxLength + ")");
                    }
                    catch (Exception ex)
                    {
                        BP.DA.Log.DebugWriteWarning(ex.Message);
                    }
                }
            }
            #endregion

            #region  Check whether the field is an enumeration type INT  Type 
            Attrs attrs = this._enMap.HisEnumAttrs;
            foreach (Attr attr in attrs)
            {
                if (attr.MyDataType != DataType.AppInt)
                    continue;
#warning  No deal .
                continue;

                sql = "SELECT DATA_TYPE FROM ALL_TAB_COLUMNS WHERE  TABLE_NAME='" + this.EnMap.PhysicsTableExt.ToLower() + "' AND COLUMN_NAME='" + attr.Field.ToLower() + "'";
                string val = DBAccess.RunSQLReturnString(sql);
                if (val == null)
                    Log.DefaultLogWriteLineError("@ Field is not detected :" + attr.Key);

                if (val.IndexOf("CHAR") != -1)
                {
                    /* If it is  varchar  Field */
                    sql = "SELECT OWNER FROM ALL_TAB_COLUMNS WHERE upper(TABLE_NAME)='" + this.EnMap.PhysicsTableExt.ToUpper() + "' AND UPPER(COLUMN_NAME)='" + attr.Field.ToUpper() + "' ";
                    string OWNER = DBAccess.RunSQLReturnString(sql);
                    try
                    {
                        this.RunSQL("alter table  " + this.EnMap.PhysicsTableExt + " modify " + attr.Field + " NUMBER ");
                    }
                    catch (Exception ex)
                    {
                        Log.DefaultLogWriteLineError(" Run sql  Failure :alter table  " + this.EnMap.PhysicsTableExt + " modify " + attr.Field + " NUMBER " + ex.Message);
                    }
                }
            }
            #endregion

            #region  Check whether there is an enumeration type .
            attrs = this._enMap.HisEnumAttrs;
            foreach (Attr attr in attrs)
            {
                if (attr.MyDataType != DataType.AppInt)
                    continue;
                if (attr.UITag == null)
                    continue;
                try
                {
                    SysEnums ses = new SysEnums(attr.UIBindKey, attr.UITag);
                    continue;
                }
                catch
                {
                }
                string[] strs = attr.UITag.Split('@');
                SysEnums ens = new SysEnums();
                ens.Delete(SysEnumAttr.EnumKey, attr.UIBindKey);
                foreach (string s in strs)
                {
                    if (s == "" || s == null)
                        continue;

                    string[] vk = s.Split('=');
                    SysEnum se = new SysEnum();
                    se.IntKey = int.Parse(vk[0]);
                    se.Lab = vk[1];
                    se.EnumKey = attr.UIBindKey;
                    se.Insert();
                }
            }
            #endregion

            this.CreateIndexAndPK();
        }
        private void CheckPhysicsTable_MySQL()
        {
            #region  Check the field if there is 
            string sql = "SELECT *  FROM " + this._enMap.PhysicsTable + " WHERE 1=2";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);

            // If there is no .
            foreach (Attr attr in this._enMap.Attrs)
            {
                if (attr.MyFieldType == FieldType.RefText)
                    continue;

                if (attr.IsPK)
                    continue;

                if (dt.Columns.Contains(attr.Key) == true)
                    continue;

                if (attr.Key == "AID")
                {
                    /*  Automatic growth column  */
                    DBAccess.RunSQL("ALTER TABLE " + this._enMap.PhysicsTable + " ADD " + attr.Field + " INT  Identity(1,1)");
                    continue;
                }

                /* This column does not exist  ,  This column is increased .*/
                switch (attr.MyDataType)
                {
                    case DataType.AppString:
                        int len = attr.MaxLength;
                        if (len == 0)
                            len = 200;
                        if (len > 3000)
                            DBAccess.RunSQL("ALTER TABLE " + this._enMap.PhysicsTable + " ADD " + attr.Field + " TEXT ");
                        else
                            DBAccess.RunSQL("ALTER TABLE " + this._enMap.PhysicsTable + " ADD " + attr.Field + " NVARCHAR(" + len + ") DEFAULT '" + attr.DefaultVal + "' NULL");
                        break;
                    case DataType.AppDate:
                    case DataType.AppDateTime:
                        DBAccess.RunSQL("ALTER TABLE " + this._enMap.PhysicsTable + " ADD " + attr.Field + " NVARCHAR(20) DEFAULT '" + attr.DefaultVal + "' NULL");
                        break;
                    case DataType.AppInt:
                    case DataType.AppBoolean:
                        DBAccess.RunSQL("ALTER TABLE " + this._enMap.PhysicsTable + " ADD " + attr.Field + " INT DEFAULT '" + attr.DefaultVal + "' NOT NULL");
                        break;
                    case DataType.AppFloat:
                    case DataType.AppMoney:
                    case DataType.AppRate:
                    case DataType.AppDouble:
                        DBAccess.RunSQL("ALTER TABLE " + this._enMap.PhysicsTable + " ADD " + attr.Field + " FLOAT DEFAULT '" + attr.DefaultVal + "' NULL");
                        break;
                    default:
                        throw new Exception("error MyFieldType= " + attr.MyFieldType + " key=" + attr.Key);
                }
            }
            #endregion

            #region  Check the length of the field meets the minimum requirements 
            foreach (Attr attr in this._enMap.Attrs)
            {
                if (attr.MyFieldType == FieldType.RefText)
                    continue;
                if (attr.MyDataType == DataType.AppDouble
                    || attr.MyDataType == DataType.AppFloat
                    || attr.MyDataType == DataType.AppInt
                    || attr.MyDataType == DataType.AppMoney
                    || attr.MyDataType == DataType.AppBoolean
                    || attr.MyDataType == DataType.AppRate)
                    continue;

                int maxLen = attr.MaxLength;
                dt = new DataTable();
                sql = "select character_maximum_length as Len, table_schema as OWNER FROM information_schema.columns WHERE TABLE_SCHEMA='" + BP.Sys.SystemConfig.AppCenterDBDatabase + "' AND table_name ='" + this._enMap.PhysicsTable + "' and column_Name='" + attr.Field + "' AND character_maximum_length < " + attr.MaxLength;
                dt = this.RunSQLReturnTable(sql);
                if (dt.Rows.Count == 0)
                    continue;
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        this.RunSQL("alter table " + dr["OWNER"] + "." + this._enMap.PhysicsTableExt + " modify " + attr.Field + " NVARCHAR(" + attr.MaxLength + ")");
                    }
                    catch (Exception ex)
                    {
                        BP.DA.Log.DebugWriteWarning(ex.Message);
                    }
                }
            }
            #endregion

            #region  Check whether the field is an enumeration type INT  Type 
            Attrs attrs = this._enMap.HisEnumAttrs;
            foreach (Attr attr in attrs)
            {
                if (attr.MyDataType != DataType.AppInt)
                    continue;

                sql = "SELECT DATA_TYPE FROM information_schema.columns WHERE table_name='" + this._enMap.PhysicsTable + "' AND COLUMN_NAME='" + attr.Field + "' and table_schema='" + SystemConfig.AppCenterDBDatabase + "'";
                string val = DBAccess.RunSQLReturnString(sql);
                if (val == null)
                    Log.DefaultLogWriteLineError("@ Field is not detected eunm" + attr.Key);

                if (val.IndexOf("CHAR") != -1)
                {
                    /* If it is  varchar  Field */
                    sql = "SELECT table_schema as OWNER FROM information_schema.columns WHERE  table_name='" + this._enMap.PhysicsTableExt + "' AND COLUMN_NAME='" + attr.Field + "' and table_schema='" + SystemConfig.AppCenterDBDatabase + "'";
                    string OWNER = DBAccess.RunSQLReturnString(sql);
                    try
                    {
                        this.RunSQL("alter table  " + this._enMap.PhysicsTableExt + " modify " + attr.Field + " NUMBER ");
                    }
                    catch (Exception ex)
                    {
                        Log.DefaultLogWriteLineError(" Run sql  Failure :alter table  " + this._enMap.PhysicsTableExt + " modify " + attr.Field + " NUMBER " + ex.Message);
                    }
                }
            }
            #endregion

            #region  Check whether there is an enumeration type .
            attrs = this._enMap.HisEnumAttrs;
            foreach (Attr attr in attrs)
            {
                if (attr.MyDataType != DataType.AppInt)
                    continue;
                if (attr.UITag == null)
                    continue;
                try
                {
                    SysEnums ses = new SysEnums(attr.UIBindKey, attr.UITag);
                    continue;
                }
                catch
                {
                }
                string[] strs = attr.UITag.Split('@');
                SysEnums ens = new SysEnums();
                ens.Delete(SysEnumAttr.EnumKey, attr.UIBindKey);
                foreach (string s in strs)
                {
                    if (s == "" || s == null)
                        continue;

                    string[] vk = s.Split('=');
                    SysEnum se = new SysEnum();
                    se.IntKey = int.Parse(vk[0]);
                    se.Lab = vk[1];
                    se.EnumKey = attr.UIBindKey;
                    se.Insert();
                }
            }
            #endregion
            this.CreateIndexAndPK();
        }
        private void CheckPhysicsTable_Ora()
        {
            #region  Check the field if there is 
            string sql = "SELECT *  FROM " + this.EnMap.PhysicsTable + " WHERE 1=2";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);

            // If there is no .
            foreach (Attr attr in this.EnMap.Attrs)
            {
                if (attr.MyFieldType == FieldType.RefText)
                    continue;

                if (attr.IsPK)
                    continue;

                if (dt.Columns.Contains(attr.Key) == true)
                    continue;

                if (attr.Key == "AID")
                {
                    /*  Automatic growth column  */
                    DBAccess.RunSQL("ALTER TABLE " + this.EnMap.PhysicsTable + " ADD " + attr.Field + " INT  Identity(1,1)");
                    continue;
                }

                /* This column does not exist  ,  This column is increased .*/
                switch (attr.MyDataType)
                {
                    case DataType.AppString:
                    case DataType.AppDate:
                    case DataType.AppDateTime:
                        int len = attr.MaxLength;
                        if (len == 0)
                            len = 200;
                        DBAccess.RunSQL("ALTER TABLE " + this.EnMap.PhysicsTable + " ADD " + attr.Field + " VARCHAR(" + len + ") DEFAULT '" + attr.DefaultVal + "' NULL");
                        break;
                    case DataType.AppInt:
                    case DataType.AppBoolean:
                        DBAccess.RunSQL("ALTER TABLE " + this.EnMap.PhysicsTable + " ADD " + attr.Field + " INT DEFAULT '" + attr.DefaultVal + "' NOT NULL");
                        break;
                    case DataType.AppFloat:
                    case DataType.AppMoney:
                    case DataType.AppRate:
                    case DataType.AppDouble:
                        DBAccess.RunSQL("ALTER TABLE " + this.EnMap.PhysicsTable + " ADD " + attr.Field + " FLOAT DEFAULT '" + attr.DefaultVal + "' NULL");
                        break;
                    default:
                        throw new Exception("error MyFieldType= " + attr.MyFieldType + " key=" + attr.Key);
                }
            }
            #endregion

            #region  Check the length of the field meets the minimum requirements 
            foreach (Attr attr in this.EnMap.Attrs)
            {
                if (attr.MyFieldType == FieldType.RefText)
                    continue;
                if (attr.MyDataType == DataType.AppDouble
                    || attr.MyDataType == DataType.AppFloat
                    || attr.MyDataType == DataType.AppInt
                    || attr.MyDataType == DataType.AppMoney
                    || attr.MyDataType == DataType.AppBoolean
                    || attr.MyDataType == DataType.AppRate)
                    continue;

                int maxLen = attr.MaxLength;
                dt = new DataTable();
                sql = "SELECT DATA_LENGTH AS LEN, OWNER FROM ALL_TAB_COLUMNS WHERE upper(TABLE_NAME)='" + this.EnMap.PhysicsTableExt.ToUpper() + "' AND UPPER(COLUMN_NAME)='" + attr.Field.ToUpper() + "' AND DATA_LENGTH < " + attr.MaxLength;
                dt = this.RunSQLReturnTable(sql);
                if (dt.Rows.Count == 0)
                    continue;
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        this.RunSQL("alter table " + dr["OWNER"] + "." + this.EnMap.PhysicsTableExt + " modify " + attr.Field + " varchar2(" + attr.MaxLength + ")");
                    }
                    catch (Exception ex)
                    {
                        BP.DA.Log.DebugWriteWarning(ex.Message);
                    }
                }
            }
            #endregion

            #region  Check whether the field is an enumeration type INT  Type 
            Attrs attrs = this._enMap.HisEnumAttrs;
            foreach (Attr attr in attrs)
            {
                if (attr.MyDataType != DataType.AppInt)
                    continue;
                sql = "SELECT DATA_TYPE FROM ALL_TAB_COLUMNS WHERE upper(TABLE_NAME)='" + this.EnMap.PhysicsTableExt.ToUpper() + "' AND UPPER(COLUMN_NAME)='" + attr.Field.ToUpper() + "' ";
                string val = DBAccess.RunSQLReturnString(sql);
                if (val == null)
                    Log.DefaultLogWriteLineError("@ Field is not detected eunm" + attr.Key);
                if (val.IndexOf("CHAR") != -1)
                {
                    /* If it is  varchar  Field */
                    sql = "SELECT OWNER FROM ALL_TAB_COLUMNS WHERE upper(TABLE_NAME)='" + this.EnMap.PhysicsTableExt.ToUpper() + "' AND UPPER(COLUMN_NAME)='" + attr.Field.ToUpper() + "' ";
                    string OWNER = DBAccess.RunSQLReturnString(sql);
                    try
                    {
                        this.RunSQL("alter table  " + this.EnMap.PhysicsTableExt + " modify " + attr.Field + " NUMBER ");
                    }
                    catch (Exception ex)
                    {
                        Log.DefaultLogWriteLineError(" Run sql  Failure :alter table  " + this.EnMap.PhysicsTableExt + " modify " + attr.Field + " NUMBER " + ex.Message);
                    }
                }
            }
            #endregion

            #region  Check whether there is an enumeration type .
            attrs = this._enMap.HisEnumAttrs;
            foreach (Attr attr in attrs)
            {
                if (attr.MyDataType != DataType.AppInt)
                    continue;
                if (attr.UITag == null)
                    continue;
                try
                {
                    SysEnums ses = new SysEnums(attr.UIBindKey, attr.UITag);
                    continue;
                }
                catch
                {
                }
                string[] strs = attr.UITag.Split('@');
                SysEnums ens = new SysEnums();
                ens.Delete(SysEnumAttr.EnumKey, attr.UIBindKey);
                foreach (string s in strs)
                {
                    if (s == "" || s == null)
                        continue;

                    string[] vk = s.Split('=');
                    SysEnum se = new SysEnum();
                    se.IntKey = int.Parse(vk[0]);
                    se.Lab = vk[1];
                    se.EnumKey = attr.UIBindKey;
                    se.Insert();
                }
            }
            #endregion
            this.CreateIndexAndPK();
        }
        #endregion

        #region  Automatic Data Processing 
        public void AutoFull()
        {
            if (this.PKVal == "0" || this.PKVal == "")
                return;

            if (this.EnMap.IsHaveAutoFull == false)
                return;

            Attrs attrs = this.EnMap.Attrs;
            string jsAttrs = "";
            ArrayList al = new ArrayList();
            foreach (Attr attr in attrs)
            {
                if (attr.AutoFullDoc == null || attr.AutoFullDoc.Length == 0)
                    continue;

                //  This code needs to purify the base class .
                switch (attr.AutoFullWay)
                {
                    case AutoFullWay.Way0:
                        continue;
                    case AutoFullWay.Way1_JS:
                        al.Add(attr);
                        break;
                    case AutoFullWay.Way2_SQL:
                        string sql = attr.AutoFullDoc;
                        sql = sql.Replace("~", "'");

                        sql = sql.Replace("@WebUser.No", Web.WebUser.No);
                        sql = sql.Replace("@WebUser.Name", Web.WebUser.Name);
                        sql = sql.Replace("@WebUser.FK_Dept", Web.WebUser.FK_Dept);

                        if (sql.Contains("@") == true)
                        {
                            Attrs attrs1 = this.EnMap.Attrs;
                            foreach (Attr a1 in attrs1)
                            {
                                if (sql.Contains("@") == false)
                                    break;

                                if (sql.Contains("@" + a1.Key) == false)
                                    continue;

                                if (a1.IsNum)
                                    sql = sql.Replace("@" + a1.Key, this.GetValStrByKey(a1.Key));
                                else
                                    sql = sql.Replace("@" + a1.Key, "'" + this.GetValStrByKey(a1.Key) + "'");
                            }
                        }

                        sql = sql.Replace("''", "'");
                        string val = "";
                        try
                        {
                            val = DBAccess.RunSQLReturnString(sql);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("@ Field (" + attr.Key + "," + attr.Desc + ") Automatic error during data acquisition ( There may be you write sql Statement returns multiple rows and columns of table, Now just a row table To fill , Please check sql.):" + sql.Replace("'", "[") + " @Tech Info:" + ex.Message.Replace("'", "[") + "@ Execution sql:" + sql);
                        }

                        if (attr.IsNum)
                        {
                            /*  If this is the type of value to try to convert the value , Conversion not ran out anomalies .*/
                            try
                            {
                                decimal d = decimal.Parse(val);
                            }
                            catch
                            {
                                throw new Exception(val);
                            }
                        }
                        this.SetValByKey(attr.Key, val);
                        break;
                    case AutoFullWay.Way3_FK:
                        try
                        {
                            string sqlfk = "SELECT @Field FROM @Table WHERE No=@AttrKey";
                            string[] strsFK = attr.AutoFullDoc.Split('@');
                            foreach (string str in strsFK)
                            {
                                if (str == null || str.Length == 0)
                                    continue;

                                string[] ss = str.Split('=');
                                if (ss[0] == "AttrKey")
                                {
                                    string tempV = this.GetValStringByKey(ss[1]);
                                    if (tempV == "" || tempV == null)
                                    {
                                        if (this.EnMap.Attrs.Contains(ss[1]) == false)
                                            throw new Exception("@ Automatic acquisition value information is incomplete ,Map  No longer contains a Key=" + ss[1] + " Properties .");

                                        //throw new Exception("@ Automatic acquisition value information is incomplete ,Map  No longer contains a Key=" + ss[1] + " Properties .");
                                        sqlfk = sqlfk.Replace('@' + ss[0], "'@xxx'");
                                        Log.DefaultLogWriteLineWarning("@ An error occurred during auto value :" + this.ToString() + " , " + this.PKVal + " Does not automatically get the information .");
                                    }
                                    else
                                    {
                                        sqlfk = sqlfk.Replace('@' + ss[0], "'" + this.GetValStringByKey(ss[1]) + "'");
                                    }
                                }
                                else
                                {
                                    sqlfk = sqlfk.Replace('@' + ss[0], ss[1]);
                                }
                            }

                            sqlfk = sqlfk.Replace("''", "'");
                            this.SetValByKey(attr.Key, DBAccess.RunSQLReturnStringIsNull(sqlfk, null));
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("@ Processing AutoComplete : Foreign key [" + attr.Key + ";" + attr.Desc + "], When an error occurs . Exception Information :" + ex.Message);
                        }
                        break;
                    case AutoFullWay.Way4_Dtl:
                        if (this.PKVal == "0")
                            continue;

                        string mysql = "SELECT @Way(@Field) FROM @Table WHERE RefPK =" + this.PKVal;
                        string[] strs = attr.AutoFullDoc.Split('@');
                        foreach (string str in strs)
                        {
                            if (str == null || str.Length == 0)
                                continue;

                            string[] ss = str.Split('=');
                            mysql = mysql.Replace('@' + ss[0], ss[1]);
                        }

                        string v = DBAccess.RunSQLReturnString(mysql);
                        if (v == null)
                            v = "0";
                        this.SetValByKey(attr.Key, decimal.Parse(v));

                        break;
                    default:
                        throw new Exception(" Type is not involved .");
                }
            }

            //  Deal with JS Calculation .
            foreach (Attr attr in al)
            {
                string doc = attr.AutoFullDoc.Clone().ToString();
                foreach (Attr a in attrs)
                {
                    if (a.Key == attr.Key)
                        continue;

                    doc = doc.Replace("@" + a.Key, this.GetValStrByKey(a.Key).ToString());
                    doc = doc.Replace("@" + a.Desc, this.GetValStrByKey(a.Key).ToString());
                }

                try
                {
                    decimal d = DataType.ParseExpToDecimal(doc);
                    this.SetValByKey(attr.Key, d);
                }
                catch (Exception ex)
                {
                    Log.DefaultLogWriteLineError("@(" + this.ToString() + ") Automatic calculation processing {" + this.EnDesc + "}:" + this.PK + "=" + this.PKVal + "时, Property [" + attr.Key + "], Computing content [" + doc + "], Error :" + ex.Message);
                    throw new Exception("@(" + this.ToString() + ") Automatic calculation processing {" + this.EnDesc + "}:" + this.PK + "=" + this.PKVal + "时, Property [" + attr.Key + "], Computing content [" + doc + "], Error :" + ex.Message);
                }
            }

        }
        #endregion
    }
    /// <summary>
    ///  Entity data collection 
    /// </summary>
    [Serializable]
    public abstract class Entities : CollectionBase
    {
        #region  For configuration information .
        /// <summary>
        ///  Get the application configuration information 
        /// </summary>
        /// <param name="key"> Key </param>
        /// <returns> The return value </returns>
        public string GetEnsAppCfgByKeyString(string key)
        {
            BP.Sys.EnsAppCfg cfg = new EnsAppCfg(this.ToString(), key);
            return cfg.CfgVal;
        }
        public int GetEnsAppCfgByKeyInt(string key)
        {
            return GetEnsAppCfgByKeyInt(key, 0);
        }
        /// <summary>
        ///  Get the default value 
        /// </summary>
        /// <param name="key"> Field </param>
        /// <param name="isNullAsVal">值</param>
        /// <returns></returns>
        public int GetEnsAppCfgByKeyInt(string key, int isNullAsVal)
        {
            try
            {
                BP.Sys.EnsAppCfg cfg = new EnsAppCfg(this.ToString(), key);
                return cfg.CfgValOfInt;
            }
            catch
            {
                return isNullAsVal;
            }
        }
        public bool GetEnsAppCfgByKeyBoolen(string key)
        {
            BP.Sys.EnsAppCfg cfg = new EnsAppCfg(this.ToString(), key);
            return cfg.CfgValOfBoolen;
        }
        #endregion  For configuration information .

        #region  Query methods .
        public virtual int RetrieveAllFromDBSource()
        {
            QueryObject qo = new QueryObject(this);
            return qo.DoQuery();
        }
        public virtual int RetrieveAllFromDBSource(string orderByAttr)
        {
            QueryObject qo = new QueryObject(this);
            qo.addOrderBy(orderByAttr);
            return qo.DoQuery();
        }
        #endregion  Query methods .

        #region  Filtration 
        public Entity Filter(string key, string val)
        {
            foreach (Entity en in this)
            {
                if (en.GetValStringByKey(key) == val)
                    return en;
            }
            return null;
        }
        public Entity Filter(string key1, string val1, string key2, string val2)
        {
            foreach (Entity en in this)
            {
                if (en.GetValStringByKey(key1) == val1 && en.GetValStringByKey(key2) == val2)
                    return en;
            }
            return null;
        }
        public Entity Filter(string key1, string val1, string key2, string val2, string key3, string val3)
        {
            foreach (Entity en in this)
            {
                if (en.GetValStringByKey(key1) == val1 &&
                    en.GetValStringByKey(key2) == val2 &&
                    en.GetValStringByKey(key3) == val3)
                    return en;
            }
            return null;
        }
        #endregion

        #region  Virtual Methods 
        /// <summary>
        ///  According to attribute query 
        /// </summary>
        /// <param name="attr"> Property name </param>
        /// <param name="val">值</param>
        /// <returns> Whether the query to </returns>
        public int RetrieveByAttr(string attr, object val)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(attr, val);
            return qo.DoQuery();
        }
        public int RetrieveLikeAttr(string attr, string val)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(attr, " like ", val);
            return qo.DoQuery();
        }

        #endregion

        #region  Extended Attributes 
        /// <summary>
        ///  Is not hierarchical dictionary .
        /// </summary>
        public bool IsGradeEntities
        {
            get
            {
                try
                {
                    Attr attr = null;
                    attr = this.GetNewEntity.EnMap.GetAttrByKey("Grade");
                    attr = this.GetNewEntity.EnMap.GetAttrByKey("IsDtl");
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        #endregion

        #region  By datatable  Conversion set for the entity 
        #endregion

        #region  Public Methods 
        /// <summary>
        ///  Write to xml.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public virtual int ExpDataToXml(string file)
        {
            DataTable dt = this.ToDataTableField();
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            ds.WriteXml(file);
            return dt.Rows.Count;
        }
        ///// <summary>
        ///// DBSimpleNoNames
        ///// </summary>
        ///// <returns></returns>
        //public DBSimpleNoNames ToEntitiesNoName(string refNo, string refName)
        //{
        //    DBSimpleNoNames ens = new DBSimpleNoNames();
        //    foreach (Entity en in this)
        //    {
        //        ens.AddByNoName(en.GetValStringByKey(refNo), en.GetValStringByKey(refName));
        //    }
        //    return ens;
        //}
        /// <summary>
        ///  By datatable  Convert this entity set Table One of the primary key column name 
        /// </summary>
        /// <param name="dt">Table</param>
        /// <param name="fieldName"> Field Name , When this field is contained in table  The primary key  </param>
        public void InitCollectionByTable(DataTable dt, string fieldName)
        {
            Entity en = this.GetNewEntity;
            string pk = en.PK;
            foreach (DataRow dr in dt.Rows)
            {
                Entity en1 = this.GetNewEntity;
                en1.SetValByKey(pk, dr[fieldName]);
                en1.Retrieve();
                this.AddEntity(en1);
            }
        }
        /// <summary>
        ///  By datatable  Conversion set for the entity .
        ///  This one Table  The structure requires the same structure and properties .
        /// </summary>
        /// <param name="dt"> Converted to Table</param>
        public void InitCollectionByTable(DataTable dt)
        {
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Entity en = this.GetNewEntity;
                    foreach (Attr attr in en.EnMap.Attrs)
                    {
                        if (attr.MyFieldType == FieldType.RefText)
                        {
                            try
                            {
                                en.Row.SetValByKey(attr.Key, dr[attr.Key]);
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            en.Row.SetValByKey(attr.Key, dr[attr.Key]);
                        }
                    }
                    this.AddEntity(en);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("@ This table can not be converted to a set of detailed error :" + ex.Message);
            }
        }
        /// <summary>
        ///  Two judgment is not the same entity set .
        /// </summary>
        /// <param name="ens"></param>
        /// <returns></returns>
        public bool Equals(Entities ens)
        {
            if (ens.Count != this.Count)
                return false;

            foreach (Entity en in this)
            {
                bool isExits = false;
                foreach (Entity en1 in ens)
                {
                    if (en.PKVal.Equals(en1.PKVal))
                    {
                        isExits = true;
                        break;
                    }
                }
                if (isExits == false)
                    return false;
            }
            return true;
        }
        #endregion

        #region  Extended Attributes 
        //		/// <summary>
        //		///  His related functions .
        //		/// </summary>
        //		public SysUIEnsRefFuncs HisSysUIEnsRefFuncs
        //		{
        //			get
        //			{
        //				return new SysUIEnsRefFuncs(this.ToString()) ; 
        //			}
        //
        //		}
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Constructor 
        /// </summary>
        public Entities() { }
        #endregion

        #region  Public Methods 
        /// <summary>
        ///  Whether there key= val  Entity .
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool IsExits(string key, object val)
        {
            foreach (Entity en in this)
            {
                if (en.GetValStringByKey(key) == val.ToString())
                    return true;
            }
            return false;
        }
        /// <summary>
        ///  Create a new instance of an element of the collection type 
        /// </summary>
        /// <returns></returns>
        public abstract Entity GetNewEntity { get; }
        /// <summary>
        ///  Access to data based on location 
        /// </summary>
        public Entity this[int index]
        {
            get
            {
                return this.InnerList[index] as Entity;
            }
        }
        /// <summary>
        ///  Add objects to the collection at the end of , If the object already exists , Not added 
        /// </summary>
        /// <param name="entity"> The object to add </param>
        /// <returns> Adding to return to the place </returns>
        public virtual int AddEntity(Entity entity)
        {
            return this.InnerList.Add(entity);
        }
        public virtual int AddEntity(Entity entity, int idx)
        {
            this.InnerList.Insert(idx, entity);
            return idx;
        }
        public virtual void AddEntities(Entities ens)
        {
            foreach (Entity en in ens)
                this.AddEntity(en);
            // this.InnerList.AddRange(ens);
        }
        /// <summary>
        ///  Increase entities
        /// </summary>
        /// <param name="pks"> The value of the primary key , Middle with @ Separated symbol </param>
        public virtual void AddEntities(string pks)
        {
            this.Clear();
            if (pks == null || pks == "")
                return;

            string[] strs = pks.Split('@');
            foreach (string str in strs)
            {
                if (str == null || str == "")
                    continue;

                Entity en = this.GetNewEntity;
                en.PKVal = str;
                if (en.RetrieveFromDBSources() == 0)
                    continue;
                this.AddEntity(en);
            }
        }
        public virtual void Insert(int index, Entity entity)
        {
            this.InnerList.Insert(index, entity);
        }
        /// <summary>
        ///  Judgment is not included specified Entity .
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public bool Contains(Entity en)
        {
            return this.Contains(en.PKVal);
        }
        /// <summary>
        ///  This set contains 
        /// </summary>
        /// <param name="ens"></param>
        /// <returns>true / false </returns>
        public bool Contains(Entities ens)
        {
            return this.Contains(ens, ens.GetNewEntity.PK);
        }
        public bool Contains(Entities ens, string key)
        {
            if (ens.Count == 0)
                return false;
            foreach (Entity en in ens)
            {
                if (this.Contains(key, en.GetValByKey(key)) == false)
                    return false;
            }
            return true;
        }
        public bool Contains(Entities ens, string key1, string key2)
        {
            if (ens.Count == 0)
                return false;
            foreach (Entity en in ens)
            {
                if (this.Contains(key1, en.GetValByKey(key1), key2, en.GetValByKey(key2)) == false)
                    return false;
            }
            return true;
        }
        /// <summary>
        ///  Is not contain the specified PK
        /// </summary>
        /// <param name="pkVal"></param>
        /// <returns></returns>
        public bool Contains(object pkVal)
        {
            string pk = this.GetNewEntity.PK;
            return this.Contains(pk, pkVal);
        }
        /// <summary>
        ///  Specified attribute which contains the specified value .
        /// </summary>
        /// <param name="attr"> Specified attribute </param>
        /// <param name="pkVal"> The specified value </param>
        /// <returns> Return is equal to </returns>
        public bool Contains(string attr, object pkVal)
        {
            foreach (Entity myen in this)
            {
                if (myen.GetValByKey(attr).ToString().Equals(pkVal.ToString()))
                    return true;
            }
            return false;
        }
        public bool Contains(string attr1, object pkVal1, string attr2, object pkVal2)
        {
            foreach (Entity myen in this)
            {
                if (myen.GetValByKey(attr1).ToString().Equals(pkVal1.ToString()) && myen.GetValByKey(attr2).ToString().Equals(pkVal2.ToString())
                    )
                    return true;
            }
            return false;
        }
        public bool Contains(string attr1, object pkVal1, string attr2, object pkVal2, string attr3, object pkVal3)
        {
            foreach (Entity myen in this)
            {
                if (myen.GetValByKey(attr1).ToString().Equals(pkVal1.ToString())
                    && myen.GetValByKey(attr2).ToString().Equals(pkVal2.ToString())
                    && myen.GetValByKey(attr3).ToString().Equals(pkVal3.ToString())
                    )
                    return true;
            }
            return false;
        }
        /// <summary>
        ///  Get current collection to pass over the set intersection .
        /// </summary>
        /// <param name="ens"> An entity set </param>
        /// <returns> After comparing collections </returns>
        public Entities GainIntersection(Entities ens)
        {
            Entities myens = this.CreateInstance();
            string pk = this.GetNewEntity.PK;
            foreach (Entity en in this)
            {
                foreach (Entity hisen in ens)
                {
                    if (en.GetValByKey(pk).Equals(hisen.GetValByKey(pk)))
                    {
                        myens.AddEntity(en);
                    }
                }
            }
            return myens;
        }
        /// <summary>
        ///  Create an instance of itself establish .
        /// </summary>
        /// <returns>Entities</returns>
        public Entities CreateInstance()
        {
            return ClassFactory.GetEns(this.ToString());
        }
        #endregion

        #region  Get an entity 
        /// <summary>
        ///  Get an entity 
        /// </summary>
        /// <param name="val">值</param>
        /// <returns></returns>
        public Entity GetEntityByKey(object val)
        {
            string pk = this.GetNewEntity.PK;
            foreach (Entity en in this)
            {
                if (en.GetValStrByKey(pk) == val.ToString())
                    return en;
            }
            return null;
        }
        /// <summary>
        ///  Get an entity 
        /// </summary>
        /// <param name="attr"> Property </param>
        /// <param name="val">值</param>
        /// <returns></returns>
        public Entity GetEntityByKey(string attr, object val)
        {
            foreach (Entity en in this)
            {
                if (en.GetValByKey(attr).Equals(val))
                    return en;
            }
            return null;
        }
        public Entity GetEntityByKey(string attr, int val)
        {
            foreach (Entity en in this)
            {
                if (en.GetValIntByKey(attr) == val)
                    return en;
            }
            return null;
        }
        public Entity GetEntityByKey(string attr1, object val1, string attr2, object val2)
        {
            foreach (Entity en in this)
            {
                if (en.GetValStrByKey(attr1) == val1.ToString()
                    && en.GetValStrByKey(attr2) == val2.ToString())
                    return en;
            }
            return null;
        }
        public Entity GetEntityByKey(string attr1, object val1, string attr2, object val2, string attr3, object val3)
        {
            foreach (Entity en in this)
            {
                if (en.GetValByKey(attr1).Equals(val1)
                    && en.GetValByKey(attr2).Equals(val2)
                    && en.GetValByKey(attr3).Equals(val3))
                    return en;
            }
            return null;
        }
        #endregion

        #region   Operating on a property 
        /// <summary>
        ///  Sum 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public decimal GetSumDecimalByKey(string key)
        {
            decimal sum = 0;
            foreach (Entity en in this)
            {
                sum += en.GetValDecimalByKey(key);
            }
            return sum;
        }
        public decimal GetSumDecimalByKey(string key, string attrOfGroup, object valOfGroup)
        {
            decimal sum = 0;
            foreach (Entity en in this)
            {
                if (en.GetValStrByKey(attrOfGroup) == valOfGroup.ToString())
                    sum += en.GetValDecimalByKey(key);
            }
            return sum;
        }
        public decimal GetAvgDecimalByKey(string key)
        {
            if (this.Count == 0)
                return 0;
            decimal sum = 0;
            foreach (Entity en in this)
            {
                sum += en.GetValDecimalByKey(key);
            }
            return sum / this.Count;
        }
        public decimal GetAvgIntByKey(string key)
        {
            if (this.Count == 0)
                return 0;
            decimal sum = 0;
            foreach (Entity en in this)
            {
                sum = sum + en.GetValDecimalByKey(key);
            }
            return sum / this.Count;
        }
        /// <summary>
        ///  Sum 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetSumIntByKey(string key)
        {
            int sum = 0;
            foreach (Entity en in this)
            {
                sum += en.GetValIntByKey(key);
            }
            return sum;
        }
        /// <summary>
        ///  Sum 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public float GetSumFloatByKey(string key)
        {
            float sum = 0;
            foreach (Entity en in this)
            {
                sum += en.GetValFloatByKey(key);
            }
            return sum;
        }

        /// <summary>
        ///  Number demand 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetCountByKey(string key, string val)
        {
            int sum = 0;
            foreach (Entity en in this)
            {
                if (en.GetValStringByKey(key) == val)
                    sum++;
            }
            return sum;
        }
        public int GetCountByKey(string key, int val)
        {
            int sum = 0;
            foreach (Entity en in this)
            {
                if (en.GetValIntByKey(key) == val)
                    sum++;
            }
            return sum;
        }
        #endregion

        #region  Collection operation 
        /// <summary>
        ///  Loaded into memory 
        /// </summary>
        /// <returns></returns>
        public int FlodInCash()
        {
            //this.Clear();
            QueryObject qo = new QueryObject(this);

            // qo.Top = 2000;
            int num = qo.DoQuery();

            /*  The number of queries added memory  */
            Entity en = this.GetNewEntity;
            CashEntity.PubEns(en.ToString(), this, en.PK);
            BP.DA.Log.DefaultLogWriteLineInfo(" Success [" + en.ToString() + "-" + num + "] Placed in the cache .");
            return num;
        }
        /// <summary>
        ///  Perform a data check 
        /// </summary>
        public string DoDBCheck(DBCheckLevel level)
        {
            return PubClass.DBRpt1(level, this);
        }
        /// <summary>
        ///  Remove the object from the collection 
        /// </summary>
        /// <param name="entity"></param>
        public virtual void RemoveEn(Entity entity)
        {
            this.InnerList.Remove(entity);
        }
        /// <summary>
        ///  Remove 
        /// </summary>
        /// <param name="pk"></param>
        public virtual void RemoveEn(string pk)
        {
            string key = this.GetNewEntity.PK;
            RemoveEn(key, pk);
        }
        public virtual void RemoveEn(string key, string val)
        {
            foreach (Entity en in this)
            {
                if (en.GetValStringByKey(key) == val)
                {
                    this.RemoveEn(en);
                    return;
                }
            }
        }
        public virtual void Remove(string pks)
        {
            string[] mypks = pks.Split('@');
            string pkAttr = this.GetNewEntity.PK;

            foreach (string pk in mypks)
            {
                if (pk == null || pk.Length == 0)
                    continue;

                this.RemoveEn(pkAttr, pk);
            }
        }

        /// <summary>
        ///  Delete table.
        /// </summary>
        /// <returns></returns>
        public int ClearTable()
        {
            Entity en = this.GetNewEntity;
            return en.RunSQL("DELETE FROM " + en.EnMap.PhysicsTable);
        }
        /// <summary>
        ///  Deleted objects within the collection 
        /// </summary>
        public void Delete()
        {
            foreach (Entity en in this)
                if (en.IsExits)
                    en.Delete();
            this.Clear();
        }
        public int RunSQL(string sql)
        {
            return this.GetNewEntity.RunSQL(sql);
        }
        public int Delete(string key, object val)
        {
            Entity en = this.GetNewEntity;
            Paras ps = new Paras();
            ps.SQL = "DELETE FROM " + en.EnMap.PhysicsTable + " WHERE " + key + "=" + en.HisDBVarStr + "p";

            if (val.GetType() != typeof(string))
            {
                Attr attr = en.EnMap.GetAttrByKey(key);
                if (attr.MyDataType == DataType.AppString)
                {
                    ps.Add("p", val.ToString());
                }
                else
                {
                    ps.Add("p", val);
                }
            }
            else
            {
                ps.Add("p", val);
            }
            ps.Add("p", val);
            return en.RunSQL(ps);
        }

        public int Delete(string key1, object val1, string key2, object val2)
        {
            Entity en = this.GetNewEntity;
            Paras ps = new Paras();
            ps.SQL = "DELETE FROM " + en.EnMap.PhysicsTable + " WHERE " + key1 + "=" + en.HisDBVarStr + "p1 AND " + key2 + "=" + en.HisDBVarStr + "p2";
            if (val1.GetType() != typeof(string))
            {
                Attr attr = en.EnMap.GetAttrByKey(key1);
                if (attr.MyDataType == DataType.AppString)
                {
                    ps.Add("p1", val1.ToString());
                }
                else
                {
                    ps.Add("p1", val1);
                }
            }
            else
            {
                ps.Add("p1", val1);
            }

            if (val2.GetType() != typeof(string))
            {
                Attr attr = en.EnMap.GetAttrByKey(key2);
                if (attr.MyDataType == DataType.AppString)
                {
                    ps.Add("p2", val2.ToString());
                }
                else
                {
                    ps.Add("p2", val2);
                }
            }
            else
            {
                ps.Add("p2", val2);
            }


            return en.RunSQL(ps);
        }
        public int Delete(string key1, object val1, string key2, object val2, string key3, object val3)
        {
            Entity en = this.GetNewEntity;
            Paras ps = new Paras();
            ps.SQL = "DELETE FROM " + en.EnMap.PhysicsTable + " WHERE " + key1 + "=" + en.HisDBVarStr + "p1 AND " + key2 + "=" + en.HisDBVarStr + "p2 AND " + key3 + "=" + en.HisDBVarStr + "p3";
            if (val1.GetType() != typeof(string))
            {
                Attr attr = en.EnMap.GetAttrByKey(key1);
                if (attr.MyDataType == DataType.AppString)
                {
                    ps.Add("p1", val1.ToString());
                }
                else
                {
                    ps.Add("p1", val1);
                }
            }
            else
            {
                ps.Add("p1", val1);
            }

            if (val2.GetType() != typeof(string))
            {
                Attr attr = en.EnMap.GetAttrByKey(key2);
                if (attr.MyDataType == DataType.AppString)
                {
                    ps.Add("p2", val2.ToString());
                }
                else
                {
                    ps.Add("p2", val2);
                }
            }
            else
            {
                ps.Add("p2", val2);
            }

            if (val3.GetType() != typeof(string))
            {
                Attr attr = en.EnMap.GetAttrByKey(key3);
                if (attr.MyDataType == DataType.AppString)
                {
                    ps.Add("p3", val3.ToString());
                }
                else
                {
                    ps.Add("p3", val3);
                }
            }
            else
            {
                ps.Add("p3", val3);
            }


            return en.RunSQL(ps);
        }
        public int Delete(string key1, object val1, string key2, object val2, string key3, object val3, string key4, object val4)
        {
            Entity en = this.GetNewEntity;
            Paras ps = new Paras();
            ps.SQL = "DELETE FROM " + en.EnMap.PhysicsTable + " WHERE " + key1 + "=" + en.HisDBVarStr + "p1 AND " + key2 + "=" + en.HisDBVarStr + "p2 AND " + key3 + "=" + en.HisDBVarStr + "p3 AND " + key4 + "=" + en.HisDBVarStr + "p4";
            if (val1.GetType() != typeof(string))
            {
                Attr attr = en.EnMap.GetAttrByKey(key1);
                if (attr.MyDataType == DataType.AppString)
                {
                    ps.Add("p1", val1.ToString());
                }
                else
                {
                    ps.Add("p1", val1);
                }
            }
            else
            {
                ps.Add("p1", val1);
            }

            if (val2.GetType() != typeof(string))
            {
                Attr attr = en.EnMap.GetAttrByKey(key2);
                if (attr.MyDataType == DataType.AppString)
                {
                    ps.Add("p2", val2.ToString());
                }
                else
                {
                    ps.Add("p2", val2);
                }
            }
            else
            {
                ps.Add("p2", val2);
            }

            if (val3.GetType() != typeof(string))
            {
                Attr attr = en.EnMap.GetAttrByKey(key3);
                if (attr.MyDataType == DataType.AppString)
                {
                    ps.Add("p3", val3.ToString());
                }
                else
                {
                    ps.Add("p3", val3);
                }
            }
            else
            {
                ps.Add("p3", val3);
            }

            if (val4.GetType() != typeof(string))
            {
                Attr attr = en.EnMap.GetAttrByKey(key4);
                if (attr.MyDataType == DataType.AppString)
                {
                    ps.Add("p4", val4.ToString());
                }
                else
                {
                    ps.Add("p4", val4);
                }
            }
            else
            {
                ps.Add("p4", val4);
            }
            return en.RunSQL(ps);
        }
        /// <summary>
        ///  Updated collection of objects within 
        /// </summary>
        public void Update()
        {
            //string msg="";
            foreach (Entity en in this)
                en.Update();

        }
        /// <summary>
        ///  Save 
        /// </summary>
        public void Save()
        {
            foreach (Entity en in this)
                en.Save();
        }
        public void SaveToXml(string file)
        {
            string dir = "";

            if (file.Contains("\\"))
            {
                dir = file.Substring(0, file.LastIndexOf('\\'));
            }
            else if (file.Contains("/"))
            {
                dir = file.Substring(0, file.LastIndexOf("/"));
            }

            if (dir != "")
            {
                if (System.IO.Directory.Exists(dir) == false)
                {
                    System.IO.Directory.CreateDirectory(dir);
                }
            }

            DataSet ds = this.ToDataSet();
            ds.WriteXml(file);
        }
        #endregion

        #region  Query methods 
        public int RetrieveByKeyNoConnection(string attr, object val)
        {
            Entity en = this.GetNewEntity;
            string pk = en.PK;

            DataTable dt = DBAccess.RunSQLReturnTable("SELECT " + pk + " FROM " + this.GetNewEntity.EnMap.PhysicsTable + " WHERE " + attr + "=" + en.HisDBVarStr + "v", "v", val);
            foreach (DataRow dr in dt.Rows)
            {
                Entity en1 = this.GetNewEntity;
                en1.SetValByKey(pk, dr[0]);
                en1.Retrieve();
                this.AddEntity(en1);
            }
            return dt.Rows.Count;
        }
        /// <summary>
        ///  According to the keyword query .
        ///  Description Here is Attrs Accept 
        /// </summary>
        /// <param name="key"> Keyword </param>
        /// <param name="al"> Entity </param>
        /// <returns> Return Table</returns>
        public DataTable RetrieveByKeyReturnTable(string key, Attrs attrs)
        {
            QueryObject qo = new QueryObject(this);

            // 在 Normal  Property which increase , Query conditions .
            Map map = this.GetNewEntity.EnMap;
            qo.addLeftBracket();
            foreach (Attr en in map.Attrs)
            {
                if (en.UIContralType == UIContralType.DDL || en.UIContralType == UIContralType.CheckBok)
                    continue;
                qo.addOr();
                qo.AddWhere(en.Key, " LIKE ", key);
            }
            qo.addRightBracket();

            //            //
            //			Attrs searchAttrs = map.SearchAttrs;
            //			foreach(Attr attr  in attrs)
            //			{				
            //				qo.addAnd();
            //				qo.addLeftBracket();
            //				qo.AddWhere(attr.Key, attr.DefaultVal.ToString() ) ;
            //				qo.addRightBracket();
            //			}
            return qo.DoQueryToTable();
        }
        /// <summary>
        ///  According to KEY  Find .
        /// </summary>
        /// <param name="keyVal">KEY</param>
        /// <returns> Find out the number of return towards .</returns>
        public int RetrieveByKey(string keyVal)
        {
            keyVal = "%" + keyVal.Trim() + "%";
            QueryObject qo = new QueryObject(this);
            Attrs attrs = this.GetNewEntity.EnMap.Attrs;
            //qo.addLeftBracket();
            string pk = this.GetNewEntity.PK;
            if (pk != "OID")
                qo.AddWhere(this.GetNewEntity.PK, " LIKE ", keyVal);
            foreach (Attr en in attrs)
            {

                if (en.UIContralType == UIContralType.DDL || en.UIContralType == UIContralType.CheckBok)
                    continue;

                if (en.Key == pk)
                    continue;

                if (en.MyDataType != DataType.AppString)
                    continue;

                if (en.MyFieldType == FieldType.FK)
                    continue;

                if (en.MyFieldType == FieldType.RefText)
                    continue;

                qo.addOr();
                qo.AddWhere(en.Key, " LIKE ", keyVal);
            }
            //qo.addRightBracket();
            return qo.DoQuery();
        }
        /// <summary>
        /// 按LIKE  To investigate .
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vals"></param>
        /// <returns></returns>
        public int RetrieveByLike(string key, string vals)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(key, " LIKE ", vals);
            return qo.DoQuery();
        }

        /// <summary>
        ///   Check out , Bear pks  String .
        ///   Proportion :"001,002,003"
        /// </summary>
        /// <returns></returns>
        public int Retrieve(string pks)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(this.GetNewEntity.PK, " in ", pks);
            return qo.DoQuery();
        }
        public int RetrieveInSQL(string attr, string sql)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhereInSQL(attr, sql);
            return qo.DoQuery();
        }
        public int RetrieveInSQL(string attr, string sql, string orderBy)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhereInSQL(attr, sql);
            qo.addOrderBy(orderBy);
            return qo.DoQuery();
        }

        public int RetrieveInSQL(string sql)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhereInSQL(this.GetNewEntity.PK, sql);
            return qo.DoQuery();
        }
        public int RetrieveInSQL_Order(string sql, string orderby)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhereInSQL(this.GetNewEntity.PK, sql);
            qo.addOrderBy(orderby);
            return qo.DoQuery();
        }
        public int Retrieve(string key, bool val)
        {
            QueryObject qo = new QueryObject(this);
            if (val)
                qo.AddWhere(key, 1);
            else
                qo.AddWhere(key, 0);
            return qo.DoQuery();
        }
        public int Retrieve(string key, object val)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(key, val);
            return qo.DoQuery();
        }
        public int Retrieve(string key, object val, string orderby)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(key, val);
            //qo.addOrderByDesc(orderby);
            qo.addOrderBy(orderby); // This sort do not change , Otherwise it will affect other areas .
            return qo.DoQuery();
        }

        public int Retrieve(string key, object val, string key2, object val2)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(key, val);
            qo.addAnd();
            qo.AddWhere(key2, val2);
            return qo.DoQuery();
        }
        public int Retrieve(string key, object val, string key2, object val2, string ordery)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(key, val);
            qo.addAnd();
            qo.AddWhere(key2, val2);
            qo.addOrderBy(ordery);
            return qo.DoQuery();
        }
        public int Retrieve(string key, object val, string key2, object val2, string key3, object val3)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(key, val);
            qo.addAnd();
            qo.AddWhere(key2, val2);
            qo.addAnd();
            qo.AddWhere(key3, val3);
            return qo.DoQuery();
        }

        public int Retrieve(string key, object val, string key2, object val2, string key3, object val3, string key4, object val4)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(key, val);
            qo.addAnd();
            qo.AddWhere(key2, val2);
            qo.addAnd();
            qo.AddWhere(key3, val3);
            qo.addAnd();
            qo.AddWhere(key4, val4);
            return qo.DoQuery();
        }
        public int Retrieve(string key, object val, string key2, object val2, string key3, object val3, string key4, object val4, string orderBy)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(key, val);
            qo.addAnd();
            qo.AddWhere(key2, val2);
            qo.addAnd();
            qo.AddWhere(key3, val3);
            qo.addAnd();
            qo.AddWhere(key4, val4);
            qo.addOrderBy(orderBy);
            return qo.DoQuery();
        }
        /// <summary>
        ///  Check all 
        /// </summary>
        /// <returns></returns>
        public virtual int RetrieveAll()
        {
            return this.RetrieveAll(null);
        }
        public virtual int RetrieveAllOrderByRandom()
        {
            QueryObject qo = new QueryObject(this);
            qo.addOrderByRandom();
            return qo.DoQuery();
        }
        public virtual int RetrieveAllOrderByRandom(int topNum)
        {
            QueryObject qo = new QueryObject(this);
            qo.Top = topNum;
            qo.addOrderByRandom();
            return qo.DoQuery();
        }
        public virtual int RetrieveAll(int topNum, string orderby)
        {
            QueryObject qo = new QueryObject(this);
            qo.Top = topNum;
            qo.addOrderBy(orderby);
            return qo.DoQuery();
        }
        public virtual int RetrieveAll(int topNum, string orderby, bool isDesc)
        {
            QueryObject qo = new QueryObject(this);
            qo.Top = topNum;
            if (isDesc)
                qo.addOrderByDesc(orderby);
            else
                qo.addOrderBy(orderby);
            return qo.DoQuery();
        }
        /// <summary>
        ///  Check all 
        /// </summary>
        /// <returns></returns>
        public virtual int RetrieveAll(string orderBy)
        {
            QueryObject qo = new QueryObject(this);
            if (orderBy != null)
                qo.addOrderBy(orderBy);
            return qo.DoQuery();
        }
        /// <summary>
        ///  Check all .
        /// </summary>
        /// <returns></returns>
        public virtual int RetrieveAll(string orderBy1, string orderBy2)
        {
            QueryObject qo = new QueryObject(this);
            if (orderBy1 != null)
                qo.addOrderBy(orderBy1, orderBy2);
            return qo.DoQuery();
        }
        /// <summary>
        ///  According to the maximum number of queries .
        /// </summary>
        /// <param name="MaxNum"> Maximum NUM</param>
        /// <returns></returns>
        public int RetrieveAll(int MaxNum)
        {
            QueryObject qo = new QueryObject(this);
            qo.Top = MaxNum;
            return qo.DoQuery();
        }
        /// <summary>
        ///  All of the result in the query DataTable.
        /// </summary>
        /// <returns></returns>
        public DataTable RetrieveAllToTable()
        {
            QueryObject qo = new QueryObject(this);
            return qo.DoQueryToTable();
        }
        private DataTable DealBoolTypeInDataTable(Entity en, DataTable dt)
        {

            foreach (Attr attr in en.EnMap.Attrs)
            {
                if (attr.MyDataType == DataType.AppBoolean)
                {
                    DataColumn col = new DataColumn();
                    col.ColumnName = "tmp" + attr.Key;
                    col.DataType = typeof(bool);
                    dt.Columns.Add(col);
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr[attr.Key].ToString() == "1")
                            dr["tmp" + attr.Key] = true;
                        else
                            dr["tmp" + attr.Key] = false;
                    }
                    dt.Columns.Remove(attr.Key);
                    dt.Columns["tmp" + attr.Key].ColumnName = attr.Key;
                    continue;
                }
                if (attr.MyDataType == DataType.AppDateTime || attr.MyDataType == DataType.AppDate)
                {
                    DataColumn col = new DataColumn();
                    col.ColumnName = "tmp" + attr.Key;
                    col.DataType = typeof(DateTime);
                    dt.Columns.Add(col);
                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                            dr["tmp" + attr.Key] = DateTime.Parse(dr[attr.Key].ToString());
                        }
                        catch
                        {
                            if (attr.DefaultVal.ToString() == "")
                                dr["tmp" + attr.Key] = DateTime.Now;
                            else
                                dr["tmp" + attr.Key] = DateTime.Parse(attr.DefaultVal.ToString());

                        }

                    }
                    dt.Columns.Remove(attr.Key);
                    dt.Columns["tmp" + attr.Key].ColumnName = attr.Key;
                    continue;
                }
            }
            return dt;
        }

        /// <summary>
        ///  All of the result in the query RetrieveAllToDataSet.
        ///  Information including their associated .
        /// </summary>
        /// <returns></returns>
        public DataSet RetrieveAllToDataSet()
        {
            #region  Form dataset
            Entity en = this.GetNewEntity;
            DataSet ds = new DataSet(this.ToString());
            QueryObject qo = new QueryObject(this);
            DataTable dt = qo.DoQueryToTable();
            dt.TableName = en.EnMap.PhysicsTable;
            ds.Tables.Add(dt);
            foreach (Attr attr in en.EnMap.Attrs)
            {
                if (attr.MyFieldType == FieldType.FK || attr.MyFieldType == FieldType.PKFK)
                {
                    Entities ens = attr.HisFKEns;
                    QueryObject qo1 = new QueryObject(ens);
                    DataTable dt1 = qo1.DoQueryToTable();
                    dt1.TableName = ens.GetNewEntity.EnMap.PhysicsTable;
                    ds.Tables.Add(dt1);

                    ///  Join relationship 
                    DataColumn parentCol;
                    DataColumn childCol;
                    parentCol = dt.Columns[attr.Key];
                    childCol = dt1.Columns[attr.UIRefKeyValue];
                    DataRelation relCustOrder = new DataRelation(attr.Key, parentCol, childCol);
                    ds.Relations.Add(relCustOrder);
                    continue;
                }
                else if (attr.MyFieldType == FieldType.Enum || attr.MyFieldType == FieldType.PKEnum)
                {
                    DataTable dt1 = DBAccess.RunSQLReturnTable("select * from sys_enum WHERE enumkey=" + en.HisDBVarStr + "k", "k", attr.UIBindKey);
                    dt1.TableName = attr.UIBindKey;
                    ds.Tables.Add(dt1);

                    ///  Join relationship 
                    DataColumn parentCol;
                    DataColumn childCol;
                    parentCol = dt.Columns[attr.Key];
                    childCol = dt1.Columns["IntKey"];
                    DataRelation relCustOrder = new DataRelation(attr.Key, childCol, parentCol);
                    ds.Relations.Add(relCustOrder);

                }
            }
            #endregion

            return ds;
        }
        /// <summary>
        ///  To convert the database into the current collection of entities Dataset.
        /// </summary>
        /// <returns></returns>
        public DataSet ToDataSet()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(this.ToDataTableField());
            return ds;
        }
        public DataTable ToDataTableField()
        {
            return ToDataTableField("dt");
        }
        public DataTable ToDataTableStringField()
        {
            return ToDataTableStringField("dt");
        }
        public DataTable ToDataTableStringField(string tableName)
        {
            DataTable dt = this.ToEmptyTableStringField();
            Entity en = this.GetNewEntity;

            dt.TableName = tableName;
            foreach (Entity myen in this)
            {
                DataRow dr = dt.NewRow();
                foreach (Attr attr in en.EnMap.Attrs)
                {
                    if (attr.MyDataType == DataType.AppBoolean)
                    {
                        if (myen.GetValIntByKey(attr.Key) == 1)
                            dr[attr.Key] = "1";
                        else
                            dr[attr.Key] = "0";
                        continue;
                    }

                    /* If it is a foreign key   We must remove the spaces around .
                     *  */
                    if (attr.MyFieldType == FieldType.FK
                        || attr.MyFieldType == FieldType.PKFK)
                        dr[attr.Key] = myen.GetValByKey(attr.Key).ToString().Trim();
                    else
                        dr[attr.Key] = myen.GetValByKey(attr.Key);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /// <summary>
        ///  To convert the database into the current collection of entities Table.
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable ToDataTableField(string tableName)
        {
            DataTable dt = this.ToEmptyTableField();

            Entity en = this.GetNewEntity;
            Attrs attrs = en.EnMap.Attrs;

            dt.TableName = tableName;
            foreach (Entity myen in this)
            {
                DataRow dr = dt.NewRow();
                foreach (Attr attr in attrs)
                {
                    if (attr.MyDataType == DataType.AppBoolean)
                    {
                        if (myen.GetValIntByKey(attr.Key) == 1)
                            dr[attr.Key] = "1";
                        else
                            dr[attr.Key] = "0";
                        continue;
                    }

                    /* If it is a foreign key   We must remove the spaces around .
                     *  */
                    if (attr.MyFieldType == FieldType.FK
                        || attr.MyFieldType == FieldType.PKFK)
                        dr[attr.Key] = myen.GetValByKey(attr.Key).ToString().Trim();
                    else
                        dr[attr.Key] = myen.GetValByKey(attr.Key);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public DataTable ToDataTableDesc()
        {
            DataTable dt = this.ToEmptyTableDesc();
            Entity en = this.GetNewEntity;

            dt.TableName = en.EnMap.PhysicsTable;
            foreach (Entity myen in this)
            {
                DataRow dr = dt.NewRow();
                foreach (Attr attr in en.EnMap.Attrs)
                {

                    if (attr.MyDataType == DataType.AppBoolean)
                    {
                        if (myen.GetValBooleanByKey(attr.Key))
                            dr[attr.Desc] = "是";
                        else
                            dr[attr.Desc] = "否";
                        continue;
                    }
                    dr[attr.Desc] = myen.GetValByKey(attr.Key);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public DataTable ToEmptyTableDescField()
        {
            DataTable dt = new DataTable();
            Entity en = this.GetNewEntity;
            try
            {

                foreach (Attr attr in en.EnMap.Attrs)
                {
                    //if (attr.UIVisible == false)
                    //    continue;

                    //if (attr.MyFieldType == FieldType.Enum && attr.MyDataType == DataType.AppInt )
                    //    continue;

                    switch (attr.MyDataType)
                    {
                        case DataType.AppString:
                            dt.Columns.Add(new DataColumn(attr.Desc.Trim() + attr.Key, typeof(string)));
                            break;
                        case DataType.AppInt:
                            dt.Columns.Add(new DataColumn(attr.Desc.Trim() + attr.Key, typeof(int)));
                            break;
                        case DataType.AppFloat:
                            dt.Columns.Add(new DataColumn(attr.Desc.Trim() + attr.Key, typeof(float)));
                            break;
                        case DataType.AppBoolean:
                            dt.Columns.Add(new DataColumn(attr.Desc.Trim() + attr.Key, typeof(string)));
                            break;
                        case DataType.AppDouble:
                            dt.Columns.Add(new DataColumn(attr.Desc.Trim() + attr.Key, typeof(double)));
                            break;
                        case DataType.AppMoney:
                            dt.Columns.Add(new DataColumn(attr.Desc.Trim() + attr.Key, typeof(double)));
                            break;
                        case DataType.AppDate:
                            dt.Columns.Add(new DataColumn(attr.Desc.Trim() + attr.Key, typeof(string)));
                            break;
                        case DataType.AppDateTime:
                            dt.Columns.Add(new DataColumn(attr.Desc.Trim() + attr.Key, typeof(string)));
                            break;
                        default:
                            throw new Exception("@bulider insert sql error:  Without this type of data ");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(en.EnDesc + " error " + ex.Message);

            }
            return dt;
        }
        public DataTable ToDataTableDescField()
        {
            DataTable dt = this.ToEmptyTableDescField();
            Entity en = this.GetNewEntity;

            dt.TableName = en.EnMap.PhysicsTable;
            foreach (Entity myen in this)
            {
                DataRow dr = dt.NewRow();
                foreach (Attr attr in en.EnMap.Attrs)
                {
                    //if (attr.UIVisible == false)
                    //    continue;

                    //if (attr.MyFieldType == FieldType.Enum && attr.MyDataType == DataType.AppInt)
                    //    continue;

                    if (attr.MyDataType == DataType.AppBoolean)
                    {
                        if (myen.GetValBooleanByKey(attr.Key))
                            dr[attr.Desc.Trim() + attr.Key] = "是";
                        else
                            dr[attr.Desc.Trim() + attr.Key] = "否";
                        continue;
                    }
                    dr[attr.Desc.Trim() + attr.Key] = myen.GetValByKey(attr.Key);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        ///  The system entities PK Converted to string
        ///  Such as : "001,002,003,".
        /// </summary>
        /// <param name="flag"> Split Symbols ,  In general use  ' ; '</param>
        /// <returns> Transformed string /  Entity set is empty then  return null</returns>
        public string ToStringOfPK(string flag, bool isCutEndFlag)
        {
            string pk = null;
            foreach (Entity en in this)
            {
                pk += en.PKVal + flag;
            }
            if (isCutEndFlag)
                pk = pk.Substring(0, pk.Length - 1);

            return pk;
        }
        /// <summary>
        ///  The system entities PK Converted to  string
        ///  Such as : "001,002,003,".
        /// </summary>		 
        /// <returns> Transformed string /  Entity set is empty then  return null</returns>
        public string ToStringOfSQLModelByPK()
        {
            if (this.Count == 0)
                return "''";
            return ToStringOfSQLModelByKey(this[0].PK);
        }
        /// <summary>
        ///  The system entities PK Converted to  string
        ///  Such as : "001,002,003,".
        /// </summary>		 
        /// <returns> Transformed string /  Entity set is empty then  return "''"</returns>
        public string ToStringOfSQLModelByKey(string key)
        {
            if (this.Count == 0)
                return "''";

            string pk = null;
            foreach (Entity en in this)
            {
                pk += "'" + en.GetValStringByKey(key) + "',";
            }

            pk = pk.Substring(0, pk.Length - 1);

            return pk;
        }

        /// <summary>
        ///  Empty Table
        ///  Take into an empty table structure .
        /// </summary>
        /// <returns></returns>
        public DataTable ToEmptyTableField()
        {
            DataTable dt = new DataTable();
            Entity en = this.GetNewEntity;
            dt.TableName = en.EnMap.PhysicsTable;

            foreach (Attr attr in en.EnMap.Attrs)
            {
                switch (attr.MyDataType)
                {
                    case DataType.AppString:
                        dt.Columns.Add(new DataColumn(attr.Key, typeof(string)));
                        break;
                    case DataType.AppInt:
                        dt.Columns.Add(new DataColumn(attr.Key, typeof(int)));
                        break;
                    case DataType.AppFloat:
                        dt.Columns.Add(new DataColumn(attr.Key, typeof(float)));
                        break;
                    case DataType.AppBoolean:
                        dt.Columns.Add(new DataColumn(attr.Key, typeof(string)));
                        break;
                    case DataType.AppDouble:
                        dt.Columns.Add(new DataColumn(attr.Key, typeof(double)));
                        break;
                    case DataType.AppMoney:
                        dt.Columns.Add(new DataColumn(attr.Key, typeof(double)));
                        break;
                    case DataType.AppDate:
                        dt.Columns.Add(new DataColumn(attr.Key, typeof(string)));
                        break;
                    case DataType.AppDateTime:
                        dt.Columns.Add(new DataColumn(attr.Key, typeof(string)));
                        break;
                    default:
                        throw new Exception("@bulider insert sql error:  Without this type of data ");
                }
            }
            return dt;
        }
        public DataTable ToEmptyTableStringField()
        {
            DataTable dt = new DataTable();
            Entity en = this.GetNewEntity;
            dt.TableName = en.EnMap.PhysicsTable;

            foreach (Attr attr in en.EnMap.Attrs)
            {
                dt.Columns.Add(new DataColumn(attr.Key, typeof(string)));
            }
            return dt;
        }
        public DataTable ToEmptyTableDesc()
        {
            DataTable dt = new DataTable();
            Entity en = this.GetNewEntity;
            try
            {

                foreach (Attr attr in en.EnMap.Attrs)
                {
                    switch (attr.MyDataType)
                    {
                        case DataType.AppString:
                            dt.Columns.Add(new DataColumn(attr.Desc, typeof(string)));
                            break;
                        case DataType.AppInt:
                            dt.Columns.Add(new DataColumn(attr.Desc, typeof(int)));
                            break;
                        case DataType.AppFloat:
                            dt.Columns.Add(new DataColumn(attr.Desc, typeof(float)));
                            break;
                        case DataType.AppBoolean:
                            dt.Columns.Add(new DataColumn(attr.Desc, typeof(string)));
                            break;
                        case DataType.AppDouble:
                            dt.Columns.Add(new DataColumn(attr.Desc, typeof(double)));
                            break;
                        case DataType.AppMoney:
                            dt.Columns.Add(new DataColumn(attr.Desc, typeof(double)));
                            break;
                        case DataType.AppDate:
                            dt.Columns.Add(new DataColumn(attr.Desc, typeof(string)));
                            break;
                        case DataType.AppDateTime:
                            dt.Columns.Add(new DataColumn(attr.Desc, typeof(string)));
                            break;
                        default:
                            throw new Exception("@bulider insert sql error:  Without this type of data ");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(en.EnDesc + " error " + ex.Message);

            }
            return dt;
        }
        #endregion

        #region  Grouping method 
        #endregion

        #region  Inquiry from cash
        /// <summary>
        ///  Query Cache :  According to  in sql  Fashion .
        /// </summary>
        /// <param name="cashKey"> Specify cache Key, Global variables do not repeat .</param>
        /// <param name="inSQL">sql  Statement </param>
        /// <returns> Return result sets in the cache inside </returns>
        public int RetrieveFromCashInSQL(string cashKey, string inSQL)
        {
            this.Clear();
            Entities ens = Cash.GetEnsDataExt(cashKey) as Entities;
            if (ens == null)
            {
                QueryObject qo = new QueryObject(this);
                qo.AddWhereInSQL(this.GetNewEntity.PK, inSQL);
                qo.DoQuery();
                Cash.SetEnsDataExt(cashKey, this);
            }
            else
            {
                this.AddEntities(ens);
            }
            return this.Count;
        }
        /// <summary>
        ///  Query Cache :  According to the relevant conditions 
        /// </summary>
        /// <param name="attrKey"> Property :  Such as  FK_Sort</param>
        /// <param name="val">值:  Such as :01 </param>
        /// <param name="top"> The maximum value of information </param>
        /// <param name="orderBy"> Sort Field </param>
        /// <param name="isDesc"></param>
        /// <returns> Return result sets in the cache inside </returns>
        public int RetrieveFromCash(string attrKey, object val, int top, string orderBy, bool isDesc)
        {
            string cashKey = this.ToString() + attrKey + val + top + orderBy + isDesc;
            this.Clear();
            Entities ens = Cash.GetEnsDataExt(cashKey);
            if (ens == null)
            {
                QueryObject qo = new QueryObject(this);
                qo.Top = top;

                if (attrKey == "" || attrKey == null)
                {
                }
                else
                {
                    qo.AddWhere(attrKey, val);
                }

                if (orderBy != null)
                {
                    if (isDesc)
                        qo.addOrderByDesc(orderBy);
                    else
                        qo.addOrderBy(orderBy);
                }

                qo.DoQuery();
                Cash.SetEnsDataExt(cashKey, this);
            }
            else
            {
                this.AddEntities(ens);
            }
            return this.Count;
        }
        /// <summary>
        ///  Query Cache :  According to the relevant conditions 
        /// </summary>
        /// <param name="attrKey"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public int RetrieveFromCash(string attrKey, object val)
        {
            return RetrieveFromCash(attrKey, val, 99999, null, true);
        }
        /// <summary>
        ///  Query Cache :  According to the relevant conditions 
        /// </summary>
        /// <param name="attrKey"></param>
        /// <param name="val"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public int RetrieveFromCash(string attrKey, object val, string orderby)
        {
            return RetrieveFromCash(attrKey, val, 99999, orderby, true);
        }
        /// <summary>
        ///  Query Cache :  According to the relevant conditions 
        /// </summary>
        /// <param name="top"></param>
        /// <param name="orderBy"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        public int RetrieveFromCash(string orderBy, bool isDesc, int top)
        {
            return RetrieveFromCash(null, null, top, orderBy, isDesc);
        }
        #endregion

        #region  Contains methods 
        /// <summary>
        ///  Whether to include any one entity primary key number 
        /// </summary>
        /// <param name="keys"> With multiple primary keys , Separate accord </param>
        /// <returns>true Containing any one ,fale  One not included .</returns>
        public bool ContainsAnyOnePK(string keys)
        {
            keys=","+keys+",";
            foreach (Entity en in this)
            {
                if (keys.Contains("," + en.PKVal + ",") == true)
                    return true;
            }
            return false;
        }
        /// <summary>
        ///  Contains all of the primary key 
        /// </summary>
        /// <param name="keys"> With multiple primary keys , Separate accord </param>
        /// <returns>true All included .</returns>
        public bool ContainsAllPK(string keys)
        {
            keys = "," + keys + ",";
            foreach (Entity en in this)
            {
                if (keys.Contains("," + en.PKVal + ",") == false)
                    return false;
            }
            return true;
        }
        #endregion
    }
}
