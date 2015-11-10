using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BP.DA;
using BP.En;
using BP.Web;
using BP.Web.Controls;
using BP.Sys;
using BP;

public partial class CCFlow_Comm_HelperOfTBNum : BP.Web.WebPage
{
    //private DropDownList DropDownList11 = null;
    //private System.Web.UI.WebControls.TextBox  TextBox1= null;

    public new string EnsName
    {
        get
        {
            return this.Request.QueryString["EnsName"];
        }
    }
    public void BindToolBar()
    {
        BP.Port.Depts Depts = new BP.Port.Depts();
        Depts.RetrieveAll();


        BP.DA.Paras ps = new Paras();
        ps.SQL = "SELECT NO ,NAME FROM TAX_Dept WHERE ( GRADE>=3 ) AND NO LIKE '%'||:Dept||'%' order by no ";
        ps.Add("Dept",WebUser.FK_Dept);

        System.Data.DataTable dt = DBAccess.RunSQLReturnTable(ps);
        foreach (DataRow dr in dt.Rows)
        {
            ListItem li = new ListItem();
            li.Value = dr["No"].ToString();
            li.Text = dr["No"]+dr["Name"].ToString();
            this.DropDownList1.Items.Add(li);
        }

        //ToolbarDDL ddl = new ToolbarDDL("DDL_Dept",Depts,"No","Name",false);
        // ddl.BindEntities(Depts, false, AddAllLocation.None);
        //this.BPToolBar1.AddDDL(ddl, false);
        //this.BPToolBar1.AddBtn("Btn_Search", " Keyword ");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.IsPostBack == false)
        {
            this.BindToolBar();

            //Entities ens = ClassFactory.GetEns(this.EnsName);
            //Entity en = ens.GetNewEntity;
            // this.BPToolBar1.InitByMapV2(en.EnMap, 1);
        }
        this.SetDGData();
    }

    
    public void SetDGData()
    {

        if (this.IsPostBack == false)
        {
            this.Ucsys1.AddBR();
            this.Ucsys1.AddMsgOfInfo("<img src='../Img/Btn/help.gif' border=0 /> Using Help :", "&nbsp;&nbsp;<BR><BR>1, Please enter a keyword to find , Keyword fuzzy match any field .<BR><BR>2, Organize the query .<BR><BR>3, Press the OK button , Find complete .<BR><BR>4, Double click on your choice of recording complete data selection .<BR><BR>5, In order to improve the efficiency of , The system displays ago 100 records.<BR><BR>");
            return  ;
        }


        this.UcTitle.Clear();
        this.Ucsys1.Clear();
        this.Ucsys2.Clear();

        string key = this.TextBox1.Text.Trim();

        if (key == "" || key == null)
        {
            this.Ucsys1.AddMsgOfWarning(" Please enter a keyword ",null);
            return;
        }

        string FK_Dept = this.DropDownList1.SelectedValue;

        this.UcTitle.Add(this.GenerLabelStr(" Prompt : Double-click on your choice of records , Complete the selection operation ."));

        this.Ucsys1.AddTable();
        this.Ucsys1.AddTR();
        this.Ucsys1.AddTDTitle(" No. ");
        this.Ucsys1.AddTDTitle(" Choose ");
        this.Ucsys1.AddTDTitle(" Address ");
        this.Ucsys1.AddTDTitle(" Legal ");
        this.Ucsys1.AddTDTitle(" Phone ");
        this.Ucsys1.AddTREnd();

        string like_Dept = "'%'||:FK_Dept||'%'";
        string like = "'%'||:key||'%'";
        string sql = "SELECT No,Name,Addr, FR, Tel FROM DS_Taxpayer WHERE (FK_Dept LIKE " + like_Dept + ") AND ( No LIKE " + like + "  OR Name LIKE " + like + " OR FR LIKE " + like + ") AND rownum <=100 ";

        BP.DA.Paras ps = new Paras();
        ps.SQL = sql;
        ps.Add("FK_Dept", FK_Dept);
        ps.Add("key", key);
        System.Data.DataTable dt = DBAccess.RunSQLReturnTable(ps);

        string keyval = this.TextBox1.Text;
        string keyvalRed = "<font color=red>"+this.TextBox1.Text+"</font>";



        int i = 0;
        foreach (DataRow dr in dt.Rows)
        {
             
            this.Ucsys1.AddTRTX();
            this.Ucsys1.AddTDIdx(i++);

            BP.Web.Controls.RadioBtn rb = new RadioBtn();
            rb.ID = "RB_" + dr["No"].ToString() ;
            string text = dr["No"].ToString() + dr["Name"].ToString();
            rb.Text = text.Replace(keyval, keyvalRed);
            rb.GroupName = "s";

            string clientscript = "window.returnValue = '" + dr["No"].ToString() + "';window.close();";
            // rb.Attributes["ondblclick"] = clientscript;
            rb.Attributes["ondblclick"] = clientscript;
            this.Ucsys1.AddTD("TD", rb);

            this.Ucsys1.AddTD(dr["Addr"].ToString().Replace(keyval, keyvalRed));
            this.Ucsys1.AddTD(dr["FR"].ToString().Replace(keyval, keyvalRed));
            this.Ucsys1.AddTD(dr["Tel"].ToString().Replace(keyval, keyvalRed));
            this.Ucsys1.AddTREnd();
        }
        this.Ucsys1.AddTableEnd();

        //QueryObject qo = new QueryObject(ens);
        //qo = this.BPToolBar1.GetnQueryObject(ens, en);
        //string url = this.Request.RawUrl;
        //if (url.IndexOf("PageIdx") != -1)
        //    url = url.Substring(0, url.IndexOf("PageIdx") - 1);

        //this.Ucsys2.BindPageIdx(qo.GetCOUNT(), SystemConfig.PageSize, pageIdx, url);

        //qo.DoQuery(en.PK, SystemConfig.PageSize, pageIdx);


        //this.Ucsys1.AddTable();
        //this.Ucsys1.AddTR();
        //this.Ucsys1.AddTDTitle(" No. ");
        //this.Ucsys1.AddTDTitle(" Choose ");
        //this.Ucsys1.AddTDTitle(" Address ");
        //this.Ucsys1.AddTDTitle(" Legal ");
        //this.Ucsys1.AddTDTitle(" Phone ");
        //this.Ucsys1.AddTREnd();

        //int i = 1;

        //foreach (Entity en1 in ens)
        //{
        //    this.Ucsys1.AddTRTX();
        //    this.Ucsys1.AddTDIdx(i++);

        //    BP.Web.Controls.RadioBtn rb = new RadioBtn();
        //    rb.ID = "RB_" + en1.GetValStrByKey("No");
        //    rb.Text = en1.GetValStrByKey("No") + en1.GetValStrByKey("Name");
        //    rb.GroupName = "s";

        //    string clientscript = "window.returnValue = '" + en1.GetValStringByKey("No") + "';window.close();";
        //    // rb.Attributes["ondblclick"] = clientscript;
        //    rb.Attributes["ondblclick"] = clientscript;
        //    this.Ucsys1.AddTD("TD", rb);


        //    this.Ucsys1.AddTD(en1.GetValStrByKey("Addr"));
        //    this.Ucsys1.AddTD(en1.GetValStrByKey("FR"));
        //    this.Ucsys1.AddTD(en1.GetValStrByKey("Tel"));
        //    this.Ucsys1.AddTREnd();
        //}


        //this.Ucsys1.AddTableEnd();


        //this.Ucsys2.BindPageIdx(qo.GetCOUNT(), SystemConfig.PageSize, pageIdx, url);
        //qo.DoQuery(en.PK, SystemConfig.PageSize, pageIdx);
        //this.BPToolBar1.SaveSearchState(this.EnsName);
        //return ens;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        this.SetDGData();
    }
}
