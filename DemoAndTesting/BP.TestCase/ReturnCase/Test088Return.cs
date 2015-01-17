using System;
using System.Collections.Generic;
using System.Text;
using BP.WF;
using BP.En;
using BP.DA;
using BP.Web;
using System.Data;
using System.Collections;
using BP.CT;

namespace BP.CT.ReturnCase
{
    public class Test088Return : TestBase
    {
        /// <summary>
        ///  Test return 
        /// </summary>
        public Test088Return()
        {
            this.Title = " Intermediate step process with a sub-confluent return ";
            this.DescIt = "1, Return to starting point , Send down step by step .2, Back to the diversion point , Send down step by step .";
            this.DescIt += " Repeat 1,2, Test return and backtrack .";
            this.EditState = EditState.Passed;
        }
        public override void Do()
        {
          
            #region  Back to the start node test three models .
            // Test return from the last node to the start node , Then allowed a step by step to send .
            this.TestReturnToStartNode();

            // Test return from the last node to the start node , Then allowed to backtrack .
            this.TestReturnToStartNodeWithTrackback();

            // Test return from the last node to the start node , Then allowed to backtrack ,  Then do not backtrack in return allowed .
            this.TestReturnToStartNodeWithTrackback_1();
            #endregion  Back to the start node test three models .


            #region  Back to the three models tested shunt node .
            this.ReturnToFenLiuNode();
            this.ReturnToFenLiuNodeWithTrackback();
            this.ReturnToFenLiuNodeWithTrackback_1();
            #endregion  Back to the three models tested shunt node .

            return;
        }

        #region  Back to triage test point .
        /// <summary>
        ///  Back to triage test point , Then allowed a step by step to send .
        /// </summary>
        public void ReturnToFenLiuNode()
        {
            // Create test cases .
            Int64 workid = this.CreateTastCase();

            // Let him log in .
            BP.WF.Dev2Interface.Port_Login("zhoupeng");

            // Return to the starting node , Not backtrack .
            BP.WF.Dev2Interface.Node_ReturnWork("088", workid, 0, 8899, 8802, "test", false);

            //让 qifenglin ,  Login allowed to send , This is the confluence sent .
            BP.WF.Dev2Interface.Port_Login("qifenglin");
            BP.WF.Dev2Interface.Node_SendWork("088", workid);

            //  Let diversion point sends .
            string sql = "SELECT WorkID,FK_Emp FROM WF_GenerWorkerlist WHERE FID=" + workid;
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                Int64 id = Int64.Parse(dr["WorkID"].ToString());
                string emp = dr[1].ToString();
                BP.WF.Dev2Interface.Port_Login(emp);
                BP.WF.Dev2Interface.Node_SendWork("088", id);
            }

            //让 zhanghaicheng ,  Login allowed to send .
            BP.WF.Dev2Interface.Port_Login("zhanghaicheng");
            BP.WF.Dev2Interface.Node_SendWork("088", workid);

            //让 zhoupeng ,  Login allowed to send .
            BP.WF.Dev2Interface.Port_Login("zhoupeng");
            BP.WF.Dev2Interface.Node_SendWork("088", workid);

            /*
             *  Run to the successful implementation here .
             */
        }

        /// <summary>
        ///  Test return from the last node to split point , Then allowed to backtrack ..
        /// </summary>
        public void ReturnToFenLiuNodeWithTrackback()
        {
            // Create test cases .
            Int64 workid = this.CreateTastCase();

            // Let him log in .
            BP.WF.Dev2Interface.Port_Login("zhoupeng");

            // Return to the starting node , Not backtrack .
            BP.WF.Dev2Interface.Node_ReturnWork("088", workid, 0, 8899, 8802, "test", true);

            //让 zhoutianjiao ,  Login allowed to send .
            BP.WF.Dev2Interface.Port_Login("qifenglin");
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork("088", workid);
            if (objs.VarAcceptersID != "zhoupeng")
                throw new Exception("@ Back to the start node and backtrack error , Should be returned to the zhoupeng, It is :" + objs.VarAcceptersID);
            
            //让zhoupeng  Log in , Send down .
            BP.WF.Dev2Interface.Port_Login(objs.VarAcceptersID);
            objs = BP.WF.Dev2Interface.Node_SendWork("088", workid);
        }

        /// <summary>
        ///  Test return from the last node to split point , Then allowed to backtrack .
        ///  After return to the last node , But let it not backtrack on the implementation of a return , Let sponsor performing transmission 
        ///  To see if it is in accordance with the downward movement of the steps of a step by step .
        /// </summary>
        public void ReturnToFenLiuNodeWithTrackback_1()
        {
            // Create test cases .
            Int64 workid = this.CreateTastCase();

            // Let him log in .
            BP.WF.Dev2Interface.Port_Login("zhoupeng");

            // Return to the starting node , Not backtrack .
            BP.WF.Dev2Interface.Node_ReturnWork("088", workid, 0, 8899, 8802, "test", true);

            //让 zhoutianjiao ,  Login allowed to send .
            BP.WF.Dev2Interface.Port_Login("qifenglin");
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork("088", workid);
            if (objs.VarAcceptersID != "zhoupeng")
                throw new Exception("@ Back to the start node and backtrack error , Should be returned to the zhoupeng, It is :" + objs.VarAcceptersID);

            // In executing a return , But not backtrack .
            BP.WF.Dev2Interface.Node_ReturnWork("088", workid, 0, 8899, 8802, "test", false);


            //让qifenglin Log in ,  Test the next step , Is in accordance with a step by step toward ending node .
            BP.WF.Dev2Interface.Port_Login("qifenglin");
            objs = BP.WF.Dev2Interface.Node_SendWork("088", workid);

            //  Let diversion point sends .
            string sql = "SELECT WorkID,FK_Emp FROM WF_GenerWorkerlist WHERE FID=" + workid;
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
                throw new Exception(" No inquiry into the child thread .");

            foreach (DataRow dr in dt.Rows)
            {
                Int64 id = Int64.Parse(dr["WorkID"].ToString());
                string emp = dr[1].ToString();
                BP.WF.Dev2Interface.Port_Login(emp);
                BP.WF.Dev2Interface.Node_SendWork("088", id);
            }

            //让 zhanghaicheng ,  Login allowed to send .
            BP.WF.Dev2Interface.Port_Login("zhanghaicheng");
            BP.WF.Dev2Interface.Node_SendWork("088", workid);

            //让 zhoupeng ,  Login allowed to send .
            BP.WF.Dev2Interface.Port_Login("zhoupeng");
            BP.WF.Dev2Interface.Node_SendWork("088", workid);

            /*
             *  Run to the successful implementation here .
             */
        }

        #endregion  Back to triage test point .


        #region  Back to the start node test three models 
        /// <summary>
        ///  Test return from the last node to the start node , Then allowed to backtrack .
        ///  After return to the last node , But let it not backtrack on the implementation of a return , Let sponsor performing transmission 
        ///  To see if it is in accordance with the downward movement of the steps of a step by step .
        /// </summary>
        public void TestReturnToStartNodeWithTrackback_1()
        {
            // Create test cases .
            Int64 workid = this.CreateTastCase();

            // Let him log in .
            BP.WF.Dev2Interface.Port_Login("zhoupeng");

            // Return to the starting node , Not backtrack .
            BP.WF.Dev2Interface.Node_ReturnWork("088", workid, 0, 8899, 8801, "test", true);

            //让 zhoutianjiao ,  Login allowed to send .
            BP.WF.Dev2Interface.Port_Login("zhoutianjiao");
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork("088", workid);
            if (objs.VarAcceptersID != "zhoupeng")
                throw new Exception("@ Back to the start node and backtrack error , Should be returned to the zhoupeng, It is :" + objs.VarAcceptersID);

            // In executing a return , But not backtrack .
            BP.WF.Dev2Interface.Node_ReturnWork("088", workid, 0, 8899, 8801, "test", false);


            //让zhoutianjiao Log in ,  Test the next step , Is in accordance with a step by step toward ending node .
            BP.WF.Dev2Interface.Port_Login("zhoutianjiao");
            objs = BP.WF.Dev2Interface.Node_SendWork("088", workid);
         
            //让 qifenglin ,  Login allowed to send , This is the confluence sent .
            BP.WF.Dev2Interface.Port_Login("qifenglin");
            BP.WF.Dev2Interface.Node_SendWork("088", workid);

            //  Let diversion point sends .
            string sql = "SELECT WorkID,FK_Emp FROM WF_GenerWorkerlist WHERE FID=" + workid;
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                Int64 id = Int64.Parse(dr["WorkID"].ToString());
                string emp = dr[1].ToString();
                BP.WF.Dev2Interface.Port_Login(emp);
                BP.WF.Dev2Interface.Node_SendWork("088", id);
            }

            //让 zhanghaicheng ,  Login allowed to send .
            BP.WF.Dev2Interface.Port_Login("zhanghaicheng");
            BP.WF.Dev2Interface.Node_SendWork("088", workid);

            //让 zhoupeng ,  Login allowed to send .
            BP.WF.Dev2Interface.Port_Login("zhoupeng");
            BP.WF.Dev2Interface.Node_SendWork("088", workid);

            /*
             *  Run to the successful implementation here .
             */
        }

        /// <summary>
        ///  Test return from the last node to the start node , Then allowed to backtrack ..
        /// </summary>
        public void TestReturnToStartNodeWithTrackback()
        {
            // Create test cases .
            Int64 workid = this.CreateTastCase();

            // Let him log in .
            BP.WF.Dev2Interface.Port_Login("zhoupeng");

            // Return to the starting node , Not backtrack .
            BP.WF.Dev2Interface.Node_ReturnWork("088", workid, 0, 8899, 8801, "test", true);

            //让 zhoutianjiao ,  Login allowed to send .
            BP.WF.Dev2Interface.Port_Login("zhoutianjiao");
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork("088", workid);
            if (objs.VarAcceptersID != "zhoupeng")
                throw new Exception("@ Back to the start node and backtrack error , Should be returned to the zhoupeng, It is :" + objs.VarAcceptersID);

            //让zhoupeng  Log in , Send down .
            BP.WF.Dev2Interface.Port_Login(objs.VarAcceptersID);
            objs = BP.WF.Dev2Interface.Node_SendWork("088", workid);

        }
        /// <summary>
        ///  Test return from the last node to the start node , Then allowed a step by step to send .
        /// </summary>
        public void TestReturnToStartNode()
        {
            // Create test cases .
            Int64 workid = this.CreateTastCase();

            // Let him log in .
            BP.WF.Dev2Interface.Port_Login("zhoupeng");

            // Return to the starting node , Not backtrack .
            BP.WF.Dev2Interface.Node_ReturnWork("088", workid, 0, 8899, 8801, "test", false);

            //让 zhoutianjiao ,  Login allowed to send .
            BP.WF.Dev2Interface.Port_Login("zhoutianjiao");
            BP.WF.Dev2Interface.Node_SendWork("088", workid);

            //让 qifenglin ,  Login allowed to send , This is the confluence sent .
            BP.WF.Dev2Interface.Port_Login("qifenglin");
            BP.WF.Dev2Interface.Node_SendWork("088", workid);

            //  Let diversion point sends .
            string sql = "SELECT WorkID,FK_Emp FROM WF_GenerWorkerlist WHERE FID=" + workid;
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                Int64 id = Int64.Parse(dr["WorkID"].ToString());
                string emp = dr[1].ToString();
                BP.WF.Dev2Interface.Port_Login(emp);
                BP.WF.Dev2Interface.Node_SendWork("088", id);
            }

            //让 zhanghaicheng ,  Login allowed to send .
            BP.WF.Dev2Interface.Port_Login("zhanghaicheng");
            BP.WF.Dev2Interface.Node_SendWork("088", workid);

            //让 zhoupeng ,  Login allowed to send .
            BP.WF.Dev2Interface.Port_Login("zhoupeng");
            BP.WF.Dev2Interface.Node_SendWork("088", workid);

            /*
             *  Run to the successful implementation here .
             */
        }
        #endregion  Back to the start node test three models 

        /// <summary>
        ///  Create a test scenario , So he went to the end node .
        /// </summary>
        public Int64 CreateTastCase()
        {
            string fk_flow = "088";
            string startUser = "zhoutianjiao";
            BP.Port.Emp starterEmp = new Port.Emp(startUser);

            Flow fl = new Flow(fk_flow);

            // Let Zhou Jiao Login ,  After , You can access WebUser.No, WebUser.Name .
            BP.WF.Dev2Interface.Port_Login(startUser);

            // Create a blank ,  Initiating the start node .
            Int64 workid = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, WebUser.No, null, 0, null, 0, null);

            // Implementation of the   Split point   Send , qifenglin Accept .
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid);
            if (objs.VarAcceptersID != "qifenglin")
                throw new Exception("@ Recipient error , It should be qifenglin, It is :" + objs.VarAcceptersID);

            // Let diversion point sponsor Login .
            BP.WF.Dev2Interface.Port_Login(objs.VarAcceptersID);
            // Execution is sent down .
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid);

            if (objs.VarAcceptersID != "zhangyifan,zhoushengyu,")
                throw new Exception("@ Recipient error , It should be zhangyifan,zhoushengyu, It is :" + objs.VarAcceptersID);

            string sql = "SELECT WorkID,FK_Emp FROM WF_GenerWorkerlist WHERE FID=" + workid;
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                Int64 id = Int64.Parse(dr["WorkID"].ToString());
                string emp = dr[1].ToString();
                BP.WF.Dev2Interface.Port_Login(emp);
                objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, id);
            }

           
            //让qifenglin Log in , Sent to the last node , Carry out tastcase.
            BP.WF.Dev2Interface.Port_Login("zhanghaicheng");
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid);
            if (objs.VarAcceptersID!="zhoupeng")
                throw new Exception("@ Recipient error , It should be zhoupeng, It is :" + objs.VarAcceptersID);

            return workid;
        }
       
    }
}
