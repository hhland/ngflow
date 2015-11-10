using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.Port;
using BP.Web;

namespace CCFlow.SDKFlowDemo
{
    public partial class DoUrl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region  Check the variable is complete .
            string fk_flow = this.Request.QueryString["FK_Flow"];
            string fk_nodeStr = this.Request.QueryString["FK_Node"];
            string workidStr = this.Request.QueryString["WorkID"];
            if (string.IsNullOrEmpty(fk_flow) || string.IsNullOrEmpty(fk_nodeStr)
                || string.IsNullOrEmpty(workidStr))
            {
                this.Response.Write("ERR:  Parameters incomplete , The original url是:"+this.Request.RawUrl);
                return;
            }
            #endregion  Check the variable is complete .

            //  Be careful not to visit session  Variable , Do visit cookies.
            int nodeID = int.Parse(fk_nodeStr);
            Int64 workid = Int64.Parse(workidStr);
            string sql = "SELECT * FROM Sys_M2M WHERE EnOID='"+workid+"' AND FK_MapData='ND"+fk_nodeStr+"'";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            string doc = dt.Rows[0]["Doc"].ToString();
            string[] strs = doc.Split(',');
            foreach (string str in strs)
            {
                if (string.IsNullOrEmpty(str))
                    continue;
                //  Sponsored sub-processes .
                Hashtable ht = new Hashtable();
                BP.WF.Dev2Interface.Node_StartWork("101", ht,
                    null, 0, null, workid, "102");
            }
            this.Response.Redirect("@ Sub-processes have been successfully launched .");
        }
    }
}