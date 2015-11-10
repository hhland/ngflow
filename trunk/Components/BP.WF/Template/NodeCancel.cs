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
    ///  Revocable node properties 	  
    /// </summary>
    public class NodeCancelAttr
    {
        /// <summary>
        ///  Node 
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        ///  Revocation to 
        /// </summary>
        public const string CancelTo = "CancelTo";
    }
    /// <summary>
    ///  Revocable node 
    ///  Undo to node consists of two parts .	 
    ///  Records from one node to the other nodes of the plurality of .
    ///  Also recorded to the other nodes of this node .
    /// </summary>
    public class NodeCancel : EntityMM
    {
        #region  Basic properties 
        /// <summary>
        /// Revocation to 
        /// </summary>
        public int CancelTo
        {
            get
            {
                return this.GetValIntByKey(NodeCancelAttr.CancelTo);
            }
            set
            {
                this.SetValByKey(NodeCancelAttr.CancelTo, value);
            }
        }
        /// <summary>
        ///  Workflow 
        /// </summary>
        public int FK_Node
        {
            get
            {
                return this.GetValIntByKey(NodeCancelAttr.FK_Node);
            }
            set
            {
                this.SetValByKey(NodeCancelAttr.FK_Node, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Revocable node 
        /// </summary>
        public NodeCancel() { }
        /// <summary>
        ///  Override the base class methods 
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("WF_NodeCancel");
                map.EnDesc = " Revocable node ";

                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;

                map.AddTBIntPK(NodeCancelAttr.FK_Node, 0, " Node ", true, true);
                map.AddTBIntPK(NodeCancelAttr.CancelTo, 0, " Revocation to ", true, true);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    ///  Revocable node 
    /// </summary>
    public class NodeCancels : EntitiesMM
    {
        /// <summary>
        ///  His withdrawal to 
        /// </summary>
        public Nodes HisNodes
        {
            get
            {
                Nodes ens = new Nodes();
                foreach (NodeCancel ns in this)
                {
                    ens.AddEntity(new Node(ns.CancelTo));
                }
                return ens;
            }
        }
        /// <summary>
        ///  Revocable node 
        /// </summary>
        public NodeCancels() { }
        /// <summary>
        ///  Revocable node 
        /// </summary>
        /// <param name="NodeID"> Node ID</param>
        public NodeCancels(int NodeID)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(NodeCancelAttr.FK_Node, NodeID);
            qo.DoQuery();
        }
        /// <summary>
        ///  Revocable node 
        /// </summary>
        /// <param name="NodeNo">NodeNo </param>
        public NodeCancels(string NodeNo)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(NodeCancelAttr.CancelTo, NodeNo);
            qo.DoQuery();
        }
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new NodeCancel();
            }
        }
        /// <summary>
        ///  Revocable node s
        /// </summary>
        /// <param name="sts"> Revocable node </param>
        /// <Cancels></Cancels>
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
        ///  Revocable node 
        /// </summary>
        /// <param name="NodeNo"> Revocation of the number </param>
        /// <Cancels> Node s</Cancels>
        public Nodes GetHisNodes(string NodeNo)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(NodeCancelAttr.CancelTo, NodeNo);
            qo.DoQuery();

            Nodes ens = new Nodes();
            foreach (NodeCancel en in this)
            {
                ens.AddEntity(new Node(en.FK_Node));
            }
            return ens;
        }
        /// <summary>
        ///  Turn this collection of nodes Nodes
        /// </summary>
        /// <param name="nodeID"> This node ID</param>
        /// <Cancels> Turn this collection of nodes Nodes (FromNodes)</Cancels> 
        public Nodes GetHisNodes(int nodeID)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(NodeCancelAttr.FK_Node, nodeID);
            qo.DoQuery();

            Nodes ens = new Nodes();
            foreach (NodeCancel en in this)
            {
                ens.AddEntity(new Node(en.CancelTo));
            }
            return ens;
        }
    }
}
