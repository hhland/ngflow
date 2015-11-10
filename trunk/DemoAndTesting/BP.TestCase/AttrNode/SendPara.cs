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
    ///  Send parameters 
    /// </summary>
    public class SendPara : TestBase
    {
        /// <summary>
        ///  Send parameters 
        /// </summary>
        public SendPara()
        {
            this.Title = " Send parameters ";
            this.DescIt = " Process : 023  Performing transmission , Look yield parameters to meet the requirements .";
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

            Hashtable ht=new Hashtable();
            ht.Add(BP.WF.WorkSysFieldAttr.SysSDTOfNode,"2020-12-01 08:00"); // A node completion time .
            ht.Add(BP.WF.WorkSysFieldAttr.SysSDTOfFlow,"2020-12-31 08:00"); // The time required to complete the overall process ..
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, this.workID, ht, null, 0, null);

            #region  Send check whether the results in line with expectations .
            //sql = "SELECT "+GenerWorkFlowAttr.SDTOfFlow+","+GenerWorkFlowAttr.SDTOfNode+" FROM WF_GenerWorkFlow WHERE WorkID="+this.workID;
            GenerWorkFlow gwf = new GenerWorkFlow(this.workID);
            if (gwf.SDTOfFlow != "2020-12-31 08:00")
                throw new Exception("@ Write process should be completed no time , The time now is should be completed :"+gwf.SDTOfFlow);

            if (gwf.SDTOfNode != "2020-12-01 08:00")
                throw new Exception("@ Write node should be completed no time , The time now is should be completed :" + gwf.SDTOfNode);

            GenerWorkerLists gwls = new GenerWorkerLists(this.workID, 2302);
            foreach (GenerWorkerList gwl in gwls)
            {
                if (gwl.SDT != "2020-12-01 08:00")
                    throw new Exception("@ No time to write node should be completed .");
            }
            #endregion  Send check whether the results in line with expectations .

        }
    }
}
