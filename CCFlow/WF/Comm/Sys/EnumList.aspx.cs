using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Sys;
using BP.En;
using BP.DA;

public partial class CCFlow_Comm_Sys_EnumList : BP.Web.WebPageAdmin
{
    public void BindRefNo()
    {
        SysEnumMain sem = new SysEnumMain(this.RefNo);
        this.UCSys1.AddTable();
        this.UCSys1.AddCaptionLeft("<a href=EnumList.aspx ><img src='./../../Img/Btn/Home.gif' border=0> Enumeration value list </a> -<a href='EnumList.aspx?DoType=New' ><img src='./../../Img/Btn/New.gif' border=0/> New </a>- <img src='./../../Img/Btn/Edit.gif' border /> Editor :" + sem.No + " " + sem.Name);

        this.UCSys1.AddTR();
        Button btn = new Button();
        btn.ID = "Btn_Save";
        btn.CssClass = "Btn";
        btn.Text = "  Save  ";
        btn.Click += new EventHandler(btn_Click);
        this.UCSys1.AddTDTitle("colspan=3", btn);
        this.UCSys1.AddTREnd();

        this.UCSys1.AddTR();
        this.UCSys1.AddTDTitle(" Project ");
        this.UCSys1.AddTDTitle(" Collection ");
        this.UCSys1.AddTDTitle(" Explanation ");
        this.UCSys1.AddTREnd();

        SysEnums ses = new SysEnums();
        ses.Retrieve(SysEnumAttr.EnumKey, this.RefNo);

        this.UCSys1.AddTRSum();
        this.UCSys1.AddTD(" Serial number ");
        TextBox tb = new TextBox();
        tb.ID = "TB_No";
        tb.Text = this.RefNo;
        tb.Enabled = false;
        this.UCSys1.AddTD(tb);
        this.UCSys1.AddTD(" Can not be modified ");
        this.UCSys1.AddTREnd();

        this.UCSys1.AddTRSum();
        this.UCSys1.AddTD(" Name ");
        tb = new TextBox();
        tb.ID = "TB_Name";
        tb.Text = sem.Name;
        this.UCSys1.AddTD(tb);
        this.UCSys1.AddTD("");
        this.UCSys1.AddTREnd();

        int myNum = 0;
        foreach (SysEnum se in ses)
        {
            this.UCSys1.AddTR();
            this.UCSys1.AddTD(se.IntKey);
            tb = new TextBox();
            tb.ID = "TB_" + se.IntKey;
            tb.Text = se.Lab;
            tb.Columns = 50;
            this.UCSys1.AddTD(tb);
            this.UCSys1.AddTD("");
            this.UCSys1.AddTREnd();
            myNum = se.IntKey;
        }

        myNum++;

        for (int i = myNum; i < 20; i++)
        {
            this.UCSys1.AddTR();
            this.UCSys1.AddTD(i);
            tb = new TextBox();
            tb.ID = "TB_" + i;
            tb.Columns = 50;
            this.UCSys1.AddTD(tb);
            this.UCSys1.AddTD("");
            this.UCSys1.AddTREnd();
        }
        this.UCSys1.AddTableEnd();
    }
    public void BindNew()
    {
        this.UCSys1.AddTable();
        this.UCSys1.AddCaptionLeftTX("<a href=EnumList.aspx ><img src='./../../Img/Btn/Home.gif' border=0 /> Enumeration value list </a> - <img src='./../../Img/Btn/New.gif' /> New enumeration value ");

        this.UCSys1.AddTR();
        Button btn = new Button();
        btn.ID = "Btn_Save";
        btn.CssClass = "Btn";
        btn.Text = "  Save  ";
        btn.Click += new EventHandler(btn_New_Click);
        this.UCSys1.AddTDTitle("colspan=3", btn);
        this.UCSys1.AddTREnd();

        this.UCSys1.AddTR();
        this.UCSys1.AddTDTitle(" Project ");
        this.UCSys1.AddTDTitle(" Collection ");
        this.UCSys1.AddTDTitle(" Explanation ");
        this.UCSys1.AddTREnd();

        this.UCSys1.AddTRSum();
        this.UCSys1.AddTD(" Serial number ");
        TextBox tb = new TextBox();
        tb.ID = "TB_No";
        this.UCSys1.AddTD(tb);
        this.UCSys1.AddTD(" Unique numbering system and begin with a letter or an underscore ");
        this.UCSys1.AddTREnd();

        this.UCSys1.AddTRSum();
        this.UCSys1.AddTD(" Name ");
        tb = new TextBox();
        tb.ID = "TB_Name";
        this.UCSys1.AddTD(tb);
        this.UCSys1.AddTD(" Can not be empty ");
        this.UCSys1.AddTREnd();
        for (int i = 0; i < 20; i++)
        {
            this.UCSys1.AddTR();
            this.UCSys1.AddTD(i);
            tb = new TextBox();
            tb.ID = "TB_" + i;
            tb.Columns = 50;
            this.UCSys1.AddTD(tb);
            this.UCSys1.AddTD("");
            this.UCSys1.AddTREnd();
        }
        this.UCSys1.AddTableEnd();
    }

    void btn_Click(object sender, EventArgs e)
    {
        SysEnums ses = new SysEnums();
        for (int i = 0; i < 20; i++)
        {
            TextBox tb = this.UCSys1.GetTextBoxByID("TB_" + i);
            if (tb == null)
                continue;
            if (string.IsNullOrEmpty(tb.Text))
                continue;

            SysEnum se = new SysEnum();
            se.IntKey = i;
            se.Lab = tb.Text.Trim();
            se.Lang = BP.Web.WebUser.SysLang;
            se.EnumKey = this.RefNo;
            se.MyPK = se.EnumKey + "_" + se.Lang + "_" + se.IntKey;
            ses.AddEntity(se);
        }

        if (ses.Count == 0)
        {
            this.Alert(" Enumerate the project can not be empty .");
            return;
        }

        ses.Delete(SysEnumAttr.EnumKey, this.RefNo);

        string lab = "";
        foreach (SysEnum se in ses)
        {
            se.Save();
            lab += "@" + se.IntKey + "=" + se.Lab;
        }
        SysEnumMain main = new SysEnumMain(this.RefNo);
        main.CfgVal = lab;
        main.Update();
        this.Alert(" Saved successfully .");
    }

    void btn_New_Click(object sender, EventArgs e)
    {
        string no = this.UCSys1.GetTextBoxByID("TB_No").Text;
        string name = this.UCSys1.GetTextBoxByID("TB_Name").Text;
        SysEnumMain m = new SysEnumMain();
        m.No = no;
        if (m.RetrieveFromDBSources() == 1)
        {
            this.Alert(" Enum No. :" + m.No + "  Has been :" + m.Name + " Occupancy ");
            return;
        }
        m.Name = name;
        if (string.IsNullOrEmpty(name))
        {
            this.Alert(" Enum name can not be empty ");
            return;
        }

        SysEnums ses = new SysEnums();
        for (int i = 0; i < 20; i++)
        {
            TextBox tb = this.UCSys1.GetTextBoxByID("TB_" + i);
            if (tb == null)
                continue;
            if (string.IsNullOrEmpty(tb.Text))
                continue;

            SysEnum se = new SysEnum();
            se.IntKey = i;
            se.Lab = tb.Text.Trim();
            se.Lang = BP.Web.WebUser.SysLang;
            se.EnumKey = m.No;
            se.MyPK = se.EnumKey + "_" + se.Lang + "_" + se.IntKey;
            ses.AddEntity(se);
        }

        if (ses.Count == 0)
        {
            this.Alert(" Enumerate the project can not be empty .");
            return;
        }

        string lab = "";
        foreach (SysEnum se in ses)
        {
            se.Save();
            lab += "@" + se.IntKey + "=" + se.Lab;
        }

        m.Lang = BP.Web.WebUser.SysLang;
        m.CfgVal = lab;
        m.Insert();
        this.Response.Redirect("EnumList.aspx?RefNo=" + m.No, true);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Title = " Editing enumerated values ";
        if (this.DoType == "Del")
        {
            MapAttrs attrs = new MapAttrs();
            attrs.Retrieve(MapAttrAttr.UIBindKey, this.RefNo);
            if (attrs.Count != 0)
            {
                this.UCSys1.AddFieldSet("<a href='EnumList.aspx' ><img src='./../../Img/Btn/Home.gif' border=0/> Back to list </a> -  Delete Confirmation ");
                this.UCSys1.Add(" This enumeration value has been referenced by other fields , You can not delete it .");
                this.UCSys1.AddH2("<a href='EnumList.aspx' > Back to list </a>");
                this.UCSys1.AddFieldSetEnd();
                return;
            }

            this.UCSys1.AddFieldSet("<a href='EnumList.aspx' ><img src='./../../Img/Btn/Home.gif' border=0/> Back to list </a> -  Delete Confirmation ");
            SysEnumMain m = new SysEnumMain(this.RefNo);
            this.UCSys1.AddH2("<a href='EnumList.aspx?RefNo=" + this.RefNo + "&DoType=DelReal' > Delete :" + m.Name + "  Confirm .</a>");
            this.UCSys1.AddFieldSetEnd();
            return;
        }

        if (this.DoType == "DelReal")
        {
            SysEnumMain m = new SysEnumMain();
            m.No = this.RefNo;
            m.Delete();
            SysEnums ses = new SysEnums();
            ses.Delete(SysEnumAttr.EnumKey, this.RefNo);
            this.Response.Redirect("EnumList.aspx", true);
            return;
        }

        if (this.DoType == "New")
        {
            this.BindNew();
            return;
        }

        if (this.RefNo != null)
        {
            this.BindRefNo();
            return;
        }

        this.UCSys1.AddTable();
        this.UCSys1.AddCaptionLeftTX("<img src='./../../Img/Btn/Home.gif' border=0/> List  - <a href='EnumList.aspx?DoType=New' ><img border=0 src='./../../Img/Btn/New.gif' > New </a>");
        this.UCSys1.AddTR();
        this.UCSys1.AddTDTitle("No.");
        this.UCSys1.AddTDTitle(" Serial number ");
        this.UCSys1.AddTDTitle(" Name ");
        this.UCSys1.AddTDTitle(" Information ");
        this.UCSys1.AddTDTitle(" Operating ");
        this.UCSys1.AddTREnd();

        SysEnumMains sems = new SysEnumMains();
        sems.RetrieveAll();
        int i = 0;
        foreach (SysEnumMain se in sems)
        {
            i++;
            this.UCSys1.AddTR();
            this.UCSys1.AddTDIdx(i);
            this.UCSys1.AddTD(se.No);
            this.UCSys1.AddTDA("EnumList.aspx?RefNo=" + se.No, se.Name);
            this.UCSys1.AddTD(se.CfgVal);
            this.UCSys1.AddTDA("EnumList.aspx?RefNo=" + se.No + "&DoType=Del", "<img src='./../../Img/Btn/Delete.gif' border=0 /> Delete ");
            this.UCSys1.AddTREnd();
        }
        this.UCSys1.AddTableEnd();
    }
}