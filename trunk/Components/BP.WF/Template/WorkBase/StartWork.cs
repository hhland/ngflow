using System;
using System.Data;
using System.Collections;
using BP.DA;
using BP.En;
using BP.Port;

namespace BP.WF
{
	/// <summary>
	///  Started working base class property 
	/// </summary>
    public class StartWorkAttr : WorkAttr
    {
        #region  Property 
        /// <summary>
        ///  Department 
        /// </summary>
        public const string FK_Dept = "FK_Dept";
        /// <summary>
        ///  Work content title 
        /// </summary>
        public const string Title = "Title";
        /// <summary>
        /// PRI
        /// </summary>
        public const string PRI = "PRI";
        #endregion

        #region  Sons process property 
        public const string PFlowNo = "PFlowNo";
        public const string PWorkID = "PWorkID";
        public const string PNodeID = "PNodeID";
        #endregion  Sons process property 
    }
	/// <summary>	 
	///  Start working base class , All work must begin inherited from here 
	/// </summary>
	abstract public class StartWork : Work 
	{
        #region ”Î_SQLCash  Related to the operation 
        private SQLCash _SQLCash = null;
        public override SQLCash SQLCash
        {
            get
            {
                if (_SQLCash == null)
                {
                    _SQLCash = BP.DA.Cash.GetSQL(this.NodeID.ToString());
                    if (_SQLCash == null)
                    {
                        _SQLCash = new SQLCash(this);
                        BP.DA.Cash.SetSQL(this.NodeID.ToString(), _SQLCash);
                    }
                }
                return _SQLCash;
            }
            set
            {
                _SQLCash = value;
            }
        }
        #endregion

		#region   Document properties 
		/// <summary>
		/// FK_Dept
		/// </summary>
		public string FK_Dept
		{
			get
			{
				return this.GetValStringByKey(StartWorkAttr.FK_Dept);
			}
            set
            {
                this.SetValByKey(StartWorkAttr.FK_Dept, value);
            } 
		}
        //public string FK_DeptOf2Code
        //{
        //    get
        //    {
        //        return this.FK_Dept.Substring(6);
        //    } 
        //}
		/// <summary>
		/// FK_XJ
		/// </summary>
        //public string FK_XJ
        //{
        //    get
        //    {
        //        return this.GetValStringByKey(StartWorkAttr.FK_Dept);
        //    }
        //    set
        //    {
        //        this.SetValByKey(StartWorkAttr.FK_Dept, value);
        //    }
        //}
		#endregion

		#region  Basic properties 
		/// <summary>
		///  Work content title 
		/// </summary>
		public string Title
		{
			get
			{
				return this.GetValStringByKey(StartWorkAttr.Title);
			}
			set
			{
				this.SetValByKey(StartWorkAttr.Title,value);
			} 
		}
		#endregion

		#region  Constructor 
		/// <summary>
		///  Workflow 
		/// </summary>
		protected StartWork()
		{
		}
        protected StartWork(Int64 oid):base(oid)
        {
        }
		#endregion
		
		#region   Override the base class methods .
		/// <summary>
		///  Removed before operation .
		/// </summary>
		/// <returns></returns>
		protected override bool beforeDelete() 
		{
			if (base.beforeDelete()==false)
				return false;			 
			if (this.OID < 0 )
				throw new Exception("@ Entity ["+this.EnDesc+"] Has not been instantiated , Can not Delete().");
			return true;
		}
		/// <summary>
		///  Inserted before the operation .
		/// </summary>
		/// <returns></returns>
        protected override bool beforeInsert()
        {
            if (this.OID > 0)
                throw new Exception("@ Entity [" + this.EnDesc + "],  Has been instantiated , Can not Insert.");

            this.SetValByKey("OID", DBAccess.GenerOID());
            return base.beforeInsert();
        }
        protected override bool beforeUpdateInsertAction()
        {
            this.Emps = BP.Web.WebUser.No;
            return base.beforeUpdateInsertAction();
        }
		/// <summary>
		///  Update operation 
		/// </summary>
		/// <returns></returns>
		protected override bool beforeUpdate()
		{
			if (base.beforeUpdate()==false)
				return false;
			if (this.OID < 0 )			
				throw new Exception("@ Entity ["+this.EnDesc+"] Has not been instantiated , Can not Update().");
			return base.beforeUpdate();
		}
		#endregion
	}
	/// <summary>
	///  Workflow information collection base class   Set 
	/// </summary>
	abstract public class StartWorks : Works
	{
		#region  Constructor 
		/// <summary>
		///  Information collection base class 
		/// </summary>
		public StartWorks()
		{
		}
		#endregion 

		#region  Public inquiry method 
		/// <summary>
		///  Query to my task .
		/// </summary>		 
		/// <returns></returns>
		public DataTable RetrieveMyTask_del(string flow)
		{
			QueryObject qo = new QueryObject(this);
			//qo.Top=50;
            qo.AddWhere(StartWorkAttr.OID, " in ", " ( SELECT WorkID FROM V_WF_Msg  WHERE  FK_Flow='" + flow + "' AND FK_Emp='" + BP.Web.WebUser.No + "' )");
			return qo.DoQueryToTable();
		}
		/// <summary>
		///  Query to my task .
		/// </summary>		 
		/// <returns></returns>
		public DataTable RetrieveMyTask(string flow)
		{
			//string sql="SELECT OID AS WORKID, TITLE, ";
			QueryObject qo = new QueryObject(this);
			//qo.Top=50;
			if (BP.Sys.SystemConfig.AppCenterDBType==DBType.Oracle)
                qo.AddWhere(StartWorkAttr.OID, " in ", " (  SELECT WorkID FROM WF_GenerWorkFlow WHERE FK_Node IN ( SELECT FK_Node FROM WF_GenerWorkerlist WHERE FK_Emp='" + BP.Web.WebUser.No + "' AND FK_Flow='" + flow + "' AND WORKID=WF_GenerWorkFlow.WORKID ) )");
			else
                qo.AddWhere(StartWorkAttr.OID, " in ", " (  SELECT WorkID FROM WF_GenerWorkFlow WHERE FK_Node IN ( SELECT FK_Node FROM WF_GenerWorkerlist WHERE FK_Emp='" + BP.Web.WebUser.No + "' AND FK_Flow='" + flow + "' AND WORKID=WF_GenerWorkFlow.WORKID ) )");
			return qo.DoQueryToTable();
		}
		/// <summary>
		///  Query to my task .
		/// </summary>		 
		/// <returns></returns>
		public DataTable RetrieveMyTaskV2(string flow)
		{
            string sql = "SELECT OID, TITLE, RDT, Rec FROM  " + this.GetNewEntity.EnMap.PhysicsTable + " WHERE OID IN (   SELECT WorkID FROM WF_GenerWorkFlow WHERE FK_Node IN ( SELECT FK_Node FROM WF_GenerWorkerlist WHERE FK_Emp='" + BP.Web.WebUser.No + "' AND FK_Flow='" + flow + "' AND WORKID=WF_GenerWorkFlow.WORKID )  )";
			return DBAccess.RunSQLReturnTable(sql) ;
			/*
			QueryObject qo = new QueryObject(this);
			//qo.Top=50;
			qo.AddWhere(StartWorkAttr.OID," in ", " ( SELECT WorkID FROM V_WF_Msg  WHERE  FK_Flow='"+flow+"' AND FK_Emp="+Web.WebUser.No+")" );
			return qo.DoQueryToTable();
			*/
		}
		/// <summary>
		///  Query to my task .
		/// </summary>		 
		/// <returns></returns>
		public DataTable RetrieveMyTask(string flow,string flowSort)
		{
			QueryObject qo = new QueryObject(this);
			//qo.Top=50;
            qo.AddWhere(StartWorkAttr.OID, " IN ", " ( SELECT WorkID FROM V_WF_Msg  WHERE  (FK_Flow='" + flow + "' AND FK_Emp='" + BP.Web.WebUser.No + "' ) AND ( FK_Flow in ( SELECT No from WF_Flow WHERE FK_FlowSort='" + flowSort + "' )) )");
			return qo.DoQueryToTable();			 
		}
		#endregion 
	}
}
