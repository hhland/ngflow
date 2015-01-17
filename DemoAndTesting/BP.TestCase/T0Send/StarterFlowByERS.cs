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
    ///  Create a node for people to work 
    /// </summary>
    public class StarterFlowByERS : TestBase
    {
        /// <summary>
        ///  Department numbered GUID Send Mode 
        /// </summary>
        public StarterFlowByERS()
        {
            this.Title = " Create a node for people to work ";
            this.DescIt = "以send024,send023,send005 Based Test , Create a node for people to work .";
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
        /// 
        /// </summary>
        public override void Do()
        {
            // string 
            this.fk_flow = "023";
            this.userNo = "zhoupeng";
            string toEmps = "zhanghaicheng,zhangyifan";
            string belontToDept = "2";

            

            #region  Check the recipient work to be done .
            sql = "SELECT * FROM WF_EmpWorks WHERE WorkID=" + this.workid;
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count != 2)
                throw new Exception("@ There should be two people to do the work , It is :" + dt.Rows.Count + "个.");

            foreach (DataRow dr in dt.Rows)
            {
                string fk_emp = dr["FK_Emp"].ToString();

                if (fk_emp == "zhanghaicheng")
                {
                }

                if (fk_emp == "zhangyifan")
                {
                }

            }
            #endregion  Check the recipient work to be done .
        }
    }
}
