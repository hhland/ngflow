using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.WF;
using BP.Port;
using BP.WF.Data;

namespace BP.WF
{
	/// <summary>
	///  Process delete the log 
	/// </summary>
	public class WorkFlowDeleteLogAttr 
	{
		#region  Basic properties 
		/// <summary>
		///  The work ID
		/// </summary>
		public const  string OID="OID";
        /// <summary>
        ///  Process ID 
        /// </summary>
        public const string FK_Flow = "FK_Flow";
        /// <summary>
        ///  Process Category 
        /// </summary>
        public const string FK_FlowSort = "FK_FlowSort";
		/// <summary>
		///  Delete staff 
		/// </summary>
		public const  string Oper="Oper";
		/// <summary>
		///  Reason for deletion 
		/// </summary>
		public const  string DeleteNote="DeleteNote";
        /// <summary>
        ///  Delete Date 
        /// </summary>
        public const string DeleteDT = "DeleteDT";
        /// <summary>
        ///  Delete staff 
        /// </summary>
        public const string OperDept = "OperDept";
        /// <summary>
        ///  Remove the person name 
        /// </summary>
        public const string OperDeptName = "OperDeptName";
        /// <summary>
        ///  Delete node 
        /// </summary>
        public const string DeleteNode = "DeleteNode";
        /// <summary>
        ///  Delete node name 
        /// </summary>
        public const string DeleteNodeName = "DeleteNodeName";        
        /// <summary>
        ///  The need to backtrack after deleting nodes ?
        /// </summary>
        public const string IsBackTracking = "IsBackTracking";
		#endregion

        #region  Process Attributes 
        /// <summary>
        ///  Title 
        /// </summary>
        public const string Title = "Title";
        /// <summary>
        ///  Participants 
        /// </summary>
        public const string FlowEmps = "FlowEmps";
        /// <summary>
        ///  Process ID
        /// </summary>
        public const string FID = "FID";
        /// <summary>
        ///  Launch date 
        /// </summary>
        public const string FK_NY = "FK_NY";
        /// <summary>
        ///  Sponsor ID
        /// </summary>
        public const string FlowStarter = "FlowStarter";
        /// <summary>
        ///  Launch date 
        /// </summary>
        public const string FlowStartDeleteDT = "FlowStartDeleteDT";
        /// <summary>
        ///  Sponsor department ID
        /// </summary>
        public const string FK_Dept = "FK_Dept";
        /// <summary>
        ///  Quantity 
        /// </summary>
        public const string MyNum = "MyNum";
        /// <summary>
        ///  End people 
        /// </summary>
        public const string FlowEnder = "FlowEnder";
        /// <summary>
        ///  Last Activity Date 
        /// </summary>
        public const string FlowEnderDeleteDT = "FlowEnderDeleteDT";
        /// <summary>
        ///  Span 
        /// </summary>
        public const string FlowDaySpan = "FlowDaySpan";
        /// <summary>
        ///  End node 
        /// </summary>
        public const string FlowEndNode = "FlowEndNode";
        /// <summary>
        ///  Parent process WorkID
        /// </summary>
        public const string PWorkID = "PWorkID";
        /// <summary>
        ///  Parent process ID 
        /// </summary>
        public const string PFlowNo = "PFlowNo";
        #endregion 
    }
	/// <summary>
	///  Process delete the log 
	/// </summary>
    public class WorkFlowDeleteLog : EntityOID
    {
        #region  Basic properties 
        /// <summary>
        ///  The work ID
        /// </summary>
        public Int64 OID
        {
            get
            {
                return this.GetValInt64ByKey(WorkFlowDeleteLogAttr.OID);
            }
            set
            {
                SetValByKey(WorkFlowDeleteLogAttr.OID, value);
            }
        }
        /// <summary>
        ///  Operator 
        /// </summary>
        public string Oper
        {
            get
            {
                return this.GetValStringByKey(WorkFlowDeleteLogAttr.Oper);
            }
            set
            {
                SetValByKey(WorkFlowDeleteLogAttr.Oper, value);
            }
        }
        /// <summary>
        ///  Delete staff 
        /// </summary>
        public string OperDept
        {
            get
            {
                return this.GetValStringByKey(WorkFlowDeleteLogAttr.OperDept);
            }
            set
            {
                SetValByKey(WorkFlowDeleteLogAttr.OperDept, value);
            }
        }
        public string OperDeptName
        {
            get
            {
                return this.GetValStringByKey(WorkFlowDeleteLogAttr.OperDeptName);
            }
            set
            {
                SetValByKey(WorkFlowDeleteLogAttr.OperDeptName, value);
            }
        }
        public string DeleteNote
        {
            get
            {
                return this.GetValStringByKey(WorkFlowDeleteLogAttr.DeleteNote);
            }
            set
            {
                SetValByKey(WorkFlowDeleteLogAttr.DeleteNote, value);
            }
        }
        public string DeleteNoteHtml
        {
            get
            {
                return this.GetValHtmlStringByKey(WorkFlowDeleteLogAttr.DeleteNote);
            }
        }
        /// <summary>
        ///  Record Date 
        /// </summary>
        public string DeleteDT
        {
            get
            {
                return this.GetValStringByKey(WorkFlowDeleteLogAttr.DeleteDT);
            }
            set
            {
                SetValByKey(WorkFlowDeleteLogAttr.DeleteDT, value);
            }
        }
        /// <summary>
        ///  Process ID 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.GetValStringByKey(WorkFlowDeleteLogAttr.FK_Flow);
            }
            set
            {
                SetValByKey(WorkFlowDeleteLogAttr.FK_Flow, value);
            }
        }
        /// <summary>
        ///  Process Category 
        /// </summary>
        public string FK_FlowSort
        {
            get
            {
                return this.GetValStringByKey(WorkFlowDeleteLogAttr.FK_FlowSort);
            }
            set
            {
                SetValByKey(WorkFlowDeleteLogAttr.FK_FlowSort, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Process delete the log 
        /// </summary>
        public WorkFlowDeleteLog() { }
        /// <summary>
        ///  Override the base class methods 
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("WF_WorkFlowDeleteLog");
                map.EnDesc = " Process delete the log ";
                map.EnType = EnType.App;

                //  Basic data flow .
                map.AddTBIntPKOID(FlowDataAttr.OID, "WorkID");
                map.AddTBInt(FlowDataAttr.FID, 0, "FID", false, false);
                map.AddDDLEntities(FlowDataAttr.FK_Dept, null, " Department ", new Port.Depts(), false);
                map.AddTBString(FlowDataAttr.Title, null, " Title ", true, true, 0, 100, 100);
                map.AddTBString(FlowDataAttr.FlowStarter, null, " Sponsor ", true, true, 0, 100, 100);
                map.AddTBDateTime(FlowDataAttr.FlowStartRDT, null, " Launch date ", true, true);
                map.AddDDLSysEnum(FlowDataAttr.WFState, 0, " Process Status ", true, true, "WFStateApp");
                map.AddDDLEntities(FlowDataAttr.FK_NY, null, " Years ", new BP.Pub.NYs(), false);
                map.AddDDLEntities(FlowDataAttr.FK_Flow, null, " Process ", new Flows(), false);
                map.AddTBDateTime(FlowDataAttr.FlowEnderRDT, null, " End Date ", true, true);
                map.AddTBInt(FlowDataAttr.FlowEndNode, 0, " End node ", true, true);
                map.AddTBInt(FlowDataAttr.FlowDaySpan, 0, " Span (Ìì)", true, true);
                map.AddTBInt(FlowDataAttr.MyNum, 1, " The number of ", true, true);
                map.AddTBString(FlowDataAttr.FlowEmps, null, " Participants ", false, false, 0, 100, 100);


                // Removal Information .
                map.AddTBString(WorkFlowDeleteLogAttr.Oper, null, " Delete staff ", true, true, 0, 20, 10);
                map.AddTBString(WorkFlowDeleteLogAttr.OperDept, null, " Delete personnel department ", true, true, 0, 20, 10);
                map.AddTBString(WorkFlowDeleteLogAttr.OperDeptName, null, " Remove the person name ", true, true, 0, 200, 10);
                map.AddTBString(WorkFlowDeleteLogAttr.DeleteNote, "", " Reason for deletion ", true, true, 0, 4000, 10);
                map.AddTBDateTime(WorkFlowDeleteLogAttr.DeleteDT, null, " Delete Date ", true, true);


                // Inquiry .
                map.AddSearchAttr(FlowDataAttr.FK_Dept);
                map.AddSearchAttr(FlowDataAttr.FK_NY);
                map.AddSearchAttr(FlowDataAttr.WFState);
                map.AddSearchAttr(FlowDataAttr.FK_Flow);

               // map.AddHidden(FlowDataAttr.FlowEmps, " LIKE ", "'%@@WebUser.No%'");

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
	/// <summary>
	///  Process delete the log s 
	/// </summary>
	public class WorkFlowDeleteLogs : Entities
	{	 
		#region  Structure 
		/// <summary>
		///  Process delete the log s
		/// </summary>
		public WorkFlowDeleteLogs()
		{
		}
		/// <summary>
		///  Get it  Entity
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new WorkFlowDeleteLog();
			}
		}
		#endregion
	}
	
}
