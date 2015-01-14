using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Sys;
using BP.Web.Controls;
using BP.WF.XML;

namespace CCFlow.WF.Admin
{
    public partial class ActionEvent : BP.Web.WebPage
    {
        #region  Property 

        public string Event
        {
            get
            {
                return this.Request.QueryString["Event"];
            }
        }

        public string NodeID
        {
            get
            {
                return this.Request.QueryString["NodeID"];
            }
        }

        public string FK_MapData
        {
            get
            {
                return "ND" + this.Request.QueryString["NodeID"];
            }
        }

        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            FrmEvents ndevs = new FrmEvents();
            ndevs.Retrieve(FrmEventAttr.FK_MapData, this.FK_MapData);

            FrmEvent mynde = ndevs.GetEntityByKey(FrmEventAttr.FK_Event, this.Event) as FrmEvent;
            if (mynde == null)
            {
                mynde = new FrmEvent();
                mynde.FK_Event = this.Event;
            }

            this.Pub1.AddTable("class='Table' cellspacing='1' cellpadding='1' border='1' style='width:100%'");

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Content Type :");
            DDL ddl = new DDL();
            ddl.BindSysEnum("EventDoType");
            ddl.ID = "DDL_EventDoType";
            ddl.SetSelectItem((int)mynde.HisDoType);
            this.Pub1.AddTD(ddl);
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTDBegin("colspan=2");
            this.Pub1.Add("&nbsp; Content to be executed <br>");
            TextBox tb = new TextBox();
            tb.ID = "TB_Doc";
            tb.Columns = 50;
            tb.Style.Add("width", "99%");
            tb.TextMode = TextBoxMode.MultiLine;
            tb.Rows = 4;
            tb.Text = mynde.DoDoc;
            this.Pub1.Add(tb);
            this.Pub1.AddTDEnd();
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTDBegin("colspan=2");
            tb = new TextBox();
            tb.ID = "TB_MsgOK";
            tb.Style.Add("width", "99%");
            tb.Text = mynde.MsgOKString;
            this.Pub1.Add(" Successful implementation of information tips ( May be empty )<br>");
            this.Pub1.Add(tb);
            this.Pub1.AddTDEnd();
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTDBegin("colspan=2");
            this.Pub1.Add(" Execution failed message alert ( May be empty )<br>");
            tb = new TextBox();
            tb.ID = "TB_MsgErr";
            tb.Style.Add("width", "99%");
            tb.Text = mynde.MsgErrorString;
            this.Pub1.Add(tb);
            this.Pub1.AddTDEnd();
            this.Pub1.AddTREnd();
            this.Pub1.AddTableEnd();
            Pub1.AddBR();
            Pub1.AddSpace(1);

            var btn = new LinkBtn(false, NamesOfBtn.Save, " Save ");
            btn.Click += new EventHandler(btn_Click);
            Pub1.Add(btn);

            if (!string.IsNullOrWhiteSpace(this.MyPK))
            {
                Pub1.AddSpace(1);
                Pub1.Add(
                    string.Format(
                        "<a href='javascript:void(0)' onclick=\"DoDel('{0}','{1}')\" class='easyui-linkbutton' data-options=\"iconCls:'icon-delete'\"> Delete </a>",
                        NodeID, Event));
            }
        }

        void btn_Click(object sender, EventArgs e)
        {
            FrmEvent fe = new FrmEvent();
            fe.MyPK = this.FK_MapData + "_" + this.Event;
            fe.RetrieveFromDBSources();

            string doc = this.Pub1.GetTextBoxByID("TB_Doc").Text.Trim();

            fe = (FrmEvent)this.Pub1.Copy(fe);
            fe.MyPK = this.FK_MapData + "_" + this.Event;
            fe.DoDoc = doc;
            fe.FK_Event = this.Event;
            fe.FK_MapData = this.FK_MapData;
            fe.HisDoType = (EventDoType)this.Pub1.GetDDLByID("DDL_EventDoType").SelectedItemIntVal;
            fe.MsgOKString = this.Pub1.GetTextBoxByID("TB_MsgOK").Text;
            fe.MsgErrorString = this.Pub1.GetTextBoxByID("TB_MsgErr").Text;

            fe.Save();

            this.Response.Redirect("ActionEvent.aspx?NodeID=" + this.NodeID + "&MyPK=" + fe.MyPK + "&Event=" + this.Event + "&tk=" + new Random().NextDouble(), true);
        }
    }
}