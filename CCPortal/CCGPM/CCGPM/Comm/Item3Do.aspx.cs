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

public partial class Comm_Item3Do : System.Web.UI.Page
{
    public string EnName
    {
        get
        {
            return Request.QueryString["EnName"];
        }
    }
    public string RefPK
    {
        get
        {
            return Request.QueryString["RefPK"];
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        Entity en = ClassFactory.GetEn(this.EnName);
        try
        {
            en.PKVal = this.RefPK;
            en.RetrieveFromDBSources();
            en.Delete();
            // this.Ucsys1.AddMsgOfInfo("提示：", "已经成功的删除[" + en.EnDesc + "]。");
            BP.Sys.PubClass.WinClose();
        }
        catch (Exception ex)
        {
            this.Ucsys1.AddMsgOfWarning("提示：", "删除[" + en.EnDesc + "]，期间出现如下错误： @" + ex.Message);
        }
    }

}
