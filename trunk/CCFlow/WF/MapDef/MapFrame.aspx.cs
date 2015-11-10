using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Sys;
using BP.En;
using BP.Web.Controls;
using BP.DA;
using BP.Web;
namespace CCFlow.WF.MapDef
{
    public partial class WF_MapDef_MapFrame : BP.Web.WebPage
    {
        #region  Property 
        public new string DoType
        {
            get
            {
                return this.Request.QueryString["DoType"];
            }
        }
        public string FK_MapData
        {
            get
            {
                return this.Request.QueryString["FK_MapData"];
            }
        }
        public string FK_MapFrame
        {
            get
            {
                return this.Request.QueryString["FK_MapFrame"];
            }
        }
        #endregion  Property 

        protected void Page_Load(object sender, EventArgs e)
        {
            MapData md = new MapData(this.FK_MapData);
            this.Title = md.Name + " - " + " Design Framework ";
            switch (this.DoType)
            {
                case "DtlList":
                    BindList(md);
                    break;
                case "New":
                    int num = BP.DA.DBAccess.RunSQLReturnValInt("SELECT COUNT(*) FROM Sys_MapFrame WHERE FK_MapData='" + this.FK_MapData + "'") + 1;
                    MapFrame dtl1 = new MapFrame();
                    dtl1.Name =   " Frame "  + num;
                    dtl1.NoOfObj = "F" + num;
                    BindEdit(md, dtl1);
                    break;
                case "Edit":
                    MapFrame dtl = new MapFrame();
                    if (this.FK_MapFrame == null)
                    {
                        dtl.NoOfObj = "Frm";
                    }
                    else
                    {
                        dtl.MyPK = this.FK_MapFrame;
                        dtl.Retrieve();
                    }
                    BindEdit(md, dtl);
                    break;
                default:
                    throw new Exception("er" + this.DoType);
            }
        }
        public void BindList(MapData md)
        {
            MapFrames dtls = new MapFrames(md.No);
            if (dtls.Count == 0)
            {
                this.Response.Redirect("MapFrame.aspx?DoType=New&FK_MapData=" + this.FK_MapData + "&sd=sd", true);
                return;
            }

            if (dtls.Count == 1)
            {
                MapFrame d = (MapFrame)dtls[0];
                this.Response.Redirect("MapFrame.aspx?DoType=Edit&FK_MapData=" + this.FK_MapData + "&FK_MapFrame=" + d.MyPK, true);
                return;
            }

            this.Pub1.AddTable();
            this.Pub1.AddCaptionLeft("<a href='MapDef.aspx?MyPK=" + this.MyPK + "'> Return :" + md.Name + "</a> - <a href='MapFrame.aspx?DoType=New&FK_MapData=" + this.FK_MapData + "&sd=sd'><img src='../Img/Btn/New.gif' border=0/> New </a>");
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("IDX");
            this.Pub1.AddTDTitle(" Serial number ");
            this.Pub1.AddTDTitle(" Name ");
            this.Pub1.AddTDTitle(" Operating ");
            this.Pub1.AddTREnd();

            TB tb = new TB();
            int i = 0;
            foreach (MapFrame dtl in dtls)
            {
                i++;
                this.Pub1.AddTR();
                this.Pub1.AddTDIdx(i);
                this.Pub1.AddTD(dtl.MyPK);
                this.Pub1.AddTD(dtl.Name);
                this.Pub1.AddTD("<a href='MapFrame.aspx?FK_MapData=" + this.FK_MapData + "&DoType=Edit&FK_MapFrame=" + dtl.MyPK + "'> Editor </a>");
                this.Pub1.AddTREnd();
                continue;

                tb = new TB();
                tb.ID = "TB_No_" + dtl.MyPK;
                tb.Text = dtl.MyPK;
                this.Pub1.AddTD(tb);

                tb = new TB();
                tb.ID = "TB_Name_" + dtl.MyPK;
                tb.Text = dtl.Name;
                this.Pub1.AddTD(tb);

                this.Pub1.AddTD("<a href='MapFrame.aspx?FK_MapData=" + this.FK_MapData + "&DoType=Edit&FK_MapFrame=" + dtl.MyPK + "'> Editor </a>");
                this.Pub1.AddTREnd();
            }

            //this.Pub1.AddTRSum();
            //Button btn = new Button();
            //btn.ID = "Btn_Save";
            //btn.Text = " Save ";
            //btn.Click += new EventHandler(btn_Click);
            //this.Pub1.AddTD("colspan=5", btn);
            //this.Pub1.AddTREnd();
            this.Pub1.AddTableEnd();
        }
        void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            try
            {
                switch (this.DoType)
                {
                    case "New":
                        MapFrame dtlN = new MapFrame();
                        dtlN = (MapFrame)this.Pub1.Copy(dtlN);
                        if (this.DoType == "New")
                        {
                            if (dtlN.IsExits)
                            {
                                this.Alert(" Existing number :" + dtlN.MyPK);
                                return;
                            }
                        }
                        dtlN.FK_MapData = this.FK_MapData;
                        dtlN.GroupID = 0;
                        dtlN.RowIdx = 0;
                        GroupFields gfs1 = new GroupFields(this.FK_MapData);
                        if (gfs1.Count == 1)
                        {
                            GroupField gf = (GroupField)gfs1[0];
                            dtlN.GroupID = gf.OID;
                        }
                        else
                        {
                            dtlN.GroupID = this.Pub1.GetDDLByID("DDL_GroupField").SelectedItemIntVal;
                        }

                        dtlN.Insert();

                        if (btn.ID.Contains("AndClose"))
                        {
                            this.WinClose();
                            return;
                        }
                        this.Response.Redirect("MapFrame.aspx?DoType=Edit&FK_MapFrame=" + dtlN.MyPK + "&FK_MapData=" + this.FK_MapData, true);
                        break;
                    case "Edit":
                        MapFrame dtl = new MapFrame(this.FK_MapFrame);
                        dtl = (MapFrame)this.Pub1.Copy(dtl);
                        if (this.DoType == "New")
                        {
                            if (dtl.IsExits)
                            {
                                this.Alert(" Existing number :"  + dtl.NoOfObj);
                                return;
                            }
                        }
                        dtl.FK_MapData = this.FK_MapData;
                        dtl.IsAutoSize = this.Pub1.GetRadioBtnByID("RB_IsAutoSize_1").Checked;

                        GroupFields gfs = new GroupFields(dtl.FK_MapData);
                        dtl.GroupID = this.Pub1.GetDDLByID("DDL_GroupField").SelectedItemIntVal;

                        if (this.DoType == "New")
                            dtl.Insert();
                        else
                            dtl.Update();

                        if (btn.ID.Contains("AndC"))
                        {
                            this.WinClose();
                            return;
                        }

                        this.Response.Redirect("MapFrame.aspx?DoType=Edit&FK_MapFrame=" + dtl.MyPK + "&FK_MapData=" + this.FK_MapData, true);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }
        void btn_Del_Click(object sender, EventArgs e)
        {
            try
            {
                MapFrame dtl = new MapFrame();
                dtl.MyPK = this.FK_MapFrame;
                dtl.Delete();
                this.WinClose();
                //this.Response.Redirect("MapFrame.aspx?DoType=DtlList&FK_MapData=" + this.FK_MapData, true);
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }
        void btn_New_Click(object sender, EventArgs e)
        {
            this.Response.Redirect("MapFrame.aspx?DoType=New&FK_MapData=" + this.FK_MapData, true);

            //MapData md = new MapData(this.FK_MapData);
            //MapFrames dtls = new MapFrames(md.No);
            //MapFrame d = (MapFrame)dtls[0];
            //try
            //{
            //    MapFrame dtl = new MapFrame();
            //    dtl.No = this.FK_MapFrame;
            //    dtl.Delete();
            //    this.WinClose();
            //}
            //catch (Exception ex)
            //{
            //    this.Alert(ex.Message);
            //}
        }
        void btn_Go_Click(object sender, EventArgs e)
        {
            MapFrame dtl = new MapFrame(this.FK_MapFrame);
            //  dtl.IntMapAttrs();
            this.Response.Redirect("MapFrameDe.aspx?DoType=Edit&FK_MapData=" + this.FK_MapData + "&FK_MapFrame=" + this.FK_MapFrame, true);
        }

        public void BindEdit(MapData md, MapFrame dtl)
        {
            this.Pub1.AddTable();
            //  this.Pub1.AddCaptionLeftTX("<a href='MapDef.aspx?MyPK=" + md.No + "'>" + " Return " + ":" + md.Name + "</a> -  " + this.ToE("DtlTable", " From Table ") + ":（" + dtl.Name + "）");
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("ID");
            this.Pub1.AddTDTitle(" Project ");
            this.Pub1.AddTDTitle(" Collection ");
            this.Pub1.AddTDTitle(" Remark ");
            this.Pub1.AddTREnd();

            int idx = 1;
            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Serial number ");
            TB tb = new TB();
            tb.ID = "TB_" + MapFrameAttr.NoOfObj;
            tb.Text = dtl.NoOfObj;
            if (this.DoType == "Edit")
                tb.Enabled = false;
            this.Pub1.AddTD(tb);
            this.Pub1.AddTD();
            this.Pub1.AddTREnd();


            this.Pub1.AddTR1();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Description ");
            tb = new TB();
            tb.ID = "TB_Name";
            tb.Text = dtl.Name;
            tb.Columns = 50;
            this.Pub1.AddTD("colspan=2", tb);
            this.Pub1.AddTREnd();
            this.Pub1.AddTREnd();


            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Frame connection ");
            tb = new TB();
            tb.ID = "TB_URL";
            tb.Text = dtl.URL;
            tb.Columns = 50;
            this.Pub1.AddTD("colspan=2", tb);
            this.Pub1.AddTREnd();

            this.Pub1.AddTR1();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Frame Width ");
            tb = new TB();
            tb.ID = "TB_W";
            tb.Text = dtl.W;
            tb.ShowType = TBType.TB;
            this.Pub1.AddTD(tb);
            this.Pub1.AddTD();
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Frame height ");
            tb = new TB();
            tb.ID = "TB_H";
            tb.ShowType = TBType.TB;
            tb.Text = dtl.H;
            this.Pub1.AddTD(tb);
            this.Pub1.AddTD();
            this.Pub1.AddTREnd();

            this.Pub1.AddTR1();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTDBegin("colspan=3");

            RadioBtn rb = new RadioBtn();
            rb.Text = " Specifies the height of the frame width ";
            rb.ID = "RB_IsAutoSize_0";
            rb.GroupName = "s";
            if (dtl.IsAutoSize)
                rb.Checked = false;
            else
                rb.Checked = true;

            this.Pub1.Add(rb);


            rb = new RadioBtn();
            rb.Text = " Let adaptive frame size ";
            rb.ID = "RB_IsAutoSize_1";
            rb.GroupName = "s";

            if (dtl.IsAutoSize)
                rb.Checked = true;
            else
                rb.Checked = false;

            this.Pub1.Add(rb);
            this.Pub1.AddTDEnd();
            this.Pub1.AddTREnd();

            GroupFields gfs = new GroupFields(md.No);

            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD(" Displayed in the group ");
            DDL ddl = new DDL();
            ddl.ID = "DDL_GroupField";
            ddl.BindEntities(gfs, GroupFieldAttr.OID, GroupFieldAttr.Lab, false, AddAllLocation.None);
            ddl.SetSelectItem(dtl.GroupID);
            this.Pub1.AddTD("colspan=2", ddl);
            this.Pub1.AddTREnd();


            this.Pub1.AddTRSum();
            this.Pub1.AddTDBegin("colspan=4 align=center");

            Button btn = new Button();
            btn.ID = "Btn_Save";
            btn.CssClass = "Btn";
            btn.Text = "  Save  ";
            btn.Click += new EventHandler(btn_Click);
            this.Pub1.Add(btn);

            btn = new Button();
            btn.ID = "Btn_SaveAndClose";
            btn.CssClass = "Btn";
            btn.Text = "  Save and Close  ";
            btn.Click += new EventHandler(btn_Click);
            this.Pub1.Add(btn);

            if (this.FK_MapFrame != null)
            {
                btn = new Button();
                btn.ID = "Btn_Del";
                btn.CssClass = "Btn";
                btn.Text = " Delete "; // " Delete ";
                btn.Attributes["onclick"] = " return confirm(' You acknowledge that you ?');";
                btn.Click += new EventHandler(btn_Del_Click);
                this.Pub1.Add(btn);

                btn = new Button();
                btn.ID = "Btn_New";
                btn.CssClass = "Btn";
                btn.Text = " New "; // " Delete ";
                btn.Click += new EventHandler(btn_New_Click);
                this.Pub1.Add(btn);
            }

            this.Pub1.AddTDEnd();
            this.Pub1.AddTREnd();
            this.Pub1.AddTableEnd();
        }
    }
}