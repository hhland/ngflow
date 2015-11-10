using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Port;
namespace BP.CN
{
	/// <summary>
	///  City  
	/// </summary>
    public class CityAttr : EntityNoNameAttr
    {
        #region  Basic properties 
        public const string FK_PQ = "FK_PQ";
        public const string FK_SF = "FK_SF";
        public const string Grade = "Grade";
        public const string Names = "Names";
        #endregion
    }
	/// <summary>
    ///  City 
	/// </summary>
    public class City : EntityNoName
    {
        #region  Basic properties 
        public string Names
        {
            get
            {
                return this.GetValStrByKey(CityAttr.Names);
            }
        }
        public string FK_PQ
        {
            get
            {
                return this.GetValStrByKey(CityAttr.FK_PQ);
            }
        }
        public string FK_SF
        {
            get
            {
                return this.GetValStrByKey(CityAttr.FK_SF);
            }
        }
        #endregion

        #region  Constructor 
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
        ///  City 
        /// </summary>		
        public City() { }
        public City(string no)
            : base(no)
        {
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

                #region  Basic properties 
                map.EnDBUrl = new DBUrl(DBUrlType.AppCenterDSN);
                map.PhysicsTable = "CN_City";
                map.AdjunctType = AdjunctType.AllType;
                map.DepositaryOfMap = Depositary.Application;
                map.DepositaryOfEntity = Depositary.None;
                map.IsCheckNoLength = false;
                map.EnDesc = " City ";
                map.EnType = EnType.App;
                map.CodeStruct = "4";
                #endregion

                #region  Field 
                map.AddTBStringPK(CityAttr.No, null, " Serial number ", true, false, 0, 200, 50);
                map.AddTBString(CityAttr.Name, null, " Name ", true, false, 0, 200, 200);
                map.AddTBString(CityAttr.Names, null, " Nickname ", true, false, 0, 200, 200);
                map.AddTBInt(CityAttr.Grade, 0, "Grade", false, false);

                map.AddDDLEntities(CityAttr.FK_SF, null, " Province ", new SFs(), true);
                map.AddDDLEntities(CityAttr.FK_PQ, null, " Area ", new PQs(), true);

                map.AddSearchAttr(CityAttr.FK_SF);
                #endregion

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

    }
	/// <summary>
	///  City 
	/// </summary>
	public class Citys : EntitiesNoName
	{
		#region 
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new City();
			}
		}	
		#endregion 

		#region  Constructor 
		/// <summary>
		///  City s
		/// </summary>
		public Citys(){}

        /// <summary>
        ///  City s
        /// </summary>
        /// <param name="sf"> Province </param>
        public Citys(string sf)
        {
            this.Retrieve(CityAttr.FK_SF, sf);
        }
		#endregion
	}
	
}
