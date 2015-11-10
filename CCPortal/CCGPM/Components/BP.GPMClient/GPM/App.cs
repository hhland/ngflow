using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using System.Text;

namespace CCPortal
{
    /// <summary>
    /// 应用系统信息
    /// </summary>
    public class App
    {
        #region 属性.
        /// <summary>
        /// 行
        /// </summary>
        public Hashtable Row = null;
        /// <summary>
        /// 系统编号
        /// </summary>
        public string No
        {
            get
            {
                return this.Row["No"].ToString();
            }
        }
        /// <summary>
        /// 系统名称
        /// </summary>
        public string Name
        {
            get
            {
                return this.Row["Name"].ToString();
            }
        }
        #endregion 属性.

        #region 方法.
        /// <summary>
        /// 查询该系统
        /// </summary>
        /// <returns></returns>
        public int QueryIt()
        {
            string appNo = CCPortal.SystemConfig.AppSettings["CCPortal.AppNo"];

            CCPortal.DA.Paras ps = new DA.Paras();
            ps.SQL = "SELECT * FROM GPM_App WHERE No=" + SystemConfig.AppCenterDBVarStr + "No";
            ps.Add("No", appNo);
            DataTable dt = CCPortal.DA.DBAccess.RunSQLReturnTable(ps);
            if (dt.Rows.Count == 0)
                throw new Exception("@配置的系统编号(" + appNo + ")在GPM中不存在.");
            
            //把数据赋值.
            this.Row = new Hashtable();
            foreach (DataColumn dc in dt.Columns)
            {
                this.Row.Add(dc.ColumnName, dt.Rows[0][dc.ColumnName]);
            }
            return 1;
        }
        #endregion 方法.

    }
}
