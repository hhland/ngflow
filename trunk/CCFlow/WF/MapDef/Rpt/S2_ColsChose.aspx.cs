using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.WF.Data;
using BP.Sys;

namespace CCFlow.WF.MapDef.Rpt
{

    public partial class ColsChose : BP.Web.PageBase
    {
        #region  Property .
        public string RptNo
        {
            get
            {
                return this.Request.QueryString["RptNo"];

            }
        }
        public string FK_MapData
        {
            get
            {
                return this.Request.QueryString["FK_MapData"];

            }
        }
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];

            }
        }
        #endregion  Property .

        protected void Page_Load(object sender, EventArgs e)
        {
            GroupFields gfs = new GroupFields(this.FK_Flow);
            MapAttrs mattrs = new MapAttrs(this.FK_MapData);
            MapAttrs mattrsOfRpt = new MapAttrs(this.RptNo);
            bool isBr = false;

            this.Pub2.AddTable("class='Table' border='1' cellspacing='0' cellpadding='0' style='width:100%'");

            foreach (GroupField gf in gfs)
            {
                this.Pub2.AddTR();
                this.Pub2.AddTDGroupTitle(gf.Lab);
                this.Pub2.AddTREnd();

                this.Pub2.AddTR();
                this.Pub2.AddTDBigDocBegain();

                this.Pub2.AddTable("class='Table' border='1' cellspacing='0' cellpadding='0' style='width:100%'");
                isBr = false;
                foreach (MapAttr attr in mattrs)
                {
                    if (attr.GroupID != gf.OID)
                        continue;

                    CheckBox cb = new CheckBox();
                    cb.ID = "CB_" + attr.KeyOfEn;
                    cb.Text = attr.Name + "(" + attr.KeyOfEn + ")";
                    cb.Checked = mattrsOfRpt.Contains(MapAttrAttr.KeyOfEn, attr.KeyOfEn);

                    if (isBr == false)
                        this.Pub2.AddTR();

                    this.Pub2.AddTD("style='width:50%'", cb);

                    if (isBr)
                        this.Pub2.AddTREnd();

                    isBr = !isBr;
                }

                if (isBr)
                {
                    Pub2.AddTD();
                    Pub2.AddTREnd();
                }

                this.Pub2.AddTableEnd();

                this.Pub2.AddTDEnd();
                this.Pub2.AddTREnd();
            }

            var dictAttrs = new Dictionary<string, List<MapAttr>>();
            dictAttrs.Add(" System field ", new List<MapAttr>());
            dictAttrs.Add(" Enum field ", new List<MapAttr>());
            dictAttrs.Add(" Foreign key field ", new List<MapAttr>());
            dictAttrs.Add(" Ordinary field ", new List<MapAttr>());
            var sysFields =BP.WF.Glo.FlowFields;

            // The property group : System , Enumerate , Foreign key , General 
            foreach (MapAttr attr in mattrs)
            {
                if (gfs.Contains(attr.GroupID))
                    continue;

                if (sysFields.Contains(attr.KeyOfEn))
                {
                    dictAttrs[" System field "].Add(attr);
                }
                else if (attr.HisAttr.IsEnum)
                {
                    dictAttrs[" Enum field "].Add(attr);
                }
                else if (attr.HisAttr.IsFK)
                {
                    dictAttrs[" Foreign key field "].Add(attr);
                }
                else
                {
                    dictAttrs[" Ordinary field "].Add(attr);
                }
            }

            foreach (var de in dictAttrs)
            {
                if (de.Value.Count == 0) continue;

                this.Pub2.AddTR();
                this.Pub2.AddTDGroupTitle(de.Key);
                this.Pub2.AddTREnd();

                this.Pub2.AddTR();
                this.Pub2.AddTDBigDocBegain();

                this.Pub2.AddTable("class='Table' border='1' cellspacing='0' cellpadding='0' style='width:100%'");

                isBr = false;

                foreach (var attr in de.Value)
                {
                    CheckBox cb = new CheckBox();
                    cb.ID = "CB_" + attr.KeyOfEn;
                    cb.Text = attr.Name + "(" + attr.KeyOfEn + ")";
                    cb.Checked = mattrsOfRpt.Contains(MapAttrAttr.KeyOfEn, attr.KeyOfEn);

                    switch (attr.KeyOfEn)
                    {
                        case NDXRptBaseAttr.Title:
                        case NDXRptBaseAttr.MyNum:
                        case NDXRptBaseAttr.OID:
                        case NDXRptBaseAttr.WFSta:
                            cb.Checked = true;
                            cb.Enabled = false;
                            break;
                        case NDXRptBaseAttr.WFState:
                            continue;
                        default:
                            break;
                    }

                    if (isBr == false)
                        this.Pub2.AddTR();

                    this.Pub2.AddTD("style='width:50%'", cb);

                    if (isBr)
                        this.Pub2.AddTREnd();

                    isBr = !isBr;
                }

                if (isBr)
                {
                    Pub2.AddTD();
                    Pub2.AddTREnd();
                }

                this.Pub2.AddTableEnd();

                this.Pub2.AddTDEnd();
                this.Pub2.AddTREnd();
            }

            this.Pub2.AddTableEnd();
        }

        //void btn_SelectColumns_Click(object sender, EventArgs e)
        //{
        //    MapAttrs mattrs = new MapAttrs(this.FK_MapData);
        //    mattrs.Delete(MapAttrAttr.FK_MapData, this.RptNo);

        //    MapData md = new MapData(this.FK_MapData);
        //    foreach (MapAttr attr in mattrs)
        //    {
        //        CheckBox cb = this.Pub2.GetCBByID("CB_" + attr.KeyOfEn);
        //        if (cb == null)
        //            continue;

        //        if (cb.Checked == false)
        //            continue;

        //        attr.FK_MapData = this.RptNo;
        //        attr.Insert();

        //        switch (attr.KeyOfEn)
        //        {
        //            case NDXRptBaseAttr.Title:
        //                attr.IDX = -1;
        //                attr.Update();
        //                break;
        //            case NDXRptBaseAttr.MyNum:
        //            case NDXRptBaseAttr.OID:
        //                attr.IDX = 1000;
        //                attr.Update();
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    this.Response.Redirect("ColsLabel.aspx?FK_MapData=" + this.FK_MapData + "&FK_Flow=" + this.FK_Flow + "&RptNo=" + this.RptNo, true);
        //}

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            Save();

            Response.Redirect("S2_ColsChose.aspx?FK_MapData=" + this.FK_MapData + "&RptNo=" + this.RptNo + "&FK_Flow=" + this.FK_Flow, true);
        }

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            this.WinClose();
        }

        protected void Btn_SaveAndNext1_Click(object sender, EventArgs e)
        {
            Save();

            Response.Redirect("S3_ColsLabel.aspx?FK_MapData=" + this.FK_MapData + "&RptNo=" + this.RptNo + "&FK_Flow=" + this.FK_Flow, true);
        }

        private void Save()
        {
            MapAttrs mattrs = new MapAttrs(this.FK_MapData);
            mattrs.Delete(MapAttrAttr.FK_MapData, this.RptNo);

            MapData md = new MapData(this.FK_MapData);
            foreach (MapAttr attr in mattrs)
            {
                CheckBox cb = this.Pub2.GetCBByID("CB_" + attr.KeyOfEn);
                if (cb == null)
                    continue;
                if (cb.Checked == false)
                    continue;

                attr.FK_MapData = this.RptNo;
                attr.Insert();
            }
        }
    }
}