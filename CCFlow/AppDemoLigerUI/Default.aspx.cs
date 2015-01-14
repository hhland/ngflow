using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using BP.WF;
using BP.Port;
using BP.DA;
using BP.Web;
using CCFlow.AppDemoLigerUI.Base;

namespace CCFlow.AppDemoLigerUI
{
    public partial class Default : BasePage
    {
        #region   Operating variables 
        /// <summary>
        ///  The current process ID 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                string s = this.Request.QueryString["FK_Flow"];
                if (string.IsNullOrEmpty(s))
                    throw new Exception("@ Process ID is empty ...");
                return s;
            }
        }
        public string FromNode
        {
            get
            {
                return this.Request.QueryString["FromNode"];
            }
        }
        /// <summary>
        ///  Current work ID
        /// </summary>
        public Int64 WorkID
        {
            get
            {
                if (ViewState["WorkID"] == null)
                {
                    if (this.Request.QueryString["WorkID"] == null)
                        return 0;
                    else
                        return Int64.Parse(this.Request.QueryString["WorkID"]);
                }
                else
                    return Int64.Parse(ViewState["WorkID"].ToString());
            }
            set
            {
                ViewState["WorkID"] = value;
            }
        }
        private int _FK_Node = 0;
        /// <summary>
        ///  Current  NodeID , At the beginning of time ,nodeID, Is to a , Start node processes ID.
        /// </summary>
        public int FK_Node
        {
            get
            {
                string fk_nodeReq = this.Request.QueryString["FK_Node"];
                if (string.IsNullOrEmpty(fk_nodeReq))
                    fk_nodeReq = this.Request.QueryString["NodeID"];

                if (string.IsNullOrEmpty(fk_nodeReq) == false)
                    return int.Parse(fk_nodeReq);

                if (_FK_Node == 0)
                {
                    if (this.Request.QueryString["WorkID"] != null)
                    {
                        string sql = "SELECT FK_Node from  WF_GenerWorkFlow where WorkID=" + this.WorkID;
                        _FK_Node = DBAccess.RunSQLReturnValInt(sql);
                    }
                    else
                    {
                        _FK_Node = int.Parse(this.FK_Flow + "01");
                    }
                }
                return _FK_Node;
            }
        }
        public int FID
        {
            get
            {
                try
                {
                    return int.Parse(this.Request.QueryString["FID"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
        public int PWorkID
        {
            get
            {
                try
                {
                    return int.Parse(this.Request.QueryString["PWorkID"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
        #endregion

        public string usermsg = "";
        public string mainSrc = "";
        // Form type node bound 
        public string nodeformtype = "";
        //public string treedata = "";
        //public string strcanstartflow = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (BP.Web.WebUser.No == null)
            {
                this.Response.Redirect("Login.aspx", true);
                return;
            }
            if (BP.WF.Glo.OSModel == OSModel.BPM)
            {
                GetBPMMenu();
            }
            else
            {
                GetMenuTree();
            }

            if (!IsPostBack)
            {
                usermsg = " Account number :" + BP.Web.WebUser.No + "   Full name : " + BP.Web.WebUser.Name + "  Department :" + BP.Web.WebUser.FK_DeptName;
                // Are there traditional values 
                if (this.Request.QueryString.Count > 2)
                {
                    string paras = "";
                    foreach (string str in this.Request.QueryString)
                    {
                        string val = this.Request.QueryString[str];
                        if (val.IndexOf('@') != -1)
                            throw new Exception(" You can not have arguments : [ " + str + " ," + val + " ]  To value  ,URL  Will not be executed .");

                        switch (str)
                        {
                            case DoWhatList.DoNode:
                            case DoWhatList.Emps:
                            case DoWhatList.EmpWorks:
                            case DoWhatList.FlowSearch:
                            case DoWhatList.Login:
                            case DoWhatList.MyFlow:
                            case DoWhatList.MyWork:
                            case DoWhatList.Start:
                            case DoWhatList.Start5:
                            case DoWhatList.StartSmall:
                            case DoWhatList.FlowFX:
                            case DoWhatList.DealWork:
                            case DoWhatList.DealWorkInSmall:
                            case "FK_Flow":
                            case "WorkID":
                            case "FK_Node":
                            case "SID":
                                break;
                            default:
                                paras += "&" + str + "=" + val;
                                break;
                        }
                    }
                    // Determine the starting node   Form type 
                    BP.WF.Node CurrentNode = new BP.WF.Node(this.FK_Node);
                    if (CurrentNode.HisFormType == NodeFormType.SheetTree)
                    {
                        nodeformtype = "SheetTree";
                    }
                    else
                    {
                        nodeformtype = "Other";
                    }
                    mainSrc = "../WF/MyFlow.aspx?FK_Flow=" + this.FK_Flow + paras + "&FK_Node=" + FK_Node;
                }
            }
        }
        /// <summary>
        ///  Load Ligertree
        /// </summary>
        public void GetMenuTree()
        {
            // The default menu 
            StringBuilder sbXML = GetCCForm();
            string sqlSort = "";
            string stitle = " Process list ";

            // Loading process privilege 
            if (BP.WF.Glo.OSModel == OSModel.BPM)
            {
                // Load Flow 
                sqlSort = "SELECT * FROM V_GPM_EmpMenu WHERE FK_Emp='" + BP.Web.WebUser.No + "' and FK_App = '" + BP.Sys.SystemConfig.SysNo + "'";
                DataTable dtSort = BP.DA.DBAccess.RunSQLReturnTable(sqlSort);
                if (dtSort != null && dtSort.Rows.Count > 0)
                {
                    // Load tree 
                    sbXML.AppendFormat("<div title='{0}' class='l-scroll'>", stitle);
                    sbXML.Append("<ul id=\"tree\"></ul>");
                    sbXML.Append("</div>");
                }
            }
            // Load Structure 
            if (BP.Web.WebUser.No == "admin")
            {
                sbXML.Append(GetGpmMenu());
            }
            this.accordion1.InnerHtml = sbXML.ToString();
        }

        /// <summary>
        ///  Menu 
        /// </summary>
        public void GetBPMMenu()
        {
            StringBuilder sbXML = new StringBuilder("");

            sbXML.Append(GetSecondMenu());
            this.accordion1.InnerHtml = sbXML.ToString();
        }

        /// <summary>
        ///  Under the authority , Gets the menu 
        /// </summary>
        /// <returns></returns>
        public string GetSecondMenu()
        {
            StringBuilder sbSecontent = new StringBuilder("");
            DataTable dtMenu = CCPortal.API.GetUserMenuOfDatatable(BP.Web.WebUser.No);

            DataRow[] drSecMenu = dtMenu.Select("Menutype =3");
            if (drSecMenu.Length > 0)
            {
                foreach (DataRow dr in drSecMenu)
                {
                    sbSecontent.AppendFormat("<div title='{0}' class='l-scroll'>", dr["Name"].ToString());
                    DataRow[] drThirMenu = dtMenu.Select("ParentNo = '" + dr["No"].ToString() + "'");
                    foreach (DataRow drThir in drThirMenu)
                    {
                        sbSecontent.AppendFormat("<a class='l-link' href='javascript:f_addTab(\"{0}\",\"{1}\",\"{2}\")'>", drThir["No"].ToString(), drThir["Name"].ToString(), drThir["Url"].ToString());
                        sbSecontent.AppendFormat("<img class='img-menu' align='middle' alt='' src='{0}' />{1}</a>", drThir["WebPath"].ToString(), drThir["Name"].ToString());
                    }
                    sbSecontent.Append("</div>");
                }
            }
            else
            {
                sbSecontent = GetCCForm();
            }
            return sbSecontent.ToString();
        }

        /// <summary>
        ///  Menu 
        /// </summary>
        public StringBuilder GetCCForm()
        {
            StringBuilder sbXML = new StringBuilder("");
            sbXML.Append("<div title=' Feature List ' class='l-scroll'>");
            if (Glo.IsAdmin)
            {
                sbXML.Append("<a class='l-link' href='javascript:f_addTab(\"FlowManager\",\" Process Scheduling \",\"FlowManager.aspx\")'>");
                sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Start1.png' /><span id='Span1'> Process Scheduling </span></a>");
            }

            sbXML.Append("<a id='allowStartCount' class='l-link' href='javascript:f_addTab(\"startpage\",\" Launch \",\"Start.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Start.png' /> Launch </a>");

            sbXML.Append("<a id='allowStartCount' class='l-link' href='javascript:f_addTab(\"startpageTree\",\" Launch(Tree)\",\"StartTree.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Start.png' /> Launch(Tree)</a>");

            sbXML.Append("<a class='l-link' href='javascript:f_addTab(\"empworks\",\" Upcoming \",\"EmpWorks.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/EmpWorks.png' /><span id='empworkCount'> Upcoming (0)</span></a>");

            sbXML.Append("<a class='l-link' href='javascript:f_addTab(\"Batch\",\" Batch \",\"../WF/Batch.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Start1.png' /><span id='Span1'> Batch </span></a>");

            if (BP.WF.Glo.IsEnableTaskPool == true)
            {
                sbXML.Append("<a class='l-link' href='javascript:f_addTab(\"TaskPoolSmall\",\" Shared mission \",\"../WF/TaskPoolSharing.aspx\")'>");
                sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Start1.png' /><span id='TaskPoolNum'> Shared mission </span></a>");
            }

            sbXML.Append("<a class='l-link' href='javascript:f_addTab(\"CCSmall\",\" Cc \",\"CC.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/CC.png' /><span id='ccsmallCount'> Cc (0)</span></a>");
            sbXML.Append("<a class='l-link' href='javascript:f_addTab(\"HungUp\",\" Pending \",\"HungUp.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/hungup.png' /><span id='hungUpCount'> Pending (0)</span></a>");
            sbXML.Append(" <a class='l-link' href='javascript:f_addTab(\"Running\",\" In transit \",\"Running.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Runing1.png' /> In transit </a>");
            sbXML.Append("<a class='l-link' href='javascript:f_addTab(\"FlowSearch\",\" Inquiry \",\"FlowSearch.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Searchss.png' /> Inquiry </a>");
            sbXML.Append("<a class='l-link' href='javascript:f_addTab(\"keySearch\",\" Keyword Search \",\"keySearch.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Searchkey.png' /> Keyword Search </a>");
            sbXML.Append("<a class='l-link' href='javascript:f_addTab(\"GetTask\",\" Retrieve approval \",\"GetTask.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/tackback.png' /> Retrieve approval </a>");
            sbXML.Append("<a class='l-link' href='javascript:f_addTab(\"Emps\",\" Contacts \",\"Emps.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/AddressCard.png' /> Contacts </a>");
            sbXML.Append("<a class='l-link' href='javascript:f_addTab(\"SmsList\",\" System Messages \",\"SmsList.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/sms.png' /> System Messages </a>");
            sbXML.Append("<a class='l-link' href='javascript:f_addTab(\"ToolsSmall\",\" Set up \",\"../WF/Tools.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Set.png' /> Set up </a>");
            sbXML.Append("</div>");

            return sbXML;
        }

        /// <summary>
        ///  Organizational Structure   Menu 
        /// </summary>
        public string GetGpmMenu()
        {
            StringBuilder sbXML = new StringBuilder("");
            sbXML.Append("<div title=' Organizational Structure ' class='l-scroll'>");
            sbXML.Append("<a id='a1' class='l-link' href='javascript:f_addTab(\"a1\",\" Sector Type \",\"../WF/Comm/Search.aspx?EnsName=BP.GPM.DeptTypes\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Start.png' /> Sector Type </a>");
            sbXML.Append("<a id='a2' class='l-link' href='javascript:f_addTab(\"a2\",\" Post Type \",\"../WF/Comm/Search.aspx?EnsName=BP.GPM.StationTypes\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Start.png' /> Post Type </a>");
            sbXML.Append("<a id='a3' class='l-link' href='javascript:f_addTab(\"a3\",\" Post \",\"../WF/Comm/Search.aspx?EnsName=BP.GPM.Stations\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Start.png' /> Post </a>");
            sbXML.Append("<a id='a4' class='l-link' href='javascript:f_addTab(\"a4\",\" Maintenance duties \",\"../WF/Comm/Search.aspx?EnsName=BP.GPM.Dutys\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Start.png' /> Maintenance duties </a>");
            sbXML.Append("<a id='a5' class='l-link' href='javascript:f_addTab(\"a5\",\" Organizational Structure \",\"../WF/Admin/OrganizationalStructure.aspx?EnsName=BP.GPM.Depts\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Start.png' /> Organizational Structure </a>");
            sbXML.Append("</div>");
            return sbXML.ToString();
        }

    }
}