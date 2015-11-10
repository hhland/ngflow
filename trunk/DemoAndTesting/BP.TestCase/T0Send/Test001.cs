using System;
using System.Collections.Generic;
using System.Text;
using BP.WF;
using BP.En;
using BP.DA;
using BP.Web;
using System.Data;
using System.Collections;

namespace BP.CT
{
    public  class Test001 : TestBase
    {
        /// <summary>
        ///  Test financial reimbursement process 
        /// </summary>
        public Test001()
        {
            this.Title = " Test financial reimbursement process ";
            this.DescIt = " Direction controlled conditions with the normal operation of the process form fields .";
            this.EditState = CT.EditState.Passed;
        }
        /// <summary>
        ///  Explanation  : This test is targeted at the demo environment  001  Process writing unit test code .
        ///  Relates to :  Create , Send , Revocation , Direction of the condition , Return other functions .
        /// </summary>
        public override void Do()
        {
            string fk_flow = "001";
            string userNo = "zhoutianjiao";

            Flow fl = new Flow(fk_flow);

            // zhoutianjiao  Log in .
            BP.WF.Dev2Interface.Port_Login(userNo);

            // Create a blank ,  Initiating the start node .
            Int64 workid = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, WebUser.No, null, 0,null,0,null);

            #region  After checking the data to initiate the process is complete ?
            // Creating this blank check whether there is data integrity ?
            sql = "SELECT * FROM " + fl.PTable + " WHERE OID=" + workid;
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
                throw new Exception("@ Initiate the process error , Should not report data can not be found .");

            // Check whether there is data node table form ?
            sql = "SELECT * FROM ND101 WHERE OID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
                throw new Exception("@ Should not start node can not be found in the form of data tables ,");
            #endregion  After checking the data to initiate the process is complete ?

            // Performing transmission , Send and retrieve objects ,.
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid);

            #region  Send checks returned object information is complete ?
            // Get in objects obtained from sending to the next worker . 
            string nextUser = objs.VarAcceptersID;
            if (nextUser != "qifenglin")
                throw new Exception(WebUser.No + " Start of financial reimbursement process , Next recipient is incorrect ,  Should be qifenglin, He is the head of department . It is :" + nextUser);

            if (objs.VarToNodeID != 102)
                throw new Exception(WebUser.No + " Start of financial reimbursement process , The next node ,  Should be 102. It is :" + objs.VarToNodeID);

            // Check whether there is data node table form ?
            sql = "SELECT * FROM ND102 WHERE OID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
                throw new Exception("@ Can not find the data should not be in the department manager approval form nodes form table ,");

            #endregion  Send checks returned object information is complete ?

            // Check is performed whether the available start node revocation ?
            string info = BP.WF.Dev2Interface.Flow_DoUnSend(fk_flow, workid);

            #region  Revocation checking whether the transmission line with expectations .
            // Check whether there is data node table form ?
            sql = "SELECT * FROM ND102 WHERE OID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 1)
                throw new Exception("@ Has been withdrawn , The current node should be deleted form data , But ccflow Do not remove it .");

            // Query Process Registry .
            GenerWorkFlow gwf = new GenerWorkFlow();
            gwf.WorkID = workid;
            gwf.RetrieveFromDBSources();
            if (gwf.FK_Node != fl.StartNodeID)
                throw new Exception("@ Data is sent after revocation does not meet expectations :  Did not stay on the start node , But stay in the :" + gwf.FK_Node);

            GenerWorkerList gwl = new GenerWorkerList();
            gwl.FK_Emp = WebUser.No;
            gwl.FK_Node = fl.StartNodeID;
            gwl.WorkID = workid;
            gwl.Retrieve();
            if (gwl.IsPass == true)
                throw new Exception("@ Data is sent after revocation does not meet expectations :  Stay current state of the operator should not be sent , It is has been sent .");

            // Inspection qifenglin Whether there work to be done .
            gwl = new GenerWorkerList();
            gwl.FK_Emp = nextUser;
            gwl.FK_Node = fl.StartNodeID;
            gwl.WorkID = workid;
            if (gwl.RetrieveFromDBSources() != 0)
                throw new Exception("@ Data is sent after revocation does not meet expectations :  Revocation of the recipient work to be done should not exist .");

            // Pass inspection data are consistent with the expected results .
            BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid);
            #endregion  Revocation checking whether the transmission line with expectations 

            // 让qifenglin  Log in .
            BP.WF.Dev2Interface.Port_Login(nextUser);

            // By less than .HeJiFeiYong < 10000  Sent down .
            Hashtable ht = new Hashtable();
            ht.Add("HeJiFeiYong", 900);
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid, ht);

            #region  Inspection  HeJiFeiYong < 1000  Whether to send sent down to go up the financial sector .
            // Check whether there is data node table form ?
            sql = "SELECT HeJiFeiYong FROM ND102 WHERE OID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ Did not find the node 2 Data .");

            int je = int.Parse(dt.Rows[0][0].ToString());
            if (je != 900)
                throw new Exception("@ht  Data is not passed to go inside the node table , Note that if it is not written to automatically populate fields inside .");

            if (objs.VarAcceptersID != "yangyilei")
                throw new Exception("@ When the total cost of less than  10000 时, The results do not meet expectations .  Should be submitted to the Manager of the Finance Department yangyilei Approval . It is :" + objs.VarAcceptersID);

            // Inspection yangyilei  Whether it be to do ?
            sql = "SELECT FK_Emp FROM WF_EmpWorks WHERE WorkID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1
                || dt.Rows[0][0].ToString() != objs.VarAcceptersID)
                throw new Exception("@ The results do not meet expectations :  There should only be a to-do and is : yangyilei.");

            // Send revocation checked execution ,  The purpose is to check if the data   Greater than 1 Wan direction condition .
            BP.WF.Dev2Interface.Flow_DoUnSend(fk_flow, workid);

            // Inspection qifenglin  Whether it be to do ?
            sql = "SELECT FK_Emp FROM WF_EmpWorks WHERE WorkID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1
                || dt.Rows[0][0].ToString() != "qifenglin")
                throw new Exception("@ Node 2 Perform revocation , Data do not meet expectations :  qifenglin.");
            #endregion

            // By less than .HeJiFeiYong > 10000  Sent down .
            ht = new Hashtable();
            ht.Add("HeJiFeiYong", 990999);
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid, ht);

            #region  When the sum is greater than the cost of inspection 10000 时, Whether the data in line with expectations .
            if (objs.VarAcceptersID != "zhoupeng")
                throw new Exception("@ Node 2时, Data is greater than 1万, Direction of the conditions do not meet expectations ,  Should be performed to zhoupeng  Now turn to the :" + objs.VarAcceptersID);

            // Inspection zhoupeng  Whether it be to do ?
            sql = "SELECT FK_Emp FROM WF_EmpWorks WHERE WorkID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ Node 2时, Data is greater than 1万, Direction of the conditions do not meet expectations ,  Should be performed to zhoupeng A person ,  Now do the query results to be wrong ,  Query execution sql:" + sql);

            if (dt.Rows[0][0].ToString() != "zhoupeng")
                throw new Exception("@ Node 2时, Data is greater than 1万, Direction of the conditions do not meet expectations ,  Should be zhoupeng  Query execution sql:" + sql);

            //  Go check whether the  103  Nodes go up ?
            gwf = new GenerWorkFlow();
            gwf.WorkID = workid;
            gwf.RetrieveFromDBSources();
            if (gwf.FK_Node != 103)
                throw new Exception("@ Direction of the conditions are not transferred to  103上, Currently transferred to the :" + gwf.FK_Node);

            sql = "SELECT HeJiFeiYong FROM ND103 WHERE OID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ Did not find the node ( General manager approval ) Data .");
            #endregion  When the sum is greater than the cost of inspection 10000 时, Whether the data in line with expectations .

            // 让zhoupeng  Log in .
            BP.WF.Dev2Interface.Port_Login("zhoupeng");

            // Performing transmission .
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid);

            #region  General manager approval after checking whether the node sends data in line with expectations ?
            if (objs.VarAcceptersID != "yangyilei")
                throw new Exception("@ Should be sent to  yangyilei  But not sent to him .");

            // Inspection qifenglin  Whether it be to do ?
            sql = "SELECT FK_Emp FROM WF_EmpWorks WHERE WorkID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1
                || dt.Rows[0][0].ToString() != "yangyilei")
                throw new Exception("@ Should be sent to  yangyilei  But not sent to him .");

            // Check the report data is complete ?
            sql = "SELECT FlowEnder,FlowEndNode,FlowStarter,WFState,FID,FK_NY FROM  ND1Rpt WHERE OID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows[0]["FlowEnder"].ToString() != "zhoupeng")
                throw new Exception("@ Should be  zhoupeng 是 FlowEnder .");

            if (dt.Rows[0]["FlowStarter"].ToString() != "zhoutianjiao")
                throw new Exception("@ Should be  zhoutianjiao 是 FlowStarter .");

            if (dt.Rows[0]["FlowEndNode"].ToString() != "104")
                throw new Exception("@ Should be  104 是 FlowEndNode .");

            if (int.Parse(dt.Rows[0]["WFState"].ToString()) != (int)WFState.Runing)
                throw new Exception("@ Should be  WFState.Runing 是 WFState .");

            if (int.Parse(dt.Rows[0]["FID"].ToString()) != 0)
                throw new Exception("@ Should be  FID =0");

            if (dt.Rows[0]["FK_NY"].ToString() != DataType.CurrentYearMonth)
                throw new Exception("@ FK_NY  Field filled error . ");
            #endregion  General manager approval after checking whether the node sends data in line with expectations 
            
            // 让yangyilei  Log in .
            BP.WF.Dev2Interface.Port_Login("yangyilei");

            // Performing transmission ,  This is the last node , It should automatically end .
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid);

            #region  Check the end node is in line with expectations ?
            gwf = new GenerWorkFlow();
            gwf.WorkID = workid;

            if (BP.WF.Glo.IsDeleteGenerWorkFlow == true && gwf.RetrieveFromDBSources() != 0)
                throw new Exception("@  After the end of the process  WF_GenerWorkFlow  Not deleted . ");

            sql = "SELECT * FROM wf_GenerWorkerList WHERE WORKID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 0)
                throw new Exception("@  After the end of the process  wf_GenerWorkerList  Not deleted . ");

            // Check the report data is complete ?
            sql = "SELECT FlowEnder,FlowEndNode,FlowStarter,WFState,FID,FK_NY FROM  ND1Rpt WHERE OID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows[0]["FlowEnder"].ToString() != "yangyilei")
                throw new Exception("@ Should be  yangyilei 是 FlowEnder .");

            if (dt.Rows[0]["FlowStarter"].ToString() != "zhoutianjiao")
                throw new Exception("@ Should be  zhoutianjiao 是 FlowStarter .");

            if (dt.Rows[0]["FlowEndNode"].ToString() != "104")
                throw new Exception("@ Should be  104 是 FlowEndNode .");

            if (int.Parse(dt.Rows[0]["WFState"].ToString()) != (int)WFState.Complete)
                throw new Exception("@ Should be  WFState.Complete 是 WFState .");

            if (int.Parse(dt.Rows[0]["FID"].ToString()) != 0)
                throw new Exception("@ Should be  FID =0");

            if (dt.Rows[0]["FK_NY"].ToString() != DataType.CurrentYearMonth)
                throw new Exception("@ FK_NY  Field filled error . ");
            #endregion  Check the end node is in line with expectations 

        }
    }
}
