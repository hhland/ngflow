using System;
using BP.En;
using BP.DA;
using System.Collections;
using System.Data;
using BP.Port;
using BP.Web;
using BP.Sys;
namespace BP.WF
{
    /// <summary>
    ///  Return processing 
    /// </summary>
    public class WorkReturn
    {
        #region  Variable 
        /// <summary>
        ///  From node 
        /// </summary>
        private Node HisNode = null;
        /// <summary>
        ///  Back to the node 
        /// </summary>
        private Node ReurnToNode = null;
        /// <summary>
        ///  The work ID
        /// </summary>
        private Int64 WorkID = 0;
        /// <summary>
        ///  Process ID
        /// </summary>
        private Int64 FID = 0;
        /// <summary>
        ///  Whether the original way back ?
        /// </summary>
        private bool IsBackTrack = false;
        /// <summary>
        ///  Reason for the return 
        /// </summary>
        private string Msg = " Reason for the return is not filled .";

        public string MsgTitlePattern = " Return to work : Process :{0}. The work :{1}, Return man :{2}, You need to deal with ";

        /// <summary>
        ///  The current node 
        /// </summary>
        private Work HisWork = null;
        /// <summary>
        ///  Back to the node 
        /// </summary>
        private Work ReurnToWork = null;
        private string dbStr = BP.Sys.SystemConfig.AppCenterDBVarStr;
        private Paras ps;
        public string ReturnToEmp = null;
        #endregion

        /// <summary>
        ///  Return to work 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="workID">WorkID</param>
        /// <param name="fid"> Process ID</param>
        /// <param name="currNodeID"> From node </param>
        /// <param name="reurnToNodeID"> Back to the node </param>
        /// <param name="reutrnToEmp"> Back to the people </param>
        /// <param name="isBackTrack"> The need to backtrack ?</param>
        /// <param name="returnInfo"> Reason for the return </param>
        public WorkReturn(string fk_flow, Int64 workID, Int64 fid, int currNodeID, int reurnToNodeID, string reutrnToEmp, bool isBackTrack, string returnInfo)
        {
            this.HisNode = new Node(currNodeID);
            this.ReurnToNode = new Node(reurnToNodeID);
            this.WorkID = workID;
            this.FID = fid;
            this.IsBackTrack = isBackTrack;
            this.Msg = returnInfo;
            this.ReturnToEmp = reutrnToEmp;

            // Current work .
            this.HisWork = this.HisNode.HisWork;

            this.HisWork.OID = workID;
            this.HisWork.Retrieve();

            // Return to work 
            this.ReurnToWork = this.ReurnToNode.HisWork;
            this.ReurnToWork.OID = workID;
            if (this.ReurnToWork.RetrieveFromDBSources() == 0)
            {
                this.ReurnToWork.OID = fid;
                this.ReurnToWork.RetrieveFromDBSources();
            }
        }
        /// <summary>
        ///  Business data and process engine control data between two nodes deleted .
        /// </summary>
        private void DeleteSpanNodesGenerWorkerListData()
        {
            if (this.IsBackTrack == true)
                return;

            Paras ps = new Paras();
            string dbStr = SystemConfig.AppCenterDBVarStr;

            //  Delete FH,  Whether or not there is this data .
            ps.Clear();
            ps.SQL = "DELETE FROM WF_GenerFH WHERE FID=" + dbStr + "FID";
            ps.Add("FID", this.WorkID);
            DBAccess.RunSQL(ps);

            /* If not, go back and backtrack , It needs to be cleared   Data between two nodes ,  Include WF_GenerWorkerList Data .*/
            if (this.ReurnToNode.IsStartNode == true)
            {
                //  Remove its sub-thread processes .
                ps.Clear();
                ps.SQL = "DELETE FROM WF_GenerWorkFlow WHERE FID=" + dbStr + "FID ";
                ps.Add("FID", this.WorkID);
                DBAccess.RunSQL(ps);

                /* If you go back to the beginning of the node , Delete the data outside the starting node , Do not delete the node form data , This will cause the process track does not open .*/
                ps.Clear();
                ps.SQL = "DELETE FROM WF_GenerWorkerList WHERE FK_Node!=" + dbStr+GenerWorkerListAttr.FK_Node + " AND (WorkID=" + dbStr + "WorkID1 OR FID=" + dbStr + "WorkID2)";
                ps.Add(GenerWorkerListAttr.FK_Node, this.ReurnToNode.NodeID);
                ps.Add("WorkID1", this.WorkID);
                ps.Add("WorkID2", this.WorkID);
                DBAccess.RunSQL(ps);
                return;
            }

            /* Returned to find the time to send to , Since this point of time the data must be deleted .*/
            ps.Clear();
            ps.SQL = "SELECT RDT,ActionType,NDFrom FROM ND" + int.Parse(this.HisNode.FK_Flow) + "Track WHERE  NDFrom=" + dbStr + "NDFrom AND WorkID=" + dbStr + "WorkID AND ActionType=" + (int)ActionType.Forward + " ORDER BY RDT ";
            ps.Add("NDFrom", this.ReurnToNode.NodeID);
            ps.Add("WorkID", this.WorkID);
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(ps);
            if (dt.Rows.Count >= 1)
            {
                string rdt = dt.Rows[0][0].ToString();

                ps.Clear();
                ps.SQL = "SELECT ActionType,NDFrom,'" + rdt + "' FROM ND" + int.Parse(this.HisNode.FK_Flow) + "Track WHERE WorkID=" + dbStr + "WorkID and  RDT >=" + dbStr + "RDT   ORDER BY RDT ";
                ps.Add("RDT", rdt);
                ps.Add("WorkID", this.WorkID);
                dt = BP.DA.DBAccess.RunSQLReturnTable(ps);

                foreach (DataRow dr in dt.Rows)
                {
                    ActionType at = (ActionType)int.Parse(dr["ActionType"].ToString());
                    int nodeid = int.Parse(dr["NDFrom"].ToString());
                    if (nodeid == this.ReurnToNode.NodeID)
                        continue;

                    ps.Clear();
                    ps.SQL = "DELETE FROM WF_GenerWorkerList WHERE FK_Node=" + dbStr + "FK_Node AND (WorkID=" + dbStr + "WorkID1 OR FID=" + dbStr + "WorkID2) ";
                    ps.Add("FK_Node", nodeid);
                    ps.Add("WorkID1", this.WorkID);
                    ps.Add("WorkID2", this.WorkID);
                    DBAccess.RunSQL(ps);
                }
            }


            // Data Delete the current node .
            ps.Clear();
            ps.SQL = "DELETE FROM WF_GenerWorkerList WHERE FK_Node=" + dbStr + "FK_Node AND (WorkID=" + dbStr + "WorkID1 OR FID=" + dbStr + "WorkID2) ";
            ps.Add("FK_Node", this.HisNode.NodeID);
            ps.Add("WorkID1", this.WorkID);
            ps.Add("WorkID2", this.WorkID);
            DBAccess.RunSQL(ps);

            //  string sql = "SELECT * FROM ND" + int.Parse(this.HisNode.FK_Flow) + "Track WHERE  NDTo='"+this.ReurnToNode.NodeID+" AND WorkID="+this.WorkID;
            //  ActionType
        }
        /// <summary>
        ///  A person on the queue node returned another person .
        /// </summary>
        /// <returns></returns>
        public string DoOrderReturn()
        {
            // Return before the event 
            string atPara = "@ToNode=" + this.ReurnToNode.NodeID;
            string msg = this.HisNode.HisFlow.DoFlowEventEntity(EventListOfNode.ReturnBefore, this.HisNode, this.HisWork, atPara);
           
            if (this.HisNode.FocusField != "")
            {
                try
                {
                    string focusField = "";
                    string[] focusFields = this.HisNode.FocusField.Split('@');
                    if (focusFields.Length >= 2)
                        focusField = focusFields[1];
                    else
                        focusField = focusFields[0];



                    //  The data is updated it .
                    this.HisWork.Update(focusField, "");
                }
                catch (Exception ex)
                {
                    Log.DefaultLogWriteLineError(" When updating the focus returned to the field error :" + ex.Message);
                }
            }

            // Back to the people .
            Emp returnToEmp = new Emp(this.ReturnToEmp);

            //  Return status .
            Paras ps = new Paras();
            ps.SQL = "UPDATE WF_GenerWorkFlow SET WFState=" + dbStr + "WFState,FK_Node=" + dbStr + "FK_Node,NodeName=" + dbStr + "NodeName,TodoEmps=" + dbStr + "TodoEmps, TodoEmpsNum=0 WHERE  WorkID=" + dbStr + "WorkID";
            ps.Add(GenerWorkFlowAttr.WFState, (int)WFState.ReturnSta);
            ps.Add(GenerWorkFlowAttr.FK_Node, this.ReurnToNode.NodeID);
            ps.Add(GenerWorkFlowAttr.NodeName, this.ReurnToNode.Name);

            ps.Add(GenerWorkFlowAttr.TodoEmps, returnToEmp.No + "," + returnToEmp.Name + ";");

            ps.Add(GenerWorkFlowAttr.WorkID, this.WorkID);

            DBAccess.RunSQL(ps);

            ps = new Paras();
            ps.SQL = "UPDATE WF_GenerWorkerList SET IsPass=0,IsRead=0 WHERE FK_Node=" + dbStr + "FK_Node AND WorkID=" + dbStr + "WorkID AND FK_Emp=" + dbStr + "FK_Emp ";
            ps.Add("FK_Node", this.ReurnToNode.NodeID);
            ps.Add("WorkID", this.WorkID);
            ps.Add("FK_Emp", this.ReturnToEmp);
            DBAccess.RunSQL(ps);

            ps = new Paras();
            ps.SQL = "UPDATE WF_GenerWorkerList SET IsPass=1000,IsRead=0 WHERE FK_Node=" + dbStr + "FK_Node AND WorkID=" + dbStr + "WorkID AND FK_Emp=" + dbStr + "FK_Emp ";
            ps.Add("FK_Node", this.HisNode.NodeID);
            ps.Add("WorkID", this.WorkID);
            ps.Add("FK_Emp", WebUser.No);
            DBAccess.RunSQL(ps);

            // Report data update process .
            ps = new Paras();
            ps.SQL = "UPDATE " + this.HisNode.HisFlow.PTable + " SET  WFState=" + dbStr + "WFState, FlowEnder=" + dbStr + "FlowEnder, FlowEndNode=" + dbStr + "FlowEndNode WHERE OID=" + dbStr + "OID";
            ps.Add("WFState", (int)WFState.ReturnSta);
            ps.Add("FlowEnder", WebUser.No);
            ps.Add("FlowEndNode", ReurnToNode.NodeID);
            ps.Add("OID", this.WorkID);
            DBAccess.RunSQL(ps);

            //// Was returned to find people to accept people from the staff list .
            //GenerWorkerList gwl = new GenerWorkerList();
            //gwl.Retrieve(GenerWorkerListAttr.FK_Node, this.ReurnToNode.NodeID, GenerWorkerListAttr.WorkID, this.WorkID);

            //  Record returned to the track .
            ReturnWork rw = new ReturnWork();
            rw.WorkID = this.WorkID;
            rw.ReturnToNode = this.ReurnToNode.NodeID;
            rw.ReturnNodeName = this.ReurnToNode.Name;

            rw.ReturnNode = this.HisNode.NodeID; //  Current return node .
            rw.ReturnToEmp = this.ReturnToEmp; // Returned to .
            rw.Note = Msg;

            rw.MyPK = DBAccess.GenerOIDByGUID().ToString();
            rw.Insert();

            //  Join track.
            this.AddToTrack(ActionType.Return, returnToEmp.No, returnToEmp.Name,
                this.ReurnToNode.NodeID, this.ReurnToNode.Name, Msg);

            try
            {
                //  Log records returned .
                ReorderLog(this.ReurnToNode, this.HisNode, rw);
            }
            catch (Exception ex)
            {
                Log.DebugWriteWarning(ex.Message);
            }

            //  To return to the node forwards data using recursive delete it .
            if (IsBackTrack == false)
            {
                /* If you do not need to backtrack to return , Deleted data intermediate point .*/
#warning  Did not consider two processes data storage mode .
                //DeleteToNodesData(this.ReurnToNode.HisToNodes);
            }

            //  Send a message to him .
            if (Glo.IsEnableSysMessage == true)
            {
                //   WF.Port.WFEmp wfemp = new Port.WFEmp(wnOfBackTo.HisWork.Rec);
                string title = string.Format(this.MsgTitlePattern,
                    this.HisNode.FlowName, this.ReurnToNode.Name, WebUser.Name);

                BP.WF.Dev2Interface.Port_SendMsg(returnToEmp.No, title, Msg, "RE" + this.HisNode.NodeID + this.WorkID, BP.WF.SMSMsgType.ToDo, ReurnToNode.FK_Flow, ReurnToNode.NodeID, this.WorkID, this.FID);
            }
            // Returned after the event 

            string text = this.HisNode.HisFlow.DoFlowEventEntity(EventListOfNode.ReturnAfter, this.HisNode, this.HisWork,
                atPara);
            if (text != null && text.Length > 1000)
                text = " Return event : No return information .";
            //  Retraction information .
            if (this.ReurnToNode.IsGuestNode)
            {
                GenerWorkFlow gwf = new GenerWorkFlow(this.WorkID);
                return " You have been back to work (" + this.ReurnToNode.Name + "), Returned to (" + gwf.GuestNo + "," + gwf.GuestName + ").\n\r" + text;
            }
            else
            {
                return " You have been back to work (" + this.ReurnToNode.Name + "), Returned to (" + returnToEmp.No + "," + returnToEmp.Name + ").\n\r" + text;
            }
        }
        /// <summary>
        ///  Implementation of return .
        /// </summary>
        /// <returns> Retraction information </returns>
        public string DoIt()
        {
            if (this.HisNode.NodeID == this.ReurnToNode.NodeID)
            {
                if (this.HisNode.TodolistModel == TodolistModel.Order)
                {
                    /* A queue model , A man returned to another person  */
                    return DoOrderReturn();
                }
            }

            if (this.ReurnToNode.TodolistModel == TodolistModel.Order)
            {
                /*  When returned to the node is   Queue mode or cooperative mode . */
                return DoOrderReturn();
            }


            // Delete .
            BP.WF.Dev2Interface.DeleteCheckInfo(this.HisNode.FK_Flow, this.WorkID, this.HisNode.NodeID);

            switch (this.HisNode.HisRunModel)
            {
                case RunModel.Ordinary: /* 1:  Common node sends down */
                    switch (ReurnToNode.HisRunModel)
                    {
                        case RunModel.Ordinary:   /*1-1  Ordinary Day to Ordinary nodes  */
                            return ExeReturn1_1(); //
                            break;
                        case RunModel.FL:  /* 1-2  Ordinary Day to Split point    */
                            return ExeReturn1_1(); //
                            break;
                        case RunModel.HL:  /*1-3  Ordinary Day to Confluence    */
                            return ExeReturn1_1(); //
                            break;
                        case RunModel.FHL: /*1-4  Ordinary nodes to Confluence points  */
                            return ExeReturn1_1();
                            break;
                        case RunModel.SubThread: /*1-5  Ordinary Day to Child thread point  */
                        default:
                            throw new Exception("@ Return error : The design pattern of illegal or return mode . Ordinary Day to Child thread point ");
                            break;
                    }
                    break;
                case RunModel.FL: /* 2:  Shunt node sends down */
                    switch (this.ReurnToNode.HisRunModel)
                    {
                        case RunModel.Ordinary:    /*2.1  Split point to Ordinary nodes  */
                            return ExeReturn1_1(); //
                            break;
                        case RunModel.FL:  /*2.2  Split point to Split point   */
                        case RunModel.HL:  /*2.3  Split point to Confluence , Confluence points    */
                        case RunModel.FHL:
                            return ExeReturn1_1(); //
                            break;
                        case RunModel.SubThread: /* 2.4  Split point to Child thread point    */
                            throw new Exception("@ Return error : The design pattern of illegal or return mode . Split point to Child thread point , Please feedback to the administrator .");
                        default:
                            throw new Exception("@ Node type is not judged (" + ReurnToNode.Name + ")");
                            break;
                    }
                    break;
                case RunModel.HL:  /* 3:  Confluence node sends down  */
                    switch (this.ReurnToNode.HisRunModel)
                    {
                        case RunModel.Ordinary: /*3.1  Ordinary working nodes  */
                            return ExeReturn1_1(); //
                            break;
                        case RunModel.FL: /*3.2  Return to the confluence point of diversion  */
                            return ExeReturn3_2(); //
                            break;
                        case RunModel.HL: /*3.3  Confluence  */
                        case RunModel.FHL:
                            throw new Exception("@ Not completed .");
                            break;
                        case RunModel.SubThread:/*3.4  Confluence point to return the child thread  */
                            return ExeReturn3_4();
                            break;
                        default:
                            throw new Exception("@ Return error : The design pattern of illegal or return mode . Ordinary Day to Child thread point ");
                    }
                    break;
                case RunModel.FHL:  /* 4:  Shunt node sends down  */
                    switch (this.ReurnToNode.HisRunModel)
                    {
                        case RunModel.Ordinary: /*4.1  Ordinary working nodes  */
                            return ExeReturn1_1();
                            break;
                        case RunModel.FL: /*4.2  Split point  */
                        case RunModel.HL: /*4.3  Confluence  */
                        case RunModel.FHL:
                            throw new Exception("@ Not completed .");
                        case RunModel.SubThread:/*4.5  Child thread */
                            return ExeReturn3_4();
                            break;
                        default:
                            throw new Exception("@ Node type is not judged (" + this.ReurnToNode.Name + ")");
                    }
                    break;
                case RunModel.SubThread:  /* 5:  Child thread node sends down  */
                    switch (this.ReurnToNode.HisRunModel)
                    {
                        case RunModel.Ordinary: /*5.1  Ordinary working nodes  */
                            throw new Exception("@ Illegal return mode ,, Please feedback to the administrator .");
                            break;
                        case RunModel.FL: /*5.2  Split point  */
                            throw new Exception("@ Return is not currently supported under this scenario ,, Please feedback to the administrator .");
                        case RunModel.HL: /*5.3  Confluence  */
                            throw new Exception("@ Illegal return mode ,, Please feedback to the administrator .");
                            break;
                        case RunModel.FHL: /*5.4  Confluence points  */
                            throw new Exception("@ Return is not currently supported under this scenario , Please feedback to the administrator .");
                            break;
                        case RunModel.SubThread: /*5.5  Child thread */
                            return ExeReturn1_1();
                            break;
                        default:
                            throw new Exception("@ Node type is not judged (" + ReurnToNode.Name + ")");
                    }
                    break;
                default:
                    throw new Exception("@ Does not determine the type of :" + this.HisNode.HisRunModel);
            }
            throw new Exception("@ System abnormalities appear not to judge .");
        }
        /// <summary>
        ///  Confluence point to return the child thread 
        /// </summary>
        private string ExeReturn3_4()
        {
            GenerWorkFlow gwf = new GenerWorkFlow(this.WorkID);
            gwf.FK_Node = this.ReurnToNode.NodeID;

            string info = "@ Work has been successfully returned to （" + ReurnToNode.Name + "） Returned to :";
            GenerWorkerLists gwls = new GenerWorkerLists();
            gwls.Retrieve(GenerWorkerListAttr.WorkID, this.WorkID,
                GenerWorkerListAttr.FK_Node, this.ReurnToNode.NodeID);

            string toEmp = "";
            string toEmpName = "";
            foreach (GenerWorkerList item in gwls)
            {
                item.IsPass = false;
                item.IsRead = false;
                item.Update();
                info += item.FK_Emp + "," + item.FK_EmpText;
                toEmp = item.FK_Emp;
                toEmpName = item.FK_EmpText;
            }

            // Delete aggregated data has been sent to the confluence point .
            MapDtls dtls = new MapDtls("ND" + this.HisNode.NodeID);
            foreach (MapDtl dtl in dtls)
            {
                /* If it is the confluence of data */
                if (dtl.IsHLDtl)
                    BP.DA.DBAccess.RunSQL("DELETE FROM " + dtl.PTable + " WHERE OID=" + this.WorkID);
            }



            //  Record returned to the track .
            ReturnWork rw = new ReturnWork();
            rw.WorkID = this.WorkID;
            rw.ReturnToNode = this.ReurnToNode.NodeID;
            rw.ReturnNodeName = this.ReurnToNode.Name;

            rw.ReturnNode = this.HisNode.NodeID; //  Current return node .
            rw.ReturnToEmp = toEmp; // Returned to .

            rw.MyPK = DBAccess.GenerOIDByGUID().ToString();
            rw.Note = Msg;
            rw.IsBackTracking = this.IsBackTrack;
            rw.Insert();

            //  Join track.
            this.AddToTrack(ActionType.Return, toEmp, toEmpName,
                this.ReurnToNode.NodeID, this.ReurnToNode.Name, Msg);

            gwf.WFState = WFState.ReturnSta;
            gwf.Update();

            //  Retraction information .
            return info;
        }
        /// <summary>
        ///  Return to the confluence point of diversion 
        /// </summary>
        private string ExeReturn3_2()
        {
            // Sub-thread data deletion diversion point and confluence between .
            if (this.ReurnToNode.IsStartNode == false)
                throw new Exception("@ No processing mode .");

            // Removes the child node data thread .
            GenerWorkerLists gwls = new GenerWorkerLists();
            gwls.Retrieve(GenerWorkFlowAttr.FID, this.WorkID);

            foreach (GenerWorkerList item in gwls)
            {
                /*  Delete   Sub-thread data  */
                DBAccess.RunSQL("DELETE FROM ND" + item.FK_Node + " WHERE OID=" + item.WorkID);
            }

            // Delete process control data .
            DBAccess.RunSQL("DELETE FROM WF_GenerWorkFlow WHERE FID=" + this.WorkID);
            DBAccess.RunSQL("DELETE FROM WF_GenerWorkerList WHERE FID=" + this.WorkID);
            DBAccess.RunSQL("DELETE FROM WF_GenerFH WHERE FID=" + this.WorkID);

            return ExeReturn1_1();
        }
        /// <summary>
        ///  Whether backtrack ?
        /// </summary>
        public bool IsBackTracking = false;
        /// <summary>
        ///  Return to normal ordinary node node 
        /// </summary>
        /// <returns></returns>
        private string ExeReturn1_1()
        {
            // Return before the event 
            string atPara = "@ToNode=" + this.ReurnToNode.NodeID;
            string msg = this.HisNode.HisFlow.DoFlowEventEntity(EventListOfNode.ReturnBefore, this.HisNode, this.HisWork,
                atPara);
           
            if (this.HisNode.FocusField != "")
            {
                try
                {
                    string focusField = "";
                    string[] focusFields = this.HisNode.FocusField.Split('@');
                    if (focusFields.Length >= 2)
                        focusField = focusFields[1];
                    else
                        focusField = focusFields[0];



                    //  The data is updated it .
                    this.HisWork.Update(focusField, "");
                }
                catch (Exception ex)
                {
                    Log.DefaultLogWriteLineError(" When updating the focus returned to the field error :" + ex.Message);
                }
            }

            //  Work to be done to change the current node .
            Paras ps = new Paras();
            ps.SQL = "UPDATE WF_GenerWorkFlow  SET WFState=" + dbStr + "WFState,FK_Node=" + dbStr + "FK_Node,NodeName=" + dbStr + "NodeName WHERE  WorkID=" + dbStr + "WorkID";
            ps.Add(GenerWorkFlowAttr.WFState, (int)WFState.ReturnSta);
            ps.Add(GenerWorkFlowAttr.FK_Node, this.ReurnToNode.NodeID);
            ps.Add(GenerWorkFlowAttr.NodeName, this.ReurnToNode.Name);
            ps.Add(GenerWorkFlowAttr.WorkID, this.WorkID);
            DBAccess.RunSQL(ps);

            ps = new Paras();
            ps.SQL = "UPDATE WF_GenerWorkerList SET IsPass=0,IsRead=0 WHERE FK_Node=" + dbStr + "FK_Node AND WorkID=" + dbStr + "WorkID";
            ps.Add("FK_Node", this.ReurnToNode.NodeID);
            ps.Add("WorkID", this.WorkID);
            DBAccess.RunSQL(ps);


            // Report data update process .
            ps = new Paras();
            ps.SQL = "UPDATE " + this.HisNode.HisFlow.PTable + " SET  WFState=" + dbStr + "WFState, FlowEnder=" + dbStr + "FlowEnder, FlowEndNode=" + dbStr + "FlowEndNode WHERE OID=" + dbStr + "OID";
            ps.Add("WFState", (int)WFState.ReturnSta);
            ps.Add("FlowEnder", WebUser.No);
            ps.Add("FlowEndNode", ReurnToNode.NodeID);

            ps.Add("OID", this.WorkID);
            DBAccess.RunSQL(ps);

            // Was returned to find people to accept people from the staff list .
            GenerWorkerList gwl = new GenerWorkerList();
            gwl.Retrieve(GenerWorkerListAttr.FK_Node, this.ReurnToNode.NodeID, GenerWorkerListAttr.WorkID, this.WorkID);

            //  Record returned to the track .
            ReturnWork rw = new ReturnWork();
            rw.WorkID = this.WorkID;
            rw.ReturnToNode = this.ReurnToNode.NodeID;
            rw.ReturnNodeName = this.ReurnToNode.Name;

            rw.ReturnNode = this.HisNode.NodeID; //  Current return node .
            rw.ReturnToEmp = gwl.FK_Emp; // Returned to .
            rw.Note = Msg;

            if (this.HisNode.TodolistModel == TodolistModel.Order
                || this.HisNode.TodolistModel == TodolistModel.Sharing
                || this.HisNode.TodolistModel == TodolistModel.Teamup)
            {
                rw.IsBackTracking = true; /* If it is shared , Order , Collaborative model , And must be returned to backtrack .*/

                //  People need to update the status of the current to-do , 把1000 As a special mark , You can find him when sending allowed .
                string sql = "UPDATE WF_GenerWorkerlist SET IsPass=1000 WHERE FK_Node=" + this.HisNode.NodeID + " AND WorkID=" + this.WorkID + " AND FK_Emp='" + WebUser.No + "'";
                if (BP.DA.DBAccess.RunSQL(sql) == 0)
                    throw new Exception("@ Return error , Did not find the target data to be updated . Technical Information :" + sql);
            }
            else
            {
                rw.IsBackTracking = this.IsBackTrack;

                // Delete call GenerWorkerList Data , Otherwise it will lead to garbage data between two nodes , Especially when encountered in the middle of the confluence partakers .
                this.DeleteSpanNodesGenerWorkerListData();
            }

            rw.MyPK = DBAccess.GenerOIDByGUID().ToString();
            rw.Insert();


            //  Join track.
            this.AddToTrack(ActionType.Return, gwl.FK_Emp, gwl.FK_EmpText,
                this.ReurnToNode.NodeID, this.ReurnToNode.Name, Msg);



            try
            {
                //  Log records returned .
                ReorderLog(this.ReurnToNode, this.HisNode, rw);
            }
            catch (Exception ex)
            {
                Log.DebugWriteWarning(ex.Message);
            }

            //  To return to the node forwards data using recursive delete it .
            if (IsBackTrack == false)
            {
                /* If you do not need to backtrack to return , Deleted data intermediate point .*/
#warning  Did not consider two processes data storage mode .
                //DeleteToNodesData(this.ReurnToNode.HisToNodes);
            }

            //  Send a message to him .
            if (Glo.IsEnableSysMessage == true)
            {
                //   WF.Port.WFEmp wfemp = new Port.WFEmp(wnOfBackTo.HisWork.Rec);
                string title = string.Format(this.MsgTitlePattern,
                    this.HisNode.FlowName, this.ReurnToNode.Name, WebUser.Name);

                BP.WF.Dev2Interface.Port_SendMsg(gwl.FK_Emp, title, Msg, "RE" + this.HisNode.NodeID + this.WorkID, BP.WF.SMSMsgType.ToDo, ReurnToNode.FK_Flow, ReurnToNode.NodeID, this.WorkID, this.FID);
            }

            string text = this.HisNode.HisFlow.DoFlowEventEntity(EventListOfNode.ReturnAfter, this.HisNode, this.HisWork,
                atPara);

            if (text != null && text.Length > 1000)
                text = " Return event : No return information .";
            //  Retraction information .
            if (this.ReurnToNode.IsGuestNode)
            {
                GenerWorkFlow gwf = new GenerWorkFlow(this.WorkID);
                return " You have been back to work (" + this.ReurnToNode.Name + "), Returned to (" + gwf.GuestNo + "," + gwf.GuestName + ").\n\r" + text;
            }
            else
            {
                return " You have been back to work (" + this.ReurnToNode.Name + "), Returned to (" + gwl.FK_Emp + "," + gwl.FK_EmpText + ").\n\r" + text;
            }
        }
        /// <summary>
        ///  Increasing the log 
        /// </summary>
        /// <param name="at"> Type </param>
        /// <param name="toEmp"> To staff </param>
        /// <param name="toEmpName"> The person name </param>
        /// <param name="toNDid"> To node </param>
        /// <param name="toNDName"> To the node name </param>
        /// <param name="msg"> News </param>
        public void AddToTrack(ActionType at, string toEmp, string toEmpName, int toNDid, string toNDName, string msg)
        {
            Track t = new Track();
            t.WorkID = this.WorkID;
            t.FK_Flow = this.HisNode.FK_Flow;
            t.FID = this.FID;
            t.RDT = DataType.CurrentDataTimess;
            t.HisActionType = at;

            t.NDFrom = this.HisNode.NodeID;
            t.NDFromT = this.HisNode.Name;

            t.EmpFrom = WebUser.No;
            t.EmpFromT = WebUser.Name;
            t.FK_Flow = this.HisNode.FK_Flow;

            if (toNDid == 0)
            {
                toNDid = this.HisNode.NodeID;
                toNDName = this.HisNode.Name;
            }


            t.NDTo = toNDid;
            t.NDToT = toNDName;

            t.EmpTo = toEmp;
            t.EmpToT = toEmpName;
            t.Msg = msg;
            t.Insert();
        }
        private string infoLog = "";
        private void ReorderLog(Node fromND, Node toND, ReturnWork rw)
        {
            string filePath = BP.Sys.SystemConfig.PathOfDataUser + "\\ReturnLog\\" + this.HisNode.FK_Flow + "\\";
            if (System.IO.Directory.Exists(filePath) == false)
                System.IO.Directory.CreateDirectory(filePath);

            string file = filePath + "\\" + rw.MyPK;
            infoLog = "\r\n Return man :" + WebUser.No + "," + WebUser.Name + " \r\n Return node :" + fromND.Name + " \r\n Return to :" + toND.Name;
            infoLog += "\r\n Return time :" + DataType.CurrentDataTime;
            infoLog += "\r\n The reason :" + rw.Note;

            ReorderLog(fromND, toND);
            DataType.WriteFile(file + ".txt", infoLog);
            DataType.WriteFile(file + ".htm", infoLog.Replace("\r\n", "<br>"));

            // this.HisWork.Delete();
        }
        private void ReorderLog(Node fromND, Node toND)
        {
            /* Began to traverse to reach the set of nodes */
            foreach (Node nd in fromND.HisToNodes)
            {
                Work wk = nd.HisWork;
                wk.OID = this.WorkID;
                if (wk.RetrieveFromDBSources() == 0)
                {
                    wk.FID = this.WorkID;
                    if (wk.Retrieve(WorkAttr.FID, this.WorkID) == 0)
                        continue;
                }

                if (nd.IsFL)
                {
                    /*  If the shunt  */
                    GenerWorkerLists wls = new GenerWorkerLists();
                    QueryObject qo = new QueryObject(wls);
                    qo.AddWhere(GenerWorkerListAttr.FID, this.WorkID);
                    qo.addAnd();

                    string[] ndsStrs = nd.HisToNDs.Split('@');
                    string inStr = "";
                    foreach (string s in ndsStrs)
                    {
                        if (s == "" || s == null)
                            continue;
                        inStr += "'" + s + "',";
                    }
                    inStr = inStr.Substring(0, inStr.Length - 1);
                    if (inStr.Contains(",") == true)
                        qo.AddWhere(GenerWorkerListAttr.FK_Node, int.Parse(inStr));
                    else
                        qo.AddWhereIn(GenerWorkerListAttr.FK_Node, "(" + inStr + ")");

                    qo.DoQuery();
                    foreach (GenerWorkerList wl in wls)
                    {
                        Node subNd = new Node(wl.FK_Node);
                        Work subWK = subNd.GetWork(wl.WorkID);

                        infoLog += "\r\n*****************************************************************************************";
                        infoLog += "\r\n Node ID:" + subNd.NodeID + "   Working Title :" + subWK.EnDesc;
                        infoLog += "\r\n Processors :" + subWK.Rec + " , " + wk.RecOfEmp.Name;
                        infoLog += "\r\n Receive time :" + subWK.RDT + "  Processing time :" + subWK.CDT;
                        infoLog += "\r\n ------------------------------------------------- ";

                        foreach (Attr attr in wk.EnMap.Attrs)
                        {
                            if (attr.UIVisible == false)
                                continue;
                            infoLog += "\r\n " + attr.Desc + ":" + subWK.GetValStrByKey(attr.Key);
                        }

                        // Recursive call .
                        ReorderLog(subNd, toND);
                    }
                }
                else
                {
                    infoLog += "\r\n*****************************************************************************************";
                    infoLog += "\r\n Node ID:" + wk.NodeID + "   Working Title :" + wk.EnDesc;
                    infoLog += "\r\n Processors :" + wk.Rec + " , " + wk.RecOfEmp.Name;
                    infoLog += "\r\n Receive time :" + wk.RDT + "  Processing time :" + wk.CDT;
                    infoLog += "\r\n ------------------------------------------------- ";

                    foreach (Attr attr in wk.EnMap.Attrs)
                    {
                        if (attr.UIVisible == false)
                            continue;
                        infoLog += "\r\n" + attr.Desc + " : " + wk.GetValStrByKey(attr.Key);
                    }
                }

                /*  If the current node  */
                if (nd.NodeID == toND.NodeID)
                    break;

                // Recursive call .
                ReorderLog(nd, toND);
            }
        }
        /// <summary>
        ///  Recursive delete of data between two nodes 
        /// </summary>
        /// <param name="nds"> Set of nodes reachable </param>
        public void DeleteToNodesData(Nodes nds)
        {
            /* Began to traverse to reach the set of nodes */
            foreach (Node nd in nds)
            {
                Work wk = nd.HisWork;
                wk.OID = this.WorkID;
                if (wk.Delete() == 0)
                {
                    wk.FID = this.WorkID;
                    if (wk.Delete(WorkAttr.FID, this.WorkID) == 0)
                        continue;
                }

                #region  Delete the current node data , Remove attachment information .
                //  Delete schedule information .
                MapDtls dtls = new MapDtls("ND" + nd.NodeID);
                foreach (MapDtl dtl in dtls)
                {
                    ps = new Paras();
                    ps.SQL = "DELETE FROM " + dtl.PTable + " WHERE RefPK=" + dbStr + "WorkID";
                    ps.Add("WorkID", this.WorkID.ToString());
                    BP.DA.DBAccess.RunSQL(ps);
                }

                //  Remove attachment Information Form .
                BP.DA.DBAccess.RunSQL("DELETE FROM Sys_FrmAttachmentDB WHERE RefPKVal=" + dbStr + "WorkID AND FK_MapData=" + dbStr + "FK_MapData ",
                    "WorkID", this.WorkID.ToString(), "FK_MapData", "ND" + nd.NodeID);
                //  Delete signature information .
                BP.DA.DBAccess.RunSQL("DELETE FROM Sys_FrmEleDB WHERE RefPKVal=" + dbStr + "WorkID AND FK_MapData=" + dbStr + "FK_MapData ",
                    "WorkID", this.WorkID.ToString(), "FK_MapData", "ND" + nd.NodeID);
                #endregion  Delete the current node data .


                /* Explanation : The node data has been deleted .*/
                DBAccess.RunSQL("DELETE FROM WF_GenerWorkerList WHERE (WorkID=" + dbStr + "WorkID1 OR FID=" + dbStr + "WorkID2 ) AND FK_Node=" + dbStr + "FK_Node",
                    "WorkID1", this.WorkID, "WorkID2", this.WorkID, "FK_Node", nd.NodeID);

                if (nd.IsFL)
                {
                    /*  If the shunt  */
                    GenerWorkerLists wls = new GenerWorkerLists();
                    QueryObject qo = new QueryObject(wls);
                    qo.AddWhere(GenerWorkerListAttr.FID, this.WorkID);
                    qo.addAnd();

                    string[] ndsStrs = nd.HisToNDs.Split('@');
                    string inStr = "";
                    foreach (string s in ndsStrs)
                    {
                        if (s == "" || s == null)
                            continue;
                        inStr += "'" + s + "',";
                    }
                    inStr = inStr.Substring(0, inStr.Length - 1);
                    if (inStr.Contains(",") == true)
                        qo.AddWhere(GenerWorkerListAttr.FK_Node, int.Parse(inStr));
                    else
                        qo.AddWhereIn(GenerWorkerListAttr.FK_Node, "(" + inStr + ")");

                    qo.DoQuery();
                    foreach (GenerWorkerList wl in wls)
                    {
                        Node subNd = new Node(wl.FK_Node);
                        Work subWK = subNd.GetWork(wl.WorkID);
                        subWK.Delete();

                        // Remove the shunt node information next step .
                        DeleteToNodesData(subNd.HisToNodes);
                    }

                    DBAccess.RunSQL("DELETE FROM WF_GenerWorkFlow WHERE FID=" + dbStr + "WorkID",
                        "WorkID", this.WorkID);
                    DBAccess.RunSQL("DELETE FROM WF_GenerWorkerList WHERE FID=" + dbStr + "WorkID",
                        "WorkID", this.WorkID);
                    DBAccess.RunSQL("DELETE FROM WF_GenerFH WHERE FID=" + dbStr + "WorkID",
                        "WorkID", this.WorkID);
                }
                DeleteToNodesData(nd.HisToNodes);
            }
        }
        private WorkNode DoReturnSubFlow(int backtoNodeID, string msg, bool isHiden)
        {
            Node nd = new Node(backtoNodeID);
            ps = new Paras();
            ps.SQL = "DELETE  FROM WF_GenerWorkerList WHERE FK_Node=" + dbStr + "FK_Node AND WorkID=" + dbStr + "WorkID  AND FID=" + dbStr + "FID";
            ps.Add("FK_Node", backtoNodeID);
            ps.Add("WorkID", this.HisWork.OID);
            ps.Add("FID", this.HisWork.FID);
            BP.DA.DBAccess.RunSQL(ps);

            //  Identify sub-confluence point processing personnel .
            ps = new Paras();
            ps.SQL = "SELECT FK_Emp FROM WF_GenerWorkerList WHERE FK_Node=" + dbStr + "FK_Node AND WorkID=" + dbStr + "FID";
            ps.Add("FID", this.HisWork.FID);
            ps.Add("FK_Node", backtoNodeID);
            DataTable dt = DBAccess.RunSQLReturnTable(ps);
            if (dt.Rows.Count != 1)
                throw new Exception("@ system error , this values must be =1");

            string FK_Emp = dt.Rows[0][0].ToString();
            //  Get information about the current job .
            GenerWorkerList wl = new GenerWorkerList(this.HisWork.FID, this.HisNode.NodeID, FK_Emp);
            Emp emp = new Emp(FK_Emp);

            //  Change some of the attributes it to adapt to new data , And display a new work to be done to allow users to see .
            wl.IsPass = false;
            wl.WorkID = this.HisWork.OID;
            wl.FID = this.HisWork.FID;
            wl.RDT = DataType.CurrentDataTime;
            wl.FK_Emp = FK_Emp;
            wl.FK_EmpText = emp.Name;

            wl.FK_Node = backtoNodeID;
            wl.FK_NodeText = nd.Name;
            wl.WarningDays = nd.WarningDays;
            wl.FK_Dept = emp.FK_Dept;

            DateTime dtNew = DateTime.Now;
            dtNew = dtNew.AddDays(nd.WarningDays);
            wl.SDT = dtNew.ToString(DataType.SysDataTimeFormat); // DataType.CurrentDataTime;
            wl.FK_Flow = this.HisNode.FK_Flow;
            wl.Insert();

            GenerWorkFlow gwf = new GenerWorkFlow(this.HisWork.OID);
            gwf.FK_Node = backtoNodeID;
            gwf.NodeName = nd.Name;
            gwf.DirectUpdate();

            ps = new Paras();
            ps.Add("FK_Node", backtoNodeID);
            ps.Add("WorkID", this.HisWork.OID);
            ps.SQL = "UPDATE WF_GenerWorkerList SET IsPass=3 WHERE FK_Node=" + dbStr + "FK_Node AND WorkID=" + dbStr + "WorkID";
            BP.DA.DBAccess.RunSQL(ps);

            /*  If the return is recessive .*/
            BP.WF.ReturnWork rw = new ReturnWork();
            rw.WorkID = wl.WorkID;
            rw.ReturnToNode = wl.FK_Node;
            rw.ReturnNode = this.HisNode.NodeID;
            rw.ReturnNodeName = this.HisNode.Name;
            rw.ReturnToEmp = FK_Emp;
            rw.Note = msg;
            try
            {
                rw.MyPK = rw.ReturnToNode + "_" + rw.WorkID + "_" + DateTime.Now.ToString("yyyyMMddhhmmss");
                rw.Insert();
            }
            catch
            {
                rw.MyPK = rw.ReturnToNode + "_" + rw.WorkID + "_" + BP.DA.DBAccess.GenerOID();
                rw.Insert();
            }

            //  Join track.
            this.AddToTrack(ActionType.Return, FK_Emp, emp.Name, backtoNodeID, nd.Name, msg);

            WorkNode wn = new WorkNode(this.HisWork.FID, backtoNodeID);
            if (Glo.IsEnableSysMessage)
            {
                //  WF.Port.WFEmp wfemp = new Port.WFEmp(wn.HisWork.Rec);
                string title = string.Format(this.MsgTitlePattern,
                      wn.HisNode.FlowName, wn.HisNode.Name, WebUser.Name);

                BP.WF.Dev2Interface.Port_SendMsg(wn.HisWork.Rec, title, msg,
                    "RESub" + backtoNodeID + "_" + this.WorkID, BP.WF.SMSMsgType.ToDo, nd.FK_Flow, nd.NodeID, this.WorkID, this.FID);
            }
            return wn;
        }
    }
}
