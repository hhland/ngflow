using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BP.WF;
using BP.WF.Data;
using BP.WF.Template;
using BP.En;
using BP.DA;
using BP.Web;
using System.Data;
using System.Collections;
using BP.CT;

namespace BP.CT.NodeAttr
{
    public  class DoOutTimeCond : TestBase
    {
        /// <summary>
        ///  Test financial reimbursement process - Timeout approval function 
        /// </summary>
        public DoOutTimeCond()
        {
            this.Title = " Node property assessment - Timeout processing nodes ";
            this.DescIt = " In the process :032( Node Properties - Timeout Handling ) Test , Timeout processing logic testing under a variety of types is correct .";
            this.EditState = EditState.UnOK;
        }
        /// <summary>
        ///  Explanation  : This test is targeted at the demo environment  001  Process writing unit test code .
        ///  Relates to :  Create , Send , Revocation , Direction of the condition , Return other functions .
        /// </summary>
        public override void Do()
        {
            throw new Exception("@ This feature is not yet complete ");

            string fk_flow = "032";
            string userNo = "zhanghaicheng";
            
            Flow fl = new Flow(fk_flow);
            // zhoutianjiao  Log in .
            BP.WF.Dev2Interface.Port_Login(userNo);
            // Create a blank .
            Int64 workid = BP.WF.Dev2Interface.Node_CreateBlankWork(fl.No, null, null, WebUser.No, null, 0, null, 0, null);

            // Performing transmission , And field parameters are not processed .
            Hashtable ht = new Hashtable();
            ht.Add("OutTimeDealDir", (int)OutTimeDeal.None);
            BP.WF.Dev2Interface.Node_SendWork(fl.No, workid, ht);


        }
    }
}
