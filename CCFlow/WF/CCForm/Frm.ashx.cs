using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCFlow.WF.CCForm
{
    using System.Data;
    using System.Reflection;

    using BP.DA;
    using BP.Tools;

    /// <summary>
    /// Frm 的摘要说明
    /// </summary>
    public class Frm : IHttpHandler
    {
        public static string FIELD_PREFIX = "field_";
        protected string FK_MapData, FID;

        public void isexist(HttpRequest req, HttpResponse res)
        {
            res.ContentType = "text/javascript";
            string condition = "";
            foreach (string field in req.Params.AllKeys.Where(k=>k.StartsWith(FIELD_PREFIX)))
            {
                string val = req.Params[field];
                string column = field.Substring(FIELD_PREFIX.Length, field.Length - FIELD_PREFIX.Length);
                condition += string.Format(" and {0}='{1}' ",column,val);
            }
            string tsql = string.Format("select PTABLE from SYS_MAPDATA where NO='{0}'",FK_MapData);

            string ptable = DBAccess.RunSQLReturnString(tsql);

            string sql = string.Format("select * from {0} where 1=1 "
                ,ptable
                );
            DataTable dt= DBAccess.RunSQLReturnTable(sql+condition);
            bool exist = dt.Rows.Count > 0;
            string re = "{isexist:"+exist+"}";
            res.Write(re.ToLower());
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest req = context.Request;
            HttpResponse res = context.Response;
            string action = req.Params["action"];
            FK_MapData = req.Params["FK_MapData"];
            FID = req.Params["FID"];
            MethodInfo method = this.GetType().GetMethod(action);
            method.Invoke(this, new object[] { req, res });
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