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

namespace BP.CT.T0Send
{
    /// <summary>
    ///  Send multiplayer test 
    /// </summary>
    public class SendCaseToEms : TestBase
    {
        /// <summary>
        ///  Send multiplayer test 
        /// </summary>
        public SendCaseToEms()
        {
            this.Title = " Send multiplayer test ";
            this.DescIt = " Process : 023  Testing is sent to more than one person , Whether the data is sent to perform in line with expectations after .";
            this.EditState = CT.EditState.UnOK;
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
        public Int64 workid = 0;
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
        ///  Send Test 
        /// </summary>
        public override void Do()
        {
            // Initialize variables .
            fk_flow = "023";
            userNo = "zhanghaicheng";
            fl = new Flow(fk_flow);

            // Perform the login .
            BP.WF.Dev2Interface.Port_Login(userNo);

            // Implementation of 1 Step inspection , Create work and send .
            this.workid = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, userNo, null);

            // Accept the next step to define multiple .
            string toEmps = "zhoushengyu,zhangyifan,";

            // To send more than one person .
            objs = BP.WF.Dev2Interface.Node_SendWork(this.fk_flow, this.workid, null, null, 0, toEmps);


            #region  Check whether the correct sending variables ?
            if (objs.VarAcceptersID != toEmps)
                throw new Exception("@ Should be the recipient ID Multiplayer , It is :" + objs.VarAcceptersID);

            if (objs.VarAcceptersName != " Week liter rain , Yifan Zhang ,")
                throw new Exception("@ Should be the recipient Name Multiplayer , It is :" + objs.VarAcceptersID);

            if (objs.VarCurrNodeID != 2301)
                throw new Exception("@ The current node should be  2301 , It is :" + objs.VarCurrNodeID);

            if (objs.VarToNodeID != 2302)
                throw new Exception("@ Reach the node should be  2302 , It is :" + objs.VarToNodeID);
            #endregion  Check whether the correct sending variables ?

            #region  Check the Process Engine table is correct ?
            GenerWorkFlow gwf = new GenerWorkFlow();
            gwf.WorkID = this.workid;
            if (gwf.RetrieveFromDBSources() != 1)
                throw new Exception("@ Lost registry data flow engine ");
            if (gwf.FK_Dept != WebUser.FK_Dept)
                throw new Exception("@ Error should be attached to the department :" + WebUser.FK_Dept + ", It is :" + gwf.FK_Dept);

            if (gwf.FK_Flow != fk_flow)
                throw new Exception("@ Process number error should be :" + fk_flow + ", It is :" + gwf.FK_Flow);

            if (gwf.FK_Node != 2302)
                throw new Exception("@ Current node error should be :" + 2302 + ", It is :" + gwf.FK_Node);

            if (gwf.Starter != userNo)
                throw new Exception("@ Current  Starter  Error should be :" + userNo + ", It is :" + gwf.Starter);

            if (gwf.StarterName != WebUser.Name)
                throw new Exception("@ Current  StarterName  Error should be :" + WebUser.Name + ", It is :" + gwf.StarterName);

            if (string.IsNullOrEmpty(gwf.Title))
                throw new Exception("@ Title  Error ,  Can not be empty . ");

            GenerWorkerLists wls = new GenerWorkerLists();
            wls.Retrieve(GenerWorkerListAttr.WorkID, this.workid);
            if (wls.Count != 3)
                throw new Exception("@ There should be three data , It is :" + wls.Count);
            foreach (GenerWorkerList wl in wls)
            {
                if (wl.FID != 0)
                    throw new Exception("@ FID  Error , Should be " + 0 + ", It is :" + wl.FID);

                if (wl.FK_Emp == "zhanghaicheng")
                {
                    if (wl.FK_Dept != WebUser.FK_Dept)
                        throw new Exception("@ Departments error , Should be " + WebUser.FK_Dept + ", It is :" + wl.FK_Dept);

                    if (wl.FK_Flow != fk_flow)
                        throw new Exception("@ FK_Flow  Error , Should be " + fk_flow + ", It is :" + wl.FK_Flow);

                    if (wl.FK_Node != 2301)
                        throw new Exception("@ FK_Node  Error , Should be " + 2301 + ", It is :" + wl.FK_Node);

                    if (wl.IsEnable == false)
                        throw new Exception("@ IsEnable  Error , Should be true, It is :" + wl.IsEnable);

                    if (wl.IsRead == true)
                        throw new Exception("@ IsRead  Error , Should be false, It is :" + wl.IsEnable);

                    if (wl.IsPass == false)
                        throw new Exception("@ IsPass  Error , Should be true, It is :" + wl.IsEnable);

                    if (wl.Sender != WebUser.No + ", Zhang Haicheng ")
                        throw new Exception("@ Sender  Error , Should be " + WebUser.No + ", Zhang Haicheng  , It is :" + wl.Sender);
                }

                if (wl.FK_Emp == "zhoushengyu" || wl.FK_Emp == "zhangyifan")
                {
                    BP.Port.Emp emp = new Port.Emp(wl.FK_Emp);

                    if (wl.FK_Dept != emp.FK_Dept)
                        throw new Exception("@ Departments error , Should be " + emp.FK_Dept + ", It is :" + wl.FK_Dept);

                    if (wl.FK_Flow != fk_flow)
                        throw new Exception("@ FK_Flow  Error , Should be " + fk_flow + ", It is :" + wl.FK_Flow);

                    if (wl.FK_Node != 2302)
                        throw new Exception("@ FK_Node  Error , Should be " + 2302 + ", It is :" + wl.FK_Node);

                    if (wl.IsEnable == false)
                        throw new Exception("@ IsEnable  Error , Should be true, It is :" + wl.IsEnable);

                    if (wl.IsRead == true)
                        throw new Exception("@ IsRead  Error , Should be false, It is :" + wl.IsEnable);

                    if (wl.IsPass == true)
                        throw new Exception("@ IsPass  Error , Should be false, It is :" + wl.IsEnable);

                    if (wl.Sender != "zhanghaicheng")
                        throw new Exception("@ Sender  Error , Should be " + WebUser.No + ", It is :" + wl.Sender);
                }
            }

            string sql = "SELECT COUNT(*) FROM WF_EmpWorks WHERE WorkID=" + this.workid + " AND FK_Emp='zhoushengyu'";
            if (DBAccess.RunSQLReturnValInt(sql) != 1)
                throw new Exception("@ Should not be less than the query  zhoushengyu  To-do .");

            sql = "SELECT COUNT(*) FROM WF_EmpWorks WHERE WorkID=" + this.workid + " AND FK_Emp='zhangyifan'";
            if (DBAccess.RunSQLReturnValInt(sql) != 1)
                throw new Exception("@ Should not be less than the query  zhangyifan  To-do .");
            #endregion  Check the Process Engine table is correct ?

            //  Let one person login .
            BP.WF.Dev2Interface.Port_Login("zhoushengyu");

            // Let him send .
            BP.WF.Dev2Interface.Node_SendWork(this.fk_flow, this.workid);

            #region  Check the Process Engine table is correct ?
            sql = "SELECT COUNT(*) FROM WF_EmpWorks WHERE WorkID=" + this.workid + " AND FK_Emp='zhoushengyu'";
            if (DBAccess.RunSQLReturnValInt(sql) != 0)
                throw new Exception("@ Should not be in the query to  zhoushengyu  To-do .");

            sql = "SELECT COUNT(*) FROM WF_EmpWorks WHERE WorkID=" + this.workid + " AND FK_Emp='zhangyifan'";
            if (DBAccess.RunSQLReturnValInt(sql) != 0)
                throw new Exception("@ Should not be in the query to  zhangyifan  To-do .");
            #endregion  Check the Process Engine table is correct ?

            // Delete the test data .
            BP.WF.Dev2Interface.Flow_DoDeleteFlowByReal(this.fk_flow, workid, true);
        }
    }
}
