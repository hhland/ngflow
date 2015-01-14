using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.Services;
using BP.DA;
using BP.En;
using BP.GPM;
using BP.Sys;
using BP.Web;
using BP.WF;
using BP.WF.Template;
using FtpSupport;
using Silverlight.DataSetConnector;

namespace CCFlow.WF.Admin.XAP
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    //[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class WSDesigner : System.Web.Services.WebService
    {
        #region  Organizational structure and related methods .

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
                    BP.GPM.DeptEmps empDepts = new BP.GPM.DeptEmps();
                    QueryObject objInfo = new QueryObject(empDepts);
                    objInfo.AddWhere("MyPK", deptNo + "_" + empNo);
                    objInfo.DoQuery();

                    BP.GPM.Emp emp = new BP.GPM.Emp(empNo);
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

                BP.GPM.DeptStation ds = new BP.GPM.DeptStation();
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
            BP.GPM.Dept dept = new BP.GPM.Dept(currDeptNo);
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
            BP.GPM.Dept dept = new BP.GPM.Dept(currDeptNo);
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
           OSModel model= (OSModel)Enum.Parse(typeof(OSModel), this.GetConfig("OSModel"), true);

            if (model == OSModel.BPM)
            {
                BP.GPM.Depts rootDepts = new BP.GPM.Depts("0");
                if (rootDepts == null || rootDepts.Count == 0)
                {
                    BP.GPM.Dept rootDept = new BP.GPM.Dept();
                    rootDept.Name = " Group Headquarters ";
                    rootDept.FK_DeptType = "01";
                    rootDept.ParentNo = "0";
                    rootDept.Idx = 0;
                    rootDept.Insert();
                }
            }
            else if (model == OSModel.WorkFlow)
            {
                BP.Port.Depts rootDepts = new BP.Port.Depts("0");
                if (rootDepts == null || rootDepts.Count == 0)
                {
                    BP.GPM.Dept rootDept = new BP.GPM.Dept();
                    rootDept.Name = " Group Headquarters ";
                    rootDept.FK_DeptType = "01";
                    rootDept.ParentNo = "0";
                    rootDept.Idx = 0;
                    rootDept.Insert();
                }
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
                BP.GPM.Dept currDept = new BP.GPM.Dept(currDeptNo);
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
                BP.GPM.Dept currDept = new BP.GPM.Dept(currDeptNo);
                BP.GPM.Dept nextDept = new BP.GPM.Dept(nextDeptNo);
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
                        nextDept = new BP.GPM.Dept(nodeNo);
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
            if (dept.ParentNo == "0")
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
                BP.GPM.Depts cDepts = new BP.GPM.Depts(deptNo);
              
                foreach (BP.GPM.Dept item in cDepts)
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
            BP.GPM.Depts childDept = new BP.GPM.Depts(dept.No);
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
            BP.GPM.Dept deptParent = new BP.GPM.Dept(dept.ParentNo);
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
        /// <returns> Returns personnel department under </returns>
        [WebMethod(EnableSession = true)]
        public string GetEmpsByDeptNo(string FK_Dept)
        {
            string sql = "select No,Name,{0} as FK_Dept FROM Port_Emp where FK_Dept={1}";
            sql = string.Format(sql, FK_Dept, FK_Dept);
            return RunSQLReturnTableS(sql);
        }
        #endregion

        #endregion

        #region  And sharing templates related methods .

        /// <summary>
        ///  Obtain a shared template directory name 
        /// </summary>
        /// <returns>用@ Separate file names conform .</returns>
        [WebMethod(EnableSession = false)]
        public string GetDirs(string dir,bool FileOrDirecotry )
        {
            string ip = "online.ccflow.org";
            string userNo = "ccflowlover";
            string pass = "ccflowlover";

            List<string> listDir = new List<string>();
            string dirs = "";
            try
            {
               FtpConnection conn = new FtpConnection(ip, userNo, pass);
               List<Win32FindData> sts = getFiles(conn,dir);

                foreach (Win32FindData item in sts)
                {
                    if (FileOrDirecotry)
                    {
                        if (item.FileAttributes == FileAttributes.Directory )
                            listDir.Add(item.FileName);
                    }
                    else if (item.FileAttributes == FileAttributes.Normal)
                    {
                        string tmp = item.FileName;
                        tmp = tmp.Substring(0, tmp.LastIndexOf('.'));

                        if (!listDir.Contains(tmp))
                            listDir.Add(tmp);
                    }
                }

                foreach (string item in listDir)
                {
                    dirs += item + "@";
                }
                if (!string.IsNullOrEmpty(dirs))
                    dirs = dirs.Substring(0, dirs.LastIndexOf('@'));

            }
            catch (Exception e)
            {
                BP.DA.Log.DebugWriteError(e.ToString());
            }
            return dirs;
        }

        public class FtpFile
        {
            public enum FileType
            {
                File, Directory
            }
            public FileType Type = FileType.Directory;
            public string Name;
            public string Ext;
            public string Path;
            public FtpFile Super = null;
            public List<FtpFile> Subs;
            /// <summary>
            /// true Identify the level directory look for resource files , You can preview download , Configuration values in the lower file 
            /// </summary>
            public bool CanViewAndDown;


            public void SyncChildren()
            {
                //foreach (var item in this.Subs)
                //{
                //    item.Super = this;
                //}
            }
        }
        /// <summary>
        ///  Obtain a shared template directory name 
        /// </summary>
        /// <returns>用@ Separate file names conform .</returns>
        [WebMethod(EnableSession = false)]
        public FtpFile GetDirectory()
        {
            string FlowTemplate = DoPort.FlowTemplate;
            string ip = "online.ccflow.org";
            string userNo = "ccflowlover";
            string pass = "ccflowlover";

            FtpFile Superfile = null;
            FtpSupport.FtpConnection conn = null;
            try
            {
                conn = new FtpSupport.FtpConnection(ip, userNo, pass);

                Superfile = new FtpFile() { Name = FlowTemplate, Path = FlowTemplate, Type = FtpFile.FileType.Directory };
                Superfile.Subs = new List<FtpFile>();

                List<FtpSupport.Win32FindData> sts = getFiles(conn, FlowTemplate);
                foreach (FtpSupport.Win32FindData item in sts)
                {

                    FtpFile file = new FtpFile() { Path = FlowTemplate + "\\" + item.FileName };
                    file.Super = new FtpFile() { Name = Superfile.Name, Path = Superfile.Path };

                    Superfile.Subs.Add(file);

                    if (item.FileAttributes == FileAttributes.Directory)
                    {
                        file.Name = item.FileName;
                        file.Type = FtpFile.FileType.Directory;
                    }
                    else 
                    {
                        file.Type = FtpFile.FileType.File;
                        string tmp = item.FileName;

                        file.Name = tmp.Substring(0, tmp.LastIndexOf('.'));
                        file.Ext = tmp.Substring(tmp.LastIndexOf('.') + 1);

                    }
                }

                Superfile.SyncChildren();
                foreach (FtpFile item in Superfile.Subs)
                {
                    if (item.Type == FtpFile.FileType.Directory)
                    { getSubFile(conn, item); }
                }

                conn.Close();
            }
            catch (Exception e)
            {
                BP.DA.Log.DebugWriteError(e.ToString());
            }
           
            return Superfile;

        }

        void getSubFile(FtpSupport.FtpConnection conn, FtpFile Superfile)
        {
            Superfile.Subs = new List<FtpFile>();
            string path = Superfile.Path;

            List<FtpSupport.Win32FindData> sts = getFiles(conn, path);
            foreach (FtpSupport.Win32FindData item in sts)
            {

                FtpFile file = new FtpFile() { Name = item.FileName, Path = path + "\\" + item.FileName };
                file.Super = new FtpFile() { Name = Superfile.Name, Path = Superfile.Path };
             

                if (item.FileAttributes == FileAttributes.Directory)
                {
                    file.Type = FtpFile.FileType.Directory;
                    Superfile.Subs.Add(file);
                }
                else 
                {
                    file.Type = FtpFile.FileType.File;
                    string tmp = item.FileName;

                    file.Name = tmp.Substring(0, tmp.LastIndexOf('.'));
                    file.Ext = tmp.Substring(tmp.LastIndexOf('.') + 1);


                    if (file.Name.Contains("Flow"))
                    {
                        Superfile.CanViewAndDown = true;
                        Superfile.Type = FtpFile.FileType.File;
                    }
                    bool flag = false;
                    foreach (var f in Superfile.Subs)
                    {
                        if (f.Name.Equals(file.Name))
                        {
                            flag = true;
                            break;
                        }
                    }

                    if (!flag)
                        Superfile.Subs.Add(file);                    ;
                }
            }
                          
            Superfile.SyncChildren();

            foreach (FtpFile item in Superfile.Subs)
            {
                if (item.Type == FtpFile.FileType.Directory)
                { 
                    getSubFile(conn, item);
                }
            }
        }

        List<FtpSupport.Win32FindData> getFiles(FtpSupport.FtpConnection conn, string path)
        {
            List<FtpSupport.Win32FindData> sts = new List<Win32FindData>();
            try
            {
                string tmp = conn.GetCurrentDirectory();
                conn.SetCurrentDirectory("/");
                conn.SetCurrentDirectory(path); // Set the current directory .
                FtpSupport.Win32FindData[] f = conn.FindFiles();

                foreach (FtpSupport.Win32FindData item in f)
                {
                    if (".".Equals(item.FileName)
                        || "..".Equals(item.FileName)
                        || string.Empty.Equals(item.FileName))
                        continue;

                    sts.Add(item);
                }
            }
            catch (Exception e) 
            {
                throw new Exception("FTP The server reads the directory error :" + e.Message, e);
            }
            return sts;
        }

        [WebMethod(EnableSession = true)]
        public byte[] FlowTemplateDown(string[] FlowFileName)
        {
            string path = FlowFileName[0]
               , fileName = FlowFileName[1]
               , fileType = FlowFileName[2]
               , cmd = FlowFileName[3];

            if (string.IsNullOrEmpty(path)
                || string.IsNullOrEmpty(fileName)
                || string.IsNullOrEmpty(fileType)
                || string.IsNullOrEmpty(cmd))
            {
                throw new Exception("FTP Server parameter file read error !" );
            }

            string ip = "online.ccflow.org",
               userNo = "ccflowlover",
               pass = "ccflowlover";
            FtpConnection conn = new FtpConnection(ip, userNo, pass);

            byte[] bytes = null;
            try
            {
                bytes = new byte[] { };

                conn.SetCurrentDirectory(path); // Set the current directory .
                FtpStream fs = conn.OpenFile(fileName, GenericRights.Read);
                if (null != fs )
                {
                    System.IO.MemoryStream ms = new MemoryStream();
                    fs.CopyTo(ms);
                    bytes = new byte[ms.Length];
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Read(bytes, 0, bytes.Length);
                }
            }
            catch(Exception e)
            {
                throw new Exception("FTP Server file read error :" + e.Message, e);
            }
            conn.Close();

            if (null != bytes && 0 < bytes.Length && fileType == "XML")
            {
                if (cmd.Equals("INSTALL"))
                {// Line installation 
                    if (fileName.Equals("Flow.xml"))
                    {
                        //  Installation process 
                        path = this.FlowTemplateUpload(bytes, fileName);
                        bytes = System.Text.Encoding.UTF8.GetBytes(path);
                    }
                    else
                    {
                        // Forms installation 
                        //this.UploadfileCCForm(bytes, fileName, "");
                    }
                }
                else if (cmd == "DOWN")
                {   //  Saved to the machine 

                    //HttpContext.Current.Response.BinaryWrite(bytes);
                    //string xml = System.Text.Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                    //HttpContext.Current.Response.Write(xml);

                    //fileName = HttpUtility.UrlEncode(fileName);
                    //HttpContext.Current.Response.Charset = "GB2312";
                    //HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
                    //HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");

                    //HttpContext.Current.Response.Flush();
                    //HttpContext.Current.Response.End();
                    //HttpContext.Current.Response.Close();
                }
            }
            return bytes;
        }
             
        #endregion  And sharing templates related methods .

        #region  Public Methods 
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
        /// <summary>
        ///  Gets the value of 
        /// </summary>
        /// <param name="kev"></param>
        /// <returns></returns>
        [WebMethod]
        public string CfgKey(string kev)
        {
            switch (kev)
            {
                case "SendEmailPass":
                case "AppCenterDSN":
                case "FtpPass":
                    throw new Exception("@ Illegal access ");
                default:
                    break;
            }

            return BP.Sys.SystemConfig.AppSettings[kev];
        }
        /// <summary>
        ///  Upload file .
        /// </summary>
        /// <param name="FileByte"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [WebMethod]
        public string UploadFile(byte[] FileByte, String fileName)
        {
            string path = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;

            string filePath = path + "\\" + fileName;
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            // As used herein, the absolute path to index 
            FileStream stream = new FileStream(filePath, FileMode.CreateNew);
            stream.Write(FileByte, 0, FileByte.Length);
            stream.Close();

            DataSet ds = new DataSet();
            ds.ReadXml(filePath);

            return Silverlight.DataSetConnector.Connector.ToXml(ds);
        }
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
        ///  Save ens
        /// </summary>
        /// <param name="vals"></param>
        /// <returns></returns>
        [WebMethod]
        public string SaveEn(string vals)
        {
            Entity en = null;
            try
            {
                AtPara ap = new AtPara(vals);
                string enName = ap.GetValStrByKey("EnName");
                string pk = ap.GetValStrByKey("PKVal");
                en = ClassFactory.GetEn(enName);
                en.ResetDefaultVal();

                if (en == null)
                    throw new Exception(" Invalid class name :" + enName);

                if (string.IsNullOrEmpty(pk) == false)
                {
                    en.PKVal = pk;
                    en.RetrieveFromDBSources();
                }

                foreach (string key in ap.HisHT.Keys)
                {
                    if (key == "PKVal")
                        continue;
                    en.SetValByKey(key, ap.HisHT[key].ToString().Replace('#', '@'));
                }
                en.Save();
                return en.PKVal as string;
            }
            catch (Exception ex)
            {
                if (en != null)
                    en.CheckPhysicsTable();

                return "Error:" + ex.Message;
            }
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
            return Silverlight.DataSetConnector.Connector.ToXml(ds);
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
            DataSet ds = RunSQLReturnDataSet(sqls);
            return Connector.ToXml(ds);
        }

        public DataSet RunSQLReturnDataSet(string sqls)
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
            return ds;
        }
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
                return s;
            }
            catch
            {
                return null;
            }
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
            return Silverlight.DataSetConnector.Connector.ToXml(ds);
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
        #endregion

        [WebMethod(EnableSession = true)]
        public string[]  GetNodeIconFile()
        {
            // XAP/Admin/WF
            string path = Server.MapPath("../../../");
            path += "ClientBin\\NodeIcon";
         
            string[] files = System.IO.Directory.GetFiles(path, "*.png");

            for (int i = 0; i < files.Length; i++)
            {
                var item = files[i];
                item = item.Substring(path.Length, item.Length - path.Length);
                item = item.Substring(item.LastIndexOf('\\')+1,item.IndexOf('.')-1);
                files[i] = item;
            }

            return files;
        }

        /// <summary>
        ///  Executive functions return information 
        /// </summary>
        /// <param name="doType"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <param name="v4"></param>
        /// <param name="v5"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = false)]
        public string DoType(string doType, string v1, string v2, string v3, string v4, string v5)
        {
            try
            {
               
                switch (doType)
                {
                    case "FrmTreeUp": //  Form tree 
                        SysFormTree sft = new SysFormTree();
                        sft.DoUp();
                        return null;
                    case "FrmTreeDown": //  Form tree 
                        SysFormTree sft1 = new SysFormTree();
                        sft1.DoDown();
                        return null;
                    case "FrmUp":
                        MapData md1 = new MapData(v1);
                        md1.DoOrderDown();
                        return null;
                    case "FrmDown":
                        MapData md = new MapData(v1);
                        md.DoOrderDown();
                        return null;
                    case "AdminLogin":
                        try
                        {
                            if (BP.Sys.SystemConfig.IsDebug == true)
                                return null;

                            BP.Port.Emp emp = new BP.Port.Emp();
                            emp.No = v1;
                            emp.RetrieveFromDBSources();
                            if (emp.Pass == v2)
                                return null;
                            return "error password.";
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    case "DeleteFrmSort":
                        SysFormTree fs = new SysFormTree();
                        fs.No = v1;
                        fs.Delete();
                        SysFormTree ft = new SysFormTree();
                        ft.No = v1;
                        ft.Delete();
                        return null;
                    case "DeleteFrm":
                    case "DelFrm":
                        MapData md4 = new MapData();
                        md4.No = v1;
                        md4.Delete();
                        return null;
                    case "InitDesignerXml":
                        string path = BP.Sys.SystemConfig.PathOfData + "\\Xml\\Designer.xml";
                        DataSet ds = new DataSet();
                        ds.ReadXml(path);
                        ds = this.TurnXmlDataSet2SLDataSet(ds);
                        return Silverlight.DataSetConnector.Connector.ToXml(ds);
                    default:
                        throw new Exception(" No judgment , Function No. " + doType);
                }
            }
            catch (Exception ex)
            {
                BP.DA.Log.DefaultLogWriteLineError(" Execution error , Function No. " + doType + " error:" + ex.Message);
                throw new Exception(" Execution error , Function No. " + doType + " error:" + ex.Message);
            }
        }

        /// <summary>
        ///  According to workID Get worklist 
        /// FK_Node  Node ID
        /// rdt  Record Date , Also work Accepted .
        /// sdt  Should be completed by the date .
        /// FK_emp  Operator number .
        /// EmpName  Operator Name .
        /// </summary>
        /// <param name="workid">workid</param>
        /// <returns></returns>
        [WebMethod(EnableSession = false)]
        public string GetDTOfWorkList(string fk_flow, string workid)
        {
            DataSet ds = GetWorkList(fk_flow, workid);
            return Connector.ToXml(ds);
        }
        public DataSet GetWorkList(string fk_flow, string workid)
        {
            try
            {
                string sql = "";
                string table = "ND" + int.Parse(fk_flow) + "Track";
                DataSet ds = new DataSet();
                sql = "SELECT NDFrom, NDTo,ActionType,Msg,RDT,EmpFrom,EmpFromT FROM " + table + " WHERE WorkID=" + workid + "  OR FID=" + workid + "ORDER BY NDFrom ASC,NDTo ASC";
                DataTable mydt = BP.DA.DBAccess.RunSQLReturnTable(sql);
                mydt.TableName = "WF_Track";
                ds.Tables.Add(mydt);
                return ds;
            }
            catch (Exception ex)
            {
                BP.DA.Log.DefaultLogWriteLineError("GetDTOfWorkList An error has occurred  paras:" + fk_flow + "\t" + workid + ex.Message);
                return null;
            }
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
                    WebUser.SignInOfGener(emp, lang, "admin", true, false);
                }
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        [Obsolete]
        public string GetFlowBySort(string sort)
        {
            DataSet ds = new DataSet();
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable("SELECT No,Name,FK_FlowSort FROM WF_Flow");
            ds.Tables.Add(dt);
            return Silverlight.DataSetConnector.Connector.ToXml(ds);
        }

        [WebMethod(EnableSession = true)]
        public string Do(string doWhat, string para1, bool isLogin)
        {
            //  In case admin When an error occurs account login , It returns an error message 
            var result = LetAdminLogin("CH", isLogin);
            if (string.IsNullOrEmpty(result) == false)
                return result;

            switch (doWhat)
            {
                case "GenerFlowTemplete":
                    Flow temp = new BP.WF.Flow(para1);
                    return null;
                case "NewSameLevelFrmSort":
                    BP.Sys.SysFormTree frmSort = null;
                    try
                    {
                        var para = para1.Split(',');
                        frmSort = new SysFormTree(para[0]);
                        string sameNodeNo = frmSort.DoCreateSameLevelNode().No;
                        frmSort = new SysFormTree(sameNodeNo);
                        frmSort.Name = para[1];
                        frmSort.Update();
                        return null;
                    }
                    catch (Exception ex)
                    {
                        return "Do Method NewFormSort Branch has a error , para:\t" + para1 + ex.Message;
                    }
                case "NewSubLevelFrmSort":
                    BP.Sys.SysFormTree frmSortSub = null;
                    try
                    {
                        var para = para1.Split(',');
                        frmSortSub = new SysFormTree(para[0]);
                        string sameNodeNo = frmSortSub.DoCreateSubNode().No;
                        frmSortSub = new SysFormTree(sameNodeNo);
                        frmSortSub.Name = para[1];
                        frmSortSub.Update();
                        return null;
                    }
                    catch (Exception ex)
                    {
                        return "Do Method NewSubLevelFrmSort Branch has a error , para:\t" + para1 + ex.Message;
                    }
                case "NewSameLevelFlowSort":
                    BP.WF.FlowSort fs = null;
                    try
                    {
                        var para = para1.Split(',');
                        fs = new FlowSort(para[0]);
                        string sameNodeNo = fs.DoCreateSameLevelNode().No;
                        fs = new FlowSort(sameNodeNo);
                        fs.Name = para[1];
                        fs.Update();
                        return fs.No;
                    }
                    catch (Exception ex)
                    {
                        BP.DA.Log.DefaultLogWriteLineError("Do Method NewSameLevelFlowSort Branch has a error , para:\t" + para1 + ex.Message);
                        return null;
                    }
                case "NewSubFlowSort":
          
                    try
                    {
                        var para = para1.Split(',');
                        BP.WF.FlowSort fsSub = new FlowSort(para[0]);
                        string subNodeNo = fsSub.DoCreateSubNode().No;
                        FlowSort subFlowSort = new FlowSort(subNodeNo);
                        subFlowSort.Name = para[1];
                        subFlowSort.Update();
                        return subFlowSort.No;
                    }
                    catch (Exception ex)
                    {
                        BP.DA.Log.DefaultLogWriteLineError("Do Method NewSubFlowSort Branch has a error , para:\t"  + ex.Message);
                        return null;
                    }
                case "EditFlowSort":
                    try
                    {
                        var para = para1.Split(',');
                        fs = new FlowSort(para[0]);
                        fs.Name = para[1];
                        fs.Save();
                        return fs.No;
                    }
                    catch (Exception ex)
                    {
                        BP.DA.Log.DefaultLogWriteLineError("Do Method EditFlowSort Branch has a error , para:\t" + para1 + ex.Message);
                        return null;
                    }
                case "NewFlow":
                    Flow fl = new Flow();
                    try
                    {
                        string[] ps = para1.Split(',');
                        if (ps.Length != 5)
                            throw new Exception("@ Create a process parameter error ");

                        string fk_floSort = ps[0];
                        string flowName = ps[1];
                        DataStoreModel dataSaveModel = (DataStoreModel)int.Parse(ps[2]);
                        string pTable = ps[3];

                        string FlowMark = ps[4];

                        fl.DoNewFlow(fk_floSort, flowName, dataSaveModel, pTable, FlowMark);
                        return fl.No + ";" + fl.Name;
                    }
                    catch (Exception ex)
                    {
                        BP.DA.Log.DefaultLogWriteLineError("Do Method NewFlow Branch has a error , para:\t" + para1 + ex.Message);
                        return ex.Message;
                    }
                case "DelFlow": // Delete Process .
                    return BP.WF.WorkflowDefintionManager.DeleteFlowTemplete(para1);
                case "DelLable":
                    BP.WF.LabNote ln = new BP.WF.LabNote(para1);
                    try
                    {
                        ln.Delete();
                    }
                    catch (Exception ex)
                    {
                        BP.DA.Log.DefaultLogWriteLineError("Do Method DelLable Branch has a error , para:\t" + para1 + ex.Message);
                    }
                    return null;
                case "DelFlowSort":
                    try
                    {
                        FlowSort delfs = new FlowSort(para1);
                        delfs.Delete();
                    }
                    catch (Exception ex)
                    {
                        BP.DA.Log.DefaultLogWriteLineError("Do Method DelFlowSort Branch has a error , para:\t" + para1 + ex.Message);
                    }
                    return null;
                case "NewNode":
                    try
                    {
                        BP.WF.Flow fl11 = new BP.WF.Flow(para1);
                        BP.WF.Node node = new BP.WF.Node();
                        node.FK_Flow = "";
                        node.X = 0;
                        node.Y = 0;
                        node.Insert();
                    }
                    catch (Exception ex)
                    {
                        BP.DA.Log.DefaultLogWriteLineError("Do Method NewNode Branch has a error , para:\t" + para1 + ex.Message);
                    }

                    return null;
                case "DelNode":
                    try
                    {
                        if (!string.IsNullOrEmpty(para1))
                        {
                            BP.WF.Node delNode = new BP.WF.Node(int.Parse(para1));
                            delNode.Delete();
                        }
                    }
                    catch (Exception ex)
                    {
                        return "err:" + ex.Message;

                     //   BP.DA.Log.DefaultLogWriteLineError("Do Method DelNode Branch has a error , para:\t" + para1 + ex.Message);
                    }
                    return null;
                case "NewLab":
                    BP.WF.LabNote lab = new BP.WF.LabNote(); ;
                    try
                    {
                        lab.FK_Flow = para1;
                        lab.MyPK = BP.DA.DBAccess.GenerOID().ToString();
                        lab.Insert();

                    }
                    catch (Exception ex)
                    {
                        BP.DA.Log.DefaultLogWriteLineError("Do Method NewLab Branch has a error , para:\t" + para1 + ex.Message);
                    }
                    return lab.MyPK;
                case "DelLab":
                    try
                    {
                        BP.WF.LabNote dellab = new BP.WF.LabNote();
                        dellab.MyPK = para1;
                        dellab.Delete();
                    }
                    catch (Exception ex)
                    {
                        BP.DA.Log.DefaultLogWriteLineError("Do Method DelLab Branch has a error , para:\t" + para1 + ex.Message);
                    }
                    return null;
                case "GetSettings":
                    return SystemConfig.AppSettings[para1];
                case "GetFlows":
                    string sqls = "SELECT * FROM WF_FlowSort ORDER BY No,Idx";
                    sqls += "@SELECT No,Name,FK_FlowSort,Idx FROM WF_Flow ORDER BY FK_FlowSort,Idx,No";
                    return RunSQLReturnTableS(sqls);
                case "SaveFlowFrm":
                    Entity en = null;
                    try
                    {
                        AtPara ap = new AtPara(para1);
                        string enName = ap.GetValStrByKey("EnName");
                        string pk = ap.GetValStrByKey("PKVal");
                        en = ClassFactory.GetEn(enName);
                        en.ResetDefaultVal();

                        if (en == null)
                            throw new Exception(" Invalid class name :" + enName);

                        if (string.IsNullOrEmpty(pk) == false)
                        {
                            en.PKVal = pk;
                            en.RetrieveFromDBSources();
                        }

                        foreach (string key in ap.HisHT.Keys)
                        {
                            if (key == "PKVal")
                                continue;
                            en.SetValByKey(key, ap.HisHT[key].ToString().Replace('^', '@'));
                        }
                        en.Save();
                        return en.PKVal as string;
                    }
                    catch (Exception ex)
                    {
                        if (en != null)
                            en.CheckPhysicsTable();
                        return "Error:" + ex.Message;
                    }
                default:
                    throw null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isLogin"></param>
        /// <param name="param">fk_flow,nodeName,icon,x,y,HisRunModel</param>
        /// <returns></returns>
        /// <returns> Returns the node number </returns>
        [WebMethod(EnableSession = true)]
        public int DoNewNode1(bool isLogin, params string[] param)
        {
            LetAdminLogin("CH", isLogin);

            string fk_flow = param[0];
            if (string.IsNullOrEmpty(fk_flow))
                return 0;

            string nodeName = param[1];
            string icon = param[2];



            int x= (int)double.Parse(param[3]),
                y = (int)double.Parse(param[4]), 
                HisRunModel=int.Parse(param[5]);


            Flow fl = new Flow(fk_flow);
            try
            {
                BP.WF.Node nf = fl.DoNewNode(x, y);
                nf.ICON = icon;
                nf.Name = nodeName;
                nf.HisRunModel = (RunModel)HisRunModel;
                nf.Save();
                return nf.NodeID;
            }
            catch
            {
                return 0;
            }

            return 0;
        }

        /// <summary>
        ///  Create a node 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns> Returns the node number </returns>
        [WebMethod(EnableSession = true)]
        public int DoNewNode(string fk_flow, int x, int y, int HisRunModel, string nodeName, bool isLogin)
        {
            LetAdminLogin("CH", isLogin);
            if (string.IsNullOrEmpty(fk_flow))
                return 0;

            Flow fl = new Flow(fk_flow);
            try
            {
                BP.WF.Node nf = fl.DoNewNode(x, y);
                nf.Name = nodeName;
                nf.HisRunModel = (RunModel)HisRunModel;
                nf.Save();
                return nf.NodeID;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        ///  To delete a connection line 
        /// </summary>
        /// <param name="from"> From node </param>
        /// <param name="to"> To node </param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public bool DoDropLine(int from, int to)
        {
            BP.WF.Direction dir = new BP.WF.Direction();
            dir.Node = from;
            dir.ToNode = to;
            dir.Delete();
            #region songhonggang (2014-06-15)  Delete to delete the connection form when conditions 
            Conds conds =new Conds();
            conds.RetrieveByAttr(CondAttr.FK_Node, dir.Node);
            conds.Delete();
            #endregion
            return true;
        }

        /// <summary>
        ///  Create a label 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns> Returns the label number </returns>
        [WebMethod(EnableSession = true)]
        public string DoNewLabel(string fk_flow, int x, int y, string name, string lableId)
        {
            LabNote lab = new LabNote();
            lab.FK_Flow = fk_flow;
            lab.X = x;
            lab.Y = y;
            if (string.IsNullOrEmpty(lableId))
            {
                lab.MyPK = BP.DA.DBAccess.GenerOID().ToString();
            }
            else
            {
                lab.MyPK = lableId;
            }
            lab.Name = name;
            try
            {
                lab.Save();
            }
            catch
            {
            }
            return lab.MyPK;
        }

        [WebMethod]
        public string FlowTemplateUpload(byte[] FileByte, string fileName)
        {
            try
            {
                // File storage path 
                string filepath = BP.Sys.SystemConfig.PathOfTemp + "\\" + fileName;
                // If the file already exists, delete 
                if (File.Exists(filepath))
                    File.Delete(filepath);
                // Create a file stream instance , Write File 
                FileStream stream = new FileStream(filepath, FileMode.CreateNew);
                // Write to file 
                stream.Write(FileByte, 0, FileByte.Length);
                stream.Close();

                // Save Image .

                return filepath;
            }
            catch (Exception exception)
            {
                return "Error: Occured on upload the file. Error Message is :\n" + exception.Message;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FK_flowSort"> Process category number </param>
        /// <param name="Path"> Template file path </param>
        /// <param name="ImportModel"></param>
        /// <param name="Islogin">0,1,2,3</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public string FlowTemplateLoad(string FK_flowSort, string Path,int ImportModel,int SpecialFlowNo)
        {
            try
            {
                ImpFlowTempleteModel model = (ImpFlowTempleteModel)ImportModel;
                LetAdminLogin("CH", true);
                Flow flow = null;
                if (model == ImpFlowTempleteModel.AsSpecFlowNo)
                {
                    if (SpecialFlowNo <= 0)
                    {
                        return " Specifies the process ID error ";
                    }

                    flow = Flow.DoLoadFlowTemplate(FK_flowSort, Path, model, SpecialFlowNo);
                }
                else
                {
                    flow = Flow.DoLoadFlowTemplate(FK_flowSort, Path, model);
                }

                // The implementation of some repair view.
                Flow.RepareV_FlowData_View();

                return string.Format("TRUE,{0},{1},{2}", FK_flowSort, flow.No, flow.Name);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        ///  Save Process 
        /// </summary>
        /// <param name="fk_flow"></param>
        /// <param name="nodes"></param>
        /// <param name="dirs`"></param>
        /// <param name="labes"></param>
        [WebMethod(EnableSession = true)]
        public string FlowSave(string fk_flow, string nodes, string dirs, string labes)
        {
            LetAdminLogin("CH", true);
            return WorkflowDefintionManager.SaveFlow(fk_flow, nodes, dirs, labes);
        }

        [WebMethod]
        public string UploadfileCCForm(byte[] FileByte, string fileName, string fk_frmSort)
        {
            try
            {
                // File storage path 
                string filepath = BP.Sys.SystemConfig.PathOfTemp + "\\" + fileName;
                // If the file already exists, delete 
                if (File.Exists(filepath))
                    File.Delete(filepath);

                // Create a file stream instance , Write File 
                FileStream stream = new FileStream(filepath, FileMode.CreateNew);

                // Write to file 
                stream.Write(FileByte, 0, FileByte.Length);
                stream.Close();

                DataSet ds = new DataSet();
                ds.ReadXml(filepath);

                MapData md = MapData.ImpMapData(ds);
                md.FK_FrmSort = fk_frmSort;
                md.Update();
                return null;
            }
            catch (Exception exception)
            {
                return "Error: Occured on upload the file. Error Message is :\n" + exception.Message;
            }

        }

        [WebMethod]
        public string GetConfig(string key)
        {
            string tmp = BP.Sys.SystemConfig.AppSettings[key];

            return tmp;
        }
    }
}