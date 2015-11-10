using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BP;
using BP.En;
using BP.DA;
using BP.Sys;
using BP.WF;
using BP.Web;
namespace CCFlow.SDKFlowDemo.SDK.F115
{
    /// <summary>
    /// Serv  The summary 
    /// </summary>
    public class Serv : IHttpHandler
    {
        #region  Parameters .
        /// <summary>
        ///  Package on the individual  HTTP  All requests  HTTP  Specific information 
        /// </summary>
        HttpContext MyContext = null;
        public string getUTF8ToString(string param)
        {
            return HttpUtility.UrlDecode(MyContext.Request[param], System.Text.Encoding.UTF8);
        }
        public string DoFunc
        {
            get
            {
                return getUTF8ToString("DoFunc");
            }
        }
        public string DoType
        {
            get
            {
                return getUTF8ToString("DoType");
            }
        }
        public string CFlowNo
        {
            get
            {
                return getUTF8ToString("CFlowNo");
            }
        }
        public string WorkIDs
        {
            get
            {
                return getUTF8ToString("WorkIDs");
            }
        }
        public string FK_Flow
        {
            get
            {
                return getUTF8ToString("FK_Flow");
            }
        }
        public int FK_Node
        {
            get
            {
                string fk_node = getUTF8ToString("FK_Node");
                if (!string.IsNullOrEmpty(fk_node))
                    return Int32.Parse(getUTF8ToString("FK_Node"));
                return 0;
            }
        }
        public Int64 WorkID
        {
            get
            {
                return Int64.Parse(getUTF8ToString("WorkID"));
            }
        }
        #endregion  Parameters .

        public void ProcessRequest(HttpContext context)
        {
            MyContext = context;
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
            if (BP.Web.WebUser.No == null)
                return;

            string result = "";
            MyContext = context;
            string doType = MyContext.Request["DoType"].ToString();
            if (string.IsNullOrEmpty(doType) == true)
                doType = "Save";

            switch (doType)
            {
                case "Send":// Send 
                    result = Send();
                    break;
                case "Save":// Save 
                    result = Save();
                    break;
                default:
                    break;
            }

            // Export .
            this.OutHtml(result);
        }
        public void OutHtml(string msg)
        {
            // Assembly ajax String format , Return to the calling client 
            MyContext.Response.Charset = "UTF-8";
            MyContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
            MyContext.Response.ContentType = "text/html";
            MyContext.Response.Expires = 0;
            MyContext.Response.Write(msg);
            MyContext.Response.End();
        }
        private string Save()
        {
            try
            {
                //BP.Demo.ND112Rpt rpt = new BP.Demo.ND112Rpt(this.WorkID);
                //rpt = PubClass.CopyFromRequest(rpt) as BP.Demo.ND112Rpt;
                //rpt.Update();
                return " Saved successfully ...";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        ///  Send cases  
        /// </summary>
        /// <returns></returns>
        private string Send()
        {
            // First, the save .
            this.Save();
            string resultMsg = "";
            try
            {
                if (Dev2Interface.Flow_IsCanDoCurrentWork(this.FK_Flow, this.FK_Node, this.WorkID, WebUser.No) == false)
                {
                    resultMsg = "error| Hello :" + BP.Web.WebUser.No + ", " + WebUser.Name + " The current work has been processed , Or you do not have permission to perform this work .";
                }
                else
                {
                    SendReturnObjs returnObjs = Dev2Interface.Node_SendWork(this.FK_Flow, this.WorkID);
                    //  resultMsg = returnObjs.ToMsgOfHtml();
                    resultMsg = returnObjs.ToMsgOfHtml();

                    #region  Business logic methods to handle the general sent successfully after , This method may throw an exception .
                    /* There are two cases 
                     * 1, From the intermediate node , By batch processing , That is the case of the merger approval process , In this case you need to perform the sub process to the next step .
                       2, From the process has been completed , Or is running , That is the case of the merger approval process . */
                    try
                    {
                        // Business logic methods to handle the general sent successfully after , This method may throw an exception .
                        BP.WF.Glo.DealBuinessAfterSendWork(this.FK_Flow, this.WorkID, this.DoFunc, WorkIDs, this.CFlowNo,0,null);
                    }
                    catch (Exception ex)
                    {
                        resultMsg = "sysError|" + ex.Message.Replace("@", "<br/>");
                        return resultMsg;
                    }
                    #endregion  Business logic methods to handle the general sent successfully after , This method may throw an exception .
                }
            }
            catch (Exception ex)
            {
                resultMsg = "sysError|" + ex.Message.Replace("@", "<br/>");
            }
            return resultMsg;
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