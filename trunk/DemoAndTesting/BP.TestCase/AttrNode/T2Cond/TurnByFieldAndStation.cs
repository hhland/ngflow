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
    public class TurnByField : TestBase
    {
        /// <summary>
        ///  Test conditions for job orientation 
        /// </summary>
        public TurnByField()
        {
            this.Title = " Direction of the conditional test conditions and job orientation form fields ";
            this.DescIt = "以 002 Leave Process ( Control by post to ) Test object .";
            this.EditState = EditState.Passed;
        }
        /// <summary>
        ///  Explanation  : This test is targeted at the demo environment  002  Process writing unit test code .
        ///  Relates to :  Create , Send , Revocation , Direction of the condition , Return other functions .
        /// </summary>
        public override void Do()
        {
            string fk_flow = "002";
            Flow fl = new Flow(fk_flow);

            #region   zhoutianjiao  Log in .  Grassroots route 
            BP.WF.Dev2Interface.Port_Login("zhoutianjiao");

            // Create a blank ,  Initiating the start node .
            Int64 workid = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, WebUser.No, null, 0,null,0,null);

            // Performing transmission , Send and retrieve objects ,.
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid);
            if (objs.VarToNodeID != 299)
                throw new Exception("@ According to job conditions do wrong direction , Junior officers did not send to [ Department manager for approval ].");
            #endregion

            #region  Direction of the conditional test form fields .
            if (objs.VarAcceptersID != "qifenglin")
                throw new Exception("@ Did not let his department manager approval is not desired value .");

            // Login according to his manager .
            BP.WF.Dev2Interface.Port_Login(objs.VarAcceptersID);

            // Established form parameters .
            Hashtable ht = new Hashtable();
            ht.Add("QingJiaTianShu", 8);
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid, ht);
            if (objs.VarToNodeID != 204)
                throw new Exception("@ According to the form field to control the direction of the error , Less than 10 The day should people do resources department for approval .");

            //  Send revocation .
            BP.WF.Dev2Interface.Flow_DoUnSend(fl.No, workid);

            //  According to leave 15 Day Send .
            ht = new Hashtable();
            ht.Add("QingJiaTianShu", 15);
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid, ht);
            if (objs.VarToNodeID != 206)
                throw new Exception("@ According to the form field to control the direction of the error , Greater than or equal 10 The day should be allowed [ General manager approval ]");

            // Delete test data .
            BP.WF.Dev2Interface.Flow_DoDeleteFlowByReal(fl.No, objs.VarWorkID, false);
            #endregion

            #region  Let the middle  qifenglin Log in .
            BP.WF.Dev2Interface.Port_Login("qifenglin");
            workid = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, WebUser.No, null, 0, null, 0, null);

            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid);
            if (objs.VarToNodeID != 202)
                throw new Exception("@ According to job conditions do wrong direction , Middle-level staff did not send to [ General manager approval ] Node .");
            //  Delete test data .
            BP.WF.Dev2Interface.Flow_DoDeleteFlowByReal(fl.No, objs.VarWorkID, false);
            #endregion  Let the middle  qifenglin Log in .

            #region  Let rise  zhoupeng  Log in .
            BP.WF.Dev2Interface.Port_Login("zhoupeng");
            workid = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, WebUser.No, null, 0, null, 0, null);

            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid);
            if (objs.VarToNodeID != 203)
                throw new Exception("@ According to job conditions do wrong direction , Not sent to senior staff [ Human Resources for approval ] Node .");
            //delete it.
            BP.WF.Dev2Interface.Flow_DoDeleteFlowByReal(fl.No, objs.VarWorkID, false);
            #endregion
        }
    }
}
