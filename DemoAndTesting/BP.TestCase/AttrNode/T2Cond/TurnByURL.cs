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
    ///  Test  按URL  As the steering condition 
    /// </summary>
    public class TurnByURL : TestBase
    {
        /// <summary>
        ///  Test  按URL  As the steering condition 
        /// </summary>
        public TurnByURL()
        {
            this.Title = " Test  按URL  As the steering condition ";
            this.DescIt = " In the test case -075 Process to make a test case .";
            this.EditState = EditState.Passed;
        }
        /// <summary>
        ///  Carried out 
        /// </summary>
        public override void Do()
        {
            string fk_flow = "075";
            Flow fl = new Flow(fk_flow);

            #region   zhoupeng  Log in .
            BP.WF.Dev2Interface.Port_Login("zhoushengyu");
            // Create a blank ,  Initiating the start node .
            Int64 workid = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, WebUser.No, null, 0, null,0,null);

            // Join  按URL , There is no form TurnTo Field .
            Hashtable ht = new Hashtable();
            ht.Add("ToNodeID", "7502");
            SendReturnObjs objs = null;

            // Performing transmission , Send and retrieve objects ,  Should come out abnormal fishes , Because pass this parameter causes sql An error .
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid, ht);

            if (objs.VarToNodeID != 7502)
                throw new Exception("@ Should turn 7502 ");

            // Delete test data .
            BP.WF.Dev2Interface.Flow_DoDeleteFlowByReal(fl.No, objs.VarWorkID, false);

            // Completion of the testing process conditions .
            workid = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, WebUser.No, null, 0, null, 0, null);
            ht = new Hashtable();
            ht.Add("ToNodeID", "");
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid, ht);
            if (objs.IsStopFlow == false)
                throw new Exception("@ Process should end , But is not the end .");
            #endregion
        }
    }
}
