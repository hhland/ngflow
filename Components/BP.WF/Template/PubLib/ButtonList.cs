using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BP.WF.Template
{
    /// <summary>
    /// Btn Property 
    /// </summary>
    public class BtnAttr
    {
        /// <summary>
        ///  Node ID
        /// </summary>
        public const string NodeID = "NodeID";
        /// <summary>
        ///  Send labels 
        /// </summary>
        public const string SendLab = "SendLab";
        /// <summary>
        ///  Child thread button is enabled 
        /// </summary>
        public const string ThreadEnable = "ThreadEnable";
        /// <summary>
        ///  Child thread button labels 
        /// </summary>
        public const string ThreadLab = "ThreadLab";
        /// <summary>
        ///  Subprocess tag 
        /// </summary>
        public const string SubFlowLab = "SubFlowLab";
        /// <summary>
        ///  Sub-process control and display rules 
        /// </summary>
        public const string SubFlowCtrlRole = "SubFlowCtrlRole";
        /// <summary>
        ///  Save is enabled 
        /// </summary>
        public const string SaveEnable = "SaveEnable";
        /// <summary>
        ///  Jump Rules 
        /// </summary>
        public const string JumpWayLab = "JumpWayLab";
        /// <summary>
        ///  Save the label 
        /// </summary>
        public const string SaveLab = "SaveLab";
        /// <summary>
        ///  Return is enabled 
        /// </summary>
        public const string ReturnRole = "ReturnRole";
        /// <summary>
        ///  Return label 
        /// </summary>
        public const string ReturnLab = "ReturnLab";
        /// <summary>
        ///  Fill in the fields returned 
        /// </summary>
        public const string ReturnField = "ReturnField";
        /// <summary>
        ///  Label printing documents 
        /// </summary>
        public const string PrintDocLab = "PrintDocLab";
        /// <summary>
        ///  Whether to enable the printing of documents 
        /// </summary>
        public const string PrintDocEnable = "PrintDocEnable";
        /// <summary>
        ///  Transfer is enabled 
        /// </summary>
        public const string ShiftEnable = "ShiftEnable";
        /// <summary>
        ///  Transfer labels 
        /// </summary>
        public const string ShiftLab = "ShiftLab";
        /// <summary>
        ///  Querytags 
        /// </summary>
        public const string SearchLab = "SearchLab";
        /// <summary>
        ///  Check availability 
        /// </summary>
        public const string SearchEnable = "SearchEnable";
        /// <summary>
        ///  Locus 
        /// </summary>
        public const string TrackLab = "TrackLab";
        /// <summary>
        ///  Whether the track is enabled 
        /// </summary>
        public const string TrackEnable = "TrackEnable";
        /// <summary>
        ///  Cc 
        /// </summary>
        public const string CCLab = "CCLab";
        /// <summary>
        ///  CC rules 
        /// </summary>
        public const string CCRole = "CCRole";
        /// <summary>
        ///  Delete 
        /// </summary>
        public const string DelLab = "DelLab";
        /// <summary>
        ///  Delete is enabled 
        /// </summary>
        public const string DelEnable = "DelEnable";
        /// <summary>
        ///  End Process 
        /// </summary>
        public const string EndFlowLab = "EndFlowLab";
        /// <summary>
        ///  End Process 
        /// </summary>
        public const string EndFlowEnable = "EndFlowEnable";
        /// <summary>
        ///  Select recipient 
        /// </summary>
        public const string SelectAccepterLab = "SelectAccepterLab";
        /// <summary>
        /// SelectAccepterEnable
        /// </summary>
        public const string SelectAccepterEnable = "SelectAccepterEnable";
        /// <summary>
        ///  Send button 
        /// </summary>
        public const string SendJS = "SendJS";
        /// <summary>
        ///  Pending 
        /// </summary>
        public const string HungLab = "HungLab";
        /// <summary>
        ///  Whether to enable hang 
        /// </summary>
        public const string HungEnable = "HungEnable";
        /// <summary>
        ///  Check 
        /// </summary>
        public const string WorkCheckLab = "WorkCheckLab";
        /// <summary>
        ///  Audit is available 
        /// </summary>
        public const string WorkCheckEnable = "WorkCheckEnable";
        /// <summary>
        ///  Batch 
        /// </summary>
        public const string BatchLab = "BatchLab";
        /// <summary>
        ///  Batch is available 
        /// </summary>
        public const string BatchEnable = "BatchEnable";
        /// <summary>
        ///  Plus sign 
        /// </summary>
        public const string AskforLab = "AskforLab";
        /// <summary>
        ///  Endorsement label 
        /// </summary>
        public const string AskforEnable = "AskforEnable";

        /// <summary>
        ///  Circulation Custom  TransferCustomLab
        /// </summary>
        public const string TCLab = "TCLab";
        /// <summary>
        ///  Whether to enable - Circulation Custom 
        /// </summary>
        public const string TCEnable = "TCEnable";

        /// <summary>
        ///  Document 
        /// </summary>
        public const string WebOfficeLab = "WebOffice";
        /// <summary>
        ///  Document button labels 
        /// </summary>
        public const string WebOfficeEnable = "WebOfficeEnable";

        #region  Document Properties 
        public const string DocLeftWord = "DocLeftWord";
        public const string DocRightWord = "DocRightWord";
        #endregion  Document Properties 

        #region   Document button 
        /// <summary>
        ///  Open Local 
        /// </summary>
        public const string OfficeOpen = "OfficeOpen";
        public const string OfficeOpenEnable = "OfficeOpenEnable";
        /// <summary>
        ///  Open the template 
        /// </summary>
        public const string OfficeOpenTemplate = "OfficeOpenTemplate";
        public const string OfficeOpenTemplateEnable = "OfficeOpenTemplateEnable";
        /// <summary>
        ///  Save 
        /// </summary>
        public const string OfficeSave = "OfficeSave";
        public const string OfficeSaveEnable = "OfficeSaveEnable";
        /// <summary>
        ///  Accept Change 
        /// </summary>
        public const string OfficeAccept = "OfficeAccept";
        public const string OfficeAcceptEnable = "OfficeAcceptEnable";
        /// <summary>
        ///  Reject Changes 
        /// </summary>
        public const string OfficeRefuse = "OfficeRefuse";
        public const string OfficeRefuseEnable = "OfficeRefuseEnable";
        /// <summary>
        ///  Tao Hong button 
        /// </summary>
        public const string OfficeOver = "OfficeOver";
        public const string OfficeOverEnable = "OfficeOverEnable";
        /// <summary>
        ///  Is read-only 
        /// </summary>
        public const string OfficeReadOnly = "OfficeReadOnly";
        /// <summary>
        ///  View user traces 
        /// </summary>
        public const string OfficeMarks = "OfficeMarks";
        /// <summary>
        ///  Print 
        /// </summary>
        public const string OfficePrint = "OfficePrint";
        public const string OfficePrintEnable = "OfficePrintEnable";
        /// <summary>
        ///  Signature 
        /// </summary>
        public const string OfficeSeal = "OfficeSeal";
        public const string OfficeSealEnabel = "OfficeSealEnable";

        /// <summary>
        ///  Insertion process 
        /// </summary>
        public const string OfficeInsertFlow = "OfficeInsertFlow";
        public const string OfficeInsertFlowEnabel = "OfficeInsertFlowEnabel";


        /// <summary>
        ///  Risk insertion point 
        /// </summary>
        public const string OfficeInsertFengXian = "OfficeInsertFengXian";
        public const string OfficeInsertFengXianEnabel = "OfficeInsertFengXianEnabel";

        /// <summary>
        ///  Whether to automatically record the node information 
        /// </summary>
        public const string OfficeNodeInfo = "OfficeNodeInfo";
        /// <summary>
        ///  Whether the node is saved PDF
        /// </summary>
        public const string OfficeReSavePDF = "OfficeReSavePDF";
        /// <summary>
        ///  Whether to enter the traces mode 
        /// </summary>
        public const string OfficeIsMarks = "OfficeIsMarks";
        /// <summary>
        ///  Specify the document template 
        /// </summary>
        public const string OfficeTemplate = "OfficeTemplate";
        /// <summary>
        ///  Whether the parent process documentation 
        /// </summary>
        public const string OfficeIsParent = "OfficeIsParent";
        /// <summary>
        ///  Risk point template 
        /// </summary>
        public const string OfficeFengXianTemplate = "OfficeFengXianTemplate";
        /// <summary>
        ///  Whether to download enabled 
        /// </summary>
        public const string OfficeDownLab = "OfficeDownLab";
        public const string OfficeIsDown = "OfficeIsDown";
        // Whether automatic Taohong 
        public const string OfficeIsTrueTH = "OfficeIsTrueTH";
        public const string OfficeTHTemplate = "OfficeTHTemplate";
        #endregion
    }
    /// <summary>
    ///  Buttons list 
    /// </summary>
    public class ButtonList
    {
        /// <summary>
        ///  New Process 
        /// </summary>
        public const string Btn_NewFlow = "Btn_NewFlow";
        /// <summary>
        ///  Send Process 
        /// </summary>
        public const string Btn_Send = "Btn_Send";
        /// <summary>
        ///  Save Process 
        /// </summary>
        public const string Btn_Save = "Btn_Save";
        /// <summary>
        ///  Return 
        /// </summary>
        public const string Btn_Return = "Btn_Return";
        /// <summary>
        ///  Forwarding 
        /// </summary>
        public const string Btn_Forward = "Btn_Forward";
        /// <summary>
        ///  Send revocation 
        /// </summary>
        public const string Btn_UnSend = "Btn_UnSend";
        /// <summary>
        ///  Delete Process 
        /// </summary>
        public const string Btn_DelFlow = "Btn_DelFlow";
        /// <summary>
        ///  Process track 
        /// </summary>
        public const string Btn_Track = "Btn_Track";
        /// <summary>
        /// Btn_Search
        /// </summary>
        public const string Btn_Search = "Btn_Search";
    }
}
