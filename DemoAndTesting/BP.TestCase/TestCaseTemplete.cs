using System;
using System.Collections.Generic;
using System.Text;
using BP.WF;
using BP.En;
using BP.DA;
using BP.Web;
using System.Data;
using System.Collections;
using BP.CT;

namespace BP.CT.Model
{
    /// <summary>
    ///  Test Name 
    /// </summary>
    public class TestCaseTemplete : TestBase
    {
        /// <summary>
        ///  Test Name 
        /// </summary>
        public TestCaseTemplete()
        {
            this.Title = " Test Name ";
            this.DescIt = " Process : 005 Monthly sales summary ( With sub-confluent form ), Whether the data is sent to perform in line with expectations after .";
            this.EditState = CT.EditState.UnOK;
        }

        #region  Global Variables 
        /// <summary>
        ///  Process ID 
        /// </summary>
        public string fk_flow = "";
        /// <summary>
        ///  User ID 
        /// </summary>
        public string userNo = "";
        /// <summary>
        ///  All processes 
        /// </summary>
        public Flow fl = null;
        /// <summary>
        ///  The main thread ID
        /// </summary>
        public Int64 workID = 0;
        /// <summary>
        ///  After sending the return object 
        /// </summary>
        public SendReturnObjs objs = null;
        /// <summary>
        ///  Staff List 
        /// </summary>
        public GenerWorkerList gwl = null;
        /// <summary>
        ///  Process Registry 
        /// </summary>
        public GenerWorkFlow gwf = null;
        #endregion  Variable 

        /// <summary>
        ///  Test Case Description :
        /// 1, .
        /// 2, .
        /// 3,.
        /// </summary>
        public override void Do()
        {
            // Initialize variables .
            fk_flow = "023";
            userNo = "zhanghaicheng";
            fl = new Flow(fk_flow);
        }
    }
}
