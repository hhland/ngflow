using System;
using System.Collections.Generic;
using System.Text;
using BP.En;
using BP.Sys;

namespace BP.WF.Data
{
    /// <summary>
    ///   Property  
    /// </summary>
    public class FlowDataAttr
    {
        /// <summary>
        ///  Title 
        /// </summary>
        public const string Title = "Title";
        /// <summary>
        ///  Process ID 
        /// </summary>
        public const string FK_Flow = "FK_Flow";
        /// <summary>
        ///  Participating members 
        /// </summary>
        public const string FlowEmps = "FlowEmps";
        /// <summary>
        ///  Process ID
        /// </summary>
        public const string FID = "FID";
        /// <summary>
        ///  The work ID
        /// </summary>
        public const string OID = "OID";
        /// <summary>
        ///  Years 
        /// </summary>
        public const string FK_NY = "FK_NY";
        /// <summary>
        ///  Process sponsor 
        /// </summary>
        public const string FlowStarter = "FlowStarter";
        /// <summary>
        ///  Process initiated by date 
        /// </summary>
        public const string FlowStartRDT = "FlowStartRDT";
        /// <summary>
        ///  Department initiated department 
        /// </summary>
        public const string FK_Dept = "FK_Dept";
        /// <summary>
        ///  Process number 
        /// </summary>
        public const string WFState = "WFState";
        /// <summary>
        ///  The number of ( Statistical analysis )
        /// </summary>
        public const string MyNum = "MyNum";
        /// <summary>
        ///  End people 
        /// </summary>
        public const string FlowEnder = "FlowEnder";
        /// <summary>
        ///  Process End Date 
        /// </summary>
        public const string FlowEnderRDT = "FlowEnderRDT";
        /// <summary>
        ///  Span 
        /// </summary>
        public const string FlowDaySpan = "FlowDaySpan";
        /// <summary>
        ///  Process end node 
        /// </summary>
        public const string FlowEndNode = "FlowEndNode";
    }
    /// <summary>
    ///  Report form 
    /// </summary>
    public class FlowData : BP.En.EntityOID
    {
        #region attrs
        /// <summary>
        ///  Process sponsor 
        /// </summary>
        public string FlowStarter
        {
            get
            {
                return this.GetValStringByKey(FlowDataAttr.FlowStarter);
            }
            set
            {
                this.SetValByKey(FlowDataAttr.FlowStarter, value);
            }
        }
        public string FlowStartRDT
        {
            get
            {
                return this.GetValStringByKey(FlowDataAttr.FlowStartRDT);
            }
            set
            {
                this.SetValByKey(FlowDataAttr.FlowStartRDT, value);
            }
        }
        /// <summary>
        ///  End of the process by 
        /// </summary>
        public string FlowEnder
        {
            get
            {
                return this.GetValStringByKey(FlowDataAttr.FlowEnder);
            }
            set
            {
                this.SetValByKey(FlowDataAttr.FlowEnder, value);
            }
        }
        /// <summary>
        ///  Process End Time 
        /// </summary>
        public string FlowEnderRDT
        {
            get
            {
                return this.GetValStringByKey(FlowDataAttr.FlowEnderRDT);
            }
            set
            {
                this.SetValByKey(FlowDataAttr.FlowEnderRDT, value);
            }
        }
        public string FK_Dept
        {
            get
            {
                return this.GetValStringByKey(FlowDataAttr.FK_Dept);
            }
            set
            {
                this.SetValByKey(FlowDataAttr.FK_Dept, value);
            }
        }
        public int WFState
        {
            get
            {
                return this.GetValIntByKey(FlowDataAttr.WFState);
            }
            set
            {
                this.SetValByKey(FlowDataAttr.WFState, value);
            }
        }
        public string FK_Flow
        {
            get
            {
                return this.GetValStringByKey(FlowDataAttr.FK_Flow);
            }
            set
            {
                this.SetValByKey(FlowDataAttr.FK_Flow, value);
            }
        }
        #endregion attrs

        /// <summary>
        /// UI Access control interface 
        /// </summary>
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                uac.Readonly();
                return uac;
            }
        }

        #region attrs - attrs
        public string RptName = null;
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("V_FlowData");
                map.EnDesc = " Process data ";
                map.EnType = EnType.Admin;

                map.AddTBIntPKOID(FlowDataAttr.OID, "WorkID");
                map.AddTBInt(FlowDataAttr.FID, 0, "FID", false, false);

                map.AddDDLEntities(FlowDataAttr.FK_Dept, null, " Department ", new Port.Depts(), false);
                map.AddTBString(FlowDataAttr.Title, null, " Title ", true, true, 0, 100, 100, true);
                map.AddDDLEntities(FlowDataAttr.FlowStarter, null, " Sponsor ", new Port.Emps(), false);
                map.AddTBDateTime(FlowDataAttr.FlowStartRDT, null, " Start Time ", true, true);
                map.AddDDLSysEnum(FlowDataAttr.WFState, 0, " Process Status ", true, true, "WFStateApp");
                //map.AddTBString(FlowDataAttr.FK_NY, null, " Years ", true, true, 0, 100, 100, false);
                //map.AddDDLEntities(FlowDataAttr.FK_NY, null, " Years ", new BP.Pub.NYs(), false);
                map.AddDDLEntities(FlowDataAttr.FK_Flow, null, " Process ", new Flows(), false);
                map.AddTBDateTime(FlowDataAttr.FlowEnderRDT, null, " End Time ", true, true);
                map.AddDDLEntities(FlowDataAttr.FlowEndNode, null, BP.DA.DataType.AppString, " End node ", new BP.WF.Nodes(), "NodeID", "Name", false);
                map.AddTBInt(FlowDataAttr.FlowDaySpan, 0, " Span (days)", true, true);
                map.AddTBInt(FlowDataAttr.MyNum, 1, " The number of ", true, true);
                map.AddTBString(FlowDataAttr.FlowEmps, null, " Participants ", false, false, 0, 100, 100);

                //map.AddSearchAttr(FlowDataAttr.FK_NY);
                map.AddSearchAttr(FlowDataAttr.WFState);
                map.AddSearchAttr(FlowDataAttr.FK_Flow);

                map.AddHidden(FlowDataAttr.FlowEmps, " LIKE ", "'%@@WebUser.No%'");

                RefMethod rm = new RefMethod();
                rm.Title = " Processes running track ";
                rm.ClassMethodName = this.ToString() + ".DoOpen";
                map.AddRefMethod(rm);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion attrs

        public string DoOpen()
        {
            PubClass.WinOpen(Glo.CCFlowAppPath + "WF/WorkOpt/OneWork/Track.aspx?WorkID=" + this.OID + "&FK_Flow=" + this.FK_Flow, 900, 600);
            return null;
        }
    }
    /// <summary>
    ///  Collection of reports 
    /// </summary>
    public class FlowDatas : BP.En.EntitiesOID
    {
        /// <summary>
        ///  Collection of reports 
        /// </summary>
        public FlowDatas()
        {
        }

        public override Entity GetNewEntity
        {
            get
            {
                return new FlowData();
            }
        }
    }
}
