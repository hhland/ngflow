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
using BP.En;
using BP.DA;

public partial class CCFlow_Comm_Sys_EnConfig : BP.Web.WebPageAdmin
{
    public new  string EnsName
    {
        get
        {
            return this.Request.QueryString["EnsName"];
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Entity en = BP.En.ClassFactory.GetEn(this.EnsName);
        this.UCSys1.AddTable();
        this.UCSys1.AddCaptionLeft(en.EnDesc + ": Data analysis attribute set ");
        this.UCSys1.AddTR();
        this.UCSys1.AddTDTitle(" Property ");
        this.UCSys1.AddTDTitle(" Chinese Name ");
        this.UCSys1.AddTDTitle(" Analytical methods can be used ");
        this.UCSys1.AddTREnd();

        Map map = en.EnMap;
        bool is1 = false;
        foreach (Attr attr in map.Attrs)
        {
            if (attr.IsNum == false)
                continue;
            if (attr.IsPK)
                continue;
            if (attr.UIContralType == UIContralType.TB == false)
                continue;
            if (attr.UIVisible == false)
                continue;
            if (attr.MyFieldType == FieldType.FK)
                continue;
            if (attr.MyFieldType == FieldType.Enum)
                continue;
            if (attr.MyFieldType == FieldType.Enum)
                continue;
            if (attr.Key == "OID" || attr.Key == "WorkID" || attr.Key == "MID" || attr.Key == "FID")
                continue;

            is1 = this.UCSys1.AddTR(is1);
            this.UCSys1.AddTD(attr.Key);
            this.UCSys1.AddTD(attr.Desc);
            this.UCSys1.AddTDBegin();
            CheckBox cb = new CheckBox();
            cb.ID = attr.Key + "_AVG";
            cb.Text = " Sum ";
            this.UCSys1.Add(cb);

            cb = new CheckBox();
            cb.ID = attr.Key + "_SUM";
            cb.Text = " Averaging ";
            this.UCSys1.Add(cb);

            cb = new CheckBox();
            cb.ID = attr.Key + "_AMOUNT";
            cb.Text = " Cumulative demand ";
            this.UCSys1.Add(cb);

            cb = new CheckBox();
            cb.ID = attr.Key + "_MAX";
            cb.Text = " Seeking maximum ";
            this.UCSys1.Add(cb);

            cb = new CheckBox();
            cb.ID = attr.Key + "_MIN";
            cb.Text = " Minimum requirements ";
            this.UCSys1.Add(cb);

            cb = new CheckBox();
            cb.ID = attr.Key + "_LSXS";
            cb.Text = " Dispersion coefficient ";
            this.UCSys1.Add(cb);

            this.UCSys1.AddTDEnd();
            this.UCSys1.AddTREnd();
        }
        this.UCSys1.AddTRSum();

        this.UCSys1.AddTD();
        this.UCSys1.AddTD();

        Button btn = new Button();
        btn.Text = " Save Settings ";
        btn.ID = "Btn_Save";
        btn.CssClass = "Btn";
        btn.Click += new EventHandler(btn_Click);
        this.UCSys1.AddTD(btn);

        this.UCSys1.AddTREnd();
        this.UCSys1.AddTableEnd();
    }

    void btn_Click(object sender, EventArgs e)
    {
        Entity en = BP.En.ClassFactory.GetEn(this.EnsName);
        Map map = en.EnMap;
        string keys = "";
        foreach (Attr attr in map.Attrs)
        {
            if (attr.IsNum == false)
                continue;
            if (attr.IsPK)
                continue;
            if (attr.UIContralType == UIContralType.TB == false)
                continue;
            if (attr.UIVisible == false)
                continue;
            if (attr.MyFieldType == FieldType.FK)
                continue;
            if (attr.MyFieldType == FieldType.Enum)
                continue;
            if (attr.MyFieldType == FieldType.Enum)
                continue;
            if (attr.Key == "OID" || attr.Key == "WorkID" || attr.Key == "MID" || attr.Key == "FID")
                continue;


            string strs = "@" + attr.Key + "=";
            CheckBox cb = this.UCSys1.GetCBByID(attr.Key + "_SUM");
            if (cb.Checked)
                strs += ".SUM.";

            cb = this.UCSys1.GetCBByID(attr.Key + "_AVG");
            if (cb.Checked)
                strs += ".AVG.";

            cb = this.UCSys1.GetCBByID(attr.Key + "_AMOUNT");
            if (cb.Checked)
                strs += ".AMOUNT.";

            cb = this.UCSys1.GetCBByID(attr.Key + "_MAX");
            if (cb.Checked)
                strs += ".MAX.";

            cb = this.UCSys1.GetCBByID(attr.Key + "_MIN");
            if (cb.Checked)
                strs += ".MIN.";

            cb = this.UCSys1.GetCBByID(attr.Key + "_LSXS");
            if (cb.Checked)
                strs += ".LSXS.";
            keys += strs;
        }

        BP.Sys.EnCfg cfg = new BP.Sys.EnCfg();
        cfg.No = this.EnsName;
        if (cfg.RetrieveFromDBSources() == 0)
        {
            cfg.Datan = keys;
            cfg.Insert();
        }
        else
        {
            cfg.Datan = keys;
            cfg.Update();
        }
        cfg.Retrieve();
        cfg.Datan = keys;
        this.Alert(" Setting Success .");
    }
}
