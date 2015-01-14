using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Sys;
using BP.DA;

namespace CCFlow.WF.Admin
{
    public partial class WF_Admin_DBInstall : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region  Check whether the database link success .
            try
            {
                switch (BP.Sys.SystemConfig.AppCenterDBType)
                {
                    case DBType.MSSQL:
                        BP.DA.DBAccess.RunSQLReturnString("SELECT 1+2 ");
                        break;
                    case DBType.Oracle:
                        BP.DA.DBAccess.RunSQLReturnString("SELECT 1+2 FROM DUAL ");
                        break;
                    case DBType.Informix:
                        BP.DA.DBAccess.RunSQLReturnString("SELECT 1+2 FROM DUAL ");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                this.Response.Write("<h1> Database connection error </h1><hr> <font color=red> Please refer to the installation manual inspection web.config Database connection settings , Error Messages :</font><br>" + ex.Message);
                return;
            }
            #endregion

            this.Pub1.AddH3("ccflow  Database repair and installation tools ");
            this.Pub1.AddHR();
            if (this.Request.QueryString["DoType"] == "OK")
            {
                this.Pub1.AddFieldSet(" Prompt ");
                this.Pub1.Add("ccflow Database initialization success .");

                this.Pub1.AddBR("<a href='./XAP/Designer.aspx?IsCheckUpdate=1' > Into the Process Designer .</a>");
                this.Response.Redirect("./XAP/Designer.aspx?IsCheckUpdate=1", true);

                this.Pub1.AddFieldSetEnd();
                return;
            }

            if (BP.DA.DBAccess.IsExitsObject("WF_Flow") == true)
            {
                this.Pub1.AddFieldSet(" Prompt ");
                this.Pub1.Add(" Data has been installed , If you want to reinstall , You need to manually remove the database objects .");
                this.Pub1.AddFieldSetEnd();

                this.Pub1.AddFieldSet(" Repair Data Sheet ");
                this.Pub1.Add(" The latest version of the current data table structure , Do an auto repair ,  Fixes : Missing Column , Missing Column Comments , Column annotation is incomplete or there is a change . <br> <a href='DBInstall.aspx?DoType=FixDB' > Carried out ...</a>.");
                this.Pub1.AddB("<br><a href='/' > Into the Process Designer </a>");
                this.Pub1.AddFieldSetEnd();

                if (this.Request.QueryString["DoType"] == "FixDB")
                {
                    string rpt = BP.Sys.PubClass.DBRpt(BP.DA.DBCheckLevel.High);
                    this.Pub1.AddMsgGreen(" Synchronous data table structure success ,  Part of the error does not affect system operation .",
                        " Successful implementation , I hope to perform this function after each system upgrade , No impact on your database data .<br><br> <a href='./XAP/Designer.aspx'> Into the Process Designer .</a>");
                }
                return;
            }

            #region  Check whether the connection GPM
            if (BP.WF.Glo.OSModel == BP.WF.OSModel.BPM)
            {
                // First check whether installed on GPM.
                try
                {
                  //  CCPortal.API.CheckIsConn();
                }
                catch
                {

                    string msg = " Current ccflow The operating mode is integrated mode , You do not have to install or successful preparation CCGPM, ccflow的BPM Mode , Must rely on CCGPM To run , You can handle as follows .";
                    msg += "<ul>";
                    msg += "<li>1, Use ccflow的 workflow  Mode , 把web.config  The OSMode  Modified  0 .</li>";
                    msg += "<li>2, Use ccflow的 GPM  Mode ,  Installation ccgpm, Correct configuration ccflow Connection GPM Connection .</li>";
                    msg += "</ul>";

                    this.Pub1.AddFieldSetRed(" Error :",
                        msg);
                    return;
                }
            }
            #endregion  Check whether the connection GPM


            this.Pub1.AddFieldSet(" Select the installation language (ccflow5 Only supports Chinese ).");
            BP.WF.XML.Langs langs = new BP.WF.XML.Langs();
            langs.RetrieveAll();

            RadioButton rb = new RadioButton();
            foreach (BP.WF.XML.Lang lang in langs)
            {
                rb = new RadioButton();
                rb.Text = lang.Name;
                rb.ID = "RB_" + lang.No;
                rb.GroupName = "ch";
                if (lang.No == "CH")
                    rb.Checked = true;
                else
                    rb.Checked = false;
                rb.Enabled = false;
                this.Pub1.Add(rb);
            }
            this.Pub1.GetRadioButtonByID("RB_CH").Checked = true;
            this.Pub1.AddFieldSetEndBR();

            #region  Database Type .
            this.Pub1.AddFieldSet(" The current database installation type ( If you want to modify the database type, modify  web.config AppCenterDSNType  Set up .).");
            rb = new RadioButton();
            rb.Text = "SQLServer2000,2005,2008 Series version ";
            rb.ID = "RB_SQL";
            rb.GroupName = "sd";
            if (BP.Sys.SystemConfig.AppCenterDBType == DBType.MSSQL)
                rb.Checked = true;
            else
                rb.Checked = false;

            rb.Enabled = false;
            this.Pub1.Add(rb);
            this.Pub1.AddBR();

            rb = new RadioButton();
            rb.Text = "Oracle,Oracle9i,10g Series version ";
            rb.ID = "RB_Oracle";
            rb.GroupName = "sd";
            rb.Enabled = false;

            if (BP.Sys.SystemConfig.AppCenterDBType == DBType.Oracle)
                rb.Checked = true;
            else
                rb.Checked = false;

            this.Pub1.Add(rb);
            this.Pub1.AddBR();

            rb = new RadioButton();
            rb.Text = "Informix  Series version ( First, you need to perform :D:\\ccflow\\trunk\\CCFlow\\WF\\Data\\Install\\Informix.sql)";
            rb.ID = "RB_DB2";
            rb.GroupName = "sd";
            rb.Enabled = false;

            if (BP.Sys.SystemConfig.AppCenterDBType == DBType.Informix)
                rb.Checked = true;
            else
                rb.Checked = false;

            this.Pub1.Add(rb);
            this.Pub1.AddBR();

            rb = new RadioButton();
            rb.Text = "MySQL Series version ";
            rb.ID = "RB_MYSQL";
            rb.GroupName = "sd";
            rb.Enabled = false;

            if (BP.Sys.SystemConfig.AppCenterDBType == DBType.MySQL)
                rb.Checked = true;
            else
                rb.Checked = false;

            this.Pub1.Add(rb);
            this.Pub1.AddBR();
            this.Pub1.AddFieldSetEnd();
            #endregion  Database Type .


            #region  Installation Mode .
            this.Pub1.AddFieldSet("ccflow Operating mode , Manual changes  web.config  The  OSModel  Configure . ");
            rb = new RadioButton();
            rb.Text = " Integrated Mode ";
            rb.ID = "RB_WorkFlow";
            rb.GroupName = "model";
            if (BP.WF.Glo.OSModel == BP.WF.OSModel.WorkFlow)
                rb.Checked = true;
            rb.Enabled = false;
            this.Pub1.Add(rb);

            rb = new RadioButton();
            rb.Text = "BPM Mode ";
            rb.ID = "RB_BMP";
            rb.GroupName = "model";
            if (BP.WF.Glo.OSModel == BP.WF.OSModel.BPM)
                rb.Checked = true;
            rb.Enabled = false;
            this.Pub1.Add(rb);
            this.Pub1.AddBR();
            this.Pub1.AddFieldSetEnd();
            #endregion  Installation Mode .

            if (BP.WF.Glo.OSModel == BP.WF.OSModel.WorkFlow)
            {
                this.Pub1.AddFieldSet(" Whether you need to install CCIM.");
                rb = new RadioButton();
                rb.Text = "是";
                rb.ID = "RB_CCIM_Y";
                rb.Checked = true;
                rb.GroupName = "ccim";
                this.Pub1.Add(rb);
                rb = new RadioButton();
                rb.Text = "否";
                rb.ID = "RB_CCIM_N";
                rb.GroupName = "ccim";
                this.Pub1.Add(rb);
                this.Pub1.AddBR();
                this.Pub1.AddFieldSetEnd();

                this.Pub1.AddFieldSet(" Application Environment Simulation .");
                rb = new RadioButton();
                rb.Text = " Group Company , Business units .";
                rb.ID = "RB_Inc";
                rb.GroupName = "hj";
                rb.Checked = true;
                rb.Enabled = false;
                this.Pub1.Add(rb);
                this.Pub1.AddBR();
                rb = new RadioButton();
                rb.Text = " Government agencies , Institutions .";
                rb.ID = "RB_Gov";
                rb.GroupName = "hj";
                rb.Enabled = false;
                this.Pub1.Add(rb);
                this.Pub1.AddBR();
                this.Pub1.AddFieldSetEndBR();
            }

            this.Pub1.AddFieldSet(" Whether loading the demo process templates ?");
            rb = new RadioButton();
            rb.Text = "是: I want to install demo Organizational structure system ,demo Process Template , Form Template , To facilitate my learning ccflow与ccform.";
            rb.ID = "RB_DemoOn";
            rb.GroupName = "hjd";
            rb.Checked = true;
            this.Pub1.Add(rb);
            this.Pub1.AddBR();
            rb = new RadioButton();
            rb.Text = "否: Not installed demo.";
            rb.ID = "RB_DemoOff";
            rb.GroupName = "hjd";
            this.Pub1.Add(rb);
            this.Pub1.AddBR();
            this.Pub1.AddFieldSetEndBR();

            Button btn = new Button();
            btn.ID = "Btn_s";
            btn.Text = " Accept CCFlow的GPL Open source software agreement and install ";
            btn.CssClass = "Btn";
            btn.UseSubmitBehavior = false;
            btn.OnClientClick = "this.disabled=true;";
            btn.Click += new EventHandler(btn_Click);
            this.Pub1.Add(btn);

            this.Pub1.AddBR("1, Estimated 3-5 Within minutes, the installation is complete .");
            this.Pub1.AddBR("2, If you are using VS Please do not use open F5 Run it , Will result in the installation slower .");
            this.Pub1.AddBR("3. If you install an error , Please remove and re-install the database table , The encounter problems installing or feedback to ccflow. http://bbs.ccflow.org");

        }
        void btn_Click(object sender, EventArgs e)
        {
            string lang = "CH";
            BP.WF.XML.Langs langs = new BP.WF.XML.Langs();
            langs.RetrieveAll();
            bool isDemo = this.Pub1.GetRadioButtonByID("RB_DemoOn").Checked;

            // Run ccflow Installation 
            BP.WF.Glo.DoInstallDataBase(lang, isDemo);

            if (BP.WF.Glo.OSModel == BP.WF.OSModel.WorkFlow)
            {
                // Installation CCIM
                if (this.Pub1.GetRadioButtonByID("RB_CCIM_Y").Checked)
                {
                    BP.WF.Glo.DoInstallCCIM();
                }
            }

            // Add comment .
            BP.Sys.PubClass.AddComment();

            //  Load the demo data .
            if (this.Pub1.GetRadioButtonByID("RB_DemoOn").Checked)
            {
                BP.Port.Emp emp = new BP.Port.Emp("admin");
                BP.Web.WebUser.SignInOfGener(emp);
                BP.WF.DTS.LoadTemplete l = new BP.WF.DTS.LoadTemplete();
                string msg = l.Do() as string;
            }

            try
            {
                // Add pictures signature 
                BP.WF.DTS.GenerSiganture gs = new BP.WF.DTS.GenerSiganture();
                gs.Do();
            }
            catch
            {
            }
            this.Response.Redirect("DBInstall.aspx?DoType=OK", true);
        }
    }
}