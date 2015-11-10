using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.Port;

namespace BP.WF.Template
{
	/// <summary>
	///  Recipient Information Properties 
	/// </summary>
    public class SelectInfoAttr
    {
        /// <summary>
        ///  The work ID
        /// </summary>
        public const string WorkID = "WorkID";
        /// <summary>
        ///  Accept node 
        /// </summary>
        public const string AcceptNodeID = "AcceptNodeID";
        /// <summary>
        /// left Information 
        /// </summary>
        public const string InfoLeft = "InfoLeft";
        /// <summary>
        ///  Intermediate information 
        /// </summary>
        public const string InfoCenter = "InfoCenter";
        public const string InfoRight = "InfoRight";
        public const string AccType = "AccType";
    }
	/// <summary>
	///  Recipient Information 
	/// </summary>
    public class SelectInfo : EntityMyPK
    {
        #region  Basic properties 
        /// <summary>
        /// The work ID
        /// </summary>
        public Int64 WorkID
        {
            get
            {
                return this.GetValInt64ByKey(SelectInfoAttr.WorkID);
            }
            set
            {
                this.SetValByKey(SelectInfoAttr.WorkID, value);
            }
        }
        /// <summary>
        /// Select the node 
        /// </summary>
        public int AcceptNodeID
        {
            get
            {
                return this.GetValIntByKey(SelectInfoAttr.AcceptNodeID);
            }
            set
            {
                this.SetValByKey(SelectInfoAttr.AcceptNodeID, value);
            }
        }
        public int AccType
        {
            get
            {
                return this.GetValIntByKey(SelectInfoAttr.AccType);
            }
            set
            {
                this.SetValByKey(SelectInfoAttr.AccType, value);
            }
        }
        /// <summary>
        ///  Information 
        /// </summary>
        public string Info
        {
            get
            {
                return this.GetValStringByKey(SelectInfoAttr.InfoLeft);
            }
            set
            {
                this.SetValByKey(SelectInfoAttr.InfoLeft, value);
            }
        }
        public string InfoCenter
        {
            get
            {
                return this.GetValStringByKey(SelectInfoAttr.InfoCenter);
            }
            set
            {
                this.SetValByKey(SelectInfoAttr.InfoCenter, value);
            }
        }
        public string InfoRight
        {
            get
            {
                return this.GetValStringByKey(SelectInfoAttr.InfoRight);
            }
            set
            {
                this.SetValByKey(SelectInfoAttr.InfoRight, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Recipient Information 
        /// </summary>
        public SelectInfo()
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

                Map map = new Map("WF_SelectInfo");
                map.EnDesc = " Choose to accept / Cc node information ";

                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.AddMyPK();
                map.AddTBInt(SelectInfoAttr.AcceptNodeID, 0, " Accept node ", true, false);
                map.AddTBInt(SelectInfoAttr.WorkID, 0, " The work ID", true, false);
                map.AddTBString(SelectInfoAttr.InfoLeft, null, "InfoLeft", true, false, 0, 200, 10);
                map.AddTBString(SelectInfoAttr.InfoCenter, null, "InfoCenter", true, false, 0, 200, 10);
                map.AddTBString(SelectInfoAttr.InfoRight, null, "InfoLeft", true, false, 0, 200, 10);
                map.AddTBInt(SelectAccperAttr.AccType, 0, " Type (@0= Recipient @1= Cc )", true, false);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        protected override bool beforeUpdateInsertAction()
        {
            this.MyPK = this.AcceptNodeID + "_" + this.WorkID + "_" + this.AccType; ;
            return base.beforeUpdateInsertAction();
        }
    }
	/// <summary>
	///  Recipient Information 
	/// </summary>
    public class SelectInfos : EntitiesMyPK
    {
        /// <summary>
        ///  Recipient Information 
        /// </summary>
        public SelectInfos() { }
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new SelectInfo();
            }
        }
    }
}
