using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SDKFlows_Do : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        switch (this.Request.QueryString["DoType"])
        {
            case "DelInfo":
                this.Response.Write(" Successful implementation .");
                break;
            default:
                break;
        }
    }

    //public void TurnTo2Node(string fk_flow, int FK_NodeSheetfJump,Int64 fromOID, Int64 fromFID)
    //{
    //    // Initiate new processes about to jump directly to 103 Node up .
    //    //  Watch out : 103  Personnel nodes accepted rules , Consistent with the previous step of the operator .
    //    BP.WF.Dev2Interface.Node_StartWork("001", null, 103);
    //}
}