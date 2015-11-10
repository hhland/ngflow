using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace CCPortal
{
    /// <summary>
    /// 访问权限管理系统的接口
    /// 1, 需要在web.config 做 CCPortal.DSN,CCPortal.DBType,CCPortal.AppNo三个节点数据配置.
    /// 2, 如何配置以上三个节点，请参考操作手手册.
    /// </summary>
    public class API
    {
        /// <summary>
        /// 检查是否连接到GPM.
        /// </summary>
        /// <returns></returns>
        public static bool CheckIsConn()
        {
            string sql = "SELECT * FROM Port_Emp WHERE 1=2";
            DataTable dt = CCPortal.DA.DBAccess.RunSQLReturnTable(sql);
            return true;
        }

        #region 全局信息.
        /// <summary>
        /// 当前的应用系统
        /// </summary>
        private static App _CurrApp = null;
        /// <summary>
        /// 当前系统信息
        /// </summary>
        public static App CurrApp
        {
            get
            {
                if (_CurrApp == null)
                {
                    _CurrApp = new App();
                    _CurrApp.QueryIt();
                }
                return _CurrApp;
            }
        }
        #endregion 全局信息.

        #region 菜单的API.
        /// <summary>
        /// 按datatable的方式,返回用户菜单
        /// 根据这个菜单结构，形成自己的菜单树。
        /// </summary>
        /// <param name="userNo">用户编号</param>
        /// <returns>菜单:它的列与GPM数据库中的表GPM_EmpMenu一致.</returns>
        public static DataTable GetUserMenuOfDatatable(string userNo)
        {
            CCPortal.DA.Paras ps = new DA.Paras();
            ps.SQL = "SELECT * FROM V_GPM_EmpMenu WHERE FK_Emp=" + SystemConfig.AppCenterDBVarStr
                + "FK_Emp AND FK_App=" + SystemConfig.AppCenterDBVarStr + "FK_App ORDER BY Idx";
            ps.Add("FK_Emp", userNo);
            ps.Add("FK_App", API.CurrApp.No);
            return CCPortal.DA.DBAccess.RunSQLReturnTable(ps);
        }
        /// <summary>
        /// 按datatable的方式,返回用户菜单
        /// 根据这个菜单结构，形成自己的菜单树。
        /// </summary>
        /// <param name="userNo">用户编号</param>
        /// <returns>菜单:它的列与GPM数据库中的表GPM_EmpMenu一致.</returns>
        public static DataTable GetUserMenuOfDatatableByPNo(string userNo, string parentMenuNO)
        {
            CCPortal.DA.Paras ps = new DA.Paras();
            ps.SQL = "SELECT * FROM V_GPM_EmpMenu WHERE FK_Emp=" + SystemConfig.AppCenterDBVarStr + "FK_Emp AND ParentNo=" + SystemConfig.AppCenterDBVarStr
                + "ParentNo AND FK_App=" + SystemConfig.AppCenterDBVarStr + "FK_App ORDER BY Idx";
            ps.Add("FK_Emp", userNo);
            ps.Add("FK_App", API.CurrApp.No);
            ps.Add("ParentNo", parentMenuNO);
            return CCPortal.DA.DBAccess.RunSQLReturnTable(ps);
        }
        /// <summary>
        /// 根据用户编号、菜单父编号和是否显示功能点获取菜单项
        /// </summary>
        /// <param name="userNo">用户账号</param>
        /// <param name="menuNO">菜单编号</param>
        /// <param name="isVisibleFunPoint">是否显示功能点；true 显示，false 不显示</param>
        /// <returns></returns>
        public static DataTable GetMenu_ChildNode_Datatable_ByMenuNo(string userNo, string menuNO, bool isVisibleFunPoint)
        {
            CCPortal.DA.Paras ps = new DA.Paras();
            ps.SQL = "SELECT * FROM V_GPM_EmpMenu WHERE FK_Emp=" + SystemConfig.AppCenterDBVarStr + "FK_Emp AND ParentNo=" + SystemConfig.AppCenterDBVarStr
                + "ParentNo AND FK_App=" + SystemConfig.AppCenterDBVarStr + "FK_App ORDER BY Idx";
            //不显示功能点
            if (!isVisibleFunPoint)
            {
                ps.SQL = "SELECT * FROM V_GPM_EmpMenu WHERE FK_Emp=" + SystemConfig.AppCenterDBVarStr + "FK_Emp AND ParentNo=" + SystemConfig.AppCenterDBVarStr
                               + "ParentNo AND FK_App=" + SystemConfig.AppCenterDBVarStr + "FK_App AND MenuType <> 5 ORDER BY Idx";
            }
            ps.Add("FK_Emp", userNo);
            ps.Add("FK_App", API.CurrApp.No);
            ps.Add("ParentNo", menuNO);
            return CCPortal.DA.DBAccess.RunSQLReturnTable(ps);
        }
        /// <summary>
        /// 按实体类的方式,返回用户菜单.
        /// </summary>
        /// <param name="userNo">用户编号</param>
        /// <returns>菜单集合</returns>
        public static Menus GetUserMenuOfEntities(string userNo)
        {
            Menus ens = new Menus();
            ens.Init(API.GetUserMenuOfDatatable(userNo));
            return ens;
        }
        /// <summary>
        /// 检查是否可以使用该权限
        /// </summary>
        /// <param name="userNo">用户编号</param>
        /// <param name="funcNo">功能编号</param>
        /// <returns>返回是否可以使用该权限</returns>
        public static bool IsCanUseFunc(string userNo, string funcNo)
        {
            CCPortal.DA.Paras ps = new DA.Paras();
            ps.SQL = "SELECT * FROM V_GPM_EmpMenu WHERE FK_Emp=" + SystemConfig.AppCenterDBVarStr + "FK_Emp AND FK_App=" + SystemConfig.AppCenterDBVarStr + "FK_App AND FK_Menu=" + SystemConfig.AppCenterDBVarStr + "FK_Menu ";
            ps.Add("FK_Emp", userNo);
            ps.Add("FK_App", API.CurrApp.No);
            ps.Add("FK_Menu", funcNo);
            DataTable dt = CCPortal.DA.DBAccess.RunSQLReturnTable(ps);
            if (dt.Rows.Count == 0)
                return false;
            return true;
        }
        #endregion 菜单的API.

        #region 部门与人员API
        /// <summary>
        /// 根据配置文件中的 FK_Dept_No 部门编号获取子部门
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDeptOfDataTable()
        {
            string fk_Dept = SystemConfig.AppSettings["CCPortal.FK_Dept_No"];
            CCPortal.DA.Paras ps = new DA.Paras();
            ps.SQL = "SELECT * FROM Port_Dept WHERE ParentNo=" + SystemConfig.AppCenterDBVarStr + "ParentNo ORDER BY Idx";
            ps.Add("ParentNo", fk_Dept);
            return CCPortal.DA.DBAccess.RunSQLReturnTable(ps);
        }
        /// <summary>
        /// 根据部门编号获取部门信息
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDeptOfDataTableByNo(string deptNo)
        {
            CCPortal.DA.Paras ps = new DA.Paras();
            ps.SQL = "SELECT * FROM Port_Dept WHERE No=" + SystemConfig.AppCenterDBVarStr + "No";
            ps.Add("No", deptNo);
            return CCPortal.DA.DBAccess.RunSQLReturnTable(ps);
        }
        /// <summary>
        /// 根据部门编号获取部门下的人员
        /// </summary>
        /// <param name="deptNo"></param>
        /// <returns></returns>
        public static DataTable GetEmpOfDataTableByDeptNo(string deptNo)
        {
            CCPortal.DA.Paras ps = new DA.Paras();
            ps.SQL = "SELECT a.* FROM Port_Emp a,Port_DeptEmp b WHERE a.No=b.FK_Emp AND b.FK_Dept=" + SystemConfig.AppCenterDBVarStr + "FK_Dept";
            ps.Add("FK_Dept", deptNo);
            return CCPortal.DA.DBAccess.RunSQLReturnTable(ps);
        }
        #endregion

        #region 操作员的部门api.
        /// <summary>
        /// 按datatable的方式,返回用户的部门
        /// </summary>
        /// <param name="userNo">用户编号</param>
        /// <returns>菜单:它的列与GPM数据库中的表Port_Dept一致.</returns>
        public static DataTable GetUserDeptsOfDatatable(string userNo)
        {
            CCPortal.DA.Paras ps = new DA.Paras();
            ps.SQL = "SELECT * FROM Port_Dept WHERE No IN (SELECT FK_Dept FROM Port_DeptEmp WHERE FK_Emp=" + SystemConfig.AppCenterDBVarStr + "FK_Emp)";
            ps.Add("FK_Emp", userNo);
            return CCPortal.DA.DBAccess.RunSQLReturnTable(ps);
        }
        /// <summary>
        /// 按实体类的方式,返回用户部门.
        /// </summary>
        /// <param name="userNo">用户编号</param>
        /// <returns>部门集合</returns>
        public static Depts GetUserDeptOfEntities(string userNo)
        {
            Depts ens = new Depts();
            ens.Init(API.GetUserDeptsOfDatatable(userNo));
            return ens;
        }
        #endregion 操作员的部门api.

        #region 操作员的岗位api.
        /// <summary>
        /// 按datatable的方式,返回用户的岗位
        /// </summary>
        /// <param name="userNo">用户编号</param>
        /// <returns>菜单:它的列与GPM数据库中的表Port_Station一致.</returns>
        public static DataTable GetUserStationsOfDatatable(string userNo)
        {
            CCPortal.DA.Paras ps = new DA.Paras();
            ps.SQL = "SELECT * FROM Port_Station WHERE No IN (SELECT FK_Station FROM Port_DeptEmpStation WHERE FK_Emp=" + SystemConfig.AppCenterDBVarStr + "FK_Emp)";
            ps.Add("FK_Emp", userNo);
            return CCPortal.DA.DBAccess.RunSQLReturnTable(ps);
        }
        /// <summary>
        /// 按实体类的方式,返回用户部门.
        /// </summary>
        /// <param name="userNo">用户编号</param>
        /// <returns>岗位集合</returns>
        public static Stations GetUserStationsOfEntities(string userNo)
        {
            Stations ens = new Stations();
            ens.Init(API.GetUserStationsOfDatatable(userNo));
            return ens;
        }
        #endregion 操作员的岗位api.

        #region 操作员个人信息的api ，用于在用户登录时，把个人信息放在 session里.
        /// <summary>
        /// 用户登录验证
        /// </summary>
        /// <param name="empNo">登录帐号</param>
        /// <param name="pwd">密码</param>
        /// <returns>是否验证成功：true 成功,false 失败</returns>
        public static bool Emp_Login(string empNo, string pwd)
        {
            //将密码加密
            if (!string.IsNullOrEmpty(pwd)) pwd = Crypto.EncryptString(pwd);
            //增加工号登陆
            string sql = "SELECT COUNT(No) Num FROM Port_Emp WHERE (EmpNo='" + empNo + "' OR No='" + empNo + "') and Pass='" + pwd + "'";
            object objVal = CCPortal.DA.DBAccess.RunSQLReturnVal(sql);
            if (objVal != null && objVal.ToString() != "")
            {
                int resultVal = Convert.ToInt32(objVal.ToString());
                if (resultVal > 0)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <param name="userNo">人员编号</param>
        /// <returns>个人信息Port_Emp表里的所有字段信息。</returns>
        public static Hashtable GetEmpInfo(string userNo)
        {
            //设置用户ID
            Set_Port_Emp_SID(userNo);
            //查询出来个人信息。
            CCPortal.DA.Paras ps = new DA.Paras();
            ps.SQL = "SELECT * FROM Port_Emp WHERE No =" + SystemConfig.AppCenterDBVarStr + "No";
            ps.Add("No", userNo);
            DataTable dt = CCPortal.DA.DBAccess.RunSQLReturnTable(ps);

            //把数据放在ht里面。
            Hashtable ht = new Hashtable();
            if (dt != null && dt.Rows.Count > 0)
            {
                //调用查询日志

                foreach (DataColumn item in dt.Columns)
                    ht.Add(item.ColumnName, dt.Rows[0][item.ColumnName]);

                //查询用户的职务级别
                ps = new DA.Paras();
                ps.SQL = "SELECT * FROM Port_DeptEmp WHERE FK_Emp=" + SystemConfig.AppCenterDBVarStr + "FK_Emp order by DutyLevel";
                ps.Add("FK_Emp", userNo);
                DataTable dt_Duty = CCPortal.DA.DBAccess.RunSQLReturnTable(ps);
                //判断数据是否存在
                if (dt_Duty != null && dt_Duty.Rows.Count > 0)
                    ht.Add("DutyLevel", dt_Duty.Rows[0]["DutyLevel"].ToString());
            }
            return ht;
        }
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="userNo">登陆帐号</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static bool SetEmpPassword(string userNo, string pwd)
        {
            string sql = "UPDATE Port_Emp SET Pass='{0}' WHERE No='{1}'";
            
            //将密码加密
            if (!string.IsNullOrEmpty(pwd)) pwd = Crypto.EncryptString(pwd);

            sql = string.Format(sql, pwd, userNo);
            int i = CCPortal.DA.DBAccess.RunSQL(sql);
            if (i > 0)
                return true;
            return false;
        }
        /// <summary>
        /// 设置用户的SID
        /// </summary>
        /// <param name="userNo"></param>
        private static void Set_Port_Emp_SID(string userNo)
        {
            //string sid1 = DateTime.Now.ToString("MMddHHmmss");
            //过期时间一天
            //string outTime = DateTime.Now.ToString("MMdd");
            //string sql = "SELECT SID FROM Port_Emp WHERE No = '" + userNo + "'";
            //string sid = CCPortal.DA.DBAccess.RunSQLReturnString(sql);
            //如果小时时间不相同就改变sid
            //if (sid == null || sid.Length < outTime.Length || sid.Substring(0, outTime.Length) != outTime)
            //{
            //    CCPortal.DA.DBAccess.RunSQL("UPDATE Port_Emp SET SID='" + sid1 + "' WHERE No='" + userNo + "'");
            //}
            //加入登录日志
            string OID = CCPortal.DA.DBAccess.GenerOID("BP.GPM.SystemLoginLog").ToString();
            string FK_App = API.CurrApp.No;
            string FK_Emp = userNo;
            string customeIP = System.Web.HttpContext.Current.Request.UserHostAddress;
            string sql = "INSERT INTO GPM_SystemLoginLog(OID,FK_Emp,FK_App,RContent,LoginDateTime,IP) VALUES({0},'{1}','{2}','{3}','{4}','{5}')";
            sql = string.Format(sql, OID, FK_Emp, FK_App, "API登录", DateTime.Now.ToString(), customeIP);
            CCPortal.DA.DBAccess.RunSQL(sql);
        }

        #endregion

        #region 页面的服务端控件按钮权限控制 API
        /// <summary>
        /// 各页面的服务端控件按钮权限控制
        /// </summary>
        /// <param name="page">页面实例</param>
        /// <param name="userNo">登陆用户实例</param>
        /// <param name="parentMenuNo">菜单父节点编号</param>
        public static void SetPageCtrl(Page page, string userNo, string parentMenuNo)
        {
            DataTable menu_dt = GetUserMenuOfDatatableByPNo(userNo, parentMenuNo);
            //功能控制点集合
            string allowCtrlFlag = "";
            if (menu_dt != null && menu_dt.Rows.Count > 0)
            {
                foreach (DataRow row in menu_dt.Rows)
                {
                    //获取功能控制点
                    if (row["MenuType"].ToString() == "5")
                    {
                        allowCtrlFlag += "," + row["Flag"].ToString() + ",";
                    }
                }
            }

            //如果该页面有控制点
            if (!string.IsNullOrEmpty(allowCtrlFlag))
            {
                //获取页面的所有控件
                ControlCollection controls = page.Form == null ? page.Controls : page.Form.Controls;

                foreach (Control ctrl in controls)
                {
                    if (ctrl is Button)//如果是按钮
                    {
                        if (allowCtrlFlag.Contains(ctrl.ID) == false && allowCtrlFlag.Contains(ctrl.ClientID) == false)
                        {
                            Button btn = (Button)ctrl;
                            btn.Visible = false;
                            btn.Enabled = false;
                            btn.OnClientClick = null;
                        }
                    }
                    else if (ctrl is LinkButton)//如果是超链接
                    {
                        if (allowCtrlFlag.Contains(ctrl.ID) == false && allowCtrlFlag.Contains(ctrl.ClientID) == false)
                        {
                            LinkButton linkBtn = (LinkButton)ctrl;
                            linkBtn.Visible = false;
                            linkBtn.Enabled = false;
                            linkBtn.OnClientClick = null;
                        }
                    }
                    else if (ctrl is HtmlButton)
                    {
                        if (allowCtrlFlag.Contains(ctrl.ID) == false && allowCtrlFlag.Contains(ctrl.ClientID) == false)
                        {
                            HtmlButton htmlBtn = (HtmlButton)ctrl;
                            htmlBtn.Attributes.CssStyle["display"] = "none";
                            htmlBtn.Attributes["onclick"] = "";
                        }
                    }
                    else if (ctrl is HtmlInputButton)
                    {
                        if (allowCtrlFlag.Contains(ctrl.ID) == false && allowCtrlFlag.Contains(ctrl.ClientID) == false)
                        {
                            HtmlInputButton inputBtn = (HtmlInputButton)ctrl;
                            inputBtn.Attributes.CssStyle["display"] = "none";
                            inputBtn.Attributes["onclick"] = "";
                        }
                    }
                }
            }
        }
        #endregion
    }
}
