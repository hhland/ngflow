using System;
using System.Collections.Generic;
using System.Text;

namespace BP.WF
{
    /// <summary>
    ///  Process operation list 
    /// </summary>
    public class FlowOpList
    {
        /// <summary>
        ///  Reminders 
        /// </summary>
        public const string PressTimes = "PressTimes";
        /// <summary>
        ///  Forcibly remove 
        /// </summary>
        public const string FlowOverByCoercion = "FlowOverByCoercion.";
        /// <summary>
        ///  Send revocation 
        /// </summary>
        public const string UnSend = "UnSend";
        /// <summary>
        ///  Forced transfer 
        /// </summary>
        public const string ShiftByCoercion = "ShiftByCoercion";
        /// <summary>
        ///  Pending revocation 
        /// </summary>
        public const string UnHungUp = "UnHungUp";
        /// <summary>
        ///  Pending 
        /// </summary>
        public const string HungUp = "HungUp";
    }
}
