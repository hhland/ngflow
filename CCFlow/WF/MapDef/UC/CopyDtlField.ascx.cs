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
using BP.WF;
using BP.En;
using BP.Sys;


public partial class WF_MapDef_UC_CopyDtlField :BP.Web.UC.UCBase3
{
    public string Dtl
    {
        get
        {
            return this.Request.QueryString["Dtl"];
        }
    }
    public void BindAttrs()
    {
        MapDtl dtlFrom = new MapDtl(this.Dtl);
        MapDtl dtl = new MapDtl(this.MyPK);

        MapAttrs attrsFrom = new MapAttrs(this.Dtl);
        MapAttrs attrs = new MapAttrs(dtl.No);

        this.AddTable();
        this.AddCaptionLeft("<A href='CopyDtlField.aspx?MyPK=" + this.MyPK + "'> Return </A> -  Select the field you want to copy ");
        this.AddTR();
        this.AddTDTitle(" Name ");
        this.AddTDTitle(" Field ");
        this.AddTDTitle(" Type ");
        this.AddTDTitle(" Defaults ");
        this.AddTREnd();

        bool isHave = false;

        foreach (MapAttr attr in attrsFrom)
        {
            switch (attr.KeyOfEn)
            {
                case "OID":
                case "FID":
                case "WorkID":
                case "Rec":
                case "RDT":
                    continue;
                default:
                    break;
            }


            this.AddTR();
            CheckBox cb = new CheckBox();
            cb.ID = "CB_" + attr.MyPK;
            cb.Text = attr.Name;
            if (attrs.Contains(MapAttrAttr.KeyOfEn, attr.KeyOfEn))
                cb.Enabled = false;

            this.AddTD("nowarp=true", cb);
            this.AddTD(attr.KeyOfEn);
            this.AddTD(attr.MyDataTypeStr);
            this.AddTD(attr.DefValReal);
            isHave = true;
            this.AddTREnd();
        }

        this.AddTRSum();
        this.AddTD();
        Button btn = new Button();
        btn.CssClass = "Btn";
        btn.ID = "Btn_Copy";
        btn.Text =  " Copy ";
        btn.Click += new EventHandler(btn_Click);
        btn.Enabled = isHave;

        this.AddTD("colspan=3", btn);
        this.AddTREnd();
        this.AddTableEnd();
    }

    void btn_Click(object sender, EventArgs e)
    {
        MapAttrs attrsFrom = new MapAttrs(this.Dtl);
        MapAttrs attrs = new MapAttrs(this.MyPK);
        foreach (MapAttr attr in attrsFrom)
        {
            switch (attr.KeyOfEn)
            {
                case "OID":
                case "FID":
                case "WorkID":
                case "Rec":
                case "RDT":
                case "RefPK":
                    continue;
                default:
                    break;
            }

            if (attrs.Contains(MapAttrAttr.KeyOfEn, attr.KeyOfEn))
                continue;

            if (this.GetCBByID("CB_" + attr.MyPK).Checked == false)
                continue;

            MapAttr en = new MapAttr();
            en.Copy(attr);
            en.FK_MapData = this.MyPK;
            en.GroupID = 0;
            //en.IDX = 0;
            en.Insert();
        }
        this.WinCloseWithMsg(" Replicate success , You can adjust the order from the table .");
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.Title = "Copy Node Dtl Fields  Esc to exit.";
        if (this.Dtl != null)
        {
            this.BindAttrs();
            return;
        }

        MapDtl dtl = new MapDtl(this.MyPK);
        //string sql = "SELECT DISTINCT PTable, No, Name From Sys_MapDtl WHERE  No <> '"+this.MyPK+"'";
        string sql = "SELECT b.Name as NodeName, a.No AS DtlNo, a.Name as DtlName";
        sql += " FROM Sys_MapDtl a , Sys_MapData b  ";
        sql += " WHERE A.FK_MapData=b.No AND B.No LIKE '"+this.MyPK.Substring(0,4)+"%' AND B.No<>'"+this.MyPK+"'";


        DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
        if (dt.Rows.Count == 0)
        {
            this.WinCloseWithMsg(" Node data is not replicable .");
            return;
        }

        if (dt.Rows.Count == 1)
        {
            this.Response.Redirect("CopyDtlField.aspx?MyPK=" + this.MyPK + "&Dtl=" + dt.Rows[0]["DtlNo"].ToString(), true);
            return;
        }

        this.AddFieldSet(" Detail on this process ");
        this.AddUL();
        foreach (DataRow dr in dt.Rows)
        {
            
            this.AddLi("CopyDtlField.aspx?MyPK=" + this.MyPK + "&Dtl=" + dr["DtlNo"].ToString(), " Node name :" + dr["NodeName"].ToString() + "  Table Name :" + dr["DtlName"].ToString());
        }
        this.AddULEnd();
        this.AddFieldSetEnd();


        //this.AddFieldSet(" Detailed data on other processes ");
        //this.AddUL();
        //foreach (DataRow dr in dt.Rows)
        //{
        //    this.AddLi("CopyDtlField.aspx?MyPK=" + this.MyPK + "&Dtl=" + dr["No"].ToString(), dr["Name"].ToString());
        //}
        //this.AddULEnd();
        //this.AddFieldSetEnd();


    }
}
