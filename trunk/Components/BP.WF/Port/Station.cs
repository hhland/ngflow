using System;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.WF.Port
{	 
	/// <summary>
	///  Post Properties 
	/// </summary>
    public class StationAttr : EntityNoNameAttr
    {
        /// <summary>
        ///  Class times 
        /// </summary>
        public const string StaGrade = "StaGrade";
    }
	/// <summary>
	///  Post 
	/// </summary>
    public class Station : EntityNoName
    {
        #region  Achieve the basic square method 
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
                map.CodeStruct = "2"; //  The maximum level is 7.

                map.AddTBStringPK(SimpleNoNameAttr.No, null, null, true, false, 2, 2, 2);
                map.AddTBString(SimpleNoNameAttr.Name, null, null, true, false, 2, 50, 250);
                map.AddDDLSysEnum(StationAttr.StaGrade, 0,
                    " Type ", true, false,
                    StationAttr.StaGrade, "@1= Senior post @2= Middle Gang @3= Execution Gang ");

           //     map.AddDDLSysEnum("StaNWB", 0," Post logo ", true, true);
              //  map.AddDDLSysEnum("StaNWB", 0, " Post logo ", true, true, "StaNWB", "@1= Internal Gang @2= External Gang ");


                //switch (BP.Sys.SystemConfig.SysNo)
                //{
                //    case BP.SysNoList.WF:
                //        map.AddDDLSysEnum(StationAttr.StaGrade, 0, " Type ", true, false, StationAttr.StaGrade, "@1= Headquarters @2= Area @3= Center ");
                //        break;
                //    default:
                //        break;
                //}

                // map.AddTBInt(DeptAttr.Grade, 0, " Class times ", true, true);
                //map.AddBoolean(DeptAttr.IsDtl, true, " Whether Details ", true, true);
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
