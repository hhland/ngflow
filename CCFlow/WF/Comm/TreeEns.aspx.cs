using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using BP.En;
using BP.DA;
using BP.Web;
using BP.Web.Controls;
using BP.Sys;

namespace CCOA.Comm
{
    public partial class TreeEns : System.Web.UI.Page
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
        ///  Tree entity name 
        /// </summary>
        public string TreeEnsName
        {
            get
            {
                return getUTF8ToString("TreeEnsName");
            }
        }
        public string RefPK
        {
            get
            {
                return getUTF8ToString("RefPK");
            }
        }
        /// <summary>
        ///  Entity Name 
        /// </summary>
        public string EnsName
        {
            get
            {
                return getUTF8ToString("EnsName");
            }
        }
        /// <summary>
        ///  Tree entity name 
        /// </summary>
        public string TreeEnName
        {
            get
            {
                Entity en = GetEntityByEnName(TreeEnsName);
                if (en != null)
                    return en.ClassID;
                return "";
            }
        }
        /// <summary>
        ///  Tree explanation 
        /// </summary>
        public string TreeEnsDesc
        {
            get
            {
                Entity en = GetEntityByEnName(TreeEnsName);
                if (this.TreeEnsName == null || en == null)
                    return " Tree ";
                return en.EnDesc;
            }
        }
        /// <summary>
        /// Entity Name 
        /// </summary>
        public string EnName
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
                case "gettreenodes":// Get a tree node 
                    s_responsetext = GetTreeNodes();
                    break;
                case "getensgriddata":// Get a list of data 
                    s_responsetext = GetEnsGridData();
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
        ///  Get a tree node 
        /// </summary>
        /// <returns></returns>
        private string GetTreeNodes()
        {
            try
            {
                string parentNo = getUTF8ToString("ParentNo");
                Entity en = GetEntityByEnName(TreeEnsName);

                Entities ens = en.GetNewEntities;
                ens.RetrieveAll(EntityTreeAttr.Idx);

                TansEntitiesToGenerTree(ens, parentNo);
                return appendMenus.ToString();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(" Target invocation exception occurs "))
                    return "error:" + ex.Message + "  Or the user is not logged .";

                return "error:" + ex.Message;
            }
        }

        /// <summary>
        ///  Get ens Data 
        /// </summary>
        /// <returns></returns>
        private string GetEnsGridData()
        {
            string RefPK = getUTF8ToString("RefPK");
            string FK = getUTF8ToString("FK");

            Entity en = GetEntityByEnName(EnsName);
            Entities ens = en.GetNewEntities;
            ens.RetrieveByAttr(RefPK, FK);

            return TranslateEntitiesToGridJsonColAndData(ens);
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
                _HisEns = BP.En.ClassFactory.GetEns(EnName.Replace("#", ""));
                if (_HisEns == null)
                {
                    _HisEn = BP.En.ClassFactory.GetEn(EnName.Replace("#", ""));
                    if (_HisEn == null)
                        throw new Exception(" Did not find the namespace and class met in this project :" + EnName);
                }
                else
                    _HisEn = _HisEns.GetNewEntity;
            }
            return _HisEn;
        }

        /// <summary>
        ///  The entity class into json Format   Contains the column names and data 
        /// </summary>
        /// <param name="ens"></param>
        /// <returns></returns>
        public string TranslateEntitiesToGridJsonColAndData(BP.En.Entities ens)
        {
            Attrs attrs = ens.GetNewEntity.EnMap.Attrs;
            StringBuilder append = new StringBuilder();
            append.Append("{");
            // Finishing the column name 
            append.Append("columns:[");
            foreach (Attr attr in attrs)
            {
                if (attr.IsRefAttr || attr.UIVisible == false)
                    continue;

                if (attr.Key == this.RefPK)
                    continue;

                append.Append("{");
                append.Append(string.Format("field:'{0}',title:'{1}',width:{2},sortable:true", attr.Key, attr.Desc, attr.UIWidth * 2));
                append.Append("},");
            }
            if (append.Length > 10)
                append = append.Remove(append.Length - 1, 1);
            append.Append("]");

            // Organize data 
            bool bHaveData = false;
            append.Append(",data:[");
            foreach (Entity en in ens)
            {
                bHaveData = true;
                append.Append("{");
                foreach (Attr attr in attrs)
                {
                    if (attr.IsRefAttr || attr.UIVisible == false)
                        continue;
                    append.Append(attr.Key + ":\"" + en.GetValStrByKey(attr.Key) + "\",");
                }
                append = append.Remove(append.Length - 1, 1);
                append.Append("},");
            }
            if (append.Length > 11 && bHaveData)
                append = append.Remove(append.Length - 1, 1);
            append.Append("]");
            append.Append("}");
            return append.ToString();
        }

        /// <summary>
        ///  The entity into a tree 
        /// </summary>
        /// <param name="ens"></param>
        /// <param name="rootNo"></param>
        StringBuilder appendMenus = new StringBuilder();
        StringBuilder appendMenuSb = new StringBuilder();
        public void TansEntitiesToGenerTree(Entities ens, string rootNo)
        {
            EntityTree root = ens.GetEntityByKey(EntityTreeAttr.ParentNo, rootNo) as EntityTree;
            if (root == null)
                throw new Exception("@ Not found rootNo=" + rootNo + "的entity.");
            appendMenus.Append("[{");
            appendMenus.Append("\"id\":\"" + rootNo + "\"");
            appendMenus.Append(",\"text\":\"" + root.Name + "\"");

            //  Increase its children .
            appendMenus.Append(",\"children\":");
            AddChildren(root, ens);
            appendMenus.Append(appendMenuSb);
            appendMenus.Append("}]");
        }

        public void AddChildren(EntityTree parentEn, Entities ens)
        {
            appendMenus.Append(appendMenuSb);
            appendMenuSb.Clear();

            appendMenuSb.Append("[");
            foreach (EntityTree item in ens)
            {
                if (item.ParentNo != parentEn.No)
                    continue;

                appendMenuSb.Append("{\"id\":\"" + item.No + "\",\"text\":\"" + item.Name + "\",\"state\":\"closed\"");
                EntityTree treeNode = item as EntityTree;
                //  Increase its children .
                appendMenuSb.Append(",\"children\":");
                AddChildren(item, ens);
                appendMenuSb.Append("},");
            }
            if (appendMenuSb.Length > 1)
                appendMenuSb = appendMenuSb.Remove(appendMenuSb.Length - 1, 1);
            appendMenuSb.Append("]");
            appendMenus.Append(appendMenuSb);
            appendMenuSb.Clear();
        }
    }
}