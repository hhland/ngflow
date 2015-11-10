using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using BP.DA;

namespace CCFlow.WF.CCForm
{
    /// <summary>
    /// Summary description for Dtl
    /// </summary>
    public class Dtl : IHttpHandler
    {


        public void updateDtl(HttpRequest request, HttpResponse response)
        {
            string oid= request.Params["oid"]
                , EnsName = request.Params["EnsName"]
                , fieldname = request.Params["fieldname"]
                , fieldval = request.Params["fieldval"]
             ;
            string sql = string.Format("update {0} set {1}='{2}' where oid='{3}'"
                ,EnsName
                ,fieldname
                ,fieldval
                ,oid
                
                );
            int eff = 0;
            JObject re = new JObject();
            re["eff"] = eff;
            try
            {
                eff += DBAccess.RunSQL(sql);
                re["eff"] = eff;
            }
            catch (Exception ex) {
                re["errormsg"] = ex.Message;
            }
            response.Write(re.ToString());
        }


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
           HttpRequest  request = context.Request;
           HttpResponse response = context.Response;
            string action = request.Params["action"];
            this.GetType().GetMethod(action).Invoke(this, new object[] { request, response });
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