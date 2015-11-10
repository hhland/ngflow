using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Pub;
using BP.Sys;

namespace BP.En
{
    /// <summary>
    /// QueryObject  The summary .
    /// </summary>
    public class QueryObject
    {
        private Entity _en = null;
        private Entities _ens = null;
        private string _sql = "";
        private Entity En
        {
            get
            {
                if (this._en == null)
                    return this.Ens.GetNewEntity;
                else
                    return this._en;
            }
            set
            {
                this._en = value;
            }
        }
        private Entities Ens
        {
            get
            {
                return this._ens;
            }
            set
            {
                this._ens = value;
            }
        }
        /// <summary>
        ///  Deal with Order by , group by . 
        /// </summary>
        private string _groupBy = "";
        /// <summary>
        ///  To get the query sql .
        /// </summary>
        public string SQL
        {
            get
            {
                string sql = "";
                string selecSQL = SqlBuilder.SelectSQL(this.En, this.Top);
                if (this._sql == null || this._sql.Length == 0)
                    sql = selecSQL + this._groupBy + this._orderBy;
                else
                {
                    if (selecSQL.Contains(" WHERE "))
                        sql = selecSQL + "  AND ( " + this._sql + " ) " + _groupBy + this._orderBy;
                    else
                        sql = selecSQL + " WHERE   ( " + this._sql + " ) " + _groupBy + this._orderBy;
                }


                sql = sql.Replace("  ", " ");
                sql = sql.Replace("  ", " ");

                sql = sql.Replace("WHERE AND", "WHERE");
                sql = sql.Replace("WHERE  AND", "WHERE");

                sql = sql.Replace("WHERE ORDER", "ORDER");
                return sql;
            }
            set
            {
                if (value.IndexOf("(") == -1)
                    this.IsEndAndOR = false;
                else
                    this.IsEndAndOR = true;

                this._sql = this._sql + " " + value;
            }
        }
        public string SQLWithOutPara
        {
            get
            {
                string sql = this.SQL;
                foreach (Para en in this.MyParas)
                {
                    sql = sql.Replace(SystemConfig.AppCenterDBVarStr + en.ParaName, "'" + en.val.ToString() + "'");
                }
                return sql;
            }
        }
        public void AddWhere(string str)
        {
            this._sql = this._sql + " " + str;
        }
        /// <summary>
        ///  Modified on 2009 -05-12 
        /// </summary>
        private int _Top = -1;
        public int Top
        {
            get
            {
                return _Top;
            }
            set
            {
                this._Top = value;
            }
        }
        private Paras _Paras = null;
        public Paras MyParas
        {
            get
            {
                if (_Paras == null)
                    _Paras = new Paras();
                return _Paras;
            }
            set
            {
                _Paras = value;
            }
        }
        private Paras _ParasR = null;
        public Paras MyParasR
        {
            get
            {
                if (_ParasR == null)
                    _ParasR = new Paras();
                return _ParasR;
            }
        }
        public void AddPara(string key, object v)
        {
            key = "P" + key;
            this.MyParas.Add(key, v);
        }
        public QueryObject()
        {
        }
        /// DictBase
        public QueryObject(Entity en)
        {
            this.MyParas.Clear();
            this._en = en;
            this.HisDBType = this._en.EnMap.EnDBUrl.DBType;
        }
        public QueryObject(Entities ens)
        {
            this.MyParas.Clear();
            ens.Clear();
            this._ens = ens;
            this.HisDBType = this._ens.GetNewEntity.EnMap.EnDBUrl.DBType;
        }
        public BP.DA.DBType HisDBType = DBType.MSSQL;
        public string HisVarStr
        {
            get
            {
                switch (this.HisDBType)
                {
                    case DBType.MSSQL:
                    case DBType.Access:
                    case DBType.MySQL:
                        return "@";
                    case DBType.Informix:
                        return "?";
                    default:
                        return ":";
                }
            }
        }
        /// <summary>
        ///  Increasing function search ．
        /// </summary>
        /// <param name="attr"> Property </param>
        /// <param name="exp"> Expression format   Greater than , Equal , Less than </param>
        /// <param name="len"> Length </param>
        public void AddWhereLen(string attr, string exp, int len, BP.DA.DBType dbtype)
        {
            this.SQL = "( " + BP.Sys.SystemConfig.AppCenterDBLengthStr + "( " + attr2Field(attr) + " ) " + exp + " '" + len.ToString() + "')";
        }
        /// <summary>
        ///  Increase the query , Conditions of use  IN  Representation ．sql Must be a collection of columns ．
        /// </summary>
        /// <param name="attr"> Property </param>
        /// <param name="sql">此sql, There must be a set of columns ．</param>
        public void AddWhereInSQL(string attr, string sql)
        {
            this.AddWhere(attr, " IN ", "( " + sql + " )");
        }
        /// <summary>
        ///  Increase the query , Conditions of use  IN  Representation ．sql Must be a collection of columns ．
        /// </summary>
        /// <param name="attr"> Property </param>
        /// <param name="sql">此sql, There must be a set of columns ．</param>
        public void AddWhereInSQL(string attr, string sql, string orderBy)
        {
            this.AddWhere(attr, " IN ", "( " + sql + " )");
            this.addOrderBy(orderBy);
        }
        /// <summary>
        ///  Increase the query , Conditions of use  IN  Representation ．sql Must be a collection of columns ．
        /// </summary>
        /// <param name="attr"> Property </param>
        /// <param name="sql">此sql, There must be a set of columns ．</param>
        public void AddWhereNotInSQL(string attr, string sql)
        {
            this.AddWhere(attr, " NOT IN ", " ( " + sql + " ) ");
        }
        public void AddWhereNotIn(string attr, string val)
        {
            this.AddWhere(attr, " NOT IN ", " ( " + val + " ) ");
        }
        /// <summary>
        ///  Increased Conditions , DataTable  Value of the first column ．
        /// </summary>
        /// <param name="attr"> Property </param>
        /// <param name="dt"> The first column is to be combined values</param>
        public void AddWhereIn(string attr, DataTable dt)
        {
            string strs = "";
            foreach (DataRow dr in dt.Rows)
            {
                strs += dr[0].ToString() + ",";
            }
            strs = strs.Substring(strs.Length - 1, 0);
            this.AddWhereIn(attr, strs);
        }
        /// <summary>
        ///  Increased Conditions ,vals  Must be sql String can be identified ．
        /// </summary>
        /// <param name="attr"> Property </param>
        /// <param name="vals">用 ,  Separate ．</param>
        public void AddWhereIn(string attr, string vals)
        {
            this.AddWhere(attr, " IN ", vals);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="exp"></param>
        /// <param name="val"></param>
        public void AddWhere(string attr, string exp, string val)
        {
            AddWhere(attr, exp, val, null);
        }
        /// <summary>
        ///  Increased Conditions 
        /// </summary>
        /// <param name="attr"> Property </param>
        /// <param name="exp"> Operation Symbol （ Depending on the database ）</param>
        /// <param name="val">值</param>
        /// <param name="paraName"> Parameter name , For null,  If there are multiple parameters in the query need the same attribute name , Respectively, gave them a parameter name .</param>
        public void AddWhere(string attr, string exp, string val, string paraName)
        {
            if (val == null)
                val = "";

            if (val == "all")
            {
                this.SQL = "( 1=1 )";
                return;
            }

            if (exp.ToLower().Contains(" in"))
            {
                this.SQL = "( " + attr2Field(attr) + " " + exp + "  " + val + " )";
                return;
            }

            if (exp.ToLower().Contains("like"))
            {
                if (attr == "FK_Dept")
                {
                    val = val.Replace("'", "");
                    val = val.Replace("%", "");

                    switch (this.HisDBType)
                    {
                        case DBType.Oracle:
                            this.SQL = "( " + attr2Field(attr) + " " + exp + " '%'||" + this.HisVarStr + "FK_Dept||'%' )";
                            this.MyParas.Add("FK_Dept", val);
                            break;
                        default:
                            //this.SQL = "( " + attr2Field(attr) + " " + exp + "  '" + this.HisVarStr + "FK_Dept%' )";
                            this.SQL = "( " + attr2Field(attr) + " " + exp + "  '" + val + "%' )";
                            //this.MyParas.Add("FK_Dept", val);
                            break;
                    }
                }
                else
                {
                    if (val.Contains(":") || val.Contains("@"))
                        this.SQL = "( " + attr2Field(attr) + " " + exp + "  " + val + " )";
                    else
                    {
                        if (val.Contains("'") == false)
                            this.SQL = "( " + attr2Field(attr) + " " + exp + "  '" + val + "' )";
                        else
                            this.SQL = "( " + attr2Field(attr) + " " + exp + "  " + val + " )";
                    }
                }
                return;
            }
            if (this.HisVarStr == "?")
            {
                this.SQL = "( " + attr2Field(attr) + " " + exp + "?)";
                this.MyParas.Add(attr, val);
            }
            else
            {
                if (paraName == null)
                {
                    this.SQL = "( " + attr2Field(attr) + " " + exp + this.HisVarStr + attr + ")";
                    this.MyParas.Add(attr, val);
                }
                else
                {
                    this.SQL = "( " + attr2Field(attr) + " " + exp + this.HisVarStr + paraName + ")";
                    this.MyParas.Add(paraName, val);
                }
            }
        }
        public void AddWhereDept(string val)
        {
            string attr = "FK_Dept";
            string exp = "=";

            if (val.Contains("'") == false)
                this.SQL = "( " + attr2Field(attr) + " " + exp + "  '" + val + "' )";
            else
                this.SQL = "( " + attr2Field(attr) + " " + exp + "  " + val + " )";
        }

        public void AddWhereField(string attr, string exp, string val)
        {
            if (val.ToString() == "all")
            {
                this.SQL = "( 1=1 )";
                return;
            }

            if (exp.ToLower().Contains(" in"))
            {
                this.SQL = "( " + attr + " " + exp + "  " + val + " )";
                return;
            }

            this.SQL = "( " + attr + " " + exp + " :" + attr + " )";
            this.MyParas.Add(attr, val);
        }
        public void AddWhereField(string attr, string exp, int val)
        {
            if (val.ToString() == "all")
            {
                this.SQL = "( 1=1 )";
                return;
            }

            if (exp.ToLower().Contains(" in"))
            {
                this.SQL = "( " + attr + " " + exp + "  " + val + " )";
                return;
            }

            if (attr == "RowNum")
            {
                this.SQL = "( " + attr + " " + exp + "  " + val + " )";
                return;
            }

            if (this.HisVarStr == "?")
                this.SQL = "( " + attr + " " + exp + "?)";
            else
                this.SQL = "( " + attr + " " + exp + "  " + this.HisVarStr + attr + " )";

            this.MyParas.Add(attr, val);
        }
        /// <summary>
        ///  Increased Conditions 
        /// </summary>
        /// <param name="attr"> Property </param>
        /// <param name="exp"> Operation Symbol （ Depending on the database ）</param>
        /// <param name="val">值</param>
        public void AddWhere(string attr, string exp, int val)
        {
            if (attr == "RowNum")
            {
                this.SQL = "( " + attr2Field(attr) + " " + exp + " " + val + ")";
            }
            else
            {
                if (this.HisVarStr == "?")
                    this.SQL = "( " + attr2Field(attr) + " " + exp + "?)";
                else
                    this.SQL = "( " + attr2Field(attr) + " " + exp + this.HisVarStr + attr + ")";

                this.MyParas.Add(attr, val);
            }
        }
        public void AddHD()
        {
            this.SQL = "(  1=1 ) ";
        }
        /// <summary>
        ///  Non-identical .
        /// </summary>
        public void AddHD_Not()
        {
            this.SQL = "(  1=2 ) ";
        }
        /// <summary>
        ///  Increased Conditions 
        /// </summary>
        /// <param name="attr"> Property </param>
        /// <param name="exp"> Operation Symbol （ Depending on the database ）</param>
        /// <param name="val">值</param>
        public void AddWhere(string attr, string exp, float val)
        {
            this.MyParas.Add(attr, val);
            if (this.HisVarStr == "?")
                this.SQL = "( " + attr2Field(attr) + " " + exp + "?)";
            else
                this.SQL = "( " + attr2Field(attr) + " " + exp + " " + this.HisVarStr + attr + ")";
        }
        /// <summary>
        ///  Increased Conditions ( The default is = )
        /// </summary>
        /// <param name="attr"> Property </param>
        /// <param name="val">值</param>
        public void AddWhere(string attr, string val)
        {
            this.AddWhere(attr, "=", val);
        }
        public void AddWhere(string attr, int val)
        {
            this.AddWhere(attr, "=", val);
        }
        /// <summary>
        ///  Increased Conditions 
        /// </summary>
        /// <param name="attr"> Property </param>
        /// <param name="val">值 true/false</param>
        public void AddWhere(string attr, bool val)
        {
            if (val)
                this.AddWhere(attr, "=", 1);
            else
                this.AddWhere(attr, "=", 0);
        }
        public void AddWhere(string attr, Int64 val)
        {
            this.AddWhere(attr, val.ToString());
        }
        public void AddWhere(string attr, float val)
        {
            this.AddWhere(attr, "=", val);
        }
        public void AddWhere(string attr, object val)
        {
            if (val == null)
                throw new Exception("Attr=" + attr + ", val is null");

            if (val.GetType() == typeof(int) || val.GetType() == typeof(Int32))
            {
                //int i = int.Parse(val.ToString()) ;
                this.AddWhere(attr, "=", (Int32)val);
                return;
            }
            this.AddWhere(attr, "=", val.ToString());
        }

        public void addLeftBracket()
        {
            this.SQL = " ( ";
        }

        public void addRightBracket()
        {
            this.SQL = " ) ";
            this.IsEndAndOR = true;
        }

        public void addAnd()
        {
            this.SQL = " AND ";
        }

        public void addOr()
        {
            this.SQL = " OR ";
        }

        #region  With respect to endsql
        public void addGroupBy(string attr)
        {
            this._groupBy = " GROUP BY  " + attr2Field(attr);
        }

        public void addGroupBy(string attr1, string attr2)
        {
            this._groupBy = " GROUP BY  " + attr2Field(attr1) + " , " + attr2Field(attr2);
        }

        private string _orderBy = "";
        public void addOrderBy(string attr)
        {
            if (this._orderBy.IndexOf("ORDER BY") != -1)
            {
                this._orderBy = " , " + attr2Field(attr);
            }
            else
            {
                this._orderBy = " ORDER BY " + attr2Field(attr);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attr"></param>
        public void addOrderByRandom()
        {
            if (this._orderBy.IndexOf("ORDER BY") != -1)
            {
                this._orderBy = " , NEWID()";
            }
            else
            {
                this._orderBy = " ORDER BY NEWID()";
            }
        }
        /// <summary>
        /// addOrderByDesc
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="desc"></param>
        public void addOrderByDesc(string attr)
        {
            this._orderBy = " ORDER BY " + attr2Field(attr) + " DESC ";
        }
        public void addOrderByDesc(string attr1, string attr2)
        {
            this._orderBy = " ORDER BY  " + attr2Field(attr1) + " DESC ," + attr2Field(attr2) + " DESC";
        }
        public void addOrderBy(string attr1, string attr2)
        {
            this._orderBy = " ORDER BY  " + attr2Field(attr1) + "," + attr2Field(attr2);
        }
        #endregion

        public void addHaving() { }
        ///  Clear query conditions 
        public void clear()
        {
            this._sql = "";
            this._groupBy = "";
            //this._orderBy = "";
            this.MyParas.Clear();
        }
        private Map _HisMap;
        public Map HisMap
        {
            get
            {
                if (_HisMap == null)
                    _HisMap = this.En.EnMap;
                return _HisMap;
            }
            set
            {
                _HisMap = value;
            }
        }
        private string attr2Field(string attr)
        {
            return this.HisMap.PhysicsTable + "." + this.HisMap.GetFieldByKey(attr);
        }
        public DataTable DoGroupReturnTable(Entity en, Attrs attrsOfGroupKey, Attr attrGroup, GroupWay gw, OrderWay ow)
        {
            switch (en.EnMap.EnDBUrl.DBType)
            {
                case DBType.Oracle:
                    return DoGroupReturnTableOracle(en, attrsOfGroupKey, attrGroup, gw, ow);
                default:
                    return DoGroupReturnTableSqlServer(en, attrsOfGroupKey, attrGroup, gw, ow);
            }
        }
        public DataTable DoGroupReturnTableOracle(Entity en, Attrs attrsOfGroupKey, Attr attrGroup, GroupWay gw, OrderWay ow)
        {
            #region   To generate a query statement 
            string fields = "";
            string str = "";
            foreach (Attr attr in attrsOfGroupKey)
            {
                if (attr.Field == null)
                    continue;

                str = "," + attr.Field;
                fields += str;
            }

            if (attrGroup.Key == "MyNum")
            {
                switch (gw)
                {
                    case GroupWay.BySum:
                        fields += ", COUNT(*) AS MyNum";
                        break;
                    case GroupWay.ByAvg:
                        fields += ", AVG(" + attrGroup.Field + ") AS MyNum";
                        break;
                    default:
                        throw new Exception("no such case:");
                }
            }
            else
            {
                switch (gw)
                {
                    case GroupWay.BySum:
                        fields += ",SUM(" + attrGroup.Field + ") AS " + attrGroup.Key;
                        break;
                    case GroupWay.ByAvg:
                        fields += ",AVG(" + attrGroup.Field + ") AS " + attrGroup.Key;
                        break;
                    default:
                        throw new Exception("no such case:");
                }
            }

            string by = "";
            foreach (Attr attr in attrsOfGroupKey)
            {
                if (attr.Field == null)
                    continue;

                str = "," + attr.Field;
                by += str;
            }
            by = by.Substring(1);
            //string sql 
            string sql = "SELECT " + fields.Substring(1) + " FROM " + this.En.EnMap.PhysicsTable + " WHERE " + this._sql + " Group BY " + by;
            #endregion

            #region
            Map map = new Map();
            map.PhysicsTable = "@VT@";
            map.Attrs = attrsOfGroupKey;
            map.Attrs.Add(attrGroup);
            #endregion .

            string sql1 = SqlBuilder.SelectSQLOfOra(en.ToString(), map) + " " + SqlBuilder.GenerFormWhereOfOra(en, map);

            sql1 = sql1.Replace("@TopNum", "");
            sql1 = sql1.Replace("FROM @VT@", "FROM (" + sql + ") VT");
            sql1 = sql1.Replace("@VT@", "VT");
            sql1 = sql1.Replace("TOP", "");

            if (ow == OrderWay.OrderByUp)
                sql1 += " ORDER BY " + attrGroup.Key + " DESC ";
            else
                sql1 += " ORDER BY " + attrGroup.Key;

            return DBAccess.RunSQLReturnTable(sql1, this.MyParas);
        }

        public DataTable DoGroupReturnTableSqlServer(Entity en, Attrs attrsOfGroupKey, Attr attrGroup, GroupWay gw, OrderWay ow)
        {

            #region   To generate a query statement 
            string fields = "";
            string str = "";
            foreach (Attr attr in attrsOfGroupKey)
            {
                if (attr.Field == null)
                    continue;
                str = "," + attr.Field;
                fields += str;
            }

            if (attrGroup.Key == "MyNum")
            {
                switch (gw)
                {
                    case GroupWay.BySum:
                        fields += ", COUNT(*) AS MyNum";
                        break;
                    case GroupWay.ByAvg:
                        fields += ", AVG(*)   AS MyNum";
                        break;
                    default:
                        throw new Exception("no such case:");
                }
            }
            else
            {
                switch (gw)
                {
                    case GroupWay.BySum:
                        fields += ",SUM(" + attrGroup.Field + ") AS " + attrGroup.Key;
                        break;
                    case GroupWay.ByAvg:
                        fields += ",AVG(" + attrGroup.Field + ") AS " + attrGroup.Key;
                        break;
                    default:
                        throw new Exception("no such case:");
                }
            }

            string by = "";
            foreach (Attr attr in attrsOfGroupKey)
            {
                if (attr.Field == null)
                    continue;

                str = "," + attr.Field;
                by += str;
            }
            by = by.Substring(1);
            //string sql 
            string sql = "SELECT " + fields.Substring(1) + " FROM " + this.En.EnMap.PhysicsTable + " WHERE " + this._sql + " Group BY " + by;
            #endregion

            #region
            Map map = new Map();
            map.PhysicsTable = "@VT@";
            map.Attrs = attrsOfGroupKey;
            map.Attrs.Add(attrGroup);
            #endregion .
            //string sql1=SqlBuilder.SelectSQLOfMS( map )+" "+SqlBuilder.GenerFormWhereOfMS( en,map) + "   AND ( " + this._sql+" ) "+_endSql;

            string sql1 = SqlBuilder.SelectSQLOfMS(map) + " " + SqlBuilder.GenerFormWhereOfMS(en, map);

            sql1 = sql1.Replace("@TopNum", "");

            sql1 = sql1.Replace("FROM @VT@", "FROM (" + sql + ") VT");

            sql1 = sql1.Replace("@VT@", "VT");
            sql1 = sql1.Replace("TOP", "");
            if (ow == OrderWay.OrderByUp)
                sql1 += " ORDER BY " + attrGroup.Key + " DESC ";
            else
                sql1 += " ORDER BY " + attrGroup.Key;
            return DBAccess.RunSQLReturnTable(sql1, this.MyParas);
        }
        /// <summary>
        ///  Grouping queries , Return datatable.
        /// </summary>
        /// <param name="attrsOfGroupKey"></param>
        /// <param name="groupValField"></param>
        /// <param name="gw"></param>
        /// <returns></returns>
        public DataTable DoGroupReturnTable1(Entity en, Attrs attrsOfGroupKey, Attr attrGroup, GroupWay gw, OrderWay ow)
        {
            #region   To generate a query statement 
            string fields = "";
            string str = "";
            foreach (Attr attr in attrsOfGroupKey)
            {
                if (attr.Field == null)
                    continue;
                str = "," + attr.Field;
                fields += str;
            }

            if (attrGroup.Key == "MyNum")
            {
                switch (gw)
                {
                    case GroupWay.BySum:
                        fields += ", COUNT(*) AS MyNum";
                        break;
                    case GroupWay.ByAvg:
                        fields += ", AVG(*)   AS MyNum";
                        break;
                    default:
                        throw new Exception("no such case:");
                }
            }
            else
            {
                switch (gw)
                {
                    case GroupWay.BySum:
                        fields += ",SUM(" + attrGroup.Field + ") AS " + attrGroup.Key;
                        break;
                    case GroupWay.ByAvg:
                        fields += ",AVG(" + attrGroup.Field + ") AS " + attrGroup.Key;
                        break;
                    default:
                        throw new Exception("no such case:");
                }
            }

            string by = "";
            foreach (Attr attr in attrsOfGroupKey)
            {
                if (attr.Field == null)
                    continue;

                str = "," + attr.Field;
                by += str;
            }
            by = by.Substring(1);
            //string sql 
            string sql = "SELECT " + fields.Substring(1) + " FROM " + this.En.EnMap.PhysicsTable + " WHERE " + this._sql + " Group BY " + by;
            #endregion

            #region
            Map map = new Map();
            map.PhysicsTable = "@VT@";
            map.Attrs = attrsOfGroupKey;
            map.Attrs.Add(attrGroup);
            #endregion .

            //string sql1=SqlBuilder.SelectSQLOfMS( map )+" "+SqlBuilder.GenerFormWhereOfMS( en,map) + "   AND ( " + this._sql+" ) "+_endSql;

            string sql1 = SqlBuilder.SelectSQLOfMS(map) + " " + SqlBuilder.GenerFormWhereOfMS(en, map);

            sql1 = sql1.Replace("@TopNum", "");
            sql1 = sql1.Replace("FROM @VT@", "FROM (" + sql + ") VT");
            sql1 = sql1.Replace("@VT@", "VT");
            sql1 = sql1.Replace("TOP", "");
            if (ow == OrderWay.OrderByUp)
                sql1 += " ORDER BY " + attrGroup.Key + " DESC ";
            else
                sql1 += " ORDER BY " + attrGroup.Key;
            return DBAccess.RunSQLReturnTable(sql1);
        }
        /// <summary>
        ///  On whether to perform a tail  AddAnd() Method .
        /// </summary>
        public bool IsEndAndOR = false;
        public string[] FullAttrs = null;
        /// <summary>
        ///  Execute the query 
        /// </summary>
        /// <returns></returns>
        public int DoQuery()
        {
            try
            {
                if (this._en == null)
                    return this.doEntitiesQuery();
                else
                    return this.doEntityQuery();
            }
            catch (Exception ex)
            {
                if (this._en == null)
                    this._ens.GetNewEntity.CheckPhysicsTable();
                else
                    this._en.CheckPhysicsTable();
                throw ex;
            }
        }
        public int DoQueryBak20111203()
        {
            try
            {
                if (this._en == null)
                {
                    return this.doEntitiesQuery();
                }
                else
                    return this.doEntityQuery();
            }
            catch (Exception ex)
            {
                try
                {
                    if (this._en == null)
                        this.Ens.GetNewEntity.CheckPhysicsTable();
                    else
                        this._en.CheckPhysicsTable();
                }
                catch
                {
                }
                throw ex;
            }
        }
        public string DealString(string sql)
        {
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            string strs = "";
            foreach (DataRow dr in dt.Rows)
            {
                strs += ",'" + dr[0].ToString() + "'";
            }
            return strs.Substring(1);
        }
        public string GenerPKsByTableWithPara(string pk, string sql, int from, int to)
        {
            //Log.DefaultLogWriteLineWarning(" ***************************** From= " + from + "  T0" + to);
            DataTable dt = DBAccess.RunSQLReturnTable(sql, this.MyParas);
            string pks = "";
            int i = 0;
            int paraI = 0;

            string dbStr = SystemConfig.AppCenterDBVarStr;
            foreach (DataRow dr in dt.Rows)
            {
                i++;
                if (i > from)
                {
                    paraI++;
                    //pks += "'" + dr[0].ToString() + "'";
                    if (dbStr == "?")
                        pks += "?,";
                    else
                        pks += SystemConfig.AppCenterDBVarStr + "R" + paraI + ",";

                    this.MyParasR.Add("R" + paraI, dr[0].ToString());
                    if (i >= to)
                        return pks.Substring(0, pks.Length - 1);
                }
            }
            if (pks == "")
            {
                return null;
                //return " '1'  ";
                return "  ";
            }
            return pks.Substring(0, pks.Length - 1);
        }
        public string GenerPKsByTable(string sql, int from, int to)
        {
            //Log.DefaultLogWriteLineWarning(" ***************************** From= " + from + "  T0" + to);
            DataTable dt = DBAccess.RunSQLReturnTable(sql, this.MyParas);
            string pks = "";
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                i++;
                if (i > from)
                {
                    if (i >= to)
                    {
                        pks += "'" + dr[0].ToString() + "'";
                        return pks;
                    }
                    else
                        pks += "'" + dr[0].ToString() + "',";
                }
            }
            if (pks == "")
                return "  '11111111' ";
            return pks.Substring(0, pks.Length - 1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIdx"></param>
        /// <returns></returns>
        public int DoQuery(string pk, int pageSize, int pageIdx)
        {
            if (pk == "OID" || pk=="WorkID" )
                return DoQuery(pk, pageSize, pageIdx, pk, true);
            else
                return DoQuery(pk, pageSize, pageIdx, pk, false);
        }
        /// <summary>
        ///  Paging query methods 
        /// </summary>
        /// <param name="pk"> Primary key </param>
        /// <param name="pageSize"> Page size </param>
        /// <param name="pageIdx">第x页</param>
        /// <param name="orderby"> Sequence </param>
        /// <param name="orderway"> Sort by :  In both cases  Down UP </param>
        /// <returns> Query Results </returns>
        public int DoQuery(string pk, int pageSize, int pageIdx, string orderBy, string orderWay)
        {
            if (orderWay.ToLower().Trim() == "up")
                return DoQuery(pk, pageSize, pageIdx, orderBy, false);
            else
                return DoQuery(pk, pageSize, pageIdx, orderBy, true);
        }
        /// <summary>
        ///  Paging query methods 
        /// </summary>
        /// <param name="pk"> Primary key </param>
        /// <param name="pageSize"> Page size </param>
        /// <param name="pageIdx">第x页</param>
        /// <param name="orderby"> Sequence </param>
        /// <returns> Query Results </returns>
        public int DoQuery(string pk, int pageSize, int pageIdx, bool isDesc)
        {
            return DoQuery(pk, pageSize, pageIdx, pk, isDesc);
        }
        /// <summary>
        ///  Paging query methods 
        /// </summary>
        /// <param name="pk"> Primary key </param>
        /// <param name="pageSize"> Page size </param>
        /// <param name="pageIdx">第x页</param>
        /// <param name="orderby"> Sequence </param>
        /// <param name="orderway"> Sort by :  In both cases  desc  Or  为 null. </param>
        /// <returns> Query Results </returns>
        public int DoQuery(string pk, int pageSize, int pageIdx, string orderBy, bool isDesc)
        {
            int pageNum = 0;
            
            // If you do not join the sort field , Using the primary key 
            if (string.IsNullOrEmpty(this._orderBy))
            {
                string isDescStr = "";
                if (isDesc)
                    isDescStr = " DESC ";

                if (string.IsNullOrEmpty(orderBy)) 
                    orderBy = pk;

                this._orderBy =  attr2Field(orderBy) + isDescStr;
            }

            if (this._orderBy.Contains("ORDER BY") == false)
                _orderBy = " ORDER BY " + this._orderBy;

            try
            {
                if (this._en == null)
                {
                    int recordConut = 0;
                    recordConut = this.GetCount(); //  Get   Its number .

                    if (recordConut == 0)
                    {
                        this._ens.Clear();
                        return 0;
                    }

                    // xx!5555  Errors made .
                    if (pageSize == 0)
                        pageSize = 12;

                    decimal pageCountD = decimal.Parse(recordConut.ToString()) / decimal.Parse(pageSize.ToString()); //  Number of pages .
                    string[] strs = pageCountD.ToString("0.0000").Split('.');
                    if (int.Parse(strs[1]) > 0)
                        pageNum = int.Parse(strs[0]) + 1;
                    else
                        pageNum = int.Parse(strs[0]);

                    int myleftCount = recordConut - (pageNum * pageSize);

                    pageNum++;
                    int top = pageSize * (pageIdx - 1);

                    string sql = "";
                    Entity en = this._ens.GetNewEntity;
                    Map map = en.EnMap;
                    int toIdx = 0;
                    string pks = "";
                    switch (map.EnDBUrl.DBType)
                    {
                        case DBType.Oracle:
                            toIdx = top + pageSize;
                            if (this._sql == "" || this._sql == null)
                            {
                                if (top == 0)
                                    sql = "SELECT * FROM ( SELECT  " + pk + " FROM " + map.PhysicsTable + " " + this._orderBy + "   ) WHERE ROWNUM <=" + pageSize;
                                else
                                    sql = "SELECT * FROM ( SELECT  " + pk + " FROM " + map.PhysicsTable + " " + this._orderBy + ") ";
                            }
                            else
                            {
                                if (top == 0)
                                    sql = "SELECT * FROM ( SELECT  " + pk + " FROM " + map.PhysicsTable + " WHERE " + this._sql + " " + this._orderBy + "   )  WHERE ROWNUM <=" + pageSize;
                                else
                                    sql = "SELECT * FROM ( SELECT  " + pk + " FROM " + map.PhysicsTable + " WHERE " + this._sql + " " + this._orderBy + "   ) ";
                            }

                            sql = sql.Replace("AND ( ( 1=1 ) )", " ");

                            pks = this.GenerPKsByTableWithPara(pk, sql, top, toIdx);
                            this.clear();
                            this.MyParas = this.MyParasR;
                            if (pks != null)
                                this.AddWhereIn(pk, "(" + pks + ")");
                            else
                                this.AddHD();

                            this.Top = pageSize;
                            return this.doEntitiesQuery();
                        case DBType.Informix:
                            toIdx = top + pageSize;
                            if (this._sql == "" || this._sql == null)
                            {
                                if (top == 0)
                                    sql = " SELECT first  " + pageSize + "  " + this.En.PKField + " FROM " + map.PhysicsTable + " " + this._orderBy;
                                else
                                    sql = " SELECT  " + this.En.PKField + " FROM " + map.PhysicsTable + " " + this._orderBy;
                            }
                            else
                            {
                                if (top == 0)
                                    sql = "SELECT first " + pageSize + " " + this.En.PKField + " FROM " + map.PhysicsTable + " WHERE " + this._sql + " " + this._orderBy;
                                else
                                    sql = "SELECT  " + this.En.PKField + " FROM " + map.PhysicsTable + " WHERE " + this._sql + " " + this._orderBy;
                            }

                            sql = sql.Replace("AND ( ( 1=1 ) )", " ");

                            pks = this.GenerPKsByTableWithPara(pk, sql, top, toIdx);
                            this.clear();
                            this.MyParas = this.MyParasR;

                            if (pks == null)
                                this.AddHD_Not();
                            else
                                this.AddWhereIn(pk, "(" + pks + ")");

                            this.Top = pageSize;
                            return this.doEntitiesQuery();
                        case DBType.MySQL:
                            toIdx = top + pageSize;
                            if (this._sql == "" || this._sql == null)
                            {
                                if (top == 0)
                                    sql = " SELECT  " + this.En.PKField + " FROM " + map.PhysicsTable + " " + this._orderBy + " LIMIT "+pageSize;
                                else
                                    sql = " SELECT  " + this.En.PKField + " FROM " + map.PhysicsTable + " " + this._orderBy;
                            }
                            else
                            {
                                if (top == 0)
                                    sql = "SELECT  " + this.En.PKField + " FROM " + map.PhysicsTable + " WHERE " + this._sql + " " + this._orderBy + " LIMIT " + pageSize;
                                else
                                    sql = "SELECT  " + this.En.PKField + " FROM " + map.PhysicsTable + " WHERE " + this._sql + " " + this._orderBy;
                            }

                            sql = sql.Replace("AND ( ( 1=1 ) )", " ");

                            pks = this.GenerPKsByTableWithPara(pk, sql, top, toIdx);
                            this.clear();
                            this.MyParas = this.MyParasR;

                            if (pks == null)
                                this.AddHD_Not();
                            else
                                this.AddWhereIn(pk, "(" + pks + ")");

                            this.Top = pageSize;
                            return this.doEntitiesQuery();
                        case DBType.MSSQL:
                        default:
                            toIdx = top + pageSize;

                            
                            if (this._sql == "" || this._sql == null)
                            {
                                if (top == 0)
                                    sql = " SELECT top " + pageSize + "  [" + this.En.PKField + "] FROM " + map.PhysicsTable + " " + this._orderBy;
                                else
                                    sql = " SELECT  [" + this.En.PKField + "] FROM " + map.PhysicsTable + " " + this._orderBy;
                            }
                            else
                            {
                                if (top == 0)
                                    sql = "SELECT TOP " + pageSize + " [" + this.En.PKField + "] FROM " + map.PhysicsTable + " WHERE " + this._sql + " " + this._orderBy;
                                else
                                    sql = "SELECT  [" + this.En.PKField + "] FROM " + map.PhysicsTable + " WHERE " + this._sql + " " + this._orderBy;
                            }

                            sql = sql.Replace("AND ( ( 1=1 ) )", " ");

                            pks = this.GenerPKsByTableWithPara(pk, sql, top, toIdx);
                            this.clear();
                            this.MyParas = this.MyParasR;

                            if (pks == null)
                                this.AddHD_Not();
                            else
                                this.AddWhereIn(pk, "(" + pks + ")");

                            this.Top = pageSize;
                            return this.doEntitiesQuery();
                    }
                }
                else
                    return this.doEntityQuery();
            }
            catch (Exception ex)
            {
                try
                {
                    if (this._en == null)
                        this.Ens.GetNewEntity.CheckPhysicsTable();
                    else
                        this._en.CheckPhysicsTable();
                }
                catch
                {
                }
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable DoQueryToTable()
        {
            try
            {
                string sql = this.SQL;
                sql = sql.Replace("WHERE (1=1) AND ( AND ( ( ( 1=1 ) ) AND ( ( 1=1 ) ) ) )", "");

                return DBAccess.RunSQLReturnTable(sql, this.MyParas);
            }
            catch (Exception ex)
            {
                if (this._en == null)
                    this.Ens.GetNewEntity.CheckPhysicsTable();
                else
                    this._en.CheckPhysicsTable();
                throw ex;
            }
        }
        /// <summary>
        ///  Get the number of returns 
        /// </summary>
        /// <returns> Get the number of returns </returns>
        public int GetCount()
        {
            string sql = this.SQL;
            //sql="SELECT COUNT(*) "+sql.Substring(sql.IndexOf("FROM") ) ;
            string ptable = this.En.EnMap.PhysicsTable;
            string pk = this.En.PKField;

            switch (this.En.EnMap.EnDBUrl.DBType)
            {
                case DBType.Oracle:
                    if (this._sql == "" || this._sql == null)
                        sql = "SELECT COUNT(" + ptable + "." + pk + ") as C FROM " + ptable;
                    else
                        sql = "SELECT COUNT(" + ptable + "." + pk + ") as C " + sql.Substring(sql.IndexOf("FROM "));
                    break;
                default:
                    if (this._sql == "" || this._sql == null)
                        sql = "SELECT COUNT(" + ptable + "." + pk + ") as C FROM " + ptable;
                    else
                        sql = "SELECT COUNT(" + ptable + "." + pk + ") as C FROM " + ptable + " WHERE " + this._sql;

                    //sql="SELECT COUNT(*) as C "+this._endSql  +sql.Substring(  sql.IndexOf("FROM ") ) ;
                    //sql="SELECT COUNT(*) as C FROM "+ this._ens.GetNewEntity.EnMap.PhysicsTable+ "  " +sql.Substring(sql.IndexOf("WHERE") ) ;
                    //int i = sql.IndexOf("ORDER BY") ;
                    //if (i!=-1)
                    //	sql=sql.Substring(0,i);
                    break;
            }
            try
            {
                int i = DBAccess.RunSQLReturnValInt(sql, this.MyParas);
                if (this.Top == -1)
                    return i;

                if (this.Top >= i)
                    return i;
                else
                    return this.Top;
            }
            catch (Exception ex)
            {
                if (SystemConfig.IsDebug)
                    this.En.CheckPhysicsTable();
                throw ex;
            }
        }
        /// <summary>
        ///  The maximum number of 
        /// </summary>
        /// <param name="topNum"> The maximum number of </param>
        /// <returns> To query the information </returns>
        public DataTable DoQueryToTable(int topNum)
        {
            this.Top = topNum;
            return DBAccess.RunSQLReturnTable(this.SQL, this.MyParas);
        }

        private int doEntityQuery()
        {
            return EntityDBAccess.Retrieve(this.En, this.SQL, this.MyParas);
        }
        private int doEntitiesQuery()
        {
            switch (this._ens.GetNewEntity.EnMap.EnDBUrl.DBType)
            {
                case DBType.Oracle:
                    if (this.IsEndAndOR == false)
                    {
                        if (this.Top == -1)
                            this.AddHD();
                        else
                            this.AddWhereField("RowNum", "<=", this.Top);
                    }
                    else
                    {
                        if (this.Top == -1)
                        {
                        }
                        else
                        {
                            this.addAnd();
                            this.AddWhereField("RowNum", "<=", this.Top);
                        }
                    }
                    break;
                case DBType.MSSQL:
                case DBType.MySQL:
                default:
                    break;
            }
            return EntityDBAccess.Retrieve(this.Ens, this.SQL, this.MyParas, this.FullAttrs);
        }
        /// <summary>
        ///  According to data Initialization entiies.
        /// </summary>
        /// <param name="ens"> Entity s</param>
        /// <param name="dt"> Data sheet </param>
        /// <param name="fullAttrs"> To populate the tree </param>
        /// <returns> After initialization ens</returns>
        public static Entities InitEntitiesByDataTable(Entities ens, DataTable dt, string[] fullAttrs)
        {
            if (fullAttrs == null)
            {
                Map enMap = ens.GetNewEntity.EnMap;
                Attrs attrs = enMap.Attrs;
                try
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Entity en = ens.GetNewEntity;
                        foreach (Attr attr in attrs)
                        {
                            //if (attr.IsRefAttr)
                            //    continue;
                            if (dt.Columns.Contains(attr.Key) == false)
                                continue;
                            en.Row.SetValByKey(attr.Key, dr[attr.Key]);
                        }
                        ens.AddEntity(en);
                    }
                }
                catch (Exception ex)
                {
#warning  Error should not happen . 2011-12-03 add
                    string cols = "";
                    foreach (DataColumn dc in dt.Columns)
                    {
                        cols += " , " + dc.ColumnName;
                    }
                    throw new Exception("Columns=" + cols + "@Ens=" + ens.ToString() + " @ Exception Information :" + ex.Message);
                }
            }
            else
            {

                foreach (DataRow dr in dt.Rows)
                {
                    Entity en = ens.GetNewEntity;
                    foreach (string str in fullAttrs)
                        en.Row.SetValByKey(str, dr[str]);
                    ens.AddEntity(en);
                }
            }
            return ens;
        }
    }
}
