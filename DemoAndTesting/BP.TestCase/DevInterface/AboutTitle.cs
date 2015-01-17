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

namespace BP.CT
{
    /// <summary>
    ///  About the title 
    /// </summary>
    public class AboutTitle : TestBase
    {
        /// <summary>
        ///  About the title 
        /// </summary>
        public AboutTitle()
        {
            this.Title = " About the title ";
            this.DescIt = " In the test case -001 Process to make a test case .";
            this.EditState = EditState.Passed;
        }
        /// <summary>
        ///  Carried out 
        /// </summary>
        public override void Do()
        {
            string fk_flow = "001";
            Flow fl = new Flow(fk_flow);

            BP.WF.Dev2Interface.Port_Login("zhoutianjiao");

            // Create a blank ,  Initiating the start node .
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_StartWork(fk_flow, null, null, 0, null);

            Int64 workid = objs.VarWorkID;

            // Log in using the second person .
            BP.WF.Dev2Interface.Port_Login(objs.VarAcceptersID);

            // Set Title 
            BP.WF.Dev2Interface.Flow_SetFlowTitle(fk_flow,workid,"test");

            #region  Check the title is correct .
            string title = DBAccess.RunSQLReturnString("SELECT Title FROM WF_GenerWorkFlow  WHERE WorkID=" + workid);
            if (title != "test")
                throw new Exception("@Flow_SetFlowTitle  Method fails . WF_GenerWorkFlow No change correctly .");
            title = DBAccess.RunSQLReturnString("SELECT Title FROM ND"+int.Parse(fl.No)+"Rpt  WHERE OID=" + workid);
            if (title != "test")
                throw new Exception("@Flow_SetFlowTitle  Method fails .Rpt table  No change correctly .");
            #endregion  Check the title is correct .


            // Re-set title 
            BP.WF.Dev2Interface.Flow_ReSetFlowTitle(fk_flow, objs.VarToNodeID, workid);

            #region  Check the title is correct .
              title = DBAccess.RunSQLReturnString("SELECT Title FROM WF_GenerWorkFlow  WHERE WorkID=" + workid);
            if (title == "test")
                throw new Exception("@Flow_SetFlowTitle  Method fails . WF_GenerWorkFlow No change correctly .");
            title = DBAccess.RunSQLReturnString("SELECT Title FROM ND" + int.Parse(fl.No) + "Rpt  WHERE OID=" + workid);
            if (title == "test")
                throw new Exception("@Flow_SetFlowTitle  Method fails .Rpt table  No change correctly .");
            #endregion  Check the title is correct .

        }
    }
}
