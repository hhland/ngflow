using System;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.Port
{
    /// <summary>
    ///  Post Properties 
    /// </summary>
    public class StationAttr : EntityNoNameAttr
    {
        public const string StaGrade = "StaGrade";
    }
    /// <summary>
    ///  Post 
    /// </summary>
    public class Station : EntityNoName
    {
        #region  Achieve the basic method 
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                uac.OpenForSysAdmin();
                return uac;
            }
        }
        public new string Name
        {
            get
            {
                return this.GetValStrByKey("Name");
            }
        }
        public int Grade
        {
            get
            {
                return this.No.Length / 2;
            }
        }
        public int StaGrade
        {
            get
            {
                return this.GetValIntByKey(StationAttr.StaGrade);
            }
            set
            {
                this.SetValByKey(StationAttr.StaGrade,value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Post 
        /// </summary> 
        public Station()
        {
        }
        /// <summary>
        ///  Post 
        /// </summary>
        /// <param name="_No"></param>
        public Station(string _No) : base(_No) { }
        /// <summary>
        /// EnMap
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("Port_Station");
                map.EnDesc = " Post "; // " Post ";
                map.EnType = EnType.Admin;
                map.DepositaryOfMap = Depositary.Application;
                map.DepositaryOfEntity = Depositary.Application;
                map.CodeStruct = "2222222"; //  The maximum level is  7 .

                map.AddTBStringPK(EmpAttr.No, null, " Serial number ", true, false, 1, 20, 100);
                map.AddTBString(EmpAttr.Name, null, " Name ", true, false, 0, 100, 100);
                map.AddDDLSysEnum(StationAttr.StaGrade, 0, " Type ", true, true, StationAttr.StaGrade,
                    "@1= Senior post @2= Middle Gang @3= Execution Gang ");

                map.AttrsOfOneVSM.Add(new EmpStations(), new Emps(), EmpStationAttr.FK_Station, EmpStationAttr.FK_Emp,
                  DeptAttr.Name, DeptAttr.No, " Staff ");

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    ///  Post s
    /// </summary>
    public class Stations : EntitiesNoName
    {
        /// <summary>
        ///  Post 
        /// </summary>
        public Stations() { }
        /// <summary>
        ///  Get it  Entity
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new Station();
            }
        }
    }
}
