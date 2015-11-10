using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Web;

namespace BP.GPM
{
    /// <summary>
    ///  Department property 
    /// </summary>
    public class DeptAttr : EntityTreeAttr
    {
        /// <summary>
        ///  Sector Type 
        /// </summary>
        public const string FK_DeptType = "FK_DeptType";
        /// <summary>
        ///  Department heads 
        /// </summary>
        public const string Leader = "Leader";
        /// <summary>
        ///  Phone 
        /// </summary>
        public const string Tel = "Tel";
        /// <summary>
        ///  Units full name 
        /// </summary>
        public const string NameOfPath = "NameOfPath";
    }
    /// <summary>
    ///  Department 
    /// </summary>
    public class Dept : EntityTree
    {
        #region  Property 
        /// <summary>
        ///  Full name 
        /// </summary>
        public  string NameOfPath
        {
            get
            {
                return this.GetValStrByKey(DeptAttr.NameOfPath);
            }
            set
            {
                this.SetValByKey(DeptAttr.NameOfPath, value);
            }
        }
        /// <summary>
        ///  Parent node ID
        /// </summary>
        public new string ParentNo
        {
            get
            {
                return this.GetValStrByKey(DeptAttr.ParentNo);
            }
            set
            {
                this.SetValByKey(DeptAttr.ParentNo, value);
            }
        }
        /// <summary>
        ///  Sector Type 
        /// </summary>
        public string FK_DeptType
        {
            get
            {
                return this.GetValStrByKey(DeptAttr.FK_DeptType);
            }
            set
            {
                this.SetValByKey(DeptAttr.FK_DeptType, value);
            }
        }
        /// <summary>
        ///  Sector Type Name 
        /// </summary>
        public string FK_DeptTypeText
        {
            get
            {
                return this.GetValRefTextByKey(DeptAttr.FK_DeptType);
            }
        }
        private Depts _HisSubDepts = null;
        /// <summary>
        ///  Its child nodes 
        /// </summary>
        public Depts HisSubDepts
        {
            get
            {
                if (_HisSubDepts == null)
                    _HisSubDepts = new Depts(this.No);
                return _HisSubDepts;
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Department 
        /// </summary>
        public Dept() { }
        /// <summary>
        ///  Department 
        /// </summary>
        /// <param name="no"> Serial number </param>
        public Dept(string no) : base(no) { }
        #endregion

        #region  Overriding methods 
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                uac.OpenForSysAdmin();
                return uac;
            }
        }
        /// <summary>
        /// Map
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map();
                map.EnDBUrl = new DBUrl(DBUrlType.AppCenterDSN); // The database connected to the . ( The default is : AppCenterDSN )
                map.PhysicsTable = "Port_Dept";
                map.EnType = EnType.Admin;

                map.EnDesc = " Department "; //   Description of the entity .
                map.DepositaryOfEntity = Depositary.Application; // Entity map Storage location .
                map.DepositaryOfMap = Depositary.Application;    // Map  Storage location .

                map.AddTBStringPK(DeptAttr.No, null, " Serial number ", true, true, 1, 50, 20);

                // Such as xx Branch of the Ministry of Finance 
                map.AddTBString(DeptAttr.Name, null, " Name ", true, false, 0, 100, 30);

                // Such as :\\ Gallop Group \\ South Branch \\ Finance Department 
                map.AddTBString(DeptAttr.NameOfPath, null, " Department path ", false, false, 0, 300, 30);

                map.AddTBString(DeptAttr.ParentNo, null, " Parent node number ", false, false, 0, 100, 30);
                map.AddTBString(DeptAttr.TreeNo, null, " Tree No. ", false, false, 0, 100, 30);
                map.AddTBString(DeptAttr.Leader, null, " Leadership ", false, false, 0, 100, 30);

                // Such as :  Finance Department , Production , Human Resources .
                map.AddTBString(DeptAttr.Tel, null, " Phone ", false, false, 0, 100, 30);

                map.AddTBInt(DeptAttr.Idx, 0, "Idx", false, false);
                map.AddTBInt(DeptAttr.IsDir, 0, " Whether it is a directory ", false, false);

                map.AddDDLEntities(DeptAttr.FK_DeptType, null, " Sector Type ", new DeptTypes(), true);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        /// <summary>
        ///  Generating department full name .
        /// </summary>
        public void GenerNameOfPath()
        {
            string name = this.Name;
            Dept dept = new Dept(this.ParentNo);
            while (true)
            {
                if (dept.IsRoot)
                    break;
                name = dept.Name + "\\\\" + name;
            }
            this.NameOfPath = name;
            this.DirectUpdate();
        }
    }
    /// <summary>
    /// Get set 
    /// </summary>
    public class Depts : EntitiesNoName
    {
        /// <summary>
        ///  Get a new entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new Dept();
            }
        }
        /// <summary>
        ///  Collection department 
        /// </summary>
        public Depts()
        {

        }
        /// <summary>
        ///  Collection department 
        /// </summary>
        /// <param name="parentNo"> Father department No</param>
        public Depts(string parentNo)
        {
            this.Retrieve(DeptAttr.ParentNo, parentNo);
        }
    }
}
