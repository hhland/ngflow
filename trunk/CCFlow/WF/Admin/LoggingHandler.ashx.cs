using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace CCFlow.WF.Admin
{
    /// <summary>
    /// Summary description for LoggingHandler
    /// </summary>
    public class LoggingHandler : IHttpHandler
    {

        public void readSLLog(HttpRequest request, HttpResponse response)
        {
           // response.Write(LoggerHelper.Read());
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            response.ContentType = "text/plain";
            string action = request.Params["action"];
            this.GetType().GetMethod(action).Invoke(this, new object[] {request,response});
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