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
    ///  Test hangs 
    /// </summary>
    public class TestHungUp : TestBase
    {
        /// <summary>
        ///  Test hangs 
        /// </summary>
        public TestHungUp()
        {
            this.Title = " Test hangs ";
            this.DescIt = " Process : 以demo  Process 023  A Case Study of Testing , Process Hangs , Lifted pending .";
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
        /// 1,  This process is targeted at the most simple division processes , zhanghaicheng Launch ,zhoushengyu,zhangyifan, Two people deal with child thread ,
        ///    zhanghaicheng  Accept the child thread summary data .
        /// 2,  Test method body is divided into three parts .  Launch , Sub-thread processing , Confluence execution , Respectively : Step1(), Step2_1(), Step2_2(),Step3()  Method .
        /// 3, For sending test , Does not relate to other features .
        /// </summary>
        public override void Do()
        {
            HungUp huEn = new HungUp();
            huEn.CheckPhysicsTable();

            this.fk_flow = "023";
            fl = new Flow("023");
            string sUser = "zhoupeng";
            BP.WF.Dev2Interface.Port_Login(sUser);

            // Create .
            workID = BP.WF.Dev2Interface.Node_CreateBlankWork(fl.No, null, null, WebUser.No, null, 0, null, 0, null);

            // Performing transmission .
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(fl.No, workID);

            // Let him landing .
            BP.WF.Dev2Interface.Port_Login(objs.VarAcceptersID);

            // Pending execution .
            BP.WF.Dev2Interface.Node_HungUpWork(fl.No, workID, 0, null, "hungup test");

            #region  Check the execution pending the expected results .
            GenerWorkFlow gwf = new GenerWorkFlow(this.workID);
            if (gwf.WFState != WFState.HungUp)
                throw new Exception("@ Should be suspended state , It is :" + gwf.WFStateText);

            GenerWorkerLists gwls = new GenerWorkerLists(workID, this.fk_flow);
            foreach (GenerWorkerList gwl in gwls)
            {
                if (gwl.FK_Node == fl.StartNodeID)
                    continue;

                if (gwl.DTOfHungUp.Length < 10)
                    throw new Exception("@ Pending the date is not written ");

                if (string.IsNullOrEmpty(gwl.DTOfUnHungUp) == false)
                    throw new Exception("@ Lifting suspend date should be empty , It is :" + gwl.DTOfUnHungUp);

                if (gwl.HungUpTimes != 1)
                    throw new Exception("@ Suspend the number should be １");
            }

            HungUp hu = new HungUp();
            hu.MyPK = "2302_" + this.workID;
            if (hu.RetrieveFromDBSources() == 0)
                throw new Exception("@ Not found 　HungUp　 Data .");

            #endregion  Check the execution pending the expected results 

            // Lifted pending .
            BP.WF.Dev2Interface.Node_UnHungUpWork(fl.No, workID, "un hungup test");

            #region  Check the contact execution pending the expected results .
            gwf = new GenerWorkFlow(this.workID);
            if (gwf.WFState != WFState.Runing)
                throw new Exception("@ Should be suspended state , It is :" + gwf.WFStateText);
            #endregion  Check the contact execution pending the expected results 

            // Suspend to perform multiple lifted pending .
            BP.WF.Dev2Interface.Node_HungUpWork(fl.No, workID, 0, null, "hungup test");
            BP.WF.Dev2Interface.Node_UnHungUpWork(fl.No, workID, "un hungup test");
        }
    }
}