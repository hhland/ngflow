using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Sys;
using BP.DA;

public partial class WF_Admin_DBInstall : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Pub1.AddH3("GPM  Database repair and installation tools ");
        this.Pub1.AddHR();
        if (this.Request.QueryString["DoType"] == "OK")
        {
            this.Pub1.AddFieldSet(" Prompt ");
            this.Pub1.Add("GPM Database initialization success .");
            this.Pub1.AddBR("<a href='/SSO/Loginin.aspx?DoType=Logout' > Landed  admin  The password is the first test of  pub .</a>");
            this.Pub1.AddFieldSetEnd();
            return;
        }

        if (BP.DA.DBAccess.IsExitsObject("Port_Emp") == true)
        {
            this.Pub1.AddFieldSet(" Prompt ");
            this.Pub1.Add(" Data has been installed , If you want to reinstall , You need to manually remove the database objects .");
            this.Pub1.AddFieldSetEnd();

            this.Pub1.AddFieldSet(" Repair Data Sheet ");
            this.Pub1.Add(" The latest version of the current data table structure , Do an auto repair ,  Fixes : Missing Column , Missing Column Comments , Column annotation is incomplete or there is a change . <br> <a href='DBInstall.aspx?DoType=FixDB' > Carried out ...</a>.");
            this.Pub1.AddFieldSetEnd();

            if (this.Request.QueryString["DoType"] == "FixDB")
            {
                #region  Repair  Port_Emp  Table Fields  EmpNo
                //switch (BP.Sys.SystemConfig.AppCenterDBType)
                //{
                //    case DBType.Oracle:
                //        int i = DBAccess.RunSQLReturnCOUNT("SELECT * FROM USER_TAB_COLUMNS WHERE TABLE_NAME = 'PORT_EMP' AND COLUMN_NAME = 'EMPNO'");
                //        if (i == 0)
                //        {
                //            DBAccess.RunSQL("ALTER TABLE PORT_EMP ADD (EMPNO nvarchar(20))");
                //        }
                //        break;
                //    case DBType.MSSQL:
                //        i = DBAccess.RunSQLReturnCOUNT("SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('Port_Emp') AND NAME='EmpNo'");
                //        if (i == 0)
                //        {
                //            DBAccess.RunSQL("ALTER TABLE Port_Emp ADD EmpNo nvarchar(20)");
                //        }
                //        break;
                //    default:
                //        break;
                //}
                #endregion

                string rpt = BP.Sys.PubClass.DBRpt(BP.DA.DBCheckLevel.High);

                this.Pub1.AddMsgGreen(" Synchronous data table structure success ,  Part of the error does not affect system operation .",
                    " Successful implementation , I hope to perform this function after each system upgrade , No impact on your database data .<br><br> <a href='/'> Login now GPM.</a>");
            }
            return;
        }


        // this.Pub1.AddH2(" Database Setup Wizard ...");
        RadioButton rb = new RadioButton();

        //this.Pub1.AddFieldSet(" Select the installation language .");
        //BP.WF.XML.Langs langs = new BP.WF.XML.Langs();
        //langs.RetrieveAll();
        //RadioButton rb = new RadioButton();
        //foreach (BP.WF.XML.Lang lang in langs)
        //{
        //    rb = new RadioButton();
        //    rb.Text = lang.Name;
        //    rb.ID = "RB_" + lang.No;
        //    rb.GroupName = "ch";
        //    this.Pub1.Add(rb);
        //    this.Pub1.AddBR();
        //}
        //this.Pub1.GetRadioButtonByID("RB_CH").Checked = true;
        //this.Pub1.AddFieldSetEndBR();


        this.Pub1.AddFieldSet(" Select the type of database installation .");
        rb = new RadioButton();
        rb.Text = "SQLServer2000,2005,2008";
        rb.ID = "MSSQL";
        if (BP.Sys.SystemConfig.AppCenterDBType == BP.DA.DBType.MSSQL)
            rb.Checked = true;

        rb.GroupName = "sd";
        rb.Checked = true;
        rb.Enabled = false;
        this.Pub1.Add(rb);
        this.Pub1.AddBR();

        rb = new RadioButton();
        rb.Text = "Oracle,Oracle 10g";
        rb.ID = "RB_Oracle";
        rb.GroupName = "sd";
        if (BP.Sys.SystemConfig.AppCenterDBType == BP.DA.DBType.Oracle)
            rb.Checked = true;
        rb.Enabled = false;
        this.Pub1.Add(rb);
        this.Pub1.AddBR();

        rb = new RadioButton();
        rb.Text = "DB2";
        rb.ID = "RB_DB2";
        rb.GroupName = "sd";
        if (BP.Sys.SystemConfig.AppCenterDBType == BP.DA.DBType.DB2)
            rb.Checked = true;
        rb.Enabled = false;
        this.Pub1.Add(rb);
        this.Pub1.AddBR();

        rb = new RadioButton();
        rb.Text = "MySQL";
        rb.ID = "RB_MYSQL";
        rb.GroupName = "sd";
        if (BP.Sys.SystemConfig.AppCenterDBType == BP.DA.DBType.MySQL)
            rb.Checked = true;
        rb.Enabled = false;
        this.Pub1.Add(rb);
        this.Pub1.AddBR();
        this.Pub1.AddFieldSetEnd();

        this.Pub1.AddFieldSet(" Whether you need to install CCIM.");
        rb = new RadioButton();
        rb.Text = "Yes";
        rb.ID = "RB_CCIM_Y";
        rb.Checked = true;
        rb.GroupName = "ccim";
        this.Pub1.Add(rb);
        this.Pub1.AddBR();
        rb = new RadioButton();
        rb.Text = "No";
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

        //this.Pub1.AddFieldSet(" Whether loading the demo process templates ?");
        //rb = new RadioButton();
        //rb.Text = "是: I want to install demo Process Template , Form Template , To facilitate my learning ccflow与ccform.";
        //rb.ID = "RB_DemoOn";
        //rb.GroupName = "hjd";
        //rb.Checked = true;
        //this.Pub1.Add(rb);
        //this.Pub1.AddBR();
        //rb = new RadioButton();
        //rb.Text = "否: Not installed .";
        //rb.ID = "RB_DemoOff";
        //rb.GroupName = "hjd";
        //this.Pub1.Add(rb);
        //this.Pub1.AddBR();
        //this.Pub1.AddFieldSetEndBR();

        Button btn = new Button();
        btn.ID = "Btn_s";
        btn.Text = " Next ";
        btn.UseSubmitBehavior = false;
        btn.OnClientClick = "this.disabled=true;";
        btn.Click += new EventHandler(btn_Click);
        this.Pub1.Add(btn);
    }
    void btn_Click(object sender, EventArgs e)
    {
        string lang = "CH";
        string hj = "Inc";

        //if (this.Pub1.GetRadioButtonByID("RB_Inc").Checked)
        //    hj = "Inc";
        //if (this.Pub1.GetRadioButtonByID("RB_Gov").Checked)
        //    hj = "Gov";

        hj = "Inc";
        // Run .
        BP.GPM.Glo.DoInstallDataBase(lang, hj);

        // Installation CCIM
        if (this.Pub1.GetRadioButtonByID("RB_CCIM_Y").Checked)
        {
            BP.GPM.Glo.DoInstallCCIM(lang, hj);
        }

        // Add comment .
        BP.Sys.PubClass.AddComment();

        this.Response.Redirect("DBInstall.aspx?DoType=OK", true);
    }
}