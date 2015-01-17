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


namespace BP.CT.T2Cond
{
    /// <summary>
    ///  Conditions sector steering direction 
    /// </summary>
    public class TurnByDept : TestBase
    {
        /// <summary>
        ///  Test conditions for job orientation 
        /// </summary>
        public TurnByDept()
        {
            this.Title = " Test post , Department , Direction form field conditions ";
            this.DescIt = " In the test case -028 Sector direction conditions as the test case .";
            this.EditState = CT.EditState.Passed;
        }
        /// <summary>
        ///  Explanation  : This test is targeted at the demo environment  028  Process writing unit test code .
        ///  Relates to :  Direction in many ways steering function is available .
        /// </summary>
        public override void Do()
        {
            string fk_flow = "028";
            Flow fl = new Flow(fk_flow);

            #region   yangyilei  Log in .  Grassroots route 
            BP.WF.Dev2Interface.Port_Login("yangyilei");
            // Create a blank ,  Initiating the start node .
            Int64 workid = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, WebUser.No, null, 0, null, 0, null);

            // Performing transmission , Send and retrieve objects ,.
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid);
            if (objs.VarToNodeID != 2802)
                throw new Exception("@ Finance department staff transferred to the Finance Ministry did not initiate the node up .");

            // Delete test data .
            BP.WF.Dev2Interface.Flow_DoDeleteFlowByReal(fl.No, objs.VarWorkID, false);
            #endregion yangyilei

            #region  qifenglin Log in .
            BP.WF.Dev2Interface.Port_Login("qifenglin");
            workid = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, WebUser.No, null, 0, null, 0, null);

            objs = BP.WF.Dev2Interface.Node_SendWork(fl.No, workid);
            if (objs.VarToNodeID != 2899)
                throw new Exception("@ Develop Develop staff initiated did not turn up the node .");
            //  Delete test data .
            BP.WF.Dev2Interface.Flow_DoDeleteFlowByReal(fl.No, objs.VarWorkID, false);
            #endregion  qifenglin Log in .

            #region  liyan Log in .
            BP.WF.Dev2Interface.Port_Login("liyan");
            workid = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, WebUser.No, null, 0, null, 0, null);

            objs = BP.WF.Dev2Interface.Node_SendWork(fl.No, workid);
            if (objs.VarToNodeID != 2803)
                throw new Exception("@ HR did not turn up .");
            //  Delete test data .
            BP.WF.Dev2Interface.Flow_DoDeleteFlowByReal(fl.No, objs.VarWorkID, false);
            #endregion liyan Log in .
        }
    }
}
