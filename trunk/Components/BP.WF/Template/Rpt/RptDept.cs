using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Port;

namespace BP.WF.Rpt
{
	/// <summary>
	///  Reports department 
	/// </summary>
	public class RptDeptAttr  
	{
		#region  Basic properties 
		/// <summary>
		///  Report form ID
		/// </summary>
		public const  string FK_Rpt="FK_Rpt";
		/// <summary>
		///  Department 
		/// </summary>
		public const  string FK_Dept="FK_Dept";		 
		#endregion	
	}
	/// <summary>
	/// RptDept  The summary .
	/// </summary>
	public class RptDept :Entity
	{
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
		///  Report form ID
		/// </summary>
		public string FK_Rpt
		{
			get
			{
				return this.GetValStringByKey(RptDeptAttr.FK_Rpt);
			}
			set
			{
				SetValByKey(RptDeptAttr.FK_Rpt,value);
			}
		}
        public string FK_DeptT
        {
            get
            {
                return this.GetValRefTextByKey(RptDeptAttr.FK_Dept);
            }
        }
		/// <summary>
		/// Department 
		/// </summary>
		public string FK_Dept
		{
			get
			{
				return this.GetValStringByKey(RptDeptAttr.FK_Dept);
			}
			set
			{
				SetValByKey(RptDeptAttr.FK_Dept,value);
			}
		}		  
		#endregion

		#region  Extended Attributes 
		 
		#endregion		

		#region  Constructor 
		/// <summary>
		///  Reports posts 
		/// </summary> 
		public RptDept(){}
		/// <summary>
		///  Corresponding department reports 
		/// </summary>
		/// <param name="_empoid"> Report form ID</param>
		/// <param name="wsNo"> Department number </param> 	
		public RptDept(string _empoid,string wsNo)
		{
			this.FK_Rpt  = _empoid;
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
				
				Map map = new Map("Sys_RptDept");
				map.EnDesc=" Corresponding information department reports ";	
				map.EnType=EnType.Dot2Dot;

                map.AddTBStringPK(RptDeptAttr.FK_Rpt, null, " Report form ", false, false, 1, 15, 1);
				map.AddDDLEntitiesPK(RptDeptAttr.FK_Dept,null," Department ",new Depts(),true);

				this._enMap=map;
				return this._enMap;
			}
		}
		#endregion
	}
	/// <summary>
	///  Reports department  
	/// </summary>
	public class RptDepts : Entities
	{
		#region  Structure 
		/// <summary>
		///  Reporting and collection department 
		/// </summary>
		public RptDepts(){}
		#endregion

		#region  Method 
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new RptDept();
			}
		}	
		#endregion 
	}
}
