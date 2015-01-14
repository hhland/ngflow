using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BP.WF;
using BP.En;
using BP.Port;
using BP.Web.Controls;
using BP.Web;
using BP.Sys;
using CCFlow.WF.Admin.UC;

namespace CCFlow.WF.Admin
{
    public partial class WF_Admin_listen : BP.Web.WebPage
    {
        public int FK_Node
        {
            get
            {
                return int.Parse(this.Request.QueryString["FK_Node"]);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            switch (this.DoType)
            {
                case "New":
                    this.BindNew();
                    break;
                default:
                    this.BindList();
                    break;
            }
        }

        public void BindNew()
        {
            Listen li = new Listen();
            if (this.RefOID != 0)
            {
                li.OID = this.RefOID;
                li.Retrieve();
            }

            BP.WF.Node nd = new BP.WF.Node(this.FK_Node);

            this.Pub1.AddTable("class='Table' cellSpacing='1' cellPadding='1' border='1' style='width:100%'");
            this.Pub1.AddTR();
            this.Pub1.AddTD("class='GroupTitle'", " Setting listen :" + nd.Name + " - <a href='Listen.aspx?FK_Node=" + this.FK_Node + "' > Listen List </a>");
            this.Pub1.AddTR();
            this.Pub1.AddTD("class='GroupTitle'", " Select the node you want to listen （ You can select multiple ）");
            this.Pub1.AddTREnd();
            
            this.Pub1.AddTR();
            this.Pub1.AddTDBegin();

            BP.WF.Nodes nds = new BP.WF.Nodes(nd.FK_Flow);

            foreach (BP.WF.Node en in nds)
            {
                if (en.NodeID == this.FK_Node)
                    continue;

                CheckBox cb = new CheckBox();
                cb.Text = " Step :" + en.Step + " - " + en.Name;
                cb.ID = "CB_" + en.NodeID;

                cb.Checked = li.Nodes.Contains("@" + en.NodeID);
                this.Pub1.Add(cb);
                this.Pub1.AddBR();
            }

            this.Pub1.AddTDEnd();
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD("class='GroupTitle'", " Set Title ( Maximum length 250 Characters , Variables can contain fields to @ Beginning )");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();

            TextBox tb = new TextBox();
            tb.ID = "TB_Title";
            tb.Columns = 70;
            tb.Style.Add("width", "99%");
            tb.Text = li.Title;

            this.Pub1.AddTDBegin();
            this.Pub1.Add(tb);
            this.Pub1.AddBR();
            this.Pub1.Add(" Such as : You initiate work @Title Has been @WebUser.Name Deal with .");
            this.Pub1.AddTDEnd();
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD("class='GroupTitle'", " Content Information ( Length limit , Variables can contain fields to @ Beginning )");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTDBegin();

            tb = new TextBox();
            tb.TextMode = TextBoxMode.MultiLine;
            tb.ID = "TB_Doc";
            tb.Columns = 70;
            tb.Rows = 8;
            tb.Style.Add("width", "99%");
            tb.Text = li.Doc;

            this.Pub1.Add(tb);
            this.Pub1.AddBR();
            this.Pub1.Add(" Such as : Processing time @RDT, For more information, you can log in to view the processing system , Notice is hereby given .");
            this.Pub1.AddTDEnd();
            this.Pub1.AddTREnd();
            
            this.Pub1.AddTableEnd();

            this.Pub1.AddBR();
            this.Pub1.AddSpace(1);

            var btn = new LinkBtn(false, NamesOfBtn.Save, " Save ");
            btn.Click += new EventHandler(btn_Click);
            this.Pub1.Add(btn);
            this.Pub1.AddSpace(1);

            btn = new LinkBtn(false, NamesOfBtn.SaveAndNew, " Save and New ");
            btn.Click += new EventHandler(btn_Click);
            this.Pub1.Add(btn);
            this.Pub1.AddSpace(1);

            btn = new LinkBtn(false, NamesOfBtn.Delete, " Delete ");
            btn.Attributes["onclick"] = " return confirm(' You acknowledge delete it ?');";
            btn.Click += new EventHandler(btn_Del_Click);

            if (this.RefOID == 0)
                btn.Enabled = false;

            this.Pub1.Add(btn);
            this.Pub1.AddBR();
            this.Pub1.AddBR();

            this.Pub1.AddEasyUiPanelInfo(" Special Note ", " What kind of message channels ( SMS , Mail ) Sent , Is set by the user  [ Information Tips ] Determined .");
        }

        void btn_Click(object sender, EventArgs e)
        {
            Listen li = new Listen();

            if (this.RefOID != 0)
            {
                li.OID = this.RefOID;
                li.Retrieve();
            }

            li = this.Pub1.Copy(li) as Listen;
            li.OID = this.RefOID;

            BP.WF.Node nd = new BP.WF.Node(this.FK_Node);
            BP.WF.Nodes nds = new BP.WF.Nodes(nd.FK_Flow);
            string strs = "";

            foreach (BP.WF.Node en in nds)
            {
                if (en.NodeID == this.FK_Node)
                    continue;

                CheckBox cb = this.Pub1.GetCBByID("CB_" + en.NodeID);
                if (cb.Checked)
                    strs += "@" + en.NodeID;
            }

            li.Nodes = strs;
            li.FK_Node = this.FK_Node;

            if (li.OID == 0)
                li.Insert();
            else
                li.Update();

            var btn = (LinkBtn)sender;
            if (btn.ID == NamesOfBtn.Save)
                this.Response.Redirect("Listen.aspx?FK_Node=" + this.FK_Node + "&DoType=New&RefOID=" + li.OID, true);
            else
                this.Response.Redirect("Listen.aspx?FK_Node=" + this.FK_Node + "&DoType=New&RefOID=0", true);
        }

        void btn_Del_Click(object sender, EventArgs e)
        {
            Listen li = new Listen();

            if (this.RefOID != 0)
            {
                li.OID = this.RefOID;
                li.Delete();
            }

            this.Response.Redirect("Listen.aspx?FK_Node=" + this.FK_Node + "&DoType=New&RefOID=0", true);
        }

        public void BindList()
        {
            BP.WF.Node nd = new BP.WF.Node(this.FK_Node);

            Listens ens = new Listens();
            ens.Retrieve(ListenAttr.FK_Node, this.FK_Node);

            if (ens.Count == 0)
            {
                this.Response.Redirect("Listen.aspx?FK_Node=" + this.FK_Node + "&DoType=New", true);
                return;
            }

            this.Pub1.AddTable("class='Table' cellSpacing='1' cellPadding='1' border='1' style='width:100%'");
            this.Pub1.AddTR();
            this.Pub1.AddTD("class='GroupTitle' colspan='3'", " Setting listen :" + nd.Name + " - <a href='Listen.aspx?FK_Node=" + this.FK_Node + "&DoType=New' > New </a>");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD("class='GroupTitle'", " The current node ");
            this.Pub1.AddTD("class='GroupTitle'", " Listen node ");
            this.Pub1.AddTD("class='GroupTitle'", " Operating ");
            this.Pub1.AddTREnd();

            foreach (Listen en in ens)
            {
                this.Pub1.AddTR();
                this.Pub1.AddTD(nd.Name);
                this.Pub1.AddTD(en.Nodes);
                this.Pub1.AddTD("<a href='Listen.aspx?FK_Node=" + this.FK_Node + "&DoType=New&RefOID=" + en.OID + "' class='easyui-linkbutton' data-options=\"iconCls:'icon-edit'\"> Editor </a>");
                this.Pub1.AddTREnd();
            }

            this.Pub1.AddTableEnd();
        }
    }
}
