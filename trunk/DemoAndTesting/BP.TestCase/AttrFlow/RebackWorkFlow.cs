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

namespace BP.CT
{
    public class RebackWorkFlow : TestBase
    {
        /// <summary>
        ///  Rollback of a process 
        /// </summary>
        public RebackWorkFlow()
        {
            this.Title = " Rollback of a process ";
            this.DescIt = " After the process is complete , Due to various reasons, it needs to be rolled back .";
            this.EditState = EditState.Passed;
        }
        public override void Do()
        {
            //  Implementation of the completion of a process .
            Int64 workid = this.RunCompeleteOneWork();

            BP.WF.Dev2Interface.Port_Login("admin");

            // Restored to the last node .
            BP.WF.Dev2Interface.Flow_DoRebackWorkFlow("024", workid, 0, "test");

            #region  Check the data is complete .
            string sql = "SELECT COUNT(*) AS N FROM WF_EmpWorks where fk_emp='zhanghaicheng' and workid=" + workid;
            DataTable dt= DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count==0)
                throw new Exception("@ Should not not be found  zhanghaicheng  To-do .");
            #endregion  Check the data is complete .

            // So he sent down .
         //   BP.WF.Dev2Interface.Port_Login("zhanghaicheng");
        //    BP.WF.Dev2Interface.Node_SendWork("024", workid);

            // Go back to the second node .
            workid = this.RunCompeleteOneWork();
            BP.WF.Dev2Interface.Flow_DoRebackWorkFlow("024", workid, 2402, "test");

            #region  Check the data is complete .
            sql = "SELECT COUNT(*) AS N FROM WF_EmpWorks where fk_emp='zhoupeng' and workid=" + workid;
             dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
                throw new Exception("@ Should not not be found  zhoupeng  To-do .");
            #endregion  Check the data is complete .
        }
        /// <summary>
        ///  After running a process , And returns it workid.
        /// </summary>
        /// <returns></returns>
        public Int64 RunCompeleteOneWork()
        {
            string fk_flow = "024";
            string startUser = "zhanghaicheng";
            BP.Port.Emp starterEmp = new Port.Emp(startUser);

            Flow fl = new Flow(fk_flow);

            //让zhanghaicheng Log in ,  After , You can access WebUser.No, WebUser.Name .
            BP.WF.Dev2Interface.Port_Login(startUser);

            // Create a blank ,  Initiating the start node .
            Int64 workid = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, WebUser.No, null, 0, null, 0, null);
            // Performing transmission , Send and retrieve objects ,.
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid);

            // The second step  :  .
            BP.WF.Dev2Interface.Port_Login(objs.VarAcceptersID);
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid);

            // The third step is to complete the implementation of :  .
            BP.WF.Dev2Interface.Port_Login(objs.VarAcceptersID);
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid);
            return workid;
        }
    }
}
