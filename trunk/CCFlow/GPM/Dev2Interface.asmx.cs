using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Data;
using BP.GPM;
using BP.Sys;

namespace BP.Web.GPM
{
    /// <summary>
    /// Dev2Interface  The summary 
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    //  To allow the use of  ASP.NET AJAX  Call this from a script  Web  Service , Please cancel the downlink comment .
    // [System.Web.Script.Services.ScriptService]
    public class Dev2Interface : System.Web.Services.WebService
    {
        /// <summary>
        ///  Check whether the connection to the GPM.
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public bool CheckIsConn()
        {
            string sql = "SELECT * FROM Port_Emp WHERE 1=2";
            DataTable dt = DA.DBAccess.RunSQLReturnTable(sql);
            return true;
        }
        
        #region  Menu API.
        /// <summary>
        /// 按datatable Way , Returns the user menu 
        ///  According to this menu structure , Form their own menu tree .
        /// </summary>
        /// <param name="AppNo"> System Number </param>
        /// <param name="userNo"> User ID </param>
        /// <returns> Menu : Its columns and GPM Database table GPM_EmpMenu Consistency .</returns>
        [WebMethod]
        public DataTable GetUserMenuOfDatatable(string AppNo, string userNo)
        {
            DA.Paras ps = new DA.Paras();
            ps.SQL = "SELECT * FROM V_GPM_EmpMenu WHERE FK_Emp=" + SystemConfig.AppCenterDBVarStr
                + "FK_Emp AND FK_App=" + SystemConfig.AppCenterDBVarStr + "FK_App ORDER BY Idx";
            ps.Add("FK_Emp", userNo);
            ps.Add("FK_App", AppNo);
            return DA.DBAccess.RunSQLReturnTable(ps);
        }
        /// <summary>
        /// 按datatable Way , Returns the user menu 
        ///  According to this menu structure , Form their own menu tree .
        /// </summary>
        /// <param name="userNo"> User ID </param>
        /// <returns> Menu : Its columns and GPM Database table GPM_EmpMenu Consistency .</returns>
        [WebMethod]
        public DataTable GetUserMenuOfDatatableByPNo(string userNo, string parentMenuNO)
        {
            DA.Paras ps = new DA.Paras();
            ps.SQL = "SELECT * FROM V_GPM_EmpMenu WHERE FK_Emp=" + SystemConfig.AppCenterDBVarStr + "FK_Emp AND ParentNo=" + SystemConfig.AppCenterDBVarStr
                + "ParentNo ORDER BY Idx";
            ps.Add("FK_Emp", userNo);
            ps.Add("ParentNo", parentMenuNO);
            return DA.DBAccess.RunSQLReturnTable(ps);
        }
        /// <summary>
        ///  According to the user ID , Number and whether the parent menu display function point Gets the menu item 
        /// </summary>
        /// <param name="userNo"> User Account </param>
        /// <param name="ParentNo"> Menu No. </param>
        /// <param name="isVisibleFunPoint"> Whether to display the function point ;true  Show ,false  Do not show </param>
        /// <returns></returns>
        [WebMethod]
        public static DataTable GetMenu_ChildNode_Datatable_ByMenuNo(string userNo, string ParentNo, bool isVisibleFunPoint)
        {
            DA.Paras ps = new DA.Paras();
            ps.SQL = "SELECT * FROM V_GPM_EmpMenu WHERE FK_Emp=" + SystemConfig.AppCenterDBVarStr + "FK_Emp AND ParentNo=" + SystemConfig.AppCenterDBVarStr
                + "ParentNo  ORDER BY Idx";
            // Function points are not displayed 
            if (!isVisibleFunPoint)
            {
                ps.SQL = "SELECT * FROM V_GPM_EmpMenu WHERE FK_Emp=" + SystemConfig.AppCenterDBVarStr + "FK_Emp AND ParentNo=" + SystemConfig.AppCenterDBVarStr
                               + "ParentNo AND MenuType <> 5 ORDER BY Idx";
            }
            ps.Add("FK_Emp", userNo);
            ps.Add("ParentNo", ParentNo);
            return DA.DBAccess.RunSQLReturnTable(ps);
        }
        /// <summary>
        ///  Check whether you can use this privilege 
        /// </summary>
        /// <param name="userNo"> User ID </param>
        /// <param name="funcNo"> Function No. </param>
        /// <returns> Returns whether you can use this privilege </returns>
        [WebMethod]
        public bool IsCanUseFunc(string userNo, string FK_Menu)
        {
            DA.Paras ps = new DA.Paras();
            ps.SQL = "SELECT * FROM V_GPM_EmpMenu WHERE FK_Emp=" + SystemConfig.AppCenterDBVarStr + "FK_Emp AND FK_Menu=" + SystemConfig.AppCenterDBVarStr + "FK_Menu ";
            ps.Add("FK_Emp", userNo);
            ps.Add("FK_Menu", FK_Menu);
            DataTable dt = DA.DBAccess.RunSQLReturnTable(ps);
            if (dt.Rows.Count == 0)
                return false;
            return true;
        }
        #endregion  Menu API.
    }
}
