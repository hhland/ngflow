using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using CCFlow.WF.Comm.UC;
using BP.WF;
using BP.Port;
using BP.Web.Controls;
using BP.DA;
using BP.En;
using BP.Web;
using BP.WF.Template;
using BP.WF.Data;
using BP.Sys;

namespace CCFlow.WF.UC
{
    public partial class MyFlow : BP.Web.UC.UCBase3
    {
        #region  Controls 
        public string _PageSamll = null;
        public string PageSmall
        {
            get
            {
                if (_PageSamll == null)
                {
                    if (this.PageID.ToLower().Contains("small"))
                        _PageSamll = "Small";
                    else
                        _PageSamll = "";
                }
                return _PageSamll;
            }
        }
        /// <summary>
        ///  Send 
        /// </summary>
        protected Btn Btn_Send
        {
            get
            {
                return this.toolbar.GetBtnByID(NamesOfBtn.Send);
            }
        }
        protected Btn Btn_Delete
        {
            get
            {
                return this.toolbar.GetBtnByID(NamesOfBtn.Delete);
            }
        }
        /// <summary>
        ///  Save 
        /// </summary>
        protected Btn Btn_Save
        {
            get
            {
                Btn btn = this.toolbar.GetBtnByID(NamesOfBtn.Save);
                if (btn == null)
                    btn = new Btn();
                return btn;
            }
        }
        protected Btn Btn_ReturnWork
        {
            get
            {
                Btn btn = this.toolbar.GetBtnByID("Btn_ReturnWork");
                if (btn == null)
                    btn = new Btn();
                return btn;
            }
        }
        protected Btn Btn_Shift
        {
            get
            {
                return this.toolbar.GetBtnByID(BP.Web.Controls.NamesOfBtn.Shift);
            }
        }
        #endregion

        #region   Operating variables 
        /// <summary>
        ///  The current process ID 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                string s = this.Request.QueryString["FK_Flow"];
                if (string.IsNullOrEmpty(s))
                    throw new Exception("@ Process ID parameter error ...");

                return BP.WF.Dev2Interface.TurnFlowMarkToFlowNo(s);
            }
        }
        public string FromNode
        {
            get
            {
                return this.Request.QueryString["FromNode"];
            }
        }
        public string DoFunc
        {
            get
            {
                return this.Request.QueryString["DoFunc"];
            }
        }
        public string CFlowNo
        {
            get
            {
                return this.Request.QueryString["CFlowNo"];
            }
        }
        public string WorkIDs
        {
            get
            {
                return this.Request.QueryString["WorkIDs"];
            }
        }
        public string Nos
        {
            get
            {
                return this.Request.QueryString["Nos"];
            }
        }

        public bool IsCC
        {
            get
            {

                if (string.IsNullOrEmpty(this.Request.QueryString["Paras"]) == false)
                {
                    string myps = this.Request.QueryString["Paras"];

                    if (myps.Contains("IsCC=1") == true)
                        return true;
                }
                if (string.IsNullOrEmpty(this.Request.QueryString["AtPara"]) == false)
                {
                    string myps = this.Request.QueryString["AtPara"];

                    if (myps.Contains("IsCC=1") == true)
                        return true;
                }
                return false;
            }
        }
        /// <summary>
        ///  Current work ID
        /// </summary>
        public Int64 WorkID
        {
            get
            {
                if (ViewState["WorkID"] == null)
                {
                    if (this.Request.QueryString["WorkID"] == null)
                        return 0;
                    else
                        return Int64.Parse(this.Request.QueryString["WorkID"]);
                }
                else
                    return Int64.Parse(ViewState["WorkID"].ToString());
            }
            set
            {
                ViewState["WorkID"] = value;
            }
        }
        public Int64 CWorkID
        {
            get
            {
                if (ViewState["CWorkID"] == null)
                {
                    if (this.Request.QueryString["CWorkID"] == null)
                        return 0;
                    else
                        return Int64.Parse(this.Request.QueryString["CWorkID"]);
                }
                else
                    return Int64.Parse(ViewState["CWorkID"].ToString());
            }
            set
            {
                ViewState["CWorkID"] = value;
            }
        }
        private int _FK_Node = 0;
        /// <summary>
        ///  Current  NodeID , At the beginning of time ,nodeID, Is to a , Start node processes ID.
        /// </summary>
        public int FK_Node
        {
            get
            {
                string fk_nodeReq = this.Request.QueryString["FK_Node"];
                if (string.IsNullOrEmpty(fk_nodeReq))
                    fk_nodeReq = this.Request.QueryString["NodeID"];

                if (string.IsNullOrEmpty(fk_nodeReq) == false)
                    return int.Parse(fk_nodeReq);

                if (_FK_Node == 0)
                {
                    if (this.Request.QueryString["WorkID"] != null)
                    {
                        string sql = "SELECT FK_Node from  WF_GenerWorkFlow where WorkID=" + this.WorkID;
                        _FK_Node = DBAccess.RunSQLReturnValInt(sql);
                    }
                    else
                    {
                        _FK_Node = int.Parse(this.FK_Flow + "01");
                    }
                }
                return _FK_Node;
            }
        }
        public int FID
        {
            get
            {
                try
                {
                    return int.Parse(this.Request.QueryString["FID"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
        public int PWorkID
        {
            get
            {
                try
                {
                    string s = this.Request.QueryString["PWorkID"];
                    if (string.IsNullOrEmpty(s) == true)
                        s = this.Request.QueryString["PWorkID"];
                    if (string.IsNullOrEmpty(s) == true)
                        s = "0";
                    return int.Parse(s);
                }
                catch
                {
                    return 0;
                }
            }
        }

        private string _width = "";

        public string Width
        {
            get { return _width; }
            set { _width = value; }
        }
        private string _height = "";

        public string Height
        {
            get { return _height; }
            set { _height = value; }

        }
        public string _btnWord = "";
        public string BtnWord
        {
            get { return _btnWord; }
            set { _btnWord = value; }
        }
        #endregion

        private string tKey = DateTime.Now.ToString("yyMMddhhmmss");
        private string small = null;

        #region  Method .
        public void InitToolbar(bool isAskFor, string appPath)
        {
            this.Page.Title = this.currND.Name;
            small = this.PageID;
            // Definition timekey
            small = small.Replace("MyFlow", "");

            if (small != "")
                toolbar.AddBR();

            if (this.IsCC)
            {
                toolbar.Add("<input type=button class=Btn value=' Processes running track ' enable=true onclick=\"WinOpen('" + appPath + "WF/Chart.aspx?WorkID=" + this.WorkID + "&FK_Flow=" + this.FK_Flow + "&FID=" + this.FID + "&FK_Node=" + this.FK_Node + "&s=" + tKey + "','ds'); \" />");
                //  Judge reviewed the components in the current form is enabled , If you enable .
                BP.Sys.FrmWorkCheck fwc = new FrmWorkCheck(this.FK_Node);
                if (fwc.HisFrmWorkCheckSta != FrmWorkCheckSta.Enable)
                {
                    /* If not equal enabled , */
                    toolbar.Add("<input type=button class=Btn value=' Fill audit opinion ' enable=true onclick=\"WinOpen('" + appPath + "WF/WorkOpt/CCCheckNote.aspx?WorkID=" + this.WorkID + "&FK_Flow=" + this.FK_Flow + "&FID=" + this.FID + "&FK_Node=" + this.FK_Node + "&s=" + tKey + "','ds'); \" />");
                }
                return;
            }

            #region  Load flow controller  -  Push button 
            BtnLab btnLab = new BtnLab(currND.NodeID);

            BtnWord = btnLab.WebOfficeEnable + "";
            if (currND.IsEndNode && isAskFor == false)
            {
                /* If the current node is the end node .*/
                if (btnLab.SendEnable && currND.HisBatchRole != BatchRole.Group)
                {
                    /* If the Send button is enabled .*/
                    toolbar.AddBtn(NamesOfBtn.Send, btnLab.SendLab);
                    this.Btn_Send.UseSubmitBehavior = false;
                    this.Btn_Send.OnClientClick = "if(SysCheckFrm()==false) return false;this.disabled=true;SaveDtlAll();KindEditerSync();";
                    this.Btn_Send.Click += new System.EventHandler(ToolBar1_ButtonClick);
                }
            }
            else
            {
                if (btnLab.SendEnable && currND.HisBatchRole != BatchRole.Group && isAskFor == false)
                {
                    /* If the Send button is enabled .*/
                    if (btnLab.SelectAccepterEnable == 2)
                    {
                        /* If people choose window mode is enabled 【 Choose to send both 】.*/
                        toolbar.Add("<input type=button class=Btn value='" + btnLab.SendLab + "' enable=true onclick=\"if(SysCheckFrm()==false) return false;KindEditerSync();window.open('" + appPath + "WF/WorkOpt/Accepter.aspx?WorkID=" + this.WorkID + "&FK_Node=" + currND.NodeID + "&FK_Flow=" + this.FK_Flow + "&FID=" + this.FID + "&type=1',' Select Recipients ', 'height=500, width=400,scrollbars=yes'); \" />");
                        toolbar.AddBtn(NamesOfBtn.Send, btnLab.SendLab);
                        Btn_Send.Style.Add("display", "none");
                        this.Btn_Send.UseSubmitBehavior = false;

                        if (this.currND.HisFormType == NodeFormType.DisableIt)
                            this.Btn_Send.OnClientClick = btnLab.SendJS + "this.disabled=true;"; //this.disabled='disabled'; return true;";
                        else
                            this.Btn_Send.OnClientClick = btnLab.SendJS + "if(SysCheckFrm()==false) return false;this.disabled=true;SaveDtlAll();KindEditerSync();"; //this.disabled='disabled'; return true;";
                        //   this.Btn_Send.OnClientClick = "this.disabled=true;"; //this.disabled='disabled'; return true;";
                        this.Btn_Send.Click += new System.EventHandler(ToolBar1_ButtonClick);
                    }
                    else
                    {
                        toolbar.AddBtn(NamesOfBtn.Send, btnLab.SendLab);
                        this.Btn_Send.UseSubmitBehavior = false;
                        if (btnLab.SendJS.Trim().Length > 2)
                        {
                            this.Btn_Send.OnClientClick = btnLab.SendJS + ";if(SysCheckFrm()==false) return false;this.disabled=true;SaveDtlAll();KindEditerSync();"; //this.disabled='disabled'; return true;";
                        }
                        else
                        {
                            this.Btn_Send.UseSubmitBehavior = false;
                            if (this.currND.HisFormType == NodeFormType.DisableIt)
                                this.Btn_Send.OnClientClick = "this.disabled=true;"; //this.disabled='disabled'; return true;";
                            else
                                this.Btn_Send.OnClientClick = "if(SysCheckFrm()==false) return false;this.disabled=true;SaveDtlAll();KindEditerSync();"; //this.disabled='disabled'; return true;";
                        }
                        this.Btn_Send.Click += new System.EventHandler(ToolBar1_ButtonClick);
                    }
                }
            }

            if (btnLab.SaveEnable && isAskFor == false)
            {
                toolbar.AddBtn(NamesOfBtn.Save, btnLab.SaveLab);
                this.Btn_Save.UseSubmitBehavior = false;
                this.Btn_Save.OnClientClick = "if(SysCheckFrm()==false) return false;this.disabled=true;SaveDtlAll();KindEditerSync();"; //this.disabled='disabled'; return true;";
                //  this.Btn_Save.OnClientClick = "this.disabled=true;"; //this.disabled='disabled'; return true;";
                this.Btn_Save.Click += new System.EventHandler(ToolBar1_ButtonClick);
            }

            if (btnLab.WorkCheckEnable && isAskFor == false)
            {
                /* Check */
                string urlr1 = appPath + "WF/WorkOpt/WorkCheck.aspx?FK_Node=" + this.FK_Node + "&FID=" + this.FID + "&WorkID=" + this.WorkID + "&FK_Flow=" + this.FK_Flow + "&s=" + tKey;
                toolbar.Add("<input type=button class=Btn value='" + btnLab.WorkCheckLab + "' enable=true onclick=\"WinOpen('" + urlr1 + "','dsdd'); \" />");
            }

            if (btnLab.ThreadEnable)
            {
                /* If you want to see the child thread .*/
                string ur2 = appPath + "WF/WorkOpt/ThreadDtl.aspx?FK_Node=" + this.FK_Node + "&FID=" + this.FID + "&WorkID=" + this.WorkID + "&FK_Flow=" + this.FK_Flow + "&s=" + tKey;
                toolbar.Add("<input type=button class=Btn value='" + btnLab.ThreadLab + "' enable=true onclick=\"WinOpen('" + ur2 + "','dsdd'); \" />");
            }

            if (btnLab.JumpWayEnable && isAskFor == false)
            {
                /* If the field does not have focus */
                string urlr = appPath + "WF/JumpWay.aspx?FK_Node=" + this.FK_Node + "&FID=" + this.FID + "&WorkID=" + this.WorkID + "&FK_Flow=" + this.FK_Flow + "&s=" + tKey;
                toolbar.Add("<input type=button class=Btn value='" + btnLab.JumpWayLab + "' enable=true onclick=\"To('" + urlr + "'); \" />");
            }

            if (btnLab.ReturnEnable && isAskFor == false && this.currND.IsStartNode == false)
            {
                /* If the field does not have focus */
                string urlr = appPath + "WF/WorkOpt/ReturnWork.aspx?FK_Node=" + this.FK_Node + "&FID=" + this.FID + "&WorkID=" + this.WorkID + "&FK_Flow=" + this.FK_Flow + "&s=" + tKey;
                toolbar.Add("<input type=button class=Btn value='" + btnLab.ReturnLab + "' enable=true onclick=\"ReturnWork('" + urlr + "','" + btnLab.ReturnField + "'); \" />");
            }

            //if (btnLab.ReturnEnable && isAskFor == false && this.currND.IsStartNode == false && this.currND.FocusField != ""  )
            //{
            //    /* If the focus of the field */
            //    toolbar.AddBtn("Btn_ReturnWork", btnLab.ReturnLab);
            //    this.Btn_ReturnWork.UseSubmitBehavior = false;
            //    this.Btn_ReturnWork.OnClientClick = "this.disabled=true;";
            //    this.Btn_ReturnWork.Click += new System.EventHandler(ToolBar1_ButtonClick);
            //}

            //  if (btnLab.HungEnable && this.currND.IsStartNode == false)
            if (btnLab.HungEnable)
            {
                /* Pending */
                string urlr = appPath + "WF/WorkOpt/HungUp.aspx?FK_Node=" + this.FK_Node + "&FID=" + this.FID + "&WorkID=" + this.WorkID + "&FK_Flow=" + this.FK_Flow + "&s=" + tKey;
                toolbar.Add("<input type=button class=Btn value='" + btnLab.HungLab + "' enable=true onclick=\"To('" + urlr + "'); \" />");
                //toolbar.Add("<input type=button class=Btn value='" + btnLab.PrintDocLab + "' enable=true onclick=\"WinOpen('" + urlr + "','dsdd'); \" />");
            }

            if (btnLab.ShiftEnable && isAskFor == false)
            {
                /* Transfer */
                toolbar.AddBtn("Btn_Shift", btnLab.ShiftLab);
                this.Btn_Shift.Click += new System.EventHandler(ToolBar1_ButtonClick);
            }

            if ((btnLab.CCRole == CCRole.HandCC || btnLab.CCRole == CCRole.HandAndAuto))
            {
                /*  Cc  */
                // toolbar.Add("<input type=button value='" + btnLab.CCLab + "' enable=true onclick=\"WinOpen('" + appPath + "WF/Msg/Write.aspx?WorkID=" + this.WorkID + "&FK_Node=" + this.FK_Node + "','ds'); \" />");
                toolbar.Add("<input type=button class=Btn value='" + btnLab.CCLab + "' enable=true onclick=\"WinOpen('" + appPath + "WF/WorkOpt/CC.aspx?WorkID=" + this.WorkID + "&FK_Node=" + this.FK_Node + "&FK_Flow=" + this.FK_Flow + "&FID=" + this.FID + "&s=" + tKey + "','ds'); \" />");
            }

            if (btnLab.DeleteEnable != 0 && isAskFor == false)
            {
                /* Process delete rules  */
                switch (this.currND.HisDelWorkFlowRole)
                {
                    case DelWorkFlowRole.None: /* Do not delete */
                        break;
                    case DelWorkFlowRole.ByUser: // Need to interact .
                    case DelWorkFlowRole.DeleteAndWriteToLog:
                    case DelWorkFlowRole.DeleteByFlag:
                        string urlrDel = appPath + "WF/DeleteWorkFlow.aspx?FK_Node=" + this.FK_Node + "&FID=" + this.FID + "&WorkID=" + this.WorkID + "&FK_Flow=" + this.FK_Flow + "&s=" + tKey;
                        toolbar.Add("<input type=button class=Btn value='" + btnLab.DeleteLab + "' enable=true onclick=\"To('" + urlrDel + "'); \" />");
                        break;
                    case DelWorkFlowRole.DeleteReal: //  No need to interact , Direct clean deleted .
                        toolbar.AddBtn("Btn_Delete", btnLab.DeleteLab);
                        this.Btn_Delete.OnClientClick = "return confirm(' Delete process to be executed , You acknowledge that you ?')";
                        this.Btn_Delete.Click += new System.EventHandler(ToolBar1_ButtonClick);
                        break;
                    default:
                        break;
                }
            }

            if (btnLab.EndFlowEnable && this.currND.IsStartNode == false && isAskFor == false)
            {
                toolbar.Add("<input type=button class=Btn value='" + btnLab.EndFlowLab + "' enable=true onclick=\"To('./WorkOpt/StopFlow.aspx?&DoType=StopFlow&FID=" + this.FID + "&WorkID=" + this.WorkID + "&FK_Node=" + this.FK_Node + "&FK_Flow=" + this.FK_Flow + "&s=" + tKey + "'); \" />");
                //toolbar.AddBtn("Btn_EndFlow", btnLab.EndFlowLab);
                //toolbar.GetBtnByID("Btn_EndFlow").OnClientClick = "return confirm('" + this.ToE("AYS", " Terminate the process to be executed , You acknowledge that you ?") + "')";
                //toolbar.GetBtnByID("Btn_EndFlow").Click += new System.EventHandler(ToolBar1_ButtonClick);
            }

            //if (btnLab.RptEnable)
            //    toolbar.Add("<input type=button class=Btn value='" + btnLab.RptLab + "' enable=true onclick=\"WinOpen('" + appPath + "WF/WFRpt.aspx?WorkID=" + this.WorkID + "&FK_Flow=" + this.FK_Flow + "&FID=" + this.FID + "','ds0'); \" />");

            if (btnLab.PrintDocEnable && isAskFor == false)
            {
                /* If not for endorsement  */
                if (this.currND.HisPrintDocEnable == PrintDocEnable.PrintRTF)
                {
                    string urlr = appPath + "WF/WorkOpt/PrintDoc.aspx?FK_Node=" + this.FK_Node + "&FID=" + this.FID + "&WorkID=" + this.WorkID + "&FK_Flow=" + this.FK_Flow + "&s=" + tKey;
                    toolbar.Add("<input type=button class=Btn value='" + btnLab.PrintDocLab + "' enable=true onclick=\"WinOpen('" + urlr + "','dsdd'); \" />");
                }

                if (this.currND.HisPrintDocEnable == PrintDocEnable.PrintHtml)
                {
                    toolbar.Add("<input type=button class=Btn value='" + btnLab.PrintDocLab + "' enable=true onclick=\"printFrom(); \" />");
                }
            }

            if (btnLab.TrackEnable && isAskFor == false)
                toolbar.Add("<input type=button class=Btn value='" + btnLab.TrackLab + "' enable=true onclick=\"WinOpen('" + appPath + "WF/Chart.aspx?WorkID=" + this.WorkID + "&FK_Flow=" + this.FK_Flow + "&FID=" + this.FID + "&FK_Node=" + this.FK_Node + "&s=" + tKey + "','ds'); \" />");

            //if (btnLab.OptEnable)
            //    toolbar.Add("<input type=button class=Btn value='" + btnLab.OptLab + "' onclick=\"WinOpen('" + appPath + "WF/WorkOpt/Home.aspx?WorkID=" + this.WorkID + "&FK_Node=" + currND.NodeID + "&FK_Flow=" + this.FK_Flow + "&FID=" + this.FID + "','opt'); \"  />");

            switch (btnLab.SelectAccepterEnable)
            {
                case 1:
                    if (isAskFor == false)
                        toolbar.Add("<input type=button class=Btn value='" + btnLab.SelectAccepterLab + "' enable=true onclick=\"WinOpen('" + appPath + "WF/WorkOpt/Accepter.aspx?WorkID=" + this.WorkID + "&FK_Node=" + currND.NodeID + "&FK_Flow=" + this.FK_Flow + "&FID=" + this.FID + "&s=" + tKey + "','dds'); \" />");
                    break;
                case 2:
                    //  toolbar.Add("<input type=button class=Btn value='" + btnLab.SelectAccepterLab + "' enable=true onclick=\"WinOpen('" + appPath + "WF/Accepter.aspx?WorkID=" + this.WorkID + "&FK_Node=" + currND.NodeID + "&FK_Flow=" + this.FK_Flow + "&FID=" + this.FID + "&s=" + tKey + "','dds'); \" />");
                    break;
                default:
                    break;
            }

            if (btnLab.SearchEnable && isAskFor == false)
                toolbar.Add("<input type=button class=Btn value='" + btnLab.SearchLab + "' enable=true onclick=\"WinOpen('" + appPath + "WF/Rpt/Search.aspx?EnsName=ND" + int.Parse(this.FK_Flow) + "Rpt&FK_Flow=" + this.FK_Flow + "&s=" + tKey + "','dsd0'); \" />");

            if (btnLab.BatchEnable && isAskFor == false)
            {
                /* Batch Processing */
                string urlr = appPath + "WF/Batch" + small + ".aspx?FK_Node=" + this.FK_Node + "&FID=" + this.FID + "&WorkID=" + this.WorkID + "&FK_Flow=" + this.FK_Flow + "&s=" + tKey;
                toolbar.Add("<input type=button class=Btn value='" + btnLab.BatchLab + "' enable=true onclick=\"To('" + urlr + "'); \" />");
            }

            if (btnLab.AskforEnable && gwf != null && gwf.WFState != WFState.Askfor)
            {
                /* Plus sign  */
                string urlr3 = appPath + "WF/WorkOpt/Askfor.aspx?FK_Node=" + this.FK_Node + "&FID=" + this.FID + "&WorkID=" + this.WorkID + "&FK_Flow=" + this.FK_Flow + "&s=" + tKey;
                toolbar.Add("<input type=button class=Btn value='" + btnLab.AskforLab + "' enable=true onclick=\"To('" + urlr3 + "'); \" />");
                //toolbar.Add("<input type=button class=Btn value='" + btnLab.BatchLab + "' enable=true onclick=\"To('" + urlr + "'); \" />");
            }
            if (btnLab.WebOfficeEnable == 1)
            {
                /* Text documents  */
                string urlr = appPath + "WF/WorkOpt/WebOffice.aspx?FK_Node=" + this.FK_Node + "&FID=" + this.FID + "&WorkID=" + this.WorkID + "&FK_Flow=" + this.FK_Flow + "&s=" + tKey;
                toolbar.Add("<input type=button class=Btn value='" + btnLab.WebOfficeLab + "' enable=true onclick=\"WinOpen('" + urlr + "',' Text documents '); \" />");
                //toolbar.Add("<input type=button class=Btn value='" + btnLab.BatchLab + "' enable=true onclick=\"To('" + urlr + "'); \" />");
            }


            if (this.currFlow.IsResetData == true && this.currND.IsStartNode)
            {
                /*  Data Reset function is enabled  */
                string urlr3 = appPath + "WF/MyFlow.aspx?FK_Node=" + this.FK_Node + "&FID=" + this.FID + "&WorkID=" + this.WorkID + "&FK_Flow=" + this.FK_Flow + "&IsDeleteDraft=1&s=" + tKey;
                toolbar.Add("<input type=button class=Btn value=' Data Reset ' enable=true onclick=\"To('" + urlr3 + "','ds'); \" />");
                //toolbar.Add("<input type=button class=Btn value='" + btnLab.BatchLab + "' enable=true onclick=\"To('" + urlr + "'); \" />");
            }
            #endregion
        }
        #endregion  Method .

        #region Page load  Event 
        public void DoDoType()
        {
            switch (this.DoType)
            {
                case "Runing":
                    ShowRuning();
                    return;
                case "Warting":
                    ShowWarting();
                    return;
                default:
                    break;
            }
        }
        public void ShowWarting()
        {
            this.ToolBar1.AddLab("s", " Upcoming work ");
            string sql = "SELECT * FROM WF_EmpWorks WHERE FK_Emp='" + BP.Web.WebUser.No + "'  AND FK_Flow='" + this.FK_Flow + "' ORDER BY WorkID ";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
                return;

            int i = 0;
            bool is1 = false;
            DateTime cdt = DateTime.Now;
            string color = "";
            this.Pub1.AddTable();
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("ID");
            this.Pub1.AddTDTitle(" Node ");
            this.Pub1.AddTDTitle(" Title ");
            this.Pub1.AddTDTitle(" Launch ");
            this.Pub1.AddTDTitle(" Launch date ");
            this.Pub1.AddTDTitle(" Accepted ");
            this.Pub1.AddTDTitle(" The term ");
            this.Pub1.AddTREnd();

            int idx = 0;
            foreach (DataRow dr in dt.Rows)
            {
                string sdt = dr["SDT"] as string;
                DateTime mysdt = DataType.ParseSysDate2DateTime(sdt);
                if (cdt >= mysdt)
                {
                    this.Pub1.AddTRRed(); // ("onmouseover='TROver(this)' onmouseout='TROut(this)' onclick=\"\" ");
                }
                else
                {
                    is1 = this.Pub1.AddTR(is1); // ("onmouseover='TROver(this)' onmouseout='TROut(this)' onclick=\"\" ");
                }
                i++;
                this.Pub1.AddTD(i);
                this.Pub1.AddTD(dr["NodeName"].ToString());
                this.Pub1.AddTD("<a href=\"MyFlow" + this.PageSmall + ".aspx?FK_Flow=" + dr["FK_Flow"] + "&WorkID=" + dr["WorkID"] + "&FID=" + dr["FID"] + "\" >" + dr["Title"].ToString());
                this.Pub1.AddTD(dr["Starter"].ToString());
                this.Pub1.AddTD(dr["RDT"].ToString());
                this.Pub1.AddTD(dr["ADT"].ToString());
                this.Pub1.AddTD(dr["SDT"].ToString());
                this.Pub1.AddTREnd();
            }
            this.Pub1.AddTableEnd();
        }
        public void ShowRuning()
        {
            this.ToolBar1.AddLab("s", " Working in transit ");

            this.Pub1.AddTable("width='80%' align=left");
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("nowarp=true", "No.");
            this.Pub1.AddTDTitle("nowarp=true", " Name ");
            this.Pub1.AddTDTitle("nowarp=true", " The current node ");
            this.Pub1.AddTDTitle("nowarp=true", " Launch date ");
            this.Pub1.AddTDTitle("nowarp=true", " Sponsor ");
            this.Pub1.AddTDTitle("nowarp=true", " Operating ");
            this.Pub1.AddTREnd();

            string sql = "  SELECT a.WorkID FROM WF_GenerWorkFlow A, WF_GenerWorkerlist B  WHERE A.WorkID=B.WorkID AND B.FK_Emp='" + BP.Web.WebUser.No + "' AND B.IsEnable=1 AND B.IsPass=1 AND b.FK_Flow='" + this.FK_Flow + "'";
            //this.Response.Write(sql);

            GenerWorkFlows gwfs = new GenerWorkFlows();
            gwfs.RetrieveInSQL(GenerWorkFlowAttr.WorkID, "(" + sql + ")");
            int i = 0;
            bool is1 = false;
            string FromPageType = BP.WF.Glo.FromPageType;
            foreach (GenerWorkFlow gwf in gwfs)
            {
                i++;
                is1 = this.Pub1.AddTR(is1);
                this.Pub1.AddTDIdx(i);
                this.Pub1.AddTDA("MyFlow" + this.PageSmall + ".aspx?WorkID=" + gwf.WorkID + "&FK_Flow=" + gwf.FK_Flow, gwf.Title);
                this.Pub1.AddTD(gwf.NodeName);
                this.Pub1.AddTD(gwf.RDT);
                this.Pub1.AddTD(gwf.StarterName);

                this.Pub1.AddTDBegin();
                this.Pub1.Add("<a href=\"javascript:Do(' You acknowledge that you ?','MyFlowInfo" + this.PageSmall + ".aspx?DoType=UnSend&FID=" + gwf.FID + "&WorkID=" + gwf.WorkID + "&FK_Flow=" + gwf.FK_Flow + "');\" ><img src='Img/Btn/delete.gif' border=0 /> Cancel </a>");
                this.Pub1.Add("-<a href=\"javascript:WinOpen('WFRpt.aspx?WorkID=" + gwf.WorkID + "&FK_Flow=" + gwf.FK_Flow + "&FID=0')\" ><img src='Img/Btn/rpt.gif' border=0 /> Report </a>");
                this.Pub1.Add("-<a href=\"javascript:WinOpen('Chart.aspx?WorkID=" + gwf.WorkID + "&FK_Flow=" + gwf.FK_Flow + "&FID=0')\" ><img src='Img/Track.png' border=0 /> Locus </a>");
                this.Pub1.AddTDEnd();
                this.Pub1.AddTREnd();
            }
            this.Pub1.AddTRSum();
            this.Pub1.AddTD("colspan=6", "&nbsp;");
            this.Pub1.AddTREnd();
            this.Pub1.AddTableEnd();
        }
        #endregion

        #region  Variable 
        ToolBar toolbar = null;
        public Flow currFlow = null;
        public Work currWK = null;
        public BP.WF.Node currND = null;
        public GenerWorkFlow gwf = null;
        #endregion

        #region Page load  Event 
        /// <summary>
        ///  Load Event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.Width = "900";

            #region  Check whether the user is disabled .
            try
            {
                string userNo = this.Request.QueryString["UserNo"];
                string no = BP.Web.WebUser.No;
            }
            catch (Exception ex)
            {
                this.ToMsg("@ Login Information WebUser.No Lose , Please log in again ." + ex.Message, "Info");
                return;
            }

            try
            {
                string name = BP.Web.WebUser.Name;
            }
            catch (Exception ex)
            {
                this.ToMsg("@ Login Information WebUser.Name Lose , Please log in again . Error Messages :" + ex.Message, "Info");
                return;
            }

            if (BP.WF.Glo.IsEnableCheckUseSta == true)
            {
                if (BP.WF.Glo.CheckIsEnableWFEmp() == false)
                {
                    this.ToMsg("<font color=red> Your account has been disabled , If you have questions, please contact the administrator .</font>", "Info");
                    BP.Web.WebUser.Exit();
                    return;
                }
            }

            if (this.DoType != null)
            {
                DoDoType();
                return;
            }
            #endregion  Check whether the user is disabled 

            #region  Determine whether there is IsRead
            try
            {
                if (this.IsCC)
                {
                    /* If CC , CC is set to read the status of .*/
                    if (this.Request.QueryString["IsRead"] == "0")
                        BP.WF.Dev2Interface.Node_CC_SetRead(this.FK_Node, this.WorkID, BP.Web.WebUser.No);
                }
                else
                {
                    /* Normal operation processing */
                    if (this.Request.QueryString["IsRead"] == "0")
                        BP.WF.Dev2Interface.Node_SetWorkRead(this.FK_Node, this.WorkID);
                }
            }
            catch (Exception ex)
            {
                this.ToMsg(" Set read status error ", ex.Message);
                return;
            }
            #endregion

            #region  Pre-judgment navigation .
            this.currFlow = new Flow(this.FK_Flow);
            this.currND = new BP.WF.Node(this.FK_Node);
            if (this.WorkID == 0 && this.currND.IsStartNode && this.Request.QueryString["IsCheckGuide"] == null)
            {
                switch (this.currFlow.StartGuideWay)
                {
                    case StartGuideWay.None:
                        break;
                    case StartGuideWay.BySystemUrlMulti:
                    case StartGuideWay.BySystemUrlMultiEntity:
                        this.Response.Redirect("StartGuide.aspx?FK_Flow=" + this.currFlow.No, true);
                        break;
                    case StartGuideWay.ByHistoryUrl: //  Historical Data .
                        if (this.currFlow.IsLoadPriData == true)
                        {
                            this.ToMsg(" Process configuration error , You can not enable the front navigation , Data is automatically loaded on a two functions .", "Info");
                            return;
                        }

                        this.Response.Redirect("StartGuide.aspx?FK_Flow=" + this.currFlow.No, true);
                        break;
                    case StartGuideWay.BySystemUrlOneEntity:
                    case StartGuideWay.BySystemUrlOne:
                        this.Response.Redirect("StartGuideEntities.aspx?FK_Flow=" + this.currFlow.No, true);
                        return;
                    case StartGuideWay.BySelfUrl:
                        this.Response.Redirect(this.currFlow.StartGuidePara1, true);
                        break;
                    default:
                        break;
                }
            }

            string appPath = BP.WF.Glo.CCFlowAppPath; //this.Request.ApplicationPath;
            this.Page.Title = "第" + this.currND.Step + "步:" + this.currND.Name;
            #endregion  Pre-judgment navigation 

            #region  Processed form type .
            if (this.currND.HisFormType == NodeFormType.SheetTree
                || this.currND.HisFormType == NodeFormType.WebOffice)
            {
                /* If it is a multi-form process .*/
                string pFlowNo = this.Request.QueryString["PFlowNo"];
                string pWorkID = this.Request.QueryString["PWorkID"];
                string pNodeID = this.Request.QueryString["PNodeID"];
                string pEmp = this.Request.QueryString["PEmp"];
                if (string.IsNullOrEmpty(pEmp))
                    pEmp = WebUser.No;

                if (this.WorkID == 0)
                {
                    if (string.IsNullOrEmpty(pFlowNo) == true)
                        this.WorkID = BP.WF.Dev2Interface.Node_CreateBlankWork(this.FK_Flow, null, null, WebUser.No, null);
                    else
                        this.WorkID = BP.WF.Dev2Interface.Node_CreateBlankWork(this.FK_Flow, null, null, WebUser.No, null, Int64.Parse(pWorkID), pFlowNo, int.Parse(pNodeID), null);

                    currWK = currND.HisWork;
                    currWK.OID = this.WorkID;
                    currWK.Retrieve();
                    this.WorkID = currWK.OID;
                }
                else
                {
                    gwf = new GenerWorkFlow();
                    gwf.WorkID = this.WorkID;
                    gwf.RetrieveFromDBSources();
                    this.CWorkID = gwf.CWorkID;
                    pFlowNo = gwf.PFlowNo;
                    pWorkID = gwf.PWorkID.ToString();
                }

                string toUrl = "";
                if (this.currND.HisFormType == NodeFormType.SheetTree)
                    toUrl = "./FlowFormTree/Default.aspx?WorkID=" + this.WorkID + "&FK_Flow=" + this.FK_Flow + "&UserNo=" + WebUser.No + "&FID=" + this.FID + "&SID=" + WebUser.SID + "&CWorkID=" + this.CWorkID + "&PFlowNo=" + pFlowNo + "&PWorkID=" + pWorkID;
                else
                    toUrl = "./WebOffice/Default.aspx?WorkID=" + this.WorkID + "&FK_Flow=" + this.FK_Flow + "&UserNo=" + WebUser.No + "&FID=" + this.FID + "&SID=" + WebUser.SID + "&CWorkID=" + this.CWorkID + "&PFlowNo=" + pFlowNo + "&PWorkID=" + pWorkID;

                string[] ps = this.RequestParas.Split('&');

                foreach (string s in ps)
                {
                    if (string.IsNullOrEmpty(s))
                        continue;
                    if (toUrl.Contains(s))
                        continue;
                    toUrl += "&" + s;
                }

                ////  Setting the parameters of the process to join his son .
                //toUrl += "&DoFunc=" + this.DoFunc;
                //toUrl += "&CFlowNo=" + this.CFlowNo;
                //toUrl += "&Nos=" + this.Nos;
                this.Response.Redirect(toUrl, true);
                return;
            }

            if (this.currND.HisFormType == NodeFormType.SLForm)
            {
                if (this.WorkID == 0)
                {
                    WorkID = BP.WF.Dev2Interface.Node_CreateBlankWork(this.FK_Flow, null, null, WebUser.No, null);
                    currWK = currND.HisWork;
                    currWK.OID = this.WorkID;
                    currWK.Retrieve();
                    //this.currFlow.NewWork();
                    this.WorkID = currWK.OID;
                }
                this.Response.Redirect("MyFlowSL.aspx?WorkID=" + this.WorkID + "&FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.FK_Node + "&UserNo=" + WebUser.No + "&CWorkID=" + this.CWorkID, true);
                return;
            }

            if (this.currND.HisFormType == NodeFormType.SDKForm)
            {

                if (this.WorkID == 0)
                {
                    currWK = this.currFlow.NewWork();
                    this.WorkID = currWK.OID;
                }

                string url = currND.FormUrl;
                if (string.IsNullOrEmpty(url))
                {
                    this.ToMsg(" Set read like process design error state error ", " There is no set form url.");
                    return;
                }

                string urlExt = this.RequestParas;

                // Prevent queries less .
                urlExt = urlExt.Replace("?WorkID=", "&WorkID=");
                if (urlExt.Contains("&WorkID") == false)
                {
                    urlExt += "&WorkID=" + this.WorkID;
                }
                else
                {
                    urlExt = urlExt.Replace("&WorkID=0", "&WorkID=" + this.WorkID);
                    urlExt = urlExt.Replace("&WorkID=&", "&WorkID=" + this.WorkID + "&");
                }

                urlExt += "&CWorkID=" + this.CWorkID;

                if (urlExt.Contains("&NodeID") == false)
                    urlExt += "&NodeID=" + currND.NodeID;

                if (urlExt.Contains("FK_Node") == false)
                    urlExt += "&FK_Node=" + currND.NodeID;

                if (urlExt.Contains("&FID") == false)
                    urlExt += "&FID=" + currWK.FID;

                if (urlExt.Contains("&UserNo") == false)
                    urlExt += "&UserNo=" + WebUser.No;

                if (urlExt.Contains("&SID") == false)
                    urlExt += "&SID=" + WebUser.SID;

                if (url.Contains("?") == true)
                    url += "&" + urlExt;
                else
                    url += "?" + urlExt;

                url = url.Replace("?&", "?");
                this.Response.Redirect(url, true);
                return;
            }
            #endregion  Processed form type .

            #region  Determine whether there is  workid
            bool isAskFor = false;
            if (this.WorkID == 0)
            {
                currWK = this.currFlow.NewWork();
                this.WorkID = currWK.OID;
            }
            else
            {
                currWK = this.currFlow.GenerWork(this.WorkID, this.currND);
                gwf = new GenerWorkFlow();
                gwf.WorkID = this.WorkID;
                gwf.RetrieveFromDBSources();
                if (BP.WF.Glo.IsEnableTaskPool && gwf.TaskSta == TaskSta.Takeback)
                {
                    /* If the task pool status , And was removed , To check the person is not removed himself .*/
                }

                this.CWorkID = gwf.CWorkID;

                string msg = "";
                switch (gwf.WFState)
                {
                    case WFState.AskForReplay: //  Returns information plus sign .
                        string mysql = "SELECT * FROM ND" + int.Parse(this.FK_Flow) + "Track WHERE WorkID=" + this.WorkID + " AND " + TrackAttr.ActionType + "=" + (int)ActionType.ForwardAskfor;
                        DataTable mydt = BP.DA.DBAccess.RunSQLReturnTable(mysql);
                        foreach (DataRow dr in mydt.Rows)
                        {
                            string msgAskFor = dr[TrackAttr.Msg].ToString();
                            string worker = dr[TrackAttr.EmpFrom].ToString();
                            string workerName = dr[TrackAttr.EmpFromT].ToString();
                            string rdt = dr[TrackAttr.RDT].ToString();

                            // Message .
                            this.FlowMsg.AlertMsg_Info(worker + "," + workerName + " Reply message :",
                                DataType.ParseText2Html(msgAskFor) + "<br>" + rdt);
                        }
                        break;
                    case WFState.Askfor: // Plus sign .
                        string sql = "SELECT * FROM ND" + int.Parse(this.FK_Flow) + "Track WHERE WorkID=" + this.WorkID + " AND " + TrackAttr.ActionType + "=" + (int)ActionType.AskforHelp;
                        DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
                        foreach (DataRow dr in dt.Rows)
                        {
                            string msgAskFor = dr[TrackAttr.Msg].ToString();
                            string worker = dr[TrackAttr.EmpFrom].ToString();
                            string workerName = dr[TrackAttr.EmpFromT].ToString();
                            string rdt = dr[TrackAttr.RDT].ToString();

                            // Message .
                            this.FlowMsg.AlertMsg_Info(worker + "," + workerName + " Request for endorsement :",
                                DataType.ParseText2Html(msgAskFor) + "<br>" + rdt + " --<a href='./WorkOpt/AskForRe.aspx?FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.FK_Node + "&WorkID=" + this.WorkID + "&FID=" + this.FID + "' > Reply endorsement views </a> --");
                        }
                        isAskFor = true;
                        break;
                    case WFState.ReturnSta:
                        /*  If the worker node to return the */
                        ReturnWorks rws = new ReturnWorks();
                        rws.Retrieve(ReturnWorkAttr.ReturnToNode, this.FK_Node,
                            ReturnWorkAttr.WorkID, this.WorkID,
                            ReturnWorkAttr.RDT);
                        if (rws.Count != 0)
                        {
                            string msgInfo = "";
                            foreach (BP.WF.ReturnWork rw in rws)
                            {
                                msgInfo += "<fieldset width='100%' ><legend>&nbsp;  From node :" + rw.ReturnNodeName + "  Return man :" + rw.ReturnerName + "  " + rw.RDT + "&nbsp;<a href='" + appPath + "DataUser/ReturnLog/" + this.FK_Flow + "/" + rw.MyPK + ".htm' target=_blank> Work Log </a></legend>";
                                msgInfo += rw.NoteHtml;
                                msgInfo += "</fieldset>";
                            }
                            this.FlowMsg.AlertMsg_Info(" Prompt refund process ", msgInfo);
                            //gwf.WFState = WFState.Runing;
                            //gwf.DirectUpdate();
                        }
                        break;
                    case WFState.Shift:
                        /*  Judge handed over . */
                        ShiftWorks fws = new ShiftWorks();
                        BP.En.QueryObject qo = new QueryObject(fws);
                        qo.AddWhere(ShiftWorkAttr.WorkID, this.WorkID);
                        qo.addAnd();
                        qo.AddWhere(ShiftWorkAttr.FK_Node, this.FK_Node);
                        qo.addOrderBy(ShiftWorkAttr.RDT);
                        qo.DoQuery();
                        if (fws.Count >= 1)
                        {
                            this.FlowMsg.AddFieldSet(" Historical information transfer ");
                            foreach (ShiftWork fw in fws)
                            {
                                msg = "@ Handed people [" + fw.FK_Emp + "," + fw.FK_EmpName + "].@ Recipient :" + fw.ToEmp + "," + fw.ToEmpName + ".<br> Transfer reason @" + fw.NoteHtml;
                                if (fw.FK_Emp == WebUser.No)
                                    msg = "<b>" + msg + "</b>";

                                msg = msg.Replace("@", "<br>@");
                                this.FlowMsg.Add(msg + "<hr>");
                            }
                            this.FlowMsg.AddFieldSetEnd();
                        }
                        gwf.WFState = WFState.Runing;
                        gwf.DirectUpdate();
                        break;
                    default:
                        break;
                }
            }
            #endregion  Determine whether there is workid

            #region  Judgment permissions  toolbar.
            if (this.IsPostBack == false)
            {
                if (this.IsCC == false && currND.IsStartNode == false && Dev2Interface.Flow_IsCanDoCurrentWork(this.FK_Flow, this.FK_Node, this.WorkID, WebUser.No) == false)
                {
                    this.ToMsg(" @ The current work has been processed , Or you do not have permission to perform this work .", "Info");
                    return;
                }
            }
            #endregion  Judgment permissions 

            #region  Deal with ctrl Show 
            this.ToolBar1.Visible = false;
            this.ToolBar2.Visible = false;
            if (BP.WF.Glo.FlowCtrlBtnPos == "Top")
            {
                this.ToolBar1.Visible = true;
                toolbar = this.ToolBar1;
            }
            else
            {
                this.ToolBar2.Visible = true;
                toolbar = this.ToolBar2;
            }

            try
            {
                // Initialization controls .
                this.InitToolbar(isAskFor, appPath);
                this.BindWork(currND, currWK);
                this.Session["Ect"] = null;
            }
            catch (Exception ex)
            {
                #region  Solve start node database field changes repair database problems  .
                string rowUrl = this.Request.RawUrl;
                if (rowUrl.IndexOf("rowUrl") > 1)
                {
                }
                else
                {
                    this.Response.Redirect(rowUrl + "&rowUrl=1", true);
                    return;
                }
                #endregion

                this.FlowMsg.DivInfoBlock(ex.Message);
                string Ect = this.Session["Ect"] as string;
                if (Ect == null)
                    Ect = "0";
                if (int.Parse(Ect) < 2)
                {
                    this.Session["Ect"] = int.Parse(Ect) + 1;
                    return;
                }
                return;
            }
            #endregion  Deal with ctrl Show 

        }
        #endregion

        #region  Public Methods 
        /// <summary>
        /// BindWork
        /// </summary>
        public void BindWork(BP.WF.Node nd, Work wk)
        {
            if (nd.IsStartNode)
            {
                /* If this is the start node ,  First check whether the flow restriction is enabled .*/
                if (BP.WF.Glo.CheckIsCanStartFlow_InitStartFlow(this.currFlow, wk) == false)
                {
                    /*  If the restriction is enabled to put out information tips . */
                    this.ToMsg(BP.WF.Glo.DealExp(this.currFlow.StartLimitAlert, wk, null), "Info");
                    return;
                }
            }

            if (nd.HisFlow.IsMD5 && nd.IsStartNode == false && wk.IsPassCheckMD5() == false)
            {
                this.ToMsg("<font color=red> Data has been illegally tampered , Please notify an administrator to solve the problem .</font>", "Info");
                //this.UCEn1.AddMsgOfWarning(" Error ", "<h2><font color=red> Data has been illegally tampered , Please notify an administrator to solve the problem .</font></h2>");
                //this.ToolBar1.EnableAllBtn(false);
                //this.ToolBar1.Clear();
                return;
            }

            if (this.IsPostBack == true)
                this.UCEn1.IsLoadData = false;
            else
                this.UCEn1.IsLoadData = true;

            if (nd.IsStartNode)
            {
                try
                {
                    string billNo = wk.GetValStringByKey(NDXRptBaseAttr.BillNo);
                    if (string.IsNullOrEmpty(billNo) && nd.HisFlow.BillNoFormat.Length > 2)
                    {
                        /* So he automatically generated number */
                        wk.SetValByKey(NDXRptBaseAttr.BillNo,
                            BP.WF.Glo.GenerBillNo(nd.HisFlow.BillNoFormat, this.WorkID, wk, nd.HisFlow.PTable));
                    }
                }
                catch
                {
                    //  May not billNo This field , Do not need to deal with it .
                }
            }
            switch (nd.HisNodeWorkType)
            {
                case NodeWorkType.StartWorkFL:
                case NodeWorkType.WorkFHL:
                case NodeWorkType.WorkFL:
                case NodeWorkType.WorkHL:
                    if (this.FID != 0 && this.FID != this.WorkID)
                    {
                        /*  This situation is shunt node to return to the sub-river .*/
                        this.UCEn1.AddFieldSet(" Shunt node return information ");

                        BP.WF.ReturnWork rw = new ReturnWork();
                        rw.Retrieve(ReturnWorkAttr.WorkID, this.WorkID, ReturnWorkAttr.ReturnToNode, nd.NodeID);
                        this.UCEn1.Add(rw.NoteHtml);
                        this.UCEn1.AddHR();
                        //this.UCEn1.addb
                        TextBox tb = new TextBox();
                        tb.ID = "TB_Doc";
                        tb.TextMode = TextBoxMode.MultiLine;
                        tb.Rows = 7;
                        tb.Columns = 50;
                        this.UCEn1.Add(tb);

                        this.UCEn1.AddBR();
                        Btn btn = new Btn();
                        btn.ID = "Btn_Reject";
                        btn.Text = " Dismissed the work ";
                        btn.Click += new EventHandler(ToolBar1_ButtonClick);
                        this.UCEn1.Add(btn);

                        btn = new Btn();
                        btn.ID = "Btn_KillSubFlow";
                        btn.Text = " Termination of work ";
                        btn.Click += new EventHandler(ToolBar1_ButtonClick);
                        this.UCEn1.Add(btn);
                        this.UCEn1.AddFieldSetEnd(); // (" Shunt node return information ");

                        //this.ToolBar1.Controls.Clear();//.Clear();
                        //this.Response.Write("<script language='JavaScript'> DoSubFlowReturn('" + this.FID + "','" + wk.OID + "','" + nd.NodeID + "');</script>");
                        //this.Response.Write("<javascript ></javascript>");
                        return;
                    }
                    break;
                default:
                    break;
            }
            if (nd.IsStartNode)
            {
                /* To determine whether the sub-processes .*/
                if (string.IsNullOrEmpty(this.Request.QueryString["FromNode"]) == false)
                {
                    if (this.PWorkID == 0)
                        throw new Exception(" Process design errors , When the transfer process screwdriver , Did not receive PWorkID Parameters .");

                    ///*  If you come from the main flow  */
                    //int FromNode = int.Parse(this.Request.QueryString["FromNode"]);
                    //BP.WF.Node FromNode_nd = new BP.WF.Node(FromNode);
                    //Work fromWk = FromNode_nd.HisWork;
                    //fromWk.OID = this.PWorkID;
                    //fromWk.RetrieveFromDBSources();
                    //wk.Copy(fromWk);
                    //   wk.FID = this.FID;
                }

                if (this.DoFunc == "SetParentFlow")
                {
                    /* If you need to set the parent process information .*/
                    string cFlowNo = this.CFlowNo;
                    string[] workids = this.WorkIDs.Split(',');
                    int count = workids.Length - 1;
                    this.UCEn1.AddFieldSet(" Group review ", " A total of selected (" + count + ") Sub-processes are merged review , Are :" + this.WorkIDs);
                }
            }

            //  Processing the passed parameter .
            foreach (string k in this.Request.QueryString.AllKeys)
            {
                wk.SetValByKey(k, this.Request.QueryString[k]);
            }



            wk.ResetDefaultVal();

            if (nd.HisFormType == NodeFormType.DisableIt)
                wk.DirectUpdate();
            else
                wk.DirectUpdate(); // Need to go inside to save defaults , Otherwise , It will not cause the current default information store .

            NodeFormType ft = nd.HisFormType;
            if (BP.Web.WebUser.IsWap)
                ft = NodeFormType.FixForm;

            switch (nd.HisFormType)
            {
                case NodeFormType.FreeForm:
                case NodeFormType.DisableIt:
                case NodeFormType.FixForm:
                    Frms frms = nd.HisFrms;
                    if (frms.Count == 0 && nd.HisFormType == NodeFormType.FreeForm)
                    {
                        /*  Only if the node is just a form of . */
                        /*  Add Save the form function , To customize the button calls , Save the form before and after the event execution . */
                        this.UCEn1.Add("\t\n<script type='text/javascript'>");
                        this.UCEn1.Add("\t\n function SaveFormData() {");
                        this.UCEn1.Add("\t\n     var btn = document.getElementById('" + Btn_Save.ClientID + "');");
                        this.UCEn1.Add("\t\n     if (btn) {");
                        this.UCEn1.Add("\t\n         btn.click();");
                        this.UCEn1.Add("\t\n      }");
                        this.UCEn1.Add("\t\n  }");
                        this.UCEn1.Add("\t\n</script>");
                        /*  Freedom Form  */

                        MapData map = new MapData("ND" + FK_Node);
                        Width = map.MaxRight + map.MaxLeft * 2 + 10 + "";
                        if (float.Parse(Width) < 500)
                            Width = "900";
                        string maxLeft = map.MaxLeft + "";
                        string maxRight = map.MaxRight + "";

                        Height = map.FrmH + "";

                        //this.UCEn1.Add("<div id=divCCForm style='width:" + map.FrmW + "px;height:" + map.FrmH + "px' >");
                        this.UCEn1.Add("<div id=divCCForm style='width:" + Width + "px;height:" + Height + "px' >");
                        this.UCEn1.BindCCForm(wk, "ND" + nd.NodeID, false, 0); //, false, false, null);
                        if (wk.WorkEndInfo.Length > 2)
                            this.Pub3.Add(wk.WorkEndInfo);
                        this.UCEn1.Add("</div>");

                    }
                    else if (frms.Count == 0 && nd.HisFormType == NodeFormType.FixForm)
                    {
                        /*  Only if the node is just a form of . */
                        /* Fool form */
                        MapData map = new MapData("ND" + FK_Node);
                        if (map.TableWidth.Contains("px"))
                            Width = map.TableWidth.Replace("px", "");
                        else
                            Width = map.TableWidth + "";
                        if (map.TableWidth.Equals("100%"))
                            Width = "900";


                        Height = map.FrmH + "";
                        this.UCEn1.Add("<div id=divCCForm style='width:" + Width + "px;height:" + Height + "px;overflow-x:scroll;' >");
                        this.UCEn1.BindColumn4(wk, "ND" + nd.NodeID); //, false, false, null);
                        if (wk.WorkEndInfo.Length > 2)
                            this.Pub3.Add(wk.WorkEndInfo);
                        this.UCEn1.Add("</div>");

                    }
                    else
                    {
                        /*  Forms Forms case mix and process node exists .  */
                        // Hide the Save button 
                        if (this.ToolBar1.IsExit(BP.Web.Controls.NamesOfBtn.Save) == true)
                            this.Btn_Save.Visible = false;

                        //  Allowed direct update, To accept external information passed over .
                        if (nd.HisFormType != NodeFormType.DisableIt)
                            wk.DirectUpdate();

                        /* Cases involving multiple forms ...*/
                        if (nd.HisFormType != NodeFormType.DisableIt)
                        {
                            Frm myfrm = new Frm();
                            myfrm.No = "ND" + nd.NodeID;
                            myfrm.Name = wk.EnDesc;
                            //myfrm.HisFormType = nd.HisFormType;
                            myfrm.HisFormRunType = (FormRunType)(int)nd.HisFormType;

                            FrmNode fnNode = new FrmNode();
                            fnNode.FK_Frm = myfrm.No;
                            fnNode.IsEdit = true;
                            fnNode.IsPrint = false;
                            switch (nd.HisFormType)
                            {
                                case NodeFormType.FixForm:
                                    fnNode.HisFrmType = FrmType.Column4Frm;
                                    break;
                                case NodeFormType.FreeForm:
                                    fnNode.HisFrmType = FrmType.AspxFrm;
                                    break;
                                case NodeFormType.SelfForm:
                                    fnNode.HisFrmType = FrmType.Url;
                                    break;
                                default:
                                    throw new Exception(" Appeared not to judge exception .");
                            }
                            myfrm.HisFrmNode = fnNode;
                            frms.AddEntity(myfrm, 0);
                        }


                        Int64 fid = this.FID;
                        if (this.FID == 0)
                            fid = this.WorkID;

                        if (frms.Count == 1)
                        {
                            /*  Situation with only one process forms . */
                            Frm frm = (Frm)frms[0];
                            FrmNode fn = frm.HisFrmNode;
                            string src = "";
                            #region update by dgq  A form can also add tab页
                            //src = "./CCForm/" + fn.FrmUrl + ".aspx?FK_MapData=" + frm.No + "&FID=" + fid + "&IsEdit=" + fn.IsEditInt + "&IsPrint=" + fn.IsPrintInt + "&FK_Node=" + nd.NodeID + "&WorkID=" + this.WorkID;
                            //this.UCEn1.Add("\t\n <DIV id='" + frm.No + "' style='width:" + frm.FrmW + "px; height:" + frm.FrmH + "px;text-align: left;' >");
                            //this.UCEn1.Add("\t\n <iframe ID='F" + frm.No + "' src='" + src + "' frameborder=0  style='position:absolute;width:" + frm.FrmW + "px; height:" + frm.FrmH + "px;text-align: left;'  leftMargin='0'  topMargin='0'  /></iframe>");
                            //this.UCEn1.Add("\t\n </DIV>");

                            //this.UCEn1.Add("\t\n<script type='text/javascript'>");
                            //this.UCEn1.Add("\t\n function SaveDtlAll(){}");
                            //this.UCEn1.Add("\t\n</script>");
                            #endregion

                            //********************BEGIN****************************
                            #region  Loading documents .
                            this.Page.RegisterClientScriptBlock("sg", "<link href='" + BP.WF.Glo.CCFlowAppPath + "WF/Style/Frm/Tab.css' rel='stylesheet' type='text/css' />");
                            this.Page.RegisterClientScriptBlock("s2g4", "<script language='JavaScript' src='" + BP.WF.Glo.CCFlowAppPath + "WF/Style/Frm/jquery.min.js' ></script>");
                            this.Page.RegisterClientScriptBlock("sdf24j", "<script language='JavaScript' src='" + BP.WF.Glo.CCFlowAppPath + "WF/Style/Frm/jquery.idTabs.min.js' ></script>");
                            this.Page.RegisterClientScriptBlock("sdsdf24j", "<script language='JavaScript' src='" + BP.WF.Glo.CCFlowAppPath + "WF/Style/Frm/TabClick.js' ></script>");
                            #endregion  Loading documents .

                            #region  Parameters .
                            string urlExtFrm = this.RequestParas;
                            if (urlExtFrm.Contains("WorkID") == false)
                                urlExtFrm += "&WorkID=" + this.WorkID;

                            if (urlExtFrm.Contains("NodeID") == false)
                                urlExtFrm += "&NodeID=" + nd.NodeID;

                            if (urlExtFrm.Contains("FK_Node") == false)
                                urlExtFrm += "&FK_Node=" + nd.NodeID;

                            if (urlExtFrm.Contains("UserNo") == false)
                                urlExtFrm += "&UserNo=" + WebUser.No;

                            if (urlExtFrm.Contains("SID") == false)
                                urlExtFrm += "&SID=" + WebUser.SID;

                            if (urlExtFrm.Contains("IsLoadData") == false)
                                urlExtFrm += "&IsLoadData=1";

                            #endregion  Loading documents .

                            src = fn.FrmUrl + ".aspx?FK_MapData=" + frm.No + "&FID=" + fid + "&IsEdit=" + fn.IsEditInt + "&IsPrint=" + fn.IsPrintInt + urlExtFrm;

                            Width = frm.FrmW + "";
                            this.UCEn1.Add("\t\n<div  id='usual2'  class='usual' style='width:" + frm.FrmW + "px;height:auto;margin:0 auto;background-color:white;'>");  //begain.

                            #region  Output tab .
                            this.UCEn1.Add("\t\n <ul  class='abc' style='background:red;border-color: #800000;border-width: 10px;' >");
                            this.UCEn1.Add("\t\n<li><a ID='HL" + frm.No + "' href=\"#" + frm.No + "\" onclick=\"TabClick('" + frm.No + "','" + src + "');\" >" + frm.Name + "</a></li>");
                            this.UCEn1.Add("\t\n </ul>");
                            #endregion  Output tab .

                            #region  Output form  iframe  Content .
                            this.UCEn1.Add("\t\n <DIV id='" + frm.No + "' style='width:" + frm.FrmW + "px; height:" + frm.FrmH + "px;text-align: left;' >");
                            this.UCEn1.Add("\t\n <iframe ID='F" + frm.No + "' Onblur=\"OnTabChange('" + frm.No + "',this);\" src='" + src + "' frameborder=0  style='width:" + frm.FrmW + "px; height:" + frm.FrmH + "px;text-align: left;'  leftMargin='0'  topMargin='0'   /></iframe>");
                            this.UCEn1.Add("\t\n </DIV>");
                            #endregion  Output form  iframe  Content .

                            this.UCEn1.Add("\t\n</div>"); // end  usual2

                            //  Select the default settings .
                            this.UCEn1.Add("\t\n<script type='text/javascript'>");
                            this.UCEn1.Add("\t\n  $(\"#usual2 ul\").idTabs(\"" + frm.No + "\");");

                            #region SaveDtlAll
                            this.UCEn1.Add("\t\n function SaveDtlAll(){");
                            this.UCEn1.Add("\t\n   var tabText = document.getElementById('HL" + frm.No + "').innerText;");
                            this.UCEn1.Add("\t\n   var scope = document.getElementById('F" + frm.No + "');");
                            this.UCEn1.Add("\t\n   var lastChar = tabText.substring(tabText.length - 1, tabText.length);");
                            this.UCEn1.Add("\t\n   if (lastChar == \"*\") {");
                            this.UCEn1.Add("\t\n   var contentWidow = scope.contentWindow;");
                            this.UCEn1.Add("\t\n   contentWidow.SaveDtlData();");
                            this.UCEn1.Add("\t\n   }");
                            this.UCEn1.Add("\t\n}");
                            #endregion

                            this.UCEn1.Add("\t\n</script>");
                            //*********************END***************************

                        }
                        else
                        {
                            /*  Forms Forms mix and process nodes exist . */
                            #region  Loading documents .
                            this.Page.RegisterClientScriptBlock("sg", "<link href='" + BP.WF.Glo.CCFlowAppPath + "WF/Style/Frm/Tab.css' rel='stylesheet' type='text/css' />");
                            this.Page.RegisterClientScriptBlock("s2g4", "<script language='JavaScript' src='" + BP.WF.Glo.CCFlowAppPath + "WF/Style/Frm/jquery.min.js' ></script>");
                            this.Page.RegisterClientScriptBlock("sdf24j", "<script language='JavaScript' src='" + BP.WF.Glo.CCFlowAppPath + "WF/Style/Frm/jquery.idTabs.min.js' ></script>");
                            this.Page.RegisterClientScriptBlock("sdsdf24j", "<script language='JavaScript' src='" + BP.WF.Glo.CCFlowAppPath + "WF/Style/Frm/TabClick.js' ></script>");
                            #endregion  Loading documents .

                            #region  Parameters .
                            string urlExtFrm = this.RequestParas;
                            if (urlExtFrm.Contains("WorkID") == false)
                                urlExtFrm += "&WorkID=" + this.WorkID;

                            if (urlExtFrm.Contains("NodeID") == false)
                                urlExtFrm += "&NodeID=" + nd.NodeID;

                            if (urlExtFrm.Contains("FK_Node") == false)
                                urlExtFrm += "&FK_Node=" + nd.NodeID;

                            if (urlExtFrm.Contains("UserNo") == false)
                                urlExtFrm += "&UserNo=" + WebUser.No;

                            if (urlExtFrm.Contains("SID") == false)
                                urlExtFrm += "&SID=" + WebUser.SID;

                            if (urlExtFrm.Contains("IsLoadData") == false)
                                urlExtFrm += "&IsLoadData=1";
                            #endregion  Loading documents .


                            Frm frmFirst = null;
                            foreach (Frm frm in frms)
                            {
                                if (frmFirst == null) frmFirst = frm;

                                if (frmFirst.FrmW < frm.FrmW)
                                    frmFirst = frm;
                            }

                            this.UCEn1.Clear();
                            this.UCEn1.Add("<div  style='clear:both' ></div>"); //
                            this.UCEn1.Add("\t\n<div  id='usual2' class='usual' style='width:" + frmFirst.FrmW + "px;height:auto;margin:0 auto;background-color:white;'>");  //begain.
                            Width = frmFirst.FrmW + "";

                            #region  Output tab .
                            this.UCEn1.Add("\t\n <ul  class='abc' style='background:red;border-color: #800000;border-width: 10px;' >");
                            foreach (Frm frm in frms)
                            {
                                FrmNode fn = frm.HisFrmNode;
                                string src = "";
                                src = fn.FrmUrl + ".aspx?FK_MapData=" + frm.No + "&IsEdit=" + fn.IsEditInt + "&IsPrint=" + fn.IsPrintInt + urlExtFrm;
                                this.UCEn1.Add("\t\n<li><a ID='HL" + frm.No + "' href=\"#" + frm.No + "\" onclick=\"TabClick('" + frm.No + "','" + src + "');\" >" + frm.Name + "</a></li>");
                            }
                            this.UCEn1.Add("\t\n </ul>");
                            #endregion  Output tab .

                            #region  Output form  iframe  Content .
                            foreach (Frm frm in frms)
                            {
                                FrmNode fn = frm.HisFrmNode;
                                this.UCEn1.Add("\t\n <DIV id='" + frm.No + "' style='width:" + frmFirst.FrmW + "px; height:" + frm.FrmH + "px;text-align: left;margin:0px;padding:0px;' >");
                                this.UCEn1.Add("\t\n <iframe ID='F" + frm.No + "' Onblur=\"OnTabChange('" + frm.No + "',this);\" src='loading.htm' frameborder=0  style='margin:0px;padding:0px;width:" + frm.FrmW + "px; height:" + frm.FrmH + "px;text-align: left;' /></iframe>");
                                this.UCEn1.Add("\t\n </DIV>");
                            }
                            #endregion  Output form  iframe  Content .

                            this.UCEn1.Add("\t\n</div>"); // end  usual2

                            //  Select the default settings .
                            this.UCEn1.Add("\t\n<script type='text/javascript'>");
                            this.UCEn1.Add("\t\n  $(\"#usual2 ul\").idTabs(\"" + frms[0].No + "\");");

                            this.UCEn1.Add("\t\n function SaveDtlAll(){}");

                            #region SaveDtlAll
                            this.UCEn1.Add("\t\n function SaveDtlAll(){");
                            this.UCEn1.Add("\t\n   var tabText = document.getElementById('HL' + currentTabId).innerText;");
                            this.UCEn1.Add("\t\n   var scope = document.getElementById('F' + currentTabId);");
                            this.UCEn1.Add("\t\n   var lastChar = tabText.substring(tabText.length - 1, tabText.length);");
                            this.UCEn1.Add("\t\n   if (lastChar == \"*\") {");
                            this.UCEn1.Add("\t\n   var contentWidow = scope.contentWindow;");
                            this.UCEn1.Add("\t\n   contentWidow.SaveDtlData();");
                            this.UCEn1.Add("\t\n   }");
                            this.UCEn1.Add("\t\n}");
                            #endregion

                            this.UCEn1.Add("\t\n</script>");
                        }
                    }
                    return;
                case NodeFormType.SelfForm:
                    wk.Save();
                    if (this.WorkID == 0)
                    {
                        this.WorkID = wk.OID;
                    }
                    string url = nd.FormUrl;
                    string urlExt = this.RequestParas;
                    if (urlExt.Contains("WorkID") == false)
                        urlExt += "&WorkID=" + this.WorkID;

                    if (urlExt.Contains("NodeID") == false)
                        urlExt += "&NodeID=" + nd.NodeID;

                    if (urlExt.Contains("FK_Node") == false)
                        urlExt += "&FK_Node=" + nd.NodeID;

                    if (urlExt.Contains("FID") == false)
                        urlExt += "&FID=" + wk.FID;

                    if (urlExt.Contains("UserNo") == false)
                        urlExt += "&UserNo=" + WebUser.No;

                    if (urlExt.Contains("SID") == false)
                        urlExt += "&SID=" + WebUser.SID;

                    if (url.Contains("?") == true)
                        url += "&" + urlExt;
                    else
                        url += "?" + urlExt;
                    url = url.Replace("?&", "?");


                    //  this.UCEn1.AddIframeExt(url, nd.FrmAttr);
                    this.UCEn1.AddTable("width='93%'  id=ere ");
                    this.UCEn1.Add("<TR id=to >");

                    this.UCEn1.Add("<TD ID='TDWorkPlace' height='700px' >");
                    this.UCEn1.AddIframeAutoSize(url, "FWorkPlace", "TDWorkPlace");
                    this.UCEn1.Add("</TD>");
                    this.UCEn1.Add("</TR>");

                    this.UCEn1.Add("<TR id=er >");
                    this.UCEn1.Add("<TD id=fd >");
                    this.UCEn1.Add(wk.WorkEndInfo);
                    this.UCEn1.Add("</TD>");
                    this.UCEn1.AddTREnd();
                    this.UCEn1.AddTableEnd();

                    //  Select the default settings .
                    this.UCEn1.Add("\t\n<script type='text/javascript'>");
                    this.UCEn1.Add("\t\n function SaveDtlAll(){}");
                    this.UCEn1.Add("\t\n</script>");
                    return;
                case NodeFormType.SDKForm:
                default:
                    throw new Exception("@ Not related to the expansion .");
            }
        }
        /// <summary>
        ///  Generate js.
        /// </summary>
        /// <param name="en"></param>
        public void OutJSAuto(Entity en)
        {
            if (en.EnMap.IsHaveJS == false)
                return;

            Attrs attrs = en.EnMap.Attrs;
            string js = "";
            foreach (Attr attr in attrs)
            {
                if (attr.UIContralType != UIContralType.TB)
                    continue;

                if (attr.IsNum == false)
                    continue;

                string tbID = "TB_" + attr.Key;
                TB tb = this.UCEn1.GetTBByID(tbID);
                if (tb == null)
                    continue;

                tb.Attributes["OnKeyPress"] = "javascript:C();";
                tb.Attributes["onkeyup"] = "javascript:C();";

                if (attr.MyDataType == DataType.AppInt)
                    tb.Attributes["OnKeyDown"] = "javascript:return VirtyInt(this);";
                else
                    tb.Attributes["OnKeyDown"] = "javascript:return VirtyNum(this);";

                //   tb.Attributes["OnKeyDown"] = "javascript:return VirtyNum(this);";

                if (attr.MyDataType == DataType.AppMoney)
                    tb.Attributes["onblur"] = "this.value=VirtyMoney(this.value);";

                if (attr.AutoFullWay == AutoFullWay.Way1_JS)
                {
                    js += attr.Key + "," + attr.AutoFullDoc + "~";
                    tb.Enabled = true;
                }
            }

            string[] strs = js.Split('~');
            ArrayList al = new ArrayList();
            foreach (string str in strs)
            {
                if (str == null || str == "")
                    continue;

                string key = str.Substring(0, str.IndexOf(','));
                string exp = str.Substring(str.IndexOf(',') + 1);

                string left = "\n  document.forms[0].UCEn1_TB_" + key + ".value = ";
                foreach (Attr attr in attrs)
                {
                    exp = exp.Replace("@" + attr.Key, "  parseFloat( document.forms[0].UCEn1_TB_" + attr.Key + ".value.replace( ',' ,  '' ) ) ");
                    exp = exp.Replace("@" + attr.Desc, " parseFloat( document.forms[0].UCEn1_TB_" + attr.Key + ".value.replace( ',' ,  '' ) ) ");
                }
                al.Add(left + exp);
            }
            string body = "";
            foreach (string s in al)
            {
                body += s;
            }
            this.Response.Write("<script language='JavaScript'> function  C(){ " + body + " }  \n </script>");
        }
        /// <summary>
        ///  Show   Association form 
        /// </summary>
        /// <param name="nd"></param>
        public void ShowSheets(BP.WF.Node nd, Work currWk)
        {
            //if (nd.HisFJOpen != FJOpen.None)
            //{
            //    string url = "FileManager.aspx?WorkID=" + this.WorkID + "&FID=" + currWk.FID
            //        + "&FJOpen=" + (int)nd.HisFJOpen + "&FK_Node=" + nd.NodeID;
            //    this.Pub1.Add("<iframe leftMargin='0' topMargin='0' src='" + url + "' width='100%' height='200px' class=iframe name=fm style='border-style:none;' id=fm > </iframe>");
            //}

            //this.Pub1.AddIframe("FileManager.aspx?WorkID="+this.WorkID+"&FID=0&FJOpen=1&FK_Node="+nd.NodeID);

            if (this.Pub1.Controls.Count > 20)
                return;

            //  To set the display .
            string[] strs = nd.ShowSheets.Split('@');

            if (strs.Length >= 1)
                this.Pub1.AddHR();

            foreach (string str in strs)
            {
                if (str == null || str == "")
                    continue;

                int FK_Node = int.Parse(str);
                BP.WF.Node mynd;
                try
                {
                    mynd = new BP.WF.Node(FK_Node);
                }
                catch
                {
                    nd.ShowSheets = nd.ShowSheets.Replace("@" + FK_Node, "");
                    nd.Update();
                    continue;
                }

                Work nwk = mynd.HisWork;
                nwk.OID = this.WorkID;
                if (nwk.RetrieveFromDBSources() == 0)
                    continue;

                // this.Pub1.AddB("== " + mynd.Name + " ==<hr width=90%>");

                this.Pub1.AddFieldSet(" Historic step :" + mynd.Name);
                // this.Pub1.DivInfoBlockBegin();
                // this.Pub1.ADDWork(nwk,nd.NodeID);
                this.Pub1.AddFieldSetEnd(); // (mynd.Name);
                //this.Pub1.DivInfoBlockEnd(); // (mynd.Name);
            }
        }
        #endregion

        #region Web  Form Designer generated code 
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN:  This call is  ASP.NET Web  Form Designer required .
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///  Required method for Designer support  -  Do not use the code editor to modify 
        ///  Contents of this method .
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion

        #region toolbar 2
        private void ToolBar1_ButtonClick(object sender, System.EventArgs e)
        {
            string id = "";
            Btn btn = sender as Btn;
            if (btn != null)
                id = btn.ID;
            switch (btn.ID)
            {
                case "Btn_Reject":
                case "Btn_KillSubFlow":
                    try
                    {
                        WorkFlow wkf = new WorkFlow(this.FK_Flow, this.WorkID);
                        if (btn.ID == "Btn_KillSubFlow")
                        {
                            this.ToMsg(" Delete process information :<hr>" + wkf.DoDeleteWorkFlowByReal(true), "info");
                        }
                        else
                        {
                            string msg = wkf.DoReject(this.FID, this.FK_Node, this.UCEn1.GetTextBoxByID("TB_Doc").Text);
                            this.ToMsg(msg, "info");
                        }
                        return;
                    }
                    catch (Exception ex)
                    {
                        this.ToMsg(ex.Message, "info");
                        return;
                    }

                case NamesOfBtn.Delete:
                case "Btn_Del":
                    //  This is no need to completely remove the interaction .
                    string delMsg = BP.WF.Dev2Interface.Flow_DoDeleteFlowByReal(this.FK_Flow, this.WorkID, true);
                    this.ToMsg(" Delete Process Tips <hr>" + delMsg, "info");
                    break;
                case NamesOfBtn.Save:
                    this.Send(true);
                    if (string.IsNullOrEmpty(this.Request.QueryString["WorkID"]))
                    {
                        //  this.Response.Redirect(this.PageID + ".aspx?FID=" + this.FID + "&WorkID=" + this.WorkID + "&FK_Node=" + this.FK_Node + "&FK_Flow=" + this.FK_Flow + "&FromNode=" + this.FromNode+"&PWorkID="+this.PWorkID, true);
                        return;
                    }
                    break;
                case "Btn_ReturnWork":
                    this.BtnReturnWork();
                    break;
                case BP.Web.Controls.NamesOfBtn.Shift:
                    this.DoShift();
                    break;
                case "Btn_WorkerList":
                    if (WorkID == 0)
                        throw new Exception(" The current work is not specified , You can not view a list of worker .");
                    break;
                case "Btn_PrintWorkRpt":
                    if (WorkID == 0)
                        throw new Exception(" The current work is not specified , Can not print work report .");
                    this.WinOpen("WFRpt.aspx?FK_Flow=" + this.FK_Flow + "&WorkID=" + WorkID, " Report ", 800, 600);
                    break;
                case NamesOfBtn.Send:
                    this.Send(false);
                    break;
                default:
                    break;
            }
            //}
            //catch (Exception ex)
            //{
            //    this.FlowMsg.AlertMsg_Warning(" Information Tips ", ex.Message);
            //}
        }
        #region  Button event 
        /// <summary>
        ///  Save your work 
        /// </summary>
        /// <param name="isDraft"> Is not saved as a draft </param> 
        private void Send(bool isSave)
        {
            //  Determine whether the current staff has execute permission to the officer .
            if (currND.IsStartNode == false
                && BP.WF.Dev2Interface.Flow_IsCanDoCurrentWork(this.FK_Flow, this.FK_Node, this.WorkID, WebUser.No) == false)
                throw new Exception(" Hello :" + WebUser.No + "," + WebUser.Name + ":<br>  The current work has been processed others , You can not save or send in execution !!!");

            Paras ps = new Paras();
            string dtStr = SystemConfig.AppCenterDBVarStr;
            try
            {
                switch (currND.HisFormType)
                {
                    case NodeFormType.SelfForm:
                        break;
                    case NodeFormType.FixForm:
                    case NodeFormType.FreeForm:
                        currWK = (Work)this.UCEn1.Copy(this.currWK);

                        //  Set Default ....
                        MapAttrs mattrs = currND.MapData.MapAttrs;
                        foreach (MapAttr attr in mattrs)
                        {
                            if (attr.TBModel == 2)
                            {
                                /*  If the rich text  */
                                currWK.SetValByKey(attr.KeyOfEn, this.Request.Form["ctl00$ContentPlaceHolder1$MyFlowUC1$MyFlow1$UCEn1$TB_" + attr.KeyOfEn]);
                            }

                            if (attr.UIIsEnable)
                                continue;
                            if (attr.DefValReal.Contains("@") == false)
                                continue;
                            currWK.SetValByKey(attr.KeyOfEn, attr.DefVal);
                        }
                        break;
                    case NodeFormType.DisableIt:
                        currWK.Retrieve();
                        break;
                    default:
                        throw new Exception("@ Without involving the .");
                }
            }
            catch (Exception ex)
            {
                this.Btn_Send.Enabled = true;
                throw new Exception("@ Execution logic to check for errors before saving ." + ex.Message + " @StackTrace:" + ex.StackTrace);
            }

            #region  Analyzing specific business logic 
            string dbStr = SystemConfig.AppCenterDBVarStr;
            if (currND.IsStartNode)
            {
                if (this.currND.HisFlow.HisFlowAppType == FlowAppType.PRJ)
                {
                    /* Check for special processes , Check for permissions .*/
                    string prjNo = currWK.GetValStringByKey("PrjNo");
                    ps = new Paras();
                    ps.SQL = "SELECT * FROM WF_NodeStation WHERE FK_Station IN ( SELECT FK_Station FROM Prj_EmpPrjStation WHERE FK_Prj=" + dbStr + "FK_Prj AND FK_Emp=" + dbStr + "FK_Emp )  AND  FK_Node=" + dbStr + "FK_Node ";
                    ps.Add("FK_Prj", prjNo);
                    ps.AddFK_Emp();
                    ps.Add("FK_Node", this.FK_Node);

                    if (DBAccess.RunSQLReturnTable(ps).Rows.Count == 0)
                    {
                        string prjName = currWK.GetValStringByKey("PrjName");
                        ps = new Paras();
                        ps.SQL = "SELECT * FROM Prj_EmpPrj WHERE FK_Prj=" + dbStr + "FK_Prj AND FK_Emp=" + dbStr + "FK_Emp ";
                        ps.Add("FK_Prj", prjNo);
                        ps.AddFK_Emp();
                        //   ps.AddFK_Emp();

                        if (DBAccess.RunSQLReturnTable(ps).Rows.Count == 0)
                            throw new Exception(" You are not (" + prjNo + "," + prjName + ") Member , You can not initiate a change process .");
                        else
                            throw new Exception(" You belong to this project (" + prjNo + "," + prjName + "), But in this project that you have not initiated the process to change jobs .");
                    }
                }
            }
            #endregion  Analyzing specific business logic .

            currWK.Rec = WebUser.No;
            currWK.SetValByKey("FK_Dept", WebUser.FK_Dept);
            currWK.SetValByKey("FK_NY", BP.DA.DataType.CurrentYearMonth);

            //  Save event processing nodes form .
            currND.MapData.FrmEvents.DoEventNode(FrmEventList.SaveBefore, currWK);
            try
            {
                if (currND.IsStartNode)
                    currWK.FID = 0;

                if (currND.HisFlow.IsMD5)
                {
                    /* Re update md5值.*/
                    currWK.SetValByKey("MD5", BP.WF.Glo.GenerMD5(currWK));
                }

                if (currND.IsStartNode && isSave)
                    currWK.SetValByKey(StartWorkAttr.Title, WorkNode.GenerTitle(currND.HisFlow, this.currWK));

                currWK.Update();
                /* If it is saved */
            }
            catch (Exception ex)
            {
                try
                {
                    currWK.CheckPhysicsTable();
                }
                catch (Exception ex1)
                {
                    throw new Exception("@ Save error :" + ex.Message + "@ Check the physical form error :" + ex1.Message);
                }
                this.Btn_Send.Enabled = true;
                this.Pub1.AlertMsg_Warning(" Error ", ex.Message + "@ There may be automatically fixes this error , Please save the new time .");
                return;
            }

            #region  After saving event processing 
            bool isHaveSaveAfter = false;
            try
            {
                // After processing the form save .
                string s = currND.MapData.FrmEvents.DoEventNode(FrmEventList.SaveAfter, currWK);
                if (s != null)
                {
                    /* If not equal null, Description has been performed data retention , Let it once from the database query .*/
                    currWK.RetrieveFromDBSources();
                    isHaveSaveAfter = true;
                }
            }
            catch (Exception ex)
            {
                //this.Response.Write(ex.Message);
                this.Alert(ex.Message.Replace("'", "["));
                return;
            }
            #endregion

            #region 2012-10-15   Data must be saved to Rpt Exterior and interior .
            if (currND.SaveModel == SaveModel.NDAndRpt)
            {
                /*  If you save mode is the node table with Node与Rpt表. */
                WorkNode wn = new WorkNode(currWK, currND);
                GERpt rptGe = currND.HisFlow.HisGERpt;
                rptGe.SetValByKey("OID", this.WorkID);
                wn.rptGe = rptGe;
                if (rptGe.RetrieveFromDBSources() == 0)
                {
                    rptGe.SetValByKey("OID", this.WorkID);
                    wn.DoCopyRptWork(currWK);

                    rptGe.SetValByKey(GERptAttr.FlowEmps, "@" + WebUser.No + "," + WebUser.Name);
                    rptGe.SetValByKey(GERptAttr.FlowStarter, WebUser.No);
                    rptGe.SetValByKey(GERptAttr.FlowStartRDT, DataType.CurrentDataTime);
                    rptGe.SetValByKey(GERptAttr.WFState, 0);

                    rptGe.WFState = WFState.Draft;

                    rptGe.SetValByKey(GERptAttr.FK_NY, DataType.CurrentYearMonth);
                    rptGe.SetValByKey(GERptAttr.FK_Dept, WebUser.FK_Dept);
                    rptGe.Insert();
                }
                else
                {
                    wn.DoCopyRptWork(currWK);
                    rptGe.Update();
                }
            }
            #endregion

            if (BP.WF.Glo.IsEnableDraft && currND.IsStartNode)
            {
                /* If the draft is enabled ,  And is the start node . */
                BP.WF.Dev2Interface.Node_SaveWork(this.FK_Flow, this.FK_Node, this.WorkID);
            }

            string msg = "";
            //  Call workflow , After processing node information collection after preservation work .
            if (isSave)
            {
                if (isHaveSaveAfter)
                {
                    /* If after save event , Let its rebinding . */
                    currWK.RetrieveFromDBSources();
                    this.UCEn1.ResetEnVal(currWK);
                    return;
                }

                if (string.IsNullOrEmpty(this.Request.QueryString["WorkID"]))
                    return;

                currWK.RetrieveFromDBSources();
                this.UCEn1.ResetEnVal(currWK);
                return;
            }

            // Check whether it is returned ?
            if (gwf.WFState == WFState.ReturnSta && gwf.Paras_IsTrackBack == false)
            {
                /*  If it is returned  */
            }
            else
            {
                if (currND.CondModel == CondModel.ByUserSelected && currND.HisToNDNum > 1)
                {
                    // If the user selects the direction condition .
                    this.Response.Redirect("./WorkOpt/ToNodes.aspx?FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.FK_Node + "&WorkID=" + this.WorkID + "&FID=" + this.FID, true);
                    return;
                }
            }

            // Performing transmission .
            WorkNode firstwn = new WorkNode(this.currWK, this.currND);
            try
            {
                msg = firstwn.NodeSend().ToMsgOfHtml();
            }
            catch (Exception exSend)
            {
                if (exSend.Message.Contains(" Please select the next step to work ") == true)
                {
                    //
                    string url = "./WorkOpt/Accepter.aspx?IsWinOpen=0&CFlowNo=" + this.CFlowNo + "&DoFunc=" + this.DoFunc + "&WorkIDs=" + this.WorkIDs + "&FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.FK_Node + "&WorkID=" + this.WorkID + "&FID=" + this.FID;
                    Page.RegisterClientScriptBlock("aaa", "<script>javascript:WinShowModalDialog_Accepter(\'" + url + "\')</script>");
                    //this.Response.Redirect("./WorkOpt/Accepter.aspx?IsWinOpen=0&CFlowNo="+this.CFlowNo+"&DoFunc=" + this.DoFunc + "&WorkIDs=" + this.WorkIDs + "&FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.FK_Node + "&WorkID=" + this.WorkID + "&FID=" + this.FID, true);
                    return;
                }

                this.FlowMsg.AddFieldSetGreen(" Error ");
                this.FlowMsg.Add(exSend.Message.Replace("@@", "@").Replace("@", "<BR>@"));
                this.FlowMsg.AddFieldSetEnd();
                return;
            }

            #region  Business logic methods to handle the general sent successfully after , This method may throw an exception .
            try
            {
                // Business logic methods to handle the general sent successfully after , This method may throw an exception .
                BP.WF.Glo.DealBuinessAfterSendWork(this.FK_Flow, this.WorkID, this.DoFunc, WorkIDs, this.CFlowNo, 0, null);
            }
            catch (Exception ex)
            {
                this.ToMsg(msg, ex.Message);
                return;
            }
            #endregion  Business logic methods to handle the general sent successfully after , This method may throw an exception .


            this.Btn_Send.Enabled = false;
            /* Processing steering problems .*/
            switch (firstwn.HisNode.HisTurnToDeal)
            {
                case TurnToDeal.SpecUrl:
                    string myurl = firstwn.HisNode.TurnToDealDoc.Clone().ToString();
                    if (myurl.Contains("&") == false)
                        myurl += "?1=1";
                    Attrs myattrs = firstwn.HisWork.EnMap.Attrs;
                    Work hisWK = firstwn.HisWork;
                    foreach (Attr attr in myattrs)
                    {
                        if (myurl.Contains("@") == false)
                            break;
                        myurl = myurl.Replace("@" + attr.Key, hisWK.GetValStrByKey(attr.Key));
                    }
                    if (myurl.Contains("@"))
                        throw new Exception(" Process design errors , In node steering url The parameters are not to be replaced .Url:" + myurl);

                    myurl += "&FromFlow=" + this.FK_Flow + "&FromNode=" + this.FK_Node + "&PWorkID=" + this.WorkID + "&UserNo=" + WebUser.No + "&SID=" + WebUser.SID;
                    this.Response.Redirect(myurl, true);
                    return;
                case TurnToDeal.TurnToByCond:
                    TurnTos tts = new TurnTos(this.FK_Flow);
                    if (tts.Count == 0)
                        throw new Exception("@ You do not turn to the conditions set after the completion of the node .");
                    foreach (TurnTo tt in tts)
                    {
                        tt.HisWork = firstwn.HisWork;
                        if (tt.IsPassed == true)
                        {
                            string url = tt.TurnToURL.Clone().ToString();
                            if (url.Contains("&") == false)
                                url += "?1=1";
                            Attrs attrs = firstwn.HisWork.EnMap.Attrs;
                            Work hisWK1 = firstwn.HisWork;
                            foreach (Attr attr in attrs)
                            {
                                if (url.Contains("@") == false)
                                    break;
                                url = url.Replace("@" + attr.Key, hisWK1.GetValStrByKey(attr.Key));
                            }
                            if (url.Contains("@"))
                                throw new Exception(" Process design errors , In node steering url The parameters are not to be replaced .Url:" + url);

                            url += "&PFlowNo=" + this.FK_Flow + "&FromNode=" + this.FK_Node + "&PWorkID=" + this.WorkID + "&UserNo=" + WebUser.No + "&SID=" + WebUser.SID;
                            this.Response.Redirect(url, true);
                            return;
                        }
                    }

#warning  For Shanghai modify it if you can not find the path information prompted by the system .

                    this.ToMsg(msg, "info");
                    //throw new Exception(" You define the steering condition is not satisfied , No Exit .");
                    break;
                default:
                    this.ToMsg(msg, "info");
                    break;
            }
            return;
        }


        public void ToMsg(string msg, string type)
        {
            this.Session["info"] = msg;
            this.Application["info" + WebUser.No] = msg;
            BP.WF.Glo.SessionMsg = msg;
            this.Response.Redirect("MyFlowInfo" + BP.WF.Glo.FromPageType + ".aspx?FK_Flow=" + this.FK_Flow + "&FK_Type=" + type + "&FK_Node=" + this.FK_Node + "&WorkID=" + this.WorkID, false);

            //if (this.PageID.Contains("Single") == true)
            //    this.Response.Redirect("MyFlowInfoSmallSingle.aspx?FK_Flow=" + this.FK_Flow + "&FK_Type=" + type + "&FK_Node=" + this.FK_Node + "&WorkID=" + this.WorkID, false);
            //else if (this.PageID.Contains("Small"))
            //    this.Response.Redirect("MyFlowInfo.aspx?FK_Flow=" + this.FK_Flow + "&FK_Type=" + type + "&FK_Node=" + this.FK_Node + "&WorkID=" + this.WorkID, false);
            //else
            //    this.Response.Redirect("MyFlowInfo.aspx?FK_Flow=" + this.FK_Flow + "&FK_Type=" + type + "&FK_Node=" + this.FK_Node + "&WorkID=" + this.WorkID, false);
        }
        public void BtnReturnWork()
        {
            BP.WF.Node nd = new BP.WF.Node(this.FK_Node);
            Work wk = nd.HisWork;
            wk.OID = this.WorkID;
            wk.Retrieve();
            wk = (Work)this.UCEn1.Copy(wk);

            string msg = BP.WF.Glo.DealExp(nd.FocusField, wk, null);
            this.Response.Redirect("./WorkOpt/ReturnWork.aspx?FK_Node=" + this.FK_Node + "&FID=" + wk.FID + "&WorkID=" + this.WorkID + "&FK_Flow=" + this.FK_Flow + "&Info=" + msg, true);
            return;
        }
        public void DoShift()
        {
            //GenerWorkFlow gwf = new GenerWorkFlow();
            //if (gwf.Retrieve(GenerWorkFlowAttr.WorkID, this.WorkID) == 0)
            //{
            //    this.Alert(" Work has not yet issued , You can not transfer .");
            //    return;
            //}

            string msg = "";
            BP.WF.Node nd = new BP.WF.Node(gwf.FK_Node);
            if (nd.FocusField != "")
            {
                Work wk = nd.HisWork;
                wk.OID = this.WorkID;
                wk.Retrieve();
                msg = BP.WF.Glo.DealExp(nd.FocusField, wk, null);
                // wk.Update(nd.FocusField, msg);
            }
            string url = "./WorkOpt/Forward.aspx?FK_Node=" + this.FK_Node + "&WorkID=" + this.WorkID + "&FID=" + this.FID + "&FK_Flow=" + this.FK_Flow + "&Info=" + msg;
            this.Response.Redirect(url, true);
        }
        #endregion

        #endregion

    }
}