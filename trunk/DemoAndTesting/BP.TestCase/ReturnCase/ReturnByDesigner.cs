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
    public class ReturnByDesigner : TestBase
    {
        /// <summary>
        ///  Test Press track return 
        /// </summary>
        public ReturnByDesigner()
        {
            this.Title = " Test Press track return ";
            this.DescIt = " Corresponding test cases  031 Process - Press return trajectory ";
            this.EditState = EditState.Passed;
        }
        /// <summary>
        ///  Carried out 
        /// </summary>
        public override void Do()
        {
            string fk_flow = "031";
            string startUser = "zhangyifan";

            Flow fl = new Flow(fk_flow);

            BP.WF.Dev2Interface.Port_Login(startUser);

            // Create a blank ,  Initiating the start node .
            Int64 workid = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, WebUser.No, null, 0,null,0,null);

            // Performing transmission , Send and retrieve objects ,.
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid);

            // 让  Under a worker Login .
            BP.WF.Dev2Interface.Port_Login(objs.VarAcceptersID);

            // So that the second node performs transmission .
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid);
            if (objs.VarAcceptersID != "zhoupeng")
                throw new Exception("@ Executives of this node should be zhoupeng.");

            // 让  A third worker Login .
            BP.WF.Dev2Interface.Port_Login(objs.VarAcceptersID);


            // Get set to return on the third node .
            DataTable dt = BP.WF.Dev2Interface.DB_GenerWillReturnNodes(objs.VarToNodeID, workid, 0);

            #region  Check that the second step to obtain return node data in line with expectations .
            if (dt.Rows.Count != 1)
                throw new Exception("@ In the first 3 Node is getting set to return nodes , Data do not meet expectations , Should only get a return node , It is :" + dt.Rows.Count);

            int nodeID = int.Parse(dt.Rows[0]["No"].ToString());
            if (nodeID != 3101)
                throw new Exception("@ In the first 3 Node is getting set to return nodes , The point should be returned 3101");
            string RecNo = dt.Rows[0]["Rec"].ToString();
            if (RecNo != startUser)
                throw new Exception("@ In the first 3 Node is getting set to return nodes , People should be returned " + startUser + ", It is " + RecNo);
            #endregion  Check that the second step to obtain return node data in line with expectations .

            // Implementation of return , Current node number .
            BP.WF.Dev2Interface.Node_ReturnWork(fk_flow, workid, 0, objs.VarToNodeID,3101, " Press the test track to return ", false);

            #region  Data integrity checks returned after .
            GenerWorkFlow gwf = new GenerWorkFlow(workid);
            if (gwf.WFState != WFState.ReturnSta)
                throw new Exception("@ Implementation of return , Process state should be returned , It is :" + gwf.WFState.ToString());

            if (gwf.FK_Node != 3101)
                throw new Exception("@ Implementation of return , The current node should be 101,  It is " + gwf.FK_Node.ToString());

            sql = "SELECT WFState from nd31rpt where oid=" + workid;
            int wfstate = DBAccess.RunSQLReturnValInt(sql, -1);
            if (wfstate != (int)WFState.ReturnSta)
                throw new Exception("@ In the first 3 After returning nodes rpt Incorrect Data , Process state should be returned , It is :" + wfstate);

            sql = "SELECT FlowEndNode from nd31rpt where oid=" + workid;
            int FlowEndNode = DBAccess.RunSQLReturnValInt(sql, -1);
            if (FlowEndNode != 3101)
                throw new Exception("@ In the first 3 After returning nodes rpt Incorrect Data , The last node should be 101, It is :" + FlowEndNode);
            #endregion  Data integrity checks returned after .


            // Remove this test data .
            BP.WF.Dev2Interface.Flow_DoDeleteFlowByReal(fl.No, objs.VarWorkID, false);

        }
    }
}
