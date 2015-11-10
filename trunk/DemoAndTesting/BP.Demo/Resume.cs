using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.WF;
using BP.Port;

namespace BP.Demo
{
    /// <summary>
    ///  CV   Property 
    /// </summary>
    public class ResumeAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  Work node 
        /// </summary>
        public const string FK_Emp = "FK_Emp";
        /// <summary>
        ///  Host computer 
        /// </summary>
        public const string GongZuoDanWei = "GongZuoDanWei";
        /// <summary>
        ///  Host computer 
        /// </summary>
        public const string ZhengMingRen = "ZhengMingRen";
        /// <summary>
        ///  Staff ги Candidate )
        /// </summary>
        public const string BeiZhu = "BeiZhu";
        /// <summary>
        ///  Years 
        /// </summary>
        public const string NianYue = "NianYue";
        #endregion
    }
    /// <summary>
    ///  CV 
    /// </summary>
    public class Resume : EntityOID
    {
        #region  Property 
        /// <summary>
        ///  Years 
        /// </summary>
        public string NianYue
        {
            get
            {
                return this.GetValStringByKey(ResumeAttr.NianYue);
            }
            set
            {
                this.SetValByKey(ResumeAttr.NianYue, value);
            }
        }
        /// <summary>
        ///  Staff 
        /// </summary>
        public string FK_Emp
        {
            get
            {
                return this.GetValStringByKey(ResumeAttr.FK_Emp);
            }
            set
            {
                this.SetValByKey(ResumeAttr.FK_Emp, value);
            }
        }
        /// <summary>
        ///  Workplace 
        /// </summary>
        public string GongZuoDanWei
        {
            get
            {
                return this.GetValStringByKey(ResumeAttr.GongZuoDanWei);
            }
            set
            {
                this.SetValByKey(ResumeAttr.GongZuoDanWei, value);
            }
        }
        /// <summary>
        ///  Certifier 
        /// </summary>
        public string ZhengMingRen
        {
            get
            {
                return this.GetValStringByKey(ResumeAttr.ZhengMingRen);
            }
            set
            {
                this.SetValByKey(ResumeAttr.ZhengMingRen, value);
            }
        }
        /// <summary>
        ///  Remark 
        /// </summary>
        public string BeiZhu
        {
            get
            {
                return this.GetValStringByKey(ResumeAttr.BeiZhu);
            }
            set
            {
                this.SetValByKey(ResumeAttr.BeiZhu, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  CV 
        /// </summary>
        public Resume()
        {
        }
        /// <summary>
        ///  CV 
        /// </summary>
        /// <param name="oid"> Entity class </param>
        public Resume(int oid):base(oid)
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
                Map map = new Map("Demo_Resume");
                map.EnDesc = " CV ";

                map.AddTBIntPKOID();
                map.AddTBString(ResumeAttr.FK_Emp, null, " Staff ", false, false, 0, 200, 10);
                map.AddTBString(ResumeAttr.NianYue, null, " Years ", true, false, 0, 200, 50);
                map.AddTBString(ResumeAttr.GongZuoDanWei, null, " Workplace ", true, false, 0, 200, 70);
                map.AddTBString(ResumeAttr.ZhengMingRen, "", " Certifier ", true, false, 0, 200, 50);
                map.AddTBString(ResumeAttr.BeiZhu, null, " Remark ", true, false, 0, 200, 150);

                map.AddTBString("QT", null, " Other ", true, false, 0, 200, 150);


                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    ///  CV s
    /// </summary>
    public class Resumes : EntitiesOID
    {
        #region  Method 
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new Resume();
            }
        }
        /// <summary>
        ///  CV s
        /// </summary>
        public Resumes() { }
        #endregion
    }
}
