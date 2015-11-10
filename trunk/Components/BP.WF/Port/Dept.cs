using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Web;

namespace BP.WF.Port
{
	/// <summary>
	///  Department property 
	/// </summary>
    public class DeptAttr : EntityNoNameAttr
    {
        /// <summary>
        ///  Parent node number 
        /// </summary>
        public const string ParentNo = "ParentNo";
        ///// <summary>
        /////  Affiliation 
        ///// </summary>
        //public const string FK_Unit = "FK_Unit";
    }
	/// <summary>
	///  Department 
	/// </summary>
	public class Dept:EntityNoName
	{
		#region  Property 
        /// <summary>
        ///  Parent node number 
        /// </summary>
        public string ParentNo
        {
            get
            {
                return this.GetValStrByKey(DeptAttr.ParentNo);
            }
            set
            {
                this.SetValByKey(DeptAttr.ParentNo, value);
            }
        }
         
		#endregion

		#region  Constructor 
		/// <summary>
		///  Department 
		/// </summary>
		public Dept(){}
		/// <summary>
		///  Department 
		/// </summary>
		/// <param name="no"> Serial number </param>
        public Dept(string no) : base(no){}
		#endregion

		#region  Overriding methods 
        /// <summary>
        /// UI Access control interface 
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
		/// Map
		/// </summary>
		public override Map EnMap
		{
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map();
                map.EnDBUrl = new DBUrl(DBUrlType.AppCenterDSN); // The database connected to the . ( The default is : AppCenterDSN )
                map.PhysicsTable = "Port_Dept";
                map.EnType = EnType.Admin;

                map.EnDesc = " Department "; // " Department ";//  Description of the entity .
                map.DepositaryOfEntity = Depositary.Application; // Entity map Storage location .
                map.DepositaryOfMap = Depositary.Application;    // Map  Storage location .

                map.AdjunctType = AdjunctType.None;

                map.AddTBStringPK(DeptAttr.No, null, " Serial number ", true, false, 1, 30, 40);
                map.AddTBString(DeptAttr.Name, null," Name ", true, false, 0, 60, 200);
                map.AddTBString(DeptAttr.ParentNo, null, " Parent node number ", true, false, 0, 30, 40);
              //  map.AddTBString(DeptAttr.FK_Unit, "1", " Affiliation ", false, false, 0, 200, 10);
                
                this._enMap = map;
                return this._enMap;
            }
		}
		#endregion
	}
	/// <summary>
	/// Collection department 
	/// </summary>
    public class Depts : EntitiesNoName
    {
        /// <summary>
        ///  Check all .
        /// </summary>
        /// <returns></returns>
        public override int RetrieveAll()
        {
            if (BP.Web.WebUser.No == "admin")
                return base.RetrieveAll();
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(DeptAttr.ParentNo, " = ", BP.Web.WebUser.FK_Dept + "%");
            return qo.DoQuery();
        }
        /// <summary>
        ///  Get a new entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new Dept();
            }
        }
        /// <summary>
        /// create ens
        /// </summary>
        public Depts()
        {
            if (BP.Web.WebUser.No == "admin")
                base.RetrieveAll();

            string fk_Dept = string.Empty;
            try { fk_Dept = BP.Web.WebUser.FK_Dept; }
            catch(Exception ){}

            if (!string.IsNullOrEmpty(fk_Dept))
            {
                QueryObject qo = new QueryObject(this);
                qo.AddWhere(DeptAttr.ParentNo, " = ", fk_Dept + "%");
                qo.DoQuery();
            }
        }
    }
}
