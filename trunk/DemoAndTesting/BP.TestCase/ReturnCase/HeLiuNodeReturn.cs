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

namespace BP.CT.ReturnCase
{
    public class HeLiuNodeReturn : TestBase
    {
        /// <summary>
        ///  Test confluence node return 
        /// </summary>
        public HeLiuNodeReturn()
        {
            this.Title = " Test confluence node returned to the child thread ";
            this.DescIt = "以demo The  FlowNo=005  Monthly sales summary ( With sub-confluent form ),  To test case .";
            this.EditState = EditState.Passed;
        }

        #region  Variable 
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
        ///  Explanation  :以demo The  FlowNo=005  Monthly sales summary ( With sub-confluent form ),  To test case .
        ///  Relates to :  Create , Send , Revocation , Direction of the condition , Return other functions .
        /// </summary>
        public override void Do()
        {
            //BP.WF.ClearDB cd = new ClearDB();
            //cd.Do();

            //  To the global variable assignment .
            fk_flow = "005";
            userNo = "zhanghaicheng";
            fl = new Flow(fk_flow);

            //  Let sponsor Login .
            BP.WF.Dev2Interface.Port_Login(userNo);

            // Create a blank ,  Initiating the start node .
            workID = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, WebUser.No, null, 0, null, 0, null);

            // Performing transmission , Send and retrieve objects .
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workID);

            #region  Send the child thread  zhangyifan
            //  After confluence send , Send sub-thread execution point .
            BP.WF.Dev2Interface.Port_Login("zhangyifan");

            // Obtain child thread ID.
            Int64 threadWorkID1 = this.GetThreadID(workID);

            //  Performing transmission ,  So he sent up to the confluence of the node .
            BP.WF.Dev2Interface.Node_SendWork(fk_flow, threadWorkID1);
            #endregion  Send the child thread  zhangyifan

            #region  Send the child thread  zhoushengyu
            //  After confluence send , Send sub-thread execution point .
            BP.WF.Dev2Interface.Port_Login("zhoushengyu");

            // Obtain child thread ID.
            Int64 threadWorkID2 = this.GetThreadID(workID);

            //  Performing transmission ,  So he sent up to the confluence of the node .
            BP.WF.Dev2Interface.Node_SendWork(fk_flow, threadWorkID2);
            #endregion  Send the child thread  zhoushengyu

            //  Let sponsor Login .
            BP.WF.Dev2Interface.Port_Login(userNo);

            //  Return the child thread execution 1.
            BP.WF.Dev2Interface.Node_ReturnWork(fl.No, threadWorkID1, workID, 599, 502, "test msg1", false);

            #region  Check the sub-thread data is correct ?
            gwf = new GenerWorkFlow(threadWorkID1);
            if (gwf.FK_Node != 502)
                throw new Exception("@ Child thread does not comply with the expected return node , It is :"+gwf.FK_Node);

            if (gwf.FID != workID)
                throw new Exception("@ After returning the child node threads  WF_GenerWorkFlow  On  FID  Lose , It is :" + gwf.FID);

            gwl = new GenerWorkerList(threadWorkID1, 502, "zhoushengyu");
            if (gwl.IsPass == true)
                throw new Exception("@ State child thread should not have been passed .");
            #endregion  Check the sub-thread data is correct ?

            //  Return the child thread execution 2.
            BP.WF.Dev2Interface.Node_ReturnWork(fl.No, threadWorkID2, workID, 599, 502, "test msg2", false);

            #region  Check the sub-thread data is correct ?
            gwf = new GenerWorkFlow(threadWorkID2);
            if (gwf.FK_Node != 502)
                throw new Exception("@ Child thread does not comply with the expected return node , It is :" + gwf.FK_Node);

            if (gwf.FID != workID)
                throw new Exception("@ After returning the child node threads  WF_GenerWorkFlow  On  FID  Lose , It is :" + gwf.FID);

            gwl = new GenerWorkerList(threadWorkID1, 502, "zhangyifan");
            if (gwl.IsPass == true)
                throw new Exception("@ State child thread should not have been passed .");
            #endregion  Check the sub-thread data is correct ?
        }

        private Int64 GetThreadID(Int64 workID)
        {
            // Get it Upcoming ,  In order to gain the child thread id.
            var dt = Dev2Interface.DB_GenerEmpWorksOfDataTable(WebUser.No, WFState.Runing, fk_flow);
            foreach (DataRow dr in dt.Rows)
            {
                Int64 fid = Int64.Parse(dr["FID"].ToString());
                if (fid != workID)
                    continue;

                return Int64.Parse(dr["WorkID"].ToString());
            }
            throw new Exception("@ Not found .");
        }
    }
}
