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
    ///  Test  按SQL  As the steering condition 
    /// </summary>
    public class TurnBySQL : TestBase
    {
        /// <summary>
        ///  Test  按SQL  As the steering condition 
        /// </summary>
        public TurnBySQL()
        {
            this.Title = " Test  by SQL  As the steering condition ";
            this.DescIt = " In the test case -030 Process to make a test case .";
            this.EditState = EditState.Passed;
        }
        /// <summary>
        ///  Carried out 
        /// </summary>
        public override void Do()
        {
            string fk_flow = "030";
            Flow fl = new Flow(fk_flow);

            #region   zhoupeng  Log in .
            BP.WF.Dev2Interface.Port_Login("zhoupeng");
            // Create a blank ,  Initiating the start node .
            Int64 workid = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, WebUser.No, null, 0, null,0,null);

            // Join  按SQL , There is no form TurnTo Field .
            Hashtable ht = new Hashtable();
            ht.Add("MyPara", "qqqq");
            SendReturnObjs objs = null;
            try
            {
                // Performing transmission , Send and retrieve objects ,  Should come out abnormal fishes , Because pass this parameter causes sql An error .
                objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid, ht);
            }
            catch (Exception ex)
            {
                Log.DefaultLogWriteLineInfo(" Has been detected SQL The variable has been replaced correctly , Cause this statement fails :" + ex.Message);
            }
            ht.Clear();
            ht.Add("MyPara", "1");
            // Performing transmission , Send and retrieve objects ,  Should come out abnormal fishes , Because pass this parameter causes sql An error .
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid, ht);
            if (objs.VarToNodeID != 3002)
                throw new Exception("@ Should turn B.");

            // Delete test data .
            BP.WF.Dev2Interface.Flow_DoDeleteFlowByReal(fl.No, objs.VarWorkID, false);
            #endregion
        }
    }
}
