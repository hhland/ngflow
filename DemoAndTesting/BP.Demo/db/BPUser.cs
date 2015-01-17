using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.WF;
using BP.Port;

namespace BP.Demo
{
    /// <summary>
    ///  Registered Users   Property 
    /// </summary>
    public class BPUserAttr : EntityNoNameAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  Password 
        /// </summary>
        public const string Pass = "Pass";
        /// <summary>
        ///  Sex 
        /// </summary>
        public const string XB = "XB";
        /// <summary>
        ///  Address 
        /// </summary>
        public const string Addr = "Addr";
        /// <summary>
        ///  Years 
        /// </summary>
        public const string FK_NY = "FK_NY";
        /// <summary>
        ///  Years 
        /// </summary>
        public const string Age = "Age";
        /// <summary>
        ///  Mail 
        /// </summary>
        public const string Email = "Email";
        /// <summary>
        ///  Phone 
        /// </summary>
        public const string Tel = "Tel";
        /// <summary>
        ///  Registration time 
        /// </summary>
        public const string RegDate = "RegDate";
        #endregion
    }
    /// <summary>
    ///  Registered Users 
    /// </summary>
    public class BPUser : EntityNoName
    {
        #region  Property 
        /// <summary>
        ///  Password 
        /// </summary>
        public string Pass
        {
            get
            {
                return this.GetValStringByKey(BPUserAttr.Pass);
            }
            set
            {
                this.SetValByKey(BPUserAttr.Pass, value);
            }
        }
        /// <summary>
        ///  Age 
        /// </summary>
        public int Age
        {
            get
            {
                return this.GetValIntByKey(BPUserAttr.Age);
            }
            set
            {
                this.SetValByKey(BPUserAttr.Age, value);
            }
        }
        /// <summary>
        ///  Sex 
        /// </summary>
        public int XB
        {
            get
            {
                return this.GetValIntByKey(BPUserAttr.XB);
            }
            set
            {
                this.SetValByKey(BPUserAttr.XB, value);
            }
        }
        /// <summary>
        ///  Sex Name 
        /// </summary>
        public string XBText
        {
            get
            {
                return this.GetValRefTextByKey(BPUserAttr.XB);
            }
        }
        /// <summary>
        ///  Address 
        /// </summary>
        public string Addr
        {
            get
            {
                return this.GetValStringByKey(BPUserAttr.Addr);
            }
            set
            {
                this.SetValByKey(BPUserAttr.Addr, value);
            }
        }
        /// <summary>
        ///  Registration date 
        /// </summary>
        public string FK_NY
        {
            get
            {
                return this.GetValStringByKey(BPUserAttr.FK_NY);
            }
            set
            {
                this.SetValByKey(BPUserAttr.FK_NY, value);
            }
        }
        /// <summary>
        ///  Mail 
        /// </summary>
        public string Email
        {
            get
            {
                return this.GetValStringByKey(BPUserAttr.Email);
            }
            set
            {
                this.SetValByKey(BPUserAttr.Email, value);
            }
        }
        /// <summary>
        ///  Phone 
        /// </summary>
        public string Tel
        {
            get
            {
                return this.GetValStringByKey(BPUserAttr.Tel);
            }
            set
            {
                this.SetValByKey(BPUserAttr.Tel, value);
            }
        }
        /// <summary>
        ///  Registration Date 
        /// </summary>
        public string RegDate
        {
            get
            {
                return this.GetValStringByKey(BPUserAttr.RegDate);
            }
            set
            {
                this.SetValByKey(BPUserAttr.RegDate, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Registered Users 
        /// </summary>
        public BPUser()
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
                Map map = new Map("Demo_BPUser");
                map.EnDesc = " Registered Users ";
                
                //  Ordinary field 
                map.AddTBStringPK(BPUserAttr.No, null, " Username ", true, false, 1, 100, 100);
                map.AddTBString(BPUserAttr.Pass, null, " Password ", true, false, 0, 200, 10);
                map.AddTBString(BPUserAttr.Name, null, " Name ", true, false, 0, 200, 10);
                map.AddTBInt(BPUserAttr.Age, 0, " Age ", true, false);
                map.AddTBString(BPUserAttr.Addr, null, " Address ", true, false, 0, 200, 10);
                map.AddTBString(BPUserAttr.Tel, null, " Phone ", true, false, 0, 200, 10);
                map.AddTBString(BPUserAttr.Email, null, " Mail ", true, false, 0, 200, 10);
                map.AddTBDateTime(BPUserAttr.RegDate, null, " Registration Date ", true, true);

                // Enum field 
                map.AddDDLSysEnum(BPUserAttr.XB, 0, " Sex ", false, true, BPUserAttr.XB, "@0=Å®@1=ÄÐ");

                // Foreign key field .
                map.AddDDLEntities(BPUserAttr.FK_NY, null, " Years of membership ", new BP.Pub.NYs(),true);


                // Set the search criteria .
                map.AddSearchAttr(BPUserAttr.XB);
                map.AddSearchAttr(BPUserAttr.FK_NY);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion


        /// <summary>
        ///  Override the base class methods .
        /// </summary>
        /// <returns></returns>
        protected override bool beforeInsert()
        {
            // Before inserting the set registration time .
            this.RegDate = DataType.CurrentDataTime;

            return base.beforeInsert();
        }
    }
    /// <summary>
    ///  Registered Users s
    /// </summary>
    public class BPUsers : EntitiesNoName
    {
        #region  Method 
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new BPUser();
            }
        }
        /// <summary>
        ///  Registered Users s
        /// </summary>
        public BPUsers() { }
        #endregion
    }
}
