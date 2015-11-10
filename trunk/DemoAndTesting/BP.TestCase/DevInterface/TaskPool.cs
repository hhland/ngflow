using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using BP.WF;
using BP.En;
using BP.DA;
using BP.Web;
using System.Data;
using System.Collections;
using BP.CT;

namespace BP.CT.Model
{
    /// <summary>
    ///  Shared task pool 
    /// </summary>
    public class TaskPool : TestBase
    {
        /// <summary>
        ///  Shared task pool 
        /// </summary>
        public TaskPool()
        {
            this.Title = " Shared task pool ";
            this.DescIt = " Process : 以demo  Process 068  A Case Study of Testing .";
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
        /// 1,  Remove the test shared tasks , Add .
        /// 2,  Both models share the task pool 
        /// </summary>
        public override void Do()
        {
            if (BP.WF.Glo.IsEnableTaskPool == false)
                throw new Exception("@ This unit tests need to open web.config The IsEnableTaskPool Configuration .");

            this.fk_flow = "068";
            fl = new Flow("068");

            Node nd = new Node(6899);
        //    nd.IsEnableTaskPool = true;
            nd.Update();

            string sUser = "zhoupeng";
            BP.WF.Dev2Interface.Port_Login(sUser);

            // Create .
            workID = BP.WF.Dev2Interface.Node_CreateBlankWork(fl.No, null, null, WebUser.No, null, 0, null, 0, null);

            // Performing transmission , Send the two people .
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(fl.No, workID, null, null, 0, "zhangyifan,zhanghaicheng");

            #region  Check the data results 
            GenerWorkFlow gwf = new GenerWorkFlow(this.workID);
            if (gwf.TodoEmpsNum != 2)
                throw new Exception("@ There should be two staff , It is :" + gwf.TodoEmpsNum);

            if (gwf.TodoEmps != "zhangyifan, Yifan Zhang ;zhanghaicheng, Zhang Haicheng ;")
                throw new Exception("@ There should be two staff , It is :" + gwf.TodoEmps);

            if (gwf.TaskSta != TaskSta.Sharing)
                throw new Exception("@ The state should be shared , It is :" + gwf.TaskSta.ToString());
            #endregion  Check the data results .

            //让zhanghaicheng Landed .
            BP.WF.Dev2Interface.Port_Login("zhangyifan");

            #region  Check that you have her to-do .
            //  Inspection  zhangyifan
            DataTable dt = BP.WF.Dev2Interface.DB_GenerEmpWorksOfDataTable();
            bool isHave = false;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["WorkID"].ToString() == this.workID.ToString())
                {
                    isHave = true;
                    break;
                }
            }
            if (isHave == true)
                throw new Exception("@ Should not find her to-do , Because it is a shared task .");

            //  Inspection  zhanghaicheng,  He should be to do the task .
            int v = BP.DA.DBAccess.RunSQLReturnValInt("SELECT COUNT(*) from wf_empWorks where WorkID=" + this.workID + " AND FK_Emp='zhanghaicheng' ", 100);
            if (v != 1)
                throw new Exception("@ Should not be do not find his .");


            //  Looking from the task pool .
            dt = BP.WF.Dev2Interface.DB_TaskPool();
            isHave = false;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["WorkID"].ToString() == this.workID.ToString())
                {
                    isHave = true;
                    break;
                }
            }
            if (isHave == false)
                throw new Exception("@ Do not find her to be in the task pool .");
            #endregion  Check that you have her to-do .

            //  Test 1
            CheckData();

            //  Test 2
            CheckData();

            //  Test 3
            CheckData();
        }
        /// <summary>
        ///  Check the data 
        /// </summary>
        private void CheckData()
        {
            //  Get task execution .
            BP.WF.Dev2Interface.Node_TaskPoolTakebackOne(workID);

            #region  Check the task 
            gwf = new GenerWorkFlow(this.workID);
            if (gwf.TaskSta != TaskSta.Takeback)
                throw new Exception("@ Should be taken away from the state , But now is :" + gwf.TaskSta.ToString());

            //  Inspection  zhanghaicheng,  He should not be to do the task .
            int v = BP.DA.DBAccess.RunSQLReturnValInt("SELECT COUNT(*) from wf_empWorks where WorkID=" + this.workID + " AND FK_Emp='zhanghaicheng' ", 100);
            if (v != 0)
                throw new Exception("@ He should not be found to do .");

            //  From the inside looking Upcoming , To check zhangyifan  Task .
            dt = BP.WF.Dev2Interface.DB_GenerEmpWorksOfDataTable();
            bool isHave = false;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["WorkID"].ToString() == this.workID.ToString())
                {
                    isHave = true;
                    break;
                }
            }
            if (isHave == false)
                throw new Exception("@ Should not not be found [" + WebUser.No + "] Upcoming , Shared mission , After the application to be run down and should be placed in .");

            //  Looking from the task pool .
            dt = BP.WF.Dev2Interface.DB_TaskPool();
            isHave = false;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["WorkID"].ToString() == this.workID.ToString())
                {
                    isHave = true;
                    break;
                }
            }
            if (isHave == true)
                throw new Exception("@ Remove this task after execution , Should no longer find her task .");

            //  I applied to get down to the task 
            dt = BP.WF.Dev2Interface.DB_TaskPoolOfMyApply();
            isHave = false;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["WorkID"].ToString() == this.workID.ToString())
                {
                    isHave = true;
                    break;
                }
            }
            if (isHave == false)
                throw new Exception("@ Not found " + WebUser.No + " Tasks applications ");
            #endregion  Check the task 

            //  Into the task pool 
            BP.WF.Dev2Interface.Node_TaskPoolPutOne(workID);

            #region  Data Check 
            gwf = new GenerWorkFlow(this.workID);
            if (gwf.TaskSta != TaskSta.Sharing)
                throw new Exception("@ It should be sharing  It is :" + gwf.TaskSta.ToString());

            //  Inspection  zhangyifan
            dt = BP.WF.Dev2Interface.DB_GenerEmpWorksOfDataTable();
            isHave = false;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["WorkID"].ToString() == this.workID.ToString())
                {
                    isHave = true;
                    break;
                }
            }
            if (isHave == true)
                throw new Exception("@ Should not find her to-do , Because it is a shared task .");

            //  Inspection  zhanghaicheng,  He should be to do the task .
            v = BP.DA.DBAccess.RunSQLReturnValInt("SELECT COUNT(*) from wf_empWorks where WorkID=" + this.workID + " AND FK_Emp='zhanghaicheng' ", 100);
            if (v != 1)
                throw new Exception("@ Should not be do not find his .");

            //  Looking from the task pool .
            dt = BP.WF.Dev2Interface.DB_TaskPool();
            isHave = false;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["WorkID"].ToString() == this.workID.ToString())
                {
                    isHave = true;
                    break;
                }
            }
            if (isHave == false)
                throw new Exception("@ Do not find her to be in the task pool .");
            #endregion  Check that you have her to-do .
        }
    }
}
