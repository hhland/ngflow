using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.En;
using BP.WF.Port;

namespace BP.WF.Template
{
	/// <summary>
	///  Cc department property 	  
	/// </summary>
	public class CCDeptAttr
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
	///  Cc department 
	///  Departments node consists of two parts .	 
	///  Records from one node to the other nodes of the plurality of .
	///  Also recorded to the other nodes of this node .
	/// </summary>
	public class CCDept :EntityMM
	{
		#region  Basic properties 
		/// <summary>
		/// Node 
		/// </summary>
		public int  FK_Node
		{
			get
			{
				return this.GetValIntByKey(CCDeptAttr.FK_Node);
			}
			set
			{
				this.SetValByKey(CCDeptAttr.FK_Node,value);
			}
		}
		/// <summary>
		///  Departments 
		/// </summary>
		public string FK_Dept
		{
			get
			{
				return this.GetValStringByKey(CCDeptAttr.FK_Dept);
			}
			set
			{
				this.SetValByKey(CCDeptAttr.FK_Dept,value);
			}
		}
		#endregion 

		#region  Constructor 
		/// <summary>
		///  Cc department 
		/// </summary>
		public CCDept(){}
		/// <summary>
		///  Override the base class methods 
		/// </summary>
		public override Map EnMap
		{
			get
			{
				if (this._enMap!=null) 
					return this._enMap;
				
				Map map = new Map("WF_CCDept");				 
				map.EnDesc=" Cc department ";

				map.DepositaryOfEntity=Depositary.None;
				map.DepositaryOfMap=Depositary.Application;
				map.AddDDLEntitiesPK(CCDeptAttr.FK_Node,0,DataType.AppInt," Node ",new Nodes(),NodeAttr.NodeID,NodeAttr.Name,true);
				map.AddDDLEntitiesPK( CCDeptAttr.FK_Dept,null," Department ",new Depts(),true);
				this._enMap=map;
				return this._enMap;
			}
		}
		#endregion
	}
	/// <summary>
	///  Cc department 
	/// </summary>
    public class CCDepts : EntitiesMM
    {
        /// <summary>
        ///  His work department 
        /// </summary>
        public Stations HisStations
        {
            get
            {
                Stations ens = new Stations();
                foreach (CCDept ns in this)
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
                foreach (CCDept ns in this)
                {
                    ens.AddEntity(new Node(ns.FK_Node));
                }
                return ens;
            }
        }
        /// <summary>
        ///  Cc department 
        /// </summary>
        public CCDepts() { }
        /// <summary>
        ///  Cc department 
        /// </summary>
        /// <param name="NodeID"> Node ID</param>
        public CCDepts(int NodeID)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(CCDeptAttr.FK_Node, NodeID);
            qo.DoQuery();
        }
        /// <summary>
        ///  Cc department 
        /// </summary>
        /// <param name="StationNo">StationNo </param>
        public CCDepts(string StationNo)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(CCDeptAttr.FK_Dept, StationNo);
            qo.DoQuery();
        }
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new CCDept();
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
            qo.AddWhere(CCDeptAttr.FK_Dept, stationNo);
            qo.DoQuery();

            Nodes ens = new Nodes();
            foreach (CCDept en in this)
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
            qo.AddWhere(CCDeptAttr.FK_Node, nodeID);
            qo.DoQuery();

            Stations ens = new Stations();
            foreach (CCDept en in this)
            {
                ens.AddEntity(new Station(en.FK_Dept));
            }
            return ens;
        }
    }
}
