using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using System.Text;

namespace CCPortal
{
    /// <summary>
    /// 部门
    /// </summary>
    public class Dept
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
        /// <summary>
        /// 父节点编号
        /// </summary>
        public string ParentNo
        {
            get
            {
                return this.Row["ParentNo"].ToString();
            }
        }
        #endregion 属性.

        #region 方法.
        /// <summary>
        /// 查询该系统
        /// </summary>
        /// <returns></returns>
        public int Init(DataTable dt,DataRow dr)
        {
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
    /// <summary>
    /// 部门
    /// </summary>
    public class Depts:System.Collections.CollectionBase
    {
        #region 构造.
        /// <summary>
        /// 菜单集合
        /// </summary>
        public Depts()
        {
        }
        #endregion 构造.

        #region 方法.
        /// <summary>
        /// 按照用户编号查询菜单
        /// </summary>
        /// <returns>返回查询的个数</returns>
        public int Init(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                Dept en = new Dept();
                en.Init(dt, dr);
            }
            return dt.Rows.Count;
        }
        #endregion 方法.
    }
}
