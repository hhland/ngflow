using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.WF;
using BP.WF.Template;
using BP.Port;

namespace BP.WF
{
	/// <summary>
	///  Pending   Property 
	/// </summary>
    public class HungUpAttr:EntityMyPKAttr
    {
        #region  Basic properties 
        public const string Title = "Title";
        /// <summary>
        ///  The work ID
        /// </summary>
        public const string WorkID = "WorkID";
        /// <summary>
        ///  Executor 
        /// </summary>
        public const string Rec = "Rec";
        /// <summary>
        ///  Notification to 
        /// </summary>
        public const string NoticeTo = "NoticeTo";
        /// <summary>
        ///  Pending reason 
        /// </summary>
        public const string Note = "Note";
        /// <summary>
        ///  Hang time 
        /// </summary>
        public const string DTOfHungUp = "DTOfHungUp";
        /// <summary>
        ///  Node ID
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        ///  Recipient 
        /// </summary>
        public const string Accepter = "Accepter";
        /// <summary>
        ///  Suspend Mode .
        /// </summary>
        public const string HungUpWay = "HungUpWay";
        /// <summary>
        ///  Lift hang time 
        /// </summary>
        public const string DTOfUnHungUp = "DTOfUnHungUp";
        /// <summary>
        ///  Plans to lift the suspension time 
        /// </summary>
        public const string DTOfUnHungUpPlan = "DTOfUnHungUpPlan";
        #endregion
    }
	/// <summary>
	///  Pending 
	/// </summary>
    public class HungUp:EntityMyPK
    {
        #region  Property 
        public HungUpWay HungUpWay
        {
            get
            {
                return (HungUpWay)this.GetValIntByKey(HungUpAttr.HungUpWay);
            }
            set
            {
                this.SetValByKey(HungUpAttr.HungUpWay, (int)value);
            }
        }
        public int FK_Node
        {
            get
            {
                return this.GetValIntByKey(HungUpAttr.FK_Node);
            }
            set
            {
                this.SetValByKey(HungUpAttr.FK_Node, value);
            }
        }
        public Int64 WorkID
        {
            get
            {
                return this.GetValInt64ByKey(HungUpAttr.WorkID);
            }
            set
            {
                this.SetValByKey(HungUpAttr.WorkID, value);
            }
        }
        /// <summary>
        ///  Suspend title 
        /// </summary>
        public string Title
        {
            get
            {
                string s= this.GetValStringByKey(HungUpAttr.Title);
                if (string.IsNullOrEmpty(s))
                    s = " Come from @Rec Suspend information .";
                return s;
            }
            set
            {
                this.SetValByKey(HungUpAttr.Title, value);
            }
        }
        /// <summary>
        ///  Pending reason 
        /// </summary>
        public string Note
        {
            get
            {
               return this.GetValStringByKey(HungUpAttr.Note);
            }
            set
            {
                this.SetValByKey(HungUpAttr.Note, value);
            }
        }
        public string Rec
        {
            get
            {
                return this.GetValStringByKey(HungUpAttr.Rec);
            }
            set
            {
                this.SetValByKey(HungUpAttr.Rec, value);
            }
        }
        /// <summary>
        ///  Lift hang time 
        /// </summary>
        public string DTOfUnHungUp
        {
            get
            {
                return this.GetValStringByKey(HungUpAttr.DTOfUnHungUp);
            }
            set
            {
                this.SetValByKey(HungUpAttr.DTOfUnHungUp, value);
            }
        }
        /// <summary>
        ///  Expected to lift the suspension time 
        /// </summary>
        public string DTOfUnHungUpPlan
        {
            get
            {
                return this.GetValStringByKey(HungUpAttr.DTOfUnHungUpPlan);
            }
            set
            {
                this.SetValByKey(HungUpAttr.DTOfUnHungUpPlan, value);
            }
        }
        /// <summary>
        ///  Lift hang time 
        /// </summary>
        public string DTOfHungUp
        {
            get
            {
                return this.GetValStringByKey(HungUpAttr.DTOfHungUp);
            }
            set
            {
                this.SetValByKey(HungUpAttr.DTOfHungUp, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Pending 
        /// </summary>
        public HungUp()
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

                Map map = new Map("WF_HungUp");
                map.EnDesc = " Pending ";
                map.EnType = EnType.Admin;

                map.AddMyPK();
                map.AddTBInt(HungUpAttr.FK_Node, 0, " Node ID", true, true);
                map.AddTBInt(HungUpAttr.WorkID, 0, "WorkID", true, true);
                map.AddDDLSysEnum(HungUpAttr.HungUpWay, 0, " Suspend Mode ", true, true, HungUpAttr.HungUpWay, 
                    "@0= Unlimited hang @1= Pending the lifting by the specified time and inform myself @2= Pending the lifting by the specified time and notify the owner ");

                map.AddTBStringDoc(HungUpAttr.Note, null, " Pending reason ( Title and content support variables )", true, false, true);

                map.AddTBString(HungUpAttr.Rec, null, " Suspend people ", true, false, 0, 200, 10, true);

                map.AddTBDateTime(HungUpAttr.DTOfHungUp, null, " Hang time ", true, false);
                map.AddTBDateTime(HungUpAttr.DTOfUnHungUp, null, " The actual lifting of the suspension time ", true, false);
                map.AddTBDateTime(HungUpAttr.DTOfUnHungUpPlan, null, " Expected to lift the suspension time ", true, false);

                this._enMap = map;
                return this._enMap;
            }
        }
        /// <summary>
        ///  Release pending execution 
        /// </summary>
        public void DoRelease()
        {
        }
        #endregion
    }
	/// <summary>
	///  Pending 
	/// </summary>
	public class HungUps: EntitiesMyPK
	{
		#region  Method 
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new HungUp();
			}
		}
		/// <summary>
        ///  Pending 
		/// </summary>
		public HungUps(){} 		 
		#endregion
	}
}
