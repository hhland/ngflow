using System;
using System.Data;
using BP.DA;
using BP.Port;
using BP.En; 

namespace BP.PRJ
{
	/// <summary>
	///  Project team staff 
	/// </summary>
	public class EmpPrjAttr
	{
		#region  Basic properties 
		/// <summary>
		///  Staff ID
		/// </summary>
		public const  string FK_Emp="FK_Emp";
		/// <summary>
		///  Project Team 
		/// </summary>
		public const  string FK_Prj="FK_Prj";
        /// <summary>
        /// MyPK
        /// </summary>
        public const string MyPK = "MyPK";
        /// <summary>
        /// Name
        /// </summary>
        public const string Name = "Name";
		#endregion	
	}
	/// <summary>
    ///  Project team staff   The summary .
	/// </summary>
    public class EmpPrj : Entity
    {
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                uac.OpenForSysAdmin();
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
                return this.GetValStringByKey(EmpPrjAttr.FK_Emp);
            }
            set
            {
                SetValByKey(EmpPrjAttr.FK_Emp, value);
            }
        }
        public string FK_EmpT
        {
            get
            {
                return this.GetValRefTextByKey(EmpPrjAttr.FK_Emp);
            }
        }
        public string FK_PrjT
        {
            get
            {
                return this.GetValRefTextByKey(EmpPrjAttr.FK_Prj);
            }
        }
        /// <summary>
        /// Project Team 
        /// </summary>
        public string FK_Prj
        {
            get
            {
                return this.GetValStringByKey(EmpPrjAttr.FK_Prj);
            }
            set
            {
                SetValByKey(EmpPrjAttr.FK_Prj, value);
            }
        }
        public string MyPK
        {
            get
            {
                return this.GetValStringByKey(EmpPrjAttr.MyPK);
            }
            set
            {
                SetValByKey(EmpPrjAttr.MyPK, value);
            }
        }
        public string Name
        {
            get
            {
                return this.GetValStringByKey(EmpPrjAttr.Name);
            }
            set
            {
                SetValByKey(EmpPrjAttr.Name, value);
            }
        }
        #endregion

        #region  Extended Attributes 

        #endregion

        #region  Constructor 
        /// <summary>
        ///  Staff team 
        /// </summary> 
        public EmpPrj()
        {
        }
        /// <summary>
        ///  Counterpart staff the project team 
        /// </summary>
        /// <param name="_empoid"> Staff ID</param>
        /// <param name="wsNo"> Project group number </param> 	
        public EmpPrj(string _empoid, string wsNo)
        {
            this.FK_Emp = _empoid;
            this.FK_Prj = wsNo;
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

                Map map = new Map("Prj_EmpPrj");
                map.EnDesc = " Project team staff ";
                map.EnType = EnType.Dot2Dot;

                map.AddTBString(EmpPrjAttr.MyPK, null, "MyPK", true, true, 0, 20, 20);
                map.AddTBString(EmpPrjAttr.Name, null, "Name", true, true, 0, 3000, 20);

                map.AddDDLEntitiesPK(EmpPrjAttr.FK_Emp, null, " The operator ", new BP.WF.Port.WFEmps(), true);
                map.AddDDLEntitiesPK(EmpPrjAttr.FK_Prj, null, " Project Team ", new Prjs(), true);


                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        protected override bool beforeInsert()
        {
            BP.WF.Port.WFEmp emp = new WF.Port.WFEmp(this.FK_Emp);
            Prj p = new Prj(this.FK_Prj);

            this.MyPK = this.FK_Emp + "-" + this.FK_Prj;
            this.Name = p.Name + " - " + emp.Name;

            return base.beforeInsert();
        }
    }
	/// <summary>
    ///  Project team staff 
	/// </summary>
    public class EmpPrjs : Entities
    {
        #region  Structure 
        /// <summary>
        ///  Staff team 
        /// </summary>
        public EmpPrjs()
        {
        }
        /// <summary>
        ///  Staff and the project team set 
        /// </summary>
        public EmpPrjs(string GroupNo)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(EmpPrjAttr.FK_Prj, GroupNo);
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
                return new EmpPrj();
            }
        }
        #endregion
    }
}
