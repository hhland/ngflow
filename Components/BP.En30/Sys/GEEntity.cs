
using System;
using System.Data;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.Sys
{
    /// <summary>
    ///  General entity 
    /// </summary>
    public class GEEntity : Entity
    {
        #region  Constructor 
        public override string PK
        {
            get
            {
                return "OID";
            }
        }
        public override string PKField
        {
            get
            {
                return "OID";
            }
        }
        public override string ToString()
        {
            return this.FK_MapData;
        }
        public override string ClassID
        {
            get
            {
                return this.FK_MapData;
            }
        }
        /// <summary>
        ///  Primary key 
        /// </summary>
        public string FK_MapData = null;
        /// <summary>
        ///  General entity 
        /// </summary>
        public GEEntity()
        {
        }
        /// <summary>
        ///  General entity 
        /// </summary>
        /// <param name="nodeid"> Node ID</param>
        public GEEntity(string fk_mapdata)
        {
            this.FK_MapData = fk_mapdata;
        }
        /// <summary>
        ///  General entity 
        /// </summary>
        /// <param name="nodeid"> Node ID</param>
        /// <param name="_oid">OID</param>
        public GEEntity(string fk_mapdata, object pk)
        {
            this.FK_MapData = fk_mapdata;
            this.PKVal = pk;
            this.Retrieve();
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

                if (this.FK_MapData == null)
                    throw new Exception(" Did not give " + this.FK_MapData + "ֵ, You can not get it Map.");

                this._enMap = BP.Sys.MapData.GenerHisMap(this.FK_MapData);
                return this._enMap;
            }
        }
        /// <summary>
        /// GEEntitys
        /// </summary>
        public override Entities GetNewEntities
        {
            get
            {
                if (this.FK_MapData == null)
                    return new GEEntitys();
                return new GEEntitys(this.FK_MapData);
            }
        }
        #endregion

        private ArrayList _Dtls = null;
        public ArrayList Dtls
        {
            get
            {
                if (_Dtls == null)
                    _Dtls = new ArrayList();
                return _Dtls;
            }
        }
    }
    /// <summary>
    ///  General entity s
    /// </summary>
    public class GEEntitys : EntitiesOID
    {
        #region  Override the base class methods 
        public override string ToString()
        {
            //if (this.FK_MapData == null)
            //    throw new Exception("@ Not able  FK_MapData  To value .");
            return this.FK_MapData;
        }
        /// <summary>
        ///  Primary key 
        /// </summary>
        public string FK_MapData = null;
        #endregion

        #region  Method 
        /// <summary>
        ///  Get it  Entity
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                //if (this.FK_MapData == null)
                //    throw new Exception("@ Not able  FK_MapData  To value .");

                if (this.FK_MapData == null)
                    return new GEEntity();
                return new GEEntity(this.FK_MapData);
            }
        }
        /// <summary>
        ///  General entity ID
        /// </summary>
        public GEEntitys()
        {
        }
        /// <summary>
        ///  General entity ID
        /// </summary>
        /// <param name="fk_mapdtl"></param>
        public GEEntitys(string fk_mapdata)
        {
            this.FK_MapData = fk_mapdata;
        }
        #endregion
    }
}
