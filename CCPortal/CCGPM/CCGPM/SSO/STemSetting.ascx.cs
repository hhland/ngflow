using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Web;
using BP.Port;
using BP.GPM;

public partial class SSO_PerSetting : BP.Web.UC.UCBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //this.AddTable();
        this.Add("<table class='STemSetting'>");
        this.AddTR();
        this.AddTDTitle("序号");
        this.AddTDTitle("系统编号");
        this.AddTDTitle("名称");
        this.AddTDTitle("用户名");
        this.AddTDTitle("密码");
        this.AddTREnd();

        Apps ens = new Apps();
        ens.RetrieveAll();

        PerSettings pss = new PerSettings();
        pss.Retrieve(PerSettingAttr.FK_Emp, WebUser.No);

        int idx = 0;
        foreach (App en in ens)
        {
            idx++;
            this.AddTR();
            this.AddTDIdx(idx);
            this.AddTD(en.No);
            this.AddTD(en.Name);

            PerSetting ps = pss.GetEntityByKey(PerSettingAttr.FK_App, en.No) as PerSetting;
            if (ps == null)
                ps = new PerSetting();

            TextBox tb = new TextBox();
            tb.ID = en.No + "_No";
            tb.Text = ps.UserNo;
            this.AddTD(tb);

            tb = new TextBox();
            tb.ID = en.No + "_Pass";
            tb.TextMode = TextBoxMode.Password;
            tb.Text = ps.UserPass;
            this.AddTD(tb);
            this.AddTREnd();
        }

        this.AddTR();
        this.AddTDBegin("colspan=5");

        Button btn = new Button();
        btn.Text = " 保 存 ";
        btn.ID = "Save";
        btn.Click += new EventHandler(btn_Click);
        this.Add(btn);
        this.AddTDEnd();
        this.AddTREnd();
        this.AddTableEnd();

     
    }
    void btn_Click(object sender, EventArgs e)
    {
        Apps ens = new Apps();
        ens.RetrieveAll();
        PerSettings pss = new PerSettings();
        pss.Retrieve(PerSettingAttr.FK_Emp, WebUser.No);
        foreach (App en in ens)
        {
            PerSetting ps = pss.GetEntityByKey(PerSettingAttr.FK_App, en.No) as PerSetting;
            if (ps == null)
                ps = new PerSetting();
            ps.FK_App = en.No;
            ps.FK_Emp = WebUser.No;
            ps.UserNo = this.GetTextBoxByID(en.No + "_No").Text;
            ps.UserPass = this.GetTextBoxByID(en.No + "_Pass").Text;
            ps.Save();
        }
        this.Response.Redirect(this.Request.RawUrl);
    }
}