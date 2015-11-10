using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BP.WF;
using BP.DA;
using BP.En;
using BP.Port;
using BP.Sys;

namespace BP.Web.Port
{
    /// <summary>
    /// Port  The summary .
    /// </summary>
    public partial class Port : System.Web.UI.Page
    {
        #region  Must pass parameters 
        public string DoWhat
        {
            get
            {
                return this.Request.QueryString["DoWhat"];
            }
        }
        public string UserNo
        {
            get
            {
                return this.Request.QueryString["UserNo"];
            }
        }
        public string SID
        {
            get
            {
                return this.Request.QueryString["SID"];
            }
        }
        #endregion

        #region   Optional parameters 
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        public string FK_Node
        {
            get
            {
                return this.Request.QueryString["FK_Node"];
            }
        }
        public string WorkID
        {
            get
            {
                return this.Request.QueryString["WorkID"];
            }
        }
        #endregion

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Response.AddHeader("P3P", "CP=CAO PSA OUR");
            if (this.UserNo != null && this.SID != null)
            {
                try
                {
                    string sql = "SELECT SID FROM Port_Emp WHERE No='" + this.UserNo + "'";
                    string sid = BP.DA.DBAccess.RunSQLReturnString(sql);
                    if (sid != this.SID)
                    {
                        this.Response.Write(" Illegal access , Please contact your administrator .sid=" + sid);
                        return;
                    }
                    else
                    {
                        Emp emL = new Emp(this.UserNo);
                        WebUser.Token = this.Session.SessionID;
                        WebUser.SignInOfGenerLang(emL, SystemConfig.SysLanguage);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("@ Maybe you have not configured ccflow The security authentication mechanism :" + ex.Message);
                }
            }

            Emp em = new Emp(this.UserNo);
            WebUser.Token = this.Session.SessionID;
            WebUser.SignInOfGenerLang(em, SystemConfig.SysLanguage);

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
                    case DoWhatList.JiSu:
                    case DoWhatList.StartSmall:
                    case DoWhatList.FlowFX:
                    case DoWhatList.DealWork:
                    case DoWhatList.DealWorkInSmall:
                    //   case DoWhatList.CallMyFlow:
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

            if (this.IsPostBack == false)
            {
                if (this.IsCanLogin() == false)
                {
                    this.ShowMsg("<fieldset><legend> Security validation error </legend>  The system can not perform your request , May be your landing time is too long , Please re-login .<br> If you want to cancel, modify security verification web.config  IsDebug  Set of values 1.</fieldset>");
                    return;
                }

                BP.Port.Emp emp = new BP.Port.Emp(this.UserNo);
                BP.Web.WebUser.SignInOfGener(emp); // Started landing .

                string nodeID = int.Parse(this.FK_Flow + "01").ToString();
                switch (this.DoWhat)
                {
                    case DoWhatList.OneWork: //  Work processor calls .
                        if (this.FK_Flow == null || this.WorkID == null)
                            throw new Exception("@ Parameters  FK_Flow  Or  WorkID is null .");
                        this.Response.Redirect("/WF/WFRpt.aspx?FK_Flow=" + this.FK_Flow + "&WorkID=" + this.WorkID + "&o2=1" + paras, true);
                        break;
                    case DoWhatList.JiSu: //  Way speed mode of initiating work 
                        if (this.FK_Flow == null)
                            this.Response.Redirect("./App/Simple/Default.aspx", true);
                        else
                            this.Response.Redirect("./App/Simple/Default.aspx?FK_Flow=" + this.FK_Flow + paras + "&FK_Node=" + nodeID, true);
                        break;
                    case DoWhatList.Start5: //  Initiate work 
                        if (this.FK_Flow == null)
                            this.Response.Redirect("./App/Classic/Default.aspx", true);
                        else
                            this.Response.Redirect("./App/Classic/Default.aspx?FK_Flow=" + this.FK_Flow + paras + "&FK_Node=" + nodeID, true);
                        break;
                    case DoWhatList.StartLigerUI:
                        if (this.FK_Flow == null)
                            this.Response.Redirect("../AppDemoLigerUI/Default.aspx", true);
                        else
                            this.Response.Redirect("../AppDemoLigerUI/Default.aspx?FK_Flow=" + this.FK_Flow + paras + "&FK_Node=" + nodeID, true);
                        break;
                    case DoWhatList.Start: //  Initiate work 
                        if (this.FK_Flow == null)
                            this.Response.Redirect("Start.aspx", true);
                        else
                            this.Response.Redirect("MyFlow.aspx?FK_Flow=" + this.FK_Flow + paras + "&FK_Node=" + nodeID, true);
                        break;
                    case DoWhatList.StartSmall: //  Initiate work ¡¡ Small window 
                        if (this.FK_Flow == null)
                            this.Response.Redirect("Start.aspx?FK_Flow=" + this.FK_Flow + paras, true);
                        else
                            this.Response.Redirect("MyFlow.aspx?FK_Flow=" + this.FK_Flow + paras, true);
                        break;
                    case DoWhatList.StartSmallSingle: //  Launched a small window to work alone 
                        if (this.FK_Flow == null)
                            this.Response.Redirect("Start.aspx?FK_Flow=" + this.FK_Flow + paras + "&IsSingle=1&FK_Node=" + nodeID, true);
                        else
                            this.Response.Redirect("MyFlowSmallSingle.aspx?FK_Flow=" + this.FK_Flow + paras + "&FK_Node=" + nodeID, true);
                        break;
                    case DoWhatList.Runing: //  On the way to work 
                        this.Response.Redirect("Runing.aspx?FK_Flow=" + this.FK_Flow, true);
                        break;
                    case DoWhatList.Tools: //  Tools section .
                        this.Response.Redirect("Tools.aspx", true);
                        break;
                    case DoWhatList.ToolsSmall: //  Gadgets Column .
                        this.Response.Redirect("Tools.aspx?RefNo=" + this.Request["RefNo"], true);
                        break;
                    case DoWhatList.EmpWorks: //  I work a small window 
                        if (this.FK_Flow == null || this.FK_Flow == "")
                            this.Response.Redirect("EmpWorks.aspx", true);
                        else
                            this.Response.Redirect("EmpWorks.aspx?FK_Flow=" + this.FK_Flow, true);
                        break;
                    case DoWhatList.Login:
                        if (this.FK_Flow == null)
                            this.Response.Redirect("EmpWorks.aspx", true);
                        else
                            this.Response.Redirect("EmpWorks.aspx?FK_Flow=" + this.FK_Flow, true);
                        break;
                    case DoWhatList.Emps: //  Contacts .
                        this.Response.Redirect("Emps.aspx", true);
                        break;
                    case DoWhatList.FlowSearch: //  Process Query .
                        if (this.FK_Flow == null)
                            this.Response.Redirect("FlowSearch.aspx", true);
                        else
                            this.Response.Redirect("/Rpt/Search.aspx?Endse=s&FK_Flow=001&EnsName=ND" + int.Parse(this.FK_Flow) + "Rpt" + paras, true);
                        break;
                    case DoWhatList.FlowSearchSmall: //  Process Query .
                        if (this.FK_Flow == null)
                            this.Response.Redirect("FlowSearch.aspx", true);
                        else
                            this.Response.Redirect("./Comm/Search.aspx?EnsName=ND" + int.Parse(this.FK_Flow) + "Rpt" + paras, true);
                        break;
                    case DoWhatList.FlowSearchSmallSingle: //  Process Query .
                        if (this.FK_Flow == null)
                            this.Response.Redirect("FlowSearchSmallSingle.aspx", true);
                        else
                            this.Response.Redirect("./Comm/Search.aspx?EnsName=ND" + int.Parse(this.FK_Flow) + "Rpt" + paras, true);
                        break;
                    case DoWhatList.FlowFX: //  Process Query .
                        if (this.FK_Flow == null)
                            throw new Exception("@ No parameters Process ID .");

                        this.Response.Redirect("./Comm/Group.aspx?EnsName=ND" + int.Parse(this.FK_Flow) + "Rpt" + paras, true);
                        break;
                    case DoWhatList.DealWork:
                        if (this.FK_Flow == null || this.WorkID == null)
                            throw new Exception("@ Parameters  FK_Flow  Or  WorkID is Null .");
                        this.Response.Redirect("MyFlow.aspx?FK_Flow=" + this.FK_Flow + "&WorkID=" + this.WorkID + "&o2=1" + paras, true);
                        break;
                    case DoWhatList.DealWorkInSmall:
                        if (this.FK_Flow == null || this.WorkID == null)
                            throw new Exception("@ Parameters  FK_Flow  Or  WorkID is Null .");

                        this.Response.Redirect("MyFlow.aspx?FK_Flow=" + this.FK_Flow + "&WorkID=" + this.WorkID + "&o2=1" + paras, true);
                        break;
                    default:
                        this.ToErrorPage(" No agreement marks :DoWhat=" + this.DoWhat);
                        break;
                }
            }
        }
        public void ShowMsg(string msg)
        {
            this.Response.Write(msg);
        }
        /// <summary>
        ///  Login verify the legality 
        /// </summary>
        /// <returns></returns>
        public bool IsCanLogin()
        {
            if (BP.Sys.SystemConfig.AppSettings["IsAuth"] == "1")
            {
                if (this.SID != this.GetKey())
                {
                    if (SystemConfig.IsDebug)
                        return true;
                    else
                        return false;
                }
            }
            return true;
        }
        public string GetKey()
        {
            return BP.DA.DBAccess.RunSQLReturnString("SELECT SID From Port_Emp WHERE no='" + this.UserNo + "'");
        }

        #region Web  Form Designer generated code 
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN:  This call is  ASP.NET Web  Form Designer required .
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///  Required method for Designer support  -  Do not use the code editor to modify 
        ///  Contents of this method .
        /// </summary>
        private void InitializeComponent()
        {
        }
        #endregion

        public void ToMsgPage(string mess)
        {
            System.Web.HttpContext.Current.Session["info"] = mess;
            System.Web.HttpContext.Current.Response.Redirect(System.Web.HttpContext.Current.Request.ApplicationPath + "Port/InfoPage.aspx", true);
            return;
        }
        /// <summary>
        ///  Information can also switch to the surface .
        /// </summary>
        /// <param name="mess"></param>
        public void ToErrorPage(string mess)
        {
            System.Web.HttpContext.Current.Session["info"] = mess;
            System.Web.HttpContext.Current.Response.Redirect("./Comm/Port/InfoPage.aspx");
            return;
        }
    }
}
