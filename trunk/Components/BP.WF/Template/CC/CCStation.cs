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
    ///  Cc to post property 	  
    /// </summary>
    public class CCStationAttr
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
    ///  Cc to post 
    ///  Jobs node consists of two parts .	 
    ///  Records from one node to the other nodes of the plurality of .
    ///  Also recorded to the other nodes of this node .
    /// </summary>
    public class CCStation : EntityMM
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
                return this.GetValIntByKey(CCStationAttr.FK_Node);
            }
            set
            {
                this.SetValByKey(CCStationAttr.FK_Node, value);
            }
        }
        /// <summary>
        ///  Job Title 
        /// </summary>
        public string FK_StationT
        {
            get
            {
                return this.GetValRefTextByKey(CCStationAttr.FK_Station);
            }
        }
        /// <summary>
        ///  Jobs 
        /// </summary>
        public string FK_Station
        {
            get
            {
                return this.GetValStringByKey(CCStationAttr.FK_Station);
            }
            set
            {
                this.SetValByKey(CCStationAttr.FK_Station, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Cc to post 
        /// </summary>
        public CCStation() { }
        /// <summary>
        ///  Override the base class methods 
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("WF_CCStation");
                map.EnDesc = " Cc jobs ";

                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;

                map.AddDDLEntitiesPK(CCStationAttr.FK_Node, 0, DataType.AppInt, " Node ", new Nodes(), NodeAttr.NodeID, NodeAttr.Name, true);
                map.AddDDLEntitiesPK(CCStationAttr.FK_Station, null, " Jobs ", new Stations(), true);
              
                this._enMap = map;

                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    ///  Cc to post 
    /// </summary>
    public class CCStations : EntitiesMM
    {
        /// <summary>
        ///  His job 
        /// </summary>
        public Stations HisStations
        {
            get
            {
                Stations ens = new Stations();
                foreach (CCStation ns in this)
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
                foreach (CCStation ns in this)
                {
                    ens.AddEntity(new Node(ns.FK_Node));
                }
                return ens;

            }
        }
        /// <summary>
        ///  Cc to post 
        /// </summary>
        public CCStations() { }
        /// <summary>
        ///  Cc to post 
        /// </summary>
        /// <param name="nodeID"> Node ID</param>
        public CCStations(int nodeID)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(CCStationAttr.FK_Node, nodeID);
            qo.DoQuery();
        }
        /// <summary>
        ///  Cc to post 
        /// </summary>
        /// <param name="StationNo">StationNo </param>
        public CCStations(string StationNo)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(CCStationAttr.FK_Station, StationNo);
            qo.DoQuery();
        }
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new CCStation();
            }
        }
        /// <summary>
        ///  Jobs corresponding node 
        /// </summary>
        /// <param name="stationNo"> Number of jobs </param>
        /// <returns> Node s</returns>
        public Nodes GetHisNodes(string stationNo)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(CCStationAttr.FK_Station, stationNo);
            qo.DoQuery();

            Nodes ens = new Nodes();
            foreach (CCStation en in this)
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
            qo.AddWhere(CCStationAttr.FK_Node, nodeID);
            qo.DoQuery();

            Stations ens = new Stations();
            foreach (CCStation en in this)
            {
                ens.AddEntity(new Station(en.FK_Station));
            }
            return ens;
        }
    }
}
