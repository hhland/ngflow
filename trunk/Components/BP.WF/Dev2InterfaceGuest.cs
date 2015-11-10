using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Text;
using BP.WF;
using BP.DA;
using BP.Port;
using BP.Web;
using BP.En;
using BP.Sys;
using BP.WF.Data;

namespace BP.WF
{
    /// <summary>
    ///  This interface for programmers to use the secondary development , In reading the code before Note the following .
    /// 1, CCFlow The external interface is static methods to achieve .
    /// 2, 以 DB_  The beginning of the need to return a result set of interface .
    /// 3, 以 Flow_  Is the process interface .
    /// 4, 以 Node_  Node interface .
    /// 5, 以 Port_  Is the organization interface .
    /// 6, 以 DTS_  Is scheduling ． 
    /// 7, 以 UI_  Is a function of the process window ． 
    ///  External user access interface 
    /// </summary>
    public class Dev2InterfaceGuest
    {
        /// <summary>
        ///  Create WorkID
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="ht"> Form parameters , For null.</param>
        /// <param name="workDtls"> Parameter list , For null.</param>
        /// <param name="nextWorker"> The operator , If it is null Is the current staff .</param>
        /// <param name="title"> When the title to create work , If it is null, It generated according to the rules set .</param>
        /// <returns> To create the work after the start node generated WorkID.</returns>
        public static Int64 Node_CreateBlankWork(string flowNo, Hashtable ht, DataSet workDtls,
            string guestNo, string title)
        {
            return Node_CreateBlankWork(flowNo, ht, workDtls, guestNo, title, 0, null,0,null);
        }
        /// <summary>
        ///  Create WorkID
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="ht"> Form parameters , For null.</param>
        /// <param name="workDtls"> Parameter list , For null.</param>
        /// <param name="starter"> Process sponsor </param>
        /// <param name="title"> When the title to create work , If it is null, It generated according to the rules set .</param>
        /// <param name="parentWorkID"> Parent process WorkID, If not passed on to the parent process 0.</param>
        /// <param name="parentFlowNo"> Parent process ID of the process , If not passed on to the parent process null.</param>
        /// <returns> To create the work after the start node generated WorkID.</returns>
        public static Int64 Node_CreateBlankWork(string flowNo, Hashtable ht, DataSet workDtls,
            string guestNo, string title, Int64 parentWorkID, string parentFlowNo, int parentNodeID, string parentEmp)
        {
            //if (BP.Web.WebUser.No != "Guest")
            //    throw new Exception("@ Must be Guest Login to initiate .");

            //  Converted into numbers .
            flowNo = TurnFlowMarkToFlowNo(flowNo);

            // Converted into numbers 
            parentFlowNo = TurnFlowMarkToFlowNo(parentFlowNo);

            string dbstr = SystemConfig.AppCenterDBVarStr;

            Flow fl = new Flow(flowNo);
            Node nd = new Node(fl.StartNodeID);

            Emp empStarter = new Emp(BP.Web.WebUser.No);
            Work wk = fl.NewWork(empStarter);
            Int64 workID = wk.OID;

            #region  To each property - Assignment 
            if (ht != null)
            {
                foreach (string str in ht.Keys)
                    wk.SetValByKey(str, ht[str]);
            }
            wk.OID = workID;
            if (workDtls != null)
            {
                // Save from the table 
                foreach (DataTable dt in workDtls.Tables)
                {
                    foreach (MapDtl dtl in wk.HisMapDtls)
                    {
                        if (dt.TableName != dtl.No)
                            continue;
                        // Get dtls
                        GEDtls daDtls = new GEDtls(dtl.No);
                        daDtls.Delete(GEDtlAttr.RefPK, wk.OID); //  Clear existing data .

                        GEDtl daDtl = daDtls.GetNewEntity as GEDtl;
                        daDtl.RefPK = wk.OID.ToString();

                        //  To copy the data from the table .
                        foreach (DataRow dr in dt.Rows)
                        {
                            daDtl.ResetDefaultVal();
                            daDtl.RefPK = wk.OID.ToString();

                            // Details column .
                            foreach (DataColumn dc in dt.Columns)
                            {
                                // Setting properties .
                                daDtl.SetValByKey(dc.ColumnName, dr[dc.ColumnName]);
                            }
                            daDtl.InsertAsOID(DBAccess.GenerOID("Dtl")); // Insert data .
                        }
                    }
                }
            }
            #endregion  Assignment 

            Paras ps = new Paras();
            //  Implementation of the report data sheet WFState Update status , It is runing Status .
            if (string.IsNullOrEmpty(title) == false)
            {
                if (fl.TitleRole != "@OutPara")
                {
                    fl.TitleRole = "@OutPara";
                    fl.Update();
                }

                ps = new Paras();
                ps.SQL = "UPDATE " + fl.PTable + " SET WFState=" + dbstr + "WFState,Title=" + dbstr + "Title WHERE OID=" + dbstr + "OID";
                ps.Add(GERptAttr.WFState, (int)WFState.Blank);
                ps.Add(GERptAttr.Title, title);
                ps.Add(GERptAttr.OID, wk.OID);
                DBAccess.RunSQL(ps);
            }
            else
            {
                ps = new Paras();
                ps.SQL = "UPDATE " + fl.PTable + " SET WFState=" + dbstr + "WFState,FK_Dept=" + dbstr + "FK_Dept,Title=" + dbstr + "Title WHERE OID=" + dbstr + "OID";
                ps.Add(GERptAttr.WFState, (int)WFState.Blank);
                ps.Add(GERptAttr.FK_Dept, empStarter.FK_Dept);
                ps.Add(GERptAttr.Title, WorkNode.GenerTitle(fl, wk));
                ps.Add(GERptAttr.OID, wk.OID);
                DBAccess.RunSQL(ps);
            }

            //  Delete junk data may produce , For example, the last time was not sent successfully , Cause data not cleared .
            ps = new Paras();
            ps.SQL = "DELETE FROM WF_GenerWorkFlow  WHERE WorkID=" + dbstr + "WorkID1 OR FID=" + dbstr + "WorkID2";
            ps.Add("WorkID1", wk.OID);
            ps.Add("WorkID2", wk.OID);
            DBAccess.RunSQL(ps);

            ps = new Paras();
            ps.SQL = "DELETE FROM WF_GenerWorkerList  WHERE WorkID=" + dbstr + "WorkID1 OR FID=" + dbstr + "WorkID2";
            ps.Add("WorkID1", wk.OID);
            ps.Add("WorkID2", wk.OID);
            DBAccess.RunSQL(ps);

            //  Setting process information 
            if (parentWorkID != 0)
                BP.WF.Dev2Interface.SetParentInfo(flowNo, workID, parentFlowNo, parentWorkID,parentNodeID,parentEmp);

            return wk.OID;
        }

        #region  Portal .
        /// <summary>
        ///  Landed 
        /// </summary>
        /// <param name="guestNo"> Customer Number </param>
        /// <param name="guestName"> Customer Name </param>
        public static void Port_Login(string guestNo,string guestName)
        {
            // Landed .
            BP.Web.GuestUser.SignInOfGener(guestNo, guestName, "CH", true);
        }
        /// <summary>
        ///  Landed 
        /// </summary>
        /// <param name="guestNo"> Customer Number </param>
        /// <param name="guestName"> Customer Name </param>
        /// <param name="deptNo"> Customer department number </param>
        /// <param name="deptName"> Customer department name </param>
        public static void Port_Login(string guestNo, string guestName, string deptNo, string deptName)
        {
            // Landed .
            BP.Web.GuestUser.SignInOfGener(guestNo, guestName, deptNo,deptName,"CH", true);
        }
        /// <summary>
        ///  Logout .
        /// </summary>
        public static void Port_LoginOunt()
        {
            // Landed .
            BP.Web.GuestUser.Exit();
        }
        #endregion  Portal .


        #region  Get Guest To-do 
        /// <summary>
        ///  Get Guest To-do 
        /// </summary>
        /// <param name="fk_flow"> Process ID , Process ID is empty means that all processes .</param>
        /// <param name="guestNo"> Customer Number </param>
        /// <returns> Result set </returns>
        public static DataTable DB_GenerEmpWorksOfDataTable(string fk_flow, string guestNo)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            Paras ps = new Paras();
            string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
            string sql;

            /* Not authorize state */
            if (string.IsNullOrEmpty(fk_flow))
            {
                ps.SQL = "SELECT * FROM WF_EmpWorks WHERE GuestNo=" + dbstr + "GuestNo AND FK_Emp='Guest' ORDER BY FK_Flow,ADT DESC ";
                ps.Add("GuestNo", guestNo);
            }
            else
            {
                ps.SQL = "SELECT * FROM WF_EmpWorks WHERE GuestNo=" + dbstr + "GuestNo AND FK_Emp='Guest' AND FK_Flow=" + dbstr + "FK_Flow ORDER BY  ADT DESC ";
                ps.Add("FK_Flow", fk_flow);
                ps.Add("GuestNo", guestNo);
            }
            return BP.DA.DBAccess.RunSQLReturnTable(ps);
        }
        /// <summary>
        ///  Get unfinished process ( Also known as in-transit process : I participated in this process is not completed yet )
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        /// <returns> Returns from the data view WF_GenerWorkflow Check out the data .</returns>
        public static DataTable DB_GenerRuning(string fk_flow, string guestNo)
        {
            //  Converted into numbers .
            fk_flow = TurnFlowMarkToFlowNo(fk_flow);

            string sql;
            int state = (int)WFState.Runing;

            if (string.IsNullOrEmpty(fk_flow))
                sql = "SELECT a.WorkID FROM WF_GenerWorkFlow A, WF_GenerWorkerlist B WHERE A.WorkID=B.WorkID AND B.FK_Emp='" + WebUser.No + "' AND B.IsEnable=1 AND B.IsPass=1 AND A.GuestNo='" + guestNo + "' ";
            else
                sql = "SELECT a.WorkID FROM WF_GenerWorkFlow A, WF_GenerWorkerlist B WHERE A.FK_Flow='" + fk_flow + "'  AND A.WorkID=B.WorkID AND B.FK_Emp='" + WebUser.No + "' AND B.IsEnable=1 AND B.IsPass=1  AND A.GuestNo='" + guestNo + "'";

            GenerWorkFlows gwfs = new GenerWorkFlows();
            gwfs.RetrieveInSQL(GenerWorkFlowAttr.WorkID, "(" + sql + ")");
            return gwfs.ToDataTableField();
        }
        #endregion

        #region  Function 
        /// <summary>
        ///  Setting up user information 
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="workID"> The work ID</param>
        /// <param name="guestNo"> Customer Number </param>
        /// <param name="guestName"> Customer Name </param>
        public static void SetGuestInfo(string flowNo, Int64 workID, string guestNo, string guestName)
        {
            string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
            Paras ps = new Paras();
            ps.SQL = "UPDATE WF_GenerWorkFlow SET GuestNo=" + dbstr + "GuestNo, GuestName=" + dbstr + "GuestName WHERE WorkID=" + dbstr + "WorkID";
            ps.Add("GuestNo", guestNo);
            ps.Add("GuestName", guestName);
            ps.Add("WorkID", workID);
            BP.DA.DBAccess.RunSQL(ps);

            Flow fl = new Flow(flowNo);
            ps = new Paras();
            ps.SQL = "UPDATE " + fl.PTable + " SET GuestNo=" + dbstr + "GuestNo, GuestName=" + dbstr + "GuestName WHERE OID=" + dbstr + "OID";
            ps.Add("GuestNo", guestNo);
            ps.Add("GuestName", guestName);
            ps.Add("OID", workID);
            BP.DA.DBAccess.RunSQL(ps);
        }
        /// <summary>
        ///  Set the current user to-do 
        /// </summary>
        /// <param name="workID"> The work ID</param>
        /// <param name="guestNo"> Customer Number </param>
        /// <param name="guestName"> Customer Name </param>
        public static void SetGuestToDoList(Int64 workID, string guestNo, string guestName)
        {
            if (guestNo == "")
                throw new Exception("@ Set the external user information failed Upcoming : Parameters guestNo Can not be empty .");
            if (workID == 0)
                throw new Exception("@ Set the external user information failed Upcoming : Parameters workID Can not be 0.");

            string dbstr = BP.Sys.SystemConfig.AppCenterDBVarStr;
            Paras ps = new Paras();
            ps.SQL = "UPDATE WF_GenerWorkerList SET GuestNo=" + dbstr + "GuestNo, GuestName=" + dbstr + "GuestName WHERE WorkID=" + dbstr + "WorkID AND IsPass=0";
            ps.Add("GuestNo", guestNo);
            ps.Add("GuestName", guestName);
            ps.Add("WorkID", workID);
            int i = BP.DA.DBAccess.RunSQL(ps);
            if (i == 0)
                throw new Exception("@ Set the external user information failed Upcoming : Parameters workID Can not be empty .");

            ps = new Paras();
            ps.SQL = "UPDATE WF_GenerWorkFlow SET GuestNo=" + dbstr + "GuestNo, GuestName=" + dbstr + "GuestName WHERE WorkID=" + dbstr + "WorkID ";
            ps.Add("GuestNo", guestNo);
            ps.Add("GuestName", guestName);
            ps.Add("WorkID", workID);
            i = BP.DA.DBAccess.RunSQL(ps);
            if (i == 0)
                throw new Exception("@WF_GenerWorkFlow -  Set the external user information failed Upcoming : Parameters WorkID Can not be empty .");
        }
        #endregion


        #region  General Method 
        public static string TurnFlowMarkToFlowNo(string FlowMark)
        {
            if (string.IsNullOrEmpty(FlowMark))
                return null;

            //  If the number , Would not have transformed .
            if (DataType.IsNumStr(FlowMark))
                return FlowMark;

            string s = DBAccess.RunSQLReturnStringIsNull("SELECT No FROM WF_Flow WHERE FlowMark='" + FlowMark + "'", null);
            if (s == null)
                throw new Exception("@FlowMark Error :" + FlowMark + ", Did not find it flow numbers .");
            return s;
        }
        #endregion
    }
}
