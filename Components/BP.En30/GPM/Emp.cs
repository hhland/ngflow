using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Port;

namespace BP.GPM
{
    /// <summary>
    ///  Operator Properties 
    /// </summary>
    public class EmpAttr : BP.En.EntityNoNameAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  Number of employees 
        /// </summary>
        public const string EmpNo = "EmpNo";
        /// <summary>
        ///  Department 
        /// </summary>
        public const string FK_Dept = "FK_Dept";
        /// <summary>
        ///  Position 
        /// </summary>
        public const string FK_Duty = "FK_Duty";
        /// <summary>
        /// FK_Unit
        /// </summary>
        public const string FK_Unit = "FK_Unit";
        /// <summary>
        ///  Password 
        /// </summary>
        public const string Pass = "Pass";
        /// <summary>
        /// sid
        /// </summary>
        public const string SID = "SID";
        /// <summary>
        ///  Phone 
        /// </summary>
        public const string Tel = "Tel";
        /// <summary>
        ///  Mailbox 
        /// </summary>
        public const string Email = "Email";
        /// <summary>
        ///  Number of departments 
        /// </summary>
        public const string NumOfDept = "NumOfDept";
        /// <summary>
        ///  No. 
        /// </summary>
        public const string Idx = "Idx";
        #endregion
    }
    /// <summary>
    ///  The operator   The summary .
    /// </summary>
    public class Emp : EntityNoName
    {
        #region  Extended Attributes 
        /// <summary>
        ///  Number of employees 
        /// </summary>
        public string EmpNo
        {
            get
            {
                return this.GetValStrByKey(EmpAttr.EmpNo);
            }
            set
            {
                this.SetValByKey(EmpAttr.EmpNo, value);
            }
        }
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
        /// <summary>
        ///  Position 
        /// </summary>
        public string FK_Duty
        {
            get
            {
                return this.GetValStrByKey(EmpAttr.FK_Duty);
            }
            set
            {
                this.SetValByKey(EmpAttr.FK_Duty, value);
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
        /// <summary>
        ///  Sequence number 
        /// </summary>
        public int Idx
        {
            get
            {
                return this.GetValIntByKey(EmpAttr.Idx);
            }
            set
            {
                this.SetValByKey(EmpAttr.Idx, value);
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
            if (this.Pass == pass)
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
                int i = this.RetrieveFromDBSources();
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
                    new DBUrl(DBUrlType.AppCenterDSN); // To connect to a data source £¨ Indicate that the system you want to connect to the database £©.
                map.PhysicsTable = "Port_Emp"; //  To physical table .
                map.DepositaryOfMap = Depositary.Application;    // Entity map Storage location .
                map.DepositaryOfEntity = Depositary.Application; // Physical storage location 
                map.EnDesc = " User "; // " User "; //  Description of the entity .
                map.EnType = EnType.App;   // Entity Type .
                #endregion

                #region  Field 
                /* Add information about field properties  */
                map.AddTBStringPK(EmpAttr.No, null, " Serial number ", true, false, 1, 20, 30);
                map.AddTBString(EmpAttr.EmpNo, null, " Number of workers ", true, false, 0, 20, 30);
                map.AddTBString(EmpAttr.Name, null, " Name ", true, false, 0, 100, 30);
                map.AddTBString(EmpAttr.Pass, "pub", " Password ", false, false, 0, 100, 10);

                //map.AddDDLEntities(EmpAttr.FK_Dept, null, " Department ", new Port.Depts(), true);

                map.AddTBString(EmpAttr.FK_Dept, null, " The current department ", false, false, 0, 20, 10);
                map.AddTBString(EmpAttr.FK_Duty, null, " Current position ", false, false, 0, 20, 10);
                map.AddTBString(DeptEmpAttr.Leader, null, " Current leadership ", false, false, 0, 200, 1);

                map.AddTBString(EmpAttr.SID, null, " Security Code ", false, false, 0, 36, 36);
                map.AddTBString(EmpAttr.Tel, null, " Phone ", true, false, 0, 20, 130);
                map.AddTBString(EmpAttr.Email, null, " Mailbox ", true, false, 0, 100, 132);
                map.AddTBInt(EmpAttr.NumOfDept, 0, " Number of departments ", true, false);

                map.AddTBInt(EmpAttr.Idx, 0, " No. ", true, false);
                #endregion  Field 

                 map.AddSearchAttr(EmpAttr.FK_Dept);

                ////#region  Point increase on multi-attribute 
                //// His department permission 
                //map.AttrsOfOneVSM.Add(new EmpDepts(), new Depts(), EmpDeptAttr.FK_Emp, EmpDeptAttr.FK_Dept,
                //    DeptAttr.Name, DeptAttr.No, " Department Permissions ");

                //map.AttrsOfOneVSM.Add(new EmpStations(), new Stations(), EmpStationAttr.FK_Emp, EmpStationAttr.FK_Station,
                //    DeptAttr.Name, DeptAttr.No, " Permissions post ");
                ////#endregion

                RefMethod rm = new RefMethod();
                rm.Title = " Change Password ";
                rm.ClassMethodName = this.ToString() + ".DoResetpassword";
                rm.HisAttrs.AddTBString("pass1", null, " Enter the password ", true, false, 0, 100, 100);
                rm.HisAttrs.AddTBString("pass2", null, " Re-enter ", true, false, 0, 100, 100);
                map.AddRefMethod(rm);

                this._enMap = map;
                return this._enMap;
            }
        }

        /// <summary>
        ///  Move up 
        /// </summary>
        public void DoUp()
        {
            this.DoOrderUp(EmpAttr.FK_Dept, this.FK_Dept, EmpAttr.Idx);
        }
        /// <summary>
        ///  Move down 
        /// </summary>
        public void DoDown()
        {
            this.DoOrderDown(EmpAttr.FK_Dept, this.FK_Dept, EmpAttr.Idx);
        }

        public string DoResetpassword(string pass1, string pass2)
        {
            if (pass1.Equals(pass2) == false)
                return " The two passwords do not match ";

            this.Pass = pass1;
            this.Update();
            return " Password set successfully ";
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
    ///  The operator s
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
        public override int RetrieveAll()
        {
            return base.RetrieveAll("Name");
        }
        #endregion  Constructor 
    }
}
