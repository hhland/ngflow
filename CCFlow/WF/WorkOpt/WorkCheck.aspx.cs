using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.DA;
using BP.WF.Template;
using BP.Sys;

namespace CCFlow.WF.WorkOpt
{
    public partial class FrmWorkCheck : System.Web.UI.Page
    {
        #region  Property 
        public bool IsHidden
        {
            get
            {
                try
                {
                    if (DoType == "View")
                        return true;
                    return bool.Parse(Request["IsHidden"]);
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        public int NodeID
        {
            get
            {
                try
                {
                    return int.Parse(this.Request.QueryString["FK_Node"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        ///  The work ID
        /// </summary>
        public Int64 WorkID
        {
            get
            {
                string workid = this.Request.QueryString["OID"];
                if (workid == null)
                    workid = this.Request.QueryString["WorkID"];
                return Int64.Parse(workid);
            }
        }
        public Int64 FID
        {
            get
            {
                string workid = this.Request.QueryString["FID"];
                if (string.IsNullOrEmpty(workid) == true)
                    return 0;
                return Int64.Parse(workid);
            }
        }

        /// <summary>
        ///  Process ID 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }

        /// <summary>
        ///  Operating View
        /// </summary>
        public string DoType
        {
            get
            {
                return this.Request.QueryString["DoType"];
            }
        }
        /// <summary>
        ///  Is CC .
        /// </summary>
        public bool IsCC
        {
            get
            {
                string s = this.Request.QueryString["Paras"];
                if (s == null)
                    return false;

                if (s.Contains("IsCC") == true)
                    return true;
                return false;
            }
        }


        #endregion  Property 

        protected void Page_Load(object sender, EventArgs e)
        {
            // Binding workflow empty frame number does not exist .
            if (this.FK_Flow == null)
            {
                ViewEmptyForm();
                return;
            }

            // Approval node .
            BP.Sys.FrmWorkCheck wcDesc = new BP.Sys.FrmWorkCheck(this.NodeID);
            if (wcDesc.HisFrmWorkShowModel == BP.Sys.FrmWorkShowModel.Free)
                this.BindFreeModel(wcDesc);
            else
                this.BindFreeModel(wcDesc);

            // this.BindTableModel(wcDesc);
        }
        /// <summary>
        ///  Achieve the function :
        /// 1, Displays the path table .
        /// 2, If you enable auditing , Put the audit information displayed .
        /// 3, If you enable the CC , Cc put people show up .
        /// 4, You can handle the resulting information flow and processing displayed .
        /// 5, You can handle the information listed thread .
        /// 6, You can reach the next node processing people show up .
        /// </summary>
        /// <param name="wcDesc"></param>
        public void BindFreeModel(BP.Sys.FrmWorkCheck wcDesc)
        {
            BP.WF.WorkCheck wc = null;
            if (FID != 0)
                wc = new WorkCheck(this.FK_Flow, this.NodeID, this.FID, 0);
            else
                wc = new WorkCheck(this.FK_Flow, this.NodeID, this.WorkID, this.FID);

            bool isCanDo = BP.WF.Dev2Interface.Flow_IsCanDoCurrentWork(this.FK_Flow, this.NodeID, this.WorkID,
                BP.Web.WebUser.No);

            #region  Processing audit opinion box .
            if (IsHidden == false && wcDesc.HisFrmWorkCheckSta == BP.Sys.FrmWorkCheckSta.Enable && isCanDo)
            {
                this.Pub1.AddTable("border=0 style='padding:0px;width:100%;' leftMargin=0 topMargin=0");
                this.Pub1.AddTR();
                this.Pub1.AddTDTitle("<div style='float:left'>" + wcDesc.FWCOpLabel + "</div><div style='float:right'><a href=javascript:TBHelp('TB_Doc')><img src='" + BP.WF.Glo.CCFlowAppPath + "WF/Img/Emps.gif' align='middle' border=0 /> Choose vocabulary </a>&nbsp;&nbsp;</div>");
                this.Pub1.AddTREnd();

                PostBackTextBox tb = new PostBackTextBox();
                tb.ID = "TB_Doc";
                tb.TextMode = TextBoxMode.MultiLine;
                tb.OnBlur += new EventHandler(btn_Click);

                tb.Style["width"] = "100%";
                tb.Rows = 3;
                if (DoType != null && DoType == "View")
                {
                    tb.ReadOnly = true;
                }

                tb.Text = BP.WF.Dev2Interface.GetCheckInfo(this.FK_Flow, this.WorkID, this.NodeID);

                if (tb.Text == " Agree ")
                    tb.Text = "";

                if (string.IsNullOrEmpty(tb.Text))
                {
                    tb.Text = wcDesc.FWCDefInfo;

                    //  The following are not going to deal with the phone side 
                    if (this.IsCC)
                    {
                        /* If the current work is CC . */
                        BP.WF.Dev2Interface.WriteTrackWorkCheck(this.FK_Flow, this.NodeID, this.WorkID, this.FID, tb.Text, " Cc ");

                        // Set the current review has been completed .
                        BP.WF.Dev2Interface.Node_CC_SetSta(this.NodeID, this.WorkID, BP.Web.WebUser.No, CCSta.CheckOver);

                    }
                    else
                    {
                        if (wcDesc.FWCIsFullInfo == true)
                            BP.WF.Dev2Interface.WriteTrackWorkCheck(this.FK_Flow, this.NodeID, this.WorkID, this.FID, tb.Text, wcDesc.FWCOpLabel);
                    }
                    //  Not going to deal with over the phone side .

                }
                this.Pub1.AddTR();
                this.Pub1.AddTD(tb);
                this.Pub1.AddTREnd();
                this.Pub1.AddTableEnd();
            }

            if (wcDesc.FWCListEnable == false)
                return;  /*   Historical audit information is displayed ?  Not displayed on return. */

            #endregion  Processing audit opinion box .

            // Seek trajectory table .
            BP.WF.Tracks tks = wc.HisWorkChecks;

            // Seeking CC list , The read status information and Cc Cc displayed .
            CCLists ccls = new CCLists(this.FK_Flow, this.WorkID, this.FID);

            // Check out the next node processing personal information , To facilitate the display does not move to the next node in the trajectory .
            Int64 wfid = this.WorkID;
            if (this.FID != 0)
                wfid = this.FID;

            // Get   Node processing of personal data .
            SelectAccpers accepts = new SelectAccpers(wfid);

            // All nodes taken out of the process .
            Nodes nds = new Nodes(this.FK_Flow);
            Nodes ndsOrder = new Nodes();
            // Obtaining step has occurred .
            string nodes = ""; // Step has occurred .

            foreach (BP.WF.Track tk in tks)
            {
                switch (tk.HisActionType)
                {
                    case ActionType.Forward:
                    case ActionType.WorkCheck:
                        if (nodes.Contains(tk.NDFrom + ",") == false)
                        {
                            //ndsOrder.AddEntity(nds.GetEntityByKey(tk.NDFrom));
                            nodes += tk.NDFrom + ",";
                        }
                        break;
                    case ActionType.StartChildenFlow:
                        if (nodes.Contains(tk.NDFrom + ",") == false)
                        {
                            //ndsOrder.AddEntity(nds.GetEntityByKey(tk.NDFrom));
                            nodes += tk.NDFrom + ",";
                        }
                        break;
                    default:
                        continue;
                }
            }
            this.Pub1.AddTable("border=0 style='padding:0px;width:100%;' leftMargin=0 topMargin=0");

            int biaoji = 0;
            foreach (Node nd in nds)
            {
                if (nodes.Contains(nd.NodeID + ",") == true)
                {
                    /* Has been treated ..*/
                    this.Pub1.AddTR();

                    this.Pub1.AddTDBegin("colspan=4");
                    this.Pub1.AddTable("border=0 style='padding:0px;width:100%;' leftMargin=0 topMargin=0 id='tb" + nd.NodeID + "'");

                    /* Node does not appear .*/
                    this.Pub1.AddTR();
                    this.Pub1.AddTDTitle("colspan=4", "<div style='float:left'><image src='../Img/Tree/Cut.gif' onclick=\"show_and_hide_tr('tb" + nd.NodeID + "',this);\" style='cursor:pointer;'></image>" + nd.Name + "</div>");
                    this.Pub1.AddTREnd();

                    this.Pub1.AddTR("style='font-size:14px;font-weight:bold;'");
                    this.Pub1.AddTD("width='50' style='font-weight:bold;'", " Event ");
                    this.Pub1.AddTD("width='150' style='font-weight:bold;'", " Time ");
                    this.Pub1.AddTD("width='100' style='font-weight:bold;'", " The operator ");
                    this.Pub1.AddTD("style='font-weight:bold;'", " Information ");
                    this.Pub1.AddTREnd();

                    // Send a copy to review the information and output information .
                    string emps = "";
                    string empsorder = "";    // Save queue display of staff , To make a judgment , Avoid duplication display 
                    string empcheck = "";   // Record the current node has output 

                    foreach (Track tk in tks)
                    {
                        if (tk.NDFrom != nd.NodeID)
                            continue;

                        #region  If it is forward , And the current node is not enabled auditing components 
                        if (tk.HisActionType == ActionType.Forward)
                        {
                            BP.Sys.FrmWorkCheck fwc = new BP.Sys.FrmWorkCheck(nd.NodeID);
                            if (fwc.HisFrmWorkCheckSta == BP.Sys.FrmWorkCheckSta.Disable)
                            {
                                this.Pub1.AddTR();
                                this.Pub1.AddTD("<img src='../Img/Mail_Read.png' border=0 />" + tk.ActionTypeText);
                                this.Pub1.AddTD(tk.RDT);

                                if (wcDesc.SigantureEnabel == true)
                                    this.Pub1.AddTD(BP.WF.Glo.GenerUserSigantureHtml(Server.MapPath(""), tk.EmpFrom, tk.EmpFromT));
                                else
                                    this.Pub1.AddTD(BP.WF.Glo.GenerUserImgSmallerHtml(tk.EmpFrom, tk.EmpFromT));

                                this.Pub1.AddTD(tk.MsgHtml);
                                this.Pub1.AddTREnd();
                                continue;
                            }
                        }
                        #endregion

                        if (tk.HisActionType != ActionType.WorkCheck && tk.HisActionType != ActionType.StartChildenFlow)
                            continue;

                        emps += tk.EmpFrom + ",";

                        if (tk.HisActionType == ActionType.WorkCheck)
                        {
                            #region  Those who show up unreviewed queue process .
                            if (nd.TodolistModel == TodolistModel.Order)
                            {
                                /*  If it is necessary to process the queue of those who did not show up audit .*/
                                string empsNodeOrder = "";  // Record staff access node queue currently not implemented 

                                GenerWorkerLists gwls = new GenerWorkerLists(this.WorkID);
                                foreach (GenerWorkerList item in gwls)
                                {
                                    if (item.FK_Node == nd.NodeID)
                                    {
                                        empsNodeOrder += item.FK_Emp;
                                    }
                                }

                                foreach (SelectAccper accper in accepts)
                                {
                                    if (empsorder.Contains(accper.FK_Emp) == true)
                                        continue;
                                    if (empsNodeOrder.Contains(accper.FK_Emp) == false)
                                        continue;
                                    if (tk.EmpFrom == accper.FK_Emp)
                                    {
                                        /* Audit information , First output it .*/
                                        this.Pub1.AddTR();
                                        this.Pub1.AddTD("<img src='../Img/Mail_Read.png' border=0/>" + tk.ActionTypeText);
                                        this.Pub1.AddTD(tk.RDT);
                                        //this.Pub1.AddTD(tk.EmpFromT);
                                        this.Pub1.AddTD(BP.WF.Glo.GenerUserImgSmallerHtml(tk.EmpFrom, tk.EmpFromT));
                                        this.Pub1.AddTD(tk.MsgHtml);
                                        this.Pub1.AddTREnd();
                                        empcheck += tk.EmpFrom;
                                    }
                                    else
                                    {
                                        this.Pub1.AddTR();
                                        if (accper.AccType == 0)
                                            this.Pub1.AddTD("style='color:Red;'", " Carried out ");
                                        else
                                            this.Pub1.AddTD("style='color:Red;'", " Cc ");
                                        this.Pub1.AddTD("style='color:Red;'", "无");
                                        this.Pub1.AddTD("style='color:Red;'", BP.WF.Glo.GenerUserImgSmallerHtml(accper.FK_Emp, accper.EmpName));
                                        this.Pub1.AddTD("style='color:Red;'", accper.Info);
                                        this.Pub1.AddTREnd();
                                        empsorder += accper.FK_Emp;
                                    }
                                }
                            }
                            #endregion  Those who show up unreviewed queue process .
                            else
                            {
                                /* Audit information , First output it .*/
                                this.Pub1.AddTR();
                                this.Pub1.AddTD("<img src='../Img/Mail_Read.png' border=0/>" + tk.ActionTypeText);
                                this.Pub1.AddTD(tk.RDT);
                                //this.Pub1.AddTD(tk.EmpFromT);

                                if (wcDesc.SigantureEnabel == true)
                                    this.Pub1.AddTD(BP.WF.Glo.GenerUserSigantureHtml(Server.MapPath("../../"), tk.EmpFrom, tk.EmpFromT));
                                else
                                    this.Pub1.AddTD(BP.WF.Glo.GenerUserImgSmallerHtml(tk.EmpFrom, tk.EmpFromT));
                                this.Pub1.AddTDBigDoc(tk.MsgHtml);
                                this.Pub1.AddTREnd();
                                empcheck += tk.EmpFrom;
                            }
                        }

                        #region  Check whether there are circumstances call sub-processes . If you have to call the child process information output . ( Phone translation is not considered part of the temporary ).
                        // int atTmp = (int)ActionType.StartChildenFlow;
                        BP.WF.WorkCheck wc2 = new WorkCheck(FK_Flow, tk.NDFrom, tk.WorkID, tk.FID);
                        if (wc2.FID != 0)
                        {
                            //Tracks ztks = wc2.HisWorkChecks;    // Repeat cycle !
                            //foreach (BP.WF.Track subTK in ztks)
                            //{
                            if (tk.HisActionType == ActionType.StartChildenFlow)
                            {
                                /* Illustrate the sub-processes */
                                /* If you are calling sub-processes , We must get to the parameters in the sub-processes are called , And to display them .*/
                                string[] paras = tk.Tag.Split('@');
                                string[] p1 = paras[1].Split('=');
                                string fk_flow = p1[1]; // Subprocess number 

                                string[] p2 = paras[2].Split('=');
                                string workId = p2[1]; // Subprocess ID.

                                BP.WF.WorkCheck subwc = new WorkCheck(fk_flow, int.Parse(fk_flow + "01"), Int64.Parse(workId), 0);

                                Tracks subtks = subwc.HisWorkChecks;

                                // All nodes taken out sub-processes .
                                Nodes subNds = new Nodes(fk_flow);
                                foreach (Node item in subNds)     // The main sequence display 
                                {
                                    foreach (BP.WF.Track mysubtk in subtks)
                                    {
                                        if (item.NodeID != mysubtk.NDFrom)
                                            continue;
                                        /* The output of the sub-process audit information , Should be considered a sub-sub-process flow information ,  Did not consider the complexity of the .*/
                                        if (mysubtk.HisActionType == ActionType.WorkCheck)
                                        {
                                            //biaojie   When initiating multiple sub-processes , Promoters displayed only once 
                                            if (mysubtk.NDFrom == int.Parse(fk_flow + "01") && biaoji == 1)
                                                continue;
                                            /* If the audit .*/
                                            this.Pub1.AddTR();
                                            this.Pub1.AddTD(mysubtk.ActionTypeText + "<img src='../Img/Mail_Read.png' border=0/>");
                                            this.Pub1.AddTD(mysubtk.RDT);
                                            //this.Pub1.AddTD(subtk.EmpFromT);
                                            this.Pub1.AddTD(BP.WF.Glo.GenerUserImgSmallerHtml(mysubtk.EmpFrom, mysubtk.EmpFromT));
                                            this.Pub1.AddTDBigDoc(mysubtk.MsgHtml);
                                            this.Pub1.AddTREnd();
                                            if (mysubtk.NDFrom == int.Parse(fk_flow + "01"))
                                            {
                                                biaoji = 1;
                                            }
                                        }
                                    }
                                }

                            }
                            //}
                        }
                        #endregion  Check whether there are circumstances call sub-processes . If you have to call the child process information output .
                    }

                    foreach (SelectAccper item in accepts)
                    {
                        if (item.FK_Node != nd.NodeID)
                            continue;
                        if (empcheck.Contains(item.FK_Emp) == true)
                            continue;
                        if (item.AccType == 0)
                            continue;
                        if (ccls.IsExits(CCListAttr.FK_Node, nd.NodeID) == true)
                            continue;

                        this.Pub1.AddTR();
                        this.Pub1.AddTD("style='color:Red;'", " Carried out ");
                        //else
                        //this.Pub1.AddTD("style='color:Red;'", " Cc ");
                        this.Pub1.AddTD("style='color:Red;'", "无");

                        //  Show staff to perform .
                        this.Pub1.AddTD("style='color:Red;'", BP.WF.Glo.GenerUserImgSmallerHtml(item.FK_Emp, item.EmpName));

                        //info.
                        this.Pub1.AddTD("style='color:Red;'", item.Info);
                        this.Pub1.AddTREnd();
                    }

                    #region  Output CC 
                    foreach (SelectAccper item in accepts)
                    {
                        if (item.FK_Node != nd.NodeID)
                            continue;
                        if (item.AccType != 1)
                            continue;
                        if (ccls.IsExits(CCListAttr.FK_Node, nd.NodeID) == false)
                        {
                            this.Pub1.AddTR();
                            this.Pub1.AddTD("style='color:Red;'", " Cc ");
                            this.Pub1.AddTD("style='color:Red;'", "无");
                            //  Show staff to perform .
                            this.Pub1.AddTD("style='color:Red;'", BP.WF.Glo.GenerUserImgSmallerHtml(item.FK_Emp, item.EmpName));

                            //info.
                            this.Pub1.AddTD("style='color:Red;'", item.Info);
                            this.Pub1.AddTREnd();
                        }
                        else
                        {
                            foreach (CCList cc in ccls)
                            {
                                if (cc.FK_Node != nd.NodeID)
                                    continue;

                                if (cc.HisSta == CCSta.CheckOver)
                                    continue;
                                if (cc.CCTo != item.FK_Emp)
                                    continue;

                                this.Pub1.AddTR();
                                if (cc.HisSta == CCSta.Read)
                                {
                                    if (nd.IsEndNode == true)
                                    {
                                        this.Pub1.AddTD("<img src='../Img/Mail_Read.png' border=0/> CC have read ");
                                        this.Pub1.AddTD(cc.CDT); // Read Time .
                                        this.Pub1.AddTD(BP.WF.Glo.GenerUserImgSmallerHtml(cc.CCTo, cc.CCToName));
                                        this.Pub1.AddTD(cc.CheckNoteHtml);
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    if (BP.Web.WebUser.No == cc.CCTo)
                                    {
                                        continue;

                                        /* If you open the I ,*/
                                        if (cc.HisSta == CCSta.UnRead)
                                            BP.WF.Dev2Interface.Node_CC_SetRead(cc.MyPK);
                                        this.Pub1.AddTD("<img src='../Img/Mail_Read.png' border=0/> Under review ");
                                    }
                                    else
                                    {
                                        this.Pub1.AddTD("<img src='../Img/Mail_UnRead.png' border=0/> CC did not read ");
                                    }

                                    this.Pub1.AddTD("无");
                                    this.Pub1.AddTD(BP.WF.Glo.GenerUserImgSmallerHtml(cc.CCTo, cc.CCToName));
                                    this.Pub1.AddTD("无");
                                }
                                this.Pub1.AddTREnd();
                            }
                        }
                    }
                    #endregion

                    this.Pub1.AddTableEnd();
                    this.Pub1.AddTDEnd();
                    this.Pub1.AddTREnd();
                }
                else
                {
                    if (wcDesc.FWCIsShowAllStep == false)
                        continue;

                    /* Does anyone have access to determine if the node , Or has been set up to receive a copy to the person object ,  If you do not do not output */
                    if (accepts.IsExits(SelectAccperAttr.FK_Node, nd.NodeID) == false)
                        continue;

                    /* Node does not appear .*/
                    this.Pub1.AddTR();
                    this.Pub1.AddTDBegin("colspan=4");
                    this.Pub1.AddTable("border=0 style='padding:0px;width:100%;' leftMargin=0 topMargin=0 id='tb" + nd.NodeID + "'");

                    /* Node does not appear .*/
                    this.Pub1.AddTR();
                    this.Pub1.AddTDTitle("colspan=4", "<div style='float:left'><image src='../Img/Tree/Cut.gif' onclick=\"show_and_hide_tr('tb" + nd.NodeID + "',this);\" style='cursor:pointer;'></image>" + nd.Name + "</div>");
                    this.Pub1.AddTREnd();

                    this.Pub1.AddTR("style='font-size:14px;font-weight:bold;'");
                    this.Pub1.AddTD("width='50'", " Event ");
                    this.Pub1.AddTD("width='150'", " Time ");
                    this.Pub1.AddTD("width='100'", " The operator ");
                    this.Pub1.AddTD(" Information ");
                    this.Pub1.AddTREnd();

                    // Whether the output of the .
                    bool isHaveIt = false;
                    foreach (SelectAccper item in accepts)
                    {
                        if (item.FK_Node != nd.NodeID)
                            continue;
                        if (item.AccType != 0)
                            continue;
                        this.Pub1.AddTR();
                        this.Pub1.AddTD("style='color:Red;'", " Carried out ");
                        //else
                        //this.Pub1.AddTD("style='color:Red;'", " Cc ");
                        this.Pub1.AddTD("style='color:Red;'", "无");

                        //  Show staff to perform .
                        this.Pub1.AddTD("style='color:Red;'", BP.WF.Glo.GenerUserImgSmallerHtml(item.FK_Emp, item.EmpName));

                        //info.
                        this.Pub1.AddTD("style='color:Red;'", item.Info);
                        this.Pub1.AddTREnd();
                        isHaveIt = true;
                    }

                    #region  Output CC 
                    foreach (SelectAccper item in accepts)
                    {
                        if (item.FK_Node != nd.NodeID)
                            continue;
                        if (item.AccType != 1)
                            continue;
                        if (ccls.IsExits(CCListAttr.FK_Node, nd.NodeID) == false)
                        {
                            this.Pub1.AddTR();
                            this.Pub1.AddTD("style='color:Red;'", " Cc ");
                            this.Pub1.AddTD("style='color:Red;'", "无");
                            //  Show staff to perform .
                            this.Pub1.AddTD("style='color:Red;'", BP.WF.Glo.GenerUserImgSmallerHtml(item.FK_Emp, item.EmpName));

                            //info.
                            this.Pub1.AddTD("style='color:Red;'", item.Info);
                            this.Pub1.AddTREnd();
                            isHaveIt = true;
                        }
                        else
                        {
                            foreach (CCList cc in ccls)
                            {
                                if (cc.FK_Node != nd.NodeID)
                                    continue;

                                if (cc.HisSta == CCSta.CheckOver)
                                    continue;
                                if (cc.CCTo != item.FK_Emp)
                                    continue;

                                this.Pub1.AddTR();
                                if (cc.HisSta == CCSta.Read)
                                {
                                    if (nd.IsEndNode == true)
                                    {
                                        this.Pub1.AddTD("<img src='../Img/Mail_Read.png' border=0/> CC have read ");
                                        this.Pub1.AddTD(cc.CDT); // Read Time .
                                        this.Pub1.AddTD(BP.WF.Glo.GenerUserImgSmallerHtml(cc.CCTo, cc.CCToName));
                                        this.Pub1.AddTD(cc.CheckNoteHtml);
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    if (BP.Web.WebUser.No == cc.CCTo)
                                    {
                                        continue;

                                        /* If you open the I ,*/
                                        if (cc.HisSta == CCSta.UnRead)
                                            BP.WF.Dev2Interface.Node_CC_SetRead(cc.MyPK);
                                        this.Pub1.AddTD("<img src='../Img/Mail_Read.png' border=0/> Under review ");
                                    }
                                    else
                                    {
                                        this.Pub1.AddTD("<img src='../Img/Mail_UnRead.png' border=0/> CC did not read ");
                                    }

                                    this.Pub1.AddTD("无");
                                    this.Pub1.AddTD(BP.WF.Glo.GenerUserImgSmallerHtml(cc.CCTo, cc.CCToName));
                                    this.Pub1.AddTD("无");
                                }
                                this.Pub1.AddTREnd();
                            }
                        }
                    }
                    #endregion

                    this.Pub1.AddTableEnd();
                    this.Pub1.AddTDEnd();
                    this.Pub1.AddTREnd();
                }



            }
            this.Pub1.AddTableEnd();

        }
        public void BindFreeModelV1_del(BP.Sys.FrmWorkCheck wcDesc)
        {
            BP.WF.WorkCheck wc = new WorkCheck(this.FK_Flow, this.NodeID, this.WorkID, this.FID);
            this.Pub1.AddTable("border=0 style='padding:0px;width:100%;' leftMargin=0 topMargin=0");
            if (IsHidden == false && wcDesc.HisFrmWorkCheckSta == BP.Sys.FrmWorkCheckSta.Enable)
            {
                this.Pub1.AddTR();
                this.Pub1.AddTD("<div style='float:right'><img src='/WF/Img/Btn/Save.gif' border=0 /> Save </div>");
                this.Pub1.AddTREnd();

                PostBackTextBox tb = new PostBackTextBox();
                tb.ID = "TB_Doc";
                tb.TextMode = TextBoxMode.MultiLine;
                tb.OnBlur += new EventHandler(btn_Click);
                tb.Style["width"] = "100%";
                tb.Rows = 3;
                if (DoType != null && DoType == "View")
                    tb.ReadOnly = true;
                tb.Text = BP.WF.Dev2Interface.GetCheckInfo(this.FK_Flow, this.WorkID, this.NodeID);
                if (tb.Text == "")
                {
                    tb.Text = wcDesc.FWCDefInfo;
                    BP.WF.Dev2Interface.WriteTrackWorkCheck(this.FK_Flow, this.NodeID, this.WorkID, this.FID, tb.Text, wcDesc.FWCOpLabel);
                }

                this.Pub1.AddTR();
                this.Pub1.AddTD(tb);
                this.Pub1.AddTREnd();
            }

            if (wcDesc.FWCListEnable == false)
            {
                this.Pub1.AddTableEnd();
                return;
            }

            int i = 0;
            BP.WF.Tracks tks = wc.HisWorkChecks;
            foreach (BP.WF.Track tk in tks)
            {
                #region  Output Review .
                if (tk.HisActionType == ActionType.WorkCheck)
                {
                    /* If the audit .*/
                    i++;
                    ActionType at = tk.HisActionType;
                    DateTime dtt = BP.DA.DataType.ParseSysDateTime2DateTime(tk.RDT);

                    this.Pub1.AddTR();
                    this.Pub1.AddTDBegin();
                    this.Pub1.AddB(tk.NDFromT);
                    this.Pub1.AddBR(tk.MsgHtml);
                    this.Pub1.AddBR("<div style='float:right'>" + BP.WF.Glo.GenerUserImgSmallerHtml(tk.EmpFrom, tk.EmpFromT) + " &nbsp;&nbsp;&nbsp; " + dtt.ToString("yy年MM月dd日HH时mm分") + "</div>");
                    this.Pub1.AddTDEnd();
                    this.Pub1.AddTREnd();
                }
                #endregion  Output Review .

                #region  Check for sub-processes .
                if (tk.HisActionType == ActionType.StartChildenFlow)
                {
                    /* If you are calling sub-processes , We must get to the parameters in the sub-processes are called , And to display them .*/
                    string[] paras = tk.Tag.Split('@');
                    string[] p1 = paras[1].Split('=');
                    string fk_flow = p1[1];

                    string[] p2 = paras[2].Split('=');
                    string workId = p2[1];

                    BP.WF.WorkCheck subwc = new WorkCheck(fk_flow, int.Parse(fk_flow + "01"), Int64.Parse(workId), 0);
                    Tracks subtks = subwc.HisWorkChecks;
                    foreach (BP.WF.Track subtk in subtks)
                    {
                        if (subtk.HisActionType == ActionType.WorkCheck)
                        {
                            /* If the audit .*/
                            i++;
                            ActionType at = subtk.HisActionType;
                            DateTime dtt = BP.DA.DataType.ParseSysDateTime2DateTime(subtk.RDT);

                            this.Pub1.AddTR();
                            this.Pub1.AddTDBegin();

                            this.Pub1.AddB(subtk.NDFromT);
                            this.Pub1.AddBR(subtk.MsgHtml);
                            this.Pub1.AddBR("<div style='float:right'>" + BP.WF.Glo.GenerUserImgSmallerHtml(subtk.EmpFrom, subtk.EmpFromT) + " &nbsp;&nbsp;&nbsp; " + dtt.ToString("yy年MM月dd日HH时mm分") + "</div>");

                            this.Pub1.AddTDEnd();
                            this.Pub1.AddTREnd();

                        }
                    }
                }
                #endregion  Check for sub-processes .

            }
            this.Pub1.AddTableEnd();
        }
        public void BindTableModel(BP.Sys.FrmWorkCheck wcDesc)
        {
            BP.WF.WorkCheck wc = new WorkCheck(this.FK_Flow, this.NodeID, this.WorkID, this.FID);

            this.Pub1.AddTable("border=1 style='padding:0px;width:100%;'");
            this.Pub1.AddTR();
            this.Pub1.AddTD("colspan=8", "<div style='float:left'> Approval comments </div> <div style='float:right'><img src='../Img/Btn/Save.gif' border=0 /></div>");
            this.Pub1.AddTREnd();

            if (!IsHidden)
            {
                PostBackTextBox tb = new PostBackTextBox();
                tb.ID = "TB_Doc";
                tb.TextMode = TextBoxMode.MultiLine;
                tb.OnBlur += new EventHandler(btn_Click);
                tb.Style["width"] = "100%";
                tb.Rows = 3;
                if (DoType != null && DoType == "View") tb.ReadOnly = true;

                tb.Text = BP.WF.Dev2Interface.GetCheckInfo(this.FK_Flow, this.WorkID, this.NodeID);

                this.Pub1.AddTD("colspan=8", tb);
                this.Pub1.AddTREnd();
            }

            this.Pub1.AddTR();
            this.Pub1.AddTD("IDX");
            this.Pub1.AddTD(" Time of occurrence ");
            this.Pub1.AddTD(" On node ");
            //   this.Pub1.AddTD(" Staff ");
            this.Pub1.AddTD(" Activity ");
            this.Pub1.AddTD(" Information / Approval comments ");
            this.Pub1.AddTD(" Executor ");
            this.Pub1.AddTREnd();

            int i = 0;
            BP.WF.Tracks tks = wc.HisWorkChecks;
            foreach (BP.WF.Track tk in tks)
            {
                if (tk.HisActionType == ActionType.Forward
                    || tk.HisActionType == ActionType.ForwardFL
                    || tk.HisActionType == ActionType.ForwardHL
                    )
                {
                    string nd = tk.NDFrom.ToString();
                    if (nd.Substring(nd.Length - 2) != "01")
                        continue;
                    //string len=tk.NDFrom.ToString();
                    //if (
                    //if (tk.NDFrom.ToString().Contains
                }

                if (tk.HisActionType != ActionType.WorkCheck)
                    continue;

                i++;
                this.Pub1.AddTR();
                this.Pub1.AddTD(i);
                DateTime dtt = BP.DA.DataType.ParseSysDateTime2DateTime(tk.RDT);
                this.Pub1.AddTD(dtt.ToString("MM月dd日HH时mm分"));
                this.Pub1.AddTD(tk.NDFromT);
                //  this.Pub1.AddTD(tk.EmpFromT);
                ActionType at = tk.HisActionType;
                //this.Pub1.AddTD("<img src='./../Img/Action/" + at.ToString() + ".png' class='ActionType' width='16px' border=0/>" + BP.WF.Track.GetActionTypeT(at));
                this.Pub1.AddTD("<img src='./../Img/Action/" + at.ToString() + ".png' class='ActionType' width='16px' border=0/>" + tk.ActionTypeText);
                this.Pub1.AddTD(tk.MsgHtml);
                this.Pub1.AddTD(tk.Exer); // If the commission , Adding a    Staff ( Entrust )
                this.Pub1.AddTREnd();
            }
            this.Pub1.AddTableEnd();
        }

        // Show an empty form 
        private void ViewEmptyForm()
        {
            this.Pub1.AddTable(" border=1 style='padding:0px;width:100%;'");
            this.Pub1.AddTR();
            this.Pub1.AddTD("colspan=6 style='text-align:left' ", " Approval comments ");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD("IDX");
            this.Pub1.AddTD(" Time of occurrence ");
            this.Pub1.AddTD(" On node ");
            this.Pub1.AddTD(" Activity ");
            this.Pub1.AddTD(" Information / Approval comments ");
            this.Pub1.AddTD(" Executor ");
            this.Pub1.AddTREnd();

            this.Pub1.AddTableEnd();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            // Cancel Save time viewing 
            if (DoType != null && DoType == "View")
                return;

            // Content is empty , Cancel Save 
            if (string.IsNullOrEmpty(this.Pub1.GetTextBoxByID("TB_Doc").Text.Trim())) return;
            //  Join audit information .
            string msg = this.Pub1.GetTextBoxByID("TB_Doc").Text;

            BP.Sys.FrmWorkCheck wcDesc = new BP.Sys.FrmWorkCheck(this.NodeID);

            //  NPC processing needs , Need to be written to the audit opinion FlowNote Go inside .
            string sql = "UPDATE WF_GenerWorkFlow SET FlowNote='" + msg + "' WHERE WorkID=" + this.WorkID;
            BP.DA.DBAccess.RunSQL(sql);

            //  Determine whether the CC ?
            if (this.IsCC)
            {
                //  Write audit information , There may be update Data .
                BP.WF.Dev2Interface.WriteTrackWorkCheck(this.FK_Flow, this.NodeID, this.WorkID, this.FID, msg, wcDesc.FWCOpLabel);

                // Set CC status  -  Has completed the audit .
                BP.WF.Dev2Interface.Node_CC_SetSta(this.NodeID, this.WorkID, BP.Web.WebUser.No, CCSta.CheckOver);
            }
            else
            {
                BP.WF.Dev2Interface.WriteTrackWorkCheck(this.FK_Flow, this.NodeID, this.WorkID, this.FID, msg, wcDesc.FWCOpLabel);
            }

            this.Response.Redirect("WorkCheck.aspx?s=2&OID=" + this.WorkID + "&FK_Node=" + this.NodeID + "&FK_Flow=" + this.FK_Flow + "&FID=" + this.FID + "&Paras=" + this.Request.QueryString["Paras"], true);
            // Execution approval .
            //BP.Sys.PubClass.Alert(" Saved successfully ...");

            // Close .
            //BP.Sys.PubClass.WinClose();
        }
    }

    // Custom Controls 
    public class PostBackTextBox : System.Web.UI.WebControls.TextBox, System.Web.UI.IPostBackEventHandler
    {
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            Attributes["onblur"] = Page.GetPostBackEventReference(this);
            base.Render(writer);
        }

        public event EventHandler OnBlur;

        public virtual void RaisePostBackEvent(string eventArgument)
        {
            if (OnBlur != null)
            {
                OnBlur(this, null);
            }
        }
    }
}