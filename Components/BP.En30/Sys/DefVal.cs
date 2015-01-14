using System;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.Sys
{
	 /// <summary>
	 ///  Property 
	 /// </summary>
    public class DefValAttr : EntityTreeAttr
	{
		/// <summary>
		///  Property Key
		/// </summary>
		public const string AttrKey="AttrKey";
        /// <summary>
        ///  Description 
        /// </summary>
        public const string AttrDesc = "AttrDesc";
		/// <summary>
		///  Staff ID
		/// </summary>
		public const string FK_Emp="FK_Emp";
		/// <summary>
		///  Defaults 
		/// </summary>
		public const string Val="Val";
		/// <summary>
		/// EnsName
		/// </summary>
		public const string EnsName="EnsName";
        /// <summary>
        ///  Description 
        /// </summary>
        public const string EnsDesc = "EnsDesc";
        /// <summary>
        ///  Parent node number 
        /// </summary>
        public const string ParentNo = "ParentNo";
        /// <summary>
        ///  Whether the parent node 
        /// </summary>
        public const string IsParent = "IsParent";
        /// <summary>
        ///  History Glossary ---- Added    Last used previously reserved 30 Article vocabulary 
        /// </summary>
        public const string HistoryWords = "HistoryWords";
	}
	/// <summary>
	///  Defaults 
	/// </summary>
    public class DefVal : EntityTree
	{
		#region  Basic properties 
        /// <summary>
        ///  Class name 
        /// </summary>
		public string EnsName
		{
			get
			{
				return this.GetValStringByKey(DefValAttr.EnsName ) ; 
			}
			set
			{
				this.SetValByKey(DefValAttr.EnsName,value) ; 
			}
		}
        /// <summary>
        ///  Description 
        /// </summary>
        public string EnsDesc
        {
            get
            {
                return this.GetValStringByKey(DefValAttr.EnsDesc);
            }
            set
            {
                this.SetValByKey(DefValAttr.EnsDesc, value);
            }
        }
		/// <summary>
		///  Defaults 
		/// </summary>
		public string Val
		{
			get
			{
				return this.GetValStringByKey(DefValAttr.Val ) ; 
			}
			set
			{
				this.SetValByKey(DefValAttr.Val,value) ; 
			}
		}
		/// <summary>
		///  The operator ID
		/// </summary>
		public string FK_Emp
		{
			get
			{
				return this.GetValStringByKey(DefValAttr.FK_Emp ) ; 
			}
			set
			{
				this.SetValByKey(DefValAttr.FK_Emp,value) ; 
			}
		}
		/// <summary>
		///  Property 
		/// </summary>
		public string AttrKey
		{
			get
			{
				return this.GetValStringByKey(DefValAttr.AttrKey ) ; 
			}
			set
			{
				this.SetValByKey(DefValAttr.AttrKey,value) ; 
			}
		}
        /// <summary>
        ///  Property Description 
        /// </summary>
        public string AttrDesc
        {
            get
            {
                return this.GetValStringByKey(DefValAttr.AttrDesc);
            }
            set
            {
                this.SetValByKey(DefValAttr.AttrDesc, value);
            }
        }
        /// <summary>
        ///  Whether the parent node 
        /// </summary>
        public string IsParent
        {
            get
            {
                return this.GetValStringByKey(DefValAttr.IsParent);
            }
            set
            {
                this.SetValByKey(DefValAttr.IsParent, value);
            }
        }
         /// <summary>
        ///  History Glossary 
        /// </summary>
        public string HistoryWords
        {
            get
            {
                return this.GetValStringByKey(DefValAttr.HistoryWords);
            }
            set
            {
                this.SetValByKey(DefValAttr.HistoryWords, value);
            }
        }
		#endregion

		#region  Constructor 
       
		/// <summary>
		///  Defaults 
		/// </summary>
		public DefVal()
		{
		}
		/// <summary>
		/// map
		/// </summary>
		public override Map EnMap
		{
            get
            {
                if (this._enMap != null) return this._enMap;
                Map map = new Map("Sys_DefVal");
                map.EnType = EnType.Sys;
                map.EnDesc = " Defaults ";
                map.DepositaryOfEntity = Depositary.Application;
                map.DepositaryOfMap = Depositary.Application;

                map.AddTBStringPK(DefValAttr.No, null, " Serial number ", true, true, 1, 50, 20);
                map.AddTBString(DefValAttr.EnsName, null, " The class name ", false, true, 0, 100, 10);
                map.AddTBString(DefValAttr.EnsDesc, null, " Class Description ", false, true, 0, 100, 10);

                map.AddTBString(DefValAttr.AttrKey, null, " Property ", false, true, 0, 100, 10);
                map.AddTBString(DefValAttr.AttrDesc, null, " Property Description ", false, false, 0, 100, 10);

                map.AddTBString(DefValAttr.FK_Emp, null, " Staff ", false, true, 0, 100, 10);
                map.AddTBString(DefValAttr.Val, null, "ֵ", true, false, 0, 1000, 10);
                map.AddTBString(DefValAttr.ParentNo, null, " Parent node number ", false, false,0,50,20);
                map.AddTBInt(DefValAttr.IsParent, 0, " Whether the parent node ", false, false);

               //map.AddTBInt(DefValAttr.IsParent,0," Whether the parent node ";

                map.AddTBString(DefValAttr.HistoryWords, null, " History Glossary ", false, false, 0, 2000, 20);
                this._enMap = map;
                return this._enMap;
            }
		}
		#endregion 
	}
	/// <summary>
	///  Defaults s
	/// </summary>
    public class DefVals : EntitiesNoName
	{
		/// <summary>
		///  Inquiry .
		/// </summary>
		/// <param name="EnsName"></param>
		/// <param name="key"></param>
		/// <param name="FK_Emp"></param>
        public void Retrieve(string EnsName, string key, int FK_Emp)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(DefValAttr.AttrKey, key);
            qo.addAnd();
            qo.AddWhere(DefValAttr.EnsName, EnsName);
            qo.addAnd();
            qo.AddWhere(DefValAttr.FK_Emp, FK_Emp);
            qo.DoQuery();
        }
		/// <summary>
		///  Inquiry 
		/// </summary>
		/// <param name="EnsName"></param>
		/// <param name="key"></param>
        public void Retrieve(string EnsName, string key)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(DefValAttr.AttrKey, key);
            qo.addAnd();
            qo.AddWhere(DefValAttr.EnsName, EnsName);
            qo.DoQuery();
        }
		/// <summary>
		///  Defaults s
		/// </summary>
		public DefVals()
		{
		}
		/// <summary>
		///  Get it  Entity
		/// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new DefVal();
            }
        }
	}
}
