using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.WF;
using BP.Port;

namespace BP.Demo
{
    /// <summary>
    ///  PUC   Property 
    /// </summary>
    public class tab_wf_commonkpioptiAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  Primary key association 
        /// </summary>
        public const string tab_wf_commonkpiopti_main = "tab_wf_commonkpiopti_main";
        /// <summary>
        ///  Process ID 
        /// </summary>
        public const string fk_flow = "fk_flow";
        /// <summary>
        /// WorkID
        /// </summary>
        public const string WorkID = "WorkID";
        /// <summary>
        ///  Parent ID
        /// </summary>
        public const string ParentWorkID = "ParentWorkID";
        /// <summary>
        /// regionid
        /// </summary>
        public const string region_id = "region_id";
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


        #endregion
    }
    /// <summary>
    ///  PUC 
    /// </summary>
    public class tab_wf_commonkpiopti : EntityOID
    {
        #region  Property 
        /// <summary>
        ///  Start Time 
        /// </summary>
        public int tab_wf_commonkpiopti_main
        {
            get
            {
                return this.GetValIntByKey(tab_wf_commonkpioptiAttr.tab_wf_commonkpiopti_main);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpioptiAttr.tab_wf_commonkpiopti_main, value);
            }
        }
        /// <summary>
        ///  Title 
        /// </summary>
        public string fk_flow
        {
            get
            {
                return this.GetValStringByKey(tab_wf_commonkpioptiAttr.fk_flow);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpioptiAttr.fk_flow, value);
            }
        }
        /// <summary>
        ///  Document Number 
        /// </summary>
        public string region_id
        {
            get
            {
                return this.GetValStringByKey(tab_wf_commonkpioptiAttr.region_id);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpioptiAttr.region_id, value);
            }
        }
        /// <summary>
        ///  Index 
        /// </summary>
        public string wf_no
        {
            get
            {
                return this.GetValStringByKey(tab_wf_commonkpioptiAttr.wf_no);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpioptiAttr.wf_no, value);
            }
        }
        /// <summary>
        ///  Council Leader 
        /// </summary>
        public string wf_category
        {
            get
            {
                return this.GetValStringByKey(tab_wf_commonkpioptiAttr.wf_category);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpioptiAttr.wf_category, value);
            }
        }
        public Int64 WorkID
        {
            get
            {
                return this.GetValInt64ByKey(tab_wf_commonkpioptiAttr.WorkID);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpioptiAttr.WorkID, value);
            }
        }
        public Int64 ParentWorkID
        {
            get
            {
                return this.GetValInt64ByKey(tab_wf_commonkpioptiAttr.ParentWorkID);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpioptiAttr.ParentWorkID, value);
            }
        }
        /// <summary>
        ///  Urgency 
        /// </summary>
        public int wf_priority
        {
            get
            {
                return this.GetValIntByKey(tab_wf_commonkpioptiAttr.wf_priority);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpioptiAttr.wf_priority, value);
            }
        }
        /// <summary>
        ///  Sender 
        /// </summary>
        public string wf_send_user
        {
            get
            {
                return this.GetValStringByKey(tab_wf_commonkpioptiAttr.wf_send_user);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpioptiAttr.wf_send_user, value);
            }
        }
        /// <summary>
        ///  People handling department 
        /// </summary>
        public string wf_send_department
        {
            get
            {
                return this.GetValStringByKey(tab_wf_commonkpioptiAttr.wf_send_department);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpioptiAttr.wf_send_department, value);
            }
        }
        /// <summary>
        ///  Time sponsor 
        /// </summary>
        public string wf_send_time
        {
            get
            {
                return this.GetValStringByKey(tab_wf_commonkpioptiAttr.wf_send_time);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpioptiAttr.wf_send_time, value);
            }
        }
        /// <summary>
        ///  Sponsor phone 
        /// </summary>
        public string wf_send_phone
        {
            get
            {
                return this.GetValStringByKey(tab_wf_commonkpioptiAttr.wf_send_phone);
            }
            set
            {
                this.SetValByKey(tab_wf_commonkpioptiAttr.wf_send_phone, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  PUC 
        /// </summary>
        public tab_wf_commonkpiopti()
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
                Map map = new Map("tab_wf_commonkpiopti");
                map.EnDesc = " PUC ";

                map.AddTBIntPKOID();

                map.AddTBInt(tab_wf_commonkpioptiAttr.WorkID, 0, "WorkID", true, false);
                map.AddTBInt(tab_wf_commonkpioptiAttr.ParentWorkID, 0, " Parent process ID", true, false);

                map.AddTBInt(tab_wf_commonkpioptiAttr.wf_priority, 0, " Urgency ", true, false);

                map.AddTBString(tab_wf_commonkpioptiAttr.tab_wf_commonkpiopti_main, null, " Associated with the provincial bureau ID", true, false, 0, 200, 10);

                map.AddTBString(tab_wf_commonkpioptiAttr.region_id, null, " Document Number ", true, false, 0, 200, 10);
                map.AddTBString(tab_wf_commonkpioptiAttr.wf_category, null, " Category ", true, false, 0, 200, 10);
                map.AddTBString(tab_wf_commonkpioptiAttr.wf_no, null, " Document Number ", true, false, 0, 200, 10);

                map.AddTBString(tab_wf_commonkpioptiAttr.wf_priority, null, " Urgency ", true, false, 0, 200, 10);
                map.AddTBString(tab_wf_commonkpioptiAttr.wf_send_user, null, " Sponsor ", true, false, 0, 200, 10);
                map.AddTBString(tab_wf_commonkpioptiAttr.wf_send_department, null, " Sponsor department ", true, false, 0, 200, 10);
                map.AddTBString(tab_wf_commonkpioptiAttr.wf_send_time, null, " Start Time ", true, false, 0, 200, 10);
                map.AddTBString(tab_wf_commonkpioptiAttr.wf_send_phone, null, " Sponsor phone ", true, false, 0, 200, 10);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    ///  PUC s
    /// </summary>
    public class tab_wf_commonkpioptis : EntitiesOID
    {
        #region  Method 
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new tab_wf_commonkpiopti();
            }
        }
        /// <summary>
        ///  PUC s
        /// </summary>
        public tab_wf_commonkpioptis() { }
        #endregion
    }
}
