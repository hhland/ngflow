using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Sys;
using BP.En;
using BP.DA;

public partial class CCFlow_Comm_Sys_SFTableList : BP.Web.WebPageAdmin
{
    public void BindIt()
    {
        SFTable sem = new SFTable(this.RefNo);

        this.UCSys1.AddTable("width=500px class=Table");
        this.UCSys1.AddCaptionLeft("<a href=SFTableList.aspx ><img src='./../../Img/Btn/Home.gif' border=0> List </a> -<a href='SFTableList.aspx?DoType=New' ><img src='./../../Img/Btn/New.gif' border=0/> New </a>- <img src='./../../Img/Btn/Edit.gif' border /> Editor :" + sem.No + " " + sem.Name);
        this.UCSys1.AddTR();
        this.UCSys1.AddTDTitle(" Project ");
        this.UCSys1.AddTDTitle(" Collection ");
        this.UCSys1.AddTDTitle(" Explanation ");
        this.UCSys1.AddTREnd();

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

        this.UCSys1.AddTR();
        Button btn = new Button();
        btn.ID = "Btn_Save";
        btn.CssClass = "Btn";
        btn.Text = " Save  ";
        btn.Click += new EventHandler(btn_Click);
        this.UCSys1.AddTD("colspan=2", btn);
        if (this.RefNo.Contains(".") == false)
            this.UCSys1.AddTD("<a href='./../../MapDef/SFTableEditData.aspx?RefNo=" + this.RefNo + "' > Edit Data </a>");
        else
            this.UCSys1.AddTD("<a href='./../Ens.aspx?EnsName=" + this.RefNo + "' > Edit Data </a>");

        this.UCSys1.AddTREnd();
        this.UCSys1.AddTableEnd();
    }
    public void BindNew()
    {
        this.UCSys1.AddTable("width=500px class=Table");
        this.UCSys1.AddCaptionLeft("<a href=SFTableList.aspx ><img src='./../../Img/Btn/Home.gif' border=0 /> List </a> - <img src='./../../Img/Btn/New.gif' /> New coding table ");

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

        //this.UCSys1.AddTRSum();
        //this.UCSys1.AddTD(" Type ");
        //DropDownList ddl =new DropDownList();
        //ddl.ID = "DDL_Type";
        //ddl.Items.Add(new ListItem(" Physical table ", "0"));
        //ddl.Items.Add(new ListItem(" View ", "0"));
        //this.UCSys1.AddTD(ddl);
        //this.UCSys1.AddTD("");
        //this.UCSys1.AddTREnd();

        //this.UCSys1.AddTRSum();
        //this.UCSys1.AddTD("colspan=3"," View SQL:( Type is valid for a view , Query must contain No,Name Two .)");
        //this.UCSys1.AddTREnd();

        //this.UCSys1.AddTRSum();
        //tb = new TextBox();
        //tb.ID = "TB_SQL";
        //tb.TextMode = TextBoxMode.MultiLine;
        //tb.Columns = 70;
        //tb.Rows = 10;
        //this.UCSys1.AddTD("colspan=3",tb);
        //this.UCSys1.AddTREnd();


        this.UCSys1.AddTR();
        Button btn = new Button();
        btn.ID = "Btn_Save";
        btn.Text = "  Save  ";
        btn.CssClass = "Btn";
        btn.Click += new EventHandler(btn_New_Click);
        this.UCSys1.AddTD("colspan=3", btn);
        this.UCSys1.AddTREnd();
        this.UCSys1.AddTableEnd();
    }

    void btn_Click(object sender, EventArgs e)
    {
        string no = this.UCSys1.GetTextBoxByID("TB_No").Text;
        string name = this.UCSys1.GetTextBoxByID("TB_Name").Text;
        SFTable m = new SFTable();
        m.No = no;
        m.RetrieveFromDBSources();
        m.Name = name;
        if (string.IsNullOrEmpty(name))
        {
            this.Alert(" Coding table name can not be empty ");
            return;
        }
      //  m.HisSFTableType = SFTableType.SFTable;
        m.Save();
        this.Response.Redirect("SFTableList.aspx?RefNo=" + m.No, true);
         
    }

    void btn_New_Click(object sender, EventArgs e)
    {
        string no = this.UCSys1.GetTextBoxByID("TB_No").Text;
        string name = this.UCSys1.GetTextBoxByID("TB_Name").Text;
        SFTable m = new SFTable();
        m.No = no;
        if (m.RetrieveFromDBSources() == 1)
        {
            this.Alert(" Coding table number :" + m.No + "  Has been :" + m.Name + " Occupancy ");
            return;
        }
        m.Name = name;
        if (string.IsNullOrEmpty(name))
        {
            this.Alert(" Coding table name can not be empty ");
            return;
        }
        //  m.HisSFTableType = SFTableType.SFTable;
        m.Insert();
        this.Response.Redirect("SFTableList.aspx?RefNo=" + m.No, true);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Title = " Coding Table Editor ";
        if (this.DoType == "Del")
        {
            MapAttrs attrs = new MapAttrs();
            attrs.Retrieve(MapAttrAttr.UIBindKey, this.RefNo);
            if (attrs.Count != 0)
            {
                this.UCSys1.AddFieldSet("<a href='SFTableList.aspx' ><img src='./../../Img/Btn/Home.gif' border=0/> Back to list </a> -  Delete Confirmation ");
                this.UCSys1.Add(" This table has been referenced encoding other fields , You can not delete it .");
                this.UCSys1.AddH2("<a href='SFTableList.aspx' > Back to list </a>");
                this.UCSys1.AddFieldSetEnd();
                return;
            }

            this.UCSys1.AddFieldSet("<a href='SFTableList.aspx' ><img src='./../../Img/Btn/Home.gif' border=0/> Back to list </a> -  Delete Confirmation ");
            SFTable m = new SFTable(this.RefNo);
            this.UCSys1.AddH2("<a href='SFTableList.aspx?RefNo=" + this.RefNo + "&DoType=DelReal' > Delete :" + m.Name + "  Confirm .</a>");
            this.UCSys1.AddFieldSetEnd();
            return;
        }

        if (this.DoType == "DelReal")
        {
            SFTable m = new SFTable();
            m.No = this.RefNo;
            m.Delete();
            SFTables ses = new SFTables();
           // ses.Delete(SFTableAttr.SFTableKey, this.RefNo);
            this.Response.Redirect("SFTableList.aspx", true);
            return;
        }

        if (this.DoType == "New")
        {
            this.BindNew();
            return;
        }

        if (this.RefNo != null)
        {
            this.BindIt();
            return;
        }

        this.UCSys1.AddTable("class=Table width=500px");
        this.UCSys1.AddCaption("<img src='./../../Img/Btn/Home.gif' border=0/> List  - <a href='SFTableList.aspx?DoType=New' ><img border=0 src='./../../Img/Btn/New.gif' > New </a>");
        this.UCSys1.AddTR();
        this.UCSys1.AddTDTitle("No.");
        this.UCSys1.AddTDTitle(" Serial number ");
        this.UCSys1.AddTDTitle(" Name ");
        //this.UCSys1.AddTDTitle(" Type ");
        this.UCSys1.AddTDTitle(" Description ");
        this.UCSys1.AddTDTitle(" Operating ");
        this.UCSys1.AddTREnd();

        SFTables sems = new SFTables();
        sems.RetrieveAll();
        int i = 0;
        foreach (SFTable se in sems)
        {
            i++;
            this.UCSys1.AddTR();
            this.UCSys1.AddTDIdx(i);
            this.UCSys1.AddTD(se.No);
            this.UCSys1.AddTDA("SFTableList.aspx?RefNo=" + se.No, se.Name);
          //  this.UCSys1.AddTD(se.SFTableTypeT);
            this.UCSys1.AddTD(se.TableDesc);

            this.UCSys1.AddTDA("SFTableList.aspx?RefNo=" + se.No + "&DoType=Del", "<img src='./../../Img/Btn/Delete.gif' border=0 /> Delete ");

            //switch (se.HisSFTableType)
            //{
            //    case SFTableType.SFTable:
            //        this.UCSys1.AddTDA("SFTableList.aspx?RefNo=" + se.No + "&DoType=Del", "<img src='./../../Img/Btn/Delete.gif' border=0 /> Delete ");
            //        break;
            //    case SFTableType.ClsLab:
            //    case SFTableType.SysTable:
            //    default:
            //        this.UCSys1.AddTD();
            //        break;
            //}
            this.UCSys1.AddTREnd();
        }
        this.UCSys1.AddTableEnd();
    }
}