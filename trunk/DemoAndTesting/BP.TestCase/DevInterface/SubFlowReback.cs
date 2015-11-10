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
    ///  Sub-processes rollback 
    /// </summary>
    public class SubFlowReback : TestBase
    {
        /// <summary>
        ///  Sub-processes rollback 
        /// </summary>
        public SubFlowReback()
        {
            this.Title = " Sub-processes rollback ";
            this.DescIt = "以023,024 Process to test the sub-processes of rollback ";
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
        public Int64 workid = 0;
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
        ///  Sub-processes rollback :
        /// 1, 把023 Do the parent process ,024 As sub-processes .
        /// 2,  After lifting the child process parent process , Sub-process is completed , Let rollback , Check whether the data in line with expectations .
        /// </summary>
        public override void Do()
        {
            // Initialize variables .
            fk_flow = "005";
            userNo = "zhanghaicheng";
            fl = new Flow(fk_flow);

            // 让 zhanghaicheng   Log in .
            BP.WF.Dev2Interface.Port_Login(userNo);

            //  Create .
            this.workid = BP.WF.Dev2Interface.Node_CreateBlankWork(this.fk_flow, null, null, WebUser.No, "parent flow");

            // Sent to the next step , Or let zhoupeng  Deal with .
            BP.WF.Dev2Interface.Node_SendWork(this.fk_flow, this.workid, null, null, 0, "zhoupeng");

            // Create a child process ,让 zhoushengyu  As sponsor .
            Int64 subWorkID = BP.WF.Dev2Interface.Node_CreateStartNodeWork("024", null, null, "zhoushengyu", "sub flow", this.workid, this.fk_flow,0);

            //  Let the child to the end of the implementation process .
            LetSubFlowRunOver(subWorkID);

            // 让 zhoupeng   Log in , Start monitoring sub-processes .
            BP.WF.Dev2Interface.Port_Login("zhoupeng");

            // A rollback to the last node .
            BP.WF.Dev2Interface.Flow_DoRebackWorkFlow("024", subWorkID, 2401, "test reback");

            //  Let the child process execution to the end .
            LetSubFlowRunOver(subWorkID);


            // Repeat , Check if there is a problem ?  To the last node in the implementation of rollback .
            BP.WF.Dev2Interface.Flow_DoRebackWorkFlow("024", subWorkID, 2401, "test reback");

            //  Let the child process execution to the end .
            LetSubFlowRunOver(subWorkID);

        }
        /// <summary>
        ///  Let the child process runs to the end .
        /// </summary>
        public void LetSubFlowRunOver(Int64 subWorkID)
        {
            #region  The operator sub-process of the first node .
            // 让 zhoushengyu  Log in .
            BP.WF.Dev2Interface.Port_Login("zhoushengyu");

            // Send this sub-process ,到 zhangyifan,  Send up to the second node .
            BP.WF.Dev2Interface.Node_SendWork("024", subWorkID, null, null, 0, "zhangyifan");
            #endregion  The operator sub-process of the first node .


            #region  The operator of the second sub-process node .
            // 让 zhangyifan  Log in .
            BP.WF.Dev2Interface.Port_Login("zhangyifan");

            // Send this sub-process ,到 zhoutianjiao.
            BP.WF.Dev2Interface.Node_SendWork("024", subWorkID, null, null, 0, "zhoutianjiao");
            #endregion  The operator of the second sub-process node .


            #region  The third sub-process operator nodes , Now the sub-process is completed .
            // 让 zhoutianjiao  Log in .
            BP.WF.Dev2Interface.Port_Login("zhoutianjiao");

            // Send this sub-process ,到 zhoutianjiao.
            BP.WF.Dev2Interface.Node_SendWork("024", subWorkID, null, null, 0, "zhoutianjiao");
            #endregion  The third sub-process operator nodes , Now the sub-process is completed .
        }
    }
}
