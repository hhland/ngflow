using System;
using System.Collections;
using BP.DA;
using BP.En;
namespace BP.Sys
{
	/// <summary>
	///  Property 
	/// </summary>
    public class SerialAttr
    {
        /// <summary>
        ///  Property Key
        /// </summary>
        public const string EnsName = "EnsName";
        /// <summary>
        ///  Staff 
        /// </summary>
        public const string CfgKey = "CfgKey";
        /// <summary>
        ///  Serial Number 
        /// </summary>
        public const string IntVal = "IntVal";
    }
	/// <summary>
	///  Serial Number 
	/// </summary>
	public class Serial: Entity
	{
		#region  Basic properties 
		/// <summary>
		///  Serial Number 
		/// </summary>
		public string IntVal
		{
			get
			{
				return this.GetValStringByKey(SerialAttr.IntVal ) ; 
			}
			set
			{
				this.SetValByKey(SerialAttr.IntVal,value) ; 
			}
		}
		/// <summary>
		///  The operator ID
		/// </summary>
		public string CfgKey
		{
			get
			{
				return this.GetValStringByKey(SerialAttr.CfgKey ) ; 
			}
			set
			{
				this.SetValByKey(SerialAttr.CfgKey,value) ; 
			}
		}
		#endregion

		#region  Constructor 

		/// <summary>
		///  Serial Number 
		/// </summary>
		public Serial()
		{
		}
		/// <summary>
		/// map
		/// </summary>
		public override Map EnMap
		{
			get
			{
				if (this._enMap!=null) return this._enMap;
				Map map = new Map("Sys_Serial");
				map.EnType=EnType.Sys;
				map.EnDesc=" Serial Number ";
				map.DepositaryOfEntity=Depositary.None;
				map.AddTBStringPK(SerialAttr.CfgKey,"OID","CfgKey",false,true,1,100,10);
				map.AddTBInt(SerialAttr.IntVal,0," Property ",true,false);
				this._enMap=map;
				return this._enMap;
			}
		}
		#endregion 

        public int Gener(string CfgKey)
        {
            Paras ps = new Paras();
            ps.Add("p", CfgKey);

            string sql = "SELECT IntVal Sys_Serial WHERE CfgKey="+SystemConfig.AppCenterDBVarStr+"p";
            int val = DBAccess.RunSQLReturnValInt(sql, 0,ps);
            if (val == 0)
            {
                sql = "INSERT INTO Sys_Serial VALUES(" + SystemConfig.AppCenterDBVarStr + "p,1)";
                DBAccess.RunSQLReturnVal(sql, ps);
                return 1;
            }
            else
            {
                val++;
                ps.Add("intV", val);
                sql = "UPDATE  Sys_Serial SET IntVal="+SystemConfig.AppCenterDBVarStr+"intV WHERE  CfgKey=" + SystemConfig.AppCenterDBVarStr + "p";
                DBAccess.RunSQLReturnVal(sql);
                return val;
            }
        }
	}
	/// <summary>
	///  Serial Number s
	/// </summary>
	public class Serials : Entities
	{
		/// <summary>
		///  Serial Number s
		/// </summary>
		public Serials()
		{
		}
		/// <summary>
		///  Get it  Entity
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new Serial();
			}
		}
	}
}
