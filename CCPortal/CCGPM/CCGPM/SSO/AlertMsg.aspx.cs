using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.GPM;

public partial class SSO_AlertMsg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
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
}