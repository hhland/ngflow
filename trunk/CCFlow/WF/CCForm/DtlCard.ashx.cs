using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCFlow.WF.CCForm
{
    using BP.En;
    using BP.Sys;

    /// <summary>
    /// DtlCard 的摘要说明
    /// </summary>
    public class DtlCard : IHttpHandler
    {

        

        protected void addRow(HttpRequest req,HttpResponse res)
        {
            string EnsName = req.Params["EnsName"], RefPKVal = req.Params["RefPKVal"];
            int addRowNum = int.Parse(req.Params["addRowNum"]);
            MapDtl mdtl = new MapDtl(EnsName);
            GEDtls dtls = new GEDtls(EnsName);

            QueryObject qo = null;
            try
            {
                qo = new QueryObject(dtls);
                switch (mdtl.DtlOpenType)
                {
                    case DtlOpenType.ForEmp:
                        qo.AddWhere(GEDtlAttr.RefPK, RefPKVal);
                        break;
                    case DtlOpenType.ForWorkID:
                        qo.AddWhere(GEDtlAttr.RefPK, RefPKVal);
                        break;
                    case DtlOpenType.ForFID:
                        qo.AddWhere(GEDtlAttr.FID, RefPKVal);
                        break;
                }
                qo.addOrderBy("oid");
                qo.DoQuery();
            }
            catch (Exception ex)
            {
                dtls.GetNewEntity.CheckPhysicsTable();
                throw ex;

                //#region  Solve Access  The problem does not refresh .
                //string rowUrl = this.Request.RawUrl;
                //if (rowUrl.IndexOf("rowUrl") > 1)
                //{
                //    throw ex;
                //}
                //else
                //{
                //    //this.Response.Redirect(rowUrl + "&rowUrl=1&IsWap=" + this.IsWap, true);
                //    return;
                //}
                //#endregion
            }

            mdtl.RowsOfList = mdtl.RowsOfList + addRowNum;
            int num = dtls.Count;
            if (mdtl.IsInsert)
            {
                int dtlCount = dtls.Count;
                for (int i = 0; i < mdtl.RowsOfList - dtlCount; i++)
                {
                    BP.Sys.GEDtl dt = new GEDtl(EnsName);
                    dt.ResetDefaultVal();
                    dt.OID = i;
                    dtls.AddEntity(dt);
                }

                if (num == mdtl.RowsOfList)
                {
                    BP.Sys.GEDtl dt1 = new GEDtl(EnsName);
                    dt1.ResetDefaultVal();
                    dt1.OID = mdtl.RowsOfList + 1;
                    dtls.AddEntity(dt1);
                }
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            HttpRequest req = context.Request;
            string action=req.QueryString["action"];

            switch (action)
            {
                case "addRow":
                    this.addRow(context.Request,context.Response);
                    break;
                case "deleteRow":
                    
                    break;
                default:break;
            }
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