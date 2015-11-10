using System;
using System.Collections.Generic;
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
    public partial class Tree : System.Web.UI.Page
    {
        public string EnsDesc
        {
            get
            {
                if (this.EnsName == null || this.HisEn == null)
                    return " Tree ";
                return this.HisEn.EnDesc;
            }
        }
        /// <summary>
        ///  Entity Name 
        /// </summary>
        public string EnName
        {
            get
            {
                if (this._HisEn != null)
                    return this.HisEn.ToString();
                return "";
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
        ///  Entity set 
        /// </summary>
        public Entities _HisEns = null;
        public Entities HisEns
        {
            get
            {
                if (this.EnsName != null)
                {
                    if (this._HisEns == null)
                    {
                        _HisEns = BP.En.ClassFactory.GetEns(this.EnsName.Replace("#", ""));
                    }
                }
                return _HisEns;
            }
        }
        /// <summary>
        ///  Single entity 
        /// </summary>
        private Entity _HisEn = null;
        public Entity HisEn
        {
            get
            {
                if (_HisEn == null)
                {
                    if (this.HisEns == null)
                    {
                        _HisEn = BP.En.ClassFactory.GetEn(this.EnsName.Replace("#", ""));
                        if (this._HisEn == null)
                            throw new Exception(" Did not find the namespace and class met in this project :" + this.EnsName);
                    }
                    else
                        _HisEn = this.HisEns.GetNewEntity;

                    EntityTree enTree = _HisEn as EntityTree;
                    if (enTree == null)
                    {
                        if (_HisEn.IsNoEntity)
                            throw new Exception(" Incoming entity must inherit EntityTree; The incoming " + this.EnsName + " Inherited :NoEntity.");
                        else if (_HisEn.IsOIDEntity)
                            throw new Exception(" Incoming entity must inherit EntityTree; The incoming " + this.EnsName + " Inherited :OIDEntity.");
                        else if (_HisEn.IsMIDEntity)
                            throw new Exception(" Incoming entity must inherit EntityTree; The incoming " + this.EnsName + " Inherited :MIDEntity.");
                        else
                            throw new Exception(" Incoming entity must inherit EntityTree.");
                    }
                }
                return _HisEn;
            }
        }
        /// <summary>
        ///  Get incoming parameters 
        /// </summary>
        /// <param name="param"> Parameter name </param>
        /// <returns></returns>
        public string getUTF8ToString(string param)
        {
            return HttpUtility.UrlDecode(Request[param], System.Text.Encoding.UTF8);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (BP.Web.WebUser.No == null)
            //    return;

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
                case "treesortmanage":// Knowledge Tree Operation 
                    s_responsetext = TreeNodeManage();
                    break;
                case "updatetreenodename":// Modify knowledge category names 
                    s_responsetext = UpdateTreeNodeName();
                    break;
                case "gettreenodename":// Get node text 
                    s_responsetext = GetTreeNodeName();
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

                Entities ens = this.HisEn.GetNewEntities;
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
                throw new Exception("@ Not found rootNo=" + rootNo + " entity.");
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

        /// <summary>
        ///  Tree node operations 
        /// </summary>
        /// <returns></returns>
        private string TreeNodeManage()
        {
            string nodeNo = getUTF8ToString("nodeNo");
            string dowhat = getUTF8ToString("dowhat");
            string returnVal = "";

            EntityTree treeNode = this.HisEn as EntityTree;
            treeNode.RetrieveByAttr(EntityTreeAttr.No, nodeNo);

            switch (dowhat.ToLower())
            {
                case "sample":// Added siblings    
                    EntityTree enTree = treeNode.DoCreateSameLevelNode();
                    returnVal = "{No:'" + enTree.No + "',Name:'" + enTree.Name + "'}";
                    break;
                case "children":// New child node 
                    enTree = treeNode.DoCreateSubNode();
                    returnVal = "{No:'" + enTree.No + "',Name:'" + enTree.Name + "'}";
                    break;
                case "doup":// Move 
                    treeNode.DoUp();
                    break;
                case "dodown":// Down 
                    treeNode.DoDown();
                    break;
                case "delete":// Delete 
                    treeNode.Delete();
                    break;
            }
            // Return 
            return returnVal;
        }

        /// <summary>
        ///  Modify Node Name 
        /// </summary>
        /// <returns></returns>
        private string UpdateTreeNodeName()
        {
            string nodeNo = getUTF8ToString("nodeNo");
            string nodeName = getUTF8ToString("nodeName");

            EntityTree treeNode = this.HisEn as EntityTree;
            treeNode.RetrieveByAttr(EntityTreeAttr.No, nodeNo);
            treeNode.Name = nodeName;
            int i = treeNode.Update();
            if (i > 0)
            {
                return "true";
            }

            return "false";
        }

        /// <summary>
        ///  Get node text according to ID 
        /// </summary>
        /// <returns></returns>
        private string GetTreeNodeName()
        {
            string nodeNo = getUTF8ToString("nodeNo");
            EntityTree treeNode = this.HisEn as EntityTree;
            treeNode.RetrieveByAttr(EntityTreeAttr.No, nodeNo);

            return treeNode.Name;
        }
    }
}