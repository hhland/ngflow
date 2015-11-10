using System;
using System.Collections.Generic;
using System.Text;
using BP.En;
using BP.WF.Template;
using BP.Sys;

namespace BP.WF.Data
{
    /// <summary>
    ///   Reporting base class property 
    /// </summary>
    public class NDXRptBaseAttr
    {
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
        /// Workid
        /// </summary>
        public const string OID = "OID";
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
        public const string FlowStartRDT = "FlowStartRDT";
        /// <summary>
        ///  Sponsor department number 
        /// </summary>
        public const string FK_Dept = "FK_Dept";
        /// <summary>
        ///  Process Status 
        /// </summary>
        public const string WFState = "WFState";
        /// <summary>
        ///  Process 
        /// </summary>
        public const string WFSta = "WFSta";

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
        public const string FlowEnderRDT = "FlowEnderRDT";
        /// <summary>
        ///  Span 
        /// </summary>
        public const string FlowDaySpan = "FlowDaySpan";
        /// <summary>
        ///  End node 
        /// </summary>
        public const string FlowEndNode = "FlowEndNode";
      
        /// <summary>
        ///  Continuation of the process WorkID
        /// </summary>
        public const string CWorkID = "CWorkID";
        /// <summary>
        ///  Continuation of the process ID 
        /// </summary>
        public const string CFlowNo = "CFlowNo";
        /// <summary>
        ///  Customer Number 
        /// </summary>
        public const string GuestNo = "GuestNo";
        /// <summary>
        ///  Customer Name 
        /// </summary>
        public const string GuestName = "GuestName";
        /// <summary>
        /// BillNo
        /// </summary>
        public const string BillNo = "BillNo";

        #region  Sons process property .
        /// <summary>
        ///  Parent process WorkID
        /// </summary>
        public const string PWorkID = "PWorkID";
        /// <summary>
        ///  Parent process ID 
        /// </summary>
        public const string PFlowNo = "PFlowNo";
        /// <summary>
        ///  Node calls sub-processes 
        /// </summary>
        public const string PNodeID = "PNodeID";
        /// <summary>
        ///  Lift the sub-processes of people 
        /// </summary>
        public const string PEmp = "PEmp";
        /// <summary>
        ///  Parameters 
        /// </summary>
        public const string AtPara = "AtPara";
        #endregion  Sons process property .
    }
    /// <summary>
    ///  Reporting base class 
    /// </summary>
    abstract public class NDXRptBase : BP.En.EntityOID
    {
        #region  Property 
        /// <summary>
        ///  The work ID
        /// </summary>
        public new Int64 OID
        {
            get
            {
                return this.GetValInt64ByKey(NDXRptBaseAttr.OID);
            }
            set
            {
                this.SetValByKey(NDXRptBaseAttr.OID, value);
            }
        }
        /// <summary>
        ///  Process time span 
        /// </summary>
        public int FlowDaySpan
        {
            get
            {
                return this.GetValIntByKey(NDXRptBaseAttr.FlowDaySpan);
            }
            set
            {
                this.SetValByKey(NDXRptBaseAttr.FlowDaySpan, value);
            }
        }
        /// <summary>
        ///  Quantity 
        /// </summary>
        public int MyNum
        {
            get
            {
                return 1;
            }
        }
        /// <summary>
        ///  The main flow ID
        /// </summary>
        public Int64 FID
        {
            get
            {
                return this.GetValInt64ByKey(NDXRptBaseAttr.FID);
            }
            set
            {
                this.SetValByKey(NDXRptBaseAttr.FID, value);
            }
        }
        /// <summary>
        ///  Process participants 
        /// </summary>
        public string FlowEmps
        {
            get
            {
                return this.GetValStringByKey(NDXRptBaseAttr.FlowEmps);
            }
            set
            {
                this.SetValByKey(NDXRptBaseAttr.FlowEmps, value);
            }
        }
        /// <summary>
        ///  Customer Number 
        /// </summary>
        public string GuestNo
        {
            get
            {
                return this.GetValStringByKey(NDXRptBaseAttr.GuestNo);
            }
            set
            {
                this.SetValByKey(NDXRptBaseAttr.GuestNo, value);
            }
        }
        /// <summary>
        ///  Customer Name 
        /// </summary>
        public string GuestName
        {
            get
            {
                return this.GetValStringByKey(NDXRptBaseAttr.GuestName);
            }
            set
            {
                this.SetValByKey(NDXRptBaseAttr.GuestName, value);
            }
        }
        public string BillNo
        {
            get
            {
                return this.GetValStringByKey(NDXRptBaseAttr.BillNo);
            }
            set
            {
                this.SetValByKey(NDXRptBaseAttr.BillNo, value);
            }
        }
        /// <summary>
        ///  Process sponsor 
        /// </summary>
        public string FlowStarter
        {
            get
            {
                return this.GetValStringByKey(NDXRptBaseAttr.FlowStarter);
            }
            set
            {
                this.SetValByKey(NDXRptBaseAttr.FlowStarter, value);
            }
        }
        /// <summary>
        ///  Process initiated by date 
        /// </summary>
        public string FlowStartRDT
        {
            get
            {
                return this.GetValStringByKey(NDXRptBaseAttr.FlowStartRDT);
            }
            set
            {
                this.SetValByKey(NDXRptBaseAttr.FlowStartRDT, value);
            }
        }
        /// <summary>
        ///  End of the process by 
        /// </summary>
        public string FlowEnder
        {
            get
            {
                return this.GetValStringByKey(NDXRptBaseAttr.FlowEnder);
            }
            set
            {
                this.SetValByKey(NDXRptBaseAttr.FlowEnder, value);
            }
        }
        /// <summary>
        ///  Process End Time 
        /// </summary>
        public string FlowEnderRDT
        {
            get
            {
                return this.GetValStringByKey(NDXRptBaseAttr.FlowEnderRDT);
            }
            set
            {
                this.SetValByKey(NDXRptBaseAttr.FlowEnderRDT, value);
            }
        }
        /// <summary>
        ///  End Node Name 
        /// </summary>
        public string FlowEndNodeText
        {
            get
            {
                Node nd = new Node(this.FlowEndNode);
                return nd.Name;
            }
        }
        /// <summary>
        ///  Node ID
        /// </summary>
        public int FlowEndNode
        {
            get
            {
                return this.GetValIntByKey(NDXRptBaseAttr.FlowEndNode);
            }
            set
            {
                this.SetValByKey(NDXRptBaseAttr.FlowEndNode, value);
            }
        }
        /// <summary>
        ///  Process title 
        /// </summary>
        public string Title
        {
            get
            {
                return this.GetValStringByKey(NDXRptBaseAttr.Title);
            }
            set
            {
                this.SetValByKey(NDXRptBaseAttr.Title, value);
            }
        }
        /// <summary>
        ///  Years of membership 
        /// </summary>
        public string FK_NY
        {
            get
            {
                return this.GetValStringByKey(NDXRptBaseAttr.FK_NY);
            }
            set
            {
                this.SetValByKey(NDXRptBaseAttr.FK_NY, value);
            }
        }
        /// <summary>
        ///  Sponsor department 
        /// </summary>
        public string FK_Dept
        {
            get
            {
                return this.GetValStringByKey(NDXRptBaseAttr.FK_Dept);
            }
            set
            {
                this.SetValByKey(NDXRptBaseAttr.FK_Dept, value);
            }
        }
        /// <summary>
        ///  Process Status 
        /// </summary>
        public WFState WFState
        {
            get
            {
                return (WFState)this.GetValIntByKey(NDXRptBaseAttr.WFState);
            }
            set
            {
                this.SetValByKey(NDXRptBaseAttr.WFState, (int)value);
            }
        }
        /// <summary>
        ///  State name 
        /// </summary>
        public string WFStateText
        {
            get
            {
                switch (this.WFState)
                {
                    case WF.WFState.Complete:
                        return " Completed ";
                    case WF.WFState.Delete:
                        return " Deleted ";
                    default:
                        return " Run ";
                }
            }
        }
        /// <summary>
        ///  Parent process WorkID
        /// </summary>
        public Int64 PWorkID
        {
            get
            {
                return this.GetValInt64ByKey(NDXRptBaseAttr.PWorkID);
            }
            set
            {
                this.SetValByKey(NDXRptBaseAttr.PWorkID, value);
            }
        }
        /// <summary>
        ///  Parent process ID process 
        /// </summary>
        public string PFlowNo
        {
            get
            {
                return this.GetValStringByKey(NDXRptBaseAttr.PFlowNo);
            }
            set
            {
                this.SetValByKey(NDXRptBaseAttr.PFlowNo, value);
            }
        }
        /// <summary>
        ///  Continuation of the process WorkID
        /// </summary>
        public Int64 CWorkID
        {
            get
            {
                return this.GetValInt64ByKey(NDXRptBaseAttr.CWorkID);
            }
            set
            {
                this.SetValByKey(NDXRptBaseAttr.CWorkID, value);
            }
        }
        /// <summary>
        ///  Continuation of the process ID 
        /// </summary>
        public string CFlowNo
        {
            get
            {
                return this.GetValStringByKey(NDXRptBaseAttr.CFlowNo);
            }
            set
            {
                this.SetValByKey(NDXRptBaseAttr.CFlowNo, value);
            }
        }
        #endregion attrs

        #region  Structure 
        /// <summary>
        ///  Structure 
        /// </summary>
        protected NDXRptBase()
        {
        }
        /// <summary>
		///  According to OID Constructive Solid 
		/// </summary>
        /// <param name=" The work ID">workid</param>
        protected NDXRptBase(int workid):base(workid)  
		{
        }
        #endregion  Structure 
    }
    /// <summary>
    ///  Reporting base class s
    /// </summary>
    abstract public class NDXRptBases : BP.En.EntitiesOID
    {
        /// <summary>
        ///  Reporting base class s
        /// </summary>
        public NDXRptBases()
        {
        }
    }
}
