using System;
using System.Data;
using BP.DA;
using BP.En;

namespace BP.GPM
{
	/// <summary>
	///  Sector posts 
	/// </summary>
	public class DeptDutyAttr  
	{
		#region  Basic properties 
		/// <summary>
		///  Department 
		/// </summary>
		public const  string FK_Dept="FK_Dept";
		/// <summary>
		///  Position 
		/// </summary>
		public const  string FK_Duty="FK_Duty";		 
		#endregion	
	}
	/// <summary>
    ///  Sector posts   The summary .
	/// </summary>
    public class DeptDuty : Entity
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
        ///  Department 
        /// </summary>
        public string FK_Dept
        {
            get
            {
                return this.GetValStringByKey(DeptDutyAttr.FK_Dept);
            }
            set
            {
                SetValByKey(DeptDutyAttr.FK_Dept, value);
            }
        }
        public string FK_DutyT
        {
            get
            {
                return this.GetValRefTextByKey(DeptDutyAttr.FK_Duty);
            }
        }
        /// <summary>
        /// Position 
        /// </summary>
        public string FK_Duty
        {
            get
            {
                return this.GetValStringByKey(DeptDutyAttr.FK_Duty);
            }
            set
            {
                SetValByKey(DeptDutyAttr.FK_Duty, value);
            }
        }
        #endregion

        #region  Extended Attributes 

        #endregion

        #region  Constructor 
        /// <summary>
        ///  Sector posts 
        /// </summary> 
        public DeptDuty() { }
        /// <summary>
        ///  Staff positions corresponding 
        /// </summary>
        /// <param name="_empoid"> Department </param>
        /// <param name="wsNo"> Job numbers </param> 	
        public DeptDuty(string _empoid, string wsNo)
        {
            this.FK_Dept = _empoid;
            this.FK_Duty = wsNo;
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

                Map map = new Map("Port_DeptDuty");
                map.EnDesc = " Sector posts ";
                map.EnType = EnType.Dot2Dot; // Entity Type ,admin  System administrators table ,PowerAble  Rights management table , Also the user table , You want to add it to rights management inside your set here ..

                map.AddTBStringPK(DeptDutyAttr.FK_Dept, null, " Department ", false, false, 1, 15, 1);
                map.AddDDLEntitiesPK(DeptDutyAttr.FK_Duty, null, " Position ", new Dutys(), true);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
	/// <summary>
    ///  Sector posts  
	/// </summary>
	public class DeptDutys : Entities
	{
		#region  Structure 
		/// <summary>
		///  Sector posts 
		/// </summary>
		public DeptDutys()
		{
		}
		/// <summary>
		///  Staff and duties set 
		/// </summary>
		public DeptDutys(string DutyNo)
		{
			QueryObject qo = new QueryObject(this);
			qo.AddWhere(DeptDutyAttr.FK_Duty, DutyNo);
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
				return new DeptDuty();
			}
		}	
		#endregion 

		#region  Query methods 
		#endregion
	}
}
