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
    ///  Read Receipts 
    /// </summary>
    public class ReadReceipts : TestBase
    {
        /// <summary>
        ///  Read Receipts 
        /// </summary>
        public ReadReceipts()
        {
            this.Title = " Read Receipts ";
            this.DescIt = "036  Read Receipts - Leave Process  ";
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
        ///  Test Case  
        /// </summary>
        public override void Do()
        {
            // Initialize variables .
            fk_flow = "036";
            userNo = "zhangyifan";
            fl = new Flow(fk_flow);

            // Execution landing .
            BP.WF.Dev2Interface.Port_Login(userNo);

            //  Creating work .
            this.workID = BP.WF.Dev2Interface.Node_CreateBlankWork(this.fk_flow, null, null, null, null);
            //  Sent to the next step ( Department manager for approval ).
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(this.fk_flow, this.workID, null,null, 0, null);

            //  Get the next step of the reception staff , Let the next person landing .
            string nextWorker2 = objs.VarAcceptersID;
            BP.WF.Dev2Interface.Port_Login(nextWorker2);

            //  Read the work of the executive branch manager api.
            BP.WF.Dev2Interface.Node_SetWorkRead(objs.VarToNodeID, this.workID);

            #region  Check whether the sponsor receives a receipt message .
            if (BP.WF.Glo.IsEnableSysMessage == true)
            {
                string sql = "SELECT * FROM Sys_SMS WHERE MsgFlag='RP" + this.workID + "_3602' AND " + SMSAttr.SendTo + "='" + userNo + "'";
                DataTable dt = DBAccess.RunSQLReturnTable(sql);
                if (dt.Rows.Count == 0)
                    throw new Exception("@ Staff (" + userNo + ") Does not receive acknowledgment messages .");
            }
            #endregion  Check whether the sponsor receives a receipt message .

            //  Let first 2 Step personnel sent ,  Send and retrieve objects .
            Hashtable ht = new Hashtable();
            ht.Add(BP.WF.WorkSysFieldAttr.SysIsReadReceipts, "1"); // Incoming form parameters .
            objs = BP.WF.Dev2Interface.Node_SendWork(this.fk_flow, this.workID,
                ht, null, 0, null);

            string nextWorker3 = objs.VarAcceptersID; // The third step to obtain the recipient .
            BP.WF.Dev2Interface.Port_Login(nextWorker3); //  The third step to allow staff login .
            BP.WF.Dev2Interface.Node_SetWorkRead(objs.VarToNodeID, this.workID); // A read receipt .

            #region  Check the department manager whether to accept a read receipt .
            if (BP.WF.Glo.IsEnableSysMessage == true)
            {
                string sql = "SELECT * FROM Sys_SMS WHERE MsgFlag='RP" + this.workID + "_3699' AND " + SMSAttr.SendTo + "='" + nextWorker2 + "'";
                DataTable dt = DBAccess.RunSQLReturnTable(sql);
                if (dt.Rows.Count == 0)
                    throw new Exception("@ Staff (" + nextWorker2 + ") Does not receive acknowledgment messages .");
            }
            #endregion  Check the department manager whether to accept a read receipt .
        }
    }
}
