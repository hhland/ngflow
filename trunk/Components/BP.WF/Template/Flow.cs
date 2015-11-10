using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Text;
using BP.DA;
using BP.Sys;
using BP.Port;
using BP.En;
using BP.WF.Template;
using BP.WF.Data;
using BP.Web;
using Microsoft.Win32;

namespace BP.WF
{
    /// <summary>
    ///  Process 
    ///  Record the flow of information ．
    ///  Process ID , Name , Setup time ．
    /// </summary>
    public class Flow : EntityNoName
    {
        #region  Basic properties .

        public string mailUrlsPattern = "";

        /// <summary>
        ///  Process events entity 
        /// </summary>
        public string FlowEventEntity
        {
            get
            {
                return this.GetValStringByKey(FlowAttr.FlowEventEntity);
            }
            set
            {
                this.SetValByKey(FlowAttr.FlowEventEntity, value);
            }
        }
        /// <summary>
        ///  Process tag 
        /// </summary>
        public string FlowMark
        {
            get
            {
                return this.GetValStringByKey(FlowAttr.FlowMark);
            }
            set
            {
                this.SetValByKey(FlowAttr.FlowMark, value);
            }
        }
        #endregion

        #region  Initiate restrictions .
        /// <summary>
        ///  Initiate restrictions .
        /// </summary>
        public StartLimitRole StartLimitRole
        {
            get
            {
                return (StartLimitRole)this.GetValIntByKey(FlowAttr.StartLimitRole);
            }
            set
            {
                this.SetValByKey(FlowAttr.StartLimitRole, (int)value);
            }
        }
        /// <summary>
        ///  Sponsored content 
        /// </summary>
        public string StartLimitPara
        {
            get
            {
                return this.GetValStringByKey(FlowAttr.StartLimitPara);
            }
            set
            {
                this.SetValByKey(FlowAttr.StartLimitPara,value);
            }
        }
        public string StartLimitAlert
        {
            get
            {
                string s= this.GetValStringByKey(FlowAttr.StartLimitAlert);
                if (s == "")
                    return " You have to start the process over , Not repeat start .";
                return s;
            }
            set
            {
                this.SetValByKey(FlowAttr.StartLimitAlert, value);
            }
        }
        /// <summary>
        ///  Trigger time limit 
        /// </summary>
        public StartLimitWhen StartLimitWhen
        {
            get
            {
                return (StartLimitWhen)this.GetValIntByKey(FlowAttr.StartLimitWhen);
            }
            set
            {
                this.SetValByKey(FlowAttr.StartLimitWhen, (int)value);
            }
        }
        #endregion  Initiate restrictions .

        #region  Navigation mode 
        /// <summary>
        ///  Initiate navigation 
        /// </summary>
        public StartGuideWay StartGuideWay
        {
            get
            {
                return (StartGuideWay)this.GetValIntByKey(FlowAttr.StartGuideWay);
            }
            set
            {
                this.SetValByKey(FlowAttr.StartGuideWay, (int)value);
            }
        }
        /// <summary>
        ///  Process initiated parameter 1
        /// </summary>
        public string StartGuidePara1
        {
            get
            {
                return this.GetValStrByKey(FlowAttr.StartGuidePara1);
            }
            set
            {
                this.SetValByKey(FlowAttr.StartGuidePara1, value);
            }
        }
        /// <summary>
        ///  Process initiated parameter 2
        /// </summary>
        public string StartGuidePara2
        {
            get
            {
                string s = this.GetValStrByKey(FlowAttr.StartGuidePara2);
                if (string.IsNullOrEmpty(s) == null)
                {
                    if (this.StartGuideWay == BP.WF.Template.StartGuideWay.ByHistoryUrl)
                    {

                    }
                }
                return s;
            }
            set
            {
                this.SetValByKey(FlowAttr.StartGuidePara2, value);
            }
        }
        /// <summary>
        ///  Process initiated parameter 3
        /// </summary>
        public string StartGuidePara3
        {
            get
            {
                return this.GetValStrByKey(FlowAttr.StartGuidePara3);
            }
            set
            {
                this.SetValByKey(FlowAttr.StartGuidePara3, value);
            }
        }
        /// <summary>
        ///  Whether the data reset button is enabled ?
        /// </summary>
        public bool IsResetData
        {
            get
            {
                return this.GetValBooleanByKey(FlowAttr.IsResetData);
            }
        }
        /// <summary>
        ///  Whether to enable import historical data button ?
        /// </summary>
        public bool IsImpHistory
        {
            get
            {
                return this.GetValBooleanByKey(FlowAttr.IsImpHistory);
            }
        }
        /// <summary>
        ///  Whether to automatically load the data on a ?
        /// </summary>
        public bool IsLoadPriData
        {
            get
            {
                return this.GetValBooleanByKey(FlowAttr.IsLoadPriData);
            }
        }
        #endregion

        #region  Other properties 
        public string Tag = null;
        /// <summary>
        ///  Run Type 
        /// </summary>
        public FlowRunWay HisFlowRunWay
        {
            get
            {
                return (FlowRunWay)this.GetValIntByKey(FlowAttr.FlowRunWay);
            }
            set
            {
                this.SetValByKey(FlowAttr.FlowRunWay, (int)value);
            }
        }
        /// <summary>
        ///  Running Object 
        /// </summary>
        public string RunObj
        {
            get
            {
                return this.GetValStrByKey(FlowAttr.RunObj);
            }
            set
            {
                this.SetValByKey(FlowAttr.RunObj, value);
            }
        }
        /// <summary>
        ///  Time rule 
        /// </summary>
        public TimelineRole HisTimelineRole
        {
            get
            {
                return (TimelineRole)this.GetValIntByKey(FlowAttr.TimelineRole);
            }
        }
        /// <summary>
        ///  Process department data query access control mode 
        /// </summary>
        public FlowDeptDataRightCtrlType HisFlowDeptDataRightCtrlType
        {
            get
            {
                return (FlowDeptDataRightCtrlType)this.GetValIntByKey(FlowAttr.DRCtrlType);
            }
            set
            {
                this.SetValByKey(FlowAttr.DRCtrlType, value);
            }
        }
        /// <summary>
        ///  Process Application Type 
        /// </summary>
        public FlowAppType FlowAppType
        {
            get
            {
                return (FlowAppType)this.GetValIntByKey(FlowAttr.FlowAppType);
            }
            set
            {
                this.SetValByKey(FlowAttr.FlowAppType, (int)value);
            }
        }
        /// <summary>
        ///  Continuation of the process approach 
        /// </summary>
        public CFlowWay CFlowWay
        {
            get
            {
                return (CFlowWay)this.GetValIntByKey(FlowAttr.CFlowWay);
            }
            set
            {
                this.SetValByKey(FlowAttr.CFlowWay, (int)value);
            }
        }
        /// <summary>
        ///  Continuation of the process parameters .
        /// </summary>
        public string CFlowPara
        {
            get
            {
                return this.GetValStrByKey(FlowAttr.CFlowPara);
            }
            set
            {
                this.SetValByKey(FlowAttr.CFlowPara, value);
            }
        }
    
        /// <summary>
        ///  Flow Remark expressions 
        /// </summary>
        public string FlowNoteExp
        {
            get
            {
                return this.GetValStrByKey(FlowAttr.FlowNoteExp);
            }
            set
            {
                this.SetValByKey(FlowAttr.FlowNoteExp, value);
            }
        }
        #endregion  Business Process 

        #region  Creating new jobs .
        /// <summary>
        ///  Creating new jobs 
        /// </summary>
        /// <returns></returns>
        public StartWork NewWork()
        {
            return NewWork(WebUser.No);
        }
        /// <summary>
        ///  Creating new jobs .
        /// </summary>
        /// <param name="empNo"> Personnel Number </param>
        /// <returns></returns>
        public StartWork NewWork(string empNo)
        {
            Emp emp = new Emp(empNo);
            return NewWork(emp);
        }
       
        /// <summary>
        ///  Generating a new job start node 
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        public StartWork NewWork(Emp emp)
        {
            // Start node .
            BP.WF.Node nd = new BP.WF.Node(this.StartNodeID);

            // From a look at whether there is a new job in the draft ?
            StartWork wk = (StartWork)nd.HisWork;
            string dbstr=SystemConfig.AppCenterDBVarStr;
            
            Paras ps = new Paras();
            GERpt rpt = this.HisGERpt;

            // Whether the newly created WorKID
            bool IsNewWorkID = true;
            /* If you want to enable the draft , To create a new WorkID .*/
            try
            {
                // Query whether the data from the report in the presence of ?
                if (this.IsGuestFlow == true && string.IsNullOrEmpty(GuestUser.No) == false)
                {
                    /* The process is customer involvement , And has a client login information .*/
                    ps.SQL = "SELECT OID,FlowEndNode FROM " + this.PTable + " WHERE GuestNo=" + dbstr + "GuestNo AND WFState=" + dbstr + "WFState ";
                    ps.Add(GERptAttr.GuestNo, GuestUser.No);
                    ps.Add(GERptAttr.WFState, (int)WFState.Blank);
                    DataTable dt = DBAccess.RunSQLReturnTable(ps);
                    if (dt.Rows.Count == 1)
                    {
                        IsNewWorkID = false;
                        wk.OID = Int64.Parse(dt.Rows[0][0].ToString());
                        int nodeID = int.Parse(dt.Rows[0][1].ToString());
                        if (nodeID != this.StartNodeID)
                        {
                            string error = "@ Here there is blank Under the state of the process to other nodes running up the situation .";
                            Log.DefaultLogWriteLineError(error);
                            throw new Exception(error);
                        }
                    }
                }
                else
                {
                    ps.SQL = "SELECT OID,FlowEndNode FROM " + this.PTable + " WHERE FlowStarter=" + dbstr + "FlowStarter AND WFState=" + dbstr + "WFState ";
                    ps.Add(GERptAttr.FlowStarter, emp.No);
                    ps.Add(GERptAttr.WFState, (int)WFState.Blank);
                    DataTable dt = DBAccess.RunSQLReturnTable(ps);
                    if (dt.Rows.Count == 1)
                    {
                        IsNewWorkID = false;
                        wk.OID = Int64.Parse(dt.Rows[0][0].ToString());
                        int nodeID = int.Parse(dt.Rows[0][1].ToString());
                        if (nodeID != this.StartNodeID)
                        {
                            string error = "@ Here there is blank Under the state of the process to other nodes running up the situation .";
                            Log.DefaultLogWriteLineError(error);
                            throw new Exception(error);
                        }
                    }
                }
                // The draft is not enabled , Regarded as New workid
                if (!BP.WF.Glo.IsEnableDraft && nd.IsStartNode)
                    IsNewWorkID = true;

                if (wk.OID == 0)
                {
                    /*  Description no blank , Creates a blank ..*/
                    wk.ResetDefaultVal();
                    wk.Rec = WebUser.No;

                    wk.SetValByKey(StartWorkAttr.RecText, emp.Name);
                    wk.SetValByKey(StartWorkAttr.Emps, emp.No);

                    wk.SetValByKey(WorkAttr.RDT, BP.DA.DataType.CurrentDataTime);
                    wk.SetValByKey(WorkAttr.CDT, BP.DA.DataType.CurrentDataTime);
                    wk.SetValByKey(GERptAttr.WFState, (int)WFState.Blank);

                    wk.OID = DBAccess.GenerOID("WorkID"); /* Produced here WorkID , This is the only generation WorkID Place .*/

                    // The flow field as possible into , Otherwise there will be obliterated flow field properties .
                    wk.SetValByKey(GERptAttr.FK_NY, BP.DA.DataType.CurrentYearMonth);
                    wk.SetValByKey(GERptAttr.FK_Dept, emp.FK_Dept);
                    wk.DirectInsert();

                    // Select initialization staff .
                    this.InitSelectAccper(nd,wk.OID);

                    if (SystemConfig.IsBSsystem == true)
                    {
                        /* In the case of bs System , Came to accept arguments .*/
                        foreach (string k in BP.Sys.Glo.Request.QueryString.AllKeys)
                            rpt.SetValByKey(k, BP.Sys.Glo.Request.QueryString[k]);
                    }

                    if (this.PTable == wk.EnMap.PhysicsTable)
                    {
                        /* If the table is equal to the starting node and process reports .*/
                        rpt.OID = wk.OID;
                        rpt.RetrieveFromDBSources();
                        rpt.FID = 0;
                        rpt.FlowStartRDT = BP.DA.DataType.CurrentDataTime;
                        rpt.MyNum = 0;
                        rpt.Title = WorkNode.GenerTitle(this, wk);
                        //WebUser.No + "," + BP.Web.WebUser.Name + "在" + DataType.CurrentDataCNOfShort + " Launch .";
                        rpt.WFState = WFState.Blank;
                        rpt.FlowStarter = emp.No;
                        rpt.FK_NY = DataType.CurrentYearMonth;
                        if (Glo.UserInfoShowModel == UserInfoShowModel.UserNameOnly)
                            rpt.FlowEmps = "@" + emp.Name;

                        if (Glo.UserInfoShowModel == UserInfoShowModel.UserIDUserName)
                            rpt.FlowEmps = "@" + emp.No;

                        if (Glo.UserInfoShowModel == UserInfoShowModel.UserIDUserName)
                            rpt.FlowEmps = "@" + emp.No + "," + emp.Name;

                        rpt.FlowEnderRDT = BP.DA.DataType.CurrentDataTime;
                        rpt.FK_Dept = emp.FK_Dept;
                        rpt.FlowEnder = emp.No;
                        rpt.FlowEndNode = this.StartNodeID;

                        // Generate a document number .
                        string billNoFormat = this.BillNoFormat.Clone() as string;
                        if (string.IsNullOrEmpty(billNoFormat) == false)
                            rpt.BillNo = BP.WF.Glo.GenerBillNo(billNoFormat, rpt.OID, rpt, this.PTable);

                        rpt.DirectUpdate();
                    }
                    else
                    {
                        rpt.OID = wk.OID;
                        rpt.FID = 0;
                        rpt.FlowStartRDT = BP.DA.DataType.CurrentDataTime;
                        rpt.FlowEnderRDT = BP.DA.DataType.CurrentDataTime;
                        rpt.MyNum = 0;

                        rpt.Title = WorkNode.GenerTitle(this, wk);
                        // rpt.Title = WebUser.No + "," + BP.Web.WebUser.Name + "在" + DataType.CurrentDataCNOfShort + " Launch .";

                        rpt.WFState = WFState.Blank;
                        rpt.FlowStarter = emp.No;

                        rpt.FlowEndNode = this.StartNodeID;
                        if (Glo.UserInfoShowModel == UserInfoShowModel.UserNameOnly)
                            rpt.FlowEmps = "@" + emp.Name;

                        if (Glo.UserInfoShowModel == UserInfoShowModel.UserIDUserName)
                            rpt.FlowEmps = "@" + emp.No;

                        if (Glo.UserInfoShowModel == UserInfoShowModel.UserIDUserName)
                            rpt.FlowEmps = "@" + emp.No + "," + emp.Name;

                        rpt.FlowEndNode = this.StartNodeID;
                        rpt.FK_NY = DataType.CurrentYearMonth;
                        rpt.FK_Dept = emp.FK_Dept;

                        rpt.FlowEnder = emp.No;
                        rpt.FlowEndNode = this.StartNodeID;

                        // Generate a document number .
                        string billNoFormat = this.BillNoFormat.Clone() as string;
                        if (string.IsNullOrEmpty(billNoFormat) == false)
                            rpt.BillNo = BP.WF.Glo.GenerBillNo(billNoFormat, rpt.OID, rpt, this.PTable);

                        rpt.InsertAsOID(wk.OID);
                    }
                }
            }
            catch(Exception ex)
            {
                wk.CheckPhysicsTable();
                throw ex;
            }

            if (SystemConfig.IsBSsystem == true)
            {
                //  Record this id , Do not let the other in replication time is modified .
                Int64 newOID = wk.OID;
                if (IsNewWorkID == true)
                {
                    //  Processing the passed parameter .
                    int i = 0;
                    foreach (string k in BP.Sys.Glo.Request.QueryString.AllKeys)
                    {
                        i++;
                        wk.SetValByKey(k, BP.Sys.Glo.Request.QueryString[k]);
                    }

                    if (i >= 3)
                    {
                        wk.OID = newOID;
                        wk.DirectUpdate();
                    }
                }

                #region  Demand processing delete draft .
                if (BP.Sys.Glo.Request.QueryString["IsDeleteDraft"] == "1")
                {
                    /* Do you want to delete Draft */
                    Int64 oid = wk.OID;
                    wk.ResetDefaultVal();
                    wk.DirectUpdate();

                    MapDtls dtls = wk.HisMapDtls;
                    foreach (MapDtl dtl in dtls)
                        DBAccess.RunSQL("DELETE FROM " + dtl.PTable + " WHERE RefPK=" + oid);

                    // Remove attachment data .
                    DBAccess.RunSQL("DELETE FROM Sys_FrmAttachmentDB WHERE FK_MapData='ND" + wk.NodeID + "' AND RefPKVal='" + wk.OID + "'");
                    wk.OID = newOID;
                }
                #endregion  Demand processing delete draft .

                #region  Processing start node ,  If you pass over  FromTableName  Is from this list copy Data .
                if (BP.Sys.Glo.Request.QueryString["FromTableName"] != null)
                {
                    string tableName = BP.Sys.Glo.Request.QueryString["FromTableName"];
                    string tablePK = BP.Sys.Glo.Request.QueryString["FromTablePK"];
                    string tablePKVal = BP.Sys.Glo.Request.QueryString["FromTablePKVal"];

                    DataTable dt = DBAccess.RunSQLReturnTable("SELECT * FROM " + tableName + " WHERE " + tablePK + "='" + tablePKVal + "'");
                    if (dt.Rows.Count == 0)
                        throw new Exception("@ Use table Transfer data error , Did not find the specified line data , Unable to populate the data for the user .");

                    string innerKeys = ",OID,RDT,CDT,FID,WFState,";
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (innerKeys.Contains("," + dc.ColumnName + ","))
                            continue;

                        wk.SetValByKey(dc.ColumnName, dt.Rows[0][dc.ColumnName].ToString());
                        rpt.SetValByKey(dc.ColumnName, dt.Rows[0][dc.ColumnName].ToString());

                    }
                    rpt.Update();
                }
                #endregion  Processing start node ,  If you pass over  FromTableName  Is from this list copy Data .

                #region  Get special flag variable 
                //  Get special flag variable .
                string PFlowNo = null;
                string PNodeIDStr = null;
                string PWorkIDStr = null;
                string PFIDStr = null;

                string CopyFormWorkID = BP.Sys.Glo.Request.QueryString["CopyFormWorkID"];
                if (string.IsNullOrEmpty(CopyFormWorkID) == false)
                {
                    PFlowNo = this.No;
                    PNodeIDStr = BP.Sys.Glo.Request.QueryString["CopyFormNode"];
                    PWorkIDStr = CopyFormWorkID;
                    PFIDStr = "0";
                }
                else
                {
                    PFlowNo = BP.Sys.Glo.Request.QueryString["PFlowNo"];
                    PNodeIDStr = BP.Sys.Glo.Request.QueryString["PNodeID"];
                    PWorkIDStr = BP.Sys.Glo.Request.QueryString["PWorkID"];
                    PFIDStr = BP.Sys.Glo.Request.QueryString["PFID"]; // Parent process .
                }
                #endregion  Get special flag variable 

                #region   Determine whether the load on a data .
                if (this.IsLoadPriData == true && this.StartGuideWay == BP.WF.Template.StartGuideWay.None)
                {
                    /*  If you need a process instance from the copy Data . */
                    string sql = "SELECT OID FROM " + this.PTable + " WHERE FlowStarter='" + WebUser.No + "' AND OID!=" + wk.OID + " ORDER BY OID DESC";
                    string workidPri = DBAccess.RunSQLReturnStringIsNull(sql, "0");
                    if (workidPri == "0")
                    {
                        /* Description no first data .*/
                    }
                    else
                    {
                        PFlowNo = this.No;
                        PNodeIDStr = int.Parse(this.No) + "01";
                        PWorkIDStr = workidPri;
                        PFIDStr = "0";
                        CopyFormWorkID = workidPri;
                    }
                }
                #endregion   Determine whether the load on a data .

                #region  Data transfer between processing 1.
                if (string.IsNullOrEmpty(PNodeIDStr) == false && string.IsNullOrEmpty(PWorkIDStr) == false)
                {
                    Int64 PWorkID = Int64.Parse(PWorkIDStr);
                    Int64 PNodeID = 0;
                    if (CopyFormWorkID != null)
                        PNodeID = Int64.Parse(PNodeIDStr);

                    /*  If it is passed from one process over another , Also consider the flow of data .*/

                    #region copy  First, from the parent process NDxxxRpt copy.
                    Int64 pWorkIDReal = 0;
                    Flow pFlow = new Flow(PFlowNo);
                    string pOID = "";
                    if (string.IsNullOrEmpty(PFIDStr) == true || PFIDStr == "0")
                        pOID = PWorkID.ToString();
                    else
                        pOID = PFIDStr;

                    string sql = "SELECT * FROM " + pFlow.PTable + " where OID=" + pOID;
                    DataTable dt = DBAccess.RunSQLReturnTable(sql);
                    if (dt.Rows.Count != 1)
                        throw new Exception("@ Should not be less than the parent process data query ,  One possible , Make sure to call the parent node process is sub-line city , But the city does not handle line FID Arguments passed in .");
                    wk.Copy(dt.Rows[0]);
                    rpt.Copy(dt.Rows[0]);
                    #endregion copy  First, from the parent process NDxxxRpt copy.

                    #region  Called from the node copy.
                    BP.WF.Node fromNd = new BP.WF.Node(int.Parse(PNodeIDStr));
                    Work wkFrom = fromNd.HisWork;
                    wkFrom.OID = PWorkID;
                    if (wkFrom.RetrieveFromDBSources() == 0)
                        throw new Exception("@ Parent process work ID Incorrect , No query to the data " + PWorkID);
                    //wk.Copy(wkFrom);
                    //rpt.Copy(wkFrom);
                    #endregion  Called from the node copy.

                    #region  Get web Variable .
                    foreach (string k in BP.Sys.Glo.Request.QueryString.AllKeys)
                    {
                        wk.SetValByKey(k, BP.Sys.Glo.Request.QueryString[k]);
                        rpt.SetValByKey(k, BP.Sys.Glo.Request.QueryString[k]);
                    }
                    #endregion  Get web Variable .

                    #region  Special Assignment .
                    wk.OID = newOID;
                    rpt.OID = newOID;

                    //  In execution copy后, These two fields will likely be washed away .
                    if (CopyFormWorkID != null)
                    {
                        /* If it is not executed from the process has been completed copy.*/

                        wk.SetValByKey(StartWorkAttr.PFlowNo, PFlowNo);
                        wk.SetValByKey(StartWorkAttr.PNodeID, PNodeID);
                        wk.SetValByKey(StartWorkAttr.PWorkID, PWorkID);

                        rpt.SetValByKey(GERptAttr.PFlowNo, PFlowNo);
                        rpt.SetValByKey(GERptAttr.PNodeID, PNodeID);
                        rpt.SetValByKey(GERptAttr.PWorkID, PWorkID);
                    }

                    if (rpt.EnMap.PhysicsTable != wk.EnMap.PhysicsTable)
                        wk.Update(); // Updating the node data .
                    rpt.Update(); //  Update Process Data Sheet .
                    #endregion  Special Assignment .

                    #region  Other forms of data replication .
                    // Copy the details .
                    MapDtls dtls = wk.HisMapDtls;
                    if (dtls.Count > 0)
                    {
                        MapDtls dtlsFrom = wkFrom.HisMapDtls;
                        int idx = 0;
                        if (dtlsFrom.Count == dtls.Count)
                        {
                            foreach (MapDtl dtl in dtls)
                            {
                                if (dtl.IsCopyNDData == false)
                                    continue;

                                //new  An example of .
                                GEDtl dtlData = new GEDtl(dtl.No);
                                MapDtl dtlFrom = dtlsFrom[idx] as MapDtl;

                                GEDtls dtlsFromData = new GEDtls(dtlFrom.No);
                                dtlsFromData.Retrieve(GEDtlAttr.RefPK, PWorkID);
                                foreach (GEDtl geDtlFromData in dtlsFromData)
                                {
                                    dtlData.Copy(geDtlFromData);
                                    dtlData.RefPK = wk.OID.ToString();
                                    if (this.No == PFlowNo)
                                        dtlData.InsertAsNew();
                                    else
                                        dtlData.SaveAsOID(geDtlFromData.OID);
                                }
                            }
                        }
                    }

                    // Copy the attachment data .
                    if (wk.HisFrmAttachments.Count > 0)
                    {
                        if (wkFrom.HisFrmAttachments.Count > 0)
                        {
                            int toNodeID = wk.NodeID;

                            // Deleting Data .
                            DBAccess.RunSQL("DELETE FROM Sys_FrmAttachmentDB WHERE FK_MapData='ND" + toNodeID + "' AND RefPKVal='" + wk.OID + "'");
                            FrmAttachmentDBs athDBs = new FrmAttachmentDBs("ND" + PNodeIDStr, PWorkID.ToString());

                            foreach (FrmAttachmentDB athDB in athDBs)
                            {
                                FrmAttachmentDB athDB_N = new FrmAttachmentDB();
                                athDB_N.Copy(athDB);
                                athDB_N.FK_MapData = "ND" + toNodeID;
                                athDB_N.RefPKVal = wk.OID.ToString();
                                athDB_N.FK_FrmAttachment = athDB_N.FK_FrmAttachment.Replace("ND" + PNodeIDStr,
                                  "ND" + toNodeID);

                                if (athDB_N.HisAttachmentUploadType == AttachmentUploadType.Single)
                                {
                                    /* If a single attachment .*/
                                    athDB_N.MyPK = athDB_N.FK_FrmAttachment + "_" + wk.OID;
                                    if (athDB_N.IsExits == true)
                                        continue; /* Instructions on a node or sub-thread already copy After a ,  But there is a child thread to pass data to the confluence point may be , It can not be used break.*/
                                    athDB_N.Insert();
                                }
                                else
                                {
                                    athDB_N.MyPK = athDB_N.UploadGUID + "_" + athDB_N.FK_MapData + "_" + wk.OID;
                                    athDB_N.Insert();
                                }
                            }
                        }
                    }
                    #endregion  Other forms of data replication .

                }
                #endregion  Data transfer between processing 1.

                #region  Data transfer between processing 2,  If you want to jump directly to the specified node up .
                if (BP.Sys.Glo.Request.QueryString["JumpToNode"] != null)
                {
                    wk.Rec = WebUser.No;
                    wk.SetValByKey(StartWorkAttr.RDT, BP.DA.DataType.CurrentDataTime);
                    wk.SetValByKey(StartWorkAttr.CDT, BP.DA.DataType.CurrentDataTime);
                    wk.SetValByKey("FK_NY", DataType.CurrentYearMonth);
                    wk.FK_Dept = emp.FK_Dept;
                    wk.SetValByKey("FK_DeptName", emp.FK_DeptText);
                    wk.SetValByKey("FK_DeptText", emp.FK_DeptText);
                    wk.FID = 0;
                    wk.SetValByKey(StartWorkAttr.RecText, emp.Name);

                    int jumpNodeID = int.Parse(BP.Sys.Glo.Request.QueryString["JumpToNode"]);
                    Node jumpNode = new Node(jumpNodeID);

                    string jumpToEmp = BP.Sys.Glo.Request.QueryString["JumpToEmp"];
                    if (string.IsNullOrEmpty(jumpToEmp))
                        jumpToEmp = emp.No;

                    WorkNode wn = new WorkNode(wk, nd);
                    wn.NodeSend(jumpNode, jumpToEmp);

                    WorkFlow wf = new WorkFlow(this, wk.OID, wk.FID);
                    return wf.GetCurrentWorkNode().HisWork as StartWork;
                }
                #endregion  Data transfer between processing .
            }

            wk.Rec = emp.No;
            wk.SetValByKey(WorkAttr.RDT, BP.DA.DataType.CurrentDataTime);
            wk.SetValByKey(WorkAttr.CDT, BP.DA.DataType.CurrentDataTime);
            wk.SetValByKey("FK_NY", DataType.CurrentYearMonth);
            wk.FK_Dept = emp.FK_Dept;
            wk.SetValByKey("FK_DeptName", emp.FK_DeptText);
            wk.SetValByKey("FK_DeptText", emp.FK_DeptText);
            wk.FID = 0;
            wk.SetValByKey(StartWorkAttr.RecText, emp.Name);
            if (wk.IsExits == false)
                wk.DirectInsert();

            return wk;
        }
        /// <summary>
        ///  The system is launched on the handling of each node to accept staff .
        /// </summary>
        /// <param name="currND"></param>
        /// <param name="workid"></param>
        public void InitSelectAccper(Node currND, Int64 workid)
        {
            if (this.IsFullSA == false)
                return;

            // Check out all the nodes .
            Nodes nds = new Nodes(this.No);

            //  Start node requires special handling 》
            /*  If you enable a person to calculate the future treatment  */
            SelectAccper sa = new SelectAccper();

            sa.FK_Emp = WebUser.No;
            sa.FK_Node = currND.NodeID;
            sa.WorkID = workid;
            sa.ResetPK();
            if (sa.RetrieveFromDBSources() == 0)
            {
                sa.AccType = 0;
                sa.EmpName = WebUser.Name;
                sa.Insert();
            }
            else
            {
                sa.AccType = 0;
                sa.EmpName = WebUser.Name;
                sa.Update();
            }


            foreach (Node item in nds)
            {
                if (item.IsStartNode == true)
                    continue;

                // If calculated in accordance with the post （ The default first rule .）
                if (item.HisDeliveryWay == DeliveryWay.ByStation)
                {
                    string sql = "SELECT No, Name FROM Port_Emp WHERE No IN (SELECT A.FK_Emp FROM Port_EmpStation A, WF_NodeStation B WHERE A.FK_Station=B.FK_Station AND B.FK_Node="+item.NodeID+")";
                    DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
                    if (dt.Rows.Count != 1)
                        continue;

                    string no = dt.Rows[0][0].ToString();
                    string name = dt.Rows[0][1].ToString();

                   // sa.Delete(SelectAccperAttr.FK_Node,item.NodeID, SelectAccperAttr.WorkID, workid); // Delete existing data .
                    sa.FK_Emp = no;
                    sa.EmpName = name;
                    sa.FK_Node = item.NodeID;
                    sa.WorkID = workid;
                    sa.Info = "无";
                    sa.AccType = 0;
                    sa.ResetPK();
                    if (sa.IsExits)
                        continue;

                    sa.Insert();
                    continue;
                }
               

                // Treatment with the same personnel specified node .
                if (item.HisDeliveryWay == DeliveryWay.BySpecNodeEmp
                   && item.DeliveryParas == currND.NodeID.ToString())
                {

                    sa.FK_Emp = WebUser.No;
                    sa.FK_Node = item.NodeID;
                    sa.WorkID = workid;
                    sa.Info = "无";
                    sa.AccType = 0;
                    sa.EmpName = WebUser.Name;

                    sa.ResetPK();
                    if (sa.IsExits)
                        continue;

                    sa.Insert();
                    continue;
                }

                // Binding node processing staff ..
                if (item.HisDeliveryWay == DeliveryWay.ByBindEmp)
                {
                    NodeEmps nes = new NodeEmps();
                    nes.Retrieve(NodeEmpAttr.FK_Node, item.NodeID);
                    foreach (NodeEmp ne in nes)
                    {
                        sa.FK_Emp = ne.FK_Emp;
                        sa.FK_Node = item.NodeID;
                        sa.WorkID = workid;
                        sa.Info = "无";
                        sa.AccType = 0;
                        sa.EmpName = ne.FK_EmpT;

                        sa.ResetPK();
                        if (sa.IsExits)
                            continue;

                        sa.Insert();
                    }
                }

                // According to the node   Intersection with the department of computing jobs .
                #region  By department and job intersection of computing .
                if (item.HisDeliveryWay == DeliveryWay.ByDeptAndStation)
                {
                    string dbStr = BP.Sys.SystemConfig.AppCenterDBVarStr;
                    string sql = "SELECT No FROM Port_Emp WHERE No IN ";
                    sql += "(SELECT FK_Emp FROM Port_EmpDept WHERE FK_Dept IN ";
                    sql += "( SELECT FK_Dept FROM WF_NodeDept WHERE FK_Node=" + dbStr + "FK_Node1)";
                    sql += ")";
                    sql += "AND No IN ";
                    sql += "(";
                    sql += "SELECT FK_Emp FROM Port_EmpStation WHERE FK_Station IN ";
                    sql += "( SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + dbStr + "FK_Node1 )";
                    sql += ") ORDER BY No ";

                    Paras ps = new Paras();
                    ps.Add("FK_Node1", item.NodeID);
                    ps.Add("FK_Node2", item.NodeID);
                    ps.SQL = sql;
                    DataTable dt = DBAccess.RunSQLReturnTable(ps);
                    foreach (DataRow dr in dt.Rows)
                    {
                        Emp emp = new Emp(dr[0].ToString());
                        sa.FK_Emp = emp.No;
                        sa.FK_Node = item.NodeID;
                        sa.WorkID = workid;
                        sa.Info = "无";
                        sa.AccType = 0;
                        sa.EmpName = emp.Name;

                        sa.ResetPK();
                        if (sa.IsExits)
                            continue;

                        sa.Insert();
                    }
                }
                #endregion  By department and job intersection of computing .
            }


            // Prefabricated current node arrival node data .
            Nodes toNDs = currND.HisToNodes;
            foreach (Node item in toNDs)
            {
                if (item.HisDeliveryWay == DeliveryWay.ByStation)
                {
                    /* If the access according to job */
                    #region  Final judgment  -  In accordance with the job to execute .
                    string dbStr = BP.Sys.SystemConfig.AppCenterDBVarStr;
                    string sql = "";
                    Paras ps = new Paras();
                    /*  If the node is executed  与  Accept node positions inconsistent collection  */
                    /*  Under no inquiry into the circumstances ,  According to the department to calculate .*/
                    if (this.FlowAppType == FlowAppType.Normal)
                    {
                        switch (BP.Sys.SystemConfig.AppCenterDBType)
                        {
                            case DBType.MySQL:
                            case DBType.MSSQL:
                                sql = "select No from Port_Emp x inner join (select FK_Emp from Port_EmpStation a inner join WF_NodeStation b ";
                                sql += " on a.FK_Station=b.FK_Station where FK_Node=" + dbStr + "FK_Node) as y on x.No=y.FK_Emp inner join Port_EmpDept z on";
                                sql += " x.No=z.FK_Emp where z.FK_Dept =" + dbStr + "FK_Dept order by x.No";
                                break;
                            default:
                                sql = "SELECT No FROM Port_Emp WHERE NO IN "
                              + "(SELECT  FK_Emp  FROM Port_EmpStation WHERE FK_Station IN (SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + dbStr + "FK_Node) )"
                              + " AND  NO IN "
                              + "(SELECT  FK_Emp  FROM Port_EmpDept WHERE FK_Dept =" + dbStr + "FK_Dept)";
                                sql += " ORDER BY No ";
                                break;
                        }

                        ps = new Paras();
                        ps.SQL = sql;
                        ps.Add("FK_Node", item.NodeID);
                        ps.Add("FK_Dept", WebUser.FK_Dept);
                    }

                    DataTable dt = DBAccess.RunSQLReturnTable(ps);
                    foreach (DataRow dr in dt.Rows)
                    {
                        Emp emp = new Emp(dr[0].ToString());
                        sa.FK_Emp = emp.No;
                        sa.FK_Node = item.NodeID;
                        sa.WorkID = workid;
                        sa.Info = "无";
                        sa.AccType = 0;
                        sa.EmpName = emp.Name;

                        sa.ResetPK();
                        if (sa.IsExits)
                            continue;

                        sa.Insert();
                    }
                    #endregion   In accordance with the job to execute .
                }
            }
        }
        #endregion  Creating new jobs .

        #region  Initialize a job .
        /// <summary>
        ///  Initialize a job 
        /// </summary>
        /// <param name="workid"></param>
        /// <param name="fk_node"></param>
        /// <returns></returns>
        public Work GenerWork(Int64 workid, Node nd)
        {
            Work wk = nd.HisWork;
           // wk.ResetDefaultVal();
            wk.OID = workid;
            if (wk.RetrieveFromDBSources() == 0)
            {
                /*
                 * 2012-10-15  Stumbled upon a job loss situation , WF_GenerWorkerlist WF_GenerWorkFlow  This data has , There is no reason to identify missing . stone.
                 *  Use the following code to automatically fix , But experience data copy Incomplete question .
                 * */
#warning 2011-10-15  Stumbled upon a job loss situation .

                string fk_mapData = "ND" + int.Parse(this.No) + "Rpt";
                GERpt rpt = new GERpt(fk_mapData);
                rpt.OID = int.Parse(workid.ToString());
                if (rpt.RetrieveFromDBSources() >= 1)
                {
                    /*   Query to report data .  */
                    wk.Copy(rpt);
                    wk.Rec = WebUser.No;
                    wk.InsertAsOID(workid);
                }
                else
                {
                    /*   No inquiry into the report data .  */

#warning  Exception information should not appear here .

                    string msg = "@ Exception should not happen .";
                    msg += "@ For the node NodeID=" + nd.NodeID + " workid:" + workid + "  Acquiring data .";
                    msg += "@ Get it Rpt Table data , Should not be less than the query .";
                    msg += "@GERpt  Information : table:" + rpt.EnMap.PhysicsTable + "   OID=" + rpt.OID;

                    string sql = "SELECT count(*) FROM " + rpt.EnMap.PhysicsTable + " WHERE OID=" + workid;
                    int num = DBAccess.RunSQLReturnValInt(sql);

                    msg += " @SQL:" + sql;
                    msg += " ReturnNum:" + num;
                    if (num == 0)
                    {
                        msg += " Has been used sql You can check out the , But should not check it out with the class .";
                    }
                    else
                    {
                        /* If you can use sql  Check out .*/
                        num = rpt.RetrieveFromDBSources();
                        msg += "@from rpt.RetrieveFromDBSources = " + num;
                    }

                    Log.DefaultLogWriteLineError(msg);

                    MapData md = new MapData("ND" + int.Parse(nd.FK_Flow) + "01");
                    sql = "SELECT * FROM " + md.PTable + " WHERE OID=" + workid;
                    DataTable dt = DBAccess.RunSQLReturnTable(sql);
                    if (dt.Rows.Count == 1)
                    {
                        rpt.Copy(dt.Rows[0]);
                        try
                        {
                            rpt.FlowStarter = dt.Rows[0][StartWorkAttr.Rec].ToString();
                            rpt.FlowStartRDT = dt.Rows[0][StartWorkAttr.RDT].ToString();
                            rpt.FK_Dept = dt.Rows[0][StartWorkAttr.FK_Dept].ToString();
                        }
                        catch
                        {
                        }

                        rpt.OID = int.Parse(workid.ToString());
                        try
                        {
                            rpt.InsertAsOID(rpt.OID);
                        }
                        catch (Exception ex)
                        {
                            Log.DefaultLogWriteLineError("@ Should not be inserted into the  rpt:" + rpt.EnMap.PhysicsTable + " workid=" + workid);
                            rpt.RetrieveFromDBSources();
                        }
                    }
                    else
                    {
                        Log.DefaultLogWriteLineError("@ No data found start node , NodeID:" + nd.NodeID + " workid:" + workid);
                        throw new Exception("@ No data found start node , NodeID:" + nd.NodeID + " workid:" + workid + " SQL:" + sql);
                    }

#warning  Job loss should not appear .
                    Log.DefaultLogWriteLineError("@ The work [" + nd.NodeID + " : " + wk.EnDesc + "],  Report Data WorkID=" + workid + "  Lose ,  No from NDxxxRpt Records found , Please contact the administrator .");

                    wk.Copy(rpt);
                    wk.Rec = WebUser.No;
                    wk.ResetDefaultVal();
                    wk.Insert();
                }
            }

            #region  Determine whether there needs to delete draft .
            if (SystemConfig.IsBSsystem == true && nd.IsStartNode && BP.Sys.Glo.Request.QueryString["IsDeleteDraft"] == "1")
            {
                /* Need to remove the draft .*/
                /* Do you want to delete Draft */
                string title = wk.GetValStringByKey("Title");
                wk.ResetDefaultValAllAttr();
                wk.OID = workid;
                wk.SetValByKey(GenerWorkFlowAttr.Title, title);
                wk.DirectUpdate();

                MapDtls dtls = wk.HisMapDtls;
                foreach (MapDtl dtl in dtls)
                    DBAccess.RunSQL("DELETE FROM " + dtl.PTable + " WHERE RefPK=" + wk.OID);

                // Remove attachment data .
                DBAccess.RunSQL("DELETE FROM Sys_FrmAttachmentDB WHERE FK_MapData='ND" + wk.NodeID + "' AND RefPKVal='" + wk.OID + "'");
            }
            #endregion


            //  Set the current staff to record people .
            wk.Rec = WebUser.No;
            wk.RecText = WebUser.Name;
            wk.Rec = WebUser.No;
            wk.SetValByKey(WorkAttr.RDT, BP.DA.DataType.CurrentDataTime);
            wk.SetValByKey(WorkAttr.CDT, BP.DA.DataType.CurrentDataTime);
            wk.SetValByKey(GERptAttr.WFState, WFState.Runing);
            wk.SetValByKey("FK_Dept", WebUser.FK_Dept);
            wk.SetValByKey("FK_DeptName", WebUser.FK_DeptName);
            wk.SetValByKey("FK_DeptText", WebUser.FK_DeptName);
            wk.FID = 0;
            wk.SetValByKey("RecText", WebUser.Name);

            return wk;
        }
        #endregion  Initialize a job 

        #region  Other common methods .
        /// <summary>
        ///  Automatically initiates 
        /// </summary>
        /// <returns></returns>
        public string DoAutoStartIt()
        {
            switch (this.HisFlowRunWay)
            {
                case BP.WF.FlowRunWay.SpecEmp: // Designated personnel to run on time .
                    string RunObj = this.RunObj;
                    string FK_Emp = RunObj.Substring(0, RunObj.IndexOf('@'));
                    BP.Port.Emp emp = new BP.Port.Emp();
                    emp.No = FK_Emp;
                    if (emp.RetrieveFromDBSources() == 0)
                    {
                        return " Automatically start the process error : Sponsor (" + FK_Emp + ") Does not exist .";
                    }

                    BP.Web.WebUser.SignInOfGener(emp);
                    string info_send= BP.WF.Dev2Interface.Node_StartWork(this.No,null,null,0,null,0,null).ToMsgOfText();
                    if (WebUser.No != "admin")
                    {
                        emp = new BP.Port.Emp();
                        emp.No = "admin";
                        emp.Retrieve();
                        BP.Web.WebUser.SignInOfGener(emp);
                        return info_send;
                    }
                    return info_send;
                case BP.WF.FlowRunWay.DataModel: // Driven by the data collection mode execution .
                    break;
                default:
                    return "@ The process you are not set to automatically start the process type .";
            }

            string msg = "";
            BP.Sys.MapExt me = new MapExt();
            me.MyPK = "ND" + int.Parse(this.No) + "01_" + MapExtXmlList.StartFlow;
            int i = me.RetrieveFromDBSources();
            if (i == 0)
            {
                BP.DA.Log.DefaultLogWriteLineError(" No for the process (" + this.Name + ") The start node is set to initiate data , Please refer to the instructions to solve .");
                return " No for the process (" + this.Name + ") The start node is set to initiate data , Please refer to the instructions to solve .";
            }
            if (string.IsNullOrEmpty(me.Tag))
            {
                BP.DA.Log.DefaultLogWriteLineError(" No for the process (" + this.Name + ") The start node is set to initiate data , Please refer to the instructions to solve .");
                return " No for the process (" + this.Name + ") The start node is set to initiate data , Please refer to the instructions to solve .";
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

            #region  Check the data source is correct .
            string errMsg = "";
            //  Get the main table data .
            DataTable dtMain = BP.DA.DBAccess.RunSQLReturnTable(me.Tag);
            if (dtMain.Rows.Count == 0)
            {
                return " Process (" + this.Name + ") At this time no task , Query :"+me.Tag.Replace("'","]");
            }

            msg+="@ Queried (" + dtMain.Rows.Count + ") Article task .";

            if (dtMain.Columns.Contains("Starter") == false)
                errMsg += "@ The main table with a value of no Starter列.";

            if (dtMain.Columns.Contains("MainPK") == false)
                errMsg += "@ The main table with a value of no MainPK列.";

            if (errMsg.Length > 2)
            {
                return " Process (" + this.Name + ") The start node is set to initiate data , Incomplete ." + errMsg;
            }
            #endregion  Check the data source is correct .

            #region  Processing launched .
          
               string fk_mapdata = "ND" + int.Parse(this.No) + "01";

            MapData md = new MapData(fk_mapdata);
            int idx = 0;
            foreach (DataRow dr in dtMain.Rows)
            {
                idx++;

                string mainPK = dr["MainPK"].ToString();
                string sql = "SELECT OID FROM " + md.PTable + " WHERE MainPK='" + mainPK + "'";
                if (DBAccess.RunSQLReturnTable(sql).Rows.Count != 0)
                {
                    msg+= "@" + this.Name + ",第" + idx + "条, This task has been completed before .";
                    continue; /* Description been scheduled over */
                }

                string starter = dr["Starter"].ToString();
                if (WebUser.No != starter)
                {
                    BP.Web.WebUser.Exit();
                    BP.Port.Emp emp = new BP.Port.Emp();
                    emp.No = starter;
                    if (emp.RetrieveFromDBSources() == 0)
                    {
                        msg+="@" + this.Name + ",第" + idx + "条, Sponsored personnel set :" + emp.No + " Does not exist .";
                       msg+="@ Data-driven approach to initiate the process (" + this.Name + ") Sponsored personnel set :" + emp.No + " Does not exist .";
                        continue;
                    }
                    WebUser.SignInOfGener(emp);
                }

                #region   To value .
                Work wk = this.NewWork();
                foreach (DataColumn dc in dtMain.Columns)
                    wk.SetValByKey(dc.ColumnName, dr[dc.ColumnName].ToString());

                if (ds.Tables.Count != 0)
                {
                    // MapData md = new MapData(nodeTable);
                    MapDtls dtls = md.MapDtls; // new MapDtls(nodeTable);
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
                                if (drDtl["RefMainPK"].ToString() != mainPK)
                                    continue;

                                dtlEn = dtl.HisGEDtl;
                                foreach (DataColumn dc in dt.Columns)
                                    dtlEn.SetValByKey(dc.ColumnName, drDtl[dc.ColumnName].ToString());

                                dtlEn.RefPK = wk.OID.ToString();
                                dtlEn.OID = 0;
                                dtlEn.Insert();
                            }
                        }
                    }
                }
                #endregion   To value .

                //  Send information processing .
                Node nd = this.HisStartNode;
                try
                {
                    WorkNode wn = new WorkNode(wk, nd);
                    string infoSend = wn.NodeSend().ToMsgOfHtml();
                    BP.DA.Log.DefaultLogWriteLineInfo(msg);
                    msg+= "@" + this.Name + ",第" + idx + "条, Sponsored staff :" + WebUser.No + "-" + WebUser.Name + " Completed .\r\n" + infoSend;
                    //this.SetText("@第（" + idx + "） Article task ," + WebUser.No + " - " + WebUser.Name + " Has been completed .\r\n" + msg);
                }
                catch (Exception ex)
                {
                    msg+= "@" + this.Name + ",第" + idx + "条, Sponsored staff :" + WebUser.No + "-" + WebUser.Name + " An error occurred while initiating .\r\n" + ex.Message;
                }
                  msg+= "<hr>";
            }
            return msg;
            #endregion  Processing launched .
        }
       
        /// <summary>
        /// UI Access control interface 
        /// </summary>
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                if (BP.Web.WebUser.No == "admin")
                    uac.IsUpdate = true;
                return uac;
            }
        }
        /// <summary>
        ///  Repair process data view 
        /// </summary>
        /// <returns></returns>
        public static string RepareV_FlowData_View()
        {
            string err = "";
            Flows fls = new Flows();
            fls.RetrieveAllFromDBSource();

            if (fls.Count == 0)
                return null;

            string sql = "";
            sql = "CREATE VIEW V_FlowData (FK_FlowSort,FK_Flow,OID,FID,Title,WFState,CDT,FlowStarter,FlowStartRDT,FK_Dept,FK_NY,FlowDaySpan,FlowEmps,FlowEnder,FlowEnderRDT,FlowEndNode,MyNum, PWorkID,PFlowNo,BillNo,ProjNo) ";
            //     sql += "\t\n /*  WorkFlow Data " + DateTime.Now.ToString("yyyy-MM-dd") + " */ ";
            sql += " AS ";
            foreach (Flow fl in fls)
            {
                if (fl.IsCanStart==false)
                    continue;


                string mysql = "\t\n SELECT '" + fl.FK_FlowSort + "' AS FK_FlowSort,'" + fl.No + "' AS FK_Flow,OID,FID,Title,WFState,CDT,FlowStarter,FlowStartRDT,FK_Dept,FK_NY,FlowDaySpan,FlowEmps,FlowEnder,FlowEnderRDT,FlowEndNode,1 as MyNum,PWorkID,PFlowNo,BillNo,ProjNo FROM " + fl.PTable + " WHERE WFState >1 ";
                try
                {
                    DBAccess.RunSQLReturnTable(mysql);
                }
                catch (Exception ex)
                {
                    continue;
                    try
                    {
                        fl.DoCheck();
                        DBAccess.RunSQLReturnTable(mysql);
                    }
                    catch (Exception ex1)
                    {
                        err += ex1.Message;
                        continue;
                    }
                }

                if (fls.Count == 1)
                    break;

                sql += mysql;
                sql += "\t\n UNION ";
            }
            if (sql.Contains("SELECT") == false)
                return null;

            if (fls.Count >1 )
            sql = sql.Substring(0, sql.Length - 6);

            if (sql.Length > 20)
            {

                #region  Delete  V_FlowData
                try
                {
                    DBAccess.RunSQL("DROP VIEW V_FlowData");
                }
                catch
                {
                    try
                    {
                        DBAccess.RunSQL("DROP table V_FlowData");
                    }
                    catch
                    {
                    }
                }
                #endregion  Delete  V_FlowData

                #region  Create a view .
                try
                {
                    DBAccess.RunSQL(sql);
                }
                catch
                {
                }
                #endregion  Create a view .

            }
            return null;
        }

        public string DoCheck() {
            return DoCheck(true);
        }

        /// <summary>
        ///  Validation process 
        /// </summary>
        /// <returns></returns>
        public string DoCheck(bool dirUpdate)
        {
            #region  Inspection process forms 
            FrmNodes fns = new FrmNodes();
            fns.Retrieve(FrmNodeAttr.FK_Flow, this.No);
            string frms = "";
            string err = "";
            foreach (FrmNode item in fns)
            {
                if (frms.Contains(item.FK_Frm + ","))
                    continue;

                frms += item.FK_Frm+",";
                try
                {
                    MapData md = new MapData(item.FK_Frm);
                    md.RepairMap();
                    Entity en = md.HisEn;
                    en.CheckPhysicsTable();
                }
                catch(Exception ex)
                {
                    err += "@ Node bound form :" + item.FK_Frm + ", Has been deleted . Exception Information ." + ex.Message;
                }
            }
            #endregion

            try
            {
                //  Setting Process Name .
                DBAccess.RunSQL("UPDATE WF_Node SET FlowName = (SELECT Name FROM WF_Flow WHERE NO=WF_Node.FK_Flow)");

                // Delete junk , Illegal Data .
                string sqls = "DELETE FROM Sys_FrmSln where fk_mapdata not in (select no from sys_mapdata)";
                sqls += "@ DELETE FROM WF_Direction WHERE Node=ToNode";
                DBAccess.RunSQLs(sqls);

                // Updated calculations .
                this.NumOfBill = DBAccess.RunSQLReturnValInt("SELECT count(*) FROM WF_BillTemplate WHERE NodeID IN (select NodeID from WF_Flow WHERE no='" + this.No + "')");
                this.NumOfDtl = DBAccess.RunSQLReturnValInt("SELECT count(*) FROM Sys_MapDtl WHERE FK_MapData='ND" + int.Parse(this.No) + "Rpt'");
                if (dirUpdate)
                {
                    this.DirectUpdate();
                }
                string msg = "@  =======   With respect to 《" + this.Name + " 》 Process inspection report   ============";
                  msg += "@ Information output is divided into three :  Information    Caveat    Error .  If you encounter an error output , You must go to modify or set .";
                  msg += "@ Inspection process is not currently covered by 100% Error , Need to manually run a process designed to ensure the correctness of .";
                
                Nodes nds = new Nodes(this.No);

                // Document templates .
                BillTemplates bks = new BillTemplates(this.No);

                // Set of conditions .
                Conds conds = new Conds(this.No);

                #region  Check node 
                foreach (Node nd in nds)
                {
                    // Set its Location Type .
                    nd.SetValByKey(NodeAttr.NodePosType, (int)nd.GetHisNodePosType());

                    msg += "@ Information : --------  Start checking node ID:("+nd.NodeID+") Name :("+nd.Name+") Information  -------------";

                    #region  Number of nodes forms database repair .
                    msg += "@ Information : Began to supplement & Repairing node necessary fields ";
                    try
                    {
                        nd.RepareMap();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("@ An error occurred while repair node table required field :"+nd.Name + " - " + ex.Message);
                    }

                    msg += "@ Information : Began to repair the physical node table .";
                    DBAccess.RunSQL("UPDATE Sys_MapData SET Name='" + nd.Name + "' WHERE No='ND" + nd.NodeID + "'");
                    try
                    {
                        nd.HisWork.CheckPhysicsTable();
                    }
                    catch (Exception ex)
                    {
                        msg += "@ An error occurred while checking node table fields :" +"NodeID"+nd.NodeID + " Table:"+nd.HisWork.EnMap.PhysicsTable +" Name:"+ nd.Name + " ,  Node Type NodeWorkTypeText=" + nd.NodeWorkTypeText + " Error .@err=" + ex.Message;
                    }

                    //  From table to check .
                    Sys.MapDtls dtls = new BP.Sys.MapDtls("ND" + nd.NodeID);
                    foreach (Sys.MapDtl dtl in dtls)
                    {
                        msg += "@ Check the schedule :" + dtl.Name;
                        try
                        {
                            dtl.HisGEDtl.CheckPhysicsTable();
                        }
                        catch(Exception ex)
                        {
                            msg += "@ Check the schedule time error " + ex.Message;
                        }
                    }
                    #endregion  Number of nodes forms database repair .

                    MapAttrs mattrs = new MapAttrs("ND" + nd.NodeID);

                    #region  Access rule node checks 

                    msg += "@ Information : Node access rules began to be checked .";

                    switch (nd.HisDeliveryWay)
                    {
                        case DeliveryWay.ByStation:
                            if (nd.NodeStations.Count == 0)
                                msg += "@ Error : You set up access rules of the node is by post , But you do not have to node binding posts .";
                            break;
                        case DeliveryWay.ByDept:
                            if (nd.NodeDepts.Count == 0)
                                msg += "@ Error : You set up access rules of the node is by sector , But you do not have to node binding department .";
                            break;
                        case DeliveryWay.ByBindEmp:
                            if (nd.NodeEmps.Count == 0)
                                msg += "@ Error : You set up access rules to the node is based on staff , But you do not have to bind the node staff .";
                            break;
                        case DeliveryWay.BySpecNodeEmp: /* Calculation specified positions .*/
                        case DeliveryWay.BySpecNodeEmpStation: /* Calculation specified positions .*/
                            if (nd.DeliveryParas.Trim().Length == 0)
                            {
                                msg += "@ Error : You set the node access rules are calculated according to the specified positions , But you do not set the node number .</font>";
                            }
                            else
                            {
                                if (DataType.IsNumStr(nd.DeliveryParas) == false)
                                {
                                    msg += "@ Error : You do not set the specified node number posts , Currently set for {" + nd.DeliveryParas + "}";
                                }
                            }
                            break;
                        case DeliveryWay.ByDeptAndStation: /* By department and job intersection of computing .*/
                            string mysql = "SELECT No FROM Port_Emp WHERE No IN (SELECT FK_Emp FROM Port_EmpDept WHERE FK_Dept IN ( SELECT FK_Dept FROM WF_NodeDept WHERE FK_Node=" + nd.NodeID + "))AND No IN (SELECT FK_Emp FROM Port_EmpStation WHERE FK_Station IN ( SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + nd.NodeID + " )) ORDER BY No ";
                            DataTable mydt = DBAccess.RunSQLReturnTable(mysql);
                            if (mydt.Rows.Count==0)
                                msg += "@ Error : According to the intersection of the post and miscalculations sector , There were no collections {" + mysql + "}";
                            break;
                        case DeliveryWay.BySQL:
                        case DeliveryWay.BySQLAsSubThreadEmpsAndData:
                            if (nd.DeliveryParas.Trim().Length == 0)
                            {
                                msg += "@ Error : You set access rules is based on the node SQL Inquiry , But you did not set the query node properties in sql,此sql The requirement is that the query must contain No,Name Two columns ,sql Expressions support @+ Field variables , Develop detailed reference manual .";
                            }
                            else
                            {
                                try
                                {
                                    string sql = nd.DeliveryParas;
                                    foreach (MapAttr item in mattrs)
                                    {
                                        if (item.IsNum)
                                            sql = sql.Replace("@" + item.KeyOfEn, "0");
                                        else
                                            sql = sql.Replace("@" + item.KeyOfEn, "'0'");
                                    }

                                    sql = sql.Replace("@WebUser.No", "'ss'");
                                    sql = sql.Replace("@WebUser.Name", "'ss'");
                                    sql = sql.Replace("@WebUser.FK_Dept", "'ss'");
                                    sql = sql.Replace("@WebUser.FK_DeptName", "'ss'");

                                    if (sql.Contains("@"))
                                        throw new Exception(" You write sql Fill incorrect variable , The actual implementation , Not be completely replaced down " + sql);

                                    DataTable testDB = null;
                                    try
                                    {
                                        testDB = DBAccess.RunSQLReturnTable(sql);
                                    }
                                    catch (Exception ex)
                                    {
                                        msg += "@ Error : You set access rules is based on the node SQL Inquiry , Execute this statement error ." + ex.Message;
                                    }

                                    if (testDB.Columns.Contains("No") == false || testDB.Columns.Contains("Name") == false)
                                    {
                                        msg += "@ Error : You set access rules is based on the node SQL Inquiry , Set sql Does not comply with the rules ,此sql The requirement is that the query must contain No,Name Two columns ,sql Expressions support @+ Field variables , Develop detailed reference manual .";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    msg += ex.Message;
                                }
                            }
                            break;
                        case DeliveryWay.ByPreviousNodeFormEmpsField:
                            if (mattrs.Contains(BP.Sys.MapAttrAttr.KeyOfEn, nd.DeliveryParas) == false)
                            {
                                /* Check whether there is a node field FK_Emp Field */
                                msg += "@ Error : You set the rules of the node is accessed by personnel designated node form , But you did not increase in node form FK_Emp Field , Develop detailed reference manual .";
                            }
                            if (mattrs.Contains(BP.Sys.MapAttrAttr.KeyOfEn, "FK_Emp") == false)
                            {
                                /* Check whether there is a node field FK_Emp Field */
                                msg += "@ Error : You set the rules of the node is accessed by personnel designated node form , But you did not increase in node form FK_Emp Field , Develop detailed reference manual  .";
                            }
                            break;
                        case DeliveryWay.BySelected: /*  Select from the previous step to send staff  */
                            if (nd.IsStartNode)
                            {
                                msg += "@ Error : You can not set the starting node access rules specified selection staff .";
                                break;
                            }
                            break;
                        case DeliveryWay.ByPreviousNodeEmp: /*  Select from the previous step to send staff  */
                            if (nd.IsStartNode)
                            {
                                msg += "@ Error : Node access rules set error : Start node , Allowed to set up and staff the same as the previous node .";
                                break;
                            }
                            break;
                        default:
                            break;
                    }
                    msg += "@ Access rule node checks completed ....";
                    #endregion

                    #region  Check the node closing conditions , Define the direction of the condition .
                    // Setting it does not complete the process conditions .
                    nd.IsCCFlow = false;

                    if (conds.Count != 0)
                    {
                        msg += "@ Information : Start checking ("+nd.Name+") Direction of the condition :";
                        foreach (Cond cond in conds)
                        {
                            if (cond.FK_Node == nd.NodeID && cond.HisCondType == CondType.Flow)
                            {
                                nd.IsCCFlow = true;
                                nd.Update();
                            }

                            Node ndOfCond = new Node();
                            ndOfCond.NodeID = ndOfCond.NodeID;
                            if (ndOfCond.RetrieveFromDBSources() == 0)
                                continue;

                            try
                            {
                                if (cond.AttrKey.Length < 2)
                                    continue;
                                if (ndOfCond.HisWork.EnMap.Attrs.Contains(cond.AttrKey) == false)
                                    throw new Exception("@ Error : Property :" + cond.AttrKey + " , " + cond.AttrName + "  Does not exist .");
                            }
                            catch (Exception ex)
                            {
                                msg += "@ Error :" + ex.Message;
                                ndOfCond.Delete();
                            }
                            msg += cond.AttrKey + cond.AttrName + cond.OperatorValue + ",";
                        }
                        msg += "@("+nd.Name+") Direction of the condition check is complete .....";
                    }
                    #endregion  Check the condition of the node to complete the definition of .
                }
                #endregion

                msg += "@ Basic information processes : ------ ";
                msg += "@ Serial number :  "+this.No+"  Name :"+this.Name+" ,  Storage Table :"+this.PTable;

                msg += "@ Information : Reports began to check node process .";
                this.DoCheck_CheckRpt(this.HisNodes);

                #region  Check the focus field is still valid 
                msg += "@ Information : Focus field start checking node ";

                // Get gerpt Field .
                GERpt rpt = this.HisGERpt;
                foreach (Node nd in nds)
                {
                    if (nd.FocusField.Trim() == "")
                    {
                        msg += "@ Caveat : Node ID:" + nd.NodeID + "  Name :" + nd.Name + " There is no set focus attribute field , Will result in the trajectory table information into a blank , In order to ensure the flow of the track is readable, set the focus field .";
                        continue;
                    }

                    string strs = nd.FocusField.Clone() as string;
                    
                    strs = Glo.DealExp(strs, rpt, "err");

                    if (strs.Contains("@") == true)
                    {
                        msg += "@ Error : Focus field （" + nd.FocusField + "） Node (step:" + nd.Step + "  Name :" + nd.Name + ") Properties in the settings have been invalid , Form in the field does not exist .";
                    }
                    else
                    {
                        msg += "@ Prompt : Node ("+nd.NodeID+","+nd.Name+") Focus field （" + nd.FocusField + "） Set complete inspection by .";
                    }

                    if (this.IsMD5)
                    {
                        if (nd.HisWork.EnMap.Attrs.Contains(WorkAttr.MD5) == false)
                            nd.RepareMap();
                    }
                }
                msg += "@ Information : Focus field inspection nodes complete .";
                #endregion

                #region  Check the quality of the assessment points .
                msg += "@ Information : Began to check the quality of the assessment points ";
                foreach (Node nd in nds)
                {
                    if (nd.IsEval)
                    {
                        /* If it is the quality of the assessment points , Check nodes form a particular field is not the quality of the assessment ?*/
                        string sql = "SELECT COUNT(*) FROM Sys_MapAttr WHERE FK_MapData='ND" + nd.NodeID + "' AND KeyOfEn IN ('EvalEmpNo','EvalEmpName','EvalEmpCent')";
                        if (DBAccess.RunSQLReturnValInt(sql) != 3)
                            msg += "@ Information : You set up a node ("+nd.NodeID+","+nd.Name+") Node for quality assessment , But you do not set the necessary fields in the node node evaluation form .";
                    }
                }
                msg += "@ Check the complete quality assessment point .";
                #endregion


                msg += "@ Reports inspection process is completed ...";
        
                //  Inspection process .
                Node.CheckFlow(this,dirUpdate);

                // Generate  V001  View .
                CheckRptView(nds);
                return msg;
            }
            catch (Exception ex)
            {
                throw new Exception("@ Inspection process error :" + ex.Message + " @" + ex.StackTrace);
            }
        }
        #endregion  Other methods .

        #region  Generating a data template .
        readonly static string PathFlowDesc;
        static Flow()
        {
            PathFlowDesc = SystemConfig.PathOfDataUser + "FlowDesc\\";
        }
        /// <summary>
        ///  Generation process template 
        /// </summary>
        /// <returns></returns>
        public string GenerFlowXmlTemplete()
        {
            string name = this.Name;
            name = BP.Tools.StringExpressionCalculate.ReplaceBadCharOfFileName(name);

            string path = this.No + "." + name;
            path = PathFlowDesc + path + "\\";
            this.DoExpFlowXmlTemplete(path);

            name = path + name + "." + this.Ver.Replace(":", "_") + ".xml";
            return name;
        }
        /// <summary>
        ///  Generation process template 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DataSet DoExpFlowXmlTemplete(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            DataSet ds = GetFlow(path);
            if (ds != null)
            {
                string name = this.Name;
                name = BP.Tools.StringExpressionCalculate.ReplaceBadCharOfFileName(name);
                name = name + "." + this.Ver.Replace(":", "_") + ".xml";
                string filePath = path + name;
                ds.WriteXml(filePath);
            }
            return ds;
        }
        
        //xml If the file is in operation 
        static bool isXmlLocked;
        /// <summary>
        ///  Back up the current process to the user xml File 
        ///  When you save each user calls 
        ///  Catch the exception written to the log , Backup failure does not affect the normal save 
        /// </summary>
        public void  WriteToXml()
        {
            try
            {
                string name = this.No + "." + this.Name;
                name = BP.Tools.StringExpressionCalculate.ReplaceBadCharOfFileName(name);
                string path = PathFlowDesc + name + "\\";
                DataSet ds = GetFlow(path);
                if (ds == null)
                    return;

                string directory = this.No + "." + this.Name;
                directory = BP.Tools.StringExpressionCalculate.ReplaceBadCharOfFileName(directory);
                path = PathFlowDesc + directory + "\\";
                string xmlName = path + "Flow" + ".xml";

                if (!isXmlLocked)
                {
                    if (!string.IsNullOrEmpty(path))
                    {
                        if (!System.IO.Directory.Exists(path))
                            System.IO.Directory.CreateDirectory(path);
                        else if (System.IO.File.Exists(xmlName))
                        {
                            DateTime time = File.GetLastWriteTime(xmlName);
                            string xmlNameOld = path + "Flow" + time.ToString("@yyyyMMddHHmmss") + ".xml";

                            isXmlLocked = true;
                            if (File.Exists(xmlNameOld))
                                File.Delete(xmlNameOld);
                            File.Move(xmlName, xmlNameOld);
                        }
                    }

                    if (!string.IsNullOrEmpty(xmlName))
                    {
                        ds.WriteXml(xmlName);
                        isXmlLocked = false;
                    }
                }
            }
            catch(Exception e)
            {
                isXmlLocked = false;
                BP.DA.Log.DefaultLogWriteLineError(" Process template file backup error :"+e.Message);
            }
        }
       

        public DataSet GetFlow(string path)
        {
            //  All the data are stored here .
            DataSet ds = new DataSet();

            //  Process Information .
            string sql = "SELECT * FROM WF_Flow WHERE No='" + this.No + "'";
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "WF_Flow";
            ds.Tables.Add(dt);

            //  Node information 
            sql = "SELECT * FROM WF_Node WHERE FK_Flow='" + this.No + "'";
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "WF_Node";
            ds.Tables.Add(dt);

            //  Document information 
            BillTemplates tmps = new BillTemplates(this.No);
            string pks = "";
            foreach (BillTemplate tmp in tmps)
            {
                try
                {
                    if (path != null)
                        System.IO.File.Copy(SystemConfig.PathOfDataUser + @"\CyclostyleFile\" + tmp.No + ".rtf", path + "\\" + tmp.No + ".rtf", true);
                }
                catch
                {
                    pks += "@" + tmp.PKVal;
                    tmp.Delete();
                }
            }
            tmps.Remove(pks);
            ds.Tables.Add(tmps.ToDataTableField("WF_BillTemplate"));

            string sqlin = "SELECT NodeID FROM WF_Node WHERE fk_flow='" + this.No + "'";

            //  Process tree form 
            sql = "SELECT * FROM WF_FlowFormTree WHERE FK_Flow='" + this.No + "'";
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "WF_FlowFormTree";
            ds.Tables.Add(dt);

            ////  Process Form 
            //sql = "SELECT * FROM WF_FlowForm WHERE FK_Flow='" + this.No + "'";
            //dt = DBAccess.RunSQLReturnTable(sql);
            //dt.TableName = "WF_FlowForm";
            //ds.Tables.Add(dt);

            ////  Node permission form 
            //sql = "SELECT * FROM WF_NodeForm WHERE FK_Node IN (" + sqlin + ")";
            //dt = DBAccess.RunSQLReturnTable(sql);
            //dt.TableName = "WF_NodeForm";
            //ds.Tables.Add(dt);

            //  Conditions Information 
            sql = "SELECT * FROM WF_Cond WHERE FK_Flow='" + this.No + "'";
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "WF_Cond";
            ds.Tables.Add(dt);

            //  Steering Rules .
            sql = "SELECT * FROM WF_TurnTo WHERE FK_Flow='" + this.No + "'";
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "WF_TurnTo";
            ds.Tables.Add(dt);

            //  Binding node with the form .
            sql = "SELECT * FROM WF_FrmNode WHERE FK_Flow='" + this.No + "'";
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "WF_FrmNode";
            ds.Tables.Add(dt);

            //  Forms program .
            sql = "SELECT * FROM Sys_FrmSln WHERE FK_Node IN (" + sqlin + ")";
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "Sys_FrmSln";
            ds.Tables.Add(dt);

            //  Direction 
            sql = "SELECT * from WF_Direction WHERE Node IN (" + sqlin + ") OR ToNode In (" + sqlin + ")";
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "WF_Direction";
            ds.Tables.Add(dt);

            ////  Application Settings  FAppSet
            //sql = "SELECT * FROM WF_FAppSet WHERE FK_Flow='" + this.No + "'";
            //dt = DBAccess.RunSQLReturnTable(sql);
            //dt.TableName = "WF_FAppSet";
            //ds.Tables.Add(dt);

            //  Process tag .
            LabNotes labs = new LabNotes(this.No);
            ds.Tables.Add(labs.ToDataTableField("WF_LabNote"));

            //  The message listener .
            Listens lts = new Listens(this.No);
            ds.Tables.Add(lts.ToDataTableField("WF_Listen"));

            //  Returnable node .
            sql = "SELECT * FROM WF_NodeReturn WHERE FK_Node IN (" + sqlin + ")";
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "WF_NodeReturn";
            ds.Tables.Add(dt);

            //  Toolbar .
            sql = "SELECT * FROM WF_NodeToolbar WHERE FK_Node IN (" + sqlin + ")";
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "WF_NodeToolbar";
            ds.Tables.Add(dt);

            //  Node with the department .
            sql = "SELECT * FROM WF_NodeDept WHERE FK_Node IN (" + sqlin + ")";
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "WF_NodeDept";
            ds.Tables.Add(dt);


            //  Node and job competence .
            sql = "SELECT * FROM WF_NodeStation WHERE FK_Node IN (" + sqlin + ")";
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "WF_NodeStation";
            ds.Tables.Add(dt);

            //  Node and staff .
            sql = "SELECT * FROM WF_NodeEmp WHERE FK_Node IN (" + sqlin + ")";
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "WF_NodeEmp";
            ds.Tables.Add(dt);

            //  CC staff .
            sql = "SELECT * FROM WF_CCEmp WHERE FK_Node IN (" + sqlin + ")";
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "WF_CCEmp";
            ds.Tables.Add(dt);

            //  Cc department .
            sql = "SELECT * FROM WF_CCDept WHERE FK_Node IN (" + sqlin + ")";
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "WF_CCDept";
            ds.Tables.Add(dt);

            //  Cc department .
            sql = "SELECT * FROM WF_CCStation WHERE FK_Node IN (" + sqlin + ")";
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "WF_CCStation";
            ds.Tables.Add(dt);

            ////  Reporting Process .
            //WFRpts rpts = new WFRpts(this.No);
            //// rpts.SaveToXml(path + "WFRpts.xml");
            //ds.Tables.Add(rpts.ToDataTableField("WF_Rpt"));

            ////  Process Report Properties 
            //RptAttrs rptAttrs = new RptAttrs();
            //rptAttrs.RetrieveAll();
            //ds.Tables.Add(rptAttrs.ToDataTableField("RptAttrs"));

            ////  Process Reports Access .
            //RptStations rptStations = new RptStations(this.No);
            //rptStations.RetrieveAll();
            ////  rptStations.SaveToXml(path + "RptStations.xml");
            //ds.Tables.Add(rptStations.ToDataTableField("RptStations"));

            ////  Flow statements personnel access .
            //RptEmps rptEmps = new RptEmps(this.No);
            //rptEmps.RetrieveAll();

            // rptEmps.SaveToXml(path + "RptEmps.xml");
            // ds.Tables.Add(rptEmps.ToDataTableField("RptEmps"));

            int flowID = int.Parse(this.No);
            sql = "SELECT * FROM Sys_MapData WHERE " + Glo.MapDataLikeKey(this.No, "No");
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "Sys_MapData";
            ds.Tables.Add(dt);


            // Sys_MapAttr.
            sql = "SELECT * FROM Sys_MapAttr WHERE  FK_MapData LIKE 'ND" + flowID + "%' ORDER BY FK_MapData,Idx";
            //sql = "SELECT * FROM Sys_MapAttr WHERE " + Glo.MapDataLikeKey(this.No, "FK_MapData") + "  ORDER BY FK_MapData,Idx";
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "Sys_MapAttr";
            ds.Tables.Add(dt);

            // Sys_EnumMain
            //sql = "SELECT * FROM Sys_EnumMain WHERE No IN (SELECT KeyOfEn from Sys_MapAttr WHERE " + Glo.MapDataLikeKey(this.No, "FK_MapData") +")";
            sql = "SELECT * FROM Sys_EnumMain WHERE No IN (SELECT KeyOfEn from Sys_MapAttr WHERE FK_MapData LIKE 'ND" + flowID + "%')";
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "Sys_EnumMain";
            ds.Tables.Add(dt);

            // Sys_Enum
            sql = "SELECT * FROM Sys_Enum WHERE EnumKey IN ( SELECT No FROM Sys_EnumMain WHERE No IN (SELECT KeyOfEn from Sys_MapAttr WHERE FK_MapData LIKE 'ND" + flowID + "%' ) )";
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "Sys_Enum";
            ds.Tables.Add(dt);

            // Sys_MapDtl
            sql = "SELECT * FROM Sys_MapDtl WHERE " + Glo.MapDataLikeKey(this.No, "FK_MapData");
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "Sys_MapDtl";
            ds.Tables.Add(dt);

            // Sys_MapExt
            //sql = "SELECT * FROM Sys_MapExt WHERE " + Glo.MapDataLikeKey(this.No, "FK_MapData");
            sql = "SELECT * FROM Sys_MapExt WHERE FK_MapData LIKE 'ND" + flowID + "%'";  // +Glo.MapDataLikeKey(this.No, "FK_MapData");

            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "Sys_MapExt";
            ds.Tables.Add(dt);

            // Sys_GroupField
            sql = "SELECT * FROM Sys_GroupField WHERE " + Glo.MapDataLikeKey(this.No, "EnName");
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "Sys_GroupField";
            ds.Tables.Add(dt);

            // Sys_MapFrame
            sql = "SELECT * FROM Sys_MapFrame WHERE" + Glo.MapDataLikeKey(this.No, "FK_MapData");
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "Sys_MapFrame";
            ds.Tables.Add(dt);

            // Sys_MapM2M
            sql = "SELECT * FROM Sys_MapM2M WHERE " + Glo.MapDataLikeKey(this.No, "FK_MapData");
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "Sys_MapM2M";
            ds.Tables.Add(dt);

            // Sys_FrmLine.
            sql = "SELECT * FROM Sys_FrmLine WHERE " + Glo.MapDataLikeKey(this.No, "FK_MapData");
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "Sys_FrmLine";
            ds.Tables.Add(dt);

            // Sys_FrmLab.
            sql = "SELECT * FROM Sys_FrmLab WHERE " + Glo.MapDataLikeKey(this.No, "FK_MapData");
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "Sys_FrmLab";
            ds.Tables.Add(dt);

            // Sys_FrmEle.
            sql = "SELECT * FROM Sys_FrmEle WHERE " + Glo.MapDataLikeKey(this.No, "FK_MapData");
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "Sys_FrmEle";
            ds.Tables.Add(dt);

            // Sys_FrmLink.
            sql = "SELECT * FROM Sys_FrmLink WHERE " + Glo.MapDataLikeKey(this.No, "FK_MapData");
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "Sys_FrmLink";
            ds.Tables.Add(dt);

            // Sys_FrmRB.
            sql = "SELECT * FROM Sys_FrmRB WHERE " + Glo.MapDataLikeKey(this.No, "FK_MapData");
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "Sys_FrmRB";
            ds.Tables.Add(dt);

            // Sys_FrmImgAth.
            sql = "SELECT * FROM Sys_FrmImgAth WHERE " + Glo.MapDataLikeKey(this.No, "FK_MapData");
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "Sys_FrmImgAth";
            ds.Tables.Add(dt);

            // Sys_FrmImg.
            sql = "SELECT * FROM Sys_FrmImg WHERE " + Glo.MapDataLikeKey(this.No, "FK_MapData");
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "Sys_FrmImg";
            ds.Tables.Add(dt);

            // Sys_FrmAttachment.
            sql = "SELECT * FROM Sys_FrmAttachment WHERE FK_Node=0 AND " + Glo.MapDataLikeKey(this.No, "FK_MapData");
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "Sys_FrmAttachment";
            ds.Tables.Add(dt);

            // Sys_FrmEvent.
            sql = "SELECT * FROM Sys_FrmEvent WHERE " + Glo.MapDataLikeKey(this.No, "FK_MapData");
            dt = DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "Sys_FrmEvent";
            ds.Tables.Add(dt);

            return ds;
        }

        #endregion  Generating a data template .

        #region  Other public methods 1
        /// <summary>
        ///  Reset Rpt表
        /// </summary>
        public void CheckRptOfReset()
        {
            string fk_mapData = "ND" + int.Parse(this.No) + "Rpt";
            string sql = "DELETE FROM Sys_MapAttr WHERE FK_MapData='" + fk_mapData + "'";
            DBAccess.RunSQL(sql);

            sql = "DELETE FROM Sys_MapData WHERE No='" + fk_mapData + "'";
            DBAccess.RunSQL(sql);
            this.DoCheck_CheckRpt(this.HisNodes);
        }
        /// <summary>
        ///  Reload 
        /// </summary>
        /// <returns></returns>
        public string DoReloadRptData()
        {
            this.DoCheck_CheckRpt(this.HisNodes);

            //  Check whether the report data is lost .

            if (this.HisDataStoreModel != DataStoreModel.ByCCFlow)
                return "@ Process " + this.No + this.Name + " The data is stored non-track mode can not be regenerated .";

            DBAccess.RunSQL("DELETE FROM " + this.PTable);

            string sql = "SELECT OID FROM ND" + int.Parse(this.No) + "01 WHERE  OID NOT IN (SELECT OID FROM  "+this.PTable+" ) ";
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            this.CheckRptData(this.HisNodes, dt);

            return "@ Total :" + dt.Rows.Count + "条(" + this.Name + ") Data is loaded successfully .";
        }
        /// <summary>
        ///  Inspection and repair report data 
        /// </summary>
        /// <param name="nds"></param>
        /// <param name="dt"></param>
        private string CheckRptData(Nodes nds, DataTable dt)
        {
            GERpt rpt = new GERpt("ND" + int.Parse(this.No) + "Rpt");
            string err = "";
            foreach (DataRow dr in dt.Rows)
            {
                rpt.ResetDefaultVal();
                int oid = int.Parse(dr[0].ToString());
                rpt.SetValByKey("OID", oid);
                Work startWork = null;
                Work endWK = null;
                string flowEmps = "";
                foreach (Node nd in nds)
                {
                    try
                    {
                        Work wk = nd.HisWork;
                        wk.OID = oid;
                        if (wk.RetrieveFromDBSources() == 0)
                            continue;

                        rpt.Copy(wk);
                        if (nd.NodeID == int.Parse(this.No + "01"))
                            startWork = wk;

                        try
                        {
                            if (flowEmps.Contains("@" + wk.Rec + ","))
                                continue;

                            flowEmps += "@" + wk.Rec + "," + wk.RecOfEmp.Name;
                        }
                        catch
                        {
                        }
                        endWK = wk;
                    }
                    catch (Exception ex)
                    {
                        err += ex.Message;
                    }
                }

                if (startWork == null || endWK == null)
                    continue;

                rpt.SetValByKey("OID", oid);
                rpt.FK_NY = startWork.GetValStrByKey("RDT").Substring(0, 7);
                rpt.FK_Dept = startWork.GetValStrByKey("FK_Dept");
                if (string.IsNullOrEmpty(rpt.FK_Dept))
                {
                    string fk_dept = DBAccess.RunSQLReturnString("SELECT FK_Dept FROM Port_Emp WHERE No='" + startWork.Rec + "'");
                    rpt.FK_Dept = fk_dept;

                    startWork.SetValByKey("FK_Dept", fk_dept);
                    startWork.Update();
                }
                rpt.Title = startWork.GetValStrByKey("Title");
                string wfState = DBAccess.RunSQLReturnStringIsNull("SELECT WFState FROM WF_GenerWorkFlow WHERE WorkID=" + oid, "1");
                rpt.WFState = (WFState) int.Parse(wfState);
                rpt.FlowStarter = startWork.Rec;
                rpt.FlowStartRDT = startWork.RDT;
                rpt.FID = startWork.GetValIntByKey("FID");
                rpt.FlowEmps = flowEmps;
                rpt.FlowEnder = endWK.Rec;
                rpt.FlowEnderRDT = endWK.RDT;
                rpt.FlowEndNode = endWK.NodeID;
                rpt.MyNum = 1;

                // Repair title field .
                WorkNode wn = new WorkNode(startWork, this.HisStartNode);
                rpt.Title = WorkNode.GenerTitle(this, startWork);
                try
                {
                    TimeSpan ts = endWK.RDT_DateTime - startWork.RDT_DateTime;
                    rpt.FlowDaySpan = ts.Days;
                }
                catch
                {
                }
                rpt.InsertAsOID(rpt.OID);
            } //  End loop .
            return err;
        }
        /// <summary>
        ///  Generate detailed reports information 
        /// </summary>
        /// <param name="nds"></param>
        private void CheckRptDtl(Nodes nds)
        {
            MapDtls dtlsDtl = new MapDtls();
            dtlsDtl.Retrieve(MapDtlAttr.FK_MapData, "ND" + int.Parse(this.No) + "Rpt");
            foreach (MapDtl dtl in dtlsDtl)
            {
                dtl.Delete();
            }

            //  dtlsDtl.Delete(MapDtlAttr.FK_MapData, "ND" + int.Parse(this.No) + "Rpt");
            foreach (Node nd in nds)
            {
                if (nd.IsEndNode == false)
                    continue;

                //  Taken out from the table .
                MapDtls dtls = new MapDtls("ND" + nd.NodeID);
                if (dtls.Count == 0)
                    continue;

                string rpt = "ND" + int.Parse(this.No) + "Rpt";
                int i = 0;
                foreach (MapDtl dtl in dtls)
                {
                    i++;
                    string rptDtlNo = "ND" + int.Parse(this.No) + "RptDtl" + i.ToString();
                    MapDtl rtpDtl = new MapDtl();
                    rtpDtl.No = rptDtlNo;
                    if (rtpDtl.RetrieveFromDBSources() == 0)
                    {
                        rtpDtl.Copy(dtl);
                        rtpDtl.No = rptDtlNo;
                        rtpDtl.FK_MapData = rpt;
                        rtpDtl.PTable = rptDtlNo;
                        rtpDtl.GroupID = -1;
                        rtpDtl.Insert();
                    }

                    

                    MapAttrs attrsRptDtl = new MapAttrs(rptDtlNo);
                    MapAttrs attrs = new MapAttrs(dtl.No);
                    foreach (MapAttr attr in attrs)
                    {
                        if (attrsRptDtl.Contains(MapAttrAttr.KeyOfEn, attr.KeyOfEn) == true)
                            continue;

                        MapAttr attrN = new MapAttr();
                        attrN.Copy(attr);
                        attrN.FK_MapData = rptDtlNo;
                        switch (attr.KeyOfEn)
                        {
                            case "FK_NY":
                                attrN.UIVisible = true;
                                attrN.IDX = 100;
                                attrN.UIWidth = 60;
                                break;
                            case "RDT":
                                attrN.UIVisible = true;
                                attrN.IDX = 100;
                                attrN.UIWidth = 60;
                                break;
                            case "Rec":
                                attrN.UIVisible = true;
                                attrN.IDX = 100;
                                attrN.UIWidth = 60;
                                break;
                            default:
                                break;
                        }

                        attrN.Save();
                    }

                    GEDtl geDtl = new GEDtl(rptDtlNo);
                    geDtl.CheckPhysicsTable();
                }
            }
        }
        /// <summary>
        ///  Generates all node view 
        /// </summary>
        /// <param name="nds"></param>
        private void CheckRptView(Nodes nds)
        {
            if (this.HisDataStoreModel == DataStoreModel.SpecTable)
                return;

            string viewName = "V" + this.No;
            string sql = "CREATE VIEW " + viewName + " ";
            sql += "/* CCFlow Auto Create :" + this.Name + " Date:" + DateTime.Now.ToString("yyyy-MM-dd") + " */ ";
            sql += "\r\n (MyPK,FK_Node,OID,FID,RDT,FK_NY,CDT,Rec,Emps,FK_Dept,MyNum) AS ";
            bool is1 = false;
            foreach (Node nd in nds)
            {
                //  nd.HisWork.CheckPhysicsTable();
                if (is1 == false)
                    is1 = true;
                else
                    sql += "\r\n UNION ";

                switch (SystemConfig.AppCenterDBType)
                {
                    case DBType.Oracle:
                    case DBType.Informix:
                        sql += "\r\n SELECT '" + nd.NodeID + "' || '_'|| OID||'_'|| FID  AS MyPK, '" + nd.NodeID + "' AS FK_Node,OID,FID,RDT,SUBSTR(RDT,1,7) AS FK_NY,CDT,Rec,Emps,FK_Dept, 1 AS MyNum FROM ND" + nd.NodeID + " ";
                        break;
                    case DBType.MySQL:
                        sql += "\r\n SELECT '" + nd.NodeID + "'+'_'+CHAR(OID)  +'_'+CHAR(FID)  AS MyPK, '" + nd.NodeID + "' AS FK_Node,OID,FID,RDT," + BP.Sys.SystemConfig.AppCenterDBSubstringStr + "(RDT,1,7) AS FK_NY,CDT,Rec,Emps,FK_Dept, 1 AS MyNum FROM ND" + nd.NodeID + " ";
                        break;
                    default:
                        sql += "\r\n SELECT '" + nd.NodeID + "'+'_'+CAST(OID AS varchar(10)) +'_'+CAST(FID AS VARCHAR(10)) AS MyPK, '" + nd.NodeID + "' AS FK_Node,OID,FID,RDT," + BP.Sys.SystemConfig.AppCenterDBSubstringStr + "(RDT,1,7) AS FK_NY,CDT,Rec,Emps,FK_Dept, 1 AS MyNum FROM ND" + nd.NodeID + " ";
                        break;
                }
            }
            if (SystemConfig.AppCenterDBType != DBType.Informix)
                sql += "\r\n GO ";

            try
            {
                if (DBAccess.IsExitsObject(viewName) == true)
                    DBAccess.RunSQL("DROP VIEW " + viewName);
            }
            catch
            {
            }

            try
            {
                DBAccess.RunSQL(sql);
            }
            catch(Exception ex)
            {
                BP.DA.Log.DefaultLogWriteLineError(ex.Message);
            }
        }
        /// <summary>
        ///  Check the data report .
        /// </summary>
        /// <param name="nds"></param>
        private void DoCheck_CheckRpt(Nodes nds)
        {
            string fk_mapData = "ND" + int.Parse(this.No) + "Rpt";
            string flowId = int.Parse(this.No).ToString();

            //  Deal with track表.
            Track.CreateOrRepairTrackTable(flowId);

            #region  Insert Field .
            string sql = "";
            switch (SystemConfig.AppCenterDBType)
            {
                case DBType.Oracle:
                case DBType.MSSQL:
                    sql = "SELECT distinct  KeyOfEn FROM Sys_MapAttr WHERE FK_MapData IN ( SELECT 'ND' " + SystemConfig.AppCenterDBAddStringStr + " cast(NodeID as varchar(20)) FROM WF_Node WHERE FK_Flow='" + this.No + "')";
                    break;
                case DBType.Informix:
                    sql = "SELECT distinct  KeyOfEn FROM Sys_MapAttr WHERE FK_MapData IN ( SELECT 'ND' " + SystemConfig.AppCenterDBAddStringStr + " cast(NodeID as varchar(20)) FROM WF_Node WHERE FK_Flow='" + this.No + "')";
                    break;
                case DBType.MySQL:
                    sql = "SELECT DISTINCT KeyOfEn FROM Sys_MapAttr  WHERE FK_MapData IN (SELECT X.No FROM ( SELECT CONCAT('ND',NodeID) AS No FROM WF_Node WHERE FK_Flow='"+this.No+"') AS X )";
                    break;
                default:
                    sql = "SELECT distinct  KeyOfEn FROM Sys_MapAttr WHERE FK_MapData IN ( SELECT 'ND' " + SystemConfig.AppCenterDBAddStringStr + " cast(NodeID as varchar(20)) FROM WF_Node WHERE FK_Flow='" + this.No + "')";
                    break;
            }

            if (SystemConfig.AppCenterDBType == DBType.MySQL)
            {
                sql = "SELECT A.* FROM (" + sql + ") AS A ";
                string sql3 = "DELETE FROM Sys_MapAttr WHERE KeyOfEn NOT IN (" + sql + ") AND FK_MapData='" + fk_mapData + "' ";
                DBAccess.RunSQL(sql3); //  Delete field does not exist .
            }
            else
            {
                string sql2 = "DELETE FROM Sys_MapAttr WHERE KeyOfEn NOT IN (" + sql + ") AND FK_MapData='"+fk_mapData+"' ";
                DBAccess.RunSQL(sql2); //  Delete field does not exist .
            }


            //  No fields on the supplementary .
            switch (SystemConfig.AppCenterDBType)
            {
                case DBType.Oracle:
                    sql = "SELECT MyPK, KeyOfEn FROM Sys_MapAttr WHERE FK_MapData IN ( SELECT 'ND' " + SystemConfig.AppCenterDBAddStringStr + " cast(NodeID as varchar(20)) FROM WF_Node WHERE FK_Flow='" + this.No + "')";
                    break;
                case DBType.MySQL:
                    sql = "SELECT MyPK, KeyOfEn FROM Sys_MapAttr WHERE FK_MapData IN (SELECT X.No FROM ( SELECT CONCAT('ND',NodeID) AS No FROM WF_Node WHERE FK_Flow='"+this.No+"') AS X )";
                    break;
                default:
                    sql = "SELECT MyPK, KeyOfEn FROM Sys_MapAttr WHERE FK_MapData IN ( SELECT 'ND' " + SystemConfig.AppCenterDBAddStringStr + " cast(NodeID as varchar(20)) FROM WF_Node WHERE FK_Flow='" + this.No + "')";
                    break;
            }

            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            sql = "SELECT KeyOfEn FROM Sys_MapAttr WHERE FK_MapData='ND" + flowId + "Rpt'";
            DataTable dtExits = DBAccess.RunSQLReturnTable(sql);
            string pks = "@";
            foreach (DataRow dr in dtExits.Rows)
                pks += dr[0] + "@";

            foreach (DataRow dr in dt.Rows)
            {
                string mypk = dr["MyPK"].ToString();
                if (pks.Contains("@" + dr["KeyOfEn"].ToString() + "@"))
                    continue;

                pks += dr["KeyOfEn"].ToString() + "@";

                BP.Sys.MapAttr ma = new BP.Sys.MapAttr(mypk);
                ma.MyPK = "ND" + flowId + "Rpt_" + ma.KeyOfEn;
                ma.FK_MapData = "ND" + flowId + "Rpt";
                ma.UIIsEnable = false;

                if (ma.DefValReal.Contains("@"))
                {
                    /* If there is a variable parameter .*/
                    ma.DefVal = "";
                }

                try
                {
                    ma.Insert();
                }
                catch
                {
                }
            }

            MapAttrs attrs = new MapAttrs(fk_mapData);

            //  Create mapData.
            BP.Sys.MapData md = new BP.Sys.MapData();
            md.No = "ND" + flowId + "Rpt";
            if (md.RetrieveFromDBSources() == 0)
            {
                md.Name = this.Name;
                md.PTable = this.PTable;
                md.Insert();
            }
            else
            {
                md.Name = this.Name;
                md.PTable = this.PTable;
                md.Update();
            }
            #endregion  Insert Field .

            #region  The replenishment process field .
            int groupID = 0;
            foreach (MapAttr attr in attrs)
            {
                switch (attr.KeyOfEn)
                {
                    case StartWorkAttr.FK_Dept:
                        attr.UIBindKey = "BP.Port.Depts";
                        attr.UIContralType = UIContralType.DDL;
                        attr.LGType = FieldTypeS.FK;
                        attr.UIVisible = true;
                        attr.GroupID = groupID;// gfs[0].GetValIntByKey("OID");
                        attr.UIIsEnable = false;
                        attr.DefVal = "";
                        attr.MaxLen = 100;
                        attr.Update();
                        break;
                    case "FK_NY":
                        attr.UIBindKey = "BP.Pub.NYs";
                        attr.UIContralType = UIContralType.DDL;
                        attr.LGType = FieldTypeS.FK;
                        attr.UIVisible = true;
                        attr.UIIsEnable = false;
                        attr.GroupID = groupID;
                        attr.Update();
                        break;
                    case "FK_Emp":
                        break;
                    default:
                        break;
                }
            }
          
            if (attrs.Contains(md.No + "_" + GERptAttr.WFState) == false)
            {
                /*  Process Status  */
                MapAttr attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = EditType.UnDel;
                attr.KeyOfEn = GERptAttr.WFState;
                attr.Name = " Process Status "; //  
                attr.MyDataType = DataType.AppInt;
                attr.UIBindKey = GERptAttr.WFState;
                attr.UIContralType = UIContralType.DDL;
                attr.LGType = FieldTypeS.Enum;
                attr.UIVisible = true;
                attr.UIIsEnable = false;
                attr.MinLen = 0;
                attr.MaxLen = 1000;
                attr.IDX = -1;
                attr.Insert();
            }

            if (attrs.Contains(md.No + "_" + GERptAttr.WFSta) == false)
            {
                /*  Process Status Ext */
                MapAttr attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = EditType.UnDel;
                attr.KeyOfEn = GERptAttr.WFSta;
                attr.Name = " Status "; //  
                attr.MyDataType = DataType.AppInt;
                attr.UIBindKey = GERptAttr.WFSta;
                attr.UIContralType = UIContralType.DDL;
                attr.LGType = FieldTypeS.Enum;
                attr.UIVisible = true;
                attr.UIIsEnable = false;
                attr.MinLen = 0;
                attr.MaxLen = 1000;
                attr.IDX = -1;
                attr.Insert();
            }

            if (attrs.Contains(md.No + "_" + GERptAttr.FlowEmps) == false)
            {
                /*  Participants  */
                MapAttr attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = EditType.UnDel;
                attr.KeyOfEn = GERptAttr.FlowEmps; // "FlowEmps";
                attr.Name = " Participants "; //  
                attr.MyDataType = DataType.AppString;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = true;
                attr.UIIsEnable = false;
                attr.UIIsLine = true;
                attr.MinLen = 0;
                attr.MaxLen = 1000;
                attr.IDX = -100;
                attr.Insert();
            }

            if (attrs.Contains(md.No + "_" + GERptAttr.FlowStarter) == false)
            {
                /*  Sponsor  */
                MapAttr attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = EditType.UnDel;
                attr.KeyOfEn = GERptAttr.FlowStarter;
                attr.Name = " Sponsor "; //  
                attr.MyDataType = DataType.AppString;

                attr.UIBindKey = "BP.Port.Emps";
                attr.UIContralType = UIContralType.DDL;
                attr.LGType = FieldTypeS.FK;

                attr.UIVisible = true;
                attr.UIIsEnable = false;
                attr.MinLen = 0;
                attr.MaxLen = 20;
                attr.IDX = -1;
                attr.Insert();
            }

            if (attrs.Contains(md.No + "_" + GERptAttr.FlowStartRDT) == false)
            {
                /* MyNum */
                MapAttr attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = EditType.UnDel;
                attr.KeyOfEn = GERptAttr.FlowStartRDT; // "FlowStartRDT";
                attr.Name = " Start Time ";
                attr.MyDataType = DataType.AppDateTime;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = true;
                attr.UIIsEnable = false;
                attr.UIIsLine = false;
                attr.IDX = -101;
                attr.Insert();
            }

            if (attrs.Contains(md.No + "_" + GERptAttr.FlowEnder) == false)
            {
                /*  Sponsor  */
                MapAttr attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = EditType.UnDel;
                attr.KeyOfEn = GERptAttr.FlowEnder;
                attr.Name = " End people "; //  
                attr.MyDataType = DataType.AppString;
                attr.UIBindKey = "BP.Port.Emps";
                attr.UIContralType = UIContralType.DDL;
                attr.LGType = FieldTypeS.FK;
                attr.UIVisible = true;
                attr.UIIsEnable = false;
                attr.MinLen = 0;
                attr.MaxLen = 20;
                attr.IDX = -1;
                attr.Insert();
            }

            if (attrs.Contains(md.No + "_" + GERptAttr.FlowEnderRDT) == false)
            {
                /* MyNum */
                MapAttr attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = EditType.UnDel;
                attr.KeyOfEn = GERptAttr.FlowEnderRDT; // "FlowStartRDT";
                attr.Name = " End Time ";
                attr.MyDataType = DataType.AppDateTime;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = true;
                attr.UIIsEnable = false;
                attr.UIIsLine = false;
                attr.IDX = -101;
                attr.Insert();
            }

            if (attrs.Contains(md.No + "_" + GERptAttr.FlowEndNode) == false)
            {
                /*  End node  */
                MapAttr attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = EditType.UnDel;
                attr.KeyOfEn = GERptAttr.FlowEndNode;
                attr.Name = " End node ";
                attr.MyDataType = DataType.AppInt;
                attr.DefVal = "0";
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = true;
                attr.UIIsEnable = false;
                attr.UIIsLine = false;
                attr.HisEditType = EditType.UnDel;
                attr.IDX = -101;
                attr.Insert();
            }

            if (attrs.Contains(md.No + "_" + GERptAttr.FlowDaySpan) == false)
            {
                /* FlowDaySpan */
                MapAttr attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = EditType.UnDel;
                attr.KeyOfEn = GERptAttr.FlowDaySpan; // "FlowStartRDT";
                attr.Name = " Span (days)";
                attr.MyDataType = DataType.AppMoney;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = true;
                attr.UIIsEnable = true;
                attr.UIIsLine = false;
                attr.IDX = -101;
                attr.Insert();
            }

            if (attrs.Contains(md.No + "_" + GERptAttr.PFlowNo) == false)
            {
                /*  Parent process   Process ID  */
                MapAttr attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = EditType.UnDel;
                attr.KeyOfEn = GERptAttr.PFlowNo;
                attr.Name = " Parent process ID process "; //   Parent process ID process 
                attr.MyDataType = DataType.AppString;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = true;
                attr.UIIsEnable = false;
                attr.UIIsLine = true;
                attr.MinLen = 0;
                attr.MaxLen = 3;
                attr.IDX = -100;
                attr.Insert();
            }

            if (attrs.Contains(md.No + "_" + GERptAttr.PNodeID) == false)
            {
                /*  Parent process WorkID */
                MapAttr attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = EditType.UnDel;
                attr.KeyOfEn = GERptAttr.PNodeID;
                attr.Name = " Parent process start node ";
                attr.MyDataType = DataType.AppInt;
                attr.DefVal = "0";
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = true;
                attr.UIIsEnable = false;
                attr.UIIsLine = false;
                attr.HisEditType = EditType.UnDel;
                attr.IDX = -101;
                attr.Insert();
            }

            if (attrs.Contains(md.No + "_" + GERptAttr.PWorkID) == false)
            {
                /*  Parent process WorkID */
                MapAttr attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = EditType.UnDel;
                attr.KeyOfEn = GERptAttr.PWorkID;
                attr.Name = " Parent process WorkID";
                attr.MyDataType = DataType.AppInt;
                attr.DefVal = "0";
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = true;
                attr.UIIsEnable = false;
                attr.UIIsLine = false;
                attr.HisEditType = EditType.UnDel;
                attr.IDX = -101;
                attr.Insert();
            }

            if (attrs.Contains(md.No + "_" + GERptAttr.PEmp) == false)
            {
                /*  Screwdriver adjustment process staff  */
                MapAttr attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = EditType.UnDel;
                attr.KeyOfEn = GERptAttr.PEmp;
                attr.Name = " Screwdriver adjustment process staff ";  
                attr.MyDataType = DataType.AppString;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = true;
                attr.UIIsEnable = false;
                attr.UIIsLine = true;
                attr.MinLen = 0;
                attr.MaxLen = 32;
                attr.IDX = -100;
                attr.Insert();
            }
          

            if (attrs.Contains(md.No + "_" + GERptAttr.CWorkID) == false)
            {
                /*  Continuation of the process WorkID */
                MapAttr attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = EditType.UnDel;
                attr.KeyOfEn = GERptAttr.CWorkID;
                attr.Name = " Continuation of the process WorkID";
                attr.MyDataType = DataType.AppInt;
                attr.DefVal = "0";
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = true;
                attr.UIIsEnable = false;
                attr.UIIsLine = false;
                attr.HisEditType = EditType.UnDel;
                attr.IDX = -101;
                attr.Insert();
            }

            if (attrs.Contains(md.No + "_" + GERptAttr.CFlowNo) == false)
            {
                /*  Continuation of the process ID  */
                MapAttr attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = EditType.UnDel;
                attr.KeyOfEn = GERptAttr.CFlowNo;
                attr.Name = " Continuation of the process ID "; 
                attr.MyDataType = DataType.AppString;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = true;
                attr.UIIsEnable = false;
                attr.UIIsLine = true;
                attr.MinLen = 0;
                attr.MaxLen = 3;
                attr.IDX = -100;
                attr.Insert();
            }

            if (attrs.Contains(md.No + "_" + GERptAttr.BillNo) == false)
            {
                /*  Parent process   Process ID  */
                MapAttr attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = EditType.UnDel;
                attr.KeyOfEn = GERptAttr.BillNo;
                attr.Name = " Document Number "; //   Document Number 
                attr.MyDataType = DataType.AppString;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = true;
                attr.UIIsEnable = false;
                attr.UIIsLine = false;
                attr.MinLen = 0;
                attr.MaxLen = 100;
                attr.IDX = -100;
                attr.Insert();
            }


            if (attrs.Contains(md.No + "_MyNum") == false)
            {
                /* MyNum */
                MapAttr attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = EditType.UnDel;
                attr.KeyOfEn = "MyNum";
                attr.Name = "条";
                attr.MyDataType = DataType.AppInt;
                attr.DefVal = "1";
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = true;
                attr.UIIsEnable = false;
                attr.UIIsLine = false;
                attr.HisEditType = EditType.UnDel;
                attr.IDX = -101;
                attr.Insert();
            }

            if (attrs.Contains(md.No + "_" + GERptAttr.AtPara) == false)
            {
                /*  Parent process   Process ID  */
                MapAttr attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = EditType.UnDel;
                attr.KeyOfEn = GERptAttr.AtPara;
                attr.Name = " Parameters "; //  Document Number 
                attr.MyDataType = DataType.AppString;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = false;
                attr.UIIsEnable = false;
                attr.UIIsLine = false;
                attr.MinLen = 0;
                attr.MaxLen = 4000;
                attr.IDX = -100;
                attr.Insert();
            }

            if (attrs.Contains(md.No + "_" + GERptAttr.GUID) == false)
            {
                /*  Parent process   Process ID  */
                MapAttr attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = EditType.UnDel;
                attr.KeyOfEn = GERptAttr.GUID;
                attr.Name = "GUID"; //  Document Number 
                attr.MyDataType = DataType.AppString;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = false;
                attr.UIIsEnable = false;
                attr.UIIsLine = false;
                attr.MinLen = 0;
                attr.MaxLen = 32;
                attr.IDX = -100;
                attr.Insert();
            }

            if (attrs.Contains(md.No + "_" + GERptAttr.ProjNo) == false)
            {
                /*  Item Number  */
                MapAttr attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = EditType.UnDel;
                attr.KeyOfEn = GERptAttr.ProjNo;
                attr.Name = " Item Number "; //   Item Number 
                attr.MyDataType = DataType.AppString;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = true;
                attr.UIIsEnable = false;
                attr.UIIsLine = false;
                attr.MinLen = 0;
                attr.MaxLen = 100;
                attr.IDX = -100;
                attr.Insert();
            }
            #endregion  The replenishment process field .

            #region  Set the grouping field for the process .
            try
            {
                string flowInfo = " Process Information ";
                GroupField flowGF = new GroupField();
                int num = flowGF.Retrieve(GroupFieldAttr.EnName, fk_mapData, GroupFieldAttr.Lab, " Process Information ");
                if (num == 0)
                {
                    flowGF = new GroupField();
                    flowGF.Lab = flowInfo;
                    flowGF.EnName = fk_mapData;
                    flowGF.Idx = -1;
                    flowGF.Insert();
                }
                sql = "UPDATE Sys_MapAttr SET GroupID=" + flowGF.OID + " WHERE  FK_MapData='" + fk_mapData + "'  AND KeyOfEn IN('" + GERptAttr.PFlowNo + "','" + GERptAttr.PWorkID + "','" + GERptAttr.MyNum + "','" + GERptAttr.FK_Dept + "','" + GERptAttr.FK_NY + "','" + GERptAttr.FlowDaySpan + "','" + GERptAttr.FlowEmps + "','" + GERptAttr.FlowEnder + "','" + GERptAttr.FlowEnderRDT + "','" + GERptAttr.FlowEndNode + "','" + GERptAttr.FlowStarter + "','" + GERptAttr.FlowStartRDT + "','" + GERptAttr.WFState + "')";
                DBAccess.RunSQL(sql);
            }
            catch (Exception ex)
            {
                Log.DefaultLogWriteLineError(ex.Message);
            }
            #endregion  Set the grouping field for the process 

            #region  After the end of treatment .
            GERpt sw = this.HisGERpt;
            sw.CheckPhysicsTable();  // Let reports regenerate .

            DBAccess.RunSQL("DELETE FROM Sys_GroupField WHERE EnName='" + fk_mapData + "' AND OID NOT IN (SELECT GroupID FROM Sys_MapAttr WHERE FK_MapData = '" + fk_mapData + "')");
            DBAccess.RunSQL("UPDATE Sys_MapAttr SET Name=' Time ' WHERE FK_MapData='ND" + flowId + "Rpt' AND KeyOfEn='CDT'");
            DBAccess.RunSQL("UPDATE Sys_MapAttr SET Name=' Participants ' WHERE FK_MapData='ND" + flowId + "Rpt' AND KeyOfEn='Emps'");
            #endregion  After the end of treatment .

            #region  Working with Reports .
            string mapRpt = "ND" + int.Parse(No) + "MyRpt";
            MapData mapData = new MapData();
            mapData.No = mapRpt;
            if (mapData.RetrieveFromDBSources() == 0)
            {
                mapData.No = mapRpt;
                mapData.PTable = this.PTable;
                mapData.Name = this.Name + " Report form ";
                mapData.Note = " Default .";
                mapData.Insert();

                BP.WF.Rpt.MapRpt rpt = new Rpt.MapRpt();
                rpt.No = mapRpt;
                rpt.RetrieveFromDBSources();

                rpt.FK_Flow = this.No;
                rpt.ParentMapData = "ND" + int.Parse(this.No) + "Rpt";
                rpt.ResetIt();
                rpt.Update();
            }

            if (mapData.PTable != this.PTable)
            {
                mapData.PTable = this.PTable;
                mapData.Update();
            }
            #endregion  Working with Reports .
        }
        #endregion  Other public methods 1

        #region  Execution process events .
        /// <summary>
        ///  Performing motion events 
        /// </summary>
        /// <param name="doType"> Event Type </param>
        /// <param name="currNode"> The current node </param>
        /// <param name="en"> Entity </param>
        /// <param name="atPara"> Parameters </param>
        /// <param name="objs"> Send object , Optional </param>
        /// <returns> Execution results </returns>
        public string DoFlowEventEntity(string doType, Node currNode, Entity en, string atPara, SendReturnObjs objs)
        {
            if (currNode == null)
                return null;

            string str = null;
            if (this.FEventEntity != null)
            {
                this.FEventEntity.SendReturnObjs = objs;
                str = this.FEventEntity.DoIt(doType, currNode, en, atPara);
            }

            FrmEvents fes = currNode.MapData.FrmEvents;
            if (str == null)
                str = fes.DoEventNode(doType, en, atPara);


            // What is the process of sending a message .
            switch (doType)
            {
                case EventListOfNode.SendSuccess:
                case EventListOfNode.ShitAfter:
                case EventListOfNode.ReturnAfter:
                case EventListOfNode.UndoneAfter:
                case EventListOfNode.AskerReAfter:
                    break;
                default:
                    return str;
            }

            // Get the message entity .
            FrmEvent nev = fes.GetEntityByKey(FrmEventAttr.FK_Event, doType) as FrmEvent;
            if (nev == null)
            {
                nev = new FrmEvent();
                nev.FK_Event = doType;
            }

            // Defines whether to send text messages .
            bool isSendEmail = false;
            bool isSendSMS = false;

            // Processing parameters .
            Row r = en.Row;
            try
            {
                // System parameters .
                r.Add("FK_MapData", en.ClassID);
            }
            catch
            {
                r["FK_MapData"] = en.ClassID;
            }

            if (atPara != null)
            {
                AtPara ap = new AtPara(atPara);
                foreach (string s in ap.HisHT.Keys)
                {
                    try
                    {
                        r.Add(s, ap.GetValStrByKey(s));
                    }
                    catch
                    {
                        r[s] = ap.GetValStrByKey(s);
                    }
                }
            }

            // Sub-mode processing data .
            switch (nev.MsgCtrl)
            {
                case MsgCtrl.BySet: /* In accordance with the calculation set .*/
                    isSendEmail = nev.MsgMailEnable;
                    isSendSMS = nev.SMSEnable;
                    break;
                case MsgCtrl.BySDK: /* In accordance with the calculation set .*/
                case MsgCtrl.ByFrmIsSendMsg: /* In accordance with the calculation set .*/
                    if (r.ContainsKey("IsSendEmail") == true)
                        isSendEmail = r.GetBoolenByKey("IsSendEmail");
                    if (r.ContainsKey("IsSendSMS") == true)
                        isSendSMS = r.GetBoolenByKey("IsSendSMS");
                    break;
                default:
                    break;
            }

            //  Determines whether to send a message ?
            if (isSendSMS == false && isSendEmail == false)
                return str;

            Int64 workid = Int64.Parse(en.PKVal.ToString());

            string title="";
            try{
              title=en.GetValStrByKey("Title");
            }catch
            {
            }

            string hostUrl = Glo.HostURL;
            string sid =   "{EmpStr}_" + workid + "_" + currNode.NodeID + "_" + DataType.CurrentDataTime;
            string openWorkURl = hostUrl + "WF/Do.aspx?DoType=OF&SID=" + sid;
            openWorkURl = openWorkURl.Replace("//", "/");
            openWorkURl = openWorkURl.Replace("//", "/");

            //  Define the message variables .
            string mailTitleTmp = "";
            string mailDocTmp = "";
            if (isSendEmail)
            {
                //  Title .
                mailTitleTmp = nev.MailTitle;
                mailTitleTmp = mailTitleTmp.Replace("{Title}", title)
               .Replace("@WebUser.No", WebUser.No)
               .Replace("@WebUser.Name", WebUser.Name)
               .Replace("@WF.FK_FlowName", currNode.FlowName)
                        .Replace("@WF.FK_NodeName", currNode.Name)
                .Replace("@WebUser.FK_DeptName", WebUser.FK_DeptName)
                        .Replace("@WebUser.FK_Dept", WebUser.FK_Dept)
                       
                        .Replace("@WF.FK_Node", currNode.NodeID.ToString())
                        .Replace("@WF.WorkID", workid.ToString())
                        .Replace("@WF.FK_Flow",currNode.FK_Flow)
                        
                        .Replace("@WF.Title", title)
               ;

                //  Content .
                mailDocTmp = nev.MailDoc;
                mailDocTmp = mailDocTmp.Replace("{Url}", openWorkURl)
                .Replace("{Title}", title)
                 .Replace("@WebUser.No", WebUser.No)
                .Replace("@WebUser.Name", WebUser.Name)
                 .Replace("@WF.FK_FlowName", currNode.FlowName)
                        .Replace("@WF.FK_NodeName", currNode.Name)
                        .Replace("@WebUser.FK_DeptName", WebUser.FK_DeptName)
                .Replace("@WebUser.FK_Dept", WebUser.FK_Dept)
                        
                        .Replace("@WF.FK_Node", currNode.NodeID.ToString())
                        .Replace("@WF.WorkID", workid.ToString())
                        .Replace("@WF.FK_Flow", currNode.FK_Flow)
                        
                        .Replace("@WF.Title", title)
                ;

                /* If you still have not replaced the variables down .*/
                if (mailDocTmp.Contains("@"))
                    mailDocTmp = Glo.DealExp(mailDocTmp, en, null);

                //parse dtl
                DataTable dtDtls = DBAccess.RunSQLReturnTable(string.Format("select NO,NAME from sys_mapdtl where FK_MAPDATA = 'ND{0}'", currNode.NodeID));
            foreach (DataRow row in dtDtls.Rows)
            {
                string EnsName = row["NO"].ToString(), key = "#dtl(" + EnsName + ")";
                if (mailDocTmp.Contains(key))
                    mailDocTmp = mailDocTmp.Replace(key, MapDtl.GenTableHtml(EnsName,workid));
             
            }

            //parse ddltable
                string ddltable_prefix = "#ddltable(";
                while (mailDocTmp.Contains(ddltable_prefix)) {
                    int si = mailDocTmp.IndexOf(ddltable_prefix), ei = mailDocTmp.Substring(si).IndexOf(")");
                    string pattern = mailDocTmp.Substring(si, ei+1);
                    string[] vals = pattern.Substring(ddltable_prefix.Length).TrimEnd(')').Split(',');
                    string tablename = vals[0], enname = vals[1];
                    string no = en.GetValStrByKey(enname);
                    string sql = string.Format("select Name from {0} where NO={1}",tablename,no);
                    string text = DBAccess.RunSQLReturnString(sql);
                    mailDocTmp= mailDocTmp.Replace(pattern,text);
                }


            }

            string smsDocTmp = "";
            if (isSendSMS)
            {
                smsDocTmp = nev.SMSDoc.Clone() as string;

                smsDocTmp = smsDocTmp.Replace("{Title}", title)
                .Replace("{Url}", openWorkURl)
                .Replace("@WebUser.No", WebUser.No)
                .Replace("@WebUser.Name", WebUser.Name)
                .Replace("@WebUser.FK_Dept", WebUser.FK_Dept)
                        .Replace("@WebUser.FK_DeptName", WebUser.FK_DeptName)
                        .Replace("@WF.FK_Node", currNode.NodeID.ToString())
                        .Replace("@WF.WorkID", workid.ToString())
                        .Replace("@WF.FK_Flow", currNode.FK_Flow)
                ;

                /* If you still have not replaced the variables down .*/
                if (smsDocTmp.Contains("@")==true)
                    smsDocTmp = Glo.DealExp(smsDocTmp, en, null);
            }

            // To get people to send ids.
            string toEmpIDs = "";
            switch (doType)
            {
                case EventListOfNode.SendSuccess:
                    toEmpIDs = objs.VarAcceptersID;
                    break;
                case EventListOfNode.ReturnAfter: //  If it is returned , Returned to find the party .
                    toEmpIDs = objs.VarAcceptersID;
                    break;
                default:
                    break;
            }

            //  Executed to send a message .
            string[] emps = toEmpIDs.Split(',');
            int tonodeid = objs==null?currNode.NodeID: objs.VarToNodeID;
            foreach (string emp in emps)
            {
                if (string.IsNullOrEmpty(emp))
                    continue;

                string mailDocReal = mailDocTmp.Clone() as string;
                mailDocReal = mailDocReal.Replace("{EmpStr}", emp);

                // Send a message .
                BP.WF.Dev2Interface.Port_SendMsg(emp, mailTitleTmp, mailDocReal, smsDocTmp, en.PKVal.ToString(),
                    doType, this.No, tonodeid, Int64.Parse(en.PKVal.ToString()), 0, isSendEmail, isSendSMS,this.mailUrlsPattern);
            }
            return str;
        }
        /// <summary>
        ///  Performing motion events 
        /// </summary>
        /// <param name="doType"> Event Type </param>
        /// <param name="en"> Entity Parameters </param>
        public string DoFlowEventEntity(string doType, Node currNode, Entity en, string atPara)
        {
            return DoFlowEventEntity(doType, currNode, en, atPara,null);
        }
        private BP.WF.FlowEventBase _FDEventEntity = null;
        /// <summary>
        ///  Entity class node , Did not return empty .
        /// </summary>
        private BP.WF.FlowEventBase FEventEntity
        {
            get
            {
                if (_FDEventEntity == null && this.FlowMark != "" && this.FlowEventEntity != "")
                    _FDEventEntity = BP.WF.Glo.GetFlowEventEntityByEnName(this.FlowEventEntity);
                return _FDEventEntity;
            }
        }
        #endregion  Execution process events .

        #region  Basic properties 
        /// <summary>
        ///  Is MD5 Encryption process 
        /// </summary>
        public bool IsMD5
        {
            get
            {
                return this.GetValBooleanByKey(FlowAttr.IsMD5);
            }
            set
            {
                this.SetValByKey(FlowAttr.IsMD5, value);
            }
        }
        /// <summary>
        ///  Are there documents 
        /// </summary>
        public int NumOfBill
        {
            get
            {
                return this.GetValIntByKey(FlowAttr.NumOfBill);
            }
            set
            {
                this.SetValByKey(FlowAttr.NumOfBill, value);
            }
        }
        /// <summary>
        ///  Title generation rules 
        /// </summary>
        public string TitleRole
        {
            get
            {
                return this.GetValStringByKey(FlowAttr.TitleRole);
            }
            set
            {
                this.SetValByKey(FlowAttr.TitleRole, value);
            }
        }
        /// <summary>
        ///  List 
        /// </summary>
        public int NumOfDtl
        {
            get
            {
                return this.GetValIntByKey(FlowAttr.NumOfDtl);
            }
            set
            {
                this.SetValByKey(FlowAttr.NumOfDtl, value);
            }
        }
        public decimal AvgDay
        {
            get
            {
                return this.GetValIntByKey(FlowAttr.AvgDay);
            }
            set
            {
                this.SetValByKey(FlowAttr.AvgDay, value);
            }
        }
        public int StartNodeID
        {
            get
            {
                return int.Parse(this.No + "01");
                //return this.GetValIntByKey(FlowAttr.StartNodeID);
            }
        }
        /// <summary>
        /// add 2013-01-01.
        ///  Business main table ( The default is NDxxRpt)
        /// </summary>
        public string PTable
        {
            get
            {
                string s = this.GetValStringByKey(FlowAttr.PTable);
                if (string.IsNullOrEmpty(s))
                    s = "ND" + int.Parse(this.No) + "Rpt";
                return s;
            }
            set
            {
                this.SetValByKey(FlowAttr.PTable, value);
            }
        }
        /// <summary>
        ///  Historical records show that the field .
        /// </summary>
        public string HistoryFields
        {
            get
            {
                string strs = this.GetValStringByKey(FlowAttr.HistoryFields);
                if (string.IsNullOrEmpty(strs))
                    strs = "WFState,Title,FlowStartRDT,FlowEndNode";

                return strs;
            }
        }
        /// <summary>
        ///  Whether to enable ?
        /// </summary>
        public bool IsGuestFlow
        {
            get
            {
                return this.GetValBooleanByKey(FlowAttr.IsGuestFlow);
            }
            set
            {
                this.SetValByKey(FlowAttr.IsGuestFlow, value);
            }
        }
        /// <summary>
        ///  Can independent start 
        /// </summary>
        public bool IsCanStart
        {
            get
            {
                return this.GetValBooleanByKey(FlowAttr.IsCanStart);
            }
            set
            {
                this.SetValByKey(FlowAttr.IsCanStart, value);
            }
        }
        /// <summary>
        ///  Can initiate batch 
        /// </summary>
        public bool IsBatchStart
        {
            get
            {
                return this.GetValBooleanByKey(FlowAttr.IsBatchStart);
            }
            set
            {
                this.SetValByKey(FlowAttr.IsBatchStart, value);
            }
        }
        /// <summary>
        ///  Whether to automatically calculate future processors 
        /// </summary>
        public bool IsFullSA
        {
            get
            {
                return this.GetValBooleanByKey(FlowAttr.IsFullSA);
            }
            set
            {
                this.SetValByKey(FlowAttr.IsFullSA, value);
            }
        }
        /// <summary>
        ///  Batch initiate field 
        /// </summary>
        public string BatchStartFields
        {
            get
            {
                return this.GetValStringByKey(FlowAttr.BatchStartFields);
            }
            set
            {
                this.SetValByKey(FlowAttr.BatchStartFields, value);
            }
        }
        /// <summary>
        ///  Document format 
        /// </summary>
        public string BillNoFormat
        {
            get
            {
                return this.GetValStringByKey(FlowAttr.BillNoFormat);
            }
            set
            {
                this.SetValByKey(FlowAttr.BillNoFormat, value);
            }
        }
        /// <summary>
        ///  Process Category 
        /// </summary>
        public string FK_FlowSort
        {
            get
            {
                return this.GetValStringByKey(FlowAttr.FK_FlowSort);
            }
            set
            {
                this.SetValByKey(FlowAttr.FK_FlowSort, value);
            }
        }
        /// <summary>
        ///  Parameters 
        /// </summary>
        public string Paras
        {
            get
            {
                return this.GetValStringByKey(FlowAttr.Paras);
            }
            set
            {
                this.SetValByKey(FlowAttr.Paras, value);
            }
        }
        /// <summary>
        ///  Process category names 
        /// </summary>
        public string FK_FlowSortText
        {
            get
            {
                return this.GetValRefTextByKey(FlowAttr.FK_FlowSort);
            }
        }
        /// <summary>
        ///  Designers number 
        /// </summary>
        public string DesignerNo1
        {
            get
            {
                return this.GetValStringByKey(FlowAttr.DesignerNo);
            }
            set
            {
                this.SetValByKey(FlowAttr.DesignerNo, value);
            }
        }
        /// <summary>
        ///  Designer name 
        /// </summary>
        public string DesignerName1
        {
            get
            {
                return this.GetValStringByKey(FlowAttr.DesignerName);
            }
            set
            {
                this.SetValByKey(FlowAttr.DesignerName, value);
            }
        }
        /// <summary>
        ///  The version number 
        /// </summary>
        public string Ver
        {
            get
            {
                return this.GetValStringByKey(FlowAttr.Ver);
            }
            set
            {
                this.SetValByKey(FlowAttr.Ver, value);
            }
        }
        #endregion

        #region  Calculated property 
        /// <summary>
        ///  Process Type ( Large type )
        /// </summary>
        public int FlowType_del
        {
            get
            {
                return this.GetValIntByKey(FlowAttr.FlowType);
            }
        }
        /// <summary>
        /// ( When the current node as a child process ) Check that all sub-processes to complete the parent process to automatically send 
        /// </summary>
        public bool IsAutoSendSubFlowOver
        {
            get
            {
                return this.GetValBooleanByKey(FlowAttr.IsAutoSendSubFlowOver);
            }
        }
        public string Note
        {
            get
            {
                string s = this.GetValStringByKey("Note");
                if (s.Length == 0)
                {
                    return "无";
                }
                return s;
            }
        }
        public string NoteHtml
        {
            get
            {
                if (this.Note == "无" || this.Note == "")
                    return  " Process design staff did not help write this process , Open Designer -》 This process opens -》 Right-click on the design canvas -》 Process Attributes -》 Fill in the process help .";
                else
                    return this.Note;
            }
        }
        /// <summary>
        ///  Are Multithreading automated process 
        /// </summary>
        public bool IsMutiLineWorkFlow_del
        {
            get
            {
                return false;
                /*
                if (this.FlowType==2 || this.FlowType==1 )
                    return true;
                else
                    return false;
                    */
            }
        }
        #endregion

        #region  Extended Attributes 
        /// <summary>
        ///  Application Type 
        /// </summary>
        public FlowAppType HisFlowAppType
        {
            get
            {
                return (FlowAppType)this.GetValIntByKey(FlowAttr.FlowAppType);
            }
            set
            {
                this.SetValByKey(FlowAttr.FlowAppType, (int)value);
            }
        }
        /// <summary>
        ///  Data storage mode 
        /// </summary>
        public DataStoreModel HisDataStoreModel
        {
            get
            {
                return (DataStoreModel)this.GetValIntByKey(FlowAttr.DataStoreModel);
            }
            set
            {
                this.SetValByKey(FlowAttr.DataStoreModel, (int)value);
            }
        }
        /// <summary>
        ///  Node 
        /// </summary>
        public Nodes _HisNodes = null;
        /// <summary>
        ///  His collection of nodes .
        /// </summary>
        public Nodes HisNodes
        {
            get
            {
                if (this._HisNodes == null)
                    _HisNodes = new Nodes(this.No);
                return _HisNodes;
            }
            set
            {
                _HisNodes = value;
            }
        }
        /// <summary>
        ///  His  Start  Node 
        /// </summary>
        public Node HisStartNode
        {
            get
            {

                foreach (Node nd in this.HisNodes)
                {
                    if (nd.IsStartNode)
                        return nd;
                }
                throw new Exception("@ He did not find the starting node , Workflow [" + this.Name + "] Custom Error .");
            }
        }
        /// <summary>
        ///  His business category 
        /// </summary>
        public FlowSort HisFlowSort
        {
            get
            {
                return new FlowSort(this.FK_FlowSort);
            }
        }
        /// <summary>
        /// flow data  Data 
        /// </summary>
        public BP.WF.Data.GERpt HisGERpt
        {
            get
            {
                try
                {
                    BP.WF.Data.GERpt wk = new BP.WF.Data.GERpt("ND" + int.Parse(this.No) + "Rpt");
                    return wk;
                }
                catch
                {
                    this.DoCheck();
                    BP.WF.Data.GERpt wk1 = new BP.WF.Data.GERpt("ND" + int.Parse(this.No) + "Rpt");
                    return wk1;
                }
            }
        }
        #endregion

        #region  Constructor 
       
        /// <summary>
        ///  Process 
        /// </summary>
        public Flow()
        {
        }
        /// <summary>
        ///  Process 
        /// </summary>
        /// <param name="_No"> Serial number </param>
        public Flow(string _No)
        {
            this.No = _No;
           if (1==1||SystemConfig.IsDebug)
            {
                //alway use RetrieveFromDBSources
                int i = this.RetrieveFromDBSources();
                if (i == 0)
                    throw new Exception(" Process number does not exist ");
            }
            else
            {
                this.Retrieve();
            }
        }
        protected override bool beforeUpdateInsertAction()
        {
            try
            {
                if (string.IsNullOrEmpty(this.FlowMark) == false)
                    this.FlowEventEntity = BP.WF.Glo.GetFlowEventEntityByFlowMark(this.FlowMark).ToString();
                else
                    this.FlowEventEntity = "";
            }
            catch
            {
            }

            DBAccess.RunSQL("UPDATE WF_Node SET FlowName='" + this.Name + "' WHERE FK_Flow='" + this.No + "'");
            DBAccess.RunSQL("UPDATE Sys_MapData SET  Name='" + this.Name + "' WHERE No='" + this.PTable + "'");
            return base.beforeUpdateInsertAction();
        }
        /// <summary>
        ///  Override the base class methods 
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("WF_Flow");

                map.DepositaryOfEntity = Depositary.Application;
                map.DepositaryOfMap = Depositary.Application;
                map.EnDesc = " Process ";
                map.CodeStruct = "3";
                map.AddTBStringPK(FlowAttr.No, null, null, true, true, 1, 10, 3);
                map.AddTBString(FlowAttr.Name, null, null, true, false, 0, 500, 10);
                map.AddDDLEntities(FlowAttr.FK_FlowSort, "01", " Process Category ", new FlowSorts(), false);
                map.AddDDLSysEnum(FlowAttr.FlowRunWay, (int)FlowRunWay.HandWork, " Run ", false,
                    false, FlowAttr.FlowRunWay,
                    "@0= Manually start @1= Start time designated staff @2= Data collection started on time @3= Trigger Start ");
                map.AddTBString(FlowAttr.RunObj, null, " Run content ", true, false, 0, 3000, 10);

                map.AddTBString(FlowAttr.Note, null, " Remark ", true, false, 0, 100, 10);
                map.AddTBString(FlowAttr.RunSQL, null, " After the end of the process of doing SQL", true, false, 0, 2000, 10);

                map.AddTBInt(FlowAttr.NumOfBill, 0, " Are there documents ", false, false);
                map.AddTBInt(FlowAttr.NumOfDtl, 0, "NumOfDtl", false, false);
                map.AddTBInt(FlowAttr.FlowAppType, 0, " Process Type ", false, false);
                map.AddTBInt(FlowAttr.ChartType, 1, " Node graph type ", false, false);

               // map.AddDDLSysEnum(FlowAttr.ChartType, (int)ChartType.Icon, " Node graph type ", true, true, "ChartType", "@0= Collection of graphics @1= Portrait Photos ");

               // map.AddBoolean(FlowAttr.IsOK, true, " Whether to enable ", true, true);
                map.AddBoolean(FlowAttr.IsCanStart, true, " No independent start ?", true, true, true);
                map.AddTBDecimal(FlowAttr.AvgDay, 0, " The average running in days ", false, false);


                map.AddTBInt(FlowAttr.IsFullSA, 0, " Whether to automatically calculate future processors ?( When enabled ,ccflow Would have been aware of for people to deal with node filling WF_SelectAccper)", false, false);
                map.AddTBInt(FlowAttr.IsMD5, 0, "IsMD5", false, false);
                map.AddTBInt(FlowAttr.Idx, 0, " Shows the sequence number ( Initiating list )",true, false);
                map.AddTBInt(FlowAttr.TimelineRole, 0, " Timeliness rules ", true, false);
                map.AddTBString(FlowAttr.Paras, null, " Parameters ", false, false, 0, 400, 10);

                // add 2013-01-01. 
                map.AddTBString(FlowAttr.PTable, null, " Process data stored in the primary table ", true, false, 0, 30, 10);

                // add 2013-01-01.
                map.AddTBInt(FlowAttr.DataStoreModel, 0, " Data storage mode ", true, false);

                // add 2013-02-05.
                map.AddTBString(FlowAttr.TitleRole, null, " Title generation rules ", true, false, 0, 150, 10, true);

                // add 2013-02-14 
                map.AddTBString(FlowAttr.FlowMark, null, " Process tag ", true, false, 0, 150, 10);
                map.AddTBString(FlowAttr.FlowEventEntity, null, "FlowEventEntity", true, false, 0, 100, 10, true);
                map.AddTBString(FlowAttr.HistoryFields, null, " History View Fields ", true, false, 0, 500, 10, true);
                map.AddTBInt(FlowAttr.IsGuestFlow, 0, " Is customer engagement process ?", true, false);
                map.AddTBString(FlowAttr.BillNoFormat, null, " Document Numbering Format ", true, false, 0, 200, 10, true);
                map.AddTBString(FlowAttr.FlowNoteExp, null, " Remarks expression ", true, false, 0, 500, 10, true);

               // Department access control type , This property is in the report control .
               map.AddTBInt(FlowAttr.DRCtrlType, 0, " Permissions control department inquiry ", true, false);

               #region  Process starts restrictions 
               map.AddTBInt(FlowAttr.StartLimitRole, 0, " Start limit rules ", true, false);
               map.AddTBString(FlowAttr.StartLimitPara, null, " Content Rules ", true, false, 0, 500, 10, true);
               map.AddTBString(FlowAttr.StartLimitAlert, null, " Tips restrictions ", true, false, 0, 500, 10, false);
               map.AddTBInt(FlowAttr.StartLimitWhen, 0, " Tip Time ", true, false);
               #endregion  Process starts restrictions 

               #region  Navigation .
               map.AddTBInt(FlowAttr.StartGuideWay, 0, " Pre navigation ", false, false);

               map.AddTBString(FlowAttr.StartGuidePara1, null, " Parameters 1", true, false, 0, 500, 10, true);
               map.AddTBString(FlowAttr.StartGuidePara2, null, " Parameters 2", true, false, 0, 500, 10, true);
               map.AddTBString(FlowAttr.StartGuidePara3, null, " Parameters 3", true, false, 0, 500, 10, true);
               map.AddTBInt(FlowAttr.IsResetData, 0, " Whether the data reset button is enabled ?", true, false);
           //    map.AddTBInt(FlowAttr.IsImpHistory, 0, " Whether to enable import historical data button ?", true, false);
               map.AddTBInt(FlowAttr.IsLoadPriData, 0, " Whether to import a data ?", true, false);
               #endregion  Navigation .

               map.AddTBInt(FlowAttr.CFlowWay, 0, " Continuation of the process approach ", true, false);
               map.AddTBString(FlowAttr.CFlowPara, null, " Continuation of the process parameters ", true, false, 0, 100, 10, true);

               // Batch launched  add 2013-12-27. 
               map.AddTBInt(FlowAttr.IsBatchStart, 0, " Can initiate batch ", true, false);
               map.AddTBString(FlowAttr.BatchStartFields, null, " Batch initiate field ( Separated by commas )", true, false, 0, 500, 10, true);

               // map.AddTBInt(FlowAttr.IsEnableTaskPool, 0, " Whether to enable shared task pool ", true, false);
                //map.AddDDLSysEnum(FlowAttr.TimelineRole, (int)TimelineRole.ByNodeSet, " Timeliness rules ",
                // true, true, FlowAttr.TimelineRole, "@0= By node ( Defined by node attribute )@1= By promoters ( Start node SysSDTOfFlow Field calculations )");

               map.AddTBInt(FlowAttr.IsAutoSendSubFlowOver, 0, "( When the current node as a child process ) Check that all sub-processes to complete the parent process to automatically send ", true, true);

               map.AddTBString(FlowAttr.Ver, null, " The version number ", true, true, 0, 20, 10);


                map.AddSearchAttr(FlowAttr.FK_FlowSort);
                map.AddSearchAttr(FlowAttr.FlowRunWay);

                RefMethod rm = new RefMethod();
                rm.Title = " Design inspection report "; // " Design inspection report ";
                rm.ToolTip = " Inspection process design problems .";
                rm.Icon = "/WF/Img/Btn/Confirm.gif";
                rm.ClassMethodName = this.ToString() + ".DoCheck";
                map.AddRefMethod(rm);

                //   rm = new RefMethod();
                //rm.Title = this.ToE("ViewDef", " View definition "); //" View definition ";
                //rm.Icon = "/WF/Img/Btn/View.gif";
                //rm.ClassMethodName = this.ToString() + ".DoDRpt";
                //map.AddRefMethod(rm);

                rm = new RefMethod();
                rm.Title = " Reports run "; // " Reports run ";
                rm.Icon = "/WF/Img/Btn/View.gif";
                rm.ClassMethodName = this.ToString() + ".DoOpenRpt()";
                //rm.Icon = "/WF/Img/Btn/Table.gif";
                map.AddRefMethod(rm);

                //rm = new RefMethod();
                //rm.Title = this.ToE("FlowDataOut", " Data transferred definition ");  //" Data transferred definition ";
                ////  rm.Icon = "/WF/Img/Btn/Table.gif";
                //rm.ToolTip = " In the process completion time , Process data is transferred to another storage system applications .";
                //rm.ClassMethodName = this.ToString() + ".DoExp";
                //map.AddRefMethod(rm);


                rm = new RefMethod();
                rm.Title = " Deleting Data ";
                rm.Warning = " Are you sure you want to delete the data it processes ?";
                rm.ToolTip = " Clear historical process data .";
                rm.ClassMethodName = this.ToString() + ".DoExp";
                map.AddRefMethod(rm);

                //map.AttrsOfOneVSM.Add(new FlowStations(), new Stations(), FlowStationAttr.FK_Flow,
                //    FlowStationAttr.FK_Station, DeptAttr.Name, DeptAttr.No, " Cc jobs ");

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        #region   Public Methods 
        /// <summary>
        ///  Design data turn out 
        /// </summary>
        /// <returns></returns>
        public string DoExp()
        {
            this.DoCheck();
            PubClass.WinOpen(Glo.CCFlowAppPath+"WF/Admin/Exp.aspx?CondType=0&FK_Flow=" + this.No, " Invoice ", "cdsn", 800, 500, 210, 300);
            return null;
        }
        /// <summary>
        ///  Defining Report 
        /// </summary>
        /// <returns></returns>
        public string DoDRpt()
        {
            this.DoCheck();
            PubClass.WinOpen(Glo.CCFlowAppPath+"WF/Admin/WFRpt.aspx?CondType=0&FK_Flow=" + this.No, " Invoice ", "cdsn", 800, 500, 210, 300);
            return null;
        }
        /// <summary>
        ///  Run the report 
        /// </summary>
        /// <returns></returns>
        public string DoOpenRpt()
        {
            return null;
        }
        public string DoDelData()
        {
            #region  Data deletion process forms .
            //if (DBAccess.RunSQLReturnValInt(string.Format("SELECT use_oldworkid FROM WF_Flow where no={0}", this.No)) == 0)
            if(DBAccess.RunSQLReturnString(string.Format("SELECT use_oldworkid FROM WF_Flow where no={0}", this.No)) =="0")
            {
                string mysql = "SELECT OID FROM " + this.PTable;
                FrmNodes fns = new FrmNodes();
                fns.Retrieve(FrmNodeAttr.FK_Flow, this.No);
                string strs = "";
                foreach (FrmNode nd in fns)
                {
                    if (strs.Contains("@" + nd.FK_Frm) == true)
                        continue;

                    strs += "@" + nd.FK_Frm + "@";
                    try
                    {
                        MapData md = new MapData(nd.FK_Frm);
                        DBAccess.RunSQL("DELETE FROM " + md.PTable + " WHERE OID in (" + mysql + ")");
                    }
                    catch
                    {
                    }
                }
            }
            #endregion  Data deletion process forms .



            string sql = "  where FK_Node in (SELECT NodeID FROM WF_Node WHERE fk_flow='" + this.No + "')";
            string sql1 = " where NodeID in (SELECT NodeID FROM WF_Node WHERE fk_flow='" + this.No + "')";

           // DA.DBAccess.RunSQL("DELETE FROM WF_CHOfFlow WHERE FK_Flow='" + this.No + "'");
            
            DA.DBAccess.RunSQL("DELETE FROM WF_Bill WHERE FK_Flow='" + this.No + "'");
            DA.DBAccess.RunSQL("DELETE FROM WF_GenerWorkerlist WHERE FK_Flow='" + this.No + "'");
            DA.DBAccess.RunSQL("DELETE FROM WF_GenerWorkFlow WHERE FK_Flow='" + this.No + "'");

            DA.DBAccess.RunSQL("DELETE FROM WF_GenerWorkFlow WHERE FK_Flow='" + this.No + "'");

            string sqlIn = " WHERE ReturnNode IN (SELECT NodeID FROM WF_Node WHERE FK_Flow='" + this.No + "')";
            DA.DBAccess.RunSQL("DELETE FROM WF_ReturnWork " + sqlIn);
            DA.DBAccess.RunSQL("DELETE FROM WF_GenerFH WHERE FK_Flow='" + this.No + "'");
            DA.DBAccess.RunSQL("DELETE FROM WF_SelectAccper " + sql);
                DA.DBAccess.RunSQL("DELETE FROM WF_TransferCustom " + sql);
           // DA.DBAccess.RunSQL("DELETE FROM WF_FileManager " + sql);
            DA.DBAccess.RunSQL("DELETE FROM WF_RememberMe " + sql);

            try
            {
                DA.DBAccess.RunSQL("DELETE FROM ND" + int.Parse(this.No) + "Track ");
            }
            catch
            {
            }

            if (DBAccess.IsExitsObject( this.PTable))
                DBAccess.RunSQL("DELETE FROM "+this.PTable);

            //DA.DBAccess.RunSQL("DELETE FROM WF_WorkList WHERE FK_Flow='" + this.No + "'");
            //DA.DBAccess.RunSQL("DELETE FROM Sys_MapExt WHERE FK_MapData LIKE 'ND"+int.Parse(this.No)+"%'" );

            // Delete node data .
            Nodes nds = new Nodes(this.No);
            foreach (Node nd in nds)
            {
                try
                {
                    Work wk = nd.HisWork;
                    DA.DBAccess.RunSQL("DELETE FROM " + wk.EnMap.PhysicsTable);
                }
                catch
                {
                }

                MapDtls dtls = new MapDtls("ND" + nd.NodeID);
                foreach (MapDtl dtl in dtls)
                {
                    try
                    {
                        DA.DBAccess.RunSQL("DELETE FROM " + dtl.PTable);
                    }
                    catch
                    {
                    }
                }
            }
            MapDtls mydtls = new MapDtls("ND" + int.Parse(this.No) + "Rpt");
            foreach (MapDtl dtl in mydtls)
            {
                try
                {
                    DA.DBAccess.RunSQL("DELETE FROM " + dtl.PTable);
                }
                catch
                {
                }
            }
            return " Deleted successfully ...";
        }
         
        /// <summary>
        ///  Loading process templates 
        /// </summary>
        /// <param name="fk_flowSort"> Process Category </param>
        /// <param name="path"> Process Name </param>
        /// <returns></returns>
        public static Flow DoLoadFlowTemplate(string fk_flowSort, string path, ImpFlowTempleteModel model,int SpecialFlowNo=-1)
        {
            FileInfo info = new FileInfo(path);
            DataSet ds = new DataSet();
            ds.ReadXml(path);

            if (ds.Tables.Contains("WF_Flow") == false)
                throw new Exception(" Import Error , Non-flow template file .");

            DataTable dtFlow = ds.Tables["WF_Flow"];
            Flow fl = new Flow();
            string oldFlowNo = dtFlow.Rows[0]["No"].ToString();
            string oldFlowName = dtFlow.Rows[0]["Name"].ToString();

            int oldFlowID = int.Parse(oldFlowNo);
            string timeKey = DateTime.Now.ToString("yyMMddhhmmss");

            // Judgment process marked .
            if (dtFlow.Columns.Contains("FlowMark") == true)
            {
                string FlowMark = dtFlow.Rows[0]["FlowMark"].ToString();
                if (string.IsNullOrEmpty(FlowMark) == false)
                {
                    if (fl.IsExit(FlowAttr.FlowMark, FlowMark))
                        throw new Exception("@ The process marked :" + FlowMark + " Already exists in the system , You can not import .");
                }
            }

            switch (model)
            {
                case ImpFlowTempleteModel.AsNewFlow: /* As a new process . */
                    fl.No = fl.GenerNewNo;
                    fl.DoDelData();
                    fl.DoDelete(); /* There may delete junk .*/
                    break;
                case ImpFlowTempleteModel.AsTempleteFlowNo: /* Process template with numbers */
                    fl.No = oldFlowNo;
                    if (fl.IsExits)
                    {
                        throw new Exception(" Import Error : Process Template (" + oldFlowName + ") The number (" + oldFlowNo + ") Already exists in the system , Process name :" + dtFlow.Rows[0]["Name"].ToString());
                    }
                    else
                    {
                        fl.No = oldFlowNo;
                        fl.DoDelData();
                        fl.DoDelete(); /* There may delete junk .*/
                    }
                    break;
                case ImpFlowTempleteModel.AsTempleteFlowNoOvrewaiteWhenExit: /* Process template with numbers , If there and cover it .*/
                    fl.No = oldFlowNo;
                    fl.DoDelData();
                    fl.DoDelete(); /* There may delete junk .*/
                    break;
                case ImpFlowTempleteModel.AsSpecFlowNo:
                    if (SpecialFlowNo <= 0)
                    {
                        throw new Exception("@ Specify number error ");
                    }
                    break;
                default:
                    throw new Exception("@ No judgment ");
            }

            // string timeKey = fl.No;
            int idx = 0;
            string infoErr = "";
            string infoTable = "";
            int flowID = int.Parse(fl.No);

            #region  Table data processing 
            foreach (DataColumn dc in dtFlow.Columns)
            {
                string val = dtFlow.Rows[0][dc.ColumnName] as string;
                switch (dc.ColumnName.ToLower())
                {
                    case "no":
                    case "fk_flowsort":
                        continue;
                    case "name":
                        val = " Copy :" + val + "_" + DateTime.Now.ToString("yyyyMMdd_HHmm");
                        break;
                    default:
                        break;
                }
                fl.SetValByKey(dc.ColumnName, val);
            }
            fl.FK_FlowSort = fk_flowSort;
            fl.Insert();
            #endregion  Table data processing 

            #region  Deal with OID  Insert duplicate issue  Sys_GroupField, Sys_MapAttr.
            DataTable mydtGF = ds.Tables["Sys_GroupField"];
            DataTable myDTAttr = ds.Tables["Sys_MapAttr"];
            DataTable myDTAth = ds.Tables["Sys_FrmAttachment"];
            DataTable myDTDtl = ds.Tables["Sys_MapDtl"];
            DataTable myDFrm = ds.Tables["Sys_MapFrame"];
            DataTable myDM2M = ds.Tables["Sys_MapM2M"];
            if (mydtGF != null)
            {
                //throw new Exception("@" + fl.No + fl.Name + ",  Lack :Sys_GroupField");
                foreach (DataRow dr in mydtGF.Rows)
                {
                    Sys.GroupField gf = new Sys.GroupField();
                    foreach (DataColumn dc in mydtGF.Columns)
                    {
                        string val = dr[dc.ColumnName] as string;
                        gf.SetValByKey(dc.ColumnName, val);
                    }
                    int oldID = gf.OID;
                    gf.OID = DBAccess.GenerOID();
                    dr["OID"] = gf.OID;

                    //  Property .
                    if (myDTAttr != null && myDTAttr.Columns.Contains("GroupID"))
                    {
                        foreach (DataRow dr1 in myDTAttr.Rows)
                        {
                            if (dr1["GroupID"] == null)
                                dr1["GroupID"] = 0;

                            if (dr1["GroupID"].ToString() == oldID.ToString())
                                dr1["GroupID"] = gf.OID;
                        }
                    }

                    if (myDTAth != null && myDTAth.Columns.Contains("GroupID"))
                    {
                        //  Accessory .
                        foreach (DataRow dr1 in myDTAth.Rows)
                        {
                            if (dr1["GroupID"] == null)
                                dr1["GroupID"] = 0;

                            if (dr1["GroupID"].ToString() == oldID.ToString())
                                dr1["GroupID"] = gf.OID;
                        }
                    }

                    if (myDTDtl != null && myDTDtl.Columns.Contains("GroupID"))
                    {
                        //  From Table .
                        foreach (DataRow dr1 in myDTDtl.Rows)
                        {
                            if (dr1["GroupID"] == null)
                                dr1["GroupID"] = 0;

                            if (dr1["GroupID"].ToString() == oldID.ToString())
                                dr1["GroupID"] = gf.OID;
                        }
                    }

                    if (myDFrm != null && myDFrm.Columns.Contains("GroupID"))
                    {
                        // frm.
                        foreach (DataRow dr1 in myDFrm.Rows)
                        {
                            if (dr1["GroupID"] == null)
                                dr1["GroupID"] = 0;

                            if (dr1["GroupID"].ToString() == oldID.ToString())
                                dr1["GroupID"] = gf.OID;
                        }
                    }

                    if (myDM2M != null && myDM2M.Columns.Contains("GroupID"))
                    {
                        // m2m.
                        foreach (DataRow dr1 in myDM2M.Rows)
                        {
                            if (dr1["GroupID"] == null)
                                dr1["GroupID"] = 0;

                            if (dr1["GroupID"].ToString() == oldID.ToString())
                                dr1["GroupID"] = gf.OID;
                        }
                    }
                }
            }
            #endregion  Deal with OID  Insert duplicate issue . Sys_GroupField , Sys_MapAttr.

            int timeKeyIdx = 0;
            foreach (DataTable dt in ds.Tables)
            {
                timeKeyIdx++;
                timeKey = timeKey + timeKeyIdx.ToString();

                infoTable = "@ Importing :" + dt.TableName + "  Abnormal .";
                switch (dt.TableName)
                {
                    case "WF_Flow": // Template file .
                        continue;
                    case "WF_FlowFormTree": // Process Forms directory  add 2013-12-03
                        //foreach (DataRow dr in dt.Rows)
                        //{
                        //    FlowForm cd = new FlowForm();
                        //    foreach (DataColumn dc in dt.Columns)
                        //    {
                        //        string val = dr[dc.ColumnName] as string;
                        //        if (val == null)
                        //            continue;
                        //        switch (dc.ColumnName.ToLower())
                        //        {
                        //            case "fk_flow":
                        //                val = fl.No;
                        //                break;
                        //            default:
                        //                val = val.Replace("ND" + oldFlowID, "ND" + flowID);
                        //                break;
                        //        }
                        //        cd.SetValByKey(dc.ColumnName, val);
                        //    }
                        //    cd.Insert();
                        //}
                        break;
                    case "WF_FlowForm": // Process Form . add 2013-12-03
                        //foreach (DataRow dr in dt.Rows)
                        //{
                        //    FlowForm cd = new FlowForm();
                        //    foreach (DataColumn dc in dt.Columns)
                        //    {
                        //        string val = dr[dc.ColumnName] as string;
                        //        if (val == null)
                        //            continue;
                        //        switch (dc.ColumnName.ToLower())
                        //        {
                        //            case "fk_flow":
                        //                val = fl.No;
                        //                break;
                        //            default:
                        //                val = val.Replace("ND" + oldFlowID, "ND" + flowID);
                        //                break;
                        //        }
                        //        cd.SetValByKey(dc.ColumnName, val);
                        //    }
                        //    cd.Insert();
                        //}
                        break;
                    case "WF_NodeForm": // Node permission form . 2013-12-03
                        foreach (DataRow dr in dt.Rows)
                        {
                            NodeToolbar cd = new NodeToolbar();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;
                                switch (dc.ColumnName.ToLower())
                                {
                                    case "tonodeid":
                                    case "fk_node":
                                    case "nodeid":
                                        if (val.Length == 3)
                                            val = flowID + val.Substring(1);
                                        else if (val.Length == 4)
                                            val = flowID + val.Substring(2);
                                        else if (val.Length == 5)
                                            val = flowID + val.Substring(3);
                                        break;
                                    case "fk_flow":
                                        val = fl.No;
                                        break;
                                    default:
                                        val = val.Replace("ND" + oldFlowID, "ND" + flowID);
                                        break;
                                }
                                cd.SetValByKey(dc.ColumnName, val);
                            }
                            cd.Insert();
                        }
                        break;
                    case "Sys_FrmSln": // Permission form fields . 2013-12-03
                        foreach (DataRow dr in dt.Rows)
                        {
                            FrmField cd = new FrmField();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;
                                switch (dc.ColumnName.ToLower())
                                {
                                    case "tonodeid":
                                    case "fk_node":
                                    case "nodeid":
                                        if (val.Length == 3)
                                            val = flowID + val.Substring(1);
                                        else if (val.Length == 4)
                                            val = flowID + val.Substring(2);
                                        else if (val.Length == 5)
                                            val = flowID + val.Substring(3);
                                        break;
                                    case "fk_flow":
                                        val = fl.No;
                                        break;
                                    default:
                                        val = val.Replace("ND" + oldFlowID, "ND" + flowID);
                                        break;
                                }
                                cd.SetValByKey(dc.ColumnName, val);
                            }
                            cd.Insert();
                        }
                        break;
                    case "WF_NodeToolbar": // Toolbar .
                        foreach (DataRow dr in dt.Rows)
                        {
                            NodeToolbar cd = new NodeToolbar();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;
                                switch (dc.ColumnName.ToLower())
                                {
                                    case "tonodeid":
                                    case "fk_node":
                                    case "nodeid":
                                        if (val.Length == 3)
                                            val = flowID + val.Substring(1);
                                        else if (val.Length == 4)
                                            val = flowID + val.Substring(2);
                                        else if (val.Length == 5)
                                            val = flowID + val.Substring(3);
                                        break;
                                    case "fk_flow":
                                        val = fl.No;
                                        break;
                                    default:
                                        val = val.Replace("ND" + oldFlowID, "ND" + flowID);
                                        break;
                                }
                                cd.SetValByKey(dc.ColumnName, val);
                            }
                            cd.OID = DA.DBAccess.GenerOID();
                            cd.DirectInsert();
                        }
                        break;
                    case "WF_BillTemplate":
                        continue; /* Because eliminating the need for   Processing the print template .*/
                        foreach (DataRow dr in dt.Rows)
                        {
                            BillTemplate bt = new BillTemplate();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;
                                switch (dc.ColumnName.ToLower())
                                {
                                    case "fk_flow":
                                        val = flowID.ToString();
                                        break;
                                    case "nodeid":
                                    case "fk_node":
                                        if (val.Length == 3)
                                            val = flowID + val.Substring(1);
                                        else if (val.Length == 4)
                                            val = flowID + val.Substring(2);
                                        else if (val.Length == 5)
                                            val = flowID + val.Substring(3);
                                        break;
                                    default:
                                        break;
                                }
                                bt.SetValByKey(dc.ColumnName, val);
                            }
                            int i = 0;
                            string no = bt.No;
                            while (bt.IsExits)
                            {
                                bt.No = no + i.ToString();
                                i++;
                            }

                            try
                            {
                                File.Copy(info.DirectoryName + "\\" + no + ".rtf", BP.Sys.SystemConfig.PathOfWebApp + @"\DataUser\CyclostyleFile\" + bt.No + ".rtf", true);
                            }
                            catch (Exception ex)
                            {
                                // infoErr += "@ An error occurred while restoring the document template :" + ex.Message + ", There may be you do not have to copy the document template file in the same directory replication process templates .";
                            }
                            bt.Insert();
                        }
                        break;
                    case "WF_FrmNode": //Conds.xml.
                        DBAccess.RunSQL("DELETE FROM WF_FrmNode WHERE FK_Flow='" + fl.No + "'");
                        foreach (DataRow dr in dt.Rows)
                        {
                            FrmNode fn = new FrmNode();
                            fn.FK_Flow = fl.No;
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;
                                switch (dc.ColumnName.ToLower())
                                {
                                    case "fk_node":
                                        if (val.Length == 3)
                                            val = flowID + val.Substring(1);
                                        else if (val.Length == 4)
                                            val = flowID + val.Substring(2);
                                        else if (val.Length == 5)
                                            val = flowID + val.Substring(3);
                                        break;
                                    case "fk_flow":
                                        val = fl.No;
                                        break;
                                    default:
                                        break;
                                }
                                fn.SetValByKey(dc.ColumnName, val);
                            }
                            //  Begin inserting .
                            fn.MyPK = fn.FK_Frm + "_" + fn.FK_Node;
                            fn.Insert();
                        }
                        break;
                    case "WF_FindWorkerRole": // Someone Rules 
                        foreach (DataRow dr in dt.Rows)
                        {
                            FindWorkerRole en = new FindWorkerRole();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;
                                switch (dc.ColumnName.ToLower())
                                {
                                    case "fk_node":
                                    case "nodeid":
                                        if (val.Length == 3)
                                            val = flowID + val.Substring(1);
                                        else if (val.Length == 4)
                                            val = flowID + val.Substring(2);
                                        else if (val.Length == 5)
                                            val = flowID + val.Substring(3);
                                        break;
                                    case "fk_flow":
                                        val = fl.No;
                                        break;
                                    default:
                                        val = val.Replace("ND" + oldFlowID, "ND" + flowID);
                                        break;
                                }
                                en.SetValByKey(dc.ColumnName, val);
                            }

                            // Insert .
                            en.DirectInsert();
                        }
                        break;
                    case "WF_Cond": //Conds.xml.
                        foreach (DataRow dr in dt.Rows)
                        {
                            Cond cd = new Cond();
                            cd.FK_Flow = fl.No;
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;

                                switch (dc.ColumnName.ToLower())
                                {
                                    case "tonodeid":
                                    case "fk_node":
                                    case "nodeid":
                                        if (val.Length == 3)
                                            val = flowID + val.Substring(1);
                                        else if (val.Length == 4)
                                            val = flowID + val.Substring(2);
                                        else if (val.Length == 5)
                                            val = flowID + val.Substring(3);
                                        break;
                                    case "fk_flow":
                                        val = fl.No;
                                        break;
                                    default:
                                        val = val.Replace("ND" + oldFlowID, "ND" + flowID);
                                        break;
                                }
                                cd.SetValByKey(dc.ColumnName, val);
                            }

                            cd.FK_Flow = fl.No;

                            //  return this.FK_MainNode + "_" + this.ToNodeID + "_" + this.HisCondType.ToString() + "_" + ConnDataFrom.Stas.ToString();
                            // , Begin inserting . 
                            if (cd.MyPK.Contains("Stas"))
                            {
                                cd.MyPK = cd.FK_Node + "_" + cd.ToNodeID + "_" + cd.HisCondType.ToString() + "_" + ConnDataFrom.Stas.ToString();
                            }
                            else if (cd.MyPK.Contains("Dept"))
                            {
                                cd.MyPK = cd.FK_Node + "_" + cd.ToNodeID + "_" + cd.HisCondType.ToString() + "_" + ConnDataFrom.Depts.ToString();
                            }
                            else if (cd.MyPK.Contains("Paras"))
                            {
                                cd.MyPK = cd.FK_Node + "_" + cd.ToNodeID + "_" + cd.HisCondType.ToString() + "_" + ConnDataFrom.Paras.ToString();
                            }
                            else if (cd.MyPK.Contains("Url"))
                            {
                                cd.MyPK = cd.FK_Node + "_" + cd.ToNodeID + "_" + cd.HisCondType.ToString() + "_" + ConnDataFrom.Url.ToString();
                            }
                            else
                            {
                                cd.MyPK = DA.DBAccess.GenerOID().ToString() + DateTime.Now.ToString("yyMMddHHmmss");
                            }
                            cd.DirectInsert();
                        }
                        break;
                    case "WF_NodeReturn":// Returnable node .
                        foreach (DataRow dr in dt.Rows)
                        {
                            NodeReturn cd = new NodeReturn();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;
                                switch (dc.ColumnName.ToLower())
                                {
                                    case "fk_node":
                                    case "returnn":
                                        if (val.Length == 3)
                                            val = flowID + val.Substring(1);
                                        else if (val.Length == 4)
                                            val = flowID + val.Substring(2);
                                        else if (val.Length == 5)
                                            val = flowID + val.Substring(3);
                                        break;
                                    default:
                                        break;
                                }
                                cd.SetValByKey(dc.ColumnName, val);
                            }

                            // Begin inserting .
                            try
                            {
                                cd.Insert();
                            }
                            catch
                            {
                                cd.Update();
                            }
                        }
                        break;
                    case "WF_Direction": // Direction .
                        foreach (DataRow dr in dt.Rows)
                        {
                            Direction dir = new Direction();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;
                                switch (dc.ColumnName.ToLower())
                                {
                                    case "node":
                                    case "tonode":
                                        if (val.Length == 3)
                                            val = flowID + val.Substring(1);
                                        else if (val.Length == 4)
                                            val = flowID + val.Substring(2);
                                        else if (val.Length == 5)
                                            val = flowID + val.Substring(3);
                                        break;
                                    default:
                                        break;
                                }
                                dir.SetValByKey(dc.ColumnName, val);
                            }
                            dir.FK_Flow = fl.No;
                            dir.Insert();
                        }
                        break;
                    case "WF_TurnTo": // Steering Rules .
                        foreach (DataRow dr in dt.Rows)
                        {
                            TurnTo fs = new TurnTo();

                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;
                                switch (dc.ColumnName.ToLower())
                                {
                                    case "fk_node":
                                        if (val.Length == 3)
                                            val = flowID + val.Substring(1);
                                        else if (val.Length == 4)
                                            val = flowID + val.Substring(2);
                                        else if (val.Length == 5)
                                            val = flowID + val.Substring(3);
                                        break;
                                    default:
                                        break;
                                }
                                fs.SetValByKey(dc.ColumnName, val);
                            }
                            fs.FK_Flow = fl.No;
                            fs.Save();
                        }
                        break;
                    case "WF_FAppSet": //FAppSets.xml.
                        continue;
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    FAppSet fs = new FAppSet();
                    //    fs.FK_Flow = fl.No;
                    //    foreach (DataColumn dc in dt.Columns)
                    //    {
                    //        string val = dr[dc.ColumnName] as string;
                    //        if (val == null)
                    //            continue;

                    //        switch (dc.ColumnName.ToLower())
                    //        {
                    //            case "fk_node":
                    //                if (val.Length == 3)
                    //                    val = flowID + val.Substring(1);
                    //                else if (val.Length == 4)
                    //                    val = flowID + val.Substring(2);
                    //                else if (val.Length == 5)
                    //                    val = flowID + val.Substring(3);
                    //                break;
                    //            default:
                    //                break;
                    //        }
                    //        fs.SetValByKey(dc.ColumnName, val);
                    //    }
                    //    fs.OID = 0;
                    //    fs.Insert();
                    //}
                    case "WF_LabNote": //LabNotes.xml.
                        idx = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            LabNote ln = new LabNote();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;
                                ln.SetValByKey(dc.ColumnName, val);
                            }
                            idx++;
                            ln.FK_Flow = fl.No;
                            ln.MyPK = ln.FK_Flow + "_" + ln.X + "_" + ln.Y + "_" + idx;
                            ln.DirectInsert();
                        }
                        break;
                    case "WF_NodeDept": //FAppSets.xml.
                        foreach (DataRow dr in dt.Rows)
                        {
                            NodeDept dir = new NodeDept();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;

                                switch (dc.ColumnName.ToLower())
                                {
                                    case "fk_node":
                                        if (val.Length == 3)
                                            val = flowID + val.Substring(1);
                                        else if (val.Length == 4)
                                            val = flowID + val.Substring(2);
                                        else if (val.Length == 5)
                                            val = flowID + val.Substring(3);
                                        break;
                                    default:
                                        break;
                                }
                                dir.SetValByKey(dc.ColumnName, val);
                            }
                            dir.Insert();
                        }

                        break;
                    case "WF_Node": //LabNotes.xml.
                        foreach (DataRow dr in dt.Rows)
                        {
                            BP.WF.Template.Ext.NodeSheet nd = new BP.WF.Template.Ext.NodeSheet();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;

                                switch (dc.ColumnName.ToLower())
                                {
                                    case "nodeid":
                                        if (val.Length == 3)
                                            val = flowID + val.Substring(1);
                                        else if (val.Length == 4)
                                            val = flowID + val.Substring(2);
                                        else if (val.Length == 5)
                                            val = flowID + val.Substring(3);
                                        break;
                                    case "fk_flow":
                                    case "fk_flowsort":
                                        continue;
                                    case "showsheets":
                                    case "histonds":
                                    case "groupstands":
                                        string key = "@" + flowID;
                                        val = val.Replace(key, "");
                                        break;
                                    default:
                                        break;
                                }
                                nd.SetValByKey(dc.ColumnName, val);
                            }

                            nd.FK_Flow = fl.No;
                            nd.FlowName = fl.Name;
                            nd.DirectInsert();

                            // Delete mapdata.
                            DBAccess.RunSQL("DELETE FROM Sys_MapAttr WHERE FK_MapData='ND" + nd.NodeID + "'");
                        }
                        foreach (DataRow dr in dt.Rows)
                        {
                            Node nd = new Node();
                            nd.NodeID = int.Parse(dr[NodeAttr.NodeID].ToString());
                            nd.RetrieveFromDBSources();
                            nd.FK_Flow = fl.No;
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;
                                switch (dc.ColumnName.ToLower())
                                {
                                    case "nodeid":
                                        if (val.Length == 3)
                                            val = flowID + val.Substring(1);
                                        else if (val.Length == 4)
                                            val = flowID + val.Substring(2);
                                        else if (val.Length == 5)
                                            val = flowID + val.Substring(3);
                                        break;
                                    case "fk_flow":
                                    case "fk_flowsort":
                                        continue;
                                    case "showsheets":
                                    case "histonds":
                                    case "groupstands":
                                        string key = "@" + flowID;
                                        val = val.Replace(key, "");
                                        break;
                                    default:
                                        break;
                                }
                                nd.SetValByKey(dc.ColumnName, val);
                            }
                            nd.FK_Flow = fl.No;
                            nd.FlowName = fl.Name;
                            nd.DirectUpdate();
                        }
                        break;
                    case "WF_NodeStation": //FAppSets.xml.
                        DBAccess.RunSQL("DELETE FROM WF_NodeStation WHERE FK_Node IN (SELECT NodeID FROM WF_Node WHERE FK_Flow='" + fl.No + "')");
                        foreach (DataRow dr in dt.Rows)
                        {
                            NodeStation ns = new NodeStation();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;

                                switch (dc.ColumnName.ToLower())
                                {
                                    case "fk_node":
                                        if (val.Length == 3)
                                            val = flowID + val.Substring(1);
                                        else if (val.Length == 4)
                                            val = flowID + val.Substring(2);
                                        else if (val.Length == 5)
                                            val = flowID + val.Substring(3);
                                        break;
                                    default:
                                        break;
                                }
                                ns.SetValByKey(dc.ColumnName, val);
                            }
                            ns.Insert();
                        }
                        break;
                    case "WF_Listen": //  Information listeners .
                        foreach (DataRow dr in dt.Rows)
                        {
                            Listen li = new Listen();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;

                                switch (dc.ColumnName.ToLower())
                                {
                                    case "oid":
                                        continue;
                                        break;
                                    case "fk_node":
                                        if (val.Length == 3)
                                            val = flowID + val.Substring(1);
                                        else if (val.Length == 4)
                                            val = flowID + val.Substring(2);
                                        else if (val.Length == 5)
                                            val = flowID + val.Substring(3);
                                        break;
                                    case "nodes":
                                        string[] nds = val.Split('@');
                                        string valExt = "";
                                        foreach (string nd in nds)
                                        {
                                            if (nd == "" || nd == null)
                                                continue;
                                            string ndExt = nd.Clone() as string;
                                            if (ndExt.Length == 3)
                                                ndExt = flowID + ndExt.Substring(1);
                                            else if (val.Length == 4)
                                                ndExt = flowID + ndExt.Substring(2);
                                            else if (val.Length == 5)
                                                ndExt = flowID + ndExt.Substring(3);
                                            ndExt = "@" + ndExt;
                                            valExt += ndExt;
                                        }
                                        val = valExt;
                                        break;
                                    default:
                                        break;
                                }
                                li.SetValByKey(dc.ColumnName, val);
                            }
                            li.Insert();
                        }
                        break;
                    case "Sys_Enum": //RptEmps.xml.
                        foreach (DataRow dr in dt.Rows)
                        {
                            Sys.SysEnum se = new Sys.SysEnum();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                switch (dc.ColumnName.ToLower())
                                {
                                    case "fk_node":
                                        break;
                                    default:
                                        break;
                                }
                                se.SetValByKey(dc.ColumnName, val);
                            }
                            se.MyPK = se.EnumKey + "_" + se.Lang + "_" + se.IntKey;
                            if (se.IsExits)
                                continue;
                            se.Insert();
                        }
                        break;
                    case "Sys_EnumMain": //RptEmps.xml.
                        foreach (DataRow dr in dt.Rows)
                        {
                            Sys.SysEnumMain sem = new Sys.SysEnumMain();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;
                                sem.SetValByKey(dc.ColumnName, val);
                            }
                            if (sem.IsExits)
                                continue;
                            sem.Insert();
                        }
                        break;
                    case "Sys_MapAttr": //RptEmps.xml.
                        foreach (DataRow dr in dt.Rows)
                        {
                            Sys.MapAttr ma = new Sys.MapAttr();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                switch (dc.ColumnName.ToLower())
                                {
                                    case "fk_mapdata":
                                    case "keyofen":
                                    case "autofulldoc":
                                        if (val == null)
                                            continue;

                                        val = val.Replace("ND" + oldFlowID, "ND" + flowID);
                                        break;
                                    default:
                                        break;
                                }
                                ma.SetValByKey(dc.ColumnName, val);
                            }
                            bool b = ma.IsExit(Sys.MapAttrAttr.FK_MapData, ma.FK_MapData,
                                Sys.MapAttrAttr.KeyOfEn, ma.KeyOfEn);

                            ma.MyPK = ma.FK_MapData + "_" + ma.KeyOfEn;
                            if (b == true)
                                ma.DirectUpdate();
                            else
                                ma.DirectInsert();
                        }
                        break;
                    case "Sys_MapData": //RptEmps.xml.
                        foreach (DataRow dr in dt.Rows)
                        {
                            Sys.MapData md = new Sys.MapData();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;

                                val = val.Replace("ND" + oldFlowID, "ND" + int.Parse(fl.No));
                                md.SetValByKey(dc.ColumnName, val);
                            }
                            md.Save();
                        }
                        break;
                    case "Sys_MapDtl": //RptEmps.xml.
                        foreach (DataRow dr in dt.Rows)
                        {
                            Sys.MapDtl md = new Sys.MapDtl();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;

                                val = val.Replace("ND" + oldFlowID, "ND" + flowID);
                                md.SetValByKey(dc.ColumnName, val);
                            }
                            md.Save();
                        }
                        break;
                    case "Sys_MapExt":
                        foreach (DataRow dr in dt.Rows)
                        {
                            Sys.MapExt md = new Sys.MapExt();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;

                                val = val.Replace("ND" + oldFlowID, "ND" + flowID);
                                md.SetValByKey(dc.ColumnName, val);
                            }
                            md.Save();
                        }
                        break;
                    case "Sys_FrmLine":
                        idx = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            idx++;
                            FrmLine en = new FrmLine();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;

                                val = val.Replace("ND" + oldFlowID, "ND" + flowID);
                                en.SetValByKey(dc.ColumnName, val);
                            }

                            en.MyPK = Guid.NewGuid().ToString(); 
                            // BP.DA.DBAccess.GenerOIDByGUID(); "LIE" + timeKey + "_" + idx;
                            //if (en.IsExitGenerPK())
                            //    continue;
                            en.Insert();
                        }
                        break;
                    case "Sys_FrmEle":
                        idx = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            idx++;
                            FrmEle en = new FrmEle();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;

                                val = val.Replace("ND" + oldFlowID, "ND" + flowID);
                                en.SetValByKey(dc.ColumnName, val);
                            }
                            en.Insert();
                        }
                        break;
                    case "Sys_FrmImg":
                        idx = 0;
                        timeKey = DateTime.Now.ToString("YYYYMMddHHmmss");
                        foreach (DataRow dr in dt.Rows)
                        {
                            idx++;
                            FrmImg en = new FrmImg();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;

                                val = val.Replace("ND" + oldFlowID, "ND" + flowID);
                                en.SetValByKey(dc.ColumnName, val);
                            }
                            
                            try
                            {
                                en.MyPK = "Img" + timeKey + "_" + idx;
                                en.Insert();
                            }
                            catch
                            {
                                en.MyPK = Guid.NewGuid().ToString();
                                en.Insert();
                            }
                        }
                        break;
                    case "Sys_FrmLab":
                        idx = 0;
                        timeKey = DateTime.Now.ToString("yyyyMMddHHmmss");
                        foreach (DataRow dr in dt.Rows)
                        {
                            idx++;
                            FrmLab en = new FrmLab();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;

                                val = val.Replace("ND" + oldFlowID, "ND" + flowID);
                                en.SetValByKey(dc.ColumnName, val);
                            }

                            //en.MyPK = Guid.NewGuid().ToString();
                            //  Duplicate 
                            try
                            {
                                en.MyPK = "Lab" + timeKey + "_" + idx;
                                en.Insert();
                            }
                            catch
                            {
                                en.MyPK = Guid.NewGuid().ToString();
                                en.Insert();
                            }
                        }
                        break;
                    case "Sys_FrmLink":
                        idx = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            idx++;
                            FrmLink en = new FrmLink();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                val = val.Replace("ND" + oldFlowID, "ND" + flowID);
                                if (val == null)
                                    continue;

                                en.SetValByKey(dc.ColumnName, val);
                            }
                            en.MyPK = Guid.NewGuid().ToString();
                            //en.MyPK = "LK" + timeKey + "_" + idx;
                            en.Insert();
                        }
                        break;
                    case "Sys_FrmAttachment":
                        idx = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            idx++;
                            FrmAttachment en = new FrmAttachment();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;

                                val = val.Replace("ND" + oldFlowID, "ND" + flowID);
                                en.SetValByKey(dc.ColumnName, val);
                            }
                            en.MyPK = en.FK_MapData + "_" + en.NoOfObj;
                            en.Insert();
                        }
                        break;
                    case "Sys_FrmEvent": // Event .
                        idx = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            idx++;
                            FrmEvent en = new FrmEvent();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;

                                val = val.Replace("ND" + oldFlowID, "ND" + flowID);
                                en.SetValByKey(dc.ColumnName, val);
                            }
                            en.Insert();
                        }
                        break;
                    case "Sys_MapM2M": //Sys_MapM2M.
                        idx = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            idx++;
                            MapM2M en = new MapM2M();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;

                                val = val.Replace("ND" + oldFlowID, "ND" + flowID);
                                en.SetValByKey(dc.ColumnName, val);
                            }
                            en.Insert();
                        }
                        break;
                    case "Sys_FrmRB": //Sys_FrmRB.
                        idx = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            idx++;
                            FrmRB en = new FrmRB();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;

                                val = val.Replace("ND" + oldFlowID, "ND" + flowID);
                                en.SetValByKey(dc.ColumnName, val);
                            }
                            en.Insert();
                        }
                        break;
                    case "WF_NodeEmp": //FAppSets.xml.
                        foreach (DataRow dr in dt.Rows)
                        {
                            NodeEmp ne = new NodeEmp();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;

                                switch (dc.ColumnName.ToLower())
                                {
                                    case "fk_node":
                                        if (val.Length == 3)
                                            val = flowID + val.Substring(1);
                                        else if (val.Length == 4)
                                            val = flowID + val.Substring(2);
                                        else if (val.Length == 5)
                                            val = flowID + val.Substring(3);
                                        break;
                                    default:
                                        break;
                                }
                                ne.SetValByKey(dc.ColumnName, val);
                            }
                            ne.Insert();
                        }
                        break;
                    case "Sys_GroupField": // 
                        foreach (DataRow dr in dt.Rows)
                        {
                            Sys.GroupField gf = new Sys.GroupField();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;

                                switch (dc.ColumnName.ToLower())
                                {
                                    case "enname":
                                    case "keyofen":
                                        val = val.Replace("ND" + oldFlowID, "ND" + flowID);
                                        break;
                                    default:
                                        break;
                                }
                                gf.SetValByKey(dc.ColumnName, val);
                            }
                            //  int oid = DBAccess.GenerOID();
                            //  DBAccess.RunSQL("UPDATE Sys_MapAttr SET GroupID=" + gf.OID + " WHERE FK_MapData='" + gf.EnName + "' AND GroupID=" + gf.OID);
                            gf.InsertAsOID(gf.OID);
                        }
                        break;
                    case "WF_CCDept": //  Cc .
                        foreach (DataRow dr in dt.Rows)
                        {
                            CCDept ne = new CCDept();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;

                                switch (dc.ColumnName.ToLower())
                                {
                                    case "fk_node":
                                        if (val.Length == 3)
                                            val = flowID + val.Substring(1);
                                        else if (val.Length == 4)
                                            val = flowID + val.Substring(2);
                                        else if (val.Length == 5)
                                            val = flowID + val.Substring(3);
                                        break;
                                    default:
                                        break;
                                }
                                ne.SetValByKey(dc.ColumnName, val);
                            }
                            ne.Insert();
                        }
                        break;
                    case "WF_CCEmp": //  Cc .
                        foreach (DataRow dr in dt.Rows)
                        {
                            CCEmp ne = new CCEmp();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;

                                switch (dc.ColumnName.ToLower())
                                {
                                    case "fk_node":
                                        if (val.Length == 3)
                                            val = flowID + val.Substring(1);
                                        else if (val.Length == 4)
                                            val = flowID + val.Substring(2);
                                        else if (val.Length == 5)
                                            val = flowID + val.Substring(3);
                                        break;
                                    default:
                                        break;
                                }
                                ne.SetValByKey(dc.ColumnName, val);
                            }
                            ne.Insert();
                        }
                        break;
                    case "WF_CCStation": //  Cc .
                        foreach (DataRow dr in dt.Rows)
                        {
                            CCStation ne = new CCStation();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                string val = dr[dc.ColumnName] as string;
                                if (val == null)
                                    continue;

                                switch (dc.ColumnName.ToLower())
                                {
                                    case "fk_node":
                                        if (val.Length == 3)
                                            val = flowID + val.Substring(1);
                                        else if (val.Length == 4)
                                            val = flowID + val.Substring(2);
                                        else if (val.Length == 5)
                                            val = flowID + val.Substring(3);
                                        break;
                                    default:
                                        break;
                                }
                                ne.SetValByKey(dc.ColumnName, val);
                            }
                            ne.Insert();
                        }
                        break;
                    default:
                        // infoErr += "Error:" + dt.TableName;
                        break;
                    //    throw new Exception("@unhandle named " + dt.TableName);
                }
            }

            #region  Processing data integrity .
            DBAccess.RunSQL("UPDATE WF_Cond SET FK_Node=NodeID WHERE FK_Node=0");
            DBAccess.RunSQL("UPDATE WF_Cond SET ToNodeID=NodeID WHERE ToNodeID=0");

            DBAccess.RunSQL("DELETE FROM WF_Cond WHERE NodeID NOT IN (SELECT NodeID FROM WF_Node)");
            DBAccess.RunSQL("DELETE FROM WF_Cond WHERE ToNodeID NOT IN (SELECT NodeID FROM WF_Node) " );
            DBAccess.RunSQL("DELETE FROM WF_Cond WHERE FK_Node NOT IN (SELECT NodeID FROM WF_Node) AND FK_Node > 0");
            #endregion

            if (infoErr == "")
            {
                ////  Delete blank lines .
                //BP.DTS.DeleteBlankGroupField en = new BP.DTS.DeleteBlankGroupField();
                //en.Do();

                infoTable = "";
                fl.DoCheck();

                // Write permissions .
                fl.WritToGPM(fl.FK_FlowSort);

                return fl; // " Entirely successful .";
            }

            infoErr = "@ The following non-fatal error occurs during execution :\t\r" + infoErr + "@ " + infoTable;
            throw new Exception(infoErr);
            //}
            //catch (Exception ex)
            //{
            //    try
            //    {
            //        fl.DoDelete();
            //        throw new Exception("@" + infoErr + " @table=" + infoTable + "@" + ex.Message);
            //    }
            //    catch (Exception ex1)
            //    {
            //        throw new Exception("@ During the process of data deletion has generated errors error :" + ex1.Message );
            //    }
            //}
        }
        public Node DoNewNode(int x, int y)
        {
            Node nd = new Node();
            int idx = this.HisNodes.Count;
            if (idx == 0) 
                idx++;

            while (true)
            {
                string strID = this.No + idx.ToString().PadLeft(2, '0');
                nd.NodeID = int.Parse(strID);
                if (!nd.IsExits)
                    break;
                idx++;
            }

            nd.HisNodeWorkType = NodeWorkType.Work;
            nd.Name = " Node " + idx;
            nd.HisNodePosType = NodePosType.Mid;
            nd.FK_Flow = this.No;
            nd.FlowName = this.Name;
            nd.X = x;
            nd.Y = y;
            nd.Step = idx;
            nd.Insert();

            nd.CreateMap();
            return nd;
        }
        /// <summary>
        ///  Implementation of the new 
        /// </summary>
        /// <param name="flowSort"> Category </param>
        /// <param name="flowName"> Process Name </param>
        /// <param name="model"> Data storage mode </param>
        /// <param name="pTable"> Data Storage physical table </param>
        /// <param name="FlowMark"> Process tag </param>
        public void DoNewFlow(string flowSort, string flowName, DataStoreModel model, string pTable, string FlowMark)
        {
            try
            {
                // Integrity checking parameters .
                if (string.IsNullOrEmpty(pTable) == false && pTable.Length >= 1)
                {
                    string c = pTable.Substring(0, 1);
                    if (DataType.IsNumStr(c) == true)
                        throw new Exception("@ Illegal process data table (" + pTable + "), It will lead to ccflow You can not create this table .");
                }

                this.Name = flowName;
                if (string.IsNullOrWhiteSpace(this.Name))
                    this.Name = " New Process " + this.No; // New Process .

                this.No = this.GenerNewNoByKey(FlowAttr.No);
                this.HisDataStoreModel = model;
                this.PTable = pTable;
                this.FK_FlowSort = flowSort;
                this.FlowMark = FlowMark;

                if (string.IsNullOrEmpty(FlowMark) == false)
                {
                    if (this.IsExit(FlowAttr.FlowMark, FlowMark))
                        throw new Exception("@ The process marked :" + FlowMark + " Already exists in the system .");
                }

                /* To the initial value */
                //this.Paras = "@StartNodeX=10@StartNodeY=15@EndNodeX=40@EndNodeY=10";
                this.Paras = "@StartNodeX=200@StartNodeY=50@EndNodeX=200@EndNodeY=350";
                this.Save();

                #region  Delete historical data that may exist .
                Flow fl = new Flow(this.No);
                fl.DoDelData();
                fl.DoDelete();

                // Designers , Designers ?
                //fl.DesignerNo = WebUser.No;
                //fl.DesignerName = WebUser.Name;
                this.Save();
                #endregion  Delete historical data that may exist .

                Node nd = new Node();
                nd.NodeID = int.Parse(this.No + "01");
                nd.Name = " Start node ";//  " Start node "; 
                nd.Step = 1;
                nd.FK_Flow = this.No;
                nd.FlowName = this.Name;
                nd.HisNodePosType = NodePosType.Start;
                nd.HisNodeWorkType = NodeWorkType.StartWork;
                nd.X = 200;
                nd.Y = 150;
                nd.ICON = " Reception ";
                nd.Insert();

                nd.CreateMap();
                nd.HisWork.CheckPhysicsTable();

                nd = new Node();
                nd.NodeID = int.Parse(this.No + "02");
                nd.Name = " Node 2"; // " End node ";
                nd.Step = 2;
                nd.FK_Flow = this.No;
                nd.FlowName = this.Name;
                nd.HisNodePosType = NodePosType.Mid;
                nd.HisNodeWorkType = NodeWorkType.Work;
                nd.X = 200;
                nd.Y = 250;
                nd.ICON = " Check ";
                nd.Insert();
                nd.CreateMap();
                nd.HisWork.CheckPhysicsTable();

                BP.Sys.MapData md = new BP.Sys.MapData();
                md.No = "ND" + int.Parse(this.No) + "Rpt";
                md.Name = this.Name;
                md.Save();

                //  Load Template .
                string file = BP.Sys.SystemConfig.PathOfDataUser + "XML\\TempleteSheetOfStartNode.xml";
                if (System.IO.File.Exists(file))
                {
                    /* If there is a start node form templates */
                    DataSet ds = new DataSet();
                    ds.ReadXml(file);

                    string nodeID = "ND" + int.Parse(this.No + "01");
                    BP.Sys.MapData.ImpMapData(nodeID, ds, false);
                }
                else
                {
                    #region  Generate CCForm  Decorative .
                    FrmImg img = new FrmImg();
                    img.MyPK = "Img" + DateTime.Now.ToString("yyMMddhhmmss") + WebUser.No;
                    img.FK_MapData = "ND" + int.Parse(this.No + "01");
                    img.X = (float)577.26;
                    img.Y = (float)3.45;

                    img.W = (float)137;
                    img.H = (float)40;

                    img.ImgURL = "/ccform;component/Img/LogoBig.png";
                    img.LinkURL = "http://ccflow.org";
                    img.LinkTarget = "_blank";
                    img.Insert();

                    FrmLab lab = new FrmLab();
                    if (Glo.IsEnablePRI)
                    {
                        lab = new FrmLab();
                        lab.MyPK = "Lab" + DateTime.Now.ToString("yyMMddhhmmss") + WebUser.No + 1;
                        lab.Text = " Priority ";
                        lab.FK_MapData = "ND" + int.Parse(this.No + "01");
                        lab.X = (float)109.05;
                        lab.Y = (float)58.1;
                        lab.FontSize = 11;
                        lab.FontColor = "black";
                        lab.FontName = "Portable User Interface";
                        lab.FontStyle = "Normal";
                        lab.FontWeight = "normal";
                        lab.Insert();
                    }


                    lab = new FrmLab();
                    lab.MyPK = "Lab" + DateTime.Now.ToString("yyMMddhhmmss") + WebUser.No + 2;
                    lab.Text = " Sponsor ";
                    lab.FK_MapData = "ND" + int.Parse(this.No + "01");
                    lab.X = (float)106.48;
                    lab.Y = (float)96.08;
                    lab.FontSize = 11;
                    lab.FontColor = "black";
                    lab.FontName = "Portable User Interface";
                    lab.FontStyle = "Normal";
                    lab.FontWeight = "normal";
                    lab.Insert();

                    lab = new FrmLab();
                    lab.MyPK = "Lab" + DateTime.Now.ToString("yyMMddhhmmss") + WebUser.No + 3;
                    lab.Text = " Start Time ";
                    lab.FK_MapData = "ND" + int.Parse(this.No + "01");
                    lab.X = (float)307.64;
                    lab.Y = (float)95.17;

                    lab.FontSize = 11;
                    lab.FontColor = "black";
                    lab.FontName = "Portable User Interface";
                    lab.FontStyle = "Normal";
                    lab.FontWeight = "normal";
                    lab.Insert();

                    lab = new FrmLab();
                    lab.MyPK = "Lab" + DateTime.Now.ToString("yyMMddhhmmss") + WebUser.No + 4;
                    lab.Text = " New node ( Please modify the title )";
                    lab.FK_MapData = "ND" + int.Parse(this.No + "01");

                    lab.X = (float)294.67;
                    lab.Y = (float)8.27;

                    lab.FontSize = 23;
                    lab.FontColor = "Blue";
                    lab.FontName = "Portable User Interface";
                    lab.FontStyle = "Normal";
                    lab.FontWeight = "normal";
                    lab.Insert();

                    lab = new FrmLab();
                    lab.MyPK = "Lab" + DateTime.Now.ToString("yyMMddhhmmss") + WebUser.No + 5;
                    lab.Text = " Explanation : The above content is ccflow Automatically generated , You can modify the / Remove it .@ In order to facilitate your design you can go to http://ccflow.org Official website to download the form template .";
                    lab.Text += "@ Because the current technical issues silverlight The use of special tools are described below :@";
                    lab.Text += "@1, Change control position : ";
                    lab.Text += "@   All controls support  wasd,  As the arrow keys to position control ,  Partial control supports the direction keys . ";
                    lab.Text += "@2,  Increase textbox,  From Table , dropdownlistbox,  Width  shift+ ->  Increase the width of the arrow keys  shift + <-  Reduce the width .";
                    lab.Text += "@3,  Save  win+ s.   Delete  delete.   Copy  ctrl+c    Paste : ctrl+v.";
                    lab.Text += "@4,  Select Support , Batch move ,  Batch zoom font .,  Batch change the line width .";
                    lab.Text += "@5,  Change the length of the line :  Select Line , Dot green dot , Drag it ..";
                    lab.Text += "@6,  Zoom in or out 　label  Font  ,  Select more than one label , Press  A+  Or 　A－　 Push button .";
                    lab.Text += "@7,  Change the color of the line or label ,  Select operation target , Palette point toolbar .";

                    lab.X = (float)168.24;
                    lab.Y = (float)163.7;
                    lab.FK_MapData = "ND" + int.Parse(this.No + "01");
                    lab.FontSize = 11;
                    lab.FontColor = "Red";
                    lab.FontName = "Portable User Interface";
                    lab.FontStyle = "Normal";
                    lab.FontWeight = "normal";
                    lab.Insert();

                    string key = "L" + DateTime.Now.ToString("yyMMddhhmmss") + WebUser.No;
                    FrmLine line = new FrmLine();
                    line.MyPK = key + "_1";
                    line.FK_MapData = "ND" + int.Parse(this.No + "01");
                    line.X1 = (float)281.82;
                    line.Y1 = (float)81.82;
                    line.X2 = (float)281.82;
                    line.Y2 = (float)121.82;
                    line.BorderWidth = (float)2;
                    line.BorderColor = "Black";
                    line.Insert();

                    line.MyPK = key + "_2";
                    line.FK_MapData = "ND" + int.Parse(this.No + "01");
                    line.X1 = (float)360;
                    line.Y1 = (float)80.91;
                    line.X2 = (float)360;
                    line.Y2 = (float)120.91;
                    line.BorderWidth = (float)2;
                    line.BorderColor = "Black";
                    line.Insert();

                    line.MyPK = key + "_3";
                    line.FK_MapData = "ND" + int.Parse(this.No + "01");
                    line.X1 = (float)158.82;
                    line.Y1 = (float)41.82;
                    line.X2 = (float)158.82;
                    line.Y2 = (float)482.73;
                    line.BorderWidth = (float)2;
                    line.BorderColor = "Black";
                    line.Insert();

                    line.MyPK = key + "_4";
                    line.FK_MapData = "ND" + int.Parse(this.No + "01");
                    line.X1 = (float)81.55;
                    line.Y1 = (float)80;
                    line.X2 = (float)718.82;
                    line.Y2 = (float)80;
                    line.BorderWidth = (float)2;
                    line.BorderColor = "Black";
                    line.Insert();


                    line.MyPK = key + "_5";
                    line.FK_MapData = "ND" + int.Parse(this.No + "01");
                    line.X1 = (float)81.82;
                    line.Y1 = (float)40;
                    line.X2 = (float)81.82;
                    line.Y2 = (float)480.91;
                    line.BorderWidth = (float)2;
                    line.BorderColor = "Black";
                    line.Insert();

                    line.MyPK = key + "_6";
                    line.FK_MapData = "ND" + int.Parse(this.No + "01");
                    line.X1 = (float)81.82;
                    line.Y1 = (float)481.82;
                    line.X2 = (float)720;
                    line.Y2 = (float)481.82;
                    line.BorderWidth = (float)2;
                    line.BorderColor = "Black";
                    line.Insert();

                    line.MyPK = key + "_7";
                    line.FK_MapData = "ND" + int.Parse(this.No + "01");
                    line.X1 = (float)83.36;
                    line.Y1 = (float)40.91;
                    line.X2 = (float)717.91;
                    line.Y2 = (float)40.91;
                    line.BorderWidth = (float)2;
                    line.BorderColor = "Black";
                    line.Insert();

                    line.MyPK = key + "_8";
                    line.FK_MapData = "ND" + int.Parse(this.No + "01");
                    line.X1 = (float)83.36;
                    line.Y1 = (float)120.91;
                    line.X2 = (float)717.91;
                    line.Y2 = (float)120.91;
                    line.BorderWidth = (float)2;
                    line.BorderColor = "Black";
                    line.Insert();

                    line.MyPK = key + "_9";
                    line.FK_MapData = "ND" + int.Parse(this.No + "01");
                    line.X1 = (float)719.09;
                    line.Y1 = (float)40;
                    line.X2 = (float)719.09;
                    line.Y2 = (float)482.73;
                    line.BorderWidth = (float)2;
                    line.BorderColor = "Black";
                    line.Insert();
                    #endregion
                }

                // Write permissions .
                WritToGPM(flowSort);

                this.DoCheck_CheckRpt(this.HisNodes);
                Flow.RepareV_FlowData_View();
            }
            catch (Exception ex)
            {
                /// Delete junk data .
                this.DoDelete();

                // An error .
                throw new Exception(" Create a process error :" + ex.Message);
            }


        }
       
        /// <summary>
        ///  Write permissions 
        /// </summary>
        /// <param name="flowSort"></param>
        public void WritToGPM(string flowSort)
        {

            return;

            #region  Write permissions management 
            if (Glo.OSModel == OSModel.BPM)
            {
                string sql = "";

                try
                {
                    sql = "DELETE FROM GPM_Menu WHERE FK_App='" + SystemConfig.SysNo + "' AND Flag='Flow" + this.No + "'";
                    BP.DA.DBAccess.RunSQL(sql);
                }
                catch
                {
                }

                //  Began to organize to initiate the process of data .
                //  The process of obtaining Catalogue .
                sql = "SELECT No FROM GPM_Menu WHERE Flag='FlowSort" + flowSort + "' AND FK_App='" + BP.Sys.SystemConfig.SysNo + "'";
                string parentNoOfMenu = DBAccess.RunSQLReturnStringIsNull(sql, null);
                if (parentNoOfMenu == null)
                    throw new Exception("@ Did not find the process ("+BP.Sys.SystemConfig.SysNo+") Directory GPM System , Please re-create this directory .");

                //  This feature has made the primary key number .
                string treeNo = DBAccess.GenerOID("BP.GPM.Menu").ToString();

                //  Insert the name of the process .
                string url = "/WF/MyFlow.aspx?FK_Flow=" + this.No + "&FK_Node=" + int.Parse(this.No) + "01";

                sql = "INSERT INTO GPM_Menu(No,Name,ParentNo,IsDir,MenuType,FK_App,IsEnable,Flag,Url)";
                sql += " VALUES('{0}','{1}','{2}',{3},{4},'{5}',{6},'{7}','{8}')";
                sql = string.Format(sql, treeNo, this.Name, parentNoOfMenu, 0, 4, SystemConfig.SysNo, 1, "Flow" + this.No, url);
                DBAccess.RunSQL(sql);
            }
            #endregion 
        }
        /// <summary>
        ///  Check Report 
        /// </summary>
        public void CheckRpt()
        {
            this.DoCheck_CheckRpt(this.HisNodes);
        }
        /// <summary>
        ///  Do check before updating 
        /// </summary>
        /// <returns></returns>
        protected override bool beforeUpdate()
        {
            this.Ver = BP.DA.DataType.CurrentDataTimess;  
            Node.CheckFlow(this);
            return base.beforeUpdate();
        }
        
        /// <summary>
        ///  Updated version number 
        /// </summary>
        public static void UpdateVer(string flowNo)
        {
            string sql = "UPDATE WF_Flow SET VER='" + BP.DA.DataType.CurrentDataTimess + "' WHERE No='" + flowNo + "'";
            BP.DA.DBAccess.RunSQL(sql);
        }
        public void DoDelete()
        {
            // Delete process data .
            this.DoDelData();

            string sql = "";
            //sql = " DELETE FROM WF_chofflow WHERE FK_Flow='" + this.No + "'";
            sql += "@ DELETE  FROM WF_GenerWorkerlist WHERE FK_Flow='" + this.No + "'";
            sql += "@ DELETE FROM  WF_GenerWorkFlow WHERE FK_Flow='" + this.No + "'";
            sql += "@ DELETE FROM  WF_Cond WHERE FK_Flow='" + this.No + "'";

            //  Delete posts node .
            sql += "@ DELETE  FROM  WF_NodeStation WHERE FK_Node IN (SELECT NodeID FROM WF_Node WHERE FK_Flow='" + this.No + "')";

            //  Delete direction .
            sql += "@ DELETE FROM  WF_Direction WHERE FK_Flow='" + this.No + "'";

            // Delete nodes binding information .
            sql += "@ DELETE FROM WF_FrmNode  WHERE   FK_Node IN (SELECT NodeID FROM WF_Node WHERE FK_Flow='" + this.No + "')";

            sql += "@ DELETE FROM WF_NodeEmp  WHERE   FK_Node IN (SELECT NodeID FROM WF_Node WHERE FK_Flow='" + this.No + "')";
            sql += "@ DELETE FROM WF_CCEmp WHERE   FK_Node IN (SELECT NodeID FROM WF_Node WHERE FK_Flow='" + this.No + "')";
            sql += "@ DELETE FROM WF_CCDept WHERE   FK_Node IN (SELECT NodeID FROM WF_Node WHERE FK_Flow='" + this.No + "')";
            sql += "@ DELETE FROM WF_CCStation WHERE   FK_Node IN (SELECT NodeID FROM WF_Node WHERE FK_Flow='" + this.No + "')";

            sql += "@ DELETE FROM WF_NodeFlow WHERE   FK_Node IN (SELECT NodeID FROM WF_Node WHERE FK_Flow='" + this.No + "')";
            sql += "@ DELETE FROM WF_NodeReturn WHERE   FK_Node IN (SELECT NodeID FROM WF_Node WHERE FK_Flow='" + this.No + "')";

            sql += "@ DELETE FROM WF_NodeDept WHERE   FK_Node IN (SELECT NodeID FROM WF_Node WHERE FK_Flow='" + this.No + "')";
            sql += "@ DELETE FROM WF_NodeStation WHERE   FK_Node IN (SELECT NodeID FROM WF_Node WHERE FK_Flow='" + this.No + "')";
            sql += "@ DELETE FROM WF_NodeEmp WHERE   FK_Node IN (SELECT NodeID FROM WF_Node WHERE FK_Flow='" + this.No + "')";
            
            sql += "@ DELETE FROM WF_NodeToolbar WHERE   FK_Node IN (SELECT NodeID FROM WF_Node WHERE FK_Flow='" + this.No + "')";
            sql += "@ DELETE FROM WF_SelectAccper WHERE   FK_Node IN (SELECT NodeID FROM WF_Node WHERE FK_Flow='" + this.No + "')";
            sql += "@ DELETE FROM WF_TurnTo WHERE   FK_Node IN (SELECT NodeID FROM WF_Node WHERE FK_Flow='" + this.No + "')";

            
            // Removes a listener .
            sql += "@ DELETE FROM WF_Listen WHERE FK_Node IN (SELECT NodeID FROM WF_Node WHERE FK_Flow='" + this.No + "')";

            //  Delete d2d Data .
            //  sql += "@GO DELETE WF_M2M WHERE FK_Node IN (SELECT NodeID FROM WF_Node WHERE FK_Flow='" + this.No + "')";
            ////  Delete Configuration .
            //sql += "@ DELETE FROM WF_FAppSet WHERE NodeID IN (SELECT NodeID FROM WF_Node WHERE FK_Flow='" + this.No + "')";

            //  Delete Configuration .
            sql += "@ DELETE FROM WF_FlowEmp WHERE FK_Flow='" + this.No + "' ";

            ////  External Program Settings 
            //sql += "@ DELETE FROM WF_FAppSet WHERE  NodeID in (SELECT NodeID FROM WF_Node WHERE FK_Flow='" + this.No + "')";

            // Delete documents .
            sql += "@ DELETE FROM WF_BillTemplate WHERE  NodeID in (SELECT NodeID FROM WF_Node WHERE FK_Flow='" + this.No + "')";

            // Delete access control .
            sql += "@ DELETE FROM Sys_FrmSln WHERE FK_Flow='"+this.No+"'";

            Nodes nds = new Nodes(this.No);
            foreach (Node nd in nds)
            {
                //  Delete all things related to the node .
                sql += "@ DELETE  FROM Sys_MapM2M WHERE FK_MapData='ND" + nd.NodeID+ "'";
                nd.Delete();
            }

            sql += "@ DELETE  FROM WF_Node WHERE FK_Flow='" + this.No + "'";
            sql += "@ DELETE  FROM  WF_LabNote WHERE FK_Flow='" + this.No + "'";

            // Delete group information 
            sql += "@ DELETE FROM Sys_GroupField WHERE EnName NOT IN(SELECT NO FROM Sys_MapData)";

            #region  Delete flow statements , Delete track 
            MapData md = new MapData();
            md.No = "ND" + int.Parse(this.No) + "Rpt";
            md.Delete();

            // Delete View .
            try
            {
                BP.DA.DBAccess.RunSQL("DROP VIEW V_" + this.No);
            }
            catch
            {
            }

            // Delete track .
            try
            {
                BP.DA.DBAccess.RunSQL("DROP TABLE ND" + int.Parse(this.No) + "Track ");
            }
            catch
            {
            }
            #endregion  Delete flow statements , Delete track .

            //  Recorded execution sql scripts.
            BP.DA.DBAccess.RunSQLs(sql);
            this.Delete(); // Remove the need to remove the cache .

            Flow.RepareV_FlowData_View();

            // Delete rights management 
            if (BP.WF.Glo.OSModel == OSModel.BPM)
            {
                try
                {
                    DBAccess.RunSQL("DELETE FROM GPM_Menu WHERE Flag='Flow" + this.No + "' AND FK_App='" + SystemConfig.SysNo + "'");
                }catch
                {
                }
            }
        }
        #endregion
    }

    /// <summary>
    ///  Collection process 
    /// </summary>
    public class Flows : EntitiesNoName
    {
        #region  Inquiry 
        public static void GenerHtmlRpts()
        {
            Flows fls = new Flows();
            fls.RetrieveAll();

            foreach (Flow fl in fls)
            {
                fl.DoCheck();
                fl.GenerFlowXmlTemplete();
            }

            //  Generate index interface 
            string path = SystemConfig.PathOfWorkDir + @"\VisualFlow\DataUser\FlowDesc\";
            string msg = "";
            msg += "<html>";
            msg += "\r\n<title>.net Workflow engine design , Process Template </title>";

            msg += "\r\n<body>";

            msg += "\r\n<h1> Gallop process template network </h1> <br><a href=index.htm > Back Home </a> - <a href='http://ccFlow.org' > Access gallop workflow management system , Workflow engine Official Website </a>  Workflow system construction, please contact :QQ:793719823,Tel:18660153393<hr>";

            foreach (Flow fl in fls)
            {
                msg += "\r\n <h3><b><a href='./" + fl.No + "/index.htm' target=_blank>" + fl.Name + "</a></b> - <a href='" + fl.No + ".gif' target=_blank  >" + fl.Name + " Flow chart </a></h3>";

                msg += "\r\n<UL>";
                Nodes nds = fl.HisNodes;
                foreach (Node nd in nds)
                {
                    msg += "\r\n<li><a href='./" + fl.No + "/" + nd.NodeID + "_" + nd.FlowName + "_" + nd.Name + " Form .doc' target=_blank> Step " + nd.Step + ", - " + nd.Name + " Template </a> -<a href='./" + fl.No + "/" + nd.NodeID + "_" + nd.Name + "_ Form Template .htm' target=_blank>Html</a></li>";
                }
                msg += "\r\n</UL>";
            }
            msg += "\r\n</body>";
            msg += "\r\n</html>";

            try
            {
                string pathDef = SystemConfig.PathOfWorkDir + "\\VisualFlow\\DataUser\\FlowDesc\\" + SystemConfig.CustomerNo + "_index.htm";
                DataType.WriteFile(pathDef, msg);

                pathDef = SystemConfig.PathOfWorkDir + "\\VisualFlow\\DataUser\\FlowDesc\\index.htm";
                DataType.WriteFile(pathDef, msg);
                System.Diagnostics.Process.Start(SystemConfig.PathOfWorkDir + "\\VisualFlow\\DataUser\\FlowDesc\\");
            }
            catch
            {
            }
        }
        #endregion  Inquiry 

        #region  Inquiry 
        /// <summary>
        ///  Check out all of the automated process 
        /// </summary>
        public void RetrieveIsAutoWorkFlow()
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(FlowAttr.FlowType, 1);
            qo.addOrderBy(FlowAttr.No);
            qo.DoQuery();
        }
        /// <summary>
        ///  During check out the entire process in survival within 
        /// </summary>
        /// <param name="flowSort"> Process Category </param>
        /// <param name="IsCountInLifeCycle"> Is not included in the period of survival  true  Check out all of the  </param>
        public void Retrieve(string flowSort)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(FlowAttr.FK_FlowSort, flowSort);
            qo.addOrderBy(FlowAttr.No);
            qo.DoQuery();
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Workflow 
        /// </summary>
        public Flows() { }
        /// <summary>
        ///  Workflow 
        /// </summary>
        /// <param name="fk_sort"></param>
        public Flows(string fk_sort)
        {
            this.Retrieve(FlowAttr.FK_FlowSort, fk_sort);
        }
        #endregion

        #region  Get real 
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new Flow();
            }
        }
        #endregion
    }
}

