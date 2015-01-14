using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using BP.Sys;
using BP.DA;
using BP.En;
using BP.WF.Template;
using BP.WF;

namespace CCFlow.WF.Admin
{
    public partial class FlowFormTree : System.Web.UI.Page
    {
        public string getUTF8ToString(string param)
        {
            return HttpUtility.UrlDecode(Request[param], System.Text.Encoding.UTF8);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (BP.Web.WebUser.No != "admin")
                throw new Exception("@ Users must be made illegal admin To operate , Users are now logged :" + BP.Web.WebUser.No);
            
            string method = string.Empty;
            // The return value 
            string s_responsetext = string.Empty;
            if (!string.IsNullOrEmpty(Request["method"]))
                method = Request["method"].ToString();

            switch (method)
            {
                case "getflowformtree":
                    s_responsetext = GetFlowFormTree();
                    break;
                case "getnodeformtree":
                    s_responsetext = GetNodeFormTree();
                    break;
                case "saveflowformtree":
                    s_responsetext = SaveFlowFormTree();
                    break;
                case "savenodeformtree":
                    s_responsetext = SaveNodeFormTree();
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
        ///  Get the process tree form 
        /// </summary>
        /// <returns></returns>
        private string GetFlowFormTree()
        {
            string flowId = getUTF8ToString("flowId");
            string parentNo = getUTF8ToString("parentno");
            string isFirstLoad = getUTF8ToString("isFirstLoad");

            // Gets the child node content 
            SysFormTrees flowFormTrees = new SysFormTrees();
            QueryObject objInfo = new QueryObject(flowFormTrees);
            objInfo.AddWhere("ParentNo", parentNo);
            objInfo.addOrderBy("Idx");
            objInfo.DoQuery();
            
            if (isFirstLoad == "true")
            {
                SysFormTree formTree = new SysFormTree("0");
                StringBuilder appSend = new StringBuilder();
                appSend.Append("[");
                appSend.Append("{");
                appSend.Append("\"id\":\"0\"");
                appSend.Append(",\"text\":\"" + formTree.Name + "\"");

                appSend.Append(",iconCls:\"icon-0\"");
                appSend.Append(",\"children\":");
                appSend.Append("[");
                // Gets the node under 
                SysForms sysForms = new SysForms();
                QueryObject objFlowForms = new QueryObject(sysForms);
                objFlowForms.AddWhere(SysFormAttr.FK_FormTree, parentNo);
                objFlowForms.addOrderBy(SysFormAttr.Name);
                objFlowForms.DoQuery();
                
                // Add a subkey folder 
                foreach (SysFormTree item in flowFormTrees)
                {
                    // Get selected items 
                    FrmNodes flowForms = new FrmNodes();
                    QueryObject objFlowForm = new QueryObject(flowForms);
                    objFlowForm.AddWhere("FK_Flow", flowId);
                    objFlowForm.addAnd();
                    objFlowForm.AddWhere("FK_FlowFormTree", item.No);
                    objFlowForm.DoQuery();

                    if (flowForms != null && flowForms.Count > 0)
                    {

                    }
                }
                // Add form 
                foreach (SysForm sysForm in sysForms)
                {
                    appSend.Append("{");
                    appSend.Append("\"id\":\"0\"");
                    appSend.Append(",\"text\":\"" + formTree.Name + "\"");

                    appSend.Append(",iconCls:\"icon-3\"");
                    appSend.Append("},");
                }
                appSend.Append("]");
                appSend.Append("}");
                appSend.Append("]");
                return appSend.ToString();
            }

            return "";
        }
        /// <summary>
        ///  Get node tree form 
        /// </summary>
        /// <returns></returns>
        private string GetNodeFormTree()
        {
            return "";
        }
        /// <summary>
        ///  Save the process tree form 
        /// </summary>
        /// <returns></returns>
        private string SaveFlowFormTree()
        {
            return "";
        }
        /// <summary>
        ///  Save a tree node form 
        /// </summary>
        /// <returns></returns>
        private string SaveNodeFormTree()
        {
            return "";
        }

        /// <summary>
        ///  Get a list of nodes in the tree 
        /// </summary>
        /// <param name="ens"></param>
        /// <param name="checkIds"></param>
        /// <returns></returns>
        public string GetTreeList(Entities ens, string checkIds, string unCheckIds)
        {
            StringBuilder appSend = new StringBuilder();
            appSend.Append("[");
            foreach (EntityTree item in ens)
            {
                if (appSend.Length > 1) appSend.Append(",{"); else appSend.Append("{");

                appSend.Append("\"id\":\"" + item.No + "\"");
                appSend.Append(",\"text\":\"" + item.Name + "\"");

                SysFormTree node = item as SysFormTree;

                // Folders node icon 
                string ico = "icon-tree_folder";
                // Judgment is not fully checked 
                if (unCheckIds.Contains("," + item.No + ","))
                    ico = "collaboration";

                appSend.Append(",iconCls:\"");
                appSend.Append(ico);
                appSend.Append("\"");

                if (checkIds.Contains("," + item.No + ","))
                    appSend.Append(",\"checked\":true");

                // Determine whether there are child nodes icon-3
                //BP.GPM.Menus menus = new BP.GPM.Menus();
                //menus.RetrieveByAttr("ParentNo", item.No);

                //if (menus != null && menus.Count > 0)
                //{
                //    appSend.Append(",state:\"closed\"");
                //    appSend.Append(",\"children\":");
                //    appSend.Append("[{");
                //    appSend.Append(string.Format("\"id\":\"{0}\",\"text\":\"{1}\"", item.No + "01", " Loading ..."));
                //    appSend.Append("}]");
                //}
                appSend.Append("}");
            }
            appSend.Append("]");

            return appSend.ToString();
        }
    }
}