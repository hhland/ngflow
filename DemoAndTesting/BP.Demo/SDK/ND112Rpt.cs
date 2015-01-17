using System;
using System.Collections.Generic;
using System.Text;
using BP.En;
using BP.WF.Data;
using BP.WF.Template;

namespace BP.Demo
{
    /// <summary>
    ///  Office of the text inside 
    /// </summary>
    public class ND112RptAttr : BP.WF.Data.NDXRptBaseAttr
    {
        #region  Extension field 
        /// <summary>
        ///  Secret registration 
        /// </summary>
        public const string MiMiDengJi = "MiMiDengJi";
        /// <summary>
        ///  Document Title 
        /// </summary>
        public const string WenJianBiaoTi = "WenJianBiaoTi";
        /// <summary>
        ///  Urgency 
        /// </summary>
        public const string JinJiChengDu = "JinJiChengDu";
        /// <summary>
        ///  Audit Office of the leadership needed 
        /// </summary>
        public const string BanGongTing = "BanGongTing";
        /// <summary>
        ///  Office of Leadership 
        /// </summary>
        public const string BgtLingDao = "BgtLingDao";
        /// <summary>
        ///  Standing Committee Leadership 
        /// </summary>
        public const string CwhLingDao = "CwhLingDao";
        /// <summary>
        ///  Communications Unit category 
        /// </summary>
        public const string Port_DeptType = "Port_DeptType";
        /// <summary>
        ///  Communications Unit 
        /// </summary>
        public const string Port_OutDept = "Port_OutDept";
        /// <summary>
        ///  Number to text 
        /// </summary>
        public const string FaWenZiHao = "LaiWenZiHao";
        /// <summary>
        ///  Read organizer 
        /// </summary>
        public const string FK_LEADER = "FK_LEADER";
        /// <summary>
        ///  Date of receipt 
        /// </summary>
        public const string ShouWenRiQi = "ShouWenRiQi";
        /// <summary>
        ///  Office of the text number 
        /// </summary>
        public const string BanWenBianHao = "BanWenBianHao";
        /// <summary>
        ///  Summary 
        /// </summary>
        public const string nrzynbyj = "nrzynbyj";
        #endregion  Extension field 
    }
    /// <summary>
    ///  Issued 
    /// </summary>
    public class ND112Rpt : BP.WF.Data.NDXRptBase
    {
        #region  Basic properties 
        /// <summary>
        ///  Secret Level 
        /// </summary>
        public string MiMiDengJi
        {
            get { return this.GetValStringByKey(ND112RptAttr.MiMiDengJi); }
            set
            {
                this.SetValByKey(ND112RptAttr.MiMiDengJi, value);
            }
        }
        /// <summary>
        ///  Document Title 
        /// </summary>
        public string WenJianBiaoTi
        {
            get { return this.GetValStringByKey(ND112RptAttr.WenJianBiaoTi); }
            set
            {
                this.SetValByKey(ND112RptAttr.WenJianBiaoTi, value);
            }
        }


        /// <summary>
        ///  Office of the leadership needed 
        /// </summary>
        public bool BanGongTing
        {
            get { return this.GetValBooleanByKey(ND112RptAttr.BanGongTing); }
            set
            {
                this.SetValByKey(ND112RptAttr.BanGongTing, value);
            }
        }
        public string BgtLingDao
        {
            get { return this.GetValStringByKey(ND112RptAttr.BgtLingDao); }
            set
            {
                this.SetValByKey(ND112RptAttr.BgtLingDao, value);
            }
        }
        public string CwhLingDao
        {
            get { return this.GetValStringByKey(ND112RptAttr.CwhLingDao); }
            set
            {
                this.SetValByKey(ND112RptAttr.CwhLingDao, value);
            }
        }

        /// <summary>
        ///  Date of receipt 
        /// </summary>
        public bool ShouWenRiQi
        {
            get { return this.GetValBooleanByKey(ND112RptAttr.ShouWenRiQi); }
            set
            {
                this.SetValByKey(ND112RptAttr.ShouWenRiQi, value);
            }
        }
        
        public string BanWenBianHao
        {
            get { return this.GetValStringByKey(ND112RptAttr.BanWenBianHao); }
            set
            {
                this.SetValByKey(ND112RptAttr.BanWenBianHao, value);
            }
        }

        /// <summary>
        ///  Standing Committee leadership needed 
        /// </summary>
        public string Port_DeptType
        {
            get { return this.GetValStringByKey(ND112RptAttr.Port_DeptType); }
            set { this.SetValByKey(ND112RptAttr.Port_DeptType, value); }
        }
        public string Port_OutDept
        {
            get { return this.GetValStringByKey(ND112RptAttr.Port_OutDept); }
            set { this.SetValByKey(ND112RptAttr.Port_OutDept, value); }
        }
        /// <summary>
        ///  Urgency 
        /// </summary>
        public string JinJiChengDu
        {
            get { return this.GetValStringByKey(ND112RptAttr.JinJiChengDu); }
            set { this.SetValByKey(ND112RptAttr.JinJiChengDu, value); }
        }

        /// <summary>
        ///  Units issued 
        /// </summary>
        public string FaWenZiHao
        {
            get { return this.GetValStringByKey(ND112RptAttr.FaWenZiHao); }
            set { this.SetValByKey(ND112RptAttr.FaWenZiHao, value); }
        }

        /// <summary>
        ///  Read organizer 
        /// </summary>
        public string FK_LEADER
        {
            get { return this.GetValStringByKey(ND112RptAttr.FK_LEADER); }
            set
            { this.SetValByKey(ND112RptAttr.FK_LEADER, value); }
        }

        public string nrzynbyj
        {
            get { return this.GetValStringByKey(ND112RptAttr.nrzynbyj); }
            set
            { this.SetValByKey(ND112RptAttr.nrzynbyj, value); }
        }

        #endregion  Basic properties 

        #region  Constructor 

        /// <summary>
        ///  Issued 
        /// </summary>
        public ND112Rpt()
        {
        }

        /// <summary>
        ///  Issued 
        /// </summary>
        /// <param name="workid"></param>
        public ND112Rpt(Int64 workid)
        {
            this.OID = workid;
            this.Retrieve();
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
                Map map = new Map("ND112Rpt");
                map.EnDesc = " Number issued ";
                map.EnType = EnType.App;

                #region  The basic flow field 

                map.AddTBIntPKOID();
                map.AddTBString(ND112RptAttr.Title, null, " Title ", false, true, 0, 500, 10);
                map.AddTBString(ND112RptAttr.FK_Dept, null, " Membership department ", false, true, 0, 200, 10);
                map.AddTBString(ND112RptAttr.FK_NY, null, " Years ", false, true, 0, 200, 10);
                map.AddDDLSysEnum(ND112RptAttr.WFState, 0, " Status ", false, true, "WFState");

                map.AddTBInt(ND112RptAttr.FID, 0, " Status ", false, true);
                map.AddTBInt(ND112RptAttr.FlowDaySpan, 0, " Status ", false, true);
                map.AddTBInt(ND112RptAttr.FlowEndNode, 0, " End point ", false, true);

                map.AddTBString(ND112RptAttr.FlowEmps, null, " Participants ", false, true, 0, 200, 10);
                map.AddTBString(ND112RptAttr.FlowEnder, null, " The last node processors ", false, true, 0, 200, 10);
                map.AddTBString(ND112RptAttr.FlowEnderRDT, null, " The final disposition date ", false, true, 0, 200, 10);
                map.AddTBString(ND112RptAttr.FlowStarter, null, " Process sponsor ", false, true, 0, 200, 10);
                map.AddTBString(ND112RptAttr.FlowStartRDT, null, " Process initiated by date ", false, true, 0, 200, 10);
                map.AddTBString(ND112RptAttr.GuestNo, null, " Customer Number ", false, true, 0, 200, 10);
                map.AddTBString(ND112RptAttr.GuestName, null, " Customer Name ", false, true, 0, 200, 10);

                map.AddTBString(ND112RptAttr.PFlowNo, null, " Parent process ID ", false, true, 0, 200, 10);
                map.AddTBInt(ND112RptAttr.PWorkID, 0, " Parent process ID", false, true);
                map.AddTBString(ND112RptAttr.BillNo, null, " Document Number ", false, true, 0, 100, 10);

                #endregion  The basic flow field 

                #region  Extension field 

                map.AddTBString(ND112RptAttr.WenJianBiaoTi, null, " Document Title ", false, true, 0, 20, 10);
                map.AddDDLSysEnum(ND112RptAttr.MiMiDengJi, 0, " Secret registration ", false, true, ND112RptAttr.MiMiDengJi, "@0=无@1= General @2= Secret @3= Confidential @4= Top-secret ");
                map.AddDDLSysEnum(ND112RptAttr.JinJiChengDu, 0, " Urgency ", false, true, ND112RptAttr.JinJiChengDu, "@0= Flat pieces @1= Urgent ");
                map.AddTBString(ND112RptAttr.FaWenZiHao, null, " Number to text ", false, true, 0, 20, 10);
                map.AddDDLEntities(ND112RptAttr.Port_DeptType, null, " Communications Unit category ", new BP.Port.Emps(), false);
                map.AddDDLEntities(ND112RptAttr.Port_OutDept, null, " Communications Unit ", new BP.Port.Emps(), false);
                map.AddDDLEntities(ND112RptAttr.FK_LEADER, null, " Read organizer ", new BP.Port.Depts(), false);

                map.AddTBString(ND112RptAttr.ShouWenRiQi, null, " Date of receipt ", false, true, 0, 20, 10);
                map.AddTBString(ND112RptAttr.BanWenBianHao, null, " Office of the text number ", false, true, 0, 20, 10);
                map.AddTBString(ND112RptAttr.nrzynbyj, null, " Summary ", false, true, 0, 20, 10);

                map.AddBoolean(ND112RptAttr.BanGongTing, false, " Office of Leadership ", false, true);
                map.AddDDLEntities(ND112RptAttr.BgtLingDao, null, " Office of Leadership ", new BP.Port.Emps(), false);
                map.AddDDLEntities(ND112RptAttr.CwhLingDao, null, " Standing Committee Leadership ", new BP.Port.Emps(), false);

                #endregion  Extension field 

                this._enMap = map;
                return this._enMap;
            }
        }

        #endregion  Constructor 
    }
    /// <summary>
    ///  Issued s BP.Port.FK.ND112Rpts
    /// </summary>
    public class ND112Rpts : BP.WF.Data.NDXRptBases
    {
        #region  Method 
        /// <summary>
        ///  Get it  Entity
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new ND112Rpt();
            }
        }
        /// <summary>
        ///
        /// </summary>
        public ND112Rpts()
        {
        }

        #endregion  Method 
    }
}
