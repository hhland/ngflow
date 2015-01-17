using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Port;

namespace BP.Demo
{
    /// <summary>
    ///  Student   Property 
    /// </summary>
    public class StudentAttr : EntityNoNameAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  Sex 
        /// </summary>
        public const string XB = "XB";
        /// <summary>
        ///  Address 
        /// </summary>
        public const string Addr = "Addr";
        /// <summary>
        ///  Class 
        /// </summary>
        public const string FJ_BanJi = "FJ_BanJi";
        /// <summary>
        ///  Class 
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
        /// <summary>
        ///  Remark 
        /// </summary>
        public const string Note = "Note";
        #endregion
    }
    /// <summary>
    ///  Student 
    /// </summary>
    public class Student : EntityNoName
    {
        #region  Property 
        /// <summary>
        ///  Age 
        /// </summary>
        public int Age
        {
            get
            {
                return this.GetValIntByKey(StudentAttr.Age);
            }
            set
            {
                this.SetValByKey(StudentAttr.Age, value);
            }
        }
        /// <summary>
        ///  Address 
        /// </summary>
        public string Addr
        {
            get
            {
                return this.GetValStringByKey(StudentAttr.Addr);
            }
            set
            {
                this.SetValByKey(StudentAttr.Addr, value);
            }
        }

        /// <summary>
        ///  Sex 
        /// </summary>
        public int XB
        {
            get
            {
                return this.GetValIntByKey(StudentAttr.XB);
            }
            set
            {
                this.SetValByKey(StudentAttr.XB, value);
            }
        }
        /// <summary>
        ///  Sex Name 
        /// </summary>
        public string XBText
        {
            get
            {
                return this.GetValRefTextByKey(StudentAttr.XB);
            }
        }

        /// <summary>
        ///  Number of classes 
        /// </summary>
        public string FJ_BanJi
        {
            get
            {
                return this.GetValStringByKey(StudentAttr.FJ_BanJi);
            }
            set
            {
                this.SetValByKey(StudentAttr.FJ_BanJi, value);
            }
        }
          /// <summary>
        ///  Class name 
        /// </summary>
        public string FJ_BanJiText
        {
            get
            {
                return this.GetValRefTextByKey(StudentAttr.FJ_BanJi);
            }
        }
        /// <summary>
        ///  Mail 
        /// </summary>
        public string Email
        {
            get
            {
                return this.GetValStringByKey(StudentAttr.Email);
            }
            set
            {
                this.SetValByKey(StudentAttr.Email, value);
            }
        }
        /// <summary>
        ///  Phone 
        /// </summary>
        public string Tel
        {
            get
            {
                return this.GetValStringByKey(StudentAttr.Tel);
            }
            set
            {
                this.SetValByKey(StudentAttr.Tel, value);
            }
        }
        /// <summary>
        ///  Registration Date 
        /// </summary>
        public string RegDate
        {
            get
            {
                return this.GetValStringByKey(StudentAttr.RegDate);
            }
            set
            {
                this.SetValByKey(StudentAttr.RegDate, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Student 
        /// </summary>
        public Student()
        {
        }
        /// <summary>
        ///  Student 
        /// </summary>
        /// <param name="no"></param>
        public Student(string no):base(no)
        {
        }
        #endregion

        #region  Override the base class methods 
        /// <summary>
        ///  Override the base class methods 
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;
                Map map = new Map();

                // Basic Information .
                map.EnDesc = " Student ";
                map.PhysicsTable = "Demo_Student";
                map.IsAllowRepeatName = true; // Whether to allow duplicate names .
                map.IsAutoGenerNo = true; // Whether to automatically generate numbers .
                map.CodeStruct = "4"; // 4 Digit number ,从0001  Begin ,到 9999.

                //  Ordinary field 
                map.AddTBStringPK(StudentAttr.No, null, " Student ID ", true, true, 4, 4, 4); //  If you set the auto number field must be read-only .
                map.AddTBString(StudentAttr.Name, null, " Name ", true, false, 0, 200, 70);
                map.AddTBString(StudentAttr.Addr, null, " Address ", true, false, 0, 200, 100,true);
                map.AddTBInt(StudentAttr.Age, 0, " Age ", true, false);
                map.AddTBString(StudentAttr.Tel, null, " Phone ", true, false, 0, 200, 60);
                map.AddTBString(StudentAttr.Email, null, " Mail ", true, false, 0, 200, 50);
                map.AddTBDateTime(StudentAttr.RegDate, null, " Registration Date ", true, true);
                map.AddTBStringDoc(StudentAttr.Note, null, " Remark ", true, false, true); // Large fast text box .

                // Enum field 
                map.AddDDLSysEnum(StudentAttr.XB, 0, " Sex ", true, true,StudentAttr.XB, "@0=女@1=男");

                // Foreign key field .
                map.AddDDLEntities(StudentAttr.FJ_BanJi, null, 
                    " Class ", new BP.Demo.BanJis(), true);

                // Set the search criteria .
                map.AddSearchAttr(StudentAttr.XB);
                map.AddSearchAttr(StudentAttr.FJ_BanJi);

                // #NAME? .
                map.AttrsOfOneVSM.Add(new StudentKeMus(), new KeMus(), StudentKeMuAttr.FK_Student,
                  StudentKeMuAttr.FK_KeMu, KeMuAttr.Name, KeMuAttr.No, " Study subjects ");

                // List map .
                map.AddDtl(new Resumes(), ResumeAttr.FK_Emp);

                // Method with parameters .
                RefMethod rm = new RefMethod();
                rm.Title = " Pay the class fee ";
                rm.HisAttrs.AddTBDecimal("JinE", 100, " Payment amount ", true, false);
                rm.HisAttrs.AddTBString("Note", null, " Remark ", true, false,0,100,100);
                rm.ClassMethodName = this.ToString() + ".DoJiaoNaBanFei";
                map.AddRefMethod(rm);

                // Method with no arguments .
                rm = new RefMethod();
                rm.Title = " Cancellation of school ";
                rm.ClassMethodName = this.ToString() + ".DoZhuXiao";
                map.AddRefMethod(rm);

                this._enMap = map;
                return this._enMap;
            }
        }
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
        protected override bool beforeDelete()
        {
            return base.beforeDelete();
        }
        protected override bool beforeUpdate()
        {
            return base.beforeUpdate();
        }
        protected override void afterInsertUpdateAction()
        {
            base.afterInsertUpdateAction();
        }
        protected override void afterDelete()
        {
            base.afterDelete();
        }
        protected override void afterInsert()
        {
            base.afterInsert();
        }
        protected override void afterUpdate()
        {
            base.afterUpdate();
        }
        #endregion  Override the base class methods 

        #region  Method 
        /// <summary>
        ///  Method with parameters : Pay the class fee 
        ///  Explanation : Must return string Type .
        /// </summary>
        /// <returns></returns>
        public string DoJiaoNaBanFei(decimal jine, string note)
        {
            return " Student ID :"+this.No+", Full name :"+this.Name+", Paid :"+jine+"元, Explanation :"+note;
        }
        /// <summary>
        ///  No method parameters : Cancellation of school 
        ///  Explanation : Must return string Type .
        /// </summary>
        /// <returns></returns>
        public string DoZhuXiao()
        {
            return " Student ID :" + this.No + ", Full name :" + this.Name + ", Has been canceled .";
        }
        #endregion

    }
    /// <summary>
    ///  Student s
    /// </summary>
    public class Students : BP.En.EntitiesNoName
    {
        #region  Method 
        /// <summary>
        ///  Student s
        /// </summary>
        public Students() { }
        #endregion

        #region  Override the base class methods 
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new Student();
            }
        }
        #endregion  Override the base class methods 

    }
}
