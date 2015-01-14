using System;
using System.Data;
using BP.DA;
using BP.En;

namespace BP.GPM
{
    /// <summary>
    ///  Sector inquiries Permissions 
    /// </summary>
    public class DeptSearchScorpAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  Staff ID
        /// </summary>
        public const string FK_Emp = "FK_Emp";
        /// <summary>
        ///  Department 
        /// </summary>
        public const string FK_Dept = "FK_Dept";
        #endregion
    }
    /// <summary>
    ///  Sector inquiries Permissions   The summary .
    /// </summary>
    public class DeptSearchScorp : Entity
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
        ///  Staff ID
        /// </summary>
        public string FK_Emp
        {
            get
            {
                return this.GetValStringByKey(DeptSearchScorpAttr.FK_Emp);
            }
            set
            {
                SetValByKey(DeptSearchScorpAttr.FK_Emp, value);
            }
        }
        public string FK_DeptT
        {
            get
            {
                return this.GetValRefTextByKey(DeptSearchScorpAttr.FK_Dept);
            }
        }
        /// <summary>
        /// Department 
        /// </summary>
        public string FK_Dept
        {
            get
            {
                return this.GetValStringByKey(DeptSearchScorpAttr.FK_Dept);
            }
            set
            {
                SetValByKey(DeptSearchScorpAttr.FK_Dept, value);
            }
        }
        #endregion

        #region  Extended Attributes 

        #endregion

        #region  Constructor 
        /// <summary>
        ///  Staff positions 
        /// </summary> 
        public DeptSearchScorp() { }
        /// <summary>
        ///  Corresponding department staff 
        /// </summary>
        /// <param name="_empoid"> Staff ID</param>
        /// <param name="wsNo"> Department number </param> 	
        public DeptSearchScorp(string _empoid, string wsNo)
        {
            this.FK_Emp = _empoid;
            this.FK_Dept = wsNo;
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

                Map map = new Map("Port_DeptSearchScorp");
                map.EnDesc = " Sector inquiries Permissions ";
                map.EnType = EnType.Dot2Dot;

                map.AddTBStringPK(DeptSearchScorpAttr.FK_Emp, null, " The operator ", true, true, 1, 50, 11);
                map.AddDDLEntitiesPK(DeptSearchScorpAttr.FK_Dept, null, " Department ", new Depts(), true);
                // map.AddDDLEntitiesPK(DeptSearchScorpAttr.FK_Emp, null, " The operator ", new Emps(), true);
                this._enMap = map;
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
    ///  Sector inquiries Permissions  
    /// </summary>
    public class DeptSearchScorps : Entities
    {
        #region  Structure 
        /// <summary>
        ///  Sector inquiries Permissions 
        /// </summary>
        public DeptSearchScorps() { }
        /// <summary>
        ///  Sector inquiries Permissions 
        /// </summary>
        /// <param name="FK_Emp">FK_Emp</param>
        public DeptSearchScorps(string FK_Emp)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(DeptSearchScorpAttr.FK_Emp, FK_Emp);
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
                return new DeptSearchScorp();
            }
        }
        #endregion

        #region  Query methods 

        #endregion

    }

}
