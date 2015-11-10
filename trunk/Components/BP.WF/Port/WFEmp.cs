using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.WF;
using BP.Port; 
using BP.Port; 
using BP.En;
using BP.Web;
using System.Drawing;
using System.Text;
using System.IO;

namespace BP.WF.Port
{
    /// <summary>
    ///  Authorization 
    /// </summary>
    public enum AuthorWay
    {
        /// <summary>
        ///  Not authorized 
        /// </summary>
        None,
        /// <summary>
        ///  All authorized 
        /// </summary>
        All,
        /// <summary>
        ///  Specifies the authorization process 
        /// </summary>
        SpecFlows
    }
    public enum AlertWay
    {
        /// <summary>
        ///  Do not prompt 
        /// </summary>
        None,
        /// <summary>
        ///  SMS 
        /// </summary>
        SMS,
        /// <summary>
        ///  Mail 
        /// </summary>
        Email,
        /// <summary>
        ///  SMS + Mail 
        /// </summary>
        SMSAndEmail,
        /// <summary>
        ///  Internal message 
        /// </summary>
        AppSystemMsg
    }
	/// <summary>
	///  The operator 
	/// </summary>
    public class WFEmpAttr
    {
        #region  Basic properties 
        /// <summary>
        /// No
        /// </summary>
        public const string No = "No";
        /// <summary>
        ///  Applicant 
        /// </summary>
        public const string Name = "Name";
        public const string LoginData = "LoginData";
        public const string Tel = "Tel";
        /// <summary>
        ///  Authorized person 
        /// </summary>
        public const string Author = "Author";
        /// <summary>
        ///  Authorization Date 
        /// </summary>
        public const string AuthorDate = "AuthorDate";
        /// <summary>
        ///  Whether it is authorized by state 
        /// </summary>
        public const string AuthorWay = "AuthorWay";
        /// <summary>
        ///  Authorize automatic recovery date 
        /// </summary>
        public const string AuthorToDate = "AuthorToDate";
        public const string Email = "Email";
        public const string AlertWay = "AlertWay";
        public const string Stas = "Stas";
        public const string FK_Dept = "FK_Dept";
        public const string Idx = "Idx";
        public const string FtpUrl = "FtpUrl";
        public const string Style = "Style";
        public const string Msg = "Msg";
        public const string TM = "TM";
        public const string UseSta = "UseSta";
        /// <summary>
        ///  Authorized personnel 
        /// </summary>
        public const string AuthorFlows = "AuthorFlows";
        #endregion
    }
	/// <summary>
	///  The operator 
	/// </summary>
    public class WFEmp : EntityNoName
    {
        #region  Basic properties 
        public string HisAlertWayT
        {
            get
            {
                return this.GetValRefTextByKey(WFEmpAttr.AlertWay);
            }
        }
        public AlertWay HisAlertWay
        {
            get
            {
                return (AlertWay)this.GetValIntByKey(WFEmpAttr.AlertWay);
            }
            set
            {
                SetValByKey(WFEmpAttr.AlertWay, (int)value);
            }
        }
        /// <summary>
        ///  User Status 
        /// </summary>
        public int UseSta
        {
            get
            {
                return this.GetValIntByKey(WFEmpAttr.UseSta);
            }
            set
            {
                SetValByKey(WFEmpAttr.UseSta, value);
            }
        }
        public string FK_Dept
        {
            get
            {
                return this.GetValStringByKey(WFEmpAttr.FK_Dept);
            }
            set
            {
                SetValByKey(WFEmpAttr.FK_Dept, value);
            }
        }
        public string Style
        {
            get
            {
                return this.GetValStringByKey(WFEmpAttr.Style);
            }
            set
            {
                this.SetValByKey(WFEmpAttr.Style, value);
            }
        }
        public string TM
        {
            get
            {
                return this.GetValStringByKey(WFEmpAttr.TM);
            }
            set
            {
                this.SetValByKey(WFEmpAttr.TM, value);
            }
        }
        public string Tel
        {
            get
            {
                return this.GetValStringByKey(WFEmpAttr.Tel);
            }
            set
            {
                SetValByKey(WFEmpAttr.Tel, value);
            }
        }
        public int Idx
        {
            get
            {
                return this.GetValIntByKey(WFEmpAttr.Idx);
            }
            set
            {
                SetValByKey(WFEmpAttr.Idx, value);
            }
        }
        public string TelHtml
        {
            get
            {
                if (this.Tel.Length == 0)
                    return " Not set ";
                else
                    return "<a href=\"javascript:WinOpen('./Msg/SMS.aspx?Tel=" + this.Tel + "');\"  ><img src='/WF/Img/SMS.gif' border=0/>" + this.Tel + "</a>";
            }
        }
        public string EmailHtml
        {
            get
            {
                if (this.Email==null || this.Email.Length == 0)
                    return  " Not set ";
                else
                    return "<a href='Mailto:" + this.Email + "' ><img src='/WF/Img/SMS.gif' border=0/>" + this.Email + "</a>";
            }
        }
        public string Email
        {
            get
            {
                return this.GetValStringByKey(WFEmpAttr.Email);
            }
            set
            {
                SetValByKey(WFEmpAttr.Email, value);
            }
        }
        public string Author
        {
            get
            {
                return this.GetValStrByKey(WFEmpAttr.Author);
            }
            set
            {
                SetValByKey(WFEmpAttr.Author, value);
            }
        }
        public string AuthorDate
        {
            get
            {
                return this.GetValStringByKey(WFEmpAttr.AuthorDate);
            }
            set
            {
                SetValByKey(WFEmpAttr.AuthorDate, value);
            }
        }
        public string AuthorToDate
        {
            get
            {
                return this.GetValStringByKey(WFEmpAttr.AuthorToDate);
            }
            set
            {
                SetValByKey(WFEmpAttr.AuthorToDate, value);
            }
        }
        /// <summary>
        ///  Authorization Process 
        /// </summary>
        public string AuthorFlows
        {
            get
            {
                string s= this.GetValStringByKey(WFEmpAttr.AuthorFlows);
                s = s.Replace(",", "','");
                return "('"+s+"')";
            }
            set
            {
                // The authorization process is empty bug   Solve 
                if (!string.IsNullOrEmpty(value))
                {
                    SetValByKey(WFEmpAttr.AuthorFlows, value.Substring(1));
                }
                else
                {
                    SetValByKey(WFEmpAttr.AuthorFlows, "");
                }
                //SetValByKey(WFEmpAttr.AuthorFlows, value.Substring(1));
            }
        }
        public string FtpUrl
        {
            get
            {
                return this.GetValStringByKey(WFEmpAttr.FtpUrl);
            }
            set
            {
                SetValByKey(WFEmpAttr.FtpUrl, value);
            }
        }
        public string Stas
        {
            get
            {
                string s= this.GetValStringByKey(WFEmpAttr.Stas);
                if (s == "")
                {
                    EmpStations ess = new EmpStations();
                    ess.Retrieve(EmpStationAttr.FK_Emp, this.No);
                    foreach (EmpStation es in ess)
                    {
                        s += es.FK_StationT + ",";
                    }

                    if (ess.Count != 0)
                    {
                        this.Stas = s;
                        this.Update();
                        //this.Update(WFEmpAttr.Stas, s);
                    }
                    return s;
                }
                else
                {
                    return s;
                }
            }
            set
            {
                SetValByKey(WFEmpAttr.Stas, value);
            }
        }
        /// <summary>
        ///  Authorization 
        /// </summary>
        public AuthorWay HisAuthorWay
        {
            get
            {
                return (AuthorWay)this.AuthorWay;
            }
        }
        /// <summary>
        ///  Authorization 
        /// </summary>
        public int AuthorWay
        {
            get
            {
                return this.GetValIntByKey(WFEmpAttr.AuthorWay);
            }
            set
            {
                SetValByKey(WFEmpAttr.AuthorWay, value);
            }
        }
        public bool AuthorIsOK
        {
            get
            {
                int b= this.GetValIntByKey(WFEmpAttr.AuthorWay);
                if (b == 0)
                    return false;

                if (this.AuthorToDate.Length < 4)
                    return false; /* Do not fill in time */

                DateTime dt = DataType.ParseSysDateTime2DateTime(this.AuthorToDate);
                if (dt < DateTime.Now)
                    return false;

                return true;
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  The operator 
        /// </summary>
        public WFEmp() { }
        /// <summary>
        ///  The operator 
        /// </summary>
        /// <param name="no"></param>
        public WFEmp(string no)
        {
            this.No = no;
            try
            {
                if (this.RetrieveFromDBSources() == 0)
                {
                    Emp emp = new Emp(no);
                    this.Copy(emp);
                    this.Insert();
                }
            }
            catch
            {
                this.CheckPhysicsTable();
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

                Map map = new Map();
                map.PhysicsTable = "WF_Emp";
                map.EnDesc = " The operator ";
                map.EnType = EnType.App;

                map.AddTBStringPK(WFEmpAttr.No, null, "No", true, true, 1, 50, 36);
                map.AddTBString(WFEmpAttr.Name, null, "Name", true,false, 0, 200, 20);
                map.AddTBInt(WFEmpAttr.UseSta, 1, " User Status 0 Disable ,1 Normal .", true, true);

                map.AddTBString(WFEmpAttr.Tel, null, "Tel", true, true, 0, 200, 20);
                map.AddTBString(WFEmpAttr.FK_Dept, null, "FK_Dept", true, true, 0, 100, 36);
                map.AddTBString(WFEmpAttr.Email, null, "Email", true, true, 0, 200, 20);
                map.AddTBString(WFEmpAttr.TM, null, " Im No. ", true, true, 0, 200, 20);

                map.AddDDLSysEnum(WFEmpAttr.AlertWay, 3, " Listen to the way ", true, true, 
                    WFEmpAttr.AlertWay);
                map.AddTBString(WFEmpAttr.Author, null, " Authorized person ", true, true, 0, 200, 20);
                map.AddTBString(WFEmpAttr.AuthorDate, null, " Authorization Date ", true, true, 0, 200, 20);

                //0 Not authorized , 1 Fully licensed ,2, Specifies the authorization process scope . 
                map.AddTBInt(WFEmpAttr.AuthorWay, 0, " Authorization ", true, true);
                map.AddTBDate(WFEmpAttr.AuthorToDate, null, " Authorization to date ", true, true);
                map.AddTBString(WFEmpAttr.AuthorFlows, null, " Authorization process can be performed ", true, true, 0, 1000, 20);

                map.AddTBString(WFEmpAttr.Stas, null, " Post s", true, true, 0, 3000, 20);
                map.AddTBString(WFEmpAttr.FtpUrl, null, "FtpUrl", true, true, 0, 200, 20);
                map.AddTBString(WFEmpAttr.Msg, null, "Msg", true, true, 0, 4000, 20);
                map.AddTBString(WFEmpAttr.Style, null, "Style", true, true, 0, 4000, 20);
                map.AddTBInt(WFEmpAttr.Idx, 0, "Idx", false, false);
                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        #region  Method 
        protected override bool beforeUpdate()
        {
            string msg = "";
            //if (this.Email.Length == 0)
            //{
            //    if (this.HisAlertWay == AlertWay.SMSAndEmail || this.HisAlertWay == AlertWay.Email)
            //        msg += " Error : You set up by e-mail Receive information , But you do not set e-mail.";
            //}

            //if (this.Tel.Length == 0)
            //{
            //    if (this.HisAlertWay == AlertWay.SMSAndEmail || this.HisAlertWay == AlertWay.SMS)
            //        msg += " Error : You set up to receive information by SMS , But you did not set up a phone number .";
            //}

            EmpStations ess = new EmpStations();
            ess.Retrieve(EmpStationAttr.FK_Emp, this.No);
            string sts = "";
            foreach (EmpStation es in ess)
            {
                sts += es.FK_StationT + " ";
            }
            this.Stas = sts;

            if (msg != "")
                throw new Exception(msg);

            return base.beforeUpdate();
        }
        protected override bool beforeInsert()
        {
            this.UseSta = 1;
            return base.beforeInsert();
        }
        #endregion

        public static void DTSData()
        {
            string sql = "select No from Port_Emp where No not in (select No from WF_Emp)";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                BP.Port.Emp emp1 = new BP.Port.Emp(dr["No"].ToString());
                BP.WF.Port.WFEmp empWF = new BP.WF.Port.WFEmp();
                empWF.Copy(emp1);
                try
                {
                    empWF.UseSta = 1;
                    empWF.DirectInsert();
                }
                catch
                {
                }
            }
        }
        public void DoUp()
        {
            this.DoOrderUp("FK_Dept", this.FK_Dept, "Idx");
            return;
        }
        public void DoDown()
        {
            this.DoOrderDown("FK_Dept", this.FK_Dept, "Idx");
            return;
        }
    }
	/// <summary>
	///  The operator s 
	/// </summary>
	public class WFEmps : EntitiesNoName
	{	 
		#region  Structure 
		/// <summary>
		///  The operator s
		/// </summary>
		public WFEmps()
		{
		}
		/// <summary>
		///  Get it  Entity
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new WFEmp();
			}
		}

        public override int RetrieveAll()
        {
            return base.RetrieveAll("FK_Dept","Idx");
        }
		#endregion
	}
	
}
