using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.WF.Port;

namespace BP.WF.Template
{
	/// <summary>
	///  Department attribute node 	  
	/// </summary>
	public class NodeDeptAttr
	{
		/// <summary>
		///  Node 
		/// </summary>
		public const string FK_Node="FK_Node";
		/// <summary>
		///  Departments 
		/// </summary>
		public const string FK_Dept="FK_Dept";
	}
	/// <summary>
	///  Node department 
	///  Departments node consists of two parts .	 
	///  Records from one node to the other nodes of the plurality of .
	///  Also recorded to the other nodes of this node .
	/// </summary>
	public class NodeDept :EntityMM
	{
		#region  Basic properties 
		/// <summary>
		/// Node 
		/// </summary>
		public int  FK_Node
		{
			get
			{
				return this.GetValIntByKey(NodeDeptAttr.FK_Node);
			}
			set
			{
				this.SetValByKey(NodeDeptAttr.FK_Node,value);
			}
		}
		/// <summary>
		///  Departments 
		/// </summary>
		public string FK_Dept
		{
			get
			{
				return this.GetValStringByKey(NodeDeptAttr.FK_Dept);
			}
			set
			{
				this.SetValByKey(NodeDeptAttr.FK_Dept,value);
			}
		}
		#endregion 

		#region  Constructor 
		/// <summary>
		///  Node department 
		/// </summary>
		public NodeDept(){}
		/// <summary>
		///  Override the base class methods 
		/// </summary>
		public override Map EnMap
		{
			get
			{
				if (this._enMap!=null) 
					return this._enMap;
				
				Map map = new Map("WF_NodeDept");				 
				map.EnDesc=" Node department ";

				map.DepositaryOfEntity=Depositary.None;
				map.DepositaryOfMap=Depositary.Application;

				map.AddDDLEntitiesPK(NodeDeptAttr.FK_Node,0,DataType.AppInt," Node ",new Nodes(),
                    NodeAttr.NodeID,NodeAttr.Name,true);
				map.AddDDLEntitiesPK( NodeDeptAttr.FK_Dept,null," Department ",new Depts(),true);

				this._enMap=map;
				 
				return this._enMap;
			}
		}
		#endregion

	}
	/// <summary>
	///  Node department 
	/// </summary>
    public class NodeDepts : EntitiesMM
    {
        /// <summary>
        ///  His work department 
        /// </summary>
        public Stations HisStations
        {
            get
            {
                Stations ens = new Stations();
                foreach (NodeDept ns in this)
                {
                    ens.AddEntity(new Station(ns.FK_Dept));
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
                foreach (NodeDept ns in this)
                {
                    ens.AddEntity(new Node(ns.FK_Node));
                }
                return ens;

            }
        }
        /// <summary>
        ///  Node department 
        /// </summary>
        public NodeDepts() { }
        /// <summary>
        ///  Node department 
        /// </summary>
        /// <param name="NodeID"> Node ID</param>
        public NodeDepts(int NodeID)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(NodeDeptAttr.FK_Node, NodeID);
            qo.DoQuery();
        }
        /// <summary>
        ///  Node department 
        /// </summary>
        /// <param name="StationNo">StationNo </param>
        public NodeDepts(string StationNo)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(NodeDeptAttr.FK_Dept, StationNo);
            qo.DoQuery();
        }
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new NodeDept();
            }
        }
        /// <summary>
        ///  Take it to the collection of nodes can access to a work department s
        /// </summary>
        /// <param name="sts"> Collection department </param>
        /// <returns></returns>
        public Nodes GetHisNodes(Stations sts)
        {
            Nodes nds = new Nodes();
            Nodes tmp = new Nodes();
            foreach (Station st in sts)
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
        ///  Take a staff to have access to the node .
        /// </summary>
        /// <param name="empId"> Staff ID</param>
        /// <returns></returns>
        public Nodes GetHisNodes_del(string empId)
        {
            Emp em = new Emp(empId);
            return this.GetHisNodes(em.HisStations);
        }
        /// <summary>
        ///  Nodes corresponding departments 
        /// </summary>
        /// <param name="stationNo"> Work department number </param>
        /// <returns> Node s</returns>
        public Nodes GetHisNodes(string stationNo)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(NodeDeptAttr.FK_Dept, stationNo);
            qo.DoQuery();

            Nodes ens = new Nodes();
            foreach (NodeDept en in this)
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
        public Stations GetHisStations(int nodeID)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(NodeDeptAttr.FK_Node, nodeID);
            qo.DoQuery();

            Stations ens = new Stations();
            foreach (NodeDept en in this)
            {
                ens.AddEntity(new Station(en.FK_Dept));
            }
            return ens;
        }

    }
}
