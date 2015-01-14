using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.XML;
using BP.Sys;

namespace CCFlow.WF.MapDef
{
    public partial class RegularExpressionTemplete : BP.Web.WebPage
    {
        #region  Property 
        public string FK_MapData
        {
            get
            {
                return this.Request.QueryString["FK_MapData"];
            }
        }
        public string KeyOfEn
        {
            get
            {
                return this.Request.QueryString["KeyOfEn"];
            }
        }
        public string ForCtrl
        {
            get
            {
                return this.Request.QueryString["ForCtrl"];
            }
        }
        #endregion

        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Request.QueryString["REID"] != null)
            {
                BP.XML.RegularExpressionDtls reDtls = new RegularExpressionDtls();
                reDtls.RetrieveAll();

                // Delete the existing logic .
                BP.Sys.MapExts exts = new BP.Sys.MapExts();
                exts.Delete(MapExtAttr.AttrOfOper, this.KeyOfEn, 
                    MapExtAttr.ExtType, BP.Sys.MapExtXmlList.RegularExpression);

                //  Start loading .
                foreach (RegularExpressionDtl dtl in reDtls)
                {
                    if (dtl.ItemNo != this.Request.QueryString["REID"])
                        continue;

                    BP.Sys.MapExt ext = new BP.Sys.MapExt();
                    ext.MyPK = this.FK_MapData + "_" + this.KeyOfEn + "_" + MapExtXmlList.RegularExpression + "_" + dtl.ForEvent;
                    ext.FK_MapData = this.FK_MapData;
                    ext.AttrOfOper = this.KeyOfEn;
                    ext.Doc = dtl.Exp; // Expression formula .
                    ext.Tag = dtl.ForEvent; // Time .
                    ext.Tag1 = dtl.Msg;  // News 
                    ext.ExtType = MapExtXmlList.RegularExpression; //  Expression formula  .
                    ext.Insert();
                }
                //this.WinClose("1"); // Close and return a value .
                return;
            }

            BP.XML.RegularExpressions res = new RegularExpressions();
            res.RetrieveAll();

            this.Pub1.AddH3(" Event Templates - Click on the name of the choice it .");
            this.Pub1.AddHR();

            this.Pub1.AddUL();
            foreach (RegularExpression item in res)
            {
                this.Pub1.AddLi("<a href=\"javascript:DoIt('" + this.FK_MapData + "','" + this.KeyOfEn 
                    + "','" + this.ForCtrl + "','" + item.No + "','" + item.Name
                    + "');\" >" + item.Name + "</a> - " + item.Note);
            }
            this.Pub1.AddULEnd();
        }
    }
}