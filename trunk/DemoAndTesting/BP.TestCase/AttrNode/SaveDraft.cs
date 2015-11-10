using System;
using System.Collections.Generic;
using System.Text;
using BP.WF;
using BP.WF.Template;
using BP.WF.Data;
using BP.En;
using BP.DA;
using BP.Web;
using System.Data;
using System.Collections;
using BP.CT;

namespace BP.CT.NodeAttr
{
    public  class SaveDraft : TestBase
    {
        /// <summary>
        ///  Save Draft - Save Draft 
        /// </summary>
        public SaveDraft()
        {
            this.Title = " Save Draft ";
            this.DescIt = " Establishment of a new process instance , Can I save a draft ?";
            this.EditState = EditState.Passed;
        }
        /// <summary>
        ///  Explanation  : This test is targeted at the demo environment  001  Process writing unit test code .
        ///  Relates to :  Create , Send , Revocation , Direction of the condition , Return other functions .
        /// </summary>
        public override void Do()
        {
            if (BP.WF.Glo.IsEnableDraft == false)
                throw new Exception("@ This test requires the Web.config IsEnableDraft = 1  Under the state to test it .");

            string fk_flow = "032";
            string userNo = "zhanghaicheng";

            Flow fl = new Flow(fk_flow);

            // zhoutianjiao  Log in .
            BP.WF.Dev2Interface.Port_Login(userNo);

            // Create a blank .
            Int64 workid = BP.WF.Dev2Interface.Node_CreateBlankWork(fl.No, null, null, WebUser.No, null, 0, null, 0, null);

            // The save .
            BP.WF.Dev2Interface.Node_SaveWork(fl.No, 3201,  workid);

            #region  Check the stored data is complete draft .
            GERpt rpt = fl.HisGERpt;
            rpt.OID = workid;
            rpt.RetrieveFromDBSources();
            if (rpt.WFState != WFState.Blank)
                throw new Exception("@ Save error ,此 GERpt  Should be  Blank  Status , It is :" + rpt.WFState);

            bool isHave = false;
            DataTable dt = BP.WF.Dev2Interface.DB_GenerDraftDataTable(fl.No);
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["OID"].ToString() == workid.ToString())
                {
                    isHave = true;
                    break;
                }
            }
            if (isHave == true)
                throw new Exception("@ The draft should not be found ..");
            #endregion

            // Set to Draft .
            BP.WF.Dev2Interface.Node_SetDraft(fl.No, workid);

            #region  Check the stored data is complete draft .
            rpt = fl.HisGERpt;
            rpt.OID = workid;
            rpt.RetrieveFromDBSources();
            if (rpt.WFState != WFState.Draft && BP.WF.Glo.IsEnableDraft)
                throw new Exception("@此 GERpt  Should be  Draft  Status , It is :" + rpt.WFState);

            isHave = false;
            dt = BP.WF.Dev2Interface.DB_GenerDraftDataTable(fl.No);
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["OID"].ToString() == workid.ToString())
                {
                    isHave = true;
                    break;
                }
            }
            if (isHave == false)
                throw new Exception("@ Not found in the draft from the interface .");
            #endregion

        }
    }
}
