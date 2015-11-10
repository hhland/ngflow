using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Sys;
using BP.Web.Controls;
using BP.WF;
using BP.WF.XML;

namespace CCFlow.WF.Admin
{
    public partial class ActionPush2Curr : BP.Web.WebPage
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
            this.Pub1.AddTD(" Control mode ");
            var ddl = new DDL();
            ddl.BindSysEnum("MsgCtrl");
            ddl.ID = "DDL_" + FrmEventAttr.MsgCtrl;
            ddl.SetSelectItem((int)mynde.MsgCtrl);
            this.Pub1.AddTD(ddl);
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD("");
            CheckBox cb = new CheckBox();
            cb.ID = "CB_" + FrmEventAttr.MsgMailEnable;
            cb.Text = " Whether to enable e-mail notification ?";
            cb.Checked = mynde.MsgMailEnable;
            this.Pub1.AddTD(cb);
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Mail headline templates ");
            var tb = new TextBox();
            tb.ID = "TB_" + FrmEventAttr.MailTitle;
            tb.Text = mynde.MailTitle_Real;
            tb.Style.Add("width", "99%");
            this.Pub1.AddTD(tb);
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Mail content templates :");
            tb = new TextBox();
            tb.ID = "TB_" + FrmEventAttr.MailDoc;
            tb.Text = mynde.MailDoc_Real;
            tb.TextMode = TextBoxMode.MultiLine;
            tb.Columns = 50;
            tb.Rows = 4;
            tb.Style.Add("width", "99%");
            this.Pub1.AddTD(tb);
            this.Pub1.AddTREnd();

            // SMS ....
            this.Pub1.AddTR();
            this.Pub1.AddTD(" Default : Not enabled ");
            cb = new CheckBox();
            cb.ID = "CB_" + FrmEventAttr.SMSEnable;
            cb.Text = " Whether SMS notification is enabled ?";
            cb.Checked = mynde.SMSEnable;
            this.Pub1.AddTD(cb);
            this.Pub1.AddTREnd();

            tb = new TextBox();
            tb.ID = "TB_" + FrmEventAttr.SMSDoc;
            tb.Text = mynde.SMSDoc_Real;
            tb.Style.Add("width", "99%");
            tb.Rows = 2;
            this.Pub1.AddTR();
            if (string.IsNullOrEmpty(tb.Text) == true)
                this.Pub1.AddTD(" SMS templates :");
            else
                this.Pub1.AddTD(" SMS templates ");

            this.Pub1.AddTD(tb);
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Default : Enable ");
            cb = new CheckBox();
            cb.ID = "CB_" + FrmEventAttr.MobilePushEnable;
            cb.Text = " Whether to enable mobile applications , Tablet Application Information Push ?";
            cb.Checked = mynde.MobilePushEnable;
            this.Pub1.AddTD(cb);
            this.Pub1.AddTREnd();
            this.Pub1.AddTableEnd();

            Pub1.AddBR();
            Pub1.AddSpace(1);

            var btn = new LinkBtn(false, NamesOfBtn.Save, " Save ");
            btn.Click += new EventHandler(btn_Click);
            Pub1.Add(btn);
        }

        void btn_Click(object sender, EventArgs e)
        {
            FrmEvent fe = new FrmEvent();
            fe.MyPK = this.FK_MapData + "_" + this.Event;
            int eff=fe.RetrieveFromDBSources();
            fe = (FrmEvent)this.Pub1.Copy(fe);
            if (eff == 0)
            {
                fe.MyPK = this.FK_MapData + "_" + this.Event;
                fe.FK_MapData = this.FK_MapData;
                fe.FK_Event = this.Event;
                eff +=fe.Insert();
            }
            else {
                eff+=fe.Update();
            }
           
            
            
            //var pm = new PushMsg();
            //pm.Retrieve(PushMsgAttr.FK_Event, this.Event, PushMsgAttr.FK_Node, this.NodeID);

            this.Response.Redirect("ActionPush2Curr.aspx?NodeID=" + this.NodeID + "&MyPK=" + fe.MyPK + "&Event=" + this.Event + "&tk=" + new Random().NextDouble(), true);
        }
    }
}