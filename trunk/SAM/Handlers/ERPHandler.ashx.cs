using BP.DA;
using SAM.ERPWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Jlob.OpenErpNet;
using BP.WF;

namespace SAM.Handlers
{
    /// <summary>
    /// Summary description for ERPHandler
    /// </summary>
    public class ERPHandler : IHttpHandler
    {

        string OpenErpServer = System.Configuration.ConfigurationManager.AppSettings["OpenErpServer"];
        string OpenerpDataBase = System.Configuration.ConfigurationManager.AppSettings["OpenerpDataBase"];
        string OpenerpUser = System.Configuration.ConfigurationManager.AppSettings["OpenerpUser"];
        string OpenerpPass = System.Configuration.ConfigurationManager.AppSettings["OpenerpPass"];

        OpenErpService openErpService {
            get {
                return new OpenErpService(OpenErpServer, OpenerpDataBase, OpenerpUser, OpenerpPass);
            }
        }

       

        private string getDataRowStr(DataRow dr, string columnname, string valueAsNull) {
            string val = valueAsNull;
            try
            {
                val = dr[columnname].ToString();
            }
            catch { }
            return val;
        }

        protected int UpdateSupplierByVal(string  FID,string FK_Node,long WorkID) {
            int eff = 0;
           // Work work = Dev2Interface.Flow_GetCurrentWork(WorkID);

            string ssql = string.Format("select * from ND{0} where OID={1}"
                 , FK_Node
            , WorkID
        
                );
            
            /*


             
             */
            DataTable dt = DBAccess.RunSQLReturnTable(ssql);

            foreach (DataRow dr in dt.Rows)
            {
                NakSamSupplier nss = new NakSamSupplier
                    {
                        acc_number = getDataRowStr(dr,"ACC_NUMBER", "")
                        ,
                        acc_owner = getDataRowStr(dr,"ACC_OWNER", "")
                        ,
                        bank_name = getDataRowStr(dr,"BANK_NAME", "")
                        ,
                        contact_ref = getDataRowStr(dr,"CONTRACT_REF", "")
                        ,
                        economic_id = getDataRowStr(dr,"ECONOMIC_ID", "")
                            // , id = dr[""].ToString()
                        ,
                        is_company = getDataRowStr(dr,"IS_COMPANY", "false").ToLower() == "true"
                        ,
                        name = getDataRowStr(dr, "NAME", "")
                        ,
                        name_en = getDataRowStr(dr,"LL_NAME_ENG", "")
                        ,
                        national_id = getDataRowStr(dr,"NATIONAL_ID", "")
                        ,
                        note = getDataRowStr(dr,"NOTE", "")
                        ,
                        requested_by = getDataRowStr(dr,"REQUESTED_BY", "")
                        ,
                        request_type = getDataRowStr(dr, "REQUEST_TYPE", "") //!= "0" ? "Update" : "New"
                        ,
                        reserved_01 = getDataRowStr(dr,"RESERVED_01", "")
                        ,
                        reserved_02 = getDataRowStr(dr,"RESERVED_02", "")
                        ,
                        reserved_03 = getDataRowStr(dr,"RESERVED_03", "")
                        ,
                        reserved_04 = getDataRowStr(dr,"RESERVED_04", "")
                        ,
                        reserved_05 = getDataRowStr(dr,"RESERVED_05", "")
                        ,
                        sam_db_id = getDataRowStr(dr,"SAM_DB_ID", "")
                        ,
                        sam_default = getDataRowStr(dr,"SAM_DEFAULT", "").ToString().ToLower() == "true"
                        ,shaba_number=getDataRowStr(dr,"SHABA_NUMBER","")                    
                        ,LL_ID = FID
                        ,address=getDataRowStr(dr,"ADDRESS","unknow")
                        ,country=getDataRowStr(dr,"COUNTRY","unknow")
                        ,city=getDataRowStr(dr,"CITY","unknow")
                     
                    };
                openErpService.AddEntity(nss);
                
                string srcCols = nss.id + ",REQUESTED_BY,SAM_DB_ID ,REQUEST_TYPE ,NAME ,'LL_NAME_ENG' ,IS_COMPANY ,NOTE ,BANK_NAME , ACC_NUMBER , SHABA_NUMBER , SAM_DEFAULT , NATIONAL_ID ,ECONOMIC_ID ",
        dstCols = "ERP_SUPPLIER_REQUEST_ID, REQUESTED_BY , SAM_DB_ID , REQUEST_TYPE ,LL_NAME ,NAME_EN , IS_COMPANY , ADDRESS , BANK_NAME ,ACC_NUMBER , SHEBA_NUMBER , SAM_DEFAULT , NATIONAL_ID , ECONOMIC_ID";

                string iqsql = string.Format("insert into SAM_ERP_LLINFO ({0},OID) select {1},{3} from ND{2} where OID={3}"
                    , dstCols
                    , srcCols
                    , FK_Node
                    , WorkID
                    );
                DBAccess.RunSQL(iqsql);

                string uqsql = string.Format("insert into sam_erp_llinfoupdate (ID,ERP_Supplier_Request_ID,OID) values (seq_sam_erp_llinfoupdate.nextval,'{0}',{1})"
                    ,nss.id,WorkID);
                DBAccess.RunSQL(uqsql);
                eff++;
            }
            return eff;
        }

        protected int UpdateSupplierByDtl(string FID,string DTL) {
            int eff = 0;
            string srcCols = "REQUESTED_BY,SAM_DB_ID ,SAM_ERP_LL_REQUEST_TYPE ,LL_FULLNAME ,LL_NAME_ENG ,SAM_ERP_LL_IS_COMPANY ,LL__ADD_NOTE ,BANK_NAME , ACC_NUMBER , SHEBA_NUMBER , SAM_DEFAULT , NATIONAL_ID , ECONOMIC_ID",
               dstCols = "REQUESTED_BY , SAM_DB_ID , REQUEST_TYPE ,LL_NAME ,NAME_EN , IS_COMPANY , ADDRESS , BANK_NAME ,ACC_NUMBER , SHEBA_NUMBER , SAM_DEFAULT , NATIONAL_ID , ECONOMIC_ID";

            
            string ssql = string.Format("select {1} from {2} where REFPK='{3}'"
                 , dstCols
            , srcCols
            , DTL
            , FID
                );

            DataTable dt = DBAccess.RunSQLReturnTable(ssql);

            foreach (DataRow dr in dt.Rows)
            {

                NakSamSupplier nss = new NakSamSupplier
                {
                    acc_number = dr["ACC_NUMBER"].ToString()
                    ,
                    acc_owner = ""
                    ,
                    bank_name = dr["BANK_NAME"].ToString()
                    ,
                    contact_ref = ""
                    ,
                    economic_id = dr["ECONOMIC_ID"].ToString()
                        // , id = dr[""].ToString()
                    ,
                    is_company = dr["SAM_ERP_LL_IS_COMPANY"].ToString().ToLower() == "true"
                    ,
                    name = dr["LL_FULLNAME"].ToString()
                    ,
                    name_en = dr["LL_NAME_ENG"].ToString()
                    ,
                    national_id = dr["NATIONAL_ID"].ToString()
                    ,
                    note = dr["LL__ADD_NOTE"].ToString()
                    ,
                    requested_by = dr["REQUESTED_BY"].ToString()
                    ,
                    request_type = dr["SAM_ERP_LL_REQUEST_TYPE"].ToString() == "0" ? "New" : "Update"
                    ,
                    reserved_01 = ""
                    ,
                    reserved_02 = ""
                    ,
                    reserved_03 = ""
                    ,
                    reserved_04 = ""
                    ,
                    reserved_05 = ""
                    ,
                    sam_db_id = dr["SAM_DB_ID"].ToString()
                    ,
                    sam_default = dr["SAM_DEFAULT"].ToString().ToString().ToLower() == "true"
                    ,
                    LL_ID = FID
                    //, shaba_number = dr["SHEBA_NUMBER"].ToString()
                };
                openErpService.AddEntity(nss);
                //string isql=string.Format("insert into SAM_ERP_LLINFO ({0})")

            }
            return eff;
        }

        public void UpdateSupplier(HttpRequest request, HttpResponse response)
        {
            string FID = request.Params["OID"], DTL = request.Params["DTL"]
                , NodeID = request.Params["NodeID"], _WorkID =request.Params["WorkID"]
                ;
            
            
            
            int eff = 0;
           // string qsql = string.Format("select * from {0} where FID='{1}'", DTL, FID);

           
            try
            {
                if (!string.IsNullOrWhiteSpace(DTL))
                {
                    eff += UpdateSupplierByDtl(FID, DTL);
                }
                else {
                    if (string.IsNullOrWhiteSpace(_WorkID)) {
                        _WorkID = request.Params["OID"];
                    }
                    if (string.IsNullOrWhiteSpace(NodeID)) {
                        NodeID = request.Params["FK_Node"];
                    }
                    long WorkID = long.Parse(_WorkID);
                    eff += UpdateSupplierByVal(FID,NodeID,WorkID);
                }
                string re = string.Format("rows :{0} created!", eff);
                response.Write(re);
            }
            catch (Exception ex) {
                response.Write(string.Format("url:{1} \n error:{2},{0}",ex.StackTrace,request.Url,ex.Message));
            }
        }



        public void CreateNewSPRForSign(HttpRequest request, HttpResponse response)
        {
            string FID = request.Params["OID"], DTL = request.Params["DTL"],
                re = "", _for = request.Params["for"],user=request.Params["UserNo"];
            string dtstr=DateTime.Now.ToString("yyyy-MM-dd");
           // string srcsql = string.Format("select * from Sam_Erp_Payment where OID='{0}'");
            int eff = 0;
            try
            {
                //DataTable dtDtl = DBAccess.RunSQLReturnTable(srcsql);
                List<NakSamSprLog> slogs=openErpService.GetEntities<NakSamSprLog>(e=>e.SAM_SPR_ParentID==FID).ToList();
                foreach(NakSamSprLog slog in slogs){
               // foreach (DataRow dtlRow in dtDtl.Rows)
               // {
                //    int SAM_SPR_ID =int.Parse(dtlRow["SAM_SPR_ID"].ToString());
                   // NakSamSprLog slog=openErpService.GetEntities<NakSamSprLog>(e => e.id == SAM_SPR_ID).Single();
                    switch(_for){
                        case "Approved":
                            { 
                            slog.Approved_By = user;
                            slog.Approved_By_Date = dtstr;
                            break; }
                        case "Initiated":
                            {
                                slog.Initiated_By = user;
                                slog.Initiated_By_Date = dtstr;
                                break;
                            }
                        case "Reviewed":
                            {
                                slog.Reviewed_By = user;
                                slog.Reviewed_By_Date = dtstr;
                                break;
                            }
                        case "Tech_Approved":
                            {
                                slog.Tech_Approved_By = user;
                                slog.Tech_Approved_By_Date = dtstr;
                                break;
                            }
                        default:
                            {
                                throw new Exception(string.Format("the is for:{0} is unkonw",_for));
                            }
                            break;
                    }
                    openErpService.UpdateEntity(slog);
                    eff++;
                }
                re = string.Format("{1} Signed by {2}:{0}", eff,_for,user);
            }
            catch (Exception ex) {
                re = string.Format("exception url:{0} \n error:{1} \n stack:{2} ", request.Url, ex.Message, ex.StackTrace);
            }
            response.Write(re);
        }

        /// <summary>

        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>

        public void CreateNewSPR(HttpRequest request,HttpResponse response) {
            string FID = request.Params["OID"],DTL=request.Params["DTL"];

            string sql = string.Format("select * from {0} where REFPK='{1}'",DTL,FID);
           // string columns="OID,REFPK,REC,RDT,FID,SITEID,LOCATION,AMOUNT,LANDLORDNAME,NATIONALID,ACCOUNTNUMBE,BANKNAME,VALIDITYFROM,VALIDITYTO,SHEBANO";
            //string isql=string.Format("insert into ERP_SPR ({0}) select {0} from {1} where FID='{2}'",columns,DTL,FID);
           // DBAccess.RunSQL(isql);
            string re="",nullstr="unknow";
            try
            {
                DataTable dtDtl = DBAccess.RunSQLReturnTable(sql);
                string dtstr = DateTime.Now.ToString("yyyy-MM-dd");
                //ERPWebServiceSoapClient erpservice = new ERPWebServiceSoapClient();
                int eff = 0;
                string Requested_By = "SAM9402", rdt = DateTime.Now.ToShortDateString();
                foreach (DataRow dtlRow in dtDtl.Rows)
                {
                    string SITEID = dtlRow["SITE_ID"].ToString()
                        , OID = dtlRow["OID"].ToString()
                        , AMOUNT = dtlRow["AMOUNT"].ToString()
                        , BANKNAME = dtlRow["BANKNAME"].ToString()
                        , VALIDITYFROM = dtlRow["VALIDITYFROM"].ToString()
                        , VALIDITYTO = dtlRow["VALIDITYTO"].ToString()
                        , ACCOUNTNUMBE = dtlRow["ACCOUNTNUMBE"].ToString()
                        , NATIONALID = dtlRow["NATIONALID"].ToString()
                        , LANDLORDNAME = dtlRow["LANDLORDNAME"].ToString()
                        , SUPPLIER_ID = dtlRow["SUPPLIER_ID"].ToString()
                        , SAM_CONTRACT_ID = dtlRow["SAM_CONTRACT_ID"].ToString()
                        //, SPR_Type = "RENT"//dtlRow["SPR_TYPE"].ToString()
                        , SPR_Type = dtlRow["SPR_TYPE"].ToString()
                        //, TOTAL_SPR_PASSED =dtDtl.Rows.Count.ToString()// dtlRow["TOTAL_SPR_PASSED"].ToString()
                        , TOTAL_SPR_PASSED = dtlRow["TOTAL_SPR_PASSED"].ToString()

                        , Due_Date = dtlRow["DUE_DATE"].ToString()
                        , ApprovedBy = dtlRow["APPROVED_BY"].ToString()
                        , Tech_Approved_By = dtlRow["TECH_APPROVED_BY"].ToString()
                        , Reviewed_By = dtlRow["REVIEWED_BY"].ToString()
                        , Initiated_By = dtlRow["INITIATED_BY"].ToString()
                        , Approved_By_Date = dtlRow["APPROVED_BY_DATE"].ToString()
                        , Reviewed_By_Date = dtlRow["REVIEWED_BY_DATE"].ToString()//	REVIEWED_BY_DATE
                        , Initiated_By_Date = dtlRow["INITIATED_BY_DATE"].ToString()//INITIATED_BY_DATE
                        , Tech_Approved_By_Date = dtlRow["TECH_APPROVED_BY_DATE"].ToString()//	TECH_APPROVED_BY_DATE
                        , Is_Check_Only = dtlRow["IS_CHECK_ONLY"].ToString()
                        , Is_Net = dtlRow["IS_NET"].ToString()
                        , SAM_SPR_ID = dtlRow["SAM_SPR_ID"].ToString()
                        ;


                    string guid = Guid.NewGuid().ToString();
                    NakSamSprLog sprlog = new NakSamSprLog
                    {
                         SAM_SPR_ID = SAM_SPR_ID
                        ,Requested_By = Requested_By
                        ,Supplier_ID = SUPPLIER_ID //SITEID
                        ,Ammount = AMOUNT
                        ,Payment_Validity_From = VALIDITYFROM
                        ,Payment_Validity_To = VALIDITYTO
                        ,Supplier_Acc_Bank = BANKNAME
                        ,Supplier_Acc_NO = ACCOUNTNUMBE
                        ,Supplier_ACC_SHEBA=nullstr
                        ,Request_Date = dtstr
                        ,SAM_Contract_ID = SAM_CONTRACT_ID
                        ,Site_ID=SITEID
                        ,SAM_SPR_ParentID =FID // OID
                        ,SAM_SPR_ParentDate = dtstr
                        ,Tech_Approved_By = Tech_Approved_By
                        ,Total_SPR_Passed =TOTAL_SPR_PASSED
                        ,Reserved_01 =nullstr
                        ,Reserved_02 = nullstr
                        ,Reserved_03 = nullstr
                        ,Reserved_04= nullstr
                        ,reserved_4 = nullstr
                        ,Approved_By = ApprovedBy
                        ,Reviewed_By = Reviewed_By
                        ,Initiated_By = Initiated_By
                        ,Site_Address= nullstr
                        ,SPR_Type = SPR_Type
                        ,Due_Date=Due_Date
                        ,Approved_By_Date= Approved_By_Date
                        ,Reviewed_By_Date = Reviewed_By_Date
                        ,Initiated_By_Date = Initiated_By_Date
                        ,Tech_Approved_By_Date = Tech_Approved_By_Date
                        ,Is_Check_Only = Is_Check_Only
                        ,Is_Net = Is_Net
 
                    };


                    openErpService.AddEntity(sprlog);
              

                    string issql = string.Format("insert into Sam_Erp_Payment (PaymentRef,SPR_Type,Requested_By,Ammount,Request_Date,Site_ID,Payment_Validity_From," +
                                                 "Payment_Validity_To,Supplier_ACC_SHEBA,Site_Address,Bank_name,LAndLordName,NationalID,AccontNumber, SAM_SPR_ID,OID) " +
                                         "values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}',{15})"
                          , ""
                          , ""
                          , Requested_By
                          , AMOUNT
                          , rdt
                          , SITEID
                          , VALIDITYFROM
                          , VALIDITYTO
                          , ""
                          , ""
                          , BANKNAME
                          , LANDLORDNAME
                          , NATIONALID
                          , ACCOUNTNUMBE
                          , SAM_SPR_ID
                          ,FID
                        );
                    eff += DBAccess.RunSQL(issql);

                    //SPR spr = new SPR(); //erpservice.CreateNewSPR(SITEID, "", Requested_By, Guid.NewGuid().ToString(), AMOUNT, rdt, "", SITEID, "", "", "", VALIDITYFROM, VALIDITYTO, "", "", "", "", "", "", "", "", "");

                    string td = "TO_DATE('" + DateTime.Now.ToString("dd-MM-yy") + "','DD-MM-YY')";
                    string sql2 =
                        string.Format(
                            "insert into Sam_Erp_payment_update (ERP_SPR_ID,Status,Status_Updated_date,Status_Recieved_date,PaymentRef,Remarks,OID,SAM_SPR_ID) values ('{0}','{1}',{2},{2},'{3}','{4}',{5},'{6}')"
                            , sprlog.id, "toSubmit", td, "unknown", "nothing",FID,SAM_SPR_ID
                        );
                    eff = DBAccess.RunSQL(sql2);

                    string usql = string.Format("update Sam_Erp_Payment set PaymentRef='{0}' where Site_ID='{1}'"

                        , "unknown"

                        , SITEID
                        );
                    eff += DBAccess.RunSQL(usql);
                    //PaymentRef,SPR_Type,Requested_By,Ammount,Request_Date,Site_ID,Payment_Validity_From,Payment_Validity_To,Supplier_ACC_SHEBA,Site_Address,Bank_name,LAndLordName,NationalID,AccontNumber




                }


                re = string.Format("rows :{0}, {1} SPR created!" + sql, dtDtl.Rows.Count, eff);
            }
            catch (Exception ex) {
                re = string.Format("exception url:{0} \n error:{1} \n stack:{2} ", request.Url,ex.Message,ex.StackTrace);
            }
                response.Write(re);
            //string param = "";
            //foreach(string key in request.Params)
            //{
            //    param += key + ":" + request.Params[key] + ",";
            //}
            // sql = string.Format("insert into WS_DEBUG (url,params,result) values ('{0}','{1}','{2}')"
            //    ,request.Url
            //    ,param
            //    ,re
            //    );
            //DBAccess.RunSQL(sql);
            //response.Write("1");
        }


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            string action = request.Params["action"];
            this.GetType().GetMethod(action).Invoke(this, new object[] {request,response });
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}