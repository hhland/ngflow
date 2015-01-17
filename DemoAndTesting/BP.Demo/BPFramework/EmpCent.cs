using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Port;

namespace BP.Demo
{
    /// <summary>
    ///  Employee assessment score   Property 
    /// </summary>
    public class EmpCentAttr : EntityNoNameAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  Staff 
        /// </summary>
        public const string FK_Emp = "FK_Emp";
        /// <summary>
        ///  Score 
        /// </summary>
        public const string Cent = "Cent";
        /// <summary>
        ///  Years 
        /// </summary>
        public const string FK_NY = "FK_NY";
        /// <summary>
        ///  Department 
        /// </summary>
        public const string FK_Dept = "FK_Dept";
        #endregion
    }
    /// <summary>
    ///  Employee assessment score 
    /// </summary>
    public class EmpCent : EntityMyPK
    {
        #region  Property 
        /// <summary>
        ///  Staff 
        /// </summary>
        public string FK_Emp
        {
            get
            {
                return this.GetValStringByKey(EmpCentAttr.FK_Emp);
            }
            set
            {
                this.SetValByKey(EmpCentAttr.FK_Emp, value);
            }
        }
        /// <summary>
        ///  Score 
        /// </summary>
        public float Cent
        {
            get
            {
                return this.GetValFloatByKey(EmpCentAttr.Cent);
            }
            set
            {
                this.SetValByKey(EmpCentAttr.Cent, value);
            }
        }
        /// <summary>
        ///  Years 
        /// </summary>
        public string FK_NY
        {
            get
            {
                return this.GetValStringByKey(EmpCentAttr.FK_NY);
            }
            set
            {
                this.SetValByKey(EmpCentAttr.FK_NY, value);
            }
        }
        /// <summary>
        ///  Department 
        /// </summary>
        public string FK_Dept
        {
            get
            {
                return this.GetValStringByKey(EmpCentAttr.FK_Dept);
            }
            set
            {
                this.SetValByKey(EmpCentAttr.FK_Dept, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Employee assessment score 
        /// </summary>
        public EmpCent()
        {
        }
        /// <summary>
        ///  Employee assessment score 
        /// </summary>
        /// <param name="mypk"></param>
        public EmpCent(string mypk):base(mypk)
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
                Map map = new Map("Demo_EmpCent");
                map.EnDesc = " Employee assessment score ";
                
                //  Ordinary field 
                map.AddMyPK();
                map.AddTBString(EmpCentAttr.FK_Emp, null, " Staff ", true, false, 0, 200, 10);
                map.AddTBString(EmpCentAttr.FK_Dept, null, " Membership department ( Redundant column )", true, false, 0, 200, 10);
                map.AddTBString(EmpCentAttr.FK_NY, null, " Month ", true, false, 0, 200, 10);
                map.AddTBFloat(EmpCentAttr.Cent, 0, " Score ", true, false);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        /// <summary>
        ///  Override the base class methods .
        /// </summary>
        /// <returns></returns>
        protected override bool beforeInsert()
        {
            return base.beforeInsert();
        }
    }
    /// <summary>
    ///  Employee assessment score s
    /// </summary>
    public class EmpCents : EntitiesMyPK
    {
        #region  Method 
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new EmpCent();
            }
        }
        /// <summary>
        ///  Employee assessment score s
        /// </summary>
        public EmpCents() { }
        #endregion
    }
}
