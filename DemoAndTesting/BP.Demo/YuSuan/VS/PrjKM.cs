using System;
using System.Data;
using BP.DA;
using BP.En;

namespace BP.YS
{
	/// <summary>
	///  Project vs Subject 
	/// </summary>
	public class PrjKMAttr  
	{
		#region  Basic properties 
		/// <summary>
		///  Staff ID
		/// </summary>
		public const  string FK_Prj="FK_Prj";
		/// <summary>
		///  Jobs 
		/// </summary>
		public const  string FK_KM="FK_KM";		 
		#endregion	
	}
	/// <summary>
    ///  Project vs Subject   The summary .
	/// </summary>
    public class PrjKM : Entity
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
                uac.OpenForSysAdmin();
                return uac;

            }
        }
        /// <summary>
        ///  Staff ID
        /// </summary>
        public string FK_Prj
        {
            get
            {
                return this.GetValStringByKey(PrjKMAttr.FK_Prj);
            }
            set
            {
                SetValByKey(PrjKMAttr.FK_Prj, value);
            }
        }
        public string FK_KMT
        {
            get
            {
                return this.GetValRefTextByKey(PrjKMAttr.FK_KM);
            }
        }
        /// <summary>
        /// Jobs 
        /// </summary>
        public string FK_KM
        {
            get
            {
                return this.GetValStringByKey(PrjKMAttr.FK_KM);
            }
            set
            {
                SetValByKey(PrjKMAttr.FK_KM, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Work Item vs Subject 
        /// </summary> 
        public PrjKM() { }
        /// <summary>
        ///  Staff jobs corresponding 
        /// </summary>
        /// <param name="FK_Prj"> Staff ID</param>
        /// <param name="FK_KM"> Number of jobs </param> 	
        public PrjKM(string FK_Prj, string FK_KM)
        {
            this.FK_Prj = FK_Prj;
            this.FK_KM = FK_KM;
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

                Map map = new Map("YS_PrjKM");
                map.EnDesc = " Project vs Subject ";
                map.EnType = EnType.Dot2Dot;

              //  map.AddDDLEntitiesPK(PrjKMAttr.FK_Prj, null, " The operator ", new Emps(), true);
                map.AddTBStringPK(PrjKMAttr.FK_Prj, null, " Project ", true, false, 0, 100, 100);
                map.AddDDLEntitiesPK(PrjKMAttr.FK_KM, null, " Subject ", new KMs(), true);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
	/// <summary>
    ///  Project vs Subject  
	/// </summary>
	public class PrjKMs : Entities
	{
		#region  Structure 
		/// <summary>
		///  Work Item vs Subject 
		/// </summary>
		public PrjKMs()
		{
		}
		/// <summary>
		///  Staff work with collections 
		/// </summary>
		public PrjKMs(string stationNo)
		{
			QueryObject qo = new QueryObject(this);
			qo.AddWhere(PrjKMAttr.FK_KM, stationNo);
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
				return new PrjKM();
			}
		}	
		#endregion 
	}
}
