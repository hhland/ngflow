using System;
using System.Data;
using System.Collections;
using BP.DA;
using BP.Port;
using BP.En;
using BP.Web;
using BP.Sys;
using BP.WF.Data;
using BP.WF.Data;

namespace BP.WF.Template.Ext
{
    /// <summary>
    ///  Process 
    /// </summary>
    public class FlowSheet : EntityNoName
    {
        #region  Property .
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
        /// <summary>
        ///  Designers number 
        /// </summary>
        public string DesignerNo
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
        public string DesignerName
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
        ///  Number generation format 
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
        #endregion  Property .

        #region  Constructor 
        /// <summary>
        /// UI Access control interface 
        /// </summary>
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                if (BP.Web.WebUser.No == "admin" || this.DesignerNo == WebUser.No)
                {
                    uac.IsUpdate = true;
                }
                return uac;
            }
        }
        /// <summary>
        ///  Process 
        /// </summary>
        public FlowSheet()
        {
        }
        /// <summary>
        ///  Process 
        /// </summary>
        /// <param name="_No"> Serial number </param>
        public FlowSheet(string _No)
        {
            this.No = _No;
            if (SystemConfig.IsDebug)
            {
                int i = this.RetrieveFromDBSources();
                if (i == 0)
                    throw new Exception(" Process number does not exist ");
            }
            else
            {
                this.Retrieve();
            }
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

                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.EnDesc = " Process ";
                map.CodeStruct = "3";

                #region  Basic properties .
                map.AddTBStringPK(FlowAttr.No, null, " Serial number ", true, true, 1, 10, 3);
                map.SetHelperAlert(FlowAttr.No, " Process numbered 001 Begin ,string Type , Node number is int Type , Plus a two-digit number is the process ID . \t\n Such as : Process number is 001, Node number is :101,102....."); // Use alert Way to display help information .

                map.AddDDLEntities(FlowAttr.FK_FlowSort, "01", " Process Category ",new FlowSorts(), true);
                map.AddTBString(FlowAttr.Name, null, " Name ", true, false, 0, 200, 10, true);

                // add 2013-02-14  Mark this process is uniquely determined 
                map.AddTBString(FlowAttr.FlowMark, null, " Process tag ", true, false, 0, 150, 10);
                map.AddTBString(FlowAttr.FlowEventEntity, null, " Process events entity ", true, true, 0, 150, 10);
                map.SetHelperBaidu(FlowAttr.FlowMark, "ccflow  Process tag ");

                // add 2013-02-05.
                map.AddTBString(FlowAttr.TitleRole, null, " Title generation rules ", true, false, 0, 150, 10, true);
                map.SetHelperBaidu(FlowAttr.TitleRole, "ccflow  Title generation rules ");

                //add  2013-08-30.
                map.AddTBString(FlowAttr.BillNoFormat, null, " Document Numbering Format ", true, false, 0, 200, 10, false);
                map.SetHelperBaidu(FlowAttr.BillNoFormat, "ccflow  Document Numbering Format ");

                // add 2014-10-19.
                map.AddDDLSysEnum(FlowAttr.ChartType, (int)FlowChartType.Icon, " Node graph type ",true, true, 
                    "ChartType", "@0= Geometry @1= Portrait Photos ");
                
                map.AddBoolean(FlowAttr.IsCanStart, true,  " No independent start ?( Independent startup process can be displayed in initiating the process list )" , true, true, true);
                map.AddBoolean(FlowAttr.IsMD5, false, " Whether the data encryption process (MD5 Data encryption tamperproof )", true, true,true);
                map.SetHelperBaidu(FlowAttr.IsMD5, "ccflow MD5");
                map.AddBoolean(FlowAttr.IsFullSA, false, " Whether to automatically calculate future processors ?", true, true, true);

                map.AddBoolean(FlowAttr.IsAutoSendSubFlowOver, false,
                    "( When the sub-processes ) At the end of the process , Check that all the sub-process is completed , Let the parent process is automatically sent to the next step .", true, true,true);
                map.SetHelperBaidu(FlowAttr.IsAutoSendSubFlowOver, "ccflow  Check that all sub-processes are automatically sent to the parent process to complete the next step ");
                map.AddBoolean(FlowAttr.IsGuestFlow, false, " Whether customer engagement process ( Personnel involved in the process of non-organizational structure )", true, true, false);

                // Batch launched  add 2013-12-27. 
                map.AddBoolean(FlowAttr.IsBatchStart, false, " Can batch process initiated ?( If it is necessary to set the field need to fill initiated , Multiple separated by commas )", true, true, true);
                map.AddTBString(FlowAttr.BatchStartFields, null, " Initiated field s", true, false, 0, 500, 10, true);
                map.SetHelperBaidu(FlowAttr.IsBatchStart, "ccflow  Can batch process initiated ");
                map.AddDDLSysEnum(FlowAttr.FlowAppType, (int)FlowAppType.Normal, " Process Application Type ",
                  true, true, "FlowAppType", "@0= Business Process @1= Engineering ( Team Process )@2= Document Process (VSTO)");
                map.AddDDLSysEnum(FlowAttr.TimelineRole, (int)TimelineRole.ByNodeSet, " Timeliness rules ",
                 true, true, FlowAttr.TimelineRole, "@0= By node ( Defined by node attribute )@1= By promoters ( Start node SysSDTOfFlow Field calculations )");

                //  Data Storage .
                map.AddDDLSysEnum(FlowAttr.DataStoreModel, (int)DataStoreModel.ByCCFlow,
                    " Process data storage mode ", true, true, FlowAttr.DataStoreModel,
                   "@0= Data track mode @1= Data consolidation mode ");
                map.AddTBString(FlowAttr.PTable, null, " Storing the main table ", true, false, 0, 30, 10);

                //add 2013-05-22.
                map.AddTBString(FlowAttr.HistoryFields, null, " History View Fields ", true, false, 0, 500, 10, true);
                map.SetHelperBaidu(FlowAttr.HistoryFields, "ccflow  History View Fields "); 
                map.AddTBString(FlowAttr.FlowNoteExp, null, " Remarks expression ", true, false, 0, 500, 10, true);
                map.SetHelperBaidu(FlowAttr.FlowNoteExp, "ccflow  Remarks expression ");
                map.AddTBString(FlowAttr.Note, null, " Process Description ", true, false, 0, 100, 10, true);

                map.AddDDLSysEnum(FlowAttr.FlowAppType, (int)FlowAppType.Normal, " Process Application Type ",true, true, "FlowAppType", "@0= Business Process @1= Engineering ( Team Process )@2= Document Process (VSTO)");
                #endregion  Basic properties .

                #region  Automatically start 
                map.AddDDLSysEnum(FlowAttr.FlowRunWay, (int)FlowRunWay.HandWork, " Run ",
                    true, true, FlowAttr.FlowRunWay, "@0= Manually start @1= Start time designated staff @2= Data collection started on time @3= Trigger Start ");

                map.SetHelperBaidu(FlowAttr.FlowRunWay, "ccflow  Run ");
               // map.AddTBString(FlowAttr.RunObj, null, " Run content ", true, false, 0, 100, 10, true);
                map.AddTBStringDoc(FlowAttr.RunObj, null, " Run content ", true, false, true);
                #endregion  Automatically start 

                #region  Process starts restrictions 
                string role = "@0= Without limiting the ";
                role += "@1= Per person per day ";
                role += "@2= Per person per week ";
                role += "@3= Per person per month ";
                role += "@4= Quarterly per person ";
                role += "@5= Once per year ";
                role += "@6= Sponsored column can not be repeated ,( Multiple columns can be separated by commas )";
                role += "@7= Set SQL Data source is empty , You can start or return zero results .";
                role += "@8= Set SQL Data source is empty , Can not start or return zero results .";
                map.AddDDLSysEnum(FlowAttr.StartLimitRole, (int)StartLimitRole.None, " Start limit rules ", true, true, FlowAttr.StartLimitRole, role,true);
                map.AddTBString(FlowAttr.StartLimitPara, null, " Rule parameters ", true, false, 0, 500, 10, true);
                map.AddTBStringDoc(FlowAttr.StartLimitAlert, null, " Tips restrictions ", true, false,true);

             //   map.AddTBString(FlowAttr.StartLimitAlert, null, " Tips restrictions ", true, false, 0, 500, 10, true);
            //    map.AddDDLSysEnum(FlowAttr.StartLimitWhen, (int)StartLimitWhen.StartFlow, " Tip Time ", true, true, FlowAttr.StartLimitWhen, "@0= When you start the process @1= Before sending tips ", false);
                #endregion  Process starts restrictions 

                #region  Before initiating navigation .
                //map.AddDDLSysEnum(FlowAttr.DataStoreModel, (int)DataStoreModel.ByCCFlow,
                //    " Process data storage mode ", true, true, FlowAttr.DataStoreModel,
                //   "@0= Data track mode @1= Data consolidation mode ");

                // Initiated prior to setting rules .
                map.AddDDLSysEnum(FlowAttr.StartGuideWay, (int)StartGuideWay.None, " Pre navigation ", true, true,
                    FlowAttr.StartGuideWay,
                    "@0=None@1= According to the system URL-( Sons Process ) Single mode @2= According to the system URL-( Child parent process ) Multiple modes @3= According to the system URL-( Entity records ) Single mode @4= According to the system URL-( Entity records ) Multiple modes @5= From the start node Copy Data @10= According to custom Url@11= According to user input parameters ",true);
                map.SetHelperBaidu(FlowAttr.StartGuideWay, "ccflow  Pre navigation ");

                map.AddTBStringDoc(FlowAttr.StartGuidePara1, null, " Parameters 1", true, false,true);
                map.AddTBStringDoc(FlowAttr.StartGuidePara2, null, " Parameters 2", true, false, true);
                map.AddTBStringDoc(FlowAttr.StartGuidePara3, null, " Parameters 3", true, false, true);

                map.AddBoolean(FlowAttr.IsResetData, false, " Whether starting node data reset button is enabled ?", true, true, true);
           //     map.AddBoolean(FlowAttr.IsImpHistory, false, " Whether to enable import historical data button ?", true, true, true);
                map.AddBoolean(FlowAttr.IsLoadPriData, false, " Whether to automatically load the data on a ?", true, true, true);

                #endregion  Before initiating navigation .

                #region  Continuation of the process .
                // Continuation of the process .
                map.AddDDLSysEnum(FlowAttr.CFlowWay, (int)CFlowWay.None," Continuation of the process ", true, true,
                    FlowAttr.CFlowWay,"@0=None: Non-continuation of the class Process @1= In accordance with the parameters @2= According to field configuration ");
                map.AddTBStringDoc(FlowAttr.CFlowPara, null, " Continuation of the process parameters ", true, false,true);
        
                // add 2013-03-24.
                map.AddTBString(FlowAttr.DesignerNo, null, " Designers number ", false, false, 0, 32, 10);
                map.AddTBString(FlowAttr.DesignerName, null, " Designer name ", false, false, 0, 100, 10);
                #endregion  Continuation of the process .


                map.AddSearchAttr(FlowAttr.FK_FlowSort);

                //map.AddRefMethod(rm);
                RefMethod rm = new RefMethod();
                rm = new RefMethod();
                rm.Title = " Commissioning "; // " Design inspection report ";
                //rm.ToolTip = " Inspection process design problems .";
                rm.Icon = Glo.CCFlowAppPath + "WF/Img/EntityFunc/Flow/Run.png";
                rm.ClassMethodName = this.ToString() + ".DoRunIt";
                rm.RefMethodType = RefMethodType.LinkeWinOpen;
                map.AddRefMethod(rm);

                rm = new RefMethod();
                rm.Title = " Inspection Report "; // " Design inspection report ";
                rm.Icon = Glo.CCFlowAppPath + "WF/Img/EntityFunc/Flow/CheckRpt.png";
                rm.ClassMethodName = this.ToString() + ".DoCheck";
                map.AddRefMethod(rm);

                rm = new RefMethod();
                rm.Title = " Design Reports "; // " Reports run ";
                rm.Icon = Glo.CCFlowAppPath + "WF/Img/Btn/Rpt.gif";
                rm.ClassMethodName = this.ToString() + ".DoOpenRpt()";
                rm.RefMethodType = RefMethodType.LinkeWinOpen;
                map.AddRefMethod(rm);

                rm = new RefMethod();
                rm.Icon = Glo.CCFlowAppPath + "WF/Img/Btn/Delete.gif";
                rm.Title = " Delete all process data "; // this.ToE("DelFlowData", " Deleting Data "); // " Deleting Data ";
                rm.Warning = " Are you sure you want to delete the data it processes ? \t\n The flow of data is deleted , Can not be restored , Please note that the deleted content .";// " Are you sure you want to delete the data it processes ?";
                rm.ClassMethodName = this.ToString() + ".DoDelData";
                map.AddRefMethod(rm);

                rm = new RefMethod();
                rm.Icon = Glo.CCFlowAppPath + "WF/Img/Btn/Delete.gif";
                rm.Title = " According to the work ID To delete a single process "; // this.ToE("DelFlowData", " Deleting Data "); // " Deleting Data ";
                rm.ClassMethodName = this.ToString() + ".DoDelDataOne";
                rm.HisAttrs.AddTBInt("WorkID",0, " Enter the job ID",true,false);
                rm.HisAttrs.AddTBString("beizhu", null, " Delete Notes ", true, false,0,100,100);
                map.AddRefMethod(rm);

                rm = new RefMethod();
                rm.Icon = Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";
                rm.Title = " Regenerate the report data "; // " Deleting Data ";
                rm.Warning = " Are you sure you want to perform ?  Watch out : This method is resource-intensive .";// " Are you sure you want to delete the data it processes ?";
                rm.ClassMethodName = this.ToString() + ".DoReloadRptData";
                map.AddRefMethod(rm);


                rm = new RefMethod();
                rm.Title = " Set automatically initiates a data source ";
                rm.Icon = Glo.CCFlowAppPath + "WF/Img/EntityFunc/Flow/Run.png";
                rm.ClassMethodName = this.ToString() + ".DoSetStartFlowDataSources()";
                // Setting the relevant fields .
                rm.RefAttrKey = FlowAttr.FlowRunWay;
                rm.RefMethodType = RefMethodType.LinkeWinOpen;
                rm.Target = "_blank";
                map.AddRefMethod(rm);

                rm = new RefMethod();
                rm.Title = " Manually start the regular tasks ";
                rm.Icon = Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";
                rm.Warning = " Are you sure you want to perform ?  Watch out : For the amount of data because data Jiaotong University web The execution time limit problem , Will cause execution failed .";// " Are you sure you want to delete the data it processes ?";
                rm.ClassMethodName = this.ToString() + ".DoAutoStartIt()";
                // Setting the relevant fields .
                rm.RefAttrKey = FlowAttr.FlowRunWay;
                rm.Target = "_blank";
                map.AddRefMethod(rm);


                rm = new RefMethod();
                rm.Title = " Process Monitoring ";
                rm.Icon = Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";
                rm.ClassMethodName = this.ToString() + ".DoDataManger()";
                map.AddRefMethod(rm);

                rm = new RefMethod();
                rm.Title = " Batch Edit Node Properties ";
                rm.Icon = Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";
                rm.ClassMethodName = this.ToString() + ".DoFeatureSetUI()";
                rm.RefMethodType = RefMethodType.RightFrameOpen;
                map.AddRefMethod(rm);
              

                rm = new RefMethod();
                rm.Title = " Regenerate process title ";
                rm.Icon = Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";
                rm.ClassMethodName = this.ToString() + ".DoGenerTitle()";
                rm.Warning = " Are you sure you want to re-generate the title under the new rules it ?";
                //rm.RefAttrKey = FlowAttr.TitleRole;
                //rm.RefMethodType = RefMethodType.LinkModel;
                map.AddRefMethod(rm);


                rm = new RefMethod();
                rm.Title = " Rollback Process ";
                rm.Icon = Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";
                rm.ClassMethodName = this.ToString() + ".DoRebackFlowData()";
               // rm.Warning = " Are you sure you want to roll back its ?";
                rm.HisAttrs.AddTBInt("workid", 0, " Please enter will roll WorkID", true, false);
                rm.HisAttrs.AddTBInt("nodeid", 0, " The node will be rolled ID", true, false);
                rm.HisAttrs.AddTBString("note", null, " Rollback reason ", true, false,0,600,200);
                map.AddRefMethod(rm);

                rm = new RefMethod();
                rm.Title = " Process tree form ";
                rm.Icon = Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";
                rm.ClassMethodName = this.ToString() + ".DoFlowFormTree()";
                map.AddRefMethod(rm);

                  rm = new RefMethod();
                rm.Title = " Batch initiate field ";
                rm.Icon = Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";
                rm.ClassMethodName = this.ToString() + ".DoBatchStartFields()";
                map.AddRefMethod(rm);


                rm = new RefMethod();
                rm.Title = " Binding Process Form ";
                rm.Icon = Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";
                rm.ClassMethodName = this.ToString() + ".DoBindFlowSheet()";
                map.AddRefMethod(rm);


                //rm = new RefMethod();
                //rm.Title = " Set up automatic launch "; // " Reports run ";
                //rm.Icon = "/WF/Img/Btn/View.gif";
                //rm.ClassMethodName = this.ToString() + ".DoOpenRpt()";
                ////rm.Icon = "/WF/Img/Btn/Table.gif"; 
                //map.AddRefMethod(rm);

                //rm = new RefMethod();
                //rm.Title = this.ToE("Event", " Event "); // " Reports run ";
                //rm.Icon = "/WF/Img/Btn/View.gif";
                //rm.ClassMethodName = this.ToString() + ".DoOpenRpt()";
                ////rm.Icon = "/WF/Img/Btn/Table.gif";
                //map.AddRefMethod(rm);

                //rm = new RefMethod();
                //rm.Title = this.ToE("FlowSheetDataOut", " Data transferred definition ");  //" Data transferred definition ";
                ////  rm.Icon = "/WF/Img/Btn/Table.gif";
                //rm.ToolTip = " In the process completion time , Process data is transferred to another storage system applications .";
                //rm.ClassMethodName = this.ToString() + ".DoExp";
                //map.AddRefMethod(rm);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        #region   Public Methods 
        /// <summary>
        ///  Batch Edit Node Properties .
        /// </summary>
        /// <returns></returns>
        public string DoFeatureSetUI()
        {
            return Glo.CCFlowAppPath + "WF/Admin/FeatureSetUI.aspx?s=d34&ShowType=FlowFrms&FK_Node="+int.Parse(this.No)+"01&FK_Flow=" + this.No + "&ExtType=StartFlow&RefNo=" + DataType.CurrentDataTime;
        }
        public string DoBindFlowSheet()
        {
            PubClass.WinOpen(Glo.CCFlowAppPath+"WF/Admin/FlowFrms.aspx?s=d34&ShowType=FlowFrms&FK_Node=0&FK_Flow=" + this.No + "&ExtType=StartFlow&RefNo=" + DataType.CurrentDataTime, 700, 500);
            return null;
        }
        /// <summary>
        ///  Batch initiate field 
        /// </summary>
        /// <returns></returns>
        public string DoBatchStartFields()
        {
            PubClass.WinOpen(Glo.CCFlowAppPath+"WF/Admin/BatchStartFields.aspx?s=d34&FK_Flow=" + this.No + "&ExtType=StartFlow&RefNo=" + DataType.CurrentDataTime, 700, 500);
            return null;
        }
        /// <summary>
        ///  Data recovery process has been completed to a specified node , If the node is 0 Was restored to the last completed node up .
        /// </summary>
        /// <param name="workid"> To be restored workid</param>
        /// <param name="backToNodeID"> Restored to the node number , In the case of 0, Flag last node in response to the process up .</param>
        /// <param name="note"></param>
        /// <returns></returns>
        public string DoRebackFlowData(Int64 workid, int backToNodeID, string note)
        {
            if (note.Length <= 2)
                return " Please fill out the reasons for the recovery process has been completed .";

            Flow fl = new Flow(this.No);
            GERpt rpt = new GERpt("ND" + int.Parse(this.No) + "Rpt");
            rpt.OID = workid;
            int i = rpt.RetrieveFromDBSources();
            if (i == 0)
                throw new Exception("@ Error , Process data loss .");
            if (backToNodeID == 0)
                backToNodeID = rpt.FlowEndNode;

            Emp empStarter = new Emp(rpt.FlowStarter);

            //  Last node .
            Node endN = new Node(backToNodeID);
            GenerWorkFlow gwf = null;
			bool isHaveGener=false;
            try
            {
                #region  Create a master table data flow engine .
                gwf = new GenerWorkFlow();
                gwf.WorkID = workid;
                if (gwf.RetrieveFromDBSources() == 1)
				{
					isHaveGener=true;
                    throw new Exception("@ Current work ID:" + workid + " The process is not over , Can not use this method to recover .");
				}

                gwf.FK_Flow = this.No;
                gwf.FlowName = this.Name;
                gwf.WorkID = workid;
                gwf.PWorkID = rpt.PWorkID;
                gwf.PFlowNo = rpt.PFlowNo;
                gwf.PNodeID = rpt.PNodeID;
                gwf.PEmp = rpt.PEmp;


                gwf.FK_Node = backToNodeID;
                gwf.NodeName = endN.Name;

                gwf.Starter = rpt.FlowStarter;
                gwf.StarterName = empStarter.Name;
                gwf.FK_FlowSort = fl.FK_FlowSort;
                gwf.Title = rpt.Title;
                gwf.WFState = WFState.ReturnSta; /* The state is set to return */
                gwf.FK_Dept = rpt.FK_Dept;

                Dept dept = new Dept(empStarter.FK_Dept);

                gwf.DeptName = dept.Name;
                gwf.PRI = 1;

                DateTime dttime = DateTime.Now;
                dttime = dttime.AddDays(3);

                gwf.SDTOfNode = dttime.ToString("yyyy-MM-dd HH:mm:ss");
                gwf.SDTOfFlow = dttime.ToString("yyyy-MM-dd HH:mm:ss");
                if (isHaveGener)
                    gwf.Update();
                else
                    gwf.Insert(); /* Insert data flow engine .*/

                #endregion  Create a master table data flow engine 

                int startNode = int.Parse(this.No + "01");
                string ndTrack = "ND" + int.Parse(this.No) + "Track";
               string actionType = (int)ActionType.Forward + "," + (int)ActionType.FlowOver + "," + (int)ActionType.ForwardFL + "," + (int)ActionType.ForwardHL;
               // string actionType = " NDFrom=" + (int)ActionType.Forward + " OR NDFrom=" + (int)ActionType.FlowOver + " OR NDFrom=" + (int)ActionType.ForwardFL + " OR NDFrom=" + (int)ActionType.ForwardHL;
               string sql = "SELECT  * FROM " + ndTrack + " WHERE   ActionType IN (" + actionType + ")  and WorkID=" + workid + " ORDER BY RDT DESC, NDFrom ";
                System.Data.DataTable dt = DBAccess.RunSQLReturnTable(sql);
                if (dt.Rows.Count == 0)
                    throw new Exception("@ The work ID:" + workid + " The data does not exist .");

                string starter = "";
                bool isMeetSpecNode = false;
                GenerWorkerList currWl = new GenerWorkerList();
                foreach (DataRow dr in dt.Rows)
                {
                    int ndFrom = int.Parse(dr["NDFrom"].ToString());
                    Node nd = new Node(ndFrom);

                    string ndFromT = dr["NDFromT"].ToString();
                    string EmpFrom = dr[TrackAttr.EmpFrom].ToString();
                    string EmpFromT = dr[TrackAttr.EmpFromT].ToString();

                    //  Increase the   Information Staff .
                    GenerWorkerList gwl = new GenerWorkerList();
                    gwl.WorkID = workid;
                    gwl.FK_Flow = this.No;

                    gwl.FK_Node = ndFrom;
                    gwl.FK_NodeText = ndFromT;

                    if (gwl.FK_Node == backToNodeID)
                    {
                        gwl.IsPass = false;
                        currWl = gwl;
                    }

                    gwl.FK_Emp = EmpFrom;
                    gwl.FK_EmpText = EmpFromT;
                    if (gwl.IsExits)
                        continue; /* There may be a case of repeated return .*/

                    Emp emp = new Emp(gwl.FK_Emp);
                    gwl.FK_Dept = emp.FK_Dept;

                    gwl.RDT = dr["RDT"].ToString();
                    gwl.SDT = dr["RDT"].ToString();
                    gwl.DTOfWarning = gwf.SDTOfNode;
                    gwl.WarningDays = nd.WarningDays;
                    gwl.IsEnable = true;
                    gwl.WhoExeIt = nd.WhoExeIt;
                    gwl.Insert();
                }

                #region  Join return information ,  Let the recipient can see the reason for the return .
                ReturnWork rw = new ReturnWork();
                rw.WorkID = workid;
                rw.ReturnNode = backToNodeID;
                rw.ReturnNodeName = endN.Name;
                rw.Returner = WebUser.No;
                rw.ReturnerName = WebUser.Name;

                rw.ReturnToNode = currWl.FK_Node;
                rw.ReturnToEmp = currWl.FK_Emp;
                rw.Note = note;
                rw.RDT = DataType.CurrentDataTime;
                rw.IsBackTracking = false;
                rw.MyPK = BP.DA.DBAccess.GenerGUID();
                #endregion    Join return information ,  Let the recipient can see the reason for the return .

                // Update the status of the process table .
                rpt.FlowEnder = currWl.FK_Emp;
                rpt.WFState = WFState.ReturnSta; /* The state is set to return */
                rpt.FlowEndNode = currWl.FK_Node;
                rpt.Update();

                //  Send a message to the recipient .
                BP.WF.Dev2Interface.Port_SendMsg(currWl.FK_Emp, " Restoration work :" + gwf.Title, " Work is :" + WebUser.No + "  Recovery ." + note, "ReBack" + workid, BP.WF.SMSMsgType.ToDo, this.No, int.Parse(this.No + "01"), workid, 0);

                // Written to the log .
                WorkNode wn = new WorkNode(workid, currWl.FK_Node);
                wn.AddToTrack(ActionType.RebackOverFlow, currWl.FK_Emp, currWl.FK_EmpText, currWl.FK_Node, currWl.FK_NodeText, note);

                return "@ Has been restored successfully , Now the flow has been restored to ("+currWl.FK_NodeText+"). @ Current treatment of human work (" + currWl.FK_Emp + " , " + currWl.FK_EmpText + ")  @ Please inform him handling .";
            }
            catch (Exception ex)
            {
                gwf.Delete();
                GenerWorkerList wl = new GenerWorkerList();
                wl.Delete(GenerWorkerListAttr.WorkID, workid);

                string sqls = "";
                sqls += "@UPDATE " + fl.PTable + " SET WFState=" + (int)WFState.Complete + " WHERE OID=" + workid;
                DBAccess.RunSQLs(sqls);
                return "<font color=red> An error occurred during will roll </font><hr>" + ex.Message;
            }
        }
        /// <summary>
        ///  Regenerate title , Under the new rules .
        /// </summary>
        public string DoGenerTitle()
        {
            if (WebUser.No != "admin")
                return "only admin Users can execute .";
            Flow fl = new Flow(this.No);
            Node nd = fl.HisStartNode;
            Works wks = nd.HisWorks;
            wks.RetrieveAllFromDBSource(WorkAttr.Rec);
            string table = nd.HisWork.EnMap.PhysicsTable;
            string tableRpt = "ND" + int.Parse(this.No) + "Rpt";
            Sys.MapData md = new Sys.MapData(tableRpt);
            foreach (Work wk in wks)
            {

                if (wk.Rec != WebUser.No)
                {
                    BP.Web.WebUser.Exit();
                    try
                    {
                        Emp emp = new Emp(wk.Rec);
                        BP.Web.WebUser.SignInOfGener(emp);
                    }
                    catch
                    {
                        continue;
                    }
                }
                string sql = "";
                string title = WorkNode.GenerTitle(fl, wk);
                Paras ps = new Paras();
                ps.Add("Title", title);
                ps.Add("OID", wk.OID);
                ps.SQL = "UPDATE " + table + " SET Title=" + SystemConfig.AppCenterDBVarStr + "Title WHERE OID=" + SystemConfig.AppCenterDBVarStr + "OID";
                DBAccess.RunSQL(ps);

                ps.SQL = "UPDATE " + md.PTable + " SET Title=" + SystemConfig.AppCenterDBVarStr + "Title WHERE OID=" + SystemConfig.AppCenterDBVarStr + "OID";
                DBAccess.RunSQL(ps);

                ps.SQL = "UPDATE WF_GenerWorkFlow SET Title=" + SystemConfig.AppCenterDBVarStr + "Title WHERE WorkID=" + SystemConfig.AppCenterDBVarStr + "OID";
                DBAccess.RunSQL(ps);

                ps.SQL = "UPDATE WF_GenerFH SET Title=" + SystemConfig.AppCenterDBVarStr + "Title WHERE FID=" + SystemConfig.AppCenterDBVarStr + "OID";
                DBAccess.RunSQLs(sql);
            }
            Emp emp1 = new Emp("admin");
            BP.Web.WebUser.SignInOfGener(emp1);

            return " All generate success , Impact data (" + wks.Count + ") records";
        }
        /// <summary>
        ///  Process Monitoring 
        /// </summary>
        /// <returns></returns>
        public string DoDataManger()
        {
            PubClass.WinOpen(Glo.CCFlowAppPath + "WF/Rpt/OneFlow.aspx?FK_Flow=" + this.No + "&ExtType=StartFlow&RefNo=", 700, 500);
            return null;
        }
        /// <summary>
        ///  Binding Process Form 
        /// </summary>
        /// <returns></returns>
        public string DoFlowFormTree()
        {
            PubClass.WinOpen(Glo.CCFlowAppPath+"WF/Admin/FlowFormTree.aspx?s=d34&FK_Flow=" + this.No + "&ExtType=StartFlow&RefNo=" + DataType.CurrentDataTime, 700, 500);
            return null;
        }
        /// <summary>
        ///  Defining Report 
        /// </summary>
        /// <returns></returns>
        public string DoAutoStartIt()
        {
            Flow fl = new Flow();
            fl.No = this.No;
            fl.RetrieveFromDBSources();
            return fl.DoAutoStartIt();
        }
        /// <summary>
        ///  Delete Process 
        /// </summary>
        /// <param name="workid"></param>
        /// <param name="sd"></param>
        /// <returns></returns>
        public string DoDelDataOne(int workid, string note)
        {
            BP.WF.Dev2Interface.Flow_DoDeleteFlowByReal(this.No, workid, true);
            return " Deleted successfully  workid=" + workid + "   Grounds :" + note;
        }
        /// <summary>
        ///  Set initiate a data source 
        /// </summary>
        /// <returns></returns>
        public string DoSetStartFlowDataSources()
        {
            string flowID=int.Parse(this.No).ToString()+"01";
            return Glo.CCFlowAppPath+"WF/MapDef/MapExt.aspx?s=d34&FK_MapData=ND" + flowID + "&ExtType=StartFlow&RefNo=" ;
        }
        public string DoCCNode()
        {
            PubClass.WinOpen(Glo.CCFlowAppPath+"WF/Admin/CCNode.aspx?FK_Flow=" + this.No, 400, 500);
            return null;
        }
        /// <summary>
        ///  Execute the run 
        /// </summary>
        /// <returns></returns>
        public string DoRunIt()
        {
            return "/WF/Admin/TestFlow.aspx?FK_Flow="+this.No+"&Lang=CH";
        }
        /// <summary>
        ///  Perform checks 
        /// </summary>
        /// <returns></returns>
        public string DoCheck()
        {
            Flow fl = new Flow();
            fl.No = this.No;
            fl.RetrieveFromDBSources();
            return fl.DoCheck();
        }
        /// <summary>
        ///  Execution reload data 
        /// </summary>
        /// <returns></returns>
        public string DoReloadRptData()
        {
            Flow fl = new Flow();
            fl.No = this.No;
            fl.RetrieveFromDBSources();
            return fl.DoReloadRptData();
        }
        /// <summary>
        ///  Deleting Data .
        /// </summary>
        /// <returns></returns>
        public string DoDelData()
        {
            Flow fl = new Flow();
            fl.No = this.No;
            fl.RetrieveFromDBSources();
            return fl.DoDelData();
        }
        /// <summary>
        ///  Design data turn out 
        /// </summary>
        /// <returns></returns>
        public string DoExp()
        {
            Flow fl = new Flow();
            fl.No = this.No;
            fl.RetrieveFromDBSources();
            return fl.DoExp();
        }
        /// <summary>
        ///  Defining Report 
        /// </summary>
        /// <returns></returns>
        public string DoDRpt()
        {
            Flow fl = new Flow();
            fl.No = this.No;
            fl.RetrieveFromDBSources();
            return fl.DoDRpt();
        }
        /// <summary>
        ///  Run the report 
        /// </summary>
        /// <returns></returns>
        public string DoOpenRpt()
        {
            return Glo.CCFlowAppPath + "WF/Rpt/OneFlow.aspx?FK_Flow=" + this.No + "&DoType=Edit&FK_MapData=ND" +
                   int.Parse(this.No) + "Rpt";
        }
        /// <summary>
        ///  Things after update , Also update the cache .
        /// </summary>
        protected override void afterUpdate()
        {
           // Flow fl = new Flow();
           // fl.No = this.No;
           // fl.RetrieveFromDBSources();
           //fl.Update();

            if (Glo.OSModel == OSModel.BPM)
            {
                DBAccess.RunSQL("UPDATE  GPM_Menu SET Name='" + this.Name + "' WHERE Flag='Flow" + this.No + "' AND FK_App='" + SystemConfig.SysNo + "'");
            }
        }
        protected override bool beforeUpdate()
        {
            // Version update process 
            Flow.UpdateVer(this.No);

            #region  Synchronization event entity .
            try
            {
                if (string.IsNullOrEmpty(this.FlowMark) == false)
                    this.FlowEventEntity = BP.WF.Glo.GetFlowEventEntityByFlowMark(this.FlowMark).ToString();
                else
                    this.FlowEventEntity = "";
            }
            catch
            {
                this.FlowEventEntity = "";
            }
            #endregion  Synchronization event entity .

            return base.beforeUpdate();
        }
        protected override void afterInsertUpdateAction()
        {
            // Synchronization process data table .
            string ndxxRpt="ND"+int.Parse(this.No)+"Rpt";
            Flow fl = new Flow(this.No);
            if (fl.PTable != "ND" + int.Parse(this.No) + "Rpt")
            {
                BP.Sys.MapData md = new Sys.MapData(ndxxRpt);
                if (md.PTable != fl.PTable)
                    md.Update();
            }
            base.afterInsertUpdateAction();
        }
        #endregion
    }
    /// <summary>
    ///  Collection process 
    /// </summary>
    public class FlowSheets : EntitiesNoName
    {
        #region  Inquiry 
        /// <summary>
        ///  During check out the entire process in survival within 
        /// </summary>
        /// <param name="FlowSort"> Process Category </param>
        /// <param name="IsCountInLifeCycle"> Is not included in the period of survival  true  Check out all of the  </param>
        public void Retrieve(string FlowSort)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(BP.WF.Template.FlowAttr.FK_FlowSort, FlowSort);
            qo.addOrderBy(BP.WF.Template.FlowAttr.No);
            qo.DoQuery();
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Workflow 
        /// </summary>
        public FlowSheets() { }
        /// <summary>
        ///  Workflow 
        /// </summary>
        /// <param name="fk_sort"></param>
        public FlowSheets(string fk_sort)
        {
            this.Retrieve(BP.WF.Template.FlowAttr.FK_FlowSort, fk_sort);
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
                return new FlowSheet();
            }
        }
        #endregion
    }
}

