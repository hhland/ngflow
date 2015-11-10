using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.WF;
using BP.Port;

namespace BP.Demo
{
    /// <summary>
    ///  Ask for leave   Property 
    /// </summary>
    public class QingJiaAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  Number of people leave 
        /// </summary>
        public const string QingJiaRenNo = "QingJiaRenNo";
        /// <summary>
        ///  Name of leave 
        /// </summary>
        public const string QingJiaRenName = "QingJiaRenName";
        /// <summary>
        ///  Department number 
        /// </summary>
        public const string QingJiaRenDeptNo = "QingJiaRenDeptNo";
        /// <summary>
        ///  Department name 
        /// </summary>
        public const string QingJiaRenDeptName = "QingJiaRenDeptName";
        /// <summary>
        ///  Leave a few days 
        /// </summary>
        public const string QingJiaTianShu = "QingJiaTianShu";
        /// <summary>
        ///  The reason for leave 
        /// </summary>
        public const string QingJiaYuanYin = "QingJiaYuanYin";
        #endregion

        #region  Audit Properties .
        public const string NoteBM = "NoteBM";
        public const string NoteZJL = "NoteZJL";
        public const string NoteRL = "NoteRL";
        #endregion  Audit Properties .

    }
    /// <summary>
    ///  Ask for leave 
    /// </summary>
    public class QingJia : EntityOID
    {
        #region  Property 
        /// <summary>
        ///  People leave the department name 
        /// </summary>
        public string QingJiaRenDeptName
        {
            get
            {
                return this.GetValStringByKey(QingJiaAttr.QingJiaRenDeptName);
            }
            set
            {
                this.SetValByKey(QingJiaAttr.QingJiaRenDeptName, value);
            }
        }
        /// <summary>
        ///  Number of people leave 
        /// </summary>
        public string QingJiaRenNo
        {
            get
            {
                return this.GetValStringByKey(QingJiaAttr.QingJiaRenNo);
            }
            set
            {
                this.SetValByKey(QingJiaAttr.QingJiaRenNo, value);
            }
        }
        /// <summary>
        ///  Name of leave 
        /// </summary>
        public string QingJiaRenName
        {
            get
            {
                return this.GetValStringByKey(QingJiaAttr.QingJiaRenName);
            }
            set
            {
                this.SetValByKey(QingJiaAttr.QingJiaRenName, value);
            }
        }
        /// <summary>
        ///  People leave the department number 
        /// </summary>
        public string QingJiaRenDeptNo
        {
            get
            {
                return this.GetValStringByKey(QingJiaAttr.QingJiaRenDeptNo);
            }
            set
            {
                this.SetValByKey(QingJiaAttr.QingJiaRenDeptNo, value);
            }
        }
        /// <summary>
        ///  The reason for leave 
        /// </summary>
        public string QingJiaYuanYin
        {
            get
            {
                return this.GetValStringByKey(QingJiaAttr.QingJiaYuanYin);
            }
            set
            {
                this.SetValByKey(QingJiaAttr.QingJiaYuanYin, value);
            }
        }
        /// <summary>
        ///  Leave a few days 
        /// </summary>
        public float QingJiaTianShu
        {
            get
            {
                return this.GetValIntByKey(QingJiaAttr.QingJiaTianShu);
            }
            set
            {
                this.SetValByKey(QingJiaAttr.QingJiaTianShu, value);
            }
        }
        /// <summary>
        ///  Department approval comments 
        /// </summary>
        public string NoteBM
        {
            get
            {
                return this.GetValStringByKey(QingJiaAttr.NoteBM);
            }
            set
            {
                this.SetValByKey(QingJiaAttr.NoteBM, value);
            }
        }
        /// <summary>
        ///  General Manager of views 
        /// </summary>
        public string NoteZJL
        {
            get
            {
                return this.GetValStringByKey(QingJiaAttr.NoteZJL);
            }
            set
            {
                this.SetValByKey(QingJiaAttr.NoteZJL, value);
            }
        }
        /// <summary>
        ///  HR advice 
        /// </summary>
        public string NoteRL
        {
            get
            {
                return this.GetValStringByKey(QingJiaAttr.NoteRL);
            }
            set
            {
                this.SetValByKey(QingJiaAttr.NoteRL, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Ask for leave 
        /// </summary>
        public QingJia()
        {
        }
        /// <summary>
        ///  Ask for leave 
        /// </summary>
        /// <param name="oid"> Entity class </param>
        public QingJia(int oid):base(oid)
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

                Map map = new Map("Demo_QingJia");
                map.EnDesc = " Ask for leave ";

                map.AddTBIntPKOID();
                map.AddTBString(QingJiaAttr.QingJiaRenNo, null, " Number of people leave ", false, false, 0, 200, 10);
                map.AddTBString(QingJiaAttr.QingJiaRenName, null, " Name of leave ", true, false, 0, 200, 70);
                map.AddTBString(QingJiaAttr.QingJiaRenDeptNo, "", " People leave the department number ", true, false, 0, 200, 50);
                map.AddTBString(QingJiaAttr.QingJiaRenDeptName, null, " People leave the department name ", true, false, 0, 200, 50);
                map.AddTBString(QingJiaAttr.QingJiaYuanYin, null, " The reason for leave ", true, false, 0, 200, 150);
                map.AddTBFloat(QingJiaAttr.QingJiaTianShu, 0, " Leave a few days ", true, false);

                //  Audit information .
                map.AddTBString(QingJiaAttr.NoteBM, null, " Department manager comments ", true, false, 0, 200, 150);
                map.AddTBString(QingJiaAttr.NoteZJL, null, " General Manager of views ", true, false, 0, 200, 150);
                map.AddTBString(QingJiaAttr.NoteRL, null, " HR advice ", true, false, 0, 200, 150);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    ///  Ask for leave s
    /// </summary>
    public class QingJias : EntitiesOID
    {
        #region  Method 
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new QingJia();
            }
        }
        /// <summary>
        ///  Ask for leave s
        /// </summary>
        public QingJias() { }
        #endregion
    }
}
