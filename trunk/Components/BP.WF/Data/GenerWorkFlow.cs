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
    public class GenerWorkFlowAttr
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
        #endregion
    }
	/// <summary>
    ///  Process instance 
	/// </summary>
	public class GenerWorkFlow : Entity
	{	
		#region  Basic properties 
        /// <summary>
        ///  Primary key 
        /// </summary>
        public override string PK
        {
            get
            {
                return GenerWorkFlowAttr.WorkID;
            }
        }
        /// <summary>
        ///  Remark 
        /// </summary>
        public string FlowNote
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowAttr.FlowNote);
            }
            set
            {
                SetValByKey(GenerWorkFlowAttr.FlowNote, value);
            }
        }
		/// <summary>
		///  Workflow number 
		/// </summary>
		public string  FK_Flow
		{
			get
			{
				return this.GetValStrByKey(GenerWorkFlowAttr.FK_Flow);
			}
			set
			{
				SetValByKey(GenerWorkFlowAttr.FK_Flow,value);
			}
		}
        /// <summary>
        /// BillNo
        /// </summary>
        public string BillNo
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowAttr.BillNo);
            }
            set
            {
                SetValByKey(GenerWorkFlowAttr.BillNo, value);
            }
        }
        /// <summary>
        ///  Process Name 
        /// </summary>
        public string FlowName
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowAttr.FlowName);
            }
            set
            {
                SetValByKey(GenerWorkFlowAttr.FlowName, value);
            }
        }
       
        /// <summary>
        ///  Priority 
        /// </summary>
        public int PRI
        {
            get
            {
                return this.GetValIntByKey(GenerWorkFlowAttr.PRI);
            }
            set
            {
                SetValByKey(GenerWorkFlowAttr.PRI, value);
            }
        }
        /// <summary>
        ///  Upcoming number of personnel 
        /// </summary>
        public int TodoEmpsNum
        {
            get
            {
                return this.GetValIntByKey(GenerWorkFlowAttr.TodoEmpsNum);
            }
            set
            {
                SetValByKey(GenerWorkFlowAttr.TodoEmpsNum, value);
            }
        }
        /// <summary>
        ///  To-do list of people 
        /// </summary>
        public string TodoEmps
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowAttr.TodoEmps);
            }
            set
            {
                SetValByKey(GenerWorkFlowAttr.TodoEmps, value);
            }
        }
        /// <summary>
        ///  Status 
        /// </summary>
        public TaskSta TaskSta
        {
            get
            {
                return (TaskSta)this.GetValIntByKey(GenerWorkFlowAttr.TaskSta);
            }
            set
            {
                SetValByKey(GenerWorkFlowAttr.TaskSta, (int)value);
            }
        }
        /// <summary>
        ///  Category Number 
        /// </summary>
        public string FK_FlowSort
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowAttr.FK_FlowSort);
            }
            set
            {
                SetValByKey(GenerWorkFlowAttr.FK_FlowSort, value);
            }
        }
		public string  FK_Dept
		{
			get
			{
				return this.GetValStrByKey(GenerWorkFlowAttr.FK_Dept);
			}
			set
			{
				SetValByKey(GenerWorkFlowAttr.FK_Dept,value);
			}
		}
		/// <summary>
		///  Title 
		/// </summary>
		public string  Title
		{
			get
			{
				return this.GetValStrByKey(GenerWorkFlowAttr.Title);
			}
			set
			{
				SetValByKey(GenerWorkFlowAttr.Title,value);
			}
		}
        /// <summary>
        ///  Customer Number 
        /// </summary>
        public string GuestNo
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowAttr.GuestNo);
            }
            set
            {
                SetValByKey(GenerWorkFlowAttr.GuestNo, value);
            }
        }
        /// <summary>
        ///  Customer Name 
        /// </summary>
        public string GuestName
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowAttr.GuestName);
            }
            set
            {
                SetValByKey(GenerWorkFlowAttr.GuestName, value);
            }
        }
		/// <summary>
		///  Generation time 
		/// </summary>
		public string  RDT
		{
			get
			{
				return this.GetValStrByKey(GenerWorkFlowAttr.RDT);
			}
			set
			{
				SetValByKey(GenerWorkFlowAttr.RDT,value);
			}
		}
        /// <summary>
        ///  Node should finish time 
        /// </summary>
        public string SDTOfNode
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowAttr.SDTOfNode);
            }
            set
            {
                SetValByKey(GenerWorkFlowAttr.SDTOfNode, value);
            }
        }
        /// <summary>
        ///  The process should be completed by the time 
        /// </summary>
        public string SDTOfFlow
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowAttr.SDTOfFlow);
            }
            set
            {
                SetValByKey(GenerWorkFlowAttr.SDTOfFlow, value);
            }
        }
		/// <summary>
		///  Process ID
		/// </summary>
        public Int64 WorkID
		{
			get
			{
                return this.GetValInt64ByKey(GenerWorkFlowAttr.WorkID);
			}
			set
			{
				SetValByKey(GenerWorkFlowAttr.WorkID,value);
			}
		}
        /// <summary>
        ///  The main thread ID
        /// </summary>
        public Int64 FID
        {
            get
            {
                return this.GetValInt64ByKey(GenerWorkFlowAttr.FID);
            }
            set
            {
                SetValByKey(GenerWorkFlowAttr.FID, value);
            }
        }
        /// <summary>
        ///  Parent ID  For or -1.
        /// </summary>
        public Int64 PWorkID
        {
            get
            {
                return this.GetValInt64ByKey(GenerWorkFlowAttr.PWorkID);
            }
            set
            {
                SetValByKey(GenerWorkFlowAttr.PWorkID, value);
            }
        }
        /// <summary>
        /// PFlowNo
        /// </summary>
        public string PFlowNo
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowAttr.PFlowNo);
            }
            set
            {
                SetValByKey(GenerWorkFlowAttr.PFlowNo, value);
            }
        }
        /// <summary>
        ///  Sponsor 
        /// </summary>
        public string Starter
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowAttr.Starter);
            }
            set
            {
                SetValByKey(GenerWorkFlowAttr.Starter, value);
            }
        }
        /// <summary>
        ///  Sponsor name 
        /// </summary>
        public string StarterName
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowAttr.StarterName);
            }
            set
            {
                this.SetValByKey(GenerWorkFlowAttr.StarterName, value);
            }
        }
        /// <summary>
        ///  Sponsor department name 
        /// </summary>
        public string DeptName
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowAttr.DeptName);
            }
            set
            {
                this.SetValByKey(GenerWorkFlowAttr.DeptName, value);
            }
        }
        /// <summary>
        ///  The current node name 
        /// </summary>
        public string NodeName
        {
            get
            {
                return this.GetValStrByKey(GenerWorkFlowAttr.NodeName);
            }
            set
            {
                this.SetValByKey(GenerWorkFlowAttr.NodeName, value);
            }
        }
		/// <summary>
		///  Work to the current node 
		/// </summary>
        public int FK_Node
        {
            get
            {
                return this.GetValIntByKey(GenerWorkFlowAttr.FK_Node);
            }
            set
            {
                SetValByKey(GenerWorkFlowAttr.FK_Node, value);
            }
        }
        /// <summary>
		///  Workflow status 
		/// </summary>
        public WFState WFState
        {
            get
            {
                return (WFState)this.GetValIntByKey(GenerWorkFlowAttr.WFState);
            }
            set
            {
                SetValByKey(GenerWorkFlowAttr.WFState, (int)value);
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
		#endregion

        #region  Extended Attributes 
        /// <summary>
        ///  Its sub-processes 
        /// </summary>
        public GenerWorkFlows HisSubFlowGenerWorkFlows
        {
            get
            {
                GenerWorkFlows ens = new GenerWorkFlows();
                ens.Retrieve(GenerWorkFlowAttr.PWorkID, this.WorkID);
                return ens;
            }
        }
        #endregion  Extended Attributes 

        #region  Constructor 
        /// <summary>
		///  Generating workflow 
		/// </summary>
		public GenerWorkFlow()
		{
		}
        public GenerWorkFlow(Int64 workId)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(GenerWorkFlowAttr.WorkID, workId);
            if (qo.DoQuery() == 0)
                throw new Exception(" The work  GenerWorkFlow [" + workId + "] Does not exist .");
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
                map.EnDesc = " Running process analysis ";

                map.AddTBIntPK(GenerWorkFlowAttr.WorkID, 0, " The work ID", true, true);

                map.AddDDLEntities(GenerWorkFlowAttr.FK_Flow, null, " Process ",new Flows(),false);
                map.AddDDLSysEnum(GenerWorkFlowAttr.WFState, 0, " Process Status ", true, false, GenerWorkFlowAttr.WFState);
                map.AddTBString(GenerWorkFlowAttr.Title, null, " Title ", true, false, 0, 100, 10);

                map.AddDDLEntities(GenerWorkFlowAttr.Starter, null, " Sponsor ", new  Emps(), false);

                map.AddTBString(GenerWorkFlowAttr.NodeName, null, " Node Name ", true, false, 0, 100, 10);

                map.AddDDLEntities(GenerWorkFlowAttr.FK_Dept, null, " Department ", new Depts(), false);

                map.AddDDLSysEnum(GenerWorkFlowAttr.PRI, 0, " Priority ", true, false, GenerWorkFlowAttr.PRI);

                map.AddTBDateTime(GenerWorkFlowAttr.RDT, " Record Date ", true, true);
                map.AddTBDateTime(GenerWorkFlowAttr.SDTOfNode, " Node should finish time ", true, true);
                map.AddTBDateTime(GenerWorkFlowAttr.SDTOfFlow, " The process should be completed by the time ", true, true);

             
                map.AddTBString(GenerWorkFlowAttr.BillNo, null, " Document Number ", true, false, 0, 100, 10);
                map.AddTBString(GenerWorkFlowAttr.FlowNote, null, " Remark ", true, false, 0, 4000, 10);


                map.AddTBString(GenerWorkFlowAttr.GuestNo, null, " Customer Number ", true, false, 0, 100, 10);
                map.AddTBString(GenerWorkFlowAttr.GuestName, null, " Customer Name ", true, false, 0, 100, 10);

                // Pool-related tasks .
                map.AddTBString(GenerWorkFlowAttr.TodoEmps, null, " Upcoming staff ", true, false, 0, 4000, 10);
           //     map.AddDDLSysEnum(GenerWorkFlowAttr.TaskSta, 0, " Sharing status ", true, false, GenerWorkFlowAttr.TaskSta);
                map.AddTBMyNum();

                //map.AddTBInt(GenerWorkFlowAttr.TodoEmpsNum, 0, " Upcoming number of personnel ", true, true);
                //map.AddTBInt(GenerWorkFlowAttr.TaskSta, 0, " Sharing status ", true, true);

                RefMethod rm = new RefMethod();
                rm.Title = " Work trajectory ";  // " Report ";
                rm.ClassMethodName = this.ToString() + ".DoRpt";
                rm.Icon = "/WF/Img/FileType/doc.gif";
                map.AddRefMethod(rm);

                rm = new RefMethod();
                rm.Title = " Process self-test "; // " Process self-test ";
                rm.ClassMethodName = this.ToString() + ".DoSelfTestInfo";
                map.AddRefMethod(rm);

                rm = new RefMethod();
                rm.Title = " Self-test and repair process ";
                rm.ClassMethodName = this.ToString() + ".DoRepare";
                rm.Warning = " Are you sure you want to perform this function ? \t\n 1) If the process is broken , And stay on the first node , System delete it .\t\n 2) If the non-ground first node , The system will return to the last position initiated .";
                map.AddRefMethod(rm);

                map.AddSearchAttr(GenerWorkFlowAttr.FK_Dept);
                map.AddSearchAttr(GenerWorkFlowAttr.FK_Flow);


                this._enMap = map;
                return this._enMap;
            }
        }
		#endregion 

		#region  Override the base class methods 
		/// <summary>
		///  Deleted , Workers also need to delete the list .
		/// </summary>
        protected override void afterDelete()
        {
            // . clear bad worker .  
            DBAccess.RunSQLReturnTable("DELETE FROM WF_GenerWorkerlist WHERE WorkID in  ( select WorkID from WF_GenerWorkerlist WHERE WorkID not in (select WorkID from WF_GenerWorkFlow) )");

            WorkFlow wf = new WorkFlow(new Flow(this.FK_Flow), this.WorkID,this.FID);
            wf.DoDeleteWorkFlowByReal(true); /*  Delete the following work .*/
            base.afterDelete();
        }
		#endregion 

		#region  Perform diagnostics 
        public string DoRpt()
        {
            PubClass.WinOpen("WFRpt.aspx?WorkID=" + this.WorkID + "&FID="+this.FID+"&FK_Flow="+this.FK_Flow);
            return null;
        }
		/// <summary>
		///  Perform repairs 
		/// </summary>
		/// <returns></returns>
        public string DoRepare()
        {
            if (this.DoSelfTestInfo() == " No abnormalities were found .")
                return " No abnormalities were found .";

            string sql = "SELECT FK_Node FROM WF_GenerWorkerlist WHERE WORKID='" + this.WorkID + "' ORDER BY FK_Node desc";
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
            {
                /* If the node is the start of work , Delete it .*/
                WorkFlow wf = new WorkFlow(new Flow(this.FK_Flow), this.WorkID, this.FID );
                wf.DoDeleteWorkFlowByReal(true);
                return " This process is initiated by the work because the system failed to be deleted .";
            }

            int FK_Node = int.Parse(dt.Rows[0][0].ToString());

            Node nd = new Node(FK_Node);
            if (nd.IsStartNode)
            {
                /* If the node is the start of work , Delete it .*/
                WorkFlow wf = new WorkFlow(new Flow(this.FK_Flow), this.WorkID, this.FID);
                wf.DoDeleteWorkFlowByReal(true);
                return " This process is initiated by the work because the system failed to be deleted .";
            }

            this.FK_Node = nd.NodeID;
            this.NodeName = nd.Name;
            this.Update();

            string str = "";
            GenerWorkerLists wls = new GenerWorkerLists();
            wls.Retrieve(GenerWorkerListAttr.FK_Node, FK_Node, GenerWorkerListAttr.WorkID, this.WorkID);
            foreach (GenerWorkerList wl in wls)
            {
                str += wl.FK_Emp + wl.FK_EmpText + ",";
            }

            return " This process is due [" + nd.Name + "] Failed to send the work to be rolled back to the current location , Please tell [" + str + "] Successful repair process .";
        }
		public string DoSelfTestInfo()
		{
            GenerWorkerLists wls = new GenerWorkerLists(this.WorkID, this.FK_Flow);

			#region   Look at the current node is the node start work .
			Node nd = new Node(this.FK_Node);
			if (nd.IsStartNode)
			{
				/*  Determine whether the node is returned  */
				Work wk = nd.HisWork;
				wk.OID = this.WorkID;
				wk.Retrieve();
			}
			#endregion


			#region   Check to see if there is a current job node information .
			bool isHave=false;
            foreach (GenerWorkerList wl in wls)
			{
				if (wl.FK_Node==this.FK_Node)
					isHave=true;
			}

			if (isHave==false)
			{
				/*  */
				return " Current working node information does not exist , The reason causing this process may not capture system abnormalities , Proposed the deletion of this process, or to automatically fix it .";
			}
			#endregion

			return " No abnormalities were found .";
		}
		#endregion
	}
	/// <summary>
    ///  Process instance s
	/// </summary>
	public class GenerWorkFlows : Entities
	{
		/// <summary>
		///  According to the workflow , Staff  ID  Check out his current work to do .
		/// </summary>
		/// <param name="flowNo"> Process ID </param>
		/// <param name="empId"> Staff ID</param>
		/// <returns></returns>
		public static DataTable QuByFlowAndEmp(string flowNo, int empId)
		{
			string sql="SELECT a.WorkID FROM WF_GenerWorkFlow a, WF_GenerWorkerlist b WHERE a.WorkID=b.WorkID   AND b.FK_Node=a.FK_Node  AND b.FK_Emp='"+empId.ToString()+"' AND a.FK_Flow='"+flowNo+"'";
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
				return new GenerWorkFlow();
			}
		}
		/// <summary>
		///  Examples of the collection process 
		/// </summary>
		public GenerWorkFlows(){}
		#endregion
	}
	
}
