using System;
using System.Data;
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
    ///  Testing endorsement 
    /// </summary>
    public class TestAskFor : TestBase
    {
        /// <summary>
        ///  Testing endorsement 
        /// </summary>
        public TestAskFor()
        {
            this.Title = " Testing endorsement ";
            this.DescIt = " Process : 以demo  Process 023  A Case Study of Testing , Endorsement of nodes , Send endorsement .";
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
        /// 1,  In the most simple three-node processes  023  Explanation .
        /// 2,  Testing endorsement of two modes 
        /// </summary>
        public override void Do()
        {
            // Mode 0, Plus sign after , Send people to the next step by endorsement .
            this.Mode0();

            // Mode 1, Plus sign after , Send by being added to the signature of person who signed .
            this.Mode1();
        }

        public void Mode0()
        {
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

            // Execution endorsement , And directly sent down .
            BP.WF.Dev2Interface.Node_Askfor(workID, AskforHelpSta.AfterDealSend, "liping", "askforhelp test");

            #region  Check the execution pending the expected results .
            GenerWorkFlow gwf = new GenerWorkFlow(this.workID);
            if (gwf.WFState != WFState.Askfor)
                throw new Exception("@ It should be a state endorsement , It is :" + gwf.WFStateText);

            if (gwf.FK_Node != objs.VarToNodeID)
                throw new Exception("@ Upcoming node processes should be (" + objs.VarToNodeID + "), It is :" + gwf.FK_Node);

            //  Get the current job list .
            GenerWorkerLists gwls = new GenerWorkerLists(objs.VarWorkID, objs.VarToNodeID);
            if (gwls.Count != 2)
                throw new Exception("@ There should be a list of two officers , People with the plus sign plus sign people , It is :" + gwls.Count + "个");

            string sql = "SELECT * FROM WF_GenerWorkerList WHERE FK_Emp='liping' AND WorkID=" + workID + " AND FK_Node=" + gwf.FK_Node;
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ People do not find work by endorsement .");

            //  Check the person status by endorsement .
            string var = dt.Rows[0][GenerWorkerListAttr.IsPass].ToString();
            if (var != "0")
                throw new Exception("@ Endorsement by people isPass State should be 0, Upcoming displayed ., It is :" + var);

            //  Check the person endorsement was work to be done .
            sql = "SELECT * FROM WF_EmpWorks WHERE FK_Emp='liping' AND WorkID=" + workID + " AND FK_Node=" + gwf.FK_Node;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ Was not found to be the work of people do endorsement .");
            #endregion  Check the execution pending the expected results 


            #region  Check the plus sign people 
            sql = "SELECT * FROM WF_GenerWorkerList WHERE FK_Emp='" + objs.VarAcceptersID + "' AND WorkID=" + workID + " AND FK_Node=" + gwf.FK_Node;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ Endorsement of people do not find work .");
            var = dt.Rows[0][GenerWorkerListAttr.IsPass].ToString();
            if (var != "5")
                throw new Exception("@ Endorsement by people isPass State should be 0, Upcoming displayed ., It is :" + var);
            #endregion  Check the plus sign people 

            // Let endorsement Login .
            BP.WF.Dev2Interface.Port_Login("liping");

            //  Let people perform the login endorsement .
            SendReturnObjs objsAskFor = BP.WF.Dev2Interface.Node_SendWork(fl.No, workID);

            #region  The results were performed to check for endorsement .
            gwf = new GenerWorkFlow(this.workID);
            if (gwf.WFState != WFState.Runing)
                throw new Exception("@ Should be the operating state , It is :" + gwf.WFStateText);

            if (gwf.FK_Node == objs.VarCurrNodeID)
                throw new Exception("@ It should be run to the next node , But it is still stuck on the current node :" + gwf.FK_Node);

            //  Check the person work plus sign .
            sql = "SELECT * FROM WF_EmpWorks WHERE FK_Emp='" + objs.VarAcceptersID + "' AND WorkID=" + workID + " AND FK_Node=" + gwf.FK_Node;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 0)
                throw new Exception("@ Should not be found , Endorsement of human work to be done ." + objs.VarAcceptersID);
            #endregion  The results were performed to check for endorsement .
        }

        /// <summary>
        ///  After the implementation of endorsement , Let be added to the previous , Send to a plus sign people .
        /// </summary>
        public void Mode1()
        {
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

            // Execution endorsement , And directly sent down .
            BP.WF.Dev2Interface.Node_Askfor(workID, AskforHelpSta.AfterDealSendByWorker, "liping", "askforhelp test");

            #region  Check the execution pending the expected results .
            GenerWorkFlow gwf = new GenerWorkFlow(this.workID);
            if (gwf.WFState != WFState.Askfor)
                throw new Exception("@ It should be a state endorsement , It is :" + gwf.WFStateText);

            if (gwf.FK_Node != objs.VarToNodeID)
                throw new Exception("@ Upcoming node processes should be (" + objs.VarToNodeID + "), It is :" + gwf.FK_Node);

            //  Get the current job list .
            GenerWorkerLists gwls = new GenerWorkerLists(objs.VarWorkID, objs.VarToNodeID);
            if (gwls.Count != 2)
                throw new Exception("@ There should be a list of two officers , People with the plus sign plus sign people , It is :" + gwls.Count + "个");

            string sql = "SELECT * FROM WF_GenerWorkerList WHERE FK_Emp='liping' AND WorkID=" + workID + " AND FK_Node=" + gwf.FK_Node;
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ People do not find work by endorsement .");

            //  Check the person status by endorsement .
            string var = dt.Rows[0][GenerWorkerListAttr.IsPass].ToString();
            if (var != "0")
                throw new Exception("@ Endorsement by people isPass State should be 0, Upcoming displayed ., It is :" + var);

            //  Check the person endorsement was work to be done .
            sql = "SELECT * FROM WF_EmpWorks WHERE FK_Emp='liping' AND WorkID=" + workID + " AND FK_Node=" + gwf.FK_Node;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ Was not found to be the work of people do endorsement .");
            #endregion  Check the execution pending the expected results 


            #region  Check the plus sign people 
            sql = "SELECT * FROM WF_GenerWorkerList WHERE FK_Emp='" + objs.VarAcceptersID + "' AND WorkID=" + workID + " AND FK_Node=" + gwf.FK_Node;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ Endorsement of people do not find work .");
            var = dt.Rows[0][GenerWorkerListAttr.IsPass].ToString();
            if (var != "6")
                throw new Exception("@ Endorsement of people isPass State should be 6, It is :" + var);
            #endregion  Check the plus sign people 

            // Let endorsement Login .
            BP.WF.Dev2Interface.Port_Login("liping");

            //  Let people perform the login endorsement .
            SendReturnObjs objsAskFor = BP.WF.Dev2Interface.Node_SendWork(fl.No, workID);

            #region  The results were performed to check for endorsement .
            gwf = new GenerWorkFlow(this.workID);
            if (gwf.WFState != WFState.Askfor)
                throw new Exception("@ It should be a plus sign state , It is :" + gwf.WFStateText);

            if (gwf.FK_Node != objsAskFor.VarToNodeID)
                throw new Exception("@ Should not run to the next node , Now run to the :" + gwf.FK_Node);

            //  Check the person work plus sign .
            sql = "SELECT * FROM WF_EmpWorks WHERE FK_Emp='" + objs.VarAcceptersID + "' AND WorkID=" + workID + " AND FK_Node=" + gwf.FK_Node;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
                throw new Exception("@ Should not endorse people can not find work to be done :" + objs.VarAcceptersID);
            #endregion  The results were performed to check for endorsement .
        }
    }
}
