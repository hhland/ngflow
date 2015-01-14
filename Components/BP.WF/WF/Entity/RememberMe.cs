
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
	///  Remember me   Property 
	/// </summary>
    public class RememberMeAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  Work node 
        /// </summary>
        public const string FK_Emp = "FK_Emp";
        /// <summary>
        ///  The current node 
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        ///  Executable staff 
        /// </summary>
        public const string Objs = "Objs";
        /// <summary>
        ///  Executable staff 
        /// </summary>
        public const string ObjsExt = "ObjsExt";
        /// <summary>
        ///  Executable personnel data 
        /// </summary>
        public const string NumOfObjs = "NumOfObjs";
        /// <summary>
        ///  Staff ги Candidate )
        /// </summary>
        public const string Emps = "Emps";
        /// <summary>
        ///  Number of staff ги Candidate )
        /// </summary>
        public const string NumOfEmps = "NumOfEmps";
        /// <summary>
        ///  Staff ги Candidate )
        /// </summary>
        public const string EmpsExt = "EmpsExt";
        #endregion
    }
	/// <summary>
	///  Remember me 
	/// </summary>
    public class RememberMe : EntityMyPK
    {
        #region  Property 
        /// <summary>
        ///  The operator 
        /// </summary>
        public string FK_Emp
        {
            get
            {
                return this.GetValStringByKey(RememberMeAttr.FK_Emp);
            }
            set
            {
                this.SetValByKey(RememberMeAttr.FK_Emp, value);
                this.MyPK = this.FK_Node + "_" + BP.Web.WebUser.No;
            }
        }
        /// <summary>
        ///  The current node 
        /// </summary>
        public int FK_Node
        {
            get
            {
                return this.GetValIntByKey(RememberMeAttr.FK_Node);
            }
            set
            {
                this.SetValByKey(RememberMeAttr.FK_Node, value);
                this.MyPK = this.FK_Node + "_" + BP.Web.WebUser.No;
            }
        }
        /// <summary>
        ///  Effective staff 
        /// </summary>
        public string Objs
        {
            get
            {
                return this.GetValStringByKey(RememberMeAttr.Objs);
            }
            set
            {
                this.SetValByKey(RememberMeAttr.Objs, value);
            }
        }
        /// <summary>
        ///  Effective operator ext
        /// </summary>
        public string ObjsExt
        {
            get
            {
                return this.GetValStringByKey(RememberMeAttr.ObjsExt);
            }
            set
            {
                this.SetValByKey(RememberMeAttr.ObjsExt, value);
            }
        }
        /// <summary>
        ///  All of the number of personnel .
        /// </summary>
        public int NumOfEmps
        {
            get
            {
                return this.Emps.Split('@').Length - 2;
            }
        }
        /// <summary>
        ///  Number of staff can handle 
        /// </summary>
        public int NumOfObjs
        {
            get
            {
                return this.Objs.Split('@').Length - 2;
            }
        }
        /// <summary>
        ///  All staff 
        /// </summary>
        public string Emps
        {
            get
            {
                return this.GetValStringByKey(RememberMeAttr.Emps);
            }
            set
            {
                this.SetValByKey(RememberMeAttr.Emps, value);
            }
        }
        /// <summary>
        ///  All staff ext
        /// </summary>
        public string EmpsExt
        {
            get
            {
                string str = this.GetValStringByKey(RememberMeAttr.EmpsExt).Trim();
                if (str.Length == 0)
                    return str;

                if (str.Substring(str.Length - 1) == ",")
                    return str.Substring(0, str.Length - 1);
                else
                    return str;
            }
            set
            {
                this.SetValByKey(RememberMeAttr.EmpsExt, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        /// RememberMe
        /// </summary>
        public RememberMe()
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
                Map map = new Map("WF_RememberMe");
                map.EnDesc = " Remember me ";
                map.EnType = EnType.Admin;

                map.AddMyPK();

                map.AddTBInt(RememberMeAttr.FK_Node, 0, " Node ", false, false);
                map.AddTBString(RememberMeAttr.FK_Emp, "", " The current operator ", true, false, 1, 30, 10);

                map.AddTBString(RememberMeAttr.Objs, "", " Assigned personnel ", true, false, 0, 4000, 10);
                map.AddTBString(RememberMeAttr.ObjsExt, "", " Assigned personnel Ext", true, false, 0, 4000, 10);

                map.AddTBString(RememberMeAttr.Emps, "", " All staff ", true, false, 0, 4000, 10);
                map.AddTBString(RememberMeAttr.EmpsExt, "", " Staff Ext", true, false, 0, 4000, 10);
                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        protected override bool beforeUpdateInsertAction()
        {
            this.FK_Emp = BP.Web.WebUser.No;
            this.MyPK = this.FK_Node + "_" + this.FK_Emp;
            return base.beforeUpdateInsertAction();
        }
    }
	/// <summary>
	///  Remember me 
	/// </summary>
	public class RememberMes: Entities
	{
		#region  Method 
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new RememberMe();
			}
		}
		/// <summary>
		/// RememberMe
		/// </summary>
		public RememberMes(){} 		 
		#endregion
	}
	
}
