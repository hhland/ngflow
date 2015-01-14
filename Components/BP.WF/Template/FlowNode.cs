using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.En;
using BP.Port;
//using BP.ZHZS.Base;

namespace BP.WF
{
    /// <summary>
    ///  Property   
    /// </summary>
    public class FlowNodeAttr
    {
        /// <summary>
        ///  Node 
        /// </summary>
        public const string FK_Flow = "FK_Flow";
        /// <summary>
        ///  Work node 
        /// </summary>
        public const string FK_Node = "FK_Node";
    }
    /// <summary>
    ///  Process Node 
    /// </summary>
    public class FlowNode : EntityMM
    {
        #region  Basic properties 
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
        /// <summary>
        /// Node 
        /// </summary>
        public int FK_Node
        {
            get
            {
                return this.GetValIntByKey(FlowNodeAttr.FK_Node);
            }
            set
            {
                this.SetValByKey(FlowNodeAttr.FK_Node, value);
            }
        }
        /// <summary>
        ///  Workflow 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.GetValStringByKey(FlowNodeAttr.FK_Flow);
            }
            set
            {
                this.SetValByKey(FlowNodeAttr.FK_Flow, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Process job attributes 
        /// </summary>
        public FlowNode() { }
        /// <summary>
        ///  Override the base class methods 
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("WF_FlowNode");
                map.EnDesc = " Cc process node ";

                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;

                map.AddTBStringPK(FlowNodeAttr.FK_Flow, null, " Process ID ", true, true, 1, 20, 20);
                map.AddTBStringPK(FlowNodeAttr.FK_Node, null, " Node ", true, true, 1, 20, 20);

                //      map.AddDDLEntitiesPK(FlowNodeAttr.FK_Flow, null, "FK_Flow", new Flows(), true);
                //     map.AddDDLEntitiesPK(FlowNodeAttr.FK_Node, null, " Work node ", new Nodes(), true);
                this._enMap = map;
                

                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    ///  Property 
    /// </summary>
    public class FlowNodes : EntitiesMM
    {
        /// <summary>
        ///  His work node 
        /// </summary>
        public Nodes HisNodes
        {
            get
            {
                Nodes ens = new Nodes();
                foreach (FlowNode ns in this)
                {
                    ens.AddEntity(new Node(ns.FK_Node));
                }
                return ens;
            }
        }
        /// <summary>
        ///  Cc process node 
        /// </summary>
        public FlowNodes() { }
        /// <summary>
        ///  Cc process node 
        /// </summary>
        /// <param name="NodeID"> Node ID</param>
        public FlowNodes(int NodeID)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(FlowNodeAttr.FK_Flow, NodeID);
            qo.DoQuery();
        }
        /// <summary>
        ///  Cc process node 
        /// </summary>
        /// <param name="NodeNo">NodeNo </param>
        public FlowNodes(string NodeNo)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(FlowNodeAttr.FK_Node, NodeNo);
            qo.DoQuery();
        }
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new FlowNode();
            }
        }
        /// <summary>
        ///  Cc process node s
        /// </summary>
        /// <param name="sts"> Cc process node </param>
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
        ///  Cc process node 
        /// </summary>
        /// <param name="NodeNo"> Work node number </param>
        /// <returns> Node s</returns>
        public Nodes GetHisNodes(string NodeNo)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(FlowNodeAttr.FK_Node, NodeNo);
            qo.DoQuery();

            Nodes ens = new Nodes();
            foreach (FlowNode en in this)
            {
                ens.AddEntity(new Node(en.FK_Flow));
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
            qo.AddWhere(FlowNodeAttr.FK_Flow, nodeID);
            qo.DoQuery();

            Nodes ens = new Nodes();
            foreach (FlowNode en in this)
            {
                ens.AddEntity(new Node(en.FK_Node));
            }
            return ens;
        }
    }
}
