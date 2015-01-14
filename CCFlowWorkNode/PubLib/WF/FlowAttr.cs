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
        public const string RunSQL = "RunSQL";
        public const string StartListUrl = "StartListUrl";
        /// <summary>
        ///  Process Type 
        /// </summary>
        public const string FlowType = "FlowType";
        /// <summary>
        ///  The average in days 
        /// </summary>
        public const string AvgDay = "AvgDay";
        public const string FlowSheetType = "FlowSheetType";
        /// <summary>
        ///  Document Type 
        /// </summary>
        public const string DocType = "DocType";
        /// <summary>
        ///  Type wording 
        /// </summary>
        public const string XWType = "XWType";
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
        /// 
        /// </summary>
        public const string NumOfDtl = "NumOfDtl";
        /// <summary>
        ///  Can start ?
        /// </summary>
        public const string IsCanStart = "IsCanStart";
        /// <summary>
        ///  Track field 
        /// </summary>
        public const string AppType = "AppType";
        /// <summary>
        ///  Sequence number 
        /// </summary>
        public const string Idx = "Idx";
    }
}
