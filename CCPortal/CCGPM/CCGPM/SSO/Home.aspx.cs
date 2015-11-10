using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.GPM;

public partial class SSO_Home : System.Web.UI.Page
{

    public string FK_Bar
    {
        get
        {
            return this.Request.QueryString["FK_Bar"];
        }
    }

    public InfoPushs InfoPushs
    {
        get
        {
            BP.GPM.InfoPushs pls = new InfoPushs();
            pls.RetrieveAll();

            return pls;
        }
    }
    public int GetNum(InfoPush pa)
    {
        int num = BP.DA.DBAccess.RunSQLReturnValInt(pa.GetSQL);

        return num;
    }
    private BarEmps BarEmps
    {
        get
        {
            BarEmps bes = new BarEmps();
            bes.RetrieveByAttr("FK_Emp", BP.Web.WebUser.No);
            return bes;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
}