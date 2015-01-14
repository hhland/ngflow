using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using BP.WF;
using BP.DA;
using BP.Port;
using BP.Web;
using BP.En;
using BP.WF.Template;
using BP.WF.Data;
using BP.Sys;

namespace BP.WF
{
    /// <summary>
    ///  This interface for programmers to use the secondary development , In reading the code before Note the following .
    /// 1, CCFlow The external interface is static methods to achieve .
    /// 2, 以 DB_  The beginning of the need to return a result set of interface .
    /// 3, 以 Flow_  Is the process interface .
    /// 4, 以 Node_  Node interface .
    /// 5, 以 Port_  Is the organization interface .
    /// 6, 以 DTS_  Is scheduling ． 
    /// 7, 以 UI_  Is a function of the process window 
    /// 8, 以 WorkOpt_  Processor interface associated with the work .
    /// </summary>
    public class Dev2Interface
    {
        #region  Write messages table .
        /// <summary>
        ///  Write messages 
        ///  Uses can handle reminders .
        /// </summary>
        /// <param name="sendToUserNo"> Send an operator to the ID</param>
        /// <param name="sendDT"> Transmission time , In case null  Said sent immediately .</param>
        /// <param name="title"> Title </param>
        /// <param name="doc"> Content </param>
        /// <param name="msgFlag"> Message mark </param>
        /// <returns> Write to the success or failure .</returns>
        public static bool WriteToSMS(string sendToUserNo, string sendDT, string title, string doc, string msgFlag)
        {
            SMS.SendMsg(sendToUserNo, title, doc, msgFlag, "Info");
            return true;
        }
        #endregion

        #region  The number of messages waiting to go to treatment .
        /// <summary>
        ///  Upcoming number of jobs 
        /// </summary>
        public static int Todolist_EmpWorks
        {
            get
            {
                Paras ps = new Paras();
                string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
                string wfSql = "  WFState=" + (int)WFState.Askfor + " OR WFState=" + (int)WFState.Runing + "  OR WFState=" + (int)WFState.AskForReplay + " OR WFState=" + (int)WFState.Shift + " OR WFState=" + (int)WFState.ReturnSta + " OR WFState=" + (int)WFState.Fix;
                string sql;

                if (WebUser.IsAuthorize == false)
                {
                    /* Not authorize state */
                    if (BP.WF.Glo.IsEnableTaskPool == true)
                        ps.SQL = "SELECT count(*) as Num FROM WF_EmpWorks WHERE (" + wfSql + ") AND FK_Emp=" + dbstr + "FK_Emp AND TaskSta!=1 ";
                    else
                        ps.SQL = "SELECT count(*) as Num FROM WF_EmpWorks WHERE (" + wfSql + ") AND FK_Emp=" + dbstr + "FK_Emp ";

                    ps.Add("FK_Emp", BP.Web.WebUser.No);
                    return BP.DA.DBAccess.RunSQLReturnValInt(ps);
                }

                /* If you are an authorized state ,  Get information about the current principal . */
                WF.Port.WFEmp emp = new Port.WFEmp(WebUser.No);
                switch (emp.HisAuthorWay)
                {
                    case Port.AuthorWay.All:
                        if (BP.WF.Glo.IsEnableTaskPool == true)
                            ps.SQL = "SELECT count(*) as Num FROM WF_EmpWorks WHERE (" + wfSql + ") AND FK_Emp=" + dbstr + "FK_Emp AND TaskSta!=0  ";
                        else
                            ps.SQL = "SELECT count(*) as Num FROM WF_EmpWorks WHERE (" + wfSql + ") AND FK_Emp=" + dbstr + "FK_Emp ";
                        ps.Add("FK_Emp", BP.Web.WebUser.No);
                        break;
                    case Port.AuthorWay.SpecFlows:
                        if (BP.WF.Glo.IsEnableTaskPool == true)
                            ps.SQL = "SELECT count(*) as Num FROM WF_EmpWorks WHERE (" + wfSql + ") AND FK_Emp=" + dbstr + "FK_Emp AND  FK_Flow IN " + emp.AuthorFlows + " AND TaskSta!=0   ";
                        else
                            ps.SQL = "SELECT count(*) as Num FROM WF_EmpWorks WHERE (" + wfSql + ") AND FK_Emp=" + dbstr + "FK_Emp AND  FK_Flow IN " + emp.AuthorFlows;

                        ps.Add("FK_Emp", BP.Web.WebUser.No);
                        break;
                    case Port.AuthorWay.None:
                        throw new Exception(" Other side (" + WebUser.No + ") Authorization has been canceled .");
                    default:
                        throw new Exception("no such way...");
                }
                return BP.DA.DBAccess.RunSQLReturnValInt(ps);
            }
        }
        /// <summary>
        ///  CC number 
        /// </summary>
        public static int Todolist_CCWorks
        {
            get
            {
                Paras ps = new Paras();
                ps.SQL = "SELECT count(*) as Num FROM WF_CCList WHERE CCTo=" + SystemConfig.AppCenterDBVarStr + "FK_Emp AND Sta=0";
                ps.Add("FK_Emp", BP.Web.WebUser.No);
                return DBAccess.RunSQLReturnValInt(ps, 0);
            }
        }
        /// <summary>
        ///  Get CC staff 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string GetNode_CCList(WorkNode node)
        {
            string ccers = null;

            if (node.HisNode.HisCCRole == CCRole.AutoCC
                   || node.HisNode.HisCCRole == CCRole.HandAndAuto)
            {
                try
                {
                    /* If the automatic CC */
                    CC cc = node.HisNode.HisCC;

                    DataTable table = cc.GenerCCers(node.rptGe);
                    if (table.Rows.Count > 0)
                    {
                        string ccMsg = "@ Messages automatically copied to ";

                        foreach (DataRow dr in table.Rows)
                        {
                            ccers += dr[0] + ",";
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("@ An error occurred while processing operations sent :" + ex.Message);
                }
            }

            #region   Execution Cc  BySysCCEmps
            if (node.HisNode.HisCCRole == CCRole.BySysCCEmps)
            {
                CC cc = node.HisNode.HisCC;

                // Remove the Cc list 
                string temps = node.rptGe.GetValStrByKey("SysCCEmps");
                if (!string.IsNullOrEmpty(temps))
                {
                    string[] cclist = temps.Split('|');
                    Hashtable ht = new Hashtable();
                    foreach (string item in cclist)
                    {
                        string[] tmp = item.Split(',');
                        ccers += tmp[0] + ",";
                    }
                }
            }
            #endregion

            return ccers;
        }
        /// <summary>
        ///  Returns the number of processes to hang 
        /// </summary>
        public static int Todolist_HungUpNum
        {
            get
            {
                string sql = "select  COUNT(*) AS Num from WF_GenerWorkFlow where WFState=4 and  WorkID in (SELECT distinct WorkID FROM WF_HungUp WHERE Rec='" + BP.Web.WebUser.No + "')";
                return BP.DA.DBAccess.RunSQLReturnValInt(sql);
            }
        }
        /// <summary>
        ///  The number of jobs in the way of 
        /// </summary>
        public static int Todolist_Runing
        {
            get
            {
                string sql;
                int state = (int)WFState.Runing;
                if (WebUser.IsAuthorize)
                {
                    /* If you are an authorized state .*/
                    WF.Port.WFEmp emp = new Port.WFEmp(WebUser.No);
                    sql = "SELECT count( distinct a.WorkID ) as Num FROM WF_GenerWorkFlow A, WF_GenerWorkerlist B WHERE A.WorkID=B.WorkID AND B.FK_Emp='" + WebUser.No + "' AND B.IsEnable=1 AND B.IsPass=1 AND A.FK_Flow IN " + emp.AuthorFlows;
                    return BP.DA.DBAccess.RunSQLReturnValInt(sql);
                }
                else
                {
                    Paras ps = new Paras();
                    ps.SQL = "SELECT count( distinct a.WorkID ) as Num FROM WF_GenerWorkFlow A, WF_GenerWorkerlist B WHERE A.WorkID=B.WorkID AND B.FK_Emp=" + SystemConfig.AppCenterDBVarStr + "FK_Emp AND B.IsEnable=1 AND B.IsPass=1 ";
                    ps.Add("FK_Emp", WebUser.No);
                    return BP.DA.DBAccess.RunSQLReturnValInt(ps);
                }
            }
        }
        /// <summary>
        ///  Get the number of processes have been completed 
        /// </summary>
        /// <returns></returns>
        public static int Todolist_Complete
        {
            get
            {
                if (Glo.IsDeleteGenerWorkFlow == false)
                {
                    /*  If not, delete the registry process . */
                    Paras ps = new Paras();
                    string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
                    ps.SQL = "SELECT count(*) Num FROM WF_GenerWorkFlow WHERE Emps LIKE '%@" + WebUser.No + "@%' AND FID=0 AND WFState=" + (int)WFState.Complete;
                    return BP.DA.DBAccess.RunSQLReturnValInt(ps, 0);
                }
                else
                {
                    Paras ps = new Paras();
                    string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
                    ps.SQL = "SELECT count(*) Num FROM V_FlowData WHERE FlowEmps LIKE '%@" + WebUser.No + "%' AND FID=0 AND WFState=" + (int)WFState.Complete;
                    return BP.DA.DBAccess.RunSQLReturnValInt(ps, 0);
                }
            }
        }
        #endregion  The number of messages waiting to go to treatment .

        #region  Automatic execution 
        /// <summary>
        ///  Processing tasks postponed . Depending on the setting node attributes 
        /// </summary>
        /// <returns> Return message processing </returns>
        public static string DTS_DealDeferredWork()
        {
            //string sql = "SELECT * FROM WF_EmpWorks WHERE FK_Node IN (SELECT NodeID FROM WF_Node WHERE OutTimeDeal >0 ) AND SDT <='" + DataType.CurrentData + "' ORDER BY FK_Emp";
            // Into a less-than sign SDT <'" + DataType.CurrentData
            string sql = "SELECT * FROM WF_EmpWorks WHERE FK_Node IN (SELECT NodeID FROM WF_Node WHERE OutTimeDeal >0 ) AND SDT <'" + DataType.CurrentData + "' ORDER BY FK_Emp";
            //string sql = "SELECT * FROM WF_EmpWorks WHERE FK_Node IN (SELECT NodeID FROM WF_Node WHERE OutTimeDeal >0 ) AND SDT <='2013-12-30' ORDER BY FK_Emp";
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            string msg = "";
            string dealWorkIDs = "";
            foreach (DataRow dr in dt.Rows)
            {
                string FK_Emp = dr["FK_Emp"].ToString();
                string fk_flow = dr["FK_Flow"].ToString();
                int fk_node = int.Parse(dr["FK_Node"].ToString());
                Int64 workid = Int64.Parse(dr["WorkID"].ToString());
                Int64 fid = Int64.Parse(dr["FID"].ToString());

                //  Way two people to handle a job ,  After one treatment , Another case of a person can also handle .
                if (dealWorkIDs.Contains("," + workid + ","))
                    continue;
                dealWorkIDs += "," + workid + ",";

                if (WebUser.No != FK_Emp)
                {
                    Emp emp = new Emp(FK_Emp);
                    BP.Web.WebUser.SignInOfGener(emp);
                }

                BP.WF.Template.Ext.NodeSheet nd = new BP.WF.Template.Ext.NodeSheet();
                nd.NodeID = fk_node;
                nd.Retrieve();

                //  First determines whether there is a startup expression ,  It is always the valve is automatically executed .
                if (string.IsNullOrEmpty(nd.DoOutTimeCond) == false)
                {
                    Node nodeN = new Node(nd.NodeID);
                    Work wk = nodeN.HisWork;
                    wk.OID = workid;
                    wk.Retrieve();
                    string exp = nd.DoOutTimeCond.Clone() as string;
                    if (Glo.ExeExp(exp, wk) == false)
                        continue; //  Not by the conditions set .
                }

                switch (nd.HisOutTimeDeal)
                {
                    case OutTimeDeal.None:
                        break;
                    case OutTimeDeal.AutoTurntoNextStep: // Automatically go to the next step .
                        if (string.IsNullOrEmpty(nd.DoOutTime))
                        {
                            /* If it is empty , No specific point allows , Let other execution down .*/
                            msg += BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid).ToMsgOfText();
                        }
                        else
                        {
                            int nextNode = Dev2Interface.Node_GetNextStepNode(fk_flow, workid);
                            if (nd.DoOutTime.Contains(nextNode.ToString())) /* If you include the current point ID, Just let it go on execution .*/
                                msg += BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid).ToMsgOfText();
                        }
                        break;
                    case OutTimeDeal.AutoJumpToSpecNode: // Automatically jump to the next node .
                        if (string.IsNullOrEmpty(nd.DoOutTime))
                            throw new Exception("@ Setting error , Is not set to jump to the next node .");
                        int nextNodeID = int.Parse(nd.DoOutTime);
                        msg += BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid, null, null, nextNodeID, null).ToMsgOfText();
                        break;
                    case OutTimeDeal.AutoShiftToSpecUser: // Handed over to the designated person .
                        msg += BP.WF.Dev2Interface.Node_Shift(fk_flow, fk_node, workid, fid, nd.DoOutTime, " Come from ccflow Automatic Message :(" + BP.Web.WebUser.Name + ") Work fails to deal with (" + nd.Name + "), Now handed over to you .");
                        break;
                    case OutTimeDeal.SendMsgToSpecUser: // Message to the designated staff .
                        BP.WF.Dev2Interface.Port_SendMsg(nd.DoOutTime,
                            " Come from ccflow Automatic Message :(" + BP.Web.WebUser.Name + ") Work fails to deal with (" + nd.Name + ")",
                            " Thank you for choosing ccflow.", "SpecEmp" + workid);
                        break;
                    case OutTimeDeal.DeleteFlow: // Delete Process .
                        msg += BP.WF.Dev2Interface.Flow_DoDeleteFlowByReal(fk_flow, workid, true);
                        break;
                    case OutTimeDeal.RunSQL:
                        msg += BP.DA.DBAccess.RunSQL(nd.DoOutTime);
                        break;
                    default:
                        throw new Exception("@ Timeout handling errors no judgment ." + nd.HisOutTimeDeal);
                }
            }
            Emp emp1 = new Emp("admin");
            BP.Web.WebUser.SignInOfGener(emp1);
            return msg;
        }
        /// <summary>
        ///  Performed automatically start node data 
        ///  Explanation : According to the process of setting the automatic execution , Automatically start the process initiated .
        ///  Such as : According to you ccflow Automatically start the process of setting , The process starts automatically , Do not use ccflow Service program provided , You need to do the following steps .
        /// 1,  Write an automated scheduling program .
        /// 2, According to their own time to call this interface .
        /// </summary>
        /// <param name="fl"> Process Entity , You can  new Flow(flowNo);  To pass .</param>
        public static void DTS_AutoStarterFlow(Flow fl)
        {
            #region  Read data .
            BP.Sys.MapExt me = new BP.Sys.MapExt();
            int i = me.Retrieve(MapExtAttr.FK_MapData, "ND" + int.Parse(fl.No) + "01",
                MapExtAttr.ExtType, "PageLoadFull");
            if (i == 0)
            {
                BP.DA.Log.DefaultLogWriteLineError(" No for the process (" + fl.Name + ") The start node is set to initiate data , Please refer to the instructions to solve .");
                return;
            }

            //  Data obtained from the table .
            DataSet ds = new DataSet();
            string[] dtlSQLs = me.Tag1.Split('*');
            foreach (string sql in dtlSQLs)
            {
                if (string.IsNullOrEmpty(sql))
                    continue;

                string[] tempStrs = sql.Split('=');
                string dtlName = tempStrs[0];
                DataTable dtlTable = BP.DA.DBAccess.RunSQLReturnTable(sql.Replace(dtlName + "=", ""));
                dtlTable.TableName = dtlName;
                ds.Tables.Add(dtlTable);
            }
            #endregion  Read data .

            #region  Check the data source is correct .
            string errMsg = "";
            //  Get the main table data .
            DataTable dtMain = BP.DA.DBAccess.RunSQLReturnTable(me.Tag);
            if (dtMain.Columns.Contains("Starter") == false)
                errMsg += "@ The main table with a value of no Starter列.";

            if (dtMain.Columns.Contains("MainPK") == false)
                errMsg += "@ The main table with a value of no MainPK列.";

            if (errMsg.Length > 2)
            {
                BP.DA.Log.DefaultLogWriteLineError(" Process (" + fl.Name + ") The start node is set to initiate data , Incomplete ." + errMsg);
                return;
            }
            #endregion  Check the data source is correct .

            #region  Processing launched .

            string nodeTable = "ND" + int.Parse(fl.No) + "01";
            MapData md = new MapData(nodeTable);

            foreach (DataRow dr in dtMain.Rows)
            {
                string mainPK = dr["MainPK"].ToString();
                string sql = "SELECT OID FROM " + md.PTable + " WHERE MainPK='" + mainPK + "'";
                if (DBAccess.RunSQLReturnTable(sql).Rows.Count != 0)
                    continue; /* Description been scheduled over */

                string starter = dr["Starter"].ToString();
                if (BP.Web.WebUser.No != starter)
                {
                    BP.Web.WebUser.Exit();
                    BP.Port.Emp emp = new BP.Port.Emp();
                    emp.No = starter;
                    if (emp.RetrieveFromDBSources() == 0)
                    {
                        BP.DA.Log.DefaultLogWriteLineInfo("@ Data-driven approach to initiate the process (" + fl.Name + ") Sponsored personnel set :" + emp.No + " Does not exist .");
                        continue;
                    }

                    BP.Web.WebUser.SignInOfGener(emp);
                }

                #region   To value .
                Work wk = fl.NewWork();
                foreach (DataColumn dc in dtMain.Columns)
                    wk.SetValByKey(dc.ColumnName, dr[dc.ColumnName].ToString());

                if (ds.Tables.Count != 0)
                {
                    string refPK = dr["MainPK"].ToString();
                    MapDtls dtls = wk.HisNode.MapData.MapDtls; // new MapDtls(nodeTable);
                    foreach (MapDtl dtl in dtls)
                    {
                        foreach (DataTable dt in ds.Tables)
                        {
                            if (dt.TableName != dtl.No)
                                continue;

                            // Delete the original data .
                            GEDtl dtlEn = dtl.HisGEDtl;
                            dtlEn.Delete(GEDtlAttr.RefPK, wk.OID.ToString());

                            //  Executing data insertion .
                            foreach (DataRow drDtl in dt.Rows)
                            {
                                if (drDtl["RefMainPK"].ToString() != refPK)
                                    continue;

                                dtlEn = dtl.HisGEDtl;

                                foreach (DataColumn dc in dt.Columns)
                                    dtlEn.SetValByKey(dc.ColumnName, drDtl[dc.ColumnName].ToString());

                                dtlEn.RefPK = wk.OID.ToString();
                                dtlEn.Insert();
                            }
                        }
                    }
                }
                #endregion   To value .

                //  Send information processing .
                Node nd = fl.HisStartNode;
                try
                {
                    WorkNode wn = new WorkNode(wk, nd);
                    string msg = wn.NodeSend().ToMsgOfHtml();
                    //BP.DA.Log.DefaultLogWriteLineInfo(msg);
                }
                catch (Exception ex)
                {
                    BP.DA.Log.DefaultLogWriteLineWarning(ex.Message);
                }
            }
            #endregion  Processing launched .

        }
        #endregion

        #region  Data collection interfaces ( Interface if you want to get a result set , Are based on DB_ The beginning of the .)
        /// <summary>
        ///  Who can initiate the process to obtain 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <returns></returns>
        public static string GetFlowStarters(string fk_flow)
        {
            BP.WF.Node nd = new Node(int.Parse(fk_flow + "01"));
            string sql = "";
            switch (nd.HisDeliveryWay)
            {
                case DeliveryWay.ByBindEmp: /* By staff */
                    sql = "SELECT * FROM Port_Emp WHERE No IN (SELECT FK_Emp FROM WF_NodeEmp WHERE FK_Node=" + nd.NodeID + ")";
                    break;
                case DeliveryWay.ByDept: /* By sector */
                    sql = "SELECT * FROM Port_Emp WHERE FK_Dept IN (SELECT FK_Dept FROM WF_NodeDept WHERE FK_Node=" + nd.NodeID + ")";
                    break;
                case DeliveryWay.ByStation: /* By post */
                    sql = "SELECT * FROM Port_Emp WHERE No IN (SELECT FK_Emp FROM Port_EmpStation WHERE FK_Station IN ( SELECT FK_Station from WF_nodeStation where FK_Node=" + nd.NodeID + ")) ";
                    break;
                default:
                    throw new Exception("@ Personnel access rules start node error , Node settings do not allow this access type at the beginning :" + nd.HisDeliveryWay);
                    break;
            }
            return sql;
        }
        public static string GetFlowStarters(string fk_flow, string fk_dept)
        {
            BP.WF.Node nd = new Node(int.Parse(fk_flow + "01"));
            string sql = "";
            switch (nd.HisDeliveryWay)
            {
                case DeliveryWay.ByBindEmp: /* By staff */
                    sql = "SELECT * FROM Port_Emp WHERE No IN (SELECT FK_Emp FROM WF_NodeEmp WHERE FK_Node=" + nd.NodeID + ") and fk_dept='" + fk_dept + "'";
                    break;
                case DeliveryWay.ByDept: /* By sector */
                    sql = "SELECT * FROM Port_Emp WHERE FK_Dept IN (SELECT FK_Dept FROM WF_NodeDept WHERE FK_Node=" + nd.NodeID + ") and fk_dept='" + fk_dept + "' ";
                    break;
                case DeliveryWay.ByStation: /* By post */
                    sql = "SELECT * FROM Port_Emp WHERE No IN (SELECT FK_Emp FROM Port_EmpStation WHERE FK_Station IN ( SELECT FK_Station from WF_nodeStation where FK_Node=" + nd.NodeID + ")) and fk_dept='" + fk_dept + "' ";
                    break;
                default:
                    throw new Exception("@ Personnel access rules start node error , Node settings do not allow this access type at the beginning :" + nd.HisDeliveryWay);
                    break;
            }
            return sql;
        }

        #region  Associated with the sub-processes .
        /// <summary>
        ///  Get the process instances running track data .
        ///  Explanation : These data can be generated using the process operation log .
        /// </summary>
        /// <param name="workid"> The work ID</param>
        /// <returns>GenerWorkFlows</returns>
        public static GenerWorkFlows DB_SubFlows(Int64 workid)
        {
            GenerWorkFlows gwf = new GenerWorkFlows();
            gwf.Retrieve(GenerWorkFlowAttr.PWorkID, workid);
            return gwf;
        }
        #endregion  Get process instances of trajectories 

        #region  Get process instances of trajectories 
        /// <summary>
        ///  Get the process instances running track data .
        ///  Explanation : These data can be generated using the process operation log .
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="workid"> The work ID</param>
        /// <param name="fid"> Process ID</param>
        /// <returns> Get the process track and trace data from the temporary table table .</returns>
        public static DataSet DB_GenerTrack(string fk_flow, Int64 workid, Int64 fid)
        {
            // Definition of variables , The data are placed in the track Inside .
            DataSet ds = new DataSet();

            #region  Get track Data .
            string sqlOfWhere2 = "";
            string sqlOfWhere1 = "";
            string dbStr = BP.Sys.SystemConfig.AppCenterDBVarStr;
            Paras ps = new Paras();
            if (fid == 0)
            {
                sqlOfWhere1 = " WHERE (FID=" + dbStr + "WorkID11 OR WorkID=" + dbStr + "WorkID12 )  ";
                ps.Add("WorkID11", workid);
                ps.Add("WorkID12", workid);
            }
            else
            {
                sqlOfWhere1 = " WHERE (FID=" + dbStr + "FID11 OR WorkID=" + dbStr + "FID12 ) ";
                ps.Add("FID11", fid);
                ps.Add("FID12", fid);
            }

            string sql = "";
            sql = "SELECT MyPK,ActionType,ActionTypeText,FID,WorkID,NDFrom,NDFromT,NDTo,NDToT,EmpFrom,EmpFromT,EmpTo,EmpToT,RDT,WorkTimeSpan,Msg,NodeData,Exer,Tag FROM ND" + int.Parse(fk_flow) + "Track " + sqlOfWhere1 + " ORDER BY RDT asc";
            ps.SQL = sql;
            DataTable dt = null;
            try
            {
                dt = DBAccess.RunSQLReturnTable(ps);
            }
            catch
            {
                //  Deal with track表.
                Track.CreateOrRepairTrackTable(fk_flow);
                dt = DBAccess.RunSQLReturnTable(ps);
            }

            //把track Join to go inside .
            dt.TableName = "Track";
            ds.Tables.Add(dt);
            #endregion  Get track Data .

            // Join the CC .
            CCLists ens = new CCLists(fk_flow, workid, fid);
            ds.Tables.Add(ens.ToDataTableField("WF_CCList"));

            // The person next node selection information is written inside .
            Int64 wfid = 0;
            if (fid != 0)
                wfid = fid;
            SelectAccpers accepts = new SelectAccpers(wfid);
            ds.Tables.Add(ens.ToDataTableField("WF_SelectAccper"));


            // The node information is written inside .
            sql = "SELECT * FROM WF_Node WHERE FK_Flow='" + fk_flow + "'";
            DataTable dtNode = BP.DA.DBAccess.RunSQLReturnTable(sql);
            dtNode.TableName = "WF_Node";
            ds.Tables.Add(dtNode);

            // Written inside direction .
            sql = "SELECT * FROM WF_Direction WHERE FK_Flow='" + fk_flow + "'";
            DataTable dtDir = BP.DA.DBAccess.RunSQLReturnTable(sql);
            dtDir.TableName = "WF_Direction";
            ds.Tables.Add(dtDir);

            return ds;
        }

        /// <summary>
        ///  Get a process 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="userNo"> Operator number </param>
        /// <returns></returns>
        public static DataTable DB_GenerNDxxxRpt(string fk_flow, string userNo)
        {
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);
            string dbstr = SystemConfig.AppCenterDBVarStr;
            Paras ps = new Paras();
            ps.SQL = "SELECT * FROM ND" + int.Parse(fk_flow) + "Rpt WHERE FlowStarter=" + dbstr + "FlowStarter  ORDER BY RDT";
            ps.Add(GERptAttr.FlowStarter, userNo);
            return DBAccess.RunSQLReturnTable(ps);

        }
        #endregion  Get process instances of trajectories 

        #region  CC staff can send or get a list of .
        /// <summary>
        ///  CC staff can obtain a list of the current node 
        /// </summary>
        /// <param name="fk_node"></param>
        /// <param name="workid"></param>
        /// <returns></returns>
        public static DataTable DB_CanCCEmps(int fk_node, Int64 workid)
        {
            DataTable table = new DataTable();
            string ccers = null;

            WorkNode node = new WorkNode(workid, fk_node);

            if (node.HisNode.HisCCRole == CCRole.AutoCC
                   || node.HisNode.HisCCRole == CCRole.HandAndAuto)
            {
                try
                {
                    /* If the automatic CC */
                    CC cc = node.HisNode.HisCC;

                    table = cc.GenerCCers(node.rptGe);


                }
                catch (Exception ex)
                {
                    throw new Exception("@ An error occurred while processing operations sent :" + ex.Message);
                }
            }

            #region   Execution Cc  BySysCCEmps
            if (node.HisNode.HisCCRole == CCRole.BySysCCEmps)
            {
                CC cc = node.HisNode.HisCC;

                // Remove the Cc list 
                string temps = node.rptGe.GetValStrByKey("SysCCEmps");
                if (!string.IsNullOrEmpty(temps))
                {
                    string[] cclist = temps.Split('|');

                    if (!table.Columns.Contains("No"))
                        table.Columns.Add("No");
                    if (!table.Columns.Contains("Name"))
                        table.Columns.Add("Name");
                    foreach (string item in cclist)
                    {
                        string[] tmp = item.Split(',');

                        DataRow row = table.NewRow();

                        row["No"] = tmp[0];
                        row["Name"] = tmp[1];
                        table.Rows.Add(row);
                    }
                }
            }
            #endregion



            return table;
        }
        /// <summary>
        ///  Get a list of people you can perform the specified node 
        /// </summary>
        /// <param name="fk_node"> Node number </param>
        /// <param name="workid"> The work ID</param>
        /// <returns></returns>
        public static DataSet DB_CanExecSpecNodeEmps(int fk_node, Int64 workid)
        {
            DataSet ds = new DataSet();
            Paras ps = new Paras();
            ps.SQL = "SELECT No,Name,FK_Dept FROM Port_Emp ";
            DataTable dtEmp = DBAccess.RunSQLReturnTable(ps);
            dtEmp.TableName = "Emps";
            ds.Tables.Add(dtEmp);


            ps = new Paras();
            ps.SQL = "SELECT No,Name FROM Port_Dept ";
            DataTable dtDept = DBAccess.RunSQLReturnTable(ps);
            dtDept.TableName = "Depts";
            ds.Tables.Add(dtDept);

            return ds;
        }
        /// <summary>
        ///  You can get a list of personnel cc 
        /// </summary>
        /// <param name="fk_node"> Node number </param>
        /// <param name="workid"> The work ID</param>
        /// <returns></returns>
        public static DataSet DB_CanCCSpecNodeEmps(int fk_node, Int64 workid)
        {
            DataSet ds = new DataSet();
            Paras ps = new Paras();
            ps.SQL = "SELECT No,Name,FK_Dept FROM Port_Emp ";
            DataTable dtEmp = DBAccess.RunSQLReturnTable(ps);
            dtEmp.TableName = "Emps";
            ds.Tables.Add(dtEmp);


            ps = new Paras();
            ps.SQL = "SELECT No,Name FROM Port_Dept ";
            DataTable dtDept = DBAccess.RunSQLReturnTable(ps);
            dtDept.TableName = "Depts";
            ds.Tables.Add(dtDept);

            return ds;
        }
        #endregion  CC staff can send or get a list of .


        #region  Get a list of operations to send 
        /// <summary>
        ///  Get a list of designated personnel CC 
        ///  Explanation : You can generate this list based on the specified user data cc .
        /// </summary>
        /// <param name="FK_Emp"> Personnel Number </param>
        /// <returns> Returns a list of all the staff of the CC , Structure with table WF_CCList.</returns>
        public static DataTable DB_CCList(string FK_Emp)
        {
            Paras ps = new Paras();
            ps.SQL = "SELECT * FROM WF_CCList WHERE CCTo=" + SystemConfig.AppCenterDBVarStr + "FK_Emp";
            ps.Add("FK_Emp", FK_Emp);
            return DBAccess.RunSQLReturnTable(ps);
        }
        public static DataTable DB_CCList(string FK_Emp, CCSta sta)
        {
            Paras ps = new Paras();
            ps.SQL = "SELECT * FROM WF_CCList WHERE CCTo=" + SystemConfig.AppCenterDBVarStr + "FK_Emp AND Sta=" + SystemConfig.AppCenterDBVarStr + "Sta";
            ps.Add("FK_Emp", FK_Emp);
            ps.Add("Sta", (int)sta);
            return DBAccess.RunSQLReturnTable(ps);
        }
        /// <summary>
        ///  Get a list of designated personnel CC ( Unread )
        /// </summary>
        /// <param name="FK_Emp"> Personnel Number </param>
        /// <returns> Returns the CC list of the staff of unread </returns>
        public static DataTable DB_CCList_UnRead(string FK_Emp)
        {
            return DB_CCList(FK_Emp, CCSta.UnRead);
        }
        /// <summary>
        ///  Get a list of designated personnel CC ( Read )
        /// </summary>
        /// <param name="FK_Emp"> Personnel Number </param>
        /// <returns> Returns the CC list of the officers read </returns>
        public static DataTable DB_CCList_Read(string FK_Emp)
        {
            return DB_CCList(FK_Emp, CCSta.Read);
        }
        /// <summary>
        ///  Get a list of designated personnel CC ( Deleted )
        /// </summary>
        /// <param name="FK_Emp"> Personnel Number </param>
        /// <returns> Returns a list of the officers Cc deleted </returns>
        public static DataTable DB_CCList_Delete(string FK_Emp)
        {
            return DB_CCList(FK_Emp, CCSta.Del);
        }
        /// <summary>
        ///  Get a list of designated personnel CC ( Replied )
        /// </summary>
        /// <param name="FK_Emp"> Personnel Number </param>
        /// <returns> Returns a list of the officers Cc deleted </returns>
        public static DataTable DB_CCList_CheckOver(string FK_Emp)
        {
            return DB_CCList(FK_Emp, CCSta.CheckOver);
        }
        #endregion

        #region  Gets the collection process can be initiated by the current operator 
        /// <summary>
        ///  Get a designated officer can initiate collection procedures .
        ///  Explanation : Using this interface can generate a list of user-initiated process .
        /// </summary>
        /// <param name="userNo"> Operator number </param>
        /// <returns>BP.WF.Flows  May initiate the process of collection of objects , How to use this method to initiate the formation of a worklist , Please refer to :\WF\UC\Start.ascx</returns>
        public static Flows DB_GenerCanStartFlowsOfEntities(string userNo)
        {
            //  Calculation by post .
            string sql = "SELECT FK_Flow FROM WF_Node WHERE NodePosType=0 AND ( WhoExeIt=0 OR WhoExeIt=2 ) AND NodeID IN ( SELECT FK_Node FROM WF_NodeStation WHERE FK_Station IN (SELECT FK_Station FROM Port_EmpStation WHERE FK_Emp='" + userNo + "')) ";
            sql += " UNION  "; // By designated personnel calculation .
            sql += "  SELECT FK_Flow FROM WF_Node WHERE NodePosType=0 AND ( WhoExeIt=0 OR WhoExeIt=2 ) AND NodeID IN ( SELECT FK_Node FROM WF_NodeEmp WHERE FK_Emp='" + userNo + "' ) ";
            sql += " UNION  "; // Calculation by post .
            sql += " SELECT FK_Flow FROM WF_Node WHERE NodePosType=0 AND ( WhoExeIt=0 OR WhoExeIt=2 ) AND NodeID IN ( SELECT FK_Node FROM WF_NodeDept WHERE FK_Dept IN(SELECT FK_Dept FROM Port_Emp WHERE No='" + userNo + "' UNION SELECT FK_DEPT FROM Port_EmpDept WHERE FK_Emp='" + userNo + "') ) ";
            //sql += " UNION  "; // Communication by computing jobs and departments .
            //sql += " SELECT FK_Flow FROM WF_Node WHERE NodePosType=0 AND ( WhoExeIt=0 OR WhoExeIt=2 ) AND NodeID IN ( SELECT FK_Node FROM WF_NodeDept WHERE FK_Dept IN(SELECT FK_Dept FROM Port_Emp WHERE No='" + userNo + "' UNION SELECT FK_DEPT FROM Port_EmpDept WHERE FK_Emp='" + userNo + "') ) AND NodeID IN ( SELECT FK_Node FROM WF_NodeStation WHERE FK_Station IN( SELECT FK_Station FROM Port_EmpStation WHERE FK_Emp='" + userNo + "') ) ";

            Flows fls = new Flows();
            BP.En.QueryObject qo = new BP.En.QueryObject(fls);
            qo.AddWhereInSQL("No", sql);
            qo.addAnd();
            qo.AddWhere(FlowAttr.IsCanStart, true);
            if (WebUser.IsAuthorize)
            {
                /* If you are an authorized state */
                qo.addAnd();
                WF.Port.WFEmp wfEmp = new Port.WFEmp(userNo);
                qo.AddWhereIn("No", wfEmp.AuthorFlows);
            }

            qo.addOrderBy(FlowAttr.FK_FlowSort, FlowAttr.Idx);
            qo.DoQuery();
            return fls;
        }
        /// <summary>
        ///  Get a designated officer can initiate collection procedures 
        ///  Explanation : Using this interface can generate a list of user-initiated process .
        /// </summary>
        /// <param name="userNo"> Operator number </param>
        /// <returns>Datatable Types of data collection , Data Structures and tables WF_Flow Roughly the same .  How to use this method to initiate the formation of a worklist , Please refer to :\WF\UC\Start.ascx</returns>
        public static DataTable DB_GenerCanStartFlowsOfDataTable(string userNo)
        {
            //  Calculation by post .
            string sql = "";
            sql += "SELECT FK_Flow FROM WF_Node WHERE NodePosType=0 AND ( WhoExeIt=0 OR WhoExeIt=2 ) AND NodeID IN ( SELECT FK_Node FROM WF_NodeStation WHERE FK_Station IN (SELECT FK_Station FROM Port_EmpStation WHERE FK_Emp='" + userNo + "')) ";
            sql += " UNION  "; // By designated personnel calculation .
            sql += "SELECT FK_Flow FROM WF_Node WHERE NodePosType=0 AND ( WhoExeIt=0 OR WhoExeIt=2 ) AND NodeID IN ( SELECT FK_Node FROM WF_NodeEmp WHERE FK_Emp='" + userNo + "' ) ";
            sql += " UNION  "; //  Calculation by post .
            sql += "SELECT FK_Flow FROM WF_Node WHERE NodePosType=0 AND ( WhoExeIt=0 OR WhoExeIt=2 ) AND NodeID IN ( SELECT FK_Node FROM WF_NodeDept WHERE FK_Dept IN(SELECT FK_Dept FROM Port_Emp WHERE No='" + userNo + "' UNION SELECT FK_DEPT FROM Port_EmpDept WHERE FK_Emp='" + userNo + "') ) ";

            Flows fls = new Flows();
            BP.En.QueryObject qo = new BP.En.QueryObject(fls);
            qo.AddWhereInSQL("No", sql);
            qo.addAnd();
            qo.AddWhere(FlowAttr.IsCanStart, true);
            if (WebUser.IsAuthorize)
            {
                /* If you are an authorized state */
                qo.addAnd();
                WF.Port.WFEmp wfEmp = new Port.WFEmp(userNo);
                qo.AddWhereIn("No", wfEmp.AuthorFlows);
            }
            qo.addOrderBy("FK_FlowSort", FlowAttr.Idx);
            return qo.DoQueryToTable();
        }
        public static DataTable DB_GenerCanStartFlowsTree(string userNo)
        {
            //  Calculation by post .
            string sql = "";
            sql += "SELECT FK_Flow FROM WF_Node WHERE NodePosType=0 AND ( WhoExeIt=0 OR WhoExeIt=2 ) AND NodeID IN ( SELECT FK_Node FROM WF_NodeStation WHERE FK_Station IN (SELECT FK_Station FROM Port_EmpStation WHERE FK_Emp='" + userNo + "')) ";
            sql += " UNION  "; // By designated personnel calculation .
            sql += "SELECT FK_Flow FROM WF_Node WHERE NodePosType=0 AND ( WhoExeIt=0 OR WhoExeIt=2 ) AND NodeID IN ( SELECT FK_Node FROM WF_NodeEmp WHERE FK_Emp='" + userNo + "' ) ";
            sql += " UNION  "; //  Calculation by post .
            sql += "SELECT FK_Flow FROM WF_Node WHERE NodePosType=0 AND ( WhoExeIt=0 OR WhoExeIt=2 ) AND NodeID IN ( SELECT FK_Node FROM WF_NodeDept WHERE FK_Dept IN(SELECT FK_Dept FROM Port_Emp WHERE No='" + userNo + "' UNION SELECT FK_DEPT FROM Port_EmpDept WHERE FK_Emp='" + userNo + "') ) ";

            Flows fls = new Flows();
            BP.En.QueryObject qo = new BP.En.QueryObject(fls);
            qo.AddWhereInSQL("No", sql);
            qo.addAnd();
            qo.AddWhere(FlowAttr.IsCanStart, true);
            if (WebUser.IsAuthorize)
            {
                /* If you are an authorized state */
                qo.addAnd();
                WF.Port.WFEmp wfEmp = new Port.WFEmp(userNo);
                qo.AddWhereIn("No", wfEmp.AuthorFlows);
            }
            qo.addOrderBy("FK_FlowSort", FlowAttr.Idx);
            DataTable table = qo.DoQueryToTable();


            table.Columns.Add("_parentId");
            table.Columns.Add("state");

            string flowSort = string.Format("select No,Name,ParentNo from WF_FlowSort");

            DataTable sortTable = DBAccess.RunSQLReturnTable(flowSort);

            foreach (DataRow row in sortTable.Rows)
            {

                DataRow newRow = table.NewRow();

                newRow["No"] = row["No"];
                newRow["Name"] = row["Name"];
                newRow["state"] = "closed";

                if (row["ParentNo"] + "" != "0")
                {
                    newRow["_parentId"] = row["ParentNo"];
                }

                table.Rows.Add(newRow);
            }

            foreach (DataRow row in table.Rows)
            {
                if (string.IsNullOrEmpty(row["_parentId"] + "") && row["_parentId"] + "" != "0")
                {
                    row["_parentId"] = row["FK_FlowSort"];
                }
            }
            return table;
        }


        /// <summary>
        ///  Get ( With Form ) Confluence on the child thread 
        ///  Explanation : If you want to see all of the sub-thread running state at the confluence point .
        /// </summary>
        /// <param name="nodeIDOfHL"> Confluence ID</param>
        /// <param name="workid"> The work ID</param>
        /// <returns> Table WF_GenerWorkerList Structure similar to the datatable.</returns>
        public static DataTable DB_GenerHLSubFlowDtl_TB(int nodeIDOfHL, Int64 workid)
        {
            Node nd = new Node(nodeIDOfHL);
            Work wk = nd.HisWork;
            wk.OID = workid;
            wk.Retrieve();

            GenerWorkerLists wls = new GenerWorkerLists();
            QueryObject qo = new QueryObject(wls);
            qo.AddWhere(GenerWorkerListAttr.FID, wk.OID);
            qo.addAnd();
            qo.AddWhere(GenerWorkerListAttr.IsEnable, 1);
            qo.addAnd();
            qo.AddWhere(GenerWorkerListAttr.FK_Node,
                nd.FromNodes[0].GetValByKey(NodeAttr.NodeID));

            DataTable dt = qo.DoQueryToTable();
            if (dt.Rows.Count == 1)
            {
                qo.clear();
                qo.AddWhere(GenerWorkerListAttr.FID, wk.OID);
                qo.addAnd();
                qo.AddWhere(GenerWorkerListAttr.IsEnable, 1);
                return qo.DoQueryToTable();
            }
            return dt;
        }
        /// <summary>
        ///  Get ( Different forms ) Confluence on the child thread 
        /// </summary>
        /// <param name="nodeIDOfHL"> Confluence ID</param>
        /// <param name="workid"> The work ID</param>
        /// <returns> Table WF_GenerWorkerList Structure similar to the datatable.</returns>
        public static DataTable DB_GenerHLSubFlowDtl_YB(int nodeIDOfHL, Int64 workid)
        {
            Node nd = new Node(nodeIDOfHL);
            Work wk = nd.HisWork;
            wk.OID = workid;
            wk.Retrieve();

            GenerWorkerLists wls = new GenerWorkerLists();
            QueryObject qo = new QueryObject(wls);
            qo.AddWhere(GenerWorkerListAttr.FID, wk.OID);
            qo.addAnd();
            qo.AddWhere(GenerWorkerListAttr.IsEnable, 1);
            qo.addAnd();
            qo.AddWhere(GenerWorkerListAttr.IsPass, 0);
            return qo.DoQueryToTable();
        }
        #endregion  Gets the collection process can be initiated by the current operator 

        #region  Draft Process 
        /// <summary>
        ///  Gets the current operator of the process flow of the draft data 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <returns> Returns draft data collection , Column Information . OID= The work ID,Title= Title ,RDT= Record Date ,FK_Flow= Process ID ,FID= Process ID, FK_Node= Node ID</returns>
        public static DataTable DB_GenerDraftDataTable(string fk_flow)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            /* Get Data .*/
            Flow fl = new Flow(fk_flow);
            string dbStr = BP.Sys.SystemConfig.AppCenterDBVarStr;


            int val = (int)WFState.Draft;
            BP.DA.Paras ps = new BP.DA.Paras();
            ps.SQL = "SELECT OID,Title,RDT,'" + fk_flow + "' as FK_Flow,FID, " + int.Parse(fk_flow) + "01 as FK_Node FROM " + fl.PTable + " WHERE WFState=" + val + " AND FlowStarter=" + dbStr + "FlowStarter";
            ps.Add(GERptAttr.FlowStarter, BP.Web.WebUser.No);

            return BP.DA.DBAccess.RunSQLReturnTable(ps);
        }
        #endregion  Draft Process 


        #region  Get the current operator of the shared work 
        /// <summary>
        ///  Get to work pending the current staff 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <returns> Shared Work List </returns>
        public static DataTable DB_GenerEmpWorksOfDataTable(string userNo, string fk_flow)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            Paras ps = new Paras();
            string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
            string sql;
            if (WebUser.IsAuthorize == false)
            {
                /* Not authorize state */
                if (string.IsNullOrEmpty(fk_flow))
                {
                    if (BP.WF.Glo.IsEnableTaskPool == true)
                        ps.SQL = "SELECT * FROM WF_EmpWorks WHERE FK_Emp=" + dbstr + "FK_Emp AND TaskSta=0 AND WFState!=" + (int)WFState.Batch + " ORDER BY FK_Flow,ADT DESC ";
                    else
                        ps.SQL = "SELECT * FROM WF_EmpWorks WHERE FK_Emp=" + dbstr + "FK_Emp  AND WFState!=" + (int)WFState.Batch + " ORDER BY FK_Flow,ADT DESC ";

                    ps.Add("FK_Emp", userNo);
                }
                else
                {
                    if (BP.WF.Glo.IsEnableTaskPool == true)
                        ps.SQL = "SELECT * FROM WF_EmpWorks WHERE FK_Emp=" + dbstr + "FK_Emp AND TaskSta=0 AND FK_Flow=" + dbstr + "FK_Flow  AND WFState!=" + (int)WFState.Batch + " ORDER BY  ADT DESC ";
                    else
                        ps.SQL = "SELECT * FROM WF_EmpWorks WHERE FK_Emp=" + dbstr + "FK_Emp AND FK_Flow=" + dbstr + "FK_Flow  AND WFState!=" + (int)WFState.Batch + " ORDER BY  ADT DESC ";

                    ps.Add("FK_Flow", fk_flow);
                    ps.Add("FK_Emp", userNo);
                }
                return BP.DA.DBAccess.RunSQLReturnTable(ps);
            }

            /* If you are an authorized state ,  Get information about the current principal . */
            WF.Port.WFEmp emp = new Port.WFEmp(WebUser.No);
            switch (emp.HisAuthorWay)
            {
                case Port.AuthorWay.All:
                    if (string.IsNullOrEmpty(fk_flow))
                    {
                        if (BP.WF.Glo.IsEnableTaskPool == true)
                            ps.SQL = "SELECT * FROM WF_EmpWorks WHERE  FK_Emp=" + dbstr + "FK_Emp  AND TaskSta=0 ORDER BY FK_Flow,ADT DESC ";
                        else
                            ps.SQL = "SELECT * FROM WF_EmpWorks WHERE  FK_Emp=" + dbstr + "FK_Emp  ORDER BY FK_Flow,ADT DESC ";

                        ps.Add("FK_Emp", userNo);
                    }
                    else
                    {
                        if (BP.WF.Glo.IsEnableTaskPool == true)
                            ps.SQL = "SELECT * FROM WF_EmpWorks WHERE  FK_Emp=" + dbstr + "FK_Emp AND FK_Flow" + dbstr + "FK_Flow AND TaskSta=0 ORDER BY FK_Flow,ADT DESC ";
                        else
                            ps.SQL = "SELECT * FROM WF_EmpWorks WHERE  FK_Emp=" + dbstr + "FK_Emp AND FK_Flow" + dbstr + "FK_Flow ORDER BY FK_Flow,ADT DESC ";

                        ps.Add("FK_Emp", userNo);
                        ps.Add("FK_Flow", fk_flow);
                    }
                    break;
                case Port.AuthorWay.SpecFlows:
                    if (string.IsNullOrEmpty(fk_flow))
                    {
                        if (BP.WF.Glo.IsEnableTaskPool == true)
                            sql = "SELECT * FROM WF_EmpWorks WHERE FK_Emp=" + dbstr + "FK_Emp AND  FK_Flow IN " + emp.AuthorFlows + " AND TaskSta=0 ORDER BY FK_Flow,ADT DESC ";
                        else
                            sql = "SELECT * FROM WF_EmpWorks WHERE FK_Emp=" + dbstr + "FK_Emp AND  FK_Flow IN " + emp.AuthorFlows + "  ORDER BY FK_Flow,ADT DESC ";

                        ps.Add("FK_Emp", userNo);
                    }
                    else
                    {
                        if (BP.WF.Glo.IsEnableTaskPool == true)
                            sql = "SELECT * FROM WF_EmpWorks WHERE  FK_Emp=" + dbstr + "FK_Emp  AND FK_Flow" + dbstr + "FK_Flow AND FK_Flow IN " + emp.AuthorFlows + " AND TaskSta=0  ORDER BY FK_Flow,ADT DESC ";
                        else
                            sql = "SELECT * FROM WF_EmpWorks WHERE  FK_Emp=" + dbstr + "FK_Emp  AND FK_Flow" + dbstr + "FK_Flow AND FK_Flow IN " + emp.AuthorFlows + "  ORDER BY FK_Flow,ADT DESC ";

                        ps.Add("FK_Emp", userNo);
                        ps.Add("FK_Flow", fk_flow);
                    }
                    break;
                case Port.AuthorWay.None:
                    throw new Exception(" Other side (" + WebUser.No + ") Authorization has been canceled .");
                default:
                    throw new Exception("no such way...");
            }
            return BP.DA.DBAccess.RunSQLReturnTable(ps);
        }
        /// <summary>
        ///  Get the current operator share of work according to the state 
        /// </summary>
        /// <param name="wfState"> Process Status </param>
        /// <param name="fk_flow"> Process ID </param>
        /// <returns> Table structure and view WF_EmpWorks Consistency </returns>
        public static DataTable DB_GenerEmpWorksOfDataTable(string userNo, WFState wfState, string fk_flow)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            Paras ps = new Paras();
            string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
            string sql;
            if (WebUser.IsAuthorize == false)
            {
                /* Not authorize state */
                if (string.IsNullOrEmpty(fk_flow))
                {
                    if (BP.WF.Glo.IsEnableTaskPool == true)
                        ps.SQL = "SELECT * FROM WF_EmpWorks WHERE WFState=" + dbstr + "WFState AND FK_Emp=" + dbstr + "FK_Emp AND TaskSta=0   ORDER BY FK_Flow,ADT DESC ";
                    else
                        ps.SQL = "SELECT * FROM WF_EmpWorks WHERE WFState=" + dbstr + "WFState AND FK_Emp=" + dbstr + "FK_Emp  ORDER BY FK_Flow,ADT DESC ";


                    ps.Add("WFState", (int)wfState);
                    ps.Add("FK_Emp", userNo);
                }
                else
                {
                    if (BP.WF.Glo.IsEnableTaskPool == true)
                        ps.SQL = "SELECT * FROM WF_EmpWorks WHERE WFState=" + dbstr + "WFState AND FK_Emp=" + dbstr + "FK_Emp AND FK_Flow=" + dbstr + "FK_Flow AND TaskSta=0  ORDER BY  ADT DESC ";
                    else
                        ps.SQL = "SELECT * FROM WF_EmpWorks WHERE WFState=" + dbstr + "WFState AND FK_Emp=" + dbstr + "FK_Emp AND FK_Flow=" + dbstr + "FK_Flow ORDER BY  ADT DESC ";

                    ps.Add("WFState", (int)wfState);
                    ps.Add("FK_Flow", fk_flow);
                    ps.Add("FK_Emp", userNo);
                }
                return BP.DA.DBAccess.RunSQLReturnTable(ps);
            }

            /* If you are an authorized state ,  Get information about the current principal . */
            WF.Port.WFEmp emp = new Port.WFEmp(WebUser.No);
            switch (emp.HisAuthorWay)
            {
                case Port.AuthorWay.All:
                    if (string.IsNullOrEmpty(fk_flow))
                    {
                        if (BP.WF.Glo.IsEnableTaskPool == true)
                            ps.SQL = "SELECT * FROM WF_EmpWorks WHERE WFState=" + dbstr + "WFState AND FK_Emp=" + dbstr + "FK_Emp  AND TaskSta=0  ORDER BY FK_Flow,ADT DESC ";
                        else
                            ps.SQL = "SELECT * FROM WF_EmpWorks WHERE WFState=" + dbstr + "WFState AND FK_Emp=" + dbstr + "FK_Emp  ORDER BY FK_Flow,ADT DESC ";

                        ps.Add("WFState", (int)wfState);
                        ps.Add("FK_Emp", BP.Web.WebUser.No);
                    }
                    else
                    {
                        if (BP.WF.Glo.IsEnableTaskPool == true)
                            ps.SQL = "SELECT * FROM WF_EmpWorks WHERE WFState=" + dbstr + "WFState AND FK_Emp=" + dbstr + "FK_Emp AND FK_Flow" + dbstr + "FK_Flow AND TaskSta=0  ORDER BY FK_Flow,ADT DESC ";
                        else
                            ps.SQL = "SELECT * FROM WF_EmpWorks WHERE WFState=" + dbstr + "WFState AND FK_Emp=" + dbstr + "FK_Emp AND FK_Flow" + dbstr + "FK_Flow ORDER BY FK_Flow,ADT DESC ";


                        ps.Add("WFState", (int)wfState);
                        ps.Add("FK_Emp", BP.Web.WebUser.No);
                        ps.Add("FK_Flow", fk_flow);
                    }
                    break;
                case Port.AuthorWay.SpecFlows:
                    if (string.IsNullOrEmpty(fk_flow))
                    {
                        if (BP.WF.Glo.IsEnableTaskPool == true)
                            sql = "SELECT * FROM WF_EmpWorks WHERE WFState=" + dbstr + "WFState AND FK_Emp=" + dbstr + "FK_Emp AND  FK_Flow IN " + emp.AuthorFlows + " AND TaskSta=0   ORDER BY FK_Flow,ADT DESC ";
                        else
                            sql = "SELECT * FROM WF_EmpWorks WHERE WFState=" + dbstr + "WFState AND FK_Emp=" + dbstr + "FK_Emp AND  FK_Flow IN " + emp.AuthorFlows + "  ORDER BY FK_Flow,ADT DESC ";


                        ps.Add("WFState", (int)wfState);
                        ps.Add("FK_Emp", BP.Web.WebUser.No);
                    }
                    else
                    {
                        if (BP.WF.Glo.IsEnableTaskPool == true)
                            sql = "SELECT * FROM WF_EmpWorks WHERE WFState=" + dbstr + "WFState AND FK_Emp=" + dbstr + "FK_Emp  AND FK_Flow" + dbstr + "FK_Flow AND FK_Flow IN " + emp.AuthorFlows + " AND TaskSta=0   ORDER BY FK_Flow,ADT DESC ";
                        else
                            sql = "SELECT * FROM WF_EmpWorks WHERE WFState=" + dbstr + "WFState AND FK_Emp=" + dbstr + "FK_Emp  AND FK_Flow" + dbstr + "FK_Flow AND FK_Flow IN " + emp.AuthorFlows + "  ORDER BY FK_Flow,ADT DESC ";

                        ps.Add("WFState", (int)wfState);
                        ps.Add("FK_Emp", BP.Web.WebUser.No);
                        ps.Add("FK_Flow", fk_flow);
                    }
                    break;
                case Port.AuthorWay.None:
                    throw new Exception(" Other side (" + WebUser.No + ") Authorization has been canceled .");
                default:
                    throw new Exception("no such way...");
            }
            return BP.DA.DBAccess.RunSQLReturnTable(ps);
        }
        /// <summary>
        ///  Get current information Upcoming operator 
        ///  Please refer to map data :WF_EmpWorks
        /// </summary>
        /// <returns> Return from view WF_EmpWorks Check out the data .</returns>
        public static DataTable DB_GenerEmpWorksOfDataTable()
        {
            Paras ps = new Paras();
            string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
            string wfSql = "  WFState=" + (int)WFState.Askfor + " OR WFState=" + (int)WFState.Runing + "  OR WFState=" + (int)WFState.AskForReplay + " OR WFState=" + (int)WFState.Shift + " OR WFState=" + (int)WFState.ReturnSta + " OR WFState=" + (int)WFState.Fix;
            string sql;

            if (WebUser.IsAuthorize == false)
            {
                /* Not authorize state */
                if (BP.WF.Glo.IsEnableTaskPool == true)
                    ps.SQL = "SELECT * FROM WF_EmpWorks WHERE (" + wfSql + ") AND FK_Emp=" + dbstr + "FK_Emp AND TaskSta!=1  ORDER BY ADT DESC";
                else
                    ps.SQL = "SELECT * FROM WF_EmpWorks WHERE (" + wfSql + ") AND FK_Emp=" + dbstr + "FK_Emp ORDER BY ADT DESC";

                ps.Add("FK_Emp", BP.Web.WebUser.No);
                return BP.DA.DBAccess.RunSQLReturnTable(ps);
            }

            /* If you are an authorized state ,  Get information about the current principal . */
            WF.Port.WFEmp emp = new Port.WFEmp(WebUser.No);
            switch (emp.HisAuthorWay)
            {
                case Port.AuthorWay.All:
                    if (BP.WF.Glo.IsEnableTaskPool == true)
                        ps.SQL = "SELECT * FROM WF_EmpWorks WHERE (" + wfSql + ") AND FK_Emp=" + dbstr + "FK_Emp AND TaskSta!=0  ORDER BY ADT DESC";
                    else
                        ps.SQL = "SELECT * FROM WF_EmpWorks WHERE (" + wfSql + ") AND FK_Emp=" + dbstr + "FK_Emp ORDER BY ADT DESC";
                    ps.Add("FK_Emp", BP.Web.WebUser.No);
                    break;
                case Port.AuthorWay.SpecFlows:
                    if (BP.WF.Glo.IsEnableTaskPool == true)
                        ps.SQL = "SELECT * FROM WF_EmpWorks WHERE (" + wfSql + ") AND FK_Emp=" + dbstr + "FK_Emp AND  FK_Flow IN " + emp.AuthorFlows + " AND TaskSta!=0    ORDER BY ADT DESC";
                    else
                        ps.SQL = "SELECT * FROM WF_EmpWorks WHERE (" + wfSql + ") AND FK_Emp=" + dbstr + "FK_Emp AND  FK_Flow IN " + emp.AuthorFlows + "   ORDER BY ADT DESC";

                    ps.Add("FK_Emp", BP.Web.WebUser.No);
                    break;
                case Port.AuthorWay.None:
                    throw new Exception(" Other side (" + WebUser.No + ") Authorization has been canceled .");
                default:
                    throw new Exception("no such way...");
            }
            return BP.DA.DBAccess.RunSQLReturnTable(ps);
        }
        /// <summary>
        ///  Obtain a list of statistics has been completed 
        /// </summary>
        /// <param name="userNo"> Operator number </param>
        /// <returns> Have FlowNo,FlowName,Num Be designated person to do three columns list </returns>
        public static DataTable DB_FlowCompleteGroup(string userNo)
        {
            if (Glo.IsDeleteGenerWorkFlow == false)
            {
                /*  If not, delete the registry process . */
                Paras ps = new Paras();
                string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
                ps.SQL = "SELECT FK_Flow as No,FlowName,COUNT(*) Num FROM WF_GenerWorkFlow WHERE Emps LIKE '%@" + userNo + "@%' AND FID=0 AND WFState=" + (int)WFState.Complete + " GROUP BY FK_Flow,FlowName";
                return BP.DA.DBAccess.RunSQLReturnTable(ps);
            }
            else
            {
                throw new Exception(" Unrealized ..");
                //Paras ps = new Paras();
                //string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
                //ps.SQL = "SELECT * FROM V_FlowData WHERE FlowEmps LIKE '%@" + userNo + "%' AND FID=0 AND WFState=" + (int)WFState.Complete;
                //return BP.DA.DBAccess.RunSQLReturnTable(ps);
            }
        }

        /// <summary>
        ///  Gets the page has completed the process 
        /// </summary>
        /// <param name="userNo"> User ID </param>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="pageSize"> Number per page </param>
        /// <param name="pageIdx"> The first few pages </param>
        /// <returns> User ID </returns>
        public static DataTable DB_FlowComplete(string userNo, string flowNo, int pageSize, int pageIdx)
        {
            if (Glo.IsDeleteGenerWorkFlow == false)
            {
                /*  If not, delete the registry process . */
                GenerWorkFlows ens = new GenerWorkFlows();
                QueryObject qo = new QueryObject(ens);
                if (flowNo != null)
                {
                    qo.AddWhere(GenerWorkFlowAttr.FK_Flow, flowNo);
                    qo.addAnd();
                }
                qo.AddWhere(GenerWorkFlowAttr.FID, 0);
                qo.addAnd();
                qo.AddWhere(GenerWorkFlowAttr.WFState, (int)WFState.Complete);
                qo.addAnd();
                qo.AddWhere(GenerWorkFlowAttr.Emps, " LIKE ", " '%@" + userNo + "@%'");
                /** Zhou Peng small modifications -----------------------------START**/
                // qo.DoQuery(GenerWorkFlowAttr.WorkID,pageSize, pageIdx);
                qo.DoQuery(GenerWorkFlowAttr.WorkID, pageSize, pageIdx, GenerWorkFlowAttr.RDT, true);
                /** Zhou Peng small modifications -----------------------------END**/
                return ens.ToDataTableField();
            }
            else
            {
                Paras ps = new Paras();
                string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
                ps.SQL = "SELECT *,FlowEndNode FK_Node FROM V_FlowData WHERE FlowEmps LIKE '%@" + userNo + "%' AND   FID=0 AND WFState=" + (int)WFState.Complete;

                return BP.DA.DBAccess.RunSQLReturnTable(ps);
            }
        }

        /// <summary>
        ///  Query specifies the process has been completed the process 
        /// </summary>
        /// <param name="userNo"></param>
        /// <param name="pageCount"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIdx"></param>
        /// <param name="strFlow"></param>
        /// <returns></returns>
        public static DataTable DB_FlowComplete(string userNo, int pageCount, int pageSize, int pageIdx, string strFlow)
        {
            if (Glo.IsDeleteGenerWorkFlow == false)
            {
                /*  If not, delete the registry process . */
                GenerWorkFlows ens = new GenerWorkFlows();
                QueryObject qo = new QueryObject(ens);
                qo.AddWhere(GenerWorkFlowAttr.FID, 0);
                qo.addAnd();
                qo.AddWhere(GenerWorkFlowAttr.WFState, (int)WFState.Complete);
                qo.addAnd();
                qo.AddWhere(GenerWorkFlowAttr.Emps, " LIKE ", " '%@" + userNo + "@%'");
                qo.addAnd();
                qo.AddWhere(GenerWorkFlowAttr.FK_Flow, strFlow);
                qo.DoQuery(GenerWorkFlowAttr.WorkID, pageSize, pageIdx);
                return ens.ToDataTableField();
            }
            else
            {
                Paras ps = new Paras();
                string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
                ps.SQL = "SELECT *,FlowEndNode FK_Node FROM V_FlowData WHERE FK_Flow='" + strFlow + "' AND FlowEmps LIKE '%@" + userNo + "%' AND FID=0 AND WFState=" + (int)WFState.Complete;
                return BP.DA.DBAccess.RunSQLReturnTable(ps);
            }
        }
        /// <summary>
        ///  Query bulletin process specifies the process has been completed 
        /// </summary>
        /// <param name="pageCount"> Pages </param>
        /// <param name="pageSize"> The number of page </param>
        /// <param name="pageIdx"> Page number </param>
        /// <param name="strFlow"> Process ID </param>
        /// <returns></returns>
        public static DataTable DB_FlowComplete(string strFlow, int pageSize, int pageIdx)
        {
            if (Glo.IsDeleteGenerWorkFlow == false)
            {
                /*  If not, delete the registry process . */
                GenerWorkFlows ens = new GenerWorkFlows();
                QueryObject qo = new QueryObject(ens);
                qo.AddWhere(GenerWorkFlowAttr.FID, 0);
                qo.addAnd();
                qo.AddWhere(GenerWorkFlowAttr.WFState, (int)WFState.Complete);
                qo.addAnd();
                qo.AddWhere(GenerWorkFlowAttr.FK_Flow, strFlow);
                qo.DoQuery(GenerWorkFlowAttr.WorkID, pageSize, pageIdx);
                return ens.ToDataTableField();
            }
            else
            {
                Paras ps = new Paras();
                string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
                ps.SQL = "SELECT *,FlowEndNode FK_Node FROM V_FlowData WHERE FK_Flow='" + strFlow + "' AND FID=0 AND WFState=" + (int)WFState.Complete;
                return BP.DA.DBAccess.RunSQLReturnTable(ps);
            }
        }
        /// <summary>
        ///  Acquisition process has been completed 
        /// </summary>
        /// <returns></returns>
        public static DataTable DB_FlowComplete()
        {
            if (Glo.IsDeleteGenerWorkFlow == false)
            {
                /*  If not, delete the registry process . */
                Paras ps = new Paras();
                string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
                ps.SQL = "SELECT 'RUNNING' AS Type, * FROM WF_GenerWorkFlow WHERE Emps LIKE '%@" + WebUser.No + "@%' AND FID=0 AND WFState=" + (int)WFState.Complete + " ORDER BY  RDT DESC";
                return BP.DA.DBAccess.RunSQLReturnTable(ps);
            }
            else
            {
                Paras ps = new Paras();
                string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
                ps.SQL = "SELECT 'RUNNING' AS Type, * FROM V_FlowData WHERE FlowEmps LIKE '%@" + WebUser.No + "@%' AND FID=0 AND WFState=" + (int)WFState.Complete + " ORDER BY RDT DESC";
                return BP.DA.DBAccess.RunSQLReturnTable(ps);
            }
        }
        /// <summary>
        ///  Acquisition has been completed 
        /// </summary>
        /// <returns></returns>
        public static DataTable DB_FlowCompleteAndCC()
        {
            DataTable dt = DB_FlowComplete();
            DataTable ccDT = DB_CCList_CheckOver(WebUser.No);

            try
            {
                dt.Columns.Add("MyPK");
                dt.Columns.Add("Sta");
            }
            catch (Exception)
            {

            }

            foreach (DataRow row in ccDT.Rows)
            {
                DataRow newRow = dt.NewRow();

                foreach (DataColumn column in ccDT.Columns)
                {
                    foreach (DataColumn dtColumn in dt.Columns)
                    {
                        if (column.ColumnName == dtColumn.ColumnName)
                        {
                            newRow[column.ColumnName] = row[dtColumn.ColumnName];
                        }

                    }

                }
                newRow["Type"] = "CC";
                dt.Rows.Add(newRow);
            }
            dt.DefaultView.Sort = "RDT DESC";
            return dt.DefaultView.ToTable();
        }
        public static DataTable DB_FlowComplete2(string fk_flow, string title)
        {
            if (Glo.IsDeleteGenerWorkFlow == false)
            {
                /*  If not, delete the registry process . */
                Paras ps = new Paras();
                string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
                if (string.IsNullOrEmpty(fk_flow))
                {
                    if (string.IsNullOrEmpty(title))
                        ps.SQL = "SELECT 'RUNNING' AS Type,* FROM WF_GenerWorkFlow WHERE Emps LIKE '%@" + WebUser.No + "@%' AND FID=0 AND WFState=" + (int)WFState.Complete + " and FK_Flow!='010' order by RDT desc";
                    else
                        ps.SQL = "SELECT 'RUNNING' AS Type,* FROM WF_GenerWorkFlow WHERE Emps LIKE '%@" + WebUser.No + "@%' and Title Like '%" + title + "%' AND FID=0 AND WFState=" + (int)WFState.Complete + " and FK_Flow!='010' order by RDT desc";
                }
                else
                {
                    if (string.IsNullOrEmpty(title))
                        ps.SQL = "SELECT 'RUNNING' AS Type,* FROM WF_GenerWorkFlow WHERE Emps LIKE '%@" + WebUser.No + "@%' AND FID=0 AND WFState=" + (int)WFState.Complete + " and FK_Flow='" + fk_flow + "' order by RDT desc";
                    else
                        ps.SQL = "SELECT 'RUNNING' AS Type,* FROM WF_GenerWorkFlow WHERE Emps LIKE '%@" + WebUser.No + "@%' and Title Like '%" + title + "%' AND FID=0 AND WFState=" + (int)WFState.Complete + " and FK_Flow='" + fk_flow + "' order by RDT desc";
                }
                return BP.DA.DBAccess.RunSQLReturnTable(ps);
            }
            else
            {
                Paras ps = new Paras();
                string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
                if (string.IsNullOrEmpty(fk_flow))
                {
                    if (string.IsNullOrEmpty(title))
                        ps.SQL = "SELECT 'RUNNING' AS Type,* FROM V_FlowData WHERE FlowEmps LIKE '%@" + WebUser.No + "%' AND FID=0 AND WFState=" + (int)WFState.Complete + " and FK_Flow!='010' order by RDT desc";
                    else
                        ps.SQL = "SELECT 'RUNNING' AS Type,* FROM V_FlowData WHERE FlowEmps LIKE '%@" + WebUser.No + "%' and Title Like '%" + title + "%' AND FID=0 AND WFState=" + (int)WFState.Complete + " and FK_Flow!='010' order by RDT desc";
                }
                else
                {
                    if (string.IsNullOrEmpty(title))
                        ps.SQL = "SELECT 'RUNNING' AS Type,* FROM V_FlowData WHERE FlowEmps LIKE '%@" + WebUser.No + "%' AND FID=0 AND WFState=" + (int)WFState.Complete + " and FK_Flow='" + fk_flow + "' order by RDT desc";
                    else
                        ps.SQL = "SELECT 'RUNNING' AS Type,* FROM V_FlowData WHERE FlowEmps LIKE '%@" + WebUser.No + "%' and Title Like '%" + title + "%' AND FID=0 AND WFState=" + (int)WFState.Complete + " and FK_Flow='" + fk_flow + "' order by RDT desc";
                }
                return BP.DA.DBAccess.RunSQLReturnTable(ps);
            }
        }

        public static DataTable DB_FlowCompleteAndCC2(string fk_flow, string title)
        {
            DataTable dt = DB_FlowComplete2(fk_flow, title);
            DataTable ccDT = DB_CCList_CheckOver(WebUser.No);
            try
            {
                dt.Columns.Add("MyPK");
                dt.Columns.Add("Sta");
            }
            catch (Exception)
            {

            }

            foreach (DataRow row in ccDT.Rows)
            {
                DataRow newRow = dt.NewRow();

                foreach (DataColumn column in ccDT.Columns)
                {
                    foreach (DataColumn dtColumn in dt.Columns)
                    {
                        if (column.ColumnName == dtColumn.ColumnName)
                        {
                            newRow[column.ColumnName] = row[dtColumn.ColumnName];
                        }

                    }

                }
                newRow["Type"] = "CC";
                dt.Rows.Add(newRow);
            }
            dt.DefaultView.Sort = "RDT DESC";
            return dt.DefaultView.ToTable();
        }
        // I initiated the process of obtaining 
        public static DataTable DB_MyFlow(string fk_flow, string title)
        {
            Paras ps = new Paras();
            string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
            if (string.IsNullOrEmpty(fk_flow))
            {
                if (string.IsNullOrEmpty(title))
                    ps.SQL = "SELECT * FROM WF_GenerWorkFlow WHERE Starter='" + WebUser.No + "' AND FK_Flow!='010' ";
                else
                    ps.SQL = "SELECT * FROM WF_GenerWorkFlow WHERE Starter='" + WebUser.No + "' AND FK_Flow!='010' and Title Like '%" + title + "%' ";
            }
            else
            {
                if (string.IsNullOrEmpty(title))
                    ps.SQL = "SELECT * FROM WF_GenerWorkFlow WHERE Starter='" + WebUser.No + "' AND FK_Flow='" + fk_flow + "' ";
                else
                    ps.SQL = "SELECT * FROM WF_GenerWorkFlow WHERE Starter='" + WebUser.No + "' AND FK_Flow='" + fk_flow + "' and Title Like '%" + title + "%'";
            }
            return BP.DA.DBAccess.RunSQLReturnTable(ps);
        }

        /// <summary>
        ///  A list of work to get the task pool 
        /// </summary>
        /// <returns> Worklist task pools </returns>
        public static DataTable DB_TaskPool()
        {
            if (BP.WF.Glo.IsEnableTaskPool == false)
                throw new Exception("@ You must Web.config Enable IsEnableTaskPool We can do this .");

            Paras ps = new Paras();
            string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
            string wfSql = "  (WFState=" + (int)WFState.Askfor + " OR WFState=" + (int)WFState.Runing + " OR WFState=" + (int)WFState.Shift + " OR WFState=" + (int)WFState.ReturnSta + ") AND TaskSta=" + (int)TaskSta.Sharing;
            string sql;
            string realSql = null;
            if (WebUser.IsAuthorize == false)
            {
                /* Not authorize state */
                //ps.SQL = "SELECT * FROM WF_EmpWorks WHERE (" + wfSql + ") AND FK_Emp=" + dbstr + "FK_Emp ORDER BY FK_Flow,ADT DESC ";
                // ps.SQL = "SELECT * FROM WF_EmpWorks WHERE (" + wfSql + ") AND FK_Emp=" + dbstr + "FK_Emp ORDER BY  ADT DESC ";

                ps.SQL = "SELECT * FROM WF_EmpWorks WHERE (" + wfSql + ") AND FK_Emp=" + dbstr + "FK_Emp ";

                ps.Add("FK_Emp", BP.Web.WebUser.No);
                return BP.DA.DBAccess.RunSQLReturnTable(ps);
            }

            /* If you are an authorized state ,  Get information about the current principal . */
            WF.Port.WFEmp emp = new Port.WFEmp(WebUser.No);
            switch (emp.HisAuthorWay)
            {
                case Port.AuthorWay.All:
                    ps.SQL = "SELECT * FROM WF_EmpWorks WHERE (" + wfSql + ") AND FK_Emp=" + dbstr + "FK_Emp AND TaskSta=0";
                    ps.Add("FK_Emp", BP.Web.WebUser.No);
                    break;
                case Port.AuthorWay.SpecFlows:
                    ps.SQL = "SELECT * FROM WF_EmpWorks WHERE (" + wfSql + ") AND FK_Emp=" + dbstr + "FK_Emp AND  FK_Flow IN " + emp.AuthorFlows + " ";
                    ps.Add("FK_Emp", BP.Web.WebUser.No);
                    break;
                case Port.AuthorWay.None:
                    throw new Exception(" Other side (" + WebUser.No + ") Authorization has been canceled .");
                default:
                    throw new Exception("no such way...");
            }
            return BP.DA.DBAccess.RunSQLReturnTable(ps);
        }
        /// <summary>
        ///  For a list of job I applied down from the task pool 
        /// </summary>
        /// <returns></returns>
        public static DataTable DB_TaskPoolOfMyApply()
        {
            if (BP.WF.Glo.IsEnableTaskPool == false)
                throw new Exception("@ You must Web.config Enable IsEnableTaskPool We can do this .");

            Paras ps = new Paras();
            string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
            string wfSql = "  (WFState=" + (int)WFState.Askfor + " OR WFState=" + (int)WFState.Runing + " OR WFState=" + (int)WFState.Shift + " OR WFState=" + (int)WFState.ReturnSta + ") AND TaskSta=" + (int)TaskSta.Takeback;
            string sql;
            string realSql;
            if (WebUser.IsAuthorize == false)
            {
                /* Not authorize state */
                // ps.SQL = "SELECT * FROM WF_EmpWorks WHERE (" + wfSql + ") AND FK_Emp=" + dbstr + "FK_Emp ORDER BY FK_Flow,ADT DESC ";
                //ps.SQL = "SELECT * FROM WF_EmpWorks WHERE (" + wfSql + ") AND FK_Emp=" + dbstr + "FK_Emp ORDER BY ADT DESC ";
                ps.SQL = "SELECT * FROM WF_EmpWorks WHERE (" + wfSql + ") AND FK_Emp=" + dbstr + "FK_Emp";

                // ps.SQL = "select v1.*,v2.name,v3.name as ParentName from (" + realSql + ") as v1 left join JXW_Inc v2 on v1.WorkID=v2.OID left join Jxw_Inc V3 on v1.PWorkID = v3.OID ORDER BY v1.ADT DESC";

                ps.Add("FK_Emp", BP.Web.WebUser.No);
                return BP.DA.DBAccess.RunSQLReturnTable(ps);
            }

            /* If you are an authorized state ,  Get information about the current principal . */
            WF.Port.WFEmp emp = new Port.WFEmp(WebUser.No);
            switch (emp.HisAuthorWay)
            {
                case Port.AuthorWay.All:
                    ps.SQL = "SELECT * FROM WF_EmpWorks WHERE (" + wfSql + ") AND FK_Emp=" + dbstr + "FK_Emp AND TaskSta=0";
                    ps.Add("FK_Emp", BP.Web.WebUser.No);
                    break;
                case Port.AuthorWay.SpecFlows:
                    ps.SQL = "SELECT * FROM WF_EmpWorks WHERE (" + wfSql + ") AND FK_Emp=" + dbstr + "FK_Emp AND  FK_Flow IN " + emp.AuthorFlows + "";
                    ps.Add("FK_Emp", BP.Web.WebUser.No);
                    break;
                case Port.AuthorWay.None:
                    throw new Exception(" Other side (" + WebUser.No + ") Authorization has been canceled .");
                default:
                    throw new Exception("no such way...");
            }
            return BP.DA.DBAccess.RunSQLReturnTable(ps);
        }
        /// <summary>
        ///  Get a list of all processes suspend work 
        /// </summary>
        /// <returns> Return from view WF_EmpWorks Check out the data .</returns>
        public static DataTable DB_GenerHungUpList()
        {
            return DB_GenerHungUpList(null);
        }
        /// <summary>
        ///  Process for the specified list of pending work 
        /// </summary>
        /// <param name="fk_flow"> Process ID , If the number is empty then return a list of all processes suspend work .</param>
        /// <returns> Return from view WF_EmpWorks Check out the data .</returns>
        public static DataTable DB_GenerHungUpList(string fk_flow)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            string sql;
            int state = (int)WFState.HungUp;
            if (WebUser.IsAuthorize)
            {
                WF.Port.WFEmp emp = new Port.WFEmp(WebUser.No);
                if (string.IsNullOrEmpty(fk_flow))
                    sql = "SELECT a.WorkID FROM WF_GenerWorkFlow A, WF_GenerWorkerlist B WHERE  A.WFState=" + state + " AND A.WorkID=B.WorkID AND B.FK_Emp='" + WebUser.No + "' AND B.IsEnable=1 AND A.FK_Flow IN " + emp.AuthorFlows;
                else
                    sql = "SELECT a.WorkID FROM WF_GenerWorkFlow A, WF_GenerWorkerlist B WHERE  A.FK_Flow='" + fk_flow + "' AND A.WFState=" + state + " AND A.WorkID=B.WorkID AND B.FK_Emp='" + WebUser.No + "' AND  B.IsPass=1 AND A.FK_Flow IN " + emp.AuthorFlows;
            }
            else
            {
                if (string.IsNullOrEmpty(fk_flow))
                    sql = "SELECT a.WorkID FROM WF_GenerWorkFlow A, WF_GenerWorkerlist B WHERE  A.WFState=" + state + " AND A.WorkID=B.WorkID AND B.FK_Emp='" + WebUser.No + "' AND B.IsEnable=1   ";
                else
                    sql = "SELECT a.WorkID FROM WF_GenerWorkFlow A, WF_GenerWorkerlist B WHERE A.FK_Flow='" + fk_flow + "'  AND A.WFState=" + state + " AND A.WorkID=B.WorkID AND B.FK_Emp='" + WebUser.No + "' AND B.IsEnable=1 ";
            }
            GenerWorkFlows gwfs = new GenerWorkFlows();
            gwfs.RetrieveInSQL(GenerWorkFlowAttr.WorkID, "(" + sql + ")");
            return gwfs.ToDataTableField();
        }
        /// <summary>
        ///  Get tombstoned flow 
        /// </summary>
        /// <returns> Return from view WF_EmpWorks Check out the data .</returns>
        public static DataTable DB_GenerDeleteWorkList()
        {
            return DB_GenerDeleteWorkList(WebUser.No, null);
        }
        /// <summary>
        ///  Get tombstoned flow : According to the process ID 
        /// </summary>
        /// <param name="userNo"> Operator number </param>
        /// <param name="fk_flow"> Process ID ( Can be empty )</param>
        /// <returns>WF_GenerWorkFlow Collection of data structures </returns>
        public static DataTable DB_GenerDeleteWorkList(string userNo, string fk_flow)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            string sql;
            int state = (int)WFState.Delete;
            if (WebUser.IsAuthorize)
            {
                WF.Port.WFEmp emp = new Port.WFEmp(WebUser.No);
                if (string.IsNullOrEmpty(fk_flow))
                    sql = "SELECT a.WorkID FROM WF_GenerWorkFlow A, WF_GenerWorkerlist B WHERE  A.WFState=" + state + " AND A.WorkID=B.WorkID AND B.FK_Emp='" + WebUser.No + "' AND B.IsEnable=1 AND A.FK_Flow IN " + emp.AuthorFlows;
                else
                    sql = "SELECT a.WorkID FROM WF_GenerWorkFlow A, WF_GenerWorkerlist B WHERE A.FK_Flow='" + fk_flow + "'  AND A.WFState=" + state + " AND A.WorkID=B.WorkID AND B.FK_Emp='" + WebUser.No + "' AND  B.IsPass=1 AND A.FK_Flow IN " + emp.AuthorFlows;
            }
            else
            {
                if (string.IsNullOrEmpty(fk_flow))
                    sql = "SELECT a.WorkID FROM WF_GenerWorkFlow A, WF_GenerWorkerlist B WHERE  A.WFState=" + state + " AND A.WorkID=B.WorkID AND B.FK_Emp='" + WebUser.No + "' AND B.IsEnable=1   ";
                else
                    sql = "SELECT a.WorkID FROM WF_GenerWorkFlow A, WF_GenerWorkerlist B WHERE A.FK_Flow='" + fk_flow + "'  AND A.WFState=" + state + " AND A.WorkID=B.WorkID AND B.FK_Emp='" + WebUser.No + "' AND B.IsEnable=1 ";
            }
            GenerWorkFlows gwfs = new GenerWorkFlows();
            gwfs.RetrieveInSQL(GenerWorkFlowAttr.WorkID, "(" + sql + ")");
            return gwfs.ToDataTableField();
        }
        #endregion  Get the current operator of the shared work 

        #region  Data acquisition process 
        /// <summary>
        ///  Gets the process data in accordance with the process state 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="sta"> Process Status </param>
        /// <returns> Data sheet OID,Title,RDT,FID</returns>
        public static DataTable DB_NDxxRpt(string fk_flow, WFState sta)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            Flow fl = new Flow(fk_flow);
            string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
            string sql = "SELECT OID,Title,RDT,FID FROM " + fl.PTable + " WHERE WFState=" + (int)sta + " AND Rec=" + dbstr + "Rec";
            BP.DA.Paras ps = new BP.DA.Paras();
            ps.SQL = sql;
            ps.Add("Rec", BP.Web.WebUser.No);
            return DBAccess.RunSQLReturnTable(ps);
        }
        #endregion

        #region  Get the current node can return the .
        /// <summary>
        ///  Get the current node can return nodes 
        /// </summary>
        /// <param name="fk_node"> Node ID</param>
        /// <param name="workid"> The work ID</param>
        /// <param name="fid">FID</param>
        /// <returns>No Node number ,Name Node Name ,Rec Record people ,RecName Name of recording </returns>
        public static DataTable DB_GenerWillReturnNodes(int fk_node, Int64 workid, Int64 fid)
        {
            DataTable dt = new DataTable("obt");
            dt.Columns.Add("No"); //  Node ID
            dt.Columns.Add("Name"); //  Node Name .
            dt.Columns.Add("Rec"); //  Be returned to the operator on the node number .
            dt.Columns.Add("RecName"); //  Be returned to the operator on the node name .

            Node nd = new Node(fk_node);
            if (nd.HisRunModel == RunModel.SubThread)
            {
                /* If the child thread , It can only be returned to its previous node , Write now dead , Other settings does not work .*/
                Nodes nds = nd.FromNodes;
                foreach (Node ndFrom in nds)
                {
                    Work wk;
                    switch (ndFrom.HisRunModel)
                    {
                        case RunModel.FL:
                        case RunModel.FHL:
                            wk = ndFrom.HisWork;
                            wk.OID = fid;
                            if (wk.RetrieveFromDBSources() == 0)
                                continue;
                            break;
                        case RunModel.SubThread:
                            wk = ndFrom.HisWork;
                            wk.OID = workid;
                            if (wk.RetrieveFromDBSources() == 0)
                                continue;
                            break;
                        case RunModel.Ordinary:
                        default:
                            throw new Exception(" Exception Process Design , Child thread on one node can not be ordinary nodes .");
                            break;
                    }

                    if (ndFrom.NodeID == fk_node)
                        continue;

                    DataRow dr = dt.NewRow();
                    dr["No"] = ndFrom.NodeID.ToString();
                    dr["Name"] = ndFrom.Name;

                    dr["Rec"] = wk.Rec;
                    dr["RecName"] = wk.RecText;

                    dt.Rows.Add(dr);
                }
                if (dt.Rows.Count == 0)
                    throw new Exception(" Did not get to the list of nodes should be returned .");
                return dt;
            }

            string sql = "";
            if (nd.IsHL || nd.IsFLHL)
            {
                /* If the current point is the shunt , Or sub-confluent , Calculated according to the rules do not return .*/
                sql = "SELECT FK_Node AS No,FK_NodeText as Name, FK_Emp as Rec, FK_EmpText as RecName FROM WF_GenerWorkerlist WHERE FID=" + fid + " AND WorkID=" + workid + " AND FK_Node!=" + fk_node + "  ORDER BY RDT  ";
                return DBAccess.RunSQLReturnTable(sql);
            }

            WorkNode wn = new WorkNode(workid, fk_node);
            WorkNodes wns = new WorkNodes();
            switch (nd.HisReturnRole)
            {
                case ReturnRole.CanNotReturn:
                    return dt;
                case ReturnRole.ReturnAnyNodes:
                    if (nd.TodolistModel == TodolistModel.Order)
                        sql = "SELECT FK_Node as No,FK_NodeText as Name,FK_Emp as Rec,FK_EmpText as RecName FROM WF_GenerWorkerlist WHERE  (WorkID=" + workid + " AND IsEnable=1 AND IsPass=1 AND FK_Node!=" + fk_node + ") OR (FK_Node=" + fk_node + " AND IsPass <0)  ORDER BY RDT";
                    else
                        sql = "SELECT FK_Node as No,FK_NodeText as Name,FK_Emp as Rec,FK_EmpText as RecName FROM WF_GenerWorkerlist WHERE  WorkID=" + workid + " AND IsEnable=1 AND IsPass=1 AND FK_Node!=" + fk_node + " ORDER BY RDT";
                    return DBAccess.RunSQLReturnTable(sql);
                    break;
                case ReturnRole.ReturnPreviousNode:
                    WorkNode mywnP = wn.GetPreviousWorkNode();
                    //  turnTo = mywnP.HisWork.Rec + mywnP.HisWork.RecText;
                    DataRow dr1 = dt.NewRow();
                    dr1["No"] = mywnP.HisNode.NodeID.ToString();
                    dr1["Name"] = mywnP.HisNode.Name;

                    dr1["Rec"] = mywnP.HisWork.Rec;
                    dr1["RecName"] = mywnP.HisWork.RecText;
                    dt.Rows.Add(dr1);
                    break;
                case ReturnRole.ReturnSpecifiedNodes: // Return the specified node .
                    if (wns.Count == 0)
                        wns.GenerByWorkID(wn.HisNode.HisFlow, workid);

                    NodeReturns rnds = new NodeReturns();
                    rnds.Retrieve(NodeReturnAttr.FK_Node, fk_node);
                    if (rnds.Count == 0)
                        throw new Exception("@ Process design errors , You set the node can return the specified node , But the specified node set is empty , Please set it to formulate node in the node properties .");
                    foreach (WorkNode mywn in wns)
                    {
                        if (mywn.HisNode.NodeID == fk_node)
                            continue;

                        if (rnds.Contains(NodeReturnAttr.ReturnTo,
                            mywn.HisNode.NodeID) == false)
                            continue;

                        DataRow dr = dt.NewRow();
                        dr["No"] = mywn.HisNode.NodeID.ToString();
                        dr["Name"] = mywn.HisNode.Name;
                        dr["Rec"] = mywn.HisWork.Rec;
                        dr["RecName"] = mywn.HisWork.RecText;
                        dt.Rows.Add(dr);
                    }
                    break;
                case ReturnRole.ByReturnLine: // Returned in accordance with the process of the implementation of return line drawing .
                    Directions dirs = new Directions();
                    dirs.Retrieve(DirectionAttr.Node, fk_node, DirectionAttr.DirType, 1);
                    if (dirs.Count == 0)
                        throw new Exception("@ Process design errors : The current node does not draw the line bounce bounce back , For more information, please refer to refund rules .");
                    foreach (Direction dir in dirs)
                    {
                        Node toNode = new Node(dir.ToNode);
                        sql = "SELECT FK_Emp,FK_EmpText FROM WF_GenerWorkerlist WHERE FK_Node=" + toNode.NodeID + " AND WorkID=" + workid + " AND IsEnable=1 AND IsPass=1";
                        DataTable dt1 = DBAccess.RunSQLReturnTable(sql);
                        if (dt1.Rows.Count == 0)
                            continue;

                        DataRow dr = dt.NewRow();
                        dr["No"] = toNode.NodeID.ToString();
                        dr["Name"] = toNode.Name;
                        dr["Rec"] = dt1.Rows[0][0];
                        dr["RecName"] = dt1.Rows[0][1];
                        dt.Rows.Add(dr);
                    }
                    break;
                default:
                    throw new Exception("@ Does not determine the type of return .");
            }

            if (dt.Rows.Count == 0)
                throw new Exception("@ Node is not calculated to be returned , Please confirm node administrator reasonable return rules ?");
            return dt;
        }
        #endregion  Get the current node can return the 

        #region  Get the current work-in-transit operators 
        /// <summary>
        ///  Get unfinished process ( Also known as in-transit process : I participated in this process is not completed yet )
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <returns> Returns from the data view WF_GenerWorkflow Check out the data .</returns>
        public static DataTable DB_GenerRuning(string userNo, string fk_flow)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            string sql;
            int state = (int)WFState.Runing;
            if (WebUser.IsAuthorize)
            {
                WF.Port.WFEmp emp = new Port.WFEmp(userNo);
                if (string.IsNullOrEmpty(fk_flow))
                    sql = "SELECT a.WorkID FROM WF_GenerWorkFlow A, WF_GenerWorkerlist B WHERE A.WorkID=B.WorkID AND B.FK_Emp='" + userNo + "' AND B.IsEnable=1 AND  (B.IsPass=1 or B.IsPass < 0) AND A.FK_Flow IN " + emp.AuthorFlows;
                else
                    sql = "SELECT a.WorkID FROM WF_GenerWorkFlow A, WF_GenerWorkerlist B WHERE A.FK_Flow='" + fk_flow + "'  AND A.WorkID=B.WorkID AND B.FK_Emp='" + WebUser.No + "' AND B.IsEnable=1 AND  (B.IsPass=1 or B.IsPass < 0) AND A.FK_Flow IN " + emp.AuthorFlows;
            }
            else
            {
                if (string.IsNullOrEmpty(fk_flow))
                    sql = "SELECT a.WorkID FROM WF_GenerWorkFlow A, WF_GenerWorkerlist B WHERE A.WorkID=B.WorkID AND B.FK_Emp='" + userNo + "' AND B.IsEnable=1 AND  (B.IsPass=1 or B.IsPass < 0) ";
                else
                    sql = "SELECT a.WorkID FROM WF_GenerWorkFlow A, WF_GenerWorkerlist B WHERE A.FK_Flow='" + fk_flow + "'  AND A.WorkID=B.WorkID AND B.FK_Emp='" + userNo + "' AND B.IsEnable=1 AND (B.IsPass=1 or B.IsPass < 0 ) ";
            }
            GenerWorkFlows gwfs = new GenerWorkFlows();
            gwfs.RetrieveInSQL(GenerWorkFlowAttr.WorkID, "(" + sql + ")");
            return gwfs.ToDataTableField();
        }
        public static DataTable DB_GenerRuning2(string userNo, string fk_flow, string title)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            string sql;
            int state = (int)WFState.Runing;
            if (string.IsNullOrEmpty(fk_flow))
            {
                if (string.IsNullOrEmpty(title))
                    sql = "SELECT a.WorkID FROM WF_GenerWorkFlow A, WF_GenerWorkerlist B WHERE A.WorkID=B.WorkID AND B.FK_Emp='" + userNo + "' AND B.IsEnable=1 AND  (B.IsPass=1 or B.IsPass < 0) and A.FK_Flow!='010'";
                else
                    sql = "SELECT a.WorkID FROM WF_GenerWorkFlow A, WF_GenerWorkerlist B WHERE A.WorkID=B.WorkID AND B.FK_Emp='" + userNo + "' AND B.IsEnable=1 AND  (B.IsPass=1 or B.IsPass < 0) and A.FK_Flow!='010' and A.Title Like '%" + title + "%'";
            }
            else
            {
                if (string.IsNullOrEmpty(title))
                    sql = "SELECT a.WorkID FROM WF_GenerWorkFlow A, WF_GenerWorkerlist B WHERE A.FK_Flow='" + fk_flow + "'  AND A.WorkID=B.WorkID AND B.FK_Emp='" + userNo + "' AND B.IsEnable=1 AND (B.IsPass=1 or B.IsPass < 0 )";
                else
                    sql = "SELECT a.WorkID FROM WF_GenerWorkFlow A, WF_GenerWorkerlist B WHERE A.FK_Flow='" + fk_flow + "'  AND A.WorkID=B.WorkID AND B.FK_Emp='" + userNo + "' AND B.IsEnable=1 AND (B.IsPass=1 or B.IsPass < 0 ) and A.Title Like '%" + title + "%' ";
            }
            GenerWorkFlows gwfs = new GenerWorkFlows();
            gwfs.RetrieveInSQL(GenerWorkFlowAttr.WorkID, "(" + sql + ")");
            return gwfs.ToDataTableField();
        }
        /// <summary>
        ///  Working in transit 
        /// </summary>
        /// <returns></returns>
        public static DataTable DB_GenerRuningV2()
        {
            string userNo = WebUser.No;
            string fk_flow = null;
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            string sql;
            int state = (int)WFState.Runing;
            if (WebUser.IsAuthorize)
            {
                WF.Port.WFEmp emp = new Port.WFEmp(userNo);
                if (string.IsNullOrEmpty(fk_flow))
                    sql = "SELECT a.* FROM WF_GenerWorkFlow A, WF_GenerWorkerlist B WHERE A.WorkID=B.WorkID AND B.FK_Emp='" + userNo + "' AND B.IsEnable=1 AND B.IsPass=1 AND A.FK_Flow IN " + emp.AuthorFlows;
                else
                    sql = "SELECT a.* FROM WF_GenerWorkFlow A, WF_GenerWorkerlist B WHERE A.FK_Flow='" + fk_flow + "'  AND A.WorkID=B.WorkID AND B.FK_Emp='" + WebUser.No + "' AND B.IsEnable=1 AND B.IsPass=1 AND A.FK_Flow IN " + emp.AuthorFlows;
            }
            else
            {
                if (string.IsNullOrEmpty(fk_flow))
                    sql = "SELECT a.* FROM WF_GenerWorkFlow A, WF_GenerWorkerlist B WHERE A.WorkID=B.WorkID AND B.FK_Emp='" + userNo + "' AND B.IsEnable=1 AND B.IsPass=1 ";
                else
                    sql = "SELECT a.* FROM WF_GenerWorkFlow A, WF_GenerWorkerlist B WHERE A.FK_Flow='" + fk_flow + "'  AND A.WorkID=B.WorkID AND B.FK_Emp='" + userNo + "' AND B.IsEnable=1 AND B.IsPass=1 ";
            }
            return DBAccess.RunSQLReturnTable(sql);
        }
        /// <summary>
        ///  Get inside the system message 
        /// </summary>
        /// <param name="myPK"></param>
        /// <returns></returns>
        public static DataTable DB_GenerPopAlert(string type)
        {
            string sql = "";
            if (type == "unRead")
            {
                sql = "SELECT LEFT(CONVERT(VARCHAR(20),RDT,120),10) AS SortRDT,Datepart(WEEKDAY, CONVERT(DATETIME,RDT)  + @@DateFirst - 1) AS WeekRDT,"
                    + "* FROM Sys_SMS WHERE SendTo ='" + WebUser.No + "' AND (IsRead = 0 OR IsRead IS NULL)  ORDER BY RDT DESC";
            }
            else
            {
                sql = "SELECT LEFT(CONVERT(VARCHAR(20),RDT,120),10) AS SortRDT,Datepart(WEEKDAY, CONVERT(DATETIME,RDT)  + @@DateFirst - 1) AS WeekRDT,"
                    + "* FROM Sys_SMS WHERE SendTo ='" + WebUser.No + "'  ORDER BY RDT DESC";
            }
            return BP.DA.DBAccess.RunSQLReturnTable(sql);
        }

        /// <summary>
        ///  Get External System Messages 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="No"></param>
        /// <returns></returns>
        public static DataTable DB_GenerPopAlert(string type, string No)
        {
            string sql = "";
            if (type == "unRead")
            {
                sql = "SELECT LEFT(CONVERT(VARCHAR(20),RDT,120),10) AS SortRDT,Datepart(WEEKDAY, CONVERT(DATETIME,RDT)  + @@DateFirst - 1) AS WeekRDT,"
                    + "* FROM Sys_SMS WHERE SendTo ='" + No + "' AND (IsRead = 0 OR IsRead IS NULL)  ORDER BY RDT DESC";
            }
            else
            {
                sql = "SELECT LEFT(CONVERT(VARCHAR(20),RDT,120),10) AS SortRDT,Datepart(WEEKDAY, CONVERT(DATETIME,RDT)  + @@DateFirst - 1) AS WeekRDT,"
                    + "* FROM Sys_SMS WHERE SendTo ='" + No + "'  ORDER BY RDT DESC";
            }
            return BP.DA.DBAccess.RunSQLReturnTable(sql);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myPK"></param>
        public static DataTable DB_GenerUpdateMsgSta(string myPK)
        {
            string sql = "";
            sql = " UPDATE Sys_SMS SET IsRead=1 WHERE MyPK='" + myPK + "'";
            return BP.DA.DBAccess.RunSQLReturnTable(sql);
        }
        /// <summary>
        ///  Get unfinished process ( Also known as in-transit process : I participated in this process is not completed yet )
        /// </summary>
        /// <returns> Returns from the data view WF_GenerWorkflow Check out the data .</returns>
        public static DataTable DB_GenerRuning()
        {
            DataTable dt = DB_GenerRuning(BP.Web.WebUser.No, null);
            dt.Columns.Add("Type");

            foreach (DataRow row in dt.Rows)
            {
                row["Type"] = "RUNNING";
            }

            dt.DefaultView.Sort = "RDT DESC";
            return dt.DefaultView.ToTable();
        }
        /// <summary>
        ///  The information is also sent to CC 
        /// </summary>
        /// <returns></returns>
        public static DataTable DB_GenerRuningAndCC()
        {
            DataTable dt = DB_GenerRuning();
            DataTable ccDT = DB_CCList_CheckOver(WebUser.No);
            try
            {
                dt.Columns.Add("MyPK");
                dt.Columns.Add("Sta");
            }
            catch (Exception)
            {

            }

            foreach (DataRow row in ccDT.Rows)
            {
                DataRow newRow = dt.NewRow();

                foreach (DataColumn column in ccDT.Columns)
                {
                    foreach (DataColumn dtColumn in dt.Columns)
                    {
                        if (column.ColumnName == dtColumn.ColumnName)
                        {
                            newRow[column.ColumnName] = row[dtColumn.ColumnName];
                        }

                    }

                }
                newRow["Type"] = "CC";
                dt.Rows.Add(newRow);
            }
            dt.DefaultView.Sort = "RDT DESC";
            return dt.DefaultView.ToTable();
        }
        public static DataTable DB_GenerRuning3(string name, string fk_flow, string title)
        {
            DataTable dt = DB_GenerRuning2(name, fk_flow, title);

            dt.Columns.Add("Type");

            foreach (DataRow row in dt.Rows)
            {
                row["Type"] = "RUNNING";
            }

            dt.DefaultView.Sort = "RDT DESC";
            return dt.DefaultView.ToTable();
        }
        public static DataTable DB_GenerRuningAndCC2(string name, string fk_flow, string title)
        {
            DataTable dt = DB_GenerRuning3(name, fk_flow, title);
            DataTable ccDT = DB_CCList_CheckOver(WebUser.No);
            try
            {
                dt.Columns.Add("MyPK");
                dt.Columns.Add("Sta");
            }
            catch (Exception)
            {

            }

            foreach (DataRow row in ccDT.Rows)
            {
                DataRow newRow = dt.NewRow();

                foreach (DataColumn column in ccDT.Columns)
                {
                    foreach (DataColumn dtColumn in dt.Columns)
                    {
                        if (column.ColumnName == dtColumn.ColumnName)
                        {
                            newRow[column.ColumnName] = row[dtColumn.ColumnName];
                        }

                    }

                }
                newRow["Type"] = "CC";
                dt.Rows.Add(newRow);
            }
            dt.DefaultView.Sort = "RDT DESC";
            return dt.DefaultView.ToTable();
        }
        #endregion  Get the current operator of the shared work 

        #endregion

        #region  Login interface 
        /// <summary>
        ///  User Login , This method is easy to use after the developer check username and password to perform 
        /// </summary>
        /// <param name="userNo"> Username </param>
        /// <param name="SID"> Security ID, Refer to the operation manual Process Designer </param>
        public static void Port_Login(string userNo, string sid)
        {
            Paras ps = new Paras();
            ps.SQL = "SELECT SID FROM Port_Emp WHERE No=" + BP.Sys.SystemConfig.AppCenterDBVarStr + "No";
            ps.Add("No", userNo);
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(ps);
            if (dt.Rows.Count == 0)
                throw new Exception(" User does not exist or SID Error .");

            if (dt.Rows[0]["SID"].ToString() != sid)
                throw new Exception(" User does not exist or SID Error .");

            BP.Port.Emp emp = new BP.Port.Emp(userNo);
            WebUser.SignInOfGener(emp, true);
            WebUser.IsWap = false;
            return;
        }
        /// <summary>
        ///  User Login , This method is easy to use after the developer check username and password to perform 
        /// </summary>
        /// <param name="userNo"> Username </param>
        public static string Port_Login(string userNo)
        {
            BP.Port.Emp emp = new BP.Port.Emp(userNo);
            WebUser.SignInOfGener(emp, true);
            WebUser.IsWap = false;
            return Port_GetSID(userNo);
        }
        /// <summary>
        ///  Cancellation of the currently logged on 
        /// </summary>
        public static void Port_SigOut()
        {
            WebUser.Exit();
        }
        /// <summary>
        ///  Get unread messages 
        ///  For message alerts .
        /// </summary>
        /// <param name="userNo"> User ID</param>
        public static string Port_SMSInfo(string userNo)
        {
            Paras ps = new Paras();
            ps.SQL = "SELECT MyPK, EmailTitle  FROM sys_sms where SendTo=" + SystemConfig.AppCenterDBVarStr + "SendTo AND IsAlert=0";
            ps.Add("SendTo", userNo);
            DataTable dt = DBAccess.RunSQLReturnTable(ps);
            string strs = "";
            foreach (DataRow dr in dt.Rows)
            {
                strs += "@" + dr[0] + "=" + dr[1].ToString();
            }
            ps = new Paras();
            ps.SQL = "UPDATE  sys_sms SET IsAlert=1 WHERE  SendTo=" + SystemConfig.AppCenterDBVarStr + "SendTo AND IsAlert=0";
            ps.Add("SendTo", userNo);
            DBAccess.RunSQL(ps);
            return strs;
        }
        /// <summary>
        ///  Send a message 
        /// </summary>
        /// <param name="userNo"> Information Recipients </param>
        /// <param name="msgTitle"> Title </param>
        /// <param name="msgDoc"> Content </param>
        public static void Port_SendMsg(string userNo, string msgTitle, string msgDoc, string msgFlag)
        {
            Port_SendMsg(userNo, msgTitle, msgDoc, msgFlag, BP.WF.SMSMsgType.Self, null, 0, 0, 0);
        }
        /// <summary>
        ///  Get SID
        /// </summary>
        /// <param name="userNo"> User ID </param>
        /// <returns>SID</returns>
        public static string Port_GetSidName(string userNo)
        {
            string info = "";
            Paras ps = new Paras();
            ps.SQL = "SELECT SID, Name FROM Port_Emp WHERE No=" + BP.Sys.SystemConfig.AppCenterDBVarStr + "No";
            ps.Add("No", userNo);
            DataTable table = BP.DA.DBAccess.RunSQLReturnTable(ps);
            info = BP.Tools.FormatToJson.ToJson(table);

            return info;
        }
        /// <summary>
        ///  Get SID
        /// </summary>
        /// <param name="userNo"> User ID </param>
        /// <returns>SID</returns>
        public static string Port_GetSID(string userNo)
        {
            Paras ps = new Paras();
            ps.SQL = "SELECT SID FROM Port_Emp WHERE No=" + BP.Sys.SystemConfig.AppCenterDBVarStr + "No";
            ps.Add("No", userNo);
            string sid = BP.DA.DBAccess.RunSQLReturnString(ps);
            if (string.IsNullOrEmpty(sid) == true)
            {
                try
                {
                    sid = BP.DA.DBAccess.GenerGUID();
                    ps.SQL = Glo.UpdataSID;
                    ps.Add("SID", sid);
                    ps.Add("No", userNo);
                    BP.DA.DBAccess.RunSQL(ps);
                }
                catch
                {
                    // throw new Exception("@可");
                    /* There may be an update fails , Because the user view of the connection . */
                }
            }
            return sid;
        }
        /// <summary>
        ///  Set up SID
        /// </summary>
        /// <param name="userNo"> User ID </param>
        /// <param name="sid">SID Information </param>
        /// <returns>SID</returns>
        public static bool Port_SetSID(string userNo, string sid)
        {
            Paras ps = new Paras();
            ps.SQL = "UPDATE Port_Emp SET SID=" + BP.Sys.SystemConfig.AppCenterDBVarStr + "SID WHERE No=" + BP.Sys.SystemConfig.AppCenterDBVarStr + "No";
            ps.Add("SID", sid);
            ps.Add("No", userNo);
            if (BP.DA.DBAccess.RunSQL(ps) == 1)
                return true;
            else
                return false;
        }
        /// <summary>
        ///  Send e-mail with the message ( If the incoming 4 Process parameters will add a great job link )
        /// </summary>
        /// <param name="userNo"> Information Recipients </param>
        /// <param name="title"> Title </param>
        /// <param name="msgDoc"> Content </param>
        /// <param name="msgFlag"> Message Flags </param>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="nodeID"> Node ID</param>
        /// <param name="workID"> The work ID</param>
        /// <param name="fid">FID</param>
        public static void Port_SendMsg(string userNo, string title, string msgDoc, string msgFlag, string msgType,
            string flowNo, Int64 nodeID, Int64 workID, Int64 fid)
        {
            if (workID != 0)
            {
                string url = Glo.HostURL + "WF/Do.aspx?SID=" + userNo + "_" + workID + "_" + nodeID;
                url = url.Replace("//", "/");
                url = url.Replace("//", "/");

                msgDoc += " <hr> Open Work : " + url;
            }
            BP.WF.SMS.SendMsg(userNo, title, msgDoc, msgFlag, msgType);
        }
        /// <summary>
        ///  Send a message 
        /// </summary>
        /// <param name="sendTo"> Send to </param>
        /// <param name="mailTitle"> Title </param>
        /// <param name="mailDoc"> Content </param>
        /// <param name="smsInfo"> SMS information </param>
        /// <param name="msgFlag"> Message mark </param>
        /// <param name="msgType"> Message Type </param>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="nodeID"> Node number </param>
        /// <param name="workID"> The work ID</param>
        /// <param name="fid">FID</param>
        /// <param name="isEmail"> Whether to send mail </param>
        /// <param name="isSMS"> Whether to send text messages </param>
        public static void Port_SendMsg(string sendTo, string mailTitle, string mailDoc, string smsInfo, string msgFlag, string msgType,
            string flowNo, Int64 nodeID, Int64 workID, Int64 fid, bool isEmail, bool isSMS)
        {
            SMS sms = new SMS();
            sms.MyPK = DBAccess.GenerGUID();
            sms.HisEmaiSta = MsgSta.UnRun;
            sms.Sender = WebUser.No;
            sms.SendTo = sendTo;

            // Mail title .
            sms.Title = mailTitle;
            sms.DocOfEmail = mailDoc;
            if (isEmail == true)
                sms.HisEmaiSta = MsgSta.UnRun;
            else
                sms.HisEmaiSta = MsgSta.Disable;

            // SMS Properties .
            if (isSMS == true)
                sms.HisMobileSta = MsgSta.UnRun;
            else
                sms.HisMobileSta = MsgSta.Disable;

            sms.MobileInfo = smsInfo; // SMS information .

            //  Other properties .
            sms.Sender = BP.Web.WebUser.No;
            sms.RDT = BP.DA.DataType.CurrentDataTime;

            sms.MsgFlag = msgFlag; //  Message Flags .
            sms.MsgType = msgType; //  Message Type .
            sms.Insert();

            string sql = "UPDATE SYS_SMS SET () WHERE ";
        }
        /// <summary>
        ///  Send a message 
        /// </summary>
        /// <param name="mobileNum"> The phone number you </param>
        /// <param name="mobileInfo"> SMS information </param>
        /// <param name="emailAddress"> Mail </param>
        /// <param name="emilTitle"> Title </param>
        /// <param name="emailBody"> Message content </param>
        /// <param name="msgFlag"> Message mark , Can be empty .</param>
        public static void Port_SendMsg(string mobileNum, string mobileInfo, string emailAddress, string emilTitle, string emailBody, string msgFlag, string msgType)
        {
            BP.WF.SMS.SendMsg(mobileNum, mobileInfo, emailAddress, emilTitle, emailBody, msgFlag, msgType, null);
        }
        /// <summary>
        ///  Send a message 
        /// </summary>
        /// <param name="mobileNum"> The phone number you </param>
        /// <param name="mobileInfo"> SMS information </param>
        /// <param name="emailAddress"> Mail </param>
        /// <param name="emilTitle"> Title </param>
        /// <param name="emailBody"> Message content </param>
        /// <param name="msgFlag"> Message mark , Can be empty .</param>
        /// <param name="msgType"> Message Type , Can be empty .</param>
        public static void Port_SendMsg(string mobileNum, string mobileInfo, string emailAddress, string emilTitle, string emailBody,
            string msgFlag, string msgType, string guestNo)
        {
            BP.WF.SMS.SendMsg(mobileNum, mobileInfo, emailAddress, emilTitle, emailBody, msgFlag, msgType, guestNo);
        }
        /// <summary>
        ///  Conversion process Code To the process ID 
        /// </summary>
        /// <param name="FlowMark"> Process ID </param>
        /// <returns> Returns coding </returns>
        public static string TurnFlowMarkToFlowNo(string FlowMark)
        {
            if (string.IsNullOrEmpty(FlowMark))
                return "";

            //  If the number , Would not have transformed .
            if (DataType.IsNumStr(FlowMark))
                return FlowMark;

            string s = DBAccess.RunSQLReturnStringIsNull("SELECT No FROM WF_Flow WHERE FlowMark='" + FlowMark + "'", null);
            if (s == null)
                throw new Exception("@FlowMark Error :" + FlowMark + ", Did not find it flow numbers .");
            return s;
        }
        /// <summary>
        ///  Get the latest news 
        /// </summary>
        /// <param name="userNo"> User ID </param>
        /// <param name="dateLastTime"> Last time acquisition </param>
        /// <returns> Return message : Returns the data source with two columns MsgType,Num.</returns>
        public static DataTable Port_GetNewMsg(string userNo, string dateLastTime)
        {
            Paras ps = new Paras();
            ps.SQL = "SELECT MsgType , Count(*) as Num FROM Sys_SMS WHERE SendTo=" + BP.Sys.SystemConfig.AppCenterDBVarStr + "SendTo AND RDT >" + BP.Sys.SystemConfig.AppCenterDBVarStr + "RDT Group By MsgType";
            ps.Add(BP.WF.SMSAttr.SendTo, userNo);
            ps.Add(BP.WF.SMSAttr.RDT, dateLastTime);
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(ps);
            return dt;
        }
        /// <summary>
        ///  Get the latest news 
        /// </summary>
        /// <param name="userNo"> User ID </param>
        /// <returns></returns>
        public static DataTable Port_GetNewMsg(string userNo)
        {
            Paras ps = new Paras();
            ps.SQL = "SELECT MsgType , Count(*) as Num FROM Sys_SMS WHERE SendTo=" + BP.Sys.SystemConfig.AppCenterDBVarStr + "SendTo  Group By MsgType";
            ps.Add(BP.WF.SMSAttr.SendTo, userNo);
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(ps);
            return dt;
        }
        #endregion  Login interface 

        #region  And process-related interface 
        /// <summary>
        ///  Written to the log 
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="nodeFrom"> Node from </param>
        /// <param name="workid"> The work ID</param>
        /// <param name="fid">FID</param>
        /// <param name="msg"> Information </param>
        /// <param name="at"> Activity Type </param>
        /// <param name="tag"> Parameters :用@ Symbols separated by such , @PWorkID=101@PFlowNo=003</param>
        /// <param name="cFlowInfo"> Sub-process information </param>
        public static void WriteTrack(string flowNo, int nodeFrom, Int64 workid, Int64 fid, string msg, ActionType at, string tag,
            string cFlowInfo, string optionMsg)
        {
            if (at == ActionType.CallChildenFlow)
                if (string.IsNullOrEmpty(cFlowInfo) == true)
                    throw new Exception("@ Must enter a message cFlowInfo Information ,在 CallChildenFlow  Mode .");

            if (string.IsNullOrEmpty(optionMsg))
                optionMsg = Track.GetActionTypeT(at);

            Track t = new Track();
            t.WorkID = workid;
            t.FID = fid;
            t.RDT = DataType.CurrentDataTimess;
            t.HisActionType = at;
            t.ActionTypeText = optionMsg;

            Node nd = new Node(nodeFrom);
            t.NDFrom = nodeFrom;
            t.NDFromT = nd.Name;

            t.EmpFrom = WebUser.No;
            t.EmpFromT = WebUser.Name;
            t.FK_Flow = flowNo;

            t.NDTo = nodeFrom;
            t.NDToT = nd.Name;

            t.EmpTo = WebUser.No;
            t.EmpToT = WebUser.Name;
            t.Msg = msg;

            if (tag != null)
                t.Tag = tag;

            try
            {
                t.Insert();
            }
            catch
            {
                t.CheckPhysicsTable();
                t.Insert();
                t.DirectInsert();
            }

            #region  Special judge .
            if (at == ActionType.CallChildenFlow)
            {
                /*  If it is lifted subprocess , It is the parent process information necessary to write data , Let the parent process can be seen that the process can initiate data .*/
                AtPara ap = new AtPara(tag);
                BP.WF.GenerWorkFlow gwf = new GenerWorkFlow(ap.GetValInt64ByKey(GenerWorkFlowAttr.PWorkID));
                t.WorkID = gwf.WorkID;

                nd = new Node(gwf.FK_Node);
                t.NDFrom = gwf.FK_Node;
                t.NDFromT = nd.Name;

                t.NDTo = t.NDFrom;
                t.NDToT = t.NDFromT;

                t.FK_Flow = gwf.FK_Flow;

                t.HisActionType = ActionType.StartChildenFlow;
                t.Tag = "@CWorkID=" + workid + "@CFlowNo=" + flowNo;
                t.Msg = cFlowInfo;
                t.Insert();
            }
            #endregion  Special judge .
        }
        /// <summary>
        ///  Written to the log 
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="nodeFrom"> Node from </param>
        /// <param name="workid"> The work ID</param>
        /// <param name="fid">fID</param>
        /// <param name="msg"> Information </param>
        public static void WriteTrackInfo(string flowNo, int nodeFrom, Int64 workid, Int64 fid, string msg, string optionMsg)
        {
            WriteTrack(flowNo, nodeFrom, workid, fid, msg, ActionType.Info, null, null, optionMsg);
        }
        /// <summary>
        ///  Write operation audit log :
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="nodeID"> Node from </param>
        /// <param name="workid"> The work ID</param>
        /// <param name="FID">FID</param>
        /// <param name="msg"> Audit information </param>
        /// <param name="optionName"> Action Name ( Such as : Chief Audit , Department manager for approval ), That is, if it is empty " Check ".</param>
        public static void WriteTrackWorkCheck(string flowNo, int nodeFrom, Int64 workid, Int64 fid, string msg, string optionName)
        {
            string timeLine = DataType.CurrentDataTime;
            BP.DA.DBAccess.RunSQL("DELETE FROM ND" + int.Parse(flowNo) + "Track WHERE WorkID=" + workid + " AND NDFrom=" + nodeFrom + " AND EmpFrom='" + WebUser.No + "' AND ActionType=" + (int)ActionType.WorkCheck);
            //  Written to the log .
            WriteTrack(flowNo, nodeFrom, workid, fid, msg, ActionType.WorkCheck, null, null, optionName);

        }
        /// <summary>
        ///  Modify audit information to identify 
        ///  Such as : In case of default is [ Check ], Now put ActionTypeText  Modified [ Audit Leader .].
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="workid"> The work ID</param>
        /// <param name="nodeFrom"> Node ID</param>
        /// <param name="label"> To modify a label </param>
        /// <returns> Success </returns>
        public static bool WriteTrackWorkCheckLabel(string flowNo, Int64 workid, int nodeFrom, string label)
        {
            string table = "ND" + int.Parse(flowNo) + "Track";
            string sql = "SELECT MyPK FROM " + table + " WHERE NDFrom=" + nodeFrom + " AND WorkID=" + workid + " And NDTo='0' ORDER BY RDT DESC ";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
                return false;

            string pk = dt.Rows[0][0].ToString();
            sql = "UPDATE " + table + " SET " + TrackAttr.ActionTypeText + "='" + label + "' WHERE MyPK=" + pk;
            BP.DA.DBAccess.RunSQL(sql);
            return true;
        }

        /// <summary>
        ///  Go ahead , Get other labels 
        ///  Such as : In case of default is [ Tombstone ], Now put ActionTypeText  Modified [ Delete ( Clearance ).].
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="workid"> The work ID</param>
        /// <param name="nodeFrom"> Node ID</param>
        /// <param name="label"> To modify a label </param>
        /// <returns> Success </returns>
        public static bool WriteTrackLabel(string flowNo, Int64 workid, int nodeFrom, string label)
        {
            string table = "ND" + int.Parse(flowNo) + "Track";
            string sql = "SELECT MyPK FROM " + table + " WHERE NDFrom=" + nodeFrom + " AND WorkID=" + workid + "  ORDER BY RDT DESC ";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
                return false;

            string pk = dt.Rows[0][0].ToString();
            sql = "UPDATE " + table + " SET " + TrackAttr.ActionTypeText + "='" + label + "' WHERE MyPK=" + pk;
            BP.DA.DBAccess.RunSQL(sql);
            return true;
        }
        /// <summary>
        ///  Get Track  Table audit information 
        /// </summary>
        /// <param name="flowNo"></param>
        /// <param name="workId"></param>
        /// <param name="nodeFrom"></param>
        /// <returns></returns>
        public static string GetCheckInfo(string flowNo, Int64 workId, int nodeFrom)
        {
            string table = "ND" + int.Parse(flowNo) + "Track";
            string sql = "SELECT Msg FROM " + table + " WHERE NDFrom=" + nodeFrom + " AND ActionType=" + (int)ActionType.WorkCheck + " AND EmpFrom='" + WebUser.No + "' AND WorkID=" + workId + " ORDER BY RDT DESC ";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
            {
                //BP.Sys.FrmWorkCheck fwc = new FrmWorkCheck(nodeFrom);
                //return fwc.FWCDefInfo;
                return null;
            }
            string checkinfo = dt.Rows[0][0].ToString();
            return checkinfo;
        }
        /// <summary>
        ///  Delete audit information , To return after .
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="workId"> The work ID</param>
        /// <param name="nodeFrom"> Node from </param>
        /// <returns></returns>
        public static void DeleteCheckInfo(string flowNo, Int64 workId, int nodeFrom)
        {
            string table = "ND" + int.Parse(flowNo) + "Track";
            string sql = "DELETE FROM " + table + " WHERE NDFrom=" + nodeFrom + " AND ActionType=" + (int)ActionType.WorkCheck + " AND EmpFrom='" + WebUser.No + "' AND WorkID=" + workId;
            BP.DA.DBAccess.RunSQL(sql);
        }
        public static string GetAskForHelpReInfo(string flowNo, Int64 workId, int nodeFrom)
        {
            string table = "ND" + int.Parse(flowNo) + "Track";
            string sql = "SELECT Msg FROM " + table + " WHERE NDFrom=" + nodeFrom + " AND ActionType=" + (int)ActionType.AskforHelp + " AND EmpFrom='" + WebUser.No + "' AND WorkID=" + workId + " ORDER BY RDT DESC ";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
                return "";
            string checkinfo = dt.Rows[0][0].ToString();
            return checkinfo;
        }

        /// <summary>
        ///  Update Track Information 
        /// </summary>
        /// <param name="flowNo"></param>
        /// <param name="workId"></param>
        /// <param name="nodeFrom"></param>
        /// <param name="actionType"></param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public static void SetTrackInfo(string flowNo, Int64 workId, int nodeFrom, int actionType, string strMsg)
        {
            string table = "ND" + int.Parse(flowNo) + "Track";

            string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
            Paras ps = new Paras();
            ps.SQL = "UPDATE " + table + " SET Msg=" + dbstr + "Msg  WHERE ActionType=" + dbstr +
                "ActionType and WorkID=" + dbstr + "WorkID and NDFrom=" + dbstr + "NDFrom";
            ps.Add("Msg", strMsg);
            ps.Add("ActionType", actionType);
            ps.Add("WorkID", workId);
            ps.Add("NDFrom", nodeFrom);
            BP.DA.DBAccess.RunSQL(ps);
        }

        /// <summary>
        ///  Set up BillNo Information 
        /// </summary>
        /// <param name="flowNo"></param>
        /// <param name="workID"></param>
        /// <param name="newBillNo"></param>
        public static void SetBillNo(string flowNo, Int64 workID, string newBillNo)
        {
            string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
            Paras ps = new Paras();
            ps.SQL = "UPDATE WF_GenerWorkFlow SET BillNo=" + dbstr + "BillNo  WHERE WorkID=" + dbstr + "WorkID";
            ps.Add("BillNo", newBillNo);
            ps.Add("WorkID", workID);
            BP.DA.DBAccess.RunSQL(ps);

            Flow fl = new Flow(flowNo);
            ps = new Paras();
            ps.SQL = "UPDATE " + fl.PTable + " SET BillNo=" + dbstr + "BillNo WHERE OID=" + dbstr + "OID";
            ps.Add("BillNo", newBillNo);
            ps.Add("OID", workID);
            BP.DA.DBAccess.RunSQL(ps);
        }
        /// <summary>
        ///  Set the parent process information 
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="workID"> The work ID</param>
        /// <param name="parentFlowNo"> Parent process ID </param>
        /// <param name="parentFlowWorkID"> Parent process WorkID</param>
        public static void SetParentInfo(string flowNo, Int64 workID, string parentFlowNo, Int64 parentWorkID,
            int parentNodeID, string parentEmp)
        {
            if (parentWorkID == 0)
                throw new Exception("@ Set the parent process ID of the process 0, It is illegal .");

            if (string.IsNullOrEmpty(parentEmp))
                parentEmp = WebUser.No;

            string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
            Paras ps = new Paras();
            ps.SQL = "UPDATE WF_GenerWorkFlow SET PFlowNo=" + dbstr + "PFlowNo, PWorkID=" + dbstr + "PWorkID,PNodeID=" + dbstr + "PNodeID,PEmp=" + dbstr + "PEmp WHERE WorkID=" + dbstr + "WorkID";
            ps.Add(GenerWorkFlowAttr.PFlowNo, parentFlowNo);
            ps.Add(GenerWorkFlowAttr.PWorkID, parentWorkID);
            ps.Add(GenerWorkFlowAttr.PNodeID, parentNodeID);
            ps.Add(GenerWorkFlowAttr.PEmp, parentEmp);
            ps.Add(GenerWorkFlowAttr.WorkID, workID);

            BP.DA.DBAccess.RunSQL(ps);

            Flow fl = new Flow(flowNo);
            ps = new Paras();
            ps.SQL = "UPDATE " + fl.PTable + " SET PFlowNo=" + dbstr + "PFlowNo, PWorkID=" + dbstr + "PWorkID,PNodeID=" + dbstr + "PNodeID, PEmp=" + dbstr + "PEmp WHERE OID=" + dbstr + "OID";
            ps.Add(NDXRptBaseAttr.PFlowNo, parentFlowNo);
            ps.Add(NDXRptBaseAttr.PWorkID, parentWorkID);
            ps.Add(NDXRptBaseAttr.PNodeID, parentNodeID);
            ps.Add(NDXRptBaseAttr.PEmp, parentEmp);

            ps.Add(NDXRptBaseAttr.OID, workID);
            BP.DA.DBAccess.RunSQL(ps);
        }

        public static GERpt Flow_GenerGERpt(string flowNo, Int64 workID)
        {
            //  Converted into numbers .
            flowNo = TurnFlowMarkToFlowNo(flowNo);

            GERpt rpt = new GERpt("ND" + int.Parse(flowNo) + "Rpt", workID);
            return rpt;
        }
        /// <summary>
        ///  Generating a new work ID
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <returns> Return to work created by the current operator ID</returns>
        public static Int64 Flow_GenerWorkID(string flowNo)
        {
            //  Converted into numbers .
            flowNo = TurnFlowMarkToFlowNo(flowNo);

            Flow fl = new Flow(flowNo);
            return fl.NewWork().OID;
        }
        /// <summary>
        ///  Generating a new work 
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <returns> Return to work created by the current operator </returns>
        public static Work Flow_GenerWork(string flowNo)
        {
            //  Converted into numbers .
            flowNo = TurnFlowMarkToFlowNo(flowNo);

            Flow fl = new Flow(flowNo);
            Work wk = fl.NewWork();
            wk.ResetDefaultVal();
            return wk;
        }
        /// <summary>
        ///  The process to recover from abnormal operating state to the normal operating state 
        ///  For example, the state of the current process is , Delete , Pending , Now restored to normal operation .
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="workID"> The work ID</param>
        /// <param name="msg"> The reason </param>
        /// <returns> Execution information </returns>
        public static void Flow_DoComeBackWorkFlow(string flowNo, Int64 workID, string msg)
        {
            //  Converted into numbers .
            flowNo = TurnFlowMarkToFlowNo(flowNo);

            WorkFlow wf = new WorkFlow(flowNo, workID);
            wf.DoComeBackWorkFlow(msg);
        }
        /// <summary>
        ///  Data recovery process has been completed to a specified node , If the node is 0 Was restored to the last completed node up .
        ///  Restore failed thrown 
        /// </summary>
        /// <param name="flowNo"> To restore the flow numbers </param>
        /// <param name="workid"> To be restored workid</param>
        /// <param name="backToNodeID"> Restored to the node number , In the case of 0, Flag last node in response to the process up .</param>
        /// <param name="note"> Reasons for recovery , For this reason will be logged .</param>
        public static string Flow_DoRebackWorkFlow(string flowNo, Int64 workid,
            int backToNodeID, string note)
        {
            BP.WF.Template.Ext.FlowSheet fs = new BP.WF.Template.Ext.FlowSheet(flowNo);
            return fs.DoRebackFlowData(workid, backToNodeID, note);
        }
        /// <summary>
        ///  Delete process : Completely delete process .
        ///  Clear reads as follows :
        /// 1,  Data flow engine .
        /// 2,  Node data ,NDxxRpt Data .
        /// 3,  Track table data .
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="workID"> The work ID</param>
        /// <param name="isDelSubFlow"> Do you want to delete its child processes </param>
        /// <returns> Execution information </returns>
        public static string Flow_DoDeleteFlowByReal(string flowNo, Int64 workID, bool isDelSubFlow)
        {
            //  Converted into numbers .
            flowNo = TurnFlowMarkToFlowNo(flowNo);
            try
            {
                WorkFlow.DeleteFlowByReal(flowNo, workID, isDelSubFlow);
                // WorkFlow wf = new WorkFlow(flowNo, workID);
                //wf.DoDeleteWorkFlowByReal(isDelSubFlow);
            }
            catch (Exception ex)
            {
                throw new Exception("@ Before deleting error ," + ex.StackTrace);
            }
            return " Deleted successfully ";
        }
        public static string Flow_DoDeleteDraft(string flowNo, Int64 workID, bool isDelSubFlow)
        {
            //  Converted into numbers .
            flowNo = TurnFlowMarkToFlowNo(flowNo);

            string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
            Paras ps = new Paras();
            ps.SQL = "DELETE WF_GenerWorkFlow WHERE WorkID=" + dbstr + "WorkID";
            ps.Add("WorkID", workID);
            BP.DA.DBAccess.RunSQL(ps);

            Flow fl = new Flow(flowNo);
            ps = new Paras();
            ps.SQL = "DELETE " + fl.PTable + " WHERE OID=" + dbstr + "OID";
            ps.Add("OID", workID);
            BP.DA.DBAccess.RunSQL(ps);

            // Delete the starting node data .
            Node nd = fl.HisStartNode;
            Work wk = nd.HisWork;
            ps = new Paras();
            ps.SQL = "DELETE " + wk.EnMap.PhysicsTable + " WHERE OID=" + dbstr + "OID";
            ps.Add("OID", workID);
            BP.DA.DBAccess.RunSQL(ps);

            BP.DA.Log.DefaultLogWriteLineInfo(WebUser.Name + " Removed FlowNo 为'" + flowNo + "',workID为'" + workID + "' Data ");

            return " Deleted successfully ";
        }
        /// <summary>
        ///  Deletion process has been completed 
        ///  Watch out : It does not trigger events .
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="workID"> The work ID</param>
        /// <param name="isDelSubFlow"> Delete sub-processes </param>
        /// <param name="note"> Reason for deletion </param>
        /// <returns> Removal process information </returns>
        public static string Flow_DoDeleteWorkFlowAlreadyComplete(string flowNo, Int64 workID, bool isDelSubFlow, string note)
        {
            return WorkFlow.DoDeleteWorkFlowAlreadyComplete(flowNo, workID, isDelSubFlow, note);
        }
        /// <summary>
        ///  Delete the process and written to the log 
        ///  Clear reads as follows :
        /// 1,  Data flow engine .
        /// 2,  Node data ,NDxxRpt Data .
        ///  And treated as follows :
        /// 1,  Retention track Data .
        /// 2,  Write process delete records table .
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="workID"> The work ID</param>
        /// <param name="deleteNote"> Reason for deletion </param>
        /// <param name="isDelSubFlow"> Do you want to delete its child processes </param>
        /// <returns> Execution information </returns>
        public static string Flow_DoDeleteFlowByWriteLog(string flowNo, Int64 workID, string deleteNote, bool isDelSubFlow)
        {
            //  Converted into numbers .
            flowNo = TurnFlowMarkToFlowNo(flowNo);
            WorkFlow wf = new WorkFlow(flowNo, workID);
            return wf.DoDeleteWorkFlowByWriteLog(deleteNote, isDelSubFlow);
        }
        /// <summary>
        ///  Tombstone execution process : This process is not really made the process removes only marked for deletion 
        ///  Such as : Tombstone Ticket .
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="workID"> The work ID</param>
        /// <param name="msg"> The reason tombstoned </param>
        /// <param name="isDelSubFlow"> The reason tombstoned </param>
        /// <returns> Execution information , Unsuccessful thrown .</returns>
        public static string Flow_DoDeleteFlowByFlag(string flowNo, Int64 workID, string msg, bool isDelSubFlow)
        {
            // Converted into numbers .
            flowNo = TurnFlowMarkToFlowNo(flowNo);

            WorkFlow wf = new WorkFlow(flowNo, workID);
            wf.DoDeleteWorkFlowByFlag(msg);
            if (isDelSubFlow)
            {
                GenerWorkFlows gwfs = new GenerWorkFlows();
                gwfs.Retrieve(GenerWorkFlowAttr.PWorkID, workID);
                foreach (GenerWorkFlow item in gwfs)
                {
                    Flow_DoDeleteFlowByFlag(item.FK_Flow, item.WorkID, " Delete sub-processes :" + msg, false);
                }
            }
            return " Deleted successfully ";
        }
        /// <summary>
        ///  Undelete Process 
        ///  Explanation : If a process is in the tombstone state , To return to normal operating status , On the implementation of this interface .
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="workID"> Workflow ID</param>
        /// <param name="msg"> The reason undelete </param>
        /// <returns> Perform message , If an exception is thrown unsuccessful revocation .</returns>
        public static string Flow_DoUnDeleteFlowByFlag(string flowNo, Int64 workID, string msg)
        {
            //  Converted into numbers .
            flowNo = TurnFlowMarkToFlowNo(flowNo);

            WorkFlow wf = new WorkFlow(flowNo, workID);
            wf.DoUnDeleteWorkFlowByFlag(msg);
            return " Undelete success .";
        }

        /// <summary>
        ///  Carried out - Send revocation 
        ///  Explanation : If the process proceeds to the next node , Will perform failure , Will throw an exception .
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="workID"> The work ID</param>
        /// <returns> Return to the successful implementation of information </returns>
        public static string Flow_DoUnSend(string flowNo, Int64 workID)
        {
            //  Converted into numbers .
            flowNo = TurnFlowMarkToFlowNo(flowNo);

            WorkUnSend unSend = new WorkUnSend(flowNo, workID);
            return unSend.DoUnSend();
        }
        /// <summary>
        ///  Execution freeze 
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="workid">workid</param>
        /// <param name="msg"> Blocking Reasons </param>
        public static string Flow_DoFix(string flowNo, Int64 workid, string msg)
        {
            //  Converted into numbers .
            flowNo = TurnFlowMarkToFlowNo(flowNo);

            //  Execution freeze .
            WorkFlow wf = new WorkFlow(flowNo, workid);
            return wf.DoFix(msg);
        }
        /// <summary>
        ///  Execution unfreeze 
        ///  The difference is in the pending , People need to have permission to freeze can perform unfrozen ,
        ///  Hang their own work can also be lifted pending pending .
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="workid">workid</param>
        /// <param name="msg"> Lifting reason </param>
        public static string Flow_DoUnFix(string flowNo, Int64 workid, string msg)
        {
            //  Converted into numbers .
            flowNo = TurnFlowMarkToFlowNo(flowNo);

            //  Execution freeze .
            WorkFlow wf = new WorkFlow(flowNo, workid);
            return wf.DoUnFix(msg);
        }
        /// <summary>
        ///  Execution process ends 
        ///  Explanation : Normal flow ends .
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="workID"> The work ID</param>
        /// <param name="msg"> The reason the process ends </param>
        /// <returns> Return to the successful implementation of information </returns>
        public static string Flow_DoFlowOver(string flowNo, Int64 workID, string msg)
        {
            //  Converted into numbers .
            flowNo = TurnFlowMarkToFlowNo(flowNo);

            WorkFlow wf = new WorkFlow(flowNo, workID);
            return wf.DoFlowOver(ActionType.FlowOver, msg, null, null);
        }
        /// <summary>
        ///  Execution process ends : Compulsory process ends .
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="flowNo"> Current node number </param>
        /// <param name="workID"> The work ID</param>
        /// <param name="fid"> The work ID</param>
        /// <param name="msg"> The reason to force the process ends </param>
        /// <returns> Perform a forced end process </returns>
        public static string Flow_DoFlowOverByCoercion(string flowNo, int nodeid, Int64 workID, Int64 fid, string msg)
        {
            //  Converted into numbers .
            flowNo = TurnFlowMarkToFlowNo(flowNo);
            WorkFlow wf = new WorkFlow(flowNo, workID);

            Node currND = new Node(nodeid);

            Flow fl = new Flow(flowNo);
            GERpt rpt = fl.HisGERpt;
            rpt.OID = workID;
            rpt.RetrieveFromDBSources();

            string s = wf.DoFlowOver(ActionType.FlowOverByCoercion, msg, currND, rpt);
            if (string.IsNullOrEmpty(s))
                s = " The process has been successfully completed .";
            return s;
        }
        /// <summary>
        ///  Obtaining node to the next step of ID, This function is known in advance before the process unsent 
        ///  It is necessary to reach that one node up , To facilitate the sending node before the current business logic .
        /// 1, First, ensure that the current staff can perform the work of the current node .
        /// 2, Second, ensure that only one to get the next node .
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="workid"> The work ID</param>
        /// <returns> The next step to reach the node ,  If you get less than it will throw an exception .</returns>
        public static int Node_GetNextStepNode(string fk_flow, Int64 workid)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            //// Check that the current staff can execute the current work .
            //if (BP.WF.Dev2Interface.Flow_CheckIsCanDoCurrentWork( workid, WebUser.No) == false)
            //    throw new Exception("@ The current staff can not perform work on this node .");

            // Get current nodeID.
            int currNodeID = BP.WF.Dev2Interface.Node_GetCurrentNodeID(fk_flow, workid);

            // Get 
            Node nd = new Node(currNodeID);
            Work wk = nd.HisWork;
            wk.OID = workid;
            wk.Retrieve();

            WorkNode wn = new WorkNode(wk, nd);
            return wn.NodeSend_GenerNextStepNode().NodeID;
        }
        /// <summary>
        ///  Gets the specified workid  In the run to the node number 
        /// </summary>
        /// <param name="workID"> Need to find workid</param>
        /// <returns> Returns the node number .  If you do not find , Will throw an exception .</returns>
        public static int Flow_GetCurrentNode(Int64 workID)
        {
            Paras ps = new Paras();
            ps.SQL = "SELECT FK_Node FROM WF_GenerWorkFlow WHERE WorkID=" + SystemConfig.AppCenterDBVarStr + "WorkID";
            ps.Add("WorkID", workID);
            return BP.DA.DBAccess.RunSQLReturnValInt(ps);
        }
        /// <summary>
        ///  Gets the specified node Work
        /// </summary>
        /// <param name="nodeID"> Node ID</param>
        /// <param name="workID"> The work ID</param>
        /// <returns> Current work </returns>
        public static Work Flow_GetCurrentWork(int nodeID, Int64 workID)
        {
            Node nd = new Node(nodeID);
            Work wk = nd.HisWork;
            wk.OID = workID;
            wk.Retrieve();
            return wk;
        }
        /// <summary>
        ///  Get the current work node Work
        /// </summary>
        /// <param name="workID"> The work ID</param>
        /// <returns> The current work node Work</returns>
        public static Work Flow_GetCurrentWork(Int64 workID)
        {
            Node nd = new Node(Flow_GetCurrentNode(workID));
            Work wk = nd.HisWork;
            wk.OID = workID;
            wk.Retrieve();
            wk.ResetDefaultVal();
            return wk;
        }
        /// <summary>
        ///  Designation  workid  The current node can be performed by whom .
        /// </summary>
        /// <param name="workID"> Need to find workid</param>
        /// <returns> Returns the current list of personnel , Data Structures and WF_GenerWorkerList Consistency .</returns>
        public static DataTable Flow_GetWorkerList(Int64 workID)
        {
            Paras ps = new Paras();
            ps.SQL = "SELECT * FROM WF_GenerWorkerList WHERE IsEnable=1 AND IsPass=0 AND WorkID=" + SystemConfig.AppCenterDBVarStr + "WorkID";
            ps.Add("WorkID", workID);
            return BP.DA.DBAccess.RunSQLReturnTable(ps);
        }
        /// <summary>
        ///  Check whether the process can be initiated 
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="userNo"> User ID </param>
        /// <returns> Whether the current process can be initiated </returns>
        public static bool Flow_IsCanStartThisFlow(string flowNo, string userNo)
        {
            Node nd = new Node(int.Parse(flowNo + "01"));
            Paras ps = new Paras();
            string dbstr = SystemConfig.AppCenterDBVarStr;
            int num = 0;
            switch (nd.HisDeliveryWay)
            {
                case DeliveryWay.ByStation:
                    ps.SQL = "SELECT COUNT(*) AS Num FROM WF_NodeStation WHERE FK_Station IN (SELECT FK_Station FROM Port_EmpStation WHERE FK_Emp=" + dbstr + "FK_Emp) AND FK_Node=" + dbstr + "FK_Node";
                    ps.Add("FK_Emp", userNo);
                    ps.Add("FK_Node", nd.NodeID);
                    num = DBAccess.RunSQLReturnValInt(ps);
                    break;
                case DeliveryWay.ByDept:
                    ps.SQL = "SELECT COUNT(*) AS Num FROM WF_NodeDept WHERE FK_Dept IN (SELECT FK_Dept FROM Port_EmpDept WHERE FK_Emp=" + dbstr + "FK_Emp) AND FK_Node=" + dbstr + "FK_Node";
                    ps.Add("FK_Emp", userNo);
                    ps.Add("FK_Node", nd.NodeID);
                    num = DBAccess.RunSQLReturnValInt(ps);
                    break;
                case DeliveryWay.ByBindEmp:
                    ps.SQL = "SELECT COUNT(*) AS Num FROM WF_NodeEmp WHERE FK_Emp=" + dbstr + "FK_Emp AND FK_Node=" + dbstr + "FK_Node";
                    ps.Add("FK_Emp", userNo);
                    ps.Add("FK_Node", nd.NodeID);
                    num = DBAccess.RunSQLReturnValInt(ps);
                    break;
                default:
                    throw new Exception("@ Start node is not allowed to set this access rule :" + nd.HisDeliveryWay);
            }
            if (num == 0)
                return false;
            return true;
        }
        /// <summary>
        ///  Get the running number of sub-processes 
        /// </summary>
        /// <param name="workID"> Parent process workid</param>
        /// <returns> Get the running number of sub-processes . In the case of 0, Means that all processes have been completed sub-processes .</returns>
        public static int Flow_NumOfSubFlowRuning(Int64 pWorkID)
        {
            string sql = "SELECT COUNT(*) AS num FROM WF_GenerWorkFlow WHERE WFState!=" + (int)WFState.Complete + " AND PWorkID=" + pWorkID;
            return DBAccess.RunSQLReturnValInt(sql);
        }
        /// <summary>
        ///  Get the running number of sub-processes 
        /// </summary>
        /// <param name="pWorkID"> Parent process workid</param>
        /// <param name="currWorkID"> Node does not contain the current work ID</param>
        /// <param name="workID"> Parent process workid</param>
        /// <returns> Get the running number of sub-processes . In the case of 0, Means that all processes have been completed sub-processes .</returns>
        public static int Flow_NumOfSubFlowRuning(Int64 pWorkID, Int64 currWorkID)
        {
            string sql = "SELECT COUNT(*) AS num FROM WF_GenerWorkFlow WHERE WFState!=" + (int)WFState.Complete + " AND WorkID!=" + currWorkID + " AND PWorkID=" + pWorkID;
            return DBAccess.RunSQLReturnValInt(sql);
        }
        public static bool Flow_IsInGenerWork(Int64 workID)
        {

            if (workID == 0)
                return false;

            string sql = "select * from WF_Generworkflow where WorkID='" + workID + "'";


            return DBAccess.RunSQLReturnCOUNT(sql) > 0;
        }
        /// <summary>
        ///  Whether it can handle the current work 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="workID"> The work ID</param>
        /// <param name="userNo"> User ID </param>
        /// <returns> Whether it can handle the current work </returns>
        public static bool Flow_IsCanDoCurrentWork(string fk_flow, Int64 workID, string userNo)
        {
            try
            {
                GenerWorkFlow gwf = new GenerWorkFlow(workID);
                return Flow_IsCanDoCurrentWork(fk_flow, gwf.FK_Node, workID, userNo);
            }
            catch
            {
                return false;
            }
        }
        public static bool Flow_IsCanDoCurrentWork(int nodeID, Int64 workID, string userNo)
        {
            Node nd = new Node(nodeID);
            return Flow_IsCanDoCurrentWork(nd.FK_Flow, nodeID, workID, userNo);
        }
        /// <summary>
        ///  Check whether the current permission to deal with the current personnel work .
        /// </summary>
        /// <param name="nodeID"> Node ID</param>
        /// <param name="workID"> The work ID</param>
        /// <param name="userNo"> The operator must determine the </param>
        /// <returns> Returns whether the specified personnel operating current work rights </returns>
        public static bool Flow_IsCanDoCurrentWork(string fk_flow, int nodeID, Int64 workID, string userNo)
        {
            if (workID == 0)
                return true;

            if (userNo == "admin")
                return true;


            #region  Determine whether it is the starting node .
            /*  Determine whether it is the starting node  . */
            string str = nodeID.ToString();
            int len = str.Length - 2;
            if (str.Substring(len, 2) == "01")
                return true;
            #endregion  Determine whether it is the starting node .

            string dbstr = SystemConfig.AppCenterDBVarStr;
            Paras ps = new Paras();
            string sql = "SELECT c.RunModel, a.TaskSta, a.WFState, IsPass FROM WF_GenerWorkFlow a , WF_GenerWorkerlist b, WF_Node c WHERE a.FK_Node='" + nodeID + "'  AND b.FK_Node=c.NodeID AND a.WorkID=b.WorkID AND a.FK_Node=b.FK_Node  AND b.FK_Emp='" + userNo + "' AND b.IsEnable=1 AND a.workid='" + workID + "'";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
                return false;

            // Determine whether the to-do .
            int isPass = int.Parse(dt.Rows[0]["IsPass"].ToString());
            if (isPass != 0)
                return false;

            WFState wfsta = (WFState)int.Parse(dt.Rows[0][2].ToString());
            if (wfsta == WFState.Complete)
                return false;
            if (wfsta == WFState.Delete)
                return false;

            int i = int.Parse(dt.Rows[0][0].ToString());
            TaskSta TaskStai = (TaskSta)int.Parse(dt.Rows[0][1].ToString());
            if (TaskStai == TaskSta.Sharing)
                return false;

            RunModel rm = (RunModel)i;
            switch (rm)
            {
                case RunModel.Ordinary:
                    return true;
                case RunModel.FL:
                    return true;
                case RunModel.HL:
                    return true;
                case RunModel.FHL:
                    return true;
                case RunModel.SubThread:
                    return true;
                default:
                    break;
            }
            return true;
        }
        /// <summary>
        ///  Check whether the current permission to deal with the current personnel work .
        /// </summary>
        /// <param name="nodeID"> Node ID</param>
        /// <param name="workID"> The work ID</param>
        /// <param name="userNo"> The operator must determine the </param>
        /// <returns> Returns whether the specified personnel operating current work rights </returns>
        public static bool Flow_IsCanDoCurrentWorkGuest(int nodeID, Int64 workID, string userNo)
        {
            if (workID == 0)
                return true;

            if (userNo == "admin")
                return true;

            string dbstr = SystemConfig.AppCenterDBVarStr;
            Paras ps = new Paras();
            //ps.SQL = "SELECT c.RunModel FROM WF_GenerWorkFlow a , WF_GenerWorkerlist b, WF_Node c WHERE a.FK_Node=" + dbstr + "FK_Node AND b.FK_Node=c.NodeID AND a.WorkID=b.WorkID AND a.FK_Node=b.FK_Node  AND b.FK_Emp=" + dbstr + "FK_Emp AND b.IsEnable=1 AND a.workid=" + dbstr + "WorkID";
            //ps.Add("FK_Node", nodeID);
            //ps.Add("FK_Emp", userNo);
            //ps.Add("WorkID", workID);
            string sql = "SELECT c.RunModel, a.TaskSta FROM WF_GenerWorkFlow a , WF_GenerWorkerlist b, WF_Node c WHERE a.FK_Node='" + nodeID + "'  AND b.FK_Node=c.NodeID AND a.WorkID=b.WorkID AND a.FK_Node=b.FK_Node  AND b.GuestNo='" + userNo + "' AND b.IsEnable=1 AND a.workid='" + workID + "'";

            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
                return false;

            int i = int.Parse(dt.Rows[0][0].ToString());
            TaskSta TaskStai = (TaskSta)int.Parse(dt.Rows[0][1].ToString());
            if (TaskStai == TaskSta.Sharing)
                return false;

            RunModel rm = (RunModel)i;
            switch (rm)
            {
                case RunModel.Ordinary:
                    return true;
                case RunModel.FL:
                    return true;
                case RunModel.HL:
                    return true;
                case RunModel.FHL:
                    return true;
                case RunModel.SubThread:
                    return true;
                default:
                    break;
            }

            if (DBAccess.RunSQLReturnValInt(ps) == 0)
                return false;
            return true;
        }
        /// <summary>
        ///  Check that the current staff have permission to view the specified process 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="workID"> The work ID</param>
        /// <param name="userNo"> User ID </param>
        /// <returns> Returns whether you can view </returns>
        public static bool Flow_IsCanViewCurrentWork(string fk_flow, Int64 workID, string userNo)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            string dbstr = SystemConfig.AppCenterDBVarStr;
            Paras ps = new Paras();
            ps.SQL = "SELECT COUNT(*) FROM ND" + int.Parse(fk_flow) + "Track WHERE WorkID=" + dbstr + "WorkID AND (EmpFrom=" + dbstr + "user1 OR EmpTo=" + dbstr + "user2)";
            ps.Add("WorkID", workID);
            ps.Add("user1", userNo);
            ps.Add("user2", userNo);
            if (DBAccess.RunSQLReturnValInt(ps) == 0)
                return false;
            return true;
        }

        /// <summary>
        ///  Remove the child thread 
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="workid"> Sub-thread work ID</param>
        public static void Flow_DeleteSubThread(string flowNo, Int64 workid)
        {
            WorkFlow wf = new WorkFlow(flowNo, workid);
            wf.DoDeleteWorkFlowByReal(true);
        }
        /// <summary>
        ///  Implementation reminders 
        /// </summary>
        /// <param name="workID"> The work ID</param>
        /// <param name="msg"> Reminders news </param>
        /// <param name="isPressSubFlow"> Are reminders sub-processes ?</param>
        /// <returns> Return to the results </returns>
        public static string Flow_DoPress(Int64 workID, string msg, bool isPressSubFlow)
        {
            GenerWorkFlow gwf = new GenerWorkFlow(workID);

            /* Find the staff to be run by the current */
            GenerWorkerLists wls = new GenerWorkerLists(workID, gwf.FK_Node);
            string toEmp = "", toEmpName = "";
            string mailTitle = " Reminders :" + gwf.Title + ",  Sender :" + WebUser.Name;
            // If you can not find the child thread transfer log and the parent process ID is not empty , In the parent process to find the recipient 
            if (wls.Count == 0 && gwf.FID != 0)
            {
                wls = new GenerWorkerLists(gwf.FID, gwf.FK_Node);
            }

            foreach (GenerWorkerList wl in wls)
            {
                if (wl.IsEnable == false)
                    continue;

                toEmp += wl.FK_Emp + ",";
                toEmpName += wl.FK_EmpText + ",";

                //  Message .
                BP.WF.Dev2Interface.Port_SendMsg(wl.FK_Emp, mailTitle, msg, null, BP.WF.SMSMsgType.Self, gwf.FK_Flow, gwf.FK_Node, gwf.WorkID, gwf.FID);

                wl.PressTimes = wl.PressTimes + 1;
                wl.Update();

                //wl.Update(GenerWorkerListAttr.PressTimes, wl.PressTimes + 1);
            }

            // Written to the log .
            WorkNode wn = new WorkNode(workID, gwf.FK_Node);
            wn.AddToTrack(ActionType.Press, toEmp, toEmpName, gwf.FK_Node, gwf.NodeName, msg);

            // If reminders subprocess .
            if (isPressSubFlow)
            {
                string subMsg = "";
                GenerWorkFlows gwfs = gwf.HisSubFlowGenerWorkFlows;
                foreach (GenerWorkFlow item in gwfs)
                {
                    subMsg += "@ The pair has started threads :" + item.Title + " The reminders , Messages are as follows :";
                    subMsg += Flow_DoPress(item.WorkID, msg, false);
                }
                return " System has the information to notify you :" + toEmpName + "" + subMsg;
            }
            else
            {
                return " System has the information to notify you :" + toEmpName;
            }
        }
        /// <summary>
        ///  Reset Process title 
        ///  You can call it at any location of the node , Generating new title .
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="workid"> The work ID</param>
        /// <returns> Whether set successfully </returns>
        public static bool Flow_ReSetFlowTitle(string flowNo, int nodeID, Int64 workid)
        {
            //  Converted into numbers .
            flowNo = TurnFlowMarkToFlowNo(flowNo);

            Node nd = new Node(nodeID);
            Work wk = nd.HisWork;
            wk.OID = workid;
            wk.RetrieveFromDBSources();
            Flow fl = nd.HisFlow;
            string title = BP.WF.WorkNode.GenerTitle(fl, wk);
            return Flow_SetFlowTitle(flowNo, workid, title);
        }
        /// <summary>
        ///  Setting Process title 
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="workid"> The work ID</param>
        /// <param name="title"> Title </param>
        /// <returns> Whether set successfully </returns>
        public static bool Flow_SetFlowTitle(string flowNo, Int64 workid, string title)
        {
            //  Converted into numbers .
            flowNo = TurnFlowMarkToFlowNo(flowNo);


            string dbstr = SystemConfig.AppCenterDBVarStr;
            Paras ps = new Paras();
            ps.SQL = "UPDATE WF_GenerWorkFlow SET Title=" + dbstr + "Title WHERE WorkID=" + dbstr + "WorkID";
            ps.Add(GenerWorkFlowAttr.Title, title);
            ps.Add(GenerWorkFlowAttr.WorkID, workid);
            DBAccess.RunSQL(ps);

            Flow fl = new Flow(flowNo);
            ps = new Paras();
            ps.SQL = "UPDATE " + fl.PTable + " SET Title=" + dbstr + "Title WHERE OID=" + dbstr + "WorkID";
            ps.Add(GenerWorkFlowAttr.Title, title);
            ps.Add(GenerWorkFlowAttr.WorkID, workid);
            int num = DBAccess.RunSQL(ps);

            if (fl.HisDataStoreModel == DataStoreModel.ByCCFlow)
            {
                ps = new Paras();
                ps.SQL = "UPDATE ND" + int.Parse(flowNo + "01") + " SET Title=" + dbstr + "Title WHERE OID=" + dbstr + "WorkID";
                ps.Add(GenerWorkFlowAttr.Title, title);
                ps.Add(GenerWorkFlowAttr.WorkID, workid);
                DBAccess.RunSQL(ps);
            }

            if (num == 0)
                return false;
            else
                return true;
        }

        /// <summary>
        ///  Scheduling Process 
        ///  Explanation :
        /// 1, Is usually caused by admin Scheduled for execution .
        /// 2, Under special circumstances , Needs to be run from one person to another specified node scheduling , The staff developed .
        /// </summary>
        /// <param name="workid"> The work ID</param>
        /// <param name="toNodeID"> Dispatched to the node </param>
        /// <param name="toEmper"> Dispatched to the staff </param>
        public static string Flow_Schedule(Int64 workid, int toNodeID, string toEmper)
        {
            string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;

            Node nd = new Node(toNodeID);
            Emp emp = new Emp(toEmper);

            //  Turn up GenerWorkFlow, And perform updates .
            GenerWorkFlow gwf = new GenerWorkFlow(workid);
            gwf.WFState = WFState.Runing;
            gwf.TaskSta = TaskSta.None;
            gwf.TodoEmps = toEmper;
            gwf.FK_Node = toNodeID;
            gwf.NodeName = nd.Name;
            //gwf.StarterName =emp.Name;
            gwf.Update();

            // Let it all setup is complete .
            Paras ps = new Paras();
            ps.SQL = "UPDATE WF_GenerWorkerList SET IsPass=1 WHERE WorkID=" + dbstr + "WorkID";
            ps.Add(GenerWorkFlowAttr.WorkID, workid);
            BP.DA.DBAccess.RunSQL(ps);

            //  Update process data .
            Flow fl = new Flow(gwf.FK_Flow);
            ps = new Paras();
            ps.SQL = "UPDATE " + fl.PTable + " SET FlowEnder=" + dbstr + "FlowEnder,FlowEndNode=" + dbstr + "FlowEndNode WHERE OID=" + dbstr + "OID";
            ps.Add(NDXRptBaseAttr.FlowEnder, toEmper);
            ps.Add(NDXRptBaseAttr.FlowEndNode, toNodeID);
            ps.Add(NDXRptBaseAttr.OID, workid);
            BP.DA.DBAccess.RunSQL(ps);

            //  Perform an update .
            GenerWorkerLists gwls = new GenerWorkerLists(workid);
            GenerWorkerList gwl = gwls[gwls.Count - 1] as GenerWorkerList; // Get the last one .
            gwl.RDT = DataType.CurrentDataTimess;
            gwl.WorkID = workid;
            gwl.FK_Node = toNodeID;
            gwl.FK_NodeText = nd.Name;
            gwl.FK_Emp = toEmper;
            gwl.FK_EmpText = emp.Name;
            gwl.IsPass = false;
            gwl.IsEnable = true;
            gwl.IsRead = false;
            gwl.WhoExeIt = nd.WhoExeIt;
            gwl.Sender = BP.Web.WebUser.No;
            gwl.HungUpTimes = 0;
            gwl.FID = gwf.FID;
            gwl.FK_Dept = emp.FK_Dept;

            if (gwl.Update() == 0)
                gwl.Insert();

            string sql = "SELECT COUNT(*) FROM WF_EmpWorks where WorkID=" + workid + " and fk_emp='" + toEmper + "'";
            int i = BP.DA.DBAccess.RunSQLReturnValInt(sql);
            if (i == 0)
                throw new Exception("@ Scheduling error ");

            return " The process (" + gwf.Title + "), Has been dispatched to the (" + nd.Name + "), Assigned to (" + emp.Name + ")";
        }
        /// <summary>
        ///  Set flow mode of operation 
        ///  If it is an automated process .  Person in charge :liuxianchen.
        ///  Local calls /WorkOpt/TransferCustom.aspx
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="workid"> The work ID</param>
        /// <param name="isAutoRun"> Whether to run automatically ?  If Autorun , The process is run according to the rules set .
        ///  Non-automatic operation , Operate in accordance with the order of user-defined calculations .</param>
        /// <param name="paras"> Manual operation in the format : @ Node ID1, Subprocess No, Recipient 1, Recipient n,@ Node ID2, Subprocess No, Recipient 1, Recipient n,</param>
        public static void Flow_SetFlowTransferCustom(string flowNo, Int64 workid, bool isAutoRun, string paras)
        {
            // Delete previously stored parameters .
            BP.DA.DBAccess.RunSQL("DELETE FROM WF_TransferCustom WHERE WorkID=" + workid);

            // Save parameters .
            //  Parameter format   @104,SubFlow002,zhangsan,lisi@103,,zhaoliu,wangqi
            string[] strs = paras.Split('@');
            int idx = 0;
            foreach (string str in strs)
            {
                if (string.IsNullOrEmpty(str))
                    continue;

                if (str.Contains(",") == false)
                    continue;

                //  Working with strings .
                string[] vals = str.Split(',');
                int nodeid = int.Parse(vals[0]);
                var subFlow = vals[1];

                TransferCustom tc = new TransferCustom();
                tc.Idx = idx;
                tc.FK_Node = nodeid;
                tc.WorkID = workid;
                tc.Worker = str.Replace(nodeid + "," + subFlow + ",", "");
                tc.SubFlowNo = subFlow;
                tc.MyPK = tc.FK_Node + "_" + tc.WorkID + "_" + idx;
                tc.Save();
                idx++;
            }

            //  Set the operation mode .
            GenerWorkFlow gwf = new GenerWorkFlow();
            gwf.WorkID = workid;
            if (gwf.RetrieveFromDBSources() == 0)
            {
                gwf.WFSta = WFSta.Runing;
                gwf.WFState = WFState.Blank;

                gwf.Starter = WebUser.No;
                gwf.StarterName = WebUser.Name;

                gwf.FK_Flow = flowNo;
                BP.WF.Flow fl = new Flow(flowNo);
                gwf.FK_FlowSort = fl.FK_FlowSort;
                gwf.FK_Dept = WebUser.FK_Dept;
                gwf.IsAutoRun = isAutoRun;
                gwf.Insert();
                return;
            }
            gwf.Update();
        }
        #endregion  And process-related interface 

        #region get  Properties section mouth 
        /// <summary>
        ///  Obtained during process execution parameters 
        /// </summary>
        /// <param name="nodeID"> Node ID</param>
        /// <param name="workid"> The work ID</param>
        /// <returns> If you do not return null, There will return @ Parameter name 0= Parameter values 0@ Parameter name 1= Parameter values 1</returns>
        public static string GetFlowParas(int nodeID, Int64 workid)
        {
            string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
            Paras ps = new Paras();
            ps.SQL = "SELECT Paras FROM WF_GenerWorkerlist WHERE FK_Node=" + dbstr + "FK_Node AND WorkID=" + dbstr + "WorkID";
            ps.Add(GenerWorkerListAttr.FK_Node, nodeID);
            ps.Add(GenerWorkerListAttr.WorkID, workid);
            return DBAccess.RunSQLReturnStringIsNull(ps, null);
        }
        #endregion get  Properties section mouth 

        #region  Work on the interface 
        /// <summary>
        ///  Initiate the process 
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="ht"> Node Form : Master table data Key Value  Passed by ( Can be empty )</param>
        /// <param name="workDtls"> Node Form : Data from the table , From the table name and number from the table from a form should correspond ( Can be empty )</param>
        /// <param name="nextNodeID"> After the launch to jump to the node ( Can be empty )</param>
        /// <param name="nextWorker"> Staff after launch to jump to a specified node and ( Can be empty )</param>
        /// <returns> Execution information transmitted to the second node </returns>
        public static SendReturnObjs Node_StartWork(string flowNo, Hashtable ht, DataSet workDtls,
           int nextNodeID, string nextWorker)
        {
            return Node_StartWork(flowNo, ht, workDtls, nextNodeID, nextWorker, 0, null);
        }
        /// <summary>
        ///  Initiate the process 
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="htWork"> Node Form : Master table data Key Value  Passed by ( Can be empty )</param>
        /// <param name="workDtls"> Node Form : Data from the table , From the table name and number from the table from a form should correspond ( Can be empty )</param>
        /// <param name="nextNodeID"> After the launch to jump to the node ( Can be empty )</param>
        /// <param name="nextWorker"> Staff after launch to jump to a specified node and ( Can be empty )</param>
        /// <param name="parentWorkID"> Parent process workid, If not for 0</param>
        /// <param name="parentFlowNo"> Parent process ID , If you do not be empty </param>
        /// <returns> Execution information transmitted to the second node </returns>
        public static SendReturnObjs Node_StartWork(string flowNo, Hashtable htWork, DataSet workDtls,
            int nextNodeID, string nextWorker, Int64 parentWorkID, string parentFlowNo)
        {
            //  To the global variable assignment .
            BP.WF.Glo.SendHTOfTemp = htWork;

            // To the global variable assignment .
            BP.WF.Glo.SendHTOfTemp = htWork;

            //  Converted into numbers .
            flowNo = TurnFlowMarkToFlowNo(flowNo);

            //
            parentFlowNo = TurnFlowMarkToFlowNo(parentFlowNo);
            Flow fl = new Flow(flowNo);
            Work wk = fl.NewWork();
            Int64 workID = wk.OID;
            if (htWork != null)
            {
                foreach (string str in htWork.Keys)
                {
                    switch (str)
                    {
                        case StartWorkAttr.OID:
                        case StartWorkAttr.CDT:
                        case StartWorkAttr.MD5:
                        case StartWorkAttr.Emps:
                        case StartWorkAttr.FID:
                        case StartWorkAttr.FK_Dept:
                        case StartWorkAttr.PRI:
                        case StartWorkAttr.Rec:
                        case StartWorkAttr.Title:
                            continue;
                        default:
                            break;
                    }
                    wk.SetValByKey(str, htWork[str]);
                }
            }

            wk.OID = workID;
            if (workDtls != null)
            {
                // Save from the table 
                foreach (DataTable dt in workDtls.Tables)
                {
                    foreach (MapDtl dtl in wk.HisMapDtls)
                    {
                        if (dt.TableName != dtl.No)
                            continue;
                        // Get dtls
                        GEDtls daDtls = new GEDtls(dtl.No);
                        daDtls.Delete(GEDtlAttr.RefPK, wk.OID); //  Clear existing data .

                        GEDtl daDtl = daDtls.GetNewEntity as GEDtl;
                        daDtl.RefPK = wk.OID.ToString();

                        //  To copy the data from the table .
                        foreach (DataRow dr in dt.Rows)
                        {
                            daDtl.ResetDefaultVal();
                            daDtl.RefPK = wk.OID.ToString();

                            // Details column .
                            foreach (DataColumn dc in dt.Columns)
                            {
                                // Setting properties .
                                daDtl.SetValByKey(dc.ColumnName, dr[dc.ColumnName]);
                            }
                            daDtl.InsertAsOID(DBAccess.GenerOID("Dtl")); // Insert data .
                        }
                    }
                }
            }

            WorkNode wn = new WorkNode(wk, fl.HisStartNode);

            Node nextNoode = null;
            if (nextNodeID != 0)
                nextNoode = new Node(nextNodeID);

            SendReturnObjs objs = wn.NodeSend(nextNoode, nextWorker);
            if (parentWorkID != 0)
                DBAccess.RunSQL("UPDATE WF_GenerWorkFlow SET PWorkID=" + parentWorkID + ",PFlowNo='" + parentFlowNo + "' WHERE WorkID=" + objs.VarWorkID);

            #region  Send parameter update .
            if (htWork != null)
            {
                string paras = "";
                foreach (string key in htWork.Keys)
                    paras += "@" + key + "=" + htWork[key].ToString();

                if (string.IsNullOrEmpty(paras) == false)
                {
                    string dbstr = SystemConfig.AppCenterDBVarStr;
                    Paras ps = new Paras();
                    ps.SQL = "UPDATE WF_GenerWorkerlist set AtPara=" + dbstr + "Paras WHERE WorkID=" + dbstr + "WorkID AND FK_Node=" + dbstr + "FK_Node";
                    ps.Add(GenerWorkerListAttr.Paras, paras);
                    ps.Add(GenerWorkerListAttr.WorkID, workID);
                    ps.Add(GenerWorkerListAttr.FK_Node, int.Parse(flowNo + "01"));
                    try
                    {
                        DBAccess.RunSQL(ps);
                    }
                    catch
                    {
                        GenerWorkerList gwl = new GenerWorkerList();
                        gwl.CheckPhysicsTable();
                        DBAccess.RunSQL(ps);
                    }
                }
            }
            #endregion  Send parameter update .

            return objs;
        }
        /// <summary>
        ///  Create WorkID
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="ht"> Form parameters , For null.</param>
        /// <param name="workDtls"> Parameter list , For null.</param>
        /// <param name="nextWorker"> The operator , If it is null Is the current staff .</param>
        /// <param name="title"> When the title to create work , If it is null, It generated according to the rules set .</param>
        /// <returns> To create the work after the start node generated WorkID.</returns>
        public static Int64 Node_CreateBlankWork(string flowNo, Hashtable ht, DataSet workDtls,
            string starter, string title)
        {
            return Node_CreateBlankWork(flowNo, ht, workDtls, starter, title, 0, null, 0, null);
        }
        /// <summary>
        ///  Create WorkID
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="ht"> Form parameters , For null.</param>
        /// <param name="workDtls"> Parameter list , For null.</param>
        /// <param name="starter"> Process sponsor </param>
        /// <param name="title"> When the title to create work , If it is null, It generated according to the rules set .</param>
        /// <param name="parentWorkID"> Parent process WorkID, If not passed on to the parent process 0.</param>
        /// <param name="parentFlowNo"> Parent process ID of the process , If not passed on to the parent process null.</param>
        /// <returns> To create the work after the start node generated WorkID.</returns>
        public static Int64 Node_CreateBlankWork(string flowNo, Hashtable ht, DataSet workDtls,
            string starter, string title, Int64 parentWorkID, string parentFlowNo, int parentNodeID, string parentEmp)
        {
            //  To the global variable assignment .
            BP.WF.Glo.SendHTOfTemp = ht;

            //  Converted into numbers .
            flowNo = TurnFlowMarkToFlowNo(flowNo);

            // Converted into numbers 
            parentFlowNo = TurnFlowMarkToFlowNo(parentFlowNo);

            if (parentFlowNo == null)
                parentFlowNo = "";

            string dbstr = SystemConfig.AppCenterDBVarStr;

            if (string.IsNullOrEmpty(starter))
                starter = WebUser.No;

            Flow fl = new Flow(flowNo);
            Node nd = new Node(fl.StartNodeID);

            //  Next staff .
            Emp empStarter = new Emp(starter);
            Work wk = fl.NewWork(starter);
            Int64 workID = wk.OID;

            #region  To each property - Assignment 
            if (ht != null)
            {
                foreach (string str in ht.Keys)
                {
                    switch (str)
                    {
                        case StartWorkAttr.OID:
                        case StartWorkAttr.CDT:
                        case StartWorkAttr.MD5:
                        case StartWorkAttr.Emps:
                        case StartWorkAttr.FID:
                        case StartWorkAttr.FK_Dept:
                        case StartWorkAttr.PRI:
                        case StartWorkAttr.Rec:
                        case StartWorkAttr.Title:
                            continue;
                        default:
                            break;
                    }
                    wk.SetValByKey(str, ht[str]);
                }
            }
            wk.OID = workID;
            if (workDtls != null)
            {
                // Save from the table 
                foreach (DataTable dt in workDtls.Tables)
                {
                    foreach (MapDtl dtl in wk.HisMapDtls)
                    {
                        if (dt.TableName != dtl.No)
                            continue;
                        // Get dtls
                        GEDtls daDtls = new GEDtls(dtl.No);
                        daDtls.Delete(GEDtlAttr.RefPK, wk.OID); //  Clear existing data .

                        GEDtl daDtl = daDtls.GetNewEntity as GEDtl;
                        daDtl.RefPK = wk.OID.ToString();

                        //  To copy the data from the table .
                        foreach (DataRow dr in dt.Rows)
                        {
                            daDtl.ResetDefaultVal();
                            daDtl.RefPK = wk.OID.ToString();

                            // Details column .
                            foreach (DataColumn dc in dt.Columns)
                            {
                                // Setting properties .
                                daDtl.SetValByKey(dc.ColumnName, dr[dc.ColumnName]);
                            }
                            daDtl.InsertAsOID(DBAccess.GenerOID("Dtl")); // Insert data .
                        }
                    }
                }
            }
            #endregion  Assignment 

            Paras ps = new Paras();
            //  Implementation of the report data sheet WFState Update status , It is runing Status .
            if (string.IsNullOrEmpty(title) == false)
            {
                if (fl.TitleRole != "@OutPara")
                {
                    fl.TitleRole = "@OutPara";
                    fl.Update();
                }

                ps = new Paras();
                ps.SQL = "UPDATE " + fl.PTable + " SET PFlowNo=" + dbstr + "PFlowNo,PWorkID=" + dbstr + "PWorkID,WFState=" + dbstr + "WFState,Title=" + dbstr + "Title WHERE OID=" + dbstr + "OID";
                ps.Add(GERptAttr.PFlowNo, parentFlowNo);
                ps.Add(GERptAttr.PWorkID, parentWorkID);

                ps.Add(GERptAttr.WFState, (int)WFState.Blank);
                ps.Add(GERptAttr.Title, title);
                ps.Add(GERptAttr.OID, wk.OID);
                DBAccess.RunSQL(ps);
            }
            else
            {
                ps = new Paras();
                ps.SQL = "UPDATE " + fl.PTable + " SET PFlowNo=" + dbstr + "PFlowNo,PWorkID=" + dbstr + "PWorkID,WFState=" + dbstr + "WFState,FK_Dept=" + dbstr + "FK_Dept,Title=" + dbstr + "Title WHERE OID=" + dbstr + "OID";
                ps.Add(GERptAttr.PFlowNo, parentFlowNo);
                ps.Add(GERptAttr.PWorkID, parentWorkID);

                ps.Add(GERptAttr.WFState, (int)WFState.Blank);
                ps.Add(GERptAttr.FK_Dept, empStarter.FK_Dept);
                ps.Add(GERptAttr.Title, WorkNode.GenerTitle(fl, wk));
                ps.Add(GERptAttr.OID, wk.OID);
                DBAccess.RunSQL(ps);
            }

            //  Set the parent process information .
            if (parentWorkID != 0)
            {
                   //  Delete junk data may produce , For example, the last time was not sent successfully , Cause data not cleared .
                ps = new Paras();
                ps.SQL = "DELETE FROM WF_GenerWorkFlow  WHERE WorkID=" + dbstr + "WorkID1 OR FID=" + dbstr + "WorkID2";
                ps.Add("WorkID1", wk.OID);
                ps.Add("WorkID2", wk.OID);
                DBAccess.RunSQL(ps);


                GenerWorkFlow gwf = new GenerWorkFlow();
                gwf.WorkID = wk.OID;
                int i = gwf.RetrieveFromDBSources();
                if (i == 0)
                {
                    // The flow of information is written in advance wf_GenerWorkFlow, Avoid queries less 
                    gwf.FlowName = fl.Name;
                    gwf.FK_Flow = flowNo;
                    gwf.FK_FlowSort = fl.FK_FlowSort;
                    gwf.FK_Dept = WebUser.FK_Dept;
                    gwf.DeptName = WebUser.FK_DeptName;
                    gwf.FK_Node = fl.StartNodeID;
                    gwf.NodeName = nd.Name;
                    gwf.WFState = WFState.Runing;
                    if (string.IsNullOrEmpty(title))
                        gwf.Title = BP.WF.WorkNode.GenerTitle(fl, wk);
                    else
                        gwf.Title = title;
                    gwf.Starter = WebUser.No;
                    gwf.StarterName = WebUser.Name;
                    gwf.RDT = DataType.CurrentDataTime;
                    gwf.PWorkID = parentWorkID;
                    gwf.PFlowNo = parentFlowNo;
                    gwf.PNodeID = parentNodeID;
                    gwf.Insert();

                    GenerWorkerList gwl = new GenerWorkerList();
                    gwl.WorkID = wk.OID;
                    gwl.FK_Emp = WebUser.No;
                    gwl.FK_EmpText = WebUser.Name;
                    gwl.FK_Node = nd.NodeID;
                    gwl.FK_NodeText = nd.Name;
                    gwl.FID = 0;
                    gwl.FK_Flow = fl.No;
                    gwl.FK_Dept = WebUser.FK_Dept;
                    gwl.SDT = DataType.CurrentDataTime;
                    gwl.DTOfWarning = DataType.CurrentDataTime;
                    gwl.RDT = DataType.CurrentDataTime;
                    gwl.IsEnable = true;
                    gwl.IsPass = false;
                    gwl.Sender = WebUser.No;
                    gwl.PRI = gwf.PRI;
                    gwl.Insert();
                }
                // Set the parent process information 
                BP.WF.Dev2Interface.SetParentInfo(flowNo, wk.OID, parentFlowNo, parentWorkID, parentNodeID, parentEmp);
            }
            else
            {
                //  Delete junk data may produce , For example, the last time was not sent successfully , Cause data not cleared .
                ps = new Paras();
                ps.SQL = "DELETE FROM WF_GenerWorkFlow  WHERE WorkID=" + dbstr + "WorkID1 OR FID=" + dbstr + "WorkID2";
                ps.Add("WorkID1", wk.OID);
                ps.Add("WorkID2", wk.OID);
                DBAccess.RunSQL(ps);
            }
            ps = new Paras();
            ps.SQL = "DELETE FROM WF_GenerWorkerList  WHERE WorkID=" + dbstr + "WorkID1 OR FID=" + dbstr + "WorkID2";
            ps.Add("WorkID1", wk.OID);
            ps.Add("WorkID2", wk.OID);
            DBAccess.RunSQL(ps);

            return wk.OID;
        }
        /// <summary>
        ///  Initiate work 
        ///  After you create can be the founder of the formation of a to-do .
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="ht"> Form parameters , For null.</param>
        /// <param name="workDtls"> Form parameter list , For null.</param>
        /// <param name="flowStarter"> Process sponsor , If it is null Is the current staff .</param>
        /// <param name="title"> When the title to create work , If it is null, It generated according to the rules set .</param>
        /// <returns> To create the work after the start node generated WorkID.</returns>
        public static Int64 Node_CreateStartNodeWork(string flowNo, Hashtable ht, DataSet workDtls,
            string flowStarter, string title)
        {
            return Node_CreateStartNodeWork(flowNo, ht, workDtls, flowStarter, title, 0, null, 0);
        }
        /// <summary>
        ///  Create a start node work 
        ///  After you create can be the founder of the formation of a to-do .
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="htWork"> Form parameters , For null.</param>
        /// <param name="workDtls"> Parameter list , For null.</param>
        /// <param name="flowStarter"> Process sponsor , If it is null Is the current staff .</param>
        /// <param name="title"> When the title to create work , If it is null, It generated according to the rules set .</param>
        /// <param name="parentWorkID"> Parent process WorkID, If not passed on to the parent process 0.</param>
        /// <param name="parentFlowNo"> Parent process ID of the process , If not passed on to the parent process null.</param>
        /// <returns> To create the work after the start node generated WorkID.</returns>
        public static Int64 Node_CreateStartNodeWork(string flowNo, Hashtable htWork, DataSet workDtls,
            string flowStarter, string title, Int64 parentWorkID, string parentFlowNo, int parentNDFrom)
        {

            //  To the global variable assignment .
            BP.WF.Glo.SendHTOfTemp = htWork;

            //  Converted into numbers .
            flowNo = TurnFlowMarkToFlowNo(flowNo);

            // Converted into numbers 
            parentFlowNo = TurnFlowMarkToFlowNo(parentFlowNo);

            if (string.IsNullOrEmpty(flowStarter))
                flowStarter = WebUser.No;

            Flow fl = new Flow(flowNo);

            #region  Processing title .
            if (string.IsNullOrEmpty(title) == false && fl.TitleRole != "@OutPara")
            {
                /* If the title is not empty */
                fl.TitleRole = "@OutPara"; // Special marking is not empty .
                fl.Update();
            }
            if (string.IsNullOrEmpty(title) == true && fl.TitleRole == "@OutPara")
            {
                /* If the title is empty  */
                fl.TitleRole = ""; // Special marking is not empty .
                fl.Update();
            }
            #endregion  Processing title .

            Node nd = new Node(fl.StartNodeID);

            //  Next staff .
            Emp emp = new Emp(flowStarter);
            Work wk = fl.NewWork(flowStarter);
            Int64 workID = wk.OID;


            #region  Create a to-do to start work 
            GenerWorkFlow gwf = new GenerWorkFlow();
            gwf.WorkID = workID;
            int i = gwf.RetrieveFromDBSources();
            if (i == 0)
            {
                gwf.FlowName = fl.Name;
                gwf.FK_Flow = flowNo;
                gwf.FK_FlowSort = fl.FK_FlowSort;

                gwf.FK_Dept = emp.FK_Dept;
                gwf.DeptName = emp.FK_DeptText;
                gwf.FK_Node = fl.StartNodeID;

                gwf.NodeName = nd.Name;
                gwf.WFState = WFState.Runing;

                if (string.IsNullOrEmpty(title))
                    gwf.Title = BP.WF.WorkNode.GenerTitle(fl, wk);
                else
                    gwf.Title = title;

                gwf.Starter = emp.No;
                gwf.StarterName = emp.Name;
                gwf.RDT = DataType.CurrentDataTime;

                if (htWork != null && htWork.ContainsKey("PRI") == true)
                    gwf.PRI = int.Parse(htWork["PRI"].ToString());

                if (htWork != null && htWork.ContainsKey("SDTOfNode") == true)
                    /* Node should finish time */
                    gwf.SDTOfNode = htWork["SDTOfNode"].ToString();

                if (htWork != null && htWork.ContainsKey("SDTOfFlow") == true)
                    /* The process should be completed by the time */
                    gwf.SDTOfNode = htWork["SDTOfFlow"].ToString();

                gwf.PWorkID = parentWorkID;
                gwf.PFlowNo = parentFlowNo;
                gwf.PNodeID = parentNDFrom;
                gwf.Insert();

                //  Generate job listings .
                GenerWorkerList gwl = new GenerWorkerList();
                gwl.WorkID = wk.OID;
                gwl.FK_Emp = emp.No;
                gwl.FK_EmpText = emp.Name;

                gwl.FK_Node = nd.NodeID;
                gwl.FK_NodeText = nd.Name;
                gwl.FID = 0;

                gwl.FK_Flow = fl.No;
                gwl.FK_Dept = emp.FK_Dept;

                gwl.SDT = DataType.CurrentDataTime;
                gwl.DTOfWarning = DataType.CurrentDataTime;
                gwl.RDT = DataType.CurrentDataTime;
                gwl.IsEnable = true;

                gwl.IsPass = false;
                gwl.Sender = WebUser.No;
                gwl.PRI = gwf.PRI;
                gwl.Insert();
            }
            #endregion  Create a to-do to start work 

            #region  To each property - Assignment 
            if (htWork != null)
            {
                foreach (string str in htWork.Keys)
                {
                    switch (str)
                    {
                        case StartWorkAttr.OID:
                        case StartWorkAttr.CDT:
                        case StartWorkAttr.MD5:
                        case StartWorkAttr.Emps:
                        case StartWorkAttr.FID:
                        case StartWorkAttr.FK_Dept:
                        case StartWorkAttr.PRI:
                        case StartWorkAttr.Rec:
                        case StartWorkAttr.Title:
                            continue;
                        default:
                            break;
                    }

                    wk.SetValByKey(str, htWork[str]);
                }
            }
            wk.OID = workID;
            if (workDtls != null)
            {
                // Save from the table 
                foreach (DataTable dt in workDtls.Tables)
                {
                    foreach (MapDtl dtl in wk.HisMapDtls)
                    {
                        if (dt.TableName != dtl.No)
                            continue;
                        // Get dtls
                        GEDtls daDtls = new GEDtls(dtl.No);
                        daDtls.Delete(GEDtlAttr.RefPK, wk.OID); //  Clear existing data .

                        GEDtl daDtl = daDtls.GetNewEntity as GEDtl;
                        daDtl.RefPK = wk.OID.ToString();

                        //  To copy the data from the table .
                        foreach (DataRow dr in dt.Rows)
                        {
                            daDtl.ResetDefaultVal();
                            daDtl.RefPK = wk.OID.ToString();

                            // Details column .
                            foreach (DataColumn dc in dt.Columns)
                            {
                                // Setting properties .
                                daDtl.SetValByKey(dc.ColumnName, dr[dc.ColumnName]);
                            }
                            daDtl.InsertAsOID(DBAccess.GenerOID("Dtl")); // Insert data .
                        }
                    }
                }
            }
            #endregion  Assignment 

            //  Implementation of the report data sheet WFState Update status , It is runing Status . 
            string dbstr = SystemConfig.AppCenterDBVarStr;
            Paras ps = new Paras();
            ps.SQL = "UPDATE " + fl.PTable + " SET WFState=" + dbstr + "WFState,Title=" + dbstr + "Title,FK_Dept=" + dbstr + "FK_Dept,PFlowNo=" + dbstr + "PFlowNo,PWorkID=" + dbstr + "PWorkID WHERE OID=" + dbstr + "OID";
            ps.Add("WFState", (int)WFState.Runing);
            ps.Add("Title", gwf.Title);
            ps.Add("FK_Dept", gwf.FK_Dept);

            ps.Add("PFlowNo", gwf.PFlowNo);
            ps.Add("PWorkID", gwf.PWorkID);

            ps.Add("OID", wk.OID);
            DBAccess.RunSQL(ps);

            //// Written to the log .
            //WorkNode wn = new WorkNode(wk, nd);
            //wn.AddToTrack(ActionType.CallSubFlow, flowStarter, emp.Name, nd.NodeID, nd.Name, " Come from " + WebUser.No + "," + WebUser.Name
            //    + " Work initiated .");

            #region  Send parameter update .
            if (htWork != null)
            {
                string paras = "";
                foreach (string key in htWork.Keys)
                    paras += "@" + key + "=" + htWork[key].ToString();

                if (string.IsNullOrEmpty(paras) == false)
                {
                    ps = new Paras();
                    ps.SQL = "UPDATE WF_GenerWorkerlist SET AtPara=" + dbstr + "Paras WHERE WorkID=" + dbstr + "WorkID AND FK_Node=" + dbstr + "FK_Node";
                    ps.Add(GenerWorkerListAttr.Paras, paras);
                    ps.Add(GenerWorkerListAttr.WorkID, workID);
                    ps.Add(GenerWorkerListAttr.FK_Node, nd.NodeID);
                    DBAccess.RunSQL(ps);
                }
            }
            #endregion  Send parameter update .

            return wk.OID;
        }
        /// <summary>
        ///  Send implementation 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="workID"> The work ID</param>
        /// <returns> Back Send results </returns>
        public static SendReturnObjs Node_SendWork(string fk_flow, Int64 workID)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            return Node_SendWork(fk_flow, workID, null, null, 0, null);
        }
        /// <summary>
        ///  Send implementation 
        /// </summary>
        /// <param name="fk_flow"> Job No. </param>
        /// <param name="workID"> The work ID</param>
        /// <param name="ht"> Nodes form data </param>
        /// <returns> Back Send results </returns>
        public static SendReturnObjs Node_SendWork(string fk_flow, Int64 workID, Hashtable ht)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);
            return Node_SendWork(fk_flow, workID, ht, null, 0, null);
        }
        /// <summary>
        ///  Send implementation 
        /// </summary>
        /// <param name="fk_flow"> Job No. </param>
        /// <param name="workID"> The work ID</param>
        /// <param name="ht"> Nodes form data </param>
        /// <param name="dsDtl"> Nodes form from table data </param>
        /// <returns> Back Send results </returns>
        public static SendReturnObjs Node_SendWork(string fk_flow, Int64 workID, Hashtable ht, DataSet dsDtl)
        {

            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            return Node_SendWork(fk_flow, workID, ht, dsDtl, 0, null);
        }
        /// <summary>
        ///  Send Work 
        /// </summary>
        /// <param name="nodeID"> Node number </param>
        /// <param name="workID"> The work ID</param>
        /// <param name="toNodeID"> Sent to the node number , In the case of 0 Let ccflow Automatic calculation .</param>
        /// <param name="toEmps"> Sent to the personnel , For example, several people separated by commas :zhangsan,lisi.  In the case of null Said let ccflow Automatic calculation .</param>
        /// <returns> Returns execution information </returns>
        public static SendReturnObjs Node_SendWork(string fk_flow, Int64 workID, int toNodeID, string toEmps)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);
            return Node_SendWork(fk_flow, workID, null, null, toNodeID, toEmps);
        }
        /// <summary>
        ///  Send Work 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="workID"> The work ID</param>
        /// <param name="htWork"> Nodes form data (Hashtable The key Form field names with the same node ,value  Is the field value )</param>
        /// <returns> Execution information </returns>
        public static SendReturnObjs Node_SendWork(string fk_flow, Int64 workID,
            Hashtable htWork, int toNodeID, string nextWorkers)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            return Node_SendWork(fk_flow, workID, htWork, null, toNodeID, nextWorkers, WebUser.No, WebUser.Name, WebUser.FK_Dept, WebUser.FK_DeptName, null);
        }
        /// <summary>
        ///  Send Work 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="workID"> The work ID</param>
        /// <param name="htWork"> Nodes form data (Hashtable The key Form field names with the same node ,value  Is the field value )</param>
        /// <param name="workDtls"> Form data from a table next node (dataset May comprise a plurality of table, Each table The name of the same name from the table , Column names from the fields of the table with the same , OID,RefPK Column is empty or needs null )</param>
        /// <param name="toNodeID"> Arrival node , In the case of 0 Let denote ccflow Automatically find , Otherwise in accordance with the parameters sent .</param>
        /// <param name="nextWorkers"> Next to the recipient , If more than one person separated by commas , Such as :zhangsan,lisi,
        ///  If it is empty , Let the logo ccflow According to the node access rules automatically find .</param>
        /// <returns> Execution information </returns>
        public static SendReturnObjs Node_SendWork(string fk_flow, Int64 workID, Hashtable htWork, DataSet workDtls, int toNodeID, string nextWorkers)
        {
            return Node_SendWork(fk_flow, workID, htWork, workDtls, toNodeID, nextWorkers, WebUser.No, WebUser.Name, WebUser.FK_Dept, WebUser.FK_DeptName, null);
        }
        /// <summary>
        ///  Send Work 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="workID"> The work ID</param>
        /// <param name="htWork"> Nodes form data (Hashtable The key Form field names with the same node ,value  Is the field value )</param>
        /// <param name="workDtls"> Form data from a table next node (dataset May comprise a plurality of table, Each table The name of the same name from the table , Column names from the fields of the table with the same , OID,RefPK Column is empty or needs null )</param>
        /// <param name="toNodeID"> Arrival node , In the case of 0 Let denote ccflow Automatically find , Otherwise in accordance with the parameters sent .</param>
        /// <param name="nextWorkers"> Next to the recipient , If more than one person separated by commas , Such as :zhangsan,lisi,
        ///  If it is empty , Let the logo ccflow According to the node access rules automatically find .</param>
        /// <param name="execUserNo"> Number executor </param>
        /// <param name="execUserName"> Executor name </param>
        /// <param name="execUserDeptNo"> Executor department name </param>
        /// <param name="execUserDeptName"> Executor department number </param>
        /// <returns> Send the result object </returns>
        public static SendReturnObjs Node_SendWork(string fk_flow, Int64 workID, Hashtable htWork, DataSet workDtls, int toNodeID,
            string nextWorkers, string execUserNo, string execUserName, string execUserDeptNo, string execUserDeptName, string title)
        {
            // Send to a temporary variable assignment , Solve steering with parameters .
            Glo.SendHTOfTemp = htWork;

            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            int currNodeId = Dev2Interface.Node_GetCurrentNodeID(fk_flow, workID);

            BP.WF.Dev2Interface.Node_SaveWork(fk_flow, currNodeId, workID, htWork, workDtls);

            //  Variable 》
            Node nd = new Node(currNodeId);
            Work sw = nd.HisWork;
            sw.OID = workID;
            sw.RetrieveFromDBSources();

            SendReturnObjs objs;
            // Send implementation process .
            WorkNode wn = new WorkNode(sw, nd);
            wn.Execer = execUserNo;
            wn.ExecerName = execUserName;
            wn.Execer = execUserNo;
            wn.title = title; //  Set Title , There may be passed from the outside over the title .
            wn.SendHTOfTemp = htWork;

            if (toNodeID == 0 || toNodeID == null)
                objs = wn.NodeSend(null, nextWorkers);
            else
                objs = wn.NodeSend(new Node(toNodeID), nextWorkers);



            #region  Send parameter update .
            if (htWork != null)
            {
                string dbstr = SystemConfig.AppCenterDBVarStr;
                Paras ps = new Paras();

                string paras = "";
                foreach (string key in htWork.Keys)
                {
                    paras += "@" + key + "=" + htWork[key].ToString();
                    switch (key)
                    {
                        case WorkSysFieldAttr.SysSDTOfFlow:
                            ps = new Paras();
                            ps.SQL = "UPDATE WF_GenerWorkFlow SET SDTOfFlow=" + dbstr + "SDTOfFlow WHERE WorkID=" + dbstr + "WorkID";
                            ps.Add(GenerWorkFlowAttr.SDTOfFlow, htWork[key].ToString());
                            ps.Add(GenerWorkerListAttr.WorkID, workID);
                            DBAccess.RunSQL(ps);

                            break;
                        case WorkSysFieldAttr.SysSDTOfNode:
                            ps = new Paras();
                            ps.SQL = "UPDATE WF_GenerWorkFlow SET SDTOfNode=" + dbstr + "SDTOfNode WHERE WorkID=" + dbstr + "WorkID";
                            ps.Add(GenerWorkFlowAttr.SDTOfNode, htWork[key].ToString());
                            ps.Add(GenerWorkerListAttr.WorkID, workID);
                            DBAccess.RunSQL(ps);

                            ps = new Paras();
                            ps.SQL = "UPDATE WF_GenerWorkerlist SET SDT=" + dbstr + "SDT WHERE WorkID=" + dbstr + "WorkID AND FK_Node=" + dbstr + "FK_Node";
                            ps.Add(GenerWorkerListAttr.SDT, htWork[key].ToString());
                            ps.Add(GenerWorkerListAttr.WorkID, workID);
                            ps.Add(GenerWorkerListAttr.FK_Node, objs.VarToNodeID);
                            DBAccess.RunSQL(ps);
                            break;
                        default:
                            break;
                    }
                }

                if (string.IsNullOrEmpty(paras) == false)
                {
                    ps = new Paras();
                    ps.SQL = "UPDATE WF_GenerWorkerlist SET AtPara=" + dbstr + "Paras WHERE WorkID=" + dbstr + "WorkID AND FK_Node=" + dbstr + "FK_Node";
                    ps.Add(GenerWorkerListAttr.Paras, paras);
                    ps.Add(GenerWorkerListAttr.WorkID, workID);
                    ps.Add(GenerWorkerListAttr.FK_Node, nd.NodeID);
                    DBAccess.RunSQL(ps);
                }
            }
            #endregion  Send parameter update .

            return objs;
        }
        /// <summary>
        ///  Increase an additional treatment in the work queue .
        ///  This processing system has automatically processed order .
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="nodeID"> The work ID</param>
        /// <param name="workid">workid</param>
        /// <param name="fid">fid</param>
        /// <param name="empNo"> To deal with the increased number of people </param>
        /// <param name="empName"> To increase the treatment of human names </param>
        public static void Node_InsertOrderEmp(string flowNo, int nodeID, Int64 workid, Int64 fid, string empNo, string empName)
        {
            GenerWorkerList gwl = new GenerWorkerList();
            int i = gwl.Retrieve(GenerWorkerListAttr.WorkID, workid, GenerWorkerListAttr.FK_Node, nodeID);
            if (i == 0)
                throw new Exception("@ Did not find the current to-do staff , Please check whether the process has been running to the node will come up .");

            gwl.IsPassInt = 100;
            gwl.IsEnable = true;
            gwl.FK_Emp = empNo;
            gwl.FK_EmpText = empName;

            try
            {
                gwl.Insert();
            }
            catch
            {
                throw new Exception("@ The staff processing queue already exists , You can not increase .");
            }

            // Start updating their order ,  First, get their order from the database .     lxl Position steeled 
            string sql = "SELECT No,Name FROM Port_Emp WHERE No IN (SELECT FK_Emp FROM WF_GenerWorkerList WHERE WorkID=" + workid + " AND FK_Node=" + nodeID + " AND IsPass >=100 ) ORDER BY IDX desc";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            int idx = 100;
            foreach (DataRow dr in dt.Rows)
            {
                idx++;
                string myEmpNo = dr[0].ToString();
                sql = "UPDATE WF_GenerWorkerList SET IsPass=" + idx + " WHERE FK_Emp='" + myEmpNo + "' AND WorkID=" + workid + " AND FK_Node=" + nodeID;
                BP.DA.DBAccess.RunSQL(sql);
            }
        }
        /// <summary>
        ///  The CC is written to-do list 
        /// </summary>
        /// <param name="nodeID"> Node ID</param>
        /// <param name="workID"> The work ID</param>
        /// <param name="ccToEmpNo"> Copied to </param>
        /// <param name="ccToEmpName"> Copied to name </param>
        /// <returns></returns>
        public static string Node_CC_WriteTo_Todolist(int ndFrom, int ndTo, Int64 workID, string ccToEmpNo, string ccToEmpName)
        {
            return Node_CC_WriteTo_CClist(ndFrom, ndTo, workID, ccToEmpNo, ccToEmpName, "", "");

            ///* If you want to write to-do */
            //GenerWorkerList gwl =new GenerWorkerList();
            //int i=gwl.Retrieve(GenerWorkerListAttr.WorkID, workID, GenerWorkerListAttr.FK_Node, nodeID);
            //if (i == 0)
            //    throw new Exception(" Error , Do not find it to be .");

            //gwl.FK_Emp = ccToEmpNo;
            //gwl.FK_EmpText = ccToEmpName;
            //gwl.IsCC = true;
            //try
            //{
            //    gwl.Insert();
            //}
            //catch
            //{
            //    /* There may be */
            //}
            //return " Successful implementation ";
        }
        /// <summary>
        ///  Execution Cc 
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="workID"> The work ID</param>
        /// <param name="toEmpNo"> CC staff numbers </param>
        /// <param name="toEmpName"> CC staff person name </param>
        /// <param name="msgTitle"> Title </param>
        /// <param name="msgDoc"> Content </param>
        /// <returns> Execution information </returns>
        public static string Node_CC_WriteTo_CClist(int ndFrom, int ndTo, Int64 workID, string toEmpNo, string toEmpName,
            string msgTitle, string msgDoc)
        {
            GenerWorkFlow gwf = new GenerWorkFlow();
            gwf.WorkID = workID;
            if (gwf.RetrieveFromDBSources() == 0)
            {
                Node nd = new Node(ndTo);
                gwf.FK_Node = ndTo;
                gwf.FK_Flow = nd.FK_Flow;
                gwf.FlowName = nd.FlowName;
                gwf.NodeName = nd.Name;
            }

            CCList list = new CCList();
            list.MyPK = DBAccess.GenerOIDByGUID().ToString(); // workID + "_" + fk_node + "_" + empNo;
            list.FK_Flow = gwf.FK_Flow;
            list.FlowName = gwf.FlowName;
            list.FK_Node = ndTo;
            list.NodeName = gwf.NodeName;
            list.Title = msgTitle;
            list.Doc = msgDoc;
            list.CCTo = toEmpNo;
            list.CCToName = toEmpName;
            list.RDT = DataType.CurrentDataTime;
            list.Rec = WebUser.No;
            list.WorkID = gwf.WorkID;
            list.FID = gwf.FID;
            list.PFlowNo = gwf.PFlowNo;
            list.PWorkID = gwf.PWorkID;
            list.NDFrom = ndFrom;
            try
            {
                list.Insert();
            }
            catch
            {
                list.CheckPhysicsTable();
                list.Update();
            }

            // Logging .
            Glo.AddToTrack(ActionType.CC, gwf.FK_Flow, workID, gwf.FID, gwf.FK_Node, gwf.NodeName,
                WebUser.No, WebUser.Name, gwf.FK_Node, gwf.NodeName, toEmpNo, toEmpName, msgTitle, null);

            return " The success of the work has been copied to :" + toEmpNo + "," + toEmpName;
        }
        /// <summary>
        ///  Delete 
        /// </summary>
        /// <param name="mypk"> Delete </param>
        public static void Node_CC_DoDel(string mypk)
        {
            Paras ps = new Paras();
            ps.SQL = "DELETE WF_CCList WHERE MyPK=" + SystemConfig.AppCenterDBVarStr + "MyPK";
            ps.Add(CCListAttr.MyPK, mypk);
            BP.DA.DBAccess.RunSQL(ps);
        }
        /// <summary>
        ///  Set CC status 
        /// </summary>
        /// <param name="nodeID"> Node ID</param>
        /// <param name="workid"> The work ID</param>
        /// <param name="empNo"> Personnel Number </param>
        /// <param name="sta"> Status </param>
        public static void Node_CC_SetSta(int nodeID, Int64 workid, string empNo, CCSta sta)
        {
            string dbstr = SystemConfig.AppCenterDBVarStr;
            Paras ps = new Paras();
            ps.SQL = "UPDATE WF_CCList   SET Sta=" + dbstr + "Sta WHERE WorkID=" + dbstr + "WorkID AND FK_Node=" + dbstr + "FK_Node AND CCTo=" + dbstr + "CCTo";
            ps.Add(CCListAttr.Sta, (int)sta);
            ps.Add(CCListAttr.WorkID, workid);
            ps.Add(CCListAttr.FK_Node, nodeID);
            ps.Add(CCListAttr.CCTo, empNo);
            BP.DA.DBAccess.RunSQL(ps);
        }
        /// <summary>
        ///  Performs read 
        /// </summary>
        /// <param name="mypk"></param>
        public static void Node_CC_SetRead(string mypk)
        {
            if (string.IsNullOrEmpty(mypk))
                return;

            Paras ps = new Paras();
            ps.SQL = "UPDATE WF_CCList SET Sta=" + SystemConfig.AppCenterDBVarStr + "Sta  WHERE MyPK=" + SystemConfig.AppCenterDBVarStr + "MyPK";
            ps.Add(CCListAttr.Sta, (int)CCSta.Read);
            ps.Add(CCListAttr.MyPK, mypk);
            BP.DA.DBAccess.RunSQL(ps);
        }
        /// <summary>
        ///  Set CC reads 
        /// </summary>
        /// <param name="nodeID"> Node ID</param>
        /// <param name="workid"> The work ID</param>
        /// <param name="empNo"> Read Personnel Number </param>
        public static void Node_CC_SetRead(int nodeID, Int64 workid, string empNo)
        {
            Paras ps = new Paras();
            ps.SQL = "UPDATE WF_CCList SET Sta=" + SystemConfig.AppCenterDBVarStr + "Sta  WHERE WorkID=" + SystemConfig.AppCenterDBVarStr + "WorkID AND FK_Node=" + SystemConfig.AppCenterDBVarStr + "FK_Node AND CCTo=" + SystemConfig.AppCenterDBVarStr + "CCTo";
            ps.Add(CCListAttr.Sta, (int)CCSta.Read);
            ps.Add(CCListAttr.WorkID, workid);
            ps.Add(CCListAttr.FK_Node, nodeID);
            ps.Add(CCListAttr.CCTo, empNo);
            DBAccess.RunSQL(ps);
        }
        /// <summary>
        ///  Execution Cc 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="fk_node"> Node number </param>
        /// <param name="workID"> The work ID</param>
        /// <param name="toEmpNo"> Copied to staff numbers </param>
        /// <param name="toEmpName"> Copied to a person name </param>
        /// <param name="msgTitle"> Message title </param>
        /// <param name="msgDoc"> Message content </param>
        /// <param name="pFlowNo"> Parent process ID ( For null)</param>
        /// <param name="pWorkID"> Parent process WorkID( For 0)</param>
        /// <returns></returns>
        public static string Node_CC(string fk_flow, int fk_node, Int64 workID, string toEmpNo, string toEmpName, string msgTitle, string msgDoc, string pFlowNo, Int64 pWorkID)
        {
            Flow fl = new Flow(fk_flow);
            Node nd = new Node(fk_node);

            if (nd.CCWriteTo == CCWriteTo.Todolist)
            {
                /* If you want to write to-do */
                GenerWorkerList gwl = new GenerWorkerList();
                gwl.Retrieve(GenerWorkerListAttr.WorkID, workID, GenerWorkerListAttr.FK_Node, fk_node,
                    GenerWorkerListAttr.IsPass, 0);

                gwl.WorkID = workID;
                gwl.FK_Flow = fk_flow;
                gwl.FK_Node = fk_node;
                gwl.FK_NodeText = nd.Name;
                gwl.IsPassInt = 0;

                gwl.FK_Emp = toEmpNo;
                gwl.FK_EmpText = toEmpName;
                gwl.IsCC = true;
                try
                {
                    gwl.Insert();

                }
                catch (Exception ex)
                {
                    /* There may be , CC staff and personnel to repeat .*/
                    return "@ CC fails to perform , There may be staff (" + toEmpNo + "," + toEmpName + ") Repeat or , The staff needs to be run .";
                }
            }

            if (nd.CCWriteTo == CCWriteTo.All || nd.CCWriteTo == CCWriteTo.CCList)
            {
                CCList list = new CCList();
                list.MyPK = DBAccess.GenerOIDByGUID().ToString(); // workID + "_" + fk_node + "_" + empNo;
                list.FK_Flow = fk_flow;
                list.FlowName = fl.Name;
                list.FK_Node = fk_node;
                list.NodeName = nd.Name;
                list.Title = msgTitle;
                list.Doc = msgDoc;
                list.CCTo = toEmpNo;
                list.CCToName = toEmpName;

                list.RDT = DataType.CurrentDataTime;
                list.Rec = WebUser.No;
                list.WorkID = workID;
                list.FID = 0;
                list.PFlowNo = pFlowNo;
                list.PWorkID = pWorkID;

                try
                {
                    list.Insert();
                }
                catch
                {
                    list.CheckPhysicsTable();
                    list.Update();
                }
            }

            GenerWorkFlow gwf = new GenerWorkFlow(workID);

            // Logging .
            Glo.AddToTrack(ActionType.CC, fk_flow, workID, 0, nd.NodeID, nd.Name,
                WebUser.No, WebUser.Name, nd.NodeID, nd.Name, toEmpNo, toEmpName, msgTitle, null);

            // Send e-mail .
            BP.WF.Dev2Interface.Port_SendMsg(toEmpNo, WebUser.Name + " The work :" + gwf.Title, " Cc :" + msgTitle, "CC" + nd.NodeID + "_" + workID + "_", BP.WF.SMSMsgType.CC,
                gwf.FK_Flow, gwf.FK_Node, gwf.WorkID, gwf.FID);

            return " The success of the work has been copied to :" + toEmpNo + "," + toEmpName;

        }
        /// <summary>
        ///  Delete Draft 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="workID"> The work ID</param>
        public static void Node_DeleteDraft(string fk_flow, Int64 workID)
        {
            // Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            // Set engine table .
            GenerWorkFlow gwf = new GenerWorkFlow();
            gwf.WorkID = workID;
            if (gwf.RetrieveFromDBSources() == 1)
            {
                if (gwf.FK_Node != int.Parse(fk_flow + "01"))
                    throw new Exception("@ The flow of non-draft process can not be deleted :" + gwf.Title);

                if (gwf.WFState != WFState.Draft)
                    throw new Exception("@ Non-draft state can not be deleted ");

                gwf.Delete();
            }

            // Delete Process .
            string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
            Flow fl = new Flow(fk_flow);
            Paras ps = new Paras();
            ps.SQL = "DELETE FROM " + fl.PTable + " WHERE OID=" + dbstr + "OID ";
            ps.Add(GERptAttr.OID, workID);
            DBAccess.RunSQL(ps);


            // Delete the starting node data .
            try
            {
                ps = new Paras();
                ps.SQL = "DELETE FROM ND" + int.Parse(fk_flow + "01") + " WHERE OID=" + dbstr + "OID ";
                ps.Add(GERptAttr.OID, workID);
                DBAccess.RunSQL(ps);
            }
            catch
            {
            }

        }
        /// <summary>
        ///  The draft is set to be run 
        /// </summary>
        /// <param name="fk_flow"></param>
        /// <param name="workID"></param>
        public static void Node_SetDraft2Todolist(string fk_flow, Int64 workID)
        {

        }
        /// <summary>
        ///  Set the current work status as a draft , If you enable the draft ,  Please increase it in the form on the Save button start node .
        ///  Watch out : Must be invoked at the start node .
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="workID"> The work ID</param>
        public static void Node_SetDraft(string fk_flow, Int64 workID)
        {
            // Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            // Set engine table .
            GenerWorkFlow gwf = new GenerWorkFlow();
            gwf.WorkID = workID;
            if (gwf.RetrieveFromDBSources() == 1)
            {
                if (gwf.FK_Node != int.Parse(fk_flow + "01"))
                    throw new Exception("@ Setting draft error , Only at the beginning of the node can be set Drafts , The node is now :" + gwf.Title);

                // Set to Draft .
                gwf.Update(GenerWorkFlowAttr.WFState, (int)WFState.Draft);
            }

            // Setting Process Data Sheet .
            string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
            Flow fl = new Flow(fk_flow);
            Paras ps = new Paras();
            ps.SQL = "UPDATE " + fl.PTable + " SET WFState=" + dbstr + "WFState WHERE OID=" + dbstr + "OID ";
            ps.Add(GERptAttr.WFState, (int)WFState.Draft);
            ps.Add(GERptAttr.OID, workID);
            DBAccess.RunSQL(ps);
        }
        /// <summary>
        ///  Save 
        /// </summary>
        /// <param name="nodeID"> Node ID</param>
        /// <param name="workID"> The work ID</param>
        /// <returns> Returns the stored information </returns>
        public static string Node_SaveWork(string fk_flow, int fk_node, Int64 workID)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            return Node_SaveWork(fk_flow, fk_node, workID, new Hashtable(), null);
        }
        /// <summary>
        ///  Save 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="workID">workid</param>
        /// <param name="wk"> Nodes form parameters </param>
        /// <returns></returns>
        public static string Node_SaveWork(string fk_flow, int fk_node, Int64 workID, Hashtable wk)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            return Node_SaveWork(fk_flow, fk_node, workID, wk, null);
        }
        /// <summary>
        ///  Save 
        /// </summary>
        /// <param name="nodeID"> Node ID</param>
        /// <param name="workID"> The work ID</param>
        /// <param name="htWork"> Job data </param>
        /// <returns> Returns execution information </returns>
        public static string Node_SaveWork(string fk_flow, int fk_node, Int64 workID, Hashtable htWork, DataSet dsDtls)
        {
            if (htWork == null)
                return " Parameter error , Failed to save .";

            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            try
            {
                Node nd = new Node(fk_node);
                Work wk = nd.HisWork;
                if (workID != 0)
                {
                    wk.OID = workID;
                    wk.RetrieveFromDBSources();
                }
                wk.ResetDefaultVal();

                if (htWork != null)
                {
                    foreach (string str in htWork.Keys)
                    {
                        switch (str)
                        {
                            case StartWorkAttr.OID:
                            case StartWorkAttr.CDT:
                            case StartWorkAttr.MD5:
                            case StartWorkAttr.Emps:
                            case StartWorkAttr.FID:
                            case StartWorkAttr.FK_Dept:
                            case StartWorkAttr.PRI:
                            case StartWorkAttr.Rec:
                            case StartWorkAttr.Title:
                                continue;
                            default:
                                break;
                        }

                        if (wk.Row.ContainsKey(str))
                            wk.SetValByKey(str, htWork[str]);
                        else
                            wk.Row.Add(str, htWork[str]);
                    }
                }

                wk.Rec = WebUser.No;
                wk.RecText = WebUser.Name;
                wk.SetValByKey(StartWorkAttr.FK_Dept, WebUser.FK_Dept);
                wk.BeforeSave();
                wk.Save();

                #region  Save from the table 
                if (dsDtls != null)
                {
                    // Save from the table 
                    foreach (DataTable dt in dsDtls.Tables)
                    {
                        foreach (MapDtl dtl in wk.HisMapDtls)
                        {
                            if (dt.TableName != dtl.No)
                                continue;
                            // Get dtls
                            GEDtls daDtls = new GEDtls(dtl.No);
                            daDtls.Delete(GEDtlAttr.RefPK, workID); //  Clear existing data .

                            GEDtl daDtl = daDtls.GetNewEntity as GEDtl;
                            daDtl.RefPK = workID.ToString();

                            //  To copy the data from the table .
                            foreach (DataRow dr in dt.Rows)
                            {
                                // Details column .
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    // Setting properties .
                                    daDtl.SetValByKey(dc.ColumnName, dr[dc.ColumnName]);
                                }

                                daDtl.ResetDefaultVal();

                                daDtl.RefPK = workID.ToString();
                                daDtl.RDT = DataType.CurrentDataTime;

                                // The save .
                                if (daDtl.OID == 0)
                                    daDtl.InsertAsOID(DBAccess.GenerOID("Dtl")); // Insert data .
                                else
                                    daDtl.InsertAsOID(daDtl.OID); // Insert data .
                            }
                        }
                    }
                }
                #endregion  Save from the table end 

                #region  Send parameter update .
                if (htWork != null)
                {
                    string paras = "";
                    foreach (string key in htWork.Keys)
                        paras += "@" + key + "=" + htWork[key].ToString();

                    if (string.IsNullOrEmpty(paras) == false)
                    {
                        string dbstr = SystemConfig.AppCenterDBVarStr;
                        Paras ps = new Paras();
                        ps.SQL = "UPDATE WF_GenerWorkerlist SET AtPara=" + dbstr + "Paras WHERE WorkID=" + dbstr + "WorkID AND FK_Node=" + dbstr + "FK_Node";
                        ps.Add(GenerWorkerListAttr.Paras, paras);
                        ps.Add(GenerWorkerListAttr.WorkID, workID);
                        ps.Add(GenerWorkerListAttr.FK_Node, nd.NodeID);
                        DBAccess.RunSQL(ps);
                    }
                }
                #endregion  Send parameter update .


                if (nd.SaveModel == SaveModel.NDAndRpt)
                {
                    /*  If you save mode is the node table with Node与Rpt表. */
                    WorkNode wn = new WorkNode(wk, nd);
                    GERpt rptGe = nd.HisFlow.HisGERpt;
                    rptGe.SetValByKey("OID", workID);
                    wn.rptGe = rptGe;
                    if (rptGe.RetrieveFromDBSources() == 0)
                    {
                        rptGe.SetValByKey("OID", workID);
                        wn.DoCopyRptWork(wk);

                        if (Glo.UserInfoShowModel == UserInfoShowModel.UserIDUserName)
                            rptGe.SetValByKey(GERptAttr.FlowEmps, "@" + WebUser.No + "," + WebUser.Name);

                        if (Glo.UserInfoShowModel == UserInfoShowModel.UserIDOnly)
                            rptGe.SetValByKey(GERptAttr.FlowEmps, "@" + WebUser.No);

                        if (Glo.UserInfoShowModel == UserInfoShowModel.UserNameOnly)
                            rptGe.SetValByKey(GERptAttr.FlowEmps, "@" + WebUser.Name);

                        rptGe.SetValByKey(GERptAttr.FlowStarter, WebUser.No);
                        rptGe.SetValByKey(GERptAttr.FlowStartRDT, DataType.CurrentDataTime);
                        rptGe.SetValByKey(GERptAttr.WFState, 0);
                        rptGe.SetValByKey(GERptAttr.FK_NY, DataType.CurrentYearMonth);
                        rptGe.SetValByKey(GERptAttr.FK_Dept, WebUser.FK_Dept);
                        rptGe.Insert();
                    }
                    else
                    {
                        wn.DoCopyRptWork(wk);
                        rptGe.Update();
                    }
                }

                #region  Create a to-do to start work 
                GenerWorkFlow gwf = new GenerWorkFlow();
                Flow fl = new Flow(fk_flow);
                gwf.WorkID = workID;
                int i = gwf.RetrieveFromDBSources();
                if (i == 0)
                {
                    gwf.FlowName = fl.Name;
                    gwf.FK_Flow = fk_flow;
                    gwf.FK_FlowSort = fl.FK_FlowSort;

                    gwf.FK_Dept = WebUser.FK_Dept;
                    gwf.DeptName = WebUser.FK_DeptName;
                    gwf.FK_Node = fl.StartNodeID;

                    gwf.NodeName = nd.Name;
                    gwf.WFState = WFState.Runing;
                    gwf.Title = BP.WF.WorkNode.GenerTitle(fl, wk);


                    gwf.Starter = WebUser.No;
                    gwf.StarterName = WebUser.Name;
                    gwf.RDT = DataType.CurrentDataTime;
                    gwf.Insert();

                    //  Generate job listings .
                    GenerWorkerList gwl = new GenerWorkerList();
                    gwl.WorkID = workID;
                    gwl.FK_Emp = WebUser.No;
                    gwl.FK_EmpText = WebUser.Name;

                    gwl.FK_Node = fk_node;
                    gwl.FK_NodeText = nd.Name;
                    gwl.FID = 0;

                    gwl.FK_Flow = fk_flow;
                    gwl.FK_Dept = WebUser.FK_Dept;
                    gwl.SDT = DataType.CurrentDataTime;
                    gwl.DTOfWarning = DataType.CurrentDataTime;
                    gwl.RDT = DataType.CurrentDataTime;
                    gwl.IsEnable = true;

                    gwl.IsPass = false;
                    gwl.Sender = WebUser.No;
                    gwl.PRI = gwf.PRI;
                    gwl.Insert();
                }
                #endregion  Create a to-do to start work 

                return " Saved successfully .";
            }
            catch (Exception ex)
            {
                return " Failed to save :" + ex.Message;
            }
        }
        /// <summary>
        ///  Save the process form 
        /// </summary>
        /// <param name="fk_mapdata"> Process Form ID</param>
        /// <param name="workID"> The work ID</param>
        /// <param name="htData"> Process form data Key Value  Format store .</param>
        /// <returns> Returns execution information </returns>
        public static void Node_SaveFlowSheet(string fk_mapdata, Int64 workID, Hashtable htData)
        {
            Node_SaveFlowSheet(fk_mapdata, workID, htData, null);
        }
        /// <summary>
        ///  Save the process form 
        /// </summary>
        /// <param name="fk_mapdata"> Process Form ID</param>
        /// <param name="workID"> The work ID</param>
        /// <param name="htData"> Process form data Key Value  Format store .</param>
        /// <param name="workDtls"> Data from the table </param>
        /// <returns> Returns execution information </returns>
        public static void Node_SaveFlowSheet(string fk_mapdata, Int64 workID, Hashtable htData, DataSet workDtls)
        {
            MapData md = new MapData(fk_mapdata);
            GEEntity en = md.HisGEEn;
            en.SetValByKey("OID", workID);
            int i = en.RetrieveFromDBSources();

            foreach (string key in htData.Keys)
                en.SetValByKey(key, htData[key].ToString());

            en.SetValByKey("OID", workID);

            FrmEvents fes = md.FrmEvents;
            fes.DoEventNode(FrmEventList.SaveBefore, en);
            if (i == 0)
                en.Insert();
            else
                en.Update();

            if (workDtls != null)
            {
                MapDtls dtls = new MapDtls(fk_mapdata);
                // Save from the table 
                foreach (DataTable dt in workDtls.Tables)
                {
                    foreach (MapDtl dtl in dtls)
                    {
                        if (dt.TableName != dtl.No)
                            continue;
                        // Get dtls
                        GEDtls daDtls = new GEDtls(dtl.No);
                        daDtls.Delete(GEDtlAttr.RefPK, workID); //  Clear existing data .

                        GEDtl daDtl = daDtls.GetNewEntity as GEDtl;
                        daDtl.RefPK = workID.ToString();

                        //  To copy the data from the table .
                        foreach (DataRow dr in dt.Rows)
                        {
                            daDtl.ResetDefaultVal();
                            daDtl.RefPK = workID.ToString();

                            // Details column .
                            foreach (DataColumn dc in dt.Columns)
                            {
                                // Setting properties .
                                daDtl.SetValByKey(dc.ColumnName, dr[dc.ColumnName]);
                            }
                            daDtl.InsertAsOID(DBAccess.GenerOID("Dtl")); // Insert data .
                        }
                    }
                }
            }
            fes.DoEventNode(FrmEventList.SaveAfter, en);
        }
        /// <summary>
        ///  Take out a sub-task from the task pool 
        /// </summary>
        /// <param name="nodeid"> Node number </param>
        /// <param name="workid"> The work ID</param>
        /// <param name="empNo"> Taken out of the personnel numbers </param>
        public static bool Node_TaskPoolTakebackOne(Int64 workid)
        {
            if (Glo.IsEnableTaskPool == false)
                throw new Exception("@ Configuration is not set to the status of a shared task pool .");

            GenerWorkFlow gwf = new GenerWorkFlow(workid);
            if (gwf.TaskSta == TaskSta.None)
                throw new Exception("@ The task unshared task .");

            if (gwf.TaskSta == TaskSta.Takeback)
                throw new Exception("@ This task has been taken away by others .");

            // Update Status .
            gwf.TaskSta = TaskSta.Takeback;
            gwf.Update();

            string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
            Paras ps = new Paras();
            // Settings have been removed in the state .
            ps.SQL = "UPDATE WF_GenerWorkerlist SET IsEnable=-1 WHERE IsEnable=1 AND WorkID=" + dbstr + "WorkID AND FK_Node=" + dbstr + "FK_Node AND FK_Emp!=" + dbstr + "FK_Emp ";
            ps.Add(GenerWorkerListAttr.WorkID, workid);
            ps.Add(GenerWorkerListAttr.FK_Node, gwf.FK_Node);
            ps.Add(GenerWorkerListAttr.FK_Emp, BP.Web.WebUser.No);
            int i = DBAccess.RunSQL(ps);

            BP.WF.Dev2Interface.WriteTrackInfo(gwf.FK_Flow, gwf.FK_Node, workid, 0, " The task is " + WebUser.Name + " Removed from the task pool .", " Get ");
            if (i == 1)
                return true;
            else
                return false;
        }
        /// <summary>
        ///  Add a task 
        /// </summary>
        /// <param name="nodeid"> Node number </param>
        /// <param name="workid"> The work ID</param>
        /// <param name="empNo"> Staff ID</param>
        public static void Node_TaskPoolPutOne(Int64 workid)
        {
            if (Glo.IsEnableTaskPool == false)
                throw new Exception("@ Configuration is not set to the status of a shared task pool .");

            GenerWorkFlow gwf = new GenerWorkFlow(workid);
            if (gwf.TaskSta == TaskSta.None)
                throw new Exception("@ The task unshared task .");

            if (gwf.TaskSta == TaskSta.Sharing)
                throw new Exception("@ This task has been shared state .");

            //  Update   Status .
            gwf.TaskSta = TaskSta.Sharing;
            gwf.Update();

            string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
            Paras ps = new Paras();
            // Settings have been removed in the state .
            ps.SQL = "UPDATE WF_GenerWorkerlist SET IsEnable=1 WHERE IsEnable=-1 AND WorkID=" + dbstr + "WorkID ";
            ps.Add(GenerWorkerListAttr.WorkID, workid);
            int i = DBAccess.RunSQL(ps);
            if (i == 0)
                throw new Exception("@ Process data error , Should not be less than the data update .");

            BP.WF.Dev2Interface.WriteTrackInfo(gwf.FK_Flow, gwf.FK_Node, workid, 0, " The task is " + WebUser.Name + " Into the task pool .", " Add ");
        }
        /// <summary>
        ///  The next step is to increase acceptance of people ( Increase for the current step down when the recipient sends a step )
        /// </summary>
        /// <param name="workID"> The work ID</param>
        /// <param name="formNodeID"> Node ID</param>
        /// <param name="emps"> If more then separated by commas </param>
        public static void Node_AddNextStepAccepters(Int64 workID, int formNodeID, string emps)
        {
            SelectAccper sa = new SelectAccper();
            sa.Delete(SelectAccperAttr.FK_Node, formNodeID, SelectAccperAttr.WorkID, workID);
            emps = emps.Replace(" ", "");
            emps = emps.Replace(";", ",");
            emps = emps.Replace("@", ",");
            string[] strs = emps.Split(',');

            Emp empEn = new Emp();
            foreach (string emp in strs)
            {
                if (string.IsNullOrEmpty(emp))
                    continue;
                sa.MyPK = formNodeID + "_" + workID + "_" + emp;

                empEn.No = emp;
                int i = empEn.RetrieveFromDBSources();
                if (i == 0)
                    continue;

                sa.FK_Emp = emp;
                sa.EmpName = empEn.Name;

                sa.FK_Node = formNodeID;
                sa.WorkID = workID;
                sa.Insert();
            }
        }
        /// <summary>
        ///  Node work hangs 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="workid"> The work ID</param>
        /// <param name="way"> Suspend Mode </param>
        /// <param name="reldata"> Lifting suspend date ( Can be empty )</param>
        /// <param name="hungNote"> Pending reason </param>
        /// <returns> Returns execution information </returns>
        public static string Node_HungUpWork(string fk_flow, Int64 workid, int wayInt, string reldata, string hungNote)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            HungUpWay way = (HungUpWay)wayInt;
            BP.WF.WorkFlow wf = new WorkFlow(fk_flow, workid);
            return wf.DoHungUp(way, reldata, hungNote);
        }
        /// <summary>
        ///  Node work unsuspend 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="workid"> The work ID</param>
        /// <param name="msg"> Unsuspend reason </param>
        /// <returns> Execution information </returns>
        public static void Node_UnHungUpWork(string fk_flow, Int64 workid, string msg)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);
            BP.WF.WorkFlow wf = new WorkFlow(fk_flow, workid);
            wf.DoUnHungUp();
        }
        /// <summary>
        ///  Get hang time on the node 
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="nodeID"> Node ID</param>
        /// <param name="workid"> The work ID</param>
        /// <returns> Return time series , If there are no pending action throws an exception .</returns>
        public static TimeSpan Node_GetHungUpTimeSpan(string flowNo, int nodeID, Int64 workid)
        {
            string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;

            string instr = (int)ActionType.HungUp + "," + (int)ActionType.UnHungUp;
            Paras ps = new Paras();
            ps.SQL = "SELECT * FROM ND" + int.Parse(flowNo) + "Track WHERE WorkID=" + dbstr + "WorkID AND " + TrackAttr.ActionType + " in (" + instr + ")  and  NDFrom=" + dbstr + "NDFrom ";
            ps.Add(TrackAttr.WorkID, workid);
            ps.Add(TrackAttr.NDFrom, nodeID);
            DataTable dt = DBAccess.RunSQLReturnTable(ps);

            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            foreach (DataRow item in dt.Rows)
            {
                ActionType at = (ActionType)item[TrackAttr.ActionType];

                // Hang time .
                if (at == ActionType.HungUp)
                    dtStart = DataType.ParseSysDateTime2DateTime(item[TrackAttr.RDT].ToString());

                // Lift hang time .
                if (at == ActionType.UnHungUp)
                    dtEnd = DataType.ParseSysDateTime2DateTime(item[TrackAttr.RDT].ToString());
            }

            TimeSpan ts = dtEnd - dtStart;
            return ts;
        }



        /// <summary>
        ///  Execution endorsement 
        /// </summary>
        /// <param name="workid"> The work ID</param>
        /// <param name="askfor"> Plus sign way </param>
        /// <param name="askForEmp"> Request staff </param>
        /// <param name="askForNote"> Content </param>
        /// <returns></returns>
        public static string Node_Askfor(Int64 workid, AskforHelpSta askforSta, string askForEmp, string askForNote)
        {

            Emp emp = new Emp(askForEmp);
            BP.WF.GenerWorkFlow gwf = new GenerWorkFlow(workid);
            // throw new Exception("@ This work belongs to rush to do the work , You can not perform endorsement .");

            if (Flow_IsCanDoCurrentWork(gwf.FK_Node, gwf.WorkID, WebUser.No) == false)
                throw new Exception("@ Privileges of the current work has been processed to others or you do not handle the job .");

            gwf.WFState = WFState.Askfor; //  Update process for endorsement status .
            gwf.Update();

            //  Set the current state  2  Said the endorsement status .
            GenerWorkerLists gwls = new GenerWorkerLists(workid, gwf.FK_Node);
            if (gwls.Contains(GenerWorkerListAttr.FK_Emp, askForEmp, GenerWorkerListAttr.IsEnable, 0) == true)
                throw new Exception(" Endorsement failure , Endorsement of your choice who can handle the current work .");

            foreach (GenerWorkerList item in gwls)
            {
                if (item.IsEnable == false)
                    continue;

 

                if (item.FK_Emp == WebUser.No)
                {
                    // GenerWorkerList gwl = gwls[0] as GenerWorkerList;
                    item.IsPassInt = (int)askforSta;
                    item.Update();

                    //  After replacing the primary key , Carried out insert , Let people have been endorsement agency work .
                    item.IsPassInt = 0;
                    item.FK_Emp = emp.No;
                    item.FK_EmpText = emp.Name;
                    try
                    {
                        item.Insert();
                    }
                    catch
                    {
                        item.Update();
                    }
                }
                else
                {
                    item.IsEnable = false; // = (int)askforSta;
                    item.Update();
                }
            }


            BP.WF.Dev2Interface.WriteTrack(gwf.FK_Flow, gwf.FK_Node, workid, gwf.FID, askForNote, ActionType.AskforHelp, "", null, null);
            Flow fl = new Flow(gwf.FK_Flow);
            // Update Status .
            DBAccess.RunSQL("UPDATE " + fl.PTable + " SET WFState=" + (int)WFState.Askfor + " WHERE OID=" + workid);

            // Work is set to Unread .
            BP.WF.Dev2Interface.Node_SetWorkUnRead(gwf.FK_Node, workid);

            string msg = " Your work has been submitted to the (" + askForEmp + " " + emp.Name + ") Canada signed .";

            // Plus sign after the incident 
            BP.WF.Node hisNode = new BP.WF.Node(gwf.FK_Node);
            Work currWK = hisNode.HisWork;
            currWK.OID = gwf.WorkID;
            currWK.Retrieve();
            msg += fl.DoFlowEventEntity(EventListOfNode.AskerAfter, hisNode, currWK, null);

            return msg;
        }
        /// <summary>
        ///  Reply endorsement information 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="fk_node"> Node number </param>
        /// <param name="workid"> The work ID</param>
        /// <param name="fid">FID</param>
        /// <param name="replyNote"> Reply message </param>
        /// <returns></returns>
        public static string Node_AskforReply(string fk_flow, int fk_node, Int64 workid, Int64 fid, string replyNote)
        {
            //  The written reply message temporary   Process registration information table in order to allow the transmission method to obtain this information into the log .
            GenerWorkFlow gwf = new GenerWorkFlow(workid);
            gwf.Paras_AskForReply = replyNote;
            gwf.Update();

            // Performing transmission ,  In the process of sending a judgment which has been done , And the   The information is written to the log reply .
            string info = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid).ToMsgOfHtml();

            Node node = new Node(fk_node);
            // Recovery after endorsement execution events 
            info += node.HisFlow.DoFlowEventEntity(EventListOfNode.AskerReAfter, node, node.HisWork, null);
            return info;
        }
        /// <summary>
        ///  Job transfer 
        /// </summary>
        /// <param name="workid"> The work ID</param>
        /// <param name="toEmp"> Transferred to the staff ( Only to be handed over to a person )</param>
        /// <param name="msg"> Transfer news </param>
        public static string Node_Shift(string flowNo, int nodeID, Int64 workID, Int64 fid, string toEmp, string msg)
        {
            //  Staff .
            Emp emp = new Emp(toEmp);
            Node nd = new Node(nodeID);

            if (nd.TodolistModel == TodolistModel.Order || nd.TodolistModel == TodolistModel.Teamup)
            {
                /* If the queue mode , Or cooperative mode . */
                try
                {
                    string sql = "UPDATE WF_GenerWorkerlist SET FK_Emp='" + emp.No + "', FK_EmpText='" + emp.Name + "' WHERE FK_Emp='" + WebUser.No + "' AND FK_Node=" + nodeID + " AND WorkID=" + workID;
                    BP.DA.DBAccess.RunSQL(sql);
                }
                catch
                {
                    return "@ Transfer failure , You have transferred staff (" + emp.No + " " + emp.Name + ") Already in the agency list .";
                }

                // Logging .
                Glo.AddToTrack(ActionType.Shift, nd.FK_Flow, workID, fid, nd.NodeID, nd.Name,
                    WebUser.No, WebUser.Name, nd.NodeID, nd.Name, toEmp, emp.Name, msg, null);

                string info = "@ Job transfer success .@ You have successfully handed over the work to :" + emp.No + " , " + emp.Name;

                // After the handover event 
                info += "@" + nd.HisFlow.DoFlowEventEntity(EventListOfNode.ShitAfter, nd, nd.HisWork, null);

                info += "@<a href='" + Glo.CCFlowAppPath + "WF/MyFlowInfo.aspx?DoType=UnShift&FK_Flow=" + nd.FK_Flow + "&WorkID=" + workID + "&FK_Node=" + nodeID + "&FID=" + fid + "' ><img src='Img/UnDo.gif' border=0 /> Undo the handover of work </a>.";
                return info;

            }

            GenerWorkFlow gwf = new GenerWorkFlow();
            gwf.WorkID = workID;
            if (gwf.RetrieveFromDBSources() == 0)
            {
                /* Description Start node data transfer form .*/
                gwf.WorkID = workID;
                gwf.Title = "由" + WebUser.No + " ; " + WebUser.Name + ", 在(" + DataType.CurrentDataCNOfShort + ") Transfer to work ";
                gwf.FK_Dept = WebUser.FK_Dept;
                gwf.FK_Flow = flowNo;

                Flow fl = new Flow(flowNo);
                gwf.FK_FlowSort = fl.FK_FlowSort;
                gwf.FK_Node = nodeID;
                gwf.Starter = emp.No;
                gwf.StarterName = emp.Name;
                gwf.WFState = WFState.Runing;
                gwf.TodoEmps = toEmp;
                gwf.TodoEmpsNum = 1;
                gwf.RDT = DataType.CurrentDataTime;
                gwf.NodeName = "";
                gwf.FlowName = fl.Name;
                gwf.Emps = toEmp;
                gwf.DeptName = WebUser.FK_DeptName;
                gwf.Insert();

                GenerWorkerList gwl = new GenerWorkerList();
                gwl.WorkID = workID;
                gwl.FK_Dept = WebUser.FK_Dept;

                //gwl.FK_DeptT = WebUser.FK_DeptName;
                gwl.FK_Node = nodeID;
                gwl.FK_NodeText = nd.Name;

                gwl.FK_Emp = toEmp;
                gwl.FK_EmpText = emp.Name;

                gwl.FK_Flow = flowNo;

                gwl.IsPass = false;
                gwl.IsPassInt = 0;
                gwl.IsRead = false;
                gwl.PressTimes = 0;
                gwl.RDT = gwf.RDT;
                gwl.SDT = gwf.RDT;
                gwl.Sender = WebUser.No;
                gwl.Insert();
            }
            else
            {
                //  Delete work with the current non- .
                //  Has a non-coordinating or automatically assigned tasks .
                // Set all the staff can not handle .
                string dbStr = SystemConfig.AppCenterDBVarStr;
                Paras ps = new Paras();
                ps.SQL = "UPDATE WF_GenerWorkerlist SET IsEnable=0  WHERE WorkID=" + dbStr + "WorkID AND FK_Node=" + dbStr + "FK_Node";
                ps.Add(GenerWorkerListAttr.WorkID, workID);
                ps.Add(GenerWorkerListAttr.FK_Node, nodeID);
                DBAccess.RunSQL(ps);


                // Set to be handed people FK_Emp  Current treatment for people ,（ There are likely to be handed over the list of people not working , Returns 0.）
                ps = new Paras();
                ps.SQL = "UPDATE WF_GenerWorkerlist SET IsEnable=1  WHERE WorkID=" + dbStr + "WorkID AND FK_Node=" + dbStr + "FK_Node AND FK_Emp=" + dbStr + "FK_Emp";
                ps.Add(GenerWorkerListAttr.WorkID, workID);
                ps.Add(GenerWorkerListAttr.FK_Node, nodeID);
                ps.Add(GenerWorkerListAttr.FK_Emp, toEmp);
                int i = DBAccess.RunSQL(ps);

                GenerWorkerLists wls = null;
                GenerWorkerList wl = null;
                if (i == 0)
                {
                    /* Explanation :  With people on other jobs to handle , He gave work to increase share .*/
                    wls = new GenerWorkerLists(workID, nodeID);
                    wl = wls[0] as GenerWorkerList;
                    wl.FK_Emp = toEmp.ToString();
                    wl.FK_EmpText = emp.Name;
                    wl.IsEnable = true;
                    wl.Insert();

                    //  Clear worker , Used for forwarding messages .
                    wls.Clear();
                    wls.AddEntity(wl);
                }
            }

            ShiftWork sf = new ShiftWork();
            sf.WorkID = workID;
            sf.FK_Node = nodeID;
            sf.ToEmp = toEmp;
            sf.ToEmpName = emp.Name;
            sf.Note = msg;
            sf.FK_Emp = WebUser.No;
            sf.FK_EmpName = WebUser.Name;
            sf.Insert();
            // Logging .
            Glo.AddToTrack(ActionType.Shift, nd.FK_Flow, workID, gwf.FID, nd.NodeID, nd.Name,
                WebUser.No, WebUser.Name, nd.NodeID, nd.Name, toEmp, emp.Name, msg, null);

            // Send e-mail .
            BP.WF.Dev2Interface.Port_SendMsg(emp.No, WebUser.Name + " Handed over the job to you " + gwf.Title, " Transfer information :" + msg, "SF" + workID + "_" + sf.FK_Node, BP.WF.SMSMsgType.Self, gwf.FK_Flow, gwf.FK_Node, gwf.WorkID, gwf.FID);

            string inf1o = "@ Job transfer success .@ You have successfully handed over the work to :" + emp.No + " , " + emp.Name;

            // After the handover event 
            inf1o += "@" + nd.HisFlow.DoFlowEventEntity(EventListOfNode.ShitAfter, nd, nd.HisWork, null);

            inf1o += "@<a href='" + Glo.CCFlowAppPath + "WF/MyFlowInfo.aspx?DoType=UnShift&FK_Flow=" + nd.FK_Flow + "&WorkID=" + workID + "&FK_Node=" + nodeID + "&FID=" + fid + "' ><img src='Img/UnDo.gif' border=0 /> Undo the handover of work </a>.";
            return inf1o;
        }
        /// <summary>
        ///  Implementation of return ( Returned to the specified point )
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="workID"> The work ID</param>
        /// <param name="fid"> Process ID</param>
        /// <param name="currentNodeID"> The current node ID</param>
        /// <param name="returnToNodeID"> Return to work ID</param>
        /// <param name="returnToEmp"> Back to staff </param>
        /// <param name="msg"> Reason for the return </param>
        /// <param name="isBackToThisNode"> Do you want to backtrack after return ?</param>
        /// <returns> Execution results , The results must be presented to the user .</returns>
        public static string Node_ReturnWork(string fk_flow, Int64 workID, Int64 fid, int currentNodeID, int returnToNodeID,
            string returnToEmp, string msg, bool isBackToThisNode)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);
            WorkReturn wr = new WorkReturn(fk_flow, workID, fid, currentNodeID, returnToNodeID, returnToEmp, isBackToThisNode, msg);
            return wr.DoIt();
        }
        public static string Node_ReturnWork(string fk_flow, Int64 workID, Int64 fid, int currentNodeID, int returnToNodeID, string msg, bool isBackToThisNode)
        {
            return Node_ReturnWork(fk_flow, workID, fid, currentNodeID, returnToNodeID, null, msg, isBackToThisNode);
        }
        /// <summary>
        ///  Get the current work NodeID
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="workid"> The work ID</param>
        /// <returns> Designated work NodeID.</returns>
        public static int Node_GetCurrentNodeID(string fk_flow, Int64 workid)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            int nodeID = BP.DA.DBAccess.RunSQLReturnValInt("SELECT FK_Node FROM WF_GenerWorkFlow WHERE WorkID=" + workid + " AND FK_Flow='" + fk_flow + "'", 0);
            if (nodeID == 0)
                return int.Parse(fk_flow + "01");
            return nodeID;
        }

        /// <summary>
        ///  Remove the child thread 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="fid"> Process ID</param>
        /// <param name="workid"> The work ID</param>
        public static void Node_FHL_KillSubFlow(string fk_flow, Int64 fid, Int64 workid)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            WorkFlow wkf = new WorkFlow(fk_flow, workid);
            wkf.DoDeleteWorkFlowByReal(true);
        }
        /// <summary>
        ///  Confluence dismissed child thread 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="fid"> Process ID</param>
        /// <param name="workid"> Child thread ID</param>
        /// <param name="msg"> Dismissed the news </param>
        public static string Node_FHL_DoReject(string fk_flow, int NodeSheetfReject, Int64 fid, Int64 workid, string msg)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            WorkFlow wkf = new WorkFlow(fk_flow, workid);
            return wkf.DoReject(fid, NodeSheetfReject, msg);
        }

        /// <summary>
        ///  Jump to retrieve audit 
        /// </summary>
        /// <param name="fromNodeID"> From node ID</param>
        /// <param name="workid"> The work ID</param>
        /// <param name="tackToNodeID"> Get back to the node ID</param>
        /// <returns></returns>
        public static string Node_Tackback(int fromNodeID, Int64 workid, int tackToNodeID)
        {
            /*
             * 1, First check whether this permission .
             * 2,  Implementation Jump .
             * 3,  Write to the log .
             */
            Node nd = new Node(tackToNodeID);
            switch (nd.HisDeliveryWay)
            {
                case DeliveryWay.ByPreviousNodeFormEmpsField:
                    break;
            }

            WorkNode wn = new WorkNode(workid, fromNodeID);
            string msg = wn.NodeSend(new Node(tackToNodeID), BP.Web.WebUser.No).ToMsgOfHtml();
            wn.AddToTrack(ActionType.Tackback, WebUser.No, WebUser.Name, tackToNodeID, nd.Name, " Execution jumps to retrieve audit .");
            return msg;
        }

        /// <summary>
        ///  CC have read execution 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="fk_node"> Process Node </param>
        /// <param name="workid"> The work id</param>
        /// <param name="fid"> Process id</param>
        /// <param name="checkNote"> Fill views </param>
        public static void Node_DoCCCheckNote(string fk_flow, int fk_node, Int64 workid, Int64 fid, string checkNote)
        {
            BP.Sys.FrmWorkCheck fwc = new BP.Sys.FrmWorkCheck(fk_node);

            BP.WF.Dev2Interface.WriteTrackWorkCheck(fk_flow, fk_node, workid,
                fid, checkNote, fwc.FWCOpLabel);

            // Setting audit completed .
            BP.WF.Dev2Interface.Node_CC_SetSta(fk_node, workid, BP.Web.WebUser.No, BP.WF.Template.CCSta.CheckOver);

        }
        /// <summary>
        ///  This work is set to read status 
        /// </summary>
        /// <param name="nodeID"> Node ID</param>
        /// <param name="workid">WorkID</param>
        public static void Node_SetWorkRead(int nodeID, Int64 workid)
        {
            string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
            Paras ps = new Paras();
            ps.SQL = "UPDATE WF_GenerWorkerList SET IsRead=1 WHERE WorkID=" + dbstr + "WorkID AND FK_Node=" + dbstr + "FK_Node AND FK_Emp=" + dbstr + "FK_Emp";
            ps.Add("WorkID", workid);
            ps.Add("FK_Node", nodeID);
            ps.Add("FK_Emp", WebUser.No);
            if (DBAccess.RunSQL(ps) == 0)
                throw new Exception("@ Operation setting does not exist , Or the current staff has changed landing .");

            //  Determine the current node read receipts .
            Node nd = new Node(nodeID);
            if (nd.ReadReceipts == ReadReceipts.None)
                return;

            bool isSend = false;
            if (nd.ReadReceipts == ReadReceipts.Auto)
                isSend = true;

            if (nd.ReadReceipts == ReadReceipts.BySysField)
            {
                /* Get on a node ID*/
                Nodes fromNodes = nd.FromNodes;
                int fromNodeID = 0;
                foreach (Node item in fromNodes)
                {
                    ps = new Paras();
                    ps.SQL = "SELECT FK_Node FROM WF_GenerWorkerlist  WHERE WorkID=" + dbstr + "WorkID AND FK_Node=" + dbstr + "FK_Node ";
                    ps.Add("WorkID", workid);
                    ps.Add("FK_Node", item.NodeID);
                    DataTable dt = DBAccess.RunSQLReturnTable(ps);
                    if (dt.Rows.Count == 0)
                        continue;
                    fromNodeID = item.NodeID;
                    break;
                }
                if (fromNodeID == 0)
                    throw new Exception("@ Did not find it on the further work .");

                try
                {
                    ps = new Paras();
                    ps.SQL = "SELECT " + BP.WF.WorkSysFieldAttr.SysIsReadReceipts + " FROM ND" + fromNodeID + "    WHERE OID=" + dbstr + "OID";
                    ps.Add("OID", workid);
                    DataTable dt1 = DBAccess.RunSQLReturnTable(ps);
                    if (dt1.Rows[0][0].ToString() == "1")
                        isSend = true;
                }
                catch (Exception ex)
                {
                    throw new Exception("@ Process design errors :" + ex.Message + "  In the current node last step you set up the placement of receipt to decide whether form fields , But on one node there is no agreement in the form of field .");
                }
            }

            if (nd.ReadReceipts == ReadReceipts.BySDKPara)
            {
                /* If the argument is based developer */

                /* Get on a node ID*/
                Nodes fromNodes = nd.FromNodes;
                int fromNodeID = 0;
                foreach (Node item in fromNodes)
                {
                    ps = new Paras();
                    ps.SQL = "SELECT FK_Node FROM WF_GenerWorkerlist  WHERE WorkID=" + dbstr + "WorkID AND FK_Node=" + dbstr + "FK_Node ";
                    ps.Add("WorkID", workid);
                    ps.Add("FK_Node", item.NodeID);
                    DataTable dt = DBAccess.RunSQLReturnTable(ps);
                    if (dt.Rows.Count == 0)
                        continue;

                    fromNodeID = item.NodeID;
                    break;
                }
                if (fromNodeID == 0)
                    throw new Exception("@ Did not find it on the further work .");

                string paras = BP.WF.Dev2Interface.GetFlowParas(fromNodeID, workid);
                if (string.IsNullOrEmpty(paras) || paras.Contains("@" + BP.WF.WorkSysFieldAttr.SysIsReadReceipts + "=") == false)
                    throw new Exception("@ Process design errors : In the current node on one of your parameters set by the developer to decide whether receipt , But did not find the arguments .");

                //  Developers parameters .
                if (paras.Contains("@" + BP.WF.WorkSysFieldAttr.SysIsReadReceipts + "=1") == true)
                    isSend = true;
            }


            if (isSend == true)
            {
                /* If it is an automatic read receipt , Let it be sent to the current node of a sender .*/

                //  Get Process title .
                ps = new Paras();
                ps.SQL = "SELECT Title FROM WF_GenerWorkFlow WHERE WorkID=" + dbstr + "WorkID ";
                ps.Add("WorkID", workid);
                DataTable dt = DBAccess.RunSQLReturnTable(ps);
                string title = dt.Rows[0][0].ToString();

                //  Get the sender process .
                ps = new Paras();
                ps.SQL = "SELECT " + GenerWorkerListAttr.Sender + " FROM WF_GenerWorkerlist WHERE WorkID=" + dbstr + "WorkID AND FK_Node=" + dbstr + "FK_Node ";
                ps.Add("WorkID", workid);
                ps.Add("FK_Node", nodeID);
                dt = DBAccess.RunSQLReturnTable(ps);
                string sender = dt.Rows[0][0].ToString();

                // Send Read Receipts .
                BP.WF.Dev2Interface.Port_SendMsg(sender, " Read Receipts :" + title,
                    " Send your work has been " + WebUser.Name + "在" + DataType.CurrentDataTimeCNOfShort + "  Turn on .",
                    "RP" + workid + "_" + nodeID, BP.WF.SMSMsgType.Self, nd.FK_Flow, nd.NodeID, workid, 0);
            }
        }
        /// <summary>
        ///  Set Working unread 
        /// </summary>
        /// <param name="nodeID"> Node ID</param>
        /// <param name="workid"> The work ID</param>
        /// <param name="userNo"> People want to set </param>
        public static void Node_SetWorkUnRead(int nodeID, Int64 workid, string userNo)
        {
            string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
            Paras ps = new Paras();
            ps.SQL = "UPDATE WF_GenerWorkerList SET IsRead=0 WHERE WorkID=" + dbstr + "WorkID AND FK_Node=" + dbstr + "FK_Node AND FK_Emp=" + dbstr + "FK_Emp";
            ps.Add("WorkID", workid);
            ps.Add("FK_Node", nodeID);
            ps.Add("FK_Emp", userNo);
            DBAccess.RunSQL(ps);
        }
        /// <summary>
        ///  Set Working unread 
        /// </summary>
        /// <param name="nodeID"> Node ID</param>
        /// <param name="workid"> The work ID</param>
        public static void Node_SetWorkUnRead(int nodeID, Int64 workid)
        {
            Node_SetWorkUnRead(nodeID, workid, BP.Web.WebUser.No);
        }
        #endregion  Work on the interface 

        #region  Process attributes and node attributes change interface .
        /// <summary>
        ///  Change the process property 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="attr1"> Field 1</param>
        /// <param name="v1">值1</param>
        /// <param name="attr2"> Field 2( For null)</param>
        /// <param name="v2">值2( For null)</param>
        /// <returns> Execution results </returns>
        public static string ChangeAttr_Flow(string fk_flow, string attr1, object v1, string attr2, object v2)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            Flow fl = new Flow(fk_flow);
            if (attr1 != null)
                fl.SetValByKey(attr1, v1);
            if (attr2 != null)
                fl.SetValByKey(attr2, v2);
            fl.Update();
            return " Modified successfully ";
        }
        #endregion  Process attributes and node attributes change interface .

        #region UI  Interface 
        /// <summary>
        ///  Get the button state 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="workid"> Process ID</param>
        /// <returns> Back button state </returns>
        public static ButtonState UI_GetButtonState(string fk_flow, int fk_node, Int64 workid)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            ButtonState bs = new ButtonState(fk_flow, fk_node, workid);
            return bs;
        }
        /// <summary>
        ///  Open the window to return 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="fk_node"> Current node number </param>
        /// <param name="workid"> The work ID</param>
        /// <param name="fid"> Process ID</param>
        public static void UI_Window_Return(string fk_flow, int fk_node, Int64 workid, Int64 fid)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);
            string url = Glo.CCFlowAppPath + "WF/WorkOpt/ReturnWork.aspx?FK_Flow=" + fk_flow + "&FK_Node=" + fk_node + "&WorkID=" + workid + "&FID=" + fid;
            System.Web.HttpContext.Current.Response.Redirect(url, true);
            return;
        }
        /// <summary>
        ///  Cc window open 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="fk_node"> Current node number </param>
        /// <param name="workid"> The work ID</param>
        /// <param name="fid"> Process ID</param>
        public static void UI_Window_CC(string fk_flow, int fk_node, Int64 workid, Int64 fid)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            PubClass.WinOpen(Glo.CCFlowAppPath + "WF/WorkOpt/CC.aspx?FK_Flow=" + fk_flow + "&FK_Node=" + fk_node + "&WorkID=" + workid + "&FID=" + fid,
                800, 600);
        }
        /// <summary>
        ///  Open the window for endorsement 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="fk_node"> Current node number </param>
        /// <param name="workid"> The work ID</param>
        /// <param name="fid"> Process ID</param>
        public static void UI_Window_AskForHelp(string fk_flow, int fk_node, Int64 workid, Int64 fid)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            string tKey = DateTime.Now.ToString("MMddhhmmss");
            string urlr3 = Glo.CCFlowAppPath + "WF/WorkOpt/Askfor.aspx?FK_Node=" + fk_node + "&FID=" + fid + "&WorkID=" + workid + "&FK_Flow=" + fk_flow + "&s=" + tKey;
            PubClass.WinOpen(urlr3, 800, 600);
        }
        /// <summary>
        ///  Open the window hangs 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="fk_node"> Current node number </param>
        /// <param name="workid"> The work ID</param>
        /// <param name="fid"> Process ID</param>
        public static void UI_Window_HungUp(string fk_flow, int fk_node, Int64 workid, Int64 fid)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            PubClass.WinOpen(Glo.CCFlowAppPath + "WF/WorkOpt/HungUp.aspx?FK_Flow=" + fk_flow + "&FK_Node=" + fk_node + "&WorkID=" + workid + "&FID=" + fid,
                500, 400);
        }
        /// <summary>
        ///  Open reminders window 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="fk_node"> Current node number </param>
        /// <param name="workid"> The work ID</param>
        /// <param name="fid"> Process ID</param>
        public static void UI_Window_Hurry(string fk_flow, int fk_node, Int64 workid, Int64 fid)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            PubClass.WinOpen(Glo.CCFlowAppPath + "WF/Hurry.aspx?FK_Flow=" + fk_flow + "&FK_Node=" + fk_node + "&WorkID=" + workid + "&FID=" + fid,
                500, 400);
        }
        /// <summary>
        ///  Jump window open 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="fk_node"> Current node number </param>
        /// <param name="workid"> The work ID</param>
        /// <param name="fid"> Process ID</param>
        public static void UI_Window_JumpWay(string fk_flow, int fk_node, Int64 workid, Int64 fid)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            PubClass.WinOpen(Glo.CCFlowAppPath + "WF/JumpWaySmallSingle.aspx?FK_Flow=" + fk_flow + "&FK_Node=" + fk_node + "&WorkID=" + workid + "&FID=" + fid,
                500, 400);
        }
        /// <summary>
        ///  Open the process trajectory window 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="nodeID"> Current node number </param>
        /// <param name="workid"> The work ID</param>
        /// <param name="fid"> Process ID</param>
        public static void UI_Window_FlowChartTruck(string fk_flow, int nodeID, Int64 workid, Int64 fid)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);
            PubClass.WinOpen(Glo.CCFlowAppPath + "WF/Chart.aspx?FK_Flow=" + fk_flow + "&WorkID=" + workid + "&FID=" + fid,
                500, 400);
        }
        /// <summary>
        ///  The next step of the recipient 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="fk_node"> Current node number </param>
        /// <param name="workid"> The work ID</param>
        /// <param name="fid"> Process ID</param>
        public static void UI_Window_Accepter(string fk_flow, int fk_node, Int64 workid, Int64 fid)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            PubClass.WinOpen(Glo.CCFlowAppPath + "WF/WorkOpt/Accepter.aspx?FK_Flow=" + fk_flow + "&FK_Node=" + fk_node + "&WorkID=" + workid + "&FID=" + fid,
                500, 400);
        }
        /// <summary>
        ///  Open flowchart window 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="fk_node"> Current node number </param>
        /// <param name="workid"> The work ID</param>
        /// <param name="fid"> Process ID</param>
        public static void UI_Window_FlowChart(string fk_flow)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);
            PubClass.WinOpen(Glo.CCFlowAppPath + "WF/Chart.aspx?FK_Flow=" + fk_flow,
                500, 400);
        }
        /// <summary>
        ///  Turn on OneWork
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="workid"> The work ID</param>
        /// <param name="fid"> Process ID</param>
        public static void UI_Window_OneWork(string fk_flow, Int64 workid, Int64 fid)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);
            PubClass.WinOpen(Glo.CCFlowAppPath + "WF/WorkOpt/OneWork/Track.aspx?FK_Flow=" + fk_flow + "&WorkID=" + workid + "&FID=" + fid,
                500, 400);
        }
        /// <summary>
        ///  View information about the child thread 
        /// </summary>
        /// <param name="fk_flow"></param>
        /// <param name="fk_node"></param>
        /// <param name="workid"></param>
        /// <param name="fid"></param>
        public static void UI_Window_ThreadInfo(string fk_flow, int fk_node, Int64 workid, Int64 fid)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);
            string key = DateTime.Now.ToString("yyyyMMddhhmmss");
            string url = Glo.CCFlowAppPath + "WF/ThreadDtl.aspx?FK_Node=" + fk_node + "&FID=" + fid + "&WorkID=" + workid + "&FK_Flow=" + fk_flow + "&s=" + key;
            PubClass.WinOpen(url, 500, 400);
        }
        #endregion UI  Interface 

        #region ccform  Interface 
        public static void UI_CCForm_XYSpan()
        {

        }
        #endregion ccform  Interface 

        #region  And work related to the processor interface 
        /// <summary>
        ///  Obtaining of a node to turn the 
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="ndFrom"> Node from </param>
        /// <param name="workid"> The work ID</param>
        /// <returns> Returns can reach node </returns>
        public static Nodes WorkOpt_GetToNodes(string flowNo, int ndFrom, Int64 workid, Int64 FID)
        {
            Nodes nds = new Nodes();

            Node nd = new Node(ndFrom);
            Nodes toNDs = nd.HisToNodes;

            Flow fl = nd.HisFlow;
            GERpt rpt = fl.HisGERpt;
            rpt.OID = FID == 0 ? workid : FID;
            rpt.Retrieve();

            // First output common node  
            foreach (Node mynd in toNDs)
            {
                if (mynd.HisRunModel == RunModel.SubThread)
                    continue; // If the node is a child thread .

                #region  Determine the direction of the condition , If you set the direction of the condition , You can determine whether , Not adopted , Do not let it show .
                Cond cond = new Cond();
                int i = cond.Retrieve(CondAttr.FK_Node, nd.NodeID, CondAttr.ToNodeID, mynd.NodeID);
                //  Conditions set direction , To judge it .
                if (i > 0)
                {
                    cond.WorkID = workid;
                    cond.en = rpt;
                    if (cond.IsPassed == false)
                        continue;
                }
                #endregion

                nds.AddEntity(mynd);
            }

            // Form with child thread .
            foreach (Node mynd in toNDs)
            {
                if (mynd.HisRunModel != RunModel.SubThread)
                    continue; // If the node is a child thread .

                if (mynd.HisSubThreadType == SubThreadType.UnSameSheet)
                    continue; // If it is different form the sub-confluent .

                nds.AddEntity(mynd);
            }

            //  Check whether the child has a different thread forms .
            bool isHave = false;
            foreach (Node mynd in toNDs)
            {
                if (mynd.HisSubThreadType == SubThreadType.UnSameSheet)
                    isHave = true;
            }

            if (isHave)
            {
                Node myn1d = new Node();
                myn1d.NodeID = 0;
                myn1d.Name = " You can start node distribution ";
                nds.AddEntity(myn1d);

                /* Increase child thread different forms */
                foreach (Node mynd in toNDs)
                {
                    if (mynd.HisSubThreadType != SubThreadType.UnSameSheet)
                        continue;

                    nds.AddEntity(mynd);
                }
            }
            // Return it .
            return nds;
        }
        /// <summary>
        ///  To the node 
        /// </summary>
        /// <param name="flowNo"></param>
        /// <param name="node"></param>
        /// <param name="workid"></param>
        /// <param name="fid"></param>
        /// <param name="toNodes"></param>
        public static SendReturnObjs WorkOpt_SendToNodes(string flowNo, int nodeID, Int64 workid, Int64 fid, string toNodes)
        {
            // The parameter update to the database inside .
            GenerWorkFlow gwf = new GenerWorkFlow();
            gwf.WorkID = workid;
            gwf.RetrieveFromDBSources();
            gwf.Paras_ToNodes = toNodes;
            gwf.Save();

            Node nd = new Node(nodeID);
            Work wk = nd.HisWork;
            wk.OID = workid;
            wk.Retrieve();

            //  The following code is from  MyFlow.aspx Send  Method copy  Over , The need to maintain the consistency of the business logic , So the code needs to be consistent .
            WorkNode firstwn = new WorkNode(wk, nd);
            string msg = "";
            SendReturnObjs objs = firstwn.NodeSend();
            return objs;
        }
        /// <summary>
        ///  Obtaining recipient data source 
        /// </summary>
        /// <param name="FK_Flow"> Process ID </param>
        /// <param name="ToNode"> Arrives at a node ID</param>
        /// <param name="WorkID"> The work ID</param>
        /// <param name="FID"> Process ID</param>
        /// <returns></returns>
        public static DataSet WorkOpt_AccepterDB(string FK_Flow, int ToNode, Int64 WorkID, Int64 FID)
        {
            DataSet ds = new DataSet();
            Selector MySelector = new Selector(ToNode);
            switch (MySelector.SelectorModel)
            {
                case SelectorModel.Station:
                    DataTable dt = WorkOpt_Accepter_ByStation(ToNode);
                    dt.TableName = "Port_Emp";
                    ds.Tables.Add(dt);
                    // Sector Table 
                    string sql = "SELECT * FROM Port_Dept ";
                    DataTable dt1 = DBAccess.RunSQLReturnTable(sql);
                    dt1.TableName = "Port_Dept";
                    ds.Tables.Add(dt1);
                    break;
                case SelectorModel.SQL:
                    ds = WorkOpt_Accepter_BySQL(ToNode);
                    break;
                case SelectorModel.Dept:
                    ds = WorkOpt_Accepter_ByDept(ToNode);
                    break;
                case SelectorModel.Emp:
                    ds = WorkOpt_Accepter_ByEmp(ToNode);
                    break;
                case SelectorModel.Url:
                default:
                    break;
            }
            return ds;
        }
        /// <summary>
        ///  Get node binding staff positions 
        /// </summary>
        /// <param name="ToNode"></param>
        /// <returns></returns>
        public static DataTable WorkOpt_Accepter_ByStation(int ToNode)
        {
            if (ToNode == 0)
                throw new Exception("@ Process design errors , Node does not turn . Illustration :  Currently A Node . If you are A Attribute points in enabled [ Recipient ] Push button , Then he turned to the node-set ( Is A For example, you can go to the set of nodes :A到B,A到C,  Then B,C Node is turned to a set of nodes ), There must be a node is the node attributes [ Access Rules ] Set [ Select from the previous step to send staff ]");

            NodeStations stas = new NodeStations(ToNode);
            if (stas.Count == 0)
            {
                BP.WF.Node toNd = new BP.WF.Node(ToNode);
                throw new Exception("@ Process design errors : Designers do not design nodes [" + toNd.Name + "], Jobs range recipients .");
            }
            //  Priority to solve the problem in this sector .
            string sql = "";
            if (BP.WF.Glo.OSModel == OSModel.BPM)
            {
                sql = "SELECT A.No,A.Name, A.FK_Dept, B.Name as DeptName FROM Port_Emp A,Port_Dept B WHERE A.FK_Dept=B.No AND a.NO IN ( ";
                sql += "SELECT FK_EMP FROM Port_DeptEmpStation WHERE FK_STATION ";
                sql += "IN (SELECT FK_STATION FROM WF_NodeStation WHERE FK_Node=" + ToNode + ") ";
                sql += ") AND a.No IN (SELECT FK_Emp FROM Port_EmpDept WHERE FK_Dept ='" + WebUser.FK_Dept + "')";
                sql += " ORDER BY FK_DEPT ";
            }
            else
            {
                sql = "SELECT A.No,A.Name, A.FK_Dept, B.Name as DeptName FROM Port_Emp A,Port_Dept B WHERE A.FK_Dept=B.No AND a.NO IN ( ";
                sql += "SELECT FK_EMP FROM Port_EmpSTATION WHERE FK_STATION ";
                sql += "IN (SELECT FK_STATION FROM WF_NodeStation WHERE FK_Node=" + ToNode + ") ";
                sql += ") AND a.No IN (SELECT FK_Emp FROM Port_EmpDept WHERE FK_Dept ='" + WebUser.FK_Dept + "')";
                sql += " ORDER BY FK_DEPT ";
            }
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 0)
                return dt;

            // All staff positions in the organizational structure 
            sql = "SELECT A.No,A.Name, A.FK_Dept, B.Name as DeptName FROM Port_Emp A,Port_Dept B WHERE A.FK_Dept=B.No AND a.NO IN ( ";
            sql += "SELECT FK_EMP FROM Port_EmpSTATION WHERE FK_STATION ";
            sql += "IN (SELECT FK_STATION FROM WF_NodeStation WHERE FK_Node=" + ToNode + ") ";
            sql += ") ORDER BY FK_DEPT ";
            return BP.DA.DBAccess.RunSQLReturnTable(sql);
        }

        /// <summary>
        /// 按sql The way 
        /// </summary>
        public static DataSet WorkOpt_Accepter_BySQL(int ToNode)
        {
            DataSet ds = new DataSet();
            Selector MySelector = new Selector(ToNode);
            string sqlGroup = MySelector.SelectorP1;
            sqlGroup = sqlGroup.Replace("@WebUser.No", WebUser.No);
            sqlGroup = sqlGroup.Replace("@WebUser.Name", WebUser.Name);
            sqlGroup = sqlGroup.Replace("@WebUser.FK_Dept", WebUser.FK_Dept);

            string sqlDB = MySelector.SelectorP2;
            sqlDB = sqlDB.Replace("@WebUser.No", WebUser.No);
            sqlDB = sqlDB.Replace("@WebUser.Name", WebUser.Name);
            sqlDB = sqlDB.Replace("@WebUser.FK_Dept", WebUser.FK_Dept);

            DataTable dtGroup = DBAccess.RunSQLReturnTable(sqlGroup);
            dtGroup.TableName = "Port_Dept";
            ds.Tables.Add(dtGroup);
            DataTable dtDB = DBAccess.RunSQLReturnTable(sqlDB);
            dtDB.TableName = "Port_Emp";
            ds.Tables.Add(dtDB);

            return ds;
        }

        /// <summary>
        ///  Get recipient selector , Bound by sector 
        /// </summary>
        /// <param name="ToNode"></param>
        /// <returns></returns>
        public static DataSet WorkOpt_Accepter_ByDept(int ToNode)
        {
            DataSet ds = new DataSet();
            string sqlGroup = "SELECT No,Name FROM Port_Dept WHERE No IN (SELECT FK_Dept FROM WF_NodeDept WHERE FK_Node='" + ToNode + "')";
            string sqlDB = "SELECT No,Name, FK_Dept FROM Port_Emp WHERE FK_Dept IN (SELECT FK_Dept FROM WF_NodeDept WHERE FK_Node='" + ToNode + "')";

            DataTable dtGroup = DBAccess.RunSQLReturnTable(sqlGroup);
            dtGroup.TableName = "Port_Dept";
            ds.Tables.Add(dtGroup);

            DataTable dtDB = DBAccess.RunSQLReturnTable(sqlDB);
            dtDB.TableName = "Port_Emp";
            ds.Tables.Add(dtDB);

            return ds;
        }

        /// <summary>
        /// 按BindByEmp  The way 
        /// </summary>
        public static DataSet WorkOpt_Accepter_ByEmp(int ToNode)
        {
            string sqlGroup = "SELECT No,Name FROM Port_Dept WHERE No IN (SELECT FK_Dept FROM Port_Emp WHERE No in(SELECT FK_EMP FROM WF_NodeEmp WHERE FK_Node='" + ToNode + "'))";
            string sqlDB = "SELECT No,Name,FK_Dept FROM Port_Emp WHERE No in (SELECT FK_EMP FROM WF_NodeEmp WHERE FK_Node='" + ToNode + "')";

            DataSet ds = new DataSet();
            DataTable dtGroup = DBAccess.RunSQLReturnTable(sqlGroup);
            dtGroup.TableName = "Port_Dept";
            ds.Tables.Add(dtGroup);
            DataTable dtDB = DBAccess.RunSQLReturnTable(sqlDB);
            dtDB.TableName = "Port_Emp";
            ds.Tables.Add(dtDB);

            return ds;
        }

        /// <summary>
        ///  Sets the specified node accepts people 
        /// </summary>
        /// <param name="nodeID"> Node ID</param>
        /// <param name="workid"> The work ID</param>
        /// <param name="fid"> Process ID</param>
        /// <param name="emps"> Persons designated collection zhangsan,lisi,wangwu</param>
        /// <param name="isNextTime"> Whether the next automatic settings </param>
        public static void WorkOpt_SetAccepter(int toNode, Int64 workid, Int64 fid, string emps, bool isNextTime)
        {
            SelectAccpers ens = new SelectAccpers();
            ens.Delete(SelectAccperAttr.FK_Node, toNode,
                SelectAccperAttr.WorkID, workid);

            // Whether the next memory selection , Empty out .
            string sql = "UPDATE WF_SelectAccper SET " + SelectAccperAttr.IsRemember + " = 0 WHERE Rec='" + WebUser.No + "' AND IsRemember=1 AND FK_Node=" + toNode;
            BP.DA.DBAccess.RunSQL(sql);

            // Started saving .
            string[] strs = emps.Split(',');
            foreach (string str in strs)
            {
                if (str == null || str == "")
                    continue;

                SelectAccper en = new SelectAccper();
                en.MyPK = toNode + "_" + workid + "_" + str;
                en.FK_Emp = str;
                en.FK_Node = toNode;
                en.WorkID = workid;
                en.Rec = WebUser.No;
                en.IsRemember = isNextTime;
                en.Insert();
            }
        }
        /// <summary>
        ///  To the node 
        /// </summary>
        /// <param name="flowNo"></param>
        /// <param name="node"></param>
        /// <param name="workid"></param>
        /// <param name="fid"></param>
        /// <param name="toNodes"></param>
        public static SendReturnObjs WorkOpt_SendToEmps(string flowNo, int nodeID, Int64 workid, Int64 fid,
            int toNodeID, string toEmps, bool isRememberMe)
        {
            WorkOpt_SetAccepter(toNodeID, workid, fid, toEmps, isRememberMe);

            Node nd = new Node(nodeID);
            Work wk = nd.HisWork;
            wk.OID = workid;
            wk.Retrieve();

            //  The following code is from  MyFlow.aspx Send  Method copy  Over , The need to maintain the consistency of the business logic , So the code needs to be consistent .
            WorkNode firstwn = new WorkNode(wk, nd);
            string msg = "";
            SendReturnObjs objs = firstwn.NodeSend();
            return objs;
        }
        #endregion

        #region  Upload attachment 
        public static string SaveImageAsFile(byte[] img, string pkval, string fk_Frm_Ele)
        {
            FrmEle fe = new FrmEle(fk_Frm_Ele);
            System.Drawing.Image newImage;
            using (MemoryStream ms = new MemoryStream(img, 0, img.Length))
            {
                ms.Write(img, 0, img.Length);
                newImage = Image.FromStream(ms, true);
                Bitmap bitmap = new Bitmap(newImage, new Size(fe.WOfInt, fe.HOfInt));

                if (System.IO.Directory.Exists(fe.HandSigantureSavePath + "\\" + fe.FK_MapData + "\\") == false)
                    System.IO.Directory.CreateDirectory(fe.HandSigantureSavePath + "\\" + fe.FK_MapData + "\\");

                string saveTo = fe.HandSigantureSavePath + "\\" + fe.FK_MapData + "\\" + pkval + ".jpg";
                bitmap.Save(saveTo, ImageFormat.Jpeg);

                string pathFile = BP.Sys.Glo.Request.ApplicationPath + fe.HandSiganture_UrlPath + fe.FK_MapData + "/" + pkval + ".jpg";
                FrmEleDB ele = new FrmEleDB();
                ele.FK_MapData = fe.FK_MapData;
                ele.EleID = fe.EleID;
                ele.RefPKVal = pkval;
                ele.Tag1 = pathFile.Replace("\\\\", "\\");
                ele.Tag1 = pathFile.Replace("////", "//");

                ele.Tag2 = saveTo.Replace("\\\\", "\\");
                ele.Tag2 = saveTo.Replace("////", "//");

                ele.GenerPKVal();
                ele.Save();

                return pathFile;
            }
        }

        /// <summary>
        ///  Upload file .
        /// </summary>
        /// <param name="FileByte"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string UploadFile(byte[] FileByte, String fileName)
        {
            string path = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\DataUser\\UploadFile";
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
            string filePath = path + "\\" + fileName;
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            // As used herein, the absolute path to index 
            FileStream stream = new FileStream(filePath, FileMode.CreateNew);
            stream.Write(FileByte, 0, FileByte.Length);
            stream.Close();

            return filePath;
        }

        #endregion
    }

}
