using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Text;
using BP.WF.Template;
using BP.Web;
using BP.WF;
using BP.En;

namespace CCFlow.AppDemoLigerUI.Base
{
    public partial class DataService : BasePage
    {
        public string getUTF8ToString(string param)
        {
            return HttpUtility.UrlDecode(Request[param], System.Text.Encoding.UTF8);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (BP.Web.WebUser.No == null)
                return;

            string method = string.Empty;
            // The return value 
            string s_responsetext = string.Empty;
            if (!string.IsNullOrEmpty(Request["method"]))
                method = Request["method"].ToString();

            switch (method)
            {
                case "startflow":// Startup Items 
                    s_responsetext = GetStartFlowEUI();
                    break;
                case "startflowTree":
                    s_responsetext = GetStartFlowTreeEUI();
                    break;
                case "getempworks":// Get Upcoming 
                    s_responsetext = GetEmpWorksEUI();
                    break;
                case "getccflowlist":// Get Cc 
                    s_responsetext = GetCCList();
                    break;
                case "monthplancollect":// Month plan summary 
                    s_responsetext = MonthPlanCollect();
                    break;
                case "workflowmanage":// Business Process Operation 
                    s_responsetext = WorkFlowManage();
                    break;
                case "createmonthplan":// New monthly plan 
                    s_responsetext = MonthPlan_Create();
                    break;
                case "gethunguplist":// Get pending process 
                    s_responsetext = GetHungUpList();
                    break;
                case "Running":// Get in transit 
                    s_responsetext = GetRunning();
                    break;
                case "unsend":// Send revocation 
                    s_responsetext = WorkUnSend();
                    break;
                case "flowsearch":// Inquiry 
                    s_responsetext = FlowSearchMethod();
                    break;
                case "getemps":// Get Contacts 
                    s_responsetext = GetEmpsAndEmp();// Getemps();
                    break;
                case "gettask": // Retrieve approval 
                    s_responsetext = Gettask();
                    break;
                case "keySearch":// Keyword   Inquiry  
                    s_responsetext = KeySearch();
                    break;
                case "getconfigparm":// Get the profile parameters 
                    s_responsetext = GetConfigParm();
                    break;
                case "getempworkcounts":// Get Upcoming , Cc , Suspend number 
                    s_responsetext = GetEmpWorkCounts();
                    break;
                case "historystartflow":// Get Historical launched 
                    s_responsetext = GetHistoryStartFlowEUI();
                    break;
                case "popAlert":// Pop up 
                    s_responsetext = PopAlert();
                    break;
                case "upMsgSta":// Change data   Status 
                    s_responsetext = UpdateMsgSta();
                    break;
                case "getDetailSms":// Details 
                    s_responsetext = GetDetailSms();
                    break;
                case "getmenu":// Gets the menu 
                    s_responsetext = GetMenu();
                    break;
                case "getstoryHistory":// Get historical process initiated 
                    s_responsetext = GetStoryHistory();
                    break;
                case "treeData":// Process tree 
                    s_responsetext = GetTreeData();
                    break;
                case "createemptycase":// Create an empty Process 
                    s_responsetext = CreateEmptyCase();
                    break;
            }
            if (string.IsNullOrEmpty(s_responsetext))
                s_responsetext = "";

            // Assembly ajax String format , Return to the calling client 
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "text/html";
            Response.Expires = 0;
            Response.Write(s_responsetext);
            Response.End();


        }
        /// <summary>
        ///  Create an empty Process 
        /// </summary>
        /// <returns></returns>
        private string CreateEmptyCase()
        {
            string flowId = getUTF8ToString("flowId");
            string title = getUTF8ToString("title");
            string Url = "addform";
            int nodeId = int.Parse(flowId + "01");
            Node wfNode = new Node(nodeId);
            if (wfNode.HisFormType == NodeFormType.SheetTree)
            {
                if (title == "")
                    return "noform";
                long workID = BP.WF.Dev2Interface.Node_CreateStartNodeWork(flowId, null, null, WebUser.No, title);
                Url = "../WF/FlowFormTree/Default.aspx?WorkID=" + workID + "&FK_Flow=" + flowId + "&FK_Node=" + flowId + "01&UserNo=" + WebUser.No + "&FID=0&SID=" + WebUser.SID;
            }
            return Url;
        }
        /// <summary>
        ///  Get the process tree   Data 
        /// </summary>
        /// <returns></returns>
        public string GetTreeData()
        {
            // Loading process privilege 
            if (BP.WF.Glo.OSModel == OSModel.BPM)
            {
                StringBuilder sbContent = new StringBuilder("");
                string sqlSort = "SELECT No,Name,ParentNo,MenuType,Flag FROM V_GPM_EmpMenu WHERE FK_Emp='" + BP.Web.WebUser.No + "' and FK_App = '" + BP.Sys.SystemConfig.SysNo + "'";
                DataTable dtSort = BP.DA.DBAccess.RunSQLReturnTable(sqlSort);
                Flows fls = BP.WF.Dev2Interface.DB_GenerCanStartFlowsOfEntities(BP.Web.WebUser.No);
                if (dtSort != null)
                {
                    int iCount = 0;
                    sbContent.Append("[");
                    foreach (DataRow dr in dtSort.Rows)
                    {
                        iCount++;
                        sbContent.Append("{");
                        sbContent.AppendFormat("No:\"{0}\",", dr["No"].ToString());
                        sbContent.AppendFormat("Name:\"{0}\",", dr["Name"].ToString());
                        sbContent.AppendFormat("ParentNo:\"{0}\",", dr["ParentNo"].ToString());
                        sbContent.AppendFormat("MenuType:\"{0}\",", dr["MenuType"].ToString());
                        sbContent.AppendFormat("Flag:\"{0}\",", dr["Flag"].ToString());
                        string fk_no = dr["Flag"].ToString().Replace("Flow", "");
                        if (fls.Contains(fk_no) == false)  // Determine whether the user has permission to initiate 
                        {
                            sbContent.Append("IsStart:\"0\"");
                        }
                        else
                        {
                            sbContent.Append("IsStart:\"1\"");
                        }
                        if (iCount == dtSort.Rows.Count)
                        {
                            sbContent.Append("}");
                        }
                        else
                        {
                            sbContent.Append("},");
                        }

                    }
                    sbContent.Append("]");
                }
                return sbContent.ToString();
            }
            return "";
        }
        /// <summary>
        ///  Get historical process information 
        /// </summary>
        public string GetStoryHistory()
        {
            string fk_flow = "";
            if (!string.IsNullOrEmpty(Request["FK_Flow"]))
            {
                fk_flow = Request["FK_Flow"].ToString();
            }

            Flow fl = new Flow(fk_flow);
            string sql = "";

            sql = "SELECT " + fl.HistoryFields + ",OID,FID FROM " + fl.PTable + " WHERE FlowStarter='" + BP.Web.WebUser.No + "' AND WFState!='" + (int)WFState.Blank + "'" + "    ORDER by OID DESC";

            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            return CommonDbOperator.GetJsonFromTable(dt);
        }
        /// <summary>
        ///  Get the configuration parameters 
        /// </summary>
        /// <returns></returns>
        private string GetConfigParm()
        {
            StringBuilder returnVal = new StringBuilder();
            returnVal.Append("{config:[{");
            returnVal.Append(string.Format("IsWinOpenStartWork:'{0}',", Glo.IsWinOpenStartWork));
            returnVal.Append(string.Format("IsWinOpenEmpWorks:'{0}'", Glo.IsWinOpenEmpWorks));
            returnVal.Append("}]}");
            return returnVal.ToString();
        }

        public string GetEasyUIJson(DataTable table)
        {

            string json = "{\"total\":" + table.Rows.Count + ",\"rows\":" + Newtonsoft.Json.JsonConvert.SerializeObject(table) + "}";

            return json;
        }
        /// <summary>
        ///  Start the process to obtain entry 
        /// </summary>
        /// <returns></returns>
        private string GetSartFlow()
        {
            DataTable dt = BP.WF.Dev2Interface.DB_GenerCanStartFlowsOfDataTable(WebUser.No);
            return CommonDbOperator.GetJsonFromTable(dt);
        }
        /// <summary>
        ///  Get To Do List 
        /// </summary>
        /// <returns></returns>
        private string GetEmpWorks()
        {
            DataTable dt = BP.WF.Dev2Interface.DB_GenerEmpWorksOfDataTable();
            return CommonDbOperator.GetJsonFromTable(dt);
        }
        /// <summary>
        ///  Get To Do List 
        /// </summary>
        /// <returns></returns>
        private string GetEmpWorksEUI()
        {
            DataTable dt = BP.WF.Dev2Interface.DB_GenerEmpWorksOfDataTable();
            return GetEasyUIJson(dt);
        }
        private string GetStartFlowEUI()
        {
            DataTable dt = BP.WF.Dev2Interface.DB_GenerCanStartFlowsOfDataTable(WebUser.No);
            return GetEasyUIJson(dt);
        }

        private string GetStartFlowTreeEUI()
        {
            DataTable dt = BP.WF.Dev2Interface.DB_GenerCanStartFlowsTree(WebUser.No);

            return GetEasyUIJson(dt);
        }
        /// <summary>
        ///  Get Historical launched 
        /// </summary>
        /// <returns></returns>
        private string GetHistoryStartFlowEUI()
        {
            string fk_flow = getUTF8ToString("FK_Flow");
            Flow startFlow = new Flow(fk_flow);
            string sql = "SELECT * FROM " + startFlow.PTable + " WHERE FlowStarter='" + WebUser.No + "'  and WFState not in (" + (int)WFState.Blank + "," + (int)WFState.Draft + ")";
            DataTable dt = startFlow.RunSQLReturnTable(sql);
            return GetEasyUIJson(dt);
        }
        /// <summary>
        ///  Get CC list 
        /// </summary>
        /// <returns></returns>
        private string GetCCList()
        {
            DataTable dt = null;
            string strCCSta = getUTF8ToString("ccSta");
            // Whole 
            if (strCCSta == "all")
            {
                dt = BP.WF.Dev2Interface.DB_CCList(WebUser.No);
            }
            // Unread 
            if (strCCSta == "unread")
            {
                dt = BP.WF.Dev2Interface.DB_CCList_UnRead(WebUser.No);
            }
            // Read 
            if (strCCSta == "isread")
            {
                dt = BP.WF.Dev2Interface.DB_CCList_Read(WebUser.No);
            }
            // Delete 
            if (strCCSta == "delete")
            {
                dt = BP.WF.Dev2Interface.DB_CCList_Delete(WebUser.No);
            }
            return CommonDbOperator.GetJsonFromTable(dt);
        }
        #region  Program Management 
        /// <summary>
        ///  Month plan summary 
        /// </summary>
        /// <returns></returns>
        private string MonthPlanCollect()
        {
            string sql = "SELECT a.* ,b.FK_Flow,b.FK_Node,b.FlowName,b.NodeName,b.IsRead,b.Starter,b.ADT,b.SDT,b.WorkID FROM ND22Rpt"
                        + " a , WF_EmpWorks b WHERE a.OID=B.WorkID AND b.WFState not in (7)"
                        + " AND b.FK_Emp='" + WebUser.No + "'"
                        + " ORDER BY ADT DESC";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);

            return CommonDbOperator.GetJsonFromTable(dt);
        }
        /// <summary>
        ///  New monthly plan 
        /// </summary>
        /// <returns></returns>
        private string MonthPlan_Create()
        {
            int nextNodeId = 2201;
            string strStation = "";
            BP.Port.Stations userStations = WebUser.HisStations;
            foreach (BP.Port.Station sta in userStations)
            {
                strStation += "," + sta.Name + ",";
            }
            if (strStation.Contains(", Department planner post ,"))
            {
                nextNodeId = 2211;
            }
            if (strStation.Contains(" Director "))
            {
                nextNodeId = 2203;
            }
            long workId = BP.WF.Dev2Interface.Node_CreateStartNodeWork("022", null, null, WebUser.No, WebUser.FK_DeptName + "[" + WebUser.Name + "]" + DateTime.Now.ToString("yyyy-MM") + " Sector plans ");
            if (nextNodeId != 2201)
                BP.WF.Dev2Interface.Node_SendWork("022", workId, nextNodeId, WebUser.No);
            return "success";
        }
        /// <summary>
        ///  Business Process Operation 
        /// </summary>
        /// <returns></returns>
        private string WorkFlowManage()
        {
            string doWhat = getUTF8ToString("doWhat");
            string flowIdAndWorkId = getUTF8ToString("flowIdAndWorkId");
            string[] array = flowIdAndWorkId.Split('^');
            string Msg = "";

            // Performing transmission 
            if (doWhat == "send")
            {
                foreach (string item in array)
                {
                    string[] item_c = item.Split(',');
                    Int64 workid = Int64.Parse(item_c[1].ToString());
                    SendReturnObjs objSend = null;
                    objSend = BP.WF.Dev2Interface.Node_SendWork(item_c[0].ToString(), workid);
                    Msg += objSend.ToMsgOfHtml();
                    Msg += "<hr>";
                }
            }
            // Delete 
            if (doWhat == "delete")
            {
                foreach (string item in array)
                {
                    string[] item_c = item.Split(',');
                    Int64 workid = Int64.Parse(item_c[1].ToString());
                    string mes = BP.WF.Dev2Interface.Flow_DoDeleteFlowByFlag(item_c[0].ToString(), workid, " Bulk Delete ", true);
                    Msg += mes;
                    Msg += "<hr>";
                }
            }

            return Msg;
        }
        #endregion

        /// <summary>
        ///  Get pending process 
        /// </summary>
        /// <returns></returns>
        private string GetHungUpList()
        {
            DataTable dt = BP.WF.Dev2Interface.DB_GenerHungUpList();
            return CommonDbOperator.GetJsonFromTable(dt);
        }
        /// <summary>
        ///  Get the list of passers 
        /// </summary>
        /// <returns></returns>
        public string GetRunning()
        {
            DataTable dt = BP.WF.Dev2Interface.DB_GenerRuning();
            // Accepted Sort by 
            dt.DefaultView.Sort = "RDT DESC";
            //return CommonDbOperator.GetJsonFromTable(dt.DefaultView.ToTable());
            return GetEasyUIJson(dt.DefaultView.ToTable());
        }
        /// <summary>
        ///  Send revocation 
        /// </summary>
        /// <returns></returns>
        public string WorkUnSend()
        {
            try
            {
                string FK_Flow = getUTF8ToString("FK_Flow");
                string WorkID = getUTF8ToString("WorkID");
                string str1 = BP.WF.Dev2Interface.Flow_DoUnSend(FK_Flow, Int64.Parse(WorkID));
                return "{message:' The successful implementation of revocation , Go to to-do list for processing .'}";
            }
            catch (Exception ex)
            {
                return "{message:' Undo failure , Failure information " + ex.Message + "'}";
            }
        }
        /// <summary>
        ///  Workflow inquiry 
        /// </summary>
        /// <returns></returns>
        private string FlowSearchMethod()
        {
            FlowSorts fss = new FlowSorts();
            fss.RetrieveAll();
            //Flows fls = new Flows();
            //fls.RetrieveAll();

            //ating #65 2015-03-13
            string deptSql =string.Format( @"select fl.No, fl.Name, fl.NumOfBill, fl.FK_FlowSort from wf_flow fl where fl.no in(
                                 select distinct FK_Flow from(
                                        SELECT FK_Flow FROM WF_Node 
                                        WHERE 
                                        NodePosType=0 AND ( WhoExeIt=0 OR WhoExeIt=2 ) 
                                        AND NodeID IN 
                                           ( SELECT FK_Node FROM WF_NodeStation WHERE FK_Station IN (SELECT FK_Station FROM Port_EmpStation WHERE FK_Emp='{0}'))  
                                        UNION  
                                        SELECT FK_Flow FROM WF_Node 
                                        WHERE NodePosType=0 AND ( WhoExeIt=0 OR WhoExeIt=2 ) 
                                        AND NodeID IN ( SELECT FK_Node FROM WF_NodeEmp WHERE FK_Emp='{0}' )  
                                        UNION  
                                        SELECT FK_Flow FROM WF_Node 
                                        WHERE NodePosType=0 
                                        AND ( WhoExeIt=0 OR WhoExeIt=2 ) 
                                        AND NodeID IN ( SELECT FK_Node FROM WF_NodeDept WHERE FK_Dept IN(SELECT FK_Dept FROM Port_Emp WHERE No='{0}' UNION SELECT FK_DEPT FROM Port_EmpDept WHERE FK_Emp='{0}') ) 
                                )
                            )", WebUser.No);
            DataTable fls = BP.DA.DBAccess.RunSQLReturnTable(deptSql);

            StringBuilder appFlow = new StringBuilder();
            appFlow.Append("{");
            appFlow.Append("\"rows\":[");

            foreach (FlowSort fs in fss)
            {
                if (appFlow.Length == 9) { appFlow.Append("{"); } else { appFlow.Append(",{"); }
                if (fs.ParentNo + "" == "0")
                {
                    appFlow.Append(string.Format("\"No\":\"{0}\",\"Name\":\"{1}\",\"NumOfBill\":\"{2}\",\"_parentId\":null,\"state\":\"closed\",\"Element\":\"sort\"", fs.No, fs.Name, "0"));
                }
                else
                {
                    appFlow.Append(string.Format("\"No\":\"{0}\",\"Name\":\"{1}\",\"NumOfBill\":\"{2}\",\"_parentId\":\"{3}\",\"state\":\"closed\",\"Element\":\"sort\"", fs.No, fs.Name, "0", fs.ParentNo));
                }
                appFlow.Append("}");
            }

            foreach (FlowSort fs in fss)
            {
                foreach (DataRow fl in fls.Rows)
                {
                    if (fl["FK_FLOWSORT"].ToString() != fs.No.ToString())
                        continue;

                    if (appFlow.Length == 9) { appFlow.Append("{"); } else { appFlow.Append(",{"); }

                    appFlow.Append(string.Format("\"No\":\"{0}\",\"Name\":\"{1}\",\"NumOfBill\":\"{2}\",\"_parentId\":\"{3}\",\"Element\":\"flow\"", fl["NO"], fl["NAME"], fl["NUMOFBILL"], fl["FK_FLOWSORT"]));
                    appFlow.Append("}");
                }
            }
            appFlow.Append("]");
            appFlow.Append(",\"total\":" + fls.Rows.Count + fss.Count + "");
            appFlow.Append("}");
            return appFlow.ToString();
        }

        /// <summary>
        ///  Get departments 
        /// </summary>
        public string GetEmpsAndEmp()
        {
            StringBuilder sbJson = new StringBuilder("{Rows:[");
            string deptSql = "SELECT No,Name as DeptName,Leader,ParentNo  FROM  Port_Dept";

            DataTable deptDT = BP.DA.DBAccess.RunSQLReturnTable(deptSql);
            if (deptDT != null)
            {
                sbJson.Append("{");
                sbJson.AppendFormat("No:\"{0}\",", string.IsNullOrEmpty(deptDT.Rows[0]["No"].ToString()) ? "" : deptDT.Rows[0]["No"].ToString());
                sbJson.AppendFormat("Name:\"{0}\",", "");
                sbJson.AppendFormat("DeptName:\"{0}\",", string.IsNullOrEmpty(deptDT.Rows[0]["DeptName"].ToString()) ? "" : deptDT.Rows[0]["DeptName"].ToString());
                sbJson.AppendFormat("DutyName:\"{0}\",", "");
                sbJson.AppendFormat("Leader:\"{0}\",", string.IsNullOrEmpty(deptDT.Rows[0]["Leader"].ToString()) ? "" : deptDT.Rows[0]["Leader"].ToString());
                sbJson.AppendFormat("Tel:\"{0}\",", "");
                sbJson.AppendFormat("Email:\"{0}\",", "");
                sbJson.AppendFormat("QianMing:\"{0}\",", "");

                // Child 
                sbJson.Append("children: [");
                // Staff 
                int iEmp = 0;
                string strEmp = Getemps(deptDT.Rows[0]["No"].ToString());
                sbJson.Append(strEmp);
                // Child department 
                if (strEmp.Length < 2)
                {
                    iEmp = 0;
                }
                else
                {
                    iEmp = 1;
                }
                Getemps2(deptDT, deptDT.Select("ParentNo=" + deptDT.Rows[0]["No"].ToString()), sbJson, iEmp);
                sbJson.Append("]}");
            }
            sbJson.Append("]}");
            return sbJson.ToString();
        }

        /// <summary>
        ///  Get subsectors 
        /// </summary>
        /// <returns></returns>
        public void Getemps2(DataTable dt, DataRow[] drChilds, StringBuilder sbJson, int iEmp)
        {
            if (dt != null && drChilds != null)
            {
                for (int i = 0; i < drChilds.Length; i++)
                {
                    if (i == 0)
                    {
                        if (iEmp == 0)
                        {
                            sbJson.Append("{");
                        }
                        else
                        {
                            sbJson.Append(",{");
                        }
                    }
                    else { sbJson.Append(",{"); }
                    sbJson.AppendFormat("No:\"{0}\",", string.IsNullOrEmpty(drChilds[i]["No"].ToString()) ? "" : drChilds[i]["No"].ToString());
                    sbJson.AppendFormat("Name:\"{0}\",", "");
                    sbJson.AppendFormat("DeptName:\"{0}\",", string.IsNullOrEmpty(drChilds[i]["DeptName"].ToString()) ? "" : drChilds[i]["DeptName"].ToString());
                    sbJson.AppendFormat("DutyName:\"{0}\",", "");
                    sbJson.AppendFormat("Leader:\"{0}\",", string.IsNullOrEmpty(drChilds[i]["Leader"].ToString()) ? "" : drChilds[i]["Leader"].ToString());
                    sbJson.AppendFormat("Tel:\"{0}\",", "");
                    sbJson.AppendFormat("Email:\"{0}\",", "");
                    sbJson.AppendFormat("QianMing:\"{0}\",", "");

                    // Child 
                    sbJson.Append("children: [");
                    // Staff 
                    int iEmp2 = 0;
                    string strEmp = Getemps(drChilds[i]["No"].ToString());
                    sbJson.Append(strEmp);
                    // Child department 
                    if (strEmp.Length < 2)
                    {
                        iEmp2 = 0;
                    }
                    else
                    {
                        iEmp2 = 1;
                    }
                    Getemps2(dt, dt.Select("ParentNo='" + drChilds[i]["No"].ToString() + "'"), sbJson, iEmp2);
                    sbJson.Append("]}");
                }
            }
        }

        /// <summary>
        ///  Get   Contacts , Staff 
        /// /// </summary>
        /// <returns></returns>
        public string Getemps(string DeptNo)
        {
            StringBuilder sbJson = new StringBuilder("");
            string sql = "select a.No,  a.Name"
                + @",b.Name as DeptName"
               + @",c.Name as DutyName"
               + @",a.Leader"
               + @",a.Tel"
               + @",a.Email "
               + @",'/DataUser/Siganture/'+a.No +'.jpg' as QianMing "
               + @",convert(int,b.No) as dtNo "
               + @"from Port_Emp a, Port_Dept b,port_duty c "
               + @"where a.FK_Dept=b.No and a.FK_Duty=c.No  and convert(int,b.No)=" + int.Parse(DeptNo)
               + @"order by dtNo ";

            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        sbJson.Append("{");
                    }
                    else
                    {
                        sbJson.Append(",{");
                    }
                    sbJson.AppendFormat("No:\"{0}\",", string.IsNullOrEmpty(dt.Rows[i]["No"].ToString()) ? "" : dt.Rows[i]["No"].ToString());
                    sbJson.AppendFormat("Name:\"{0}\",", string.IsNullOrEmpty(dt.Rows[i]["Name"].ToString()) ? "" : dt.Rows[i]["Name"].ToString());
                    sbJson.AppendFormat("DeptName:\"{0}\",", string.IsNullOrEmpty(dt.Rows[i]["DeptName"].ToString()) ? "" : dt.Rows[i]["DeptName"].ToString());
                    sbJson.AppendFormat("DutyName:\"{0}\",", string.IsNullOrEmpty(dt.Rows[i]["DutyName"].ToString()) ? "" : dt.Rows[i]["DutyName"].ToString());
                    sbJson.AppendFormat("Leader:\"{0}\",", string.IsNullOrEmpty(dt.Rows[i]["Leader"].ToString()) ? "" : dt.Rows[i]["Leader"].ToString());
                    sbJson.AppendFormat("Tel:\"{0}\",", string.IsNullOrEmpty(dt.Rows[i]["Tel"].ToString()) ? "" : dt.Rows[i]["Tel"].ToString());
                    sbJson.AppendFormat("Email:\"{0}\",", string.IsNullOrEmpty(dt.Rows[i]["Email"].ToString()) ? "" : dt.Rows[i]["Email"].ToString());
                    sbJson.AppendFormat("QianMing:\"{0}\"", string.IsNullOrEmpty(dt.Rows[i]["QianMing"].ToString()) ? "" : dt.Rows[i]["QianMing"].ToString());

                    sbJson.Append("}");
                }
            }
            return sbJson.ToString();
        }

        /// <summary>
        ///  Retrieve   Approval 
        /// </summary>
        /// <returns></returns>
        public string Gettask()
        {
            Flows fls = new Flows();
            BP.En.QueryObject qo = new BP.En.QueryObject(fls);
            qo.addOrderBy(FlowAttr.FK_FlowSort);
            qo.DoQuery();

            // The collection   Converted to datatable
            DataTable dt = new DataTable("Flows");

            DataColumn dc0 = new DataColumn("No", Type.GetType("System.String"));// Serial number 
            DataColumn dc1 = new DataColumn("FK_FlowSortText", Type.GetType("System.String")); // Process Category 
            DataColumn dc2 = new DataColumn("Name", Type.GetType("System.String"));// Name 
            DataColumn dc3 = new DataColumn("FlowImage", Type.GetType("System.String")); // Flow chart 
            DataColumn dc4 = new DataColumn("Note", Type.GetType("System.String"));// Description 

            dt.Columns.Add(dc0);
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
            dt.Columns.Add(dc4);

            foreach (Flow fl in fls)
            {
                DataRow dr = dt.NewRow();

                dr["No"] = fl.No;
                dr["FK_FlowSortText"] = fl.FK_FlowSortText;
                dr["Name"] = fl.Name;
                dr["FlowImage"] = fl.No;
                dr["Note"] = fl.Note;

                dt.Rows.Add(dr);
            }
            //将dt以json Format   Return 
            return CommonDbOperator.GetJsonFromTable(dt);
        }
        /// <summary>
        ///  Keyword Search 
        /// </summary>
        /// <returns></returns>
        private string KeySearch()
        {
            string queryType = getUTF8ToString("queryType");
            string content = getUTF8ToString("content");
            string ckbQueryOwner = getUTF8ToString("checkBox");
            if (queryType == "workid")
            {
                return KeySearchByWorkID(content, ckbQueryOwner);
            }
            else if (queryType == "title")
            {
                return KeySearchByTitle(content, ckbQueryOwner);
            }
            else if (queryType == "all")
            {
                return KeySearchByAll(content, ckbQueryOwner);
            }
            return "[]";
        }
        /// <summary>
        ///  Keyword   Inquiry    By Job ID查
        /// </summary>
        /// <returns></returns>
        public string KeySearchByWorkID(string content, string ck)
        {
            int workid = 0;
            string sql = "";
            try
            {
                workid = int.Parse(content);
            }
            catch
            {
                //this.Alert(" You entered is not a WorkID" + content);
                //return;
            }
            if (ck.ToUpper() == "TRUE")
                sql = "SELECT A.*,B.Name as FlowName FROM V_FlowData a,WF_Flow b  WHERE A.FK_Flow=B.No AND A.OID=" + workid + " AND FlowEmps LIKE '%@" + WebUser.No + ",%'";
            else
                sql = "SELECT A.*,B.Name as FlowName FROM V_FlowData a,WF_Flow b  WHERE A.FK_Flow=B.No AND A.OID=" + workid;

            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            return CommonDbOperator.GetJsonFromTable(dt);
        }
        /// <summary>
        ///  Keyword   Inquiry    Title field investigation process by keyword 
        /// </summary>
        /// <returns></returns>
        public string KeySearchByTitle(string content, string ck)
        {
            string sql = "";
            if (ck.ToUpper() == "TRUE")
                sql = "SELECT A.*,B.Name as FlowName FROM V_FlowData a,WF_Flow b  WHERE A.FK_Flow=B.No AND a.Title LIKE '%" + content + "%' AND FlowEmps LIKE '%@" + WebUser.No + ",%'";
            else
                sql = "SELECT A.*,B.Name as FlowName FROM V_FlowData a,WF_Flow b  WHERE A.FK_Flow=B.No AND a.Title LIKE '%" + content + "%'";

            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            return CommonDbOperator.GetJsonFromTable(dt);

        }
        /// <summary>
        ///  Keyword   Inquiry   Check all fields keywords 
        /// </summary>
        /// <returns></returns>
        public string KeySearchByAll(string content, string ck)
        {
            return "[]";
        }
        /// <summary>
        ///  Get Upcoming , Cc , Suspend number 
        /// </summary>
        /// <returns></returns>
        private string GetEmpWorkCounts()
        {
            // Note naming variables , by peng.
            return "{message:{empwork:'" + EmpWorks + "',ccnum:'" + CCNum + "',hungupnum:'" + HungUpNum + "',TaskPoolNum:'" + TaskPoolNum + "'}}";
            //  return "{message:{empwork:'" + EmpWorks + "',ccnum:'" + CCNum + "',hungupnum:'" + HungUpNum + "',TaskPoolNum:'" + TaskPoolNum + "'}}";
        }
        /// <summary>
        ///  Returns the number of to-do items 
        /// </summary>
        /// <returns></returns>
        private int EmpWorks
        {
            get
            {
                DataTable dt = BP.WF.Dev2Interface.DB_GenerEmpWorksOfDataTable();
                return dt.Rows.Count;
                //string sql = "SELECT COUNT(*) AS Num FROM WF_EmpWorks WHERE FK_Emp='" + BP.Web.WebUser.No + "' AND WFState=2 AND TaskSta=0";
                //return BP.DA.DBAccess.RunSQLReturnValInt(sql);
            }
        }
        /// <summary>
        ///  Returns the number of to-do items 
        /// </summary>
        /// <returns></returns>
        private int TaskPoolNum
        {
            get
            {
                if (Glo.IsEnableTaskPool == false)
                    return 0;

                string sql = "SELECT COUNT(*) AS Num FROM WF_EmpWorks WHERE FK_Emp='" + BP.Web.WebUser.No + "' AND WFState=2 AND TaskSta=1";
                return BP.DA.DBAccess.RunSQLReturnValInt(sql);
            }
        }
        /// <summary>
        ///  Returns the number of CC members 
        /// </summary>
        /// <returns></returns>
        private int CCNum
        {
            get
            {
                string sql = "SELECT COUNT(*) AS Num FROM WF_CCList WHERE CCTo='" + BP.Web.WebUser.No + "' AND Sta=0";
                return BP.DA.DBAccess.RunSQLReturnValInt(sql);
            }
        }
        /// <summary>
        ///  Returns the number of processes to hang 
        /// </summary>
        private int HungUpNum
        {
            get
            {
                string sql = "SELECT COUNT(*) AS Num FROM WF_HungUp WHERE Rec='" + BP.Web.WebUser.No + "'";
                return BP.DA.DBAccess.RunSQLReturnValInt(sql);
            }
        }

        /// <summary>
        ///  Get Historical launched 
        /// </summary>
        /// <returns></returns>
        private string GetHistoryStartFlow()
        {
            string fk_flow = getUTF8ToString("FK_FLOW");
            Flow startFlow = new Flow(fk_flow);
            string sql = "SELECT * FROM " + startFlow.PTable + " WHERE FlowStarter='" + WebUser.No + "'";
            DataTable dt = startFlow.RunSQLReturnTable(sql);
            return CommonDbOperator.GetJsonFromTable(dt);
        }
        /// <summary>
        ///  Pop up   Window   2013.05.23 H
        /// </summary>
        /// <returns></returns>
        public string PopAlert()
        {
            // IsRead = 0  Unread  
            string type = getUTF8ToString("type");

            DataTable dt = BP.WF.Dev2Interface.DB_GenerPopAlert(type);
            if (dt.Rows.Count >= 1)
            {
                return CommonDbOperator.GetJsonFromTable(GetNewDataTable(dt));
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        ///  Change data state  2013.05.23 H
        /// </summary> 
        /// <returns></returns>
        public string UpdateMsgSta()
        {
            string myPK = getUTF8ToString("myPK");

            DataTable dt = BP.WF.Dev2Interface.DB_GenerUpdateMsgSta(myPK);
            return CommonDbOperator.GetJsonFromTable(dt);
        }
        /// <summary>
        ///   Detailed   Information   2013.05.23 H
        /// </summary>
        /// <returns></returns>
        public string GetDetailSms()
        {
            string myPK = getUTF8ToString("myPK");
            string sql = "";

            sql = "SELECT * FROM Sys_SMS WHERE MyPK='" + myPK + "'";

            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            return CommonDbOperator.GetJsonFromTable(dt);
        }
        /// <summary>
        ///  Integrate  datatable
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable GetNewDataTable(DataTable dt)
        {
            DataTable Newdt = new DataTable("DT");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                Newdt.Columns.Add(dt.Columns[i].ColumnName);
            }
            Newdt.Columns.Add("GroupBy");
            foreach (DataRow dr in dt.Rows)
            {
                DataRow Newdr = Newdt.NewRow();
                for (int a = 0; a < dt.Columns.Count; a++)
                {
                    Newdr[dt.Columns[a].ColumnName] = dr[dt.Columns[a].ColumnName].ToString();
                }
                //Newdr["GroupBy"] = GetGropuBy(dr["RDT"].ToString());  IsToday
                Newdr["GroupBy"] = JugeDay(DateTime.Parse(dr["RDT"].ToString()));
                Newdt.Rows.Add(Newdr);
            }
            return Newdt;
        }

        /// <summary>
        ///  Determine the date chosen 
        /// </summary>
        /// <param name="someDate"></param>
        /// <returns></returns>
        public static string JugeDay(DateTime someDate)
        {
            DateTime dt = DateTime.Now;
            int Nowhour = dt.Hour;

            TimeSpan ts = dt - someDate;
            int hours = ts.Days == 0 ? ts.Hours : ts.Days * 24 + ts.Hours;

            if (hours <= Nowhour) // Today 
            {
                return " Today ";
            }
            else if ((hours > Nowhour) && (hours <= (24 + Nowhour)))// Yesterday 
            {
                return " Yesterday ";
            }
            else if ((hours > (24 + Nowhour)) && (hours <= (24 * 2 + Nowhour)))
            {
                switch (Convert.ToInt32(someDate.DayOfWeek))
                {
                    case 1:
                        return " Monday ";
                    case 2:
                        return " Tuesday ";
                    case 3:
                        return " Wednesday ";
                    case 4:
                        return " Thursday ";
                    case 5:
                        return " Friday ";
                    case 6:
                        return " On Saturday ";
                    default:
                        return " On Sunday ";
                }

            }
            else if ((hours > (24 * 2 + Nowhour)) && (hours <= (Convert.ToInt32(dt.DayOfWeek) * 24 + Nowhour)))
            {
                return " Within the week ";
            }
            else if ((hours > (Convert.ToInt32(dt.DayOfWeek) * 24 + Nowhour)) && (hours <= ((dt.Day + 7) * 24 + Nowhour)))
            {
                return " Within last week ";
            }
            else if ((hours > (Convert.ToInt32(dt.DayOfWeek) * 24 + Nowhour)) && (hours <= (dt.Day * 24 + Nowhour)))
            {
                return " Within a month ";
            }
            else if ((someDate.Year == dt.Year) && (someDate.Month == dt.Month - 1))
            {
                return " Within the last month ";
            }
            else
            {
                return " Earlier ";
            }
        }

        /// <summary>
        ///  Gets the menu  20130723 H
        /// </summary>
        /// <returns></returns>
        private string GetMenu()
        {
            // Submenu 
            string sqlSort = "";
            StringBuilder sbXML = new StringBuilder();
            sbXML.Append("[");
            if (BP.Web.WebUser.No == "admin")
            {
                sqlSort = "select * from wf_flowsort ";
            }
            else
            {
                sqlSort = "select * from V_FlowSortEmp where FK_Emp ='" + BP.Web.WebUser.No + "'";
            }

            DataTable dtSort = BP.DA.DBAccess.RunSQLReturnTable(sqlSort);
            if (dtSort != null)
            {
                int iCount = 0;
                foreach (DataRow dr in dtSort.Rows)
                {
                    if (iCount > 0) sbXML.Append(",");
                    sbXML.Append("{");
                    sbXML.AppendFormat("No:\"{0}\"", dr["No"].ToString());
                    sbXML.AppendFormat(",Name:\"{0}\"", dr["Name"].ToString());
                    sbXML.AppendFormat(",ParentNo:\"{0}\"", dr["ParentNo"].ToString());
                    sbXML.Append("}");
                    string sqlChild = "select  No,Name,Fk_FlowSort as ParentNo  from wf_flow where   FK_FlowSort = '" + dr["No"].ToString() + "'";
                    DataTable dtChild = BP.DA.DBAccess.RunSQLReturnTable(sqlChild);
                    if (dtChild != null)
                    {
                        foreach (DataRow drChild in dtChild.Rows)
                        {
                            sbXML.Append(",");
                            sbXML.Append("{");
                            sbXML.AppendFormat("No:\"{0}\"", drChild["No"].ToString());
                            sbXML.AppendFormat(",Name:\"{0}\"", drChild["Name"].ToString());
                            sbXML.AppendFormat(",ParentNo:\"{0}\"", drChild["ParentNo"].ToString());
                            sbXML.Append("}");
                        }
                    }

                    iCount++;
                }
            }
            sbXML.Append("]");
            return sbXML.ToString();
        }
    }
}