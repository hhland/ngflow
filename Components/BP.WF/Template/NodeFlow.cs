using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.En;
using BP.WF.Port;

namespace BP.WF
{
	/// <summary>
	///  Call sub-process node attributes 
	/// </summary>
    public class NodeFlowAttr
    {
        /// <summary>
        ///  Node 
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        ///  Call the sub-processes 
        /// </summary>
        public const string FK_Flow = "FK_Flow";
    }
	/// <summary>
	///  Call sub-process node 
	///  Call sub-process node consists of two parts .	 
	///  Recording the other of the plurality of nodes from a node calls .
	///  Also recorded other nodes call this node .
	/// </summary>
    public class NodeFlow : EntityMM
    {
        #region  Basic properties 
        /// <summary>
        /// Node 
        /// </summary>
        public string FK_Node
        {
            get
            {
                return this.GetValStringByKey(NodeFlowAttr.FK_Node);
            }
            set
            {
                this.SetValByKey(NodeFlowAttr.FK_Node, value);
            }
        }
        /// <summary>
        ///  Call the sub-processes 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.GetValStringByKey(NodeFlowAttr.FK_Flow);
            }
            set
            {
                this.SetValByKey(NodeFlowAttr.FK_Flow, value);
            }
        }
        public string FK_FlowT
        {
            get
            {
                return this.GetValRefTextByKey(NodeFlowAttr.FK_Flow);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Call sub-process node 
        /// </summary>
        public NodeFlow() { }
        /// <summary>
        ///  Override the base class methods 
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("WF_NodeFlow");
                map.EnDesc = " Call sub-process node ";

                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;

                map.AddTBIntPK(NodeFlowAttr.FK_Node, 0, " Node ", true, true);
                // map.AddDDLEntitiesPK(NodeFlowAttr.FK_Node, null, " Node ", new NodeSheets(), true);
                map.AddDDLEntitiesPK(NodeFlowAttr.FK_Flow, null, " Subprocess ", new Flows(), true);

                //map.AddSearchAttr(NodeFlowAttr.FK_Node);
                map.AddSearchAttr(NodeFlowAttr.FK_Flow);
                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
	/// <summary>
	///  Call sub-process node 
	/// </summary>
    public class NodeFlows : EntitiesMM
    {
        /// <summary>
        ///  His call to sub-processes 
        /// </summary>
        public Emps HisEmps
        {
            get
            {
                Emps ens = new Emps();
                foreach (NodeFlow ns in this)
                {
                    ens.AddEntity(new Emp(ns.FK_Flow));
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
                foreach (NodeFlow ns in this)
                {
                    ens.AddEntity(new Node(ns.FK_Node));
                }
                return ens;

            }
        }
        /// <summary>
        ///  Call sub-process node 
        /// </summary>
        public NodeFlows() { }
        /// <summary>
        ///  Call sub-process node 
        /// </summary>
        /// <param name="NodeID"> Node ID</param>
        public NodeFlows(int NodeID)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(NodeFlowAttr.FK_Node, NodeID);
            qo.DoQuery();
        }
        /// <summary>
        ///  Call sub-process node 
        /// </summary>
        /// <param name="EmpNo">EmpNo </param>
        public NodeFlows(string EmpNo)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(NodeFlowAttr.FK_Flow, EmpNo);
            qo.DoQuery();
        }
        /// <summary>
        ///  It was invoked  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new NodeFlow();
            }
        }
        /// <summary>
        ///  Take calls a call nodes can access a collection of sub-process calls s
        /// </summary>
        /// <param name="sts"> A collection of sub-process calls </param>
        /// <returns></returns>
        public Nodes GetHisNodes(Emps sts)
        {
            Nodes nds = new Nodes();
            Nodes tmp = new Nodes();
            foreach (Emp st in sts)
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
        ///  Call the corresponding sub-process node 
        /// </summary>
        /// <param name="EmpNo"> Call sub-process ID </param>
        /// <returns> Node s</returns>
        public Nodes GetHisNodes(string EmpNo)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(NodeFlowAttr.FK_Flow, EmpNo);
            qo.DoQuery();

            Nodes ens = new Nodes();
            foreach (NodeFlow en in this)
            {
                ens.AddEntity(new Node(en.FK_Node));
            }
            return ens;
        }
        /// <summary>
        ///  Turn this collection of nodes  Nodes
        /// </summary>
        /// <param name="nodeID"> This node ID</param>
        /// <returns> Turn this collection of nodes Nodes (FromNodes)</returns> 
        public Emps GetHisEmps(int nodeID)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(NodeFlowAttr.FK_Node, nodeID);
            qo.DoQuery();

            Emps ens = new Emps();
            foreach (NodeFlow en in this)
            {
                ens.AddEntity(new Emp(en.FK_Flow));
            }
            return ens;
        }
    }
}
