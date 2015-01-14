
using System;
using System.Data;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.Sys
{
    public class GEEntityWordFrmAttr
    {
        /// <summary>
        ///  File Path 
        /// </summary>
        public const string FilePath="FilePath";
        /// <summary>
        ///  Record Time 
        /// </summary>
        public const string RDT="RDT";
        /// <summary>
        ///  Last Modified people 
        /// </summary>
        public const string LastEditer="LastEditer";

        public const string OID="OID";

    }
    /// <summary>
    ///  General entity 
    /// </summary>
    public class GEEntityWordFrm  : Entity
    {
#region  Property .
        public int OID
        {
            get
            {
               return this.GetValIntByKey(GEEntityWordFrmAttr.OID);
            }
            set
            {
                this.SetValByKey(GEEntityWordFrmAttr.OID,value);
            }
        }
        /// <summary>
        ///  Last Modified people 
        /// </summary>
          public string LastEditer
        {
            get
            {
               return this.GetValStringByKey(GEEntityWordFrmAttr.LastEditer);
            }
            set
            {
                this.SetValByKey(GEEntityWordFrmAttr.LastEditer,value);
            }
        }
        /// <summary>
        ///  Record Time 
        /// </summary>
           public string RDT
        {
            get
            {
               return this.GetValStringByKey(GEEntityWordFrmAttr.RDT);
            }
            set
            {
                this.SetValByKey(GEEntityWordFrmAttr.RDT,value);
            }
        }

        /// <summary>
        ///  File Path 
        /// </summary>
           public string FilePath
           {
               get
               {
                   return this.GetValStringByKey(GEEntityWordFrmAttr.FilePath);
               }
               set
               {
                   this.SetValByKey(GEEntityWordFrmAttr.FilePath, value);
               }
           }
#endregion  Property .


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
        public GEEntityWordFrm()
        {
        }
        /// <summary>
        ///  General entity 
        /// </summary>
        /// <param name="nodeid"> Node ID</param>
        public GEEntityWordFrm(string fk_mapdata)
        {
            this.FK_MapData = fk_mapdata;
        }
        /// <summary>
        ///  General entity 
        /// </summary>
        /// <param name="nodeid"> Node ID</param>
        /// <param name="_oid">OID</param>
        public GEEntityWordFrm(string fk_mapdata, object pk)
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
                    return new GEEntityWordFrms();
                return new GEEntityWordFrms(this.FK_MapData);
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
    public class GEEntityWordFrms : EntitiesOID
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
        public GEEntityWordFrms()
        {
        }
        /// <summary>
        ///  General entity ID
        /// </summary>
        /// <param name="fk_mapdtl"></param>
        public GEEntityWordFrms(string fk_mapdata)
        {
            this.FK_MapData = fk_mapdata;
        }
        #endregion
    }
}
