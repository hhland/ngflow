using System;
using System.Threading;
using System.Collections;
using System.Data;
using BP.DA;
using BP.DTS;
using BP.En;
using BP.Web.Controls;
using BP.Web;

namespace BP.Sys
{
    /// <summary>
    ///  Event Demo
    /// </summary>
    abstract public class EventDemo:EventBase
    {
        #region  Property .
        #endregion  Property .

        /// <summary>
        ///  Event Demo
        /// </summary>
        public EventDemo()
        {
            this.Title = " Event demo Implementation of demonstration .";
        }
        /// <summary>
        ///  Execution events 
        /// 1, If an error is thrown Information , Reception interface will prompt an error does not execute down .
        /// 2, Successful implementation , Assigned to the implementation of the results SucessInfo Variable , If you do not need tips on assignment or for empty null.
        /// 3, All parameters are available from   this.SysPara.GetValByKey Get .
        /// </summary>
        public override void Do()
        {
            if (1 == 2)
                throw new Exception("@ Execution error xxxxxx.");


            // If you want to prompt the user to perform successful information , Give him an assignment , Otherwise, you do not have an assignment .
            this.SucessInfo = " Successful implementation tips .";
        }
    }
}
