using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.Port;

namespace BP.WF
{
    /// <summary>
    ///  Process Status Properties 	  
    /// </summary>
    public class FlowEmpAttr
    {
        /// <summary>
        ///  Process 
        /// </summary>
        public const string FK_Flow = "FK_Flow";
        /// <summary>
        ///  Staff 
        /// </summary>
        public const string FK_Emp = "FK_Emp";
    }
    /// <summary>
    ///  Process job attributes 
    ///  Personnel process consists of two parts .	 
    ///  Records from one process to another multiple processes .
    ///  Also recorded other processes to this process .
    /// </summary>
    public class FlowEmp : EntityMM
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
        /// Process 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.GetValStringByKey(FlowEmpAttr.FK_Flow);
            }
            set
            {
                this.SetValByKey(FlowEmpAttr.FK_Flow, value);
            }
        }
        /// <summary>
        ///  Staff 
        /// </summary>
        public string FK_Emp
        {
            get
            {
                return this.GetValStringByKey(FlowEmpAttr.FK_Emp);
            }
            set
            {
                this.SetValByKey(FlowEmpAttr.FK_Emp, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Process job attributes 
        /// </summary>
        public FlowEmp() { }
        /// <summary>
        ///  Override the base class methods 
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("WF_FlowEmp");
                map.EnDesc = " Process job attribute information ";

                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;

                map.AddDDLEntitiesPK(FlowEmpAttr.FK_Flow, null, "FK_Flow", new Flows(), true);
                map.AddDDLEntitiesPK(FlowEmpAttr.FK_Emp, null, " Staff ", new Emps(), true);
                this._enMap = map;

                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    ///  Process job attributes 
    /// </summary>
    public class FlowEmps : EntitiesMM
    {
        /// <summary>
        ///  His staff 
        /// </summary>
        public Stations HisStations
        {
            get
            {
                Stations ens = new Stations();
                foreach (FlowEmp ns in this)
                {
                    ens.AddEntity(new Station(ns.FK_Emp));
                }
                return ens;
            }
        }
        /// <summary>
        ///  His workflow 
        /// </summary>
        public Nodes HisNodes
        {
            get
            {
                Nodes ens = new Nodes();
                foreach (FlowEmp ns in this)
                {
                    ens.AddEntity(new Node(ns.FK_Flow));
                }
                return ens;

            }
        }
        /// <summary>
        ///  Process job attributes 
        /// </summary>
        public FlowEmps() { }
        /// <summary>
        ///  Process job attributes 
        /// </summary>
        /// <param name="NodeID"> Process ID</param>
        public FlowEmps(int NodeID)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(FlowEmpAttr.FK_Flow, NodeID);
            qo.DoQuery();
        }
        /// <summary>
        ///  Process job attributes 
        /// </summary>
        /// <param name="StationNo">StationNo </param>
        public FlowEmps(string StationNo)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(FlowEmpAttr.FK_Emp, StationNo);
            qo.DoQuery();
        }
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new FlowEmp();
            }
        }
        /// <summary>
        ///  Take a person can gain access to the collection process s
        /// </summary>
        /// <param name="sts"> Staff set </param>
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
        ///  Take a staff to have access to the process .
        /// </summary>
        /// <param name="empId"> Staff ID</param>
        /// <returns></returns>
        public Nodes GetHisNodes_del(string empId)
        {
            Emp em = new Emp(empId);
            return this.GetHisNodes(em.HisStations);
        }
        /// <summary>
        ///  Personnel corresponding flow 
        /// </summary>
        /// <param name="stationNo"> Personnel Number </param>
        /// <returns> Process s</returns>
        public Nodes GetHisNodes(string stationNo)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(FlowEmpAttr.FK_Emp, stationNo);
            qo.DoQuery();

            Nodes ens = new Nodes();
            foreach (FlowEmp en in this)
            {
                ens.AddEntity(new Node(en.FK_Flow));
            }
            return ens;
        }
        /// <summary>
        ///  This process of turning a collection of Nodes
        /// </summary>
        /// <param name="nodeID"> This process ID</param>
        /// <returns> This process of turning a collection of Nodes (FromNodes)</returns> 
        public Stations GetHisStations(int nodeID)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(FlowEmpAttr.FK_Flow, nodeID);
            qo.DoQuery();

            Stations ens = new Stations();
            foreach (FlowEmp en in this)
            {
                ens.AddEntity(new Station(en.FK_Emp));
            }
            return ens;
        }
    }
}
