using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP;
namespace BP.Sys
{
	/// <summary>
	///  User registry 
	/// </summary>
    public class UserRegeditAttr
    {
        /// <summary>
        ///  Whether to display the picture 
        /// </summary>
        public const string IsPic = "IsPic";
        /// <summary>
        ///  Name 
        /// </summary>
        public const string FK_Emp = "FK_Emp";
        /// <summary>
        ///  Entity Name 
        /// </summary>
        public const string CfgKey = "CfgKey";
        /// <summary>
        ///  Property 
        /// </summary> 
        public const string Vals = "Vals";
        /// <summary>
        ///  Inquiry 
        /// </summary>
        public const string SearchKey = "SearchKey";
        /// <summary>
        /// MyPK
        /// </summary>
        public const string MyPK = "MyPK";
        /// <summary>
        /// OrderBy
        /// </summary>
        public const string OrderBy = "OrderBy";
        /// <summary>
        /// OrderWay
        /// </summary>
        public const string OrderWay = "OrderWay";
        /// <summary>
        ///  Produced sql
        /// </summary>
        public const string GenerSQL = "GenerSQL";
        /// <summary>
        ///  Parameters 
        /// </summary>
        public const string Paras = "Paras";
        /// <summary>
        ///  Numerical 
        /// </summary>
        public const string NumKey = "NumKey";
        /// <summary>
        ///  Inquiry 
        /// </summary>
        public const string MVals = "MVals";
        /// <summary>
        ///  Query time from 
        /// </summary>
        public const string DTFrom = "DTFrom";
        /// <summary>
        ///  Query time to 
        /// </summary>
        public const string DTTo = "DTTo";
    }
	/// <summary>
	///  User registry 
	/// </summary>
	public class UserRegedit: EntityMyPK
	{
		#region  User registry keys list 
		#endregion

        /// <summary>
        ///  Use of an automatic MyPK,��FK_Emp + CfgKey
        /// </summary>
        public bool AutoMyPK { get; set; }

		#region  Basic properties 
        /// <summary>
        ///  Whether to display the picture 
        /// </summary>
        public bool IsPic
        {
            get
            {
                return this.GetValBooleanByKey(UserRegeditAttr.IsPic);
            }
            set
            {
                this.SetValByKey(UserRegeditAttr.IsPic, value);
            }
        }
        /// <summary>
        ///  Numeric keys 
        /// </summary>
        public string NumKey
        {
            get
            {
                return this.GetValStringByKey(UserRegeditAttr.NumKey);
            }
            set
            {
                this.SetValByKey(UserRegeditAttr.NumKey, value);
            }
        }
        /// <summary>
        ///  Parameters 
        /// </summary>
        public string Paras
        {
            get
            {
                return this.GetValStringByKey(UserRegeditAttr.Paras);
            }
            set
            {
                this.SetValByKey(UserRegeditAttr.Paras, value);
            }
        }
        /// <summary>
        ///  Produced sql
        /// </summary>
        public string GenerSQL
        {
            get
            {
                string GenerSQL = this.GetValStringByKey(UserRegeditAttr.GenerSQL);
                GenerSQL = GenerSQL.Replace("~", "'");
                return GenerSQL;
            }
            set
            {
                this.SetValByKey(UserRegeditAttr.GenerSQL, value);
            }
        }
        /// <summary>
        ///  Sort by 
        /// </summary>
        public string OrderWay
        {
            get
            {
                return this.GetValStringByKey(UserRegeditAttr.OrderWay);
            }
            set
            {
                this.SetValByKey(UserRegeditAttr.OrderWay, value);
            }
        }
        public string OrderBy
        {
            get
            {
                return this.GetValStringByKey(UserRegeditAttr.OrderBy);
            }
            set
            {
                this.SetValByKey(UserRegeditAttr.OrderBy, value);
            }
        }
		/// <summary>
		/// FK_Emp
		/// </summary>
		public string FK_Emp
		{
			get
			{
				return this.GetValStringByKey(UserRegeditAttr.FK_Emp) ; 
			}
			set
			{
				this.SetValByKey(UserRegeditAttr.FK_Emp,value) ; 
			}
		}
        /// <summary>
        ///  Query time from 
        /// </summary>
        public string DTFrom_Data
        {
            get
            {
                string s = this.GetValStringByKey(UserRegeditAttr.DTFrom);
                if (string.IsNullOrEmpty(s) || 1==1)
                {
                    DateTime dt = DateTime.Now.AddDays(-14);
                    return dt.ToString(DataType.SysDataFormat);
                }
                return s.Substring(0, 10);
            }
            set
            {
                this.SetValByKey(UserRegeditAttr.DTFrom, value);
            }
        }
        /// <summary>
        /// ��
        /// </summary>
        public string DTTo_Data
        {
            get
            {
                string s = this.GetValStringByKey(UserRegeditAttr.DTTo);
                if (string.IsNullOrEmpty(s) || 1 == 1 )
                {
                    DateTime dt = DateTime.Now;
                    return dt.ToString(DataType.SysDataFormat);
                }
                return s.Substring(0, 10);
            }
            set
            {
                this.SetValByKey(UserRegeditAttr.DTTo, value);
            }
        }

        public string DTFrom_Datatime
        {
            get
            {
                string s = this.GetValStringByKey(UserRegeditAttr.DTFrom);
                if (string.IsNullOrEmpty(s))
                {
                    DateTime dt = DateTime.Now.AddDays(-14);
                    return dt.ToString(DataType.SysDataTimeFormat);
                }
                return s ;
            }
            set
            {
                this.SetValByKey(UserRegeditAttr.DTFrom, value);
            }
        }
        /// <summary>
        /// ��
        /// </summary>
        public string DTTo_Datatime
        {
            get
            {
                string s = this.GetValStringByKey(UserRegeditAttr.DTTo);
                if (string.IsNullOrEmpty(s))
                {
                    DateTime dt = DateTime.Now;
                    return dt.ToString(DataType.SysDataTimeFormat);
                }
                return s ;
            }
            set
            {
                this.SetValByKey(UserRegeditAttr.DTTo, value);
            }
        }
		/// <summary>
		/// CfgKey
		/// </summary>
		public string CfgKey
		{
			get
			{
				return this.GetValStringByKey(UserRegeditAttr.CfgKey ) ; 
			}
			set
			{
				this.SetValByKey(UserRegeditAttr.CfgKey,value) ; 
			}
		}
        public string SearchKey
        {
            get
            {
                return this.GetValStringByKey(UserRegeditAttr.SearchKey);
            }
            set
            {
                this.SetValByKey(UserRegeditAttr.SearchKey, value);
            }
        }
		/// <summary>
		/// Vals
		/// </summary>
		public string Vals
		{
			get
			{
				return this.GetValStringByKey(UserRegeditAttr.Vals ) ; 
			}
			set
			{
				this.SetValByKey(UserRegeditAttr.Vals,value) ; 
			}
		}
        public string MVals
        {
            get
            {
                return this.GetValStringByKey(UserRegeditAttr.MVals);
            }
            set
            {
                this.SetValByKey(UserRegeditAttr.MVals, value);
            }
        }
        public string MyPK
        {
            get
            {
                return this.GetValStringByKey(UserRegeditAttr.MyPK);
            }
            set
            {
                this.SetValByKey(UserRegeditAttr.MyPK, value);
            }
        }
		#endregion

		#region  Constructor 
		/// <summary>
		///  User registry 
		/// </summary>
		public UserRegedit()
		{
            AutoMyPK = true;
		}
		/// <summary>
		///  User registry 
		/// </summary>
		/// <param name="fk_emp"> Staff </param>
		/// <param name="cfgkey"> Configuration </param>
		public UserRegedit(string fk_emp, string cfgkey)
            :this()
		{
            this.MyPK = fk_emp + cfgkey;
            this.CfgKey = cfgkey;
            this.FK_Emp = fk_emp;
            int i = this.RetrieveFromDBSources();
            if (i == 0)
            {
                this.CfgKey = cfgkey;
                this.FK_Emp = fk_emp;
                this.DirectInsert();
               // this.DirectInsert();
            }
		}
		/// <summary>
		/// EnMap
		/// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;
                Map map = new Map("Sys_UserRegedit");
                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;

                map.EnDesc = " User registry ";
                map.EnType = EnType.Sys;
                map.AddMyPK();
                map.AddTBString(UserRegeditAttr.FK_Emp, null, " User ", false, false, 1, 30, 20);
                map.AddTBString(UserRegeditAttr.CfgKey, null, "Key", true, false, 1, 200, 20);
                map.AddTBString(UserRegeditAttr.Vals, null, "Value", true, false, 0, 2000, 20);
                map.AddTBString(UserRegeditAttr.GenerSQL, null, "GenerSQL", true, false, 0, 2000, 20);
                map.AddTBString(UserRegeditAttr.Paras, null, "Paras", true, false, 0, 2000, 20);
                map.AddTBString(UserRegeditAttr.NumKey, null, " Analysis Key", true, false, 0, 300, 20);
                map.AddTBString(UserRegeditAttr.OrderBy, null, "OrderBy", true, false, 0, 300, 20);
                map.AddTBString(UserRegeditAttr.OrderWay, null, "OrderWay", true, false, 0, 300, 20);
                map.AddTBString(UserRegeditAttr.SearchKey, null, "SearchKey", true, false, 0, 300, 20);
                map.AddTBString(UserRegeditAttr.MVals, null, "MVals", true, false, 0, 300, 20);
                map.AddBoolean(UserRegeditAttr.IsPic, false, " Are Pictures ", true, false);

                map.AddTBString(UserRegeditAttr.DTFrom, null, " Query time from ", true, false, 0, 20, 20);
                map.AddTBString(UserRegeditAttr.DTTo, null, " to ", true, false, 0, 20, 20);
                
                this._enMap = map;
                return this._enMap;
            }
        }
		#endregion 

        #region  Rewrite 
        public override Entities GetNewEntities
        {
            get { return new UserRegedits(); }
        }
        protected override bool beforeUpdateInsertAction()
        {
            if (AutoMyPK)
                this.MyPK = this.FK_Emp + this.CfgKey;

            return base.beforeUpdateInsertAction();
        }
        #endregion  Rewrite 
    }
	/// <summary>
	///  User registry s
	/// </summary>
    public class UserRegedits : EntitiesMyPK
    {
        #region  Structure 
        public UserRegedits()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emp"></param>
        public UserRegedits(string emp)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(UserRegeditAttr.FK_Emp, emp);
            qo.DoQuery();
        }
        #endregion

        #region  Rewrite 
        /// <summary>
        ///  Get it  Entity
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new UserRegedit();
            }
        }
        #endregion

    }
}
