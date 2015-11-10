using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class CCFlow_Comm_Sys_Cfg : BP.Web.MasterPage
{
    private string _pageID = null;
    public new  string PageID
    {
        get
        {
            if (_pageID == null)
            {
                string url = System.Web.HttpContext.Current.Request.RawUrl;
                int i = url.LastIndexOf("/") + 1;
                int i2 = url.IndexOf(".aspx") - 6;
                try
                {
                    url = url.Substring(i);
                    _pageID = url.Substring(0, url.IndexOf(".aspx"));

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + url + " i=" + i + " i2=" + i2);
                }
            }
            return _pageID;
        }
    }
    public new string EnsName
    {
        get
        {
            return this.Request.QueryString["EnsName"];
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        BP.Web.Comm.CfgMenus ens = new BP.Web.Comm.CfgMenus();
        ens.RetrieveAll();

        this.Pub1.MenuSelfBegin();
        foreach (BP.Web.Comm.CfgMenu en in ens)
        {
            if (en.No == "")
                this.Pub1.MenuSelfItemS(en.Url + "&EnsName=" + this.EnsName, en.Name, en.Target);
            else
                this.Pub1.MenuSelfItem(en.Url + "&EnsName=" + this.EnsName, en.Name, en.Target);
        }
        this.Pub1.MenuSelfEnd();
    }
}
