using System;
using System.Data;
using BP.DA;
using BP.WF;
using BP.En;

namespace BP.WF
{
    /// <summary>
    ///  Approval Status 
    /// </summary>
    public enum CheckState
    {
        /// <summary>
        ///  Time out 
        /// </summary>
        Pause = 2,
        /// <summary>
        ///  Agree 
        /// </summary>
        Agree = 1,
        /// <summary>
        ///  Disagree 
        /// </summary>
        Dissent = 0
    }
	/// <summary>
	///  Start working node 
	/// </summary>
	public class GEStartWorkAttr :WorkAttr
	{
        /// <summary>
        ///  Years 
        /// </summary>
        public const string FK_NY = "FK_NY";
        /// <summary>
        ///  Process Status 
        /// </summary>
        public const string WFState = "WFState";
        /// <summary>
        ///  Department 
        /// </summary>
        public const string FK_Dept = "FK_Dept";
        /// <summary>
        ///  Department text
        /// </summary>
        public const string FK_DeptText = "FK_DeptText";
	}
    /// <summary>
    ///  Start working node 
    /// </summary>
    public class GEStartWork : StartWork
    {
        #region  Constructor 
        /// <summary>
        ///  Start working node 
        /// </summary>
        public GEStartWork()
        {
        }
        /// <summary>
        ///  Start working node 
        /// </summary>
        /// <param name="nodeid"> Node ID</param>
        public GEStartWork(int nodeid, string nodeFrmID)
        {
            this.NodeID = nodeid;
            this.NodeFrmID = nodeFrmID;
            this.SQLCash=null;

        }
        /// <summary>
        ///  Start working node 
        /// </summary>
        /// <param name="nodeid"> Node ID</param>
        /// <param name="_oid">OID</param>
        public GEStartWork(int nodeid, string nodeFrmID,Int64 _oid)
        {
            this.NodeID = nodeid;
            this.NodeFrmID = nodeFrmID;
            this.OID = _oid;
            this.SQLCash = null;
        }
        #endregion

        #region Map
        /// <summary>
        ///  Override the base class methods 
        /// </summary>
        public override Map EnMap
        {
            get
            {
                this._enMap = BP.Sys.MapData.GenerHisMap(this.NodeFrmID);
                return this._enMap;
            }
        }
        public override Entities GetNewEntities
        {
            get
            {
                if (this.NodeID == 0)
                    return new GEStartWorks();
                return new GEStartWorks(this.NodeID,this.NodeFrmID);
            }
        }
        #endregion
    }
    /// <summary>
    ///  Start working node s
    /// </summary>
    public class GEStartWorks : Works
    {
        #region  Override the base class methods 
        /// <summary>
        ///  Node ID
        /// </summary>
        public int NodeID = 0;
        public string NodeFrmID = "";
        #endregion

        #region  Method 
        /// <summary>
        ///  Get it  Entity
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                if (this.NodeID == 0)
                    return new GEStartWork();
                return new GEStartWork(this.NodeID,this.NodeFrmID);
            }
        }
        /// <summary>
        ///  Start working node ID
        /// </summary>
        public GEStartWorks()
        {

        }
        /// <summary>
        ///  Start working node ID
        /// </summary>
        /// <param name="nodeid"></param>
        public GEStartWorks(int nodeid,string nodeFrmID)
        {
            this.NodeID = nodeid;
            this.NodeFrmID = nodeFrmID;
        }
        #endregion
    }
}
