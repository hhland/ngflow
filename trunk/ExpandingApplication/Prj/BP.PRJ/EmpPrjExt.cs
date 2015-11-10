using System;
using System.Data;
using BP.DA;
using BP.Port;
using BP.En;

namespace BP.PRJ
{
	/// <summary>
	///  Project team staff 
	/// </summary>
	public class EmpPrjExtAttr 
	{
		#region  Basic properties 
		/// <summary>
		///  Staff ID
		/// </summary>
		public const  string FK_Emp="FK_Emp";
		/// <summary>
		///  Project Team 
		/// </summary>
		public const  string FK_Prj="FK_Prj";
        /// <summary>
        /// EmpPrjExt
        /// </summary>
        public const string EmpPrjExt = "EmpPrjExt";
        /// <summary>
        /// Name
        /// </summary>
        public const string Name = "Name";
        /// <summary>
        ///  Post collection 
        /// </summary>
        public const string StationStrs = "StationStrs";
		#endregion	
	}
	/// <summary>
    ///  Project team staff   The summary .
	/// </summary>
    public class EmpPrjExt : EntityMyPK
    {
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                uac.OpenForSysAdmin();
                return uac;
            }
        }

        #region  Basic properties 
        /// <summary>
        ///  Staff ID
        /// </summary>
        public string FK_Emp
        {
            get
            {
                return this.GetValStringByKey(EmpPrjExtAttr.FK_Emp);
            }
            set
            {
                SetValByKey(EmpPrjExtAttr.FK_Emp, value);
            }
        }
        public string StationStrs
        {
            get
            {
                return this.GetValStringByKey(EmpPrjExtAttr.StationStrs);
            }
            set
            {
                SetValByKey(EmpPrjExtAttr.StationStrs, value);
            }
        }
        
        public string FK_PrjT
        {
            get
            {
                return this.GetValRefTextByKey(EmpPrjExtAttr.FK_Prj);
            }
        }
        /// <summary>
        /// Project Team 
        /// </summary>
        public string FK_Prj
        {
            get
            {
                return this.GetValStringByKey(EmpPrjExtAttr.FK_Prj);
            }
            set
            {
                SetValByKey(EmpPrjExtAttr.FK_Prj, value);
            }
        }
        public string Name
        {
            get
            {
                return this.GetValStringByKey(EmpPrjExtAttr.Name);
            }
            set
            {
                SetValByKey(EmpPrjExtAttr.Name, value);
            }
        }
        #endregion

        #region  Extended Attributes 

        #endregion

        #region  Constructor 
        /// <summary>
        ///  Staff team 
        /// </summary> 
        public EmpPrjExt()
        {
        }
        /// <summary>
        ///  Counterpart staff the project team 
        /// </summary>
        /// <param name="_empoid"> Staff ID</param>
        /// <param name="wsNo"> Project group number </param> 	
        public EmpPrjExt(string _empoid, string wsNo)
        {
            this.FK_Emp = _empoid;
            this.FK_Prj = wsNo;
            if (this.Retrieve() == 0)
                this.Insert();
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

                Map map = new Map("Prj_EmpPrj");
                map.EnDesc = " Project team staff ";
                map.EnType = EnType.Dot2Dot;

                map.AddMyPK();
                map.AddTBString(EmpPrjExtAttr.Name, null, "Name", false, false, 0, 3000, 20);

                map.AddDDLEntities(EmpPrjExtAttr.FK_Prj, null, " Project Team ", new Prjs(), true);
                map.AddDDLEntities(EmpPrjExtAttr.FK_Emp, null, " Member ", new BP.WF.Port.WFEmps(), true);


                map.AddTBString(EmpPrjExtAttr.StationStrs, null, " Post collection ", true, true, 0, 4000, 20);

                map.AddSearchAttr(EmpPrjExtAttr.FK_Prj);

                map.AttrsOfOneVSM.Add(new EmpPrjStations(), new Stations(),
                    EmpPrjStationAttr.FK_EmpPrj, EmpPrjStationAttr.FK_Station,
                    DeptAttr.Name, DeptAttr.No, " Post ");

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion


        protected override bool beforeUpdate()
        {
            EmpPrjStations ens = new EmpPrjStations();
            ens.Retrieve(EmpPrjStationAttr.FK_EmpPrj, this.MyPK);

            string strs = "";
            foreach (EmpPrjStation en in ens)
            {
                strs += en.FK_StationT + ",";
            }

            this.StationStrs = strs;
            return base.beforeUpdate();
        }
    }
	/// <summary>
    ///  Project team staff 
	/// </summary>
    public class EmpPrjExts : Entities
    {
        #region  Structure 
        /// <summary>
        ///  Staff team 
        /// </summary>
        public EmpPrjExts()
        {
        }
        /// <summary>
        ///  Staff and the project team set 
        /// </summary>
        public EmpPrjExts(string GroupNo)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(EmpPrjExtAttr.FK_Prj, GroupNo);
            qo.DoQuery();
        }
        #endregion

        #region  Method 
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new EmpPrjExt();
            }
        }
        #endregion
    }
}
