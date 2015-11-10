using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using BP.DA;

namespace SAM.WebServices
{
    /// <summary>
    /// Summary description for SAMWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SAMWebService : System.Web.Services.WebService
    {

       

        /// <summary>
        /// SAM Recieve ERP_SPR_ID,Status,Status_Updated_Date,Remarks from ERP. WS will get  system date as Status_Recieved_Date and PaymentRef and inster them in Sam_Erp_payment_update
        /// </summary>
        /// <param name="ERP_SPR_ID"></param>
        /// <param name="Status"></param>
        /// <param name="Execution_Date"></param>
        /// <param name="PaymentRef"></param>
        /// <param name="Remarks"></param>
        /// <returns></returns>
        [WebMethod(Description = "The interface will be used to update status of the SPRs. This interface should be provided by SAM system. If the call wasn’t successful an error code (a minus number) will be returned, otherwise the ERP_SPR_ID will be returned. ")]
        public string UpdateSPRStatus(string ERP_SPR_ID, String Status, String Execution_Date, String PaymentRef, String Remarks)
        {
            string td = "TO_DATE('"+DateTime.Now.ToString("dd-MM-yy")+"','DD-MM-YY')";  
            string sql = string.Format("insert into Sam_Erp_payment_update (ERP_SPR_ID,Status,Status_Updated_date,Status_Recieved_date,PaymentRef,Remarks) values ('{0}','{1}',{2},{2},'{3}','{4}')"
                , ERP_SPR_ID, Status, td, PaymentRef, Remarks
                );
            sql = string.Format("update  Sam_Erp_payment_update set Status='{1}',Status_Updated_date=TO_DATE('{2}','YYYY-MM-DD'),Status_Recieved_date=TO_DATE('{2}','YYYY-MM-DD'),PaymentRef='{3}',Remarks='{4}' where ERP_SPR_ID='{0}'"
                ,ERP_SPR_ID
                ,Status
                , Execution_Date
                , PaymentRef
                , Remarks
                );
            int eff = 0;
            string re = "";
            try
            {
                eff+=DBAccess.RunSQL(sql);
                re=string.Format("{0} UpdateSPRStatus ok,ERP_SPR_ID:{1}",eff,ERP_SPR_ID);
            }catch(Exception ex){
                re = string.Format("exception:{0},{1},sql:{2}",ex.GetType().Name,ex.Message,sql);
            }
            return re;
        }


        public class SAM_Contract
        {
            public string ERP_SPR_ID, SAM_Contract_Info,_errmsg;
        }

        /// <summary>
        ///  ERP recieve return SAM_Contract_Info, and insert both of them into Erp_Sam_Contract_Info
        /// </summary>
        /// <param name="SAM_Contract_ID"></param>
        /// <returns></returns>
        [WebMethod(Description = "The interface will be used to get more info related to a SAM contract. This interface should be provided by SAM system. If the call wasn’t successful an error code (a minus number) will be returned, otherwise a 512 bytes sting will be returned containing a summary of the contract. ")]
        public SAM_Contract GetContractSummary(string SAM_Contract_ID)
        {
            string sql = string.Format("select * from Sam_Erp_Contract_Info where ERP_SPR_ID='{0}'",SAM_Contract_ID);
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            SAM_Contract samContract= new SAM_Contract();
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                samContract.ERP_SPR_ID = dr["ERP_SPR_ID"].ToString();
                samContract.SAM_Contract_Info = dr["SAM_Contract_Info"].ToString();
                sql = string.Format("insert into Erp_Sam_Contract_Info (ERP_SPR_ID,SAM_Contract_Info) values ('{0}','{1}')"
                 , samContract.ERP_SPR_ID
                 ,samContract.SAM_Contract_Info

               );
                int eff = DBAccess.RunSQL(sql);
            }
            else
            {
                samContract._errmsg = string.Format("can't find contract with {0} ", SAM_Contract_ID);
            }
           

            return samContract;
        }

        /// <summary>
        /// The interface will be used to update status of the SupplierUpdate Requests described above.
        ///This interface can be provided by SAM system but it is optional.
        ///If the call wasn’t successful an error code(a minus number) will be returned, otherwise the
        /// ERP_SPR_ID will be returned.The error code details should be provided by SAM team.
        /// </summary>
        /// <param name="ERP_Supplier_Request_ID"></param>
        /// <param name="Status"></param>
        /// <param name="Execution_Date"></param>
        /// <param name="Remarks"></param>
        [WebMethod(Description = "The interface will be used to update status of the SupplierUpdate Requests described above.This interface can be provided by SAM system but it is optional.If the call wasn’t successful an error code(a minus number) will be returned, otherwise the ERP_SPR_ID will be returned.The error code details should be provided by SAM team.")]
        public int UpdateRequestStatus(string ERP_Supplier_Request_ID,string Status,string Execution_Date,string Remarks) {
            int eff = 0;
            string sql = string.Format("update sam_erp_llinfoupdate set Status='{1}',Execution_Date=TO_DATE('{2}','YYYY-MM-DD'),Remarks='{3}' where ERP_Supplier_Request_ID='{0}'"
            //string sql = string.Format("insert into sam_erp_llinfoupdate (ID,ERP_Supplier_Request_ID,Status,Execution_Date,Remarks) values (seq_sam_erp_llinfoupdate.nextval,'{0}','{1}',TO_DATE('{2}','YYYY-MM-DD'),'{3}')"
  
                , ERP_Supplier_Request_ID
                ,Status
                ,Execution_Date
                ,Remarks
                );
            try
            {
                DBAccess.RunSQL(sql);
               // eff = DBAccess.RunSQLReturnValInt("select max(id) from sam_erp_llinfoupdate");
                eff = int.Parse(ERP_Supplier_Request_ID);
            }
            catch (Exception ex) {
                eff = -1;
            }
            return eff;
        }
    }


}
