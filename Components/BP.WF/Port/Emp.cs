using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Port;

namespace BP.WF.Port
{
	/// <summary>
	///  Staff Properties 
	/// </summary>
	public class EmpAttr: BP.En.EntityNoNameAttr
	{
		#region  Basic properties 
		/// <summary>
		///  Department 
		/// </summary>
		public const  string FK_Dept="FK_Dept";
        ///// <summary>
        /////  Unit 
        ///// </summary>
        //public const string FK_Unit = "FK_Unit";
        /// <summary>
        ///  Password 
        /// </summary>
        public const string Pass = "Pass";
        /// <summary>
        /// SID
        /// </summary>
        public const string SID = "SID";
		#endregion 
	}
	/// <summary>
	/// Emp  The summary .
	/// </summary>
    public class Emp : EntityNoName
    {
        
        #region  Extended Attributes 
        /// <summary>
        ///  The main sectors .
        /// </summary>
        public Dept HisDept
        {
            get
            {

                try
                {
                    return new Dept(this.FK_Dept);
                }
                catch (Exception ex)
                {
                    throw new Exception("@ Get operator " + this.No + " Department [" + this.FK_Dept + "] Error , May be the system administrator did not give him the maintenance department .@" + ex.Message);
                }
            }
        }
        /// <summary>
        ///  Work set .
        /// </summary>
        public Stations HisStations
        {
            get
            {
                EmpStations sts = new EmpStations();
                Stations mysts = sts.GetHisStations(this.No);
                return mysts;
                //return new Station(this.FK_Station);
            }
        }
        /// <summary>
        ///  Department 
        /// </summary>
        public string FK_Dept
        {
            get
            {
                return this.GetValStrByKey(EmpAttr.FK_Dept);
            }
            set
            {
                this.SetValByKey(EmpAttr.FK_Dept, value);
            }
        }
        public string FK_DeptText
        {
            get
            {
                return this.GetValRefTextByKey(EmpAttr.FK_Dept);
            }
        }
        /// <summary>
        ///  Password 
        /// </summary>
        public string Pass
        {
            get
            {
                return this.GetValStrByKey(EmpAttr.Pass);
            }
            set
            {
                this.SetValByKey(EmpAttr.Pass, value);
            }
        }
        #endregion

        public bool CheckPass(string pass)
        {
            if (this.Pass == pass)
                return true;
            return false;
        }
        /// <summary>
        ///  Staff 
        /// </summary>
        public Emp()
        {
        }
        /// <summary>
        ///  Staff numbers 
        /// </summary>
        /// <param name="_No">No</param>
        public Emp(string no)
        {
            this.No = no.Trim();
            if (this.No.Length == 0)
                throw new Exception("@ Operator number to query is empty .");
            try
            {
                this.Retrieve();
            }
            catch (Exception ex1)
            {
                int i = this.RetrieveFromDBSources();
                if (i == 0)
                    throw new Exception("@ User or password is incorrect :[" + no + "], Or account is deactivated .@ Technical Information ( Query error from memory ):ex1=" + ex1.Message);
            }
        }
        /// <summary>
        /// UI Access control interface 
        /// </summary>
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                uac.OpenForAppAdmin();
                return uac;
            }
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

                #region  Basic properties 
                Map map = new Map();
                map.EnDBUrl =  new DBUrl(DBUrlType.AppCenterDSN); // To connect to a data source £¨ Indicate that the system you want to connect to the database £©.
                map.PhysicsTable = "Port_Emp"; //  To physical table .
                map.DepositaryOfMap = Depositary.Application;    // Entity map Storage location .
                map.DepositaryOfEntity = Depositary.Application; // Physical storage location 
                map.EnDesc = " User "; // " User ";       //  Description of the entity .
                map.EnType = EnType.App;   // Entity Type .
                #endregion

                #region  Field 
                /* Add information about field properties  */
                map.AddTBStringPK(EmpAttr.No, null, " Serial number ", true, false, 1, 20, 100);
                map.AddTBString(EmpAttr.Name, null, " Name ", true, false, 0, 100, 100);
                map.AddTBString(EmpAttr.Pass, "pub", " Password ", false, false, 0, 20, 10);
                map.AddDDLEntities(EmpAttr.FK_Dept, null, " Department ", new BP.Port.Depts(), true);
                map.AddTBString(EmpAttr.SID, null, "SID", false, false, 0, 20, 10);
                
              //  map.AddTBString(EmpAttr.FK_Unit, "1", " Affiliation ", false, false, 0, 200, 10);
                #endregion  Field 

                map.AddSearchAttr(EmpAttr.FK_Dept); // Query conditions .

                // Point increase on multi-attribute   Department of a query operator privileges and permissions posts .
                map.AttrsOfOneVSM.Add(new EmpStations(), new Stations(), 
                    EmpStationAttr.FK_Emp, EmpStationAttr.FK_Station, DeptAttr.Name, DeptAttr.No, " Permissions post ");
                map.AttrsOfOneVSM.Add(new EmpDepts(), new Depts(), EmpDeptAttr.FK_Emp,
                    EmpDeptAttr.FK_Dept, DeptAttr.Name, DeptAttr.No, " Departments ");

                RefMethod rm = new RefMethod();
                rm.Title = " Disable ";
                rm.Warning = " Are you sure you want to perform ?";
                rm.ClassMethodName = this.ToString() + ".DoDisableIt";
                map.AddRefMethod(rm);
                rm = new RefMethod();
                rm.Title = " Enable ";
                rm.Warning = " Are you sure you want to perform ?";
                rm.ClassMethodName = this.ToString() + ".DoEnableIt";
                map.AddRefMethod(rm);
                this._enMap = map;
                return this._enMap;
            }
        }
        /// <summary>
        ///  Execute Disable 
        /// </summary>
        public string DoDisableIt()
        {
            WFEmp emp = new WFEmp(this.No);
            emp.UseSta = 0;
            emp.Update();
            return " Has been executed ( Disable ) Success ";
        }
        /// <summary>
        ///  Execution enabled 
        /// </summary>
        public string DoEnableIt()
        {
            WFEmp emp = new WFEmp(this.No);
            emp.UseSta = 1;
            emp.Update();
            return " Has been executed ( Enable ) Success ";
        }

        protected override bool beforeUpdate()
        {
            WFEmp emp = new WFEmp(this.No);
            emp.Update();
            return base.beforeUpdate();
        }
        public override Entities GetNewEntities
        {
            get { return new Emps(); }
        }
    }
	/// <summary>
	///  Staff 
	/// </summary>
    public class Emps : EntitiesNoName
    {
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new Emp();
            }
        }
        /// <summary>
        ///  Staff s
        /// </summary>
        public Emps()
        {
        }
        /// <summary>
        ///  Check all 
        /// </summary>
        /// <returns></returns>
        public override int RetrieveAll()
        {
           return  base.RetrieveAll();

            //QueryObject qo = new QueryObject(this);
            //qo.AddWhere(EmpAttr.FK_Dept, " like ", BP.Web.WebUser.FK_Dept + "%");
            //qo.addOrderBy(EmpAttr.No);
            //return qo.DoQuery();
        }
    }
}
 