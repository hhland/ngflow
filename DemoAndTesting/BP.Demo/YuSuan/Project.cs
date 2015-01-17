using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.Port;

namespace BP.YS
{
	/// <summary>
	///  Project Properties 
	/// </summary>
    public class ProjectAttr
    {
        public const string Doc = "Doc";
        /// <summary>
        ///  Node 
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        ///  Listen node 
        /// </summary>
        public const string Nodes = "Nodes";
        /// <summary>
        ///  Description 
        /// </summary>
        public const string NodesDesc = "NodesDesc";
        /// <summary>
        ///  Listen to the way 
        /// </summary>
        public const string AlertWay = "AlertWay";
        /// <summary>
        /// FZR
        /// </summary>
        public const string FZR = "FZR";
    }
	/// <summary>
	///  Project 
	///  Listen node has two parts .	 
	///  Records from one node to the other nodes of the plurality of .
	///  Also recorded to the other nodes of this node .
	/// </summary>
    public class Project : EntityNoName
    {
        #region  Basic properties 
        /// <summary>
        /// Node 
        /// </summary>
        public int FK_Node
        {
            get
            {
                return this.GetValIntByKey(ProjectAttr.FK_Node);
            }
            set
            {
                this.SetValByKey(ProjectAttr.FK_Node, value);
            }
        }
        /// <summary>
        ///  Listen node 
        /// </summary>
        public string Nodes
        {
            get
            {
                return this.GetValStringByKey(ProjectAttr.Nodes);
            }
            set
            {
                this.SetValByKey(ProjectAttr.Nodes, value);
            }
        }
        public string NodesDesc
        {
            get
            {
                return this.GetValStringByKey(ProjectAttr.NodesDesc);
            }
            set
            {
                this.SetValByKey(ProjectAttr.NodesDesc, value);
            }
        }
        public string Doc
        {
            get
            {
                string s= this.GetValStringByKey(ProjectAttr.Doc);
                if (string.IsNullOrEmpty(s) == true)
                    s = "";
                return s;
            }
            set
            {
                this.SetValByKey(ProjectAttr.Doc, value);
            }
        }
        public string FZR
        {
            get
            {
                return this.GetValStringByKey(ProjectAttr.FZR);
            }
            set
            {
                this.SetValByKey(ProjectAttr.FZR, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Project 
        /// </summary>
        public Project()
        {

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

                Map map = new Map("YS_Project");
                map.EnDesc = " Project ";

                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.IsAutoGenerNo = true;

                map.AddTBStringPK(EmpAttr.No, null, " Serial number ", true, true, 5, 5, 5);
                map.AddTBString(EmpAttr.Name, null, " Name ", true, false, 0, 100, 100);
                map.AddTBStringDoc();

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
	/// <summary>
	///  Project 
	/// </summary>
    public class Projects : EntitiesNoName
    {
        /// <summary>
        ///  Project 
        /// </summary>
        public Projects() { }
        /// <summary>
        ///  Project 
        /// </summary>
        /// <param name="fk_flow"></param>
        public Projects(string fk_flow)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhereInSQL(ProjectAttr.FK_Node, "SELECT NodeID FROM WF_Node WHERE FK_Flow='" + fk_flow + "'");
            qo.DoQuery();
        }
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new Project();
            }
        }
    }
}
