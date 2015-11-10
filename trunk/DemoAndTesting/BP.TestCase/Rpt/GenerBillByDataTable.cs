using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BP.WF;
using BP.En;
using BP.DA;
using BP.Web;
using System.Data;
using System.Collections;
using BP.CT;

namespace BP.CT
{
    /// <summary>
    ///  According to datatable  Generate documents 
    /// </summary>
    public class GenerBillByDataTable : TestBase
    {
        /// <summary>
        ///  According to datatable  Generate documents 
        /// </summary>
        public GenerBillByDataTable()
        {
            this.Title = " According to  datatable  Generate documents ";
            this.DescIt = " Process :  Financial reimbursement process .";
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
        public Int64 workID = 0;
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
        ///  Test Case Description :
        /// 1, .
        /// 2, .
        /// 3,.
        /// </summary>
        public override void Do()
        {
            #region  Initialization data .
            // Initialize variables .
            fk_flow = "001";
            userNo = "zhangyifan";
            fl = new Flow(fk_flow);
            BP.WF.Dev2Interface.Port_Login(userNo);

            // Create a blank process .
            this.workID = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, null, null);

            // Create a master table data . （ Cost master table ）.
            Hashtable ht = new Hashtable();
            ht.Add("ZhaiYaoShuoMing", " Summary Description :ZhaiYaoShuoMing.");
            ht.Add("RPI", 1);

            //  Created from the table data .（ Fees Breakdown ）
            DataSet ds = new DataSet();

            DataTable dt = new DataTable();
            dt.TableName = "ND101Dtl1";
            dt.Columns.Add("RefPK", typeof(int)); // Primary key association , Here it is workid.
            dt.Columns.Add("FYLX", typeof(int)); //  Expense Type .
            dt.Columns.Add("JinE", typeof(decimal)); //  Money 
            dt.Columns.Add("ShuLiang", typeof(decimal)); // Quantity .
            dt.Columns.Add("XiaoJi", typeof(decimal)); //  Subtotal .

            DataRow dr = dt.NewRow();
            dr["RefPK"] = this.workID;
            dr["FYLX"] = 1;
            dr["JinE"] = 150;
            dr["ShuLiang"] = 2;
            dr["XiaoJi"] = 300;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["RefPK"] = this.workID;
            dr["FYLX"] = 2;
            dr["JinE"] = 250;
            dr["ShuLiang"] = 3;
            dr["XiaoJi"] = 750;
            dt.Rows.Add(dr);
            ds.Tables.Add(dt);

            // Performing transmission .
            BP.WF.Dev2Interface.Node_SendWork(fk_flow, this.workID, ht, ds, 0, null);
            #endregion  Initialization data .

            #region  Generate test data .
            // Save the query to the data source database .
            string sql = "SELECT * FROM ND101 WHERE OID=" + this.workID;
            DataTable dtMain = DBAccess.RunSQLReturnTable(sql);

            sql = "SELECT * FROM ND101Dtl1 WHERE RefPK=" + this.workID;
            DataTable dtDtl = DBAccess.RunSQLReturnTable(sql);
            dtDtl.TableName = "ND101Dtl1";

            DataSet myds = new DataSet();
            myds.Tables.Add(dtDtl);
            #endregion  Generate test data .

            string templeteFilePath = @"D:\ccflow\trunk\CCFlow\DataUser\CyclostyleFile\ Documents print samples .rtf";
            BP.Pub.RTFEngine rpt = new BP.Pub.RTFEngine();

            rpt.MakeDocByDataSet(templeteFilePath, "C:\\", this.workID + ".doc", dtMain, myds);
        }
    }
}
