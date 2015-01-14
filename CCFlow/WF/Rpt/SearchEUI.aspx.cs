using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BP.En;
using BP.DA;
using BP.Sys;
using BP.Web;

namespace CCFlow.WF.Rpt
{
    public partial class SearchEUI : System.Web.UI.Page
    {
        #region   Property 
        /// <summary>
        ///  Get incoming parameters 
        /// </summary>
        /// <param name="param"> Parameter name </param>
        /// <returns></returns>
        public string getUTF8ToString(string param)
        {
            return HttpUtility.UrlDecode(Request[param], System.Text.Encoding.UTF8);
        }
        /// <summary>
        ///  Form Number 
        /// </summary>
        public string FK_MapData
        {
            get
            {
                return this.Request["FK_MapData"];
            }
        }
        public Entities _HisEns = null;
        public Entities HisEns
        {
            get
            {
                if (_HisEns == null)
                {
                    if (this.FK_MapData != null)
                    {
                        if (this._HisEns == null)
                            _HisEns = BP.En.ClassFactory.GetEns(this.FK_MapData);
                    }
                }
                return _HisEns;
            }
        }
        #endregion

        protected override void OnInitComplete(EventArgs e)
        {
            MapData md = new MapData();
            md.No = this.FK_MapData;
            md.RetrieveFromDBSources();
            this.ToolBar1._AddSearchBtn = false;
            this.ToolBar1.InitByMapV2(md.EnMap, 1);

            base.OnInitComplete(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string method = string.Empty;
            // The return value 
            string s_responsetext = string.Empty;
            if (string.IsNullOrEmpty(Request["method"]))
                return;
            method = Request["method"].ToString();
            switch (method)
            {
                case "getnewid":// Get a new number 
                    s_responsetext = GetNewOID();
                    break;
                case "getensgriddata":// Get a list of data 
                    s_responsetext = GetEnsGridData();
                    break;
                case "deleteentity":// Delete Record 
                    s_responsetext = DeleteEnsData();
                    break;
            }
            if (string.IsNullOrEmpty(s_responsetext))
                s_responsetext = "";
            // Assembly ajax String format , Return to the calling client 
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "text/html";
            Response.Expires = 0;
            Response.Write(s_responsetext);
            Response.End();
        }

        /// <summary>
        ///  Get a new number 
        /// </summary>
        /// <returns></returns>
        private string GetNewOID()
        {
            string intKey = getUTF8ToString("FK_MapData");
            return DBAccess.GenerOID(intKey).ToString();
        }

        /// <summary>
        ///  Delete Record 
        /// </summary>
        /// <returns></returns>
        private string DeleteEnsData()
        {
            string oid = getUTF8ToString("OID");
            int i = HisEns.Delete("OID", oid);
            return i > 0 ? "true" : "false";
        }

        /// <summary>
        ///  Get a list of data 
        /// </summary>
        /// <returns></returns>
        private string GetEnsGridData()
        {
            int totalRows = 0;
            // Configuring the column name 
            FrmReportFields rePortFields = new FrmReportFields();
            QueryObject objFields = new QueryObject(rePortFields);
            objFields.AddWhere(FrmReportFieldAttr.FK_MapData, this.FK_MapData);
            objFields.addOrderBy(FrmReportFieldAttr.Idx);
            objFields.DoQuery();
            if (rePortFields.Count == 0) return "";
            // Entity column name 
            Attrs mdAttrs = HisEns.GetNewEntity.EnMap.Attrs;
            // Returns a string 
            StringBuilder append = new StringBuilder();
            append.Append("{");
            // Finishing the column name 
            append.Append("columns:[");
            foreach (FrmReportField field in rePortFields)
            {
                foreach (Attr mdAttr in mdAttrs)
                {
                    if (field.KeyOfEn == mdAttr.Key)
                    {
                        if (field.UIVisible == false)
                        {
                            append.Append("{");
                            append.Append(string.Format("field:'{0}',title:'{1}',width:{2},hidden: true", field.KeyOfEn + "Text", field.Name, field.UIWidth));
                            append.Append("},");
                            continue;
                        }
                        if (mdAttr.IsRefAttr || mdAttr.IsFK || mdAttr.IsEnum)
                        {
                            append.Append("{");
                            append.Append(string.Format("field:'{0}',title:'{1}',width:{2},sortable:true", field.KeyOfEn + "Text", field.Name, field.UIWidth));
                            append.Append("},");
                            continue;
                        }
                        append.Append("{");
                        append.Append(string.Format("field:'{0}',title:'{1}',width:{2},sortable:true", mdAttr.Key, field.Name, field.UIWidth));
                        append.Append("},");
                    }
                }
            }
            if (append.Length > 10)
                append = append.Remove(append.Length - 1, 1);
            append.Append("]");

            // Organize data 
            bool bHaveData = false;
            append.Append(",data:{rows:[");

            // Get Data 
            MapData md = new MapData();
            md.No = this.FK_MapData;
            if (md.RetrieveFromDBSources() > 0)
            {
                // Current Page 
                string pageNumber = getUTF8ToString("pageNumber");
                int iPageNumber = string.IsNullOrEmpty(pageNumber) ? 1 : Convert.ToInt32(pageNumber);
                // How many rows per page 
                string pageSize = getUTF8ToString("pageSize");
                int iPageSize = string.IsNullOrEmpty(pageSize) ? 9999 : Convert.ToInt32(pageSize);

                QueryObject obj = new QueryObject(HisEns);
                obj = this.ToolBar1.GetnQueryObject(HisEns, HisEns.GetNewEntity);
                totalRows = obj.GetCount();
                // Inquiry 
                obj.DoQuery(HisEns.GetNewEntity.PK, iPageSize, iPageNumber);
                foreach (Entity en in HisEns)
                {
                    bHaveData = true;
                    append.Append("{");
                    foreach (Attr attr in mdAttrs)
                    {
                        if (attr.IsRefAttr || attr.IsFK || attr.IsEnum)
                        {
                            append.Append(attr.Key + "Text:'" + en.GetValRefTextByKey(attr.Key) + "',");
                            continue;
                        }
                        append.Append(attr.Key + ":'" + en.GetValStrByKey(attr.Key) + "',");
                    }
                    append = append.Remove(append.Length - 1, 1);
                    append.Append("},");
                }
            }

            if (append.Length > 11 && bHaveData)
                append = append.Remove(append.Length - 1, 1);

            append.Append("],total:" + totalRows + "}");
            append.Append("}");
            
            return ReplaceIllgalChart(append.ToString());
        }

        protected void ToolBar1_ButtonClick(object sender, System.EventArgs e)
        {
            this.ToolBar1.SaveSearchState(this.FK_MapData, null);
        }

        /// <summary>
        ///  Remove the special characters 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ReplaceIllgalChart(string s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0, j = s.Length; i < j; i++)
            {

                char c = s[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '/':
                        sb.Append("\\/");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }
    }
}