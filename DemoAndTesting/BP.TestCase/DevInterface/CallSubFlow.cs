using System;
using System.Collections.Generic;
using System.Text;
using BP.WF;
using BP.WF.Data;
using BP.WF.Template;
using BP.En;
using BP.DA;
using BP.Web;
using BP.Port;
using System.Data;
using System.Collections;

namespace BP.CT
{
    /// <summary>
    ///  Sons Process 
    /// </summary>
    public  class CallSubFlow : TestBase
    {
        /// <summary>
        ///  Sons Process 
        /// </summary>
        public CallSubFlow()
        {
            this.Title = " Sons Process ";
            this.DescIt = " Test call  Subprocess ,以023 与024 Process for instance .";
            this.EditState = CT.EditState.Passed;
        }
        /// <summary>
        ///  Explanation  : Sons Process 
        ///  Relates to :   Other functions .
        /// </summary>
        public override void Do()
        {
            // Only one sub-process test .
            Test1();

            // Start node sub-processes have a group of people .
            Test2();
        }
       
        /// <summary>
        ///  Only one sub-process test 
        /// </summary>
        private void Test1()
        {
            string fk_flow = "023";
            string userNo = "zhanghaicheng";
            Flow fl = new Flow(fk_flow);

            // zhanghaicheng  Log in .
            BP.WF.Dev2Interface.Port_Login(userNo);

            // Create a blank ,  Initiating the start node .
            Int64 workid = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, WebUser.No, null, 0, null,0,null);
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid);


            // Creating the first sub-processes , Only one person calling interface .
            Emp empSubFlowStarter = new Emp("zhoupeng");
            Int64 subFlowWorkID = BP.WF.Dev2Interface.Node_CreateStartNodeWork("024", null, null, empSubFlowStarter.No,
                " Sub-processes initiated testing ", workid,"023",0);

            #region  Check the sub-processes initiated   Process Engine table   Is complete ?
            GenerWorkFlow gwf = new GenerWorkFlow(subFlowWorkID);
            if (gwf.PFlowNo != "023")
                throw new Exception("@ Parent process ID error , It should be 023 It is " + gwf.PFlowNo);

            if (gwf.PWorkID != workid)
                throw new Exception("@ Parent process WorkID Error , It should be " + workid + " It is " + gwf.PWorkID);

            if (gwf.Starter != empSubFlowStarter.No)
                throw new Exception("@ Flow promoters number error , It should be " + empSubFlowStarter.No + " It is " + gwf.Starter);

            if (gwf.StarterName != empSubFlowStarter.Name)
                throw new Exception("@ Process sponsor  Name  Error , It should be " + empSubFlowStarter.Name + " It is " + gwf.StarterName);

            if (gwf.FK_Dept != empSubFlowStarter.FK_Dept)
                throw new Exception("@ Process membership department error , It should be " + empSubFlowStarter.FK_Dept + " It is " + gwf.FK_Dept);

            if (gwf.Title != " Sub-processes initiated testing ")
                throw new Exception("@ Process title   Sub-processes initiated testing   Error , It should be   Sub-processes initiated testing   It is " + gwf.Title);

            if (gwf.WFState != WFState.Runing)
                throw new Exception("@ Process  WFState  Error , It should be  Runing  It is " + gwf.WFState);

            if (gwf.FID != 0)
                throw new Exception("@FID Error , It should be 0 It is " + gwf.FID);

            if (gwf.FK_Flow != "024")
                throw new Exception("@FK_Flow Error , It should be 024 It is " + gwf.FK_Flow);

            if (gwf.FK_Node != 2401)
                throw new Exception("@ Stay current node error , It should be 2401 It is " + gwf.FK_Flow);

            GenerWorkerLists gwls = new GenerWorkerLists(subFlowWorkID, 2401);
            if (gwls.Count != 1)
                throw new Exception("@ The number of to-do list should 1, It is " + gwls.Count);


            //  Check the sponsor list is complete ?
            GenerWorkerList gwl = (GenerWorkerList)gwls[0];
            if (gwl.FK_Emp != empSubFlowStarter.No)
                throw new Exception("@ Treatment of human error , It is :" + empSubFlowStarter.No);

            if (gwl.IsPassInt != 0)
                throw new Exception("@ Should not pass through the state , It is :" + gwl.IsPassInt);

            if (gwl.FID != 0)
                throw new Exception("@ Process ID  It should be 0 , It is :" + gwl.FID);

            if (gwl.FK_EmpText != empSubFlowStarter.Name)
                throw new Exception("@FK_EmpText   Error ,  It is :" + gwl.FK_EmpText);

            #endregion  Check the sub-processes initiated   Process Engine table   Is complete ?

            #region  Sub-processes data integrity checking whether initiated ?
            // Check the report data is complete ?
            sql = "SELECT * FROM ND24Rpt WHERE OID=" + subFlowWorkID;
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
                throw new Exception("@ Initiate the process error , Can not find the child process should not report data .");

            foreach (DataColumn dc in dt.Columns)
            {
                string val = dt.Rows[0][dc.ColumnName].ToString();
                switch (dc.ColumnName)
                {
                    case GERptAttr.FID:
                        if (val != "0")
                            throw new Exception("@ It should be 0");
                        break;
                    case GERptAttr.FK_Dept:
                        if (val != empSubFlowStarter.FK_Dept)
                            throw new Exception("@ It should be " + empSubFlowStarter.FK_Dept + ",  It is :" + val);
                        break;
                    case GERptAttr.FK_NY:
                        if (val != DataType.CurrentYearMonth)
                            throw new Exception("@ It should be " + DataType.CurrentYearMonth + ",  It is :" + val);
                        break;
                    case GERptAttr.FlowDaySpan:
                        if (val != "0")
                            throw new Exception("@ It should be  0 ,  It is :" + val);
                        break;
                    case GERptAttr.FlowEmps:
                        if (val.Contains(empSubFlowStarter.No) == false)
                            throw new Exception("@ Current staff should be included ,  It is :" + val);
                        break;
                    case GERptAttr.FlowEnder:
                        if (val != empSubFlowStarter.No)
                            throw new Exception("@ It should be  empSubFlowStarter.No,  It is :" + val);
                        break;
                    case GERptAttr.FlowEnderRDT:
                        if (val.Contains(DataType.CurrentData) == false)
                            throw new Exception("@ It should be   The current date ,  It is :" + val);
                        break;
                    case GERptAttr.FlowEndNode:
                        if (val != "2401")
                            throw new Exception("@ It should be  2401,  It is :" + val);
                        break;
                    case GERptAttr.FlowStarter:
                        if (val != empSubFlowStarter.No)
                            throw new Exception("@ It should be   WebUser.No,  It is :" + val);
                        break;
                    case GERptAttr.FlowStartRDT:
                        if (string.IsNullOrEmpty(val))
                            throw new Exception("@ It should not be empty , It is :" + val);
                        break;
                    case GERptAttr.Title:
                        if (string.IsNullOrEmpty(val))
                            throw new Exception("@ Can not be empty title" + val);
                        break;
                    case GERptAttr.WFState:
                        WFState sta = (WFState)int.Parse(val);
                        if (sta != WFState.Runing)
                            throw new Exception("@ It should be   WFState.Runing  It is " + sta.ToString());
                        break;
                    default:
                        break;
                }
            }
            #endregion  Check for complete ?

            //  Sent down to the sub-process test , Success ?
            BP.WF.Dev2Interface.Port_Login(empSubFlowStarter.No);
            objs = BP.WF.Dev2Interface.Node_SendWork("024", subFlowWorkID);
            if (objs.VarToNodeID != 2402)
                throw new Exception("@ Sub-process is unsuccessful when sending down .");
        }

        /// <summary>
        ///  Only one sub-process test 
        /// </summary>
        private void Test2()
        {
            string fk_flow = "023";
            string userNo = "zhanghaicheng";
            Flow fl = new Flow(fk_flow);

            // zhanghaicheng  Log in .
            BP.WF.Dev2Interface.Port_Login(userNo);

            // Create a blank ,  Initiating the start node .
            Int64 workid = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, WebUser.No, null, 0, null, 0, null);
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid); /* Send up to the second node */

            /* Create a sub-process .  Designated personnel to deal with the sub-process can handle is a collection . 
             *
             * 此api More than two parameters :
             * 1, This process is part of the department . 
             * 2, The people involved in the process of collection , Separated by commas .
             */

            Emp flowStarter = new Emp(WebUser.No);

            Int64 subFlowWorkID=0; // = new Emp(WebUser.No);

            //Int64 subFlowWorkID = BP.WF.Dev2Interface.Node_CreateStartNodeWork("024", null, null, "zhanghaicheng",
            //    "1", "zhoupeng,zhoushengyu", " Sub-processes initiated testing ( Create a multiplayer deal for the start node work )", "023", workid);


            #region  Check the sub-processes initiated   Process Engine table   Is complete ?
            GenerWorkFlow gwf = new GenerWorkFlow(subFlowWorkID);
            if (gwf.PFlowNo != "023")
                throw new Exception("@ Parent process ID error , It should be 023 It is " + gwf.PFlowNo);

            if (gwf.PWorkID != workid)
                throw new Exception("@ Parent process WorkID Error , It should be " + workid + " It is " + gwf.PWorkID);

            if (gwf.Starter != flowStarter.No)
                throw new Exception("@ Flow promoters number error , It should be " + flowStarter.No + " It is " + gwf.Starter);

            if (gwf.StarterName != flowStarter.Name)
                throw new Exception("@ Process sponsor  Name  Error , It should be " + flowStarter.Name + " It is " + gwf.StarterName);

            if (gwf.FK_Dept != "1")
                throw new Exception("@ Process membership department error , It should be   1  It is " + gwf.FK_Dept);

            if (gwf.Title != " Sub-processes initiated testing ( Create a multiplayer deal for the start node work )")
                throw new Exception("@ Process title   Sub-processes initiated testing   Error , It should be   Sub-processes initiated testing   It is " + gwf.Title);

            if (gwf.WFState != WFState.Runing)
                throw new Exception("@ Process  WFState  Error , It should be  Runing  It is " + gwf.WFState);

            if (gwf.FID != 0)
                throw new Exception("@FID Error , It should be 0 It is " + gwf.FID);

            if (gwf.FK_Flow != "024")
                throw new Exception("@FK_Flow Error , It should be 024 It is " + gwf.FK_Flow);

            if (gwf.FK_Node != 2401)
                throw new Exception("@ Stay current node error , It should be 2401 It is " + gwf.FK_Flow);

            GenerWorkerLists gwls = new GenerWorkerLists(subFlowWorkID, 2401);
            if (gwls.Count != 2)
                throw new Exception("@ The number of to-do list should 2, It is " + gwls.Count);

            //  Check the sponsor list is complete ?
            foreach (GenerWorkerList gwl in gwls)
            {
                if (gwl.IsPassInt != 0)
                    throw new Exception("@ Should not pass through the state , It is :" + gwl.IsPassInt);

                if (gwl.FID != 0)
                    throw new Exception("@ Process ID  It should be 0 , It is :" + gwl.FID);

                if (gwl.FK_Emp == "zhoupeng")
                {
                    Emp tempEmp = new Emp(gwl.FK_Emp);
                    if (gwl.FK_Dept != tempEmp.FK_Dept)
                        throw new Exception("@FK_Dept   Error ,  It is :" + gwl.FK_Dept);
                }
            }
            #endregion  Check the sub-processes initiated   Process Engine table   Is complete ?

            #region  Sub-processes data integrity checking whether initiated ?
            // Check the report data is complete ?
            sql = "SELECT * FROM ND24Rpt WHERE OID=" + subFlowWorkID;
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
                throw new Exception("@ Initiate the process error , Can not find the child process should not report data .");

            foreach (DataColumn dc in dt.Columns)
            {
                string val = dt.Rows[0][dc.ColumnName].ToString();
                switch (dc.ColumnName)
                {
                    case GERptAttr.PWorkID:
                        if (val != gwf.PWorkID.ToString() )
                            throw new Exception("@ It should be the parent process workid, Now is the :"+val);
                        break;
                    case GERptAttr.PFlowNo:
                        if (val != "023")
                            throw new Exception("@ It should be 023");
                        break;
                    case GERptAttr.FID:
                        if (val != "0")
                            throw new Exception("@ It should be 0");
                        break;
                    case GERptAttr.FK_Dept:
                        if (val != "1")
                            throw new Exception("@ It should be   1,  It is :" + val);
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
                    //    if (val.Contains(empSubFlowStarter.No) == false)
                    //        throw new Exception("@ Current staff should be included ,  It is :" + val);
                    //    break;
                    //case GERptAttr.FlowEnder:
                    //    if (val != empSubFlowStarter.No)
                    //        throw new Exception("@ It should be  empSubFlowStarter.No,  It is :" + val);
                    //    break;
                    case GERptAttr.FlowEnderRDT:
                        if (val.Contains(DataType.CurrentData) == false)
                            throw new Exception("@ It should be   The current date ,  It is :" + val);
                        break;
                    case GERptAttr.FlowEndNode:
                        if (val != "2401")
                            throw new Exception("@ It should be  2401,  It is :" + val);
                        break;
                    //case GERptAttr.FlowStarter:
                    //    if (val != empSubFlowStarter.No)
                    //        throw new Exception("@ It should be   WebUser.No,  It is :" + val);
                    //    break;
                    case GERptAttr.FlowStartRDT:
                        if (string.IsNullOrEmpty(val))
                            throw new Exception("@ It should not be empty , It is :" + val);
                        break;
                    case GERptAttr.Title:
                        if (string.IsNullOrEmpty(val))
                            throw new Exception("@ Can not be empty title" + val);
                        break;
                    case GERptAttr.WFState:
                        WFState sta = (WFState)int.Parse(val);
                        if (sta != WFState.Runing)
                            throw new Exception("@ It should be   WFState.Runing  It is " + sta.ToString());
                        break;
                    default:
                        break;
                }
            }
            #endregion  Check for complete ?

            //  Sent down to the sub-process test , Success ?
            BP.WF.Dev2Interface.Port_Login("zhoupeng");
            objs = BP.WF.Dev2Interface.Node_SendWork("024", subFlowWorkID);

            #region  Check the return to the variable data integrity .
            if (objs.VarToNodeID != 2402)
                throw new Exception("@ Sub-process is unsuccessful when sending down .");

            #endregion  Check data integrity .

            #region  Check for other people on the start node is there work to be done ?
            // Check the report data is complete ?
            sql = "SELECT * FROM WF_EmpWorks WHERE WorkID=" + subFlowWorkID + " AND FK_Emp='zhoushengyu'";
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 0)
                throw new Exception("@ After one start node processing is complete , Other people have a to-do .");

            #endregion  Check for other people on the start node is there work to be done .
        }
    }
}
