using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.WF;
using BP.Port; 
using BP.En;

namespace BP.WF
{
	/// <summary>
	///  Transfer record 
	/// </summary>
    public class ShiftWorkAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  The work ID
        /// </summary>
        public const string WorkID = "WorkID";
        /// <summary>
        ///  Node 
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        ///  Transfer reason 
        /// </summary>
        public const string Note = "Note";
        /// <summary>
        ///  Handed people 
        /// </summary>
        public const string FK_Emp = "FK_Emp";
        /// <summary>
        ///  Transfer of names into 
        /// </summary>
        public const string FK_EmpName = "FK_EmpName";
        /// <summary>
        ///  Transfer time 
        /// </summary>
        public const string RDT = "RDT";
        /// <summary>
        ///  Whether read ?
        /// </summary>
        public const string IsRead = "IsRead";
        /// <summary>
        ///  Handed over to the 
        /// </summary>
        public const string ToEmp = "ToEmp";
        /// <summary>
        ///  Handed over to the person name 
        /// </summary>
        public const string ToEmpName = "ToEmpName";
        #endregion
    }
	/// <summary>
	///  Transfer record 
	/// </summary>
	public class ShiftWork : EntityMyPK
	{		
		#region  Basic properties 
		/// <summary>
		///  The work ID
		/// </summary>
        public Int64 WorkID
		{
			get
			{
				return this.GetValInt64ByKey(ShiftWorkAttr.WorkID);
			}
			set
			{
				SetValByKey(ShiftWorkAttr.WorkID,value);
			}
		}
		/// <summary>
		///  Work node 
		/// </summary>
		public int FK_Node
		{
			get
			{
				return this.GetValIntByKey(ShiftWorkAttr.FK_Node);
			}
			set
			{
				SetValByKey(ShiftWorkAttr.FK_Node,value);
			}
		}
        /// <summary>
        ///  Whether read ?
        /// </summary>
        public bool IsRead
        {
            get
            {
                return this.GetValBooleanByKey(ShiftWorkAttr.IsRead);
            }
            set
            {
                SetValByKey(ShiftWorkAttr.IsRead, value);
            }
        }
        /// <summary>
        /// ToEmpName
        /// </summary>
        public string ToEmpName
        {
            get
            {
                return this.GetValStringByKey(ShiftWorkAttr.ToEmpName);
            }
            set
            {
                SetValByKey(ShiftWorkAttr.ToEmpName, value);
            }
        }
        /// <summary>
        ///  Name of transfer .
        /// </summary>
        public string FK_EmpName
        {
            get
            {
                return this.GetValStringByKey(ShiftWorkAttr.FK_EmpName);
            }
            set
            {
                SetValByKey(ShiftWorkAttr.FK_EmpName, value);
            }
        }
        /// <summary>
        ///  Transfer time 
        /// </summary>
        public string RDT
        {
            get
            {
                return this.GetValStringByKey(ShiftWorkAttr.RDT);
            }
            set
            {
                SetValByKey(ShiftWorkAttr.RDT, value);
            }
        }
        /// <summary>
        ///  Transfer comments 
        /// </summary>
		public string Note
		{
			get
			{
				return this.GetValStringByKey(ShiftWorkAttr.Note);
			}
			set
			{
				SetValByKey(ShiftWorkAttr.Note,value);
			}
		}
        /// <summary>
        ///  Transfer comments html Format 
        /// </summary>
        public string NoteHtml
        {
            get
            {
                return this.GetValHtmlStringByKey(ShiftWorkAttr.Note);
            }
        }
        /// <summary>
        ///  Handed people 
        /// </summary>
        public string FK_Emp
        {
            get
            {
                return this.GetValStringByKey(ShiftWorkAttr.FK_Emp);
            }
            set
            {
                SetValByKey(ShiftWorkAttr.FK_Emp, value);
            }
        }
        /// <summary>
        ///  Handed over to the 
        /// </summary>
        public string ToEmp
        {
            get
            {
                return this.GetValStringByKey(ShiftWorkAttr.ToEmp);
            }
            set
            {
                SetValByKey(ShiftWorkAttr.ToEmp, value);
            }
        }
		#endregion

		#region  Constructor 
		/// <summary>
		///  Transfer record 
		/// </summary>
		public ShiftWork()
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

                Map map = new Map("WF_ShiftWork");
                map.EnDesc = " Transfer record ";
                map.EnType = EnType.App;

                map.AddMyPK();

                map.AddTBInt(ShiftWorkAttr.WorkID, 0, " The work ID", true, true);
                map.AddTBInt(ShiftWorkAttr.FK_Node, 0, "FK_Node", true, true);
                map.AddTBString(ShiftWorkAttr.FK_Emp, null, " Handed people ", true, true, 0, 40, 10);
                map.AddTBString(ShiftWorkAttr.FK_EmpName, null, " Name of transfer ", true, true, 0, 40, 10);

                map.AddTBString(ShiftWorkAttr.ToEmp, null, " Handed over to the ", true, true, 0, 40, 10);
                map.AddTBString(ShiftWorkAttr.ToEmpName, null, " Transferred to the name of the ", true, true, 0, 40, 10);

                map.AddTBDateTime(ShiftWorkAttr.RDT, null, " Transfer time ", true, true);
                map.AddTBString(ShiftWorkAttr.Note, null, " Transfer reason ", true, true, 0, 2000, 10);

                map.AddTBInt(ShiftWorkAttr.IsRead, 0, " Whether read ?", true, true);
                this._enMap = map;
                return this._enMap;
            }
        }
        protected override bool beforeInsert()
        {
            this.MyPK = BP.DA.DBAccess.GenerOIDByGUID().ToString();
            this.RDT = DataType.CurrentDataTime;
            return base.beforeInsert();
        }
		#endregion	 
	}
	/// <summary>
	///  Transfer record s 
	/// </summary>
	public class ShiftWorks : Entities
	{	 
		#region  Structure 
		/// <summary>
		///  Transfer record s
		/// </summary>
		public ShiftWorks()
		{
		}
		/// <summary>
		///  Get it  Entity
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new ShiftWork();
			}
		}
		#endregion
	}
	
}
