using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Port;

namespace BP.WF.Rpt
{
    /// <summary>
    ///  Reports posts 
    /// </summary>
    public class RptStationAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  Report form ID
        /// </summary>
        public const string FK_Rpt = "FK_Rpt";
        /// <summary>
        ///  Post 
        /// </summary>
        public const string FK_Station = "FK_Station";
        #endregion
    }
    /// <summary>
    /// RptStation  The summary .
    /// </summary>
    public class RptStation : Entity
    {

        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                if (BP.Web.WebUser.No == "admin")
                {
                    uac.IsView = true;
                    uac.IsDelete = true;
                    uac.IsInsert = true;
                    uac.IsUpdate = true;
                    uac.IsAdjunct = true;
                }
                return uac;
            }
        }

        #region  Basic properties 
        /// <summary>
        ///  Report form ID
        /// </summary>
        public string FK_Rpt
        {
            get
            {
                return this.GetValStringByKey(RptStationAttr.FK_Rpt);
            }
            set
            {
                SetValByKey(RptStationAttr.FK_Rpt, value);
            }
        }
        public string FK_StationT
        {
            get
            {
                return this.GetValRefTextByKey(RptStationAttr.FK_Station);
            }
        }
        /// <summary>
        /// Post 
        /// </summary>
        public string FK_Station
        {
            get
            {
                return this.GetValStringByKey(RptStationAttr.FK_Station);
            }
            set
            {
                SetValByKey(RptStationAttr.FK_Station, value);
            }
        }
        #endregion

        #region  Extended Attributes 

        #endregion

        #region  Constructor 
        /// <summary>
        ///  Reports posts 
        /// </summary> 
        public RptStation() { }
        /// <summary>
        ///  Reports corresponding positions 
        /// </summary>
        /// <param name="_empoid"> Report form ID</param>
        /// <param name="wsNo"> Job numbers </param> 	
        public RptStation(string _empoid, string wsNo)
        {
            this.FK_Rpt = _empoid;
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

                Map map = new Map("Sys_RptStation");
                map.EnDesc = " Reports corresponding job information ";
                map.EnType = EnType.Dot2Dot;

                map.AddTBStringPK(RptStationAttr.FK_Rpt, null, " Report form ", false, false, 1, 15, 1);
                map.AddDDLEntitiesPK(RptStationAttr.FK_Station, null, " Post ", new Stations(), true);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    ///  Reports posts  
    /// </summary>
    public class RptStations : Entities
    {
        #region  Structure 
        /// <summary>
        ///  Statements and positions set 
        /// </summary>
        public RptStations() { }
        #endregion

        #region  Method 
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new RptStation();
            }
        }
        #endregion
    }
}
