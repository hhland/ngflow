using System;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.En
{
	/// <summary>
	///  Property 
	/// </summary>
	public class GENoNameAttr : EntityNoNameAttr
	{

    }
	/// <summary>
	/// 
	/// </summary>
    public class GENoName : EntityNoName
    {
        #region  Structure 
        public override string ToString()
        {
            return this.PhysicsTable;
        }
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                uac.OpenForSysAdmin();
                return uac;
            }
        }
        public GENoName()
        {

        }
        /// <summary>
        ///  Serial number 
        /// </summary>
        /// <param name="no"> Serial number </param>
        public GENoName(string no)  :base(no)
        {

        }
        public GENoName(string sftable, string tableDesc)
        {
            this.PhysicsTable = sftable;
            this.Desc = tableDesc;
        }
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null) return this._enMap;
                Map map = new Map(this.PhysicsTable);
                map.EnDesc = this.Desc;
                map.IsAutoGenerNo = true;

                map.DepositaryOfEntity = Depositary.Application;
                map.DepositaryOfMap = Depositary.Application;
                map.EnType = EnType.App;
                map.IsAutoGenerNo = true;

                map.AddTBStringPK(GENoNameAttr.No, null, " Serial number ", true, true, 1, 30, 3);
                map.AddTBString(GENoNameAttr.Name, null, " Name ", true, false, 1, 60, 500);
                //return map;
                this._enMap = map;
                return this._enMap;
            }
        }
        public string PhysicsTable = null;
        public string Desc = null;

        #endregion
    }
	/// <summary>
	/// GENoNames
	/// </summary>
	public class GENoNames : EntitiesNoName
	{
        /// <summary>
        ///  Physical table 
        /// </summary>
        public string SFTable = null;
        public string Desc = null;

		/// <summary>
		/// GENoNames
		/// </summary>
        public GENoNames()
		{
		}
        public GENoNames(string sftable, string tableDesc)
        {
            this.SFTable = sftable;
            this.Desc = tableDesc;
        }
        public override Entity GetNewEntity
        {
            get 
            {
                return new GENoName(this.SFTable, this.Desc);
            }
        }
        public override int RetrieveAll()
        {
            return this.RetrieveAllFromDBSource();
        }
	}
}
