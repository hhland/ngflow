using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Port;

namespace BP.Demo
{
	/// <summary>
	///  Product 
	/// </summary>
	public class ProductAttr: EntityNoNameAttr
	{
		#region  Basic properties 
		public const  string FK_SF="FK_SF";
        public const string Addr = "Addr";

        public const string Price = "Price";


		#endregion
	}
	/// <summary>
    ///  Product 
	/// </summary>
    public class Product : EntityNoName
    {
        #region  Basic properties 
        public string Addr
        {
            get
            {
                return this.GetValStringByKey(ProductAttr.Addr);
            }
            set
            {
                this.SetValByKey(ProductAttr.Addr, value);
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
        ///  Product 
        /// </summary>		
        public Product() { }
        /// <summary>
        ///  Product 
        /// </summary>
        /// <param name="no"></param>
        public Product(string no)
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
                map.PhysicsTable = "Demo_Product";
                map.AdjunctType = AdjunctType.AllType;
                map.DepositaryOfMap = Depositary.Application;
                map.DepositaryOfEntity = Depositary.None;
                map.IsCheckNoLength = false;
                map.EnDesc = " Product ";
                map.EnType = EnType.App;
                map.CodeStruct = "4";
                #endregion

                #region  Field 
                map.AddTBStringPK(ProductAttr.No, null, " Serial number ", true, false, 0, 200, 50);
                map.AddTBString(ProductAttr.Name, null, " Name ", true, false, 0, 200, 200);
                map.AddTBString(ProductAttr.Addr, null, " Production Address ", true, false, 0, 200, 200);
                map.AddTBFloat(ProductAttr.Price, 0, " Price ", true, false);
                #endregion

                this._enMap = map;
                return this._enMap;
            }
        }
        public override Entities GetNewEntities
        {
            get { return new Products(); }
        }
        #endregion
    }
	/// <summary>
	///  Product 
	/// </summary>
	public class Products : EntitiesNoName
	{
		#region 
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new Product();
			}
		}	
		#endregion 

		#region  Constructor 
		/// <summary>
		///  Product s
		/// </summary>
		public Products(){}
		#endregion
	}
	
}
