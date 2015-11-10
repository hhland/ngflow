using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.Port;

namespace BP.WF.Template
{
	/// <summary>
	///  People choose to accept property 
	/// </summary>
    public class SelectAccperAttr
    {
        /// <summary>
        ///  The work ID
        /// </summary>
        public const string WorkID = "WorkID";
        /// <summary>
        ///  Node 
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        ///  To staff 
        /// </summary>
        public const string FK_Emp = "FK_Emp";
        /// <summary>
        ///  Operator Name 
        /// </summary>
        public const string EmpName = "EmpName";
        /// <summary>
        ///  Record people 
        /// </summary>
        public const string Rec = "Rec";
        /// <summary>
        ///  Handle comments    Information 
        /// </summary>
        public const string Info = "Info";
        /// <summary>
        ///  After sending whether this calculation 
        /// </summary>
        public const string IsRemember = "IsRemember";
        /// <summary>
        ///  Sequence number 
        /// </summary>
        public const string Idx = "Idx";
        /// <summary>
        ///  Type (@0= Recipient @1= Cc )
        /// </summary>
        public const string AccType = "AccType";
    }
	/// <summary>
	///  Select recipient 
	///  Node to the staff consists of two parts .	 
	///  Records from one node to the other nodes of the plurality of .
	///  Also recorded to the other nodes of this node .
	/// </summary>
    public class SelectAccper : EntityMyPK
    {
        #region  Basic properties 
        /// <summary>
        /// The work ID
        /// </summary>
        public Int64 WorkID
        {
            get
            {
                return this.GetValInt64ByKey(SelectAccperAttr.WorkID);
            }
            set
            {
                this.SetValByKey(SelectAccperAttr.WorkID, value);
            }
        }
        /// <summary>
        /// Node 
        /// </summary>
        public int FK_Node
        {
            get
            {
                return this.GetValIntByKey(SelectAccperAttr.FK_Node);
            }
            set
            {
                this.SetValByKey(SelectAccperAttr.FK_Node, value);
            }
        }
        /// <summary>
        ///  To staff 
        /// </summary>
        public string FK_Emp
        {
            get
            {
                return this.GetValStringByKey(SelectAccperAttr.FK_Emp);
            }
            set
            {
                this.SetValByKey(SelectAccperAttr.FK_Emp, value);
            }
        }
        /// <summary>
        ///  PERSON 
        /// </summary>
        public string EmpName
        {
            get
            {
                string s= this.GetValStringByKey(SelectAccperAttr.EmpName);
                if (s == "" || s == null)
                    s = this.FK_Emp;
                return s;
            }
            set
            {
                this.SetValByKey(SelectAccperAttr.EmpName, value);
            }
        }
        /// <summary>
        ///  Recipient 
        /// </summary>
        public string Rec
        {
            get
            {
                return this.GetValStringByKey(SelectAccperAttr.Rec);
            }
            set
            {
                this.SetValByKey(SelectAccperAttr.Rec, value);
            }
        }
        /// <summary>
        ///  Handle comments    Information 
        /// </summary>
        public string Info
        {
            get
            {
                return this.GetValStringByKey(SelectAccperAttr.Info);
            }
            set
            {
                this.SetValByKey(SelectAccperAttr.Info, value);
            }
        }
        /// <summary>
        ///  Are memories 
        /// </summary>
        public bool IsRemember
        {
            get
            {
                return this.GetValBooleanByKey(SelectAccperAttr.IsRemember);
            }
            set
            {
                this.SetValByKey(SelectAccperAttr.IsRemember, value);
            }
        }
        /// <summary>
        ///  Sequence number 
        /// </summary>
        public int Idx
        {
            get
            {
                return this.GetValIntByKey(SelectAccperAttr.Idx);
            }
            set
            {
                this.SetValByKey(SelectAccperAttr.Idx, value);
            }
        }
        /// <summary>
        ///   Type (@0= Recipient @1= Cc )
        /// </summary>
        public int AccType
        {
            get
            {
                return this.GetValIntByKey(SelectAccperAttr.AccType);
            }
            set
            {
                this.SetValByKey(SelectAccperAttr.AccType, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Select recipient 
        /// </summary>
        public SelectAccper()
        {
        }
        public SelectAccper(string mypk)
        {
            this.MyPK = mypk;
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

                Map map = new Map("WF_SelectAccper");
                map.EnDesc = " Choose to accept / Cc information ";

                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.AddMyPK();

                map.AddTBInt(SelectAccperAttr.FK_Node, 0, " Recipient node ", true, false);
                map.AddTBInt(SelectAccperAttr.WorkID, 0, "WorkID", true, false);
                map.AddTBString(SelectAccperAttr.FK_Emp, null, "FK_Emp", true, false, 0, 20, 10);
                map.AddTBString(SelectAccperAttr.EmpName, null, "EmpName", true, false, 0, 20, 10);

                map.AddTBInt(SelectAccperAttr.AccType, 0, " Type (@0= Recipient @1= Cc )", true, false);
                map.AddTBString(SelectAccperAttr.Rec, null, " Record people ", true, false, 0, 20, 10);
                map.AddTBString(SelectAccperAttr.Info, null, " Handle comments   Information ", true, false, 0, 200, 10);

                map.AddTBInt(SelectAccperAttr.IsRemember, 0, " After the transmission is by this calculation ", true, false);
                map.AddTBInt(SelectAccperAttr.Idx, 0, " Sequence number ( Audit mode can be used to process the queue )", true, false);
                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion


        protected override bool beforeInsert()
        {
            this.ResetPK();
            return base.beforeInsert();
        }

        public void ResetPK()
        {
            this.MyPK = this.FK_Node + "_" + this.WorkID + "_" + this.FK_Emp;
        }
        protected override bool beforeUpdateInsertAction()
        {
            this.ResetPK();
            this.Rec = BP.Web.WebUser.No;
            return base.beforeUpdateInsertAction();
        }
        //protected override bool beforeUpdateInsertAction()
        //{
        //    this.Rec = BP.Web.WebUser.No;
        //    return base.beforeUpdateInsertAction();
        //}
    }
	/// <summary>
	///  Select recipient 
	/// </summary>
    public class SelectAccpers : EntitiesMyPK
    {
        /// <summary>
        ///  Choose whether the next memory 
        /// </summary>
        public bool IsSetNextTime
        {
            get
            {
                if (this.Count == 0)
                    return false;

                foreach (SelectAccper item in this)
                {
                    if (item.IsRemember == true)
                        return item.IsRemember;
                }
                return false;
            }
        }
        /// <summary>
        ///  Query recipient , If the recipient is not set to check the history settings .
        /// </summary>
        /// <param name="fk_node"></param>
        /// <param name="Rec"></param>
        /// <returns></returns>
        public int QueryAccepter(int fk_node, string rec, Int64 workid)
        {
            // Check out the current data .
            int i = this.Retrieve(SelectAccperAttr.FK_Node, fk_node,
                 SelectAccperAttr.WorkID, workid);
            if (i != 0)
                return i; /* If you did not find the greatest workid.*/

            // Identify recent work ID
            int maxWorkID = BP.DA.DBAccess.RunSQLReturnValInt("SELECT Max(WorkID) FROM WF_SelectAccper WHERE Rec='" + rec + "' AND FK_Node=" + fk_node, 0);
            if (maxWorkID == 0)
                return 0;

            // Check out the data .
            this.Retrieve(SelectAccperAttr.FK_Node, fk_node,
           SelectAccperAttr.WorkID, maxWorkID);

            // Return query results .
            return this.Count;
        }
        /// <summary>
        ///  Check the last setting 
        /// </summary>
        /// <param name="fk_node"> Node number </param>
        /// <param name="rec"> Current staff </param>
        /// <param name="workid"> The work ID</param>
        /// <returns></returns>
        public int QueryAccepterPriSetting(int fk_node)
        {
            // Identify recent work ID.
            int maxWorkID = BP.DA.DBAccess.RunSQLReturnValInt("SELECT Max(WorkID) FROM WF_SelectAccper WHERE " + SelectAccperAttr.IsRemember + "=1 AND Rec='" + BP.Web.WebUser.No + "' AND FK_Node=" + fk_node, 0);
            if (maxWorkID == 0)
                return 0;

            // Check out the data .
            this.Retrieve(SelectAccperAttr.FK_Node, fk_node,
           SelectAccperAttr.WorkID, maxWorkID);

            // Return query results .
            return this.Count;
        }
        /// <summary>
        ///  To his staff 
        /// </summary>
        public Emps HisEmps
        {
            get
            {
                Emps ens = new Emps();
                foreach (SelectAccper ns in this)
                {
                    ens.AddEntity(new Emp(ns.FK_Emp));
                }
                return ens;
            }
        }
        /// <summary>
        ///  His work node 
        /// </summary>
        public Nodes HisNodes
        {
            get
            {
                Nodes ens = new Nodes();
                foreach (SelectAccper ns in this)
                {
                    ens.AddEntity(new Node(ns.FK_Node));
                }
                return ens;
            }
        }
        /// <summary>
        ///  Select recipient 
        /// </summary>
        public SelectAccpers() { }
        /// <summary>
        ///  Check out the selection of personnel 
        /// </summary>
        /// <param name="fk_flow"></param>
        /// <param name="workid"></param>
        public SelectAccpers( Int64 workid)
        {
            BP.En.QueryObject qo = new QueryObject(this);
            qo.AddWhere(SelectAccperAttr.WorkID, workid);
            qo.addOrderByDesc(SelectAccperAttr.FK_Node,SelectAccperAttr.Idx);
            qo.DoQuery();

          //  this.Retrieve(SelectAccperAttr.WorkID, workid, SelectAccperAttr.Idx);
        }
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new SelectAccper();
            }
        }
    }
}
