using System;
using System.Data;
using System.Collections;
using BP.DA;
using BP.Web.Controls;
using System.Reflection;
using BP.Port;
using BP.En;
namespace BP.WF.DTS
{
    /// <summary>
    ///  Upgrade ccflow5 Scheduling to be performed 
    /// </summary>
    public class UpTrack : Method
    {
        /// <summary>
        ///  Method with no arguments 
        /// </summary>
        public UpTrack()
        {
            this.Title = " Upgrade ccflow5 Scheduling to be performed ( Only deal wf_track Section , Not repeated .)";
            this.Help = " Perform this procedure to ccflow4  Upgrade to ccflow5  This procedure is only resolved wf_track Problem table .";
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
                if (BP.Web.WebUser.No == "admin")
                    return true;
                else
                    return false;
            }
        }
        /// <summary>
        ///  Carried out 
        /// </summary>
        /// <returns> Return to the results </returns>
        public override object Do()
        {
            Flows fls = new Flows();
            fls.RetrieveAllFromDBSource();

            string info = "";
            foreach (Flow fl in fls)
            {
                //  Check Report .
                Track.CreateOrRepairTrackTable(fl.No);

                //  Inquiry .
                string sql = "SELECT * FROM WF_Track WHERE FK_Flow='" + fl.No + "'";
                DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Track tk = new Track();
                    tk.FK_Flow = fl.No;
                    tk.Row.LoadDataTable(dt, dt.Rows[0]);
                    tk.DoInsert(0); //  Carried out insert.
                }
            }
            return  " Execution is completed , You can not do it in the , Otherwise there will be duplicate track data .";
        }
    }
}
