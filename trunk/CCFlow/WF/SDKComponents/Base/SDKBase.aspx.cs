using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using BP.DA;
using BP.Web;
using BP.WF.Data;
using BP.WF;
using BP.En;
using BP.WF.Template;
using BP.Sys;


namespace CCFlow.WF.SDKComponents.Base
{
    public partial class SDKBase : System.Web.UI.Page
    {
        #region  Parameters .
        public string getUTF8ToString(string param)
        {
            return HttpUtility.UrlDecode(Request[param], System.Text.Encoding.UTF8);
        }
        public string DoFunc
        {
            get
            {
                return getUTF8ToString("DoFunc");
            }
        }
        public string CFlowNo
        {
            get
            {
                return getUTF8ToString("CFlowNo");
            }
        }
        public string WorkIDs
        {
            get
            {
                return getUTF8ToString("WorkIDs");
            }
        }
        public string FK_Flow
        {
            get
            {
                return getUTF8ToString("FK_Flow");
            }
        }
        public int FK_Node
        {
            get
            {
                string fk_node = getUTF8ToString("FK_Node");
                if (!string.IsNullOrEmpty(fk_node))
                    return Int32.Parse(getUTF8ToString("FK_Node"));
                return 0;
            }
        }
        public Int64 WorkID
        {
            get
            {
                return Int64.Parse(getUTF8ToString("WorkID"));
            }
        }
        public Int64 FID
        {
            get
            {
                return Int64.Parse(getUTF8ToString("FID"));
            }
        }
        #endregion  Parameters .
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
                case "getapptoolbar":
                    s_responsetext = GetAppToolBar();
                    break;
                case "getflowformtree":// Gets the tree 
                    s_responsetext = GetFlowFormTree();
                    break;
                case "checkaccepter":// Recipient checks 
                    s_responsetext = CheckAccepterOper();
                    break;
                case "sendcase":// Performing transmission 
                    s_responsetext = SendCase();
                    break;
                case "sendcasetonode":// Transmission is performed to the specified node 
                    s_responsetext = SendCaseToNode();
                    break;
                case "unsendcase":// Send revocation 
                    s_responsetext = UnSendCase();
                    break;
                case "":// Save 
                    s_responsetext = Send();
                    break;
                case "delcase":// Delete Process 
                    s_responsetext = Delcase();
                    break;
                case "signcase":// Signature Process 
                    s_responsetext = Signcase();
                    break;
                case "endcase":// End Process 
                    s_responsetext = EndCase();
                    break;
                case "readCC":
                    s_responsetext = ReadCC();
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

        public string ReadCC()
        {
            string str = "";
            try
            {
                int nodeID = int.Parse(Request["FK_Node"]);
                long workID = Int64.Parse(Request["WorkID"]);

                BP.WF.Dev2Interface.Node_CC_SetSta(nodeID, workID, BP.Web.WebUser.No, BP.WF.Template.CCSta.CheckOver);
                str = "true";
            }
            catch (Exception)
            {
                str = "false";

            }
            return str;


        }
        /// <summary>
        ///  Save  
        /// </summary>
        /// <returns></returns>
        public string Send()
        {
            return "";
        }
        /// <summary>
        ///  End Process 
        /// </summary>
        /// <returns></returns>
        public string EndCase()
        {
            string flowId = getUTF8ToString("flowId");
            long workID = Int64.Parse(getUTF8ToString("workId"));
            
            return BP.WF.Dev2Interface.Flow_DoFlowOverByCoercion(flowId,this.FK_Node, workID, this.FID, "");
        }
        /// <summary>
        ///  Delete Process 
        /// </summary>
        public string Delcase()
        {
            string returnstr = "";
            string flowId = getUTF8ToString("flowId");
            int fk_Node = Int32.Parse(getUTF8ToString("nodeId"));
            long workID = Int64.Parse(getUTF8ToString("workId"));
            long fId = Int64.Parse(getUTF8ToString("fId"));
            BP.WF.Node currND = new BP.WF.Node(fk_Node);

            return BP.WF.Dev2Interface.Flow_DoDeleteFlowByReal(flowId, workID, false);
        }
        /// <summary>
        ///  Signature Process 
        /// </summary>
        public string Signcase()
        {
            string flowId = getUTF8ToString("flowId");
            int fk_Node = Int32.Parse(getUTF8ToString("nodeId"));
            long workID = Int64.Parse(getUTF8ToString("workId"));
            long fId = Int64.Parse(getUTF8ToString("fId"));
            if (fId > 0) workID = fId;
            string yj = getUTF8ToString("yj");
            if (yj == null || yj == "") yj = " Have read ";

            BP.DA.Paras ps = new BP.DA.Paras();
            ps.Add("FK_Node", fk_Node);

            DataTable Sys_FrmSln = BP.DA.DBAccess.RunSQLReturnTable("select Sys_FrmSln.FK_MapData,Sys_FrmSln.KeyOfEn,Sys_FrmSln.IsSigan,Sys_MapAttr.MyDataType from Sys_FrmSln,Sys_MapAttr where Sys_FrmSln.UIIsEnable=1 and Sys_FrmSln.IsNotNull=1 and Sys_FrmSln.FK_Node=" + BP.Sys.SystemConfig.AppCenterDBVarStr + "FK_Node and Sys_FrmSln.KeyOfEn=Sys_MapAttr.KeyOfEn and Sys_FrmSln.FK_MapData=Sys_MapAttr.FK_MapData", ps);
            Boolean IsSign = false;
            foreach (DataRow DR in Sys_FrmSln.Rows)
            {
                string PTableField = DR["KeyOfEn"].ToString();
                string autotext = "";
                if (DR["IsSigan"].ToString() == "1")
                    autotext = WebUser.No;
                else if (DR["MyDataType"].ToString() == "6")// Time Field 
                    autotext = DateTime.Now.ToString("yyyy-MM-dd");
                else if (DR["MyDataType"].ToString() == "1")// Field observations 
                {
                    PTableField = PTableField.ToUpper();
                    if (PTableField.EndsWith("YJ") || PTableField.EndsWith("YJ1") || PTableField.EndsWith("YJ2") || PTableField.EndsWith("YJ3") || PTableField.EndsWith("YJ4") || PTableField.EndsWith("YJ5") || PTableField.EndsWith("YJ6") || PTableField.EndsWith("YJ7") || PTableField.EndsWith("YJ8") || PTableField.EndsWith("YJ9"))
                        autotext = yj;
                    else
                        continue;
                }
                else
                    continue;
                string PTable = BP.DA.DBAccess.RunSQLReturnString("select PTable from Sys_MapData where No='" + DR["FK_MapData"].ToString() + "'");
                if (PTable != null)
                {
                    Int32 HavData = BP.DA.DBAccess.RunSQLReturnValInt("select count(*) from " + PTable + " where oid=" + workID.ToString());
                    if (HavData == 0)
                        BP.DA.DBAccess.RunSQL("insert into " + PTable + "(oid," + PTableField + ") values(" + workID.ToString() + ",'" + autotext + "')");
                    else
                        BP.DA.DBAccess.RunSQL("update " + PTable + " set " + PTableField + "='" + autotext + "' where oid=" + workID.ToString());
                    IsSign = true;
                }
            }
            if (IsSign)
                return " Signature completed ";
            else
                return " No signature can be checked ";
        }
        /// <summary>
        ///  Get Toolbar 
        /// </summary>
        /// <returns></returns>
        private string GetAppToolBar()
        {
            int fk_Node = Int32.Parse(getUTF8ToString("nodeId"));
            BtnLab btnLab = new BtnLab(fk_Node);
            StringBuilder toolsBar = new StringBuilder();
            toolsBar.Append("{");

            // System Tools 
            toolsBar.Append("tools:[");
            //Send,Save,Thread,Return,CC,Shift,Del,EndFLow,RptTrack,HungUp"
            // Send 
            if (btnLab.SendEnable)
            {
                toolsBar.Append("{");
                toolsBar.Append("no:'Send',btnlabel:'" + btnLab.SendLab + "'");
                toolsBar.Append("},");
            }
            // Save 
            if (btnLab.SaveEnable)
            {
                toolsBar.Append("{");
                toolsBar.Append("no:'Save',btnlabel:'" + btnLab.SaveLab + "'");
                toolsBar.Append("},");
            }
            // Child thread 
            if (btnLab.ThreadEnable)
            {
                toolsBar.Append("{");
                toolsBar.Append("no:'Thread',btnlabel:'" + btnLab.ThreadLab + "'");
                toolsBar.Append("},");
            }

            // Return 
            if (btnLab.ReturnEnable)
            {
                toolsBar.Append("{");
                toolsBar.Append("no:'Return',btnlabel:'" + btnLab.ReturnLab + "'");
                toolsBar.Append("},");
            }
            // Cc 
            if (btnLab.CCRole != 0)
            {
                toolsBar.Append("{");
                toolsBar.Append("no:'CC',btnlabel:'" + btnLab.CCLab + "'");
                toolsBar.Append("},");
            }
            // Transfer 
            if (btnLab.ShiftEnable)
            {
                toolsBar.Append("{");
                toolsBar.Append("no:'Shift',btnlabel:'" + btnLab.ShiftLab + "'");
                toolsBar.Append("},");
            }
            // Delete  
            if (btnLab.DeleteEnable != 0)
            {
                toolsBar.Append("{");
                toolsBar.Append("no:'Del',btnlabel:'" + btnLab.DeleteLab + "'");
                toolsBar.Append("},");
            }
            // End  
            if (btnLab.EndFlowEnable)
            {
                toolsBar.Append("{");
                toolsBar.Append("no:'EndFLow',btnlabel:'" + btnLab.EndFlowLab + "'");
                toolsBar.Append("},");
            }
            // Print  
            if (btnLab.PrintDocEnable)
            {
                toolsBar.Append("{");
                toolsBar.Append("no:'Rpt',btnlabel:'" + btnLab.PrintDocLab + "'");
                toolsBar.Append("},");
            }
            // Locus 
            if (btnLab.TrackEnable)
            {
                toolsBar.Append("{");
                toolsBar.Append("no:'Track',btnlabel:'" + btnLab.TrackLab + "'");
                toolsBar.Append("},");
            }
            // Pending 
            if (btnLab.HungEnable)
            {
                toolsBar.Append("{");
                toolsBar.Append("no:'HungUp',btnlabel:'" + btnLab.HungLab + "'");
                toolsBar.Append("},");
            }
            if (toolsBar.Length > 8)
                toolsBar.Remove(toolsBar.Length - 1, 1);
            toolsBar.Append("]");
            // Expansion Tool 
            NodeToolbars extToolBars = new NodeToolbars();
            extToolBars.RetrieveByAttr(NodeToolbarAttr.FK_Node, fk_Node);
            toolsBar.Append(",extTools:[");
            if (extToolBars.Count > 0)
            {
                foreach (NodeToolbar item in extToolBars)
                {
                    toolsBar.Append("{OID:'" + item.OID + "',Title:'" + item.Title + "',Target:'" + item.Target + "',Url:'" + item.Url + "'},");
                }
                toolsBar.Remove(toolsBar.Length - 1, 1);
            }
            toolsBar.Append("]");
            toolsBar.Append("}");
            return toolsBar.ToString();
        }

        /// <summary>
        ///  Check whether the node is enabled recipient selector 
        /// </summary>
        /// <returns></returns>
        private string CheckAccepterOper()
        {
            int tempToNodeID = 0;
            // Get to the current node 
            Node _HisNode = new Node(this.FK_Node);

            /* If you reach the point is empty  */
            Nodes nds = _HisNode.HisToNodes;
            if (nds.Count == 0)
            {
                // The current point is the last node , You can not use this feature 
                return "end";
            }
            else if (nds.Count == 1)
            {
                BP.WF.Node toND = nds[0] as BP.WF.Node;
                tempToNodeID = toND.NodeID;
            }
            else
            {
                foreach (BP.WF.Node mynd in nds)
                {
                    //if (mynd.HisDeliveryWay != DeliveryWay.BySelected)
                    //    continue;

                    GERpt _wk = _HisNode.HisFlow.HisGERpt;
                    _wk.OID = this.WorkID;
                    _wk.Retrieve();
                    _wk.ResetDefaultVal();

                    #region  Filtered inaccessible node .
                    Cond cond = new Cond();
                    int i = cond.Retrieve(CondAttr.FK_Node, _HisNode.NodeID, CondAttr.ToNodeID, mynd.NodeID);
                    if (i == 0)
                        continue; //  The condition is not set direction , Just let it jump .
                    cond.WorkID = this.WorkID;
                    cond.en = _wk;
                    if (cond.IsPassed == false)
                        continue;
                    #endregion  Filtered inaccessible node .
                    tempToNodeID = mynd.NodeID;
                }
            }
            // A node in the absence of , Check whether the user to select a node 
            if (tempToNodeID == 0)
            {
                try
                {
                    // Check required 
                    BP.WF.WorkNode workeNode = new WorkNode(this.WorkID, this.FK_Node);
                    workeNode.CheckFrmIsNotNull();
                }
                catch (Exception ex)
                {
                    return "error:" + ex.Message;
                }
                // Calculated in accordance with the user selects 
                if (_HisNode.CondModel == CondModel.ByUserSelected)
                {
                    return "byuserselected";
                }
                return "notonode";
            }

            // Determining whether the node is reachable by accepting people choose 
            Node toNode = new Node(tempToNodeID);
            if (toNode.HisDeliveryWay == DeliveryWay.BySelected)
            {
                return "byselected";
            }
            return "nodata";
        }

        /// <summary>
        ///  Performing transmission 
        /// </summary>
        /// <returns></returns>
        private string SendCase()
        {
            string resultMsg = "";
            try
            {
                if (Dev2Interface.Flow_IsCanDoCurrentWork(this.FK_Flow, this.FK_Node, this.WorkID, WebUser.No) == false)
                {
                    resultMsg = "error| Hello :" + BP.Web.WebUser.No + ", " + WebUser.Name + " The current work has been processed , Or you do not have permission to perform this work .";
                }
                SendReturnObjs returnObjs = Dev2Interface.Node_SendWork(this.FK_Flow, this.WorkID);
                resultMsg = returnObjs.ToMsgOfHtml();
                if (resultMsg.IndexOf("@<a") > 0)
                {
                    string kj = resultMsg.Substring(0, resultMsg.IndexOf("@<a"));
                    resultMsg = resultMsg.Substring(resultMsg.IndexOf("@<a")) + "<br/><br/>" + kj;
                }
                // Revocation of documents 
                int docindex = resultMsg.IndexOf("@<img src='../../Img/FileType/doc.gif' />");
                if (docindex != -1)
                {
                    String kj = resultMsg.Substring(0, docindex);
                    String kp = "";
                    int nextdocindex = resultMsg.IndexOf("@", docindex + 1);
                    if (nextdocindex != -1)
                        kp = resultMsg.Substring(nextdocindex);
                    resultMsg = kj + kp;
                }
                // Revocation   Revocation of this transmission 
                int UnSendindex = resultMsg.IndexOf("@<a href='../../MyFlowInfo.aspx?DoType=UnSend");
                if (UnSendindex != -1)
                {
                    String kj = resultMsg.Substring(0, UnSendindex);
                    String kp = "";
                    int nextUnSendindex = resultMsg.IndexOf("@", UnSendindex + 1);
                    if (nextUnSendindex != -1)
                        kp = resultMsg.Substring(nextUnSendindex);
                    resultMsg = kj + "<a href='javascript:UnSend();'><img src='../../Img/UnDo.gif' border=0/> Revocation of this transmission </a>" + kp;
                }

                resultMsg = resultMsg.Replace(" Specify a particular officers dealing ", " Designated officer ");
                resultMsg = resultMsg.Replace(" Send SMS to remind him(them)", " SMS notification ");
                resultMsg = resultMsg.Replace(" Revocation of this transmission ", " Revocation cases ");
                resultMsg = resultMsg.Replace(" New Process ", " Initiate cases ");
                resultMsg = resultMsg.Replace(".", "");
                resultMsg = resultMsg.Replace(",", "");

                resultMsg = resultMsg.Replace("@ Next ", "<br/><br/>&nbsp;&nbsp;&nbsp; Next ");
                resultMsg = "success|<br/>" + resultMsg.Replace("@", "&nbsp;&nbsp;&nbsp;");

                #region  Business logic methods to handle the general sent successfully after , This method may throw an exception .
                /* There are two cases 
                 * 1, From the intermediate node , By batch processing , That is the case of the merger approval process , In this case you need to perform the sub process to the next step .
                   2, From the process has been completed , Or is running , That is the case of the merger approval process . */
                try
                {
                    // Business logic methods to handle the general sent successfully after , This method may throw an exception .
                    BP.WF.Glo.DealBuinessAfterSendWork(this.FK_Flow, this.WorkID, this.DoFunc, WorkIDs, this.CFlowNo, 0, null);
                }
                catch (Exception ex)
                {
                    resultMsg = "sysError|" + ex.Message.Replace("@", "<br/>");
                    return resultMsg;
                }
                #endregion  Business logic methods to handle the general sent successfully after , This method may throw an exception .

            }
            catch (Exception ex)
            {
                resultMsg = "sysError|" + ex.Message.Replace("@", "<br/>");
            }
            return resultMsg;
        }

        /// <summary>
        ///  Transmission is performed to the specified node 
        /// </summary>
        /// <returns></returns>
        private string SendCaseToNode()
        {
            int ToNode = Convert.ToInt32(getUTF8ToString("ToNode"));
            string resultMsg = "";
            try
            {
                if (Dev2Interface.Flow_IsCanDoCurrentWork(this.FK_Flow, this.FK_Node, this.WorkID, WebUser.No) == false)
                {
                    return resultMsg = "error| Hello :" + BP.Web.WebUser.No + ", " + WebUser.Name + " The current work has been processed , Or you do not have permission to perform this work .";
                }
                SendReturnObjs returnObjs = Dev2Interface.Node_SendWork(this.FK_Flow, this.WorkID, ToNode, null);
                resultMsg = returnObjs.ToMsgOfHtml();
                if (resultMsg.IndexOf("@<a") > 0)
                {
                    string kj = resultMsg.Substring(0, resultMsg.IndexOf("@<a"));
                    resultMsg = resultMsg.Substring(resultMsg.IndexOf("@<a")) + "<br/><br/>" + kj;
                }
                // Revocation of documents 
                int docindex = resultMsg.IndexOf("@<img src='../../Img/FileType/doc.gif' />");
                if (docindex != -1)
                {
                    String kj = resultMsg.Substring(0, docindex);
                    String kp = "";
                    int nextdocindex = resultMsg.IndexOf("@", docindex + 1);
                    if (nextdocindex != -1)
                        kp = resultMsg.Substring(nextdocindex);
                    resultMsg = kj + kp;
                }
                // Revocation   Revocation of this transmission 
                int UnSendindex = resultMsg.IndexOf("@<a href='MyFlowInfo.aspx?DoType=UnSend");
                if (UnSendindex != -1)
                {
                    String kj = resultMsg.Substring(0, UnSendindex);
                    String kp = "";
                    int nextUnSendindex = resultMsg.IndexOf("@", UnSendindex + 1);
                    if (nextUnSendindex != -1)
                        kp = resultMsg.Substring(nextUnSendindex);
                    resultMsg = kj + "<a href='javascript:UnSend();'><img src='../../Img/UnDo.gif' border=0/> Revocation of this transmission </a>" + kp;
                }

                resultMsg = resultMsg.Replace(" Specify a particular officers dealing ", " Designated officer ");
                resultMsg = resultMsg.Replace(" Send SMS to remind him (them)", " SMS notification ");
                resultMsg = resultMsg.Replace(" Revocation of this transmission ", " Revocation cases ");
                resultMsg = resultMsg.Replace(" New Process ", " Initiate cases ");
                resultMsg = resultMsg.Replace(".", "");
                resultMsg = resultMsg.Replace(",", "");

                resultMsg = resultMsg.Replace("@ Next ", "<br/><br/>&nbsp;&nbsp;&nbsp; Next ");
                resultMsg = "success|<br/>" + resultMsg.Replace("@", "&nbsp;&nbsp;&nbsp;");

                #region  Business logic methods to handle the general sent successfully after , This method may throw an exception .
                /* There are two cases 
                 * 1, From the intermediate node , By batch processing , That is the case of the merger approval process , In this case you need to perform the sub process to the next step .
                   2, From the process has been completed , Or is running , That is the case of the merger approval process . */
                try
                {
                    // Business logic methods to handle the general sent successfully after , This method may throw an exception .
                    BP.WF.Glo.DealBuinessAfterSendWork(this.FK_Flow, this.WorkID, this.DoFunc, WorkIDs, this.CFlowNo, 0, null);
                }
                catch (Exception ex)
                {
                    resultMsg = "sysError|" + ex.Message.Replace("@", "<br/>");
                    return resultMsg;
                }
                #endregion  Business logic methods to handle the general sent successfully after , This method may throw an exception .

            }
            catch (Exception ex)
            {
                resultMsg = "sysError|" + ex.Message.Replace("@", "<br/>");
            }
            return resultMsg;
        }

        /// <summary>
        ///  Send revocation 
        /// </summary>
        /// <returns></returns>
        private string UnSendCase()
        {
            try
            {
                string FK_Flow = getUTF8ToString("FK_Flow");
                string WorkID = getUTF8ToString("WorkID");
                string str1 = BP.WF.Dev2Interface.Flow_DoUnSend(FK_Flow, Int64.Parse(WorkID));
                return "true";
            }
            catch (Exception ex)
            {
                return "{message:' Undo failure , Failure information " + ex.Message + "'}";
            }
        }
        /// <summary>
        ///  Gets the tree 
        /// </summary>
        /// <returns></returns>
        BP.WF.Template.FlowFormTrees appFlowFormTree = new FlowFormTrees();
        private string GetFlowFormTree()
        {
            string flowId = getUTF8ToString("flowId");
            string nodeId = getUTF8ToString("nodeId");

            //add root
            BP.WF.Template.FlowFormTree root = new BP.WF.Template.FlowFormTree();
            root.No = "01";
            root.ParentNo = "0";
            root.Name = " Table of Contents ";
            root.NodeType = "root";
            appFlowFormTree.Clear();
            appFlowFormTree.AddEntity(root);

            #region  Add forms and folders 

            // Node Form 
            FrmNodes frmNodes = new FrmNodes();
            QueryObject qo = new QueryObject(frmNodes);
            qo.AddWhere(FrmNodeAttr.FK_Node, nodeId);
            qo.addAnd();
            qo.AddWhere(FrmNodeAttr.FK_Flow, flowId);
            qo.addOrderBy(FrmNodeAttr.Idx);
            qo.DoQuery();
            // Folder 
            SysFormTrees formTrees = new SysFormTrees();
            formTrees.RetrieveAll(SysFormTreeAttr.Name);
            // All forms collection 
            MapDatas mds = new MapDatas();
            mds.Retrieve(MapDataAttr.AppType, (int)AppType.Application);
            foreach (FrmNode frmNode in frmNodes)
            {
                foreach (MapData md in mds)
                {
                    if (frmNode.FK_Frm != md.No)
                        continue;

                    foreach (SysFormTree formTree in formTrees)
                    {
                        if (md.FK_FormTree != formTree.No)
                            continue;

                        if (!appFlowFormTree.Contains("No", formTree.No))
                        {
                            BP.WF.Template.FlowFormTree nodeFolder = new BP.WF.Template.FlowFormTree();
                            nodeFolder.No = formTree.No;
                            nodeFolder.ParentNo = root.No;
                            nodeFolder.Name = formTree.Name;
                            nodeFolder.NodeType = "folder";
                            appFlowFormTree.AddEntity(nodeFolder);
                        }
                    }
                    // Check required 
                    bool IsNotNull = false;
                    FrmFields formFields = new FrmFields();
                    QueryObject obj = new QueryObject(formFields);
                    obj.AddWhere(FrmFieldAttr.FK_Node, nodeId);
                    obj.addAnd();
                    obj.AddWhere(FrmFieldAttr.FK_MapData, md.No);
                    obj.addAnd();
                    obj.AddWhere(FrmFieldAttr.IsNotNull, "1");
                    obj.DoQuery();
                    if (formFields != null && formFields.Count > 0) IsNotNull = true;

                    BP.WF.Template.FlowFormTree nodeForm = new BP.WF.Template.FlowFormTree();
                    nodeForm.No = md.No;
                    nodeForm.ParentNo = md.FK_FormTree;
                    nodeForm.Name = md.Name;
                    nodeForm.NodeType = IsNotNull ? "form|1" : "form|0";
                    appFlowFormTree.AddEntity(nodeForm);
                }
            }
            #endregion
            // Expansion Tool , Display position for the form tree type 
            NodeToolbars extToolBars = new NodeToolbars();
            QueryObject info = new QueryObject(extToolBars);
            info.AddWhere(NodeToolbarAttr.FK_Node, nodeId);
            info.addAnd();
            info.AddWhere(NodeToolbarAttr.ShowWhere, (int)ShowWhere.Tree);
            info.DoQuery();

            foreach (NodeToolbar item in extToolBars)
            {
                string url = "";
                if (string.IsNullOrEmpty(item.Url))
                    continue;

                url = item.Url;

                BP.WF.Template.FlowFormTree formTree = new BP.WF.Template.FlowFormTree();
                formTree.No = item.OID.ToString();
                formTree.ParentNo = "01";
                formTree.Name = item.Title;
                formTree.NodeType = "tools|0";
                if (!string.IsNullOrEmpty(item.Target) && item.Target.ToUpper() == "_BLANK")
                {
                    formTree.NodeType = "tools|1";
                }

                formTree.Url = url;
                appFlowFormTree.AddEntity(formTree);
            }
            TansEntitiesToGenerTree(appFlowFormTree, root.No, "");
            return appendMenus.ToString();
        }

        //private string GetFlowFormTree_old()
        //{
        //    string flowId = getUTF8ToString("flowId");
        //    string nodeId = getUTF8ToString("nodeId");
        //    // Process parent tree form 
        //    BP.WF.Template.FlowFormTrees flowFormTree = new BP.WF.Template.FlowFormTrees();
        //    QueryObject obj = new QueryObject(flowFormTree);
        //    obj.AddWhere(FlowFormTreeAttr.FK_Flow, flowId);
        //    obj.addAnd();
        //    obj.AddWhere(FlowFormTreeAttr.ParentNo, "01");
        //    obj.addOrderBy(FlowFormTreeAttr.Name);
        //    obj.DoQuery();
        //    // If it is empty , The initial data 
        //    if (flowFormTree == null || flowFormTree.Count == 0)
        //    {
        //        InitFlowFormTree(flowId);

        //        // Reacquire 
        //        flowFormTree = new BP.WF.Template.FlowFormTrees();
        //        obj = new QueryObject(flowFormTree);
        //        obj.AddWhere(FlowFormTreeAttr.FK_Flow, flowId);
        //        obj.addAnd();
        //        obj.AddWhere(FlowFormTreeAttr.ParentNo, "01");
        //        obj.addOrderBy(FlowFormTreeAttr.Idx);
        //        obj.DoQuery();
        //    }
        //    // Process Form 
        //    FlowForms flowForms = new FlowForms();
        //    flowForms.RetrieveByAttr(FlowFormAttr.FK_Flow, flowId);
        //    // Process node form 
        //    NodeForms nodeForms = new NodeForms();
        //    nodeForms.RetrieveByAttr(NodeFormAttr.FK_Node, nodeId);
        //    // If the node form does not exist 
        //    if (nodeForms == null || nodeForms.Count == 0)
        //    {
        //        InitNodeForms(flowId, nodeId);
        //        // Requeried 
        //        nodeForms = new NodeForms();
        //        nodeForms.RetrieveByAttr(NodeFormAttr.FK_Node, nodeId);
        //    }
        //    //add root
        //    BP.WF.Template.FlowFormTree root = new BP.WF.Template.FlowFormTree();
        //    root.No = "01";
        //    root.ParentNo = "0";
        //    root.Name = " Table of Contents ";
        //    root.NodeType = "root";
        //    appFlowFormTree.Clear();
        //    appFlowFormTree.AddEntity(root);
        //    // Add to json
        //    foreach (BP.WF.Template.FlowFormTree item in flowFormTree)
        //    {
        //        item.NodeType = "folder";
        //        appFlowFormTree.AddEntity(item);
        //        foreach (FlowForm flowForm in flowForms)
        //        {
        //            if (item.No != flowForm.FK_FlowFormTree)
        //                continue;

        //            foreach (NodeForm nodeForm in nodeForms)
        //            {
        //                if (nodeForm.FK_SysForm == flowForm.FK_SysForm)
        //                {
        //                    SysForm sysForm = new SysForm(nodeForm.FK_SysForm);
        //                    BP.WF.Template.FlowFormTree formTree = new BP.WF.Template.FlowFormTree();
        //                    formTree.No = nodeForm.FK_SysForm;
        //                    formTree.ParentNo = item.No;
        //                    formTree.Name = sysForm.Name;
        //                    formTree.NodeType = "form";
        //                    appFlowFormTree.AddEntity(formTree);
        //                }
        //            }
        //        }
        //        // Add a child node 
        //        GetChildTreeNode(flowId, item.No, flowForms, nodeForms);
        //    }
        //    // Expansion Tool 
        //    NodeToolbars extToolBars = new NodeToolbars();
        //    extToolBars.RetrieveByAttr(NodeToolbarAttr.FK_Node, nodeId);

        //    foreach (NodeToolbar item in extToolBars)
        //    {
        //        string url = "";
        //        if (string.IsNullOrEmpty(item.Url))
        //            continue;

        //        url = item.Url;

        //        BP.WF.Template.FlowFormTree formTree = new BP.WF.Template.FlowFormTree();
        //        formTree.No = item.OID.ToString();
        //        formTree.ParentNo = "01";
        //        formTree.Name = item.Title;
        //        formTree.NodeType = "tools";
        //        formTree.Url = url;
        //        appFlowFormTree.AddEntity(formTree);
        //    }
        //    TansEntitiesToGenerTree(appFlowFormTree, root.No, "");
        //    return appendMenus.ToString();
        //}

        //private void GetChildTreeNode(string flowId, string parentNo, FlowForms flowForms, NodeForms nodeForms)
        //{
        //    // Process parent tree form 
        //    BP.WF.Template.FlowFormTrees flowFormTree = new BP.WF.Template.FlowFormTrees();
        //    QueryObject obj = new QueryObject(flowFormTree);
        //    obj.AddWhere(FlowFormTreeAttr.FK_Flow, flowId);
        //    obj.addAnd();
        //    obj.AddWhere(FlowFormTreeAttr.ParentNo, parentNo);
        //    obj.addOrderBy(FlowFormTreeAttr.Name);
        //    obj.DoQuery();

        //    foreach (BP.WF.Template.FlowFormTree item in flowFormTree)
        //    {
        //        item.NodeType = "folder";
        //        appFlowFormTree.AddEntity(item);
        //        foreach (FlowForm flowForm in flowForms)
        //        {
        //            if (item.No != flowForm.FK_FlowFormTree)
        //                continue;

        //            foreach (NodeForm nodeForm in nodeForms)
        //            {
        //                if (nodeForm.FK_SysForm == flowForm.FK_SysForm)
        //                {
        //                    SysForm sysForm = new SysForm(nodeForm.FK_SysForm);
        //                    BP.WF.Template.FlowFormTree formTree = new BP.WF.Template.FlowFormTree();
        //                    formTree.No = nodeForm.FK_SysForm;
        //                    formTree.ParentNo = item.No;
        //                    formTree.Name = sysForm.Name;
        //                    formTree.NodeType = "form";
        //                    appFlowFormTree.AddEntity(formTree);
        //                }
        //            }
        //        }
        //        // Add a child node 
        //        GetChildTreeNode(flowId, item.No, flowForms, nodeForms);
        //    }
        //}

        #region  The initial node tree form 
        /// <summary>
        ///  The initial node tree form 
        /// </summary>
        //private void InitFlowFormTree(string flowId)
        //{
        //    BP.WF.Flow flow = new Flow(flowId);
        //    // Get templates 
        //    SysFormTree sysFormTree = new SysFormTree();
        //    sysFormTree.RetrieveByAttr(SysFormTreeAttr.Name, flow.Name);

        //    if (sysFormTree != null)
        //    {
        //        // Add to Process 
        //        BP.WF.Template.FlowFormTree flowFormTree = new BP.WF.Template.FlowFormTree();
        //        QueryObject objFlowFormTree = new QueryObject(flowFormTree);
        //        objFlowFormTree.AddWhere("No", "01");
        //        objFlowFormTree.DoQuery();

        //        if (flowFormTree == null || flowFormTree.Name == "")
        //        {
        //            flowFormTree = new BP.WF.Template.FlowFormTree();
        //            flowFormTree.No = "01";
        //            flowFormTree.Name = " Table of Contents ";
        //            flowFormTree.ParentNo = "0";
        //            flowFormTree.Idx = 0;
        //            flowFormTree.IsDir = true;
        //            flowFormTree.DirectInsert();
        //        }
        //        string subNo = flowFormTree.DoCreateSubNode().No;
        //        flowFormTree = new BP.WF.Template.FlowFormTree(subNo);
        //        flowFormTree.Name = flow.Name;
        //        flowFormTree.FK_Flow = flowId;
        //        flowFormTree.Update();
        //        // Adding a child 
        //        InitChildNode(flowId, sysFormTree.No, subNo);
        //    }
        //}

        //private void InitNodeForms(string flowId, string nodeId)
        //{
        //    FlowForms flowForm = new FlowForms(flowId);

        //    NodeForm fnodeForm = new NodeForm();
        //    fnodeForm.Delete(NodeFormAttr.FK_Node, nodeId);
        //    foreach (FlowForm item in flowForm)
        //    {
        //        fnodeForm = new NodeForm();
        //        fnodeForm.FK_Node = nodeId;
        //        fnodeForm.FK_SysForm = item.FK_SysForm;
        //        fnodeForm.Insert();
        //    }
        //}

        //private void InitChildNode(string flowId, string parentNo, string wfParetNo)
        //{
        //    // Get templates 
        //    SysFormTrees formTrees = new SysFormTrees();
        //    QueryObject objInfo = new QueryObject(formTrees);
        //    objInfo.AddWhere(SysFormTreeAttr.ParentNo, parentNo);
        //    objInfo.addOrderBy(SysFormTreeAttr.Name);
        //    objInfo.DoQuery();

        //    BP.WF.Template.FlowFormTree pFlowFormTree = new BP.WF.Template.FlowFormTree(wfParetNo);

        //    foreach (SysFormTree item in formTrees)
        //    {
        //        string subNo = pFlowFormTree.DoCreateSubNode().No;
        //        BP.WF.Template.FlowFormTree flowFormTree = new BP.WF.Template.FlowFormTree(subNo);
        //        flowFormTree.Name = item.Name;
        //        flowFormTree.FK_Flow = flowId;
        //        flowFormTree.Update();
        //        InitChildNode(flowId, item.No, subNo);
        //    }
        //    // Form tree form 
        //    SysForm sysForm = new SysForm();
        //    DataTable dt_Forms = sysForm.RunSQLReturnTable("SELECT No,Name FROM Sys_MapData WHERE FK_FormTree='" + parentNo + "' ORDER BY Name");
        //    // Process Form 
        //    foreach (DataRow row in dt_Forms.Rows)
        //    {
        //        FlowForm flowForm = new FlowForm();
        //        flowForm.FK_Flow = flowId;
        //        flowForm.FK_SysForm = row["No"].ToString();
        //        flowForm.FK_FlowFormTree = wfParetNo;
        //        flowForm.Insert();
        //    }
        //}
        #endregion

        /// <summary>
        ///  The entity into a tree 
        /// </summary>
        /// <param name="ens"></param>
        /// <param name="rootNo"></param>
        /// <param name="checkIds"></param>
        StringBuilder appendMenus = new StringBuilder();
        StringBuilder appendMenuSb = new StringBuilder();
        public void TansEntitiesToGenerTree(Entities ens, string rootNo, string checkIds)
        {
            EntityMultiTree root = ens.GetEntityByKey(rootNo) as EntityMultiTree;
            if (root == null)
                throw new Exception("@ Not found rootNo=" + rootNo + " entity.");
            appendMenus.Append("[{");
            appendMenus.Append("\"id\":\"" + rootNo + "\"");
            appendMenus.Append(",\"text\":\"" + root.Name + "\"");

            //attributes
            BP.WF.Template.FlowFormTree formTree = root as BP.WF.Template.FlowFormTree;
            if (formTree != null)
            {
                string url = formTree.Url == null ? "" : formTree.Url;
                url = url.Replace("/", "|");
                appendMenus.Append(",\"attributes\":{\"NodeType\":\"" + formTree.NodeType + "\",\"Url\":\"" + url + "\"}");
            }
            //  Increase its children .
            appendMenus.Append(",\"children\":");
            AddChildren(root, ens, checkIds);
            appendMenus.Append(appendMenuSb);
            appendMenus.Append("}]");
        }

        public void AddChildren(EntityMultiTree parentEn, Entities ens, string checkIds)
        {
            appendMenus.Append(appendMenuSb);
            appendMenuSb.Clear();

            appendMenuSb.Append("[");
            foreach (EntityMultiTree item in ens)
            {
                if (item.ParentNo != parentEn.No)
                    continue;

                if (checkIds.Contains("," + item.No + ","))
                    appendMenuSb.Append("{\"id\":\"" + item.No + "\",\"text\":\"" + item.Name + "\",\"checked\":true");
                else
                    appendMenuSb.Append("{\"id\":\"" + item.No + "\",\"text\":\"" + item.Name + "\",\"checked\":false");


                //attributes
                BP.WF.Template.FlowFormTree formTree = item as BP.WF.Template.FlowFormTree;
                if (formTree != null)
                {
                    string url = formTree.Url == null ? "" : formTree.Url;
                    string ico = "icon-tree_folder";
                    url = url.Replace("/", "|");
                    appendMenuSb.Append(",\"attributes\":{\"NodeType\":\"" + formTree.NodeType + "\",\"Url\":\"" + url + "\"}");
                    // Icon 
                    if (formTree.NodeType == "form|0")
                    {
                        ico = "form0";
                    }
                    if (formTree.NodeType == "form|1")
                    {
                        ico = "form1";
                    }
                    if (formTree.NodeType.Contains("tools"))
                    {
                        ico = "icon-4";
                    }
                    appendMenuSb.Append(",iconCls:\"");
                    appendMenuSb.Append(ico);
                    appendMenuSb.Append("\"");
                }
                //  Increase its children .
                appendMenuSb.Append(",\"children\":");
                AddChildren(item, ens, checkIds);
                appendMenuSb.Append("},");
            }
            if (appendMenuSb.Length > 1)
                appendMenuSb = appendMenuSb.Remove(appendMenuSb.Length - 1, 1);
            appendMenuSb.Append("]");
            appendMenus.Append(appendMenuSb);
            appendMenuSb.Clear();
        }
    }
}