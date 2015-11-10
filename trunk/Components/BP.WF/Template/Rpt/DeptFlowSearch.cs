using System;
using System.Data;
using BP.DA;
using BP.En;

namespace BP.WF.Port
{
    /// <summary>
    ///  Process data query permissions department 
    /// </summary>
    public class DeptFlowSearchAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  Staff ID
        /// </summary>
        public const string FK_Emp = "FK_Emp";
        /// <summary>
        ///  Department 
        /// </summary>
        public const string FK_Dept = "FK_Dept";
        /// <summary>
        ///  Process ID 
        /// </summary>
        public const string FK_Flow = "FK_Flow";
        #endregion
    }
    /// <summary>
    ///  Process data query permissions department   The summary .
    /// </summary>
    public class DeptFlowSearch : EntityMyPK
    {
        /// <summary>
        /// UI Access control interface 
        /// </summary>
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                if (BP.Web.WebUser.No == "admin")
                {
                    uac.IsView = true;
                    uac.IsDelete = true;
                    uac.IsInsert = true;
                    uac.IsUpdate = true;
                    uac.IsAdjunct = true;
                }
                return uac;
            }
        }

        #region  Basic properties 
        /// <summary>
        ///  Staff ID
        /// </summary>
        public string FK_Emp
        {
            get
            {
                return this.GetValStringByKey(DeptFlowSearchAttr.FK_Emp);
            }
            set
            {
                SetValByKey(DeptFlowSearchAttr.FK_Emp, value);
            }
        }
        /// <summary>
        /// Department 
        /// </summary>
        public string FK_Dept
        {
            get
            {
                return this.GetValStringByKey(DeptFlowSearchAttr.FK_Dept);
            }
            set
            {
                SetValByKey(DeptFlowSearchAttr.FK_Dept, value);
            }
        }
        /// <summary>
        ///  Process ID 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.GetValStringByKey(DeptFlowSearchAttr.FK_Flow);
            }
            set
            {
                this.SetValByKey(DeptFlowSearchAttr.FK_Flow, value);
            }
        }
        #endregion
      

        #region  Constructor 
        /// <summary>
        ///  Process data query permissions department 
        /// </summary> 
        public DeptFlowSearch() { }
        /// <summary>
        ///  Override the base class methods 
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("WF_DeptFlowSearch");
                map.EnDesc = " Process data query permissions department ";
                map.AddMyPK();
                map.AddTBString(DeptFlowSearchAttr.FK_Emp, null, " The operator ", true, true, 1, 50, 11);
                map.AddTBString(DeptFlowSearchAttr.FK_Flow, null, " Process ID ", true, true, 1, 50, 11);
                map.AddTBString(DeptFlowSearchAttr.FK_Dept, null, " Department number ", true, true, 1, 100, 11);
                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
        
    }
    /// <summary>
    ///  Process data query permissions department  
    /// </summary>
    public class DeptFlowSearchs : Entities
    {
        #region  Structure 
        /// <summary>
        ///  Process data query permissions department 
        /// </summary>
        public DeptFlowSearchs() { }
        /// <summary>
        ///  Process data query permissions department 
        /// </summary>
        /// <param name="FK_Emp">FK_Emp</param>
        public DeptFlowSearchs(string FK_Emp)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(DeptFlowSearchAttr.FK_Emp, FK_Emp);
            qo.DoQuery();
        }
        #endregion

        #region  Method 
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new DeptFlowSearch();
            }
        }
        #endregion

        #region  Query methods 
        #endregion
    }
}
