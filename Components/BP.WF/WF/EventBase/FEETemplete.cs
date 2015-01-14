using System;
using System.Collections;
using System.Data;
using BP.DA;
using BP.En;
using BP.Web;
using BP.Sys;
using BP.WF;
using BP.Port;

namespace BP.Demo
{
    /// <summary>
    /// xxxxxx  Process events entity 
    /// </summary>
    public class FEETemplete : BP.WF.FlowEventBase
    {
        #region  Structure .
        /// <summary>
        /// xxxxxx  Process events entity 
        /// </summary>
        public FEETemplete()
        {
        }
        #endregion  Structure .

        #region  Overriding Properties .
        public override string FlowMark
        {
            get { return "Templete"; }
        }
        #endregion  Overriding Properties .

        #region  Rewrite Process sports events .
        /// <summary>
        ///  Deleted 
        /// </summary>
        /// <returns></returns>
        public override string AfterFlowDel()
        {
            return null;
        }
        /// <summary>
        ///  Delete ago 
        /// </summary>
        /// <returns></returns>
        public override string BeforeFlowDel()
        {
            return null;
        }
        /// <summary>
        ///  After 
        /// </summary>
        /// <returns></returns>
        public override string FlowOverAfter()
        {
            throw new Exception("@ Has been called up to the end of the event .");
            return null;
        }
        /// <summary>
        ///  Before the end of 
        /// </summary>
        /// <returns></returns>
        public override string FlowOverBefore()
        {
            return null;
        }
        #endregion  Rewrite Process sports events 

        #region  Nodes form events 
        /// <summary>
        ///  Form before loading 
        /// </summary>
        public override string FrmLoadAfter()
        {
            return null;
        }
        /// <summary>
        ///  After loading the form 
        /// </summary>
        public override string FrmLoadBefore()
        {
            return null;
        }
        #endregion


        #region  Rewrite node movement events .

        public override string SaveBefore()
        {
            return null;
        }
        public override string SaveAfter()
        {
            return null;
        }
        public override string SendWhen()
        {
            return null;
        }
        public override string SendSuccess()
        {
            return null;
        }
        public override string SendError()
        {
            return null;
        }
        public override string ReturnAfter()
        {
            return null;
        }
        public override string ReturnBefore()
        {
            return null;
        }
        public override string UndoneAfter()
        {
            return null;
        }
        public override string UndoneBefore()
        {
            return null;
        }
        #endregion  Rewrite node movement events 
    }
}
