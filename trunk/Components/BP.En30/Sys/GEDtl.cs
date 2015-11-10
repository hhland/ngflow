
using System;
using System.Data;
using BP.DA;
using BP.En;

namespace BP.Sys
{
	/// <summary>
	///  Common Table 
	/// </summary>
    public class GEDtlAttr : EntityOIDAttr
    {
        public const string RefPK = "RefPK";
        public const string FID = "FID";
        public const string Rec = "Rec";
        public const string RDT = "RDT";
        /// <summary>
        ///  OK if you want to lock 
        /// </summary>
        public const string IsRowLock = "IsRowLock";

    }
    /// <summary>
    ///  Common Table 
    /// </summary>
    public class GEDtl : EntityOID
    {
        #region  Constructor 
        public override string ToString()
        {
            return this.FK_MapDtl;
        }
        public override string ClassID
        {
            get
            {
                return this.FK_MapDtl;
            }
        }
        public string RDT
        {
            get
            {
                return this.GetValStringByKey(GEDtlAttr.RDT);
            }
            set
            {
                this.SetValByKey(GEDtlAttr.RDT, value);
            }
        }
        public string Rec
        {
            get
            {
                return this.GetValStringByKey(GEDtlAttr.Rec);
            }
            set
            {
                this.SetValByKey(GEDtlAttr.Rec, value);
            }
        }
        /// <summary>
        ///  Associated PKֵ
        /// </summary>
        public string RefPK
        {
            get
            {
                return this.GetValStringByKey(GEDtlAttr.RefPK);
            }
            set
            {
                this.SetValByKey(GEDtlAttr.RefPK, value);
            }
        }
        public Int64 RefPKInt64
        {
            get
            {
                return this.GetValInt64ByKey(GEDtlAttr.RefPK);
            }
            set
            {
                this.SetValByKey(GEDtlAttr.RefPK, value);
            }
        }
        /// <summary>
        ///  Row is locked 
        /// </summary>
        public bool IsRowLock
        {
            get
            {
                return this.GetValBooleanByKey(GEDtlAttr.IsRowLock);
            }
            set
            {
                this.SetValByKey(GEDtlAttr.IsRowLock, value);
            }
        }
        /// <summary>
        ///  Associated PKint
        /// </summary>
        public int RefPKInt
        {
            get
            {
                return this.GetValIntByKey(GEDtlAttr.RefPK);
            }
            set
            {
                this.SetValByKey(GEDtlAttr.RefPK, value);
            }
        }
        public Int64 FID
        {
            get
            {
                return this.GetValInt64ByKey(GEDtlAttr.FID);
            }
            set
            {
                this.SetValByKey(GEDtlAttr.FID, value);
            }
        }
        /// <summary>
        ///  Primary key 
        /// </summary>
        public string FK_MapDtl = null;
        /// <summary>
        ///  Common Table 
        /// </summary>
        public GEDtl()
        {
        }
        /// <summary>
        ///  Common Table 
        /// </summary>
        /// <param name="nodeid"> Node ID</param>
        public GEDtl(string fk_mapdtl)
        {
            this.FK_MapDtl = fk_mapdtl;
        }
        /// <summary>
        ///  Common Table 
        /// </summary>
        /// <param name="nodeid"> Node ID</param>
        /// <param name="_oid">OID</param>
        public GEDtl(string fk_mapdtl, int _oid)
        {
            this.FK_MapDtl = fk_mapdtl;
            this.OID = _oid;
        }
        #endregion

        #region Map
        /// <summary>
        ///  Override the base class methods 
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                if (this.FK_MapDtl == null)
                    throw new Exception(" Did not give " + this.FK_MapDtl + "ֵ, You can not get it Map.");

                BP.Sys.MapDtl md = new BP.Sys.MapDtl(this.FK_MapDtl);
                this._enMap = md.GenerMap();
                return this._enMap;
            }
        }
        /// <summary>
        /// GEDtls
        /// </summary>
        public override Entities GetNewEntities
        {
            get
            {
                if (this.FK_MapDtl == null)
                    return new GEDtls();

                return new GEDtls(this.FK_MapDtl);
            }
        }
        public bool IsChange(GEDtl dtl)
        {
            Attrs attrs = dtl.EnMap.Attrs;
            foreach (Attr attr in attrs)
            {
                if (this.GetValByKey(attr.Key) == dtl.GetValByKey(attr.Key))
                    continue;
                else
                    return true;
            }
            return false;
        }
        protected override bool beforeUpdate()
        {
            this.AutoFull(); /* Automatic calculation processing .*/
            return base.beforeUpdate();
        }
        /// <summary>
        ///  Record people 
        /// </summary>
        /// <returns></returns>
        protected override bool beforeInsert()
        {
            //  Determines whether there is a change of the project , Decide whether to perform store .
            MapAttrs mattrs = new MapAttrs(this.FK_MapDtl);
            bool isC = false;
            foreach (MapAttr mattr in mattrs)
            {
                if (isC)
                    break;
                switch (mattr.KeyOfEn)
                {
                    case "Rec":
                    case "RDT":
                    case "RefPK":
                    case "FID":
                        break;
                    default:
                        if (mattr.IsNum)
                        {
                            string s = this.GetValStrByKey(mattr.KeyOfEn);
                            if (string.IsNullOrEmpty(s))
                            {
                                this.SetValByKey(mattr.KeyOfEn, mattr.DefVal);
                                s = mattr.DefVal.ToString();
                            }

                            if (decimal.Parse(s) == mattr.DefValDecimal)
                                continue;
                            isC = true;
                            break;
                        }
                        else
                        {
                            if (this.GetValStrByKey(mattr.KeyOfEn) == mattr.DefVal)
                                continue;
                            isC = true;
                            break;
                        }
                        break;
                }
            }
            if (isC == false)
                return false;

            this.Rec = BP.Web.WebUser.No;
            this.RDT = DataType.CurrentDataTime;

            this.AutoFull(); /* Automatic calculation processing .*/
            return base.beforeInsert();
        }
        #endregion
    }
    /// <summary>
    ///  Common Table s
    /// </summary>
    public class GEDtls : EntitiesOID
    {
        #region  Override the base class methods 
        /// <summary>
        ///  Node ID
        /// </summary>
        public string FK_MapDtl = null;
        #endregion

        #region  Method 
        /// <summary>
        ///  Get it  Entity
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                if (this.FK_MapDtl == null)
                    return new GEDtl();
                return new GEDtl(this.FK_MapDtl);
            }
        }
        /// <summary>
        ///  Common Table ID
        /// </summary>
        public GEDtls()
        {
        }
        /// <summary>
        ///  Common Table ID
        /// </summary>
        /// <param name="fk_mapdtl"></param>
        public GEDtls(string fk_mapdtl)
        {
            this.FK_MapDtl = fk_mapdtl;
        }
        #endregion
    }
}
