using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BP.WF
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
        #endregion  Basic properties 

        /// <summary>
        ///  Whether hired 
        /// </summary>
        public const string IsOK = "IsOK";
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
        /// <summary>
        ///  The list starts running 
        /// </summary>
        public const string StartListUrl = "StartListUrl";
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
        ///  Type 
        /// </summary>
        public const string FlowAppType = "FlowAppType";
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
        ///  Process ID 
        /// </summary>
        public const string FlowCode = "FlowCode";
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
}
