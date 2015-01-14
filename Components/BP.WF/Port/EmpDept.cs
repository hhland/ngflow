using System;
using System.Data;
using BP.DA;
using BP.En;

namespace BP.WF.Port
{
	/// <summary>
    ///  Operator and departments 
	/// </summary>
	public class EmpDeptAttr  
	{
		#region  Basic properties 
		/// <summary>
		///  Staff ID
		/// </summary>
		public const  string FK_Emp="FK_Emp";
		/// <summary>
		///  Department 
		/// </summary>
		public const  string FK_Dept="FK_Dept";		 
		#endregion	
	}
	/// <summary>
    ///  Operator and departments   The summary .
	/// </summary>
	public class EmpDept :Entity
	{
        /// <summary>
        /// UI Access control interface 
        /// </summary>
		public override UAC HisUAC
		{
			get
			{
				UAC uac = new UAC();
				if (BP.Web.WebUser.No== "admin"   )
				{
					uac.IsView=true;
					uac.IsDelete=true;
					uac.IsInsert=true;
					uac.IsUpdate=true;
					uac.IsAdjunct=true;
				}
				return uac;
			}
		}

		#region  Basic properties 
		/// <summary>
		///  Staff ID
		/// </summary>
		public string FK_Emp
		{
			get
			{
				return this.GetValStringByKey(EmpDeptAttr.FK_Emp);
			}
			set
			{
				SetValByKey(EmpDeptAttr.FK_Emp,value);
			}
		}
        public string FK_DeptT
        {
            get
            {
                return this.GetValRefTextByKey(EmpDeptAttr.FK_Dept);
            }
        }
		/// <summary>
		/// Department 
		/// </summary>
		public string FK_Dept
		{
			get
			{
				return this.GetValStringByKey(EmpDeptAttr.FK_Dept);
			}
			set
			{
				SetValByKey(EmpDeptAttr.FK_Dept,value);
			}
		}		  
		#endregion

		#region  Extended Attributes 
		 
		#endregion		

		#region  Constructor 
		/// <summary>
		///  Staff positions 
		/// </summary> 
		public EmpDept(){}
		/// <summary>
		///  Corresponding department staff 
		/// </summary>
		/// <param name="_empoid"> Staff ID</param>
		/// <param name="wsNo"> Department number </param> 	
		public EmpDept(string _empoid,string wsNo)
		{
			this.FK_Emp  = _empoid;
			this.FK_Dept = wsNo ;
			if (this.Retrieve()==0)
				this.Insert();
		}		
		/// <summary>
		///  Override the base class methods 
		/// </summary>
		public override Map EnMap
		{
			get
			{
				if (this._enMap!=null) 
					return this._enMap;
				
				Map map = new Map("Port_EmpDept");
				map.EnDesc=" Operator and departments ";	
				map.EnType=EnType.Dot2Dot;


                map.AddTBStringPK(EmpDeptAttr.FK_Emp, null, " The operator ", false, false, 1, 15, 1);
				//map.AddDDLEntitiesPK(EmpDeptAttr.FK_Emp,null," The operator ",new Emps(),true);
				map.AddDDLEntitiesPK(EmpDeptAttr.FK_Dept,null," Department ",new Depts(),true);

				this._enMap=map;
				return this._enMap;
			}
		}
		#endregion

		#region  Override the base class methods 
		/// <summary>
		///  Before inserting the work done 
		/// </summary>
		/// <returns>true/false</returns>
		protected override bool beforeInsert()
		{
			return base.beforeInsert();			
		}
		/// <summary>
		///  Work done before update 
		/// </summary>
		/// <returns>true/false</returns>
		protected override bool beforeUpdate()
		{
			return base.beforeUpdate(); 
		}
		/// <summary>
		///  Work done before deleting 
		/// </summary>
		/// <returns>true/false</returns>
		protected override bool beforeDelete()
		{
			return base.beforeDelete(); 
		}
		#endregion 
	
	}
	/// <summary>
	///  Operator and departments  
	/// </summary>
	public class EmpDepts : Entities
	{
		#region  Structure 
		/// <summary>
		///  Staff and departments set 
		/// </summary>
		public EmpDepts(){}
		/// <summary>
		///  Staff and departments set 
		/// </summary>
		/// <param name="FK_Emp">FK_Emp</param>
		public EmpDepts(string  FK_Emp)
		{
			QueryObject qo = new QueryObject(this);
			qo.AddWhere(EmpDeptAttr.FK_Emp, FK_Emp);
			qo.DoQuery();
		}
		#endregion

		#region  Method 
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new EmpDept();
			}
		}	
		#endregion 

		#region  Query methods 
	 
		#endregion
				
	}
	
}
