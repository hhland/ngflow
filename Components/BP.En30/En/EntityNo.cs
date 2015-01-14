using System;
using System.Collections;
using BP.En;
using BP.DA;

namespace BP.En
{
	/// <summary>
	///  Property 
	/// </summary>
	public class EntityNoAttr
	{
		/// <summary>
		///  Serial number 
		/// </summary>
		public const string No="No";
	}
    public class EntityNoMyFileAttr : EntityNoAttr
    {
        /// <summary>
        /// MyFileName
        /// </summary>
        public const string MyFileName = "MyFileName";
        /// <summary>
        /// MyFilePath
        /// </summary>
        public const string MyFilePath = "MyFilePath";
        /// <summary>
        /// MyFileExt
        /// </summary>
        public const string MyFileExt = "MyFileExt";
        public const string WebPath = "WebPath";
        public const string MyFileH = "MyFileH";
        public const string MyFileW = "MyFileW";
        public const string MyFileNum = "MyFileNum";
    }
	/// <summary>
	/// NoEntity  The summary .
	/// </summary>
	abstract public class EntityNo: Entity
	{
		#region  Properties offered 
        public override string PK
        {
            get
            {
                return "No";
            }
        }
		/// <summary>
		///  Serial number 
		/// </summary>
		public string No
		{
            get
            {
                return this.GetValStringByKey(EntityNoNameAttr.No);
            }
            set
            {
                this.SetValByKey(EntityNoNameAttr.No, value);
            }
		}
		#endregion

		#region  With the number of logical operations related ( This property only with dict EntityNo,  Base class has a relationship .)
		/// <summary>
		/// Insert  Before operation .
		/// </summary>
		/// <returns></returns>
        protected override bool beforeInsert()
        {

            Attr attr = this.EnMap.GetAttrByKey("No");
            if (attr.UIVisible == true && attr.UIIsReadonly && this.EnMap.IsAutoGenerNo && this.No.Length==0)
                this.No = this.GenerNewNo;

            return base.beforeInsert();
            ////if (this.EnMap.IsAutoGenerNo == true && (this.No == "" || this.No == null || this.No == " Automatic generation "))
            ////{
            ////    this.No = this.GenerNewNo;
            ////}
            //if (this.EnMap.IsAllowRepeatNo == false)
            //{
            //    string field = attr.Field;

            //    Paras ps = new Paras();
            //    ps.Add("no", No);
            //    string sql = "SELECT " + field + " FROM " + this.EnMap.PhysicsTable + " WHERE " + field + "=:no";
            //    if (DBAccess.IsExits(sql, ps))
            //        throw new Exception("@[" + this.EnMap.EnDesc + " , " + this.EnMap.PhysicsTable + "]  Serial number [" + No + "] Repeat .");
            //}

            ////  Is not check the length of numbers .
            //if (this.EnMap.IsCheckNoLength)
            //{
            //    if (this.No.Length!=this.EnMap.CodeLength )
            //        throw new Exception("@ ["+this.EnMap.EnDesc+"] Serial number ["+this.No+"] Error , Length does not meet the system requirements , Must be ["+this.EnMap.CodeLength.ToString()+"]位, And now there is a length ["+this.No.Length.ToString()+"]位.");
            //}
            //return base.beforeInsert();
        }
		#endregion 
		 
		#region  Number culvert construction 
		/// <summary>
		///  Case of an entity 
		/// </summary>
		public EntityNo()
		{
		}
		/// <summary>
		///  Get entity by number .
		/// </summary>
		/// <param name="_no"> Serial number </param>
		public EntityNo(string _no)  
		{
			if (_no==null || _no=="")
				throw new Exception( this.EnDesc+"@ On the table ["+this.EnDesc+"] You must specify the number before query .");

			this.No = _no ;
			if (this.Retrieve()==0) 
			{				
				throw new Exception("@ No "+this._enMap.PhysicsTable+", No = "+No+" Records .");
			}
		}
        public override int Save()
        {
            /* If you include the number . */
            if (this.IsExits)
            {
                return this.Update();
            }
            else
            {
                if (this.EnMap.IsAutoGenerNo
                    && this.EnMap.GetAttrByKey("No").UIIsReadonly)
                    this.No = this.GenerNewNo;

                this.Insert();
                return 0;
            }

           // return base.Save();
        }
		#endregion		

		#region  Search methods provided 
		/// <summary>
		///  Generate a number 
		/// </summary>
		public string GenerNewNo
		{
            get
            {
                return this.GenerNewNoByKey("No");
            }
		}
		/// <summary>
		/// 按 No  Inquiry .
		/// </summary>
		/// <returns></returns>
        public int RetrieveByNo()
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(EntityNoAttr.No, this.No);
            return qo.DoQuery();
        }
		/// <summary>
		/// 按 No  Inquiry .
		/// </summary>
		/// <param name="_No">No</param>
		/// <returns></returns>
		public int RetrieveByNo(string _No) 
		{
			this.No = _No ;
			QueryObject qo = new QueryObject(this);
			qo.AddWhere(EntityNoAttr.No,this.No);
			return qo.DoQuery();
		}
		#endregion
	}
	/// <summary>
	///  Number entity set .
	/// </summary>
	abstract public class EntitiesNo : Entities
	{
        public override int RetrieveAllFromDBSource()
        {
            QueryObject qo = new QueryObject(this);
            qo.addOrderBy("No");
            return qo.DoQuery();
        }
	}
}
