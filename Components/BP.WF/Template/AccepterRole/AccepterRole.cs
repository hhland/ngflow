using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.En;
using System.Collections;
using BP.Port;

namespace BP.WF.Template
{
    /// <summary>
    ///  Recipient Rule Properties 
    /// </summary>
    public class AccepterRoleAttr:BP.En.EntityOIDNameAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  Node number 
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        ///  Mode type 
        /// </summary>
        public const string FK_ModeSort = "FK_ModeSort";
        /// <summary>
        ///  Mode 
        /// </summary>
        public const string FK_Mode = "FK_Mode";

        public const string Tag0 = "Tag0";
        public const string Tag1 = "Tag1";
        public const string Tag2 = "Tag2";
        public const string Tag3 = "Tag3";
        public const string Tag4 = "Tag4";
        public const string Tag5 = "Tag5";
        #endregion
    }
    /// <summary>
    ///  Here each message recipient to store rules .	 
    /// </summary>
    public class AccepterRole : EntityOID
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
                uac.IsUpdate = true;
                return uac;
            }
        }
        /// <summary>
        ///  Node number 
        /// </summary>
        public string FK_Node
        {
            get
            {
                return this.GetValStringByKey(AccepterRoleAttr.FK_Node);
            }
            set
            {
                SetValByKey(AccepterRoleAttr.FK_Node, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  People accept the rules 
        /// </summary>
        public AccepterRole() { }
        /// <summary>
        ///  People accept the rules 
        /// </summary>
        /// <param name="oid"> People accept the rules ID</param>	
        public AccepterRole(int oid)
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

                Map map = new Map("WF_AccepterRole");
                map.EnDesc =   " People accept the rules "; //" People accept the rules ";

                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;

                map.AddTBIntPKOID();

                map.AddTBString(AccepterRoleAttr.Name, null, null, true, false, 0, 200, 10, true);
                map.AddTBString(AccepterRoleAttr.FK_Node, null, " Node ", false, true, 0, 100, 10);
                map.AddTBInt(AccepterRoleAttr.FK_Mode, 0, " Mode type ", false, true);

                map.AddTBString(AccepterRoleAttr.Tag0, null, "Tag0", false, true, 0, 999, 10);
                map.AddTBString(AccepterRoleAttr.Tag1, null, "Tag1", false, true, 0, 999, 10);
                map.AddTBString(AccepterRoleAttr.Tag2, null, "Tag2", false, true, 0, 999, 10);
                map.AddTBString(AccepterRoleAttr.Tag3, null, "Tag3", false, true, 0, 999, 10);
                map.AddTBString(AccepterRoleAttr.Tag4, null, "Tag4", false, true, 0, 999, 10);
                map.AddTBString(AccepterRoleAttr.Tag5, null, "Tag5", false, true, 0, 999, 10);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

       
    }
    /// <summary>
    ///  Recipient set of rules 
    /// </summary>
    public class AccepterRoles : Entities
    {
        #region  Method 
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new AccepterRole();
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Recipient set of rules 
        /// </summary>
        public AccepterRoles()
        {
        }
        /// <summary>
        ///  Recipient set of rules .
        /// </summary>
        /// <param name="FlowNo"></param>
        public AccepterRoles(string FK_Node)
        {
            this.Retrieve(AccepterRoleAttr.FK_Node, FK_Node);
        }
        #endregion
    }
}
