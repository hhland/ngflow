using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCFlow.SDKFlowDemo
{
    public partial class FrmEventND8601 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region  Check the variable is complete .
                string fk_flow = this.Request.QueryString["FK_Flow"];
                string fk_nodeStr = this.Request.QueryString["FK_Node"];
                string OID = this.Request.QueryString["OID"];
                if (string.IsNullOrEmpty(fk_flow) || string.IsNullOrEmpty(fk_nodeStr)
                    || string.IsNullOrEmpty(OID))
                {
                    this.Response.Write("ERR:  Parameters incomplete , The original url是:" + this.Request.RawUrl);
                    return;
                }
                #endregion  Check the variable is complete .

                string workidsStrs = this.Request.QueryString["WorkIDs"];
                if (string.IsNullOrEmpty(workidsStrs))
                {
                    /* There may be a view node form information , Fill in the information, do not execute down the .*/
                    return;
                }

                string[] workids =workidsStrs.Split(',');

                //  Some information has been cleared .
                BP.DA.DBAccess.RunSQL("DELETE ND8601Dtl1 WHERE RefPK='" + OID + "'");

                foreach (string id in workids)
                {
                    if (string.IsNullOrEmpty(id))
                        continue;

                    string sql = "SELECT * FROM ND82Rpt WHERE OID=" + id;
                    DataTable dt =BP.DA.DBAccess.RunSQLReturnTable(sql);
                    string oid = dt.Rows[0]["OID"].ToString();
                    string Title = dt.Rows[0]["Title"].ToString();
                    string XMBH = dt.Rows[0]["XMBH"].ToString();
                    string XMMC = dt.Rows[0]["XMMC"].ToString();
                    string XMDZ = dt.Rows[0]["XMDZ"].ToString();
                    string XMJE = dt.Rows[0]["XMJE"].ToString();


                    // Need to remove this before inserting oid, In order to prevent pk Repeat .
                    BP.DA.DBAccess.RunSQL("DELETE ND8601Dtl1 WHERE OID='" + id + "'");


                    // The data is inserted inside , And handle threads WorkID Master key , Current work ID做RefPK.
                    string insertSQL = "";
                    insertSQL = "INSERT INTO ND8601Dtl1(OID,RefPK,XMBH,XMMC,XMDZ,XMJE) VALUES(" + id + ",'" + OID + "','" + XMBH + "'";
                    insertSQL += ",'" + XMMC + "'";
                    insertSQL += ",'" + XMDZ + "'";
                    insertSQL += "," + XMJE + ")";

                    BP.DA.DBAccess.RunSQL(insertSQL);
                }

                this.Response.Write(" Load success ,");
            }
            catch(Exception ex)
            {
                this.Response.Write("err:  Error loading form execution events :" +ex.Message+" ,URL:"+ this.Request.RawUrl);
            }
        }
    }
}