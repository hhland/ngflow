using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.En;
using BP.WF.Port;
//using BP.ZHZS.Base;

namespace BP.WF.Template
{
	/// <summary>
	///  Attribute node to staff 
	/// </summary>
	public class NodeEmpAttr
	{
		/// <summary>
		///  Node 
		/// </summary>
		public const string FK_Node="FK_Node";
		/// <summary>
		///  To staff 
		/// </summary>
		public const string FK_Emp="FK_Emp";
	}
	/// <summary>
	///  Node to the staff 
	///  Node to the staff consists of two parts .	 
	///  Records from one node to the other nodes of the plurality of .
	///  Also recorded to the other nodes of this node .
	/// </summary>
	public class NodeEmp :EntityMM
	{
		#region  Basic properties 
		/// <summary>
		/// Node 
		/// </summary>
		public int  FK_Node
		{
			get
			{
				return this.GetValIntByKey(NodeEmpAttr.FK_Node);
			}
			set
			{
				this.SetValByKey(NodeEmpAttr.FK_Node,value);
			}
		}
		/// <summary>
		///  To staff 
		/// </summary>
		public string FK_Emp
		{
			get
			{
				return this.GetValStringByKey(NodeEmpAttr.FK_Emp);
			}
			set
			{
				this.SetValByKey(NodeEmpAttr.FK_Emp,value);
			}
		}
        public string FK_EmpT
        {
            get
            {
                return this.GetValRefTextByKey(NodeEmpAttr.FK_Emp);
            }
        }
		#endregion 

		#region  Constructor 
		/// <summary>
		///  Node to the staff 
		/// </summary>
		public NodeEmp(){}
		/// <summary>
		///  Override the base class methods 
		/// </summary>
		public override Map EnMap
		{
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("WF_NodeEmp");
                map.EnDesc = " Node staff ";

                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;

                map.AddTBIntPK(NodeEmpAttr.FK_Node,0,"Node",true,true );
                map.AddDDLEntitiesPK(NodeEmpAttr.FK_Emp, null, " To staff ", new Emps(), true);

                this._enMap = map;
                return this._enMap;
            }
		}
		#endregion
	}
	/// <summary>
	///  Node to the staff 
	/// </summary>
    public class NodeEmps : EntitiesMM
    {
        /// <summary>
        ///  To his staff 
        /// </summary>
        public Emps HisEmps
        {
            get
            {
                Emps ens = new Emps();
                foreach (NodeEmp ns in this)
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
                foreach (NodeEmp ns in this)
                {
                    ens.AddEntity(new Node(ns.FK_Node));
                }
                return ens;

            }
        }
        /// <summary>
        ///  Node to the staff 
        /// </summary>
        public NodeEmps() { }
        /// <summary>
        ///  Node to the staff 
        /// </summary>
        /// <param name="NodeID"> Node ID</param>
        public NodeEmps(int NodeID)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(NodeEmpAttr.FK_Node, NodeID);
            qo.DoQuery();
        }
        /// <summary>
        ///  Node to the staff 
        /// </summary>
        /// <param name="EmpNo">EmpNo </param>
        public NodeEmps(string EmpNo)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(NodeEmpAttr.FK_Emp, EmpNo);
            qo.DoQuery();
        }
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new NodeEmp();
            }
        }
        /// <summary>
        ///  To get into a person can gain access to a collection of nodes s
        /// </summary>
        /// <param name="sts"> To staff the collection </param>
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
        ///  Nodes corresponding to the personnel 
        /// </summary>
        /// <param name="EmpNo"> To staff numbers </param>
        /// <returns> Node s</returns>
        public Nodes GetHisNodes(string EmpNo)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(NodeEmpAttr.FK_Emp, EmpNo);
            qo.DoQuery();

            Nodes ens = new Nodes();
            foreach (NodeEmp en in this)
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
            qo.AddWhere(NodeEmpAttr.FK_Node, nodeID);
            qo.DoQuery();

            Emps ens = new Emps();
            foreach (NodeEmp en in this)
            {
                ens.AddEntity(new Emp(en.FK_Emp));
            }
            return ens;
        }
    }
}
