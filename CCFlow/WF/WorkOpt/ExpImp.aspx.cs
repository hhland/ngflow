using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.Web;
using BP.Sys;

namespace CCFlow.WF.WorkOpt
{
public partial class WF_Opt_ExpImp : BP.Web.WebPage
{
    public Int64 WorkID
    {
        get
        {
            return Int64.Parse(this.Request.QueryString["WorkID"]);
        }
    }
    public int FK_Node
    {
        get
        {
            return int.Parse(this.Request.QueryString["FK_Node"]);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        switch (this.DoType)
        {
            case "Word":
                WorkNode wnP1 = new WorkNode(this.WorkID, this.FK_Node);
                wnP1.DoPrint();
                return;
            case "Excel":
                WorkNode wnP2 = new WorkNode(this.WorkID, this.FK_Node);
                wnP2.DoPrint();
                break;
            case "PDF":
                WorkNode wnP3 = new WorkNode(this.WorkID, this.FK_Node);
                wnP3.DoPrint();
                break;
            default:
                break;
        }

        this.Pub2.AddFieldSet(" Export Forms ");
        this.Pub2.AddUL();
        this.Pub2.AddLi("<a href='ExpImp.aspx?DoType=Word&WorkID=" + this.WorkID + "&FK_Node=" + this.FK_Node + "' ><img src='/WF/Img/FileType/doc.gif' border=0/> Export to Word</a>");
        this.Pub2.AddLi("<a href='ExpImp.aspx?DoType=Excel&WorkID=" + this.WorkID + "&FK_Node=" + this.FK_Node + "' ><img src='/WF/Img/FileType/xls.gif' border=0/> Export to Excel</a>");
        this.Pub2.AddLi("<a href='ExpImp.aspx?DoType=PDF&WorkID=" + this.WorkID + "&FK_Node=" + this.FK_Node + "' ><img src='/WF/Img/FileType/pdf.gif' border=0/> Export to PDF</a>");
        this.Pub2.AddULEnd();
        this.Pub2.AddFieldSetEnd();

        MapDtls dtls = new MapDtls("ND" + this.FK_Node);
        Node nd = new Node(this.FK_Node);
        if (dtls.Count == 0)
        {
            this.Pub2.AddFieldSet(" Import from table ");
            this.Pub2.Add(" Unable to import the table ");
            this.Pub2.AddFieldSetEnd();
            return;
        }

        foreach (MapDtl dtl in dtls)
        {
            this.Pub2.AddFieldSet(" Importing :" + dtl.Name);
            this.Pub2.Add(" Format data files :");

            FileUpload fu = new FileUpload();
            fu.ID = "F" + dtl.No;
            this.Pub2.Add(fu);
            this.Pub2.Add("<br>");

            Button btn = new Button();
            btn.Text = " Importing ";
            btn.CssClass = "Btn";
            btn.ID = "Btn_" + dtl.No;
            btn.Click += new EventHandler(btn_Click);
            this.Pub2.Add(btn);
            btn = new Button();
            btn.CssClass = "Btn";
            btn.Text = " Download data template ";
            btn.ID = "Btn_Exp" + dtl.No;
            btn.Click += new EventHandler(btn_Exp_Click);
            this.Pub2.Add(btn);
            this.Pub2.AddFieldSetEnd();
        }
    }
    void btn_Exp_Click(object sender, EventArgs e)
    {
        Button btn = sender as Button;
        string id = btn.ID.Replace("Btn_Exp", "");

        MapDtl dtl = new MapDtl(id);
        GEDtls dtls = new GEDtls(id);
        this.ExportDGToExcelV2(dtls, dtl.Name + ".xls");
        return;
    }
    void btn_Click(object sender, EventArgs e)
    {
        Button btn = sender as Button;
        string id = btn.ID.Replace("Btn_", "");
    }
}
}