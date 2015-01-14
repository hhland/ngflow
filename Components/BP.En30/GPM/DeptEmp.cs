using System;
using System.Data;
using BP.DA;
using BP.En;

namespace BP.GPM
{
	/// <summary>
	///  Department personnel information 
	/// </summary>
	public class DeptEmpAttr  
	{
		#region  Basic properties 
		/// <summary>
		///  Department 
		/// </summary>
		public const  string FK_Dept="FK_Dept";
        /// <summary>
        ///  Staff 
        /// </summary>
        public const string FK_Emp = "FK_Emp";
		/// <summary>
		///  Position 
		/// </summary>
		public const  string FK_Duty="FK_Duty";
        /// <summary>
        ///  Position Level 
        /// </summary>
        public const string DutyLevel = "DutyLevel";
        /// <summary>
        ///  Its leadership 
        /// </summary>
        public const string Leader = "Leader";
		#endregion	
	}
	/// <summary>
    ///  Department personnel information   The summary .
	/// </summary>
    public class DeptEmp : EntityMyPK
    {
        #region  Basic properties 
        /// <summary>
        /// UI Access control interface 
        /// </summary>
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                uac.OpenForSysAdmin();
                return uac;
            }
        }
        /// <summary>
        ///  Staff 
        /// </summary>
        public string FK_Emp
        {
            get
            {
                return this.GetValStringByKey(DeptEmpAttr.FK_Emp);
            }
            set
            {
                SetValByKey(DeptEmpAttr.FK_Emp, value);
                this.MyPK = this.FK_Dept + "_"  + this.FK_Emp;
            }
        }
        /// <summary>
        ///  Department 
        /// </summary>
        public string FK_Dept
        {
            get
            {
                return this.GetValStringByKey(DeptEmpAttr.FK_Dept);
            }
            set
            {
                SetValByKey(DeptEmpAttr.FK_Dept, value);
                this.MyPK = this.FK_Dept + "_" + this.FK_Emp;
            }
        }
        public string FK_DutyT
        {
            get
            {
                return this.GetValRefTextByKey(DeptEmpAttr.FK_Duty);
            }
        }
        /// <summary>
        /// Position 
        /// </summary>
        public string FK_Duty
        {
            get
            {
                return this.GetValStringByKey(DeptEmpAttr.FK_Duty);
            }
            set
            {
                SetValByKey(DeptEmpAttr.FK_Duty, value);
                this.MyPK = this.FK_Dept + "_" + this.FK_Duty + "_" + this.FK_Emp;
            }
        }
        /// <summary>
        ///  Leadership 
        /// </summary>
        public string Leader
        {
            get
            {
                return this.GetValStringByKey(DeptEmpAttr.Leader);
            }
            set
            {
                SetValByKey(DeptEmpAttr.Leader, value);
            }
        }
        #endregion

        #region  Extended Attributes 

        #endregion

        #region  Constructor 
        /// <summary>
        ///  Department personnel information 
        /// </summary> 
        public DeptEmp() { }
        /// <summary>
        ///  Inquiry 
        /// </summary>
        /// <param name="deptNo"> Department number </param>
        /// <param name="empNo"> Personnel Number </param>
        public DeptEmp(string deptNo, string empNo)
        {
            this.FK_Dept = deptNo;
            this.FK_Emp = empNo;
            this.MyPK = this.FK_Dept + "_" + this.FK_Emp;
            this.Retrieve();
        }
        /// <summary>
        ///  Override the base class methods 
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("Port_DeptEmp");
                map.EnDesc = " Department personnel information ";

                map.AddMyPK();
                map.AddTBString(DeptEmpAttr.FK_Emp, null, " The operator ", false, false, 1, 50, 1);
                map.AddTBString(DeptEmpAttr.FK_Dept, null, " Department ", false, false, 1, 50, 1);
                map.AddTBString(DeptEmpAttr.FK_Duty, null, " Position ", false, false, 0, 200, 1);
                map.AddTBInt(DeptEmpAttr.DutyLevel, 0, " Position Level ", false, false);

                map.AddTBString(DeptEmpAttr.Leader, null, " Leadership ", false, false, 0, 200, 1);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        /// <summary>
        ///  Before doing the update 
        /// </summary>
        /// <returns></returns>
        protected override bool beforeUpdateInsertAction()
        {
            this.MyPK = this.FK_Dept + "_" + this.FK_Emp;
            return base.beforeUpdateInsertAction();
        }
    }
	/// <summary>
    ///  Department personnel information  
	/// </summary>
	public class DeptEmps : EntitiesMyPK
	{
		#region  Structure 
		/// <summary>
		///  Department personnel information 
		/// </summary>
		public DeptEmps()
		{
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
				return new DeptEmp();
			}
		}	
		#endregion 
		
	}
}
