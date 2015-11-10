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


namespace BP.CT.AttrFlow
{
    /// <summary>
    ///  Generate WorkID
    /// </summary>
    public  class GeneWorkID : TestBase
    {
        #region  Variable 
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
        ///  Generate WorkID
        /// </summary>
        public GeneWorkID()
        {
            this.Title = " Check the generation WorkID Whether to repeat ";
            this.DescIt = " There are several appearances generation WorkID Repeat phenomenon .";
            this.EditState = EditState.Passed; 
        }
        /// <summary>
        ///  Process Description :
        /// 1, In the process  023 The simplest 3 Node ( Track mode ),  To test .
        /// 2, Only test transmission function , After checking with the data sent is complete .
        /// 3,  This test has three node initiates points , Intermediate point , End point , Corresponding to the three test methods .
        /// </summary>
        public override void Do()
        {
            #region  Definition of variables .
            fk_flow = "023";
            userNo = "zhanghaicheng";
            fl = new Flow(fk_flow);
            #endregion  Definition of variables .

            //让 userNo  Log in .
            BP.WF.Dev2Interface.Port_Login(userNo);

            //  Create a blank ,  In case the title is empty .
            workID = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, WebUser.No, "TitleTest", 0, null, 0, null);

            #region  Check whether the data in line with expectations .
            #endregion  Check whether the data in line with expectations 

            // In the implementation of a creation .
            Int64 workID2 = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, WebUser.No, "TitleTest", 0, null, 0, null);
            if (workID != workID2)
                throw new Exception(" Should generate twice WorkID The same ,  But things are different now .");

             // Performing transmission .
             BP.WF.Dev2Interface.Node_SendWork(fk_flow, workID);

             // In the implementation of a creation .
             workID2 = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, WebUser.No, "TitleTest", 0, null, 0, null);

               if (workID == workID2)
                   throw new Exception(" Should generate twice WorkID Are not the same ,  But now the same .");
             
            // Delete test data .
            BP.WF.Dev2Interface.Flow_DoDeleteFlowByReal(fl.No, workID, false);
        }
    }
}
