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
    ///  Test developers parameters as steering condition 
    /// </summary>
    public class TurnByDevPara : TestBase
    {
        /// <summary>
        ///  Test developers parameters as steering condition 
        /// </summary>
        public TurnByDevPara()
        {
            this.Title = " Test developers parameters as steering condition ";
            this.DescIt = " In the test case -029 Process parameters as developer .";
            this.EditState = EditState.Passed;
        }
        /// <summary>
        ///  Carried out 
        /// </summary>
        public override void Do()
        {
            string fk_flow = "029";
            Flow fl = new Flow(fk_flow);
            fl.DoCheck();

            #region   zhoupeng  Log in .
            BP.WF.Dev2Interface.Port_Login("zhoupeng");
            // Create a blank ,  Initiating the start node .
            Int64 workid = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, WebUser.No, null, 0, null, 0, null);
            // Join the Developer parameters , There is no form TurnTo Field .
            Hashtable ht = new Hashtable();
            ht.Add("Turn", "A");

            // Performing transmission , Send and retrieve objects ,.
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid, ht);
            if (objs.VarToNodeID != 2999)
                throw new Exception("@ Should turn A.");

            // Delete test data .
            BP.WF.Dev2Interface.Flow_DoDeleteFlowByReal(fl.No, objs.VarWorkID, false);
            #endregion

            #region   zhoupeng  Log in .
            BP.WF.Dev2Interface.Port_Login("zhoupeng");
            // Create a blank ,  Initiating the start node .
            workid = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, WebUser.No, null, 0, null, 0, null);
            // Join the Developer parameters , There is no form TurnTo Field .
            ht = new Hashtable();
            ht.Add("Turn", "B");

            // Performing transmission , Send and retrieve objects ,.
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid, ht);
            if (objs.VarToNodeID != 2902)
                throw new Exception("@ Should turn B.");

            // Delete test data .
            BP.WF.Dev2Interface.Flow_DoDeleteFlowByReal(fl.No, objs.VarWorkID, false);
            #endregion
        }
    }
}
