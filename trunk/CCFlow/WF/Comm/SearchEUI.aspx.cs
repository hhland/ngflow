using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BP.En;
using BP.DA;
using BP.Web;
using BP.Web.Controls;
using BP.Sys;
using BP.Tools;

namespace CCFlow.WF.Comm
{
    public partial class SearchEUI : System.Web.UI.Page
    {
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
        ///  Entity Name 
        /// </summary>
        public new string EnsName
        {
            get
            {
                return getUTF8ToString("EnsName");
            }
        }
        /// <summary>
        /// Entity Name 
        /// </summary>
        public new string EnName
        {
            get
            {
                Entity en = GetEntityByEnName(EnsName);
                if (en != null)
                    return en.ClassID;
                return "";
            }
        }
        /// <summary>
        ///  Explanation 
        /// </summary>
        public string EnsDesc
        {
            get
            {
                Entity en = GetEntityByEnName(EnsName);
                if (this.EnsName == null || en == null)
                    return " Entity name ";
                return en.EnDesc;
            }
        }
        /// <summary>
        ///  Entity primary key 
        /// </summary>
        public string EnPK
        {
            get
            {
                Entity en = GetEntityByEnName(EnsName);
                if (this.EnsName == null || en == null)
                    return "No";
                return en.PK;
            }
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
                case "getensgriddata":// Get a list of data 
                    s_responsetext = GetEnsGridData();
                    break;
                case "delSelected":// Get a list of data 
                    s_responsetext = EnsDel();
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
        protected override void OnInitComplete(EventArgs e)
        {
            InitToolBar();
            base.OnInitComplete(e);
        }

        protected void ToolBar1_ButtonClick(object sender, System.EventArgs e)
        {
            this.ToolBar1.SaveSearchState(this.EnsName, null);
        }
        /// <summary>
        ///  Delete Select Data 
        /// </summary>
        /// <returns></returns>
        private string EnsDel()
        {
            Entity en = GetEntityByEnName(EnsName);
            Entities ens = en.GetNewEntities;

            string GetDelOid = getUTF8ToString("GetDelOid");
            if (!string.IsNullOrEmpty(GetDelOid))
            {
                return en.Delete(en.PKField, GetDelOid).ToString();
            }
            return "0";
        }
        /// <summary>
        ///  Get ens Data 
        /// </summary>
        /// <returns></returns>
        private string GetEnsGridData()
        {
            Entity en = GetEntityByEnName(EnsName);
            Entities ens = en.GetNewEntities;

            //string GetDelOid=getUTF8ToString("GetDelOid");
            //if (!string.IsNullOrEmpty(GetDelOid))
            //{
            //    en.Delete(en.PKField, GetDelOid);
            //}


            // Total number of rows 
            int RowCount = 0;
            try
            {
                // Current Page 
                string pageNumber = getUTF8ToString("pageNumber");
                int iPageNumber = string.IsNullOrEmpty(pageNumber) ? 1 : Convert.ToInt32(pageNumber);
                // How many rows per page 
                string pageSize = getUTF8ToString("pageSize");
                int iPageSize = string.IsNullOrEmpty(pageSize) ? 9999 : Convert.ToInt32(pageSize);

                QueryObject obj = new QueryObject(ens);
                obj = this.ToolBar1.GetnQueryObject(ens, en);
                RowCount = obj.GetCount();
                // Inquiry 
                obj.DoQuery(en.PK, iPageSize, iPageNumber);

                return Entitis2Json.ConvertEntitis2GridJsonAndData(ens, RowCount);
            }
            catch
            {
                try
                {
                    en.CheckPhysicsTable();
                }
                catch (Exception wx)
                {
                    BP.DA.Log.DefaultLogWriteLineError(wx.Message);
                }
            }
            return "{[]}";
        }

        // Initialization inquiry 
        private void InitToolBar()
        {
            Entity en = GetEntityByEnName(EnsName);
            if (en != null)
            {
                this.ToolBar1._AddSearchBtn = false;
                this.ToolBar1.InitByMapV2(en.EnMap, 1);
            }
        }

        /// <summary>
        ///  Get entity by name 
        /// </summary>
        /// <param name="EnName"></param>
        /// <returns></returns>
        private Entity GetEntityByEnName(string EnName)
        {
            Entities _HisEns = null;
            Entity _HisEn = null;
            if (EnName != null)
            {
                _HisEns = ClassFactory.GetEns(EnName.Replace("#", ""));
                if (_HisEns == null)
                {
                    _HisEn = ClassFactory.GetEn(EnName.Replace("#", ""));
                    if (_HisEn == null)
                        throw new Exception(" Did not find the namespace and class met in this project :" + EnName);
                }
                else
                    _HisEn = _HisEns.GetNewEntity;
            }
            return _HisEn;
        }
    }
}