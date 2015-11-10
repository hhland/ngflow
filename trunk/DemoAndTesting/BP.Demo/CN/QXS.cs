using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Port;

namespace BP.CN
{
	/// <summary>
	///  City districts 
	/// </summary>
    public class QXSAttr : EntityNoNameAttr
    {
        #region  Basic properties 
        public const string FK_PQ = "FK_PQ";
        public const string FK_SF = "FK_SF";
        public const string NameS = "NameS";
        #endregion
    }
	/// <summary>
    ///  City districts 
	/// </summary>
	public class QXS :EntityNoName
	{	
		#region  Basic properties 
        public string NameS
        {
            get
            {
                return this.GetValStrByKey(QXSAttr.NameS);
            }
        }
        public string FK_PQ
        {
            get
            {
                return this.GetValStrByKey(QXSAttr.FK_PQ);
            }
        }
        public string FK_SF
        {
            get
            {
                return this.GetValStrByKey(QXSAttr.FK_SF);
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
		///  City districts 
		/// </summary>		
		public QXS(){}
		public QXS(string no):base(no)
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
                map.PhysicsTable = "CN_QXS";
                map.AdjunctType = AdjunctType.AllType;
                map.DepositaryOfMap = Depositary.Application;
                map.DepositaryOfEntity = Depositary.None;
                map.IsCheckNoLength = false;
                map.EnDesc = " City districts ";
                map.EnType = EnType.App;
                map.CodeStruct = "4";
                #endregion

                #region  Field 
                map.AddTBStringPK(QXSAttr.No, null, " Serial number ", true, false, 0, 200, 50);
                map.AddTBString(QXSAttr.Name, null, " Name ", true, false, 0, 200, 200);
                map.AddTBString(QXSAttr.NameS, null, "NameS", true, false, 0, 200, 200);


                map.AddDDLEntities(QXSAttr.FK_SF, null, " Province ", new SFs(), true);
                map.AddDDLEntities(QXSAttr.FK_PQ, null, " Area ", new PQs(), true);

                map.AddSearchAttr(QXSAttr.FK_SF);
                #endregion

                this._enMap = map;
                return this._enMap;
            }
		}
		#endregion

        /// <summary>
        ///  Gets a string contains the name of the county , If you include the number of returns it , Does not contain a default value is returned .
        /// </summary>
        /// <param name="name"> Character string </param>
        /// <param name="defVal"> Defaults </param>
        /// <returns> County code </returns>
        public static string GenerQXSNoByName(string name, string defVal)
        {
            // Fuzzy matching region .
            QXSs qxss = new QXSs();
            qxss.RetrieveAll();

            foreach (QXS qxs in qxss)
            {
                if (name.Contains(qxs.NameS))
                    return qxs.No;
            }

            SFs sfs = new SFs();
            sfs.RetrieveAll();
            foreach (SF sf in sfs)
            {
                if (name.Contains(sf.Names))
                    return sf.No;
            }

            return defVal;
        }
	}
	/// <summary>
	///  City districts 
	/// </summary>
	public class QXSs : EntitiesNoName
	{
		#region 
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new QXS();
			}
		}	
		#endregion 

		#region  Constructor 
		/// <summary>
		///  City districts s
		/// </summary>
		public QXSs(){}

        /// <summary>
        ///  City districts s
        /// </summary>
        /// <param name="sf"> Province </param>
        public QXSs(string sf)
        {
            this.Retrieve(QXSAttr.FK_SF, sf);
        }
		#endregion
	}
	
}
