using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BP.WF
{
    public class DoTypeConst
    {
        public const string OpenDoc = "OpenDoc";
        /// <summary>
        ///  Start the process 
        /// </summary>
        public const string DoStartFlowByTemple = "DoStartFlowByTemple";
        /// <summary>
        ///  Edit draft 
        /// </summary>
        public const string DoStartFlow = "DoStartFlow";
        /// <summary>
        ///  Open Process 
        /// </summary>
        public const string OpenFlow = "OpenFlow";
        /// <summary>
        ///  Send 
        /// </summary>
        public const string Send = "Send";
        /// <summary>
        ///  Undo Send 
        /// </summary>
        public const string UnSend = "UnSend";
        /// <summary>
        ///  Delete Process 
        /// </summary>
        public const string DelFlow = "DelFlow";
    }
    /// <summary>
    ///  Start tag 
    /// </summary>
    public class StartFlag
    {
        public const string DoNewFlow = "DoNewFlow";
        public const string DoOpenFlow = "DoOpenFlow";
        public const string DoOpenDoc = "DoOpenDoc";


    }
}
