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
using BP.Web;
using BP.Sys;
using BP.Port;
using BP.DA;
using BP.Sys.Xml;
using BP.En;

public partial class CCFlow_Comm_Sys_EnsAppCfg : BP.Web.WebPageAdmin
{

    public new string EnsName
    {
        get
        {
            if (this.Request.QueryString["EnsName"] == null)
                return "BP.GE.Infos";
            return this.Request.QueryString["EnsName"];
        }
    }
    public void BindAdv()
    {
        EnsAppCfgs ens = new EnsAppCfgs();
        ens.Retrieve(EnsAppCfgAttr.EnsName, this.EnsName);

        if (ens.Count == 0)
        {
            this.UCSys1.AddMsgOfInfo(" Prompt :", " No application configuration settings .");
            return;
        }

        this.UCSys1.AddTable("width=100%");
        if (BP.Web.WebUser.No == "admin")
            this.UCSys1.AddCaptionLeftTX("<a href='?EnsName=" + this.EnsName + "'> Basic Settings </a> - <a href='?EnsName=" + this.EnsName + "&DoType=Adv'> Advanced Settings </a> - <a href='EnsDataIO.aspx?EnsName=" + this.EnsName + "' > Import and Export </a>");
        else
            this.UCSys1.AddCaptionLeftTX(" Basic Settings ");

        this.UCSys1.AddTR();
        this.UCSys1.AddTDTitle( " Configuration Item ");
        this.UCSys1.AddTDTitle(" Content ");
        this.UCSys1.AddTDTitle(" Information " );
        this.UCSys1.AddTREnd();

        Entity en1 = BP.En.ClassFactory.GetEns(this.EnsName).GetNewEntity;
        Attrs attrs = en1.EnMap.HisCfgAttrs;
        bool is1 = false;
        foreach (Attr attr in attrs)
        {
            if (attr.IsRefAttr)
                continue;
            EnsAppCfg en = ens.GetEntityByKey(EnsAppCfgAttr.CfgKey, attr.Key) as EnsAppCfg;
            is1 = this.UCSys1.AddTR(is1);
            this.UCSys1.AddTD(attr.Key);
            this.UCSys1.AddTD(attr.Desc);
            switch (attr.UIContralType)
            {
                case UIContralType.DDL:
                    BP.Web.Controls.DDL ddl = new BP.Web.Controls.DDL();
                    ddl.ID = "DDL_" + attr.Key;
                    
                        SysEnums ses = new SysEnums(attr.Key, attr.UITag);
                        ddl.BindSysEnum(attr.Key);
                        if (en == null)
                            ddl.SetSelectItem(attr.DefaultVal.ToString());
                        else
                            ddl.SetSelectItem(en.CfgValOfInt);
                    
                    this.UCSys1.AddTD(ddl);
                    break;
                case UIContralType.CheckBok:
                    CheckBox cb = new CheckBox();
                    cb.ID = "CB_" + attr.Key;
                    cb.Text = attr.Desc;
                    if (en == null)
                    {
                        if (attr.DefaultVal.ToString() == "0")
                            cb.Checked = false;
                        else
                            cb.Checked = true;
                    }
                    else
                    {
                        cb.Checked = en.CfgValOfBoolen;
                    }
                    this.UCSys1.AddTD(cb);
                    break;
                default:
                    TextBox tb = new TextBox();
                    tb.ID = "TB_" + attr.Key;
                    if (en == null)
                        tb.Text = attr.DefaultVal.ToString();
                    else
                        tb.Text = en.CfgVal;
                    tb.Attributes["width"] = "100%";
                    this.UCSys1.AddTD(tb);
                    break;
            }
            this.UCSys1.AddTREnd();
        }

        this.UCSys1.AddTableEnd();
        Button btn = new Button();
        btn.ID = "Btn_Save";
        btn.CssClass = "Btn";
        btn.Text = " Save ";
        btn.Click += new EventHandler(btn_Click);
        this.UCSys1.Add(btn);

        btn = new Button();
        btn.ID = "Btn_SaveAndClose";
        btn.CssClass = "Btn";

        btn.Text = " Save and Close ";
        btn.Click += new EventHandler(btn_Click);
        this.UCSys1.Add(btn);
    }
    public void BindNormal()
    {
        EnsAppCfgs ens = new EnsAppCfgs();
        ens.Retrieve(EnsAppCfgAttr.EnsName, this.EnsName);

        EnsAppXmls xmls = new EnsAppXmls();
        xmls.Retrieve(EnsAppCfgAttr.EnsName, this.EnsName);

        this.UCSys1.AddTable("width=100%");
        if (BP.Web.WebUser.No == "admin")
            this.UCSys1.AddCaptionLeftTX("<a href='?EnsName=" + this.EnsName + "'> Basic Settings </a> - <a href='?EnsName=" + this.EnsName + "&DoType=Adv'> Advanced Settings </a> - <a href='EnsDataIO.aspx?EnsName=" + this.EnsName + "' > Import and Export </a>");
        else
            this.UCSys1.AddCaptionLeftTX(" Basic Settings ");
        this.UCSys1.AddTR();

        this.UCSys1.AddTR();
        this.UCSys1.AddTDTitle(" Configuration Item ");
        this.UCSys1.AddTDTitle( " Content ");
        this.UCSys1.AddTDTitle( " Information ");
        this.UCSys1.AddTDTitle(" Remark ");

        //this.UCSys1.AddTDTitle(" Configuration Item ");
        //this.UCSys1.AddTDTitle(" Content ");
        //this.UCSys1.AddTDTitle(" Information ");
        //this.UCSys1.AddTDTitle(" Remark ");
        this.UCSys1.AddTREnd();

        bool is1 = false;
        foreach (EnsAppXml xml in xmls)
        {
            EnsAppCfg en = ens.GetEntityByKey(EnsAppCfgAttr.CfgKey, xml.No) as EnsAppCfg;
            is1 = this.UCSys1.AddTR(is1);
            this.UCSys1.AddTD(xml.No);
            this.UCSys1.AddTD(xml.Name);
            switch (xml.DBType)
            {
                case "Enum":
                    BP.Web.Controls.DDL ddl = new BP.Web.Controls.DDL();
                    ddl.ID = "DDL_" + xml.No;

                    SysEnums ses = new SysEnums(xml.EnumKey, xml.EnumVals);
                    ddl.BindSysEnum(xml.EnumKey);

                    if (en == null)
                        ddl.SetSelectItem(xml.DefValInt);
                    else
                        ddl.SetSelectItem(en.CfgValOfInt);
                    this.UCSys1.AddTD(ddl);
                    break;
                case "Boolen":
                    CheckBox cb = new CheckBox();
                    cb.ID = "CB_" + xml.No;
                    cb.Text = xml.Name;
                    if (en == null)
                        cb.Checked = xml.DefValBoolen;
                    else
                        cb.Checked = en.CfgValOfBoolen;
                    this.UCSys1.AddTD(cb);
                    break;
                default:
                    TextBox tb = new TextBox();
                    tb.ID = "TB_" + xml.No;
                    if (en == null)
                        tb.Text = xml.DefVal;
                    else
                        tb.Text = en.CfgVal;
                    tb.Attributes["width"] = "100%";
                    this.UCSys1.AddTD(tb);
                    break;
            }
            this.UCSys1.AddTDBigDoc(xml.Desc);
            this.UCSys1.AddTREnd();
        }

        if (xmls.Count == 0)
        {
            this.UCSys1.AddTableEnd();
            return;
        }


        this.UCSys1.AddTableEnd();
        Button btn = new Button();
        btn.ID = "Btn_Save";
        btn.Text =  " Save ";
        btn.CssClass = "Btn";
        btn.Click += new EventHandler(btn_Click);
        this.UCSys1.Add(btn);

        btn = new Button();
        btn.ID = "Btn_SaveAndClose";
        btn.CssClass = "Btn";
        btn.Text =  " Save and Close ";
        btn.Click += new EventHandler(btn_Click);
        this.UCSys1.Add(btn);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        switch (this.DoType)
        {
            case "Adv":
                this.BindAdv();
                break;
            default:
                this.BindNormal();
                break;
        }
    }
    void btn_Click(object sender, EventArgs e)
    {

        if (this.DoType == null)
        {
            EnsAppXmls xmls = new EnsAppXmls();
            xmls.Retrieve(EnsAppCfgAttr.EnsName, this.EnsName);
            foreach (EnsAppXml xml in xmls)
            {
                EnsAppCfg en = new EnsAppCfg(this.EnsName + "@" + xml.No);
                string val = "";
                switch (xml.DBType)
                {
                    case "Enum":
                        val = this.UCSys1.GetDDLByID("DDL_" + xml.No).SelectedItemStringVal;
                        break;
                    case "Boolen":
                        if (this.UCSys1.GetCBByID("CB_" + xml.No).Checked)
                            val = "1";
                        else
                            val = "0";
                        break;
                    default:
                        val = this.UCSys1.GetTextBoxByID("TB_" + xml.No).Text;
                        break;
                }
                en.CfgVal = val;
                en.EnsName = this.EnsName;
                en.CfgKey = xml.No;
                en.Save();
            }
        }

        if (WebUser.No == "admin" && this.DoType != null)
        {
            Entity en1 = BP.En.ClassFactory.GetEns(this.EnsName).GetNewEntity;
            Attrs attrs = en1.EnMap.HisCfgAttrs;
            foreach (Attr attr in attrs)
            {
                if (attr.IsRefAttr)
                    continue;

                EnsAppCfg en = new EnsAppCfg(this.EnsName + "@" + attr.Key);
                string val = "";
                switch (attr.UIContralType)
                {
                    case UIContralType.DDL:
                        val = this.UCSys1.GetDDLByID("DDL_" + attr.Key).SelectedItemStringVal;
                        break;
                    case UIContralType.CheckBok:
                        if (this.UCSys1.GetCBByID("CB_" + attr.Key).Checked)
                            val = "1";
                        else
                            val = "0";
                        break;
                    default:
                        val = this.UCSys1.GetTextBoxByID("TB_" + attr.Key).Text;
                        break;
                }
                en.CfgVal = val;
                en.EnsName = this.EnsName;
                en.CfgKey = attr.Key;

                if (attr.Key.Contains("Glo"))
                {
                    BP.DA.DBAccess.RunSQL("UPDATE Sys_EnsAppCfg SET CfgVal='" + val + "' WHERE CfgKey='" + attr.Key + "'");
                }
                en.Save();
            }
        }
        Button btn = sender as Button;
        if (btn.ID.Contains("Close"))
            this.WinClose();
        //   Button 
    }
}
