using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.En;
using BP.Web;
using BP.DA;
using BP.Sys;
using BP;

public partial class WF_MapDef_Rpt_Home : BP.Web.WebPage
{
    #region  Property .
    public string FK_Flow
    {
        get
        {
            string s= this.Request.QueryString["FK_Flow"];
            if (s == null)
                s ="007";
            return s;
        }
    }
    public int Idx
    {
        get
        {
            string s = this.Request.QueryString["Idx"];
            if (s == null)
                s = "0";
            return int.Parse(s);
        }
    }
    public string FK_MapAttr
    {
        get
        {
            string s = this.Request.QueryString["FK_MapAttr"];
            return s;
        }
    }
    public string FK_MapData
    {
        get
        {
            string s = this.Request.QueryString["FK_MapData"];
            if (s == null)
                s = "ND"+int.Parse(this.FK_Flow)+"Rpt";
            return s;
        }
    }
    public string RptNo
    {
        get
        {
            string s = this.Request.QueryString["RptNo"];
            if (s == null)
                s = "ND"+int.Parse(this.FK_Flow)+"MyRpt";
            return s;
        }
    }
    #endregion  Property .

    protected void Page_Load(object sender, EventArgs e)
    {
        var fl = new Flow(FK_Flow);
        Title = " Report Design :" + fl.Name;

        //  Clear cached data .
        Cash.EnsData_Cash.Clear();
    }

    public void BindHome()
    {
        //this.Pub2.AddH2(" Welcome ccflow Report Designer .");
        //this.Pub2.AddHR();

        //this.Pub2.AddFieldSet(" What is the process data ?");
        //this.Pub2.AddUL();
        //this.Pub2.AddLi(" Process data query ");
        //this.Pub2.AddLi(" Statistical analysis of process data ");
        //this.Pub2.AddLi(" The comparative analysis process ");
        //this.Pub2.AddULEnd();
        //this.Pub2.AddFieldSetEnd();

        //this.Pub2.AddFieldSet(" Designer reading ?");
        //string info = "";
        //info += "<b> About Process Data Sheet :</b><br>";
        //info += " Process data is a table of all nodes on the form fields collection consisting of a process , Is NDxxxRpt Named , Process initiated after the data to be added to this list .";
        //info += "<br><b> How to access control :</b><br>";
        //info += " Data permissions based query and analysis department controlled conditions , An operator can query the data in those sectors is maintained in the system administrator , Stored in WF_DeptFlowSearch Table .";
        //this.Pub2.Add(info);
        //this.Pub2.AddULEnd();
        //this.Pub2.AddFieldSetEnd();
    }
}