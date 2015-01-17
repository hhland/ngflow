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
    public class F025:BP.Sys.EventBase
    {
        #region  Property .
        #endregion  Property .

        #region  Structure .
        /// <summary>
        ///  Reimbursement process events 
        /// </summary>
        public F025()
        {
            this.Title = " Reimbursement process ";
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
                case 2501: // Fill in the application form for reimbursement node .
                    switch (this.EventType)
                    {
                            case EventListOfNode.FrmLoadBefore: // Forms saved events .
                            this.ND2501_FrmLoadBefore();
                            break;
                        case EventListOfNode.SendWhen: // Send ago .
                            this.ND2501_SendWhen();
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
        ///  Loading before the event 
        ///  Methods for naming :ND+ Node name _ Event name .
        /// </summary>
        public void ND2501_FrmLoadBefore()
        {
            //  Seeking balance . 
            int dtlOID=this.GetValInt("ItemNo");
            int Totalmoney=this.GetValInt("Totalmoney");

            string sql="select sum(BXJE) as Num from ND25Rpt WHERE ItemNo='"+dtlOID+"' AND WFState!=0";
            decimal bxjeSum=BP.DA.DBAccess.RunSQLReturnValDecimal(sql,0,1);
                  
            decimal ye=Totalmoney-bxjeSum;

            /* After obtaining the balance , Need to do two operations .*/

            // Set to the entity , So that it could be displayed on the form .
            this.HisEn.SetValByKey("YuE", ye);

            // Set to the data source , Allowed direct updates to the data source , In case YuE Fields can be edited this step is not necessary .
            sql = "UPDATE ND2501 SET YuE=" + ye + " WHERE OID=" + this.OID;
            BP.DA.DBAccess.RunSQL(sql);
        }
        /// <summary>
        ///  Send ago 
        ///  Methods for naming :ND+ Node name _ Event name .
        /// </summary>
        public void ND2501_SendWhen()
        {
            throw new¡¡ Exception("@ You have exceeded the amount of the reimbursement budget .");
        }
    }
}
