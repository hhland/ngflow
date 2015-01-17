using System;
using System.Collections;
using System.Data;
using BP.DA;
using BP.DTS;
using BP.En;
using BP.Web;
using BP.Sys;
using BP.WF;
using BP.Port;

namespace BP.Demo
{
    /// <summary>
    ///  Reimbursement process  -  Start node .
    /// </summary>
    public class ND101 : BP.WF.FlowEventBase
    {
        #region  Structure .
        /// <summary>
        ///  Reimbursement process events 
        /// </summary>
        public ND101()
        {
        }
        #endregion  Property .

        #region  Overriding Properties .
        public override string FlowMark
        {
            get { return "QingJia"; }
        }
        #endregion  Overriding Properties .

        #region  Rewrite nodes form events .
        /// <summary>
        ///  Form before loading 
        /// </summary>
        public override string FrmLoadAfter()
        {
            return null;
        }
        /// <summary>
        ///  After loading the form 
        /// </summary>
        public override string FrmLoadBefore()
        {
            return null;
        }
        /// <summary>
        ///  Forms saved 
        /// </summary>
        public override string SaveAfter()
        {
            return null;
        }
        /// <summary>
        ///  Form before saving 
        /// </summary>
        public override string SaveBefore()
        {
            if (this.HisNode.NodeID == 101)
            {
                // Total lowercase update .
                string sql = "UPDATE ND101 SET HeJi=(SELECT SUM(XiaoJi) FROM ND101Dtl1 WHERE RefPK=" + this.OID + ") WHERE OID=" + this.OID;
                BP.DA.DBAccess.RunSQL(sql);
                // The total conversion to uppercase .
                float hj = BP.DA.DBAccess.RunSQLReturnValFloat("SELECT HeJi FROM ND101 WHERE OID=" + this.OID, 0);

                sql = "UPDATE ND101 SET DaXie='" + BP.DA.DataType.ParseFloatToCash(hj) + "' WHERE OID=" + this.OID;
                BP.DA.DBAccess.RunSQL(sql);
                return null;
            }
            return null;
        }
        #endregion  Rewrite nodes form events 

        #region  Rewrite node movement events .
        /// <summary>
        ///  Send ago : Business logic can be used to check whether the transmission is performed , Send to throw an exception can not be executed .
        /// </summary>
        public override string SendWhen()
        {
            if (this.HisNode.NodeID == 101)
            {
                // Total lowercase update .
                string sql = "UPDATE ND101 SET HeJi=(SELECT SUM(XiaoJi) FROM ND101Dtl1 WHERE RefPK=" + this.OID + ") WHERE OID=" + this.OID;
                BP.DA.DBAccess.RunSQL(sql);
                // The total conversion to uppercase .
                float hj = BP.DA.DBAccess.RunSQLReturnValFloat("SELECT HeJi FROM ND101 WHERE OID=" + this.OID, 0);
                if (hj == 0)
                    throw new Exception("@ You need to enter the details of the project cost .");

                sql = "UPDATE ND101 SET DaXie='" + BP.DA.DataType.ParseFloatToCash(hj) + "' WHERE OID=" + this.OID;
                BP.DA.DBAccess.RunSQL(sql);
                return " Total has been completed before sending the event .";
            }

            return null;
        }
        /// <summary>
        ///  After successfully sent 
        /// </summary>
        public override string SendSuccess()
        {
            return null;
        }
        /// <summary>
        ///  After sending failed 
        /// </summary>
        public override string SendError()
        {
            return null;
        }
        /// <summary>
        ///  Return ago 
        /// </summary>
        public override string ReturnBefore()
        {
            return null;
        }
        /// <summary>
        ///  After returning 
        /// </summary>
        public override string ReturnAfter()
        {
            return null;
        }
        #endregion  Rewrite event , Business logic .
    }
}
