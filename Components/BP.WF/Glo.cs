using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Diagnostics;
//using Word = Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using BP.Sys;
using BP.DA;
using BP.En;
using BP;
using BP.Web;
using System.Security.Cryptography;
using System.Text;
using BP.Port;
using BP.WF.Rpt;
using BP.WF.Data;
using BP.WF.Template;

namespace BP.WF
{
    /// <summary>
    ///  Overall situation ( The method of treatment )
    /// </summary>
    public class Glo
    {
        #region  Installation / Upgrade .
        /// <summary>
        ///  Perform the upgrade 
        /// </summary>
        /// <returns></returns>
        public static string UpdataCCFlowVer()
        {
            #region  Check whether you need to upgrade , And an upgrade of the business logic .
            string val = "20141221";
            string sql = "SELECT IntVal FROM Sys_Serial WHERE CfgKey='Ver'";
            string currVer = DBAccess.RunSQLReturnStringIsNull(sql, "");
            if (currVer == val)
                return null; // Need to upgrade .

            sql = "UPDATE Sys_Serial SET IntVal=" + val + " WHERE CfgKey='Ver'";
            if (DBAccess.RunSQL(sql) == 0)
            {
                sql = "INSERT INTO Sys_Serial (CfgKey,IntVal) VALUES('Ver'," + val + ") ";
                DBAccess.RunSQL(sql);
            }
            #endregion  Check whether you need to upgrade , And an upgrade of the business logic .


            string msg = "";
            try
            {
                TransferCustom tc = new TransferCustom();
                tc.CheckPhysicsTable();

                #region  Boundary update form .2014-10-18
                MapDatas mds = new MapDatas();
                mds.RetrieveAll();

                foreach (MapData md in mds)
                    md.ResetMaxMinXY(); // Update border .
                #endregion  Boundary update form .

                #region  Basic data update .
                // Delete enumeration value , It automatically generates .
                BP.DA.DBAccess.RunSQL("DELETE from Sys_Enum WHERE EnumKey in ('StartGuideWay','" + FlowAttr.StartLimitRole + "','BillFileType','EventDoType','FormType','BatchRole','StartGuideWay','NodeFormType')");

                Node wf_Node = new Node();
                wf_Node.CheckPhysicsTable();
                //  Settings node ICON.
                sql = "UPDATE WF_Node SET ICON='/WF/Data/NodeIcon/ Check .png' WHERE ICON IS NULL";
                BP.DA.DBAccess.RunSQL(sql);

                BP.WF.Template.Ext.NodeSheet nodeSheet = new BP.WF.Template.Ext.NodeSheet();
                nodeSheet.CheckPhysicsTable();
                //  Upgrading mobile applications . 2014-08-02.
                sql = "UPDATE WF_Node SET MPhone_WorkModel=0,MPhone_SrcModel=0,MPad_WorkModel=0,MPad_SrcModel=0 WHERE MPhone_WorkModel IS NULL";
                BP.DA.DBAccess.RunSQL(sql);
                #endregion  Basic data update .

                #region  Label 
                sql = "DELETE FROM Sys_EnCfg WHERE No='BP.WF.Template.Ext.NodeSheet'";
                BP.DA.DBAccess.RunSQL(sql);
                sql = "INSERT INTO Sys_EnCfg(No,GroupTitle) VALUES ('BP.WF.Template.Ext.NodeSheet','";
                sql += "@NodeID= Basic Configuration ";
                sql += "@FormType= Form ";
                sql += "@FWCSta= Audit Components , Apply sdk Form and audit components ccform The property is set on the audit component .";
                sql += "@SendLab= Permissions button , Work node is operable to control buttons .";
                sql += "@RunModel= Run mode , Confluence points , Sons Process ";
                sql += "@AutoJumpRole0= Jump , Automatically jump rule when it comes to how to let the nodes automatically to the next step .";
                sql += "@MPhone_WorkModel= Mobile , Application settings associated with Mobile Phone Tablet .";
                sql += "@WarningDays= Check , Aging Assessment , Quality assessment .";
                //  sql += "@MsgCtrl= News , Message information flow .";
                sql += "@OfficeOpen= Document button , Only when the node is valid when the document flow ";
                sql += "')";
                BP.DA.DBAccess.RunSQL(sql);

                sql = "DELETE FROM Sys_EnCfg WHERE No='BP.WF.Template.Ext.FlowSheet'";
                BP.DA.DBAccess.RunSQL(sql);
                sql = "INSERT INTO Sys_EnCfg(No,GroupTitle) VALUES ('BP.WF.Template.Ext.FlowSheet','";
                sql += "@No= Basic Configuration ";
                sql += "@FlowRunWay= Automatically start , How to automatically initiate workflow configuration , This option should work with process services to be effective .";
                sql += "@StartLimitRole= Start limit rules ";
                sql += "@StartGuideWay= Pre-launch navigation ";
                sql += "@CFlowWay= Continuation of the process ";
                sql += "')";
                BP.DA.DBAccess.RunSQL(sql);
                #endregion

                #region  Upgrade SelectAccper
                Direction dir = new Direction();
                dir.CheckPhysicsTable();

                SelectAccper selectAccper = new SelectAccper();
                selectAccper.CheckPhysicsTable();
                #endregion

                #region  Upgrade wf-generworkerlist 2014-05-09
                GenerWorkerList gwl = new GenerWorkerList();
                gwl.CheckPhysicsTable();
                #endregion  Upgrade wf-generworkerlist 2014-05-09

                #region   Upgrade  NodeToolbar
                FrmField ff = new FrmField();
                ff.CheckPhysicsTable();

                MapAttr attr = new MapAttr();
                attr.CheckPhysicsTable();

                NodeToolbar bar = new NodeToolbar();
                bar.CheckPhysicsTable();

                BP.WF.Template.FlowFormTree tree = new BP.WF.Template.FlowFormTree();
                tree.CheckPhysicsTable();

                FrmNode nff = new FrmNode();
                nff.CheckPhysicsTable();

                SysForm ssf = new SysForm();
                ssf.CheckPhysicsTable();

                SysFormTree ssft = new SysFormTree();
                ssft.CheckPhysicsTable();

                BP.Sys.FrmAttachment ath = new FrmAttachment();
                ath.CheckPhysicsTable();

                BP.Sys.FrmField ffs = new FrmField();
                ffs.CheckPhysicsTable();
                #endregion

                #region  Carried out sql．
                BP.DA.DBAccess.RunSQL("delete  from Sys_Enum WHERE EnumKey in ('BillFileType','EventDoType','FormType','BatchRole','StartGuideWay','NodeFormType')");
                DBAccess.RunSQL("UPDATE Sys_FrmSln SET FK_Flow =(SELECT FK_FLOW FROM WF_Node WHERE NODEID=Sys_FrmSln.FK_Node) WHERE FK_Flow IS NULL");
                try
                {
                    DBAccess.RunSQL("UPDATE WF_Flow SET StartGuidePara1=StartGuidePara WHERE  " + BP.Sys.SystemConfig.AppCenterDBLengthStr + "(StartGuidePara) >=1 ");
                }
                catch
                {
                }

                try
                {
                    DBAccess.RunSQL("UPDATE WF_FrmNode SET MyPK=FK_Frm+'_'+convert(varchar,FK_Node )+'_'+FK_Flow");
                }
                catch
                {
                }
                #endregion

                #region  Check the necessary upgrades .
                // Department 
                BP.Port.Dept d = new BP.Port.Dept();
                d.CheckPhysicsTable();

                FrmWorkCheck fwc = new FrmWorkCheck();
                fwc.CheckPhysicsTable();

                BP.WF.GenerWorkFlow gwf = new BP.WF.GenerWorkFlow();
                gwf.CheckPhysicsTable();

                Flow myfl = new Flow();
                myfl.CheckPhysicsTable();

                Node nd = new Node();
                nd.CheckPhysicsTable();
                #endregion  Check the necessary upgrades .

                #region  Perform an update .wf_node
                sql = "UPDATE WF_Node SET FWCType=0 WHERE FWCType IS NULL";
                sql += "@UPDATE WF_Node SET FWC_X=0 WHERE FWC_X IS NULL";
                sql += "@UPDATE WF_Node SET FWC_Y=0 WHERE FWC_Y IS NULL";
                sql += "@UPDATE WF_Node SET FWC_W=0 WHERE FWC_W IS NULL";
                sql += "@UPDATE WF_Node SET FWC_H=0 WHERE FWC_H IS NULL";
                BP.DA.DBAccess.RunSQLs(sql);
                #endregion  Perform an update .

                #region  Execution Report Design .
                Flows fls = new Flows();
                fls.RetrieveAll();
                foreach (Flow fl in fls)
                {
                    try
                    {
                        MapRpts rpts = new MapRpts(fl.No);
                    }
                    catch
                    {
                        fl.DoCheck();
                    }
                }
                #endregion

                #region  Upgrade station within the message table  2013-10-20
                BP.WF.SMS sms = new SMS();
                sms.CheckPhysicsTable();
                #endregion

                #region  Upgrade Img
                FrmImg img = new FrmImg();
                img.CheckPhysicsTable();
                #endregion

                #region  Repair  mapattr UIHeight, UIWidth  Type error .
                switch (BP.Sys.SystemConfig.AppCenterDBType)
                {
                    case DBType.Oracle:
                        msg = "@Sys_MapAttr  Modify the field ";
                        break;
                    case DBType.MSSQL:
                        msg = "@ Modification sql server Controls the height and width of the field .";
                        DBAccess.RunSQL("ALTER TABLE Sys_MapAttr ALTER COLUMN UIWidth float");
                        DBAccess.RunSQL("ALTER TABLE Sys_MapAttr ALTER COLUMN UIHeight float");
                        break;
                    default:
                        break;
                }
                #endregion

                #region  Upgrade vocabulary 
                switch (BP.Sys.SystemConfig.AppCenterDBType)
                {
                    case DBType.Oracle:
                        int i = DBAccess.RunSQLReturnCOUNT("SELECT * FROM USER_TAB_COLUMNS WHERE TABLE_NAME = 'SYS_DEFVAL' AND COLUMN_NAME = 'PARENTNO'");
                        if (i == 0)
                        {
                            DBAccess.RunSQL("drop table Sys_DefVal");
                            DefVal dv = new DefVal();
                            dv.CheckPhysicsTable();
                        }
                        msg = "@Sys_DefVal  Modify the field ";
                        break;
                    case DBType.MSSQL:
                        msg = "@ Modification  Sys_DefVal.";
                        i = DBAccess.RunSQLReturnCOUNT("SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('Sys_DefVal') AND NAME='ParentNo'");
                        if (i == 0)
                        {
                            DBAccess.RunSQL("drop table Sys_DefVal");
                            DefVal dv = new DefVal();
                            dv.CheckPhysicsTable();
                        }
                        break;
                    default:
                        break;
                }
                #endregion

                #region  Update Error Log 
                msg = "@ Landing error .";
                DBAccess.RunSQL("DELETE FROM Sys_Enum WHERE EnumKey IN ('DeliveryWay','RunModel','OutTimeDeal','FlowAppType')");
                try
                {
                    DBAccess.RunSQL("UPDATE Port_Station SET StaGrade=FK_StationType WHERE StaGrade IS null ");
                }
                catch
                {

                }
                #endregion  Update Error Log 

                #region  Upgrade Form tree 
                //  First check if upgraded .
                sql = "SELECT * FROM Sys_FormTree WHERE No = '0'";
                DataTable formTree_dt = DBAccess.RunSQLReturnTable(sql);
                if (formTree_dt.Rows.Count == 0)
                {
                    /* Not upgraded . Increase the root */
                    SysFormTree formTree = new SysFormTree();
                    formTree.No = "0";
                    formTree.Name = " Form Library ";
                    formTree.ParentNo = "";
                    formTree.TreeNo = "0";
                    formTree.Idx = 0;
                    formTree.IsDir = true;

                    try
                    {
                        formTree.DirectInsert();
                    }
                    catch
                    {
                    }
                    // The data is transferred in the form library forms a tree 
                    SysFormTrees formSorts = new SysFormTrees();
                    formSorts.RetrieveAll();

                    foreach (SysFormTree item in formSorts)
                    {
                        if (item.No == "0")
                            continue;

                        SysFormTree subFormTree = new SysFormTree();
                        subFormTree.No = item.No;
                        subFormTree.Name = item.Name;
                        subFormTree.ParentNo = "0";
                        subFormTree.Save();
                    }
                    // Associate to form a tree form 
                    sql = "UPDATE Sys_MapData SET FK_FormTree=FK_FrmSort WHERE FK_FrmSort <> '' AND FK_FrmSort is not null";
                    DBAccess.RunSQL(sql);
                }
                #endregion

                #region  Regenerate view WF_EmpWorks,  2013-08-06.
                try
                {
                    BP.DA.DBAccess.RunSQL("DROP VIEW WF_EmpWorks");
                }
                catch
                {
                }

                try
                {
                    BP.DA.DBAccess.RunSQL("DROP VIEW WF_NodeExt");
                }
                catch
                {
                }

                // Perform the necessary sql.
                string sqlscript = BP.Sys.SystemConfig.CCFlowAppPath + "\\WF\\Data\\Install\\SQLScript\\InitCCFlowData.sql";
                BP.DA.DBAccess.RunSQLScript(sqlscript);
                #endregion

                #region  Carried out admin Landed . 2012-12-25  The new version .
                Emp emp = new Emp();
                emp.No = "admin";
                if (emp.RetrieveFromDBSources() == 1)
                {
                    BP.Web.WebUser.SignInOfGener(emp, true);
                }
                else
                {
                    emp.No = "admin";
                    emp.Name = "admin";
                    emp.FK_Dept = "01";
                    emp.Pass = "pub";
                    emp.Insert();
                    BP.Web.WebUser.SignInOfGener(emp, true);
                    //throw new Exception("admin  Users lose , Please note case .");
                }
                #endregion  Carried out admin Landed .

                #region  Repair  Sys_FrmImg  Table Fields  ImgAppType Tag0
                switch (BP.Sys.SystemConfig.AppCenterDBType)
                {
                    case DBType.Oracle:
                        int i = DBAccess.RunSQLReturnCOUNT("SELECT * FROM USER_TAB_COLUMNS WHERE TABLE_NAME = 'SYS_FRMIMG' AND COLUMN_NAME = 'TAG0'");
                        if (i == 0)
                        {
                            DBAccess.RunSQL("ALTER TABLE SYS_FRMIMG ADD (ImgAppType number,TAG0 nvarchar(500))");
                        }
                        msg = "@Sys_FrmImg  Modify the field ";
                        break;
                    case DBType.MSSQL:
                        msg = "@ Modification  Sys_FrmImg.";
                        i = DBAccess.RunSQLReturnCOUNT("SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('Sys_FrmImg') AND NAME='Tag0'");
                        if (i == 0)
                        {
                            DBAccess.RunSQL("ALTER TABLE Sys_FrmImg ADD ImgAppType int");
                            DBAccess.RunSQL("ALTER TABLE Sys_FrmImg ADD Tag0 nvarchar(500)");
                        }
                        break;
                    default:
                        break;
                }
                #endregion

                //  Returns the version number .
                return val;
            }
            catch (Exception ex)
            {
                return " Problem Source :" + ex.Message + "<hr>" + msg + "<br> Details :@" + ex.StackTrace + "<br>@<a href='../DBInstall.aspx' > Click here to upgrade to the system interface .</a>";
            }
        }
        /// <summary>
        ///  Installation package 
        /// </summary>
        /// <param name="lang"> Language </param>
        /// <param name="isDemo"> Whether installation Demo</param>
        /// <param name="isOAInstall"> Is OA Installation </param>
        public static void DoInstallDataBase(string lang, bool isDemo)
        {
            DoInstallDataBase(lang, isDemo, false);
        }
        /// <summary>
        /// CCFlowAppPath
        /// </summary>
        public static string CCFlowAppPath
        {
            get
            {
                return BP.Sys.SystemConfig.GetValByKey("CCFlowAppPath", "/");
            }
        }
        /// <summary>
        ///  Installation package 
        /// </summary>
        public static void DoInstallDataBase(string lang, bool isDemo, bool isOAInstall)
        {
            ArrayList al = null;
            string info = "BP.En.Entity";
            al = BP.En.ClassFactory.GetObjects(info);

            #region 1,  Create or Repair Table 
            foreach (Object obj in al)
            {
                Entity en = null;
                en = obj as Entity;
                if (en == null)
                    continue;

                if (isDemo == false)
                {
                    /* If you do not install demo.*/
                    string clsName = en.ToString();
                    if (clsName != null)
                    {
                        if (clsName.Contains("BP.CN")
                            || clsName.Contains("BP.Demo"))
                            continue;
                    }
                }
                if (Glo.OSModel == WF.OSModel.WorkFlow)
                {
                    /* If you do not install gpm  Put bp.gpm  Namespace excluded . */
                    string clsName = en.ToString();
                    if (clsName != null)
                    {
                        if (clsName.Contains("BP.GMP"))
                            continue;
                    }
                }

                string table = null;
                try
                {
                    table = en.EnMap.PhysicsTable;
                    if (table == null)
                        continue;
                }
                catch
                {
                    continue;
                }



                switch (table)
                {
                    case "WF_EmpWorks":
                    case "WF_GenerEmpWorkDtls":
                    case "WF_GenerEmpWorks":
                    case "WF_NodeExt":
                    case "V_FlowData":
                        continue;
                    case "Sys_Enum":
                        en.CheckPhysicsTable();
                        break;
                    default:
                        en.CheckPhysicsTable();
                        break;
                }
                en.PKVal = "123";
                try
                {
                    en.RetrieveFromDBSources();
                }
                catch (Exception ex)
                {
                    Log.DebugWriteWarning(ex.Message);
                    en.CheckPhysicsTable();
                }
            }
            #endregion  Repair 

            #region 2,  Registration enumeration type  SQL
            // 2,  Registration enumeration type .
            BP.Sys.Xml.EnumInfoXmls xmls = new BP.Sys.Xml.EnumInfoXmls();
            xmls.RetrieveAll();
            foreach (BP.Sys.Xml.EnumInfoXml xml in xmls)
            {
                BP.Sys.SysEnums ses = new BP.Sys.SysEnums();
                ses.RegIt(xml.Key, xml.Vals);
            }
            #endregion  Registration enumeration type 

            #region 3,  Perform basic  sql

            if (Glo.OSModel == BP.WF.OSModel.BPM)
            {
                /* In the case of BPM Mode */
                try
                {
                    BP.DA.DBAccess.RunSQL("DROP TABLE Port_EmpStation");
                    BP.DA.DBAccess.RunSQL("DROP TABLE Port_EmpDept");
                }
                catch
                {
                    // BP.DA.DBAccess.RunSQL("DROP VIEW Port_EmpStation");
                    //  BP.DA.DBAccess.RunSQL("DROP VIEW Port_EmpDept");
                }
            }

            if (Glo.OSModel == BP.WF.OSModel.WorkFlow)
            {
                /* Remove  BPM  Some physical table mode  */

            }

            string sqlscript = "";
            if (isOAInstall == true)
            {
            }
            else
            {
                if (Glo.OSModel == WF.OSModel.BPM)
                {
                    // Already installed on GPM  Therefore, this information does not need to install the .
                    //sqlscript = SystemConfig.PathOfWebApp + "\\WF\\Data\\Install\\SQLScript\\Port_Inc_CH_BPM.sql";
                    //BP.DA.DBAccess.RunSQLScript(sqlscript);
                }
                else
                {
                    try
                    {
                        BP.DA.DBAccess.RunSQL("DROP VIEW Port_EmpDept");
                        BP.DA.DBAccess.RunSQL("DROP VIEW Port_EmpStation");
                    }
                    catch
                    {
                        BP.Port.EmpDept ed = new BP.Port.EmpDept();
                        ed.CheckPhysicsTable();
                        BP.Port.EmpStation es = new BP.Port.EmpStation();
                        es.CheckPhysicsTable();
                    }

                    try
                    {
                        sqlscript = BP.Sys.SystemConfig.CCFlowAppPath + "\\WF\\Data\\Install\\SQLScript\\Port_Inc_CH_WorkFlow.sql";
                        BP.DA.DBAccess.RunSQLScript(sqlscript);
                    }
                    catch (Exception ex)
                    {
                        if (BP.WF.Glo.OSModel == WF.OSModel.WorkFlow)
                            throw new Exception("@  Carried out sql Failure : There may be reasons , You installed on GPM, Also installed ccflow, But web.config The model is not BPM Mode , You need to modify web.config OSModel=1");
                        throw ex;
                    }
                    sqlscript = BP.Sys.SystemConfig.CCFlowAppPath + "\\WF\\Data\\Install\\SQLScript\\Port_Inc_CH_WorkFlow.sql";
                    BP.DA.DBAccess.RunSQLScript(sqlscript);
                }
            }

            if (isDemo == false)
            {
                SysFormTree frmSort = new SysFormTree();
                frmSort.No = "01";
                frmSort.Name = " Forms category 1";
                frmSort.Insert();
            }
            #endregion  Repair 

            #region 4,  Creating a view with data .
            // Perform the necessary sql.
            sqlscript = BP.Sys.SystemConfig.CCFlowAppPath + "\\WF\\Data\\Install\\SQLScript\\InitCCFlowData.sql";
            BP.DA.DBAccess.RunSQLScript(sqlscript);
            #endregion  Creating a view with data 

            #region 5,  Initialization data .
            if (isDemo)
            {
                sqlscript = SystemConfig.PathOfData + "\\Install\\SQLScript\\InitPublicData.sql";
                BP.DA.DBAccess.RunSQLScript(sqlscript);
            }
            else
            {
                FlowSort fs = new FlowSort();
                fs.No = "02";
                fs.ParentNo = "99";
                fs.Name = " Other categories ";
                fs.DirectInsert();
            }
            #endregion  Initialization data 

            #region 6,  Generate temporary wf Data .
            if (isDemo)
            {
                BP.Port.Emps emps = new BP.Port.Emps();
                emps.RetrieveAllFromDBSource();
                int i = 0;
                foreach (BP.Port.Emp emp in emps)
                {
                    i++;
                    BP.WF.Port.WFEmp wfEmp = new BP.WF.Port.WFEmp();
                    wfEmp.Copy(emp);
                    wfEmp.No = emp.No;

                    if (wfEmp.Email.Length == 0)
                        wfEmp.Email = emp.No + "@ccflow.org";

                    if (wfEmp.Tel.Length == 0)
                        wfEmp.Tel = "82374939-6" + i.ToString().PadLeft(2, '0');

                    if (wfEmp.IsExits)
                        wfEmp.Update();
                    else
                        wfEmp.Insert();
                }

                //  CV data generation .
                int oid = 1000;
                foreach (BP.Port.Emp emp in emps)
                {
                    //for (int myIdx = 0; myIdx < 6; myIdx++)
                    //{
                    //    BP.WF.Demo.Resume re = new Demo.Resume();
                    //    re.NianYue = "200" + myIdx + "年01月";
                    //    re.FK_Emp = emp.No;
                    //    re.GongZuoDanWei = " Departments -" + myIdx;
                    //    re.ZhengMingRen = "张" + myIdx;
                    //    re.BeiZhu = emp.Name + " Comrade work seriously .";
                    //    oid++;
                    //    re.InsertAsOID(oid);
                    //}
                }
                //  Generate annual month data .
                string sql = "";
                DateTime dtNow = DateTime.Now;
                for (int num = 0; num < 12; num++)
                {
                    sql = "INSERT INTO Pub_NY (No,Name) VALUES ('" + dtNow.ToString("yyyy-MM") + "','" + dtNow.ToString("yyyy-MM") + "')";
                    dtNow = dtNow.AddMonths(1);
                }
            }
            #endregion  Initialization data 

            #region  Execution supplement sql,  Let the foreign key field length are set to 100.
            DBAccess.RunSQL("UPDATE Sys_MapAttr SET maxlen=100 WHERE LGType=2 AND MaxLen<100");
            DBAccess.RunSQL("UPDATE Sys_MapAttr SET maxlen=100 WHERE KeyOfEn='FK_Dept'");

            Nodes nds = new Nodes();
            nds.RetrieveAll();
            foreach (Node nd in nds)
                nd.HisWork.CheckPhysicsTable();

            #endregion  Execution supplement sql,  Let the foreign key field length are set to 100.

            //  Remove the blank field grouping .
            BP.WF.DTS.DeleteBlankGroupField dts = new DTS.DeleteBlankGroupField();
            dts.Do();
        }

        /// <summary>
        ///  Installation CCIM
        /// </summary>
        /// <param name="lang"></param>
        /// <param name="yunXingHuanjing"></param>
        /// <param name="isDemo"></param>
        public static void DoInstallCCIM()
        {
            string sqlscript = SystemConfig.PathOfData + "Install\\SQLScript\\CCIM.sql";
            BP.DA.DBAccess.RunSQLScriptGo(sqlscript);
        }
        public static void KillProcess(string processName) // Kill the process approach 
        {
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process pro in processes)
            {
                string name = pro.ProcessName + ".exe";
                if (name.ToLower() == processName.ToLower())
                    pro.Kill();
            }
        }
        /// <summary>
        ///  Generate a new number 
        /// </summary>
        /// <param name="rptKey"></param>
        /// <returns></returns>
        public static string GenerFlowNo(string rptKey)
        {
            rptKey = rptKey.Replace("ND", "");
            rptKey = rptKey.Replace("Rpt", "");
            switch (rptKey.Length)
            {
                case 0:
                    return "001";
                case 1:
                    return "00" + rptKey;
                case 2:
                    return "0" + rptKey;
                case 3:
                    return rptKey;
                default:
                    return "001";
            }
            return rptKey;
        }
        /// <summary>
        /// 
        /// </summary>
        public static bool IsShowFlowNum
        {
            get
            {
                switch (SystemConfig.AppSettings["IsShowFlowNum"])
                {
                    case "1":
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        ///  Produce word File .
        /// </summary>
        /// <param name="wk"></param>
        public static void GenerWord(object filename, Work wk)
        {
            BP.WF.Glo.KillProcess("WINWORD.EXE");
            string enName = wk.EnMap.PhysicsTable;
            try
            {
                RegistryKey delKey = Registry.LocalMachine.OpenSubKey(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Shared Tools\Text Converters\Import\",
                    true);
                delKey.DeleteValue("MSWord6.wpc");
                delKey.Close();
            }
            catch
            {
            }

            GroupField currGF = new GroupField();
            MapAttrs mattrs = new MapAttrs(enName);
            GroupFields gfs = new GroupFields(enName);
            MapDtls dtls = new MapDtls(enName);
            foreach (MapDtl dtl in dtls)
                dtl.IsUse = false;

            //  Calculated from the number of rows of cells .
            int rowNum = 0;
            foreach (GroupField gf in gfs)
            {
                rowNum++;
                bool isLeft = true;
                foreach (MapAttr attr in mattrs)
                {
                    if (attr.UIVisible == false)
                        continue;

                    if (attr.GroupID != gf.OID)
                        continue;

                    if (attr.UIIsLine)
                    {
                        rowNum++;
                        isLeft = true;
                        continue;
                    }

                    if (isLeft == false)
                        rowNum++;
                    isLeft = !isLeft;
                }
            }

            rowNum = rowNum + 2 + dtls.Count;

            //  Create Word File 
            string CheckedInfo = "";
            string message = "";
            Object Nothing = System.Reflection.Missing.Value;

            #region  Useless code 
            //  object filename = fileName;

            //Word.Application WordApp = new Word.ApplicationClass();
            //Word.Document WordDoc = WordApp.Documents.Add(ref  Nothing, ref  Nothing, ref  Nothing, ref  Nothing);
            //try
            //{
            //    WordApp.ActiveWindow.View.Type = Word.WdViewType.wdOutlineView;
            //    WordApp.ActiveWindow.View.SeekView = Word.WdSeekView.wdSeekPrimaryHeader;

            //    #region  Increase Header 
            //    //  Add Header   Insert Picture 
            //    string pict = SystemConfig.PathOfDataUser + "log.jpg"; //  Pictures of the path 
            //    if (System.IO.File.Exists(pict))
            //    {
            //        System.Drawing.Image img = System.Drawing.Image.FromFile(pict);
            //        object LinkToFile = false;
            //        object SaveWithDocument = true;
            //        object Anchor = WordDoc.Application.Selection.Range;
            //        WordDoc.Application.ActiveDocument.InlineShapes.AddPicture(pict, ref  LinkToFile,
            //            ref  SaveWithDocument, ref  Anchor);
            //        //    WordDoc.Application.ActiveDocument.InlineShapes[1].Width = img.Width; //  Image Width 
            //        //    WordDoc.Application.ActiveDocument.InlineShapes[1].Height = img.Height; //  Image Height 
            //    }
            //    WordApp.ActiveWindow.ActivePane.Selection.InsertAfter("[ Gallop Business Process Management System  http://ccFlow.org]");
            //    WordApp.Selection.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft; //  Set right-aligned 
            //    WordApp.ActiveWindow.View.SeekView = Word.WdSeekView.wdSeekMainDocument; //  Jump Header Settings 
            //    WordApp.Selection.ParagraphFormat.LineSpacing = 15f; //  Set document line spacing 
            //    #endregion

            //    //  Move the focus and wrap 
            //    object count = 14;
            //    object WdLine = Word.WdUnits.wdLine; //  Change row ;
            //    WordApp.Selection.MoveDown(ref  WdLine, ref  count, ref  Nothing); //  Mobile Focus 
            //    WordApp.Selection.TypeParagraph(); //  Insert a paragraph 

            //    //  Create a spreadsheet document 
            //    Word.Table newTable = WordDoc.Tables.Add(WordApp.Selection.Range, rowNum, 4, ref  Nothing, ref  Nothing);

            //    //  Set table style 
            //    newTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleThickThinLargeGap;
            //    newTable.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;

            //    newTable.Columns[1].Width = 100f;
            //    newTable.Columns[2].Width = 100f;
            //    newTable.Columns[3].Width = 100f;
            //    newTable.Columns[4].Width = 100f;

            //    //  Fill table of contents 
            //    newTable.Cell(1, 1).Range.Text = wk.EnDesc;
            //    newTable.Cell(1, 1).Range.Bold = 2; //  Set cell bold font 

            //    //  Merge Cells 
            //    newTable.Cell(1, 1).Merge(newTable.Cell(1, 4));
            //    WordApp.Selection.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter; //  Vertical Centers 
            //    WordApp.Selection.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter; //  Horizontal Centers 

            //    int groupIdx = 1;
            //    foreach (GroupField gf in gfs)
            //    {
            //        groupIdx++;
            //        //  Fill table of contents 
            //        newTable.Cell(groupIdx, 1).Range.Text = gf.Lab;
            //        newTable.Cell(groupIdx, 1).Range.Font.Color = Word.WdColor.wdColorDarkBlue; //  Set within the cell font color 
            //        newTable.Cell(groupIdx, 1).Shading.BackgroundPatternColor = Word.WdColor.wdColorGray25;
            //        //  Merge Cells 
            //        newTable.Cell(groupIdx, 1).Merge(newTable.Cell(groupIdx, 4));
            //        WordApp.Selection.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            //        groupIdx++;

            //        bool isLeft = true;
            //        bool isColumns2 = false;
            //        int currColumnIndex = 0;
            //        foreach (MapAttr attr in mattrs)
            //        {
            //            if (attr.UIVisible == false)
            //                continue;

            //            if (attr.GroupID != gf.OID)
            //                continue;

            //            if (newTable.Rows.Count < groupIdx)
            //                continue;

            //            #region  Increase from the table 
            //            foreach (MapDtl dtl in dtls)
            //            {
            //                if (dtl.IsUse)
            //                    continue;

            //                if (dtl.RowIdx != groupIdx - 3)
            //                    continue;

            //                if (gf.OID != dtl.GroupID)
            //                    continue;

            //                GEDtls dtlsDB = new GEDtls(dtl.No);
            //                QueryObject qo = new QueryObject(dtlsDB);
            //                switch (dtl.DtlOpenType)
            //                {
            //                    case DtlOpenType.ForEmp:
            //                        qo.AddWhere(GEDtlAttr.RefPK, wk.OID);
            //                        break;
            //                    case DtlOpenType.ForWorkID:
            //                        qo.AddWhere(GEDtlAttr.RefPK, wk.OID);
            //                        break;
            //                    case DtlOpenType.ForFID:
            //                        qo.AddWhere(GEDtlAttr.FID, wk.OID);
            //                        break;
            //                }
            //                qo.DoQuery();

            //                newTable.Rows[groupIdx].SetHeight(100f, Word.WdRowHeightRule.wdRowHeightAtLeast);
            //                newTable.Cell(groupIdx, 1).Merge(newTable.Cell(groupIdx, 4));

            //                Attrs dtlAttrs = dtl.GenerMap().Attrs;
            //                int colNum = 0;
            //                foreach (Attr attrDtl in dtlAttrs)
            //                {
            //                    if (attrDtl.UIVisible == false)
            //                        continue;
            //                    colNum++;
            //                }

            //                newTable.Cell(groupIdx, 1).Select();
            //                WordApp.Selection.Delete(ref Nothing, ref Nothing);
            //                Word.Table newTableDtl = WordDoc.Tables.Add(WordApp.Selection.Range, dtlsDB.Count + 1, colNum,
            //                    ref Nothing, ref Nothing);

            //                newTableDtl.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            //                newTableDtl.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;

            //                int colIdx = 1;
            //                foreach (Attr attrDtl in dtlAttrs)
            //                {
            //                    if (attrDtl.UIVisible == false)
            //                        continue;
            //                    newTableDtl.Cell(1, colIdx).Range.Text = attrDtl.Desc;
            //                    colIdx++;
            //                }

            //                int idxRow = 1;
            //                foreach (GEDtl item in dtlsDB)
            //                {
            //                    idxRow++;
            //                    int columIdx = 0;
            //                    foreach (Attr attrDtl in dtlAttrs)
            //                    {
            //                        if (attrDtl.UIVisible == false)
            //                            continue;
            //                        columIdx++;

            //                        if (attrDtl.IsFKorEnum)
            //                            newTableDtl.Cell(idxRow, columIdx).Range.Text = item.GetValRefTextByKey(attrDtl.Key);
            //                        else
            //                        {
            //                            if (attrDtl.MyDataType == DataType.AppMoney)
            //                                newTableDtl.Cell(idxRow, columIdx).Range.Text = item.GetValMoneyByKey(attrDtl.Key).ToString("0.00");
            //                            else
            //                                newTableDtl.Cell(idxRow, columIdx).Range.Text = item.GetValStrByKey(attrDtl.Key);

            //                            if (attrDtl.IsNum)
            //                                newTableDtl.Cell(idxRow, columIdx).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
            //                        }
            //                    }
            //                }

            //                groupIdx++;
            //                isLeft = true;
            //            }
            //            #endregion  Increase from the table 

            //            if (attr.UIIsLine)
            //            {
            //                currColumnIndex = 0;
            //                isLeft = true;
            //                if (attr.IsBigDoc)
            //                {
            //                    newTable.Rows[groupIdx].SetHeight(100f, Word.WdRowHeightRule.wdRowHeightAtLeast);
            //                    newTable.Cell(groupIdx, 1).Merge(newTable.Cell(groupIdx, 4));
            //                    newTable.Cell(groupIdx, 1).Range.Text = attr.Name + ":\r\n" + wk.GetValStrByKey(attr.KeyOfEn);
            //                }
            //                else
            //                {
            //                    newTable.Cell(groupIdx, 2).Merge(newTable.Cell(groupIdx, 4));
            //                    newTable.Cell(groupIdx, 1).Range.Text = attr.Name;
            //                    newTable.Cell(groupIdx, 2).Range.Text = wk.GetValStrByKey(attr.KeyOfEn);
            //                }
            //                groupIdx++;
            //                continue;
            //            }
            //            else
            //            {
            //                if (attr.IsBigDoc)
            //                {
            //                    if (currColumnIndex == 2)
            //                    {
            //                        currColumnIndex = 0;
            //                    }

            //                    newTable.Rows[groupIdx].SetHeight(100f, Word.WdRowHeightRule.wdRowHeightAtLeast);
            //                    if (currColumnIndex == 0)
            //                    {
            //                        newTable.Cell(groupIdx, 1).Merge(newTable.Cell(groupIdx, 2));
            //                        newTable.Cell(groupIdx, 1).Range.Text = attr.Name + ":\r\n" + wk.GetValStrByKey(attr.KeyOfEn);
            //                        currColumnIndex = 3;
            //                        continue;
            //                    }
            //                    else if (currColumnIndex == 3)
            //                    {
            //                        newTable.Cell(groupIdx, 2).Merge(newTable.Cell(groupIdx, 3));
            //                        newTable.Cell(groupIdx, 2).Range.Text = attr.Name + ":\r\n" + wk.GetValStrByKey(attr.KeyOfEn);
            //                        currColumnIndex = 0;
            //                        groupIdx++;
            //                        continue;
            //                    }
            //                    else
            //                    {
            //                        continue;
            //                    }
            //                }
            //                else
            //                {
            //                    string s = "";
            //                    if (attr.LGType == FieldTypeS.Normal)
            //                    {
            //                        if (attr.MyDataType == DataType.AppMoney)
            //                            s = wk.GetValDecimalByKey(attr.KeyOfEn).ToString("0.00");
            //                        else
            //                            s = wk.GetValStrByKey(attr.KeyOfEn);
            //                    }
            //                    else
            //                    {
            //                        s = wk.GetValRefTextByKey(attr.KeyOfEn);
            //                    }

            //                    switch (currColumnIndex)
            //                    {
            //                        case 0:
            //                            newTable.Cell(groupIdx, 1).Range.Text = attr.Name;
            //                            if (attr.IsSigan)
            //                            {
            //                                string path = BP.Sys.SystemConfig.PathOfDataUser + "\\Siganture\\" + s + ".jpg";
            //                                if (System.IO.File.Exists(path))
            //                                {
            //                                    System.Drawing.Image img = System.Drawing.Image.FromFile(path);
            //                                    object LinkToFile = false;
            //                                    object SaveWithDocument = true;
            //                                    //object Anchor = WordDoc.Application.Selection.Range;
            //                                    object Anchor = newTable.Cell(groupIdx, 2).Range;

            //                                    WordDoc.Application.ActiveDocument.InlineShapes.AddPicture(path, ref  LinkToFile,
            //                                        ref  SaveWithDocument, ref  Anchor);
            //                                    //    WordDoc.Application.ActiveDocument.InlineShapes[1].Width = img.Width; //  Image Width 
            //                                    //    WordDoc.Application.ActiveDocument.InlineShapes[1].Height = img.Height; //  Image Height 
            //                                }
            //                                else
            //                                {
            //                                    newTable.Cell(groupIdx, 2).Range.Text = s;
            //                                }
            //                            }
            //                            else
            //                            {
            //                                if (attr.IsNum)
            //                                {
            //                                    newTable.Cell(groupIdx, 2).Range.Text = s;
            //                                    newTable.Cell(groupIdx, 2).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
            //                                }
            //                                else
            //                                {
            //                                    newTable.Cell(groupIdx, 2).Range.Text = s;
            //                                }
            //                            }
            //                            currColumnIndex = 1;
            //                            continue;
            //                            break;
            //                        case 1:
            //                            newTable.Cell(groupIdx, 3).Range.Text = attr.Name;
            //                            if (attr.IsSigan)
            //                            {
            //                                string path = BP.Sys.SystemConfig.PathOfDataUser + "\\Siganture\\" + s + ".jpg";
            //                                if (System.IO.File.Exists(path))
            //                                {
            //                                    System.Drawing.Image img = System.Drawing.Image.FromFile(path);
            //                                    object LinkToFile = false;
            //                                    object SaveWithDocument = true;
            //                                    object Anchor = newTable.Cell(groupIdx, 4).Range;
            //                                    WordDoc.Application.ActiveDocument.InlineShapes.AddPicture(path, ref  LinkToFile,
            //                                        ref  SaveWithDocument, ref  Anchor);
            //                                }
            //                                else
            //                                {
            //                                    newTable.Cell(groupIdx, 4).Range.Text = s;
            //                                }
            //                            }
            //                            else
            //                            {
            //                                if (attr.IsNum)
            //                                {
            //                                    newTable.Cell(groupIdx, 4).Range.Text = s;
            //                                    newTable.Cell(groupIdx, 4).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
            //                                }
            //                                else
            //                                {
            //                                    newTable.Cell(groupIdx, 4).Range.Text = s;
            //                                }
            //                            }
            //                            currColumnIndex = 0;
            //                            groupIdx++;
            //                            continue;
            //                            break;
            //                        default:
            //                            break;
            //                    }
            //                }
            //            }
            //        }
            //    }  // End loop 

            //    #region  Add footer 
            //    WordApp.ActiveWindow.View.SeekView = Word.WdSeekView.wdSeekPrimaryFooter;
            //    WordApp.ActiveWindow.ActivePane.Selection.InsertAfter(" Template by ccflow Automatic generation , Rigorous reprint . Details of this process, please visit  http://doc.ccFlow.org.  Construction process management system, please call : 0531-82374939  ");
            //    WordApp.Selection.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
            //    #endregion

            //    //  File Save 
            //    WordDoc.SaveAs(ref  filename, ref  Nothing, ref  Nothing, ref  Nothing,
            //        ref  Nothing, ref  Nothing, ref  Nothing, ref  Nothing,
            //        ref  Nothing, ref  Nothing, ref  Nothing, ref  Nothing, ref  Nothing,
            //        ref  Nothing, ref  Nothing, ref  Nothing);

            //    WordDoc.Close(ref  Nothing, ref  Nothing, ref  Nothing);
            //    WordApp.Quit(ref  Nothing, ref  Nothing, ref  Nothing);
            //    try
            //    {
            //        string docFile = filename.ToString();
            //        string pdfFile = docFile.Replace(".doc", ".pdf");
            //        Glo.Rtf2PDF(docFile, pdfFile);
            //    }
            //    catch (Exception ex)
            //    {
            //        BP.DA.Log.DebugWriteInfo("@ Generate pdf Failure ." + ex.Message);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //    // WordApp.Quit(ref  Nothing, ref  Nothing, ref  Nothing);
            //    WordDoc.Close(ref  Nothing, ref  Nothing, ref  Nothing);
            //    WordApp.Quit(ref  Nothing, ref  Nothing, ref  Nothing);
            //}
            #endregion
        }
        #endregion  Installation .

        #region  Global approach to 
        public static List<string> FlowFields
        {
            get
            {
                return typeof(GERptAttr).GetFields().Select(o => o.Name).ToList();
            }
        }
        /// <summary>
        ///  According to word processing Cc , And the sender 
        /// </summary>
        /// <param name="note"></param>
        /// <param name="emps"></param>
        public static void DealNote(string note, BP.Port.Emps emps)
        {
            note = " General knowledge of the reading . Lijiang Long He Shi . Please Wang Wei , CEREALS instructions .";
            note = note.Replace(" Access to knowledge ", " Access to knowledge @");

            note = note.Replace("请", "@");
            note = note.Replace("呈", "@");
            note = note.Replace("报", "@");
            string[] strs = note.Split('@');

            string ccTo = "";
            string sendTo = "";
            foreach (string str in strs)
            {
                if (string.IsNullOrEmpty(str))
                    continue;

                if (str.Contains(" Access to knowledge ") == true
                    || str.Contains(" Degree reading ") == true)
                {
                    /* Cc .*/
                    foreach (BP.Port.Emp emp in emps)
                    {
                        if (str.Contains(emp.No) == false)
                            continue;
                        ccTo += emp.No + ",";
                    }
                    continue;
                }

                if (str.Contains(" Read at ") == true
                  || str.Contains(" Read Office ") == true)
                {
                    /* Send send .*/
                    foreach (BP.Port.Emp emp in emps)
                    {
                        if (str.Contains(emp.No) == false)
                            continue;
                        sendTo += emp.No + ",";
                    }
                    continue;
                }
            }
        }



        #region  Entity and process events .
        private static Hashtable Htable_FlowFEE = null;
        /// <summary>
        ///  Get node event entity 
        /// </summary>
        /// <param name="enName"> Instance name </param>
        /// <returns> Get node event entity , If you do not return empty .</returns>
        public static FlowEventBase GetFlowEventEntityByEnName(string enName)
        {
            if (Htable_FlowFEE == null || Htable_FlowFEE.Count == 0)
            {
                Htable_FlowFEE = new Hashtable();
                ArrayList al = BP.En.ClassFactory.GetObjects("BP.WF.FlowEventBase");
                foreach (FlowEventBase en in al)
                {
                    Htable_FlowFEE.Add(en.ToString(), en);
                }
            }
            FlowEventBase myen = Htable_FlowFEE[enName] as FlowEventBase;
            if (myen == null)
                throw new Exception("@ Gets an entity instance error process events according to the class name :" + enName + ", There is no such entity found .");
            return myen;
        }
        /// <summary>
        ///  Get node by node coding entity event .
        /// </summary>
        /// <param name="NodeMark"> Node coding </param>
        /// <returns> Return entities , Or null</returns>
        public static FlowEventBase GetFlowEventEntityByFlowMark(string flowMark)
        {
            if (Htable_FlowFEE == null || Htable_FlowFEE.Count == 0)
            {
                Htable_FlowFEE = new Hashtable();
                ArrayList al = BP.En.ClassFactory.GetObjects("BP.WF.FlowEventBase");
                Htable_FlowFEE.Clear();
                foreach (FlowEventBase en in al)
                {
                    Htable_FlowFEE.Add(en.ToString(), en);
                }
            }

            foreach (string key in Htable_FlowFEE.Keys)
            {
                FlowEventBase fee = Htable_FlowFEE[key] as FlowEventBase;
                if (fee.FlowMark == flowMark)
                    return fee;
            }

            //for (int i = 0; i < Htable_FlowFEE.Count; i++)
            //{
            //    FlowEventBase fee = Htable_FlowFEE[i] as FlowEventBase;
            //}
            return null;
        }
        #endregion  Entity and process events .

        /// <summary>
        ///  After the execution of business logic to send the work process 
        ///  After the event calls for the process to send .
        ///  If the process fails , Will throw an exception .
        /// </summary>
        public static void DealBuinessAfterSendWork(string fk_flow, Int64 workid,
            string doFunc, string WorkIDs, string cFlowNo, int cNodeID, string cEmp)
        {
            if (doFunc == "SetParentFlow")
            {
                /*  If you need to set up child-parent process information .
                 *  Applied to the merger approval , When multiple sub-processes merger approval , A parent process initiated after approval .
                 */
                string[] workids = WorkIDs.Split(',');
                string okworkids = ""; // After successfully sent workids.
                GenerWorkFlow gwf = new GenerWorkFlow();
                foreach (string id in workids)
                {
                    if (string.IsNullOrEmpty(id))
                        continue;

                    //  The data copy To the inside , Let the child process can also get the parent process data .
                    Int64 workidC = Int64.Parse(id);

                    // Set the current process ID
                    BP.WF.Dev2Interface.SetParentInfo(cFlowNo, workidC, fk_flow, workid, cNodeID, cEmp);

                    //  Determine whether they can perform , Should not be executed to send down .
                    gwf.WorkID = workidC;
                    if (gwf.RetrieveFromDBSources() == 0)
                        continue;

                    //  Whether it can be executed ?
                    if (BP.WF.Dev2Interface.Flow_IsCanDoCurrentWork(gwf.FK_Flow, gwf.FK_Node, workidC, WebUser.No) == false)
                        continue;

                    // Execution is sent down .
                    try
                    {
                        BP.WF.Dev2Interface.Node_SendWork(cFlowNo, workidC);
                        okworkids += workidC;
                    }
                    catch (Exception ex)
                    {
                        #region  If there is a transmission failure , Concerning the revocation of sub-processes and the parent process .
                        // First, the main flow of revocation sent .
                        BP.WF.Dev2Interface.Flow_DoUnSend(fk_flow, workid);

                        // The sub-processes has been sent successfully sent revocation .
                        string[] myokwokid = okworkids.Split(',');
                        foreach (string okwokid in myokwokid)
                        {
                            if (string.IsNullOrEmpty(id))
                                continue;

                            //  The data copy To the inside , Let the child process can also get the parent process data .
                            workidC = Int64.Parse(id);
                            BP.WF.Dev2Interface.Flow_DoUnSend(cFlowNo, workidC);
                        }
                        #endregion  If there is a transmission failure , Concerning the revocation of sub-processes and the parent process .
                        throw new Exception("@ In the implementation of the sub-process (" + gwf.Title + ") The following error occurred while sending :" + ex.Message);
                    }
                }
            }

        }
        #endregion  Global approach to 

        #region web.config  Property .
        /// <summary>
        ///  Whether admin
        /// </summary>
        public static bool IsAdmin
        {
            get
            {
                string s = BP.Sys.SystemConfig.AppSettings["adminers"];
                if (string.IsNullOrEmpty(s))
                    s = "admin,";
                return s.Contains(BP.Web.WebUser.No);
            }
        }
        /// <summary>
        ///  Get mapdata Field inquiry Like.
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="colName"> Column number </param>
        public static string MapDataLikeKey(string flowNo, string colName)
        {
            flowNo = int.Parse(flowNo).ToString();
            string len = BP.Sys.SystemConfig.AppCenterDBLengthStr;
            if (flowNo.Length == 1)
                return " " + colName + " LIKE 'ND" + flowNo + "%' AND " + len + "(" + colName + ")=5";
            if (flowNo.Length == 2)
                return " " + colName + " LIKE 'ND" + flowNo + "%' AND " + len + "(" + colName + ")=6";
            if (flowNo.Length == 3)
                return " " + colName + " LIKE 'ND" + flowNo + "%' AND " + len + "(" + colName + ")=7";

            return " " + colName + " LIKE 'ND" + flowNo + "%' AND " + len + "(" + colName + ")=8";
        }
        /// <summary>
        ///  From time to send SMS 
        ///  Default from  8  Point .
        /// </summary>
        public static int SMSSendTimeFromHour
        {
            get
            {
                try
                {
                    return int.Parse(BP.Sys.SystemConfig.AppSettings["SMSSendTimeFromHour"]);
                }
                catch
                {
                    return 8;
                }
            }
        }
        /// <summary>
        ///  Time to send messages to 
        ///  Defaults to  20  End Point .
        /// </summary>
        public static int SMSSendTimeToHour
        {
            get
            {
                try
                {
                    return int.Parse(BP.Sys.SystemConfig.AppSettings["SMSSendTimeToHour"]);
                }
                catch
                {
                    return 8;
                }
            }
        }
        #endregion webconfig Property .

        #region  Common method 
        private static string html = "";
        private static ArrayList htmlArr = new ArrayList();
        private static string backHtml = "";
        private static Int64 workid = 0;
        /// <summary>
        ///  Simulation run 
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="empNo"> Personnel to perform .</param>
        /// <returns> Execution information .</returns>
        public static string Simulation_RunOne(string flowNo, string empNo, string paras)
        {
            backHtml = "";// Need to re-assign a null value 
            Hashtable ht = null;
            if (string.IsNullOrEmpty(paras) == false)
            {
                AtPara ap = new AtPara(paras);
                ht = ap.HisHT;
            }

            Emp emp = new Emp(empNo);
            backHtml += " ****  Started :" + Glo.GenerUserImgSmallerHtml(emp.No, emp.Name) + " Login simulation workflow execution ";
            BP.WF.Dev2Interface.Port_Login(empNo);

            workid = BP.WF.Dev2Interface.Node_CreateBlankWork(flowNo, ht, null, emp.No, null);
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(flowNo, workid, ht);
            backHtml += objs.ToMsgOfHtml().Replace("@", "<br>@");  // Record messages .


            string[] accepters = objs.VarAcceptersID.Split(',');


            foreach (string acce in accepters)
            {
                if (string.IsNullOrEmpty(acce) == true)
                    continue;

                //  Performing transmission .
                Simulation_Run_S1(flowNo, workid, acce, ht, empNo);
                break;
            }
            //return html;
            //return htmlArr;
            return backHtml;
        }
        private static bool isAdd = true;
        private static void Simulation_Run_S1(string flowNo, Int64 workid, string empNo, Hashtable ht, string beginEmp)
        {
            //htmlArr.Add(html);
            Emp emp = new Emp(empNo);
            //html = "";
            backHtml += "empNo" + beginEmp;
            backHtml += "<br> **** 让:" + Glo.GenerUserImgSmallerHtml(emp.No, emp.Name) + " Performs analog Login . ";
            //  Let Login .
            BP.WF.Dev2Interface.Port_Login(empNo);

            // Performing transmission .
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(flowNo, workid, ht);
            backHtml += "<br>" + objs.ToMsgOfHtml().Replace("@", "<br>@");

            if (objs.VarAcceptersID == null)
            {
                isAdd = false;
                backHtml += " <br> ****  Process ends , Check out <a href='/WF/WFRpt.aspx?WorkID=" + workid + "&FK_Flow=" + flowNo + "' target=_blank > Process track </a> ====";
                //htmlArr.Add(html);
                //backHtml += "nextEmpNo";
                return;
            }

            if (string.IsNullOrEmpty(objs.VarAcceptersID))// Here to add an empty judgment , Skip the implementation of the following methods , Otherwise an error .
            {
                return;
            }
            string[] accepters = objs.VarAcceptersID.Split(',');

            foreach (string acce in accepters)
            {
                if (string.IsNullOrEmpty(acce) == true)
                    continue;

                // Performing transmission .
                Simulation_Run_S1(flowNo, workid, acce, ht, beginEmp);
                break; // Not allowed to perform the .
            }
        }
        /// <summary>
        ///  Whether mobile access ?
        /// </summary>
        /// <returns></returns>
        public static bool IsMobile()
        {
            if (SystemConfig.IsBSsystem == false)
                return false;

            string agent = (BP.Sys.Glo.Request.UserAgent + "").ToLower().Trim();
            if (agent == "" || agent.IndexOf("mozilla") != -1 || agent.IndexOf("opera") != -1)
                return false;
            return true;
        }
        /// <summary>
        ///  Generated document number 
        /// </summary>
        /// <param name="billFormat"></param>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string GenerBillNo(string billNo, Int64 workid, Entity en, string flowPTable)
        {
            if (string.IsNullOrEmpty(billNo))
                return "";
            /* In case ,Bill  There are rules  */
            billNo = billNo.Replace("{YYYY}", DateTime.Now.ToString("yyyy"));
            billNo = billNo.Replace("{yyyy}", DateTime.Now.ToString("yyyy"));

            billNo = billNo.Replace("{yy}", DateTime.Now.ToString("yy"));
            billNo = billNo.Replace("{YY}", DateTime.Now.ToString("YY"));

            billNo = billNo.Replace("{MM}", DateTime.Now.ToString("MM"));
            billNo = billNo.Replace("{DD}", DateTime.Now.ToString("DD"));
            billNo = billNo.Replace("{dd}", DateTime.Now.ToString("dd"));
            billNo = billNo.Replace("{HH}", DateTime.Now.ToString("HH"));
            billNo = billNo.Replace("{mm}", DateTime.Now.ToString("mm"));
            billNo = billNo.Replace("{LSH}", workid.ToString());
            billNo = billNo.Replace("{WorkID}", workid.ToString());
            billNo = billNo.Replace("{OID}", workid.ToString());

            if (billNo.Contains("@WebUser.DeptZi"))
            {
                string val = DBAccess.RunSQLReturnStringIsNull("SELECT Zi FROM Port_Dept where no='" + WebUser.FK_Dept + "'", "");
                billNo = billNo.Replace("@WebUser.DeptZi", val.ToString());
            }

            if (billNo.Contains("{ParentBillNo}"))
            {
                string pWorkID = DBAccess.RunSQLReturnStringIsNull("SELECT PWorkID FROM " + flowPTable + " WHERE OID=" + workid, "0");
                string parentBillNo = DBAccess.RunSQLReturnStringIsNull("SELECT BillNo FROM WF_GenerWorkFlow WHERE WorkID=" + pWorkID, "");
                billNo = billNo.Replace("{ParentBillNo}", parentBillNo);

                string sql = "";
                int num = 0;
                for (int i = 2; i < 7; i++)
                {
                    if (billNo.Contains("{LSH" + i + "}") == false)
                        continue;

                    sql = "SELECT COUNT(*) FROM " + flowPTable + " WHERE PWorkID =" + pWorkID;
                    num = BP.DA.DBAccess.RunSQLReturnValInt(sql, 0);
                    billNo = billNo + num.ToString().PadLeft(i, '0');
                    billNo = billNo.Replace("{LSH" + i + "}", "");
                    break;
                }
            }
            else
            {
                string sql = "";
                int num = 0;
                for (int i = 2; i < 7; i++)
                {
                    if (billNo.Contains("{LSH" + i + "}") == false)
                        continue;

                    billNo = billNo.Replace("{LSH" + i + "}", "");
                    sql = "SELECT COUNT(*) FROM " + flowPTable + " WHERE BillNo LIKE '" + billNo + "%'";
                    num = BP.DA.DBAccess.RunSQLReturnValInt(sql, 0) + 1;
                    billNo = billNo + num.ToString().PadLeft(i, '0');
                }
            }
            return billNo;
        }
        /// <summary>
        ///  Join track
        /// </summary>
        /// <param name="at"> Event Type </param>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="workID"> The work ID</param>
        /// <param name="fid"> Process ID</param>
        /// <param name="fromNodeID"> From the node number </param>
        /// <param name="fromNodeName"> From the node name </param>
        /// <param name="fromEmpID"> From staff ID</param>
        /// <param name="fromEmpName"> From the name of the person </param>
        /// <param name="toNodeID"> To the node number </param>
        /// <param name="toNodeName"> To the node name </param>
        /// <param name="toEmpID"> To staff ID</param>
        /// <param name="toEmpName"> The person name </param>
        /// <param name="note"> News </param>
        /// <param name="tag"> Parameters @ Separate </param>
        public static void AddToTrack(ActionType at, string flowNo, Int64 workID, Int64 fid, int fromNodeID, string fromNodeName, string fromEmpID, string fromEmpName,
            int toNodeID, string toNodeName, string toEmpID, string toEmpName, string note, string tag)
        {
            if (toNodeID == 0)
            {
                toNodeID = fromNodeID;
                toNodeName = fromNodeName;
            }

            Track t = new Track();
            t.WorkID = workID;
            t.FID = fid;
            t.RDT = DataType.CurrentDataTimess;
            t.HisActionType = at;

            t.NDFrom = fromNodeID;
            t.NDFromT = fromNodeName;

            t.EmpFrom = fromEmpID;
            t.EmpFromT = fromEmpName;
            t.FK_Flow = flowNo;

            t.NDTo = toNodeID;
            t.NDToT = toNodeName;

            t.EmpTo = toEmpID;
            t.EmpToT = toEmpName;
            t.Msg = note;

            // Parameters .
            if (tag != null)
                t.Tag = tag;

            try
            {
                t.Insert();
            }
            catch
            {
                t.CheckPhysicsTable();
                t.Insert();
            }
        }
        /// <summary>
        ///  Whether by calculation expression ( Or is correct .)
        /// </summary>
        /// <param name="exp"> Expression </param>
        /// <param name="en"> Entity </param>
        /// <returns>true/false</returns>
        public static bool ExeExp(string exp, Entity en)
        {
            exp = exp.Replace("@WebUser.No", WebUser.No);
            exp = exp.Replace("@WebUser.Name", WebUser.Name);
            exp = exp.Replace("@WebUser.FK_Dept", WebUser.FK_Dept);
            exp = exp.Replace("@WebUser.FK_DeptName", WebUser.FK_DeptName);


            string[] strs = exp.Split(' ');
            bool isPass = false;

            string key = strs[0].Trim();
            string oper = strs[1].Trim();
            string val = strs[2].Trim();
            val = val.Replace("'", "");
            val = val.Replace("%", "");
            val = val.Replace("~", "");
            BP.En.Row row = en.Row;
            foreach (string item in row.Keys)
            {
                if (key != item.Trim())
                    continue;

                string valPara = row[key].ToString();
                if (oper == "=")
                {
                    if (valPara == val)
                        return true;
                }

                if (oper.ToUpper() == "LIKE")
                {
                    if (valPara.Contains(val))
                        return true;
                }

                if (oper == ">")
                {
                    if (float.Parse(valPara) > float.Parse(val))
                        return true;
                }
                if (oper == ">=")
                {
                    if (float.Parse(valPara) >= float.Parse(val))
                        return true;
                }
                if (oper == "<")
                {
                    if (float.Parse(valPara) < float.Parse(val))
                        return true;
                }
                if (oper == "<=")
                {
                    if (float.Parse(valPara) <= float.Parse(val))
                        return true;
                }

                if (oper == "!=")
                {
                    if (float.Parse(valPara) != float.Parse(val))
                        return true;
                }

                throw new Exception("@ Parameter format error :" + exp + " Key=" + key + " oper=" + oper + " Val=" + val);
            }

            return false;
        }
        /// <summary>
        ///  Handle expressions 
        /// </summary>
        /// <param name="exp"> Expression </param>
        /// <param name="en"> Data Sources </param>
        /// <param name="errInfo"> Error </param>
        /// <returns></returns>
        public static string DealExp(string exp, Entity en, string errInfo)
        {
            exp = exp.Replace("~", "'");

            exp = exp.Replace("@WebUser.No;", WebUser.No);
            exp = exp.Replace("@WebUser.Name;", WebUser.Name);
            exp = exp.Replace("@WebUser.FK_Dept;", WebUser.FK_Dept);
            exp = exp.Replace("@WebUser.FK_DeptName;", WebUser.FK_DeptName);

            exp = exp.Replace("@WebUser.No", WebUser.No);
            exp = exp.Replace("@WebUser.Name", WebUser.Name);
            exp = exp.Replace("@WebUser.FK_Dept", WebUser.FK_Dept);
            exp = exp.Replace("@WebUser.FK_DeptName", WebUser.FK_DeptName);

            if (exp.Contains("@") == false)
            {
                exp = exp.Replace("~", "'");
                //exp = exp.Replace("''", "'");
                //exp = exp.Replace("''", "'");
                // exp = exp.Replace("='", "=''");
                //exp = exp.Replace("= '", "=''");
                //  Special judge .
                //   exp = exp.Replace("=''0'", "='0'");
                //  exp = exp.Replace("=' ", "='' ");
                return exp;
            }

            // Added support for new rules . @MyField;  Format .
            foreach (Attr item in en.EnMap.Attrs)
            {
                if (exp.Contains("@" + item.Key + ";"))
                    exp = exp.Replace("@" + item.Key + ";", en.GetValStrByKey(item.Key));
            }
            if (exp.Contains("@") == false)
                return exp;

            #region  Solve scheduling problems .
            Attrs attrs = en.EnMap.Attrs;
            string mystrs = "";
            foreach (Attr attr in attrs)
            {
                if (attr.MyDataType == DataType.AppString)
                    mystrs += "@" + attr.Key + ",";
                else
                    mystrs += "@" + attr.Key;
            }
            string[] strs = mystrs.Split('@');
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("No", typeof(string)));
            foreach (string str in strs)
            {
                if (string.IsNullOrEmpty(str))
                    continue;

                DataRow dr = dt.NewRow();
                dr[0] = str;
                dt.Rows.Add(dr);
            }
            DataView dv = dt.DefaultView;
            dv.Sort = "No DESC";
            DataTable dtNew = dv.Table;
            #endregion   Solve scheduling problems .

            #region  Substitution variables .
            foreach (DataRow dr in dtNew.Rows)
            {
                string key = dr[0].ToString();
                bool isStr = key.Contains(",");
                if (isStr == true)
                {
                    key = key.Replace(",", "");
                    exp = exp.Replace("@" + key, en.GetValStrByKey(key));
                }
                else
                {
                    exp = exp.Replace("@" + key, en.GetValStrByKey(key));
                }
            }

            //  Deal with Para Replacement .
            if (exp.Contains("@") && Glo.SendHTOfTemp != null)
            {
                foreach (string key in Glo.SendHTOfTemp.Keys)
                    exp = exp.Replace("@" + key, Glo.SendHTOfTemp[key].ToString());
            }

            if (exp.Contains("@"))
            {
                Log.DefaultLogWriteLineError(exp);
                // throw new Exception("@ccflow的(" + errInfo + ") Expression error , Some fields do not replace down , Please confirm whether the field is removed :" + exp);
            }
            #endregion

            exp = exp.Replace("~", "'");
            //exp = exp.Replace("''", "'");
            //exp = exp.Replace("''", "'");
            //exp = exp.Replace("=' ", "=''");
            //exp = exp.Replace("= ' ", "=''");
            return exp;
        }
        /// <summary>
        ///  Encryption MD5
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GenerMD5(BP.WF.Work wk)
        {
            string s = null;
            foreach (Attr attr in wk.EnMap.Attrs)
            {
                switch (attr.Key)
                {
                    case WorkAttr.MD5:
                    case WorkAttr.RDT:
                    case WorkAttr.CDT:
                    case WorkAttr.Rec:
                    case StartWorkAttr.Title:
                    case StartWorkAttr.Emps:
                    case StartWorkAttr.FK_Dept:
                    case StartWorkAttr.PRI:
                    case StartWorkAttr.FID:
                        continue;
                    default:
                        break;
                }

                string obj = attr.DefaultVal as string;
                //if (obj == null)
                //    continue;
                if (obj != null && obj.Contains("@"))
                    continue;

                s += wk.GetValStrByKey(attr.Key);
            }
            s += "ccflow";
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(s, "MD5").ToLower();
        }
        /// <summary>
        ///  Loading process data  
        /// </summary>
        /// <param name="xlsFile"></param>
        public static string LoadFlowDataWithToSpecNode(string xlsFile)
        {
            DataTable dt = BP.DA.DBLoad.GetTableByExt(xlsFile);
            string err = "";
            string info = "";

            foreach (DataRow dr in dt.Rows)
            {
                string flowPK = dr["FlowPK"].ToString();
                string starter = dr["Starter"].ToString();
                string executer = dr["Executer"].ToString();
                int toNode = int.Parse(dr["ToNodeID"].ToString().Replace("ND", ""));
                Node nd = new Node();
                nd.NodeID = toNode;
                if (nd.RetrieveFromDBSources() == 0)
                {
                    err += " Node ID Error :" + toNode;
                    continue;
                }
                string sql = "SELECT count(*) as Num FROM ND" + int.Parse(nd.FK_Flow) + "01 WHERE FlowPK='" + flowPK + "'";
                int i = DBAccess.RunSQLReturnValInt(sql);
                if (i == 1)
                    continue; //  This data has been scheduled .

                #region  Check the data is complete .
                BP.Port.Emp emp = new BP.Port.Emp();
                emp.No = executer;
                if (emp.RetrieveFromDBSources() == 0)
                {
                    err += "@ Account number :" + starter + ", Does not exist .";
                    continue;
                }
                if (string.IsNullOrEmpty(emp.FK_Dept))
                {
                    err += "@ Account number :" + starter + ", No department .";
                    continue;
                }

                emp.No = starter;
                if (emp.RetrieveFromDBSources() == 0)
                {
                    err += "@ Account number :" + executer + ", Does not exist .";
                    continue;
                }
                if (string.IsNullOrEmpty(emp.FK_Dept))
                {
                    err += "@ Account number :" + executer + ", No department .";
                    continue;
                }
                #endregion  Check the data is complete .

                BP.Web.WebUser.SignInOfGener(emp);
                Flow fl = nd.HisFlow;
                Work wk = fl.NewWork();

                Attrs attrs = wk.EnMap.Attrs;
                //foreach (Attr attr in wk.EnMap.Attrs)
                //{
                //}

                foreach (DataColumn dc in dt.Columns)
                {
                    Attr attr = attrs.GetAttrByKey(dc.ColumnName.Trim());
                    if (attr == null)
                        continue;

                    string val = dr[dc.ColumnName].ToString().Trim();
                    switch (attr.MyDataType)
                    {
                        case DataType.AppString:
                        case DataType.AppDate:
                        case DataType.AppDateTime:
                            wk.SetValByKey(attr.Key, val);
                            break;
                        case DataType.AppInt:
                        case DataType.AppBoolean:
                            wk.SetValByKey(attr.Key, int.Parse(val));
                            break;
                        case DataType.AppMoney:
                        case DataType.AppDouble:
                        case DataType.AppRate:
                        case DataType.AppFloat:
                            wk.SetValByKey(attr.Key, decimal.Parse(val));
                            break;
                        default:
                            wk.SetValByKey(attr.Key, val);
                            break;
                    }
                }

                wk.SetValByKey(WorkAttr.Rec, BP.Web.WebUser.No);
                wk.SetValByKey(StartWorkAttr.FK_Dept, BP.Web.WebUser.FK_Dept);
                wk.SetValByKey("FK_NY", DataType.CurrentYearMonth);
                wk.SetValByKey(WorkAttr.MyNum, 1);
                wk.Update();

                Node ndStart = nd.HisFlow.HisStartNode;
                WorkNode wn = new WorkNode(wk, ndStart);
                try
                {
                    info += "<hr>" + wn.NodeSend(nd, executer).ToMsgOfHtml();
                }
                catch (Exception ex)
                {
                    err += "<hr>" + ex.Message;
                    WorkFlow wf = new WorkFlow(fl, wk.OID);
                    wf.DoDeleteWorkFlowByReal(true);
                    continue;
                }

                #region  Update   Next node data .
                Work wkNext = nd.HisWork;
                wkNext.OID = wk.OID;
                wkNext.RetrieveFromDBSources();
                attrs = wkNext.EnMap.Attrs;
                foreach (DataColumn dc in dt.Columns)
                {
                    Attr attr = attrs.GetAttrByKey(dc.ColumnName.Trim());
                    if (attr == null)
                        continue;

                    string val = dr[dc.ColumnName].ToString().Trim();
                    switch (attr.MyDataType)
                    {
                        case DataType.AppString:
                        case DataType.AppDate:
                        case DataType.AppDateTime:
                            wkNext.SetValByKey(attr.Key, val);
                            break;
                        case DataType.AppInt:
                        case DataType.AppBoolean:
                            wkNext.SetValByKey(attr.Key, int.Parse(val));
                            break;
                        case DataType.AppMoney:
                        case DataType.AppDouble:
                        case DataType.AppRate:
                        case DataType.AppFloat:
                            wkNext.SetValByKey(attr.Key, decimal.Parse(val));
                            break;
                        default:
                            wkNext.SetValByKey(attr.Key, val);
                            break;
                    }
                }

                wkNext.DirectUpdate();

                GERpt rtp = fl.HisGERpt;
                rtp.SetValByKey("OID", wkNext.OID);
                rtp.RetrieveFromDBSources();
                rtp.Copy(wkNext);
                rtp.DirectUpdate();

                #endregion  Update   Next node data .
            }
            return info + err;
        }
        public static string LoadFlowDataWithToSpecEndNode(string xlsFile)
        {
            DataTable dt = BP.DA.DBLoad.GetTableByExt(xlsFile);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            ds.WriteXml("C:\\ Completed .xml");

            string err = "";
            string info = "";
            int idx = 0;
            foreach (DataRow dr in dt.Rows)
            {
                string flowPK = dr["FlowPK"].ToString().Trim();
                if (string.IsNullOrEmpty(flowPK))
                    continue;

                string starter = dr["Starter"].ToString();
                string executer = dr["Executer"].ToString();
                int toNode = int.Parse(dr["ToNodeID"].ToString().Replace("ND", ""));
                Node ndOfEnd = new Node();
                ndOfEnd.NodeID = toNode;
                if (ndOfEnd.RetrieveFromDBSources() == 0)
                {
                    err += " Node ID Error :" + toNode;
                    continue;
                }

                if (ndOfEnd.IsEndNode == false)
                {
                    err += " Node ID Error :" + toNode + ",  Non-end node .";
                    continue;
                }

                string sql = "SELECT count(*) as Num FROM ND" + int.Parse(ndOfEnd.FK_Flow) + "01 WHERE FlowPK='" + flowPK + "'";
                int i = DBAccess.RunSQLReturnValInt(sql);
                if (i == 1)
                    continue; //  This data has been scheduled .

                #region  Check the data is complete .
                // Initiating sponsor .
                BP.Port.Emp emp = new BP.Port.Emp();
                emp.No = executer;
                if (emp.RetrieveFromDBSources() == 0)
                {
                    err += "@ Account number :" + starter + ", Does not exist .";
                    continue;
                }

                if (string.IsNullOrEmpty(emp.FK_Dept))
                {
                    err += "@ Account number :" + starter + ", Department is not set .";
                    continue;
                }

                emp = new BP.Port.Emp();
                emp.No = starter;
                if (emp.RetrieveFromDBSources() == 0)
                {
                    err += "@ Account number :" + starter + ", Does not exist .";
                    continue;
                }
                else
                {
                    emp.RetrieveFromDBSources();
                    if (string.IsNullOrEmpty(emp.FK_Dept))
                    {
                        err += "@ Account number :" + starter + ", Department is not set .";
                        continue;
                    }
                }
                #endregion  Check the data is complete .


                BP.Web.WebUser.SignInOfGener(emp);
                Flow fl = ndOfEnd.HisFlow;
                Work wk = fl.NewWork();
                foreach (DataColumn dc in dt.Columns)
                    wk.SetValByKey(dc.ColumnName.Trim(), dr[dc.ColumnName].ToString().Trim());

                wk.SetValByKey(WorkAttr.Rec, BP.Web.WebUser.No);
                wk.SetValByKey(StartWorkAttr.FK_Dept, BP.Web.WebUser.FK_Dept);
                wk.SetValByKey("FK_NY", DataType.CurrentYearMonth);
                wk.SetValByKey(WorkAttr.MyNum, 1);
                wk.Update();

                Node ndStart = fl.HisStartNode;
                WorkNode wn = new WorkNode(wk, ndStart);
                try
                {
                    info += "<hr>" + wn.NodeSend(ndOfEnd, executer).ToMsgOfHtml();
                }
                catch (Exception ex)
                {
                    err += "<hr> Startup Errors :" + ex.Message;
                    DBAccess.RunSQL("DELETE FROM ND" + int.Parse(ndOfEnd.FK_Flow) + "01 WHERE FlowPK='" + flowPK + "'");
                    WorkFlow wf = new WorkFlow(fl, wk.OID);
                    wf.DoDeleteWorkFlowByReal(true);
                    continue;
                }

                // End end point .
                emp = new BP.Port.Emp(executer);
                BP.Web.WebUser.SignInOfGener(emp);

                Work wkEnd = ndOfEnd.GetWork(wk.OID);
                foreach (DataColumn dc in dt.Columns)
                    wkEnd.SetValByKey(dc.ColumnName.Trim(), dr[dc.ColumnName].ToString().Trim());

                wkEnd.SetValByKey(WorkAttr.Rec, BP.Web.WebUser.No);
                wkEnd.SetValByKey(StartWorkAttr.FK_Dept, BP.Web.WebUser.FK_Dept);
                wkEnd.SetValByKey("FK_NY", DataType.CurrentYearMonth);
                wkEnd.SetValByKey(WorkAttr.MyNum, 1);
                wkEnd.Update();

                try
                {
                    WorkNode wnEnd = new WorkNode(wkEnd, ndOfEnd);
                    //  wnEnd.AfterNodeSave();
                    info += "<hr>" + wnEnd.NodeSend().ToMsgOfHtml();
                }
                catch (Exception ex)
                {
                    err += "<hr> End Error ( System simply delete it ):" + ex.Message;
                    WorkFlow wf = new WorkFlow(fl, wk.OID);
                    wf.DoDeleteWorkFlowByReal(true);
                    continue;
                }
            }
            return info + err;
        }
        //public static void ResetFlowView()
        //{
        //    string sql = "DROP VIEW V_WF_Data ";
        //    try
        //    {
        //        BP.DA.DBAccess.RunSQL(sql);
        //    }
        //    catch
        //    {
        //    }

        //    Flows fls = new Flows();
        //    fls.RetrieveAll();
        //    sql = "CREATE VIEW V_WF_Data AS ";
        //    foreach (Flow fl in fls)
        //    {
        //        fl.CheckRpt();
        //        sql += "\t\n SELECT '" + fl.No + "' as FK_Flow, '" + fl.Name + "' AS FlowName, '" + fl.FK_FlowSort + "' as FK_FlowSort,CDT,Emps,FID,FK_Dept,FK_NY,";
        //        sql += "MyNum,OID,RDT,Rec,Title,WFState,FlowEmps,";
        //        sql += "FlowStarter,FlowStartRDT,FlowEnder,FlowEnderRDT,FlowDaySpan FROM ND" + int.Parse(fl.No) + "Rpt";
        //        sql += "\t\n  UNION";
        //    }
        //    sql = sql.Substring(0, sql.Length - 6);
        //    sql += "\t\n GO";
        //    BP.DA.DBAccess.RunSQL(sql);
        //}
        public static void Rtf2PDF(object pathOfRtf, object pathOfPDF)
        {
            //        Object Nothing = System.Reflection.Missing.Value;
            //        // Create a file called WordApp Component Object     
            //        Microsoft.Office.Interop.Word.Application wordApp =
            //new Microsoft.Office.Interop.Word.ApplicationClass();
            //        // Create a file called WordDoc And open the document object     
            //        Microsoft.Office.Interop.Word.Document doc = wordApp.Documents.Open(ref pathOfRtf, ref Nothing, ref Nothing, ref Nothing, ref Nothing,
            // ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing,
            //ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);

            //        // The format settings are saved     
            //        object filefarmat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;

            //        // Save as PDF    
            //        doc.SaveAs(ref pathOfPDF, ref filefarmat, ref Nothing, ref Nothing, ref Nothing, ref Nothing,
            //ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing,
            // ref Nothing, ref Nothing, ref Nothing);
            //        // Close the document object     
            //        doc.Close(ref Nothing, ref Nothing, ref Nothing);
            //        // Launched formation     
            //        wordApp.Quit(ref Nothing, ref Nothing, ref Nothing);
            //        GC.Collect();
        }
        #endregion  Common method 

        #region  Property 
        public static string SessionMsg
        {
            get
            {
                Paras p = new Paras();
                p.SQL = "SELECT Msg FROM WF_Emp where No=" + SystemConfig.AppCenterDBVarStr + "FK_Emp";
                p.AddFK_Emp();
                return DBAccess.RunSQLReturnString(p);

                //string SQL = "SELECT Msg FROM WF_Emp where No='"+BP.Web.WebUser.No+"'";
                //return DBAccess.RunSQLReturnString(SQL);
            }
            set
            {
                if (string.IsNullOrEmpty(value) == true)
                    return;

                Paras p = new Paras();
                p.SQL = "UPDATE WF_Emp SET Msg=" + SystemConfig.AppCenterDBVarStr + "v WHERE No=" + SystemConfig.AppCenterDBVarStr + "FK_Emp";
                p.AddFK_Emp();
                p.Add("v", value);
                DBAccess.RunSQL(p);

                //string SQL = "UPDATE WF_Emp SET Msg='" + value + "' WHERE No='" + BP.Web.WebUser.No + "'";
                //DBAccess.RunSQL(SQL);
            }
        }
        #endregion  Property 

        #region  Property 
        private static string _FromPageType = null;
        public static string FromPageType
        {
            get
            {
                _FromPageType = null;
                if (_FromPageType == null)
                {
                    try
                    {
                        string url = BP.Sys.Glo.Request.RawUrl;
                        int i = url.LastIndexOf("/") + 1;
                        int i2 = url.IndexOf(".aspx") - 6;

                        url = url.Substring(i);
                        url = url.Substring(0, url.IndexOf(".aspx"));
                        _FromPageType = url;
                        if (_FromPageType.Contains("SmallSingle"))
                            _FromPageType = "SmallSingle";
                        else if (_FromPageType.Contains("Small"))
                            _FromPageType = "Small";
                        else
                            _FromPageType = "";
                    }
                    catch (Exception ex)
                    {
                        _FromPageType = "";
                        //  throw new Exception(ex.Message + url + " i=" + i + " i2=" + i2);
                    }
                }
                return _FromPageType;
            }
        }
        /// <summary>
        ///  Send a temporary variable transmission .
        /// </summary>
        public static Hashtable SendHTOfTemp = null;
        /// <summary>
        ///  Report Properties collection 
        /// </summary>
        private static Attrs _AttrsOfRpt = null;
        /// <summary>
        ///  Report Properties collection 
        /// </summary>
        public static Attrs AttrsOfRpt
        {
            get
            {
                if (_AttrsOfRpt == null)
                {
                    _AttrsOfRpt = new Attrs();
                    _AttrsOfRpt.AddTBInt(GERptAttr.OID, 0, "WorkID", true, true);
                    _AttrsOfRpt.AddTBInt(GERptAttr.FID, 0, "FlowID", false, false);

                    _AttrsOfRpt.AddTBString(GERptAttr.Title, null, " Title ", true, false, 0, 10, 10);
                    _AttrsOfRpt.AddTBString(GERptAttr.FlowStarter, null, " Sponsor ", true, false, 0, 10, 10);
                    _AttrsOfRpt.AddTBString(GERptAttr.FlowStartRDT, null, " Start Time ", true, false, 0, 10, 10);
                    _AttrsOfRpt.AddTBString(GERptAttr.WFState, null, " Status ", true, false, 0, 10, 10);

                    //Attr attr = new Attr();
                    //attr.Desc = " Process Status ";
                    //attr.Key = "WFState";
                    //attr.MyFieldType = FieldType.Enum;
                    //attr.UIBindKey = "WFState";
                    //attr.UITag = "@0= In progress @1= Has been completed ";

                    _AttrsOfRpt.AddDDLSysEnum(GERptAttr.WFState, 0, " Process Status ", true, true, GERptAttr.WFState);
                    _AttrsOfRpt.AddTBString(GERptAttr.FlowEmps, null, " Participants ", true, false, 0, 10, 10);
                    _AttrsOfRpt.AddTBString(GERptAttr.FlowEnder, null, " End people ", true, false, 0, 10, 10);
                    _AttrsOfRpt.AddTBString(GERptAttr.FlowEnderRDT, null, " End Time ", true, false, 0, 10, 10);
                    _AttrsOfRpt.AddTBDecimal(GERptAttr.FlowEndNode, 0, " End node ", true, false);
                    _AttrsOfRpt.AddTBDecimal(GERptAttr.FlowDaySpan, 0, " Span (days)", true, false);
                    //_AttrsOfRpt.AddTBString(GERptAttr.FK_NY, null, " Membership Month ", true, false, 0, 10, 10);
                }
                return _AttrsOfRpt;
            }
        }
        #endregion  Property 


        #region  Other configurations .
        public static string GenerHelp(string helpId)
        {
            return "";
            switch (helpId)
            {
                case "Bill":
                    return "<a href=\"http://ccFlow.org\"  target=_blank><img src='../../WF/Img/FileType/rm.gif' border=0/> Operating Video </a>";
                case "FAppSet":
                    return "<a href=\"http://ccFlow.org\"  target=_blank><img src='../../WF/Img/FileType/rm.gif' border=0/> Operating Video </a>";
                default:
                    return "<a href=\"http://ccFlow.org\"  target=_blank><img src='../../WF/Img/FileType/rm.gif' border=0/> Operating Video </a>";
                    break;
            }
        }
        public static string NodeImagePath
        {
            get
            {
                return Glo.IntallPath + "\\Data\\Node\\";
            }
        }
        public static void ClearDBData()
        {
            string sql = "DELETE FROM WF_GenerWorkFlow WHERE fk_flow not in (select no from wf_flow )";
            BP.DA.DBAccess.RunSQL(sql);

            sql = "DELETE FROM WF_GenerWorkerlist WHERE fk_flow not in (select no from wf_flow )";
            BP.DA.DBAccess.RunSQL(sql);
        }
        public static string OEM_Flag = "CCS";
        public static string FlowFileBill
        {
            get { return Glo.IntallPath + "\\DataUser\\Bill\\"; }
        }
        private static string _IntallPath = null;
        public static string IntallPath
        {
            get
            {
                if (_IntallPath == null)
                {
                    _IntallPath = BP.WF.Glo.CCFlowAppPath;
                    //   throw new Exception("@ You do not  _IntallPath  Assignment .");
                }
                return _IntallPath;
            }
            set
            {
                _IntallPath = value;
            }
        }
        private static string _ServerIP = null;
        public static string ServerIP
        {
            get
            {
                if (_ServerIP == null)
                {
                    string ip = "127.0.0.1";
                    System.Net.IPAddress[] addressList = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList;
                    if (addressList.Length > 1)
                        _ServerIP = addressList[1].ToString();
                    else
                        _ServerIP = addressList[0].ToString();
                }
                return _ServerIP;
            }
            set
            {
                _ServerIP = value;
            }
        }
        /// <summary>
        ///  Process controller buttons 
        /// </summary>
        public static string FlowCtrlBtnPos
        {
            get
            {
                string s = BP.Sys.SystemConfig.AppSettings["FlowCtrlBtnPos"] as string;
                if (s == null || s == "Top")
                    return "Top";
                return "Bottom";
            }
        }
        /// <summary>
        ///  Global security code 
        /// </summary>
        public static string GloSID
        {
            get
            {
                string s = BP.Sys.SystemConfig.AppSettings["GloSID"] as string;
                if (s == null || s == "")
                    s = "sdfq2erre-2342-234sdf23423-323";
                return s;
            }
        }
        /// <summary>
        ///  Whether to enable the user to check the status of ?
        ///  If you enable :在MyFlow.aspx Each time the user checks whether the current state of banned 
        /// 用, If you disable it can not perform any operations of the . When enabled , It means that every time 
        ///  Access database .
        /// </summary>
        public static bool IsEnableCheckUseSta
        {
            get
            {
                string s = BP.Sys.SystemConfig.AppSettings["IsEnableCheckUseSta"] as string;
                if (s == null || s == "0")
                    return false;
                return true;
            }
        }
        /// <summary>
        ///  Check whether the current user is still valid to use ?
        /// </summary>
        /// <returns></returns>
        public static bool CheckIsEnableWFEmp()
        {
            Paras ps = new Paras();
            ps.SQL = "SELECT UseSta FROM WF_Emp WHERE No=" + SystemConfig.AppCenterDBVarStr + "FK_Emp";
            ps.AddFK_Emp();
            string s = DBAccess.RunSQLReturnStringIsNull(ps, "1");
            if (s == "1" || s == null)
                return true;
            return false;
        }
        /// <summary>
        ///  Language 
        /// </summary>
        public static string Language = "CH";
        public static bool IsQL
        {
            get
            {
                string s = BP.Sys.SystemConfig.AppSettings["IsQL"];
                if (s == null || s == "0")
                    return false;
                return true;
            }
        }
        /// <summary>
        ///  Whether to enable shared task pool ?
        /// </summary>
        public static bool IsEnableTaskPool
        {
            get
            {
                return BP.Sys.SystemConfig.GetValByKeyBoolen("IsEnableTaskPool", false);
            }
        }
        /// <summary>
        ///  Whether to display the title 
        /// </summary>
        public static bool IsShowTitle
        {
            get
            {
                return BP.Sys.SystemConfig.GetValByKeyBoolen("IsShowTitle", false);
            }
        }
        /// <summary>
        ///  Whether it is working to increase a priority 
        /// </summary>
        public static bool IsEnablePRI
        {
            get
            {
                return BP.Sys.SystemConfig.GetValByKeyBoolen("IsEnablePRI", false);
            }
        }
        /// <summary>
        ///  User Information Display Format 
        /// </summary>
        public static UserInfoShowModel UserInfoShowModel
        {
            get
            {
                return (UserInfoShowModel)BP.Sys.SystemConfig.GetValByKeyInt("UserInfoShowModel", 0);
            }
        }
        /// <summary>
        ///  User generated digital signature 
        /// </summary>
        /// <returns></returns>
        public static string GenerUserSigantureHtml(string serverPath, string userNo, string userName)
        {
            string siganturePath = serverPath + "/" + CCFlowAppPath + "DataUser/Siganture/" + userNo + ".jpg";
            if (System.IO.File.Exists(siganturePath))
            {
                return "<img src='" + CCFlowAppPath + "DataUser/Siganture/" + userNo + ".jpg' width='90' height='30' title='" + userName + "' border=0 onerror=\"src='" + CCFlowAppPath + "DataUser/UserICON/DefaultSmaller.png'\" />";
            }
            return "<img src='" + CCFlowAppPath + "DataUser/UserICON/" + userNo + "Smaller.png' border=0 width='24px' onerror=\"src='" + CCFlowAppPath + "DataUser/UserICON/DefaultSmaller.png'\" />" + userName;
        }
        /// <summary>
        ///  User generated small pictures 
        /// </summary>
        /// <returns></returns>
        public static string GenerUserImgSmallerHtml(string userNo, string userName)
        {
            return "<img src='" + CCFlowAppPath + "DataUser/UserICON/" + userNo + "Smaller.png' border=0 width='24px' onerror=\"src='" + CCFlowAppPath + "DataUser/UserICON/DefaultSmaller.png'\" />" + userName;
        }
        /// <summary>
        ///  Update the main table SQL
        /// </summary>
        public static string UpdataMainDeptSQL
        {
            get
            {
                return BP.Sys.SystemConfig.GetValByKey("UpdataMainDeptSQL", "UPDATE Port_Emp SET FK_Dept=" + BP.Sys.SystemConfig.AppCenterDBVarStr + "FK_Dept WHERE No=" + BP.Sys.SystemConfig.AppCenterDBVarStr + "No");
            }
        }
        /// <summary>
        ///  Update SID的SQL
        /// </summary>
        public static string UpdataSID
        {
            get
            {
                return BP.Sys.SystemConfig.GetValByKey("UpdataSID", "UPDATE Port_Emp SET SID=" + BP.Sys.SystemConfig.AppCenterDBVarStr + "SID WHERE No=" + BP.Sys.SystemConfig.AppCenterDBVarStr + "No");
            }
        }
        /// <summary>
        ///  Download sl Address 
        /// </summary>
        public static string SilverlightDownloadUrl
        {
            get
            {
                return BP.Sys.SystemConfig.GetValByKey("SilverlightDownloadUrl", "http://go.microsoft.com/fwlink/?LinkID=124807");
            }
        }
        /// <summary>
        ///  Processing and display format 
        /// </summary>
        /// <param name="no"></param>
        /// <param name="name"></param>
        /// <returns> Reality format </returns>
        public static string DealUserInfoShowModel(string no, string name)
        {
            switch (BP.WF.Glo.UserInfoShowModel)
            {
                case UserInfoShowModel.UserIDOnly:
                    return "(" + no + ")";
                case UserInfoShowModel.UserIDUserName:
                    return "(" + no + "," + name + ")";
                case UserInfoShowModel.UserNameOnly:
                    return "(" + name + ")";
                default:
                    throw new Exception("@ Format types do not judge .");
                    break;
            }
        }
        /// <summary>
        ///  Run mode 
        /// </summary>
        public static OSModel OSModel
        {
            get
            {
                OSModel os = (OSModel)BP.Sys.SystemConfig.GetValByKeyInt("OSModel", 0);
                return os;
            }
        }
        /// <summary>
        ///  Is Group uses 
        /// </summary>
        public static bool IsUnit
        {
            get
            {
                return BP.Sys.SystemConfig.GetValByKeyBoolen("IsUnit", false);
            }
        }
        /// <summary>
        ///  Whether the system is enabled 
        /// </summary>
        public static bool IsEnableZhiDu
        {
            get
            {
                return BP.Sys.SystemConfig.GetValByKeyBoolen("IsEnableZhiDu", false);
            }
        }
        /// <summary>
        ///  Whether to enable the recipient picker 
        /// </summary>
        public static bool IsEnableDoAccepter
        {
            get
            {
                return BP.Sys.SystemConfig.GetValByKeyBoolen("IsEnableDoAccepter", false);
            }
        }
        /// <summary>
        ///  Whether the draft enabled 
        /// </summary>
        public static bool IsEnableDraft
        {
            get
            {
                return BP.Sys.SystemConfig.GetValByKeyBoolen("IsEnableDraft", false);
            }
        }
        /// <summary>
        ///  Delete registry data flow ?
        /// </summary>
        public static bool IsDeleteGenerWorkFlow
        {
            get
            {
                return BP.Sys.SystemConfig.GetValByKeyBoolen("IsDeleteGenerWorkFlow", false);
            }
        }
        /// <summary>
        ///  Fill in the form field to check whether the tree is empty 
        /// </summary>
        public static bool IsEnableCheckFrmTreeIsNull
        {
            get
            {
                return BP.Sys.SystemConfig.GetValByKeyBoolen("IsEnableCheckFrmTreeIsNull", true);
            }
        }

        /// <summary>
        ///  Whether to start work opens a new window 
        /// </summary>
        public static int IsWinOpenStartWork
        {
            get
            {
                return BP.Sys.SystemConfig.GetValByKeyInt("IsWinOpenStartWork", 1);
            }
        }
        /// <summary>
        ///  Whether to open the work to be done to open the window 
        /// </summary>
        public static bool IsWinOpenEmpWorks
        {
            get
            {
                return BP.Sys.SystemConfig.GetValByKeyBoolen("IsWinOpenEmpWorks", true);
            }
        }
        /// <summary>
        ///  Whether the message system messages enabled .
        /// </summary>
        public static bool IsEnableSysMessage
        {
            get
            {
                return BP.Sys.SystemConfig.GetValByKeyBoolen("IsEnableSysMessage", true);
            }
        }
        /// <summary>
        /// ccim Integrated database .
        ///  In order to ccim Write messages .
        /// </summary>
        public static string CCIMDBName
        {
            get
            {
                string baseUrl = BP.Sys.SystemConfig.AppSettings["CCIMDBName"];
                if (string.IsNullOrEmpty(baseUrl) == true)
                    baseUrl = "ccPort.dbo";
                return baseUrl;
            }
        }
        /// <summary>
        ///  Host computer 
        /// </summary>
        public static string HostURL
        {
            get
            {
                string baseUrl = BP.Sys.SystemConfig.AppSettings["HostURL"];
                if (string.IsNullOrEmpty(baseUrl) == true)
                    baseUrl = BP.Sys.SystemConfig.AppSettings["BaseURL"];

                if (string.IsNullOrEmpty(baseUrl) == true)
                    baseUrl = "http://127.0.0.1/";

                if (baseUrl.Substring(baseUrl.Length - 1) != "/")
                    baseUrl = baseUrl + "/";
                return baseUrl;
            }
        }
        public static string CurrPageID
        {
            get
            {
                try
                {
                    string url = BP.Sys.Glo.Request.RawUrl;

                    int i = url.LastIndexOf("/") + 1;
                    int i2 = url.IndexOf(".aspx") - 6;
                    try
                    {
                        url = url.Substring(i);
                        return url.Substring(0, url.IndexOf(".aspx"));

                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message + url + " i=" + i + " i2=" + i2);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(" Get current PageID Error :" + ex.Message);
                }
            }
        }


        // User form style controls 
        public static string GetUserStyle
        {
            get
            {
                BP.WF.Port.WFEmp emp = new Port.WFEmp(WebUser.No);
                if (string.IsNullOrEmpty(emp.Style) || emp.Style == "0")
                {
                    return "0";
                }
                else
                    return emp.Style;
            }

        }
        #endregion  Other configurations .

        #region  Other methods .
        /// <summary>
        ///  Go to the message display interface .
        /// </summary>
        /// <param name="info"></param>
        public static void ToMsg(string info)
        {
            //string rowUrl = BP.Sys.Glo.Request.RawUrl;
            //if (rowUrl.Contains("&IsClient=1"))
            //{
            //    /* Description This is vsto Calls .*/
            //    return;
            //}

            System.Web.HttpContext.Current.Session["info"] = info;
            System.Web.HttpContext.Current.Response.Redirect(Glo.CCFlowAppPath + "WF/MyFlowInfo.aspx?Msg=" + info, false);
        }
        /// <summary>
        ///  Inspection process initiated restrictions 
        /// </summary>
        /// <param name="flow"> Process </param>
        /// <param name="wk"> Work began node </param>
        /// <returns></returns>
        public static bool CheckIsCanStartFlow_InitStartFlow(Flow flow, Work wk)
        {
            StartLimitRole role = flow.StartLimitRole;
            if (role == StartLimitRole.None)
                return true;

            string sql = "";
            string ptable = flow.PTable;

            #region  Must be in accordance with the time , After the form is loaded judge ,  Regardless of the user settings are correct .
            DateTime dtNow = DateTime.Now;
            if (role == StartLimitRole.Day)
            {
                /*  Allows only one day to launch a  */
                sql = "SELECT COUNT(*) as Num FROM " + ptable + " WHERE RDT LIKE '" + DataType.CurrentData + "%' AND WFState NOT IN(0,1) AND FlowStarter='" + WebUser.No + "'";
                if (DBAccess.RunSQLReturnValInt(sql, 0) == 0)
                {
                    if (flow.StartLimitPara == "")
                        return true;

                    // Initiated to determine whether the time in the range of settings .  Format configured for  @11:00-12:00@15:00-13:45
                    string[] strs = flow.StartLimitPara.Split('@');
                    foreach (string str in strs)
                    {
                        if (string.IsNullOrEmpty(str))
                            continue;
                        string[] timeStrs = str.Split('-');
                        string tFrom = DateTime.Now.ToString("yyyy-MM-dd") + " " + timeStrs[0].Trim();
                        string tTo = DateTime.Now.ToString("yyyy-MM-dd") + " " + timeStrs[1].Trim();
                        if (DataType.ParseSysDateTime2DateTime(tFrom) <= dtNow && dtNow >= DataType.ParseSysDateTime2DateTime(tTo))
                            return true;
                    }
                    return false;
                }
                else
                    return false;
            }

            if (role == StartLimitRole.Week)
            {
                /*
                 * 1,  Identify the Week 1  And Sunday are the first few days .
                 * 2,  According to this range to query , If the query to results , It explained that it had started .
                 */
                sql = "SELECT COUNT(*) as Num FROM " + ptable + " WHERE RDT >= '" + DataType.WeekOfMonday(dtNow) + "' AND WFState NOT IN(0,1) AND FlowStarter='" + WebUser.No + "'";
                if (DBAccess.RunSQLReturnValInt(sql, 0) == 0)
                {
                    if (flow.StartLimitPara == "")
                        return true; /* If there is no time limit .*/

                    // Initiated to determine whether the time in the range of settings . 
                    //  Format configured for  @Sunday,11:00-12:00@Monday,15:00-13:45,  Meaning . Sun. , You can start the process within the specified time point range Monday .

                    string[] strs = flow.StartLimitPara.Split('@');
                    foreach (string str in strs)
                    {
                        if (string.IsNullOrEmpty(str))
                            continue;

                        string weekStr = DateTime.Now.DayOfWeek.ToString().ToLower();
                        if (str.ToLower().Contains(weekStr) == false)
                            continue; //  Determines whether the current week .

                        string[] timeStrs = str.Split(',');
                        string tFrom = DateTime.Now.ToString("yyyy-MM-dd") + " " + timeStrs[0].Trim();
                        string tTo = DateTime.Now.ToString("yyyy-MM-dd") + " " + timeStrs[1].Trim();
                        if (DataType.ParseSysDateTime2DateTime(tFrom) <= dtNow && dtNow >= DataType.ParseSysDateTime2DateTime(tTo))
                            return true;
                    }
                    return false;
                }
                else
                    return false;
            }

            // #warning  Did not consider how to deal with the circumference .

            if (role == StartLimitRole.Month)
            {
                sql = "SELECT COUNT(*) as Num FROM " + ptable + " WHERE FK_NY = '" + DataType.CurrentYearMonth + "' AND WFState NOT IN(0,1) AND FlowStarter='" + WebUser.No + "'";
                if (DBAccess.RunSQLReturnValInt(sql, 0) == 0)
                {
                    if (flow.StartLimitPara == "")
                        return true;

                    // Initiated to determine whether the time in the range of settings .  Configuration format : @-01 12:00-13:11@-15 12:00-13:11 ,  Meaning : In the month 1号,15号 12:00-13:11 You can start the process .
                    string[] strs = flow.StartLimitPara.Split('@');
                    foreach (string str in strs)
                    {
                        if (string.IsNullOrEmpty(str))
                            continue;
                        string[] timeStrs = str.Split('-');
                        string tFrom = DateTime.Now.ToString("yyyy-MM-") + " " + timeStrs[0].Trim();
                        string tTo = DateTime.Now.ToString("yyyy-MM-") + " " + timeStrs[1].Trim();
                        if (DataType.ParseSysDateTime2DateTime(tFrom) <= dtNow && dtNow >= DataType.ParseSysDateTime2DateTime(tTo))
                            return true;
                    }
                    return false;
                }
                else
                    return false;
            }

            if (role == StartLimitRole.JD)
            {
                sql = "SELECT COUNT(*) as Num FROM " + ptable + " WHERE FK_NY = '" + DataType.CurrentAPOfJD + "' AND WFState NOT IN(0,1) AND FlowStarter='" + WebUser.No + "'";
                if (DBAccess.RunSQLReturnValInt(sql, 0) == 0)
                {
                    if (flow.StartLimitPara == "")
                        return true;

                    // Initiated to determine whether the time in the range of settings .
                    string[] strs = flow.StartLimitPara.Split('@');
                    foreach (string str in strs)
                    {
                        if (string.IsNullOrEmpty(str))
                            continue;
                        string[] timeStrs = str.Split('-');
                        string tFrom = DateTime.Now.ToString("yyyy-") + " " + timeStrs[0].Trim();
                        string tTo = DateTime.Now.ToString("yyyy-") + " " + timeStrs[1].Trim();
                        if (DataType.ParseSysDateTime2DateTime(tFrom) <= dtNow && dtNow >= DataType.ParseSysDateTime2DateTime(tTo))
                            return true;
                    }
                    return false;
                }
                else
                    return false;
            }

            if (role == StartLimitRole.Year)
            {
                sql = "SELECT COUNT(*) as Num FROM " + ptable + " WHERE FK_NY LIKE '" + DataType.CurrentYear + "%' AND WFState NOT IN(0,1) AND FlowStarter='" + WebUser.No + "'";
                if (DBAccess.RunSQLReturnValInt(sql, 0) == 0)
                {
                    if (flow.StartLimitPara == "")
                        return true;

                    // Initiated to determine whether the time in the range of settings .
                    string[] strs = flow.StartLimitPara.Split('@');
                    foreach (string str in strs)
                    {
                        if (string.IsNullOrEmpty(str))
                            continue;
                        string[] timeStrs = str.Split('-');
                        string tFrom = DateTime.Now.ToString("yyyy-") + " " + timeStrs[0].Trim();
                        string tTo = DateTime.Now.ToString("yyyy-") + " " + timeStrs[1].Trim();
                        if (DataType.ParseSysDateTime2DateTime(tFrom) <= dtNow && dtNow >= DataType.ParseSysDateTime2DateTime(tTo))
                            return true;
                    }
                    return false;
                }
                else
                    return false;
            }
            #endregion  Must be in accordance with the time , After the form is loaded judge ,  Regardless of the user settings are correct .

            return true;
        }
        /// <summary>
        ///  When you want to send is to check whether the process can be allowed to initiate .
        /// </summary>
        /// <param name="flow"> Process </param>
        /// <param name="wk"> Work began node </param>
        /// <returns></returns>
        public static bool CheckIsCanStartFlow_SendStartFlow(Flow flow, Work wk)
        {
            StartLimitRole role = flow.StartLimitRole;
            if (role == StartLimitRole.None)
                return true;

            string sql = "";
            string ptable = flow.PTable;

            if (role == StartLimitRole.ColNotExit)
            {
                /*  Column name specified collection does not exist , Before they can initiate the process .*/

                // Find the original value .
                string[] strs = flow.StartLimitPara.Split(',');
                string val = "";
                foreach (string str in strs)
                {
                    if (string.IsNullOrEmpty(str) == true)
                        continue;
                    try
                    {
                        val += wk.GetValStringByKey(str);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("@ Process design errors , Check the parameters that you configure (" + flow.StartLimitPara + "), The column (" + str + ") Does not exist in the form .");
                    }
                }

                // Identify all the processes have been initiated .
                sql = "SELECT " + flow.StartLimitPara + " FROM " + ptable + " WHERE  WFState NOT IN(0,1) AND FlowStarter='" + WebUser.No + "'";
                DataTable dt = DBAccess.RunSQLReturnTable(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    string v = dr[0] + "" + dr[1] + "" + dr[2];
                    if (v == val)
                        return false;
                }
                return true;
            }

            //  Configuration sql, After performing , The result is  0 .
            if (role == StartLimitRole.ResultIsZero)
            {
                sql = BP.WF.Glo.DealExp(flow.StartLimitPara, wk, null);
                if (DBAccess.RunSQLReturnValInt(sql, 0) == 0)
                    return true;
                else
                    return false;
            }

            //  Configuration sql, After performing , The result is  <> 0 .
            if (role == StartLimitRole.ResultIsNotZero)
            {
                sql = BP.WF.Glo.DealExp(flow.StartLimitPara, wk, null);
                if (DBAccess.RunSQLReturnValInt(sql, 0) != 0)
                    return true;
                else
                    return false;
            }
            return true;
        }
        #endregion  Other methods .
    }
}
