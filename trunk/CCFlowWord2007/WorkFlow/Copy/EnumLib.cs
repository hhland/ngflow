using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BP.WF
{
   
    /// <summary>
    ///  Endorsement mode 
    /// </summary>
    public enum AskforHelpSta
    {
        /// <summary>
        ///  Sent directly after endorsement 
        /// </summary>
        AfterDealSend=5,
        /// <summary>
        ///  Sent directly from me after endorsement 
        /// </summary>
        AfterDealSendByWorker=6
    }
    /// <summary>
    ///  Delete process rules 
    /// @0= You can not delete 
    /// @1= Tombstone 
    /// @2= Logging way to delete :  Data deleted , Recorded WF_DeleteWorkFlow中.
    /// @3= Completely remove :
    /// @4= Let the user decide to delete the way 
    /// </summary>
    public enum DelWorkFlowRole
    {
        /// <summary>
        ///  You can not delete 
        /// </summary>
        None,
        /// <summary>
        ///  Delete as marked ( Need to interact , Fill delete reasons )
        /// </summary>
        DeleteByFlag,
        /// <summary>
        ///  Delete to log library ( Need to interact , Fill delete reasons )
        /// </summary>
        DeleteAndWriteToLog,
        /// <summary>
        ///  Completely delete ( No need to interact , Direct clean thoroughly delete )
        /// </summary>
        DeleteReal,
        /// <summary>
        ///  Let the user decide to delete the way ( Need to interact )
        /// </summary>
        ByUser
    }
    /// <summary>
    ///  Import process model 
    /// </summary>
    public enum ImpFlowTempleteModel
    {
        /// <summary>
        ///  According to the new process 
        /// </summary>
        AsNewFlow,
        /// <summary>
        ///  The process according to the original number 
        /// </summary>
        AsTempleteFlowNo,
        /// <summary>
        ///  The process according to the original number, if it exists for the number on the cover 
        /// </summary>
        AsTempleteFlowNoOvrewaiteWhenExit,
        /// <summary>
        ///  Importing process specified number 
        /// </summary>
        AsSpecFlowNo
    }
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
        CallChildenFlow,
        /// <summary>
        ///  Promoter Process 
        /// </summary>
        StartChildenFlow,
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
        Skip,
        /// <summary>
        ///  Information 
        /// </summary>
        Info
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
    ///  When people did not find the handle 
    /// </summary>
    public enum WhenNoWorker
    {
         /// <summary>
        ///  An error 
        /// </summary>
        AlertErr,
        /// <summary>
        ///  Jump to the next step 
        /// </summary>
        Skip
    }
    /// <summary>
    ///  Automatically jump Rules 
    /// </summary>
    public enum AutoJumpRole
    {
        /// <summary>
        ///  Who is the author treatment 
        /// </summary>
        DealerIsDealer,
        /// <summary>
        ///  Treatment have occurred 
        /// </summary>
        DealerIsInWorkerList,
        /// <summary>
        ///  People with the same processing step 
        /// </summary>
        DealerAsNextStepWorker
    }
    /// <summary>
    ///  Batch nodes work 
    /// </summary>
    public enum BatchRole
    {
        /// <summary>
        ///  Not 
        /// </summary>
        No,
        /// <summary>
        ///  Can 
        /// </summary>
        Yes
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
        PRJ,
        /// <summary>
        ///  Document Process 
        /// </summary>
        DocFlow
    }
    /// <summary>
    ///  Child thread startup mode 
    /// </summary>
    public enum SubFlowStartWay
    {
        /// <summary>
        ///  Does not start 
        /// </summary>
        None,
        /// <summary>
        ///  According to the form field 
        /// </summary>
        BySheetField,
        /// <summary>
        ///  According to data from the table 
        /// </summary>
        BySheetDtlTable
    }
    /// <summary>
    ///  Avoidance rules 
    /// </summary>
    public enum CancelRole
    {
        /// <summary>
        ///  Previous only 
        /// </summary>
        OnlyNextStep,
        /// <summary>
        ///  Can not be undone 
        /// </summary>
        None,
        /// <summary>
        ///  Back with the start node .
        /// </summary>
        NextStepAndStartNode,
        /// <summary>
        ///  You can revoke the specified node 
        /// </summary>
        SpecNodes
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
    ///  Process Type 
    /// </summary>
    public enum FlowType_del
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
        ///  By post ( Sector-latitude )
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
        ///  The node is bound by staff 
        /// </summary>
        ByBindEmp = 3,
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
        BySpecNodeEmpStation = 11,
        /// <summary>
        /// 按SQL Determine the child thread to accept people with a data source .
        /// </summary>
        BySQLAsSubThreadEmpsAndData = 12,
        /// <summary>
        ///  Schedule determined by the recipient of the child thread .
        /// </summary>
        ByDtlAsSubThreadEmps = 13,
        /// <summary>
        ///  Only post by computing 
        /// </summary>
        ByStationOnly = 14,
        /// <summary>
        ///  According to ccflow的BPM Mode processing 
        /// </summary>
        ByCCFlowBPM=100
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
        ReturnSpecifiedNodes,
        /// <summary>
        ///  By a return line designed to determine the flow chart 
        /// </summary>
        ByReturnLine
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
    ///  Process Status 
    /// ccflow Depending on whether to enable the draft at the two operating modes , It is provided in web.config 是 IsEnableDraft  Node configuration .
    /// 1,  The draft is not enabled   IsEnableDraft = 0.
    ///     In this mode , There is no draft status ,  A user interface to enter the work after generating a Blank,  Users save time , Is stored blank Status .
    /// 2,  Enable draft .
    /// </summary>
    public enum WFState
    {
        /// <summary>
        ///  Blank 
        /// </summary>
        Blank = 0,
        /// <summary>
        ///  Draft 
        /// </summary>
        Draft = 1,
        /// <summary>
        ///  Run 
        /// </summary>
        Runing = 2,
        /// <summary>
        ///  Completed 
        /// </summary>
        Complete = 3,
        /// <summary>
        ///  Pending 
        /// </summary>
        HungUp = 4,
        /// <summary>
        ///  Return 
        /// </summary>
        ReturnSta = 5,
        /// <summary>
        ///  Forwarding ( Transfer )
        /// </summary>
        Shift = 6,
        /// <summary>
        ///  Delete ( Tombstoned state )
        /// </summary>
        Delete = 7,
        /// <summary>
        ///  Plus sign 
        /// </summary>
        Askfor=8,
        /// <summary>
        ///  Freeze 
        /// </summary>
        Fix=9
    }
    /// <summary>
    ///  Node types of work 
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
    ///// <summary>
    /////  Process Node Type 
    ///// </summary>
    //public enum FNType
    //{
    //    /// <summary>
    //    ///  Plane node 
    //    /// </summary>
    //    Plane = 0,
    //    /// <summary>
    //    ///  Confluence points 
    //    /// </summary>
    //    River = 1,
    //    /// <summary>
    //    ///  Tributary 
    //    /// </summary>
    //    Branch = 2
    //}
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
    /// <summary>
    ///  Child thread type 
    /// </summary>
    public enum SubThreadType
    {
        /// <summary>
        ///  With Form 
        /// </summary>
        SameSheet,
        /// <summary>
        ///  Different forms 
        /// </summary>
        UnSameSheet
    }
    /// <summary>
    ///  Read Receipts type 
    /// </summary>
    public enum ReadReceipts
    {
        /// <summary>
        ///  No receipt 
        /// </summary>
        None,
        /// <summary>
        ///  Automatic receipt 
        /// </summary>
        Auto,
        /// <summary>
        ///  Field is determined by the system 
        /// </summary>
        BySysField,
        /// <summary>
        ///  Press Developers parameters 
        /// </summary>
        BySDKPara
    }
    /// <summary>
    ///  Printing Methods 
    /// @0= Do not print @1= Print page @2= Print RTF Template 
    /// </summary>
    public enum PrintDocEnable
    {
        /// <summary>
        ///  Do not print 
        /// </summary>
        None,
        /// <summary>
        ///  Print page 
        /// </summary>
        PrintHtml,
        /// <summary>
        ///  Print RTF Template 
        /// </summary>
        PrintRTF
    }
}
