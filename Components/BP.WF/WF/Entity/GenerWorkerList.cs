using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.WF;
using BP.Port;
using BP.En;

namespace BP.WF
{
    /// <summary>
    ///  Staff set 
    /// </summary>
    public class GenerWorkerListAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  Work node 
        /// </summary>
        public const string WorkID = "WorkID";
        /// <summary>
        ///  Punishment Document Number 
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        ///  Process 
        /// </summary>
        public const string FK_Flow = "FK_Flow";
        /// <summary>
        ///  Collection software is not fine 
        /// </summary>
        public const string FK_Emp = "FK_Emp";
        /// <summary>
        ///  Positions used 
        /// </summary>
        public const string UseStation_del = "UseStation";
        /// <summary>
        ///  Department use 
        /// </summary>
        public const string FK_Dept = "FK_Dept";
        /// <summary>
        ///  Time should be completed 
        /// </summary>
        public const string SDT = "SDT";
        /// <summary>
        ///  Warning Date 
        /// </summary>
        public const string DTOfWarning = "DTOfWarning";
        /// <summary>
        ///  Record Time 
        /// </summary>
        public const string RDT = "RDT";
        /// <summary>
        ///  Completion Time 
        /// </summary>
        public const string CDT = "CDT";
        /// <summary>
        ///  Is available 
        /// </summary>
        public const string IsEnable = "IsEnable";
        /// <summary>
        /// WarningDays
        /// </summary>
        public const string WarningDays = "WarningDays";
        /// <summary>
        ///  Is automatically assigned 
        /// </summary>
        //public const  string IsAutoGener="IsAutoGener";
        /// <summary>
        ///  Generation time 
        /// </summary>
        //public const  string GenerDateTime="GenerDateTime";
        /// <summary>
        /// IsPass
        /// </summary>
        public const string IsPass = "IsPass";
        /// <summary>
        ///  Process ID
        /// </summary>
        public const string FID = "FID";
        /// <summary>
        ///  PERSON 
        /// </summary>
        public const string FK_EmpText = "FK_EmpText";
        /// <summary>
        ///  Node Name 
        /// </summary>
        public const string FK_NodeText = "FK_NodeText";
        /// <summary>
        ///  Sender 
        /// </summary>
        public const string Sender = "Sender";
        /// <summary>
        ///  Who performed it ?
        /// </summary>
        public const string WhoExeIt = "WhoExeIt";
        /// <summary>
        ///  Priority 
        /// </summary>
        public const string PRI = "PRI";
        /// <summary>
        ///  Whether read ?
        /// </summary>
        public const string IsRead = "IsRead";
        /// <summary>
        ///  Reminders times 
        /// </summary>
        public const string PressTimes = "PressTimes";
        /// <summary>
        ///  Remark 
        /// </summary>
        public const string Tag = "Tag";
        /// <summary>
        ///  Parameters 
        /// </summary>
        public const string Paras = "Paras";
        /// <summary>
        ///  Hang time 
        /// </summary>
        public const string DTOfHungUp = "DTOfHungUp";
        /// <summary>
        ///  Lift hang time 
        /// </summary>
        public const string DTOfUnHungUp = "DTOfUnHungUp";
        /// <summary>
        ///  Pending times 
        /// </summary>
        public const string HungUpTimes = "HungUpTimes";
        /// <summary>
        ///  External user ID 
        /// </summary>
        public const string GuestNo = "GuestNo";
        /// <summary>
        ///  External user name 
        /// </summary>
        public const string GuestName = "GuestName";
        #endregion
    }
    /// <summary>
    ///  Workers List 
    /// </summary>
    public class GenerWorkerList : Entity
    {
        #region  Parameter Properties .
        /// <summary>
        ///  Is CC .
        /// </summary>
        public bool IsCC
        {
            get
            {
                return this.GetParaBoolen("IsCC", false);
            }
            set
            {
                this.SetPara("IsCC", value);
            }
        }
        #endregion  Parameter Properties .

        #region  Basic properties 
        /// <summary>
        ///  Who will perform it 
        /// </summary>
        public int WhoExeIt
        {
            get
            {
                return this.GetValIntByKey(GenerWorkerListAttr.WhoExeIt);
            }
            set
            {
                SetValByKey(GenerWorkerListAttr.WhoExeIt, value);
            }
        }
        public int PressTimes
        {
            get
            {
                return this.GetValIntByKey(GenerWorkerListAttr.PressTimes);
            }
            set
            {
                SetValByKey(GenerWorkerListAttr.PressTimes, value);
            }
        }
        /// <summary>
        ///  Priority 
        /// </summary>
        public int PRI
        {
            get
            {
                return this.GetValIntByKey(GenerWorkFlowAttr.PRI);
            }
            set
            {
                SetValByKey(GenerWorkFlowAttr.PRI, value);
            }
        }
        /// <summary>
        /// WorkID
        /// </summary>
        public override string PK
        {
            get
            {
                return "WorkID,FK_Emp,FK_Node";
            }
        }
        /// <summary>
        ///  Is available ( Effective in allocating work )
        /// </summary>
        public bool IsEnable
        {
            get
            {
                return this.GetValBooleanByKey(GenerWorkerListAttr.IsEnable);
            }
            set
            {
                this.SetValByKey(GenerWorkerListAttr.IsEnable, value);
            }
        }
        /// <summary>
        ///  Whether through ( Countersigned node for effective audit )
        /// </summary>
        public bool IsPass
        {
            get
            {
                return this.GetValBooleanByKey(GenerWorkerListAttr.IsPass);
            }
            set
            {
                this.SetValByKey(GenerWorkerListAttr.IsPass, value);
            }
        }
        public int IsPassInt
        {
            get
            {
                return this.GetValIntByKey(GenerWorkerListAttr.IsPass);
            }
            set
            {
                this.SetValByKey(GenerWorkerListAttr.IsPass, value);
            }
        }
        public bool IsRead
        {
            get
            {
                return this.GetValBooleanByKey(GenerWorkerListAttr.IsRead);
            }
            set
            {
                this.SetValByKey(GenerWorkerListAttr.IsRead, value);
            }
        }
        /// <summary>
        /// WorkID
        /// </summary>
        public Int64 WorkID
        {
            get
            {
                return this.GetValInt64ByKey(GenerWorkerListAttr.WorkID);
            }
            set
            {
                this.SetValByKey(GenerWorkerListAttr.WorkID, value);
            }
        }
        /// <summary>
        /// Node
        /// </summary>
        public int FK_Node
        {
            get
            {
                return this.GetValIntByKey(GenerWorkerListAttr.FK_Node);
            }
            set
            {
                this.SetValByKey(GenerWorkerListAttr.FK_Node, value);
            }
           
        }
        public string FK_DeptT
        {
            get
            {
                try
                {
                    Dept d = new Dept(this.FK_Dept);
                    return d.Name;
                }
                catch
                {
                    return "";
                }
            }
        }
        public string FK_Dept
        {
            get
            {
                return this.GetValStrByKey(GenerWorkerListAttr.FK_Dept);
            }
            set
            {
                this.SetValByKey(GenerWorkerListAttr.FK_Dept, value);
            }
        }
        /// <summary>
        ///  Sender 
        /// </summary>
        public string Sender
        {
            get
            {
                return this.GetValStrByKey(GenerWorkerListAttr.Sender);
            }
            set
            {
                this.SetValByKey(GenerWorkerListAttr.Sender, value);
            }
        }
        /// <summary>
        ///  Node Name 
        /// </summary>
        public string FK_NodeText
        {
            get
            {
                return this.GetValStrByKey(GenerWorkerListAttr.FK_NodeText);
            }
            set
            {
                this.SetValByKey(GenerWorkerListAttr.FK_NodeText, value);
            }
        }
        /// <summary>
        ///  Process ID
        /// </summary>
        public Int64 FID
        {
            get
            {
                return this.GetValInt64ByKey(GenerWorkerListAttr.FID);
            }
            set
            {
                this.SetValByKey(GenerWorkerListAttr.FID, value);
            }
        }
        /// <summary>
        ///  Warnings days 
        /// </summary>
        public float WarningDays
        {
            get
            {
                return this.GetValFloatByKey(GenerWorkerListAttr.WarningDays);
            }
            set
            {
                this.SetValByKey(GenerWorkerListAttr.WarningDays, value);
            }
        }
        /// <summary>
        ///  Staff 
        /// </summary>
        public Emp HisEmp
        {
            get
            {
                return new Emp(this.FK_Emp);
            }
        }
        /// <summary>
        ///  Send date 
        /// </summary>
        public string RDT
        {
            get
            {
                return this.GetValStringByKey(GenerWorkerListAttr.RDT);
            }
            set
            {
                this.SetValByKey(GenerWorkerListAttr.RDT, value);
            }
        }
        /// <summary>
        ///  Completion Time 
        /// </summary>
        public string CDT
        {
            get
            {
                return this.GetValStringByKey(GenerWorkerListAttr.CDT);
            }
            set
            {
                this.SetValByKey(GenerWorkerListAttr.CDT, value);
            }
        }
        /// <summary>
        ///  Should be completed by the date 
        /// </summary>
        public string SDT
        {
            get
            {
                return this.GetValStringByKey(GenerWorkerListAttr.SDT);
            }
            set
            {
                this.SetValByKey(GenerWorkerListAttr.SDT, value);
            }
        }
        /// <summary>
        ///  Warning Date 
        /// </summary>
        public string DTOfWarning
        {
            get
            {
                return this.GetValStringByKey(GenerWorkerListAttr.DTOfWarning);
            }
            set
            {
                this.SetValByKey(GenerWorkerListAttr.DTOfWarning, value);
            }
        }
        /// <summary>
        ///  Staff 
        /// </summary>
        public string FK_Emp
        {
            get
            {
                return this.GetValStringByKey(GenerWorkerListAttr.FK_Emp);
            }
            set
            {
                this.SetValByKey(GenerWorkerListAttr.FK_Emp, value);
            }
        }
        /// <summary>
        ///  PERSON 
        /// </summary>
        public string FK_EmpText
        {
            get
            {
                return this.GetValStrByKey(GenerWorkerListAttr.FK_EmpText);
            }
            set
            {
                this.SetValByKey(GenerWorkerListAttr.FK_EmpText, value);
            }
        }
        /// <summary>
        ///  Process ID 
        /// </summary>		 
        public string FK_Flow
        {
            get
            {
                return this.GetValStringByKey(GenerWorkerListAttr.FK_Flow);
            }
            set
            {
                this.SetValByKey(GenerWorkerListAttr.FK_Flow, value);
            }
        }

        public string GuestNo
        {
            get
            {
                return this.GetValStringByKey(GenerWorkerListAttr.GuestNo);
            }
            set
            {
                this.SetValByKey(GenerWorkerListAttr.GuestNo, value);
            }
        }
        public string GuestName
        {
            get
            {
                return this.GetValStringByKey(GenerWorkerListAttr.GuestName);
            }
            set
            {
                this.SetValByKey(GenerWorkerListAttr.GuestName, value);
            }
        }
        #endregion

        #region  Pending Properties 
        /// <summary>
        ///  Hang time 
        /// </summary>
        public string DTOfHungUp
        {
            get
            {
                return this.GetValStringByKey(GenerWorkerListAttr.DTOfHungUp);
            }
            set
            {
                this.SetValByKey(GenerWorkerListAttr.DTOfHungUp, value);
            }
        }
        /// <summary>
        ///  Lift hang time 
        /// </summary>
        public string DTOfUnHungUp
        {
            get
            {
                return this.GetValStringByKey(GenerWorkerListAttr.DTOfUnHungUp);
            }
            set
            {
                this.SetValByKey(GenerWorkerListAttr.DTOfUnHungUp, value);
            }
        }
        /// <summary>
        ///  Pending times 
        /// </summary>
        public int HungUpTimes
        {
            get
            {
                return this.GetValIntByKey(GenerWorkerListAttr.HungUpTimes);
            }
            set
            {
                this.SetValByKey(GenerWorkerListAttr.HungUpTimes, value);
            }
        }
        #endregion 

        #region  Constructor 
        /// <summary>
        ///  Primary key 
        /// </summary>
        public override string PKField
        {
            get
            {
                return "WorkID,FK_Emp,FK_Node";
            }
        }
        /// <summary>
        ///  Workers 
        /// </summary>
        public GenerWorkerList()
        {
        }
        public GenerWorkerList(Int64 workid, int FK_Node, string FK_Emp)
        {
            if (this.WorkID == 0)
                return;

            this.WorkID = workid;
            this.FK_Node = FK_Node;
            this.FK_Emp = FK_Emp;
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

                Map map = new Map("WF_GenerWorkerlist");
                map.EnDesc = " Workers ";
                map.AddTBIntPK(GenerWorkerListAttr.WorkID, 0, " The work ID", true, true);
                map.AddTBStringPK(GenerWorkerListAttr.FK_Emp, null, " Staff ", true, false, 0, 20, 100);
                map.AddTBIntPK(GenerWorkerListAttr.FK_Node, 0, " Node ID", true, false);
                map.AddTBInt(GenerWorkerListAttr.FID, 0, " Process ID", true, false);

                map.AddTBString(GenerWorkerListAttr.FK_EmpText, null, " PERSON ", true, false, 0, 30, 100);
                map.AddTBString(GenerWorkerListAttr.FK_NodeText, null, " Node Name ", true, false, 0, 100, 100);

                map.AddTBString(GenerWorkerListAttr.FK_Flow, null, " Process ", true, false, 0, 3, 100);
                map.AddTBString(GenerWorkerListAttr.FK_Dept, null, " Use Sector ", true, false, 0, 100, 100);

                // If the flow properties to control the flow properties to be calculated according to .
                map.AddTBDateTime(GenerWorkerListAttr.SDT, " Should be completed by the date ", false, false);
                map.AddTBDateTime(GenerWorkerListAttr.DTOfWarning, " Warning Date ", false, false);
                map.AddTBFloat(GenerWorkerListAttr.WarningDays, 0, " Warning Day ", true, false);
                map.AddTBDateTime(GenerWorkerListAttr.RDT, " Record Time ", false, false);
                map.AddTBDateTime(GenerWorkerListAttr.CDT, " Completion Time ", false, false);
                map.AddBoolean(GenerWorkerListAttr.IsEnable, true, " Is available ", true, true);

                //  add for  Shanghai  2012-11-30
                map.AddTBInt(GenerWorkerListAttr.IsRead, 0, " Whether read ", true, true);

                // Countersigned node for effective 
                map.AddTBInt(GenerWorkerListAttr.IsPass, 0, " Whether through ( Valid for merging nodes )", false, false);

                //  Who performed it ?
                map.AddTBInt(GenerWorkerListAttr.WhoExeIt, 0, " Who performed it ", false, false);

                // Sender . 2011-11-12  Tianjin increased user .
                map.AddTBString(GenerWorkerListAttr.Sender, null, " Sender ", true, false, 0, 20, 100);

                // Priority ,2012-06-15  Qingdao increase user .
                map.AddTBInt(GenerWorkFlowAttr.PRI, 1, " Priority ", true, true);

                // Reminders times .  To add Boco .
                map.AddTBInt(GenerWorkerListAttr.PressTimes, 0, " Reminders times ", true, false);

                //  Pending 
                map.AddTBDateTime(GenerWorkerListAttr.DTOfHungUp,null, " Hang time ", false, false);
                map.AddTBDateTime(GenerWorkerListAttr.DTOfUnHungUp,null, " Expected to lift the suspension time ", false, false);
                map.AddTBInt(GenerWorkerListAttr.HungUpTimes, 0, " Pending times ", true, false);

                // External users . 203-08-30
                map.AddTBString(GenerWorkerListAttr.GuestNo, null, " External user ID ", true, false, 0, 30, 100);
                map.AddTBString(GenerWorkerListAttr.GuestName, null, " External user name ", true, false, 0, 100, 100);

                // Parameter markers  2014-04-05.
                map.AddTBAtParas(4000); 

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        protected override bool beforeInsert()
        {
            if (this.FID != 0)
            {
                if (this.FID == this.WorkID)
                    this.FID = 0;
            }
            this.Sender = BP.Web.WebUser.No;
            return base.beforeInsert();
        }
    }
    /// <summary>
    ///  Staff set 
    /// </summary>
    public class GenerWorkerLists : Entities
    {
        #region  Method 
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new GenerWorkerList();
            }
        }
        /// <summary>
        /// GenerWorkerList
        /// </summary>
        public GenerWorkerLists() { }
        public GenerWorkerLists(Int64 workId)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(GenerWorkerListAttr.WorkID, workId);
            qo.addOrderBy(GenerWorkerListAttr.RDT);
            qo.DoQuery();
            return;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="workId"></param>
        /// <param name="nodeId"></param>
        public GenerWorkerLists(Int64 workId, int nodeId)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(GenerWorkerListAttr.WorkID, workId);
            qo.addAnd();
            qo.AddWhere(GenerWorkerListAttr.FK_Node, nodeId);
            qo.DoQuery();
            return;
        }
        public GenerWorkerLists(Int64 workId, int nodeId,string FK_Emp)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(GenerWorkerListAttr.WorkID, workId);
            qo.addAnd();
            qo.AddWhere(GenerWorkerListAttr.FK_Node, nodeId);
            qo.addAnd();
            qo.AddWhere(GenerWorkerListAttr.FK_Emp, FK_Emp);
            qo.DoQuery();
            return;
        }
        /// <summary>
        ///  Construction workers set 
        /// </summary>
        /// <param name="workId"> The work ID</param>
        /// <param name="nodeId"> Node ID</param>
        /// <param name="isWithEmpExts"> Whether to include personnel assigned </param>
        public GenerWorkerLists(Int64 workId, int nodeId, bool isWithEmpExts)
        {
            QueryObject qo = new QueryObject(this);
            qo.addLeftBracket();
            qo.AddWhere(GenerWorkerListAttr.WorkID, workId);
            qo.addOr();
            qo.AddWhere(GenerWorkerListAttr.FID, workId);
            qo.addRightBracket();
            qo.addAnd();
            qo.AddWhere(GenerWorkerListAttr.FK_Node, nodeId);
            int i = qo.DoQuery();

            if (isWithEmpExts == false)
                return;

            if (i == 0)
                throw new Exception("@ System error , Staff is missing, please contact your administrator .NodeID=" + nodeId + " WorkID=" + workId);

            RememberMe rm = new RememberMe();
            rm.FK_Emp = BP.Web.WebUser.No;
            rm.FK_Node = nodeId;
            if (rm.RetrieveFromDBSources() == 0)
                return;

            GenerWorkerList wl = (GenerWorkerList)this[0];
            string[] emps = rm.Emps.Split('@');
            foreach (string emp in emps)
            {
                if (emp==null || emp=="")
                    continue;

                if (this.GetCountByKey(GenerWorkerListAttr.FK_Emp, emp) >= 1)
                    continue;

                GenerWorkerList mywl = new GenerWorkerList();
                mywl.Copy(wl);
                mywl.IsEnable = false;
                mywl.FK_Emp = emp;
                WF.Port.WFEmp myEmp = new Port.WFEmp(emp);
                mywl.FK_EmpText = myEmp.Name;
                try
                {
                    mywl.Insert();
                }
                catch
                {
                    mywl.Update();
                    continue;
                }
                this.AddEntity(mywl);
            }
            return;
        }
        /// <summary>
        ///  Workers 
        /// </summary>
        /// <param name="workId"> Workers ID</param>
        /// <param name="flowNo"> Process ID </param>
        public GenerWorkerLists(Int64 workId, string flowNo)
        {
            if (workId == 0)
                return;

            Flow fl = new Flow(flowNo);
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(GenerWorkerListAttr.WorkID, workId);
            qo.addAnd();
            qo.AddWhere(GenerWorkerListAttr.FK_Flow, flowNo);
            qo.DoQuery();
        }
        #endregion
    }
}
