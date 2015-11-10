using System;
using System.Data;
using BP.DA;
using BP.En;

namespace BP.GPM
{
	/// <summary>
	///  Corresponding department staff positions 
	/// </summary>
	public class DeptEmpStationAttr
	{
		#region  Basic properties 
		/// <summary>
		///  Department 
		/// </summary>
		public const  string FK_Dept="FK_Dept";
		/// <summary>
		///  Post 
		/// </summary>
		public const  string FK_Station="FK_Station";
        /// <summary>
        ///  Staff 
        /// </summary>
        public const string FK_Emp = "FK_Emp";
		#endregion	
	}
	/// <summary>
    ///  Corresponding department staff positions   The summary .
	/// </summary>
    public class DeptEmpStation : EntityMyPK
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
                return this.GetValStringByKey(DeptEmpStationAttr.FK_Emp);
            }
            set
            {
                SetValByKey(DeptEmpStationAttr.FK_Emp, value);
                this.MyPK = this.FK_Dept + "_" + this.FK_Emp+"_"+this.FK_Station;
            }
        }
        /// <summary>
        ///  Department 
        /// </summary>
        public string FK_Dept
        {
            get
            {
                return this.GetValStringByKey(DeptEmpStationAttr.FK_Dept);
            }
            set
            {
                SetValByKey(DeptEmpStationAttr.FK_Dept, value);
                this.MyPK = this.FK_Dept + "_" + this.FK_Emp + "_" + this.FK_Station;
            }
        }
        public string FK_StationT
        {
            get
            {
                return this.GetValRefTextByKey(DeptEmpStationAttr.FK_Station);
            }
        }
        /// <summary>
        /// Post 
        /// </summary>
        public string FK_Station
        {
            get
            {
                return this.GetValStringByKey(DeptEmpStationAttr.FK_Station);
            }
            set
            {
                SetValByKey(DeptEmpStationAttr.FK_Station, value);
                this.MyPK = this.FK_Dept + "_" + this.FK_Emp + "_" + this.FK_Station;
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Corresponding department staff positions 
        /// </summary> 
        public DeptEmpStation() { }
        /// <summary>
        ///  Override the base class methods 
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("Port_DeptEmpStation");
                map.EnDesc = " Corresponding department staff positions ";

                map.AddMyPK();

                map.AddTBString(DeptEmpStationAttr.FK_Dept, null, " Department ", false, false, 1, 50, 1);
                map.AddTBString(DeptEmpStationAttr.FK_Station, null, " Post ", false, false, 1, 50, 1);
                map.AddTBString(DeptEmpStationAttr.FK_Emp, null, " The operator ", false, false, 1, 50, 1);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        /// <summary>
        ///  Updates do before deleting 
        /// </summary>
        /// <returns></returns>
        protected override bool beforeUpdateInsertAction()
        {
            this.MyPK = this.FK_Dept + "_" + this.FK_Emp + "_" + this.FK_Station;
            return base.beforeUpdateInsertAction();
        }
    }
	/// <summary>
    ///  Corresponding department staff positions  
	/// </summary>
	public class DeptEmpStations : EntitiesMyPK
	{
		#region  Structure 
		/// <summary>
		///  Corresponding department staff positions 
		/// </summary>
		public DeptEmpStations()
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
				return new DeptEmpStation();
			}
		}	
		#endregion 
		
	}
}
