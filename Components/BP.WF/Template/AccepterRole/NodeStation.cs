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
    ///  Node jobs property 	  
    /// </summary>
    public class NodeStationAttr
    {
        /// <summary>
        ///  Node 
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        ///  Jobs 
        /// </summary>
        public const string FK_Station = "FK_Station";
    }
    /// <summary>
    ///  Node jobs 
    ///  Jobs node consists of two parts .	 
    ///  Records from one node to the other nodes of the plurality of .
    ///  Also recorded to the other nodes of this node .
    /// </summary>
    public class NodeStation : EntityMM
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
                uac.OpenAll();
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
                return this.GetValIntByKey(NodeStationAttr.FK_Node);
            }
            set
            {
                this.SetValByKey(NodeStationAttr.FK_Node, value);
            }
        }
        public string FK_StationT
        {
            get
            {
                return this.GetValRefTextByKey(NodeStationAttr.FK_Station);
            }
        }
        /// <summary>
        ///  Jobs 
        /// </summary>
        public string FK_Station
        {
            get
            {
                return this.GetValStringByKey(NodeStationAttr.FK_Station);
            }
            set
            {
                this.SetValByKey(NodeStationAttr.FK_Station, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Node jobs 
        /// </summary>
        public NodeStation() { }
        /// <summary>
        ///  Override the base class methods 
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("WF_NodeStation");

                map.EnDesc = " Node positions ";
                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.AddTBIntPK(NodeStationAttr.FK_Node, 0," Node ", false,false);

                if (Glo.OSModel == OSModel.WorkFlow)
                {
                    map.AddDDLEntitiesPK(NodeStationAttr.FK_Station, null, " Jobs ",
                        new BP.Port.Stations(), true);
                }
                else
                {
                    map.AddDDLEntitiesPK(NodeStationAttr.FK_Station, null, " Jobs ",
                       new BP.GPM.Stations(), true);
                }
                this._enMap = map;

                return this._enMap;
            }
        }

        /// <summary>
        ///  Node status changes , Delete the node memory reception staff .
        /// </summary>
        /// <returns></returns>
        protected override bool beforeInsert()
        {
            RememberMe remeberMe = new RememberMe();
            remeberMe.Delete(RememberMeAttr.FK_Node, this.FK_Node);
            return base.beforeInsert();
        }
        #endregion

    }
    /// <summary>
    ///  Node jobs 
    /// </summary>
    public class NodeStations : EntitiesMM
    {
        /// <summary>
        ///  His job 
        /// </summary>
        public Stations HisStations
        {
            get
            {
                Stations ens = new Stations();
                foreach (NodeStation ns in this)
                {
                    ens.AddEntity(new Station(ns.FK_Station));
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
                foreach (NodeStation ns in this)
                {
                    ens.AddEntity(new Node(ns.FK_Node));
                }
                return ens;

            }
        }
        /// <summary>
        ///  Node jobs 
        /// </summary>
        public NodeStations() { }
        /// <summary>
        ///  Node jobs 
        /// </summary>
        /// <param name="nodeID"> Node ID</param>
        public NodeStations(int nodeID)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(NodeStationAttr.FK_Node, nodeID);
            qo.DoQuery();
        }
        /// <summary>
        ///  Node jobs 
        /// </summary>
        /// <param name="StationNo">StationNo </param>
        public NodeStations(string StationNo)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(NodeStationAttr.FK_Station, StationNo);
            qo.DoQuery();
        }
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new NodeStation();
            }
        }
        /// <summary>
        ///  Take a job to be able to have access to a collection of nodes s
        /// </summary>
        /// <param name="sts"> Work set </param>
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
        ///  Jobs corresponding node 
        /// </summary>
        /// <param name="stationNo"> Number of jobs </param>
        /// <returns> Node s</returns>
        public Nodes GetHisNodes(string stationNo)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(NodeStationAttr.FK_Station, stationNo);
            qo.DoQuery();

            Nodes ens = new Nodes();
            foreach (NodeStation en in this)
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
            qo.AddWhere(NodeStationAttr.FK_Node, nodeID);
            qo.DoQuery();

            Stations ens = new Stations();
            foreach (NodeStation en in this)
            {
                ens.AddEntity(new Station(en.FK_Station));
            }
            return ens;
        }
    }
}
