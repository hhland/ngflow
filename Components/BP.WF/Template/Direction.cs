using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.WF.Template;

namespace BP.WF
{
	/// <summary>
	///  Direction attribute node 	  
	/// </summary>
	public class DirectionAttr
	{
		/// <summary>
		///  Node 
		/// </summary>
		public const string Node="Node";
		/// <summary>
		///  Node steering 
		/// </summary>
		public const string ToNode="ToNode";
        /// <summary>
        ///  Process ID 
        /// </summary>
        public const string FK_Flow = "FK_Flow";
        /// <summary>
        ///  Direction Type 
        /// </summary>
        public const string DirType = "DirType";
        /// <summary>
        ///  Can backtrack 
        /// </summary>
        public const string IsCanBack = "IsCanBack";
        /// <summary>
        ///  Polyline information 
        /// </summary>
        public const string Dots = "Dots";
	}
	/// <summary>
	///  Node direction 
	///  Direction of the node consists of two parts .
	/// 1, Node.
	/// 2, toNode.
	///  Records from one node to the other nodes of the plurality of .
	///  Also recorded to the other nodes of this node .
	/// </summary>
	public class Direction :EntityMyPK
	{
		#region  Basic properties 
		/// <summary>
		/// Node 
		/// </summary>
        public int Node
        {
            get
            {
                return this.GetValIntByKey(DirectionAttr.Node);
            }
            set
            {
                this.SetValByKey(DirectionAttr.Node, value);
            }
        }
        public int DirType
        {
            get
            {
                return this.GetValIntByKey(DirectionAttr.DirType);
            }
            set
            {
                this.SetValByKey(DirectionAttr.DirType, value);
            }
        }
        public string FK_Flow
        {
            get
            {
                return this.GetValStringByKey(DirectionAttr.FK_Flow);
            }
            set
            {
                this.SetValByKey(DirectionAttr.FK_Flow, value);
            }
        }
		/// <summary>
		///  Node steering 
		/// </summary>
		public int  ToNode
		{
			get
			{
				return this.GetValIntByKey(DirectionAttr.ToNode);
			}
			set
			{
				this.SetValByKey(DirectionAttr.ToNode,value);
			}
		}
		#endregion 

		#region  Constructor 
		/// <summary>
		///  Node direction 
		/// </summary>
		public Direction(){}
		/// <summary>
		///  Override the base class methods 
		/// </summary>
		public override Map EnMap
		{
			get
			{
				if (this._enMap!=null) 
					return this._enMap;
				
				Map map = new Map("WF_Direction");			 
				map.EnDesc=" Node direction information ";

                /*
                 * MyPK  Is a composite primary key   From  Node+'_'+ToNode+'_'+DirType  Portfolio .  Such as : 101_102_1
                 */
                map.AddMyPK();
                map.AddTBString(DirectionAttr.FK_Flow, null, " Process ", true, true, 0, 3, 0, false);
                map.AddTBInt(DirectionAttr.Node, 0, " From node ", false, true);
				map.AddTBInt( DirectionAttr.ToNode,0," To node ",false,true);
                map.AddTBInt(DirectionAttr.DirType, 0, " Type 0 Go ahead 1 Return ", false, true);
                map.AddTBInt(DirectionAttr.IsCanBack, 0, " Can backtrack ( Back line for effective )", false, true);
                /*
                 * Dots  Storage format : @x1,y1@x2,y2
                 */
                map.AddTBString(NodeReturnAttr.Dots, null, " Track information ", true, true, 0, 300, 0, false);

				this._enMap=map;
				return this._enMap;
			}
		}
		#endregion

        /// <summary>
        ///  Deal with pk 
        /// </summary>
        /// <returns></returns>
        protected override bool beforeInsert()
        {
            this.MyPK = this.Node + "_" + this.ToNode + "_" + this.DirType;
            return base.beforeInsert();
        }
	}
	 /// <summary>
	 ///  Node direction 
	 /// </summary>
	public class Directions :En.Entities
	{
		/// <summary>
		///  Node direction 
		/// </summary>
		public Directions(){}
        /// <summary>
        ///  Node direction 
        /// </summary>
        /// <param name="NodeID"> Node ID</param>
        /// <param name="dirType"> Type </param>
        public Directions(int NodeID, int dirType)
		{
			QueryObject qo = new QueryObject(this);
			qo.AddWhere(DirectionAttr.Node,NodeID);
            qo.addAnd();
            qo.AddWhere(DirectionAttr.DirType, dirType);
		    qo.DoQuery();			
		}
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new Direction();
			}
		}
		/// <summary>
		///  Steering direction of this set of nodes 
		/// </summary>
		/// <param name="nodeID"> This node ID</param>
		/// <param name="isLifecyle"> Is not determine the lifetime of the node </param>		 
		/// <returns> Steering direction set (ToNodes)</returns> 
		public Nodes GetHisToNodes(int nodeID, bool isLifecyle)
		{
			Nodes nds = new Nodes();
			QueryObject qo = new QueryObject(nds);
			qo.AddWhereInSQL(NodeAttr.NodeID,"SELECT ToNode FROM WF_Direction WHERE Node="+nodeID );
			qo.DoQuery();
			return nds;
		}
		/// <summary>
		///  Turn this collection of nodes Nodes
		/// </summary>
		/// <param name="nodeID"> This node ID</param>
		/// <returns> Turn this collection of nodes Nodes (FromNodes)</returns> 
		public Nodes GetHisFromNodes(int nodeID)
		{
			QueryObject qo = new QueryObject(this);
			qo.AddWhere(DirectionAttr.ToNode,nodeID);
			qo.DoQuery();
			Nodes ens = new Nodes();
			foreach(Direction en in this)
			{
				ens.AddEntity( new Node(en.Node) ) ;
			}
			return ens;
		}
		 
	}
}
