using System;
using System.IO;
using System.Data;
using System.Collections;
using BP.DA;
using BP.Web.Controls;
using System.Reflection;
using BP.Port;
using BP.En;
using BP.Sys;
namespace BP.WF.DTS
{
    /// <summary>
    /// Method  The summary 
    /// </summary>
    public class OneKeyLoadTemplete : Method
    {
        /// <summary>
        ///  Method with no arguments 
        /// </summary>
        public OneKeyLoadTemplete()
        {
            this.Title = " A key recovery process template directory ";
            this.Help = " This feature is a key backup process reverse operation .";
            this.Help += "@ Please note that the implementation of ";
            this.Help += "@1, All system processes data , Template data , Deconstructing organizational data , Will be deleted .";
            this.Help += "@2, Reload C:\\CCFlowTemplete  Data .";
            this.Help += "@3, This function is generally provided to the ccflow Developer for transplantation between different databases .";
        }
        /// <summary>
        ///  Set the execution variables 
        /// </summary>
        /// <returns></returns>
        public override void Init()
        {
        }
        /// <summary>
        ///  Whether the current operator can perform this method 
        /// </summary>
        public override bool IsCanDo
        {
            get
            {
                if (BP.Web.WebUser.No != "admin")
                    return false;

                return true;
            }
        }
        public override object Do()
        {
            string msg = "";

            #region  Check the data file is complete .
            string path = "C:\\CCFlowTemplete";
            if (System.IO.Directory.Exists(path) == false)
                msg += "@ Error : Agreed server directory does not exist " + path + ", Please put the ccflow Backup file into " + path;

            //PortTables.
            string file = path + "\\PortTables.xml";
            if (System.IO.File.Exists(file) == false)
                msg += "@ Error : Conventions file does not exist ," + file;

            //SysTables.
            file = path + "\\SysTables.xml";
            if (System.IO.File.Exists(file) == false)
                msg += "@ Error : Conventions file does not exist ," + file;

            //FlowTables.
            file = path + "\\FlowTables.xml";
            if (System.IO.File.Exists(file) == false)
                msg += "@ Error : Conventions file does not exist ," + file;
            #endregion  Check the data file is complete .

            #region 1  Load flow data base table .
            DataSet ds = new DataSet();
            ds.ReadXml(path + "\\FlowTables.xml");

            // Process Category .
            FlowSorts sorts = new FlowSorts();
            sorts.ClearTable();
            DataTable dt = ds.Tables["WF_FlowSort"];
           // sorts = QueryObject.InitEntitiesByDataTable(sorts, dt, null) as FlowSorts;
            foreach (FlowSort item in sorts)
            {
                item.DirectInsert(); // Insert data .
            }
            #endregion 1  Load flow data base table .

            #region 2  Organizational Structure .
            ds = new DataSet();
            ds.ReadXml(path + "\\PortTables.xml");

            //Port_Emp.
            Emps emps = new Emps();
            emps.ClearTable();
            dt = ds.Tables["Port_Emp"];
            emps = QueryObject.InitEntitiesByDataTable(emps, dt, null) as Emps;
            foreach (Emp item in emps)
            {
                item.DirectInsert(); // Insert data .
            }

            //Depts.
            Depts depts = new Depts();
            depts.ClearTable();
            dt = ds.Tables["Port_Dept"];
            depts = QueryObject.InitEntitiesByDataTable(depts, dt, null) as Depts;
            foreach (Dept item in depts)
            {
                item.DirectInsert(); // Insert data .
            }

            //Stations.
            Stations stas = new Stations();
            stas.ClearTable();
            dt = ds.Tables["Port_Station"];
            stas = QueryObject.InitEntitiesByDataTable(stas, dt, null) as Stations;
            foreach (Station item in stas)
            {
                item.DirectInsert(); // Insert data .
            }


            //EmpDepts.
            EmpDepts eds = new EmpDepts();
            eds.ClearTable();
            dt = ds.Tables["Port_EmpDept"];
            eds = QueryObject.InitEntitiesByDataTable(eds, dt, null) as EmpDepts;
            foreach (EmpDept item in eds)
            {
                item.DirectInsert(); // Insert data .
            }

            //EmpStations.
            EmpStations ess = new EmpStations();
            ess.ClearTable();
            dt = ds.Tables["Port_EmpStation"];
            ess = QueryObject.InitEntitiesByDataTable(ess, dt, null) as EmpStations;
            foreach (EmpStation item in ess)
            {
                item.DirectInsert(); // Insert data .
            }
            #endregion 2  Organizational Structure .

            #region 3  Data recovery system .
            ds = new DataSet();
            ds.ReadXml(path + "\\SysTables.xml");

            // Enumerate Main.
            SysEnumMains sems = new SysEnumMains();
            sems.ClearTable();
            dt = ds.Tables["Sys_EnumMain"];
            sems = QueryObject.InitEntitiesByDataTable(sems, dt, null) as SysEnumMains;
            foreach (SysEnumMain item in sems)
            {
                item.DirectInsert(); // Insert data .
            }

            // Enumerate .
            SysEnums ses = new SysEnums();
            ses.ClearTable();
            dt = ds.Tables["Sys_Enum"];
            ses = QueryObject.InitEntitiesByDataTable(ses, dt, null) as SysEnums;
            foreach (SysEnum item in ses)
            {
                item.DirectInsert(); // Insert data .
            }

            ////Sys_FormTree.
            //BP.Sys.SysFormTrees sfts = new SysFormTrees();
            //sfts.ClearTable();
            //dt = ds.Tables["Sys_FormTree"];
            //sfts = QueryObject.InitEntitiesByDataTable(sfts, dt, null) as SysFormTrees;
            //foreach (SysFormTree item in sfts)
            //{
            //    try
            //    {
            //       item.DirectInsert(); // Insert data .
            //    }
            //    catch
            //    {
            //    }
            //}
            #endregion 3  Data recovery system .

            #region 4. Form data backup .
            if (1 == 2)
            {
                string pathOfTables = path + "\\SFTables";
                System.IO.Directory.CreateDirectory(pathOfTables);
                SFTables tabs = new SFTables();
                tabs.RetrieveAll();
                foreach (SFTable item in tabs)
                {
                    if (item.No.Contains("."))
                        continue;

                    string sql = "SELECT * FROM " + item.No;
                    ds = new DataSet();
                    ds.Tables.Add(BP.DA.DBAccess.RunSQLReturnTable(sql));
                    ds.WriteXml(pathOfTables + "\\" + item.No + ".xml");
                }
            }
            #endregion 4  Form data backup .

            #region 5. Data recovery form .
            // Delete all of the process data .
            MapDatas mds = new MapDatas();
            mds.RetrieveAll();
            foreach (MapData fl in mds)
            {
                //if (fl.FK_FormTree.Length > 1 || fl.FK_FrmSort.Length > 1)
                //    continue;
                fl.Delete(); // Delete Process .
            }

            // Clear Data .
            SysFormTrees fss = new SysFormTrees();
            fss.ClearTable();

            //  Scheduling form file .         
            string frmPath = path + "\\Form";
            DirectoryInfo dirInfo = new DirectoryInfo(frmPath);
            DirectoryInfo[] dirs = dirInfo.GetDirectories();
            foreach (DirectoryInfo item in dirs)
            {
                if (item.FullName.Contains(".svn"))
                    continue;

                string[] fls = System.IO.Directory.GetFiles(item.FullName);
                if (fls.Length == 0)
                    continue;

                SysFormTree fs = new SysFormTree();
                fs.No = item.Name.Substring(0, item.Name.IndexOf('.') );
                fs.Name = item.Name.Substring(item.Name.IndexOf('.'));
                fs.ParentNo = "0";
                fs.Insert();

                foreach (string f in fls)
                {
                    try
                    {
                        msg += "@ Begin scheduling form template file :" + f;
                        System.IO.FileInfo info = new System.IO.FileInfo(f);
                        if (info.Extension != ".xml")
                            continue;

                        ds = new DataSet();
                        ds.ReadXml(f);

                        MapData md = MapData.ImpMapData(ds, false);
                        md.FK_FrmSort = fs.No;
                        md.Update();
                    }
                    catch (Exception ex)
                    {
                        msg += "@ Scheduling failure , File :"+f+", Exception Information :" + ex.Message;
                    }
                }
            }
            #endregion 5. Data recovery form .

            #region 6. Data recovery process .
            // Delete all of the process data .
            Flows flsEns = new Flows();
            flsEns.RetrieveAll();
            foreach (Flow fl in flsEns)
            {
                fl.DoDelete(); // Delete Process .
            }

            dirInfo = new DirectoryInfo(path + "\\Flow\\");
            dirs = dirInfo.GetDirectories();

            // Deleting Data .
            FlowSorts fsRoots = new FlowSorts();
            fsRoots.ClearTable();

            // Generation process tree .
            FlowSort fsRoot = new FlowSort();
            fsRoot.No = "99";
            fsRoot.Name = " Process tree ";
            fsRoot.ParentNo = "0";
            fsRoot.DirectInsert();

            foreach (DirectoryInfo dir in dirs)
            {
                if (dir.FullName.Contains(".svn"))
                    continue;

                string[] fls = System.IO.Directory.GetFiles(dir.FullName);
                if (fls.Length == 0)
                    continue;

                FlowSort fs = new FlowSort();
                fs.No = dir.Name.Substring(0, dir.Name.IndexOf('.') );
                fs.Name = dir.Name.Substring(3);
                fs.ParentNo = fsRoot.No;
                fs.Insert();

                foreach (string filePath in fls)
                {
                    msg += "@ Begin scheduling process template file :" + filePath;
                    Flow myflow = BP.WF.Flow.DoLoadFlowTemplate(fs.No, filePath, ImpFlowTempleteModel.AsTempleteFlowNo);
                    msg += "@ Process :" + myflow.Name + " Load success .";

                    System.IO.FileInfo info = new System.IO.FileInfo(filePath);
                    myflow.Name = info.Name.Replace(".xml", "");
                    if (myflow.Name.Substring(2, 1) == ".")
                        myflow.Name = myflow.Name.Substring(3);

                    myflow.DirectUpdate();
                }
            }
            #endregion 6. Data recovery process .

            BP.DA.Log.DefaultLogWriteLineInfo(msg);

            // Remove the extra space .
            BP.WF.DTS.DeleteBlankGroupField dts = new DeleteBlankGroupField();
            dts.Do();

            // Execute the generated signature .
            GenerSiganture gs = new GenerSiganture();
            gs.Do();

            return msg;
        }
    }
}
