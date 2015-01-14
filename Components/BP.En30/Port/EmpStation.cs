using System;
using System.Data;
using BP.DA;
using BP.En;

namespace BP.Port
{
	/// <summary>
	///  Staff positions 
	/// </summary>
	public class EmpStationAttr  
	{
		#region  Basic properties 
		/// <summary>
		///  Staff ID
		/// </summary>
		public const  string FK_Emp="FK_Emp";
		/// <summary>
		///  Jobs 
		/// </summary>
		public const  string FK_Station="FK_Station";		 
		#endregion	
	}
	/// <summary>
    ///  Staff positions   The summary .
	/// </summary>
    public class EmpStation : Entity
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
        ///  Staff ID
        /// </summary>
        public string FK_Emp
        {
            get
            {
                return this.GetValStringByKey(EmpStationAttr.FK_Emp);
            }
            set
            {
                SetValByKey(EmpStationAttr.FK_Emp, value);
            }
        }
        public string FK_StationT
        {
            get
            {
                return this.GetValRefTextByKey(EmpStationAttr.FK_Station);
            }
        }
        /// <summary>
        /// Jobs 
        /// </summary>
        public string FK_Station
        {
            get
            {
                return this.GetValStringByKey(EmpStationAttr.FK_Station);
            }
            set
            {
                SetValByKey(EmpStationAttr.FK_Station, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Staff positions 
        /// </summary> 
        public EmpStation() { }
        /// <summary>
        ///  Staff jobs corresponding 
        /// </summary>
        /// <param name="fk_emp"> Staff ID</param>
        /// <param name="fk_station"> Number of jobs </param> 	
        public EmpStation(string fk_emp, string fk_station)
        {
            this.FK_Emp = fk_emp;
            this.FK_Station = fk_station;
            this.Retrieve();
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

                Map map = new Map("Port_EmpStation");
                map.EnDesc = " Staff positions ";
                map.EnType = EnType.Dot2Dot;

                map.AddDDLEntitiesPK(EmpStationAttr.FK_Emp, null, " The operator ", new Emps(), true);
                map.AddDDLEntitiesPK(EmpStationAttr.FK_Station, null, " Jobs ", new Stations(), true);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
	/// <summary>
    ///  Staff positions  
	/// </summary>
	public class EmpStations : Entities
	{
		#region  Structure 
		/// <summary>
		///  Staff positions 
		/// </summary>
		public EmpStations()
		{
		}
		/// <summary>
		///  Staff work with collections 
		/// </summary>
		public EmpStations(string stationNo)
		{
			QueryObject qo = new QueryObject(this);
			qo.AddWhere(EmpStationAttr.FK_Station, stationNo);
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
				return new EmpStation();
			}
		}	
		#endregion 

		#region  Query methods 
		/// <summary>
		///  Jobs corresponding node 
		/// </summary>
		/// <param name="stationNo"> Number of jobs </param>
		/// <returns> Node s</returns>
		public Emps GetHisEmps(string stationNo)
		{
			QueryObject qo = new QueryObject(this);
			qo.AddWhere(EmpStationAttr.FK_Station, stationNo );
			qo.addOrderBy(EmpStationAttr.FK_Station);
			qo.DoQuery();

			Emps ens = new Emps();
			foreach(EmpStation en in this)
				ens.AddEntity( new Emp(en.FK_Emp)) ;
			
			return ens;
		}
		/// <summary>
		///  Staff positions s
		/// </summary>
		/// <param name="empId">empId</param>
		/// <returns> Staff positions s</returns> 
		public Stations GetHisStations(string empId)
		{
			Stations ens = new Stations();
			if ( Cash.IsExits("EmpStationsOf"+empId, Depositary.Application))
			{
				return (Stations)Cash.GetObjFormApplication("EmpStationsOf"+empId,null );				 
			}
			else
			{
				QueryObject qo = new QueryObject(this);
				qo.AddWhere(EmpStationAttr.FK_Emp,empId);
				qo.addOrderBy(EmpStationAttr.FK_Station);
				qo.DoQuery();				
				foreach(EmpStation en in this)
					ens.AddEntity( new Station(en.FK_Station) ) ;
				Cash.AddObj("EmpStationsOf"+empId,Depositary.Application,ens);
				return ens;
			}
		}
		#endregion
	}
}
