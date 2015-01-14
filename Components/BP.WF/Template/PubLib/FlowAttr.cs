using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BP.WF.Template
{
    /// <summary>
    ///  Process Attributes 
    /// </summary>
    public class FlowAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  Serial number 
        /// </summary>
        public const string No = "No";
        /// <summary>
        ///  Name 
        /// </summary>
        public const string Name = "Name";
        /// <summary>
        /// CCType
        /// </summary>
        public const string CCType = "CCType";
        /// <summary>
        ///  Cc way 
        /// </summary>
        public const string CCWay = "CCWay";
        /// <summary>
        ///  Process Category 
        /// </summary>
        public const string FK_FlowSort = "FK_FlowSort";
        /// <summary>
        ///  Date of establishment .
        /// </summary>
        public const string CreateDate = "CreateDate";
        /// <summary>
        /// BillTable
        /// </summary>
        public const string BillTable = "BillTable";
        /// <summary>
        ///  Start working node type 
        /// </summary>
        public const string StartNodeType = "StartNodeType";
        /// <summary>
        /// StartNodeID
        /// </summary>
        public const string StartNodeID = "StartNodeID";
        /// <summary>
        ///  Can Process Num Check .
        /// </summary>
        public const string IsCanNumCheck = "IsCanNumCheck";
        /// <summary>
        ///  Whether Attachments 
        /// </summary>
        public const string IsFJ = "IsFJ";
        /// <summary>
        ///  Title generation rules 
        /// </summary>
        public const string TitleRole = "TitleRole";
        /// <summary>
        ///  Process Type 
        /// </summary>
        public const string FlowType = "FlowType";
        /// <summary>
        ///  The average in days 
        /// </summary>
        public const string AvgDay = "AvgDay";
        /// <summary>
        ///  Process type of operation 
        /// </summary>
        public const string FlowRunWay = "FlowRunWay";
        /// <summary>
        ///  Run setup 
        /// </summary>
        public const string RunObj = "RunObj";
        /// <summary>
        ///  Are there Bill
        /// </summary>
        public const string NumOfBill = "NumOfBill";
        /// <summary>
        ///  Schedule number 
        /// </summary>
        public const string NumOfDtl = "NumOfDtl";
        /// <summary>
        ///  Can start ?
        /// </summary>
        public const string IsCanStart = "IsCanStart";
        /// <summary>
        ///  Whether to automatically calculate future processors ?
        /// </summary>
        public const string IsFullSA = "IsFullSA";
        /// <summary>
        ///  Type 
        /// </summary>
        public const string FlowAppType = "FlowAppType";
        /// <summary>
        ///  Image Type 
        /// </summary>
        public const string ChartType = "ChartType";
        /// <summary>
        ///  Timeliness rules 
        /// </summary>
        public const string TimelineRole = "TimelineRole";
        /// <summary>
        ///  Sequence number 
        /// </summary>
        public const string Idx = "Idx";
        /// <summary>
        ///  Parameters 
        /// </summary>
        public const string Paras = "Paras";
        /// <summary>
        ///  Business main table 
        /// </summary>
        public const string PTable = "PTable";
        /// <summary>
        ///  Process data storage mode 
        /// </summary>
        public const string DataStoreModel = "DataStoreModel";
        /// <summary>
        ///  Process tag 
        /// </summary>
        public const string FlowMark = "FlowMark";
        /// <summary>
        ///  Process events entity 
        /// </summary>
        public const string FlowEventEntity = "FlowEventEntity";
        /// <summary>
        ///  Process designers Number 
        /// </summary>
        public const string DesignerNo = "DesignerNo";
        /// <summary>
        ///  Process designer name 
        /// </summary>
        public const string DesignerName = "DesignerName";
        /// <summary>
        ///  History initiate View field 
        /// </summary>
        public const string HistoryFields = "HistoryFields";
        /// <summary>
        ///  Is customer engagement process 
        /// </summary>
        public const string IsGuestFlow = "IsGuestFlow";
        /// <summary>
        ///  Document Numbering Format 
        /// </summary>
        public const string BillNoFormat = "BillNoFormat";
        /// <summary>
        ///  Flow Remark expressions 
        /// </summary>
        public const string FlowNoteExp = "FlowNoteExp";
        /// <summary>
        ///  Data access control mode 
        /// </summary>
        public const string DRCtrlType = "DRCtrlType";
        /// <summary>
        ///  Can initiate batch ?
        /// </summary>
        public const string IsBatchStart = "IsBatchStart";
        /// <summary>
        ///  Fill in the fields of bulk launched .
        /// </summary>
        public const string BatchStartFields = "BatchStartFields";
        /// <summary>
        ///  Continuation of the process approach 
        /// </summary>
        public const string CFlowWay = "CFlowWay";
        /// <summary>
        ///  Continuation of the process parameters 
        /// </summary>
        public const string CFlowPara = "CFlowPara";
        /// <summary>
        ///  Whether to enable 
        /// </summary>
        public const string IsOK1 = "IsOK";
        /// <summary>
        ///  Is MD5
        /// </summary>
        public const string IsMD5 = "IsMD5";
        public const string CCStas = "CCStas";
        public const string Note = "Note";
        /// <summary>
        ///  Run SQL
        /// </summary>
        public const string RunSQL = "RunSQL";
        #endregion  Basic properties 

        #region  Initiated limit rules .
        /// <summary>
        ///  Initiated limit rules 
        /// </summary>
        public const string StartLimitRole = "StartLimitRole";
        /// <summary>
        ///  Content Rules 
        /// </summary>
        public const string StartLimitPara = "StartLimitPara";
        /// <summary>
        ///  Rules Tip 
        /// </summary>
        public const string StartLimitAlert = "StartLimitAlert";
        /// <summary>
        ///  Tip Time 
        /// </summary>
        public const string StartLimitWhen = "StartLimitWhen";
        #endregion  Initiated limit rules .

        #region  Starting node data import rules .
        /// <summary>
        ///  Pre-launch rules 
        /// </summary>
        public const string StartGuideWay = "StartGuideWay";
        /// <summary>
        ///  Pre-launch parameters 1
        /// </summary>
        public const string StartGuidePara1 = "StartGuidePara1";
        /// <summary>
        ///  Pre-launch parameters 2
        /// </summary>
        public const string StartGuidePara2 = "StartGuidePara2";
        /// <summary>
        /// StartGuidePara3
        /// </summary>
        public const string StartGuidePara3 = "StartGuidePara3";
        /// <summary>
        ///  Whether to reset the start node to enable data ?
        /// </summary>
        public const string IsResetData = "IsResetData";
        /// <summary>
        ///  Whether to enable import historical data button ?
        /// </summary>
        public const string IsImpHistory = "IsImpHistory";
        /// <summary>
        ///  Whether to automatically load the data on a ?
        /// </summary>
        public const string IsLoadPriData = "IsLoadPriData";
        #endregion  Starting node data import rules .

        #region  Sons Process 
        /// <summary>
        /// ( When the current node as a child process ) Check that all sub-processes to complete the parent process to automatically send 
        /// </summary>
        public const string IsAutoSendSubFlowOver = "IsAutoSendSubFlowOver";
        /// <summary>
        ///  The version number 
        /// </summary>
        public const string Ver = "Ver";
        #endregion  Sons Process 
    }
    /// <summary>
    ///  Process evaluation type 
    /// </summary>
    public enum TimelineRole
    {
        /// <summary>
        ///  By node 
        /// </summary>
        ByNodeSet,
        /// <summary>
        ///  By the process 
        /// </summary>
        ByFlow
    }
    /// <summary>
    ///  Process initiated navigation 
    /// </summary>
    public enum StartGuideWay
    {
        /// <summary>
        /// 无
        /// </summary>
        None=0,
        /// <summary>
        ///  According to the system URL-( Child parent process ) Single mode 
        /// </summary>
        BySystemUrlOne=1,
        /// <summary>
        ///  According to the system URL-( Child parent process ) Multiple modes 
        /// </summary>
        BySystemUrlMulti=2,
        /// <summary>
        ///  According to the system URL-( Entity records ) Single mode 
        /// </summary>
        BySystemUrlOneEntity = 3,
        /// <summary>
        ///  According to the system URL-( Entity records ) Multiple modes 
        /// </summary>
        BySystemUrlMultiEntity = 4,
        /// <summary>
        ///  Historical Data 
        /// </summary>
        ByHistoryUrl = 5,
        /// <summary>
        ///  According to custom Url
        /// </summary>
        BySelfUrl=10,
        /// <summary>
        ///  According to user input parameters 
        /// </summary>
        ByParas=11
    }
    /// <summary>
    ///  Continuation of the process approach 
    /// </summary>
    public enum CFlowWay
    {
        /// <summary>
        /// 无: Non-continuation of the class Process 
        /// </summary>
        None,
        /// <summary>
        ///  Get in accordance with the parameters 
        /// </summary>
        ByParas,
        /// <summary>
        ///  Gets the specified field 
        /// </summary>
        BySpecField
    }
    /// <summary>
    ///  Process data storage mode 
    /// </summary>
    public enum DataStoreModel
    {
        /// <summary>
        ///  Stored in CCFlow Data table 
        /// </summary>
        ByCCFlow,
        /// <summary>
        ///  Specified business master table 
        /// </summary>
        SpecTable
    }
    /// <summary>
    ///  Process department access control type ( And other relevant reports )
    /// </summary>
    public enum FlowDeptDataRightCtrlType
    {
        /// <summary>
        ///  The department can only view 
        /// </summary>
        MyDeptOnly,
        /// <summary>
        ///  View this sector and lower sector 
        /// </summary>
        MyDeptAndBeloneToMyDeptOnly,
        /// <summary>
        ///  The process according to the specified department personnel access control 
        /// </summary>
        BySpecFlowDept,
        /// <summary>
        ///  Does not control , Anyone can view the data in any sector .
        /// </summary>
        AnyoneAndAnydept
    }
    /// <summary>
    ///  Image Type 
    /// </summary>
    public enum FlowChartType
    {
        /// <summary>
        ///  Geometry 
        /// </summary>
        Geometrical, 
        /// <summary>
        ///  Avatar Graphics 
        /// </summary>
        Icon
    }
}
