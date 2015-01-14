using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Web;
using BP.Sys;
using BP.WF.Port;
using System.Net.Mail;

namespace BP.WF
{
    /// <summary>
    ///  Message Type 
    /// </summary>
    public class SMSMsgType
    {
        /// <summary>
        ///  Custom message 
        /// </summary>
        public const string Self = "Self";
        /// <summary>
        ///  Send a copy of the message 
        /// </summary>
        public const string CC = "CC";
        /// <summary>
        ///  Upcoming news 
        /// </summary>
        public const string ToDo = "ToDo";
        /// <summary>
        ///  Other 
        /// </summary>
        public const string Etc = "Etc";
    }
	/// <summary>
	///  Message Status 
	/// </summary>
    public enum MsgSta
    {
        /// <summary>
        ///  Not Started 
        /// </summary>
        UnRun,
        /// <summary>
        ///  Success 
        /// </summary>
        RunOK,
        /// <summary>
        ///  Failure 
        /// </summary>
        RunError,
        /// <summary>
        ///  Prohibit send 
        /// </summary>
        Disable
    }
	/// <summary>
	///  Message Properties 
	/// </summary>
	public class SMSAttr:EntityMyPKAttr
	{
        /// <summary>
        ///  Message mark £¨ There are not sending this tag £©
        /// </summary>
        public const string MsgFlag = "MsgFlag";
		/// <summary>
		///  Status  0  Unsent , 1  Sent successfully ,2 Failed to send .
		/// </summary>
		public const string EmaiSta="EmaiSta";
        /// <summary>
        ///  Mail 
        /// </summary>
        public const string Email = "Email";
        /// <summary>
        ///  Mail title 
        /// </summary>
        public const string EmailTitle = "EmailTitle";
        /// <summary>
        ///  Message content 
        /// </summary>
        public const string EmailDoc = "EmailDoc";
        /// <summary>
        ///  Sender 
        /// </summary>
        public const string Sender = "Sender";
        /// <summary>
        ///  Send to 
        /// </summary>
        public const string SendTo = "SendTo";
        /// <summary>
        ///  Insert Date 
        /// </summary>
        public const string RDT = "RDT";
        /// <summary>
        ///  Send date 
        /// </summary>
        public const string SendDT = "SendDT";
        /// <summary>
        ///  Whether read 
        /// </summary>
        public const string IsRead = "IsRead";
        /// <summary>
        ///  Status  0  Unsent , 1  Sent successfully ,2 Failed to send .
        /// </summary>
        public const string MobileSta = "MobileSta";
        /// <summary>
        ///  Cell phone 
        /// </summary>
        public const string Mobile = "Mobile";
        /// <summary>
        ///  Phone Info 
        /// </summary>
        public const string MobileInfo = "MobileInfo";
        /// <summary>
        ///  Whether tips over 
        /// </summary>
        public const string IsAlert = "IsAlert";
        /// <summary>
        ///  Message Type 
        /// </summary>
        public const string MsgType = "MsgType";
	}
	/// <summary>
	///  News 
	/// </summary> 
    public class SMS : EntityMyPK
    {
        #region  New Methods  2013 
        /// <summary>
        ///  Send phone messages 
        /// </summary>
        /// <param name="Mobile"> Phone number </param>
        /// <param name="doc"> Mobile content. </param>
        public static void SendSMS_del(string Mobile, string doc)
        {
            //  If you do not enable the message mechanism .
            if (BP.WF.Glo.IsEnableSysMessage == false)
                return;

            SMS sms = new SMS();
            sms.MyPK = DBAccess.GenerGUID();
            sms.HisEmaiSta = MsgSta.UnRun;
            sms.Email = Mobile;
            sms.Title = doc;
            sms.Sender = BP.Web.WebUser.No;
            sms.RDT = BP.DA.DataType.CurrentDataTime;
            try
            {
                sms.Insert();
            }
            catch
            {
                sms.CheckPhysicsTable();
                sms.Insert();
            }
        }
        /// <summary>
        ///  Send a message 
        /// </summary>
        /// <param name="userNo"> Recipient </param>
        /// <param name="msgTitle"> Title </param>
        /// <param name="msgDoc"> Content </param>
        /// <param name="msgFlag"> Mark </param>
        /// <param name="msgType"> Type </param>
        public static void SendMsg(string userNo, string msgTitle, string msgDoc, string msgFlag, string msgType)
        {
           
            SMS sms = new SMS();
            sms.MyPK = DBAccess.GenerGUID();
            sms.HisEmaiSta = MsgSta.UnRun;

            sms.Sender=WebUser.No;
            sms.SendTo = userNo;

            sms.Title = msgTitle;
            sms.DocOfEmail = msgDoc;

            sms.Sender = BP.Web.WebUser.No;
            sms.RDT = BP.DA.DataType.CurrentDataTime;
            
            sms.MsgFlag = msgFlag; //  Message Flags .
            sms.MsgType = msgType; //  Message Type .
            sms.Insert();
        }
        /// <summary>
        ///  Send a message 
        /// </summary>
        /// <param name="mobileNum"> The phone number you </param>
        /// <param name="mobileInfo"> SMS information </param>
        /// <param name="email"> Mail </param>
        /// <param name="title"> Title </param>
        /// <param name="infoBody"> Message content </param>
        /// <param name="msgFlag"> Message mark , Can be empty .</param>
        /// <param name="guestNo"> User ID </param>
        public static void SendMsg(string mobileNum, string mobileInfo, string email, string title, string
            infoBody, string msgFlag,string msgType,string guestNo)
        {
            

            SMS sms = new SMS();
            sms.Sender = WebUser.No;
            sms.RDT = BP.DA.DataType.CurrentDataTimess;
            sms.SendTo = guestNo;


            //  Mail information 
            sms.HisEmaiSta = MsgSta.UnRun;
            sms.Title = title;
            sms.DocOfEmail = infoBody;

            // Phone Info .
            sms.Mobile = mobileNum;
            sms.HisMobileSta = MsgSta.UnRun;
            sms.MobileInfo = mobileInfo;
            sms.MsgFlag = msgFlag; //  Message Flags .

            if (string.IsNullOrEmpty(msgFlag))
            {
                sms.MyPK = DBAccess.GenerGUID();
                sms.Insert();
            }
            else
            {
                //  If you already have the PK, Not allow the insertion of the .
                try
                {
                    sms.MyPK = msgFlag;
                    sms.Insert();
                }
                catch
                {
                }
            }
        }
        #endregion  New Methods 

        #region  SMS Properties 
        /// <summary>
        ///  Mobile number 
        /// </summary>
        public string Mobile
        {
            get
            {
                return this.GetValStringByKey(SMSAttr.Mobile);
            }
            set
            {
                SetValByKey(SMSAttr.Mobile, value);
            }
        }
        /// <summary>
        ///  Phone status 
        /// </summary>
        public MsgSta HisMobileSta
        {
            get
            {
                return (MsgSta)this.GetValIntByKey(SMSAttr.MobileSta);
            } 
            set
            {
                SetValByKey(SMSAttr.MobileSta, (int)value);
            }
        }
        /// <summary>
        ///  Phone Info 
        /// </summary>
        public string MobileInfo
        {
            get
            {
                return this.GetValStringByKey(SMSAttr.MobileInfo);
            }
            set
            {
                SetValByKey(SMSAttr.MobileInfo, value);
            }
        }
        #endregion

        #region   Message Properties 
        /// <summary>
        ///  Message status 
        /// </summary>
        public MsgSta HisEmaiSta
        {
            get
            {
                return (MsgSta)this.GetValIntByKey(SMSAttr.EmaiSta);
            }
            set
            {
                this.SetValByKey(SMSAttr.EmaiSta, (int)value);
            }
        }
        /// <summary>
        /// Email
        /// </summary>
        public string Email
        {
            get
            {
                return this.GetValStringByKey(SMSAttr.Email);
            }
            set
            {
                SetValByKey(SMSAttr.Email, value);
            }
        }
        /// <summary>
        ///  Send to 
        /// </summary>
        public string SendToEmpNo
        {
            get
            {
                return this.GetValStringByKey(SMSAttr.SendTo);
            }
            set
            {
                SetValByKey(SMSAttr.SendTo, value);
            }
        }
        /// <summary>
        ///  Send to  
        /// </summary>
        public string SendTo
        {
            get
            {
                return this.GetValStringByKey(SMSAttr.SendTo);
            }
            set
            {
                SetValByKey(SMSAttr.SendTo, value);
            }
        }
        /// <summary>
        ///  Message mark ( You can use it to avoid sending duplicate )
        /// </summary>
        public string MsgFlag
        {
            get
            {
                return this.GetValStringByKey(SMSAttr.MsgFlag);
            }
            set
            {
                SetValByKey(SMSAttr.MsgFlag, value);
            }
        }
        /// <summary>
        ///  Type 
        /// </summary>
        public string MsgType
        {
            get
            {
                return this.GetValStringByKey(SMSAttr.MsgType);
            }
            set
            {
                SetValByKey(SMSAttr.MsgType, value);
            }
        }
        /// <summary>
        ///  Sender 
        /// </summary>
        public string Sender
        {
            get
            {
                return this.GetValStringByKey(SMSAttr.Sender);
            }
            set
            {
                SetValByKey(SMSAttr.Sender, value);
            }
        }
        /// <summary>
        ///  Record Date 
        /// </summary>
        public string RDT
        {
            get
            {
                return this.GetValStringByKey(SMSAttr.RDT);
            }
            set
            {
                SetValByKey(SMSAttr.RDT, value);
            }
        }
        /// <summary>
        ///  Send date 
        /// </summary>
        public string SendDT
        {
            get
            {
                return this.GetValStringByKey(SMSAttr.SendDT);
            }
            set
            {
                SetValByKey(SMSAttr.SendDT, value);
            }
        }
        /// <summary>
        ///  Title 
        /// </summary>
        public string Title
        {
            get
            {
                return this.GetValStringByKey(SMSAttr.EmailTitle);
            }
            set
            {
                SetValByKey(SMSAttr.EmailTitle, value);
            }
        }
        /// <summary>
        ///  Message content 
        /// </summary>
        public string DocOfEmail
        {
            get
            {
                string doc = this.GetValStringByKey(SMSAttr.EmailDoc);
                if (string.IsNullOrEmpty(doc))
                    return this.Title;
                return doc.Replace('~', '\'');
            }
            set
            {
                SetValByKey(SMSAttr.EmailDoc, value);
            }
        }
        public string Doc
        {
            get
            {
                return this.DocOfEmail;
            }
            set
            {
                SetValByKey(SMSAttr.EmailDoc, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        /// UI Access control interface 
        /// </summary>
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                uac.OpenAll();
                return uac;
            }
        }
        /// <summary>
        ///  News 
        /// </summary>
        public SMS()
        {
        }
        /// <summary>
        /// Map
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("Sys_SMS");
                map.EnDesc = " News ";

                map.AddMyPK();

                map.AddTBString(SMSAttr.Sender, null, " Sender ( Can be empty )", false, true, 0, 200, 20);
                map.AddTBString(SMSAttr.SendTo, null, " Send to ( Can be empty )", false, true, 0, 200, 20);
                map.AddTBDateTime(SMSAttr.RDT, " Write time ", true, false);

                map.AddTBString(SMSAttr.Mobile, null, " Phone number ( Can be empty )", false, true, 0, 30, 20);
                map.AddTBInt(SMSAttr.MobileSta, (int)MsgSta.UnRun, " Message Status ", true, true);
                map.AddTBString(SMSAttr.MobileInfo, null, " SMS information ", false, true, 0, 1000, 20);

                map.AddTBString(SMSAttr.Email, null, "Email( Can be empty )", false, true, 0, 200, 20);
                map.AddTBInt(SMSAttr.EmaiSta, (int)MsgSta.UnRun, "EmaiSta Message Status ", true, true);
                map.AddTBString(SMSAttr.EmailTitle, null, " Title ", false, true, 0, 3000, 20);
                map.AddTBStringDoc(SMSAttr.EmailDoc, null, " Content ", false, true);
                map.AddTBDateTime(SMSAttr.SendDT,null, " Transmission time ", false, false);

                map.AddTBInt(SMSAttr.IsRead, 0, " Whether read ?", true, true);
                map.AddTBInt(SMSAttr.IsAlert, 0, " Are Tips ?", true, true);

                map.AddTBString(SMSAttr.MsgFlag, null, " Message mark ( To prevent sending duplicate )", false, true, 0, 200, 20);
                map.AddTBString(SMSAttr.MsgType, null, " Message Type (CC Cc ,ToDo Upcoming )", false, true, 0, 200, 20);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
        /// <summary>
        ///  Send e-mail 
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="mailTitle"></param>
        /// <param name="mailDoc"></param>
        /// <returns></returns>
        public static bool SendEmailNow(string mail, string mailTitle, string mailDoc)
        {
            System.Net.Mail.MailMessage myEmail = new System.Net.Mail.MailMessage();
            myEmail.From = new System.Net.Mail.MailAddress("ccflow.cn@gmail.com", "ccflow", System.Text.Encoding.UTF8);

            myEmail.To.Add(mail);
            myEmail.Subject = mailTitle;
            myEmail.SubjectEncoding = System.Text.Encoding.UTF8;// Mail header encoding 

            myEmail.Body = mailDoc;
            myEmail.BodyEncoding = System.Text.Encoding.UTF8;// Mail content encoding 
            myEmail.IsBodyHtml = true;// Is HTML Mail 

            myEmail.Priority = MailPriority.High;// Priority Mail 

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(SystemConfig.GetValByKey("SendEmailAddress", "ccflow.cn@gmail.com"),
                SystemConfig.GetValByKey("SendEmailPass", "ccflow123"));
            // Said write your email and password 
            client.Port = SystemConfig.GetValByKeyInt("SendEmailPort", 587); // Ports Used 
            client.Host = SystemConfig.GetValByKey("SendEmailHost", "smtp.gmail.com");

            //  After ssl Encryption . 
            if (SystemConfig.GetValByKeyInt("SendEmailEnableSsl", 1) == 1)
                client.EnableSsl = true;  // After ssl Encryption .
            else
                client.EnableSsl = false; // After ssl Encryption .


            try
            {
                object userState = myEmail;
                client.SendAsync(myEmail, userState);
                return true;
            }
            catch
            {
                return false;
            }
        }
         
    }
	/// <summary>
	///  News s
	/// </summary> 
    public class SMSs : Entities
    {
        public override Entity GetNewEntity
        {
            get
            {
                return new SMS();
            }
        }
        public SMSs()
        {
        }
    }
}
 