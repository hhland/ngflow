using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.DA;
using BP.Sys;
using BP.Port;
using BP.En;
using BP.WF;
using BP.Web;

public partial class DataUser_AppCoder_FrmEventHandle : System.Web.UI.Page
{
    #region  Property 
    public string SID
    {
        get
        {
            return this.Request.QueryString["SID"];
        }
    }
    public string WebUserNo
    {
        get
        {
            return this.Request.QueryString["WebUserNo"];
        }
    }
    public string FK_MapData
    {
        get
        {
            return this.Request.QueryString["FK_MapData"];
        }
    }
    public string FK_Event
    {
        get
        {
            return this.Request.QueryString["FK_Event"];
        }
    }
    public int OID
    {
        get
        {
            return int.Parse(this.Request.QueryString["OID"]);
        }
    }
    #endregion  Property 

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            switch (this.FK_MapData)
            {
                case "ND101":
                    this.ND101();
                    break;
                default:
                    throw new Exception(" No (" + this.FK_MapData + ") Business logic .");
            }
        }
        catch (Exception ex)
        {
            this.OutInfoErrMsg(ex.Message);
        }
    }

    #region  Business Process Demo Case 
    /// <summary>
    ///  In the form ID=ND101 Event .
    /// </summary>
    public void ND101()
    {
        #region  When the form before saving .
        if (this.FK_Event == FrmEventList.FrmLoadBefore)
        {
            /*  When the form is loaded before . */
        }
        #endregion  When the form before saving .

        #region  When the form is loaded .
        if (this.FK_Event == FrmEventList.FrmLoadAfter)
        {
            /*  When the form is loaded . */
        }
        #endregion  When the form is loaded .

        #region  When the form before saving .
        if (this.FK_Event == FrmEventList.SaveBefore)
        {
            /*  When the form before saving . */

        }
        #endregion  When the form before saving .

        #region  When the form is saved .
        if (this.FK_Event == FrmEventList.SaveAfter)
        {
            throw new Exception("");
            /*  When the form is saved . */
            GEEntity en = new GEEntity(this.FK_MapData, this.OID);
            string rdt = en.GetValStrByKey("RDT");
            string fid = this.Request.QueryString["FID"];
            string rec = en.GetValStrByKey("Rec");

            // Access database Case .
            int result = BP.DA.DBAccess.RunSQL("DELETE WF_Emp WHERE 1=2 ");
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable("SELECT * FROM WF_Emp");
        }
        #endregion  When the form is saved .
    }
    #endregion  Business Process Demo Case 

    #region  Public Methods 
    public void OutMsg(string msg)
    {
        this.Response.Write(msg);
    }
    public void OutInfoErrMsg(string msg)
    {
        this.Response.Write("Error:" + msg);
    }
    #endregion  Public Methods 

}