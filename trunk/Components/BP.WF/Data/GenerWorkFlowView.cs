using System;
using System.Data;
using BP.DA;
using BP.WF;
using BP.Port ;
using BP.Sys;
using BP.En;

namespace BP.WF.Data
{
	/// <summary>
    ///  Process instance 
	/// </summary>
    public class GenerWorkFlowViewAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  The work ID
        /// </summary>
        public const string WorkID = "WorkID";
        /// <summary>
        ///  Workflow 
        /// </summary>
        public const string FK_Flow = "FK_Flow";
        /// <summary>
        ///  Process Status 
        /// </summary>
        public const string WFState = "WFState";
        /// <summary>
        ///  Process Status ( Simple )
        /// </summary>
        public const string WFSta = "WFSta";
        /// <summary>
        ///  Title 
        /// </summary>
        public const string Title = "Title";
        /// <summary>
        ///  Sponsor 
        /// </summary>
        public const string Starter = "Starter";
        /// <summary>
        ///  Generation time 
        /// </summary>
        public const string RDT = "RDT";
        /// <summary>
        ///  Completion Time 
        /// </summary>
        public const string CDT = "CDT";
        /// <summary>
        ///  Score 
        /// </summary>
        public const string Cent = "Cent";
        /// <summary>
        ///  Work to the current node .
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        ///  Current jobs 
        /// </summary>
        public const string FK_Station = "FK_Station";
        /// <summary>
        ///  Department 
        /// </summary>
        public const string FK_Dept = "FK_Dept";
        /// <summary>
        ///  Process ID
        /// </summary>
        public const string FID = "FID";
        /// <summary>
        ///  Whether to enable 
        /// </summary>
        public const string IsEnable = "IsEnable";
        /// <summary>
        ///  Process Name 
        /// </summary>
        public const string FlowName = "FlowName";
        /// <summary>
        ///  Sponsor name 
        /// </summary>
        public const string StarterName = "StarterName";
        /// <summary>
        ///  Node Name 
        /// </summary>
        public const string NodeName = "NodeName";
        /// <summary>
        ///  Department name 
        /// </summary>
        public const string DeptName = "DeptName";
        /// <summary>
        ///  Process Category 
        /// </summary>
        public const string FK_FlowSort = "FK_FlowSort";
        /// <summary>
        ///  Priority 
        /// </summary>
        public const string PRI = "PRI";
        /// <summary>
        ///  The process should be completed by the time 
        /// </summary>
        public const string SDTOfFlow = "SDTOfFlow";
        /// <summary>
        ///  Node should finish time 
        /// </summary>
        public const string SDTOfNode = "SDTOfNode";
        /// <summary>
        ///  Parent process ID
        /// </summary>
        public const string PWorkID = "PWorkID";
        /// <summary>
        ///  Parent process ID 
        /// </summary>
        public const string PFlowNo = "PFlowNo";
        /// <summary>
        ///  Parent process node 
        /// </summary>
        public const string PNodeID = "PNodeID";
        /// <summary>
        ///  People calling sub-processes .
        /// </summary>
        public const string PEmp = "PEmp";
        /// <summary>
        ///  Customer Number ( For client-initiated processes effectively )
        /// </summary>
        public const string GuestNo = "GuestNo";
        /// <summary>
        ///  Customer Name 
        /// </summary>
        public const string GuestName = "GuestName";
        /// <summary>
        ///  Document Number 
        /// </summary>
        public const string BillNo = "BillNo";
        /// <summary>
        ///  Remark 
        /// </summary>
        public const string FlowNote = "FlowNote";
        /// <summary>
        ///  Upcoming staff 
        /// </summary>
        public const string TodoEmps = "TodoEmps";
        /// <summary>
        ///  Upcoming number of personnel 
        /// </summary>
        public const string TodoEmpsNum = "TodoEmpsNum";
        /// <summary>
        ///  Task Status 
        /// </summary>
        public const string TaskSta = "TaskSta";
        /// <summary>
        ///  Continuation of the process ID 
        /// </summary>
        public const string CFlowNo = "CFlowNo";
        /// <summary>
        ///  Continuation of the process ID
        /// </summary>
        public const string CWorkID = "CWorkID";
        /// <summary>
        ///  Temporary storage parameters 
        /// </summary>
        public const string AtPara = "AtPara";
        /// <summary>
        ///  Participants 
        /// </summary>
        public const string Emps = "Emps";
        /// <summary>
        /// GUID
        /// </summary>
        public const string GUID = "GUID";
        #endregion
    }
	/// <summary>
    ///  Process instance 
	/// </summary>
	public class GenerWorkFlowView : Entity
	{	
		#region  Basic properties 
        /// <summary>
        ///  Primary key 
        /// </summary>
        public override string PK
        {
            get
            {
                return GenerWorkFlowViewAttr.WorkID;
            }
        }
        /// <summary>
        ///  Remark 
        /// </summary>
        public string FlowNote
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowViewAttr.FlowNote);
            }
            set
            {
                SetValByKey(GenerWorkFlowViewAttr.FlowNote, value);
            }
        }
		/// <summary>
		///  Workflow number 
		/// </summary>
		public string  FK_Flow
		{
			get
			{
				return this.GetValStrByKey(GenerWorkFlowViewAttr.FK_Flow);
			}
			set
			{
				SetValByKey(GenerWorkFlowViewAttr.FK_Flow,value);
			}
		}
        /// <summary>
        /// BillNo
        /// </summary>
        public string BillNo
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowViewAttr.BillNo);
            }
            set
            {
                SetValByKey(GenerWorkFlowViewAttr.BillNo, value);
            }
        }
        /// <summary>
        ///  Process Name 
        /// </summary>
        public string FlowName
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowViewAttr.FlowName);
            }
            set
            {
                SetValByKey(GenerWorkFlowViewAttr.FlowName, value);
            }
        }
        /// <summary>
        ///  Priority 
        /// </summary>
        public int PRI
        {
            get
            {
                return this.GetValIntByKey(GenerWorkFlowViewAttr.PRI);
            }
            set
            {
                SetValByKey(GenerWorkFlowViewAttr.PRI, value);
            }
        }
        /// <summary>
        ///  Upcoming number of personnel 
        /// </summary>
        public int TodoEmpsNum
        {
            get
            {
                return this.GetValIntByKey(GenerWorkFlowViewAttr.TodoEmpsNum);
            }
            set
            {
                SetValByKey(GenerWorkFlowViewAttr.TodoEmpsNum, value);
            }
        }
        /// <summary>
        ///  To-do list of people 
        /// </summary>
        public string TodoEmps
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowViewAttr.TodoEmps);
            }
            set
            {
                SetValByKey(GenerWorkFlowViewAttr.TodoEmps, value);
            }
        }
        /// <summary>
        ///  Participants 
        /// </summary>
        public string Emps
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowViewAttr.Emps);
            }
            set
            {
                SetValByKey(GenerWorkFlowViewAttr.Emps, value);
            }
        }
        /// <summary>
        ///  Status 
        /// </summary>
        public TaskSta TaskSta
        {
            get
            {
                return (TaskSta)this.GetValIntByKey(GenerWorkFlowViewAttr.TaskSta);
            }
            set
            {
                SetValByKey(GenerWorkFlowViewAttr.TaskSta, (int)value);
            }
        }
        /// <summary>
        ///  Category Number 
        /// </summary>
        public string FK_FlowSort
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowViewAttr.FK_FlowSort);
            }
            set
            {
                SetValByKey(GenerWorkFlowViewAttr.FK_FlowSort, value);
            }
        }
        /// <summary>
        ///  Department number 
        /// </summary>
		public string  FK_Dept
		{
			get
			{
				return this.GetValStrByKey(GenerWorkFlowViewAttr.FK_Dept);
			}
			set
			{
				SetValByKey(GenerWorkFlowViewAttr.FK_Dept,value);
			}
		}
		/// <summary>
		///  Title 
		/// </summary>
		public string  Title
		{
			get
			{
				return this.GetValStrByKey(GenerWorkFlowViewAttr.Title);
			}
			set
			{
				SetValByKey(GenerWorkFlowViewAttr.Title,value);
			}
		}
        /// <summary>
        ///  Customer Number 
        /// </summary>
        public string GuestNo
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowViewAttr.GuestNo);
            }
            set
            {
                SetValByKey(GenerWorkFlowViewAttr.GuestNo, value);
            }
        }
        /// <summary>
        ///  Customer Name 
        /// </summary>
        public string GuestName
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowViewAttr.GuestName);
            }
            set
            {
                SetValByKey(GenerWorkFlowViewAttr.GuestName, value);
            }
        }
		/// <summary>
		///  Generation time 
		/// </summary>
		public string  RDT
		{
			get
			{
				return this.GetValStrByKey(GenerWorkFlowViewAttr.RDT);
			}
			set
			{
				SetValByKey(GenerWorkFlowViewAttr.RDT,value);
			}
		}
        /// <summary>
        ///  Node should finish time 
        /// </summary>
        public string SDTOfNode
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowViewAttr.SDTOfNode);
            }
            set
            {
                SetValByKey(GenerWorkFlowViewAttr.SDTOfNode, value);
            }
        }
        /// <summary>
        ///  The process should be completed by the time 
        /// </summary>
        public string SDTOfFlow
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowViewAttr.SDTOfFlow);
            }
            set
            {
                SetValByKey(GenerWorkFlowViewAttr.SDTOfFlow, value);
            }
        }
		/// <summary>
		///  Process ID
		/// </summary>
        public Int64 WorkID
		{
			get
			{
                return this.GetValInt64ByKey(GenerWorkFlowViewAttr.WorkID);
			}
			set
			{
				SetValByKey(GenerWorkFlowViewAttr.WorkID,value);
			}
		}
        /// <summary>
        ///  The main thread ID
        /// </summary>
        public Int64 FID
        {
            get
            {
                return this.GetValInt64ByKey(GenerWorkFlowViewAttr.FID);
            }
            set
            {
                SetValByKey(GenerWorkFlowViewAttr.FID, value);
            }
        }
        /// <summary>
        ///  Parent ID  For or -1.
        /// </summary>
        public Int64 CWorkID
        {
            get
            {
                return this.GetValInt64ByKey(GenerWorkFlowViewAttr.CWorkID);
            }
            set
            {
                SetValByKey(GenerWorkFlowViewAttr.CWorkID, value);
            }
        }
        /// <summary>
        /// PFlowNo
        /// </summary>
        public string CFlowNo
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowViewAttr.CFlowNo);
            }
            set
            {
                SetValByKey(GenerWorkFlowViewAttr.CFlowNo, value);
            }
        }
        /// <summary>
        ///  Parent process ID .
        /// </summary>
        public Int64 PWorkID
        {
            get
            {
                return this.GetValInt64ByKey(GenerWorkFlowViewAttr.PWorkID);
            }
            set
            {
                SetValByKey(GenerWorkFlowViewAttr.PWorkID, value);
            }
        }
        /// <summary>
        ///  Node parent process calls 
        /// </summary>
        public int PNodeID
        {
            get
            {
                return this.GetValIntByKey(GenerWorkFlowViewAttr.PNodeID);
            }
            set
            {
                SetValByKey(GenerWorkFlowViewAttr.PNodeID, value);
            }
        }
        /// <summary>
        /// PFlowNo
        /// </summary>
        public string PFlowNo
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowViewAttr.PFlowNo);
            }
            set
            {
                SetValByKey(GenerWorkFlowViewAttr.PFlowNo, value);
            }
        }
        /// <summary>
        ///  Lift the sub-processes of staff 
        /// </summary>
        public string PEmp
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowViewAttr.PEmp);
            }
            set
            {
                SetValByKey(GenerWorkFlowViewAttr.PEmp, value);
            }
        }
        /// <summary>
        ///  Sponsor 
        /// </summary>
        public string Starter
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowViewAttr.Starter);
            }
            set
            {
                SetValByKey(GenerWorkFlowViewAttr.Starter, value);
            }
        }
        /// <summary>
        ///  Sponsor name 
        /// </summary>
        public string StarterName
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowViewAttr.StarterName);
            }
            set
            {
                this.SetValByKey(GenerWorkFlowViewAttr.StarterName, value);
            }
        }
        /// <summary>
        ///  Sponsor department name 
        /// </summary>
        public string DeptName
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowViewAttr.DeptName);
            }
            set
            {
                this.SetValByKey(GenerWorkFlowViewAttr.DeptName, value);
            }
        }
        /// <summary>
        ///  The current node name 
        /// </summary>
        public string NodeName
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowViewAttr.NodeName);
            }
            set
            {
                this.SetValByKey(GenerWorkFlowViewAttr.NodeName, value);
            }
        }
		/// <summary>
		///  Work to the current node 
		/// </summary>
        public int FK_Node
        {
            get
            {
                return this.GetValIntByKey(GenerWorkFlowViewAttr.FK_Node);
            }
            set
            {
                SetValByKey(GenerWorkFlowViewAttr.FK_Node, value);
            }
        }
        /// <summary>
		///  Workflow status 
		/// </summary>
        public WFState WFState
        {
            get
            {
                return (WFState)this.GetValIntByKey(GenerWorkFlowViewAttr.WFState);
            }
            set
            {
                if (value == WF.WFState.Complete)
                    SetValByKey(GenerWorkFlowViewAttr.WFSta, (int)WFSta.Complete);
                else if (value == WF.WFState.Delete)
                    SetValByKey(GenerWorkFlowViewAttr.WFSta, (int)WFSta.Delete);
                else
                    SetValByKey(GenerWorkFlowViewAttr.WFSta, (int)WFSta.Runing);

                SetValByKey(GenerWorkFlowViewAttr.WFState, (int)value);
            }
        }
        /// <summary>
        ///  Status ( Simple )
        /// </summary>
        public WFSta WFSta
        {
            get
            {
                return (WFSta)this.GetValIntByKey(GenerWorkFlowViewAttr.WFSta);
            }
            set
            {
                SetValByKey(GenerWorkFlowViewAttr.WFSta, (int)value);
            }
        }
        public string WFStateText
        {
            get
            {
                BP.WF.WFState ws = (WFState)this.WFState;
                switch(ws)
                {
                    case WF.WFState.Complete:
                        return " Completed ";
                    case WF.WFState.Runing:
                        return " In the run ";
                    case WF.WFState.HungUp:
                        return " Pending ";
                    case WF.WFState.Askfor:
                        return " Plus sign ";
                    default:
                        return " No judgment ";
                }
            }
        }
        /// <summary>
        /// GUID
        /// </summary>
        public string GUID
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowViewAttr.GUID);
            }
            set
            {
                SetValByKey(GenerWorkFlowViewAttr.GUID, value);
            }
        }
		#endregion

      

        #region  Parameter Properties .
        public string Paras_ToNodes
        {

            get
            {
                return this.GetParaString("ToNodes");
            }

            set
            {
                this.SetPara("ToNodes", value);
            }
        }
        /// <summary>
        ///  Endorsement information 
        /// </summary>
        public string Paras_AskForReply
        {

            get
            {
                return this.GetParaString("AskForReply");
            }

            set
            {
                this.SetPara("AskForReply", value);
            }
        }
        #endregion  Parameter Properties .

        #region  Constructor 
        /// <summary>
		///  Generating workflow 
		/// </summary>
		public GenerWorkFlowView()
		{
		}
        public GenerWorkFlowView(Int64 workId)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(GenerWorkFlowViewAttr.WorkID, workId);
            if (qo.DoQuery() == 0)
                throw new Exception(" The work  GenerWorkFlowView [" + workId + "] Does not exist .");
        }
        /// <summary>
        ///  Perform repairs 
        /// </summary>
        public void DoRepair()
        { 
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

                Map map = new Map("WF_GenerWorkFlow");
                map.EnDesc = " Process Query ";
                map.AddTBIntPK(GenerWorkFlowViewAttr.WorkID, 0, "WorkID", true, true);
                map.AddTBInt(GenerWorkFlowViewAttr.FID, 0, "FID", false, false);

                map.AddDDLEntities(GenerWorkFlowViewAttr.FK_FlowSort, null, " Category ", new FlowSorts(), false);
                map.AddDDLEntities(GenerWorkFlowViewAttr.FK_Flow, null, " Process ", new Flows(), false);
                map.AddDDLEntities(GenerWorkFlowViewAttr.FK_Dept, null, " Department ", new BP.Port.Depts(), false);
                map.AddTBString(GenerWorkFlowViewAttr.StarterName, null, " Sponsor ", true, false, 0, 30, 10);
                map.AddTBString(GenerWorkFlowViewAttr.BillNo, null, " Document Number ", true, false, 0, 100, 10);
                map.AddTBString(GenerWorkFlowViewAttr.Title, null, " Title ", true, false, 0, 100, 10);
                map.AddDDLSysEnum(GenerWorkFlowViewAttr.WFSta, 0, " Process Status ", true, false, GenerWorkFlowViewAttr.WFSta, "@0= Run @1= Completed @2= Other ");
                map.AddTBString(GenerWorkFlowViewAttr.NodeName, null, " The current node name ", true, false, 0, 100, 10);
                map.AddTBDateTime(GenerWorkFlowViewAttr.RDT, " Record Date ", true, true);
                map.AddTBString(GenerWorkFlowViewAttr.FlowNote, null, " Remark ", true, false, 0, 4000, 10);
                map.AddTBMyNum();

                map.AddSearchAttr(GenerWorkFlowViewAttr.FK_Dept);
                map.AddSearchAttr(GenerWorkFlowViewAttr.FK_Flow);
                map.AddSearchAttr(GenerWorkFlowViewAttr.WFSta);

                RefMethod rm = new RefMethod();
                rm.Title = " Running Information ";  
                rm.ClassMethodName = this.ToString() + ".DoTrack";
                rm.Icon = "/WF/Img/FileType/doc.gif";
                map.AddRefMethod(rm);
              
                this._enMap = map;
                return this._enMap;
            }
        }
		#endregion 

		#region  Perform diagnostics 
        public string DoTrack()
        {
            PubClass.WinOpen("/WF/WFRpt.aspx?WorkID=" + this.WorkID + "&FID="+this.FID+"&FK_Flow="+this.FK_Flow,900,800);
            return null;
        }
		#endregion
	}
	/// <summary>
    ///  Process instance s
	/// </summary>
	public class GenerWorkFlowViews : Entities
	{
		/// <summary>
		///  According to the workflow , Staff  ID  Check out his current work to do .
		/// </summary>
		/// <param name="flowNo"> Process ID </param>
		/// <param name="empId"> Staff ID</param>
		/// <returns></returns>
		public static DataTable QuByFlowAndEmp(string flowNo, int empId)
		{
			string sql="SELECT a.WorkID FROM WF_GenerWorkFlowView a, WF_GenerWorkerlist b WHERE a.WorkID=b.WorkID   AND b.FK_Node=a.FK_Node  AND b.FK_Emp='"+empId.ToString()+"' AND a.FK_Flow='"+flowNo+"'";
			return DBAccess.RunSQLReturnTable(sql);
		}

		#region  Method 
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{			 
				return new GenerWorkFlowView();
			}
		}
		/// <summary>
		///  Examples of the collection process 
		/// </summary>
		public GenerWorkFlowViews(){}
		#endregion
	}
	
}
