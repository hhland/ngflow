using System;
using System.Data;
using BP.DA;
using BP.WF;
using BP.En;

namespace BP.WF
{
	/// <summary>
	///  Ordinary working 
	/// </summary>
	public class GEWorkAttr :WorkAttr
	{
	}
    /// <summary>
    ///  Ordinary working 
    /// </summary>
    public class GEWork : Work
    {
        #region ”Î_SQLCash  Related to the operation 
        private SQLCash _SQLCash = null;
        public override SQLCash SQLCash
        {
            get
            {
                if (_SQLCash == null)
                {
                    _SQLCash = BP.DA.Cash.GetSQL(this.NodeFrmID.ToString());
                    if (_SQLCash == null)
                    {
                        _SQLCash = new SQLCash(this);
                        BP.DA.Cash.SetSQL(this.NodeFrmID.ToString(), _SQLCash);
                    }
                }
                return _SQLCash;
            }
            set
            {
                _SQLCash = value;
            }
        }
        #endregion

        #region  Constructor         
        /// <summary>
        ///  Ordinary working 
        /// </summary>
        public GEWork()
        {
        }
        /// <summary>
        ///  Ordinary working 
        /// </summary>
        /// <param name="nodeid"> Node ID</param>
        public GEWork(int nodeid, string nodeFrmID)
        {
            this.NodeFrmID = nodeFrmID;
            this.NodeID = nodeid;
            this.SQLCash = null;
        }
        /// <summary>
        ///  Ordinary working 
        /// </summary>
        /// <param name="nodeid"> Node ID</param>
        /// <param name="_oid">OID</param>
        public GEWork(int nodeid, string nodeFrmID, Int64 _oid)
        {
            this.NodeFrmID = nodeFrmID;
            this.NodeID = nodeid;
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
                this._enMap = BP.Sys.MapData.GenerHisMap(this.NodeFrmID );
                return this._enMap;
            }
        }
        /// <summary>
        /// GEWorks
        /// </summary>
        public override Entities GetNewEntities
        {
            get
            {
                if (this.NodeID == 0)
                    return new GEWorks();
                return new GEWorks(this.NodeID,this.NodeFrmID);
            }
        }
        #endregion
    }
    /// <summary>
    ///  Ordinary working s
    /// </summary>
    public class GEWorks : Works
    {
        #region  Override the base class methods 
        /// <summary>
        ///  Node ID
        /// </summary>
        public int NodeID = 0;
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
                    return new GEWork();
                return new GEWork(this.NodeID,this.NodeFrmID);
            }
        }
        /// <summary>
        ///  Ordinary working ID
        /// </summary>
        public GEWorks()
        {
        }
        /// <summary>
        ///  Ordinary working ID
        /// </summary>
        /// <param name="nodeid"></param>
        public GEWorks(int nodeid, string nodeFrmID)
        {
            this.NodeID = nodeid;
            this.NodeFrmID = nodeFrmID;
        }
        public string NodeFrmID = "";
        #endregion
    }
}
