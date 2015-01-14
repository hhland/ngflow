using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.En;
using BP.Port;
using BP.Web;
using BP.Sys;

namespace BP.WF.Data
{
	/// <summary>
	///  Document properties 
	/// </summary>
    public class BillAttr
    {
        #region  Property 
        public const string MyPK = "MyPK";
        /// <summary>
        ///  The work ID
        /// </summary>
        public const string WorkID = "WorkID";
        /// <summary>
        ///  Node 
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        /// FK_Bill
        /// </summary>
        public const string FK_Bill = "FK_Bill";
        /// <summary>
        /// FK_Bill
        /// </summary>
        public const string Url = "Url";
        /// <summary>
        ///  No delivery 
        /// </summary>
        public const string FK_Emp = "FK_Emp";
        /// <summary>
        ///  Sponsor 
        /// </summary>
        public const string FK_Starter = "FK_Starter";
        /// <summary>
        ///  Symbol 
        /// </summary>
        public const string FilePrix = "FilePrix";
        /// <summary>
        /// FileName
        /// </summary>
        public const string FileName = "FileName";
        /// <summary>
        ///  Record Time го
        /// </summary>
        public const string RDT = "RDT";
        /// <summary>
        ///  Department 
        /// </summary>
        public const string FK_Dept = "FK_Dept";
        /// <summary>
        ///  Years 
        /// </summary>
        public const string FK_NY = "FK_NY";
        /// <summary>
        /// FID
        /// </summary>
        public const string FID = "FID";
        /// <summary>
        /// FK_Flow
        /// </summary>
        public const string FK_Flow = "FK_Flow";
        /// <summary>
        /// FK_BillType
        /// </summary>
        public const string FK_BillType = "FK_BillType";
        public const string Title = "Title";
        public const string StartDT = "StartDT";
        /// <summary>
        ///  Participants 
        /// </summary>
        public const string Emps = "Emps";
        /// <summary>
        ///  Full path 
        /// </summary>
        public const string FullPath = "FullPath";
        #endregion
    }
	/// <summary>
	///  Invoice 
	/// </summary> 
    public class Bill : EntityMyPK
    {
        #region  Basic properties 
        /// <summary>
        ///  Path 
        /// </summary>
        public string FullPath
        {
            get
            {
                return this.GetValStringByKey(BillAttr.FullPath);
            }
            set
            {
                this.SetValByKey(BillAttr.FullPath, value);
            }
        }
        /// <summary>
        ///  Participants 
        /// </summary>
        public string Emps
        {
            get
            {
                return this.GetValStringByKey(BillAttr.Emps);
            }
            set
            {
                this.SetValByKey(BillAttr.Emps, value);
            }
        }
        /// <summary>
        ///  Launch date 
        /// </summary>
        public string StartDT
        {
            get
            {
                return this.GetValStringByKey(BillAttr.StartDT);
            }
            set
            {
                this.SetValByKey(BillAttr.StartDT, value);
            }
        }
        /// <summary>
        ///  Document Type 
        /// </summary>
        public string FK_BillType
        {
            get
            {
                return this.GetValStringByKey(BillAttr.FK_BillType);
            }
            set
            {
                this.SetValByKey(BillAttr.FK_BillType, value);
            }
        }
        /// <summary>
        ///  Document Type Name 
        /// </summary>
        public string FK_BillTypeT
        {
            get
            {
                return this.GetValStrByKey(BillAttr.FK_BillType);
            }
        }
        /// <summary>
        ///  Process title 
        /// </summary>
        public string Title
        {
            get
            {
                return this.GetValStringByKey(BillAttr.Title);
            }
            set
            {
                this.SetValByKey(BillAttr.Title, value);
            }
        }
        /// <summary>
        ///  Process ID 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.GetValStringByKey(BillAttr.FK_Flow);
            }
            set
            {
                this.SetValByKey(BillAttr.FK_Flow, value);
            }
        }
        /// <summary>
        ///  Process Name 
        /// </summary>
        public string FK_FlowT
        {
            get
            {
                return this.GetValRefTextByKey(BillAttr.FK_Flow);
            }
        }
        /// <summary>
        ///  Sponsor name 
        /// </summary>
        public string FK_StarterT
        {
            get
            {
                return this.GetValRefTextByKey(BillAttr.FK_Starter);
            }
        }
        /// <summary>
        ///  Sponsor 
        /// </summary>
        public string FK_Starter
        {
            get
            {
                return this.GetValStringByKey(BillAttr.FK_Starter);
            }
            set
            {
                this.SetValByKey(BillAttr.FK_Starter, value);
            }
        }
        /// <summary>
        ///  The operator 
        /// </summary>
        public string FK_Emp
        {
            get
            {
                return this.GetValStringByKey(BillAttr.FK_Emp);
            }
            set
            {
                this.SetValByKey(BillAttr.FK_Emp, value);
            }
        }
        /// <summary>
        ///  The operator name 
        /// </summary>
        public string FK_EmpT
        {
            get
            {
                return this.GetValRefTextByKey(BillAttr.FK_Emp);
            }
        }
        /// <summary>
        ///  Documents Name 
        /// </summary>
        public string FK_BillText
        {
            get
            {
                return this.GetValRefTextByKey(BillAttr.FK_Bill);
            }
        }
        /// <summary>
        ///  Document Number 
        /// </summary>
        public string FK_Bill
        {
            get
            {
                return this.GetValStrByKey(BillAttr.FK_Bill);
            }
            set
            {
                this.SetValByKey(BillAttr.FK_Bill, value);
            }
        }
        /// <summary>
        ///  Years 
        /// </summary>
        public string FK_NY
        {
            get
            {
                return this.GetValStrByKey(BillAttr.FK_NY);
            }
            set
            {
                this.SetValByKey(BillAttr.FK_NY, value);
            }
        }
        /// <summary>
        ///  The work ID
        /// </summary>
        public Int64 WorkID
        {
            get
            {
                return this.GetValInt64ByKey(BillAttr.WorkID);
            }
            set
            {
                this.SetValByKey(BillAttr.WorkID, value);
            }
        }
        /// <summary>
        ///  Process ID
        /// </summary>
        public Int64 FID
        {
            get
            {
                return this.GetValInt64ByKey(BillAttr.FID);
            }
            set
            {
                this.SetValByKey(BillAttr.FID, value);
            }
        }
        /// <summary>
        ///  Node ID
        /// </summary>
        public int FK_Node
        {
            get
            {
                return this.GetValIntByKey(BillAttr.FK_Node);
            }
            set
            {
                this.SetValByKey(BillAttr.FK_Node, value);
            }
        }
        /// <summary>
        ///  Node Name 
        /// </summary>
        public string FK_NodeT
        {
            get
            {
                Node nd = new Node(this.FK_Node);
                return nd.Name;
                //return this.GetValRefTextByKey(BillAttr.FK_Node);
            }
        }
        /// <summary>
        ///  Document Printing time 
        /// </summary>
        public string RDT
        {
            get
            {
                return this.GetValStringByKey(BillAttr.RDT);
            }
            set
            {
                this.SetValByKey(BillAttr.RDT, value);
            }
        }
        /// <summary>
        ///  Department 
        /// </summary>
        public string FK_Dept
        {
            get
            {
                return this.GetValStringByKey(BillAttr.FK_Dept);
            }
            set
            {
                this.SetValByKey(BillAttr.FK_Dept, value);
            }
        }
        /// <summary>
        ///  Department name 
        /// </summary>
        public string FK_DeptT
        {
            get
            {
                return this.GetValRefTextByKey(BillAttr.FK_Dept);
            }
        }
        /// <summary>
        ///  Hyperlinks 
        /// </summary>
        public string Url
        {
            get
            {
                return this.GetValStringByKey(BillAttr.Url);
            }
            set
            {
                this.SetValByKey(BillAttr.Url, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        /// UI Access control interface 
        /// </summary>
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                uac.IsDelete = false;
                uac.IsInsert = false;
                uac.IsUpdate = false;
                uac.IsView = true;
                return uac;
            }
        }
        /// <summary>
        ///  Invoice 
        /// </summary>
        public Bill() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pk"></param>
        public Bill(string pk)
            : base(pk)
        {
        }
        #endregion

        #region Map
        /// <summary>
        /// EnMap
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;
                Map map = new Map("WF_Bill");
                map.DepositaryOfMap = Depositary.None;
                map.EnDesc = " Invoice ";

                map.AddMyPKNoVisable();

                map.AddTBInt(BillAttr.WorkID, 0, " The work ID", false, true);
                map.AddTBInt(BillAttr.FID, 0, "FID", false, true);
                map.AddTBString(BillAttr.FK_Flow, null, " Process ", false, false, 0, 4, 5);
                map.AddTBString(BillAttr.FK_BillType, null, " Document Type ", false, false, 0, 300, 5);
                map.AddTBString(BillAttr.Title, null, " Title ", false, false, 0, 900, 5);
                map.AddTBString(BillAttr.FK_Starter, null, " Sponsor ", true, true, 0, 200, 5);
                map.AddTBDateTime(BillAttr.StartDT, " Start Time ", true, true);

                map.AddTBString(BillAttr.Url, null, "Url", false, false, 0, 2000, 5);
                map.AddTBString(BillAttr.FullPath, null, "FullPath", false, false, 0, 2000, 5);

                map.AddDDLEntities(BillAttr.FK_Emp, null, " Print People ", new Emps(), false);
                map.AddTBDateTime(BillAttr.RDT, " Print Time ", true, true);

                map.AddDDLEntities(BillAttr.FK_Dept, null, " Membership department ", new BP.Port.Depts(), false);
                
                map.AddDDLEntities(BillAttr.FK_NY, null, " Years of membership ", new BP.Pub.NYs(), false);

                map.AddTBString(BillAttr.Emps, null, "Emps", false, false, 0, 60, 5);

                map.AddTBString(BillAttr.FK_Node, null, " Node ", false, false, 0, 30, 5);
                map.AddTBString(BillAttr.FK_Bill, null, "FK_Bill", false, false, 0, 500, 5);
                map.AddTBIntMyNum();

                map.AddSearchAttr(BillAttr.FK_Dept);
                map.AddSearchAttr(BillAttr.FK_NY);

                map.AddSearchAttr(BillAttr.FK_Emp);

                RefMethod rm = new RefMethod();
                rm.Title = " Turn on ";
                rm.ClassMethodName = this.ToString() + ".DoOpen";
                rm.Icon = "/WF/Img/FileType/doc.gif";
                map.AddRefMethod(rm);

                rm = new RefMethod();
                rm.Title = " Turn on ";
                rm.ClassMethodName = this.ToString() + ".DoOpenPDF";
                rm.Icon = "/WF/Img/FileType/pdf.gif";
                map.AddRefMethod(rm);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        /// <summary>
        ///  Turn on 
        /// </summary>
        /// <returns></returns>
        public string DoOpen()
        {
            string path = BP.Sys.Glo.Request.MapPath(this.Url);
            PubClass.OpenWordDocV2(path, this.FK_EmpT + " Print " + this.FK_BillTypeT + ".doc");
            return null;
        }
        /// <summary>
        ///  Turn on pdf
        /// </summary>
        /// <returns></returns>
        public string DoOpenPDF()
        {
            string path = BP.Sys.Glo.Request.MapPath(this.Url);
            PubClass.OpenWordDocV2(path, this.FK_EmpT + " Print " + this.FK_BillTypeT + ".pdf");
            return null;
        }
    }
	/// <summary>
	///  Invoice s
	/// </summary>
	public class Bills :Entities
	{
		#region  Constructor property 
		/// <summary>
        ///  Invoice s
		/// </summary>
		public Bills(){}
		#endregion 

		#region  Property 
		/// <summary>
        ///  Invoice 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new Bill();
			}
		}
		#endregion
	}
}
