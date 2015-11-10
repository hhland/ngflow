using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.En;
using BP.Port;
using BP.WF;

namespace BP.PRJ
{
    /// <summary>
    ///  Process Status Properties 	  
    /// </summary>
    public class NodeAccessAttr
    {
        /// <summary>
        ///  File name 
        /// </summary>
        public const string FileName = "FileName";
        /// <summary>
        ///  Full name 
        /// </summary>
        public const string FileFullName = "FileFullName";
        /// <summary>
        ///  Work node 
        /// </summary>
        public const string FK_Node = "FK_Node";

        /// <summary>
        ///  Is visible 
        /// </summary>
        public const string IsView = "IsView";
        /// <summary>
        ///  Can upload 
        /// </summary>
        public const string IsUpload = "IsUpload";
        /// <summary>
        ///  Can download 
        /// </summary>
        public const string IsDown = "IsDown";
        /// <summary>
        ///  Whether it can be deleted 
        /// </summary>
        public const string IsDelete = "IsDelete";
        /// <summary>
        ///  Project 
        /// </summary>
        public const string FK_Prj = "FK_Prj";

    }
    /// <summary>
    ///  Process job attributes 
    ///  The work of the node consists of two parts .	 
    ///  Records from one node to the other nodes of the plurality of .
    ///  Also recorded to the other nodes of this node .
    /// </summary>
    public class NodeAccess : EntityMyPK
    {
        #region  Basic properties 
        /// <summary>
        /// HisUAC
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
        /// Node 
        /// </summary>
        public int FK_Node
        {
            get
            {
                return this.GetValIntByKey(NodeAccessAttr.FK_Node);
            }
            set
            {
                this.SetValByKey(NodeAccessAttr.FK_Node, value);
            }
        }
        /// <summary>
        ///  File name 
        /// </summary>
        public string FileName
        {
            get
            {
                return this.GetValStringByKey(NodeAccessAttr.FileName);
            }
            set
            {
                this.SetValByKey(NodeAccessAttr.FileName, value);
            }
        }
        /// <summary>
        /// FK_Prj
        /// </summary>
        public string FK_Prj
        {
            get
            {
                return this.GetValStringByKey(NodeAccessAttr.FK_Prj);
            }
            set
            {
                this.SetValByKey(NodeAccessAttr.FK_Prj, value);
            }
        }
        /// <summary>
        ///  Full file name 
        /// </summary>
        public string FileFullName
        {
            get
            {
                return this.GetValStringByKey(NodeAccessAttr.FileFullName);
            }
            set
            {
                this.SetValByKey(NodeAccessAttr.FileFullName, value);
            }
        }

        public bool IsView
        {
            get
            {
                return this.GetValBooleanByKey(NodeAccessAttr.IsView);
            }
            set
            {
                this.SetValByKey(NodeAccessAttr.IsView, value);
            }
        }
        public bool IsUpload
        {
            get
            {
                return this.GetValBooleanByKey(NodeAccessAttr.IsUpload);
            }
            set
            {
                this.SetValByKey(NodeAccessAttr.IsUpload, value);
            }
        }
        public bool IsDown
        {
            get
            {
                return this.GetValBooleanByKey(NodeAccessAttr.IsDown);
            }
            set
            {
                this.SetValByKey(NodeAccessAttr.IsDown, value);
            }
        }
        public bool IsDelete
        {
            get
            {
                return this.GetValBooleanByKey(NodeAccessAttr.IsDelete);
            }
            set
            {
                this.SetValByKey(NodeAccessAttr.IsDelete, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Process job attributes 
        /// </summary>
        public NodeAccess() { }
        /// <summary>
        ///  Override the base class methods 
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("Prj_NodeAccess");
                map.EnDesc = " Access Rules ";

                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.AddMyPK();

                map.AddTBString(NodeAccessAttr.FK_Prj, null, " Item Number ", true, true, 0, 200, 20);
                map.AddTBString(NodeAccessAttr.FileName, null, " File name ", true, true, 0, 200, 20);
                map.AddTBString(NodeAccessAttr.FileFullName, null, " Full file name ", true, true, 0, 200, 20);
                map.AddTBInt(NodeAccessAttr.FK_Node, 0, " Node ", true, true);

                map.AddTBInt(NodeAccessAttr.IsView, 0, "IsView", true, true);
                map.AddTBInt(NodeAccessAttr.IsUpload, 0, "IsUpload", true, true);
                map.AddTBInt(NodeAccessAttr.IsDown, 0, "IsDown", true, true);
                map.AddTBInt(NodeAccessAttr.IsDelete, 0, "IsDelete", true, true);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    ///  Node Access 
    /// </summary>
    public class NodeAccesss : EntitiesMyPK
    {
        /// <summary>
        ///  Node Access 
        /// </summary>
        public NodeAccesss() { }
        /// <summary>
        ///  Node Access 
        /// </summary>
        /// <param name="NodeID"> Node ID</param>
        public NodeAccesss(int NodeID)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(NodeAccessAttr.FileName, NodeID);
            qo.DoQuery();
        }
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new NodeAccess();
            }
        }
    }
}
