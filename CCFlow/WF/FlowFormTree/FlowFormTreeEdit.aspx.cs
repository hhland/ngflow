using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CCFlow.WF.Comm.UC;
using BP.WF.Template;
using BP.WF;
using BP.Sys;
using BP.Port;
using BP.Web.Controls;
using BP.DA;
using BP.En;
using BP.Web;

namespace CCFlow.WF.SheetTree
{
    /// <summary>
    ///  With respect to :/WF/FlowFormTree/FlowFormTreeEdit.aspx  Function interface for use :
    ///  This page is a function call to the page , It requires two parameters . FK_Flow,WorkID.
    ///  Is used to solve the flow form the tree view and edit , He corresponds to the process rather than the node ID number .
    ///  The main application of the following scenarios :
    /// 1,  Need to see the flow of information , Rather than specifying a particular node .
    /// 2, Need to see the flow of information , You can edit the information and forms , Used in data collection , The process has been completed immediately .
    /// 3,  On the feature toolbar , Only a ,  Save , Locus , Closed three buttons .
    /// 
    ///  Related :
    /// 1,  Information bind normal process node tree form , It Node Properties , Node Form Settings .
    /// 2,  Forms node in the node set binding properties .  Form bound to set in the process flow properties in .
    /// 3,  Node binding function is to process the form page is  FlowFormTreeView.aspx  Parameter needed is  FK_Flow,WorkID,FK_Node,FID
    /// </summary>
    public partial class FlowFormTreeEdit : System.Web.UI.Page
    {
        public string FK_Node
        {
            get
            {
                return this.Request.QueryString["FK_Node"];
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                InitToolsBar();
        }
        /// <summary>
        ///  Initialization Toolbar 
        /// </summary>
        private void InitToolsBar()
        {
            string toolsDefault = "";
            // Save 
            toolsDefault += "<a id=\"save\" href=\"#\" class=\"easyui-linkbutton\" data-options=\"plain:true,iconCls:'icon-save'\" onclick=\"EventFactory('save')\"> Save </a>";
            
            // Locus 
            toolsDefault += "<a id=\"Track\" href=\"#\" class=\"easyui-linkbutton\" data-options=\"plain:true,iconCls:'icon-flowmap'\" onclick=\"EventFactory('showchart')\"> Locus </a>";
            //// Inquiry 
            //toolsDefault += "<a id=\"Search\" href=\"#\" class=\"easyui-linkbutton\" data-options=\"plain:true,iconCls:'icon-search'\" onclick=\"EventFactory('search')\"> Inquiry </a>";
            // Expansion Tool , Display position for the toolbar type 
            NodeToolbars extToolBars = new NodeToolbars();
            QueryObject info = new QueryObject(extToolBars);
            info.AddWhere(NodeToolbarAttr.FK_Node, this.FK_Node);
            info.addAnd();
            info.AddWhere(NodeToolbarAttr.ShowWhere, (int)ShowWhere.Toolbar);
            info.DoQuery();
            foreach (NodeToolbar item in extToolBars)
            {
                string url = "";
                if (string.IsNullOrEmpty(item.Url))
                    continue;

                string urlExt = this.RequestParas;
                url = item.Url;
                if (url.Contains("?"))
                {
                    url += urlExt;
                }
                else
                {
                    url += "?" + urlExt;
                }
                toolsDefault += "<a target=\"" + item.Target + "\" href=\"" + url + "\" class=\"easyui-linkbutton\" data-options=\"plain:true,iconCls:'icon-new'\">" + item.Title + "</a>";
            }
            // Shut down 
            toolsDefault += "<a id=\"closeWin\" href=\"#\" class=\"easyui-linkbutton\" data-options=\"plain:true,iconCls:'icon-no'\" onclick=\"EventFactory('closeWin')\"> Shut down </a>";
            // Add Content 
            this.toolBars.InnerHtml = toolsDefault;
        }

        public string RequestParas
        {
            get
            {
                string urlExt = "";
                string rawUrl = this.Request.RawUrl;
                rawUrl = "&" + rawUrl.Substring(rawUrl.IndexOf('?') + 1);
                string[] paras = rawUrl.Split('&');
                foreach (string para in paras)
                {
                    if (para == null
                        || para == ""
                        || para.Contains("=") == false)
                        continue;
                    urlExt += "&" + para;
                }
                return urlExt;
            }
        }
    }
}