using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using BP.WF;
using BP.En;
using BP.DA;
using BP.Web;
using System.Collections;
using BP.CT;

namespace BP.CT.Model
{
    /// <summary>
    ///  Test looking for leadership 
    /// </summary>
    public class FindLeader : TestBase
    {
        /// <summary>
        ///  Test looking for leadership 
        /// </summary>
        public FindLeader()
        {
            this.Title = " Test looking for leadership ";
            this.DescIt = " Process : 以demo  Process  054: Someone Rules ( Looking for leadership )  A Case Study of Testing .";
            this.EditState = CT.EditState.Passed;
        }

        #region  Global Variables 
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
        ///  Test Case Description :
        /// 1,  Were cited 4种.
        /// 2,  Test to find the leadership of the two modes 
        /// </summary>
        public override void Do()
        {
            this.fk_flow = "054";
            fl = new Flow(this.fk_flow);
            string sUser = "zhoushengyu";
            BP.WF.Dev2Interface.Port_Login(sUser);

            // Create .
            workID = BP.WF.Dev2Interface.Node_CreateBlankWork(fl.No, null, null, WebUser.No, null, 0, null, 0, null);

            // Performing transmission ,  Direct leadership .
            Hashtable ht = new Hashtable();
            ht.Add("FindLeader", 0); //  Find direct leadership .
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(fl.No, workID, ht);

            #region  Analysis of expected 
            if (objs.VarAcceptersID != "zhanghaicheng")
                throw new Exception("@ According to find the direct leadership of the way 0, （ Direct leadership model ） Looking for leadership error , It should be zhanghaicheng It is " + objs.VarAcceptersID);
            #endregion

            // Send perform revocation .  Specified level executives .
            BP.WF.Dev2Interface.Flow_DoUnSend(fl.No, workID);
            ht.Clear(); // Clear parameters .
            ht.Add("FindLeader", 1); //  Find the specified level of leadership positions .
            objs = BP.WF.Dev2Interface.Node_SendWork(fl.No, workID, ht);

            #region  Analysis of expected 
            if (objs.VarAcceptersID != "zhoupeng")
                throw new Exception("@ According to find the direct leadership of the way 1, （ Specify the level of leadership ） Looking for leadership error , It should be zhoupeng It is " + objs.VarAcceptersID);
            #endregion



            // Send perform revocation .  Specific leadership positions .
            BP.WF.Dev2Interface.Flow_DoUnSend(fl.No, workID);
            ht.Clear(); // Clear parameters .
            ht.Add("FindLeader", 2); //  Find the specified level of leadership positions .
            objs = BP.WF.Dev2Interface.Node_SendWork(fl.No, workID, ht);

            #region  Analysis of expected 
            if (objs.VarAcceptersID != "zhoupeng")
                throw new Exception("@ According to find the direct leadership of the way 1, （ Specify the level of leadership ） Looking for leadership error , It should be zhoupeng It is " + objs.VarAcceptersID);
            #endregion


            // Send perform revocation .  Specific leadership positions .
            BP.WF.Dev2Interface.Flow_DoUnSend(fl.No, workID);
            ht.Clear(); // Clear parameters .
            ht.Add("FindLeader", 2); //  Find the specified level of leadership positions .
            objs = BP.WF.Dev2Interface.Node_SendWork(fl.No, workID, ht);

            #region  Analysis of expected 
            if (objs.VarAcceptersID != "zhoupeng")
                throw new Exception("@ According to find the direct leadership of the way 1, （ Specify the level of leadership ） Looking for leadership error , It should be zhoupeng It is " + objs.VarAcceptersID);
            #endregion


            // Send perform revocation .  Specific leadership positions .
            BP.WF.Dev2Interface.Flow_DoUnSend(fl.No, workID);
            ht.Clear(); // Clear parameters .
            ht.Add("FindLeader", 3); //  Find the specified level of leadership positions .
            objs = BP.WF.Dev2Interface.Node_SendWork(fl.No, workID, ht);

            #region  Analysis of expected 
            if (objs.VarAcceptersID != "zhoupeng")
                throw new Exception("@ According to find the direct leadership of the way 1, （ Specify the level of leadership ） Looking for leadership error , It should be zhoupeng It is " + objs.VarAcceptersID);
            #endregion


            //  The node is now the first row , Have been looking for the whole of ,  Two test the left case . Whom ?

            //  Whom test ,  The direct leadership of the specified node staff .
            this.DoFineWho_1();


            //  Whom test ,  The direct leadership of the staff designated form fields .
            this.DoFineWho_2();

        }
        /// <summary>
        ///  Whom test ?  The direct leadership of the specified node staff .
        /// </summary>
        public void DoFineWho_1()
        {
            string sUser = "zhoushengyu";
            BP.WF.Dev2Interface.Port_Login(sUser);

            // Create .
            workID = BP.WF.Dev2Interface.Node_CreateBlankWork(fl.No, null, null, WebUser.No, null, 0, null, 0, null);

            // Performing transmission ,  Direct leadership .
            Hashtable ht = new Hashtable();
            ht.Add("FindLeader", 0); //  Find direct leadership .
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(fl.No, workID, ht);

            // 让zhanghaicheng Log in ,  Send to perform the second step .
            BP.WF.Dev2Interface.Port_Login(objs.VarAcceptersID);

            // 让zhanghaicheng  Performing transmission , Should also be sent to zhanghaicheng.
            objs = BP.WF.Dev2Interface.Node_SendWork(fl.No, workID);

            #region  Analysis of expected 
            if (objs.VarAcceptersID != "zhanghaicheng")
                throw new Exception("@ According to find the direct leadership of the way 1, （ Specify the level of leadership ） Looking for leadership error , It should be zhanghaicheng It is " + objs.VarAcceptersID);
            #endregion

        }
        /// <summary>
        ///  The direct leadership of the specified form field staff .
        /// </summary>
        public void DoFineWho_2()
        {
            string sUser = "zhoushengyu";
            BP.WF.Dev2Interface.Port_Login(sUser);

            // Create .
            workID = BP.WF.Dev2Interface.Node_CreateBlankWork(fl.No, null, null, WebUser.No, null, 0, null, 0, null);

            // Performing transmission ,  Direct leadership .
            Hashtable ht = new Hashtable();
            ht.Add("FindLeader", 1); //  Find direct leadership .
            ht.Add("RenYuanBianHao", "zhoutianjiao");
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(fl.No, workID, ht);

            // 让zhanghaicheng Log in ,  Send to perform the second step .
            BP.WF.Dev2Interface.Port_Login(objs.VarAcceptersID);

            // 让zhanghaicheng  Performing transmission , Should also be sent to zhanghaicheng.
            objs = BP.WF.Dev2Interface.Node_SendWork(fl.No, workID);

            #region  Analysis of expected 
            if (objs.VarAcceptersID != "qifenglin")
                throw new Exception("@ According to find the direct leadership of the way 1, （ Specify the level of leadership ） Looking for leadership error , It should be qifenglin It is " + objs.VarAcceptersID);
            #endregion
        }
    }
}
