using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.WF.Template;
using System.Collections;
using BP.Port;

namespace BP.WF
{
    /// <summary>
    ///  Label Properties 
    /// </summary>
    public class LabNoteAttr:BP.En.EntityOIDNameAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  Process 
        /// </summary>
        public const string FK_Flow = "FK_Flow";
        /// <summary>
        /// x
        /// </summary>
        public const string X = "X";
        /// <summary>
        /// y
        /// </summary>
        public const string Y = "Y";
        #endregion
    }
    /// <summary>
    ///  Label .	 
    /// </summary>
    public class LabNote : EntityMyPK
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
        /// x
        /// </summary>
        public int X
        {
            get
            {
                return this.GetValIntByKey(NodeAttr.X);
            }
            set
            {
                this.SetValByKey(NodeAttr.X, value);
            }
        }

        /// <summary>
        /// y
        /// </summary>
        public int Y
        {
            get
            {
                return this.GetValIntByKey(NodeAttr.Y);
            }
            set
            {
                this.SetValByKey(NodeAttr.Y, value);
            }
        }
        /// <summary>
        ///  Transaction ID tags 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.GetValStringByKey(NodeAttr.FK_Flow);
            }
            set
            {
                SetValByKey(NodeAttr.FK_Flow, value);
            }
        }
        public string Name
        {
            get
            {
                return this.GetValStringByKey(NodeAttr.Name);
            }
            set
            {
                SetValByKey(NodeAttr.Name, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Label 
        /// </summary>
        public LabNote() { }
        /// <summary>
        ///  Label 
        /// </summary>
        /// <param name="_oid"> Label ID</param>	
        public LabNote(string mypk)
        {
            this.MyPK = mypk;
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

                Map map = new Map("WF_LabNote");
                map.EnDesc =   " Label "; // " Label ";

                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;

                map.AddMyPK();

                map.AddTBString(NodeAttr.Name, null, null, true, false, 0, 3000, 10, true);
                map.AddTBString(NodeAttr.FK_Flow, null, " Process ", false, true, 0, 100, 10);

                map.AddTBInt(NodeAttr.X, 0, "X Coordinate ", false, false);
                map.AddTBInt(NodeAttr.Y, 0, "Y Coordinate ", false, false);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        protected override bool beforeInsert()
        {
            this.MyPK = BP.DA.DBAccess.GenerOID().ToString();
            return base.beforeInsert();
        }
    }
    /// <summary>
    ///  Label collection 
    /// </summary>
    public class LabNotes : Entities
    {
        #region  Method 
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new LabNote();
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Label collection 
        /// </summary>
        public LabNotes()
        {
        }
        /// <summary>
        ///  Label collection .
        /// </summary>
        /// <param name="FlowNo"></param>
        public LabNotes(string fk_flow)
        {
            this.Retrieve(NodeAttr.FK_Flow, fk_flow);
        }
        #endregion
    }
}
