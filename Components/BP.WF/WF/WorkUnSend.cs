using System;
using System.Collections;
using System.Data;
using BP.En;
using BP.Web;
using BP.DA;
using BP.Port;
using BP.Sys;
using BP.WF.XML;

namespace BP.WF
{
    /// <summary>
    ///  Send revocation 
    /// </summary>
    public class WorkUnSend
    {
        #region  Property .
        private string _AppType = null;
        /// <summary>
        ///  Virtual directory path 
        /// </summary>
        public string AppType
        {
            get
            {
                if (_AppType == null)
                {
                    if (BP.Sys.SystemConfig.IsBSsystem == false)
                    {
                        _AppType = "WF";
                    }
                    else
                    {
                        if (BP.Web.WebUser.IsWap)
                            _AppType = "WF/WAP";
                        else
                        {
                            bool b = BP.Sys.Glo.Request.RawUrl.ToLower().Contains("oneflow");
                            if (b)
                                _AppType = "WF/OneFlow";
                            else
                                _AppType = "WF";
                        }
                    }
                }
                return _AppType;
            }
        }
        private string _VirPath = null;
        /// <summary>
        ///  Virtual directory path 
        /// </summary>
        public string VirPath
        {
            get
            {
                if (_VirPath == null)
                {
                    if (BP.Sys.SystemConfig.IsBSsystem)
                        _VirPath = Glo.CCFlowAppPath;//BP.Sys.Glo.Request.ApplicationPath;
                    else
                        _VirPath = "";
                }
                return _VirPath;
            }
        }
        public string FlowNo = null;
        private Flow _HisFlow = null;
        public Flow HisFlow
        {
            get
            {
                if (_HisFlow == null)
                    this._HisFlow = new Flow(this.FlowNo);
                return this._HisFlow;
            }
        }
        /// <summary>
        ///  The work ID
        /// </summary>
        public Int64 WorkID = 0;
        /// <summary>
        /// FID
        /// </summary>
        public Int64 FID = 0;
        /// <summary>
        ///  Is River 
        /// </summary>
        public bool IsMainFlow
        {
            get
            {
                if (this.FID != 0 && this.FID != this.WorkID)
                    return false;
                else
                    return true;
            }
        }
        #endregion

        /// <summary>
        ///  Send revocation 
        /// </summary>
        public WorkUnSend(string flowNo, Int64 workID)
        {
            this.FlowNo = flowNo;
            this.WorkID = workID;
        }
        /// <summary>
        ///  Get the current work in progress .
        /// </summary>
        /// <returns></returns>		 
        public WorkNode GetCurrentWorkNode()
        {
            int currNodeID = 0;
            GenerWorkFlow gwf = new GenerWorkFlow(this.WorkID);
            gwf.WorkID = this.WorkID;
            if (gwf.RetrieveFromDBSources() == 0)
            {
                // this.DoFlowOver(ActionType.FlowOver, " Non-normal end , The current process does not find Records .");
                throw new Exception("@" + string.Format(" Workflow {0} Has been completed .", this.WorkID));
            }

            Node nd = new Node(gwf.FK_Node);
            Work work = nd.HisWork;
            work.OID = this.WorkID;
            work.NodeID = nd.NodeID;
            work.SetValByKey("FK_Dept", BP.Web.WebUser.FK_Dept);
            if (work.RetrieveFromDBSources() == 0)
            {
                Log.DefaultLogWriteLineError("@WorkID=" + this.WorkID + ",FK_Node=" + gwf.FK_Node + ". Check it out should not appear to work ."); //  No data found current work node , Unknown exception process .
                work.Rec = BP.Web.WebUser.No;
                try
                {
                    work.Insert();
                }
                catch (Exception ex)
                {
                    Log.DefaultLogWriteLineError("@ No data found current work node , Unknown exception process " + ex.Message + ", Should not appear "); //  No data found current work node 
                }
            }
            work.FID = gwf.FID;
            WorkNode wn = new WorkNode(work, nd);
            return wn;
        }
        /// <summary>
        ///  Undo 
        /// </summary>
        public string DoUnSend()
        {
            GenerWorkFlow gwf = new GenerWorkFlow(this.WorkID);

            //  If the node is a sub-confluent stay .
            Node nd = new Node(gwf.FK_Node);
            if (nd.HisCancelRole == CancelRole.None)
            {
                /* The node is not allowed to return .*/
                throw new Exception(" The current node , Allowed to withdraw .");
            }

            switch (nd.HisNodeWorkType)
            {
                case NodeWorkType.WorkFHL:
                    throw new Exception(" Confluence points allowed to revoke .");
                case NodeWorkType.WorkFL:
                    /* Reached the diversion point ,  There are two cases 1, Untreated . 2, Been processed .
                     *   These two different ways of handling the situation .
                     *   Untreated directly returned through the normal mode .
                     *   Has been treated to kill all processes have been initiated .
                     */
                    DataTable mydt = DBAccess.RunSQLReturnTable("SELECT * FROM WF_GenerWorkerList WHERE FK_Node=" + nd.NodeID + " AND WorkID=" + this.WorkID + "  AND IsPass=1");
                    if (mydt.Rows.Count >= 1)
                        return this.DoUnSendFeiLiu(gwf);
                    break;
                case NodeWorkType.StartWorkFL:
                    return this.DoUnSendFeiLiu(gwf);
                case NodeWorkType.WorkHL:
                    if (this.IsMainFlow)
                    {
                        /*  First, find a diversion with his nearest point , And determine the current operator is not the point of diversion of staff .*/
                        return this.DoUnSendHeiLiu_Main(gwf);
                    }
                    else
                    {
                        return this.DoUnSendSubFlow(gwf); // When is a child process .
                    }
                    break;
                case NodeWorkType.SubThreadWork:
                    break;
                default:
                    break;
            }

            if (nd.IsStartNode)
                throw new Exception(" The current node is the start node , So you can not undo .");

            // Define the current node .
            WorkNode wn = this.GetCurrentWorkNode();

            #region  Seeking revocation of nodes .
            int cancelToNodeID = 0;

            if (nd.HisCancelRole == CancelRole.SpecNodes)
            {
                /* Specified node can be revoked , First, determine whether the current staff has permission .*/
                NodeCancels ncs = new NodeCancels();
                ncs.Retrieve(NodeCancelAttr.FK_Node, wn.HisNode.NodeID);
                if (ncs.Count == 0)
                    throw new Exception("@ Process design errors ,  You set the current node (" + wn.HisNode.Name + ") Who can make the specified node revocation , But you do not set the specified node .");

                /*  Check out . */
                string sql = "SELECT FK_Node FROM WF_GenerWorkerList WHERE FK_Emp='" + WebUser.No + "' AND IsPass=1 AND IsEnable=1 AND WorkID=" + wn.HisWork.OID + " ORDER BY RDT DESC ";
                DataTable dt = DBAccess.RunSQLReturnTable(sql);
                if (dt.Rows.Count == 0)
                    throw new Exception("@ Revocation process error , You do not have permission to perform revocation sent .");

                //  Found to be revoked NodeID.
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (NodeCancel nc in ncs)
                    {
                        if (nc.CancelTo == int.Parse(dr[0].ToString()))
                        {
                            cancelToNodeID = nc.CancelTo;
                            break;
                        }
                    }

                    if (cancelToNodeID != 0)
                        break;
                }

                if (cancelToNodeID == 0)
                    throw new Exception("@ Revocation process error , You do not have permission to perform revocation sent , You can not find a node revocation .");
            }

            if (nd.HisCancelRole == CancelRole.OnlyNextStep)
            {
                /* If only one allowed to undo the last step .*/
                WorkNode wnPri = wn.GetPreviousWorkNode();

                GenerWorkerList wl = new GenerWorkerList();
                int num = wl.Retrieve(GenerWorkerListAttr.FK_Emp, BP.Web.WebUser.No,
                    GenerWorkerListAttr.FK_Node, wnPri.HisNode.NodeID);
                if (num == 0)
                    throw new Exception("@ You can not undo send , Because the current work is not that you send or the next step has been processed .");
                cancelToNodeID = wnPri.HisNode.NodeID;
            }

            if (cancelToNodeID == 0)
                throw new Exception("@ Did not find to be withdrawn to the node .");
            #endregion  Seeking revocation of nodes .

            /**********  Started revocation . **********************/
            Node cancelToNode = new Node(cancelToNodeID);
            WorkNode wnOfCancelTo = new WorkNode(this.WorkID, cancelToNodeID);

            //  Undo Send call before the event .
            string msg = nd.HisFlow.DoFlowEventEntity(EventListOfNode.UndoneBefore, nd, wn.HisWork, null);

            #region  Delete the current node data .
            //  Job Remove generated list .
            GenerWorkerLists wls = new GenerWorkerLists();
            wls.Delete(GenerWorkerListAttr.WorkID, this.WorkID, GenerWorkerListAttr.FK_Node, gwf.FK_Node.ToString());

            //  Delete Job Information , If it is in accordance with the ccflow Format stores .
            if (this.HisFlow.HisDataStoreModel == BP.WF.Template.DataStoreModel.ByCCFlow)
                wn.HisWork.Delete();

            //  Remove attachment information .
            DBAccess.RunSQL("DELETE FROM Sys_FrmAttachmentDB WHERE FK_MapData='ND" + gwf.FK_Node + "' AND RefPKVal='" + this.WorkID + "'");
            #endregion  Delete the current node data .

            //  Update .
            gwf.FK_Node = cancelToNode.NodeID;
            gwf.NodeName = cancelToNode.Name;

            if (cancelToNode.IsEnableTaskPool && Glo.IsEnableTaskPool)
                gwf.TaskSta = TaskSta.Takeback;
            else
                gwf.TaskSta = TaskSta.None;

            gwf.TodoEmps = WebUser.No + "," + WebUser.Name;
            gwf.Update();

            if (cancelToNode.IsEnableTaskPool && Glo.IsEnableTaskPool)
            {
                // Set all of the staff are not available .
                BP.DA.DBAccess.RunSQL("UPDATE WF_GenerWorkerlist SET IsPass=0,  IsEnable=-1 WHERE WorkID=" + this.WorkID + " AND FK_Node=" + gwf.FK_Node);

                // Set the current staff available .
                BP.DA.DBAccess.RunSQL("UPDATE WF_GenerWorkerlist SET IsPass=0,  IsEnable=1  WHERE WorkID=" + this.WorkID + " AND FK_Node=" + gwf.FK_Node + " AND FK_Emp='" + WebUser.No + "'");
            }
            else
            {
                BP.DA.DBAccess.RunSQL("UPDATE WF_GenerWorkerlist SET IsPass=0  WHERE WorkID=" + this.WorkID + " AND FK_Node=" + gwf.FK_Node);
            }

            // Update the current node ,到rpt Inside .
            BP.DA.DBAccess.RunSQL("UPDATE " + this.HisFlow.PTable + " SET FlowEndNode=" + gwf.FK_Node + " WHERE OID=" + this.WorkID);


            //  Logging ..
            wn.AddToTrack(ActionType.UnSend, WebUser.No, WebUser.Name, cancelToNode.NodeID, cancelToNode.Name, "无");

            //  Deleting Data .
            if (wn.HisNode.IsStartNode)
            {
                DBAccess.RunSQL("DELETE FROM WF_GenerFH WHERE FID=" + this.WorkID);
                DBAccess.RunSQL("DELETE FROM WF_GenerWorkFlow WHERE WorkID=" + this.WorkID);
                DBAccess.RunSQL("DELETE FROM WF_GenerWorkerlist WHERE WorkID=" + this.WorkID + " AND FK_Node=" + nd.NodeID);
            }

            if (wn.HisNode.IsEval)
            {
                /* If the node is a quality assessment , And revoked .*/
                DBAccess.RunSQL("DELETE FROM WF_CHEval WHERE FK_Node=" + wn.HisNode.NodeID + " AND WorkID=" + this.WorkID);
            }

            #region  Recovery track , Work to do to solve the grab .
            if (cancelToNode.IsStartNode == false && cancelToNode.IsEnableTaskPool == false)
            {
                WorkNode ppPri = wnOfCancelTo.GetPreviousWorkNode();
                GenerWorkerList wl = new GenerWorkerList();
                wl.Retrieve(GenerWorkerListAttr.FK_Node, wnOfCancelTo.HisNode.NodeID, GenerWorkerListAttr.WorkID, this.WorkID);
                // BP.DA.DBAccess.RunSQL("UPDATE WF_GenerWorkerList SET IsPass=0 WHERE FK_Node=" + backtoNodeID + " AND WorkID=" + this.WorkID);
                RememberMe rm = new RememberMe();
                rm.Retrieve(RememberMeAttr.FK_Node, wnOfCancelTo.HisNode.NodeID, RememberMeAttr.FK_Emp, ppPri.HisWork.Rec);

                string[] empStrs = rm.Objs.Split('@');
                foreach (string s in empStrs)
                {
                    if (s == "" || s == null)
                        continue;

                    if (s == wl.FK_Emp)
                        continue;
                    GenerWorkerList wlN = new GenerWorkerList();
                    wlN.Copy(wl);
                    wlN.FK_Emp = s;

                    WF.Port.WFEmp myEmp = new Port.WFEmp(s);
                    wlN.FK_EmpText = myEmp.Name;

                    wlN.Insert();
                }
            }
            #endregion  Recovery track , Work to do to solve the grab .


            #region  If this is the start node ,  This process checks whether there is a child thread , If you remove them .
            if (nd.IsStartNode)
            {
                /* To check whether there is a   Subprocess , If there , Then remove them .*/
                GenerWorkFlows gwfs = new GenerWorkFlows();
                gwfs.Retrieve(GenerWorkFlowAttr.PWorkID, this.WorkID);
                if (gwfs.Count > 0)
                {
                    foreach (GenerWorkFlow item in gwfs)
                    {
                        /* Delete each child thread .*/
                        BP.WF.Dev2Interface.Flow_DoDeleteFlowByReal(item.FK_Flow, item.WorkID, true);
                    }
                }
            }
            #endregion

            // Undo call after sending the event .
            msg += nd.HisFlow.DoFlowEventEntity(EventListOfNode.UndoneAfter, nd, wn.HisWork, null);

            if (wnOfCancelTo.HisNode.IsStartNode)
            {
                if (BP.Web.WebUser.IsWap)
                {
                    if (wnOfCancelTo.HisNode.HisFormType != NodeFormType.SDKForm)
                        return "@ Undo Send executed successfully , You can click here <a href='" + VirPath + "WF/Wap/MyFlow" + Glo.FromPageType + ".aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/> Implementation </A> , <a href='" + VirPath + "WF/Wap/MyFlowInfo" + Glo.FromPageType + ".aspx?DoType=DeleteFlow&WorkID=" + wn.HisWork.OID + "&FK_Flow=" + this.FlowNo + "' /><img src='" + VirPath + "WF/Img/Btn/Delete.gif' border=0/> This process has been completed ( Remove it )</a>." + msg;
                    else
                        return "@ Revocation of success ." + msg;
                }
                else
                {
                    switch (wnOfCancelTo.HisNode.HisFormType)
                    {
                        case NodeFormType.FixForm:
                        case NodeFormType.FreeForm:
                            return "@ Undo the successful implementation , You can click here <a href='" + this.VirPath + this.AppType + "/MyFlow" + Glo.FromPageType + ".aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/> Implementation </A> , <a href='" + this.VirPath + this.AppType + "/Do.aspx?ActionType=DeleteFlow&WorkID=" + wn.HisWork.OID + "&FK_Flow=" + this.FlowNo + "' /><img src='" + VirPath + "WF/Img/Btn/Delete.gif' border=0/> This process has been completed ( Remove it )</a>." + msg;
                            break;
                        default:
                            return " Revocation of success " + msg;
                            break;
                    }
                }
            }
            else
            {
                //  Updates are displayed .
                //  DBAccess.RunSQL("UPDATE WF_ForwardWork SET IsRead=1 WHERE WORKID=" + this.WorkID + " AND FK_Node=" + cancelToNode.NodeID);
                switch (wnOfCancelTo.HisNode.HisFormType)
                {
                    case NodeFormType.FixForm:
                    case NodeFormType.FreeForm:
                        return "@ Undo the successful implementation , You can click here <a href='" + this.VirPath + this.AppType + "/MyFlow" + Glo.FromPageType + ".aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/> Implementation </A>  . " + msg;
                        break;
                    default:
                        return " Revocation of success :" + msg;
                        break;
                }
            }
            return " Work has been revoked to you :" + cancelToNode.Name;
        }
        /// <summary>
        ///  Undo split point 
        /// </summary>
        /// <param name="gwf"></param>
        /// <returns></returns>
        private string DoUnSendFeiLiu(GenerWorkFlow gwf)
        {
            string sql = "SELECT FK_Node FROM WF_GenerWorkerList WHERE WorkID=" + this.WorkID + " AND FK_Emp='" + BP.Web.WebUser.No + "' AND FK_Node='" + gwf.FK_Node + "'";
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
                return "@ You can not undo send , Because the current work is not that you send .";

            // Handling Events .
            Node nd = new Node(gwf.FK_Node);
            Work wk = nd.HisWork;
            wk.OID = gwf.WorkID;
            wk.RetrieveFromDBSources();

            string msg = nd.HisFlow.DoFlowEventEntity(EventListOfNode.UndoneBefore, nd, wk, null);

            //  Logging ..
            WorkNode wn = new WorkNode(wk, nd);
            wn.AddToTrack(ActionType.UnSend, WebUser.No, WebUser.Name, gwf.FK_Node, gwf.NodeName, "");

            //  Delete sub-confluent record .
            if (nd.IsStartNode)
            {
                DBAccess.RunSQL("DELETE FROM WF_GenerFH WHERE FID=" + this.WorkID);
                DBAccess.RunSQL("DELETE FROM WF_GenerWorkFlow WHERE WorkID=" + this.WorkID);
                DBAccess.RunSQL("DELETE FROM WF_GenerWorkerlist WHERE WorkID=" + this.WorkID + " AND FK_Node=" + nd.NodeID);
                DBAccess.RunSQL("DELETE FROM WF_GenerWorkerlist WHERE FID=" + this.WorkID);
            }

            // Delete data on a node .
            foreach (Node ndNext in nd.HisToNodes)
            {
                int i = DBAccess.RunSQL("DELETE FROM WF_GenerWorkerList WHERE FID=" + this.WorkID + " AND FK_Node=" + ndNext.NodeID);
                if (i == 0)
                    continue;

                //  Delete work record .
                Works wks = ndNext.HisWorks;
                if (this.HisFlow.HisDataStoreModel == BP.WF.Template.DataStoreModel.ByCCFlow)
                    wks.Delete(GenerWorkerListAttr.FID, this.WorkID);

                //  Delete process has been initiated .
                DBAccess.RunSQL("DELETE FROM WF_GenerWorkFlow WHERE FID=" + this.WorkID + " AND FK_Node=" + ndNext.NodeID);
            }

            // Set the current node .
            BP.DA.DBAccess.RunSQL("UPDATE WF_GenerWorkerlist SET IsPass=0 WHERE WorkID=" + this.WorkID + " AND FK_Node=" + gwf.FK_Node + " AND IsPass=1");
            BP.DA.DBAccess.RunSQL("UPDATE WF_GenerFH SET FK_Node=" + gwf.FK_Node + " WHERE FID=" + this.WorkID);

            //  Set state of the current node .
            Node cNode = new Node(gwf.FK_Node);
            Work cWork = cNode.HisWork;
            cWork.OID = this.WorkID;
            msg += nd.HisFlow.DoFlowEventEntity(EventListOfNode.UndoneAfter, nd, wk, null);
            if (cNode.IsStartNode)
            {
                if (BP.Web.WebUser.IsWap)
                {
                    return "@ Undo the successful implementation , You can click here <a href='" + this.VirPath + this.AppType + "/MyFlow" + Glo.FromPageType + ".aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=0&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/> Implementation </A> , <a href='MyFlowInfo" + Glo.FromPageType + ".aspx?DoType=DeleteFlow&WorkID=" + cWork.OID + "&FK_Flow=" + this.FlowNo + "' /><img src='" + VirPath + "WF/Img/Btn/Delete.gif' border=0/> This process has been completed ( Remove it )</a>." + msg;
                }
                else
                {
                    if (this.HisFlow.FK_FlowSort != "00")
                        return "@ Undo the successful implementation , You can click here <a href='" + this.VirPath + this.AppType + "/MyFlow" + Glo.FromPageType + ".aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=0&FK_Node=" + gwf.FK_Node + "' ><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/> Implementation </A> , <a href='MyFlowInfo" + Glo.FromPageType + ".aspx?DoType=DeleteFlow&WorkID=" + cWork.OID + "&FK_Flow=" + this.FlowNo + "' /><img src='" + VirPath + "WF/Img/Btn/Delete.gif' border=0/> This process has been completed ( Remove it )</a>." + msg;
                    else
                        return "@ Undo the successful implementation , You can click here <a href='" + this.VirPath + this.AppType + "/MyFlow" + Glo.FromPageType + ".aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=0&FK_Node=" + gwf.FK_Node + "' ><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/> Implementation </A> , <a href='Do.aspx?ActionType=DeleteFlow&WorkID=" + cWork.OID + "&FK_Flow=" + this.FlowNo + "' /><img src='" + VirPath + "WF/Img/Btn/Delete.gif' border=0/> This process has been completed ( Remove it )</a>." + msg;
                }
            }
            else
            {
                //  Updates are displayed .
                // DBAccess.RunSQL("UPDATE WF_ForwardWork SET IsRead=1 WHERE WORKID=" + this.WorkID + " AND FK_Node=" + cNode.NodeID);
                if (BP.Web.WebUser.IsWap == false)
                {
                    if (this.HisFlow.FK_FlowSort != "00")
                        return "@ Undo the successful implementation , You can click here <a href='" + this.VirPath + this.AppType + "/MyFlow" + Glo.FromPageType + ".aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=0&FK_Node=" + gwf.FK_Node + "' ><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/> Implementation </A>." + msg;
                    else
                        return "@ Undo the successful implementation , You can click here <a href='" + this.VirPath + this.AppType + "/MyFlow" + Glo.FromPageType + ".aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=0&FK_Node=" + gwf.FK_Node + "' ><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/> Implementation </A>." + msg;
                }
                else
                {
                    return "@ Undo the successful implementation , You can click here <a href='" + this.VirPath + this.AppType + "/MyFlow" + Glo.FromPageType + ".aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=0&FK_Node=" + gwf.FK_Node + "' ><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/> Implementation </A>." + msg;
                }
            }
        }
        /// <summary>
        ///  Send perform revocation 
        /// </summary>
        /// <param name="gwf"></param>
        /// <returns></returns>
        public string DoUnSendHeiLiu_Main(GenerWorkFlow gwf)
        {
            Node currNode = new Node(gwf.FK_Node);
            Node priFLNode = currNode.HisPriFLNode;
            GenerWorkerList wl = new GenerWorkerList();
            int i = wl.Retrieve(GenerWorkerListAttr.FK_Node, priFLNode.NodeID, GenerWorkerListAttr.FK_Emp, BP.Web.WebUser.No);
            if (i == 0)
                return "@ Not what you send work to the current node , So you can not undo .";

            WorkNode wn = this.GetCurrentWorkNode();
            WorkNode wnPri = new WorkNode(this.WorkID, priFLNode.NodeID);

            //  Logging ..
            wnPri.AddToTrack(ActionType.UnSend, WebUser.No, WebUser.Name, wnPri.HisNode.NodeID, wnPri.HisNode.Name, "无");

            GenerWorkerLists wls = new GenerWorkerLists();
            wls.Delete(GenerWorkerListAttr.WorkID, this.WorkID, GenerWorkerListAttr.FK_Node, gwf.FK_Node.ToString());

            if (this.HisFlow.HisDataStoreModel == BP.WF.Template.DataStoreModel.ByCCFlow)
                wn.HisWork.Delete();

            gwf.FK_Node = wnPri.HisNode.NodeID;
            gwf.NodeName = wnPri.HisNode.Name;
            gwf.Update();

            BP.DA.DBAccess.RunSQL("UPDATE WF_GenerWorkerlist SET IsPass=0 WHERE WorkID=" + this.WorkID + " AND FK_Node=" + gwf.FK_Node);
            BP.DA.DBAccess.RunSQL("UPDATE WF_GenerFH SET FK_Node=" + gwf.FK_Node + " WHERE FID=" + this.WorkID);

            ShiftWorks fws = new ShiftWorks();
            fws.Delete(ShiftWorkAttr.FK_Node, wn.HisNode.NodeID.ToString(),
                ShiftWorkAttr.WorkID, this.WorkID.ToString());

            #region  Recovery track , Work to do to solve the grab .
            if (wnPri.HisNode.IsStartNode == false)
            {
                WorkNode ppPri = wnPri.GetPreviousWorkNode();
                wl = new GenerWorkerList();
                wl.Retrieve(GenerWorkerListAttr.FK_Node, wnPri.HisNode.NodeID, GenerWorkerListAttr.WorkID, this.WorkID);
                // BP.DA.DBAccess.RunSQL("UPDATE WF_GenerWorkerList SET IsPass=0 WHERE FK_Node=" + backtoNodeID + " AND WorkID=" + this.WorkID);
                RememberMe rm = new RememberMe();
                rm.Retrieve(RememberMeAttr.FK_Node, wnPri.HisNode.NodeID, RememberMeAttr.FK_Emp, ppPri.HisWork.Rec);

                string[] empStrs = rm.Objs.Split('@');
                foreach (string s in empStrs)
                {
                    if (s == "" || s == null)
                        continue;

                    if (s == wl.FK_Emp)
                        continue;
                    GenerWorkerList wlN = new GenerWorkerList();
                    wlN.Copy(wl);
                    wlN.FK_Emp = s;

                    WF.Port.WFEmp myEmp = new Port.WFEmp(s);
                    wlN.FK_EmpText = myEmp.Name;

                    wlN.Insert();
                }
            }
            #endregion  Recovery track , Work to do to solve the grab .

            //  Delete the previous node data .
            wnPri.DeleteToNodesData(priFLNode.HisToNodes);
            if (wnPri.HisNode.IsStartNode)
            {
                if (BP.Web.WebUser.IsWap)
                {
                    if (wnPri.HisNode.HisFormType != NodeFormType.SDKForm)
                        return "@ Undo the successful implementation , You can click here <a href='" + this.VirPath + this.AppType + "/MyFlow" + Glo.FromPageType + ".aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/> Implementation </A> , <a href='" + this.VirPath + this.AppType + "/MyFlowInfo" + Glo.FromPageType + ".aspx?DoType=DeleteFlow&WorkID=" + wn.HisWork.OID + "&FK_Flow=" + this.FlowNo + "' /><img src='" + VirPath + "WF/Img/Btn/Delete.gif' border=0/> This process has been completed ( Remove it )</a>.";
                    else
                        return "@ Revocation of success .";
                }
                else
                {
                    if (wnPri.HisNode.HisFormType != NodeFormType.SDKForm)
                        return "@ Undo the successful implementation , You can click here <a href='" + this.VirPath + this.AppType + "/MyFlow" + Glo.FromPageType + ".aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/> Implementation </A> , <a href='" + this.VirPath + this.AppType + "/MyFlowInfo" + Glo.FromPageType + ".aspx?DoType=DeleteFlow&WorkID=" + wn.HisWork.OID + "&FK_Flow=" + this.FlowNo + "' /><img src='" + VirPath + "WF/Img/Btn/Delete.gif' border=0/> This process has been completed ( Remove it )</a>.";
                    else
                        return "@ Revocation of success .";
                }
            }
            else
            {
                //  Updates are displayed .
#warning
                // DBAccess.RunSQL("UPDATE WF_ForwardWork SET IsRead=1 WHERE WORKID=" + this.WorkID + " AND FK_Node=" + wnPri.HisNode.NodeID);
                if (BP.Web.WebUser.IsWap == false)
                {
                    return "@ Undo the successful implementation , You can click here <a href='" + this.VirPath + this.AppType + "/MyFlow" + Glo.FromPageType + ".aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/> Implementation </A>.";
                }
                else
                {
                    return "@ Undo the successful implementation , You can click here <a href='" + this.VirPath + this.AppType + "/MyFlow" + Glo.FromPageType + ".aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/> Implementation </A>.";
                }
            }
        }
        public string DoUnSendSubFlow(GenerWorkFlow gwf)
        {
            WorkNode wn = this.GetCurrentWorkNode();
            WorkNode wnPri = wn.GetPreviousWorkNode();

            GenerWorkerList wl = new GenerWorkerList();
            int num = wl.Retrieve(GenerWorkerListAttr.FK_Emp, BP.Web.WebUser.No,
                GenerWorkerListAttr.FK_Node, wnPri.HisNode.NodeID);
            if (num == 0)
                return "@ You can not undo send , Because the current work is not that you send .";

            //  Handling Events .
            string msg = wn.HisFlow.DoFlowEventEntity(EventListOfNode.UndoneBefore, wn.HisNode, wn.HisWork, null);

            //  Delete workers .
            GenerWorkerLists wls = new GenerWorkerLists();
            wls.Delete(GenerWorkerListAttr.WorkID, this.WorkID, GenerWorkerListAttr.FK_Node, gwf.FK_Node.ToString());

            if (this.HisFlow.HisDataStoreModel == BP.WF.Template.DataStoreModel.ByCCFlow)
                wn.HisWork.Delete();

            gwf.FK_Node = wnPri.HisNode.NodeID;
            gwf.NodeName = wnPri.HisNode.Name;
            gwf.Update();

            BP.DA.DBAccess.RunSQL("UPDATE WF_GenerWorkerlist SET IsPass=0 WHERE WorkID=" + this.WorkID + " AND FK_Node=" + gwf.FK_Node);
            ShiftWorks fws = new ShiftWorks();
            fws.Delete(ShiftWorkAttr.FK_Node, wn.HisNode.NodeID.ToString(), ShiftWorkAttr.WorkID, this.WorkID.ToString());

            #region  Critical point percentage conditional withdrawal of conditions 
            if (wn.HisNode.PassRate != 0)
            {
                decimal all = (decimal)BP.DA.DBAccess.RunSQLReturnValInt("SELECT COUNT(*) NUM FROM dbo.WF_GenerWorkerList WHERE FID=" + this.FID + " AND FK_Node=" + wnPri.HisNode.NodeID);
                decimal ok = (decimal)BP.DA.DBAccess.RunSQLReturnValInt("SELECT COUNT(*) NUM FROM dbo.WF_GenerWorkerList WHERE FID=" + this.FID + " AND IsPass=1 AND FK_Node=" + wnPri.HisNode.NodeID);
                decimal rate = ok / all * 100;
                if (wn.HisNode.PassRate <= rate)
                    DBAccess.RunSQL("UPDATE WF_GenerWorkerList SET IsPass=0 WHERE FK_Node=" + wn.HisNode.NodeID + " AND WorkID=" + this.FID);
                else
                    DBAccess.RunSQL("UPDATE WF_GenerWorkerList SET IsPass=3 WHERE FK_Node=" + wn.HisNode.NodeID + " AND WorkID=" + this.FID);
            }
            #endregion

            //  Handling Events .
            msg += wn.HisFlow.DoFlowEventEntity(EventListOfNode.UndoneAfter, wn.HisNode, wn.HisWork, null);

            //  Logging ..
            wn.AddToTrack(ActionType.UnSend, WebUser.No, WebUser.Name, wn.HisNode.NodeID, wn.HisNode.Name, "无");

            if (wnPri.HisNode.IsStartNode)
            {
                if (BP.Web.WebUser.IsWap)
                {
                    return "@ Undo the successful implementation , You can click here <a href='" + this.VirPath + this.AppType + "/MyFlow" + Glo.FromPageType + ".aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=" + gwf.FID + "&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/> Implementation </A> , <a href='" + VirPath + "WF/MyFlowInfo" + Glo.FromPageType + ".aspx?DoType=DeleteFlow&WorkID=" + wn.HisWork.OID + "&FK_Flow=" + this.FlowNo + "' /><img src='" + VirPath + "WF/Img/Btn/Delete.gif' border=0/> This process has been completed ( Remove it )</a>." + msg;
                }
                else
                {
                    if (this.HisFlow.FK_FlowSort != "00")
                        return "@ Undo the successful implementation , You can click here <a href='" + this.VirPath + this.AppType + "/MyFlow" + Glo.FromPageType + ".aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=" + gwf.FID + "&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/> Implementation </A> , <a href='" + VirPath + "WF/MyFlowInfo" + Glo.FromPageType + ".aspx?DoType=DeleteFlow&WorkID=" + wn.HisWork.OID + "&FK_Flow=" + this.FlowNo + "' /><img src='" + VirPath + "WF/Img/Btn/Delete.gif' border=0/> This process has been completed ( Remove it )</a>." + msg;
                    else
                        return "@ Undo the successful implementation , You can click here <a href='" + this.VirPath + this.AppType + "/MyFlow" + Glo.FromPageType + ".aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=" + gwf.FID + "&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/> Implementation </A> , <a href='" + VirPath + "WF/Do.aspx?ActionType=DeleteFlow&WorkID=" + wn.HisWork.OID + "&FK_Flow=" + this.FlowNo + "' /><img src='" + VirPath + "WF/Img/Btn/Delete.gif' border=0/> This process has been completed ( Remove it )</a>." + msg;
                }
            }
            else
            {
                //  Updates are displayed .
                //  DBAccess.RunSQL("UPDATE WF_ForwardWork SET IsRead=1 WHERE WORKID=" + this.WorkID + " AND FK_Node=" + wnPri.HisNode.NodeID);
                if (BP.Web.WebUser.IsWap == false)
                {
                    if (this.HisFlow.FK_FlowSort != "00")
                        return "@ Undo the successful implementation , You can click here <a href='" + this.VirPath + this.AppType + "/MyFlow" + Glo.FromPageType + ".aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=" + gwf.FID + "&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/> Implementation </A>." + msg;
                    else
                        return "@ Undo the successful implementation , You can click here <a href='" + this.VirPath + this.AppType + "/MyFlow" + Glo.FromPageType + ".aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=" + gwf.FID + "&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/> Implementation </A>." + msg;
                }
                else
                {
                    return "@ Undo the successful implementation , You can click here <a href='" + this.VirPath + this.AppType + "/MyFlow" + Glo.FromPageType + ".aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=" + gwf.FID + "&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/> Implementation </A>." + msg;
                }
            }
        }
    }
}
