using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCFlow.WF.MapDef
{
    using BP.DA;

    /// <summary>
    /// MapExt 的摘要说明
    /// </summary>
    public class MapExt_h: IHttpHandler
    {
        protected string FK_MapData, OperAttrKey, Event, ExtType;

        public void updateDoc(HttpRequest req, HttpResponse res)
        {
            string doc = req.Form["doc"];
            string MyPK = req.Params["MyPK"];
            string sql = string.Format("update SYS_MAPEXT set DOC='{0}' where MYPK='{1}'"
                ,doc
                ,MyPK
                );
            int i= DBAccess.RunSQL(sql);
            string re = "{code:"+i+"}";
            res.Write(re);
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest req = context.Request;
            HttpResponse res = context.Response;
            res.ContentType = "text/html";
            string action = req.Params["action"];
            FK_MapData = req.Params["FK_MapData"];
            OperAttrKey = req.Params["OperAttrKey"];
            Event = req.Params["event"];
            ExtType = req.Params["ExtType"];
            this.GetType().GetMethod(action).Invoke(this, new object[] { req, res });

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