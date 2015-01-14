using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Port;

namespace BP.WF.Rpt
{
    /// <summary>
    ///  Staff report 
    /// </summary>
    public class RptEmpAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  Report form ID
        /// </summary>
        public const string FK_Rpt = "FK_Rpt";
        /// <summary>
        ///  Staff 
        /// </summary>
        public const string FK_Emp = "FK_Emp";
        #endregion
    }
    /// <summary>
    /// RptEmp  The summary .
    /// </summary>
    public class RptEmp : Entity
    {

        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                if (BP.Web.WebUser.No == "admin")
                {
                    uac.IsView = true;
                    uac.IsDelete = true;
                    uac.IsInsert = true;
                    uac.IsUpdate = true;
                    uac.IsAdjunct = true;
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
                return this.GetValStringByKey(RptEmpAttr.FK_Rpt);
            }
            set
            {
                SetValByKey(RptEmpAttr.FK_Rpt, value);
            }
        }
        public string FK_EmpT
        {
            get
            {
                return this.GetValRefTextByKey(RptEmpAttr.FK_Emp);
            }
        }
        /// <summary>
        /// Staff 
        /// </summary>
        public string FK_Emp
        {
            get
            {
                return this.GetValStringByKey(RptEmpAttr.FK_Emp);
            }
            set
            {
                SetValByKey(RptEmpAttr.FK_Emp, value);
            }
        }
        #endregion

        #region  Extended Attributes 

        #endregion

        #region  Constructor 
        /// <summary>
        ///  Staff report 
        /// </summary> 
        public RptEmp() { }
        /// <summary>
        ///  Reports corresponding staff 
        /// </summary>
        /// <param name="_empoid"> Report form ID</param>
        /// <param name="wsNo"> Personnel Number </param> 	
        public RptEmp(string _empoid, string wsNo)
        {
            this.FK_Rpt = _empoid;
            this.FK_Emp = wsNo;
            if (this.Retrieve() == 0)
                this.Insert();
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

                Map map = new Map("Sys_RptEmp");
                map.EnDesc = " Corresponding information staff report ";
                map.EnType = EnType.Dot2Dot;

                map.AddTBStringPK(RptEmpAttr.FK_Rpt, null, " Report form ", false, false, 1, 15, 1);
                map.AddDDLEntitiesPK(RptEmpAttr.FK_Emp, null, " Staff ", new Emps(), true);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    ///  Staff report  
    /// </summary>
    public class RptEmps : Entities
    {
        #region  Structure 
        /// <summary>
        ///  Reporting and collection staff 
        /// </summary>
        public RptEmps() { }
        #endregion

        #region  Method 
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new RptEmp();
            }
        }
        #endregion
    }
}
