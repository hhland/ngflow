using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.WF;
using BP.Port; 
using BP.En;

namespace BP.WF.Data
{
	/// <summary>
	///  Evaluate the quality of work 
	/// </summary>
	public class EvalAttr 
	{
		#region  Basic properties 
		/// <summary>
		///  Process ID 
		/// </summary>
		public const  string FK_Flow="FK_Flow";
        /// <summary>
        ///  Process Name 
        /// </summary>
        public const string FlowName = "FlowName";
        /// <summary>
        ///  Node assessment 
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        ///  Node Name 
        /// </summary>
        public const string NodeName = "NodeName";
        /// <summary>
        ///  The work ID
        /// </summary>
        public const string WorkID = "WorkID";
        /// <summary>
        ///  Membership department 
        /// </summary>
        public const string FK_Dept = "FK_Dept";
        /// <summary>
        ///  Department name 
        /// </summary>
        public const string DeptName = "DeptName";
		/// <summary>
		///  Years 
		/// </summary>
		public const  string FK_NY="FK_NY";
        /// <summary>
        ///  Title 
        /// </summary>
        public const string Title = "Title";
        /// <summary>
        ///  Evaluation time 
        /// </summary>
        public const string RDT = "RDT";
		/// <summary>
		///  Name of the person to be the assessment of 
		/// </summary>
		public const  string EvalEmpName="EvalEmpName";
		/// <summary>
        ///  Is the assessment of personnel numbers 
		/// </summary>
		public const  string EvalEmpNo="EvalEmpNo";
        /// <summary>
        ///  Evaluation scores 
        /// </summary>
        public const string EvalCent = "EvalCent";
        /// <summary>
        ///  Evaluation Content 
        /// </summary>
        public const string EvalNote = "EvalNote";
		/// <summary>
		///  Evaluators 
		/// </summary>
		public const  string Rec="Rec";
        /// <summary>
        ///  Evaluators name 
        /// </summary>
        public const string RecName = "RecName";
		#endregion
	}
	/// <summary>
	///  Evaluate the quality of work 
	/// </summary>
	public class Eval : EntityMyPK
	{
		#region  Basic properties 
        /// <summary>
        ///  Process title 
        /// </summary>
        public string Title
        {
            get
            {
                return this.GetValStringByKey(EvalAttr.Title);
            }
            set
            {
                this.SetValByKey(EvalAttr.Title, value);
            }
        }
        /// <summary>
        ///  The work ID
        /// </summary>
        public Int64 WorkID
		{
			get
			{
				return this.GetValInt64ByKey(EvalAttr.WorkID);
			}
			set
			{
				this.SetValByKey(EvalAttr.WorkID,value);
			}
		}
        /// <summary>
        ///  Node number 
        /// </summary>
		public int FK_Node
		{
			get
			{
				return this.GetValIntByKey(EvalAttr.FK_Node);
			}
			set
			{
				this.SetValByKey(EvalAttr.FK_Node,value);
			}
		}
        /// <summary>
        ///  Node Name 
        /// </summary>
        public string NodeName
        {
            get
            {
                return this.GetValStringByKey(EvalAttr.NodeName);
            }
            set
            {
                this.SetValByKey(EvalAttr.NodeName, value);
            }
        }
        /// <summary>
        ///  The name assessors 
        /// </summary>
		public string  EvalEmpName
		{
			get
			{
				return this.GetValStringByKey(EvalAttr.EvalEmpName);
			}
			set
			{
				this.SetValByKey(EvalAttr.EvalEmpName,value);
			}
		}
        /// <summary>
        ///  Record Date 
        /// </summary>
		public string  RDT
		{
			get
			{
				return this.GetValStringByKey(EvalAttr.RDT);
			}
			set
			{
				this.SetValByKey(EvalAttr.RDT,value);
			}
		}
		/// <summary>
		///  Process membership department 
		/// </summary>
		public string FK_Dept
		{
			get
			{
				return this.GetValStringByKey(EvalAttr.FK_Dept);
			}
			set
			{
				this.SetValByKey(EvalAttr.FK_Dept,value);
			}
		}
        /// <summary>
        ///  Department name 
        /// </summary>
        public string DeptName
        {
            get
            {
                return this.GetValStringByKey(EvalAttr.DeptName);
            }
            set
            {
                this.SetValByKey(EvalAttr.DeptName, value);
            }
        }
        /// <summary>
        ///  Years of membership 
        /// </summary>
		public string  FK_NY
		{
			get
			{
				return this.GetValStringByKey(EvalAttr.FK_NY);
			}
			set
			{
				this.SetValByKey(EvalAttr.FK_NY,value);
			}
		}
        /// <summary>
        ///  Process ID 
        /// </summary>
		public string  FK_Flow
		{
			get
			{
				return this.GetValStringByKey(EvalAttr.FK_Flow);
			}
            set
            {
                this.SetValByKey(EvalAttr.FK_Flow, value);
            }
		}
        /// <summary>
        ///  Process Name 
        /// </summary>
        public string FlowName
		{
			get
			{
                return this.GetValStringByKey(EvalAttr.FlowName);
			}
			set
			{
                this.SetValByKey(EvalAttr.FlowName, value);
			}
		}
        /// <summary>
        ///  Appraiser 
        /// </summary>
		public string  Rec
		{
			get
			{
				return this.GetValStringByKey(EvalAttr.Rec);
			}
			set
			{
				this.SetValByKey(EvalAttr.Rec,value);
			}
		}
        /// <summary>
        ///  Name of Evaluation 
        /// </summary>
        public string RecName
        {
            get
            {
                return this.GetValStringByKey(EvalAttr.RecName);
            }
            set
            {
                this.SetValByKey(EvalAttr.RecName, value);
            }
        }
        /// <summary>
        ///  Evaluation Content 
        /// </summary>
        public string EvalNote
        {
            get
            {
                return this.GetValStringByKey(EvalAttr.EvalNote);
            }
            set
            {
                this.SetValByKey(EvalAttr.EvalNote, value);
            }
        }
        /// <summary>
        ///  Is the assessment of personnel numbers 
        /// </summary>
        public string EvalEmpNo
        {
            get
            {
                return this.GetValStringByKey(EvalAttr.EvalEmpNo);
            }
            set
            {
                this.SetValByKey(EvalAttr.EvalEmpNo, value);
            }
        }
        /// <summary>
        ///  Evaluation scores 
        /// </summary>
        public string EvalCent
        {
            get
            {
                return this.GetValStringByKey(EvalAttr.EvalCent);
            }
            set
            {
                this.SetValByKey(EvalAttr.EvalCent, value);
            }
        }
		#endregion 

		#region  Constructor 
		/// <summary>
		///  Evaluate the quality of work 
		/// </summary>
		public Eval()
        {
        }
        /// <summary>
        ///  Evaluate the quality of work 
        /// </summary>
        /// <param name="workid"></param>
        /// <param name="FK_Node"></param>
		public Eval(int workid, int FK_Node)
		{
			this.WorkID=workid;
			this.FK_Node=FK_Node;
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
                Map map = new Map("WF_CHEval");
                map.EnDesc = " Evaluate the quality of work ";
                map.EnType = EnType.App;

                map.AddMyPK();
                map.AddTBString(EvalAttr.Title, null, " Title ", false, true, 0, 500, 10);
                map.AddTBString(EvalAttr.FK_Flow, null, " Process ID ", false, true, 0, 7, 10);
                map.AddTBString(EvalAttr.FlowName, null, " Process Name ", false, true, 0, 100, 10);

                map.AddTBInt(EvalAttr.WorkID, 0, " The work ID", false, true);
                map.AddTBInt(EvalAttr.FK_Node, 0, " Evaluation node ", false, true);
                map.AddTBString(EvalAttr.NodeName, null, " Node Name ", false, true, 0, 100, 10);

                map.AddTBString(EvalAttr.Rec, null, " Appraiser ", false, true, 0, 200, 10);
                map.AddTBString(EvalAttr.RecName, null, " Name of Evaluation ", false, true, 0, 200, 10);

                map.AddTBDateTime(EvalAttr.RDT, " Review Date ", true, true);

                map.AddTBString(EvalAttr.EvalEmpNo, null, " Is the assessment of personnel numbers ", false, true, 0, 200, 10);
                map.AddTBString(EvalAttr.EvalEmpName, null, " Name of the person to be the assessment of ", false, true, 0, 200, 10);
                map.AddTBString(EvalAttr.EvalCent, null, " Evaluation scores ", false, true, 0, 20, 10);
                map.AddTBString(EvalAttr.EvalNote, null, " Evaluation Content ", false, true, 0, 20, 10);

                map.AddTBString(EvalAttr.FK_Dept, null, " Department ", false, true, 0, 200, 10);
                map.AddTBString(EvalAttr.DeptName, null, " Department name ", false, true, 0, 100, 10);
                map.AddTBString(EvalAttr.FK_NY, null, " Years ", false, true, 0, 7, 10);

                this._enMap = map;
                return this._enMap;
            }
        }
		#endregion
	}
	/// <summary>
	///  Evaluate the quality of work s BP.Port.FK.Evals
	/// </summary>
	public class Evals : EntitiesMyPK
	{
		#region  Method 
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new Eval();
			}
		}
		/// <summary>
        ///  Evaluate the quality of work s
		/// </summary>
		public Evals(){}
		#endregion
	}
	
}
