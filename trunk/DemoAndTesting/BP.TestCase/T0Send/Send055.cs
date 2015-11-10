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
    public class Send055 : TestBase
    {
        /// <summary>
        ///  Customer Involvement Process 
        /// </summary>
        public Send055()
        {
            this.Title = " Customer Involvement Process ";
            this.DescIt = " Process :055 Students leave the process , Whether the data is sent to perform in line with expectations after .";
            this.EditState = CT.EditState.Passed;
        }

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
        ///  Test Case Description :
        /// 1,  This process is targeted at the most simple division processes , zhanghaicheng Launch ,zhoushengyu,zhangyifan, Two people deal with child thread ,
        ///    zhanghaicheng  Accept the child thread summary data .
        /// 2,  Test method body is divided into three parts .  Launch , Sub-thread processing , Confluence execution , Respectively : Step1(), Step2_1(), Step2_2(),Step3()  Method .
        /// 3, For sending test , Does not relate to other features .
        /// </summary>
        public override void Do()
        {
            // Initialize variables .
            fk_flow = "055";
            userNo = "zhanghaicheng";

            fl = new Flow(fk_flow);

            // Implementation of 1 Step inspection , Create work and send .
            this.Step1();

            // Implementation of 2_1 Step inspection ,zhoushengyu Send results .
            this.Step2_1();

            // Implementation of 2_2 Step inspection ,zhangyifan Send results .
            this.Step2_2();

            // Final inspection .
            this.Step3();
        }
        /// <summary>
        ///  Creating Process , Send diversion point 1步.
        /// </summary>
        public void Step1()
        {
            // 让zhanghaicheng  Log in .
            BP.WF.Dev2Interface.Port_Login(userNo);

            // Create a blank ,  Initiating the start node .
            workid = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, WebUser.No, null, 0, null, 0, null);

            #region  Inspection   After the data creation process is complete  ?
            // " Creating this blank check whether there is data integrity ?;
            sql = "SELECT * FROM " + fl.PTable + " WHERE OID=" + workid;
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
                throw new Exception("@ Initiate the process error , Should not report data can not be found .");

            //  Check whether there is data node table form ?;
            sql = "SELECT * FROM ND501 WHERE OID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
                throw new Exception("@ Should not start node can not be found in the form of data tables ,");

            if (dt.Rows[0]["Rec"].ToString() != WebUser.No)
                throw new Exception("@ Record should be the current staff .");

            //  Creating this blank check whether there is data integrity ?;
            sql = "SELECT * FROM WF_EmpWorks WHERE WorkID=" + workid + " AND FK_Emp='" + WebUser.No + "'";
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count !=0 )
                throw new Exception("@ Upcoming find the current staff is wrong .");
            #endregion  After checking the data to initiate the process is complete ?

            // Start node : Performing transmission , Send and retrieve objects .  Cheng Xiangzai main thread sends .
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid);

            #region 第1步:  Inspection 【 Start node 】 Send returned object information is complete ?
            // Get in objects obtained from sending to the next worker . zhangyifan( Yifan Zhang ),zhoushengyu( Week liter rain ).
            if (objs.VarAcceptersID != "zhangyifan,zhoushengyu,")
                throw new Exception("@ Next recipient is incorrect ,  zhangyifan,zhoushengyu,  It is :" + objs.VarAcceptersID);

            if (objs.VarToNodeID != 502)
                throw new Exception("@ Should be  502 Node .  It is :" + objs.VarToNodeID);

            if (objs.VarWorkID != workid)
                throw new Exception("@ Main thread workid Should not change :" + objs.VarWorkID);

            if (objs.VarCurrNodeID != 501)
                throw new Exception("@ Can not change the number of the current node :" + objs.VarCurrNodeID);

            if (objs.VarTreadWorkIDs == null)
                throw new Exception("@ Did not get to the two sub-thread ID.");

            if (objs.VarTreadWorkIDs.Contains(",") == false)
                throw new Exception("@ Did not get to the two sub-thread WorkID:" + objs.VarTreadWorkIDs);
            #endregion   Inspection 【 Start node 】 Send returned object information is complete ?

            #region 第2步:  Check the process engine control system tables are in line with expectations .
            gwf = new GenerWorkFlow(workid);
            if (gwf.FK_Node != 501)
                throw new Exception("@ When the main thread sends Cheng Xiangzai , Main thread FK_Node Should not change , Right now :" + gwf.FK_Node);

            if (gwf.WFState != WFState.Runing)
                throw new Exception("@ When the main thread sends Cheng Xiangzai , Main thread  WFState  Should  WFState.Runing :" + gwf.WFState.ToString());

            if (gwf.Starter != WebUser.No)
                throw new Exception("@ Should be initiated by staff , It is :" + gwf.Starter );

            // Find job listings sponsor .
            gwl = new GenerWorkerList(workid, 501, WebUser.No);
            if (gwl.IsPass == true)
                throw new Exception("@ River on pass State should be by , This person has no work of his to-do .");

            // Identify staff on child thread .
            GenerWorkFlows gwfs = new GenerWorkFlows();
            gwfs.Retrieve(GenerWorkerListAttr.FID, workid);
            if (gwfs.Count != 2)
                throw new Exception("@ There are two processes should be registered , It is :"+gwfs.Count+"个.");

            // Check their registration data is complete .
            foreach (GenerWorkFlow item in gwfs)
            {
                if (item.Starter != WebUser.No)
                    throw new Exception("@ The current staff should sponsor , It is :" + item.Starter);

                if (item.FK_Node != 502)
                    throw new Exception("@ The current node should be  502 , It is :" + item.FK_Node);

                if (item.WFState != WFState.Runing)
                    throw new Exception("@ Current  WFState  It should be  Runing , It is :" + item.WFState.ToString());
            }

            // Identify sub-thread work worklist handlers .
            GenerWorkerLists gwls = new GenerWorkerLists();
            gwls.Retrieve(GenerWorkerListAttr.FID, workid);
            if (gwls.Count != 2)
                throw new Exception("@ Should check out the thread on the two sub-to-do , Now only (" + gwls.Count + ")个.");

            // Check the integrity of the child thread to-do .
            foreach (GenerWorkerList item in gwls)
            {
                if (item.IsPass)
                    throw new Exception("@ Should not have passed , Because they do not deal with .");

                if (item.IsEnable == false)
                    throw new Exception("@ Should be :IsEnable ");

                if (item.Sender.Contains(WebUser.No) == false)
                    throw new Exception("@ Sender , Should be the current staff . It is :" + item.Sender);

                if (item.FK_Flow != "055")
                    throw new Exception("@ Should be  055  It is :" + item.FK_Flow);

                if (item.FK_Node != 502)
                    throw new Exception("@ Should be  502  It is :" + item.FK_Node);
            }

            // Take the main thread of the work to be done .
            sql = "SELECT * FROM WF_EmpWorks WHERE WorkID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 0)
                throw new Exception("@ Should not appear to be run in the main thread  WF_EmpWorks  View . " + sql);

            // Upcoming take the child thread work to be done .
            sql = "SELECT * FROM WF_EmpWorks WHERE FID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 2)
                throw new Exception("@ Should be taken out of the two sub-threads  WF_EmpWorks  View . " + sql);

            #endregion end  Check the process engine control system tables are in line with expectations .

            #region 第3步:  Inspection 【 Start node 】 Sending node form - No data integrity ?
            // Check whether there is data node table form ?
            sql = "SELECT * FROM ND501 WHERE OID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ Should find the starting node form data , But no .");

            if (dt.Rows[0]["Rec"].ToString() != WebUser.No)
                throw new Exception("@ The main thread is not written to the starting node table Rec Field , It is :" + dt.Rows[0]["Rec"].ToString() + " It should be :" + WebUser.No);

            // Check whether there is data node table form , And the data is correct ?
            sql = "SELECT * FROM ND502 WHERE FID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 2)
                throw new Exception("@ Should find two data nodes on the first child thread .");
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Rec"].ToString() == "zhangyifan")
                {
                    continue;
                }
                if (dr["Rec"].ToString() == "zhoushengyu")
                {
                    continue;
                }
                throw new Exception("@ Child thread form data is not correctly written Rec Field .");
            }


            sql = "SELECT * FROM  ND5Rpt WHERE OID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows[0][GERptAttr.FlowEnder].ToString() != "zhanghaicheng")
                throw new Exception("@ Should be  zhanghaicheng 是 FlowEnder .");

            if (dt.Rows[0][GERptAttr.FlowStarter].ToString() != "zhanghaicheng")
                throw new Exception("@ Should be  zhanghaicheng 是 FlowStarter .");

            if (dt.Rows[0][GERptAttr.FlowEndNode].ToString() != "502")
                throw new Exception("@ Should be  502 是 FlowEndNode .");

            if (int.Parse(dt.Rows[0][GERptAttr.WFState].ToString()) != (int)WFState.Runing)
                throw new Exception("@ Should be  WFState.Runing  Is the current state of .");

            if (int.Parse(dt.Rows[0][GERptAttr.FID].ToString()) != 0)
                throw new Exception("@ Should be  FID =0 ");

            if (dt.Rows[0]["FK_NY"].ToString() != DataType.CurrentYearMonth)
                throw new Exception("@ FK_NY  Field filled error . ");
            #endregion   Inspection 【 Start node 】 No sending data integrity ?
        }
        /// <summary>
        ///  Let the child thread of a person  zhoushengyu  Log in ,  Then initiate execution down .
        ///  Check the business logic is correct ?
        /// </summary>
        public void Step2_1()
        {
            // Child thread to accept staff ,  Are  zhoushengyu,zhangyifan

            //  Let the child thread of a person  zhoushengyu  Log in ,  Then initiate execution down ,
            BP.WF.Dev2Interface.Port_Login("zhoushengyu");

            // Get this person  055  The work to be done .
            dt = BP.WF.Dev2Interface.DB_GenerEmpWorksOfDataTable(WebUser.No, WFState.Runing, "055");
            if (dt.Rows.Count == 0)
                throw new Exception("@ It should not get less than a to-do data .");

            // Gets the child thread workID.
            int threahWorkID = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (int.Parse(dr["FID"].ToString()) == workid)
                {
                    threahWorkID = int.Parse(dr["WorkID"].ToString());
                    break;
                }
            }
            if (threahWorkID == 0)
                throw new Exception("@ It should not be do not find it .");

            //  Carried out   Send to a confluence child thread .
            Hashtable ht = new Hashtable();
            ht.Add("FuWuQi",90);
            ht.Add("ShuMaXiangJi", 20);// The data is put in there , It is saved to the main table child thread , To check whether the data is aggregated to the confluence node .
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, threahWorkID, ht);

            #region 第1步:  Variable inspection after sending .
            if (objs.VarWorkID != threahWorkID)
                throw new Exception("@ It should be  VarWorkID=" + threahWorkID + " , It is :" + objs.VarWorkID);

            if (objs.VarCurrNodeID != 502)
                throw new Exception("@ It should be  VarCurrNodeID=502 是, It is :" + objs.VarCurrNodeID);

            if (objs.VarToNodeID != 599)
                throw new Exception("@ It should be  VarToNodeID= 599 是, It is :" + objs.VarToNodeID);

            if (objs.VarAcceptersID != "zhanghaicheng")
                throw new Exception("@ It should be  VarAcceptersID= zhanghaicheng 是, It is :" + objs.VarAcceptersID);
            #endregion 第1步:  Variable inspection after sending .

            #region 第2步:  Check the engine control system table .
            // Check Mainstream Data .
            gwf = new GenerWorkFlow(workid);
            if (gwf.WFState != WFState.Runing)
                throw new Exception("@ It should be  Runing,  It is :" + gwf.WFState);

            if (gwf.FID != 0)
                throw new Exception("@ It should be  0,  It is :" + gwf.FID);

            if (gwf.FK_Node != 599)
                throw new Exception("@ It should be  599,  It is :" + gwf.FK_Node);

            if (gwf.Starter != "zhanghaicheng")
                throw new Exception("@ It should be  zhanghaicheng,  It is :" + gwf.Starter);

            //  Mainstream staff table if there is a change ?
            GenerWorkerLists gwls = new GenerWorkerLists();
            gwls.Retrieve(GenerWorkerListAttr.WorkID, workid);
            foreach (GenerWorkerList item in gwls)
            {
                if (item.FK_Emp != "zhanghaicheng")
                    throw new Exception("@ It should be  zhanghaicheng,  It is :" + item.FK_Emp);

                // If this is the start node .
                if (item.FK_Node == 501)
                {
                    if (item.IsPass == false)
                        throw new Exception("@pass Status wrong , Should have been passed, .");
                }

                // If the end node .
                if (item.FK_Node == 599)
                {
                    // Check the child thread completion rate . 
                    Node nd = new Node(599);
                    if (nd.PassRate > 50)
                    {
                        if (item.IsPassInt != 3)
                            throw new Exception("@ Since the completion rate of more than  50,  So one by the , Staff can not see the main thread , It is :"+item.IsPassInt);
                    }
                    else
                    {
                        if (item.IsPassInt != 0)
                            throw new Exception("@ Because less than 50, So long as there is an adoption , Main thread zhanghaicheng  Staff should be able to see the Upcoming , But to no avail . ");
                    }
                }
            }

            // Staff check a list of sub-thread table .
            gwls = new GenerWorkerLists();
            gwls.Retrieve(GenerWorkerListAttr.FID, workid);
            if (gwls.Count != 2)
                throw new Exception("@ Staff are not expected on the two sub-thread list data .");
            foreach (GenerWorkerList item in gwls)
            {
                if (item.FK_Emp == "zhoushengyu")
                {
                    if (item.IsPass == false)
                        throw new Exception("@ This person should be handled through the , Now do not pass .");
                }

                if (item.FK_Emp == "zhangyifan")
                {
                    if (item.IsPass == true)
                        throw new Exception("@ This person should be run , The results do not meet expectations .");
                }
            }
            #endregion 第2步:  Check the engine control system table .

            #region 第3步:  Inspection   Nodes form table data .
            sql = "SELECT * FROM ND501 WHERE OID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows[0]["Rec"].ToString() != "zhanghaicheng")
                throw new Exception("@ Start node Rec  Fields write error .");

            // Check whether there is data node table form , And the data is correct ?
            sql = "SELECT * FROM ND502 WHERE FID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 2)
                throw new Exception("@ Should find two data nodes on the first child thread .");
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Rec"].ToString() == "zhangyifan")
                    continue;
                if (dr["Rec"].ToString() == "zhoushengyu")
                    continue;
                throw new Exception("@ Child thread form data is not correctly written Rec Field .");
            }

            // Check the parameters are stored in the main table of the child thread ?
            sql = "SELECT * FROM ND502 WHERE OID=" + threahWorkID;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ Did not find the desired data sub-thread .");

            if (dt.Rows[0]["FuWuQi"].ToString()!="90")
                throw new Exception(" Not saved to the specified location .");

              if (dt.Rows[0]["ShuMaXiangJi"].ToString()!="20")
                throw new Exception(" Not saved to the specified location .");

              

            //  Check whether the data is aggregated list copy Correct ?
              sql = "SELECT * FROM ND599Dtl1 WHERE OID=" + threahWorkID;
              dt = DBAccess.RunSQLReturnTable(sql);
              if (dt.Rows.Count != 1)
                  throw new Exception("@ Sub-thread data is not copy To the list in the summary .");
              dt = DBAccess.RunSQLReturnTable(sql);
              if (dt.Rows.Count != 1)
                  throw new Exception("@ Did not find the desired data sub-thread .");

              if (dt.Rows[0]["FuWuQi"].ToString() != "90")
                  throw new Exception(" Not saved to the specified location .");

              if (dt.Rows[0]["ShuMaXiangJi"].ToString() != "20")
                  throw new Exception(" Not saved to the specified location .");


            // Check the report data is correct ?
            sql = "SELECT * FROM  ND5Rpt WHERE OID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows[0][GERptAttr.FlowEnder].ToString() != "zhanghaicheng")
                throw new Exception("@ Should be  zhanghaicheng 是 FlowEnder .");

            if (dt.Rows[0][GERptAttr.FlowStarter].ToString() != "zhanghaicheng")
                throw new Exception("@ Should be  zhanghaicheng 是 FlowStarter .");

            if (dt.Rows[0][GERptAttr.FlowEndNode].ToString() != "502")
                throw new Exception("@ Should be  502 是 FlowEndNode .");

            if (int.Parse(dt.Rows[0][GERptAttr.WFState].ToString()) != (int)WFState.Runing)
                throw new Exception("@ Should be  WFState.Runing 是 WFState .");

            if (int.Parse(dt.Rows[0][GERptAttr.FID].ToString()) != 0)
                throw new Exception("@ Should be  FID =0 ");

            if (dt.Rows[0]["FK_NY"].ToString() != DataType.CurrentYearMonth)
                throw new Exception("@ FK_NY  Field filled error . ");
            #endregion 第3步:  Inspection   Nodes form table data .
        }
        /// <summary>
        ///  Each sub-thread sends down 
        /// </summary>
        public void Step2_2()
        {
            //  Let the child thread of a person  zhoushengyu  Log in ,  Then initiate execution down ,
            BP.WF.Dev2Interface.Port_Login("zhangyifan");

            // Get this person  055  The work to be done .
            dt = BP.WF.Dev2Interface.DB_GenerEmpWorksOfDataTable(WebUser.No, WFState.Runing, "055");
            if (dt.Rows.Count == 0)
                throw new Exception("@ It should not get less than a to-do data .");

            // Gets the child thread workID.
            int threahWorkID = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (int.Parse(dr["FID"].ToString()) == workid)
                {
                    threahWorkID = int.Parse(dr["WorkID"].ToString());
                    break;
                }
            }
            if (threahWorkID == 0)
                throw new Exception("@ It should not be do not find it .");

            //  Carried out   Send to a confluence child thread .
            Hashtable ht = new Hashtable();
            ht.Add("FuWuQi", 100);
            ht.Add("ShuMaXiangJi", 30);// The data is put in there , It is saved to the main table child thread , To check whether the data is aggregated to the confluence node .
    
            //  Carried out   Send to a confluence child thread .
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, threahWorkID, ht);

            #region 第1步:  Variable inspection after sending .
            if (objs.VarWorkID != threahWorkID)
                throw new Exception("@ It should be  VarWorkID=" + threahWorkID + " , It is :" + objs.VarWorkID);

            if (objs.VarCurrNodeID != 502)
                throw new Exception("@ It should be  VarCurrNodeID=502 是, It is :" + objs.VarCurrNodeID);

            if (objs.VarToNodeID != 599)
                throw new Exception("@ It should be  VarToNodeID= 599 是, It is :" + objs.VarToNodeID);

            if (objs.VarAcceptersID != "zhanghaicheng")
                throw new Exception("@ It should be  VarAcceptersID= zhanghaicheng 是, It is :" + objs.VarAcceptersID);
            #endregion 第1步:  Variable inspection after sending .

            #region 第2步:  Check the engine control system table .
            // Check Mainstream Data .
            gwf = new GenerWorkFlow(workid);
            if (gwf.WFState != WFState.Runing)
                throw new Exception("@ It should be  Runing,  It is :" + gwf.WFState);

            if (gwf.FID != 0)
                throw new Exception("@ It should be  0,  It is :" + gwf.FID);

            if (gwf.FK_Node != 599)
                throw new Exception("@ It should be  599,  It is :" + gwf.FK_Node);

            if (gwf.Starter != "zhanghaicheng")
                throw new Exception("@ It should be  zhanghaicheng,  It is :" + gwf.Starter);

            //  Mainstream staff table if there is a change ?
            GenerWorkerLists gwls = new GenerWorkerLists();
            gwls.Retrieve(GenerWorkerListAttr.WorkID, workid);
            foreach (GenerWorkerList item in gwls)
            {
                if (item.FK_Emp != "zhanghaicheng")
                    throw new Exception("@ It should be  zhanghaicheng,  It is :" + item.FK_Emp);

                // If this is the start node .
                if (item.FK_Node == 501)
                {
                    if (item.IsPass == false)
                        throw new Exception("@pass Status wrong , Should have been passed, .");
                }

                // If the end node .
                if (item.FK_Node == 599)
                {
                    // Check the child thread completion rate .
                    Node nd = new Node(599);
                    if (nd.PassRate > 50)
                    {
                        if (item.IsPassInt != 0)
                            throw new Exception("@ Since the completion rate of more than  50,  Now the two have passed , So this confluence point should also be adopted by state .");
                    }
                    else
                    {
                        if (item.IsPassInt != 0)
                            throw new Exception("@ Because less than 50, So long as there is an adoption , Main thread zhanghaicheng  Staff should be able to see the Upcoming , But to no avail . ");
                    }
                }
            }

            // Staff check a list of sub-thread table .
            gwls = new GenerWorkerLists();
            gwls.Retrieve(GenerWorkerListAttr.FID, workid);
            if (gwls.Count != 2)
                throw new Exception("@ Staff are not expected on the two sub-thread list data .");
            foreach (GenerWorkerList item in gwls)
            {
                if (item.FK_Emp == "zhoushengyu")
                {
                    if (item.IsPass == false)
                        throw new Exception("@ This person should be handled through the , Now do not pass .");
                }

                if (item.FK_Emp == "zhangyifan")
                {
                    if (item.IsPass == false)
                        throw new Exception("@ This person should be handled through the , Now do not pass .");
                }
            }
            #endregion 第2步:  Check the engine control system table .

            #region 第3步:  Inspection   Nodes form table data .
            sql = "SELECT * FROM ND501 WHERE OID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows[0]["Rec"].ToString() != "zhanghaicheng")
                throw new Exception("@ Start node Rec  Fields write error .");

            // Check whether there is data node table form , And the data is correct ?
            sql = "SELECT * FROM ND502 WHERE FID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 2)
                throw new Exception("@ Should find two data nodes on the first child thread .");
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Rec"].ToString() == "zhangyifan")
                {
                    continue;
                }
                if (dr["Rec"].ToString() == "zhoushengyu")
                {
                    continue;
                }
                throw new Exception("@ Child thread form data is not correctly written Rec Field .");
            }

            // Check the parameters are stored in the main table of the child thread ?
            sql = "SELECT * FROM ND502 WHERE OID=" + threahWorkID;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ Did not find the desired data sub-thread .");

            if (dt.Rows[0]["FuWuQi"].ToString() != "100")
                throw new Exception(" Not saved to the specified location .");

            if (dt.Rows[0]["ShuMaXiangJi"].ToString() != "30")
                throw new Exception(" Not saved to the specified location .");

             

            //  Check whether the data is aggregated list copy Correct ?
            sql = "SELECT * FROM ND599Dtl1 WHERE OID=" + threahWorkID;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ Sub-thread data is not copy To the list in the summary .");
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ Did not find the desired data sub-thread .");

            if (dt.Rows[0]["FuWuQi"].ToString() != "100")
                throw new Exception(" Not saved to the specified location .");

            if (dt.Rows[0]["ShuMaXiangJi"].ToString() != "30")
                throw new Exception(" Not saved to the specified location .");
             


            // Check the report data is correct ?
            sql = "SELECT * FROM  ND5Rpt WHERE OID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows[0][GERptAttr.FlowEnder].ToString() != "zhanghaicheng")
                throw new Exception("@ Should be  zhanghaicheng 是 FlowEnder .");

            if (dt.Rows[0][GERptAttr.FlowStarter].ToString() != "zhanghaicheng")
                throw new Exception("@ Should be  zhanghaicheng 是 FlowStarter .");

            if (dt.Rows[0][GERptAttr.FlowEndNode].ToString() != "502")
                throw new Exception("@ Should be  502 是 FlowEndNode .");

            if (int.Parse(dt.Rows[0][GERptAttr.WFState].ToString()) != (int)WFState.Runing)
                throw new Exception("@ Should be  WFState.Runing 是 WFState .");

            if (int.Parse(dt.Rows[0][GERptAttr.FID].ToString()) != 0)
                throw new Exception("@ Should be  FID =0 ");

            if (dt.Rows[0]["FK_NY"].ToString() != DataType.CurrentYearMonth)
                throw new Exception("@ FK_NY  Field filled error . ");
            #endregion 第3步:  Inspection   Nodes form table data .
        }
        /// <summary>
        ///  Carried out zhanghaicheng的  Send .
        /// 1, Object Inspector sent .
        /// 2, Check the process engine control table .
        /// 3, Check the node table .
        /// </summary>
        public void Step3()
        {
            //  Let sponsor on the main thread Login .
            BP.WF.Dev2Interface.Port_Login("zhanghaicheng");

            //  Transmission is performed to the last node 
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid);

            #region 第1步:  Variable inspection after sending .
            if (objs.VarWorkID != workid)
                throw new Exception("@ It should be  VarWorkID=" + workid + " , It is :" + objs.VarWorkID);

            if (objs.VarCurrNodeID != 599)
                throw new Exception("@ It should be  VarCurrNodeID=599 是, It is :" + objs.VarCurrNodeID);

            if (objs.VarToNodeID != 0)
                throw new Exception("@ It should be  VarToNodeID= 0 是, It is :" + objs.VarToNodeID);

            if (objs.VarAcceptersID != null)
                throw new Exception("@ It should be  VarAcceptersID= null 是, It is :" + objs.VarAcceptersID);
            #endregion 第1步:  Variable inspection after sending .

            #region 第2步:  Check the engine control system table .
            // Check the main thread .
            sql = "SELECT * FROM WF_GenerWorkFlow WHERE WorkID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 0)
                throw new Exception("@ Process ended , Data Engine table is not deleted .");

            sql = "SELECT * FROM WF_GenerWorkerlist WHERE WorkID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 0)
                throw new Exception("@ Process ended , Data Engine table is not deleted .");

            // Check the child thread .
            sql = "SELECT * FROM WF_GenerWorkFlow WHERE FID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 0)
                throw new Exception("@ Process ended , Data sub-thread engine tables are not deleted .");

            sql = "SELECT * FROM WF_GenerWorkerlist WHERE FID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 0)
                throw new Exception("@ Process ended , Data Engine table is not deleted .");
            #endregion 第2步:  Check the engine control system table .

            #region 第3步:  Inspection   Nodes form table data .
            // Check the list summary data on the confluence .
            sql = "SELECT * FROM  ND599Dtl1 WHERE RefPK=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 2)
                throw new Exception("@ Summary list of missing data ");

            int sum = 0;
            foreach (DataRow dr in dt.Rows)
            {
                sum += int.Parse(dr["FuWuQi"].ToString());
            }
            if (sum!=190)
                throw new Exception("@ Summary list of data errors ");

            // Check the report data is correct ?
            sql = "SELECT * FROM  ND5Rpt WHERE OID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows[0][GERptAttr.FlowEnder].ToString() != "zhanghaicheng")
                throw new Exception("@ Should be  zhanghaicheng 是 FlowEnder .");

               if ( string.IsNullOrEmpty(dt.Rows[0][GERptAttr.Title].ToString()) )
                            throw new Exception("@ After the process has completed the title is lost ");

            if (dt.Rows[0][GERptAttr.FlowStarter].ToString() != "zhanghaicheng")
                throw new Exception("@ Should be  zhanghaicheng 是 FlowStarter .");

            if (dt.Rows[0][GERptAttr.FlowEndNode].ToString() != "599")
                throw new Exception("@ Should be  599 是 FlowEndNode .");

            if (dt.Rows[0][GERptAttr.FlowEnder].ToString() != "zhanghaicheng")
                throw new Exception("@ Should be  zhanghaicheng 是 FlowEnder .");

            if (int.Parse(dt.Rows[0][GERptAttr.WFState].ToString()) != (int)WFState.Complete)
                throw new Exception("@ Should be  WFState.Complete  Is the current state of  .");

            if (int.Parse(dt.Rows[0][GERptAttr.FID].ToString()) != 0)
                throw new Exception("@ Should be  FID =0 ");

            if (dt.Rows[0]["FK_NY"].ToString() != DataType.CurrentYearMonth)
                throw new Exception("@ FK_NY  Field filled error . ");

            //  Check that the child thread node table data exists ?
            sql = "SELECT * FROM ND502 WHERE FID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 2)
                throw new Exception("@ Should find two data nodes on the first child thread .");
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Rec"].ToString() == "zhangyifan")
                    continue;
                if (dr["Rec"].ToString() == "zhoushengyu")
                    continue;
                throw new Exception("@ Child thread form data is not correctly written Rec Field .");
            }
            #endregion 第3步:  Inspection   Nodes form table data .
           
        }
    }
}
