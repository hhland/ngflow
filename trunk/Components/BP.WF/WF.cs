using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BP.WF;
using BP.Web;
using BP.En;
using BP.DA;
using BP.En;

namespace BP.WF
{
    /// <summary>
    ///  Execution of contents 
    /// </summary>
    public class DoWhatList
    {
        public const string DoNode = "DoNode";
        public const string Start = "Start";
        public const string Start5 = "Start5";
        public const string JiSu = "JiSu";
        public const string StartLigerUI = "StartLigerUI";
        public const string StartSmall = "StartSmall";
        public const string StartSmallSingle = "StartSmallSingle";
        public const string MyWork = "MyWork";
        public const string Login = "Login";
        public const string FlowSearch = "FlowSearch";
        public const string FlowSearchSmall = "FlowSearchSmall";
        public const string FlowSearchSmallSingle = "FlowSearchSmallSingle";
        public const string Emps = "Emps";
        public const string EmpWorks = "EmpWorks";
        public const string MyFlow = "MyFlow";
        public const string FlowFX = "FlowFX";
        public const string DealWork = "DealWork";
        public const string DealWorkInSmall = "DealWorkInSmall";
        public const string DealWorkInSmallSingle = "DealWorkInSmallSingle";
        public const string Tools = "Tools";
        public const string ToolsSmall = "ToolsSmall";
        public const string Runing = "Runing";
        public const string EmpWorksSmall = "EmpWorksSmall";
        public const string EmpWorksSmallSingle = "EmpWorksSmallSingle";
        public const string RuningSmall = "RuningSmall";
        /// <summary>
        ///  Work Processor 
        /// </summary>
        public const string OneWork = "OneWork";
    }
    public enum FlowShowType
    {
        /// <summary>
        ///  Current work 
        /// </summary>
        MyWorks,
        /// <summary>
        ///  New 
        /// </summary>
        WorkNew,
        /// <summary>
        ///  Work steps 
        /// </summary>
        WorkStep,
        /// <summary>
        ///  Work Pictures 
        /// </summary>
        WorkImages
    }
    public enum WorkProgress
    {
        /// <summary>
        ///  Normal operation 
        /// </summary>
        Runing,
        /// <summary>
        ///  Warning 
        /// </summary>
        Alert,
        /// <summary>
        ///  Overdue 
        /// </summary>
        Timeout
    }

}