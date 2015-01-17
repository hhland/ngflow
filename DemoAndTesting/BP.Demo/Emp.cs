using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.WF;
using BP.Port;

namespace BP.Demo
{
    /// <summary>
    ///  Staff   Property 
    /// </summary>
    public class EmpAttr:EntityNoNameAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  Phone 
        /// </summary>
        public const string Tel = "Tel";
        /// <summary>
        ///  Mail 
        /// </summary>
        public const string Email = "Email";
        /// <summary>
        ///  Sex 
        /// </summary>
        public const string XB = "XB";
        /// <summary>
        ///  Address 
        /// </summary>
        public const string Addr = "Addr";
        /// <summary>
        ///  Department 
        /// </summary>
        public const string FK_Dept = "FK_Dept";
        /// <summary>
        ///  Whether to enable 
        /// </summary>
        public const string IsEnable = "IsEnable";
        /// <summary>
        ///  Age 
        /// </summary>
        public const string Age = "Age";
        #endregion
    }
    /// <summary>
    ///  Staff 
    /// </summary>
    public class Emp : EntityNoName
    {
        #region  Property 
        /// <summary>
        ///  Mail 
        /// </summary>
        public string Email
        {
            get
            {  return this.GetValStringByKey(EmpAttr.Email); }
            set
            {  this.SetValByKey(EmpAttr.Email, value); }
        }
        /// <summary>
        ///  Sex 
        /// </summary>
        public int XB
        {
            get
            { return this.GetValIntByKey(EmpAttr.XB);    }
            set
            {  this.SetValByKey(EmpAttr.XB, value);      }
        }
        /// <summary>
        ///  Gender labels 
        /// </summary>
        public string XB_Text
        {
            get
            { return this.GetValRefTextByKey(EmpAttr.XB); }
        }
        /// <summary>
        ///  Address 
        /// </summary>
        public string Addr
        {
            get
            {
                return this.GetValStringByKey(EmpAttr.Addr);
            }
            set
            {
                this.SetValByKey(EmpAttr.Addr, value);
            }
        }
        /// <summary>
        ///  Phone 
        /// </summary>
        public string Tel
        {
            get
            {
                return this.GetValStringByKey(EmpAttr.Tel);
            }
            set
            {
                this.SetValByKey(EmpAttr.Tel, value);
            }
        }
        /// <summary>
        ///  Department number 
        /// </summary>
        public string FK_Dept
        {
            get
            { return this.GetValStringByKey(EmpAttr.FK_Dept);  }
            set
            {  this.SetValByKey(EmpAttr.FK_Dept, value); }
        }
        /// <summary>
        ///  Department name 
        /// </summary>
        public string FK_Dept_Text
        {
            get
            { return this.GetValStringByKey(EmpAttr.FK_Dept);   }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Staff 
        /// </summary>
        public Emp()
        {
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
                Map map = new Map("Demo_Emp");
                map.EnDesc = " Staff ";
                map.DepositaryOfEntity= Depositary.None;
                // Field Mapping .
                map.AddTBStringPK(EmpAttr.No,null," Serial number ",true,false,5,40,4);
                map.AddTBString(EmpAttr.Name, null, "name", true, false, 0, 200, 10);
                map.AddTBString(EmpAttr.Tel, null, " Phone ", true, false, 0, 200, 10);
                map.AddTBString(EmpAttr.Email, null, "Email", true, false, 0, 200, 10);
                map.AddTBString(EmpAttr.Addr, null, "Addr", true, false, 0, 200, 10);
                map.AddBoolean(EmpAttr.IsEnable, true, " Whether to enable ", true, true);
                map.AddDDLSysEnum(EmpAttr.XB, 0, " Sex ", true,true,"XB","@0=Å®@1=ÄÐ");
                map.AddDDLEntities(EmpAttr.FK_Dept, null, " Department ", new BP.Port.Depts(), true);
                map.AddTBInt(EmpAttr.Age, 18, " Age ", true, false);

                // Query conditions 
                map.AddSearchAttr(EmpAttr.XB);
                map.AddSearchAttr(EmpAttr.FK_Dept);

                // Method with a parameter mapping .
                RefMethod rm = new RefMethod();
                rm.Title = " Transfer ";
                rm.Warning = " Are you sure you want to perform mobilize ?";
                // Two additional parameters .
                //rm.HisAttrs.AddDDLEntities("FK_Dept", null, " To transfer to the department ",  new BP.Port.Depts(), true);
                rm.HisAttrs.AddTBString("Note", null, " Reason for the call ", true, false, 0, 1000, 100);
                rm.ClassMethodName = this.ToString() + ".DoMove";
                map.AddRefMethod(rm);
                this._enMap = map;
                return this._enMap;
            }
        }
        /// <summary>
        ///  Execution mobilize 
        /// </summary>
        /// <param name="fk_dept"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public string DoMove(string fk_dept, string note)
        {
            // Write your logic .
            Dept dept = new Dept(fk_dept);
            return " The staff has been successfully mobilized to change :"+dept.Name+" ,  Reason for the call is :"+note;
        }
        #endregion
    }
    /// <summary>
    ///  Staff s
    /// </summary>
    public class Emps : EntitiesNoName
    {
        #region  Method 
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
        ///  Check all ( You can override this method )
        /// </summary>
        /// <returns></returns>
        public override int RetrieveAll()
        {
            return base.RetrieveAll();
        }
        #endregion
    }
}