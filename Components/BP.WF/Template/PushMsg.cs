using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.Port;
using BP.Sys;

namespace BP.WF
{
    /// <summary>
    ///  Push manner specified 
    /// </summary>
    public enum PushWay
    {
        /// <summary>
        ///  Staff specified node 
        /// </summary>
        NodeWorker,
        /// <summary>
        ///  Staff performed s
        /// </summary>
        SpecEmps,
        /// <summary>
        ///  Specify jobs s
        /// </summary>
        SpecStations,
        /// <summary>
        ///  Designated departments 
        /// </summary>
        SpecDepts,
        /// <summary>
        ///  Specified SQL
        /// </summary>
        SpecSQL,
        /// <summary>
        ///  The system specified field 
        /// </summary>
        ByParas
    }
	/// <summary>
	///  Message push property 
	/// </summary>
    public class PushMsgAttr
    {
        /// <summary>
        ///  Node 
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        ///  Event 
        /// </summary>
        public const string FK_Event = "FK_Event";
        /// <summary>
        ///  Push mode 
        /// </summary>
        public const string PushWay = "PushWay";
        /// <summary>
        ///  Push processing content 
        /// </summary>
        public const string PushDoc = "PushDoc";
        /// <summary>
        ///  Push processing content  tag.
        /// </summary>
        public const string Tag = "Tag";

        #region  Message Settings .
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
	///  Push messages 
	/// </summary>
    public class PushMsg : EntityMyPK
    {
        #region  Basic properties 
        /// <summary>
        ///  Event 
        /// </summary>
        public string FK_Event
        {
            get
            {
                return this.GetValStringByKey(PushMsgAttr.FK_Event);
            }
            set
            {
                this.SetValByKey(PushMsgAttr.FK_Event, value);
            }
        }
        public int PushWay
        {
            get
            {
                return this.GetValIntByKey(PushMsgAttr.PushWay);
            }
            set
            {
                this.SetValByKey(PushMsgAttr.PushWay, value);
            }
        }
        /// <summary>
        /// Node 
        /// </summary>
        public int FK_Node
        {
            get
            {
                return this.GetValIntByKey(PushMsgAttr.FK_Node);
            }
            set
            {
                this.SetValByKey(PushMsgAttr.FK_Node, value);
            }
        }
        public string PushDoc
        {
            get
            {
                string s = this.GetValStringByKey(PushMsgAttr.PushDoc);
                if (string.IsNullOrEmpty(s) == true)
                    s = "";
                return s;
            }
            set
            {
                this.SetValByKey(PushMsgAttr.PushDoc, value);
            }
        }
        public string Tag
        {
            get
            {
                string s = this.GetValStringByKey(PushMsgAttr.Tag);
                if (string.IsNullOrEmpty(s) == true)
                    s = "";
                return s;
            }
            set
            {
                this.SetValByKey(PushMsgAttr.Tag, value);
            }
        }
        #endregion

        #region  Event messages .
        public bool MobilePushEnable
        {
            get
            {
                return this.GetValBooleanByKey(PushMsgAttr.MobilePushEnable);
            }
            set
            {
                this.SetValByKey(PushMsgAttr.MobilePushEnable, value);
            }
        }
        /// <summary>
        ///  Whether sending mail enabled ?
        /// </summary>
        public bool MsgMailEnable
        {
            get
            {
                return this.GetValBooleanByKey(PushMsgAttr.MsgMailEnable);
            }
            set
            {
                this.SetValByKey(PushMsgAttr.MsgMailEnable, value);
            }
        }
        public string MailTitle
        {
            get
            {
                string str = this.GetValStrByKey(PushMsgAttr.MailTitle);
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
                string str = this.GetValStrByKey(PushMsgAttr.MailTitle);
                return str;
            }
            set
            {
                this.SetValByKey(PushMsgAttr.MailTitle, value);
            }
        }
        /// <summary>
        ///  Message content 
        /// </summary>
        public string MailDoc_Real
        {
            get
            {
                return this.GetValStrByKey(PushMsgAttr.MailDoc);
            }
            set
            {
                this.SetValByKey(PushMsgAttr.MailDoc, value);
            }
        }
        public string MailDoc
        {
            get
            {
                string str = this.GetValStrByKey(PushMsgAttr.MailDoc);
                if (string.IsNullOrEmpty(str) == false)
                    return str;
                switch (this.FK_Event)
                {
                    case EventListOfNode.SendSuccess:
                        str += "\t\n Hello :";
                        str += "\t\n     There are new job {{Title}} You need to deal with ,  Click here to open the work {Url} .";
                        str += "\t\nÖÂ! ";
                        str += "\t\n    @WebUser.No, @WebUser.Name";
                        str += "\t\n    @RDT";
                        break;
                    case EventListOfNode.ReturnAfter:
                        str += "\t\n Hello :";
                        str += "\t\n     The work {{Title}} Be returned to the ,  Click here to open the work {Url} .";
                        str += "\t\n ÖÂ! ";
                        str += "\t\n    @WebUser.No,@WebUser.Name";
                        str += "\t\n    @RDT";
                        break;
                    case EventListOfNode.ShitAfter:
                        str += "\t\n Hello :";
                        str += "\t\n     Transferred to your work {{Title}},  Click here to open the work {Url} .";
                        str += "\t\n ÖÂ! ";
                        str += "\t\n    @WebUser.No,@WebUser.Name";
                        str += "\t\n    @RDT";
                        break;
                    case EventListOfNode.UndoneAfter:
                        str += "\t\n Hello :";
                        str += "\t\n     Transferred to your work {{Title}},  Click here to open the work {Url} .";
                        str += "\t\n ÖÂ! ";
                        str += "\t\n    @WebUser.No,@WebUser.Name";
                        str += "\t\n    @RDT";
                        break;
                    case EventListOfNode.AskerReAfter: // Plus sign .
                        str += "\t\n Hello :";
                        str += "\t\n     Transferred to your work {{Title}},  Click here to open the work {Url} .";
                        str += "\t\n ÖÂ! ";
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
                return this.GetValBooleanByKey(PushMsgAttr.SMSEnable);
            }
            set
            {
                this.SetValByKey(PushMsgAttr.SMSEnable, value);
            }
        }
        /// <summary>
        ///  SMS template content 
        /// </summary>
        public string SMSDoc_Real
        {
            get
            {
                string str = this.GetValStrByKey(PushMsgAttr.SMSDoc);
                return str;
            }
            set
            {
                this.SetValByKey(PushMsgAttr.SMSDoc, value);
            }
        }
        /// <summary>
        ///  SMS template content 
        /// </summary>
        public string SMSDoc
        {
            get
            {
                string str = this.GetValStrByKey(PushMsgAttr.SMSDoc);
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
                this.SetValByKey(PushMsgAttr.SMSDoc, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Push messages 
        /// </summary>
        public PushMsg()
        {

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

                Map map = new Map("WF_PushMsg");
                map.EnDesc = " Push messages ";

                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;

                map.AddMyPK();

                map.AddTBInt(PushMsgAttr.FK_Node, 0, " Node ", true, false);
                map.AddTBString(PushMsgAttr.FK_Event, null, "FK_Event", true, false, 0, 15, 10);
                //map.AddTBInt(PushMsgAttr.PushWay, 0, " Push mode ", true, false);
                map.AddDDLSysEnum(PushMsgAttr.PushWay, 0, " Push mode ", true, false, PushMsgAttr.PushWay, "@0= According to the staff of the specified node @1= According to designated staff @2= According to the specified jobs @3= According to the specified department @4= As specified SQL@5= The system specified field ");

                // Set content .
                map.AddTBString(PushMsgAttr.PushDoc, null, " Push save content ", true, false, 0, 3500, 10);
                map.AddTBString(PushMsgAttr.Tag, null, "Tag", true, false, 0, 500, 10);


                #region  Message Settings .
                map.AddBoolean(FrmEventAttr.MsgMailEnable, true, " Whether sending mail enabled ?( If you must set up a mail-enabled template , Stand by ccflow Expression .)", true, true, true);
                map.AddTBString(FrmEventAttr.MailTitle, null, " Mail headline templates ", true, false, 0, 200, 20, true);
                map.AddTBStringDoc(FrmEventAttr.MailDoc, null, " Mail content templates ", true, false, true);

                // Whether to enable SMS ?
                map.AddBoolean(FrmEventAttr.SMSEnable, false, " Whether SMS enabled ?( If you enable necessary to set SMS templates , Stand by ccflow Expression .)", true, true, true);
                map.AddTBStringDoc(FrmEventAttr.SMSDoc, null, " SMS content templates ", true, false, true);
                map.AddBoolean(FrmEventAttr.MobilePushEnable, true, " If pushed to your phone ,pad¶Ë.", true, true, true);
                #endregion  Message Settings .

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        protected override bool beforeUpdateInsertAction()
        {
             this.MyPK = this.FK_Event + "_" + this.FK_Node + "_" + this.PushWay;
            return base.beforeUpdateInsertAction();
        }
    }
	/// <summary>
	///  Push messages 
	/// </summary>
    public class PushMsgs : EntitiesMyPK
    {
        /// <summary>
        ///  Push messages 
        /// </summary>
        public PushMsgs() { }
        /// <summary>
        ///  Push messages 
        /// </summary>
        /// <param name="fk_flow"></param>
        public PushMsgs(string fk_flow)
        {
            

            QueryObject qo = new QueryObject(this);
            qo.AddWhereInSQL(PushMsgAttr.FK_Node, "SELECT NodeID FROM WF_Node WHERE FK_Flow='" + fk_flow + "'");
            qo.DoQuery();
        }
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new PushMsg();
            }
        }
    }
}
