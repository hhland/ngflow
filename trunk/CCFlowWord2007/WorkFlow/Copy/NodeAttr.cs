using System;
using System.Collections.Generic;
using System.Text;

namespace BP.WF
{
    /// <summary>
    ///  Node Properties 
    /// </summary>
    public class NodeAttr
    {
        #region  New Properties 
        /// <summary>
        ///  Approach when people do not find treatment 
        /// </summary>
        public const string WhenNoWorker = "WhenNoWorker";
        /// <summary>
        ///  Child thread type 
        /// </summary>
        public const string SubThreadType = "SubThreadType";
        /// <summary>
        ///  Can implicit return 
        /// </summary>
        public const string IsCanHidReturn = "IsCanHidReturn";
        /// <summary>
        ///  By Rate 
        /// </summary>
        public const string PassRate = "PassRate";
        /// <summary>
        ///  Can I set up process is complete 
        /// </summary>
        public const string IsCanOver = "IsCanOver";
        /// <summary>
        ///  Is secrecy step 
        /// </summary>
        public const string IsSecret = "IsSecret";
        public const string IsCCNode = "IsCCNode";
        public const string IsCCFlow = "IsCCFlow";
        public const string HisStas = "HisStas";
        public const string HisToNDs = "HisToNDs";
        public const string HisBillIDs = "HisBillIDs";
        public const string NodePosType = "NodePosType";
        public const string HisDeptStrs = "HisDeptStrs";
        public const string HisEmps = "HisEmps";
        public const string GroupStaNDs = "GroupStaNDs";
        public const string FJOpen = "FJOpen";
        public const string IsCanReturn = "IsCanReturn";
        public const string IsHandOver = "IsHandOver";
        public const string IsCanDelFlow = "IsCanDelFlow";
        /// <summary>
        ///  Can backtrack 
        /// </summary>
        public const string IsBackTracking = "IsBackTracking";
        /// <summary>
        ///  Whether hired delivery path automatic memory function ?
        /// </summary>
        public const string IsRM = "IsRM";

        public const string FormType = "FormType";
        public const string FormUrl = "FormUrl";
        /// <summary>
        ///  Can I view Report 
        /// </summary>
        public const string IsCanRpt = "IsCanRpt";
        /// <summary>
        ///  Before sending the message alert 
        /// </summary>
        public const string BeforeSendAlert = "BeforeSendAlert";
        /// <summary>
        ///  Whether it can be forced to delete threads 
        /// </summary>
        public const string IsForceKill = "IsForceKill";
        /// <summary>
        ///  Recipient sql
        /// </summary>
        public const string DeliveryParas = "DeliveryParas";
        /// <summary>
        ///  Return rules 
        /// </summary>
        public const string ReturnRole = "ReturnRole";
        /// <summary>
        ///  Steering handle 
        /// </summary>
        public const string TurnToDeal = "TurnToDeal";
        /// <summary>
        ///  Assessment methods 
        /// </summary>
        public const string CHWay = "CHWay";
        /// <summary>
        ///  Workload 
        /// </summary>
        public const string Workload = "Workload";
        #endregion

        #region  Basic properties 
        /// <summary>
        /// OID
        /// </summary>
        public const string NodeID = "NodeID";
        /// <summary>
        ///  Process node 
        /// </summary>
        public const string FK_Flow = "FK_Flow";
        /// <summary>
        /// FK_FlowSort
        /// </summary>
        public const string FK_FlowSort = "FK_FlowSort";
        /// <summary>
        /// FK_FlowSortT
        /// </summary>
        public const string FK_FlowSortT = "FK_FlowSortT";
        /// <summary>
        ///  Process name 
        /// </summary>
        public const string FlowName = "FlowName";
        /// <summary>
        ///  Whether the distribution of work 
        /// </summary>
        public const string IsTask = "IsTask";
        /// <summary>
        ///  Node types of work 
        /// </summary>
        public const string NodeWorkType = "NodeWorkType";
        /// <summary>
        ///  Description node 
        /// </summary>
        public const string Name = "Name";
        /// <summary>
        /// x
        /// </summary>
        public const string X = "X";
        /// <summary>
        /// y
        /// </summary>
        public const string Y = "Y";
        /// <summary>
        /// WarningDays( Warning Days )
        /// </summary>
        public const string WarningDays_del = "WarningDays";
        /// <summary>
        /// DeductDays( Deduction Days )
        /// </summary>
        public const string DeductDays = "DeductDays";
        /// <summary>
        ///  Warnings days 
        /// </summary>
        public const string WarningDays = "WarningDays";
        /// <summary>
        ///  Deduction 
        /// </summary>
        public const string DeductCent = "DeductCent";
        /// <summary>
        ///  Maximum deduction 
        /// </summary>
        public const string MaxDeductCent = "MaxDeductCent";
        /// <summary>
        ///  Plus hard 
        /// </summary>
        public const string SwinkCent = "SwinkCent";
        /// <summary>
        ///  The largest hard points 
        /// </summary>
        public const string MaxSwinkCent = "MaxSwinkCent";
        /// <summary>
        ///  Process Steps 
        /// </summary>
        public const string Step = "Step";
        /// <summary>
        ///  Work content 
        /// </summary>
        public const string Doc = "Doc";
        /// <summary>
        ///   Physical table name 
        /// </summary>
        public const string PTable = "PTable";
        /// <summary>
        ///  Signature Type 
        /// </summary>
        public const string SignType = "SignType";
        /// <summary>
        ///  Form display 
        /// </summary>
        public const string ShowSheets = "ShowSheets";
        /// <summary>
        ///  Run mode 
        /// </summary>
        public const string RunModel = "RunModel";
        /// <summary>
        ///  Who performed it ?
        /// </summary>
        public const string WhoExeIt = "WhoExeIt";
        /// <summary>
        ///  Diversion rules 
        /// </summary>
        public const string FLRole = "FLRole";
        /// <summary>
        /// IsSubFlow
        /// </summary>
        public const string HisSubFlows = "HisSubFlows";
        /// <summary>
        ///  Timeout processing content 
        /// </summary>
        public const string DoOutTime = "DoOutTime";
        /// <summary>
        ///  Timeout processing content 
        /// </summary>
        public const string OutTimeDeal = "OutTimeDeal";
        /// <summary>
        ///  Timeout condition 
        /// </summary>
        public const string DoOutTimeCond = "DoOutTimeCond";
        /// <summary>
        ///  Whether to allow the child to accept the staff duplicate threads ?
        /// </summary>
        public const string IsAllowRepeatEmps = "IsAllowRepeatEmps";
        /// <summary>
        ///  Property 
        /// </summary>
        public const string FrmAttr = "FrmAttr";
        /// <summary>
        ///  Send a personalized message 
        /// </summary>
        public const string TurnToDealDoc = "TurnToDealDoc";
        /// <summary>
        ///  Access Rules 
        /// </summary>
        public const string DeliveryWay = "DeliveryWay";
        /// <summary>
        ///  Focus field 
        /// </summary>
        public const string FocusField = "FocusField";
        /// <summary>
        ///  Node Form ID
        /// </summary>
        public const string NodeFrmID = "NodeFrmID";
        /// <summary>
        ///  Jump Rules 
        /// </summary>
        public const string JumpWay = "JumpWay";
        /// <summary>
        ///  Redirect node 
        /// </summary>
        public const string JumpSQL = "JumpSQL";
        /// <summary>
        ///  Read Receipts 
        /// </summary>
        public const string ReadReceipts = "ReadReceipts";
        /// <summary>
        ///  Send operation rules 
        /// </summary>
        public const string CCRole = "CCRole";
        /// <summary>
        ///  Save mode 
        /// </summary>
        public const string SaveModel = "SaveModel";
        /// <summary>
        ///  Sub-process start-up mode 
        /// </summary>
        public const string SubFlowStartWay = "SubFlowStartWay";
        /// <summary>
        ///  Subprocess startup parameters 
        /// </summary>
        public const string SubFlowStartParas = "SubFlowStartParas";
        /// <summary>
        ///  Whether the quality assessment point 
        /// </summary>
        public const string IsEval = "IsEval";
        /// <summary>
        ///  Avoidance rules 
        /// </summary>
        public const string CancelRole = "CancelRole";
        /// <summary>
        ///  Batch 
        /// </summary>
        public const string BatchRole = "BatchRole";
        /// <summary>
        ///  Batch parameters 
        /// </summary>
        public const string BatchParas = "BatchParas";

        /// <summary>
        ///  Automatically jump Rules 
        /// </summary>
        public const string AutoJumpRole0 = "AutoJumpRole0";
        public const string AutoJumpRole1 = "AutoJumpRole1";
        public const string AutoJumpRole2 = "AutoJumpRole2";
        /// <summary>
        ///  Is the client node is executed ?
        /// </summary>
        public const string IsGuestNode = "IsGuestNode";
        /// <summary>
        ///  Whether to enable shared task pool 
        /// </summary>
        public const string IsEnableTaskPool = "IsEnableTaskPool";
        /// <summary>
        ///  Print the document mode 
        /// </summary>
        public const string PrintDocEnable = "PrintDocEnable";
        #endregion
    }
}
