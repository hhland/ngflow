using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Sys;

namespace BP.Port
{
	/// <summary>
	///  Operator Properties 
	/// </summary>
    public class EmpAttr : BP.En.EntityNoNameAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  Department 
        /// </summary>
        public const string FK_Dept = "FK_Dept";
        /// <summary>
        ///  Password 
        /// </summary>
        public const string Pass = "Pass";
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
        ///  Collection department 
        /// </summary>
        public Depts HisDepts
        {
            get
            {
                EmpDepts sts = new EmpDepts();
                Depts dpts = sts.GetHisDepts(this.No);
                if (dpts.Count == 0)
                {
                    string sql = "select FK_Dept from Port_Emp where No='" + this.No + "' and FK_Dept in(select No from Port_Dept)";
                    string fk_dept = BP.DA.DBAccess.RunSQLReturnVal(sql) as string;
                    if (fk_dept == null)
                        return dpts;

                    Dept dept = new Dept(fk_dept);
                    dpts.AddEntity(dept);
                }
                return dpts;
            }
        }
        /// <summary>
        ///  Department number 
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
        /// <summary>
        ///  Department number 
        /// </summary>
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

        #region  Public Methods 
        /// <summary>
        ///  Check the password ( You can override this method )
        /// </summary>
        /// <param name="pass"> Password </param>
        /// <returns> Match success </returns>
        public bool CheckPass(string pass)
        {
            if (SystemConfig.IsDebug)
                return true;

            if (this.Pass == pass )
                return true;
            return false;
        }
        #endregion  Public Methods 

        #region  Constructor 
        /// <summary>
        ///  The operator 
        /// </summary>
        public Emp()
        {
        }
        /// <summary>
        ///  The operator 
        /// </summary>
        /// <param name="no"> Serial number </param>
        public Emp(string no)
        {
            this.No = no.Trim();
            if (this.No.Length == 0)
                throw new Exception("@ Operator number to query is empty .");
            try
            {
                this.Retrieve();
            }
            catch (Exception ex)
            {
                // No user login account inquiries , Number of workers using a query .
                QueryObject obj = new QueryObject(this);
                obj.AddWhere(EmpAttr.No, no);
                int i = obj.DoQuery();
                if (i == 0)
                    i = this.RetrieveFromDBSources();
                if (i == 0)
                    throw new Exception("@ User or password is incorrect :[" + no + "], Or account is deactivated .@ Technical Information ( Query error from memory ):ex1=" + ex.Message);
            }
        }
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

                Map map = new Map();

                #region  Basic properties 
                map.EnDBUrl =
                    new DBUrl(DBUrlType.AppCenterDSN); // To connect to a data source （ Indicate that the system you want to connect to the database ）.
                map.PhysicsTable = "Port_Emp"; //  To physical table .
                map.DepositaryOfMap = Depositary.Application;    // Entity map Storage location .
                map.DepositaryOfEntity = Depositary.Application; // Physical storage location 
                map.EnDesc = " User "; // " User ";
                map.EnType = EnType.App;   // Entity Type .
                #endregion

                #region  Field 
                /*  Add information about field properties  */
                map.AddTBStringPK(EmpAttr.No, null, " Serial number ", true, false, 1, 20, 30); 
                map.AddTBString(EmpAttr.Name, null, " Name ", true, false, 0, 200, 30);
                map.AddTBString(EmpAttr.Pass, "pub", " Password ", false, false, 0, 20, 10);
                map.AddDDLEntities(EmpAttr.FK_Dept, null, " Department ", new Port.Depts(), true);

                //map.AddTBString("Tel", null, "Tel", false, false, 0, 20, 10);
                //map.AddTBString(EmpAttr.PID, null, this.ToE("PID", "UKEY的PID"), true, false, 0, 100, 30);
                //map.AddTBString(EmpAttr.PIN, null, this.ToE("PIN", "UKEY的PIN"), true, false, 0, 100, 30);
                //map.AddTBString(EmpAttr.KeyPass, null, this.ToE("KeyPass", "UKEY的KeyPass"), true, false, 0, 100, 30);
                //map.AddTBString(EmpAttr.IsUSBKEY, null, this.ToE("IsUSBKEY", " Whether to use usbkey"), true, false, 0, 100, 30);
                //map.AddDDLSysEnum("Sex", 0, " Sex ", "@0=女@1=男");
                #endregion  Field 

                map.AddSearchAttr(EmpAttr.FK_Dept);

                #region  Increased many-property 
                // His department permission 
                map.AttrsOfOneVSM.Add(new EmpDepts(), new Depts(), EmpDeptAttr.FK_Emp, 
                    EmpDeptAttr.FK_Dept, DeptAttr.Name, DeptAttr.No, " Department Permissions ");
                map.AttrsOfOneVSM.Add(new EmpStations(), new Stations(), EmpStationAttr.FK_Emp, 
                    EmpStationAttr.FK_Station,DeptAttr.Name, DeptAttr.No, " Permissions post ");
                #endregion

                this._enMap = map;
                return this._enMap;
            }
        }
        /// <summary>
        ///  Gets the collection 
        /// </summary>
        public override Entities GetNewEntities
        {
            get { return new Emps(); }
        }
        #endregion  Constructor 
    }
	/// <summary>
	///  The operator 
	// </summary>
    public class Emps : EntitiesNoName
    {
        #region  Constructor 
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
        ///  The operator s
        /// </summary>
        public Emps()
        {
        }
        #endregion  Constructor 
    }
}
 