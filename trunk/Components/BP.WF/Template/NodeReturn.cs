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
    ///  Node attributes returnable 	  
    /// </summary>
    public class NodeReturnAttr
    {
        /// <summary>
        ///  Node 
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        ///  Return to 
        /// </summary>
        public const string ReturnTo = "ReturnTo";
        /// <summary>
        ///  Intermediate point 
        /// </summary>
        public const string Dots = "Dots";
    }
    /// <summary>
    ///  Returnable node 
    ///  Node consists of two parts back to .	 
    ///  Records from one node to the other nodes of the plurality of .
    ///  Also recorded to the other nodes of this node .
    /// </summary>
    public class NodeReturn : EntityMM
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
        /// Return to 
        /// </summary>
        public int ReturnTo
        {
            get
            {
                return this.GetValIntByKey(NodeReturnAttr.ReturnTo);
            }
            set
            {
                this.SetValByKey(NodeReturnAttr.ReturnTo, value);
            }
        }
        /// <summary>
        ///  Workflow 
        /// </summary>
        public int FK_Node
        {
            get
            {
                return this.GetValIntByKey(NodeReturnAttr.FK_Node);
            }
            set
            {
                this.SetValByKey(NodeReturnAttr.FK_Node, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Returnable node 
        /// </summary>
        public NodeReturn() { }
        /// <summary>
        ///  Override the base class methods 
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("WF_NodeReturn");
                map.EnDesc = " Returnable node ";

                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;

                map.AddTBIntPK(NodeReturnAttr.FK_Node, 0, " Node ", true, true);
                map.AddTBIntPK(NodeReturnAttr.ReturnTo, 0, " Return to ", true, true);
                map.AddTBString(NodeReturnAttr.Dots, null, " Track information ", true, true,0,300,0,false);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    ///  Returnable node 
    /// </summary>
    public class NodeReturns : EntitiesMM
    {
        /// <summary>
        ///  His return to 
        /// </summary>
        public Nodes HisNodes
        {
            get
            {
                Nodes ens = new Nodes();
                foreach (NodeReturn ns in this)
                {
                    ens.AddEntity(new Node(ns.ReturnTo));
                }
                return ens;
            }
        }
        /// <summary>
        ///  Returnable node 
        /// </summary>
        public NodeReturns() { }
        /// <summary>
        ///  Returnable node 
        /// </summary>
        /// <param name="NodeID"> Node ID</param>
        public NodeReturns(int NodeID)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(NodeReturnAttr.FK_Node, NodeID);
            qo.DoQuery();
        }
        /// <summary>
        ///  Returnable node 
        /// </summary>
        /// <param name="NodeNo">NodeNo </param>
        public NodeReturns(string NodeNo)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(NodeReturnAttr.ReturnTo, NodeNo);
            qo.DoQuery();
        }
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new NodeReturn();
            }
        }
        /// <summary>
        ///  Returnable node s
        /// </summary>
        /// <param name="sts"> Returnable node </param>
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
        ///  Returnable node 
        /// </summary>
        /// <param name="NodeNo"> Return to No. </param>
        /// <returns> Node s</returns>
        public Nodes GetHisNodes(string NodeNo)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(NodeReturnAttr.ReturnTo, NodeNo);
            qo.DoQuery();

            Nodes ens = new Nodes();
            foreach (NodeReturn en in this)
            {
                ens.AddEntity(new Node(en.FK_Node));
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
            qo.AddWhere(NodeReturnAttr.FK_Node, nodeID);
            qo.DoQuery();

            Nodes ens = new Nodes();
            foreach (NodeReturn en in this)
            {
                ens.AddEntity(new Node(en.ReturnTo));
            }
            return ens;
        }
    }
}
