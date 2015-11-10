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
    public class Send009 : TestBase
    {
        /// <summary>
        ///  Different forms sub-confluent send 
        /// </summary>
        public Send009()
        {
            this.Title = " Different forms sub-confluent send ";
            this.DescIt = " Process :009 Sector in the planning process ( Different forms sub-confluent ), Whether the data is sent to perform in line with expectations after .";
            this.EditState = CT.EditState.UnOK;
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
        ///  Explanation :
        /// 1, zhoupeng  Start up .
        /// 2,  Sent to three people  zhoupeng, Zhang Haicheng  qifenglin, Qifeng Lin ,guoxiangbin, Guoxiang Bin ,.
        /// 3,zhoupeng  End and confluence .
        /// </summary>
        public override void Do()
        {
            // Initialize variables .
            fk_flow = "009";
            userNo = "zhoupeng";
            fl = new Flow(fk_flow);

            // Implementation of 1 Step inspection , Create work and send .
            this.Step1();

            // Implementation of 2_1 Step inspection ,zhanghaicheng Send results .
            this.Step2_1();

            // Implementation of 2_2 Step inspection ,qifenglin  Send results .
            this.Step2_2();

            // Implementation of 2_3 Step inspection ,guoxiangbin  Send results .
            this.Step2_3();

            // Final inspection .
            this.Step3();
        }
        /// <summary>
        ///  Creating Process , Send diversion point 1步.
        /// </summary>
        public void Step1()
        {
            // 让zhoupeng  Log in .
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
            sql = "SELECT * FROM ND901 WHERE OID=" + workid;
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

            // Organization Parameters .
            Hashtable ht = new Hashtable();
            ht.Add("KeFuBu",1); 
            ht.Add("ShiChangBu", 1);
            ht.Add("YanFaBu", 1);

            // Start node : Performing transmission , Send and retrieve objects .  Cheng Xiangzai main thread sends .
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid,ht);

            #region 第1步:  Inspection 【 Start node 】 Send returned object information is complete ?
            // Get in objects obtained from sending to the next worker : zhanghaicheng,qifenglin,guoxiangbin .
            if (objs.VarAcceptersID != "zhanghaicheng,qifenglin,guoxiangbin,")
                throw new Exception("@ Next recipient is incorrect ,  zhanghaicheng,qifenglin,guoxiangbin, . It is :" + objs.VarAcceptersID);

            if (objs.VarToNodeIDs != "902,903,904,")
                throw new Exception("@ Should be  902,903,904,   It is :" + objs.VarToNodeIDs);

            if (objs.VarWorkID != workid)
                throw new Exception("@ Main thread workid Should not change :" + objs.VarWorkID);

            if (objs.VarCurrNodeID != 901)
                throw new Exception("@ Can not change the number of the current node :" + objs.VarCurrNodeID);

            if (objs.VarTreadWorkIDs == null)
                throw new Exception("@ Did not get to the three sub-thread ID.");

            if (objs.VarTreadWorkIDs.Contains(",") == false)
                throw new Exception("@ Did not get to the three sub-thread WorkID:" + objs.VarTreadWorkIDs);

            #endregion   Inspection 【 Start node 】 Send returned object information is complete ?

            #region 第2步:  Check the process engine control system tables are in line with expectations .
            gwf = new GenerWorkFlow(workid);
            if (gwf.FK_Node != 901)
                throw new Exception("@ When the main thread sends Cheng Xiangzai , Main thread FK_Node Should not change , Right now :" + gwf.FK_Node);

            if (gwf.WFState != WFState.Runing)
                throw new Exception("@ When the main thread sends Cheng Xiangzai , Main thread  WFState  Should  WFState.Runing :" + gwf.WFState.ToString());

            if (gwf.Starter != WebUser.No)
                throw new Exception("@ Should be initiated by staff , It is :" + gwf.Starter);

            // Find job listings sponsor .
            gwl = new GenerWorkerList(workid, 901, WebUser.No);
            if (gwl.IsPass == true)
                throw new Exception("@ River on pass State should be by , This person has no work of his to-do .");

            // Identify staff on child thread .
            GenerWorkFlows gwfs = new GenerWorkFlows();
            gwfs.Retrieve(GenerWorkerListAttr.FID, workid);
            if (gwfs.Count != 3)
                throw new Exception("@ There are two processes should be registered , It is :"+gwfs.Count+"个.");

            // Check their registration data is complete .
            foreach (GenerWorkFlow item in gwfs)
            {
                if (item.Starter != WebUser.No)
                    throw new Exception("@ The current staff should sponsor , It is :" + item.Starter);

                //Node nd = new Node(item.FK_Node);
                //if (nd.iss

                //if (item.FK_Node == 901)
                //    throw new Exception("@ The current node should be  902 , It is :" + item.FK_Node);

                if (item.WFState != WFState.Runing)
                    throw new Exception("@ Current  WFState  It should be  Runing , It is :" + item.WFState.ToString());
            }

            // Identify sub-thread work worklist handlers .
            GenerWorkerLists gwls = new GenerWorkerLists();
            gwls.Retrieve(GenerWorkerListAttr.FID, workid);
            if (gwls.Count != 3)
                throw new Exception("@ Should check out the child thread  3  A to-do , Now only (" + gwls.Count + ")个.");

            // Check the integrity of the child thread to-do .
            foreach (GenerWorkerList item in gwls)
            {
                if (item.IsPass)
                    throw new Exception("@ Should not have passed , Because they do not deal with .");

                if (item.IsEnable == false)
                    throw new Exception("@ Should be :IsEnable ");

                if (item.Sender.Contains(WebUser.No) == false)
                    throw new Exception("@ Sender , Should be the current staff . It is :" + item.Sender);

                if (item.FK_Flow != "009")
                    throw new Exception("@ Should be  009  It is :" + item.FK_Flow);

                //if (item.FK_Node != 902)
                //    throw new Exception("@ Should be  902  It is :" + item.FK_Node);
            }

            // Take the main thread of the work to be done .
            sql = "SELECT * FROM WF_EmpWorks WHERE WorkID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 0)
                throw new Exception("@ Should not appear to be run in the main thread  WF_EmpWorks  View . " + sql);

            // Upcoming take the child thread work to be done .
            sql = "SELECT * FROM WF_EmpWorks WHERE FID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 3)
                throw new Exception("@ Should be taken out of the two sub-threads  WF_EmpWorks  View . " + sql);

            #endregion end  Check the process engine control system tables are in line with expectations .

            #region 第3步:  Inspection 【 Start node 】 Sending node form - No data integrity ?
            // Check whether there is data node table form ?
            sql = "SELECT * FROM ND901 WHERE OID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 1)
                throw new Exception("@ Should find the starting node form data , But no .");

            if (dt.Rows[0]["Rec"].ToString() != WebUser.No)
                throw new Exception("@ The main thread is not written to the starting node table Rec Field , It is :" + dt.Rows[0]["Rec"].ToString() + " It should be :" + WebUser.No);

            // Identify sub-thread work worklist handlers .
            gwls = new GenerWorkerLists();
            gwls.Retrieve(GenerWorkerListAttr.FID, workid);

            // Check the child thread node data table is correct .
            foreach (GenerWorkerList item in gwls)
            {
                sql = "SELECT * FROM ND" + item.FK_Node + " WHERE OID=" + item.WorkID;
                dt = DBAccess.RunSQLReturnTable(sql);
                if (dt.Rows.Count != 1)
                    throw new Exception("@ Should not be found within a sub-thread node data .");

                foreach (DataColumn dc in dt.Columns)
                {
                    string val = dt.Rows[0][dc.ColumnName].ToString();
                    switch (dc.ColumnName)
                    {
                        case WorkAttr.FID:
                            if (val != workid.ToString())
                                throw new Exception("@ Should not be not equal workid.");
                            break;
                        case WorkAttr.Rec:
                            if (val != item.FK_Emp)
                                throw new Exception("@ Should not be not equal :" + item.FK_Emp);
                            break;
                        case WorkAttr.MyNum:
                            if (string.IsNullOrEmpty(val))
                                throw new Exception("@ Should not empty :" + dc.ColumnName);
                            break;
                        case WorkAttr.RDT:
                            if (string.IsNullOrEmpty(val))
                                throw new Exception("@ RDT  Should not empty :" + dc.ColumnName);
                            break;
                        case WorkAttr.CDT:
                            if (string.IsNullOrEmpty(val))
                                throw new Exception("@ CDT  Should not empty :" + dc.ColumnName);
                            break;
                        case WorkAttr.Emps:
                            if (string.IsNullOrEmpty(val) || val.Contains(item.FK_Emp) == false)
                                throw new Exception("@ Emps  Should not empty , Or does not contain promoters .");
                            break;
                        default:
                            break;
                    } // End column col Judge .
                }
            }

            //  Check the report data is complete .
            sql = "SELECT * FROM ND9Rpt WHERE OID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            foreach (DataColumn dc in dt.Columns)
            {
                string val = dt.Rows[0][dc.ColumnName].ToString();
                switch (dc.ColumnName)
                {
                    case GERptAttr.FID:
                        if (int.Parse(val) != 0)
                            throw new Exception("@ Should be  FID =0 ");
                        break;
                    case GERptAttr.FK_Dept:
                        if (val!=WebUser.FK_Dept)
                            throw new Exception("@ FK_Dept  Field filled error , It should be :"+WebUser.FK_Dept+", It is :"+val);
                        break;
                    case GERptAttr.FK_NY:
                        if (val != DataType.CurrentYearMonth)
                            throw new Exception("@ FK_NY  Field filled error . ");
                        break;
                    case GERptAttr.FlowDaySpan:
                        if (val != "0")
                            throw new Exception("@ FlowDaySpan  It should be  0 . ");
                        break;
                    case GERptAttr.FlowEmps:
                        if (BP.WF.Glo.UserInfoShowModel != UserInfoShowModel.UserNameOnly)
                        {
                            if (val.Contains(WebUser.No) == false)
                                throw new Exception("@  Should contain  zhoupeng ,  It is : " + val);
                        }
                        break;
                    case GERptAttr.FlowEnder:
                        if ( val != "zhoupeng")
                            throw new Exception("@ Should be  zhoupeng 是 FlowEnder .");
                        break;
                    case GERptAttr.FlowEnderRDT:
                        break;
                    case GERptAttr.FlowEndNode:
                        if ( val != "901")
                            throw new Exception("@ Should be  901 是 FlowEndNode, It is :"+val);
                        break;
                    case GERptAttr.FlowStarter:
                        if ( val != "zhoupeng")
                            throw new Exception("@ Should be  zhoupeng 是 FlowStarter .");
                        break;
                    case GERptAttr.MyNum:
                        if (val != "1")
                            throw new Exception("@ MyNum  It should be 1  . ");
                        break;
                    case GERptAttr.PFlowNo:
                        if (val != "")
                            throw new Exception("@ PFlowNo  It should be  ''  It is :"+val);
                        break;
                    case GERptAttr.PWorkID:
                        if (val != "0")
                            throw new Exception("@ PWorkID  It should be  '0'  It is :" + val);
                        break;
                    case GERptAttr.Title:
                        if (string.IsNullOrEmpty(val))
                            throw new Exception("@ Title  Should not be empty  " + val);
                        break;
                    case GERptAttr.WFState:
                        if (int.Parse(val) != (int)WFState.Runing)
                            throw new Exception("@ Should be  WFState.Runing  Is the current state of .");
                        break;
                    default:
                        break;
                }
            }
            #endregion   Inspection 【 Start node 】 No sending data integrity ?
        }
        /// <summary>
        ///  Let the child thread of a person  zhoushengyu  Log in ,  Then initiate execution down .
        ///  Check the business logic is correct ?
        /// </summary>
        public void Step2_1()
        {
            // Child thread to accept staff ,  Are  zhanghaicheng,qifenglin,guoxiangbin

            //  Let the child thread of a person  zhanghaicheng  Log in ,  Then initiate execution down ,
            BP.WF.Dev2Interface.Port_Login("zhanghaicheng");

            // Get this person  009  The work to be done .
            dt = BP.WF.Dev2Interface.DB_GenerEmpWorksOfDataTable(WebUser.No, WFState.Runing, "009");
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
            ht.Add("XianYouRenShu", 90);
            ht.Add("XinZengRenShu", 20);// The data is put in there , It is saved to the main table child thread , To check whether the data is aggregated to the confluence node .
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, threahWorkID, ht);

            #region 第1步:  Variable inspection after sending .
            if (objs.VarWorkID != threahWorkID)
                throw new Exception("@ It should be  VarWorkID=" + threahWorkID + " , It is :" + objs.VarWorkID);
            
            if (objs.VarCurrNodeID != 902)
                throw new Exception("@ It should be  VarCurrNodeID=902 是, It is :" + objs.VarCurrNodeID);

            if (objs.VarToNodeID != 999)
                throw new Exception("@ It should be  VarToNodeID= 999 是, It is :" + objs.VarToNodeID);

            if (objs.VarAcceptersID != "zhoupeng")
                throw new Exception("@ It should be  VarAcceptersID= zhoupeng 是, It is :" + objs.VarAcceptersID);
            #endregion 第1步:  Variable inspection after sending .

            #region 第2步:  Check the engine control system table .
            // Check Mainstream Data .
            gwf = new GenerWorkFlow(workid);
            if (gwf.WFState != WFState.Runing)
                throw new Exception("@ It should be  Runing,  It is :" + gwf.WFState);

            if (gwf.FID != 0)
                throw new Exception("@ It should be  0,  It is :" + gwf.FID);

            if (gwf.FK_Node != 901)
                throw new Exception("@ It should be  901,  It is :" + gwf.FK_Node);

            if (gwf.Starter != "zhoupeng")
                throw new Exception("@ It should be  zhoupeng,  It is :" + gwf.Starter);

            //  Mainstream staff table if there is a change ?
            GenerWorkerLists gwls = new GenerWorkerLists();
            gwls.Retrieve(GenerWorkerListAttr.WorkID, workid);
            foreach (GenerWorkerList item in gwls)
            {
                if (item.FK_Emp != "zhoupeng")
                    throw new Exception("@ It should be  zhoupeng,  It is :" + item.FK_Emp);

                // If this is the start node .
                if (item.FK_Node == 901)
                {
                    if (item.IsPass == false)
                        throw new Exception("@pass Status wrong , Should have been passed, .");
                }

                // If the end node .
                if (item.FK_Node == 999)
                {
                    // Check the child thread completion rate . 
                    Node nd = new Node(999);
                    if (nd.PassRate > 50)
                    {
                        //  Check the main thread data is correct .
                         sql = "SELECT COUNT(*) FROM WF_EmpWorks WHERE WorkID="+workid;
                        if (DBAccess.RunSQLReturnValInt(sql)!=0)
                            throw new Exception("@ Since the completion rate of more than  50,  So one by the , Staff can not see the main thread .");

                        //if (item.IsPassInt != 3)
                        //    throw new Exception("@ Since the completion rate of more than  50,  So one by the , Staff can not see the main thread , It is :"+item.IsPassInt);
                    }
                    else
                    {
                        if (item.IsPassInt != 0)
                            throw new Exception("@ Because less than 50, So long as there is an adoption , Main thread zhoupeng  Staff should be able to see the Upcoming , But to no avail . ");
                    }
                }
            }

            // Staff check a list of sub-thread table  .
            gwls = new GenerWorkerLists();
            gwls.Retrieve(GenerWorkerListAttr.FID, workid);
            if (gwls.Count != 3)
                throw new Exception("@ Staff are not expected on the two sub-thread list data .");
            foreach (GenerWorkerList item in gwls)
            {
                if (item.FK_Emp == "zhanghaicheng")
                {
                    if (item.IsPass == false)
                        throw new Exception("@ This person should be handled through the , Now do not pass .");
                }

                if (item.FK_Emp == "qifenglin")
                {
                    if (item.IsPass == true)
                        throw new Exception("@ This person should be run , The results do not meet expectations .");
                }

                if (item.FK_Emp == "guoxiangbin")
                {
                    if (item.IsPass == true)
                        throw new Exception("@ This person should be run , The results do not meet expectations .");
                }
            }
            #endregion 第2步:  Check the engine control system table .

            #region 第3步:  Inspection   Nodes form table data .
            sql = "SELECT * FROM ND901 WHERE OID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows[0]["Rec"].ToString() != "zhoupeng")
                throw new Exception("@ Start node Rec  Fields write error .");

            //// Check whether there is data node table form , And the data is correct ?
            //sql = "SELECT * FROM ND902 WHERE FID=" + workid;
            //dt = DBAccess.RunSQLReturnTable(sql);
            //if (dt.Rows.Count != 2)
            //    throw new Exception("@ Should find two data nodes on the first child thread ");

            //foreach (DataRow dr in dt.Rows)
            //{
            //    if (dr["Rec"].ToString() == "zhangyifan")
            //        continue;
            //    if (dr["Rec"].ToString() == "zhoushengyu")
            //        continue;
            //    throw new Exception("@ Child thread form data is not correctly written Rec Field .");
            //}

            //// Check the parameters are stored in the main table of the child thread ?
            //sql = "SELECT * FROM ND902 WHERE OID=" + threahWorkID;
            //dt = DBAccess.RunSQLReturnTable(sql);
            //if (dt.Rows.Count != 1)
            //    throw new Exception("@ Did not find the desired data sub-thread .");

            //if (dt.Rows[0]["FuWuQi"].ToString() != "90")
            //    throw new Exception(" Not saved to the specified location .");

            //if (dt.Rows[0]["ShuMaXiangJi"].ToString() != "20")
            //    throw new Exception(" Not saved to the specified location .");

            ////  Check whether the data is aggregated list copy Correct ?
            //  sql = "SELECT * FROM ND999Dtl1 WHERE OID=" + threahWorkID;
            //  dt = DBAccess.RunSQLReturnTable(sql);
            //  if (dt.Rows.Count != 1)
            //      throw new Exception("@ Sub-thread data is not copy To the list in the summary .");
            //  dt = DBAccess.RunSQLReturnTable(sql);
            //  if (dt.Rows.Count != 1)
            //      throw new Exception("@ Did not find the desired data sub-thread .");

            //  if (dt.Rows[0]["FuWuQi"].ToString() != "90")
            //      throw new Exception(" Not saved to the specified location .");

            //  if (dt.Rows[0]["ShuMaXiangJi"].ToString() != "20")
            //      throw new Exception(" Not saved to the specified location .");

            // Check the report data is correct ?
            sql = "SELECT * FROM  ND9Rpt WHERE OID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows[0][GERptAttr.FlowEnder].ToString() != "zhoupeng")
                throw new Exception("@ Should be  zhoupeng 是 FlowEnder .");

            if (dt.Rows[0][GERptAttr.FlowStarter].ToString() != "zhoupeng")
                throw new Exception("@ Should be  zhoupeng 是 FlowStarter .");

            if (dt.Rows[0][GERptAttr.FlowEndNode].ToString() != "901")
                throw new Exception("@ Should be  901 是 FlowEndNode, And now is :" + dt.Rows[0][GERptAttr.FlowEndNode].ToString());

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
            BP.WF.Dev2Interface.Port_Login("qifenglin");

            // Get this person  009  The work to be done .
            dt = BP.WF.Dev2Interface.DB_GenerEmpWorksOfDataTable(WebUser.No, WFState.Runing, "009");
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

            if (objs.VarCurrNodeID != 903)
                throw new Exception("@ It should be  VarCurrNodeID=903 是, It is :" + objs.VarCurrNodeID);

            if (objs.VarToNodeID != 999)
                throw new Exception("@ It should be  VarToNodeID= 999 是, It is :" + objs.VarToNodeID);

            //if (objs.VarAcceptersID != "zhoupeng")
            //    throw new Exception("@ It should be  VarAcceptersID= zhoupeng 是, It is :" + objs.VarAcceptersID);
            #endregion 第1步:  Variable inspection after sending .

            #region 第2步:  Check the engine control system table .
            // Check Mainstream Data .
            gwf = new GenerWorkFlow(workid);
            if (gwf.WFState != WFState.Runing)
                throw new Exception("@ It should be  Runing,  It is :" + gwf.WFState);

            if (gwf.FID != 0)
                throw new Exception("@ It should be  0,  It is :" + gwf.FID);

            if (gwf.FK_Node != 901)
                throw new Exception("@ It should be  901,  It is :" + gwf.FK_Node);

            if (gwf.Starter != "zhoupeng")
                throw new Exception("@ It should be  zhoupeng,  It is :" + gwf.Starter);

            //  Mainstream staff table if there is a change ?
            GenerWorkerLists gwls = new GenerWorkerLists();
            gwls.Retrieve(GenerWorkerListAttr.WorkID, workid);
            foreach (GenerWorkerList item in gwls)
            {
                if (item.FK_Emp != "zhoupeng")
                    throw new Exception("@ It should be  zhoupeng,  It is :" + item.FK_Emp);

                // If this is the start node .
                if (item.FK_Node == 901)
                {
                    if (item.IsPass == false)
                        throw new Exception("@pass Status wrong , Should have been passed, .");
                }

                // If the end node .
                if (item.FK_Node == 999)
                {
                    // Check the child thread completion rate .
                    Node nd = new Node(999);
                    if (nd.PassRate > 50)
                    {
                        if (item.IsPassInt != 0)
                            throw new Exception("@ Since the completion rate of more than  50,  Now the two have passed , So this confluence point should also be adopted by state .");
                    }
                    else
                    {
                        if (item.IsPassInt != 0)
                            throw new Exception("@ Because less than 50, So long as there is an adoption , Main thread zhoupeng  Staff should be able to see the Upcoming , But to no avail . ");
                    }
                }
            }

            // Staff check a list of sub-thread table .
            gwls = new GenerWorkerLists();
            gwls.Retrieve(GenerWorkerListAttr.FID, workid);
            if (gwls.Count != 3)
                throw new Exception("@ Staff are not expected on the two sub-thread list data ,  Should be 3, It is :"+gwls.Count);
            foreach (GenerWorkerList item in gwls)
            {
                if (item.FK_Emp == "zhanghaicheng")
                {
                    if (item.IsPass == false)
                        throw new Exception("@ This person should be handled through the , Now do not pass .");
                }

                if (item.FK_Emp == "qifenglin")
                {
                    if (item.IsPass == false)
                        throw new Exception("@ This person should be handled through the , Now do not pass .");
                }

                if (item.FK_Emp == "guoxiangbin")
                {
                    if (item.IsPass == true)
                        throw new Exception("@ This person should be run , The results do not meet expectations .");
                }
            }
            #endregion 第2步:  Check the engine control system table .

            #region 第3步:  Inspection   Nodes form table data .
            sql = "SELECT * FROM ND901 WHERE OID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows[0]["Rec"].ToString() != "zhoupeng")
                throw new Exception("@ Start node Rec  Fields write error .");

            //// Check whether there is data node table form , And the data is correct ?
            //sql = "SELECT * FROM ND902 WHERE FID=" + workid;
            //dt = DBAccess.RunSQLReturnTable(sql);
            //if (dt.Rows.Count != 3)
            //    throw new Exception("@ Should be found on the first child node thread  3  Data , It is :"+dt.Rows.Count);]

            //foreach (DataRow dr in dt.Rows)
            //{
            //    if (dr["Rec"].ToString() == "zhangyifan")
            //    {
            //        continue;
            //    }
            //    if (dr["Rec"].ToString() == "zhoushengyu")
            //    {
            //        continue;
            //    }
            //    throw new Exception("@ Child thread form data is not correctly written Rec Field .");
            //}

            //// Check the parameters are stored in the main table of the child thread ?
            //sql = "SELECT * FROM ND902 WHERE OID=" + threahWorkID;
            //dt = DBAccess.RunSQLReturnTable(sql);
            //if (dt.Rows.Count != 1)
            //    throw new Exception("@ Did not find the desired data sub-thread .");

            //if (dt.Rows[0]["FuWuQi"].ToString() != "100")
            //    throw new Exception(" Not saved to the specified location .");

            //if (dt.Rows[0]["ShuMaXiangJi"].ToString() != "30")
            //    throw new Exception(" Not saved to the specified location .");


            ////  Check whether the data is aggregated list copy Correct ?
            //sql = "SELECT * FROM ND999Dtl1 WHERE OID=" + threahWorkID;
            //dt = DBAccess.RunSQLReturnTable(sql);
            //if (dt.Rows.Count != 1)
            //    throw new Exception("@ Sub-thread data is not copy To the list in the summary .");
            //dt = DBAccess.RunSQLReturnTable(sql);
            //if (dt.Rows.Count != 1)
            //    throw new Exception("@ Did not find the desired data sub-thread .");

            //if (dt.Rows[0]["FuWuQi"].ToString() != "100")
            //    throw new Exception(" Not saved to the specified location .");

            //if (dt.Rows[0]["ShuMaXiangJi"].ToString() != "30")
            //    throw new Exception(" Not saved to the specified location .");
             

            // Check the report data is correct ?
            sql = "SELECT * FROM  ND9Rpt WHERE OID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows[0][GERptAttr.FlowEnder].ToString() != "zhoupeng")
                throw new Exception("@ Should be  zhoupeng 是 FlowEnder .");

            if (dt.Rows[0][GERptAttr.FlowStarter].ToString() != "zhoupeng")
                throw new Exception("@ Should be  zhoupeng 是 FlowStarter .");

            if (dt.Rows[0][GERptAttr.FlowEndNode].ToString() != "901")
                throw new Exception("@ Should be  901 是 FlowEndNode, It is :" + dt.Rows[0][GERptAttr.FlowEndNode].ToString());

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
        public void Step2_3()
        {
            //  Let the child thread of a person  zhoushengyu  Log in ,  Then initiate execution down ,
            BP.WF.Dev2Interface.Port_Login("guoxiangbin");

            // Get this person  009  The work to be done .
            dt = BP.WF.Dev2Interface.DB_GenerEmpWorksOfDataTable(WebUser.No, WFState.Runing, "009");
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

            if (objs.VarCurrNodeID != 904)
                throw new Exception("@ It should be  VarCurrNodeID=904 是, It is :" + objs.VarCurrNodeID);

            if (objs.VarToNodeID != 999)
                throw new Exception("@ It should be  VarToNodeID= 999 是, It is :" + objs.VarToNodeID);

            //if (objs.VarAcceptersID != "zhoupeng")
            //    throw new Exception("@ It should be  VarAcceptersID= zhoupeng 是, It is :" + objs.VarAcceptersID);
            #endregion 第1步:  Variable inspection after sending .

            #region 第2步:  Check the engine control system table .
            // Check Mainstream Data .
            gwf = new GenerWorkFlow(workid);
            if (gwf.WFState != WFState.Runing)
                throw new Exception("@ It should be  Runing,  It is :" + gwf.WFState);

            if (gwf.FID != 0)
                throw new Exception("@ It should be  0,  It is :" + gwf.FID);

            if (gwf.FK_Node != 999)
                throw new Exception("@ It should be  999,  It is :" + gwf.FK_Node);

            if (gwf.Starter != "zhoupeng")
                throw new Exception("@ It should be  zhoupeng,  It is :" + gwf.Starter);

            //  Mainstream staff table if there is a change ?
            GenerWorkerLists gwls = new GenerWorkerLists();
            gwls.Retrieve(GenerWorkerListAttr.WorkID, workid);
            foreach (GenerWorkerList item in gwls)
            {
                if (item.FK_Emp != "zhoupeng")
                    throw new Exception("@ It should be  zhoupeng,  It is :" + item.FK_Emp);

                // If this is the start node .
                if (item.FK_Node == 901)
                {
                    if (item.IsPass == false)
                        throw new Exception("@pass Status wrong , Should have been passed, .");
                }

                // If the end node .
                if (item.FK_Node == 999)
                {
                    // Check the child thread completion rate .
                    Node nd = new Node(999);
                    if (nd.PassRate > 50)
                    {
                        if (item.IsPassInt != 0)
                            throw new Exception("@ Since the completion rate of more than  50,  Now the two have passed , So this confluence point should also be adopted by state .");
                    }
                    else
                    {
                        if (item.IsPassInt != 0)
                            throw new Exception("@ Because less than 50, So long as there is an adoption , Main thread zhoupeng  Staff should be able to see the Upcoming , But to no avail . ");
                    }
                }
            }

            // Staff check a list of sub-thread table .
            gwls = new GenerWorkerLists();
            gwls.Retrieve(GenerWorkerListAttr.FID, workid);
            if (gwls.Count != 3)
                throw new Exception("@ Not expected  3  Staff sliver thread list data .");

            foreach (GenerWorkerList item in gwls)
            {
                if (item.FK_Emp == "zhanghaicheng")
                {
                    if (item.IsPass == true)
                        throw new Exception("@ This person should be handled through the , Now do not pass .");
                }

                if (item.FK_Emp == "qifenglin")
                {
                    if (item.IsPass == true)
                        throw new Exception("@ This person should be handled through the , Now do not pass .");
                }

                if (item.FK_Emp == "guoxiangbin")
                {
                    if (item.IsPass == false)
                        throw new Exception("@ This person untreated .");
                }
            }
            #endregion 第2步:  Check the engine control system table .

            #region 第3步:  Inspection   Nodes form table data .
            sql = "SELECT * FROM ND901 WHERE OID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows[0]["Rec"].ToString() != "zhoupeng")
                throw new Exception("@ Start node Rec  Fields write error .");


            ////  Check whether the data is aggregated list copy Correct ?
            //sql = "SELECT * FROM ND999Dtl1 WHERE OID=" + threahWorkID;
            //dt = DBAccess.RunSQLReturnTable(sql);
            //if (dt.Rows.Count != 1)
            //    throw new Exception("@ Sub-thread data is not copy To the list in the summary .");
            //dt = DBAccess.RunSQLReturnTable(sql);
            //if (dt.Rows.Count != 1)
            //    throw new Exception("@ Did not find the desired data sub-thread .");

            //if (dt.Rows[0]["FuWuQi"].ToString() != "100")
            //    throw new Exception(" Not saved to the specified location .");

            //if (dt.Rows[0]["ShuMaXiangJi"].ToString() != "30")
            //    throw new Exception(" Not saved to the specified location .");


            // Check the report data is correct ?
            sql = "SELECT * FROM  ND9Rpt WHERE OID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows[0][GERptAttr.FlowEnder].ToString() != "zhoupeng")
                throw new Exception("@ Should be  zhoupeng 是 FlowEnder .");

            if (dt.Rows[0][GERptAttr.FlowStarter].ToString() != "zhoupeng")
                throw new Exception("@ Should be  zhoupeng 是 FlowStarter .");

            if (dt.Rows[0][GERptAttr.FlowEndNode].ToString() != "902")
                throw new Exception("@ Should be  902 是 FlowEndNode .");

            if (int.Parse(dt.Rows[0][GERptAttr.WFState].ToString()) != (int)WFState.Runing)
                throw new Exception("@ Should be  WFState.Runing 是 WFState .");

            if (int.Parse(dt.Rows[0][GERptAttr.FID].ToString()) != 0)
                throw new Exception("@ Should be  FID =0 ");

            if (dt.Rows[0]["FK_NY"].ToString() != DataType.CurrentYearMonth)
                throw new Exception("@ FK_NY  Field filled error . ");
            #endregion 第3步:  Inspection   Nodes form table data .
        }
        /// <summary>
        ///  Carried out zhoupeng的  Send .
        /// 1, Object Inspector sent .
        /// 2, Check the process engine control table .
        /// 3, Check the node table .
        /// </summary>
        public void Step3()
        {
            //  Let sponsor on the main thread Login .
            BP.WF.Dev2Interface.Port_Login("zhoupeng");

            //  Transmission is performed to the last node 
            objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid);

            #region 第1步:  Variable inspection after sending .
            if (objs.VarWorkID != workid)
                throw new Exception("@ It should be  VarWorkID=" + workid + " , It is :" + objs.VarWorkID);

            if (objs.VarCurrNodeID != 999)
                throw new Exception("@ It should be  VarCurrNodeID=999 是, It is :" + objs.VarCurrNodeID);

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
            sql = "SELECT * FROM  ND999Dtl1 WHERE RefPK=" + workid;
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
            sql = "SELECT * FROM  ND9Rpt WHERE OID=" + workid;
            dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows[0][GERptAttr.FlowEnder].ToString() != "zhoupeng")
                throw new Exception("@ Should be  zhoupeng 是 FlowEnder .");

               if ( string.IsNullOrEmpty(dt.Rows[0][GERptAttr.Title].ToString()) )
                            throw new Exception("@ After the process has completed the title is lost ");

            if (dt.Rows[0][GERptAttr.FlowStarter].ToString() != "zhoupeng")
                throw new Exception("@ Should be  zhoupeng 是 FlowStarter .");

            if (dt.Rows[0][GERptAttr.FlowEndNode].ToString() != "999")
                throw new Exception("@ Should be  999 是 FlowEndNode .");

            if (dt.Rows[0][GERptAttr.FlowEnder].ToString() != "zhoupeng")
                throw new Exception("@ Should be  zhoupeng 是 FlowEnder .");

            if (int.Parse(dt.Rows[0][GERptAttr.WFState].ToString()) != (int)WFState.Complete)
                throw new Exception("@ Should be  WFState.Complete  Is the current state of  .");

            if (int.Parse(dt.Rows[0][GERptAttr.FID].ToString()) != 0)
                throw new Exception("@ Should be  FID =0 ");

            if (dt.Rows[0]["FK_NY"].ToString() != DataType.CurrentYearMonth)
                throw new Exception("@ FK_NY  Field filled error . ");

            //  Check that the child thread node table data exists ?
            sql = "SELECT * FROM ND902 WHERE FID=" + workid;
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
