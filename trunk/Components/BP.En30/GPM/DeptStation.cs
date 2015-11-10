using System;
using System.Data;
using BP.DA;
using BP.En;

namespace BP.GPM
{
	/// <summary>
	///  Positions corresponding department 
	/// </summary>
	public class DeptStationAttr  
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
		#endregion	
	}
	/// <summary>
    ///  Positions corresponding department   The summary .
	/// </summary>
    public class DeptStation : Entity
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
                return this.GetValStringByKey(DeptStationAttr.FK_Dept);
            }
            set
            {
                SetValByKey(DeptStationAttr.FK_Dept, value);
            }
        }
        public string FK_StationT
        {
            get
            {
                return this.GetValRefTextByKey(DeptStationAttr.FK_Station);
            }
        }
        /// <summary>
        /// Post 
        /// </summary>
        public string FK_Station
        {
            get
            {
                return this.GetValStringByKey(DeptStationAttr.FK_Station);
            }
            set
            {
                SetValByKey(DeptStationAttr.FK_Station, value);
            }
        }
        #endregion

        #region  Extended Attributes 

        #endregion

        #region  Constructor 
        /// <summary>
        ///  Positions corresponding departments 
        /// </summary> 
        public DeptStation() { }
        /// <summary>
        ///  Staff positions corresponding 
        /// </summary>
        /// <param name="_empoid"> Department </param>
        /// <param name="wsNo"> Job numbers </param> 	
        public DeptStation(string _empoid, string wsNo)
        {
            this.FK_Dept = _empoid;
            this.FK_Station = wsNo;
            if (this.Retrieve(DeptStationAttr.FK_Dept, this.FK_Dept, DeptStationAttr.FK_Station, this.FK_Station) == 0)
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

                Map map = new Map("Port_DeptStation");
                map.EnDesc = " Positions corresponding department ";
                map.EnType = EnType.Dot2Dot; // Entity Type ,admin  System administrators table ,PowerAble  Rights management table , Also the user table , You want to add it to rights management inside your set here ..

                map.AddTBStringPK(DeptStationAttr.FK_Dept, null, " The operator ", false, false, 1, 15, 1);
                map.AddDDLEntitiesPK(DeptStationAttr.FK_Station, null, " Post ", new Stations(), true);
                map.AddSearchAttr(DeptStationAttr.FK_Station);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
	/// <summary>
    ///  Positions corresponding department  
	/// </summary>
	public class DeptStations : Entities
	{
		#region  Structure 
		/// <summary>
		///  Positions corresponding departments 
		/// </summary>
		public DeptStations()
		{
		}
		/// <summary>
		///  Staff and job set 
		/// </summary>
		public DeptStations(string stationNo)
		{
			QueryObject qo = new QueryObject(this);
			qo.AddWhere(DeptStationAttr.FK_Station, stationNo);
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
				return new DeptStation();
			}
		}	
		#endregion 

		#region  Query methods 
		/// <summary>
		///  Positions corresponding node 
		/// </summary>
		/// <param name="stationNo"> Job numbers </param>
		/// <returns> Node s</returns>
		public Emps GetHisEmps(string stationNo)
		{
			QueryObject qo = new QueryObject(this);
			qo.AddWhere(DeptStationAttr.FK_Station, stationNo );
			qo.addOrderBy(DeptStationAttr.FK_Station);
			qo.DoQuery();

			Emps ens = new Emps();
			foreach(DeptStation en in this)
				ens.AddEntity( new Emp(en.FK_Dept)) ;
			
			return ens;
		}
		/// <summary>
		///  Positions corresponding departments s
		/// </summary>
		/// <param name="empId">empId</param>
		/// <returns> Positions corresponding departments s</returns> 
		public Stations GetHisStations(string empId)
		{
			Stations ens = new Stations();
			if ( Cash.IsExits("DeptStationsOf"+empId, Depositary.Application))
			{
				return (Stations)Cash.GetObjFormApplication("DeptStationsOf"+empId,null );				 
			}
			else
			{
				QueryObject qo = new QueryObject(this);
				qo.AddWhere(DeptStationAttr.FK_Dept,empId);
				qo.addOrderBy(DeptStationAttr.FK_Station);
				qo.DoQuery();				
				foreach(DeptStation en in this)
					ens.AddEntity( new Station(en.FK_Station) ) ;
				Cash.AddObj("DeptStationsOf"+empId,Depositary.Application,ens);
				return ens;
			}
		}
		#endregion
	}
}
