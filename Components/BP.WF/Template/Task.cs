using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.WF;
using BP.Port;

namespace BP.WF
{
	/// <summary>
	///  Task   Property 
	/// </summary>
    public class TaskAttr : EntityMyPKAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  Sponsor 
        /// </summary>
        public const string Starter = "Starter";
        /// <summary>
        ///  Process 
        /// </summary>
        public const string FK_Flow = "FK_Flow";
        /// <summary>
        ///  Parameters 
        /// </summary>
        public const string Paras = "Paras";
        /// <summary>
        ///  Task Status 
        /// </summary>
        public const string TaskSta = "TaskSta";
        /// <summary>
        /// Msg
        /// </summary>
        public const string Msg = "Msg";
        /// <summary>
        ///  Start Time 
        /// </summary>
        public const string StartDT = "StartDT";
        /// <summary>
        ///  Insert Date 
        /// </summary>
        public const string RDT = "RDT";
        #endregion
    }
	/// <summary>
	///  Task 
	/// </summary>
    public class Task : EntityMyPK
    {
        #region  Property 
        /// <summary>
        ///  Parameters 
        /// </summary>
        public string Paras
        {
            get
            {
                return this.GetValStringByKey(TaskAttr.Paras);
            }
            set
            {
                this.SetValByKey(TaskAttr.Paras, value);
            }
        }
        /// <summary>
        ///  Sponsor 
        /// </summary>
        public string Starter
        {
            get
            {
                return this.GetValStringByKey(TaskAttr.Starter);
            }
            set
            {
                this.SetValByKey(TaskAttr.Starter, value);
            }
        }
        /// <summary>
        ///  Process ID 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.GetValStringByKey(TaskAttr.FK_Flow);
            }
            set
            {
                this.SetValByKey(TaskAttr.FK_Flow, value);
            }
        }
        /// <summary>
        ///  Start Time £¨ Can be empty £©
        /// </summary>
        public string StartDT
        {
            get
            {
                return this.GetValStringByKey(TaskAttr.StartDT);
            }
            set
            {
                this.SetValByKey(TaskAttr.StartDT, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        /// Task
        /// </summary>
        public Task()
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
                Map map = new Map("WF_Task");
                map.EnDesc = " Task ";
                map.EnType = EnType.Admin;

                map.AddMyPK(); // Unique primary key .
                map.AddTBString(TaskAttr.FK_Flow, null, " Process ID ", true, false, 0, 200, 10);
                map.AddTBString(TaskAttr.Starter, null, " Sponsor ", true, false, 0, 200, 10);
                map.AddTBString(TaskAttr.Paras, null, " Parameters ", true, false, 0, 4000, 10);

                // TaskSta 0= Did not initiate ,1= Successfully launched ,2= Launch failure .
                map.AddTBInt(TaskAttr.TaskSta, 0, " Task Status ", true, false);

                map.AddTBString(TaskAttr.Msg, null, " News ", true, false, 0, 4000, 10);
                map.AddTBString(TaskAttr.StartDT, null, " Start Time ", true, false, 0, 20, 10);
                map.AddTBString(TaskAttr.RDT, null, " Time to insert data ", true, false, 0, 20, 10);
                
                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
	/// <summary>
	///  Task 
	/// </summary>
	public class Tasks: Entities
	{
		#region  Method 
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new Task();
			}
		}
		/// <summary>
        ///  Task 
		/// </summary>
		public Tasks(){} 		 
		#endregion
	}
}
