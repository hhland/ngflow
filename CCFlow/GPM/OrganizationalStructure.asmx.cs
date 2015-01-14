using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Web.Services;
using BP.Sys;
using BP.En;
using BP.Web;
using Silverlight;
using Silverlight.Controls;
using Silverlight.DataSetConnector;

namespace BP.GPM
{
    /// <summary>
    /// OrganizationalStructure  The summary 
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    //  To allow the use of  ASP.NET AJAX  Call this from a script  Web  Service , Please cancel the downlink comment .
    // [System.Web.Script.Services.ScriptService]
    public class OS : System.Web.Services.WebService
    {
        #region  Property .
        public DataSet TurnXmlDataSet2SLDataSet(DataSet ds)
        {
            DataSet myds = new DataSet();
            foreach (DataTable dtXml in ds.Tables)
            {
                DataTable dt = new DataTable(dtXml.TableName);
                foreach (DataColumn dc in dtXml.Columns)
                {
                    DataColumn mydc = new DataColumn(dc.ColumnName, typeof(string));
                    dt.Columns.Add(mydc);
                }
                foreach (DataRow dr in dtXml.Rows)
                {
                    DataRow drNew = dt.NewRow();
                    foreach (DataColumn dc in dtXml.Columns)
                    {
                        drNew[dc.ColumnName] = dr[dc.ColumnName];
                    }
                    dt.Rows.Add(drNew);
                }
                myds.Tables.Add(dt);
            }
            return myds;
        }
        #endregion  Property .


        #region  Maintenance staff 
        /// <summary>
        ///  To modify the 
        /// </summary>
        /// <param name="empNo"> Personnel Number </param>
        /// <param name="deptNo"> Department number </param>
        /// <param name="attrs"> Staff attribute format @ Field 1=值1@ Field 2=值2</param>
        /// <param name="stations"> The staff at the next set of jobs in this sector </param>
        [WebMethod(EnableSession = true)]
        public string Emp_Edit(string empNo, string deptNo, string attrs, string stations)
        {
            //  Update  emp  Information .
            BP.GPM.Emp emp = new BP.GPM.Emp(empNo);
            string[] strs = attrs.Split('^');
            foreach (string str in strs)
            {
                if (string.IsNullOrEmpty(str))
                    continue;
                string[] kv = str.Split('=');
                emp.SetValByKey(kv[0], kv[1]); // Setting .
            }
            emp.Update();

            //  Update  empDept  Information .m
            BP.GPM.DeptEmp deptEmp = new BP.GPM.DeptEmp();
            deptEmp.MyPK = deptNo + "_" + empNo;
            deptEmp.FK_Emp = empNo;
            deptEmp.FK_Dept = deptNo;

            int i = deptEmp.RetrieveFromDBSources();
            if (i == 0)
                deptEmp.DirectInsert();

            strs = attrs.Split('^');
            foreach (string str in strs)
            {
                if (string.IsNullOrEmpty(str))
                    continue;

                string[] kv = str.Split('=');
                deptEmp.SetValByKey(kv[0], kv[1]); // Setting .
            }
            deptEmp.MyPK = deptEmp.FK_Dept + "_" + deptEmp.FK_Emp;
            deptEmp.Update(); // The save .


            // Update the corresponding positions .
            BP.DA.DBAccess.RunSQL("DELETE Port_DeptEmpStation WHERE FK_Dept='" + deptNo + "' AND FK_Emp='" + emp.No + "' ");
            strs = stations.Split(',');
            foreach (string str in strs)
            {
                if (string.IsNullOrEmpty(str))
                    continue;

                //  Insert .
                BP.GPM.DeptEmpStation ds = new BP.GPM.DeptEmpStation();
                ds.FK_Dept = deptNo;
                ds.FK_Emp = emp.No;
                ds.FK_Station = str;
                ds.Insert();
            }
            return "@ Modified successfully ";
        }
        /// <summary>
        ///  New staff 
        /// </summary>
        /// <param name="empNo"> Personnel Number </param>
        /// <param name="deptNo"> Department number </param>
        /// <param name="attrs"> Staff attribute format @ Field 1=值1@ Field 2=值2</param>
        /// <param name="stations"> The staff at the next set of jobs in this sector </param>
        [WebMethod(EnableSession = true)]
        public string Emp_New(string empNo, string deptNo, string attrs, string stations)
        {
            //  Update  emp  Information .
            BP.GPM.Emp emp = new BP.GPM.Emp();
            emp.No = empNo;
            //if (emp.IsExits)
            //    return "@Error: No. [" + empNo + "] New information already exists ";
            if (emp.IsExits == false)
            {
                string[] strs = attrs.Split('^');
                foreach (string str in strs)
                {
                    if (string.IsNullOrEmpty(str))
                        continue;

                    string[] kv = str.Split('=');
                    emp.SetValByKey(kv[0], kv[1]); // Setting .
                }
                emp.Insert();
            }
            // Executive Editor .
            this.Emp_Edit(empNo, deptNo, attrs, stations);

            return "@ Increase the success ";
        }
        /// <summary>
        ///  Delete staff 
        /// </summary>
        /// <param name="empNo"> Personnel Number </param>
        /// <param name="deptNo"> Department number </param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public string Emp_Delete(string empNo, string deptNo)
        {
            //  Remove it from the department of information in .
            DeptEmp de = new DeptEmp(deptNo, empNo);
            de.Delete();

            // Delete this data .
            string sql = "DELETE Port_DeptEmpStation WHERE FK_Emp='" + empNo + "' AND FK_Dept='" + deptNo + "'";
            BP.DA.DBAccess.RunSQL(sql);

            //  Check whether there is data in other sectors .
            DeptEmps des = new DeptEmps();
            des.Retrieve(DeptEmpAttr.FK_Emp, empNo);
            if (des.Count != 0)
            {
                /* There is also described in other sectors that account information , Where the main table data can not be deleted .*/
            }
            else
            {
                //  There is no other sector of the account information , Delete data in the main table .
                BP.GPM.Emp emp = new BP.GPM.Emp();
                emp.No = empNo;
                emp.Delete();
            }
            return "@ Staff [" + empNo + "] Deleted successfully ";
        }
        /// <summary>
        ///  Personnel associated with the department 
        /// </summary>
        /// <param name="empNos"> Staff numbers set </param>
        /// <param name="deptNo"> Department number </param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public string Dept_Emp_Related(string empNos, string deptNo)
        {
            try
            {
                string[] emps = empNos.Split('^');
                foreach (string empNo in emps)
                {
                    BP.GPM.DeptEmps empDepts = new DeptEmps();
                    QueryObject objInfo = new QueryObject(empDepts);
                    objInfo.AddWhere("MyPK", deptNo + "_" + empNo);
                    objInfo.DoQuery();

                    BP.GPM.Emp emp = new Emp(empNo);
                    emp.FK_Dept = deptNo;
                    emp.Update();

                    BP.GPM.DeptEmp empDept = new DeptEmp();
                    empDept.MyPK = deptNo + "_" + empNo;
                    empDept.FK_Duty = emp.FK_Duty;
                    empDept.FK_Dept = deptNo;
                    empDept.FK_Emp = empNo;
                    // Determine whether there 
                    if (empDepts == null || empDepts.Count == 0)
                        empDept.Insert();
                    else
                        empDept.Update();
                }
            }
            catch (Exception ex)
            {
                return "err:" + ex.Message;
            }
            return "@ Associated with success ";
        }
        #endregion

        #region  Maintenance department 
        /// <summary>
        ///  Edit department properties .
        /// </summary>
        /// <param name="deptNo"> Department number </param>
        /// <param name="attrs"> Property is @ Field name =值</param>
        /// <param name="stations"> Multiple separated by commas .</param>
        /// <param name="dutys"> Various positions separated by commas .</param>
        [WebMethod(EnableSession = true)]
        public void Dept_Edit(string deptNo, string attrs, string stations, string dutys)
        {
            //  Update dept  Information .
            BP.GPM.Dept dept = new BP.GPM.Dept(deptNo);
            string[] strs = attrs.Split('^');
            foreach (string str in strs)
            {
                if (string.IsNullOrEmpty(str))
                    continue;

                string[] kv = str.Split('=');
                dept.SetValByKey(kv[0], kv[1]); // Setting .
            }
            dept.Update();

            // Update the corresponding positions .
            strs = stations.Split(',');
            BP.DA.DBAccess.RunSQL("DELETE Port_DeptStation WHERE FK_Dept='" + dept.No + "'");
            foreach (string str in strs)
            {
                if (string.IsNullOrEmpty(str))
                    continue;

                DeptStation ds = new DeptStation();
                ds.FK_Dept = dept.No;
                ds.FK_Station = str;
                ds.Insert();
            }

            // Update the corresponding duties .
            strs = dutys.Split(',');
            BP.DA.DBAccess.RunSQL("DELETE Port_DeptDuty WHERE FK_Dept='" + dept.No + "'");
            foreach (string str in strs)
            {
                if (string.IsNullOrEmpty(str))
                    continue;

                DeptDuty ds = new DeptDuty();
                ds.FK_Dept = dept.No;
                ds.FK_Duty = str;
                ds.Insert();
            }
        }
        /// <summary>
        ///  Increase at the same level departments 
        /// </summary>
        /// <param name="currDeptNo"> The current department number </param>
        /// <param name="attrs"> The new department property </param>
        /// <param name="stations"> Post new department associated collection , Separated by commas .</param>
        /// <param name="dutys"> Collection of duties associated with the new department , Separated by commas .</param>
        /// <returns> New department number </returns>
        [WebMethod(EnableSession = true)]
        public string Dept_CreateSameLevel(string currDeptNo, string attrs, string stations, string dutys)
        {
            // Check the department if there is 
            if (DeptName_Check(attrs))
            {
                return "err: The department already exists .";
            }
            BP.GPM.Dept dept = new GPM.Dept(currDeptNo);
            BP.GPM.Dept newDept = dept.DoCreateSameLevelNode() as BP.GPM.Dept;
            newDept.Name = "new dept";
            newDept.FK_DeptType = "";

            // Call the editorial department , And save it .
            this.Dept_Edit(newDept.No, attrs, stations, dutys);
            return newDept.No;
        }
        /// <summary>
        ///  Increase Increase subordinate departments 
        /// </summary>
        /// <param name="currDeptNo"> The current department number </param>
        /// <param name="attrs"> The new department property </param>
        /// <param name="stations"> Post new department associated collection , Separated by commas .</param>
        /// <param name="dutys"> Collection of duties associated with the new department , Separated by commas .</param>
        /// <returns> New department number </returns>
        [WebMethod(EnableSession = true)]
        public string Dept_CreateSubLevel(string currDeptNo, string attrs, string stations, string dutys)
        {
            // Check the department if there is 
            if (DeptName_Check(attrs))
            {
                return "err: The department already exists .";
            }
            BP.GPM.Dept dept = new GPM.Dept(currDeptNo);
            BP.GPM.Dept newDept = dept.DoCreateSubNode() as BP.GPM.Dept;
            newDept.Name = "new dept";
            newDept.FK_DeptType = "";

            // Call the editorial department , And save it .
            this.Dept_Edit(newDept.No, attrs, stations, dutys);
            return newDept.No;
        }
        /// <summary>
        ///  Check the root node 
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public string Dept_CheckRootNode()
        {
            Depts rootDepts = new Depts("0");
            if (rootDepts == null || rootDepts.Count == 0)
            {
                Dept rootDept = new Dept();
                rootDept.Name = " Group Headquarters ";
                rootDept.FK_DeptType = "01";
                rootDept.ParentNo = "0";
                rootDept.Idx = 0;
                rootDept.Insert();
            }
            return "true";
        }
        /// <summary>
        ///  Drag the department to change the parent node number 
        /// </summary>
        /// <param name="currDeptNo"> Drag the node </param>
        /// <param name="pDeptNo"> Drag the parent node </param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public string Dept_DragTarget(string currDeptNo, string pDeptNo)
        {
            try
            {
                Dept currDept = new Dept(currDeptNo);
                currDept.ParentNo = pDeptNo;
                currDept.Update();
            }
            catch (Exception ex)
            {
                return "err:" + ex.Message;
            }
            return null;
        }
        /// <summary>
        ///  Drag the department to sort 
        /// </summary>
        /// <param name="currDeptNo"> Drag the node </param>
        /// <param name="nextDeptNo"> Relations node </param>
        /// <param name="nextNodeNos"> The following number of nodes set </param>
        /// <param name="isUpNode"> Whether drag the node after node above </param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public string Dept_DragSort(string currDeptNo, string nextDeptNo, string nextNodeNos, bool isUpNode)
        {
            try
            {
                Dept currDept = new Dept(currDeptNo);
                Dept nextDept = new Dept(nextDeptNo);
                if (isUpNode)// If the relationship between the nodes is above a node 
                {
                    // Set No. 
                    currDept.Idx = nextDept.Idx + 1;
                    currDept.Update();
                }
                else
                {
                    // Exchange numbers 
                    currDept.Idx = nextDept.Idx;
                    currDept.Update();
                    // All nodes down below 
                    int Idx = currDept.Idx;
                    string[] nodeNos = nextNodeNos.Split(',');
                    foreach (string nodeNo in nodeNos)
                    {
                        if (string.IsNullOrEmpty(nodeNo)) continue;
                        Idx++;
                        // The following node down 
                        nextDept = new Dept(nodeNo);
                        nextDept.Idx = Idx;
                        nextDept.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                return "err:" + ex.Message;
            }
            return null;
        }
        /// <summary>
        ///  Check the department if there is 
        /// </summary>
        /// <param name="attrs"> Department name </param>
        /// <returns></returns>
        public bool DeptName_Check(string attrs)
        {
            bool isHave = false;
            string repeatName = System.Configuration.ConfigurationManager.AppSettings["RepeatDeptName"];
            // Allow duplicate names .
            if (repeatName == null || repeatName == "0")
                return false;

            string[] strs = attrs.Split('^');
            string deptName = " System Management ";
            foreach (string str in strs)
            {
                if (string.IsNullOrEmpty(str))
                    continue;

                string[] kv = str.Split('=');
                if (kv[0] == "Name")
                {
                    deptName = kv[1];
                }
            }
            string sql = "SELECT COUNT(Name) NUM FROM Port_Dept WHERE Name='" + deptName + "'";
            int rowCount = RunSQLReturnValInt(sql);
            if (rowCount > 0) isHave = true;

            return isHave;
        }
        /// <summary>
        ///  Delete department 
        /// </summary>
        /// <param name="deptNo"> Department number </param>
        /// <param name="forceDel"> Forcibly remove </param>
        /// <returns> Returns delete information </returns>
        [WebMethod(EnableSession = true)]
        public string Dept_Delete(string deptNo, bool forceDel)
        {
            // Get information department 
            BP.GPM.Dept dept = new BP.GPM.Dept(deptNo);
            if(dept.ParentNo == "0")
                return "@error: The root can not be deleted , Forced to delete the invalid .";

            if (forceDel)// If forced to delete 
            {
                // Departments and staff 
                DeptEmp deptEmp = new DeptEmp();
                deptEmp.Delete(DeptEmpAttr.FK_Dept, deptNo);
                // Department , Personnel and positions 
                DeptEmpStation deptEmpStation = new DeptEmpStation();
                deptEmpStation.Delete(DeptEmpStationAttr.FK_Dept, deptNo);
                // Sub-departments and staff 
                Depts cDepts = new Depts(deptNo);
                foreach (Dept item in cDepts)
                {
                    // Departments and staff 
                    DeptEmp cdeptEmp = new DeptEmp();
                    cdeptEmp.Delete(DeptEmpAttr.FK_Dept, item.No);
                    // Department , Personnel and positions 
                    DeptEmpStation cDeptEmpStation = new DeptEmpStation();
                    cDeptEmpStation.Delete(DeptEmpStationAttr.FK_Dept, item.No);
                    // Delete sub-department 
                    item.Delete();
                }
                // Delete .
                dept.Delete();
                return "@ Department [" + dept.Name + "] Deleted successfully .";
            }

            // Sub-sector 
            Depts childDept = new Depts(dept.No);
            if (childDept != null && childDept.Count > 0)
                return "@error: The current department has lower ";

            //  Are there personnel department under check .
            string sql = "SELECT * FROM Port_DeptEmp WHERE FK_Dept='" + dept.No + "'";
            if (BP.DA.DBAccess.RunSQLReturnTable(sql).Rows.Count != 0)
                return "@error: There are people in the current department ";

            //  Are there personnel department under check .
            sql = "SELECT * FROM Port_DeptEmpStation WHERE FK_Dept='" + dept.No + "'";
            if (BP.DA.DBAccess.RunSQLReturnTable(sql).Rows.Count != 0)
                return "@error: Under the current staff positions corresponding sector information ";

            // Delete .
            dept.Delete();

            // Updated parent directory 
            BP.GPM.Dept deptParent = new Dept(dept.ParentNo);
            if (deptParent != null)
            {
                if (deptParent.HisSubDepts != null && deptParent.HisSubDepts.Count == 0)
                {
                    deptParent.IsDir = false;
                    deptParent.Update("IsDir", false);
                }
            }
            return "@ Department [" + dept.Name + "] Deleted successfully .";
        }

        /// <summary>
        ///  No. Get departments according to department 
        /// </summary>
        /// <param name="FK_Dept"> Department number </param>
        /// <param name="Dept_Name"> Department name </param>
        /// <returns> Returns personnel department under </returns>
        [WebMethod(EnableSession = true)]
        public string GetEmpsByDeptNo(string FK_Dept, string Dept_Name)
        {
            string sql = "";
            //SQL Server  Database 
            if (BP.Sys.SystemConfig.AppCenterDBType == DA.DBType.MSSQL)
            {
                sql = "SELECT a.No,a.EmpNo,a.Name,a.Email,a.Tel,b.FK_Duty,b.FK_Dept,'" + Dept_Name + "' as DetpName, c.Name as DutyName, b.DutyLevel,b.Leader";
                sql += ",STUFF((SELECT ','+e.Name FROM Port_DeptEmpStation d,Port_Station e ";
                sql += " WHERE d.FK_Station = e.No AND d.FK_Emp=a.no FOR XML PATH('')),1,1,'') AS Stations";
                sql += " FROM Port_Emp a, Port_DeptEmp b , Port_Duty c ";
                sql += " WHERE A.No=B.FK_Emp AND b.FK_Duty=c.No and b.FK_Dept='" + FK_Dept + "'";
                sql += "@ SELECT No,Name FROM Port_Station WHERE No IN (SELECT FK_Station FROM Port_DeptEmpStation WHERE FK_Dept='" + FK_Dept + "')";
            }
            //MySQl Database 
            if (BP.Sys.SystemConfig.AppCenterDBType == DA.DBType.MySQL)
            {
                sql = "SELECT a.No,a.EmpNo,a.Name,a.Email,a.Tel,b.FK_Duty,b.FK_Dept,'" + Dept_Name + "' as DetpName, c.Name as DutyName, b.DutyLevel,b.Leader";
                sql += ",(SELECT group_concat(e.Name separator ',') FROM Port_DeptEmpStation d,Port_Station e ";
                sql += " WHERE d.FK_Station = e.No AND d.FK_Emp=a.no) AS Stations";
                sql += " FROM Port_Emp a, Port_DeptEmp b , Port_Duty c ";
                sql += " WHERE A.No=B.FK_Emp AND b.FK_Duty=c.No and b.FK_Dept='" + FK_Dept + "'";
                sql += "@ SELECT No,Name FROM Port_Station WHERE No IN (SELECT FK_Station FROM Port_DeptEmpStation WHERE FK_Dept='" + FK_Dept + "')";
            }
            //Oracle Database 
            if (BP.Sys.SystemConfig.AppCenterDBType == DA.DBType.Oracle)
            {
                sql = "SELECT a.No,a.EmpNo,a.Name,a.Email,a.Tel,b.FK_Duty,b.FK_Dept,'" + Dept_Name + "' as DetpName, c.Name as DutyName, b.DutyLevel,b.Leader";
                sql += ",(SELECT to_char(wmsys.wm_concat(e.Name)) FROM Port_DeptEmpStation d,Port_Station e ";
                sql += " WHERE d.FK_Station = e.No AND d.FK_Emp=a.no) AS Stations";
                sql += " FROM Port_Emp a, Port_DeptEmp b , Port_Duty c ";
                sql += " WHERE A.No=B.FK_Emp AND b.FK_Duty=c.No and b.FK_Dept='" + FK_Dept + "'";
                sql += "@ SELECT No,Name FROM Port_Station WHERE No IN (SELECT FK_Station FROM Port_DeptEmpStation WHERE FK_Dept='" + FK_Dept + "')";
            }
            return RunSQLReturnTableS(sql);
        }
        #endregion

        /// <summary>
        ///  Will be transformed into Chinese Pinyin .
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [WebMethod]
        public string ParseStringToPinyin(string name)
        {
            try
            {
                string s = BP.DA.DataType.ParseStringToPinyin(name);
                if (s.Length > 15)
                    s = BP.DA.DataType.ParseStringToPinyinWordFirst(name);
                s = s.Trim().Replace(" ", "");
                s = s.Trim().Replace(" ", "");
                s = s.Replace(",", "");
                s = s.Replace(".", "");
                return s;
            }
            catch
            {
                return null;
            }
        }

        #region  Interact with the database 
        [WebMethod(EnableSession = true)]
        public int RunSQL(string sql)
        {
            return BP.DA.DBAccess.RunSQL(sql);
        }
        /// <summary>
        ///  Run sqls
        /// </summary>
        /// <param name="sqls"></param>
        /// <returns></returns>
        [WebMethod]
        public int RunSQLs(string sqls)
        {
            if (string.IsNullOrEmpty(sqls))
                return 0;

            int i = 0;
            string[] strs = sqls.Split('@');
            foreach (string str in strs)
            {
                if (string.IsNullOrEmpty(str))
                    continue;
                i += BP.DA.DBAccess.RunSQL(str);
            }
            return i;
        }
        /// <summary>
        ///  Run sql Return table.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        [WebMethod]
        public string RunSQLReturnTable(string sql)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(BP.DA.DBAccess.RunSQLReturnTable(sql));
            return Connector.ToXml(ds);
        }
        /// <summary>
        ///  Run sql Return String.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        [WebMethod]
        public string RunSQLReturnString(string sql)
        {
            return BP.DA.DBAccess.RunSQLReturnString(sql);
        }
        /// <summary>
        ///  Run sql Return String.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        [WebMethod]
        public int RunSQLReturnValInt(string sql)
        {
            return BP.DA.DBAccess.RunSQLReturnValInt(sql);
        }
        /// <summary>
        ///  Run sql Return float.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        [WebMethod]
        public float RunSQLReturnValFloat(string sql)
        {
            return BP.DA.DBAccess.RunSQLReturnValFloat(sql);
        }
        /// <summary>
        ///  Run sql Return table.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        [WebMethod]
        public string RunSQLReturnTableS(string sqls)
        {
            string[] strs = sqls.Split('@');
            DataSet ds = new DataSet();
            int i = 0;
            foreach (string sql in strs)
            {
                if (string.IsNullOrEmpty(sql))
                    continue;

                DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
                dt.TableName = "DT" + i;
                ds.Tables.Add(dt);
                i++;
            }
            return Connector.ToXml(ds);
        }
        /// <summary>
        ///  Get custom table 
        /// </summary>
        /// <param name="ensName"></param>
        /// <returns></returns>
        [WebMethod]
        public string RequestSFTable(string ensName)
        {
            if (string.IsNullOrEmpty(ensName))
                throw new Exception("@EnsName Value null.");

            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            if (ensName.Contains("."))
            {
                Entities ens = ClassFactory.GetEns(ensName);
                if (ens == null)
                    ens = ClassFactory.GetEns(ensName);

                ens.RetrieveAllFromDBSource();
                dt = ens.ToDataTableField();
                ds.Tables.Add(dt);
            }
            else
            {
                string sql = "SELECT No,Name FROM " + ensName;
                ds.Tables.Add(BP.DA.DBAccess.RunSQLReturnTable(sql));
            }
            return Connector.ToXml(ds);
        }
        private string DealPK(string pk, string fromMapdata, string toMapdata)
        {
            if (pk.Contains("*" + fromMapdata))
                return pk.Replace("*" + toMapdata, "*" + toMapdata);
            else
                return pk + "*" + toMapdata;
        }
        public void LetAdminLogin()
        {
            BP.Port.Emp emp = new BP.Port.Emp("admin");
            BP.Web.WebUser.SignInOfGener(emp);
        }
        [WebMethod]
        public string SaveEnum(string enumKey, string enumLab, string cfg)
        {
            SysEnumMain sem = new SysEnumMain();
            sem.No = enumKey;
            if (sem.RetrieveFromDBSources() == 0)
            {
                sem.Name = enumLab;
                sem.CfgVal = cfg;
                sem.Lang = WebUser.SysLang;
                sem.Insert();
            }
            else
            {
                sem.Name = enumLab;
                sem.CfgVal = cfg;
                sem.Lang = WebUser.SysLang;
                sem.Update();
            }

            string[] strs = cfg.Split('@');
            foreach (string str in strs)
            {
                if (string.IsNullOrEmpty(str))
                    continue;
                string[] kvs = str.Split('=');
                SysEnum se = new SysEnum();
                se.EnumKey = enumKey;
                se.Lang = WebUser.SysLang;
                se.IntKey = int.Parse(kvs[0]);
                se.Lab = kvs[1];
                se.Insert();
            }
            return "save ok.";
        }
        /// <summary>
        /// 让admin  Landed 
        /// </summary>
        /// <param name="lang"> Current language </param>
        /// <returns> Success null , Exception information is returned when there is abnormal </returns>
        [WebMethod(EnableSession = true)]
        public string LetAdminLogin(string lang, bool islogin)
        {
            try
            {
                if (islogin)
                {
                    BP.Port.Emp emp = new BP.Port.Emp("admin");
                    BP.Web.WebUser.SignInOfGener(emp);
                }
                return null;
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
            return string.Empty;
        }
        #endregion

        [WebMethod(EnableSession = true)]
        public string Do(string doWhat, string para1, bool isLogin)
        {
            //  In case admin When an error occurs account login , It returns an error message 
            var result = LetAdminLogin("CH", isLogin);
            if (string.IsNullOrEmpty(result) == false)
            {
                return result;
            }

            switch (doWhat)
            {
                case "GenerFlowTemplete":
                    break;
                case "DeptUp": //  Move 
                    BP.GPM.Dept d = new Dept(para1);
                    d.DoUp();
                    break;
                case "DeptDown": //  Down 
                    BP.GPM.Dept d2 = new Dept(para1);
                    d2.DoDown();
                    break;
                case "ResetPassword": //  Reset Password .
                    BP.GPM.Emp emp = new Emp();
                    emp.No = para1;
                    emp.Retrieve();
                    emp.Pass = "123";
                    emp.Update();
                    break;
                default:
                    break;
            }
            return null;
        }
    }
}
