using System;
using System.Collections.Generic;
using System.Linq;
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
    ///  Send object 
    /// </summary>
    public class SendObjs : TestBase
    {
        /// <summary>
        ///  Send object 
        /// </summary>
        public SendObjs()
        {
            this.Title = " Send object ";
            this.DescIt = " Process : 023  Performing transmission , Send object to see whether the capacity to meet the requirements .";
            this.EditState = CT.EditState.Passed;
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
            BP.WF.Dev2Interface.Port_Login(userNo);

            // Create a job .
            this.workID = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, userNo, null);
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, this.workID, null, null, 0, null);


            #region  Send check whether the results in line with expectations .
            string msgText = objs.ToMsgOfText();
            if (msgText.Contains("<"))
                throw new Exception("text Information ,  Have html Mark .");


             
            #endregion  Send check whether the results in line with expectations .

        }
    }
}
