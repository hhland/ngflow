using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BP.WF;
using BP.WF.Data;
using BP.WF.Template;
using BP.En;
using BP.DA;
using BP.Web;
using System.Data;
using BP.CT;

namespace BP.CT.T0Send
{
    /// <summary>
    ///  Linear process node sends 
    /// </summary>
    public  class Send023 : TestBase
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
        /// <summary>
        ///  Sponsor 
        /// </summary>
        public BP.Port.Emp starterEmp = null;
        #endregion  Variable 

        /// <summary>
        ///  Linear process node sends 
        /// </summary>
        public Send023()
        {
            this.Title = " Linear process node sends ";
            this.DescIt = " Process :023 The simplest 3 Node ( Track mode ), Whether the data is sent to perform in line with expectations after .";
            this.EditState = EditState.Passed; // Have been able to live .
        }
   
        /// <summary>
        ///  Process Description :
        /// 1, In the process  023 The simplest 3 Node ( Track mode ),  To test .
        /// 2, Only test transmission function , After checking with the data sent is complete .
        /// 3,  This test has three node initiates points , Intermediate point , End point , Corresponding to the three test methods .
        /// </summary>
        public override void Do()
        {
            #region  Definition of variables .
            fk_flow = "023";
            userNo = "zhanghaicheng";
            fl = new Flow(fk_flow);
            #endregion  Definition of variables .

            // Implementation of 1 Step . 让zhanghaicheng  Initiate the process .
            this.Step1();

            // Implementation of 2 Step . 让 zhoupeng  Deal with .
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
            workID = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null,
                WebUser.No, null, 0, null, 0, null);

            #region  Check whether the work is created in line with expectations .
            // Check the node to start writing data is correct ?
            sql = "SELECT * FROM ND2301 WHERE OID=" + workID;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ Initiate the process error , Should not start node is not found data .");
            foreach (DataColumn dc in dt.Columns)
            {
                string val = dt.Rows[0][dc.ColumnName].ToString();
                switch (dc.ColumnName)
                {
                    case WorkAttr.CDT:
                        if (val.Contains(DataType.CurrentData) == false)
                            throw new Exception("CDT, Date error . The date is now :" + val);
                        break;
                    case WorkAttr.RDT:
                        if (val.Contains(DataType.CurrentData) == false)
                            throw new Exception("RDT, Date error . The date is now :"+val);
                        break;
                    case WorkAttr.Emps:
                        if (val.Contains(WebUser.No) == false)
                            throw new Exception(" It should contain the current staff , It is :" + val);
                        break;
                    case WorkAttr.FID:
                        if (val != "0")
                            throw new Exception(" Should  = 0, It is :" + val);
                        break;
                    case WorkAttr.MD5:
                        //if (Glo.ism
                        //if (val !="0")
                        //    throw new Exception(" Should  = 0, It is :"+val);
                        break;
                    case WorkAttr.MyNum:
                        if (val != "1")
                            throw new Exception(" Should  = 1, It is :" + val);
                        break;
                    case WorkAttr.Rec:
                        if (val != WebUser.No)
                            throw new Exception(" Should  Rec= " + WebUser.No + ", It is :" + val);
                        break;
                    //case WorkAttr.Sender:
                    //    if (val != WebUser.No)
                    //        throw new Exception(" Should  Sender= " + WebUser.No + ", It is :" + val);
                    //    break;
                    default:
                        break;
                }
            }


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
                        if (val != "2301")
                            throw new Exception("@ It should be  2301,  It is :" + val);
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

            // Define a parameter .
            Hashtable ht = new System.Collections.Hashtable();
            ht.Add("GoTo", 1);
            ht.Add("MyPara", "TestPara");

            // Performing transmission .
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workID,ht);

            #region 第1步:  Send object inspection .
            // Get in objects obtained from sending to the next worker . zhangyifan( Yifan Zhang ),zhoushengyu( Week liter rain ).
            if (objs.VarAcceptersID != "zhoupeng")
                throw new Exception("@ Next recipient is incorrect ,  It should be : zhoupeng. It is :" + objs.VarAcceptersID);

            if (objs.VarToNodeID != 2302)
                throw new Exception("@ Should be  2302  Node .  It is :" + objs.VarToNodeID);

            if (objs.VarWorkID != workID)
                throw new Exception("@ Main thread workid Should not change :" + objs.VarWorkID);

            if (objs.VarCurrNodeID != 2301)
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
            if (gwf.FK_Node != 2302)
                throw new Exception("@ The current point should be  2302  It is :" + gwf.FK_Node);

            // Judge TodoEmpsNum.
            if (gwf.TodoEmpsNum != 1)
                throw new Exception("@ The current point should be  TodoEmpsNum=0   It is :" + gwf.TodoEmpsNum);

            // Judge TodoEmps.
            if (gwf.TodoEmps != "zhoupeng, Zhou Peng ;")
                throw new Exception("@ The current point should be  TodoEmps=zhoupeng, Zhou Peng ;   It is :" + gwf.TodoEmps);

            // Judge FID.
            if (gwf.FID != 0)
                throw new Exception("@ The current point should be  FID=0   It is :" + gwf.FID);

            // Judge PWorkID, No one calls it , It should be  0. 
            if (gwf.PWorkID != 0)
                throw new Exception("@ No one calls it ,  The current point should be  PWorkID=0   It is :" + gwf.PWorkID);

            // Judge  WFState . 
            if (gwf.WFState != WFState.Runing)
                throw new Exception("@ It should be  WFState=Runing  It is :" + gwf.WFState.ToString());

            // Check the start node   Sender WF_GenerWorkerList的.
            gwl = new GenerWorkerList();
            gwl.CheckPhysicsTable();

            gwl.FK_Emp = Web.WebUser.No;
            gwl.FK_Node = 2301;
            gwl.WorkID = workID;
            gwl.Retrieve();

            //  It should be no division confluence  0 .
            if (gwl.FID != 0)
                throw new Exception("@ It should be no division confluence  0.");

            if (gwl.IsEnable == false)
                throw new Exception("@ Should be enabled status  ");

            if (gwl.IsPass == false)
                throw new Exception("@ The state should be adopted  ");

            if (gwl.Sender.Contains(WebUser.No)==false)
                throw new Exception("@ Should be   Contains the current status  . ");

            //if (gwl. != "@MyPara=TestPara@GoTo=1")
            //    throw new Exception("@ Parameters should be :@MyPara=TestPara@GoTo=1 . It is :" + gwl.Paras);

            // Check the recipient  WF_GenerWorkerList 的.
            gwl = new GenerWorkerList();
            gwl.FK_Emp = objs.VarAcceptersID;
            gwl.FK_Node = 2302;
            gwl.WorkID = workID;
            gwl.Retrieve();

            //  It should be no division confluence  0 .
            if (gwl.FID != 0)
                throw new Exception("@ It should be no division confluence  0.");

            if (gwl.IsEnable == false)
                throw new Exception("@ Should be enabled status  ");

            if (gwl.IsPass == true)
                throw new Exception("@ Should be the state does not pass  ");

            if (gwl.Sender.Contains(WebUser.No)==false)
                throw new Exception("@ Should be   Current human transmission , It is : " + gwl.Sender);
            #endregion 第2步:  Check the Process Engine table .

            #region 第3步:  Check the node data table .
            sql = "SELECT * FROM ND2301 WHERE OID=" + workID;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ Initiate the process error , Should not start node is not found data .");
            foreach (DataColumn dc in dt.Columns)
            {
                string val = dt.Rows[0][dc.ColumnName].ToString();
                switch (dc.ColumnName)
                {
                    case WorkAttr.CDT:
                        if (val.Contains(DataType.CurrentData) == false)
                            throw new Exception("CDT, Date error .");
                        break;
                    case WorkAttr.RDT:
                        if (val.Contains(DataType.CurrentData) == false)
                            throw new Exception("RDT, Date error .");
                        break;
                    case WorkAttr.Emps:
                        if (val.Contains(WebUser.No) == false)
                            throw new Exception(" It should contain the current staff , It is :" + val);
                        break;
                    case WorkAttr.FID:
                        if (val != "0")
                            throw new Exception(" Should  = 0, It is :" + val);
                        break;
                    case WorkAttr.MD5:
                        //if (Glo.ism
                        //if (val !="0")
                        //    throw new Exception(" Should  = 0, It is :"+val);
                        break;
                    case WorkAttr.MyNum:
                        if (val != "1")
                            throw new Exception(" Should  = 1, It is :" + val);
                        break;
                    case WorkAttr.Rec:
                        if (val != WebUser.No)
                            throw new Exception(" Should  Rec= " + WebUser.No + ", It is :" + val);
                        break;
                    //case WorkAttr.Sender:
                    //    if (val != WebUser.No)
                    //        throw new Exception(" Should  Sender= " + WebUser.No + ", It is :" + val);
                    //    break;
                    default:
                        break;
                }
            }

            // Check the node 2 Data .
            sql = "SELECT * FROM ND2302 WHERE OID=" + workID;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ Initiate the process error , Should not not be found  ND2302  Data .");
            foreach (DataColumn dc in dt.Columns)
            {
                string val = dt.Rows[0][dc.ColumnName].ToString();
                switch (dc.ColumnName)
                {
                    case WorkAttr.CDT:
                        if (val.Contains(DataType.CurrentData) == false)
                            throw new Exception("CDT, Date error .");
                        break;
                    case WorkAttr.RDT:
                        if (val.Contains(DataType.CurrentData) == false)
                            throw new Exception("RDT, Date error .");
                        break;
                    case WorkAttr.Emps:
                        if (val.Contains("zhoupeng") == false)
                            throw new Exception(" The second step of personnel , Should zhoupeng , It is :" + val);
                        break;
                    case WorkAttr.FID:
                        if (val != "0")
                            throw new Exception(" Should  = 0, It is :" + val);
                        break;
                    case WorkAttr.MD5:
                        //if (Glo.ism
                        //if (val !="0")
                        //    throw new Exception(" Should  = 0, It is :"+val);
                        break;
                    case WorkAttr.MyNum:
                        if (val != "1")
                            throw new Exception(" Should  = 1, It is :" + val);
                        break;
                    case WorkAttr.Rec:
                        if (val != "zhoupeng")
                            throw new Exception(" Should  Rec=zhoupeng, It is :" + val);
                        break;
                    default:
                        break;
                }
            }

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
                        if (val != "2302")
                            throw new Exception("@ It should be  2302,  It is :" + val);
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
        ///  Step 1 让 zhoupeng  Login to deal .
        /// </summary>
        public void Step2()
        {
            //让 zhouepng  Log in .
            BP.WF.Dev2Interface.Port_Login("zhoupeng");

            // So he sent down .
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow,workID);

            #region 第1步:  Send object inspection .
            // Get in objects obtained from sending to the next worker . zhangyifan( Yifan Zhang ),zhoushengyu( Week liter rain ).
            if (objs.VarAcceptersID != "zhanghaicheng")
                throw new Exception("@ Next recipient is incorrect ,  It should be : zhanghaicheng. It is :" + objs.VarAcceptersID);

            if (objs.VarToNodeID != 2399)
                throw new Exception("@ Should be  2399  Node .  It is :" + objs.VarToNodeID);

            if (objs.VarWorkID != workID)
                throw new Exception("@ Main thread workid Should not change :" + objs.VarWorkID);

            if (objs.VarCurrNodeID != 2302)
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

            if (gwf.FK_Dept != starterEmp.FK_Dept)
                throw new Exception("@ The department sponsors have changed , It should be " + starterEmp.FK_Dept + ", It is :" + gwf.FK_Dept);

            if (gwf.Starter != starterEmp.No)
                throw new Exception("@ Promoters  No  Changes , It should be " + starterEmp.No + ", It is :" + gwf.Starter);

            // Determine the current point .
            if (gwf.FK_Node != 2399)
                throw new Exception("@ The current point should be  2399  It is :" + gwf.FK_Node);

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
            gwl.FK_Node = 2302;
            gwl.WorkID = workID;
            gwl.Retrieve();

            //  It should be no division confluence  0 .
            if (gwl.FID != 0)
                throw new Exception("@ It should be no division confluence  0.");

            if (gwl.IsEnable == false)
                throw new Exception("@ Should be enabled status  ");

            if (gwl.IsPass == false)
                throw new Exception("@ The state should be adopted  ");

            if (gwl.Sender.Contains("zhanghaicheng")==false)
                throw new Exception("@ Should be   Contains the current status  . ");
             
            // Check the recipient  WF_GenerWorkerList 的.
            gwl = new GenerWorkerList();
            gwl.FK_Emp = objs.VarAcceptersID;
            gwl.FK_Node = 2399;
            gwl.WorkID = workID;
            gwl.Retrieve();

            //  It should be no division confluence  0 .
            if (gwl.FID != 0)
                throw new Exception("@ It should be no division confluence  0.");

            if (gwl.IsEnable == false)
                throw new Exception("@ Should be enabled status  ");

            if (gwl.IsPass == true)
                throw new Exception("@ Should be the state does not pass  ");

            if (gwl.Sender.Contains(WebUser.No)==false)
                throw new Exception("@ Should be   Current human transmission , It is : " + gwl.Sender);
            #endregion 第2步:  Check the Process Engine table .

            #region 第3步:  Check the node data table .
            sql = "SELECT * FROM ND2301 WHERE OID=" + workID;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ Initiate the process error , Should not start node is not found data .");
            foreach (DataColumn dc in dt.Columns)
            {
                string val = dt.Rows[0][dc.ColumnName].ToString();
                switch (dc.ColumnName)
                {
                    case WorkAttr.CDT:
                        if (val.Contains(DataType.CurrentData) == false)
                            throw new Exception("CDT, Date error .");
                        break;
                    case WorkAttr.RDT:
                        if (val.Contains(DataType.CurrentData) == false)
                            throw new Exception("RDT, Date error .");
                        break;
                    case WorkAttr.Emps:
                        if (val.Contains("zhanghaicheng") == false)
                            throw new Exception(" It should contain the current staff , It is :" + val);
                        break;
                    case WorkAttr.FID:
                        if (val != "0")
                            throw new Exception(" Should  = 0, It is :" + val);
                        break;
                    case WorkAttr.MyNum:
                        if (val != "1")
                            throw new Exception(" Should  = 1, It is :" + val);
                        break;
                    case WorkAttr.Rec:
                        if (val != objs.VarAcceptersID)
                            throw new Exception(" Should  Rec=zhanghaicheng, It is :" + val);
                        break;
                   
                    default:
                        break;
                }
            }

            // Check the node 2 Data .
            sql = "SELECT * FROM ND2302 WHERE OID=" + workID;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ Initiate the process error , Should not not be found  ND2302  Data .");
            foreach (DataColumn dc in dt.Columns)
            {
                string val = dt.Rows[0][dc.ColumnName].ToString();
                switch (dc.ColumnName)
                {
                    case WorkAttr.CDT:
                        if (val.Contains(DataType.CurrentData) == false)
                            throw new Exception("CDT, Date error .");
                        break;
                    case WorkAttr.RDT:
                        if (val.Contains(DataType.CurrentData) == false)
                            throw new Exception("RDT, Date error .");
                        break;
                    case WorkAttr.Emps:
                        if (val.Contains(WebUser.No) == false)
                            throw new Exception(" It should contain the current staff , It is :" + val);
                        break;
                    case WorkAttr.FID:
                        if (val != "0")
                            throw new Exception(" Should  = 0, It is :" + val);
                        break;
                    case WorkAttr.MyNum:
                        if (val != "1")
                            throw new Exception(" Should  = 1, It is :" + val);
                        break;
                    case WorkAttr.Rec:
                        if (val != "zhoupeng")
                            throw new Exception(" Should  Rec= zhoupeng, It is :" + val);
                        break;
                     
                    default:
                        break;
                }
            }

            // Check the node 3 Data .
            sql = "SELECT * FROM ND2399 WHERE OID=" + workID;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ Initiate the process error , Should not not be found  ND2399  Data .");
            foreach (DataColumn dc in dt.Columns)
            {
                string val = dt.Rows[0][dc.ColumnName].ToString();
                switch (dc.ColumnName)
                {
                    case WorkAttr.CDT:
                        if (val.Contains(DataType.CurrentData) == false)
                            throw new Exception("CDT, Date error .");
                        break;
                    case WorkAttr.RDT:
                        if (val.Contains(DataType.CurrentData) == false)
                            throw new Exception("RDT, Date error .");
                        break;
                    case WorkAttr.Emps:
                        if (val.Contains("zhanghaicheng") == false)
                            throw new Exception(" It should contain the current staff , It is :" + val);
                        break;
                    case WorkAttr.FID:
                        if (val != "0")
                            throw new Exception(" Should  = 0, It is :" + val);
                        break;
                    case WorkAttr.MyNum:
                        if (val != "1")
                            throw new Exception(" Should  = 1, It is :" + val);
                        break;
                    case WorkAttr.Rec:
                        if (val != objs.VarAcceptersID)
                            throw new Exception(" Should  Rec= " + objs.VarAcceptersID + ", It is :" + val);
                        break;
                   
                    default:
                        break;
                }
            }

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
                        if (val != "2399")
                            throw new Exception("@ It should be  2399,  It is :" + val);
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

            if (objs.VarCurrNodeID != 2399)
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
            sql = "SELECT * FROM ND2301 WHERE OID=" + workID;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ Initiate the process error , Should not start node is not found data .");
            foreach (DataColumn dc in dt.Columns)
            {
                string val = dt.Rows[0][dc.ColumnName].ToString();
                switch (dc.ColumnName)
                {
                    case GERptAttr.Title:
                        if (string.IsNullOrEmpty(val))
                            throw new Exception("@ After the process has completed the title is lost ");
                        break;
                    case WorkAttr.CDT:
                        if (val.Contains(DataType.CurrentData) == false)
                            throw new Exception("CDT, Date error .");
                        break;
                    case WorkAttr.RDT:
                        if (val.Contains(DataType.CurrentData) == false)
                            throw new Exception("RDT, Date error .");
                        break;
                    case WorkAttr.Emps:
                        if (val.Contains(WebUser.No) == false)
                            throw new Exception(" It should contain the current staff , It is :" + val);
                        break;
                    case WorkAttr.FID:
                        if (val != "0")
                            throw new Exception(" Should  = 0, It is :" + val);
                        break;
                    case WorkAttr.MyNum:
                        if (val != "1")
                            throw new Exception(" Should  = 1, It is :" + val);
                        break;
                    case WorkAttr.Rec:
                        if (val != "zhanghaicheng")
                            throw new Exception(" Should  Rec=zhanghaicheng, It is :" + val);
                        break;
                    default:
                        break;
                }
            }

            // Check the node 2 Data .
            sql = "SELECT * FROM ND2302 WHERE OID=" + workID;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ Initiate the process error , Should not not be found  ND2302  Data .");
            foreach (DataColumn dc in dt.Columns)
            {
                string val = dt.Rows[0][dc.ColumnName].ToString();
                switch (dc.ColumnName)
                {
                    case WorkAttr.CDT:
                        if (val.Contains(DataType.CurrentData) == false)
                            throw new Exception("CDT, Date error .");
                        break;
                    case WorkAttr.RDT:
                        if (val.Contains(DataType.CurrentData) == false)
                            throw new Exception("RDT, Date error .");
                        break;
                    case WorkAttr.Emps:
                        if (val.Contains("zhoupeng") == false)
                            throw new Exception(" It should contain the current staff , It is :" + val);
                        break;
                    case WorkAttr.Rec:
                        if (val != "zhoupeng")
                            throw new Exception(" Should  Rec= zhoupeng, It is :" + val);
                        break;
                    case WorkAttr.FID:
                        if (val != "0")
                            throw new Exception(" Should  = 0, It is :" + val);
                        break;
                    case WorkAttr.MyNum:
                        if (val != "1")
                            throw new Exception(" Should  = 1, It is :" + val);
                        break;
                   
                    default:
                        break;
                }
            }

            // Check the node 3 Data .
            sql = "SELECT * FROM ND2399 WHERE OID=" + workID;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ Initiate the process error , Should not not be found  ND2399  Data .");
            foreach (DataColumn dc in dt.Columns)
            {
                string val = dt.Rows[0][dc.ColumnName].ToString();
                switch (dc.ColumnName)
                {
                    case WorkAttr.CDT:
                        if (val.Contains(DataType.CurrentData) == false)
                            throw new Exception("CDT, Date error .");
                        break;
                    case WorkAttr.RDT:
                        if (val.Contains(DataType.CurrentData) == false)
                            throw new Exception("RDT, Date error .");
                        break;
                    case WorkAttr.Emps:
                        if (val.Contains(WebUser.No) == false)
                            throw new Exception(" It should contain the current staff , It is :" + val);
                        break;
                    case WorkAttr.FID:
                        if (val != "0")
                            throw new Exception(" Should  = 0, It is :" + val);
                        break;
                    case WorkAttr.MyNum:
                        if (val != "1")
                            throw new Exception(" Should  = 1, It is :" + val);
                        break;
                    case WorkAttr.Rec:
                        if (val != "zhanghaicheng")
                            throw new Exception(" Should  Rec= zhanghaicheng, It is :" + val);
                        break;
                    default:
                        break;
                }
            }

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
                        if (val != "2399")
                            throw new Exception("@ It should be  2399,  It is :" + val);
                        break;
                    case GERptAttr.FlowStarter:
                        if (val != "zhanghaicheng")
                            throw new Exception("@ It should be  zhanghaicheng,  It is :" + val);
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
