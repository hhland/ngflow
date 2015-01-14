using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Sys;
using BP.En;
using BP.Web.Controls;
using BP.DA;
using BP.Web;
using BP;
using BP.WF;

namespace CCFlow.WF.MapDef
{
    public partial class SlnDtl : System.Web.UI.Page
    {
        #region  Property .
        public string FK_MapData
        {
            get
            {
                return this.Request.QueryString["FK_MapData"];
            }
        }
        public string FK_Node
        {
            get
            {
                return this.Request.QueryString["FK_Node"];
            }
        }
        public string KeyOfEn
        {
            get
            {
                return this.Request.QueryString["KeyOfEn"];
            }
        }
        public string Ath
        {
            get
            {
                return this.Request.QueryString["Ath"];
            }
        }
        #endregion  Property .
         
        protected void Page_Load(object sender, EventArgs e)
        {
            BP.Sys.MapDtls ens = new BP.Sys.MapDtls();
            ens.Retrieve(MapDtlAttr.FK_MapData, this.FK_MapData);

            this.Pub1.AddTable("width='100%'");
            this.Pub1.AddCaptionLeft(" Form permissions list .");
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("Idx");
            this.Pub1.AddTDTitle(" Serial number ");
            this.Pub1.AddTDTitle(" Name ");
            this.Pub1.AddTDTitle(" Editor ");
            this.Pub1.AddTDTitle(" Delete ");
            this.Pub1.AddTREnd();

            int idx = 0;
            foreach (BP.Sys.MapDtl item in ens)
            {
                if (item.FK_Node != 0)
                    continue;

                idx++;
                this.Pub1.AddTR();
                this.Pub1.AddTDIdx(idx);
                this.Pub1.AddTD(item.No);
                this.Pub1.AddTD(item.Name);
                this.Pub1.AddTD("<a href=\"javascript:Edit('" + this.FK_Node + "','" + this.FK_MapData + "','" + item.No + "')\"> Editor </a>");

                MapDtl en = new MapDtl();
                if (en.RetrieveFromDBSources() == 0)
                    this.Pub1.AddTD();
                else
                    this.Pub1.AddTD("<a href=\"javascript:Delete('" + this.FK_Node + "','" + this.FK_MapData + "','" + item.No + "')\"> Delete </a>");

                this.Pub1.AddTREnd();
            }
            this.Pub1.AddTableEnd();
        }
    }
}