using System;
using System.Data;
using System.Collections;
using BP.DA;
using BP.En;
using BP.Port;

namespace BP.WF.Template
{
    /// <summary>
    ///  Display position 
    /// </summary>
    public enum ShowWhere
    {
        /// <summary>
        /// 树
        /// </summary>
        Tree,
        /// <summary>
        ///  Toolbar 
        /// </summary>
        Toolbar
    }
    /// <summary>
    ///  Toolbar Properties 
    /// </summary>
    public class NodeToolbarAttr : BP.En.EntityOIDNameAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  Node 
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        ///  To reach the target 
        /// </summary>
        public const string Target = "Target";
        /// <summary>
        ///  Title 
        /// </summary>
        public const string Title = "Title";
        /// <summary>
        /// url
        /// </summary>
        public const string Url = "Url";
        /// <summary>
        ///  Sequence number 
        /// </summary>
        public const string Idx = "Idx";
        /// <summary>
        ///  Show where ?
        /// </summary>
        public const string ShowWhere = "ShowWhere";
        #endregion
    }
    /// <summary>
    ///  Toolbar .	 
    /// </summary>
    public class NodeToolbar : EntityOID
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
        ///  Toolbar transaction number 
        /// </summary>
        public int FK_Node
        {
            get
            {
                return this.GetValIntByKey(NodeToolbarAttr.FK_Node);
            }
            set
            {
                SetValByKey(NodeToolbarAttr.FK_Node, value);
            }
        }
        public string Title
        {
            get
            {
                return this.GetValStringByKey(NodeToolbarAttr.Title);
            }
            set
            {
                SetValByKey(NodeToolbarAttr.Title, value);
            }
        }
        public string Url
        {
            get
            {
                return this.GetValStringByKey(NodeToolbarAttr.Url);
            }
            set
            {
                SetValByKey(NodeToolbarAttr.Url, value);
            }
        }
        public string Target
        {
            get
            {
                return this.GetValStringByKey(NodeToolbarAttr.Target);
            }
            set
            {
                SetValByKey(NodeToolbarAttr.Target, value);
            }
        }
        /// <summary>
        ///  Show where ?
        /// </summary>
        public ShowWhere ShowWhere
        {
            get
            {
                return (ShowWhere)this.GetValIntByKey(NodeToolbarAttr.ShowWhere);
            }
            set
            {
                SetValByKey(NodeToolbarAttr.ShowWhere, (int)value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Toolbar 
        /// </summary>
        public NodeToolbar() { }
        /// <summary>
        ///  Toolbar 
        /// </summary>
        /// <param name="_oid"> Toolbar ID</param>	
        public NodeToolbar(int oid)
        {
            this.OID = oid;
            this.Retrieve();
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

                Map map = new Map("WF_NodeToolbar");
                map.EnDesc = " Customize Toolbar "; 

                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;

                map.AddTBIntPKOID();

                map.AddTBString(NodeToolbarAttr.Title, null, " Title ", true, false, 0, 100, 100, true);
                map.AddTBString(NodeToolbarAttr.Target, null, " The goal ", true, false, 0, 200, 50, true);
                map.AddTBString(NodeToolbarAttr.Url, null, " Connection ", true, false, 0, 500, 300, true);
                //  Display position .
                map.AddDDLSysEnum(NodeToolbarAttr.ShowWhere, 0, " Display position ", true,
                    true, NodeToolbarAttr.ShowWhere,
                    "@0= Tree form @1= Toolbar ");


                map.AddTBInt(NodeToolbarAttr.Idx, 0, " Display Order ", true, false);
                map.AddTBInt(NodeToolbarAttr.FK_Node, 0, " Node ", false,true);
                map.AddMyFile(" Icon ");


                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    ///  Toolbar collection 
    /// </summary>
    public class NodeToolbars : Entities
    {
        #region  Method 
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new NodeToolbar();
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Toolbar collection 
        /// </summary>
        public NodeToolbars()
        {
        }
        /// <summary>
        ///  Toolbar collection .
        /// </summary>
        /// <param name="FlowNo"></param>
        public NodeToolbars(string FK_Node)
        {
            this.Retrieve(NodeToolbarAttr.FK_Node, FK_Node);
        }
        #endregion
    }
}
