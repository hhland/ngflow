using System;
using System.Threading;
using System.Collections;
using BP.Web.Controls;
using System.Data;
using BP.DA;
using BP.DTS;
using BP.En;
using BP.Web;
using BP.Sys;
using BP.WF;

namespace BP.FlowEvent
{
    /// <summary>
    ///  Reimbursement process 001
    /// </summary>
    public class F104:BP.Sys.EventBase
    {
        #region  Property .
        #endregion  Property .

        #region  Structure .
        /// <summary>
        ///  Reimbursement process events 
        /// </summary>
        public F104()
        {
            this.Title = " Leave Process ";
        }
        #endregion  Property .

        /// <summary>
        ///  Execution events 
        /// 1, If an error is thrown Information , Reception interface will prompt an error does not execute down .
        /// 2, Successful implementation , Assigned to the implementation of the results SucessInfo Variable , If you do not need tips on assignment or for empty null.
        /// 3, All parameters are available from   this.SysPara.GetValByKey  Get .
        /// </summary>
        public override void Do()
        {
            switch (this.FK_Node)
            {
                case 10401: // Fill in the application form for reimbursement node .
                case 10402: // Fill in the application form for reimbursement node .
                case 10403: // Fill in the application form for reimbursement node .
                    switch (this.EventType)
                    {
                        case EventListOfNode.FrmLoadBefore: // Forms saved events .
                            break;
                        case EventListOfNode.SaveBefore: // Save the form before the event .
                            SendWhen10401();
                            break;
                        case EventListOfNode.SendWhen: // Send ago .
                            SendWhen10401();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        ///  Send executed before the event 
        /// </summary>
        public void SendWhen10401()
        {
            DateTime dtFrom = this.HisEn.GetValDateTime("QingJiaShiJianCong");
            DateTime dtTo = this.HisEn.GetValDateTime("Dao");

            if (dtFrom > dtTo)
                throw new Exception(" Leave time to leave is not less than the time from .");

            // Determine the number of days leave .
            TimeSpan ts = dtTo - dtFrom;
            float span = ts.Days;
            this.HisEn.SetValByKey("QingJiaTianShu", span);

            // Set to the data source , Allowed direct updates to the data source , In case YuE Fields can be edited this step is not necessary .
            string sql = "UPDATE ND10401 SET QingJiaTianShu=" + span + " WHERE OID=" + this.OID;
            BP.DA.DBAccess.RunSQL(sql);
        }
    }
}
