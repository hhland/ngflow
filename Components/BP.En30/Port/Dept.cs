using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Web;

namespace BP.Port
{
    /// <summary>
    ///  Department property 
    /// </summary>
    public class DeptAttr : EntityNoNameAttr
    {
        /// <summary>
        ///  Parent node number 
        /// </summary>
        public const string ParentNo = "ParentNo";
    }
    /// <summary>
    ///  Department 
    /// </summary>
    public class Dept : EntityNoName
    {
        #region  Property 
        /// <summary>
        ///  Parent node ID
        /// </summary>
        public string ParentNo
        {
            get
            {
                return this.GetValStrByKey(DeptAttr.ParentNo);
            }
            set
            {
                this.SetValByKey(DeptAttr.ParentNo, value);
            }
        }
        public int Grade
        {
            get
            {
                return 1;
            }
        }
        private Depts _HisSubDepts = null;
        /// <summary>
        ///  Its child nodes 
        /// </summary>
        public Depts HisSubDepts
        {
            get
            {
                if (_HisSubDepts == null)
                    _HisSubDepts = new Depts(this.No);
                return _HisSubDepts;
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Department 
        /// </summary>
        public Dept() { }
        /// <summary>
        ///  Department 
        /// </summary>
        /// <param name="no"> Serial number </param>
        public Dept(string no) : base(no) { }
        #endregion

        #region  Overriding methods 
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
        /// Map
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map();
                map.EnDBUrl = new DBUrl(DBUrlType.AppCenterDSN); // The database connected to the . ( The default is : AppCenterDSN )
                map.PhysicsTable = "Port_Dept";
                map.EnType = EnType.Admin;

                map.EnDesc = " Department "; //   Description of the entity .
                map.DepositaryOfEntity = Depositary.Application; // Entity map Storage location .
                map.DepositaryOfMap = Depositary.Application;    // Map  Storage location .

                map.AddTBStringPK(DeptAttr.No, null, " Serial number ", true, false, 1, 50, 20);
                map.AddTBString(DeptAttr.Name, null, " Name ", true, false, 0, 100, 30);
                map.AddTBString(DeptAttr.ParentNo, null, " Parent No", true, false, 0, 100, 30);

                #region  Point increase on multi-attribute 
                // His department permission 
                map.AttrsOfOneVSM.Add(new DeptStations(), new Stations(), DeptStationAttr.FK_Dept, DeptStationAttr.FK_Station, StationAttr.Name, StationAttr.No, " Permissions post ");
                #endregion 

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    /// Department s
    /// </summary>
    public class Depts : EntitiesNoName
    {
        /// <summary>
        ///  Get a new entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new Dept();
            }
        }
        /// <summary>
        ///  Collection department 
        /// </summary>
        public Depts()
        {

        }
        /// <summary>
        ///  Collection department 
        /// </summary>
        /// <param name="parentNo"> Father department No</param>
        public Depts(string parentNo)
        {
            this.Retrieve(DeptAttr.ParentNo, parentNo);
        }
    }
}
