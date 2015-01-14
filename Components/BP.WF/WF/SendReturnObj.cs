using System;
using System.Collections.Generic;
using System.Text;

namespace BP.WF
{
    /// <summary>
    ///  Message Type 
    /// </summary>
    public enum SendReturnMsgType
    {
        /// <summary>
        ///  News 
        /// </summary>
        Info,
        /// <summary>
        ///  System Messages 
        /// </summary>
        SystemMsg
    }
    /// <summary>
    ///  Message mark 
    /// </summary>
    public class SendReturnMsgFlag
    {
        /// <summary>
        ///  In line with the completion of the workflow conditions 
        /// </summary>
        public const string MacthFlowOver = "MacthFlowOver";
        /// <summary>
        ///  Current work [{0}] Has been completed 
        /// </summary>
        public const string CurrWorkOver = "CurrWorkOver";
        /// <summary>
        ///  Meet the closing conditions , The process is completed 
        /// </summary>
        public const string FlowOverByCond = "FlowOverByCond";
        /// <summary>
        ///  To staff 
        /// </summary>
        public const string ToEmps = "ToEmps";
        /// <summary>
        ///  To extended information officer 
        /// </summary>
        public const string ToEmpExt = "ToEmpExt";
        /// <summary>
        ///  Assignments 
        /// </summary>
        public const string AllotTask = "AllotTask";
        /// <summary>
        ///  End confluence 
        /// </summary>
        public const string HeLiuOver = "HeLiuOver";
        /// <summary>
        ///  Report 
        /// </summary>
        public const string WorkRpt = "WorkRpt";
        /// <summary>
        ///  Start node 
        /// </summary>
        public const string WorkStartNode = "WorkStartNode";
        /// <summary>
        ///  Work started 
        /// </summary>
        public const string WorkStart = "WorkStart";
        /// <summary>
        ///  Process ends 
        /// </summary>
        public const string FlowOver = "FlowOver";
        /// <summary>
        ///  After the success of abnormal events sent 
        /// </summary>
        public const string SendSuccessMsgErr = "SendSuccessMsgErr";
        /// <summary>
        ///  Information sent successfully 
        /// </summary>
        public const string SendSuccessMsg = "SendSuccessMsg";
        /// <summary>
        ///  Separation process information 
        /// </summary>
        public const string FenLiuInfo = "FenLiuInfo";
        /// <summary>
        ///  Send a copy of the message 
        /// </summary>
        public const string CCMsg = "CCMsg";
        /// <summary>
        ///  Edit recipient 
        /// </summary>
        public const string EditAccepter = "EditAccepter";
        /// <summary>
        ///  New Process 
        /// </summary>
        public const string NewFlowUnSend = "NewFlowUnSend";
        /// <summary>
        ///  Send revocation 
        /// </summary>
        public const string UnSend = "UnSend";
        /// <summary>
        ///  Report form 
        /// </summary>
        public const string Rpt = "Rpt";
        /// <summary>
        ///  Sent 
        /// </summary>
        public const string SendWhen = "SendWhen";
        /// <summary>
        ///  The current process ends 
        /// </summary>
        public const string End = "End";
        /// <summary>
        ///  The current process is completed 
        /// </summary>
        public const string OverCurr = "OverCurr";
        /// <summary>
        ///  Flow direction information 
        /// </summary>
        public const string CondInfo = "CondInfo";
        /// <summary>
        ///  A node is completed 
        /// </summary>
        public const string OneNodeSheetver = "OneNodeSheetver";
        /// <summary>
        ///  Document information 
        /// </summary>
        public const string BillInfo = "BillInfo";
        /// <summary>
        ///  Text information ( The system does not generate )
        /// </summary>
        public const string MsgOfText = "MsgOfText";
        /// <summary>
        ///  Listen to the message information 
        /// </summary>
        public const string ListenInfo = "ListenInfo";
        /// <summary>
        ///  The process is finished ?
        /// </summary>
        public const string IsStopFlow = "IsStopFlow";

        #region  System Variables 
        /// <summary>
        ///  The work ID
        /// </summary>
        public const string VarWorkID = "VarWorkID";
        /// <summary>
        ///  The current node ID
        /// </summary>
        public const string VarCurrNodeID = "VarCurrNodeID";
        /// <summary>
        ///  The current node name 
        /// </summary>
        public const string VarCurrNodeName = "VarCurrNodeName";
        /// <summary>
        ///  Arrives at a node ID
        /// </summary>
        public const string VarToNodeID = "VarToNodeID";
        /// <summary>
        ///  Set of nodes reachable 
        /// </summary>
        public const string VarToNodeIDs = "VarToNodeIDs";
        /// <summary>
        ///  Arrives at a node name 
        /// </summary>
        public const string VarToNodeName = "VarToNodeName";
        /// <summary>
        ///  People accept the collection name ( Separated by commas )
        /// </summary>
        public const string VarAcceptersName = "VarAcceptersName";
        /// <summary>
        ///  Recipient collection ID( Separated by commas )
        /// </summary>
        public const string VarAcceptersID = "VarAcceptersID";
        /// <summary>
        ///  Recipient collection ID Name( Separated by commas )
        /// </summary>
        public const string VarAcceptersNID = "VarAcceptersNID";
        /// <summary>
        ///  Child threads WorkIDs
        /// </summary>
        public const string VarTreadWorkIDs = "VarTreadWorkIDs";
        #endregion  System Variables 
    }
    /// <summary>
    ///  Send return objects work 
    /// </summary>
    public class SendReturnObj
    {
        /// <summary>
        ///  Message mark 
        /// </summary>
        public string MsgFlag = null;
        /// <summary>
        ///  Message tag description 
        /// </summary>
        public string MsgFlagDesc
        {
            get
            {
                if (MsgFlag == null)
                    throw new Exception("@ No tag ");

                switch (MsgFlag)
                {
                    case SendReturnMsgFlag.VarAcceptersID:
                        return " Recipient ID";
                    case SendReturnMsgFlag.VarAcceptersName:
                        return " Recipient Name ";
                    case SendReturnMsgFlag.VarAcceptersNID:
                        return " Recipient ID Set ";
                    case SendReturnMsgFlag.VarCurrNodeID:
                        return " The current node ID";
                    case SendReturnMsgFlag.VarCurrNodeName:
                        return " People accept the collection name ( Separated by commas )";
                    case SendReturnMsgFlag.VarToNodeID:
                        return " Arrives at a node ID";
                    case SendReturnMsgFlag.VarToNodeName:
                        return " Arrives at a node name ";
                    case SendReturnMsgFlag.VarTreadWorkIDs:
                        return " Child threads WorkIDs";
                    case SendReturnMsgFlag.BillInfo:
                        return " Document information ";
                    case SendReturnMsgFlag.CCMsg:
                        return " CC Information ";
                    case SendReturnMsgFlag.CondInfo:
                        return " Conditions Information ";
                    case SendReturnMsgFlag.CurrWorkOver:
                        return " The current work has been completed ";
                    case SendReturnMsgFlag.EditAccepter:
                        return " Edit recipient ";
                    case SendReturnMsgFlag.End:
                        return " The current process has ended ";
                    case SendReturnMsgFlag.FenLiuInfo:
                        return " Streaming information ";
                    case SendReturnMsgFlag.FlowOver:
                        return " The current process has been completed ";
                    case SendReturnMsgFlag.FlowOverByCond:
                        return " Meet the closing conditions , The process is completed .";
                    case SendReturnMsgFlag.HeLiuOver:
                        return " Triage complete ";
                    case SendReturnMsgFlag.MacthFlowOver:
                        return " In line with the completion of the workflow conditions ";
                    case SendReturnMsgFlag.NewFlowUnSend:
                        return " New Process ";
                    case SendReturnMsgFlag.OverCurr:
                        return " The current process is completed ";
                    case SendReturnMsgFlag.Rpt:
                        return " Report form ";
                    case SendReturnMsgFlag.SendSuccessMsg:
                        return " Information sent successfully ";
                    case SendReturnMsgFlag.SendSuccessMsgErr:
                        return " Send Error ";
                    case SendReturnMsgFlag.SendWhen:
                        return " Sent ";
                    case SendReturnMsgFlag.ToEmps:
                        return " Reach staff ";
                    case SendReturnMsgFlag.UnSend:
                        return " Undo Send ";
                    case SendReturnMsgFlag.ToEmpExt:
                        return " To extended information officer ";
                    case SendReturnMsgFlag.VarWorkID:
                        return " The work ID";
                    case SendReturnMsgFlag.IsStopFlow:
                        return " The process is finished ";
                    case SendReturnMsgFlag.WorkRpt:
                        return " Report ";
                    case SendReturnMsgFlag.WorkStartNode:
                        return " Start node ";
                    default:
                        throw new Exception("@ No judgment tags ...");
                }
            }
        }
        /// <summary>
        ///  Message Type 
        /// </summary>
        public SendReturnMsgType HisSendReturnMsgType = SendReturnMsgType.Info;
        /// <summary>
        ///  Message content 
        /// </summary>
        public string MsgOfText = null;
        /// <summary>
        ///  Message content Html
        /// </summary>
        public string MsgOfHtml = null;
        /// <summary>
        ///  Send a message 
        /// </summary>
        public SendReturnObj()
        {
        }
    }
    /// <summary>
    ///  Send to return a collection of objects work .
    /// </summary>
    public class SendReturnObjs:System.Collections.CollectionBase
    {
        #region  Get System Variables .
        public Int64 VarWorkID
        {
            get
            {
                foreach (SendReturnObj item in this)
                {
                    if (item.MsgFlag == SendReturnMsgFlag.VarWorkID)
                        return Int64.Parse(item.MsgOfText);
                }
                return 0;
            }
        }
        public bool IsStopFlow
        {
             get
            {
                foreach (SendReturnObj item in this)
                {
                    if (item.MsgFlag == SendReturnMsgFlag.IsStopFlow)
                    {
                        if (item.MsgOfText == "1")
                            return true;
                        else
                            return false;
                    }
                }
                throw new Exception("@ Did not find the system variables IsStopFlow");
            }
        }

        /// <summary>
        ///  Arrives at a node ID
        /// </summary>
        public int VarToNodeID
        {
            get
            {
                foreach (SendReturnObj item in this)
                {
                    if (item.MsgFlag == SendReturnMsgFlag.VarToNodeID)
                        return int.Parse( item.MsgOfText);
                }
                return 0;
            }
        }
        /// <summary>
        ///  Arrives at a node IDs
        /// </summary>
        public string VarToNodeIDs
        {
            get
            {
                foreach (SendReturnObj item in this)
                {
                    if (item.MsgFlag == SendReturnMsgFlag.VarToNodeIDs)
                        return item.MsgOfText;
                }
                return null;
            }
        }
        /// <summary>
        ///  Arrives at a node name 
        /// </summary>
        public string VarToNodeName
        {
            get
            {
                foreach (SendReturnObj item in this)
                {
                    if (item.MsgFlag == SendReturnMsgFlag.VarToNodeName)
                        return item.MsgOfText;
                }
                return " Variable not found .";
            }
        }
        /// <summary>
        ///  Node name arrival 
        /// </summary>
        public string VarCurrNodeName
        {
            get
            {
                foreach (SendReturnObj item in this)
                {
                    if (item.MsgFlag == SendReturnMsgFlag.VarCurrNodeName)
                        return  item.MsgOfText;
                }
                return null;
            }
        }
        public int VarCurrNodeID
        {
            get
            {
                foreach (SendReturnObj item in this)
                {
                    if (item.MsgFlag == SendReturnMsgFlag.VarCurrNodeID)
                        return int.Parse( item.MsgOfText);
                }
                return 0;
            }
        }
        /// <summary>
        ///  Recipient 
        /// </summary>
        public string VarAcceptersName
        {
            get
            {
                foreach (SendReturnObj item in this)
                {
                    if (item.MsgFlag == SendReturnMsgFlag.VarAcceptersName)
                        return item.MsgOfText;
                }
                return null;
            }
        }
        /// <summary>
        ///  Recipient IDs
        /// </summary>
        public string VarAcceptersID
        {
            get
            {
                foreach (SendReturnObj item in this)
                {
                    if (item.MsgFlag == SendReturnMsgFlag.VarAcceptersID)
                        return item.MsgOfText;
                }
                return null;
            }
        }
        /// <summary>
        ///  Shunt child thread to child thread when sending generated WorkIDs,  There are multiple comma-separated .
        /// </summary>
        public string VarTreadWorkIDs
        {
            get
            {
                foreach (SendReturnObj item in this)
                {
                    if (item.MsgFlag == SendReturnMsgFlag.VarTreadWorkIDs)
                        return item.MsgOfText;
                }
                return null;
            }
        }
        #endregion

        /// <summary>
        ///  Export text News 
        /// </summary>
        public string OutMessageText = null;
        /// <summary>
        ///  Export html Information 
        /// </summary>
        public string OutMessageHtml = null;
        /// <summary>
        ///  Increase news 
        /// </summary>
        /// <param name="msgFlag"> Message mark </param>
        /// <param name="msg"> Text messages </param>
        /// <param name="msgOfHtml">html News </param>
        /// <param name="type"> Message Type </param>
        public void AddMsg(string msgFlag, string msg, string msgOfHtml, SendReturnMsgType type)
        {
            SendReturnObj obj = new SendReturnObj();
            obj.MsgFlag = msgFlag;
            obj.MsgOfText = msg;
            obj.MsgOfHtml = msgOfHtml;
            obj.HisSendReturnMsgType = type;
            foreach (SendReturnObj item in this)
            {
                if (item.MsgFlag == msgFlag)
                {
                    item.MsgFlag = msgFlag;
                    item.MsgOfText = msg;
                    item.MsgOfHtml = msgOfHtml;
                    item.HisSendReturnMsgType = type;
                    return;
                }
            }
            this.InnerList.Add(obj);
        }
        /// <summary>
        ///  Transforming into a special format 
        /// </summary>
        /// <returns></returns>
        public string ToMsgOfSpecText()
        {
            string msg = "";
            foreach (SendReturnObj item in this)
            {
                if (item.MsgOfText != null)
                {
                    msg += "$" + item.MsgFlag + "^" + item.MsgOfText;
                }
            }

            // Increase the  text Information .
            msg += "$" + BP.WF.SendReturnMsgFlag.MsgOfText + "^" + this.ToMsgOfText();

            msg.Replace("@@", "@");
            return msg;
        }
        /// <summary>
        ///  Converted into text Messages way , To facilitate the recognition does not come out html The device output .
        /// </summary>
        /// <returns></returns>
        public string ToMsgOfText()
        {
            if (this.OutMessageText != null)
                return this.OutMessageText;

            string msg = "";
            foreach (SendReturnObj item in this)
            {
                if (item.HisSendReturnMsgType == SendReturnMsgType.SystemMsg)
                    continue;

                // Special judge .
                if (item.MsgFlag == SendReturnMsgFlag.IsStopFlow)
                {
                    msg += "@" + item.MsgOfHtml;
                    continue; 
                }


                if (item.MsgOfText != null   )
                {
                    if (item.MsgOfText.Contains("<"))
                    {
#warning  Should not appear .
                        BP.DA.Log.DefaultLogWriteLineWarning("@ There are text messages html Mark :" + item.MsgOfText);
                        continue;
                    }
                    msg += "@" + item.MsgOfText;
                    continue; 
                }

            }
            msg.Replace("@@", "@");
            return msg;
        }
        /// <summary>
        ///  Converted into html Messages way , To facilitate html The information output .
        /// </summary>
        /// <returns></returns>
        public string ToMsgOfHtml()
        {
            if (this.OutMessageHtml != null)
                return this.OutMessageHtml;

            string msg = "";
            foreach (SendReturnObj item in this)
            {
                if (item.HisSendReturnMsgType != SendReturnMsgType.Info)
                    continue;

                if (item.MsgOfHtml != null)
                {
                    msg += "@" + item.MsgOfHtml;
                    continue;
                }

                if (item.MsgOfText != null)
                {
                    msg += "@" + item.MsgOfText;
                    continue;
                }
            }
            msg = msg.Replace("@@", "@");
            msg = msg.Replace("@@", "@");
            if (msg == "@")
                return "@ The process has been completed .";

            return msg;
        }
    }
}
