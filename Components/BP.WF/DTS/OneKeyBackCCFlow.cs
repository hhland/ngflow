using System;
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
    public class OneKeyBackCCFlow : Method
    {
        /// <summary>
        ///  Method with no arguments 
        /// </summary>
        public OneKeyBackCCFlow()
        {
            this.Title = " A key backup processes and forms .";
            this.Help = " The Process , Form , Organizational structure of the data are generated xml Document backup to C:\\CCFlowTemplete Below .";
        }
        /// <summary>
        ///  Set the execution variables 
        /// </summary>
        /// <returns></returns>
        public override void Init()
        {
            //this.Warning = " Are you sure you want to perform ?";
            //HisAttrs.AddTBString("P1", null, " Old Password ", true, false, 0, 10, 10);
            //HisAttrs.AddTBString("P2", null, " New Password ", true, false, 0, 10, 10);
            //HisAttrs.AddTBString("P3", null, " Confirm ", true, false, 0, 10, 10);
        }
        /// <summary>
        ///  Whether the current operator can perform this method 
        /// </summary>
        public override bool IsCanDo
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        ///  Carried out 
        /// </summary>
        /// <returns> Return to the results </returns>
        public override object Do()
        {
            string path = "C:\\CCFlowTemplete" + DateTime.Now.ToString("yy年MM月dd日HH时mm分ss秒");
            if (System.IO.Directory.Exists(path) == false)
                System.IO.Directory.CreateDirectory(path);

            #region 1. Backup process category information 
            DataSet dsFlows = new DataSet();
            //WF_FlowSort
            DataTable dt = DBAccess.RunSQLReturnTable("SELECT * FROM WF_FlowSort");
            dt.TableName = "WF_FlowSort";
            dsFlows.Tables.Add(dt);
            dsFlows.WriteXml(path + "\\FlowTables.xml");
            #endregion  Backup process category information .

            #region 2. Backup Structure .
            DataSet dsPort = new DataSet();
            //emps
            dt = DBAccess.RunSQLReturnTable("SELECT * FROM Port_Emp");
            dt.TableName = "Port_Emp";
            dsPort.Tables.Add(dt);

            //Port_Dept
            dt = DBAccess.RunSQLReturnTable("SELECT * FROM Port_Dept");
            dt.TableName = "Port_Dept";
            dsPort.Tables.Add(dt);

            //Port_Station
            dt = DBAccess.RunSQLReturnTable("SELECT * FROM Port_Station");
            dt.TableName = "Port_Station";
            dsPort.Tables.Add(dt);

            //Port_EmpStation
            dt = DBAccess.RunSQLReturnTable("SELECT * FROM Port_EmpStation");
            dt.TableName = "Port_EmpStation";
            dsPort.Tables.Add(dt);

            //Port_EmpDept
            dt = DBAccess.RunSQLReturnTable("SELECT * FROM Port_EmpDept");
            dt.TableName = "Port_EmpDept";
            dsPort.Tables.Add(dt);

            dsPort.WriteXml(path + "\\PortTables.xml");
            #endregion  Form data backup .

            #region 3. Data backup system 
            DataSet dsSysTables = new DataSet();

            //Sys_EnumMain
            dt = DBAccess.RunSQLReturnTable("SELECT * FROM Sys_EnumMain");
            dt.TableName = "Sys_EnumMain";
            dsSysTables.Tables.Add(dt);

            //Sys_Enum
            dt = DBAccess.RunSQLReturnTable("SELECT * FROM Sys_Enum");
            dt.TableName = "Sys_Enum";
            dsSysTables.Tables.Add(dt);

            //Sys_FormTree
            dt = DBAccess.RunSQLReturnTable("SELECT * FROM Sys_FormTree");
            dt.TableName = "Sys_FormTree";
            dsSysTables.Tables.Add(dt);
            dsSysTables.WriteXml(path + "\\SysTables.xml");
            #endregion  Data backup system .

            #region 4. Form data backup .
            string pathOfTables = path + "\\SFTables";
            System.IO.Directory.CreateDirectory(pathOfTables);
            SFTables tabs = new SFTables();
            tabs.RetrieveAll();
            foreach (SFTable item in tabs)
            {
                if (item.No.Contains("."))
                    continue;

                string sql = "SELECT * FROM " + item.No + " ";
                DataSet ds = new DataSet();
                ds.Tables.Add(BP.DA.DBAccess.RunSQLReturnTable(sql));
                ds.WriteXml(pathOfTables + "\\" + item.No + ".xml");
            }
            #endregion  Form data backup .

            #region 5. Backup Process .
            Flows fls = new Flows();
            fls.RetrieveAllFromDBSource();
            foreach (Flow fl in fls)
            {
                FlowSort fs = new FlowSort(fl.FK_FlowSort);
                string pathDir = path + "\\Flow\\" + fs.No + "." + fs.Name;
                if (System.IO.Directory.Exists(pathDir) == false)
                    System.IO.Directory.CreateDirectory(pathDir);

                fl.DoExpFlowXmlTemplete(pathDir);
            }
            #endregion  Backup Process .

            #region 6. Backup form .
            MapDatas mds = new MapDatas();
            mds.RetrieveAllFromDBSource();
            foreach (MapData md in mds)
            {
                if (md.FK_FrmSort.Length < 2)
                    continue;

                BP.Sys.SysFormTree fs = new SysFormTree(md.FK_FormTree);
                string pathDir = path + "\\Form\\" + fs.No + "." + fs.Name;
                if (System.IO.Directory.Exists(pathDir) == false)
                    System.IO.Directory.CreateDirectory(pathDir);
                DataSet ds = md.GenerHisDataSet();
                ds.WriteXml(pathDir + "\\" + md.Name + ".xml");
            }
            #endregion  Backup form .

            return " Successful implementation , Storage path :" + path;
        }
    }
}
