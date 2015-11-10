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
    ///  Testing by department staff to find 
    /// </summary>
    public class FindByDept : TestBase
    {
        /// <summary>
        ///  Testing by department staff to find 
        /// </summary>
        public FindByDept()
        {
            this.Title = " Testing by department staff to find ";
            this.DescIt = " Process : 以demo  Process  064: Someone Rules ( Looking for leadership )  A Case Study of Testing .";
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
        /// 2,  Testing by department staff to find two modes 
        /// </summary>
        public override void Do()
        {
            this.fk_flow = "064";
            fl = new Flow(this.fk_flow);
            string sUser = "zhoupeng";
            BP.WF.Dev2Interface.Port_Login(sUser);

            // Create .
            workID = BP.WF.Dev2Interface.Node_CreateBlankWork(fl.No, null, null, WebUser.No, null, 0, null, 0, null);

            // Performing transmission ,  According to find a job 
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(fl.No, workID, null, 6499, null);

            #region  Analysis of expected 
            if (objs.VarAcceptersID != "zhanghaicheng")
                throw new Exception("@ In accordance with their duties to find errors ,  It should be zhanghaicheng It is " + objs.VarAcceptersID);
            #endregion

            // Send perform revocation , According to find jobs .
            BP.WF.Dev2Interface.Flow_DoUnSend(fl.No, workID);
            objs = BP.WF.Dev2Interface.Node_SendWork(fl.No, workID, null, 6402, null);

            #region  Analysis of expected 
            if (objs.VarAcceptersID != "zhanghaicheng")
                throw new Exception("@ In accordance with their duties to find errors ,  It should be zhanghaicheng It is " + objs.VarAcceptersID);
            #endregion

            // Send perform revocation , Find all personnel department .
            BP.WF.Dev2Interface.Flow_DoUnSend(fl.No, workID);
            objs = BP.WF.Dev2Interface.Node_SendWork(fl.No, workID, null, 6403, null);

            #region  Analysis of expected 
            if (objs.VarAcceptersID != "zhoupeng")
                throw new Exception("@ In accordance with their duties to find errors ,  It should be zhoupeng It is " + objs.VarAcceptersID);
            #endregion

        }
    }
}
