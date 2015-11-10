using System;
using System.Data;
using BP.DA;
using BP.Port; 
using BP.En;

namespace BP.PRJ
{
    /// <summary>
    ///  Project Status 
    /// </summary>
    public enum PrjState
    {
        /// <summary>
        ///  New 
        /// </summary>
        Init,
        /// <summary>
        ///  Run 
        /// </summary>
        Runing,
        /// <summary>
        ///  Delete 
        /// </summary>
        Delete
    }
	/// <summary>
	///  List Item Properties 
	/// </summary>
    public class PrjAttr : EntityNoNameAttr
    {
        /// <summary>
        ///  Unit 
        /// </summary>
        public const string DW = "DW";
        /// <summary>
        ///  Address 
        /// </summary>
        public const string Addr = "Addr";
        /// <summary>
        ///  Cost 
        /// </summary>
        public const string ZJ = "ZJ";
        /// <summary>
        ///  Development costs 
        /// </summary>
        public const string KFFY = "KFFY";
        /// <summary>
        ///  Reporting date 
        /// </summary>
        public const string SBRQ = "SBRQ";
        /// <summary>
        ///  Are special office 
        /// </summary>
        public const string IsTB = "IsTB";
        /// <summary>
        ///  Project Status 
        /// </summary>
        public const string PrjState = "PrjState";
        /// <summary>
        ///  Department 
        /// </summary>
        public const string FK_Dept = "FK_Dept";
        /// <summary>
        ///  File 
        /// </summary>
        public const string Files = "Files";
    }
	/// <summary>
	///  Project 
	/// </summary>
    public class Prj : EntityNoName
    {
        #region  Basic properties 
        /// <summary>
        ///  Project Status 
        /// </summary>
        public PrjState HisPrjState
        {
            get
            {
                return (PrjState)this.GetValIntByKey(PrjAttr.PrjState);
            }
            set
            {
                this.SetValByKey(PrjAttr.PrjState, (int)value);
            }
        }
        public string Files
        {
            get
            {
                return this.GetValStrByKey(PrjAttr.Files);
            }
            set
            {
                this.SetValByKey(PrjAttr.Files, value);
            }
        }
        /// <summary>
        ///  Address 
        /// </summary>
        public string Addr
        {
            get
            {
                return this.GetValStrByKey(PrjAttr.Addr);
            }
            set
            {
                this.SetValByKey(PrjAttr.Addr, value);
            }
        }
        /// <summary>
        ///  Unit 
        /// </summary>
        public string DW
        {
            get
            {
                return this.GetValStrByKey(PrjAttr.DW);
            }
            set
            {
                this.SetValByKey(PrjAttr.DW, value);
            }
        }
        /// <summary>
        ///  Cost 
        /// </summary>
        public float ZJ
        {
            get
            {
                return this.GetValFloatByKey(PrjAttr.ZJ);
            }
            set
            {
                this.SetValByKey(PrjAttr.ZJ, value);
            }
        }
        /// <summary>
        ///  Development costs 
        /// </summary>
        public float KFFY
        {
            get
            {
                return this.GetValFloatByKey(PrjAttr.KFFY);
            }
            set
            {
                this.SetValByKey(PrjAttr.KFFY, value);
            }
        }
        /// <summary>
        ///  Reporting date 
        /// </summary>
        public string SBRQ
        {
            get
            {
                return this.GetValStrByKey(PrjAttr.SBRQ);
            }
            set
            {
                this.SetValByKey(PrjAttr.SBRQ, value);
            }
        }
        /// <summary>
        ///  Are special office 
        /// </summary>
        public bool IsTB
        {
            get
            {
                return this.GetValBooleanByKey(PrjAttr.IsTB);
            }
            set
            {
                this.SetValByKey(PrjAttr.IsTB, value);
            }
        }
        /// <summary>
        ///  Project Status 
        /// </summary>
        public int PrjState
        {
            get
            {
                return this.GetValIntByKey(PrjAttr.PrjState);
            }
            set
            {
                this.SetValByKey(PrjAttr.PrjState, value);
            }
        }
        /// <summary>
        ///  Are special labels do 
        /// </summary>
        public string PrjStateText
        {
            get
            {
                return this.GetValRefTextByKey(PrjAttr.PrjState);
            }
        }
        #endregion

        #region  Constructor 
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                uac.IsDelete = true;
                uac.IsInsert = true;
                uac.IsUpdate = true;
                uac.IsView = true;
                return uac;
            }
        }
        /// <summary>
        ///  Project 
        /// </summary>
        public Prj() { }
        /// <summary>
        /// strubg
        /// </summary>
        public Prj(string no)
        {
            this.No = no;
            this.Retrieve();
        }
        public Prj(int no)
        {
            this.No = no.ToString();
            this.Retrieve();
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


                Map map = new Map("Prj_Prj");
                map.EnDesc = " Project ";

                map.DepositaryOfMap = Depositary.Application;
                map.CodeStruct = "4";
                map.IsAutoGenerNo = true;

                map.AddTBStringPK(PrjAttr.No, null, " Serial number ", true, true, 4, 4, 4);
                map.AddTBString(PrjAttr.Name, null, " Name ", true, false, 0, 60, 500);
                map.AddDDLEntities(PrjAttr.FK_Dept, null, this.ToE("Dept", " Department "), new Port.Depts(), true);

                map.AddDDLSysEnum(PrjAttr.PrjState, 0, " Project Status ", true, true, PrjAttr.PrjState,
                    "@0= New @1= Run @2= Put on record ");

                map.AddTBString(PrjAttr.DW, null, " Unit ", true, false, 0, 60, 500, true);
                map.AddTBString(PrjAttr.Addr, null, " Address ", true, false, 0, 60, 500, true);

                map.AddTBString(PrjAttr.Files, null, " File s", false, false, 0, 3000, 500, true);

                map.AttrsOfOneVSM.Add(new EmpPrjs(), new Emps(), EmpPrjAttr.FK_Prj, EmpPrjAttr.FK_Emp,
                    DeptAttr.Name, DeptAttr.No, " Member ");

                map.AddSearchAttr(PrjAttr.FK_Dept);
                map.AddSearchAttr(PrjAttr.PrjState);

                RefMethod rm = new RefMethod();
                rm.Title = " Members post ";
                rm.ClassMethodName = this.ToString() + ".DoEmpPrjStations";
                rm.IsCanBatch = true;
                map.AddRefMethod(rm);

                rm = new RefMethod();
                rm.Title = " Data Tree ";
                rm.ClassMethodName = this.ToString() + ".DoDocTree";
                rm.IsCanBatch = true;
                map.AddRefMethod(rm);

                rm = new RefMethod();
                rm.Title = " Upload Rules node ";
                rm.ClassMethodName = this.ToString() + ".DoNodeAccess";
                rm.IsCanBatch = true;
                map.AddRefMethod(rm);

                //rm = new RefMethod();
                //rm.Title = " Perform fine ";
                //rm.ClassMethodName = this.ToString() + ".DoFK";
                //rm.HisAttrs.AddTBDecimal("JE", 100, " Enter the amount ", true, false);
                //map.AddRefMethod(rm);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        public string DoNodeAccess()
        {
            PubClass.WinOpen("../PRJ/NodeAccess.aspx?FK_Prj=" + this.No, 500, 500);
            return null;
        }


        public string DoEmpPrjStations()
        {
            PubClass.WinOpen("../Comm/PanelEns.aspx?EnsName=BP.PRJ.EmpPrjExts&FK_Prj=" + this.No, 800, 500);
            return null;
        }

        public string DoDocTree()
        {
            PubClass.WinOpen("../PRJ/DocTree.aspx?No=" + this.No, 500, 500);
            return null;
        }
        protected override bool beforeInsert()
        {
            this.No = this.GenerNewNo;
            this.SBRQ = DataType.CurrentData;

            string root = BP.SystemConfig.PathOfDataUser + "\\PrjData\\Templete";
            if (System.IO.Directory.Exists(root) == false)
                System.IO.Directory.CreateDirectory(root);

            root += "\\" + this.No;
            if (System.IO.Directory.Exists(root) == false)
                System.IO.Directory.CreateDirectory(root);


            if (System.IO.Directory.Exists(root + "\\01. Bibliography 1") == false)
                System.IO.Directory.CreateDirectory(root + "\\01. Bibliography 1");

            if (System.IO.Directory.Exists(root + "\\02. Bibliography 2") == false)
                System.IO.Directory.CreateDirectory(root + "\\02. Bibliography 2");

            if (System.IO.Directory.Exists(root + "\\03. Bibliography 3") == false)
                System.IO.Directory.CreateDirectory(root + "\\03. Bibliography 3");

            return base.beforeInsert();
        }
    }
	/// <summary>
	///  Project s
	/// </summary>
	public class Prjs : EntitiesNoName
	{	
		#region  Constructor 
		/// <summary>
		///  Get it  Entity
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{			 
				return new Prj();
			}
		}
		/// <summary>
		///  Project s 
		/// </summary>
		public Prjs(){}
		#endregion
	}
	
}
