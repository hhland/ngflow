using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.En;
using BP.Port;
using BP.Sys;

namespace BP.WF.Template
{
    /// <summary>
    ///  Node Form Properties 	  
    /// </summary>
    public class FrmNodeAttr
    {
        /// <summary>
        ///  Node 
        /// </summary>
        public const string FK_Frm = "FK_Frm";
        /// <summary>
        ///  Work node 
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        ///  Whether readonly.
        /// </summary>
        public const string IsEdit = "IsEdit";
        /// <summary>
        /// IsPrint
        /// </summary>
        public const string IsPrint = "IsPrint";
        /// <summary>
        /// Idx
        /// </summary>
        public const string Idx = "Idx";
        /// <summary>
        /// FK_Flow
        /// </summary>
        public const string FK_Flow = "FK_Flow";
        /// <summary>
        ///  Form type 
        /// </summary>
        public const string FrmType = "FrmType";
        /// <summary>
        ///  Program 
        /// </summary>
        public const string FrmSln = "FrmSln";
        /// <summary>
        ///  Who is the primary key ?
        /// </summary>
        public const string WhoIsPK = "WhoIsPK";
    }
    /// <summary>
    ///  Who is the primary key ?
    /// </summary>
    public enum WhoIsPK
    {
        /// <summary>
        ///  The work ID Is the primary key 
        /// </summary>
        OID,
        /// <summary>
        ///  Process ID Is the primary key 
        /// </summary>
        FID,
        /// <summary>
        ///  Parent process ID Is the primary key 
        /// </summary>
        PWorkID,
        /// <summary>
        ///  Continuation of the process ID Is the primary key 
        /// </summary>
        CWorkID
    }
    /// <summary>
    ///  Node Form 
    ///  The work of the node consists of two parts .	 
    ///  Records from one node to the other nodes of the plurality of .
    ///  Also recorded to the other nodes of this node .
    /// </summary>
    public class FrmNode : EntityMyPK
    {
        #region  Basic properties 
        public string FrmUrl
        {
            get
            {
                switch (this.HisFrmType)
                {
                    case FrmType.Column4Frm:
                        return Glo.CCFlowAppPath + "WF/CCForm/FrmFix";
                    case FrmType.AspxFrm:
                        return Glo.CCFlowAppPath + "WF/CCForm/Frm";
                    case FrmType.SLFrm:
                        return Glo.CCFlowAppPath + "WF/CCForm/SLFrm";
                    default:
                        throw new Exception("err, Untreated .");
                }
            }
        }
        private Frm _hisFrm = null;
        public Frm HisFrm
        {
            get
            {
                if (this._hisFrm == null)
                {
                    this._hisFrm = new Frm(this.FK_Frm);
                    this._hisFrm.HisFrmNode = this;
                }
                return this._hisFrm;
            }
        }
        /// <summary>
        /// UI Access control interface 
        /// </summary>
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                uac.OpenForSysAdmin();
                return uac;
            }
        }
        public BP.Sys.FrmType HisFrmType
        {
            get
            {
                return (BP.Sys.FrmType)this.GetValIntByKey(FrmNodeAttr.FrmType);
            }
            set
            {
                this.SetValByKey(FrmNodeAttr.FrmType, (int)value);
            }
        }
        /// <summary>
        /// Node 
        /// </summary>
        public int FK_Node
        {
            get
            {
                return this.GetValIntByKey(FrmNodeAttr.FK_Node);
            }
            set
            {
                this.SetValByKey(FrmNodeAttr.FK_Node, value);
            }
        }
        /// <summary>
        ///  Sequence number 
        /// </summary>
        public int Idx
        {
            get
            {
                return this.GetValIntByKey(FrmNodeAttr.Idx);
            }
            set
            {
                this.SetValByKey(FrmNodeAttr.Idx, value);
            }
        }
        /// <summary>
        ///  Who is the primary key ?
        /// </summary>
        public WhoIsPK WhoIsPK
        {
            get
            {
                return (WhoIsPK)this.GetValIntByKey(FrmNodeAttr.WhoIsPK);
            }
            set
            {
                this.SetValByKey(FrmNodeAttr.WhoIsPK, (int)value);
            }
        }
        /// <summary>
        ///  Workflow 
        /// </summary>
        public string FK_Frm
        {
            get
            {
                return this.GetValStringByKey(FrmNodeAttr.FK_Frm);
            }
            set
            {
                this.SetValByKey(FrmNodeAttr.FK_Frm, value);
            }
        }
        /// <summary>
        ///  Corresponding solution 
        /// </summary>
        public int FrmSln
        {
            get
            {
                return this.GetValIntByKey(FrmNodeAttr.FrmSln);
            }
            set
            {
                this.SetValByKey(FrmNodeAttr.FrmSln, value);
            }
        }
        /// <summary>
        ///  Process ID 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.GetValStringByKey(FrmNodeAttr.FK_Flow);
            }
            set
            {
                this.SetValByKey(FrmNodeAttr.FK_Flow, value);
            }
        }
        public bool IsEdit
        {
            get
            {
                return this.GetValBooleanByKey(FrmNodeAttr.IsEdit);
            }
            set
            {
                this.SetValByKey(FrmNodeAttr.IsEdit, value);
            }
        }
        public bool IsPrint
        {
            get
            {
                return this.GetValBooleanByKey(FrmNodeAttr.IsPrint);
            }
            set
            {
                this.SetValByKey(FrmNodeAttr.IsPrint, value);
            }
        }
        public int IsEditInt
        {
            get
            {
                return this.GetValIntByKey(FrmNodeAttr.IsEdit);
            }
        }
        public int IsPrintInt
        {
            get
            {
                return this.GetValIntByKey(FrmNodeAttr.IsPrint);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Node Form 
        /// </summary>
        public FrmNode() { }
        /// <summary>
        ///  Node Form 
        /// </summary>
        /// <param name="mypk"></param>
        public FrmNode(string mypk)
            : base(mypk)
        {
        }
        /// <summary>
        ///  Node Form 
        /// </summary>
        /// <param name="fk_node"> Node </param>
        /// <param name="fk_frm"> Form </param>
        public FrmNode(string fk_flow, int fk_node, string fk_frm)
        {
            int i = this.Retrieve(FrmNodeAttr.FK_Flow, fk_flow, FrmNodeAttr.FK_Node, fk_node, FrmNodeAttr.FK_Frm, fk_frm);
            if (i == 0)
            {
                this.IsPrint = false;
                this.IsEdit = false;
                return;
                throw new Exception("@ Form-related information has been deleted .");
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

                Map map = new Map("WF_FrmNode");
                map.EnDesc = " Node Form ";

                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;

                map.AddMyPK();
                map.AddTBString(FrmNodeAttr.FK_Frm, null, " Form ID", true, true, 1, 32, 32);
                map.AddTBInt(FrmNodeAttr.FK_Node, 0, " Node number ", true, false);
                map.AddTBString(FrmNodeAttr.FK_Flow, null, " Process ID ", true, true, 1, 20, 20);
                map.AddTBString(FrmNodeAttr.FrmType, "0", " Form type ", true, true, 1, 20, 20);
                // Permissions menu in the control of this node .
                map.AddTBInt(FrmNodeAttr.IsEdit, 1, " Whether it can be updated ", true, false);
                map.AddTBInt(FrmNodeAttr.IsPrint, 0, "IsPrint", true, false);

                // Displayed 
                map.AddTBInt(FrmNodeAttr.Idx, 0, " Sequence number ", true, false);
                map.AddTBInt(FrmNodeAttr.FrmSln, 0, " Forms control scheme ", true, false);

                // add 2014-01-26
                map.AddTBInt(FrmNodeAttr.WhoIsPK, 0, " Who is the primary key ?", true, false);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        public void DoUp()
        {
            this.DoOrderUp(FrmNodeAttr.FK_Node, this.FK_Node.ToString(), FrmNodeAttr.Idx);
        }
        public void DoDown()
        {
            this.DoOrderDown(FrmNodeAttr.FK_Node, this.FK_Node.ToString(), FrmNodeAttr.Idx);
        }

        protected override bool beforeUpdateInsertAction()
        {
            this.MyPK = this.FK_Frm + "_" + this.FK_Node + "_" + this.FK_Flow;
            return base.beforeUpdateInsertAction();
        }
    }
    /// <summary>
    ///  Node Form 
    /// </summary>
    public class FrmNodes : EntitiesMM
    {
        /// <summary>
        ///  His work node 
        /// </summary>
        public Nodes HisNodes
        {
            get
            {
                Nodes ens = new Nodes();
                foreach (FrmNode ns in this)
                {
                    ens.AddEntity(new Node(ns.FK_Node));
                }
                return ens;
            }
        }
        /// <summary>
        ///  Node Form 
        /// </summary>
        public FrmNodes() { }
        /// <summary>
        ///  Node Form 
        /// </summary>
        /// <param name="NodeID"> Node ID</param>
        public FrmNodes(string fk_flow, int nodeID)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(FrmNodeAttr.FK_Flow, fk_flow);
            qo.addAnd();
            qo.AddWhere(FrmNodeAttr.FK_Node, nodeID);

            qo.addOrderBy(FrmNodeAttr.Idx);
            qo.DoQuery();
        }
        /// <summary>
        ///  Node Form 
        /// </summary>
        /// <param name="NodeNo">NodeNo </param>
        public FrmNodes(string NodeNo)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(FrmNodeAttr.FK_Node, NodeNo);
            qo.addOrderBy(FrmNodeAttr.Idx);
            qo.DoQuery();
        }
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new FrmNode();
            }
        }
        /// <summary>
        ///  Node Form s
        /// </summary>
        /// <param name="sts"> Node Form </param>
        /// <returns></returns>
        public Nodes GetHisNodes(Nodes sts)
        {
            Nodes nds = new Nodes();
            Nodes tmp = new Nodes();
            foreach (Node st in sts)
            {
                tmp = this.GetHisNodes(st.No);
                foreach (Node nd in tmp)
                {
                    if (nds.Contains(nd))
                        continue;
                    nds.AddEntity(nd);
                }
            }
            return nds;
        }
        /// <summary>
        ///  Node Form 
        /// </summary>
        /// <param name="NodeNo"> Work node number </param>
        /// <returns> Node s</returns>
        public Nodes GetHisNodes(string NodeNo)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(FrmNodeAttr.FK_Node, NodeNo);
            qo.DoQuery();

            Nodes ens = new Nodes();
            foreach (FrmNode en in this)
            {
                ens.AddEntity(new Node(en.FK_Frm));
            }
            return ens;
        }
        /// <summary>
        ///  Turn this collection of nodes Nodes
        /// </summary>
        /// <param name="nodeID"> This node ID</param>
        /// <returns> Turn this collection of nodes Nodes (FromNodes)</returns> 
        public Nodes GetHisNodes(int nodeID)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(FrmNodeAttr.FK_Frm, nodeID);
            qo.DoQuery();

            Nodes ens = new Nodes();
            foreach (FrmNode en in this)
            {
                ens.AddEntity(new Node(en.FK_Node));
            }
            return ens;
        }
    }
}
