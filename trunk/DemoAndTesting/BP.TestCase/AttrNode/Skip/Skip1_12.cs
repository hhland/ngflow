using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using BP.WF;
using BP.En;
using BP.DA;
using BP.Web;
using System.Collections;
using BP.CT;

namespace BP.CT.Model
{
    /// <summary>
    ///  Test Jump Rules 1
    /// </summary>
    public class Skip1_12 : TestBase
    {
        /// <summary>
        ///  Test Jump Rules 
        /// </summary>
        public Skip1_12()
        {
            this.Title = " Test Jump Rules 1-12  Consecutive jump 2 Nodes .";
            this.DescIt = " Process : 以demo  Process  057: Test Jump Rules , Consecutive jump 2 Nodes .";
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
        /// 1,  Were cited 4种.
        /// 2,  Test to find the leadership of the two modes 
        /// </summary>
        public override void Do()
        {
            this.fk_flow = "057";
            fl = new Flow(this.fk_flow);
            string sUser = "zhoupeng";
            BP.WF.Dev2Interface.Port_Login(sUser);

            // Create .
            workID = BP.WF.Dev2Interface.Node_CreateBlankWork(fl.No, null, null, WebUser.No, null, 0, null, 0, null);

            // Performing transmission ,  Should jump up to the last step .
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(fl.No, workID);

            #region  Analysis of expected 
            if (objs.VarAcceptersID != "qifenglin")
                throw new Exception("@ It should be qifenglin It is " + objs.VarAcceptersID);

            if (objs.VarToNodeID != 5799)
                throw new Exception("@ Should jump to the last node , But running the :"+objs.VarToNodeID+" - "+objs.VarToNodeName);
            #endregion
        }
         
    }
}
