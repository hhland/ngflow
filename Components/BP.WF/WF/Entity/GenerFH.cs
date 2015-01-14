using System;
using System.Data;
using BP.DA;
using BP.WF;
using BP.Port;
using BP.En;

namespace BP.WF
{
	/// <summary>
	///  Generates flow control division 
	/// </summary>
    public class GenerFHAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  Number rent 
        /// </summary>
        public const string FID = "FID";
        /// <summary>
        ///  Workflow 
        /// </summary>
        public const string FK_Flow = "FK_Flow";
        /// <summary>
        ///  Process Status 
        /// </summary>
        public const string WFState = "WFState";
        /// <summary>
        ///  Record people 
        /// </summary>
        public const string GroupKey = "GroupKey";
        /// <summary>
        ///  Work to the current node .
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        ///  Send to staff 
        /// </summary>
        public const string ToEmpsMsg = "ToEmpsMsg";
        /// <summary>
        ///  Title 
        /// </summary>
        public const string Title = "Title";
        /// <summary>
        ///  Record Time 
        /// </summary>
        public const string RDT = "RDT";
        #endregion
    }
	/// <summary>
	///  Generates flow control division 
	/// </summary>
    public class GenerFH : Entity
    {
        #region  Basic properties 
        public override string PK
        {
            get
            {
                return "FID";
            }
        }
        /// <summary>
        /// HisFlow
        /// </summary>
        public Flow HisFlow
        {
            get
            {
                return new Flow(this.FK_Flow);
            }
        }
        public string RDT
        {
            get
            {
                return this.GetValStringByKey(GenerFHAttr.RDT);
            }
            set
            {
                SetValByKey(GenerFHAttr.RDT, value);
            }
        }
        public string Title
        {
            get
            {
                return this.GetValStringByKey(GenerFHAttr.Title);
            }
            set
            {
                SetValByKey(GenerFHAttr.Title, value);
            }
        }
        /// <summary>
        ///  Workflow number 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.GetValStringByKey(GenerFHAttr.FK_Flow);
            }
            set
            {
                SetValByKey(GenerFHAttr.FK_Flow, value);
            }
        }
        public string ToEmpsMsg
        {
            get
            {
                return this.GetValStringByKey(GenerFHAttr.ToEmpsMsg);
            }
            set
            {
                SetValByKey(GenerFHAttr.ToEmpsMsg, value);
            }
        }
        
        /// <summary>
        ///  Process ID
        /// </summary>
        public Int64 FID
        {
            get
            {
                return this.GetValInt64ByKey(GenerFHAttr.FID);
            }
            set
            {
                SetValByKey(GenerFHAttr.FID, value);
            }
        }
        public string GroupKey
        {
            get
            {
                return this.GetValStringByKey(GenerFHAttr.GroupKey);
            }
            set
            {
                this.SetValByKey(GenerFHAttr.GroupKey, value);
            }
        }
        public string FK_NodeText
        {
            get
            {
                Node nd = new Node(this.FK_Node);
                return nd.Name;
            }
        }
        /// <summary>
        ///  Work to the current node 
        /// </summary>
        public int FK_Node
        {
            get
            {
                return this.GetValIntByKey(GenerFHAttr.FK_Node);
            }
            set
            {
                SetValByKey(GenerFHAttr.FK_Node, value);
            }
        }
        /// <summary>
        ///  Workflow status ( 0,  Unfinished ,1  Carry out , 2  Forced termination  3,  Delete status ,) 
        /// </summary>
        public int WFState
        {
            get
            {
                return this.GetValIntByKey(GenerFHAttr.WFState);
            }
            set
            {
                SetValByKey(GenerFHAttr.WFState, value);
            }
        }
        #endregion  

        #region  Constructor 
        /// <summary>
        ///  The switching process control flow generated 
        /// </summary>
        public GenerFH()
        {
        }
        /// <summary>
        ///  The switching process control flow generated 
        /// </summary>
        /// <param name="FID"></param>
        public GenerFH(Int64 FID)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(GenerFHAttr.FID, FID);
            if (qo.DoQuery() == 0)
                throw new Exception(" Inquiry  GenerFH  The work [" + FID + "] Does not exist , May be completed .");
        }
        /// <summary>
        ///  The switching process control flow generated 
        /// </summary>
        /// <param name="FID"> Workflow ID</param>
        /// <param name="flowNo"> Process ID </param>
        public GenerFH(Int64 FID, string flowNo)
        {
            try
            {
                this.FID = FID;
                this.FK_Flow = flowNo;
                this.Retrieve();
            }
            catch (Exception ex)
            {
                //WorkFlow wf = new WorkFlow(new Flow(flowNo), FID, FID);
                //StartWork wk = wf.HisStartWork;
                //if (wf.WFState == BP.WF.WFState.Complete)
                //{
                //    throw new Exception("@ The process has been completed , Does not exist in the current work in the collection , If you want to get details on this process, please see the history of work . Technical Information :" + ex.Message);
                //}
                //else
                //{
                //    this.Copy(wk);
                //    //string msg = "@ Process internal error , To inconvenience , Deeply sorry , Please notify this case to the system administrator .error code:0001 More information :" + ex.Message;
                //    string msg = "@ Process internal error , To inconvenience , Deeply sorry , Please notify this case to the system administrator .error code:0001 More information :" + ex.Message;
                //    Log.DefaultLogWriteLine(LogType.Error, "@ After the work is done using it throws an exception :" + msg);
                //    //throw new Exception(msg);
                //}
            }
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
                Map map = new Map("WF_GenerFH");
                map.EnDesc = " Flow Control division ";

                map.AddTBIntPK(GenerFHAttr.FID, 0, " Process ID", true, true);

                map.AddTBString(GenerFHAttr.Title, null, " Title ", true, false, 0, 4000, 10);
                map.AddTBString(GenerFHAttr.GroupKey, null, " Grouping the primary key ", true, false, 0, 3000, 10);
                map.AddTBString(GenerFHAttr.FK_Flow, null, " Process ", true, false, 0, 500, 10);
                map.AddTBString(GenerFHAttr.ToEmpsMsg, null, " Acceptance of staff ", true, false, 0, 4000, 10);
                map.AddTBInt(GenerFHAttr.FK_Node, 0, " Stay node ", true, false);
                map.AddTBInt(GenerFHAttr.WFState, 0, "WFState", true, false);
                map.AddTBDate(GenerFHAttr.RDT, null, "RDT", true, false);

                //RefMethod rm = new RefMethod();
                //rm.Title = " Report ";  // " Report ";
                //rm.ClassMethodName = this.ToString() + ".DoRpt";
                //rm.Icon = "../WF/Img/Btn/doc.gif";
                //map.AddRefMethod(rm);

                //rm = new RefMethod();
                //rm.Title = this.ToE("FlowSelfTest", " Process self-test "); // " Process self-test ";
                //rm.ClassMethodName = this.ToString() + ".DoSelfTestInfo";
                //map.AddRefMethod(rm);

                //rm = new RefMethod();
                //rm.Title = " Self-test and repair process ";
                //rm.ClassMethodName = this.ToString() + ".DoRepare";
                //rm.Warning = " Are you sure you want to perform this function ? \t\n 1) If the process is broken , And stay on the first node , System delete it .\t\n 2) If the non-ground first node , The system will return to the last position initiated .";
                //map.AddRefMethod(rm);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        #region  Override the base class methods 
        protected override void afterDelete()
        {
            base.afterDelete();
        }
        #endregion
    }
	/// <summary>
	///  Generates flow control division s
	/// </summary>
	public class GenerFHs : Entities
	{
		/// <summary>
		///  According to the workflow , Staff ID  Check out his current work to do .
		/// </summary>
		/// <param name="flowNo"> Process ID </param>
		/// <param name="empId"> Staff ID</param>
		/// <returns></returns>
		public static DataTable QuByFlowAndEmp(string flowNo, int empId)
		{
			string sql="SELECT a.FID FROM WF_GenerFH a, WF_GenerWorkerlist b WHERE a.FID=b.FID   AND b.FK_Node=a.FK_Node  AND b.FK_Emp='"+empId.ToString()+"' AND a.FK_Flow='"+flowNo+"'";
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
				return new GenerFH();
			}
		}
		/// <summary>
		///  Generate workflow collection 
		/// </summary>
		public GenerFHs(){}
		#endregion
	}
	
}
