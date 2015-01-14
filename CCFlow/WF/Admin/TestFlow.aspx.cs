using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BP.WF;
using BP.En;
using BP.Port;
using BP.Web.Controls;
using BP.Web;
using BP.Sys;

namespace CCFlow.WF.Admin
{
    public partial class WF_Admin_TestFlow : BP.Web.WebPage
    {
        #region  Property .
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        public string Lang
        {
            get
            {
                return this.Request.QueryString["Lang"];
            }
        }
        public string GloSID
        {
            get
            {
                return this.Request.QueryString["GloSID"];
            }
        }
        #endregion  Property .

        public void BindFlowList()
        {
            this.Title = " Thank you for choosing to ride workflow engine - Process Design & Test interface ";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (this.GloSID != BP.WF.Glo.GloSID)
            //{
            //    this.Response.Write(" Global security code errors , Or you do not set , Please Web.config The appsetting Node in the set GloSID  Value .");
            //    return;
            //}

            BP.Sys.SystemConfig.DoClearCash();
            // 让admin  Log in .
            BP.WF.Dev2Interface.Port_Login("admin");
            //BP.WF.Dev2Interface.Port_Login("admin", this.SID);

            if (this.FK_Flow == null)
            {
                this.Ucsys1.AddFieldSet(" About Process Testing ");
                this.Ucsys1.AddUL();
                this.Ucsys1.AddLi(" Now is the flow test status , This feature is available to process tightly designers use .");
                this.Ucsys1.AddLi(" The purpose is to provide this functionality , Let each person quickly landed roles , In order to reduce the tedious trouble landing .");
                this.Ucsys1.AddLi(" After the point of the process list on the left , The system automatically displays the staff can initiate this process , Point a staff member directly landed .");
                this.Ucsys1.AddULEnd();
                this.Ucsys1.AddFieldSetEnd();
                return;
            }

            if (this.RefNo != null)
            {
                Emp emp = new Emp(this.RefNo);
                BP.Web.WebUser.SignInOfGenerLang(emp, this.Lang);
                this.Session["FK_Flow"] = this.FK_Flow;
                if (this.Request.QueryString["Type"] != null)
                {
                    string url = "../WAP/MyFlow.aspx?FK_Flow=" + this.FK_Flow;
                    if (this.Request.QueryString["IsWap"] == "1")
                        this.Response.Redirect("../WAP/MyFlow.aspx?FK_Flow=" + this.FK_Flow + "&FK_Node=" + int.Parse(this.FK_Flow) + "01", true);
                    else
                        this.Response.Redirect("../MyFlow.aspx?FK_Flow=" + this.FK_Flow + "&FK_Node=" + int.Parse(this.FK_Flow) + "01", true);
                }
                else
                {
                    this.Response.Redirect("../Port/Home.htm?FK_Flow=" + this.FK_Flow, true);
                }
                return;
            }

            BP.Web.WebUser.SysLang = this.Lang;
            Flow fl = new Flow(this.FK_Flow);
            fl.DoCheck();

            int nodeid = int.Parse(this.FK_Flow + "01");
            DataTable dt=null;
            string sql = "";
            BP.WF.Node nd = new BP.WF.Node(nodeid);
            try
            {
                switch (nd.HisDeliveryWay)
                {
                    case DeliveryWay.ByStation:
                        sql = "SELECT Port_Emp.No  FROM Port_Emp LEFT JOIN Port_Dept   Port_Dept_FK_Dept ON  Port_Emp.FK_Dept=Port_Dept_FK_Dept.No   join Port_Empstation on (fk_emp=Port_Emp.No)   join WF_NodeStation on (WF_NodeStation.fk_station=Port_Empstation.fk_station) WHERE (1=1) AND  FK_Node="+nd.NodeID;
                       // emps.RetrieveInSQL_Order("select fk_emp from Port_Empstation WHERE fk_station in (select fk_station from WF_NodeStation WHERE FK_Node=" + nodeid + " )", "FK_Dept");
                        break;
                    case DeliveryWay.ByDept:
                        sql = "select No,Name from Port_Emp where FK_Dept in (select FK_Dept from WF_NodeDept where FK_Node='" + nodeid + "') ";
                        //emps.RetrieveInSQL("");
                        break;
                    case DeliveryWay.ByBindEmp:
                        sql = "select No,Name from Port_Emp where No in (select FK_Emp from WF_NodeEmp where FK_Node='" + nodeid + "') ";
                        //emps.RetrieveInSQL("select fk_emp from wf_NodeEmp WHERE fk_node=" + int.Parse(this.FK_Flow + "01") + " ");
                        break;
                    case DeliveryWay.ByDeptAndStation:
                        sql = "SELECT No FROM Port_Emp WHERE No IN ";
                        sql += "(SELECT FK_Emp FROM Port_EmpDept WHERE FK_Dept IN ";
                        sql += "( SELECT FK_Dept FROM WF_NodeDept WHERE FK_Node=" + nodeid + ")";
                        sql += ")";
                        sql += "AND No IN ";
                        sql += "(";
                        sql += "SELECT FK_Emp FROM Port_EmpStation WHERE FK_Station IN ";
                        sql += "( SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + nodeid + ")";
                        sql += ") ORDER BY No ";
                        break;
                    case DeliveryWay.BySQL:
                        if (string.IsNullOrEmpty(nd.DeliveryParas))
                            throw new Exception("@ You set up by SQL Access the starting node , But you do not set sql.");
                       // emps.RetrieveInSQL(nd.DeliveryParas);
                        break;
                    default:
                        break;
                }

                 dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
                if (dt.Rows.Count == 0)
                    throw new Exception("@ You follow :" + nd.HisDeliveryWay + " The manner of access rules start node , But there were no starting node .");
            }
            catch (Exception ex)
            {
                this.Ucsys1.AddMsgOfWarning("Error",
                        " Cause of error  <h2> You do not have the right to set the starting node access rules , See the operating manual process design .</h2>" + ex.StackTrace + " - " + ex.Message);
                return;
            }

            this.Ucsys1.AddFieldSet(" May initiate (<font color=red>" + fl.Name + "</font>) Process staff , Select an officer to enter and test .");
            this.Ucsys1.AddTable("align=center");
            this.Ucsys1.AddCaptionLeft(" Process ID :" + fl.No + "  Name :" + fl.Name);
            this.Ucsys1.AddTR();
            this.Ucsys1.AddTDTitle("IDX");

            CheckBox cball = new CheckBox();
            cball.Attributes["onclick"] = "SelectAll(this);";
            cball.Text = " Select all ";
            this.Ucsys1.AddTDTitle(cball);
            this.Ucsys1.AddTDTitle(" Speed mode ");
            this.Ucsys1.AddTDTitle("LigerUI Mode ");
            this.Ucsys1.AddTDTitle(" Classic mode ");
          //  this.Ucsys1.AddTDTitle(" Phone mode ");
            this.Ucsys1.AddTDTitle("Dept");
            this.Ucsys1.AddTREnd();
            bool is1 = false;
            int idx = 0;

            foreach (DataRow dr in dt.Rows)
            {
                BP.Port.Emp emp = new Emp(dr[0].ToString());
                idx++;
                is1 = this.Ucsys1.AddTR(is1);
                this.Ucsys1.AddTDIdx(idx);

                CheckBox cb = new CheckBox();
                cb.ID = "CB_" + emp.No;
                cb.Text = emp.No + "," + emp.Name;
                this.Ucsys1.AddTD(cb);

                this.Ucsys1.AddTD("<a href='./../Port.aspx?DoWhat=JiSu&UserNo=" + emp.No + "&FK_Flow=" + this.FK_Flow + "&Lang=" + BP.Web.WebUser.SysLang + "&Type=" + this.Request.QueryString["Type"] + "'  ><img src='./../Img/IE.gif' border=0 /> Speed mode </a>");
                this.Ucsys1.AddTD("<a href='./../Port.aspx?DoWhat=StartLigerUI&UserNo=" + emp.No + "&FK_Flow=" + this.FK_Flow + "&Lang=" + BP.Web.WebUser.SysLang + "&Type=" + this.Request.QueryString["Type"] + "'  ><img src='./../Img/IE.gif' border=0 />LigerUI Mode </a>");
                this.Ucsys1.AddTD("<a href='./../Port.aspx?DoWhat=Start5&UserNo=" + emp.No + "&FK_Flow=" + this.FK_Flow + "&Lang=" + BP.Web.WebUser.SysLang + "&Type=" + this.Request.QueryString["Type"] + "'  ><img src='./../Img/IE.gif' border=0 /> Classic mode </a>");
               // this.Ucsys1.AddTD("<a href='./../Port.aspx?DoWhat=Start&UserNo=" + emp.No + "&FK_Flow=" + this.FK_Flow + "&Lang=" + BP.Web.WebUser.SysLang + "&Type=" + this.Request.QueryString["Type"] + "'  ><img src='./../Img/IE.gif' border=0 />Blog Mode </a>");                
                //this.Ucsys1.AddTD("<a href='./../Port.aspx?DoWhat=StartSmallSingle&UserNo=" + emp.No + "&FK_Flow=" + this.FK_Flow + "&Lang=" + BP.Web.WebUser.SysLang + "&Type=" + this.Request.QueryString["Type"] + "'  ><img src='./../Img/IE.gif' border=0 />Internet Explorer</a>");
                //this.Ucsys1.AddTD("<a href=\"javascript:WinOpen('TestFlow.aspx?RefNo=" + emp.No + "&FK_Flow=" + this.FK_Flow + "&Lang=" + BP.Web.WebUser.SysLang + "&Type=" + this.Request.QueryString["Type"] + "&IsWap=1','470px','600px','" + emp.No + "');\"  ><img src='./../Img/Mobile.gif' border=0 width=25px height=18px />Mobile</a> ");
                this.Ucsys1.AddTD(emp.FK_DeptText);
                this.Ucsys1.AddTD("<a href='TestSDK.aspx?RefNo=" + emp.No + "&FK_Flow=" + this.FK_Flow + "&Lang=" + BP.Web.WebUser.SysLang + "&Type=" + this.Request.QueryString["Type"] + "&IsWap=1'  >SDK</a> ");
                this.Ucsys1.AddTREnd();
            }

            Button btn = new Button();
            btn.Text = " The selection of personnel to perform automatic simulation run ";
            btn.Click += new EventHandler(btn_Click);

            this.Ucsys1.AddTR();
            this.Ucsys1.AddTD("colspan=7", btn);
            this.Ucsys1.AddTREnd();

            this.Ucsys1.AddTableEnd();
            this.Ucsys1.AddFieldSetEnd();
        }

        void btn_Click(object sender, EventArgs e)
        {
            string ids = "";
            foreach (Control ctl in this.Ucsys1.Controls)
            {
                if (ctl == null || ctl.ID == null || ctl.ID.Contains("CB_") == false)
                    continue;
                CheckBox cb = ctl as CheckBox;
                if (cb.Checked == false)
                    continue;

                ids += ctl.ID.Replace("CB_", "") + ",";
            }
            if (string.IsNullOrEmpty(ids) == true)
            {
                this.Response.Write(" Please select staff to simulate initiated ");
                return;
            }

            string url = "SimulationRun.aspx?FK_Flow=" + this.FK_Flow + "&IDs=" + ids;
            this.Response.Redirect(url, true);
        }
    }
}