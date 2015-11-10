using System;
using System.Collections;
using System.Data;
using BP.DA;
using BP.Web;
using BP.En;

namespace BP.GPM
{
    /// <summary>
    /// 用户菜单
    /// </summary>
    public class UserMenuAttr
    {
        /// <summary>
        /// 菜单
        /// </summary>
        public const string FK_Menu = "FK_Menu";
        /// <summary>
        /// 用户
        /// </summary>
        public const string FK_Emp = "FK_Emp";
        /// <summary>
        /// 是否选中.
        /// </summary>
        public const string IsChecked = "IsChecked";
    }
    /// <summary>
    /// 用户菜单
    /// </summary>
    public class UserMenu : EntityMM
    {
        #region 属性
        public string FK_Menu
        {
            get
            {
                return this.GetValStringByKey(UserMenuAttr.FK_Menu);
            }
            set
            {
                this.SetValByKey(UserMenuAttr.FK_Menu, value);
            }
        }
        public string FK_Emp
        {
            get
            {
                return this.GetValStringByKey(UserMenuAttr.FK_Emp);
            }
            set
            {
                this.SetValByKey(UserMenuAttr.FK_Emp, value);
            }
        }
        public string IsChecked
        {
            get
            {
                return this.GetValStringByKey(UserMenuAttr.IsChecked);
            }
            set
            {
                this.SetValByKey(UserMenuAttr.IsChecked, value);
            }
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// 用户菜单
        /// </summary>
        public UserMenu()
        {
        }
        /// <summary>
        /// 用户菜单
        /// </summary>
        /// <param name="mypk"></param>
        public UserMenu(string no)
        {
            this.Retrieve();
        }
        /// <summary>
        /// 用户菜单
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;
                Map map = new Map("GPM_UserMenu");
                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.EnDesc = "用户菜单";
                map.EnType = EnType.Sys;

                map.AddTBStringPK(UserMenuAttr.FK_Emp, null, "用户", false, false, 0, 50, 20);
                map.AddTBStringPK(UserMenuAttr.FK_Menu, null, "菜单", false, false, 0, 50, 20);
                map.AddBoolean(UserMenuAttr.IsChecked, true, "是否选中", true, true);
                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    /// 用户菜单s
    /// </summary>
    public class UserMenus : EntitiesMM
    {
        #region 构造
        /// <summary>
        /// 用户s
        /// </summary>
        public UserMenus()
        {
        }
        /// <summary>
        /// 得到它的 Entity
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new UserMenu();
            }
        }
        #endregion
    }
}
