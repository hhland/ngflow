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
	public class EmpPrjStationAttr
	{
		#region  Basic properties 
		/// <summary>
		///  Project staff 
		/// </summary>
        public const string FK_EmpPrj = "FK_EmpPrj";
		/// <summary>
		///  Project Team 
		/// </summary>
		public const  string FK_Station="FK_Station";
        /// <summary>
        /// FK_Emp
        /// </summary>
        public const string FK_Emp = "FK_Emp";
        /// <summary>
        /// FK_Prj
        /// </summary>
        public const string FK_Prj = "FK_Prj";
		#endregion
	}
	/// <summary>
    ///  Project team staff   The summary .
	/// </summary>
    public class EmpPrjStation : Entity
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
        public string FK_Prj
        {
            get
            {
                return this.GetValStringByKey(EmpPrjStationAttr.FK_Prj);
            }
            set
            {
                SetValByKey(EmpPrjStationAttr.FK_Prj, value);
            }
        }
        public string FK_Emp
        {
            get
            {
                return this.GetValStringByKey(EmpPrjStationAttr.FK_Emp);
            }
            set
            {
                SetValByKey(EmpPrjStationAttr.FK_Emp, value);
            }
        }
        /// <summary>
        ///  Project staff 
        /// </summary>
        public string FK_EmpPrj
        {
            get
            {
                return this.GetValStringByKey(EmpPrjStationAttr.FK_EmpPrj);
            }
            set
            {
                SetValByKey(EmpPrjStationAttr.FK_EmpPrj, value);
            }
        }
        public string FK_StationT
        {
            get
            {
                return this.GetValRefTextByKey(EmpPrjStationAttr.FK_Station);
            }
        }
        /// <summary>
        /// Project Team 
        /// </summary>
        public string FK_Station
        {
            get
            {
                return this.GetValStringByKey(EmpPrjStationAttr.FK_Station);
            }
            set
            {
                SetValByKey(EmpPrjStationAttr.FK_Station, value);
            }
        }
        #endregion

        #region  Extended Attributes 
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Staff team 
        /// </summary> 
        public EmpPrjStation() { }
        /// <summary>
        ///  Counterpart staff the project team 
        /// </summary>
        /// <param name="_empoid"> Project staff </param>
        /// <param name="wsNo"> Project group number </param> 	
        public EmpPrjStation(string _empoid, string wsNo)
        {
            this.FK_EmpPrj = _empoid;
            this.FK_Station = wsNo;
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

                Map map = new Map("Prj_EmpPrjStation");
                map.EnDesc = " Project team staff ";
                map.EnType = EnType.Dot2Dot;
                map.AddTBStringPK(EmpPrjStationAttr.FK_EmpPrj, null, "FK_EmpPrj", true, true, 0, 20, 20);
                map.AddDDLEntitiesPK(EmpPrjStationAttr.FK_Station, null, " Post ", new Stations(), true);
                map.AddTBString(EmpPrjStationAttr.FK_Emp, null, "FK_Emp", true, true, 0, 20, 20);
                map.AddTBString(EmpPrjStationAttr.FK_Prj, null, "FK_Prj", true, true, 0, 20, 20);
                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
        protected override bool beforeInsert()
        {
            string[] strs = this.FK_EmpPrj.Split('-');
            this.FK_Prj = strs[1];
            this.FK_Emp = strs[0];
            return base.beforeInsert();
        }
    }
	/// <summary>
    ///  Project team staff 
	/// </summary>
    public class EmpPrjStations : Entities
    {
        #region  Structure 
        /// <summary>
        ///  Staff team 
        /// </summary>
        public EmpPrjStations()
        {
        }
        /// <summary>
        ///  Staff and the project team set 
        /// </summary>
        public EmpPrjStations(string GroupNo)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(EmpPrjStationAttr.FK_Station, GroupNo);
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
                return new EmpPrjStation();
            }
        }
        #endregion
    }
}
