using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Port;

namespace BP.CN
{
	/// <summary>
	///  City coding 
	/// </summary>
    public class AreaAttr : EntityNoNameAttr
    {
        #region  Basic properties 
        public const string FK_PQ = "FK_PQ";
        public const string FK_SF = "FK_SF";
        public const string Grade = "Grade";
        public const string Names = "Names";
        #endregion
    }
	/// <summary>
    ///  City coding 
	/// </summary>
	public class Area :EntityNoName
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
                return this.GetValStrByKey(AreaAttr.FK_PQ);
            }
        }
        public string FK_SF
        {
            get
            {
                return this.GetValStrByKey(AreaAttr.FK_SF);
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
		///  City coding 
		/// </summary>		
		public Area(){}
		public Area(string no):base(no)
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
                map.PhysicsTable = "CN_Area";
                map.AdjunctType = AdjunctType.AllType;
                map.DepositaryOfMap = Depositary.Application;
                map.DepositaryOfEntity = Depositary.None;
                map.IsCheckNoLength = false;
                map.EnDesc = " City coding ";
                map.EnType = EnType.App;
                map.CodeStruct = "4";
                #endregion

                #region  Field 
                map.AddTBStringPK(AreaAttr.No, null, " Serial number ", true, false, 0, 200, 50);
                map.AddTBString(AreaAttr.Name, null, " Name ", true, false, 0, 200, 200);
                map.AddTBString(AreaAttr.Names, null, " Nickname ", true, false, 0, 200, 200);
                map.AddTBInt(AreaAttr.Grade, 0, "Grade", false, false);

                map.AddDDLEntities(AreaAttr.FK_SF, null, " Province ", new SFs(), true);
                map.AddDDLEntities(AreaAttr.FK_PQ, null, " Area ", new PQs(), true);

                map.AddSearchAttr(AreaAttr.FK_SF);
                #endregion

                this._enMap = map;
                return this._enMap;
            }
		}
		#endregion

        public static string GenerAreaNoByName(string name1, string name2, string oldcity)
        {
            string fk_city1 = BP.CN.Area.GenerAreaNoByName(name1 , "");
            string fk_city2 = BP.CN.Area.GenerAreaNoByName(name2 , "");
            string fk_city = null;

            if (fk_city1.Length >= 4)
            {
                fk_city = fk_city1;
            }

            if (fk_city1.Length == 2)
            {
                if (fk_city2.Contains(fk_city1))
                    fk_city = fk_city2;
                else
                    fk_city = fk_city1;
            }
            return fk_city;
        }

        public static string GenerAreaNoByName(string name, string oldcity)
        {
            // Fuzzy matching region , Find someone counties .
            string sql = "SELECT NO FROM CN_Area WHERE indexof('" + name + "', names ) >0 ORDER BY GRADE DESC ";
            string val = DBAccess.RunSQLReturnString(sql);
            if (val != null)
                return val;
            else
                return oldcity;
        }
	}
	/// <summary>
	///  City coding 
	/// </summary>
    public class Areas : EntitiesNoName
    {
        #region   Get it  Entity
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new Area();
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  City coding s
        /// </summary>
        public Areas() { }
        /// <summary>
        ///  City coding s
        /// </summary>
        /// <param name="sf"> Province </param>
        public Areas(string sf)
        {
            this.Retrieve(AreaAttr.FK_SF, sf);
        }
        #endregion
    }
}
