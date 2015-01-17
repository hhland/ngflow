using System;
using System.Threading;
using System.Collections;
using BP.Web.Controls;
using System.Data;
using BP.DA;
using BP.DTS;
using BP.En;
using BP.Web;
using BP.Sys;
using BP.WF;

namespace BP.FlowEvent
{
    /// <summary>
    ///  Reimbursement process 001
    /// </summary>
    public class F001:BP.WF.FlowEventBase
    {
        #region  Property .
        /// <summary>
        ///  Process re-mark 
        /// </summary>
        public override string FlowMark
        {
            get { return "BaoXiao";   }
        }
        #endregion  Property .

        #region  Structure .
        /// <summary>
        ///  Reimbursement process events 
        /// </summary>
        public F001()
        {
        }
        #endregion  Property .


        /// <summary>
        ///  Rewrite before sending event 
        /// </summary>
        /// <returns></returns>
        public override string SendWhen()
        {
            switch (this.HisNode.NodeID)
            {
                case 101:
                    string empstr = "zhangsan";
                    //// Total lowercase update  ,  The total conversion to uppercase .
                    //string sql = "UPDATE ND101 SET JIeshourn='" + empstr + "'  WHERE OID=" + this.OID;
                    //BP.DA.DBAccess.RunSQL(sql);
                    ////this.ND101_SaveAfter();
                    break;
                default:
                    break;
            }
            return base.SendWhen();
        }

        /// <summary>
        ///  After executing the save event 
        /// </summary>
        /// <returns></returns>
        public override string SaveAfter()
        {
            switch (this.HisNode.NodeID)
            {
                case 101:
                    this.ND101_SaveAfter();
                    break;
                default:
                    break;
            }
            return base.SaveAfter();
        }
        /// <summary>
        ///  After the incident node holds 
        ///  Methods for naming :ND+ Node name _ Event name .
        /// </summary>
        public void ND101_SaveAfter()
        {
            // Calculated the total list of .
            float hj = BP.DA.DBAccess.RunSQLReturnValFloat("SELECT HeJi FROM ND101 WHERE OID=" + this.OID, 0);

            // Total lowercase update  ,  The total conversion to uppercase .
            string sql = "UPDATE ND101 SET DaXie='" + BP.DA.DataType.ParseFloatToCash(hj) + "',HeJi="+hj+"  WHERE OID=" + this.OID;
            BP.DA.DBAccess.RunSQL(sql);

            //if (1 == 2)
            //    throw new Exception("@ Execution error xxxxxx.");
            // If you want to prompt the user to perform successful information , Give him an assignment , Otherwise, you do not have an assignment .
            //this.SucessInfo = " Successful implementation tips .";
        }
        /// <summary>
        ///  Send a successful event , When sent successfully , The process Upcoming write to other systems .
        /// </summary>
        /// <returns> Return to the results , If the return null No tips .</returns>
        public override string SendSuccess()
        {
            try
            {
                //  Organize the necessary variables .
                Int64 workid = this.WorkID; //  The work id.
                string flowNo = this.HisNode.FK_Flow; //  Process ID .
                int currNodeID = this.SendReturnObjs.VarCurrNodeID; // The current node id
                int toNodeID = this.SendReturnObjs.VarToNodeID; //  Arrives at a node id.
                string toNodeName = this.SendReturnObjs.VarToNodeName; //  Arrives at a node name .
                string acceptersID = this.SendReturnObjs.VarAcceptersID; //  Acceptance of staff id,  More than one person will use   Look comma  , Such as  zhangsan,lisi.
                string acceptersName = this.SendReturnObjs.VarAcceptersName; //  Accept a person name , More than one person will be separated by commas, such as : Joe Smith , John Doe .

                // Upcoming perform write to other systems .
                /*
                 *  Here you need to write your business logic , According to the organization of the above variables .
                 */

                // Return .
                return base.SendSuccess();
            }
            catch(Exception ex)
            {
                throw new Exception(" Upcoming write to other systems fail , Details :"+ex.Message);
            }
        }
    }
}
