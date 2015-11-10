using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace BP.WF
{
    public class SendReturnMsgFlag
    {
        /// <summary>
        ///  In line with the completion of the workflow conditions 
        /// </summary>
        public const string MacthFlowOver = "MacthFlowOver";
        /// <summary>
        ///  Current work [{0}] Has been completed 
        /// </summary>
        public const string CurrWorkOver = "CurrWorkOver";
        /// <summary>
        ///  Meet the closing conditions , The process is completed 
        /// </summary>
        public const string FlowOverByCond = "FlowOverByCond";
        /// <summary>
        ///  To staff 
        /// </summary>
        public const string ToEmps = "ToEmps";
        /// <summary>
        ///  End confluence 
        /// </summary>
        public const string HeLiuOver = "HeLiuOver";
        /// <summary>
        ///  Report 
        /// </summary>
        public const string WorkRpt = "WorkRpt";
        /// <summary>
        ///  Start node 
        /// </summary>
        public const string WorkStartNode = "WorkStartNode";
        /// <summary>
        ///  Work started 
        /// </summary>
        public const string WorkStart = "WorkStart";
        /// <summary>
        ///  Process ends 
        /// </summary>
        public const string FlowOver = "FlowOver";
        /// <summary>
        ///  After the success of abnormal events sent 
        /// </summary>
        public const string SendSuccessMsgErr = "SendSuccessMsgErr";
        /// <summary>
        ///  Information sent successfully 
        /// </summary>
        public const string SendSuccessMsg = "SendSuccessMsg";
        /// <summary>
        ///  Separation process information 
        /// </summary>
        public const string FenLiuInfo = "FenLiuInfo";
        /// <summary>
        ///  Send a copy of the message 
        /// </summary>
        public const string CCMsg = "CCMsg";
        /// <summary>
        ///  Edit recipient 
        /// </summary>
        public const string EditAccepter = "EditAccepter";
        /// <summary>
        ///  New Process 
        /// </summary>
        public const string NewFlowUnSend = "NewFlowUnSend";
        /// <summary>
        ///  Send revocation 
        /// </summary>
        public const string UnSend = "UnSend";
        /// <summary>
        ///  Report form 
        /// </summary>
        public const string Rpt = "Rpt";
        /// <summary>
        ///  Sent 
        /// </summary>
        public const string SendWhen = "SendWhen";
        /// <summary>
        ///  The current process ends 
        /// </summary>
        public const string End = "End";
        /// <summary>
        ///  The current process is completed 
        /// </summary>
        public const string OverCurr = "OverCurr";
        /// <summary>
        ///  Flow direction information 
        /// </summary>
        public const string CondInfo = "CondInfo";
        /// <summary>
        ///  A node is completed 
        /// </summary>
        public const string OneNodeSheetver = "OneNodeSheetver";
        /// <summary>
        ///  Document information 
        /// </summary>
        public const string BillInfo = "BillInfo";
        /// <summary>
        ///  Text information ( The system does not generate )
        /// </summary>
        public const string MsgOfText = "MsgOfText";

        #region  System Variables 
        /// <summary>
        ///  The work ID
        /// </summary>
        public const string VarWorkID = "VarWorkID";
        /// <summary>
        ///  The current node ID
        /// </summary>
        public const string VarCurrNodeID = "VarCurrNodeID";
        /// <summary>
        ///  The current node name 
        /// </summary>
        public const string VarCurrNodeName = "VarCurrNodeName";
        /// <summary>
        ///  Arrives at a node ID
        /// </summary>
        public const string VarToNodeID = "VarToNodeID";
        /// <summary>
        ///  Arrives at a node name 
        /// </summary>
        public const string VarToNodeName = "VarToNodeName";
        /// <summary>
        ///  People accept the collection name ( Separated by commas )
        /// </summary>
        public const string VarAcceptersName = "VarAcceptersName";
        /// <summary>
        ///  Recipient collection ID( Separated by commas )
        /// </summary>
        public const string VarAcceptersID = "VarAcceptersID";
        /// <summary>
        ///  Recipient collection ID Name( Separated by commas )
        /// </summary>
        public const string VarAcceptersNID = "VarAcceptersNID";
        /// <summary>
        ///  Child threads WorkIDs
        /// </summary>
        public const string VarTreadWorkIDs = "VarTreadWorkIDs";
        #endregion  System Variables 
    }
}
