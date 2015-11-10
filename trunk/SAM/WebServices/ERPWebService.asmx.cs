using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using BP.DA;

namespace SAM.WebServices
{
    /// <summary>
    /// Summary description for ERPWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ERPWebService : System.Web.Services.WebService
    {

        public class SPR
        {
            public string ERP_SPR_ID
 , Status
 , Status_Updated_Date
 , Execution_Date
 , PaymentRef
 , Remarks;
        }


        /// <summary>
        /// return ERP_SPR_ID,Status,Status_Updated_Date,Remarks from ERP. WS will get  system date as Status_Recieved_Date and PaymentRef and inster them in Sam_Erp_payment_update
        /// </summary>
        /// <param name="SAM_SPR_ID"></param>
        /// <param name="SPR_Type"></param>
        /// <param name="Requested_By"></param>
        /// <param name="Supplier_ID"></param>
        /// <param name="Ammount"></param>
        /// <param name="Request_Date"></param>
        /// <param name="Due_Date"></param>
        /// <param name="Site_ID"></param>
        /// <param name="SAM_Contract_ID"></param>
        /// <param name="SAM_Contract_Summary"></param>
        /// <param name="Comment"></param>
        /// <param name="Payment_Validity_From"></param>
        /// <param name="Payment_Validity_To"></param>
        /// <param name="Analytical_Account"></param>
        /// <param name="Supplier_Acc_NO"></param>
        /// <param name="Supplier_Acc_Bank"></param>
        /// <param name="Supplier_ACC_SHEBA"></param>
        /// <param name="Supplier_Address"></param>
        /// <param name="Site_Address"></param>
        /// <param name="Approved_By"></param>
        /// <param name="Reviewed_By"></param>
        /// <param name="Initiated_By"></param>
        /// <returns></returns>
        [WebMethod(Description = "This interface will be used to create a new Supplier Payment Request in the ERP system. The function will return the “ERP_SPR_ID” if it is successful, otherwise an error code will be returned that is a minus number. The requester will require to pass following data.")]
        public SPR CreateNewSPR(String SAM_SPR_ID, String
SPR_Type, String
Requested_By, String
Supplier_ID, String
Ammount, String
Request_Date, String
Due_Date, String
Site_ID, String
SAM_Contract_ID, String
SAM_Contract_Summary, String
Comment, String
Payment_Validity_From, String
Payment_Validity_To, String
Analytical_Account, String
Supplier_Acc_NO, String
Supplier_Acc_Bank, String
Supplier_ACC_SHEBA, String
Supplier_Address, String
Site_Address, String
Approved_By, String
Reviewed_By, String
Initiated_By)
        {
            string dt=DateTime.Now.ToShortDateString();

            

            SPR spr= new SPR
            {
                ERP_SPR_ID=Guid.NewGuid().ToString()
                ,
                Status = "toSubmit"
                ,
                Status_Updated_Date=dt
                ,Execution_Date=dt
               
                ,PaymentRef="unknown"
                ,Remarks="nothing"
                
            };
            string td = "TO_DATE('" + DateTime.Now.ToString("dd-MM-yy") + "','DD-MM-YY')"; 
            string sql =
                string.Format(
                    "insert into Sam_Erp_payment_update (ERP_SPR_ID,Status,Status_Updated_date,Status_Recieved_date,PaymentRef,Remarks) values ('{0}','{1}',{2},{2},'{3}','{4}')"
                    , spr.ERP_SPR_ID, spr.Status, td, spr.PaymentRef, spr.Remarks
                );
            int eff = DBAccess.RunSQL(sql);
            return spr;
        }
    }
}
