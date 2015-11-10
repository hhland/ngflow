using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using BP.WF;
using BP.DA;
using BP.Web;
using BP.Sys;
using BP.En;
using BP.WF.Data;
using BP.WF.Template;

namespace BP.WF
{
    /// <summary>
    ///  Cc 
    /// </summary>
    public class CCWork
    {
        #region  Property .
        /// <summary>
        ///  Work node 
        /// </summary>
        public WorkNode HisWorkNode = null;
        /// <summary>
        ///  Node 
        /// </summary>
        public Node HisNode
        {
            get
            {
                return this.HisWorkNode.HisNode;
            }
        }
        /// <summary>
        ///  Report form 
        /// </summary>
        public GERpt rptGe
        {
            get
            {
                return this.HisWorkNode.rptGe;
            }
        }
        /// <summary>
        ///  The work ID
        /// </summary>
        public Int64 WorkID
        {
            get
            {
                return this.HisWorkNode.WorkID;
            }
        }
        #endregion  Property .

        /// <summary>
        ///  Structure 
        /// </summary>
        /// <param name="wn"></param>
        public CCWork(WorkNode wn)
        {
            this.HisWorkNode = wn;
            AutoCC();
            CCByEmps();

        }
        public void AutoCC()
        {
            if (this.HisWorkNode.HisNode.HisCCRole == CCRole.AutoCC
               || this.HisWorkNode.HisNode.HisCCRole == CCRole.HandAndAuto)
            {
            }
            else
            {
                return;
            }

            /* If the automatic CC */
            CC cc = this.HisWorkNode.HisNode.HisCC;

            //  Execution Cc .
            DataTable dt = cc.GenerCCers(this.HisWorkNode.rptGe);
            if (dt.Rows.Count == 0)
                return;

            string ccMsg = "@ Messages automatically copied to ";
            string basePath = BP.WF.Glo.HostURL;
            string mailTemp = BP.DA.DataType.ReadTextFile2Html(BP.Sys.SystemConfig.PathOfDataUser + "\\EmailTemplete\\CC_" + WebUser.SysLang + ".txt");

            GenerWorkerLists gwls = null;
            if (this.HisWorkNode.town != null)
            {
                // Remove CC collection , If you wait to do this, there are people on the abolition of the CC of the officer .
                gwls = new GenerWorkerLists(this.WorkID, this.HisWorkNode.town.HisNode.NodeID);
            }
            foreach (DataRow dr in dt.Rows)
            {
                string toUserNo = dr[0].ToString();

                // If it contains to-do .
                if (gwls != null && gwls.Contains(GenerWorkerListAttr.FK_Emp, toUserNo) == true)
                    continue;

                string toUserName = dr[1].ToString();

                // Generate title and content .
                string ccTitle = cc.CCTitle.Clone() as string;
                ccTitle = BP.WF.Glo.DealExp(ccTitle, this.rptGe, null);

                string ccDoc = cc.CCDoc.Clone() as string;
                ccDoc = BP.WF.Glo.DealExp(ccDoc, this.rptGe, null);

                ccDoc = ccDoc.Replace("@Accepter", toUserNo);
                ccTitle = ccTitle.Replace("@Accepter", toUserNo);

                // CC Information .
                ccMsg += "(" + toUserNo + " - " + toUserName + ");";

                #region  If it is written to the CC list .
                if (this.HisNode.CCWriteTo == CCWriteTo.All || this.HisNode.CCWriteTo == CCWriteTo.CCList)
                {
                    /* If the request is written CC list .*/
                    CCList list = new CCList();
                    list.MyPK = this.HisWorkNode.WorkID + "_" + this.HisWorkNode.HisNode.NodeID + "_" + dr[0].ToString();
                    list.FK_Flow = this.HisWorkNode.HisNode.FK_Flow;
                    list.FlowName = this.HisWorkNode.HisNode.FlowName;
                    list.FK_Node = this.HisWorkNode.HisNode.NodeID;
                    list.NodeName = this.HisWorkNode.HisNode.Name;
                    list.Title = ccTitle;
                    list.Doc = ccDoc;
                    list.CCTo = dr[0].ToString();
                    list.CCToName = dr[1].ToString();

                    list.RDT = DataType.CurrentDataTime;
                    list.Rec = WebUser.No;
                    list.WorkID = this.HisWorkNode.WorkID;
                    list.FID = this.HisWorkNode.HisWork.FID;
                    try
                    {
                        list.Insert();
                    }
                    catch
                    {
                        list.Update();
                    }
                }
                #endregion  If it is written to the CC list .

                #region  If you want to write to-do .
                if (this.HisNode.CCWriteTo == CCWriteTo.All || this.HisNode.CCWriteTo == CCWriteTo.Todolist)
                {
                    /* If you want to write to-do */
                    GenerWorkerList gwl = (GenerWorkerList)gwls[0];
                    gwl.FK_Emp=dr[0].ToString();
                    gwl.FK_EmpText = dr[1].ToString();
                    gwl.IsCC = true;
                    try
                    {
                        gwl.Insert();
                    }
                    catch
                    {
                          /* There may be , Cc and to-do people repeat .*/
                    }
                }
                #endregion  If you want to write to-do .

                if (BP.WF.Glo.IsEnableSysMessage == true)
                {
                    //     // Write message prompts .
                    //     ccMsg += list.CCTo + "(" + dr[1].ToString() + ");";
                    //     BP.WF.Port.WFEmp wfemp = new Port.WFEmp(list.CCTo);
                    //     string sid = list.CCTo + "_" + list.WorkID + "_" + list.FK_Node + "_" + list.RDT;
                    //     string url = basePath + "WF/Do.aspx?DoType=OF&SID=" + sid;
                    //     string urlWap = basePath + "WF/Do.aspx?DoType=OF&SID=" + sid + "&IsWap=1";
                    //     string mytemp = mailTemp.Clone() as string;
                    //     mytemp = string.Format(mytemp, wfemp.Name, WebUser.Name, url, urlWap);
                    //     string title = string.Format(" Work Cc :{0}. The work :{1}, Sender :{2}, You need to consult ",
                    //this.HisNode.FlowName, this.HisNode.Name, WebUser.Name);
                    //     BP.WF.Dev2Interface.Port_SendMsg(wfemp.No, title, mytemp, null, BP.Sys.SMSMsgType.CC, list.FK_Flow, list.FK_Node, list.WorkID, list.FID);
                }
            }

            this.HisWorkNode.addMsg(SendReturnMsgFlag.CCMsg, ccMsg);

            // Written to the log .
            this.HisWorkNode.AddToTrack(ActionType.CC,WebUser.No,WebUser.Name,this.HisNode.NodeID,this.HisNode.Name, ccMsg, this.HisNode);
        }
        /// <summary>
        ///  By convention the field  SysCCEmps  System personnel .
        /// </summary>
        public void CCByEmps()
        {
            if (this.HisNode.HisCCRole != CCRole.BySysCCEmps)
                return;

            CC cc = this.HisNode.HisCC;

            // Generate title and content .
            string ccTitle = cc.CCTitle.Clone() as string;
            ccTitle = BP.WF.Glo.DealExp(ccTitle, this.rptGe, null);

            string ccDoc = cc.CCDoc.Clone() as string;
            ccDoc = BP.WF.Glo.DealExp(ccDoc, this.rptGe, null);

            // Remove the Cc list 
            string ccers = this.rptGe.GetValStrByKey("SysCCEmps");
            if (!string.IsNullOrEmpty(ccers))
            {
                string[] cclist = ccers.Split('|');
                Hashtable ht = new Hashtable();
                foreach (string item in cclist)
                {
                    string[] tmp = item.Split(',');
                    ht.Add(tmp[0], tmp[1]);
                }
                string ccMsg = "@ Messages automatically copied to ";
                string basePath = BP.WF.Glo.HostURL;

                string mailTemp = BP.DA.DataType.ReadTextFile2Html(BP.Sys.SystemConfig.PathOfDataUser + "\\EmailTemplete\\CC_" + WebUser.SysLang + ".txt");
                foreach (DictionaryEntry item in ht)
                {
                    ccDoc = ccDoc.Replace("@Accepter", item.Value.ToString());
                    ccTitle = ccTitle.Replace("@Accepter", item.Value.ToString());

                    // CC Information .
                    ccMsg += "(" + item.Value.ToString() +" - "+ item.Value.ToString() + ");";

                    CCList list = new CCList();
                    list.MyPK = this.WorkID + "_" + this.HisNode.NodeID + "_" + item.Key.ToString();
                    list.FK_Flow = this.HisNode.FK_Flow;
                    list.FlowName = this.HisNode.FlowName;
                    list.FK_Node = this.HisNode.NodeID;
                    list.NodeName = this.HisNode.Name;
                    list.Title = ccTitle;
                    list.Doc = ccDoc;
                    list.CCTo = item.Key.ToString();
                    list.CCToName = item.Value.ToString();
                    list.RDT = DataType.CurrentDataTime;
                    list.Rec = WebUser.No;
                    list.WorkID = this.WorkID;
                    list.FID = this.HisWorkNode.HisWork.FID;
                    try
                    {
                        list.Insert();
                    }
                    catch
                    {
                        list.CheckPhysicsTable();
                        list.Update();
                    }


                    if (BP.WF.Glo.IsEnableSysMessage == true)
                    {
                        ccMsg += list.CCTo + "(" + item.Value.ToString() + ");";
                        BP.WF.Port.WFEmp wfemp = new Port.WFEmp(list.CCTo);

                        string sid = list.CCTo + "_" + list.WorkID + "_" + list.FK_Node + "_" + list.RDT;
                        string url = basePath + "WF/Do.aspx?DoType=OF&SID=" + sid;
                        url = url.Replace("//", "/");
                        url = url.Replace("//", "/");

                        string urlWap = basePath + "WF/Do.aspx?DoType=OF&SID=" + sid + "&IsWap=1";
                        urlWap = urlWap.Replace("//", "/");
                        urlWap = urlWap.Replace("//", "/");

                        string mytemp = mailTemp.Clone() as string;
                        mytemp = string.Format(mytemp, wfemp.Name, WebUser.Name, url, urlWap);

                        string title = string.Format(" Work Cc :{0}. The work :{1}, Sender :{2}, You need to consult ",
                   this.HisNode.FlowName, this.HisNode.Name, WebUser.Name);

                        BP.WF.Dev2Interface.Port_SendMsg(wfemp.No, title, mytemp, null, BP.WF.SMSMsgType.CC, list.FK_Flow, list.FK_Node, list.WorkID, list.FID);
                    }
                }

                // Write to system messages .
                this.HisWorkNode.addMsg(SendReturnMsgFlag.CCMsg, ccMsg);

                // Written to the log .
                this.HisWorkNode.AddToTrack(ActionType.CC, WebUser.No, WebUser.Name, this.HisNode.NodeID, this.HisNode.Name, ccMsg, this.HisNode);

            }
        }
    }
}
