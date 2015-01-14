using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.WF;
using BP.Port; 

namespace BP.WF
{
	/// <summary>
	///  Return trajectory 
	/// </summary>
	public class ReturnWorkAttr 
	{
		#region  Basic properties 
		/// <summary>
		///  The work ID
		/// </summary>
		public const  string WorkID="WorkID";
		/// <summary>
		///  Staff 
		/// </summary>
		public const  string Worker="Worker";
		/// <summary>
		///  Reason for the return 
		/// </summary>
		public const  string Note="Note";
        /// <summary>
        ///  Return date 
        /// </summary>
        public const string RDT = "RDT";
        /// <summary>
        ///  Return man 
        /// </summary>
        public const string Returner = "Returner";
        /// <summary>
        ///  Name of return 
        /// </summary>
        public const string ReturnerName = "ReturnerName";
        /// <summary>
        ///  Back to the node 
        /// </summary>
        public const string ReturnToNode = "ReturnToNode";
        /// <summary>
        ///  Return node 
        /// </summary>
        public const string ReturnNode = "ReturnNode";
        /// <summary>
        ///  Return the node name 
        /// </summary>
        public const string ReturnNodeName = "ReturnNodeName";
        /// <summary>
        ///  Returned to 
        /// </summary>
        public const string ReturnToEmp = "ReturnToEmp";
        /// <summary>
        ///  After returning the need to backtrack ?
        /// </summary>
        public const string IsBackTracking = "IsBackTracking";
		#endregion
	}
	/// <summary>
	///  Return trajectory 
	/// </summary>
    public class ReturnWork : EntityMyPK
    {
        #region  Basic properties 
        /// <summary>
        ///  The work ID
        /// </summary>
        public Int64 WorkID
        {
            get
            {
                return this.GetValInt64ByKey(ReturnWorkAttr.WorkID);
            }
            set
            {
                SetValByKey(ReturnWorkAttr.WorkID, value);
            }
        }
        /// <summary>
        ///  Back to the node 
        /// </summary>
        public int ReturnToNode
        {
            get
            {
                return this.GetValIntByKey(ReturnWorkAttr.ReturnToNode);
            }
            set
            {
                SetValByKey(ReturnWorkAttr.ReturnToNode, value);
            }
        }
        /// <summary>
        ///  Return node 
        /// </summary>
        public int ReturnNode
        {
            get
            {
                return this.GetValIntByKey(ReturnWorkAttr.ReturnNode);
            }
            set
            {
                SetValByKey(ReturnWorkAttr.ReturnNode, value);
            }
        }
        public string ReturnNodeName
        {
            get
            {
                return this.GetValStrByKey(ReturnWorkAttr.ReturnNodeName);
            }
            set
            {
                SetValByKey(ReturnWorkAttr.ReturnNodeName, value);
            }
        }
        /// <summary>
        ///  Return man 
        /// </summary>
        public string Returner
        {
            get
            {
                return this.GetValStringByKey(ReturnWorkAttr.Returner);
            }
            set
            {
                SetValByKey(ReturnWorkAttr.Returner, value);
            }
        }
        public string ReturnerName
        {
            get
            {
                return this.GetValStringByKey(ReturnWorkAttr.ReturnerName);
            }
            set
            {
                SetValByKey(ReturnWorkAttr.ReturnerName, value);
            }
        }
        /// <summary>
        ///  Returned to 
        /// </summary>
        public string ReturnToEmp
        {
            get
            {
                return this.GetValStringByKey(ReturnWorkAttr.ReturnToEmp);
            }
            set
            {
                SetValByKey(ReturnWorkAttr.ReturnToEmp, value);
            }
        }
        public string Note
        {
            get
            {
                return this.GetValStringByKey(ReturnWorkAttr.Note);
            }
            set
            {
                SetValByKey(ReturnWorkAttr.Note, value);
            }
        }
        public string NoteHtml
        {
            get
            {
                return this.GetValHtmlStringByKey(ReturnWorkAttr.Note);
            }
        }
        /// <summary>
        ///  Record Date 
        /// </summary>
        public string RDT
        {
            get
            {
                return this.GetValStringByKey(ReturnWorkAttr.RDT);
            }
            set
            {
                SetValByKey(ReturnWorkAttr.RDT, value);
            }
        }
        /// <summary>
        ///  Do you want to backtrack ?
        /// </summary>
        public bool IsBackTracking
        {
            get
            {
                return this.GetValBooleanByKey(ReturnWorkAttr.IsBackTracking);
            }
            set
            {
                SetValByKey(ReturnWorkAttr.IsBackTracking, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Return trajectory 
        /// </summary>
        public ReturnWork() { }
        /// <summary>
        ///  Override the base class methods 
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("WF_ReturnWork");
                map.EnDesc =  " Return trajectory ";
                map.EnType = EnType.App;

                map.AddMyPK();

                map.AddTBInt(ReturnWorkAttr.WorkID, 0, "WorkID", true, true);

                map.AddTBInt(ReturnWorkAttr.ReturnNode, 0, " Return node ", true, true);
                map.AddTBString(ReturnWorkAttr.ReturnNodeName, null, " Return the node name ", true, true, 0, 200, 10);

                map.AddTBString(ReturnWorkAttr.Returner, null, " Return man ", true, true, 0, 20, 10);
                map.AddTBString(ReturnWorkAttr.ReturnerName, null, " Name of return ", true, true, 0, 200, 10);

                map.AddTBInt(ReturnWorkAttr.ReturnToNode, 0, "ReturnToNode", true, true);
                map.AddTBString(ReturnWorkAttr.ReturnToEmp, null, " Returned to ", true, true, 0, 4000, 10);

                map.AddTBString(ReturnWorkAttr.Note, "", " Reason for the return ", true, true, 0, 4000, 10);
                map.AddTBDateTime(ReturnWorkAttr.RDT, null, " Return date ", true, true);

                map.AddTBInt(ReturnWorkAttr.IsBackTracking, 0, " Do you want to backtrack ?", true, true);
                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion


        protected override bool beforeInsert()
        {
            this.Returner = BP.Web.WebUser.No;
            this.ReturnerName = BP.Web.WebUser.Name;

            this.RDT =DataType.CurrentDataTime;
            return base.beforeInsert();
        }
    }
	/// <summary>
	///  Return trajectory s 
	/// </summary>
	public class ReturnWorks : Entities
	{	 
		#region  Structure 
		/// <summary>
		///  Return trajectory s
		/// </summary>
		public ReturnWorks()
		{
		}
		/// <summary>
		///  Get it  Entity
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new ReturnWork();
			}
		}
		#endregion
	}
	
}
