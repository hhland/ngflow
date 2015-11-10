﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BP.WF
{
    public enum ActionType
    {
        /// <summary>
        ///  Launch 
        /// </summary>
        Start,
        /// <summary>
        ///  Go ahead ( Send )
        /// </summary>
        Forward,
        /// <summary>
        ///  Return 
        /// </summary>
        Return,
        /// <summary>
        ///  Transfer 
        /// </summary>
        Shift,
        /// <summary>
        ///  Undo transfer 
        /// </summary>
        UnShift,
        /// <summary>
        ///  Undo Send 
        /// </summary>
        UnSend,
        /// <summary>
        ///  Shunt forward 
        /// </summary>
        ForwardFL,
        /// <summary>
        ///  Confluence forward 
        /// </summary>
        ForwardHL,
        /// <summary>
        ///  Normal termination process 
        /// </summary>
        FlowOver,
        /// <summary>
        ///  Call flow screwdriver 
        /// </summary>
        CallSubFlow,
        /// <summary>
        ///  Promoter Process 
        /// </summary>
        StartSubFlow,
        /// <summary>
        ///  Child thread forward 
        /// </summary>
        SubFlowForward,
        /// <summary>
        ///  Retrieve 
        /// </summary>
        Tackback,
        /// <summary>
        ///  Recovery process has been completed 
        /// </summary>
        RebackOverFlow,
        /// <summary>
        ///  Forced termination process  For lijian:2012-10-24.
        /// </summary>
        FlowOverByCoercion,
        /// <summary>
        ///  Pending 
        /// </summary>
        HungUp,
        /// <summary>
        ///  Unsuspend 
        /// </summary>
        UnHungUp,
        /// <summary>
        ///  Forced transfer 
        /// </summary>
        ShiftByCoercion,
        /// <summary>
        ///  Reminders 
        /// </summary>
        Press,
        /// <summary>
        ///  Tombstone Process ( Revocation process )
        /// </summary>
        DeleteFlowByFlag,
        /// <summary>
        ///  Undelete process ( Revocation process )
        /// </summary>
        UnDeleteFlowByFlag,
        /// <summary>
        ///  Cc 
        /// </summary>
        CC,
        /// <summary>
        ///  Audit work 
        /// </summary>
        WorkCheck,
        /// <summary>
        ///  Remove the child thread 
        /// </summary>
        DeleteSubThread,
        /// <summary>
        ///  Request for endorsement 
        /// </summary>
        AskforHelp,
        /// <summary>
        ///  Plus sign sent down 
        /// </summary>
        ForwardAskfor,
        /// <summary>
        ///  Article sent automatically turn down 
        /// </summary>
        Skip
    }
    /// <summary>
    ///  Suspend Mode 
    /// </summary>
    public enum HungUpWay
    {
        /// <summary>
        ///  Permanently suspend 
        /// </summary>
        Forever,
        /// <summary>
        ///  Lift at the specified date 
        /// </summary>
        SpecDataRel
    }
    /// <summary>
    ///  Process Application Type 
    /// </summary>
    public enum FlowAppType
    {
        /// <summary>
        ///  Ordinary 
        /// </summary>
        Normal,
        /// <summary>
        ///  Engineering 
        /// </summary>
        PRJ
    }
    /// <summary>
    ///  Cc way 
    /// </summary>
    public enum CCWay
    {
        /// <summary>
        ///  According to information sent 
        /// </summary>
        ByMsg,
        /// <summary>
        ///  According to e-mail
        /// </summary>
        ByEmail,
        /// <summary>
        ///  According to phone 
        /// </summary>
        ByPhone,
        /// <summary>
        ///  According to database functions 
        /// </summary>
        ByDBFunc
    }
    /// <summary>
    ///  Cc Type 
    /// </summary>
    public enum CCType
    {
        /// <summary>
        ///  No CC 
        /// </summary>
        None,
        /// <summary>
        ///  By staff 
        /// </summary>
        AsEmps,
        /// <summary>
        ///  By post 
        /// </summary>
        AsStation,
        /// <summary>
        ///  By node 
        /// </summary>
        AsNode,
        /// <summary>
        ///  By sector 
        /// </summary>
        AsDept,
        /// <summary>
        ///  According to department and job 
        /// </summary>
        AsDeptAndStation
    }
    /// <summary>
    ///  Type wording 
    /// </summary>
    public enum XWType
    {
        /// <summary>
        ///  The wording 
        /// </summary>
        Up,
        /// <summary>
        ///  Parallel text 
        /// </summary>
        Line,
        /// <summary>
        ///  Under the wording 
        /// </summary>
        Down
    }
    /// <summary>
    ///  Document Type 
    /// </summary>
    public enum DocType
    {
        /// <summary>
        ///  Formal 
        /// </summary>
        OfficialDoc,
        /// <summary>
        ///  Memo 
        /// </summary>
        UnOfficialDoc,
        /// <summary>
        ///  Other 
        /// </summary>
        Etc
    }
    /// <summary>
    ///  Process form type 
    /// </summary>
    public enum FlowSheetType
    {
        /// <summary>
        ///  Forms Process 
        /// </summary>
        SheetFlow,
        /// <summary>
        ///  Document Process 
        /// </summary>
        DocFlow
    }
    /// <summary>
    ///  Process Type 
    /// </summary>
    public enum FlowType
    {
        /// <summary>
        ///  Plane flow 
        /// </summary>
        Panel,
        /// <summary>
        ///  Confluence points 
        /// </summary>
        FHL
    }
    /// <summary>
    ///  Process Status 
    /// </summary>
    public enum WFState
    {
        /// <summary>
        ///  Draft 
        /// </summary>
        Draft = 4,
        /// <summary>
        ///  Run 
        /// </summary>
        Runing = 0,
        /// <summary>
        ///  Normal completion 
        /// </summary>
        Complete = 1,
        /// <summary>
        ///  Pending 
        /// </summary>
        HungUp = 2,
        /// <summary>
        ///  Forced to complete 
        /// </summary>
        Cancel = 3
    }
    /// <summary>
    ///  Process startup type 
    /// </summary>
    public enum FlowRunWay
    {
        /// <summary>
        ///  Manually start 
        /// </summary>
        HandWork,
        /// <summary>
        ///  Start time designated staff 
        /// </summary>
        SpecEmp,
        /// <summary>
        ///  Data collection started on time 
        /// </summary>
        DataModel,
        /// <summary>
        ///  Trigger Start 
        /// </summary>
        InsertModel
    }
    /// <summary>
    ///  Save mode 
    /// </summary>
    public enum SaveModel
    {
        /// <summary>
        ///  Only node table .
        /// </summary>
        NDOnly,
        /// <summary>
        ///  Node table and Rpt表.
        /// </summary>
        NDAndRpt
    }
    /// <summary>
    ///  Complete shift processing nodes 
    /// </summary>
    public enum TurnToDeal
    {
        /// <summary>
        ///  Prompted by the system default 
        /// </summary>
        CCFlowMsg,
        /// <summary>
        ///  Specifies the message 
        /// </summary>
        SpecMsg,
        /// <summary>
        ///  Designation Url
        /// </summary>
        SpecUrl,
        /// <summary>
        ///  Conditional steering 
        /// </summary>
        TurnToByCond
    }
    /// <summary>
    ///  Delivery methods 
    /// </summary>
    public enum DeliveryWay
    {
        /// <summary>
        ///  By post 
        /// </summary>
        ByStation = 0,
        /// <summary>
        ///  By sector 
        /// </summary>
        ByDept = 1,
        /// <summary>
        /// 按SQL
        /// </summary>
        BySQL = 2,
        /// <summary>
        ///  Designated officer under this node set 
        /// </summary>
        ByEmp = 3,
        /// <summary>
        ///  Select from the previous step Sender 
        /// </summary>
        BySelected = 4,
        /// <summary>
        ///  By Form select staff 
        /// </summary>
        ByPreviousNodeFormEmpsField = 5,
        /// <summary>
        ///  Previous press operator 
        /// </summary>
        ByPreviousOper = 6,
        /// <summary>
        ///  Previous press operator and automatically jump 
        /// </summary>
        ByPreviousOperSkip = 7,
        /// <summary>
        ///  By personnel designated node calculation 
        /// </summary>
        BySpecNodeEmp = 8,
        /// <summary>
        ///  By computing the intersection of jobs and departments 
        /// </summary>
        ByDeptAndStation = 9,
        /// <summary>
        ///  Calculation by post ( Sectoral set latitude )
        /// </summary>
        ByStationAndEmpDept = 10,
        /// <summary>
        ///  Press staff positions specified node computing 
        /// </summary>
        BySpecNodeEmpStation = 11
    }
    /// <summary>
    ///  Return nodes work rules 
    /// </summary>
    public enum JumpWay
    {
        /// <summary>
        ///  Can not jump 
        /// </summary>
        CanNotJump,
        /// <summary>
        ///  Jump backwards 
        /// </summary>
        Next,
        /// <summary>
        ///  Jump forward 
        /// </summary>
        Previous,
        /// <summary>
        ///  Any node 
        /// </summary>
        AnyNode,
        /// <summary>
        ///  At any point 
        /// </summary>
        JumpSpecifiedNodes
    }
    /// <summary>
    ///  Return nodes work rules 
    /// </summary>
    public enum ReturnRole
    {
        /// <summary>
        ///  Can not be returned 
        /// </summary>
        CanNotReturn,
        /// <summary>
        ///  Only to return on a node 
        /// </summary>
        ReturnPreviousNode,
        /// <summary>
        ///  Returnable before any node ( Default )
        /// </summary>
        ReturnAnyNodes,
        /// <summary>
        ///  Returnable specified node 
        /// </summary>
        ReturnSpecifiedNodes
    }
    /// <summary>
    ///  Accessories open type 
    /// </summary>
    public enum FJOpen
    {
        /// <summary>
        ///  Not open 
        /// </summary>
        None,
        /// <summary>
        ///  Operator open 
        /// </summary>
        ForEmp,
        /// <summary>
        ///  Work ID Open 
        /// </summary>
        ForWorkID,
        /// <summary>
        ///  For the process ID Open 
        /// </summary>
        ForFID
    }
    /// <summary>
    ///  Diversion rules 
    /// </summary>
    public enum FLRole
    {
        /// <summary>
        ///  According to the recipient 
        /// </summary>
        ByEmp,
        /// <summary>
        ///  According to department 
        /// </summary>
        ByDept,
        /// <summary>
        ///  According to the post 
        /// </summary>
        ByStation
    }
    /// <summary>
    ///  Run mode 
    /// </summary>
    public enum RunModel
    {
        /// <summary>
        ///  General 
        /// </summary>
        Ordinary = 0,
        /// <summary>
        ///  Confluence 
        /// </summary>
        HL = 1,
        /// <summary>
        ///  Bypass 
        /// </summary>
        FL = 2,
        /// <summary>
        ///  Confluence points 
        /// </summary>
        FHL = 3,
        /// <summary>
        ///  Child thread 
        /// </summary>
        SubThread = 4
    }
    /// <summary>
    ///  Signature node type 
    /// </summary>
    public enum SignType
    {
        /// <summary>
        ///  Single sign 
        /// </summary>
        OneSign,
        /// <summary>
        ///  Countersign 
        /// </summary>
        Countersign
    }
    /// <summary>
    ///  Node types of work 
    ///  Node types of work ( 0,  Audit node , 1  Information collection node ,  2,  Start node )
    /// </summary>
    public enum NodeWorkType
    {
        Work = 0,
        /// <summary>
        ///  Start node 
        /// </summary>
        StartWork = 1,
        /// <summary>
        ///  Start node shunt 
        /// </summary>
        StartWorkFL = 2,
        /// <summary>
        ///  Confluent nodes 
        /// </summary>
        WorkHL = 3,
        /// <summary>
        ///  Shunt node 
        /// </summary>
        WorkFL = 4,
        /// <summary>
        ///  Confluence points 
        /// </summary>
        WorkFHL = 5,
        /// <summary>
        ///  Subprocess 
        /// </summary>
        SubThreadWork = 6
    }
    /// <summary>
    ///  Process Node Type 
    /// </summary>
    public enum FNType
    {
        /// <summary>
        ///  Plane node 
        /// </summary>
        Plane = 0,
        /// <summary>
        ///  Confluence points 
        /// </summary>
        River = 1,
        /// <summary>
        ///  Tributary 
        /// </summary>
        Branch = 2
    }
    /// <summary>
    ///  Who performed it 
    /// </summary>
    public enum CCRole
    {
        /// <summary>
        ///  Not Cc 
        /// </summary>
        UnCC,
        /// <summary>
        ///  Hand Cc 
        /// </summary>
        HandCC,
        /// <summary>
        ///  Automatic CC 
        /// </summary>
        AutoCC,
        /// <summary>
        ///  Manual and automatic coexist 
        /// </summary>
        HandAndAuto,
        /// <summary>
        ///  By Field 
        /// </summary>
        BySysCCEmps
    }
    /// <summary>
    ///  Who performed it 
    /// </summary>
    public enum WhoDoIt
    {
        /// <summary>
        ///  The operator 
        /// </summary>
        Operator,
        /// <summary>
        ///  Machine 
        /// </summary>
        MachtionOnly,
        /// <summary>
        ///  Mix 
        /// </summary>
        Mux
    }
    /// <summary>
    ///  Location Type 
    /// </summary>
    public enum NodePosType
    {
        Start,
        Mid,
        End
    }
    /// <summary>
    ///  Data collection node type 
    /// </summary>
    public enum FormType
    {
        /// <summary>
        ///  Fool form .
        /// </summary>
        FixForm = 0,
        /// <summary>
        ///  Freedom Form .
        /// </summary>
        FreeForm = 1,
        /// <summary>
        ///  Custom Form .
        /// </summary>
        SelfForm = 2,
        /// <summary>
        /// SDKForm
        /// </summary>
        SDKForm = 3,
        /// <summary>
        /// SL Form 
        /// </summary>
        SLForm=4,
        /// <summary>
        ///  Disable ( Form for more efficient processes )
        /// </summary>
        DisableIt = 9
    }
    /// <summary>
    ///  Job Type 
    /// </summary>
    public enum WorkType
    {
        /// <summary>
        ///  Ordinary 
        /// </summary>
        Ordinary,
        /// <summary>
        ///  Auto 
        /// </summary>
        Auto
    }
}
