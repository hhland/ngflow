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
            string culture = Request.Params["culture"];
            if (!string.IsNullOrWhiteSpace(culture))
            {
                //HttpContext.Current.Profile.SetPropertyValue("culture", culture);
                Session["culture"] = culture;
                Response.Redirect(Request.Url.LocalPath);
            }

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
                usermsg = string.Format(GetGlobalResourceMsg("Usermsg.Pattern"), BP.Web.WebUser.No, BP.Web.WebUser.Name, BP.Web.WebUser.FK_DeptName); 
                    //" Account number :" + BP.Web.WebUser.No + "   Full name : " + BP.Web.WebUser.Name + "  Department :" + BP.Web.WebUser.FK_DeptName;
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
            sbXML.Append("<div title=' " + GetGlobalResourceTitle("FeatureList.Text")+" ' class='l-scroll'>");
            if (Glo.IsAdmin)
            {
                sbXML.Append("<a class='l-link' href='javascript:f_addTab(\"FlowManager\",\" " + GetGlobalResourceMenu("ProcessScheduling.Text") + " \",\"FlowManager.aspx\")'>");
                sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Start1.png' /><span id='Span1'> "+GetGlobalResourceMenu("ProcessScheduling.Text")+" </span></a>");
            }

            sbXML.Append("<a id='allowStartCount' class='l-link' href='javascript:f_addTab(\"startpage\",\" " + GetGlobalResourceMenu("Launch.Text") + " \",\"Start.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Start.png' /> "+GetGlobalResourceMenu("Launch.Text")+" </a>");

            sbXML.Append("<a id='allowStartCount' class='l-link' href='javascript:f_addTab(\"startpageTree\",\" " + GetGlobalResourceMenu("LaunchTree.Text") + "\",\"StartTree.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Start.png' /> "+GetGlobalResourceMenu("LaunchTree.Text")+"</a>");

            sbXML.Append("<a class='l-link' href='javascript:f_addTab(\"empworks\",\" " + GetGlobalResourceMenu("Upcoming.Text") + " \",\"EmpWorks.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/EmpWorks.png' /> "+GetGlobalResourceMenu("Upcoming.Text")+ " <span id='empworkCount'>(0)</span></a>");

            sbXML.Append("<a class='l-link' href='javascript:f_addTab(\"Batch\",\" " + GetGlobalResourceMenu("Batch.Text") + " \",\"../WF/Batch.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Start1.png' /><span id='Span1'> "+GetGlobalResourceMenu("Batch.Text")+" </span></a>");

            if (BP.WF.Glo.IsEnableTaskPool == true)
            {
                sbXML.Append("<a class='l-link' href='javascript:f_addTab(\"TaskPoolSmall\",\" " + GetGlobalResourceMenu("SharedMission.Text") + " \",\"../WF/TaskPoolSharing.aspx\")'>");
                sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Start1.png' /> "+GetGlobalResourceMenu("SharedMission.Text")+ " <span id='TaskPoolNum'></span></a>");
            }

            sbXML.Append("<a class='l-link' href='javascript:f_addTab(\"CCSmall\",\" " + GetGlobalResourceMenu("Cc.Text") + " \",\"CC.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/CC.png' /> "+GetGlobalResourceMenu("Cc.Text")+ " <span id='ccsmallCount'>(0)</span></a>");
            sbXML.Append("<a class='l-link' href='javascript:f_addTab(\"HungUp\",\" " + GetGlobalResourceMenu("Pending.Text") + " \",\"HungUp.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/hungup.png' /> "+GetGlobalResourceMenu("Pending.Text")+ " <span id='hungUpCount'>(0)</span></a>");
            sbXML.Append(" <a class='l-link' href='javascript:f_addTab(\"Running\",\" " + GetGlobalResourceMenu("InTransit.Text") + " \",\"Running.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Runing1.png' /> "+GetGlobalResourceMenu("InTransit.Text")+" </a>");
            sbXML.Append("<a class='l-link' href='javascript:f_addTab(\"FlowSearch\",\" " + GetGlobalResourceMenu("Inquiry.Text") + " \",\"FlowSearch.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Searchss.png' /> "+GetGlobalResourceMenu("Inquiry.Text")+" </a>");
            sbXML.Append("<a class='l-link' href='javascript:f_addTab(\"keySearch\",\" " + GetGlobalResourceMenu("KeywordSearch.Text") + " \",\"keySearch.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Searchkey.png' /> "+GetGlobalResourceMenu("KeywordSearch.Text")+" </a>");
            sbXML.Append("<a class='l-link' href='javascript:f_addTab(\"GetTask\",\" " + GetGlobalResourceMenu("RetrieveApproval.Text") + " \",\"GetTask.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/tackback.png' /> "+GetGlobalResourceMenu("RetrieveApproval.Text")+" </a>");
            sbXML.Append("<a class='l-link' href='javascript:f_addTab(\"Emps\",\" " + GetGlobalResourceMenu("Contacts.Text") + " \",\"Emps.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/AddressCard.png' /> "+GetGlobalResourceMenu("Contacts.Text")+" </a>");
            sbXML.Append("<a class='l-link' href='javascript:f_addTab(\"SmsList\",\" " + GetGlobalResourceMenu("SystemMessages.Text") + " \",\"SmsList.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/sms.png' /> "+GetGlobalResourceMenu("SystemMessages.Text")+" </a>");
            sbXML.Append("<a class='l-link' href='javascript:f_addTab(\"ToolsSmall\",\" " + GetGlobalResourceMenu("SetUp.Text") + " \",\"../WF/Tools.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Set.png' /> "+GetGlobalResourceMenu("SetUp.Text")+" </a>");
            sbXML.Append("</div>");

            return sbXML;
        }

        /// <summary>
        ///  Organizational Structure   Menu 
        /// </summary>
        public string GetGpmMenu()
        {
            
            StringBuilder sbXML = new StringBuilder("");
            sbXML.Append("<div title=' " + GetGlobalResourceTitle("OrganizationalStructure.Text")+ " ' class='l-scroll'>");
            sbXML.Append("<a id='a1' class='l-link' href='javascript:f_addTab(\"a1\",\" " + GetGlobalResourceMenu("SectorType.Text") + " \",\"../WF/Comm/Search.aspx?EnsName=BP.GPM.DeptTypes\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Start.png' />"+GetGlobalResourceMenu("SectorType.Text")+"</a>");
            sbXML.Append("<a id='a2' class='l-link' href='javascript:f_addTab(\"a2\",\" " + GetGlobalResourceMenu("PostType.Text") + " \",\"../WF/Comm/Search.aspx?EnsName=BP.GPM.StationTypes\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Start.png' /> "+GetGlobalResourceMenu("PostType.Text")+" </a>");
            sbXML.Append("<a id='a3' class='l-link' href='javascript:f_addTab(\"a3\",\" " + GetGlobalResourceMenu("Post.Text") + " \",\"../WF/Comm/Search.aspx?EnsName=BP.GPM.Stations\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Start.png' /> "+GetGlobalResourceMenu("Post.Text")+" </a>");
            sbXML.Append("<a id='a4' class='l-link' href='javascript:f_addTab(\"a4\",\" " + GetGlobalResourceMenu("MaintenanceDuties.Text") + " \",\"../WF/Comm/Search.aspx?EnsName=BP.GPM.Dutys\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Start.png' /> "+GetGlobalResourceMenu("MaintenanceDuties.Text")+" </a>");
            sbXML.Append("<a id='a5' class='l-link' href='javascript:f_addTab(\"a5\",\" " + GetGlobalResourceMenu("OrganizationalStructure.Text") + " \",\"../WF/Admin/OrganizationalStructure.aspx?EnsName=BP.GPM.Depts\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Start.png' /> "+GetGlobalResourceMenu("OrganizationalStructure.Text")+" </a>");
            sbXML.Append("<a id='a6' class='l-link' href='javascript:f_addTab(\"a6\",\" " + GetGlobalResourceMenu("Globalization.Text") + " \",\"../WF/Globalization.aspx\")'>");
            sbXML.Append("<img class='img-menu' align='middle' alt='' src='Img/Menu/Start.png' /> " + GetGlobalResourceMenu("Globalization.Text") + " </a>");
            sbXML.Append("</div>");
            return sbXML.ToString();
        }

    }
}