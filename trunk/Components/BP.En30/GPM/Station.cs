using System;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.GPM
{	 
	/// <summary>
	///  Post Properties 
	/// </summary>
    public class StationAttr : EntityNoNameAttr
    {
        /// <summary>
        ///  Post Type 
        /// </summary>
        public const string FK_StationType = "FK_StationType";
        /// <summary>
        ///  Quality Requirements 
        /// </summary>
        public const string Makings = "Makings";
        /// <summary>
        ///  Call of duty 
        /// </summary>
        public const string DutyReq = "DutyReq";
    }
	/// <summary>
	///  Post 
	/// </summary>
    public class Station : EntityNoName
    {
        #region  Property 
        public string FK_StationType
        {
            get
            {
                return this.GetValStrByKey(StationAttr.FK_StationType);
            }
            set
            {
                this.SetValByKey(StationAttr.FK_StationType, value);
            }
        }
        #endregion

        #region  Achieve the basic square method 
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                uac.OpenForSysAdmin();
                return uac;
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

                map.AddTBStringDoc(StationAttr.DutyReq, null, " Call of duty ", true, false);
                map.AddTBStringDoc(StationAttr.Makings, null, " Quality Requirements ", true, false);

                map.AddDDLEntities(StationAttr.FK_StationType, null, " Type ", new StationTypes(), true);


                map.AddSearchAttr(StationAttr.FK_StationType);
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
				return new BP.GPM.Station();
			}
		}
	}
}
