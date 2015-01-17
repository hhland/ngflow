using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.WF;
using BP.Port;

namespace BP.Demo
{
    /// <summary>
    ///  Provincial Bureau   Property 
    /// </summary>
    public class tab_wf_commonkpiopti_mainAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  Process ID 
        /// </summary>
        public const string fk_flow = "fk_flow";
        /// <summary>
        ///  The work ID
        /// </summary>
        public const string WorkID = "WorkID";
        /// <summary>
        ///  Title 
        /// </summary>
        public const string wf_title = "wf_title";
        /// <summary>
        ///  Document Number 
        /// </summary>
        public const string wf_no = "wf_no";
        /// <summary>
        ///  Process Category 
        /// </summary>
        public const string wf_category = "wf_category";
        /// <summary>
        ///  Urgency 
        /// </summary>
        public const string wf_priority = "wf_priority";
        /// <summary>
        ///  Sponsor 
        /// </summary>
        public const string wf_send_user = "wf_send_user";
        /// <summary>
        ///  Sponsor department 
        /// </summary>
        public const string wf_send_department = "wf_send_department";
        /// <summary>
        ///  Start Time 
        /// </summary>
        public const string wf_send_time = "wf_send_time";
        /// <summary>
        ///  Sponsor phone 
        /// </summary>
        public const string wf_send_phone = "wf_send_phone";
        /// <summary>
        ///  Technical Information 
        /// </summary>
        public const string techology = "techology";
        /// <summary>
        /// KIPID
        /// </summary>
        public const string kpi_id = "kpi_id";
        /// <summary>
        /// 
        /// </summary>
        public const string threshold = "threshold";
        #endregion
    }
    /// <summary>
    ///  Provincial Bureau 
    /// </summary>
    public class tab_wf_commonkpiopti_main : EntityOID
    {
        #region  Property 
        /// <summary>
        ///  Start Time 
        /// </summary>
        public Int64 WorkID
        {
            get
            {
                return this.GetValInt64ByKey(tab_wf_commonkpiopti_mainAttr.WorkID);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpiopti_mainAttr.WorkID, value);
            }
        }
        /// <summary>
        ///  Process ID 
        /// </summary>
        public string fk_flow
        {
            get
            {
                return this.GetValStringByKey(tab_wf_commonkpiopti_mainAttr.fk_flow);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpiopti_mainAttr.fk_flow, value);
            }
        }
        /// <summary>
        ///  Sponsor 
        /// </summary>
        public string wf_send_user
        {
            get
            {
                return this.GetValStringByKey(tab_wf_commonkpiopti_mainAttr.wf_send_user);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpiopti_mainAttr.wf_send_user, value);
            }
        }
        /// <summary>
        ///  Process Category 
        /// </summary>
        public string wf_category
        {
            get
            {
                return this.GetValStringByKey(tab_wf_commonkpiopti_mainAttr.wf_category);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpiopti_mainAttr.wf_category, value);
            }
        }
        /// <summary>
        ///  Urgency 
        /// </summary>
        public int wf_priority
        {
            get
            {
                return this.GetValIntByKey(tab_wf_commonkpiopti_mainAttr.wf_priority);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpiopti_mainAttr.wf_priority, value);
            }
        }
        /// <summary>
        ///  Title 
        /// </summary>
        public string wf_title
        {
            get
            {
                return this.GetValStringByKey(tab_wf_commonkpiopti_mainAttr.wf_title);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpiopti_mainAttr.wf_title, value);
            }
        }
        /// <summary>
        ///  Document Number 
        /// </summary>
        public string wf_no
        {
            get
            {
                return this.GetValStringByKey(tab_wf_commonkpiopti_mainAttr.wf_no);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpiopti_mainAttr.wf_no, value);
            }
        }
        /// <summary>
        ///  Sponsor department 
        /// </summary>
        public string wf_send_department
        {
            get
            {
                return this.GetValStringByKey(tab_wf_commonkpiopti_mainAttr.wf_send_department);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpiopti_mainAttr.wf_send_department, value);
            }
        }
        /// <summary>
        ///  Start Time 
        /// </summary>
        public string wf_send_time
        {
            get
            {
                return this.GetValStringByKey(tab_wf_commonkpiopti_mainAttr.wf_send_time);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpiopti_mainAttr.wf_send_time, value);
            }
        }
        /// <summary>
        ///  Sponsor phone 
        /// </summary>
        public string wf_send_phone
        {
            get
            {
                return this.GetValStringByKey(tab_wf_commonkpiopti_mainAttr.wf_send_phone);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpiopti_mainAttr.wf_send_phone, value);
            }
        }
        /// <summary>
        ///  Technical Information 
        /// </summary>
        public string techology
        {
            get
            {
                return this.GetValStringByKey(tab_wf_commonkpiopti_mainAttr.techology);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpiopti_mainAttr.techology, value);
            }
        }
        /// <summary>
        /// kpi_id
        /// </summary>
        public string kpi_id
        {
            get
            {
                return this.GetValStringByKey(tab_wf_commonkpiopti_mainAttr.kpi_id);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpiopti_mainAttr.kpi_id, value);
            }
        }
        /// <summary>
        /// threshold
        /// </summary>
        public string threshold
        {
            get
            {
                return this.GetValStringByKey(tab_wf_commonkpiopti_mainAttr.threshold);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpiopti_mainAttr.threshold, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Provincial Bureau 
        /// </summary>
        public tab_wf_commonkpiopti_main()
        {
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
                Map map = new Map("tab_wf_commonkpiopti_main");
                map.EnDesc = " Provincial Bureau ";

                map.AddTBIntPKOID();

                map.AddTBInt(tab_wf_commonkpiopti_mainAttr.WorkID, 0, " The work ID", true, false);

                map.AddTBString(tab_wf_commonkpiopti_mainAttr.fk_flow, null, " Process ID ", true, false, 0, 200, 10);
                map.AddTBString(tab_wf_commonkpiopti_mainAttr.wf_title, null, " Title ", true, false, 0, 200, 10);

                map.AddTBString(tab_wf_commonkpiopti_mainAttr.wf_no, null, " Document Number ", true, false, 0, 200, 10);
                map.AddTBString(tab_wf_commonkpiopti_mainAttr.wf_category, null, " Process Category ", true, false, 0, 200, 10);
                map.AddTBString(tab_wf_commonkpiopti_mainAttr.wf_priority, null, " Urgency ", true, false, 0, 200, 10);
                map.AddTBString(tab_wf_commonkpiopti_mainAttr.wf_send_user, null, " Sponsor ", true, false, 0, 200, 10);
                map.AddTBString(tab_wf_commonkpiopti_mainAttr.wf_send_department, null, " Sponsor department ", true, false, 0, 200, 10);
                map.AddTBString(tab_wf_commonkpiopti_mainAttr.wf_send_time, null, " Start Time ", true, false, 0, 200, 10);
                map.AddTBString(tab_wf_commonkpiopti_mainAttr.wf_send_phone, null, " Sponsor phone ", true, false, 0, 200, 10);
                map.AddTBString(tab_wf_commonkpiopti_mainAttr.techology, null, " Technical Information ", true, false, 0, 200, 10);
                map.AddTBString(tab_wf_commonkpiopti_mainAttr.kpi_id, null, "kpi_id", true, false, 0, 200, 10);
                map.AddTBString(tab_wf_commonkpiopti_mainAttr.threshold, null, "threshold", true, false, 0, 200, 10);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    ///  Provincial Bureau s
    /// </summary>
    public class tab_wf_commonkpiopti_mains : EntitiesOID
    {
        #region  Method 
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new tab_wf_commonkpiopti_main();
            }
        }
        /// <summary>
        ///  Provincial Bureau s
        /// </summary>
        public tab_wf_commonkpiopti_mains() { }
        #endregion
    }
}
