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

namespace BP.CT.AttrFlow
{
    /// <summary>
    ///  Flow basis function 
    /// </summary>
    public  class FlowBaseFunc : TestBase
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
        ///  Flow basis function 
        /// </summary>
        public FlowBaseFunc()
        {
            this.Title = " Flow basis  API  Functional Testing ";
            this.DescIt = " Create , Delete , Transfer , Forwarding , Cc .";
            this.EditState = EditState.Passed; 
        }
        /// <summary>
        ///  Carried out 
        /// </summary>
        public override void Do()
        {
            #region  Define global variables .
            fk_flow = "023";
            userNo = "zhanghaicheng";
            fl = new Flow(fk_flow);
            #endregion  Define global variables .

            //  Testing Delete .
            this.TestDelete();

            //  Testing handover .
            this.TestShift();

            //  Test Cc .
            this.TestCC();
        }
       
        /// <summary>
        ///  Test Cc 
        /// </summary>
        public void TestCC()
        {
            string sUser = "zhoupeng";
            BP.WF.Dev2Interface.Port_Login(sUser);

            // Create .
            workID = BP.WF.Dev2Interface.Node_CreateBlankWork(fl.No, null, null, WebUser.No, null, 0, null, 0, null);

            // Performing transmission .
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(fl.No, workID);

            // Execution Cc .
            BP.WF.Dev2Interface.Node_CC(fl.No, objs.VarToNodeID, workID, "zhoushengyu", "zhoupeng", " Transfer test ", "", null, 0);

            //让 zhoushengyu  Landed .
            BP.WF.Dev2Interface.Port_Login("zhoushengyu");

            #region  Check the expected results .
            sql = "SELECT FK_Emp FROM WF_EmpWorks WHERE WorkID=" + workID;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
                throw new Exception("@ Upcoming lost after transfer .");

            if (dt.Rows.Count != 1)
                throw new Exception("@ There should be only one person in the to-do state .");

            if (dt.Rows[0][0].ToString() != "zhoupeng")
                throw new Exception("@ Should be :zhoupeng  It is :" + dt.Rows[0][0].ToString());

            CCList list = new CCList();
            int num = list.Retrieve(CCListAttr.Rec, sUser, CCListAttr.WorkID, workID);
            if (num <= 0)
                throw new Exception("@ CC data is not written in  WF_CCList  Table , Inquiry number is :" + num);
            #endregion  Check the expected results 
        }
        /// <summary>
        ///  Testing handover 
        /// </summary>
        public void TestShift()
        {
            // Create .
            workID = BP.WF.Dev2Interface.Node_CreateBlankWork(fl.No, null, null, WebUser.No, null, 0, null, 0, null);

            // Performing transmission .
            BP.WF.Dev2Interface.Node_SendWork(fl.No, workID);

            // Performing handover .
            //BP.WF.Dev2Interface.Node_Shift(fl.No, 0, workID, "zhoushengyu", " Transfer test ");

            #region  Check the expected results 
            sql = "SELECT * FROM WF_EmpWorks WHERE WorkID="+workID;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
                throw new Exception("@ Upcoming lost after transfer .");

            if (dt.Rows.Count!=1)
                throw new Exception("@ There should be only one person in the to-do state .");

            if (dt.Rows[0]["FK_Emp"].ToString() !="zhoushengyu" )
                throw new Exception("@ No transfer to  zhoushengyu");

            ShiftWork sw = new ShiftWork();
            int i = sw.Retrieve(ShiftWorkAttr.WorkID,workID, ShiftWorkAttr.FK_Node, 2302, ShiftWorkAttr.ToEmp, "zhoushengyu");
            if (i==0)
                throw new Exception("@ The transfer of data is not written in WF_ShiftWork Table  zhoushengyu");
            if (i != 1)
                throw new Exception("@ Transfer data is written to a number of ,在WF_ShiftWork Table  zhoushengyu");
            #endregion  Check the expected results 
        }
        /// <summary>
        ///  Testing Delete 
        /// </summary>
        public void TestDelete()
        {
            // Create .
            workID = BP.WF.Dev2Interface.Node_CreateBlankWork(fl.No, null, null, WebUser.No, null, 0, null, 0, null);

            // Performing transmission .
            BP.WF.Dev2Interface.Node_SendWork(fl.No, workID);

            // Delete .
            BP.WF.Dev2Interface.Flow_DoDeleteFlowByReal(fl.No, workID,false);

            #region  Check the delete function is in line with expectations .
            gwf = new GenerWorkFlow();
            gwf.WorkID = workID;
            if (gwf.RetrieveFromDBSources() != 0)
                throw new Exception("@GenerWorkFlow Undeleted data .");

            gwl = new GenerWorkerList();
            gwl.WorkID = workID;
            if (gwl.RetrieveFromDBSources() != 0)
                throw new Exception("@GenerWorkerList Undeleted data .");

            sql = "SELECT * FROM ND2301 WHERE OID="+workID;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 0)
                throw new Exception("@ ND2301  Node data is not deleted . ");

            sql = "SELECT * FROM ND2302 WHERE OID=" + workID;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 0)
                throw new Exception("@ ND2302  Node data is not deleted . ");

            sql = "SELECT * FROM ND23Rpt WHERE OID=" + workID;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 0)
                throw new Exception("@ ND23Rpt  Data is not deleted . ");
            #endregion  Check the delete function is in line with expectations .
        }
    }
}
