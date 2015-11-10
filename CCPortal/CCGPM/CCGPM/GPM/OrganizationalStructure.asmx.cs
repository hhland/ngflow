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
    /// OrganizationalStructure 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class OS : System.Web.Services.WebService
    {
        #region 属性。
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
        #endregion 属性。


        #region 人员维护
        /// <summary>
        /// 人员修改
        /// </summary>
        /// <param name="empNo">人员编号</param>
        /// <param name="deptNo">部门编号</param>
        /// <param name="attrs">人员属性格式为@字段1=值1@字段2=值2</param>
        /// <param name="stations">该人员在本部门下的岗位集合</param>
        [WebMethod(EnableSession = true)]
        public string Emp_Edit(string empNo, string deptNo, string attrs, string stations)
        {
            // 更新 emp 信息.
            BP.GPM.Emp emp = new BP.GPM.Emp(empNo);
            string[] strs = attrs.Split('^');
            foreach (string str in strs)
            {
                if (string.IsNullOrEmpty(str))
                    continue;
                string[] kv = str.Split('=');
                emp.SetValByKey(kv[0], kv[1]); //设置值.
            }
            emp.Update();

            // 更新 empDept 信息.m
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
                deptEmp.SetValByKey(kv[0], kv[1]); //设置值.
            }
            deptEmp.MyPK = deptEmp.FK_Dept + "_" + deptEmp.FK_Emp;
            deptEmp.Update(); //执行保存.


            //更新岗位对应.
            BP.DA.DBAccess.RunSQL("DELETE Port_DeptEmpStation WHERE FK_Dept='" + deptNo + "' AND FK_Emp='" + emp.No + "' ");
            strs = stations.Split(',');
            foreach (string str in strs)
            {
                if (string.IsNullOrEmpty(str))
                    continue;

                // 插入.
                BP.GPM.DeptEmpStation ds = new BP.GPM.DeptEmpStation();
                ds.FK_Dept = deptNo;
                ds.FK_Emp = emp.No;
                ds.FK_Station = str;
                ds.Insert();
            }
            return "@修改成功";
        }
        /// <summary>
        /// 人员新增
        /// </summary>
        /// <param name="empNo">人员编号</param>
        /// <param name="deptNo">部门编号</param>
        /// <param name="attrs">人员属性格式为@字段1=值1@字段2=值2</param>
        /// <param name="stations">该人员在本部门下的岗位集合</param>
        [WebMethod(EnableSession = true)]
        public string Emp_New(string empNo, string deptNo, string attrs, string stations)
        {
            // 更新 emp 信息.
            BP.GPM.Emp emp = new BP.GPM.Emp();
            emp.No = empNo;
            //if (emp.IsExits)
            //    return "@Error:编号为[" + empNo + "]新信息已经存在";
            if (emp.IsExits == false)
            {
                string[] strs = attrs.Split('^');
                foreach (string str in strs)
                {
                    if (string.IsNullOrEmpty(str))
                        continue;

                    string[] kv = str.Split('=');
                    emp.SetValByKey(kv[0], kv[1]); //设置值.
                }
                emp.Insert();
            }
            //执行编辑.
            this.Emp_Edit(empNo, deptNo, attrs, stations);

            return "@增加成功";
        }
        /// <summary>
        /// 删除人员
        /// </summary>
        /// <param name="empNo">人员编号</param>
        /// <param name="deptNo">部门编号</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public string Emp_Delete(string empNo, string deptNo)
        {
            // 从部门信息里删除它.
            DeptEmp de = new DeptEmp(deptNo, empNo);
            de.Delete();

            //删除这笔数据.
            string sql = "DELETE Port_DeptEmpStation WHERE FK_Emp='" + empNo + "' AND FK_Dept='" + deptNo + "'";
            BP.DA.DBAccess.RunSQL(sql);

            // 检查其它部门里是否有此数据.
            DeptEmps des = new DeptEmps();
            des.Retrieve(DeptEmpAttr.FK_Emp, empNo);
            if (des.Count != 0)
            {
                /*说明其它部门里还有此帐号的信息，所在不能删除主表的数据.*/
            }
            else
            {
                // 其它部门里没有此帐号的信息了，就删除主表的数据。
                BP.GPM.Emp emp = new BP.GPM.Emp();
                emp.No = empNo;
                emp.Delete();
            }
            return "@人员[" + empNo + "]删除成功";
        }
        /// <summary>
        /// 人员与部门进行关联
        /// </summary>
        /// <param name="empNos">人员编号集合</param>
        /// <param name="deptNo">部门编号</param>
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
                    //判断是否存在
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
            return "@关联成功";
        }
        #endregion

        #region 部门维护
        /// <summary>
        /// 编辑部门属性.
        /// </summary>
        /// <param name="deptNo">部门编号</param>
        /// <param name="attrs">属性是@字段名=值</param>
        /// <param name="stations">多个用逗号分开.</param>
        /// <param name="dutys">多个职务用逗号分开.</param>
        [WebMethod(EnableSession = true)]
        public void Dept_Edit(string deptNo, string attrs, string stations, string dutys)
        {
            // 更新dept 信息.
            BP.GPM.Dept dept = new BP.GPM.Dept(deptNo);
            string[] strs = attrs.Split('^');
            foreach (string str in strs)
            {
                if (string.IsNullOrEmpty(str))
                    continue;

                string[] kv = str.Split('=');
                dept.SetValByKey(kv[0], kv[1]); //设置值.
            }
            dept.Update();

            //更新岗位对应.
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

            //更新职务对应.
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
        /// 增加同级部门
        /// </summary>
        /// <param name="currDeptNo">当前部门编号</param>
        /// <param name="attrs">新部门属性</param>
        /// <param name="stations">新部门关联的岗位集合，用逗号分开.</param>
        /// <param name="dutys">新部门关联的职务集合，用逗号分开.</param>
        /// <returns>新建部门的编号</returns>
        [WebMethod(EnableSession = true)]
        public string Dept_CreateSameLevel(string currDeptNo, string attrs, string stations, string dutys)
        {
            //检查部门是否存在
            if (DeptName_Check(attrs))
            {
                return "err:该部门已经存在。";
            }
            BP.GPM.Dept dept = new GPM.Dept(currDeptNo);
            BP.GPM.Dept newDept = dept.DoCreateSameLevelNode() as BP.GPM.Dept;
            newDept.Name = "new dept";
            newDept.FK_DeptType = "";

            //调用编辑部门，并保存它.
            this.Dept_Edit(newDept.No, attrs, stations, dutys);
            return newDept.No;
        }
        /// <summary>
        /// 增加增加下级部门
        /// </summary>
        /// <param name="currDeptNo">当前部门编号</param>
        /// <param name="attrs">新部门属性</param>
        /// <param name="stations">新部门关联的岗位集合，用逗号分开.</param>
        /// <param name="dutys">新部门关联的职务集合，用逗号分开.</param>
        /// <returns>新建部门的编号</returns>
        [WebMethod(EnableSession = true)]
        public string Dept_CreateSubLevel(string currDeptNo, string attrs, string stations, string dutys)
        {
            //检查部门是否存在
            if (DeptName_Check(attrs))
            {
                return "err:该部门已经存在。";
            }
            BP.GPM.Dept dept = new GPM.Dept(currDeptNo);
            BP.GPM.Dept newDept = dept.DoCreateSubNode() as BP.GPM.Dept;
            newDept.Name = "new dept";
            newDept.FK_DeptType = "";

            //调用编辑部门，并保存它.
            this.Dept_Edit(newDept.No, attrs, stations, dutys);
            return newDept.No;
        }
        /// <summary>
        /// 检查根节点
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public string Dept_CheckRootNode()
        {
            Depts rootDepts = new Depts("0");
            if (rootDepts == null || rootDepts.Count == 0)
            {
                Dept rootDept = new Dept();
                rootDept.Name = "集团总部";
                rootDept.FK_DeptType = "01";
                rootDept.ParentNo = "0";
                rootDept.Idx = 0;
                rootDept.Insert();
            }
            return "true";
        }
        /// <summary>
        /// 拖动部门改变节点父编号
        /// </summary>
        /// <param name="currDeptNo">拖动节点</param>
        /// <param name="pDeptNo">拖动节点的父节点</param>
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
        /// 拖动部门进行排序
        /// </summary>
        /// <param name="currDeptNo">拖动节点</param>
        /// <param name="nextDeptNo">关系节点</param>
        /// <param name="nextNodeNos">下面节点的编号集合</param>
        /// <param name="isUpNode">是否拖动节点后的上面节点</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public string Dept_DragSort(string currDeptNo, string nextDeptNo, string nextNodeNos, bool isUpNode)
        {
            try
            {
                Dept currDept = new Dept(currDeptNo);
                Dept nextDept = new Dept(nextDeptNo);
                if (isUpNode)//如果关系节点为上面的节点
                {
                    //设置序号
                    currDept.Idx = nextDept.Idx + 1;
                    currDept.Update();
                }
                else
                {
                    //交换序号
                    currDept.Idx = nextDept.Idx;
                    currDept.Update();
                    //下面节点全部下移
                    int Idx = currDept.Idx;
                    string[] nodeNos = nextNodeNos.Split(',');
                    foreach (string nodeNo in nodeNos)
                    {
                        if (string.IsNullOrEmpty(nodeNo)) continue;
                        Idx++;
                        //下面的节点下移
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
        /// 检查部门是否存在
        /// </summary>
        /// <param name="attrs">部门名称</param>
        /// <returns></returns>
        public bool DeptName_Check(string attrs)
        {
            bool isHave = false;
            string repeatName = System.Configuration.ConfigurationManager.AppSettings["RepeatDeptName"];
            //允许名称重复。
            if (repeatName == null || repeatName == "0")
                return false;

            string[] strs = attrs.Split('^');
            string deptName = "系统管理";
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
        /// 删除部门
        /// </summary>
        /// <param name="deptNo">部门编号</param>
        /// <param name="forceDel">强制删除</param>
        /// <returns>返回删除信息</returns>
        [WebMethod(EnableSession = true)]
        public string Dept_Delete(string deptNo, bool forceDel)
        {
            //获取部门信息
            BP.GPM.Dept dept = new BP.GPM.Dept(deptNo);
            if(dept.ParentNo == "0")
                return "@error:根节点不允许被删除，强制删除将无效.";

            if (forceDel)//如果为强制删除
            {
                //部门与人员
                DeptEmp deptEmp = new DeptEmp();
                deptEmp.Delete(DeptEmpAttr.FK_Dept, deptNo);
                //部门、人员与岗位
                DeptEmpStation deptEmpStation = new DeptEmpStation();
                deptEmpStation.Delete(DeptEmpStationAttr.FK_Dept, deptNo);
                //子部门与人员
                Depts cDepts = new Depts(deptNo);
                foreach (Dept item in cDepts)
                {
                    //部门与人员
                    DeptEmp cdeptEmp = new DeptEmp();
                    cdeptEmp.Delete(DeptEmpAttr.FK_Dept, item.No);
                    //部门、人员与岗位
                    DeptEmpStation cDeptEmpStation = new DeptEmpStation();
                    cDeptEmpStation.Delete(DeptEmpStationAttr.FK_Dept, item.No);
                    //删除子部门
                    item.Delete();
                }
                //执行删除.
                dept.Delete();
                return "@部门[" + dept.Name + "]删除成功.";
            }

            //子部门
            Depts childDept = new Depts(dept.No);
            if (childDept != null && childDept.Count > 0)
                return "@error:当前部门有下级";

            // 检查一下部门下是否有人员.
            string sql = "SELECT * FROM Port_DeptEmp WHERE FK_Dept='" + dept.No + "'";
            if (BP.DA.DBAccess.RunSQLReturnTable(sql).Rows.Count != 0)
                return "@error:当前部门下有人员";

            // 检查一下部门下是否有人员.
            sql = "SELECT * FROM Port_DeptEmpStation WHERE FK_Dept='" + dept.No + "'";
            if (BP.DA.DBAccess.RunSQLReturnTable(sql).Rows.Count != 0)
                return "@error:当前部门下有人员岗位对应信息";

            //执行删除.
            dept.Delete();

            //更新父节点目录
            BP.GPM.Dept deptParent = new Dept(dept.ParentNo);
            if (deptParent != null)
            {
                if (deptParent.HisSubDepts != null && deptParent.HisSubDepts.Count == 0)
                {
                    deptParent.IsDir = false;
                    deptParent.Update("IsDir", false);
                }
            }
            return "@部门[" + dept.Name + "]删除成功.";
        }

        /// <summary>
        /// 根据部门编号获取部门人员
        /// </summary>
        /// <param name="FK_Dept">部门编号</param>
        /// <param name="Dept_Name">部门名称</param>
        /// <returns>返回部门下的人员</returns>
        [WebMethod(EnableSession = true)]
        public string GetEmpsByDeptNo(string FK_Dept, string Dept_Name)
        {
            string sql = "";
            //SQL Server 数据库
            if (BP.Sys.SystemConfig.AppCenterDBType == DA.DBType.MSSQL)
            {
                sql = "SELECT a.No,a.EmpNo,a.Name,a.Email,a.Tel,b.FK_Duty,b.FK_Dept,'" + Dept_Name + "' as DetpName, c.Name as DutyName, b.DutyLevel,b.Leader";
                sql += ",STUFF((SELECT ','+e.Name FROM Port_DeptEmpStation d,Port_Station e ";
                sql += " WHERE d.FK_Station = e.No AND d.FK_Emp=a.no FOR XML PATH('')),1,1,'') AS Stations";
                sql += " FROM Port_Emp a, Port_DeptEmp b , Port_Duty c ";
                sql += " WHERE A.No=B.FK_Emp AND b.FK_Duty=c.No and b.FK_Dept='" + FK_Dept + "'";
                sql += "@ SELECT No,Name FROM Port_Station WHERE No IN (SELECT FK_Station FROM Port_DeptEmpStation WHERE FK_Dept='" + FK_Dept + "')";
            }
            //MySQl数据库
            if (BP.Sys.SystemConfig.AppCenterDBType == DA.DBType.MySQL)
            {
                sql = "SELECT a.No,a.EmpNo,a.Name,a.Email,a.Tel,b.FK_Duty,b.FK_Dept,'" + Dept_Name + "' as DetpName, c.Name as DutyName, b.DutyLevel,b.Leader";
                sql += ",(SELECT group_concat(e.Name separator ',') FROM Port_DeptEmpStation d,Port_Station e ";
                sql += " WHERE d.FK_Station = e.No AND d.FK_Emp=a.no) AS Stations";
                sql += " FROM Port_Emp a, Port_DeptEmp b , Port_Duty c ";
                sql += " WHERE A.No=B.FK_Emp AND b.FK_Duty=c.No and b.FK_Dept='" + FK_Dept + "'";
                sql += "@ SELECT No,Name FROM Port_Station WHERE No IN (SELECT FK_Station FROM Port_DeptEmpStation WHERE FK_Dept='" + FK_Dept + "')";
            }
            //Oracle数据库
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
        /// 将中文转化成拼音.
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

        #region 与数据库交互
        [WebMethod(EnableSession = true)]
        public int RunSQL(string sql)
        {
            return BP.DA.DBAccess.RunSQL(sql);
        }
        /// <summary>
        /// 运行sqls
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
        /// 运行sql返回table.
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
        /// 运行sql返回String.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        [WebMethod]
        public string RunSQLReturnString(string sql)
        {
            return BP.DA.DBAccess.RunSQLReturnString(sql);
        }
        /// <summary>
        /// 运行sql返回String.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        [WebMethod]
        public int RunSQLReturnValInt(string sql)
        {
            return BP.DA.DBAccess.RunSQLReturnValInt(sql);
        }
        /// <summary>
        /// 运行sql返回float.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        [WebMethod]
        public float RunSQLReturnValFloat(string sql)
        {
            return BP.DA.DBAccess.RunSQLReturnValFloat(sql);
        }
        /// <summary>
        /// 运行sql返回table.
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
        /// 获取自定义表
        /// </summary>
        /// <param name="ensName"></param>
        /// <returns></returns>
        [WebMethod]
        public string RequestSFTable(string ensName)
        {
            if (string.IsNullOrEmpty(ensName))
                throw new Exception("@EnsName值为null.");

            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            if (ensName.Contains("."))
            {
                Entities ens = BP.En.ClassFactory.GetEns(ensName);
                if (ens == null)
                    ens = BP.En.ClassFactory.GetEns(ensName);

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
        /// 让admin 登陆
        /// </summary>
        /// <param name="lang">当前的语言</param>
        /// <returns>成功则为空，有异常时返回异常信息</returns>
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
            // 如果admin账户登陆时有错误发生，则返回错误信息
            var result = LetAdminLogin("CH", isLogin);
            if (string.IsNullOrEmpty(result) == false)
            {
                return result;
            }

            switch (doWhat)
            {
                case "GenerFlowTemplete":
                    break;
                case "DeptUp": // 上移
                    BP.GPM.Dept d = new Dept(para1);
                    d.DoUp();
                    break;
                case "DeptDown": // 下移
                    BP.GPM.Dept d2 = new Dept(para1);
                    d2.DoDown();
                    break;
                case "ResetPassword": // 重置密码.
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
