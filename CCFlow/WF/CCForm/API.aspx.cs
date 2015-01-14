using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.En;
using BP.DA;
using BP.Sys;
using BP.Web;

namespace CCFlow.WF.CCForm
{
    public partial class API : BP.Web.WebPage
    {
        /// <summary>
        /// ccform 的api.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (WebUser.NoOfRel == null)
            {
                this.Pub1.AddFieldSet("ERR"," User login information is lost .");
                return;
            }

            string fk_mapdata = this.Request.QueryString["FK_MapData"];
            string oid = this.Request.QueryString["OID"];

            MapData md = new MapData();
            md.No = fk_mapdata;
            if (md.RetrieveFromDBSources() == 0)
                this.Pub1.AddFieldSetRed(" Error "," Form number error :"+md.No);
            if (md.HisFrmType != FrmType.Url)
            {
                /* Processing the form is loaded before the event .*/
               GEEntity entity=md.HisGEEn;
               entity.PKVal = oid;
               entity.RetrieveFromDBSources();
               entity.ResetDefaultVal();

               // Perform a load before filling .
               string msg = md.FrmEvents.DoEventNode(FrmEventList.FrmLoadBefore, entity);
               if (string.IsNullOrEmpty(msg) == false)
               {
                   this.Pub1.AddFieldSetRed(" Error "," Error "+msg);
                   return;
               }
            }

            switch(md.HisFrmType)
            {
                case FrmType.AspxFrm:
                    this.Response.Redirect("Frm.aspx?FK_MapData=" + fk_mapdata + "&OID=" + oid, true); 
                    break;
                case FrmType.Column4Frm:
                    this.Response.Redirect("FrmFix.aspx?FK_MapData=" + fk_mapdata + "&OID=" + oid, true);
                    break;
                case FrmType.SLFrm: // 
                    this.Response.Redirect("SLFrm.aspx?FK_MapData=" + fk_mapdata + "&OID=" + oid, true);
                    break;
                case FrmType.Url: //  If it is a hyperlink 
                    string url = md.PTable;
                    url = url.Replace("@OID", oid);
                    url = url.Replace("@PK", oid);
                    this.Response.Redirect(url, true);
                    break;
                default:
                    break;
            }

        }
    }
}