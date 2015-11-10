using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.DA;
using BP.En;

public partial class Comm_RefFunc_EntitiesTree : System.Web.UI.Page
{
    #region 属性
    string EnsName
    {
        get
        {
            return this.Request.QueryString["EnsName"];
        }
    }
    string No
    {
        get
        {
            string s = this.Request.QueryString["No"];
            if (string.IsNullOrEmpty(s))
                s = "0001";
            return s;
        }
    }
    #endregion

    public EntitiesTree HisEns = null;
    public EntityTree HisEn = null;
    public void DoFunc(EntityTree en)
    {
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.HisEns = ClassFactory.GetEns(this.EnsName) as EntitiesTree;
        this.HisEn = this.HisEns.GetNewEntity as EntityTree;
        if (this.HisEn == null)
            throw new Exception("@非 EntityTree 实体.");

        this.HisEns.RetrieveAll(EntityTreeAttr.TreeNo);
        if (HisEns.Count == 0)
        {
            this.HisEn.No = "0001";
            this.HisEn.ParentNo = "-1";
            this.HisEn.Name = "根节点";
            this.HisEn.TreeNo = "01";
            this.HisEn.IsDir = true; 
            this.HisEn.Insert();

            this.HisEn = this.HisEns.GetNewEntity as EntityTree;
            this.HisEn.No = "0002";
            this.HisEn.ParentNo = "0001";
            this.HisEn.Name = "目录1";
            this.HisEn.TreeNo = "0101";
            this.HisEn.IsDir = false; 
            this.HisEn.Insert();
            this.HisEns.RetrieveAll(EntityTreeAttr.TreeNo);
        }

        string no=this.Request.QueryString[BP.En.EntityTreeAttr.No];
        if (string.IsNullOrEmpty(no)==false)
        {
            this.HisEn.PKVal = no;
            this.HisEn.Retrieve();
        }

        string pk = null;
        switch (this.Request.QueryString["DoType"])
        {
            case "AddSameLevelNode":
                pk = this.HisEn.DoCreateSameLevelNode().No;
                this.Response.Redirect("EntitiesTree.aspx?EnsName=" + this.HisEns.ToString() + "&EnName=" + this.HisEn.ToString() + "&PK=" + pk, true);
                break;
            case "AddSubNode":
                pk = this.HisEn.DoCreateSubNode().No;
                this.Response.Redirect("EntitiesTree.aspx?EnsName=" + this.HisEns.ToString() + "&EnName=" + this.HisEn.ToString() + "&PK=" + pk, true);
                return;
            case "DoUp":
                this.HisEn.DoUp();
                break;
            case "DoDown":
                this.HisEn.DoDown();
                break;
            default:
                break;
        }

        this.HisEn.No = this.ID;
        this.Title = HisEn.EnDesc;
        this.BindTree();
    }
    public void BindTree()
    {
        Map map = this.HisEn.EnMap;
        this.Pub1.AddTable();
        this.Pub1.AddTR();
        this.Pub1.AddTDTitle("IDX");
        this.Pub1.AddTDTitle("树");
        foreach (Attr attr in map.Attrs)
        {
            if (attr.Key == BP.En.EntityTreeAttr.No || attr.Key == "Name"
                || attr.IsRefAttr || attr.UIVisible == false)
                continue;
            this.Pub1.AddTDTitle(attr.Desc);
        }
        this.Pub1.AddTREnd();

        int i = 0;
        foreach (EntityTree en in this.HisEns)
        {
            i++;
            this.Pub1.AddTR();

            RadioButton rb = new RadioButton();
            rb.ID = "RB_" + en.No;
            rb.GroupName = "s";
            rb.Attributes["onclick"] = "javascript:pk='"+en.No+"';";
            rb.Text = i.ToString();

            this.Pub1.AddTDBegin("class=Idx");
            this.Pub1.Add(rb);
            this.Pub1.AddTDEnd();

            this.Pub1.AddTDBegin();
            this.Pub1.Add(DataType.GenerSpace(en.Grade-1));
            string text = "";
            if (en.No == "0001")
                text = "<img src='/Images/Tree/root.gif' border=0/>" + en.Name;
            else if (en.IsDir)
                text = "<img src='/Images/Tree/Dir.gif' border=0/>" + en.Name;
            else
                text = "<img src='/Images/Tree/File.gif' border=0/>" + en.Name;

            this.Pub1.Add("<a href=\"javascript:ShowEn('../RefFunc/UIEn.aspx?EnsName=" + this.EnsName + "&PK=" + en.No + "','sd',500,700)\">" + text + "</a>");
            this.Pub1.AddTDEnd();

            foreach (Attr attr in map.Attrs)
            {
                if (attr.Key == "No" || attr.Key == "Name"
                    || attr.IsRefAttr || attr.UIVisible == false)
                    continue;

                if (attr.IsEnum)
                    this.Pub1.AddTD(en.GetValRefTextByKey(attr.Key));
                else
                    this.Pub1.AddTD(en.GetValStrByKey(attr.Key));
            }
            this.Pub1.AddTREnd();
        }
        this.Pub1.AddTableEnd();
    }
}
