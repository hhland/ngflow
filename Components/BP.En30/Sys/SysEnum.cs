using System;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.Sys
{
	/// <summary>
	/// sss
	/// </summary>
	public class SysEnumAttr 
	{
		/// <summary>
		///  Title   
		/// </summary>
		public const string Lab="Lab";
		/// <summary>
		/// Int key
		/// </summary>
		public const string IntKey="IntKey";
		/// <summary>
		/// EnumKey
		/// </summary>
		public const string EnumKey="EnumKey";
		/// <summary>
		/// Language
		/// </summary>
		public const string Lang="Lang";
        ///// <summary>
        /////  Style 
        ///// </summary>
        //public const string Style="Style";
	}
	/// <summary>
	/// SysEnum
	/// </summary>
	public class SysEnum : EntityMyPK
	{
		/// <summary>
		///  Get a String By LabKey.
		/// </summary>
		/// <param name="EnumKey"></param>
		/// <param name="intKey"></param>
		/// <returns></returns>
		public static string GetLabByPK(string EnumKey, int intKey)
		{
			SysEnum en = new SysEnum(EnumKey,intKey);
			return en.Lab;
		}

		#region  Achieve the basic square method 
		/// <summary>
		///  Label 
		/// </summary>
		public  string  Lab
		{
			get
			{
			  return this.GetValStringByKey(SysEnumAttr.Lab);
			}
			set
			{
				this.SetValByKey(SysEnumAttr.Lab,value);
			}
		}
		/// <summary>
		///  Label 
		/// </summary>
		public  string  Lang
		{
			get
			{
				return this.GetValStringByKey(SysEnumAttr.Lang);
			}
			set
			{
				this.SetValByKey(SysEnumAttr.Lang,value);
			}
		}
		/// <summary>
		/// Int val
		/// </summary>
		public  int  IntKey
		{
			get
			{
				return this.GetValIntByKey(SysEnumAttr.IntKey);
			}
			set
			{
				this.SetValByKey(SysEnumAttr.IntKey,value);
			}
		}
		/// <summary>
		/// EnumKey
		/// </summary>
		public  string  EnumKey
		{
			get
			{
				return this.GetValStringByKey(SysEnumAttr.EnumKey);
			}
			set
			{
				this.SetValByKey(SysEnumAttr.EnumKey,value);
			}
		}
        ///// <summary>
        /////  Style 
        ///// </summary>
        //public  string  Style
        //{
        //    get
        //    {
        //        return this.GetValStringByKey(SysEnumAttr.Style);
        //    }
        //    set
        //    {
        //        this.SetValByKey(SysEnumAttr.Style,value);
        //    }
        //}
		 
		#endregion 

		#region  Constructor 
		/// <summary>
		/// SysEnum
		/// </summary>
		public SysEnum(){}
		/// <summary>
		///  Tax Number 
		/// </summary>
		/// <param name="_No"> Serial number </param>
        public SysEnum(string EnumKey, int val)
        {
            this.EnumKey = EnumKey;
            this.Lang = BP.Web.WebUser.SysLang;
            this.IntKey = val;
            this.MyPK = this.EnumKey + "_" + this.Lang + "_" + this.IntKey;
            int i = this.RetrieveFromDBSources();
            if (i == 0)
            {
                i = this.Retrieve(SysEnumAttr.EnumKey, EnumKey, SysEnumAttr.Lang, BP.Web.WebUser.SysLang ,
                     SysEnumAttr.IntKey, this.IntKey);
                SysEnums ses = new SysEnums();
                ses.Full(EnumKey);
                if (i == 0)
                    throw new Exception("@ EnumKey=" + EnumKey+ " Val=" + val + " Lang="+Web.WebUser.SysLang+" ...Error");
            }
        }
        public SysEnum(string enumKey, string Lang, int val)
        {
            this.EnumKey = enumKey;
            this.Lang = Lang;
            this.IntKey = val;
            this.MyPK = this.EnumKey + "_" + this.Lang + "_" + this.IntKey;
            int i = this.RetrieveFromDBSources();
            if (i == 0)
            {
                i = this.Retrieve(SysEnumAttr.EnumKey, enumKey, SysEnumAttr.Lang, Lang,
                    SysEnumAttr.IntKey, this.IntKey);

                SysEnums ses = new SysEnums();
                ses.Full(enumKey);

                if (i == 0)
                    throw new Exception("@ EnumKey=" + enumKey + " Val=" + val + " Lang=" + Lang + " Error");
            }
        }
		/// <summary>
		/// Map
		/// </summary>
		public override Map EnMap
		{
            get
            {
                if (this._enMap != null) return this._enMap;
                Map map = new Map("Sys_Enum");
                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.EnDesc = " Enumerate ";
                map.EnType = EnType.Sys;
                map.AddMyPK();

                map.AddTBString(SysEnumAttr.Lab, null, "Lab", true, false, 1, 200, 8);
                map.AddTBString(SysEnumAttr.EnumKey, null, "EnumKey", true, false, 1, 40, 8);
                map.AddTBInt(SysEnumAttr.IntKey, 0, "Val", true, false);
                map.AddTBString(SysEnumAttr.Lang, "CH", " Language ", true, false, 0, 10, 8);

                this._enMap = map;
                return this._enMap;
            }
		}		 
		#endregion 

         
        protected override bool beforeUpdateInsertAction()
        {
            if (this.Lang == null && this.Lang == "")
                this.Lang = BP.Web.WebUser.SysLang;

            this.MyPK = this.EnumKey + "_" + this.Lang + "_" + this.IntKey;
            return base.beforeUpdateInsertAction();
        }
	}
	/// <summary>
	///  Taxpayers collection  
	/// </summary>
	public class SysEnums : Entities
	{
        /// <summary>
        ///  The number of this enum type 
        /// </summary>
        public int Num = -1;
        public string ToDesc()
        {
            string strs = "";
            foreach (SysEnum se in this)
            {
                strs += se.IntKey + " " + se.Lab + ";";
            }
            return strs;
        }
        public string GenerCaseWhenForOracle(string enName, string mTable, string key, string field, string enumKey, int def)
        {
            string sql = (string)Cash.GetObjFormApplication("ESQL" + enName +mTable+ key + "_" + enumKey, null);
            // string sql = "";
            if (sql != null)
                return sql;

            if (this.Count == 0)
                throw new Exception("@ Enum value " + enumKey + " Has been deleted .");

            sql = " CASE NVL(" + mTable + field+","+def+")";
            foreach (SysEnum se1 in this)
            {
                sql += " WHEN " + se1.IntKey + " THEN '" + se1.Lab + "'";
            }

            SysEnum se = (SysEnum)this.GetEntityByKey(SysEnumAttr.IntKey, def);
            if (se == null)
                sql += " END " + key + "Text";
            else
                sql += " WHEN NULL THEN '" + se.Lab + "' END " + key + "TEXT";

            Cash.AddObj("ESQL" + enName + mTable + key + "_" + enumKey, Depositary.Application, sql);
            return sql;
        }

        public string GenerCaseWhenForOracle(string mTable, string key, string field, string enumKey, int def)
        {
            if (this.Count == 0)
                throw new Exception("@ Enum value £¨" + enumKey + "£© Has been deleted , Can not form the desired SQL.");


            string sql = "";
            sql = " CASE " + mTable + field;
            foreach (SysEnum se1 in this)
                sql += " WHEN " + se1.IntKey + " THEN '" + se1.Lab + "'";

            SysEnum se = (SysEnum)this.GetEntityByKey(SysEnumAttr.IntKey, def);
            if (se == null)
                sql += " END " + key + "Text";
            else
                sql += " WHEN NULL THEN '" + se.Lab + "' END " + key + "TEXT";

            // Cash.AddObj("ESQL" + enName + key + "_" + enumKey, Depositary.Application, sql);
            return sql;
        }
        public void LoadIt(string enumKey)
        {
            if (this.Full(enumKey) == false)
            {

                try
                {
                    BP.DA.DBAccess.RunSQL("UPDATE Sys_Enum SET Lang='" + Web.WebUser.SysLang + "' WHERE LANG IS NULL ");
                    BP.DA.DBAccess.RunSQL("UPDATE Sys_Enum SET MyPK=EnumKey+'_'+Lang+'_'+cast(IntKey as NVARCHAR )");
                }
                catch
                {

                }

                try
                {
                    BP.Sys.Xml.EnumInfoXml xml = new Xml.EnumInfoXml(enumKey);
                    this.RegIt(enumKey, xml.Vals);
                }
                catch (Exception ex)
                {
                    throw new Exception("@ You do not have pre- [" + enumKey + "] Enum value .@ An error occurred in the repair enum values :" + ex.Message);
                }
            }
        }
		/// <summary>
		/// SysEnums
		/// </summary>
		/// <param name="EnumKey"></param>
        public SysEnums(string enumKey)
        {
            this.LoadIt(enumKey);
        }
        public SysEnums(string enumKey, string vals)
        {
            if (vals == null || vals == "")
            {
                this.LoadIt(enumKey);
                return;
            }

            if (this.Full(enumKey) == false)
                this.RegIt(enumKey, vals);
        }
        public void RegIt(string EnumKey, string vals)
        {
            try
            {
                string[] strs = vals.Split('@');
                SysEnums ens = new SysEnums();
                ens.Delete(SysEnumAttr.EnumKey, EnumKey);
                this.Clear();

                foreach (string s in strs)
                {
                    if (s == "" || s == null)
                        continue;

                    string[] vk = s.Split('=');
                    SysEnum se = new SysEnum();
                    se.IntKey = int.Parse(vk[0]);
                    se.Lab = vk[1];
                    se.EnumKey = EnumKey;
                    se.Lang = BP.Web.WebUser.SysLang;
                    se.Insert();
                    this.AddEntity(se);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " - " + vals);
            }
            //  this.Full(EnumKey);
        }
        public bool Full(string enumKey)
        {
            Entities ens = (Entities)Cash.GetObjFormApplication("EnumOf" + enumKey + Web.WebUser.SysLang, null);
            if (ens != null)
            {
                this.AddEntities(ens);
                return true;
            }

            QueryObject qo = new QueryObject(this);
            qo.AddWhere(SysEnumAttr.EnumKey, enumKey);
            qo.addAnd();
            qo.AddWhere(SysEnumAttr.Lang, Web.WebUser.SysLang);
            qo.addOrderBy(SysEnumAttr.IntKey);
            if (qo.DoQuery() == 0)
            {
                /*  Look xml Are there configuration inside ?*/
                return false;
            }

            Cash.AddObj("EnumOf" + enumKey + Web.WebUser.SysLang, Depositary.Application, this);
            return true;
        }
        ///// <summary>
        ///// DBSimpleNoNames
        ///// </summary>
        ///// <returns></returns>
        //public DBSimpleNoNames ToEntitiesNoName()
        //{
        //    DBSimpleNoNames ens = new DBSimpleNoNames();
        //    foreach (SysEnum en in this)
        //    {
        //        ens.AddByNoName(en.IntKey.ToString(), en.Lab);
        //    }
        //    return ens;
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public int Delete(string key, object val)
        {
            try
            {
                Entity en = this.GetNewEntity;
                Paras ps = new Paras();

                ps.SQL = "DELETE FROM " + en.EnMap.PhysicsTable + " WHERE " + key + "=" + en.HisDBVarStr + "p";
                ps.Add("p", val);
                return en.RunSQL(ps);
            }
            catch
            {
                Entity en = this.GetNewEntity;
                en.CheckPhysicsTable();

                Paras ps = new Paras();
                ps.SQL = "DELETE FROM " + en.EnMap.PhysicsTable + " WHERE " + key + "=" + en.HisDBVarStr + "p";
                ps.Add("p", val);
                return en.RunSQL(ps);
            }
        }
		/// <summary>
		/// SysEnums
		/// </summary>
		public SysEnums(){}
		/// <summary>
		///  Get it  Entity
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new SysEnum();
			}
		}
		/// <summary>
		///  By int  Get Lab
		/// </summary>
		/// <param name="val">val</param>
		/// <returns>string val</returns>
		public string GetLabByVal(int val)
		{
			foreach(SysEnum en in this)
			{
				if (en.IntKey == val)
					return en.Lab;
			}
			return null;
		}
	}
}
