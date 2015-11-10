using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.WF;
using BP.WF.Template;
using BP.Port;

namespace BP.WF.Template
{
    /// <summary>
    ///  CC control 
    /// </summary>
    public enum CtrlWay
    {
        /// <summary>
        ///  According to the post 
        /// </summary>
        ByStation,
        /// <summary>
        ///  By sector 
        /// </summary>
        ByDept,
        /// <summary>
        ///  By staff 
        /// </summary>
        ByEmp,
        /// <summary>
        /// °´SQL
        /// </summary>
        BySQL
    }
	/// <summary>
	///  Cc property 
	/// </summary>
    public class CCAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  Title 
        /// </summary>
        public const string CCTitle = "CCTitle";
        /// <summary>
        ///  Cc content 
        /// </summary>
        public const string CCDoc = "CCDoc";
        /// <summary>
        ///  CC control 
        /// </summary>
        public const string CCCtrlWay = "CCCtrlWay";
        /// <summary>
        ///  CC object 
        /// </summary>
        public const string CCSQL = "CCSQL";
        #endregion
    }
	/// <summary>
	///  Cc 
	/// </summary>
    public class CC : Entity
    {
        #region  Property 
        /// <summary>
        ///  Cc 
        /// </summary>
        /// <param name="rpt"></param>
        /// <returns></returns>
        public DataTable GenerCCers(Entity rpt)
        {
            string sql = "";
            switch (this.HisCtrlWay)
            {
                case CtrlWay.BySQL:
                    sql = this.CCSQL.Clone() as string;
                    sql = sql.Replace("@WebUser.No", BP.Web.WebUser.No);
                    sql = sql.Replace("@WebUser.Name", BP.Web.WebUser.Name);
                    sql = sql.Replace("@WebUser.FK_Dept", BP.Web.WebUser.FK_Dept);
                    if (sql.Contains("@"))
                    {
                        foreach (Attr attr in rpt.EnMap.Attrs)
                        {
                            if (sql.Contains("@") == false)
                                break;
                            sql = sql.Replace("@" + attr.Key, rpt.GetValStrByKey(attr.Key));
                        }
                    }
                    break;
                case CtrlWay.ByEmp:
                    sql = "SELECT No,Name FROM Port_Emp WHERE No IN (SELECT FK_Emp FROM WF_CCEmp WHERE FK_Node=" + this.NodeID + ")";
                    break;
                case CtrlWay.ByDept:
                    sql = "SELECT No,Name FROM Port_Emp WHERE No IN (SELECT FK_Emp FROM Port_EmpDept WHERE FK_Dept IN ( SELECT FK_Dept FROM WF_CCDept WHERE FK_Node=" + this.NodeID + "))";
                    break;
                case CtrlWay.ByStation:
                    sql = "SELECT No,Name FROM Port_Emp WHERE No IN (SELECT FK_Emp FROM Port_EmpStation WHERE FK_Station IN ( SELECT FK_Station FROM WF_CCStation WHERE FK_Node=" + this.NodeID + "))";
                    break;
                default:
                    throw new Exception(" Unhandled exception ");
            }
            DataTable dt= DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
                throw new Exception("@ Process node design errors , CC staff found , Cc way :"+this.HisCtrlWay+" SQL:"+sql);

            return dt;
        }
        /// <summary>
        /// Node ID
        /// </summary>
        public int NodeID
        {
            get
            {
                return this.GetValIntByKey(NodeAttr.NodeID);
            }
            set
            {
                this.SetValByKey(NodeAttr.NodeID, value);
            }
        }
        /// <summary>
        /// UI Access control interface 
        /// </summary>
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                if (BP.Web.WebUser.No != "admin")
                {
                    uac.IsView = false;
                    return uac;
                }
                uac.IsDelete = false;
                uac.IsInsert = false;
                uac.IsUpdate = true;
                return uac;
            }
        }
        /// <summary>
        ///  CC title 
        /// </summary>
        public string CCTitle
        {
            get
            {
                string s= this.GetValStringByKey(CCAttr.CCTitle);
                if (string.IsNullOrEmpty(s))
                    s = " Come from @Rec The CC information .";
                return s;
            }
            set
            {
                this.SetValByKey(CCAttr.CCTitle, value);
            }
        }
        /// <summary>
        ///  Cc content 
        /// </summary>
        public string CCDoc
        {
            get
            {
                string s = this.GetValStringByKey(CCAttr.CCDoc);
                if (string.IsNullOrEmpty(s))
                    s = " Come from @Rec The CC information .";
                return s;
            }
            set
            {
                this.SetValByKey(CCAttr.CCDoc, value);
            }
        }
        /// <summary>
        ///  CC object 
        /// </summary>
        public string CCSQL
        {
            get
            {
                string sql= this.GetValStringByKey(CCAttr.CCSQL);
                sql = sql.Replace("~", "'");
                sql = sql.Replace("[", "'");
                sql = sql.Replace("]", "'");
                sql = sql.Replace("''", "'");
                return sql;
            }
            set
            {
                this.SetValByKey(CCAttr.CCSQL, value);
            }
        }
        /// <summary>
        ///  Control mode 
        /// </summary>
        public CtrlWay HisCtrlWay
        {
            get
            {
                return (CtrlWay)this.GetValIntByKey(CCAttr.CCCtrlWay);
            }
            set
            {
                this.SetValByKey(CCAttr.CCCtrlWay, (int)value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        /// CC
        /// </summary>
        public CC()
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
                Map map = new Map("WF_Node");
                map.EnDesc = " CC rules ";
                map.EnType = EnType.Admin;
                map.AddTBString(NodeAttr.Name, null, " Node Name ", true, true, 0, 100, 10, true);
                map.AddTBIntPK(NodeAttr.NodeID, 0," Node ID", true, true);

                map.AddDDLSysEnum(CCAttr.CCCtrlWay, 0, " Control mode ",true, true,"CtrlWay");
                map.AddTBString(CCAttr.CCSQL, null, "SQL Expression ", true, false, 0, 500, 10, true);
                map.AddTBString(CCAttr.CCTitle, null, " CC title ", true, false, 0, 500, 10,true);
                map.AddTBStringDoc(CCAttr.CCDoc, null, " Cc content ( Title and content support variables )", true, false,true);

                map.AddSearchAttr(CCAttr.CCCtrlWay);

                //  Related functions .
                map.AttrsOfOneVSM.Add(new BP.WF.Template.CCStations(), new BP.WF.Port.Stations(),
                    NodeStationAttr.FK_Node, NodeStationAttr.FK_Station,
                    DeptAttr.Name, DeptAttr.No, " Cc jobs ");

                map.AttrsOfOneVSM.Add(new BP.WF.Template.CCDepts(), new BP.WF.Port.Depts(), NodeDeptAttr.FK_Node, NodeDeptAttr.FK_Dept, DeptAttr.Name,
                DeptAttr.No,  " Cc department " );

                map.AttrsOfOneVSM.Add(new BP.WF.Template.CCEmps(), new BP.WF.Port.Emps(), NodeEmpAttr.FK_Node, EmpDeptAttr.FK_Emp, DeptAttr.Name,
                    DeptAttr.No, " CC staff ");

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
	/// <summary>
	///  Cc s
	/// </summary>
	public class CCs: Entities
	{
		#region  Method 
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new CC();
			}
		}
		/// <summary>
        ///  Cc 
		/// </summary>
		public CCs(){} 		 
		#endregion
	}
}
