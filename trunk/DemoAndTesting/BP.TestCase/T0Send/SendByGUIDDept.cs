using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BP.WF;
using BP.En;
using BP.DA;
using BP.Web;
using System.Data;
using System.Collections;
using BP.CT;

namespace BP.CT.T0Send
{
    /// <summary>
    ///  Department numbered GUID Send Mode 
    /// </summary>
    public class SendByGUIDDept : TestBase
    {
        /// <summary>
        ///  Department numbered GUID Send Mode 
        /// </summary>
        public SendByGUIDDept()
        {
            this.Title = " Department numbered GUID Send Mode ";
            this.DescIt = "以send024,send023,send005 Based Test , Department number is GUID Data storage problems Mode .";
            this.EditState = CT.EditState.Passed;
        }

        #region  Global Variables 
        /// <summary>
        ///  Process ID 
        /// </summary>
        public string fk_flow = "";
        /// <summary>
        ///  User ID 
        /// </summary>
        public string userNo = "";
        /// <summary>
        ///  All processes 
        /// </summary>
        public Flow fl = null;
        /// <summary>
        ///  The main thread ID
        /// </summary>
        public Int64 workid = 0;
        /// <summary>
        ///  After sending the return object 
        /// </summary>
        public SendReturnObjs objs = null;
        /// <summary>
        ///  Staff List 
        /// </summary>
        public GenerWorkerList gwl = null;
        /// <summary>
        ///  Process Registry 
        /// </summary>
        public GenerWorkFlow gwf = null;
        #endregion  Variable 

        /// <summary>
        ///  Department numbered GUID Send Mode 
        /// </summary>
        public override void Do()
        {
            // Reloaded demo environment .
            ReLoadDept();

            try
            {
                #region  The data into guid Mode .
                BP.Port.Depts depts = new Port.Depts();
                depts.RetrieveAll();

                string guid1 = "";
                foreach (BP.Port.Dept item in depts)
                {
                    string deptNo = item.No;
                    string guid = DBAccess.GenerGUID();
                    if (item.No == "1")
                        guid1 = guid;

                    sql = "UPDATE Port_Dept SET No='" + guid + "' WHERE No='" + deptNo + "'";
                    DBAccess.RunSQL(sql);

                    sql = "UPDATE Port_Emp SET FK_Dept='" + guid + "' WHERE FK_Dept='" + deptNo + "'";
                    DBAccess.RunSQL(sql);

                    sql = "UPDATE Port_EmpDept SET FK_Dept='" + guid + "' WHERE FK_Dept='" + deptNo + "'";
                    DBAccess.RunSQL(sql);
                }

                sql = "UPDATE Port_Dept SET ParentNo='" + guid1 + "' WHERE ParentNo='1'";
                DBAccess.RunSQL(sql);
                #endregion

                string err = "";
                Flow fl = new Flow("023");
                fl.CheckRpt();
                fl.DoDelData();

                BP.Sys.SystemConfig.DoClearCash_del();

                err = "@第Send023  Error .";
                Send023 se = new Send023();
                se.Do();

                fl = new Flow("024");
                fl.CheckRpt();
                err = "@第Send024  Error .";
                Send024 s2e = new Send024();
                s2e.Do();


                fl = new Flow("005");
                fl.CheckRpt();
                err = "@第Send005  Error .";
                Send005 s5 = new Send005();
                s5.Do();

                // Reloaded demo environment .
                ReLoadDept();
            }
            catch (Exception ex)
            {
                // Reloaded demo environment .
                ReLoadDept();

                throw ex;
            }
        }
        /// <summary>
        ///  Reloading environment .
        /// </summary>
        public void ReLoadDept()
        {
            string sqls = "@DROP VIEW Port_EmpDept";
            sqls += "@DROP VIEW Port_EmpStation";
            BP.DA.DBAccess.RunSQLs(sqls);

            string sqlscript = "";
            sqlscript = BP.Sys.SystemConfig.PathOfData + "\\Install\\SQLScript\\Port_Inc_CH.sql";
            BP.DA.DBAccess.RunSQLScript(sqlscript);
            BP.Sys.SystemConfig.DoClearCash_del();
        }
    }
}
