using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Xml;

using BP;
using BP.En;
using BP.DA;
using BP.Web;
using BP.GPM;
using BP.GPM.Utility;
using BP.Sys;

namespace GMP2.GPM
{
    public partial class DataService : WebPage
    {
        public string getUTF8ToString(string param)
        {
            return HttpUtility.UrlDecode(Request[param], System.Text.Encoding.UTF8);
        }
        //页面加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (BP.Web.WebUser.No == null)
                return;

            string method = string.Empty;
            //返回值
            string s_responsetext = string.Empty;
            if (!string.IsNullOrEmpty(Request["method"]))
                method = Request["method"].ToString();

            switch (method)
            {
                case "getemps"://获取所有人员信息
                    s_responsetext = GetEmps();
                    break;
                case "getempsbynoorname"://根据用户名或编号模糊查找用户
                    s_responsetext = GetEmpsByNoOrName();
                    break;
                case "getempgroups"://查找所有权限组
                    s_responsetext = GetEmpGroups();
                    break;
                case "getempgroupsbyname"://权限组模糊查找
                    s_responsetext = GetEmpGroupsByName();
                    break;
                case "getapps"://获取所有系统
                    s_responsetext = GetApps();
                    break;
                case "getmenusofmenuforemp"://获取所有目录菜单
                    s_responsetext = GetMenusOfMenuForEmp();
                    break;
                case "getmenusbyid"://根据编号获取菜单
                    s_responsetext = GetMenusById();
                    break;
                case "getleftmenu"://左侧菜单
                    s_responsetext = GetLeftMenu();
                    break;
                case "getsystemmenu":// 获取系统菜单
                    s_responsetext = GetSystemMenus();
                    break;
                case "menunodemanage"://菜单管理
                    s_responsetext = MenuNodeManage();
                    break;
                case "getmenubyempno"://根据用户编号查找菜单
                    s_responsetext = GetMenuByEmpNo();
                    break;
                case "getdeptemptree"://获取部门人员信息
                    s_responsetext = GetDeptEmpTree();
                    break;
                case "getdeptempchildnodes"://根据节点编号获取子部门人员
                    s_responsetext = GetDeptEmpChildNodes();
                    break;
                case "getempofmenusbyempno"://用户菜单权限
                    s_responsetext = GetEmpOfMenusByEmpNo();
                    break;
                case "saveuserofmenus"://保存用户与菜单的对应关系
                    s_responsetext = SaveUserOfMenus();
                    break;
                case "getempgroupofmenusbyno"://获取权限组菜单
                    s_responsetext = GetEmpGroupOfMenusByNo();
                    break;
                case "saveusergroupofmenus"://保存权限组菜单
                    s_responsetext = SaveUserGroupOfMenus();
                    break;
                case "clearofcopyuserpower"://清空式复制用户权限
                    s_responsetext = ClearOfCopyUserPower();
                    break;
                case "coverofcopyuserpower"://覆盖式复制用户权限
                    s_responsetext = CoverOfCopyUserPower();
                    break;
                case "clearofcopyusergrouppower"://清空式复制权限组权限
                    s_responsetext = ClearOfCopyUserGroupPower();
                    break;
                case "coverofcopyusergrouppower"://覆盖式覆盖权限组
                    s_responsetext = CoverOfCopyUserGroupPower();
                    break;
                case "getAppChildMenus"://打开新窗口 菜单获取
                    s_responsetext = getMenu();
                    break;
                case "GetAllDept"://获取所有部门
                    s_responsetext = GetAllDept();
                    break;
                case "gettemplatedata"://按菜单分配权限，获取模版数据
                    s_responsetext = getTemplateData();
                    break;
                case "savemenuforemp"://保存按菜单分配权限
                    s_responsetext = SaveMenuForEmp();
                    break;
                case "getsystemloginlogs"://获取系统登录日志
                    s_responsetext = GetSystemLoginLogs();
                    break;
                case "getstations"://获取所有岗位
                    s_responsetext = GetStations();
                    break;
                case "savestationofmenus":// 保存岗位菜单
                    s_responsetext = SaveStationOfMenus();
                    break;
                case "getstationofmenusbyno"://获取岗位菜单
                    s_responsetext = GetStationOfMenusByNo();
                    break;
                case "clearofcopystation"://清空式 复制岗位
                    s_responsetext = ClearOfCopyStation();
                    break;
                case "coverofcopystation"://覆盖式 复制岗位
                    s_responsetext = CoverOfCopyStation();
                    break;
                case "getstationbyname":// 岗位 模糊查找
                    s_responsetext = GetStationByName();
                    break;
            }
            if (string.IsNullOrEmpty(s_responsetext))
                s_responsetext = "";
            //组装ajax字符串格式,返回调用客户端
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "text/html";
            Response.Expires = 0;
            Response.Write(s_responsetext);
            Response.End();
        }

        #region  按岗位分配菜单
        /// <summary>
        /// 获取所有岗位
        /// </summary>
        /// <returns></returns>
        public string GetStations()
        {
            Stations stations = new Stations();
            stations.RetrieveAll("No");
            return TranslateEntitiesToGridJsonOnlyData(stations);
        }
        /// <summary>
        /// 保存 岗位 菜单
        /// </summary>
        /// <returns></returns>
        public string SaveStationOfMenus()
        {
            try
            {
                string stationNo = getUTF8ToString("stationNo");
                string menuIds = getUTF8ToString("menuIds");
                string menuIdsUn = getUTF8ToString("menuIdsUn");
                string menuIdsUnExt = getUTF8ToString("menuIdsUnExt");

                //将未展开项包含的子项补充到已选择和未选择项中
                if (!string.IsNullOrEmpty(menuIdsUnExt))
                {
                    string[] menuParentNos = menuIdsUnExt.Split(',');
                    foreach (string item in menuParentNos)
                    {
                        SetUnCheckedStationOfMenus(stationNo, item, ref menuIds, ref menuIdsUn);
                    }
                }
                //删除该岗位下的菜单
                StationMenus stationMenus = new StationMenus();
                stationMenus.Delete(StationMenuAttr.FK_Station, stationNo);

                //保存选中菜单
                if (!string.IsNullOrEmpty(menuIds))
                {
                    string[] menus = menuIds.Split(',');
                    foreach (string item in menus)
                    {
                        StationMenu stationMenu = new StationMenu();
                        stationMenu.FK_Station = stationNo;
                        stationMenu.FK_Menu = item;
                        stationMenu.IsChecked = "1";
                        stationMenu.Insert();

                        SaveStationOfMenusChild(stationNo, item, menuIds);
                    }
                }
                //保存未完全选中项
                if (!string.IsNullOrEmpty(menuIdsUn))
                {
                    string[] menus = menuIdsUn.Split(',');
                    foreach (string item in menus)
                    {
                        StationMenu stationMenu = new StationMenu();
                        stationMenu.FK_Station = stationNo;
                        stationMenu.FK_Menu = item;
                        stationMenu.IsChecked = "0";
                        stationMenu.Insert();
                    }
                }
                //处理未完全选择项，不包含子项的未完全选择项删除
                Del_UnCheckedNoChildNodes("StationMenu", stationNo);
            }
            catch (Exception ex)
            {
                return "error" + ex.Message;
            }
            return "success";
        }
        /// <summary>
        /// 保存岗位菜单 子节点
        /// </summary>
        /// <returns></returns>
        public void SaveStationOfMenusChild(string stationNo, string parentNo, string menuIds)
        {
            //根据父节点编号获取子节点
            BP.GPM.Menus menus = new BP.GPM.Menus();
            menus.RetrieveByAttr("ParentNo", parentNo);

            foreach (BP.GPM.Menu item in menus)
            {
                if (menuIds.Contains(item.No))
                    continue;

                StationMenu stationMenu = new StationMenu();
                stationMenu.FK_Station = stationNo;
                stationMenu.FK_Menu = item.No;
                stationMenu.IsChecked = "1";
                stationMenu.Insert();

                SaveStationOfMenusChild(stationNo, item.No, menuIds);
            }

        }
        /// <summary>
        /// 获取岗位菜单
        /// </summary>
        public string GetStationOfMenusByNo()
        {
            string checkedMenuIds = "";
            string stationNo = getUTF8ToString("stationNo");
            string parentNo = getUTF8ToString("parentNo");
            string isLoadChild = getUTF8ToString("isLoadChild");
            //根据岗位编号获取菜单
            StationMenus stationMenus = new StationMenus();


            QueryObject objWhere = new QueryObject(stationMenus);
            objWhere.AddWhere(StationMenuAttr.FK_Station, stationNo);
            objWhere.addAnd();
            objWhere.AddWhere(StationMenuAttr.IsChecked, true);
            objWhere.DoQuery();

            //获取节点
            BP.GPM.Menus menus = new BP.GPM.Menus();
            menus.RetrieveByAttr("ParentNo", parentNo);

            //整理选中项
            foreach (StationMenu item in stationMenus)
            {
                checkedMenuIds += "," + item.FK_Menu + ",";
            }
            //整理未完全选中
            string unCheckedMenuIds = "";
            StationMenus unCStationMenus = new StationMenus();

            QueryObject unObjWhere = new QueryObject(unCStationMenus);
            unObjWhere.AddWhere(StationMenuAttr.FK_Station, stationNo);
            unObjWhere.addAnd();
            unObjWhere.AddWhere(StationMenuAttr.IsChecked, false);
            unObjWhere.DoQuery();
            foreach (StationMenu unItem in unCStationMenus)
            {
                unCheckedMenuIds += "," + unItem.FK_Menu + ",";
            }

            //如果是第一次加载
            if (isLoadChild == "false")
            {
                StringBuilder appSend = new StringBuilder();
                appSend.Append("[");
                foreach (EntityTree item in menus)
                {
                    if (appSend.Length > 1) appSend.Append(",{"); else appSend.Append("{");

                    appSend.Append("\"id\":\"" + item.No + "\"");
                    appSend.Append(",\"text\":\"" + item.Name + "\"");

                    BP.GPM.Menu menu = item as BP.GPM.Menu;

                    //节点图标
                    string ico = "icon-" + menu.MenuType;
                    //判断未完全选中
                    if (unCheckedMenuIds.Contains("," + item.No + ","))
                        ico = "collaboration";

                    appSend.Append(",iconCls:\"");
                    appSend.Append(ico);
                    appSend.Append("\"");

                    //判断选中
                    if (checkedMenuIds.Contains("," + item.No + ","))
                        appSend.Append(",\"checked\":true");

                    // 增加它的子级.
                    appSend.Append(",\"children\":");
                    appSend.Append(GetMenusByParentNo(item.No, checkedMenuIds, unCheckedMenuIds, true));
                    appSend.Append("}");
                }
                appSend.Append("]");

                return appSend.ToString();
            }
            //返回获取的子节点
            return GetTreeList(menus, checkedMenuIds, unCheckedMenuIds);
        }

        /// <summary>
        /// 清空式 复制岗位  保存
        /// </summary>
        /// <returns></returns>
        public string ClearOfCopyStation()
        {
            try
            {
                string copyStationNo = getUTF8ToString("copyStationNo");
                string pastStationNos = getUTF8ToString("pastStationNos");
                string[] pastArry = pastStationNos.Split(',');

                //获取复制岗位权限
                StationMenus copyStationMenus = new StationMenus();
                copyStationMenus.RetrieveByAttr(StationMenuAttr.FK_Station, copyStationNo);

                //循环目标对象
                foreach (string pastStation in pastArry)
                {
                    //清空之前的权限
                    StationMenu stationMenu = new StationMenu();
                    stationMenu.Delete(StationMenuAttr.FK_Station, pastStation);
                    //授权
                    foreach (StationMenu copyMenu in copyStationMenus)
                    {
                        stationMenu = new StationMenu();
                        stationMenu.FK_Station = pastStation;
                        stationMenu.FK_Menu = copyMenu.FK_Menu;
                        stationMenu.IsChecked = copyMenu.IsChecked;

                        stationMenu.Insert();
                    }
                }
            }
            catch (Exception ex)
            {
                return "error:" + ex.Message;
            }
            return "success";
        }

        /// <summary>
        /// 覆盖式 复制岗位 保存
        /// </summary>
        /// <returns></returns>
        public string CoverOfCopyStation()
        {
            try
            {
                string copyStationNo = getUTF8ToString("copyStationNo");
                string pastStationNos = getUTF8ToString("pastStationNos");
                string[] pastArry = pastStationNos.Split(',');

                //获取复制岗位 权限
                StationMenus copyStationMenus = new StationMenus();
                copyStationMenus.RetrieveByAttr(StationMenuAttr.FK_Station, copyStationNo);

                //循环目标对象
                foreach (string pastStaion in pastArry)
                {
                    //授权
                    foreach (StationMenu copyMenu in copyStationMenus)
                    {
                        StationMenu stationMenu = new StationMenu();

                        bool isHave = stationMenu.RetrieveByAttrAnd(StationMenuAttr.FK_Station, pastStaion, StationMenuAttr.FK_Menu, copyMenu.FK_Menu);
                        //判断之前的权限是否存在
                        if (!isHave)
                        {
                            stationMenu = new StationMenu();
                            stationMenu.FK_Station = pastStaion;
                            stationMenu.FK_Menu = copyMenu.FK_Menu;
                            stationMenu.IsChecked = copyMenu.IsChecked;

                            stationMenu.Insert();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "error:" + ex.Message;
            }
            return "success";
        }
        #endregion

        /// <summary>
        /// 岗位 模糊 查找
        /// </summary>
        /// <returns></returns>
        public string GetStationByName()
        {
            string stationName = getUTF8ToString("stationName");
            Stations stations = new Stations();
            QueryObject qo = new QueryObject(stations);
            qo.AddWhere(StationAttr.Name, " LIKE ", "'%" + stationName + "%'");
            qo.addOr();
            qo.AddWhere(StationAttr.No, " LIKE ", "'%" + stationName + "%'");
            qo.DoQuery();

            return TranslateEntitiesToGridJsonOnlyData(stations);
        }

        /// <summary>
        /// 获取模板数据
        /// </summary>
        /// <returns></returns>
        public string getTemplateData()
        {
            string sql = "";
            string menuNo = getUTF8ToString("menuNo");
            string model = getUTF8ToString("model");
            //按岗位分配
            if (model == "station")
            {
                sql = "SELECT a.No,a.Name"
                            + ",(case b.IsChecked "
                            + "when 1 then 1 "
                            + "when 0 then 1 "
                            + "else 5 end) as isCheck "
                            + "FROM Port_Station a "
                            + "left join GPM_StationMenu b "
                            + "on a.No=b.FK_Station "
                            + "and b.FK_Menu=" + menuNo
                            + " order by a.No";
                //获取所有岗位
                StationMenu station = new StationMenu();
                DataTable dt_StationMenu = station.RunSQLReturnTable(sql);
                string rVal = CommonDbOperator.GetListJsonFromTable(dt_StationMenu);
                rVal = "{station:" + rVal + "}";
                return rVal;
            }
            //按权限组分配
            if (model == "group")
            {
                sql = "SELECT a.No,a.Name"
                            + ",(case b.IsChecked "
                            + "when 1 then 1 "
                            + "when 0 then 1 "
                            + "else 5 end) as isCheck "
                            + "FROM GPM_Group a "
                            + "left join GPM_GroupMenu b "
                            + "on a.No=b.FK_Group "
                            + "and b.FK_Menu=" + menuNo
                            + " order by Idx";
                //获取所有权限组
                Group group = new Group();
                DataTable dt_GroupMenu = group.RunSQLReturnTable(sql);

                string rVal = CommonDbOperator.GetListJsonFromTable(dt_GroupMenu);
                rVal = "{group:" + rVal + "}";
                return rVal;
            }
            //按用户分配菜单
            sql = "SELECT distinct a.No,a.Name,a.FK_Dept,"
                        + "(case b.IsChecked "
                        + " when 1 then 1"
                        + " when 0 then 1"
                        + " else 5"
                        + " end) isCheck"
                        + " FROM Port_Emp a left join V_GPM_EmpMenu_GPM b"
                        + " on a.No=b.FK_Emp"
                        + " and b.FK_Menu =" + menuNo
                        + " order by a.Name";
            string strdept = GetEmpDeptInfo();
            Emp emp = new Emp();
            DataTable dt_Emp = emp.RunSQLReturnTable(sql);
            string stremp = CommonDbOperator.GetListJsonFromTable(dt_Emp);
            return "{bmList:" + strdept + ",empList:" + stremp + "}";
        }
        /// <summary>
        /// 保存按菜单分配权限
        /// </summary>
        /// <returns></returns>
        private string SaveMenuForEmp()
        {
            try
            {
                string menuNo = getUTF8ToString("menuNo");
                string saveNos = getUTF8ToString("ckNos");
                string curModel = getUTF8ToString("model");
                string saveChildNode = getUTF8ToString("saveChildNode");
                string[] str_Arrary = saveNos.Split(',');
                //按用户分配权限
                if (curModel == "emp")
                {
                    //删除菜单下的所有用户
                    UserMenus userMenus = new UserMenus();
                    userMenus.Delete("FK_Menu", menuNo);
                    //对用户进行授权
                    foreach (string item in str_Arrary)
                    {
                        UserMenu userMenu = new UserMenu();
                        userMenu.FK_Emp = item;
                        userMenu.FK_Menu = menuNo;
                        userMenu.IsChecked = "1";
                        userMenu.Insert();

                        //保存子菜单
                        if (saveChildNode == "true")
                            SaveUserOfMenusChild(item, menuNo, menuNo);
                    }
                    return "true";
                }
                //按岗位分配权限
                if (curModel == "station")
                {
                    //删除菜单下的所有岗位
                    StationMenus staMenus = new StationMenus();
                    staMenus.Delete("FK_Menu", menuNo);

                    //对用户进行授权
                    foreach (string item in str_Arrary)
                    {
                        StationMenu stationMenu = new StationMenu();
                        stationMenu.FK_Station = item;
                        stationMenu.FK_Menu = menuNo;
                        stationMenu.IsChecked = "1";
                        stationMenu.Insert();
                        //保存子菜单
                        if (saveChildNode == "true")
                            SaveStationOfMenusChild(item, menuNo, menuNo);
                    }
                    return "true";
                }
                //删除菜单下的权限组
                GroupMenus groupMenus = new GroupMenus();
                groupMenus.Delete(GroupMenuAttr.FK_Menu, menuNo);
                //对权限组进行授权
                foreach (string item in str_Arrary)
                {
                    GroupMenu groupMenu = new GroupMenu();
                    groupMenu.FK_Group = item;
                    groupMenu.FK_Menu = menuNo;
                    groupMenu.IsChecked = "1";
                    groupMenu.Insert();
                    //对子节点进行授权
                    if (saveChildNode == "true")
                        SaveGroupOfMenusChild(item, menuNo, menuNo);
                }
                return "true";
            }
            catch (Exception ex)
            {
                return "false";
            }
        }
        /// <summary>
        /// 获取包含人员的部门
        /// </summary>
        /// <returns></returns>
        public string GetEmpDeptInfo()
        {
            string sql = "SELECT distinct Port_Dept.No,Port_Dept.Name,Port_Dept.ParentNo,Port_Dept.Idx "
                + "FROM Port_Dept,port_emp  "
                + "where Port_Emp.FK_Dept = Port_Dept.No order by Port_Dept.ParentNo,Port_Dept.Idx";
            Dept dept = new Dept();
            DataTable dt = dept.RunSQLReturnTable(sql);
            return CommonDbOperator.GetListJsonFromTable(dt);
        }
        /// <summary>
        /// 获取 所有部门
        /// </summary>
        /// <returns></returns>
        public string GetAllDept()
        {
            Depts depts = new Depts();
            depts.RetrieveAll();
            return TranslateEntitiesToGridJsonOnlyData(depts);
        }

        /// <summary>
        /// 获取所有人员信息
        /// </summary>
        /// <returns></returns>
        private string GetEmps()
        {
            Emps emps = new Emps();
            emps.RetrieveAll("No");
            return TranslateEntitiesToGridJsonOnlyData(emps);
        }
        /// <summary>
        /// 根据用户编号或名称模糊查询
        /// </summary>
        /// <returns></returns>
        private string GetEmpsByNoOrName()
        {
            string objSearch = getUTF8ToString("objSearch");
            Emps emps = new Emps();
            BP.En.QueryObject qo = new QueryObject(emps);

            qo.AddWhere(EmpAttr.No, " LIKE ", "'%" + objSearch + "%'");
            qo.addOr();
            qo.AddWhere(EmpAttr.Name, " LIKE ", "'%" + objSearch + "%'");
            qo.addOr();
            qo.AddWhere(EmpAttr.EmpNo, " LIKE ", "'%" + objSearch + "%'");

            qo.DoQuery();
            return TranslateEntitiesToGridJsonOnlyData(emps);
        }

        /// <summary>
        /// 查找所有权限组
        /// </summary>
        /// <returns></returns>
        private string GetEmpGroups()
        {
            Groups groups = new Groups();
            groups.RetrieveAll(GroupAttr.Idx);
            return TranslateEntitiesToGridJsonOnlyData(groups);
        }
        /// <summary>
        /// 权限组模糊查找
        /// </summary>
        /// <returns></returns>
        private string GetEmpGroupsByName()
        {
            string objSearch = getUTF8ToString("objSearch");
            Groups groups = new Groups();
            QueryObject qo = new QueryObject(groups);
            qo.AddWhere(GroupAttr.Name, " LIKE ", "'%" + objSearch + "%'");
            qo.addOr();
            qo.AddWhere(GroupAttr.No, " LIKE ", "'%" + objSearch + "%'");
            qo.DoQuery();

            return TranslateEntitiesToGridJsonOnlyData(groups);
        }
        /// <summary>
        /// 保存用户与菜单的关系
        /// </summary>
        /// <returns></returns>
        private string SaveUserOfMenus()
        {
            try
            {
                string empNo = getUTF8ToString("empNo");
                string menuIds = getUTF8ToString("menuIds");
                string menuIdsUn = getUTF8ToString("menuIdsUn");
                string menuIdsUnExt = getUTF8ToString("menuIdsUnExt");

                //将未展开项包含的子项补充到已选择和未选择项中
                if (!string.IsNullOrEmpty(menuIdsUnExt))
                {
                    string[] menuParentNos = menuIdsUnExt.Split(',');
                    foreach (string item in menuParentNos)
                    {
                        SetUnCheckedUserOfMenus(empNo, item, ref menuIds, ref menuIdsUn);
                    }
                }

                //删除用户下的菜单
                UserMenus userMenus = new UserMenus();
                userMenus.Delete("FK_Emp", empNo);
                //保存选中菜单
                if (!string.IsNullOrEmpty(menuIds))
                {
                    string[] menus = menuIds.Split(',');
                    foreach (string item in menus)
                    {
                        UserMenu userMenu = new UserMenu();
                        userMenu.FK_Emp = empNo;
                        userMenu.FK_Menu = item;
                        userMenu.IsChecked = "1";
                        userMenu.Insert();

                        SaveUserOfMenusChild(empNo, item, menuIds);
                    }
                }
                //保存未完全选中项
                if (!string.IsNullOrEmpty(menuIdsUn))
                {
                    string[] menus = menuIdsUn.Split(',');
                    foreach (string item in menus)
                    {
                        UserMenu userMenu = new UserMenu();
                        userMenu.FK_Emp = empNo;
                        userMenu.FK_Menu = item;
                        userMenu.IsChecked = "0";
                        userMenu.Insert();
                    }
                }
                //处理未完全选择项，不包含子项的未完全选择项删除
                Del_UnCheckedNoChildNodes("UserMenu", empNo);
            }
            catch (Exception ex)
            {
                return "error" + ex.Message;
            }
            return "success";
        }

        /// <summary>
        /// 保存用户菜单子节点
        /// </summary>
        private void SaveUserOfMenusChild(string fk_EmpNo, string parentNo, string menuIds)
        {
            //根据父节点编号获取子节点
            BP.GPM.Menus menus = new BP.GPM.Menus();
            menus.RetrieveByAttr("ParentNo", parentNo);

            foreach (BP.GPM.Menu cMenu in menus)
            {
                if (menuIds.Contains(cMenu.No))
                    continue;

                UserMenu userMenu = new UserMenu();
                userMenu.FK_Emp = fk_EmpNo;
                userMenu.FK_Menu = cMenu.No;
                userMenu.IsChecked = "1";
                userMenu.Insert();

                SaveUserOfMenusChild(fk_EmpNo, cMenu.No, menuIds);
            }
        }
        /// <summary>
        /// 设置未展开项
        /// </summary>
        /// <param name="fk_EmpNo">用户编号</param>
        /// <param name="parentNo">父节点编号</param>
        /// <param name="menuIds">选择项，进行拼接</param>
        /// <param name="menuIdsUn">未完全选中项，进行拼接</param>
        private void SetUnCheckedUserOfMenus(string fk_EmpNo, string parentNo, ref string menuIds, ref string menuIdsUn)
        {
            //根据父节点编号获取子节点
            BP.GPM.Menu menu = new BP.GPM.Menu();
            string sql = "SELECT a.FK_Emp,a.FK_Menu,a.IsChecked FROM GPM_UserMenu a,GPM_Menu b "
                            + " WHERE a.FK_Menu = b.No "
                            + " AND b.ParentNo='" + parentNo + "'"
                            + " AND a.FK_Emp='" + fk_EmpNo + "'";
            //获取数据集
            DataTable dt = menu.RunSQLReturnTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    //未完全选中
                    if (row["IsChecked"].ToString() == "0")
                    {
                        if (!menuIdsUn.Contains(row["FK_Menu"].ToString()))
                            menuIdsUn += "," + row["FK_Menu"];
                    }
                    //选中
                    if (row["IsChecked"].ToString() == "1")
                    {
                        if (string.IsNullOrEmpty(menuIds))
                        {
                            menuIds = row["FK_Menu"].ToString();
                        }
                        else
                        {
                            if (!menuIds.Contains(row["FK_Menu"].ToString()))
                                menuIds += "," + row["FK_Menu"];
                        }
                    }
                    //迭代进行处理
                    SetUnCheckedUserOfMenus(fk_EmpNo, row["FK_Menu"].ToString(), ref menuIds, ref menuIdsUn);
                }
            }
        }
        /// <summary>
        /// 设置未展开项
        /// </summary>
        /// <param name="fk_StationNo">岗位编号</param>
        /// <param name="parentNo">父节点编号</param>
        /// <param name="menuIds">选择项，进行拼接</param>
        /// <param name="menuIdsUn">未完全选中项，进行拼接</param>
        private void SetUnCheckedStationOfMenus(string fk_StationNo, string parentNo, ref string menuIds, ref string menuIdsUn)
        {
            //根据父节点编号获取子节点
            BP.GPM.Menu menu = new BP.GPM.Menu();
            string sql = "SELECT a.FK_Station,a.FK_Menu,a.IsChecked FROM GPM_StationMenu a,GPM_Menu b "
                            + " WHERE a.FK_Menu = b.No "
                            + " AND b.ParentNo='" + parentNo + "'"
                            + " AND a.FK_Station='" + fk_StationNo + "'";
            //获取数据集
            DataTable dt = menu.RunSQLReturnTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    //未完全选中
                    if (row["IsChecked"].ToString() == "0")
                    {
                        if (!menuIdsUn.Contains(row["FK_Menu"].ToString()))
                            menuIdsUn += "," + row["FK_Menu"];
                    }
                    //选中
                    if (row["IsChecked"].ToString() == "1")
                    {
                        if (string.IsNullOrEmpty(menuIds))
                        {
                            menuIds = row["FK_Menu"].ToString();
                        }
                        else
                        {
                            if (!menuIds.Contains(row["FK_Menu"].ToString()))
                                menuIds += "," + row["FK_Menu"];
                        }
                    }
                    //迭代进行处理
                    SetUnCheckedStationOfMenus(fk_StationNo, row["FK_Menu"].ToString(), ref menuIds, ref menuIdsUn);
                }
            }
        }

        /// <summary>
        /// 设置未展开项
        /// </summary>
        /// <param name="FK_Group">权限组编号</param>
        /// <param name="parentNo">父节点编号</param>
        /// <param name="menuIds">选择项，进行拼接</param>
        /// <param name="menuIdsUn">未完全选中项，进行拼接</param>
        private void SetUnCheckedGroupOfMenus(string FK_Group, string parentNo, ref string menuIds, ref string menuIdsUn)
        {
            //根据父节点编号获取子节点
            BP.GPM.Menu menu = new BP.GPM.Menu();
            string sql = "SELECT a.FK_Group,a.FK_Menu,a.IsChecked FROM GPM_GroupMenu a,GPM_Menu b "
                            + " WHERE a.FK_Menu = b.No "
                            + " AND b.ParentNo='" + parentNo + "'"
                            + " AND a.FK_Group='" + FK_Group + "'";
            //获取数据集
            DataTable dt = menu.RunSQLReturnTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    //未完全选中
                    if (row["IsChecked"].ToString() == "0")
                    {
                        if (!menuIdsUn.Contains(row["FK_Menu"].ToString()))
                            menuIdsUn += "," + row["FK_Menu"];
                    }
                    //选中
                    if (row["IsChecked"].ToString() == "1")
                    {
                        if (string.IsNullOrEmpty(menuIds))
                        {
                            menuIds = row["FK_Menu"].ToString();
                        }
                        else
                        {
                            if (!menuIds.Contains(row["FK_Menu"].ToString()))
                                menuIds += "," + row["FK_Menu"];
                        }
                    }
                    //迭代进行处理
                    SetUnCheckedGroupOfMenus(FK_Group, row["FK_Menu"].ToString(), ref menuIds, ref menuIdsUn);
                }
            }
        }
        /// <summary>
        /// 将未完全选择项，不包含子节点的节点删除
        /// </summary>
        /// <param name="saveType"></param>
        /// <param name="FK_Val"></param>
        private void Del_UnCheckedNoChildNodes(string saveType, string FK_Val)
        {
            string sql = "";
            //保存用户菜单
            if (saveType == "UserMenu")
            {
                UserMenus userMenus = new UserMenus();
                userMenus.Retrieve("FK_Emp", FK_Val, "IsChecked", "0");
                if (userMenus != null && userMenus.Count > 0)
                {
                    //循环删除子级项，方法有待优化；先删除3级关联
                    for (int i = 0, k = 3; i < k; i++)
                    {
                        foreach (UserMenu userMenu in userMenus)
                        {
                            sql = "SELECT a.FK_Emp,a.FK_Menu,a.IsChecked FROM GPM_UserMenu a,GPM_Menu b "
                                + " WHERE a.FK_Menu = b.No"
                                + " AND b.ParentNo='" + userMenu.FK_Menu + "'"
                                + " AND a.FK_Emp='" + FK_Val + "'";
                            DataTable dt_UserMenu = DBAccess.RunSQLReturnTable(sql);
                            //判断是否含有子项
                            if (dt_UserMenu == null || dt_UserMenu.Rows.Count == 0)
                            {
                                //执行删除
                                DBAccess.RunSQL("DELETE FROM GPM_UserMenu WHERE FK_Emp='" + FK_Val + "' and FK_Menu='" + userMenu.FK_Menu + "'");
                            }
                        }
                    }
                }
            }
            else if (saveType == "StationMenu")//岗位菜单
            {
                StationMenus stationMenus = new StationMenus();
                stationMenus.Retrieve("FK_Station", FK_Val, "IsChecked", "0");
                if (stationMenus != null && stationMenus.Count > 0)
                {
                    //循环删除子级项，方法有待优化；先删除3级关联
                    for (int i = 0, k = 3; i < k; i++)
                    {
                        foreach (StationMenu stationMenu in stationMenus)
                        {
                            sql = "SELECT a.FK_Station,a.FK_Menu,a.IsChecked FROM GPM_StationMenu a,GPM_Menu b "
                                + " WHERE a.FK_Menu = b.No"
                                + " AND b.ParentNo='" + stationMenu.FK_Menu + "'"
                                + " AND a.FK_Station='" + FK_Val + "'";
                            DataTable dt_StationMenu = DBAccess.RunSQLReturnTable(sql);
                            //判断是否含有子项
                            if (dt_StationMenu == null || dt_StationMenu.Rows.Count == 0)
                            {
                                //执行删除
                                DBAccess.RunSQL("DELETE FROM GPM_StationMenu WHERE FK_Station='" + FK_Val + "' and FK_Menu='" + stationMenu.FK_Menu + "'");
                            }
                        }
                    }
                }
            }
            else if (saveType == "GroupMenu")//权限组菜单
            {
                GroupMenus groupMenus = new GroupMenus();
                groupMenus.Retrieve("FK_Group", FK_Val, "IsChecked", "0");
                if (groupMenus != null && groupMenus.Count > 0)
                {
                    //循环删除子级项，方法有待优化；先删除3级关联
                    for (int i = 0, k = 3; i < k; i++)
                    {
                        foreach (GroupMenu groupMenu in groupMenus)
                        {
                            sql = "SELECT a.FK_Group,a.FK_Menu,a.IsChecked FROM GPM_GroupMenu a,GPM_Menu b "
                                + " WHERE a.FK_Menu = b.No"
                                + " AND b.ParentNo='" + groupMenu.FK_Menu + "'"
                                + " AND a.FK_Group='" + FK_Val + "'";
                            DataTable dt_StationMenu = DBAccess.RunSQLReturnTable(sql);
                            //判断是否含有子项
                            if (dt_StationMenu == null || dt_StationMenu.Rows.Count == 0)
                            {
                                //执行删除
                                DBAccess.RunSQL("DELETE FROM GPM_GroupMenu WHERE FK_Group='" + FK_Val + "' and FK_Menu='" + groupMenu.FK_Menu + "'");
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 保存权限组菜单子节点
        /// </summary>
        private void SaveGroupOfMenusChild(string fk_GroupNo, string parentNo, string menuIds)
        {
            //根据父节点编号获取子节点
            BP.GPM.Menus menus = new BP.GPM.Menus();
            menus.RetrieveByAttr("ParentNo", parentNo);

            foreach (BP.GPM.Menu item in menus)
            {
                if (menuIds.Contains(item.No))
                    continue;

                GroupMenu groupMenu = new GroupMenu();
                groupMenu.FK_Group = fk_GroupNo;
                groupMenu.FK_Menu = item.No;
                groupMenu.IsChecked = "1";
                groupMenu.Insert();

                SaveGroupOfMenusChild(fk_GroupNo, item.No, menuIds);
            }
        }

        /// <summary>
        /// 获取权限组菜单
        /// </summary>
        /// <returns></returns>
        private string GetEmpGroupOfMenusByNo()
        {
            string checkedMenuIds = "";
            string groupNO = getUTF8ToString("groupNo");
            string parentNo = getUTF8ToString("parentNo");
            string isLoadChild = getUTF8ToString("isLoadChild");
            //根据权限组编号获取菜单
            GroupMenus groupMenus = new GroupMenus();

            QueryObject objWhere = new QueryObject(groupMenus);
            objWhere.AddWhere(GroupMenuAttr.FK_Group, groupNO);
            objWhere.addAnd();
            objWhere.AddWhere(GroupMenuAttr.IsChecked, true);

            objWhere.DoQuery();
            //获取节点
            BP.GPM.Menus menus = new BP.GPM.Menus();
            menus.RetrieveByAttr("ParentNo", parentNo);

            //整理选中项
            foreach (GroupMenu item in groupMenus)
            {
                checkedMenuIds += "," + item.FK_Menu + ",";
            }
            //整理未完全选中
            string unCheckedMenuIds = "";
            GroupMenus unCGroupMenus = new GroupMenus();
            QueryObject unObjWhere = new QueryObject(unCGroupMenus);
            unObjWhere.AddWhere(GroupMenuAttr.FK_Group, groupNO);
            unObjWhere.addAnd();
            unObjWhere.AddWhere(GroupMenuAttr.IsChecked, false);
            unObjWhere.DoQuery();
            foreach (GroupMenu unItem in unCGroupMenus)
            {
                unCheckedMenuIds += "," + unItem.FK_Menu + ",";
            }

            //如果是第一次加载
            if (isLoadChild == "false")
            {
                StringBuilder appSend = new StringBuilder();
                appSend.Append("[");
                foreach (EntityTree item in menus)
                {
                    if (appSend.Length > 1) appSend.Append(",{"); else appSend.Append("{");

                    appSend.Append("\"id\":\"" + item.No + "\"");
                    appSend.Append(",\"text\":\"" + item.Name + "\"");

                    BP.GPM.Menu menu = item as BP.GPM.Menu;

                    //节点图标
                    string ico = "icon-" + menu.MenuType;
                    //判断未完全选中
                    if (unCheckedMenuIds.Contains("," + item.No + ","))
                        ico = "collaboration";

                    appSend.Append(",iconCls:\"");
                    appSend.Append(ico);
                    appSend.Append("\"");

                    //判断选中
                    if (checkedMenuIds.Contains("," + item.No + ","))
                        appSend.Append(",\"checked\":true");

                    // 增加它的子级.
                    appSend.Append(",\"children\":");
                    appSend.Append(GetMenusByParentNo(item.No, checkedMenuIds, unCheckedMenuIds, true));
                    appSend.Append("}");
                }
                appSend.Append("]");

                return appSend.ToString();
            }
            //返回获取的子节点
            return GetTreeList(menus, checkedMenuIds, unCheckedMenuIds);
        }

        /// <summary>
        /// 保存权限组菜单
        /// </summary>
        /// <returns></returns>
        private string SaveUserGroupOfMenus()
        {
            try
            {
                string groupNo = getUTF8ToString("groupNo");
                string menuIds = getUTF8ToString("menuIds");
                string menuIdsUn = getUTF8ToString("menuIdsUn");
                string menuIdsUnExt = getUTF8ToString("menuIdsUnExt");

                //将未展开项包含的子项补充到已选择和未选择项中
                if (!string.IsNullOrEmpty(menuIdsUnExt))
                {
                    string[] menuParentNos = menuIdsUnExt.Split(',');
                    foreach (string item in menuParentNos)
                    {
                        SetUnCheckedGroupOfMenus(groupNo, item, ref menuIds, ref menuIdsUn);
                    }
                }

                //删除权限组下的菜单
                GroupMenus groupMenus = new GroupMenus();
                groupMenus.Delete(GroupMenuAttr.FK_Group, groupNo);

                //保存选中菜单
                if (!string.IsNullOrEmpty(menuIds))
                {
                    string[] menus = menuIds.Split(',');
                    foreach (string item in menus)
                    {
                        GroupMenu groupMenu = new GroupMenu();
                        groupMenu.FK_Group = groupNo;
                        groupMenu.FK_Menu = item;
                        groupMenu.IsChecked = "1";
                        groupMenu.Insert();

                        SaveGroupOfMenusChild(groupNo, item, menuIds);
                    }
                }
                //保存未完全选中项
                if (!string.IsNullOrEmpty(menuIdsUn))
                {
                    string[] menus = menuIdsUn.Split(',');
                    foreach (string item in menus)
                    {
                        GroupMenu groupMenu = new GroupMenu();
                        groupMenu.FK_Group = groupNo;
                        groupMenu.FK_Menu = item;
                        groupMenu.IsChecked = "0";
                        groupMenu.Insert();
                    }
                }
                //处理未完全选择项，不包含子项的未完全选择项删除
                Del_UnCheckedNoChildNodes("GroupMenu", groupNo);
            }
            catch (Exception ex)
            {
                return "error" + ex.Message;
            }
            return "success";
        }

        /// <summary>
        /// 清空式复制用户权限
        /// </summary>
        /// <returns></returns>
        private string ClearOfCopyUserPower()
        {
            try
            {
                string copyUser = getUTF8ToString("copyUser");
                string pastUsers = getUTF8ToString("pastUsers");
                string[] pastArry = pastUsers.Split(',');


                //获取复制用户权限
                UserMenu userMenu = new UserMenu();
                DataTable userMenu_dt = userMenu.RunSQLReturnTable("SELECT FK_Menu,IsChecked FROM V_GPM_EmpMenu_GPM WHERE FK_Emp='" + copyUser + "'");

                //循环目标对象
                foreach (string pastUser in pastArry)
                {
                    //清空之前的权限
                    userMenu = new UserMenu();
                    userMenu.FK_Emp = pastUser;
                    userMenu.Delete();

                    //授权
                    foreach (DataRow row in userMenu_dt.Rows)
                    {
                        userMenu = new UserMenu();
                        userMenu.FK_Emp = pastUser;
                        userMenu.FK_Menu = row["FK_Menu"].ToString();
                        userMenu.IsChecked = row["IsChecked"].ToString();
                        userMenu.Insert();
                    }
                }
            }
            catch (Exception ex)
            {
                return "error:" + ex.Message;
            }
            return "success";
        }

        /// <summary>
        /// 覆盖式复制用户权限
        /// </summary>
        /// <returns></returns>
        private string CoverOfCopyUserPower()
        {
            try
            {
                string copyUser = getUTF8ToString("copyUser");
                string pastUsers = getUTF8ToString("pastUsers");
                string[] pastArry = pastUsers.Split(',');


                //获取复制用户权限
                UserMenu userMenu = new UserMenu();
                DataTable userMenu_dt = userMenu.RunSQLReturnTable("SELECT FK_Menu,IsChecked FROM V_GPM_EmpMenu_GPM WHERE FK_Emp='" + copyUser + "'");

                //循环目标对象
                foreach (string pastUser in pastArry)
                {
                    //授权
                    foreach (DataRow row in userMenu_dt.Rows)
                    {
                        //判断权限是否存在
                        userMenu = new UserMenu();
                        DataTable menu_dt = userMenu.RunSQLReturnTable("SELECT FK_Menu,IsChecked FROM V_GPM_EmpMenu_GPM WHERE FK_Emp='" + pastUser + "' AND FK_Menu = '" + row["FK_Menu"].ToString() + "'");
                        if (menu_dt != null && menu_dt.Rows.Count == 0)
                        {
                            userMenu = new UserMenu();
                            userMenu.FK_Emp = pastUser;
                            userMenu.FK_Menu = row["FK_Menu"].ToString();
                            userMenu.IsChecked = row["IsChecked"].ToString();
                            userMenu.Insert();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "error:" + ex.Message;
            }
            return "success";
        }

        /// <summary>
        /// 清空式复制权限
        /// </summary>
        /// <returns></returns>
        private string ClearOfCopyUserGroupPower()
        {
            try
            {
                string copyGroupNo = getUTF8ToString("copyGroupNo");
                string pastGroupNos = getUTF8ToString("pastGroupNos");
                string[] pastArry = pastGroupNos.Split(',');

                //获取复制权限组权限
                GroupMenus copyGroupMenus = new GroupMenus();
                copyGroupMenus.RetrieveByAttr(GroupMenuAttr.FK_Group, copyGroupNo);

                //循环目标对象
                foreach (string pastGroup in pastArry)
                {
                    //清空之前的权限
                    GroupMenu groupMenu = new GroupMenu();
                    groupMenu.Delete(GroupMenuAttr.FK_Group, pastGroup);

                    //授权
                    foreach (GroupMenu copyMenu in copyGroupMenus)
                    {
                        groupMenu = new GroupMenu();
                        groupMenu.FK_Group = pastGroup;
                        groupMenu.FK_Menu = copyMenu.FK_Menu;
                        groupMenu.IsChecked = copyMenu.IsChecked;
                        groupMenu.Insert();
                    }
                }
            }
            catch (Exception ex)
            {
                return "error:" + ex.Message;
            }
            return "success";
        }

        /// <summary>
        /// 覆盖式复制权限
        /// </summary>
        /// <returns></returns>
        private string CoverOfCopyUserGroupPower()
        {
            try
            {
                string copyGroupNo = getUTF8ToString("copyGroupNo");
                string pastGroupNos = getUTF8ToString("pastGroupNos");
                string[] pastArry = pastGroupNos.Split(',');

                //获取复制权限组权限
                GroupMenus copyGroupMenus = new GroupMenus();
                copyGroupMenus.RetrieveByAttr(GroupMenuAttr.FK_Group, copyGroupNo);

                //循环目标对象
                foreach (string pastGroup in pastArry)
                {
                    //授权
                    foreach (GroupMenu copyMenu in copyGroupMenus)
                    {
                        GroupMenu groupMenu = new GroupMenu();
                        bool isHave = groupMenu.RetrieveByAttrAnd(GroupMenuAttr.FK_Group, pastGroup, GroupMenuAttr.FK_Menu, copyMenu.FK_Menu);
                        //判断之前的权限是否存在
                        if (!isHave)
                        {
                            groupMenu = new GroupMenu();
                            groupMenu.FK_Group = pastGroup;
                            groupMenu.FK_Menu = copyMenu.FK_Menu;
                            groupMenu.IsChecked = copyMenu.IsChecked;
                            groupMenu.Insert();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "error:" + ex.Message;
            }
            return "success";
        }

        /// <summary>
        /// 获取系统
        /// </summary>
        /// <returns></returns>
        private string GetApps()
        {
            Apps apps = new Apps();

            apps.RetrieveAll();
            return TranslateEntitiesToGridJsonOnlyData(apps);
        }

        /// <summary>
        /// 获取系统日志
        /// </summary>
        /// <returns></returns>
        private string GetSystemLoginLogs()
        {
            string startdate = DateTime.Parse(getUTF8ToString("startdate")).ToString("yyyy-MM-dd HH:mm:ss");
            string enddate = DateTime.Parse(getUTF8ToString("enddate")).ToString("yyyy-MM-dd HH:mm:ss");

            string sql = "select OID,FK_EMP,Port_Emp.Name as EMP_Name,Port_Dept.Name as Dept_Name,GPM_App.Name as Sys_Name, LoginDateTime,IP "
                + "from Port_Emp inner join Port_Dept on Port_Emp.FK_Dept=Port_Dept.No "
                + "inner join GPM_SystemLoginLog on GPM_SystemLoginLog.FK_Emp=Port_Emp.No "
                + "and  CONVERT(DATETIME,GPM_SystemLoginLog.LoginDateTime)  between CONVERT(DATETIME,'" + startdate + "')  and  CONVERT(DATETIME,'" + enddate + "')  "
                + "inner join GPM_App on GPM_App.No=GPM_SystemLoginLog.FK_App order by CONVERT(DATETIME,LoginDateTime) desc";

            SystemLoginLog loginLog = new SystemLoginLog();
            DataTable dt = loginLog.RunSQLReturnTable(sql);
            return CommonDbOperator.GetJsonFromTable(dt);
        }

        /// <summary>
        /// 将实体类转为json格式
        /// </summary>
        /// <param name="ens"></param>
        /// <returns></returns>
        public string TranslateEntitiesToGridJsonOnlyData(BP.En.Entities ens)
        {
            Attrs attrs = ens.GetNewEntity.EnMap.Attrs;
            StringBuilder append = new StringBuilder();
            append.Append("[");

            foreach (Entity en in ens)
            {
                append.Append("{");
                foreach (Attr attr in attrs)
                {
                    //if (attr.IsRefAttr || attr.UIVisible == false)
                    //    continue;
                    string strValue = en.GetValStrByKey(attr.Key);
                    if (!string.IsNullOrEmpty(strValue) && strValue.LastIndexOf("\\") > -1)
                    {
                        strValue = strValue.Substring(0, strValue.LastIndexOf("\\"));
                    }
                    append.Append(attr.Key + ":\"" + strValue + "\",");
                }
                append = append.Remove(append.Length - 1, 1);
                append.Append("},");
            }
            if (append.Length > 1)
                append = append.Remove(append.Length - 1, 1);
            append.Append("]");
            return append.ToString();
        }
        /// <summary>
        /// 将实体类转为json格式 包含列名和数据
        /// </summary>
        /// <param name="ens"></param>
        /// <returns></returns>
        public string TranslateEntitiesToGridJsonColAndData(BP.En.Entities ens)
        {
            Attrs attrs = ens.GetNewEntity.EnMap.Attrs;
            StringBuilder append = new StringBuilder();
            append.Append("{");
            //整理列名
            append.Append("columns:[");
            foreach (Attr attr in attrs)
            {
                if (attr.IsRefAttr || attr.UIVisible == false)
                    continue;
                append.Append("{");
                append.Append(string.Format("field:'{0}',title:'{1}',width:{2}", attr.Key, attr.Desc, attr.UIWidth));
                append.Append("},");
            }
            if (append.Length > 10)
                append = append.Remove(append.Length - 1, 1);
            append.Append("]");

            //整理数据
            append.Append(",data:[");
            foreach (Entity en in ens)
            {
                append.Append("{");
                foreach (Attr attr in attrs)
                {
                    if (attr.IsRefAttr || attr.UIVisible == false)
                        continue;
                    append.Append(attr.Key + ":\"" + en.GetValStrByKey(attr.Key) + "\",");
                }
                append = append.Remove(append.Length - 1, 1);
                append.Append("},");
            }
            if (append.Length > 11)
                append = append.Remove(append.Length - 1, 1);
            append.Append("]");
            append.Append("}");
            return append.ToString();
        }

        /// <summary>
        /// 获取所有目录菜单
        /// </summary>
        /// <returns></returns>
        private string GetMenusOfMenuForEmp()
        {
            string parentNo = getUTF8ToString("parentNo");
            string isLoadChild = getUTF8ToString("isLoadChild");

            //根据父节点编号获取子节点
            BP.GPM.Menus menus = new BP.GPM.Menus();
            menus.RetrieveByAttr("ParentNo", parentNo);

            //如果是第一次加载
            if (isLoadChild == "false")
            {
                StringBuilder appSend = new StringBuilder();
                appSend.Append("[");
                foreach (EntityTree item in menus)
                {
                    if (appSend.Length > 1) appSend.Append(",{"); else appSend.Append("{");

                    appSend.Append("\"id\":\"" + item.No + "\"");
                    appSend.Append(",\"text\":\"" + item.Name + "\"");

                    BP.GPM.Menu menu = item as BP.GPM.Menu;

                    //节点图标
                    string ico = "icon-" + menu.MenuType;

                    appSend.Append(",iconCls:\"");
                    appSend.Append(ico);
                    appSend.Append("\"");
                    appSend.Append(",\"children\":");
                    appSend.Append(GetMenusByParentNo(item.No, "", "", true));
                    appSend.Append("}");
                }
                appSend.Append("]");

                return appSend.ToString();
            }
            //获取树节点数据
            return GetTreeList(menus, "", "");
        }

        //加载左侧菜单
        private string GetLeftMenu()
        {
            StringBuilder menuApp = new StringBuilder();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(SystemConfig.PathOfXML + "Menu.xml");
            //得到顶层节点列表
            XmlNodeList topM = xmlDoc.DocumentElement.ChildNodes;
            menuApp.Append("[");
            foreach (XmlNode node in topM)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    if (menuApp.Length > 1) menuApp.Append(",");
                    menuApp.Append("{");
                    menuApp.Append("No:'" + node.Attributes["No"].InnerText + "'");
                    menuApp.Append(",Name:'" + node.Attributes["Name"].InnerText + "'");
                    menuApp.Append(",Img:'" + node.Attributes["Img"].InnerText + "'");
                    menuApp.Append(",Url:'" + node.Attributes["Url"].InnerText + "'");
                    menuApp.Append(",Children:[");
                    if (node.ChildNodes.Count > 0)
                    {
                        string childrenMenu = "";
                        foreach (XmlNode cNode in node.ChildNodes)
                        {
                            if (cNode.NodeType == XmlNodeType.Element)
                            {
                                if (childrenMenu.Length > 0) childrenMenu += ",";
                                childrenMenu += "{";
                                childrenMenu += "No:'" + cNode.Attributes["No"].InnerText + "'";
                                childrenMenu += ",Name:'" + cNode.Attributes["Name"].InnerText + "'";
                                childrenMenu += ",Img:'" + cNode.Attributes["Img"].InnerText + "'";
                                childrenMenu += ",Url:'" + cNode.Attributes["Url"].InnerText + "'";
                                childrenMenu += ",Children:[]";
                                childrenMenu += "}";
                            }
                        }
                        menuApp.Append(childrenMenu);
                    }
                    menuApp.Append("]}");
                }
            }
            menuApp.Append("]");
            return menuApp.ToString();
        }

        /// <summary>
        /// 得到菜单
        /// </summary>
        public string getMenu()
        {
            StringBuilder sbMenu = new StringBuilder();
            string appName = getUTF8ToString("appname");
            string no = getUTF8ToString("no");

            sbMenu.Append("[");
            string sql = "select * from GPM_Menu where  fk_app='" + appName + "' and  No='" + no + "'";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    sbMenu.Append("{");
                    foreach (DataColumn dc in dt.Rows[0].Table.Columns)
                    {
                        if (dc.Ordinal > 0) sbMenu.Append(",");
                        sbMenu.AppendFormat(dc.ColumnName + ":\"{0}\" ", dt.Rows[0][dc.ColumnName].ToString());
                    }
                    sbMenu.AppendFormat(",iconCls:\"{0}\" ", "icon-" + dt.Rows[0]["MenuType"].ToString());
                    sbMenu.Append(",children:[");
                    sbMenu.Append(GetChildMenu(appName, no));
                    sbMenu.Append("]}");
                }
            }
            sbMenu.Append("]");

            return sbMenu.ToString();
        }
        /// <summary>
        /// 迭代获取菜单
        /// </summary>
        /// <returns></returns>
        public string GetChildMenu(string appName, string nodeno)
        {
            StringBuilder sbContent = new StringBuilder("");
            string sql = "select * from GPM_Menu  where  fk_app='" + appName + "' and  ParentNo='" + nodeno + "' ORDER BY Idx";

            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            if (dt != null)
            {
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    i++;
                    sbContent.Append("{");
                    foreach (DataColumn dc in dt.Rows[0].Table.Columns)
                    {
                        if (dc.Ordinal > 0) sbContent.Append(",");
                        sbContent.AppendFormat(dc.ColumnName + ":\"{0}\" ", dr[dc.ColumnName].ToString());
                    }
                    sbContent.AppendFormat(",iconCls:\"{0}\", ", "icon-" + dr["MenuType"].ToString());
                    sbContent.Append("children:[");
                    sbContent.Append(GetChildMenu(appName, dr["No"].ToString()));
                    if (i == dt.Rows.Count)
                    {
                        sbContent.Append("]}");
                    }
                    else
                    {
                        sbContent.Append("]},");
                    }

                }
                return sbContent.ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 根据系统名称获取菜单
        /// </summary>
        /// <returns></returns>
        private string GetSystemMenus()
        {
            string strMenus = "[]";
            string appName = getUTF8ToString("appName");

            BP.GPM.App app = new App(appName);
            BP.GPM.Menu sysMenu = new BP.GPM.Menu();
            DataTable dt = sysMenu.RunSQLReturnTable("select * from GPM_Menu WHERE FK_App='" + appName + "' ORDER BY Idx");
            strMenus = CommonDbOperator.GetGridTreeDataString(dt, "ParentNo", "No", app.RefMenuNo, true);

            if (strMenus.Length > 2)
                strMenus = strMenus.Remove(strMenus.Length - 2, 2);
            else
            {
                sysMenu = new BP.GPM.Menu();
                sysMenu.No = BP.DA.DBAccess.GenerOID("BP.GPM.Menu").ToString();
                sysMenu.Name = "新建目录";
                sysMenu.ParentNo = app.RefMenuNo;
                sysMenu.FK_App = appName;
                sysMenu.MenuType = 3;
                sysMenu.IsDir = true;
                sysMenu.Insert();
                //重新查询
                dt = sysMenu.RunSQLReturnTable("select * from GPM_Menu WHERE FK_App='" + appName + "' ORDER BY Idx");
                strMenus = CommonDbOperator.GetGridTreeDataString(dt, "ParentNo", "No", app.RefMenuNo, true);

                if (strMenus.Length > 2)
                    strMenus = strMenus.Remove(strMenus.Length - 2, 2);
            }
            return strMenus;
        }
        /// <summary>
        /// 根据编号获取子菜单
        /// </summary>
        /// <returns></returns>
        private string GetMenusById()
        {
            string strMenus = "[]";
            string id = getUTF8ToString("Id");
            BP.GPM.Menu sysMenu = new BP.GPM.Menu();
            DataTable dt = sysMenu.RunSQLReturnTable("select * from GPM_Menu WHERE ParentNo='" + id + "' ORDER BY Idx");
            strMenus = CommonDbOperator.GetGridTreeDataString(dt, "ParentNo", "No", id, true);

            if (strMenus.Length > 2)
                strMenus = strMenus.Remove(strMenus.Length - 2, 2);
            return strMenus;
        }
        /// <summary>
        /// 获取部门和用户
        /// </summary>
        /// <returns></returns>
        private string GetDeptEmpTree()
        {
            StringBuilder deptEmp = new StringBuilder();
            //部门信息
            Dept dept = new Dept();
            dept.Retrieve("ParentNo", "0");
            deptEmp.Append("[");
            if (dept != null)
            {
                deptEmp.Append("{");
                deptEmp.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\",\"iconCls\":\"icon-tree_folder\",\"state\":\"open\"", dept.No, dept.Name));
                DataTable dt_dept = dept.RunSQLReturnTable("select * from Port_Dept where ParentNo='" + dept.No + "' order by Idx");
                DataTable dt_emp = dept.RunSQLReturnTable("select distinct b.No,b.Name from Port_DeptEmp a,Port_Emp b where a.FK_Dept = b.FK_Dept and a.FK_Dept = '" + dept.No + "'");
                deptEmp.Append(",\"children\":");
                deptEmp.Append("[");
                //绑定部门
                if (dt_dept != null && dt_dept.Rows.Count > 0)
                {
                    foreach (DataRow row in dt_dept.Rows)
                    {
                        deptEmp.Append("{");
                        deptEmp.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\",\"iconCls\":\"icon-tree_folder\",\"state\":\"closed\"", row["No"].ToString(), row["Name"].ToString()));
                        deptEmp.Append(",\"children\":");
                        deptEmp.Append("[{");
                        deptEmp.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\"", row["No"].ToString() + "01", "加载中..."));
                        deptEmp.Append("}]");
                        deptEmp.Append("},");
                    }
                    deptEmp = deptEmp.Remove(deptEmp.Length - 1, 1);
                }
                //绑定人员
                if (dt_emp != null && dt_emp.Rows.Count > 0)
                {
                    foreach (DataRow empRow in dt_emp.Rows)
                    {
                        deptEmp.Append(",{");
                        deptEmp.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\",\"iconCls\":\"icon-person\"", empRow["No"].ToString(), empRow["Name"].ToString()));
                        deptEmp.Append("}");
                    }
                }
                deptEmp.Append("]");
                deptEmp.Append("}");
            }
            deptEmp.Append("]");

            return deptEmp.ToString();
        }

        /// <summary>
        /// 获取部门用户子节点
        /// </summary>
        /// <returns></returns>
        private string GetDeptEmpChildNodes()
        {
            string nodeNo = getUTF8ToString("nodeNo");

            StringBuilder deptEmp = new StringBuilder();
            Dept dept = new Dept();
            DataTable dt_dept = dept.RunSQLReturnTable("select * from Port_Dept where ParentNo='" + nodeNo + "' order by Idx");
            DataTable dt_emp = dept.RunSQLReturnTable("select distinct b.No,b.Name from Port_DeptEmp a,Port_Emp b where a.FK_Dept = b.FK_Dept and a.FK_Dept = '" + nodeNo + "'");

            deptEmp.Append("[");
            //绑定部门
            if (dt_dept != null && dt_dept.Rows.Count > 0)
            {
                foreach (DataRow row in dt_dept.Rows)
                {
                    deptEmp.Append("{");
                    deptEmp.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\",\"iconCls\":\"icon-tree_folder\",\"state\":\"closed\"", row["No"].ToString(), row["Name"].ToString()));
                    deptEmp.Append(",\"children\":");
                    deptEmp.Append("[{");
                    deptEmp.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\"", row["No"].ToString() + "01", "加载中..."));
                    deptEmp.Append("}]");
                    deptEmp.Append("},");
                }
                deptEmp = deptEmp.Remove(deptEmp.Length - 1, 1);
            }
            //绑定人员
            if (dt_emp != null && dt_emp.Rows.Count > 0)
            {
                foreach (DataRow empRow in dt_emp.Rows)
                {
                    if (deptEmp.Length == 1)
                        deptEmp.Append("{");
                    else
                        deptEmp.Append(",{");
                    deptEmp.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\",\"iconCls\":\"icon-person\"", empRow["No"].ToString(), empRow["Name"].ToString()));
                    deptEmp.Append("}");
                }
            }
            deptEmp.Append("]");

            return deptEmp.ToString();
        }

        /// <summary>
        /// 菜单管理
        /// </summary>
        /// <returns></returns>
        private string MenuNodeManage()
        {
            string nodeNo = getUTF8ToString("nodeNo");
            string dowhat = getUTF8ToString("dowhat");
            string returnVal = "";
            BP.GPM.Menu sysMenu = new BP.GPM.Menu(nodeNo);
            switch (dowhat.ToLower())
            {
                case "sample"://新增同级节点                    
                    nodeNo = sysMenu.DoCreateSameLevelNode().No;
                    //新节点赋值
                    BP.GPM.Menu newMenu = new BP.GPM.Menu(nodeNo); ;
                    newMenu.FK_App = sysMenu.FK_App;
                    newMenu.Update();
                    returnVal = newMenu.No;
                    break;
                case "children"://新增子节点
                    nodeNo = sysMenu.DoCreateSubNode().No;
                    //新节点赋值
                    BP.GPM.Menu newcMenu = new BP.GPM.Menu(nodeNo);
                    newcMenu.FK_App = sysMenu.FK_App;
                    newcMenu.Update();
                    returnVal = newcMenu.No;
                    break;
                case "doup"://上移
                    sysMenu.DoUp();
                    break;
                case "dodown"://下移
                    sysMenu.DoDown();
                    break;
                case "delete"://删除
                    sysMenu.Delete();
                    break;
            }
            //返回
            return returnVal;
        }

        /// <summary>
        /// 根据用户编号获取菜单
        /// </summary>
        /// <returns></returns>
        private string GetMenuByEmpNo()
        {
            string fk_emp = getUTF8ToString("fk_emp");
            string fk_app = getUTF8ToString("fk_app");
            // 获取菜单，并把它展现出来.
            DataTable dt = BP.GPM.Dev2Interface.DB_Menus(fk_emp, fk_app);
            return CommonDbOperator.GetJsonFromTable(dt);
        }

        /// <summary>
        /// 获取所有菜单，根据用户权限设置所选项
        /// </summary>
        /// <returns></returns>
        private string GetEmpOfMenusByEmpNo()
        {
            string checkedMenuIds = "";
            string empNO = getUTF8ToString("empNo");
            string parentNo = getUTF8ToString("parentNo");
            string isLoadChild = getUTF8ToString("isLoadChild");
            //根据用户编号获取菜单
            //UserMenus userMenus = new UserMenus();

            //QueryObject objWhere = new QueryObject(userMenus);
            //objWhere.AddWhere(UserMenuAttr.FK_Emp, empNO);
            //objWhere.addAnd();
            //objWhere.AddWhere(UserMenuAttr.IsChecked, true);
            //objWhere.DoQuery();
            UserMenu userMenu = new UserMenu();
            DataTable userMenu_dt = userMenu.RunSQLReturnTable("SELECT FK_Menu FROM V_GPM_EmpMenu_GPM WHERE FK_Emp='" + empNO + "' AND IsChecked=1");

            //根据父节点编号获取子节点
            BP.GPM.Menus menus = new BP.GPM.Menus();
            menus.RetrieveByAttr("ParentNo", parentNo);

            //整理选中项
            foreach (DataRow row in userMenu_dt.Rows)
            {
                checkedMenuIds += "," + row["FK_Menu"].ToString() + ",";
            }
            //整理未完全选中
            string unCheckedMenuIds = "";
            DataTable unCheck_dt = userMenu.RunSQLReturnTable("SELECT FK_Menu FROM V_GPM_EmpMenu_GPM WHERE FK_Emp='" + empNO + "' AND IsChecked=0");
            foreach (DataRow unItem in unCheck_dt.Rows)
            {
                unCheckedMenuIds += "," + unItem["FK_Menu"] + ",";
            }
            //如果是第一次加载
            if (isLoadChild == "false")
            {
                StringBuilder appSend = new StringBuilder();
                appSend.Append("[");
                foreach (EntityTree item in menus)
                {
                    if (appSend.Length > 1) appSend.Append(",{"); else appSend.Append("{");

                    appSend.Append("\"id\":\"" + item.No + "\"");
                    appSend.Append(",\"text\":\"" + item.Name + "\"");

                    BP.GPM.Menu menu = item as BP.GPM.Menu;

                    //节点图标
                    string ico = "icon-" + menu.MenuType;
                    //判断未完全选中
                    if (unCheckedMenuIds.Contains("," + item.No + ","))
                        ico = "collaboration";

                    appSend.Append(",iconCls:\"");
                    appSend.Append(ico);
                    appSend.Append("\"");

                    //判断选中
                    if (checkedMenuIds.Contains("," + item.No + ","))
                        appSend.Append(",\"checked\":true");

                    appSend.Append(",\"children\":");
                    appSend.Append(GetMenusByParentNo(item.No, checkedMenuIds, unCheckedMenuIds, true));
                    appSend.Append("}");
                }
                appSend.Append("]");

                return appSend.ToString();
            }
            //获取树节点数据
            return GetTreeList(menus, checkedMenuIds, unCheckedMenuIds);
        }

        /// <summary>
        /// 根据父节点编号获取子菜单
        /// </summary>
        /// <returns></returns>
        private string GetMenusByParentNo(string parentNo, string checkedMenuIds, string unCheckedMenuIds, bool addChild)
        {
            StringBuilder menuAppend = new StringBuilder();
            //获取菜单
            BP.GPM.Menus menus = new BP.GPM.Menus();
            menus.RetrieveByAttr("ParentNo", parentNo);

            //是否添加下一级
            if (addChild)
            {
                menuAppend.Append("[");
                foreach (EntityTree item in menus)
                {
                    if (menuAppend.Length > 1) menuAppend.Append(",{"); else menuAppend.Append("{");

                    menuAppend.Append("\"id\":\"" + item.No + "\"");
                    menuAppend.Append(",\"text\":\"" + item.Name + "\"");

                    BP.GPM.Menu menu = item as BP.GPM.Menu;

                    //节点图标
                    string ico = "icon-" + menu.MenuType;
                    //判断未完全选中
                    if (unCheckedMenuIds.Contains("," + item.No + ","))
                        ico = "collaboration";

                    menuAppend.Append(",iconCls:\"");
                    menuAppend.Append(ico);
                    menuAppend.Append("\"");

                    //判断选中
                    if (checkedMenuIds.Contains("," + item.No + ","))
                        menuAppend.Append(",\"checked\":true");

                    menuAppend.Append(",\"children\":");
                    menuAppend.Append(GetMenusByParentNo(item.No, checkedMenuIds, unCheckedMenuIds, false));
                    menuAppend.Append("}");
                }
                menuAppend.Append("]");

                return menuAppend.ToString();
            }
            return GetTreeList(menus, checkedMenuIds, unCheckedMenuIds);
        }

        /// <summary>
        /// 获取树节点列表
        /// </summary>
        /// <param name="ens"></param>
        /// <param name="checkIds"></param>
        /// <returns></returns>
        public string GetTreeList(Entities ens, string checkIds, string unCheckIds)
        {
            StringBuilder appSend = new StringBuilder();
            appSend.Append("[");
            foreach (EntityTree item in ens)
            {
                if (appSend.Length > 1) appSend.Append(",{"); else appSend.Append("{");

                appSend.Append("\"id\":\"" + item.No + "\"");
                appSend.Append(",\"text\":\"" + item.Name + "\"");

                BP.GPM.Menu menu = item as BP.GPM.Menu;

                //节点图标
                string ico = "icon-" + menu.MenuType;
                //判断未完全选中
                if (unCheckIds.Contains("," + item.No + ","))
                    ico = "collaboration";

                appSend.Append(",iconCls:\"");
                appSend.Append(ico);
                appSend.Append("\"");

                if (checkIds.Contains("," + item.No + ","))
                    appSend.Append(",\"checked\":true");

                //判断是否还有子节点
                BP.GPM.Menus menus = new BP.GPM.Menus();
                menus.RetrieveByAttr("ParentNo", item.No);

                if (menus != null && menus.Count > 0)
                {
                    appSend.Append(",state:\"closed\"");
                    appSend.Append(",\"children\":");
                    appSend.Append("[{");
                    appSend.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\"", item.No + "01", "加载中..."));
                    appSend.Append("}]");
                }
                appSend.Append("}");
            }
            appSend.Append("]");

            return appSend.ToString();
        }

        /// <summary>
        /// 将实体转为树形
        /// </summary>
        /// <param name="ens"></param>
        /// <param name="rootNo"></param>
        /// <param name="checkIds"></param>
        StringBuilder appendMenus = new StringBuilder();
        StringBuilder appendMenuSb = new StringBuilder();
        public void TansEntitiesToGenerTree(Entities ens, string rootNo, string checkIds)
        {
            EntityTree root = ens.GetEntityByKey(rootNo) as EntityTree;
            if (root == null)
                throw new Exception("@没有找到rootNo=" + rootNo + "的entity.");
            appendMenus.Append("[{");
            appendMenus.Append("\"id\":\"" + rootNo + "\"");
            appendMenus.Append(",\"text\":\"" + root.Name + "\"");

            BP.GPM.Menu menu = ens.GetEntityByKey(rootNo) as BP.GPM.Menu;
            //添加图标
            appendMenus.Append(",iconCls:\"");
            appendMenus.Append("icon-" + menu.MenuType);
            appendMenus.Append("\"");

            // 增加它的子级.
            appendMenus.Append(",\"children\":");
            AddChildren(menu, ens, checkIds);
            appendMenus.Append(appendMenuSb);
            appendMenus.Append("}]");
        }

        public void AddChildren(EntityTree parentEn, Entities ens, string checkIds)
        {
            appendMenus.Append(appendMenuSb);
            appendMenuSb.Clear();

            appendMenuSb.Append("[");
            foreach (EntityTree item in ens)
            {
                if (item.ParentNo != parentEn.No)
                    continue;

                if (checkIds.Contains("," + item.No + ","))
                    appendMenuSb.Append("{\"id\":\"" + item.No + "\",\"text\":\"" + item.Name + "\",\"checked\":true");
                else
                    appendMenuSb.Append("{\"id\":\"" + item.No + "\",\"text\":\"" + item.Name + "\",\"checked\":false");

                BP.GPM.Menu menu = item as BP.GPM.Menu;
                if (menu != null)
                {
                    //添加图标
                    appendMenuSb.Append(",iconCls:\"");
                    appendMenuSb.Append("icon-" + menu.MenuType);
                    appendMenuSb.Append("\"");
                }
                // 增加它的子级.
                appendMenuSb.Append(",\"children\":");
                AddChildren(item, ens, checkIds);
                treeResult.Append(treesb.ToString());
                treesb.Clear();
                appendMenuSb.Append("},");
            }
            if (appendMenuSb.Length > 1)
                appendMenuSb = appendMenuSb.Remove(appendMenuSb.Length - 1, 1);
            appendMenuSb.Append("]");
            appendMenus.Append(appendMenuSb);
            appendMenuSb.Clear();
        }



        /// <summary>
        /// 根据DataTable生成Json树结构
        /// </summary>
        /// <param name="tabel">数据源</param>
        /// <param name="idCol">ID列</param>
        /// <param name="txtCol">Text列</param>
        /// <param name="rela">关系字段</param>
        /// <param name="pId">父ID</param>
        ///<returns>easyui tree json格式</returns>
        StringBuilder treeResult = new StringBuilder();
        StringBuilder treesb = new StringBuilder();
        public string GetTreeJsonByTable(DataTable tabel, string idCol, string txtCol, string rela, object pId)
        {
            string treeJson = string.Empty;
            string treeState = "close";
            treeResult.Append(treesb.ToString());

            treesb.Clear();
            if (treeResult.Length == 0)
            {
                treeState = "open";
            }
            if (tabel.Rows.Count > 0)
            {
                treesb.Append("[");
                string filer = string.Empty;
                if (pId.ToString() == "")
                {
                    filer = string.Format("{0} is null", rela);
                }
                else
                {
                    filer = string.Format("{0}='{1}'", rela, pId);
                }
                DataRow[] rows = tabel.Select(filer);
                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        treesb.Append("{\"id\":\"" + row[idCol]
                                + "\",\"text\":\"" + row[txtCol]
                                + "\",\"state\":\"" + treeState + "\"");

                        //更换节点图标
                        if (tabel.Columns.Contains("MenuType"))
                        {
                            //目录级别不展开
                            if (row["MenuType"].ToString() == "3")
                            {
                                treesb.Append(",state:\"closed\"");
                            }
                            treesb.Append(",iconCls:\"");
                            treesb.Append("icon-" + row["MenuType"].ToString());
                            treesb.Append("\"");
                        }

                        if (tabel.Select(string.Format("{0}='{1}'", rela, row[idCol])).Length > 0)
                        {
                            treesb.Append(",\"children\":");
                            GetTreeJsonByTable(tabel, idCol, txtCol, rela, row[idCol]);
                            treeResult.Append(treesb.ToString());
                            treesb.Clear();
                        }
                        treeResult.Append(treesb.ToString());
                        treesb.Clear();
                        treesb.Append("},");
                    }
                    treesb = treesb.Remove(treesb.Length - 1, 1);
                }
                treesb.Append("]");
                treeResult.Append(treesb.ToString());
                treeJson = treeResult.ToString();
                treesb.Clear();
            }
            return treeJson;
        }
    }
}