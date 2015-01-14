using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Xml;
using BP.Sys;
using BP.En;
using BP.DA;
using BP.Web;
using BP.GPM;
using BP.GPM.Utility;

namespace GMP2.GPM
{
    public partial class DataService : WebPage
    {
        public string getUTF8ToString(string param)
        {
            return HttpUtility.UrlDecode(Request[param], System.Text.Encoding.UTF8);
        }
        // Page load 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (BP.Web.WebUser.No == null)
                return;


            string method = string.Empty;
            // The return value 
            string s_responsetext = string.Empty;
            if (!string.IsNullOrEmpty(Request["method"]))
                method = Request["method"].ToString();

            switch (method)
            {
                case "getemps":// Get all personnel information 
                    s_responsetext = GetEmps();
                    break;
                case "getempsbynoorname":// The user name or user ID fuzzy search 
                    s_responsetext = GetEmpsByNoOrName();
                    break;
                case "getempgroups":// Find all rights group 
                    s_responsetext = GetEmpGroups();
                    break;
                case "getempgroupsbyname":// Rights Groups fuzzy search 
                    s_responsetext = GetEmpGroupsByName();
                    break;
                case "getapps":// Get all system 
                    s_responsetext = GetApps();
                    break;
                case "getmenusofmenuforemp":// Get all directories menu 
                    s_responsetext = GetMenusOfMenuForEmp();
                    break;
                case "getmenusbyid":// According to numbers get menu 
                    s_responsetext = GetMenusById();
                    break;
                case "getleftmenu":// The left menu 
                    s_responsetext = GetLeftMenu();
                    break;
                case "getsystemmenu"://  Get System Menu 
                    s_responsetext = GetSystemMenus();
                    break;
                case "menunodemanage":// Menu Management 
                    s_responsetext = MenuNodeManage();
                    break;
                case "getmenubyempno":// Find a menu based on user ID 
                    s_responsetext = GetMenuByEmpNo();
                    break;
                case "getdeptemptree":// Get information department staff 
                    s_responsetext = GetDeptEmpTree();
                    break;
                case "getdeptempchildnodes":// Gets the child node number according to department staff 
                    s_responsetext = GetDeptEmpChildNodes();
                    break;
                case "getempofmenusbyempno":// User menu Permissions 
                    s_responsetext = GetEmpOfMenusByEmpNo();
                    break;
                case "saveuserofmenus":// Save the correspondence between the user and the menu 
                    s_responsetext = SaveUserOfMenus();
                    break;
                case "getempgroupofmenusbyno":// Get permission group menu 
                    s_responsetext = GetEmpGroupOfMenusByNo();
                    break;
                case "saveusergroupofmenus":// Save Permissions Group Menu 
                    s_responsetext = SaveUserGroupOfMenus();
                    break;
                case "clearofcopyuserpower":// Empty replication user rights 
                    s_responsetext = ClearOfCopyUserPower();
                    break;
                case "coverofcopyuserpower":// Overlay copy user permissions 
                    s_responsetext = CoverOfCopyUserPower();
                    break;
                case "clearofcopyusergrouppower":// Empty replication Permissions Group Permissions 
                    s_responsetext = ClearOfCopyUserGroupPower();
                    break;
                case "coverofcopyusergrouppower":// Overlay covering the right group 
                    s_responsetext = CoverOfCopyUserGroupPower();
                    break;
                case "getAppChildMenus":// Open a new window   Get menu 
                    s_responsetext = getMenu();
                    break;
                case "GetAllDept":// Get all departments 
                    s_responsetext = GetAllDept();
                    break;
                case "gettemplatedata":// Press Menu to assign permissions , Get the template data 
                    s_responsetext = getTemplateData();
                    break;
                case "savemenuforemp":// Press Menu to assign permissions to save 
                    s_responsetext = SaveMenuForEmp();
                    break;
                case "getsystemloginlogs":// Get System Log Log 
                    s_responsetext = GetSystemLoginLogs();
                    break;
                case "getstations":// Get all posts 
                    s_responsetext = GetStations();
                    break;
                case "savestationofmenus"://  Save jobs menu 
                    s_responsetext = SaveStationOfMenus();
                    break;
                case "getstationofmenusbyno":// Get job menu 
                    s_responsetext = GetStationOfMenusByNo();
                    break;
                case "clearofcopystation":// Empty style   Copy jobs 
                    s_responsetext = ClearOfCopyStation();
                    break;
                case "coverofcopystation":// Overlay   Copy jobs 
                    s_responsetext = CoverOfCopyStation();
                    break;
                case "getstationbyname"://  Post   Fuzzy Lookup 
                    s_responsetext = GetStationByName();
                    break;
                case "getflowtree":// Get process categories and processes 
                    s_responsetext = GetFlowTree();
                    break;
                case "getformtree":// Get a form library 
                    s_responsetext = GetFormTree();
                    break;
                case "saveflowformmenu":// Save Process / Forms menu 
                    s_responsetext = SaveFlowFormMenu();
                    break;
            }
            if (string.IsNullOrEmpty(s_responsetext))
                s_responsetext = "";
            // Assembly ajax String format , Return to the calling client 
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "text/html";
            Response.Expires = 0;
            Response.Write(s_responsetext);
            Response.End();
        }

        #region  Model menu creation 
        /// <summary>
        ///  Get process categories and processes 
        /// </summary>
        /// <returns></returns>
        private string GetFlowTree()
        {
            string nodeNo = getUTF8ToString("nodeNo");
            StringBuilder flowTree = new StringBuilder();
            flowTree.Append("[");
            if (nodeNo == "0")
            {
                // Process category information 
                BP.WF.FlowSort flowSort = new BP.WF.FlowSort();
                flowSort.Retrieve("ParentNo", "0");

                if (flowSort != null)
                {
                    flowTree.Append("{");
                    flowTree.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\",\"iconCls\":\"icon-tree_folder\",\"state\":\"open\"", flowSort.No, flowSort.Name));
                    DataTable dt_sort = flowSort.RunSQLReturnTable("select * from wf_flowsort where ParentNo='" + flowSort.No + "' order by Idx");
                    DataTable dt_flow = flowSort.RunSQLReturnTable("select No,Name from wf_flow Where FK_FlowSort = '" + flowSort.No + "'");
                    flowTree.Append(",\"children\":");
                    flowTree.Append("[");
                    // Binding Process category 
                    if (dt_sort != null && dt_sort.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt_sort.Rows)
                        {
                            flowTree.Append("{");
                            flowTree.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\",\"iconCls\":\"icon-tree_folder\",\"state\":\"closed\"", row["No"].ToString(), row["Name"].ToString()));
                            flowTree.Append(",\"children\":");
                            flowTree.Append("[{");
                            flowTree.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\"", row["No"].ToString() + "01", " Loading ..."));
                            flowTree.Append("}]");
                            flowTree.Append("},");
                        }
                        flowTree = flowTree.Remove(flowTree.Length - 1, 1);
                    }
                    // Binding Process 
                    if (dt_flow != null && dt_flow.Rows.Count > 0)
                    {
                        // Determine whether there is a folder , If you have to add a comma 
                        if (dt_sort != null && dt_sort.Rows.Count > 0)
                        {
                            flowTree.Append(",");
                        }
                        foreach (DataRow flowRow in dt_flow.Rows)
                        {
                            flowTree.Append("{");
                            flowTree.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\",\"iconCls\":\"icon-4\"", flowRow["No"].ToString(), flowRow["Name"].ToString()));
                            flowTree.Append("},");
                        }
                        flowTree = flowTree.Remove(flowTree.Length - 1, 1);
                    }
                    flowTree.Append("]");
                    flowTree.Append("}");
                }
            }
            else
            {
                BP.WF.FlowSort flowSort = new BP.WF.FlowSort();
                DataTable dt_sort = flowSort.RunSQLReturnTable("select * from wf_flowsort where ParentNo='" + nodeNo + "' order by Idx");
                DataTable dt_flow = flowSort.RunSQLReturnTable("select No,Name from wf_flow Where FK_FlowSort = '" + nodeNo + "'");

                // Binding Process category 
                if (dt_sort != null && dt_sort.Rows.Count > 0)
                {
                    foreach (DataRow row in dt_sort.Rows)
                    {
                        flowTree.Append("{");
                        flowTree.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\",\"iconCls\":\"icon-tree_folder\",\"state\":\"closed\"", row["No"].ToString(), row["Name"].ToString()));
                        flowTree.Append(",\"children\":");
                        flowTree.Append("[{");
                        flowTree.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\"", row["No"].ToString() + "01", " Loading ..."));
                        flowTree.Append("}]");
                        flowTree.Append("},");
                    }
                    flowTree = flowTree.Remove(flowTree.Length - 1, 1);
                }
                // Binding Process 
                if (dt_flow != null && dt_flow.Rows.Count > 0)
                {
                    foreach (DataRow flowRow in dt_flow.Rows)
                    {
                        if (flowTree.Length == 1)
                            flowTree.Append("{");
                        else
                            flowTree.Append(",{");
                        flowTree.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\",\"iconCls\":\"icon-4\"", flowRow["No"].ToString(), flowRow["Name"].ToString()));
                        flowTree.Append("}");
                    }
                }
            }
            flowTree.Append("]");

            return flowTree.ToString();
        }

        /// <summary>
        ///  Get a form library 
        /// </summary>
        /// <returns></returns>
        private string GetFormTree()
        {
            string nodeNo = getUTF8ToString("nodeNo");
            StringBuilder formTree = new StringBuilder();
            formTree.Append("[");
            if (nodeNo == "0")
            {
                // Category Information Form 
                BP.Sys.SysFormTree formSort = new BP.Sys.SysFormTree();
                formSort.Retrieve("No", "0");

                if (formSort != null)
                {
                    formTree.Append("{");
                    formTree.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\",\"iconCls\":\"icon-tree_folder\",\"state\":\"open\"", formSort.No, formSort.Name));
                    DataTable dt_sort = formSort.RunSQLReturnTable("select * from sys_formtree where ParentNo='" + formSort.No + "' order by Idx");
                    DataTable dt_form = formSort.RunSQLReturnTable("select No,Name from SYS_MAPDATA Where FK_FrmSort = '" + formSort.No + "'");
                    formTree.Append(",\"children\":");
                    formTree.Append("[");
                    // Binding Form Category 
                    if (dt_sort != null && dt_sort.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt_sort.Rows)
                        {
                            formTree.Append("{");
                            formTree.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\",\"iconCls\":\"icon-tree_folder\",\"state\":\"closed\"", row["No"].ToString(), row["Name"].ToString()));
                            formTree.Append(",\"children\":");
                            formTree.Append("[{");
                            formTree.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\"", row["No"].ToString() + "01", " Loading ..."));
                            formTree.Append("}]");
                            formTree.Append("},");
                        }
                        formTree = formTree.Remove(formTree.Length - 1, 1);
                    }
                    // Unbound form 
                    if (dt_form != null && dt_form.Rows.Count > 0)
                    {
                        // Determine whether there is a folder , If you have to add a comma 
                        if (dt_sort != null && dt_sort.Rows.Count > 0)
                        {
                            formTree.Append(",");
                        }
                        foreach (DataRow formRow in dt_form.Rows)
                        {
                            formTree.Append("{");
                            formTree.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\",\"iconCls\":\"icon-4\"", formRow["No"].ToString(), formRow["Name"].ToString()));
                            formTree.Append("},");
                        }
                        formTree = formTree.Remove(formTree.Length - 1, 1);
                    }
                    formTree.Append("]");
                    formTree.Append("}");
                }
            }
            else
            {
                BP.Sys.SysFormTree formSort = new BP.Sys.SysFormTree();
                DataTable dt_sort = formSort.RunSQLReturnTable("select * from sys_formtree where ParentNo='" + nodeNo + "' order by Idx");
                DataTable dt_form = formSort.RunSQLReturnTable("select No,Name from SYS_MAPDATA Where FK_FrmSort = '" + nodeNo + "'");

                // Binding Form Category 
                if (dt_sort != null && dt_sort.Rows.Count > 0)
                {
                    foreach (DataRow row in dt_sort.Rows)
                    {
                        formTree.Append("{");
                        formTree.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\",\"iconCls\":\"icon-tree_folder\",\"state\":\"closed\"", row["No"].ToString(), row["Name"].ToString()));
                        formTree.Append(",\"children\":");
                        formTree.Append("[{");
                        formTree.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\"", row["No"].ToString() + "01", " Loading ..."));
                        formTree.Append("}]");
                        formTree.Append("},");
                    }
                    formTree = formTree.Remove(formTree.Length - 1, 1);
                }
                // Unbound form 
                if (dt_form != null && dt_form.Rows.Count > 0)
                {
                    foreach (DataRow formRow in dt_form.Rows)
                    {
                        if (formTree.Length == 1)
                            formTree.Append("{");
                        else
                            formTree.Append(",{");
                        formTree.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\",\"iconCls\":\"icon-4\"", formRow["No"].ToString(), formRow["Name"].ToString()));
                        formTree.Append("}");
                    }
                }
            }
            formTree.Append("]");

            return formTree.ToString();
        }

        /// <summary>
        ///  Save Process , Forms menu 
        /// </summary>
        /// <returns></returns>
        private string SaveFlowFormMenu()
        {
            string model = getUTF8ToString("model");
            string curMenuNo = getUTF8ToString("curMenuNo");
            string pastSortNos = getUTF8ToString("pastSortNos");
            string pastNos = getUTF8ToString("pastNos");

            // Process Model 
            if (model == "flow")
            {
                // The sub-items under the category added 
                GetFlowBySortNos(pastSortNos, ref pastNos);
                // Traversal selected process 
                if (!string.IsNullOrEmpty(pastNos))
                {
                    string[] flowNos = pastNos.Split(',');
                    foreach (string item in flowNos)
                    {
                        if (string.IsNullOrEmpty(item))
                            continue;
                        CreateFlowMenu(item, curMenuNo);
                    }
                }
                return pastNos;
            }
            // Forms Mode 
            if (model == "form")
            {
                // The sub-items under the category added 
                GetFormBySortNos(pastSortNos, ref pastNos);
                // Traversing the selected form 
                if (!string.IsNullOrEmpty(pastNos))
                {
                    string[] formNos = pastNos.Split(',');
                    foreach (string item in formNos)
                    {
                        if (string.IsNullOrEmpty(item))
                            continue;
                        CreateFormMenu(item, curMenuNo);
                    }
                }
                return pastNos;
            }
            return "true";
        }

        /// <summary>
        ///  Get the process ID number by category 
        /// </summary>
        /// <returns></returns>
        private void GetFlowBySortNos(string flowSortNos, ref string flowNos)
        {
            string[] sortNos = flowSortNos.Split(',');
            foreach (string sortNo in sortNos)
            {
                // Subkey 
                BP.WF.FlowSorts sorts = new BP.WF.FlowSorts();
                sorts.Retrieve(BP.WF.FlowSortAttr.ParentNo, sortNo);
                if (sorts != null && sorts.Count > 0)
                {
                    foreach (BP.WF.FlowSort sort in sorts)
                    {
                        GetFlowBySortNos(sort.No, ref flowNos);
                    }
                }

                BP.WF.Flows flows = new BP.WF.Flows(sortNo);
                if (flows != null && flows.Count > 0)
                {
                    foreach (BP.WF.Flow flow in flows)
                    {
                        if (flowNos.Contains(flow.No))
                            continue;
                        if (string.IsNullOrEmpty(flowNos))
                            flowNos += flow.No;
                        else
                            flowNos += "," + flow.No;
                    }
                }
            }
        }

        /// <summary>
        ///  Number Gets the number by category 
        /// </summary>
        /// <returns></returns>
        private void GetFormBySortNos(string formSortNos, ref string formNos)
        {
            string[] sortNos = formSortNos.Split(',');
            foreach (string sortNo in sortNos)
            {
                // Subkey 
                if (string.IsNullOrEmpty(sortNo))
                    continue;

                BP.Sys.SysFormTrees sorts = new BP.Sys.SysFormTrees();
                sorts.Retrieve(BP.Sys.SysFormTreeAttr.ParentNo, sortNo);
                if (sorts != null && sorts.Count > 0)
                {
                    formSortNos = "";
                    foreach (BP.Sys.SysFormTree sort in sorts)
                    {
                        if (formSortNos == "")
                            formSortNos = sort.No;
                        else
                            formSortNos += "," + sort.No;
                    }
                    GetFormBySortNos(formSortNos, ref formNos);
                }

                BP.Sys.MapDatas forms = new BP.Sys.MapDatas();
                forms.Retrieve(BP.Sys.MapDataAttr.FK_FrmSort, sortNo);
                if (forms != null && forms.Count > 0)
                {
                    foreach (BP.Sys.MapData form in forms)
                    {
                        if (formNos.Contains(form.No))
                            continue;

                        if (string.IsNullOrEmpty(formNos))
                            formNos += form.No;
                        else
                            formNos += "," + form.No;
                    }
                }
            }
        }
        /// <summary>
        ///  Menu creation process 
        /// </summary>
        /// <param name="FK_Flow"></param>
        /// <param name="curMenuNo"></param>
        private void CreateFlowMenu(string FK_Flow, string curMenuNo)
        {
            BP.WF.Flow flow = new BP.WF.Flow();
            flow.Retrieve(BP.WF.Template.FlowAttr.No, FK_Flow);
            if (flow != null && flow.Name != "")
            {
                BP.GPM.Menu menu = new BP.GPM.Menu(curMenuNo);
                // New launch menu 
                BP.GPM.Menu flowMenu = menu.DoCreateSubNode() as BP.GPM.Menu;
                flowMenu.FK_App = menu.FK_App;
                flowMenu.Name = " Launch " + flow.Name;
                flowMenu.MenuType = 4;
                flowMenu.Flag = FK_Flow;
                flowMenu.Url = BP.WF.Glo.CCFlowAppPath + "WF/MyFlow.aspx?FK_Flow=" + FK_Flow + "&FK_Node=" + Convert.ToInt32(FK_Flow) + "01";
                flowMenu.Update();
                // New Query menu 
                flowMenu = menu.DoCreateSubNode() as BP.GPM.Menu;
                flowMenu.FK_App = menu.FK_App;
                flowMenu.Name = " Inquiry " + flow.Name;
                flowMenu.MenuType = 4;
                flowMenu.Flag = FK_Flow;
                flowMenu.Url = BP.WF.Glo.CCFlowAppPath + "WF/Rpt/Search.aspx?RptNo=ND" + Convert.ToInt32(FK_Flow) + "MyRpt";
                flowMenu.Update();
                // New analysis menu 
                flowMenu = menu.DoCreateSubNode() as BP.GPM.Menu;
                flowMenu.FK_App = menu.FK_App;
                flowMenu.Name = flow.Name + " Statistical Analysis ";
                flowMenu.MenuType = 4;
                flowMenu.Flag = FK_Flow;
                flowMenu.Url = BP.WF.Glo.CCFlowAppPath + "WF/Rpt/Group.aspx?DoType=Dept&FK_Flow=" + FK_Flow;
                flowMenu.Update();
            }
        }

        /// <summary>
        ///  Create a form menu 
        /// </summary>
        private void CreateFormMenu(string FK_MapData, string curMenuNo)
        {
            BP.Sys.MapData formMap = new BP.Sys.MapData();
            formMap.Retrieve(BP.Sys.MapDataAttr.No, FK_MapData);
            if (formMap != null && formMap.Name != "")
            {
                BP.GPM.Menu menu = new BP.GPM.Menu(curMenuNo);
                // New List 
                BP.GPM.Menu formMenu = menu.DoCreateSubNode() as BP.GPM.Menu;
                formMenu.FK_App = menu.FK_App;
                formMenu.Name = formMap.Name;
                formMenu.MenuType = 4;
                formMenu.Flag = formMap.No;
                formMenu.Url = BP.WF.Glo.CCFlowAppPath + "WF/Rpt/SearchEUI.aspx?FK_MapData=" + FK_MapData;
                formMenu.Update();
                // Add button 
                //BP.GPM.Menu btnMenu = formMenu.DoCreateSubNode() as BP.GPM.Menu;
                //flowMenu.FK_App = menu.FK_App;
                //flowMenu.Name = flow.Name + " Inquiry ";
                //flowMenu.MenuType = 4;
                //flowMenu.Flag = FK_Flow;
                //flowMenu.Url = BP.WF.Glo.CCFlowAppPath + "WF/Rpt/Search.aspx?RptNo=ND" + Convert.ToInt32(FK_Flow) + "MyRpt";
                //flowMenu.Update();                
            }
        }

        #endregion

        #region   By job assignment menu 
        /// <summary>
        ///  Get all posts 
        /// </summary>
        /// <returns></returns>
        public string GetStations()
        {
            Stations stations = new Stations();
            stations.RetrieveAll("No");
            return TranslateEntitiesToGridJsonOnlyData(stations);
        }
        /// <summary>
        ///  Save   Post   Menu 
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

                // Subkey contains entries will not be added to expand the selected and unselected items 
                if (!string.IsNullOrEmpty(menuIdsUnExt))
                {
                    string[] menuParentNos = menuIdsUnExt.Split(',');
                    foreach (string item in menuParentNos)
                    {
                        SetUnCheckedStationOfMenus(stationNo, item, ref menuIds, ref menuIdsUn);
                    }
                }
                // Delete menu under the posts 
                StationMenus stationMenus = new StationMenus();
                stationMenus.Delete(StationMenuAttr.FK_Station, stationNo);

                // Save the selected menu 
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
                // Save the selected item is not completely 
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
                // Selection process is not complete , Does not include child not completely delete option 
                Del_UnCheckedNoChildNodes("StationMenu", stationNo);
            }
            catch (Exception ex)
            {
                return "error" + ex.Message;
            }
            return "success";
        }
        /// <summary>
        ///  Save jobs menu   Child nodes 
        /// </summary>
        /// <returns></returns>
        public void SaveStationOfMenusChild(string stationNo, string parentNo, string menuIds)
        {
            // Number Gets the child node based on parent 
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
        ///  Get job menu 
        /// </summary>
        public string GetStationOfMenusByNo()
        {
            string checkedMenuIds = "";
            string stationNo = getUTF8ToString("stationNo");
            string parentNo = getUTF8ToString("parentNo");
            string isLoadChild = getUTF8ToString("isLoadChild");
            // Number Gets the menu according to the post 
            StationMenus stationMenus = new StationMenus();


            QueryObject objWhere = new QueryObject(stationMenus);
            objWhere.AddWhere(StationMenuAttr.FK_Station, stationNo);
            objWhere.addAnd();
            objWhere.AddWhere(StationMenuAttr.IsChecked, true);
            objWhere.DoQuery();

            // Get node 
            BP.GPM.Menus menus = new BP.GPM.Menus();
            //menus.RetrieveByAttr("ParentNo", parentNo);
            QueryObject objMenu = new QueryObject(menus);
            objMenu.AddWhere(MenuAttr.ParentNo, parentNo);
            objMenu.addAnd();
            objMenu.AddWhere(MenuAttr.FK_App, "CCFlowBPM");
            objMenu.DoQuery();

            // Finishing the selected item 
            foreach (StationMenu item in stationMenus)
            {
                checkedMenuIds += "," + item.FK_Menu + ",";
            }
            // Consolidation is not fully checked 
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

            // If this is the first load 
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

                    // Node icon 
                    string ico = "icon-" + menu.MenuType;
                    // Judgment is not fully checked 
                    if (unCheckedMenuIds.Contains("," + item.No + ","))
                        ico = "collaboration";

                    appSend.Append(",iconCls:\"");
                    appSend.Append(ico);
                    appSend.Append("\"");

                    // Judge selected 
                    if (checkedMenuIds.Contains("," + item.No + ","))
                        appSend.Append(",\"checked\":true");

                    //  Increase its children .
                    appSend.Append(",\"children\":");
                    appSend.Append(GetMenusByParentNo(item.No, checkedMenuIds, unCheckedMenuIds, true));
                    appSend.Append("}");
                }
                appSend.Append("]");

                return appSend.ToString();
            }
            // Returns the child node acquired 
            return GetTreeList(menus, checkedMenuIds, unCheckedMenuIds);
        }

        /// <summary>
        ///  Empty style   Copy jobs    Save 
        /// </summary>
        /// <returns></returns>
        public string ClearOfCopyStation()
        {
            try
            {
                string copyStationNo = getUTF8ToString("copyStationNo");
                string pastStationNos = getUTF8ToString("pastStationNos");
                string[] pastArry = pastStationNos.Split(',');

                // Get permission to copy jobs 
                StationMenus copyStationMenus = new StationMenus();
                copyStationMenus.RetrieveByAttr(StationMenuAttr.FK_Station, copyStationNo);

                // Cycle of the target object 
                foreach (string pastStation in pastArry)
                {
                    // Empty prior permission 
                    StationMenu stationMenu = new StationMenu();
                    stationMenu.Delete(StationMenuAttr.FK_Station, pastStation);
                    // Authorize 
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
        ///  Overlay   Copy jobs   Save 
        /// </summary>
        /// <returns></returns>
        public string CoverOfCopyStation()
        {
            try
            {
                string copyStationNo = getUTF8ToString("copyStationNo");
                string pastStationNos = getUTF8ToString("pastStationNos");
                string[] pastArry = pastStationNos.Split(',');

                // Get copy job   Competence 
                StationMenus copyStationMenus = new StationMenus();
                copyStationMenus.RetrieveByAttr(StationMenuAttr.FK_Station, copyStationNo);

                // Cycle of the target object 
                foreach (string pastStaion in pastArry)
                {
                    // Authorize 
                    foreach (StationMenu copyMenu in copyStationMenus)
                    {
                        StationMenu stationMenu = new StationMenu();

                        bool isHave = stationMenu.RetrieveByAttrAnd(StationMenuAttr.FK_Station, pastStaion, StationMenuAttr.FK_Menu, copyMenu.FK_Menu);
                        // Determine whether there is prior permission 
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
        ///  Post   Fuzzy   Find 
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
        ///  Get the template data 
        /// </summary>
        /// <returns></returns>
        public string getTemplateData()
        {
            string sql = "";
            string menuNo = getUTF8ToString("menuNo");
            string model = getUTF8ToString("model");
            // By job assignment 
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
                // Get all posts 
                StationMenu station = new StationMenu();
                DataTable dt_StationMenu = station.RunSQLReturnTable(sql);
                string rVal = CommonDbOperator.GetListJsonFromTable(dt_StationMenu);
                rVal = "{station:" + rVal + "}";
                return rVal;
            }
            // Assign permissions by group 
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
                // Get all rights group 
                Group group = new Group();
                DataTable dt_GroupMenu = group.RunSQLReturnTable(sql);

                string rVal = CommonDbOperator.GetListJsonFromTable(dt_GroupMenu);
                rVal = "{group:" + rVal + "}";
                return rVal;
            }
            // Assigned by the user menu 
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
        ///  Press Menu to assign permissions to save 
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
                // Assign permissions by user 
                if (curModel == "emp")
                {
                    // All users delete menu 
                    UserMenus userMenus = new UserMenus();
                    userMenus.Delete("FK_Menu", menuNo);
                    // Users are authorized 
                    foreach (string item in str_Arrary)
                    {
                        UserMenu userMenu = new UserMenu();
                        userMenu.FK_Emp = item;
                        userMenu.FK_Menu = menuNo;
                        userMenu.IsChecked = "1";
                        userMenu.Insert();

                        // Save submenu 
                        if (saveChildNode == "true")
                            SaveUserOfMenusChild(item, menuNo, menuNo);
                    }
                    return "true";
                }
                // Assign permissions by post 
                if (curModel == "station")
                {
                    // All posts Delete menu 
                    StationMenus staMenus = new StationMenus();
                    staMenus.Delete("FK_Menu", menuNo);

                    // Users are authorized 
                    foreach (string item in str_Arrary)
                    {
                        StationMenu stationMenu = new StationMenu();
                        stationMenu.FK_Station = item;
                        stationMenu.FK_Menu = menuNo;
                        stationMenu.IsChecked = "1";
                        stationMenu.Insert();
                        // Save submenu 
                        if (saveChildNode == "true")
                            SaveStationOfMenusChild(item, menuNo, menuNo);
                    }
                    return "true";
                }
                // Rights Groups delete menu 
                GroupMenus groupMenus = new GroupMenus();
                groupMenus.Delete(GroupMenuAttr.FK_Menu, menuNo);
                // Authorization permissions group 
                foreach (string item in str_Arrary)
                {
                    GroupMenu groupMenu = new GroupMenu();
                    groupMenu.FK_Group = item;
                    groupMenu.FK_Menu = menuNo;
                    groupMenu.IsChecked = "1";
                    groupMenu.Insert();
                    // Authorize the child node 
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
        ///  Get contains personnel department 
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
        ///  Get   All sectors 
        /// </summary>
        /// <returns></returns>
        public string GetAllDept()
        {
            Depts depts = new Depts();
            depts.RetrieveAll();
            return TranslateEntitiesToGridJsonOnlyData(depts);
        }

        /// <summary>
        ///  Get all personnel information 
        /// </summary>
        /// <returns></returns>
        private string GetEmps()
        {
            Emps emps = new Emps();
            emps.RetrieveAll("No");
            return TranslateEntitiesToGridJsonOnlyData(emps);
        }
        /// <summary>
        ///  Fuzzy query based on user ID or name 
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
        ///  Find all rights group 
        /// </summary>
        /// <returns></returns>
        private string GetEmpGroups()
        {
            Groups groups = new Groups();
            groups.RetrieveAll(GroupAttr.Idx);
            return TranslateEntitiesToGridJsonOnlyData(groups);
        }
        /// <summary>
        ///  Rights Groups fuzzy search 
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
        ///  Save the relationship between the user and the menu 
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

                // Subkey contains entries will not be added to expand the selected and unselected items 
                if (!string.IsNullOrEmpty(menuIdsUnExt))
                {
                    string[] menuParentNos = menuIdsUnExt.Split(',');
                    foreach (string item in menuParentNos)
                    {
                        SetUnCheckedUserOfMenus(empNo, item, ref menuIds, ref menuIdsUn);
                    }
                }

                // Delete User menu under 
                UserMenus userMenus = new UserMenus();
                userMenus.Delete("FK_Emp", empNo);
                // Save the selected menu 
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
                // Save the selected item is not completely 
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
                // Selection process is not complete , Does not include child not completely delete option 
                Del_UnCheckedNoChildNodes("UserMenu", empNo);
            }
            catch (Exception ex)
            {
                return "error" + ex.Message;
            }
            return "success";
        }
        /// <summary>
        ///  Save User Menu child nodes 
        /// </summary>
        private void SaveUserOfMenusChild(string fk_EmpNo, string parentNo, string menuIds)
        {
            // Number Gets the child node based on parent 
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
        ///  Setting item is not expanded 
        /// </summary>
        /// <param name="fk_EmpNo"> User ID </param>
        /// <param name="parentNo"> Parent node number </param>
        /// <param name="menuIds"> Select items , Splicing </param>
        /// <param name="menuIdsUn"> Not fully checked items , Splicing </param>
        private void SetUnCheckedUserOfMenus(string fk_EmpNo, string parentNo, ref string menuIds, ref string menuIdsUn)
        {
            // Number Gets the child node based on parent 
            BP.GPM.Menu menu = new BP.GPM.Menu();
            string sql = "SELECT a.FK_Emp,a.FK_Menu,a.IsChecked FROM GPM_UserMenu a,GPM_Menu b "
                            + " WHERE a.FK_Menu = b.No "
                            + " AND b.ParentNo='" + parentNo + "'"
                            + " AND a.FK_Emp='" + fk_EmpNo + "'";
            // Get datasets 
            DataTable dt = menu.RunSQLReturnTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    // Not fully checked 
                    if (row["IsChecked"].ToString() == "0")
                    {
                        if (!menuIdsUn.Contains(row["FK_Menu"].ToString()))
                            menuIdsUn += "," + row["FK_Menu"];
                    }
                    // Selected 
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
                    // Iterative processing 
                    SetUnCheckedUserOfMenus(fk_EmpNo, row["FK_Menu"].ToString(), ref menuIds, ref menuIdsUn);
                }
            }
        }
        /// <summary>
        ///  Setting item is not expanded 
        /// </summary>
        /// <param name="fk_StationNo"> Job numbers </param>
        /// <param name="parentNo"> Parent node number </param>
        /// <param name="menuIds"> Select items , Splicing </param>
        /// <param name="menuIdsUn"> Not fully checked items , Splicing </param>
        private void SetUnCheckedStationOfMenus(string fk_StationNo, string parentNo, ref string menuIds, ref string menuIdsUn)
        {
            // Number Gets the child node based on parent 
            BP.GPM.Menu menu = new BP.GPM.Menu();
            string sql = "SELECT a.FK_Station,a.FK_Menu,a.IsChecked FROM GPM_StationMenu a,GPM_Menu b "
                            + " WHERE a.FK_Menu = b.No "
                            + " AND b.ParentNo='" + parentNo + "'"
                            + " AND a.FK_Station='" + fk_StationNo + "'";
            // Get datasets 
            DataTable dt = menu.RunSQLReturnTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    // Not fully checked 
                    if (row["IsChecked"].ToString() == "0")
                    {
                        if (!menuIdsUn.Contains(row["FK_Menu"].ToString()))
                            menuIdsUn += "," + row["FK_Menu"];
                    }
                    // Selected 
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
                    // Iterative processing 
                    SetUnCheckedStationOfMenus(fk_StationNo, row["FK_Menu"].ToString(), ref menuIds, ref menuIdsUn);
                }
            }
        }

        /// <summary>
        ///  Setting item is not expanded 
        /// </summary>
        /// <param name="FK_Group"> Permissions Group Number </param>
        /// <param name="parentNo"> Parent node number </param>
        /// <param name="menuIds"> Select items , Splicing </param>
        /// <param name="menuIdsUn"> Not fully checked items , Splicing </param>
        private void SetUnCheckedGroupOfMenus(string FK_Group, string parentNo, ref string menuIds, ref string menuIdsUn)
        {
            // Number Gets the child node based on parent 
            BP.GPM.Menu menu = new BP.GPM.Menu();
            string sql = "SELECT a.FK_Group,a.FK_Menu,a.IsChecked FROM GPM_GroupMenu a,GPM_Menu b "
                            + " WHERE a.FK_Menu = b.No "
                            + " AND b.ParentNo='" + parentNo + "'"
                            + " AND a.FK_Group='" + FK_Group + "'";
            // Get datasets 
            DataTable dt = menu.RunSQLReturnTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    // Not fully checked 
                    if (row["IsChecked"].ToString() == "0")
                    {
                        if (!menuIdsUn.Contains(row["FK_Menu"].ToString()))
                            menuIdsUn += "," + row["FK_Menu"];
                    }
                    // Selected 
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
                    // Iterative processing 
                    SetUnCheckedGroupOfMenus(FK_Group, row["FK_Menu"].ToString(), ref menuIds, ref menuIdsUn);
                }
            }
        }
        /// <summary>
        ///  The choice is not completely , Node does not contain child nodes deleted 
        /// </summary>
        /// <param name="saveType"></param>
        /// <param name="FK_Val"></param>
        private void Del_UnCheckedNoChildNodes(string saveType, string FK_Val)
        {
            string sql = "";
            // Save User Menu 
            if (saveType == "UserMenu")
            {
                UserMenus userMenus = new UserMenus();
                userMenus.Retrieve("FK_Emp", FK_Val, "IsChecked", "0");
                if (userMenus != null && userMenus.Count > 0)
                {
                    // Circulation delete child entries , Methods to be optimized ; Remove 3 Association level 
                    for (int i = 0, k = 3; i < k; i++)
                    {
                        foreach (UserMenu userMenu in userMenus)
                        {
                            sql = "SELECT a.FK_Emp,a.FK_Menu,a.IsChecked FROM GPM_UserMenu a,GPM_Menu b "
                                + " WHERE a.FK_Menu = b.No"
                                + " AND b.ParentNo='" + userMenu.FK_Menu + "'"
                                + " AND a.FK_Emp='" + FK_Val + "'";
                            DataTable dt_UserMenu = DBAccess.RunSQLReturnTable(sql);
                            // Determine whether it contains child 
                            if (dt_UserMenu == null || dt_UserMenu.Rows.Count == 0)
                            {
                                // Delete 
                                DBAccess.RunSQL("DELETE FROM GPM_UserMenu WHERE FK_Emp='" + FK_Val + "' and FK_Menu='" + userMenu.FK_Menu + "'");
                            }
                        }
                    }
                }
            }
            else if (saveType == "StationMenu")// Status menu 
            {
                StationMenus stationMenus = new StationMenus();
                stationMenus.Retrieve("FK_Station", FK_Val, "IsChecked", "0");
                if (stationMenus != null && stationMenus.Count > 0)
                {
                    // Circulation delete child entries , Methods to be optimized ; Remove 3 Association level 
                    for (int i = 0, k = 3; i < k; i++)
                    {
                        foreach (StationMenu stationMenu in stationMenus)
                        {
                            sql = "SELECT a.FK_Station,a.FK_Menu,a.IsChecked FROM GPM_StationMenu a,GPM_Menu b "
                                + " WHERE a.FK_Menu = b.No"
                                + " AND b.ParentNo='" + stationMenu.FK_Menu + "'"
                                + " AND a.FK_Station='" + FK_Val + "'";
                            DataTable dt_StationMenu = DBAccess.RunSQLReturnTable(sql);
                            // Determine whether it contains child 
                            if (dt_StationMenu == null || dt_StationMenu.Rows.Count == 0)
                            {
                                // Delete 
                                DBAccess.RunSQL("DELETE FROM GPM_StationMenu WHERE FK_Station='" + FK_Val + "' and FK_Menu='" + stationMenu.FK_Menu + "'");
                            }
                        }
                    }
                }
            }
            else if (saveType == "GroupMenu")// Permissions Group Menu 
            {
                GroupMenus groupMenus = new GroupMenus();
                groupMenus.Retrieve("FK_Group", FK_Val, "IsChecked", "0");
                if (groupMenus != null && groupMenus.Count > 0)
                {
                    // Circulation delete child entries , Methods to be optimized ; Remove 3 Association level 
                    for (int i = 0, k = 3; i < k; i++)
                    {
                        foreach (GroupMenu groupMenu in groupMenus)
                        {
                            sql = "SELECT a.FK_Group,a.FK_Menu,a.IsChecked FROM GPM_GroupMenu a,GPM_Menu b "
                                + " WHERE a.FK_Menu = b.No"
                                + " AND b.ParentNo='" + groupMenu.FK_Menu + "'"
                                + " AND a.FK_Group='" + FK_Val + "'";
                            DataTable dt_StationMenu = DBAccess.RunSQLReturnTable(sql);
                            // Determine whether it contains child 
                            if (dt_StationMenu == null || dt_StationMenu.Rows.Count == 0)
                            {
                                // Delete 
                                DBAccess.RunSQL("DELETE FROM GPM_GroupMenu WHERE FK_Group='" + FK_Val + "' and FK_Menu='" + groupMenu.FK_Menu + "'");
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        ///  Save a child node permissions set menu 
        /// </summary>
        private void SaveGroupOfMenusChild(string fk_GroupNo, string parentNo, string menuIds)
        {
            // Number Gets the child node based on parent 
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
        ///  Get permission group menu 
        /// </summary>
        /// <returns></returns>
        private string GetEmpGroupOfMenusByNo()
        {
            string checkedMenuIds = "";
            string groupNO = getUTF8ToString("groupNo");
            string parentNo = getUTF8ToString("parentNo");
            string isLoadChild = getUTF8ToString("isLoadChild");
            // Number Gets the menu under the authority of the group 
            GroupMenus groupMenus = new GroupMenus();

            QueryObject objWhere = new QueryObject(groupMenus);
            objWhere.AddWhere(GroupMenuAttr.FK_Group, groupNO);
            objWhere.addAnd();
            objWhere.AddWhere(GroupMenuAttr.IsChecked, true);

            objWhere.DoQuery();
            // Get node 
            BP.GPM.Menus menus = new BP.GPM.Menus();
            //menus.RetrieveByAttr("ParentNo", parentNo);
            QueryObject objMenu = new QueryObject(menus);
            objMenu.AddWhere(MenuAttr.ParentNo, parentNo);
            objMenu.addAnd();
            objMenu.AddWhere(MenuAttr.FK_App, "CCFlowBPM");
            objMenu.DoQuery();

            // Finishing the selected item 
            foreach (GroupMenu item in groupMenus)
            {
                checkedMenuIds += "," + item.FK_Menu + ",";
            }
            // Consolidation is not fully checked 
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

            // If this is the first load 
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

                    // Node icon 
                    string ico = "icon-" + menu.MenuType;
                    // Judgment is not fully checked 
                    if (unCheckedMenuIds.Contains("," + item.No + ","))
                        ico = "collaboration";

                    appSend.Append(",iconCls:\"");
                    appSend.Append(ico);
                    appSend.Append("\"");

                    // Judge selected 
                    if (checkedMenuIds.Contains("," + item.No + ","))
                        appSend.Append(",\"checked\":true");

                    //  Increase its children .
                    appSend.Append(",\"children\":");
                    appSend.Append(GetMenusByParentNo(item.No, checkedMenuIds, unCheckedMenuIds, true));
                    appSend.Append("}");
                }
                appSend.Append("]");

                return appSend.ToString();
            }
            // Returns the child node acquired 
            return GetTreeList(menus, checkedMenuIds, unCheckedMenuIds);
        }

        /// <summary>
        ///  Save Permissions Group Menu 
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

                // Subkey contains entries will not be added to expand the selected and unselected items 
                if (!string.IsNullOrEmpty(menuIdsUnExt))
                {
                    string[] menuParentNos = menuIdsUnExt.Split(',');
                    foreach (string item in menuParentNos)
                    {
                        SetUnCheckedGroupOfMenus(groupNo, item, ref menuIds, ref menuIdsUn);
                    }
                }

                // Delete menu under the right group 
                GroupMenus groupMenus = new GroupMenus();
                groupMenus.Delete(GroupMenuAttr.FK_Group, groupNo);

                // Save the selected menu 
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
                // Save the selected item is not completely 
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
                // Selection process is not complete , Does not include child not completely delete option 
                Del_UnCheckedNoChildNodes("GroupMenu", groupNo);
            }
            catch (Exception ex)
            {
                return "error" + ex.Message;
            }
            return "success";
        }

        /// <summary>
        ///  Empty replication user rights 
        /// </summary>
        /// <returns></returns>
        private string ClearOfCopyUserPower()
        {
            try
            {
                string copyUser = getUTF8ToString("copyUser");
                string pastUsers = getUTF8ToString("pastUsers");
                string[] pastArry = pastUsers.Split(',');


                // Get copy user permissions 
                UserMenu userMenu = new UserMenu();
                DataTable userMenu_dt = userMenu.RunSQLReturnTable("SELECT FK_Menu,IsChecked FROM V_GPM_EmpMenu_GPM WHERE FK_Emp='" + copyUser + "'");

                // Cycle of the target object 
                foreach (string pastUser in pastArry)
                {
                    // Empty prior permission 
                    userMenu = new UserMenu();
                    userMenu.FK_Emp = pastUser;
                    userMenu.Delete();

                    // Authorize 
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
        ///  Overlay copy user permissions 
        /// </summary>
        /// <returns></returns>
        private string CoverOfCopyUserPower()
        {
            try
            {
                string copyUser = getUTF8ToString("copyUser");
                string pastUsers = getUTF8ToString("pastUsers");
                string[] pastArry = pastUsers.Split(',');


                // Get copy user permissions 
                UserMenu userMenu = new UserMenu();
                DataTable userMenu_dt = userMenu.RunSQLReturnTable("SELECT FK_Menu,IsChecked FROM V_GPM_EmpMenu_GPM WHERE FK_Emp='" + copyUser + "'");

                // Cycle of the target object 
                foreach (string pastUser in pastArry)
                {
                    // Authorize 
                    foreach (DataRow row in userMenu_dt.Rows)
                    {
                        // Permissions determine whether there is 
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
        ///  Empty replication permissions 
        /// </summary>
        /// <returns></returns>
        private string ClearOfCopyUserGroupPower()
        {
            try
            {
                string copyGroupNo = getUTF8ToString("copyGroupNo");
                string pastGroupNos = getUTF8ToString("pastGroupNos");
                string[] pastArry = pastGroupNos.Split(',');

                // Get copy rights group permissions 
                GroupMenus copyGroupMenus = new GroupMenus();
                copyGroupMenus.RetrieveByAttr(GroupMenuAttr.FK_Group, copyGroupNo);

                // Cycle of the target object 
                foreach (string pastGroup in pastArry)
                {
                    // Empty prior permission 
                    GroupMenu groupMenu = new GroupMenu();
                    groupMenu.Delete(GroupMenuAttr.FK_Group, pastGroup);

                    // Authorize 
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
        ///  Overlay copy rights 
        /// </summary>
        /// <returns></returns>
        private string CoverOfCopyUserGroupPower()
        {
            try
            {
                string copyGroupNo = getUTF8ToString("copyGroupNo");
                string pastGroupNos = getUTF8ToString("pastGroupNos");
                string[] pastArry = pastGroupNos.Split(',');

                // Get copy rights group permissions 
                GroupMenus copyGroupMenus = new GroupMenus();
                copyGroupMenus.RetrieveByAttr(GroupMenuAttr.FK_Group, copyGroupNo);

                // Cycle of the target object 
                foreach (string pastGroup in pastArry)
                {
                    // Authorize 
                    foreach (GroupMenu copyMenu in copyGroupMenus)
                    {
                        GroupMenu groupMenu = new GroupMenu();
                        bool isHave = groupMenu.RetrieveByAttrAnd(GroupMenuAttr.FK_Group, pastGroup, GroupMenuAttr.FK_Menu, copyMenu.FK_Menu);
                        // Determine whether there is prior permission 
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
        ///  Get System 
        /// </summary>
        /// <returns></returns>
        private string GetApps()
        {
            Apps apps = new Apps();

            apps.RetrieveAll();
            return TranslateEntitiesToGridJsonOnlyData(apps);
        }

        /// <summary>
        ///  Get the system log 
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
        ///  The entity class into json Format 
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
        ///  The entity class into json Format   Contains the column names and data 
        /// </summary>
        /// <param name="ens"></param>
        /// <returns></returns>
        public string TranslateEntitiesToGridJsonColAndData(BP.En.Entities ens)
        {
            Attrs attrs = ens.GetNewEntity.EnMap.Attrs;
            StringBuilder append = new StringBuilder();
            append.Append("{");
            // Finishing the column name 
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

            // Organize data 
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
        ///  Get all directories menu 
        /// </summary>
        /// <returns></returns>
        private string GetMenusOfMenuForEmp()
        {
            string parentNo = getUTF8ToString("parentNo");
            string isLoadChild = getUTF8ToString("isLoadChild");

            // Number Gets the child node based on parent 
            BP.GPM.Menus menus = new BP.GPM.Menus();
            //menus.RetrieveByAttr("ParentNo", parentNo);
            QueryObject objMenu = new QueryObject(menus);
            objMenu.AddWhere(MenuAttr.ParentNo, parentNo);
            objMenu.addAnd();
            objMenu.AddWhere(MenuAttr.FK_App, "CCFlowBPM");
            objMenu.DoQuery();
            // If this is the first load 
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

                    // Node icon 
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
            // Get the tree node data 
            return GetTreeList(menus, "", "");
        }

        // Load the left menu 
        private string GetLeftMenu()
        {
            StringBuilder menuApp = new StringBuilder();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(SystemConfig.PathOfXML + "BPMMenu.xml");
            // Get a list of the top-level node 
            XmlNodeList topM = xmlDoc.DocumentElement.ChildNodes;
            menuApp.Append("[");
            foreach (XmlNode node in topM)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    if (menuApp.Length > 1) menuApp.Append(",");
                    menuApp.Append("{");
                    menuApp.Append("No:'" + node.Attributes["No"].InnerText + "'");
                    menuApp.Append(",Name:'" + node.Attributes["Name"].InnerText.Trim().Replace(" ","_") + "'");
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
                                childrenMenu += ",Name:'" + cNode.Attributes["Name"].InnerText.Trim().Replace(" ", "_") + "'";
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
        ///  Get menu 
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
        ///  Get iteration menu 
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
        ///  Get the name of the menu according to the system 
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
                sysMenu.Name = " New Catalog ";
                sysMenu.ParentNo = app.RefMenuNo;
                sysMenu.FK_App = appName;
                sysMenu.MenuType = 3;
                sysMenu.IsDir = true;
                sysMenu.Insert();
                // Requeried 
                dt = sysMenu.RunSQLReturnTable("select * from GPM_Menu WHERE FK_App='" + appName + "' ORDER BY Idx");
                strMenus = CommonDbOperator.GetGridTreeDataString(dt, "ParentNo", "No", app.RefMenuNo, true);

                if (strMenus.Length > 2)
                    strMenus = strMenus.Remove(strMenus.Length - 2, 2);
            }
            return strMenus;
        }
        /// <summary>
        ///  According to numbers get submenu 
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
        ///  Get departments and users 
        /// </summary>
        /// <returns></returns>
        private string GetDeptEmpTree()
        {
            StringBuilder deptEmp = new StringBuilder();
            // Sector Information 
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
                // Binding department 
                if (dt_dept != null && dt_dept.Rows.Count > 0)
                {
                    foreach (DataRow row in dt_dept.Rows)
                    {
                        deptEmp.Append("{");
                        deptEmp.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\",\"iconCls\":\"icon-tree_folder\",\"state\":\"closed\"", row["No"].ToString(), row["Name"].ToString()));
                        deptEmp.Append(",\"children\":");
                        deptEmp.Append("[{");
                        deptEmp.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\"", row["No"].ToString() + "01", " Loading ..."));
                        deptEmp.Append("}]");
                        deptEmp.Append("},");
                    }
                    deptEmp = deptEmp.Remove(deptEmp.Length - 1, 1);
                }
                // Binding staff 
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
        ///  Users get the department of child nodes 
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
            // Binding department 
            if (dt_dept != null && dt_dept.Rows.Count > 0)
            {
                foreach (DataRow row in dt_dept.Rows)
                {
                    deptEmp.Append("{");
                    deptEmp.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\",\"iconCls\":\"icon-tree_folder\",\"state\":\"closed\"", row["No"].ToString(), row["Name"].ToString()));
                    deptEmp.Append(",\"children\":");
                    deptEmp.Append("[{");
                    deptEmp.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\"", row["No"].ToString() + "01", " Loading ..."));
                    deptEmp.Append("}]");
                    deptEmp.Append("},");
                }
                deptEmp = deptEmp.Remove(deptEmp.Length - 1, 1);
            }
            // Binding staff 
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
        ///  Menu Management 
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
                case "sample":// Added siblings                     
                    nodeNo = sysMenu.DoCreateSameLevelNode().No;
                    // The new node assignment 
                    BP.GPM.Menu newMenu = new BP.GPM.Menu(nodeNo); ;
                    newMenu.FK_App = sysMenu.FK_App;
                    newMenu.Update();
                    returnVal = newMenu.No;
                    break;
                case "children":// New child node 
                    nodeNo = sysMenu.DoCreateSubNode().No;
                    // The new node assignment 
                    BP.GPM.Menu newcMenu = new BP.GPM.Menu(nodeNo);
                    newcMenu.FK_App = sysMenu.FK_App;
                    newcMenu.Update();
                    returnVal = newcMenu.No;
                    break;
                case "doup":// Move 
                    sysMenu.DoUp();
                    break;
                case "dodown":// Down 
                    sysMenu.DoDown();
                    break;
                case "delete":// Delete 
                    sysMenu.Delete();
                    break;
            }
            // Return 
            return returnVal;
        }

        /// <summary>
        ///  Number Gets the menu according to the user 
        /// </summary>
        /// <returns></returns>
        private string GetMenuByEmpNo()
        {
            string fk_emp = getUTF8ToString("fk_emp");
            string fk_app = getUTF8ToString("fk_app");
            //  Gets the menu , And put it unfolded .
            DataTable dt = BP.GPM.Dev2Interface.DB_Menus(fk_emp, fk_app);
            return CommonDbOperator.GetJsonFromTable(dt);
        }

        /// <summary>
        ///  Get all menus , The options are based on user permission settings 
        /// </summary>
        /// <returns></returns>
        private string GetEmpOfMenusByEmpNo()
        {
            string checkedMenuIds = "";
            string empNO = getUTF8ToString("empNo");
            string parentNo = getUTF8ToString("parentNo");
            string isLoadChild = getUTF8ToString("isLoadChild");
            // Number Gets the menu according to the user 
            //UserMenus userMenus = new UserMenus();

            //QueryObject objWhere = new QueryObject(userMenus);
            //objWhere.AddWhere(UserMenuAttr.FK_Emp, empNO);
            //objWhere.addAnd();
            //objWhere.AddWhere(UserMenuAttr.IsChecked, true);
            //objWhere.DoQuery();
            UserMenu userMenu = new UserMenu();
            DataTable userMenu_dt = userMenu.RunSQLReturnTable("SELECT FK_Menu FROM V_GPM_EmpMenu_GPM WHERE FK_Emp='" + empNO + "' AND IsChecked=1");

            // Number Gets the child node based on parent 
            BP.GPM.Menus menus = new BP.GPM.Menus();
            //menus.RetrieveByAttr("ParentNo", parentNo);
            QueryObject objMenu = new QueryObject(menus);
            objMenu.AddWhere(MenuAttr.ParentNo, parentNo);
            objMenu.addAnd();
            objMenu.AddWhere(MenuAttr.FK_App, "CCFlowBPM");
            objMenu.DoQuery();
            // Finishing the selected item 
            foreach (DataRow row in userMenu_dt.Rows)
            {
                checkedMenuIds += "," + row["FK_Menu"].ToString() + ",";
            }
            // Consolidation is not fully checked 
            string unCheckedMenuIds = "";
            DataTable unCheck_dt = userMenu.RunSQLReturnTable("SELECT FK_Menu FROM V_GPM_EmpMenu_GPM WHERE FK_Emp='" + empNO + "' AND IsChecked=0");
            foreach (DataRow unItem in unCheck_dt.Rows)
            {
                unCheckedMenuIds += "," + unItem["FK_Menu"] + ",";
            }
            // If this is the first load 
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

                    // Node icon 
                    string ico = "icon-" + menu.MenuType;
                    // Judgment is not fully checked 
                    if (unCheckedMenuIds.Contains("," + item.No + ","))
                        ico = "collaboration";

                    appSend.Append(",iconCls:\"");
                    appSend.Append(ico);
                    appSend.Append("\"");

                    // Judge selected 
                    if (checkedMenuIds.Contains("," + item.No + ","))
                        appSend.Append(",\"checked\":true");

                    appSend.Append(",\"children\":");
                    appSend.Append(GetMenusByParentNo(item.No, checkedMenuIds, unCheckedMenuIds, true));
                    appSend.Append("}");
                }
                appSend.Append("]");

                return appSend.ToString();
            }
            // Get the tree node data 
            return GetTreeList(menus, checkedMenuIds, unCheckedMenuIds);
        }

        /// <summary>
        ///  No. Get submenu according to parent 
        /// </summary>
        /// <returns></returns>
        private string GetMenusByParentNo(string parentNo, string checkedMenuIds, string unCheckedMenuIds, bool addChild)
        {
            StringBuilder menuAppend = new StringBuilder();
            // Gets the menu 
            BP.GPM.Menus menus = new BP.GPM.Menus();
            menus.RetrieveByAttr("ParentNo", parentNo);

            // Whether to add the next level 
            if (addChild)
            {
                menuAppend.Append("[");
                foreach (EntityTree item in menus)
                {
                    if (menuAppend.Length > 1) menuAppend.Append(",{"); else menuAppend.Append("{");

                    menuAppend.Append("\"id\":\"" + item.No + "\"");
                    menuAppend.Append(",\"text\":\"" + item.Name + "\"");

                    BP.GPM.Menu menu = item as BP.GPM.Menu;

                    // Node icon 
                    string ico = "icon-" + menu.MenuType;
                    // Judgment is not fully checked 
                    if (unCheckedMenuIds.Contains("," + item.No + ","))
                        ico = "collaboration";

                    menuAppend.Append(",iconCls:\"");
                    menuAppend.Append(ico);
                    menuAppend.Append("\"");

                    // Judge selected 
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
        ///  Get a list of nodes in the tree 
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

                // Node icon 
                string ico = "icon-" + menu.MenuType;
                // Judgment is not fully checked 
                if (unCheckIds.Contains("," + item.No + ","))
                    ico = "collaboration";

                appSend.Append(",iconCls:\"");
                appSend.Append(ico);
                appSend.Append("\"");

                if (checkIds.Contains("," + item.No + ","))
                    appSend.Append(",\"checked\":true");

                // Determine whether there are child nodes 
                BP.GPM.Menus menus = new BP.GPM.Menus();
                menus.RetrieveByAttr("ParentNo", item.No);

                if (menus != null && menus.Count > 0)
                {
                    appSend.Append(",state:\"closed\"");
                    appSend.Append(",\"children\":");
                    appSend.Append("[{");
                    appSend.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\"", item.No + "01", " Loading ..."));
                    appSend.Append("}]");
                }
                appSend.Append("}");
            }
            appSend.Append("]");

            return appSend.ToString();
        }

        /// <summary>
        ///  The entity into a tree 
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
                throw new Exception("@ Not found rootNo=" + rootNo + "的entity.");
            appendMenus.Append("[{");
            appendMenus.Append("\"id\":\"" + rootNo + "\"");
            appendMenus.Append(",\"text\":\"" + root.Name + "\"");

            BP.GPM.Menu menu = ens.GetEntityByKey(rootNo) as BP.GPM.Menu;
            // Adding an icon 
            appendMenus.Append(",iconCls:\"");
            appendMenus.Append("icon-" + menu.MenuType);
            appendMenus.Append("\"");

            //  Increase its children .
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
                    // Adding an icon 
                    appendMenuSb.Append(",iconCls:\"");
                    appendMenuSb.Append("icon-" + menu.MenuType);
                    appendMenuSb.Append("\"");
                }
                //  Increase its children .
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
        ///  According to DataTable Generate Json Tree 
        /// </summary>
        /// <param name="tabel"> Data Sources </param>
        /// <param name="idCol">ID列</param>
        /// <param name="txtCol">Text列</param>
        /// <param name="rela"> Relationship field </param>
        /// <param name="pId">父ID</param>
        ///<returns>easyui tree json Format </returns>
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

                        // Replace the node icon 
                        if (tabel.Columns.Contains("MenuType"))
                        {
                            // Do not expand the directory level 
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