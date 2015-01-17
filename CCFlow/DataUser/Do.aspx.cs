using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.DA;
using BP.En;
using BP.WF;
using BP.Web;
public partial class DataUser_Do : System.Web.UI.Page
{
    #region  Common variable .
    public string DoType
    {
        get
        {
            return this.Request.QueryString["DoType"];
        }
    }
    public string FK_Node
    {
        get
        {
            return this.Request.QueryString["FK_Node"];
        }
    }
    public Int64 WorkID
    {
        get
        {
            return Int64.Parse( this.Request.QueryString["WorkID"]);
        }
    }
    public Int64 OID
    {
        get
        {
            return Int64.Parse(this.Request.QueryString["OID"]);
        }
    }
    #endregion  Common variable .

    protected void Page_Load(object sender, EventArgs e)
    {
        #region  Output System url.
        //string s = "";
        //foreach (string key   in this.Request.QueryString.AllKeys)
        //{
        //    s += " , " + key + "=" + this.Request.QueryString[key];
        //}
        //this.Response.Write(s);
        //Log.DefaultLogWriteLineError(s);
        //return;
        #endregion  Output System url.

        try
        {
            switch (this.DoType)
            {
                case "SetHeJi":
                    string sql = "UPDATE ND101 SET HeJi=(SELECT SUM(XiaoJi) FROM ND101Dtl1 WHERE RefPK=" + this.OID + ") WHERE OID=" + this.OID;
                    BP.DA.DBAccess.RunSQL(sql);
                    // The total conversion to uppercase .
                    float hj = BP.DA.DBAccess.RunSQLReturnValFloat("SELECT HeJi FROM ND101 WHERE OID=" + this.OID,0);
                    sql = "UPDATE ND101 SET DaXie='" + BP.DA.DataType.ParseFloatToCash(hj) + "' WHERE OID=" + this.OID;
                    BP.DA.DBAccess.RunSQL(sql);
                    return;
                case "OutOK":
                    /* This is where the processing of your business processes .*/
                    return;
                default:
                    break;
            }
        }
        catch(Exception ex)
        {
            this.Response.Write("error:" + ex.StackTrace);
        }
    }
     
}