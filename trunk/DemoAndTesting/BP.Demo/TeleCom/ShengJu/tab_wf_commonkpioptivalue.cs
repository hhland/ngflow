using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.WF;
using BP.Port;

namespace BP.Demo
{
    /// <summary>
    ///  Equipment   Property 
    /// </summary>
    public class tab_wf_commonkpioptivalueAttr:EntityOIDAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  Associated with PUC id
        /// </summary>
        public const string wf_commonkpioptivalue_id = "wf_commonkpioptivalue_id";
        /// <summary>
        /// kpi_id
        /// </summary>
        public const string kpi_id = "kpi_id";
        /// <summary>
        /// WorkID
        /// </summary>
        public const string WorkID = "WorkID";
        /// <summary>
        /// ParentWorkID
        /// </summary>
        public const string ParentWorkID = "ParentWorkID";
        /// <summary>
        ///  Process ID 
        /// </summary>
        public const string fk_flow = "fk_flow";
        /// <summary>
        ///  Business Field 
        /// </summary>
        public const string region_id = "region_id";
        /// <summary>
        ///  Business Field 1
        /// </summary>
        public const string remark = "remark";
        /// <summary>
        ///  Person in charge 
        /// </summary>
        public const string fuzeren = "fuzeren";
        /// <summary>
        ///  Location 
        /// </summary>
        public const string addr = "addr";
        #endregion
    }
    /// <summary>
    ///  Equipment 
    /// </summary>
    public class tab_wf_commonkpioptivalue : EntityOID
    {
        #region  Property 
        /// <summary>
        /// wf_commonkpioptivalue_id
        /// </summary>
        public int wf_commonkpioptivalue_id
        {
            get
            {
                return this.GetValIntByKey(tab_wf_commonkpioptivalueAttr.wf_commonkpioptivalue_id);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpioptivalueAttr.wf_commonkpioptivalue_id, value);
            }
        }
        /// <summary>
        ///  Process ID 
        /// </summary>
        public string fk_flow
        {
            get
            {
                return this.GetValStringByKey(tab_wf_commonkpioptivalueAttr.fk_flow);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpioptivalueAttr.fk_flow, value);
            }
        }
      
        /// <summary>
        /// WorkID
        /// </summary>
        public int WorkID
        {
            get
            {
                return this.GetValIntByKey(tab_wf_commonkpioptivalueAttr.WorkID);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpioptivalueAttr.WorkID, value);
            }
        }
        /// <summary>
        /// ParentWorkID
        /// </summary>
        public int ParentWorkID
        {
            get
            {
                return this.GetValIntByKey(tab_wf_commonkpioptivalueAttr.ParentWorkID);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpioptivalueAttr.ParentWorkID, value);
            }
        }
        #endregion

        #region  Business Field 
        public string region_id
        {
            get
            {
                return this.GetValStringByKey(tab_wf_commonkpioptivalueAttr.region_id);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpioptivalueAttr.region_id, value);
            }
        }
        public string addr
        {
            get
            {
                return this.GetValStringByKey(tab_wf_commonkpioptivalueAttr.addr);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpioptivalueAttr.addr, value);
            }
        }
        public string remark
        {
            get
            {
                return this.GetValStringByKey(tab_wf_commonkpioptivalueAttr.remark);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpioptivalueAttr.remark, value);
            }
        }
        public string fuzeren
        {
            get
            {
                return this.GetValStringByKey(tab_wf_commonkpioptivalueAttr.fuzeren);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpioptivalueAttr.fuzeren, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Equipment 
        /// </summary>
        public tab_wf_commonkpioptivalue()
        {
        }
    
        public tab_wf_commonkpioptivalue(Int64 WorkID)
        {
            this.Retrieve(tab_wf_commonkpioptivalueAttr.WorkID, WorkID);
        }
        /// <summary>
        ///  Override the base class methods 
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;
                Map map = new Map("tab_wf_commonkpioptivalue");
                map.EnDesc = " Equipment ";

                map.AddTBIntPKOID();
                map.AddTBInt(tab_wf_commonkpioptivalueAttr.wf_commonkpioptivalue_id, 0,
                    "wf_commonkpioptivalue_id", true, false);

                map.AddTBInt(tab_wf_commonkpioptivalueAttr.WorkID, 0, " The work ID", true, false);
                map.AddTBInt(tab_wf_commonkpioptivalueAttr.ParentWorkID, 0, " Parent ID", true, false);

                map.AddTBString(tab_wf_commonkpioptivalueAttr.fk_flow, null, "fk_flow", true, false, 0, 200, 10);
                map.AddTBString(tab_wf_commonkpioptivalueAttr.fuzeren, null, " Person in charge ", true, false, 0, 200, 10);

                map.AddTBString(tab_wf_commonkpioptivalueAttr.kpi_id, null, "kpi_id", true, false, 0, 200, 10);
                map.AddTBString(tab_wf_commonkpioptivalueAttr.region_id, null, "region_id", true, false, 0, 200, 10);
                map.AddTBString(tab_wf_commonkpioptivalueAttr.remark, null, "remark", true, false, 0, 200, 10);
                map.AddTBString(tab_wf_commonkpioptivalueAttr.addr, null, "addr", true, false, 0, 200, 10);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    ///  Equipment s
    /// </summary>
    public class tab_wf_commonkpioptivalues : EntitiesOID
    {
        #region  Method 
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new tab_wf_commonkpioptivalue();
            }
        }
        /// <summary>
        ///  Equipment s
        /// </summary>
        public tab_wf_commonkpioptivalues() { }
        #endregion
    }
}
