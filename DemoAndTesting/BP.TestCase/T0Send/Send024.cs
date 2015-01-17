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

namespace BP.CT.T0Send
{
    /// <summary>
    ///  Linear process node sends 
    /// </summary>
    public  class Send024 : TestBase
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
        public BP.Port.Emp starterEmp = null;
        #endregion  Variable 

        /// <summary>
        ///  Linear process node sends 
        /// </summary>
        public Send024()
        {
            this.Title = " Linear process node sends ";
            this.DescIt = " Process :024 Simple 3 Node ( Consolidation mode ).";
            this.EditState = EditState.Passed;
        }
        /// <summary>
        ///  Process Description :
        /// 1, In the process  024 The simplest 3 Node ( Track mode ),  To test .
        /// 2, Only test transmission function , After checking with the data sent is complete .
        /// 3,  This test has three node initiates points , Intermediate point , End point , Corresponding to the three test methods .
        /// </summary>
        public override void Do()
        {
            #region  Definition of variables .
            fk_flow = "024";
            userNo = "zhanghaicheng";
            fl = new Flow(fk_flow);
            #endregion  Definition of variables .

            // Implementation of 1 Step . 让 zhanghaicheng  Initiate the process .
            this.Step1();

            // Implementation of 2 Step . 让zhoupeng  Deal with .
            this.Step2();

            // Implementation of 3 Step . 让zhanghaicheng  End .
            this.Step3();
        }
        /// <summary>
        ///  Step 1 让zhanghaicheng  Initiate the process .
        /// </summary>
        public void Step1()
        {
            // Assigned to the sponsor .
            starterEmp = new Port.Emp(userNo);

            //让 userNo  Log in .
            BP.WF.Dev2Interface.Port_Login(userNo);

            // Create a blank ,  Initiating the start node .
            workID = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, WebUser.No, null, 0, null, 0, null);

            #region  Check whether the work is created in line with expectations .
            // Data inspection process table .
            sql = "SELECT * FROM " + fl.PTable + " WHERE OID=" + workID;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ Process report data is deleted .");
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
                        if (val != WebUser.FK_Dept)
                            throw new Exception("@ It should be " + WebUser.FK_Dept + ",  It is :" + val);
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
                        if (BP.WF.Glo.UserInfoShowModel != UserInfoShowModel.UserNameOnly)
                        {
                            if (val.Contains(WebUser.No) == false)
                                throw new Exception("@ Current staff should be included ,  It is :" + val);
                        }
                        break;
                    case GERptAttr.FlowEnder:
                        if (val != WebUser.No)
                            throw new Exception("@ It should be   Current staff ,  It is :" + val);
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
                        if (val != WebUser.No)
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
                        if (sta != WFState.Blank)
                            throw new Exception("@ It should be   WFState.Blank  It is " + sta.ToString());
                        break;
                    default:
                        break;
                }
            }
            #endregion  Check whether the work is created in line with expectations 

            // Performing transmission .
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workID, null, null, 0, "zhoupeng");

            #region 第1步:  Send object inspection .
            // Get in objects obtained from sending to the next worker . zhangyifan( Yifan Zhang ),zhoushengyu( Week liter rain ).
            if (objs.VarAcceptersID != "zhoupeng")
                throw new Exception("@ Next recipient is incorrect ,  It should be : zhoupeng. It is :" + objs.VarAcceptersID);

            if (objs.VarToNodeID != 2402)
                throw new Exception("@ Should be  2401  Node .  It is :" + objs.VarToNodeID);

            if (objs.VarWorkID != workID)
                throw new Exception("@ Main thread workid Should not change :" + objs.VarWorkID);

            if (objs.VarCurrNodeID != 2401)
                throw new Exception("@ Can not change the number of the current node :" + objs.VarCurrNodeID);

            if (objs.VarTreadWorkIDs != null)
                throw new Exception("@ Should not get the child thread WorkID.");
            #endregion 第1步:  Send object inspection .

            #region 第2步:  Check the Process Engine table .
            // Creating this blank check whether there is data integrity ?
            sql = "SELECT * FROM WF_EmpWorks WHERE WorkID=" + workID + " AND FK_Emp='" + objs.VarAcceptersID + "'";
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
                throw new Exception("@ Should not be do not find the current staff .");

            gwf = new GenerWorkFlow(workID);
            if (gwf.Starter != WebUser.No || gwf.StarterName != WebUser.Name)
                throw new Exception(" The information is not written promoters .");

            if (gwf.FK_Dept != starterEmp.FK_Dept)
                throw new Exception("@ The department sponsors have changed , It should be " + starterEmp.FK_Dept + ", It is :" + gwf.FK_Dept);

            if (gwf.Starter != starterEmp.No)
                throw new Exception("@ Promoters  No  Changes , It should be " + starterEmp.No + ", It is :" + gwf.Starter);

            // Determine the current point .
            if (gwf.FK_Node != 2402)
                throw new Exception("@ The current point should be  2402  It is :" + gwf.FK_Node);

            // Determine the current point .
            if (gwf.FID != 0)
                throw new Exception("@ The current point should be  FID=0   It is :" + gwf.FID);

            // Judge PWorkID, No one calls it , It should be  0. 
            if (gwf.PWorkID != 0)
                throw new Exception("@ No one calls it ,  The current point should be  PWorkID=0   It is :" + gwf.PWorkID);

            // Judge  WFState . 
            if (gwf.WFState != WFState.Runing)
                throw new Exception("@ It should be  WFState=Runing  It is :" + gwf.WFState.ToString());

            // Check the start node   Sender WF_GenerWorkerList 的.
            gwl = new GenerWorkerList();
            gwl.FK_Emp = Web.WebUser.No;
            gwl.FK_Node = 2401;
            gwl.WorkID = workID;
            gwl.Retrieve();

            //  It should be no division confluence  0 .
            if (gwl.FID != 0)
                throw new Exception("@ It should be no division confluence  0.");

            if (gwl.IsEnable == false)
                throw new Exception("@ Should be enabled status  ");

            if (gwl.IsPass == false)
                throw new Exception("@ The state should be adopted  ");

            if (gwl.Sender.Contains(WebUser.No) == false)
                throw new Exception("@ Should be   Contains the current status  . ");


            // Check the recipient  WF_GenerWorkerList 的.
            gwl = new GenerWorkerList();
            gwl.FK_Emp = objs.VarAcceptersID;
            gwl.FK_Node = 2402;
            gwl.WorkID = workID;
            gwl.Retrieve();

            //  It should be no division confluence  0 .
            if (gwl.FID != 0)
                throw new Exception("@ It should be no division confluence  0.");

            if (gwl.IsEnable == false)
                throw new Exception("@ Should be enabled status  ");

            if (gwl.IsPass == true)
                throw new Exception("@ Should be the state does not pass  ");

            if (gwl.Sender.Contains(WebUser.No) == false)
                throw new Exception("@ Should be   Current human transmission , It is : " + gwl.Sender);
            #endregion 第2步:  Check the Process Engine table .

            #region 第3步:  Check the node data table .
            // Data inspection process table .
            sql = "SELECT * FROM " + fl.PTable + " WHERE OID=" + workID;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ Process report data is deleted .");
            foreach (DataColumn dc in dt.Columns)
            {
                string val = dt.Rows[0][dc.ColumnName].ToString();
                switch (dc.ColumnName)
                {
                    case GERptAttr.PWorkID:
                        if (val != "0")
                            throw new Exception("@PWorkID It should be 0,  It is :" + val);
                        break;
                    case GERptAttr.PFlowNo:
                        if (val != "")
                            throw new Exception("@PFlowNo It should be  ''  It is :" + val);
                        break;
                    case GERptAttr.FID:
                        if (val != "0")
                            throw new Exception("@ It should be 0");
                        break;
                    case GERptAttr.FK_Dept:
                        if (val != starterEmp.FK_Dept)
                            throw new Exception("@ It should be " + starterEmp.FK_Dept + ",  It is :" + val);
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
                        if (BP.WF.Glo.UserInfoShowModel != UserInfoShowModel.UserNameOnly)
                        {
                            if (val.Contains(WebUser.No) == false)
                                throw new Exception("@ Current staff should be included ,  It is :" + val);
                        }
                        break;
                    case GERptAttr.FlowEnder:
                        if (val != WebUser.No)
                            throw new Exception("@ It should be   Current staff ,  It is :" + val);
                        break;
                    case GERptAttr.FlowEnderRDT:
                        if (val.Contains(DataType.CurrentData) == false)
                            throw new Exception("@ It should be   The current date ,  It is :" + val);
                        break;
                    case GERptAttr.FlowEndNode:
                        if (val != "2402")
                            throw new Exception("@ It should be  2402,  It is :" + val);
                        break;
                    case GERptAttr.FlowStarter:
                        if (val != starterEmp.No)
                            throw new Exception("@ It should be   " + starterEmp.No + ",  It is :" + val);
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
            #endregion 第3步:  Check the node data table .
        }

        /// <summary>
        ///  Step 1 让zhoupeng  Login to deal .
        /// </summary>
        public void Step2()
        {
            //让 zhouepng  Log in .
            BP.WF.Dev2Interface.Port_Login("zhoupeng");

            // So he sent down .
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workID, null, null, 0, "zhanghaicheng");

            #region 第1步:  Send object inspection .
            // Get in objects obtained from sending to the next worker . zhangyifan( Yifan Zhang ),zhoushengyu( Week liter rain ).
            if (objs.VarAcceptersID != "zhanghaicheng")
                throw new Exception("@ Next recipient is incorrect ,  It should be : zhanghaicheng. It is :" + objs.VarAcceptersID);

            if (objs.VarToNodeID != 2499)
                throw new Exception("@ Should be  2499  Node .  It is :" + objs.VarToNodeID);

            if (objs.VarWorkID != workID)
                throw new Exception("@ Main thread workid Should not change :" + objs.VarWorkID);

            if (objs.VarCurrNodeID != 2402)
                throw new Exception("@ Can not change the number of the current node , It is :" + objs.VarCurrNodeID);

            if (objs.VarTreadWorkIDs != null)
                throw new Exception("@ Should not get the child thread WorkID.");

            #endregion 第1步:  Send object inspection .

            #region 第2步:  Check the Process Engine table .
            // Upcoming check whether there .
            sql = "SELECT * FROM WF_EmpWorks WHERE WorkID=" + workID + " AND FK_Emp='" + objs.VarAcceptersID + "'";
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
                throw new Exception("@ Should not be do not find the current staff .");

            gwf = new GenerWorkFlow(workID);
            if (gwf.Starter != "zhanghaicheng")
                throw new Exception(" Sponsor information changes .");

            if (gwf.FK_Dept != starterEmp.FK_Dept)
                throw new Exception("@ The department sponsors have changed , It should be " + starterEmp.FK_Dept + ", It is :" + gwf.FK_Dept);

            if (gwf.Starter != starterEmp.No)
                throw new Exception("@ Promoters  No  Changes , It should be " + starterEmp.No + ", It is :" + gwf.Starter);


            // Determine the current point .
            if (gwf.FK_Node != 2499)
                throw new Exception("@ The current point should be  2499  It is :" + gwf.FK_Node);

            // Determine the current point .
            if (gwf.FID != 0)
                throw new Exception("@ The current point should be  FID=0   It is :" + gwf.FID);

            // Judge PWorkID, No one calls it , It should be  0. 
            if (gwf.PWorkID != 0)
                throw new Exception("@ No one calls it ,  The current point should be  PWorkID=0   It is :" + gwf.PWorkID);

            // Judge  WFState . 
            if (gwf.WFState != WFState.Runing)
                throw new Exception("@ It should be  WFState=Runing  It is :" + gwf.WFState.ToString());

            // Check the start node   Sender WF_GenerWorkerList 的.
            gwl = new GenerWorkerList();
            gwl.FK_Emp = Web.WebUser.No;
            gwl.FK_Node = 2402;
            gwl.WorkID = workID;
            gwl.Retrieve();

            //  It should be no division confluence  0 .
            if (gwl.FID != 0)
                throw new Exception("@ It should be no division confluence  0.");

            if (gwl.IsEnable == false)
                throw new Exception("@ Should be enabled status  ");

            if (gwl.IsPass == false)
                throw new Exception("@ The state should be adopted  ");

            if (gwl.Sender.Contains("zhanghaicheng") == false)
                throw new Exception("@ Should be   Contains the current status  . ");

            // Check the recipient  WF_GenerWorkerList 的.
            gwl = new GenerWorkerList();
            gwl.FK_Emp = objs.VarAcceptersID;
            gwl.FK_Node = 2499;
            gwl.WorkID = workID;
            gwl.Retrieve();

            //  It should be no division confluence  0 .
            if (gwl.FID != 0)
                throw new Exception("@ It should be no division confluence  0.");

            if (gwl.IsEnable == false)
                throw new Exception("@ Should be enabled status  ");

            if (gwl.IsPass == true)
                throw new Exception("@ Should be the state does not pass  ");

            if (gwl.Sender.Contains(WebUser.No) == false)
                throw new Exception("@ Should be   Current human transmission , It is : " + gwl.Sender);
            #endregion 第2步:  Check the Process Engine table .

            #region 第3步:  Check the node data table .
            // Data inspection process table .
            sql = "SELECT * FROM " + fl.PTable + " WHERE OID=" + workID;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ Process report data is deleted .");
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
                        BP.Port.Emp emp = new Port.Emp("zhanghaicheng");
                        if (val != emp.FK_Dept)
                            throw new Exception("@ It should be " + emp.FK_Dept + ",  It is :" + val);
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
                        if (BP.WF.Glo.UserInfoShowModel != UserInfoShowModel.UserNameOnly)
                        {
                            if (val.Contains(WebUser.No) == false)
                                throw new Exception("@ Current staff should be included ,  It is :" + val);
                        }
                        break;
                    case GERptAttr.FlowEnder:
                        if (val != WebUser.No)
                            throw new Exception("@ It should be   Current staff ,  It is :" + val);
                        break;
                    case GERptAttr.FlowEnderRDT:
                        if (val.Contains(DataType.CurrentData) == false)
                            throw new Exception("@ It should be   The current date ,  It is :" + val);
                        break;
                    case GERptAttr.FlowEndNode:
                        if (val != "2499")
                            throw new Exception("@ It should be  2499,  It is :" + val);
                        break;
                    case GERptAttr.FlowStarter:
                        if (val == WebUser.No)
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
                        if (int.Parse(val) != (int)WFState.Runing)
                            throw new Exception("@ It should be   WFState.Runing  It is " + val);
                        break;
                    default:
                        break;
                }
            }
            #endregion 第3步:  Check the node data table .
        }

        /// <summary>
        ///  Step 3 让zhanghaicheng  End Process .
        /// </summary>
        public void Step3()
        {
            //让 zhanghaicheng  Log in .
            BP.WF.Dev2Interface.Port_Login("zhanghaicheng");

            // So he sent down .
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workID);

            #region 第1步:  Send object inspection .
            // Get in objects obtained from sending to the next worker . zhangyifan( Yifan Zhang ),zhoushengyu( Week liter rain ).
            if (objs.VarAcceptersID != null)
                throw new Exception("@ Personnel shall accept empty ." + objs.VarAcceptersID);

            if (objs.VarToNodeID != 0)
                throw new Exception("@ It should be  0   It is :" + objs.VarToNodeID);

            if (objs.VarWorkID != workID)
                throw new Exception("@ Main thread workid Should not change :" + objs.VarWorkID);

            if (objs.VarCurrNodeID != 2499)
                throw new Exception("@ Can not change the number of the current node , It is :" + objs.VarCurrNodeID);

            if (objs.VarTreadWorkIDs != null)
                throw new Exception("@ Should not get the child thread WorkID.");

            #endregion 第1步:  Send object inspection .

            #region 第2步:  Check the Process Engine table .
            // Upcoming check whether there .
            sql = "SELECT * FROM WF_EmpWorks WHERE WorkID=" + workID;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 0)
                throw new Exception("@ Should not be run .");


            sql = "SELECT * FROM WF_GenerWorkFlow WHERE WorkID=" + workID;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 0)
                throw new Exception("@ Process information is not deleted .");

            sql = "SELECT * FROM WF_GenerWorkerList  WHERE WorkID=" + workID;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 0)
                throw new Exception("@ Staff information is not deleted ..");
            #endregion 第2步:  Check the Process Engine table .

            #region 第3步:  Check the node data table .
            BP.Port.Emp emp=new Port.Emp("zhanghaicheng");

            // Data inspection process table .
            sql = "SELECT * FROM " + fl.PTable + " WHERE OID=" + workID;
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
                            throw new Exception("@ After the process has completed the title is lost ");
                        break;
                    case GERptAttr.FID:
                        if (val != "0")
                            throw new Exception("@ It should be 0");
                        break;
                    case GERptAttr.FK_Dept:
                        if (val != emp.FK_Dept)
                            throw new Exception("@ The department sponsors changed , It should be ("+emp.FK_Dept+"), It is :" + val);
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
                        if (BP.WF.Glo.UserInfoShowModel != UserInfoShowModel.UserNameOnly)
                        {
                            if (val.Contains("zhanghaicheng") == false || val.Contains("zhoupeng") == false)
                                throw new Exception("@ Personnel should be included , Now do not exist ,  It is :" + val);
                        }
                        break;
                    case GERptAttr.FlowEnder:
                        if (val != "zhanghaicheng")
                            throw new Exception("@ It should be  zhanghaicheng ,  It is :" + val);
                        break;
                    case GERptAttr.FlowEnderRDT:
                        if (val.Contains(DataType.CurrentData) == false)
                            throw new Exception("@ It should be   The current date ,  It is :" + val);
                        break;
                    case GERptAttr.FlowEndNode:
                        if (val != "2499")
                            throw new Exception("@ It should be  2499,  It is :" + val);
                        break;
                    case GERptAttr.FlowStarter:
                        if (val != "zhanghaicheng")
                            throw new Exception("@ It should be  zhanghaicheng,  It is :" + val);
                        break;
                    case GERptAttr.FlowStartRDT:
                        if (string.IsNullOrEmpty(val))
                            throw new Exception("@ It should not be empty , It is :" + val);
                        break;
                    case GERptAttr.WFState:
                        if (int.Parse(val) != (int)WFState.Complete)
                            throw new Exception("@ It should be   WFState.Complete  It is " + val);
                        break;
                    default:
                        break;
                }
            }
            #endregion 第3步:  Check the node data table .
        }
    }
}
