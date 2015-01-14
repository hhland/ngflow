using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP;
namespace BP.Sys
{
	/// <summary>
	///  User Log 
	/// </summary>
    public class UserLogAttr
    {
        /// <summary>
        ///  Name 
        /// </summary>
        public const string FK_Emp = "FK_Emp";
        /// <summary>
        ///  Log mark 
        /// </summary>
        public const string LogFlag = "LogFlag";
        /// <summary>
        ///  Processing content 
        /// </summary>
        public const string Docs = "Docs";
        /// <summary>
        ///  Record Date 
        /// </summary>
        public const string RDT = "RDT";
        public const string IP = "IP";

    }
	/// <summary>
	///  User Log 
	/// </summary>
	public class UserLog: EntityMyPK
	{

		#region  User log key list 
		#endregion

		#region  Basic properties 
        public string IP
        {
            get
            {
                return this.GetValStringByKey(UserLogAttr.IP);
            }
            set
            {
                this.SetValByKey(UserLogAttr.IP, value);
            }
        }
        /// <summary>
        ///  Log mark key 
        /// </summary>
        public string LogFlag
        {
            get
            {
                return this.GetValStringByKey(UserLogAttr.LogFlag);
            }
            set
            {
                this.SetValByKey(UserLogAttr.LogFlag, value);
            }
        }
		/// <summary>
		/// FK_Emp
		/// </summary>
		public string FK_Emp
		{
			get
			{
				return this.GetValStringByKey(UserLogAttr.FK_Emp) ; 
			}
			set
			{
				this.SetValByKey(UserLogAttr.FK_Emp,value) ; 
			}
		}
        public string RDT
        {
            get
            {
                return this.GetValStringByKey(UserLogAttr.RDT);
            }
            set
            {
                this.SetValByKey(UserLogAttr.RDT, value);
            }
        }
      
        public string Docs
        {
            get
            {
                return this.GetValStringByKey(UserLogAttr.Docs);
            }
            set
            {
                this.SetValByKey(UserLogAttr.Docs, value);
            }
        }
      
		#endregion

		#region  Constructor 
		/// <summary>
		///  User Log 
		/// </summary>
		public UserLog()
		{
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
                Map map = new Map("Sys_UserLogT");
                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;

                map.EnDesc = " User Log ";
                map.EnType = EnType.Sys;
                map.AddMyPK();
                map.AddTBString(UserLogAttr.FK_Emp, null, " User ", false, false, 0, 30, 20);
                map.AddTBString(UserLogAttr.IP, null, "IP", true, false, 0, 200, 20);
                map.AddTBString(UserLogAttr.LogFlag, null, "Flag", true, false, 0, 300, 20);
                map.AddTBString(UserLogAttr.Docs, null, "Docs", true, false, 0, 300, 20);
                map.AddTBString(UserLogAttr.RDT, null, " Record Date ", true, false, 0, 20, 20);
                this._enMap = map;
                return this._enMap;
            }
        }
		#endregion 

        #region  Rewrite 
        public override Entities GetNewEntities
        {
            get { return new UserLogs(); }
        }
        #endregion  Rewrite 
    }
	/// <summary>
	///  User Log s
	/// </summary>
    public class UserLogs : EntitiesMyPK
    {
        #region  Structure 
        public UserLogs()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emp"></param>
        public UserLogs(string emp)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(UserLogAttr.FK_Emp, emp);
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
                return new UserLog();
            }
        }
        #endregion

    }
}
