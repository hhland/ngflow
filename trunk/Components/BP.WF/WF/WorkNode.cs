using System;
using BP.En;
using BP.DA;
using System.Collections;
using System.Data;
using BP.Port;
using BP.Web;
using BP.Sys;
using BP.WF.Template;
using BP.WF.Data;

namespace BP.WF
{
    using System.Threading;

    /// <summary>
    /// WF  The summary .
    ///  Workflow . 
    ///  This contains two aspects   
    ///  Information Work ．
    ///  Process Information .
    /// </summary>
    public class WorkNode
    {
        public string mailUrlsPattern = "";

        #region  Authority to judge 
        /// <summary>
        ///  Judge a person can not operate on this work node .
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        private bool IsCanOpenCurrentWorkNode(string empId)
        {
            WFState stat = this.HisGenerWorkFlow.WFState;
            if (stat == WFState.Runing)
            {
                if (this.HisNode.IsStartNode)
                {
                    /* If the node is the start of work , Judge he had no permission to work from job .*/
                    return WorkFlow.IsCanDoWorkCheckByEmpStation(this.HisNode.NodeID, empId);
                }
                else
                {
                    /*  If the initialization phase , Judge his initialize node  */
                    GenerWorkerList wl = new GenerWorkerList();
                    wl.WorkID = this.HisWork.OID;
                    wl.FK_Emp = empId;

                    Emp myEmp = new Emp(empId);
                    wl.FK_EmpText = myEmp.Name;

                    wl.FK_Node = this.HisNode.NodeID;
                    wl.FK_NodeText = this.HisNode.Name;
                    return wl.IsExits;
                }
            }
            else
            {
                /*  If the initialization phase  */
                return false;
            }
        }
        #endregion

        #region  Property / Variable .
        private string _execer = null;
        /// <summary>
        ///  Actual executor , Send enforcing , Sometimes the current  WebUser.No  Not the actual executor .
        /// </summary>
        public string Execer
        {
            get
            {
                if (_execer == null)
                    _execer = WebUser.No;
                return _execer;
            }
            set
            {
                _execer = value;
            }
        }
        private string _execerName = null;
        /// <summary>
        ///  The actual name of the executor ( Please refer to the actual executor )
        /// </summary>
        public string ExecerName
        {
            get
            {
                if (_execerName == null)
                    _execerName = WebUser.Name;
                return _execerName;
            }
            set
            {
                _execerName = value;
            }
        }
        private string _execerDeptName = null;
        /// <summary>
        ///  The actual name of the executor ( Please refer to the actual executor )
        /// </summary>
        public string ExecerDeptName
        {
            get
            {
                if (_execerDeptName == null)
                    _execerDeptName = WebUser.FK_DeptName;
                return _execerDeptName;
            }
            set
            {
                _execerDeptName = value;
            }
        }
        private string _execerDeptNo = null;
        /// <summary>
        ///  The actual name of the executor ( Please refer to the actual executor )
        /// </summary>
        public string ExecerDeptNo
        {
            get
            {
                if (_execerDeptNo == null)
                    _execerDeptNo = WebUser.FK_Dept;
                return _execerDeptNo;
            }
            set
            {
                _execerDeptNo = value;
            }
        }
        /// <summary>
        ///  Virtual directory path 
        /// </summary>
        private string _VirPath = null;
        /// <summary>
        ///  Virtual directory path  
        /// </summary>
        public string VirPath
        {
            get
            {
                if (_VirPath == null && BP.Sys.SystemConfig.IsBSsystem)
                    _VirPath = Glo.CCFlowAppPath;//BP.Sys.Glo.Request.ApplicationPath;
                return _VirPath;
            }
        }
        private string _AppType = null;
        /// <summary>
        ///  Virtual directory path 
        /// </summary>
        public string AppType
        {
            get
            {
                if (BP.Sys.SystemConfig.IsBSsystem == false)
                {
                    return "CCFlow";
                }

                if (_AppType == null && BP.Sys.SystemConfig.IsBSsystem)
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
                return _AppType;
            }
        }
        private string nextStationName = "";
        public WorkNode town = null;
        private bool IsFindWorker = false;
        public bool IsSubFlowWorkNode
        {
            get
            {
                if (this.HisWork.FID == 0)
                    return false;
                else
                    return true;
            }
        }
        #endregion  Property / Variable .

        #region GenerWorkerList  Related Methods .
        // Check out the list of recipients for each node set （Emps）.
        public string GenerEmps(Node nd)
        {
            string str = "";
            foreach (GenerWorkerList wl in this.HisWorkerLists)
                str = wl.FK_Emp + ",";
            return str;
        }
        /// <summary>
        ///  Produce its workers .
        /// </summary>
        /// <param name="town"></param>
        /// <returns></returns>
        public GenerWorkerLists Func_GenerWorkerLists(WorkNode town)
        {
            this.town = town;
            //if (this.HisGenerWorkFlow.WFState == WFState.ReturnSta )
            //{
            //    /*  If it is returned to the state .*/
            //    if (this.town.HisNode.TodolistModel == TodolistModel.Order || this.town.HisNode.TodolistModel == TodolistModel.Teamup)
            //    {


            //        return ens;
            //    }
            //}

            DataTable dt = new DataTable();
            dt.Columns.Add("No", typeof(string));
            string sql;
            string FK_Emp;




            //  If you specify a particular staff to handle .
            if (string.IsNullOrEmpty(JumpToEmp) == false)
            {
                string[] emps = JumpToEmp.Split(',');
                foreach (string emp in emps)
                {
                    if (string.IsNullOrEmpty(emp))
                        continue;
                    DataRow dr = dt.NewRow();
                    dr[0] = emp;
                    dt.Rows.Add(dr);
                }


                /* If you are robbed or shared office .*/

                //  If you do send twice , Previous trajectory that would need to be removed , Here is to avoid errors .
                ps = new Paras();
                ps.Add("WorkID", this.HisWork.OID);
                ps.Add("FK_Node", town.HisNode.NodeID);
                ps.SQL = "DELETE FROM WF_GenerWorkerlist WHERE WorkID=" + dbStr + "WorkID AND FK_Node =" + dbStr + "FK_Node";
                DBAccess.RunSQL(ps);

                return WorkerListWayOfDept(town, dt);
            }

            //  If you do send twice , Previous trajectory that would need to be removed , Here is to avoid errors .
            ps = new Paras();
            ps.Add("WorkID", this.HisWork.OID);
            ps.Add("FK_Node", town.HisNode.NodeID);
            ps.SQL = "DELETE FROM WF_GenerWorkerlist WHERE WorkID=" + dbStr + "WorkID AND FK_Node =" + dbStr + "FK_Node";
            DBAccess.RunSQL(ps);



            if (this.town.HisNode.HisDeliveryWay == DeliveryWay.ByCCFlowBPM || 1 == 1)
            {
                /* If you set up security ccflow的BPM Mode */
                while (true)
                {
                    FindWorker fw = new FindWorker();
                    dt = fw.DoIt(this.HisFlow, this, town);
                    if (dt == null)
                    {
                        // if (this.town.HisNode.IsRememberMe
                        throw new Exception("@ Did not find the recipient .");
                    }
                    return WorkerListWayOfDept(town, dt);
                }
            }
            throw new Exception("@ This part of the code has been removed .");
        }
        /// <summary>
        ///  According to department get the next operator 
        /// </summary>
        /// <param name="deptNo"></param>
        /// <param name="emp1"></param>
        /// <returns></returns>
        public GenerWorkerLists Func_GenerWorkerList_DiGui(string deptNo, string empNo)
        {
            string sql = "SELECT NO FROM Port_Emp WHERE No IN "
                + "(SELECT  FK_Emp  FROM Port_EmpStation WHERE FK_Station IN (SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + dbStr + "FK_Node ) )"
                + " AND  NO IN "
                + "(SELECT  FK_Emp  FROM Port_EmpDept WHERE FK_Dept=" + dbStr + "FK_Dept )"
                + " AND No!=" + dbStr + "FK_Emp";

            ps = new Paras();
            ps.SQL = sql;
            ps.Add("FK_Node", town.HisNode.NodeID);
            ps.Add("FK_Emp", empNo);
            ps.Add("FK_Dept", deptNo);

            DataTable dt = DBAccess.RunSQLReturnTable(ps);
            if (dt.Rows.Count == 0)
            {
                NodeStations nextStations = town.HisNode.NodeStations;
                if (nextStations.Count == 0)
                    throw new Exception("@ Node has no job :" + town.HisNode.NodeID + "  " + town.HisNode.Name);

                sql = "SELECT No FROM Port_Emp WHERE No IN ";
                sql += "(SELECT  FK_Emp  FROM Port_EmpStation WHERE FK_Station IN (SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + dbStr + "FK_Node ) )";
                sql += " AND No IN ";

                if (deptNo == "1")
                    sql += "(SELECT FK_Emp FROM Port_EmpDept WHERE FK_Emp!=" + dbStr + "FK_Emp ) ";
                else
                {
                    Dept deptP = new Dept(deptNo);
                    sql += "(SELECT FK_Emp FROM Port_EmpDept WHERE FK_Emp!=" + dbStr + "FK_Emp AND FK_Dept = '" + deptP.ParentNo + "')";
                }

                ps = new Paras();
                ps.SQL = sql;
                ps.Add("FK_Node", town.HisNode.NodeID);
                ps.Add("FK_Emp", empNo);

                dt = DBAccess.RunSQLReturnTable(ps);
                if (dt.Rows.Count == 0)
                {
                    sql = "SELECT No FROM Port_Emp WHERE No!=" + dbStr + "FK_Emp AND No IN ";
                    sql += "(SELECT  FK_Emp  FROM Port_EmpStation WHERE FK_Station IN (SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + dbStr + "FK_Node ) )";
                    ps = new Paras();
                    ps.SQL = sql;
                    ps.Add("FK_Emp", empNo);
                    ps.Add("FK_Node", town.HisNode.NodeID);
                    dt = DBAccess.RunSQLReturnTable(ps);
                    if (dt.Rows.Count == 0)
                        throw new Exception("@ Post (" + town.HisNode.HisStationsStr + ") Without staff , The corresponding node :" + town.HisNode.Name);
                }
                return WorkerListWayOfDept(town, dt);
            }
            else
            {
                return WorkerListWayOfDept(town, dt);
            }
            return null;
        }
        #endregion GenerWorkerList  Related Methods .

        /// <summary>
        ///  Generate a word
        /// </summary>
        public void DoPrint()
        {
            string tempFile = SystemConfig.PathOfTemp + "\\" + this.WorkID + ".doc";
            Work wk = this.HisNode.HisWork;
            wk.OID = this.WorkID;
            wk.Retrieve();
            Glo.GenerWord(tempFile, wk);
            PubClass.OpenWordDocV2(tempFile, this.HisNode.Name + ".doc");
        }
        string dbStr = SystemConfig.AppCenterDBVarStr;
        public Paras ps = new Paras();
        /// <summary>
        ///  Recursive delete of data between two nodes 
        /// </summary>
        /// <param name="nds"> Set of nodes reachable </param>
        public void DeleteToNodesData(Nodes nds)
        {
            if (this.HisFlow.HisDataStoreModel == DataStoreModel.SpecTable)
                return;

            /* Began to traverse to reach the set of nodes */
            foreach (Node nd in nds)
            {
                Work wk = nd.HisWork;
                if (wk.EnMap.PhysicsTable == this.HisFlow.PTable)
                    continue;

                wk.OID = this.WorkID;
                if (wk.Delete() == 0)
                {
                    wk.FID = this.WorkID;
                    if (wk.EnMap.PhysicsTable == this.HisFlow.PTable)
                        continue;

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

        #region  According to generate worker jobs 
        private Node _ndFrom = null;
        private Node ndFrom
        {
            get
            {
                if (_ndFrom == null)
                    _ndFrom = this.HisNode;
                return _ndFrom;
            }
            set
            {
                _ndFrom = value;
            }
        }

        private GenerWorkerLists WorkerListWayOfDept(WorkNode town, DataTable dt)
        {
            return WorkerListWayOfDept(town, dt, 0);
        }
        private GenerWorkerLists WorkerListWayOfDept(WorkNode town, DataTable dt, Int64 fid)
        {

            if (dt.Rows.Count == 0)
                throw new Exception(" Accept staff list is empty "); //  Accept staff list is empty 




            this.HisGenerWorkFlow.TodoEmpsNum = -1;

            #region  Determine the type of transmission , Process-related FID.
            //  Define a node to accept people  FID 与 WorkID.
            Int64 nextUsersWorkID = this.WorkID;
            Int64 nextUsersFID = this.HisWork.FID;

            //  Whether it is split into sub-thread .
            bool isFenLiuToSubThread = false;
            if (this.HisNode.IsFLHL == true
                && town.HisNode.HisRunModel == RunModel.SubThread)
            {
                isFenLiuToSubThread = true;
                nextUsersWorkID = 0;
                nextUsersFID = this.HisWork.OID;
            }


            //  Child thread  到  Confluence or  Confluence points .
            bool isSubThreadToFenLiu = false;
            if (this.HisNode.HisRunModel == RunModel.SubThread
                && town.HisNode.IsFLHL == true)
            {
                nextUsersWorkID = this.HisWork.FID;
                nextUsersFID = 0;
                isSubThreadToFenLiu = true;
            }

            //  Child thread to child thread 
            bool isSubthread2Subthread = false;
            if (this.HisNode.HisRunModel == RunModel.SubThread && town.HisNode.HisRunModel == RunModel.SubThread)
            {
                nextUsersWorkID = this.HisWork.OID;
                nextUsersFID = this.HisWork.FID;
                isSubthread2Subthread = true;
            }
            #endregion  Determine the type of transmission , Process-related FID.

            int toNodeId = town.HisNode.NodeID;
            this.HisWorkerLists = new GenerWorkerLists();
            this.HisWorkerLists.Clear();

            #region  Deadline time   town.HisNode.DeductDays-1
            // 2008-01-22  Before things .
            //int i = town.HisNode.DeductDays;
            //dtOfShould = DataType.AddDays(dtOfShould, i);
            //if (town.HisNode.WarningDays > 0)
            //    dtOfWarning = DataType.AddDays(dtOfWarning, i - town.HisNode.WarningDays);
            // edit at 2008-01-22 ,  Deal with the problem of early warning date .

            DateTime dtOfShould;
            if (this.HisFlow.HisTimelineRole == TimelineRole.ByFlow)
            {
                /* If the whole process is calculated according to the process set .*/
                dtOfShould = DataType.ParseSysDateTime2DateTime(this.HisGenerWorkFlow.SDTOfFlow);
            }
            else
            {
                int day = 0;
                int hh = 0;
                if (town.HisNode.DeductDays < 1)
                    day = 0;
                else
                    day = int.Parse(town.HisNode.DeductDays.ToString());

                dtOfShould = DataType.AddDays(DateTime.Now, day);
            }

            DateTime dtOfWarning = DateTime.Now;
            if (town.HisNode.WarningDays > 0)
                dtOfWarning = DataType.AddDays(dtOfShould, -int.Parse(town.HisNode.WarningDays.ToString())); // dtOfShould.AddDays(-town.HisNode.WarningDays);

            switch (this.HisNode.HisNodeWorkType)
            {
                case NodeWorkType.StartWorkFL:
                case NodeWorkType.WorkFHL:
                case NodeWorkType.WorkFL:
                case NodeWorkType.WorkHL:
                    break;
                default:
                    this.HisGenerWorkFlow.FK_Node = town.HisNode.NodeID;
                    this.HisGenerWorkFlow.SDTOfNode = dtOfShould.ToString("yyyy-MM-dd HH:mm:ss");
                    this.HisGenerWorkFlow.TodoEmpsNum = dt.Rows.Count;
                    break;
            }
            #endregion  Deadline time   town.HisNode.DeductDays-1

            #region  Deal with   Staff list   Data Sources .
            if (dt.Rows.Count == 1)
            {
                /*  If only one person  */
                GenerWorkerList wl = new GenerWorkerList();
                if (isFenLiuToSubThread)
                {
                    /*   Description This is a diversion point sent down 
                     *   Here produce temporary workid.
                     */
                    wl.WorkID = DBAccess.GenerOIDByGUID();
                }
                else
                {
                    wl.WorkID = nextUsersWorkID;
                }


                wl.FID = nextUsersFID;
                wl.FK_Node = toNodeId;
                wl.FK_NodeText = town.HisNode.Name;

                wl.FK_Emp = dt.Rows[0][0].ToString();

                Emp emp = new Emp(wl.FK_Emp);
                wl.FK_EmpText = emp.Name;
                wl.FK_Dept = emp.FK_Dept;
                wl.WarningDays = town.HisNode.WarningDays;
                wl.SDT = dtOfShould.ToString(DataType.SysDataTimeFormat);

                wl.DTOfWarning = dtOfWarning.ToString(DataType.SysDataTimeFormat);
                wl.RDT = DateTime.Now.ToString(DataType.SysDataTimeFormat);
                wl.FK_Flow = town.HisNode.FK_Flow;
                wl.Sender = this.Execer;

                wl.DirectInsert();
                this.HisWorkerLists.AddEntity(wl);

                RememberMe rm = new RememberMe(); // this.GetHisRememberMe(town.HisNode);
                rm.Objs = "@" + wl.FK_Emp + "@";

                rm.ObjsExt += BP.WF.Glo.DealUserInfoShowModel(wl.FK_Emp, wl.FK_EmpText);

                rm.Emps = "@" + wl.FK_Emp + "@";

                rm.EmpsExt = BP.WF.Glo.DealUserInfoShowModel(wl.FK_Emp, wl.FK_EmpText);
                this.HisRememberMe = rm;
            }
            else
            {
                /*  If there is more than one person , We must consider whether to accept the problem of memory attributes . */
                RememberMe rm = this.GetHisRememberMe(town.HisNode);

                #region  Whether you need to clear the memory properties .
                //  If the process according to the selected staff , It set its memory is empty .2011-11-06  Plant processing requirements  .
                if (this.town.HisNode.HisDeliveryWay == DeliveryWay.BySelected
                    || this.town.HisNode.IsAllowRepeatEmps == true  /* Allow staff to accept repeated */
                    || town.HisNode.IsRememberMe == false)
                {
                    if (rm != null)
                        rm.Objs = "";
                }

                if (this.HisNode.IsFL)
                {
                    if (rm != null)
                        rm.Objs = "";
                }

                if (rm != null && rm.Objs != "")
                {
                    /* Check the recipient list if changes , If you change the , To take effective recipient Empty , Allowed to regenerate .*/

                    string emps = "@";
                    foreach (DataRow dr in dt.Rows)
                    {
                        emps += dr[0].ToString() + "@";
                    }

                    if (rm.Emps != emps)
                    {
                        //  List changes .
                        rm.Emps = emps;
                        rm.Objs = ""; // Empty the effective collection of recipient .
                    }
                }
                #endregion  Whether you need to clear the memory properties .


                string myemps = "";
                Emp emp = null;
                foreach (DataRow dr in dt.Rows)
                {

                    string fk_emp = dr[0].ToString();

                    //  Duplicate personnel , Otherwise it will lead to generworkerlist的pk Error .
                    if (myemps.IndexOf("@" + dr[0].ToString() + ",") != -1)
                        continue;
                    myemps += "@" + dr[0].ToString() + ",";


                    //if (town.HisNode.HisRunModel == RunModel.SubThread
                    //    && town.HisNode.IsAllowRepeatEmps == true && this.HisNode.IsFLHL)
                    //{
                    //    /* If the child thread , And allow the child thread personnel repeat ,  Do not judge people repeat the question . */
                    //}
                    //else
                    //{
                    //}
                    // 

                    GenerWorkerList wl = new GenerWorkerList();

                    #region  The operator can set whether or not based on memory .
                    if (rm != null)
                    {
                        if (rm.Objs == "")
                        {
                            /* If it is empty .*/
                            wl.IsEnable = true;
                        }
                        else
                        {
                            if (rm.Objs.Contains("@" + fk_emp + "@") == true)
                                wl.IsEnable = true; /*  If you include , It shows that he already had */
                            else
                                wl.IsEnable = false;
                        }
                    }
                    else
                    {
                        wl.IsEnable = false;
                    }
                    #endregion  The operator can set whether or not based on memory .


                    wl.FK_Node = toNodeId;
                    wl.FK_NodeText = town.HisNode.Name;
                    wl.FK_Emp = dr[0].ToString();
                    try
                    {
                        emp = new Emp(wl.FK_Emp);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("@ Error occurs when personnel assigned to work for :" + wl.FK_Emp + ", Did not execute successfully , Exception Information ." + ex.Message);
                    }

                    wl.FK_EmpText = emp.Name;
                    wl.FK_Dept = emp.FK_Dept;
                    wl.WarningDays = town.HisNode.WarningDays;
                    wl.SDT = dtOfShould.ToString(DataType.SysDataTimeFormat);
                    wl.DTOfWarning = dtOfWarning.ToString(DataType.SysDataTimeFormat);
                    wl.RDT = DateTime.Now.ToString(DataType.SysDataTimeFormat);
                    wl.FK_Flow = town.HisNode.FK_Flow;
                    wl.Sender = this.Execer;

                    wl.FID = nextUsersFID;
                    if (isFenLiuToSubThread)
                    {
                        /*  Description This is a diversion point sent down 
                         *   Here produce temporary workid.
                         */
                        wl.WorkID = DBAccess.GenerOIDByGUID();
                    }
                    else
                    {
                        wl.WorkID = nextUsersWorkID;
                    }

                    try
                    {
                        wl.DirectInsert();
                        this.HisWorkerLists.AddEntity(wl);
                    }
                    catch (Exception ex)
                    {
                        Log.DefaultLogWriteLineError(" Exception information should not appear in the :" + ex.Message);
                    }
                }

                // Perform rm Updates .
                if (rm != null)
                {
                    string empExts = "";
                    string objs = "@"; //  Effective staff .
                    string objsExt = "@"; //  Effective staff .
                    foreach (GenerWorkerList wl in this.HisWorkerLists)
                    {
                        if (wl.IsEnable == false)
                            empExts += "<strike><font color=red>" + BP.WF.Glo.DealUserInfoShowModel(wl.FK_Emp, wl.FK_EmpText) + "</font></strike>,";
                        else
                            empExts += BP.WF.Glo.DealUserInfoShowModel(wl.FK_Emp, wl.FK_EmpText);

                        if (wl.IsEnable == true)
                        {
                            objs += wl.FK_Emp + "@";
                            objsExt += BP.WF.Glo.DealUserInfoShowModel(wl.FK_Emp, wl.FK_EmpText);
                        }
                    }
                    rm.EmpsExt = empExts;

                    rm.Objs = objs;
                    rm.ObjsExt = objsExt;
                    //  rm.Save(); // Save .
                    this.HisRememberMe = rm;
                }
            }

            if (this.HisWorkerLists.Count == 0)
                throw new Exception("@ According to [" + this.town.HisNode.HisRunModel + "] Department staff produced an error , Process [" + this.HisWorkFlow.HisFlow.Name + "], Nodes [" + town.HisNode.Name + "] Custom Error , Did not find the staff to accept this job .");
            #endregion  Deal with   Staff list   Data Sources .

            #region  Set the number of processes , Other information provided data for the task pool .
            string hisEmps = "";
            int num = 0;
            foreach (GenerWorkerList wl in this.HisWorkerLists)
            {
                if (wl.IsPassInt == 0 && wl.IsEnable == true)
                {
                    num++;
                    hisEmps += wl.FK_Emp + "," + wl.FK_EmpText + ";";
                }
            }
            if (num == 0)
                throw new Exception("@ The results should not generate an error ");
            this.HisGenerWorkFlow.TodoEmpsNum = num;
            this.HisGenerWorkFlow.TodoEmps = hisEmps;
            #endregion

            #region   Determine the type of log , And add a variable .
            ActionType at = ActionType.Forward;
            switch (town.HisNode.HisNodeWorkType)
            {
                case NodeWorkType.StartWork:
                case NodeWorkType.StartWorkFL:
                    at = ActionType.Start;
                    break;
                case NodeWorkType.Work:
                    if (this.HisNode.HisNodeWorkType == NodeWorkType.WorkFL
                        || this.HisNode.HisNodeWorkType == NodeWorkType.WorkFHL)
                        at = ActionType.ForwardFL;
                    else
                        at = ActionType.Forward;
                    break;
                case NodeWorkType.WorkHL:
                    at = ActionType.ForwardHL;
                    break;
                case NodeWorkType.SubThreadWork:
                    at = ActionType.SubFlowForward;
                    break;
                default:
                    break;
            }
            if (this.HisWorkerLists.Count == 1)
            {
                GenerWorkerList wl = this.HisWorkerLists[0] as GenerWorkerList;
                this.AddToTrack(at, wl.FK_Emp, wl.FK_EmpText, wl.FK_Node, wl.FK_NodeText, null, this.ndFrom);
            }
            else
            {
                string info = "Total(" + this.HisWorkerLists.Count + ") People receive \t\n";
                foreach (GenerWorkerList wl in this.HisWorkerLists)
                {
                    info += BP.WF.Glo.DealUserInfoShowModel(wl.FK_DeptT, wl.FK_EmpText) + "\t\n";
                }
                this.AddToTrack(at, this.Execer, " People accept ( See information bar )", town.HisNode.NodeID, town.HisNode.Name, info, this.ndFrom);
            }
            #endregion

            #region  The data added to a variable .
            string ids = "";
            string names = "";
            string idNames = "";
            if (this.HisWorkerLists.Count == 1)
            {
                GenerWorkerList gwl = (GenerWorkerList)this.HisWorkerLists[0];
                ids = gwl.FK_Emp;
                names = gwl.FK_EmpText;

                // Set state .
                this.HisGenerWorkFlow.TaskSta = TaskSta.None;
            }
            else
            {
                foreach (GenerWorkerList gwl in this.HisWorkerLists)
                {
                    ids += gwl.FK_Emp + ",";
                    names += gwl.FK_EmpText + ",";
                    // idNames += gwl.FK_Emp + " " + gwl.FK_EmpText + ",";
                }

                // Set state ,  If the process is used to enable the shared task pool .
                if (town.HisNode.IsEnableTaskPool && this.HisNode.HisRunModel == RunModel.Ordinary)
                    this.HisGenerWorkFlow.TaskSta = TaskSta.Sharing;
                else
                    this.HisGenerWorkFlow.TaskSta = TaskSta.None;
            }

            this.addMsg(SendReturnMsgFlag.VarAcceptersID, ids, ids, SendReturnMsgType.SystemMsg);
            this.addMsg(SendReturnMsgFlag.VarAcceptersName, names, names, SendReturnMsgType.SystemMsg);
            this.addMsg(SendReturnMsgFlag.VarAcceptersNID, idNames, idNames, SendReturnMsgType.SystemMsg);
            #endregion

            return this.HisWorkerLists;
        }
        #endregion


        #region  Assess the situation one multisectoral 
        /// <summary>
        /// HisDeptOfUse
        /// </summary>
        private Dept _HisDeptOfUse = null;
        /// <summary>
        /// HisDeptOfUse
        /// </summary>
        public Dept HisDeptOfUse
        {
            get
            {
                if (_HisDeptOfUse == null)
                {
                    // Find all departments .
                    Depts depts;
                    if (this.HisWork.Rec == this.Execer)
                        depts = WebUser.HisDepts;
                    else
                        depts = this.HisWork.RecOfEmp.HisDepts;

                    if (depts.Count == 0)
                    {
                        throw new Exception(" You do not have to [" + this.HisWork.Rec + "] Setting department .");
                    }

                    if (depts.Count == 1) /*  If all of the department is only one , Returns it .*/
                    {
                        _HisDeptOfUse = (Dept)depts[0];
                        return _HisDeptOfUse;
                    }

                    if (_HisDeptOfUse == null)
                    {
                        /*  If not found , Would return the first division . */
                        _HisDeptOfUse = depts[0] as Dept;
                    }
                }
                return _HisDeptOfUse;
            }
        }
        #endregion

        #region  Condition 
        private Conds _HisNodeCompleteConditions = null;
        /// <summary>
        ///  Conditions node to complete the task 
        ///  Between conditions and conditions or  Relationship ,  That is , If any one of the conditions are met , The staff at this node task is complete .
        /// </summary>
        public Conds HisNodeCompleteConditions
        {
            get
            {
                if (this._HisNodeCompleteConditions == null)
                {
                    _HisNodeCompleteConditions = new Conds(CondType.Node, this.HisNode.NodeID, this.WorkID, this.rptGe);

                    return _HisNodeCompleteConditions;
                }
                return _HisNodeCompleteConditions;
            }
        }
        private Conds _HisFlowCompleteConditions = null;
        /// <summary>
        ///  His condition to complete the task , This node is a set of conditions to complete the task 
        ///  Between conditions and conditions or  Relationship ,  That is , If any one of the conditions are met , This task is complete .
        /// </summary>
        public Conds HisFlowCompleteConditions
        {
            get
            {
                if (this._HisFlowCompleteConditions == null)
                {
                    _HisFlowCompleteConditions = new Conds(CondType.Flow, this.HisNode.NodeID, this.WorkID, this.rptGe);
                }
                return _HisFlowCompleteConditions;
            }
        }
        #endregion

        #region  About quality assessment 
        ///// <summary>
        /////  Obtained before the work has been completed node .
        ///// </summary>
        ///// <returns></returns>
        //public WorkNodes GetHadCompleteWorkNodes()
        //{
        //    WorkNodes mywns = new WorkNodes();
        //    WorkNodes wns = new WorkNodes(this.HisNode.HisFlow, this.HisWork.OID);
        //    foreach (WorkNode wn in wns)
        //    {
        //        if (wn.IsComplete)
        //            mywns.Add(wn);
        //    }
        //    return mywns;
        //}
        #endregion

        #region  Process Public Methods 
        private Flow _HisFlow = null;
        public Flow HisFlow
        {
            get
            {
                if (_HisFlow == null)
                { _HisFlow = this.HisNode.HisFlow;
                _HisFlow.mailUrlsPattern = this.mailUrlsPattern;
                }
                return _HisFlow;
            }
        }
        private Node JumpToNode = null;
        private string JumpToEmp = null;


        #region NodeSend  Subsidiary function .
        public Node NodeSend_GenerNextStepNode()
        {
            // If we jump to the node , Rules rule will fail automatically jump .
            if (this.JumpToNode != null)
                return this.JumpToNode;

            #region delete by zhoupeng 14.11.12  If you do it , Can not execute automatically jump up .
            //Nodes toNDs = this.HisNode.HisToNodes;
            //if (toNDs.Count == 1)
            //{
            //    Node mynd = toNDs[0] as Node;
            //    // Write arrival information .
            //    this.addMsg(SendReturnMsgFlag.VarToNodeID, mynd.NodeID.ToString(), mynd.NodeID.ToString(),
            //     SendReturnMsgType.SystemMsg);
            //    this.addMsg(SendReturnMsgFlag.VarToNodeName, mynd.Name, mynd.Name, SendReturnMsgType.SystemMsg);
            //    return mynd;
            //}
            #endregion delete by zhoupeng 14.11.12


            //  Determine whether there is a node selected by the user .
            if (this.HisNode.CondModel == CondModel.ByUserSelected)
            {
                //  Get the node selected by the user .
                string nodes = this.HisGenerWorkFlow.Paras_ToNodes;
                if (string.IsNullOrEmpty(nodes))
                    throw new Exception("@ Users do not have to send to the node selection .");

                string[] mynodes = nodes.Split(',');
                foreach (string item in mynodes)
                {
                    if (string.IsNullOrEmpty(item))
                        continue;

                    return new Node(int.Parse(item));
                }

                // Set him empty , To prevent the next transmission errors .
                this.HisGenerWorkFlow.Paras_ToNodes = "";
            }

            Node nd = NodeSend_GenerNextStepNode_Ext1();
            // Write arrival information .
            this.addMsg(SendReturnMsgFlag.VarToNodeID, nd.NodeID.ToString(), nd.NodeID.ToString(),
             SendReturnMsgType.SystemMsg);
            this.addMsg(SendReturnMsgFlag.VarToNodeName, nd.Name, nd.Name, SendReturnMsgType.SystemMsg);
            return nd;
        }
        /// <summary>
        ///  Know whether the jump is executed .
        /// </summary>
        public bool IsSkip = false;
        /// <summary>
        ///  Get the next step of the work node .
        /// </summary>
        /// <returns></returns>
        public Node NodeSend_GenerNextStepNode_Ext1()
        {
            // If we jump to the node , Rules rule will fail automatically jump .
            if (this.JumpToNode != null)
                return this.JumpToNode;

            Node mynd = this.HisNode;
            Work mywork = this.HisWork;

            this.ndFrom = this.HisNode;
            while (true)
            {
                // Previous work node .
                int prvNodeID = mynd.NodeID;
                if (mynd.IsEndNode)
                {
                    /* If it is the last node of the , Still can not find the next node ...*/
                    this.IsStopFlow = true;
                    return mynd;
                }

                //  Get it next node .
                Node nd = this.NodeSend_GenerNextStepNode_Ext(mynd);
                mynd = nd;
                Work skip = null;
                if (mywork.NodeFrmID != nd.NodeFrmID)
                {
                    /* Jump over the node should write data , Or will cause signature error .*/
                    skip = nd.HisWork;
                    skip.Copy(this.rptGe);
                    skip.Copy(mywork);

                    skip.OID = this.WorkID;
                    skip.Rec = this.Execer;
                    skip.SetValByKey(WorkAttr.RDT, DataType.CurrentDataTimess);
                    skip.SetValByKey(WorkAttr.CDT, DataType.CurrentDataTimess);
                    
                    skip.ResetDefaultVal();
                    
                   
                    //  The inside of the default values copy Reports go inside .
                    rptGe.Copy(skip);

                    // If there is to modify 
                    if (skip.IsExit(skip.PK, this.WorkID) == true)
                        skip.DirectUpdate();
                    else
                        skip.InsertAsOID(this.WorkID);

                    #region   Initialization initiated work node .

                    if (1 == 2)
                    {

#warning zhoupeng  Delete  2014-06-20,  Should not exist here .
                        if (this.HisWork.EnMap.PhysicsTable == nd.HisWork.EnMap.PhysicsTable)
                        {
                            /* This is the data consolidation mode ,  Not executed copy*/
                        }
                        else
                        {
                            /*  If not want to wait two data sources , Implementation copy. */
                            #region  Copy the attachment .
                            if (this.HisNode.MapData.FrmAttachments.Count > 0)
                            {
                                FrmAttachmentDBs athDBs = new FrmAttachmentDBs("ND" + this.HisNode.NodeID,
                                      this.WorkID.ToString());
                                int idx = 0;
                                if (athDBs.Count > 0)
                                {
                                    athDBs.Delete(FrmAttachmentDBAttr.FK_MapData, "ND" + nd.NodeID,
                                        FrmAttachmentDBAttr.RefPKVal, this.WorkID);

                                    /* Description of the current node has an attachment data */
                                    foreach (FrmAttachmentDB athDB in athDBs)
                                    {
                                        idx++;
                                        FrmAttachmentDB athDB_N = new FrmAttachmentDB();
                                        athDB_N.Copy(athDB);
                                        athDB_N.FK_MapData = "ND" + nd.NodeID;
                                        athDB_N.RefPKVal = this.WorkID.ToString();

                                        // athDB_N.MyPK = this.WorkID + "_" + idx + "_" + athDB_N.FK_MapData;
                                        //   if (athDB.dbt
                                        //  athDB_N.MyPK = athDB_N.FK_FrmAttachment.Replace("ND" + this.HisNode.NodeID, "ND" + nd.NodeID) + "_" + this.WorkID;

                                        athDB_N.MyPK = DBAccess.GenerGUID(); // athDB_N.FK_FrmAttachment.Replace("ND" + this.HisNode.NodeID, "ND" + nd.NodeID) + "_" + this.WorkID;

                                        athDB_N.FK_FrmAttachment = athDB_N.FK_FrmAttachment.Replace("ND" + this.HisNode.NodeID,
                                           "ND" + nd.NodeID);

                                        athDB_N.Save();
                                    }
                                }
                            }
                            #endregion  Copy the attachment .

                            #region  Copy Picture upload attachments .
                            if (this.HisNode.MapData.FrmImgAths.Count > 0)
                            {
                                FrmImgAthDBs athDBs = new FrmImgAthDBs("ND" + this.HisNode.NodeID,
                                      this.WorkID.ToString());
                                int idx = 0;
                                if (athDBs.Count > 0)
                                {
                                    athDBs.Delete(FrmAttachmentDBAttr.FK_MapData, "ND" + nd.NodeID,
                                        FrmAttachmentDBAttr.RefPKVal, this.WorkID);

                                    /* Description of the current node has an attachment data */
                                    foreach (FrmImgAthDB athDB in athDBs)
                                    {
                                        idx++;
                                        FrmImgAthDB athDB_N = new FrmImgAthDB();
                                        athDB_N.Copy(athDB);
                                        athDB_N.FK_MapData = "ND" + nd.NodeID;
                                        athDB_N.RefPKVal = this.WorkID.ToString();
                                        athDB_N.MyPK = this.WorkID + "_" + idx + "_" + athDB_N.FK_MapData;
                                        athDB_N.FK_FrmImgAth = athDB_N.FK_FrmImgAth.Replace("ND" + this.HisNode.NodeID, "ND" + nd.NodeID);
                                        athDB_N.Save();
                                    }
                                }
                            }
                            #endregion  Copy Picture upload attachments .

                            #region  Copy Ele
                            if (this.HisNode.MapData.FrmEles.Count > 0)
                            {
                                FrmEleDBs eleDBs = new FrmEleDBs("ND" + this.HisNode.NodeID,
                                      this.WorkID.ToString());
                                if (eleDBs.Count > 0)
                                {
                                    eleDBs.Delete(FrmEleDBAttr.FK_MapData, "ND" + nd.NodeID,
                                        FrmEleDBAttr.RefPKVal, this.WorkID);

                                    /* Description of the current node has an attachment data */
                                    foreach (FrmEleDB eleDB in eleDBs)
                                    {
                                        FrmEleDB eleDB_N = new FrmEleDB();
                                        eleDB_N.Copy(eleDB);
                                        eleDB_N.FK_MapData = "ND" + nd.NodeID;
                                        eleDB_N.GenerPKVal();
                                        eleDB_N.Save();
                                    }
                                }
                            }
                            #endregion  Copy Ele

                            #region  Copy the multiple choice data 
                            if (this.HisNode.MapData.MapM2Ms.Count > 0)
                            {
                                M2Ms m2ms = new M2Ms("ND" + this.HisNode.NodeID, this.WorkID);
                                if (m2ms.Count >= 1)
                                {
                                    foreach (M2M item in m2ms)
                                    {
                                        M2M m2 = new M2M();
                                        m2.Copy(item);
                                        m2.EnOID = this.WorkID;
                                        m2.FK_MapData = m2.FK_MapData.Replace("ND" + this.HisNode.NodeID, "ND" + nd.NodeID);
                                        m2.InitMyPK();
                                        try
                                        {
                                            m2.DirectInsert();
                                        }
                                        catch
                                        {
                                            m2.DirectUpdate();
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region  Copy the detail 
                            // int deBugDtlCount=
                            Sys.MapDtls dtls = this.HisNode.MapData.MapDtls;
                            string recDtlLog = "@ Record test schedule Copy Process , From node ID:" + this.HisNode.NodeID + " WorkID:" + this.WorkID + ",  To node ID=" + nd.NodeID;
                            if (dtls.Count > 0)
                            {
                                Sys.MapDtls toDtls = nd.MapData.MapDtls;
                                recDtlLog += "@ To list the number of nodes is :" + dtls.Count + "";

                                Sys.MapDtls startDtls = null;
                                bool isEnablePass = false; /* Is there a list of approvals .*/
                                foreach (MapDtl dtl in dtls)
                                {
                                    if (dtl.IsEnablePass)
                                        isEnablePass = true;
                                }

                                if (isEnablePass) /*  If you have to build it began node table data  */
                                    startDtls = new BP.Sys.MapDtls("ND" + int.Parse(nd.FK_Flow) + "01");

                                recDtlLog += "@ Started one by one into the circulation list copy.";
                                int i = -1;
                                foreach (Sys.MapDtl dtl in dtls)
                                {
                                    recDtlLog += "@ Enter the cycle begins execution schedule (" + dtl.No + ")copy.";

                                    //i++;
                                    //if (toDtls.Count <= i)
                                    //    continue;

                                    //Sys.MapDtl toDtl = (Sys.MapDtl)toDtls[i];


                                    i++;
                                    //if (toDtls.Count <= i)
                                    //    continue;
                                    Sys.MapDtl toDtl = null;

                                    foreach (MapDtl todtl in toDtls)
                                    {
                                        if (todtl.Name.Substring(6, todtl.Name.Length - 6).Equals(dtl.Name.Substring(6, dtl.Name.Length - 6)))
                                        {
                                            toDtl = todtl;
                                            break;
                                        }
                                    }

                                    if (toDtl == null)
                                        continue;

                                    if (dtl.IsEnablePass == true)
                                    {
                                        /* If the audit schedule through mechanism is enabled , Allows copy Node data .*/
                                        toDtl.IsCopyNDData = true;
                                    }

                                    if (toDtl.IsCopyNDData == false)
                                        continue;

                                    // Get detailed data .
                                    GEDtls gedtls = new GEDtls(dtl.No);
                                    QueryObject qo = null;
                                    qo = new QueryObject(gedtls);
                                    switch (dtl.DtlOpenType)
                                    {
                                        case DtlOpenType.ForEmp:
                                            qo.AddWhere(GEDtlAttr.RefPK, this.WorkID);
                                            break;
                                        case DtlOpenType.ForWorkID:
                                            qo.AddWhere(GEDtlAttr.RefPK, this.WorkID);
                                            break;
                                        case DtlOpenType.ForFID:
                                            qo.AddWhere(GEDtlAttr.FID, this.WorkID);
                                            break;
                                    }
                                    qo.DoQuery();

                                    recDtlLog += "@ Check out the schedule :" + dtl.No + ", Detail :" + gedtls.Count + " records.";

                                    int unPass = 0;
                                    //  Whether audit mechanism to enable .
                                    isEnablePass = dtl.IsEnablePass;
                                    if (isEnablePass && this.HisNode.IsStartNode == false)
                                        isEnablePass = true;
                                    else
                                        isEnablePass = false;

                                    if (isEnablePass == true)
                                    {
                                        /* Determine whether the current node on the list ,isPass  Field audits , If no exception is thrown Information .*/
                                        if (gedtls.Count != 0)
                                        {
                                            GEDtl dtl1 = gedtls[0] as GEDtl;
                                            if (dtl1.EnMap.Attrs.Contains("IsPass") == false)
                                                isEnablePass = false;
                                        }
                                    }

                                    recDtlLog += "@ Delete arrival schedule :" + dtl.No + ", Data ,  And began to traverse the list , Execution line by line copy.";
                                    DBAccess.RunSQL("DELETE FROM " + toDtl.PTable + " WHERE RefPK=" + dbStr + "RefPK", "RefPK", this.WorkID.ToString());

                                    // copy Quantity .
                                    int deBugNumCopy = 0;
                                    foreach (GEDtl gedtl in gedtls)
                                    {
                                        if (isEnablePass)
                                        {
                                            if (gedtl.GetValBooleanByKey("IsPass") == false)
                                            {
                                                /* There will be no review by the  continue  They , Copy only been approved by the .*/
                                                continue;
                                            }
                                        }

                                        BP.Sys.GEDtl dtCopy = new GEDtl(toDtl.No);
                                        dtCopy.Copy(gedtl);
                                        dtCopy.FK_MapDtl = toDtl.No;
                                        dtCopy.RefPK = this.WorkID.ToString();
                                        dtCopy.InsertAsOID(dtCopy.OID);
                                        dtCopy.RefPKInt64 = this.WorkID;
                                        deBugNumCopy++;

                                        #region   Schedule single copy  -  Additional information 
                                        if (toDtl.IsEnableAthM)
                                        {
                                            /* If multiple attachments enabled , Additional information on the copy of this detail data .*/
                                            FrmAttachmentDBs athDBs = new FrmAttachmentDBs(dtl.No, gedtl.OID.ToString());
                                            if (athDBs.Count > 0)
                                            {
                                                i = 0;
                                                foreach (FrmAttachmentDB athDB in athDBs)
                                                {
                                                    i++;
                                                    FrmAttachmentDB athDB_N = new FrmAttachmentDB();
                                                    athDB_N.Copy(athDB);
                                                    athDB_N.FK_MapData = toDtl.No;
                                                    athDB_N.MyPK = toDtl.No + "_" + dtCopy.OID + "_" + i.ToString();
                                                    athDB_N.FK_FrmAttachment = athDB_N.FK_FrmAttachment.Replace("ND" + this.HisNode.NodeID,
                                                        "ND" + nd.NodeID);
                                                    athDB_N.RefPKVal = dtCopy.OID.ToString();
                                                    athDB_N.DirectInsert();
                                                }
                                            }
                                        }
                                        if (toDtl.IsEnableM2M || toDtl.IsEnableM2MM)
                                        {
                                            /* If you enable m2m */
                                            M2Ms m2ms = new M2Ms(dtl.No, gedtl.OID);
                                            if (m2ms.Count > 0)
                                            {
                                                i = 0;
                                                foreach (M2M m2m in m2ms)
                                                {
                                                    i++;
                                                    M2M m2m_N = new M2M();
                                                    m2m_N.Copy(m2m);
                                                    m2m_N.FK_MapData = toDtl.No;
                                                    m2m_N.MyPK = toDtl.No + "_" + m2m.M2MNo + "_" + gedtl.ToString() + "_" + m2m.DtlObj;
                                                    m2m_N.EnOID = gedtl.OID;
                                                    m2m.InitMyPK();
                                                    m2m_N.DirectInsert();
                                                }
                                            }
                                        }
                                        #endregion   Schedule single copy  -  Additional information 

                                    }
#warning  Logging .
                                    if (gedtls.Count != deBugNumCopy)
                                    {
                                        recDtlLog += "@ From the list :" + dtl.No + ", Detail :" + gedtls.Count + " records.";
                                        // Logging .
                                        Log.DefaultLogWriteLineInfo(recDtlLog);
                                        throw new Exception("@ System error , Keep the following information back to the administrator , Thank you .:  Technical Information :" + recDtlLog);
                                    }

                                    #region  If the audit mechanism to enable 
                                    if (isEnablePass)
                                    {
                                        /*  If the mechanism is enabled by the audit , Put unaudited data copy Up to the first node  
                                         * 1,  Find the corresponding breakdown point .
                                         * 2,  Copy the data is not audited by the schedule to begin in .
                                         */
                                        string fk_mapdata = "ND" + int.Parse(nd.FK_Flow) + "01";
                                        MapData md = new MapData(fk_mapdata);
                                        string startUser = "SELECT Rec FROM " + md.PTable + " WHERE OID=" + this.WorkID;
                                        startUser = DBAccess.RunSQLReturnString(startUser);

                                        MapDtl startDtl = (MapDtl)startDtls[i];
                                        foreach (GEDtl gedtl in gedtls)
                                        {
                                            if (gedtl.GetValBooleanByKey("IsPass"))
                                                continue; /*  Preclude review by the  */

                                            BP.Sys.GEDtl dtCopy = new GEDtl(startDtl.No);
                                            dtCopy.Copy(gedtl);
                                            dtCopy.OID = 0;
                                            dtCopy.FK_MapDtl = startDtl.No;
                                            dtCopy.RefPK = gedtl.OID.ToString(); //this.WorkID.ToString();
                                            dtCopy.SetValByKey("BatchID", this.WorkID);
                                            dtCopy.SetValByKey("IsPass", 0);
                                            dtCopy.SetValByKey("Rec", startUser);
                                            dtCopy.SetValByKey("Checker", this.ExecerName);
                                            dtCopy.RefPKInt64 = this.WorkID;
                                            dtCopy.SaveAsOID(gedtl.OID);
                                        }
                                        DBAccess.RunSQL("UPDATE " + startDtl.PTable + " SET Rec='" + startUser + "',Checker='" + this.Execer + "' WHERE BatchID=" + this.WorkID + " AND Rec='" + this.Execer + "'");
                                    }
                                    #endregion  If the audit mechanism to enable 
                                }
                            }
                            #endregion  Copy the detail 
                        }
                    }
                    #endregion  Initialization initiated work node .

                    IsSkip = true;
                    mywork = skip;
                }

                // Jump to determine whether to set up , Is not set to return to his .
                if (nd.AutoJumpRole0 == false
                    && nd.AutoJumpRole1 == false
                    && nd.AutoJumpRole2 == false)
                    return nd;

                FindWorker fw = new FindWorker();
                WorkNode toWn = new WorkNode(this.WorkID, nd.NodeID);
                DataTable dt = fw.DoIt(this.HisFlow, this, toWn); //  The next step is to find the recipient .
                if (dt == null || dt.Rows.Count == 0)
                {
                    if (nd.HisWhenNoWorker == WhenNoWorker.Skip)
                    {
                        this.AddToTrack(ActionType.Skip, this.Execer, this.ExecerName,
                            nd.NodeID, nd.Name, " Automatically jump .( When people did not find the handle )", ndFrom);
                        ndFrom = nd;
                        continue;
                    }
                    else
                        throw new Exception("@ Did not find the man .");
                }

                if (dt.Rows.Count == 0)
                    throw new Exception("@ Did not find the next node (" + nd.Name + ") Handling people ");

                if (nd.AutoJumpRole0)
                {
                    /* Who is the sponsor deal */
                    bool isHave = false;
                    foreach (DataRow dr in dt.Rows)
                    {
                        //  If there   Who is handling the case of the promoters .
                        if (dr[0].ToString() == this.HisGenerWorkFlow.Starter)
                        {
                            #region  Processing signature , Let signatures are sponsors .
                            Attrs attrs = skip.EnMap.Attrs;
                            bool isUpdate = false;
                            foreach (Attr attr in attrs)
                            {
                                if (attr.UIIsReadonly && attr.UIVisible == true
                                    )
                                {
                                    if (attr.DefaultValOfReal == "@WebUser.No")
                                    {
                                        skip.SetValByKey(attr.Key, this.HisGenerWorkFlow.Starter);
                                        isUpdate = true;
                                    }
                                    if (attr.DefaultValOfReal == "@WebUser.Name")
                                    {
                                        skip.SetValByKey(attr.Key, this.HisGenerWorkFlow.StarterName);
                                        isUpdate = true;
                                    }
                                    if (attr.DefaultValOfReal == "@WebUser.FK_Dept")
                                    {
                                        skip.SetValByKey(attr.Key, this.HisGenerWorkFlow.FK_Dept);
                                        isUpdate = true;
                                    }
                                    if (attr.DefaultValOfReal == "@WebUser.DeptName")
                                    {
                                        skip.SetValByKey(attr.Key, this.HisGenerWorkFlow.DeptName);
                                        isUpdate = true;
                                    }
                                }
                            }
                            if (isUpdate)
                                skip.DirectUpdate();
                            #endregion  Processing signature , Let signatures are sponsors .

                            isHave = true;
                            break;
                        }
                    }

                    if (isHave == true)
                    {
                        /* If found , The current staff who deal with the collection contains .*/
                        this.AddToTrack(ActionType.Skip, this.Execer, this.ExecerName, nd.NodeID, nd.Name, " Automatically jump ,( Who is the author treatment )", ndFrom);
                        ndFrom = nd;
                        continue;
                    }

                    // If you do not jump , To determine whether , The other two conditions are set .
                    if (nd.AutoJumpRole1 == false && nd.AutoJumpRole2 == false)
                        return nd;
                }

                if (nd.AutoJumpRole1)
                {
                    /* Treatment have occurred */
                    bool isHave = false;
                    foreach (DataRow dr in dt.Rows)
                    {
                        //  If there is a situation that is the author treatment of people .
                        string sql = "SELECT COUNT(*) FROM WF_GenerWorkerList WHERE FK_Emp='" + dr[0].ToString() + "' AND WorkID=" + this.WorkID;
                        if (DBAccess.RunSQLReturnValInt(sql) == 1)
                        {
                            /* Here does not deal with the signature .*/
                            isHave = true;
                            break;
                        }
                    }
                    if (isHave == true)
                    {
                        this.AddToTrack(ActionType.Skip, this.Execer, this.ExecerName, nd.NodeID, nd.Name, " Automatically jump .( Treatment have occurred )", ndFrom);
                        ndFrom = nd;
                        continue;
                    }

                    // If you do not jump , To determine whether , The other two conditions are set .
                    if (nd.AutoJumpRole2 == false)
                        return nd;
                }

                if (nd.AutoJumpRole2)
                {
                    /*  People with the same processing step  */
                    bool isHave = false;
                    foreach (DataRow dr in dt.Rows)
                    {
                        string sql = "SELECT COUNT(*) FROM WF_GenerWorkerList WHERE FK_Emp='" + this.Execer + "' AND WorkID=" + this.WorkID + " AND FK_Node=" + prvNodeID;
                        if (DBAccess.RunSQLReturnValInt(sql) == 1)
                        {
                            /* Here does not deal with the signature .*/
                            isHave = true;
                            break;
                        }
                    }

                    if (isHave == true)
                    {
                        this.AddToTrack(ActionType.Skip, this.Execer, this.ExecerName, nd.NodeID, nd.Name, " Automatically jump .( People with the same processing step )", ndFrom);
                        ndFrom = nd;
                        continue;
                    }

                    // Conditions did not jump out of turn , Returns itself .
                    return nd;
                }

                mynd = nd;
                ndFrom = nd;
            }// End loop .

            throw new Exception("@ Find the next node .");
        }
        /// <summary>
        ///  Deal with OrderTeamup Return mode 
        /// </summary>
        public void DealReturnOrderTeamup()
        {
            /* If Collaboration , Sequential manner .*/
            GenerWorkerList gwl = new GenerWorkerList();
            gwl.FK_Emp = WebUser.No;
            gwl.FK_Node = this.HisNode.NodeID;
            gwl.WorkID = this.WorkID;
            if (gwl.RetrieveFromDBSources() == 0)
                throw new Exception("@ Did not find their desired data .");
            gwl.IsPass = true;
            gwl.Update();

            gwl.FK_Emp = this.JumpToEmp;
            gwl.FK_Node = this.JumpToNode.NodeID;
            gwl.WorkID = this.WorkID;
            if (gwl.RetrieveFromDBSources() == 0)
                throw new Exception("@ The recipient did not find the desired data .");

            gwl.IsPass = false;
            gwl.Update();
            GenerWorkerLists ens = new GenerWorkerLists();
            ens.AddEntity(gwl);
            this.HisWorkerLists = ens;

            this.addMsg(SendReturnMsgFlag.VarAcceptersID, gwl.FK_Emp, gwl.FK_Emp, SendReturnMsgType.SystemMsg);
            this.addMsg(SendReturnMsgFlag.VarAcceptersName, gwl.FK_EmpText, gwl.FK_EmpText, SendReturnMsgType.SystemMsg);
            this.addMsg(SendReturnMsgFlag.OverCurr, " The current work has been sent to the return of people (" + gwl.FK_Emp + "," + gwl.FK_EmpText + ").", null, SendReturnMsgType.Info);

            this.HisGenerWorkFlow.WFState = WFState.Runing;
            this.HisGenerWorkFlow.FK_Node = gwl.FK_Node;
            this.HisGenerWorkFlow.NodeName = gwl.FK_NodeText;

            this.HisGenerWorkFlow.TodoEmps = gwl.FK_Emp;
            this.HisGenerWorkFlow.TodoEmpsNum = 0;
            this.HisGenerWorkFlow.TaskSta = TaskSta.None;
            this.HisGenerWorkFlow.Update();
        }
        /// <summary>
        ///  Get the next step of the work node 
        /// </summary>
        /// <returns></returns>
        private Node NodeSend_GenerNextStepNode_Ext(Node currNode)
        {
            //  Check whether the current state is returned ,.
            if (this.SendNodeWFState == WFState.ReturnSta)
            {

            }

            Nodes nds = currNode.HisToNodes;
            if (nds.Count == 1)
            {
                Node toND = (Node)nds[0];
                return toND;
            }

            if (nds.Count == 0)
                throw new Exception(" Did not find it under the step node .");

            Conds dcsAll = new Conds();
            dcsAll.Retrieve(CondAttr.NodeID, currNode.NodeID, CondAttr.CondType, (int)CondType.Dir, CondAttr.PRI);
            if (dcsAll.Count == 0)
                throw new Exception("@ No node (" + currNode.NodeID + " , " + currNode.Name + ") Conditions set direction ");

            #region  Get a set of nodes that can pass through , If you do not set the direction of the condition on by default .
            Nodes myNodes = new Nodes();
            int toNodeId = 0;
            int numOfWay = 0;
            foreach (Node nd in nds)
            {
                Conds dcs = new Conds();
                foreach (Cond dc in dcsAll)
                {
                    if (dc.ToNodeID != nd.NodeID)
                        continue;

                    dc.WorkID = this.WorkID;
                    dc.en = this.rptGe;
                    dcs.AddEntity(dc);
                }

                if (dcs.Count == 0)
                {
                    throw new Exception("@ Process design errors : From node (" + currNode.Name + ") To node (" + nd.Name + "), The condition is not set direction , There branch node must have direction Conditions .");
                    continue;
                    // throw new Exception(string.Format(this.ToE("WN10", "@ Direction of the error condition is defined nodes : Did not give the {0} Node {1}, Definition steering condition ."), this.HisNode.NodeID + this.HisNode.Name, nd.NodeID + nd.Name));
                }

                if (dcs.IsPass) //  If it passes .
                    myNodes.AddEntity(nd);
            }
            #endregion  Get a set of nodes that can pass through , If you do not set the direction of the condition on by default .

            //  If you do not find .
            if (myNodes.Count == 0)
                throw new Exception("@ Current users (" + this.ExecerName + "), Direction of the error condition is defined nodes :From {" + currNode.NodeID + currNode.Name + "} Node to the other nodes , All turned defined conditions are not set up .");

            // If you find 1个.
            if (myNodes.Count == 1)
            {
                Node toND = myNodes[0] as Node;
                return toND;
            }


            // If you find more than one .
            foreach (Cond dc in dcsAll)
            {
                foreach (Node myND in myNodes)
                {
                    if (dc.ToNodeID == myND.NodeID)
                    {
                        return myND;
                    }
                }
            }
            throw new Exception("@ Exception should not happen , Should not be run here .");
        }
        /// <summary>
        ///  The next step is to obtain a set of nodes 
        /// </summary>
        /// <returns></returns>
        public Nodes Func_GenerNextStepNodes()
        {
            // If you already have a variable node jump .
            if (this.JumpToNode != null)
            {
                Nodes myNodesTo = new Nodes();
                myNodesTo.AddEntity(this.JumpToNode);
                return myNodesTo;
            }

            if (this.HisNode.CondModel == CondModel.ByUserSelected)
            {
                //  Get the node selected by the user .
                string nodes = this.HisGenerWorkFlow.Paras_ToNodes;
                if (string.IsNullOrEmpty(nodes))
                    throw new Exception("@ Users do not have to send to the node selection .");

                Nodes nds = new Nodes();
                string[] mynodes = nodes.Split(',');
                foreach (string item in mynodes)
                {
                    if (string.IsNullOrEmpty(item))
                        continue;
                    nds.AddEntity(new Node(int.Parse(item)));
                }
                return nds;

                // Set him empty , To prevent the next transmission errors .
                this.HisGenerWorkFlow.Paras_ToNodes = "";
            }


            Nodes toNodes = this.HisNode.HisToNodes;

            //  If only one node in turn ,  Do not judge the condition of the , He turned directly .
            if (toNodes.Count == 1)
                return toNodes;
            Conds dcsAll = new Conds();
            dcsAll.Retrieve(CondAttr.NodeID, this.HisNode.NodeID, CondAttr.PRI);

            #region  Get a set of nodes that can pass through , If you do not set the direction of the condition on by default .
            Nodes myNodes = new Nodes();
            int toNodeId = 0;
            int numOfWay = 0;

            foreach (Node nd in toNodes)
            {
                Conds dcs = new Conds();
                foreach (Cond dc in dcsAll)
                {
                    if (dc.ToNodeID != nd.NodeID)
                        continue;

                    dc.WorkID = this.HisWork.OID;
                    dc.en = this.rptGe;
                    dcs.AddEntity(dc);
                }

                if (dcs.Count == 0)
                {
                    myNodes.AddEntity(nd);
                    continue;
                }

                if (dcs.IsPass) //  If there is a plurality of steering conditions established .
                {
                    myNodes.AddEntity(nd);
                    continue;
                }
            }
            #endregion  Get a set of nodes that can pass through , If you do not set the direction of the condition on by default .

            if (myNodes.Count == 0)
                throw new Exception(string.Format("@ Direction of the error condition is defined nodes : Did not give the {0} Node to the other nodes , Define all steering steering condition or conditions that you define not substantiated .",
                    this.HisNode.NodeID + this.HisNode.Name));
            return myNodes;
        }
        /// <summary>
        ///  Check process completes conditions .
        /// </summary>
        /// <returns></returns>
        private void Func_CheckCompleteCondition()
        {
            if (this.HisNode.HisRunModel == RunModel.SubThread)
                throw new Exception("@ Process design errors : Allowed to set conditions for the process to complete the child thread .");

            this.IsStopFlow = false;
            this.addMsg("CurrWorkOver", string.Format(" Current work [{0}] Has been completed ", this.HisNode.Name));

            #region  Decision flow conditions .
            try
            {
                if (this.HisNode.HisToNodes.Count == 0 && this.HisNode.IsStartNode)
                {
                    /*  If the process is completed  */
                    string overMsg = this.HisWorkFlow.DoFlowOver(ActionType.FlowOver, " Meet the conditions of the process is completed ", this.HisNode, this.rptGe);
                    this.IsStopFlow = true;
                    this.addMsg("OneNodeFlowOver", "@ Work has been successfully processed ( A flow of work ).");
                    //msg+="@ Work has been successfully processed ( A flow of work ). @ Check out <img src='WF/Img/Btn/PrintWorkRpt.gif' ><a href='WFRpt.aspx?WorkID=" + this.HisWork.OID + "&FID=" + this.HisWork.FID + "&FK_Flow=" + this.HisNode.FK_Flow + "'target='_blank' > Work trajectory </a>";
                }

                if (this.HisNode.IsCCFlow && this.HisFlowCompleteConditions.IsPass)
                {
                    /* If you have completed the process conditions , And the process completed by the condition .*/

                    string stopMsg = this.HisFlowCompleteConditions.ConditionDesc;
                    /*  If the process is completed  */
                    string overMsg = this.HisWorkFlow.DoFlowOver(ActionType.FlowOver, " Meet the conditions of the process is completed :" + stopMsg, this.HisNode, this.rptGe);
                    this.IsStopFlow = true;

                    // string path = BP.Sys.Glo.Request.ApplicationPath;
                    string mymsg = "@ In line with the completion of the workflow conditions " + stopMsg + "" + overMsg;
                    string mymsgHtml = mymsg + "@ Check out <img src='" + VirPath + "WF/Img/Btn/PrintWorkRpt.gif' ><a href='" + VirPath + "WF/WFRpt.aspx?WorkID=" + this.HisWork.OID + "&FID=" + this.HisWork.FID + "&FK_Flow=" + this.HisNode.FK_Flow + "'target='_blank' > Work trajectory </a>";
                    this.addMsg(SendReturnMsgFlag.FlowOver, mymsg, mymsgHtml, SendReturnMsgType.Info);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("@ Judging process {0} Error closing conditions ." + ex.Message, this.HisNode.Name));
            }
            #endregion
        }
        private string Func_DoSetThisWorkOver()
        {
            // Set the end people .  
            this.rptGe.SetValByKey(GERptAttr.FK_Dept, this.HisGenerWorkFlow.FK_Dept); // This value can not change .
            this.rptGe.SetValByKey(GERptAttr.FlowEnder, this.Execer);
            this.rptGe.SetValByKey(GERptAttr.FlowEnderRDT, DataType.CurrentDataTime);
            if (this.town == null)
                this.rptGe.SetValByKey(GERptAttr.FlowEndNode, this.HisNode.NodeID);
            else
            {
                if (this.HisNode.HisRunModel == RunModel.FL || this.HisNode.HisRunModel == RunModel.FHL)
                    this.rptGe.SetValByKey(GERptAttr.FlowEndNode, this.HisNode.NodeID);
                else
                    this.rptGe.SetValByKey(GERptAttr.FlowEndNode, this.town.HisNode.NodeID);
            }

            this.rptGe.SetValByKey(GERptAttr.FlowDaySpan,
                DataType.GetSpanDays(rptGe.FlowStartRDT, DataType.CurrentDataTime));

            // If not want to wait two physical tables .
            if (this.HisWork.EnMap.PhysicsTable != this.rptGe.EnMap.PhysicsTable)
            {
                //  Update Status .
                this.HisWork.SetValByKey("CDT", DataType.CurrentDataTime);
                this.HisWork.Rec = this.Execer;

                // Judgment is not MD5 Process ?
                if (this.HisFlow.IsMD5)
                    this.HisWork.SetValByKey("MD5", Glo.GenerMD5(this.HisWork));

                if (this.HisNode.IsStartNode)
                    this.HisWork.SetValByKey(StartWorkAttr.Title, this.HisGenerWorkFlow.Title);

                this.HisWork.DirectUpdate();
            }


            #region 2014-08-02  Upcoming delete other personnel , Increased  IsPass=0  Parameters .
            //  Other workers cleared .
            ps.SQL = "DELETE FROM WF_GenerWorkerlist WHERE IsPass=0 AND FK_Node=" + dbStr + "FK_Node AND WorkID=" + dbStr + "WorkID AND FK_Emp <> " + dbStr + "FK_Emp"; ;
            ps.Clear();
            ps.Add("FK_Node", this.HisNode.NodeID);
            ps.Add("WorkID", this.WorkID);
            ps.Add("FK_Emp", this.Execer);
            DBAccess.RunSQL(ps);
            #endregion 2014-08-02  Upcoming delete other personnel , Increased  IsPass=0  Parameters .


            ps = new Paras();
            ps.SQL = "UPDATE WF_GenerWorkerList SET IsPass=1 WHERE FK_Node=" + dbStr + "FK_Node AND WorkID=" + dbStr + "WorkID";
            ps.Add("FK_Node", this.HisNode.NodeID);
            ps.Add("WorkID", this.WorkID);
            DBAccess.RunSQL(ps);


            // 给generworkflow Assignment .
            if (this.IsStopFlow == true)
                this.HisGenerWorkFlow.WFState = WFState.Complete;
            else
                this.HisGenerWorkFlow.WFState = WFState.Runing;


            //  The process should be completed by the time .
            if (this.HisWork.EnMap.Attrs.Contains(WorkSysFieldAttr.SysSDTOfFlow))
                this.HisGenerWorkFlow.SDTOfFlow = this.HisWork.GetValStrByKey(WorkSysFieldAttr.SysSDTOfFlow);

            //  Should be completed by the time the next node .
            if (this.HisWork.EnMap.Attrs.Contains(WorkSysFieldAttr.SysSDTOfNode))
                this.HisGenerWorkFlow.SDTOfFlow = this.HisWork.GetValStrByKey(WorkSysFieldAttr.SysSDTOfNode);

            // Perform an update .
            if (this.IsStopFlow == false)
                this.HisGenerWorkFlow.Update();

            return "@ The process has been completed .";
        }
        #endregion  Subsidiary function 
        /// <summary>
        ///  Ordinary nodes to ordinary nodes 
        /// </summary>
        /// <param name="toND"> Given the need to reach a node </param>
        /// <returns> Perform message </returns>
        private void NodeSend_11(Node toND)
        {
            string sql = "";
            string errMsg = "";
            Work toWK = toND.HisWork;
            toWK.OID = this.WorkID;
            toWK.FID = this.HisWork.FID;

            //  If you execute the jump .
            if (this.IsSkip == true)
                toWK.RetrieveFromDBSources(); // There may be a jump .

            #region  Perform data initialization 
            // town.
            WorkNode town = new WorkNode(toWK, toND);

            errMsg = " First test of their staff  -  An error occurred during .";

            //  First test of their staff ．
            GenerWorkerLists gwls = this.Func_GenerWorkerLists(town);
            if (town.HisNode.TodolistModel == TodolistModel.Order && gwls.Count > 1)
            {
                /* If the node is reachable queue process node , Should set their queue order .*/
                int idx = 0;
                foreach (GenerWorkerList gwl in gwls)
                {
                    idx++;
                    if (idx == 1)
                        continue;
                    gwl.IsPassInt = idx + 100;
                    gwl.Update();
                }
            }


            #region  Save target node data .
            if (this.HisWork.EnMap.PhysicsTable != toWK.EnMap.PhysicsTable)
            {
                errMsg = " Save target node data  -  An error occurred during .";

                // Initialization data for the next step .
                GenerWorkerList gwl = gwls[0] as GenerWorkerList;
                toWK.Rec = gwl.FK_Emp;
                string emps = gwl.FK_Emp;
                if (gwls.Count != 1)
                {
                    foreach (GenerWorkerList item in gwls)
                        emps += item.FK_Emp + ",";
                }
                toWK.Emps = emps;

                try
                {
                    if (this.IsSkip == true)
                        toWK.DirectUpdate(); //  If you execute the jump .
                    else
                        toWK.DirectInsert();
                }
                catch (Exception ex)
                {
                    Log.DefaultLogWriteLineInfo("@ Appear SQL There may be no abnormal repair table , Or send repeat . Ext=" + ex.Message);
                    try
                    {
                        toWK.CheckPhysicsTable();
                        toWK.DirectUpdate();
                    }
                    catch (Exception ex1)
                    {
                        Log.DefaultLogWriteLineInfo("@ Save the error :" + ex1.Message);
                        throw new Exception("@ Save the error :" + toWK.EnDesc + ex1.Message);
                    }
                }
            }
            #endregion  Save target node data .

            //@ Join message in the collection .
            this.SendMsgToThem(gwls);

            string htmlInfo = string.Format("@ Tasks are automatically sent to {0} As follows {1} Bit processors ,{2}.", this.nextStationName,
                this.HisRememberMe.NumOfObjs.ToString(), this.HisRememberMe.EmpsExt);

            string textInfo = string.Format("@ Tasks are automatically sent to {0} As follows {1} Bit processors ,{2}.", this.nextStationName,
                this.HisRememberMe.NumOfObjs.ToString(), this.HisRememberMe.ObjsExt);

            this.addMsg(SendReturnMsgFlag.ToEmps, textInfo, htmlInfo);


            #region  Deal with audit issues , Update audit opinion in the audit component into   To node , To staff .
            Paras ps = new Paras();
            ps.SQL = "UPDATE ND" + int.Parse(toND.FK_Flow) + "Track SET NDTo=" + dbStr + "NDTo,NDToT=" + dbStr + "NDToT,EmpTo=" + dbStr + "EmpTo,EmpToT=" + dbStr + "EmpToT WHERE NDFrom=" + dbStr + "NDFrom AND EmpFrom=" + dbStr + "EmpFrom AND WorkID=" + dbStr + "WorkID AND ActionType=" + (int)ActionType.WorkCheck;
            ps.Add(TrackAttr.NDTo, toND.NodeID);
            ps.Add(TrackAttr.NDToT, toND.Name);
            ps.Add(TrackAttr.EmpTo, this.HisRememberMe.EmpsExt);
            ps.Add(TrackAttr.EmpToT, this.HisRememberMe.EmpsExt);
            ps.Add(TrackAttr.NDFrom, this.HisNode.NodeID);
            ps.Add(TrackAttr.EmpFrom, WebUser.No);
            ps.Add(TrackAttr.WorkID, this.WorkID);
            BP.DA.DBAccess.RunSQL(ps);
            #endregion  Deal with audit issues .

            //string htmlInfo = string.Format("@ Tasks are automatically sent to {0} People are treated {1}.", this.nextStationName,this._RememberMe.EmpsExt);
            //string textInfo = string.Format("@ Tasks are automatically sent to {0} People are treated {1}.", this.nextStationName,this._RememberMe.ObjsExt);

            if (this.HisWorkerLists.Count >= 2 && this.HisNode.IsTask)
            {
                if (WebUser.IsWap)
                    this.addMsg(SendReturnMsgFlag.AllotTask, null, "<a id='linkAllotTask' href=\"" + this.VirPath + "WF/WorkOpt/AllotTask.aspx?WorkID=" + this.WorkID + "&NodeID=" + toND.NodeID + "&FK_Flow=" + toND.FK_Flow + "')\"><img src='" + VirPath + "WF/Img/AllotTask.gif' border=0/> Specify a particular officers dealing </a>.", SendReturnMsgType.Info);
                else
                    this.addMsg(SendReturnMsgFlag.AllotTask, null, "<a id='linkAllotTask' href=\"javascript:WinOpen('" + VirPath + "WF/WorkOpt/AllotTask.aspx?WorkID=" + this.WorkID + "&NodeID=" + toND.NodeID + "&FK_Flow=" + toND.FK_Flow + "')\"><img src='" + VirPath + "WF/Img/AllotTask.gif' border=0/> Specify a particular officers dealing </a>.", SendReturnMsgType.Info);
            }

            //if (WebUser.IsWap == false)
            //    this.addMsg(SendReturnMsgFlag.ToEmpExt, null, "@<a href=\"javascript:WinOpen('" + VirPath + "WF/Msg/SMS.aspx?WorkID=" + this.WorkID + "&FK_Node=" + toND.NodeID + "');\" ><img src='" + VirPath + "WF/Img/SMS.gif' border=0 /> Send SMS to remind him (们)</a>", SendReturnMsgType.Info);

            if (this.HisNode.HisFormType != NodeFormType.SDKForm)
            {
                if (this.HisNode.IsStartNode)
                {
                    if (WebUser.IsWap)
                        this.addMsg(SendReturnMsgFlag.ToEmpExt, null, "@<a id='linkUnDo' href='" + VirPath + "WF/Wap/MyFlowInfo" + Glo.FromPageType + ".aspx?DoType=UnSend&WorkID=" + this.HisWork.OID + "&FK_Flow=" + toND.FK_Flow + "'><img src='" + VirPath + "WF/Img/UnDo.gif' border=0/> Revocation of this transmission </a>, <a id='linkNewProcess' href='" + VirPath + "WF/Wap/MyFlow" + Glo.FromPageType + ".aspx?FK_Flow=" + toND.FK_Flow + "&FK_Node=" + toND.FK_Flow + "01'><img src='" + VirPath + "WF/Img/New.gif' border=0/> New Process </a>.", SendReturnMsgType.Info);
                    else
                        this.addMsg(SendReturnMsgFlag.ToEmpExt, null, "@<a id='linkUnDo' href='" + this.VirPath + this.AppType + "/MyFlowInfo" + Glo.FromPageType + ".aspx?DoType=UnSend&WorkID=" + this.HisWork.OID + "&FK_Flow=" + toND.FK_Flow + "'><img src='" + VirPath + "WF/Img/UnDo.gif' border=0/> Revocation of this transmission </a>, <a  id='linkNewProcess' href='" + VirPath + "WF/MyFlow" + Glo.FromPageType + ".aspx?FK_Flow=" + toND.FK_Flow + "&FK_Node=" + toND.FK_Flow + "01'><img src='" + VirPath + "WF/Img/New.gif' border=0/> New Process </a>.", SendReturnMsgType.Info);
                }
                else
                    this.addMsg(SendReturnMsgFlag.ToEmpExt, null, "@<a id='linkUnDo' href='" + this.VirPath + this.AppType + "/MyFlowInfo" + Glo.FromPageType + ".aspx?DoType=UnSend&WorkID=" + this.HisWork.OID + "&FK_Flow=" + toND.FK_Flow + "'><img src='" + VirPath + "WF/Img/UnDo.gif' border=0/> Revocation of this transmission </a>.", SendReturnMsgType.Info);
            }


            this.HisGenerWorkFlow.FK_Node = toND.NodeID;
            this.HisGenerWorkFlow.NodeName = toND.Name;

            //ps = new Paras();
            //ps.SQL = "UPDATE WF_GenerWorkFlow SET WFState=" + (int)WFState.Runing + ", FK_Node=" + dbStr + "FK_Node,NodeName=" + dbStr + "NodeName WHERE WorkID=" + dbStr + "WorkID";
            //ps.Add("FK_Node", toND.NodeID);
            //ps.Add("NodeName", toND.Name);
            //ps.Add("WorkID", this.HisWork.OID);
            //DBAccess.RunSQL(ps);

            if (this.HisNode.HisFormType == NodeFormType.SDKForm || this.HisNode.HisFormType == NodeFormType.SelfForm)
            {
            }
            else
            {
                this.addMsg(SendReturnMsgFlag.WorkRpt, null, "@<img src='" + VirPath + "WF/Img/Btn/PrintWorkRpt.gif' ><a id='linkRpt' href='" + VirPath + "WF/WFRpt.aspx?WorkID=" + this.HisWork.OID + "&FID=" + this.HisWork.FID + "&FK_Flow=" + toND.FK_Flow + "'target='_blank' > Work trajectory </a>.");
            }
            this.addMsg(SendReturnMsgFlag.WorkStartNode, "@ Next [" + toND.Name + "] Work starts successfully .", "<span id='nextWorkStartSuccessfully' toND='"+toND.Name+"' > @ Next <font color=blue>[" + toND.Name + "]</font> Work starts successfully . </span>");
            #endregion

            #region   Initialization initiated work node .
            if (this.HisWork.EnMap.PhysicsTable == toWK.EnMap.PhysicsTable)
            {
                /* This is the data consolidation mode ,  Not executed copy*/
                this.CopyData(toWK, toND, true);
            }
            else
            {
                /*  If not want to wait two data sources , Implementation copy. */
                this.CopyData(toWK, toND, false);
            }
            #endregion  Initialization initiated work node .

            #region  It is determined whether the quality evaluation .
            if (toND.IsEval)
            {
                /* If the quality of the evaluation process */
                toWK.SetValByKey(WorkSysFieldAttr.EvalEmpNo, this.Execer);
                toWK.SetValByKey(WorkSysFieldAttr.EvalEmpName, this.ExecerName);
                toWK.SetValByKey(WorkSysFieldAttr.EvalCent, 0);
                toWK.SetValByKey(WorkSysFieldAttr.EvalNote, "");
            }
            #endregion

        }
        private void NodeSend_2X_GenerFH()
        {
            #region GenerFH
            GenerFH fh = new GenerFH();
            fh.FID = this.WorkID;
            if (this.HisNode.IsStartNode || fh.IsExits == false)
            {
                try
                {
                    fh.Title = this.HisWork.GetValStringByKey(StartWorkAttr.Title);
                }
                catch (Exception ex)
                {
                    BP.Sys.MapAttr attr = new BP.Sys.MapAttr();
                    attr.FK_MapData = "ND" + this.HisNode.NodeID;
                    attr.HisEditType = BP.En.EditType.UnDel;
                    attr.KeyOfEn = "Title";
                    int i = attr.Retrieve(MapAttrAttr.FK_MapData, attr.FK_MapData, MapAttrAttr.KeyOfEn, attr.KeyOfEn);
                    if (i == 0)
                    {
                        attr.KeyOfEn = "Title";
                        attr.Name = " Title "; // " Process title ";
                        attr.MyDataType = BP.DA.DataType.AppString;
                        attr.UIContralType = UIContralType.TB;
                        attr.LGType = FieldTypeS.Normal;
                        attr.UIVisible = true;
                        attr.UIIsEnable = true;
                        attr.UIIsLine = true;
                        attr.MinLen = 0;
                        attr.MaxLen = 200;
                        attr.IDX = -100;
                        attr.Insert();
                    }
                    fh.Title = this.Execer + "-" + this.ExecerName + " @ " + DataType.CurrentDataTime + " ";
                }
                fh.RDT = DataType.CurrentData;
                fh.FID = this.WorkID;
                fh.FK_Flow = this.HisNode.FK_Flow;
                fh.FK_Node = this.HisNode.NodeID;
                fh.GroupKey = this.Execer;
                fh.WFState = 0;
                try
                {
                    fh.DirectInsert();
                }
                catch
                {
                    fh.DirectUpdate();
                }
            }
            #endregion GenerFH
        }
        /// <summary>
        ///  Treatment diversion point sent down  to  Different forms .
        /// </summary>
        /// <returns></returns>
        private void NodeSend_24_UnSameSheet(Nodes toNDs)
        {
            NodeSend_2X_GenerFH();

            /* Start information separately for each node .*/
            string msg = "";

            #region  Check out the current process node data , Child node threads copy Data used .
            // Additional information check out on one node to .
            FrmAttachmentDBs athDBs = new FrmAttachmentDBs("ND" + this.HisNode.NodeID,
                       this.WorkID.ToString());
            // Check out on a Ele Information .
            FrmEleDBs eleDBs = new FrmEleDBs("ND" + this.HisNode.NodeID,
                       this.WorkID.ToString());
            #endregion

            // Definition of system variables .
            string workIDs = "";
            string empIDs = "";
            string empNames = "";
            string toNodeIDs = "";

            foreach (Node nd in toNDs)
            {
                msg += "@" + nd.Name + " Work has already started , Processing workers :";

                // Generate a Job Information .
                Work wk = nd.HisWork;
                wk.Copy(this.HisWork);
                wk.FID = this.HisWork.OID;
                wk.OID = BP.DA.DBAccess.GenerOID("WorkID");
                wk.BeforeSave();
                wk.DirectInsert();

                // Get its workers .
                WorkNode town = new WorkNode(wk, nd);
                GenerWorkerLists gwls = this.Func_GenerWorkerLists(town);
                if (gwls.Count == 0)
                {
                    msg += "@" + nd.Name + " Work has already started , Processing workers :";
                    msg += " Node :" + nd.Name + ", Did not find the staff can handle , This thread node fails to start .";
                    wk.Delete();
                    continue;
                }

                #region  Perform data copy.
                if (athDBs.Count > 0)
                {
                    /* Description of the current node has an attachment data */
                    int idx = 0;
                    foreach (FrmAttachmentDB athDB in athDBs)
                    {
                        idx++;
                        FrmAttachmentDB athDB_N = new FrmAttachmentDB();
                        athDB_N.Copy(athDB);
                        athDB_N.FK_MapData = "ND" + nd.NodeID;
                        athDB_N.MyPK = BP.DA.DBAccess.GenerGUID();
                        athDB_N.FK_FrmAttachment = athDB_N.FK_FrmAttachment.Replace("ND" + this.HisNode.NodeID,"ND" + nd.NodeID);
                        athDB_N.RefPKVal = wk.OID.ToString();
                        athDB_N.Insert();
                    }
                }

                if (eleDBs.Count > 0)
                {
                    /* Description of the current node has an attachment data */
                    int idx = 0;
                    foreach (FrmEleDB eleDB in eleDBs)
                    {
                        idx++;
                        FrmEleDB eleDB_N = new FrmEleDB();
                        eleDB_N.Copy(eleDB);
                        eleDB_N.FK_MapData = "ND" + nd.NodeID;
                        eleDB_N.Insert();
                    }
                }
                #endregion  Perform data copy.

                foreach (GenerWorkerList wl in gwls)
                {
                    msg += wl.FK_Emp + "," + wl.FK_EmpText + ",";
                    //  Information produced work .
                    GenerWorkFlow gwf = new GenerWorkFlow();
                    gwf.WorkID = wk.OID;
                    if (gwf.IsExits == false)
                    {
                        gwf.FID = this.WorkID;

                        //#warning  The title needs to be modified to generate rules .
                        //#warning  Let the child process Titlte Like the parent process .

                        gwf.Title = this.HisGenerWorkFlow.Title; // WorkNode.GenerTitle(this.rptGe);
                        gwf.WFState = WFState.Runing;
                        gwf.RDT = DataType.CurrentDataTime;
                        gwf.Starter = this.Execer;
                        gwf.StarterName = this.ExecerName;
                        gwf.FK_Flow = nd.FK_Flow;
                        gwf.FlowName = nd.FlowName;
                        gwf.FK_FlowSort = this.HisNode.HisFlow.FK_FlowSort;
                        gwf.FK_Node = nd.NodeID;
                        gwf.NodeName = nd.Name;
                        gwf.FK_Dept = wl.FK_Dept;
                        gwf.DeptName = wl.FK_DeptT;
                        gwf.DirectInsert();
                    }

                    ps = new Paras();
                    ps.SQL = "UPDATE WF_GenerWorkerlist SET WorkID=" + dbStr + "WorkID1,FID=" + dbStr + "FID WHERE FK_Emp=" + dbStr + "FK_Emp AND WorkID=" + dbStr + "WorkID2 AND FK_Node=" + dbStr + "FK_Node ";
                    ps.Add("WorkID1", wk.OID);
                    ps.Add("FID", this.WorkID);

                    ps.Add("FK_Emp", wl.FK_Emp);
                    ps.Add("WorkID2", wl.WorkID);
                    ps.Add("FK_Node", wl.FK_Node);
                    DBAccess.RunSQL(ps);

                    // Record variable .
                    workIDs += wk.OID.ToString() + ",";
                    empIDs += wl.FK_Emp + ",";
                    empNames += wl.FK_EmpText + ",";
                    toNodeIDs += gwf.FK_Node + ",";

                    // Updating information .
                    wk.Rec = wl.FK_Emp;
                    wk.Emps = "@" + wl.FK_Emp;
                    //wk.RDT = DataType.CurrentDataTimess;
                    wk.DirectUpdate();
                }
            }

            // Join shunt different forms , Message .
            this.addMsg("FenLiuUnSameSheet", msg);



            // Join variable .
            this.addMsg(SendReturnMsgFlag.VarTreadWorkIDs, workIDs, workIDs, SendReturnMsgType.SystemMsg);
            this.addMsg(SendReturnMsgFlag.VarAcceptersID, empIDs, empIDs, SendReturnMsgType.SystemMsg);
            this.addMsg(SendReturnMsgFlag.VarAcceptersName, empNames, empNames, SendReturnMsgType.SystemMsg);
            this.addMsg(SendReturnMsgFlag.VarToNodeIDs, toNodeIDs, toNodeIDs, SendReturnMsgType.SystemMsg);
        }
        /// <summary>
        ///  Generate diversion point 
        /// </summary>
        /// <param name="toWN"></param>
        /// <returns></returns>
        private GenerWorkerLists NodeSend_24_SameSheet_GenerWorkerList(WorkNode toWN)
        {
            return null;
        }
        /// <summary>
        ///  Treatment diversion point sent down  to  With Form .
        /// </summary>
        /// <param name="toNode"> Reach shunt node </param>
        private void NodeSend_24_SameSheet(Node toNode)
        {
            if (this.HisGenerWorkFlow.Title == " Not Generated ")
                this.HisGenerWorkFlow.Title = WorkNode.GenerTitle(this.HisFlow, this.HisWork);

            #region  Remove the child thread to node if there , Return information to prevent garbage problem , If the return does not need to deal with this part of the deal .
            ps = new Paras();
            ps.SQL = "DELETE FROM WF_GenerWorkerlist WHERE FID=" + dbStr + "FID  AND FK_Node=" + dbStr + "FK_Node";
            ps.Add("FID", this.HisWork.OID);
            ps.Add("FK_Node", toNode.NodeID);
            #endregion  Remove the child thread to node if there , Return information to prevent garbage problem , If the return does not need to deal with this part of the deal .

            #region GenerFH
            GenerFH fh = new GenerFH();
            fh.FID = this.WorkID;
            if (this.HisNode.IsStartNode || fh.IsExits == false)
            {
                try
                {
                    fh.Title = this.HisWork.GetValStringByKey(StartWorkAttr.Title);
                }
                catch (Exception ex)
                {
                    BP.Sys.MapAttr attr = new BP.Sys.MapAttr();
                    attr.FK_MapData = "ND" + this.HisNode.NodeID;
                    attr.HisEditType = BP.En.EditType.UnDel;
                    attr.KeyOfEn = "Title";
                    int i = attr.Retrieve(MapAttrAttr.FK_MapData, attr.FK_MapData, MapAttrAttr.KeyOfEn, attr.KeyOfEn);
                    if (i == 0)
                    {
                        attr.KeyOfEn = "Title";
                        attr.Name = " Title "; // " Process title ";
                        attr.MyDataType = BP.DA.DataType.AppString;
                        attr.UIContralType = UIContralType.TB;
                        attr.LGType = FieldTypeS.Normal;
                        attr.UIVisible = true;
                        attr.UIIsEnable = true;
                        attr.UIIsLine = true;
                        attr.MinLen = 0;
                        attr.MaxLen = 200;
                        attr.IDX = -100;
                        attr.Insert();
                    }
                    fh.Title = this.Execer + "-" + this.ExecerName + " @ " + DataType.CurrentDataTime + " ";
                }
                fh.RDT = DataType.CurrentData;
                fh.FID = this.WorkID;
                fh.FK_Flow = this.HisNode.FK_Flow;
                fh.FK_Node = this.HisNode.NodeID;
                fh.GroupKey = this.Execer;
                fh.WFState = 0;
                fh.Save();
            }
            #endregion GenerFH

            #region  Staff in the next step of generating 
            //  Launch .
            Work wk = toNode.HisWork;
            wk.Copy(this.rptGe);
            wk.Copy(this.HisWork);  // Basic information copied the master table .
            wk.FID = this.HisWork.OID; //  The work of the FID Set to work on a dry process ID.

            //  Arrival node .
            town = new WorkNode(wk, toNode);

            //  Produce personnel to perform the next step .
            GenerWorkerLists gwls = this.Func_GenerWorkerLists(town);

            // Clear previous data , For example, to send two .
            if (this.HisFlow.HisDataStoreModel == DataStoreModel.ByCCFlow)
                wk.Delete(WorkAttr.FID, this.HisWork.OID);

            //  Determine the number of shunt . Is there a history shunt .
            bool IsHaveFH = false;
            if (this.HisNode.IsStartNode == false)
            {
                ps = new Paras();
                ps.SQL = "SELECT COUNT(*) FROM WF_GenerWorkerlist WHERE FID=" + dbStr + "OID";
                ps.Add("OID", this.HisWork.OID);
                if (DBAccess.RunSQLReturnValInt(ps) != 0)
                    IsHaveFH = true;
            }
            #endregion  Staff in the next step of generating 

            #region  Replicate data .

            // Get the current number of nodes in the data flow .
            FrmAttachmentDBs athDBs = new FrmAttachmentDBs("ND" + this.HisNode.NodeID,
                                            this.WorkID.ToString());

            FrmEleDBs eleDBs = new FrmEleDBs("ND" + this.HisNode.NodeID,
                                           this.WorkID.ToString());

            MapDtls dtlsFrom = new MapDtls("ND" + this.HisNode.NodeID);
            if (dtlsFrom.Count > 1)
            {
                foreach (MapDtl d in dtlsFrom)
                    d.HisGEDtls_temp = null;
            }
            MapDtls dtlsTo = null;
            if (dtlsFrom.Count >= 1)
                dtlsTo = new MapDtls("ND" + toNode.NodeID);

            /// Definition of system variables .
            string workIDs = "";

            DataTable dtWork = null;
            if (toNode.HisDeliveryWay == DeliveryWay.BySQLAsSubThreadEmpsAndData)
            {
                /* If it is in accordance with the query ＳＱＬ, Determine the list of recipients and sub-thread data .*/
                string sql = toNode.DeliveryParas;
                sql = Glo.DealExp(sql, this.HisWork, null);
                dtWork = BP.DA.DBAccess.RunSQLReturnTable(sql);
            }
            if (toNode.HisDeliveryWay == DeliveryWay.ByDtlAsSubThreadEmps)
            {
                /* If it is in accordance with the schedule , Determine the list of recipients and sub-thread data .*/
                foreach (MapDtl dtl in dtlsFrom)
                {
                    // Plus order , Prevent changes , Changes in staff numbers , Treatment schedule recipient duplication .
                    string sql = "SELECT * FROM " + dtl.PTable + " WHERE RefPK=" + this.WorkID + " ORDER BY OID";
                    dtWork = BP.DA.DBAccess.RunSQLReturnTable(sql);
                    if (dtWork.Columns.Contains("UserNo"))
                        break;
                    else
                        dtWork = null;
                }
            }

            int idx = -1;
            foreach (GenerWorkerList wl in gwls)
            {
                idx++;
                Work mywk = toNode.HisWork;

                #region  Replicate data .
                mywk.Copy(this.rptGe);
                mywk.Copy(this.HisWork);  //  Copied information .
                if (dtWork != null)
                {
                    /*用IDX Treatment in order to solve , Who repeated in the data source and also in accordance with the corresponding index .*/
                    DataRow dr = dtWork.Rows[idx];

                    if (dtWork.Columns.Contains("UserNo")
                        && dr["UserNo"].ToString() == wl.FK_Emp)
                    {
                        mywk.Copy(dr);
                    }

                    if (dtWork.Columns.Contains("No")
                       && dr["No"].ToString() == wl.FK_Emp)
                    {
                        mywk.Copy(dr);
                    }
                }
                #endregion  Replicate data .

                bool isHaveEmp = false;
                if (IsHaveFH)
                {
                    /*  If you have gone through the shunt confluence , To find a single person with a FID Under the OID , To do this for the current thread ID.*/
                    ps = new Paras();
                    ps.SQL = "SELECT WorkID,FK_Node FROM WF_GenerWorkerlist WHERE FK_Node!=" + dbStr + "FK_Node AND FID=" + dbStr + "FID AND FK_Emp=" + dbStr + "FK_Emp ORDER BY RDT DESC";
                    ps.Add("FK_Node", toNode.NodeID);
                    ps.Add("FID", this.WorkID);
                    ps.Add("FK_Emp", wl.FK_Emp);
                    DataTable dt = DBAccess.RunSQLReturnTable(ps);
                    if (dt.Rows.Count == 0)
                    {
                        /* Not found , It shows no shunt node information before this person shunt . */
                        mywk.OID = DBAccess.GenerOID("WorkID");
                    }
                    else
                    {
                        int workid_old = (int)dt.Rows[0][0];
                        int fk_Node_nearly = (int)dt.Rows[0][1];
                        Node nd_nearly = new Node(fk_Node_nearly);
                        Work nd_nearly_work = nd_nearly.HisWork;
                        nd_nearly_work.OID = workid_old;
                        if (nd_nearly_work.RetrieveFromDBSources() != 0)
                        {
                            mywk.Copy(nd_nearly_work);
                            mywk.OID = workid_old;
                        }
                        else
                        {
                            mywk.OID = DBAccess.GenerOID("WorkID");
                        }

                        //  Schedule data summary table , To copy the master table to the child thread up .
                        foreach (MapDtl dtl in dtlsFrom)
                        {
                            if (dtl.IsHLDtl == false)
                                continue;

                            string sql = "SELECT * FROM " + dtl.PTable + " WHERE Rec='" + wl.FK_Emp + "' AND RefPK='" + this.WorkID + "'";
                            DataTable myDT = DBAccess.RunSQLReturnTable(sql);
                            if (myDT.Rows.Count == 1)
                            {
                                Attrs attrs = mywk.EnMap.Attrs;
                                foreach (Attr attr in attrs)
                                {
                                    switch (attr.Key)
                                    {
                                        case GEDtlAttr.FID:
                                        case GEDtlAttr.OID:
                                        case GEDtlAttr.Rec:
                                        case GEDtlAttr.RefPK:
                                            continue;
                                        default:
                                            break;
                                    }

                                    if (myDT.Columns.Contains(attr.Field) == true)
                                        mywk.SetValByKey(attr.Key, myDT.Rows[0][attr.Field]);
                                }
                            }
                        }

                        //ps = new Paras();
                        //ps.SQL = "SELECT *　from where fk";
                        //DataTable dtMy = DBAccess.RunSQLReturnTable("");

                        isHaveEmp = true;
                    }
                }
                else
                {
                    // To produce the child thread WorkID.
                    mywk.OID = DBAccess.GenerOID("WorkID");  //BP.DA.DBAccess.GenerOID();
                }
                if (this.HisWork.FID == 0)
                    mywk.FID = this.HisWork.OID;

                mywk.Rec = wl.FK_Emp;
                mywk.Emps = wl.FK_Emp;
                mywk.BeforeSave();

                // Judgment is not MD5 Process ?
                if (this.HisFlow.IsMD5)
                    mywk.SetValByKey("MD5", Glo.GenerMD5(mywk));

                mywk.InsertAsOID(mywk.OID);

                // To the system variable assignment , Returns the object in place after sending .
                workIDs += mywk.OID + ",";

                #region   Copy the attachment information 
                if (athDBs.Count > 0)
                {
                    /*  Description of the current node has an attachment data  */
                    athDBs.Delete(FrmAttachmentDBAttr.FK_MapData, "ND" + toNode.NodeID,
                        FrmAttachmentDBAttr.RefPKVal, mywk.OID);

                    foreach (FrmAttachmentDB athDB in athDBs)
                    {
                        FrmAttachmentDB athDB_N = new FrmAttachmentDB();
                        athDB_N.Copy(athDB);
                        athDB_N.FK_MapData = "ND" + toNode.NodeID;
                        athDB_N.RefPKVal = mywk.OID.ToString();
                        athDB_N.FK_FrmAttachment = athDB_N.FK_FrmAttachment.Replace("ND" + this.HisNode.NodeID,
                          "ND" + toNode.NodeID);

                        if (athDB_N.HisAttachmentUploadType == AttachmentUploadType.Single)
                        {
                            // Note If you are naming a single accessory can not change the primary key , Otherwise, the agreement will lead to the reception to get data errors .
                            athDB_N.MyPK = athDB_N.FK_FrmAttachment + "_" + mywk.OID;
                            try
                            {
                                athDB_N.DirectInsert();
                            }
                            catch
                            {
                                athDB_N.MyPK = BP.DA.DBAccess.GenerGUID();
                                athDB_N.Insert();
                            }
                        }
                        else
                        {
                            try
                            {
                                //  Multiple attachments is : FK_MapData+ Way serial number ,  Replace the primary key so that it could save , Not repeat .
                                athDB_N.MyPK = athDB_N.UploadGUID + "_" + athDB_N.FK_MapData + "_" + athDB_N.RefPKVal;
                                athDB_N.DirectInsert();
                            }
                            catch
                            {
                                athDB_N.MyPK = BP.DA.DBAccess.GenerGUID();
                                athDB_N.Insert();
                            }
                        }
                    }
                }
                #endregion   Copy the attachment information 

                #region   Copy the signature information 
                if (eleDBs.Count > 0)
                {
                    /*  Description of the current node has an attachment data  */
                    eleDBs.Delete(FrmEleDBAttr.FK_MapData, "ND" + toNode.NodeID,
                        FrmEleDBAttr.RefPKVal, mywk.OID);
                    int i = 0;
                    foreach (FrmEleDB eleDB in eleDBs)
                    {
                        i++;
                        FrmEleDB athDB_N = new FrmEleDB();
                        athDB_N.Copy(eleDB);
                        athDB_N.FK_MapData = "ND" + toNode.NodeID;
                        athDB_N.RefPKVal = mywk.OID.ToString();
                        athDB_N.GenerPKVal();
                        athDB_N.DirectInsert();
                    }
                }
                #endregion   Copy the attachment information 

                #region   Copy the information from the table .
                if (dtlsFrom.Count > 0)
                {
                    int i = -1;
                    foreach (Sys.MapDtl dtl in dtlsFrom)
                    {
                        i++;
                        if (dtlsTo.Count <= i)
                            continue;
                        Sys.MapDtl toDtl = (Sys.MapDtl)dtlsTo[i];
                        if (toDtl.IsCopyNDData == false)
                            continue;

                        if (toDtl.PTable == dtl.PTable)
                            continue;

                        // Get detailed data .
                        GEDtls gedtls = null;
                        if (dtl.HisGEDtls_temp == null)
                        {
                            gedtls = new GEDtls(dtl.No);
                            QueryObject qo = null;
                            qo = new QueryObject(gedtls);
                            switch (dtl.DtlOpenType)
                            {
                                case DtlOpenType.ForEmp:
                                    qo.AddWhere(GEDtlAttr.RefPK, this.WorkID);
                                    break;
                                case DtlOpenType.ForWorkID:
                                    qo.AddWhere(GEDtlAttr.RefPK, this.WorkID);
                                    break;
                                case DtlOpenType.ForFID:
                                    qo.AddWhere(GEDtlAttr.FID, this.WorkID);
                                    break;
                            }
                            qo.DoQuery();
                            dtl.HisGEDtls_temp = gedtls;
                        }
                        gedtls = dtl.HisGEDtls_temp;

                        int unPass = 0;
                        DBAccess.RunSQL("DELETE FROM " + toDtl.PTable + " WHERE RefPK=" + dbStr + "RefPK", "RefPK", mywk.OID);
                        foreach (GEDtl gedtl in gedtls)
                        {
                            BP.Sys.GEDtl dtCopy = new GEDtl(toDtl.No);
                            dtCopy.Copy(gedtl);
                            dtCopy.FK_MapDtl = toDtl.No;
                            dtCopy.RefPK = mywk.OID.ToString();
                            dtCopy.OID = 0;
                            dtCopy.Insert();

                            #region   Article copied from a form  -  Additional information  - M2M- M2MM
                            if (toDtl.IsEnableAthM)
                            {
                                /* If multiple attachments enabled , Additional information on the copy of this detail data .*/
                                athDBs = new FrmAttachmentDBs(dtl.No, gedtl.OID.ToString());
                                if (athDBs.Count > 0)
                                {
                                    i = 0;
                                    foreach (FrmAttachmentDB athDB in athDBs)
                                    {
                                        i++;
                                        FrmAttachmentDB athDB_N = new FrmAttachmentDB();
                                        athDB_N.Copy(athDB);
                                        athDB_N.FK_MapData = toDtl.No;
                                        athDB_N.MyPK = toDtl.No + "_" + dtCopy.OID + "_" + i.ToString();
                                        athDB_N.FK_FrmAttachment = athDB_N.FK_FrmAttachment.Replace("ND" + this.HisNode.NodeID,
                                            "ND" + toNode.NodeID);
                                        athDB_N.RefPKVal = dtCopy.OID.ToString();
                                        athDB_N.DirectInsert();
                                    }
                                }
                            }
                            if (toDtl.IsEnableM2M || toDtl.IsEnableM2MM)
                            {
                                /* If you enable m2m */
                                M2Ms m2ms = new M2Ms(dtl.No, gedtl.OID);
                                if (m2ms.Count > 0)
                                {
                                    i = 0;
                                    foreach (M2M m2m in m2ms)
                                    {
                                        i++;
                                        M2M m2m_N = new M2M();
                                        m2m_N.Copy(m2m);
                                        m2m_N.FK_MapData = toDtl.No;
                                        m2m_N.MyPK = toDtl.No + "_" + m2m.M2MNo + "_" + gedtl.ToString() + "_" + m2m.DtlObj;
                                        m2m_N.EnOID = gedtl.OID;
                                        m2m_N.InitMyPK();
                                        m2m_N.DirectInsert();
                                    }
                                }
                            }
                            #endregion   Article copied from a form  -  Additional information 

                        }
                    }
                }
                #endregion   Copy the attachment information 

                //  Information produced work .
                GenerWorkFlow gwf = new GenerWorkFlow();
                gwf.WorkID = mywk.OID;
                if (gwf.RetrieveFromDBSources() == 0)
                {
                    gwf.FID = this.WorkID;
                    gwf.FK_Node = toNode.NodeID;

                    if (this.HisNode.IsStartNode)
                        gwf.Title = WorkNode.GenerTitle(this.HisFlow, this.HisWork) + "(" + wl.FK_EmpText + ")";
                    else
                        gwf.Title = this.HisGenerWorkFlow.Title + "(" + wl.FK_EmpText + ")";

                    gwf.WFState = WFState.Runing;
                    gwf.RDT = DataType.CurrentDataTime;
                    gwf.Starter = this.Execer;
                    gwf.StarterName = this.ExecerName;
                    gwf.FK_Flow = toNode.FK_Flow;
                    gwf.FlowName = toNode.FlowName;
                    gwf.FID = this.WorkID;
                    gwf.FK_FlowSort = toNode.HisFlow.FK_FlowSort;
                    gwf.NodeName = toNode.Name;
                    gwf.FK_Dept = wl.FK_Dept;
                    gwf.DeptName = wl.FK_DeptT;
                    gwf.DirectInsert();
                }
                else
                {
                    gwf.FK_Node = toNode.NodeID;
                    gwf.NodeName = toNode.Name;
                    gwf.Update();
                }


                //  The temporary workid  Updates to 
                ps = new Paras();
                ps.SQL = "UPDATE WF_GenerWorkerlist SET WorkID=" + dbStr + "WorkID1 WHERE WorkID=" + dbStr + "WorkID2";
                ps.Add("WorkID1", mywk.OID);
                ps.Add("WorkID2", wl.WorkID); // Occasional ID
                int num = DBAccess.RunSQL(ps);
                if (num == 0)
                    throw new Exception("@ No less than it should be updated .");
            }

            #endregion  Replicate data .

            #region  Processing the message prompt 
            string info = "@ Shunt node :{0} Has launched ,@ Tasks are automatically sent to the following (" + this.HisRememberMe.NumOfObjs + ") Treatments people {1}.";
            this.addMsg("FenLiuInfo",
                string.Format("<span id='workSendResult' toNode='{0}' EmpsExt='{1}' >" + info + "</span>", toNode.Name, this.HisRememberMe.EmpsExt));


            // Handle thread  WorkIDs  Added to the system variable .
            this.addMsg(SendReturnMsgFlag.VarTreadWorkIDs, workIDs, workIDs, SendReturnMsgType.SystemMsg);

            //  If this is the start node , You can choose to accept people allowed .
            if (this.HisNode.IsStartNode)
            {
                if (gwls.Count >= 2 && this.HisNode.IsTask)
                    this.addMsg("AllotTask", "@<img src='" + VirPath + "WF/Img/AllotTask.gif' border=0 /><a id='linkAllotTask' href=\"javascript:WinOpen('" + VirPath + "WF/WorkOpt/AllotTask.aspx?WorkID=" + this.WorkID + "&FID=" + this.WorkID + "&NodeID=" + toNode.NodeID + "')\" > Modify accept object </a>.");
            }

            if (this.HisNode.IsStartNode)
            {
                if (WebUser.IsWap)
                    this.addMsg("UnDoNew", "@<a id='linkUnDo' href='" + VirPath + "WF/Wap/MyFlowInfo" + Glo.FromPageType + ".aspx?DoType=UnSend&WorkID=" + this.WorkID + "&FK_Flow=" + toNode.FK_Flow + "'><img src='" + VirPath + "WF/Img/UnDo.gif' border=0/> Revocation of this transmission </a>, <a id='linkNewProcess' href='" + VirPath + "WF/Wap/MyFlow" + Glo.FromPageType + ".aspx?FK_Flow=" + toNode.FK_Flow + "&FK_Node=" + toNode.FK_Flow + "01' ><img id='linkNewProcess' src='" + VirPath + "WF/Img/New.gif' border=0/> New Process </a>.");
                else
                    this.addMsg("UnDoNew", "@<a id='linkUnDo' href='" + this.VirPath + this.AppType + "/MyFlowInfo" + Glo.FromPageType + ".aspx?DoType=UnSend&WorkID=" + this.WorkID + "&FK_Flow=" + toNode.FK_Flow + "'><img src='" + VirPath + "WF/Img/UnDo.gif' border=0/> Revocation of this transmission </a>, <a id='linkNewProcess' href='" + this.VirPath + this.AppType + "/MyFlow" + Glo.FromPageType + ".aspx?FK_Flow=" + toNode.FK_Flow + "&FK_Node=" + toNode.FK_Flow + "01' ><img id='linkNewProcess' src='" + VirPath + "WF/Img/New.gif' border=0/> New Process </a>.");
            }
            else
            {
                this.addMsg("UnDo", "@<a id='linkUnDo' href='" + this.VirPath + this.AppType + "/MyFlowInfo" + Glo.FromPageType + ".aspx?DoType=UnSend&WorkID=" + this.WorkID + "&FK_Flow=" + toNode.FK_Flow + "'><img src='" + VirPath + "WF/Img/UnDo.gif' border=0/> Revocation of this transmission </a>.");
            }

            this.addMsg("Rpt", "@<a id='linkRpt' href='" + VirPath + "WF/WFRpt.aspx?WorkID=" + this.WorkID + "&FID=" + wk.FID + "&FK_Flow=" + this.HisNode.FK_Flow + "'target='_blank' > Work trajectory </a>");
            #endregion  Processing the message prompt 
        }
        /// <summary>
        ///  Send to a common point of confluence 
        /// 1.  First check completion rate .
        /// 2,  Send to a common node in the normal node .
        /// </summary>
        /// <returns></returns>
        private void NodeSend_31(Node nd)
        {
            // Check the completion rate .

            // 与1-1 The same logic processing .
            this.NodeSend_11(nd);
        }
        /// <summary>
        ///  Child thread sent down 
        /// </summary>
        /// <returns></returns>
        private string NodeSend_4x()
        {
            return null;
        }
        /// <summary>
        ///  Child thread to the confluence 
        /// </summary>
        /// <returns></returns>
        private void NodeSend_53_SameSheet_To_HeLiu(Node toNode)
        {
            Work toNodeWK = toNode.HisWork;
            toNodeWK.Copy(this.HisWork);
            toNodeWK.OID = this.HisWork.FID;
            toNodeWK.FID = 0;
            this.town = new WorkNode(toNodeWK, toNode);

            //  Get reach the current confluence node   Set with a sub-thread split point between nodes .
            string spanNodes = this.SpanSubTheadNodes(toNode);

            #region FID
#warning lost FID.

            Int64 fid = this.HisWork.FID;
            if (fid == 0)
            {
                if (this.HisNode.HisRunModel != RunModel.SubThread)
                    throw new Exception("@ Non-child node of the current node thread .");

                string strs = BP.DA.DBAccess.RunSQLReturnStringIsNull("SELECT FID FROM WF_GenerWorkFlow WHERE WorkID=" + this.HisWork.OID, "0");
                if (strs == "0")
                    throw new Exception("@ Lose FID Information ");
                fid = Int64.Parse(strs);

                this.HisWork.FID = fid;
            }
            #endregion FID

            GenerFH myfh = new GenerFH(fid);
            if (myfh.FK_Node == toNode.NodeID)
            {
                /*  Description is not the first to come up this node , 
                 *  Such as : A process :
                 * A Bypass -> B General -> C Confluence 
                 * 从B 到C 中, B There N  Threads , Before there is already a thread reaches over C.
                 */

                /* 
                 *  First of all : Update its node  worklist  Information ,  Description of the current node has been completed .
                 *  Let the current operator can see their work .
                 */

                ps = new Paras();
                ps.SQL = "UPDATE WF_GenerWorkerlist SET IsPass=1  WHERE WorkID=" + dbStr + "WorkID AND FID=" + dbStr + "FID AND FK_Node=" + dbStr + "FK_Node";
                ps.Add("WorkID", this.WorkID);
                ps.Add("FID", this.HisWork.FID);
                ps.Add("FK_Node", this.HisNode.NodeID);
                DBAccess.RunSQL(ps);


                this.HisGenerWorkFlow.FK_Node = toNode.NodeID;
                this.HisGenerWorkFlow.NodeName = toNode.Name;

                ps = new Paras();
                ps.SQL = "UPDATE WF_GenerWorkFlow  SET  WFState=" + (int)WFState.Runing + ", FK_Node=" + dbStr + "FK_Node,NodeName=" + dbStr + "NodeName WHERE WorkID=" + dbStr + "WorkID";
                ps.Add("FK_Node", toNode.NodeID);
                ps.Add("NodeName", toNode.Name);
                ps.Add("WorkID", this.HisWork.OID);
                DBAccess.RunSQL(ps);

                /*
                 *  Then update the current status of the nodes and the completion time .
                 */
                this.HisWork.Update(WorkAttr.CDT, BP.DA.DataType.CurrentDataTime);

                #region  Treatment completion rate 

                ps = new Paras();
                ps.SQL = "SELECT FK_Emp,FK_EmpText FROM WF_GenerWorkerList WHERE FK_Node=" + dbStr + "FK_Node AND FID=" + dbStr + "FID AND IsPass=1";
                ps.Add("FK_Node", this.HisNode.NodeID);
                ps.Add("FID", this.HisWork.FID);
                DataTable dt_worker = BP.DA.DBAccess.RunSQLReturnTable(ps);
                string numStr = "@ Triage officer has completed the implementation of the following :";
                foreach (DataRow dr in dt_worker.Rows)
                    numStr += "@" + dr[0] + "," + dr[1];

                // Determine the number of child threads .
                ps = new Paras();
                ps.SQL = "SELECT DISTINCT(WorkID) FROM WF_GenerWorkerList WHERE FK_Node=" + dbStr + "FK_Node AND FID=" + dbStr + "FID AND IsPass=1";
                ps.Add("FK_Node", this.HisNode.NodeID);
                ps.Add("FID", this.HisWork.FID);
                DataTable dt_thread = BP.DA.DBAccess.RunSQLReturnTable(ps);
                decimal ok = (decimal)dt_thread.Rows.Count;

                ps = new Paras();
                ps.SQL = "SELECT  COUNT(distinct WorkID) AS Num FROM WF_GenerWorkerList WHERE   IsEnable=1 AND FID=" + dbStr + "FID AND FK_Node IN (" + spanNodes + ")";
                ps.Add("FID", this.HisWork.FID);
                decimal all = (decimal)DBAccess.RunSQLReturnValInt(ps);
                if (all == 0)
                    throw new Exception("@ Get the total number of sub-thread error , Number of threads 0, Execution sql:" + ps.SQL + " FID=" + this.HisWork.FID);

                decimal passRate = ok / all * 100;
                numStr = "@ You are (" + ok + ") Reach deal with people on this node , Total start-up (" + all + ") Sub-processes .";
                if (toNode.PassRate <= passRate)
                {
                    /* Description All the staff have completed , Let confluence display it .*/
                    DBAccess.RunSQL("UPDATE WF_GenerWorkerList SET IsPass=0  WHERE FK_Node=" + dbStr + "FK_Node AND WorkID=" + dbStr + "WorkID",
                        "FK_Node", toNode.NodeID, "WorkID", this.HisWork.FID);
                    numStr += "@ The next step (" + toNode.Name + ") Has started .";
                }
                #endregion  Treatment completion rate 


                if (myfh.ToEmpsMsg.Contains("("))
                {
                    string FK_Emp1 = myfh.ToEmpsMsg.Substring(0, myfh.ToEmpsMsg.LastIndexOf('('));
                    this.AddToTrack(ActionType.ForwardHL, FK_Emp1, myfh.ToEmpsMsg, toNode.NodeID, toNode.Name, null);

                    // Increased variable .
                    this.addMsg(SendReturnMsgFlag.VarAcceptersID, FK_Emp1, SendReturnMsgType.SystemMsg);
                    this.addMsg(SendReturnMsgFlag.VarAcceptersName, FK_Emp1, SendReturnMsgType.SystemMsg);
                }

                //  Generated from the confluence of the summary table data .
                this.GenerHieLiuHuiZhongDtlData_2013(toNode);

                this.addMsg("ToHeLiuEmp",
                    "@ The process has been running to the confluence of the node [" + toNode.Name + "].@ Your work has been sent to the following persons [" + myfh.ToEmpsMsg + "],<a href=\"javascript:WinOpen('./Msg/SMS.aspx?WorkID=" + this.WorkID + "&FK_Node=" + toNode.NodeID + "')\" > SMS informing them </a>." + this.GenerWhySendToThem(this.HisNode.NodeID, toNode.NodeID) + numStr);
            }
            else
            {
                /*  Have FID, Explanation : Previously have been split or merge node .*/
                /*
                 *  The following process is no process to reach this location 
                 *  Description is the first to come up this node .
                 *  Such as : A process :
                 * A Bypass -> B General -> C Confluence 
                 * 从B 到C 中, B There N  Threads , Before he was the first to arrive C.
                 */

                //  First test of their staff ．
                GenerWorkerLists gwls = this.Func_GenerWorkerLists(this.town);

                string FK_Emp = "";
                string toEmpsStr = "";
                string emps = "";
                foreach (GenerWorkerList wl in gwls)
                {
                    toEmpsStr += BP.WF.Glo.DealUserInfoShowModel(wl.FK_Emp, wl.FK_EmpText);

                    if (gwls.Count == 1)
                        emps = wl.FK_Emp;
                    else
                        emps += "@" + FK_Emp;
                }
                // Increased variable .
                this.addMsg(SendReturnMsgFlag.VarAcceptersID, emps.Replace("@", ","), SendReturnMsgType.SystemMsg);
                this.addMsg(SendReturnMsgFlag.VarAcceptersName, toEmpsStr, SendReturnMsgType.SystemMsg);

                /* 
                *  Update its node  worklist  Information ,  Description of the current node has been completed .
                *  Let the current operator can see their work .
                */

                #region  Set the parent process status   Set the current node :
                myfh.Update(GenerFHAttr.FK_Node, toNode.NodeID,
                    GenerFHAttr.ToEmpsMsg, toEmpsStr);

                Work mainWK = town.HisWork;
                mainWK.OID = this.HisWork.FID;
                mainWK.RetrieveFromDBSources();


                //  Copy the report to the confluence of the above data point up .
                DataTable dt = DBAccess.RunSQLReturnTable("SELECT * FROM " + this.HisFlow.PTable + " WHERE OID=" + dbStr + "OID",
                    "OID", this.HisWork.FID);
                foreach (DataColumn dc in dt.Columns)
                    mainWK.SetValByKey(dc.ColumnName, dt.Rows[0][dc.ColumnName]);

                mainWK.Rec = FK_Emp;
                mainWK.Emps = emps;
                mainWK.OID = this.HisWork.FID;
                mainWK.Save();

                //  Generated from the confluence of the summary table data .
                this.GenerHieLiuHuiZhongDtlData_2013(toNode);

                /* Copy processing form data .*/
                #region  Copy the attachment .
                FrmAttachmentDBs athDBs = new FrmAttachmentDBs("ND" + this.HisNode.NodeID,
                      this.WorkID.ToString());
                if (athDBs.Count > 0)
                {
                    /* Description of the current node has an attachment data */
                    foreach (FrmAttachmentDB athDB in athDBs)
                    {
                        FrmAttachmentDB athDB_N = new FrmAttachmentDB();
                        athDB_N.Copy(athDB);
                        athDB_N.FK_MapData = "ND" + toNode.NodeID;
                        athDB_N.RefPKVal = this.HisWork.FID.ToString();
                        athDB_N.FK_FrmAttachment = athDB_N.FK_FrmAttachment.Replace("ND" + this.HisNode.NodeID,
                          "ND" + toNode.NodeID);

                        if (athDB_N.HisAttachmentUploadType == AttachmentUploadType.Single)
                        {
                            /* If a single attachment .*/
                            athDB_N.MyPK = athDB_N.FK_FrmAttachment + "_" + this.HisWork.FID;
                            if (athDB_N.IsExits == true)
                                continue; /* Instructions on a node or sub-thread already copy After a ,  But there is a child thread to pass data to the confluence point may be , It can not be used break.*/
                            athDB_N.Insert();
                        }
                        else
                        {
                            // This judgment guid  The file is uploaded by other threads copy Later ?
                            if (athDB_N.IsExit(FrmAttachmentDBAttr.UploadGUID, athDB_N.UploadGUID,
                                FrmAttachmentDBAttr.FK_MapData, athDB_N.FK_MapData) == true)
                                continue; /* If you do not copy.*/

                            athDB_N.MyPK = athDB_N.UploadGUID + "_" + athDB_N.FK_MapData;
                            athDB_N.Insert();
                        }
                    }
                }
                #endregion  Copy the attachment .

                #region  Copy Ele.
                FrmEleDBs eleDBs = new FrmEleDBs("ND" + this.HisNode.NodeID,
                      this.WorkID.ToString());
                if (eleDBs.Count > 0)
                {
                    /* Description of the current node has an attachment data */
                    int idx = 0;
                    foreach (FrmEleDB eleDB in eleDBs)
                    {
                        idx++;
                        FrmEleDB eleDB_N = new FrmEleDB();
                        eleDB_N.Copy(eleDB);
                        eleDB_N.FK_MapData = "ND" + toNode.NodeID;
                        eleDB_N.MyPK = eleDB_N.MyPK.Replace("ND" + this.HisNode.NodeID, "ND" + toNode.NodeID);
                        eleDB_N.RefPKVal = this.HisWork.FID.ToString();
                        eleDB_N.Save();
                    }
                }
                #endregion  Copy the attachment .

                /*  Confluence need to wait for each split point has been dealt with in order to see it .*/
                string sql1 = "";
                // "SELECT COUNT(*) AS Num FROM WF_GenerWorkerList WHERE FK_Node=" + this.HisNode.NodeID + " AND FID=" + this.HisWork.FID;
                // string sql1 = "SELECT COUNT(*) AS Num FROM WF_GenerWorkerList WHERE  IsPass=0 AND FID=" + this.HisWork.FID;

#warning  For multiple sub-confluent point may be a problem .
                sql1 = "SELECT COUNT(distinct WorkID) AS Num FROM WF_GenerWorkerList WHERE  FID=" + this.HisWork.FID + " AND FK_Node IN (" + spanNodes + ")";
                decimal numAll1 = (decimal)DBAccess.RunSQLReturnValInt(sql1);
                decimal passRate1 = 1 / numAll1 * 100;
                if (toNode.PassRate <= passRate1)
                {
                    /* At this time has passed , Lets see the main thread Upcoming . */
                    ps = new Paras();
                    ps.SQL = "UPDATE WF_GenerWorkerList SET IsPass=0 WHERE FK_Node=" + dbStr + "FK_Node AND WorkID=" + dbStr + "WorkID";
                    ps.Add("FK_Node", toNode.NodeID);
                    ps.Add("WorkID", this.HisWork.FID);
                    int num = DBAccess.RunSQL(ps);
                    if (num == 0)
                        throw new Exception("@ No less than it should be updated .");
                }
                else
                {
#warning  To keep it displayed in the way of work required , =3  Is not a normal processing mode .
                    ps = new Paras();
                    ps.SQL = "UPDATE WF_GenerWorkerList SET IsPass=3 WHERE FK_Node=" + dbStr + "FK_Node AND WorkID=" + dbStr + "WorkID";
                    ps.Add("FK_Node", toNode.NodeID);
                    ps.Add("WorkID", this.HisWork.FID);
                    int num = DBAccess.RunSQL(ps);
                    if (num == 0)
                        throw new Exception("@ No less than it should be updated .");
                }

                this.HisGenerWorkFlow.FK_Node = toNode.NodeID;
                this.HisGenerWorkFlow.NodeName = toNode.Name;

                // Change the current process of the current node .
                ps = new Paras();
                ps.SQL = "UPDATE WF_GenerWorkFlow SET WFState=" + (int)WFState.Runing + ",  FK_Node=" + dbStr + "FK_Node,NodeName=" + dbStr + "NodeName WHERE WorkID=" + dbStr + "WorkID";
                ps.Add("FK_Node", toNode.NodeID);
                ps.Add("NodeName", toNode.Name);
                ps.Add("WorkID", this.HisWork.FID);
                DBAccess.RunSQL(ps);

                // Set the current sub-thread has passed .
                ps = new Paras();
                ps.SQL = "UPDATE WF_GenerWorkerlist SET IsPass=1  WHERE WorkID=" + dbStr + "WorkID AND FID=" + dbStr + "FID";
                ps.Add("WorkID", this.WorkID);
                ps.Add("FID", this.HisWork.FID);
                DBAccess.RunSQL(ps);
                #endregion  Set the parent process status 

                this.addMsg("InfoToHeLiu", "@ The process has been running to the confluence of the node [" + toNode.Name + "].@ Your work has been sent to the following persons [" + toEmpsStr + "],<a href=\"javascript:WinOpen('" + VirPath + "WF/Msg/SMS.aspx?WorkID=" + this.WorkID + "&FK_Node=" + toNode.NodeID + "')\" > SMS informing them </a>.@ You are the first person to reach this node processing .");
            }
        }
        private string NodeSend_55(Node toNode)
        {
            return null;
        }
        /// <summary>
        ///  Node downward movement 
        /// </summary>
        private void NodeSend_Send_5_5()
        {
            // Set the current time to complete the implementation of staff . for: anhua 2013-12-18.
            string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
            Paras ps = new Paras();
            ps.SQL = "UPDATE WF_GenerWorkerlist SET CDT=" + dbstr + "CDT WHERE WorkID=" + dbstr + "WorkID AND FK_Node=" + dbstr + "FK_Node AND FK_Emp=" + dbstr + "FK_Emp";
            ps.Add(GenerWorkerListAttr.CDT, DataType.CurrentDataTimess);
            ps.Add(GenerWorkerListAttr.WorkID, this.WorkID);
            ps.Add(GenerWorkerListAttr.FK_Node, this.HisNode.NodeID);
            ps.Add(GenerWorkerListAttr.FK_Emp, this.Execer);
            BP.DA.DBAccess.RunSQL(ps);

            #region  Check whether the current state is returned , If it is returned to the state , Give him an assignment .
            //  Check whether the current state is returned ,.
            if (this.SendNodeWFState == WFState.ReturnSta)
            {
                /* Check whether the return is backtrack ?*/
                ps = new Paras();
                ps.SQL = "SELECT ReturnNode,Returner,IsBackTracking FROM WF_ReturnWork WHERE WorkID=" + dbStr + "WorkID AND IsBackTracking=1 ORDER BY RDT DESC";
                ps.Add(ReturnWorkAttr.WorkID, this.WorkID);
                DataTable dt = DBAccess.RunSQLReturnTable(ps);
                if (dt.Rows.Count != 0)
                {
                    // Check out the multiple possible , Because sorted by time , Remove only the last returned , Return and see if there is information to backtrack .

                    /* Confirm the return , Is returned and backtrack  ,   Here initialize its staff ,  With the node that will be sent . */
                    this.JumpToNode = new Node(int.Parse(dt.Rows[0]["ReturnNode"].ToString()));
                    this.JumpToEmp = dt.Rows[0]["Returner"].ToString();
                    this.IsSkip = true; // If not set true,  Will delete the target data .
                    //  this.NodeSend_11(this.JumpToNode);
                }
            }
            #endregion.

            switch (this.HisNode.HisRunModel)
            {
                case RunModel.Ordinary: /* 1:  Common node sends down */
                    Node toND = this.NodeSend_GenerNextStepNode();
                    if (this.IsStopFlow)
                        return;
                    switch (toND.HisRunModel)
                    {
                        case RunModel.Ordinary:   /*1-1  Ordinary Day to Ordinary nodes  */
                            this.NodeSend_11(toND);
                            break;
                        case RunModel.FL:  /* 1-2  Ordinary Day to Split point  */
                            this.NodeSend_11(toND);
                            break;
                        case RunModel.HL:  /*1-3  Ordinary Day to Confluence    */
                            this.NodeSend_11(toND);
                            // throw new Exception("@ Process design errors : Please check the flow for more information ,  Ordinary nodes below the confluence of the nodes can not be connected (" + toND.Name + ").");
                            break;
                        case RunModel.FHL: /*1-4  Ordinary nodes to Confluence points  */
                            this.NodeSend_11(toND);
                            break;
                        // throw new Exception("@ Process design errors : Please check the flow for more information ,  Common node points below the confluence of the nodes can not be connected (" + toND.Name + ").");
                        case RunModel.SubThread: /*1-5  Ordinary Day to Child thread point  */
                            throw new Exception("@ Process design errors : Please check the flow for more information ,  Ordinary nodes below the node can not connect the child thread (" + toND.Name + ").");
                        default:
                            throw new Exception("@ Node type is not judged (" + toND.Name + ")");
                            break;
                    }
                    break;
                case RunModel.FL: /* 2:  Shunt node sends down */
                    Nodes toNDs = this.Func_GenerNextStepNodes();
                    if (toNDs.Count == 1)
                    {
                        Node toND2 = toNDs[0] as Node;
                        // Added to the system variable .
                        this.addMsg(SendReturnMsgFlag.VarToNodeID, toND2.NodeID.ToString(), toND2.NodeID.ToString(), SendReturnMsgType.SystemMsg);
                        this.addMsg(SendReturnMsgFlag.VarToNodeName, toND2.Name, toND2.Name, SendReturnMsgType.SystemMsg);

                        switch (toND2.HisRunModel)
                        {
                            case RunModel.Ordinary:    /*2.1  Split point to Ordinary nodes  */
                                this.NodeSend_11(toND2); /*  By ordinary nodes to ordinary node processing . */
                                break;
                            case RunModel.FL:  /*2.2  Split point to Split point   */
                            //  throw new Exception("@ Process design errors : Please check the flow for more information ,  Split point (" + this.HisNode.Name + ") The following can not be connected shunt node (" + toND2.Name + ").");
                            case RunModel.HL:  /*2.3  Split point to Confluence , Confluence points . */
                            case RunModel.FHL:
                                this.NodeSend_11(toND2); /*  By ordinary nodes to ordinary node processing . */
                                break;
                            // throw new Exception("@ Process design errors : Please check the flow for more information ,  Split point (" + this.HisNode.Name + ") Below the confluence of the nodes can not be connected (" + toND2.Name + ").");
                            case RunModel.SubThread: /* 2.4  Split point to Child thread point    */
                                if (toND2.HisSubThreadType == SubThreadType.SameSheet)
                                    NodeSend_24_SameSheet(toND2);
                                else
                                    NodeSend_24_UnSameSheet(toNDs); /* May be sent only 1 A different form */
                                break;
                            default:
                                throw new Exception("@ Node type is not judged (" + toND2.Name + ")");
                                break;
                        }
                    }
                    else
                    {
                        /*  If there are multiple nodes , Check their child thread node must be otherwise , That design errors .*/
                        bool isHaveSameSheet = false;
                        bool isHaveUnSameSheet = false;
                        foreach (Node nd in toNDs)
                        {
                            switch (nd.HisRunModel)
                            {
                                case RunModel.Ordinary:
                                    NodeSend_11(nd); /* By ordinary nodes to ordinary node processing .*/
                                    break;
                                case RunModel.FL:
                                case RunModel.FHL:
                                case RunModel.HL:
                                    NodeSend_11(nd); /* By ordinary nodes to ordinary node processing .*/
                                    break;
                                //    throw new Exception("@ Process design errors : Please check the flow for more information ,  Split point (" + this.HisNode.Name + ") The following can not be connected shunt node (" + nd.Name + ").");
                                //case RunModel.FHL:
                                //    throw new Exception("@ Process design errors : Please check the flow for more information ,  Split point (" + this.HisNode.Name + ") The following sub-confluent nodes can not be connected (" + nd.Name + ").");
                                //case RunModel.HL:
                                //    throw new Exception("@ Process design errors : Please check the flow for more information ,  Split point (" + this.HisNode.Name + ") Below the confluence of the nodes can not be connected (" + nd.Name + ").");
                                default:
                                    break;
                            }
                            if (nd.HisSubThreadType == SubThreadType.SameSheet)
                                isHaveSameSheet = true;

                            if (nd.HisSubThreadType == SubThreadType.UnSameSheet)
                                isHaveUnSameSheet = true;
                        }

                        if (isHaveUnSameSheet && isHaveSameSheet)
                            throw new Exception("@ Process models are not supported :  Shunt nodes simultaneously with the start of the sub-thread forms with different forms of child thread .");

                        if (isHaveSameSheet == true)
                            throw new Exception("@ Process models are not supported :  Shunt nodes simultaneously launched with multiple sub-thread forms .");

                        // Start several different forms sub-thread node .
                        this.NodeSend_24_UnSameSheet(toNDs);
                    }
                    break;
                case RunModel.HL:  /* 3:  Confluence node sends down  */
                    Node toND3 = this.NodeSend_GenerNextStepNode();
                    if (this.IsStopFlow)
                        return;

                    // Added to the system variable .
                    this.addMsg(SendReturnMsgFlag.VarToNodeID, toND3.NodeID.ToString(), toND3.NodeID.ToString(), SendReturnMsgType.SystemMsg);
                    this.addMsg(SendReturnMsgFlag.VarToNodeName, toND3.Name, toND3.Name, SendReturnMsgType.SystemMsg);

                    switch (toND3.HisRunModel)
                    {
                        case RunModel.Ordinary: /*3.1  Ordinary working nodes  */
                            this.NodeSend_31(toND3); /*  It just like ordinary little common point logic . */
                            break;
                        case RunModel.FL: /*3.2  Split point  */
                            this.NodeSend_31(toND3); /*  It just like ordinary little common point logic . */
                            break;
                        //throw new Exception("@ Process design errors : Please check the flow for more information ,  Confluence (" + this.HisNode.Name + ") The following can not be connected shunt node (" + toND3.Name + ").");
                        case RunModel.HL: /*3.3  Confluence  */
                        case RunModel.FHL:
                            this.NodeSend_31(toND3); /*  It just like ordinary little common point logic . */
                            break;
                        //throw new Exception("@ Process design errors : Please check the flow for more information ,  Confluence (" + this.HisNode.Name + ") Below the confluence of the nodes can not be connected (" + toND3.Name + ").");
                        case RunModel.SubThread:/*3.4  Child thread */
                            throw new Exception("@ Process design errors : Please check the flow for more information ,  Confluence (" + this.HisNode.Name + ") The following sub-thread can not be connected nodes (" + toND3.Name + ").");
                        default:
                            throw new Exception("@ Node type is not judged (" + toND3.Name + ")");
                    }
                    break;
                case RunModel.FHL:  /* 4:  Shunt node sends down  */
                    Node toND4 = this.NodeSend_GenerNextStepNode();
                    if (this.IsStopFlow)
                        return;

                    // Added to the system variable .
                    this.addMsg(SendReturnMsgFlag.VarToNodeID, toND4.NodeID.ToString(), toND4.NodeID.ToString(), SendReturnMsgType.SystemMsg);
                    this.addMsg(SendReturnMsgFlag.VarToNodeName, toND4.Name, toND4.Name, SendReturnMsgType.SystemMsg);

                    switch (toND4.HisRunModel)
                    {
                        case RunModel.Ordinary: /*4.1  Ordinary working nodes  */
                            this.NodeSend_11(toND4); /*  It just like ordinary little common point logic . */
                            break;
                        case RunModel.FL: /*4.2  Split point  */
                            throw new Exception("@ Process design errors : Please check the flow for more information ,  Confluence (" + this.HisNode.Name + ") The following can not be connected shunt node (" + toND4.Name + ").");
                        case RunModel.HL: /*4.3  Confluence  */
                        case RunModel.FHL:
                            this.NodeSend_11(toND4); /*  It just like ordinary little common point logic . */
                            break;
                        //  throw new Exception("@ Process design errors : Please check the flow for more information ,  Confluence (" + this.HisNode.Name + ") Below the confluence of the nodes can not be connected (" + toND4.Name + ").");
                        case RunModel.SubThread:/*4.5  Child thread */
                            if (toND4.HisSubThreadType == SubThreadType.SameSheet)
                                NodeSend_24_SameSheet(toND4);
                            //else
                            //    NodeSend_24_UnSameSheet(toNDs); /* May be sent only 1 A different form */
                            break;
                        //throw new Exception("@ Process design errors : Please check the flow for more information ,  Confluence (" + this.HisNode.Name + ") The following sub-thread can not be connected nodes (" + toND4.Name + ").");
                        default:
                            throw new Exception("@ Node type is not judged (" + toND4.Name + ")");
                    }
                    break;
                // throw new Exception("@ Does not determine the type of :" + this.HisNode.HisNodeWorkTypeT);
                case RunModel.SubThread:  /* 5:  Child thread node sends down  */
                    Node toND5 = this.NodeSend_GenerNextStepNode();
                    if (this.IsStopFlow)
                        return;

                    // Added to the system variable .
                    this.addMsg(SendReturnMsgFlag.VarToNodeID, toND5.NodeID.ToString(), toND5.NodeID.ToString(), SendReturnMsgType.SystemMsg);
                    this.addMsg(SendReturnMsgFlag.VarToNodeName, toND5.Name, toND5.Name, SendReturnMsgType.SystemMsg);

                    switch (toND5.HisRunModel)
                    {
                        case RunModel.Ordinary: /*5.1  Ordinary working nodes  */
                            throw new Exception("@ Process design errors : Please check the flow for more information ,  Child thread point (" + this.HisNode.Name + ") The following ordinary nodes can not be connected (" + toND5.Name + ").");
                            break;
                        case RunModel.FL: /*5.2  Split point  */
                            throw new Exception("@ Process design errors : Please check the flow for more information ,  Child thread point (" + this.HisNode.Name + ") The following can not be connected shunt node (" + toND5.Name + ").");
                        case RunModel.HL: /*5.3  Confluence  */
                        case RunModel.FHL: /*5.4  Confluence points  */
                            if (this.HisNode.HisSubThreadType == SubThreadType.SameSheet)
                                this.NodeSend_53_SameSheet_To_HeLiu(toND5);
                            else
                                this.NodeSend_53_UnSameSheet_To_HeLiu(toND5);

                            // The confluence point setting unread .
                            ps = new Paras();
                            ps.SQL = "UPDATE WF_GenerWorkerList SET IsRead=0 WHERE WorkID=" + SystemConfig.AppCenterDBVarStr + "WorkID AND  FK_Node=" + SystemConfig.AppCenterDBVarStr + "FK_Node";
                            ps.Add("WorkID", this.HisWork.FID);
                            ps.Add("FK_Node", toND5.NodeID);
                            BP.DA.DBAccess.RunSQL(ps);
                            break;
                        case RunModel.SubThread: /*5.5  Child thread */
                            if (toND5.HisSubThreadType == this.HisNode.HisSubThreadType)
                            {
                                #region  Remove the child thread to node if there , Return information to prevent garbage problem , If the return does not need to deal with this part of the deal .
                                ps = new Paras();
                                ps.SQL = "DELETE FROM WF_GenerWorkerlist WHERE FID=" + dbStr + "FID  AND FK_Node=" + dbStr + "FK_Node";
                                ps.Add("FID", this.HisWork.FID);
                                ps.Add("FK_Node", toND5.NodeID);
                                #endregion  Remove the child thread to node if there , Return information to prevent garbage problem , If the return does not need to deal with this part of the deal .

                                this.NodeSend_11(toND5); /* Like ordinary node .*/
                            }
                            else
                                throw new Exception("@ Process Design mode error : Two consecutive sub-thread mode is not the same as a child thread , From node (" + this.HisNode.Name + ") To node (" + toND5.Name + ")");
                            break;
                        default:
                            throw new Exception("@ Node type is not judged (" + toND5.Name + ")");
                    }
                    break;
                default:
                    throw new Exception("@ Does not determine the type of :" + this.HisNode.HisNodeWorkTypeT);
            }
        }

        #region  Perform data copy.
        public void CopyData(Work toWK, Node toND, bool isSamePTable)
        {
            string errMsg = " If not want to wait two data sources , Implementation copy -  An error occurred during .";

            #region  Master table data copy.
            if (isSamePTable == false)
            {
                toWK.SetValByKey("OID", this.HisWork.OID); // Setting it ID.
                if (this.HisNode.IsStartNode == false)
                    toWK.Copy(this.rptGe);

                toWK.Copy(this.HisWork); //  Carried out  copy  Data on a node .
                toWK.Rec = this.Execer;

                // To be considered FID The problem .
                if (this.HisNode.HisRunModel == RunModel.SubThread
                    && toND.HisRunModel == RunModel.SubThread)
                    toWK.FID = this.HisWork.FID;

                try
                {
                    // Judgment is not MD5 Process ?
                    if (this.HisFlow.IsMD5)
                        toWK.SetValByKey("MD5", Glo.GenerMD5(toWK));

                    if (toWK.IsExits)
                        toWK.Update();
                    else
                        toWK.Insert();
                }
                catch (Exception ex)
                {
                    toWK.CheckPhysicsTable();
                    try
                    {
                        toWK.Copy(this.HisWork); //  Carried out  copy  Data on a node .
                        toWK.Rec = this.Execer;
                        toWK.SaveAsOID(toWK.OID);
                    }
                    catch (Exception ex11)
                    {
                        if (toWK.Update() == 0)
                            throw new Exception(ex.Message + " == " + ex11.Message);
                    }
                }
            }
            #endregion  Master table data copy.

            #region  Copy the attachment .
            if (this.HisNode.MapData.FrmAttachments.Count > 0)
            {
                FrmAttachmentDBs athDBs = new FrmAttachmentDBs("ND" + this.HisNode.NodeID,
                      this.WorkID.ToString());
                int idx = 0;
                if (athDBs.Count > 0)
                {
                    athDBs.Delete(FrmAttachmentDBAttr.FK_MapData, "ND" + toND.NodeID,
                        FrmAttachmentDBAttr.RefPKVal, this.WorkID);

                    /* Description of the current node has an attachment data */
                    foreach (FrmAttachmentDB athDB in athDBs)
                    {
                        FrmAttachmentDB athDB_N = new FrmAttachmentDB();
                        athDB_N.Copy(athDB);
                        athDB_N.FK_MapData = "ND" + toND.NodeID;
                        athDB_N.RefPKVal = this.HisWork.OID.ToString();
                        athDB_N.FK_FrmAttachment = athDB_N.FK_FrmAttachment.Replace("ND" + this.HisNode.NodeID,
                          "ND" + toND.NodeID);

                        if (athDB_N.HisAttachmentUploadType == AttachmentUploadType.Single)
                        {
                            /* If a single attachment .*/
                            athDB_N.MyPK = athDB_N.FK_FrmAttachment + "_" + this.HisWork.OID;
                            //if (athDB_N.IsExits == true)
                            //    continue; /* Instructions on a node or sub-thread already copy After a ,  But there is a child thread to pass data to the confluence point may be , It can not be used break.*/
                            try
                            {
                                athDB_N.Insert();
                            }
                            catch
                            {
                                athDB_N.MyPK = BP.DA.DBAccess.GenerGUID();
                                athDB_N.Insert();
                            }
                        }
                        else
                        {
                            //// This judgment guid  The file is uploaded by other threads copy Later ?
                            //if (athDB_N.IsExit(FrmAttachmentDBAttr.UploadGUID, athDB_N.UploadGUID,
                            //    FrmAttachmentDBAttr.FK_MapData, athDB_N.FK_MapData) == true)
                            //    continue; /* If you do not copy了.*/

                            athDB_N.MyPK = athDB_N.UploadGUID + "_" + athDB_N.FK_MapData + "_" + toWK.OID;
                            try
                            {
                                athDB_N.Insert();
                            }
                            catch
                            {
                                athDB_N.MyPK = BP.DA.DBAccess.GenerGUID();
                                athDB_N.Insert();
                            }
                        }
                    }
                }
            }
            #endregion  Copy the attachment .

            #region  Copy Picture upload attachments .
            if (this.HisNode.MapData.FrmImgAths.Count > 0)
            {
                FrmImgAthDBs athDBs = new FrmImgAthDBs("ND" + this.HisNode.NodeID,
                      this.WorkID.ToString());
                int idx = 0;
                if (athDBs.Count > 0)
                {
                    athDBs.Delete(FrmAttachmentDBAttr.FK_MapData, "ND" + toND.NodeID,
                        FrmAttachmentDBAttr.RefPKVal, this.WorkID);

                    /* Description of the current node has an attachment data */
                    foreach (FrmImgAthDB athDB in athDBs)
                    {
                        idx++;
                        FrmImgAthDB athDB_N = new FrmImgAthDB();
                        athDB_N.Copy(athDB);
                        athDB_N.FK_MapData = "ND" + toND.NodeID;
                        athDB_N.RefPKVal = this.WorkID.ToString();
                        athDB_N.MyPK = this.WorkID + "_" + idx + "_" + athDB_N.FK_MapData;
                        athDB_N.FK_FrmImgAth = athDB_N.FK_FrmImgAth.Replace("ND" + this.HisNode.NodeID, "ND" + toND.NodeID);
                        athDB_N.Save();
                    }
                }
            }
            #endregion  Copy Picture upload attachments .

            #region  Copy Ele
            if (this.HisNode.MapData.FrmEles.Count > 0)
            {
                FrmEleDBs eleDBs = new FrmEleDBs("ND" + this.HisNode.NodeID,
                      this.WorkID.ToString());
                if (eleDBs.Count > 0)
                {
                    eleDBs.Delete(FrmEleDBAttr.FK_MapData, "ND" + toND.NodeID,
                        FrmEleDBAttr.RefPKVal, this.WorkID);

                    /* Description of the current node has an attachment data */
                    foreach (FrmEleDB eleDB in eleDBs)
                    {
                        FrmEleDB eleDB_N = new FrmEleDB();
                        eleDB_N.Copy(eleDB);
                        eleDB_N.FK_MapData = "ND" + toND.NodeID;
                        eleDB_N.GenerPKVal();
                        eleDB_N.Save();
                    }
                }
            }
            #endregion  Copy Ele

            #region  Copy the multiple choice data 
            if (this.HisNode.MapData.MapM2Ms.Count > 0)
            {
                M2Ms m2ms = new M2Ms("ND" + this.HisNode.NodeID, this.WorkID);
                if (m2ms.Count >= 1)
                {
                    foreach (M2M item in m2ms)
                    {
                        M2M m2 = new M2M();
                        m2.Copy(item);
                        m2.EnOID = this.WorkID;
                        m2.FK_MapData = m2.FK_MapData.Replace("ND" + this.HisNode.NodeID, "ND" + toND.NodeID);
                        m2.InitMyPK();
                        try
                        {
                            m2.DirectInsert();
                        }
                        catch
                        {
                            m2.DirectUpdate();
                        }
                    }
                }
            }
            #endregion

            #region  Copy the detail 
            // int deBugDtlCount=
            Sys.MapDtls dtls = this.HisNode.MapData.MapDtls;
            string recDtlLog = "@ Record test schedule Copy Process , From node ID:" + this.HisNode.NodeID + " WorkID:" + this.WorkID + ",  To node ID=" + toND.NodeID;
            if (dtls.Count > 0)
            {
                Sys.MapDtls toDtls = toND.MapData.MapDtls;
                recDtlLog += "@ To list the number of nodes is :" + dtls.Count + "";

                Sys.MapDtls startDtls = null;
                bool isEnablePass = false; /* Is there a list of approvals .*/
                foreach (MapDtl dtl in dtls)
                {
                    if (dtl.IsEnablePass)
                        isEnablePass = true;
                }

                if (isEnablePass) /*  If you have to build it began node table data  */
                    startDtls = new BP.Sys.MapDtls("ND" + int.Parse(toND.FK_Flow) + "01");

                recDtlLog += "@ Started one by one into the circulation list copy.";
                int i = -1;

                foreach (Sys.MapDtl dtl in dtls)
                {
                    recDtlLog += "@ Enter the cycle begins execution schedule (" + dtl.No + ")copy.";

                    //i++;
                    //if (toDtls.Count <= i)
                    //    continue;
                    //Sys.MapDtl toDtl = (Sys.MapDtl)toDtls[i];

                    i++;
                    //if (toDtls.Count <= i)
                    //    continue;
                    Sys.MapDtl toDtl = null;
                    foreach (MapDtl todtl in toDtls)
                    {
                        string toDtlName = "";
                        string dtlName = "";
                        try
                        {

                            toDtlName = todtl.HisGEDtl.FK_MapDtl.Substring(todtl.HisGEDtl.FK_MapDtl.IndexOf("Dtl"), todtl.HisGEDtl.FK_MapDtl.Length - todtl.HisGEDtl.FK_MapDtl.IndexOf("Dtl"));
                            dtlName = dtl.HisGEDtl.FK_MapDtl.Substring(dtl.HisGEDtl.FK_MapDtl.IndexOf("Dtl"), dtl.HisGEDtl.FK_MapDtl.Length - dtl.HisGEDtl.FK_MapDtl.IndexOf("Dtl"));
                        }
                        catch
                        {
                            continue;
                        }

                        if (toDtlName == dtlName)
                        {
                            toDtl = todtl;
                            break;
                        }
                    }

                    if (dtl.IsEnablePass == true)
                    {
                        /* If the audit schedule through mechanism is enabled , Allows copy Node data .*/
                        toDtl.IsCopyNDData = true;
                    }

                    if (toDtl == null || toDtl.IsCopyNDData == false)
                        continue;

                    if (dtl.PTable == toDtl.PTable)
                        continue;


                    // Get detailed data .
                    GEDtls gedtls = new GEDtls(dtl.No);
                    QueryObject qo = null;
                    qo = new QueryObject(gedtls);
                    switch (dtl.DtlOpenType)
                    {
                        case DtlOpenType.ForEmp:
                            qo.AddWhere(GEDtlAttr.RefPK, this.WorkID);
                            break;
                        case DtlOpenType.ForWorkID:
                            qo.AddWhere(GEDtlAttr.RefPK, this.WorkID);
                            break;
                        case DtlOpenType.ForFID:
                            qo.AddWhere(GEDtlAttr.FID, this.WorkID);
                            break;
                    }
                    qo.DoQuery();

                    recDtlLog += "@ Check out the schedule :" + dtl.No + ", Detail :" + gedtls.Count + " records.";

                    int unPass = 0;
                    //  Whether audit mechanism to enable .
                    isEnablePass = dtl.IsEnablePass;
                    if (isEnablePass && this.HisNode.IsStartNode == false)
                        isEnablePass = true;
                    else
                        isEnablePass = false;

                    if (isEnablePass == true)
                    {
                        /* Determine whether the current node on the list ,isPass  Field audits , If no exception is thrown Information .*/
                        if (gedtls.Count != 0)
                        {
                            GEDtl dtl1 = gedtls[0] as GEDtl;
                            if (dtl1.EnMap.Attrs.Contains("IsPass") == false)
                                isEnablePass = false;
                        }
                    }

                    recDtlLog += "@ Delete arrival schedule :" + dtl.No + ", Data ,  And began to traverse the list , Execution line by line copy.";
                    DBAccess.RunSQL("DELETE FROM " + toDtl.PTable + " WHERE RefPK=" + dbStr + "RefPK", "RefPK", this.WorkID.ToString());

                    // copy Quantity .
                    int deBugNumCopy = 0;
                    foreach (GEDtl gedtl in gedtls)
                    {
                        if (isEnablePass)
                        {
                            if (gedtl.GetValBooleanByKey("IsPass") == false)
                            {
                                /* There will be no review by the  continue  They , Copy only been approved by the .*/
                                continue;
                            }
                        }

                        BP.Sys.GEDtl dtCopy = new GEDtl(toDtl.No);
                        dtCopy.Copy(gedtl);
                        dtCopy.FK_MapDtl = toDtl.No;
                        dtCopy.RefPK = this.WorkID.ToString();
                        dtCopy.InsertAsOID(dtCopy.OID);
                        dtCopy.RefPKInt64 = this.WorkID;
                        deBugNumCopy++;

                        #region   Schedule single copy  -  Additional information 
                        if (toDtl.IsEnableAthM)
                        {
                            /* If multiple attachments enabled , Additional information on the copy of this detail data .*/
                            FrmAttachmentDBs athDBs = new FrmAttachmentDBs(dtl.No, gedtl.OID.ToString());
                            if (athDBs.Count > 0)
                            {
                                i = 0;
                                foreach (FrmAttachmentDB athDB in athDBs)
                                {
                                    i++;
                                    FrmAttachmentDB athDB_N = new FrmAttachmentDB();
                                    athDB_N.Copy(athDB);
                                    athDB_N.FK_MapData = toDtl.No;
                                    athDB_N.MyPK = toDtl.No + "_" + dtCopy.OID + "_" + i.ToString();
                                    athDB_N.FK_FrmAttachment = athDB_N.FK_FrmAttachment.Replace("ND" + this.HisNode.NodeID,
                                        "ND" + toND.NodeID);
                                    athDB_N.RefPKVal = dtCopy.OID.ToString();
                                    athDB_N.DirectInsert();
                                }
                            }
                        }
                        if (toDtl.IsEnableM2M || toDtl.IsEnableM2MM)
                        {
                            /* If you enable m2m */
                            M2Ms m2ms = new M2Ms(dtl.No, gedtl.OID);
                            if (m2ms.Count > 0)
                            {
                                i = 0;
                                foreach (M2M m2m in m2ms)
                                {
                                    i++;
                                    M2M m2m_N = new M2M();
                                    m2m_N.Copy(m2m);
                                    m2m_N.FK_MapData = toDtl.No;
                                    m2m_N.MyPK = toDtl.No + "_" + m2m.M2MNo + "_" + gedtl.ToString() + "_" + m2m.DtlObj;
                                    m2m_N.EnOID = gedtl.OID;
                                    m2m.InitMyPK();
                                    m2m_N.DirectInsert();
                                }
                            }
                        }
                        #endregion   Schedule single copy  -  Additional information 

                    }
#warning  Logging .
                    if (gedtls.Count != deBugNumCopy)
                    {
                        recDtlLog += "@ From the list :" + dtl.No + ", Detail :" + gedtls.Count + " records.";
                        // Logging .
                        Log.DefaultLogWriteLineInfo(recDtlLog);
                        throw new Exception("@ System error , Keep the following information back to the administrator , Thank you .:  Technical Information :" + recDtlLog);
                    }

                    #region  If the audit mechanism to enable 
                    if (isEnablePass)
                    {
                        /*  If the mechanism is enabled by the audit , Put unaudited data copy Up to the first node  
                         * 1,  Find the corresponding breakdown point .
                         * 2,  Copy the data is not audited by the schedule to begin in .
                         */
                        string fk_mapdata = "ND" + int.Parse(toND.FK_Flow) + "01";
                        MapData md = new MapData(fk_mapdata);
                        string startUser = "SELECT Rec FROM " + md.PTable + " WHERE OID=" + this.WorkID;
                        startUser = DBAccess.RunSQLReturnString(startUser);

                        MapDtl startDtl = (MapDtl)startDtls[i];
                        foreach (GEDtl gedtl in gedtls)
                        {
                            if (gedtl.GetValBooleanByKey("IsPass"))
                                continue; /*  Preclude review by the  */

                            BP.Sys.GEDtl dtCopy = new GEDtl(startDtl.No);
                            dtCopy.Copy(gedtl);
                            dtCopy.OID = 0;
                            dtCopy.FK_MapDtl = startDtl.No;
                            dtCopy.RefPK = gedtl.OID.ToString(); //this.WorkID.ToString();
                            dtCopy.SetValByKey("BatchID", this.WorkID);
                            dtCopy.SetValByKey("IsPass", 0);
                            dtCopy.SetValByKey("Rec", startUser);
                            dtCopy.SetValByKey("Checker", this.ExecerName);
                            dtCopy.RefPKInt64 = this.WorkID;
                            dtCopy.SaveAsOID(gedtl.OID);
                        }
                        DBAccess.RunSQL("UPDATE " + startDtl.PTable + " SET Rec='" + startUser + "',Checker='" + this.Execer + "' WHERE BatchID=" + this.WorkID + " AND Rec='" + this.Execer + "'");
                    }
                    #endregion  If the audit mechanism to enable 
                }
            }
            #endregion  Copy the detail 
        }
        #endregion

        #region  Returns object processing .
        private SendReturnObjs HisMsgObjs = null;
        public void addMsg(string flag, string msg)
        {
            addMsg(flag, msg, null, SendReturnMsgType.Info);
        }
        public void addMsg(string flag, string msg, SendReturnMsgType msgType)
        {
            addMsg(flag, msg, null, msgType);
        }
        public void addMsg(string flag, string msg, string msgofHtml, SendReturnMsgType msgType)
        {
            if (HisMsgObjs == null)
                HisMsgObjs = new SendReturnObjs();
            this.HisMsgObjs.AddMsg(flag, msg, msgofHtml, msgType);
        }
        public void addMsg(string flag, string msg, string msgofHtml)
        {
            addMsg(flag, msg, msgofHtml, SendReturnMsgType.Info);
        }
        #endregion  Returns object processing .

        #region  Method 
        /// <summary>
        ///  Data transmission failure is withdrawn .
        /// </summary>
        public void DealEvalUn()
        {
            // Data transmission .
            BP.WF.Data.Eval eval = new Eval();
            if (this.HisNode.IsFLHL == false)
            {
                eval.MyPK = this.WorkID + "_" + this.HisNode.NodeID;
                eval.Delete();
            }

            //  The case of sub-confluent , It is produced by quality evaluation schedule .
            MapDtls dtls = this.HisNode.MapData.MapDtls;
            foreach (MapDtl dtl in dtls)
            {
                if (dtl.IsHLDtl == false)
                    continue;

                // Get detailed data .
                GEDtls gedtls = new GEDtls(dtl.No);
                QueryObject qo = null;
                qo = new QueryObject(gedtls);
                switch (dtl.DtlOpenType)
                {
                    case DtlOpenType.ForEmp:
                        qo.AddWhere(GEDtlAttr.RefPK, this.WorkID);
                        break;
                    case DtlOpenType.ForWorkID:
                        qo.AddWhere(GEDtlAttr.RefPK, this.WorkID);
                        break;
                    case DtlOpenType.ForFID:
                        qo.AddWhere(GEDtlAttr.FID, this.WorkID);
                        break;
                }
                qo.DoQuery();

                foreach (GEDtl gedtl in gedtls)
                {
                    eval = new Eval();
                    eval.MyPK = gedtl.OID + "_" + gedtl.Rec;
                    eval.Delete();
                }
            }
        }
        /// <summary>
        ///  Processing quality assessment 
        /// </summary>
        public void DealEval()
        {
            if (this.HisNode.IsEval == false)
                return;

            BP.WF.Data.Eval eval = new Eval();
            eval.CheckPhysicsTable();

            if (this.HisNode.IsFLHL == false)
            {
                eval.MyPK = this.WorkID + "_" + this.HisNode.NodeID;
                eval.Delete();

                eval.Title = this.HisGenerWorkFlow.Title;

                eval.WorkID = this.WorkID;
                eval.FK_Node = this.HisNode.NodeID;
                eval.NodeName = this.HisNode.Name;

                eval.FK_Flow = this.HisNode.FK_Flow;
                eval.FlowName = this.HisNode.FlowName;

                eval.FK_Dept = this.ExecerDeptNo;
                eval.DeptName = this.ExecerDeptName;

                eval.Rec = this.Execer;
                eval.RecName = this.ExecerName;

                eval.RDT = DataType.CurrentDataTime;
                eval.FK_NY = DataType.CurrentYearMonth;

                eval.EvalEmpNo = this.HisWork.GetValStringByKey(WorkSysFieldAttr.EvalEmpNo);
                eval.EvalEmpName = this.HisWork.GetValStringByKey(WorkSysFieldAttr.EvalEmpName);
                eval.EvalCent = this.HisWork.GetValStringByKey(WorkSysFieldAttr.EvalCent);
                eval.EvalNote = this.HisWork.GetValStringByKey(WorkSysFieldAttr.EvalNote);

                eval.Insert();
                return;
            }

            //  The case of sub-confluent , It is produced by quality evaluation schedule .
            Sys.MapDtls dtls = this.HisNode.MapData.MapDtls;
            foreach (MapDtl dtl in dtls)
            {
                if (dtl.IsHLDtl == false)
                    continue;

                // Get detailed data .
                GEDtls gedtls = new GEDtls(dtl.No);
                QueryObject qo = null;
                qo = new QueryObject(gedtls);
                switch (dtl.DtlOpenType)
                {
                    case DtlOpenType.ForEmp:
                        qo.AddWhere(GEDtlAttr.RefPK, this.WorkID);
                        break;
                    case DtlOpenType.ForWorkID:
                        qo.AddWhere(GEDtlAttr.RefPK, this.WorkID);
                        break;
                    case DtlOpenType.ForFID:
                        qo.AddWhere(GEDtlAttr.FID, this.WorkID);
                        break;
                }
                qo.DoQuery();

                foreach (GEDtl gedtl in gedtls)
                {
                    eval = new Eval();
                    eval.MyPK = gedtl.OID + "_" + gedtl.Rec;
                    eval.Delete();

                    eval.Title = this.HisGenerWorkFlow.Title;

                    eval.WorkID = this.WorkID;
                    eval.FK_Node = this.HisNode.NodeID;
                    eval.NodeName = this.HisNode.Name;

                    eval.FK_Flow = this.HisNode.FK_Flow;
                    eval.FlowName = this.HisNode.FlowName;

                    eval.FK_Dept = this.ExecerDeptNo;
                    eval.DeptName = this.ExecerDeptName;

                    eval.Rec = this.Execer;
                    eval.RecName = this.ExecerName;

                    eval.RDT = DataType.CurrentDataTime;
                    eval.FK_NY = DataType.CurrentYearMonth;

                    eval.EvalEmpNo = gedtl.GetValStringByKey(WorkSysFieldAttr.EvalEmpNo);
                    eval.EvalEmpName = gedtl.GetValStringByKey(WorkSysFieldAttr.EvalEmpName);
                    eval.EvalCent = gedtl.GetValStringByKey(WorkSysFieldAttr.EvalCent);
                    eval.EvalNote = gedtl.GetValStringByKey(WorkSysFieldAttr.EvalNote);
                    eval.Insert();
                }
            }
        }
        private void CallSubFlow()
        {
            //  For configuration information .
            string[] paras = this.HisNode.SubFlowStartParas.Split('@');
            foreach (string item in paras)
            {
                if (string.IsNullOrEmpty(item))
                    continue;

                string[] keyvals = item.Split(';');

                string FlowNo = ""; // Process ID 
                string EmpField = ""; //  Field staff .
                string DtlTable = ""; // List .
                foreach (string keyval in keyvals)
                {
                    if (string.IsNullOrEmpty(keyval))
                        continue;

                    string[] strs = keyval.Split('=');
                    switch (strs[0])
                    {
                        case "FlowNo":
                            FlowNo = strs[1];
                            break;
                        case "EmpField":
                            EmpField = strs[1];
                            break;
                        case "DtlTable":
                            DtlTable = strs[1];
                            break;
                        default:
                            throw new Exception("@ Process design errors , When the acquisition process initiated parameter configuration attributes , Mark unspecified : " + strs[0]);
                    }

                    if (this.HisNode.SubFlowStartWay == SubFlowStartWay.BySheetField)
                    {
                        string emps = this.HisWork.GetValStringByKey(EmpField) + ",";
                        string[] empStrs = emps.Split(',');

                        string currUser = this.Execer;
                        Emps empEns = new Emps();
                        string msgInfo = "";
                        foreach (string emp in empStrs)
                        {
                            if (string.IsNullOrEmpty(emp))
                                continue;

                            // Log in as current staff .
                            Emp empEn = new Emp(emp);
                            BP.Web.WebUser.SignInOfGener(empEn);

                            //  To copy data to it .
                            Flow fl = new Flow(FlowNo);
                            StartWork sw = fl.NewWork();

                            Int64 workID = sw.OID;
                            sw.Copy(this.HisWork);
                            sw.OID = workID;
                            sw.Update();

                            WorkNode wn = new WorkNode(sw, new Node(int.Parse(FlowNo + "01")));
                            wn.NodeSend(null, this.Execer);
                            msgInfo += BP.WF.Dev2Interface.Node_StartWork(FlowNo, null, null, 0, emp, this.WorkID, FlowNo);
                        }
                    }

                }
            }


            //BP.WF.Dev2Interface.Flow_NewStartWork(
            DataTable dt;

        }
        #endregion

        #region  Related functions .
        /// <summary>
        ///  Listen to perform message-related functions 
        /// </summary>
        public void DoRefFunc_Listens()
        {
            Listens lts = new Listens();
            lts.RetrieveByLike(ListenAttr.Nodes, "%" + this.HisNode.NodeID + "%");
            string info = "";
            foreach (Listen lt in lts)
            {
                ps = new Paras();
                ps.SQL = "SELECT FK_Emp,FK_EmpText FROM WF_GenerWorkerList WHERE IsEnable=1 AND IsPass=1 AND FK_Node=" + dbStr + "FK_Node AND WorkID=" + dbStr + "WorkID";
                ps.Add("FK_Node", lt.FK_Node);
                ps.Add("WorkID", this.WorkID);
                DataTable dtRem = BP.DA.DBAccess.RunSQLReturnTable(ps);
                foreach (DataRow dr in dtRem.Rows)
                {
                    string FK_Emp = dr["FK_Emp"] as string;

                    string title = lt.Title.Clone() as string;
                    title = title.Replace("@WebUser.No", this.Execer)
                        .Replace("@WebUser.Name", this.ExecerName)
                        .Replace("@WebUser.FK_Dept", this.ExecerDeptNo)
                        .Replace("@WebUser.FK_DeptName", this.ExecerDeptName)
                        .Replace("@WF.FK_Node", this.HisNode.NodeID.ToString())
                        .Replace("@WF.WorkID", this.WorkID.ToString())
                        .Replace("@WF.FK_Flow", this.HisNode.FK_Flow)
                        ;
                    

                    string doc = lt.Doc.Clone() as string;
                    doc = doc.Replace("@WebUser.No", this.Execer)
                        .Replace("@WebUser.Name", this.ExecerName)
                        .Replace("@WebUser.FK_Dept", this.ExecerDeptNo)
                        .Replace("@WebUser.FK_DeptName", this.ExecerDeptName)
                        .Replace("@WF.FK_Node", this.HisNode.NodeID.ToString())
                        .Replace("@WF.WorkID", this.WorkID.ToString())
                        .Replace("@WF.FK_Flow", this.HisNode.FK_Flow);

                    Attrs attrs = this.rptGe.EnMap.Attrs;
                    foreach (Attr attr in attrs)
                    {
                        title = title.Replace("@" + attr.Key, this.rptGe.GetValStrByKey(attr.Key));
                        doc = doc.Replace("@" + attr.Key, this.rptGe.GetValStrByKey(attr.Key));

                        doc = doc.Replace("@dtl(" + attr.Key + ")",MapDtl.GenTableHtml(attr.Key,this.WorkID));
                    }

                    if (this.town == null)
                        BP.WF.Dev2Interface.Port_SendMsg(FK_Emp, title, doc,
                            "LS" + FK_Emp + "_" + this.WorkID, BP.WF.SMSMsgType.Self,
                            this.HisFlow.No, this.HisNode.NodeID, this.WorkID, 0);
                    else
                        BP.WF.Dev2Interface.Port_SendMsg(FK_Emp, title, doc,
                            "LS" + FK_Emp + "_" + this.WorkID, BP.WF.SMSMsgType.Self,
                        this.HisFlow.No, this.town.HisNode.NodeID, this.WorkID, 0);

                    info += dr[GenerWorkerListAttr.FK_EmpText].ToString() + ",";
                }
            }

            if (string.IsNullOrEmpty(info) == false)
            {
                //this.addMsg(SendReturnMsgFlag.End, "@ The process has come to the last node , Successful conclusion of the process .");
                this.addMsg(SendReturnMsgFlag.ListenInfo, "@ Current execution has been notified to the :" + info);
            }
        }
        #endregion  Related functions .

        /// <summary>
        ///  Send business process workflow 
        /// </summary>
        public SendReturnObjs NodeSend()
        {
            return NodeSend(null, null);
        }
        /// <summary>
        ///  Check the items must be filled .
        /// </summary>
        /// <returns></returns>
        public bool CheckFrmIsNotNull()
        {
            if (this.HisNode.HisFormType != NodeFormType.SheetTree)
                return true;

            // Check out all the settings .
            FrmFields ffs = new FrmFields();

            QueryObject qo = new QueryObject(ffs);
            qo.AddWhere(FrmFieldAttr.FK_Node, this.HisNode.NodeID);
            qo.addAnd();
            qo.addLeftBracket();
            qo.AddWhere(FrmFieldAttr.IsNotNull, 1);
            qo.addOr();
            qo.AddWhere(FrmFieldAttr.IsWriteToFlowTable, 1);
            qo.addRightBracket();
            qo.DoQuery();

            if (ffs.Count == 0)
                return true;


            BP.WF.Template.FrmNodes frmNDs = new FrmNodes(this.HisNode.FK_Flow, this.HisNode.NodeID);
            string err = "";
            foreach (FrmNode item in frmNDs)
            {
                MapData md = new MapData(item.FK_Frm);

                // May be url.
                if (md.HisFrmType == FrmType.Url)
                    continue;

                // Check ?
                bool isHave = false;
                foreach (FrmField myff in ffs)
                {
                    if (myff.FK_MapData != item.FK_Frm)
                        continue;
                    isHave = true;
                    break;
                }
                if (isHave == false)
                    continue;

                //  Processing primary key .
                long pk = 0;// this.WorkID;

                switch (item.WhoIsPK)
                {
                    case WhoIsPK.FID:
                        pk = this.HisWork.FID;
                        break;
                    case WhoIsPK.OID:
                        pk = this.HisWork.OID;
                        break;
                    case WhoIsPK.PWorkID:
                        pk = this.rptGe.PWorkID;
                        break;
                    case WhoIsPK.CWorkID:
                        pk = this.HisGenerWorkFlow.CWorkID;
                        break;
                    default:
                        throw new Exception("@ Not the type of judge .");
                }

                if (pk == 0)
                    throw new Exception("@ Failed to get form the primary key .");



                // Get form values 
                ps = new Paras();
                ps.SQL = "SELECT * FROM " + md.PTable + " WHERE OID=" + ps.DBStr + "OID";
                ps.Add(WorkAttr.OID, pk);
                DataTable dt = DBAccess.RunSQLReturnTable(ps);
                if (dt.Rows.Count == 0)
                {
                    err += "@ Form {" + md.Name + "} No input data .";
                    continue;
                }


                //  Check the data is complete .
                foreach (FrmField ff in ffs)
                {
                    if (ff.FK_MapData != item.FK_Frm)
                        continue;

                    // Get the data .
                    string val = string.Empty;
                    val = dt.Rows[0][ff.KeyOfEn].ToString();

                    if (ff.IsNotNull == true && Glo.IsEnableCheckFrmTreeIsNull == true)
                    {
                        /* If the check can not be empty  */
                        if (string.IsNullOrEmpty(val) == true || val.Trim() == "")
                            err += "@ Form {" + md.Name + "} Field {" + ff.KeyOfEn + " ; " + ff.Name + "}, Can not be empty .";
                    }

                    // Determine whether the need to write process data table .
                    if (ff.IsWriteToFlowTable == true)
                    {
                        this.HisWork.SetValByKey(ff.KeyOfEn, val);
                        //this.rptGe.SetValByKey(ff.KeyOfEn, val);
                    }
                }
            }
            if (err != "")
                throw new Exception(" Before submitting checks to fill in the fields below will lose incomplete :" + err);

            return true;
        }
        /// <summary>
        ///  Execution Cc 
        /// </summary>
        public void DoCC()
        {
        }
        /// <summary>
        ///  If collaboration .
        /// </summary>
        /// <returns></returns>
        public bool DealTeamUpNode()
        {
            GenerWorkerLists gwls = new GenerWorkerLists();
            gwls.Retrieve(GenerWorkerListAttr.WorkID, this.WorkID,
                GenerWorkerListAttr.FK_Node, this.HisNode.NodeID, GenerWorkerListAttr.IsPass);

            if (gwls.Count == 1)
                return false; /* Allowed to execute down , Because only one person . There is no order in question .*/

            // To see if I was the last one ?
            int num = 0;
            string todoEmps = ""; // Record does not deal with people .
            foreach (GenerWorkerList item in gwls)
            {
                if (item.IsPassInt == 0)
                {
                    if (item.FK_Emp != WebUser.No)
                        todoEmps += BP.WF.Glo.DealUserInfoShowModel(item.FK_Emp, item.FK_EmpText) + " ";
                    num++;
                }
            }
            if (num == 1)
                return false; /* Only a to-do , Explained that he is the last person .*/

            // The current to-do settings do , Tips and untreated people .
            foreach (GenerWorkerList gwl in gwls)
            {

                if (gwl.FK_Emp != WebUser.No)
                    continue;

                // Can not be used to set the current .
                gwl.IsPassInt = 1;
                gwl.Update();

                // Written to the log .
                this.AddToTrack(ActionType.TeampUp, gwl.FK_Emp, todoEmps, this.HisNode.NodeID, this.HisNode.Name, " Send collaboration ");
                this.addMsg(SendReturnMsgFlag.OverCurr, " The current work of untreated people : " + todoEmps + " .", null, SendReturnMsgType.Info);
                return true;
            }

            throw new Exception("@ Should not be run here .");
        }
        /// <summary>
        ///  Processing queue node 
        /// </summary>
        /// <returns> Can I send down ?</returns>
        public bool DealOradeNode()
        {
            GenerWorkerLists gwls = new GenerWorkerLists();
            gwls.Retrieve(GenerWorkerListAttr.WorkID, this.WorkID,
                GenerWorkerListAttr.FK_Node, this.HisNode.NodeID, GenerWorkerListAttr.IsPass);

            if (gwls.Count == 1)
                return false; /* Allowed to execute down , Because only one person . There is no order in question .*/

            int idx = -100;
            foreach (GenerWorkerList gwl in gwls)
            {
                idx++;
                if (gwl.FK_Emp != WebUser.No)
                    continue;

                // Can not be used to set the current .
                gwl.IsPassInt = idx;
                gwl.Update();
            }

            foreach (GenerWorkerList gwl in gwls)
            {
                if (gwl.IsPassInt > 10)
                {
                    /* Began to be sent to the person who . */
                    gwl.IsPassInt = 0;
                    gwl.Update();

                    // Written to the log .
                    this.AddToTrack(ActionType.Order, gwl.FK_Emp, gwl.FK_EmpText, this.HisNode.NodeID,
                        this.HisNode.Name, " Send Queue ");

                    this.addMsg(SendReturnMsgFlag.VarAcceptersID, gwl.FK_Emp, gwl.FK_Emp, SendReturnMsgType.SystemMsg);
                    this.addMsg(SendReturnMsgFlag.VarAcceptersName, gwl.FK_EmpText, gwl.FK_EmpText, SendReturnMsgType.SystemMsg);
                    this.addMsg(SendReturnMsgFlag.OverCurr, " The current work has been sent to the (" + gwl.FK_Emp + "," + gwl.FK_EmpText + ").", null, SendReturnMsgType.Info);

                    // Perform an update .
                    if (this.HisGenerWorkFlow.Emps.Contains("@" + WebUser.No + "@") == false)
                        this.HisGenerWorkFlow.Emps = this.HisGenerWorkFlow.Emps + WebUser.No + "@";

                    this.rptGe.FlowEmps = this.HisGenerWorkFlow.Emps;
                    this.rptGe.WFState = WFState.Runing;

                    this.rptGe.Update(GERptAttr.FlowEmps, this.rptGe.FlowEmps, GERptAttr.WFState, (int)WFState.Runing);


                    this.HisGenerWorkFlow.WFState = WFState.Runing;
                    this.HisGenerWorkFlow.Update();
                    return true;
                }
            }

            //  If it is the last one , He would send down .
            return false;
        }
        /// <summary>
        ///  Send business process workflow .
        ///  Upgrade Date :2012-11-11.
        ///  Upgrade reason : Code logic is not clear , Omission processing mode .
        ///  Modifier :zhoupeng.
        ///  Modify Location : Xiamen .
        /// -----------------------------------  Explanation  -----------------------------
        /// 1, Method body is divided into three parts :  Check before sending \5*5 Algorithm \ Business processing after sending .
        /// 2,  Refer to instructions on the body of code .
        /// 3,  After sending direct access to its 
        /// </summary>
        /// <param name="jumpToNode"> To jump to the node </param>
        /// <param name="jumpToEmp"> To jump man </param>
        /// <returns> Implementation structure </returns>
        public SendReturnObjs NodeSend(Node jumpToNode, string jumpToEmp)
        {
            if (this.HisNode.IsGuestNode)
            {
                if (this.Execer != "Guest")
                    throw new Exception("@ The current node （" + this.HisNode.Name + "） Is a client node is executed , So currently logged in staff should be Guest, It is :" + this.Execer);
            }

            #region  Security check .
            // 第1:  Check whether you can deal with the current work .
            if (this.HisNode.IsStartNode == false
                && BP.WF.Dev2Interface.Flow_IsCanDoCurrentWork(this.HisNode.FK_Flow, this.HisNode.NodeID,
                this.WorkID, this.Execer) == false)
                throw new Exception("@ You have dealt with the current work is completed , Or you (" + this.Execer + " " + this.ExecerName + ") Did not deal with the current work rights .");

            // 第1.2:  Event interface calls initiated before , Handling user-defined business logic .
            string sendWhen = this.HisFlow.DoFlowEventEntity(EventListOfNode.SendWhen, this.HisNode, this.HisWork, null);
            if (sendWhen != null)
            {
                /* Illustrate the events to be executed , After execution of the query data to an entity in */
                this.HisWork.RetrieveFromDBSources();
                this.HisWork.ResetDefaultVal();
                this.HisWork.Rec = this.Execer;
                this.HisWork.RecText = this.ExecerName;
                if (string.IsNullOrEmpty(sendWhen) == false)
                    this.addMsg(SendReturnMsgFlag.SendWhen, sendWhen);
            }
            #endregion  Security check .

            // Added to the system variable .
            this.addMsg(SendReturnMsgFlag.VarCurrNodeID, this.HisNode.NodeID.ToString(), this.HisNode.NodeID.ToString(), SendReturnMsgType.SystemMsg);
            this.addMsg(SendReturnMsgFlag.VarCurrNodeName, this.HisNode.Name, this.HisNode.Name, SendReturnMsgType.SystemMsg);
            this.addMsg(SendReturnMsgFlag.VarWorkID, this.WorkID.ToString(), this.WorkID.ToString(), SendReturnMsgType.SystemMsg);


            // Jump node set , If there can be null.
            this.JumpToNode = jumpToNode;
            this.JumpToEmp = jumpToEmp;

            string sql = null;
            DateTime dt = DateTime.Now;
            this.HisWork.Rec = this.Execer;
            this.WorkID = this.HisWork.OID;

            #region  First step :  Check whether the current operator can send :  Divided as follows  3  Steps .
            //第1.2.1:  If this is the start node , Initiate the process necessary to check restrictions .
            if (this.HisNode.IsStartNode)
            {
                if (Glo.CheckIsCanStartFlow_SendStartFlow(this.HisFlow, this.HisWork) == false)
                    throw new Exception("@ Violation of the process initiated by restrictions :" + Glo.DealExp(this.HisFlow.StartLimitAlert, this.HisWork, null));
            }

            // 第1.3:  To determine the current state of the process .
            if (this.HisNode.IsStartNode == false
                && this.HisGenerWorkFlow.WFState == WFState.Askfor)
            {
                /* If the signature status ,  After you determine endorsement , Do you want to return to the execution of people plus sign .*/
                GenerWorkerLists gwls = new GenerWorkerLists();
                gwls.Retrieve(GenerWorkerListAttr.FK_Node, this.HisNode.NodeID,
                    GenerWorkerListAttr.WorkID, this.WorkID);

                bool isDeal = false;
                AskforHelpSta askForSta = AskforHelpSta.AfterDealSend;
                foreach (GenerWorkerList item in gwls)
                {
                    if (item.IsPassInt == (int)AskforHelpSta.AfterDealSend)
                    {
                        /* If it is after endorsement , Do not deal directly sent .*/
                        isDeal = true;
                        askForSta = AskforHelpSta.AfterDealSend;

                        //  Update workerlist,  Set the status of all persons that have been processed for the state , It went to the next step .
                        DBAccess.RunSQL("UPDATE WF_GenerWorkerList SET IsPass=1 WHERE FK_Node=" + this.HisNode.NodeID + " AND WorkID=" + this.WorkID);

                        // Written to the log .
                        this.AddToTrack(ActionType.ForwardAskfor, item.FK_Emp, item.FK_EmpText,
                            this.HisNode.NodeID, this.HisNode.Name, " Sent down after endorsement , Sent directly to the person for further processing .");
                    }

                    if (item.IsPassInt == (int)AskforHelpSta.AfterDealSendByWorker)
                    {
                        /* If it is after endorsement , Sent directly from me .*/
                        item.IsPassInt = 0;

                        isDeal = true;
                        askForSta = AskforHelpSta.AfterDealSendByWorker;

                        //  Update workerlist,  Set the status of all persons that have been processed for the state .
                        DBAccess.RunSQL("UPDATE WF_GenerWorkerList SET IsPass=1 WHERE FK_Node=" + this.HisNode.NodeID + " AND WorkID=" + this.WorkID);

                        //  The officer initiated endorsement status updates over , Let him do the work to be seen .
                        item.IsPassInt = 0;
                        item.Update();

                        //  Update process status .
                        this.HisGenerWorkFlow.WFState = WFState.AskForReplay;
                        this.HisGenerWorkFlow.Update();

                        // Let people plus sign , Work is set to Unread .
                        BP.WF.Dev2Interface.Node_SetWorkUnRead(this.HisNode.NodeID, this.WorkID, item.FK_Emp);

                        //  Reply to obtain endorsement from the temporary variable comments .
                        string replyInfo = this.HisGenerWorkFlow.Paras_AskForReply;

                        //// Written to the log .
                        //this.AddToTrack(ActionType.ForwardAskfor, item.FK_Emp, item.FK_EmpText,
                        //    this.HisNode.NodeID, this.HisNode.Name,
                        //    " Sent down after endorsement , And people turned endorsement sponsors （" + item.FK_Emp + "," + item.FK_EmpText + "）.<br> Opinion :" + replyInfo);

                        // Written to the log .
                        this.AddToTrack(ActionType.ForwardAskfor, item.FK_Emp, item.FK_EmpText,
                            this.HisNode.NodeID, this.HisNode.Name, " Reply Comment :" + replyInfo);

                        // Added to the system variable .
                        this.addMsg(SendReturnMsgFlag.VarToNodeID, this.HisNode.NodeID.ToString(), SendReturnMsgType.SystemMsg);
                        this.addMsg(SendReturnMsgFlag.VarToNodeName, this.HisNode.Name, SendReturnMsgType.SystemMsg);
                        this.addMsg(SendReturnMsgFlag.VarAcceptersID, item.FK_Emp, SendReturnMsgType.SystemMsg);
                        this.addMsg(SendReturnMsgFlag.VarAcceptersName, item.FK_EmpText, SendReturnMsgType.SystemMsg);

                        // Join message .
                        this.addMsg(SendReturnMsgFlag.SendSuccessMsg, " Has been transferred to , Endorsement of sponsors (" + item.FK_Emp + "," + item.FK_EmpText + ")", SendReturnMsgType.Info);

                        // Delete temporary increase in the current operator work list records ,  If you do not remove the plus sign will lead to a second defeat .
                        GenerWorkerList gwl = new GenerWorkerList();
                        gwl.Delete(GenerWorkerListAttr.FK_Node, this.HisNode.NodeID,
                            GenerWorkerListAttr.WorkID, this.WorkID, GenerWorkerListAttr.FK_Emp, this.Execer);

                        // Back Send object .
                        return this.HisMsgObjs;
                    }
                }

                if (isDeal == false)
                    throw new Exception("@ Process Engine Error , Should not the state can not find the plus sign .");
            }


            // 第3:  If yes confluence , Situation unfinished child thread .
            if (this.HisNode.IsHL || this.HisNode.HisRunModel == RunModel.FHL)
            {
                /*    If it is the confluence point   Check whether the current confluence point if it is , Check on whether the child thread shunt completed .*/
                /* Check if there is no end child thread */
                Paras ps = new Paras();
                ps.SQL = "SELECT WorkID,FK_Emp,FK_EmpText,FK_NodeText FROM WF_GenerWorkerList WHERE FID=" + ps.DBStr + "FID AND IsPass=0 AND IsEnable=1";
                ps.Add(WorkAttr.FID, this.WorkID);

                DataTable dtWL = DBAccess.RunSQLReturnTable(ps);
                string infoErr = "";
                if (dtWL.Rows.Count != 0)
                {
                    if (this.HisNode.ThreadKillRole == ThreadKillRole.None
                        || this.HisNode.ThreadKillRole == ThreadKillRole.ByHand)
                    {
                        infoErr += "@ You can not send down , There are sub-thread is not completed .";
                        foreach (DataRow dr in dtWL.Rows)
                        {
                            infoErr += "@ Operator number :" + dr["FK_Emp"] + "," + dr["FK_EmpText"] + ", Stay node :" + dr["FK_NodeText"];
                        }

                        if (this.HisNode.ThreadKillRole == ThreadKillRole.ByHand)
                            infoErr += "@ Please notify their processing is complete , Or you can force the removal of the sub-process is sent down .";
                        else
                            infoErr += "@ Please notify their processing is complete , You can send down .";

                        // Thrown downward movement to stop it .
                        throw new Exception(infoErr);
                    }

                    if (this.HisNode.ThreadKillRole == ThreadKillRole.ByAuto)
                    {
                        // Delete each child thread , Then the downward movement .
                        foreach (DataRow dr in dtWL.Rows)
                            BP.WF.Dev2Interface.Flow_DeleteSubThread(this.HisFlow.No, Int64.Parse(dr[0].ToString()));
                    }
                }
            }
            #endregion  First step :  Check whether the current operator can send 


            // Check out the current work reports node .
            this.rptGe = this.HisNode.HisFlow.HisGERpt;
            this.rptGe.SetValByKey("OID", this.WorkID);
            this.rptGe.RetrieveFromDBSources();

            // Check for the presence of sub-processes ,  If you have let the child process to send down .
            if (this.HisNode.IsCheckSubFlowOver == true)
            {
                /* If set to check whether the child process ends .*/
                if (BP.WF.Dev2Interface.Flow_NumOfSubFlowRuning(this.WorkID) > 0)
                {
                    /* If the number of sub-processes is greater than 0,  There is no complete description of the sub-process .*/
                    throw new Exception("@ There are no sub-process is complete, you can not submit , Need to wait until all of the sub-process is complete, you can send .");
                }
            }

            //  Inspection FormTree Required items , If there are some items do not fill it throws an exception .
            this.CheckFrmIsNotNull();

            // The data is updated to the database .
            this.HisWork.DirectUpdate();
            if (this.HisWork.EnMap.PhysicsTable != this.rptGe.EnMap.PhysicsTable)
            {
                //  There may be external parameters passed over lead ,rpt Table data has not changed .
                this.rptGe.Copy(this.HisWork);
            }

            // If the queue node ,  Would not let him judge .
            if (this.HisNode.TodolistModel == TodolistModel.Order)
            {
                if (this.DealOradeNode() == true)
                    return this.HisMsgObjs;
            }

            // If the node is a collaborative model ,  Would not let him judge .
            if (this.HisNode.TodolistModel == TodolistModel.Teamup)
            {
                /* If collaboration .*/
                if (this.DealTeamUpNode() == true)
                    return this.HisMsgObjs;
            }

            //  Start Services ,  There is no realization .
            DBAccess.DoTransactionBegin();
            try
            {
                if (this.HisNode.IsStartNode)
                    InitStartWorkDataV2(); //  Initialize the starting node data ,  If the current node is the start node .

                if (this.HisGenerWorkFlow.WFState == WFState.ReturnSta)
                {
                    /*  Check whether the return is backtrack ? */
                    Paras ps = new Paras();
                    ps.SQL = "SELECT ReturnNode,Returner,IsBackTracking FROM WF_ReturnWork WHERE WorkID=" + dbStr + "WorkID AND IsBackTracking=1 ORDER BY RDT DESC";
                    ps.Add(ReturnWorkAttr.WorkID, this.WorkID);
                    DataTable mydt = DBAccess.RunSQLReturnTable(ps);
                    if (mydt.Rows.Count != 0)
                    {
                        // Check out the multiple possible , Because sorted by time , Remove only the last returned , Return and see if there is information to backtrack .

                        /* Confirm the return , Is returned and backtrack  ,   Here initialize its staff ,  With the node that will be sent . */
                        this.JumpToNode = new Node(int.Parse(mydt.Rows[0]["ReturnNode"].ToString()));
                        this.JumpToEmp = mydt.Rows[0]["Returner"].ToString();

                        /* If the current is returned .*/
                        if (this.JumpToNode.TodolistModel == TodolistModel.Order
                            || this.JumpToNode.TodolistModel == TodolistModel.Teamup)
                        {
                            /* If it is more than the processing nodes .*/
                            this.DealReturnOrderTeamup();
                            return this.HisMsgObjs;
                        }
                    }
                }

                if (this.HisGenerWorkFlow.FK_Node != this.HisNode.NodeID)
                    throw new Exception("@ Error process appears , Activities inconsistent with the current point and send point ");

                //  Check the complete condition .
                if (jumpToNode != null && this.HisNode.IsEndNode)
                {
                    /* Jump is the case , And is the last node , Check the condition of the process is not completed .*/
                }
                else
                {
                    this.CheckCompleteCondition();
                }

                //  Deal flow freely . add by stone. 2014-11-23.
                if (jumpToNode == null && this.HisGenerWorkFlow.IsAutoRun == false)
                {
                    // If you do not want to jump to the specified node , And the current state of the processing operation manual intervention .
                    TransferCustom tc = TransferCustom.GetNextTransferCustom(this.WorkID, this.HisNode.NodeID);
                    if (tc == null)
                    {
                        /* This is the end that the implementation process .*/
                        this.IsStopFlow = true;
                    }
                    else
                    {
                        this.JumpToNode = new Node(tc.FK_Node);
                        this.JumpToEmp = tc.Worker;
                    }
                }


                //  Processing quality assessment , Before sending .
                this.DealEval();

                //  Added to the system variable .
                if (this.IsStopFlow)
                    this.addMsg(SendReturnMsgFlag.IsStopFlow, "1", " The process has ended ", SendReturnMsgType.Info);
                else
                    this.addMsg(SendReturnMsgFlag.IsStopFlow, "0", " The process has not been completed ", SendReturnMsgType.SystemMsg);

                if (this.IsStopFlow == true)
                {
                    /* After checking , Feedback processes have stopped logo .*/
                    this.Func_DoSetThisWorkOver();
                    this.rptGe.WFState = WFState.Complete;


                    this.rptGe.Update();
                }
                else
                {
                    #region  The second step :  The flow into the core area of operation is calculated . 5*5  Send a way to deal with different situations .
                    //  Execution node sends down 25 Judge cases of .
                    this.NodeSend_Send_5_5();

                    if (this.IsStopFlow)
                    {
                        this.rptGe.WFState = WFState.Complete;
                        this.Func_DoSetThisWorkOver();
                    }
                    else
                    {
                        // If it is returned to the state , Whether put backtrack trajectory removed .
                        if (this.HisGenerWorkFlow.WFState == WFState.ReturnSta)
                            BP.DA.DBAccess.RunSQL("UPDATE WF_ReturnWork SET IsBackTracking=0 WHERE WorkID=" + this.WorkID);

                        this.Func_DoSetThisWorkOver();
                        if (town != null && town.HisNode.HisBatchRole == BatchRole.Group)
                        {
                            this.HisGenerWorkFlow.WFState = WFState.Batch;
                            this.HisGenerWorkFlow.Update();
                        }
                    }

                    this.rptGe.Update();
                    #endregion  The second step : 5*5  Send a way to deal with different situations .
                }

                #region  The third step :  Sent after business logic .
                // The current node form data copy The process data table .
                this.DoCopyCurrentWorkDataToRpt();

                #endregion  The third step :  Sent after business logic .

                #region  Processing sub-thread 
                if (this.HisNode.IsStartNode && this.HisNode.SubFlowStartWay != SubFlowStartWay.None)
                    CallSubFlow();

                #endregion  Processing sub-thread 

                #region  Listen treatment .
                if (Glo.IsEnableSysMessage && this.IsStopFlow == false)
                    this.DoRefFunc_Listens(); //  If you have terminated workflow, Listen to the message has been called .
                #endregion

                #region  Generate documents 
                if (this.HisNode.BillTemplates.Count > 0)
                {
                    BillTemplates reffunc = this.HisNode.BillTemplates;

                    #region  Information generated documents 
                    Int64 workid = this.HisWork.OID;
                    int nodeId = this.HisNode.NodeID;
                    string flowNo = this.HisNode.FK_Flow;
                    #endregion

                    DateTime dtNow = DateTime.Now;
                    Flow fl = this.HisNode.HisFlow;
                    string year = dt.Year.ToString();
                    string billInfo = "";
                    foreach (BillTemplate func in reffunc)
                    {
                        if (func.HisBillFileType != BillFileType.RuiLang)
                        {
                            string file = year + "_" + this.ExecerDeptNo + "_" + func.No + "_" + workid + ".doc";
                            BP.Pub.RTFEngine rtf = new BP.Pub.RTFEngine();

                            Works works;
                            string[] paths;
                            string path;
                            try
                            {
                                #region  The data in   Documents Engine .
                                rtf.HisEns.Clear(); // Master table data .
                                rtf.EnsDataDtls.Clear(); //  Schedule data .

                                //  Find the master table data .
                                rtf.HisGEEntity = new GEEntity(this.rptGe.ClassID);
                                rtf.HisGEEntity.Row = rptGe.Row;

                                //  The work on each node is added to the Report Engine .
                                rtf.AddEn(this.HisWork);
                                rtf.ensStrs += ".ND" + this.HisNode.NodeID;

                                // The current work的Dtl  Data put to the .
                                ArrayList al = this.HisWork.GetDtlsDatasOfArrayList();
                                foreach (Entities ens in al)
                                    rtf.AddDtlEns(ens);
                                #endregion  The data in   Documents Engine .

                                #region  Generate documents 

                                paths = file.Split('_');
                                path = paths[0] + "/" + paths[1] + "/" + paths[2] + "/";
                                string billUrl = "/DataUser/Bill/" + path + file;
                                if (func.HisBillFileType == BillFileType.PDF)
                                {
                                    billUrl = billUrl.Replace(".doc", ".pdf");
                                    billInfo += "<img src='" + VirPath + "WF/Img/FileType/PDF.gif' /><a href='" + billUrl + "' target=_blank >" + func.Name + "</a>";
                                }
                                else
                                {
                                    billInfo += "<img src='" + VirPath + "WF/Img/FileType/doc.gif' /><a href='" + billUrl + "' target=_blank >" + func.Name + "</a>";
                                }

                                path = BP.WF.Glo.FlowFileBill + year + "\\" + this.ExecerDeptNo + "\\" + func.No + "\\";
                                path = AppDomain.CurrentDomain.BaseDirectory + path;
                                if (System.IO.Directory.Exists(path) == false)
                                    System.IO.Directory.CreateDirectory(path);

                                rtf.MakeDoc(func.Url + ".rtf",
                                    path, file, func.ReplaceVal, false);
                                #endregion

                                #region  Converted into pdf.
                                if (func.HisBillFileType == BillFileType.PDF)
                                {
                                    string rtfPath = path + file;
                                    string pdfPath = rtfPath.Replace(".doc", ".pdf");
                                    try
                                    {
                                        Glo.Rtf2PDF(rtfPath, pdfPath);
                                    }
                                    catch (Exception ex)
                                    {
                                        this.addMsg("RptError", " Generate reports data errors :" + ex.Message);
                                    }
                                }
                                #endregion

                                #region  Save the document 
                                Bill bill = new Bill();
                                bill.MyPK = this.HisWork.FID + "_" + this.HisWork.OID + "_" + this.HisNode.NodeID + "_" + func.No;
                                bill.FID = this.HisWork.FID;
                                bill.WorkID = this.HisWork.OID;
                                bill.FK_Node = this.HisNode.NodeID;
                                bill.FK_Dept = this.ExecerDeptNo;
                                bill.FK_Emp = this.Execer;
                                bill.Url = billUrl;
                                bill.RDT = DataType.CurrentDataTime;
                                bill.FullPath = path + file;
                                bill.FK_NY = DataType.CurrentYearMonth;
                                bill.FK_Flow = this.HisNode.FK_Flow;
                                bill.FK_BillType = func.FK_BillType;
                                bill.FK_Flow = this.HisNode.FK_Flow;
                                bill.Emps = this.rptGe.GetValStrByKey("Emps");
                                bill.FK_Starter = this.rptGe.GetValStrByKey("Rec");
                                bill.StartDT = this.rptGe.GetValStrByKey("RDT");
                                bill.Title = this.rptGe.GetValStrByKey("Title");
                                bill.FK_Dept = this.rptGe.GetValStrByKey("FK_Dept");
                                try
                                {
                                    bill.Insert();
                                }
                                catch
                                {
                                    bill.Update();
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                BP.WF.DTS.InitBillDir dir = new BP.WF.DTS.InitBillDir();
                                dir.Do();
                                path = BP.WF.Glo.FlowFileBill + year + "\\" + this.ExecerDeptNo + "\\" + func.No + "\\";
                                string msgErr = "@" + string.Format(" Failed to generate documents , Please allow administrators to check the directory settings ") + "[" + BP.WF.Glo.FlowFileBill + "].@Err:" + ex.Message + " @File=" + file + " @Path:" + path;
                                billInfo += "@<font color=red>" + msgErr + "</font>";
                                throw new Exception(msgErr + "@ Other Information :" + ex.Message);
                            }
                        }

                    } // end  Generation cycle documents .

                    if (billInfo != "")
                        billInfo = "@" + billInfo;
                    this.addMsg(SendReturnMsgFlag.BillInfo, billInfo);
                }
                #endregion

                #region  Execution Cc .
                // Execution Cc .
                CCWork cc = new CCWork(this);

                DBAccess.DoTransactionCommit(); // Commit the transaction .
                #endregion  The main business logic processing .

                #region  Send a successful event processing .
                try
                {
                    // After adjusting the incident sent successfully , The argument to go .
                    if (this.SendHTOfTemp != null)
                    {
                        foreach (string key in this.SendHTOfTemp.Keys)
                        {
                            if (rptGe.Row.ContainsKey(key) == true)
                                this.rptGe.Row[key] = this.SendHTOfTemp[key].ToString();
                            else
                                this.rptGe.Row.Add(key, this.SendHTOfTemp[key].ToString());
                        }
                    }

                    // Performing transmission .
                    string sendSuccess = this.HisFlow.DoFlowEventEntity(EventListOfNode.SendSuccess,
                        this.HisNode, this.rptGe, null, this.HisMsgObjs);

                    //string SendSuccess = this.HisNode.MapData.FrmEvents.DoEventNode(EventListOfNode.SendSuccess, this.HisWork);
                    if (sendSuccess != null)
                        this.addMsg(SendReturnMsgFlag.SendSuccessMsg, sendSuccess);
                }
                catch (Exception ex)
                {
                    this.addMsg(SendReturnMsgFlag.SendSuccessMsgErr, ex.Message);
                }
                #endregion  Send a successful event processing .

                #region  Process messages sent after the success tips 
                if (this.HisNode.HisTurnToDeal == TurnToDeal.SpecMsg)
                {
                    string msgOfSend = this.HisNode.TurnToDealDoc;
                    if (msgOfSend.Contains("@"))
                    {
                        Attrs attrs = this.HisWork.EnMap.Attrs;
                        foreach (Attr attr in attrs)
                        {
                            if (msgOfSend.Contains("@") == false)
                                continue;
                            msgOfSend = msgOfSend.Replace("@" + attr.Key, this.HisWork.GetValStrByKey(attr.Key));
                        }
                    }

                    if (msgOfSend.Contains("@") == true)
                    {
                        /* Description There are some variables in the system running inside .*/
                        string msgOfSendText = msgOfSend.Clone() as string;
                        foreach (SendReturnObj item in this.HisMsgObjs)
                        {
                            if (string.IsNullOrEmpty(item.MsgFlag))
                                continue;

                            if (msgOfSend.Contains("@") == false)
                                continue;

                            msgOfSendText = msgOfSendText.Replace("@" + item.MsgFlag, item.MsgOfText);

                            if (item.MsgOfHtml != null)
                                msgOfSend = msgOfSend.Replace("@" + item.MsgFlag, item.MsgOfHtml);
                            else
                                msgOfSend = msgOfSend.Replace("@" + item.MsgFlag, item.MsgOfText);
                        }

                        this.HisMsgObjs.OutMessageHtml = msgOfSend;
                        this.HisMsgObjs.OutMessageText = msgOfSendText;
                    }
                    else
                    {
                        this.HisMsgObjs.OutMessageHtml = msgOfSend;
                        this.HisMsgObjs.OutMessageText = msgOfSend;
                    }

                    //return msgOfSend;
                }
                #endregion  Send a successful event processing .

                #region  If you need to jump .
                if (town != null)
                {
                    if (this.town.HisNode.HisRunModel == RunModel.SubThread && this.town.HisNode.HisRunModel == RunModel.SubThread)
                    {
                        this.addMsg(SendReturnMsgFlag.VarToNodeID, town.HisNode.NodeID.ToString(), town.HisNode.NodeID.ToString(), SendReturnMsgType.SystemMsg);
                        this.addMsg(SendReturnMsgFlag.VarToNodeName, town.HisNode.Name, town.HisNode.Name, SendReturnMsgType.SystemMsg);
                    }

#warning  If this is set automatically jump , Now removed . 2014-11-07.
                    //if (town.HisNode.HisDeliveryWay == DeliveryWay.ByPreviousOperSkip)
                    //{
                    //    town.NodeSend();
                    //    this.HisMsgObjs = town.HisMsgObjs;
                    //}
                }
                #endregion  If you need to jump .

                #region  Delete the message has been sent , Those messages have become a spam .
                if (Glo.IsEnableSysMessage == true)
                {
                    Paras ps = new Paras();
                    ps.SQL = "DELETE FROM Sys_SMS WHERE MsgFlag=" + SystemConfig.AppCenterDBVarStr + "MsgFlag";
                    ps.Add("MsgFlag", "WKAlt" + this.HisNode.NodeID + "_" + this.WorkID);
                    BP.DA.DBAccess.RunSQL(ps);
                }
                #endregion

                #region  Flag setting process .
                if (this.HisNode.IsStartNode)
                {
                    if (this.rptGe.PWorkID != 0 && this.HisGenerWorkFlow.PWorkID == 0)
                    {
                        BP.WF.Dev2Interface.SetParentInfo(this.HisFlow.No, this.WorkID, this.rptGe.PFlowNo, this.rptGe.PWorkID, this.rptGe.PNodeID, this.rptGe.PEmp);

                        // Write track,  Call the parent process .
                        Node pND = new Node(rptGe.PNodeID);
                        Int64 fid = 0;
                        if (pND.HisNodeWorkType == NodeWorkType.SubThreadWork)
                        {
                            GenerWorkFlow gwf = new GenerWorkFlow(this.rptGe.PWorkID);
                            fid = gwf.FID;
                        }

                        string paras = "@SubFlowNo=" + this.HisFlow.No + "@SubWorkID=" + this.WorkID;

                        Glo.AddToTrack(ActionType.StartChildenFlow, rptGe.PFlowNo, rptGe.PWorkID, fid, pND.NodeID, pND.Name,
                            WebUser.No, WebUser.Name,
                            pND.NodeID, pND.Name, WebUser.No, WebUser.Name,
                            "<a href='/WF/WFRpt.aspx?FK_Flow=" + this.HisFlow.No + "&WorkID=" + this.WorkID + "' target=_blank > Open sub-processes </a>", paras);
                    }
                    else if (SystemConfig.IsBSsystem == true)
                    {
                        /* In the case of BS System */
                        string pflowNo = BP.Sys.Glo.Request.QueryString["PFlowNo"];
                        if (string.IsNullOrEmpty(pflowNo) == false)
                        {
                            string pWorkID = BP.Sys.Glo.Request.QueryString["PWorkID"];
                            string pNodeID = BP.Sys.Glo.Request.QueryString["PNodeID"];
                            string pEmp = BP.Sys.Glo.Request.QueryString["PEmp"];

                            //  Set parent process relationship .
                            BP.WF.Dev2Interface.SetParentInfo(this.HisFlow.No, this.WorkID, pflowNo, Int64.Parse(pWorkID), int.Parse(pNodeID), pEmp);

                            // Write track,  Call the parent process .
                            Node pND = new Node(pNodeID);
                            Int64 fid = 0;
                            if (pND.HisNodeWorkType == NodeWorkType.SubThreadWork)
                            {
                                GenerWorkFlow gwf = new GenerWorkFlow(Int64.Parse(pWorkID));
                                fid = gwf.FID;
                            }
                            string paras = "@SubFlowNo=" + this.HisFlow.No + "@SubWorkID=" + this.WorkID;
                            Glo.AddToTrack(ActionType.StartChildenFlow, pflowNo, Int64.Parse(pWorkID), fid, pND.NodeID, pND.Name, WebUser.No, WebUser.Name,
                                pND.NodeID, pND.Name, WebUser.No, WebUser.Name,
                                "<a href='/WF/WFRpt.aspx?FK_Flow=" + this.HisFlow.No + "&WorkID=" + this.WorkID + "' target=_blank > Open sub-processes </a>", paras);
                        }
                    }
                }
                #endregion  Flag setting process .

                // Returns this object .
                return this.HisMsgObjs;
            }
            catch (Exception ex)
            {

                this.WhenTranscactionRollbackError(ex);
                DBAccess.DoTransactionRollback();
                throw new Exception("Message:" + ex.Message + " StackTrace:" + ex.StackTrace);
            }
        }
        /// <summary>
        ///  Failure to submit the information by hand rollback .
        /// </summary>
        /// <param name="ex"></param>
        private void WhenTranscactionRollbackError(Exception ex)
        {
            /* In the case of submitting false , Rollback data .*/

            #region  If the point is the same below shunt failed to send the form to send an error occurs again 
            if (this.town != null
                && this.town.HisNode.HisNodeWorkType == NodeWorkType.SubThreadWork
                && this.town.HisNode.HisSubThreadType == SubThreadType.SameSheet)
            {
                /* If the child thread */
                DBAccess.RunSQL("DELETE FROM WF_GenerWorkerList WHERE FID=" + this.WorkID + " AND FK_Node=" + this.town.HisNode.NodeID);
                // Delete sub-thread data .
                DBAccess.RunSQL("DELETE FROM " + this.town.HisWork.EnMap.PhysicsTable + " WHERE FID=" + this.WorkID);
            }
            #endregion  If the point is the same below shunt failed to send the form to send an error occurs again 

            try
            {
                // Delete the log occurred .
                DBAccess.RunSQL("DELETE FROM ND" + int.Parse(this.HisFlow.No) + "Track WHERE WorkID=" + this.WorkID +
                                " AND NDFrom=" + this.HisNode.NodeID + " AND ActionType=" + (int)ActionType.Forward);

                //  Delete assessment information .
                this.DealEvalUn();

                //  The state is set to come back to work .
                if (this.HisNode.IsStartNode)
                {
                    ps = new Paras();
                    ps.SQL = "UPDATE " + this.HisFlow.PTable + " SET WFState=" + (int)WFState.Runing + " WHERE OID=" +
                             dbStr + "OID ";
                    ps.Add(GERptAttr.OID, this.WorkID);
                    DBAccess.RunSQL(ps);
                    //  this.HisWork.Update(GERptAttr.WFState, (int)WFState.Runing);
                }

                //  The status of the process of setting back .
                GenerWorkFlow gwf = new GenerWorkFlow();
                gwf.WorkID = this.WorkID;
                if (gwf.RetrieveFromDBSources() == 0)
                    return;

                if (gwf.WFState != 0 || gwf.FK_Node != this.HisNode.NodeID)
                {
                    /*  If two of which have a change .*/
                    gwf.FK_Node = this.HisNode.NodeID;
                    gwf.NodeName = this.HisNode.Name;
                    gwf.WFState = WFState.Runing;
                    gwf.Update();
                }

                // Perform data .
                ps = new Paras();
                ps.SQL = "UPDATE WF_GenerWorkerlist SET IsPass=0 WHERE FK_Emp=" + dbStr + "FK_Emp AND WorkID=" + dbStr +
                         "WorkID AND FK_Node=" + dbStr + "FK_Node ";
                ps.AddFK_Emp();
                ps.Add("WorkID", this.WorkID);
                ps.Add("FK_Node", this.HisNode.NodeID);
                DBAccess.RunSQL(ps);

                Node startND = this.HisNode.HisFlow.HisStartNode;
                StartWork wk = startND.HisWork as StartWork;
                switch (startND.HisNodeWorkType)
                {
                    case NodeWorkType.StartWorkFL:
                    case NodeWorkType.WorkFL:
                        break;
                    default:
                        /*
                          To consider removing WFState  Problem node fields .
                         */
                        ////  The state began loading node set back .
                        //DBAccess.RunSQL("UPDATE " + wk.EnMap.PhysicsTable + " SET WFState=0 WHERE OID="+this.WorkID+" OR OID="+this);
                        //wk.OID = this.WorkID;
                        //int i =wk.RetrieveFromDBSources();
                        //if (wk.WFState == WFState.Complete)
                        //{
                        //    wk.Update("WFState", (int)WFState.Runing);
                        //}
                        break;
                }
                Nodes nds = this.HisNode.HisToNodes;
                foreach (Node nd in nds)
                {
                    if (nd.NodeID == this.HisNode.NodeID)
                        continue;

                    Work mwk = nd.HisWork;
                    if (mwk.EnMap.PhysicsTable == this.HisFlow.PTable
                        || mwk.EnMap.PhysicsTable == this.HisWork.EnMap.PhysicsTable)
                        continue;

                    mwk.OID = this.WorkID;
                    mwk.DirectDelete();
                }
                this.HisFlow.DoFlowEventEntity(EventListOfNode.SendError, this.HisNode, this.HisWork, null);

            }
            catch (Exception ex1)
            {
                if (this.rptGe != null)
                    this.rptGe.CheckPhysicsTable();
                throw new Exception(ex.Message + "@ Rollback failed to send data errors :" + ex1.Message + "@ There may have been automatically fix system errors , Please re-executed once .");
            }
        }
        #endregion

        #region  User to the variable 
        public GenerWorkerLists HisWorkerLists = null;
        private GenerWorkFlow _HisGenerWorkFlow;
        public GenerWorkFlow HisGenerWorkFlow
        {
            get
            {
                if (_HisGenerWorkFlow == null)
                {
                    _HisGenerWorkFlow = new GenerWorkFlow(this.WorkID);
                    SendNodeWFState = _HisGenerWorkFlow.WFState; // Set before sending node status .
                }
                return _HisGenerWorkFlow;
            }
            set
            {
                _HisGenerWorkFlow = value;
            }
        }
        private Int64 _WorkID = 0;
        public Int64 WorkID
        {
            get
            {
                return _WorkID;
            }
            set
            {
                _WorkID = value;
            }
        }
        #endregion

        /// <summary>
        /// 追加绑定表单的值,优先进行解析 变量格式为:@表名[字段]
        /// </summary>
        /// <param name="wk"></param>
        /// <param name="titleRole"></param>
        /// <returns></returns>
        public static string FormatTitleWithBindingFrm(Work wk, string titleRole)
        {
            var frms = wk.HisNode.HisFrms;
            
            foreach (Frm frm in frms)
            {
                string datasql = string.Format("select * from {0} where OID ={1}", frm.PTable, wk.OID);
                DataTable dt = DBAccess.RunSQLReturnTable(datasql);
                if (dt.Rows.Count <= 0) break;

                foreach (DataColumn col in dt.Columns)
                {
                    string key = col.ColumnName;
                    string regex = string.Format("@{0}[{1}]", frm.No.ToUpper(), key.ToUpper());
                    object val = dt.Rows[0][key];
                    string strval = val == null ? "[null]" : val.ToString();
                    titleRole = titleRole.Replace(regex, strval);
                }
            }
            return titleRole;
        }

        /// <summary>
        ///  Generation title 
        /// </summary>
        /// <param name="wk"></param>
        /// <param name="emp"></param>
        /// <param name="rdt"></param>
        /// <returns></returns>
        public static string GenerTitle(Flow fl, Work wk, Emp emp, string rdt)
        {
            string titleRole = fl.TitleRole.Clone() as string;
            if (string.IsNullOrEmpty(titleRole))
            {
                //  In order to maintain ccflow4.5 Compatible , Obtained from the starting node properties in .
                Attr myattr = wk.EnMap.Attrs.GetAttrByKey("Title");
                if (myattr == null)
                    myattr = wk.EnMap.Attrs.GetAttrByKey("Title");

                if (myattr != null)
                    titleRole = myattr.DefaultVal.ToString();

                if (string.IsNullOrEmpty(titleRole) || titleRole.Contains("@") == false)
                    titleRole = "@WebUser.FK_DeptName-@WebUser.No,@WebUser.Name Launch at @RDT.";
            }


            titleRole = titleRole.Replace("@WebUser.No", emp.No);
            titleRole = titleRole.Replace("@WebUser.Name", emp.Name);
            titleRole = titleRole.Replace("@WebUser.FK_DeptName", emp.FK_DeptText);
            titleRole = titleRole.Replace("@WebUser.FK_Dept", emp.FK_Dept);
            titleRole = titleRole.Replace("@RDT", rdt);
            if (titleRole.Contains("@"))
            {
                Thread.Sleep(1000);
                titleRole = FormatTitleWithBindingFrm(wk, titleRole);

                Attrs attrs = wk.EnMap.Attrs;

                //  Replace foreign key priority .
                foreach (Attr attr in attrs)
                {
                    
                    if (attr.IsRefAttr == false)
                        continue;
                    titleRole = titleRole.Replace("@" + attr.Key, wk.GetValStrByKey(attr.Key));
                }

                // In considering the replacement of other fields .
                foreach (Attr attr in attrs)
                {
                    

                    if (attr.IsRefAttr == true)
                        continue;
                    titleRole = titleRole.Replace("@" + attr.Key, wk.GetValStrByKey(attr.Key));
                }
            }
            titleRole = titleRole.Replace('~', '-');
            titleRole = titleRole.Replace("'", "]");

            if (titleRole.Contains("@"))
            {
                /* If you do not replace clean , User field to consider is misspelled */
                throw new Exception("@ Please check whether the field is misspelled , The title has not been replaced by a variable down . @" + titleRole);
            }
            wk.SetValByKey("Title", titleRole);
            return titleRole;
        }
        /// <summary>
        ///  Generation title 
        /// </summary>
        /// <param name="wk"></param>
        /// <returns></returns>
        public static string GenerTitle(Flow fl, Work wk)
        {

            string titleRole = fl.TitleRole.Clone() as string;
            
            if (string.IsNullOrEmpty(titleRole))
            {
                //  In order to maintain ccflow4.5 Compatible , Obtained from the starting node properties in .
                Attr myattr = wk.EnMap.Attrs.GetAttrByKey("Title");
                if (myattr == null)
                    myattr = wk.EnMap.Attrs.GetAttrByKey("Title");

                if (myattr != null)
                    titleRole = myattr.DefaultVal.ToString();

                if (string.IsNullOrEmpty(titleRole) || titleRole.Contains("@") == false)
                    titleRole = "@WebUser.FK_DeptName-@WebUser.No,@WebUser.Name Launch at @RDT.";
            }

            if (titleRole == "@OutPara")
                titleRole = "@WebUser.FK_DeptName-@WebUser.No,@WebUser.Name Launch at @RDT.";


            titleRole = titleRole.Replace("@WebUser.No", wk.Rec);
            titleRole = titleRole.Replace("@WebUser.Name", wk.RecText);
            titleRole = titleRole.Replace("@WebUser.FK_DeptName", wk.RecOfEmp.FK_DeptText);
            titleRole = titleRole.Replace("@WebUser.FK_Dept", wk.RecOfEmp.FK_Dept);
            titleRole = titleRole.Replace("@RDT", wk.RDT);
            if (titleRole.Contains("@"))
            {
                Thread.Sleep(1000);
                titleRole = FormatTitleWithBindingFrm(wk, titleRole);

                Attrs attrs = wk.EnMap.Attrs;
               
                //  Replace foreign key priority , Because the length of the field in the foreign key of the text is relatively long .
                foreach (Attr attr in attrs)
                {
                    if (titleRole.Contains("@") == false)
                        break;
                    if (attr.IsRefAttr == false)
                        continue;
                    titleRole = titleRole.Replace("@" + attr.Key, wk.GetValStrByKey(attr.Key));
                }

                // In considering the replacement of other fields .
                foreach (Attr attr in attrs)
                {
                    if (titleRole.Contains("@") == false)
                        break;

                    if (attr.IsRefAttr == true)
                        continue;
                    titleRole = titleRole.Replace("@" + attr.Key, wk.GetValStrByKey(attr.Key));
                }

                
                
            }
            titleRole = titleRole.Replace('~', '-');
            titleRole = titleRole.Replace("'", "]");

            //  Settings for the current job title.
            wk.SetValByKey("Title", titleRole);
            return titleRole;
        }
        /// <summary>
        ///  Generation title 
        /// </summary>
        /// <param name="fl"></param>
        /// <param name="wk"></param>
        /// <returns></returns>
        public static string GenerTitle(Flow fl, GERpt wk)
        {
            string titleRole = fl.TitleRole.Clone() as string;
            if (string.IsNullOrEmpty(titleRole))
            {
                //  In order to maintain ccflow4.5 Compatible , Obtained from the starting node properties in .
                Attr myattr = wk.EnMap.Attrs.GetAttrByKey("Title");
                if (myattr == null)
                    myattr = wk.EnMap.Attrs.GetAttrByKey("Title");

                if (myattr != null)
                    titleRole = myattr.DefaultVal.ToString();

                if (string.IsNullOrEmpty(titleRole) || titleRole.Contains("@") == false)
                    titleRole = "@WebUser.FK_DeptName-@WebUser.No,@WebUser.Name Launch at @RDT.";
            }

            if (titleRole == "@OutPara")
                titleRole = "@WebUser.FK_DeptName-@WebUser.No,@WebUser.Name Launch at @RDT.";


            titleRole = titleRole.Replace("@WebUser.No", wk.FlowStarter);
            titleRole = titleRole.Replace("@WebUser.Name", WebUser.Name);
            titleRole = titleRole.Replace("@WebUser.FK_DeptName", WebUser.FK_DeptName);
            titleRole = titleRole.Replace("@WebUser.FK_Dept", WebUser.FK_Dept);
            titleRole = titleRole.Replace("@RDT", wk.FlowStartRDT);
            if (titleRole.Contains("@"))
            {



                Attrs attrs = wk.EnMap.Attrs;

                //  Replace foreign key priority , Because the length of the field in the foreign key of the text is relatively long .
                foreach (Attr attr in attrs)
                {
                    if (titleRole.Contains("@") == false)
                        break;
                    if (attr.IsRefAttr == false)
                        continue;
                    titleRole = titleRole.Replace("@" + attr.Key, wk.GetValStrByKey(attr.Key));
                }

                // In considering the replacement of other fields .
                foreach (Attr attr in attrs)
                {
                    if (titleRole.Contains("@") == false)
                        break;

                    if (attr.IsRefAttr == true)
                        continue;
                    titleRole = titleRole.Replace("@" + attr.Key, wk.GetValStrByKey(attr.Key));
                }
            }
            titleRole = titleRole.Replace('~', '-');
            titleRole = titleRole.Replace("'", "]");

            //  Settings for the current job title.
            wk.SetValByKey("Title", titleRole);
            return titleRole;
        }
        public static string GenerTitle_Del(Work wk)
        {
            //  Generation title .
            Attr myattr = wk.EnMap.Attrs.GetAttrByKey("Title");
            if (myattr == null)
                myattr = wk.EnMap.Attrs.GetAttrByKey("Title");

            string titleRole = "";
            if (myattr != null)
                titleRole = myattr.DefaultVal.ToString();

            if (string.IsNullOrEmpty(titleRole) || titleRole.Contains("@") == false)
                titleRole = "@WebUser.FK_DeptName-@WebUser.No,@WebUser.Name Launch at @RDT.";

            titleRole = titleRole.Replace("@WebUser.No", wk.Rec);
            titleRole = titleRole.Replace("@WebUser.Name", wk.RecText);
            titleRole = titleRole.Replace("@WebUser.FK_DeptName", wk.RecOfEmp.FK_DeptText);
            titleRole = titleRole.Replace("@WebUser.FK_Dept", wk.RecOfEmp.FK_Dept);
            titleRole = titleRole.Replace("@RDT", wk.RDT);
            if (titleRole.Contains("@"))
            {
                Attrs attrs = wk.EnMap.Attrs;

                //  Replace foreign key priority .
                foreach (Attr attr in attrs)
                {
                    if (titleRole.Contains("@") == false)
                        break;
                    if (attr.IsRefAttr == false)
                        continue;
                    titleRole = titleRole.Replace("@" + attr.Key, wk.GetValStrByKey(attr.Key));
                }

                // In considering the replacement of other fields .
                foreach (Attr attr in attrs)
                {
                    if (titleRole.Contains("@") == false)
                        break;

                    if (attr.IsRefAttr == true)
                        continue;
                    titleRole = titleRole.Replace("@" + attr.Key, wk.GetValStrByKey(attr.Key));
                }
            }
            titleRole = titleRole.Replace('~', '-');
            titleRole = titleRole.Replace("'", "]");
            wk.SetValByKey("Title", titleRole);
            return titleRole;
        }
        public GERpt rptGe = null;
        private void InitStartWorkDataV2()
        {
            /* If this is the beginning of the process to determine the process is not being lifted , If it is necessary to write logs to the parent process .*/
            if (SystemConfig.IsBSsystem)
            {
                string fk_nodeFrom = BP.Sys.Glo.Request.QueryString["FromNode"];
                if (string.IsNullOrEmpty(fk_nodeFrom) == false)
                {
                    Node ndFrom = new Node(int.Parse(fk_nodeFrom));
                    string PWorkID = BP.Sys.Glo.Request.QueryString["PWorkID"];
                    if (string.IsNullOrEmpty(PWorkID))
                        PWorkID = BP.Sys.Glo.Request.QueryString["PWorkID"];

                    string pTitle = DBAccess.RunSQLReturnStringIsNull("SELECT Title FROM  ND" + int.Parse(ndFrom.FK_Flow) + "01 WHERE OID=" + PWorkID, "");

                    //// Record the current flow is from the transfer .
                    //  this.AddToTrack(ActionType.StartSubFlow, WebUser.No,
                    //  WebUser.Name, ndFrom.NodeID, ndFrom.FlowName + "\t\n" + ndFrom.FlowName, " Is the parent process (" + ndFrom.FlowName + ":" + pTitle + ") From the transfer .");

                    // Records from the parent process is transferred .
                    BP.WF.Dev2Interface.WriteTrack(this.HisFlow.No, this.HisNode.NodeID, this.WorkID, 0,
                        " Launch by {" + ndFrom.FlowName + "}  , Sponsor :" + this.ExecerName, ActionType.CallChildenFlow,
                        "@PWorkID=" + PWorkID + "@PFlowNo=" + ndFrom.HisFlow.No, " Sponsored sub-processes :" + this.HisFlow.Name, null);
                }
            }

            /*  Workflow generates a start record . */
            GenerWorkFlow gwf = new GenerWorkFlow();
            gwf.WorkID = this.HisWork.OID;
            int srcNum = gwf.RetrieveFromDBSources();
            if (srcNum == 0)
            {
                gwf.WFState = WFState.Runing;
            }
            else
            {
                if (gwf.WFState == WFState.Blank)
                    gwf.WFState = WFState.Runing;

                SendNodeWFState = gwf.WFState; // Set before sending node status .
            }

            #region  Setting Process title .
            if (this.title == null)
            {
                if (this.HisFlow.TitleRole == "@OutPara")
                {
                    /* If the external parameters ,*/
                    gwf.Title = DBAccess.RunSQLReturnString("SELECT Title FROM " + this.HisFlow.PTable + " WHERE OID=" + this.WorkID);
                    if (string.IsNullOrEmpty(gwf.Title))
                        gwf.Title = this.Execer + "," + this.ExecerName + " Launch at:" + DataType.CurrentDataTimeCN + ".";
                    //throw new Exception(" You set the rules for the external flow generated title came parameters , But you 岋 material when creating a blank , Process is an empty title .");
                }
                else
                {
                    gwf.Title = WorkNode.GenerTitle(this.HisFlow, this.HisWork);
                }
            }
            else
            {
                gwf.Title = this.title;
            }

            // Process title .
            this.rptGe.Title = gwf.Title;
            #endregion  Setting Process title .

            if (string.IsNullOrEmpty(rptGe.BillNo))
            {
                // Processing Document Number .
                string billNo = this.HisFlow.BillNoFormat.Clone() as string;
                if (string.IsNullOrEmpty(billNo) == false)
                {
                    billNo = BP.WF.Glo.GenerBillNo(billNo, this.WorkID, this.rptGe, this.HisFlow.PTable);
                    gwf.BillNo = billNo;
                    this.rptGe.BillNo = billNo;
                }
            }
            else
            {
                gwf.BillNo = rptGe.BillNo;
            }

            this.HisWork.SetValByKey("Title", gwf.Title);
            gwf.RDT = this.HisWork.RDT;
            gwf.Starter = this.Execer;
            gwf.StarterName = this.ExecerName;
            gwf.FK_Flow = this.HisNode.FK_Flow;
            gwf.FlowName = this.HisNode.FlowName;
            gwf.FK_FlowSort = this.HisNode.HisFlow.FK_FlowSort;
            gwf.FK_Node = this.HisNode.NodeID;
            gwf.NodeName = this.HisNode.Name;
            gwf.FK_Dept = this.HisWork.RecOfEmp.FK_Dept;
            gwf.DeptName = this.HisWork.RecOfEmp.FK_DeptText;
            if (Glo.IsEnablePRI)
            {
                try
                {
                    gwf.PRI = this.HisWork.GetValIntByKey("PRI");
                }
                catch (Exception ex)
                {
                    this.HisNode.RepareMap();
                }
            }

            if (this.HisFlow.HisTimelineRole == TimelineRole.ByFlow)
            {
                try
                {
                    gwf.SDTOfFlow = this.HisWork.GetValStrByKey(WorkSysFieldAttr.SysSDTOfFlow);
                }
                catch (Exception ex)
                {
                    Log.DefaultLogWriteLineError(" Process design may be wrong , Get the starting node {" + gwf.Title + "} The whole process should be completed by the time an error , Contains SysSDTOfFlow Field ?  Exception Information :" + ex.Message);
                    /* Get the whole process should start node completion time error , Contains SysSDTOfFlow Field ? .*/
                    if (this.HisWork.EnMap.Attrs.Contains(WorkSysFieldAttr.SysSDTOfFlow) == false)
                        throw new Exception(" Process design errors , You set the aging process attributes ｛ Press the Start node form SysSDTOfFlow Field calculations ｝, However, the form does not contain field starting node  SysSDTOfFlow ,  System error messages :" + ex.Message);
                    throw new Exception(" Starting node data initialization error :" + ex.Message);
                }
            }

            // Adding two parameters . 2013-02-17
            if (gwf.PWorkID != 0)
            {
                this.rptGe.PWorkID = gwf.PWorkID;
                this.rptGe.PFlowNo = gwf.PFlowNo;
                this.rptGe.PNodeID = gwf.PNodeID;
            }
            else
            {
                gwf.PWorkID = this.rptGe.PWorkID;
                gwf.PFlowNo = this.rptGe.PFlowNo;
                gwf.PNodeID = this.rptGe.PNodeID;
            }

            //  Generate FlowNote
            string note = this.HisFlow.FlowNoteExp.Clone() as string;
            if (string.IsNullOrEmpty(note) == false)
                note = BP.WF.Glo.DealExp(note, this.rptGe, null);
            this.rptGe.FlowNote = note;
            gwf.FlowNote = note;


            if (srcNum == 0)
                gwf.DirectInsert();
            else
                gwf.DirectUpdate();

            StartWork sw = (StartWork)this.HisWork;

            #region  Set up   HisGenerWorkFlow

            this.HisGenerWorkFlow = gwf;

            #endregion HisCHOfFlow

            #region   Workers began to produce , Able to perform their staff .
            GenerWorkerList wl = new GenerWorkerList();
            wl.WorkID = this.HisWork.OID;
            wl.FK_Node = this.HisNode.NodeID;
            wl.FK_Emp = this.Execer;
            wl.Delete();

            wl.FK_NodeText = this.HisNode.Name;
            wl.FK_EmpText = this.ExecerName;
            wl.FK_Flow = this.HisNode.FK_Flow;
            wl.FK_Dept = this.ExecerDeptNo;
            wl.WarningDays = this.HisNode.WarningDays;
            wl.SDT = DataType.CurrentDataTime;
            wl.DTOfWarning = DataType.CurrentData;
            wl.RDT = DataType.CurrentDataTime;

            try
            {
                wl.Save();
            }
            catch
            {
                wl.CheckPhysicsTable();
                wl.Update();
            }
            #endregion
        }

        /// <summary>
        ///  The data from the current node to perform the work of copy到Rpt Go inside .
        /// </summary>
        public void DoCopyCurrentWorkDataToRpt()
        {
            /*  If an agreement on the return of two tables ..*/
            //  Increase the current staff to go inside .
            string str = rptGe.GetValStrByKey(GERptAttr.FlowEmps);
            if (Glo.UserInfoShowModel == UserInfoShowModel.UserIDOnly)
            {
                if (str.Contains("@" + this.Execer) == false)
                    rptGe.SetValByKey(GERptAttr.FlowEmps, str + "@" + this.Execer);
            }

            if (Glo.UserInfoShowModel == UserInfoShowModel.UserNameOnly)
            {
                if (str.Contains("@" + WebUser.Name) == false)
                    rptGe.SetValByKey(GERptAttr.FlowEmps, str + "@" + this.ExecerName);
            }

            if (Glo.UserInfoShowModel == UserInfoShowModel.UserIDUserName)
            {
                if (str.Contains("@" + this.Execer + "," + this.ExecerName) == false)
                    rptGe.SetValByKey(GERptAttr.FlowEmps, str + "@" + this.Execer + "," + this.ExecerName);
            }

            rptGe.FlowEnder = this.Execer;
            rptGe.FlowEnderRDT = DataType.CurrentDataTime;

            rptGe.FlowDaySpan = DataType.GetSpanDays(this.rptGe.GetValStringByKey(GERptAttr.FlowStartRDT), DataType.CurrentDataTime);
            if (this.HisNode.IsEndNode || this.IsStopFlow)
                rptGe.WFState = WFState.Complete;
            else
                rptGe.WFState = WFState.Runing;

            if (this.HisWork.EnMap.PhysicsTable == this.HisFlow.PTable)
            {
                rptGe.DirectUpdate();
            }
            else
            {
                /* Copy the current properties to rpt Table to go inside .*/
                DoCopyRptWork(this.HisWork);
                rptGe.DirectUpdate();
            }
        }
        /// <summary>
        ///  Perform data copy.
        /// </summary>
        /// <param name="fromWK"></param>
        public void DoCopyRptWork(Work fromWK)
        {
            foreach (Attr attr in fromWK.EnMap.Attrs)
            {
                switch (attr.Key)
                {
                    case BP.WF.Data.GERptAttr.FK_NY:
                    case BP.WF.Data.GERptAttr.FK_Dept:
                    case BP.WF.Data.GERptAttr.FlowDaySpan:
                    case BP.WF.Data.GERptAttr.FlowEmps:
                    case BP.WF.Data.GERptAttr.FlowEnder:
                    case BP.WF.Data.GERptAttr.FlowEnderRDT:
                    case BP.WF.Data.GERptAttr.FlowEndNode:
                    case BP.WF.Data.GERptAttr.FlowStarter:
                    case BP.WF.Data.GERptAttr.Title:
                    case BP.WF.Data.GERptAttr.WFSta:
                        continue;
                    default:
                        break;
                }

                object obj = fromWK.GetValByKey(attr.Key);
                if (obj == null)
                    continue;
                this.rptGe.SetValByKey(attr.Key, obj);
            }
            if (this.HisNode.IsStartNode)
                this.rptGe.SetValByKey("Title", fromWK.GetValByKey("Title"));
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
            AddToTrack(at, toEmp, toEmpName, toNDid, toNDName, msg, this.HisNode);
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
        public void AddToTrack(ActionType at, string toEmp, string toEmpName, int toNDid, string toNDName, string msg, Node ndFrom)
        {
            Track t = new Track();
            t.WorkID = this.HisWork.OID;
            t.FID = this.HisWork.FID;
            t.RDT = DataType.CurrentDataTimess;
            t.HisActionType = at;

            t.NDFrom = ndFrom.NodeID;
            t.NDFromT = ndFrom.Name;

            t.EmpFrom = this.Execer;
            t.EmpFromT = this.ExecerName;
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

            switch (at)
            {
                case ActionType.Forward:
                case ActionType.Start:
                case ActionType.UnSend:
                case ActionType.ForwardFL:
                case ActionType.ForwardHL:
                    // Determine whether there is focus field , If you have to put it into the log in .
                    if (this.HisNode.FocusField.Length > 1)
                    {
                        string exp = this.HisNode.FocusField;
                        if (this.rptGe != null)
                            exp = Glo.DealExp(exp, this.rptGe, null);
                        else
                            exp = Glo.DealExp(exp, this.HisWork, null);

                        t.Msg += exp;
                        if (t.Msg.Contains("@"))
                            Log.DebugWriteError("@ Node (" + this.HisNode.NodeID + " , " + this.HisNode.Name + ") Focus field is removed , Expressions for :" + this.HisNode.FocusField + "  The results for the replacement :" + t.Msg);
                    }
                    break;
                default:
                    break;
            }

            if (at == ActionType.Forward)
            {
                if (this.HisNode.IsFL)
                    at = ActionType.ForwardFL;
            }

            try
            {
                // t.MyPK = t.WorkID + "_" + t.FID + "_"  + t.NDFrom + "_" + t.NDTo +"_"+t.EmpFrom+"_"+t.EmpTo+"_"+ DateTime.Now.ToString("yyMMddHHmmss");
                t.Insert();
            }
            catch
            {
                t.CheckPhysicsTable();
            }
        }
        /// <summary>
        ///  Send a message to them 
        /// </summary>
        /// <param name="gwls"></param>
        public void SendMsgToThem(GenerWorkerLists gwls)
        {
            if (BP.WF.Glo.IsEnableSysMessage == false)
                return;

            //#region  Determine whether they can be sent .
            //bool isSendEmail = false;
            //bool isSendSMS = false;
            //MsgCtrl mc = this.HisNode.MsgCtrl;
            //switch (this.HisNode.MsgCtrl)
            //{
            //    case MsgCtrl.BySet:
            //        if (this.HisNode.MsgIsSend == false)
            //            return;
            //        if (this.HisNode.MsgMailEnable == false 
            //            && this.HisNode.MsgSMSEnable == false)
            //            return;

            //        isSendEmail = this.HisNode.MsgMailEnable;
            //        isSendSMS = this.HisNode.MsgSMSEnable;
            //        break;
            //    case MsgCtrl.ByFrmIsSendMsg:
            //        try
            //        {
            //            /* Taken from a form field parameters .*/
            //            if (this.HisWork.Row.ContainsKey("IsSendEmail") == true)
            //                isSendEmail = this.HisWork.GetValBooleanByKey("IsSendEmail");
            //            if (this.HisWork.Row.ContainsKey("IsSendSMS") == true)
            //                isSendSMS = this.HisWork.GetValBooleanByKey("IsSendSMS");

            //            if (isSendEmail == false || isSendSMS == false)
            //                return;
            //        }
            //        catch
            //        {
            //            if (this.HisWork.Row.ContainsKey("IsSendEmail") == false || this.HisWork.Row.ContainsKey("IsSendSMS") == false)
            //                throw new Exception(" Not ccform Lane received IsSendEmail, IsSendSMS  Parameters .");
            //        }
            //        break;
            //    case MsgCtrl.BySDK:
            //        try
            //        {
            //            if (this.HisWork.GetValBooleanByKey("IsSendMsg") == false)
            //                return;
            //        }
            //        catch
            //        {
            //            if (this.HisWork.Row.ContainsKey("IsSendMsg") == false)
            //                throw new Exception(" Not received IsSendMsg Parameters .");
            //        }
            //        break;
            //    default:
            //        break;
            //}
            //#endregion  Determine whether they can be sent .

            ////  Remove the template file .
            //string hostUrl = Glo.HostURL;
            //string mailDoc = this.HisNode.MsgMailDoc;
            //string mailEnd = "<a href='{0}'> Open the computer </a>, Address :{0}.";
            //string mailTitle = this.HisNode.MsgMailTitle;
            //string msgTemp = this.HisNode.MsgSMSDoc;

            //foreach (GenerWorkerList wl in gwls)
            //{
            //    if (wl.IsEnable == false)
            //        continue;

            //    //  Mail title .
            //    string title = "";
            //    if (string.IsNullOrEmpty(mailTitle))
            //        title = string.Format(" Process :{0}. The work :{1}, Sender :{2}, Title :{3}, You need to deal with .",this.HisNode.FlowName, wl.FK_NodeText, this.ExecerName, this.rptGe.Title);
            //    else
            //        title = Glo.DealExp(mailTitle, this.HisWork, null);

            //    // Message content .
            //    string sid = wl.FK_Emp + "_" + wl.WorkID + "_" + wl.FK_Node + "_" + wl.RDT;
            //    string url = hostUrl + "WF/Do.aspx?DoType=OF&SID=" + sid;
            //    url = url.Replace("//", "/");
            //    url = url.Replace("//", "/");
            //    mailDoc = Glo.DealExp(mailDoc, this.HisWork,null);
            //    mailDoc += "\t\n " + string.Format(mailEnd.Clone().ToString(), url);

            //    //  SMS information .
            //    if (string.IsNullOrEmpty(msgTemp) == true)
            //        msgTemp = " New Work " + this.rptGe.Title + " Sender :" + WebUser.No + "," + WebUser.Name + ", Process :" + this.HisFlow.Name;
            //    else
            //        msgTemp = Glo.DealExp(msgTemp, this.HisWork, null);


            //    BP.WF.Dev2Interface.Port_SendMsg(wl.FK_Emp, title, mailDoc,
            //        "WKAlt" + wl.FK_Node + "_" + wl.WorkID, BP.Sys.SMSMsgType.ToDo, wl.FK_Flow, wl.FK_Node, wl.WorkID, wl.FID);
            //}
        }
        /// <summary>
        ///  Process state before sending .
        /// </summary>
        private WFState SendNodeWFState = WFState.Blank;
        /// <summary>
        ///  Confluent nodes are completed ?
        /// </summary>
        private bool IsOverMGECheckStand = false;
        private bool _IsStopFlow = false;
        private bool IsStopFlow
        {
            get
            {
                return _IsStopFlow;
            }
            set
            {
                _IsStopFlow = value;
                if (_IsStopFlow == true)
                {
                    this.rptGe.WFState = WFState.Complete;
                    this.rptGe.Update("WFState", (int)WFState.Complete);
                }
            }
        }
        /// <summary>
        ///  Inspection 
        /// </summary>
        private void CheckCompleteCondition_IntCompleteEmps()
        {
            string sql = "SELECT FK_Emp,FK_EmpText FROM WF_GenerWorkerlist WHERE WorkID=" + this.WorkID + " AND IsEnable=1";
            DataTable dt = DBAccess.RunSQLReturnTable(sql);

            string emps = "@";
            string flowEmps = "@";
            foreach (DataRow dr in dt.Rows)
            {
                if (emps.Contains("@" + dr[0].ToString() + "@"))
                    continue;

                emps = emps + dr[0].ToString() + "@";
                flowEmps = flowEmps + dr[1] + "," + dr[0].ToString() + "@";
            }

            //  Give them an assignment .
            this.rptGe.FlowEmps = flowEmps;
            this.HisGenerWorkFlow.Emps = emps;
        }
        /// <summary>
        ///  Inspection process , Closing conditions nodes 
        /// </summary>
        /// <returns></returns>
        private void CheckCompleteCondition()
        {
            //  Perform initialization staff .
            this.CheckCompleteCondition_IntCompleteEmps();

            this.IsStopFlow = false;
            if (this.HisNode.IsEndNode)
            {
                /*  If the process is completed  */

                //  Before the process is complete lock handle messages listen , Otherwise WF_GenerWorkerlist To delete the .
                if (Glo.IsEnableSysMessage)
                    this.DoRefFunc_Listens();

                this.rptGe.WFState = WFState.Complete;

                string msg = this.HisWorkFlow.DoFlowOver(ActionType.FlowOver, " The process has come to the last node , Successful conclusion of the process .", this.HisNode, this.rptGe);
                this.addMsg(SendReturnMsgFlag.End, msg);

                this.IsStopFlow = true;
                this.HisGenerWorkFlow.WFState = WFState.Complete;
                return;
            }

            #region  Judgment node closing conditions 
            this.addMsg(SendReturnMsgFlag.OverCurr, string.Format(" Current work [{0}] Has been completed ", this.HisNode.Name));
            #endregion

            #region  Decision flow conditions .
            try
            {
                if (this.HisNode.HisToNodes.Count == 0 && this.HisNode.IsStartNode)
                {
                    //  Before the process is complete lock handle messages listen , Otherwise WF_GenerWorkerlist To delete the .
                    if (Glo.IsEnableSysMessage)
                        this.DoRefFunc_Listens();

                    /*  If the process is completed  */
                    this.HisWorkFlow.DoFlowOver(ActionType.FlowOver, " Meet the conditions of the process is completed ", this.HisNode, this.rptGe);
                    this.IsStopFlow = true;
                    this.addMsg(SendReturnMsgFlag.OneNodeSheetver, " Work has been successfully processed ( A flow of work ).",
                        " Work has been successfully processed ( A flow of work ). @ Check out <img src='" + VirPath + "WF/Img/Btn/PrintWorkRpt.gif' ><a href='WFRpt.aspx?WorkID=" + this.HisWork.OID + "&FID=" + this.HisWork.FID + "&FK_Flow=" + this.HisNode.FK_Flow + "'target='_blank' > Work trajectory </a>", SendReturnMsgType.Info);
                    return;
                }

                if (this.HisNode.IsCCFlow && this.HisFlowCompleteConditions.IsPass)
                {
                    //  Before the process is complete lock handle messages listen , Otherwise WF_GenerWorkerlist To delete the .
                    if (Glo.IsEnableSysMessage)
                        this.DoRefFunc_Listens();

                    string stopMsg = this.HisFlowCompleteConditions.ConditionDesc;
                    /*  If the process is completed  */
                    string overMsg = this.HisWorkFlow.DoFlowOver(ActionType.FlowOver, " Meet the conditions of the process is completed :" + stopMsg, this.HisNode, this.rptGe);
                    this.IsStopFlow = true;

                    // string path = BP.Sys.Glo.Request.ApplicationPath;
                    this.addMsg(SendReturnMsgFlag.MacthFlowOver, "@ In line with the completion of the workflow conditions " + stopMsg + "" + overMsg,
                        "@ In line with the completion of the workflow conditions " + stopMsg + "" + overMsg + " @ Check out <img src='" + VirPath + "WF/Img/Btn/PrintWorkRpt.gif' ><a href='WFRpt.aspx?WorkID=" + this.HisWork.OID + "&FID=" + this.HisWork.FID + "&FK_Flow=" + this.HisNode.FK_Flow + "'target='_blank' > Work trajectory </a>", SendReturnMsgType.Info);
                    return;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("@ Judging process {0} Error closing conditions ." + ex.Message, this.HisNode.Name));
            }
            #endregion

        }

        #region  Start a plurality of nodes 

        /// <summary>
        ///  Why send them generate 
        /// </summary>
        /// <param name="fNodeID"></param>
        /// <param name="toNodeID"></param>
        /// <returns></returns>
        public string GenerWhySendToThem(int fNodeID, int toNodeID)
        {
            return "";
            //return "@<a href='WhySendToThem.aspx?NodeID=" + fNodeID + "&ToNodeID=" + toNodeID + "&WorkID=" + this.WorkID + "' target=_blank >" + this.ToE("WN20", " Why should we send them ?") + "</a>";
        }
        /// <summary>
        ///  Workflow ID
        /// </summary>
        public static Int64 FID = 0;
        /// <summary>
        ///  No FID
        /// </summary>
        /// <param name="nd"></param>
        /// <returns></returns>
        private string StartNextWorkNodeHeLiu_WithOutFID(Node nd)
        {
            throw new Exception(" Unfinished :StartNextWorkNodeHeLiu_WithOutFID");
        }
        /// <summary>
        ///  Different threads to form sub-confluent point motion 
        /// </summary>
        /// <param name="nd"></param>
        private void NodeSend_53_UnSameSheet_To_HeLiu(Node nd)
        {

            Work heLiuWK = nd.HisWork;
            heLiuWK.OID = this.HisWork.FID;
            heLiuWK.RetrieveFromDBSources(); // Check out the data .

            heLiuWK.Copy(this.HisWork); //  Carried out copy.

            heLiuWK.OID = this.HisWork.FID;
            heLiuWK.FID = 0;

            this.town = new WorkNode(heLiuWK, nd);

            // Work on the confluence node processor .
            GenerWorkerLists gwls = new GenerWorkerLists(this.HisWork.FID, nd.NodeID);
            GenerFH myfh = new GenerFH(this.HisWork.FID);

            if (myfh.FK_Node == nd.NodeID && gwls.Count != 0)
            {
                /*  Description is not the first to come up this node , 
                 *  Such as : A process :
                 * A Bypass -> B Child thread  -> C Confluence 
                 * 从B 到C 中, B There N  Threads , Before arriving already had at least one thread C.
                 */

                /* 
                 *  First of all : Update its node  worklist  Information ,  Description of the current node has been completed .
                 *  Let the current operator can see their work , Maintain their status is completed .
                 */

                ps = new Paras();
                ps.SQL = "UPDATE WF_GenerWorkerlist SET IsPass=1 WHERE WorkID=" + dbStr + "WorkID AND FID=" + dbStr + "FID AND FK_Node=" + dbStr + "FK_Node";
                ps.Add("WorkID", this.WorkID);
                ps.Add("FID", this.HisWork.FID);
                ps.Add("FK_Node", this.HisNode.NodeID);
                DBAccess.RunSQL(ps);

                this.HisGenerWorkFlow.FK_Node = nd.NodeID;
                this.HisGenerWorkFlow.NodeName = nd.Name;

                /*
                 *  Then update the current status of the nodes and the completion time .
                 */
                this.HisWork.Update(WorkAttr.CDT, BP.DA.DataType.CurrentDataTime);

                #region  Treatment completion rate 
                Nodes fromNds = nd.FromNodes;
                string nearHLNodes = "";
                foreach (Node mynd in fromNds)
                {
                    if (mynd.HisNodeWorkType == NodeWorkType.SubThreadWork)
                        nearHLNodes += "," + mynd.NodeID;
                }
                nearHLNodes = nearHLNodes.Substring(1);

                ps = new Paras();
                ps.SQL = "SELECT FK_Emp,FK_EmpText FROM WF_GenerWorkerList WHERE FK_Node IN (" + nearHLNodes + ") AND FID=" + dbStr + "FID AND IsPass=1 AND IsEnable=1";
                ps.Add("FID", this.HisWork.FID);
                DataTable dt_worker = BP.DA.DBAccess.RunSQLReturnTable(ps);
                string numStr = "@ Triage officer has completed the implementation of the following :";
                foreach (DataRow dr in dt_worker.Rows)
                    numStr += "@" + dr[0] + "," + dr[1];

                //  Praying the number of threads .
                ps = new Paras();
                ps.SQL = "SELECT DISTINCT(WorkID) FROM WF_GenerWorkerList WHERE FK_Node IN (" + nearHLNodes + ") AND FID=" + dbStr + "FID AND IsPass=1 AND IsEnable=1";
                ps.Add("FID", this.HisWork.FID);
                DataTable dt_thread = BP.DA.DBAccess.RunSQLReturnTable(ps);
                decimal ok = (decimal)dt_thread.Rows.Count;

                ps = new Paras();
                ps.SQL = "SELECT  COUNT(distinct WorkID) AS Num FROM WF_GenerWorkerList WHERE IsEnable=1 AND FID=" + dbStr + "FID AND FK_Node IN (" + this.SpanSubTheadNodes(nd) + ")";
                ps.Add("FID", this.HisWork.FID);
                decimal all = (decimal)DBAccess.RunSQLReturnValInt(ps);
                decimal passRate = ok / all * 100;
                numStr += "@ You are (" + ok + ") Reach deal with people on this node , Total start-up (" + all + ") Sub-processes .";
                if (nd.PassRate <= passRate)
                {
                    /* Description All the staff have completed , Let confluence display it .*/
                    ps = new Paras();
                    ps.SQL = "UPDATE WF_GenerWorkerList SET IsPass=0  WHERE FK_Node=" + dbStr + "FK_Node AND WorkID=" + dbStr + "WorkID ";
                    ps.Add("FK_Node", nd.NodeID);
                    ps.Add("WorkID", this.HisWork.FID);
                    DBAccess.RunSQL(ps);

                    ps = new Paras();
                    ps.SQL = "UPDATE WF_GenerWorkFlow SET   FK_Node=" + dbStr + "FK_Node WHERE  WorkID=" + dbStr + "WorkID ";
                    ps.Add("FK_Node", nd.NodeID);
                    ps.Add("WorkID", this.HisWork.FID);
                    DBAccess.RunSQL(ps);

                    numStr += "@ The next step (" + nd.Name + ") Has started .";
                }
                #endregion  Treatment completion rate 

                if (myfh.ToEmpsMsg.Contains("("))
                {
                    string FK_Emp1 = myfh.ToEmpsMsg.Substring(0, myfh.ToEmpsMsg.LastIndexOf('('));
                    this.AddToTrack(ActionType.ForwardHL, FK_Emp1, myfh.ToEmpsMsg, nd.NodeID, nd.Name, null);
                }
                this.addMsg("ToHeLiuInfo",
                    "@ The process has been running to the confluence of the node [" + nd.Name + "],@ Your work has been sent to the following persons [" + myfh.ToEmpsMsg + "],<a href=\"javascript:WinOpen('./Msg/SMS.aspx?WorkID=" + this.WorkID + "&FK_Node=" + nd.NodeID + "')\" > SMS informing them </a>." + this.GenerWhySendToThem(this.HisNode.NodeID, nd.NodeID) + numStr);
            }
            else
            {
                //  Description first arrived river node .
                gwls = this.Func_GenerWorkerLists(this.town);
            }

            string FK_Emp = "";
            string toEmpsStr = "";
            string emps = "";
            foreach (GenerWorkerList wl in gwls)
            {
                toEmpsStr += BP.WF.Glo.DealUserInfoShowModel(wl.FK_Emp, wl.FK_EmpText);
                if (gwls.Count == 1)
                    emps = wl.FK_Emp;
                else
                    emps += "@" + FK_Emp;
            }


            /* 
            *  Update its node  worklist  Information ,  Description of the current node has been completed .
            *  Let the current operator can see their work .
            */

            //  Set the parent process status   Set the current node :
            myfh.Update(GenerFHAttr.FK_Node, nd.NodeID,
                GenerFHAttr.ToEmpsMsg, toEmpsStr);
            #region  Merging form data processing node .


            #region  Copy the master table data . edit 2014-11-20  Aggregated data to the confluence .
            // Copy the current node form data .
            heLiuWK.FID = 0;
            heLiuWK.Rec = FK_Emp;
            heLiuWK.Emps = emps;
            heLiuWK.OID = this.HisWork.FID;
            heLiuWK.DirectUpdate(); // In the updated .

            /*  Copy the data to rpt Data table . */
            this.rptGe.OID = this.HisWork.FID;
            this.rptGe.RetrieveFromDBSources();
            this.rptGe.Copy(this.HisWork);
            this.rptGe.DirectUpdate();

            #endregion  Copy the master table data .

            #region  Copy the attachment .
            if (this.HisNode.MapData.FrmAttachments.Count != 0)
            {
                FrmAttachmentDBs athDBs = new FrmAttachmentDBs("ND" + this.HisNode.NodeID,
                      this.WorkID.ToString());
                if (athDBs.Count > 0)
                {
                    /* Description of the current node has an attachment data */
                    int idx = 0;
                    foreach (FrmAttachmentDB athDB in athDBs)
                    {
                        idx++;
                        FrmAttachmentDB athDB_N = new FrmAttachmentDB();
                        athDB_N.Copy(athDB);
                        athDB_N.FK_MapData = "ND" + nd.NodeID;
                        athDB_N.MyPK = athDB_N.MyPK.Replace("ND" + this.HisNode.NodeID, "ND" + nd.NodeID) + "_" + idx;
                        athDB_N.FK_FrmAttachment = athDB_N.FK_FrmAttachment.Replace("ND" + this.HisNode.NodeID,
                           "ND" + nd.NodeID);
                        athDB_N.RefPKVal = this.HisWork.FID.ToString();
                        athDB_N.Save();
                    }
                }
            }
            #endregion  Copy the attachment .

            #region  Copy EleDB.
            if (this.HisNode.MapData.FrmEles.Count != 0)
            {
                FrmEleDBs eleDBs = new FrmEleDBs("ND" + this.HisNode.NodeID,
                      this.WorkID.ToString());
                if (eleDBs.Count > 0)
                {
                    /* Description of the current node has an attachment data */
                    int idx = 0;
                    foreach (FrmEleDB eleDB in eleDBs)
                    {
                        idx++;
                        FrmEleDB eleDB_N = new FrmEleDB();
                        eleDB_N.Copy(eleDB);
                        eleDB_N.FK_MapData = "ND" + nd.NodeID;
                        eleDB_N.MyPK = eleDB_N.MyPK.Replace("ND" + this.HisNode.NodeID, "ND" + nd.NodeID);

                        eleDB_N.RefPKVal = this.HisWork.FID.ToString();
                        eleDB_N.Save();
                    }
                }
            }
            #endregion  Copy EleDB.

            //  Confluence summary data generated list .
            this.GenerHieLiuHuiZhongDtlData_2013(nd);

            #endregion  Merging form data processing node 

            /*  Confluence need to wait for each split point has been dealt with in order to see it .*/
            string info = "";
            string sql1 = "";
#warning  For multiple sub-confluent point may be a problem .
            ps = new Paras();
            ps.SQL = "SELECT COUNT(distinct WorkID) AS Num FROM WF_GenerWorkerList WHERE  FID=" + dbStr + "FID AND FK_Node IN (" + this.SpanSubTheadNodes(nd) + ")";
            ps.Add("FID", this.HisWork.FID);
            decimal numAll1 = (decimal)DBAccess.RunSQLReturnValInt(ps);
            decimal passRate1 = 1 / numAll1 * 100;
            if (nd.PassRate <= passRate1)
            {
                ps = new Paras();
                ps.SQL = "UPDATE WF_GenerWorkerList SET IsPass=0,FID=0 WHERE FK_Node=" + dbStr + "FK_Node AND WorkID=" + dbStr + "WorkID";
                ps.Add("FK_Node", nd.NodeID);
                ps.Add("WorkID", this.HisWork.FID);
                DBAccess.RunSQL(ps);

                ps = new Paras();
                ps.SQL = "UPDATE WF_GenerWorkFlow SET FK_Node=" + dbStr + "FK_Node,NodeName=" + dbStr + "NodeName WHERE WorkID=" + dbStr + "WorkID";
                ps.Add("FK_Node", nd.NodeID);
                ps.Add("NodeName", nd.Name);
                ps.Add("WorkID", this.HisWork.FID);
                DBAccess.RunSQL(ps);

                info = "@ Next confluence (" + nd.Name + ") Has started .";
            }
            else
            {
#warning  To keep it displayed in the way of work required , =3  Is not a normal processing mode .
                ps = new Paras();
                ps.SQL = "UPDATE WF_GenerWorkerList SET IsPass=3,FID=0 WHERE FK_Node=" + dbStr + "FK_Node AND WorkID=" + dbStr + "WorkID";
                ps.Add("FK_Node", nd.NodeID);
                ps.Add("WorkID", this.HisWork.OID);
                DBAccess.RunSQL(ps);
            }

            this.HisGenerWorkFlow.FK_Node = nd.NodeID;
            this.HisGenerWorkFlow.NodeName = nd.Name;

            //ps = new Paras();
            //ps.SQL = "UPDATE WF_GenerWorkFlow SET  WFState=" + (int)WFState.Runing + ", FK_Node=" + nd.NodeID + ",NodeName='" + nd.Name + "' WHERE WorkID=" + this.HisWork.FID;
            //ps.Add("FK_Node", nd.NodeID);
            //ps.Add("NodeName", nd.Name);
            //ps.Add("WorkID", this.HisWork.FID);
            //DBAccess.RunSQL(ps);
            this.addMsg(SendReturnMsgFlag.VarAcceptersID, emps, SendReturnMsgType.SystemMsg);

            if (myfh.FK_Node != nd.NodeID)
            {
                this.addMsg("HeLiuInfo",
                    "@ The current work has been completed , The process has been running to the confluence of the node [" + nd.Name + "].@ Your work has been sent to the following persons [" + toEmpsStr + "],<a href=\"javascript:WinOpen('" + VirPath + "WF/Msg/SMS.aspx?WorkID=" + this.WorkID + "&FK_Node=" + nd.NodeID + "')\" > SMS informing them </a>.@ You are the first person to reach this node processing ." + info);
            }
            else
            {
                this.addMsg("HeLiuInfo", "@ The next step processors [" + emps + "]" + info, SendReturnMsgType.Info);
            }
        }
        /// <summary>
        ///  Generate aggregated data confluence 
        ///  Thread handle child table data into the master table confluence points up from the table 
        /// </summary>
        /// <param name="nd"></param>
        private void GenerHieLiuHuiZhongDtlData_2013(Node ndOfHeLiu)
        {
            MapDtls mydtls = ndOfHeLiu.HisWork.HisMapDtls;
            foreach (MapDtl dtl in mydtls)
            {
                if (dtl.IsHLDtl == false)
                    continue;

                GEDtl geDtl = dtl.HisGEDtl;
                geDtl.Copy(this.HisWork);
                geDtl.RefPK = this.HisWork.FID.ToString(); // RefPK  Is the current sub-thread FID.
                geDtl.Rec = this.Execer;
                geDtl.RDT = DataType.CurrentDataTime;

                #region  It is determined whether the quality evaluation 
                if (ndOfHeLiu.IsEval)
                {
                    /* If the quality of the evaluation process */
                    geDtl.SetValByKey(WorkSysFieldAttr.EvalEmpNo, this.Execer);
                    geDtl.SetValByKey(WorkSysFieldAttr.EvalEmpName, this.ExecerName);
                    geDtl.SetValByKey(WorkSysFieldAttr.EvalCent, 0);
                    geDtl.SetValByKey(WorkSysFieldAttr.EvalNote, "");
                }
                #endregion

                try
                {
                    geDtl.InsertAsOID(this.HisWork.OID);
                }
                catch
                {
                    geDtl.Update();
                }
                break;
            }
        }
        /// <summary>
        ///  Child thread node 
        /// </summary>
        private string _SpanSubTheadNodes = null;
        /// <summary>
        ///  Get the child thread split and merge a set of nodes between .
        /// </summary>
        /// <param name="toNode"></param>
        /// <returns></returns>
        private string SpanSubTheadNodes(Node toHLNode)
        {
            _SpanSubTheadNodes = "";
            SpanSubTheadNodes_DiGui(toHLNode.FromNodes);
            if (_SpanSubTheadNodes == "")
                throw new Exception(" Get the child thread confluence between node points set is empty , Check Process Design , Nodes between sub-confluent nodes must be set to the child thread .");
            _SpanSubTheadNodes = _SpanSubTheadNodes.Substring(1);
            return _SpanSubTheadNodes;

        }
        private void SpanSubTheadNodes_DiGui(Nodes subNDs)
        {
            foreach (Node nd in subNDs)
            {
                if (nd.HisNodeWorkType == NodeWorkType.SubThreadWork)
                {
                    // Determines whether or not already contain , Or may loop 
                    if (_SpanSubTheadNodes.Contains("," + nd.NodeID))
                        continue;

                    _SpanSubTheadNodes += "," + nd.NodeID;
                    SpanSubTheadNodes_DiGui(nd.FromNodes);
                }
            }
        }
        #endregion

        #region  Basic properties 
        /// <summary>
        ///  The work 
        /// </summary>
        private Work _HisWork = null;
        /// <summary>
        ///  The work 
        /// </summary>
        public Work HisWork
        {
            get
            {
                return this._HisWork;
            }
        }
        /// <summary>
        ///  Node 
        /// </summary>
        private Node _HisNode = null;
        /// <summary>
        ///  Node 
        /// </summary>
        public Node HisNode
        {
            get
            {
                return this._HisNode;
            }
        }
        private RememberMe HisRememberMe = null;
        public RememberMe GetHisRememberMe(Node nd)
        {
            if (HisRememberMe == null || HisRememberMe.FK_Node != nd.NodeID)
            {
                HisRememberMe = new RememberMe();
                HisRememberMe.FK_Emp = this.Execer;
                HisRememberMe.FK_Node = nd.NodeID;
                HisRememberMe.RetrieveFromDBSources();
            }
            return this.HisRememberMe;
        }
        private WorkFlow _HisWorkFlow = null;
        /// <summary>
        ///  Workflow 
        /// </summary>
        public WorkFlow HisWorkFlow
        {
            get
            {
                if (_HisWorkFlow == null)
                    _HisWorkFlow = new WorkFlow(this.HisNode.HisFlow, this.HisWork.OID, this.HisWork.FID);
                return _HisWorkFlow;
            }
        }
        /// <summary>
        ///  The current node job is not complete .
        /// </summary>
        public bool IsComplete
        {
            get
            {
                if (this.HisGenerWorkFlow.WFState == WFState.Complete)
                    return true;
                else
                    return false;
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Establish a working node case .
        /// </summary>
        /// <param name="workId"> The work ID</param>
        /// <param name="nodeId"> Node ID</param>
        public WorkNode(Int64 workId, int nodeId)
        {
            this.WorkID = workId;
            Node nd = new Node(nodeId);
            Work wk = nd.HisWork;
            wk.OID = workId;
            int i = wk.RetrieveFromDBSources();
            if (i == 0)
            {
                this.rptGe = nd.HisFlow.HisGERpt;
                this.rptGe.OID = this.WorkID;
                this.rptGe.Retrieve();
                wk.Row = rptGe.Row;
            }
            this._HisWork = wk;
            this._HisNode = nd;
        }
        public Hashtable SendHTOfTemp = null;
        public string title = null;
        /// <summary>
        ///  Establish a working node case 
        /// </summary>
        /// <param name="wk"> The work </param>
        /// <param name="nd"> Node </param>
        public WorkNode(Work wk, Node nd)
        {
            this.WorkID = wk.OID;
            this._HisWork = wk;
            this._HisNode = nd;
        }
        #endregion

        #region  Operational Attributes 
        private void Repair()
        {
        }
        public WorkNode GetPreviousWorkNode_FHL(Int64 workid)
        {
            Nodes nds = this.HisNode.FromNodes;
            foreach (Node nd in nds)
            {
                if (nd.HisRunModel == RunModel.SubThread)
                {
                    Work wk = nd.HisWork;
                    wk.OID = workid;
                    if (wk.RetrieveFromDBSources() != 0)
                    {
                        WorkNode wn = new WorkNode(wk, nd);
                        return wn;
                    }
                }
            }

            //WorkNodes wns = this.GetPreviousWorkNodes_FHL();
            //foreach (WorkNode wn in wns)
            //{
            //    if (wn.HisWork.OID == workid)
            //        return wn;
            //}
            return null;
        }
        public WorkNodes GetPreviousWorkNodes_FHL()
        {
            //  If you do not find him turning nodes , Returns , Current work .
            if (this.HisNode.IsStartNode)
                throw new Exception("@ This node is the start node , No further work on "); // This node is the start node , No further work on .

            if (this.HisNode.HisNodeWorkType == NodeWorkType.WorkHL
               || this.HisNode.HisNodeWorkType == NodeWorkType.WorkFHL)
            {
            }
            else
            {
                throw new Exception("@ Current Work Day  -  Non-confluent nodes are divided .");
            }

            WorkNodes wns = new WorkNodes();
            Nodes nds = this.HisNode.FromNodes;
            foreach (Node nd in nds)
            {
                Works wks = (Works)nd.HisWorks;
                wks.Retrieve(WorkAttr.FID, this.HisWork.OID);

                if (wks.Count == 0)
                    continue;

                foreach (Work wk in wks)
                {
                    WorkNode wn = new WorkNode(wk, nd);
                    wns.Add(wn);
                }
            }
            return wns;
        }
        /// <summary>
        ///  Previous to his work properly 
        /// 1,  Collection from the current step to find his work on node .		 
        ///  If you do not find him turning nodes , Returns , Current work .
        /// </summary>
        /// <returns> Previous to his work properly </returns>
        public WorkNode GetPreviousWorkNode()
        {
            //  If you do not find him turning nodes , Returns , Current work .
            if (this.HisNode.IsStartNode)
                throw new Exception("@" + string.Format(" This node is the start node , No further work on ")); // This node is the start node , No further work on .


            string sql = "";

            // Get on one node of the current node according to , Do not bother to send that person 
            sql = "SELECT NDFrom FROM ND" + int.Parse(this.HisNode.FK_Flow) + "Track WHERE WorkID=" + this.WorkID
                                                                                        + " AND NDTo='" + this.HisNode.NodeID
                                                                                        + "' AND ActionType=1 ORDER BY RDT DESC";
            int nodeid = DBAccess.RunSQLReturnValInt(sql, 0);
            if (nodeid == 0)
            {
                switch (this.HisNode.HisRunModel)
                {
                    case RunModel.HL:
                    case RunModel.FHL:
                        sql = "SELECT NDFrom FROM ND" + int.Parse(this.HisNode.FK_Flow) + "Track WHERE WorkID=" + this.WorkID
                                                                                       + " AND NDTo='" + this.HisNode.NodeID
                                                                                       + "' AND ActionType=" + (int)ActionType.ForwardHL + " ORDER BY RDT DESC";
                        break;
                    default:
                        break;
                }

                nodeid = DBAccess.RunSQLReturnValInt(sql, 0);
            }

            if (nodeid == 0)
                throw new Exception("@ Error , Node is not found in the previous step .");

            Node nd = new Node(nodeid);
            Work wk = nd.HisWork;
            wk.OID = this.WorkID;
            wk.RetrieveFromDBSources();

            WorkNode wn = new WorkNode(wk, nd);
            return wn;


            //WorkNodes wns = new WorkNodes();
            //Nodes nds = this.HisNode.FromNodes;
            //foreach (Node nd in nds)
            //{
            //    switch (this.HisNode.HisNodeWorkType)
            //    {
            //        case NodeWorkType.WorkHL: /*  If it is the confluence  */
            //            if (this.IsSubFlowWorkNode == false)
            //            {
            //                /*  If not thread  */
            //                Node pnd = nd.HisPriFLNode;
            //                if (pnd == null)
            //                    throw new Exception("@ Shunt node not via its previous step , Make sure the design is wrong ?");

            //                Work wk1 = (Work)pnd.HisWorks.GetNewEntity;
            //                wk1.OID = this.HisWork.OID;
            //                if (wk1.RetrieveFromDBSources() == 0)
            //                    continue;
            //                WorkNode wn11 = new WorkNode(wk1, pnd);
            //                return wn11;
            //                break;
            //            }
            //            break;
            //        default:
            //            break;
            //    }

            //    Work wk = (Work)nd.HisWorks.GetNewEntity;
            //    wk.OID = this.HisWork.OID;
            //    if (wk.RetrieveFromDBSources() == 0)
            //        continue;

            //    string table = "ND" + int.Parse(this.HisNode.FK_Flow) + "Track";
            //    string actionSQL = "SELECT EmpFrom,EmpFromT,RDT FROM " + table + " WHERE WorkID=" + this.WorkID + " AND NDFrom=" + nd.NodeID + " AND ActionType=" + (int)ActionType.Forward;
            //    DataTable dt = DBAccess.RunSQLReturnTable(actionSQL);
            //    if (dt.Rows.Count == 0)
            //        continue;

            //    wk.Rec = dt.Rows[0]["EmpFrom"].ToString();
            //    wk.RecText = dt.Rows[0]["EmpFromT"].ToString();
            //    wk.SetValByKey("RDT", dt.Rows[0]["RDT"].ToString());

            //    WorkNode wn = new WorkNode(wk, nd);
            //    wns.Add(wn);
            //}
            //switch (wns.Count)
            //{
            //    case 0:
            //        throw new Exception(" Did not find his work on the step , System error , Please notify an administrator to handle , Please make a step on the treatment of human Undo Send , Or use of the county administrator user login =》 Upcoming work =》 Process Query =》 Enter keywords Workid Select all other conditions , Inquiry into the process to remove it . @WorkID=" + this.WorkID);
            //    case 1:
            //        return (WorkNode)wns[0];
            //    default:
            //        break;
            //}
            //Node nd1 = wns[0].HisNode;
            //Node nd2 = wns[1].HisNode;
            //if (nd1.FromNodes.Contains(NodeAttr.NodeID, nd2.NodeID))
            //{
            //    return wns[0];
            //}
            //else
            //{
            //    return wns[1];
            //}
        }
        #endregion
    }
    /// <summary>
    ///  Working set of nodes .
    /// </summary>
    public class WorkNodes : CollectionBase
    {
        #region  Structure 
        /// <summary>
        ///  His work s
        /// </summary> 
        public Works GetWorks
        {
            get
            {
                if (this.Count == 0)
                    throw new Exception("@ Initialization failed , Did not find any node .");

                Works ens = this[0].HisNode.HisWorks;
                ens.Clear();

                foreach (WorkNode wn in this)
                {
                    ens.AddEntity(wn.HisWork);
                }
                return ens;
            }
        }
        /// <summary>
        ///  Working set of nodes 
        /// </summary>
        public WorkNodes()
        {
        }

        public int GenerByFID(Flow flow, Int64 fid)
        {
            this.Clear();

            Nodes nds = flow.HisNodes;
            foreach (Node nd in nds)
            {
                if (nd.HisRunModel == RunModel.SubThread)
                    continue;

                Work wk = nd.GetWork(fid);
                if (wk == null)
                    continue;


                this.Add(new WorkNode(wk, nd));
            }
            return this.Count;
        }
        /// <summary>
        ///  This method has the problem of 
        /// </summary>
        /// <param name="flow"></param>
        /// <param name="oid"></param>
        /// <returns></returns>
        public int GenerByWorkID2014_01_06(Flow flow, Int64 oid)
        {
            Nodes nds = flow.HisNodes;
            foreach (Node nd in nds)
            {
                Work wk = nd.GetWork(oid);
                if (wk == null)
                    continue;
                string table = "ND" + int.Parse(flow.No) + "Track";
                string actionSQL = "SELECT EmpFrom,EmpFromT,RDT FROM " + table + " WHERE WorkID=" + oid + " AND NDFrom=" + nd.NodeID + " AND ActionType=" + (int)ActionType.Forward;
                DataTable dt = DBAccess.RunSQLReturnTable(actionSQL);
                if (dt.Rows.Count == 0)
                    continue;

                wk.Rec = dt.Rows[0]["EmpFrom"].ToString();
                wk.RecText = dt.Rows[0]["EmpFromT"].ToString();
                wk.SetValByKey("RDT", dt.Rows[0]["RDT"].ToString());
                this.Add(new WorkNode(wk, nd));
            }
            return this.Count;
        }
        public int GenerByWorkID(Flow flow, Int64 oid)
        {
            string table = "ND" + int.Parse(flow.No) + "Track";
            string actionSQL = "SELECT EmpFrom,EmpFromT,RDT,NDFrom FROM " + table + " WHERE WorkID=" + oid + " AND (ActionType=" + (int)ActionType.Forward + " OR ActionType=" + (int)ActionType.ForwardFL + " OR ActionType=" + (int)ActionType.ForwardHL + " OR ActionType=" + (int)ActionType.SubFlowForward + " ) ORDER BY RDT";
            DataTable dt = DBAccess.RunSQLReturnTable(actionSQL);

            string nds = "";
            foreach (DataRow dr in dt.Rows)
            {
                Node nd = new Node(int.Parse(dr["NDFrom"].ToString()));
                Work wk = nd.GetWork(oid);
                if (wk == null)
                    wk = nd.HisWork;

                //  Deal with the problem of repeat .
                if (nds.Contains(nd.NodeID.ToString() + ",") == true)
                    continue;
                nds += nd.NodeID.ToString() + ",";


                wk.Rec = dr["EmpFrom"].ToString();
                wk.RecText = dr["EmpFromT"].ToString();
                wk.SetValByKey("RDT", dr["RDT"].ToString());
                this.Add(new WorkNode(wk, nd));
            }
            return this.Count;
        }
        /// <summary>
        ///  Delete Workflow 
        /// </summary>
        public void DeleteWorks()
        {
            foreach (WorkNode wn in this)
            {
                if (wn.HisFlow.HisDataStoreModel != DataStoreModel.ByCCFlow)
                    return;
                wn.HisWork.Delete();
            }
        }
        #endregion

        #region  Method 
        /// <summary>
        ///  Adding a WorkNode
        /// </summary>
        /// <param name="wn"> The work   Node </param>
        public void Add(WorkNode wn)
        {
            this.InnerList.Add(wn);
        }
        /// <summary>
        ///  Access to data based on location 
        /// </summary>
        public WorkNode this[int index]
        {
            get
            {
                return (WorkNode)this.InnerList[index];
            }
        }
        #endregion
    }
}
