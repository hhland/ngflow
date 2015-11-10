using System;
using System.Text;
using System.Collections;
using BP.DA;
using BP.En;
using BP.En;
using BP.Port;
using BP.Web;
using System.Data.OracleClient;

namespace BP.Sys
{
    /// <summary>
    ///  Message control 
    /// </summary>
    public enum MsgCtrl
    {
        /// <summary>
        /// bufasong 
        /// </summary>
        None,
        /// <summary>
        ///  In accordance with the calculation set 
        /// </summary>
        BySet,
        /// <summary>
        ///  Whether to send the form in accordance with calculated fields , Field :IsSendMsg
        /// </summary>
        ByFrmIsSendMsg,
        /// <summary>
        ///  According to SDK Parameter calculation .
        /// </summary>
        BySDK
    }
    public enum EventDoType
    {
        /// <summary>
        ///  Disable 
        /// </summary>
        Disable=0,
        /// <summary>
        ///  Execute a stored procedure 
        /// </summary>
        SP=1,
        /// <summary>
        ///  Run SQL
        /// </summary>
        SQL=2,
        /// <summary>
        ///  Custom URL
        /// </summary>
        URLOfSelf=3,
        /// <summary>
        ///  Custom WS
        /// </summary>
        WSOfSelf=4,
        /// <summary>
        /// EXE
        /// </summary>
        EXE=5,
        /// <summary>
        ///  Base class 
        /// </summary>
        EventBase=6,
        /// <summary>
        /// JS
        /// </summary>
        Javascript=7
    }
    public class FrmEventList
    {
        /// <summary>
        ///  Form before loading 
        /// </summary>
        public const string FrmLoadBefore = "FrmLoadBefore";
        /// <summary>
        ///  After loading the form 
        /// </summary>
        public const string FrmLoadAfter = "FrmLoadAfter";
        /// <summary>
        ///  Form before saving 
        /// </summary>
        public const string SaveBefore = "SaveBefore";
        /// <summary>
        ///  Forms saved 
        /// </summary>
        public const string SaveAfter = "SaveAfter";
    }
    /// <summary>
    ///  Mark List 
    /// </summary>
    public class EventListOfNode : FrmEventList
    {
        #region  Node event 
        /// <summary>
        ///  Node before sending 
        /// </summary>
        public const string SendWhen = "SendWhen";
        /// <summary>
        ///  After the node sent successfully 
        /// </summary>
        public const string SendSuccess = "SendSuccess";
        /// <summary>
        ///  After sending node failure 
        /// </summary>
        public const string SendError = "SendError";
        /// <summary>
        ///  When a node before returning 
        /// </summary>
        public const string ReturnBefore = "ReturnBefore";
        /// <summary>
        ///  When the node back 
        /// </summary>
        public const string ReturnAfter = "ReturnAfter";
        /// <summary>
        ///  When withdrawn before sending node 
        /// </summary>
        public const string UndoneBefore = "UndoneBefore";
        /// <summary>
        ///  When sending node revocation 
        /// </summary>
        public const string UndoneAfter = "UndoneAfter";
        /// <summary>
        ///  After the transfer of the current node 
        /// </summary>
        public const string ShitAfter = "ShitAfter";
        /// <summary>
        ///  When the node plus sign 
        /// </summary>
        public const string AskerAfter = "AskerAfter";
       /// <summary>
       ///  When the node endorsement reply 
       /// </summary>
        public const string AskerReAfter = "AskerReAfter";
        #endregion  Node event 

        #region  Process events 
        /// <summary>
        ///  When the process is completed .
        /// </summary>
        public const string FlowOverBefore = "FlowOverBefore";
        /// <summary>
        ///  After .
        /// </summary>
        public const string FlowOverAfter = "FlowOverAfter";
        /// <summary>
        ///  Process before deleting 
        /// </summary>
        public const string BeforeFlowDel = "BeforeFlowDel";
        /// <summary>
        ///  Process deleted 
        /// </summary>
        public const string AfterFlowDel = "AfterFlowDel";
        #endregion  Process events 
    }
	/// <summary>
	///  Event Properties 
	/// </summary>
    public class FrmEventAttr
    {
        /// <summary>
        ///  Event Type 
        /// </summary>
        public const string FK_Event = "FK_Event";
        /// <summary>
        ///  Node ID
        /// </summary>
        public const string FK_MapData = "FK_MapData";
        /// <summary>
        ///  Execution Type 
        /// </summary>
        public const string DoType = "DoType";
        /// <summary>
        ///  Perform content 
        /// </summary>
        public const string DoDoc = "DoDoc";
        /// <summary>
        ///  Label 
        /// </summary>
        public const string MsgOK = "MsgOK";
        /// <summary>
        ///  Execution error 
        /// </summary>
        public const string MsgError = "MsgError";


        #region  Message Settings .
        /// <summary>
        ///  Control mode 
        /// </summary>
        public const string MsgCtrl = "MsgCtrl";
        /// <summary>
        ///  Whether to enable sending mail 
        /// </summary>
        public const string MsgMailEnable = "MsgMailEnable";
        /// <summary>
        ///  Message title 
        /// </summary>
        public const string MailTitle = "MailTitle";
        /// <summary>
        ///  Message content templates 
        /// </summary>
        public const string MailDoc = "MailDoc";
        /// <summary>
        ///  Whether SMS enabled 
        /// </summary>
        public const string SMSEnable = "SMSEnable";
        /// <summary>
        ///  SMS content templates 
        /// </summary>
        public const string SMSDoc = "SMSDoc";
        /// <summary>
        ///  Whether push ?
        /// </summary>
        public const string MobilePushEnable = "MobilePushEnable";
        #endregion  Message Settings .
    }
	/// <summary>
	///  Event 
	///  Save the event node node consists of two parts .	 
	///  Records from one node to the other nodes of the plurality of .
	///  Also recorded to the other nodes of this node .
	/// </summary>
    public class FrmEvent : EntityMyPK
    {
        #region  Basic properties 
        public override En.UAC HisUAC
        {
            get
            {
                UAC uac = new En.UAC();
                uac.IsAdjunct = false;
                uac.IsDelete = false;
                uac.IsInsert = false;
                uac.IsUpdate = true;
                return uac;
            }
        }
        /// <summary>
        ///  Node 
        /// </summary>
        public string FK_MapData
        {
            get
            {
                return this.GetValStringByKey(FrmEventAttr.FK_MapData);
            }
            set
            {
                this.SetValByKey(FrmEventAttr.FK_MapData, value);
            }
        }
        public string DoDoc
        {
            get
            {
                return this.GetValStringByKey(FrmEventAttr.DoDoc).Replace("~", "'");
            }
            set
            {
                string doc = value.Replace("'", "~");
                this.SetValByKey(FrmEventAttr.DoDoc, doc);
            }
        }
        /// <summary>
        ///  Successful implementation tips 
        /// </summary>
        public string MsgOK(Entity en)
        {
            string val = this.GetValStringByKey(FrmEventAttr.MsgOK);
            if (val.Trim() == "")
                return "";

            if (val.IndexOf('@') == -1)
                return val;

            foreach (Attr attr in en.EnMap.Attrs)
            {
                val = val.Replace("@" + attr.Key, en.GetValStringByKey(attr.Key));
            }
            return val;
        }
        public string MsgOKString
        {
            get
            {
                return this.GetValStringByKey(FrmEventAttr.MsgOK);
            }
            set
            {
                this.SetValByKey(FrmEventAttr.MsgOK, value);
            }
        }
        public string MsgErrorString
        {
            get
            {
                return this.GetValStringByKey(FrmEventAttr.MsgError);
            }
            set
            {
                this.SetValByKey(FrmEventAttr.MsgError, value);
            }
        }
        /// <summary>
        ///  Errors or anomalies tips 
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public string MsgError(Entity en)
        {
            string val = this.GetValStringByKey(FrmEventAttr.MsgError);
            if (val.Trim() == "")
                return null;

            if (val.IndexOf('@') == -1)
                return val;

            foreach (Attr attr in en.EnMap.Attrs)
            {
                val = val.Replace("@" + attr.Key, en.GetValStringByKey(attr.Key));
            }
            return val;
        }

        public string FK_Event
        {
            get
            {
                return this.GetValStringByKey(FrmEventAttr.FK_Event);
            }
            set
            {
                this.SetValByKey(FrmEventAttr.FK_Event, value);
            }
        }
        /// <summary>
        ///  Execution Type 
        /// </summary>
        public EventDoType HisDoType
        {
            get
            {
                return (EventDoType)this.GetValIntByKey(FrmEventAttr.DoType);
            }
            set
            {
                this.SetValByKey(FrmEventAttr.DoType, (int)value);
            }
        }
        #endregion

        #region  Event messages .
        /// <summary>
        ///  Message Control Type .
        /// </summary>
        public MsgCtrl MsgCtrl
        {
            get
            {
                return (MsgCtrl)this.GetValIntByKey(FrmEventAttr.MsgCtrl);
            }
            set
            {
                this.SetValByKey(FrmEventAttr.MsgCtrl, (int)value);
            }
        }
        public bool MobilePushEnable
        {
            get
            {
                return this.GetValBooleanByKey(FrmEventAttr.MobilePushEnable);
            }
            set
            {
                this.SetValByKey(FrmEventAttr.MobilePushEnable, value);
            }
        }
        /// <summary>
        ///  Whether sending mail enabled ?
        /// </summary>
        public bool MsgMailEnable
        {
            get
            {
                return this.GetValBooleanByKey(FrmEventAttr.MsgMailEnable);
            }
            set
            {
                this.SetValByKey(FrmEventAttr.MsgMailEnable, value);
            }
        }
        public string MailTitle
        {
            get
            {
                string str = this.GetValStrByKey(FrmEventAttr.MailTitle);
                if (string.IsNullOrEmpty(str) == false)
                    return str;
                switch (this.FK_Event)
                {
                    case EventListOfNode.SendSuccess:
                        return " New Work {{Title}}, Sender @WebUser.No,@WebUser.Name";
                    case EventListOfNode.ShitAfter:
                        return " Transfer to a new job {{Title}}, Handed people @WebUser.No,@WebUser.Name";
                    case EventListOfNode.ReturnAfter:
                        return " To be returned {{Title}}, Return man @WebUser.No,@WebUser.Name";
                    case EventListOfNode.UndoneAfter:
                        return " Work is revoked {{Title}}, Sender @WebUser.No,@WebUser.Name";
                    case EventListOfNode.AskerReAfter:
                        return " Canada signed a new job {{Title}}, Sender @WebUser.No,@WebUser.Name";
                        break;
                    default:
                        throw new Exception("@ This type of event does not define the default message template :" + this.FK_Event);
                        break;
                }
                return str;
            }
        }
        /// <summary>
        ///  Mail title 
        /// </summary>
        public string MailTitle_Real
        {
            get
            {
                string str = this.GetValStrByKey(FrmEventAttr.MailTitle);
                return str;
            }
            set
            {
                this.SetValByKey(FrmEventAttr.MailTitle, value);
            }
        }
        /// <summary>
        ///  Message content 
        /// </summary>
        public string MailDoc_Real
        {
            get
            {
                return this.GetValStrByKey(FrmEventAttr.MailDoc);
            }
            set
            {
                this.SetValByKey(FrmEventAttr.MailDoc, value);
            }
        }
        public string MailDoc
        {
            get
            {
                string str = this.GetValStrByKey(FrmEventAttr.MailDoc);
                if (string.IsNullOrEmpty(str) == false)
                    return str;
                switch (this.FK_Event)
                {
                    case EventListOfNode.SendSuccess:
                        str += "\t\n Hello :";
                        str += "\t\n     There are new job {{Title}} You need to deal with ,  Click here to open the work {Url} .";
                        str += "\t\nBR, ";
                        str += "\t\n    @WebUser.No, @WebUser.Name";
                        str += "\t\n    @RDT";
                        break;
                    case EventListOfNode.ReturnAfter:
                        str += "\t\n Hello :";
                        str += "\t\n     The work {{Title}} Be returned to the ,  Click here to open the work {Url} .";
                        str += "\t\n BR, ";
                        str += "\t\n    @WebUser.No,@WebUser.Name";
                        str += "\t\n    @RDT";
                        break;
                    case EventListOfNode.ShitAfter:
                        str += "\t\n Hello :";
                        str += "\t\n     Transferred to your work {{Title}},  Click here to open the work {Url} .";
                        str += "\t\n BR, ";
                        str += "\t\n    @WebUser.No,@WebUser.Name";
                        str += "\t\n    @RDT";
                        break;
                    case EventListOfNode.UndoneAfter:
                         str += "\t\n Hello :";
                         str += "\t\n     Transferred to your work {{Title}},  Click here to open the work {Url} .";
                        str += "\t\n BR, ";
                        str += "\t\n    @WebUser.No,@WebUser.Name";
                        str += "\t\n    @RDT";
                        break;
                    case EventListOfNode.AskerReAfter: // Plus sign .
                        str += "\t\n Hello :";
                        str += "\t\n     Transferred to your work {{Title}},  Click here to open the work {Url} .";
                        str += "\t\n BR, ";
                        str += "\t\n    @WebUser.No,@WebUser.Name";
                        str += "\t\n    @RDT";
                        break;
                    default:
                        throw new Exception("@ This type of event does not define the default message template :" + this.FK_Event);
                        break;
                }
                return str;
            }
        }
        /// <summary>
        ///  Whether SMS enabled 
        /// </summary>
        public bool SMSEnable
        {
            get
            {
                return this.GetValBooleanByKey(FrmEventAttr.SMSEnable);
            }
            set
            {
                this.SetValByKey(FrmEventAttr.SMSEnable, value);
            }
        }
        /// <summary>
        ///  SMS template content 
        /// </summary>
        public string SMSDoc_Real
        {
            get
            {
                string str = this.GetValStrByKey(FrmEventAttr.SMSDoc);
                return str;
            }
            set
            {
                this.SetValByKey(FrmEventAttr.SMSDoc, value);
            }
        }
        /// <summary>
        ///  SMS template content 
        /// </summary>
        public string SMSDoc
        {
            get
            {
                string str = this.GetValStrByKey(FrmEventAttr.SMSDoc);
                if (string.IsNullOrEmpty(str) == false)
                    return str;

                switch (this.FK_Event)
                {
                    case EventListOfNode.SendSuccess:
                        str = " There are new job {{Title}} You need to deal with ,  Sender :@WebUser.No, @WebUser.Name, Turn on {Url} .";
                        break;
                    case EventListOfNode.ReturnAfter:
                        str = " The work {{Title}} Be returned , Return man :@WebUser.No, @WebUser.Name, Turn on {Url} .";
                        break;
                    case EventListOfNode.ShitAfter:
                        str = " Handover {{Title}}, Handed people :@WebUser.No, @WebUser.Name, Turn on {Url} .";
                        break;
                    case EventListOfNode.UndoneAfter:
                        str = " Work revocation {{Title}}, Revocation people :@WebUser.No, @WebUser.Name, Turn on {Url}.";
                        break;
                    case EventListOfNode.AskerReAfter: // Plus sign .
                        str = " Work endorsement {{Title}}, Plus sign people :@WebUser.No, @WebUser.Name, Turn on {Url}.";
                        break;
                    default:
                        throw new Exception("@ This type of event does not define the default message template :" + this.FK_Event);
                        break;
                }
                return str;
            }
            set
            {
                this.SetValByKey(FrmEventAttr.SMSDoc, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Event 
        /// </summary>
        public FrmEvent()
        {
        }
        public FrmEvent(string mypk)
        {
            this.MyPK = mypk;
            this.RetrieveFromDBSources();
        }
        public FrmEvent(string fk_mapdata, string fk_Event)
        {
            this.FK_Event = fk_Event;
            this.FK_MapData = fk_mapdata;
            this.MyPK = this.FK_MapData + "_" + this.FK_Event;
            this.RetrieveFromDBSources();
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

                Map map = new Map("Sys_FrmEvent");
                map.EnDesc = " Event ";

                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.AddMyPK();

                map.AddTBString(FrmEventAttr.FK_Event, null, " Event Name ", true, true, 0, 400, 10);
                map.AddTBString(FrmEventAttr.FK_MapData, null, "FK_MapData", true, true, 0, 400, 10);

                map.AddTBInt(FrmEventAttr.DoType, 0, " Event Type ", true, true);
                map.AddTBString(FrmEventAttr.DoDoc, null, " Perform content ", true, true, 0, 400, 10);
                map.AddTBString(FrmEventAttr.MsgOK, null, " Successful implementation tips ", true, true, 0, 400, 10);
                map.AddTBString(FrmEventAttr.MsgError, null, " Exception message alert ", true, true, 0, 400, 10);

                #region  Message Settings .
                map.AddDDLSysEnum(FrmEventAttr.MsgCtrl, 0, " Messaging control ", true, true, FrmEventAttr.MsgCtrl,
                    "@0= Do not send @1= Automatic by sending the set range @2= The nodes form system by field (IsSendEmail,IsSendSMS) To decide @3=By SDK Developers parameters (IsSendEmail,IsSendSMS) To decide ", true);

                map.AddBoolean(FrmEventAttr.MsgMailEnable, true, " Whether sending mail enabled ?( If you must set up a mail-enabled template , Stand by ccflow Expression .)", true, true, true);
                map.AddTBString(FrmEventAttr.MailTitle, null, " Mail headline templates ", true, false, 0, 200, 20, true);
                map.AddTBStringDoc(FrmEventAttr.MailDoc, null, " Mail content templates ", true, false, true);

                // Whether to enable SMS ?
                map.AddBoolean(FrmEventAttr.SMSEnable, false, " Whether SMS enabled ?( If you enable necessary to set SMS templates , Stand by ccflow Expression .)", true, true, true);
                map.AddTBStringDoc(FrmEventAttr.SMSDoc, null, " SMS content templates ", true, false, true);

                map.AddBoolean(FrmEventAttr.MobilePushEnable, true, " If pushed to your phone ,pad.", true, true, true);

                #endregion  Message Settings .


                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        protected override bool beforeUpdateInsertAction()
        {
            this.MyPK = this.FK_MapData + "_" + this.FK_Event;
            return base.beforeUpdateInsertAction();
        }
    }
	/// <summary>
	///  Event 
	/// </summary>
    public class FrmEvents : EntitiesOID
    {
        /// <summary>
        ///  Execution events 
        /// </summary>
        /// <param name="dotype"> Execution Type </param>
        /// <param name="en"> Data entities </param>
        /// <returns>null  No event , The other for the execution of the event .</returns>
        public string DoEventNode(string dotype, Entity en)
        {
           return DoEventNode(dotype,en,null);
        }
        /// <summary>
        ///  Execution events 
        /// </summary>
        /// <param name="dotype"> Execution Type </param>
        /// <param name="en"> Data entities </param>
        /// <param name="atPara"> Parameters </param>
        /// <returns>null  No event , The other for the execution of the event .</returns>
        public string DoEventNode(string dotype, Entity en, string atPara)
        {
            if (this.Count == 0)
                return null;
            string val= _DoEventNode(dotype, en, atPara);
            if (val != null)
                val = val.Trim();

            if (string.IsNullOrEmpty(val))
                return ""; //  Explain an event , The successful implementation of the .
            else
                return val; //  No event . 
        }
        
        /// <summary>
        ///  Execution events , Mark is  EventList.
        /// </summary>
        /// <param name="dotype"> Execution Type </param>
        /// <param name="en"> Data entities </param>
        /// <param name="atPara"> Special parameter format @key=value  The way .</param>
        /// <returns></returns>
        private string _DoEventNode(string dotype, Entity en, string atPara)
        {
            if (this.Count == 0)
                return null;

            FrmEvent nev = this.GetEntityByKey(FrmEventAttr.FK_Event, dotype) as FrmEvent;
           
            if (nev == null || nev.HisDoType == EventDoType.Disable)
                return null;

            string doc = nev.DoDoc.Trim();
            if (doc == null || doc == "")
                return null;

            #region  Processing execution content 
            Attrs attrs = en.EnMap.Attrs;
            string MsgOK = "";
            string MsgErr = "";
            foreach (Attr attr in attrs)
            {
                if (doc.Contains("@" + attr.Key) == false)
                    continue;
                if (attr.MyDataType == DataType.AppString
                    || attr.MyDataType == DataType.AppDateTime
                    || attr.MyDataType == DataType.AppDate)
                    doc = doc.Replace("@" + attr.Key, "'" + en.GetValStrByKey(attr.Key) + "'");
                else
                    doc = doc.Replace("@" + attr.Key, en.GetValStrByKey(attr.Key));
            }

            doc = doc.Replace("~", "'");
            doc = doc.Replace("@WebUser.No", BP.Web.WebUser.No);
            doc = doc.Replace("@WebUser.Name", BP.Web.WebUser.Name);
            doc = doc.Replace("@WebUser.FK_Dept", BP.Web.WebUser.FK_Dept);
            doc = doc.Replace("@FK_Node", nev.FK_MapData.Replace("ND", ""));
            doc = doc.Replace("@FK_MapData", nev.FK_MapData);
            doc = doc.Replace("@WorkID", en.GetValStrByKey("OID","@WorkID"));

            if (System.Web.HttpContext.Current != null)
            {
                /* In the case of  bs  System ,  There may be arguments from url , To use url The parameters replace them  .*/
                string url = BP.Sys.Glo.Request.RawUrl;
                if (url.IndexOf('?') != -1)
                    url = url.Substring(url.IndexOf('?'));

                string[] paras = url.Split('&');
                foreach (string s in paras)
                {
                    if (doc.Contains("@" + s) == false)
                        continue;

                    string[] mys = s.Split('=');
                    if (doc.Contains("@" + mys[0]) == false)
                        continue;
                    doc = doc.Replace("@" + mys[0], mys[1]);
                }
            }

            if (nev.HisDoType == EventDoType.URLOfSelf)
            {
                if (doc.Contains("?") == false)
                    doc += "?1=2";

                doc += "&UserNo=" + WebUser.No;
                doc += "&SID=" + WebUser.SID;
                doc += "&FK_Dept=" + WebUser.FK_Dept;
                // doc += "&FK_Unit=" + WebUser.FK_Unit;
                doc += "&OID=" + en.PKVal;

                if (SystemConfig.IsBSsystem)
                {
                    /*是bs System , And is url Parameters execution type .*/
                    string url = BP.Sys.Glo.Request.RawUrl;
                    if (url.IndexOf('?') != -1)
                        url = url.Substring(url.IndexOf('?'));
                    string[] paras = url.Split('&');
                    foreach (string s in paras)
                    {
                        if (doc.Contains(s))
                            continue;
                        doc += "&" + s;
                    }
                    doc = doc.Replace("&?", "&");
                }

                if (SystemConfig.IsBSsystem == false)
                {
                    /*非bs Call mode , For example, in cs Call it mode , It takes less than a parameter . */
                }

                if (doc.StartsWith("http") == false)
                {
                    /* If there is no absolute path  */
                    if (SystemConfig.IsBSsystem)
                    {
                        /*在cs Automatic acquisition mode */
                        string host = BP.Sys.Glo.Request.Url.Host;
                        if (doc.Contains("@AppPath"))
                            doc = doc.Replace("@AppPath", "http://" + host + BP.Sys.Glo.Request.ApplicationPath);
                        else
                            doc = "http://" + BP.Sys.Glo.Request.Url.Authority + doc;
                    }

                    if (SystemConfig.IsBSsystem == false)
                    {
                        /*在cs Its mode baseurl 从web.config Get .*/
                        string cfgBaseUrl = SystemConfig.AppSettings["BaseUrl"];
                        if (string.IsNullOrEmpty(cfgBaseUrl))
                        {
                            string err = " Calling url Failure : Not web.config Configure BaseUrl, Resulting in url Event can not be executed .";
                            Log.DefaultLogWriteLineError(err);
                            throw new Exception(err);
                        }
                        doc = cfgBaseUrl + doc;
                    }
                }

                // Increase agreed on the system parameters .
                doc += "&EntityName=" + en.ToString() + "&EntityPK=" + en.PK + "&EntityPKVal=" + en.PKVal + "&FK_Event=" + nev.MyPK;
            }
            #endregion  Processing execution content 

            if (atPara != null && doc.Contains("@")==true)
            {
                AtPara ap = new AtPara(atPara);
                foreach (string s in ap.HisHT.Keys)
                    doc = doc.Replace("@" + s, ap.GetValStrByKey(s));
            }

            if (dotype == FrmEventList.FrmLoadBefore)
                en.Retrieve(); /* If you do not execute , Will result in the entity data and query data inconsistencies .*/

            switch (nev.HisDoType)
            {
                //case EventDoType.SP:
                //    try
                //    {
                //        Paras ps = new Paras();
                //        DBAccess.RunSP(doc, ps);
                //        return nev.MsgOK(en);
                //    }
                //    catch (Exception ex)
                //    {
                //        throw new Exception(nev.MsgError(en) + " Error:" + ex.Message);
                //    }
                //    break;
                case EventDoType.SP:
                     try
                    {
                        //  Allowed to perform with GO的sql.
                        //DBAccessOfOracle.RunSP();
                        string produce = "";
                        Paras paras = new Paras();
                        DBAccessOfOracle.parseSP(doc, out produce, out paras);
                        using (OracleConnection conn = new OracleConnection(SystemConfig.AppCenterDSN))
                        {
                            DBAccessOfOracle.RunSP(produce, paras, conn);
                        }
                         
                        return nev.MsgOK(en);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(nev.MsgError(en) + " Error:" + ex.Message);
                    }
                    break;
                case EventDoType.SQL:
                    try
                    {
                        //  Allowed to perform with GO的sql.
                        DBAccess.RunSQLs(doc);
                        return nev.MsgOK(en);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(nev.MsgError(en) + " Error:" + ex.Message);
                    }
                    break;
                case EventDoType.URLOfSelf:
                    string myURL = doc.Clone() as string;
                    if (myURL.Contains("http") == false)
                    {
                        if (SystemConfig.IsBSsystem)
                        {
                            string host = BP.Sys.Glo.Request.Url.Host;
                            if (myURL.Contains("@AppPath"))
                                myURL = myURL.Replace("@AppPath", "http://" + host + BP.Sys.Glo.Request.ApplicationPath);
                            else
                                myURL = "http://" + BP.Sys.Glo.Request.Url.Authority + myURL;
                        }
                        else
                        {
                            string cfgBaseUrl = SystemConfig.AppSettings["BaseUrl"];
                            if (string.IsNullOrEmpty(cfgBaseUrl))
                            {
                                string err = " Calling url Failure : Not web.config Configure BaseUrl, Resulting in url Event can not be executed .";
                                Log.DefaultLogWriteLineError(err);
                                throw new Exception(err);
                            }
                            myURL = cfgBaseUrl + myURL;
                        }
                    }

                    try
                    {
                        Encoding encode = System.Text.Encoding.GetEncoding("gb2312");
                        string text = DataType.ReadURLContext(myURL, 600000, encode);
                        if (text == null)
                            throw new Exception("@ Process design errors , Execution url Error :" + myURL + ",  Returns null,  Please check url The settings are correct . Prompt : You can copy Out of this url Into your browser to see if executed correctly .");

                        if (text != null
                            && text.Length > 7
                            && text.Substring(0, 7).ToLower().Contains("err"))
                            throw new Exception(text);

                        if (text == null || text.Trim() == "")
                            return null;
                        return text;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("@" + nev.MsgError(en) + " Error:" + ex.Message);
                    }
                    break;
                //case EventDoType.URLOfSystem:
                //    string hos1t = BP.Sys.Glo.Request.Url.Host;
                //    string url = "http://" + hos1t + BP.Sys.Glo.Request.ApplicationPath + "/DataUser/AppCoder/FrmEventHandle.aspx";
                //    url += "?FK_MapData=" + en.ClassID + "&WebUseNo=" + WebUser.No + "&EventType=" + nev.FK_Event;
                //    foreach (Attr attr in attrs)
                //    {
                //        if (attr.UIIsDoc || attr.IsRefAttr || attr.UIIsReadonly)
                //            continue;
                //        url += "&" + attr.Key + "=" + en.GetValStrByKey(attr.Key);
                //    }

                //    try
                //    {
                //        string text = DataType.ReadURLContext(url, 800, System.Text.Encoding.UTF8);
                //        if (text != null && text.Substring(0, 7).Contains("Err"))
                //            throw new Exception(text);

                //        if (text == null || text.Trim() == "")
                //            return null; //  In the case of Null  No event configuration .
                //        return text;
                //    }
                //    catch (Exception ex)
                //    {
                //        throw new Exception("@" + nev.MsgError(en) + " Error:" + ex.Message);
                //    }
                //    break;
                case EventDoType.EventBase: // Execution event classes .

                    //  Get event classes .
                    string evName = doc.Clone() as string;
                    BP.Sys.EventBase ev = null;
                    try
                    {
                        ev = BP.En.ClassFactory.GetEventBase(evName);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("@ Event Name :" + evName + " Spelling mistake , Or the system does not exist . Explanation : Library must be based on where the event BP. Beginning , And libraries BP.xxx.dll.");
                    }

                    // Started .
                    try
                    {
                        #region  Finishing processing parameters .
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

                        try
                        {
                            r.Add("EventType", nev.FK_Event);
                        }
                        catch
                        {
                            r["EventType"] = nev.FK_Event;
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

                        if (SystemConfig.IsBSsystem == true)
                        {
                            /* In the case of bs System ,  Join the external url Variables .*/
                            foreach (string key in BP.Sys.Glo.Request.QueryString)
                            {
                                string val = BP.Sys.Glo.Request.QueryString[key];
                                try
                                {
                                    r.Add(key, val);
                                }
                                catch
                                {
                                    r[key] = val;
                                }
                            }
                        }
                        #endregion  Finishing processing parameters .

                        ev.SysPara = r;
                        ev.HisEn = en;
                        ev.Do();
                        return ev.SucessInfo;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("@ Execution events (" + ev.Title + ") An error occurred during :" + ex.Message);
                    }
                    break;
                default:
                    throw new Exception("@no such way." + nev.HisDoType.ToString());
            }
        }
        /// <summary>
        ///  Event 
        /// </summary>
        public FrmEvents() { }
        /// <summary>
        ///  Event 
        /// </summary>
        /// <param name="FK_MapData">FK_MapData</param>
        public FrmEvents(string fk_MapData)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(FrmEventAttr.FK_MapData, fk_MapData);
            qo.DoQuery();
        }
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new FrmEvent();
            }
        }
    }
}
