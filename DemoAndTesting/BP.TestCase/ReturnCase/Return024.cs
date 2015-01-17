using System;
using System.Collections.Generic;
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

namespace BP.CT.ReturnCase
{
    public class Return024 : TestBase
    {
        /// <summary>
        ///  Test return 
        /// </summary>
        public Return024()
        {
            this.Title = " Return data coverage patterns ";
            this.DescIt = " Send bounce , And backtrack the way of return .";
            this.EditState = EditState.Passed;
        }
        /// <summary>
        ///  Explanation  : This test is targeted at the demo environment  001  Process writing unit test code .
        ///  Relates to :  Create , Send , Revocation , Direction of the condition , Return other functions .
        /// </summary>
        public override void Do()
        {
            this.Test1();
        }
        public void Test1()
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

            // Under a worker .
            string nextUser = objs.VarAcceptersID;

            //  Transmission of the next node ID
            int nextNodeID = objs.VarToNodeID;

            // 让 nextUser = zhoupeng  Log in .
            BP.WF.Dev2Interface.Port_Login(nextUser);

            // Get set to return on the second node .
            DataTable dt = BP.WF.Dev2Interface.DB_GenerWillReturnNodes(objs.VarToNodeID, workid, 0);

            #region  Check that the second step to obtain return node data in line with expectations .
            if (dt.Rows.Count != 1)
                throw new Exception("@ At the second node is to obtain a set of nodes returned , Data do not meet expectations , Should only get a return node , It is :" + dt.Rows.Count);

            int nodeID = int.Parse(dt.Rows[0]["No"].ToString());
            if (nodeID != 2401)
                throw new Exception("@ At the second node is to obtain a set of nodes returned , The point should be returned 2401");

            string RecNo = dt.Rows[0]["Rec"].ToString();
            if (RecNo != startUser)
                throw new Exception("@ At the second node is to obtain a set of nodes returned , People should be returned " + startUser + ", It is " + RecNo);
            #endregion  Check that the second step to obtain return node data in line with expectations .

            // The second node performs the return .
            BP.WF.Dev2Interface.Node_ReturnWork(fk_flow, workid, 0,
                objs.VarToNodeID, 2401, " Return test ", false);

            #region  Data integrity checks returned after .
            GenerWorkFlow gwf = new GenerWorkFlow(workid);
            if (gwf.WFState != WFState.ReturnSta)
                throw new Exception("@ Implementation of return , Process state should be returned , It is :" + gwf.WFState.ToString());

            if (gwf.FK_Node != 2401)
                throw new Exception("@ Implementation of return , The current node should be 2401,  It is " + gwf.FK_Node.ToString());

            // Check whether the report meets the needs of the process .
            sql = "SELECT * FROM " + fl.PTable + " WHERE oid=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ Process report data is deleted .");

            foreach (DataColumn dc in dt.Columns)
            {
                string val = dt.Rows[0][dc.ColumnName].ToString();
                switch (dc.ColumnName)
                {
                    case GERptAttr.Title:
                        if (string.IsNullOrEmpty(val))
                            throw new Exception("@ After the return process lost title ");
                        break;
                    case GERptAttr.FID:
                        if (val != "0")
                            throw new Exception("@ It should be 0");
                        break;
                    case GERptAttr.FK_Dept:
                        if (val != starterEmp.FK_Dept)
                            throw new Exception("@ The department sponsors changed , It should be (" + starterEmp.FK_Dept + "), It is :" + val);
                        break;
                    case GERptAttr.FK_NY:
                        if (val != DataType.CurrentYearMonth)
                            throw new Exception("@ It should be " + DataType.CurrentYearMonth + ",  It is :" + val);
                        break;
                    case GERptAttr.FlowDaySpan:
                        if (val != "0")
                            throw new Exception("@ It should be  0 ,  It is :" + val);
                        break;
                    //case GERptAttr.FlowEmps:
                    //    if (val.Contains("zhanghaicheng") == false || val.Contains("zhoupeng") == false)
                    //        throw new Exception("@ Personnel should be included , Now do not exist ,  It is :" + val);
                    //    break;
                    //case GERptAttr.FlowEnder:
                    //    if (val != "zhanghaicheng")
                    //        throw new Exception("@ It should be  zhanghaicheng ,  It is :" + val);
                    //    break;
                    case GERptAttr.FlowEnderRDT:
                        if (val.Contains(DataType.CurrentData) == false)
                            throw new Exception("@ It should be   The current date ,  It is :" + val);
                        break;
                    case GERptAttr.FlowEndNode:
                        if (val != "2401")
                            throw new Exception("@ It should be  2401,  It is :" + val);
                        break;
                    case GERptAttr.FlowStarter:
                        if (val != startUser)
                            throw new Exception("@ It should be  " + startUser + ",  It is :" + val);
                        break;
                    case GERptAttr.FlowStartRDT:
                        if (string.IsNullOrEmpty(val))
                            throw new Exception("@ It should not be empty , It is :" + val);
                        break;
                    case GERptAttr.WFState:
                        if (int.Parse(val) != (int)WFState.ReturnSta)
                            throw new Exception("@ It should be   WFState.Complete  It is " + val);
                        break;
                    default:
                        break;
                }
            }
            #endregion  Data integrity checks returned after .
        }
    }
}
