using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using System.Text;

namespace CCPortal
{
    /// <summary>
    /// 菜单类型
    /// </summary>
    public enum MenuType
    {
        /// <summary>
        /// 目录
        /// </summary>
        Dir,
        /// <summary>
        /// 菜单
        /// </summary>
        Menu,
        /// <summary>
        /// 功能控制点
        /// </summary>
        Function
    }
    /// <summary>
    /// 应用系统信息
    /// </summary>
    public class Menu
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
        /// 连接
        /// </summary>
        public string Url
        {
            get
            {
                return this.Row["Url"].ToString();
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
        /// <summary>
        /// 菜单类型
        /// </summary>
        public MenuType MenuType
        {
            get
            {
                return (MenuType) int.Parse(this.Row["MenuType"].ToString());
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
    /// 应用系统信息
    /// </summary>
    public class Menus:System.Collections.CollectionBase
    {
        #region 构造.
        /// <summary>
        /// 菜单集合
        /// </summary>
        public Menus()
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
                Menu en = new Menu();
                en.Init(dt, dr);
            }
            return dt.Rows.Count;
        }
        #endregion 方法.
    }
}
