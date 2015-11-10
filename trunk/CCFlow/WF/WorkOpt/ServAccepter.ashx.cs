using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using CCPortal.DA;
using System.Text;
using BP.WF;
using BP.WF.Template;
using BP.Web;
using BP.WF.Template;

namespace CCFlow.WF.WorkOpt
{
    /// <summary>
    /// ServAccepter  The summary 
    ///  Modified by : Zhangqingpeng 
    ///  Modify the description :1. You can distinguish the current department , Post , Character , Select the same name who 
    /// 2. When selecting staff , Initial list of people not loaded , After loading the page and use the display , Avoided due to the binding staff , Problems caused by too many departments or positions of slow response 
    /// 3. The revised , Logic clearer , Facilitate the secondary developers understand 
    /// 4. Page clearer 
    /// 5. Compared with the previous version of the operating more convenient 
    /// </summary>
    public class ServAccepter : IHttpHandler
    {
        #region  Parameters .
        public void OutHtml(string msg)
        {
            // Assembly ajax String format , Return to the calling client 
            MyContext.Response.Charset = "UTF-8";
            MyContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
            MyContext.Response.ContentType = "text/html";
            MyContext.Response.Expires = 0;
            MyContext.Response.Write(msg);
            MyContext.Response.End();
        }
        /// <summary>
        ///  Package on the individual  HTTP  All requests  HTTP  Specific information 
        /// </summary>
        HttpContext MyContext = null;
        public string getUTF8ToString(string param)
        {
            return HttpUtility.UrlDecode(MyContext.Request[param], System.Text.Encoding.UTF8);
        }
        // Post ID
        public string FK_Station
        {
            get
            {
                return getUTF8ToString("stationID");
            }
        }
        // Department ID
        public string FK_Dept
        {
            get
            {
                return getUTF8ToString("deptId");
            }
        }
        // The name of the query 
        public string Name
        {
            get
            {
                return getUTF8ToString("name");
            }
        }
        // To reach the node 
        public string ToNode
        {
            get
            {
                return getUTF8ToString("ToNode");
            }
        }
        // The work ID
        public string WorkID
        {
            get
            {
                return getUTF8ToString("WorkID");
            }
        }
        // The current node ID
        public string FK_Node
        {
            get
            {
                return getUTF8ToString("FK_Node");
            }
        }
        public Selector MySelector = null;
        #endregion  Parameters .
        public void ProcessRequest(HttpContext context)
        {
            MyContext = context;
            context.Response.Charset = "UTF-8";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.ContentType = "text/plain";
            string strResponse = String.Empty;
            var page = Convert.ToInt32(context.Request["page"]);  // Current page 
            var rows = Convert.ToInt32(context.Request["rows"]);   // Biography current page 
            var strtype = context.Request["type"];
            switch (strtype)
            {
                case "2":
                    // Get departments tree 
                    strResponse = GetDeptTree();
                    break;
                case "3":
                    // Get job tree 
                    strResponse = GetStationTree();
                    break;
                default:
                    // Get a list of staff 
                    strResponse = GetEmp(strtype,page,rows);
                    break;

            }
            context.Response.Write(strResponse);
        }
        public string GetEmp(string type,int page,int rows)
        {
            string val = "";
            // The first load will not load list of people , Here judgment , Avoid throwing null Abnormal values 
            if (!String.IsNullOrEmpty(this.ToNode))
            {
                // Check out binding rules to reach the node 
                MySelector = new Selector(int.Parse(this.ToNode));
                
                // According to the rules of the binding node properties here , Binding Data 
                switch (MySelector.SelectorModel)
                {
                    // Inquiries by post 
                    case SelectorModel.Station:
                        // Click the job list 
                        if (type == "4")
                            val = GetEmpByStation("Station", this.FK_Station, page, rows);
                        // Click department list 
                        if (type == "5")
                            val = GetEmpByDept("Station", this.FK_Dept, page, rows);
                        // Click to staff inquiries 
                        if (type == "6")
                            val = GetEmpByEmp("Station", this.Name, page, rows);
                        break;
                    //按SQL Sentences 
                    case SelectorModel.SQL:
                        if (type == "4")
                            val = BindBySQL("Station", this.FK_Station, page, rows);
                        if (type == "5")
                            val = BindBySQL("Dept", this.FK_Dept, page, rows);
                        if (type == "6")
                            val = BindBySQL("Emp", this.Name, page, rows);
                        break;
                    // Query by sector 
                    case SelectorModel.Dept:
                        if (type == "4")
                            val = GetEmpByStation("Dept", this.FK_Station, page, rows);
                        if (type == "5")
                            val = GetEmpByDept("Dept", this.FK_Dept, page, rows);
                        if (type == "6")
                            val = GetEmpByEmp("Dept", this.Name, page, rows);
                        break;
                    // Press to accept people query 
                    case SelectorModel.Emp:
                        if (type == "4")
                            val = GetEmpByStation("Emp", this.FK_Station, page, rows);
                        if (type == "5")
                            val = GetEmpByDept("Emp", this.FK_Dept, page, rows);
                        if (type == "6")
                            val = GetEmpByEmp("Emp", this.Name, page, rows);
                        break;
                    //按URL Inquiry 
                    case SelectorModel.Url:
                        if (MySelector.SelectorP1.Contains("?"))
                            this.MyContext.Response.Redirect(MySelector.SelectorP1 + "&WorkID=" + this.WorkID + "&FK_Node=" + this.FK_Node, true);
                        else
                            this.MyContext.Response.Redirect(MySelector.SelectorP1 + "?WorkID=" + this.WorkID + "&FK_Node=" + this.FK_Node, true);
                        return "";
                    default:
                        break;
                }
            }
            return val;
        }
        /// <summary>
        ///  Get departments tree 
        /// </summary>
        /// <returns></returns>
        public string GetDeptTree()
        {
            string sql = "select No,Name,ParentNo,'1' IsParent  from Port_Dept where Name not in('null','',' ')";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            string treeJson = GetTreeJsonByTable(dt, "No", "Name", "ParentNo", "0");
            return treeJson;
        }
        /// <summary>
        ///  Get job tree 
        /// </summary>
        /// <returns></returns>
        public string GetStationTree()
        {
            string sql = "select No,Name,'0' ParentNo from Port_Station";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            string treeJson = GetTreeJsonByTable(dt, "No", "Name", "ParentNo", "0");
            return treeJson;
        }
        #region  According to the rules of the node attribute binding inquiry staff list 
        /// <summary>
        ///  Bound by sector inquiry 
        /// </summary>
        /// <param name="type"> Query conditions （ Post , Department , Staff ）</param>
        /// <param name="val"> Current job , Department number </param>
        /// <param name="page"> Pages </param>
        /// <param name="rows"> The number of lines per page </param>
        /// <returns></returns>
        public string GetEmpByDept(string type, string val, int page, int rows)
        {
            string sql = "";
            string SqlCount = "";
            if (page == 0)
                page = 1;
            // Click positions when 
            if (type == "Station")
            {
                sql = "select distinct top " + rows + " Port_Emp.No,Port_Emp.Name,port_dept.Name as DeptName from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no "
                    + "where port_empstation.FK_Station "
                    + " in(select FK_Station from port_empstation where FK_Station in(SELECT FK_STATION FROM WF_NodeStation WHERE FK_Node='" + this.ToNode + "')) and Port_Emp.No!='admin' and Port_Emp.FK_Dept='" + val + "' and Port_Emp.No not in("
                    + "select distinct top " + (page - 1) * rows + " Port_Emp.No from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no "
                    + "where port_empstation.FK_Station "
                    + " in(select FK_Station from port_empstation where FK_Station in(SELECT FK_STATION FROM WF_NodeStation WHERE FK_Node='" + this.ToNode + "')) and Port_Emp.No!='admin' and Port_Emp.FK_Dept='" + val + "'  order by Port_Emp.No) order by Port_Emp.No";
                SqlCount = "select distinct Port_Emp.No,Port_Emp.Name,port_dept.Name as DeptName from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no "
                    + "where port_empstation.FK_Station "
                    + " in(select FK_Station from port_empstation where FK_Station in(SELECT FK_STATION FROM WF_NodeStation WHERE FK_Node='" + this.ToNode + "')) and Port_Emp.No!='admin' and Port_Emp.FK_Dept='" + val + "'";
            }
            // Click department when 
            if (type == "Dept")
            {
                sql = "select distinct top " + rows + " Port_emp.No,Port_emp.Name,port_dept.Name as DeptName from Port_emp left join Port_dept "
                    + "on Port_emp.FK_Dept=Port_dept.no where Port_emp.No in"
                    + "(select No from Port_emp where FK_Dept in(SELECT FK_Dept FROM WF_NodeDept where FK_Node='" + this.ToNode + "')) and Port_Emp.No!='admin' and Port_emp.FK_Dept='" + val + "' and Port_Emp.No not in("
                    + "select distinct top " + (page - 1) * rows + " Port_emp.No from Port_emp left join Port_dept "
                    + "on Port_emp.FK_Dept=Port_dept.no where Port_emp.No in"
                    + "(select No from Port_emp where FK_Dept in(SELECT FK_Dept FROM WF_NodeDept where FK_Node='" + this.ToNode + "')) and Port_Emp.No!='admin' and Port_emp.FK_Dept='" + val + "' order by Port_Emp.No) order by Port_Emp.No";
                SqlCount = "select distinct Port_emp.No,Port_emp.Name,port_dept.Name as DeptName from Port_emp left join Port_dept "
                    + "on Port_emp.FK_Dept=Port_dept.no where Port_emp.No in"
                    + "(select No from Port_emp where FK_Dept in(SELECT FK_Dept FROM WF_NodeDept where FK_Node='" + this.ToNode + "')) and Port_Emp.No!='admin' and Port_emp.FK_Dept='" + val + "'";
            }
            // Click to staff inquiries 
            if (type == "Emp")
            {
                sql = "select distinct top " + rows + " Port_Emp.No,Port_Emp.Name,port_dept.Name as DeptName from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no "
                    + "where Port_Emp.No "
                    + " in(SELECT FK_Emp FROM WF_NodeEmp where FK_Node='" + this.ToNode + "') and Port_Emp.No!='admin' and Port_dept.No='" + val + "' and Port_Emp.No not in("
                    + "select distinct top " + (page - 1) * rows + " Port_Emp.No from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no "
                    + "where Port_Emp.No "
                    + " in(SELECT FK_Emp FROM WF_NodeEmp where FK_Node='" + this.ToNode + "') and Port_Emp.No!='admin' and Port_dept.No='" + val + "' order by Port_Emp.No) order by Port_Emp.No";
                SqlCount = "select distinct Port_Emp.No,Port_Emp.Name,port_dept.Name as DeptName from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no "
                    + "where Port_Emp.No "
                    + " in(SELECT FK_Emp FROM WF_NodeEmp where FK_Node='" + this.ToNode + "') and Port_Emp.No!='admin' and Port_dept.No='" + val + "'";
            }
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            DataTable dte = BP.DA.DBAccess.RunSQLReturnTable(SqlCount);
            string gridJson = GetEmpJson(dt, dte.Rows.Count);
            return gridJson;
        }
        /// <summary>
        ///  Press officers binding inquiry 
        /// </summary>
        /// <param name="type"> Query conditions （ Post , Department , Staff ）</param>
        /// <param name="val"> Current job , Department number </param>
        /// <param name="page"> Pages </param>
        /// <param name="rows"> The number of lines per page </param>
        /// <returns></returns>
        public string GetEmpByEmp(string type, string val, int page, int rows)
        {
            string sql = "";
            string sqlCount = "";
            if (page == 0)
                page=1;
            if (String.IsNullOrEmpty(val))
            {
                // Click positions when 
                if (type == "Station")
                {
                    sql = "select distinct top " + rows + " Port_Emp.No,Port_Emp.Name,port_dept.Name as DeptName from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no where Port_Emp.No!='admin' and Port_Emp.No in"
                    + "(select distinct FK_Emp from port_empstation where FK_Station in"
                    + "(select FK_STATION from WF_NodeStation where FK_Node='" + this.ToNode + "')) and Port_Emp.No not in("
                    + "select distinct top " + (page - 1) * rows + " Port_Emp.No from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no where Port_Emp.No in"
                    + "(select distinct FK_Emp from port_empstation where FK_Station in (select FK_STATION from WF_NodeStation where FK_Node='" + this.ToNode + "')) and Port_Emp.No!='admin' order by Port_Emp.No) order by Port_Emp.No";
                    sqlCount = "select distinct Port_Emp.No,Port_Emp.Name,port_dept.Name as DeptName from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no where Port_Emp.No in"
                    + "(select distinct FK_Emp from port_empstation where FK_Station in"
                    + "(select FK_STATION from WF_NodeStation where FK_Node='" + this.ToNode + "') and Port_Emp.No!='admin')";
                }
                // Click department when 
                if (type == "Dept")
                {
                    sql = "select distinct top " + rows + " Port_emp.No,Port_emp.Name,port_dept.Name as DeptName from Port_emp left join Port_dept "
                    + "on Port_emp.FK_Dept=Port_dept.no where Port_emp.No in"
                    + "(select distinct No from Port_emp where FK_Dept in(SELECT FK_Dept FROM WF_NodeDept where FK_Node='" + this.ToNode + "')) and Port_Emp.No!='admin' and Port_emp.No not in(select distinct top " + (page - 1) * rows + " Port_emp.No from Port_emp left join Port_dept "
                    + "on Port_emp.FK_Dept=Port_dept.no where Port_emp.No in(select distinct No from Port_emp where FK_Dept in(SELECT FK_Dept FROM WF_NodeDept where FK_Node='" + this.ToNode + "')) and Port_Emp.No!='admin' order by Port_Emp.No) order by Port_Emp.No";
                    sqlCount = "select distinct Port_emp.No,Port_emp.Name,port_dept.Name as DeptName from Port_emp left join Port_dept "
                    + "on Port_emp.FK_Dept=Port_dept.no where Port_emp.No in"
                    + "(select distinct No from Port_emp where FK_Dept in(SELECT FK_Dept FROM WF_NodeDept where FK_Node='" + this.ToNode + "') and Port_Emp.No!='admin')";
                }
                // Click to staff inquiries 
                if (type == "Emp")
                {
                    sql = "select distinct top " + rows + " Port_emp.No,Port_emp.Name,port_dept.Name as DeptName from Port_emp left join Port_dept "
                        + "on Port_emp.FK_Dept=Port_dept.no where Port_emp.No in"
                        + "(select distinct FK_EMP from WF_NodeEmp where FK_Node='" + this.ToNode + "') and Port_Emp.No!='admin' and Port_emp.No not in("
                    + "select distinct top " + (page - 1) * rows + " Port_emp.No from Port_emp left join Port_dept on Port_emp.FK_Dept=Port_dept.no"
                    + " where Port_emp.No in(select distinct FK_EMP from WF_NodeEmp where FK_Node='" + this.ToNode + "') and Port_Emp.No!='admin' order by Port_Emp.No) order by Port_Emp.No";
                    sqlCount = "select distinct Port_emp.No,Port_emp.Name,port_dept.Name as DeptName from Port_emp left join Port_dept "
                        + "on Port_emp.FK_Dept=Port_dept.no where Port_emp.No in"
                        + "(select distinct FK_EMP from WF_NodeEmp where FK_Node='" + this.ToNode + "' and Port_Emp.No!='admin')";
                }
            }
            else
            {
                string strSql = "select No from Port_Emp where Name like'%" + Name + "%'";
                DataTable dtl = BP.DA.DBAccess.RunSQLReturnTable(strSql);
                string emps = "";
                foreach (DataRow item in dtl.Rows)
                {
                    emps += item["No"].ToString() + "','";
                }
                // Click positions when 
                if (type == "Station")
                {
                    sql = "select distinct top " + rows + " Port_Emp.No,Port_Emp.Name,port_dept.Name as DeptName from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no where Port_Emp.No in"
                    + "(select distinct FK_Emp from port_empstation where FK_Station in"
                    + "(select FK_STATION from WF_NodeStation where FK_Node='" + this.ToNode + "')) and Port_Emp.No!='admin' and Port_Emp.No in('" + emps + "') and Port_Emp.No not in("
                    + "select distinct top " + (page - 1) * rows + " Port_Emp.No from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no where Port_Emp.No in"
                    + "(select distinct FK_Emp from port_empstation where FK_Station in (select FK_STATION from WF_NodeStation where FK_Node='" + this.ToNode + "')) and Port_Emp.No in('" + emps + "') and Port_Emp.No!='admin' order by Port_Emp.No) order by Port_Emp.No";
                    sqlCount = "select distinct Port_Emp.No,Port_Emp.Name,port_dept.Name as DeptName from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no where Port_Emp.No in"
                    + "(select distinct FK_Emp from port_empstation where FK_Station in"
                    + "(select FK_STATION from WF_NodeStation where FK_Node='" + this.ToNode + "') and Port_Emp.No!='admin')";
                }
                // Click department when 
                if (type == "Dept")
                {
                    sql = "select distinct top " + rows + " Port_emp.No,Port_emp.Name,port_dept.Name as DeptName from Port_emp left join Port_dept "
                    + "on Port_emp.FK_Dept=Port_dept.no where Port_emp.No in"
                    + "(select distinct No from Port_emp where FK_Dept in(SELECT FK_Dept FROM WF_NodeDept where FK_Node='" + this.ToNode + "')) and Port_Emp.No in('" + emps + "') and Port_Emp.No!='admin' and Port_emp.No not in(select distinct top " + (page - 1) * rows + " Port_emp.No from Port_emp left join Port_dept "
                    + "on Port_emp.FK_Dept=Port_dept.no where Port_emp.No in(select distinct No from Port_emp where FK_Dept in(SELECT FK_Dept FROM WF_NodeDept where FK_Node='" + this.ToNode + "')) and Port_Emp.No!='admin' and Port_Emp.No in('" + emps + "') order by Port_Emp.No) order by Port_Emp.No";
                    sqlCount = "select distinct Port_emp.No,Port_emp.Name,port_dept.Name as DeptName from Port_emp left join Port_dept "
                    + "on Port_emp.FK_Dept=Port_dept.no where Port_emp.No in"
                    + "(select distinct No from Port_emp where FK_Dept in(SELECT FK_Dept FROM WF_NodeDept where FK_Node='" + this.ToNode + "') and Port_Emp.No!='admin')";
                }
                // Click to staff inquiries 
                if (type == "Emp")
                {
                    sql = "select distinct top " + rows + " Port_emp.No,Port_emp.Name,port_dept.Name as DeptName from Port_emp left join Port_dept "
                            + "on Port_emp.FK_Dept=Port_dept.no where Port_emp.No in"
                            + "(select distinct FK_EMP from WF_NodeEmp where FK_Node='" + this.ToNode + "') and Port_Emp.No!='admin' and Port_Emp.No in('" + emps + "') and Port_emp.No not in("
                        + "select distinct top " + (page - 1) * rows + " Port_emp.No from Port_emp left join Port_dept on Port_emp.FK_Dept=Port_dept.no"
                        + " where Port_emp.No in(select distinct FK_EMP from WF_NodeEmp where FK_Node='" + this.ToNode + "') and Port_Emp.No!='admin' and Port_Emp.No in('" + emps + "') order by Port_Emp.No) order by Port_Emp.No";
                    sqlCount = "select distinct Port_emp.No,Port_emp.Name,port_dept.Name as DeptName from Port_emp left join Port_dept "
                            + "on Port_emp.FK_Dept=Port_dept.no where Port_emp.No in"
                            + "(select distinct FK_EMP from WF_NodeEmp where FK_Node='" + this.ToNode + "') and Port_Emp.No!='admin'";
                }
            }   
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            DataTable dte = BP.DA.DBAccess.RunSQLReturnTable(sqlCount);
            string gridJson = GetEmpJson(dt, dte.Rows.Count);
            return gridJson;
        }
        /// <summary>
        ///  Binding inquiry by post 
        /// </summary>
        /// <param name="type"> Query conditions （ Post , Department , Staff ）</param>
        /// <param name="val"> Current job , Department number </param>
        /// <param name="page"> Pages </param>
        /// <param name="rows"> The number of lines per page </param>
        /// <returns></returns>
        public string GetEmpByStation(string type, string val, int page, int rows)
        {
            string sql = "";
            string sqlCount = "";
            if (page == 0)
                page = 1;
            // If you click on the job when 
            if (type == "Station")
            {
                sql = "select distinct top " + rows + " Port_Emp.No,Port_Emp.Name,port_dept.Name as DeptName from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no "
                    + "where port_empstation.FK_Station='" + val + "' and port_empstation.FK_Station "
                    + " in(select FK_Station from port_empstation where FK_Station in(SELECT FK_STATION FROM WF_NodeStation WHERE FK_Node='" + this.ToNode + "')) and Port_Emp.No!='admin' and Port_Emp.No not in("
                    + "select distinct top " + (page - 1) * rows + " Port_Emp.No from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no "
                    + "where port_empstation.FK_Station='" + val + "' and port_empstation.FK_Station "
                    + " in(select FK_Station from port_empstation where FK_Station in(SELECT FK_STATION FROM WF_NodeStation WHERE FK_Node='" + this.ToNode + "')) and Port_Emp.No!='admin' order by Port_Emp.No) order by Port_Emp.No";
                sqlCount = "select distinct Port_Emp.No,Port_Emp.Name,port_dept.Name as DeptName from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no "
                    + "where port_empstation.FK_Station='" + val + "' and port_empstation.FK_Station "
                    + " in(select FK_Station from port_empstation where FK_Station in(SELECT FK_STATION FROM WF_NodeStation WHERE FK_Node='" + this.ToNode + "')) and Port_Emp.No!='admin'";
            }
            // If you click on the department when 
            if (type == "Dept")
            {
                sql = "select distinct top " + rows + " Port_Emp.No,Port_Emp.Name,port_dept.Name as DeptName from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no "
                    + "where Port_Emp.No "
                    + " in(select No from Port_emp where FK_Dept in(SELECT FK_Dept FROM WF_NodeDept where FK_Node='" + this.ToNode + "')) and Port_Emp.No!='admin' and port_empstation.FK_Station='" + val + "' and Port_Emp.No not in("
                    + "select distinct top " + (page - 1) * rows + " Port_Emp.No from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no "
                    + "where Port_Emp.No "
                    + " in(select No from Port_emp where FK_Dept in(SELECT FK_Dept FROM WF_NodeDept where FK_Node='" + this.ToNode + "')) and Port_Emp.No!='admin' and port_empstation.FK_Station='" + val + "' order by Port_Emp.No) order by Port_Emp.No";
                sqlCount="select distinct Port_Emp.No,Port_Emp.Name,port_dept.Name as DeptName from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no "
                    + "where Port_Emp.No "
                    + " in(select No from Port_emp where FK_Dept in(SELECT FK_Dept FROM WF_NodeDept where FK_Node='" + this.ToNode + "')) and Port_Emp.No!='admin' and port_empstation.FK_Station='" + val + "'";
            }
            // If you click on staff query 
            if (type == "Emp")
            {
                sql = "select distinct top " + rows + " Port_Emp.No,Port_Emp.Name,port_dept.Name as DeptName from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no "
                    + "where Port_Emp.No "
                    + " in(SELECT FK_Emp FROM WF_NodeEmp where FK_Node='" + this.ToNode + "') and Port_Emp.No!='admin' and port_empstation.FK_Station='" + val + "' and Port_Emp.No not in("
                    + "select distinct top " + (page - 1) * rows + " Port_Emp.No from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no "
                    + "where Port_Emp.No "
                    + " in(SELECT FK_Emp FROM WF_NodeEmp where FK_Node='" + this.ToNode + "') and Port_Emp.No!='admin' and port_empstation.FK_Station='" + val + "' order by Port_Emp.No) order by Port_Emp.No";
                sqlCount = "select distinct Port_Emp.No,Port_Emp.Name,port_dept.Name as DeptName from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no "
                    + "where Port_Emp.No "
                    + " in(SELECT FK_Emp FROM WF_NodeEmp where FK_Node='" + this.ToNode + "') and Port_Emp.No!='admin' and port_empstation.FK_Station='" + val + "'";
            }
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            DataTable dte = BP.DA.DBAccess.RunSQLReturnTable(sqlCount);
            string gridJson = GetEmpJson(dt, dte.Rows.Count);
            return gridJson;
        }
        /// <summary>
        ///  By binding SQL Sentences 
        /// </summary>
        /// <param name="type"> Query conditions （ Post , Department , Staff ）</param>
        /// <param name="val"> Current job , Names of the departments or personnel numbers </param>
        /// <returns></returns>
        public string BindBySQL(string type, string val, int page, int rows)
        {
            string BindBySQL = MySelector.SelectorP1;
            BindBySQL = BindBySQL.Replace("@WebUser.No", WebUser.No);
            BindBySQL = BindBySQL.Replace("@WebUser.Name", WebUser.Name);
            BindBySQL = BindBySQL.Replace("@WebUser.FK_Dept", WebUser.FK_Dept);
            string sql = "";
            string sqlCount = "";
            if (page == 0)
                page = 1;
            // If you click on the job when 
            if (type == "Station")
            {
                sql = "select distinct top " + rows + " Port_Emp.No,Port_Emp.Name,port_dept.Name as DeptName from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no "
                    + "where port_empstation.FK_Station='" + val + "' and Port_Emp.No in(" + BindBySQL + ") and Port_Emp.No!='admin' and Port_Emp.No not in("
                    + "select distinct top " + (page - 1) * rows + " Port_Emp.No from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no "
                    + "where port_empstation.FK_Station='" + val + "' and Port_Emp.No in(" + BindBySQL + ") and Port_Emp.No!='admin' order by Port_Emp.No) order by Port_Emp.No";
                sqlCount = "select distinct Port_Emp.No,Port_Emp.Name,port_dept.Name as DeptName from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no "
                    + "where port_empstation.FK_Station='" + val + "' and Port_Emp.No in(" + BindBySQL + ") and Port_Emp.No!='admin'";
            }
            // If you click on the department when 
            if (type == "Dept")
            {
                sql = "select distinct top " + rows + " Port_Emp.No,Port_Emp.Name,port_dept.Name as DeptName from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no "
                    + "where Port_Dept.No='" + val + "' and Port_Emp.No!='admin' and Port_Emp.No in(" + BindBySQL + ") and Port_Emp.No!='admin' and Port_Emp.No not in("
                    + "select distinct top " + (page - 1) * rows + " Port_Emp.No from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no "
                    + "where Port_Dept.No='" + val + "' and Port_Emp.No!='admin' and Port_Emp.No in(" + BindBySQL + ") and Port_Emp.No!='admin' order by Port_Emp.No) order by Port_Emp.No";
                sqlCount = "select distinct Port_Emp.No,Port_Emp.Name,port_dept.Name as DeptName from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no "
                    + "where Port_Dept.No='" + val + "' and Port_Emp.No!='admin' and Port_Emp.No in(" + BindBySQL + ") and Port_Emp.No!='admin'";
            }
            // If you click on staff query 
            if (type == "Emp")
            {
                sql = "select distinct top " + rows + " Port_Emp.No,Port_Emp.Name,port_dept.Name as DeptName from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no "
                    + "where Port_Emp.No!='admin' and Port_Emp.No in(" + BindBySQL + ") and Port_Emp.No not in("
                    + "select distinct top " + (page - 1) * rows + " Port_Emp.No from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no "
                    + "where Port_Emp.No!='admin' and Port_Emp.No in(" + BindBySQL + ") order by Port_Emp.No) order by Port_Emp.No";
                sqlCount = "select distinct Port_Emp.No,Port_Emp.Name,port_dept.Name as DeptName from Port_Emp left join port_empstation "
                    + " on port_empstation.FK_Emp=Port_Emp.No left join Port_dept on Port_emp.FK_Dept=Port_dept.no "
                    + "where Port_Emp.No!='admin' and Port_Emp.No in(" + BindBySQL + ")";

            }

            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            DataTable dte = BP.DA.DBAccess.RunSQLReturnTable(sqlCount);
            string gridJson = GetEmpJson(dt,dte.Rows.Count);
            return gridJson;
        }
        #endregion
        #region  Generate json General Methods 
        /// <summary>
        ///  Generate Json General Methods 
        /// </summary>
        /// <param name="dt"> Data Sources </param>
        /// <returns></returns>
        public string GetEmpJson( DataTable dt,int count)
        {
            int cnt = count;

            StringBuilder sb = new StringBuilder();
            sb.Append("{\"total\":" + cnt.ToString() + ",\"rows\":[");
            foreach (DataRow row in dt.Rows)
            {
                sb.Append("{\"Name\":\"" + row["Name"] + "\",\"DepartName\":\"" + row["DeptName"] + "\",\"UserName\":\"" + row["No"] + "\"");
                sb.Append("},");
            }

            sb = (cnt > 0) ? sb.Remove(sb.Length - 1, 1) : sb;

            sb.Append("]}");

            return sb.ToString();
        }
        #endregion
        #region  According to DataTable Generate EasyUI Tree Json Tree 
        StringBuilder result = new StringBuilder();
        StringBuilder sb = new StringBuilder();
        /// <summary>  
        ///  According to DataTable Generate EasyUI Tree Json Tree   
        /// </summary>  
        /// <param name="tabel"> Data Sources </param>  
        /// <param name="idCol">ID列</param>  
        /// <param name="txtCol">Text列</param>  
        /// <param name="url"> Node Url</param>  
        /// <param name="rela"> Relationship field </param>  
        /// <param name="pId">父ID</param>  
        private string GetTreeJsonByTable(DataTable tabel, string idCol, string txtCol, string rela, object pId)
        {
            result.Append(sb.ToString());
            sb.Clear();

            if (tabel.Rows.Count > 0)
            {
                sb.Append("[");
                string filer = string.Format("{0}='{1}'", rela, pId);
                DataRow[] rows = tabel.Select(filer);
                if (rows.Length > 0)
                {
                    try
                    {
                        foreach (DataRow row in rows)
                        {
                            sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\"");
                            if (tabel.Select(string.Format("{0}='{1}'", rela, row[idCol])).Length > 0)
                            {
                                if (Convert.ToString(pId) == "0") { sb.Append(",\"state\":\"open\",\"children\":"); }
                                // Click to expand 
                                else
                                    sb.Append(",\"state\":\"closed\",\"children\":");
                                GetTreeJsonByTable(tabel, idCol, txtCol, rela, row[idCol]);
                                result.Append(sb.ToString());
                                sb.Clear();
                            }
                            result.Append(sb.ToString());
                            sb.Clear();
                            sb.Append("},");
                        }
                        sb = sb.Remove(sb.Length - 1, 1);
                        sb.Append("]");
                        result.Append(sb.ToString());
                        sb.Clear();
                    }
                    catch (Exception ex)
                    {
                        return ex.ToString();
                    }

                }
            }
            return result.ToString();
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}