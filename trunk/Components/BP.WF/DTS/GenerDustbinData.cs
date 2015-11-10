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
    public class GenerDustbinData : Method
    {
        /// <summary>
        ///  Method with no arguments 
        /// </summary>
        public GenerDustbinData()
        {
            this.Title = " Find out because ccflow Garbage data generated internal error ";
            this.Help = " Systems do not automatically fix it , Determine the cause of the need for manual .";
            this.Icon = "<img src='/WF/Img/Btn/Card.gif'  border=0 />";
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

            string msg = "";
            Flows fls = new Flows();
            fls.RetrieveAll();
            foreach (Flow fl in fls)
            {
                string rptTable = "ND" + int.Parse(fl.No) + "Rpt";
                string fk_mapdata = "ND" + int.Parse(fl.No) + "01";
                MapData md = new MapData(fk_mapdata);

                string sql = "SELECT OID,Title,Rec,WFState,WFState FROM " + md.PTable + " WHERE WFState=" + (int)WFState.Runing + " AND OID IN (SELECT OID FROM " + rptTable + " WHERE WFState!=0 )";
                DataTable dt = DBAccess.RunSQLReturnTable(sql);
                if (dt.Rows.Count == 0)
                    continue;

                msg += "@" + sql;
                //msg += " Repair sql: UPDATE " + ndTable"  " ;
            }
            if (string.IsNullOrEmpty(msg))
                return "@ Normal data can be detected .";

            BP.DA.Log.DefaultLogWriteLineInfo(msg);
            return  " Following an abnormal data , Draft state marked the beginning of the node marked in the actual work has been completed :"+msg+"  The above data is written to the Log File , Open View .";
        }
    }
}
