using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BP.Sys.Xml;
using BP.Web;
using BP.En;

public partial class CCFlow_Comm_Sys_EditWebconfig : BP.Web.WebPageAdmin
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.UCSys1.AddTable();
        this.UCSys1.AddCaptionLeft(" Site Global Configuration :( You can also open web.config Directly modify it )");
        this.UCSys1.AddTR();
        this.UCSys1.AddTDTitle("IDX");
        this.UCSys1.AddTDTitle(" Project Key");
        this.UCSys1.AddTDTitle(" Name ");
        this.UCSys1.AddTDTitle(" Value");
        this.UCSys1.AddTDTitle(" Description ");
        this.UCSys1.AddTREnd();

     // BP.Web.WebUser.Style

        WebConfigDescs ens = new WebConfigDescs();
        ens.RetrieveAll();

        Configuration cfg = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
        AppSettingsSection appSetting = cfg.AppSettings;
        bool is1 = false;
        int i = 1;
        foreach (System.Configuration.KeyValueConfigurationElement mycfg in appSetting.Settings)
        {
            WebConfigDesc en = ens.GetEnByKey("No", mycfg.Key) as WebConfigDesc;
            if (en == null)
            {
                en = new WebConfigDesc();
                en.No = mycfg.Key;
            }

            is1 = this.UCSys1.AddTR(is1);
            this.UCSys1.AddTDIdx(i++);
            this.UCSys1.AddTD(en.No);
            this.UCSys1.AddTD(en.Name);
            switch (en.DBType)
            {
                case "Boolen":

                    RadioButton rb1 = new RadioButton();
                    rb1.Text = "Yes";
                    rb1.GroupName = en.No;
                    rb1.ID = "rb1" + en.No;
                    rb1.Enabled = en.IsEnable;

                    RadioButton rb0 = new RadioButton();
                    rb0.Text = "No";
                    rb0.GroupName = en.No;
                    rb0.ID = "rb0" + en.No;
                    rb0.Enabled = en.IsEnable;

                 
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings[en.No] == "1")
                        rb1.Checked = true;
                    else
                        rb0.Checked = true;


                    this.UCSys1.AddTDBegin();
                    this.UCSys1.Add(rb1);
                    this.UCSys1.Add(rb0);
                    this.UCSys1.AddTDEnd();
                    break;
                case "Enum":
                    BP.Web.Controls.DDL ddl = new BP.Web.Controls.DDL();
                    ddl.ID = "DDL_" + en.No;
                    ddl.Enabled = en.IsEnable;

                    BP.Sys.SysEnums ses = new BP.Sys.SysEnums(en.No, en.Vals);
                    ddl.BindSysEnum(en.No, int.Parse( System.Web.Configuration.WebConfigurationManager.AppSettings[en.No] ));
                    this.UCSys1.AddTD(ddl);
                    break;
                case "String":
                default:
                    TextBox tb = new TextBox();
                    tb.ID = "TB_" + en.No;
                    tb.Text = System.Web.Configuration.WebConfigurationManager.AppSettings[en.No];
                    tb.Columns = 80;
                    tb.Enabled = en.IsEnable;
                    this.UCSys1.AddTD(tb);
                    break;
            }
            this.UCSys1.AddTD(en.Note);
            this.UCSys1.AddTREnd();
        }
        this.UCSys1.AddTRSum();
        this.UCSys1.AddTD();
        this.UCSys1.AddTD();
        this.UCSys1.AddTD();

        Button btn = new Button();
        btn.ID = "Btn_Save";
        btn.Text = "  Save Global Settings  ";
        btn.CssClass = "Btn";

        this.UCSys1.AddTD(btn);
        btn.Click += new EventHandler(btn_Click);
        this.UCSys1.AddTD();
        this.UCSys1.AddTREnd();
        this.UCSys1.AddTableEnd();
    }

    void btn_Click(object sender, EventArgs e)
    {
        this.Save();
    }
    public void Save()
    {
        Configuration cfg = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
        AppSettingsSection appSetting = cfg.AppSettings;
        WebConfigDescs ens = new WebConfigDescs();
        ens.RetrieveAll();
        foreach (System.Configuration.KeyValueConfigurationElement mycfg in appSetting.Settings)
        {
            WebConfigDesc en = ens.GetEnByKey("No", mycfg.Key) as WebConfigDesc;
            if (en == null)
            {
                en = new WebConfigDesc();
                en.No = mycfg.Key;
            }

            this.UCSys1.AddTR();
            this.UCSys1.AddTD(en.No);
            this.UCSys1.AddTD(en.Name);
            switch (en.DBType)
            {
                case "Boolen":
                    if (this.UCSys1.GetRadioButtonByID("rb1" + mycfg.Key).Checked)
                        mycfg.Value = "1";
                    else
                        mycfg.Value = "0";
                    break;
                case "Enum":
                    mycfg.Value = this.UCSys1.GetDDLByID("DDL_" + mycfg.Key).Text;
                    break;
                case "String":
                default:
                    mycfg.Value = this.UCSys1.GetTextBoxByID("TB_" + mycfg.Key).Text;
                    break;
            }
        }
        cfg.Save();
        this.Response.Redirect("EditWebconfig.aspx", true);
        // BP.Sys.PubClass.Alert(" Saved successfully .");
    }
}
